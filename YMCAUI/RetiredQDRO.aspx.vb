'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	RetiredQDRO.aspx.vb
' Author Name		:	Dilip Patada	
' Employee ID		:	37486
' Email			    :	dilip.patada@3i-infotech.com
' Contact No		:	080-39876746
' Creation Time	    :	13/6/2008 
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>

'
' Changed by			:	
' Changed on			:	
' Change Description	:	
'*****************************************************************************************************************************
' Changed BY        Date                Description
'*****************************************************************************************************************************
' Paramesh K.       Oct 30th 2008       Allowing the system to split even the participant has 
'                                       negative balance                    
' Dilip             Dec 22th 1008       Added by dilip on 22-12-2008 This function will check
'                                       the manditory fields values entered or not for ExistingParticipent and display message.
' Dilip             Dec 22th 1008       bug id 661
'Neeraj Singh       12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar      17/Feb/2010         Restrict Data Archived Participants To proceed ahead from Find list.
'Shashi Shekhar     2010-03-11           Allow the user to access certain functions only for Retired participants (status RD) even if they are archived (Ref:Handling usability issue of Data Archive)
'Neeraj Singh       06/jun/2010         Enhancement for .net 4.0
'Neeraj Singh       07/jun/2010         review changes done
'Neeraj             02/12/2010 :        gemini 699
'Shashi Shekhar     10-Dec-2010:        For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Priya				10-Dec-2010:		Changes made as sate n country fill up with javascript in user control
'Shashi Shekhar     10 Feb 2011         For YRS 5.0-1236 : Need ability to freeze/lock account
'Shashi Shekhar     14 Feb 2011         For BT-750 While QDRO split message showing wrong.
'Shashi             03 Mar 2011         Replacing Header formating with user control (YRS 5.0-450 )
'Harshala Trumukhe  23 Mar 2012         BT ID : 1002 - Error in Retired QDRO Settlement.
'Harshala Trumukhe  11 Jun 2012         BT ID : 981 - YRS 5.0-1525 - annuity adjustment values are incorrect. 
'Bhavna Shrivastav  18-May-2012    YRS 5.0-1470: Link to Address Edit program from Person Maintenance 

'Harshala Trumukhe  11 Jun 2012         BT ID : 981 - YRS 5.0-1525 - annuity adjustment values are incorrect. 
'Anudeep            13.Apr 2013         BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            20.jun 2013         BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            21.jun 2013         BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            10-Jul-2013         BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            18-jul-2013         BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
'Sanjay R.          2013.08.05          YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Shashank P			2013.10.16			BT-2175 : Non retired Qdro split will throws error if benefeciary name is more than person table
'Shashank P         2014.04.29          BT-2445\YRS 5.0-633 - Changes to address 3% annty option and Soc Sec leveling 
'Anudeep            2015.05.05          BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite
'Manthan Rajguru    2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod Pokale      2015.10.13          YRS-AT-2588: implement some basic telephone number validation Rules
'Manthan Rajguru    2015.10.21          YRS-AT-2182: limit effective date for address updates -Do not allow address updates with an effective date in the future. 
'Manthan Rajguru    2016.08.16          YRS-AT-2482: Add gender and marital status fields to QDRO beneficiary add screen (TrackIT 22117)
'Chandra sekar      2016.08.22          YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
'Manthan Rajguru    2016.09.27          YRS-AT-2482: Add gender and marital status fields to QDRO beneficiary add screen (TrackIT 22117)
'Pramod Pokale      2017.01.16          YRS-AT-3299 -  YRS enh:improve usability of QDRO split screens(Retired) (TrackIT 28050) 
'Pooja K            2019.28.02          YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'*****************************************************************************************************************************

Imports YMCARET
Imports System.Data.SqlClient
Imports System.IO 'Chandra sekar| 2016.08.22 | YRS-AT-3081 For using the HTML String Builder Class
Imports Microsoft.Practices.EnterpriseLibrary.Logging

Public Class RetiredQDRO
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("RetiredQDRO.aspx")
    'End issue id YRS 5.0-940


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'START: PPP | 01/24/2017 | YRS-AT-3299 | Replaced BENEFICIARY_ID with recipientIndexPersID 
    '    'Start - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Defining constant
    '#Region "Global Declaration"
    '    Const BENEFICIARY_ID As Integer = 1
    '#End Region
    '    'End - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Defining constant
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Replaced BENEFICIARY_ID with recipientIndexPersID 

    Protected WithEvents PlaceHolderACHDebitImportProcess As System.Web.UI.WebControls.PlaceHolder
    'Protected WithEvents DataGridRetireeList As System.Web.UI.WebControls.DataGrid 'MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    'START: PPP | 01/24/2017 | YRS-AT-3299 | Controls were part of internal find screen which is not in use
    'Protected WithEvents LabelFundNoList As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxFundNoList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelSSNoList As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxSSNoList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelLastNameList As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxLastNameList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelFirstNameList As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxFirstNameList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxCityList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxStateList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ButtonFindList As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Controls were part of internal find screen which is not in use
    Protected WithEvents TextboxSpouseCountry As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxAmountWorkSheet As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPercentageWorkSheet As System.Web.UI.WebControls.TextBox
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents DataGridWorkSheet As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridPartTotals As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonSplit As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridRecipientAnnuitiesBalance As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonReset As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonDocumentOK As System.Web.UI.WebControls.Button 'PPP | 01/24/2017 | YRS-AT-3299 | Old button
    Protected WithEvents Radiobuttonlist1 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LabelHBeneficiarySSno As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownlistBeneficiarySSNo As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents RadioButtonListSplitAmtType As System.Web.UI.WebControls.RadioButtonList 'PPP | 01/24/2017 | YRS-AT-3299 | Old control
    Protected WithEvents LabelHAmount As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHPercentage As System.Web.UI.WebControls.Label
    '  Protected WithEvents RadioButtonListPlanType As System.Web.UI.WebControls.RadioButtonList 'Commented by Chandra sekar| 2016.08.22 | YRS-AT-3081 | Adding Link button for Split types
    Protected WithEvents DatagridParticipantsBalance As System.Web.UI.WebControls.DataGrid
    'START: PPP | 01/16/2017 | YRS-AT-3299 | Controls were not in use
    'Protected WithEvents PopcalendarRecDate As RJS.Web.WebControl.PopCalendar
    'Protected WithEvents PopcalendarRecDate2 As RJS.Web.WebControl.PopCalendar
    'END: PPP | 01/16/2017 | YRS-AT-3299 | Controls were not in use
    Protected WithEvents QdroRetiredTabStrip As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents ListMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents DatagridBenificiaryList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridSummaryBalList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataListParticipant As System.Web.UI.WebControls.DataList
    Protected WithEvents DatalistBeneficiary As System.Web.UI.WebControls.DataList
    Protected WithEvents DatagridBeneficiarySummaryBalList As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents ButtonDocumentSave As System.Web.UI.WebControls.Button 'PPP | 01/24/2017 | YRS-AT-3299 | Old button 
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    'Protected WithEvents LabelNoDataFound As System.Web.UI.WebControls.Label 'PPP | 01/24/2017 | YRS-AT-3299 | Control was part of internal find screen which is not in use
    'START: PPP | 01/20/2017 | YRS-AT-3299 | Old controls comments and new controls added in its place
    ' START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    'Protected WithEvents lblCurrentSplitOptions As System.Web.UI.WebControls.Label
    'Protected WithEvents lnkButtonBothPlans As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents lnkButtonRetirementPlan As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents lnkButtonSavingsPlan As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents lblBothPlans As System.Web.UI.WebControls.Label
    'Protected WithEvents lblRetirementPlan As System.Web.UI.WebControls.Label
    'Protected WithEvents lblSavingsPlan As System.Web.UI.WebControls.Label
    'Protected WithEvents lblBothPlansPerOrAmt As System.Web.UI.WebControls.Label
    'Protected WithEvents lblBeneficiarySummaryBalList As System.Web.UI.WebControls.Label
    ' END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    Protected WithEvents lblPlanInProgressHeader As System.Web.UI.WebControls.Label
    Protected WithEvents trPlanInProgressHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPlanInProgressEmptyRow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trAmountPercentage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents RadioButtonListSplitAmtType_Amount As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents RadioButtonListSplitAmtType_Percentage As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents spanBothPlans As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents lnbBothPlans As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblBothPlans As System.Web.UI.WebControls.Label
    Protected WithEvents lnbRetirement As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblRetirement As System.Web.UI.WebControls.Label
    Protected WithEvents lnbSavings As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblSavings As System.Web.UI.WebControls.Label
    Protected WithEvents hdnSelectedPlanType As System.Web.UI.WebControls.HiddenField
    Protected WithEvents btnPrevious As System.Web.UI.WebControls.Button
    Protected WithEvents btnNext As System.Web.UI.WebControls.Button
    Protected WithEvents btnClose As System.Web.UI.WebControls.Button
    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
    Protected WithEvents btnFinalOK As System.Web.UI.WebControls.Button
    Protected WithEvents HiddenFieldDirty As System.Web.UI.WebControls.HiddenField
    Protected WithEvents lblMissedRecipient As Label
    Protected WithEvents lblMultiRecipientQuestion As Label
    Protected WithEvents trMissedRecipient As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trMissedRecipientEmptyRow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trMultiRecipientQuestion As System.Web.UI.HtmlControls.HtmlTableRow
    'END: PPP | 01/20/2017 | YRS-AT-3299 | Old controls comments and new controls added in its place
    'For menu binding added by nidhin
    'Personal
    'Personal
    Protected WithEvents LabelPerSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPerSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSSNoPayeePayee As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownlistPerSal As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelSal As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownlistSal As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelPerFirst As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPerFirst As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPerMiddle As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPerMiddle As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelMiddleName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMiddleName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPerLast As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPerLAst As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPerSuffix As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPerSuffix As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSuffix As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSuffix As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayeeAdd1 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayeeAdd1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelBirthDate As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxBirthDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxBirthDate As YMCAUI.DateUserControl
    Protected WithEvents LabelPayeeAdd2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayeeAdd2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelSpouseAdd1 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxSpouseAdd1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayeeAdd3 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayeeAdd3 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelSpouseAdd2 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxSpouseAdd2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayeeCity As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayeeCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelSpouseAdd3 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxSpouseAdd3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPerState As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPerState As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPerZip As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPerZip As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelSpouseCity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxSpouseCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayeeCountry As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayeeCountry As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelSpouseState As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxSpouseState As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelSpouseZip As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxSpouseZip As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayeeTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayeeTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPerTel As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPerTel As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayeeEmail As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayeeEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTel As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTel As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEmail As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEmail As System.Web.UI.WebControls.TextBox
    'added kranthi 091908
    'Protected WithEvents AddressWebUserControl1 As AddressUserControlNew
    Protected WithEvents AddressWebUserControl1 As YMCAUI.AddressUserControl
    Protected WithEvents HyperLinkViewRetireesInfo As System.Web.UI.WebControls.HyperLink
    'added kranthi 091908
    Protected WithEvents ButtonAddNewBeneficiary As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditBeneficiary As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancelBeneficiary As System.Web.UI.WebControls.Button
    'Personal

    'added kranthi 141008
    'Protected WithEvents CheckboxIsSpouse As System.Web.UI.WebControls.CheckBox

    Protected WithEvents dgPager As DataGridPager

    Protected WithEvents Menu1 As skmMenu.Menu
    'Protected WithEvents ButtonDocumentCancel As System.Web.UI.WebControls.Button 'PPP | 01/24/2017 | YRS-AT-3299 | Old button
    Protected WithEvents btnAddBeneficiaryToList As System.Web.UI.WebControls.Button
    Protected WithEvents CheckBoxSpecialDividends As System.Web.UI.WebControls.CheckBox
    Protected WithEvents ButtonShowBalance As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdjust As System.Web.UI.WebControls.Button
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl

    'SP  2014.06.25 BT-2445\YRS 5.0-633
    Protected WithEvents lblMessage As Label
    Protected WithEvents gvBeneficiaryAnnuityDetails As GridView
    Protected WithEvents btnOk As System.Web.UI.WebControls.Button
    Protected WithEvents btnCancel As System.Web.UI.WebControls.Button
    Protected WithEvents pnlBeneficiaryAnnuity As System.Web.UI.WebControls.Panel
    '---
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Declaring controls for marital status and gender.
    Protected WithEvents DropDownListMaritalStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DropDownListGender As System.Web.UI.WebControls.DropDownList
    'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Declaring controls for marital status and gender.

    'START: MMR | YRS-AT-3299 | Declared controls for displaying message
    Protected WithEvents DivWarningMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DivSuccessMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DivMainMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DivPlanWarningMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents btnConfirmDialogYes As System.Web.UI.WebControls.Button
    Protected WithEvents hdnRecipientForDeletion As System.Web.UI.WebControls.HiddenField
    'END: MMR | YRS-AT-3299 | Declared controls for displaying message

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Variable Decleration"
    Dim dsPartBal As New DataSet
    Dim dsPartTotal As DataSet
    Private Const l_string_member As String = "RETIRE"
    ' START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    ' This Constant is used for Tab Index 
    'Private Const RETIREE_QDRO_TAB_STRIP_LIST As Integer = 0 'List Tab 'MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    Private Const RETIREE_QDRO_TAB_STRIP_BENEFICIARIES As Integer = 0 ' Beneficiary Tab 'MMR | 2017.01.10 | YRS-AT-3299 | Changed the value from 1 to 0
    Private Const RETIREE_QDRO_TAB_STRIP_ANNUITIES As Integer = 1 'Annuities Tab 'MMR | 2017.01.10 | YRS-AT-3299 | Changed the value from 2 to 1
    Private Const RETIREE_QDRO_TAB_STRIP_SUMMARY As Integer = 2 ' Summary Tab 'MMR | 2017.01.10 | YRS-AT-3299 | Changed the value from 3 to 2
    ' END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    Dim PercentageForSplit As Decimal

    Dim dtBenifAccount As New DataTable    'Contains all the data for the dtBenifAccount
    Dim dtPartAccount As New DataTable     'Contains all the data for the Participant
    Dim dtRecptAccount As New DataTable    'Contains all the data for the Recipient
    Dim dtPartOriginal As New DataTable       'Contains all the data for the Participant Total 
    Dim dtRecptAccounttemp As New DataTable
    Dim dtBenifAccountTempTable As New DataTable

    'Added by Amit
    Dim dtFind As New DataTable
    Dim dtfindset As New DataSet

    Dim dsBalBeneficiary As New DataSet
    Dim l_dataset_ParticipantDetail As New DataSet
    Dim dtFindBeneficiary As New DataTable
    Dim dgBeneficiary As New DataGrid
    Dim dgParticipant As New DataGrid
    Dim dtBeneficiarySession As New DataTable
    Dim blnSave As Boolean = False
    Dim blnLnkBothPlans As Boolean = True 'Chandra sekar| 2016.08.22 | YRS-AT-3081 - This is for After splitType Of Retirement/Saving Option the Link for Both option will be Disapear
    Dim strWarningMessage As String 'Chandra sekar| 2016.08.22 | YRS-AT-3081  TO have Currently Selected Original account balances
    Dim blnbtnShowBalances As String = False
    Protected currencyType As New System.Globalization.CultureInfo("en-US") ' set the cultere here based on whatever your requirements are
    'Start - SR:2013.06.26 - BT-2445\YRS 5.0-633
    'Dim dsAnnuityTypes As New DataSet
    'Dim bitIncrease As Boolean
    'Dim bitJointSurvivior As Boolean
    'Dim bitSSLeveling As Boolean
    'End - SR:2013.06.26 - BT-2445\YRS 5.0-633

    'START: MMR | 01/10/2017 | YRS-AT-3299 | Recipient grid sell index
    Dim recipientIndexPersID As Integer = 2
    Dim recipientIndexSSN As Integer = 3
    Dim recipientIndexFundEventID As Integer = 8
    Dim recipientIndexRetireeID As Integer = 9
    'END: MMR | 01/10/2017 | YRS-AT-3299 | Recipient grid sell index

#End Region

#Region "Properties"

    Private Property SessionPageCount() As Int32
        Get
            If Not (Session("MemberInfoPageCount")) Is Nothing Then

                Return (DirectCast(Session("MemberInfoPageCount"), Int32))
            Else
                Return 0
            End If
        End Get

        Set(ByVal Value As Int32)
            Session("MemberInfoPageCount") = Value
        End Set
    End Property

    Private Property Session_Datatable_DtBenifAccount() As DataTable
        Get
            If Not (Session("dtBenifAccount")) Is Nothing Then

                Return (DirectCast(Session("dtBenifAccount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtBenifAccount") = Value
        End Set
    End Property
    Private Property Session_Datatable_DtPartAccount() As DataTable
        Get
            If Not (Session("dtPartAccount")) Is Nothing Then

                Return (DirectCast(Session("dtPartAccount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtPartAccount") = Value
        End Set
    End Property
    Private Property Session_Datatable_DtRecptAccount() As DataTable
        Get
            If Not (Session("dtRecptAccount")) Is Nothing Then

                Return (DirectCast(Session("dtRecptAccount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtRecptAccount") = Value
        End Set
    End Property
    'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START
    Private Property Session_Datatable_DsPartBal() As DataSet
        Get
            If Not (Session("dsPartBal")) Is Nothing Then

                Return (DirectCast(Session("dsPartBal"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsPartBal") = Value
        End Set
    End Property
    'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END
    Private Property Session_Datatable_DtPartTotal() As DataTable
        Get
            If Not (Session("dtPartTotal")) Is Nothing Then

                Return (DirectCast(Session("dtPartTotal"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtPartTotal") = Value
        End Set
    End Property
    Private Property Session_Datatable_DtRecptAccountTemp() As DataTable
        Get
            If Not (Session("dtRecptAccounttemp")) Is Nothing Then

                Return (DirectCast(Session("dtRecptAccounttemp"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtRecptAccounttemp") = Value
        End Set
    End Property
    Private Property Session_Datatable_DtBeneficiarySession() As DataTable
        Get
            If Not (Session("dtBeneficiarySession")) Is Nothing Then

                Return (DirectCast(Session("dtBeneficiarySession"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtBeneficiarySession") = Value
        End Set
    End Property

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Session was used to handle post back event if recipient fund event id is not set, but it is handled by modal popup so postback does not happen
    'Private Property Session_Bool_MissingFundEventId() As Boolean
    '    Get
    '        If Not (Session("bool_MissingFundEventId")) Is Nothing Then

    '            Return (CType(Session("bool_MissingFundEventId"), Boolean))
    '        Else
    '            Return Nothing
    '        End If
    '    End Get

    '    Set(ByVal Value As Boolean)
    '        Session("bool_MissingFundEventId") = Value
    '    End Set
    'End Property
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Session was used to handle post back event if recipient fund event id is not set, but it is handled by modal popup so postback does not happen

    Private Property Session_Bool_NewPerson() As Boolean
        Get
            If Not (Session("NewPerson")) Is Nothing Then

                Return (CType(Session("NewPerson"), Boolean))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As Boolean)
            Session("NewPerson") = Value
        End Set
    End Property
    Private Property String_FundEventID() As String
        Get
            If Not Session("FundEventID") Is Nothing Then
                Return Session("FundEventID")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("FundEventID") = Value
        End Set
    End Property
    Private Property String_PersId() As String
        Get
            If Not Session("PersId") Is Nothing Then
                Return Session("PersId")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersId") = Value
        End Set
    End Property
    Private Property String_RecptPersId() As String
        Get
            If Not Session("RecptPersId") Is Nothing Then
                Return Session("RecptPersId")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("RecptPersId") = Value
        End Set
    End Property
    Private Property String_RecptFundEventID() As String
        Get
            If Not Session("RecptFundEventID") Is Nothing Then
                Return Session("RecptFundEventID")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("RecptFundEventID") = Value
        End Set
    End Property
    Private Property String_RecptRetireeID() As String
        Get
            If Not Session("RecptRetireeID") Is Nothing Then
                Return Session("RecptRetireeID")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("RecptRetireeID") = Value
        End Set
    End Property

    Private Property ParticipantSSN() As String 'PPP | 01/24/2017 | YRS-AT-3299 | Renamed as "ParticipantSSN" from String_PersSSID
        Get
            If Not Session("PersSSID") Is Nothing Then
                Return Session("PersSSID")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersSSID") = Value
        End Set
    End Property
    Private Property String_QDRORequestID() As String
        Get
            If Not Session("QDRORequestID") Is Nothing Then
                Return Session("QDRORequestID")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("QDRORequestID") = Value
        End Set
    End Property
    Private Property string_Benif_PersonID() As String
        Get
            If Not Session("Benif_PersonID") Is Nothing Then
                Return Session("Benif_PersonID")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("Benif_PersonID") = Value
        End Set
    End Property
    Private Property String_Part_SSN() As String
        Get
            If Not Session("Part_SSN") Is Nothing Then
                Return Session("Part_SSN")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("Part_SSN") = Value
        End Set
    End Property
    Private Property String_PhonySSNo() As String
        Get
            If Not Session("PhonySSNo") Is Nothing Then
                Return Session("PhonySSNo")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("PhonySSNo") = Value
        End Set
    End Property

    Private Property String_Benif_SSno() As String
        Get
            If Not Session("Benif_SSno") Is Nothing Then
                Return Session("Benif_SSno")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("Benif_SSno") = Value
        End Set
    End Property

    Private Property Control_To_Focus() As Control
        Get
            If Not Session("Control_To_Focus") Is Nothing Then
                Return Session("Control_To_Focus")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As Control)
            Session("Control_To_Focus") = Value
        End Set
    End Property
    Private Property Session_Dataset_QDRORetiree() As DataSet
        Get
            If Not (Session("dataset_QDRORetiree")) Is Nothing Then

                Return (DirectCast(Session("dataset_QDRORetiree"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dataset_QDRORetiree") = Value
        End Set
    End Property

    'START: PPP | 01/23/2017 | YRS-AT-3299 | There are no conditions written for "Session_String_ISComplited" session so commenting it,
    'Private Property Session_String_ISComplited() As Boolean
    '    Get
    '        If Not (Session("Session_String_ISComplited")) Is Nothing Then

    '            Return (CType(Session("Session_String_ISComplited"), Boolean))
    '        Else
    '            Return Nothing
    '        End If
    '    End Get

    '    Set(ByVal Value As Boolean)
    '        Session("Session_String_ISComplited") = Value
    '    End Set
    'End Property
    'END: PPP | 01/23/2017 | YRS-AT-3299 | There are no conditions written for "Session_String_ISComplited" session so commenting it,

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition, at several places only values are being assigned
    'Added kranthi 101008
    'Private Property Session_String_ISAddUpdate() As String
    '    Get
    '        If Not (Session("Session_String_ISAddUpdate")) Is Nothing Then
    '            Return Session("Session_String_ISAddUpdate")
    '        Else
    '            Return Nothing
    '        End If
    '    End Get
    '    Set(ByVal Value As String)
    '        Session("Session_String_ISAddUpdate") = Value
    '    End Set
    'End Property
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition, at several places only values are being assigned

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
    'SP 2014.06.30 BT-2445\YRS 5.0-633 -Start

    'START: PPP | 01/24/2017 | YRS-AT-3299 | This property is not required now, because no differentiation is needed to identify all recipients are settled or not
    'Private Property PartialBeneficiarySettleted() As String
    '    Get
    '        If Not (ViewState("PartialBeneficiarySettleted")) Is Nothing Then

    '            Return (CType(ViewState("PartialBeneficiarySettleted"), String))
    '        Else
    '            Return Nothing
    '        End If
    '    End Get

    '    Set(ByVal Value As String)
    '        ViewState("PartialBeneficiarySettleted") = Value
    '    End Set
    'End Property
    'END: PPP | 01/24/2017 | YRS-AT-3299 | This property is not required now, because no differentiation is needed to identify all recipients are settled or not

    'SP 2014.06.30 BT-2445\YRS 5.0-633-End
    'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    Private Property SplitPlanTypeOption() As String
        Get
            If Not (ViewState("SplitPlanTypeOption")) Is Nothing Then

                Return (CType(ViewState("SplitPlanTypeOption"), String))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As String)
            ViewState("SplitPlanTypeOption") = Value
        End Set
    End Property
    Private Property ViewState_Datatable_DtPartCurrentAccount() As DataTable
        Get
            If Not (ViewState("dtPartCurrentAccount")) Is Nothing Then

                Return (DirectCast(ViewState("dtPartCurrentAccount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            ViewState("dtPartCurrentAccount") = Value
        End Set
    End Property

    Private Property ViewState_Datatable_DtPartSelectedAccount() As DataTable
        Get
            If Not (ViewState("DtPartSelectedAccount")) Is Nothing Then

                Return (DirectCast(ViewState("DtPartSelectedAccount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            ViewState("DtPartSelectedAccount") = Value
        End Set
    End Property
    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
#End Region

#Region "Page Load"


    '***************************************************************************************************//
    'Event Name                :Page_Load                 Used In     : YMCAUI                          //
    'Created By                :Dilip Patada              Modified On : 18/06/08                        //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This class is being used to adding attribute to control,initializing    //
    '                          :session variable & checking login status of the user.                   //
    '***************************************************************************************************//
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try

            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            'START: PPP | 01/24/2017 | YRS-AT-3299 | Old message box is not being used so Request.Form("OK") value will be set
            'If Me.Session_Bool_MissingFundEventId = True Then
            '    If Me.Session_Bool_MissingFundEventId = True Then
            '        If Request.Form("OK") = "OK" Then
            '            Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_LIST
            '            Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_LIST

            '            Me.Session_Bool_MissingFundEventId = Nothing
            '        End If
            '    End If
            'End If
            'END: PPP | 01/24/2017 | YRS-AT-3299 | Old message box is not being used so Request.Form("OK") value will be set

            'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
            'DataGridRetireeList.PageSize = 10
            'dgPager.Grid = DataGridRetireeList
            'dgPager.PagesToDisplay = 10
            'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True) 'PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Registering JQuery dialog
            If Not IsPostBack Then
                ClearObjects()
                LoadRetiredQDRODetails() 'MMR | 2017.01.10 | YRS-AT-3299 | Loading QDRO Details
                CheckReadOnlyMode()      'Shilpa N | 03/11/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
                AssignAttributesToControls()

                'START: MMR | 2017.01.10 | YRS-AT-3299 | Following code is old and not required now
                'EnabledAndVisibleControls(False, False, True, True, True, False, False, False, False, False, False, False, True, False, False, False, False, False, String.Empty) 'Chandra sekar | 2016.08.22 | YRS-AT-3081  Added the Intially Disable and Visible Controls
                ''Control_To_Focus = TextBoxFundNo
                'Call SetControlFocus(Me.TextBoxFundNoList)
                'END: MMR | 2017.01.10 | YRS-AT-3299 | Following code is old and not required now

                'SP 2014.06.25 BT-2445\YRS 5.0-633-
                InitializePayrollDate()
                'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Loading marital status and gender.               
                PopulateMaritalStatusDropDownList()
                PopulateGenderDropDownList()
                ManageEditableControls(False)
                'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Loading marital status and gender.
            End If

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()

            'START: PPP | 01/24/2017 | YRS-AT-3299 | Session("VerifiedAddress") is not being set anywhere in the code
            ''BS:2012.05.17:YRS 5.0-1470: here check if page request is yes then save data in database
            'If Session("VerifiedAddress") = "VerifiedAddress" Then
            '    If Request.Form("Yes") = "Yes" Then
            '        Session("VerifiedAddress") = ""
            '        DocumentSave()
            '        Exit Sub
            '    ElseIf Request.Form("No") = "No" Then
            '        Session("VerifiedAddress") = ""
            '        Exit Sub
            '    End If
            'End If
            'END: PPP | 01/24/2017 | YRS-AT-3299 | Session("VerifiedAddress") is not being set anywhere in the code

            'SP 2014.06.30 BT-2445\YRS 5.0-633 -Start
            'If Request.Form("Yes") = "Yes" And Me.Session_String_ISComplited = True Then
            '    saveSplit()
            '    Me.Session_String_ISComplited = True
            '    Exit Sub
            'End If
            'If Request.Form("No") = "No" And Me.Session_String_ISComplited = True Then
            '    Me.Session_String_ISComplited = False
            '    Exit Sub
            'End If

            'If Me.Session_String_ISComplited = True Then
            '    Response.Redirect("MainWebForm.aspx", False)
            '    Me.Session_String_ISComplited = False
            '    Me.Session_String_ISComplited = Nothing
            '    Exit Sub
            'End If
            'SP 2014.06.30 BT-2445\YRS 5.0-633 -End

            If Me.String_PhonySSNo = "Not_Phony_SSNo" Then
                Me.String_PhonySSNo = Nothing
                Call SetControlFocus(Me.TextBoxSSNo)
            End If

            'START: PPP | 01/24/2017 | YRS-AT-3299 | Old message box used to set Request.Form("Yes"), Request.Form("No") and Request.Form("Ok"), but we have moved to modal popup usage so comenting it
            'Dim obj As RequiredFieldValidator
            'If Request.Form("Yes") = "Yes" Then
            '    'START- CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function
            '    'CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = False
            '    'CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = False
            '    'END- CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function
            '    'AddressWebUserControl1.SetValidationsForSecondary()
            '    'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
            '    'CType(AddressWebUserControl1.FindControl("rgExp"), RegularExpressionValidator).Enabled = False
            '    Call ClearControls(True)
            '    Call SetControlFocus(Me.TextBoxSSNo)
            '    ButtonAddNewBeneficiary.Enabled = False
            '    LockAndUnLockRecipientControls(False)
            '    TextBoxSSNo.Enabled = True
            '    'START- CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function
            '    'TextBoxBirthDate.RequiredDate = False
            '    ''Start - Manthan Rajguru | 2016.08.16 |YRS-AT-2482 | Disabling compare Validator control
            '    'CType(Me.FindControl("compValidatorGender"), CompareValidator).Enabled = False
            '    'CType(Me.FindControl("compValidatorMaritalStatus"), CompareValidator).Enabled = False
            '    ''End - Manthan Rajguru | 2016.08.16 |YRS-AT-2482 | Disabling compare Validator control
            '    'END-CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function

            'Else
            '    'START- CS| 09/18/2016 | YRS-AT-3081 | Code is moved to the common function
            '    ''changed kranthi 171008
            '    ' If Me.Session_Bool_NewPerson = False Then
            '    '    CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = False
            '    '    CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = False
            '    '    'AddressWebUserControl1.SetValidationsForSecondary()
            '    '    ''Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
            '    '    'CType(AddressWebUserControl1.FindControl("rgExp"), RegularExpressionValidator).Enabled = False
            '    '    TextBoxBirthDate.RequiredDate = False
            '    'Else
            '    '    CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = True
            '    '    CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = True
            '    '    'AddressWebUserControl1.SetValidationsForPrimary()
            '    '    'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
            '    '    'CType(AddressWebUserControl1.FindControl("rgExp"), RegularExpressionValidator).Enabled = True
            '    '    ''Priya : 14/12/2010:Made changes in adddress user control
            '    '    'AddressWebUserControl1.SetCountStZipCodeMandatoryOnSelection()
            '    '    TextBoxBirthDate.RequiredDate = True
            '    'End If

            '    'TextBoxBirthDate.RequiredDate = True
            '    ''AddressWebUserControl1.SetValidationsForAddress()
            '    'END- CS| 09/18/2016 | YRS-AT-3081 | Code is moved to the common function

            'End If

            'If Request.Form("No") = "No" Then
            '    Call ClearControls(False)
            '    Call LoadDataSelectedBeneficiary()
            '    'AddressWebUserControl1.SetValidationsForSecondary()
            '    'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
            '    'CType(AddressWebUserControl1.FindControl("rgExp"), RegularExpressionValidator).Enabled = False
            '    'lockRecipientControls(False)
            '    ButtonAddNewBeneficiary.Enabled = True
            '    Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES
            '    Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES
            'End If

            'If Not Session_String_ISAddUpdate Is Nothing Then
            '    If Request.Form("Ok") = "OK" And Session_String_ISAddUpdate = "Update To List" Then
            '        LockAndUnLockRecipientControls(False)
            '        ButtonAddNewBeneficiary.Enabled = True
            '        Me.Session_String_ISAddUpdate = Nothing
            '    End If
            '    If Request.Form("Ok") = "OK" And Session_String_ISAddUpdate = "InvalidSSNo" Then
            '        'START- CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function
            '        'CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = False
            '        ' CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = False
            '        'END- CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function

            '        'AddressWebUserControl1.SetValidationsForSecondary()
            '        'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
            '        'CType(AddressWebUserControl1.FindControl("rgExp"), RegularExpressionValidator).Enabled = False
            '        ' TextBoxBirthDate.RequiredDate = False  'CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function
            '        Session_String_ISAddUpdate = Nothing
            '        'START- CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function
            '        ''Start - Manthan Rajguru | 2016.08.16 |YRS-AT-2482 | Disabling compare Validator control
            '        'CType(Me.FindControl("compValidatorGender"), CompareValidator).Enabled = False
            '        'CType(Me.FindControl("compValidatorMaritalStatus"), CompareValidator).Enabled = False
            '        ''End - Manthan Rajguru | 2016.08.16 |YRS-AT-2482 | Disabling compare Validator control
            '        'END-CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function
            '    End If
            'End If
            'END: PPP | 01/24/2017 | YRS-AT-3299 | Old message box used to set Request.Form("Yes"), Request.Form("No") and Request.Form("Ok"), but we have moved to modal popup usage so comenting it

            If Not Me.Control_To_Focus Is Nothing Then
                If CType(Me.Control_To_Focus, Control).GetType.ToString() = "System.Web.UI.WebControls.DropDownList" Then
                    Call SetControlFocusDropDown(CType(Me.Control_To_Focus, Control))
                    Me.Control_To_Focus = Nothing
                ElseIf CType(Me.Control_To_Focus, Control).GetType.ToString() = "ASP.DateUserControl_ascx" Then
                    Call SetControlFocusDOB(CType(Me.Control_To_Focus, Control))
                    Me.Control_To_Focus = Nothing
                Else
                    Call SetControlFocus(CType(Me.Control_To_Focus, Control))
                    Me.Control_To_Focus = Nothing
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("PageLoadError", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Request.Form("No") = "No" Then
            TextBoxBirthDate.Enabled = False
        End If
    End Sub

#End Region

#Region "Private Methods"

    'START: MMR | 2017.01.09 | YRS-AT-3299 | Commneted existing code as not required
    ''***************************************************************************************************//
    ''Method Name               :LoadQDROList              Created on  : 14/04/08                        //
    ''Created By                :Dilip Patada              Modified On :                                 //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :No  parameters                                                          //
    ''Method Description        :This method is used to get the List of the Retired Persons              //
    ''***************************************************************************************************//
    'Public Sub LoadQDROList()
    '    Dim l_PagingOn As Boolean

    '    Try
    '        Dim l_dataset_QDROList As New DataSet
    '        l_dataset_QDROList = YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.LookUpRetiredList(TextBoxSSNoList.Text.Trim(), TextBoxFundNoList.Text.ToString.Trim(), TextBoxLastNameList.Text.Trim(), TextBoxFirstNameList.Text.Trim(), TextBoxStateList.Text.Trim(), TextBoxCityList.Text.Trim())
    '        Me.Session_Dataset_QDRORetiree = l_dataset_QDROList
    '        'Me.DataGridRetireeList.DataSource = l_dataset_QDROList
    '        'DataGridRetireeList.DataBind()

    '        If Not l_dataset_QDROList Is Nothing Then

    '            l_PagingOn = l_dataset_QDROList.Tables(0).Rows.Count > 5000

    '            If Me.DataGridRetireeList.CurrentPageIndex >= Me.SessionPageCount And Me.SessionPageCount <> 0 Then Exit Sub

    '            If (l_dataset_QDROList.Tables(0).Rows.Count > 0) Then
    '                '        Session("ds") = g_dataset_dsMemberInfo
    '                LabelNoDataFound.Visible = False
    '                Me.DataGridRetireeList.Visible = True

    '                If l_PagingOn Then
    '                    dgPager.Visible = True
    '                    DataGridRetireeList.AllowPaging = True
    '                Else
    '                    dgPager.Visible = False
    '                    DataGridRetireeList.AllowPaging = False
    '                End If


    '                If l_PagingOn Then

    '                    DataGridRetireeList.AllowPaging = False
    '                    DataGridRetireeList.AllowPaging = True
    '                    DataGridRetireeList.CurrentPageIndex = 0
    '                    DataGridRetireeList.PageSize = 10
    '                    dgPager.Grid = DataGridRetireeList
    '                    dgPager.PagesToDisplay = 10
    '                    dgPager.Visible = True
    '                    'dgPager.CurrentPage = 0
    '                Else

    '                End If
    '            Else
    '                LabelNoDataFound.Visible = True
    '                Me.DataGridRetireeList.Visible = False
    '                dgPager.Visible = False
    '            End If

    '            Me.DataGridRetireeList.SelectedIndex = -1
    '            Me.DataGridRetireeList.DataSource = l_dataset_QDROList
    '            Me.SessionPageCount = Me.DataGridRetireeList.PageCount
    '            Me.DataGridRetireeList.DataBind()

    '            '    'CommonModule.HideColumnsinDataGrid(g_dataset_dsMemberInfo, Me.DataGridFindInfo, "PersID,FundIdNo,FundUniqueId")


    '        Else
    '            LabelNoDataFound.Visible = True
    '            Me.DataGridRetireeList.Visible = False
    '            dgPager.Visible = False
    '        End If
    '    Catch ex As SqlException
    '        Me.DataGridRetireeList.Visible = False
    '        dgPager.Visible = False

    '    Catch
    '        Throw
    '    End Try

    'End Sub
    'END: MMR | 2017.01.09 | YRS-AT-3299 | Commneted existing code as not required

    '***************************************************************************************************//
    'Method Name               :SetControlFocus           Created on  : 14/04/08                        //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :TestBox                                                                 //
    'Method Description        :This method is used to set the focus to the control                     //
    '***************************************************************************************************//

    Private Sub SetControlFocus(ByVal TextBoxFocus As TextBox)
        Dim l_string_script As String
        Try
            l_string_script = "<script language='Javascript'>" & _
                            "var obj = document.getElementById('" & TextBoxFocus.ID & "');" & _
                            "if (obj!=null){if (obj.disabled==false){obj.focus();}}" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("scriptsetfocus")) Then
                Page.RegisterStartupScript("scriptsetfocus", l_string_script)
            End If
        Catch
            Throw
        End Try
    End Sub


    'START: PPP | 01/24/2017 | YRS-AT-3299 | Not using EnableBeneficiaryControl
    ''***************************************************************************************************//
    ''Method Name               :EnableBeneficiaryControl           Created on  : 14/04/08               //
    ''Created By                :Kranti                             Modified On :                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         : Boolean                                                                //
    ''Method Description        :This method is used to Enable disable the control                       //
    ''***************************************************************************************************//
    'Private Sub EnableBeneficiaryControl(ByVal parameterFlag As Boolean)

    '    Try
    '        ButtonEditBeneficiary.Enabled = parameterFlag
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Not using EnableBeneficiaryControl

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Not using EnableBeneficiaryControlAddEdit
    ''***************************************************************************************************//
    ''Method Name               :EnableBeneficiaryControlAddEdit    Created on  : 14/04/08               //
    ''Created By                :Kranti                             Modified On :                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         : Boolean                                                                //
    ''Method Description        :This method is used to Enable disable the control                       //
    ''***************************************************************************************************//
    'Private Sub EnableBeneficiaryControlAddEdit(ByVal parameterFlag As Boolean)
    '    Try

    '        ButtonEditBeneficiary.Enabled = parameterFlag
    '        ButtonCancelBeneficiary.Enabled = parameterFlag

    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Not using EnableBeneficiaryControlAddEdit

    '***************************************************************************************************//
    'Method Name               :SetControlFocusDropDown   Created on  :                                 //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :DropDown                                                                //
    'Method Description        :This method is used to set the focus to the control                     //
    '***************************************************************************************************//

    Private Sub SetControlFocusDropDown(ByVal DropDownFocus As DropDownList)
        Dim l_string_script As String
        Try
            l_string_script = "<script language='Javascript'>" & _
                            "var obj = document.getElementById('" & DropDownFocus.ID & "');" & _
                            "if (obj!=null){if (obj.disabled==false){obj.focus();}}" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("scriptsetfocus")) Then
                Page.RegisterStartupScript("scriptsetfocus", l_string_script)
            End If
        Catch
            Throw
        End Try
    End Sub
    '***************************************************************************************************//
    'Method Name               :SetControlFocusDOB        Created on  :                                 //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :Date Of Birth User Control                                              //
    'Method Description        :This method is used to set the focus to the control                     //
    '***************************************************************************************************//

    Private Sub SetControlFocusDOB(ByVal DateOfBirth As UserControl)
        Dim l_string_script As String
        Try
            l_string_script = "<script language='Javascript'>" & _
                            "var obj = document.getElementById('" & DateOfBirth.ID & "');" & _
                            "if (obj!=null){if (obj.disabled==false){obj.focus();}}" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("scriptsetfocus")) Then
                Page.RegisterStartupScript("scriptsetfocus", l_string_script)
            End If
        Catch
            Throw
        End Try
    End Sub

    '***************************************************************************************************//
    'Method Name               :LoadAnnuityTab            Created on  : 14/04/08                        //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This method is used to Load Annuity detail in annuity tab.              //
    '***************************************************************************************************//
    Private Sub LoadAnnuityTab()
        Dim l_string_Part_SSN As String = String.Empty
        Dim l_string_Part_fundEventID As String = String.Empty

        Try

            l_string_Part_SSN = Me.String_Part_SSN
            l_string_Part_SSN = l_string_Part_SSN.Substring(0, l_string_Part_SSN.IndexOf("-"))
            l_string_Part_fundEventID = Me.String_FundEventID

            'RadioButtonListSplitAmtType.Items(1).Selected = True 'Chandra sekar| 2016.08.22 | YRS-AT-3081 - Remove and added Link Buttons
            'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START 
            'dsPartBal = YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.getPartAccountDetail(l_string_Part_SSN, l_string_Part_fundEventID, RadioButtonListPlanType.SelectedItem.Value.ToString())
            'Session("dsPartBal") = dsPartBal
            If Session_Datatable_DsPartBal Is Nothing Then
                dsPartBal = YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.getPartAccountDetail(l_string_Part_SSN, l_string_Part_fundEventID, "BOTH")
                Me.Session_Datatable_DsPartBal = dsPartBal
            Else
                dsPartBal = Me.Session_Datatable_DsPartBal
            End If


            'If Me.Session_Datatable_DtPartTotal Is Nothing Then
            '    dtPartTotal = CreatePartTotalTable()
            '    AddDataToPartTotal(dtPartTotal)
            'Else
            '    dtPartTotal = Me.Session_Datatable_DtPartTotal
            'End If
            dtPartOriginal = CreatePartTotalTable()
            AddDataToPartTotal(dtPartOriginal)

            '''If Me.Session_Datatable_DtPartAccount Is Nothing Then
            '''    dtPartAccount = CreateParticipantTable()
            '''    AddDataToParticipantTable(dtPartAccount)
            '''Else
            '''    dtPartAccount = Me.Session_Datatable_DtPartAccount
            '''End 
            dtPartAccount = CreateParticipantTable()
            AddDataToParticipantTable(dtPartAccount)
            'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END 
            DataGridWorkSheet.DataSource = dtPartOriginal
            DataGridWorkSheet.DataBind()
            UncheckAndDisableOriginalAccountBalance(dtPartOriginal) 'CS| 2016.08.22 | YRS-AT-3081
            Me.Session_Datatable_DtPartTotal = dtPartOriginal
            Me.ViewState_Datatable_DtPartSelectedAccount = dtPartAccount 'Chandra sekar| 2016.08.22 | YRS-AT-3081  TO have Currently Selected Original account balances
            If (HelperFunctions.isEmpty(Session_Datatable_DtRecptAccount)) Then
                Me.Session_Datatable_DtPartAccount = dtPartAccount
            End If




        Catch
            Throw
        End Try
    End Sub

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Not using ResetAnnuityTab()
    ''***************************************************************************************************//
    ''Method Name               :ResetAnnuityTab           Created on  : 08/09/08                        //
    ''Created By                :kranthi kumar             Modified On :                                 //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :                                                                        //
    ''Method Description        :This method is used to Load default annuity detail in Annuity tab.      //
    ''***************************************************************************************************//
    'Private Sub ResetAnnuityTab()

    '    Try

    '        If DataGridRecipientAnnuitiesBalance.Items.Count > 0 Then

    '            'RadioButtonListSplitAmtType.Items(1).Selected = True
    '            'RadioButtonListSplitAmtType.Items(0).Selected = False
    '            If TextBoxPercentageWorkSheet.Enabled = False Then
    '                TextBoxPercentageWorkSheet.Enabled = True
    '                TextBoxAmountWorkSheet.Enabled = False
    '            End If
    '            TextBoxPercentageWorkSheet.Text = "0.00"
    '            TextBoxAmountWorkSheet.Text = "0.00"
    '            ButtonSplit.Enabled = False
    '            ButtonAdjust.Enabled = False
    '            'ButtonDocumentSave.Enabled = False 'PPP | 01/24/2017 | YRS-AT-3299 | Old button
    '            ButtonReset.Enabled = False
    '            If CheckBoxSpecialDividends.Checked = True Then
    '                CheckBoxSpecialDividends.Checked = False
    '            End If

    '            Me.Session_Datatable_DtRecptAccount = Nothing
    '            Me.Session_Datatable_DtRecptAccountTemp = Nothing
    '            Me.Session_Datatable_DtPartAccount = Nothing
    '            Me.ViewState_Datatable_DtPartCurrentAccount = Nothing 'Chandra sekar| 2016.08.22 | YRS-AT-3081  TO have Currently Selected Original account balances
    '            Me.Session_Datatable_DtPartTotal() = Nothing
    '            'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START
    '            Me.Session_Datatable_DsPartBal() = Nothing
    '            'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END
    '            If dtRecptAccounttemp.Rows.Count > 0 Then
    '                dtRecptAccounttemp.Rows.Clear()
    '            End If

    '            DataGridRecipientAnnuitiesBalance.DataSource = dtRecptAccounttemp
    '            DataGridRecipientAnnuitiesBalance.DataBind()
    '            If dtPartAccount.Rows.Count > 0 Then
    '                dtPartAccount.Rows.Clear()
    '            End If
    '            DatagridParticipantsBalance.DataSource = dtPartAccount
    '            DatagridParticipantsBalance.DataBind()

    '            Me.Session_Datatable_DtBenifAccount = Nothing
    '            ButtonShowBalance.Enabled = False
    '            Me.SplitPlanTypeOption = Nothing
    '            ' Me.Session_String_ISComplited = True 'SP 2014.06.30 BT-2445\YRS 5.0-633
    '            'Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_LIST).Enabled = True 'MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    '            'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code and enabling beneficiary tab 
    '            'Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES).Enabled = False
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES).Enabled = True
    '            'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code and enabling beneficiary tab 
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = False
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = False
    '            'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    '            'Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_LIST
    '            'Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_LIST
    '            'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required

    '            'Added by dilip 22-12-2008 bugid 661
    '            DropdownlistBeneficiarySSNo.Enabled = False
    '            Dim sender As Object
    '            ButtonClear_Click(sender, New EventArgs())
    '            'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    '            'DataGridRetireeList.DataSource = Nothing
    '            'DataGridRetireeList.DataBind()
    '            'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    '            'SP 2014.07.17  BT-2445\YRS 5.0-633
    '            DropdownlistBeneficiarySSNo.Items.Clear()
    '            EnabledAndVisibleControls(False, False, False, False, False, False, False, False, False, False, False, False, False, False, False, False, False, False, String.Empty) 'Chandra sekar | 2016.08.22 | YRS-AT-3081  Added the Intially Disable and Visible Controls

    '        End If

    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Not using ResetAnnuityTab()

    '***************************************************************************************************//
    'Method Name               :ResetSession              Created on  : 08/09/08                        //
    'Created By                :Dilip  kumar             Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This method is used to clear the session when cancle button is pressed. //
    '***************************************************************************************************//
    Private Sub ResetSession()
        Me.string_Benif_PersonID = Nothing 'PPP | 01/24/2017 | YRS-AT-3299 
        Me.Session_Datatable_DtRecptAccount = Nothing
        Me.Session_Datatable_DtRecptAccountTemp = Nothing
        Me.Session_Datatable_DtPartAccount = Nothing
        Me.ViewState_Datatable_DtPartCurrentAccount = Nothing 'Chandra sekar| 2016.08.22 | YRS-AT-3081  TO have Currently Selected Original account balances
        Me.SplitPlanTypeOption = Nothing ''Chandra sekar| 2016.08.22 | YRS-AT-3081 - To View state Handles the Split Type
        Me.Session_Datatable_DtPartTotal = Nothing
        Me.Session_Datatable_DtBenifAccount = Nothing
        Me.Session_Datatable_DtBeneficiarySession = Nothing
        'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START
        Me.Session_Datatable_DsPartBal = Nothing
        'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END
        'Session("FindInfo_Sort") = Nothing 'PPP | 01/24/2017 | YRS-AT-3299 | This session variable was being used on old find screen
        DataGridWorkSheet.DataSource = Nothing
        DatagridBenificiaryList.DataSource = Nothing
        DataGridRecipientAnnuitiesBalance.DataSource = Nothing
        DatagridParticipantsBalance.DataSource = Nothing
        DatagridPartTotals.DataSource = Nothing
        DataGridWorkSheet.DataBind()
        DatagridBenificiaryList.DataBind()
        DataGridRecipientAnnuitiesBalance.DataBind()
        DatagridParticipantsBalance.DataBind()
        DatagridPartTotals.DataBind()
        Session("R_QDRO_PersID") = Nothing
    End Sub

    '***************************************************************************************************//
    'Method Name               :LoadSummaryTab            Created on  : 14/04/08                        //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This method is used to Load Annuity detail(Split detail)in summary tab. //
    '***************************************************************************************************//
    Private Sub LoadSummaryTab()
        'START: PPP | 01/23/2017 | YRS-AT-3299 | Handling buttons
        EnablePreviousButton(True)
        EnableNextButton(False)
        EnableSaveButton(True)
        'END: PPP | 01/23/2017 | YRS-AT-3299 | Handling buttons

        'Try 'START: PPP | 01/23/2017 | YRS-AT-3299 | try catch not handling anything so commenting it,
        dgBeneficiary.DataSource = Nothing
        dgBeneficiary.DataBind()
        dgParticipant.DataSource = Nothing
        dgParticipant.DataBind()
        Session.Remove("dtBeneficiarySession")
        'LoadSummaryBal() 'PPP | 01/24/2017 | YRS-AT-3299 | Just used to set class level datatable variable, which was moved to LoadBeneficiary method
        LoadParticipant()
        LoadBeneficiary()
        'START: PPP | 01/23/2017 | YRS-AT-3299 | There are no conditions written for "Session_String_ISComplited" session as well as try catch not handling anything so commenting it,
        'If blnSave = True Then
        '    Me.Session_String_ISComplited = True
        'End If
        'Catch
        '    Throw
        'End Try
        'END: PPP | 01/23/2017 | YRS-AT-3299 | There are no conditions written for "Session_String_ISComplited" session as well as try catch not handling anything so commenting it,
    End Sub

    'START: PPP | 01/24/2017 | YRS-AT-3299 | It is just setting dtFindBeneficiary value which is done at LoadBeneficiary method
    ''***************************************************************************************************//
    ''Method Name               :LoadSummaryBal            Created on  : 14/04/08                        //
    ''Created By                :Dilip Patada              Modified On :                                 //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :                                                                        //
    ''Method Description        :This method is used to Load Benificiary Annuity detail(Split detail)in  //
    ''                           summary tab.                                                            //
    ''***************************************************************************************************//
    'Private Sub LoadSummaryBal()
    '    'participant Bal
    '    Try

    '        dtBeneficiarySession = Me.Session_Datatable_DtRecptAccount.Copy()
    '        Me.Session_Datatable_DtBeneficiarySession = dtBeneficiarySession.Copy()
    '        dtFindBeneficiary = Me.Session_Datatable_DtBeneficiarySession.Copy()

    '    Catch
    '        Throw
    '    End Try

    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | It is just setting dtFindBeneficiary value which is done at LoadBeneficiary method

    '***************************************************************************************************//
    'Method Name               :LoadParticipant           Created on  : 14/04/08                        //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This method is used to Load Participant Annuity detail(Split detail)in  //
    '                           summary tab.                                                            //
    '***************************************************************************************************//
    Private Sub LoadParticipant()
        Try
            l_dataset_ParticipantDetail = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.GetParticipantDetail(Me.ParticipantSSN) 'PPP | 01/24/2017 | YRS-AT-3299 | Renamed as "ParticipantSSN" from String_PersSSID
            dtfindset = l_dataset_ParticipantDetail
            DataListParticipant.DataSource = dtfindset
            DataListParticipant.DataBind()
        Catch
            Throw
        End Try
    End Sub

    '***************************************************************************************************//
    'Method Name               :LoadBeneficiary           Created on  :                                 //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This method is used to populate the splited Benificiary details in      //
    '                           summary tab.                                                            //
    '***************************************************************************************************//
    Private Sub LoadBeneficiary()
        Dim dtCol As DataColumn
        Dim dttemp As New DataTable
        Try
            dtFindBeneficiary = Me.Session_Datatable_DtRecptAccount.Copy() 'PPP | 01/24/2017 | YRS-AT-3299 | Moved here from LoadSummaryBal()
            dtFind = Me.Session_Datatable_DtBenifAccount
            dttemp = dtFind.Clone
            For Each beneficiaryDetails As DataRow In dtFind.Rows
                For Each beneficiaryAccountRow As DataRow In dtFindBeneficiary.Rows
                    If beneficiaryAccountRow.Item(0) = beneficiaryDetails.Item(0) Then
                        dttemp.Rows.Add(New Object() {beneficiaryDetails(0), beneficiaryDetails(1), beneficiaryDetails(2), beneficiaryDetails(3), beneficiaryDetails(4), beneficiaryDetails(5)})
                        Exit For
                    End If
                Next
            Next
            DatalistBeneficiary.DataSource = dttemp
            DatalistBeneficiary.DataBind()
        Catch
            Throw
        End Try
    End Sub

    'START: PPP | 01/17/2017 | YRS-AT-3299 | Moved its content to LoadRecipient() method
    ''***************************************************************************************************//
    ''Method Name               :LoadPersonalTab           Created on  : 14/04/08                        //
    ''Created By                :Dilip Patada              Modified On :                                 //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :PersID & FundEventID                                                                     //
    ''Method Description        :This method is used Load the detail of the participant in the Personal  //
    ''                           Tab when user select the participant from the Participant Grid          //
    ''***************************************************************************************************//
    'Private Sub LoadPersonalTab()
    '    'Try
    '    Dim l_dataset_GeneralInfo As New DataSet
    '    Dim l_dataset_AddressInfo As New DataSet

    '    Try

    '        Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES
    '        Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES
    '        If DatagridBenificiaryList.Items.Count = 0 Then
    '            LockAndUnLockRecipientControls(False)
    '            TextBoxSSNo.Enabled = True
    '        End If
    '    Catch
    '        Throw
    '    End Try

    'End Sub
    'END: PPP | 01/17/2017 | YRS-AT-3299 | Moved its content to LoadRecipient() method

    '***************************************************************************************************//
    'Method Name               :lockRecipientControls     Created on  : 14/04/08                        //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :param_bool                                                              //
    'Method Description        :This method is used to Lock & unlock the controls based on the parameter//
    '                           passed to this method                                                   //
    '***************************************************************************************************//
    Private Sub LockAndUnLockRecipientControls(ByVal param_bool As Boolean)

        Try
            TextBoxSSNo.Enabled = param_bool
            DropdownlistSal.Enabled = param_bool
            TextBoxFirstName.Enabled = param_bool
            TextBoxMiddleName.Enabled = param_bool
            TextBoxLastName.Enabled = param_bool
            TextBoxSuffix.Enabled = param_bool
            TextBoxBirthDate.Enabled = param_bool
            'TextboxFirstName.Enabled = param_bool
            'TextboxFirstName.Enabled = param_bool
            'TextboxFirstName.Enabled = param_bool
            'TextboxFirstName.Enabled = param_bool
            'TextboxSpouseAdd1.Enabled = param_bool
            'TextboxSpouseAdd2.Enabled = param_bool
            'TextboxSpouseAdd3.Enabled = param_bool
            'TextboxSpouseCity.Enabled = param_bool
            'TextboxSpouseState.Enabled = param_bool
            'TextboxSpouseZip.Enabled = param_bool
            TextBoxEmail.Enabled = param_bool
            TextBoxTel.Enabled = param_bool
            TextBoxFirstName.Enabled = param_bool
            'TextboxSpouseCountry.Enabled = param_bool

            'Added kranthi 091908
            AddressWebUserControl1.EnableControls = param_bool
            'Added kranthi 091908

            'added kranthi 141008
            'CheckboxIsSpouse.Enabled = param_bool

            'START: PPP | 01/17/2017 | YRS-AT-3299 | Moved "EnableAndDisableValidatorcontrols()" code here becuase when we set enable property of control then their validator enable property also must be set
            CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = param_bool
            CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = param_bool
            TextBoxBirthDate.RequiredDate = param_bool
            CType(Me.FindControl("compValidatorGender"), CompareValidator).Enabled = param_bool
            CType(Me.FindControl("compValidatorMaritalStatus"), CompareValidator).Enabled = param_bool
            'END: PPP | 01/17/2017 | YRS-AT-3299 | Moved "EnableAndDisableValidatorcontrols()" code here becuase when we set enable property of control then their validator enable property also must be set
        Catch
            Throw
        End Try


    End Sub

    '***************************************************************************************************//
    'Method Name               :LoadDataBeneficiaryCancel Created on  : 14/04/08                        //
    'Created By                :Kranti                    Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This method is used  click on cancel button on beneficiary tab this     //
    '                           will select the first item from the beneficiary list.                   //
    '***************************************************************************************************//

    Private Sub LoadDataBeneficiaryCancel()

        Try
            Dim i As Integer = 0
            dtBenifAccount = Me.Session_Datatable_DtBenifAccount
            If Not dtBenifAccount Is Nothing Then
                'LoadDataSelectedBeneficiary()
                TextBoxSSNo.Text = dtBenifAccount.Rows(0).Item("SSNo")
                DropdownlistSal.SelectedValue = dtBenifAccount.Rows(0).Item("SalutationCode")
                TextBoxFirstName.Text = dtBenifAccount.Rows(0).Item("FirstName")
                TextBoxLastName.Text = dtBenifAccount.Rows(0).Item("LastName")
                TextBoxMiddleName.Text = dtBenifAccount.Rows(0).Item("MiddleName")

                TextBoxSuffix.Text = dtBenifAccount.Rows(0).Item("SuffixTitle")
                TextBoxBirthDate.Text = dtBenifAccount.Rows(0).Item("BirthDate")
                AddressWebUserControl1.Address1 = dtBenifAccount.Rows(0).Item("Address1") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add1" to "Address1", to support common recipient stagging db functionality
                AddressWebUserControl1.Address2 = dtBenifAccount.Rows(0).Item("Address2") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add2" to "Address2", to support common recipient stagging db functionality
                AddressWebUserControl1.Address3 = dtBenifAccount.Rows(0).Item("Address3") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add3" to "Address3", to support common recipient stagging db functionality
                AddressWebUserControl1.City = dtBenifAccount.Rows(0).Item("City")
                AddressWebUserControl1.DropDownListStateValue = dtBenifAccount.Rows(0).Item("State")
                AddressWebUserControl1.ZipCode = dtBenifAccount.Rows(0).Item("Zip") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "zip" to "Zip", to support common recipient stagging db functionality
                AddressWebUserControl1.DropDownListCountryValue = dtBenifAccount.Rows(0).Item("Country")
                TextBoxEmail.Text = dtBenifAccount.Rows(0).Item("EmailAddress") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "EMail" to "EmailAddress", to support common recipient stagging db functionality
                'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Selecting the marital status and gender forthe particular person.
                DropDownListMaritalStatus.SelectedValue = dtBenifAccount.Rows(0).Item("MaritalCode")
                DropDownListGender.SelectedValue = dtBenifAccount.Rows(0).Item("GenderCode")
                'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Selecting the marital status and gender forthe particular person.

                'added kranthi 141008
                'If dtBenifAccount.Rows(0).Item("IsSpouse") = "D" Then
                '    CheckboxIsSpouse.Checked = True
                'Else
                '    CheckboxIsSpouse.Checked = False
                'End If
                AddressWebUserControl1.HideNoAddressDefinedLabel() 'PPP | 01/24/2017 | YRS-AT-3299 | Hiding "No Address information defined." label

                DatagridBenificiaryList.DataSource = dtBenifAccount
                DatagridBenificiaryList.DataBind()

                If (dtBenifAccount.Rows(0).Item("FlagNewBenf") = False) Then
                    ButtonEditBeneficiary.Enabled = False
                Else
                    ButtonEditBeneficiary.Enabled = True
                End If
            End If
        Catch
            Throw
        End Try

    End Sub

    '***************************************************************************************************//
    'Method Name               :LoadDataSelectedBeneficiary Created on  : 14/04/08                      //
    'Created By                :Kranti                      Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This method is used  click on beneficiary list beneficiary tab this     //
    '                           will selectand display detail of the selected item from the beneficiary // 
    '                           list to the control.                                                    //
    '***************************************************************************************************//
    Private Sub LoadDataSelectedBeneficiary()
        Dim drBenef() As DataRow
        Dim datagridbenifrow As Integer
        Try
            dtBenifAccount = Me.Session_Datatable_DtBenifAccount
            If DropdownlistBeneficiarySSNo.Items.Count > 0 Then
                datagridbenifrow = DropdownlistBeneficiarySSNo.SelectedIndex
                drBenef = dtBenifAccount.Select(String.Format("ID='{0}'", DatagridBenificiaryList.Items(datagridbenifrow).Cells(recipientIndexPersID).Text)) 'PPP | 01/24/2017 | YRS-AT-3299 | Cells are accessed by defind index property. OLD: [DatagridBenificiaryList.Items(datagridbenifrow).Cells(1).Text]
            Else
                drBenef = dtBenifAccount.Select(String.Format("ID='{0}'", DatagridBenificiaryList.Items(0).Cells(recipientIndexPersID).Text)) 'PPP | 01/24/2017 | YRS-AT-3299 | Cells are accessed by defind index property. OLD: [DatagridBenificiaryList.Items(0).Cells(1).Text]
            End If

            Dim drDataRow As DataRow
            If (drBenef.Length <> 0) Then
                drDataRow = drBenef.GetValue(0)
                Me.String_RecptFundEventID = drDataRow("FundEventID")
                Me.String_RecptRetireeID = drDataRow("RecptRetireeID")
                TextBoxSSNo.Text = drDataRow("SSNo")
                If drDataRow("SalutationCode") <> "" Then
                    DropdownlistSal.SelectedValue = drDataRow("SalutationCode")
                End If
                TextBoxFirstName.Text = drDataRow("FirstName")
                TextBoxLastName.Text = drDataRow("LastName")
                TextBoxMiddleName.Text = drDataRow("MiddleName")
                TextBoxSuffix.Text = drDataRow("SuffixTitle")
                TextBoxBirthDate.Text = Convert.ToDateTime(drDataRow("BirthDate")) 'changed by kranthi 171008
                AddressWebUserControl1.Address1 = drDataRow("Address1") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add1" to "Address1", to support common recipient stagging db functionality
                AddressWebUserControl1.Address2 = drDataRow("Address2") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add2" to "Address2", to support common recipient stagging db functionality
                AddressWebUserControl1.Address3 = drDataRow("Address3") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add3" to "Address3", to support common recipient stagging db functionality
                AddressWebUserControl1.City = drDataRow("City")

                'Added Kranthi 101008
                'Me.AddressWebUserControl1.ShowDataForParticipant = 1
                'Me.AddressWebUserControl1.ShowDataForParticipant()
                'Added Kranthi 101008

                AddressWebUserControl1.DropDownListStateValue = drDataRow("State")
                AddressWebUserControl1.DropDownListCountryValue = drDataRow("Country")
                AddressWebUserControl1.ZipCode = drDataRow("Zip") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "zip" to "Zip", to support common recipient stagging db functionality
                TextBoxEmail.Text = drDataRow("EmailAddress") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "EMail" to "EmailAddress", to support common recipient stagging db functionality
                TextBoxTel.Text = drDataRow("PhoneNumber") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "PhoneNo" to "PhoneNumber", to support common recipient stagging db functionality
                DatagridBenificiaryList.DataSource = dtBenifAccount
                DatagridBenificiaryList.DataBind()
                AddressWebUserControl1.HideNoAddressDefinedLabel() 'PPP | 01/24/2017 | YRS-AT-3299 | Hiding "No Address information defined." label
                'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Selecting marital status and gender.
                DropDownListGender.SelectedValue = drDataRow("GenderCode")
                DropDownListMaritalStatus.SelectedValue = drDataRow("MaritalCode")
                'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Selecting marital status and gender.
                'added kranthi 141008
                'If drDataRow("IsSpouse") = "D" Then
                '    CheckboxIsSpouse.Checked = True
                'Else
                '    CheckboxIsSpouse.Checked = False
                'End If

                'Commented by kranthi. 230908
                'If (drDataRow("FlagNewBenf") = True) Then
                '    lockRecipientControls(True)
                'Else
                LockAndUnLockRecipientControls(False)
                Me.Session_Bool_NewPerson = Convert.ToBoolean(drDataRow("FlagNewBenf"))
                'End If
            Else
                ClearControls(True)
            End If

        Catch
            Throw
        End Try

    End Sub

#Region "saveSplit"

    Private Sub SaveSplit() 'PPP | 01/23/2017 | YRS-AT-3299 | Renamed from saveSplit to SaveSplit
        'START: PPP | 01/23/2017 | YRS-AT-3299 | Double causes a decimal point issue, hence using a decimal variable
        'Dim dblTotalSplitPercentage As Double
        'dblTotalSplitPercentage = 0.0
        Dim totalSplitPercentage As Decimal
        'END: PPP | 01/23/2017 | YRS-AT-3299 | Double causes a decimal point issue, hence using a decimal variable
        Try
            dtBenifAccount = DirectCast(Me.Session_Datatable_DtBenifAccount, DataTable)
            dtRecptAccount = DirectCast(Me.Session_Datatable_DtRecptAccount, DataTable)
            dtPartAccount = DirectCast(Me.Session_Datatable_DtPartAccount, DataTable)
            dtBenifAccountTempTable = dtBenifAccount.Clone

            totalSplitPercentage = 0
            For Each dtBenifAccountRow As DataRow In dtBenifAccount.Rows
                For Each dtRecptAccountRow As DataRow In dtRecptAccount.Rows
                    If dtRecptAccountRow.Item(0) = dtBenifAccountRow.Item(0) Then
                        'START: PPP | 01/23/2017 | YRS-AT-3299 | Double causes a decimal point issue, hence using a decimal variable
                        'dblTotalSplitPercentage = dblTotalSplitPercentage + Convert.ToDouble(dtRecptAccountRow("RecipientSplitPercent"))
                        totalSplitPercentage = totalSplitPercentage + Convert.ToDecimal(dtRecptAccountRow("RecipientSplitPercent"))
                        'END: PPP | 01/23/2017 | YRS-AT-3299 | Double causes a decimal point issue, hence using a decimal variable
                        dtBenifAccountTempTable.Rows.Add(New Object() {dtBenifAccountRow(0), dtBenifAccountRow(1), dtBenifAccountRow(2), dtBenifAccountRow(3), dtBenifAccountRow(4), dtBenifAccountRow(5), dtBenifAccountRow(6), dtBenifAccountRow(7), dtBenifAccountRow(8), dtBenifAccountRow(9), dtBenifAccountRow(10), dtBenifAccountRow(11), dtBenifAccountRow(12), dtBenifAccountRow(13), dtBenifAccountRow(14), dtBenifAccountRow(15), dtBenifAccountRow(16), dtBenifAccountRow(17), dtBenifAccountRow(18), dtBenifAccountRow(19), dtBenifAccountRow(20), dtBenifAccountRow(21), dtBenifAccountRow(22), dtBenifAccountRow(23), dtBenifAccountRow(24), dtBenifAccountRow(25)})
                        Exit For
                    End If
                Next
            Next

            'START: PPP | 01/23/2017 | YRS-AT-3299 | Double causes a decimal point issue, hence using a decimal variable
            'YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.SaveRetiredSplit(dtBenifAccountTempTable, dtRecptAccount, Me.String_RecptFundEventID, Me.String_FundEventID, Me.String_QDRORequestID, dtPartAccount, dblTotalSplitPercentage, dtBenifAccount)
            YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.SaveRetiredSplit(dtBenifAccountTempTable, dtRecptAccount, Me.String_RecptFundEventID, Me.String_FundEventID, Me.String_QDRORequestID, dtPartAccount, totalSplitPercentage, dtBenifAccount)
            'END: PPP | 01/23/2017 | YRS-AT-3299 | Double causes a decimal point issue, hence using a decimal variable
            blnSave = True

            'modified by dilip 22-12-2008 bug id 661

            'Me.Session_String_ISComplited = True 'SP 2014.06.30 BT-2445\YRS 5.0-633
            'modified by dilip 22-12-2008 bug id 661
            'START: PPP | 01/02/2017 | YRS-AT-3299 | Moving messages to JQuery modal popup
            'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_SAVED_SUCCESSFULLY, MessageBoxButtons.OK, False) ''Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
            ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_SAVED_SUCCESSFULLY, "ok")
            'END: PPP | 01/02/2017 | YRS-AT-3299 | Moving messages to JQuery modal popup
            'START: PPP | 01/02/2017 | YRS-AT-3299 | Hide all buttons except close, also clear all sessions
            'ResetAnnuityTab()
            ResetSession()
            EnableNextButton(False)
            EnablePreviousButton(False)
            EnableSaveButton(False)
            btnClose.Visible = False
            btnFinalOK.Visible = True
            'END: PPP | 01/02/2017 | YRS-AT-3299 | Hide all buttons except close, also clear all sessions
            HiddenFieldDirty.Value = "false" 'PPP | 01/23/2017 | YRS-AT-3299 | Resetting hidden field value
        Catch
            Throw
        End Try
    End Sub
#End Region

#End Region

#Region "Private Events"

    ''START: PPP | 01/20/2017 | YRS-AT-3299 | Navigation to each tab is controled by Next and Previous buttons, So tab strip index change event is not required
    ''***************************************************************************************************//
    ''Event Name                :QdroRetiredTabStrip_SelectedIndexChange  Created on  : 14/04/08    //
    ''Created By                :Dilip Patada                                  Modified On :             //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :                                                                        //
    ''Method Description        :This Event is used to set the focus on the selected page tab            //
    ''***************************************************************************************************//
    'Private Sub QdroRetiredTabStrip_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles QdroRetiredTabStrip.SelectedIndexChange

    '    Dim datagridbenifrow As Integer
    '    Dim selectedrow As Integer
    '    Dim drBenef() As DataRow
    '    Try
    '        Me.ListMultiPage.SelectedIndex = Me.QdroRetiredTabStrip.SelectedIndex
    '        'Call ClearControls(True)
    '        'If Me.DataGridRetireeList.Items.Count = 0 Then                     'Added kranthi 070808.
    '        '    Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_LIST
    '        '    Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_LIST
    '        '    MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTICIPANT_NOT_FOUND_VALIDATION, MessageBoxButtons.OK, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 

    '        'Else
    '        If QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES Then
    '            If DatagridBenificiaryList.Items.Count = 0 Then
    '                Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES
    '                Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES
    '                MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ADD_BENEFICIARY, MessageBoxButtons.OK, False) ''Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
    '                QDROParticipantInformation()                            'add kranthi 080808.
    '                Call SetControlFocus(Me.TextBoxSSNo)
    '                Exit Sub
    '            Else
    '                EnableAndDisableValidatorcontrols(False) 'CS | 2016.09.17 | YRS-AT-3081 | To Disable the Validator controls
    '                Me.ListMultiPage.SelectedIndex = Me.QdroRetiredTabStrip.SelectedIndex
    '                If Not Me.Session_Datatable_DtBenifAccount Is Nothing Then
    '                    'dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
    '                    dtBenifAccount = Me.Session_Datatable_DtBenifAccount
    '                    DropdownlistBeneficiarySSNo.DataSource = dtBenifAccount
    '                    DropdownlistBeneficiarySSNo.DataBind()
    '                    DropdownlistBeneficiarySSNo.SelectedValue = Me.string_Benif_PersonID
    '                    LoadAnnuityTab()
    '                    LoadBeneficiaries(Me.string_Benif_PersonID)
    '                    Call SetControlFocusDropDown(Me.DropdownlistBeneficiarySSNo)
    '                End If
    '            End If
    '        End If
    '        If QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES Then
    '            Me.ListMultiPage.SelectedIndex = Me.QdroRetiredTabStrip.SelectedIndex
    '            QDROParticipantInformation()                            'add kranthi 080808
    '            Call SetControlFocus(Me.TextBoxSSNo)
    '            If DatagridBenificiaryList.Items.Count = 0 Then
    '                Call LockAndUnLockRecipientControls(True)
    '            End If
    '            'If Me.DataGridRetireeList.Items.Count = 0 Then
    '            '    Me.LIstMultiPage.SelectedIndex = 0
    '            '    Me.QdroRetiredTabStrip.SelectedIndex = 0
    '            '    MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", "Participant Information not Found.", MessageBoxButtons.OK, False)
    '            'Else
    '            '    'LoadQDROBeneficieryList(Me.string_Part_SSN)
    '            'End If                                             'commented by kranthi 070808.

    '            If DatagridBenificiaryList.Items.Count > 0 Then
    '                For datagridbenifrow = 0 To DatagridBenificiaryList.Items.Count - 1
    '                    DatagridBenificiaryList.Items(datagridbenifrow).Font.Bold = False
    '                Next

    '                selectedrow = DropdownlistBeneficiarySSNo.SelectedIndex
    '                If DropdownlistBeneficiarySSNo.Items.Count > 0 Then
    '                    LoadDataSelectedBeneficiary()
    '                    DatagridBenificiaryList.Items(selectedrow).Font.Bold = True
    '                Else
    '                    DatagridBenificiaryList.Items(0).Font.Bold = True
    '                End If
    '                If (selectedrow <> -1) Then
    '                    dtBenifAccount = Me.Session_Datatable_DtBenifAccount
    '                    drBenef = dtBenifAccount.Select("ID='" & DatagridBenificiaryList.Items(selectedrow).Cells(1).Text & "'")
    '                    If (drBenef.Length <> 0) Then
    '                        Dim drDataRow As DataRow
    '                        drDataRow = drBenef.GetValue(0)
    '                        If (drDataRow("FlagNewBenf") = False) Then
    '                            ButtonEditBeneficiary.Enabled = False
    '                        Else
    '                            ButtonEditBeneficiary.Enabled = True
    '                        End If
    '                    End If
    '                    ButtonAddNewBeneficiary.Enabled = True
    '                End If
    '            Else
    '                ClearControls(True)
    '            End If

    '        End If
    '        'If QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_LIST Then
    '        '    Me.ListMultiPage.SelectedIndex = Me.QdroRetiredTabStrip.SelectedIndex
    '        '    btnAddBeneficiaryToList.Enabled = False
    '        '    Call SetControlFocus(Me.TextBoxFundNoList)
    '        '    'Else                                               'commented by kranthi 140808
    '        '    'ButtonDocumentBeneficiery.Enabled = True
    '        'End If
    '        If QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_SUMMARY Then
    '            Me.ListMultiPage.SelectedIndex = Me.QdroRetiredTabStrip.SelectedIndex

    '            If DatagridBenificiaryList.Items.Count = 0 Then             'checking for benificiary's add kranthi 070808.
    '                Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES
    '                Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES
    '                MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ADD_BENEFICIARY, MessageBoxButtons.OK, False) ''Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
    '                QDROParticipantInformation()                            'add kranthi 080808.
    '                Exit Sub
    '            End If

    '            If Not Me.Session_Datatable_DtRecptAccount Is Nothing Then
    '                'dtRecptAccount = CType(Me.Session_datatable_dtRecptAccount, DataTable)
    '                dtRecptAccount = Me.Session_Datatable_DtRecptAccount

    '                If dtRecptAccount.Rows.Count = 0 Then
    '                    Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES
    '                    Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES
    '                    MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_SPLIT_NOT_FOUND_VALIDATION, MessageBoxButtons.OK, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
    '                    'dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
    '                    dtBenifAccount = Me.Session_Datatable_DtBenifAccount
    '                    DropdownlistBeneficiarySSNo.DataSource = dtBenifAccount
    '                    DropdownlistBeneficiarySSNo.DataBind()
    '                    DropdownlistBeneficiarySSNo.SelectedValue = Me.string_Benif_PersonID
    '                    LoadAnnuityTab()
    '                    Exit Sub
    '                Else
    '                    LoadSummaryTab()
    '                End If
    '            ElseIf Me.Session_Datatable_DtRecptAccount Is Nothing Then   'Added kranthi 070808.
    '                Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES
    '                Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES
    '                MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_SPLIT_NOT_FOUND_VALIDATION, MessageBoxButtons.OK, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
    '                'dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
    '                dtBenifAccount = Me.Session_Datatable_DtBenifAccount

    '                DropdownlistBeneficiarySSNo.DataSource = dtBenifAccount
    '                DropdownlistBeneficiarySSNo.DataBind()
    '                DropdownlistBeneficiarySSNo.SelectedValue = Me.string_Benif_PersonID
    '                LoadAnnuityTab()
    '                Exit Sub
    '            End If
    '        End If
    '        'End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("QdroRetiredTabStrip_SelectedIndexChange", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try

    'End Sub
    'END: PPP | 01/20/2017 | YRS-AT-3299 | Navigation to each tab is controled by Next and Previous buttons, So tab strip index change event is not required

    '***************************************************************************************************//
    'Event Name                :ButtonDocumentBeneficiery_Click               Created on  : 14/04/08    //
    'Created By                :Dilip Patada                                  Modified On :             //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Event is used to add the beneficiery to the beneficiery grid for   // 
    '                           the participant it also check for the for duplicate SSNO in the         //
    '                           beneficiery Grid before adding 
    '***************************************************************************************************//

    Private Sub ButtonDocumentBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBeneficiaryToList.Click
        Dim l_string_Part_SSN As String = String.Empty
        Dim drBenifAccount As DataRow
        Dim iCount As Integer = 0
        Dim l_bool_isTrueAddress As Boolean = False
        Dim l_bool_isTrue As Boolean = True 'added kranthi 050808
        'Dim l_Char_IsSpouse As Char         'added kranthi 141008
        Dim strWSMessage As String
        Dim stTelephoneError As String 'PPP | 2015.10.13 | YRS-AT-2588

        Try
            If Me.IsValid Then                 'commented kranthi 171008
                l_string_Part_SSN = Me.String_Part_SSN
                l_string_Part_SSN = l_string_Part_SSN.Substring(0, l_string_Part_SSN.IndexOf("-"))
                'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
                If Session("R_QDRO_PersID") <> Nothing Then
                    'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
                    strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("R_QDRO_PersID"))
                    If strWSMessage <> "NoPending" Then
                        Page.ClientScript.RegisterStartupScript(Me.GetType(), "openWSMsgBox", "openDialog('" + strWSMessage + "','Bene');", True) 'PPP | 01/24/2017 | YRS-AT-3299 | Changed "key" parameter value from "YRS" to "openWSMsgBox"
                        Exit Sub
                    End If
                End If
                'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application

                If TextBoxSSNo.Text.Equals(l_string_Part_SSN) Then  'CType(TextBoxSpouseSSNo.Text, String) = CType(TextBoxPerSSNo.Text, String) Then
                    'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                    'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_RECIPIENT_DUPLICATE_SSNO_VALIDATION, MessageBoxButtons.OK, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
                    ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_RECIPIENT_DUPLICATE_SSNO_VALIDATION, "error")
                    'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                    Exit Sub
                End If

                'Anudeep:2013.06.20-BT-1555:YRS 5.0-1769:Length of phone numbers
                If Me.Session_Bool_NewPerson = True Then
                    If AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA" Then
                        'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                        'If TextboxSpouseTel.Text.Trim.Length <> 10 And TextboxSpouseTel.Text.Trim.Length > 0 Then
                        '    'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numberss
                        '    MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Telephone number must be 10 digits.", MessageBoxButtons.Stop)
                        '    Exit Sub
                        'End If
                        If TextBoxTel.Text.Trim.Length > 0 Then
                            stTelephoneError = Validation.Telephone(TextBoxTel.Text.Trim, YMCAObjects.TelephoneType.Telephone)
                            If (Not String.IsNullOrEmpty(stTelephoneError)) Then
                                'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                                'MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", stTelephoneError, MessageBoxButtons.Stop)
                                ShowModalPopupWithCustomMessage("QDRO", stTelephoneError, "error") 'MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "YMCA-YRS" to "QDRO"
                                'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                                Exit Sub
                            End If
                        End If
                        'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    End If
                End If
                'add kranthi 290808 checking the length of the SSNo.
                If TextBoxSSNo.Text.Length <> 9 Then
                    Exit Sub
                End If
                'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | checking for dropdowm selected value
                If DropDownListGender.SelectedValue = "SEL" Or DropDownListMaritalStatus.SelectedValue = "SEL" Then
                    Exit Sub
                End If
                'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | checking for dropdowm selected value
                ' Added by dilip on 22-12-2008 This function will check the manditory fields values entered or not for
                ' ExistingParticipent and display message.
                If Me.Session_Bool_NewPerson = False Then
                    l_bool_isTrueAddress = CheckMandatoryExistingParticipant()
                    If l_bool_isTrueAddress = False Then
                        'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", String.Format(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ALTERNATE_PAYEE_SSNO_ALREADY_EXISTS, TextBoxSSNo.Text.Trim()), MessageBoxButtons.OK, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
                        ShowModalPopupWithCustomMessage("QDRO", String.Format(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ALTERNATE_PAYEE_SSNO_ALREADY_EXISTS, TextBoxSSNo.Text.Trim()), "error")
                        'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        Exit Sub
                    End If
                End If
                ' Added by dilip This function will check the manditory fields values entered or not for
                ' ExistingParticipent and display message.               
                If Me.Session_Bool_NewPerson = True Then
                    l_bool_isTrue = CheckMandatory()
                End If
                If l_bool_isTrue = False Then
                    Exit Sub
                ElseIf l_bool_isTrue = True Then

                    'SP: 2013.10.16 : BT-2175 : Non retired Qdro split will throws error if benefeciary name is more than person table -Start
                    If (TextBoxFirstName.Text.Trim().Length > 20) Then
                        TextBoxFirstName.Enabled = True
                        'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        'MessageBox.Show(PlaceHolderACHDebitImportProcess, "Beneficiary", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_BENEFICIARY_FIRST_NAME_MAX_LENGTH, MessageBoxButtons.OK, False)
                        ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_BENEFICIARY_FIRST_NAME_MAX_LENGTH, "error") 'MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "Beneficiary" to "QDRO"
                        'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        Exit Sub
                    ElseIf (TextBoxMiddleName.Text.Trim.Length > 20) Then
                        TextBoxMiddleName.Enabled = True
                        'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        'MessageBox.Show(PlaceHolderACHDebitImportProcess, "Beneficiary", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_BENEFICIARY_MIDDLE_NAME_MAX_LENGTH, MessageBoxButtons.OK, False)
                        ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_BENEFICIARY_MIDDLE_NAME_MAX_LENGTH, "error") 'MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "Beneficiary" to "QDRO"
                        'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        Exit Sub
                    ElseIf (TextBoxLastName.Text.Trim.Length > 30) Then
                        TextBoxLastName.Enabled = True
                        'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        'MessageBox.Show(PlaceHolderACHDebitImportProcess, "Beneficiary", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_BENEFICIARY_LAST_NAME_MAX_LENGTH, MessageBoxButtons.OK, False)
                        ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_BENEFICIARY_LAST_NAME_MAX_LENGTH, "error") 'MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "Beneficiary" to "QDRO"
                        'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        Exit Sub
                    End If

                    'SP: 2013.10.16 : BT-2175 : Non retired Qdro split will throws error if benefeciary name is more than person table -End

                    'added kranthi 141008
                    'If CheckboxIsSpouse.Checked = True Then
                    '    l_Char_IsSpouse = "D"
                    'Else
                    '    l_Char_IsSpouse = "S"
                    'End If
                    Dim strState As String = String.Empty
                    Dim strCountry As String = String.Empty
                    'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
                    If AddressWebUserControl1.DropDownListStateValue = "- Select -" Then
                        strState = ""
                    Else
                        strState = AddressWebUserControl1.DropDownListStateValue
                    End If

                    If AddressWebUserControl1.DropDownListCountryValue = "- Select -" Then
                        strCountry = ""
                    Else
                        strCountry = AddressWebUserControl1.DropDownListCountryValue
                    End If
                    'End 10-Dec-2010:Commenetd as sate n country fill up with javascript

                    If Me.Session_Datatable_DtBenifAccount Is Nothing Then
                        dtBenifAccount = CreateDataTableBenfAccount()

                        'changed kranthi 091908
                        If btnAddBeneficiaryToList.Text = "Save Recipient" Then 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Add To List" to "Save Recipient"
                            'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
                            'AddDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'Anudeep:18.07.2013:BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
                            'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Passing gender and marital status to the method.
                            'AddDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'START: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not passing it as an parameter, hardcoded it inside in the function
                            'AddDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSal.SelectedItem.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            AddDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSal.SelectedItem.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'END: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not passing it as an parameter, hardcoded it inside in the function
                            'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Passing gender and marital status to the method.
                            'START: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition
                            ''Added kranthi 101008
                            'Me.Session_String_ISAddUpdate = "Add To List"
                            'END: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition

                            'START: PPP | 01/23/2017 | YRS-AT-3299 | Message is hanlded by Modal popup 
                            ''Added kranthi
                            'Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = True 'PPP | 01/23/2017 | YRS-AT-3299 | Tabs are handled by Next and Previous button
                            'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ADD_ANOTHER_BENEFICIARY_CONFIRMATION, MessageBoxButtons.YesNo, False)
                            ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ADD_ANOTHER_BENEFICIARY_CONFIRMATION, "infoYesNo")
                            'END: PPP | 01/23/2017 | YRS-AT-3299 | Message is hanlded by Modal popup 
                            EnableNextButton(True) 'PPP | 01/23/2017 | YRS-AT-3299 | Enable the next button
                            HiddenFieldDirty.Value = "false" 'PPP | 01/23/2017 | YRS-AT-3299 | Resetting hidden field value
                            'START: PPP | 01/23/2017 | YRS-AT-3299 | btnAddBeneficiaryToList.Text checking should be handled by "If and ElseIf" set
                            'End If 
                            'If btnAddBeneficiaryToList.Text = "Update Recipient" Then
                        ElseIf btnAddBeneficiaryToList.Text = "Update Recipient" Then
                            'END: PPP | 01/23/2017 | YRS-AT-3299 | btnAddBeneficiaryToList.Text checking should be handled by "If and ElseIf" set
                            'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
                            'UpdateDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'Anudeep:18.07.2013:BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
                            'Start - Manthan Rajguru | 2016.08.16 | Passing marital status and gender value to the update method.
                            'UpdateDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'START: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not passing it as an parameter, hardcoded it inside in the function
                            'UpdateDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSal.SelectedItem.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            UpdateDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSal.SelectedItem.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'END: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not passing it as an parameter, hardcoded it inside in the function
                            'End - Manthan Rajguru | 2016.08.16 | Passing marital status and gender value to the update method.
                            ButtonEditBeneficiary.Enabled = False
                            ButtonCancelBeneficiary.Enabled = False
                            'ButtonAddNewBeneficiery.Enabled = True
                            'START: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition
                            ''Added kranthi 101008
                            'Me.Session_String_ISAddUpdate = "Update To List"
                            'END: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition
                            'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                            'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_UPDATED_SUCCESSFULLY, MessageBoxButtons.OK, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
                            ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_UPDATED_SUCCESSFULLY, "ok")
                            'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                            EnableNextButton(True) 'PPP | 01/23/2017 | YRS-AT-3299 | Enable the next button
                            HiddenFieldDirty.Value = "false" 'PPP | 01/23/2017 | YRS-AT-3299 | Resetting hidden field value
                        End If

                        'START: PPP | 01/23/2017 | YRS-AT-3299 | LoadRecipientGrid function binds data to Recipient Grid as well as to annuities dropdown also
                        ''changed kranthi 091908
                        'DatagridBenificiaryList.DataSource = dtBenifAccount
                        'DatagridBenificiaryList.DataBind()
                        LoadRecipientGrid(dtBenifAccount)
                        'END: PPP | 01/23/2017 | YRS-AT-3299 | LoadRecipientGrid function binds data to Recipient Grid as well as to annuities dropdown also

                        'DropdownlistSpouseSal.SelectedItem.Value = ""
                        'lockRecipientControls(True)
                        ManageEditableControls(False) 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling gender and marital control

                        'ButtonAddNewBeneficiery.Enabled = True
                        btnAddBeneficiaryToList.Enabled = False
                        ButtonCancelBeneficiary.Enabled = False

                        If btnAddBeneficiaryToList.Text = "Update Recipient" Then 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Update To List" to "Update Recipient"
                            btnAddBeneficiaryToList.Text = "Save Recipient" 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Add To List" to "Save Recipient"
                        End If

                    Else
                        'changed kranthi 091908
                        If btnAddBeneficiaryToList.Text = "Save Recipient" Then 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Add To List" to "Save Recipient"
                            'dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
                            dtBenifAccount = Me.Session_Datatable_DtBenifAccount

                            For iCount = 0 To dtBenifAccount.Rows.Count - 1
                                drBenifAccount = dtBenifAccount.Rows(iCount)
                                If (drBenifAccount.Item("SSNo") = TextBoxSSNo.Text.Trim()) Then
                                    'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                                    'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_BENEFICIARY_ALREADY_EXISTS, MessageBoxButtons.OK, False)
                                    ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_BENEFICIARY_ALREADY_EXISTS, "error")
                                    'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                                    Exit Sub
                                End If
                            Next
                            'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
                            'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Passing the marital status and gender to the method.
                            'AddDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'START: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not passing it as an parameter, hardcoded it inside in the function
                            'AddDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSal.SelectedItem.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            AddDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSal.SelectedItem.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'END: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not passing it as an parameter, hardcoded it inside in the function
                            'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Passing the marital status and gender to the method.
                            'AddDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'START: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition
                            ''Added kranthi 101008
                            'Me.Session_String_ISAddUpdate = "Add To List"
                            'END: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition
                            'START: PPP | 01/23/2017 | YRS-AT-3299 | Message is hanlded by Modal popup 
                            'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ADD_ANOTHER_BENEFICIARY_CONFIRMATION, MessageBoxButtons.YesNo, False)
                            ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ADD_ANOTHER_BENEFICIARY_CONFIRMATION, "infoYesNo")
                            'END: PPP | 01/23/2017 | YRS-AT-3299 | Message is hanlded by Modal popup 
                            EnableNextButton(True) 'PPP | 01/23/2017 | YRS-AT-3299 | Enable the next button
                            HiddenFieldDirty.Value = "false" 'PPP | 01/23/2017 | YRS-AT-3299 | Resetting hidden field value
                            'START: PPP | 01/23/2017 | YRS-AT-3299 | btnAddBeneficiaryToList.Text checking should be handled by "If and ElseIf" set
                            'End If 
                            'If btnAddBeneficiaryToList.Text = "Update Recipient" Then
                        ElseIf btnAddBeneficiaryToList.Text = "Update Recipient" Then
                            'END: PPP | 01/23/2017 | YRS-AT-3299 | btnAddBeneficiaryToList.Text checking should be handled by "If and ElseIf" set
                            'dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
                            dtBenifAccount = Me.Session_Datatable_DtBenifAccount
                            'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
                            'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Passing marital status and gender value to the update.
                            'UpdateDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'START: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not passing it as an parameter, hardcoded it inside in the function
                            'UpdateDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSal.SelectedItem.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            UpdateDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSal.SelectedItem.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'END: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not passing it as an parameter, hardcoded it inside in the function
                            'UpdateDataToTable(Me.String_RecptRetireeID, Me.String_RecptPersId, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "RQ", Convert.ToBoolean(Me.Session_Bool_NewPerson), Me.String_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                            'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Passing marital status and gender value to the update.
                            ButtonEditBeneficiary.Enabled = False
                            ButtonCancelBeneficiary.Enabled = False
                            'ButtonAddNewBeneficiery.Enabled = True
                            'START: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition
                            ''Added kranthi 101008
                            'Me.Session_String_ISAddUpdate = "Update To List"
                            'END: PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition
                            'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                            'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_UPDATED_SUCCESSFULLY, MessageBoxButtons.OK, False) ''Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
                            ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_UPDATED_SUCCESSFULLY, "ok")
                            'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                            EnableNextButton(True) 'PPP | 01/23/2017 | YRS-AT-3299 | Enable the next button
                            HiddenFieldDirty.Value = "false" 'PPP | 01/23/2017 | YRS-AT-3299 | Resetting hidden field value
                        End If
                        'START: PPP | 01/23/2017 | YRS-AT-3299 | LoadRecipientGrid function binds data to Recipient Grid as well as to annuities dropdown also
                        ''changed kranthi 091908
                        'DatagridBenificiaryList.DataSource = dtBenifAccount
                        'DatagridBenificiaryList.DataBind()
                        LoadRecipientGrid(dtBenifAccount)
                        'END: PPP | 01/23/2017 | YRS-AT-3299 | LoadRecipientGrid function binds data to Recipient Grid as well as to annuities dropdown also
                        'DropdownlistSpouseSal.SelectedItem.Value = ""
                        'lockRecipientControls(True)
                        ManageEditableControls(False) 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling gender and marital control

                        'ButtonAddNewBeneficiery.Enabled = True
                        btnAddBeneficiaryToList.Enabled = False
                        ButtonCancelBeneficiary.Enabled = False

                        If btnAddBeneficiaryToList.Text = "Update Recipient" Then 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Update To List" to "Update Recipient"
                            btnAddBeneficiaryToList.Text = "Save Recipient" 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Add To List" to "Save Recipient"
                        End If

                    End If
                    'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling Gender and marital dropdown control
                    DropDownListGender.Enabled = False
                    DropDownListMaritalStatus.Enabled = False
                    'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling Gender and marital dropdown control
                    'added kranthi 071008
                    'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                    'If RadioButtonListSplitAmtType.Enabled = False Then
                    '    RadioButtonListSplitAmtType.Enabled = True
                    '    RadioButtonListSplitAmtType.SelectedValue = "Percentage"
                    '    TextBoxAmountWorkSheet.Enabled = False
                    '    TextBoxPercentageWorkSheet.Enabled = True
                    '    RadioButtonListPlanType.Enabled = True
                    'End If
                    'Commented by END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                    'added kranthi 071008

                    btnAddBeneficiaryToList.Enabled = False
                    ButtonAddNewBeneficiary.Enabled = True
                    'End If

                    Me.Session_Datatable_DtBenifAccount = dtBenifAccount
                    EnableAndDisableValidatorcontrols(False) 'CS | 2016.09.17 | YRS-AT-3081 | To Disable the Validator controls
                    LockAndUnLockRecipientControls(False) 'PPP | 01/24/2017 | YRS-AT-3299 | Disable all controls
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ButtonDocumentBeneficiary_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub
    '***************************************************************************************************//
    'Event Name                :ButtonReset_Click                             Created on  : 25/04/08    //
    'Created By                :Dilip Patada                                  Modified On :             //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Event is used to Reset the beneficiery(Recipent grid in Annuity Tab//
    '                           Reset the selected split type Recipient Annuities for the Person        //
    '                           Adding the Splited Recipient Annuities to the Participant Annuities     //
    '***************************************************************************************************//
    Private Sub ButtonReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonReset.Click
        'dtPartAccount = CType(Me.Session_datatable_dtPartAccount, DataTable)
        'dtRecptAccount = CType(Me.Session_datatable_dtRecptAccount, DataTable)
        'dtRecptAccounttemp = CType(Me.Session_datatable_dtRecptAccounttemp, DataTable)
        'dtPartTotal = CType(Me.Session_datatable_dtPartTotal, DataTable)


        Try
            ResetAnnuitiesDetails()


            'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            ' ButtonAdjust.Enabled = False
            ' ButtonReset.Enabled = False
            'RadioButtonListSplitAmtType.Enabled = True
            'TextBoxAmountWorkSheet.Text = "0.00"
            'TextBoxPercentageWorkSheet.Text = "0.00"
            'RadioButtonListSplitAmtType.Enabled = True
            'RadioButtonListSplitAmtType.SelectedValue = "Percentage"
            'TextBoxPercentageWorkSheet.Enabled = True
            'ButtonSplit.Enabled = True
            'If RadioButtonListSplitAmtType.Items(0).Selected = True Then
            '    TextBoxAmountWorkSheet.Text = "0.00"
            'ElseIf RadioButtonListSplitAmtType.Items(1).Selected = True Then
            '    TextBoxPercentageWorkSheet.Text = "0.00"
            'End If
            '  RadioButtonListPlanType.Enabled = True
            'If dtRecptAccount.Rows.Count = 0 Then
            '    Me.Session_Datatable_DtRecptAccount = Nothing
            '    'Added by Dilip
            '    DatagridParticipantsBalance.DataSource = Nothing
            '    DatagridParticipantsBalance.DataBind()
            '    'Added by Dilip
            '    'RadioButtonListPlanType.Enabled = True
            'ButtonDocumentSave.Enabled = False
            ' ButtonShowBalance.Enabled = False                       'add kranthi 080808.
            '    Me.QdroRetiredTabStrip.Items(3).Enabled = False
            'Else
            '    ButtonDocumentSave.Enabled = True
            'End If
            'If CheckBoxSpecialDividends.Checked = True Then             'if the check box is checked then un checking. add kranthi 080808.
            '    CheckBoxSpecialDividends.Checked = False
            'End If
            'Commented by END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

        Catch ex As Exception
            HelperFunctions.LogException("ButtonReset_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Old button
    '***************************************************************************************************//
    ''Event Name                :ButtonDocumentCancel_Click                    Created on  : 25/04/08    //
    ''Created By                :Dilip Patada                                  Modified On :             //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :                                                                        //
    ''Method Description        :This Event is used to cancel all Split Recipent grid in Annuity Tab)    //
    ''***************************************************************************************************//
    'Private Sub ButtonDocumentCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDocumentCancel.Click
    '    'dtRecptAccount = CType(Me.Session_datatable_dtRecptAccount, DataTable)
    '    'dtRecptAccounttemp = CType(Me.Session_datatable_dtRecptAccounttemp, DataTable)

    '    dtRecptAccount = Me.Session_Datatable_DtRecptAccount
    '    dtRecptAccounttemp = Me.Session_Datatable_DtRecptAccountTemp

    '    If Not dtRecptAccount Is Nothing Then
    '        dtRecptAccount = Nothing
    '        dtRecptAccounttemp = Nothing
    '        Try
    '            DataGridRecipientAnnuitiesBalance.DataSource = dtRecptAccounttemp
    '            DataGridRecipientAnnuitiesBalance.DataBind()
    '            DatagridParticipantsBalance.DataSource = dtPartAccount
    '            DatagridParticipantsBalance.DataBind()
    '            Me.Session_Datatable_DtPartAccount = dtPartAccount
    '            Me.Session_Datatable_DtRecptAccount = dtRecptAccount
    '            Me.Session_Datatable_DtRecptAccountTemp = dtRecptAccounttemp
    '            ButtonAdjust.Enabled = False
    '            ButtonReset.Enabled = False
    '            ButtonSplit.Enabled = True
    '            'If RadioButtonListSplitAmtType.Enabled = False Then
    '            '    RadioButtonListSplitAmtType.Enabled = True
    '            '    RadioButtonListSplitAmtType.SelectedValue = "Percentage"
    '            '    TextBoxPercentageWorkSheet.Enabled = True
    '            'End If
    '            'If RadioButtonListSplitAmtType.Items(0).Selected = True Then
    '            '    TextBoxAmountWorkSheet.Text = "0.00"
    '            'ElseIf RadioButtonListSplitAmtType.Items(1).Selected = True Then
    '            '    TextBoxPercentageWorkSheet.Text = "0.00"
    '            'End If
    '            'ButtonDocumentSave.Enabled = False 'PPP | 01/24/2017 | YRS-AT-3299 | Old button
    '            'add kranthi 070808.
    '            'RadioButtonListPlanType.Enabled = True
    '            CheckBoxSpecialDividends.Checked = False
    '            'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '            Me.SplitPlanTypeOption = Nothing
    '            Me.ViewState_Datatable_DtPartCurrentAccount = Nothing
    '            'End  - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    '            ButtonShowBalance.Enabled = False                       'add kranthi 080808.
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = False
    '            'Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_LIST
    '            'Me.ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_LIST
    '            ResetAnnuityTab()
    '            ResetSession()
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES).Enabled = False
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = False
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = False
    '            Me.string_Benif_PersonID = Nothing
    '            Me.DropdownlistBeneficiarySSNo.SelectedIndex = 0
    '        Catch ex As Exception
    '            Dim l_String_Exception_Message As String
    '            HelperFunctions.LogException("ButtonDocumentCancel_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '            l_String_Exception_Message = ex.Message.Trim.ToString()
    '            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '        End Try
    '    End If
    'End Sub

    ''***************************************************************************************************//
    ''Event Name                :ButtonDocumentOK_Click                        Created on  : 25/04/08    //
    ''Created By                :Dilip Patada                                  Modified On :             //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :                                                                        //
    ''Method Description        :This Event is used to Close Current Page clear all session Variable and //
    ''                           Redirect to MainWebForm                                                 //
    ''***************************************************************************************************//
    'Private Sub ButtonDocumentOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDocumentOK.Click
    '    'Call ClearSession()
    '    'Start - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Added try catch block for Logging exception error message
    '    Try
    '        Session("RefundRequest_Sort") = Nothing
    '        'START: MMR | 2016.09.13 | YRS-AT-2482 | Commented existing code and added parameter to avoid error while redirecting to main page
    '        'Response.Redirect("MainWebForm.aspx")
    '        'Response.Redirect("MainWebForm.aspx", False)
    '        'END: MMR | 2016.09.13 | YRS-AT-2482 | Commented existing code and added parameter to avoid error while redirecting to main page
    '        Response.Redirect("FindInfo.aspx?Name=RetiredQdro", False)
    '        Session("Page") = "RetiredQdro"
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("ButtonDocumentOK_Click", ex)
    '        l_String_Exception_Message = ex.Message.Trim.ToString()
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    '    'End - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Added try catch block for Logging exception error message
    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Old button

    '***************************************************************************************************  //
    'Event Name                :ButtonSplit_Click                             Created on  : 28/04/08      //
    'Created By                :Dilip Patada                                  Modified On :               //
    'Modified By               :                                                                          //
    'Modify Reason             :                                                                          //
    'Param Description         :                                                                          //
    'Method Description        :This Event is used to split the participant account balance to beneficiery//
    '                           and show this in recipant Annuity grid & remaining balance in Participant //
    '                           Annuity Grid                                                              //  
    'Chandra sekar-2016.08.22-YRS-AT-3081 - DataGridWorkSheet2 rename to DataGridRecipientAnnuitiesBalance
    '***************************************************************************************************  //

    Private Sub ButtonSplit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSplit.Click
        Dim drRecps() As DataRow
        Dim selectedTotalAnnutiesAmount As Decimal
        'START: PPP | 01/20/2017 | YRS-AT-3299 
        Dim isAmountSuccessfullyConvertedToPercentage As Boolean ' renamed old variable "bool_ConvertToPercentage" as isAmountSuccessfullyConvertedToPercentage
        Dim invalidAnnuitiesMessage As String
        Dim isSplitValid As Boolean
        Dim amount, percentage As Decimal
        Dim isSplitByAmount, isSplitByPercentage As Boolean
        'END: PPP | 01/20/2017 | YRS-AT-3299
        Try
            'START: PPP | 01/20/2017 | YRS-AT-3299 | Following validations will take first before performing the split
            '1. If participant's account is locked then reason will be displayed
            '2. If no annuities exists for selected plan then error message will be displayed.
            '3. If no annuities selected (all checkboxes are unchecked) then error message will be displayed.
            '4. If amount or percentage textbox are empty or contain the 0.00 value then error message will be displayed.
            '5. If annuities are not valid then error message will be displayed.
            'END: PPP | 01/20/2017 | YRS-AT-3299 | Following validations will take first before performing the split

            isSplitValid = False

            'START: Checking split amount and percentage
            amount = 0
            percentage = 0
            isSplitByAmount = False
            isSplitByPercentage = False
            If RadioButtonListSplitAmtType_Amount.Checked Then
                If TextBoxAmountWorkSheet.Text.Trim().Length > 0 AndAlso Decimal.TryParse(TextBoxAmountWorkSheet.Text, amount) Then
                    isSplitByAmount = True
                    isAmountSuccessfullyConvertedToPercentage = ConvertToPercentage()
                    If Not isAmountSuccessfullyConvertedToPercentage Then       'add kranthi 010908 if the amount is exciding than current payment.
                        ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_CHECK_SPLIT_AMOUNT_VALIDATION, "error")
                        Exit Sub
                    End If
                End If
            ElseIf RadioButtonListSplitAmtType_Percentage.Checked Then
                If TextBoxPercentageWorkSheet.Text.Trim().Length > 0 AndAlso Decimal.TryParse(TextBoxPercentageWorkSheet.Text, percentage) Then
                    isSplitByPercentage = True
                    PercentageForSplit = Convert.ToDouble(TextBoxPercentageWorkSheet.Text.Trim())
                End If
            End If
            'END: Checking split amount and percentage

            'START: Performing validations
            If Not Me.IsAccountLock = Nothing AndAlso Me.IsAccountLock Then
                ShowModalPopupWithCustomMessage("QDRO", GetLockReason(), "error") 'MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "YMCA-YRS" to "QDRO"
            ElseIf DataGridWorkSheet.Items.Count = 0 Then
                ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_NO_AMOUNT_ANNUITY, "error")
            ElseIf Not CheckValidate() Then
                ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_SELECT_ANNUITY_SPLIT, "error")
            ElseIf Not isSplitByAmount And Not isSplitByPercentage Then
                ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_AMOUNT_PERCENTAGE_VALIDATION, "error")
            Else
                isSplitValid = True
            End If
            'END: Performing validations

            If isSplitValid Then
                invalidAnnuitiesMessage = CheckIfAnnuityIsValid(DataGridWorkSheet)
                If Not String.IsNullOrEmpty(invalidAnnuitiesMessage) Then
                    ShowModalPopupWithCustomMessage("QDRO", invalidAnnuitiesMessage, "error")
                    isSplitValid = False
                End If
            End If

            If isSplitValid Then
                ' everything is ok, begin to split selected annuities

                Dim dtPartOriginalAnnuity As DataTable = Me.Session_Datatable_DtPartTotal
                Dim dtRecptAccount As DataTable = Me.Session_Datatable_DtRecptAccount
                Dim dtPartRemainingBalance = AddParticipantAnnuitiesDetails(Me.Session_Datatable_DtPartAccount, Me.ViewState_Datatable_DtPartSelectedAccount)

                If (HelperFunctions.isEmpty(dtRecptAccount)) Then dtRecptAccount = CreateRecipientTable()
                If (HelperFunctions.isEmpty(dtPartAccount)) Then dtPartAccount = CreateParticipantTable()

                selectedTotalAnnutiesAmount = CalculateSelectedAmount()

                Dim dtCurrentBeneficiary As DataTable = ReadCurrentData(dtRecptAccount, dtPartOriginalAnnuity, selectedTotalAnnutiesAmount, PercentageForSplit)

                Dim strMessage As String = ValidateSplitParameters(dtPartOriginalAnnuity, dtRecptAccount, dtCurrentBeneficiary)
                If (String.IsNullOrEmpty(strMessage) = False) Then
                    ShowModalPopupWithCustomMessage("QDRO", strMessage, "error")
                    Exit Sub
                End If

                Dim dtParticipantAnnuitiesRemainingBalances As DataTable = dtPartRemainingBalance.Copy
                Dim dtCopyRecptExistingSplit As DataTable = dtRecptAccount.Copy()
                DetermineValueSplit(dtPartOriginalAnnuity, dtCurrentBeneficiary, dtCopyRecptExistingSplit, dtParticipantAnnuitiesRemainingBalances)

                ProRateAdjustment(dtPartOriginalAnnuity, dtCopyRecptExistingSplit, dtParticipantAnnuitiesRemainingBalances, dtCurrentBeneficiary)


                If ValidateAdjustment(dtPartOriginalAnnuity, dtParticipantAnnuitiesRemainingBalances, dtCopyRecptExistingSplit, dtCurrentBeneficiary, DropdownlistBeneficiarySSNo.SelectedValue.Trim) = False Then
                    LogTraceInformation(dtPartOriginalAnnuity, dtParticipantAnnuitiesRemainingBalances, dtCopyRecptExistingSplit, dtCurrentBeneficiary, DropdownlistBeneficiarySSNo.SelectedValue.Trim)
                    ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_CHECK_SPLIT_PERCENTAGE_VALIDATION, "error")
                    Exit Sub
                End If


                dtRecptAccount = dtCurrentBeneficiary

                drRecps = dtRecptAccount.Select("RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")

                Dim dtRecipientAnnuitiesBalances As DataTable = CreateRecipientTableTemp()
                'its add records into dtRecptAccounttemp
                AddDataToRecptAccounttemp(drRecps, selectedTotalAnnutiesAmount, dtRecipientAnnuitiesBalances)


                Me.Session_Datatable_DtPartAccount = AddUpdateParticipantAnnuitiesDetails(Me.Session_Datatable_DtPartAccount, dtParticipantAnnuitiesRemainingBalances)

                'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                Me.ViewState_Datatable_DtPartCurrentAccount = dtParticipantAnnuitiesRemainingBalances
                'Below Logic to Update the Recipient Annuities
                If Not Me.Session_Datatable_DtRecptAccount Is Nothing Then
                    If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtRecptAccount) Then
                        Me.Session_Datatable_DtRecptAccount = AddRecipientAnnuitiesDetails(Me.Session_Datatable_DtRecptAccount, dtCurrentBeneficiary, Me.SplitPlanTypeOption)
                    Else
                        Me.Session_Datatable_DtRecptAccount = dtRecptAccount
                    End If
                Else
                    Me.Session_Datatable_DtRecptAccount = dtRecptAccount
                End If
                ShowHideControls()

                If Not Me.Session_Datatable_DtRecptAccountTemp Is Nothing Then
                    If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtRecptAccountTemp) Then
                        Me.Session_Datatable_DtRecptAccountTemp = AddUpdateParticipantAnnuitiesDetails(Me.Session_Datatable_DtRecptAccountTemp, dtRecipientAnnuitiesBalances)
                    Else
                        Me.Session_Datatable_DtRecptAccountTemp = dtRecipientAnnuitiesBalances
                    End If
                Else
                    Me.Session_Datatable_DtRecptAccountTemp = dtRecipientAnnuitiesBalances
                End If
            End If

            'START: PPP | 01/20/2017 | YRS-AT-3299 | Old split code commented
            ''--------------------------------------------------------------------------------------
            ''Shashi Shekhar:10 Feb 2011:  For YRS 5.0-1236 : Need ability to freeze/lock account
            ''Shashi Shekhar     14 - Feb -2011      For BT-750 While QDRO split message showing wrong.
            'If Not Me.IsAccountLock = Nothing Then
            '    If Me.IsAccountLock.ToString.Trim.ToLower = "true" Then

            '        Dim l_dsLockResDetails As DataSet
            '        Dim l_reasonLock As String
            '        If Not Me.ParticipantSSN = String.Empty Then
            '            l_dsLockResDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetLockReasonDetails(Me.ParticipantSSN.ToString.Trim)
            '        End If

            '        If Not l_dsLockResDetails Is Nothing Then
            '            If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows.Count > 0 Then
            '                If (l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "System.DBNull" And l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "") Then

            '                    l_reasonLock = l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString().Trim
            '                End If
            '            End If
            '        End If
            '        If l_reasonLock = "" Then
            '            MessageBox.Show(PlaceHolderACHDebitImportProcess, " YMCA - YRS", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTICIPANT_ACCOUNT_LOCK, MessageBoxButtons.Stop, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
            '        Else
            '            MessageBox.Show(PlaceHolderACHDebitImportProcess, " YMCA - YRS", String.Format(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTICIPANT_ACCOUNT_LOCK_REASON, l_reasonLock), MessageBoxButtons.Stop, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file  
            '        End If

            '        Exit Sub
            '    End If
            'End If

            ''-----------------------------------------------------------------------------
            'Dim dtRecipientAnnuitiesBalances As DataTable = CreateRecipientTableTemp()

            'DataGridRecipientAnnuitiesBalance.DataSource = Nothing
            'DataGridRecipientAnnuitiesBalance.DataBind()
            'Dim strValidMessage As String = CheckIfAnnuityIsValid(DataGridWorkSheet)
            'If Not (String.IsNullOrEmpty(strValidMessage)) Then
            '    MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", strValidMessage, MessageBoxButtons.Stop, False)
            '    Exit Sub
            'End If

            'If ((RadioButtonListSplitAmtType.Items(1).Selected = True And TextBoxPercentageWorkSheet.Text <> String.Empty) Or (RadioButtonListSplitAmtType.Items(0).Selected = True And TextBoxAmountWorkSheet.Text <> String.Empty)) And CheckValidate() Then
            '    If (RadioButtonListSplitAmtType.Items(1).Selected = True And TextBoxPercentageWorkSheet.Text <> String.Empty) Then
            '        PercentageForSplit = Convert.ToDouble(TextBoxPercentageWorkSheet.Text.Trim())
            '    ElseIf (RadioButtonListSplitAmtType.Items(0).Selected = True And TextBoxAmountWorkSheet.Text <> String.Empty) Then
            '        bool_ConvertToPercentage = ConvertToPercentage()
            '        If bool_ConvertToPercentage = False Then       'add kranthi 010908 if the amount is exciding than current payment.
            '            MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_CHECK_SPLIT_AMOUNT_VALIDATION, MessageBoxButtons.Stop, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
            '            Exit Sub
            '        End If

            '    End If

            '    Dim dtPartOriginalAnnuity As DataTable = Me.Session_Datatable_DtPartTotal
            '    Dim dtRecptAccount As DataTable = Me.Session_Datatable_DtRecptAccount
            '    Dim dtPartRemainingBalance = AddParticipantAnnuitiesDetails(Me.Session_Datatable_DtPartAccount, Me.ViewState_Datatable_DtPartSelectedAccount)

            '    If (HelperFunctions.isEmpty(dtRecptAccount)) Then dtRecptAccount = CreateRecipientTable()
            '    If (HelperFunctions.isEmpty(dtPartAccount)) Then dtPartAccount = CreateParticipantTable()

            '    'If ValidateSplit(dtPartOriginal, dtPartAccount) = False Then
            '    '    MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", "Please check the Percentage to split ", MessageBoxButtons.OK, False)
            '    '    Exit Sub
            '    'End If

            '    selectedTotalAnnutiesAmount = CalculateSelectedAmount()



            '    Dim dtCurrentBeneficiary As DataTable = ReadCurrentData(dtRecptAccount, dtPartOriginalAnnuity, selectedTotalAnnutiesAmount, PercentageForSplit)

            '    Dim strMessage As String = ValidateSplitParameters(dtPartOriginalAnnuity, dtRecptAccount, dtCurrentBeneficiary)
            '    If (String.IsNullOrEmpty(strMessage) = False) Then
            '        MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", strMessage, MessageBoxButtons.Stop, False)
            '        Exit Sub
            '    End If

            '    Dim dtParticipantAnnuitiesRemainingBalances As DataTable = dtPartRemainingBalance.Copy
            '    Dim dtCopyRecptExistingSplit As DataTable = dtRecptAccount.Copy()
            '    DetermineValueSplit(dtPartOriginalAnnuity, dtCurrentBeneficiary, dtCopyRecptExistingSplit, dtParticipantAnnuitiesRemainingBalances)

            '    ProRateAdjustment(dtPartOriginalAnnuity, dtCopyRecptExistingSplit, dtParticipantAnnuitiesRemainingBalances, dtCurrentBeneficiary)


            '    If ValidateAdjustment(dtPartOriginalAnnuity, dtParticipantAnnuitiesRemainingBalances, dtCopyRecptExistingSplit, dtCurrentBeneficiary, DropdownlistBeneficiarySSNo.SelectedValue.Trim) = False Then
            '        LogTraceInformation(dtPartOriginalAnnuity, dtParticipantAnnuitiesRemainingBalances, dtCopyRecptExistingSplit, dtCurrentBeneficiary, DropdownlistBeneficiarySSNo.SelectedValue.Trim)
            '        MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_CHECK_SPLIT_PERCENTAGE_VALIDATION, MessageBoxButtons.Stop, False) ''Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
            '        Exit Sub
            '    End If


            '    dtRecptAccount = dtCurrentBeneficiary

            '    drRecps = dtRecptAccount.Select("RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")

            '    'its add records into dtRecptAccounttemp
            '    AddDataToRecptAccounttemp(drRecps, selectedTotalAnnutiesAmount, dtRecipientAnnuitiesBalances)
            '    'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            '    ''DataGridWorkSheet2.DataSource = dtBeneCurrentSplitTemp
            '    'DataGridWorkSheet2.DataBind()
            '    'DatagridParticipantsBalance.DataSource = dtCopyPartRemainingBalance
            '    'DatagridParticipantsBalance.DataBind()
            '    'Commented by ENd - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)


            '    Me.Session_Datatable_DtPartAccount = AddUpdateParticipantAnnuitiesDetails(Me.Session_Datatable_DtPartAccount, dtParticipantAnnuitiesRemainingBalances)

            '    'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            '    Me.ViewState_Datatable_DtPartCurrentAccount = dtParticipantAnnuitiesRemainingBalances
            '    'Below Logic to Update the Recipient Annuities
            '    If Not Me.Session_Datatable_DtRecptAccount Is Nothing Then
            '        If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtRecptAccount) Then
            '            Me.Session_Datatable_DtRecptAccount = AddRecipientAnnuitiesDetails(Me.Session_Datatable_DtRecptAccount, dtCurrentBeneficiary, Me.SplitPlanTypeOption)
            '        Else
            '            Me.Session_Datatable_DtRecptAccount = dtRecptAccount
            '        End If
            '    Else
            '        Me.Session_Datatable_DtRecptAccount = dtRecptAccount
            '    End If
            '    GetAnnuitiesBalancesDetails(Me.SplitPlanTypeOption)
            '    DisplaySplitProgressCaption(True)

            '    '  Me.Session_Datatable_DtRecptAccountTemp = AddUpdateParticipantAnnuitiesDetails(Me.Session_Datatable_DtRecptAccountTemp, dtRecipientAnnuitiesBalances)


            '    If Not Me.Session_Datatable_DtRecptAccountTemp Is Nothing Then
            '        If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtRecptAccountTemp) Then
            '            Me.Session_Datatable_DtRecptAccountTemp = AddUpdateParticipantAnnuitiesDetails(Me.Session_Datatable_DtRecptAccountTemp, dtRecipientAnnuitiesBalances)
            '        Else
            '            Me.Session_Datatable_DtRecptAccountTemp = dtRecipientAnnuitiesBalances
            '        End If
            '    Else
            '        Me.Session_Datatable_DtRecptAccountTemp = dtRecipientAnnuitiesBalances
            '    End If
            '    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            '    'If Me.Session_Datatable_DtRecptAccount Is Nothing Then
            '    '    'dtPartTotal = CType(Me.Session_datatable_dtPartTotal, DataTable)
            '    '    dtPartOriginal = Me.Session_Datatable_DtPartTotal

            '    '    dtRecptAccount = CreateRecipientTable()
            '    '    dtPartAccount = CreateParticipantTable()
            '    '    If ValidateSplit(dtPartOriginal, dtPartAccount) Then
            '    '        l_Double_SelectedAmount = CalculateSelectedAmount()
            '    '        'dtPartAccount = CType(Me.Session_datatable_dtPartAccount, DataTable)
            '    '        dtPartAccount = Me.Session_Datatable_DtPartAccount

            '    '        Dim dtCurrentBeneficiary As DataTable = ReadCurrentData(dtRecptAccount, dtPartOriginal)

            '    '        ProRateAdjustment1(dtPartOriginal, dtRecptAccount, dtPartAccount, dtCurrentBeneficiary)
            '    '        dtRecptAccount = dtCurrentBeneficiary
            '    '        SplitParticipantBalance(dtPartOriginal, dtRecptAccount, dtPartAccount, l_Double_SelectedAmount)
            '    '        drRecps = dtRecptAccount.Select("RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString() & "'")
            '    '        AddDataToRecptAccounttemp(drRecps, l_Double_SelectedAmount)
            '    '        DataGridWorkSheet2.DataSource = dtRecptAccounttemp
            '    '        DataGridWorkSheet2.DataBind()

            '    '        DatagridParticipantsBalance.DataSource = dtPartAccount
            '    '        DatagridParticipantsBalance.DataBind()
            '    '        RadioButtonListPlanType.Enabled = False
            '    '        Me.Session_Datatable_DtPartAccount = dtPartAccount
            '    '        Me.Session_Datatable_DtRecptAccount = dtRecptAccount
            '    '        Me.Session_Datatable_DtRecptAccountTemp = dtRecptAccounttemp


            '    '    End If
            '    'Else

            '    '    If ValidateSplit(dtPartOriginal, dtPartAccount) Then
            '    '        l_Double_SelectedAmount = CalculateSelectedAmount()
            '    '        SplitParticipantBalance(dtPartOriginal, dtRecptAccount, dtPartAccount, l_Double_SelectedAmount)
            '    '        drRecps = dtRecptAccount.Select("RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString() & "'")
            '    '        AddDataToRecptAccounttemp(drRecps, l_Double_SelectedAmount)
            '    '        DataGridWorkSheet2.DataSource = dtRecptAccounttemp
            '    '        DataGridWorkSheet2.DataBind()
            '    '        DatagridParticipantsBalance.DataSource = dtPartAccount
            '    '        DatagridParticipantsBalance.DataBind()
            '    '        RadioButtonListPlanType.Enabled = False
            '    '        Me.Session_Datatable_DtPartAccount = dtPartAccount
            '    '        Me.Session_Datatable_DtRecptAccount = dtRecptAccount
            '    '        Me.Session_Datatable_DtRecptAccountTemp = dtRecptAccounttemp

            '    '    End If
            '    'End If
            '    'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            '    'ButtonSplit.Enabled = False
            '    'ButtonAdjust.Enabled = True
            '    'ButtonReset.Enabled = True
            '    'ButtonShowBalance.Enabled = True
            '    'Added Kranthi 230908
            '    'RadioButtonListSplitAmtType.Enabled = False
            '    'TextBoxAmountWorkSheet.Enabled = False
            '    'TextBoxPercentageWorkSheet.Enabled = False
            '    'Me.QdroRetiredTabStrip.Items(3).Enabled = True
            '    'ButtonDocumentSave.Enabled = True
            '    'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            'Else
            '    If DataGridWorkSheet.Items.Count = 0 Then
            '        MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_NO_AMOUNT_ANNUITY, MessageBoxButtons.Stop, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
            '        Exit Sub
            '    End If
            '    If Not CheckValidate() Then
            '        MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_SELECT_ANNUITY_SPLIT, MessageBoxButtons.Stop, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
            '    Else
            '        MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_AMOUNT_PERCENTAGE_VALIDATION, MessageBoxButtons.Stop, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
            '    End If
            'End If
            'END: PPP | 01/20/2017 | YRS-AT-3299 | Old split code commented
        Catch ex As Exception
            HelperFunctions.LogException("ButtonSplit_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub

    Public Function ConvertDataTableToString(ByVal dr As DataRow) As String

        Dim stringBuilder As New StringBuilder()
        Try

            For index As Int32 = 0 To dr.Table.Columns.Count - 1
                stringBuilder.AppendFormat("{0}:{1} , ", dr.Table.Columns(index).ColumnName, dr(index))
            Next
        Catch
            Throw
        End Try
        Return stringBuilder.ToString()
    End Function
    Public Function ConvertDataTableToString(ByVal dt As DataTable) As String
        Dim stringBuilder As New StringBuilder()
        Try

            dt.Rows.Cast(Of DataRow)().ToList().ForEach(Function(dataRow)
                                                            dt.Columns.Cast(Of DataColumn)().ToList().ForEach(Function(column)
                                                                                                                  stringBuilder.AppendFormat("{0}:{1} , ", column.ColumnName, dataRow(column))

                                                                                                              End Function)
                                                            stringBuilder.Append("|")

                                                        End Function)

        Catch
            Throw
        End Try
        Return stringBuilder.ToString()
    End Function

    '***************************************************************************************************//
    'Event Name                :ButtonAdjust_Click                            Created on  : 14/04/08    //
    'Created By                :Dilip Patada                                  Modified On :             //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Event is used to Adjust the Annuity for the beneficiary            //
    '***************************************************************************************************//

    Private Sub ButtonAdjust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdjust.Click
        Try
            'dtRecptAccount = CType(Me.Session_datatable_dtRecptAccount, DataTable)
            'dtRecptAccounttemp = CType(Me.Session_datatable_dtRecptAccounttemp, DataTable)
            'dtPartAccount = CType(Me.Session_datatable_dtPartAccount, DataTable)
            'dtPartTotal = CType(Me.Session_datatable_dtPartTotal, DataTable)

            dtRecptAccount = Me.Session_Datatable_DtRecptAccount
            Dim dtRecptCurrenttemp As DataTable = Me.Session_Datatable_DtRecptAccountTemp
            dtPartAccount = Me.Session_Datatable_DtPartAccount
            dtPartOriginal = Me.Session_Datatable_DtPartTotal
            Dim dtRecptAccountvalidation As DataTable = GetRecipientAccountBySplitTypes(Me.Session_Datatable_DtRecptAccount, Me.SplitPlanTypeOption)
            If (dtRecptAccountvalidation.Rows.Count <= 1) Then
                'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_CONDITION_FOR_ADJUSTMENT_VALIDATION, MessageBoxButtons.Stop, False)
                ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_CONDITION_FOR_ADJUSTMENT_VALIDATION, "error")
                'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                Exit Sub
            End If

            'SP 2014.07.14 BT-2445\YRS 5.0-633 -Start
            Dim dtParticipantRemainingAccBalance As DataTable
            Dim dtBeneficiaryAdjustmentAnnuity As DataTable

            dtParticipantRemainingAccBalance = AddParticipantAnnuitiesDetails(Me.Session_Datatable_DtPartAccount, dtPartOriginal)
            ' dtBeneficiaryAdjustmentAnnuity = dtRecptAccount.Copy()

            'Populate the current values into current datatset(passed as input)
            dtBeneficiaryAdjustmentAnnuity = ReadAdjustmentDataFromGrid(dtRecptAccount, DataGridRecipientAnnuitiesBalance)

            ReverseCurrentBeneficiaryComponentintoRemainigParticipantBalance(dtRecptCurrenttemp, dtParticipantRemainingAccBalance, dtRecptAccount)

            'Prorate the all the components of current adjustment for beneficiary 
            ProRateAdjustment(dtPartOriginal, dtRecptAccount, dtParticipantRemainingAccBalance, dtBeneficiaryAdjustmentAnnuity)
            ' ProRateSplitAdjustMent(dtPartOriginal, dtRecptAccount, dtParticipantRemainingAccBalance, dtBeneficiaryAdjustmentAnnuity)

            'SP 2014.07.14 BT-2445\YRS 5.0-633 -End
            ' If ValidateAdjustMent(dtPartTotal, dtPartAccount, DataGridWorkSheet2, dtRecptAccounttemp) Then 'sP 2014.07.15 BT-2445\YRS 5.0-633 
            If ValidateAdjustment(dtPartOriginal, dtParticipantRemainingAccBalance, dtRecptAccount, dtBeneficiaryAdjustmentAnnuity, DropdownlistBeneficiarySSNo.SelectedValue.Trim) Then

                Dim dtBeneCurrentSplitTemp As DataTable
                dtBeneCurrentSplitTemp = CreateRecipientTable()

                Dim drRecps As DataRow() = dtBeneficiaryAdjustmentAnnuity.Select("splitType ='" & Me.SplitPlanTypeOption & "' AND RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")
                AddDataToRemainingBalance(drRecps, dtBeneCurrentSplitTemp)

                'START: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function
                'DataGridRecipientAnnuitiesBalance.DataSource = dtBeneCurrentSplitTemp
                'DataGridRecipientAnnuitiesBalance.DataBind()

                'DatagridParticipantsBalance.DataSource = dtParticipantRemainingAccBalance
                'DatagridParticipantsBalance.DataBind()
                'END: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function

                Me.Session_Datatable_DtPartAccount = AddUpdateParticipantAnnuitiesDetails(Me.Session_Datatable_DtPartAccount, dtParticipantRemainingAccBalance)

                Me.Session_Datatable_DtRecptAccount = UpdateRecipientAnnuitiesDetails(Me.Session_Datatable_DtRecptAccount, dtBeneficiaryAdjustmentAnnuity, Me.SplitPlanTypeOption)
                Me.Session_Datatable_DtRecptAccountTemp = dtBeneCurrentSplitTemp
                'START: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function
                'ButtonDocumentSave.Enabled = True 
                'DatagridParticipantsBalance.DataSource = Me.Session_Datatable_DtPartAccount
                'DatagridParticipantsBalance.DataBind()
                ShowHideControls()
                'END: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function
                'AdJustPartBalance(dtRecptAccounttemp, dtPartAccount)
                'DatagridParticipantsBalance.DataSource = dtPartAccount
                'DatagridParticipantsBalance.DataBind()
                'AdJustRecpBalance(dtRecptAccounttemp, dtRecptAccount)
                'DataGridWorkSheet2.DataSource = dtRecptAccounttemp
                'DataGridWorkSheet2.DataBind()
            Else
                'ButtonDocumentSave.Enabled = False 'PPP | 01/24/2017 | YRS-AT-3299 | Old button
                LogTraceInformation(dtPartOriginal, dtParticipantRemainingAccBalance, dtRecptAccount, dtBeneficiaryAdjustmentAnnuity, DropdownlistBeneficiarySSNo.SelectedValue.Trim)
                'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ANNUITY_ADJUSTMENT_VALIDATION, MessageBoxButtons.Stop, False) ''Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
                ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ANNUITY_ADJUSTMENT_VALIDATION, "error")
                'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                Exit Sub
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ButtonAdjust_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Control was part of internal find screen which is not in use
    ''***************************************************************************************************//
    ''Event Name                :ButtonClear_Click                             Created on  : 14/04/08    //
    ''Created By                :Dilip Patada                                  Modified On :             //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :                                                                        //
    ''Method Description        :This Event is used to clear the controls                                //
    ''***************************************************************************************************//
    'Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
    '    Try
    '        TextBoxSSNoList.Text = String.Empty
    '        TextBoxFundNoList.Text = String.Empty
    '        TextBoxLastNameList.Text = String.Empty
    '        TextBoxFirstNameList.Text = String.Empty
    '        TextBoxCityList.Text = String.Empty
    '        TextBoxStateList.Text = String.Empty
    '    Catch ex As Exception
    '        HelperFunctions.LogException("ButtonClear_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
    '        'Dim l_String_Exception_Message As String
    '        'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '        Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
    '        'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
    '    End Try
    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Control was part of internal find screen which is not in use

    '***************************************************************************************************//
    'Event Name                :TextBoxSpouseSSNo_TextChanged                 Created on  : 14/04/08    //
    'Created By                :Dilip Patada                                  Modified On :             //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Event is used to get the details of the beneficiery for the SSno   //
    '***************************************************************************************************//

    Private Sub TextBoxSSNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxSSNo.TextChanged
        Try
            HiddenFieldDirty.Value = "true" 'PPP | 01/23/2017 | YRS-AT-3299 | Setting hidden field value
            LoanBeneficiaryPersonalDetails()
        Catch ex As Exception
            HelperFunctions.LogException("TextBoxSSNo_TextChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub

    'START: MMR | 2017.01.09 | YRS-AT-3299 | Commneted existing code as not required
    ''***************************************************************************************************//
    ''Event Name                :ButtonFindList_Click                          Created on  : 14/04/08    //
    ''Created By                :Dilip Patada                                  Modified On :             //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :                                                                        //
    ''Method Description        :This Event is used to get the the list of the participant               //
    ''***************************************************************************************************//
    'Private Sub ButtonFindList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFindList.Click
    '    Try

    '        Me.String_FundEventID = Nothing

    '        Me.String_Part_SSN = Nothing
    '        TextBoxSSNoList.Text = TextBoxSSNoList.Text.Replace("-", "")
    '        Headercontrol.PageTitle = String.Empty


    '        If (TextBoxFirstNameList.Text = String.Empty And TextBoxLastNameList.Text = String.Empty And TextBoxFundNoList.Text = String.Empty And TextBoxSSNoList.Text = String.Empty And TextBoxCityList.Text = String.Empty And TextBoxStateList.Text = String.Empty) Then
    '            MessageBox.Show(PlaceHolderACHDebitImportProcess, "QRDO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTICIPANT_SEARCH_CRITERIA, MessageBoxButtons.OK) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
    '        Else

    '            ResetAnnuityTab()
    '            LoadQDROList()

    '            'added kranthi 101008
    '            Me.Session_Datatable_DtBenifAccount = Nothing
    '            dtBenifAccount = Nothing
    '            DatagridBenificiaryList.DataSource = Nothing
    '            DatagridBenificiaryList.DataBind()
    '            Me.string_Benif_PersonID = Nothing
    '            ResetSession()
    '            'added kranthi 101008
    '            Me.Session_String_ISAddUpdate = Nothing
    '            'added kranthi 101008
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES).Enabled = False
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = False
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = False
    '            If btnAddBeneficiaryToList.Text = "Update To List" Then
    '                btnAddBeneficiaryToList.Text = "Add To List"
    '            End If
    '            ButtonEditBeneficiary.Enabled = False
    '            ButtonCancelBeneficiary.Enabled = False
    '            'added kranthi 101008
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("ButtonFindList_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    'ENDF: MMR | 2017.01.09 | YRS-AT-3299 | Commneted existing code as not required

    'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    ''***************************************************************************************************//
    ''Event Name                :DataGridRetireeList_SelectedIndexChanged             Created on  : 14/04/08    //
    ''Created By                :Dilip Patada                                  Modified On :             //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :                                                                        //
    ''Method Description        :This Event is used to get the the list of the Beneficiery for tthe      //
    ''                           Selected participant                                                    //
    ''***************************************************************************************************//
    'Private Sub DataGridRetireeList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRetireeList.SelectedIndexChanged
    '    Dim l_dataset_QDROList As New DataSet
    '    Dim datagridbenifrow As Integer
    '    Dim strWSMessage As String

    '    Try
    '        'l_dataset_QDROList = CType(Me.Session_dataset_QDRORetiree, DataSet)
    '        l_dataset_QDROList = Me.Session_Dataset_QDRORetiree


    '        Dim l_SSNo As String
    '        Dim dr As DataRow()
    '        l_SSNo = Me.DataGridRetireeList.SelectedItem.Cells(1).Text.Trim
    '        dr = l_dataset_QDROList.Tables(0).Select("[SSNo]= '" & l_SSNo & "'")

    '        '----Shashi Shekhar:2010-02-17: Code to handle Archived Participants from list--------------
    '        'Shashi Shekhar:2010-03-17:Handling usability issue of Data Archive
    '        ' If Me.DataGridRetireeList.SelectedItem.Cells(5).Text.ToUpper.Trim() = "TRUE" Then
    '        If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("FundStatus").trim <> "Retired")) Then
    '            MessageBox.Show(Me.PlaceHolderACHDebitImportProcess, " YMCA - YRS", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTICIPANT_ARCHIVE_UNARCHIVE, MessageBoxButtons.Stop, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES).Enabled = False
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = False
    '            Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = False
    '            Headercontrol.PageTitle = String.Empty

    '            Exit Sub
    '        End If
    '        '---------------------------------------------------------------------------------------
    '        If (dr(0).Item(4).ToString.Trim <> "" And dr(0).Item(4).ToString.Trim <> "System.DBNull") Then
    '            'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
    '            'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
    '            strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(dr(0).Item(4).ToString.Trim)
    '            If strWSMessage <> "NoPending" Then
    '                Page.ClientScript.RegisterStartupScript(Me.GetType(), "YRS", "openDialog('" + strWSMessage + "','Pers');", True)
    '                Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES).Enabled = False
    '                Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = False
    '                Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = False
    '                Headercontrol.PageTitle = String.Empty
    '                Exit Sub
    '            End If
    '            'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
    '        End If

    '        'Shashi Shekhar:10 Feb 2011:  For YRS 5.0-1236 : Need ability to freeze/lock account
    '        Me.IsAccountLock = Nothing
    '        Me.IsAccountLock = Me.DataGridRetireeList.SelectedItem.Cells(7).Text.ToUpper.Trim()

    '        '  RadioButtonListPlanType.Items(2).Selected = True

    '        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES).Enabled = True
    '        'If Not Me.string_PersSSID Is Nothing And Me.string_PersSSID <> DataGridRetireeList.SelectedItem.Cells(1).Text.Trim Then

    '        Me.Session_Datatable_DtBenifAccount = Nothing
    '        dtBenifAccount = Nothing
    '        DatagridBenificiaryList.DataSource = Nothing
    '        DatagridBenificiaryList.DataBind()
    '        Me.string_Benif_PersonID = Nothing
    '        ResetAnnuityTab()
    '        ResetSession()
    '        'added kranthi 101008
    '        Me.Session_String_ISAddUpdate = Nothing
    '        'added kranthi 101008
    '        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = False
    '        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = False
    '        ButtonCancelBeneficiary.Enabled = False
    '        'If ButtonDocumentBeneficiery.Text = "Update To List" Then
    '        btnAddBeneficiaryToList.Text = "Add To List"
    '        ButtonCancelBeneficiary.Enabled = False
    '        ButtonEditBeneficiary.Enabled = False
    '        'End If
    '        'End If

    '        'added kranthi 220908
    '        'changing the selected record normal font to bold.
    '        For datagridbenifrow = 0 To DataGridRetireeList.Items.Count - 1
    '            DataGridRetireeList.Items(datagridbenifrow).Font.Bold = False
    '        Next
    '        If DataGridRetireeList.Items.Count > 0 Then
    '            DataGridRetireeList.Items(DataGridRetireeList.SelectedIndex).Font.Bold = True
    '        End If

    '        If DatagridBenificiaryList.Items.Count = 0 Then
    '            ClearControls(True)
    '            'If ButtonDocumentBeneficiery.Text = "Update To List" Then
    '            '    ButtonDocumentBeneficiery.Text = "Add To List"
    '            'End If
    '            'If ButtonEditBeneficiery.Enabled = True Then
    '            '    ButtonEditBeneficiery.Enabled = False
    '            'End If
    '            ButtonAddNewBeneficiary.Enabled = False
    '        End If

    '        'added kranthi 220908

    '        LoadPersonalTab()

    '        Me.String_PersSSID = DataGridRetireeList.SelectedItem.Cells(1).Text.Trim
    '        Me.String_Part_SSN = DataGridRetireeList.SelectedItem.Cells(1).Text.Trim + "-" + DataGridRetireeList.SelectedItem.Cells(2).Text.Trim + " " + DataGridRetireeList.SelectedItem.Cells(3).Text.Trim

    '        Me.String_PersId = CType(l_dataset_QDROList.Tables(0).Rows(DataGridRetireeList.SelectedIndex)(4), Guid).ToString()
    '        Me.String_FundEventID = CType(l_dataset_QDROList.Tables(0).Rows(DataGridRetireeList.SelectedIndex)(5), Guid).ToString()
    '        Me.String_QDRORequestID = CType(l_dataset_QDROList.Tables(0).Rows(DataGridRetireeList.SelectedIndex)(6), Guid).ToString()

    '        'Shashi : 03 Mar. 2011:     Replacing Header formating with user control (YRS 5.0-450 )
    '        Headercontrol.PageTitle = "QDRO Retirees Information"
    '        Headercontrol.FundNo = DataGridRetireeList.SelectedItem.Cells(6).Text.Trim()

    '        Call SetControlFocus(Me.TextBoxSSNo)

    '        'ButtonDocumentBeneficiery.Enabled = True                       'commented by kranthi 140808.
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("DataGridRetireeList_SelectedIndexChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required

    '***************************************************************************************************//
    'Event Name                :DataListParticipant_ItemDataBound             Created on  : 14/06/08    //
    'Created By                :Dilip Patada                                  Modified On :             //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Event is used to display the participant information in the summary//
    '                           Tab                                                                     //
    '***************************************************************************************************//

    Private Sub DataListParticipant_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataListParticipant.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim dtTemp As New DataTable
                dgParticipant = DirectCast(e.Item.FindControl("DatagridSummaryBalList"), DataGrid)
                dtTemp = Me.Session_Datatable_DtPartAccount
                dgParticipant.DataSource = dtTemp
                dgParticipant.DataBind()
            End If
        Catch ex As Exception
            HelperFunctions.LogException("DataListParticipant_ItemDataBound", ex) 'PPP | 01/24/2017 | YRS-AT-3299 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub

    '***************************************************************************************************//
    'Event Name                :DatalistBeneficiary_ItemDataBound             Created on  : 14/06/08    //
    'Created By                :Dilip Patada                                  Modified On :             //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Event is used to display the Beneficiary information in the summary//
    '                           Tab                                                                     //
    '***************************************************************************************************//
    Private Sub DatalistBeneficiary_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DatalistBeneficiary.ItemDataBound
        Try

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim dtTemp As New DataTable
                dtTemp = dtFindBeneficiary.Clone()
                Dim lblRecipientAnnuitiesDetails As Label
                Dim dtTempRow As DataRow()
                Dim dtRecipientAnnuitiesBalance As DataTable
                dtTempRow = dtFindBeneficiary.Select("RecipientPersonID='" & DatalistBeneficiary.DataKeys(e.Item.ItemIndex).ToString().Trim() & "'")
                If dtTempRow.Length > 0 Then
                    For Each dtRow1 As DataRow In dtTempRow
                        dtTemp.Rows.Add(New Object() {dtRow1(0), dtRow1(1), dtRow1(2), dtRow1(3), dtRow1(4), dtRow1(5), _
                                            dtRow1(6), dtRow1(7), dtRow1(8), dtRow1(9), dtRow1(10), dtRow1(11), dtRow1(12), dtRow1(13), dtRow1(14)})
                    Next
                    ' dgBeneficiary = DirectCast(e.Item.FindControl("DatagridBeneficiarySummaryBalList"), DataGrid)
                    'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                    lblRecipientAnnuitiesDetails = DirectCast(e.Item.FindControl("lblRecipientAnnutiesDetails"), Label)
                    dtRecipientAnnuitiesBalance = GetRecipientAnnutiesDetailsForHtmlTable(Me.Session_Datatable_DtRecptAccount, DatalistBeneficiary.DataKeys(e.Item.ItemIndex))
                    lblRecipientAnnuitiesDetails.Text = HelperFunctions.GenerateHTMLTableForRecipientAnnuities(dtRecipientAnnuitiesBalance)
                    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                    dgBeneficiary.DataSource = dtTemp
                    dgBeneficiary.DataBind()
                End If

            End If
        Catch ex As Exception
            HelperFunctions.LogException("DatalistBeneficiary_ItemDataBound", ex) 'PPP | 01/24/2017 | YRS-AT-3299 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub

    '***************************************************************************************************//
    'Event Name                :DataGridWorkSheet2_ItemDataBound              Created on  : 14/06/08    //
    'Created By                :Dilip Patada                                  Modified On :             //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Event is used to Add the attribute to the editable datagrid textbox//
    '                           for decimal validation in annuity tab of recipent grid                  //
    '***************************************************************************************************//
    Private Sub DataGridRecipientAnnuitiesBalance_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridRecipientAnnuitiesBalance.ItemDataBound

        Try

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim dgItems As DataGridItem
                dgItems = e.Item
                Dim TextSSLevelingAmt As TextBox = CType(dgItems.Cells(5).FindControl("TextBoxSSLevlingAmount"), TextBox)
                Dim TextSSReductionAmt As TextBox = CType(dgItems.Cells(6).FindControl("TextboxSSReductionAmount"), TextBox)
                Dim TextCurrentPayment As TextBox = CType(dgItems.Cells(8).FindControl("TextboxCurrentPayment"), TextBox)
                Dim TextEmpPreTaxCurrentPayment As TextBox = CType(dgItems.Cells(9).FindControl("TextboxEmpPreTaxCurrentPayment"), TextBox)
                Dim TextEmpPostTaxCurrentPayment As TextBox = CType(dgItems.Cells(10).FindControl("TextboxEmpPostTaxCurrentPayment"), TextBox)
                Dim TextYmcaPreTaxCurrentPayment As TextBox = CType(dgItems.Cells(11).FindControl("TextboxYmcaPreTaxCurrentPayment"), TextBox)
                Dim TextEmpPreTaxRemainingReserves As TextBox = CType(dgItems.Cells(12).FindControl("TextboxEmpPreTaxRemainingReserves"), TextBox)
                Dim TextEmpPostTaxRemainingReserves As TextBox = CType(dgItems.Cells(13).FindControl("TextboxEmpPostTaxRemainingReserves"), TextBox)
                Dim TextYmcapreTaxRemainingReserves As TextBox = CType(dgItems.Cells(14).FindControl("TextboxYmcaPreTaxRemainingReserves"), TextBox)

                TextSSLevelingAmt.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextSSReductionAmt.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextCurrentPayment.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextEmpPreTaxCurrentPayment.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextEmpPostTaxCurrentPayment.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextYmcaPreTaxCurrentPayment.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextEmpPreTaxRemainingReserves.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextEmpPostTaxRemainingReserves.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextYmcapreTaxRemainingReserves.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

                TextSSLevelingAmt.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextSSReductionAmt.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextCurrentPayment.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextEmpPreTaxCurrentPayment.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextEmpPostTaxCurrentPayment.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextYmcaPreTaxCurrentPayment.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextEmpPreTaxRemainingReserves.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextEmpPostTaxRemainingReserves.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextYmcapreTaxRemainingReserves.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            End If

        Catch ex As Exception
            HelperFunctions.LogException("DataGridRecipientAnnuitiesBalance_ItemDataBound", ex) 'PPP | 01/24/2017 | YRS-AT-3299 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Not using DocumentSave() 
    ''***************************************************************************************************//
    ''Event Name                :ButtonDocumentSave_Click                      Created on  : 24/07/08    //
    ''Created By                :Dilip Patada                                  Modified On :             //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Param Description         :                                                                        //
    ''Method Description        :This Event is used to save the split information into the database      //
    ''***************************************************************************************************//
    ''BS:2012.05.18:YRS 5.0-1470: Create Method
    'Private Sub DocumentSave()
    '    'dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
    '    'dtRecptAccount = CType(Me.Session_datatable_dtRecptAccount, DataTable)
    '    Dim l_DoubleTotalSplitPercentage As Double
    '    l_DoubleTotalSplitPercentage = 0.0
    '    'For Each dtBenifAccountRow As DataRow In dtBenifAccount.Rows

    '    '    For Each dtRecptAccountRow As DataRow In dtRecptAccount.Rows

    '    '    Next
    '    'Next

    '    Try


    '        'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
    '        Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

    '        If Not checkSecurity.Equals("True") Then
    '            MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
    '            Exit Sub
    '        End If
    '        'End : YRS 5.0-940

    '        If Not Me.Session_Datatable_DtBenifAccount Is Nothing And Not Me.Session_Datatable_DtRecptAccount Is Nothing Then
    '            'dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
    '            'dtRecptAccount = CType(Me.Session_datatable_dtRecptAccount, DataTable)
    '            'dtPartAccount = CType(Me.Session_datatable_dtPartAccount, DataTable)

    '            dtBenifAccount = Me.Session_Datatable_DtBenifAccount
    '            dtRecptAccount = Me.Session_Datatable_DtRecptAccount
    '            dtPartAccount = Me.Session_Datatable_DtPartAccount

    '            Dim dtBenifAccountTempTable As New DataTable
    '            dtBenifAccountTempTable = dtBenifAccount.Clone

    '            For Each dtBenifAccountRow As DataRow In dtBenifAccount.Rows
    '                For Each dtRecptAccountRow As DataRow In dtRecptAccount.Rows
    '                    If dtRecptAccountRow.Item(0) = dtBenifAccountRow.Item(0) Then
    '                        l_DoubleTotalSplitPercentage = l_DoubleTotalSplitPercentage + Convert.ToDouble(dtRecptAccountRow("RecipientSplitPercent"))
    '                        dtBenifAccountTempTable.Rows.Add(New Object() {dtBenifAccountRow(0), dtBenifAccountRow(1), dtBenifAccountRow(2), dtBenifAccountRow(3), dtBenifAccountRow(4), dtBenifAccountRow(5), dtBenifAccountRow(6), dtBenifAccountRow(7), dtBenifAccountRow(8), dtBenifAccountRow(9), dtBenifAccountRow(10), dtBenifAccountRow(11), dtBenifAccountRow(12), dtBenifAccountRow(13), dtBenifAccountRow(14), dtBenifAccountRow(15), dtBenifAccountRow(16), dtBenifAccountRow(17), dtBenifAccountRow(18), dtBenifAccountRow(19), dtBenifAccountRow(20), dtBenifAccountRow(21), dtBenifAccountRow(22), dtBenifAccountRow(23), dtBenifAccountRow(24), dtBenifAccountRow(25)})
    '                        Exit For
    '                    End If
    '                Next
    '            Next
    '            'SP 2014.06.30 BT-2445\YRS 5.0-633 -Start
    '            'If dtBenifAccount.Rows.Count <> dtBenifAccountTempTable.Rows.Count Then
    '            '	Me.Session_String_ISComplited = True
    '            '	MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", "Do you want to Save the Record as there is another Recipient Pending for Split?", MessageBoxButtons.YesNo, False)
    '            'Else
    '            'SP 2014.06.30 BT-2445\YRS 5.0-633 -End

    '            YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.SaveRetiredSplit(dtBenifAccountTempTable, dtRecptAccount, Me.String_RecptFundEventID, Me.String_FundEventID, Me.String_QDRORequestID, dtPartAccount, l_DoubleTotalSplitPercentage, dtBenifAccount)

    '            blnSave = True

    '            ' Me.Session_String_ISComplited = True 'SP 2014.06.30 BT-2445\YRS 5.0-633


    '            'Added by dilip 22-12-2008 bugid 661
    '            DropdownlistBeneficiarySSNo.Enabled = False
    '            'Added by dilip 22-12-2008 bugid 661
    '            MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_SAVED_SUCCESSFULLY, MessageBoxButtons.OK, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
    '            ResetAnnuityTab()

    '            '  End If 'SP 2014.06.30 BT-2445\YRS 5.0-633 
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogMessage("QDRO Settlement" + ex.Message)
    '        Dim l_String_Exception_Message As String
    '        If ex.Message = "MESSAGE_RETIREDQDRO_ChkAnnuityCurrentVal. Operation failed." Then
    '            l_String_Exception_Message = Server.UrlEncode(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ChkAnnuityCurrentVal)
    '        Else
    '            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        End If
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

    '    End Try
    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Not using DocumentSave() 

    'START: PPP | 01/23/2017 | YRS-AT-3299 | Removed button ButtonDocumentSave and moved its functionality to btnSave
    'Private Sub ButtonDocumentSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDocumentSave.Click
    '    'Start-SR:2014.06.11- BT 2566: Commented below line of code to remove invalid address validation during save process
    '    'BS:2012.05.17:YRS 5.0-1470 :-validate verify address
    '    'Dim i_str_Message1 As String = String.Empty
    '    '      i_str_Message1 = AddressWebUserControl1.ValidateAddress
    '    'If i_str_Message1 <> "" Then
    '    '	Session("VerifiedAddress") = "VerifiedAddress"
    '    '	MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA", i_str_Message1, MessageBoxButtons.YesNo)
    '    '	Exit Sub
    '    'End If
    '    'End-SR:2014.06.11- BT 2566:Commented above line of code to remove invalid address validation during save process

    '    'SP 2014.06.30 BT-2445\YRS 5.0-633 -Start
    '    Try
    '        Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
    '        If Not checkSecurity.Equals("True") Then
    '            MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
    '            Exit Sub
    '        End If
    '        Dim l_DoubleTotalSplitPercentage As Double
    '        l_DoubleTotalSplitPercentage = 0.0
    '        Dim dtBenifAccountTempTable As New DataTable
    '        If Not Me.Session_Datatable_DtBenifAccount Is Nothing And Not Me.Session_Datatable_DtRecptAccount Is Nothing Then

    '            dtBenifAccount = Me.Session_Datatable_DtBenifAccount.Copy
    '            dtRecptAccount = Me.Session_Datatable_DtRecptAccount.Copy
    '            dtPartAccount = Me.Session_Datatable_DtPartAccount.Copy


    '            dtBenifAccountTempTable = dtBenifAccount.Clone

    '            For Each dtBenifAccountRow As DataRow In dtBenifAccount.Rows
    '                For Each dtRecptAccountRow As DataRow In dtRecptAccount.Rows
    '                    If dtRecptAccountRow.Item(0) = dtBenifAccountRow.Item(0) Then
    '                        l_DoubleTotalSplitPercentage = l_DoubleTotalSplitPercentage + Convert.ToDouble(dtRecptAccountRow("RecipientSplitPercent"))
    '                        dtBenifAccountTempTable.Rows.Add(New Object() {dtBenifAccountRow(0), dtBenifAccountRow(1), dtBenifAccountRow(2), dtBenifAccountRow(3), dtBenifAccountRow(4), dtBenifAccountRow(5), dtBenifAccountRow(6), dtBenifAccountRow(7), dtBenifAccountRow(8), dtBenifAccountRow(9), dtBenifAccountRow(10), dtBenifAccountRow(11), dtBenifAccountRow(12), dtBenifAccountRow(13), dtBenifAccountRow(14), dtBenifAccountRow(15), dtBenifAccountRow(16), dtBenifAccountRow(17), dtBenifAccountRow(18), dtBenifAccountRow(19), dtBenifAccountRow(20), dtBenifAccountRow(21), dtBenifAccountRow(22), dtBenifAccountRow(23), dtBenifAccountRow(24), dtBenifAccountRow(25)})
    '                        Exit For
    '                    End If
    '                Next
    '            Next

    '            If dtBenifAccount.Rows.Count <> dtBenifAccountTempTable.Rows.Count Then
    '                PartialBeneficiarySettleted = "yes"
    '                ShowConfirmationMessage(dtPartAccount, dtRecptAccount, dtBenifAccount, Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTIAL_BENEFICIARY_SETTLEMENT)

    '            Else
    '                ShowConfirmationMessage(dtPartAccount, dtRecptAccount, dtBenifAccount, Nothing)
    '                PartialBeneficiarySettleted = "no"
    '            End If


    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogMessage("QDRO Settlement")
    '        Dim l_String_Exception_Message As String
    '        If ex.Message = "MESSAGE_RETIREDQDRO_ChkAnnuityCurrentVal. Operation failed." Then
    '            l_String_Exception_Message = Server.UrlEncode(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ChkAnnuityCurrentVal)
    '        Else
    '            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        End If
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    '    'SP 2014.06.30 BT-2445\YRS 5.0-633 -End

    '    'DocumentSave()   'SP 2014.06.30 BT-2445\YRS 5.0-633 -Commented
    'End Sub
    'END: PPP | 01/23/2017 | YRS-AT-3299 | Removed button ButtonDocumentSave and moved its functionality to btnSave

    '***************************************************************************************************//
    'Event Name                :DatagridBenificiaryList_SelectedIndexChanged  Created on  : 24/07/08    //
    'Created By                :Dilip Patada                                  Modified On :             //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Event will load the annuity details of the participent in annuity  //
    '                           tab and select the beneficiary for split if split forcast information is//
    '                           is already there then it will display split detail.                     //
    '***************************************************************************************************//

    Private Sub DatagridBenificiaryList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DatagridBenificiaryList.SelectedIndexChanged
        Dim drBenef() As DataRow
        Dim datagridbenifrow As Integer
        Dim selectedrow As Integer
        'added kranthi 171008
        Dim l_button_Select As ImageButton

        Try
            Me.String_Benif_SSno = Me.DatagridBenificiaryList.SelectedItem.Cells(recipientIndexSSN).Text 'PPP | 01/24/2017 | YRS-AT-3299 | Cells are accessed by defind index property. OLD: [Me.DatagridBenificiaryList.SelectedItem.Cells(2).Text]
            Me.string_Benif_PersonID = Me.DatagridBenificiaryList.SelectedItem.Cells(recipientIndexPersID).Text 'PPP | 01/24/2017 | YRS-AT-3299 | Cells are accessed by defind index property. OLD: [Me.DatagridBenificiaryList.SelectedItem.Cells(1).Text]
            If Not Me.Session_Datatable_DtBenifAccount Is Nothing Then

                dtBenifAccount = Me.Session_Datatable_DtBenifAccount
                drBenef = dtBenifAccount.Select(String.Format("ID='{0}'", DatagridBenificiaryList.SelectedItem.Cells(recipientIndexPersID).Text)) 'PPP | 01/24/2017 | YRS-AT-3299 | Cells are accessed by defind index property. OLD: [Me.DatagridBenificiaryList.SelectedItem.Cells(1).Text]
                If (drBenef.Length <> 0) Then
                    Dim drDataRow As DataRow
                    drDataRow = drBenef.GetValue(0)
                    'Start - commented existing code and enabling edit beneficiary button
                    'If (drDataRow("FlagNewBenf") = False) Then
                    '    'ButtonEditBeneficiery.Enabled = False
                    'Else
                    '    'ButtonEditBeneficiery.Enabled = True
                    'End If
                    ButtonEditBeneficiary.Enabled = True
                    'End - commented existing code and enabling edit beneficiary button
                    'If ButtonEditBeneficiery.Enabled = True Then
                    '    ButtonEditBeneficiery.Enabled = False
                    'End If
                    If btnAddBeneficiaryToList.Text = "Update Recipient" Then 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Update To List" to "Update Recipient"
                        btnAddBeneficiaryToList.Text = "Save Recipient" 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Add To List" to "Save Recipient"
                    End If
                End If

                'dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
                dtBenifAccount = Me.Session_Datatable_DtBenifAccount

                DropdownlistBeneficiarySSNo.DataSource = dtBenifAccount
                DropdownlistBeneficiarySSNo.DataBind()
                DropdownlistBeneficiarySSNo.SelectedValue = Me.string_Benif_PersonID
                Call LoadDataSelectedBeneficiary()
                'LoadAnnuityTab()
                'Me.LIstMultiPage.SelectedIndex = 2
                'Me.QdroRetiredTabStrip.SelectedIndex = 2
                'LoadBeneficiarys(Me.string_Benif_PersonID)                  'add kranthi 090908.
                'Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = True 'PPP | 01/24/2017 | YRS-AT-3299 | Tabs are controled by Next Previous button
                'Call SetControlFocusDropDown(Me.DropdownlistBeneficiarySSNo)
                'ButtonEditBeneficiery.Enabled = True
                'lockRecipientControls(False)

                For datagridbenifrow = 0 To DatagridBenificiaryList.Items.Count - 1
                    DatagridBenificiaryList.Items(datagridbenifrow).Font.Bold = False
                    'added kranthi 171008
                    l_button_Select = DatagridBenificiaryList.Items(datagridbenifrow).FindControl("Imagebutton1")
                    If Not l_button_Select Is Nothing Then
                        If datagridbenifrow = DatagridBenificiaryList.SelectedIndex Then
                            l_button_Select.ImageUrl = "images\selected.gif"
                        Else
                            l_button_Select.ImageUrl = "images\select.gif"
                        End If
                    End If
                Next

                DatagridBenificiaryList.Items(DatagridBenificiaryList.SelectedIndex).Font.Bold = True
                If ButtonAddNewBeneficiary.Enabled = False Then
                    ButtonAddNewBeneficiary.Enabled = True
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("DatagridBenificiaryList_SelectedIndexChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub
    '***************************************************************************************************//
    'Event Name                :RadioButtonListPlanType_SelectedIndexChanged  Created on  : 24/07/08    //
    'Created By                :Dilip Patada                                  Modified On :             //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Event is used to load the annuity details for selected account Type//
    '***************************************************************************************************//

    'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    'Private Sub RadioButtonListPlanType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonListPlanType.SelectedIndexChanged
    '    Try
    '        LoadAnnuityTab()
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try

    'End Sub
    'Commented by END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '*******************************************************************************************************//
    'Event Name                :DropdownlistBeneficiarySSNo_SelectedIndexChanged  Created on  : 24/07/08    //
    'Created By                :Dilip Patada                                      Modified On :             //
    'Modified By               :                                                                            //
    'Modify Reason             :                                                                            //
    'Param Description         :                                                                            //
    'Method Description        :This Event will load the annuity details of the Recipent in annuity         //
    '                           tab for selected recipent.                                                  //
    '*******************************************************************************************************//
    Private Sub DropdownlistBeneficiarySSNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistBeneficiarySSNo.SelectedIndexChanged
        Try
            'LoadBeneficiaries(DropdownlistBeneficiarySSNo.SelectedItem.Value)            'add kranthi 090908. 'PPP | 01/24/2017 | YRS-AT-3299 | Not using LoadBeneficiaries 
            hdnSelectedPlanType.Value = String.Empty ' This will load default page for selected beneficiary
            SetRecipientSessionProperties()
            LoadSplitAnnuitiesTab()
        Catch ex As Exception
            HelperFunctions.LogException("DropdownlistBeneficiarySSNo_SelectedIndexChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub
    '*******************************************************************************************************//
    'Event Name                :RadioButtonListSplitAmtType_SelectedIndexChanged  Created on  : 24/07/08    //
    'Created By                :Dilip Patada                                      Modified On :             //
    'Modified By               :                                                                            //
    'Modify Reason             :                                                                            //
    'Param Description         :                                                                            //
    'Method Description        :This Event will reset the textbox for the Split Amount typein annuity tab   //
    '*******************************************************************************************************//
    'Commented by END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    'Below logic Coding is moved into Javascript in the RetrieeQDRO.aspx
    'Private Sub RadioButtonListSplitAmtType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonListSplitAmtType.SelectedIndexChanged
    '    Try
    '        If RadioButtonListSplitAmtType.SelectedValue = "AmountToSplit" Then
    '            TextBoxPercentageWorkSheet.Enabled = False
    '            TextBoxAmountWorkSheet.Enabled = True
    '            TextBoxAmountWorkSheet.Text = "0.00"
    '            TextBoxPercentageWorkSheet.Text = "0.00"
    '            If ButtonSplit.Enabled = True Then
    '                ButtonSplit.Enabled = False
    '            End If
    '        ElseIf RadioButtonListSplitAmtType.SelectedValue = "Percentage" Then
    '            TextBoxPercentageWorkSheet.Enabled = True
    '            TextBoxPercentageWorkSheet.Text = "0.00"
    '            TextBoxAmountWorkSheet.Text = "0.00"                            'changed null value to 0.00 by Kranthi 060808
    '            TextBoxAmountWorkSheet.Enabled = False
    '            If ButtonSplit.Enabled = True Then
    '                ButtonSplit.Enabled = False
    '            End If
    '        End If

    '    Catch ex As Exception
    '        HelperFunctions.LogException("RadioButtonListSplitAmtType_SelectedIndexChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub
    'Commented by END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    'START: PPP | 01/17/2017 | YRS-AT-3299 | Amount and Percentage code behind event handling is not required, it has been handled by client side scripting
    ''*****************************************************************************************************//
    ''Event Name                :TextBoxPercentageWorkSheet_TextChanged      Used In     : YMCAUI          //
    ''Created By                :Kranthi                                     Modified On : 04/09/08        //
    ''Modified By               :                                                                          //
    ''Modify Reason             :                                                                          //
    ''Constructor Description   :                                                                          //
    ''Event Description         :This event will excecute when the user changes the percentage value in    //
    ''                           accounts tab                                                              //
    ''*****************************************************************************************************//
    'Private Sub TextBoxPercentageWorkSheet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxPercentageWorkSheet.TextChanged
    '    Try
    '        If (TextBoxPercentageWorkSheet.Text.ToString <> "" And TextBoxPercentageWorkSheet.Text.ToString <> ".") Then

    '            'If CType(TextBoxPercentageWorkSheet.Text, Double) >= 1 And ButtonReset.Enabled = False Then // Commented by Chandra sekar | 2016.08.22 | YRS-AT-3081
    '            If CType(TextBoxPercentageWorkSheet.Text, Double) >= 1 Then
    '                ButtonSplit.Enabled = True
    '            Else
    '                If CType(TextBoxPercentageWorkSheet.Text, Double) < 1 Then
    '                    TextBoxPercentageWorkSheet.Text = "0.00"
    '                End If
    '                If ButtonSplit.Enabled = True Then
    '                    ButtonSplit.Enabled = False
    '                End If
    '            End If
    '            TextBoxPercentageWorkSheet.Enabled = True
    '            TextBoxAmountWorkSheet.Text = "0.00"
    '            TextBoxAmountWorkSheet.Enabled = False
    '        Else
    '            TextBoxPercentageWorkSheet.Text = "0.00"
    '            Exit Sub
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogException("TextBoxPercentageWorkSheet_TextChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
    '        'Dim l_String_Exception_Message As String
    '        'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '        Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
    '        'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable        
    '    End Try
    'End Sub

    ''*****************************************************************************************************//
    ''Event Name                :TextBoxAmountWorkSheet_TextChanged                 Used In : YMCAUI       //
    ''Created By                :Kranthi                                            Modified On : 04/09/08 //
    ''Modified By               :                                                                          //
    ''Modify Reason             :                                                                          //
    ''Constructor Description   :                                                                          //
    ''Event Description         :This event will excecute when the user changes the amount value in        //
    ''                           accounts tab                                                              //
    ''*****************************************************************************************************//
    'Private Sub TextBoxAmountWorkSheet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxAmountWorkSheet.TextChanged
    '    Try
    '        If (TextBoxAmountWorkSheet.Text.ToString <> "" And TextBoxAmountWorkSheet.Text.ToString <> ".") Then

    '            'If CType(TextBoxAmountWorkSheet.Text, Double) >= 0.01 And ButtonReset.Enabled = False Then  // Commented by Chandra sekar | 2016.08.22 | YRS-AT-3081
    '            If CType(TextBoxAmountWorkSheet.Text, Double) >= 0.01 Then
    '                ButtonSplit.Enabled = True
    '            Else
    '                If CType(TextBoxAmountWorkSheet.Text, Double) < 1 Then
    '                    TextBoxAmountWorkSheet.Text = "0.00"
    '                End If

    '                If ButtonSplit.Enabled = True Then
    '                    ButtonSplit.Enabled = False
    '                End If
    '            End If
    '            TextBoxAmountWorkSheet.Enabled = True
    '            TextBoxPercentageWorkSheet.Enabled = False
    '            TextBoxPercentageWorkSheet.Text = "0.00"
    '        Else
    '            TextBoxAmountWorkSheet.Text = "0.00"
    '            Exit Sub
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogException("TextBoxAmountWorkSheet_TextChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
    '        'Dim l_String_Exception_Message As String
    '        'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '        Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
    '        'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable        
    '    End Try
    'End Sub
    'END: PPP | 01/17/2017 | YRS-AT-3299 | Amount and Percentage code behind event handling is not required, it has been handled by client side scripting

    'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    ''*****************************************************************************************************//
    ''Event Name                :DataGridRetireeList_ItemCommand                    Used In : YMCAUI       //
    ''Created By                :Kranthi                                            Modified On : 04/09/08 //
    ''Modified By               :                                                                          //
    ''Modify Reason             :                                                                          //
    ''Constructor Description   :                                                                          //
    ''Event Description         :This event will excecute when the user click on  Retiree list to make the //
    ''                           selected row Bold                                                         //
    ''*****************************************************************************************************//
    'Private Sub DataGridRetireeList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridRetireeList.ItemCommand
    '    Dim cnt As Integer
    '    Dim l_button_Select As ImageButton
    '    Try

    '        'Added kranthi 071008

    '        For cnt = 0 To DataGridRetireeList.Items.Count - 1
    '            l_button_Select = DataGridRetireeList.Items(cnt).FindControl("ImageButtonSelect")
    '            l_button_Select.ImageUrl = "images\select.gif"
    '        Next

    '        l_button_Select = e.Item.FindControl("ImageButtonSelect")
    '        If (e.Item.ItemIndex = Me.DataGridRetireeList.SelectedIndex And Me.DataGridRetireeList.SelectedIndex >= 0) Then 'Commented by kranthi 071008 (uncommented by dilip 18112008)
    '            l_button_Select.ImageUrl = "images\selected.gif"
    '        End If

    '    Catch
    '        Throw
    '        'Dim l_String_Exception_Message As String
    '        'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required

    'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    ''*****************************************************************************************************//
    ''Event Name                :DataGridRetireeList_SortCommand                    Used In : YMCAUI       //
    ''Created By                :Dilip                                              Modified On : 04/09/08 //
    ''Modified By               :                                                                          //
    ''Modify Reason             :                                                                          //
    ''Constructor Description   :                                                                          //
    ''Event Description         :This event will excecute when the user click on  Retiree list grid Heading//
    ''                           to sort on the select item                                                //
    ''*****************************************************************************************************//
    'Private Sub DataGridRetireeList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridRetireeList.SortCommand

    '    Try
    '        Dim l_button_Select As ImageButton
    '        Dim dv As New DataView
    '        Dim SortExpression As String
    '        SortExpression = e.SortExpression

    '        dv = Me.Session_Dataset_QDRORetiree.Tables(0).DefaultView
    '        dv.Sort = SortExpression
    '        If Not Session("FindInfo_Sort") Is Nothing Then
    '            If Session("FindInfo_Sort").ToString.Trim.EndsWith("ASC") Then
    '                dv.Sort = SortExpression + " DESC"
    '            Else
    '                dv.Sort = SortExpression + " ASC"
    '            End If
    '        Else
    '            dv.Sort = SortExpression + " ASC"
    '        End If
    '        Me.DataGridRetireeList.DataSource = Nothing
    '        Me.DataGridRetireeList.DataSource = dv
    '        Me.DataGridRetireeList.DataBind()
    '        Session("FindInfo_Sort") = dv.Sort
    '        'Added by dilip on 18112008
    '        Dim datagridbenifrow As Integer
    '        For datagridbenifrow = 0 To DataGridRetireeList.Items.Count - 1
    '            DataGridRetireeList.Items(datagridbenifrow).Font.Bold = False
    '        Next
    '        For datagridbenifrow = 0 To DataGridRetireeList.Items.Count - 1
    '            If DataGridRetireeList.Items(datagridbenifrow).Cells(1).Text.ToString = Me.String_PersSSID Then
    '                DataGridRetireeList.Items(datagridbenifrow).Font.Bold = True
    '                l_button_Select = DataGridRetireeList.Items(datagridbenifrow).FindControl("ImageButtonSelect")
    '                l_button_Select.ImageUrl = "images\selected.gif"
    '                Exit For
    '            End If
    '        Next
    '        'Added by dilip 18112008
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = ex.Message.Trim.ToString()
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required

    'START: PPP | 01/24/2017 | YRS-AT-3299 | dgPager as part of internal find screen which is removed now
    ''*****************************************************************************************************//
    ''Event Name                :dgPager_PageChanged                                Used In : YMCAUI       //
    ''Created By                :Dilip                                              Modified On : 04/09/08 //
    ''Modified By               :                                                                          //
    ''Modify Reason             :                                                                          //
    ''Constructor Description   :                                                                          //
    ''Event Description         :This event will excecute when the user click on  Retiree list grid pages  //
    ''                           for Paging                                                                //
    ''*****************************************************************************************************//
    'Private Sub dgPager_PageChanged(ByVal PgNumber As Integer) Handles dgPager.PageChanged
    '    Try
    '        If Me.IsPostBack Then
    '            'LoadQDROList()
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("QdroRetiredTabStrip_SelectedIndexChange", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = ex.Message.Trim.ToString()
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | dgPager as part of internal find screen which is removed now

    '*****************************************************************************************************//
    'Event Name                :ButtonAddNewBeneficiery_Click                      Used In : YMCAUI       //
    'Created By                :Dilip                                              Modified On : 04/09/08 //
    'Modified By               :                                                                          //
    'Modify Reason             :                                                                          //
    'Constructor Description   :                                                                          //
    'Event Description         :This event occurs when user click on Add New Beneficiary button for adding//
    '                           New Benificiary to the Benificiary list                                   //
    '*****************************************************************************************************//
    Private Sub ButtonAddNewBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddNewBeneficiary.Click
        Dim stTelephoneError As String 'PPP | 2015.10.13 | YRS-AT-2588
        Try

            'AddressWebUserControl1.SetValidationsForPrimary()
            'Anudeep:2013.04.13-BT-1555:YRS 5.0-1769:Length of phone numbers
            If AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA" Then
                'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                'If TextboxSpouseTel.Text.Length <> 10 And TextboxSpouseTel.Text.Length > 0 Then
                '    MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", "Telephone number must be 10 digits.", MessageBoxButtons.Stop)
                '    Exit Sub
                'End If
                If TextBoxTel.Text.Trim.Length > 0 Then
                    stTelephoneError = Validation.Telephone(TextBoxTel.Text.Trim, YMCAObjects.TelephoneType.Telephone)
                    If (Not String.IsNullOrEmpty(stTelephoneError)) Then
                        'START: PPP | 01/24/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        'MessageBox.Show(PlaceHolderACHDebitImportProcess, "YMCA-YRS", stTelephoneError, MessageBoxButtons.Stop)
                        ShowModalPopupWithCustomMessage("QDRO", stTelephoneError, "error") 'MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "YMCA-YRS" to "QDRO"
                        'END: PPP | 01/24/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                        Exit Sub
                    End If
                End If
                'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
            End If
            LockAndUnLockRecipientControls(False)
            TextBoxSSNo.Enabled = True
            ClearControls(True)
            If btnAddBeneficiaryToList.Text = "Update Recipient" Then
                btnAddBeneficiaryToList.Text = "Save Recipient"
            End If
            ButtonEditBeneficiary.Enabled = False
            ButtonCancelBeneficiary.Enabled = True
            Call SetControlFocus(Me.TextBoxSSNo)
        Catch ex As Exception
            HelperFunctions.LogException("ButtonAddNewBeneficiary_Click", ex) 'PPP | 01/24/2017 | YRS-AT-3299 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub

    '*****************************************************************************************************//
    'Event Name                :ButtonCancelBeneficiery_Click                      Used In : YMCAUI       //
    'Created By                :Dilip                                              Modified On : 04/09/08 //
    'Modified By               :                                                                          //
    'Modify Reason             :                                                                          //
    'Constructor Description   :                                                                          //
    'Event Description         :This event occurs when user click on cancel button for canceling the      //
    '                           the current operation                                                     //
    '*****************************************************************************************************//
    Private Sub ButtonCancelBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancelBeneficiary.Click
        Try
            'If DropdownlistSpouseSal.Enabled = False Then
            '    DropdownlistSpouseSal.Enabled = True
            'End If
            ClearControls(True)
            LoadDataBeneficiaryCancel()
            LockAndUnLockRecipientControls(False)
            ManageEditableControls(False) 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling gender and marital control
            If btnAddBeneficiaryToList.Text = "Update Recipient" Then 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Update To List" to "Update Recipient"
                btnAddBeneficiaryToList.Text = "Save Recipient" 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Add To List" to "Save Recipient"
            End If
            ButtonAddNewBeneficiary.Enabled = True
            ButtonCancelBeneficiary.Enabled = False
            btnAddBeneficiaryToList.Enabled = False
        Catch ex As Exception
            HelperFunctions.LogException("ButtonCancelBeneficiary_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
        End Try
    End Sub
    '*****************************************************************************************************//
    'Event Name                :ButtonEditBeneficiery_Click                        Used In : YMCAUI       //
    'Created By                :Dilip                                              Modified On : 04/09/08 //
    'Modified By               :                                                                          //
    'Modify Reason             :                                                                          //
    'Constructor Description   :                                                                          //
    'Event Description         :This event occurs when user click on Edit Beneficiary button for editing  //
    '                           New Benificiary to the Benificiary list                                   //
    '*****************************************************************************************************//
    Private Sub ButtonEditBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditBeneficiary.Click
        Dim drBenef() As DataRow
        Try
            'dtBenifAccount = Me.Session_datatable_dtBenifAccount
            'drBenef = dtBenifAccount.Select("ID='" & DatagridBenificiaryList.SelectedItem.Cells(1).Text & "'")
            'If (drBenef.Length <> 0) Then
            '    Dim drDataRow As DataRow
            '    drDataRow = drBenef.GetValue(0)
            '    If (drDataRow("FlagNewBenf") = False) Then
            '        'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", "Recipent already exist you can not Edit", MessageBoxButtons.OK, False)
            '        Exit Sub
            '    End If
            'End If
            'Start - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Added code to enable disable controls based on existing or new beneficiary
            dtBenifAccount = Me.Session_Datatable_DtBenifAccount
            drBenef = dtBenifAccount.Select(String.Format("ID='{0}'", DatagridBenificiaryList.SelectedItem.Cells(recipientIndexPersID).Text)) 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced BENEFICIARY_ID with recipientIndexPersID. OLD: [DatagridBenificiaryList.SelectedItem.Cells(BENEFICIARY_ID).Text]
            If (drBenef.Length <> 0) Then
                Dim drDataRow As DataRow
                drDataRow = drBenef.GetValue(0)
                If (drDataRow("FlagNewBenf") = False) Then
                    LockAndUnLockRecipientControls(False)
                Else
                    LockAndUnLockRecipientControls(True)
                End If
            End If
            'End - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Added code to enable disable controls based on existing or new beneficiary
            btnAddBeneficiaryToList.Text = "Update Recipient" 'PPP | 01/24/2017 | YRS-AT-3299 | Replaced "Update To List" to "Update Recipient"
            btnAddBeneficiaryToList.Enabled = True
            ButtonCancelBeneficiary.Enabled = True
            ButtonAddNewBeneficiary.Enabled = False
            ButtonEditBeneficiary.Enabled = False
            'LockAndUnLockRecipientControls(True) 'Commented existing code as not required
            'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Enabling Gender and marital dropdown control
            ManageEditableControls(True)
            'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Enabling Gender and marital dropdown control
            TextBoxSSNo.Enabled = False
            HiddenFieldDirty.Value = "true" 'PPP | 01/23/2017 | YRS-AT-3299 | Setting hidden field value
        Catch ex As Exception
            'Start - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Commneted existing code and added to catch exception error and Logging it.
            'Throw ex
            HelperFunctions.LogException("ButtonCancelBeneficiary_Click", ex)
            'START: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/24/2017 | YRS-AT-3299 | encoding of error is done at one place without using variable
            'End - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Commneted existing code and added to catch exception error and Logging it.
        End Try
    End Sub
#End Region

#Region "Private Functions"
    '***************************************************************************************************//
    'Method Name               :FormatCurrency            Created on  :                                 //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This method is used to format the currency                              //
    '                                                                                                   //
    '***************************************************************************************************//

    Protected Function FormatCurrency(ByVal price As Double) As String
        Return price.ToString("N", currencyType)
    End Function


    '***************************************************************************************************//
    'Function Name             :ClearControls             Created on  : 14/04/08                        //
    'Created By                :Dilip Patada              Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :parameterFlag                                                           //
    'Method Description        :This Function is used to Clear the controls in the beneficieary tab     //
    '***************************************************************************************************//
    Private Function ClearControls(ByVal parameterFlag As Boolean)
        Try
            If parameterFlag = True Then
                TextBoxSSNo.Text = String.Empty
                DropdownlistSal.SelectedIndex = -1
                TextBoxFirstName.Text = String.Empty
                TextBoxMiddleName.Text = String.Empty
                TextBoxLastName.Text = String.Empty
                TextBoxSuffix.Text = String.Empty
                TextBoxBirthDate.Text = String.Empty
                'TextboxFirstName.Text = String.Empty

                'Added kranthi 091908
                AddressWebUserControl1.ClearControls = ""
                'Added kranthi 091908

                TextBoxEmail.Text = String.Empty
                TextBoxTel.Text = String.Empty

                'added kranthi 141008
                'CheckboxIsSpouse.Checked = False
                'Start - Manthan Rajguru |2016.08.16 | YRS-AT-2482 | Initializing the default value.
                'Start - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Checking if value exists in dropdown before assigning default selected value at the time of reseting controls
                If Not Me.DropDownListMaritalStatus.Items.FindByValue("D") Is Nothing Then
                    DropDownListMaritalStatus.SelectedValue = "D"
                Else
                    DropDownListMaritalStatus.SelectedValue = "SEL"
                End If
                'End - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Checking if value exists in dropdown before assigning default selected value at the time of reseting controls
                DropDownListGender.SelectedValue = "SEL"
                'End - Manthan Rajguru |2016.08.16 | YRS-AT-2482 | Initializing the default value.
            End If
        Catch
            Throw
        End Try
    End Function
#Region "DataTables"

    '***************************************************************************************************//
    'Function Name             :CreateDataTableBenfAccount  Created on  : 14/04/08                      //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function is used to create the datatable structure for beneficieary//
    '***************************************************************************************************//
    Private Function CreateDataTableBenfAccount() As DataTable
        Dim dtBenifAccount As DataTable = New DataTable
        Dim myDataColumn As DataColumn

        Try

            myDataColumn = New DataColumn("id") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing upper case name of column "ID" to lower case "id", to support common recipient stagging db functionality
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSNo")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("LastName")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("FirstName")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("MiddleName")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("FundStatus")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("FlagNewBenf")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("FundEventID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SalutationCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SuffixTitle")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("BirthDate")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("MaritalCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Adding gender code column.
            myDataColumn = New DataColumn("GenderCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Adding gender code column.
            myDataColumn = New DataColumn("EmailAddress") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "EMail" to "EmailAddress", to support common recipient stagging db functionality
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("PhoneNumber") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "PhoneNo" to "PhoneNumber", to support common recipient stagging db functionality
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("Address1") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add1" to "Address1", to support common recipient stagging db functionality
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("Address2") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add2" to "Address2", to support common recipient stagging db functionality
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("Address3") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add3" to "Address3", to support common recipient stagging db functionality
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("City")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("State")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("Zip") 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "zip" to "Zip", to support common recipient stagging db functionality
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("Country")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)
            'Added 10-09-2008 for RecptRetireeID
            myDataColumn = New DataColumn("RecptRetireeID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtBenifAccount.Columns.Add(myDataColumn)

            'Anudeep
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "Address_effectiveDate"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.Boolean")
            myDataColumn.ColumnName = "BadAddress"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "AddressNote"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "AddressNote_bitImportant"
            dtBenifAccount.Columns.Add(myDataColumn)

            'added kranthi 141008
            'myDataColumn = New DataColumn("IsSpouse")
            'myDataColumn.DataType = Type.GetType("System.Char")
            'dtBenifAccount.Columns.Add(myDataColumn)


            Return dtBenifAccount

        Catch
            Throw
        End Try

    End Function

    '***************************************************************************************************//
    'Function Name             :CreateParticipantTable      Created on  : 14/04/08                      //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function is used to create the datatable structure for Participant //
    '***************************************************************************************************//
    Private Function CreateParticipantTable() As DataTable
        'Defining the structure for  dtPartAccount
        Dim dtPartAccount As New DataTable
        Dim myDataColumn As DataColumn

        Try

            myDataColumn = New DataColumn("selected")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuitySourceCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("PlanType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("PurchaseDate")
            myDataColumn.DataType = Type.GetType("System.DateTime")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuityType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSLevelingAmt")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSReductionAmt")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSReductionEftDate")
            myDataColumn.DataType = Type.GetType("System.DateTime")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("CurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPreTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPostTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("YmcaPreTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPreTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPostTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("YmcapreTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("guiAnnuityID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartAccount.Columns.Add(myDataColumn)
            'Added for AdjustmentBasisCode & guiAnnuityJointSurvivorsID 10-09-2008
            myDataColumn = New DataColumn("AdjustmentBasisCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("guiAnnuityJointSurvivorsID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartAccount.Columns.Add(myDataColumn)
            'Start -SR:2014.06.26 BT-2445\YRS 5.0-633
            myDataColumn = New DataColumn("ShareBenefit")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtPartAccount.Columns.Add(myDataColumn)
            'End -SR:2014.06.26 BT-2445\YRS 5.0-633 
            'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            myDataColumn = New DataColumn("splitType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("RecipientPersonID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartAccount.Columns.Add(myDataColumn)
            'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            Return dtPartAccount

        Catch
            Throw
        End Try

    End Function
    '***************************************************************************************************//
    'Function Name             :CreateRecipientTable      Created on  : 14/04/08                        //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function is used to create the datatable structure for Recipient   //
    '***************************************************************************************************//
    Private Function CreateRecipientTable() As DataTable
        'Defining the structure for  RecptAccount
        Dim dtRecptAccount As New DataTable
        Dim myDataColumn As DataColumn

        Try

            myDataColumn = New DataColumn("RecipientPersonID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuitySourceCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("PlanType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("PurchaseDate")
            myDataColumn.DataType = Type.GetType("System.DateTime")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuityType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSLevelingAmt")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSReductionAmt")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSReductionEftDate")
            myDataColumn.DataType = Type.GetType("System.DateTime")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("CurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPreTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPostTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("YmcaPreTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPreTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPostTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("YmcapreTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            'Added for keeping Recipient split %
            myDataColumn = New DataColumn("RecipientSplitPercent")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            'Added for keeping Recipient split %
            myDataColumn = New DataColumn("guiAnnuityID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("IncludeSpecialdev")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuityTotal")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            'Added for keeping Recipent fundeventid 
            myDataColumn = New DataColumn("RecipientFundEventID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("RecptRetireeID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("splitType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            'Added for AdjustmentBasisCode & guiAnnuityJointSurvivorsID 10-09-2008
            myDataColumn = New DataColumn("AdjustmentBasisCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("guiAnnuityJointSurvivorsID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)

            'Start -SP:2014.07.01  BT-2445\YRS 5.0-633
            myDataColumn = New DataColumn("ShareBenefit")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtRecptAccount.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn("IsSplitPercentage")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtRecptAccount.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn("SplitAmount")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn("OriginalAnnuityType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)

            'End -SP:2014.07.01  BT-2445\YRS 5.0-633

            Return dtRecptAccount

        Catch
            Throw
        End Try

    End Function
    '*****************************************************************************************************//
    'Function Name             :CreateRecipientTableTemp    Created on  : 14/04/08                        //
    'Created By                :Dilip Patada                Modified On :                                 //
    'Modified By               :                                                                          //
    'Modify Reason             :                                                                          //
    'Param Description         :                                                                          //
    'Method Description        :This Function is used to create the datatable structure for RecipientTemp //
    '*****************************************************************************************************//
    Private Function CreateRecipientTableTemp() As DataTable
        'Defining the structure for  RecptAccount
        Dim dtRecptAccount As New DataTable
        Dim myDataColumn As DataColumn

        Try

            myDataColumn = New DataColumn("RecipientPersonID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuitySourceCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("PlanType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("PurchaseDate")
            myDataColumn.DataType = Type.GetType("System.DateTime")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuityType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSLevelingAmt")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSReductionAmt")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSReductionEftDate")
            myDataColumn.DataType = Type.GetType("System.DateTime")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("CurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPreTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPostTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("YmcaPreTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPreTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPostTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("YmcapreTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            'Added for keeping Recipient split %
            myDataColumn = New DataColumn("RecipientSplitPercent")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            'Added for keeping Recipient split %
            myDataColumn = New DataColumn("guiAnnuityID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("IncludeSpecialdev")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuityTotal")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtRecptAccount.Columns.Add(myDataColumn)
            'Added for keeping Recipent fundeventid
            myDataColumn = New DataColumn("RecipientFundEventID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("RecptRetireeID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("splitType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            'Added for AdjustmentBasisCode & guiAnnuityJointSurvivorsID 10-09-2008
            myDataColumn = New DataColumn("AdjustmentBasisCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("guiAnnuityJointSurvivorsID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtRecptAccount.Columns.Add(myDataColumn)
            Return dtRecptAccount

        Catch
            Throw
        End Try

    End Function
    '***************************************************************************************************//
    'Function Name             :CreatePartTotalTable      Created on  : 14/04/08                        //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function is used to create the datatable structure for Participiant//
    '***************************************************************************************************//
    Private Function CreatePartTotalTable() As DataTable
        'Defining the structure for  PartTotal
        Dim dtPartTotal As New DataTable
        Dim myDataColumn As DataColumn

        Try

            myDataColumn = New DataColumn("selected")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuitySourceCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("PlanType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("PurchaseDate")
            myDataColumn.DataType = Type.GetType("System.DateTime")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("AnnuityType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSLevelingAmt")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSReductionAmt")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("SSReductionEftDate")
            myDataColumn.DataType = Type.GetType("System.DateTime")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("CurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPreTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPostTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("YmcaPreTaxCurrentPayment")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPreTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("EmpPostTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("YmcapreTaxRemainingReserves")
            myDataColumn.DataType = Type.GetType("System.Decimal")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("guiAnnuityID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartTotal.Columns.Add(myDataColumn)
            'Added for AdjustmentBasisCode & guiAnnuityJointSurvivorsID
            myDataColumn = New DataColumn("AdjustmentBasisCode")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("guiAnnuityJointSurvivorsID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartTotal.Columns.Add(myDataColumn)
            'Start -SR:2014.06.26  BT-2445\YRS 5.0-633
            myDataColumn = New DataColumn("ShareBenefit")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtPartTotal.Columns.Add(myDataColumn)
            'End -SR:2014.06.26  BT-2445\YRS 5.0-633
            'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            myDataColumn = New DataColumn("RecipientPersonID")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartTotal.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("splitType")
            myDataColumn.DataType = Type.GetType("System.String")
            dtPartTotal.Columns.Add(myDataColumn)
            'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            Return dtPartTotal

        Catch
            Throw
        End Try

    End Function

#End Region

#Region "Add Data to Tables"
    '***************************************************************************************************//
    'Function Name             :AddDataToTable              Created on  : 14/04/08                      //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function is used to add the data to the datatable of beneficieary  //
    '***************************************************************************************************//
    'Private Function AddDataToTable(ByVal RecptRetireeID As String, ByVal RecptPersId As String, ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal FundStatus As String, ByVal FlagNewBenf As Boolean, ByVal RecptFundEventID As String, ByVal SalutationCode As String, ByVal SuffixTitle As String, ByVal BirthDate As String, ByVal MaritalCode As String, ByVal EMail As String, ByVal PhoneNo As String, ByVal Add1 As String, ByVal Add2 As String, ByVal Add3 As String, ByVal City As String, ByVal State As String, ByVal zip As String, ByVal Country As String, ByVal dtBenfTable As DataTable) 'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Commented Existing code
    'START: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not accepting it as an parameter, hardcoded it
    'Private Function AddDataToTable(ByVal RecptRetireeID As String, ByVal RecptPersId As String, ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal FundStatus As String, ByVal FlagNewBenf As Boolean, ByVal RecptFundEventID As String, ByVal SalutationCode As String, ByVal SuffixTitle As String, ByVal BirthDate As String, ByVal MaritalCode As String, ByVal GenderCode As String, ByVal EMail As String, ByVal PhoneNo As String, ByVal Add1 As String, ByVal Add2 As String, ByVal Add3 As String, ByVal City As String, ByVal State As String, ByVal zip As String, ByVal Country As String, ByVal dtBenfTable As DataTable) 'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Added gender code parameter.
    Private Function AddDataToTable(ByVal RecptRetireeID As String, ByVal RecptPersId As String, ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal FlagNewBenf As Boolean, ByVal RecptFundEventID As String, ByVal SalutationCode As String, ByVal SuffixTitle As String, ByVal BirthDate As String, ByVal MaritalCode As String, ByVal GenderCode As String, ByVal EMail As String, ByVal PhoneNo As String, ByVal Add1 As String, ByVal Add2 As String, ByVal Add3 As String, ByVal City As String, ByVal State As String, ByVal zip As String, ByVal Country As String, ByVal dtBenfTable As DataTable)
        'END: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not accepting it as an parameter, hardcoded it
        Dim dtRow As DataRow

        Try

            dtRow = dtBenfTable.NewRow()
            dtRow("id") = RecptPersId
            dtRow("SSNo") = SSNo
            dtRow("LastName") = LastName
            dtRow("FirstName") = FirstName
            dtRow("MiddleName") = MiddleName
            dtRow("FundStatus") = "RQ" 'PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not accepting it as an parameter, hardcoded it
            dtRow("FlagNewBenf") = FlagNewBenf
            dtRow("FundEventID") = RecptFundEventID
            dtRow("SalutationCode") = SalutationCode
            dtRow("SuffixTitle") = SuffixTitle
            dtRow("BirthDate") = BirthDate
            dtRow("MaritalCode") = MaritalCode
            dtRow("GenderCode") = GenderCode ''Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Adding gender code
            dtRow("EmailAddress") = EMail 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "EMail" to "EmailAddress", to support common recipient stagging db functionality
            dtRow("PhoneNumber") = PhoneNo 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "PhoneNo" to "PhoneNumber", to support common recipient stagging db functionality
            dtRow("Address1") = Add1 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add1" to "Address1", to support common recipient stagging db functionality
            dtRow("Address2") = Add2 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add2" to "Address2", to support common recipient stagging db functionality
            dtRow("Address3") = Add3 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add3" to "Address3", to support common recipient stagging db functionality
            dtRow("City") = City
            dtRow("State") = State
            dtRow("Zip") = zip 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "zip" to "Zip", to support common recipient stagging db functionality
            dtRow("Country") = Country
            dtRow("RecptRetireeID") = RecptRetireeID
            dtRow("Address_effectiveDate") = AddressWebUserControl1.EffectiveDate
            dtRow("BadAddress") = AddressWebUserControl1.IsBadAddress
            dtRow("AddressNote") = "Retired Qdro split:" + AddressWebUserControl1.Notes
            dtRow("AddressNote_bitImportant") = AddressWebUserControl1.BitImp
            dtBenfTable.Rows.Add(dtRow)
            MaintainRecipientDetails(dtRow, Me.String_QDRORequestID, YMCAObjects.DBActionType.CREATE) 'MMR | 01/10/2017 | YRS-AT-3299 | Recipient data will be added to staging table
            Return True

        Catch
            Throw
        End Try

    End Function

    '***************************************************************************************************//
    'Function Name             :AddDataToPartTotal              Created on  : 14/04/08                  //
    'Created By                :Dilip Patada                    Modified On :                           //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :dtTablePartTotal                                                                        //
    'Method Description        :This Function is used to add the data to the datatable of beneficieary  //
    '***************************************************************************************************//
    Private Function AddDataToPartTotal(ByVal dtTablePartTotal As DataTable)
        Dim dsRowcount As Integer
        Dim dsRow As DataRow
        Dim dtRow As DataRow
        Try
            If HelperFunctions.isNonEmpty(Me.Session_Datatable_DsPartBal) Then 'PPP | 01/20/2017 | YRS-AT-3299 | Replaced Session("dsPartBal") with Me.Session_Datatable_DsPartBal property and condition as well [OLD:If Not Session("dsPartBal") Is Nothing Then]
                dsPartTotal = Me.Session_Datatable_DsPartBal 'PPP | 01/20/2017 | YRS-AT-3299 | Replaced Session("dsPartBal") with Me.Session_Datatable_DsPartBal and casting was not required [OLD: DirectCast(Session("dsPartBal"), DataSet)]
            End If
            'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START
            Dim drParBal As DataRow()
            If Not String.IsNullOrEmpty(Me.SplitPlanTypeOption) Then
                If Me.SplitPlanTypeOption.Trim.ToLower() = EnumPlanTypes.BOTH.ToString().ToLower Then
                    drParBal = dsPartTotal.Tables(0).Select()
                Else
                    drParBal = dsPartTotal.Tables(0).Select(String.Format("PlanType='{0}'", Me.SplitPlanTypeOption)) 'PPP | 01/20/2017 | YRS-AT-3299 | Removed + operation on string [OLD:dsPartTotal.Tables(0).Select("PlanType='" + Me.SplitPlanTypeOption + "'")]
                End If

                'For dsRowcount = 0 To dsPartTotal.Tables(0).Rows.Count - 1
                For dsRowcount = 0 To drParBal.Length - 1
                    'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END
                    dtRow = dtTablePartTotal.NewRow()
                    'dsRow = dsPartTotal.Tables(0).Rows(dsRowcount)
                    dsRow = drParBal(dsRowcount)

                    dtRow("selected") = "True"
                    dtRow("AnnuitySourceCode") = dsRow("AnnuitySourceCode")
                    dtRow("PlanType") = dsRow("PlanType")
                    dtRow("PurchaseDate") = dsRow("PurchaseDate")
                    dtRow("AnnuityType") = dsRow("AnnuityType")
                    dtRow("SSLevelingAmt") = dsRow("SSLevelingAmt")
                    dtRow("SSReductionAmt") = dsRow("SSReductionAmt")
                    dtRow("SSReductionEftDate") = dsRow("SSReductionEftDate")
                    dtRow("CurrentPayment") = dsRow("CurrentPayment")
                    dtRow("EmpPreTaxCurrentPayment") = dsRow("EmpPreTaxCurrentPayment")
                    dtRow("EmpPostTaxCurrentPayment") = dsRow("EmpPostTaxCurrentPayment")
                    dtRow("YmcaPreTaxCurrentPayment") = dsRow("YmcaPreTaxCurrentPayment")
                    dtRow("EmpPreTaxRemainingReserves") = dsRow("EmpPreTaxRemainingReserves")
                    dtRow("EmpPostTaxRemainingReserves") = dsRow("EmpPostTaxRemainingReserves")
                    dtRow("YmcapreTaxRemainingReserves") = dsRow("YmcapreTaxRemainingReserves")
                    dtRow("guiAnnuityID") = dsRow("guiAnnuityID")
                    'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                    dtRow("RecipientPersonID") = DropdownlistBeneficiarySSNo.SelectedValue.ToString().Trim()
                    dtRow("AdjustmentBasisCode") = dsRow("chvAdjustmentBasisCode")
                    dtRow("guiAnnuityJointSurvivorsID") = dsRow("guiAnnuityJointSurvivorsID")
                    dtRow("splitType") = Me.SplitPlanTypeOption.Trim
                    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                    dtTablePartTotal.Rows.Add(dtRow)
                Next
            End If
            Return True

        Catch
            Throw
        End Try

    End Function
    '***************************************************************************************************//
    'Function Name             :UnselectAnnuities                    Created on  : 11/11/08             //
    'Created By                :Dilip Patada                         Modified On :                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :l_bool_Flag                                                             //
    'Method Description        :This Function is used select & Unselect Annuities                       //
    '***************************************************************************************************//

    Private Function UnselectAnnuities(ByVal l_bool_Flag As Boolean)
        Dim dgItem As DataGridItem
        For Each dgItem In DataGridWorkSheet.Items
            Dim SelectCheckbox As CheckBox = CType(dgItem.Cells(1).Controls(1), CheckBox)
            SelectCheckbox.Checked = l_bool_Flag
        Next
    End Function

    '***************************************************************************************************//
    'Function Name             :SelectAnnuities                      Created on  : 11/11/08             //
    'Created By                :Dilip Patada                         Modified On :                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function is used select Annuities for which the split is done for  //
    '                           for the selected beneficiary                                                                        //
    '***************************************************************************************************//

    Private Function SelectAnnuities(ByVal drRecps() As DataRow)
        Dim drRecp As DataRow
        Dim dgItem As DataGridItem
        Try


            For Each drRecp In drRecps
                For Each dgItem In DataGridWorkSheet.Items
                    Dim SelectCheckbox As CheckBox = CType(dgItem.Cells(1).Controls(1), CheckBox)
                    Dim ShareBenefitCheckbox As CheckBox = CType(dgItem.Cells(2).Controls(1), CheckBox)
                    If dgItem.Cells(0).Text = drRecp("guiAnnuityID") Then
                        SelectCheckbox.Checked = True
                        ShareBenefitCheckbox.Checked = drRecp("ShareBenefit")
                        Exit For
                    End If
                Next
            Next
        Catch
            Throw
        End Try
    End Function

    '***************************************************************************************************//
    'Function Name             :AddDataToRecptAccounttemp            Created on  : 14/04/08             //
    'Created By                :Dilip Patada                         Modified On :                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :drRecps()                                                               //
    'Method Description        :This Function is used to add the data to the datatable of RecptAccounttemp //
    '***************************************************************************************************//
    Private Function AddDataToRecptAccounttemp(ByVal drRecps() As DataRow, ByVal SelectedAnnuityAmount As Double, ByRef dtBeneCurrentSplitTemp As DataTable)
        Dim drRecp As DataRow
        Dim dtRow As DataRow

        Try

            For Each drRecp In drRecps
                dtRow = dtBeneCurrentSplitTemp.NewRow
                dtRow("RecipientPersonID") = drRecp("RecipientPersonID")
                dtRow("AnnuitySourceCode") = drRecp("AnnuitySourceCode")
                dtRow("PlanType") = drRecp("PlanType")
                dtRow("PurchaseDate") = drRecp("PurchaseDate")
                dtRow("AnnuityType") = drRecp("AnnuityType")
                dtRow("SSLevelingAmt") = drRecp("SSLevelingAmt")
                dtRow("SSReductionAmt") = drRecp("SSReductionAmt")
                dtRow("SSReductionEftDate") = drRecp("SSReductionEftDate")
                dtRow("CurrentPayment") = drRecp("CurrentPayment")
                dtRow("EmpPreTaxCurrentPayment") = drRecp("EmpPreTaxCurrentPayment")
                dtRow("EmpPostTaxCurrentPayment") = drRecp("EmpPostTaxCurrentPayment")
                dtRow("YmcaPreTaxCurrentPayment") = drRecp("YmcaPreTaxCurrentPayment")
                dtRow("EmpPreTaxRemainingReserves") = drRecp("EmpPreTaxRemainingReserves")
                dtRow("EmpPostTaxRemainingReserves") = drRecp("EmpPostTaxRemainingReserves")
                dtRow("YmcapreTaxRemainingReserves") = drRecp("YmcapreTaxRemainingReserves")
                dtRow("RecipientSplitPercent") = drRecp("RecipientSplitPercent")
                dtRow("guiAnnuityID") = drRecp("guiAnnuityID")
                dtRow("IncludeSpecialdev") = drRecp("IncludeSpecialdev")
                dtRow("AnnuityTotal") = SelectedAnnuityAmount
                dtRow("RecipientFundEventID") = Me.String_RecptFundEventID
                dtRow("RecptRetireeID") = Me.String_RecptRetireeID
                ' dtRow("splitType") = RadioButtonListPlanType.SelectedItem.Value.ToString()
                dtRow("splitType") = Me.SplitPlanTypeOption.Trim 'Added by - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                dtRow("AdjustmentBasisCode") = drRecp("AdjustmentBasisCode")
                dtRow("guiAnnuityJointSurvivorsID") = drRecp("guiAnnuityJointSurvivorsID")
                dtBeneCurrentSplitTemp.Rows.Add(dtRow)
            Next
            Return True

        Catch
            Throw
        End Try

    End Function
    Private Sub AddDataToRemainingBalance(ByVal drRecps() As DataRow, ByRef dtBeneCurrentSplitTemp As DataTable)

        Dim dtRow As DataRow

        Try

            For Each drRecp As DataRow In drRecps
                dtRow = dtBeneCurrentSplitTemp.NewRow
                dtRow.ItemArray = drRecp.ItemArray
                dtBeneCurrentSplitTemp.Rows.Add(dtRow)
            Next

        Catch
            Throw
        End Try

    End Sub

    '***************************************************************************************************//
    'Function Name             :AddDataToParticipantTable   Created on  : 14/04/08                      //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :dtTableParticipant                                                                        //
    'Method Description        :This Function is used to add the data to the datatable of Participant   //
    '***************************************************************************************************//
    Private Function AddDataToParticipantTable(ByVal dtTableParticipant As DataTable)
        Dim dsRowcount As Integer
        Dim dsRow As DataRow
        Dim dtRow As DataRow
        Try
            If HelperFunctions.isNonEmpty(Me.Session_Datatable_DsPartBal) Then 'PPP | 01/20/2017 | YRS-AT-3299 | Replaced Session("dsPartBal") with Me.Session_Datatable_DsPartBal property and condition as well [OLD:If Not Session("dsPartBal") Is Nothing Then]
                dsPartTotal = Me.Session_Datatable_DsPartBal 'PPP | 01/20/2017 | YRS-AT-3299 | Replaced Session("dsPartBal") with Me.Session_Datatable_DsPartBal and casting was not required [OLD: DirectCast(Session("dsPartBal"), DataSet)]
            End If
            'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START
            'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
            Dim drParBal As DataRow()
            If Not String.IsNullOrEmpty(Me.SplitPlanTypeOption) Then
                If Me.SplitPlanTypeOption.Trim().ToLower() = EnumPlanTypes.BOTH.ToString().ToLower() Then
                    drParBal = dsPartTotal.Tables(0).Select()
                Else
                    drParBal = dsPartTotal.Tables(0).Select(String.Format("PlanType='{0}'", Me.SplitPlanTypeOption)) 'PPP | 01/20/2017 | YRS-AT-3299 | Removed + operation on string [OLD:dsPartTotal.Tables(0).Select("PlanType='" + Me.SplitPlanTypeOption + "'")]
                End If

                'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
                'For dsRowcount = 0 To dsPartTotal.Tables(0).Rows.Count - 1
                For dsRowcount = 0 To drParBal.Length - 1
                    'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END
                    dtRow = dtTableParticipant.NewRow()
                    dsRow = drParBal(dsRowcount)
                    'dsRow = dsPartBal.Tables(0).Rows(dsRowcount)
                    dtRow("selected") = "True"
                    dtRow("AnnuitySourceCode") = dsRow("AnnuitySourceCode")
                    dtRow("PlanType") = dsRow("PlanType")
                    dtRow("PurchaseDate") = dsRow("PurchaseDate")
                    dtRow("AnnuityType") = dsRow("AnnuityType")
                    dtRow("SSLevelingAmt") = dsRow("SSLevelingAmt")
                    dtRow("SSReductionAmt") = dsRow("SSReductionAmt")
                    dtRow("SSReductionEftDate") = dsRow("SSReductionEftDate")
                    dtRow("CurrentPayment") = dsRow("CurrentPayment")
                    dtRow("EmpPreTaxCurrentPayment") = dsRow("EmpPreTaxCurrentPayment")
                    dtRow("EmpPostTaxCurrentPayment") = dsRow("EmpPostTaxCurrentPayment")
                    dtRow("YmcaPreTaxCurrentPayment") = dsRow("YmcaPreTaxCurrentPayment")
                    dtRow("EmpPreTaxRemainingReserves") = dsRow("EmpPreTaxRemainingReserves")
                    dtRow("EmpPostTaxRemainingReserves") = dsRow("EmpPostTaxRemainingReserves")
                    dtRow("YmcapreTaxRemainingReserves") = dsRow("YmcapreTaxRemainingReserves")
                    dtRow("guiAnnuityID") = dsRow("guiAnnuityID")
                    dtRow("AdjustmentBasisCode") = dsRow("chvAdjustmentBasisCode")
                    dtRow("guiAnnuityJointSurvivorsID") = dsRow("guiAnnuityJointSurvivorsID")
                    dtRow("splitType") = Me.SplitPlanTypeOption
                    dtTableParticipant.Rows.Add(dtRow)

                Next
            End If
            Return True

        Catch
            Throw
        End Try

    End Function
    '***************************************************************************************************//
    'Function Name             :CheckValidate               Created on  : 14/04/08                      //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function will validate the for the annuity to split checkbox in    //
    '                           annuity detail grid in Annuity Tab.                                     //
    '***************************************************************************************************//
    Private Function CheckValidate()
        Dim dgItem As DataGridItem

        Try

            For Each dgItem In DataGridWorkSheet.Items
                Dim SelectCheckbox As CheckBox = CType(dgItem.Cells(1).Controls(1), CheckBox)
                If SelectCheckbox.Checked = True Then
                    Return True
                End If
            Next
            Return False

        Catch
            Throw
        End Try
    End Function

#Region "Pro-Rate Percentage"
    '***************************************************************************************************//
    'Function Name             :ConvertToPercentage         Created on  : 14/04/08                      //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function will Pro-Rate the percentage for the amount enter in the  //
    '                           Amount to split textbox in Annuity Tab.                                 //
    '***************************************************************************************************//

    'Private Function ConvertToPercentage()
    '    Dim dgItem As DataGridItem
    '    Dim PartTotal As Double
    '    Dim TotalMaxAmount As Double = 100.0
    '    Try
    '        For Each dgItem In DataGridWorkSheet.Items
    '            Dim SelectCheckbox As CheckBox = CType(dgItem.Cells(0).Controls(1), CheckBox)
    '            If SelectCheckbox.Checked = True Then
    '                PartTotal = PartTotal + Convert.ToDouble(dgItem.Cells(6).Text())
    '            End If
    '        Next
    '        PercentageForSplit = Convert.ToDouble(TextBoxAmountWorkSheet.Text) * 100 / PartTotal
    '        If PercentageForSplit > TotalMaxAmount Then     'add kk Or PercentageForSplit < 0.01 
    '            Return False
    '        Else
    '            Return True
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    Private Function ConvertToPercentage()
        Dim dgItem As DataGridItem
        'Dim PartTotal As Double
        Dim PartTotal As Decimal
        Dim TotalMaxAmount As Decimal = 100.0
        'Dim dtPartAccount As DataTable 'PPP | 01/31/2017 | YRS-AT-3299 | Unused variable
        'Dim partTotalForValidation As Double 'PPP | 01/31/2017 | YRS-AT-3299 | Unused variable
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->ConvertToPercentage", "START") 'PPP | 01/31/2017 | YRS-AT-3299 | Started the trace

            For Each dgItem In DataGridWorkSheet.Items
                Dim SelectCheckbox As CheckBox = CType(dgItem.Cells(1).Controls(1), CheckBox)
                Dim sharedBenefitCheckbox As CheckBox = CType(dgItem.Cells(2).Controls(1), CheckBox)
                'If SelectCheckbox.Checked = True 'SP 2014.06.25 YRS 5.0-633
                'SP 2014.06.25  BT-2445\YRS 5.0-633 -Start
                If SelectCheckbox.Checked = True And sharedBenefitCheckbox.Checked = False And IsAnnuityTypeScocialSecurityLevelling(dgItem.Cells(6).Text.Trim()) Then
                    PartTotal = PartTotal + (Convert.ToDecimal(dgItem.Cells(8).Text.Trim()) + Convert.ToDecimal(dgItem.Cells(9).Text().Trim()) + Convert.ToDecimal(dgItem.Cells(10).Text().Trim()) + Convert.ToDecimal(dgItem.Cells(14).Text().Trim()) - Convert.ToDecimal(dgItem.Cells(15).Text().Trim()))
                ElseIf SelectCheckbox.Checked Then
                    PartTotal = PartTotal + Convert.ToDecimal(dgItem.Cells(7).Text())
                End If
                'SP 2014.06.25  BT-2445\YRS 5.0-633 -End
            Next

            'START: PPP | 01/31/2017 | YRS-AT-3299 | Tracing how much amount was devided by which total
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->ConvertToPercentage", String.Format("Found amount is '{0}' and it is devided by total '{1}'", TextBoxAmountWorkSheet.Text, PartTotal.ToString()))
            'END: PPP | 01/31/2017 | YRS-AT-3299 | Tracing how much amount was devided by which total

            PercentageForSplit = Math.Round(Convert.ToDecimal(TextBoxAmountWorkSheet.Text) * 100 / PartTotal, 2, MidpointRounding.AwayFromZero)

            'START: PPP | 01/31/2017 | YRS-AT-3299 | Tracing calculated percentage
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->ConvertToPercentage", String.Format("Calculated % is '{0}'", PercentageForSplit.ToString()))
            'END: PPP | 01/31/2017 | YRS-AT-3299 | Tracing calculated percentage

            If PercentageForSplit > TotalMaxAmount Then 'add kk Or PercentageForSplit < 0.01
                Return False
            Else
                Return True
            End If

        Catch
            Throw
            'START: PPP | 01/31/2017 | YRS-AT-3299 | Added Finally block to end the trace
        Finally
            dgItem = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->ConvertToPercentage", "END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            'END: PPP | 01/31/2017 | YRS-AT-3299 | Added Finally block to end the trace
        End Try

    End Function
#End Region

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Not using LoadBeneficiaries 
    '#Region "LoadBeneficiarys"
    '    '***************************************************************************************************//
    '    'Function Name             :LoadBeneficiarys            Created on  : 09/009/08                      //
    '    'Created By                :KranthiKumar                Modified On :                               //
    '    'Modified By               :                                                                        //
    '    'Modify Reason             :                                                                        //
    '    'Param Description         :                                                                        //
    '    'Method Description        :This Method will load's the beneficiary's data to the controls.           //
    '    '                                                                                                   //
    '    '***************************************************************************************************//
    '    Private Sub LoadBeneficiaries(ByVal parameterPersonID As String)
    '        Dim drRecps() As DataRow
    '        Dim drBenef() As DataRow
    '        Dim l_Double_SelectedAmount As Double
    '        Try
    '            'add kranthi 070808.
    '            'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '            'TextBoxPercentageWorkSheet.Text = "0.00"
    '            'TextBoxAmountWorkSheet.Text = "0.00"
    '            'Commented by ENd - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '            CheckBoxSpecialDividends.Checked = False
    '            'add kranthi 070808.
    '            'lblCurrentSplitOptions.Visible = False
    '            Dim strIntialSplitType As String
    '            dtRecptAccounttemp = CreateRecipientTableTemp()
    '            'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '            'DataGridWorkSheet2.DataSource = Nothing
    '            'DataGridWorkSheet2.DataBind()
    '            'Commented by End - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    '            Me.string_Benif_PersonID = parameterPersonID
    '            'Added to Get the Fundeventid of recipent 09-09-2008
    '            'dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
    '            dtBenifAccount = Me.Session_Datatable_DtBenifAccount

    '            drBenef = dtBenifAccount.Select("ID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString() & "'")
    '            Dim drDataRowFundEventid As DataRow
    '            If (drBenef.Length <> 0) Then
    '                drDataRowFundEventid = drBenef.GetValue(0)
    '                Me.String_RecptFundEventID = drDataRowFundEventid("FundEventID")
    '                Me.String_RecptRetireeID = drDataRowFundEventid("RecptRetireeID")
    '            End If
    '            'Added to Get the Fundeventid of recipent 09-09-2008

    '            If Not Me.Session_Datatable_DtRecptAccount Is Nothing Then
    '                'dtRecptAccount = CType(Me.Session_datatable_dtRecptAccount, DataTable)
    '                dtRecptAccount = Me.Session_Datatable_DtRecptAccount
    '                'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '                'If dtRecptAccount.Rows.Count = 0 Then
    '                '    RadioButtonListPlanType.Enabled = True
    '                'Else
    '                '    RadioButtonListPlanType.Enabled = False
    '                'End If
    '                'Commented by END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    '                drRecps = dtRecptAccount.Select("RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")
    '                Dim drDataRowAnnuity As DataRow
    '                If (drRecps.Length <> 0) Then
    '                    drDataRowAnnuity = drRecps.GetValue(0)
    '                    l_Double_SelectedAmount = drDataRowAnnuity("AnnuityTotal")
    '                    'Added by Dilip for selecting Annuities on 11-11-2008
    '                    UnselectAnnuities(False)
    '                    SelectAnnuities(drRecps)
    '                    'Added by Dilip for selecting Annuities on 11-11-2008
    '                    'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '                    strIntialSplitType = drDataRowAnnuity("splitType")
    '                    GetAnnuitiesBalancesDetails(strIntialSplitType)
    '                Else
    '                    Me.SplitPlanTypeOption = Nothing
    '                    DataGridRecipientAnnuitiesBalance.DataSource = Nothing ' Recipient's Annuities 
    '                    DataGridRecipientAnnuitiesBalance.DataBind()
    '                    DatagridParticipantsBalance.DataSource = Nothing
    '                    DatagridParticipantsBalance.DataBind()
    '                    EnabledAndVisibleControls(False, False, True, True, True, False, False, False, False, False, False, False, True, False, False, False, False, False, String.Empty)
    '                    'End- Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '                End If
    '                'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '                AddDataToRecptAccounttemp(drRecps, l_Double_SelectedAmount, dtRecptAccounttemp)
    '                Me.Session_Datatable_DtRecptAccountTemp = dtRecptAccounttemp
    '                'DataGridRecipientAnnuitiesBalance.DataSource = dtRecptAccounttemp
    '                'DataGridRecipientAnnuitiesBalance.DataBind()
    '                'If (drRecps.Length > 0) Then
    '                '    If TextBoxPercentageWorkSheet.Enabled = False Then
    '                '        TextBoxPercentageWorkSheet.Enabled = True
    '                '        TextBoxAmountWorkSheet.Enabled = False
    '                '    End If
    '                '    'TextBoxPercentageWorkSheet.Text = Math.Round(Convert.ToDouble(dtRecptAccounttemp.Rows(0).Item("RecipientSplitPercent")), 2)
    '                '    TextBoxAmountWorkSheet.Text = "0.00"
    '                '    'RadioButtonListPlanType.Enabled = False
    '                '    ButtonSplit.Enabled = False
    '                '    ButtonAdjust.Enabled = True
    '                '    ButtonReset.Enabled = True
    '                '    ButtonDocumentSave.Enabled = True

    '                '    If RadioButtonListSplitAmtType.Enabled = True Then
    '                '        RadioButtonListSplitAmtType.Enabled = False
    '                '        TextBoxAmountWorkSheet.Enabled = False
    '                '        TextBoxPercentageWorkSheet.Enabled = False
    '                '    End If

    '                '    'SP 2014.07.07  BT-2445\YRS 5.0-633-Start
    '                '    If (drRecps(0)("IsSplitPercentage").ToString.ToLower = "true") Then
    '                '        RadioButtonListSplitAmtType.SelectedIndex = 1
    '                '        TextBoxPercentageWorkSheet.Text = drRecps(0)("SplitAmount").ToString
    '                '    ElseIf (drRecps(0)("IsSplitPercentage").ToString.ToLower = "false") Then
    '                '        RadioButtonListSplitAmtType.SelectedIndex = 0
    '                '        TextBoxAmountWorkSheet.Text = drRecps(0)("SplitAmount").ToString
    '                '    End If
    '                'SP 2014.07.07  BT-2445\YRS 5.0-633-End

    '                'add kranthi 070808.

    '                ' CheckBoxSpecialDividends.Checked = dtRecptAccounttemp.Rows(0).Item("IncludeSpecialdev")
    '                'Commented by END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    '            Else
    '                'Added by Dilip for selecting Annuities on 11-11-2008
    '                'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    '                Me.SplitPlanTypeOption = Nothing
    '                EnabledAndVisibleControls(False, False, True, True, True, False, False, False, False, False, False, False, True, False, False, False, False, False, String.Empty)
    '                DataGridRecipientAnnuitiesBalance.DataSource = Nothing ' Recipient's Annuities 
    '                DataGridRecipientAnnuitiesBalance.DataBind()
    '                DatagridParticipantsBalance.DataSource = Nothing
    '                DatagridParticipantsBalance.DataBind()
    '                'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    '                UnselectAnnuities(True)
    '                'Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    '                'Added by Dilip for selecting Annuities on 11-11-2008
    '                'RadioButtonListSplitAmtType.Items(1).Selected = True
    '                'RadioButtonListSplitAmtType.Items(0).Selected = False
    '                'If TextBoxPercentageWorkSheet.Enabled = False Then
    '                '    TextBoxPercentageWorkSheet.Enabled = True
    '                '    TextBoxAmountWorkSheet.Enabled = False
    '                'End If
    '                'TextBoxPercentageWorkSheet.Text = "0.00"
    '                'TextBoxAmountWorkSheet.Text = "0.00"
    '                'ButtonSplit.Enabled = False
    '                'ButtonAdjust.Enabled = False

    '                ''Added Kranthi 240809
    '                'If RadioButtonListSplitAmtType.Enabled = False Then
    '                '    RadioButtonListSplitAmtType.Enabled = True
    '                '    RadioButtonListSplitAmtType.SelectedValue = "Percentage"
    '                '    TextBoxAmountWorkSheet.Enabled = False
    '                '    TextBoxPercentageWorkSheet.Enabled = True
    '                'End If

    '                'If dtRecptAccount.Rows.Count = 0 Then               'added kranthi 160908.
    '                '    ButtonDocumentSave.Enabled = False
    '                'Else
    '                '    ButtonDocumentSave.Enabled = True
    '                'End If

    '                'ButtonReset.Enabled = False
    '                'End If

    '                'Else
    '                'ButtonSplit.Enabled = False
    '                'ButtonAdjust.Enabled = False
    '                'Commented by END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    '            End If

    '        Catch
    '            Throw
    '        End Try
    '    End Sub
    '#End Region
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Not using LoadBeneficiaries 

#Region "CalculateSelectedAmount"
    '***************************************************************************************************//
    'Function Name             :CalculateSelectedAmount     Created on  : 02/09/2008                    //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function will Calculate Selected Amount (Annuity Tab)              //
    '***************************************************************************************************//
    Private Function CalculateSelectedAmount()
        'START: PPP | 01/20/2017 | YRS-AT-3299
        Dim gridItem As DataGridItem 'dgItem renamed as gridItem
        Dim selectedAnnuitiesTotal As Decimal 'PartTotal renamed as selectedAnnuitiesTotal and changed its datatype as decimal
        'Dim TotalMaxAmount As Double = 100.0 'unused variable so commenting it
        Dim checkboxControl As CheckBox  'Moved checkbox declaration out of Try block, also renamed it from SelectCheckbox to checkboxControl
        'END: PPP | 01/20/2017 | YRS-AT-3299
        Try
            selectedAnnuitiesTotal = 0
            For Each gridItem In DataGridWorkSheet.Items
                'START: PPP | 01/20/2017 | YRS-AT-3299 | Moved checkbox declaration out of Try block, also renamed it from SelectCheckbox to checkboxControl
                'Dim SelectCheckbox As CheckBox = CType(gridItem.Cells(1).Controls(1), CheckBox)
                checkboxControl = CType(gridItem.Cells(1).Controls(1), CheckBox)
                'END: PPP | 01/20/2017 | YRS-AT-3299 | Moved checkbox declaration out of Try block, also renamed it from SelectCheckbox to checkboxControl
                If checkboxControl.Checked = True Then
                    selectedAnnuitiesTotal = selectedAnnuitiesTotal + Convert.ToDecimal(gridItem.Cells(7).Text()) 'PPP | 01/20/2017 | YRS-AT-3299 | Changed Convert.ToDouble to Convert.ToDecimal, becuase selectedAnnuitiesTotal is now decimal
                End If
            Next
            Return selectedAnnuitiesTotal
        Catch
            Throw
        Finally
            checkboxControl = Nothing
            gridItem = Nothing
        End Try
    End Function
#End Region
#Region "Adjustment Recipant Balance"
    '***************************************************************************************************//
    'Function Name             :AdJustRecpBalance         Created on  : 14/04/08                        //
    'Created By                :Dilip Patada                Modified On :                               //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :dtTableParticipant,DataTableRecipant                                                                       //
    'Method Description        :This Function will AdJust the Recpcipant Balance when the adjust button //
    '                           click after change in the amount in the Recipent Annuity Grid           //
    '***************************************************************************************************//
    Private Function AdJustRecpBalance(ByVal DataTableRecipantTemp As DataTable, ByVal DataTableRecipant As DataTable)
        Dim iCount As Integer
        Dim dtRowRecipant As DataRow
        Dim dtRowRecipantTemp As DataRow
        Try
            For Each dtRowRecipantTemp In DataTableRecipantTemp.Rows
                For Each dtRowRecipant In DataTableRecipant.Rows
                    If dtRowRecipant("RecipientPersonID") = dtRowRecipantTemp("RecipientPersonID") And dtRowRecipant("guiAnnuityID") = dtRowRecipantTemp("guiAnnuityID") Then
                        dtRowRecipant.BeginEdit()
                        dtRowRecipant("SSLevelingAmt") = dtRowRecipantTemp("SSLevelingAmt")
                        dtRowRecipant("SSReductionAmt") = dtRowRecipantTemp("SSReductionAmt")
                        dtRowRecipant("CurrentPayment") = dtRowRecipantTemp("CurrentPayment")
                        dtRowRecipant("EmpPreTaxCurrentPayment") = dtRowRecipantTemp("EmpPreTaxCurrentPayment")
                        dtRowRecipant("EmpPostTaxCurrentPayment") = dtRowRecipantTemp("EmpPostTaxCurrentPayment")
                        dtRowRecipant("YmcaPreTaxCurrentPayment") = dtRowRecipantTemp("YmcaPreTaxCurrentPayment")
                        dtRowRecipant("EmpPreTaxRemainingReserves") = dtRowRecipantTemp("EmpPreTaxRemainingReserves")
                        dtRowRecipant("EmpPostTaxRemainingReserves") = dtRowRecipantTemp("EmpPostTaxRemainingReserves")
                        dtRowRecipant("YmcapreTaxRemainingReserves") = dtRowRecipantTemp("YmcapreTaxRemainingReserves")
                        dtRowRecipant("IncludeSpecialdev") = dtRowRecipantTemp("IncludeSpecialdev")
                        dtRowRecipant.EndEdit()
                        dtRowRecipant.AcceptChanges()
                    End If
                Next

            Next
            dtRecptAccount = Me.Session_Datatable_DtRecptAccount
        Catch
            Throw
        End Try

    End Function



    'Private Function UpdateDataToTable(ByVal RecptRetireeID As String, ByVal RecptPersId As String, ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal FundStatus As String, ByVal FlagNewBenf As Boolean, ByVal RecptFundEventID As String, ByVal SalutationCode As String, ByVal SuffixTitle As String, ByVal BirthDate As String, ByVal MaritalCode As String, ByVal EMail As String, ByVal PhoneNo As String, ByVal Add1 As String, ByVal Add2 As String, ByVal Add3 As String, ByVal City As String, ByVal State As String, ByVal zip As String, ByVal Country As String, ByVal dtBenfTable As DataTable) 'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Commented existing code
    'START: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not accepting it as an parameter, hardcoded it
    'Private Function UpdateDataToTable(ByVal RecptRetireeID As String, ByVal RecptPersId As String, ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal FundStatus As String, ByVal FlagNewBenf As Boolean, ByVal RecptFundEventID As String, ByVal SalutationCode As String, ByVal SuffixTitle As String, ByVal BirthDate As String, ByVal MaritalCode As String, ByVal GenderCode As String, ByVal EMail As String, ByVal PhoneNo As String, ByVal Add1 As String, ByVal Add2 As String, ByVal Add3 As String, ByVal City As String, ByVal State As String, ByVal zip As String, ByVal Country As String, ByVal dtBenfTable As DataTable) 'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Adding gender input parameter.
    Private Function UpdateDataToTable(ByVal RecptRetireeID As String, ByVal RecptPersId As String, ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal FlagNewBenf As Boolean, ByVal RecptFundEventID As String, ByVal SalutationCode As String, ByVal SuffixTitle As String, ByVal BirthDate As String, ByVal MaritalCode As String, ByVal GenderCode As String, ByVal EMail As String, ByVal PhoneNo As String, ByVal Add1 As String, ByVal Add2 As String, ByVal Add3 As String, ByVal City As String, ByVal State As String, ByVal zip As String, ByVal Country As String, ByVal dtBenfTable As DataTable) 'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Adding gender input parameter.
        'END: PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not accepting it as an parameter, hardcoded it
        Dim dtRowBenificiary As DataRow
        Try

            For Each dtRowBenificiary In dtBenfTable.Rows
                If dtRowBenificiary("SSNo") = SSNo Then
                    dtRowBenificiary.BeginEdit()
                    dtRowBenificiary("LastName") = LastName
                    dtRowBenificiary("FirstName") = FirstName
                    dtRowBenificiary("MiddleName") = MiddleName
                    dtRowBenificiary("FundStatus") = "RQ" 'PPP | 01/16/2017 | YRS-AT-3299 | RQ is the default fund status, So not accepting it as an parameter, hardcoded it
                    dtRowBenificiary("SalutationCode") = SalutationCode
                    dtRowBenificiary("SuffixTitle") = SuffixTitle
                    dtRowBenificiary("BirthDate") = BirthDate
                    dtRowBenificiary("MaritalCode") = MaritalCode
                    dtRowBenificiary("GenderCode") = GenderCode 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Updating the gender value.
                    dtRowBenificiary("EmailAddress") = EMail 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "EMail" to "EmailAddress", to support common recipient stagging db functionality
                    dtRowBenificiary("PhoneNumber") = PhoneNo 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "PhoneNo" to "PhoneNumber", to support common recipient stagging db functionality
                    dtRowBenificiary("Address1") = Add1 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add1" to "Address1", to support common recipient stagging db functionality
                    dtRowBenificiary("Address2") = Add2 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add2" to "Address2", to support common recipient stagging db functionality
                    dtRowBenificiary("Address3") = Add3 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "Add3" to "Address3", to support common recipient stagging db functionality
                    dtRowBenificiary("City") = City
                    dtRowBenificiary("State") = State
                    dtRowBenificiary("Zip") = zip 'PPP | 01/23/2017 | YRS-AT-3299 | Changing "zip" to "Zip", to support common recipient stagging db functionality
                    dtRowBenificiary("Country") = Country

                    dtRowBenificiary("Address_effectiveDate") = AddressWebUserControl1.EffectiveDate
                    dtRowBenificiary("BadAddress") = AddressWebUserControl1.IsBadAddress
                    dtRowBenificiary("AddressNote") = "Retired Qdro split:" + AddressWebUserControl1.Notes
                    dtRowBenificiary("AddressNote_bitImportant") = AddressWebUserControl1.BitImp
                    'added kranthi 141008
                    'dtRowBenificiary("IsSpouse") = IsSpouse

                    dtRowBenificiary.EndEdit()
                    dtRowBenificiary.AcceptChanges()
                    MaintainRecipientDetails(dtRowBenificiary, Me.String_QDRORequestID, YMCAObjects.DBActionType.UPDATE) 'PPP | 01/10/2017 | YRS-AT-3299 | Recipient data will be updated to staging table
                End If
            Next

            dtBenfTable = Me.Session_Datatable_DtBenifAccount
        Catch
            Throw
        End Try
    End Function

    ' "Adjustment Participant Balance"
    '******************************************************************************************************//
    'Function Name             :AdJustPartBalance         Created on  : 14/04/08                           //
    'Created By                :Dilip Patada                Modified On :                                  //
    'Modified By               :                                                                           //
    'Modify Reason             :                                                                           //
    'Param Description         :DataTableRecipantTemp,DataTablePartAccountbalance,                                                                         //
    'Method Description        :This Function will AdJust the Particiant Balance when the adjust button    //
    '                            click after change in the amount in the Recipent Annuity Grid             //
    '******************************************************************************************************//

    Private Function AdJustPartBalance(ByVal DataTableRecipantTemp As DataTable, ByVal DataTablePartAccountbalance As DataTable)
        Dim iCount As Integer
        Dim rowCount As Integer
        Dim dtRowRecipant As DataRow
        Dim dtRowParticipant As DataRow
        Dim dgItems As DataGridItem

        Try
            rowCount = DataTableRecipantTemp.Select("RecipientPersonID='" & Me.string_Benif_PersonID & "'").Length()
            For iCount = 0 To rowCount - 1
                dgItems = DataGridRecipientAnnuitiesBalance.Items(iCount)

                Dim TextSSLevelingAmt As TextBox = CType(dgItems.Cells(5).FindControl("TextBoxSSLevlingAmount"), TextBox)
                Dim TextSSReductionAmt As TextBox = CType(dgItems.Cells(6).FindControl("TextboxSSReductionAmount"), TextBox)
                Dim TextCurrentPayment As TextBox = CType(dgItems.Cells(8).FindControl("TextboxCurrentPayment"), TextBox)
                Dim TextEmpPreTaxCurrentPayment As TextBox = CType(dgItems.Cells(9).FindControl("TextboxEmpPreTaxCurrentPayment"), TextBox)
                Dim TextEmpPostTaxCurrentPayment As TextBox = CType(dgItems.Cells(10).FindControl("TextboxEmpPostTaxCurrentPayment"), TextBox)
                Dim TextYmcaPreTaxCurrentPayment As TextBox = CType(dgItems.Cells(11).FindControl("TextboxYmcaPreTaxCurrentPayment"), TextBox)
                Dim TextEmpPreTaxRemainingReserves As TextBox = CType(dgItems.Cells(12).FindControl("TextboxEmpPreTaxRemainingReserves"), TextBox)
                Dim TextEmpPostTaxRemainingReserves As TextBox = CType(dgItems.Cells(13).FindControl("TextboxEmpPostTaxRemainingReserves"), TextBox)
                Dim TextYmcapreTaxRemainingReserves As TextBox = CType(dgItems.Cells(14).FindControl("TextboxYmcaPreTaxRemainingReserves"), TextBox)

                For Each dtRowParticipant In DataTablePartAccountbalance.Rows
                    If dgItems.Cells(15).Text = dtRowParticipant("guiAnnuityID") Then
                        dtRowRecipant = DataTableRecipantTemp.Rows(iCount)

                        dtRowParticipant.BeginEdit()
                        dtRowRecipant.BeginEdit()
                        dtRowParticipant("SSLevelingAmt") = dtRowParticipant("SSLevelingAmt") + dtRowRecipant("SSLevelingAmt")
                        dtRowParticipant("SSReductionAmt") = dtRowParticipant("SSReductionAmt") + dtRowRecipant("SSReductionAmt")
                        dtRowParticipant("CurrentPayment") = dtRowParticipant("CurrentPayment") + dtRowRecipant("CurrentPayment")
                        dtRowParticipant("EmpPreTaxCurrentPayment") = dtRowParticipant("EmpPreTaxCurrentPayment") + dtRowRecipant("EmpPreTaxCurrentPayment")
                        dtRowParticipant("EmpPostTaxCurrentPayment") = dtRowParticipant("EmpPostTaxCurrentPayment") + dtRowRecipant("EmpPostTaxCurrentPayment")
                        dtRowParticipant("YmcaPreTaxCurrentPayment") = dtRowParticipant("YmcaPreTaxCurrentPayment") + dtRowRecipant("YmcaPreTaxCurrentPayment")
                        dtRowParticipant("EmpPreTaxRemainingReserves") = dtRowParticipant("EmpPreTaxRemainingReserves") + dtRowRecipant("EmpPreTaxRemainingReserves")
                        dtRowParticipant("EmpPostTaxRemainingReserves") = dtRowParticipant("EmpPostTaxRemainingReserves") + dtRowRecipant("EmpPostTaxRemainingReserves")
                        dtRowParticipant("YmcapreTaxRemainingReserves") = dtRowParticipant("YmcapreTaxRemainingReserves") + dtRowRecipant("YmcapreTaxRemainingReserves")


                        dtRowRecipant("SSLevelingAmt") = TextSSLevelingAmt.Text.Trim
                        dtRowRecipant("SSReductionAmt") = TextSSReductionAmt.Text.Trim
                        dtRowRecipant("CurrentPayment") = TextCurrentPayment.Text.Trim
                        dtRowRecipant("EmpPreTaxCurrentPayment") = TextEmpPreTaxCurrentPayment.Text.Trim
                        dtRowRecipant("EmpPostTaxCurrentPayment") = TextEmpPostTaxCurrentPayment.Text.Trim
                        dtRowRecipant("YmcaPreTaxCurrentPayment") = TextYmcaPreTaxCurrentPayment.Text.Trim
                        dtRowRecipant("EmpPreTaxRemainingReserves") = TextEmpPreTaxRemainingReserves.Text.Trim
                        dtRowRecipant("EmpPostTaxRemainingReserves") = TextEmpPostTaxRemainingReserves.Text.Trim
                        dtRowRecipant("YmcapreTaxRemainingReserves") = TextYmcapreTaxRemainingReserves.Text.Trim


                        dtRowParticipant("SSLevelingAmt") = dtRowParticipant("SSLevelingAmt") - TextSSLevelingAmt.Text.Trim
                        dtRowParticipant("SSReductionAmt") = dtRowParticipant("SSReductionAmt") - TextSSReductionAmt.Text.Trim
                        dtRowParticipant("CurrentPayment") = dtRowParticipant("CurrentPayment") - TextCurrentPayment.Text.Trim
                        dtRowParticipant("EmpPreTaxCurrentPayment") = dtRowParticipant("EmpPreTaxCurrentPayment") - TextEmpPreTaxCurrentPayment.Text.Trim
                        dtRowParticipant("EmpPostTaxCurrentPayment") = dtRowParticipant("EmpPostTaxCurrentPayment") - TextEmpPostTaxCurrentPayment.Text.Trim
                        dtRowParticipant("YmcaPreTaxCurrentPayment") = dtRowParticipant("YmcaPreTaxCurrentPayment") - TextYmcaPreTaxCurrentPayment.Text.Trim
                        dtRowParticipant("EmpPreTaxRemainingReserves") = dtRowParticipant("EmpPreTaxRemainingReserves") - TextEmpPreTaxRemainingReserves.Text.Trim
                        dtRowParticipant("EmpPostTaxRemainingReserves") = dtRowParticipant("EmpPostTaxRemainingReserves") - TextEmpPostTaxRemainingReserves.Text.Trim
                        dtRowParticipant("YmcapreTaxRemainingReserves") = dtRowParticipant("YmcapreTaxRemainingReserves") - TextYmcapreTaxRemainingReserves.Text.Trim
                        dtRowParticipant.EndEdit()
                        dtRowRecipant.EndEdit()
                        dtRowParticipant.AcceptChanges()
                        dtRowRecipant.AcceptChanges()
                    End If
                Next

            Next
            dtRecptAccount = Me.Session_Datatable_DtRecptAccount
            dtRecptAccounttemp = Me.Session_Datatable_DtRecptAccountTemp
            dtPartAccount = Me.Session_Datatable_DtPartAccount
        Catch
            Throw
        End Try

    End Function
#End Region

#Region "Split Validation"
    '*******************************************************************************************************//
    'Function Name             :ValidateSplit             Created on  : 14/04/08                            //
    'Created By                :Dilip Patada              Modified On :                                     //
    'Modified By               :                                                                            //
    'Modify Reason             :                                                                            //
    'Param Description         :DataTableParticipant,DataTablePartAccountbalance.                           //
    'Method Description        :This Function will Validate all condition for spliting the annuity like     //
    '                           is Amount  enter will be within the range of Recipant balance Amount etc.   //
    '*******************************************************************************************************//

    'Private Function ValidateSplit(ByVal DataTableParticipant As DataTable, ByVal DataTablePartAccountbalance As DataTable)
    '    Dim l_int_iCount As Integer
    '    'Dim l_Double_PartAmountBalance As Double
    '    'Dim l_Double_PartAmountBalanceAfterSplit As Double

    '    'Dim l_Double_SSLevelingAmtP As Double = 0.0
    '    'Dim l_Double_SSReductionAmtP As Double = 0.0
    '    'Dim l_Double_CurrentPaymentP As Double = 0.0
    '    'Dim l_Double_EmpPreTaxCurrentPaymentP As Double = 0.0
    '    'Dim l_Double_EmpPostTaxCurrentPaymentP As Double = 0.0
    '    'Dim l_Double_YmcaPreTaxCurrentPaymentP As Double = 0.0
    '    'Dim l_Double_EmpPreTaxRemainingReservesP As Double = 0.0
    '    'Dim l_Double_EmpPostTaxRemainingReservesP As Double = 0.0
    '    'Dim l_Double_YmcapreTaxRemainingReservesP As Double = 0.0

    '    'Dim l_Double_SSLevelingAmtPBalance As Double = 0.0
    '    'Dim l_Double_SSReductionAmtPBalance As Double = 0.0
    '    'Dim l_Double_CurrentPaymentPBalance As Double = 0.0
    '    'Dim l_Double_EmpPreTaxCurrentPaymentPBalance As Double = 0.0
    '    'Dim l_Double_EmpPostTaxCurrentPaymentPBalance As Double = 0.0
    '    'Dim l_Double_YmcaPreTaxCurrentPaymentPBalance As Double = 0.0
    '    'Dim l_Double_EmpPreTaxRemainingReservesPBalance As Double = 0.0
    '    'Dim l_Double_EmpPostTaxRemainingReservesPBalance As Double = 0.0
    '    'Dim l_Double_YmcapreTaxRemainingReservesPBalance As Double = 0.0




    '    Dim l_Decimal_CurrentPaymentP As Decimal = 0.0
    '    Dim l_Decimal_CurrentPaymentPBalance As Decimal = 0.0

    '    Dim dtRowParticipant As DataRow
    '    Dim dtRowPartAccountbalance As DataRow
    '    Dim dgItems As DataGridItem

    '    Dim decCurrentPaymentAfterReduction As Decimal = 0.0  'SR:2014.06.26  BT-2445\YRS 5.0-633
    '    Dim dtRecptAccount As DataTable  'SP 2014.07.02  BT-2445\YRS 5.0-633


    '    Try
    '        l_int_iCount = DataTableParticipant.Rows.Count
    '        If DataTablePartAccountbalance.Rows.Count = 0 Then
    '            For l_int_iCount = 0 To DataTableParticipant.Rows.Count - 1
    '                dtRowParticipant = DataTableParticipant.Rows(l_int_iCount)
    '                dgItems = DataGridWorkSheet.Items(l_int_iCount)
    '                Dim SelectCheckbox As CheckBox = CType(dgItems.Cells(1).Controls(1), CheckBox)
    '                Dim bitShareBenefit As CheckBox = CType(dgItems.Cells(2).Controls(1), CheckBox)
    '                'Start -SR:2014.06.26  BT-2445\YRS 5.0-633 - Following if condition has been modified to get Current payment after reduction amount
    '                If SelectCheckbox.Checked = True AndAlso bitShareBenefit.Checked = False And IsAnnuityTypeScocialSecurityLevelling(dtRowParticipant("Annuitytype").ToString()) Then
    '                    decCurrentPaymentAfterReduction = dtRowParticipant("EmpPreTaxCurrentPayment") + dtRowParticipant("EmpPostTaxCurrentPayment") + dtRowParticipant("YmcaPreTaxCurrentPayment") + dtRowParticipant("SSLevelingAmt") - dtRowParticipant("SSReductionAmt")
    '                    l_Decimal_CurrentPaymentP = l_Decimal_CurrentPaymentP + decCurrentPaymentAfterReduction
    '                    l_Decimal_CurrentPaymentPBalance = l_Decimal_CurrentPaymentPBalance + (decCurrentPaymentAfterReduction * PercentageForSplit) / 100
    '                Else
    '                    l_Decimal_CurrentPaymentP = l_Decimal_CurrentPaymentP + dtRowParticipant("CurrentPayment")
    '                    l_Decimal_CurrentPaymentPBalance = l_Decimal_CurrentPaymentPBalance + (dtRowParticipant("CurrentPayment") * PercentageForSplit / 100)
    '                End If
    '                'End - SR:2014.06.26  BT-2445\YRS 5.0-633 - Following if condition has been modified to get Current payment after reduction amount
    '            Next
    '            l_Decimal_CurrentPaymentPBalance = Math.Round(l_Decimal_CurrentPaymentPBalance, 2)
    '            If RadioButtonListSplitAmtType.Items(0).Selected Then
    '                If Convert.ToDouble(TextBoxAmountWorkSheet.Text.Trim < 0.01) Then
    '                    Return False
    '                End If
    '            Else
    '                If PercentageForSplit > 100 Or PercentageForSplit < 1 Then
    '                    Return False
    '                End If
    '            End If

    '            If l_Decimal_CurrentPaymentPBalance > Math.Round(l_Decimal_CurrentPaymentP, 2) And RadioButtonListSplitAmtType.Items(1).Selected = True Then
    '                Return False
    '            End If
    '            If l_Decimal_CurrentPaymentP < Convert.ToDecimal(TextBoxAmountWorkSheet.Text) And RadioButtonListSplitAmtType.Items(0).Selected = True Then
    '                Return False
    '            End If
    '            Return True
    '        Else
    '            For l_int_iCount = 0 To DataTableParticipant.Rows.Count - 1
    '                dtRowParticipant = DataTableParticipant.Rows(l_int_iCount)
    '                dtRowPartAccountbalance = DataTablePartAccountbalance.Rows(l_int_iCount)
    '                dgItems = DataGridWorkSheet.Items(l_int_iCount)
    '                Dim SelectCheckbox As CheckBox = CType(dgItems.Cells(1).Controls(1), CheckBox)
    '                Dim bitShareBenefit As CheckBox = CType(dgItems.Cells(2).Controls(1), CheckBox) 'SP 2014.07.02  BT-2445\YRS 5.0-633
    '                If SelectCheckbox.Checked = True And dtRowParticipant("guiAnnuityID") = dtRowPartAccountbalance("guiAnnuityID") Then

    '                    'l_Double_SSLevelingAmtP = l_Double_SSLevelingAmtP + dtRowParticipant("SSLevelingAmt")
    '                    'l_Double_SSReductionAmtP = l_Double_SSReductionAmtP + dtRowParticipant("SSReductionAmt")
    '                    'l_Double_CurrentPaymentP = l_Double_CurrentPaymentP + dtRowParticipant("CurrentPayment")
    '                    'l_Double_EmpPreTaxCurrentPaymentP = l_Double_EmpPreTaxCurrentPaymentP + dtRowParticipant("EmpPreTaxCurrentPayment")
    '                    'l_Double_EmpPostTaxCurrentPaymentP = l_Double_EmpPostTaxCurrentPaymentP + dtRowParticipant("EmpPostTaxCurrentPayment")
    '                    'l_Double_YmcaPreTaxCurrentPaymentP = l_Double_YmcaPreTaxCurrentPaymentP + dtRowParticipant("YmcaPreTaxCurrentPayment")
    '                    'l_Double_EmpPreTaxRemainingReservesP = l_Double_EmpPreTaxRemainingReservesPBalance + dtRowParticipant("EmpPreTaxRemainingReserves")
    '                    'l_Double_EmpPostTaxRemainingReservesP = l_Double_EmpPostTaxRemainingReservesP + dtRowParticipant("EmpPostTaxRemainingReserves")
    '                    'l_Double_YmcapreTaxRemainingReservesP = l_Double_YmcapreTaxRemainingReservesP + dtRowParticipant("YmcapreTaxRemainingReserves")

    '                    'SP 2014.07.02  BT-2445\YRS 5.0-633 -Start
    '                    If SelectCheckbox.Checked = True AndAlso bitShareBenefit.Checked = False And IsAnnuityTypeScocialSecurityLevelling(dtRowParticipant("Annuitytype").ToString()) Then
    '                        decCurrentPaymentAfterReduction = decCurrentPaymentAfterReduction + (dtRowParticipant("EmpPreTaxCurrentPayment") + dtRowParticipant("EmpPostTaxCurrentPayment") + dtRowParticipant("YmcaPreTaxCurrentPayment") + dtRowParticipant("SSLevelingAmt") - dtRowParticipant("SSReductionAmt"))
    '                        l_Decimal_CurrentPaymentP = l_Decimal_CurrentPaymentP + decCurrentPaymentAfterReduction
    '                        l_Decimal_CurrentPaymentPBalance = l_Decimal_CurrentPaymentPBalance + Math.Round(decCurrentPaymentAfterReduction * PercentageForSplit / 100, 2)
    '                    Else

    '                        l_Decimal_CurrentPaymentP = l_Decimal_CurrentPaymentP + dtRowParticipant("CurrentPayment")
    '                        ' l_Double_CurrentPaymentPBalance = l_Double_CurrentPaymentPBalance + dtRowPartAccountbalance("CurrentPayment")
    '                        l_Decimal_CurrentPaymentPBalance = l_Decimal_CurrentPaymentPBalance + Math.Round(dtRowParticipant("CurrentPayment") * PercentageForSplit / 100, 2)
    '                    End If
    '                    'SP 2014.07.02  BT-2445\YRS 5.0-633 -End

    '                    'SP 2014.07.02  BT-2445\YRS 5.0-633 -Start
    '                    'l_Double_SSReductionAmtPBalance = l_Double_SSReductionAmtPBalance + dtRowPartAccountbalance("SSLevelingAmt")
    '                    'l_Double_SSReductionAmtPBalance = l_Double_SSReductionAmtPBalance - Math.Round(dtRowParticipant("SSLevelingAmt") * PercentageForSplit / 100, 2)

    '                    'l_Double_SSLevelingAmtPBalance = l_Double_SSLevelingAmtPBalance + dtRowPartAccountbalance("SSReductionAmt")
    '                    'l_Double_SSLevelingAmtPBalance = l_Double_SSLevelingAmtPBalance - Math.Round(dtRowParticipant("SSReductionAmt") * PercentageForSplit / 100, 2)

    '                    'l_Double_CurrentPaymentPBalance = l_Double_CurrentPaymentPBalance + dtRowPartAccountbalance("CurrentPayment")
    '                    'l_Double_CurrentPaymentPBalance = l_Double_CurrentPaymentPBalance - Math.Round(dtRowParticipant("CurrentPayment") * PercentageForSplit / 100, 2)

    '                    'SP 2014.07.02  BT-2445\YRS 5.0-633 -End

    '                    'l_Double_EmpPreTaxCurrentPaymentPBalance = l_Double_EmpPreTaxCurrentPaymentPBalance + dtRowPartAccountbalance("EmpPreTaxCurrentPayment")
    '                    'l_Double_EmpPreTaxCurrentPaymentPBalance = l_Double_EmpPreTaxCurrentPaymentPBalance - Math.Round(dtRowParticipant("EmpPreTaxCurrentPayment") * PercentageForSplit / 100, 2)

    '                    'l_Double_EmpPostTaxCurrentPaymentPBalance = l_Double_EmpPostTaxCurrentPaymentPBalance + dtRowPartAccountbalance("EmpPostTaxCurrentPayment")
    '                    'l_Double_EmpPostTaxCurrentPaymentPBalance = l_Double_EmpPostTaxCurrentPaymentPBalance - Math.Round(dtRowParticipant("EmpPostTaxCurrentPayment") * PercentageForSplit / 100, 2)

    '                    'l_Double_YmcaPreTaxCurrentPaymentPBalance = l_Double_YmcaPreTaxCurrentPaymentPBalance + dtRowPartAccountbalance("YmcaPreTaxCurrentPayment")
    '                    'l_Double_YmcaPreTaxCurrentPaymentPBalance = l_Double_YmcaPreTaxCurrentPaymentPBalance - Math.Round(dtRowParticipant("YmcaPreTaxCurrentPayment") * PercentageForSplit / 100, 2)

    '                    'l_Double_EmpPreTaxRemainingReservesPBalance = l_Double_EmpPreTaxRemainingReservesPBalance + dtRowPartAccountbalance("EmpPreTaxRemainingReserves")
    '                    'l_Double_EmpPreTaxRemainingReservesPBalance = l_Double_EmpPreTaxRemainingReservesPBalance - Math.Round(dtRowParticipant("EmpPreTaxRemainingReserves") * PercentageForSplit / 100, 2)

    '                    'l_Double_EmpPostTaxRemainingReservesPBalance = l_Double_EmpPostTaxRemainingReservesPBalance + dtRowPartAccountbalance("EmpPostTaxRemainingReserves")
    '                    'l_Double_EmpPostTaxRemainingReservesPBalance = l_Double_EmpPostTaxRemainingReservesPBalance - Math.Round(dtRowParticipant("EmpPostTaxRemainingReserves") * PercentageForSplit / 100, 2)

    '                    'l_Double_YmcapreTaxRemainingReservesPBalance = l_Double_YmcapreTaxRemainingReservesPBalance + dtRowPartAccountbalance("YmcapreTaxRemainingReserves")
    '                    'l_Double_YmcapreTaxRemainingReservesPBalance = l_Double_YmcapreTaxRemainingReservesPBalance - Math.Round(dtRowParticipant("YmcapreTaxRemainingReserves") * PercentageForSplit / 100, 2)
    '                End If
    '            Next

    '            ''SP 2014.07.02  BT-2445\YRS 5.0-633 -Start
    '            ''Dim receipentCurrentPayment As Decimal

    '            ''dtRecptAccount = Me.Session_Datatable_DtRecptAccount
    '            ''For Each drReceipent As DataRow In dtRecptAccount.Rows
    '            ''    receipentCurrentPayment = receipentCurrentPayment + (drReceipent("CurrentPayment") + drReceipent("SSLevelingAmt") - drReceipent("SSReductionAmt"))
    '            ''Next

    '            ''If Math.Round((l_Double_CurrentPaymentPBalance + receipentCurrentPayment), 2) > l_Double_CurrentPaymentP And RadioButtonListSplitAmtType.Items(0).Selected = True Then
    '            ''    Return False
    '            ''End If
    '            ''SP 2014.07.02  BT-2445\YRS 5.0-633-End



    '            If RadioButtonListSplitAmtType.Items(0).Selected = True Then
    '                If Convert.ToDecimal(TextBoxAmountWorkSheet.Text.Trim < 0.01) Then
    '                    Return False
    '                End If
    '            Else
    '                If PercentageForSplit > 100 Or PercentageForSplit < 1 Then
    '                    Return False
    '                End If
    '            End If

    '            'Commented by Paramesh K. on Oct 30 2008
    '            'As it is not allowing split if the participant has a negative balance

    '            '***************************
    '            'SP 2014.06.26 - BT-2445\YRS 5.0-633 -Start Uncommented for negative balance split
    '            If l_Decimal_CurrentPaymentPBalance > l_Decimal_CurrentPaymentP And RadioButtonListSplitAmtType.Items(1).Selected = True Then
    '                Return False
    '            End If
    '            If l_Decimal_CurrentPaymentP < Convert.ToDecimal(TextBoxAmountWorkSheet.Text) And RadioButtonListSplitAmtType.Items(0).Selected = True Then
    '                Return False
    '            End If
    '            'SP 2014.06.26 - BT-2445\YRS 5.0-633 -End

    '            'If l_Double_SSLevelingAmtPBalance < 0 Then
    '            '    Return False
    '            'End If
    '            'If l_Double_SSReductionAmtPBalance < 0 Then
    '            '    Return False
    '            'End If
    '            'If l_Double_CurrentPaymentPBalance < 0 Then
    '            '    Return False
    '            'End If
    '            'If l_Double_EmpPreTaxCurrentPaymentPBalance < 0 Then
    '            '    Return False
    '            'End If
    '            'If l_Double_EmpPostTaxCurrentPaymentPBalance < 0 Then
    '            '    Return False
    '            'End If
    '            'If l_Double_YmcaPreTaxCurrentPaymentPBalance < 0 Then
    '            '    Return False
    '            'End If

    '            'If l_Double_EmpPreTaxRemainingReservesPBalance < 0 Then
    '            '    Return False
    '            'End If
    '            'If l_Double_EmpPostTaxRemainingReservesPBalance < 0 Then
    '            '    Return False
    '            'End If
    '            'If l_Double_YmcapreTaxRemainingReservesPBalance < 0 Then
    '            '    Return False
    '            'End If

    '            'Added by Paramesh K.
    '            'For checking overall(all the benifieries) split percentage should not be greater than 100%

    '            If Not Me.Session_Datatable_DtRecptAccount Is Nothing Then
    '                Dim dtRecpAccount As DataTable = Me.Session_Datatable_DtRecptAccount
    '                Dim dblTotalPercentage As Decimal
    '                Dim recipentid As String = String.Empty

    '                'Current percentage
    '                dblTotalPercentage = PercentageForSplit

    '                'Previous percentage
    '                For intRow As Integer = 0 To dtRecpAccount.Rows.Count - 1
    '                    If recipentid <> dtRecpAccount.Rows(intRow)("RecipientPersonID") Then
    '                        dblTotalPercentage = dblTotalPercentage + dtRecpAccount.Rows(intRow)("RecipientSplitPercent")
    '                        recipentid = dtRecpAccount.Rows(intRow)("RecipientPersonID")
    '                    End If
    '                Next

    '                'Neeraj 02/12/2010 :  Gemini 699
    '                'If Format(dblTotalPercentage, "0.00") > 99.99 Then
    '                If Format(dblTotalPercentage, "0.00") > 100.0 Then
    '                    Return False
    '                End If
    '            End If

    '            '***************************
    '            Return True
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

#End Region

#Region "Adjustment Validation"
    'SP 2014.07.14 BT-2445\YRS 5.0-633 -Start
    'This method is used reverse recipient Annuities while happing Adjustment Recipient annuities Added Participant Annuties 
    '#1. First Second  will work Multiple beneficary Selecting recipient Annuities For If the Person not in selected 
    '#2.Second Selection will work for Single beneficary Selecting recipient Annuities For If the selected Person
    Private Sub ReverseCurrentBeneficiaryComponentintoRemainigParticipantBalance(ByVal dtRecptCurrentSplit As DataTable, ByRef dtPartRemainingBalance As DataTable, ByVal dtRecptExisitngSplit As DataTable)
        Dim drArrCurrentReceipient As DataRow()
        Try


            For Each drRect As DataRow In dtRecptCurrentSplit.Rows
                For Each drPart As DataRow In dtPartRemainingBalance.Rows

                    If drPart("guiAnnuityID") <> drRect("guiAnnuityID") Then Continue For

                    drPart("SSLevelingAmt") += drRect("SSLevelingAmt")
                    drPart("SSReductionAmt") += drRect("SSReductionAmt")
                    drPart("CurrentPayment") += drRect("CurrentPayment")
                    drPart("EmpPreTaxCurrentPayment") += drRect("EmpPreTaxCurrentPayment")
                    drPart("EmpPostTaxCurrentPayment") += drRect("EmpPostTaxCurrentPayment")
                    drPart("YmcaPreTaxCurrentPayment") += drRect("YmcaPreTaxCurrentPayment")
                    drPart("EmpPreTaxRemainingReserves") += drRect("EmpPreTaxRemainingReserves")
                    drPart("EmpPostTaxRemainingReserves") += drRect("EmpPostTaxRemainingReserves")
                    drPart("YmcapreTaxRemainingReserves") += drRect("YmcapreTaxRemainingReserves")
                Next
            Next
            'drArrCurrentReceipeint = dtRecptExisitngSplit.Select("RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString() & "'")
            'For dtRowCount = 0 To drArrCurrentReceipeint.Length - 1
            '    dtRecptExisitngSplit.Rows.Remove(drArrCurrentReceipeint(dtRowCount))
            'Next

            'dtRecptExisitngSplit.AcceptChanges()
        Catch
            Throw
        End Try
    End Sub
    Private Function ReadAdjustmentDataFromGrid(ByVal dtRcptSplitTemp As DataTable, ByVal dgBeneficiary As DataGrid)

        Dim dtReturnValue As DataTable
        Dim drArrRcptSplitTemp As DataRow()

        Try

            dtReturnValue = dtRcptSplitTemp.Clone()

            For Each dgItem As DataGridItem In DataGridRecipientAnnuitiesBalance.Items

                Dim strAnnuityID As String = dgItem.Cells(15).Text.Trim()
                Dim strRecipientID As String = dgItem.Cells(0).Text.Trim()
                Dim TextCurrentPayment As TextBox = CType(dgItem.Cells(8).FindControl("TextboxCurrentPayment"), TextBox)
                Dim strExpression As String = "RecipientPersonID='" + strRecipientID + "' AND guiAnnuityID='" + strAnnuityID + "'"

                drArrRcptSplitTemp = dtRcptSplitTemp.Select(strExpression)

                If (drArrRcptSplitTemp.Length > 0) Then

                    Dim dr As DataRow

                    dr = dtReturnValue.NewRow()

                    dr.ItemArray = drArrRcptSplitTemp(0).ItemArray

                    dr("CurrentPayment") = Convert.ToDecimal(TextCurrentPayment.Text.Trim())

                    dtReturnValue.Rows.Add(dr)

                End If
            Next
        Catch
            Throw
        End Try
        Return dtReturnValue
    End Function
    Private Sub CopyRemainingBeneficiarySplit(ByVal dtBeneCurrentSplit As DataTable, ByVal guiRecipientID As String, ByRef dtRcptSplitTemp As DataTable)

        Dim drArrRemainingBeneficiary As DataRow()
        Dim drBeneficiary As DataRow

        Try
            Dim strExpression As String = "RecipientPersonID <>'" + guiRecipientID + "'"

            For Each drRow As DataRow In dtBeneCurrentSplit.Rows

                If (drRow("RecipientPersonID") = guiRecipientID) Then Continue For

                drBeneficiary = dtRcptSplitTemp.NewRow()
                drBeneficiary.ItemArray = drRow.ItemArray

                dtRcptSplitTemp.Rows.Add(drBeneficiary)
            Next


            dtRcptSplitTemp.AcceptChanges()

        Catch
            Throw
        End Try
    End Sub

    'Private Sub AdjustParticipantRemainingReserve(ByVal dtParticipantOriginal As DataTable, ByVal dtRcptSplitTemp As DataTable, ByRef dtPartRemainingAccountbalance As DataTable)

    '    Dim totalAvailableAnnutyValue As Decimal

    '    For Each dr As DataRow In dtPartRemainingAccountbalance.Rows

    '        ' totalAvailableAnnutyValue += GetEffectiveCurrentPaymentForAnnuity(dr)

    '    Next


    'End Sub


    Private Sub ComputeParticipantRemaining(ByVal dtParticipantOriginal As DataTable, ByVal dtRcptSplitTemp As DataTable, ByRef dtPartRemainingAccountbalance As DataTable)

        Dim dBeneCurrentPayment As Decimal
        Dim dBeneSSLevelingAmt As Decimal
        Dim dBeneAnnuitySSReductionAmt As Decimal
        Dim dBeneAnnuityPreTaxCurrentPayment As Decimal
        Dim dBeneAnnuityPostTaxCurrentPayment As Decimal
        Dim dBeneAnnuityYmcaPreTaxCurrentPayment As Decimal
        Dim dBeneAnnuityPreTaxRemainingReserves As Decimal
        Dim dBeneAnnuityPostTaxRemainingReserves As Decimal
        Dim dBeneAnnuityYmcapreTaxRemainingReserves As Decimal
        Dim strFilterExpression As String

        Try
            For Each drPart As DataRow In dtParticipantOriginal.Rows
                For Each drResserveRem As DataRow In dtPartRemainingAccountbalance.Rows

                    If (drResserveRem("guiAnnuityID") = drPart("guiAnnuityID")) Then

                        dBeneCurrentPayment = 0
                        dBeneSSLevelingAmt = 0
                        dBeneAnnuitySSReductionAmt = 0
                        dBeneAnnuityPreTaxCurrentPayment = 0
                        dBeneAnnuityPostTaxCurrentPayment = 0
                        dBeneAnnuityYmcaPreTaxCurrentPayment = 0
                        dBeneAnnuityPreTaxRemainingReserves = 0
                        dBeneAnnuityPostTaxRemainingReserves = 0
                        dBeneAnnuityYmcapreTaxRemainingReserves = 0

                        'Sum of each annuites for all beneficiaries
                        strFilterExpression = "guiAnnuityID ='" + drPart("guiAnnuityID") + "'"
                        If (dtRcptSplitTemp.Select(strFilterExpression).Length > 0) Then


                            dBeneCurrentPayment = Convert.ToDecimal(dtRcptSplitTemp.Compute("Sum(CurrentPayment)", strFilterExpression))
                            dBeneSSLevelingAmt = Convert.ToDecimal(dtRcptSplitTemp.Compute("Sum(SSLevelingAmt)", strFilterExpression))
                            dBeneAnnuitySSReductionAmt = Convert.ToDecimal(dtRcptSplitTemp.Compute("Sum(SSReductionAmt)", strFilterExpression))
                            dBeneAnnuityPreTaxCurrentPayment = Convert.ToDecimal(dtRcptSplitTemp.Compute("Sum(EmpPreTaxCurrentPayment)", strFilterExpression))
                            dBeneAnnuityPostTaxCurrentPayment = Convert.ToDecimal(dtRcptSplitTemp.Compute("Sum(EmpPostTaxCurrentPayment)", strFilterExpression))
                            dBeneAnnuityYmcaPreTaxCurrentPayment = Convert.ToDecimal(dtRcptSplitTemp.Compute("Sum(YmcaPreTaxCurrentPayment)", strFilterExpression))
                            dBeneAnnuityPreTaxRemainingReserves = Convert.ToDecimal(dtRcptSplitTemp.Compute("Sum(EmpPreTaxRemainingReserves)", strFilterExpression))
                            dBeneAnnuityPostTaxRemainingReserves = Convert.ToDecimal(dtRcptSplitTemp.Compute("Sum(EmpPostTaxRemainingReserves)", strFilterExpression))
                            dBeneAnnuityYmcapreTaxRemainingReserves = Convert.ToDecimal(dtRcptSplitTemp.Compute("Sum(YmcapreTaxRemainingReserves)", strFilterExpression))



                            'Compute Participant remaining
                            drResserveRem("CurrentPayment") = drPart("CurrentPayment") - dBeneCurrentPayment
                            drResserveRem("SSLevelingAmt") = drPart("SSLevelingAmt") - dBeneSSLevelingAmt
                            drResserveRem("SSReductionAmt") = drPart("SSReductionAmt") - dBeneAnnuitySSReductionAmt
                            drResserveRem("EmpPreTaxCurrentPayment") = drPart("EmpPreTaxCurrentPayment") - dBeneAnnuityPreTaxCurrentPayment
                            drResserveRem("EmpPostTaxCurrentPayment") = drPart("EmpPostTaxCurrentPayment") - dBeneAnnuityPostTaxCurrentPayment
                            drResserveRem("YmcaPreTaxCurrentPayment") = drPart("YmcaPreTaxCurrentPayment") - dBeneAnnuityYmcaPreTaxCurrentPayment
                            drResserveRem("EmpPreTaxRemainingReserves") = drPart("EmpPreTaxRemainingReserves") - dBeneAnnuityPreTaxRemainingReserves
                            drResserveRem("EmpPostTaxRemainingReserves") = drPart("EmpPostTaxRemainingReserves") - dBeneAnnuityPostTaxRemainingReserves
                            drResserveRem("YmcapreTaxRemainingReserves") = drPart("YmcapreTaxRemainingReserves") - dBeneAnnuityYmcapreTaxRemainingReserves
                        End If
                    End If
                Next
            Next

        Catch
            Throw
        End Try
    End Sub


    Private Sub ProRateAdjustment(ByVal dtPartOriginal As DataTable, ByVal dtRecpExistingSplit As DataTable, ByRef dtPartRemainingbalance As DataTable, ByRef dtRcptSplitTemp As DataTable)

        Dim TotalIndividualSplit As Decimal
        'Dim CurrentProratePercentage As Decimal
        Dim drArrParticipant As DataRow()
        Dim drParticipant As DataRow
        Dim bitIncrease, bitJointSurvivior, bitSSLeveling As Boolean
        Dim strCurrentRecipientID As String
        Dim dCurrentPaymentAfterReduction As Decimal
        Dim splitPercentageForSSLAnnuity As Decimal
        Try
            If HelperFunctions.isNonEmpty(dtRcptSplitTemp) Then

                strCurrentRecipientID = dtRcptSplitTemp.Rows(0)("RecipientPersonID")

                For Each dr In dtRcptSplitTemp.Rows
                    'Get Total Annuity amount
                    drArrParticipant = dtPartOriginal.Select("guiAnnuityID='" + dr("guiAnnuityID").ToString() + "'")
                    Dim drArrPartRemaining As DataRow() = dtPartRemainingbalance.Select("guiAnnuityID='" + dr("guiAnnuityID").ToString() + "'")

                    If (drArrParticipant.Length > 0) Then
                        drParticipant = drArrParticipant(0)
                        Dim drPartRemaining As DataRow = drArrPartRemaining(0)

                        'Check if SS leveling annuity then chnage the current balance -Start.
                        GetRetireeAnnuityType(drParticipant("AnnuityType"), bitIncrease, bitJointSurvivior, bitSSLeveling)
                        If dr("ShareBenefit") = False AndAlso bitSSLeveling Then
                            'Compute Prorate percentage factor &  Apply factor
                            CalculateSSLevelingWithoutShareBenefit(dr, drParticipant)

                        Else
                            'Compute Prorate percentage factor &  Apply factor
                            CalculateSSLevelingWithShareBenefit(dr, drParticipant)

                        End If

                        drPartRemaining("CurrentPayment") -= dr("CurrentPayment")

                        drPartRemaining("EmpPreTaxCurrentPayment") -= dr("EmpPreTaxCurrentPayment")
                        drPartRemaining("EmpPostTaxCurrentPayment") -= dr("EmpPostTaxCurrentPayment")
                        drPartRemaining("YmcaPreTaxCurrentPayment") -= dr("YmcaPreTaxCurrentPayment")

                        'Adjust any negative components into the beneficiary's annuity
                        Dim adjustNegative As Decimal = 0
                        If drPartRemaining("EmpPreTaxCurrentPayment") < 0 Then
                            dr("EmpPreTaxCurrentPayment") += drPartRemaining("EmpPreTaxCurrentPayment")
                            drPartRemaining("EmpPreTaxCurrentPayment") = 0
                        End If
                        If drPartRemaining("EmpPostTaxCurrentPayment") < 0 Then
                            dr("EmpPostTaxCurrentPayment") += drPartRemaining("EmpPostTaxCurrentPayment")
                            drPartRemaining("EmpPostTaxCurrentPayment") = 0
                        End If
                        If drPartRemaining("YmcaPreTaxCurrentPayment") < 0 Then
                            dr("YmcaPreTaxCurrentPayment") += drPartRemaining("YmcaPreTaxCurrentPayment")
                            drPartRemaining("YmcaPreTaxCurrentPayment") = 0
                        End If

                        Dim arr() As String = {"EmpPreTaxCurrentPayment", "EmpPostTaxCurrentPayment", "YmcaPreTaxCurrentPayment"}
                        For index = 0 To 2
                            For index1 = index To 2
                                If drPartRemaining(arr(index1)) > drPartRemaining(arr(index)) Then
                                    Dim temp As String = arr(index)
                                    arr(index) = arr(index1)
                                    arr(index1) = temp
                                End If
                            Next
                        Next

                        'Adjust Prorated components from each beneficiary annuity record from largest value - dr
                        TotalIndividualSplit = dr("EmpPreTaxCurrentPayment") + dr("EmpPostTaxCurrentPayment") + dr("YmcaPreTaxCurrentPayment")
                        Dim amountToAdjust As Double = dr("CurrentPayment") - TotalIndividualSplit
                        For index = 0 To 2
                            If (amountToAdjust <> 0) Then
                                AdjustValueFrom(dr, drPartRemaining, arr(index), amountToAdjust)
                            End If
                        Next

                        drPartRemaining("SSLevelingAmt") -= dr("SSLevelingAmt")
                        drPartRemaining("SSReductionAmt") -= dr("SSReductionAmt")

                        AdjustSSLevelingAmountAfterAdjustingRemainingBalance(drPartRemaining, dr)

                        drPartRemaining("EmpPreTaxRemainingReserves") -= dr("EmpPreTaxRemainingReserves")
                        drPartRemaining("EmpPostTaxRemainingReserves") -= dr("EmpPostTaxRemainingReserves")
                        drPartRemaining("YmcapreTaxRemainingReserves") -= dr("YmcapreTaxRemainingReserves")

                    End If

                Next

                'Copy remaining records from CurrentSplit into Temp
                CopyRemainingBeneficiarySplit(dtRecpExistingSplit, strCurrentRecipientID, dtRcptSplitTemp)

            End If
        Catch
            Throw
        End Try
    End Sub



    ''SP 2014.07.14 BT-2445\YRS 5.0-633 -End


    '*******************************************************************************************************//
    'Function Name             :ValidateAdjustMent             Created on  : 14/04/08                       //
    'Created By                :Dilip Patada                   Modified On :                                //
    'Modified By               :                                                                            //
    'Modify Reason             :                                                                            //
    'Param Description         :DataTableParticipant,DataTablePartAccountbalance.DataTableRecptAccounttemp  //
    '                           dgBenificiary                                                               //
    'Method Description        :This Function will Validate all condition for spliting the annuity like     //
    '                           is Amount enter will be within the range of Recipant balance Amount etc.    //
    '*******************************************************************************************************//
    'Private Function ValidateAdjustMent(ByVal DataTableParticipant As DataTable, ByVal DataTablePartAccountbalance As DataTable, ByVal dgBenificiary As DataGrid, ByVal DataTableRecptAccounttemp As DataTable)
    '    Dim l_int_iCount As Integer
    '    Dim l_Double_PartAmountBalance As Double
    '    Dim l_Double_PartAmountBalanceAfterSplit As Double
    '    Dim dtRowParticipant As DataRow
    '    Dim dtRowRecptAccounttemp As DataRow
    '    Dim dtRowPartAccountbalance As DataRow
    '    Dim dgItems As DataGridItem
    '    Dim dgItemsRecptAccount As DataGridItem

    '    Dim l_Double_SSLevelingAmtR As Double = 0.0
    '    Dim l_Double_SSReductionAmtR As Double = 0.0
    '    Dim l_Double_CurrentPaymentR As Double = 0.0
    '    Dim l_Double_EmpPreTaxCurrentPaymentR As Double = 0.0
    '    Dim l_Double_EmpPostTaxCurrentPaymentR As Double = 0.0
    '    Dim l_Double_YmcaPreTaxCurrentPaymentR As Double = 0.0
    '    Dim l_Double_EmpPreTaxRemainingReservesR As Double = 0.0
    '    Dim l_Double_EmpPostTaxRemainingReservesR As Double = 0.0
    '    Dim l_Double_YmcapreTaxRemainingReservesR As Double = 0.0

    '    Dim l_Double_SSLevelingAmtP As Double = 0.0
    '    Dim l_Double_SSReductionAmtP As Double = 0.0
    '    Dim l_Double_CurrentPaymentP As Double = 0.0
    '    Dim l_Double_EmpPreTaxCurrentPaymentP As Double = 0.0
    '    Dim l_Double_EmpPostTaxCurrentPaymentP As Double = 0.0
    '    Dim l_Double_YmcaPreTaxCurrentPaymentP As Double = 0.0
    '    Dim l_Double_EmpPreTaxRemainingReservesP As Double = 0.0
    '    Dim l_Double_EmpPostTaxRemainingReservesP As Double = 0.0
    '    Dim l_Double_YmcapreTaxRemainingReservesP As Double = 0.0

    '    Dim l_Double_SSLevelingAmtPBalance As Double = 0.0
    '    Dim l_Double_SSReductionAmtPBalance As Double = 0.0
    '    Dim l_Double_CurrentPaymentPBalance As Double = 0.0
    '    Dim l_Double_EmpPreTaxCurrentPaymentPBalance As Double = 0.0
    '    Dim l_Double_EmpPostTaxCurrentPaymentPBalance As Double = 0.0
    '    Dim l_Double_YmcaPreTaxCurrentPaymentPBalance As Double = 0.0
    '    Dim l_Double_EmpPreTaxRemainingReservesPBalance As Double = 0.0
    '    Dim l_Double_EmpPostTaxRemainingReservesPBalance As Double = 0.0
    '    Dim l_Double_YmcapreTaxRemainingReservesPBalance As Double = 0.0

    '    Try

    '        For Each dtRowRecptAccounttemp In DataTableRecptAccounttemp.Rows
    '            For Each dtRowParticipant In DataTableParticipant.Rows
    '                If dtRowRecptAccounttemp("guiAnnuityID") = dtRowParticipant("guiAnnuityID") Then
    '                    l_Double_SSLevelingAmtP = l_Double_SSLevelingAmtP + dtRowParticipant("SSLevelingAmt")
    '                    l_Double_SSReductionAmtP = l_Double_SSReductionAmtP + dtRowParticipant("SSReductionAmt")
    '                    l_Double_CurrentPaymentP = l_Double_CurrentPaymentP + dtRowParticipant("CurrentPayment")
    '                    l_Double_EmpPreTaxCurrentPaymentP = l_Double_EmpPreTaxCurrentPaymentP + dtRowParticipant("EmpPreTaxCurrentPayment")
    '                    l_Double_EmpPostTaxCurrentPaymentP = l_Double_EmpPostTaxCurrentPaymentP + dtRowParticipant("EmpPostTaxCurrentPayment")
    '                    l_Double_YmcaPreTaxCurrentPaymentP = l_Double_YmcaPreTaxCurrentPaymentP + dtRowParticipant("YmcaPreTaxCurrentPayment")
    '                    l_Double_EmpPreTaxRemainingReservesP = l_Double_EmpPreTaxRemainingReservesP + dtRowParticipant("EmpPreTaxRemainingReserves")
    '                    l_Double_EmpPostTaxRemainingReservesP = l_Double_EmpPostTaxRemainingReservesP + dtRowParticipant("EmpPostTaxRemainingReserves")
    '                    l_Double_YmcapreTaxRemainingReservesP = l_Double_YmcapreTaxRemainingReservesP + dtRowParticipant("YmcapreTaxRemainingReserves")
    '                End If
    '            Next
    '        Next

    '        For Each dtRowRecptAccounttemp In DataTableRecptAccounttemp.Rows

    '            For Each dgItemsRecptAccount In DataGridWorkSheet2.Items

    '                For Each dtRowPartAccountbalance In DataTablePartAccountbalance.Rows
    '                    If dtRowRecptAccounttemp("guiAnnuityID") = dtRowPartAccountbalance("guiAnnuityID") And dtRowRecptAccounttemp("guiAnnuityID") = dgItemsRecptAccount.Cells(15).Text Then
    '                        Dim TextSSLevelingAmt As TextBox = CType(dgItemsRecptAccount.Cells(5).FindControl("TextBoxSSLevlingAmount"), TextBox)
    '                        Dim TextSSReductionAmt As TextBox = CType(dgItemsRecptAccount.Cells(6).FindControl("TextboxSSReductionAmount"), TextBox)
    '                        Dim TextCurrentPayment As TextBox = CType(dgItemsRecptAccount.Cells(8).FindControl("TextboxCurrentPayment"), TextBox)
    '                        Dim TextEmpPreTaxCurrentPayment As TextBox = CType(dgItemsRecptAccount.Cells(9).FindControl("TextboxEmpPreTaxCurrentPayment"), TextBox)
    '                        Dim TextEmpPostTaxCurrentPayment As TextBox = CType(dgItemsRecptAccount.Cells(10).FindControl("TextboxEmpPostTaxCurrentPayment"), TextBox)
    '                        Dim TextYmcaPreTaxCurrentPayment As TextBox = CType(dgItemsRecptAccount.Cells(11).FindControl("TextboxYmcaPreTaxCurrentPayment"), TextBox)
    '                        Dim TextEmpPreTaxRemainingReserves As TextBox = CType(dgItemsRecptAccount.Cells(12).FindControl("TextboxEmpPreTaxRemainingReserves"), TextBox)
    '                        Dim TextEmpPostTaxRemainingReserves As TextBox = CType(dgItemsRecptAccount.Cells(13).FindControl("TextboxEmpPostTaxRemainingReserves"), TextBox)
    '                        Dim TextYmcapreTaxRemainingReserves As TextBox = CType(dgItemsRecptAccount.Cells(14).FindControl("TextboxYmcaPreTaxRemainingReserves"), TextBox)

    '                        l_Double_SSLevelingAmtR = l_Double_SSLevelingAmtR + TextSSLevelingAmt.Text.Trim
    '                        l_Double_SSReductionAmtR = l_Double_SSReductionAmtR + TextSSReductionAmt.Text.Trim
    '                        l_Double_CurrentPaymentR = l_Double_CurrentPaymentR + TextCurrentPayment.Text.Trim
    '                        l_Double_EmpPreTaxCurrentPaymentR = l_Double_EmpPreTaxCurrentPaymentR + TextEmpPreTaxCurrentPayment.Text.Trim
    '                        l_Double_EmpPostTaxCurrentPaymentR = l_Double_EmpPostTaxCurrentPaymentR + TextEmpPostTaxCurrentPayment.Text.Trim
    '                        l_Double_YmcaPreTaxCurrentPaymentR = l_Double_YmcaPreTaxCurrentPaymentR + TextYmcaPreTaxCurrentPayment.Text.Trim
    '                        l_Double_EmpPreTaxRemainingReservesR = l_Double_EmpPreTaxRemainingReservesR + TextEmpPreTaxRemainingReserves.Text.Trim
    '                        l_Double_EmpPostTaxRemainingReservesR = l_Double_EmpPostTaxRemainingReservesR + TextEmpPostTaxRemainingReserves.Text.Trim
    '                        l_Double_YmcapreTaxRemainingReservesR = l_Double_YmcapreTaxRemainingReservesR + TextYmcapreTaxRemainingReserves.Text.Trim


    '                        l_Double_SSLevelingAmtPBalance = dtRowRecptAccounttemp("SSLevelingAmt") - TextSSLevelingAmt.Text.Trim + dtRowPartAccountbalance("SSLevelingAmt")
    '                        l_Double_SSReductionAmtPBalance = dtRowRecptAccounttemp("SSReductionAmt") - TextSSReductionAmt.Text.Trim + dtRowPartAccountbalance("SSReductionAmt")
    '                        l_Double_CurrentPaymentPBalance = dtRowRecptAccounttemp("CurrentPayment") - TextCurrentPayment.Text.Trim + dtRowPartAccountbalance("CurrentPayment")
    '                        l_Double_EmpPreTaxCurrentPaymentPBalance = dtRowRecptAccounttemp("EmpPreTaxCurrentPayment") - TextEmpPreTaxCurrentPayment.Text.Trim + dtRowPartAccountbalance("EmpPreTaxCurrentPayment")
    '                        l_Double_EmpPostTaxCurrentPaymentPBalance = dtRowRecptAccounttemp("EmpPostTaxCurrentPayment") - TextEmpPostTaxCurrentPayment.Text.Trim + dtRowPartAccountbalance("EmpPostTaxCurrentPayment")
    '                        l_Double_YmcaPreTaxCurrentPaymentPBalance = dtRowRecptAccounttemp("YmcaPreTaxCurrentPayment") - TextYmcaPreTaxCurrentPayment.Text.Trim + dtRowPartAccountbalance("YmcaPreTaxCurrentPayment")
    '                        l_Double_EmpPreTaxRemainingReservesPBalance = dtRowRecptAccounttemp("EmpPreTaxRemainingReserves") - TextEmpPreTaxRemainingReserves.Text.Trim + dtRowPartAccountbalance("EmpPreTaxRemainingReserves")
    '                        l_Double_EmpPostTaxRemainingReservesPBalance = dtRowRecptAccounttemp("EmpPostTaxRemainingReserves") - TextEmpPostTaxRemainingReserves.Text.Trim + dtRowPartAccountbalance("EmpPostTaxRemainingReserves")
    '                        l_Double_YmcapreTaxRemainingReservesPBalance = dtRowRecptAccounttemp("YmcapreTaxRemainingReserves") - TextYmcapreTaxRemainingReserves.Text.Trim + dtRowPartAccountbalance("YmcapreTaxRemainingReserves")
    '                        'Added by Dilip 25-09-2008

    '                        If Math.Round(l_Double_CurrentPaymentR, 2) <> Math.Round(l_Double_EmpPreTaxCurrentPaymentR + l_Double_EmpPostTaxCurrentPaymentR + l_Double_YmcaPreTaxCurrentPaymentR, 2) Then
    '                            Return False
    '                        End If

    '                        'Added by Dilip 25-09-2008
    '                        If l_Double_SSLevelingAmtPBalance < 0 Then
    '                            Return False
    '                        End If
    '                        If l_Double_SSReductionAmtPBalance < 0 Then
    '                            Return False
    '                        End If
    '                        If l_Double_CurrentPaymentPBalance < 0 Then
    '                            Return False
    '                        End If
    '                        If l_Double_EmpPreTaxCurrentPaymentPBalance < 0 Then
    '                            Return False
    '                        End If
    '                        If l_Double_EmpPostTaxCurrentPaymentPBalance < 0 Then
    '                            Return False
    '                        End If
    '                        If l_Double_YmcaPreTaxCurrentPaymentPBalance < 0 Then
    '                            Return False
    '                        End If
    '                        If l_Double_EmpPreTaxRemainingReservesPBalance < 0 Then
    '                            Return False
    '                        End If
    '                        If l_Double_EmpPostTaxRemainingReservesPBalance < 0 Then
    '                            Return False
    '                        End If
    '                        If l_Double_YmcapreTaxRemainingReservesPBalance < 0 Then
    '                            Return False
    '                        End If

    '                    End If
    '                Next
    '            Next
    '        Next
    '        If Math.Round(l_Double_SSLevelingAmtR, 2) <> Convert.ToDouble(Math.Round(l_Double_SSLevelingAmtP * dtRowRecptAccounttemp("RecipientSplitPercent") / 100, 8)).ToString("N2") Then
    '            Return False
    '        End If
    '        If Math.Round(l_Double_SSReductionAmtR, 2) <> Convert.ToDouble(Math.Round(l_Double_SSReductionAmtP * dtRowRecptAccounttemp("RecipientSplitPercent") / 100, 8)).ToString("N2") Then
    '            Return False
    '        End If
    '        If Math.Round(l_Double_CurrentPaymentR, 2) <> Convert.ToDouble(Math.Round(l_Double_CurrentPaymentP * dtRowRecptAccounttemp("RecipientSplitPercent") / 100, 8)).ToString("N2") Then
    '            Return False
    '        End If
    '        If Math.Round(l_Double_EmpPreTaxCurrentPaymentR, 2) <> Convert.ToDouble(Math.Round(l_Double_EmpPreTaxCurrentPaymentP * dtRowRecptAccounttemp("RecipientSplitPercent") / 100, 8)).ToString("N2") Then
    '            Return False
    '        End If
    '        If Math.Round(l_Double_EmpPostTaxCurrentPaymentR, 2) <> Convert.ToDouble(Math.Round(l_Double_EmpPostTaxCurrentPaymentP * dtRowRecptAccounttemp("RecipientSplitPercent") / 100, 8)).ToString("N2") Then
    '            Return False
    '        End If
    '        If Math.Round(l_Double_YmcaPreTaxCurrentPaymentR, 2) <> Convert.ToDouble(Math.Round(l_Double_YmcaPreTaxCurrentPaymentP * dtRowRecptAccounttemp("RecipientSplitPercent") / 100, 8)).ToString("N2") Then
    '            Return False
    '        End If
    '        If Math.Round(l_Double_EmpPreTaxRemainingReservesR, 2) <> Convert.ToDouble(Math.Round(l_Double_EmpPreTaxRemainingReservesP * dtRowRecptAccounttemp("RecipientSplitPercent") / 100, 8)).ToString("N2") Then
    '            Return False
    '        End If
    '        If Math.Round(l_Double_EmpPostTaxRemainingReservesR, 2) <> Convert.ToDouble(Math.Round(l_Double_EmpPostTaxRemainingReservesP * dtRowRecptAccounttemp("RecipientSplitPercent") / 100, 8)).ToString("N2") Then
    '            Return False
    '        End If
    '        If Math.Round(l_Double_YmcapreTaxRemainingReservesR, 2) <> Convert.ToDouble(Math.Round(l_Double_YmcapreTaxRemainingReservesP * dtRowRecptAccounttemp("RecipientSplitPercent") / 100, 8)).ToString("N2") Then
    '            Return False
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function
    Private Sub LogTraceInformation(ByVal dtPartOriginal As DataTable, ByVal dtPartRemainingbalance As DataTable, ByVal dtRecpExistingSplit As DataTable, ByVal dtRecptCurrentSplit As DataTable, ByVal guiCurrRcptId As String)

        Dim strMessage As String
        Try

            strMessage = "Original Particpant Annuity Details -  " + ConvertDataTableToString(dtPartOriginal) + System.Environment.NewLine
            strMessage += "Particpant Remaining Annuity Details -  " + ConvertDataTableToString(dtPartRemainingbalance) + System.Environment.NewLine
            strMessage += "Receipeints Original Split Details Before Adjustment -  " + ConvertDataTableToString(dtRecpExistingSplit) + System.Environment.NewLine
            strMessage += "Receipeint Current Annuity Split after Adjustment Details -  " + ConvertDataTableToString(dtRecptCurrentSplit) + System.Environment.NewLine
            strMessage += "Receipeint Current receipeient ID -  " + guiCurrRcptId

            Logger.Write("Non Retired QDRO --> Split/Adjust Validation Failure Details : " + strMessage, "Application", 1, 1, System.Diagnostics.TraceEventType.Information)

        Catch
            Throw
        End Try
    End Sub
    Private Sub LogTraceInformation(ByVal strMessage)
        Logger.Write(strMessage + strMessage, "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
    End Sub
    Private Function ValidateAdjustment(ByVal dtPartOriginal As DataTable, ByVal dtPartRemainingbalance As DataTable, ByVal dtRecpExistingSplit As DataTable, ByVal dtRecptCurrentSplit As DataTable, ByVal guiCurrRcptId As String)
        ''check for each annuity
        Dim dBeneCurrentPayment As Decimal = 0

        Dim dBeneOriginalCurrentPayment As Decimal = 0

        Dim dBeneAnnuityCurrentPayment As Decimal = 0
        Dim dBeneAnnuitySSLevelingAmt As Decimal = 0
        Dim dBeneAnnuitySSReductionAmt As Decimal = 0
        Dim dBeneAnnuityPreTaxCurrentPayment As Decimal = 0
        Dim dBeneAnnuityPostTaxCurrentPayment As Decimal = 0
        Dim dBeneAnnuityYmcaPreTaxCurrentPayment As Decimal = 0
        Dim dBeneAnnuityPreTaxRemainingReserves As Decimal = 0
        Dim dBeneAnnuityPostTaxRemainingReserves As Decimal = 0
        Dim dBeneAnnuityYmcapreTaxRemainingReserves As Decimal = 0

        Try

            'Check if the Total annuity amount for the guiCurrRcptId after adjustment matches the amount computed at the time of split.
            'If the existing split details does not contain any data then ignore this check.
            If (HelperFunctions.isNonEmpty(dtRecpExistingSplit)) Then

                'fetch the original total split (column wise of all annutiy of current beneficiary)
                Dim drExistingRecordsForCurrentBeneficiary As DataRow() = dtRecpExistingSplit.Select("splitType ='" & Me.SplitPlanTypeOption.Trim & "' AND RecipientPersonID='" + guiCurrRcptId + "'")

                ''check if any records exists in original spli records in beneficiaries table the perform validation
                If (drExistingRecordsForCurrentBeneficiary.Length > 0) Then

                    'get the totals of all the column wise
                    For Each dr In drExistingRecordsForCurrentBeneficiary
                        dBeneOriginalCurrentPayment = dBeneOriginalCurrentPayment + dr("CurrentPayment")
                    Next

                    'select all the annuities for current beneficiary after adjustment and find the total of current payments
                    Dim drNewRecordsForCurrBeneficiary As DataRow() = dtRecptCurrentSplit.Select("splitType ='" & Me.SplitPlanTypeOption.Trim & "' AND RecipientPersonID='" + guiCurrRcptId + "'")
                    For Each drRow In drNewRecordsForCurrBeneficiary
                        dBeneCurrentPayment = dBeneCurrentPayment + drRow("CurrentPayment")
                    Next

                    '1 # Validation for the sum of beneficiary annuities is equaks to the original split annuities(before adjustment)
                    '. This validation for the sum of total annuities sholud be equals to the total sum of annuities of original split beneficiary annuity(before adjustment)
                    If dBeneCurrentPayment <> dBeneOriginalCurrentPayment Then
                        Return False
                    End If

                End If
            End If


            'For each participant annuity check the following conditions:
            '#1 - The components of the annuity remaining for the participant should not be less than 0
            '#2 - The total of the annuity values split to the recepients along with the annuity remaining for the participant should match with the value originally available
            '#3 - In case of annuities with SSLeveling, the current payment of the annuity should not go below 0 after reduction kicks in
            For Each drParticpantAnnuity In dtPartOriginal.Rows

                Dim stCurrPartAnnuityId As String = drParticpantAnnuity("guiAnnuityID").ToString
                dBeneAnnuityCurrentPayment = 0
                dBeneAnnuitySSLevelingAmt = 0
                dBeneAnnuitySSReductionAmt = 0
                dBeneAnnuityPreTaxCurrentPayment = 0
                dBeneAnnuityPostTaxCurrentPayment = 0
                dBeneAnnuityYmcaPreTaxCurrentPayment = 0
                dBeneAnnuityPreTaxRemainingReserves = 0
                dBeneAnnuityPostTaxRemainingReserves = 0
                dBeneAnnuityYmcapreTaxRemainingReserves = 0

                'Perform sum for each component of all annuities matching the current participant annuity
                For Each dr As DataRow In dtRecptCurrentSplit.Rows
                    If (dr("guiAnnuityID") <> stCurrPartAnnuityId) Then Continue For
                    dBeneAnnuityCurrentPayment = dBeneAnnuityCurrentPayment + Convert.ToDecimal(dr("CurrentPayment"))
                    dBeneAnnuitySSLevelingAmt = dBeneAnnuitySSLevelingAmt + Convert.ToDecimal(dr("SSLevelingAmt"))
                    dBeneAnnuitySSReductionAmt = dBeneAnnuitySSReductionAmt + Convert.ToDecimal(dr("SSReductionAmt"))
                    dBeneAnnuityPreTaxCurrentPayment = dBeneAnnuityPreTaxCurrentPayment + Convert.ToDecimal(dr("EmpPreTaxCurrentPayment"))
                    dBeneAnnuityPostTaxCurrentPayment = dBeneAnnuityPostTaxCurrentPayment + Convert.ToDecimal(dr("EmpPostTaxCurrentPayment"))
                    dBeneAnnuityYmcaPreTaxCurrentPayment = dBeneAnnuityYmcaPreTaxCurrentPayment + Convert.ToDecimal(dr("YmcaPreTaxCurrentPayment"))
                    dBeneAnnuityPreTaxRemainingReserves = dBeneAnnuityPreTaxRemainingReserves + Convert.ToDecimal(dr("EmpPreTaxRemainingReserves"))
                    dBeneAnnuityPostTaxRemainingReserves = dBeneAnnuityPostTaxRemainingReserves + Convert.ToDecimal(dr("EmpPostTaxRemainingReserves"))
                    dBeneAnnuityYmcapreTaxRemainingReserves = dBeneAnnuityYmcapreTaxRemainingReserves + Convert.ToDecimal(dr("YmcapreTaxRemainingReserves"))
                    If EnsureAnnuityComponentsAreNonNegative(dr) = False Then
                        'Log debug info - Beneficiary annuity contained a negative component.
                        LogTraceInformation("Retired QDRO - Split Validation Function EnsureAnnuityComponentsAreNonNegative, For Beneficiary annuity contained a negative component Details - " + ConvertDataTableToString(dr))
                        Return False
                    End If
                    If IsAnnuityNegativeAfterSSLevelling(dr) = True Then
                        'Log debug info - Participant remaining annuity would be negative after SS reduction
                        LogTraceInformation("Retired QDRO - Split Validation Function IsAnnuityNegativeAfterSSLevelling, For Beneficiary remaining contained negative after SS reduction Details - " + ConvertDataTableToString(dr))
                        Return False
                    End If
                Next

                'Find the remaining balance row
                Dim drArrRemainingPartBalance As DataRow() = dtPartRemainingbalance.Select("guiAnnuityID='" + stCurrPartAnnuityId + "'")

                'Sanity check. We expect at most one record to be found.
                If (drArrRemainingPartBalance.Length > 1) Then Throw New Exception("Multiple Annuity row found for particpant reamaining balance row for each annuity")
                If (drArrRemainingPartBalance.Length = 0) Then Continue For 'This record was never splitted for any recepient

                Dim drRemainingPartBalance As DataRow = drArrRemainingPartBalance(0)

                If EnsureAnnuityComponentsAreNonNegative(drRemainingPartBalance) = False Then
                    'Log debug info - Participant remaining annuity contained a negative component.
                    LogTraceInformation("Retired QDRO - Split Validation Function EnsureAnnuityComponentsAreNonNegative, For Participant remaining annuity contained a negative component Details - " + ConvertDataTableToString(drRemainingPartBalance))
                    Return False
                End If
                If IsAnnuityNegativeAfterSSLevelling(drRemainingPartBalance) = True Then
                    'Log debug info - Participant remaining annuity would be negative after SS reduction
                    LogTraceInformation("Retired QDRO - Split Validation Function IsAnnuityNegativeAfterSSLevelling, For Participant remaining contained negative after SS reduction Details - " + ConvertDataTableToString(drRemainingPartBalance))
                    Return False
                End If

                'Check to ensure there are no rounding issues. Totals should all match.
                If (drParticpantAnnuity("CurrentPayment") <> (dBeneAnnuityCurrentPayment + Convert.ToDecimal(drRemainingPartBalance("CurrentPayment")))) Then
                    Return False
                End If

                If (drParticpantAnnuity("SSReductionAmt") <> (dBeneAnnuitySSReductionAmt + Convert.ToDecimal(drRemainingPartBalance("SSReductionAmt")))) Then
                    Return False
                End If

                If (drParticpantAnnuity("SSLevelingAmt") <> (dBeneAnnuitySSLevelingAmt + Convert.ToDecimal(drRemainingPartBalance("SSLevelingAmt")))) Then
                    Return False
                End If

                If (drParticpantAnnuity("EmpPreTaxCurrentPayment") <> (dBeneAnnuityPreTaxCurrentPayment + Convert.ToDecimal(drRemainingPartBalance("EmpPreTaxCurrentPayment")))) Then
                    Return False
                End If

                If (drParticpantAnnuity("EmpPostTaxCurrentPayment") <> (dBeneAnnuityPostTaxCurrentPayment + Convert.ToDecimal(drRemainingPartBalance("EmpPostTaxCurrentPayment")))) Then
                    Return False
                End If

                If (drParticpantAnnuity("YmcaPreTaxCurrentPayment") <> (dBeneAnnuityYmcaPreTaxCurrentPayment + Convert.ToDecimal(drRemainingPartBalance("YmcaPreTaxCurrentPayment")))) Then
                    Return False
                End If

                If (drParticpantAnnuity("EmpPreTaxRemainingReserves") <> (dBeneAnnuityPreTaxRemainingReserves + Convert.ToDecimal(drRemainingPartBalance("EmpPreTaxRemainingReserves")))) Then
                    Return False
                End If

                If (drParticpantAnnuity("EmpPostTaxRemainingReserves") <> (dBeneAnnuityPostTaxRemainingReserves + Convert.ToDecimal(drRemainingPartBalance("EmpPostTaxRemainingReserves")))) Then
                    Return False
                End If

                If (drParticpantAnnuity("YmcapreTaxRemainingReserves") <> (dBeneAnnuityYmcapreTaxRemainingReserves + Convert.ToDecimal(drRemainingPartBalance("YmcapreTaxRemainingReserves")))) Then
                    Return False
                End If
            Next

        Catch
            Throw
        End Try
        Return True

    End Function
#End Region

#Region "Split"
    Private Function CheckIfAnnuityIsValid(ByVal dataGridParticipantAnnuity As DataGrid) As String
        Dim dgItem As DataGridItem
        Dim bitIncrease, bitJointSurviour, bitSSLeveling As Boolean
        Try

            For dgCount = 0 To dataGridParticipantAnnuity.Items.Count - 1
                dgItem = dataGridParticipantAnnuity.Items(dgCount)
                Dim checkboxSelect As CheckBox = DirectCast(dgItem.FindControl("CheckBoxSelect"), CheckBox)
                GetRetireeAnnuityType(dgItem.Cells(6).Text.Trim(), bitIncrease, bitJointSurviour, bitSSLeveling)
                Dim bitSSLAmountExist = dgItem.Cells(14).Text.Trim() <> "&nbsp;" AndAlso String.IsNullOrEmpty(dgItem.Cells(14).Text.Trim()) = False AndAlso Convert.ToDecimal(dgItem.Cells(14).Text.Trim()) <> 0
                Dim bitSSLReductionAmountExist = dgItem.Cells(15).Text.Trim() <> "&nbsp;" AndAlso String.IsNullOrEmpty(dgItem.Cells(15).Text.Trim()) = False AndAlso Convert.ToDecimal(dgItem.Cells(15).Text.Trim()) <> 0
                If (bitSSLeveling = False And checkboxSelect.Checked) Then
                    If (bitSSLAmountExist And bitSSLAmountExist) Then
                        Return String.Format(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_INVALID_DATA_FOR_NON_SSL_ANNUITY, dgItem.Cells(6).Text.Trim())
                    End If
                End If
            Next

        Catch
            Throw
        End Try
        Return ""
    End Function


    Private Function ReadCurrentData(ByVal dtRcptCurrentSplit As DataTable, ByVal dtParticpantOriginal As DataTable, ByVal totalAnnuityAmount As Decimal, ByVal PercentageSplit As Decimal) As DataTable

        Dim Chkbox As CheckBox
        Dim chkShareBenefit As CheckBox
        Dim dgCount As Integer
        Dim dtReturnValue As DataTable
        Dim drCurrentSplit As DataRow
        Dim dgItem As DataGridItem
        Dim drArryParticpant As DataRow()
        Dim drParticipant As DataRow
        Dim bitIncrease, bitSSLeveling, bitJointSurvivior As Boolean

        Try

            dtReturnValue = dtRcptCurrentSplit.Clone()

            For dgCount = 0 To DataGridWorkSheet.Items.Count - 1
                dgItem = DataGridWorkSheet.Items(dgCount)

                Chkbox = DirectCast(DataGridWorkSheet.Items(dgCount).FindControl("CheckBoxSelect"), CheckBox)
                chkShareBenefit = DirectCast(DataGridWorkSheet.Items(dgCount).FindControl("chkShareBenefit"), CheckBox)

                If IsNothing(Chkbox) Then Continue For

                If Not (Chkbox.Checked) Then Continue For

                'select the annuit records form original participant
                drArryParticpant = dtParticpantOriginal.Select("guiAnnuityID ='" + dgItem.Cells(0).Text.Trim() + "'")

                If (drArryParticpant.Length > 0) Then
                    drParticipant = drArryParticpant(0)

                    drCurrentSplit = dtReturnValue.NewRow()

                    'copy row of particpant into receipent row
                    GetRetireeAnnuityType(drParticipant("AnnuityType"), bitIncrease, bitJointSurvivior, bitSSLeveling)

                    drCurrentSplit("AnnuitySourceCode") = drParticipant("AnnuitySourceCode")
                    drCurrentSplit("PlanType") = drParticipant("PlanType")
                    drCurrentSplit("RecipientPersonID") = DropdownlistBeneficiarySSNo.SelectedValue.Trim()
                    drCurrentSplit("guiAnnuityID") = drParticipant("guiAnnuityID")
                    drCurrentSplit("CurrentPayment") = drParticipant("CurrentPayment")
                    drCurrentSplit("EmpPreTaxCurrentPayment") = drParticipant("EmpPreTaxCurrentPayment")
                    drCurrentSplit("EmpPostTaxCurrentPayment") = drParticipant("EmpPostTaxCurrentPayment")
                    drCurrentSplit("YmcaPreTaxCurrentPayment") = drParticipant("YmcaPreTaxCurrentPayment")
                    drCurrentSplit("SSLevelingAmt") = drParticipant("SSLevelingAmt")
                    drCurrentSplit("SSReductionAmt") = drParticipant("SSReductionAmt")
                    drCurrentSplit("SSReductionEftDate") = drParticipant("SSReductionEftDate")
                    drCurrentSplit("PurchaseDate") = drParticipant("PurchaseDate")
                    drCurrentSplit("OriginalAnnuityType") = drParticipant("AnnuityType")
                    drCurrentSplit("EmpPreTaxRemainingReserves") = drParticipant("EmpPreTaxRemainingReserves")
                    drCurrentSplit("EmpPostTaxRemainingReserves") = drParticipant("EmpPostTaxRemainingReserves")
                    drCurrentSplit("YmcapreTaxRemainingReserves") = drParticipant("YmcapreTaxRemainingReserves")
                    drCurrentSplit("IncludeSpecialdev") = CheckBoxSpecialDividends.Checked
                    drCurrentSplit("RecipientFundEventID") = Me.String_RecptFundEventID
                    drCurrentSplit("RecptRetireeID") = Me.String_RecptRetireeID
                    drCurrentSplit("AdjustmentBasisCode") = drParticipant("AdjustmentBasisCode")
                    drCurrentSplit("AnnuityTotal") = totalAnnuityAmount
                    drCurrentSplit("RecipientSplitPercent") = PercentageSplit
                    drCurrentSplit("guiAnnuityJointSurvivorsID") = drParticipant("guiAnnuityJointSurvivorsID")
                    drCurrentSplit("splitType") = Me.SplitPlanTypeOption

                    If Not IsNothing(chkShareBenefit) Then
                        drCurrentSplit("ShareBenefit") = Convert.ToString(chkShareBenefit.Checked)
                        drCurrentSplit("AnnuityType") = GetBeneficiaryAnnuityType(drParticipant("AnnuityType"), chkShareBenefit.Checked, bitIncrease, bitSSLeveling, bitJointSurvivior)

                    End If

                    If RadioButtonListSplitAmtType_Percentage.Checked Then 'PPP | 01/20/2017 | YRS-AT-3299 | Commented old condition which used Radiobutton group --If (RadioButtonListSplitAmtType.Items(1).Selected = True) Then
                        drCurrentSplit("IsSplitPercentage") = True
                        drCurrentSplit("SplitAmount") = TextBoxPercentageWorkSheet.Text.Trim()
                    ElseIf RadioButtonListSplitAmtType_Amount.Checked Then 'PPP | 01/20/2017 | YRS-AT-3299 | Commented old condition which used Radiobutton group --ElseIf (RadioButtonListSplitAmtType.Items(0).Selected = True) Then
                        drCurrentSplit("IsSplitPercentage") = False
                        drCurrentSplit("SplitAmount") = TextBoxAmountWorkSheet.Text.Trim()
                    End If

                    dtReturnValue.Rows.Add(drCurrentSplit)
                End If
            Next

        Catch
            Throw
        End Try
        Return dtReturnValue

    End Function

    Private Sub DetermineValueSplit(ByVal dtPartOriginal As DataTable, ByVal dtRcptSelectedAnnuities As DataTable, ByVal dtRecpExistingSplit As DataTable, ByRef dtPartRemainingbalance As DataTable)
        Dim splitAmount As Decimal
        Dim totalAvailableAnnuityValue As Decimal
        Try
            'Get the total amount of annuity available for split
            For Each dr As DataRow In dtRcptSelectedAnnuities.Rows
                totalAvailableAnnuityValue += GetEffectiveCurrentPaymentForAnnuity(dr)
            Next



            'Identify how much we need to give to the beneficiary for each annuity
            'If split is specified as a fixed amount then the value specified is what we have to split
            'If the split is specified as a percentage split then the value has to be computed by based on the total of all annuities
            If (dtRcptSelectedAnnuities.Rows(0)("IsSplitPercentage").ToString().ToLower = "false") Then
                splitAmount = dtRcptSelectedAnnuities.Rows(0)("SplitAmount")
            Else
                splitAmount = Math.Round(totalAvailableAnnuityValue * dtRcptSelectedAnnuities.Rows(0)("SplitAmount") / 100, 2, MidpointRounding.AwayFromZero)
            End If
            'Now we need to prorate the split amount across all annuities

            For Each dr As DataRow In dtRcptSelectedAnnuities.Rows
                Dim currAnnuityEffectiveCurrPayment As Decimal = GetEffectiveCurrentPaymentForAnnuity(dr)
                dr("CurrentPayment") = Math.Round(currAnnuityEffectiveCurrPayment * splitAmount / totalAvailableAnnuityValue, 2, MidpointRounding.AwayFromZero)
            Next

            If (HelperFunctions.isNonEmpty(dtRecpExistingSplit)) Then

                Dim isCurrentSplitPercentage As Boolean = (dtRcptSelectedAnnuities.Rows(0)("IsSplitPercentage").ToString().ToLower = "true")

                'Try to check if exisitng split is total percentage then sum the current percentage is equal to 100
                Dim isExisitngSplitContainFixed As Boolean = IIf(dtRecpExistingSplit.Select("IsSplitPercentage='false'").Count() > 0, True, False)

                If isExisitngSplitContainFixed = False AndAlso isCurrentSplitPercentage Then
                    For Each dr As DataRow In dtPartOriginal.Rows
                        Dim strExpression As String = "guiAnnuityID='" + dr("guiAnnuityID") + "'"
                        For Each drExisting As DataRow In dtRecpExistingSplit.Rows
                            If drExisting("guiAnnuityID") <> dr("guiAnnuityID") Then Continue For
                            Dim totalPercentage As Decimal = 0
                            Dim totalAnnuitySum As Object = dtRecpExistingSplit.Compute("SUM(SplitAmount)", strExpression)


                            If (IsDBNull(totalAnnuitySum)) Then
                                totalPercentage = 0
                            End If
                            totalPercentage += totalAnnuitySum
                            totalPercentage += Convert.ToDecimal(dtRcptSelectedAnnuities.Rows(0)("SplitAmount"))

                            If (totalPercentage = 100) Then
                                Dim drRemainingtAnnuity As DataRow() = dtPartRemainingbalance.Select(strExpression)
                                Dim dtRcptSelctedAnnuities As DataRow() = dtRcptSelectedAnnuities.Select(strExpression)
                                Dim drExistingRecpt = dtRecpExistingSplit.Select(strExpression)

                                Dim totalExisitngRecptAnnuitypayment As Decimal = 0

                                For Each drTemp As DataRow In drExistingRecpt
                                    totalExisitngRecptAnnuitypayment += Math.Round(dr("CurrentPayment") * drTemp("SplitAmount") / 100, 2, MidpointRounding.AwayFromZero)
                                Next

                                If (drRemainingtAnnuity.Length > 1) Then Throw New Exception("Multiple remaining annuity found with same annuity type")
                                If (dtRcptSelctedAnnuities.Length > 1) Then Throw New Exception("Multiple annuties found for current beneficiary with same type ")

                                Dim bitIncrease, bitJointSurvivior, bitSSLeveling As Boolean
                                GetRetireeAnnuityType(dr("AnnuityType"), bitIncrease, bitJointSurvivior, bitSSLeveling)

                                If (dtRcptSelctedAnnuities.Length > 0 AndAlso drRemainingtAnnuity.Length > 0 AndAlso bitSSLeveling) Then ' 
                                    'If bitSSLeveling Then 'check if ssl leveling annuity then adjsu esle assigned the remainig amount
                                    If (dtRcptSelctedAnnuities(0)("ShareBenefit").ToString.ToLower = "false") Then

                                        If (dtRcptSelctedAnnuities(0)("IsSplitPercentage").ToString().ToLower = "false") Then
                                            splitAmount -= dtRcptSelctedAnnuities(0)("SplitAmount")
                                        Else
                                            splitAmount -= Math.Round(GetEffectiveCurrentPaymentForAnnuity(dr, dr("AnnuityType"), dtRcptSelctedAnnuities(0)("ShareBenefit")) * dtRcptSelctedAnnuities(0)("SplitAmount") / 100, 2, MidpointRounding.AwayFromZero)
                                        End If

                                        dtRcptSelctedAnnuities(0)("CurrentPayment") = GetEffectiveCurrentPaymentForAnnuity(drRemainingtAnnuity(0), drRemainingtAnnuity(0)("AnnuityType"), dtRcptSelctedAnnuities(0)("ShareBenefit"))
                                        splitAmount += dtRcptSelctedAnnuities(0)("CurrentPayment")
                                    Else
                                        dtRcptSelctedAnnuities(0)("CurrentPayment") = dr("CurrentPayment") - totalExisitngRecptAnnuitypayment
                                    End If
                                    'Else
                                    '    dtRcptSelctedAnnuities(0)("CurrentPayment") = drRemainingtAnnuity(0)("CurrentPayment")
                                    'End If

                                End If

                            End If

                        Next

                    Next

                End If


            End If

            For Each drRcpt As DataRow In dtRcptSelectedAnnuities.Rows
                For Each drRemain As DataRow In dtPartRemainingbalance.Rows
                    If (drRemain("guiAnnuityID") <> drRcpt("guiAnnuityID")) Then Continue For
                    If (drRemain("CurrentPayment") < drRcpt("CurrentPayment")) Then
                        drRcpt("CurrentPayment") -= drRcpt("CurrentPayment") - drRemain("CurrentPayment")
                    End If
                Next
            Next

            AdjustAnnuityDiffernceintoMaximumAnnuity(splitAmount, dtRcptSelectedAnnuities, dtPartRemainingbalance)

            ''Remove from PartRemaining whatever was assigned to the beneficiary
            'For Each drRcpt As DataRow In dtRcptSelectedAnnuities.Rows
            '    For Each drPart As DataRow In dtPartRemainingbalance.Rows
            '        If (drPart("guiAnnuityID") <> drRcpt("guiAnnuityID")) Then Continue For
            '        drPart("CurrentPayment") -= drRcpt("CurrentPayment")
            '    Next
            'Next
        Catch
            Throw
        End Try
    End Sub

    Private Sub AdjustAnnuityDiffernceintoMaximumAnnuity(ByVal splitAmount As Decimal, ByRef dtRcptSelectedAnnuities As DataTable, ByRef dtPartRemainingbalance As DataTable)
        Dim dtTempTable As DataTable
        dtTempTable = dtRcptSelectedAnnuities.Copy()
        Try
            Dim dv As DataView = dtTempTable.DefaultView
            dv.Sort = "CurrentPayment desc"

            'caluclate running total
            Dim runningTotal As Decimal
            For Each dr As DataRow In dtRcptSelectedAnnuities.Rows
                runningTotal += dr("CurrentPayment")
            Next

            Dim amountToGive As Decimal = splitAmount - runningTotal
            If amountToGive = 0 Then Exit Sub

            For index As Integer = 0 To dv.Table.Rows.Count - 1
                Dim drPartRemainingAnnuity As DataRow = dtPartRemainingbalance.Select("guiAnnuityID = '" + dv(index).Item("guiAnnuityID").ToString + "'")(0)
                'Dim partRemainingAnnuityAmt As Decimal = drPartRemainingAnnuity("CurrentPayment")
                Dim partRemainingAnnuityAmt As Decimal = GetEffectiveCurrentPaymentForAnnuity(drPartRemainingAnnuity, drPartRemainingAnnuity("AnnuityType"), dv(index).Item("ShareBenefit"))
                If partRemainingAnnuityAmt < dv(index).Item("CurrentPayment") Then Throw New Exception("participant remaining annuity will be -ve.") 'Not possible since we are adjusting before making call
                If partRemainingAnnuityAmt < dv(index).Item("CurrentPayment") + amountToGive Then
                    'Cannot adjust from this annuity entirely. Perform partial adjustment.
                    Dim altAdjustAmount As Decimal = partRemainingAnnuityAmt - dv(index).Item("CurrentPayment")
                    dv(index).Item("CurrentPayment") = dv(index).Item("CurrentPayment") + (altAdjustAmount)
                    amountToGive -= altAdjustAmount
                Else    'Can adjust entirely from this annuity
                    dv(index).Item("CurrentPayment") = dv(index).Item("CurrentPayment") + (amountToGive)
                    amountToGive = 0
                    Exit For
                End If
            Next
            If amountToGive <> 0 Then LogTraceInformation("Retired QDRO Split\Adjust: AdjustAnnuityDiffernceintoMaximumAnnuity () not adjust remaining Annuity amount remains to be adjusted" + amountToGive.ToString)

            'Loop to copy values so that display on UI is maintained
            For Each dr As DataRow In dtRcptSelectedAnnuities.Rows
                For Each drtemp As DataRow In dtTempTable.Rows
                    If (dr("guiAnnuityID") = drtemp("guiAnnuityID")) Then
                        dr("CurrentPayment") = drtemp("CurrentPayment")
                    End If
                Next
            Next
        Catch
            Throw
        End Try
    End Sub



    'Private Function SplitParticipantBalance(ByVal DataTableParticipant As DataTable, ByVal DataTableRecipant As DataTable, ByVal DataTablePartAccountbalance As DataTable, ByVal SelectedAnnuityAmount As Double)
    '    Dim iCount As Integer
    '    Dim dtRowRecipant As DataRow
    '    Dim dtRowParticipant As DataRow
    '    Dim dgItems As DataGridItem
    '    Dim dtRow As DataRow
    '    Dim dgCount As Integer
    '    Dim Chkbox As CheckBox
    '    Dim TotalIndividualSplit As Double
    '    'Start -SR:2014.06.26  BT-2445\YRS 5.0-633
    '    Dim chkShareBenefit As CheckBox
    '    Dim decCurrentPaymentAfterReduction As Decimal
    '    Dim decTotalCurrentpayment As Decimal
    '    Dim dsAnnuityTypes As New DataSet
    '    Dim bitIncrease As Boolean
    '    Dim bitJointSurvivior As Boolean
    '    Dim bitSSLeveling As Boolean
    '    Dim splitPercentageForSSLAnnuity As Decimal
    '    Dim participantSSLAmount As Decimal
    '    Dim participantCurrentPayment As Decimal
    '    Dim participantEmpPreTaxAmount As Decimal
    '    Dim participantEmpPostTaxAmount As Decimal
    '    Dim participantYmcaPreTaxAmount As Decimal
    '    Dim participantSSReductionAmount As Decimal
    '    Dim participantpreTaxReserveAmount As Decimal
    '    Dim participantPostTaxReserveAmount As Decimal
    '    Dim participantYmcaPreTaxReserveAmount As Decimal

    '    'End -SR:2014.06.26  BT-2445\YRS 5.0-633 

    '    Try
    '        'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START
    '        For dgCount = 0 To DataGridWorkSheet.Items.Count - 1
    '            Chkbox = DirectCast(DataGridWorkSheet.Items(dgCount).FindControl("CheckBoxSelect"), CheckBox)
    '            chkShareBenefit = DirectCast(DataGridWorkSheet.Items(dgCount).FindControl("chkShareBenefit"), CheckBox)
    '            If Not IsNothing(Chkbox) Then
    '                DataTableParticipant.Rows(dgCount)("Selected") = Convert.ToString(Chkbox.Checked)
    '                DataTablePartAccountbalance.Rows(dgCount)("Selected") = Convert.ToString(Chkbox.Checked)
    '            End If
    '            'Start -SR:2014.06.26  BT-2445\YRS 5.0-633 
    '            If Not IsNothing(chkShareBenefit) Then
    '                DataTableParticipant.Rows(dgCount)("ShareBenefit") = Convert.ToString(chkShareBenefit.Checked)
    '                DataTablePartAccountbalance.Rows(dgCount)("ShareBenefit") = Convert.ToString(chkShareBenefit.Checked)
    '            End If
    '            'End -SR:2014.06.26  BT-2445\YRS 5.0-633 
    '        Next
    '        'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END
    '        For iCount = 0 To DataTableParticipant.Rows.Count - 1
    '            dgItems = DataGridWorkSheet.Items(iCount)
    '            dtRowRecipant = DataTableRecipant.NewRow()
    '            dtRowParticipant = DataTablePartAccountbalance.Rows(iCount)
    '            'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START 
    '            'Dim SelectCheckbox As CheckB = CType(dgItems.Cells(0).Controls(1), CheckBox)
    '            'If SelectCheckbox.Checked = True Then
    '            If Convert.ToString(DataTableParticipant.Rows(iCount)("Selected")).Trim.ToLower() = "true" Then
    '                GetRetireeAnnuityType(DataTableParticipant.Rows(iCount).Item("AnnuityType"), bitIncrease, bitJointSurvivior, bitSSLeveling)
    '                'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END
    '                dtRowRecipant("RecipientPersonID") = DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString()
    '                dtRowRecipant("AnnuitySourceCode") = DataTableParticipant.Rows(iCount).Item("AnnuitySourceCode")
    '                dtRowRecipant("PlanType") = DataTableParticipant.Rows(iCount).Item("PlanType")
    '                dtRowRecipant("PurchaseDate") = DataTableParticipant.Rows(iCount).Item("PurchaseDate")
    '                'dtRowRecipant("AnnuityType") = DataTableParticipant.Rows(iCount).Item("AnnuityType")
    '                dtRowRecipant("AnnuityType") = GetBeneficiaryAnnuityType(DataTableParticipant.Rows(iCount).Item("AnnuityType"), DataTableParticipant.Rows(iCount).Item("ShareBenefit"), bitIncrease, bitSSLeveling, bitJointSurvivior)

    '                'Harshala : 11 Jun 2012 : BT ID : 981 - YRS 5.0-1525 - annuity adjustment values are incorrect : START
    '                dtRowRecipant("SSLevelingAmt") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("SSLevelingAmt") * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)
    '                dtRowRecipant("SSReductionAmt") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("SSReductionAmt") * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)
    '                'Harshala : 11 Jun 2012 : BT ID : 981 - YRS 5.0-1525 - annuity adjustment values are incorrect : END
    '                dtRowRecipant("SSReductionEftDate") = DataTableParticipant.Rows(iCount).Item("SSReductionEftDate")
    '                'Harshala : 11 Jun 2012 : BT ID : 981 - YRS 5.0-1525 - annuity adjustment values are incorrect : START
    '                dtRowRecipant("CurrentPayment") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("CurrentPayment") * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)
    '                dtRowRecipant("EmpPreTaxCurrentPayment") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("EmpPreTaxCurrentPayment") * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)
    '                dtRowRecipant("EmpPostTaxCurrentPayment") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("EmpPostTaxCurrentPayment") * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)
    '                dtRowRecipant("YmcaPreTaxCurrentPayment") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("YmcaPreTaxCurrentPayment") * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)
    '                dtRowRecipant("EmpPreTaxRemainingReserves") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("EmpPreTaxRemainingReserves") * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)
    '                dtRowRecipant("EmpPostTaxRemainingReserves") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("EmpPostTaxRemainingReserves") * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)
    '                dtRowRecipant("YmcapreTaxRemainingReserves") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("YmcaPreTaxRemainingReserves") * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)
    '                'Harshala : 11 Jun 2012 : BT ID : 981 - YRS 5.0-1525 - annuity adjustment values are incorrect : END
    '                dtRowRecipant("RecipientSplitPercent") = PercentageForSplit
    '                dtRowRecipant("guiAnnuityID") = DataTableParticipant.Rows(iCount).Item("guiAnnuityID")
    '                dtRowRecipant("IncludeSpecialdev") = CheckBoxSpecialDividends.Checked
    '                dtRowRecipant("AnnuityTotal") = SelectedAnnuityAmount
    '                dtRowRecipant("RecipientFundEventID") = Me.String_RecptFundEventID
    '                dtRowRecipant("RecptRetireeID") = Me.String_RecptRetireeID
    '                dtRowRecipant("splitType") = RadioButtonListPlanType.SelectedItem.Value.ToString()
    '                dtRowRecipant("AdjustmentBasisCode") = DataTableParticipant.Rows(iCount).Item("AdjustmentBasisCode")
    '                dtRowRecipant("guiAnnuityJointSurvivorsID") = DataTableParticipant.Rows(iCount).Item("guiAnnuityJointSurvivorsID")

    '                'SP:2014.07.01  BT-2445\YRS 5.0-633 -Start
    '                dtRowRecipant("ShareBenefit") = DataTableParticipant.Rows(iCount).Item("ShareBenefit")

    '                If (RadioButtonListSplitAmtType.Items(1).Selected = True) Then
    '                    dtRowRecipant("IsSplitPercentage") = True
    '                    dtRowRecipant("SplitAmount") = TextBoxPercentageWorkSheet.Text.Trim()
    '                ElseIf (RadioButtonListSplitAmtType.Items(0).Selected = True) Then
    '                    dtRowRecipant("IsSplitPercentage") = False
    '                    dtRowRecipant("SplitAmount") = TextBoxAmountWorkSheet.Text.Trim()

    '                End If
    '                'SP:2014.07.01  BT-2445\YRS 5.0-633 -End
    '                'Start -SR:2014.06.26  BT-2445\YRS 5.0-633
    '                If DataTableParticipant.Rows(iCount).Item("ShareBenefit") = False AndAlso bitSSLeveling Then
    '                    decCurrentPaymentAfterReduction = DataTableParticipant.Rows(iCount).Item("EmpPreTaxCurrentPayment") + DataTableParticipant.Rows(iCount).Item("EmpPostTaxCurrentPayment") + DataTableParticipant.Rows(iCount).Item("YmcaPreTaxCurrentPayment") + DataTableParticipant.Rows(iCount).Item("SSLevelingAmt") - DataTableParticipant.Rows(iCount).Item("SSReductionAmt")
    '                    dtRowRecipant("CurrentPayment") = Math.Round(((decCurrentPaymentAfterReduction * PercentageForSplit) / 100), 2, MidpointRounding.AwayFromZero)

    '                    splitPercentageForSSLAnnuity = (dtRowRecipant("CurrentPayment") / (DataTableParticipant.Rows(iCount).Item("EmpPreTaxCurrentPayment") + DataTableParticipant.Rows(iCount).Item("EmpPostTaxCurrentPayment") + DataTableParticipant.Rows(iCount).Item("YmcaPreTaxCurrentPayment"))) * 100

    '                    dtRowRecipant("EmpPreTaxCurrentPayment") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("EmpPreTaxCurrentPayment") * splitPercentageForSSLAnnuity) / 100), 2, MidpointRounding.AwayFromZero)
    '                    dtRowRecipant("EmpPostTaxCurrentPayment") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("EmpPostTaxCurrentPayment") * splitPercentageForSSLAnnuity) / 100), 2, MidpointRounding.AwayFromZero)
    '                    dtRowRecipant("YmcaPreTaxCurrentPayment") = Math.Round((Convert.ToDecimal(DataTableParticipant.Rows(iCount).Item("YmcaPreTaxCurrentPayment") * splitPercentageForSSLAnnuity) / 100), 2, MidpointRounding.AwayFromZero)
    '                    dtRowRecipant("SSLevelingAmt") = 0
    '                    dtRowRecipant("SSReductionAmt") = 0
    '                    dtRowRecipant("SSReductionEftDate") = DBNull.Value

    '                End If
    '                'SP 2014.07.03  BT-2445\YRS 5.0-633-Start
    '                If DataTableParticipant.Rows(iCount).Item("ShareBenefit") = False AndAlso Not (bitSSLeveling) Then
    '                    dtRowRecipant("SSLevelingAmt") = 0
    '                    dtRowRecipant("SSReductionAmt") = 0
    '                    dtRowRecipant("SSReductionEftDate") = DBNull.Value

    '                End If
    '                'SP 2014.07.03  BT-2445\YRS 5.0-633-End
    '                decTotalCurrentpayment = decTotalCurrentpayment + dtRowRecipant("CurrentPayment")
    '                If iCount = DataTableParticipant.Rows.Count - 1 Then
    '                    If Convert.ToDecimal(TextBoxAmountWorkSheet.Text) > decTotalCurrentpayment Then
    '                        dtRowRecipant("CurrentPayment") = dtRowRecipant("CurrentPayment") + (Convert.ToDecimal(TextBoxAmountWorkSheet.Text) - decTotalCurrentpayment)
    '                    End If
    '                End If
    '                'End -SR:2014.06.26  BT-2445\YRS 5.0-633

    '                'SP 2014.07.16  BT-2445\YRS 5.0-633-Start
    '                participantCurrentPayment = IIf(DataTablePartAccountbalance.Rows(iCount).Item("CurrentPayment") = 0, 0, Math.Round(DataTablePartAccountbalance.Rows(iCount).Item("CurrentPayment") - dtRowRecipant("CurrentPayment"), 2))
    '                participantEmpPreTaxAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxCurrentPayment") = 0, 0, Math.Round(DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxCurrentPayment") - dtRowRecipant("EmpPreTaxCurrentPayment"), 2))
    '                participantEmpPostTaxAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxCurrentPayment") = 0, 0, Math.Round(DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxCurrentPayment") - dtRowRecipant("EmpPostTaxCurrentPayment"), 2))
    '                participantYmcaPreTaxAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxCurrentPayment") = 0, 0, Math.Round(DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxCurrentPayment") - dtRowRecipant("YmcaPreTaxCurrentPayment"), 2))
    '                participantSSLAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("SSLevelingAmt") = 0, 0, Math.Round(DataTablePartAccountbalance.Rows(iCount).Item("SSLevelingAmt") - dtRowRecipant("SSLevelingAmt"), 2))
    '                participantSSReductionAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt") = 0, 0, Math.Round(DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt") - dtRowRecipant("SSReductionAmt"), 2))
    '                If (participantCurrentPayment < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("CurrentPayment") <> 0) Then
    '                    dtRowRecipant("CurrentPayment") = dtRowRecipant("CurrentPayment") + participantCurrentPayment
    '                End If

    '                If (participantEmpPreTaxAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxCurrentPayment") <> 0) Then
    '                    dtRowRecipant("EmpPreTaxCurrentPayment") = dtRowRecipant("EmpPreTaxCurrentPayment") + participantEmpPreTaxAmount
    '                End If

    '                If (participantEmpPostTaxAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxCurrentPayment") <> 0) Then
    '                    dtRowRecipant("EmpPostTaxCurrentPayment") = dtRowRecipant("EmpPostTaxCurrentPayment") + participantEmpPostTaxAmount
    '                End If

    '                If (participantYmcaPreTaxAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxCurrentPayment") <> 0) Then
    '                    dtRowRecipant("YmcaPreTaxCurrentPayment") = dtRowRecipant("YmcaPreTaxCurrentPayment") + participantYmcaPreTaxAmount
    '                End If

    '                If participantSSLAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("SSLevelingAmt") <> 0 Then
    '                    dtRowRecipant("SSLevelingAmt") = dtRowRecipant("SSLevelingAmt") + participantSSLAmount
    '                End If

    '                If participantSSReductionAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt") <> 0 Then
    '                    dtRowRecipant("SSReductionAmt") = dtRowRecipant("SSReductionAmt") + participantSSReductionAmount
    '                End If
    '                'SP 2014.07.16  BT-2445\YRS 5.0-633-End


    '                TotalIndividualSplit = dtRowRecipant("EmpPreTaxCurrentPayment") + dtRowRecipant("EmpPostTaxCurrentPayment") + dtRowRecipant("YmcaPreTaxCurrentPayment")
    '                'Harshala : 11 Jun 2012 : BT ID : 981 - YRS 5.0-1525 - annuity adjustment values are incorrect : START
    '                If dtRowRecipant("CurrentPayment") <> TotalIndividualSplit Then
    '                    If dtRowRecipant("EmpPreTaxCurrentPayment") > dtRowRecipant("EmpPostTaxCurrentPayment") AndAlso dtRowRecipant("EmpPreTaxCurrentPayment") > dtRowRecipant("YmcaPreTaxCurrentPayment") Then
    '                        dtRowRecipant("EmpPreTaxCurrentPayment") = dtRowRecipant("EmpPreTaxCurrentPayment") + (dtRowRecipant("CurrentPayment") - TotalIndividualSplit)
    '                    ElseIf dtRowRecipant("EmpPostTaxCurrentPayment") > dtRowRecipant("EmpPreTaxCurrentPayment") AndAlso dtRowRecipant("EmpPostTaxCurrentPayment") > dtRowRecipant("YmcaPreTaxCurrentPayment") Then
    '                        dtRowRecipant("EmpPostTaxCurrentPayment") = dtRowRecipant("EmpPostTaxCurrentPayment") + (dtRowRecipant("CurrentPayment") - TotalIndividualSplit)
    '                    ElseIf dtRowRecipant("YmcaPreTaxCurrentPayment") > dtRowRecipant("EmpPreTaxCurrentPayment") AndAlso dtRowRecipant("YmcaPreTaxCurrentPayment") > dtRowRecipant("EmpPostTaxCurrentPayment") Then ' dtRowRecipant("EmpPreTaxCurrentPayment") Then 'SP 2014.07.03  BT-2445\YRS 5.0-633
    '                        dtRowRecipant("YmcaPreTaxCurrentPayment") = dtRowRecipant("YmcaPreTaxCurrentPayment") + (dtRowRecipant("CurrentPayment") - TotalIndividualSplit)
    '                    End If
    '                End If
    '                'Harshala : 11 Jun 2012 : BT ID : 981 - YRS 5.0-1525 - annuity adjustment values are incorrect : END

    '                dtRowParticipant("AnnuitySourceCode") = DataTablePartAccountbalance.Rows(iCount).Item("AnnuitySourceCode")
    '                dtRowParticipant("PlanType") = DataTablePartAccountbalance.Rows(iCount).Item("PlanType")
    '                dtRowParticipant("PurchaseDate") = DataTablePartAccountbalance.Rows(iCount).Item("PurchaseDate")
    '                dtRowParticipant("AnnuityType") = DataTablePartAccountbalance.Rows(iCount).Item("AnnuityType")
    '                dtRowParticipant("CurrentPayment") = DataTablePartAccountbalance.Rows(iCount).Item("CurrentPayment") - dtRowRecipant("CurrentPayment")

    '                ''SP 2014.07.02 YRS 5.0-633 ,start, Adding if condtion
    '                'If DataTableParticipant.Rows(iCount).Item("ShareBenefit") = False AndAlso DataTableParticipant.Rows(iCount).Item("Annuitytype").ToString.ToLower.Contains("s") Then
    '                '    decCurrentPaymentAfterReduction = (DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxCurrentPayment") + DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxCurrentPayment") + DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxCurrentPayment") + DataTablePartAccountbalance.Rows(iCount).Item("SSLevelingAmt") - DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt"))
    '                '    dtRowParticipant("CurrentPayment") = (decCurrentPaymentAfterReduction - dtRowRecipant("CurrentPayment"))
    '                'End If
    '                ''SP 2014.07.02 YRS 5.0-633 ,start, Adding if condtion

    '                'SP 2014.07.10 BT-2445\YRS 5.0-633 -Start

    '                'participantSSLAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("SSLevelingAmt") = 0, 0, Math.Round(Math.Abs(DataTablePartAccountbalance.Rows(iCount).Item("SSLevelingAmt")) - dtRowRecipant("SSLevelingAmt"), 2))
    '                'participantSSReductionAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt") = 0, 0, Math.Round(Math.Abs(DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt")) - dtRowRecipant("SSReductionAmt"), 2))
    '                'participantSSReductionAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt") = 0, 0, Math.Round(Math.Abs(DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt")) - dtRowRecipant("SSReductionAmt"), 2))
    '                'participantpreTaxReserveAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxRemainingReserves") = 0, 0, Math.Round(Math.Abs(DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxRemainingReserves")) - dtRowRecipant("EmpPreTaxRemainingReserves"), 2))
    '                'participantPostTaxReserveAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxRemainingReserves") = 0, 0, Math.Round(Math.Abs(DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxRemainingReserves")) - dtRowRecipant("EmpPostTaxRemainingReserves"), 2))
    '                'participantYmcaPreTaxReserveAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxRemainingReserves") = 0, 0, Math.Round(Math.Abs(DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxRemainingReserves")) - dtRowRecipant("YmcaPreTaxRemainingReserves"), 2))

    '                'If participantSSLAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("SSLevelingAmt") <> 0 Then
    '                '    'dtRowRecipant("SSLevelingAmt") = IIf(dtRowRecipant("SSLevelingAmt") < 0, (dtRowRecipant("SSLevelingAmt") - participantSSLAmount), (dtRowRecipant("SSLevelingAmt") + participantSSLAmount))
    '                'End If

    '                'If participantSSReductionAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt") <> 0 Then
    '                '    dtRowRecipant("SSReductionAmt") = IIf(dtRowRecipant("SSReductionAmt") < 0, (dtRowRecipant("SSReductionAmt") - participantSSReductionAmount), (dtRowRecipant("SSReductionAmt") + participantSSReductionAmount))
    '                'End If

    '                'If (participantpreTaxReserveAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxRemainingReserves") <> 0) Then
    '                '    dtRowRecipant("EmpPreTaxRemainingReserves") = IIf(dtRowRecipant("EmpPreTaxRemainingReserves") < 0, (dtRowRecipant("EmpPreTaxRemainingReserves") - participantpreTaxReserveAmount), (dtRowRecipant("EmpPreTaxRemainingReserves") + participantpreTaxReserveAmount))
    '                'End If

    '                'If (participantPostTaxReserveAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxRemainingReserves") <> 0) Then
    '                '    dtRowRecipant("EmpPostTaxRemainingReserves") = IIf(dtRowRecipant("EmpPostTaxRemainingReserves") < 0, (dtRowRecipant("EmpPostTaxRemainingReserves") - participantPostTaxReserveAmount), (dtRowRecipant("EmpPostTaxRemainingReserves") + participantPostTaxReserveAmount))
    '                'End If

    '                'If (participantYmcaPreTaxReserveAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxRemainingReserves") <> 0) Then
    '                '    dtRowRecipant("YmcaPreTaxRemainingReserves") = IIf(dtRowRecipant("YmcaPreTaxRemainingReserves") < 0, (dtRowRecipant("YmcaPreTaxRemainingReserves") - participantYmcaPreTaxReserveAmount), (dtRowRecipant("YmcaPreTaxRemainingReserves") + participantYmcaPreTaxReserveAmount))
    '                'End If

    '                participantpreTaxReserveAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxRemainingReserves") = 0, 0, Math.Round(Math.Abs(DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxRemainingReserves")) - Math.Abs(dtRowRecipant("EmpPreTaxRemainingReserves")), 2))
    '                participantPostTaxReserveAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxRemainingReserves") = 0, 0, Math.Round(Math.Abs(DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxRemainingReserves")) - Math.Abs(dtRowRecipant("EmpPostTaxRemainingReserves")), 2))
    '                participantYmcaPreTaxReserveAmount = IIf(DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxRemainingReserves") = 0, 0, Math.Round(Math.Abs(DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxRemainingReserves")) - Math.Abs(dtRowRecipant("YmcaPreTaxRemainingReserves")), 2))


    '                'SPlit 

    '                If (participantpreTaxReserveAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxRemainingReserves") <> 0) Then
    '                    dtRowRecipant("EmpPreTaxRemainingReserves") = IIf(DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxRemainingReserves") < 0, ((Math.Abs(dtRowRecipant("EmpPreTaxRemainingReserves")) + participantpreTaxReserveAmount) * -1), (dtRowRecipant("EmpPreTaxRemainingReserves") + participantpreTaxReserveAmount))
    '                End If

    '                If (participantPostTaxReserveAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxRemainingReserves") <> 0) Then
    '                    dtRowRecipant("EmpPostTaxRemainingReserves") = IIf(dtRowRecipant("EmpPostTaxRemainingReserves") < 0, ((Math.Abs(dtRowRecipant("EmpPostTaxRemainingReserves")) + participantPostTaxReserveAmount) * -1), (dtRowRecipant("EmpPostTaxRemainingReserves") + participantPostTaxReserveAmount))
    '                End If

    '                If (participantYmcaPreTaxReserveAmount < 0 AndAlso DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxRemainingReserves") <> 0) Then
    '                    dtRowRecipant("YmcaPreTaxRemainingReserves") = IIf(dtRowRecipant("YmcaPreTaxRemainingReserves") < 0, ((Math.Abs(dtRowRecipant("YmcaPreTaxRemainingReserves")) + participantYmcaPreTaxReserveAmount) * -1), (dtRowRecipant("YmcaPreTaxRemainingReserves") + participantYmcaPreTaxReserveAmount))
    '                End If

    '                'SP 2014.07.10 BT-2445\YRS 5.0-633 -End

    '                dtRowParticipant("SSLevelingAmt") = DataTablePartAccountbalance.Rows(iCount).Item("SSLevelingAmt") - dtRowRecipant("SSLevelingAmt")
    '                dtRowParticipant("SSReductionAmt") = DataTablePartAccountbalance.Rows(iCount).Item("SSReductionAmt") - dtRowRecipant("SSReductionAmt")
    '                dtRowParticipant("SSReductionEftDate") = DataTablePartAccountbalance.Rows(iCount).Item("SSReductionEftDate")
    '                dtRowParticipant("EmpPreTaxCurrentPayment") = DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxCurrentPayment") - dtRowRecipant("EmpPreTaxCurrentPayment")
    '                dtRowParticipant("EmpPostTaxCurrentPayment") = DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxCurrentPayment") - dtRowRecipant("EmpPostTaxCurrentPayment")
    '                dtRowParticipant("YmcaPreTaxCurrentPayment") = DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxCurrentPayment") - dtRowRecipant("YmcaPreTaxCurrentPayment")
    '                dtRowParticipant("EmpPreTaxRemainingReserves") = DataTablePartAccountbalance.Rows(iCount).Item("EmpPreTaxRemainingReserves") - dtRowRecipant("EmpPreTaxRemainingReserves")
    '                dtRowParticipant("EmpPostTaxRemainingReserves") = DataTablePartAccountbalance.Rows(iCount).Item("EmpPostTaxRemainingReserves") - dtRowRecipant("EmpPostTaxRemainingReserves")
    '                dtRowParticipant("YmcapreTaxRemainingReserves") = DataTablePartAccountbalance.Rows(iCount).Item("YmcaPreTaxRemainingReserves") - dtRowRecipant("YmcaPreTaxRemainingReserves")
    '                DataTableRecipant.Rows.Add(dtRowRecipant)
    '            End If

    '        Next

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    '    Return True
    'End Function
#End Region

#End Region
    'SP 2014.06.24 - BT-2445\YRS 5.0-633 -Start

    Private Function CreatePopupMessageDatatable(ByVal dtParticipant As DataTable, ByVal dtReceipant As DataTable, ByVal dtBenfitAccount As DataTable) As DataTable

        Dim chkbox, chkShareBenefit As CheckBox
        Dim dtParticipantTemp As DataTable
        Dim dtReceipantTemp As DataTable
        Dim dtBenifAccountTemp As DataTable
        Dim dtFinal As New DataTable
        Dim drFinal As DataRow
        Dim drArrRecipient As DataRow()
        Dim drArrParticipant As DataRow()

        Try
            dtParticipantTemp = dtParticipant.Copy()
            dtReceipantTemp = dtReceipant.Copy()
            dtBenifAccountTemp = dtBenfitAccount.Copy()

            dtFinal.Columns.Add("SSN", GetType(System.String))
            dtFinal.Columns.Add("Name", GetType(System.String))
            dtFinal.Columns.Add("AnnuityType", GetType(System.String))
            dtFinal.Columns.Add("ShareBenefit", GetType(System.String))
            dtFinal.Columns.Add("NotShareBenefit", GetType(System.String))

            'Check if more than one beneficiary then create each beneficiary annuities table.
            'This is because in case of one beneficiary no grid should be displayed.
            If (HelperFunctions.isNonEmpty(dtBenifAccountTemp) And dtBenifAccountTemp.Rows.Count > 1) Then

                'Check multiple beneficiary
                For Each drRow As DataRow In dtBenifAccountTemp.Rows
                    'Find annuites list for each beneficiary
                    drArrRecipient = dtReceipantTemp.Select("RecipientPersonID ='" + drRow("id").ToString() + "'")

                    'check if annuity finds
                    If (drArrRecipient.Length > 0) Then

                        For Each dr As DataRow In drArrRecipient
                            drArrParticipant = dtParticipant.Select("GuiAnnuityID='" + dr("guiAnnuityID") + "'")
                            'create new row records to show in save click confirmation
                            drFinal = dtFinal.NewRow()

                            'checking if benefit shared or not
                            If dr("ShareBenefit").ToString().ToLower = "true" Then
                                drFinal("ShareBenefit") = dr("AnnuityType")
                            Else
                                drFinal("NotShareBenefit") = dr("AnnuityType")
                            End If
                            drFinal("AnnuityType") = drArrParticipant(0)("AnnuityType")
                            drFinal("SSN") = drRow("SSNo")
                            drFinal("Name") = drRow("FirstName").ToString().Trim() + " " + IIf(String.IsNullOrEmpty(drRow("MiddleName").ToString()), "", drRow("MiddleName") + " ") + drRow("LastName").ToString().Trim()
                            dtFinal.Rows.Add(drFinal)
                        Next
                    End If
                Next
            End If
            Return dtFinal
        Catch
            Throw
        End Try

    End Function

    Private Sub ShowConfirmationMessage(ByVal dtParticipant As DataTable, ByVal dtRecipient As DataTable, ByVal dtBeneAccount As DataTable, Optional ByVal strMessage As String = "")

        Dim dtBeneficiaryAnnuityDetail As DataTable
        Try
            dtBeneficiaryAnnuityDetail = CreatePopupMessageDatatable(dtParticipant, dtRecipient, dtBeneAccount)

            'START: PPP | 01/24/2017 | YRS-AT-3299 | Added html rows on div to accomodate new message
            trMissedRecipient.Visible = False
            trMissedRecipientEmptyRow.Visible = False
            trMultiRecipientQuestion.Visible = False
            'END: PPP | 01/24/2017 | YRS-AT-3299 | Added html rows on div to accomodate new message

            If HelperFunctions.isNonEmpty(dtBeneficiaryAnnuityDetail) Then
                'START: PPP | 01/24/2017 | YRS-AT-3299 | New confirmation in part need to be displayed
                lblMessage.Text = Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_FINAL_SAVE_PARTIAL
                lblMultiRecipientQuestion.Text = Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_FINAL_SAVE_PARTIAL_QUESTION
                'END: PPP | 01/24/2017 | YRS-AT-3299 | New confirmation in part need to be displayed
                trMultiRecipientQuestion.Visible = True 'PPP | 01/24/2017 | YRS-AT-3299 | Table row contains MESSAGE_RETIREDQDRO_FINAL_SAVE_PARTIAL_QUESTION
                If (String.IsNullOrEmpty(strMessage)) Then
                    'lblMessage.Text = Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_MULTIPLE_BENEFICIARY_CONFIRMATION 'PPP | 01/24/2017 | YRS-AT-3299 | Message is outdated
                Else
                    'START: PPP | 01/24/2017 | YRS-AT-3299 | Assigning message to lblMissedRecipient
                    'lblMessage.Text = strMessage.Trim
                    lblMissedRecipient.Text = strMessage.Trim
                    trMissedRecipientEmptyRow.Visible = True
                    trMissedRecipient.Visible = True
                    'END: PPP | 01/24/2017 | YRS-AT-3299 | Assigning message to lblMissedRecipient
                End If
                pnlBeneficiaryAnnuity.Visible = True
                gvBeneficiaryAnnuityDetails.Visible = True
                gvBeneficiaryAnnuityDetails.DataSource = dtBeneficiaryAnnuityDetail
                gvBeneficiaryAnnuityDetails.DataBind()
            Else
                pnlBeneficiaryAnnuity.Visible = False
                gvBeneficiaryAnnuityDetails.Visible = False
                'START: PPP | 01/24/2017 | YRS-AT-3299 | Changed message as per document
                'lblMessage.Text = Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_SINGLE_BENEFICIARY_CONFIRMATION
                lblMessage.Text = Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_FINAL
                'END: PPP | 01/24/2017 | YRS-AT-3299 | Changed message as per document
            End If
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "openMsgBox", "OpenConfirmationMessageBox()", True)
            Exit Sub
        Catch
            Throw
        End Try

    End Sub
    'SP 2014.06.24 - BT-2445\YRS 5.0-633 -End

    '#Region "Private Methods"

    '    '***************************************************************************************************//
    '    'Method Name               :LoadQDROBeneficieryList   Created on  : 14/04/08                        //
    '    'Created By                :Dilip Patada              Modified On :                                 //
    '    'Modified By               :                                                                        //
    '    'Modify Reason             :                                                                        //
    '    'Param Description         :Participant SSno                                                        //
    '    'Method Description        :This method is used to Load Beneficiary of the participant              //
    '    '***************************************************************************************************//
    '    Public Sub LoadQDROBeneficieryList(ByVal PersSSID As String)

    '        Dim iCount As Integer
    '        Dim txtRecptPersId As String
    '        Dim txtSSNO As String
    '        Dim txtSpouseLast As String
    '        Dim txtSpouseFirst As String
    '        Dim txtSpouseMiddle As String
    '        Dim fundStatus As String
    '        Try
    '            Dim l_dataset_QDROBeneficieyList As New DataSet
    '            l_dataset_QDROBeneficieyList = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getQDROBeneficiarySSNo(PersSSID.Trim())
    '            dtBenifAccount = CreateDataTableBenfAccount()
    '            If (l_dataset_QDROBeneficieyList.Tables(0).Rows.Count > 0) Then
    '                For iCount = 0 To l_dataset_QDROBeneficieyList.Tables(0).Rows.Count - 1
    '                    txtRecptPersId = Convert.ToString(l_dataset_QDROBeneficieyList.Tables(0).Rows(iCount).ItemArray(0))
    '                    txtSSNO = Convert.ToString(l_dataset_QDROBeneficieyList.Tables(0).Rows(iCount).ItemArray(0))
    '                    txtSpouseLast = l_dataset_QDROBeneficieyList.Tables(0).Rows(iCount).ItemArray(1)
    '                    txtSpouseFirst = l_dataset_QDROBeneficieyList.Tables(0).Rows(iCount).ItemArray(2)
    '                    txtSpouseMiddle = l_dataset_QDROBeneficieyList.Tables(0).Rows(iCount).ItemArray(3)
    '                    AddDataToTable(txtRecptPersId.Trim(), txtSSNO.Trim(), txtSpouseLast.Trim(), txtSpouseFirst.Trim(), txtSpouseMiddle.Trim(), "RQ", True, Me.string_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, "", TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), TextboxSpouseAdd1.Text.Trim(), TextboxSpouseAdd2.Text.Trim(), TextboxSpouseAdd2.Text.Trim(), TextboxSpouseCity.Text.Trim(), TextboxSpouseState.Text.Trim(), TextboxSpouseZip.Text.Trim(), TextboxSpouseCountry.Text.Trim(), dtBenifAccount)
    '                Next
    '                DatagridBenificiaryList.DataSource = dtBenifAccount
    '                DatagridBenificiaryList.DataBind()
    '                Session("dtBenifAccount") = dtBenifAccount
    '            End If
    '        Catch ex As Exception
    '            Dim l_String_Exception_Message As String
    '            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '        End Try

    '    End Sub

    '#End Region

#End Region

#Region "CheckManditory"

    '*******************************************************************************************************//
    'Event Name                :CheckManditoryExistingParticipent                 Created on  : 19/12/08    //
    'Created By                :Dilip                                             Modified On :             //
    'Modified By               :                                                                            //
    'Modify Reason             :                                                                            //
    'Param Description         :                                                                            //
    'Method Description        :This function will check the manditory fields values entered or not for     //
    '                           ExistingParticipent.                                                        //
    '*******************************************************************************************************//

    Private Function CheckMandatoryExistingParticipant() As Boolean
        Dim l_bool_Manditory As Boolean = True

        Try

            If TextBoxSSNo.Text = String.Empty Then
                l_bool_Manditory = False
                'Control_To_Focus = TextBoxSpouseSSNo
                Exit Function
            ElseIf TextBoxFirstName.Text = String.Empty Then
                l_bool_Manditory = False
                'Control_To_Focus = TextboxSpouseFirst
                Exit Function
            ElseIf TextBoxLastName.Text = String.Empty Then
                l_bool_Manditory = False
                'Control_To_Focus = TextboxSpouseLast
                Exit Function

            ElseIf AddressWebUserControl1.Address1 = "" Then
                l_bool_Manditory = False
                'Control_To_Focus = AddressWebUserControl1
                Exit Function
            ElseIf AddressWebUserControl1.City = "" Then
                l_bool_Manditory = False
                'Control_To_Focus = AddressWebUserControl1
                Exit Function
                'ElseIf AddressWebUserControl1.DropDownListStateValue = "" Then
                '    l_bool_Manditory = False
                '    'Control_To_Focus = AddressWebUserControl1
                '    Exit Function
            ElseIf AddressWebUserControl1.DropDownListCountryValue = "" Then
                l_bool_Manditory = False
                'Control_To_Focus = AddressWebUserControl1
                Exit Function
            End If
            If l_bool_Manditory = False Then
                Return False
            Else
                Return True
            End If
        Catch
            Throw
        End Try
    End Function

    '*******************************************************************************************************//
    'Event Name                :CheckManditory                                    Created on  : 06/08/08    //
    'Created By                :Kranthi                                           Modified On :             //
    'Modified By               :                                                                            //
    'Modify Reason             :                                                                            //
    'Param Description         :                                                                            //
    'Method Description        :This function will check the manditory fields values entered or not.        //
    '*******************************************************************************************************//

    Private Function CheckMandatory() As Boolean
        Dim blnMandatory As Boolean = True
        Dim l_stringErrorMsg As String = String.Empty

        Try
            AddressWebUserControl1.IsPrimary = "1"
            l_stringErrorMsg = AddressWebUserControl1.ValidateAddress()

            If TextBoxSSNo.Text = String.Empty Then
                blnMandatory = False
                'Control_To_Focus = TextBoxSpouseSSNo
                Exit Function
            ElseIf TextBoxFirstName.Text = String.Empty Then
                blnMandatory = False
                'Control_To_Focus = TextboxSpouseFirst
                Exit Function
            ElseIf TextBoxLastName.Text = String.Empty Then
                blnMandatory = False
                'Control_To_Focus = TextboxSpouseLast
                Exit Function
            ElseIf TextBoxBirthDate.Text = String.Empty Then
                blnMandatory = False
                'Control_To_Focus = TextboxBirthDate
                Exit Function
                'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript

            ElseIf l_stringErrorMsg <> "" Then
                blnMandatory = False
                'Start -- Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Commented existing code and changed the message box button from OK to Stop
                'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", l_stringErrorMsg, MessageBoxButtons.OK, False)
                'START: PPP | 01/24/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", l_stringErrorMsg, MessageBoxButtons.Stop, False)
                ShowModalPopupWithCustomMessage("QDRO", l_stringErrorMsg, "error")
                'END: PPP | 01/24/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                'End -- Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Commented existing code and changed the message box button from OK to Stop
                Exit Function

                'Commented by kranthi 101008.
                'ElseIf AddressWebUserControl1.Address1 = "" Then
                '    l_bool_Manditory = False
                '    'Control_To_Focus = AddressWebUserControl1
                '    Exit Function
                'ElseIf AddressWebUserControl1.City = "" Then
                '    l_bool_Manditory = False
                '    'Control_To_Focus = AddressWebUserControl1
                '    Exit Function
                '    'ElseIf AddressWebUserControl1.DropDownListStateValue = "" Then
                '    '    l_bool_Manditory = False
                '    '    'Control_To_Focus = AddressWebUserControl1
                '    '    Exit Function
                'ElseIf AddressWebUserControl1.DropDownListCountryValue = "" Then
                '    l_bool_Manditory = False
                '    'Control_To_Focus = AddressWebUserControl1
                '    Exit Function
            End If
            If blnMandatory = False Then
                Return False
            Else
                Return True
            End If
        Catch
            Throw
        End Try
    End Function
#End Region

    'START: PPP | 01/24/2017 | YRS-AT-3299 | QDROParticipantInformation function is not in use
    '#Region "QDROParticipantInformation"


    '    '*******************************************************************************************************//
    '    'Event Name                :QDROParticipantInformation                        Created on  : 08/08/08    //
    '    'Created By                :Kranthi                                           Modified On :             //
    '    'Modified By               :                                                                            //
    '    'Modify Reason             :                                                                            //
    '    'Param Description         :                                                                            //
    '    'Method Description        :This Method will check the Participant information on tabe change.          //
    '    '*******************************************************************************************************//

    '    Private Sub QDROParticipantInformation()
    '        Try

    '            If Me.String_Part_SSN Is Nothing Then
    '                Me.String_PersSSID = DataGridRetireeList.Items.Item(0).Cells(1).Text.Trim
    '                Me.String_FundEventID = Me.DataGridRetireeList.Items(0).Cells(6).Text().Trim
    '                Me.String_PersId = Me.DataGridRetireeList.Items(0).Cells(5).Text().Trim
    '                Me.String_Part_SSN = DataGridRetireeList.Items(0).Cells(1).Text.Trim + "-" + DataGridRetireeList.Items(0).Cells(2).Text.Trim + " " + DataGridRetireeList.Items(0).Cells(3).Text.Trim
    '                Me.String_QDRORequestID = Me.DataGridRetireeList.Items(0).Cells(7).Text().Trim


    '                'Shashi : 03 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )
    '                Headercontrol.PageTitle = "QDRO Retirees Information"
    '                Headercontrol.FundNo = DataGridRetireeList.SelectedItem.Cells(6).Text.Trim()
    '            End If

    '        Catch
    '            Throw
    '        End Try
    '    End Sub
    '#End Region
    'END: PPP | 01/24/2017 | YRS-AT-3299 | QDROParticipantInformation function is not in use

    'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
    'Private Sub DataGridRetireeList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridRetireeList.ItemDataBound
    '    e.Item.Cells(5).Visible = False 'Shashi Shekhar:2010-02-17 :Hide IsArchived Field in grid
    '    e.Item.Cells(6).Visible = False 'Shashi Shekhar:2010-12-10 :Hide Fund no Field in grid for BT-643
    '    e.Item.Cells(7).Visible = False 'Shashi Shekhar:2011-02-10 :Hide IsLock
    'End Sub
    'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required

    'SP 2014.06.24 - BT-2445\YRS 5.0-633 -Start

    Private Sub InitializePayrollDate()

        Dim dsLastPayroll As DataSet
        Dim strDate As String
        Try
            dsLastPayroll = YMCARET.YmcaBusinessObject.MonthlyPayroll.getPayrollLast()

            If HelperFunctions.isNonEmpty(dsLastPayroll) Then

                strDate = dsLastPayroll.Tables(0).Rows(0)("Month").ToString + "/01/" + dsLastPayroll.Tables(0).Rows(0)("Year").ToString
                strDate = Convert.ToDateTime(strDate).ToShortDateString()
                ViewState("PayrollDate") = strDate

            End If

        Catch
            Throw
        End Try

    End Sub
    Private Sub DataGridWorkSheet_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles DataGridWorkSheet.ItemDataBound
        Try

            If ((e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem)) Then
                'If e.Item.Cells(5).Text.Trim = "M" Or e.Item.Cells(5).Text.Trim = "C" Or (e.Item.Cells(15).Text.Trim() <> "&nbsp;" AndAlso Not (IsAnnuityTypeIncreaseOrScocialSecurityLevelling(e.Item.Cells(5).Text.Trim())) AndAlso Convert.ToDateTime(e.Item.Cells(15).Text.Trim()).Date < Convert.ToDateTime(ViewState("PayrollDate")).Date) Then
                If (Not (IsAnnuityTypeSocialSecurityLevelling(e.Item.Cells(6).Text.Trim()))) Or (e.Item.Cells(16).Text.Trim() <> "&nbsp;" AndAlso Convert.ToDateTime(e.Item.Cells(16).Text.Trim()).Date < Convert.ToDateTime(ViewState("PayrollDate")).Date) Then
                    Dim SelectCheckbox As CheckBox = CType(e.Item.Cells(2).Controls(1), CheckBox)
                    SelectCheckbox.Enabled = False
                    SelectCheckbox.Checked = False
                End If
            End If

        Catch
            Throw
        End Try

    End Sub
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            'START: PPP | 01/23/2017 | YRS-AT-3299 | Only SaveSplit is required to be called, because DocumentSave also contain the same logic
            SaveSplit()
            'If (Not String.IsNullOrEmpty(PartialBeneficiarySettleted)) Then
            '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "closeMsgBox", "CloseConfirmationMessageBox()", True)
            '    If (PartialBeneficiarySettleted.ToLower = "yes") Then
            '        PartialBeneficiarySettleted = Nothing
            '        saveSplit()
            '    ElseIf (PartialBeneficiarySettleted.ToLower = "no") Then

            '        PartialBeneficiarySettleted = Nothing
            '        DocumentSave()
            '    End If
            'End If
            'END: PPP | 01/23/2017 | YRS-AT-3299 | Only SaveSplit is required to be called, because DocumentSave also contain the same logic
        Catch ex As Exception
            HelperFunctions.LogMessage("QDRO Settlement btnOk_Click " + ex.Message)
            Dim strMessage As String
            If ex.Message = "MESSAGE_RETIREDQDRO_ChkAnnuityCurrentVal. Operation failed." Then
                strMessage = Server.UrlEncode(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ChkAnnuityCurrentVal)
            Else
                strMessage = Server.UrlEncode(ex.Message.Trim.ToString())
            End If
            Response.Redirect("ErrorPageForm.aspx?Message=" + strMessage)
        End Try
    End Sub

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Prevented btnCancel from triggering postback
    'Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    '    Try
    '        'PartialBeneficiarySettleted = Nothing ' PPP | 01/24/2017 | YRS-AT-3299 | This property is not required now, because no differentiation is needed to identify all recipients are settled or not
    '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "closeMsgBox", "CloseConfirmationMessageBox()", True)

    '    Catch ex As Exception
    '        HelperFunctions.LogMessage("QDRO Settlement btnCancel_Click " + ex.Message)
    '        Dim strExceptionMessage As String
    '        If ex.Message = "MESSAGE_RETIREDQDRO_ChkAnnuityCurrentVal. Operation failed." Then
    '            strExceptionMessage = Server.UrlEncode(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ChkAnnuityCurrentVal)
    '        Else
    '            strExceptionMessage = Server.UrlEncode(ex.Message.Trim.ToString())
    '        End If
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + strExceptionMessage)
    '    End Try

    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Prevented btnCancel from triggering postback

    'SP 2014.06.24 - BT-2445\YRS 5.0-633 -End

    Private Function GetBeneficiaryAnnuityType(ByVal paramAnnuityType As String, ByVal paramShareBenefit As Boolean, ByVal paramBitIncrease As Boolean, ByVal paramBitSSLeveling As Boolean, ByVal paramBitJointSurvivior As Boolean) As String
        Dim l_dataset_dsAnnuityTypes As New DataSet
        Dim strAnnuityType As String
        Try
            strAnnuityType = paramAnnuityType.Trim

            If paramAnnuityType.Trim = "M" Or paramAnnuityType.Trim = "C" Then
                strAnnuityType = paramAnnuityType.Trim
            End If
            If paramBitJointSurvivior = True Then
                strAnnuityType = "M"
            End If

            If paramShareBenefit = False And paramBitIncrease = True Then
                If paramAnnuityType.Trim.ToLower.Contains("c") Then
                    strAnnuityType = "C"
                Else
                    strAnnuityType = "M"
                End If
            ElseIf paramShareBenefit = True And paramBitIncrease = True Then
                strAnnuityType = "MI"
            End If

            If paramShareBenefit = False And paramBitSSLeveling = True Then
                If paramAnnuityType.Trim.ToLower.Contains("c") Then
                    strAnnuityType = "C"
                Else
                    strAnnuityType = "M"
                End If
            ElseIf paramShareBenefit = True And paramBitSSLeveling = True Then
                If paramAnnuityType.Trim = "CS" Then
                    strAnnuityType = paramAnnuityType
                Else
                    strAnnuityType = "MS"
                End If
            End If

            Return strAnnuityType
        Catch
            Throw
        End Try
    End Function
    Private Sub GetRetireeAnnuityType(ByVal paramAnnuityType As String, ByRef bitIncrease As Boolean, ByRef bitJointSurvivior As Boolean, ByRef bitSSLeveling As Boolean)
        Dim dsAnnuityTypes As New DataSet
        Try
            dsAnnuityTypes = YMCARET.YmcaBusinessObject.MetaAnnuityTypesMain.SearchAnnuityTypes(paramAnnuityType)
            If HelperFunctions.isNonEmpty(dsAnnuityTypes) Then
                bitIncrease = Convert.ToBoolean(dsAnnuityTypes.Tables(0).Rows(0)("Increasing"))
                bitJointSurvivior = Convert.ToBoolean(dsAnnuityTypes.Tables(0).Rows(0)("Joint Survivor"))
                bitSSLeveling = Convert.ToBoolean(dsAnnuityTypes.Tables(0).Rows(0)("Ssleveling"))
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Function IsAnnuityTypeSocialSecurityLevelling(ByVal paramAnnuityType As String) As Boolean
        Dim dsAnnuityTypes As New DataSet
        Dim bitIncrease As Boolean
        Dim bitSSLeveling As Boolean

        Try
            dsAnnuityTypes = YMCARET.YmcaBusinessObject.MetaAnnuityTypesMain.SearchAnnuityTypes(paramAnnuityType)
            If HelperFunctions.isNonEmpty(dsAnnuityTypes) Then
                bitIncrease = Convert.ToBoolean(dsAnnuityTypes.Tables(0).Rows(0)("Increasing"))
                bitSSLeveling = Convert.ToBoolean(dsAnnuityTypes.Tables(0).Rows(0)("Ssleveling"))
            Else
                Throw New Exception("No annuity type exist in MetaAnnuityType table")
            End If
        Catch
            Throw
        End Try

        If (bitIncrease Or bitSSLeveling) Then
            Return True
        Else
            Return False
        End If

    End Function
    Private Function IsAnnuityTypeScocialSecurityLevelling(ByVal paramAnnuityType As String) As Boolean
        Dim dsAnnuityTypes As New DataSet
        Dim bitSSLeveling As Boolean
        Try
            'START: PPP | 01/31/2017 | YRS-AT-3299 | Tracing result
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->IsAnnuityTypeScocialSecurityLevelling", "START")
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->IsAnnuityTypeScocialSecurityLevelling", String.Format("Checking SSLeveling for '{0}'", paramAnnuityType))
            'END: PPP | 01/31/2017 | YRS-AT-3299 | Tracing result

            dsAnnuityTypes = YMCARET.YmcaBusinessObject.MetaAnnuityTypesMain.SearchAnnuityTypes(paramAnnuityType)
            If HelperFunctions.isNonEmpty(dsAnnuityTypes) Then
                bitSSLeveling = Convert.ToBoolean(dsAnnuityTypes.Tables(0).Rows(0)("Ssleveling"))
                'START: PPP | 01/31/2017 | YRS-AT-3299 | Tracing result
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->IsAnnuityTypeScocialSecurityLevelling", String.Format("Checked SSLeveling for '{0}' and Value is '{1}'", Convert.ToString(dsAnnuityTypes.Tables(0).Rows(0)("Annuity Type")), bitSSLeveling.ToString())) 'Annuity Type
                'END: PPP | 01/31/2017 | YRS-AT-3299 | Tracing result
            Else
                Throw New Exception("No annuity type exist in MetaAnnuityType table")
            End If
            Return bitSSLeveling 'PPP | 01/31/2017 | YRS-AT-3299 | Returning the result from here instead of checking the value outside try block
        Catch
            Throw
            'START: PPP | 01/31/2017 | YRS-AT-3299 | Added Finally block to complete the tracing 
        Finally
            dsAnnuityTypes = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->IsAnnuityTypeScocialSecurityLevelling", "END")
            'END: PPP | 01/31/2017 | YRS-AT-3299 | Added Finally block to complete the tracing 
        End Try

        'START: PPP | 01/31/2017 | YRS-AT-3299 | Not required to check it outside
        'If (bitSSLeveling) Then
        '    Return True
        'Else
        '    Return False
        'End If
        'END: PPP | 01/31/2017 | YRS-AT-3299 | Not required to check it outside

    End Function
    Private Function GetEffectiveCurrentPaymentForAnnuity(dr As DataRow, AnnuityType As String, ByVal ShareBenefit As Boolean) As Decimal
        Dim bitIncrease, bitJointSurvivior, bitSSLeveling As Boolean
        GetRetireeAnnuityType(AnnuityType, bitIncrease, bitJointSurvivior, bitSSLeveling)
        Try
            If (ShareBenefit = True) Then
                Return dr("CurrentPayment")
            ElseIf (bitSSLeveling = True) Then
                Return dr("EmpPreTaxCurrentPayment") + dr("EmpPostTaxCurrentPayment") + dr("YmcaPreTaxCurrentPayment") + dr("SSLevelingAmt") - dr("SSReductionAmt")
            Else
                Return dr("CurrentPayment")
            End If
        Catch
            Throw
        End Try
    End Function

    Private Sub CalculateSSLevelingWithoutShareBenefit(ByRef drRecipient As DataRow, ByRef drOrigParticipant As DataRow)
        Try
            Dim currentPaymentAfterReduction As Decimal = GetEffectiveCurrentPaymentForAnnuity(drOrigParticipant, drOrigParticipant("AnnuityType"), drRecipient("ShareBenefit"))

            Dim currentProratePercentage As Decimal = (drRecipient("CurrentPayment") / drOrigParticipant("CurrentPayment")) * 100

            Dim currentPercentageForSSLeveling As Decimal = (drRecipient("CurrentPayment") / currentPaymentAfterReduction) * 100

            'set the receipent current payem
            drRecipient("CurrentPayment") = Math.Round(((currentPaymentAfterReduction * currentPercentageForSSLeveling) / 100), 2, MidpointRounding.AwayFromZero)

            Dim splitPercentageForSSLAnnuity As Decimal = (drRecipient("CurrentPayment") / (drOrigParticipant("EmpPreTaxCurrentPayment") + drOrigParticipant("EmpPostTaxCurrentPayment") + drOrigParticipant("YmcaPreTaxCurrentPayment"))) * 100

            drRecipient("EmpPreTaxCurrentPayment") = Math.Round((Convert.ToDecimal(drOrigParticipant("EmpPreTaxCurrentPayment") * splitPercentageForSSLAnnuity) / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("EmpPostTaxCurrentPayment") = Math.Round((Convert.ToDecimal(drOrigParticipant("EmpPostTaxCurrentPayment") * splitPercentageForSSLAnnuity) / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("YmcaPreTaxCurrentPayment") = Math.Round((Convert.ToDecimal(drOrigParticipant("YmcaPreTaxCurrentPayment") * splitPercentageForSSLAnnuity) / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("SSLevelingAmt") = 0
            drRecipient("SSReductionAmt") = 0
            drRecipient("SSReductionEftDate") = DBNull.Value
            drRecipient("EmpPreTaxRemainingReserves") = Math.Round((drOrigParticipant("EmpPreTaxRemainingReserves") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("EmpPostTaxRemainingReserves") = Math.Round((drOrigParticipant("EmpPostTaxRemainingReserves") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("YmcapreTaxRemainingReserves") = Math.Round((drOrigParticipant("YmcapreTaxRemainingReserves") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)
        Catch
            Throw
        End Try

    End Sub
    Private Sub CalculateSSLevelingWithShareBenefit(ByRef drRecipient As DataRow, ByRef drOrigParticpant As DataRow)

        Try

            Dim currentProratePercentage As Decimal = (drRecipient("CurrentPayment") / drOrigParticpant("CurrentPayment")) * 100
            ' drPartRemaining("CurrentPayment") -= drReceipant("CurrentPayment")

            drRecipient("EmpPreTaxCurrentPayment") = Math.Round((drOrigParticpant("EmpPreTaxCurrentPayment") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("EmpPostTaxCurrentPayment") = Math.Round((drOrigParticpant("EmpPostTaxCurrentPayment") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("YmcaPreTaxCurrentPayment") = Math.Round((drOrigParticpant("YmcaPreTaxCurrentPayment") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)

            drRecipient("SSLevelingAmt") = Math.Round((drOrigParticpant("SSLevelingAmt") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("SSReductionAmt") = Math.Round((drOrigParticpant("SSReductionAmt") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)

            drRecipient("EmpPreTaxRemainingReserves") = Math.Round((drOrigParticpant("EmpPreTaxRemainingReserves") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("EmpPostTaxRemainingReserves") = Math.Round((drOrigParticpant("EmpPostTaxRemainingReserves") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)
            drRecipient("YmcapreTaxRemainingReserves") = Math.Round((drOrigParticpant("YmcapreTaxRemainingReserves") * currentProratePercentage / 100), 2, MidpointRounding.AwayFromZero)
        Catch
            Throw
        End Try
    End Sub

    Private Function GetEffectiveCurrentPaymentForAnnuity(dr As DataRow) As Decimal

        Return GetEffectiveCurrentPaymentForAnnuity(dr, dr("OriginalAnnuityType"), dr("ShareBenefit"))
    End Function

    Private Function EnsureAnnuityComponentsAreNonNegative(dr As DataRow)
        Try
            'Ensure no components are negative
            If (Convert.ToDecimal(dr("CurrentPayment")) < 0) Then
                Return False
            End If
            If (Convert.ToDecimal(dr("SSReductionAmt")) < 0) Then
                Return False
            End If
            If (Convert.ToDecimal(dr("SSLevelingAmt")) < 0) Then
                Return False
            End If
            If (Convert.ToDecimal(dr("EmpPreTaxCurrentPayment")) < 0) Then
                Return False
            End If
            If (Convert.ToDecimal(dr("EmpPostTaxCurrentPayment")) < 0) Then
                Return False
            End If
            If (Convert.ToDecimal(dr("YmcaPreTaxCurrentPayment")) < 0) Then
                Return False
            End If
        Catch
            Throw
        End Try
        Return True
    End Function

    Private Function IsAnnuityNegativeAfterSSLevelling(dr As DataRow) As Boolean
        Dim bitIncrease, bitJointSurvivior, bitSSLeveling As Boolean
        Try
            GetRetireeAnnuityType(dr("AnnuityType"), bitIncrease, bitJointSurvivior, bitSSLeveling)
            If bitSSLeveling = False Then Return False
            If dr("EmpPreTaxCurrentPayment") + dr("EmpPostTaxCurrentPayment") + dr("YmcaPreTaxCurrentPayment") + dr("SSLevelingAmt") - dr("SSReductionAmt") < 0 Then Return True
        Catch
            Throw
        End Try
        Return False
    End Function

    Private Function PerformPercentageValidation(dtPartOriginalAnnuity As DataTable, dtRecptAccount As DataTable, dtCurrentBeneficiary As DataTable) As String
        Try
            For Each dr As DataRow In dtCurrentBeneficiary.Rows
                Dim strFilterExpression As String = "guiAnnuityID ='" + dr("guiAnnuityID") + "'"
                Dim isContainFixed As Boolean = IIf(dtRecptAccount.Select("IsSplitPercentage='false'").Count() > 0, True, False)
                Dim drParticipant As DataRow() = dtPartOriginalAnnuity.Select(strFilterExpression)
                Dim originalAnnuityAmount As Decimal = GetEffectiveCurrentPaymentForAnnuity(drParticipant(0), drParticipant(0)("AnnuityType"), dr("ShareBenefit"))
                Dim used As Decimal
                If isContainFixed Then
                    Dim result As Object = dtRecptAccount.Compute("Sum(SplitAmount)", strFilterExpression)
                    If IsDBNull(result) = False Then
                        used = Math.Ceiling((Convert.ToDecimal(dtRecptAccount.Compute("Sum(CurrentPayment)", strFilterExpression)) * 1000 / originalAnnuityAmount)) / 100
                    End If
                Else
                    If (HelperFunctions.isNonEmpty(dtRecptAccount)) Then
                        Dim result As Object = dtRecptAccount.Compute("Sum(SplitAmount)", strFilterExpression)
                        If IsDBNull(result) = False Then
                            used = Convert.ToDecimal(dtRecptAccount.Compute("Sum(SplitAmount)", strFilterExpression))
                        End If
                    End If

                End If
                If used + dr("SplitAmount") > 100 Then
                    Return String.Format(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_SPLIT_PERCENTAGE_VALIDATION, (100 - used).ToString(), dr("PlanType")) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
                End If

            Next
            Return ""
        Catch
            Throw
        End Try
    End Function

    Private Function PerformFixedAmountValidation(dtPartOriginalAnnuity As DataTable, dtRecptAccount As DataTable, dtCurrentBeneficiary As DataTable) As String
        Dim totalAvailable As Decimal = 0
        Try
            For Each dr As DataRow In dtCurrentBeneficiary.Rows
                Dim strFilterExpression As String = "guiAnnuityID ='" + dr("guiAnnuityID") + "'"
                Dim drParticipant As DataRow() = dtPartOriginalAnnuity.Select(strFilterExpression)
                Dim originalAnnuityAmount As Decimal = GetEffectiveCurrentPaymentForAnnuity(drParticipant(0), drParticipant(0)("AnnuityType"), dr("ShareBenefit"))
                Dim used As Decimal = 0
                If (HelperFunctions.isNonEmpty(dtRecptAccount)) Then

                    Dim result As Object = dtRecptAccount.Compute("Sum(CurrentPayment)", strFilterExpression)
                    If IsDBNull(result) = False Then
                        used = Convert.ToDecimal(result)
                    End If

                End If
                totalAvailable += originalAnnuityAmount - used
            Next
            If totalAvailable - dtCurrentBeneficiary.Rows(0)("SplitAmount") < 0 Then
                Return String.Format(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_AMOUNT_VALIDATION, totalAvailable.ToString()) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
            End If
        Catch
            Throw
        End Try
        Return ""
    End Function

    Private Function ValidateSplitParameters(dtPartOriginalAnnuity As DataTable, dtRecptAccount As DataTable, dtCurrentBeneficiary As DataTable) As String
        Try
            Dim bHasBeneficiarySelectedPercent As Boolean = (dtCurrentBeneficiary.Rows(0)("IsSplitPercentage").ToString.ToLower = "true")
            If (bHasBeneficiarySelectedPercent) Then
                Return PerformPercentageValidation(dtPartOriginalAnnuity, dtRecptAccount, dtCurrentBeneficiary)
            Else
                Return PerformFixedAmountValidation(dtPartOriginalAnnuity, dtRecptAccount, dtCurrentBeneficiary)
            End If
        Catch
            Throw
        End Try
        Return ""


    End Function

    Private Sub AdjustSSLevelingAmountAfterAdjustingRemainingBalance(drPartRemaining As DataRow, ByRef drCurrentRcpt As DataRow)

        Dim bitIncrease, bitSSLeveling, bitJointSurvivior As Boolean
        Try
            'SP 2014.07.23
            GetRetireeAnnuityType(drCurrentRcpt("OriginalAnnuityType"), bitIncrease, bitSSLeveling, bitJointSurvivior)
            If (drCurrentRcpt("shareBenefit").ToString.ToLower() = "false" Or bitSSLeveling = False) Then Exit Sub
            'SP 2014.07.23

            If drPartRemaining("SSReductionAmt") < 0 Or drPartRemaining("CurrentPayment") = 0 Then
                drCurrentRcpt("SSReductionAmt") += drPartRemaining("SSReductionAmt")
                drPartRemaining("SSReductionAmt") = 0
            End If
            If drPartRemaining("SSLevelingAmt") < 0 Or drPartRemaining("CurrentPayment") = 0 Then
                drCurrentRcpt("SSLevelingAmt") += drPartRemaining("SSLevelingAmt")
                drPartRemaining("SSLevelingAmt") = 0
            End If

            Dim CurrentPartRemainingEffectivePayment As Decimal = GetEffectiveCurrentPaymentForAnnuity(drPartRemaining, drPartRemaining("AnnuityType"), drCurrentRcpt("ShareBenefit"))
            If CurrentPartRemainingEffectivePayment < 0 Then
                'Adjust SSL components so that remaining annuity is +ve
                'First try to remove from SSReductionAmt. If reduction amount <= 0 then add into SSLevelingAmt
                If drPartRemaining("SSReductionAmt") > 0 Then
                    Dim amountToAdjust As Decimal
                    If drPartRemaining("SSReductionAmt") > CurrentPartRemainingEffectivePayment Then
                        amountToAdjust = CurrentPartRemainingEffectivePayment
                        CurrentPartRemainingEffectivePayment = 0
                    Else
                        amountToAdjust = drPartRemaining("SSReductionAmt")
                        CurrentPartRemainingEffectivePayment -= amountToAdjust
                    End If

                    drCurrentRcpt("SSReductionAmt") -= amountToAdjust
                    drPartRemaining("SSReductionAmt") += amountToAdjust
                End If

                'If drPartRemaining("SSLevelingAmt") > 0 Then
                '    Dim amountToAdjust As Decimal
                '    If drPartRemaining("SSLevelingAmt") > CurrentPartRemainingEffectivePayment Then
                '        amountToAdjust = CurrentPartRemainingEffectivePayment
                '        CurrentPartRemainingEffectivePayment = 0
                '    Else
                '        amountToAdjust = drPartRemaining("SSLevelingAmt")
                '        CurrentPartRemainingEffectivePayment -= amountToAdjust
                '    End If

                '    drCurrentRcpt("SSLevelingAmt") -= amountToAdjust
                '    drPartRemaining("SSLevelingAmt") += amountToAdjust
                'End If
            End If

            If GetEffectiveCurrentPaymentForAnnuity(drCurrentRcpt) < 0 Then
                'Adjust SSL components so that remaining annuity is +ve
                'First try to add to SSReductionAmt. If remaining reduction amount <= 0 then remove from SSLevelingAmt
                If drPartRemaining("SSReductionAmt") > 0 Then
                    Dim amountToAdjust As Decimal
                    If drPartRemaining("SSReductionAmt") > CurrentPartRemainingEffectivePayment Then
                        amountToAdjust = CurrentPartRemainingEffectivePayment
                        CurrentPartRemainingEffectivePayment = 0
                    Else
                        amountToAdjust = drPartRemaining("SSReductionAmt")
                        CurrentPartRemainingEffectivePayment -= amountToAdjust
                    End If

                    drCurrentRcpt("SSReductionAmt") -= amountToAdjust
                    drPartRemaining("SSReductionAmt") += amountToAdjust
                End If

                'If drPartRemaining("SSLevelingAmt") > 0 Then
                '    Dim amountToAdjust As Decimal
                '    If drPartRemaining("SSLevelingAmt") > CurrentPartRemainingEffectivePayment Then
                '        amountToAdjust = CurrentPartRemainingEffectivePayment
                '        CurrentPartRemainingEffectivePayment = 0
                '    Else
                '        amountToAdjust = drPartRemaining("SSLevelingAmt")
                '        CurrentPartRemainingEffectivePayment -= amountToAdjust
                '    End If

                '    drCurrentRcpt("SSLevelingAmt") -= amountToAdjust
                '    drPartRemaining("SSLevelingAmt") += amountToAdjust
                'End If

            End If
        Catch
            Throw
        End Try
        'If (drPartRemaining("CurrentPayment") = 0 And drPartRemaining("SSReductionAmt") <> 0) Then
        '    drCurrentRcpt("SSReductionAmt") += drPartRemaining("SSReductionAmt")
        '    drPartRemaining("SSReductionAmt") = 0
        'End If
        'If (drPartRemaining("CurrentPayment") = 0 And drPartRemaining("SSLevelingAmt") <> 0) Then
        '    drCurrentRcpt("SSLevelingAmt") += drPartRemaining("SSLevelingAmt")
        '    drPartRemaining("SSLevelingAmt") = 0
        'End If
    End Sub

    Private Function AdjustValueFrom(ByVal dr As Object, ByVal drPartRemaining As DataRow, arr As String, ByRef amountToAdjust As Double) As Integer
        Try
            If drPartRemaining(arr) - amountToAdjust < 0 Then 'Trying to adjust more than what is available
                amountToAdjust -= drPartRemaining(arr)
                dr(arr) += drPartRemaining(arr)
                drPartRemaining(arr) = 0
            End If
            If drPartRemaining(arr) - amountToAdjust >= 0 Then
                drPartRemaining(arr) -= amountToAdjust
                dr(arr) += amountToAdjust
                amountToAdjust = 0
            End If
        Catch
            Throw
        End Try
    End Function
    'START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    ' Adding the recipient Annuities 
    'If recipient Annuities not existing of the Split Type for the Person then added to datatable
    Private Function AddRecipientAnnuitiesDetails(ByVal dtExistingRecipientData As DataTable, ByVal dtRecipientData As DataTable, ByVal strPlanType As String) As DataTable
        Dim dtReturnValue As DataTable
        Dim drCurrentSplit As DataRow
        Dim drArryRecipient As DataRow()
        Dim drArryRecipients As DataRow()
        Dim drRecipientAnnuities As DataRow
        Try
            If Not dtExistingRecipientData Is Nothing Then
                If HelperFunctions.isNonEmpty(dtExistingRecipientData) Then
                    drArryRecipient = dtExistingRecipientData.Select("splitType ='" & strPlanType & "'AND RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")
                    If (drArryRecipient.Length = 0) Then
                        drArryRecipients = dtRecipientData.Select("splitType ='" & strPlanType & "'AND RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")
                        ' For Each drRecipientAnnuities In drArryRecipients
                        For Each drRecp As DataRow In drArryRecipients
                            drCurrentSplit = dtExistingRecipientData.NewRow
                            drCurrentSplit.ItemArray = drRecp.ItemArray
                            dtExistingRecipientData.Rows.Add(drCurrentSplit)
                        Next
                    End If
                    dtReturnValue = dtExistingRecipientData.Copy()
                End If
            End If
        Catch
            Throw
        End Try
        Return dtReturnValue
    End Function
    'Updating the Recipient Annuities balance in case adjustment
    Private Function UpdateRecipientAnnuitiesDetails(ByVal dtExistingRecipientData As DataTable, ByVal dtRecipientData As DataTable, ByVal strPlanType As String) As DataTable
        Dim dtReturnValue As DataTable
        Dim drArryRecipient As DataRow()
        Try
            drArryRecipient = dtExistingRecipientData.Select("splitType ='" & strPlanType & "'AND RecipientPersonID ='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")
            For Each drRowRecAccountbalance In drArryRecipient
                For Each drRowRecpAccounttemp In dtRecipientData.Rows
                    If drRowRecAccountbalance("guiAnnuityID") = drRowRecpAccounttemp("guiAnnuityID") And drRowRecAccountbalance("RecipientPersonID") = drRowRecpAccounttemp("RecipientPersonID") And drRowRecAccountbalance("splitType") = drRowRecpAccounttemp("splitType") Then
                        drRowRecAccountbalance("SSLevelingAmt") = drRowRecpAccounttemp("SSLevelingAmt")
                        drRowRecAccountbalance("SSReductionAmt") = drRowRecpAccounttemp("SSReductionAmt")
                        drRowRecAccountbalance("CurrentPayment") = drRowRecpAccounttemp("CurrentPayment")
                        drRowRecAccountbalance("EmpPreTaxCurrentPayment") = drRowRecpAccounttemp("EmpPreTaxCurrentPayment")
                        drRowRecAccountbalance("EmpPostTaxCurrentPayment") = drRowRecpAccounttemp("EmpPostTaxCurrentPayment")
                        drRowRecAccountbalance("YmcaPreTaxCurrentPayment") = drRowRecpAccounttemp("YmcaPreTaxCurrentPayment")
                        drRowRecAccountbalance("EmpPreTaxRemainingReserves") = drRowRecpAccounttemp("EmpPreTaxRemainingReserves")
                        drRowRecAccountbalance("EmpPostTaxRemainingReserves") = drRowRecpAccounttemp("EmpPostTaxRemainingReserves")
                        drRowRecAccountbalance("YmcapreTaxRemainingReserves") = drRowRecpAccounttemp("YmcapreTaxRemainingReserves")
                    End If
                Next
            Next
            dtExistingRecipientData.AcceptChanges()
            dtReturnValue = dtExistingRecipientData.Copy()
        Catch
            Throw
        End Try
        Return dtReturnValue
    End Function
    'Adding Participant Annuities
    Private Function AddParticipantAnnuitiesDetails(ByVal dtExistingParticipantData As DataTable, ByVal dtParticipantData As DataTable) As DataTable
        Dim dtReturnValue As DataTable
        Dim drCurrentSplit As DataRow
        Dim drArryParticipant As DataRow()
        Dim drParticipant As DataRow
        Try
            If Not dtExistingParticipantData Is Nothing Then
                dtReturnValue = dtParticipantData.Clone()
                If HelperFunctions.isNonEmpty(dtExistingParticipantData) Then
                    For Each drParticipant In dtParticipantData.Rows
                        drArryParticipant = dtExistingParticipantData.Select("guiAnnuityID='" & drParticipant("guiAnnuityID") & "'")
                        If (drArryParticipant.Length = 0) Then
                            drCurrentSplit = dtReturnValue.NewRow
                            dtReturnValue.ImportRow(drParticipant)
                        Else
                            For dtRowCount = 0 To drArryParticipant.Length - 1
                                dtReturnValue.ImportRow(drArryParticipant(dtRowCount))
                            Next
                        End If
                    Next
                    dtReturnValue.AcceptChanges()
                Else
                    dtReturnValue = dtParticipantData.Copy()
                End If
                'Else
                '    dtReturnValue = dtParticipantData.Copy()
            End If
        Catch
            Throw
        End Try
        Return dtReturnValue
    End Function
    'Adding/Updating the participant Annuities Details
    'If participant Annuities is not there in the datatable then Insert of new rows will be working otherwise updation will work
    'updation Based on the Annuities 
    Private Function AddUpdateRecipientAnnuitiesCurrentSplitDetails(ByVal dtExistingRecipicentData As DataTable, ByVal dtRecipientAnnuitiesData As DataTable) As DataTable
        Dim dtReturnValue As DataTable
        Dim drCurrentSplit As DataRow
        Dim drArryParticipant As DataRow()
        Dim drParticipant As DataRow
        Try
            If Not dtExistingRecipicentData Is Nothing Then
                If HelperFunctions.isNonEmpty(dtExistingRecipicentData) Then
                    For Each drRowPartAccountbalance In dtExistingRecipicentData.Rows
                        For Each drRowParticipantAccounttemp In dtRecipientAnnuitiesData.Rows
                            If drRowPartAccountbalance("guiAnnuityID") = drRowParticipantAccounttemp("guiAnnuityID") Then
                                drRowPartAccountbalance("SSLevelingAmt") = drRowParticipantAccounttemp("SSLevelingAmt")
                                drRowPartAccountbalance("SSReductionAmt") = drRowParticipantAccounttemp("SSReductionAmt")
                                drRowPartAccountbalance("CurrentPayment") = drRowParticipantAccounttemp("CurrentPayment")
                                drRowPartAccountbalance("EmpPreTaxCurrentPayment") = drRowParticipantAccounttemp("EmpPreTaxCurrentPayment")
                                drRowPartAccountbalance("EmpPostTaxCurrentPayment") = drRowParticipantAccounttemp("EmpPostTaxCurrentPayment")
                                drRowPartAccountbalance("YmcaPreTaxCurrentPayment") = drRowParticipantAccounttemp("YmcaPreTaxCurrentPayment")
                                drRowPartAccountbalance("EmpPreTaxRemainingReserves") = drRowParticipantAccounttemp("EmpPreTaxRemainingReserves")
                                drRowPartAccountbalance("EmpPostTaxRemainingReserves") = drRowParticipantAccounttemp("EmpPostTaxRemainingReserves")
                                drRowPartAccountbalance("YmcapreTaxRemainingReserves") = drRowParticipantAccounttemp("YmcapreTaxRemainingReserves")
                            End If
                        Next
                    Next
                    dtExistingRecipicentData.AcceptChanges()
                End If
                dtReturnValue = dtExistingRecipicentData.Copy()

                For Each drParticipant In dtRecipientAnnuitiesData.Rows
                    drArryParticipant = dtExistingRecipicentData.Select("guiAnnuityID='" & drParticipant("guiAnnuityID") & "'")
                    If (drArryParticipant.Length = 0) Then
                        drCurrentSplit = dtReturnValue.NewRow
                        dtReturnValue.ImportRow(drParticipant)
                    End If
                Next
                dtReturnValue.AcceptChanges()
            End If
        Catch
            Throw
        End Try
        Return dtReturnValue
    End Function
    Private Function AddUpdateParticipantAnnuitiesDetails(ByVal dtExistingParticipantData As DataTable, ByVal dtParticipantData As DataTable) As DataTable
        Dim dtReturnValue As DataTable
        Dim drCurrentSplit As DataRow
        Dim drArryParticpant As DataRow()
        Dim drParticipant As DataRow
        Try
            If Not dtExistingParticipantData Is Nothing Then
                If HelperFunctions.isNonEmpty(dtExistingParticipantData) Then
                    For Each drRowPartAccountbalance In dtExistingParticipantData.Rows
                        For Each drRowParticipantAccounttemp In dtParticipantData.Rows
                            If drRowPartAccountbalance("guiAnnuityID") = drRowParticipantAccounttemp("guiAnnuityID") Then
                                drRowPartAccountbalance("SSLevelingAmt") = drRowParticipantAccounttemp("SSLevelingAmt")
                                drRowPartAccountbalance("SSReductionAmt") = drRowParticipantAccounttemp("SSReductionAmt")
                                drRowPartAccountbalance("CurrentPayment") = drRowParticipantAccounttemp("CurrentPayment")
                                drRowPartAccountbalance("EmpPreTaxCurrentPayment") = drRowParticipantAccounttemp("EmpPreTaxCurrentPayment")
                                drRowPartAccountbalance("EmpPostTaxCurrentPayment") = drRowParticipantAccounttemp("EmpPostTaxCurrentPayment")
                                drRowPartAccountbalance("YmcaPreTaxCurrentPayment") = drRowParticipantAccounttemp("YmcaPreTaxCurrentPayment")
                                drRowPartAccountbalance("EmpPreTaxRemainingReserves") = drRowParticipantAccounttemp("EmpPreTaxRemainingReserves")
                                drRowPartAccountbalance("EmpPostTaxRemainingReserves") = drRowParticipantAccounttemp("EmpPostTaxRemainingReserves")
                                drRowPartAccountbalance("YmcapreTaxRemainingReserves") = drRowParticipantAccounttemp("YmcapreTaxRemainingReserves")
                            End If
                        Next
                    Next
                    dtExistingParticipantData.AcceptChanges()
                End If
                dtReturnValue = dtExistingParticipantData.Copy()
                For Each drParticipant In dtParticipantData.Rows
                    drArryParticpant = dtExistingParticipantData.Select("guiAnnuityID='" & drParticipant("guiAnnuityID") & "'")
                    If (drArryParticpant.Length = 0) Then
                        drCurrentSplit = dtReturnValue.NewRow
                        dtReturnValue.ImportRow(drParticipant)
                    End If
                Next
                dtReturnValue.AcceptChanges()
            End If
        Catch
            Throw
        End Try
        Return dtReturnValue
    End Function

    ''This Event will works when click on Split Link Buttons
    'Private Sub LnkSplitTypes_Click(sender As Object, e As EventArgs) Handles lnkButtonBothPlans.Click, lnkButtonRetirementPlan.Click, lnkButtonSavingsPlan.Click
    '    Try
    '        lblCurrentSplitOptions.Visible = False
    '        Dim LnkButtonSplitTypes As LinkButton = DirectCast(sender, LinkButton)
    '        Dim strLinkButtonId As String = LnkButtonSplitTypes.ID
    '        Select Case strLinkButtonId
    '            Case "lnkButtonBothPlans"
    '                GetAnnuitiesBalancesDetails(EnumPlanTypes.BOTH.ToString())
    '            Case "lnkButtonRetirementPlan"
    '                GetAnnuitiesBalancesDetails(EnumPlanTypes.RETIREMENT.ToString())
    '            Case "lnkButtonSavingsPlan"
    '                GetAnnuitiesBalancesDetails(EnumPlanTypes.SAVINGS.ToString())
    '        End Select
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("LnkSplitTypes_Click", ex)
    '        l_String_Exception_Message = ex.Message.Trim.ToString()
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub

    'This method is used get the recipient Annuities to display in the grid
    Private Function GetRecipientAccountBySplitTypes(ByVal dtRecipientAnnuitiesDetails As DataTable, ByVal strPlanType As String) As DataTable
        Dim dtReturnValue As DataTable
        If Not dtRecipientAnnuitiesDetails Is Nothing Then
            If HelperFunctions.isNonEmpty(dtRecipientAnnuitiesDetails) Then
                dtReturnValue = dtRecipientAnnuitiesDetails.Clone()
                Dim dtrow2 As DataRow()
                dtrow2 = dtRecipientAnnuitiesDetails.Select("splitType ='" + strPlanType + "' AND RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedValue.ToString().Trim() & "'")
                For dtRowCount = 0 To dtrow2.Length - 1
                    dtReturnValue.ImportRow(dtrow2(dtRowCount))
                Next
            End If
        End If
        Return dtReturnValue
    End Function
    'This is used get the Recipient Annuities based on the Person to generate Html table for Displaying Summary of Recipient Annuites
    Private Function GetRecipientAnnutiesDetailsForHtmlTable(ByVal dtRecipientAnnutiesDetails As DataTable, ByVal RecipientPersonID As String) As DataTable
        Dim dtReturnValue As DataTable
        If Not dtRecipientAnnutiesDetails Is Nothing Then
            If HelperFunctions.isNonEmpty(dtRecipientAnnutiesDetails) Then
                dtReturnValue = dtRecipientAnnutiesDetails.Clone()
                Dim dtrow2 As DataRow()
                dtrow2 = dtRecipientAnnutiesDetails.Select("RecipientPersonID ='" & RecipientPersonID & "'")
                For dtRowCount = 0 To dtrow2.Length - 1
                    dtReturnValue.ImportRow(dtrow2(dtRowCount))
                Next
            End If
        End If
        Return dtReturnValue
    End Function
    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Loading the martial status and gender.
    Private Sub PopulateMaritalStatusDropDownList()
        Dim dsMaritalTypes As DataSet
        dsMaritalTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.MaritalTypes(0)
        Dim drMaritalTypes As DataRow = dsMaritalTypes.Tables(0).NewRow
        drMaritalTypes("Description") = "-Select-"
        drMaritalTypes("Code") = "SEL"
        dsMaritalTypes.Tables(0).Rows.InsertAt(drMaritalTypes, 0)
        If HelperFunctions.isNonEmpty(dsMaritalTypes) Then
            Me.DropDownListMaritalStatus.DataSource = dsMaritalTypes.Tables(0)
            Me.DropDownListMaritalStatus.DataTextField = "Description"
            Me.DropDownListMaritalStatus.DataValueField = "Code"
            Me.DropDownListMaritalStatus.DataBind()
            'Start - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Checking if value exists in dropdown before assigning default selected value
            If Not Me.DropDownListMaritalStatus.Items.FindByValue("D") Is Nothing Then
                Me.DropDownListMaritalStatus.SelectedValue = "D"
            Else
                Me.DropDownListMaritalStatus.SelectedValue = "SEL"
            End If
            'End - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Checking if value exists in dropdown before assigning default selected value
        End If
    End Sub

    Private Sub PopulateGenderDropDownList()
        Dim dsGenderTypes As DataSet
        dsGenderTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.GenderTypes()
        Dim drGenderTypes As DataRow = dsGenderTypes.Tables(0).NewRow
        drGenderTypes("Description") = "-Select-"
        drGenderTypes("Code") = "SEL"
        dsGenderTypes.Tables(0).Rows.InsertAt(drGenderTypes, 0)
        If HelperFunctions.isNonEmpty(dsGenderTypes) Then
            Me.DropDownListGender.DataSource = dsGenderTypes.Tables(0)
            Me.DropDownListGender.DataTextField = "Description"
            Me.DropDownListGender.DataValueField = "Code"
            Me.DropDownListGender.DataBind()
        End If
    End Sub
    'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Loading the martial status and gender.
    'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Enabling and diabling martial status and gender.
    Private Sub ManageEditableControls(ByVal bnFlag As Boolean)
        DropDownListGender.Enabled = bnFlag
        DropDownListMaritalStatus.Enabled = bnFlag
    End Sub
    'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Enabling and diabling martial status and gender.

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Not using EnabledAndVisibleControls
    ''START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    ''In-Visible/Visible and enable/Disable
    'Private Sub EnabledAndVisibleControls(ByVal blnRadioButtonListSplitAmtType As Boolean, ByVal blnRadioButtonListSplitAmtTypeEnable As Boolean, ByVal blnLinkButtonBothPlans As Boolean, ByVal blnLinkButtonRetPlan As Boolean,
    '                              ByVal blnLinkButtonSavePlan As Boolean, ByVal blnLabelBothPlans As Boolean, ByVal blnRetirementPlan As Boolean, ByVal blnLabelSavingsPlan As Boolean,
    '                              ByVal blnTextBoxAmountWorkSheet As Boolean, ByVal blnTextBoxPercentageWorkSheet As Boolean, ByVal blnTextBoxAmountWorkSheetEnable As Boolean,
    '                              ByVal blnTextBoxPercentageWorkSheetEnable As Boolean, ByVal blnBothPlansPerOrAmt As Boolean,
    '                              ByVal blnButtonSplit As Boolean, ByVal blnButtonReset As Boolean, ByVal blnButtonAdjust As Boolean, ByVal blnButtonShowBalance As Boolean, ByVal blnButtonDocumentSave As Boolean, ByVal strPlanType As String)

    '    Try
    '        'RadioButtonListSplitAmtType.Visible = blnRadioButtonListSplitAmtType
    '        'RadioButtonListSplitAmtType.Enabled = blnRadioButtonListSplitAmtTypeEnable
    '        'lnkButtonBothPlans.Visible = blnLinkButtonBothPlans
    '        'lnkButtonRetirementPlan.Visible = blnLinkButtonRetPlan
    '        'lnkButtonSavingsPlan.Visible = blnLinkButtonSavePlan

    '        'lblBothPlans.Visible = blnLabelBothPlans
    '        'lblRetirementPlan.Visible = blnRetirementPlan
    '        'lblSavingsPlan.Visible = blnLabelSavingsPlan

    '        'TextBoxAmountWorkSheet.Visible = blnTextBoxAmountWorkSheet
    '        'TextBoxPercentageWorkSheet.Visible = blnTextBoxPercentageWorkSheet
    '        TextBoxAmountWorkSheet.Enabled = blnTextBoxAmountWorkSheetEnable
    '        TextBoxPercentageWorkSheet.Enabled = blnTextBoxPercentageWorkSheetEnable
    '        'lblBothPlansPerOrAmt.Visible = blnBothPlansPerOrAmt 
    '        Me.SplitPlanTypeOption = strPlanType
    '        ButtonSplit.Enabled = blnButtonSplit
    '        ButtonAdjust.Enabled = blnButtonAdjust
    '        ButtonReset.Enabled = blnButtonReset
    '        ButtonShowBalance.Enabled = blnButtonShowBalance
    '        'ButtonDocumentSave.Enabled = blnButtonDocumentSave 'PPP | 01/24/2017 | YRS-AT-3299 | Old button
    '        If Not String.IsNullOrEmpty(strPlanType) Then
    '            'LoadAnnuityTab()
    '        Else
    '            DataGridWorkSheet.DataSource = Nothing
    '            DataGridWorkSheet.DataBind()
    '        End If
    '        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = blnButtonShowBalance
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Not using EnabledAndVisibleControls

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Not using GetAnnuitiesBalancesDetails
    'Private Sub GetAnnuitiesBalancesDetails(ByVal strPlanType As String)
    '    Try
    '        TextBoxAmountWorkSheet.Text = "0.00"
    '        TextBoxPercentageWorkSheet.Text = "0.00"
    '        CheckBoxSpecialDividends.Checked = False
    '        RadioButtonListSplitAmtType.Items(1).Selected = True
    '        blnbtnShowBalances = False
    '        Select Case strPlanType
    '            Case EnumPlanTypes.BOTH.ToString()
    '                If GetSplitAnnuitiesBalances(EnumPlanTypes.BOTH.ToString(), blnLnkBothPlans, blnbtnShowBalances) Then
    '                    EnabledAndVisibleControls(True, False, False, False, False, False, False, False, True, True, False, False, False, False, True, True, blnbtnShowBalances, True, EnumPlanTypes.BOTH.ToString())
    '                    CheckAndUncheckOriginalAccountBalance()
    '                Else
    '                    EnabledAndVisibleControls(True, True, False, True, True, True, False, False, True, True, False, True, False, False, False, False, blnbtnShowBalances, False, EnumPlanTypes.BOTH.ToString())
    '                    DisplaySplitProgressCaption(True)
    '                End If
    '            Case EnumPlanTypes.RETIREMENT.ToString()
    '                If GetSplitAnnuitiesBalances(EnumPlanTypes.RETIREMENT.ToString(), blnLnkBothPlans, blnbtnShowBalances) Then
    '                    EnabledAndVisibleControls(True, False, False, False, True, False, False, False, True, True, False, False, False, False, True, True, blnbtnShowBalances, True, EnumPlanTypes.RETIREMENT.ToString())
    '                    CheckAndUncheckOriginalAccountBalance()
    '                Else
    '                    EnabledAndVisibleControls(True, True, blnLnkBothPlans, False, True, False, True, False, True, True, False, True, blnLnkBothPlans, False, False, False, blnbtnShowBalances, False, EnumPlanTypes.RETIREMENT.ToString())
    '                    DisplaySplitProgressCaption(True)
    '                End If
    '            Case EnumPlanTypes.SAVINGS.ToString()
    '                If GetSplitAnnuitiesBalances(EnumPlanTypes.SAVINGS.ToString(), blnLnkBothPlans, blnbtnShowBalances) Then
    '                    EnabledAndVisibleControls(True, False, False, True, False, False, False, False, True, True, False, False, False, False, True, True, blnbtnShowBalances, True, EnumPlanTypes.SAVINGS.ToString())
    '                    CheckAndUncheckOriginalAccountBalance()
    '                Else
    '                    EnabledAndVisibleControls(True, True, blnLnkBothPlans, True, False, False, False, True, True, True, False, True, blnLnkBothPlans, False, False, False, blnbtnShowBalances, False, EnumPlanTypes.SAVINGS.ToString())
    '                    DisplaySplitProgressCaption(True)
    '                End If
    '        End Select
    '    Catch
    '        Throw
    '    End Try

    'End Sub
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Not using GetAnnuitiesBalancesDetails

    'START: PPP | 01/24/2017 | YRS-AT-3299 | Not using GetAnnuitiesBalancesDetails
    'Getting Exisitng Annuities after Split Retirement/Saving/both Both Link option will be invisiable 
    'Private Function GetSplitAnnuitiesBalances(ByVal strPlanType As String, ByRef blnLinkBothSplitTypes As Boolean, ByRef blnbtnShowBalances As Boolean) As Boolean

    '    Try

    '        If Not Me.Session_Datatable_DtRecptAccount Is Nothing Then
    '            If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtRecptAccount) Then

    '                blnLinkBothSplitTypes = True
    '                blnbtnShowBalances = True

    '                dtRecptAccount = Me.Session_Datatable_DtRecptAccount
    '                Dim drRecpsAccount As DataRow()
    '                drRecpsAccount = dtRecptAccount.Select("RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")
    '                If (drRecpsAccount.Length > 0) Then
    '                    blnLinkBothSplitTypes = False
    '                    drRecpsAccount = dtRecptAccount.Select("splitType='" & strPlanType & "'AND RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")
    '                    If (drRecpsAccount.Length > 0) Then
    '                        RadioButtonListSplitAmtType.ClearSelection()
    '                        If (drRecpsAccount(0)("IsSplitPercentage").ToString.ToLower = "true") Then
    '                            RadioButtonListSplitAmtType.Items(1).Selected = True
    '                            TextBoxPercentageWorkSheet.Text = drRecpsAccount(0)("SplitAmount").ToString
    '                        ElseIf (drRecpsAccount(0)("IsSplitPercentage").ToString.ToLower = "false") Then
    '                            RadioButtonListSplitAmtType.Items(0).Selected = True
    '                            TextBoxAmountWorkSheet.Text = drRecpsAccount(0)("SplitAmount").ToString
    '                        End If
    '                        CheckBoxSpecialDividends.Checked = drRecpsAccount(0)("IncludeSpecialdev")
    '                        DataGridRecipientAnnuitiesBalance.DataSource = GetRecipientAccountBySplitTypes(dtRecptAccount, strPlanType) ' Recipient's Annuities 
    '                        DataGridRecipientAnnuitiesBalance.DataBind()
    '                        dtPartAccount = Me.Session_Datatable_DtPartAccount  'Participant Annuities Grid
    '                        DatagridParticipantsBalance.DataSource = dtPartAccount
    '                        DatagridParticipantsBalance.DataBind()
    '                        Return True
    '                    Else
    '                        DataGridRecipientAnnuitiesBalance.DataSource = Nothing ' Recipient's Annuities 
    '                        DataGridRecipientAnnuitiesBalance.DataBind()
    '                        DatagridParticipantsBalance.DataSource = Nothing 'Participant Annuities Grid
    '                        DatagridParticipantsBalance.DataBind()
    '                        Return False
    '                    End If
    '                End If
    '            Else

    '                Return False
    '            End If
    '        End If
    '    Catch
    '        Throw
    '    End Try

    'End Function
    'END: PPP | 01/24/2017 | YRS-AT-3299 | Not using GetAnnuitiesBalancesDetails

    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
    Private Sub ClearObjects()

        Try
            Me.Session_Datatable_DtBenifAccount = Nothing
            'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : START
            Me.Session_Datatable_DsPartBal = Nothing
            'Harshala : 23 Mar 2012 : BT ID : 1002 - Error in Retired QDRO Settlement : END
            Me.Session_Datatable_DtPartTotal = Nothing
            Me.Session_Datatable_DtPartAccount = Nothing
            Me.ViewState_Datatable_DtPartCurrentAccount = Nothing 'Chandra sekar| 2016.08.22 | YRS-AT-3081  TO have Currently Selected Original account balances
            Me.SplitPlanTypeOption = Nothing
            Me.Session_Datatable_DtRecptAccount = Nothing
            'Session("FindInfo_Sort") = Nothing 'PPP | 01/24/2017 | YRS-AT-3299 | This session variable was being used on old find screen
            'dgPager.Visible = False 'MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
            Session("dsTransactions") = Nothing
            Session("PartAccountDetail") = Nothing
            Me.SplitPlanTypeOption = Nothing
            'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
            'Me.String_FundEventID = Nothing
            'Me.String_PersId = Nothing
            'Me.String_Part_SSN = Nothing
            'Me.String_PhonySSNo = Nothing
            'Me.String_Benif_SSno = Nothing
            'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
        Catch
            Throw
        End Try
    End Sub
    Private Sub AssignAttributesToControls()

        Try
            'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
            'TextBoxSSNoList.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'TextBoxCityList.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'TextBoxStateList.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'TextBoxFirstNameList.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'TextBoxFundNoList.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'TextBoxLastNameList.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required

            'Me.TextboxSpouseEmail.Attributes.Add("onBlur", "javascript:isValidEmail(TextboxSpouseEmail.value);")
            Me.TextBoxAmountWorkSheet.Attributes.Add("onkeypress", "ValidateNumeric();")
            Me.TextBoxPercentageWorkSheet.Attributes.Add("onkeypress", "ValidateNumeric();")
            Me.TextBoxTel.Attributes.Add("onkeypress", "ValidateNumeric();")
            'START: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
            'Me.ButtonShowBalance.Attributes.Add("onclick", "javascript:return CheckAccess('btnShowBalance');")
            'Me.TextBoxFundNoList.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
            'Me.TextBoxSSNoList.Attributes.Add("onkeypress", "javascript:ValidatePNumeric(TextBoxSSNoList.value);")
            'END: MMR | 2017.01.10 | YRS-AT-3299 | Commented existing code as not required
            'Me.TextboxSpouseZip.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
            Me.TextBoxTel.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
            Me.TextBoxSSNo.Attributes.Add("OnKeyPress", "javascript:ValidatePNumeric(TextBoxSSNo.value);")
            Me.TextBoxAmountWorkSheet.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

            Me.TextBoxAmountWorkSheet.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            Me.TextBoxPercentageWorkSheet.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
            Me.TextBoxPercentageWorkSheet.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")

            'Anudeep:13.04.2013 :BT-1555:YRS 5.0-1769:Length of phone numbers
            Me.TextBoxTel.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxTel);")
            'Me.TextboxSpouseTel.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextboxSpouseTel);")
            'add kranthi 290808.
            'Me.TextBoxAmountWorkSheet.Attributes.Add("onBlur", "Javascript:return Check_Amount_Percentage(TextBoxAmountWorkSheet.value);")
            'Me.TextBoxPercentageWorkSheet.Attributes.Add("onBlur", "Javascript:return Check_Amount_Percentage(TextBoxPercentageWorkSheet.value);")
            'add kranthi 290808.

            'add kranthi 010908.
            Me.btnAddBeneficiaryToList.Attributes.Add("onclick", "Javascript:return ValidateSSNo(TextBoxSSNo.value);")

            ButtonShowBalance.Attributes.Add("onclick", "javascript: return OpenNewWindow('ShowBalancesQDRORetired.aspx');")
        Catch
            Throw
        End Try
    End Sub
    Private Sub AssignIdToControls()

        Try
            'START: PPP | 01/24/2017 | YRS-AT-3299 | Controls were part of internal find screen which is not in use
            'LabelSSNoList.AssociatedControlID = TextBoxSSNoList.ID
            'LabelFundNoList.AssociatedControlID = TextBoxFundNoList.ID
            'LabelLastNameList.AssociatedControlID = TextBoxLastNameList.ID
            'LabelFirstNameList.AssociatedControlID = TextBoxFirstNameList.ID
            'END: PPP | 01/24/2017 | YRS-AT-3299 | Controls were part of internal find screen which is not in use
            'list
            'personal
            LabelSSNo.AssociatedControlID = TextBoxSSNo.ID
            LabelSal.AssociatedControlID = DropdownlistSal.ID
            LabelFirstName.AssociatedControlID = TextBoxFirstName.ID
            LabelMiddleName.AssociatedControlID = TextBoxMiddleName.ID
            LabelLastName.AssociatedControlID = TextBoxLastName.ID
            LabelSuffix.AssociatedControlID = TextBoxSuffix.ID
            'LabelSpouseAdd1.AssociatedControlID = TextboxSpouseAdd1.ID
            'LabelSpouseAdd2.AssociatedControlID = TextboxSpouseAdd2.ID
            'LabelSpouseAdd3.AssociatedControlID = TextboxSpouseAdd3.ID
            'LabelSpouseCity.AssociatedControlID = TextboxSpouseCity.ID
            'LabelSpouseState.AssociatedControlID = TextboxSpouseState.ID
            'LabelSpouseZip.AssociatedControlID = TextboxSpouseZip.ID
            LabelTel.AssociatedControlID = TextBoxTel.ID
            LabelEmail.AssociatedControlID = TextBoxEmail.ID
        Catch
            Throw
        End Try
    End Sub
    Private Sub PopulateBeneficiaryPersonalDetails(ByVal strSSNo As String)

        Dim strWarningsMessage As String = String.Empty
        Dim dsRecipientDetails As New DataSet
        Dim dsContactInformation As New DataSet
        Dim dsAddressDetails As New DataSet
        Dim drRecipient As DataRow
        Dim drAddressinfo As DataRow

        'START: PPP | 01/23/2017 | YRS-AT-3299 | Variables required to fetch details from AtsBeneficiary table
        Dim recipientDetailsFromBeneficiaries, addressDetails As DataSet
        Dim recipientRow As DataRow
        'END: PPP | 01/23/2017 | YRS-AT-3299 | Variables required to fetch details from AtsBeneficiary table
        Try

            dsRecipientDetails = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getQDRORecipient(strSSNo)

            If HelperFunctions.isNonEmpty(dsRecipientDetails) Then

                drRecipient = dsRecipientDetails.Tables(0).Rows(0)

                Me.String_RecptPersId = drRecipient.Item("UniqueId").ToString
                Session("R_QDRO_PersID") = Me.String_RecptPersId
                Me.String_RecptFundEventID = drRecipient.Item("FundEventID").ToString()
                'Me.string_RecptFundEventID = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getQDROFundEventID(Me.string_RecptPersId, l_string_Message)
                If strWarningsMessage <> String.Empty Then
                    'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                    'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", strWarningsMessage, MessageBoxButtons.Stop, False)
                    ShowModalPopupWithCustomMessage("QDRO", strWarningsMessage, "error")
                    'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                    'Me.Session_Bool_MissingFundEventId = True 'PPP | 01/24/2017 | YRS-AT-3299 | Session was used to handle post back event if recipient fund event id is not set, but it is handled by modal popup so postback does not happen
                    ClearControls(True)
                    Exit Sub
                End If

                If Not IsDBNull(drRecipient.Item("SalutationCode")) Then
                    If CType(drRecipient.Item("SalutationCode"), String).Trim = "" Then
                        DropdownlistSal.SelectedIndex = 0
                    ElseIf CType(drRecipient.Item("SalutationCode"), String).Trim = "Dr." Or CType(drRecipient.Item("SalutationCode"), String).Trim = "Dr" Then
                        DropdownlistSal.SelectedIndex = 1
                    ElseIf CType(drRecipient.Item("SalutationCode"), String).Trim = "Mr." Or CType(drRecipient.Item("SalutationCode"), String).Trim = "Mr" Then
                        DropdownlistSal.SelectedIndex = 2
                    ElseIf CType(drRecipient.Item("SalutationCode"), String).Trim = "Mrs." Or CType(drRecipient.Item("SalutationCode"), String).Trim = "Mrs." Then
                        DropdownlistSal.SelectedIndex = 3
                    ElseIf CType(drRecipient.Item("SalutationCode"), String).Trim = "Ms." Or CType(drRecipient.Item("SalutationCode"), String).Trim = "Ms." Then
                        DropdownlistSal.SelectedIndex = 4
                    End If
                End If

                TextBoxFirstName.Text = drRecipient.Item("FirstName").ToString()
                TextBoxMiddleName.Text = drRecipient.Item("MiddleName").ToString()
                TextBoxLastName.Text = drRecipient.Item("LastName").ToString()
                TextBoxSuffix.Text = drRecipient.Item("SuffixTitle").ToString()
                TextBoxBirthDate.Text = Convert.ToDateTime(drRecipient.Item("BirthDate")).ToString("MM/dd/yyyy") 'START: MMR | 2017.01.28 |YRS-AT-3299 |Converting datetime to date format only while assigning date to textbox
                'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Selecting marital status and gender.
                'Start - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Commented existing code as not required
                'DropDownListMaritalStatus.SelectedValue = drRecipient.Item("MaritalStatusCode").ToString()
                'Start: MMR | 2016.09.27 | YRS-AT-2482 | Setting dropdown value based on condition if value from database is empty or not present in dropdown values then set to default value "SEL" or bind values
                Dim beneficiaryGender As String = drRecipient.Item("GenderCode").ToString()
                If Me.DropDownListGender.Items.FindByValue(beneficiaryGender) Is Nothing Or String.IsNullOrEmpty(beneficiaryGender) Then
                    DropDownListGender.SelectedValue = "SEL"
                Else
                    DropDownListGender.SelectedValue = beneficiaryGender
                End If
                'End: MMR | 2016.09.27 | YRS-AT-2482 | Setting dropdown value based on condition if value from database is empty or not present in dropdown values then set to default value "SEL" or bind values
                'End - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Commented existing code as not required
                'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Selecting marital status and gender.
                'added kranthi 141008
                'If CheckboxIsSpouse.Checked = True Then
                '    CheckboxIsSpouse.Checked = False
                'End If
                'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                dsAddressDetails = Address.GetAddressByEntity(drRecipient.Item("UniqueId").ToString(), EnumEntityCode.PERSON)
                If HelperFunctions.isNonEmpty(dsAddressDetails.Tables("Address")) Then
                    ' If dsAddressDetails.Tables("Address").Rows.Count > 0 Then
                    drAddressinfo = dsAddressDetails.Tables(0).Rows(0)
                    AddressWebUserControl1.LoadAddressDetail(dsAddressDetails.Tables("Address").Select("isPrimary = True"))
                    'End If
                End If
                'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                dsContactInformation = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAddressInfo(drRecipient.Item("UniqueId").ToString)
                If HelperFunctions.isNonEmpty(dsContactInformation) Then
                    If dsContactInformation.Tables.Count > 1 Then
                        If dsContactInformation.Tables(1).Rows.Count > 0 Then
                            If (dsContactInformation.Tables(1).Rows(0).Item("EmailAddress").ToString() <> "System.DBNull" And dsContactInformation.Tables(1).Rows(0).Item("EmailAddress").ToString() <> String.Empty) Then
                                Me.TextBoxEmail.Text = dsContactInformation.Tables(1).Rows(0).Item("EmailAddress").ToString()
                            End If
                        End If
                    End If
                    If dsContactInformation.Tables.Count > 2 Then
                        If dsContactInformation.Tables(2).Rows.Count > 0 Then
                            If (dsContactInformation.Tables(2).Rows(0).Item("PhoneNumber").ToString() <> "System.DBNull" And dsContactInformation.Tables(2).Rows(0).Item("PhoneNumber").ToString() <> String.Empty) Then
                                Me.TextBoxTel.Text = dsContactInformation.Tables(2).Rows(0).Item("PhoneNumber").ToString()
                            End If
                        End If
                    End If
                End If

                Me.Session_Bool_NewPerson = False
                LockAndUnLockRecipientControls(False)
                btnAddBeneficiaryToList.Enabled = True                        'Add kranthi 140808.
            Else
                'Such a person does not exist in the system
                'START: PPP | 01/19/2017 | YRS-AT-3299 | Generate GUID at application level which will elemate database hit
                'dsRecipientDetails = YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.getNewPersonID
                'drRecipient = dsRecipientDetails.Tables(0).Rows(0)
                'Me.String_RecptPersId = drRecipient.Item("Newguid").ToString
                Me.String_RecptPersId = Guid.NewGuid().ToString()
                'END: PPP | 01/19/2017 | YRS-AT-3299 | Generate GUID at application level which will elemate database hit
                'Added to Merge the Fund event into Single fund Event id
                'dsRecipientDtls = YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.getNewPersonID
                'dr = dsRecipientDtls.Tables(0).Rows(0)
                'Me.String_RecptFundEventID = dr.Item("Newguid").ToString
                Me.Session_Bool_NewPerson = True
                LockAndUnLockRecipientControls(True)
                DropdownlistSal.SelectedItem.Value = ""
                btnAddBeneficiaryToList.Enabled = True                        'Add kranthi 140808

            End If

            'START: PPP | 01/23/2017 | YRS-AT-3299 | Similar to NonRetired QDRO fetching details from AtsBeneficiary table
            If Me.Session_Bool_NewPerson Then
                ' If SSN is not exists in AtsPers then lookup in AtsBeneficiaries table
                recipientDetailsFromBeneficiaries = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getQDRORecipientFromBenefiary("Member", TextBoxSSNo.Text)
                If recipientDetailsFromBeneficiaries.Tables(0).Rows.Count > 0 Then
                    recipientRow = recipientDetailsFromBeneficiaries.Tables(0).Rows(0)

                    Me.String_RecptPersId = recipientRow.Item("UniqueId").ToString
                    Session("NR_QDRO_PersID") = recipientRow.Item("PersID").ToString

                    TextBoxFirstName.Text = IIf(IsDBNull(recipientRow.Item("FirstName")), "", recipientRow.Item("FirstName"))
                    TextBoxLastName.Text = IIf(IsDBNull(recipientRow.Item("LastName")), "", recipientRow.Item("LastName"))
                    TextBoxBirthDate.Text = IIf(IsDBNull(recipientRow.Item("BirthDate")), "", recipientRow.Item("BirthDate"))
                    Me.Session_Bool_NewPerson = True
                    LockAndUnLockRecipientControls(True)
                    btnAddBeneficiaryToList.Enabled = True

                    addressDetails = Address.GetBeneficiariesAddress("", TextBoxSSNo.Text, TextBoxFirstName.Text, TextBoxLastName.Text)
                    If HelperFunctions.isNonEmpty(addressDetails) Then
                        AddressWebUserControl1.LoadAddressDetail(addressDetails.Tables(0).Select("isPrimary = True"))
                    Else
                        AddressWebUserControl1.LoadAddressDetail(Nothing)
                    End If
                End If
            End If
            'END: PPP | 01/23/2017 | YRS-AT-3299 | Similar to NonRetired QDRO fetching details from AtsBeneficiary table

            'Added to get New Recipent FundEvent ID
            ButtonCancelBeneficiary.Enabled = True
            'START: PPP | 01/19/2017 | YRS-AT-3299 | Generate GUID at application level which will elemate database hit
            'dsRecipientDetails = YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.getNewPersonID
            'drRecipient = dsRecipientDetails.Tables(0).Rows(0)
            'Me.String_RecptFundEventID = drRecipient.Item("Newguid").ToString
            Me.String_RecptFundEventID = Guid.NewGuid().ToString()
            'END: PPP | 01/19/2017 | YRS-AT-3299 | Generate GUID at application level which will elemate database hit
            'Added to get New Retiree ID for recipent
            'START: PPP | 01/19/2017 | YRS-AT-3299 | Generate GUID at application level which will elemate database hit
            'dsRecipientDetails = YMCARET.YmcaBusinessObject.QdroMemberRetiredBOClass.getNewPersonID
            'drRecipient = dsRecipientDetails.Tables(0).Rows(0)
            'Me.String_RecptRetireeID = drRecipient.Item("Newguid").ToString
            Me.String_RecptRetireeID = Guid.NewGuid().ToString()
            'END: PPP | 01/19/2017 | YRS-AT-3299 | Generate GUID at application level which will elemate database hit
            'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling gender,marital and compare Validator control               
            ManageEditableControls(True)
            'START-CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function
            ' CType(Me.FindControl("compValidatorGender"), CompareValidator).Enabled = True
            ' CType(Me.FindControl("compValidatorMaritalStatus"), CompareValidator).Enabled = True
            'END-CS | 2016.17.09 | YRS-AT-3081 | Code is moved to the common function
            'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling gender,marital and compare Validator control   
        Catch
            Throw
        End Try
    End Sub
    Private Sub LoanBeneficiaryPersonalDetails()
        Try


            Dim l_string_Part_SSN As String = String.Empty
            l_string_Part_SSN = Me.String_Part_SSN
            l_string_Part_SSN = l_string_Part_SSN.Substring(0, l_string_Part_SSN.IndexOf("-"))
            AddressWebUserControl1.ZipCode = ""
            TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "")
            If Len(TextBoxSSNo.Text) <> 9 Then
                'START: PPP | 01/09/2017 | YRS-AT-3299 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PLEASE_ENTER_VALID_SSNO, MessageBoxButtons.Stop, False) ''Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
                ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PLEASE_ENTER_VALID_SSNO, "error")
                'END: PPP | 01/02/2017 | YRS-AT-3299 | Moving messages to JQuery modal popup
                Control_To_Focus = TextBoxSSNo                    'Add kranthi 180808
                Call ClearControls(True)
                LockAndUnLockRecipientControls(False)
                ManageEditableControls(False) 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling gender and marital control
                TextBoxSSNo.Enabled = True
                DropdownlistSal.SelectedItem.Value = ""
                btnAddBeneficiaryToList.Enabled = False
                'Session_String_ISAddUpdate = "InvalidSSNo" 'PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition
                EnableAndDisableValidatorcontrols(False) 'CS | 2016.09.17 | YRS-AT-3081 | To Disable the Validator controls
                Exit Sub
            Else
                Session("R_QDRO_PersID") = Nothing
                If TextBoxSSNo.Text.Equals(l_string_Part_SSN) Then  'CType(TextBoxSpouseSSNo.Text, String) = CType(TextBoxPerSSNo.Text, String) Then
                    'START: PPP | 01/24/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                    'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_RECIPIENT_DUPLICATE_SSNO_VALIDATION, MessageBoxButtons.Stop, False) 'Chandra sekar|2016.08.22|YRS-AT-3081 - Static Text is moved to the Resource file 
                    ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_RECIPIENT_DUPLICATE_SSNO_VALIDATION, "error")
                    'END: PPP | 01/24/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
                    'Session_String_ISAddUpdate = "InvalidSSNo" ' PPP | 01/24/2017 | YRS-AT-3299 | Session_String_ISAddUpdate value is not used in any condition
                    EnableAndDisableValidatorcontrols(False)  'CS | 2016.09.17 | YRS-AT-3081 | To Disable the Validator controls
                    Exit Sub
                End If
                Me.ClearControls(False)
                EnableAndDisableValidatorcontrols(True) 'CS | 2016.09.17 | YRS-AT-3081 | To enable the Validator controls
                PopulateBeneficiaryPersonalDetails(TextBoxSSNo.Text)
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub ResetAnnuitiesDetails()
        Dim dtRow() As DataRow
        Dim dtRowParticipantTemp() As DataRow
        Dim dtRowCount As Integer
        Dim dtRowRecptAccount As DataRow
        Dim dtRowParticipant As DataRow
        Dim dtRowRecptAccounttemp As DataRow
        Dim dtRowPartAccountbalance As DataRow
        Dim dtParticipantAccountTemp As DataTable
        Dim dtRecptAccountBalanceTemp As DataTable
        Try

            dtPartAccount = Me.Session_Datatable_DtPartAccount 'Participant Annuities Grid
            dtRecptAccount = Me.Session_Datatable_DtRecptAccount 'Account balance grid
            dtPartOriginal = Me.Session_Datatable_DtPartTotal 'Both account balance '
            dtRecptAccountBalanceTemp = Me.Session_Datatable_DtRecptAccountTemp ' To Keep Current Recipient Balance  in the Temp Datatable
            dtRow = dtRecptAccount.Select("splitType ='" + Me.SplitPlanTypeOption + "' AND RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")

            For Each dtRowPartAccountbalance In dtPartAccount.Rows
                For Each dtRowRecptAccounttemp In dtRow
                    If dtRowPartAccountbalance("guiAnnuityID") = dtRowRecptAccounttemp("guiAnnuityID") Then
                        dtRowPartAccountbalance("SSLevelingAmt") = dtRowRecptAccounttemp("SSLevelingAmt") + dtRowPartAccountbalance("SSLevelingAmt")
                        dtRowPartAccountbalance("SSReductionAmt") = dtRowRecptAccounttemp("SSReductionAmt") + dtRowPartAccountbalance("SSReductionAmt")
                        dtRowPartAccountbalance("CurrentPayment") = dtRowRecptAccounttemp("CurrentPayment") + dtRowPartAccountbalance("CurrentPayment")
                        dtRowPartAccountbalance("EmpPreTaxCurrentPayment") = dtRowRecptAccounttemp("EmpPreTaxCurrentPayment") + dtRowPartAccountbalance("EmpPreTaxCurrentPayment")
                        dtRowPartAccountbalance("EmpPostTaxCurrentPayment") = dtRowRecptAccounttemp("EmpPostTaxCurrentPayment") + dtRowPartAccountbalance("EmpPostTaxCurrentPayment")
                        dtRowPartAccountbalance("YmcaPreTaxCurrentPayment") = dtRowRecptAccounttemp("YmcaPreTaxCurrentPayment") + dtRowPartAccountbalance("YmcaPreTaxCurrentPayment")
                        dtRowPartAccountbalance("EmpPreTaxRemainingReserves") = dtRowRecptAccounttemp("EmpPreTaxRemainingReserves") + dtRowPartAccountbalance("EmpPreTaxRemainingReserves")
                        dtRowPartAccountbalance("EmpPostTaxRemainingReserves") = dtRowRecptAccounttemp("EmpPostTaxRemainingReserves") + dtRowPartAccountbalance("EmpPostTaxRemainingReserves")
                        dtRowPartAccountbalance("YmcapreTaxRemainingReserves") = dtRowRecptAccounttemp("YmcapreTaxRemainingReserves") + dtRowPartAccountbalance("YmcapreTaxRemainingReserves")
                    End If
                Next
            Next

            dtPartAccount.AcceptChanges()

            dtRow = dtRecptAccount.Select("splitType ='" + Me.SplitPlanTypeOption + "' AND RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")

            For dtRowCount = 0 To dtRow.Length - 1
                dtRecptAccount.Rows.Remove(dtRow(dtRowCount))
            Next

            dtRecptAccount.AcceptChanges()
            dtParticipantAccountTemp = dtPartAccount.Copy
            'Remove current splited Annuities balance from temp table maintained for recipient
            If HelperFunctions.isNonEmpty(dtRecptAccountBalanceTemp) Then
                dtRow = dtRecptAccountBalanceTemp.Select(String.Format("splitType ='{0}' AND RecipientPersonID='{1}'", Me.SplitPlanTypeOption, DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim()))
                For dtRowCount = 0 To dtRow.Length - 1
                    dtRecptAccountBalanceTemp.Rows.Remove(dtRow(dtRowCount))
                Next
                dtRecptAccountBalanceTemp.AcceptChanges()
            End If
            'Below  logic to remove the participant Annuities Of Split type if there is no SPlit Type availbale in Recipient annuities
            If HelperFunctions.isNonEmpty(dtRecptAccount) Then
                For Each drRowParticipantAccountbalance In dtParticipantAccountTemp.Rows
                    dtRow = dtRecptAccount.Select("splitType ='" & drRowParticipantAccountbalance("splitType") & "'")
                    If dtRow.Length = 0 Then
                        dtRowParticipantTemp = dtPartAccount.Select("splitType ='" & drRowParticipantAccountbalance("splitType") & "'")
                        For dtRowCount = 0 To dtRowParticipantTemp.Length - 1
                            dtPartAccount.Rows.Remove(dtRowParticipantTemp(dtRowCount))
                        Next
                    End If
                Next
            End If
            dtPartAccount.AcceptChanges()

            Me.Session_Datatable_DtPartAccount = dtPartAccount
            Me.Session_Datatable_DtRecptAccount = dtRecptAccount
            Me.Session_Datatable_DtRecptAccountTemp = dtRecptAccountBalanceTemp
            'START: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function
            'DataGridRecipientAnnuitiesBalance.DataSource = Nothing
            'DataGridRecipientAnnuitiesBalance.DataBind()
            'DatagridParticipantsBalance.DataSource = dtPartAccount
            'DatagridParticipantsBalance.DataBind()
            'END: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function
            If HelperFunctions.isEmpty(Me.Session_Datatable_DtRecptAccount) Then
                'START: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function
                'DatagridParticipantsBalance.DataSource = Nothing
                'DatagridParticipantsBalance.DataBind()
                'END: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function
                Me.Session_Datatable_DtRecptAccountTemp = Nothing
                Me.Session_Datatable_DtPartAccount = Nothing
                Me.Session_Datatable_DtRecptAccount = Nothing
            End If

            'START: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function
            'GetAnnuitiesBalancesDetails(Me.SplitPlanTypeOption)
            'DisplaySplitProgressCaption(False)
            EnableNextButton(False)
            ShowHideControls()
            'END: PPP | 01/23/2017 | YRS-AT-3299 | Assigning grid values using ShowHideControls function

            strWarningMessage = String.Format(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_RESET_SPLIT_PLAN_TYPE, Me.SplitPlanTypeOption)
            'START: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
            'MessageBox.Show(PlaceHolderACHDebitImportProcess, "QDRO", strWarningMessage, MessageBoxButtons.OK, False)
            ShowModalPopupWithCustomMessage("QDRO", strWarningMessage, "ok")
            'END: PPP | 01/23/2017 | YRS-AT-3299 | Based on its type displaying message in a grid or div
        Catch
            Throw
        End Try
    End Sub

    ''To displaying the Caption of Current Split option is been Progress
    'Private Sub DisplaySplitProgressCaption(ByVal blnCurrentSplitOptions As Boolean)
    '    lblCurrentSplitOptions.Visible = blnCurrentSplitOptions
    '    strWarningMessage = String.Format(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_CURRENT_SPLIT_PLAN_TYPE, Me.SplitPlanTypeOption)
    '    lblCurrentSplitOptions.Text = strWarningMessage
    'End Sub

    ' For UnCheck and Disable of the Original Account Balance rows After Split done
    Private Sub CheckAndUncheckOriginalAccountBalance()
        Dim dgCount As Integer
        Dim dgItem As DataGridItem
        Try
            If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtRecptAccount) Then
                dtRecptAccount = Me.Session_Datatable_DtRecptAccount
                Dim drRecpsAccount As DataRow()
                drRecpsAccount = dtRecptAccount.Select(String.Format("RecipientPersonID ='{0}'", DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim()))
                If (drRecpsAccount.Length > 0) Then
                    For dgCount = 0 To DataGridWorkSheet.Items.Count - 1
                        dgItem = DataGridWorkSheet.Items(dgCount)

                        drRecpsAccount = dtRecptAccount.Select(String.Format("splitType ='{0}' AND RecipientPersonID ='{1}' AND guiAnnuityID = '{2}'", Me.SplitPlanTypeOption, DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim(), dgItem.Cells(0).Text.Trim()))
                        If (drRecpsAccount.Length = 0) Then
                            Dim Chkbox As CheckBox = CType(DataGridWorkSheet.Items(dgCount).FindControl("CheckBoxSelect"), CheckBox)
                            Chkbox.Checked = False
                            Chkbox.Enabled = False
                        End If

                    Next
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'UnCheck the CheckBox and Disable the account row if Annuities are having "0" Values
    Private Sub UncheckAndDisableOriginalAccountBalance(ByVal dtParticpantOriginal As DataTable)
        Dim dgCount As Integer
        Dim dgItem As DataGridItem
        Try
            If HelperFunctions.isNonEmpty(dtParticpantOriginal) Then
                Dim drOrginalAccountbalance As DataRow()

                For dgCount = 0 To DataGridWorkSheet.Items.Count - 1
                    dgItem = DataGridWorkSheet.Items(dgCount)
                    drOrginalAccountbalance = dtParticpantOriginal.Select(String.Format("guiAnnuityID = '{0}'", dgItem.Cells(0).Text.Trim()))
                    For Each dtRowPartAccountbalance In drOrginalAccountbalance
                        If Convert.ToDouble(dtRowPartAccountbalance("CurrentPayment")) <= 0.0 Then
                            Dim Chkbox As CheckBox = CType(DataGridWorkSheet.Items(dgCount).FindControl("CheckBoxSelect"), CheckBox)
                            Chkbox.Checked = False
                            Chkbox.Enabled = False
                        End If
                    Next
                Next
            End If
        Catch
            Throw
        End Try
    End Sub
    ' Disable/Enable the Vaildator controls
    Private Sub EnableAndDisableValidatorcontrols(ByVal blnControls As Boolean)
        CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = blnControls
        CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = blnControls
        TextBoxBirthDate.RequiredDate = blnControls
        CType(Me.FindControl("compValidatorGender"), CompareValidator).Enabled = blnControls
        CType(Me.FindControl("compValidatorMaritalStatus"), CompareValidator).Enabled = blnControls
    End Sub
    'END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)

    'START: MMR | 01/10/2017 | YRS-AT-3299 
    ' ShowModalPopupWithCustomMessage will either display message on screen in strip or in a modal popup based on its type
    Private Sub ShowModalPopupWithCustomMessage(ByVal title As String, ByVal message As String, ByVal type As String)
        Select Case type
            Case "info"
                If DivWarningMessage.InnerHtml.Trim.Length > 0 Then
                    DivWarningMessage.InnerHtml = String.Format("{0}<br />{1}", DivWarningMessage.InnerHtml, message)
                Else
                    DivWarningMessage.InnerHtml = message
                End If
                DivWarningMessage.Style("display") = "normal"
            Case "infoYesNo"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", String.Format("ShowDialog('{0}', '{1}','{2}');", title, message, type), True)
            Case "ok"
                If DivSuccessMessage.InnerHtml.Trim.Length > 0 Then
                    DivSuccessMessage.InnerHtml = String.Format("{0}<br />{1}", DivSuccessMessage.InnerHtml, message)
                Else
                    DivSuccessMessage.InnerHtml = message
                End If
                DivSuccessMessage.Style("display") = "normal"
            Case "error"
                If DivMainMessage.InnerHtml.Trim.Length > 0 Then
                    DivMainMessage.InnerHtml = String.Format("{0}<br />{1}", DivMainMessage.InnerHtml, message)
                Else
                    DivMainMessage.InnerHtml = message
                End If
                DivMainMessage.Style("display") = "normal"
        End Select
    End Sub

    ' Helps to initialize the page
    Private Sub LoadRetiredQDRODetails()
        Dim webServiceMessage As String
        Dim isSessionSet As Boolean
        Try
            isSessionSet = True
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "Start")

            If Not Session("IsArchived") Is Nothing AndAlso Not Session("FundStatus") Is Nothing Then
                If Session("IsArchived") = True And Session("FundStatus") <> "Retired" Then
                    ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTICIPANT_ARCHIVE_UNARCHIVE, "error") 'MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "YMCA-YRS" to "QDRO"
                    FreezeAllTabs()
                    Headercontrol.PageTitle = String.Empty
                    Exit Sub
                End If
            Else
                isSessionSet = False
                If Session("IsArchived") Is Nothing Then
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable IsArchived is nothing")
                ElseIf Session("FundStatus") Is Nothing Then
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable FundStatus is nothing")
                End If
            End If

            If Not Session("PersId") Is Nothing Then
                webServiceMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
                If webServiceMessage <> "NoPending" Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "openWSMsgBox", "openDialog('" + webServiceMessage + "','Pers');", True)
                    FreezeAllTabs()
                    Headercontrol.PageTitle = String.Empty
                    Exit Sub
                End If
            Else
                isSessionSet = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable PersId is nothing")
            End If

            Me.IsAccountLock = Nothing
            If Not Session("IsAccountLock") Is Nothing Then
                Me.IsAccountLock = Session("IsAccountLock")
            Else
                isSessionSet = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable IsAccountLock is nothing")
            End If

            ResetSession()

            If Not Session("PersSSID") Is Nothing Then
                Me.ParticipantSSN = Session("PersSSID")
            Else
                isSessionSet = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable PersSSID is nothing")
            End If

            If Not Session("PersSSID") Is Nothing And Not Session("LastName") Is Nothing And Not Session("FirstName") Is Nothing Then
                Me.String_Part_SSN = Session("PersSSID") + "-" + Session("LastName") + " " + Session("FirstName")
            Else
                isSessionSet = False
                If Session("LastName") Is Nothing Then
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable LastName is nothing")
                ElseIf Session("FirstName") Is Nothing Then
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable FirstName is nothing")
                End If
            End If

            If Not Session("PersId") Is Nothing Then
                Me.String_PersId = Session("PersId")
            Else
                isSessionSet = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable PersId is nothing")
            End If

            If Not Session("FundEventID") Is Nothing Then
                Me.String_FundEventID = Session("FundEventID")
            Else
                isSessionSet = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable FundEventID is nothing")
            End If

            If Not Session("RequestID") Is Nothing Then
                Me.String_QDRORequestID = Session("RequestID")
            Else
                isSessionSet = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable RequestID is nothing")
            End If

            Headercontrol.PageTitle = "QDRO Retirees Information"
            If Not Session("FundNo") Is Nothing Then
                Headercontrol.FundNo = Session("FundNo")
            Else
                isSessionSet = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "session variable FundNo is nothing")
            End If

            If Not isSessionSet Then
                FreezeAllTabs()
                ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTICIPANT_DATA_LOADING_ERROR, "error") 'MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "YMCA-YRS" to "QDRO"
            Else
                Call SetControlFocus(Me.TextBoxSSNo)
                ShowActiveTab(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES)
            End If
        Catch
            Throw
        Finally
            webServiceMessage = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro-LoadRetiredQDRODetails", "End")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub
    'START: MMR | 01/10/2017 | YRS-AT-3299 

    'START: MMR | 01/10/2017 | YRS-AT-3299 | Added to save/delete beneficiary to staging table
    Private Sub MaintainRecipientDetails(ByVal row As DataRow, ByVal requestID As String, ByVal action As String)
        Dim ds As DataSet
        Dim beneficiaryTableClone As DataTable
        Dim xml As String
        Try
            beneficiaryTableClone = CreateDataTableBenfAccount()
            beneficiaryTableClone.ImportRow(row)
            beneficiaryTableClone.TableName = "Recipient"

            If (beneficiaryTableClone.Rows.Count = 1) Then
                ds = New DataSet
                ds.Tables.Add(beneficiaryTableClone)
                xml = ds.GetXml()
                YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.MaintainRecipientDetails(xml, requestID, action)
            End If
        Catch
            Throw
        Finally
            xml = Nothing
            beneficiaryTableClone = Nothing
            ds = Nothing
        End Try
    End Sub
    'END: MMR | 01/10/2017 | YRS-AT-3299 | Added to save/delete beneficiary to staging table

    'START: PPP | 01/10/2017 | YRS-AT-3299 | Deletes recipient from table, naming method as DeleteDataFromTable because old method names are UpdateDataToTable and AddDataToTable
    Private Function DeleteDataFromTable(ByVal ssn As String)
        Dim recipientPersID As String
        Dim splitConfigurationRows() As DataRow
        Dim isSplitExists As Boolean

        dtBenifAccount = Me.Session_datatable_dtBenifAccount
        If HelperFunctions.isNonEmpty(dtBenifAccount) Then
            If dtBenifAccount.Rows.Count = 1 AndAlso Convert.ToString(dtBenifAccount.Rows(0)("SSNo")) = ssn Then
                MaintainRecipientDetails(dtBenifAccount.Rows(0), Me.String_QDRORequestID, YMCAObjects.DBActionType.DELETE)
                dtBenifAccount = Nothing
                ResetSession()
                EnableNextButton(False)
                btnNext.Visible = True
                DropdownlistBeneficiarySSNo.DataSource = Nothing
                DropdownlistBeneficiarySSNo.DataBind()
            Else
                For Each row As DataRow In dtBenifAccount.Rows
                    If Convert.ToString(row("SSNo")) = ssn Then
                        recipientPersID = Convert.ToString(row("id"))
                        DeleteAnnuitiesDetails(recipientPersID)

                        MaintainRecipientDetails(row, Me.String_QDRORequestID, YMCAObjects.DBActionType.DELETE)
                        row.Delete()
                        dtBenifAccount.AcceptChanges()
                        Exit For
                    End If
                Next
            End If
        End If
        hdnSelectedPlanType.Value = String.Empty
        Me.SplitPlanTypeOption = String.Empty
        Me.Session_datatable_dtBenifAccount = dtBenifAccount
    End Function
    'END: PPP | 01/10/2017 | YRS-AT-3299 | Deletes recipient from table, naming method as DeleteDataFromTable because old method names are UpdateDataToTable and AddDataToTable

    'START: PPP | 01/10/2017 | YRS-AT-3299 
    Private Sub btnConfirmDialogYes_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYes.Click
        If hdnRecipientForDeletion.Value.Trim.Length > 0 Then
            DeleteDataFromTable(hdnRecipientForDeletion.Value.Trim)
            LoadRecipientGrid(Me.Session_Datatable_DtBenifAccount)
            ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_DELETED_SUCCESSFULLY, "ok")
            HiddenFieldDirty.Value = "false"
            hdnRecipientForDeletion.Value = String.Empty
        Else
            LoadRecipientTab()
        End If
    End Sub

    Private Sub LoadRecipientGrid(ByVal data As DataTable)
        DatagridBenificiaryList.DataSource = data
        DatagridBenificiaryList.DataBind()
        DatagridBenificiaryList.SelectedIndex = 0

        DropdownlistBeneficiarySSNo.DataSource = data
        DropdownlistBeneficiarySSNo.DataBind()

        SetRecipientSessionProperties()
    End Sub

    Private Sub LoadRecipientDetails()
        Dim beneficaryData As DataSet
        Dim row As DataRow
        Dim fundEventID, retireeID As String
        Try
            Me.Session_datatable_dtBenifAccount = Nothing

            beneficaryData = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.GetRecipientDetails(Me.String_QDRORequestID)
            If HelperFunctions.isNonEmpty(beneficaryData) Then
                dtBenifAccount = CreateDataTableBenfAccount()
                For Each recipientRow As DataRow In beneficaryData.Tables(0).Rows
                    row = dtBenifAccount.NewRow()
                    row("id") = IIf(Convert.IsDBNull(recipientRow("guiPersID")), String.Empty, Convert.ToString(recipientRow("guiPersID")))
                    row("SSNo") = IIf(Convert.IsDBNull(recipientRow("chvSSN")), String.Empty, Convert.ToString(recipientRow("chvSSN")))
                    row("LastName") = IIf(Convert.IsDBNull(recipientRow("chvLastName")), String.Empty, Convert.ToString(recipientRow("chvLastName")))
                    row("FirstName") = IIf(Convert.IsDBNull(recipientRow("chvFirstName")), String.Empty, Convert.ToString(recipientRow("chvFirstName")))
                    row("MiddleName") = IIf(Convert.IsDBNull(recipientRow("chvMiddleName")), String.Empty, Convert.ToString(recipientRow("chvMiddleName")))
                    row("FundStatus") = "RQ" 'RQ is the default fund status, So not stored in DB, hardcoded it
                    row("SuffixTitle") = IIf(Convert.IsDBNull(recipientRow("chvSuffixTitle")), String.Empty, Convert.ToString(recipientRow("chvSuffixTitle")))
                    row("BirthDate") = IIf(Convert.IsDBNull(recipientRow("dtmBirthDate")), String.Empty, Convert.ToDateTime(recipientRow("dtmBirthDate")).ToString("MM/dd/yyyy"))
                    row("SalutationCode") = IIf(Convert.IsDBNull(recipientRow("chvSalutationCode")), String.Empty, Convert.ToString(recipientRow("chvSalutationCode")))
                    row("MaritalCode") = IIf(Convert.IsDBNull(recipientRow("chvMaritalCode")), String.Empty, Convert.ToString(recipientRow("chvMaritalCode")))
                    row("GenderCode") = IIf(Convert.IsDBNull(recipientRow("chvGenderCode")), String.Empty, Convert.ToString(recipientRow("chvGenderCode")))
                    row("Address1") = IIf(Convert.IsDBNull(recipientRow("chvAddress1")), String.Empty, Convert.ToString(recipientRow("chvAddress1")))
                    row("Address2") = IIf(Convert.IsDBNull(recipientRow("chvAddress2")), String.Empty, Convert.ToString(recipientRow("chvAddress2")))
                    row("Address3") = IIf(Convert.IsDBNull(recipientRow("chvAddress3")), String.Empty, Convert.ToString(recipientRow("chvAddress3")))
                    row("City") = IIf(Convert.IsDBNull(recipientRow("chvCity")), String.Empty, Convert.ToString(recipientRow("chvCity")))
                    row("State") = IIf(Convert.IsDBNull(recipientRow("chvState")), String.Empty, Convert.ToString(recipientRow("chvState")))
                    row("Zip") = IIf(Convert.IsDBNull(recipientRow("chvZip")), String.Empty, Convert.ToString(recipientRow("chvZip")))
                    row("Country") = IIf(Convert.IsDBNull(recipientRow("chvCountry")), String.Empty, Convert.ToString(recipientRow("chvCountry")))
                    row("Address_effectiveDate") = IIf(Convert.IsDBNull(recipientRow("dtmAddressEffectiveDate")), DateTime.MinValue, Convert.ToDateTime(recipientRow("dtmAddressEffectiveDate")))
                    row("BadAddress") = False
                    If Not Convert.IsDBNull(recipientRow("bitIsBadAddress")) Then
                        row("BadAddress") = Convert.ToBoolean(recipientRow("bitIsBadAddress"))
                    End If
                    row("AddressNote") = IIf(Convert.IsDBNull(recipientRow("chvAddressNote")), String.Empty, Convert.ToString(recipientRow("chvAddressNote")))
                    row("AddressNote_bitImportant") = False
                    If Not Convert.IsDBNull(recipientRow("bitAddressNoteIsImportant")) Then
                        row("AddressNote_bitImportant") = Convert.ToBoolean(recipientRow("bitAddressNoteIsImportant"))
                    End If
                    row("EmailAddress") = IIf(Convert.IsDBNull(recipientRow("chvEmailAddress")), String.Empty, Convert.ToString(recipientRow("chvEmailAddress")))
                    row("PhoneNumber") = IIf(Convert.IsDBNull(recipientRow("chvPhoneNumber")), String.Empty, Convert.ToString(recipientRow("chvPhoneNumber")))
                    row("FlagNewBenf") = False
                    If Not Convert.IsDBNull(recipientRow("bitIsNew")) Then
                        row("FlagNewBenf") = Convert.ToBoolean(recipientRow("bitIsNew"))
                    End If
                    fundEventID = Guid.NewGuid().ToString()
                    Me.String_RecptFundEventID = fundEventID
                    row("FundEventId") = fundEventID

                    retireeID = Guid.NewGuid().ToString()
                    Me.String_RecptRetireeID = retireeID
                    row("RecptRetireeID") = retireeID
                    dtBenifAccount.Rows.Add(row)
                Next
                Me.Session_datatable_dtBenifAccount = dtBenifAccount
            End If
        Catch
            Throw
        Finally
            row = Nothing
            beneficaryData = Nothing
        End Try
    End Sub

    ' SetRecipientData helps to initialize session data of selected recipient
    Private Sub SetRecipientSessionProperties()
        Dim splitConfigurationRows As DataRow()
        Dim persID As String
        Try
            If DropdownlistBeneficiarySSNo.Items.Count > 0 Then
                ' Set recipient sessions
                persID = DropdownlistBeneficiarySSNo.SelectedItem.Value
                Me.string_Benif_PersonID = persID
                For i As Integer = 0 To DatagridBenificiaryList.Items.Count - 1
                    If DatagridBenificiaryList.Items(i).Cells(recipientIndexPersID).Text.ToString() = persID Then
                        Me.String_RecptFundEventID = DatagridBenificiaryList.Items(i).Cells(recipientIndexFundEventID).Text.ToString()
                        Me.String_RecptRetireeID = DatagridBenificiaryList.Items(i).Cells(recipientIndexRetireeID).Text.ToString()
                        Exit For
                    End If
                Next

                ' find out existing split details
                If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtRecptAccount) Then
                    dtRecptAccount = Me.Session_Datatable_DtRecptAccount
                    splitConfigurationRows = dtRecptAccount.Select(String.Format("RecipientPersonID='{0}'", persID))
                    If splitConfigurationRows.Length > 0 Then
                        hdnSelectedPlanType.Value = splitConfigurationRows(0)("splitType")
                    End If
                End If
            End If
        Catch
            Throw
        Finally
            persID = Nothing
            splitConfigurationRows = Nothing
        End Try
    End Sub

    Private Sub ShowHideControls()
        Dim isFirstLoad As Boolean
        Dim isPlanSelected As Boolean
        Dim isBothPlanSelected As Boolean, isRetirementSelected As Boolean, isSavingsPlanSelected As Boolean
        Dim isSplitInProgress As Boolean
        Dim isOtherPlanSplitExists As Boolean
        Dim splitConfigurationRows As DataRow()
        Try
            If QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES Then
                EnablePreviousButton(True)
                btnNext.Visible = True

                'Default, hide / disable everything
                trPlanInProgressHeader.Style("display") = "none"
                trPlanInProgressEmptyRow.Style("display") = "none"
                trAmountPercentage.Style("display") = "none"
                spanBothPlans.Style("display") = "none"
                lblBothPlans.Style("display") = "none"
                lnbRetirement.Style("display") = "none"
                lblRetirement.Style("display") = "none"
                lnbSavings.Style("display") = "none"
                lblSavings.Style("display") = "none"

                ' Disable Everything
                DataGridRecipientAnnuitiesBalance.DataSource = Nothing
                DataGridRecipientAnnuitiesBalance.DataBind()

                DatagridParticipantsBalance.DataSource = Nothing
                DatagridParticipantsBalance.DataBind()

                DataGridWorkSheet.DataSource = Nothing
                DataGridWorkSheet.DataBind()

                EnableSplitButtonSet(False)

                If hdnSelectedPlanType.Value <> "" Then
                    Me.SplitPlanTypeOption = hdnSelectedPlanType.Value.Trim
                    isPlanSelected = True
                    Select Case hdnSelectedPlanType.Value
                        Case "Both"
                            isBothPlanSelected = True
                        Case "Retirement"
                            isRetirementSelected = True
                        Case "Savings"
                            isSavingsPlanSelected = True
                    End Select
                    SetSplitProgressHeader()

                    If Not Me.Session_Datatable_DtRecptAccount Is Nothing Then
                        dtRecptAccount = Me.Session_Datatable_DtRecptAccount
                        splitConfigurationRows = dtRecptAccount.Select("RecipientPersonID='" & DropdownlistBeneficiarySSNo.SelectedItem.Value.ToString().Trim() & "'")
                        If splitConfigurationRows.Length > 0 Then
                            For Each configRow In splitConfigurationRows
                                If Convert.ToString(configRow("splitType")).ToLower() = hdnSelectedPlanType.Value.ToLower() Then
                                    isSplitInProgress = True
                                    Exit For
                                End If
                            Next
                            If Not isSplitInProgress Then
                                isOtherPlanSplitExists = True
                            End If
                        End If
                    End If
                End If

                isFirstLoad = True
                If isPlanSelected Or isSplitInProgress Or isOtherPlanSplitExists Then
                    isFirstLoad = False
                End If

                If isFirstLoad Then
                    spanBothPlans.Style("display") = "normal"
                    lnbRetirement.Style("display") = "normal"
                    lnbSavings.Style("display") = "normal"
                    Call SetControlFocusDropDown(Me.DropdownlistBeneficiarySSNo)
                    CheckBoxSpecialDividends.Enabled = False
                ElseIf isPlanSelected And Not isSplitInProgress And Not isOtherPlanSplitExists Then
                    trPlanInProgressHeader.Style("display") = "normal"
                    trPlanInProgressEmptyRow.Style("display") = "normal"
                    trAmountPercentage.Style("display") = "normal"
                    If isBothPlanSelected Then
                        lnbRetirement.Style("display") = "normal"
                        lnbSavings.Style("display") = "normal"
                        lblBothPlans.Style("display") = "normal"
                    ElseIf isRetirementSelected Then
                        spanBothPlans.Style("display") = "normal"
                        lnbSavings.Style("display") = "normal"
                        lblRetirement.Style("display") = "normal"
                    ElseIf isSavingsPlanSelected Then
                        spanBothPlans.Style("display") = "normal"
                        lnbRetirement.Style("display") = "normal"
                        lblSavings.Style("display") = "normal"
                    End If

                    If RadioButtonListSplitAmtType_Amount.Checked Then
                        TextBoxAmountWorkSheet.Enabled = True
                        TextBoxPercentageWorkSheet.Enabled = False
                        TextBoxPercentageWorkSheet.Text = "0.00"
                        RadioButtonListSplitAmtType_Percentage.Checked = False
                    Else
                        TextBoxAmountWorkSheet.Enabled = False
                        TextBoxPercentageWorkSheet.Enabled = True
                        TextBoxAmountWorkSheet.Text = "0.00"
                        RadioButtonListSplitAmtType_Amount.Checked = False
                    End If
                    If (TextBoxPercentageWorkSheet.Text <> "0.00" And TextBoxPercentageWorkSheet.Text <> "") Or (TextBoxAmountWorkSheet.Text <> "0.00" And TextBoxAmountWorkSheet.Text <> "") Then
                        ButtonSplit.Enabled = True
                    End If
                    LoadAnnuityTab()
                    CheckBoxSpecialDividends.Enabled = True
                ElseIf isSplitInProgress Then
                    ShowHideControlsAfterSplit()
                    EnableNextButton(True)
                ElseIf isOtherPlanSplitExists Then
                    ShowHideControlsForPendingSplit()
                    If (TextBoxPercentageWorkSheet.Text <> "0.00" And TextBoxPercentageWorkSheet.Text <> "") Or (TextBoxAmountWorkSheet.Text <> "0.00" And TextBoxAmountWorkSheet.Text <> "") Then
                        ButtonSplit.Enabled = True
                    End If
                    EnableNextButton(True)
                End If

                ' Handle $ and % options
                If Not isSplitInProgress Then
                    RadioButtonListSplitAmtType_Amount.Disabled = False
                    RadioButtonListSplitAmtType_Percentage.Disabled = False

                    If TextBoxPercentageWorkSheet.Text <> "0.00" And TextBoxPercentageWorkSheet.Text <> "" Then
                        TextBoxPercentageWorkSheet.Enabled = True
                    ElseIf TextBoxAmountWorkSheet.Text <> "0.00" And TextBoxAmountWorkSheet.Text <> "" Then
                        TextBoxAmountWorkSheet.Enabled = True
                    End If
                End If
                ShowOtherPlanReminderWarning()
            End If
        Catch
            Throw
        Finally
            splitConfigurationRows = Nothing
        End Try
    End Sub

    Private Sub SetSplitProgressHeader()
        Select Case hdnSelectedPlanType.Value
            Case "Both"
                lblPlanInProgressHeader.Text = "Both Plans"
            Case "Retirement"
                lblPlanInProgressHeader.Text = "Retirement Plan"
            Case "Savings"
                lblPlanInProgressHeader.Text = "Savings Plan"
        End Select
    End Sub

    Private Sub EnableSplitButtonSet(ByVal isEnabled As Boolean)
        ButtonSplit.Enabled = isEnabled
        ButtonAdjust.Enabled = isEnabled
        ButtonReset.Enabled = isEnabled
        ButtonShowBalance.Enabled = isEnabled
    End Sub

    Private Sub ShowHideControlsAfterSplit()
        Dim isBothPlanSelected As Boolean, isRetirementSelected As Boolean, isSavingsPlanSelected As Boolean
        Dim isSplitByPercentage, isSplitByAmount As Boolean
        Dim splitConfigurationValue As Decimal
        Dim splitConfigurationRows As DataRow()
        Dim recipientAnnuitiesBalance As DataTable  'MMR | 2017.01.28 |YRS-AT-3299 | Declared object variable
        Try 'MMR | 2017.01.28 |YRS-AT-3299 | Added try block for tracing
            If hdnSelectedPlanType.Value <> "" Then
                Select Case hdnSelectedPlanType.Value
                    Case "Both"
                        isBothPlanSelected = True
                    Case "Retirement"
                        isRetirementSelected = True
                    Case "Savings"
                        isSavingsPlanSelected = True
                End Select
                SetSplitProgressHeader()
            End If

            trPlanInProgressHeader.Style("display") = "normal"
            trPlanInProgressEmptyRow.Style("display") = "normal"
            trAmountPercentage.Style("display") = "normal"

            EnableSplitButtonSet(True)
            ButtonSplit.Enabled = False
            CheckBoxSpecialDividends.Enabled = False

            dtRecptAccount = Me.Session_Datatable_DtRecptAccount
            If Not dtRecptAccount Is Nothing Then
                splitConfigurationRows = dtRecptAccount.Select(String.Format("RecipientPersonID='{0}' and splitType='{1}'", Me.string_Benif_PersonID, hdnSelectedPlanType.Value))
                If splitConfigurationRows.Length > 0 Then
                    isSplitByPercentage = Convert.ToBoolean(splitConfigurationRows(0)("IsSplitPercentage"))
                    splitConfigurationValue = Convert.ToDecimal(splitConfigurationRows(0)("SplitAmount"))
                    If Not isSplitByPercentage Then
                        RadioButtonListSplitAmtType_Amount.Checked = True
                        RadioButtonListSplitAmtType_Percentage.Checked = False
                        TextBoxAmountWorkSheet.Text = splitConfigurationValue.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        TextBoxPercentageWorkSheet.Text = "0.00"
                    Else
                        RadioButtonListSplitAmtType_Percentage.Checked = True
                        RadioButtonListSplitAmtType_Amount.Checked = False
                        TextBoxPercentageWorkSheet.Text = splitConfigurationValue.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        TextBoxAmountWorkSheet.Text = "0.00"
                    End If

                    RadioButtonListSplitAmtType_Amount.Disabled = True
                    RadioButtonListSplitAmtType_Percentage.Disabled = True
                    TextBoxAmountWorkSheet.Enabled = False
                    TextBoxPercentageWorkSheet.Enabled = False

                    CheckBoxSpecialDividends.Enabled = False

                    CheckBoxSpecialDividends.Checked = splitConfigurationRows(0)("IncludeSpecialdev")
                    'START: MMR | 2017.01.28 |YRS-AT-3299 | Tacing empty split balances of recipient and participant
                    'DataGridRecipientAnnuitiesBalance.DataSource = GetRecipientAccountBySplitTypes(dtRecptAccount, hdnSelectedPlanType.Value) ' Recipient's Annuities 
                    'DataGridRecipientAnnuitiesBalance.DataBind()
                    'dtPartAccount = Me.Session_Datatable_DtPartAccount  'Participant Annuities 
                    'DatagridParticipantsBalance.DataSource = dtPartAccount
                    'DatagridParticipantsBalance.DataBind()

                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->ShowHideControlsAfterSplit", "Assigning datatable recipientAnnuitiesBalance value to DataGridRecipientAnnuitiesBalance")
                    recipientAnnuitiesBalance = GetRecipientAccountBySplitTypes(dtRecptAccount, hdnSelectedPlanType.Value) 'Recipient Annuities 
                    If HelperFunctions.isNonEmpty(recipientAnnuitiesBalance) Then
                        DataGridRecipientAnnuitiesBalance.DataSource = recipientAnnuitiesBalance
                        DataGridRecipientAnnuitiesBalance.DataBind()
                    Else
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->ShowHideControlsAfterSplit", "datatable recipientAnnuitiesBalance is empty")
                    End If
                    
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->ShowHideControlsAfterSplit", "Assigning datatable dtPartAccount value to DatagridParticipantsBalance")
                    dtPartAccount = Me.Session_Datatable_DtPartAccount  'Participant Annuities 
                    If HelperFunctions.isNonEmpty(dtPartAccount) Then
                        DatagridParticipantsBalance.DataSource = dtPartAccount
                        DatagridParticipantsBalance.DataBind()
                    Else
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->ShowHideControlsAfterSplit", "datatable dtPartAccount is empty")
                    End If
                    'END: MMR | 2017.01.28 |YRS-AT-3299 | Tacing empty split balances of recipient and participant
                End If
            End If

            If isBothPlanSelected Then
                spanBothPlans.Style("display") = "none"
                lblBothPlans.Style("display") = "none"
                lnbRetirement.Style("display") = "none"
                lblRetirement.Style("display") = "none"
                lnbSavings.Style("display") = "none"
                lblSavings.Style("display") = "none"
            ElseIf isRetirementSelected Then
                spanBothPlans.Style("display") = "none"
                lblBothPlans.Style("display") = "none"
                lnbRetirement.Style("display") = "none"
                lblRetirement.Style("display") = "none"
                lnbSavings.Style("display") = "normal"
                lblSavings.Style("display") = "none"
            ElseIf isSavingsPlanSelected Then
                spanBothPlans.Style("display") = "none"
                lblBothPlans.Style("display") = "none"
                lnbRetirement.Style("display") = "normal"
                lblRetirement.Style("display") = "none"
                lnbSavings.Style("display") = "none"
                lblSavings.Style("display") = "none"
            End If

            LoadAnnuityTab()

            ' Check how many account was selected for split, if only 1 account exists then disable the adjust button
            Dim dataGridRow As DataGridItem
            Dim accountCheckBox As CheckBox
            Dim selectedAccountCounter As Integer = 0
            For Each dataGridRow In DataGridWorkSheet.Items
                accountCheckBox = CType(dataGridRow.Cells(1).Controls(1), CheckBox)
                If accountCheckBox.Checked = True Then
                    selectedAccountCounter += 1
                End If
            Next

            If selectedAccountCounter = 1 Then
                ButtonAdjust.Enabled = False
            End If
            'START: MMR | 2017.01.28 |YRS-AT-3299 |Added catch, finally block and ending trace log
        Catch
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetiredQdro->ShowHideControlsAfterSplit", "End")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()

        End Try
        'END: MMR | 2017.01.28 |YRS-AT-3299 | Added catch, finally block and ending trace log
    End Sub


    Private Sub ShowHideControlsForPendingSplit()
        Dim isBothPlanSelected As Boolean, isRetirementSelected As Boolean, isSavingsPlanSelected As Boolean

        If hdnSelectedPlanType.Value <> "" Then
            Select Case hdnSelectedPlanType.Value
                Case "Both"
                    isBothPlanSelected = True
                Case "Retirement"
                    isRetirementSelected = True
                Case "Savings"
                    isSavingsPlanSelected = True
            End Select
            SetSplitProgressHeader()
        End If

        trPlanInProgressHeader.Style("display") = "normal"
        trPlanInProgressEmptyRow.Style("display") = "normal"
        trAmountPercentage.Style("display") = "normal"

        EnableSplitButtonSet(False)
        ' split is defined we can enable the "Show Balance" button
        ButtonShowBalance.Enabled = True
        
        CheckBoxSpecialDividends.Enabled = True
        CheckBoxSpecialDividends.Checked = False

        If isRetirementSelected Then
            spanBothPlans.Style("display") = "none"
            lblBothPlans.Style("display") = "none"
            lnbRetirement.Style("display") = "none"
            lblRetirement.Style("display") = "none"
            lnbSavings.Style("display") = "normal"
            lblSavings.Style("display") = "none"
        ElseIf isSavingsPlanSelected Then
            spanBothPlans.Style("display") = "none"
            lblBothPlans.Style("display") = "none"
            lnbRetirement.Style("display") = "normal"
            lblRetirement.Style("display") = "none"
            lnbSavings.Style("display") = "none"
            lblSavings.Style("display") = "none"
        End If

        LoadAnnuityTab()
    End Sub

    Private Sub CommonPlanLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnbBothPlans.Click, lnbRetirement.Click, lnbSavings.Click
        SetDefaultAmountPercetageSetting()
        ShowHideControls()
    End Sub

    Private Sub SetDefaultAmountPercetageSetting()
        RadioButtonListSplitAmtType_Amount.Disabled = False
        RadioButtonListSplitAmtType_Amount.Checked = False
        TextBoxAmountWorkSheet.Enabled = False
        TextBoxAmountWorkSheet.Text = "0.00"

        RadioButtonListSplitAmtType_Percentage.Disabled = False
        RadioButtonListSplitAmtType_Percentage.Checked = True
        TextBoxPercentageWorkSheet.Enabled = True
        TextBoxPercentageWorkSheet.Text = "0.00"
    End Sub

    Private Sub EnableNextButton(ByVal isEnabled As Boolean)
        btnNext.Visible = isEnabled
        btnNext.Enabled = isEnabled
    End Sub

    Private Sub EnablePreviousButton(ByVal isEnabled As Boolean)
        btnPrevious.Visible = isEnabled
        btnPrevious.Enabled = isEnabled
    End Sub

    Private Sub EnableSaveButton(ByVal isEnabled As Boolean)
        btnSave.Visible = isEnabled
        btnSave.Enabled = isEnabled
    End Sub

    Private Sub ShowActiveTab(ByVal tab As Integer)
        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES).Enabled = False
        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = False
        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = False

        EnableNextButton(False)
        EnablePreviousButton(False)
        EnableSaveButton(False)

        Me.QdroRetiredTabStrip.Items(tab).Enabled = True
        Me.ListMultiPage.SelectedIndex = tab
        Me.QdroRetiredTabStrip.SelectedIndex = tab

        Select Case tab
            Case RETIREE_QDRO_TAB_STRIP_BENEFICIARIES
                LoadRecipientTab()
            Case RETIREE_QDRO_TAB_STRIP_ANNUITIES
                ' If no recipient added then keep user on same screen
                If DatagridBenificiaryList.Items.Count = 0 Then
                    ShowModalPopupWithCustomMessage("QDRO", Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_ADD_BENEFICIARY, "error")
                    LoadRecipientTab()
                Else
                    LoadSplitAnnuitiesTab()
                End If
            Case RETIREE_QDRO_TAB_STRIP_SUMMARY
                LoadSummaryTab()
        End Select
        ShowOtherPlanReminderWarning()
    End Sub

    Private Sub LoadRecipientTab()
        Dim drDataRow As DataRow
        Dim drBenef As DataRow()
        Dim selectedrow As Integer

        If Not HelperFunctions.isNonEmpty(Me.Session_Datatable_DtBenifAccount) Then
            LoadRecipientDetails()
        End If
        LoadRecipientGrid(Me.Session_Datatable_DtBenifAccount)

        ClearControls(True)
        LockAndUnLockRecipientControls(False)
        TextBoxSSNo.Enabled = True
        Call SetControlFocus(Me.TextBoxSSNo)

        ButtonAddNewBeneficiary.Enabled = True

        ButtonCancelBeneficiary.Enabled = False
        btnAddBeneficiaryToList.Text = "Save Recipient"
        ButtonCancelBeneficiary.Enabled = False
        ButtonEditBeneficiary.Enabled = False

        If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtBenifAccount) Then
            EnableNextButton(True)
        Else
            btnNext.Visible = True
        End If
    End Sub

    Private Sub LoadSplitAnnuitiesTab()
        LockAndUnLockRecipientControls(False)
        If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtBenifAccount) Then
            ShowHideControls()
            Call SetControlFocusDropDown(Me.DropdownlistBeneficiarySSNo)
        End If
    End Sub

    Private Function GetLockReason() As String
        Dim lockData As DataSet
        Dim lockReason As String
        Try
            lockReason = String.Empty
            If Not String.IsNullOrEmpty(Me.ParticipantSSN) Then
                lockData = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetLockReasonDetails(Me.ParticipantSSN)

                If Not HelperFunctions.isNonEmpty(lockData) AndAlso lockData.Tables("GetLockReasonDetails").Rows.Count > 0 Then
                    If Not Convert.IsDBNull(lockData.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc")) Then
                        lockReason = Convert.ToString(lockData.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc")).Trim
                    End If
                End If
            End If

            If String.IsNullOrEmpty(lockReason) Then
                lockReason = Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTICIPANT_ACCOUNT_LOCK
            Else
                lockReason = String.Format(Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTICIPANT_ACCOUNT_LOCK_REASON, lockReason)
            End If

            Return lockReason
        Catch
            Throw
        Finally
            lockReason = Nothing
            lockData = Nothing
        End Try
    End Function

    ' Intimates user that other plan split is pending
    Private Sub ShowOtherPlanReminderWarning()
        Dim splitConfigurationRows, accountRows As DataRow()
        Dim isMessageToBeDisplayed As Boolean
        Dim isBothSplitExists, isRetirementSplitExists, isSavingsSplitExists As Boolean
        Dim planTypeToCheckAccounts As String
        Dim hasPositiveBalance As Boolean
        Try
            '1. Check split details exists or not
            '2. If they are exists then find out for which plan details are available
            '3. If "Both" split exists then do not show message, if single plan split exists then display message
            '4. In case of single plan split exists then check whether user began another plan split, if yes then do not show message
            '5. If no split details available and user has began other than Both plan split then display message

            isMessageToBeDisplayed = False
            isBothSplitExists = False
            isRetirementSplitExists = False
            isSavingsSplitExists = False

            ' Steps: 1, 2, 3
            If Not Me.Session_Datatable_DtRecptAccount Is Nothing And Not String.IsNullOrEmpty(Me.string_Benif_PersonID) Then
                dtRecptAccount = Me.Session_Datatable_DtRecptAccount
                splitConfigurationRows = dtRecptAccount.Select(String.Format("RecipientPersonID='{0}'", Me.string_Benif_PersonID))
                If splitConfigurationRows.Length > 0 Then
                    For Each configRow In splitConfigurationRows
                        Select Case Convert.ToString(configRow("SplitType")).ToLower()
                            Case "both"
                                isBothSplitExists = True
                            Case "retirement"
                                isRetirementSplitExists = True
                            Case "savings"
                                isSavingsSplitExists = True
                        End Select
                    Next
                End If
            End If

            If isRetirementSplitExists And isSavingsSplitExists Then
                isBothSplitExists = True
            End If

            If Not isBothSplitExists Then
                If isRetirementSplitExists And Not isSavingsSplitExists Then
                    isMessageToBeDisplayed = True
                    planTypeToCheckAccounts = "Savings"
                ElseIf Not isRetirementSplitExists And isSavingsSplitExists Then
                    isMessageToBeDisplayed = True
                    planTypeToCheckAccounts = "Retirement"
                End If
            End If

            ' Steps: 4, 5
            ' If user is on Split tab and begins other plan split then do not show message
            If ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES AndAlso QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES Then
                If isMessageToBeDisplayed Then
                    If isRetirementSplitExists And Not isSavingsSplitExists Then
                        If hdnSelectedPlanType.Value.Trim().ToLower() = "savings" Then
                            isMessageToBeDisplayed = False ' Completed retirement split and started savings plan split
                        Else
                            planTypeToCheckAccounts = "Savings"
                        End If
                    ElseIf Not isRetirementSplitExists And isSavingsSplitExists Then
                        If hdnSelectedPlanType.Value.Trim().ToLower() = "retirement" Then
                            isMessageToBeDisplayed = False ' Completed savings split and started retirement plan split
                        Else
                            planTypeToCheckAccounts = "Retirement"
                        End If
                    End If
                Else
                    If Not isBothSplitExists And Not hdnSelectedPlanType.Value.Trim() = "" And Not hdnSelectedPlanType.Value.Trim().ToLower() = "both" Then
                        isMessageToBeDisplayed = True ' Just started single plan split
                        If hdnSelectedPlanType.Value.ToLower() = "savings" Then
                            planTypeToCheckAccounts = "Retirement"
                        ElseIf hdnSelectedPlanType.Value.ToLower() = "retirement" Then
                            planTypeToCheckAccounts = "Savings"
                        End If
                    End If
                End If
            ElseIf ListMultiPage.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES AndAlso QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES Then
                If Not isBothSplitExists And Not hdnSelectedPlanType.Value.Trim() = "" And Not hdnSelectedPlanType.Value.Trim().ToLower() = "both" Then
                    isMessageToBeDisplayed = True
                    If hdnSelectedPlanType.Value.ToLower() = "savings" Then
                        planTypeToCheckAccounts = "Retirement"
                    ElseIf hdnSelectedPlanType.Value.ToLower() = "retirement" Then
                        planTypeToCheckAccounts = "Savings"
                    End If
                End If
            End If

            ' Check for accounts in other plan for which we are showing warning, if no accounts exists there then do not show message.
            If isMessageToBeDisplayed AndAlso Not String.IsNullOrEmpty(planTypeToCheckAccounts) AndAlso HelperFunctions.isNonEmpty(Me.Session_Datatable_DsPartBal) Then
                dsPartBal = Me.Session_Datatable_DsPartBal
                accountRows = dsPartBal.Tables(0).Select(String.Format("PlanType='{0}'", planTypeToCheckAccounts))
                If (accountRows.Length = 0) Then
                    isMessageToBeDisplayed = False 'No accounts exists so do not display message
                    'Else
                    '    'Check total amount
                    '    hasPositiveBalance = False
                    '    For Each accountRow As DataRow In accountRows
                    '        If Not Convert.IsDBNull(accountRow("mnyBalance")) Then
                    '            If Convert.ToDecimal(accountRow("mnyBalance")) > 0 Then
                    '                hasPositiveBalance = True
                    '                Exit For
                    '            End If
                    '        End If
                    '    Next
                    '    If Not hasPositiveBalance Then
                    '        isMessageToBeDisplayed = False
                    '    End If
                End If
            End If

            If isMessageToBeDisplayed Then
                DivPlanWarningMessage.Style("display") = "normal"
                DivPlanWarningMessage.InnerHtml = Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_OTHER_PLAN_REMINDER
            Else
                DivPlanWarningMessage.Style("display") = "none"
                DivPlanWarningMessage.InnerHtml = String.Empty
            End If
        Catch
            Throw
        Finally
            splitConfigurationRows = Nothing
        End Try
    End Sub

    Private Sub DeleteAnnuitiesDetails(ByVal recipientPersID As String)
        Dim rows() As DataRow
        Dim participantTempTableRows() As DataRow
        Dim rowCounter As Integer
        Dim recipientTempTableRow As DataRow
        Dim participantTableRow As DataRow
        Dim participantTempTable As DataTable
        Dim recipientTempTable As DataTable
        Try
            If HelperFunctions.isNonEmpty(Me.Session_Datatable_DtRecptAccount) Then
                dtPartAccount = Me.Session_Datatable_DtPartAccount 'Participant Annuities Grid
                dtRecptAccount = Me.Session_Datatable_DtRecptAccount 'Account balance grid
                dtPartOriginal = Me.Session_Datatable_DtPartTotal 'Both account balance '
                recipientTempTable = Me.Session_Datatable_DtRecptAccountTemp ' To Keep Current Recipient Balance  in the Temp Datatable
                rows = dtRecptAccount.Select(String.Format("RecipientPersonID='{0}'", recipientPersID))

                For Each participantTableRow In dtPartAccount.Rows
                    For Each recipientTempTableRow In rows
                        If participantTableRow("guiAnnuityID") = recipientTempTableRow("guiAnnuityID") Then
                            participantTableRow("SSLevelingAmt") = recipientTempTableRow("SSLevelingAmt") + participantTableRow("SSLevelingAmt")
                            participantTableRow("SSReductionAmt") = recipientTempTableRow("SSReductionAmt") + participantTableRow("SSReductionAmt")
                            participantTableRow("CurrentPayment") = recipientTempTableRow("CurrentPayment") + participantTableRow("CurrentPayment")
                            participantTableRow("EmpPreTaxCurrentPayment") = recipientTempTableRow("EmpPreTaxCurrentPayment") + participantTableRow("EmpPreTaxCurrentPayment")
                            participantTableRow("EmpPostTaxCurrentPayment") = recipientTempTableRow("EmpPostTaxCurrentPayment") + participantTableRow("EmpPostTaxCurrentPayment")
                            participantTableRow("YmcaPreTaxCurrentPayment") = recipientTempTableRow("YmcaPreTaxCurrentPayment") + participantTableRow("YmcaPreTaxCurrentPayment")
                            participantTableRow("EmpPreTaxRemainingReserves") = recipientTempTableRow("EmpPreTaxRemainingReserves") + participantTableRow("EmpPreTaxRemainingReserves")
                            participantTableRow("EmpPostTaxRemainingReserves") = recipientTempTableRow("EmpPostTaxRemainingReserves") + participantTableRow("EmpPostTaxRemainingReserves")
                            participantTableRow("YmcapreTaxRemainingReserves") = recipientTempTableRow("YmcapreTaxRemainingReserves") + participantTableRow("YmcapreTaxRemainingReserves")
                        End If
                    Next
                Next
                dtPartAccount.AcceptChanges()

                rows = dtRecptAccount.Select(String.Format("RecipientPersonID='{0}'", recipientPersID))
                For rowCounter = 0 To rows.Length - 1
                    dtRecptAccount.Rows.Remove(rows(rowCounter))
                Next
                dtRecptAccount.AcceptChanges()

                participantTempTable = dtPartAccount.Copy
                'Remove current splited Annuities balance from temp table maintained for recipient
                If HelperFunctions.isNonEmpty(recipientTempTable) Then
                    rows = recipientTempTable.Select(String.Format("RecipientPersonID='{0}'", recipientPersID))
                    For rowCounter = 0 To rows.Length - 1
                        recipientTempTable.Rows.Remove(rows(rowCounter))
                    Next
                    recipientTempTable.AcceptChanges()
                End If

                'Below logic will remove the participant Annuities Of those plan which split is not there in Recipient annuities
                If HelperFunctions.isNonEmpty(dtRecptAccount) Then
                    For Each drRowParticipantAccountbalance In participantTempTable.Rows
                        rows = dtRecptAccount.Select(String.Format("splitType='{0}'", drRowParticipantAccountbalance("splitType")))
                        If rows.Length = 0 Then
                            participantTempTableRows = dtPartAccount.Select(String.Format("splitType='{0}'", drRowParticipantAccountbalance("splitType")))
                            For rowCounter = 0 To participantTempTableRows.Length - 1
                                dtPartAccount.Rows.Remove(participantTempTableRows(rowCounter))
                            Next
                        End If
                    Next
                End If
                dtPartAccount.AcceptChanges()

                Me.Session_Datatable_DtPartAccount = dtPartAccount
                Me.Session_Datatable_DtRecptAccount = dtRecptAccount
                Me.Session_Datatable_DtRecptAccountTemp = recipientTempTable

                If HelperFunctions.isEmpty(Me.Session_Datatable_DtRecptAccount) Then
                    Me.Session_Datatable_DtRecptAccountTemp = Nothing
                    Me.Session_Datatable_DtPartAccount = Nothing
                    Me.Session_Datatable_DtRecptAccount = Nothing
                End If
            End If
        Catch
            Throw
        Finally
            rows = Nothing
            participantTempTableRows = Nothing
            recipientTempTableRow = Nothing
            participantTableRow = Nothing
            participantTempTable = Nothing
            recipientTempTable = Nothing
        End Try
    End Sub

    Private Sub FreezeAllTabs()
        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES).Enabled = False
        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_ANNUITIES).Enabled = False
        Me.QdroRetiredTabStrip.Items(RETIREE_QDRO_TAB_STRIP_SUMMARY).Enabled = False

        ButtonAddNewBeneficiary.Enabled = False

        EnableNextButton(False)
        EnablePreviousButton(False)
        EnableSaveButton(False)
        btnClose.Visible = False
        btnFinalOK.Visible = True
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            HiddenFieldDirty.Value = "true" 'PPP | 01/23/2017 | YRS-AT-3299 | Setting hidden field value
            If Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_BENEFICIARIES Then
                ShowActiveTab(RETIREE_QDRO_TAB_STRIP_ANNUITIES)
            ElseIf Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES Then
                ShowActiveTab(RETIREE_QDRO_TAB_STRIP_SUMMARY)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnNext_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            If Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_ANNUITIES Then
                ShowActiveTab(RETIREE_QDRO_TAB_STRIP_BENEFICIARIES)
            ElseIf Me.QdroRetiredTabStrip.SelectedIndex = RETIREE_QDRO_TAB_STRIP_SUMMARY Then
                ShowActiveTab(RETIREE_QDRO_TAB_STRIP_ANNUITIES)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnPrevious_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ResetSession()
            Session("Page") = "RetiredQdro"
            Response.Redirect("FindInfo.aspx?Name=RetiredQdro", False)
        Catch ex As Exception
            HelperFunctions.LogException("btnClose_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim splitRows As DataRow()
        Dim checkSecurity As String
        Dim isAllRecipientSplitExists As Boolean
        Try
            checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                ShowModalPopupWithCustomMessage("QDRO", checkSecurity, "error") 'MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "YMCA-YRS" to "QDRO"
                Exit Sub
            End If

            If Not Me.Session_Datatable_DtBenifAccount Is Nothing And Not Me.Session_Datatable_DtRecptAccount Is Nothing Then
                isAllRecipientSplitExists = True

                dtBenifAccount = Me.Session_Datatable_DtBenifAccount.Copy
                dtRecptAccount = Me.Session_Datatable_DtRecptAccount.Copy
                dtPartAccount = Me.Session_Datatable_DtPartAccount.Copy

                For Each dtBenifAccountRow As DataRow In dtBenifAccount.Rows
                    splitRows = dtRecptAccount.Select(String.Format("RecipientPersonID='{0}'", Convert.ToString(dtBenifAccountRow("id"))))
                    If splitRows.Length = 0 Then
                        isAllRecipientSplitExists = False
                    End If
                Next

                If Not isAllRecipientSplitExists Then
                    ShowConfirmationMessage(dtPartAccount, dtRecptAccount, dtBenifAccount, Resources.RetiredQDRO.MESSAGE_RETIREDQDRO_PARTIAL_BENEFICIARY_SETTLEMENT)
                Else
                    ShowConfirmationMessage(dtPartAccount, dtRecptAccount, dtBenifAccount, Nothing)
                End If
                ShowOtherPlanReminderWarning()
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnSave_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        Finally
            checkSecurity = Nothing
            splitRows = Nothing
        End Try
    End Sub

    Private Sub btnFinalOK_Click(sender As Object, e As EventArgs) Handles btnFinalOK.Click
        Try
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.LogException("btnFinalOK_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub
    'END: PPP | 01/10/2017 | YRS-AT-3299 
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim tooltip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            btnAddBeneficiaryToList.Enabled = False
            btnAddBeneficiaryToList.ToolTip = tooltip

            ButtonAddNewBeneficiary.Enabled = False
            ButtonAddNewBeneficiary.ToolTip = tooltip

            TextBoxSSNo.Enabled = False
            TextBoxSSNo.ToolTip = tooltip

            DatagridBenificiaryList.Enabled = False
            DatagridBenificiaryList.ToolTip = tooltip

            btnNext.Enabled = False
            btnNext.ToolTip = tooltip
        End If
    End Sub
    'END : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
