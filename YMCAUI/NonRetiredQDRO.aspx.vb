'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA Yrs
' FileName			:	NonRetiredQDRO.aspx.vb
' Author Name		:	Ganeswar Sahoo
' Employee ID		:	35523
' Email				:	ganeswar.sahoo@3i-infotech.com
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'Modification History
'************************************************************************************
'Modified By				Date            Description
'************************************************************************************
'Paramesh K.			Oct 18th 2008       Disabled buttons Save, Edit & Show Balance on selection of participant 
'											in the DataGridList. Bug ID : 636 & 637
'Ganeswar Sahoo			Dec 4th 2008        Change the DataSet Name l_dataset_ParticipantAccountDetail to l_dataset_PartAccountDetail
'Amit Kumar Nigam		Dec 22nd 2008		Dislaying the Message box when the mandatory fields are not mentioned in the Beneficiary Tab
'											for the Existing Beneficiaries.
'Amit Kumar Nigam		Dec 22th 1008		bug id 662
'Ganeswar Sahoo			Dec 22th 1008       Modification done  GroupA and GroupB split for Participant Amount.
'Dilip Patada			jan 28th 2009		BT-676 - QDRO validation procedure for withdawals (refund, hardship, loan, retirement) 
'											of funds within the plan to be split and the withdrawal transaction date is dated 
'											after the QDRO
'Ganeswar Sahoo			Feb 25th 2009		If the PersonalPreTax,YMCAPreTax,PersonalPostTax value equal to zero then Record 
'											should not go to the database
' Dilip Yadav			2009.09.14			YRS 5.0-699 : To allow 100% in Split percentage for QDRO 
'Neeraj Singh           12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar         17/Feb/2010         Restrict Data Archived Participants To proceed in Find list Except Person 
'Neeraj Singh           06/jun/2010         Enhancement for .net 4.0
'Neeraj Singh           07/jun/2010         review changes done
'Shashi Shekhar         10-Dec-2010         For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Priya				    10-Dec-2010:		YRS 5.0-1177/BT 588 Changes made as sate n country fill up with javascript in user control
'neeraj					23-Dec-2010         self code review : datarow datatype conversion 
'neeraj					23-dec-2010         issue id BT-708
'Shashi Shekhar			10 Feb 2011         For YRS 5.0-1236 : Need ability to freeze/lock account
'Shashi Shekhar			14 - Feb -2011      For BT-750 While QDRO split message showing wrong.
'Shashi					03 Mar. 2011		Replacing Header formating with user control (YRS 5.0-450 )
'Harshala Trimukhe	    26/04/2012			YRS 5.0-1346:Cash out plan balance <= $5,000
'Priya					08/06/2012			BT:1038 - QDRO Settlement process confirmation message.
'Bhavna Shrivastav		18-May-2012			YRS 5.0-1470: Link to Address Edit program from Person Maintenance 
'Priya					22-June-2012		YRS 5.0-1167 : correction to how QS and QW transactions are created
'Priya					26-June-2012		BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
'Priya					10-07-2012			BT:1038: QDRO Settlement process confirmation message.
'Priya					11-July-2012		(Re-Open)YRS 5.0-1346:Cash out plan balance <= $5,000 REmoving data from temptable to reset session afyer reset functionality
'Priya					12.07.2012			(Re-Open) YRS 5.0-1167 : correction to how QS and QW transactions are created. Added "If TextBoxFocus.Enabled = True Then" if condition if textbox is enabled then only add validation. For javascript error
'Priya					13-07-2012			(Re-Open)BT-1038: QDRO Settlement process confirmation message.asign value to strMsg after for each loop to show all ssno which are succes and failed for request creation
'Nikunj Patel			19-07-2012			YRS 5.0-1346:Cash out plan balance <= $5,000
'Sanjay R.              21.09.2012          BT:1060/YRS 5.0-YRS 5.0-1346: Open Form and Letter on completion of QDRO beneficiary settlement.
'Sanjay R               16.10.2012          BT:1060/YRS 5.0-YRS 5.0-1346: Handle scenario when service not able to generate reports 
'Sanjay R.              10.10.2012          BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
'Sanjeev Gupta(SG)      2012.12.05          BT-1436: YRS 5.0-1729: Parameters need for new report QDROWithdrawaletterunder5k.
'Anudeep                2012.12.18          Bt-1523 - YRS 5.0-1753 Wording change in QDRO settlement page
'Sanjeev Gupta          2013.01.10          BT-1547: In case of report failure, link should always be established in between QDRO and Withdrawal request
'Anudeep                2013.01.08          BT-1550: Additional change YRS 5.0-1346  Observation on Non Retired QDRO status tab On "OK" Button Click
'Anudeep                2013.01.09          Bt-1548:Non-Retired QDRO move the hard-coded message to resource file with the requested text.
'Anudeep                2013.04.13          BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep                2013.06.20          BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep                2013.06.20          BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep                2013.07.10          BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep                2013.07.11          BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep                2013.07.18          BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
'Sanjay R.              2013.08.05          YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Shashank P				2013.10.16			BT-2175 : Non retired Qdro split will throws error if benefeciary name is more than person table
'Shashank P             2014.04.11          BT-2238\YRS 5.0-2223:Incorrect guiPersId written to QWPR QWIN transactions
'Dinesh k               2015.02.15          BT:2804: YRS 5.0-2483:RL_2_FullRefundQDRO Letter not generating from YRS.
'Anudeep                2015.05.05          BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite
'Dinesh k               2015.05.05          BT:2699: YRS 5.0-2441 : Modifications for 403b Loans 
'Dinesh k               2015.05.13          BT:2429:YRS 5.0-2313 - Need to give a warning when doing a QDRO where a loan or Withdrawal has taken place
'Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale       2015.10.13          YRS-AT-2588: implement some basic telephone number validation Rules
'Anudeep A              2015.11.18          YRS-AT-2639: YRS enh: Withdrawals Phase2: Esign Sprint: Refund Process request prepopulate data if available - make it editable by ManagementTeam only
'Manthan Rajguru        2015.10.21          YRS-AT-2182: limit effective date for address updates -Do not allow address updates with an effective date in the future. 
'Santosh Bura           2016.07.19          YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215) 
'Pramod P. Pokale       2016.08.02          YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215) 
'Manthan Rajguru        2016.08.16          YRS-AT-2482: Add gender and marital status fields to QDRO beneficiary add screen (TrackIT 22117)
'Manthan Rajguru        2016.08.26          YRS-AT-2488 -  YRS enh: PART 1 of 4:RMD's for alternate payees (QDRO recipients) (TrackIT 22284)
'Pramod P. Pokale       2016.08.24          YRS-AT-2529 - YRS Enh: request to QDRO function ( Non-Retired) to allow different amt or percent (TrackIT 23098) 
'                                           1. Removed unused private field variables
'                                           2. Removed unused private properties
'                                           3. Removed unused functions and procedures
'                                           4. Changed and improved split, show balance, summary and saving functionality
'Pramod P. Pokale       2016.09.13          YRS-AT-1973 - not handling the 'Adjust' option correctly (QDRO) 
'Manthan Rajguru        2016.09.27          YRS-AT-2482: Add gender and marital status fields to QDRO beneficiary add screen (TrackIT 22117)
'Pramod P. Pokale       2016.09.27          YRS-AT-2529 - YRS Enh: request to QDRO function ( Non-Retired) to allow different amt or percent (TrackIT 23098) 
'Sanjay GS Rawat        2016.11.15          YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215
'Pramod P. Pokale       2016.12.09          YRS-AT-2990 - YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215
'Pramod P. Pokale       2016.11.28          YRS-AT-3145 -  YRS enh: Fees - QDRO fee processing 
'                                           YRS-AT-3265 -  YRS enh:improve usability of QDRO split screens (TrackIT 28050)
'Manthan Rajguru        2017.01.20          YRS-AT-3145 -  YRS enh: Fees - QDRO fee processing 
'                                           YRS-AT-3265 -  YRS enh:improve usability of QDRO split screens (TrackIT 28050) 
'Shilpa N               2019.03.11          YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'*********************************************************************************************************************

Imports YMCARET
Imports System.Data.SqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

'Imports System
'Imports System.Collections
'Imports System.ComponentModel
'Imports System.Data
'Imports System.Drawing
'Imports System.Threading
'Imports System.Reflection
'Imports System.Web
'Imports System.Web.SessionState
'Imports System.Web.UI
'Imports System.Web.UI.WebControls
'Imports System.Web.UI.HtmlControls
'Imports System.Data.DataRow
'Imports System.Security.Permissions
'Imports Microsoft.Practices.EnterpriseLibrary.Data
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class NonRetiredQdro
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("NonRetiredQdro.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'START: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Replaced BENEFICIARY_ID with recipientIndexPersID 
    ''Start - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Defining constant
    '#Region "Global Declaration"
    'Const BENEFICIARY_ID As Integer = 1 
    '#End Region
    ''End - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Defining constant
    'END: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Replaced BENEFICIARY_ID with recipientIndexPersID 
    'PPP | 09/15/2016 | YRS-AT-2529 | Removed controls which was not declared at design side nor used in code. 

    Protected WithEvents QdroMemberActiveTabStrip As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonDocumentSave As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonDocumentCancel As System.Web.UI.WebControls.Button 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    Protected WithEvents ButtonDocumentOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdjust As System.Web.UI.WebControls.Button
    'Protected WithEvents LabelHBeneficiarySSno As System.Web.UI.WebControls.Label 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    Protected WithEvents cboBeneficiarySSNo As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents TextboxSpouseCountry As System.Web.UI.WebControls.TextBox 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    Protected WithEvents ButtonReset As System.Web.UI.WebControls.Button
    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    'Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    'Protected WithEvents LabelSSNoList As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxSSNoList As System.Web.UI.WebControls.TextBox
    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    Protected WithEvents TextboxEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxBegDate As System.Web.UI.WebControls.TextBox
    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    'Protected WithEvents LabelFundNoList As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox 'MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    'Protected WithEvents LabelLastNameList As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxLastNameList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelFirstNameList As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxFirstNameList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxCityList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxStateList As System.Web.UI.WebControls.TextBox
    'Protected WithEvents DataGridList As System.Web.UI.WebControls.DataGrid 'MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    'Protected WithEvents ButtonFindList As System.Web.UI.WebControls.Button
    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    Protected WithEvents ButtonSplit As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxAmountWorkSheet As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPercentageWorkSheet As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridWorkSheet As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridWorkSheets As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridWorkSheet2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents PopcalendarRecDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents PopcalendarRecDate2 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents Menu2 As skmMenu.Menu
    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    'Protected WithEvents dgPager As DataGridPager 'MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    'Protected WithEvents LabelNoData As System.Web.UI.WebControls.Label
    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    'personal
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSal As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownlistSal As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelMiddleName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMiddleName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSuffix As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSuffix As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelBirthDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBirthDate As YMCAUI.DateUserControl
    Protected WithEvents LabelTel As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTel As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEmail As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents DatagridBenificiaryList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents btnShowBalance As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddBeneficiaryToList As System.Web.UI.WebControls.Button
    Protected WithEvents AddressWebUserControl1 As YMCAUI.AddressUserControl
    Protected WithEvents ButtonAddNewBeneficiary As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditBeneficiary As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonResetBeneficiary As System.Web.UI.WebControls.Button
    'Summary Tab
    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    'Protected WithEvents DataListParticipant As System.Web.UI.WebControls.DataList
    'Protected WithEvents DatagridSummaryBalList As System.Web.UI.WebControls.DataList
    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    Protected WithEvents LIstMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    'Protected WithEvents Menu1 As skmMenu.Menu
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    'SR:21.09.2012 - BT:1060/YRS 5.0-YRS 5.0-1346: Open Form and Letter on completion of QDRO beneficiary settlement.
    Protected WithEvents gvQDROStatus As System.Web.UI.WebControls.GridView
    Protected WithEvents lblNotes2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblNotes1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblQDROStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents btnStatusOK As System.Web.UI.WebControls.Button
    Protected WithEvents tblButtons As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblWarning As System.Web.UI.WebControls.Label
    'SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
    Protected WithEvents ChkAdjustInterest As System.Web.UI.WebControls.CheckBox
    'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482: Declaring controls for marital status and gender.
    Protected WithEvents DropDownListMaritalStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DropDownListGender As System.Web.UI.WebControls.DropDownList
    'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482: Declaring controls for marital status and gender.
    Protected WithEvents chkSpouse As System.Web.UI.WebControls.CheckBox 'Manthan Rajguru | 2016.08.23 | YRS-AT-2488 | Initialize checkbox

    'START: PPP | 08/26/2016 | YRS-AT-2529 | Added new controls
    Protected WithEvents lblPlanInProgressHeader As System.Web.UI.WebControls.Label
    Protected WithEvents trPlanInProgressHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPlanInProgressEmptyRow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trAmountPercentage As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnbBothPlans As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblBothPlans As System.Web.UI.WebControls.Label
    Protected WithEvents lnbRetirement As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblRetirement As System.Web.UI.WebControls.Label
    Protected WithEvents lnbSavings As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblSavings As System.Web.UI.WebControls.Label
    Protected WithEvents trAdjustInterest As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents hdnSelectedPlanType As System.Web.UI.WebControls.HiddenField
    Protected WithEvents spanBothPlans As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents RadioButtonListSplitAmtType_Amount As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents RadioButtonListSplitAmtType_Percentage As System.Web.UI.HtmlControls.HtmlInputRadioButton
    Protected WithEvents divBeneficiaryTable As System.Web.UI.HtmlControls.HtmlGenericControl
    'END: PPP | 08/26/2016 | YRS-AT-2529 | Added new controls
    Protected WithEvents DivMainMessage As System.Web.UI.HtmlControls.HtmlGenericControl 'PPP | 12/09/2016 | YRS-AT-2990 | Div control added to display negative errors on screen

    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 
    Protected WithEvents divParticipantTable As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents btnPrevious As System.Web.UI.WebControls.Button
    Protected WithEvents btnNext As System.Web.UI.WebControls.Button
    Protected WithEvents btnClose As System.Web.UI.WebControls.Button
    Protected WithEvents DivPlanWarningMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DivWarningMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DivSuccessMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents btnConfirmDialogYes As System.Web.UI.WebControls.Button
    Protected WithEvents hdnRecipientForDeletion As System.Web.UI.WebControls.HiddenField
    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 
    Protected WithEvents divBalances As System.Web.UI.HtmlControls.HtmlGenericControl 'MMR | 12/02/2016 | YRS-AT-3145 | Introduced new div control which will hold HTML control created by code behind
    Protected WithEvents divApplyFees As System.Web.UI.HtmlControls.HtmlGenericControl 'MMR | 12/02/2016 | YRS-AT-3145 | Introduced new div control which will hold HTML control created by code behind
    Protected WithEvents chkApplyFees As System.Web.UI.WebControls.CheckBox 'MMR | 12/02/2016 | YRS-AT-3145 | Introduced checkBox control for new tab manage fees
    Protected WithEvents HiddenFieldDirty As System.Web.UI.WebControls.HiddenField 'MMR | 12/02/2016 | YRS-AT-3145 | Introduced Hidden filed control for displaying confirmation message on click of close button
    Protected WithEvents hdnFees As System.Web.UI.WebControls.HiddenField 'MMR | 12/02/2016 | YRS-AT-3145 | Introduced Hidden filed control to record fees

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "VARIABLE DECLARATION"
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro                            Used In     : YMCAUI           //
    'Created By                :Ganeswar Sahoo            Modified On : 08/07/2008                          //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to declare the variables used in the class.                        //
    '***************************************************************************************************//
    'PPP | 08/26/2016 | YRS-AT-2529 | Removed unused local variables 
    Dim dtPartAccount As New DataTable
    Dim dtPecentageCount As New DataTable
    Dim dsRecipantDetails As New DataSet
    Dim l_dataset_PartAccountDetail As New DataSet
    Dim dtAcctType As New DataTable
    Dim dsTransactions As New DataSet
    Dim dsAllGroupBTransactions As New DataSet
    Dim dsAllGroupATransactions As New DataSet
    Dim dsParticipantDetails As New DataSet
    Dim dsGroupBParticipantDetails As New DataSet
    Dim dsGroupBRecipantDetails As New DataSet
    Dim dsGroupAParticipantDetails As DataSet
    Dim dsGroupARecipantDetails As New DataSet
    Dim l_dataset_AnnuityBasisDetail As New DataSet
    Dim l_dataset_GroupBAnnuityBasisDetail As New DataSet
    Dim l_dataset_GroupAAnnuityBasisDetail As New DataSet
    Dim dblGroupATotal As Decimal
    Dim dblGroupBTotal As Decimal
    Dim dsALLGroupARecipantDetails As New DataSet
    Dim dsALLGroupBRecipantDetails As New DataSet
    Dim dsALLGroupAParticipantDetails As New DataSet
    Dim dsALLGroupBParticipantDetails As New DataSet
    Dim dsAllPartAccountsDetail As New DataSet
    Dim dsAllRecipantAccountsDetail As DataSet
    Private Const l_string_member As String = "Member"
    Dim dtBenifAccount As New DataTable
    Dim dsRecipientDtls As New DataSet
    Dim dsRecipientDtlsFromBeneficiaries As New DataSet
    'Summary Tab
    Dim l_dataset_ParticipantDetail As New DataSet
    Dim dtFindBeneficiary As New DataTable
    Dim dgParticipant As New DataGrid
    'Protected currencyType As New System.Globalization.CultureInfo("en-US") ' set the cultere here based on whatever your requirements are
    Dim tblStatus As New DataTable

    'START: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Recipient grid sell index
    Dim recipientIndexPersID As Integer = 2
    Dim recipientIndexSSN As Integer = 3
    Dim recipientIndexFundEventID As Integer = 7
    'END: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Recipient grid sell index
#End Region

#Region "PROPERTIES"
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo               Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to add the properties for sessions.                                //
    '***************************************************************************************************//
    'PPP | 08/26/2016 | YRS-AT-2529 | Removed unused private properties
    'START: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
    'Private Property Session_bool_MissingFundEventId() As Boolean
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
    'END: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use

    Private Property Session_dataset_AnnuityBasisDetail() As DataSet
        Get
            If Not (Session("l_dataset_AnnuityBasisDetail")) Is Nothing Then

                Return (DirectCast(Session("l_dataset_AnnuityBasisDetail"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("l_dataset_AnnuityBasisDetail") = Value
        End Set
    End Property

    Private Property Session_dataset_GroupAAnnuityBasisDetail() As DataSet
        Get
            If Not (Session("l_dataset_GroupAAnnuityBasisDetail")) Is Nothing Then

                Return (DirectCast(Session("l_dataset_GroupAAnnuityBasisDetail"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("l_dataset_GroupAAnnuityBasisDetail") = Value
        End Set
    End Property

    Private Property Session_dataset_GroupBAnnuityBasisDetail() As DataSet
        Get
            If Not (Session("l_dataset_GroupBAnnuityBasisDetail")) Is Nothing Then

                Return (DirectCast(Session("l_dataset_GroupBAnnuityBasisDetail"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("l_dataset_GroupBAnnuityBasisDetail") = Value
        End Set
    End Property

    Private Property Session_datatable_dtPartAccount() As DataTable
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

    Private Property Session_dataset_dsAllPartAccountsDetail() As DataSet
        Get
            If Not (Session("dsAllPartAccountsDetail")) Is Nothing Then

                Return (DirectCast(Session("dsAllPartAccountsDetail"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsAllPartAccountsDetail") = Value
        End Set
    End Property

    Private Property String_LoggedUserKey() As String
        Get
            If Not Session("LoggedUserKey") Is Nothing Then
                Return Session("LoggedUserKey")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("LoggedUserKey") = Value
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

    Private Property Session_finTot() As Integer
        Get
            If Not (Session("finTot")) Is Nothing Then

                Return (DirectCast(Session("finTot"), Integer))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As Integer)
            Session("finTot") = Value
        End Set
    End Property

    Private Property Session_ComboValue() As Integer
        Get
            If Not (Session("ComboValue")) Is Nothing Then

                Return (DirectCast(Session("ComboValue"), Integer))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As Integer)
            Session("ComboValue") = Value
        End Set
    End Property

    Private Property Session_Dataset_PartAccountDetail() As DataSet
        Get
            If Not (Session("PartAccountDetail")) Is Nothing Then
                Return (DirectCast(Session("PartAccountDetail"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("PartAccountDetail") = Value
        End Set
    End Property

    Private Property string_FundEventID() As String
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

    Private Property string_PersId() As String
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

    Private Property Session_dataset_dsAllRecipantAccountsDetail() As DataSet
        Get
            If Not (Session("dsAllRecipantAccountsDetail")) Is Nothing Then
                Return (DirectCast(Session("dsAllRecipantAccountsDetail"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsAllRecipantAccountsDetail") = Value
        End Set
    End Property

    Private Property Session_dataset_dsALLGroupARecipantDetails() As DataSet
        Get
            If Not (Session("dsALLGroupARecipantDetails")) Is Nothing Then
                Return (DirectCast(Session("dsALLGroupARecipantDetails"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsALLGroupARecipantDetails") = Value
        End Set
    End Property

    Private Property Session_dataset_dsALLGroupBRecipantDetails() As DataSet
        Get
            If Not (Session("dsALLGroupBRecipantDetails")) Is Nothing Then
                Return (DirectCast(Session("dsALLGroupBRecipantDetails"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsALLGroupBRecipantDetails") = Value
        End Set
    End Property

    Private Property Session_dataset_dsALLGroupAParticipantDetails() As DataSet
        Get
            If Not (Session("dsALLGroupAParticipantDetails")) Is Nothing Then
                Return (DirectCast(Session("dsALLGroupAParticipantDetails"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsALLGroupAParticipantDetails") = Value
        End Set
    End Property

    Private Property Session_dataset_dsALLGroupBParticipantDetails() As DataSet
        Get
            If Not (Session("dsALLGroupBParticipantDetails")) Is Nothing Then
                Return (DirectCast(Session("dsALLGroupBParticipantDetails"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dsALLGroupBParticipantDetails") = Value
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

    'START: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Session variable is not in use
    'Private Property String_Part_SSN() As String
    '    Get
    '        If Not Session("Part_SSN") Is Nothing Then
    '            Return Session("Part_SSN")
    '        Else
    '            Return Nothing
    '        End If

    '    End Get
    '    Set(ByVal Value As String)
    '        Session("Part_SSN") = Value
    '    End Set
    'End Property
    'END: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Session variable is not in use

    Private Property Session_datatable_dtBenifAccount() As DataTable
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

    Private Property string_PersSSID() As String
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

    Private Property String_Benif_PersonD() As String
        Get
            If Not Session("Benif_PersonD") Is Nothing Then
                Return Session("Benif_PersonD")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("Benif_PersonD") = Value
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

    Private Property Session_datatable_dtPecentageCount() As DataTable
        Get
            If Not (Session("dtPecentageCount")) Is Nothing Then

                Return (DirectCast(Session("dtPecentageCount"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("dtPecentageCount") = Value
        End Set
    End Property

    Private Property Session_bool_NewPerson() As Boolean
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

    Private Property string_RecptPersID() As String
        Get
            If Not Session("RecptPersID") Is Nothing Then
                Return Session("RecptPersID")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As String)
            Session("RecptPersID") = Value
        End Set
    End Property

    Private Property string_RecptFundEventID() As String
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

    Private Property Session_dataset_ParticipantDetails() As DataSet
        Get
            If Not (Session("ParticipantDetails")) Is Nothing Then

                Return (DirectCast(Session("ParticipantDetails"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("ParticipantDetails") = Value
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

    Private Property Control_To_Focus_DropDown() As Control
        Get
            If Not Session("Control_To_Focus_DropDown") Is Nothing Then
                Return Session("Control_To_Focus_DropDown")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Control)
            Session("Control_To_Focus_DropDown") = Value
        End Set
    End Property

    Private Property Control_To_Focus_DateUser() As UserControl
        Get
            If Not Session("Control_To_Focus_DateUser") Is Nothing Then
                Return Session("Control_To_Focus_DateUser")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As UserControl)
            Session("Control_To_Focus_DateUser") = Value
        End Set
    End Property

    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
    'Private Property Session_String_ISCompleted() As Boolean
    '    Get
    '        If Not (Session("Session_String_ISCompleted")) Is Nothing Then

    '            Return (CType(Session("Session_String_ISCompleted"), Boolean))
    '        Else
    '            Return Nothing
    '        End If
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        Session("Session_String_ISCompleted") = Value
    '    End Set
    'End Property
    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use

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
    'SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
    Private Property AdjustInterest As Boolean
        Get
            If Not (Session("AdjustInterest")) Is Nothing Then

                Return (CType(Session("AdjustInterest"), Boolean))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As Boolean)
            Session("AdjustInterest") = Value
        End Set
    End Property
    'Ends, SR:10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 

    'SG: 2012.11.30: BT-1436: YRS 5.0-1729
    Private Property String_RefRequestID() As String
        Get
            If Not Session("RefRequestID") Is Nothing Then
                Return Session("RefRequestID")
            Else
                Return Nothing
            End If

        End Get
        Set(ByVal Value As String)
            Session("RefRequestID") = Value
        End Set
    End Property
    'SG: 2012.11.30: BT-1436: YRS 5.0-1729

    'START: MMR | 2016.12.07 | YRS-AT-3145 | Setting session to store balances and fees details for participant
    Private Property ParticipantTotalBalanceAfterSplitManageFees() As DataTable
        Get
            If Not (Session("participantTotalBalanceAfterSplitManageFees")) Is Nothing Then

                Return (DirectCast(Session("participantTotalBalanceAfterSplitManageFees"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("participantTotalBalanceAfterSplitManageFees") = Value
        End Set
    End Property
    'END: MMR | 2016.12.07 | YRS-AT-3145 | Setting session to store balances and fees details for participant

    'START: MMR | 2016.12.07 | YRS-AT-3145 | Setting session to store balances and fees details for recipient
    Private Property RecipientBalanceAfterSplitManageFees() As DataTable
        Get
            If Not (Session("recipientBalanceAfterSplitManageFees")) Is Nothing Then
                Return (DirectCast(Session("recipientBalanceAfterSplitManageFees"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("recipientBalanceAfterSplitManageFees") = Value
        End Set
    End Property

    Private Property IsZeroFeeErrorDisplayed() As Boolean
        Get
            If Not (ViewState("IsZeroFeeErrorDisplayed")) Is Nothing Then
                Return (CType(ViewState("IsZeroFeeErrorDisplayed"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsZeroFeeErrorDisplayed") = Value
        End Set
    End Property
    'END: MMR | 2016.12.07 | YRS-AT-3145 | Setting session to store balances and fees details for recipient

    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 
    Private Property IsFeeGreaterThanBalance() As Boolean
        Get
            If Not (ViewState("IsFeeGreaterThanBalance")) Is Nothing Then
                Return (CType(ViewState("IsFeeGreaterThanBalance"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsFeeGreaterThanBalance") = Value
        End Set
    End Property

    Private Property IsClickedNext() As Boolean
        Get
            If Not (ViewState("IsClickedNext")) Is Nothing Then
                Return (CType(ViewState("IsClickedNext"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsClickedNext") = Value
        End Set
    End Property

    Private Property IsClickedPrevious() As Boolean
        Get
            If Not (ViewState("IsClickedPrevious")) Is Nothing Then
                Return (CType(ViewState("IsClickedPrevious"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsClickedPrevious") = Value
        End Set
    End Property

    Private Property IsFinalSaveWarning() As Boolean
        Get
            If Not (ViewState("IsFinalSaveWarning")) Is Nothing Then
                Return (CType(ViewState("IsFinalSaveWarning"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsFinalSaveWarning") = Value
        End Set
    End Property

    Private Property IsVerifiedAddressWarning() As Boolean
        Get
            If Not (ViewState("IsVerifiedAddressWarning")) Is Nothing Then
                Return (CType(ViewState("IsVerifiedAddressWarning"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsVerifiedAddressWarning") = Value
        End Set
    End Property

    Private Property IsDisbursementOrLoanExists() As Boolean
        Get
            If Not (ViewState("IsDisbursementOrLoanExists")) Is Nothing Then
                Return (CType(ViewState("IsDisbursementOrLoanExists"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsDisbursementOrLoanExists") = Value
        End Set
    End Property

    Private Property IsWithdrawalRequestStatusNotViewed() As Boolean
        Get
            If Not (ViewState("IsWithdrawalRequestStatusViewed")) Is Nothing Then
                Return (CType(ViewState("IsWithdrawalRequestStatusViewed"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsWithdrawalRequestStatusViewed") = Value
        End Set
    End Property
    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
#End Region

#Region "PAGE LOAD"
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On : 18/06/08                        //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This class is being used to adding attribute to control,initializing    //
    '                          :session variable & checking login status of the user.                   //
    '***************************************************************************************************//
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Me.String_LoggedUserKey Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True) 'PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Registering JQuery dialog
            Session("Page") = "NonRetiredQdro" 'MMR | 2016.11.23 | YRS-AT-3145 | setting session page name to bind selected grid value when redirected to find info screen
            'If Me.Session_bool_MissingFundEventId = True Then ' PPP | 09/02/2016 | YRS-AT-2529 | It was being checked twice
            'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Not using old messagebox so cannot receive "Request.Form("OK")" value
            'If Me.Session_bool_MissingFundEventId = True Then
            '    If Request.Form("OK") = "OK" Then
            '        Me.QdroMemberActiveTabStrip.SelectedIndex = 0
            '        Me.LIstMultiPage.SelectedIndex = 0
            '        Me.Session_bool_MissingFundEventId = Nothing
            '    End If
            'End If
            'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Not using old messagebox so cannot receive "Request.Form("OK")" value
            'End If ' PPP | 09/02/2016 | YRS-AT-2529 | It was being checked twice
            'START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
            'DataGridList.PageSize = 10
            'dgPager.Grid = DataGridList
            'dgPager.PagesToDisplay = 10
            'END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
            HideDivMessageControls() 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Hiding all div controls related to messages

            If Not IsPostBack Then
                'START: PPP | 09/13/2016 | YRS-AT-1973 | Following functions will ensure sessions used in Split will be empty
                Me.String_QDRORequestID = Nothing
                ClearDataTable()
                ClearSessionData()
                'END: PPP | 09/13/2016 | YRS-AT-1973 | Following functions will ensure sessions used in Split will be empty
                LoadNonRetiredQDRODetails() 'MMR | 2016.11.23 | YRS-AT-3145 | Loading Non-retired QDRO Details
                CheckReadOnlyMode()    'Shilpa N | 03/11/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
                'Me.btnShowBalance.Attributes.Add("onclick", "javascript:return CheckAccess('btnShowBalance');") ' PPP | 08/29/2016 | YRS-AT-2529 | OnClick attributes is getting new value later in the code, so this line was not effective in old code also
                'Me.TextBoxFundNo.Attributes.Add("onkeypress", "javascript:ValidateNumeric();") 'MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
                'Me.TextBoxSSNoList.Attributes.Add("onkeypress", "javascript:ValidateNumericSSNO(TextBoxSSNoList.value);") 'MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
                Me.TextBoxSSNo.Attributes.Add("OnKeyPress", "javascript:ValidateNumericSSNO(TextBoxSSNo.value);")
                Me.TextBoxTel.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
                Me.TextBoxAmountWorkSheet.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                Me.TextBoxAmountWorkSheet.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                Me.TextBoxPercentageWorkSheet.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                Me.TextBoxPercentageWorkSheet.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                Me.TextBoxEmail.Attributes.Add("onblur", "javascript:isValidEmail(document.Form1.all.TextBoxEmail.value);")
                'Anudeep:13.04.2013 :BT-1555:YRS 5.0-1769:Length of phone numbers
                Me.TextBoxTel.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxTel);")
                'Me.TextboxSpouseTel.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextboxSpouseTel);")
                'Control_To_Focus = TextBoxFundNo 'MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
                'ButtonDocumentSave.Enabled = False 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | ButtonDocumentSave button is by default not visible, so not required to handle "enable" property on PageLoad
                btnShowBalance.Enabled = False
                'START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
                'Me.QdroMemberActiveTabStrip.Items(1).Enabled = False 
                'Me.QdroMemberActiveTabStrip.Items(2).Enabled = False
                'Me.QdroMemberActiveTabStrip.Items(3).Enabled = False
                'Me.QdroMemberActiveTabStrip.Items(4).Enabled = False    'SR.:21.09.2012 -BT:1060/YRS 5.0-YRS 5.0-1346:Added
                'dgPager.Visible = False
                'END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
                ButtonResetBeneficiary.Enabled = False
                ButtonAddNewBeneficiary.Enabled = False
                ButtonEditBeneficiary.Enabled = False

                'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

                'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Loading marital status and gender
                PopulateMaritalStatusDropDownList()
                PopulateGenderDropDownList()
                'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
                'EnableDisableGenderMaritalControl(False)
                ManageEditableControls(False)
                'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2488| Loading marital status and gender             
                'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.

                'Me.String_Part_SSN = Nothing 'PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Session variable is not in use
                'SR:10.10.2012 - BT:1046/YRS 5.0-1603: Assining initial value to  AdjustInterest
                AdjustInterest = False
            End If
            'START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
            'TextBoxSSNoList.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'TextBoxFirstNameList.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'TextBoxFundNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'TextBoxLastNameList.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFindList.UniqueID + "').click();return false;}} else {return true}; ")
            'END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required

            Menu2.DataSource = Server.MapPath("SimpleXML.xml")
            Menu2.DataBind()
            'START: PPP | 08/29/2016 | YRS-AT-2529 
            'btnShowBalance.Attributes.Add("onclick", "javascript: return NewWindow('ShowBalancesQDRONonRetired.aspx','mywin','750','520','yes','center');")
            btnShowBalance.Attributes.Add("onclick", "javascript: NewWindow('ShowBalancesQDRONonRetired.aspx','mywin','750','520','yes','center'); return false;")
            'LabelAmountWorkSheet.AssociatedControlID = TextBoxAmountWorkSheet.ID
            'LabelPercentageWorkSheet.AssociatedControlID = TextBoxPercentageWorkSheet.ID
            'END: PPP | 08/29/2016 | YRS-AT-2529 

            'START: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | These controls are not in use and removed from page
            'LabelSSNoList.AssociatedControlID = TextBoxSSNoList.ID
            'LabelFundNoList.AssociatedControlID = TextBoxFundNo.ID
            'LabelLastNameList.AssociatedControlID = TextBoxLastNameList.ID
            'LabelFirstNameList.AssociatedControlID = TextBoxFirstNameList.ID
            'END: PPP | 01/03/2017 | YRS-AT-3145 & 3265 | These controls are not in use and removed from page

            LabelSSNo.AssociatedControlID = TextBoxSSNo.ID
            LabelSal.AssociatedControlID = DropdownlistSal.ID
            LabelFirstName.AssociatedControlID = TextBoxFirstName.ID
            LabelMiddleName.AssociatedControlID = TextBoxMiddleName.ID
            LabelLastName.AssociatedControlID = TextBoxLastName.ID
            LabelSuffix.AssociatedControlID = TextBoxSuffix.ID
            LabelTel.AssociatedControlID = TextBoxTel.ID
            LabelEmail.AssociatedControlID = TextBoxEmail.ID

            If Me.String_PhonySSNo = "Not_Phony_SSNo" Then
                Me.String_PhonySSNo = Nothing
                Call SetControlFocus(Me.TextBoxSSNo)
            End If

            'START: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Yes, No or any other handling is not required, becuase old Messagebox.Show is not in use
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

            'If Request.Form("Yes") = "Yes" Then
            '    If Not Me.Session_datatable_dtPecentageCount Is Nothing And Me.Session_String_ISCompleted = True Then
            '        If Me.Session_datatable_dtPecentageCount.Rows.Count > 0 Then
            '            'PPP | 08/29/2016 | YRS-AT-2529 | In old code Session_datatable_dtTempRecipientAccount was being checked instead of Session_datatable_dtPecentageCount
            '            SaveNonRetiredSplit()
            '            Exit Sub
            '        Else
            '            Call ClearControls(True)
            '            Call SetControlFocus(Me.TextBoxSSNo)
            '            TextBoxSSNo.Enabled = True
            '            ButtonAddNewBeneficiary.Enabled = False
            '        End If
            '    Else
            '        Call ClearControls(True)
            '        Call SetControlFocus(Me.TextBoxSSNo)
            '        TextBoxSSNo.Enabled = True
            '        ButtonAddNewBeneficiary.Enabled = False
            '    End If
            '    'Priya 26-June-2012	BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
            '    If Session("showCourrentBalValidation") = "TRUE" Then
            '        Split()
            '        Session("showCourrentBalValidation") = Nothing
            '    End If
            '    'END Priya	26-June-2012	BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
            '    If ViewState("Status") = "Status" Then
            '        Response.Redirect("MainWebForm.aspx", False)
            '        Me.Session_String_ISCompleted = False
            '        ClearDataTable()
            '        ClearSessionData()
            '        Exit Sub
            '    End If
            'End If

            'If Request.Form("Yes") = "Yes" OrElse Request.Form("Ok") = "OK" OrElse Me.Session_bool_NewPerson = False OrElse Request.Form("No") = "No" Then
            '    CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = False
            '    CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = False
            '    'AddressWebUserControl1.SetValidationsForSecondary()
            '    TextBoxBirthDate.RequiredDate = False
            'Else
            '    CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = True
            '    CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = True
            '    'AddressWebUserControl1.SetValidationsForPrimary()
            '    'Priya 10-Dec-2010: Changes made as sate n country fill up with javascript in user control
            '    'Changes made in adress user control
            '    'AddressWebUserControl1.SetCountStZipCodeMandatoryOnSelection()
            '    TextBoxBirthDate.RequiredDate = True
            'End If

            'If Request.Form("No") = "No" Then
            '    If Not Me.Session_datatable_dtPecentageCount Is Nothing Then
            '        If Me.Session_datatable_dtPecentageCount.Rows.Count > 0 And Me.Session_String_ISCompleted = True Then
            '            'PPP | 08/29/2016 | YRS-AT-2529 | In old code Session_datatable_dtTempRecipientAccount was being checked instead of Session_datatable_dtPecentageCount
            '            Me.Session_String_ISCompleted = False
            '            'Me.LIstMultiPage.SelectedIndex = 2
            '            'Me.QdroMemberActiveTabStrip.SelectedIndex = 2
            '            'Me.QdroMemberActiveTabStrip.Items(3).Enabled = True
            '            'Me.QdroMemberActiveTabStrip.Items(0).Enabled = True
            '            'Me.QdroMemberActiveTabStrip.Items(1).Enabled = True
            '        Else

            '            'Me.LIstMultiPage.SelectedIndex = 1
            '            'Me.QdroMemberActiveTabStrip.SelectedIndex = 1                    
            '            If Me.Session_bool_NewPerson = True Then
            '                lockRecipientControls(False)
            '            Else
            '                lockRecipientControls(False)
            '            End If
            '            ButtonAddNewBeneficiary.Enabled = True
            '            TextBoxSSNo.Enabled = False
            '        End If
            '    Else
            '        If ViewState("Status") <> "Status" Then
            '            'START : MMR | 2016.11.30 | YRS-AT-3145 | Commented existing code and changed index value of tab strip
            '            'Me.LIstMultiPage.SelectedIndex = 1
            '            'Me.QdroMemberActiveTabStrip.SelectedIndex = 1                       
            '            'SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary)
            '            'END : MMR | 2016.11.30 | YRS-AT-3145 | Commented existing code and changed index value of tab strip
            '            'PopulateBeneficiaryData(0)
            '            If Me.Session_bool_NewPerson = True Then
            '                lockRecipientControls(False)
            '                'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            '                'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling Gender and marital dropdown control
            '                'EnableDisableGenderMaritalControl(False)
            '                ManageEditableControls(False)
            '                'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling Gender and marital dropdown control
            '                'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            '            Else
            '                lockRecipientControls(False)
            '                'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            '                'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling Gender and marital dropdown control
            '                'EnableDisableGenderMaritalControl(False)
            '                ManageEditableControls(False)
            '                'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling Gender and marital dropdown control
            '                'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            '            End If
            '            ButtonAddNewBeneficiary.Enabled = True
            '            TextBoxSSNo.Enabled = False
            '        End If
            '    End If

            '    'Priya 26-June-2012	BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
            '    If Session("showCourrentBalValidation") = "TRUE" Then
            '        Session("showCourrentBalValidation") = Nothing
            '        'Me.LIstMultiPage.SelectedIndex = 2
            '        'Me.QdroMemberActiveTabStrip.SelectedIndex = 2
            '        SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts)
            '    End If
            '    'END Priya	26-June-2012	BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
            'End If
            'END: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Yes, No or any other handling is not required, becuase old Messagebox.Show is not in use

            For iCount As Integer = 0 To dtBenifAccount.Rows.Count - 1
                Dim drBenifAccount As DataRow
                drBenifAccount = dtBenifAccount.Rows(iCount)
                cboBeneficiarySSNo.Items.Add(drBenifAccount.Item(iCount))
            Next

            If Not Me.Control_To_Focus Is Nothing Then
                Call SetControlFocus(CType(Me.Control_To_Focus, Control))
                Me.Control_To_Focus = Nothing
            End If
            'Control_To_Focus_DropDown'
            If Not Me.Control_To_Focus_DropDown Is Nothing Then
                Call SetControlFocusDropDown(CType(Me.Control_To_Focus_DropDown, Control))
                Me.Control_To_Focus_DropDown = Nothing
            End If
            '' SetControlFocusDateUser
            If Not Me.Control_To_Focus_DateUser Is Nothing Then
                Call SetControlFocusDateUser(CType(Me.Control_To_Focus_DateUser, Control))
                Me.Control_To_Focus_DateUser = Nothing
            End If

            'PPP | 08/29/2016 | YRS-AT-2529 | In old code Session_datatable_dtTempRecipientAccount was being checked here to enable/disable btnShowBalance, now it is not required

            'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
            'If Me.Session_String_ISCompleted = True Then
            '    Response.Redirect("MainWebForm.aspx", False)
            '    Me.Session_String_ISCompleted = False
            'End If
            'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
            'LabelNoData.Font.Bold = False 'MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required

            'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not using old Yes-No messages
            ''Start - Manthan Rajguru | 2016.08.16 |YRS-AT-2482 | Disabling compare Validator control
            'If Request.Form("Yes") = "Yes" OrElse Request.Form("Ok") = "OK" Then
            '    If Not Session_datatable_dtBenifAccount Is Nothing Then
            '        CType(Me.FindControl("compValidatorGender"), CompareValidator).Enabled = False
            '        CType(Me.FindControl("compValidatorMaritalStatus"), CompareValidator).Enabled = False
            '    End If
            'End If
            ''End - Manthan Rajguru | 2016.08.16 |YRS-AT-2482 | Disabling compare Validator control
            'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not using old Yes-No messages

            'START: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Showing other plan pending warning if applicable
            If LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary Then
                ShowOtherPlanReminderWarning()
            ElseIf LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts Then
                ShowOtherPlanReminderWarning()
            ElseIf LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ManageFees AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ManageFees Then
                ShowOtherPlanReminderWarning()
            ElseIf LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave Then
                ShowOtherPlanReminderWarning()
            End If
            'END: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Showing other plan pending warning if applicable
        Catch ex As Exception
            'Dim l_String_Exception_Message As String 'PPP | 01/02/2017 | Not using it
            HelperFunctions.LogException("PageLoadError", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 01/02/2017 | Directly passing exception message for redirection
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 01/02/2017 | Directly passing exception message for redirection
        End Try
    End Sub
#End Region

    'START: PPP | 09/20/2016 | YRS-AT-2529 | This will make sure that birth date textbox will remain disabled in case of no edit/add is going on
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Request.Form("No") = "No" Then
            TextBoxBirthDate.Enabled = False
        End If
    End Sub
    'END: PPP | 09/20/2016 | YRS-AT-2529 | This will make sure that birth date textbox will remain disabled in case of no edit/add is going on

#Region "PRIVATE  EEVENTS"
    'START: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Switching of Tabs are managed by Previous / Next button, so QdroMemberActiveTabStrip_SelectedIndexChange event is not required 
    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    ''Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Event Description         :This event will excecute when the user move from one Tab to other                   //
    ''***************************************************************************************************//
    'Private Sub QdroMemberActiveTabStrip_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles QdroMemberActiveTabStrip.SelectedIndexChange
    '    Try
    '        'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
    '        Dim l_button_Select As ImageButton
    '        Me.LIstMultiPage.SelectedIndex = Me.QdroMemberActiveTabStrip.SelectedIndex
    '        If QdroMemberActiveTabStrip.SelectedIndex = 2 Then
    '            'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
    '            If (TextBoxFirstNameList.Text = "" And TextBoxLastNameList.Text = "" And TextBoxFundNo.Text = "" And TextBoxSSNoList.Text = "" And TextBoxCityList.Text = "" And TextBoxStateList.Text = "") Then
    '                Me.LIstMultiPage.SelectedIndex = 0
    '                Me.QdroMemberActiveTabStrip.SelectedIndex = 0
    '                'Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '                'MessageBox.Show(PlaceHolder1, "QRDO", "Please add Beneficiary.", MessageBoxButtons.OK)
    '                MessageBox.Show(PlaceHolder1, "QRDO", GetMessageFromResource("MESSAGE_QDRO_ADD_BENEFICIARY"), MessageBoxButtons.OK)
    '                'START : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    '            ElseIf DataGridList.Items.Count = 0 Then
    '                Me.LIstMultiPage.SelectedIndex = 0
    '                Me.QdroMemberActiveTabStrip.SelectedIndex = 0
    '                'Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '                'MessageBox.Show(PlaceHolder1, "QRDO", "Please add Beneficiary.", MessageBoxButtons.OK)
    '                MessageBox.Show(PlaceHolder1, "QRDO", GetMessageFromResource("MESSAGE_QDRO_ADD_BENEFICIARY"), MessageBoxButtons.OK)
    '                Exit Sub
    '                'END : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    '            ElseIf DatagridBenificiaryList.Items.Count = 0 Then
    '                Me.LIstMultiPage.SelectedIndex = 1
    '                Me.QdroMemberActiveTabStrip.SelectedIndex = 1
    '                'Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '                'MessageBox.Show(PlaceHolder1, "QDRO", "Please add Beneficiary.", MessageBoxButtons.OK, False)
    '                MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ADD_BENEFICIARY"), MessageBoxButtons.OK, False)
    '                Exit Sub
    '            Else
    '                Me.LIstMultiPage.SelectedIndex = Me.QdroMemberActiveTabStrip.SelectedIndex
    '                If Not Me.Session_datatable_dtBenifAccount Is Nothing Then
    '                    dtBenifAccount = Me.Session_datatable_dtBenifAccount
    '                    cboBeneficiarySSNo.DataSource = dtBenifAccount
    '                    cboBeneficiarySSNo.DataBind()
    '                    DatagridBenificiaryList.SelectedIndex = Me.Session_ComboValue
    '                    If DatagridBenificiaryList.SelectedIndex = -1 Then
    '                        Me.String_Benif_PersonD = Me.DatagridBenificiaryList.Items(0).Cells(1).Text
    '                    Else
    '                        Me.String_Benif_PersonD = Me.DatagridBenificiaryList.SelectedItem.Cells(1).Text
    '                    End If
    '                    cboBeneficiarySSNo.SelectedValue = Me.String_Benif_PersonD
    '                    SetBeneficiaryData() 'PPP | 9/21/2016 | YRS-AT-2529
    '                    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
    '                End If
    '            End If
    '            'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
    '        End If
    '        If QdroMemberActiveTabStrip.SelectedIndex = 1 Then
    '            If (TextBoxFirstNameList.Text = "" And TextBoxLastNameList.Text = "" And TextBoxFundNo.Text = "" And TextBoxSSNoList.Text = "" And TextBoxCityList.Text = "" And TextBoxStateList.Text = "" And DataGridList.Items.Count > 0) Then
    '                Me.LIstMultiPage.SelectedIndex = 0
    '                Me.QdroMemberActiveTabStrip.SelectedIndex = 0
    '                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '                'MessageBox.Show(PlaceHolder1, "QRDO", "Please enter the Mandatory fields.", MessageBoxButtons.OK)
    '                MessageBox.Show(PlaceHolder1, "QRDO", GetMessageFromResource("MESSAGE_QDRO_ENTER_MANDATORY_FIELDS"), MessageBoxButtons.OK)
    '            ElseIf DataGridList.Items.Count = 0 Then
    '                Me.LIstMultiPage.SelectedIndex = 0
    '                Me.QdroMemberActiveTabStrip.SelectedIndex = 0
    '                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '                'MessageBox.Show(PlaceHolder1, "QRDO", "Please enter the Mandatory fields.", MessageBoxButtons.OK)
    '                MessageBox.Show(PlaceHolder1, "QRDO", GetMessageFromResource("MESSAGE_QDRO_ENTER_MANDATORY_FIELDS"), MessageBoxButtons.OK)
    '                Exit Sub
    '            Else
    '                Me.LIstMultiPage.SelectedIndex = Me.QdroMemberActiveTabStrip.SelectedIndex
    '                'START: PPP | 08/24/2016 | YRS-AT-2529 | Changed the signature, parameters PersID and FundEventID are not required
    '                'LoadPersonalTab(Me.string_PersId, Me.string_FundEventID)
    '                LoadPersonalTab()
    '                'END: PPP | 08/24/2016 | YRS-AT-2529 | Changed the signature, parameters PersID and FundEventID are not required
    '                Call SetControlFocus(Me.TextBoxSSNo)
    '                If Not Me.Session_datatable_dtBenifAccount Is Nothing Then
    '                    If Me.Session_datatable_dtBenifAccount.Rows.Count > 0 Then
    '                        PopulateBeneficiaryData(0)
    '                        Dim selectedrow As Integer
    '                        dtBenifAccount = Me.Session_datatable_dtBenifAccount
    '                        selectedrow = cboBeneficiarySSNo.SelectedIndex
    '                        Dim datagridbenifrow As Integer
    '                        For datagridbenifrow = 0 To DatagridBenificiaryList.Items.Count - 1
    '                            DatagridBenificiaryList.Items(datagridbenifrow).Font.Bold = False
    '                            l_button_Select = DatagridBenificiaryList.Items(datagridbenifrow).FindControl("Imagebutton1")
    '                            l_button_Select.ImageUrl = "images\select.gif"
    '                        Next
    '                        If cboBeneficiarySSNo.SelectedIndex > -1 Then
    '                            DatagridBenificiaryList.Items(selectedrow).Font.Bold = True
    '                            l_button_Select = DatagridBenificiaryList.Items(selectedrow).FindControl("Imagebutton1")
    '                            l_button_Select.ImageUrl = "images\selected.gif"
    '                            If (dtBenifAccount.Rows(selectedrow).Item("FlagNewBenf")) Then
    '                                PopulateBeneficiaryData(selectedrow)
    '                                lockRecipientControls(False)
    '                                TextBoxSSNo.Enabled = False
    '                                ButtonEditBeneficiary.Enabled = True
    '                            Else
    '                                PopulateBeneficiaryData(selectedrow)
    '                                lockRecipientControls(False)
    '                                TextBoxSSNo.Enabled = False
    '                            End If
    '                        Else
    '                            DatagridBenificiaryList.Items(0).Font.Bold = True
    '                        End If
    '                        ButtonAddNewBeneficiary.Enabled = True
    '                        ButtonResetBeneficiary.Enabled = False
    '                    End If
    '                End If
    '            End If
    '        End If
    '        'START : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    '        If QdroMemberActiveTabStrip.SelectedIndex = 0 Then
    '            If (TextBoxFirstNameList.Text = "" And TextBoxLastNameList.Text = "" And TextBoxFundNo.Text = "" And TextBoxSSNoList.Text = "" And TextBoxCityList.Text = "" And TextBoxStateList.Text = "") Then
    '            Else
    '                Me.LIstMultiPage.SelectedIndex = Me.QdroMemberActiveTabStrip.SelectedIndex
    '                Dim datagridbenifrow As Integer
    '                For datagridbenifrow = 0 To DataGridList.Items.Count - 1
    '                    DataGridList.Items(datagridbenifrow).Font.Bold = False
    '                Next
    '                For datagridbenifrow = 0 To DataGridList.Items.Count - 1
    '                    If DataGridList.Items(datagridbenifrow).Cells(4).Text.ToString = Me.string_PersSSID Then
    '                        DataGridList.Items(datagridbenifrow).Font.Bold = True
    '                        Exit For
    '                    End If
    '                Next
    '                Call SetControlFocus(Me.TextBoxFundNo)
    '            End If
    '        End If
    '        'END : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    '        If QdroMemberActiveTabStrip.SelectedIndex = 3 Then
    '            If (TextBoxFirstNameList.Text = "" And TextBoxLastNameList.Text = "" And TextBoxFundNo.Text = "" And TextBoxSSNoList.Text = "" And TextBoxCityList.Text = "" And TextBoxStateList.Text = "") Then
    '                Me.LIstMultiPage.SelectedIndex = 0
    '                Me.QdroMemberActiveTabStrip.SelectedIndex = 0
    '                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '                'MessageBox.Show(PlaceHolder1, "QRDO", "Please enter the Mandatory fields.", MessageBoxButtons.OK)
    '                MessageBox.Show(PlaceHolder1, "QRDO", GetMessageFromResource("MESSAGE_QDRO_ENTER_MANDATORY_FIELDS"), MessageBoxButtons.OK)
    '            ElseIf DataGridList.Items.Count = 0 Then
    '                Me.LIstMultiPage.SelectedIndex = 0
    '                Me.QdroMemberActiveTabStrip.SelectedIndex = 0
    '                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '                'MessageBox.Show(PlaceHolder1, "QRDO", "Please enter the Mandatory fields.", MessageBoxButtons.OK)    
    '                MessageBox.Show(PlaceHolder1, "QRDO", GetMessageFromResource("MESSAGE_QDRO_ENTER_MANDATORY_FIELDS"), MessageBoxButtons.OK)
    '                Exit Sub
    '            ElseIf DatagridBenificiaryList.Items.Count = 0 Then
    '                Me.LIstMultiPage.SelectedIndex = 1
    '                Me.QdroMemberActiveTabStrip.SelectedIndex = 1
    '                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '                'MessageBox.Show(PlaceHolder1, "QRDO", "Please enter the Mandatory fields.", MessageBoxButtons.OK)
    '                MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ENTER_MANDATORY_FIELDS"), MessageBoxButtons.OK, False)
    '                Exit Sub
    '            ElseIf Not Me.Session_datatable_dtPecentageCount Is Nothing Then 'PPP | 08/29/2016 | YRS-AT-2529 | Me.Session_datatable_dtTempRecipientAccount is not being maintained and Session_datatable_dtPecentageCount contains split configuration for each beneficiary so we can use it
    '                Me.LIstMultiPage.SelectedIndex = Me.QdroMemberActiveTabStrip.SelectedIndex
    '                LoadSummaryTab()
    '            Else
    '                Me.LIstMultiPage.SelectedIndex = 2
    '                Me.QdroMemberActiveTabStrip.SelectedIndex = 2
    '                loadAccInitital()
    '                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '                'MessageBox.Show(PlaceHolder1, "QDRO", "Split Information not Found.", MessageBoxButtons.OK, False)	                    
    '                MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_SPLIT_INFO_NOT_FOUND"), MessageBoxButtons.OK, False)
    '                Call SetControlFocusDropDown(Me.cboBeneficiarySSNo)
    '                Exit Sub
    '            End If
    '        End If

    '        'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    '        If Me.ButtonAddBeneficiaryToList.Text = "Update Beneficiary" And Me.ButtonAddBeneficiaryToList.Enabled = True Then
    '            ButtonEditBeneficiary.Enabled = False
    '        End If
    '        ShowHideControls() 'PPP | 08/24/2016 | YRS-AT-2529 
    '    Catch ex As Exception
    '        HelperFunctions.LogException("QdroMemberActiveTabStrip_SelectedIndexChange", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False) 'PPP | 08/24/2016 | YRS-AT-2529 
    '        Throw ex 'PPP | 08/24/2016 | YRS-AT-2529 
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    'END: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Switching of Tabs are managed by Previous / Next button, so QdroMemberActiveTabStrip_SelectedIndexChange event is not required 

    'START : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    ''Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Event Description         :This event will excecute when the user clicks on the find button on the list tab                   //
    ''***************************************************************************************************//
    'Private Sub ButtonFindList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFindList.Click
    '    Try
    '        Me.Session_datatable_dtBenifAccount = Nothing
    '        DatagridBenificiaryList.DataSource = Nothing
    '        DatagridBenificiaryList.DataBind()
    '        Me.string_FundEventID = Nothing
    '        Me.String_Part_SSN = Nothing
    '        TextBoxSSNoList.Text = TextBoxSSNoList.Text.Replace("-", "")
    '        Headercontrol.PageTitle = ""
    '        If (TextBoxFirstNameList.Text = "" And TextBoxLastNameList.Text = "" And TextBoxFundNo.Text = "" And TextBoxSSNoList.Text = "" And TextBoxCityList.Text = "" And TextBoxStateList.Text = "") Then
    '            'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '            'MessageBox.Show(PlaceHolder1, "QRDO", "This search will take a longer time. It is suggested that you enter a search criteria.", MessageBoxButtons.OK)
    '            MessageBox.Show(PlaceHolder1, "QRDO", GetMessageFromResource("MESSAGE_QDRO_ENTER_SEARCH_CRITERIA"), MessageBoxButtons.OK)
    '        Else
    '            LoadQDROList()
    '            Me.QdroMemberActiveTabStrip.Items(1).Enabled = False
    '            Me.QdroMemberActiveTabStrip.Items(2).Enabled = False
    '            Me.QdroMemberActiveTabStrip.Items(3).Enabled = False
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("ButtonFindList_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    ''Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Event Description         :This event will excecute when the user move from List Tab to beneficiaries tab                   //
    ''***************************************************************************************************//
    'Private Sub DataGridList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridList.SelectedIndexChanged
    '    Dim strWSMessage As String
    '    Try
    '        ClearData()
    '        ClearControlsData()


    '        If (DataGridList.SelectedItem.Cells(9).Text.Trim <> "" And DataGridList.SelectedItem.Cells(9).Text.Trim <> "System.DBNull") Then
    '            Headercontrol.FundNo = DataGridList.SelectedItem.Cells(9).Text.Trim
    '            Headercontrol.PageTitle = "QDRO Member Information"
    '        End If


    '        '----Shashi Shekhar:2010-02-17: Code to handle Archived Participants from list--------------
    '        If Me.DataGridList.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
    '            'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    '            'MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
    '            MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", GetMessageFromResource("MESSAGE_QDRO_PARTICIPANT_DATA_ARCHIVED"), MessageBoxButtons.Stop, False)
    '            Me.QdroMemberActiveTabStrip.Items(1).Enabled = False
    '            Me.QdroMemberActiveTabStrip.Items(2).Enabled = False
    '            Me.QdroMemberActiveTabStrip.Items(3).Enabled = False
    '            Headercontrol.PageTitle = String.Empty

    '            Exit Sub
    '        End If
    '        If (DataGridList.SelectedItem.Cells(1).Text.Trim <> "" And DataGridList.SelectedItem.Cells(1).Text.Trim <> "System.DBNull") Then
    '            'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
    '            'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
    '            strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(DataGridList.SelectedItem.Cells(1).Text.Trim)
    '            If strWSMessage <> "NoPending" Then
    '                Page.ClientScript.RegisterStartupScript(Me.GetType(), "YRS", "openDialog('" + strWSMessage + "','Pers');", True)
    '                Me.QdroMemberActiveTabStrip.Items(1).Enabled = False
    '                Me.QdroMemberActiveTabStrip.Items(2).Enabled = False
    '                Me.QdroMemberActiveTabStrip.Items(3).Enabled = False
    '                Headercontrol.PageTitle = String.Empty
    '                Exit Sub
    '            End If
    '            'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
    '        End If
    '        '---------------------------------------------------------------------------------------
    '        'Shashi Shekhar:10 Feb 2011:  For YRS 5.0-1236 : Need ability to freeze/lock account
    '        Me.IsAccountLock = Nothing
    '        Me.IsAccountLock = Me.DataGridList.SelectedItem.Cells(10).Text.ToUpper.Trim()

    '        Dim datagridbenifrow As Integer
    '        For datagridbenifrow = 0 To DataGridList.Items.Count - 1
    '            DataGridList.Items(datagridbenifrow).Font.Bold = False
    '        Next
    '        If DataGridList.Items.Count > 0 Then
    '            DataGridList.Items(DataGridList.SelectedIndex).Font.Bold = True
    '        End If
    '        Me.Session_datatable_dtBenifAccount = Nothing
    '        DatagridBenificiaryList.DataSource = Nothing
    '        DatagridBenificiaryList.DataBind()
    '        DatagridBenificiaryList.SelectedIndex = -1
    '        'START: PPP | 08/24/2016 | YRS-AT-2529 | Changed the signature, parameters PersID and FundEventID are not required
    '        'LoadPersonalTab(Me.DataGridList.SelectedItem.Cells(1).Text(), Me.DataGridList.SelectedItem.Cells(2).Text())
    '        LoadPersonalTab()
    '        'END: PPP | 08/24/2016 | YRS-AT-2529 | Changed the signature, parameters PersID and FundEventID are not required
    '        Me.string_FundEventID = Me.DataGridList.SelectedItem.Cells(2).Text().Trim
    '        Me.string_PersId = Me.DataGridList.SelectedItem.Cells(1).Text().Trim
    '        Me.String_Part_SSN = DataGridList.SelectedItem.Cells(4).Text.Trim + "-" + DataGridList.SelectedItem.Cells(5).Text.Trim + " " + DataGridList.SelectedItem.Cells(6).Text.Trim
    '        Me.string_PersSSID = DataGridList.SelectedItem.Cells(4).Text.Trim
    '        Me.String_QDRORequestID = Me.DataGridList.SelectedItem.Cells(3).Text().Trim
    '        'Shashi : 03 Mar. 2011: Replacing Header formating with user control (YRS 5.0-450 )

    '        Call SetControlFocus(Me.TextBoxSSNo)
    '        PopcalendarRecDate.Enabled = True
    '        PopcalendarRecDate2.Enabled = True
    '        'RadioButtonListPlanTypes.Enabled = True 'PPP | 08/24/2016 | YRS-AT-2529 | Control is removed
    '        Me.QdroMemberActiveTabStrip.Items(1).Enabled = True
    '        Me.QdroMemberActiveTabStrip.Items(2).Enabled = False
    '        Me.QdroMemberActiveTabStrip.Items(3).Enabled = False
    '        TextBoxSSNo.Enabled = True
    '        ButtonAddNewBeneficiary.Enabled = False
    '        ButtonResetBeneficiary.Enabled = False
    '        'Added by Paramesh K. On Oct 18th 2008
    '        'For disabling the Edit button when a new participant is selected from the list
    '        'Bug ID: 636
    '        '******************
    '        ButtonEditBeneficiary.Enabled = False
    '        ButtonDocumentSave.Enabled = False
    '        btnShowBalance.Enabled = False
    '        'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
    '        ButtonAddBeneficiaryToList.Text = "Add To List"
    '        clearParticipantSession()
    '        '******************
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("DataGridList_SelectedIndexChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    'END : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required

    Private Sub clearParticipantSession()
        'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
        Me.Session_dataset_AnnuityBasisDetail = Nothing
        Me.Session_dataset_dsAllPartAccountsDetail = Nothing
        Me.Session_ComboValue = Nothing
        Me.Session_finTot = Nothing
        Me.Session_Dataset_PartAccountDetail = Nothing
        Me.Session_dataset_dsAllRecipantAccountsDetail = Nothing
        Me.Session_datatable_dtBenifAccount = Nothing
        Me.Session_datatable_dtPecentageCount = Nothing
        Me.String_Benif_PersonD = Nothing
    End Sub

    'START : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    ''Created By                :Ganeswar Sahoo             Modified On : 18/06/08                       //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Event Description         :This event will change the selectd button to selected                   //
    ''***************************************************************************************************//
    'Private Sub DataGridList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridList.ItemCommand
    '    Dim cnt As Integer
    '    Dim l_button_Select As ImageButton
    '    Try
    '        For cnt = 0 To DataGridList.Items.Count - 1
    '            l_button_Select = DataGridList.Items(cnt).FindControl("ImageButtonSelect")
    '            l_button_Select.ImageUrl = "images\select.gif"
    '        Next
    '        For cnt = 0 To DataGridList.Items.Count - 1
    '            l_button_Select = e.Item.FindControl("ImageButtonSelect")
    '            If (e.Item.ItemIndex = Me.DataGridList.SelectedIndex And Me.DataGridList.SelectedIndex >= 0) Then 'Commented by kranthi 071008
    '                l_button_Select.ImageUrl = "images\selected.gif"
    '            End If
    '        Next
    '        'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'END : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo             Modified On : 18/06/08                       //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Event Description         :This event will change the selectd button to selected                   //
    '***************************************************************************************************//
    Private Sub DatagridBenificiaryList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DatagridBenificiaryList.ItemCommand
        Dim cnt As Integer
        Dim l_button_Select As ImageButton
        Dim imagedelete As ImageButton
        Try
            If e.CommandName.ToLower = "delete" Then
            Else
                For cnt = 0 To DatagridBenificiaryList.Items.Count - 1
                    l_button_Select = DatagridBenificiaryList.Items(cnt).FindControl("Imagebutton1")
                    l_button_Select.ImageUrl = "images\select.gif"
                Next
                l_button_Select = e.Item.FindControl("Imagebutton1")
                l_button_Select.ImageUrl = "images\selected.gif"
                hdnSelectedPlanType.Value = String.Empty ' This will load default page for selected beneficiary
            End If
            
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Event Description         :This event will excecute when the user checks the account types in the participant grid                   //
    '***************************************************************************************************//
    Protected Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim chkBox As CheckBox = CType(sender, CheckBox)
            Dim drPartAccount As DataRow
            Dim chkValue As String
            dtPartAccount = Me.Session_datatable_dtPartAccount ' PPP | 09/29/2016 | YRS-AT-2529 | Assignment was missing
            If dtPartAccount.Rows.Count > 1 Then
                Dim dgItem As DataGridItem = CType(chkBox.NamingContainer, DataGridItem)
                'dtPartAccount = Me.Session_datatable_dtPartAccount ' PPP | 09/29/2016 | YRS-AT-2529 | Assignment at this location is not required
                drPartAccount = dtPartAccount.Rows(CType(chkBox.NamingContainer, DataGridItem).ItemIndex)
                If chkBox.Checked = False Then
                    drPartAccount("Selected") = False
                Else
                    drPartAccount("Selected") = True
                End If
                Me.Session_datatable_dtPartAccount = dtPartAccount
                'START: PPP | 09/29/2016 | YRS-AT-2529 | Populating grid with account check/uncheck details
                DataGridWorkSheets.DataSource = dtPartAccount
                DataGridWorkSheets.DataBind()
                'END: PPP | 09/29/2016 | YRS-AT-2529 | Populating grid with account check/uncheck details
                'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
            End If
        Catch ex As Exception
            'START: PPP | 09/12/2016 | YRS-AT-2529 | This function gets evoked on checkbox check/uncheck event of Participant original balance grid, so we have to handle error instead of throwing it
            'Throw ex
            'HelperFunctions.LogException("DataListParticipant_ItemDataBound", ex) ' PPP | 09/29/2016 | YRS-AT-2529 | Function name was wrong
            HelperFunctions.LogException("Check_Clicked", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            'END: PPP | 09/12/2016 | YRS-AT-2529 | This function gets evoked on checkbox check/uncheck event of Participant original balance grid, so we have to handle error instead of throwing it
        End Try
    End Sub
    'Priya 26-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
    'To Validate Whther QDRO ED balances are greater than current balance YES NO Message create function of button click code
    Private Function Split() As String
        'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from Split() function and restructured it. Old code can be checked from TF
        Dim finalTotal As Decimal
        Try
            If TextBoxAmountWorkSheet.Enabled = True Then
                TextBoxPercentageWorkSheet.Text = "0.00"
            Else
                TextBoxAmountWorkSheet.Text = "0.00"
            End If

            DataGridWorkSheet2.DataSource = Nothing
            DataGridWorkSheet2.DataBind()

            If DataGridWorkSheets.Items.Count > 0 Then

                If IsSplitInValid() Then
                    'START: PPP | 12/09/2016 | YRS-AT-2990 | Not displaying message box, "negative balance" error message will be displayed in Div
                    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_PARTICIPANT_CURRENT_BALANCE_IS_LESS_THAN_ZERO"), MessageBoxButtons.Stop, False)
                    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'DivMainMessage.InnerText = GetMessageFromResource("MESSAGE_QDRO_PARTICIPANT_CURRENT_BALANCE_IS_LESS_THAN_ZERO")
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_PARTICIPANT_CURRENT_BALANCE_IS_LESS_THAN_ZERO", "error")
                    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'END: PPP | 12/09/2016 | YRS-AT-2990 | Not displaying message box, "negative balance" error message will be displayed in Div
                    Exit Function
                End If

                finalTotal = GetSelectedAccountsTotal()
                If finalTotal > 0 Then
                    MaintainSplitConfiguration()

                    PerformSplitOperation()
                    HandleDifferenceIssue()
                    ShowHideControls()

                    If Me.Session_finTot < 0 Then
                        Me.Session_finTot = Nothing
                        'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                        'MessageBox.Show(PlaceHolder1, "QDRO", "There is Negative money in the account for this Plan Type. Please change the date range to Split.", MessageBoxButtons.OK, False)
                        'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_NEGATIVE_AMOUNT_IN_PLAN"), MessageBoxButtons.OK, False)
                        ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_NEGATIVE_AMOUNT_IN_PLAN", "error")
                        'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        Exit Function
                    End If

                    'Me.QdroMemberActiveTabStrip.Items(3).Enabled = True 'MMR | 11/28/2016 | YRS-AT-3145 & 3265 | Tabs are handled by next and previous buttons
                Else
                    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    'MessageBox.Show(PlaceHolder1, "QDRO", "Please select the Account(s) to Split", MessageBoxButtons.OK, False)
                    'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ACCOUNT_SPLIT"), MessageBoxButtons.OK, False)
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_ACCOUNT_SPLIT", "info")
                    'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    Exit Function
                End If

                'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After every split performed by user, defined fee must be reset
                If chkApplyFees.Checked Then
                    ResetFees()
                End If
                'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After every split performed by user, defined fee must be reset
            Else
                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                'MessageBox.Show(PlaceHolder1, "QDRO", "There is no money in the account for this Plan Type. Please change the Plan type to Split the account", MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_NO_MONEY_IN_ACCOUNT"), MessageBoxButtons.OK, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_NO_MONEY_IN_ACCOUNT", "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
            End If
        Catch
            Throw
        End Try
    End Function
    'END Priya 26-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created

    'Priya 22-June-2012 : YRS 5.0-1167 : correction to how QS and QW transactions are created
    'month end of month prior to QDRO end date has not happened.
    Private Function ValidateEndOfMonth() As String
        Dim l_string_message As String
        l_string_message = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.ValidateEndOfMonth(TextboxEndDate.Text.Trim())
        'Anudeep:14.04.2013:BT-1549 : Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
        If l_string_message <> "" Then
            'l_string_message = String.Format(GetMessageFromResource("MESSAGE_QDRO_AFTER_MONTH_END"), TextboxEndDate.Text.Trim(), l_string_message.Substring(l_string_message.LastIndexOf("D") + 1, l_string_message.Length - (l_string_message.LastIndexOf("D") + 1)))
            l_string_message = String.Format(GetMessageFromResource("MESSAGE_QDRO_AFTER_MONTH_END"), TextboxEndDate.Text.Trim(), l_string_message)
        End If
        Return l_string_message
    End Function
    'End Priya 22-June-2012 : YRS 5.0-1167 : correction to how QS and QW transactions are created

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Event Description         :This event will excecute when the user clicks split buttono on the accounts tab                   //
    '***************************************************************************************************//
    Private Sub ButtonSplit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSplit.Click
        'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from Split() function and restructured it. Old code can be checked from TF
        Dim fundedUnfundedTransactions As DataSet
        Dim fundedAccountRow As DataRow
        Dim isNoDisbursementExists As Boolean
        Dim isUnfundedTransactionExists As Boolean
        Dim message As String
        Dim lockReasonDetails As DataSet
        Dim lockReason As String
        Dim showCourrentBalValidation As String
        Try
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

            'Priya 22-June-2012 : YRS 5.0-1167 : correction to how QS and QW transactions are created
            'Month end of month prior to QDRO end date has not happened.
            message = ValidateEndOfMonth()
            If message.Length > 0 Then
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "Daily Interest", message.Trim(), MessageBoxButtons.OK)
                ShowModalPopupWithCustomMessage("Daily Interest", message.Trim(), "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                ButtonSplit.Enabled = True ' PPP | 09/29/2016 | YRS-AT-2529 | In case of error, keep ButtonSplit enabled
                Exit Sub
            End If
            'END Priya 22-June-2012 : YRS 5.0-1167 : correction to how QS and QW transactions are created

            'Shashi Shekhar:10 Feb 2011:  For YRS 5.0-1236 : Need ability to freeze/lock account
            '---Shashi Shekhar :14 - Feb -2011: For BT-750 While QDRO split message showing wrong.-----------------------------------------------------------------------------------
            'Shashi Shekhar:10 Feb 2011:  For YRS 5.0-1236 : Need ability to freeze/lock account
            If Not Me.IsAccountLock = Nothing Then
                If Me.IsAccountLock.ToString.Trim.ToLower = "true" Then
                    If Not Me.string_PersSSID = String.Empty Then
                        lockReasonDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetLockReasonDetails(Me.string_PersSSID.ToString().Trim)
                    End If

                    If Not lockReasonDetails Is Nothing Then
                        If lockReasonDetails.Tables("GetLockReasonDetails").Rows.Count > 0 Then
                            If (lockReasonDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "System.DBNull" And lockReasonDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "") Then
                                lockReason = lockReasonDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString().Trim
                            End If
                        End If
                    End If
                    If lockReason = "" Then
                        'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                        'MessageBox.Show(PlaceHolder1, " YMCA - YRS", "Participant account is locked. Please refer to Customer Service Supervisor.", MessageBoxButtons.Stop, False)
                        'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        'MessageBox.Show(PlaceHolder1, " YMCA - YRS", GetMessageFromResource("MESSAGE_ACCOUNT_IS_LOCKED"), MessageBoxButtons.Stop, False)
                        ShowModalPopupMessage(" YMCA - YRS", "MESSAGE_ACCOUNT_IS_LOCKED", "error")
                        'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    Else
                        'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                        'MessageBox.Show(PlaceHolder1, " YMCA - YRS", "Participant account is locked due to " + l_reasonLock + "." + " Please refer to Customer Service Supervisor.", MessageBoxButtons.Stop, False)
                        'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        'MessageBox.Show(PlaceHolder1, " YMCA - YRS", String.Format(GetMessageFromResource("MESSAGE_QDRO_ACCOUNT_LOCKED_WITH_REASON"), lockReason), MessageBoxButtons.Stop, False)
                        ShowModalPopupWithCustomMessage(" YMCA - YRS", String.Format(GetMessageFromResource("MESSAGE_QDRO_ACCOUNT_LOCKED_WITH_REASON"), lockReason), "error")
                        'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    End If

                    ButtonSplit.Enabled = True ' PPP | 09/29/2016 | YRS-AT-2529 | In case of error, keep ButtonSplit enabled
                    Exit Sub
                End If
            End If

            '*******************************************Added by Amit-26 Nov 2008-Start
            If TextboxEndDate.Text = String.Empty Then
                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                'MessageBox.Show(PlaceHolder1, "QDRO", "Please Select the End Date", MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_SELECT_END_DATE"), MessageBoxButtons.OK, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_SELECT_END_DATE", "info")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                ButtonSplit.Enabled = True ' PPP | 09/29/2016 | YRS-AT-2529 | In case of error, keep ButtonSplit enabled
                Exit Sub
            End If
            '*******************************************Added by Amit-26 Nov 2008-End

            '2009.01.28 - BT-676 - QDRO validation procedure for withdawals (refund, hardship, loan, retirement).
            'Start:Dinesh k               2015.05.13          BT:2429:YRS 5.0-2313 - Need to give a warning when doing a QDRO where a loan or Withdrawal has taken place    
            'l_bool_ValidateDisbursements = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.ValidateDisbursements(Me.string_PersId, TextboxEndDate.Text, RadioButtonListPlanTypes.SelectedValue)
            'If l_bool_ValidateDisbursements = 0 Then
            '    'Anudeep:09.01.2013 Commented Below line added message in resource file for Bt-1548:Non-Retired QDRO move the hard-coded message to resource file with the requested text.
            '    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Funds have been withdrawn from the account after the QDRO end date.  Processing cannot continue", MessageBoxButtons.OK)
            '    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            '    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.NonRetiredQDRO.MESSAGE_QRDO_SPLIT_ACCOUNT_WITHDRAWN_TDLOAN, MessageBoxButtons.OK)
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", GetMessageFromResource("MESSAGE_QRDO_SPLIT_ACCOUNT_WITHDRAWN_TDLOAN"), MessageBoxButtons.OK)
            '    Exit Sub
            'End If
            'End:Dinesh k               2015.05.13          BT:2429:YRS 5.0-2313 - Need to give a warning when doing a QDRO where a loan or Withdrawal has taken place    
            'End 2009.01.28

            isUnfundedTransactionExists = False
            fundedUnfundedTransactions = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getFundedUnfundedTransactionsDetail(Me.string_FundEventID, TextboxBegDate.Text, TextboxEndDate.Text, hdnSelectedPlanType.Value.Trim())
            For intcnt As Integer = 0 To fundedUnfundedTransactions.Tables(0).Rows.Count - 1
                fundedAccountRow = fundedUnfundedTransactions.Tables(0).Rows(intcnt)
                If IsDBNull(fundedAccountRow.Item("FundedDate")) Then
                    isUnfundedTransactionExists = True
                    Exit For
                End If
            Next
            If isUnfundedTransactionExists Then
                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                'MessageBox.Show(PlaceHolder1, "QDRO", "Unfunded transactions exist prior to the QDRO End Date. Process cannot be completed", MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_UNFUNDED_TRANSACIONS_EXISTS"), MessageBoxButtons.OK, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_UNFUNDED_TRANSACIONS_EXISTS", "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                ButtonSplit.Enabled = True ' PPP | 09/29/2016 | YRS-AT-2529 | In case of error, keep ButtonSplit enabled
                Exit Sub
            End If
            If TextBoxPercentageWorkSheet.Text.Trim > 100 Then
                TextBoxPercentageWorkSheet.Text = "0.00"
            End If
            If TextBoxPercentageWorkSheet.Enabled And TextBoxPercentageWorkSheet.Text = "0.00" Then 'And TextBoxAmountWorkSheet.Text = "0.00" Then
                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                'MessageBox.Show(PlaceHolder1, "QDRO", "Please Enter the Split Percentage", MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_SPLIT_PERCENTAGE"), MessageBoxButtons.OK, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_SPLIT_PERCENTAGE", "info")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                ButtonSplit.Enabled = True ' PPP | 09/29/2016 | YRS-AT-2529 | In case of error, keep ButtonSplit enabled
                Exit Sub
                'START: PPP | 09/07/2016 | YRS-AT-2529 | Following condition was inside in Split() function, moved it here
            ElseIf TextBoxAmountWorkSheet.Enabled And TextBoxAmountWorkSheet.Text = "0.00" Then
                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                'MessageBox.Show(PlaceHolder1, "QDRO", "Amount Should be greater than 0", MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_AMOUNT_GREATER_THAN_ZERO"), MessageBoxButtons.OK, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_AMOUNT_GREATER_THAN_ZERO", "info")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                ButtonSplit.Enabled = True ' PPP | 09/29/2016 | YRS-AT-2529 | In case of error, keep ButtonSplit enabled
                Exit Sub
                'END: PPP | 09/07/2016 | YRS-AT-2529 | Following condition was inside in Split() function, moved it here
            End If

            'Start:Dinesh k               2015.05.13          BT:2429:YRS 5.0-2313 - Need to give a warning when doing a QDRO where a loan or Withdrawal has taken place    
            'l_bool_ValidateDisbursements = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.ValidateDisbursements(Me.string_PersId, TextboxEndDate.Text, RadioButtonListPlanTypes.SelectedValue)
            isNoDisbursementExists = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.ValidateDisbursements(Me.string_PersId, TextboxEndDate.Text, hdnSelectedPlanType.Value.Trim())
            If Not isNoDisbursementExists Then
                'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup, and replaced session with viewstate variable
                Me.IsDisbursementOrLoanExists = True
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", GetMessageFromResource("MESSAGE_QRDO_SPLIT_ACCOUNT_WITHDRAWN_TDLOAN"), MessageBoxButtons.YesNo, False)
                ShowModalPopupMessage("YMCA-YRS", "MESSAGE_QRDO_SPLIT_ACCOUNT_WITHDRAWN_TDLOAN", "infoYesNo")
                'Session("showCourrentBalValidation") = "TRUE"
                'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup, and replaced session with viewstate variable
                ButtonSplit.Enabled = True ' PPP | 09/29/2016 | YRS-AT-2529 | In case of error, keep ButtonSplit enabled
                Exit Sub
            End If
            'End:Dinesh k               2015.05.13          BT:2429:YRS 5.0-2313 - Need to give a warning when doing a QDRO where a loan or Withdrawal has taken place

            'START: PPP | 12/12/2016 | YRS-AT-2990 | Following "negative balance" checking is invalid, it checks onscreen loaded balance with actual balance which is not correct
            ''Priya 26-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
            'showCourrentBalValidation = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.ValidateQDROEDBalCuurentBalances(Me.string_FundEventID, TextboxBegDate.Text, TextboxEndDate.Text, hdnSelectedPlanType.Value)
            'If showCourrentBalValidation = "TRUE" Then
            '    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            '    'MessageBox.Show(PlaceHolder1, "QDRO", Resources.NonRetiredQDRO.MESSAGE_CURRENTBALVALIDATION, MessageBoxButtons.YesNo, False)
            '    ' // START : SB | 07/19/2016 | YRS-AT-2990 | Changing the warning message with only 'OK' button
            '    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_CURRENTBALVALIDATION"), MessageBoxButtons.YesNo, False)
            '    MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_CURRENTBALVALIDATION"), MessageBoxButtons.Stop, False)
            '    ' // END: SB | 07/19/2016 | YRS-AT-2990 | Changing the warning message with only 'OK' button 
            '    Session("showCourrentBalValidation") = "TRUE"
            '    ButtonSplit.Enabled = True ' PPP | 09/29/2016 | YRS-AT-2529 | In case of error, keep ButtonSplit enabled
            '    Exit Sub
            'End If
            'END: PPP | 12/12/2016 | YRS-AT-2990 | Following "negative balance" checking is invalid, it checks onscreen loaded balance with actual balance which is not correct

            'Added split function by removing code from button click and entered into split function 
            Split()
            'End Priya 26-June-2012 BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("ButtonSplit_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        Finally
            'START: PPP | 09/29/2016 | YRS-AT-2529 | Call to ShowHideControls is not required as only ButtonSplit had problem and it is taken care in TRY block
            'ShowHideControls() ' This will ensure control-state of split related controls
            'END: PPP | 09/29/2016 | YRS-AT-2529 | Call to ShowHideControls is not required as only ButtonSplit had problem and it is taken care in TRY block
            showCourrentBalValidation = Nothing
            lockReason = Nothing
            lockReasonDetails = Nothing
            message = Nothing
            fundedAccountRow = Nothing
            fundedUnfundedTransactions = Nothing
        End Try
    End Sub

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed $ and % text change event code from here. Old code can be checked from TF

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Event Description         :This event will excecute when the user gives the ssno on the beneficiaries tab                  //
    '***************************************************************************************************//
    Private Sub TextBoxSSNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxSSNo.TextChanged
        ' ClearData()
        Dim l_string_Message As String = ""
        Try
            Dim l_dataset_ContactInfo As New DataSet
            Dim dsAddress As New DataSet
            Dim dr As DataRow
            Dim drBeneficiary As DataRow
            HiddenFieldDirty.Value = "true" 'MMR | 2016.11.23 | YRS-AT-3145 | setting hidden field value
            TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "") '- Allowing the insertion of hyphen's in the SSN
            If Len(TextBoxSSNo.Text) <> 9 Then
                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                'MessageBox.Show(PlaceHolder1, "QDRO", "Please enter a valid SSN No.", MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ENTER_VALID_SSNO"), MessageBoxButtons.OK, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_ENTER_VALID_SSNO", "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                TextBoxSSNo.Text = ""
                Control_To_Focus = TextBoxSSNo
                ButtonAddBeneficiaryToList.Enabled = False
                Me.ClearControls(True)
                'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
                'EnableDisableGenderMaritalControl(False) 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling gender and marital control               
                ManageEditableControls(False)
                'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
                Exit Sub
            Else
                Session("NR_QDRO_PersID") = Nothing
                'modified as per the ps for non retierd qdro-start
                'START: PPP | 08/24/2016 | YRS-AT-2529 | Removed string_TextBoxPerSSNo property becuase it is not required
                'If TextBoxSSNo.Text.Equals(Me.string_TextBoxPerSSNo) Then
                If TextBoxSSNo.Text.Equals(Me.string_PersSSID) Then
                    'END: PPP | 08/24/2016 | YRS-AT-2529 | Removed string_TextBoxPerSSNo property becuase it is not required
                    'modified as per the ps for non retierd qdro-end
                    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    'MessageBox.Show(PlaceHolder1, "QDRO", "Recipient cannot have same SSN No.", MessageBoxButtons.OK, False)
                    'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_RECIEPENT_SSNO_SAME"), MessageBoxButtons.OK, False)
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_RECIEPENT_SSNO_SAME", "error")
                    'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    TextBoxSSNo.Text = ""
                    Control_To_Focus = TextBoxSSNo
                    ButtonAddBeneficiaryToList.Enabled = False

                    Me.ClearControls(True)
                    Exit Sub
                End If
                Me.ClearControls(False)
                'Start - Manthan Rajguru | 2016.08.26 | YRS-AT-2482 | Enabling gender and marital control and disabling compare Validator control               
                'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2488|  Disabling spouse control.
                'EnableDisableGenderMaritalControl(True)
                ManageEditableControls(True)
                'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
                CType(Me.FindControl("compValidatorGender"), CompareValidator).Enabled = True
                CType(Me.FindControl("compValidatorMaritalStatus"), CompareValidator).Enabled = True
                'End - Manthan Rajguru | 2016.08.16 |YRS-AT-2482 | Enabling gender and marital control and disabling compare Validator control                
                dsRecipientDtls = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getQDRORecipient(TextBoxSSNo.Text)
                If dsRecipientDtls.Tables(0).Rows.Count > 0 Then
                    dr = dsRecipientDtls.Tables(0).Rows(0)
                    Me.string_RecptPersID = dr.Item("UniqueId").ToString
                    Session("NR_QDRO_PersID") = Me.string_RecptPersID
                    Me.string_RecptFundEventID = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getQDROFundEventID(Me.string_RecptPersID, l_string_Message)
                    If Not IsDBNull(dr.Item("SalutationCode")) Then
                        If CType(dr.Item("SalutationCode"), String).Trim = "" Then
                            DropdownlistSal.SelectedIndex = 0
                        ElseIf CType(dr.Item("SalutationCode"), String).Trim = "Dr." Or CType(dr.Item("SalutationCode"), String).Trim = "Dr" Then
                            DropdownlistSal.SelectedIndex = 1
                        ElseIf CType(dr.Item("SalutationCode"), String).Trim = "Mr." Or CType(dr.Item("SalutationCode"), String).Trim = "Mr" Then
                            DropdownlistSal.SelectedIndex = 2
                        ElseIf CType(dr.Item("SalutationCode"), String).Trim = "Mrs." Or CType(dr.Item("SalutationCode"), String).Trim = "Mrs." Then
                            DropdownlistSal.SelectedIndex = 3
                        ElseIf CType(dr.Item("SalutationCode"), String).Trim = "Ms." Or CType(dr.Item("SalutationCode"), String).Trim = "Ms." Then
                            DropdownlistSal.SelectedIndex = 4
                        End If
                    End If
                    TextBoxFirstName.Text = IIf(IsDBNull(dr.Item("FirstName")), "", dr.Item("FirstName"))
                    TextBoxMiddleName.Text = IIf(IsDBNull(dr.Item("MiddleName")), "", dr.Item("MiddleName"))
                    TextBoxLastName.Text = IIf(IsDBNull(dr.Item("LastName")), "", dr.Item("LastName"))
                    TextBoxSuffix.Text = IIf(IsDBNull(dr.Item("SuffixTitle")), "", dr.Item("SuffixTitle"))
                    TextBoxBirthDate.Text = IIf(IsDBNull(dr.Item("BirthDate")), "", dr.Item("BirthDate"))
                    'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Selecting Marital status and gender for the particular person.
                    'Start - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Commented existing code as not required
                    'Start: MMR | 2016.09.27 | YRS-AT-2482 | Setting dropdown value based on condition if value from database is empty or not present in dropdown values then set to default value "SEL" or bind values
                    Dim beneficiaryGender As String = dr.Item("GenderCode").ToString()
                    If Me.DropDownListGender.Items.FindByValue(beneficiaryGender) Is Nothing Or String.IsNullOrEmpty(beneficiaryGender) Then
                        DropDownListGender.SelectedValue = "SEL"
                    Else
                        DropDownListGender.SelectedValue = beneficiaryGender
                    End If
                    'End: MMR | 2016.09.27 | YRS-AT-2482 | Setting dropdown value based on condition if value from database is empty or not present in dropdown values then set to default value "SEL" or bind values
                    'DropDownListMaritalStatus.SelectedValue = IIf(IsDBNull(dr.Item("MaritalStatusCode")), "", dr.Item("MaritalStatusCode"))
                    'End - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Commented existing code as not required
                    'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Selecting Marital status and gender for the particular person.
                    'Anudeep:12.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    'l_dataset_AddressInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAddressInfo(dr.Item("UniqueId").ToString)
                    If Not String.IsNullOrEmpty(dr.Item("UniqueId").ToString()) Then
                        dsAddress = Address.GetAddressByEntity(dr.Item("UniqueId").ToString, EnumEntityCode.PERSON)
                    End If
                    If HelperFunctions.isNonEmpty(dsAddress) AndAlso HelperFunctions.isNonEmpty(dsAddress.Tables("Address")) Then
                        AddressWebUserControl1.LoadAddressDetail(dsAddress.Tables("Address").Select("isPrimary = True"))
                        'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString() <> "") Then
                        '    Me.AddressWebUserControl1.Address1 = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString()
                        'End If
                        'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString() <> "") Then
                        '    Me.AddressWebUserControl1.Address2 = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString()
                        'End If
                        'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString() <> "") Then
                        '    Me.AddressWebUserControl1.Address3 = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString()
                        'End If
                        'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString() <> "") Then
                        '    Me.AddressWebUserControl1.City = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString()
                        'End If
                        'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString().Trim() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString().Trim() <> "") Then
                        '    If l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString().Trim() <> Nothing Then
                        '        Me.AddressWebUserControl1.DropDownListStateValue = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString().Trim()
                        '    End If
                        'End If
                        'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString().Trim() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString().Trim() <> "") Then
                        '    Me.AddressWebUserControl1.ZipCode = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString()
                        'End If
                        'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString().Trim() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString().Trim() <> "") Then
                        '    Me.AddressWebUserControl1.DropDownListCountryValue = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString().Trim()
                        'End If
                    End If
                    l_dataset_ContactInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAddressInfo(dr.Item("UniqueId").ToString)

                    If HelperFunctions.isNonEmpty(l_dataset_ContactInfo) Then
                        If l_dataset_ContactInfo.Tables.Count > 1 Then
                            If l_dataset_ContactInfo.Tables(1).Rows.Count > 0 Then
                                If (l_dataset_ContactInfo.Tables(1).Rows(0).Item("EmailAddress").ToString() <> "System.DBNull" And l_dataset_ContactInfo.Tables(1).Rows(0).Item("EmailAddress").ToString() <> "") Then
                                    Me.TextBoxEmail.Text = l_dataset_ContactInfo.Tables(1).Rows(0).Item("EmailAddress").ToString()
                                End If
                            End If
                        End If
                        If l_dataset_ContactInfo.Tables.Count > 2 Then
                            If l_dataset_ContactInfo.Tables(2).Rows.Count > 0 Then
                                If (l_dataset_ContactInfo.Tables(2).Rows(0).Item("PhoneNumber").ToString() <> "System.DBNull" And l_dataset_ContactInfo.Tables(2).Rows(0).Item("PhoneNumber").ToString() <> "") Then
                                    Me.TextBoxTel.Text = l_dataset_ContactInfo.Tables(2).Rows(0).Item("PhoneNumber").ToString()
                                End If
                            End If
                        End If
                    End If

                    'End:Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    ButtonAddBeneficiaryToList.Enabled = True
                    Me.Session_bool_NewPerson = False
                    lockRecipientControls(False)
                    TextBoxSSNo.Enabled = False
                    ButtonResetBeneficiary.Enabled = True

                    'START: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Following lines of code moved to lockRecipientControls
                    'CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = False
                    'CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = False

                    ''AddressWebUserControl1.SetValidationsForSecondary()
                    'TextBoxBirthDate.RequiredDate = False
                    'END: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Following lines of code moved to lockRecipientControls

                    Exit Sub
                Else
                    'Such a person does not exist in the system
                    Me.Session_bool_NewPerson = True
                    lockRecipientControls(True)
                    ButtonAddBeneficiaryToList.Enabled = True
                End If
                dsRecipientDtlsFromBeneficiaries = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getQDRORecipientFromBenefiary(l_string_member, TextBoxSSNo.Text)
                If dsRecipientDtlsFromBeneficiaries.Tables(0).Rows.Count > 0 Then
                    drBeneficiary = dsRecipientDtlsFromBeneficiaries.Tables(0).Rows(0)

                    Me.string_RecptPersID = drBeneficiary.Item("UniqueId").ToString
                    Session("NR_QDRO_PersID") = drBeneficiary.Item("PersID").ToString

                    If l_string_Message <> "" Then
                        'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        'MessageBox.Show(PlaceHolder1, "QDRO", l_string_Message, MessageBoxButtons.Stop, False)
                        ShowModalPopupWithCustomMessage("QDRO", l_string_Message, "error")
                        'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        'Me.Session_bool_MissingFundEventId = True 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
                        ClearControls(True)
                        ButtonAddBeneficiaryToList.Enabled = False
                        Exit Sub
                    End If
                    TextBoxFirstName.Text = IIf(IsDBNull(drBeneficiary.Item("FirstName")), "", drBeneficiary.Item("FirstName"))
                    TextBoxLastName.Text = IIf(IsDBNull(drBeneficiary.Item("LastName")), "", drBeneficiary.Item("LastName"))
                    TextBoxBirthDate.Text = IIf(IsDBNull(drBeneficiary.Item("BirthDate")), "", drBeneficiary.Item("BirthDate"))
                    Me.Session_bool_NewPerson = True
                    lockRecipientControls(True)
                    ButtonAddBeneficiaryToList.Enabled = True
                    'Anudeep:11.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    dsAddress = Address.GetBeneficiariesAddress("", TextBoxSSNo.Text, TextBoxFirstName.Text, TextBoxLastName.Text)
                    If HelperFunctions.isNonEmpty(dsAddress) Then
                        AddressWebUserControl1.LoadAddressDetail(dsAddress.Tables(0).Select("isPrimary = True"))
                    Else
                        AddressWebUserControl1.LoadAddressDetail(Nothing)
                    End If

                End If
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("TextBoxSSNo_TextChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Event Description         :This event will excecute when the user changes the start date                  //
    '***************************************************************************************************//
    Private Sub PopcalendarRecDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopcalendarRecDate.SelectionChanged
        Try
            'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
            'Start change by ganesh on 12-02-2008
            If CType(PopcalendarRecDate.SelectedDate, Date) > DateTime.Now Then
                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                'MessageBox.Show(PlaceHolder1, "QDRO", "Start Date should not be greater than Cureent Date", MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_STARTDATE_GREATERTHAN_CURRENTDATE"), MessageBoxButtons.OK, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_STARTDATE_GREATERTHAN_CURRENTDATE", "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                TextboxBegDate.Text = "12/31/1995"
                Exit Sub
            End If
            'End change by ganesh on 12-02-2008
            If TextboxEndDate.Text <> String.Empty Then
                If CType(PopcalendarRecDate.SelectedDate, Date) > CType(PopcalendarRecDate2.SelectedDate, Date) Then
                    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    'MessageBox.Show(PlaceHolder1, "QDRO", "Start Date should not be greater than End Date", MessageBoxButtons.OK, False)
                    'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_STARTDATE_GREATERTHAN_ENDDATE"), MessageBoxButtons.OK, False)
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_STARTDATE_GREATERTHAN_ENDDATE", "error")
                    'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    Exit Sub
                Else
                    If CType(PopcalendarRecDate.SelectedDate, Date) < CType("12/31/1995", Date) Then
                        TextboxBegDate.Text = "12/31/1995"
                        'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                        'MessageBox.Show(PlaceHolder1, "QDRO", "Please Enter a date greater than 12/31/1995", MessageBoxButtons.Stop, False)
                        'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ENTER_DATE_GREATERTHAN"), MessageBoxButtons.Stop, False)
                        ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_ENTER_DATE_GREATERTHAN", "error")
                        'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        LoadAccountsTab()
                        Exit Sub
                    End If
                    TextboxBegDate.Text = PopcalendarRecDate.SelectedDate
                    LoadAccountsTab()
                    Exit Sub 'PPP | 08/29/2016 | YRS-AT-2529 | Once data gets loaded into participant original balance grid then exit the procedure
                End If
            Else
                'Start change by ganesh on 12-02-2008
                If CType(PopcalendarRecDate.SelectedDate, Date) < "12/31/1995" Then
                    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    'MessageBox.Show(PlaceHolder1, "QDRO", "Start Date should not be greater than End Date", MessageBoxButtons.OK, False)
                    'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_STARTDATE_GREATERTHAN_ENDDATE"), MessageBoxButtons.OK, False)
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_STARTDATE_GREATERTHAN_ENDDATE", "error")
                    'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    Exit Sub
                Else
                    TextboxBegDate.Text = CType(PopcalendarRecDate.SelectedDate, Date)
                End If
            End If
            'End change by ganesh on 12-02-2008
            ShowHideControls() 'PPP | 08/24/2016 | YRS-AT-2529 
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("PopcalendarRecDate_SelectionChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Event Description         :This event will excecute when the user changes the end date                  //
    '***************************************************************************************************//
    Private Sub PopcalendarRecDate2_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopcalendarRecDate2.SelectionChanged
        'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from PopcalendarRecDate2_SelectionChanged. Old code can be checked from TF
        Try
            If CType(PopcalendarRecDate2.SelectedDate, Date) > Date.Today Then
                'Start change by ganesh on 12-02-2008
                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                'MessageBox.Show(PlaceHolder1, "QDRO", "End Date cannot be greater than  " & txtEndDate & ".", MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", String.Format(GetMessageFromResource("MESSAGE_QDRO_END_DATE_GREATERTHAN"), Date.Today.ToString()), MessageBoxButtons.OK, False)
                ShowModalPopupWithCustomMessage("QDRO", String.Format(GetMessageFromResource("MESSAGE_QDRO_END_DATE_GREATERTHAN"), Date.Today.ToString()), "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'End change by ganesh on 12-02-2008
                '*******************************************Added by Amit-26 Nov 2008-Start
                If TextboxEndDate.Text = String.Empty Then
                    Exit Sub
                Else
                    LoadAccountsTab()
                    Exit Sub
                End If
                '*******************************************Added by Amit-26 Nov 2008-End
            End If
            TextboxEndDate.Text = PopcalendarRecDate2.SelectedDate
            ShowHideControls() 'PPP | 08/29/2016 | YRS-AT-2529 
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("PopcalendarRecDate2_SelectionChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    'START: MMR | 2016.11.30 | YRS-AT-3145 | Commented existing code as not required
    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    ''Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Event Description         :This event will cacel all the split                   //
    ''***************************************************************************************************//
    'Private Sub ButtonDocumentCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDocumentCancel.Click
    '    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
    '    Try
    '        ClearDataTable()
    '        DatagridBenificiaryList.DataSource = Nothing
    '        DatagridBenificiaryList.DataBind()
    '        DataGridWorkSheet2.DataSource = Nothing
    '        DataGridWorkSheet2.DataBind()
    '        DataGridWorkSheet.DataSource = Nothing
    '        DataGridWorkSheet.DataBind()
    '        'DataListParticipant.DataSource = Nothing
    '        'DataListParticipant.DataBind()
    '        Me.QdroMemberActiveTabStrip.SelectedIndex = 0
    '        Me.LIstMultiPage.SelectedIndex = 0
    '        PopcalendarRecDate.Enabled = True
    '        PopcalendarRecDate2.Enabled = True
    '        TextboxBegDate.Enabled = True
    '        TextboxEndDate.Enabled = True
    '        TextBoxAmountWorkSheet.Enabled = True
    '        TextBoxPercentageWorkSheet.Enabled = True
    '        EnableSplitButtonSet(False)
    '        TextBoxAmountWorkSheet.Enabled = False
    '        TextBoxAmountWorkSheet.Text = "0.00"
    '        TextBoxPercentageWorkSheet.Text = "0.00"
    '        Me.QdroMemberActiveTabStrip.Items(3).Enabled = False
    '        Me.QdroMemberActiveTabStrip.Items(2).Enabled = False
    '        Me.QdroMemberActiveTabStrip.Items(1).Enabled = False
    '        Me.QdroMemberActiveTabStrip.Items(4).Enabled = False
    '        gvQDROStatus.DataSource = Nothing
    '        gvQDROStatus.DataBind()
    '        tblButtons.Visible = True
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("ButtonDocumentCancel_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub

    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    ''Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Event Description         :This event will excecute when the user clicks on ok button                   //
    '***************************************************************************************************//
    'Private Sub ButtonDocumentOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDocumentOK.Click
    '    'Start - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Added try catch block for Logging exception error message
    '    Try
    '        ClearDataTable()
    '        ClearSessionData()
    '        'START: MMR | 2016.09.13 | YRS-AT-2482 | Commented existing code and added parameter to avoid error while redirecting to main page
    '        'Response.Redirect("MainWebForm.aspx")
    '        'START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    '        Session("Page") = "NonRetiredQdro"
    '        'Response.Redirect("MainWebForm.aspx", False)
    '        Response.Redirect("FindInfo.aspx?Name=NonRetiredQdro", False)
    '        'START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    '        'END: MMR | 2016.09.13 | YRS-AT-2482 | Commented existing code and added parameter to avoid error while redirecting to main page
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("ButtonDocumentOK_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    '    'End - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Added try catch block for Logging exception error message
    'End Sub
    'END: MMR | 2016.11.30 | YRS-AT-3145 | Commented existing code as not required

    'START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    ''Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Event Description         :This event will clear the values in the list tab                   //
    ''***************************************************************************************************//
    'Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
    '    Try
    '        TextBoxSSNoList.Text = ""
    '        TextBoxFundNo.Text = ""
    '        TextBoxLastNameList.Text = ""
    '        TextBoxFirstNameList.Text = ""
    '        TextBoxCityList.Text = ""
    '        TextBoxStateList.Text = ""
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("ButtonClear_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = ex.Message.Trim.ToString()
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    'END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Event Description         :This event will excecute when the user move to account tab                   //
    '***************************************************************************************************//
    Private Sub DatagridBenificiaryList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DatagridBenificiaryList.SelectedIndexChanged
        Try
            Dim selectedrow As Integer
            'START: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Required columns are accessed by using placeholder variables
            Me.String_Benif_SSno = Me.DatagridBenificiaryList.SelectedItem.Cells(recipientIndexSSN).Text 'Me.DatagridBenificiaryList.SelectedItem.Cells(2).Text
            Me.String_Benif_PersonD = Me.DatagridBenificiaryList.SelectedItem.Cells(recipientIndexPersID).Text 'Me.DatagridBenificiaryList.SelectedItem.Cells(1).Text
            Me.string_RecptFundEventID = Me.DatagridBenificiaryList.SelectedItem.Cells(recipientIndexFundEventID).Text 'Me.DatagridBenificiaryList.SelectedItem.Cells(6).Text
            'END: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Required columns are accessed by using placeholder variables
            'Added by Amit. On Nov 12th 2008
            '******************
            Me.Session_ComboValue = Me.DatagridBenificiaryList.SelectedIndex
            '******************
            'START: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Following code sequence is not required
            'If Not Me.Session_datatable_dtBenifAccount Is Nothing Then
            '    dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
            '    cboBeneficiarySSNo.DataSource = dtBenifAccount
            '    cboBeneficiarySSNo.DataBind()
            '    cboBeneficiarySSNo.SelectedValue = Me.String_Benif_PersonD
            '    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.
            'End If

            'Me.QdroMemberActiveTabStrip.Items(2).Enabled = True
            'END: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Following code sequence is not required
            selectedrow = DatagridBenificiaryList.SelectedIndex
            Dim datagridbenifrow As Integer
            For datagridbenifrow = 0 To DatagridBenificiaryList.Items.Count - 1
                DatagridBenificiaryList.Items(datagridbenifrow).Font.Bold = False
            Next
            DatagridBenificiaryList.Items(selectedrow).Font.Bold = True

            'START: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Both if and else part has same conditions so removed it
            'Dim isNewRecipient As Boolean = False
            'If Not Me.Session_datatable_dtBenifAccount Is Nothing Then
            '    dtBenifAccount = CType(Me.Session_datatable_dtBenifAccount, DataTable)
            '    isNewRecipient = (dtBenifAccount.Rows(selectedrow).Item("FlagNewBenf"))
            'End If
            'If (dtBenifAccount.Rows(selectedrow).Item("FlagNewBenf")) Then
            '    PopulateBeneficiaryData(selectedrow)
            '    lockRecipientControls(False)
            '    'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            '    'EnableDisableGenderMaritalControl(False) 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Enabling Gender and Marital control
            '    ManageEditableControls(False)
            '    'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            '    TextBoxSSNo.Enabled = False
            '    'ButtonEditBeneficiery.Enabled = True 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Commented existing code

            'Else
            '    PopulateBeneficiaryData(selectedrow)
            '    lockRecipientControls(False)
            '    'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            '    'EnableDisableGenderMaritalControl(False)
            '    ManageEditableControls(False)
            '    'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            '    TextBoxSSNo.Enabled = False
            '    ButtonAddNewBeneficiary.Enabled = True
            '    'ButtonEditBeneficiery.Enabled = False 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Commented existing code
            'End If
            PopulateBeneficiaryData(selectedrow)
            lockRecipientControls(False)
            ManageEditableControls(False)
            TextBoxSSNo.Enabled = False
            ButtonAddNewBeneficiary.Enabled = True
            'END: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Both if and else part has same conditions so removed it
            ButtonEditBeneficiary.Enabled = True 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Enabling Edit beneficiary button
        Catch ex As Exception
            'Dim l_String_Exception_Message As String 'PPP | 12/28/2016 | Not using it
            HelperFunctions.LogException("DatagridBenificiaryList_SelectedIndexChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 12/28/2016 | Directly passing exception message for redirection
            'l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 12/28/2016 | Directly passing exception message for redirection
        End Try

    End Sub

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Event Description         :This event will check the validations in the beneficiaries grid in accounts tab                   //
    '***************************************************************************************************//
    Private Sub DataGridWorkSheet2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridWorkSheet2.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim dgItems As DataGridItem
                dgItems = e.Item
                Dim TextboxEmpTaxable As TextBox = CType(dgItems.Cells(1).FindControl("TextboxEmpTaxable"), TextBox)
                Dim TextboxEmpNonTaxable As TextBox = CType(dgItems.Cells(2).FindControl("TextboxEmpNonTaxable"), TextBox)
                Dim TextboxEmpInterest As TextBox = CType(dgItems.Cells(3).FindControl("TextboxEmpInterest"), TextBox)
                Dim TextboxEmpTotal As TextBox = CType(dgItems.Cells(4).FindControl("TextboxEmpTotal"), TextBox)
                Dim TextboxYMCATaxable As TextBox = CType(dgItems.Cells(5).FindControl("TextboxYMCATaxable"), TextBox)
                Dim TextboxYMCAInterest As TextBox = CType(dgItems.Cells(6).FindControl("TextboxYMCAInterest"), TextBox)
                Dim TextboxTotal As TextBox = CType(dgItems.Cells(7).FindControl("TextboxTotal"), TextBox)
                Dim TextboxAcctTotal As TextBox = CType(dgItems.Cells(8).FindControl("TextboxAcctTotal"), TextBox)

                TextboxEmpTaxable.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextboxEmpNonTaxable.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextboxEmpInterest.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextboxEmpTotal.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextboxYMCATaxable.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextboxYMCAInterest.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextboxTotal.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
                TextboxAcctTotal.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

                TextboxEmpTaxable.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextboxEmpNonTaxable.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextboxEmpInterest.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextboxEmpTotal.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextboxYMCATaxable.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextboxYMCAInterest.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextboxTotal.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
                TextboxAcctTotal.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'START: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Control DataListParticipant is commented at design level so events are not requied
    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    ''Created By                :Ganeswar Sahoo             Modified On :                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Event Description         :Used to bind the grid to datalist                    //
    ''***************************************************************************************************//
    'Private Sub DataListParticipant_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataListParticipant.ItemDataBound
    '    'PPP | 09/12/2016 | YRS-AT-2529 | Restructured DataListParticipant_ItemDataBound. Old code can be checked from TF.
    '    Dim participantAfterSplitValuesTable As DataTable
    '    Dim planTypeToDisplay As String ' PPP | 09/29/2016 | YRS-AT-2529 
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            'START: PPP | 09/29/2016 | YRS-AT-2529 | Plan will be decided based on the split's recorded till now, as well as all account balances of participant will be shown
    '            '' Always show all participants splited accounts, So passing "Both" as plan type
    '            'participantAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable("Both", Me.Session_dataset_dsAllPartAccountsDetail, Me.Session_Dataset_PartAccountDetail)
    '            planTypeToDisplay = GetParticipantPlanToShowOnSummary()
    '            participantAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable(planTypeToDisplay, Me.Session_dataset_dsAllPartAccountsDetail, Me.Session_Dataset_PartAccountDetail)
    '            'END: PPP | 09/29/2016 | YRS-AT-2529 | Plan will be decided based on the split's recorded till now, as well as all account balances of participant will be shown
    '            If Not participantAfterSplitValuesTable Is Nothing Then 'PPP | 09/21/2016 | YRS-AT-2529 | Load participant data only if exists
    '                participantAfterSplitValuesTable.Rows.InsertAt(participantAfterSplitValuesTable.NewRow(), 0)

    '                dgParticipant = DirectCast(e.Item.FindControl("DatagridSummaryBalList"), DataGrid)
    '                dgParticipant.DataSource = participantAfterSplitValuesTable
    '                dgParticipant.DataBind()
    '                'DatagridSummaryBalList.DataSource = participantAfterSplitValuesTable
    '                'DatagridSummaryBalList.DataBind()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogException("DataListParticipant_ItemDataBound", ex)
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    Finally
    '        participantAfterSplitValuesTable = Nothing
    '    End Try
    'End Sub
    'END: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Control DataListParticipant is commented at design level so events are not requied

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo            Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :'This Event will fire when the user will click on the Add New Button    //
    '                            in the beneficiary tab.                                                //
    '***************************************************************************************************//
    Private Sub ButtonAddNewBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddNewBeneficiary.Click
        Try
            'Start:Anudeep:2013.06.20: Comented Below lines -BT-1555:YRS 5.0-1769:Length of phone numbers
            ''Anudeep:13.04.2013 :BT-1555:YRS 5.0-1769:Length of phone numbers
            'If AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA" Then
            '    If TextboxSpouseTel.Text.Length <> 10 And TextboxSpouseTel.Text.Length > 0 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", GetMessageFromResource("MESSAGE_QDRO_TELEPHONE_LENGTH"), MessageBoxButtons.Stop)
            '        Exit Sub
            '    End If
            'End If
            'End:Anudeep:2013.06.20: Comented Below lines -BT-1555:YRS 5.0-1769:Length of phone numbers
            ButtonAddBeneficiaryToList.Enabled = False
            ClearControls(True)
            TextBoxSSNo.Enabled = True
            Call SetControlFocus(Me.TextBoxSSNo)
            ButtonAddBeneficiaryToList.Text = "Save Recipient" 'PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Replaced "Add To List"  by "Save Recipient"
            If Not Me.Session_datatable_dtBenifAccount Is Nothing Then
                If Me.Session_datatable_dtBenifAccount.Rows.Count > 0 Then
                    ButtonResetBeneficiary.Enabled = True
                End If
            End If
            ButtonEditBeneficiary.Enabled = False
            'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After addition of new recipient, defined fee must be reset
            If chkApplyFees.Checked And (HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) AndAlso HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees)) Then
                ResetFees()
            End If
            'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After addition of new recipient, defined fee must be reset
        Catch ex As Exception
            'Dim l_String_Exception_Message As String 'PPP | 12/28/2016 | Not using it
            HelperFunctions.LogException("ButtonAddNewBeneficiary_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            'START: PPP | 12/28/2016 | Directly passing exception message for redirection
            'l_String_Exception_Message = ex.Message.Trim.ToString()
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
            'END: PPP | 12/28/2016 | Directly passing exception message for redirection
        End Try
    End Sub

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo            Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :'This Event will fire when the user will click on the Reset Button      //
    '                            in the beneficiary tab.                                                //
    '***************************************************************************************************//
    Private Sub ButtonResetBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonResetBeneficiary.Click
        Try
            Dim i As Integer
            If Not Me.Session_datatable_dtBenifAccount Is Nothing Then
                If (Me.Session_datatable_dtBenifAccount.Rows.Count > 0) Then
                    PopulateBeneficiaryData(0)
                    lockRecipientControls(False)
                    ButtonAddBeneficiaryToList.Enabled = False
                    ButtonResetBeneficiary.Enabled = False
                    TextBoxSSNo.Enabled = False
                    'START: MMR | 2017.01.25 | YRS-AT-3145 & 3265 | Binding beneficiary data to grid
                    DatagridBenificiaryList.DataSource = Me.Session_datatable_dtBenifAccount
                    DatagridBenificiaryList.DataBind()
                    'END: MMR | 2017.01.25 | YRS-AT-3145 & 3265 | Binding beneficiary data to grid
                End If
            Else
                ClearControls(True)
                Call SetControlFocus(Me.TextBoxSSNo)
                'START: MMR | 2016.09.13 | YRS-AT-2482 | Reseting controls
                ButtonResetBeneficiary.Enabled = False
                ButtonAddBeneficiaryToList.Enabled = False
                'END: MMR | 2016.09.13 | YRS-AT-2482 | Reseting controls
            End If
            'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            'EnableDisableGenderMaritalControl(False) 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling Gender and Marital control
            ManageEditableControls(False)
            'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
            ButtonAddNewBeneficiary.Enabled = True
            ButtonEditBeneficiary.Enabled = False
            ButtonAddBeneficiaryToList.Text = "Save Recipient" 'PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Replaced "Add To List"  by "Save Recipient"
            EnableDisableNextButton(True) 'MMR | 2017.01.25 | YRS-AT-3145 & 3265 | Enabling next button
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("ButtonResetBeneficiary_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo            Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :'This Event will fire when the user will click on the Edit Button       //
    '                            in the beneficiary tab.                                                //
    '***************************************************************************************************//
    Private Sub ButtonEditBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditBeneficiary.Click
        Try
            HiddenFieldDirty.Value = "true" 'MMR | 2016.11.23 | YRS-AT-3145 | setting hidden field value
            'START: MMR | 2017.01.25 | YRS-AT-3145 & 3265 | Commented existing code as text should always be update recipient on edit button
            ''Dim bnFlag As Boolean 'PPP | 09/12/2016 | YRS-AT-2529 | bnFlag is not used anywhere in this procedure
            'If ButtonAddBeneficiaryToList.Text = "Update Recipient" Then 'PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Replaced "Update To List"  by "Update Recipient"
            '    ButtonAddBeneficiaryToList.Text = "Save Recipient" 'PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Replaced "Add To List"  by "Save Recipient"
            'Else
            ButtonAddBeneficiaryToList.Text = "Update Recipient" 'PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Replaced "Update To List"  by "Update Recipient"
            'End If
            'END: MMR | 2017.01.25 | YRS-AT-3145 & 3265 | Commented existing code as text should always be update recipient on edit button
            dtBenifAccount = Me.Session_datatable_dtBenifAccount
            Dim drBenef() As DataRow
            'Start - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Commented existing code as not required and added code to enable disable controls based on existing or new beneficiary
            'drBenef = dtBenifAccount.Select("ID='" & cboBeneficiarySSNo.SelectedValue & "'")
            drBenef = dtBenifAccount.Select("ID='" & DatagridBenificiaryList.SelectedItem.Cells(recipientIndexPersID).Text & "'") 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Replaced BENEFICIARY_ID with recipientIndexPersID 
            If (drBenef.Length <> 0) Then
                Dim drDataRow As DataRow
                drDataRow = drBenef.GetValue(0)
                If (drDataRow("FlagNewBenf") = False) Then
                    lockRecipientControls(False)
                Else
                    lockRecipientControls(True)
                End If
            End If
            'lockRecipientControls(True)
            'End - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Commented existing code as not required and added code to enable disable controls based on existing or new beneficiary
            TextBoxSSNo.Enabled = False
            'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Enabling Gender and marital dropdown control
            'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Enabling spouse control.
            'EnableDisableGenderMaritalControl(True)
            ManageEditableControls(True)
            'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Enabling spouse control.
            'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Enabling Gender and marital dropdown control
            ButtonEditBeneficiary.Enabled = False
            ButtonResetBeneficiary.Enabled = True
            ButtonAddBeneficiaryToList.Enabled = True
            'START: MMR | 2017.01.25 | YRS-AT-3145 & 3265 | Disabling Next and Add new beneficiary button
            EnableDisableNextButton(False)
            ButtonAddNewBeneficiary.Enabled = False
            'END: MMR | 2017.01.25 | YRS-AT-3145 & 3265 | Disabling Next and Add new beneficiary button
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("ButtonEditBeneficiary_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'START : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    ''Created By                :Ganeswar Sahoo            Modified On :                                 //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Class Description         :'This Event will fire when the user will click on the Sort Button       //
    ''                            in the grid in the list tab.                                           //
    ''***************************************************************************************************//
    'Private Sub DataGridList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridList.SortCommand
    '    Try
    '        Dim l_button_Select As ImageButton
    '        Dim dv As New DataView
    '        Dim SortExpression As String
    '        SortExpression = e.SortExpression

    '        dv = Me.Session_dataset_ParticipantDetails.Tables(0).DefaultView
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
    '        Me.DataGridList.DataSource = Nothing
    '        Me.DataGridList.DataSource = dv
    '        Me.DataGridList.DataBind()
    '        Session("FindInfo_Sort") = dv.Sort
    '        Dim datagridbenifrow As Integer
    '        DataGridList.SelectedIndex = -1
    '        For datagridbenifrow = 0 To DataGridList.Items.Count - 1
    '            DataGridList.Items(datagridbenifrow).Font.Bold = False
    '        Next
    '        For datagridbenifrow = 0 To DataGridList.Items.Count - 1
    '            If DataGridList.Items(datagridbenifrow).Cells(1).Text.ToString = Me.string_PersId Then
    '                DataGridList.Items(datagridbenifrow).Font.Bold = True
    '                l_button_Select = DataGridList.Items(datagridbenifrow).FindControl("ImageButtonSelect")
    '                l_button_Select.ImageUrl = "images\selected.gif"
    '            Else
    '                DataGridList.Items(datagridbenifrow).Font.Bold = False
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("DataGridList_SortCommand", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = ex.Message.Trim.ToString()
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    ''Created By                :Ganeswar Sahoo            Modified On :                                 //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Class Description         :'This Event will fire when the user will get the pager                  //
    ''                            in the List tab.                                                       //
    ''***************************************************************************************************//
    'Private Sub dgPager_PageChanged(ByVal PgNumber As Integer) Handles dgPager.PageChanged
    '    Try
    '        If Me.IsPostBack Then
    '            'LoadQDROList() 'MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        HelperFunctions.LogException("dgPager_PageChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    '        l_String_Exception_Message = ex.Message.Trim.ToString()
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
#End Region

#Region "PRIVATE FUNCTIONS"

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed "FormatCurrency" function which was not in use. Old code can be checked from TF.

    'START : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    ''***************************************************************************************************//
    ''Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    ''Created By                :Ganeswar Sahoo             Modified On : 18/06/08                        //
    ''Modified By               :                                                                        //
    ''Modify Reason             :                                                                        //
    ''Constructor Description   :                                                                        //
    ''Function Description      :This function is called when the user will click  on the OK button in  the
    ''                          :List Tab
    ''***************************************************************************************************//
    'Public Sub LoadQDROList()
    '    Try
    '        Dim l_dataset_QDROList As New DataSet
    '        Dim l_PagingOn As Boolean
    '        l_dataset_QDROList = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.LookUpActiveList(TextBoxSSNoList.Text.Trim(), TextBoxFundNo.Text.ToString.Trim(), TextBoxLastNameList.Text.Trim(), TextBoxFirstNameList.Text.Trim(), TextBoxCityList.Text.Trim(), TextBoxStateList.Text.Trim())
    '        Me.Session_dataset_ParticipantDetails = l_dataset_QDROList

    '        If Not l_dataset_QDROList Is Nothing Then

    '            l_PagingOn = l_dataset_QDROList.Tables(0).Rows.Count > 5000

    '            If Me.DataGridList.CurrentPageIndex >= Me.SessionPageCount And Me.SessionPageCount <> 0 Then Exit Sub

    '            If (l_dataset_QDROList.Tables(0).Rows.Count > 0) Then
    '                LabelNoData.Visible = False
    '                Me.DataGridList.Visible = True

    '                If l_PagingOn Then
    '                    dgPager.Visible = True
    '                    DataGridList.AllowPaging = True
    '                Else
    '                    dgPager.Visible = False
    '                    DataGridList.AllowPaging = False
    '                End If


    '                If l_PagingOn Then

    '                    DataGridList.AllowPaging = False
    '                    DataGridList.AllowPaging = True
    '                    DataGridList.CurrentPageIndex = 0
    '                    DataGridList.PageSize = 10
    '                    dgPager.Grid = DataGridList
    '                    dgPager.PagesToDisplay = 10
    '                    dgPager.Visible = True
    '                Else

    '                End If
    '            Else
    '                LabelNoData.Visible = True
    '                Me.DataGridList.Visible = False
    '                dgPager.Visible = False
    '            End If

    '            Me.DataGridList.SelectedIndex = -1
    '            Me.DataGridList.DataSource = l_dataset_QDROList
    '            Me.SessionPageCount = Me.DataGridList.PageCount
    '            Me.DataGridList.DataBind()
    '        Else

    '            LabelNoData.Visible = True
    '            Me.DataGridList.Visible = False
    '            dgPager.Visible = False
    '        End If
    '    Catch ex As SqlException
    '        Me.DataGridList.Visible = False
    '        dgPager.Visible = False
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'END : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo            Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :'This Function used to populate the beneficiary data                    //
    '***************************************************************************************************//
    Private Sub PopulateBeneficiaryData(ByVal selectedrow As Integer)
        dtBenifAccount = Me.Session_datatable_dtBenifAccount
        TextBoxSSNo.Text = dtBenifAccount.Rows(selectedrow).Item("SSNo")
        If CType(dtBenifAccount.Rows(selectedrow).Item("SalutationCode"), String).Trim = "" Then
            DropdownlistSal.SelectedIndex = 0
        ElseIf CType(dtBenifAccount.Rows(selectedrow).Item("SalutationCode"), String).Trim = "Dr." Or CType(dtBenifAccount.Rows(selectedrow).Item("SalutationCode"), String).Trim = "Dr" Then
            DropdownlistSal.SelectedIndex = 1
        ElseIf CType(dtBenifAccount.Rows(selectedrow).Item("SalutationCode"), String).Trim = "Mr." Or CType(dtBenifAccount.Rows(selectedrow).Item("SalutationCode"), String).Trim = "Mr" Then
            DropdownlistSal.SelectedIndex = 2
        ElseIf CType(dtBenifAccount.Rows(selectedrow).Item("SalutationCode"), String).Trim = "Mrs." Or CType(dtBenifAccount.Rows(selectedrow).Item("SalutationCode"), String).Trim = "Mrs." Then
            DropdownlistSal.SelectedIndex = 3
        ElseIf CType(dtBenifAccount.Rows(selectedrow).Item("SalutationCode"), String).Trim = "Ms." Or CType(dtBenifAccount.Rows(selectedrow).Item("SalutationCode"), String).Trim = "Ms." Then
            DropdownlistSal.SelectedIndex = 4
        End If
        TextBoxFirstName.Text = dtBenifAccount.Rows(selectedrow).Item("FirstName")
        TextBoxLastName.Text = dtBenifAccount.Rows(selectedrow).Item("LastName")
        TextBoxMiddleName.Text = dtBenifAccount.Rows(selectedrow).Item("MiddleName")

        TextBoxSuffix.Text = dtBenifAccount.Rows(selectedrow).Item("SuffixTitle")
        TextBoxBirthDate.Text = dtBenifAccount.Rows(selectedrow).Item("BirthDate")
        AddressWebUserControl1.Address1 = dtBenifAccount.Rows(selectedrow).Item("Address1")
        AddressWebUserControl1.Address2 = dtBenifAccount.Rows(selectedrow).Item("Address2")
        AddressWebUserControl1.Address3 = dtBenifAccount.Rows(selectedrow).Item("Address3")
        AddressWebUserControl1.City = dtBenifAccount.Rows(selectedrow).Item("City")
        'Priya 10-Dec-2010: Changes made as sate n country fill up with javascript in user control
        'Me.AddressWebUserControl1.ShowDataForParticipant = 1
        'Me.AddressWebUserControl1.ShowDataForParticipant()
        AddressWebUserControl1.DropDownListCountryValue = dtBenifAccount.Rows(selectedrow).Item("Country")
        AddressWebUserControl1.DropDownListStateValue = dtBenifAccount.Rows(selectedrow).Item("State")
        AddressWebUserControl1.ZipCode = dtBenifAccount.Rows(selectedrow).Item("Zip")
        TextBoxEmail.Text = dtBenifAccount.Rows(selectedrow).Item("EmailAddress")
        TextBoxTel.Text = dtBenifAccount.Rows(selectedrow).Item("PhoneNumber")
        'Start - Manthan Rajguru| 2016.08.16 | YRS-AT-2482| Select marital status and gender.
        DropDownListMaritalStatus.SelectedValue = dtBenifAccount.Rows(selectedrow).Item("MaritalCode")
        DropDownListGender.SelectedValue = dtBenifAccount.Rows(selectedrow).Item("GenderCode")
        'End - Manthan Rajguru| 2016.08.16 | YRS-AT-2482| Select marital status and gender.
        chkSpouse.Checked = dtBenifAccount.Rows(selectedrow).Item("bitRecipientSpouse") 'Manthan Rajguru | 2016.08.26 | YRS-AT-2488| Setting checkbox based on datatable value
        AddressWebUserControl1.HideNoAddressDefinedLabel() 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Hiding "No Address information defined." label       
    End Sub

    Public Enum LoadDatasetMode
        Table
        Session
    End Enum

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :'This Function used for load the personal Tab                              //
    '***************************************************************************************************//
    'START: PPP | 08/24/2016 | YRS-AT-2529 | Removed PersID and FundEventID parameters becuase they are not required
    'Private Sub LoadPersonalTab(ByVal PersID As String, ByVal FundEventID As String)
    Private Sub LoadPersonalTab()
        'END: PPP | 08/24/2016 | YRS-AT-2529 | Removed PersID and FundEventID parameters becuase they are not required
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured LoadPersonalTab. Old code can be checked from TF.
        Try
            'START : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required and set selected index of tab to 0 as List tab removed
            'Me.LIstMultiPage.SelectedIndex = 1
            'Me.QdroMemberActiveTabStrip.SelectedIndex = 1          
            SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary)
            'END : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required and set selected index of tab to 0 as List tab removed
        Catch
            Throw
        End Try
    End Sub

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         : 'This function is used for load the Account Tab.                              //
    '***************************************************************************************************//
    Private Sub loadAccInitital()
        Try
            'TextboxEndDate.Text = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getAccountingDate()
            '*******************************************Commented by Amit-26 Nov 2008-Start
            'TextboxEndDate.Text = Date.Today
            '*******************************************Commented by Amit-26 Nov 2008-End
            TextboxBegDate.Text = "12/31/1995"
            TextBoxAmountWorkSheet.Text = "0.00"
            TextBoxPercentageWorkSheet.Text = "0.00"
            ButtonSplit.Enabled = False
            '*******************************************Commented by Amit-26 Nov 2008-Start
            'LoadAccountsTab()
            '*******************************************Commented by Amit-26 Nov 2008-End
            Call SetControlFocusDropDown(Me.cboBeneficiarySSNo)
        Catch
            Throw
        End Try
    End Sub

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         : 'This function is used for load the Account Tab.                           //
    '***************************************************************************************************//
    Private Sub LoadAccountsTab()
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured LoadAccountsTab. Old code can be checked from TF.
        CreateParticipantTable()
        LoadDataForQDRO(Me.string_FundEventID, TextboxBegDate.Text, TextboxEndDate.Text, "both")
    End Sub

    Private Sub ClearData()
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured ClearData. Old code can be checked from TF.
        TextBoxPercentageWorkSheet.Text = "0.00"
        TextBoxAmountWorkSheet.Text = "0.00"
        DataGridWorkSheet2.DataSource = Nothing
        DataGridWorkSheet2.DataBind()
        DataGridWorkSheet.DataSource = Nothing
        DataGridWorkSheet.DataBind()
        Me.Session_datatable_dtPartAccount = Nothing
        Me.Session_dataset_GroupAAnnuityBasisDetail = Nothing
        Me.Session_dataset_GroupBAnnuityBasisDetail = Nothing
        'Priya	26-June-2012	BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
        'Session("showCourrentBalValidation") = Nothing 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use 
        'END Priya	26-June-2012	BT-593,YRS 5.0-1167 : correction to how QS and QW transactions are created
        Me.Session_dataset_dsALLGroupBRecipantDetails() = Nothing
        Me.Session_dataset_dsALLGroupBParticipantDetails() = Nothing
        Me.Session_dataset_dsALLGroupAParticipantDetails() = Nothing
        Me.Session_dataset_dsALLGroupARecipantDetails() = Nothing

        'RadioButtonListPlanTypes.Enabled = True
        TextboxBegDate.Enabled = True
        TextboxEndDate.Enabled = True
        PopcalendarRecDate.Enabled = True
        PopcalendarRecDate2.Enabled = True
        '*******************************************Added by Amit-26 Nov 2008-Start
        TextboxEndDate.Text = String.Empty
        DataGridWorkSheets.DataSource = Nothing
        DataGridWorkSheets.DataBind()
        '*******************************************Added by Amit-26 Nov 2008-End
    End Sub

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         : 'This function is used for load the Summary Tab.                          //
    '***************************************************************************************************//
    Private Sub LoadSummaryTab()
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured LoadSummaryTab. Old code can be checked from TF.
        Call SetControlFocus(Me.TextBoxSSNo)
        dgParticipant.DataSource = Nothing
        dgParticipant.DataBind()
        LoadParticipant()
        DrawBeneficiaryTable()
    End Sub

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This function is used for load the Summary of Participant.                                //
    '***************************************************************************************************//
    Private Sub LoadParticipant()
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured LoadParticipant. Old code can be checked from TF.
        l_dataset_ParticipantDetail = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.GetParticipantDetail(Me.string_PersSSID)
        'START : MMR | 2016.11.28 | YRS-AT-3145 | Participant table will be designed using HTML control objects
        'DataListParticipant.DataSource = l_dataset_ParticipantDetail
        'DataListParticipant.DataBind()
        DrawParticipantTable(l_dataset_ParticipantDetail)
        'END : MMR | 2016.11.28 | YRS-AT-3145 | Participant table will be designed using HTML control objects
    End Sub

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to Create Participant Table.                                //
    '***************************************************************************************************//
    Private Sub CreateParticipantTable()
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured CreateParticipantTable. Old code can be checked from TF.
        Try
            dtPartAccount = New DataTable()
            dtPartAccount.Columns.Add("AcctType", GetType(System.String))
            dtPartAccount.Columns.Add("PersonalPreTax", GetType(System.Decimal))
            dtPartAccount.Columns.Add("PersonalPostTax", GetType(System.Decimal))
            dtPartAccount.Columns.Add("PersonalInterestBalance", GetType(System.Decimal))
            dtPartAccount.Columns.Add("PersonalTotal", GetType(System.Decimal))
            dtPartAccount.Columns.Add("YMCAPreTax", GetType(System.Decimal))
            dtPartAccount.Columns.Add("YMCAInterestBalance", GetType(System.Decimal))
            dtPartAccount.Columns.Add("YMCATotal", GetType(System.Decimal))
            dtPartAccount.Columns.Add("TotalTotal", GetType(System.Decimal))
            dtPartAccount.Columns.Add("Selected", GetType(System.Boolean))
            dtPartAccount.Columns.Add("PersId", GetType(System.String))
            DataGridWorkSheet2.Columns(4).Visible = False
            DataGridWorkSheets.Columns(5).Visible = False
            DataGridWorkSheet.Columns(4).Visible = False
            DataGridWorkSheet2.Columns(7).Visible = False
            DataGridWorkSheets.Columns(8).Visible = False
            DataGridWorkSheet.Columns(7).Visible = False
            DataGridWorkSheet.Columns(9).Visible = False
        Catch
            Throw
        End Try
    End Sub

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to Create Percentagecount Table.                                //
    '***************************************************************************************************//
    Private Sub CreatePecentageAmountTable() 'PPP | 08/29/2016 | YRS-AT-2529 | Changed name from CreatePecentagecountTable to CreatePecentageAmountTable
        Try
            dtPecentageCount.Columns.Add("Percentage", GetType(System.Decimal)) 'PPP | 08/29/2016 | YRS-AT-2529 | Changed System.Int32 to System.Decimal
            dtPecentageCount.Columns.Add("PersId", GetType(System.String))
            dtPecentageCount.Columns.Add("Amount", GetType(System.Decimal))
            dtPecentageCount.Columns.Add("PlanType", GetType(System.String)) 'PPP | 08/29/2016 | YRS-AT-2529 | Need to maintain $ and % planwise
        Catch
            Throw
        End Try
    End Sub

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to Load Data For QDRO.                                //
    '***************************************************************************************************//
    Private Sub LoadDataForQDRO(ByVal FundEventID As String, ByVal StartDate As String, ByVal EndDate As String, ByVal PlanType As String)
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured LoadDataForQDRO. Old code can be checked from TF.
        Dim accountTypeCounter As Integer
        Dim balanceRow As Integer
        Dim rows() As DataRow
        Dim participantAccount As DataRow
        Dim plantType As String
        Try
            'START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code and set selcted index  of QDRO tab strip
            'Me.LIstMultiPage.SelectedIndex = 2
            'Me.QdroMemberActiveTabStrip.SelectedIndex = 2
            SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts)
            'END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code and set selcted index  of QDRO tab strip

            If StartDate = "12/31/1995" Then
                StartDate = "01/01/1900"
            Else
                StartDate = StartDate
            End If

            l_dataset_PartAccountDetail = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getPartAccountDetailbyPlan(FundEventID, StartDate, EndDate, PlanType)
            Me.Session_Dataset_PartAccountDetail = l_dataset_PartAccountDetail
            Me.Session_dataset_AnnuityBasisDetail = Nothing 'used it at Split functions
            Me.Session_dataset_GroupAAnnuityBasisDetail = Nothing 'used it at Split functions
            Me.Session_dataset_GroupBAnnuityBasisDetail = Nothing 'used it at Split functions

            plantType = hdnSelectedPlanType.Value
            If plantType.ToUpper() <> "BOTH" Then
                GetDistinctAccountTypes(l_dataset_PartAccountDetail.Tables(0).Select(String.Format("PlanType='{0}'", plantType)))
            Else
                GetDistinctAccountTypes(l_dataset_PartAccountDetail.Tables(0).Select(""))
            End If

            For accountTypeCounter = 0 To dtAcctType.Rows.Count - 1
                rows = l_dataset_PartAccountDetail.Tables(0).Select("AcctType='" & dtAcctType.Rows(accountTypeCounter).Item(0) & "'")
                participantAccount = dtPartAccount.NewRow
                participantAccount("PersonalPreTax") = 0
                participantAccount("PersonalPostTax") = 0
                participantAccount("PersonalInterestBalance") = 0
                participantAccount("YMCAPreTax") = 0
                participantAccount("YMCAInterestBalance") = 0
                For balanceRow = 0 To rows.Length - 1
                    participantAccount("AcctType") = rows(balanceRow).Item("AcctType")
                    participantAccount("PersonalPreTax") += IIf(IsDBNull(rows(balanceRow).Item("PersonalPreTax")), 0, rows(balanceRow).Item("PersonalPreTax"))
                    participantAccount("PersonalPostTax") += IIf(IsDBNull(rows(balanceRow).Item("PersonalPostTax")), 0, rows(balanceRow).Item("PersonalPostTax"))
                    participantAccount("PersonalInterestBalance") += IIf(IsDBNull(rows(balanceRow).Item("PersonalInterestBalance")), 0, rows(balanceRow).Item("PersonalInterestBalance"))
                    participantAccount("PersonalTotal") = 0
                    participantAccount("YMCAPreTax") += IIf(IsDBNull(rows(balanceRow).Item("YMCAPreTax")), 0, rows(balanceRow).Item("YMCAPreTax"))
                    participantAccount("YMCAInterestBalance") += IIf(IsDBNull(rows(balanceRow).Item("YMCAInterestBalance")), 0, rows(balanceRow).Item("YMCAInterestBalance"))
                    participantAccount("YMCATotal") = 0
                    participantAccount("TotalTotal") = 0
                    participantAccount("Selected") = True
                Next
                participantAccount("PersonalTotal") = IIf(IsDBNull(participantAccount("PersonalPreTax")), 0, participantAccount("PersonalPreTax")) + IIf(IsDBNull(participantAccount("PersonalPostTax")), 0, participantAccount("PersonalPostTax")) + IIf(IsDBNull(participantAccount("PersonalInterestBalance")), 0, participantAccount("PersonalInterestBalance"))
                participantAccount("YMCATotal") = IIf(IsDBNull(participantAccount("YMCAPreTax")), 0, participantAccount("YMCAPreTax")) + IIf(IsDBNull(participantAccount("YMCAInterestBalance")), 0, participantAccount("YMCAInterestBalance"))
                participantAccount("TotalTotal") = participantAccount("YMCATotal") + participantAccount("PersonalTotal")
                If Me.string_PersId <> Nothing Then
                    participantAccount("PersId") = Me.string_PersId
                End If
                dtPartAccount.Rows.Add(participantAccount)
            Next

            Me.Session_datatable_dtPartAccount = dtPartAccount

            DataGridWorkSheets.DataSource = dtPartAccount
            DataGridWorkSheets.DataBind()
        Catch
            Throw
        Finally
            plantType = Nothing
            participantAccount = Nothing
            rows = Nothing
        End Try
    End Sub

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to get Distinct Account Types.                                //
    '***************************************************************************************************//
    Public Sub GetDistinctAccountTypes(ByVal drRows As DataRow()) 'PPP | 09/12/2016 | YRS-AT-2529 | Renamed it from getDistinctAccountTypes to GetDistinctAccountTypes
        'Getting the distinct supplier from the dataset
        Try
            dtAcctType.Columns.Add("AcctType")
            Dim RCnt As Integer
            Dim dr As DataRow
            For RCnt = 0 To drRows.Length - 1
                If Not IsDBNull(drRows(RCnt).Item("AcctType")) Then
                    If Not drRows(RCnt).Item("AcctType") = "" Then
                        If dtAcctType.Rows.Count = 0 Then
                            dr = dtAcctType.NewRow()
                            dr.Item(0) = drRows(RCnt).Item("AcctType")
                            dtAcctType.Rows.Add(dr)
                        Else
                            Dim drSelect() As DataRow
                            drSelect = dtAcctType.Select("AcctType='" & Replace(drRows(RCnt).Item("AcctType"), "'", "''") & "'")
                            If drSelect.Length = 0 Then
                                dr = dtAcctType.NewRow()
                                dr.Item(0) = drRows(RCnt).Item("AcctType")
                                dtAcctType.Rows.Add(dr)
                            End If
                        End If
                    End If
                End If
            Next
        Catch
            Throw
        End Try
    End Sub

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to lock Recipient Controls.                                //
    '***************************************************************************************************//
    Private Sub lockRecipientControls(ByVal param_bool As Boolean)
        Try
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
            TextBoxEmail.Enabled = param_bool
            TextBoxTel.Enabled = param_bool
            TextBoxFirstName.Enabled = param_bool
            AddressWebUserControl1.EnableControls = param_bool

            'START: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | If user is able to input/edit details then validtors also should be enabled
            CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = param_bool
            CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = param_bool
            TextBoxBirthDate.RequiredDate = param_bool
            'END: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | If user is able to input/edit details then validtors also should be enabled
        Catch
            Throw
        End Try
    End Sub

    ' This function is used for Creating the datatable for the Beneficiary
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to Create DataTable BenfAccount.                                //
    '***************************************************************************************************//
    Private Function CreateDataTableBenfAccount() As DataTable
        Try
            Dim dtBenifAccount As DataTable = New DataTable
            Dim myDataColumn As DataColumn
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "id"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "SSNo"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "LastName"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "FirstName"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "MiddleName"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "FundStatus"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "SuffixTitle"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "BirthDate"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "Address1"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "Address2"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "Address3"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "City"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "State"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "Zip"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "Country"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "EmailAddress"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "PhoneNumber"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn("FlagNewBenf")
            myDataColumn.DataType = Type.GetType("System.Boolean")
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "SalutationCode"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "MaritalCode"
            dtBenifAccount.Columns.Add(myDataColumn)
            'Start - Manthan Rajguru | 2016.08.16 | Adding gender column.
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "GenderCode"
            dtBenifAccount.Columns.Add(myDataColumn)
            'End - Manthan Rajguru | 2016.08.16 | Adding gender column.
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.String")
            myDataColumn.ColumnName = "RecpFundEventId"
            dtBenifAccount.Columns.Add(myDataColumn)
            myDataColumn = New DataColumn
            'Anudeep
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
            'Start - Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Added column for defining beneficiary as spouse
            myDataColumn = New DataColumn
            myDataColumn.DataType = Type.GetType("System.Boolean")
            myDataColumn.ColumnName = "bitRecipientSpouse"
            dtBenifAccount.Columns.Add(myDataColumn)
            'End - Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Added column for defining beneficiary as spouse

            'START: PPP | 11/29/2016 | YRS-AT-3145 | Adding fees columns to record recipient fees in AtsQdroNonRetSplit
            dtBenifAccount.Columns.Add("FeeOnRetirementPlan", Type.GetType("System.Decimal"))
            dtBenifAccount.Columns.Add("FeeOnSavingsPlan", Type.GetType("System.Decimal"))
            'END: PPP | 11/29/2016 | YRS-AT-3145 | Adding fees columns to record recipient fees in AtsQdroNonRetSplit

            Return dtBenifAccount
        Catch
            Throw
        End Try
    End Function

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :' This function is used for adding values to the datatable                           //
    '***************************************************************************************************//
    'Private Function AddDataToTable(ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal SpouseSuffix As String, ByVal BirthDate As String, ByVal SpouseAdd1 As String, ByVal SpouseAdd2 As String, ByVal SpouseAdd3 As String, ByVal SpouseCity As String, ByVal SpouseState As String, ByVal SpouseZip As String, ByVal SpouseEmail As String, ByVal SpouseTel As String, ByVal SpouseCountry As String, ByVal blnBenficiary As Boolean, ByVal myTable As DataTable, ByVal SalutationCode As String, ByVal MaritalCode As String) 'Manthan Rajguru | 2016.08.16| YRS-AT-2482| Commented Existing code
    'Start - Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Commented existing code and added parameter to identify beneficairy is spouse or not
    'Private Function AddDataToTable(ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal SpouseSuffix As String, ByVal BirthDate As String, ByVal SpouseAdd1 As String, ByVal SpouseAdd2 As String, ByVal SpouseAdd3 As String, ByVal SpouseCity As String, ByVal SpouseState As String, ByVal SpouseZip As String, ByVal SpouseEmail As String, ByVal SpouseTel As String, ByVal SpouseCountry As String, ByVal blnBenficiary As Boolean, ByVal myTable As DataTable, ByVal SalutationCode As String, ByVal MaritalCode As String, GenderCode As String) 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Gender code parameter is added.
    Private Function AddDataToTable(ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal SpouseSuffix As String, ByVal BirthDate As String, ByVal SpouseAdd1 As String, ByVal SpouseAdd2 As String, ByVal SpouseAdd3 As String, ByVal SpouseCity As String, ByVal SpouseState As String, ByVal SpouseZip As String, ByVal SpouseEmail As String, ByVal SpouseTel As String, ByVal SpouseCountry As String, ByVal blnBenficiary As Boolean, ByVal myTable As DataTable, ByVal SalutationCode As String, ByVal MaritalCode As String, GenderCode As String, ByVal blnIsRecipientSpouse As Boolean)
        'End - Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Commented existing code and added parameter to identify beneficairy is spouse or not
        Dim row As DataRow
        Dim dr As DataRow
        Dim drBeneficiary As DataRow
        Try
            row = myTable.NewRow()
            dsRecipientDtls = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getQDRORecipient(TextBoxSSNo.Text)
            If dsRecipientDtls.Tables(0).Rows.Count > 0 Then
                If dsRecipientDtls.Tables(0).Rows.Count > 0 Then
                    dr = dsRecipientDtls.Tables(0).Rows(0)

                    Me.string_RecptPersID = dr.Item("UniqueId").ToString
                End If
                row("id") = Me.string_RecptPersID
            Else
                row("id") = Guid.NewGuid().ToString()
                Me.string_RecptPersID = row("id")
            End If
            row("SSNo") = SSNo
            row("LastName") = LastName
            row("FirstName") = FirstName
            row("MiddleName") = MiddleName
            row("SuffixTitle") = SpouseSuffix
            row("BirthDate") = BirthDate
            row("Address1") = SpouseAdd1
            row("Address2") = SpouseAdd2
            row("Address3") = SpouseAdd3
            row("City") = SpouseCity
            row("State") = SpouseState
            row("Zip") = SpouseZip
            row("EmailAddress") = SpouseEmail
            row("PhoneNumber") = SpouseTel
            row("Country") = SpouseCountry
            row("FlagNewBenf") = blnBenficiary
            row("SalutationCode") = SalutationCode
            row("MaritalCode") = MaritalCode
            row("GenderCode") = GenderCode 'Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Adding gender code.
            row("RecpFundEventId") = Guid.NewGuid().ToString()
            'Anudeep:18.07.2013:BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
            row("Address_effectiveDate") = AddressWebUserControl1.EffectiveDate
            row("BadAddress") = AddressWebUserControl1.IsBadAddress
            row("AddressNote") = "Non retired Qdro split:" + AddressWebUserControl1.Notes
            row("AddressNote_bitImportant") = AddressWebUserControl1.BitImp
            row("bitRecipientSpouse") = blnIsRecipientSpouse 'Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Assigning value to row
            Me.string_RecptFundEventID = row("RecpFundEventId")
            myTable.Rows.Add(row)
            MaintainRecipientDetails(row, Me.String_QDRORequestID, YMCAObjects.DBActionType.CREATE) 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Recipient data will be added to staging table
            Return True
        Catch
            Throw
        End Try
    End Function

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to clear the controls on the beneficiaries tab.                                //
    '***************************************************************************************************//
    Private Function ClearControls(ByVal parameterFlag As Boolean)
        Try
            Dim blnIsRecipientSpouse As Boolean 'Manthan Rajguru | 2016.08.30 | Declared Boolean variable
            If parameterFlag = True Then
                TextBoxSSNo.Text = ""
            ElseIf parameterFlag = False Then
                TextBoxSSNo.Text = TextBoxSSNo.Text
            End If
            DropdownlistSal.SelectedIndex = -1
            TextBoxFirstName.Text = String.Empty
            TextBoxMiddleName.Text = String.Empty
            TextBoxLastName.Text = String.Empty
            TextBoxSuffix.Text = String.Empty
            TextBoxBirthDate.Text = String.Empty
            TextBoxFirstName.Text = String.Empty
            'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            'AddressWebUserControl1.Address1 = String.Empty
            'AddressWebUserControl1.Address2 = String.Empty
            'AddressWebUserControl1.Address3 = String.Empty
            'Priya 10-Dec-2010: Changes made as sate n country fill up with javascript in user control
            'AddressWebUserControl1.ShowDataForParticipant = 1
            'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            AddressWebUserControl1.LoadAddressDetail(Nothing)
            AddressWebUserControl1.ClearControls() = ""
            AddressWebUserControl1.City = String.Empty
            AddressWebUserControl1.ZipCode = String.Empty
            TextBoxEmail.Text = String.Empty
            TextBoxTel.Text = String.Empty
            'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Initializing the default value.
            'Start - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Checking if value exists in dropdown before assigning default selected value at the time of reseting controls
            If Not Me.DropDownListMaritalStatus.Items.FindByValue("D") Is Nothing Then
                DropDownListMaritalStatus.SelectedValue = "D"
            Else
                DropDownListMaritalStatus.SelectedValue = "SEL"
            End If
            'End - Manthan Rajguru | 2016.09.06 | YRS-AT-2482 | Checking if value exists in dropdown before assigning default selected value at the time of reseting controls
            DropDownListGender.SelectedValue = "SEL"
            'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Initializing the default value.
            blnIsRecipientSpouse = CheckForSpouseRecipient()
            'Start - Manthan Rajguru | 2016.08.26| YRS-AT-2488| If beneficiary is already defined as spouse than setting checkbox value to false
            If blnIsRecipientSpouse Then
                chkSpouse.Checked = False
            Else
                chkSpouse.Checked = True
            End If
            'Manthan Rajguru | 2016.08.26 | YRS-AT-2488| If beneficiary is already defined as spouse than setting checkbox value to false
        Catch
            Throw
        End Try
    End Function

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to Set Control Focus.                                //
    '***************************************************************************************************//
    Private Sub SetControlFocus(ByVal TextBoxFocus As TextBox)
        Dim l_string_script As String
        Try
            'Priya 12.07.2012 (Re-Open) YRS 5.0-1167 : correction to how QS and QW transactions are created 
            'Added  "If TextBoxFocus.Enabled = True Then" if condition if textbox is enabled then only add validation. For javascript error
            If TextBoxFocus.Enabled = True Then

                l_string_script = "<script language='Javascript'>" & _
                 "var obj = document.getElementById('" & TextBoxFocus.ID & "');" & _
                 "if (obj!=null){if (obj.disabled==false){obj.focus();}}" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("scriptsetfocus")) Then
                    Page.RegisterStartupScript("scriptsetfocus", l_string_script)
                End If
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub SetControlFocusDropDown(ByVal DropDownFocus As DropDownList)
        Dim l_string_script As String
        Try
            l_string_script = "<script language='Javascript'>" & _
             "var obj = document.getElementById('" & DropDownFocus.ID & "');" & _
             "if (obj!=null){if (obj.disabled==false){ obj.focus();}}" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("scriptsetfocus")) Then
                Page.RegisterStartupScript("scriptsetfocus", l_string_script)
            End If
        Catch
            Throw
        End Try
    End Sub

    'DateUserControl'
    Private Sub SetControlFocusDateUser(ByVal DateUserFocus As DateUserControl)
        Dim l_string_script As String
        Try
            l_string_script = "<script language='Javascript'>" & _
             "var obj = document.getElementById('" & DateUserFocus.ID & "');" & _
             "if (obj!=null){if (obj.disabled==false){obj.focus();}}" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("scriptsetfocus")) Then
                Page.RegisterStartupScript("scriptsetfocus", l_string_script)
            End If
        Catch
            Throw
        End Try
    End Sub
#End Region

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

#Region "BENEFICIERY BUTTON CLICK"
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                     //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to add the beneficiary to datagrid on beneficiaries tab.                                //
    '***************************************************************************************************//
    Private Sub ButtonAddBeneficiaryToList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddBeneficiaryToList.Click
        Try

            Dim drBenifAccount As DataRow
            Dim iCount As Integer = 0
            Dim blnBenFalg As Boolean
            Dim l_bool_isTrue As Boolean = True
            '***********************Enhancement Start 22/12/2008
            Dim l_bool_isTrueAddress As Boolean = False
            '***********************Enhancement End 22/12/2008
            Dim strWSMessage As String
            Dim stTelephoneError As String 'PPP | 2015.10.13 | YRS-AT-2588           

            TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "") '- Allowing the insertion of hyphen's in the SSN
            blnBenFalg = Me.Session_bool_NewPerson
            If TextBoxSSNo.Text.Length <> 9 Then
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ENTER_VALID_SSNO"), MessageBoxButtons.OK, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_ENTER_VALID_SSNO", "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                Exit Sub
            End If

            'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup. Also all name messages clubbed here.
            ''SP: 2013.10.16 : BT-2175 : Non retired Qdro split will throws error if benefeciary name is more than person table -Start
            'If (TextBoxFirstName.Text.Trim().Length > 20) Then
            '    TextBoxFirstName.Enabled = True
            '    MessageBox.Show(PlaceHolder1, "Beneficiary", GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_FIRST_NAME_MAX_LENGTH"), MessageBoxButtons.OK, False)
            '    Exit Sub
            'ElseIf (TextBoxMiddleName.Text.Trim.Length > 20) Then
            '    TextBoxMiddleName.Enabled = True
            '    MessageBox.Show(PlaceHolder1, "Beneficiary", GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_MIDDLE_NAME_MAX_LENGTH"), MessageBoxButtons.OK, False)
            '    Exit Sub
            'ElseIf (TextBoxLastName.Text.Trim.Length > 30) Then
            '    TextBoxLastName.Enabled = True
            '    MessageBox.Show(PlaceHolder1, "Beneficiary", GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_LAST_NAME_MAX_LENGTH"), MessageBoxButtons.OK, False)
            '    Exit Sub
            'End If
            ''SP: 2013.10.16 : BT-2175 : Non retired Qdro split will throws error if benefeciary name is more than person table -End
            Dim errorMessage As String = String.Empty
            Dim isErrorList As Boolean = False
            If (TextBoxFirstName.Text.Trim().Length > 20) Then
                TextBoxFirstName.Enabled = True
                errorMessage = GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_FIRST_NAME_MAX_LENGTH")
            End If

            If (TextBoxMiddleName.Text.Trim.Length > 20) Then
                TextBoxMiddleName.Enabled = True
                If Not String.IsNullOrEmpty(errorMessage) Then
                    errorMessage = String.Format("<ul><li>{0}<li><li>{1}<li>", errorMessage, GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_MIDDLE_NAME_MAX_LENGTH"))
                    isErrorList = True
                Else
                    errorMessage = GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_MIDDLE_NAME_MAX_LENGTH")
                End If
            End If

            If (TextBoxLastName.Text.Trim.Length > 30) Then
                TextBoxLastName.Enabled = True
                If isErrorList Then
                    errorMessage = String.Format("<li>{0}<li></ul>", errorMessage, GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_LAST_NAME_MAX_LENGTH"))
                ElseIf Not String.IsNullOrEmpty(errorMessage) Then
                    errorMessage = String.Format("<ul><li>{0}<li><li>{1}<li></ul>", errorMessage, GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_LAST_NAME_MAX_LENGTH"))
                Else
                    errorMessage = GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_LAST_NAME_MAX_LENGTH")
                End If
            End If

            If Not String.IsNullOrEmpty(errorMessage) Then
                ShowModalPopupWithCustomMessage("Beneficiary", errorMessage, "error")
            End If
            'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup. Also all name messages clubbed here.

            'SR:2013.08.05 - Need web service to accept beneficiary updates (Implementing restriction in YRS)
            If Session("NR_QDRO_PersID") <> Nothing Then
                'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
                strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("NR_QDRO_PersID"))
                If strWSMessage <> "NoPending" Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "YRS", "openDialog('" + strWSMessage + "','Bene');", True)
                    Exit Sub
                End If
            End If
            'End, SR:2013.08.05 - Need web service to accept beneficiary updates (Implementing restriction in YRS)
            'Anudeep:20.06.2013 :BT-1555:YRS 5.0-1769:Length of phone numbers
            If blnBenFalg = True Then
                If AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA" Then
                    'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    'If TextboxSpouseTel.Text.Trim().Length <> 10 And TextboxSpouseTel.Text.Trim().Length > 0 Then
                    '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", GetMessageFromResource("MESSAGE_QDRO_TELEPHONE_LENGTH"), MessageBoxButtons.Stop)
                    '    Exit Sub
                    'End If
                    If TextBoxTel.Text.Trim().Length > 0 Then
                        stTelephoneError = Validation.Telephone(TextBoxTel.Text.Trim(), YMCAObjects.TelephoneType.Telephone)
                        If (Not String.IsNullOrEmpty(stTelephoneError)) Then
                            'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", stTelephoneError, MessageBoxButtons.Stop)
                            ShowModalPopupWithCustomMessage("YMCA - YRS", stTelephoneError, "error")
                            'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                            Exit Sub
                        End If
                    End If
                    'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                End If
            End If
            'Priya 10-Dec-2010: Changes made as sate n country fill up with javascript in user control
            'code shifted to after If Me.Session_bool_NewPerson = False Then
            'If blnBenFalg = True Then
            '	l_bool_isTrue = checkBeneficiaryDetails()
            'End If
            'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | checking for dropdowm selected value
            If DropDownListGender.SelectedValue = "SEL" Or DropDownListMaritalStatus.SelectedValue = "SEL" Then
                Exit Sub
            End If
            'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | checking for dropdowm selected value
            '***********************Enhancement Start 22/12/2008
            If Me.Session_bool_NewPerson = False Then
                l_bool_isTrueAddress = CheckManditoryExistingParticipent()
                If l_bool_isTrueAddress = False Then
                    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    'MessageBox.Show(PlaceHolder1, "QDRO", "The Alternate Payee   SSNO = " + TextBoxSpouseSSNo.Text.Trim() + " is already a participant does not have a valid address record.  Processing cannot continue until the address is updated properly", MessageBoxButtons.OK, False)
                    'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'MessageBox.Show(PlaceHolder1, "QDRO", String.Format(GetMessageFromResource("MESSAGE_QDRO_INVALID_ADDRESS_RECORD"), TextBoxSSNo.Text.Trim()), MessageBoxButtons.OK, False)
                    ShowModalPopupWithCustomMessage("QDRO", String.Format(GetMessageFromResource("MESSAGE_QDRO_INVALID_ADDRESS_RECORD"), TextBoxSSNo.Text.Trim()), "error")
                    'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    ButtonAddBeneficiaryToList.Enabled = False
                    Exit Sub
                End If
            End If
            '***********************Enhancement End 22/12/2008
            'Priya 10-Dec-2010: Changes made as sate n country fill up with javascript in user control
            If blnBenFalg = True Then
                l_bool_isTrue = checkBeneficiaryDetails()
            End If
            'Priya 10-Dec-2010: YRS 5.0-1177:Changes made as sate n country fill up with javascript in user control
            Dim strState As String = String.Empty
            Dim strCountry As String = String.Empty
            'Priya 10-Dec-2010:Commenetd as sate n country fill up with javascript
            If AddressWebUserControl1.DropDownListStateValue = "-Select State-" Then
                strState = ""
            Else
                strState = AddressWebUserControl1.DropDownListStateValue
            End If

            If AddressWebUserControl1.DropDownListCountryValue = "-Select Country-" Then
                strCountry = ""
            Else
                strCountry = AddressWebUserControl1.DropDownListCountryValue
            End If
            'End 10-Dec-2010:Commenetd as sate n country fill up with javascript

            If l_bool_isTrue = False Then
                Exit Sub
            Else
                If Me.Session_datatable_dtBenifAccount Is Nothing Then
                    dtBenifAccount = CreateDataTableBenfAccount()
                    'AddDataToTable(TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text.trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.DropDownListCountryValue, blnBenFalg, dtBenifAccount, DropdownlistSpouseSal.SelectedItem.ToString.Trim, "S")
                    'Anudeep:18.07.2013:BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
                    'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Passing gender value to the AddDataToTable method.
                    'AddDataToTable(TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text.trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.DropDownListCountryValue, blnBenFalg, dtBenifAccount, DropdownlistSpouseSal.SelectedItem.ToString.Trim, "S")
                    AddDataToTable(TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text.trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.DropDownListCountryValue, blnBenFalg, dtBenifAccount, DropdownlistSal.SelectedItem.ToString.Trim, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, chkSpouse.Checked) 'Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Added parameter for defining benficiary as spouse 
                    'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Passing gender value to the AddDataToTable method.
                    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    'MessageBox.Show(PlaceHolder1, "QDRO", "Do you want to add another beneficiary ?", MessageBoxButtons.YesNo, False)

                    'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ADD_ANOTHER_BENEFICIARY"), MessageBoxButtons.YesNo, False)
                    NeutraliseAllYesNoMessageFlags()
                    ButtonAddNewBeneficiary.Enabled = True
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_ADD_ANOTHER_BENEFICIARY", "infoYesNo")
                    'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    HiddenFieldDirty.Value = "false" 'MMR | 2016.11.23 | YRS-AT-3145 | Resetting hidden field value

                    DatagridBenificiaryList.DataSource = dtBenifAccount
                    DatagridBenificiaryList.DataBind()
                    'Start - Manthan Rajguru | 2016.08.29 | YRS-AT-2482 | Disabling Gender and marital dropdown control
                    ManageEditableControls(False)
                    'End - Manthan Rajguru | 2016.08.29 | YRS-AT-2482 | Disabling Gender and marital dropdown control

                    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 
                    EnableDisableNextButton(True)
                    'After addition of new recipient, defined fee must be reset
                    If chkApplyFees.Checked And (HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) AndAlso HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees)) Then
                        ResetFees()
                    End If
                    hdnSelectedPlanType.Value = String.Empty
                    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265
                Else
                    If ButtonAddBeneficiaryToList.Text = "Save Recipient" Then 'PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Replaced "Add To List"  by "Save Recipient"
                        dtBenifAccount = Me.Session_datatable_dtBenifAccount
                        For iCount = 0 To dtBenifAccount.Rows.Count - 1
                            drBenifAccount = dtBenifAccount.Rows(iCount)
                            If (drBenifAccount.Item("SSNo") = TextBoxSSNo.Text.Trim()) Then
                                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                                'MessageBox.Show(PlaceHolder1, "QDRO", "Beneficiary Already Exist with this SSNo", MessageBoxButtons.OK, False)
                                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_BENEFICIARY_EXISTS"), MessageBoxButtons.OK, False)
                                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_BENEFICIARY_EXISTS", "error")
                                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                                Exit Sub
                            End If
                        Next
                        'Start -MMR | 2016.08.26 | YRS-AT-2488| Added to validate for more than one spouse beneficiary and showing error message 
                        If chkSpouse.Checked Then
                            If CheckForSpouseRecipient() Then
                                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ANOTHER_SPOUSE_NOT_ALLOWED"), MessageBoxButtons.Stop)
                                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_ANOTHER_SPOUSE_NOT_ALLOWED", "error")
                                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                                chkSpouse.Checked = False
                                Exit Sub
                            End If
                        End If
                        'End -MMR | 2016.08.26 | YRS-AT-2488| Added to validate for more than one spouse beneficiary and showing error message 
                        'AddDataToTable(TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text.trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.DropDownListCountryValue, blnBenFalg, dtBenifAccount, DropdownlistSpouseSal.SelectedItem.ToString.Trim, "S")
                        'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Passing gender value to the AddDataToTable method.
                        'AddDataToTable(TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text.trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.DropDownListCountryValue, blnBenFalg, dtBenifAccount, DropdownlistSpouseSal.SelectedItem.ToString.Trim, "S")
                        AddDataToTable(TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text.trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.DropDownListCountryValue, blnBenFalg, dtBenifAccount, DropdownlistSal.SelectedItem.ToString.Trim, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, chkSpouse.Checked) 'Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Added parameter for defining benficiary as spouse 
                        'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Passing gender value to the AddDataToTable method.
                        DatagridBenificiaryList.DataSource = dtBenifAccount
                        DatagridBenificiaryList.DataBind()
                        'Start - Manthan Rajguru | 2016.08.29 | YRS-AT-2482 | Disabling Gender and marital dropdown control
                        ManageEditableControls(False)
                        'End - Manthan Rajguru | 2016.08.29 | YRS-AT-2482 | Disabling Gender and marital dropdown control
                        'blnBeneStatus = True 'PPP | 09/12/2016 | YRS-AT-2529 | blnBeneStatus local variable is removed
                        'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                        'MessageBox.Show(PlaceHolder1, "QDRO", "Do you want to add another beneficiary ?", MessageBoxButtons.YesNo, False)
                        'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ADD_ANOTHER_BENEFICIARY"), MessageBoxButtons.YesNo, False)
                        NeutraliseAllYesNoMessageFlags()
                        ButtonAddNewBeneficiary.Enabled = True
                        ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_ADD_ANOTHER_BENEFICIARY", "infoYesNo")
                        'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        HiddenFieldDirty.Value = "false" 'MMR | 2016.11.23 | YRS-AT-3145 | Resetting hidden field value
                        'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After addition of new recipient, defined fee must be reset
                        If chkApplyFees.Checked And (HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) AndAlso HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees)) Then
                            ResetFees()
                        End If
                        'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After addition of new recipient, defined fee must be reset
                        'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265
                        EnableDisableNextButton(True)
                        hdnSelectedPlanType.Value = String.Empty
                        'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265
                    End If

                End If
            End If
            If ButtonAddBeneficiaryToList.Text = "Update Recipient" Then  'PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Replaced "Update To List" by "Update Recipient"
                If blnBenFalg = True Then
                    l_bool_isTrue = checkBeneficiaryDetails()
                End If
                If l_bool_isTrue = False Then
                    Exit Sub
                Else
                    dtBenifAccount = Me.Session_datatable_dtBenifAccount
                    'Start -MMR | 2016.08.26 | YRS-AT-2488| Added to validate for more than one spouse beneficiary and showing error message 
                    If chkSpouse.Checked Then
                        If CheckForSpouseRecipient() And ViewState("IsAlreadySpouse") = Nothing Then
                            'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                            'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ANOTHER_SPOUSE_NOT_ALLOWED"), MessageBoxButtons.Stop)
                            ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_ANOTHER_SPOUSE_NOT_ALLOWED", "error")
                            'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                            chkSpouse.Checked = False
                            Exit Sub
                        End If
                    End If
                    'End -MMR | 2016.08.26 | YRS-AT-2488| Added to validate for more than one spouse beneficiary and showing error message 
                    'UpdateDataToTable(Me.String_Benif_PersonD, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "QD", Convert.ToBoolean(Me.Session_bool_NewPerson), Me.string_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, AddressWebUserControl1.DropDownListStateValue, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                    'Anudeep:18.07.2013:BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
                    'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Passing marital status and gender value to the Updatetable method.
                    'UpdateDataToTable(Me.String_Benif_PersonD, TextBoxSpouseSSNo.Text.Trim(), TextboxSpouseLast.Text.Trim(), TextboxSpouseFirst.Text.Trim(), TextboxSpouseMiddle.Text.Trim(), "QD", Convert.ToBoolean(Me.Session_bool_NewPerson), Me.string_RecptFundEventID, DropdownlistSpouseSal.SelectedItem.Text.Trim(), TextboxSpouseSuffix.Text.Trim(), TextboxBirthDate.Text, String.Empty, TextboxSpouseEmail.Text.Trim(), TextboxSpouseTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, strState, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount)
                    UpdateDataToTable(Me.String_Benif_PersonD, TextBoxSSNo.Text.Trim(), TextBoxLastName.Text.Trim(), TextBoxFirstName.Text.Trim(), TextBoxMiddleName.Text.Trim(), "QD", Convert.ToBoolean(Me.Session_bool_NewPerson), Me.string_RecptFundEventID, DropdownlistSal.SelectedItem.Text.Trim(), TextBoxSuffix.Text.Trim(), TextBoxBirthDate.Text, DropDownListMaritalStatus.SelectedValue, DropDownListGender.SelectedValue, TextBoxEmail.Text.Trim(), TextBoxTel.Text.Trim(), AddressWebUserControl1.Address1, AddressWebUserControl1.Address2, AddressWebUserControl1.Address3, AddressWebUserControl1.City, strState, AddressWebUserControl1.ZipCode, AddressWebUserControl1.DropDownListCountryValue, dtBenifAccount, chkSpouse.Checked) 'Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Added parameter for defining benficiary as spouse 
                    'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Passing marital status and gender value to the Updatetable method.
                    DatagridBenificiaryList.DataSource = dtBenifAccount
                    DatagridBenificiaryList.DataBind()
                    'Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling Gender and marital dropdown control
                    'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
                    'EnableDisableGenderMaritalControl(False)
                    ManageEditableControls(False)
                    'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488|  Disabling spouse control.
                    'End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Disabling Gender and marital dropdown control
                    ButtonEditBeneficiary.Enabled = False
                    ButtonAddNewBeneficiary.Enabled = True
                    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    'MessageBox.Show(PlaceHolder1, "QDRO", "Updated Successfully", MessageBoxButtons.OK, False)
                    'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_UPDATED_SUCCESSFULLY"), MessageBoxButtons.OK, False)
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_UPDATED_SUCCESSFULLY", "ok")
                    'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'lockRecipientControls(False) 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Moved out of "If" condition, becuase all controls should be disabled after save
                    TextBoxSSNo.Enabled = False
                    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After addition of new recipient, defined fee must be reset
                    If chkApplyFees.Checked And (HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) AndAlso HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees)) Then
                        ResetFees()
                    End If
                    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After addition of new recipient, defined fee must be reset

                    hdnSelectedPlanType.Value = String.Empty                   
                    'START: MMR | 2017.01.25 | YRS-AT-3145 & 3265 | Enabling next & add new beneficiary button on successful updation of beneficiary
                    EnableDisableNextButton(True)
                    ButtonAddNewBeneficiary.Enabled = True
                    'END: MMR | 2017.01.25 | YRS-AT-3145 & 3265 | Enabling next & add new beneficiary button on successful updation of beneficiary
                End If
            End If
            Me.Session_datatable_dtBenifAccount = dtBenifAccount
            If ButtonAddBeneficiaryToList.Text = "Update Recipient" Then 'PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Replaced "Update To List"  by "Update Recipient"
                ButtonAddBeneficiaryToList.Text = "Save Recipient" 'PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Replaced "Add To List"  by "Save Recipient"
            End If

            'START: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Moved "lockRecipientControls" out of "If" condition, becuase all controls should be disabled after save
            lockRecipientControls(False)
            ButtonAddNewBeneficiary.Enabled = True
            TextBoxSSNo.Enabled = False
            'END: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Moved "lockRecipientControls" out of "If" condition, becuase all controls should be disabled after save

            ButtonAddBeneficiaryToList.Enabled = False
            ButtonResetBeneficiary.Enabled = False 'MMR | 2016.09.13 | YRS-AT-2482 | Disabling control
            cboBeneficiarySSNo.DataSource = dtBenifAccount
            cboBeneficiarySSNo.DataBind()
            'Me.QdroMemberActiveTabStrip.Items(2).Enabled = True 'MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required          
        Catch ex As Exception
            HelperFunctions.LogException("ButtonAddBeneficiaryToList_Click", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'Private Function UpdateDataToTable(ByVal RecptPersonID As String, ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal FundStatus As String, ByVal FlagNewBenf As Boolean, ByVal RecptFundEventID As String, ByVal SalutationCode As String, ByVal SuffixTitle As String, ByVal BirthDate As String, ByVal MaritalCode As String, ByVal EMail As String, ByVal PhoneNo As String, ByVal Add1 As String, ByVal Add2 As String, ByVal Add3 As String, ByVal City As String, ByVal State As String, ByVal zip As String, ByVal Country As String, ByVal dtBenifAccount As DataTable) 'Manthan Rajguru| 2016.08.16 | YRS-AT-2482 | Commented Existing code
    'Start - Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Commented existing code and added parameter to identify beneficairy is spouse or not
    'Private Function UpdateDataToTable(ByVal RecptPersonID As String, ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal FundStatus As String, ByVal FlagNewBenf As Boolean, ByVal RecptFundEventID As String, ByVal SalutationCode As String, ByVal SuffixTitle As String, ByVal BirthDate As String, ByVal MaritalCode As String, ByVal GenderCode As String, ByVal EMail As String, ByVal PhoneNo As String, ByVal Add1 As String, ByVal Add2 As String, ByVal Add3 As String, ByVal City As String, ByVal State As String, ByVal zip As String, ByVal Country As String, ByVal dtBenifAccount As DataTable) 'Manthan Rajguru| 2016.08.16 | YRS-AT-2482 | Adding input parameter for gendercode.
    Private Function UpdateDataToTable(ByVal RecptPersonID As String, ByVal SSNo As String, ByVal LastName As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal FundStatus As String, ByVal FlagNewBenf As Boolean, ByVal RecptFundEventID As String, ByVal SalutationCode As String, ByVal SuffixTitle As String, ByVal BirthDate As String, ByVal MaritalCode As String, ByVal GenderCode As String, ByVal EMail As String, ByVal PhoneNo As String, ByVal Add1 As String, ByVal Add2 As String, ByVal Add3 As String, ByVal City As String, ByVal State As String, ByVal zip As String, ByVal Country As String, ByVal dtBenifAccount As DataTable, ByVal blnIsRecipientSpouse As Boolean)
        'End - Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Commented existing code and added parameter to identify beneficairy is spouse or not
        Dim dtRowBenificiary As DataRow
        Try
            For Each dtRowBenificiary In dtBenifAccount.Rows
                If dtRowBenificiary("SSNo") = SSNo Then
                    dtRowBenificiary.BeginEdit()
                    dtRowBenificiary("LastName") = LastName
                    dtRowBenificiary("FirstName") = FirstName
                    dtRowBenificiary("MiddleName") = MiddleName
                    dtRowBenificiary("FundStatus") = FundStatus
                    dtRowBenificiary("SalutationCode") = SalutationCode
                    dtRowBenificiary("SuffixTitle") = SuffixTitle
                    dtRowBenificiary("BirthDate") = BirthDate
                    dtRowBenificiary("MaritalCode") = MaritalCode
                    dtRowBenificiary("GenderCode") = GenderCode 'Manthan Rajguru |2016.08.16| YRS-AT-2482| Updating the gender value.
                    dtRowBenificiary("EmailAddress") = EMail
                    dtRowBenificiary("PhoneNumber") = PhoneNo
                    dtRowBenificiary("Address1") = Add1
                    dtRowBenificiary("Address2") = Add2
                    dtRowBenificiary("Address3") = Add3
                    dtRowBenificiary("City") = City
                    dtRowBenificiary("State") = State
                    dtRowBenificiary("zip") = zip
                    dtRowBenificiary("Country") = Country
                    'Anudeep:18.07.2013:BT-1683:YRS 5.0-1862:Add notes record when user enters address in any module.
                    dtRowBenificiary("Address_effectiveDate") = AddressWebUserControl1.EffectiveDate
                    dtRowBenificiary("BadAddress") = AddressWebUserControl1.IsBadAddress
                    dtRowBenificiary("AddressNote") = "Non retired Qdro split:" + AddressWebUserControl1.Notes
                    dtRowBenificiary("AddressNote_bitImportant") = AddressWebUserControl1.BitImp
                    dtRowBenificiary("bitRecipientSpouse") = blnIsRecipientSpouse 'Manthan Rajguru | 2016.08.26 | YRS-AT-2488 | Updating value in selected datarow for beneficairy as spouse or not
                    dtRowBenificiary.EndEdit()
                    dtRowBenificiary.AcceptChanges()

                    MaintainRecipientDetails(dtRowBenificiary, Me.String_QDRORequestID, YMCAObjects.DBActionType.UPDATE) 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Recipient data will be updated to staging table
                End If
            Next

            dtBenifAccount = Me.Session_datatable_dtBenifAccount
        Catch
            Throw
        End Try
    End Function

    'START: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Deletes recipient from table, naming method as DeleteDataFromTable because old method names are UpdateDataToTable and AddDataToTable
    Private Function DeleteDataFromTable(ByVal ssn As String)
        Dim recipientPersID As String
        Dim splitConfigurationRows() As DataRow
        Dim isSplitExists As Boolean

        dtBenifAccount = Me.Session_datatable_dtBenifAccount
        If HelperFunctions.isNonEmpty(dtBenifAccount) Then
            If dtBenifAccount.Rows.Count = 1 AndAlso Convert.ToString(dtBenifAccount.Rows(0)("SSNo")) = ssn Then
                MaintainRecipientDetails(dtBenifAccount.Rows(0), Me.String_QDRORequestID, YMCAObjects.DBActionType.DELETE)
                dtBenifAccount = Nothing

                ClearDataTable()
                ClearSessionData()


            Else
                For Each row As DataRow In dtBenifAccount.Rows
                    If Convert.ToString(row("SSNo")) = ssn Then
                        recipientPersID = Convert.ToString(row("id"))
                        dtPecentageCount = Me.Session_datatable_dtPecentageCount
                        isSplitExists = False
                        If HelperFunctions.isNonEmpty(Me.Session_datatable_dtPecentageCount) Then
                            splitConfigurationRows = dtPecentageCount.Select(String.Format("PersId='{0}'", recipientPersID))
                            If splitConfigurationRows.Length > 0 Then
                                For Each splitConfigurationRow As DataRow In splitConfigurationRows
                                    splitConfigurationRow.Delete()
                                Next
                                dtPecentageCount.AcceptChanges()
                                If dtPecentageCount.Rows.Count = 0 Then
                                    dtPecentageCount = Nothing
                                End If
                                Me.Session_datatable_dtPecentageCount = dtPecentageCount
                                isSplitExists = True

                                If Not dtPecentageCount Is Nothing Then
                                    If isSplitExists Then
                                        ' Delete Participant split data
                                        Me.Session_dataset_dsAllPartAccountsDetail = DeleteSplitAccountDetails(Me.Session_dataset_dsAllPartAccountsDetail, recipientPersID, "Both", True)
                                        Me.Session_dataset_dsALLGroupAParticipantDetails = DeleteSplitAccountDetails(Me.Session_dataset_dsALLGroupAParticipantDetails, recipientPersID, "Both", True)
                                        Me.Session_dataset_dsALLGroupBParticipantDetails = DeleteSplitAccountDetails(Me.Session_dataset_dsALLGroupBParticipantDetails, recipientPersID, "Both", True)

                                        ' Delete Recipient split data
                                        Me.Session_dataset_dsAllRecipantAccountsDetail = DeleteSplitAccountDetails(Me.Session_dataset_dsAllRecipantAccountsDetail, recipientPersID, "Both", False)
                                        Me.Session_dataset_dsALLGroupARecipantDetails = DeleteSplitAccountDetails(Me.Session_dataset_dsALLGroupARecipantDetails, recipientPersID, "Both", False)
                                        Me.Session_dataset_dsALLGroupBRecipantDetails = DeleteSplitAccountDetails(Me.Session_dataset_dsALLGroupBRecipantDetails, recipientPersID, "Both", False)

                                        ResetFees()
                                    End If
                                Else
                                    ' Participant split data
                                    Me.Session_dataset_dsAllPartAccountsDetail = Nothing
                                    Me.Session_dataset_dsALLGroupAParticipantDetails = Nothing
                                    Me.Session_dataset_dsALLGroupBParticipantDetails = Nothing

                                    ' Recipient split data
                                    Me.Session_dataset_dsAllRecipantAccountsDetail = Nothing
                                    Me.Session_dataset_dsALLGroupARecipantDetails = Nothing
                                    Me.Session_dataset_dsALLGroupBRecipantDetails = Nothing

                                    ' Fee data
                                    Me.ParticipantTotalBalanceAfterSplitManageFees = Nothing
                                    Me.RecipientBalanceAfterSplitManageFees = Nothing
                                End If
                            End If
                        End If

                        MaintainRecipientDetails(row, Me.String_QDRORequestID, YMCAObjects.DBActionType.DELETE)
                        row.Delete()
                        dtBenifAccount.AcceptChanges()
                        Exit For
                    End If
                Next
            End If
        End If
        Me.Session_datatable_dtBenifAccount = dtBenifAccount
        hdnRecipientForDeletion.Value = "" 'MMR | 2017.01.20 | YRS-AT-3145 & 3265 | Resetting hidden field value
    End Function
    'END: PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Deletes recipient from table, naming method as DeleteDataFromTable because old method names are UpdateDataToTable and AddDataToTable

    Private Sub ClearControlsData()
        TextBoxSSNo.Text = String.Empty
        DropdownlistSal.SelectedIndex = -1
        TextBoxFirstName.Text = String.Empty
        TextBoxMiddleName.Text = String.Empty
        TextBoxLastName.Text = String.Empty
        TextBoxSuffix.Text = String.Empty
        TextBoxBirthDate.Text = String.Empty
        TextBoxEmail.Text = String.Empty
        'Priya 10-Dec-2010: Changes made as sate n country fill up with javascript in user control		
        'AddressWebUserControl1.ShowDataForParticipant = 1
        'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        AddressWebUserControl1.LoadAddressDetail(Nothing)
        AddressWebUserControl1.ClearControls() = ""
        'AddressWebUserControl1.Address1 = String.Empty
        'AddressWebUserControl1.Address2 = String.Empty
        'AddressWebUserControl1.Address3 = String.Empty
        'AddressWebUserControl1.City = String.Empty
        'AddressWebUserControl1.ZipCode = String.Empty
        'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        TextBoxTel.Text = String.Empty
        'RadioButtonListSplitAmtType.SelectedValue = "Percentage" 'PPP | 09/12/2016 | YRS-AT-2529 | Not required here
        'RadioButtonListPlanTypes.SelectedValue = "Both" 'PPP | 09/12/2016 | YRS-AT-2529 | Not required here
    End Sub

    '*******************************************************************************************************//
    'Event Name                :                                                  Created on  : 06/08/08    //
    'Created By                :Amit Nigam                                        Modified On :             //
    'Modified By               :                                                                            //
    'Modify Reason             :                                                                            //
    'Param Description         :                                                                            //
    'Method Description        :This function will check the manditory fields values entered or not.        //
    '*******************************************************************************************************//
    Private Function checkBeneficiaryDetails() As Boolean
        'Priya 10-Dec-2010: Changes made as sate n country fill up with javascript in user control
        Dim l_stringErrorMsg As String = String.Empty
        AddressWebUserControl1.IsPrimary = "1"
        l_stringErrorMsg = AddressWebUserControl1.ValidateAddress()
        Try
            Dim l_bool_Manditory As Boolean = True
            If TextBoxSSNo.Text = String.Empty Then
                l_bool_Manditory = False
                Exit Function
            ElseIf TextBoxFirstName.Text = String.Empty Then
                l_bool_Manditory = False
                Exit Function
            ElseIf TextBoxLastName.Text = String.Empty Then
                l_bool_Manditory = False
                Exit Function
            ElseIf TextBoxBirthDate.Text = String.Empty Then
                l_bool_Manditory = False
                Exit Function
                'Priya 10-Dec-2010: Changes made as sate n country fill up with javascript in user control
                'ElseIf AddressWebUserControl1.Address1 = "" Then
                '	l_bool_Manditory = False
                '	Exit Function
                'ElseIf AddressWebUserControl1.City = "" Then
                '	l_bool_Manditory = False
                '	Exit Function
                'ElseIf AddressWebUserControl1.DropDownListCountryValue = "" Then
                '	l_bool_Manditory = False
                '	Exit Function
            ElseIf l_stringErrorMsg <> "" Then
                l_bool_Manditory = False
                'Start -- Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Commented existing code and changed the message box button from OK to Stop
                'MessageBox.Show(PlaceHolder1, "QDRO", l_stringErrorMsg, MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", l_stringErrorMsg, MessageBoxButtons.Stop, False)
                ShowModalPopupWithCustomMessage("QDRO", l_stringErrorMsg, "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'End -- Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Commented existing code and changed the message box button from OK to Stop
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

    '***********************Enhancement Start 22/12/2008
    '*******************************************************************************************************//
    'Event Name                :CheckManditoryExistingParticipent                 Created on  : 22/12/08    //
    'Created By                :Amit Nigam                                            Modified On :         //
    'Modified By               :                                                                            //
    'Modify Reason             :                                                                            //
    'Param Description         :                                                                            //
    'Method Description        :This function will check the manditory fields values entered or not for     //
    '                           ExistingParticipent.                                                        //
    '*******************************************************************************************************//
    Private Function CheckManditoryExistingParticipent() As Boolean
        Dim l_bool_Manditory As Boolean = True

        Try

            If TextBoxSSNo.Text = String.Empty Then
                l_bool_Manditory = False
                Exit Function
            ElseIf TextBoxFirstName.Text = String.Empty Then
                l_bool_Manditory = False
                Exit Function
            ElseIf TextBoxLastName.Text = String.Empty Then
                l_bool_Manditory = False
                Exit Function
            ElseIf AddressWebUserControl1.Address1 = "" Then
                l_bool_Manditory = False
                Exit Function
            ElseIf AddressWebUserControl1.City = "" Then
                l_bool_Manditory = False
                Exit Function
            ElseIf AddressWebUserControl1.DropDownListCountryValue = "" Then
                l_bool_Manditory = False
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

    '***********************Enhancement End 22/12/2008
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo            Modified On :                                     //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to save the qdro details.                                          //
    '***************************************************************************************************//
    'BS:2012.05.17:YRS 5.0-1470: Create Method
    Private Sub DocumentSave()
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured DocumentSave. Old code can be checked from TF.
        Dim checkSecurity As String
        Dim message As String

        Dim isAnyRecipientSplitMissing, isNoFeeDefined As Boolean 'PPP | 01/02/2017 | YRS-AT-3145 & 3265
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                ShowModalPopupWithCustomMessage("YMCA-YRS", checkSecurity, "error")
                LoadSummaryTab()
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                Exit Sub
            End If
            'End : YRS 5.0-940

            'START : PPP | 08/02/2016 | YRS-AT-2990 | Checking split amount against acutal balances 
            If (Not ValidateEachBucket()) Then
                'START: PPP | 12/09/2016 | YRS-AT-2990 | Not displaying message box, error message will be displayed in Div
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_CURRENTBALVALIDATION"), MessageBoxButtons.Stop, False)
                'Session("showCourrentBalValidation") = "TRUE"
                'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'DivMainMessage.InnerText = GetMessageFromResource("MESSAGE_CURRENTBALVALIDATION")
                ShowModalPopupMessage("QDRO", "MESSAGE_CURRENTBALVALIDATION", "error")
                'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                LoadSummaryTab()
                'END: PPP | 12/09/2016 | YRS-AT-2990 | Not displaying message box, error message will be displayed in Div
                Exit Sub
            End If
            'END: PPP | 08/02/2016 | YRS-AT-2990 | Checking split amount against acutal balances 

            'Priya 22-June-2012 : YRS 5.0-1167 : correction to how QS and QW transactions are created
            'Month end of month prior to QDRO end date has not happened.
            message = ValidateEndOfMonth()
            If message.Length > 0 Then
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "Daily Interest", message.Trim(), MessageBoxButtons.OK)
                ShowModalPopupWithCustomMessage("Daily Interest", message.Trim(), "error")
                LoadSummaryTab()
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                Exit Sub
            End If
            'END Priya 22-June-2012 : YRS 5.0-1167 : correction to how QS and QW transactions are created
            CreateStatusTable() ''2012.09.21: SR - YRS 5.0-YRS 5.0-1346 : Added

            isAnyRecipientSplitMissing = False 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Assigning default value
            If Not Me.Session_datatable_dtBenifAccount Is Nothing And Not Me.Session_datatable_dtBenifAccount Is Nothing Then
                If Not IsAllBeneficiarySplitDataExists() Then
                    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Message will be shown using modal popup, and also response is handled through btnYes click and not from PageLoad event
                    '    Me.Session_String_ISCompleted = True
                    '    'Me.LIstMultiPage.SelectedIndex = 2
                    '    'Me.QdroMemberActiveTabStrip.SelectedIndex = 2
                    '    'Me.QdroMemberActiveTabStrip.Items(3).Enabled = False
                    '    'Me.QdroMemberActiveTabStrip.Items(0).Enabled = False
                    '    'Me.QdroMemberActiveTabStrip.Items(1).Enabled = False
                    '    SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts)
                    '    EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, True)

                    '    'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    '    'MessageBox.Show(PlaceHolder1, "QDRO", "Do you want to Save the Record as there is another Recipient Pending for Split?", MessageBoxButtons.YesNo, False)
                    '    MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_SAVE_RECORD_RECEIPENT_PENDING"), MessageBoxButtons.YesNo, False)
                    '    Exit Sub
                    'Else
                    '    SaveNonRetiredSplit()
                    isAnyRecipientSplitMissing = True
                    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Message will be shown using modal popup, and also response is handled through btnYes click and not from PageLoad event
                End If
            End If

            'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Checking Fee, and then showing warning the user about missing recipient split, no fee and saving confirmation message
            isNoFeeDefined = IsZeroFeeDefined(Me.ParticipantTotalBalanceAfterSplitManageFees, Me.RecipientBalanceAfterSplitManageFees)

            If isAnyRecipientSplitMissing And isNoFeeDefined Then
                message = String.Format("<ul><li>{0}</li><li>{1}</li><li>{2}</li></ul>{3}", GetMessageFromResource("MESSAGE_QDRO_SAVE_RECORD_RECEIPENT_PENDING"), GetMessageFromResource("MESSAGE_QDRO_FINAL_SAVE_WARNING_WITHOUT_QUESTION"), GetMessageFromResource("MESSAGE_QDRO_NO_FEE"), GetMessageFromResource("MESSAGE_QDRO_FINAL_SAVE_WARNING_QUESTION"))
            ElseIf isAnyRecipientSplitMissing Then
                message = String.Format("<ul><li>{0}</li><li>{1}</li></ul>{2}", GetMessageFromResource("MESSAGE_QDRO_SAVE_RECORD_RECEIPENT_PENDING"), GetMessageFromResource("MESSAGE_QDRO_FINAL_SAVE_WARNING_WITHOUT_QUESTION"), GetMessageFromResource("MESSAGE_QDRO_FINAL_SAVE_WARNING_QUESTION"))
            ElseIf isNoFeeDefined Then
                message = String.Format("<ul><li>{0}</li><li>{1}</li></ul>{2}", GetMessageFromResource("MESSAGE_QDRO_FINAL_SAVE_WARNING_WITHOUT_QUESTION"), GetMessageFromResource("MESSAGE_QDRO_NO_FEE"), GetMessageFromResource("MESSAGE_QDRO_FINAL_SAVE_WARNING_QUESTION"))
            Else
                message = GetMessageFromResource("MESSAGE_QDRO_FINAL_SAVE_WARNING")
            End If
            Me.IsFinalSaveWarning = True
            LoadSummaryTab()
            ShowModalPopupWithCustomMessage("QDRO", message, "infoYesNo")
            'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Checking Fee, and then showing warning the user about missing recipient split, no fee and saving confirmation message
        Catch
            Throw
        Finally
            message = Nothing
            checkSecurity = Nothing
        End Try
    End Sub

    Private Sub ButtonDocumentSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDocumentSave.Click
        'BS:2012.05.17:YRS 5.0-1470 :-validate verify address
        'Start - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Added try catch block for Logging exception error message
        Try
            PopulateBeneficiaryData(0) 'PPP | 12/13/2016 | YRS-AT-2990 | Address control on errors were losing its value, so setting it to default 1st beneficiary

            Dim i_str_Message1 As String = String.Empty
            'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            i_str_Message1 = AddressWebUserControl1.ValidateAddress()
            If i_str_Message1 <> "" Then
                'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moved message from old messageBox to modal popup and set flag to identify which warning was shown, also replaced session with viewstate variable
                'Session("VerifiedAddress") = "VerifiedAddress"
                Me.IsVerifiedAddressWarning = True
                'MessageBox.Show(PlaceHolder1, "YMCA", i_str_Message1, MessageBoxButtons.YesNo)
                ShowModalPopupWithCustomMessage("YMCA", i_str_Message1, "infoYesNo")
                'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moved message from old messageBox to modal popup and set flag to identify which warning was shown, also replaced session with viewstate variable
                Exit Sub
            End If
            DocumentSave()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("ButtonDocumentSave_Click", ex)
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
        'End - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Added try catch block for Logging exception error message
    End Sub

    'START: PPP | 08/25/2016 | YRS-AT-2529 | All input parameters are removed
    'Private Sub SaveNonRetiredSplit(ByVal dtBenifAccountTempTable As DataTable, ByVal participantid As String, ByVal AnnuityBasisType As String, ByVal dtGroupBRecipients As DataTable, ByVal dtGroupBParticipants As DataTable, ByVal dtGroupARecipients As DataTable, ByVal dtGroupAParticipants As DataTable, ByVal dsGroupBRecipantDetails As DataSet, ByVal dsGroupARecipantDetails As DataSet, ByVal dsGroupBParticipantDetails As DataSet, ByVal dsGroupAParticipantDetails As DataSet)
    Private Sub SaveNonRetiredSplit()
        'END: PPP | 08/25/2016 | YRS-AT-2529 | All input parameters are removed
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured SaveNonRetiredSplit. Old code can be checked from TF.

        'Harshala 18/05/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : START
        Dim strMsg As String
        'Harshala 18/05/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END
        Dim qdroNonRetSplitTable As DataTable
        Dim qdroNonRetDetailsTable As DataTable
        Dim requestDetailsTable As DataTable
        Dim groupATransactions As DataSet, groupBTransactions As DataSet
        Dim isSaveFailed As Boolean
        Dim strStatus As String = String.Empty
        Dim strSuccessMsg As String = String.Empty
        Dim strFailureMsg As String = String.Empty
        Dim strServiceAvailabilityMsg As String = String.Empty
        Dim beneficiaryFundEventID As String, beneficiarySSN As String
        Dim beneficiaryTable As DataTable
        Dim recipientDataRow As DataRow
        Dim requestResult As YMCAObjects.ReturnObject(Of Boolean) 'SR | 2016.11.15 | YRS-AT-2990

        'START: PPP | 11/29/2016 | YRS-AT-3145 & 3265 
        Dim participantFeeDetails, recipientFeeDetails As DataTable
        Dim feeRows() As DataRow
        Dim participantRetirementFee, participantSavingsFee As Decimal?
        Dim isRetirementPlanSplitExists, isSavingsPlanSplitExists As Boolean
        'END: PPP | 11/28/2016 | YRS-AT-3145 & 3265 

        'SR: 10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 
        If ChkAdjustInterest.Checked Then
            AdjustInterest = True
        Else
            AdjustInterest = False
        End If
        'Ends,SR: 10.10.2012 - BT:1046/YRS 5.0-1603: Add check box to exclude interest accrued after QDRO end date 

        'START: PPP | 11/29/2016 | YRS-AT-3145 & 3265 | Handling fee
        participantRetirementFee = 0
        participantSavingsFee = 0
        If Not chkApplyFees.Checked Then
            participantRetirementFee = Nothing
            participantSavingsFee = Nothing
        ElseIf HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) Then
            participantFeeDetails = Me.ParticipantTotalBalanceAfterSplitManageFees
            participantRetirementFee = Convert.ToDecimal(participantFeeDetails.Rows(0)("RetirementFee"))
            participantSavingsFee = Convert.ToDecimal(participantFeeDetails.Rows(0)("SavingsFee"))
            isRetirementPlanSplitExists = Convert.ToBoolean(participantFeeDetails.Rows(0)("IsRetirementPlanSplitExists"))
            isSavingsPlanSplitExists = Convert.ToBoolean(participantFeeDetails.Rows(0)("IsSavingsPlanSplitExists"))
            If Not isRetirementPlanSplitExists Then
                participantRetirementFee = Nothing
            ElseIf Not isSavingsPlanSplitExists Then
                participantSavingsFee = Nothing
            End If
        End If

        dtBenifAccount = Me.Session_datatable_dtBenifAccount
        If Not dtBenifAccount.Columns.Contains("FeeOnRetirementPlan") Then
            dtBenifAccount.Columns.Add("FeeOnRetirementPlan", GetType(System.Decimal))
            dtBenifAccount.Columns.Add("FeeOnSavingsPlan", GetType(System.Decimal))
        End If

        If Not chkApplyFees.Checked Then
            participantRetirementFee = Nothing
            participantSavingsFee = Nothing
            For Each beneficiaryRow As DataRow In dtBenifAccount.Rows
                beneficiaryRow("FeeOnRetirementPlan") = System.DBNull.Value
                beneficiaryRow("FeeOnSavingsPlan") = System.DBNull.Value
            Next
        ElseIf HelperFunctions.isNonEmpty(Me.Session_datatable_dtBenifAccount) AndAlso HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees) Then
            recipientFeeDetails = Me.RecipientBalanceAfterSplitManageFees
            For Each beneficiaryRow As DataRow In dtBenifAccount.Rows
                feeRows = recipientFeeDetails.Select(String.Format("PersID='{0}'", beneficiaryRow("ID")))
                If feeRows.Count > 0 Then
                    If Convert.ToBoolean(feeRows(0)("IsRetirementPlanSplitExists")) Then
                        beneficiaryRow("FeeOnRetirementPlan") = feeRows(0)("RetirementFee")
                    Else
                        beneficiaryRow("FeeOnRetirementPlan") = System.DBNull.Value
                    End If
                    If Convert.ToBoolean(feeRows(0)("IsSavingsPlanSplitExists")) Then
                        beneficiaryRow("FeeOnSavingsPlan") = feeRows(0)("SavingsFee")
                    Else
                        beneficiaryRow("FeeOnSavingsPlan") = System.DBNull.Value
                    End If
                Else
                    beneficiaryRow("FeeOnRetirementPlan") = System.DBNull.Value
                    beneficiaryRow("FeeOnSavingsPlan") = System.DBNull.Value
                End If
            Next
            Me.Session_datatable_dtBenifAccount = dtBenifAccount
        End If
        'END: PPP | 11/29/2016 | YRS-AT-3145 & 3265 | Handling fee

        qdroNonRetSplitTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareQdroNonRetSplitTable(Me.String_QDRORequestID, Convert.ToDateTime(TextboxBegDate.Text), Convert.ToDateTime(TextboxEndDate.Text), Me.Session_datatable_dtBenifAccount, Me.Session_datatable_dtPecentageCount, Me.Session_Dataset_PartAccountDetail.Tables(0), Me.Session_dataset_dsAllRecipantAccountsDetail.Tables(0))
        qdroNonRetDetailsTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareQdroNonRetDetailsTable(qdroNonRetSplitTable, Me.Session_dataset_dsAllRecipantAccountsDetail.Tables(0))
        requestDetailsTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareRequestDetailsTable(Me.string_PersId, Me.string_FundEventID, Me.String_QDRORequestID, Me.AdjustInterest, TextboxBegDate.Text.Trim(), TextboxEndDate.Text.Trim(), participantRetirementFee, participantSavingsFee) ' PPP | 11/29/2016 | YRS-AT-3145 & 3265 | Passing participant fee [participantRetirementFee, participantSavingsFee]

        dsALLGroupBParticipantDetails = Me.Session_dataset_dsALLGroupBParticipantDetails.Copy 'PPP | 12/13/2016 | YRS-AT-2990 | Assigning copy to local variable to avoid reference type issues
        dsALLGroupBRecipantDetails = Me.Session_dataset_dsALLGroupBRecipantDetails.Copy 'PPP | 12/13/2016 | YRS-AT-2990 | Assigning copy to local variable to avoid reference type issues
        dsALLGroupAParticipantDetails = Me.Session_dataset_dsALLGroupAParticipantDetails.Copy 'PPP | 12/13/2016 | YRS-AT-2990 | Assigning copy to local variable to avoid reference type issues
        dsALLGroupARecipantDetails = Me.Session_dataset_dsALLGroupARecipantDetails.Copy 'PPP | 12/13/2016 | YRS-AT-2990 | Assigning copy to local variable to avoid reference type issues

        groupATransactions = dsALLGroupARecipantDetails
        For i As Integer = 0 To dsALLGroupAParticipantDetails.Tables(0).Rows.Count - 1
            groupATransactions.Tables(0).ImportRow(dsALLGroupAParticipantDetails.Tables(0).Rows(i))
        Next

        groupBTransactions = dsALLGroupBRecipantDetails
        For i As Integer = 0 To dsALLGroupBParticipantDetails.Tables(0).Rows.Count - 1
            groupBTransactions.Tables(0).ImportRow(dsALLGroupBParticipantDetails.Tables(0).Rows(i))
        Next

        ' START | SR | 2016.11.15 | YRS-AT-2990 - Changed Method return type from boolean to object of ReturnObject class to return multiple values from called method.
        requestResult = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.SaveRequest(requestDetailsTable, Me.Session_datatable_dtBenifAccount, qdroNonRetSplitTable, qdroNonRetDetailsTable, groupBTransactions, groupATransactions)
        isSaveFailed = requestResult.Value
        ' END | SR | 2016.11.15 | YRS-AT-2990 - Changed Method return type from boolean to object of ReturnObject class to return multiple values from called method.

        'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Showing error in static div, so no Yes-No message is there, user stays on page
        ''START: PPP | 12/09/2016 | YRS-AT-2990 | If there is a error while splitting then page should stay in same tab and Me.Session_String_ISCompleted should stay false
        ' ''Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END
        ''If Me.QdroMemberActiveTabStrip.SelectedIndex = 3 Then
        ''    Me.QdroMemberActiveTabStrip.SelectedIndex = 2
        ''    Me.Session_String_ISCompleted = True
        ''End If
        ''If isSaveFailed = False Then
        ''    Me.Session_String_ISCompleted = True
        ''End If
        ''END: PPP | 12/09/2016 | YRS-AT-2990 | If there is a error while splitting then page should stay in same tab and Me.Session_String_ISCompleted should stay false
        'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Showing error in static div, so no Yes-No message is there, user stays on page
        'Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : START
        'Harshala 18/05/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : START

        beneficiaryTable = Me.Session_datatable_dtBenifAccount
        If Not beneficiaryTable Is Nothing And isSaveFailed = False Then
            For Each beneficiaryRow As DataRow In beneficiaryTable.Rows  'dtBenifAccountTempTable.Rows
                beneficiaryFundEventID = beneficiaryRow("RecpFundEventId")
                beneficiarySSN = beneficiaryRow("SSNo")

                'Priya start 02-07-2012
                'strServiceAvailabilityMsg = ServiceAvailability(drBenifAccountTempTable)
                strServiceAvailabilityMsg = ServiceAvailability(beneficiaryFundEventID)
                If strServiceAvailabilityMsg = String.Empty Then
                    strStatus = SaveRefundRequest(beneficiaryFundEventID)
                Else
                    strStatus = "Failure"
                End If
                'strStatus = SaveRefundRequest(drBenifAccountTempTable)
                'END Priya  02-07-2012

                If strStatus = "Success" Then
                    strSuccessMsg = String.Format("{0} {1}", strSuccessMsg, beneficiarySSN)
                    ''Priya 08/06/2012 BT:1038 - QDRO Settlement process confirmation message.Added "," between two ssn
                    'If strSuccessMsg = String.Empty Then
                    '    strSuccessMsg = strSuccessMsg + " " + drBenifAccountTempTable("SSNo")
                    'Else
                    '    strSuccessMsg = strSuccessMsg + ", " + drBenifAccountTempTable("SSNo")
                    'End If
                    ''Priya 10-07-2012 change message, as disscuss with murali and put that messages into resource file 
                    ''strMsg = Resources.NonRetiredQDRO.MESSAGE_QDRO_SAVE_COMPLETED & Resources.NonRetiredQDRO.MESSAGE_QDRO_REQUEST_FAILED_PASSED & String.Format(Resources.NonRetiredQDRO.MESSAGE_QDRO_REQUEST_PASSED, strSuccessMsg)
                ElseIf strStatus = "Failure" Then
                    strFailureMsg = String.Format("{0} {1}", strFailureMsg, beneficiarySSN)
                    ''Priya 08/06/2012 BT:1038 - QDRO Settlement process confirmation message.Added "," between two ssn
                    'If strFailureMsg = String.Empty Then
                    '    strFailureMsg = strFailureMsg + " " + drBenifAccountTempTable("SSNo")
                    'Else
                    '    strFailureMsg = strFailureMsg + ", " + drBenifAccountTempTable("SSNo")
                    'End If
                    ''Priya 10-07-2012 change message, as disscuss with murali and put that messages into resource file 
                    ''strMsg = Resources.NonRetiredQDRO.MESSAGE_QDRO_SAVE_COMPLETED & Resources.NonRetiredQDRO.MESSAGE_QDRO_REQUEST_FAILED_PASSED & String.Format(Resources.NonRetiredQDRO.MESSAGE_QDRO_REQUEST_FAILED, strFailureMsg)

                    ''Priya 10-07-2012 change message, as disscuss with murali and put that messages into resource file 
                ElseIf strStatus = "NA" Then 'means balance is greater than 5000
                    'strMsg = Resources.NonRetiredQDRO.MESSAGE_QDRO_SAVE_COMPLETED
                End If
                ''2012.09.21: SR - YRS 5.0-YRS 5.0-1346: Added below code
                If strStatus <> "NA" Then
                    LoadStatusTable(beneficiarySSN, beneficiaryRow("FirstName"), beneficiaryRow("MiddleName"), beneficiaryRow("LastName"), strStatus, "Not-Viewed")
                End If
                ''2012.09.21: SR - YRS 5.0-YRS 5.0-1346- End, Added below code

            Next
            'Priya 13-07-2012 (Re-Open)BT-1038: QDRO Settlement process confirmation message.
            'asign value to strMsg after for each loop to show all ssno which are succes and failed for request creation
            'strMsg = Resources.NonRetiredQDRO.MESSAGE_QDRO_SAVE_COMPLETED
            strMsg = GetMessageFromResource("MESSAGE_QDRO_SAVE_COMPLETED")
            If strSuccessMsg <> String.Empty Then
                'strMsg = strMsg & Resources.NonRetiredQDRO.MESSAGE_QDRO_REQUEST_FAILED_PASSED & String.Format(Resources.NonRetiredQDRO.MESSAGE_QDRO_REQUEST_PASSED, strSuccessMsg)
                strMsg = strMsg & GetMessageFromResource("MESSAGE_QDRO_REQUEST_FAILED_PASSED") & String.Format(GetMessageFromResource("MESSAGE_QDRO_REQUEST_PASSED"), strSuccessMsg)
            End If
            If strFailureMsg <> String.Empty Then
                If strSuccessMsg = String.Empty Then
                    'strMsg = strMsg & Resources.NonRetiredQDRO.MESSAGE_QDRO_REQUEST_FAILED_PASSED & String.Format(Resources.NonRetiredQDRO.MESSAGE_QDRO_REQUEST_FAILED, strFailureMsg)
                    strMsg = strMsg & GetMessageFromResource("MESSAGE_QDRO_REQUEST_FAILED_PASSED") & String.Format(GetMessageFromResource("MESSAGE_QDRO_REQUEST_FAILED"), strFailureMsg)
                Else
                    'strMsg = strMsg & String.Format(Resources.NonRetiredQDRO.MESSAGE_QDRO_REQUEST_FAILED, strFailureMsg)
                    strMsg = strMsg & String.Format(GetMessageFromResource("MESSAGE_QDRO_REQUEST_FAILED"), strFailureMsg)
                End If
            End If
            'END Priya 13-07-2012 (Re-Open)BT-1038: QDRO Settlement process confirmation message.

            'Priya 08/06/2012 BT:1038 - QDRO Settlement process confirmation message.
            'strMsg = "QDRO settlement Saved Successfully."

            'Priya 10-07-2012 BT:1038: QDRO Settlement process confirmation message. change message, disscuss wit murali
            'Priya 10-07-2012 Start Commented as it will fetch from resource file 
            'strMsg = "QDRO settlement completed. Following is the status of attempts for beneficiary’s withdrawal request creation. <BR/>"

            'If Not strSuccessMsg = String.Empty Then
            '	'Priya 08/06/2012 BT:1038 - QDRO Settlement process confirmation message.
            '	'strMsg = strMsg + "Created refund request successfully for SSN No : " + strSuccessMsg
            '	strMsg = strMsg + "Passed – " + strSuccessMsg
            'End If
            'If Not strFailureMsg = String.Empty Then
            '	'Priya 08/06/2012 BT:1038 - QDRO Settlement process confirmation message.
            '	'strMsg = strMsg + "Unable to create refund request for SSN No. : " + strFailureMsg
            '	strMsg = strMsg + "<BR/>Failed – " + strFailureMsg + "<BR/>"
            '	strMsg = strMsg + "For failed request, please try creating them manually through the withdrawal requests screen as there may be some problem with the withdrawal service."
            '	'End 08/06/2012 BT:1038 - QDRO Settlement process confirmation message.
            'End If
            ''If strServiceAvailabilityMsg <> String.Empty Then
            ''	strServiceAvailabilityMsg = "please try creating them manually through the withdrawal requests screen as there may be some problem with the withdrawal service."
            ''	strMsg = strMsg + strServiceAvailabilityMsg
            ''End If

            'Priya 10-07-2012 END  Commented 
        ElseIf isSaveFailed = True Then
            'Priya 10-07-2012 change message, as disscuss with murali and put that messages into resource file 
            'strMsg = Resources.NonRetiredQDRO.MESSAGE_QDRO_SAVE_FAILED '"Unable to complete QDRO settlement."
            strMsg = GetMessageFromResource("MESSAGE_QDRO_SAVE_FAILED")
            ' START | SR | 2016.11.15 | YRS-AT-2990 - If Negative Balance exist after Split then display negative balance message to user.
            If Not requestResult.MessageList Is Nothing AndAlso requestResult.MessageList.Count > 0 Then
                'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'DivMainMessage.InnerText = String.Format(GetMessageFromResource("MESSAGE_QDRO_REQUEST_NEGATIVEBALANCE"), requestResult.MessageList(0).ToString) 'PPP | 12/09/2016 | YRS-AT-2990 | Not displaying message box, error message will be displayed in Div
                ShowModalPopupMessage("QDRO", requestResult.MessageList(0), "error")
                'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                If Me.QdroMemberActiveTabStrip.SelectedIndex = 3 Then
                    LoadSummaryTab()
                End If
                Exit Sub
            End If
            ' END | SR | 2016.11.15 | YRS-AT-2990 - If Negative Balance exist after Split then display negative balance message to user.
        Else
            'strMsg = strMsg = Resources.NonRetiredQDRO.MESSAGE_QDRO_SAVE_COMPLETED & Resources.NonRetiredQDRO.MESSAGE_QDRO
            strMsg = strMsg = GetMessageFromResource("MESSAGE_QDRO_SAVE_COMPLETED") & GetMessageFromResource("MESSAGE_QDRO")
        End If

        ' MessageBox.Show(170, 300, 410, 150, PlaceHolder1, "QDRO", strMsg, MessageBoxButtons.OK, False)

        'Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END
        'Harshala 18/05/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END 
        'START: MMR | 11/28/2016 | YRS-AT-3145
        'Me.LIstMultiPage.SelectedIndex = 4
        'Me.QdroMemberActiveTabStrip.SelectedIndex = 4
        SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.Status)
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.Status, True)
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave, False)
        'END: MMR | 11/28/2016 | YRS-AT-3145
        'Me.Session_String_ISCompleted = True 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
        EnableSplitButtonSet(False)
        'Added by Amit 22-12-2008 bugid 662
        cboBeneficiarySSNo.Enabled = False
        'Added by Amit 22-12-2008 bugid 662
        'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
        'ButtonDocumentCancel.Enabled = False
        'ButtonDocumentOK.Enabled = False
        'Session_String_ISCompleted = False ''2012.09.21: SR - YRS 5.0-YRS 5.0-1346 : Added
        'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
        'blnSave = False 'SR.:21.09.2012 -BT:1060/YRS 5.0-YRS 5.0-1346:Added
        BindStatusGrid() ''2012.09.21: SR - YRS 5.0-YRS 5.0-1346 : Added
        'START: MMR | 11/28/2016 | YRS-AT-3145
        'Me.QdroMemberActiveTabStrip.Items(0).Enabled = False
        'Me.QdroMemberActiveTabStrip.Items(1).Enabled = False
        'Me.QdroMemberActiveTabStrip.Items(2).Enabled = False
        'Me.QdroMemberActiveTabStrip.Items(3).Enabled = False
        'Me.QdroMemberActiveTabStrip.Items(4).Enabled = True ''2012.09.21: SR - YRS 5.0-YRS 5.0-1346 : Added
        'END: MMR | 11/28/2016 | YRS-AT-3145

        ClearDataTable()
        HiddenFieldDirty.Value = "false" 'MMR | 2016.11.23 | YRS-AT-3145 | Resetting hidden field value
        HideDivMessageControls() 'PPP | 01/06/2017 | YRS-AT-3145 & 3265 | Hiding all div controls related to messages
    End Sub

    'Priya start 02-07-2012
    'START: PPP | 08/25/2016 | YRS-AT-2529 | Accepting only recipientFundEventId instead of whole row
    'Private Function ServiceAvailability(ByVal drBenifAccountTempTable As DataRow) As String
    Private Function ServiceAvailability(ByVal recipientFundEventId As String) As String
        'END: PPP | 08/25/2016 | YRS-AT-2529 | Accepting only recipientFundEventId instead of whole row
        Try
            Dim objService As New YRSWebService.YRSWithdrawalService
            'Added by Nikunj Patel 19-07-2012 YRS 5.0-1346:Cash out plan balance <= $5,000
            objService.PreAuthenticate = True
            objService.Credentials = System.Net.CredentialCache.DefaultCredentials
            'END Nikunj Patel 19-07-2012 YRS 5.0-1346:Cash out plan balance <= $5,000
            objService.GetParticipantData(recipientFundEventId)
            ServiceAvailability = String.Empty
        Catch ex As Exception
            ServiceAvailability = ex.Message
            'MessageBox.Show(PlaceHolder1, "QDRO", ex.Message, MessageBoxButtons.OK, False)
            'Exit Function
        End Try

        Return ServiceAvailability
    End Function
    'END Priya start 02-07-2012

    'Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : START
    ''' <summary>Function use to save QDRO beneficiary refund request.</summary>
    ''' <param name="recipientFundEventID">Recipient FundEventID</praram>
    ''' <returns>Returns status as string of refund request creation</returns>
    ''' <remarks> </remarks> 
    Public Function SaveRefundRequest(ByVal recipientFundEventID As String) As String
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured SaveRefundRequest. Old code can be checked from TF.
        Dim withdrawalService As New YRSWebService.YRSWithdrawalService
        Dim serviceOutput As New YRSWebService.WebServiceReturn
        Dim arrayOfRetirementAccounts() As String
        Dim arrayOfSavingAccounts() As String
        Dim refundRequestPageInstance As RefundRequestWebForm
        Dim finalDataSet As DataSet
        Dim savingsTotal As Decimal = 0
        Dim retirementTotal As Decimal = 0
        Dim planType As String
        Dim requestStatus As String
        Try
            refundRequestPageInstance = New RefundRequestWebForm()
            requestStatus = "NA"
            'Added by Nikunj Patel 19-07-2012 YRS 5.0-1346:Cash out plan balance <= $5,000
            withdrawalService.PreAuthenticate = True
            withdrawalService.Credentials = System.Net.CredentialCache.DefaultCredentials
            'END Nikunj Patel 19-07-2012 YRS 5.0-1346:Cash out plan balance <= $5,000

            serviceOutput = withdrawalService.GetParticipantData(recipientFundEventID)
            finalDataSet = serviceOutput.WebServiceDataSet
            If serviceOutput.ReturnStatus Then
                If Not finalDataSet.Tables("RetirementPlanTable") Is Nothing Then
                    retirementTotal = finalDataSet.Tables("RetirementPlanTable").Compute("Sum(Total)", "AccountType='Total'")
                End If
                If Not finalDataSet.Tables("SavingPlanTable") Is Nothing Then
                    savingsTotal = finalDataSet.Tables("SavingPlanTable").Compute("Sum(Total)", "AccountType='Total'")
                End If
                If (retirementTotal <= 5000 And retirementTotal > 0) Or (savingsTotal <= 5000 And savingsTotal > 0) Then
                    Session("FinalDataSet") = finalDataSet
                    If HelperFunctions.isNonEmpty(finalDataSet.Tables("RetirementPlanTable")) Then
                        arrayOfRetirementAccounts = refundRequestPageInstance.ArrayOfSelectedRetirementAccountTypes(finalDataSet.Tables("RetirementPlanTable"))
                    End If
                    If HelperFunctions.isNonEmpty(finalDataSet.Tables("SavingPlanTable")) Then
                        arrayOfSavingAccounts = refundRequestPageInstance.ArrayOfSelectedSavingsAccountTypes(finalDataSet.Tables("SavingPlanTable"))
                    End If
                    withdrawalService.PreAuthenticate = True
                    withdrawalService.Credentials = System.Net.CredentialCache.DefaultCredentials

                    'START: PPP | 08/30/2016 | YRS-AT-2529 | Selected plan type value is in hidden field
                    'If RadioButtonListPlanTypes.SelectedValue = "Both" And dRetirementTotal <= 5000 And dSavingTotal <= 5000 Then
                    '    If dRetirementTotal > 0 And dSavingTotal > 0 Then
                    '        PlanType = "Both"
                    '    ElseIf dRetirementTotal > 0 And dSavingTotal <= 0 Then
                    '        PlanType = "Retirement"
                    '    ElseIf dSavingTotal > 0 And dRetirementTotal Then
                    '        PlanType = "Saving"
                    '    End If
                    'ElseIf RadioButtonListPlanTypes.SelectedValue = "Both" And dRetirementTotal <= 5000 And dSavingTotal > 5000 Then
                    '    PlanType = "Retirement"
                    'ElseIf RadioButtonListPlanTypes.SelectedValue = "Both" And dRetirementTotal > 5000 And dSavingTotal <= 5000 Then
                    '    PlanType = "Saving"
                    'ElseIf RadioButtonListPlanTypes.SelectedValue = "Retirement" And dRetirementTotal <= 5000 Then
                    '    PlanType = "Retirement"
                    'ElseIf RadioButtonListPlanTypes.SelectedValue = "Savings" And dSavingTotal <= 5000 Then
                    '    PlanType = "Saving"
                    'End If
                    If retirementTotal <= 5000 And savingsTotal <= 5000 Then
                        If retirementTotal > 0 And savingsTotal > 0 Then
                            planType = "Both"
                        ElseIf retirementTotal > 0 And savingsTotal <= 0 Then
                            planType = "Retirement"
                        ElseIf savingsTotal > 0 And retirementTotal <= 0 Then
                            planType = "Savings"
                        End If
                    ElseIf retirementTotal <= 5000 Then
                        planType = "Retirement"
                    ElseIf savingsTotal <= 5000 Then
                        planType = "Savings"
                    End If
                    'END: PPP | 08/30/2016 | YRS-AT-2529 | Selected plan type value is in hidden field

                    Dim Res As String = SaveArraylistPropertys(arrayOfRetirementAccounts, arrayOfSavingAccounts, recipientFundEventID, planType)
                    'Added By SG: 2013.01.10: BT-1547
                    'objWebServiceReturn = objService.SaveRefundRequest(Res, True)
                    'Start:AA:11.18.2015 YRS-At-2639 Changed below code to use new yrs method from YRS withdrawal phase 2
                    'objWebServiceReturn = objService.SaveRefundRequest(Res, False)
                    serviceOutput = withdrawalService.SaveRefundRequestForYRS(Res, False)
                    'End:AA:11.18.2015 YRS-At-2639 Changed below code to use new yrs method from YRS withdrawal phase 2
                    If serviceOutput.ReturnStatus.Equals(YRSWebService.Status.Success) Then
                        Session("ServiceRequestReport") = serviceOutput.WebServiceDataSet
                        requestStatus = "Success"
                    ElseIf serviceOutput.ReturnStatus.Equals(YRSWebService.Status.Warning) Then
                        If (Not serviceOutput.MessageCode Is Nothing) AndAlso (serviceOutput.MessageCode.ToUpper() = "MESSAGE_WITHDRAWAL_CopyToServerError".ToUpper()) Then
                            Session("ServiceRequestReport") = serviceOutput.WebServiceDataSet
                        End If
                        Session("GenerateErrors") = serviceOutput.Message
                        requestStatus = "Success"
                        'Return True
                    ElseIf serviceOutput.ReturnStatus.Equals(YRSWebService.Status.Failure) Then
                        requestStatus = "Failure"
                    End If

                    'SG: 2012.11.30: BT-1436
                    If Not serviceOutput.WebServiceDataSet Is Nothing Then
                        If serviceOutput.WebServiceDataSet.Tables.Count > 0 AndAlso serviceOutput.WebServiceDataSet.Tables("ListOfParameter").Rows.Count > 0 Then
                            If serviceOutput.WebServiceDataSet.Tables("ListOfParameter").Rows(0)("ParaName").ToString().ToUpper() = "REFREQUESTID" Then
                                Me.String_RefRequestID = serviceOutput.WebServiceDataSet.Tables("ListOfParameter").Rows(0)("ParaValue").ToString()
                            End If
                        End If
                    End If
                    'SG: 2012.11.30: BT-1436

                    If requestStatus.ToUpper() = "SUCCESS" Then
                        YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.UpdateQDRONonRetSplitTable(Me.String_QDRORequestID, Me.String_RefRequestID, recipientFundEventID, planType)
                    End If
                End If

            End If

            Return requestStatus
        Catch ex As Exception
            HelperFunctions.LogException("DataListParticipant_ItemDataBound", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            withdrawalService.Dispose()
            refundRequestPageInstance.Dispose()
        End Try

    End Function
    'Harshala : 26/04/2012 : YRS 5.0-1346:Cash out plan balance <= $5,000 : END

    Public Function SaveArraylistPropertys(ByVal arrayOfRetirementAccounts As String(), ByVal arrayOfSavingsAccounts As String(), ByVal funEventID As String, ByVal planType As String) As String
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured SaveArraylistPropertys. Old code can be checked from TF.
        Dim serviceProperty As New RefundRequestWebForm.SavePropertys
        Dim xmlProperties As String
        Dim refundRequestPageInstance As RefundRequestWebForm
        Try
            serviceProperty.FundEventID = funEventID

            If planType = "Retirement" Or planType = "Both" Then
                serviceProperty.RetirementPlanWithdrawalType = "REG"
            Else
                serviceProperty.RetirementPlanWithdrawalType = ""
            End If

            If planType = "Savings" Or planType = "Both" Then
                serviceProperty.SavingsPlanWithdrawalType = "VOL"
            Else
                serviceProperty.SavingsPlanWithdrawalType = ""
            End If

            serviceProperty.SelectedRetirementPlanAccounts = arrayOfRetirementAccounts
            serviceProperty.SelectedSavingsPlanAccounts = arrayOfSavingsAccounts
            'Bt:961- Taxable amount updating wrongly in atsrefrequestperplan table
            serviceProperty.RetirementPlanPartialAmount = 0.0
            serviceProperty.SavingsPlanPartialAmount = 0.0
            'Dinesh k               2015.02.15           BT:2804: YRS 5.0-2483:RL_2_FullRefundQDRO Letter not generating from YRS.
            serviceProperty.WithdrawalRequest = "qdro"

            refundRequestPageInstance = New RefundRequestWebForm
            xmlProperties = refundRequestPageInstance.ConvertToXML(serviceProperty)
            xmlProperties = xmlProperties.Replace("<string>", "<AcctId>")
            xmlProperties = xmlProperties.Replace("</string>", "</AcctId>")

            Return xmlProperties
        Catch
            Throw
        Finally
            xmlProperties = Nothing
            serviceProperty = Nothing
            refundRequestPageInstance = Nothing
        End Try
    End Function

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    Private Sub ClearDataTable()
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured ClearDataTable. Old code can be checked from TF.
        DatagridBenificiaryList.DataSource = Nothing
        DatagridBenificiaryList.DataBind()
        'Me.Session_bool_MissingFundEventId = Nothing 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
        Me.Session_dataset_AnnuityBasisDetail = Nothing
        Me.Session_datatable_dtPartAccount = Nothing
        Me.Session_dataset_dsAllPartAccountsDetail = Nothing
        Me.Session_ComboValue = Nothing
        Me.String_Benif_SSno = Nothing
        Me.Session_finTot = Nothing
        Me.Session_Dataset_PartAccountDetail = Nothing
        'Me.string_FundEventID = Nothing 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
        'Me.string_PersId = Nothing 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
        Me.Session_dataset_dsAllRecipantAccountsDetail = Nothing
        Me.String_PhonySSNo = Nothing
        'Me.String_Part_SSN = Nothing 'PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Session variable is not in use
        Me.Session_datatable_dtBenifAccount = Nothing
        'Me.string_PersSSID = Nothing 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
        Me.String_Benif_PersonD = Nothing
        'Me.String_QDRORequestID = Nothing
        Me.Session_datatable_dtPecentageCount = Nothing
        Me.Session_bool_NewPerson = Nothing
        Me.string_RecptPersID = Nothing
        Me.string_RecptFundEventID = Nothing
        Me.Session_dataset_ParticipantDetails = Nothing
        Me.SessionPageCount = Nothing
        Me.Session_dataset_GroupAAnnuityBasisDetail = Nothing
        Me.Session_dataset_GroupBAnnuityBasisDetail = Nothing
        Me.Session_dataset_dsALLGroupBRecipantDetails() = Nothing
        Me.Session_dataset_dsALLGroupBParticipantDetails() = Nothing
        Me.Session_dataset_dsALLGroupAParticipantDetails() = Nothing
        Me.Session_dataset_dsALLGroupARecipantDetails() = Nothing
        'SG: 2012.11.30: BT-1436
        Me.String_RefRequestID = Nothing
        Session("NR_QDRO_PersID") = Nothing

        hdnSelectedPlanType.Value = String.Empty
        'START: PPP | 09/29/2016 | YRS-AT-2529 | Resetting both calendar control dates
        PopcalendarRecDate.SelectedDate = "12/31/1995"
        PopcalendarRecDate2.SelectedDate = Date.Today
        'END: PPP | 09/29/2016 | YRS-AT-2529 | Resetting both calendar control dates
        'START : MMR | 2016.12.07 | YRS-AT-3145 | Clearing session values
        Me.ParticipantTotalBalanceAfterSplitManageFees = Nothing
        Me.RecipientBalanceAfterSplitManageFees = Nothing

        hdnFees.Value = String.Empty
        TextboxEndDate.Text = String.Empty
        chkApplyFees.Checked = False
        ChkAdjustInterest.Checked = False
        'END : MMR | 2016.12.07 | YRS-AT-3145 | Clearing session values
    End Sub

    Private Sub ClearSessionData()
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured ClearSessionData. Old code can be checked from TF.
        DatagridBenificiaryList.DataSource = Nothing
        DatagridBenificiaryList.DataBind()
        'Me.Session_bool_MissingFundEventId = Nothing 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
        Me.Session_dataset_AnnuityBasisDetail = Nothing
        Me.Session_datatable_dtPartAccount = Nothing
        Me.Session_dataset_dsAllPartAccountsDetail = Nothing
        Me.String_Benif_SSno = Nothing
        Me.Session_ComboValue = Nothing
        Me.Session_finTot = Nothing
        Me.Session_Dataset_PartAccountDetail = Nothing
        'Me.string_FundEventID = Nothing 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
        'Me.string_PersId = Nothing 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
        Me.Session_dataset_dsAllRecipantAccountsDetail = Nothing
        Me.String_PhonySSNo = Nothing
        'Me.String_Part_SSN = Nothing 'PPP | 12/29/2016 | YRS-AT-3145 & 3265 | Session variable is not in use
        Me.Session_datatable_dtBenifAccount = Nothing
        'Me.string_PersSSID = Nothing 'PPP | 01/03/2017 | YRS-AT-3145 & 3265 | Not in use
        Me.String_Benif_PersonD = Nothing
        'Me.String_QDRORequestID = Nothing
        Me.Session_datatable_dtPecentageCount = Nothing
        Me.Session_bool_NewPerson = Nothing
        Me.string_RecptPersID = Nothing
        Me.string_RecptFundEventID = Nothing
        Me.Session_dataset_ParticipantDetails = Nothing
        'Me.Session_String_ISCompleted = Nothing 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
        Me.SessionPageCount = Nothing
        Me.Session_dataset_GroupAAnnuityBasisDetail = Nothing
        Me.Session_dataset_GroupBAnnuityBasisDetail = Nothing
        Me.Session_dataset_dsALLGroupBRecipantDetails() = Nothing
        Me.Session_dataset_dsALLGroupBParticipantDetails() = Nothing
        Me.Session_dataset_dsALLGroupAParticipantDetails() = Nothing
        Me.Session_dataset_dsALLGroupARecipantDetails() = Nothing
    End Sub
#End Region

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

#Region "BENEFICIERY COMBO BOX SELECTED CHANGED EVENT"
    'This event is used for retrive Beneficiary details
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used when the beneficiary combo is changed .                                //
    '***************************************************************************************************//
    Private Sub cboBeneficiarySSNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBeneficiarySSNo.SelectedIndexChanged
        'Start - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Added try catch block for Logging exception error message
        Try
            'START: PPP | 09/12/2016 | YRS-AT-2529 | Commented old code
            ''Added by Amit. On Nov 12th 2008
            ''******************
            'LoadBeneficiaryDataDropDown()
            ''******************
            'END: PPP | 09/12/2016 | YRS-AT-2529 | Commented old code
            hdnSelectedPlanType.Value = String.Empty ' This will load default page for selected beneficiary
            SetBeneficiaryData()
            ShowHideControls()
            ShowOtherPlanReminderWarning() 'PPP | 12/29/2016 | YRS-AT-3145 & 3265 
        Catch ex As Exception
            HelperFunctions.LogException("cboBeneficiarySSNo_SelectedIndexChanged", ex) 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
        'End - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Added try catch block for Logging exception error message
    End Sub

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    '***************************************************************************************************//
    'Function Name             :Check Account                     Created on  : 11/12/08                //
    'Created By                :Amit Nigam                           Modified On :                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Param Description         :                                                                        //
    'Method Description        :This Function is used select Accounts for which the split is done for   //
    '                           for the selected Recipient                                            //
    '***************************************************************************************************//
    Private Function CheckAccounts(ByVal drRecps() As DataRow)
        Dim drRecp As DataRow
        Dim dgItem As DataGridItem
        Try
            For Each drRecp In drRecps
                For Each dgItem In DataGridWorkSheets.Items
                    Dim SelectCheckbox As CheckBox = CType(dgItem.Cells(0).Controls(1), CheckBox)
                    If dgItem.Cells(1).Text = drRecp("Accttype") Then
                        SelectCheckbox.Checked = True
                        Exit For
                    End If
                Next
            Next
        Catch
            Throw
        End Try
    End Function

#End Region

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

#Region "GET GRID CHECKBOX STATUS"
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                          //
    'Created By                :Ganeswar Sahoo            Modified On :                                      //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to validate the accttype check box whether it is selected or not.                               //
    '***************************************************************************************************//
    Private Function CheckValidate()
        Try
            Dim dgItem As DataGridItem
            For Each dgItem In DataGridWorkSheets.Items
                Dim SelectCheckbox As CheckBox = CType(dgItem.Cells(0).Controls(1), CheckBox)
                If SelectCheckbox.Checked = True Then
                    Return True
                End If
                Return False
            Next
        Catch
            Throw
        End Try
    End Function

#End Region

#Region "LOAD BENEFICIERY DETAILS"
    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    'Start - Manthan Rajguru| 2016.08.16 | YRS-AT-2482 |Loading Marital status and gender
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
    'End - Manthan Rajguru| 2016.08.16 | YRS-AT-2482 |Loading Marital status and gender
#End Region

#Region "SPLIT OPERATION"
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo            Modified On :                                 //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :Used to Make the necessary calculations for PRE96 and PST96 Split .                     //
    '***************************************************************************************************//
    Private Sub PerformSplitOperation()
        'PPP | 09/12/2016 | YRS-AT-2529 | Renamed old "PerformParticipantSplit" as "PerformSplitOperation" and restructured it. Old code can be checked from TF.
        Dim splitBalance As Decimal = 0
        Dim participantAccountRowIndex As Integer
        Dim percentage As Decimal
        Dim rows() As DataRow
        Dim row As DataRow
        Dim transactionRow As DataRow
        Dim accountingDate As DateTime
        Dim transactDate As DateTime
        Dim fundedDate As String
        Dim transactionRow1 As DataRow
        Dim transactionRow2 As DataRow
        Dim transactionRow3 As DataRow
        Dim recipientRow As DataRow
        Dim transactionRow6 As DataRow
        Dim recipientPersID As String
        Dim amount As Decimal = 0
        Dim rowCounter As Integer
        Dim finalTotal As Decimal = 0
        Dim participantRowCounter As Integer
        Dim beneficiaryItems As DataGridItem
        Dim accountType As String
        Dim annuityBasisType As String
        Dim deleteRows As DataRow()
        Dim areRowsDeleted As Boolean
        Dim currentPlanType As String

        Dim accountCheckBox As CheckBox
        Try
            DataGridWorkSheet2.DataSource = Nothing
            DataGridWorkSheet2.DataBind()

            dtPartAccount = Me.Session_datatable_dtPartAccount.Copy 'PPP | 09/30/2016 | YRS-AT-2529 | Added ".Copy" 

            finalTotal = GetSelectedAccountsTotal()
            If finalTotal < 0 Then
                Exit Sub
            End If

            For i As Integer = 0 To DatagridBenificiaryList.Items.Count - 1
                If DatagridBenificiaryList.Items(i).Cells(recipientIndexPersID).Text.ToString() = cboBeneficiarySSNo.SelectedItem.Value Then 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Changed the index access from hardcoded 1 to by a variable recipientIndexPersID
                    Me.string_RecptFundEventID = DatagridBenificiaryList.Items(i).Cells(recipientIndexFundEventID).Text.ToString() 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Changed the index access from hardcoded 6 to by a variable recipientIndexFundEventID
                    Exit For
                End If
            Next

            If CType(TextBoxAmountWorkSheet.Text, Decimal) > finalTotal Then
                'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                'MessageBox.Show(PlaceHolder1, "QDRO", "Enter a valid Amount to split", MessageBoxButtons.OK, False)
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_ENTER_VALID_AMOUNT_TO_SPLIT"), MessageBoxButtons.OK, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_ENTER_VALID_AMOUNT_TO_SPLIT", "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                Exit Sub
            End If
            If RadioButtonListSplitAmtType_Amount.Checked Then
                If TextBoxAmountWorkSheet.Text = "" Then
                    amount = 0
                Else
                    amount = CType(TextBoxAmountWorkSheet.Text, Decimal)
                End If
                percentage = amount / finalTotal
            ElseIf RadioButtonListSplitAmtType_Percentage.Checked Then
                percentage = (TextBoxPercentageWorkSheet.Text) / 100
                'START: PPP | 09/30/2016 | YRS-AT-2529 | Rounding off amount in case of % split
                'amount = finalTotal * percentage
                amount = Math.Round(finalTotal * percentage, 2, MidpointRounding.AwayFromZero)
                'END: PPP | 09/30/2016 | YRS-AT-2529 | Rounding off amount in case of % split
            End If
            splitBalance = 0
            dsTransactions = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getTransactionsQDRO(Me.string_FundEventID) ', TextboxEndDate.Text)
            Me.String_Benif_PersonD = cboBeneficiarySSNo.SelectedValue

            If Me.Session_dataset_AnnuityBasisDetail Is Nothing Then
                l_dataset_AnnuityBasisDetail = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getAnnuityBasisDetail(Me.string_FundEventID, TextboxBegDate.Text, TextboxEndDate.Text, "Both")
                Me.Session_dataset_AnnuityBasisDetail = l_dataset_AnnuityBasisDetail.Copy
            Else
                l_dataset_AnnuityBasisDetail = Me.Session_dataset_AnnuityBasisDetail.Copy
            End If
            areRowsDeleted = False
            For participantAccountRowIndex = 0 To l_dataset_AnnuityBasisDetail.Tables(0).Rows.Count - 1
                If Not l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).RowState = DataRowState.Deleted Then
                    accountType = l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("AcctType")
                    If dtPartAccount.Select(String.Format("AcctType='{0}'", accountType)).Length = 0 Then
                        deleteRows = l_dataset_AnnuityBasisDetail.Tables(0).Select(String.Format("AcctType='{0}'", accountType))
                        For Each rowToDelete As DataRow In deleteRows
                            rowToDelete.Delete()
                            areRowsDeleted = True
                        Next
                    End If
                End If
            Next
            If areRowsDeleted Then
                l_dataset_AnnuityBasisDetail.AcceptChanges()
            End If

            dsTransactions.Tables(0).Columns.Add("PlanType", GetType(System.String))
            dsParticipantDetails = dsTransactions.Copy
            dsParticipantDetails.Tables(0).Rows.Clear()
            dsRecipantDetails = dsTransactions.Copy
            dsRecipantDetails.Tables(0).Rows.Clear()
            dsParticipantDetails.Tables(0).Columns.Add("BenfitPersId", GetType(System.String))

            l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail

            If splitBalance < amount Then
                For participantAccountRowIndex = 0 To l_dataset_AnnuityBasisDetail.Tables(0).Rows.Count - 1
                    accountType = l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("AcctType")
                    annuityBasisType = l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("AnnuityBasisType")

                    If dtPartAccount.Select(String.Format("AcctType='{0}'", accountType)).Length = 0 Then
                        'Anudeep:18.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                        'MessageBox.Show(PlaceHolder1, "QDRO", "Split Not Possible", MessageBoxButtons.OK, False)
                        'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_SPLIT_NOT_POSSIBE"), MessageBoxButtons.OK, False)
                        ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_SPLIT_NOT_POSSIBE", "error")
                        'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        Exit Sub
                    End If
                    transactionRow1 = dsParticipantDetails.Tables(0).NewRow
                    transactionRow3 = dsParticipantDetails.Tables(0).NewRow
                    transactionRow6 = dsRecipantDetails.Tables(0).NewRow
                    recipientRow = dsRecipantDetails.Tables(0).NewRow
                    transactionRow = dsTransactions.Tables(0).NewRow

                    For participantRowCounter = 0 To dtPartAccount.Rows.Count - 1
                        beneficiaryItems = DataGridWorkSheets.Items(participantRowCounter)
                        accountCheckBox = CType(beneficiaryItems.Cells(0).Controls(1), CheckBox)
                        If accountCheckBox.Checked = True Then
                            If dtPartAccount.Rows(participantRowCounter).Item("AcctType") = accountType Then

                                currentPlanType = GetPlanTypeForSelectedAccount(l_dataset_PartAccountDetail.Tables(0), accountType)

                                transactionRow("PersID") = Me.String_Benif_PersonD
                                transactionRow("FundEventID") = Me.string_RecptFundEventID
                                transactionRow("AcctType") = accountType
                                transactionRow("TransactType") = "QSPR"
                                If transactionRow("AcctType") = "RT" Then
                                    transactionRow("AnnuityBasisType") = annuityBasisType
                                Else
                                    transactionRow("AnnuityBasisType") = "PST96"
                                End If
                                transactionRow("TransactDate") = TextboxEndDate.Text
                                transactionRow("MonthlyComp") = 0.0
                                If Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPreTax") * percentage, 2) < (amount - splitBalance) Then
                                    transactionRow("PersonalPreTax") = Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPreTax") * percentage, 2)
                                Else
                                    transactionRow("PersonalPreTax") = amount - splitBalance
                                End If

                                splitBalance = splitBalance + transactionRow("PersonalPreTax")
                                If Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPostTax") * percentage, 2) <= (amount - splitBalance) Then
                                    transactionRow("PersonalPostTax") = Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPostTax") * percentage, 2)
                                Else
                                    transactionRow("PersonalPostTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow("PersonalPostTax")
                                If Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAPreTax") * percentage, 2) <= (amount - splitBalance) Then
                                    transactionRow("YMCAPreTax") = Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAPreTax") * percentage, 2)
                                Else
                                    transactionRow("YMCAPreTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow("YMCAPreTax")
                                transactionRow("ReceivedDate") = Date.Now
                                transactionRow("TransmittalID") = System.DBNull.Value
                                transactionRow("Created") = System.DBNull.Value
                                transactionRow("Creator") = "Test"
                                transactionRow("Updated") = System.DBNull.Value
                                transactionRow("Updater") = System.DBNull.Value
                                transactionRow("PlanType") = currentPlanType
                                '****************************************************************
                                If transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax") > 0.0 Then
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalPreTax") = row("PersonalPreTax") - transactionRow("PersonalPreTax")
                                    row("PersonalPostTax") = row("PersonalPostTax") - transactionRow("PersonalPostTax")
                                    row("PersonalTotal") = row("PersonalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax"))
                                    row("YMCAPreTax") = row("YMCAPreTax") - transactionRow("YMCAPreTax")
                                    row("YMCATotal") = row("YMCATotal") - transactionRow("YMCAPreTax")
                                    row("TotalTotal") = row("TotalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax"))
                                    If Not transactionRow Is Nothing Then
                                        For rowCounter = 0 To transactionRow.ItemArray.Length - 1
                                            recipientRow(rowCounter) = transactionRow(rowCounter)
                                        Next
                                    Else
                                        recipientRow("MonthlyComp") = 0.0
                                    End If
                                    recipientRow("AnnuityBasisType") = annuityBasisType
                                    recipientRow("AcctType") = accountType
                                    recipientRow("PersID") = Me.String_Benif_PersonD
                                    recipientRow("FundEventId") = Me.string_RecptFundEventID
                                    recipientRow("TransactType") = "QSPR"
                                    recipientRow("PlanType") = currentPlanType
                                    dsRecipantDetails.Tables(0).Rows.Add(recipientRow)
                                    'Session("dsRecipantDetails") = dsRecipantDetails
                                End If
                                '*****************************************************************
                                If transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax") > 0.0 Then
                                    dsTransactions.Tables(0).Rows.Add(transactionRow)
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalPreTax") = row("PersonalPreTax") - transactionRow("PersonalPreTax")
                                    row("PersonalPostTax") = row("PersonalPostTax") - transactionRow("PersonalPostTax")
                                    row("PersonalTotal") = row("PersonalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax"))
                                    row("YMCAPreTax") = row("YMCAPreTax") - transactionRow("YMCAPreTax")
                                    row("YMCATotal") = row("YMCATotal") - transactionRow("YMCAPreTax")
                                    row("TotalTotal") = row("TotalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax"))
                                    If Not transactionRow Is Nothing Then
                                        For rowCounter = 0 To transactionRow.ItemArray.Length - 1
                                            transactionRow1(rowCounter) = transactionRow(rowCounter)
                                        Next
                                    Else
                                        transactionRow1("MonthlyComp") = 0.0
                                    End If
                                    transactionRow1("AnnuityBasisType") = annuityBasisType
                                    transactionRow1("AcctType") = accountType
                                    transactionRow1("PersID") = Session("PersId")
                                    transactionRow1("FundEventId") = Me.string_FundEventID
                                    transactionRow1("TransactType") = "QWPR"
                                    transactionRow1("PersonalPreTax") = transactionRow("PersonalPreTax") * (-1)
                                    transactionRow1("PersonalPostTax") = transactionRow("PersonalPostTax") * (-1)
                                    transactionRow1("YMCAPreTax") = transactionRow("YMCAPreTax") * (-1)
                                    transactionRow1("BenfitPersId") = Me.String_Benif_PersonD
                                    transactionRow1("PlanType") = currentPlanType
                                    dsParticipantDetails.Tables(0).Rows.Add(transactionRow1)
                                End If
                                '***************************************************************************
                                transactionRow2 = dsTransactions.Tables(0).NewRow
                                If Not transactionRow1 Is Nothing Then
                                    For rowCounter = 0 To transactionRow1.ItemArray.Length - 2
                                        transactionRow2(rowCounter) = transactionRow1(rowCounter)
                                    Next
                                Else
                                    transactionRow2("MonthlyComp") = 0.0
                                End If
                                If transactionRow("AcctType") = "RT" Then
                                    transactionRow2("AnnuityBasisType") = annuityBasisType
                                Else
                                    transactionRow2("AnnuityBasisType") = "PST96"
                                End If
                                transactionRow2("AcctType") = accountType
                                transactionRow2("PersID") = Me.String_Benif_PersonD
                                transactionRow2("FundEventId") = Me.string_RecptFundEventID
                                transactionRow2("TransactType") = "QSIN"
                                If Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalInterestBalance") * percentage, 2) <= amount - splitBalance Then
                                    transactionRow2("PersonalPreTax") = Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalInterestBalance") * percentage, 2)
                                Else
                                    transactionRow2("PersonalPreTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow2("PersonalPreTax")
                                transactionRow2("PersonalPostTax") = 0.0
                                If Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAInterestBalance") * percentage, 2) <= amount - splitBalance Then
                                    transactionRow2("YMCAPreTax") = Math.Round(l_dataset_AnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAInterestBalance") * percentage, 2)
                                Else
                                    transactionRow2("YMCAPreTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow2("YMCAPreTax")
                                If (transactionRow2("PersonalPreTax") + transactionRow2("YMCAPreTax")) > 0 Then
                                    transactionRow2("PlanType") = currentPlanType
                                    dsTransactions.Tables(0).Rows.Add(transactionRow2)
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalInterestBalance") = row("PersonalInterestBalance") - transactionRow2("PersonalPreTax")
                                    row("PersonalTotal") = row("PersonalTotal") - transactionRow2("PersonalPreTax")
                                    row("YMCAInterestBalance") = row("YMCAInterestBalance") - transactionRow2("YMCAPreTax")
                                    row("YMCATotal") = row("YMCATotal") - transactionRow2("YMCAPreTax")
                                    row("TotalTotal") = row("TotalTotal") - (transactionRow2("PersonalPreTax") + transactionRow2("PersonalPostTax") + transactionRow2("YMCAPreTax"))
                                End If
                                If Not transactionRow2 Is Nothing Then
                                    For rowCounter = 0 To transactionRow2.ItemArray.Length - 1
                                        transactionRow3(rowCounter) = transactionRow2(rowCounter)
                                    Next
                                Else
                                    transactionRow3("MonthlyComp") = 0.0
                                End If
                                transactionRow3("AnnuityBasisType") = annuityBasisType
                                transactionRow3("AcctType") = accountType
                                transactionRow3("PersId") = Session("PersId")
                                transactionRow3("FundEventID") = Me.string_FundEventID
                                transactionRow3("TransactType") = "QWIN"
                                transactionRow3("PersonalPreTax") = transactionRow2("PersonalPreTax") * (-1)
                                transactionRow3("YMCAPreTax") = transactionRow2("YMCAPreTax") * (-1)
                                transactionRow3("BenfitPersId") = Me.String_Benif_PersonD
                                transactionRow3("PlanType") = currentPlanType
                                dsParticipantDetails.Tables(0).Rows.Add(transactionRow3)
                                '***************************************************************************
                                If (transactionRow2("PersonalPreTax") + transactionRow2("YMCAPreTax")) > 0 Then
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalInterestBalance") = row("PersonalInterestBalance")
                                    row("PersonalTotal") = row("PersonalTotal")
                                    row("YMCAInterestBalance") = row("YMCAInterestBalance")
                                    row("YMCATotal") = row("YMCATotal")
                                    row("TotalTotal") = row("TotalTotal")
                                End If
                                If Not transactionRow2 Is Nothing Then
                                    For rowCounter = 0 To transactionRow2.ItemArray.Length - 1
                                        transactionRow6(rowCounter) = transactionRow2(rowCounter)
                                    Next
                                Else
                                    transactionRow6("MonthlyComp") = 0.0
                                End If
                                transactionRow6("AnnuityBasisType") = annuityBasisType
                                transactionRow6("AcctType") = accountType
                                transactionRow6("PersID") = Me.String_Benif_PersonD
                                transactionRow6("FundEventId") = Me.string_RecptFundEventID
                                transactionRow6("TransactType") = "QSIN"
                                transactionRow6("PlanType") = currentPlanType
                                dsRecipantDetails.Tables(0).Rows.Add(transactionRow6)
                            End If
                        End If
                    Next
                Next
            End If

            YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.RoundOffAmount(dsParticipantDetails) ' Required to resolve Decimal datatype issue
            If Not Session_dataset_dsAllPartAccountsDetail Is Nothing Then
                dsAllPartAccountsDetail = Me.Session_dataset_dsAllPartAccountsDetail
                'START: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsAllPartAccountsDetail rows count checking dsParticipantDetails rows count
                'If dsParticipantDetails.Tables(0).Rows.Count > 0 Then 
                If HelperFunctions.isNonEmpty(dsParticipantDetails) AndAlso dsParticipantDetails.Tables(0).Rows.Count > 0 Then
                    'END: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsAllPartAccountsDetail rows count checking dsParticipantDetails rows count
                    For i As Integer = 0 To dsParticipantDetails.Tables(0).Rows.Count - 1
                        dsAllPartAccountsDetail.Tables(0).ImportRow(dsParticipantDetails.Tables(0).Rows(i))
                    Next
                End If
                Me.Session_dataset_dsAllPartAccountsDetail = dsAllPartAccountsDetail
            Else
                Me.Session_dataset_dsAllPartAccountsDetail = dsParticipantDetails.Copy
            End If

            YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.RoundOffAmount(dsRecipantDetails) ' Required to resolve Decimal datatype issue
            If Not Session_dataset_dsAllRecipantAccountsDetail Is Nothing Then
                dsAllRecipantAccountsDetail = Me.Session_dataset_dsAllRecipantAccountsDetail
                'START: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsAllRecipantAccountsDetail rows count checking dsRecipantDetails rows count
                'If dsAllRecipantAccountsDetail.Tables(0).Rows.Count > 0 Then
                If HelperFunctions.isNonEmpty(dsRecipantDetails) AndAlso dsRecipantDetails.Tables(0).Rows.Count > 0 Then
                    'END: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsAllRecipantAccountsDetail rows count checking dsRecipantDetails rows count
                    For i As Integer = 0 To dsRecipantDetails.Tables(0).Rows.Count - 1
                        dsAllRecipantAccountsDetail.Tables(0).ImportRow(dsRecipantDetails.Tables(0).Rows(i))
                    Next
                End If
                Me.Session_dataset_dsAllRecipantAccountsDetail = dsAllRecipantAccountsDetail
            Else
                Me.Session_dataset_dsAllRecipantAccountsDetail = dsRecipantDetails.Copy
            End If

            PerformGroupASplitOperation(percentage) ' in case of "split by amount", program calculates % againts total amount and pass it on to A and B group split
            PerformGroupBSplitOperation(percentage)
        Catch
            Throw
        End Try
    End Sub
#End Region

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

#Region "ADJUST BUTTON CLICK"
    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo            Modified On :                                     //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :This Event will call when user click on the AdJust button.              //
    '***************************************************************************************************//
    Private Sub ButtonAdjust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdjust.Click
        'PPP | 09/12/2016 | YRS-AT-2529 | Commented all unused code
        'START: PPP | 09/13/2016 | YRS-AT-1973
        Try
            Adjust()
        Catch ex As Exception
            HelperFunctions.LogException("ButtonAdjust_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())), False)
        End Try
        'END: PPP | 09/13/2016 | YRS-AT-1973
    End Sub
#End Region

#Region "RESET BUTTON CLICK"
    '***************************************************************************************************//
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                       //
    'Created By                :Ganeswar Sahoo            Modified On :                                     //
    'Modified By               :                                                                        //
    'Modify Reason             :                                                                        //
    'Constructor Description   :                                                                        //
    'Class Description         :'This Event will fire when the user will click on the Reset Button.     //
    '***************************************************************************************************//
    Private Sub ButtonReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonReset.Click
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured ButtonReset_Click. Old code can be checked from TF.
        Try
            ResetSelectedPlan()
            ShowHideControls()
        Catch ex As Exception
            HelperFunctions.LogException("ButtonReset_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())), False)
        End Try
    End Sub
#End Region

#Region "SPLIT FOR GROUPA AND GROUPB"
    '*************************************************************************************************** //
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                        //
    'Created By                :Ganeswar Sahoo            Modified On :                                  //
    'Modified By               :                                                                         //
    'Modify Reason             :                                                                         //
    'Constructor Description   :                                                                         //
    'Class Description         :Used to Make the necessary calculations for PRE96 and PST96 Split for GroupA .                     //
    '***************************************************************************************************//
    Private Sub PerformGroupASplitOperation(ByVal percentage As Decimal)
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured PerformGroupASplitOperation. Old code can be checked from TF.
        Dim splitBalance As Decimal = 0
        Dim participantAccountRowIndex As Integer
        Dim rows() As DataRow
        Dim row As DataRow
        Dim transactionRow As DataRow
        Dim accountingDate As DateTime
        Dim transactDate As DateTime
        Dim fundedDate As String
        Dim transactionRow1 As DataRow
        Dim transactionRow2 As DataRow
        Dim transactionRow3 As DataRow
        Dim recipientRow As DataRow
        Dim transactionRow6 As DataRow
        Dim recipientPersID As String
        Dim amount As Decimal = 0
        Dim rowCounter As Integer
        Dim finalTotal As Decimal = 0
        Dim participantRowCounter As Integer
        Dim beneficiaryItems As DataGridItem
        Dim accountType As String
        Dim annuityBasisType As String
        Dim deleteRows As DataRow()
        Dim areRowsDeleted As Boolean
        Dim currentPlanType As String
        Try
            DataGridWorkSheet2.DataSource = Nothing
            DataGridWorkSheet2.DataBind()

            If Me.Session_dataset_GroupAAnnuityBasisDetail Is Nothing Then
                l_dataset_GroupAAnnuityBasisDetail = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getGroupAPartAccountDetailbyPlan(Me.string_FundEventID, TextboxBegDate.Text, TextboxEndDate.Text, "Both")
                Me.Session_dataset_GroupAAnnuityBasisDetail = l_dataset_GroupAAnnuityBasisDetail.Copy
            Else
                l_dataset_GroupAAnnuityBasisDetail = Me.Session_dataset_GroupAAnnuityBasisDetail.Copy
            End If

            dtPartAccount = Me.Session_datatable_dtPartAccount.Copy 'PPP | 09/30/2016 | YRS-AT-2529 | Added ".Copy" 
            areRowsDeleted = False
            For participantAccountRowIndex = 0 To l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows.Count - 1
                If Not l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).RowState = DataRowState.Deleted Then
                    accountType = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("AcctType")
                    If dtPartAccount.Select(String.Format("AcctType='{0}'", accountType)).Length = 0 Then
                        deleteRows = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Select(String.Format("AcctType='{0}'", accountType))
                        For Each rowToDelete As DataRow In deleteRows
                            rowToDelete.Delete()
                            areRowsDeleted = True
                        Next
                    End If
                End If
            Next
            If areRowsDeleted Then
                l_dataset_GroupAAnnuityBasisDetail.AcceptChanges()
            End If

            For participantRowCounter = 0 To dtPartAccount.Rows.Count - 1
                accountType = dtPartAccount.Rows(participantRowCounter)("AcctType")
                For intcnt As Integer = 0 To l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows.Count - 1
                    If accountType = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(intcnt)("AcctType") Then
                        dblGroupATotal = dblGroupATotal + l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(intcnt)("mnyBalance")
                    End If
                Next
            Next

            finalTotal = dblGroupATotal
            Me.Session_finTot = finalTotal
            If finalTotal < 0 Then
                Exit Sub
            End If
            amount = finalTotal * percentage
            splitBalance = 0
            dsTransactions = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getTransactionsQDRO(Me.string_FundEventID) ', TextboxEndDate.Text)
            Me.String_Benif_PersonD = cboBeneficiarySSNo.SelectedValue

            dsTransactions.Tables(0).Columns.Add("PlanType", GetType(System.String))
            dsGroupAParticipantDetails = dsTransactions.Copy
            dsGroupAParticipantDetails.Tables(0).Rows.Clear()
            dsGroupARecipantDetails = dsTransactions.Copy
            dsGroupARecipantDetails.Tables(0).Rows.Clear()

            dsGroupAParticipantDetails.Tables(0).Columns.Add("BenfitPersId", GetType(System.String))
            'dtPartAccount = Me.Session_datatable_dtPartAccount
            If splitBalance < amount Then
                l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail
                For participantAccountRowIndex = 0 To l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows.Count - 1
                    accountType = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("AcctType")
                    annuityBasisType = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("AnnuityBasisType")
                    If dtPartAccount.Select(String.Format("AcctType='{0}'", accountType)).Length = 0 Then
                        Exit Sub
                    End If
                    transactionRow1 = dsGroupAParticipantDetails.Tables(0).NewRow
                    transactionRow3 = dsGroupAParticipantDetails.Tables(0).NewRow
                    transactionRow6 = dsGroupARecipantDetails.Tables(0).NewRow
                    recipientRow = dsGroupARecipantDetails.Tables(0).NewRow
                    transactionRow = dsTransactions.Tables(0).NewRow

                    For participantRowCounter = 0 To dtPartAccount.Rows.Count - 1
                        beneficiaryItems = DataGridWorkSheets.Items(participantRowCounter)
                        Dim SelectCheckbox As CheckBox = CType(beneficiaryItems.Cells(0).Controls(1), CheckBox)
                        If SelectCheckbox.Checked = True Then
                            If dtPartAccount.Rows(participantRowCounter).Item("AcctType") = accountType Then

                                currentPlanType = GetPlanTypeForSelectedAccount(l_dataset_PartAccountDetail.Tables(0), accountType)

                                transactionRow("PersID") = Me.String_Benif_PersonD
                                transactionRow("FundEventID") = Me.string_RecptFundEventID
                                transactionRow("AcctType") = accountType
                                transactionRow("TransactType") = "QSPR"
                                If transactionRow("AcctType") = "RT" Then
                                    transactionRow("AnnuityBasisType") = annuityBasisType
                                Else
                                    transactionRow("AnnuityBasisType") = "PST96"
                                End If
                                transactionRow("TransactDate") = TextboxEndDate.Text
                                transactionRow("MonthlyComp") = 0.0
                                If Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPreTax") * percentage, 2) < (amount - splitBalance) Then
                                    transactionRow("PersonalPreTax") = Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPreTax") * percentage, 2)
                                Else
                                    transactionRow("PersonalPreTax") = amount - splitBalance
                                End If

                                splitBalance = splitBalance + transactionRow("PersonalPreTax")
                                If Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPostTax") * percentage, 2) <= (amount - splitBalance) Then
                                    transactionRow("PersonalPostTax") = Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPostTax") * percentage, 2)
                                Else
                                    transactionRow("PersonalPostTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow("PersonalPostTax")
                                If Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAPreTax") * percentage, 2) <= (amount - splitBalance) Then
                                    transactionRow("YMCAPreTax") = Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAPreTax") * percentage, 2)
                                Else
                                    transactionRow("YMCAPreTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow("YMCAPreTax")
                                transactionRow("ReceivedDate") = Date.Now
                                transactionRow("TransmittalID") = System.DBNull.Value
                                transactionRow("Created") = System.DBNull.Value
                                transactionRow("Creator") = "Test"
                                transactionRow("Updated") = System.DBNull.Value
                                transactionRow("Updater") = System.DBNull.Value
                                transactionRow("PlanType") = currentPlanType

                                If transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax") > 0.0 Then
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalPreTax") = row("PersonalPreTax") - transactionRow("PersonalPreTax")
                                    row("PersonalPostTax") = row("PersonalPostTax") - transactionRow("PersonalPostTax")
                                    row("PersonalTotal") = row("PersonalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax"))
                                    row("YMCAPreTax") = row("YMCAPreTax") - transactionRow("YMCAPreTax")
                                    row("YMCATotal") = row("YMCATotal") - transactionRow("YMCAPreTax")
                                    row("TotalTotal") = row("TotalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax"))
                                    If Not transactionRow Is Nothing Then
                                        For rowCounter = 0 To transactionRow.ItemArray.Length - 1
                                            recipientRow(rowCounter) = transactionRow(rowCounter)
                                        Next
                                    Else
                                        recipientRow("MonthlyComp") = 0.0
                                    End If
                                    recipientRow("AnnuityBasisType") = annuityBasisType
                                    recipientRow("AcctType") = accountType
                                    If recipientRow("AcctType") = "RT" Then
                                        'START: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                        'If l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString() <> "System.DBNull" Then
                                        '    recipientRow("TransactionRefID") = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString()
                                        'Else
                                        '    recipientRow("TransactionRefID") = System.DBNull.Value
                                        'End If
                                        recipientRow("TransactionRefID") = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                        'END: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                    Else
                                        recipientRow("TransactionRefID") = System.DBNull.Value
                                    End If
                                    recipientRow("PersID") = Me.String_Benif_PersonD
                                    recipientRow("FundEventId") = Me.string_RecptFundEventID
                                    recipientRow("TransactType") = "QSPR"
                                    recipientRow("PlanType") = currentPlanType
                                    dsGroupARecipantDetails.Tables(0).Rows.Add(recipientRow)
                                End If

                                If transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax") > 0.0 Then
                                    dsTransactions.Tables(0).Rows.Add(transactionRow)
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalPreTax") = row("PersonalPreTax") - transactionRow("PersonalPreTax")
                                    row("PersonalPostTax") = row("PersonalPostTax") - transactionRow("PersonalPostTax")
                                    row("PersonalTotal") = row("PersonalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax"))
                                    row("YMCAPreTax") = row("YMCAPreTax") - transactionRow("YMCAPreTax")
                                    row("YMCATotal") = row("YMCATotal") - transactionRow("YMCAPreTax")
                                    row("TotalTotal") = row("TotalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax"))
                                    If Not transactionRow Is Nothing Then
                                        For rowCounter = 0 To transactionRow.ItemArray.Length - 1
                                            transactionRow1(rowCounter) = transactionRow(rowCounter)
                                        Next
                                    Else
                                        transactionRow1("MonthlyComp") = 0.0
                                    End If
                                    transactionRow1("AnnuityBasisType") = annuityBasisType
                                    transactionRow1("AcctType") = accountType
                                    transactionRow1("PersID") = Session("PersId")
                                    transactionRow1("FundEventId") = Me.string_FundEventID
                                    transactionRow1("TransactType") = "QWPR"
                                    transactionRow1("PersonalPreTax") = transactionRow("PersonalPreTax") * (-1)
                                    transactionRow1("PersonalPostTax") = transactionRow("PersonalPostTax") * (-1)
                                    transactionRow1("YMCAPreTax") = transactionRow("YMCAPreTax") * (-1)
                                    transactionRow1("BenfitPersId") = Me.String_Benif_PersonD
                                    If transactionRow1("AcctType") = "RT" Then
                                        'START: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                        'If l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString() <> "System.DBNull" Then
                                        '    transactionRow1("TransactionRefID") = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString()
                                        'Else
                                        '    transactionRow1("TransactionRefID") = System.DBNull.Value
                                        'End If
                                        transactionRow1("TransactionRefID") = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                        'END: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                    Else
                                        transactionRow1("TransactionRefID") = System.DBNull.Value
                                    End If
                                    transactionRow1("PlanType") = currentPlanType
                                    dsGroupAParticipantDetails.Tables(0).Rows.Add(transactionRow1)
                                End If

                                transactionRow2 = dsTransactions.Tables(0).NewRow
                                If Not transactionRow1 Is Nothing Then
                                    For rowCounter = 0 To transactionRow1.ItemArray.Length - 2
                                        transactionRow2(rowCounter) = transactionRow1(rowCounter)
                                    Next
                                Else
                                    transactionRow2("MonthlyComp") = 0.0
                                End If
                                If transactionRow("AcctType") = "RT" Then
                                    transactionRow2("AnnuityBasisType") = annuityBasisType
                                Else
                                    transactionRow2("AnnuityBasisType") = "PST96"
                                End If
                                transactionRow2("AcctType") = accountType
                                transactionRow2("PersID") = Me.String_Benif_PersonD
                                transactionRow2("FundEventId") = Me.string_RecptFundEventID
                                transactionRow2("TransactType") = "QSIN"
                                If Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalInterestBalance") * percentage, 2) <= amount - splitBalance Then
                                    transactionRow2("PersonalPreTax") = Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalInterestBalance") * percentage, 2)
                                Else
                                    transactionRow2("PersonalPreTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow2("PersonalPreTax")
                                transactionRow2("PersonalPostTax") = 0.0
                                If Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAInterestBalance") * percentage, 2) <= amount - splitBalance Then
                                    transactionRow2("YMCAPreTax") = Math.Round(l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAInterestBalance") * percentage, 2)
                                Else
                                    transactionRow2("YMCAPreTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow2("YMCAPreTax")
                                If (transactionRow2("PersonalPreTax") + transactionRow2("YMCAPreTax")) > 0 Then
                                    transactionRow2("PlanType") = currentPlanType
                                    dsTransactions.Tables(0).Rows.Add(transactionRow2)
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalInterestBalance") = row("PersonalInterestBalance") - transactionRow2("PersonalPreTax")
                                    row("PersonalTotal") = row("PersonalTotal") - transactionRow2("PersonalPreTax")
                                    row("YMCAInterestBalance") = row("YMCAInterestBalance") - transactionRow2("YMCAPreTax")
                                    row("YMCATotal") = row("YMCATotal") - transactionRow2("YMCAPreTax")
                                    row("TotalTotal") = row("TotalTotal") - (transactionRow2("PersonalPreTax") + transactionRow2("PersonalPostTax") + transactionRow2("YMCAPreTax"))
                                End If
                                If Not transactionRow2 Is Nothing Then
                                    For rowCounter = 0 To transactionRow2.ItemArray.Length - 1
                                        transactionRow3(rowCounter) = transactionRow2(rowCounter)
                                    Next
                                Else
                                    transactionRow3("MonthlyComp") = 0.0
                                End If
                                transactionRow3("AnnuityBasisType") = annuityBasisType
                                transactionRow3("AcctType") = accountType
                                transactionRow3("PersId") = Session("PersId")
                                transactionRow3("FundEventID") = Me.string_FundEventID
                                transactionRow3("TransactType") = "QWIN"
                                transactionRow3("PersonalPreTax") = transactionRow2("PersonalPreTax") * (-1)
                                transactionRow3("YMCAPreTax") = transactionRow2("YMCAPreTax") * (-1)
                                transactionRow3("BenfitPersId") = Me.String_Benif_PersonD
                                If transactionRow3("AcctType") = "RT" Then
                                    'START: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                    'If l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString() <> "System.DBNull" Then
                                    '    'Neeraj 23-Dec-2010 : issue id BT-708 - datatype assingment was not appropirate
                                    '    transactionRow3("TransactionRefID") = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                    'Else
                                    '    transactionRow3("TransactionRefID") = System.DBNull.Value
                                    'End If
                                    'Neeraj 23-Dec-2010 : issue id BT-708 - datatype assingment was not appropirate
                                    transactionRow3("TransactionRefID") = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                    'END: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                Else
                                    transactionRow3("TransactionRefID") = System.DBNull.Value
                                End If
                                'If the PersonalPreTax,YMCAPreTax,PersonalPostTax value equal to zero then Record should not go to the database
                                If transactionRow3("PersonalPreTax") + transactionRow3("YMCAPreTax") + transactionRow3("PersonalPostTax") <> 0.0 Then
                                    transactionRow3("PlanType") = currentPlanType
                                    dsGroupAParticipantDetails.Tables(0).Rows.Add(transactionRow3)
                                End If
                                If (transactionRow2("PersonalPreTax") + transactionRow2("YMCAPreTax")) > 0 Then
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalInterestBalance") = row("PersonalInterestBalance")
                                    row("PersonalTotal") = row("PersonalTotal")
                                    row("YMCAInterestBalance") = row("YMCAInterestBalance")
                                    row("YMCATotal") = row("YMCATotal")
                                    row("TotalTotal") = row("TotalTotal")
                                End If
                                If Not transactionRow2 Is Nothing Then
                                    For rowCounter = 0 To transactionRow2.ItemArray.Length - 1
                                        transactionRow6(rowCounter) = transactionRow2(rowCounter)
                                    Next
                                Else
                                    transactionRow6("MonthlyComp") = 0.0
                                End If
                                transactionRow6("AnnuityBasisType") = annuityBasisType
                                transactionRow6("AcctType") = accountType
                                transactionRow6("PersID") = Me.String_Benif_PersonD
                                transactionRow6("FundEventId") = Me.string_RecptFundEventID
                                transactionRow6("TransactType") = "QSIN"
                                If transactionRow6("AcctType") = "RT" Then
                                    'START: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                    'If l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString() <> "System.DBNull" Then
                                    '    'Neeraj 23-Dec-2010 : issue id BT-708 - datatype assingment was not appropirate
                                    '    transactionRow6("TransactionRefID") = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                    'Else
                                    '    transactionRow6("TransactionRefID") = System.DBNull.Value
                                    'End If
                                    'Neeraj 23-Dec-2010 : issue id BT-708 - datatype assingment was not appropirate
                                    transactionRow6("TransactionRefID") = l_dataset_GroupAAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                    'END: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                Else
                                    transactionRow6("TransactionRefID") = System.DBNull.Value
                                End If
                                'If the PersonalPreTax,YMCAPreTax,PersonalPostTax value equal to zero then Record should not go to the database
                                If transactionRow6("PersonalPreTax") + transactionRow6("YMCAPreTax") + transactionRow6("PersonalPostTax") <> 0.0 Then
                                    transactionRow6("PlanType") = currentPlanType
                                    dsGroupARecipantDetails.Tables(0).Rows.Add(transactionRow6)
                                    'Session("dsGroupARecipantDetails") = dsGroupARecipantDetails
                                End If
                            End If
                        End If
                    Next
                Next
            End If

            YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.RoundOffAmount(dsGroupAParticipantDetails) ' Required to resolve Decimal datatype issue
            If Not Session_dataset_dsALLGroupAParticipantDetails Is Nothing Then
                dsALLGroupAParticipantDetails = Me.Session_dataset_dsALLGroupAParticipantDetails
                'START: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsALLGroupAParticipantDetails rows count checking dsGroupAParticipantDetails rows count
                'If dsALLGroupAParticipantDetails.Tables(0).Rows.Count > 0 Then
                If HelperFunctions.isNonEmpty(dsGroupAParticipantDetails) AndAlso dsGroupAParticipantDetails.Tables(0).Rows.Count > 0 Then
                    'END: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsALLGroupAParticipantDetails rows count checking dsGroupAParticipantDetails rows count
                    For i As Integer = 0 To dsGroupAParticipantDetails.Tables(0).Rows.Count - 1
                        dsALLGroupAParticipantDetails.Tables(0).ImportRow(dsGroupAParticipantDetails.Tables(0).Rows(i))
                    Next
                End If
                Me.Session_dataset_dsALLGroupAParticipantDetails = dsALLGroupAParticipantDetails
            Else
                Me.Session_dataset_dsALLGroupAParticipantDetails = dsGroupAParticipantDetails.Copy
            End If

            YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.RoundOffAmount(dsGroupARecipantDetails) ' Required to resolve Decimal datatype issue
            If Not Session_dataset_dsALLGroupARecipantDetails Is Nothing Then
                dsALLGroupARecipantDetails = Me.Session_dataset_dsALLGroupARecipantDetails
                'START: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsALLGroupARecipantDetails rows count checking dsGroupARecipantDetails rows count
                'If dsALLGroupARecipantDetails.Tables(0).Rows.Count > 0 Then
                If HelperFunctions.isNonEmpty(dsGroupARecipantDetails) AndAlso dsGroupARecipantDetails.Tables(0).Rows.Count > 0 Then
                    'END: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsALLGroupARecipantDetails rows count checking dsGroupARecipantDetails rows count
                    For i As Integer = 0 To dsGroupARecipantDetails.Tables(0).Rows.Count - 1
                        dsALLGroupARecipantDetails.Tables(0).ImportRow(dsGroupARecipantDetails.Tables(0).Rows(i))
                    Next
                End If
                Me.Session_dataset_dsALLGroupARecipantDetails = dsALLGroupARecipantDetails
            Else
                Me.Session_dataset_dsALLGroupARecipantDetails = dsGroupARecipantDetails.Copy
            End If
        Catch
            Throw
        End Try
    End Sub

    '*************************************************************************************************** //
    'Class Name                :NonRetiredQdro               Used In     : YMCAUI                        //
    'Created By                :Ganeswar Sahoo            Modified On :                                  //
    'Modified By               :                                                                         //
    'Modify Reason             :                                                                         //
    'Constructor Description   :                                                                         //
    'Class Description         :Used to Make the necessary calculations for PRE96 and PST96 Split for GroupB .                     //
    '***************************************************************************************************//
    Private Sub PerformGroupBSplitOperation(ByVal percentage As Decimal)
        'PPP | 09/12/2016 | YRS-AT-2529 | Restructured PerformGroupBSplitOperation. Old code can be checked from TF.
        Dim splitBalance As Decimal = 0
        Dim participantAccountRowIndex As Integer
        Dim rows() As DataRow
        Dim row As DataRow
        Dim transactionRow As DataRow
        Dim accountingDate As DateTime
        Dim transactDate As DateTime
        Dim fundedDate As String
        Dim transactionRow1 As DataRow
        Dim transactionRow2 As DataRow
        Dim transactionRow3 As DataRow
        Dim recipientRow As DataRow
        Dim transactionRow6 As DataRow
        Dim recipientPersID As String
        Dim amount As Decimal = 0
        Dim finalTotal As Decimal = 0
        Dim participantRowCounter As Integer
        Dim beneficiaryItems As DataGridItem
        Dim accountType As String
        Dim annuityBasisType As String
        Dim deleteRows As DataRow()
        Dim areRowsDeleted As Boolean
        Dim currentPlanType As String
        Try
            DataGridWorkSheet2.DataSource = Nothing
            DataGridWorkSheet2.DataBind()

            If Me.Session_dataset_GroupBAnnuityBasisDetail Is Nothing Then
                l_dataset_GroupBAnnuityBasisDetail = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getGroupBPartAccountDetailbyPlan(Me.string_FundEventID, TextboxBegDate.Text, TextboxEndDate.Text, "Both")
                Me.Session_dataset_GroupBAnnuityBasisDetail = l_dataset_GroupBAnnuityBasisDetail.Copy
            Else
                l_dataset_GroupBAnnuityBasisDetail = Me.Session_dataset_GroupBAnnuityBasisDetail.Copy
            End If

            dtPartAccount = Me.Session_datatable_dtPartAccount.Copy 'PPP | 09/30/2016 | YRS-AT-2529 | Added ".Copy" 
            areRowsDeleted = False
            For participantAccountRowIndex = 0 To l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows.Count - 1
                If Not l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).RowState = DataRowState.Deleted Then
                    accountType = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("AcctType")
                    If dtPartAccount.Select(String.Format("AcctType='{0}'", accountType)).Length = 0 Then
                        deleteRows = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Select(String.Format("AcctType='{0}'", accountType))
                        For Each rowToDelete As DataRow In deleteRows
                            rowToDelete.Delete()
                            areRowsDeleted = True
                        Next
                    End If
                End If
            Next
            If areRowsDeleted Then
                l_dataset_GroupBAnnuityBasisDetail.AcceptChanges()
            End If

            For participantRowCounter = 0 To dtPartAccount.Rows.Count - 1
                accountType = dtPartAccount.Rows(participantRowCounter)("AcctType")
                For intcnt As Integer = 0 To l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows.Count - 1
                    If accountType = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(intcnt)("AcctType") Then
                        dblGroupBTotal = dblGroupBTotal + l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(intcnt)("mnyBalance")
                    End If
                Next
            Next

            finalTotal = dblGroupBTotal
            Me.Session_finTot = finalTotal
            If finalTotal < 0 Then
                Exit Sub
            End If
            amount = finalTotal * percentage
            splitBalance = 0
            dsTransactions = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getTransactionsQDRO(Me.string_FundEventID) ', TextboxEndDate.Text)
            Me.String_Benif_PersonD = cboBeneficiarySSNo.SelectedValue
            dsTransactions.Tables(0).Columns.Add("PlanType", GetType(System.String)) 'PPP | 08/29/2016 | YRS-AT-2529 | Need to maintain $ and % planwise
            dsGroupBParticipantDetails = dsTransactions.Copy
            dsGroupBParticipantDetails.Tables(0).Rows.Clear()
            dsGroupBRecipantDetails = dsTransactions.Copy
            dsGroupBRecipantDetails.Tables(0).Rows.Clear()
            dsGroupBParticipantDetails.Tables(0).Columns.Add("BenfitPersId", GetType(System.String))
            If splitBalance < amount Then
                l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail 'PPP | 08/30/2016 | YRS-AT-2529 | Required to find current account Plan Type
                For participantAccountRowIndex = 0 To l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows.Count - 1
                    accountType = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("AcctType")
                    annuityBasisType = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("AnnuityBasisType")
                    If dtPartAccount.Select(String.Format("AcctType='{0}'", accountType)).Length = 0 Then
                        Exit Sub
                    End If
                    transactionRow1 = dsGroupBParticipantDetails.Tables(0).NewRow
                    transactionRow3 = dsGroupBParticipantDetails.Tables(0).NewRow
                    transactionRow6 = dsGroupBRecipantDetails.Tables(0).NewRow
                    recipientRow = dsGroupBRecipantDetails.Tables(0).NewRow
                    transactionRow = dsTransactions.Tables(0).NewRow
                    For participantRowCounter = 0 To dtPartAccount.Rows.Count - 1
                        beneficiaryItems = DataGridWorkSheets.Items(participantRowCounter)
                        Dim SelectCheckbox As CheckBox = CType(beneficiaryItems.Cells(0).Controls(1), CheckBox)
                        If SelectCheckbox.Checked = True Then
                            If dtPartAccount.Rows(participantRowCounter).Item("AcctType") = accountType Then

                                currentPlanType = GetPlanTypeForSelectedAccount(l_dataset_PartAccountDetail.Tables(0), accountType)

                                transactionRow("PersID") = Me.String_Benif_PersonD
                                transactionRow("FundEventID") = Me.string_RecptFundEventID
                                transactionRow("AcctType") = accountType 'l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(iRowPartAccDtl).Item("AcctType")
                                transactionRow("TransactType") = "QSPR"
                                If transactionRow("AcctType") = "RT" Then
                                    transactionRow("AnnuityBasisType") = annuityBasisType 'l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(iRowPartAccDtl).Item("AnnuityBasisType")
                                Else
                                    transactionRow("AnnuityBasisType") = "PST96"
                                End If
                                transactionRow("TransactDate") = TextboxEndDate.Text
                                transactionRow("MonthlyComp") = 0.0
                                If Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPreTax") * percentage, 2) < (amount - splitBalance) Then
                                    transactionRow("PersonalPreTax") = Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPreTax") * percentage, 2)
                                Else
                                    transactionRow("PersonalPreTax") = amount - splitBalance
                                End If

                                splitBalance = splitBalance + transactionRow("PersonalPreTax")
                                If Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPostTax") * percentage, 2) <= (amount - splitBalance) Then
                                    transactionRow("PersonalPostTax") = Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalPostTax") * percentage, 2)
                                Else
                                    transactionRow("PersonalPostTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow("PersonalPostTax")
                                If Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAPreTax") * percentage, 2) <= (amount - splitBalance) Then
                                    transactionRow("YMCAPreTax") = Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAPreTax") * percentage, 2)
                                Else
                                    transactionRow("YMCAPreTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow("YMCAPreTax")
                                transactionRow("ReceivedDate") = Date.Now
                                transactionRow("TransmittalID") = System.DBNull.Value
                                transactionRow("Created") = System.DBNull.Value
                                transactionRow("Creator") = "Test"
                                transactionRow("Updated") = System.DBNull.Value
                                transactionRow("Updater") = System.DBNull.Value
                                transactionRow("PlanType") = currentPlanType

                                If transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax") > 0.0 Then
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalPreTax") = row("PersonalPreTax") - transactionRow("PersonalPreTax")
                                    row("PersonalPostTax") = row("PersonalPostTax") - transactionRow("PersonalPostTax")
                                    row("PersonalTotal") = row("PersonalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax"))
                                    row("YMCAPreTax") = row("YMCAPreTax") - transactionRow("YMCAPreTax")
                                    row("YMCATotal") = row("YMCATotal") - transactionRow("YMCAPreTax")
                                    row("TotalTotal") = row("TotalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax"))
                                    If Not transactionRow Is Nothing Then
                                        For tempCnt = 0 To transactionRow.ItemArray.Length - 1
                                            recipientRow(tempCnt) = transactionRow(tempCnt)
                                        Next
                                    Else
                                        recipientRow("MonthlyComp") = 0.0
                                    End If
                                    recipientRow("AnnuityBasisType") = annuityBasisType
                                    recipientRow("AcctType") = accountType
                                    recipientRow("PersID") = Me.String_Benif_PersonD
                                    recipientRow("FundEventId") = Me.string_RecptFundEventID
                                    recipientRow("TransactType") = "QSPR"
                                    If recipientRow("AcctType") = "RT" Then
                                        'START: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                        'If l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString() <> "System.DBNull" Then
                                        '    recipientRow("TransactionRefID") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString()
                                        'Else
                                        '    recipientRow("TransactionRefID") = System.DBNull.Value
                                        'End If
                                        recipientRow("TransactionRefID") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                        'END: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                    Else
                                        recipientRow("TransactionRefID") = System.DBNull.Value
                                    End If
                                    recipientRow("FundedDate") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("FundedDate")
                                    recipientRow("PlanType") = currentPlanType
                                    dsGroupBRecipantDetails.Tables(0).Rows.Add(recipientRow)
                                End If

                                If transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax") > 0.0 Then
                                    dsTransactions.Tables(0).Rows.Add(transactionRow)
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalPreTax") = row("PersonalPreTax") - transactionRow("PersonalPreTax")
                                    row("PersonalPostTax") = row("PersonalPostTax") - transactionRow("PersonalPostTax")
                                    row("PersonalTotal") = row("PersonalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax"))
                                    row("YMCAPreTax") = row("YMCAPreTax") - transactionRow("YMCAPreTax")
                                    row("YMCATotal") = row("YMCATotal") - transactionRow("YMCAPreTax")
                                    row("TotalTotal") = row("TotalTotal") - (transactionRow("PersonalPreTax") + transactionRow("PersonalPostTax") + transactionRow("YMCAPreTax"))
                                    If Not transactionRow Is Nothing Then
                                        For tempCnt = 0 To transactionRow.ItemArray.Length - 1
                                            transactionRow1(tempCnt) = transactionRow(tempCnt)
                                        Next
                                    Else
                                        transactionRow1("MonthlyComp") = 0.0
                                    End If
                                    transactionRow1("AnnuityBasisType") = annuityBasisType
                                    transactionRow1("AcctType") = accountType
                                    transactionRow1("PersID") = Session("PersId")
                                    transactionRow1("FundEventId") = Me.string_FundEventID
                                    transactionRow1("TransactType") = "QWPR"
                                    transactionRow1("PersonalPreTax") = transactionRow("PersonalPreTax") * (-1)
                                    transactionRow1("PersonalPostTax") = transactionRow("PersonalPostTax") * (-1)
                                    transactionRow1("YMCAPreTax") = transactionRow("YMCAPreTax") * (-1)
                                    transactionRow1("BenfitPersId") = Me.String_Benif_PersonD
                                    transactionRow1("FundedDate") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("FundedDate")
                                    If transactionRow1("AcctType") = "RT" Then
                                        'START: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                        'If l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString() <> "System.DBNull" Then
                                        '    transactionRow1("TransactionRefID") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString()
                                        'Else
                                        '    transactionRow1("TransactionRefID") = System.DBNull.Value
                                        'End If
                                        transactionRow1("TransactionRefID") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                        'END: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                    Else
                                        transactionRow1("TransactionRefID") = System.DBNull.Value
                                    End If
                                    transactionRow1("PlanType") = currentPlanType
                                    dsGroupBParticipantDetails.Tables(0).Rows.Add(transactionRow1)
                                End If

                                transactionRow2 = dsTransactions.Tables(0).NewRow
                                If Not transactionRow1 Is Nothing Then
                                    For tempCnt = 0 To transactionRow1.ItemArray.Length - 2
                                        transactionRow2(tempCnt) = transactionRow1(tempCnt)
                                    Next
                                Else
                                    transactionRow2("MonthlyComp") = 0.0
                                End If
                                If transactionRow("AcctType") = "RT" Then
                                    transactionRow2("AnnuityBasisType") = annuityBasisType
                                Else
                                    transactionRow2("AnnuityBasisType") = "PST96"
                                End If
                                transactionRow2("AcctType") = accountType
                                transactionRow2("PersID") = Me.String_Benif_PersonD
                                transactionRow2("FundEventId") = Me.string_RecptFundEventID
                                transactionRow2("TransactType") = "QSIN"
                                If Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalInterestBalance") * percentage, 2) <= amount - splitBalance Then
                                    transactionRow2("PersonalPreTax") = Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("PersonalInterestBalance") * percentage, 2)
                                Else
                                    transactionRow2("PersonalPreTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow2("PersonalPreTax")
                                transactionRow2("PersonalPostTax") = 0.0
                                If Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAInterestBalance") * percentage, 2) <= amount - splitBalance Then
                                    transactionRow2("YMCAPreTax") = Math.Round(l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("YMCAInterestBalance") * percentage, 2)
                                Else
                                    transactionRow2("YMCAPreTax") = amount - splitBalance
                                End If
                                splitBalance = splitBalance + transactionRow2("YMCAPreTax")
                                If (transactionRow2("PersonalPreTax") + transactionRow2("YMCAPreTax")) > 0 Then
                                    transactionRow2("PlanType") = currentPlanType
                                    dsTransactions.Tables(0).Rows.Add(transactionRow2)
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalInterestBalance") = row("PersonalInterestBalance") - transactionRow2("PersonalPreTax")
                                    row("PersonalTotal") = row("PersonalTotal") - transactionRow2("PersonalPreTax")
                                    row("YMCAInterestBalance") = row("YMCAInterestBalance") - transactionRow2("YMCAPreTax")
                                    row("YMCATotal") = row("YMCATotal") - transactionRow2("YMCAPreTax")
                                    row("TotalTotal") = row("TotalTotal") - (transactionRow2("PersonalPreTax") + transactionRow2("PersonalPostTax") + transactionRow2("YMCAPreTax"))
                                End If
                                If Not transactionRow2 Is Nothing Then
                                    For tempCnt = 0 To transactionRow2.ItemArray.Length - 1
                                        transactionRow3(tempCnt) = transactionRow2(tempCnt)
                                    Next
                                Else
                                    transactionRow3("MonthlyComp") = 0.0
                                End If
                                transactionRow3("AnnuityBasisType") = annuityBasisType
                                transactionRow3("AcctType") = accountType
                                transactionRow3("PersId") = Session("PersId")
                                transactionRow3("FundEventID") = Me.string_FundEventID
                                transactionRow3("TransactType") = "QWIN"
                                transactionRow3("PersonalPreTax") = transactionRow2("PersonalPreTax") * (-1)
                                transactionRow3("YMCAPreTax") = transactionRow2("YMCAPreTax") * (-1)
                                transactionRow3("BenfitPersId") = Me.String_Benif_PersonD
                                transactionRow3("FundedDate") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("FundedDate")
                                If transactionRow3("AcctType") = "RT" Then
                                    'START: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                    'If l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString() <> "System.DBNull" Then
                                    '    transactionRow3("TransactionRefID") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString()
                                    'Else
                                    '    transactionRow3("TransactionRefID") = System.DBNull.Value
                                    'End If
                                    transactionRow3("TransactionRefID") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                    'END: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                Else
                                    transactionRow3("TransactionRefID") = System.DBNull.Value
                                End If
                                If transactionRow3("PersonalPreTax") + transactionRow3("YMCAPreTax") + transactionRow3("PersonalPostTax") <> 0.0 Then
                                    transactionRow3("PlanType") = currentPlanType
                                    dsGroupBParticipantDetails.Tables(0).Rows.Add(transactionRow3)
                                End If

                                If (transactionRow2("PersonalPreTax") + transactionRow2("YMCAPreTax")) > 0 Then
                                    row = dtPartAccount.Rows(participantRowCounter)
                                    row("PersonalInterestBalance") = row("PersonalInterestBalance")
                                    row("PersonalTotal") = row("PersonalTotal")
                                    row("YMCAInterestBalance") = row("YMCAInterestBalance")
                                    row("YMCATotal") = row("YMCATotal")
                                    row("TotalTotal") = row("TotalTotal")
                                End If
                                If Not transactionRow2 Is Nothing Then
                                    For tempCnt = 0 To transactionRow2.ItemArray.Length - 1
                                        transactionRow6(tempCnt) = transactionRow2(tempCnt)
                                    Next
                                Else
                                    transactionRow6("MonthlyComp") = 0.0
                                End If
                                transactionRow6("AnnuityBasisType") = annuityBasisType
                                transactionRow6("AcctType") = accountType
                                transactionRow6("PersID") = Me.String_Benif_PersonD
                                transactionRow6("FundEventId") = Me.string_RecptFundEventID
                                transactionRow6("TransactType") = "QSIN"
                                transactionRow6("FundedDate") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("FundedDate")
                                If transactionRow6("AcctType") = "RT" Then
                                    'START: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                    'If l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString() <> "System.DBNull" Then
                                    '    transactionRow6("TransactionRefID") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID").ToString()
                                    'Else
                                    '    transactionRow6("TransactionRefID") = System.DBNull.Value
                                    'End If
                                    transactionRow6("TransactionRefID") = l_dataset_GroupBAnnuityBasisDetail.Tables(0).Rows(participantAccountRowIndex).Item("TransactionRefID")
                                    'END: PPP | 09/16/2016 | YRS-AT-2529 | No need to check DBNull, directly assign value
                                Else
                                    transactionRow6("TransactionRefID") = System.DBNull.Value
                                End If
                                'If the PersonalPreTax,YMCAPreTax,PersonalPostTax value equal to zero then Record should not go to the database
                                If transactionRow6("PersonalPreTax") + transactionRow6("YMCAPreTax") + transactionRow6("PersonalPostTax") <> 0.0 Then
                                    transactionRow6("PlanType") = currentPlanType
                                    dsGroupBRecipantDetails.Tables(0).Rows.Add(transactionRow6)
                                End If
                            End If
                        End If
                    Next
                Next
            End If

            YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.RoundOffAmount(dsGroupBParticipantDetails) ' Required to resolve Decimal datatype issue
            If Not Session_dataset_dsALLGroupBParticipantDetails Is Nothing Then
                dsALLGroupBParticipantDetails = Me.Session_dataset_dsALLGroupBParticipantDetails
                'START: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsALLGroupBParticipantDetails rows count checking dsGroupBParticipantDetails rows count
                'If dsALLGroupBParticipantDetails.Tables(0).Rows.Count > 0 Then
                If HelperFunctions.isNonEmpty(dsGroupBParticipantDetails) AndAlso dsGroupBParticipantDetails.Tables(0).Rows.Count > 0 Then
                    'END: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsALLGroupBParticipantDetails rows count checking dsGroupBParticipantDetails rows count
                    For intcnt As Integer = 0 To dsGroupBParticipantDetails.Tables(0).Rows.Count - 1
                        dsALLGroupBParticipantDetails.Tables(0).ImportRow(dsGroupBParticipantDetails.Tables(0).Rows(intcnt))
                    Next
                End If
                Me.Session_dataset_dsALLGroupBParticipantDetails = dsALLGroupBParticipantDetails
            Else
                Me.Session_dataset_dsALLGroupBParticipantDetails = dsGroupBParticipantDetails.Copy
            End If

            YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.RoundOffAmount(dsGroupBRecipantDetails) ' Required to resolve Decimal datatype issue
            If Not Session_dataset_dsALLGroupBRecipantDetails Is Nothing Then
                dsALLGroupBRecipantDetails = Me.Session_dataset_dsALLGroupBRecipantDetails
                'START: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsALLGroupBRecipantDetails rows count checking dsGroupBRecipantDetails rows count
                'If dsALLGroupBRecipantDetails.Tables(0).Rows.Count > 0 Then
                If HelperFunctions.isNonEmpty(dsGroupBRecipantDetails) AndAlso dsGroupBRecipantDetails.Tables(0).Rows.Count > 0 Then
                    'END: PPP | 09/30/2016 | YRS-AT-2529 | Instead of checking dsALLGroupBRecipantDetails rows count checking dsGroupBRecipantDetails rows count
                    For intCount As Integer = 0 To dsGroupBRecipantDetails.Tables(0).Rows.Count - 1
                        dsALLGroupBRecipantDetails.Tables(0).ImportRow(dsGroupBRecipantDetails.Tables(0).Rows(intCount))
                    Next
                End If
                Me.Session_dataset_dsALLGroupBRecipantDetails = dsALLGroupBRecipantDetails
            Else
                Me.Session_dataset_dsALLGroupBRecipantDetails = dsGroupBRecipantDetails.Copy
            End If
        Catch
            Throw
        End Try
    End Sub
#End Region

    'START : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required
    'Private Sub DataGridList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridList.ItemDataBound
    '    e.Item.Cells(8).Visible = False  'Shashi Shekhar:2010-02-17 :Hide IsArchived Field in grid
    '    e.Item.Cells(9).Visible = False  'Shashi Shekhar:2010-12-10 :Hide fund no Field in grid
    '    e.Item.Cells(10).Visible = False  'Shashi Shekhar:2011-02-10 :Hide IsLock Field in grid
    'End Sub
    'END : MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required

    Public Sub BindStatusGrid()
        If HelperFunctions.isNonEmpty(tblStatus) Then
            Session("QDROStatusTable") = tblStatus
            gvQDROStatus.DataSource = tblStatus
            gvQDROStatus.DataBind()
            tblButtons.Visible = False
        Else
            lblQDROStatus.Visible = True
            gvQDROStatus.Visible = False
            lblNotes1.Visible = False
            lblNotes2.Visible = False
            tblButtons.Visible = False
            lblMessage.Visible = False
            lblWarning.Visible = False
        End If
        'Start: Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
        'lblNotes2.Text = Resources.NonRetiredQDRO.MESSAGE_QDRO_STATUS_NOTES
        lblNotes2.Text = GetMessageFromResource("MESSAGE_QDRO_STATUS_NOTES")
        'lblQDROStatus.Text = Resources.NonRetiredQDRO.MESSAGE_QDRO_SAVE_COMPLETED
        lblQDROStatus.Text = GetMessageFromResource("MESSAGE_QDRO_SAVE_COMPLETED")
        'lblWarning.Text = Resources.NonRetiredQDRO.MESSAGE_QDRO_STATUS_WARNING
        lblWarning.Text = GetMessageFromResource("MESSAGE_QDRO_STATUS_WARNING")
        'Anudeep:18.12.2012 Bt-1523 - YRS 5.0-1753 Wording change in QDRO settlement page
        'lblMessage.Text = Resources.NonRetiredQDRO.MESSAGE_QDRO_STATUS_MESSAGE
        lblMessage.Text = GetMessageFromResource("MESSAGE_QDRO_STATUS_MESSAGE")
        'End: Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    End Sub
    ''2012.09.21: SR - YRS 5.0-YRS 5.0-1346 : Added
    Private Sub gvQDROStatus_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvQDROStatus.RowCommand
        Try
            Dim i As Integer
            Dim tblStatusCopy As New DataTable
            tblStatusCopy = CType(Session("QDROStatusTable"), DataTable)
            Dim strSSNo As String
            Dim strRefundRequest As String
            Dim StatusRow() As Data.DataRow
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvQDROStatus.Rows(index)
            strSSNo = row.Cells(0).Text

            strRefundRequest = row.Cells(4).Text
            If e.CommandName = "PrintForm" Then
                If strRefundRequest = "Success" Then
                    StatusRow = tblStatusCopy.Select("SSNo='" & strSSNo & "'")
                    If StatusRow(0)("Action") = "Not-Viewed" Then
                        'Added By SG: 2012.12.03: BT-1436
                        '    StatusRow(0)("Action") = "Form Viewed"
                        'ElseIf StatusRow(0)("Action") = "Letter Viewed" Then
                        'StatusRow(0)("Action") = "Form and Letter Viewed"
                        StatusRow(0)("Action") = "Viewed"
                    End If

                    tblStatusCopy.AcceptChanges()
                    gvQDROStatus.DataSource = tblStatusCopy
                    gvQDROStatus.DataBind()
                    '''SR:2012.10.16 - BT:1060/YRS 5.0-YRS 5.0-1346: Handle scenario when service not able to generate reports 
                    If StatusRow(0)("RefRequestId").ToString <> "00000000-0000-0000-0000-00000000000" Then
                        'Added By SG: 2012.12.03: BT-1436
                        'ProcessFormReport(StatusRow(0)("RefRequestId"), StatusRow(0)("ReportName"))
                        ProcessFormReport(StatusRow(0)("RefRequestId"), "Withdrawals_New.rpt")
                    Else
                        'Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                        'MessageBox.Show(PlaceHolder1, "QRDO", Resources.NonRetiredQDRO.MESSAGE_QDRO_STATUS_REPORT_FAILED, MessageBoxButtons.OK)
                        'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                        'MessageBox.Show(PlaceHolder1, "QRDO", GetMessageFromResource("MESSAGE_QDRO_STATUS_REPORT_FAILED"), MessageBoxButtons.OK)
                        ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_STATUS_REPORT_FAILED", "error")
                        'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    End If
                Else
                    'Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    'MessageBox.Show(PlaceHolder1, "QDRO", Resources.NonRetiredQDRO.MESSAGE_QDRO_STATUS_FAILED, MessageBoxButtons.OK, False)
                    'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_STATUS_FAILED"), MessageBoxButtons.OK, False)
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_STATUS_FAILED", "error")
                    'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                End If
                'Commented By SG: 2012.12.03: BT-1436
                'ElseIf e.CommandName = "PrintLetter" Then
                '    If strRefundRequest = "Success" Then
                '        StatusRow = tblStatusCopy.Select("SSNo='" & strSSNo & "'")
                '        If StatusRow(0)("Action") = "Not-Viewed" Then
                '            StatusRow(0)("Action") = "Letter Viewed"
                '        ElseIf StatusRow(0)("Action") = "Form Viewed" Then
                '            StatusRow(0)("Action") = "Form and Letter Viewed"
                '        End If

                '        tblStatusCopy.AcceptChanges()
                '        gvQDROStatus.DataSource = tblStatusCopy
                '        gvQDROStatus.DataBind()
                '        '''SR:21.09.2012 - BT:1060/YRS 5.0-YRS 5.0-1346: Handle scenario when service not able to generate reports 
                '        If StatusRow(0)("RefRequestId").ToString <> "00000000-0000-0000-0000-00000000000" Then
                '            ProcessLetterReport(StatusRow(0)("RefRequestId"))
                '        Else
                '            MessageBox.Show(PlaceHolder1, "QRDO", Resources.NonRetiredQDRO.MESSAGE_QDRO_STATUS_REPORT_FAILED, MessageBoxButtons.OK)
                '        End If
                '    Else
                '        MessageBox.Show(PlaceHolder1, "QDRO", Resources.NonRetiredQDRO.MESSAGE_QDRO_STATUS_FAILED, MessageBoxButtons.OK, False)
                '    End If
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    ''Ends, 2012.09.21: SR - YRS 5.0-YRS 5.0-1346 : Added
    Private Sub ProcessFormReport(ByVal RefRequestId As String, ByVal ReportName As String)
        Try
            'Set the Report Variables in Session
            Session("strReportName") = ReportName.Replace(".rpt", "")
            Session("RefRequestsID") = RefRequestId
            Session("strModuleName") = "QDRONonRet"
            'Call ReportViewer.aspx 
            Me.OpenReportViewer()

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    Private Sub ProcessLetterReport(ByVal RefRequestId As String)
        Try
            'Set the Report Variables in Session
            Session("strReportName") = System.Configuration.ConfigurationSettings.AppSettings("QDROLetterReportName")
            Session("RefRequestsID") = RefRequestId
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
            Dim popupScript As String = "<script language='javascript'>" & _
            "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If
        Catch
            Throw
        End Try
    End Sub
    ''2012.09.21: SR - YRS 5.0-YRS 5.0-1346 : Added
    Private Sub btnStatusOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStatusOK.Click
        Dim blnViewStatus As Boolean
        'Dim myGridRows As GridViewRow
        Try 'Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Added to begin with try block
            If gvQDROStatus.Rows.Count > 0 Then
                blnViewStatus = True
                For Each myGridRows As GridViewRow In gvQDROStatus.Rows
                    'Added By SG: 2012.12.03: BT-1436
                    'Modified By Anudeep:2013.01.10 BT-1550:Additional change YRS 5.0-1346  Observation on Non Retired QDRO status tab On "OK" Button Click
                    'If myGridRows.Cells(7).Text <> "Form and Letter Viewed" And myGridRows.Cells(4).Text = "Success" Then
                    If myGridRows.Cells(6).Text.Trim() <> "Viewed" And myGridRows.Cells(4).Text = "Success" Then
                        blnViewStatus = False
                    End If
                Next
                If blnViewStatus = False Then
                    'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup, also replaced session with viewstate variable 
                    'Me.Session_String_ISCompleted = False
                    'ViewState("Status") = "Status"
                    ''Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
                    ''MessageBox.Show(PlaceHolder1, "QRDO", Resources.NonRetiredQDRO.MESSAGE_QDRO_STATUS_NOTVIEWED, MessageBoxButtons.YesNo)
                    'MessageBox.Show(PlaceHolder1, "QRDO", GetMessageFromResource("MESSAGE_QDRO_STATUS_NOTVIEWED"), MessageBoxButtons.YesNo)
                    IsWithdrawalRequestStatusNotViewed = True
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_STATUS_NOTVIEWED", "infoYesNo")
                    'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup, also replaced session with viewstate variable 
                Else
                    Response.Redirect("MainWebForm.aspx", False)
                    Session("QDROStatusTable") = Nothing
                    Session("strReportName") = Nothing
                    Session("RefRequestsID") = Nothing
                    tblStatus = Nothing
                    'ViewState("Status") = Nothing 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
                    ClearDataTable()
                    ClearSessionData()
                    ClearSetSessionFromFindInfo() 'MMR | 2016.11.23 | YRS-AT-3145 | Clearing session values
                End If
            Else
                Response.Redirect("MainWebForm.aspx", False)
                Session("QDROStatusTable") = Nothing
                Session("strReportName") = Nothing
                Session("RefRequestsID") = Nothing
                tblStatus = Nothing
                'ViewState("Status") = Nothing 'PPP | 01/02/2017 | YRS-AT-3145 & 3265 | Not in use
                ClearDataTable()
                ClearSessionData()
                ClearSetSessionFromFindInfo() 'MMR | 2016.11.23 | YRS-AT-3145 | Clearing session values
            End If
            'Start - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("btnStatusOK_Click", ex)
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
        'End - Manthan Rajguru | 2016.08.25 | YRS-AT-2482 | Logging exception message
    End Sub
    ''End, 2012.09.21: SR - YRS 5.0-YRS 5.0-1346 : Added
    Public Sub CreateStatusTable()
        tblStatus.Columns.Add("SSNo", System.Type.GetType("System.String"))
        tblStatus.Columns.Add("FirstName", System.Type.GetType("System.String"))
        tblStatus.Columns.Add("MiddleName", System.Type.GetType("System.String"))
        tblStatus.Columns.Add("LastName", System.Type.GetType("System.String"))
        tblStatus.Columns.Add("RefundRequest", System.Type.GetType("System.String"))
        tblStatus.Columns.Add("Action", System.Type.GetType("System.String"))
        tblStatus.Columns.Add("RefRequestId", System.Type.GetType("System.String"))
        tblStatus.Columns.Add("ReportName", System.Type.GetType("System.String"))

        Session("QDROStatusTable") = tblStatus
    End Sub

    Public Sub LoadStatusTable(ByVal SSNo As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal LastName As String, ByVal RefundRequest As String, ByVal Action As String)
        Dim dsRefRequest As New DataSet
        tblStatus = CType(Session("QDROStatusTable"), DataTable)
        Dim drStatus As DataRow
        Dim i As Integer
        Dim reportName As String 'PPP | 09/30/2016 | YRS-AT-2529
        drStatus = tblStatus.NewRow()
        drStatus("SSNo") = SSNo
        drStatus("FirstName") = FirstName
        drStatus("MiddleName") = MiddleName
        drStatus("LastName") = LastName
        drStatus("RefundRequest") = RefundRequest
        drStatus("Action") = Action
        If RefundRequest = "Success" Then
            dsRefRequest = CType(Session("ServiceRequestReport"), DataSet)
            If HelperFunctions.isNonEmpty(dsRefRequest) Then  '''SR:2012.10.16 - BT:1060/YRS 5.0-YRS 5.0-1346: Handle scenario when service not able to generate reports 
                If HelperFunctions.isNonEmpty(dsRefRequest.Tables("ListOfParameter")) Then
                    drStatus("RefRequestId") = dsRefRequest.Tables("ListOfParameter").Rows(0)(2).ToString()
                End If
                If HelperFunctions.isNonEmpty(dsRefRequest.Tables("Report")) Then
                    drStatus("ReportName") = ""
                    For i = 0 To dsRefRequest.Tables("Report").Rows.Count - 1
                        'START: PPP | 09/30/2016 | YRS-AT-2529 | Saved report name in a variable reportName and checked against it in "If" condition, as well as added one more condition for ReleaseBlankOver5k.rpt 
                        'If dsRefRequest.Tables("Report").Rows(0)(1).ToString() = "ReleaseBlankLess1K.rpt" Or dsRefRequest.Tables("Report").Rows(0)(1).ToString() = "ReleaseBlank1kto5k.rpt" Then
                        '   drStatus("ReportName") = dsRefRequest.Tables("Report").Rows(0)(1).ToString()
                        '   Exit For
                        ' End If
                        reportName = Convert.ToString(dsRefRequest.Tables("Report").Rows(0)(1))
                        If reportName.ToUpper = ("ReleaseBlankLess1K.rpt").ToUpper Or reportName.ToUpper = ("ReleaseBlank1kto5k.rpt").ToUpper Or reportName.ToUpper = ("ReleaseBlankOver5k.rpt").ToUpper Then
                            drStatus("ReportName") = reportName
                            Exit For
                        End If
                        'END: PPP | 09/30/2016 | YRS-AT-2529 | Saved report name in a variable reportName and checked against it in "If" condition, as well as added one more condition for ReleaseBlankOver5k.rpt 
                    Next
                End If
            Else
                drStatus("RefRequestId") = "00000000-0000-0000-0000-00000000000"
                drStatus("ReportName") = ""
            End If
        Else
            drStatus("RefRequestId") = "00000000-0000-0000-0000-00000000000"
            drStatus("ReportName") = ""
        End If
        tblStatus.Rows.Add(drStatus)
        tblStatus.AcceptChanges()
    End Sub
    'Added function by Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
    Public Function GetMessageFromResource(ByVal strMessagekey As String) As String
        Dim strMessagevalue As String
        Try
            Try
                strMessagevalue = GetGlobalResourceObject("NonRetiredQDRO", strMessagekey).ToString()
            Catch
                strMessagevalue = "There is no message withrespect to given key"
            End Try
            Return strMessagevalue
        Catch
            Throw
        End Try
    End Function
    'Private Sub gvQDROStatus_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvQDROStatus.RowCreated
    '    'e.Row.Cells(8).Visible = False
    '    'e.Row.Cells(9).Visible = False        
    'End Sub
    'Private Sub gvQDROStatus_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvQDROStatus.RowDataBound
    '    'Dim cnt As Integer
    '    'Dim l_button_imgForm As ImageButton
    '    'Dim l_button_imgLetter As ImageButton

    '    'For cnt = 0 To gvQDROStatus.Rows.Count - 1
    '    '    If gvQDROStatus.Rows(cnt).Cells(4).Text = "Failure" Then
    '    '        l_button_imgForm = e.Row.FindControl("ImgForm")
    '    '        l_button_imgForm.Visible = False
    '    '    End If
    '    'Next

    '    'For cnt = 0 To gvQDROStatus.Rows.Count - 1
    '    '    If gvQDROStatus.Rows(cnt).Cells(4).Text = "Failure" Then
    '    '        l_button_imgLetter = e.Row.FindControl("ImgLetter")
    '    '        l_button_imgLetter.Visible = False
    '    '    End If
    '    'Next      
    'End Sub
    'Start:Dinesh k               2015.05.05          BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
    Private Sub DataGridWorkSheets_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles DataGridWorkSheets.ItemDataBound
        'START: PPP | 09/16/2016 | YRS-AT-2529 | It is disabling LN and LD as well as those account rows which contains 0 value in each bucket. 
        Dim chkSelect As CheckBox
        Dim accountTypeIndex, personNonTaxableIndex, personTaxableIndex, personInterestIndex, ymcaNonTaxableIndex, ymcaInterestIndex As Integer
        Dim personNonTaxable, personTaxable, personInterest, ymcaNonTaxable, ymcaInterest As Decimal
        Dim isToKeptDisabled As Boolean = False

        accountTypeIndex = 1
        personNonTaxableIndex = 2
        personTaxableIndex = 3
        personInterestIndex = 4
        ymcaNonTaxableIndex = 6
        ymcaInterestIndex = 7

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.Cells(accountTypeIndex).Text.Trim.ToLower = "ln" Or e.Item.Cells(accountTypeIndex).Text.Trim.ToLower = "ld" Then
                'chkSelect = CType(e.Item.FindControl("Checkbox1"), CheckBox)
                'chkSelect.Enabled = False
                'chkSelect.Checked = False
                isToKeptDisabled = True
            Else
                personNonTaxable = Convert.ToDecimal(e.Item.Cells(personNonTaxableIndex).Text.Trim)
                personTaxable = Convert.ToDecimal(e.Item.Cells(personTaxableIndex).Text.Trim)
                personInterest = Convert.ToDecimal(e.Item.Cells(personInterestIndex).Text.Trim)
                ymcaNonTaxable = Convert.ToDecimal(e.Item.Cells(ymcaNonTaxableIndex).Text.Trim)
                ymcaInterest = Convert.ToDecimal(e.Item.Cells(ymcaInterestIndex).Text.Trim)

                If personNonTaxable = 0 And personTaxable = 0 And personInterest = 0 And ymcaNonTaxable = 0 And ymcaInterest = 0 Then
                    isToKeptDisabled = True
                End If
            End If

            If isToKeptDisabled Then
                chkSelect = CType(e.Item.FindControl("Checkbox1"), CheckBox)
                chkSelect.Enabled = False
                chkSelect.Checked = False
            End If
        End If
    End Sub
    'End:Dinesh k               2015.05.05          BT:2699: YRS 5.0-2441 : Modifications for 403b Loans

    'PPP | 09/12/2016 | YRS-AT-2529 | Removed unnecessary code from here. Old code can be checked from TF.

    'START: PPP | 08/02/2016 | YRS-AT-2990 -  YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215)
    Public Function ValidateEachBucket() As Boolean
        'START: PPP | 09/07/2016 | YRS-AT-2529 | Splited values are being fetched from A and B group
        Dim combinedParticipantSplitDetails As DataSet
        Dim participantCurrentBalance As DataSet

        Dim recipientAfterSplitValuesTable As DataTable
        Dim finalRow As DataRow
        Dim distinctAccountTypes As List(Of String)
        Dim allAccountRows() As DataRow
        Dim currentBalanceRows, splittedRows As DataRow()
        Dim personPreTax As Decimal, personPostTax As Decimal, personInterest As Decimal
        Dim ymcaPreTax As Decimal, ymcaInterest As Decimal
        Dim isCurrentBalancePositiveAfterSplit As Boolean = True

        dtPartAccount = Me.Session_datatable_dtPartAccount
        recipientAfterSplitValuesTable = dtPartAccount.Clone

        combinedParticipantSplitDetails = Me.Session_dataset_dsALLGroupARecipantDetails.Copy

        dsALLGroupBRecipantDetails = Me.Session_dataset_dsALLGroupBRecipantDetails.Copy
        For i As Integer = 0 To dsALLGroupBRecipantDetails.Tables(0).Rows.Count - 1
            combinedParticipantSplitDetails.Tables(0).ImportRow(dsALLGroupBRecipantDetails.Tables(0).Rows(i))
        Next

        distinctAccountTypes = GetDistinctSplitedAccountTypes(combinedParticipantSplitDetails.Tables(0))
        For Each accountType As String In distinctAccountTypes
            personPreTax = 0
            personPostTax = 0
            ymcaPreTax = 0
            personInterest = 0
            ymcaInterest = 0

            ' Principal
            allAccountRows = combinedParticipantSplitDetails.Tables(0).Select(String.Format("AcctType='{0}' and TransactType='QSPR'", accountType))
            If allAccountRows.Length > 0 Then
                For Each accountRows As DataRow In allAccountRows
                    personPreTax += Convert.ToDecimal(IIf(Convert.IsDBNull(accountRows("PersonalPreTax")), 0, accountRows("PersonalPreTax")))
                    personPostTax += Convert.ToDecimal(IIf(Convert.IsDBNull(accountRows("PersonalPostTax")), 0, accountRows("PersonalPostTax")))
                    ymcaPreTax += Convert.ToDecimal(IIf(Convert.IsDBNull(accountRows("YmcaPreTax")), 0, accountRows("YmcaPreTax")))
                Next
            End If

            ' Interest
            allAccountRows = combinedParticipantSplitDetails.Tables(0).Select(String.Format("AcctType='{0}' and TransactType='QSIN'", accountType))
            If allAccountRows.Length > 0 Then
                For Each accountRows As DataRow In allAccountRows
                    personInterest += Convert.ToDecimal(IIf(Convert.IsDBNull(accountRows("PersonalPreTax")), 0, accountRows("PersonalPreTax")))
                    ymcaInterest += Convert.ToDecimal(IIf(Convert.IsDBNull(accountRows("YmcaPreTax")), 0, accountRows("YmcaPreTax")))
                Next
            End If

            finalRow = recipientAfterSplitValuesTable.NewRow
            finalRow("AcctType") = accountType
            finalRow("PersonalPreTax") = personPreTax
            finalRow("PersonalPostTax") = personPostTax
            finalRow("PersonalInterestBalance") = personInterest
            finalRow("PersonalTotal") = personPreTax + personPostTax + personInterest
            finalRow("YMCAPreTax") = ymcaPreTax
            finalRow("YMCAInterestBalance") = ymcaInterest
            finalRow("YMCATotal") = ymcaPreTax + ymcaInterest
            finalRow("TotalTotal") = personPreTax + personPostTax + personInterest + ymcaPreTax + ymcaInterest
            recipientAfterSplitValuesTable.Rows.Add(finalRow)
        Next

        participantCurrentBalance = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAccountContributionInfo("01/01/1900", DateAndTime.DateString(), Me.string_FundEventID, True)
        For Each accountType As String In distinctAccountTypes
            currentBalanceRows = participantCurrentBalance.Tables("AccountContributions").Select(String.Format("Acct='{0}'", accountType))
            splittedRows = recipientAfterSplitValuesTable.Select(String.Format("AcctType='{0}'", accountType))
            If (Not currentBalanceRows Is Nothing AndAlso currentBalanceRows.Count > 0 AndAlso Not splittedRows Is Nothing AndAlso splittedRows.Count > 0) Then
                If ((Convert.ToDecimal(currentBalanceRows(0)("EmpTaxable")) < Convert.ToDecimal(splittedRows(0)("PersonalPreTax"))) _
                     Or (Convert.ToDecimal(currentBalanceRows(0)("EmpNonTaxable")) < Convert.ToDecimal(splittedRows(0)("PersonalPostTax"))) _
                     Or (Convert.ToDecimal(currentBalanceRows(0)("EmpInterest")) < Convert.ToDecimal(splittedRows(0)("PersonalInterestBalance"))) _
                     Or (Convert.ToDecimal(currentBalanceRows(0)("YmcaTaxable")) < Convert.ToDecimal(splittedRows(0)("YMCAPreTax"))) _
                     Or (Convert.ToDecimal(currentBalanceRows(0)("YmcaInterest")) < Convert.ToDecimal(splittedRows(0)("YMCAInterestBalance")))) Then
                    isCurrentBalancePositiveAfterSplit = False
                    Exit For
                End If
            End If
        Next
        Return isCurrentBalancePositiveAfterSplit

        'Dim dtTotalSplitAmount As DataTable = Me.Session_datatable_dtTempRecipientAccount
        'Dim lsAccountTypes As List(Of String)
        'Dim drGroupedRow As DataRow
        'Dim drCurrentBalanceRows, drSplittedRows As DataRow()
        'Dim dsGetParticipantBalance As DataSet
        'Dim bResult As Boolean = True

        'If Not dtTotalSplitAmount Is Nothing Then
        '    lsAccountTypes = New List(Of String)()
        '    For counter As Integer = 0 To DataGridWorkSheets.Items.Count - 1
        '        lsAccountTypes.Add(DataGridWorkSheets.Items(counter).Cells(1).Text)
        '    Next

        '    Dim dtGroupedTotalSplitAmount As DataTable = dtTotalSplitAmount.Clone
        '    For Each strIndividualAccount As String In lsAccountTypes
        '        drGroupedRow = dtGroupedTotalSplitAmount.NewRow
        '        drGroupedRow("AcctType") = strIndividualAccount
        '        drGroupedRow("PersonalPreTax") = 0
        '        drGroupedRow("PersonalPostTax") = 0
        '        drGroupedRow("PersonalInterestBalance") = 0
        '        drGroupedRow("PersonalTotal") = 0
        '        drGroupedRow("YMCAPreTax") = 0
        '        drGroupedRow("YMCAInterestBalance") = 0
        '        drGroupedRow("YMCATotal") = 0
        '        drGroupedRow("TotalTotal") = 0

        '        drSplittedRows = dtTotalSplitAmount.Select(String.Format("AcctType='{0}'", strIndividualAccount))
        '        For Each row As DataRow In drSplittedRows
        '            drGroupedRow("PersonalPreTax") += Convert.ToDecimal(row("PersonalPreTax"))
        '            drGroupedRow("PersonalPostTax") += Convert.ToDecimal(row("PersonalPostTax"))
        '            drGroupedRow("PersonalInterestBalance") += Convert.ToDecimal(row("PersonalInterestBalance"))
        '            drGroupedRow("PersonalTotal") += Convert.ToDecimal(row("PersonalTotal"))
        '            drGroupedRow("YMCAPreTax") += Convert.ToDecimal(row("YMCAPreTax"))
        '            drGroupedRow("YMCAInterestBalance") += Convert.ToDecimal(row("YMCAInterestBalance"))
        '            drGroupedRow("YMCATotal") += Convert.ToDecimal(row("YMCATotal"))
        '            drGroupedRow("TotalTotal") += Convert.ToDecimal(row("TotalTotal"))
        '        Next
        '        dtGroupedTotalSplitAmount.Rows.Add(drGroupedRow)
        '    Next

        '    If (participantCurrentBalance Is Nothing) Then
        '        dsGetParticipantBalance = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAccountContributionInfo("01/01/1900", DateAndTime.DateString(), Me.string_FundEventID, True)
        '        participantCurrentBalance = dsGetParticipantBalance
        '    Else
        '        dsGetParticipantBalance = participantCurrentBalance
        '    End If

        '    For Each strAccountType As String In lsAccountTypes
        '        drCurrentBalanceRows = dsGetParticipantBalance.Tables("AccountContributions").Select(String.Format("Acct='{0}'", strAccountType))
        '        drSplittedRows = dtGroupedTotalSplitAmount.Select(String.Format("AcctType='{0}'", strAccountType))
        '        If (Not drCurrentBalanceRows Is Nothing AndAlso drCurrentBalanceRows.Count > 0 AndAlso Not drSplittedRows Is Nothing AndAlso drSplittedRows.Count > 0) Then
        '            If ((Convert.ToDecimal(drCurrentBalanceRows(0)("EmpTaxable")) < Convert.ToDecimal(drSplittedRows(0)("PersonalPreTax"))) _
        '                 Or (Convert.ToDecimal(drCurrentBalanceRows(0)("EmpNonTaxable")) < Convert.ToDecimal(drSplittedRows(0)("PersonalPostTax"))) _
        '                 Or (Convert.ToDecimal(drCurrentBalanceRows(0)("EmpInterest")) < Convert.ToDecimal(drSplittedRows(0)("PersonalInterestBalance"))) _
        '                 Or (Convert.ToDecimal(drCurrentBalanceRows(0)("YmcaTaxable")) < Convert.ToDecimal(drSplittedRows(0)("YMCAPreTax"))) _
        '                 Or (Convert.ToDecimal(drCurrentBalanceRows(0)("YmcaInterest")) < Convert.ToDecimal(drSplittedRows(0)("YMCAInterestBalance")))) Then
        '                bResult = False
        '                Exit For
        '            End If
        '        End If
        '    Next
        'End If

        'Return bResult
        'END: PPP | 09/07/2016 | YRS-AT-2529 | Splited values are being fetched from A and B group
    End Function
    'END: PPP | 08/02/2016 | YRS-AT-2990 -  YRS enh-additional validation to prevent negative balances after QDRO split (TrackIT 26215)

    'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Enabling and diabling martial status and gender.
    Private Sub EnableDisableGenderMaritalControl(ByVal bnFlag As Boolean)
        DropDownListGender.Enabled = bnFlag
        DropDownListMaritalStatus.Enabled = bnFlag
    End Sub
    'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Enabling and diabling martial status and gender.

    'PPP | 08/24/2016 | YRS-AT-2529 
    Private Sub ShowHideControls()
        Dim isFirstLoad As Boolean

        Dim isPlanSelected As Boolean
        Dim isBothPlanSelected As Boolean, isRetirementSelected As Boolean, isSavingsPlanSelected As Boolean
        Dim isLockOnDateRange As Boolean
        Dim isSplitInProgress As Boolean
        Dim isSplitRangeDefined As Boolean

        Dim splitConfigurationRows As DataRow()
        Try
            'START: MMR | 11/23/2016 | YRS-AT-3145 | Index's are identified using enums
            'If QdroMemberActiveTabStrip.SelectedIndex = 2 Then
            If QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts Then
                'END: MMR | 11/23/2016 | YRS-AT-3145 | Index's are identified using enums
                ' Need to disable all validators whaich are on "Beneficiary" tab.
                CType(Me.FindControl("reqFirstName"), RequiredFieldValidator).Enabled = False
                CType(Me.FindControl("Requiredfieldvalidator1"), RequiredFieldValidator).Enabled = False
                TextBoxBirthDate.RequiredDate = False
                ManageEditableControls(False)
                CType(Me.FindControl("compValidatorGender"), CompareValidator).Enabled = False
                CType(Me.FindControl("compValidatorMaritalStatus"), CompareValidator).Enabled = False

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
                trAdjustInterest.Style("display") = "none"
                EnableDateRange(False)

                ' Disable Everything
                DataGridWorkSheets.DataSource = Nothing
                DataGridWorkSheets.DataBind()

                DataGridWorkSheet2.DataSource = Nothing
                DataGridWorkSheet2.DataBind()

                DataGridWorkSheet.DataSource = Nothing
                DataGridWorkSheet.DataBind()

                EnableSplitButtonSet(False)

                If hdnSelectedPlanType.Value <> "" Then
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

                    If Not Me.Session_datatable_dtPecentageCount Is Nothing Then
                        dtPecentageCount = Me.Session_datatable_dtPecentageCount
                        splitConfigurationRows = dtPecentageCount.Select(String.Format("PersId='{0}'", Me.String_Benif_PersonD))
                        If splitConfigurationRows.Length > 0 Then
                            For Each configRow In splitConfigurationRows
                                If configRow("PlanType") = hdnSelectedPlanType.Value Then
                                    isSplitInProgress = True
                                    Exit For
                                End If
                            Next
                            If Not isSplitInProgress Then
                                isSplitRangeDefined = True
                            End If
                        End If
                        If dtPecentageCount.Rows.Count > 0 Then
                            isLockOnDateRange = True
                        End If
                    End If
                Else
                    If Not Me.Session_datatable_dtPecentageCount Is Nothing Then
                        dtPecentageCount = Me.Session_datatable_dtPecentageCount
                        If dtPecentageCount.Rows.Count > 0 Then
                            isLockOnDateRange = True
                        End If
                    End If
                End If

                isFirstLoad = True
                If isPlanSelected Or isSplitRangeDefined Or isSplitInProgress Then
                    isFirstLoad = False
                End If

                If isFirstLoad Then
                    spanBothPlans.Style("display") = "normal"
                    lnbRetirement.Style("display") = "normal"
                    lnbSavings.Style("display") = "normal"
                    loadAccInitital()
                    Call SetControlFocusDropDown(Me.cboBeneficiarySSNo)
                ElseIf isPlanSelected And Not isSplitInProgress And Not isSplitRangeDefined Then
                    trPlanInProgressHeader.Style("display") = "normal"
                    trPlanInProgressEmptyRow.Style("display") = "normal"
                    trAmountPercentage.Style("display") = "normal"
                    trAdjustInterest.Style("display") = "normal"
                    EnableDateRange(Not isLockOnDateRange)
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
                    ''START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code and disabling Manage fees tab
                    ''Me.QdroMemberActiveTabStrip.Items(3).Enabled = False
                    'EnableDisableQDROTabStrip(EnumNonRetiredQDROTabs.ManageFees, False)
                    ''END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code and disabling Manage fees tab
                    ChkAdjustInterest.Enabled = True
                ElseIf isSplitInProgress Then
                    ShowHideControlsAfterSplit()
                    ''START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code and Enabling Manage fees tab
                    ''Me.QdroMemberActiveTabStrip.Items(3).Enabled = True
                    'EnableDisableQDROTabStrip(EnumNonRetiredQDROTabs.ManageFees, True)
                    ''END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code and Enabling Manage fees tab
                ElseIf isSplitRangeDefined Then
                    ShowHideControlsForPendingSplit()
                    ''START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code and Enabling Manage fees tab
                    ''Me.QdroMemberActiveTabStrip.Items(3).Enabled = True
                    'EnableDisableQDROTabStrip(EnumNonRetiredQDROTabs.ManageFees, True)
                    ''END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code and Enabling Manage fees tab
                    If (TextBoxPercentageWorkSheet.Text <> "0.00" And TextBoxPercentageWorkSheet.Text <> "") Or (TextBoxAmountWorkSheet.Text <> "0.00" And TextBoxAmountWorkSheet.Text <> "") Then
                        ButtonSplit.Enabled = True
                    End If
                End If

                ' Handle selected option
                If Not isSplitInProgress And Not isSplitRangeDefined And TextboxBegDate.Text.Trim().Length > 0 And TextboxEndDate.Text.Trim().Length > 0 Then
                    If isPlanSelected Then
                        If Not isLockOnDateRange Then
                            LoadAccountsTab()
                        Else
                            LoadOriginalBalance()
                        End If
                    End If
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
            End If
            ShowOtherPlanReminderWarning()
        Catch
            Throw
        Finally
            splitConfigurationRows = Nothing
        End Try
    End Sub

    Private Sub ShowHideControlsAfterSplit()
        Dim isBothPlanSelected As Boolean, isRetirementSelected As Boolean, isSavingsPlanSelected As Boolean
        Dim splitConfigurationRows As DataRow()

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
        trAdjustInterest.Style("display") = "normal"

        TextboxBegDate.Enabled = False
        PopcalendarRecDate.Enabled = False
        TextboxEndDate.Enabled = False
        PopcalendarRecDate2.Enabled = False

        EnableSplitButtonSet(True)
        'ButtonDocumentSave.Visible = True 'MMR | 2016.11.23 | YRS-AT-3145 | Commented as not required
        ButtonSplit.Enabled = False

        ChkAdjustInterest.Enabled = False

        dtPecentageCount = Me.Session_datatable_dtPecentageCount
        If Not dtPecentageCount Is Nothing Then
            splitConfigurationRows = dtPecentageCount.Select(String.Format("PersId='{0}' and PlanType='{1}'", Me.String_Benif_PersonD, hdnSelectedPlanType.Value))

            If splitConfigurationRows.Length > 0 Then
                If splitConfigurationRows(0)("Amount") > 0 Then
                    RadioButtonListSplitAmtType_Amount.Checked = True
                    RadioButtonListSplitAmtType_Percentage.Checked = False
                    TextBoxAmountWorkSheet.Text = Convert.ToDecimal(splitConfigurationRows(0)("Amount")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    TextBoxPercentageWorkSheet.Text = "0.00"
                ElseIf splitConfigurationRows(0)("Percentage") > 0 Then
                    RadioButtonListSplitAmtType_Percentage.Checked = True
                    RadioButtonListSplitAmtType_Amount.Checked = False
                    TextBoxPercentageWorkSheet.Text = Convert.ToDecimal(splitConfigurationRows(0)("Percentage")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    TextBoxAmountWorkSheet.Text = "0.00"
                End If

                RadioButtonListSplitAmtType_Amount.Disabled = True
                RadioButtonListSplitAmtType_Percentage.Disabled = True
                TextBoxAmountWorkSheet.Enabled = False
                TextBoxPercentageWorkSheet.Enabled = False
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

        LoadOriginalBalance()
        LoadSplitedBalance() ' This function will load Recipient and Participant after split grid

        'START: PPP | 09/13/2016 | YRS-AT-1973 | Handling Adjust button enabled property
        ' Check how many account was selected for split, if only 1 account exists then disable the adjust button
        Dim beneficiaryItems As DataGridItem
        Dim accountCheckBox As CheckBox
        Dim selectedAccountCounter As Integer = 0
        dtPartAccount = Me.Session_datatable_dtPartAccount
        For participantRowCounter As Integer = 0 To dtPartAccount.Rows.Count - 1
            beneficiaryItems = DataGridWorkSheets.Items(participantRowCounter)
            accountCheckBox = CType(beneficiaryItems.Cells(0).Controls(1), CheckBox)
            If accountCheckBox.Checked = True Then
                selectedAccountCounter += 1
            End If
        Next
        If selectedAccountCounter = 1 Then
            ButtonAdjust.Enabled = False
        End If
        'END: PPP | 09/13/2016 | YRS-AT-1973 | Handling Adjust button enabled property
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
        trAdjustInterest.Style("display") = "normal"

        EnableDateRange(False)

        EnableSplitButtonSet(False)
        ' split is defined we can enable the "Show Balance" button
        btnShowBalance.Enabled = True
        ChkAdjustInterest.Enabled = False

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

        LoadOriginalBalance()
    End Sub

    Private Sub EnableDateRange(ByVal flag As Boolean)
        TextboxBegDate.Enabled = flag
        PopcalendarRecDate.Enabled = flag
        TextboxEndDate.Enabled = flag
        PopcalendarRecDate2.Enabled = flag
    End Sub

    Private Sub MaintainSplitConfiguration()
        Dim splitConfigurationRows As DataRow()
        Dim configurationRow As DataRow
        Dim planType As String
        Dim isPlanExists As Boolean
        Try
            planType = hdnSelectedPlanType.Value.Trim()
            isPlanExists = True
            If Me.Session_datatable_dtPecentageCount Is Nothing Then
                CreatePecentageAmountTable()
                isPlanExists = False
            Else
                dtPecentageCount = Me.Session_datatable_dtPecentageCount
                splitConfigurationRows = dtPecentageCount.Select(String.Format("PersId='{0}' and PlanType='{1}'", Me.String_Benif_PersonD, planType))
                If Not splitConfigurationRows Is Nothing AndAlso splitConfigurationRows.Length = 0 Then
                    isPlanExists = False
                End If
            End If

            If Not isPlanExists Then
                configurationRow = dtPecentageCount.NewRow
                configurationRow("PersId") = Me.String_Benif_PersonD
                configurationRow("PlanType") = planType
                configurationRow("Percentage") = Decimal.Parse(TextBoxPercentageWorkSheet.Text)
                configurationRow("Amount") = Decimal.Parse(TextBoxAmountWorkSheet.Text)
                dtPecentageCount.Rows.Add(configurationRow)
                Me.Session_datatable_dtPecentageCount = dtPecentageCount
            End If
        Catch
            Throw
        Finally
            splitConfigurationRows = Nothing
            configurationRow = Nothing
            planType = Nothing
        End Try
    End Sub

    Private Function GetPlanTypeForSelectedAccount(ByVal accountDetails As DataTable, ByVal accountType As String) As String
        Dim planType As String
        Dim rows As DataRow()
        Try
            rows = accountDetails.Select(String.Format("AcctType='{0}'", accountType))
            planType = String.Empty
            If Not rows Is Nothing AndAlso rows.Length > 0 Then
                planType = rows(0)("PlanType")
            End If
            Return planType
        Catch
            Throw
        Finally
            rows = Nothing
            planType = Nothing
        End Try
    End Function

    Private Sub LoadOriginalBalance()
        Dim rows As DataRow()
        Dim participantAccountRow As DataRow
        Dim planType As String
        Dim distinctSplitedAccountTypes As List(Of String)
        Try
            planType = hdnSelectedPlanType.Value

            CreateParticipantTable()

            l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail
            dtAcctType = New DataTable()
            If planType.ToUpper() <> "BOTH" Then
                GetDistinctAccountTypes(l_dataset_PartAccountDetail.Tables(0).Select(String.Format("PlanType='{0}'", planType)))
            Else
                GetDistinctAccountTypes(l_dataset_PartAccountDetail.Tables(0).Select(""))
            End If
            distinctSplitedAccountTypes = GetDistinctSplitedAccountTypes(planType, Me.string_RecptFundEventID)
            If distinctSplitedAccountTypes.Count = 0 Then
                ' No account splited yet, so load default account types for selected plans
                For Each accountRow As DataRow In dtAcctType.Rows
                    If Not distinctSplitedAccountTypes.Contains(accountRow("AcctType")) Then
                        distinctSplitedAccountTypes.Add(accountRow("AcctType"))
                    End If
                Next
            End If

            For iCount = 0 To dtAcctType.Rows.Count - 1
                rows = l_dataset_PartAccountDetail.Tables(0).Select(String.Format("AcctType='{0}'", dtAcctType.Rows(iCount).Item(0)))
                participantAccountRow = dtPartAccount.NewRow
                participantAccountRow("PersonalPreTax") = 0
                participantAccountRow("PersonalPostTax") = 0
                participantAccountRow("PersonalInterestBalance") = 0
                participantAccountRow("YMCAPreTax") = 0
                participantAccountRow("YMCAInterestBalance") = 0
                For iAcct = 0 To rows.Length - 1
                    participantAccountRow("AcctType") = rows(iAcct).Item("AcctType")
                    participantAccountRow("PersonalPreTax") += IIf(IsDBNull(rows(iAcct).Item("PersonalPreTax")), 0, rows(iAcct).Item("PersonalPreTax"))
                    participantAccountRow("PersonalPostTax") += IIf(IsDBNull(rows(iAcct).Item("PersonalPostTax")), 0, rows(iAcct).Item("PersonalPostTax"))
                    participantAccountRow("PersonalInterestBalance") += IIf(IsDBNull(rows(iAcct).Item("PersonalInterestBalance")), 0, rows(iAcct).Item("PersonalInterestBalance"))
                    participantAccountRow("PersonalTotal") = 0
                    participantAccountRow("YMCAPreTax") += IIf(IsDBNull(rows(iAcct).Item("YMCAPreTax")), 0, rows(iAcct).Item("YMCAPreTax"))
                    participantAccountRow("YMCAInterestBalance") += IIf(IsDBNull(rows(iAcct).Item("YMCAInterestBalance")), 0, rows(iAcct).Item("YMCAInterestBalance"))
                    participantAccountRow("YMCATotal") = 0
                    participantAccountRow("TotalTotal") = 0
                    If (distinctSplitedAccountTypes.Contains(rows(iAcct).Item("AcctType"))) Then
                        participantAccountRow("Selected") = True
                    Else
                        participantAccountRow("Selected") = False
                    End If
                Next
                participantAccountRow("PersonalTotal") = IIf(IsDBNull(participantAccountRow("PersonalPreTax")), 0, participantAccountRow("PersonalPreTax")) + IIf(IsDBNull(participantAccountRow("PersonalPostTax")), 0, participantAccountRow("PersonalPostTax")) + IIf(IsDBNull(participantAccountRow("PersonalInterestBalance")), 0, participantAccountRow("PersonalInterestBalance"))
                participantAccountRow("YMCATotal") = IIf(IsDBNull(participantAccountRow("YMCAPreTax")), 0, participantAccountRow("YMCAPreTax")) + IIf(IsDBNull(participantAccountRow("YMCAInterestBalance")), 0, participantAccountRow("YMCAInterestBalance"))
                participantAccountRow("TotalTotal") = participantAccountRow("YMCATotal") + participantAccountRow("PersonalTotal")
                If Me.string_PersId <> Nothing Then
                    participantAccountRow("PersId") = Me.string_PersId
                End If
                dtPartAccount.Rows.Add(participantAccountRow)
            Next

            Me.Session_datatable_dtPartAccount = dtPartAccount
            DataGridWorkSheets.DataSource = dtPartAccount
            DataGridWorkSheets.DataBind()
        Catch
            Throw
        Finally
            distinctSplitedAccountTypes = Nothing
            planType = Nothing
            participantAccountRow = Nothing
            rows = Nothing
        End Try
    End Sub

    Private Sub LoadSplitedBalance()
        Dim participantAfterSplitValuesTable As DataTable
        Dim recipientAfterSplitValuesTable As DataTable
        Try
            If Not Me.Session_dataset_dsAllPartAccountsDetail Is Nothing Then
                ' Prepare participant table
                dsAllPartAccountsDetail = Me.Session_dataset_dsAllPartAccountsDetail
                l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail ' Original balance
                participantAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable(hdnSelectedPlanType.Value, dsAllPartAccountsDetail, l_dataset_PartAccountDetail)

                ' Prepare recipient table
                dsAllRecipantAccountsDetail = Me.Session_dataset_dsAllRecipantAccountsDetail
                recipientAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareBeneficiaryAfterSplitAccountTable(Me.string_RecptFundEventID, hdnSelectedPlanType.Value, dsAllRecipantAccountsDetail, l_dataset_PartAccountDetail)

                ' Load Participants data
                DataGridWorkSheet.DataSource = participantAfterSplitValuesTable
                DataGridWorkSheet.DataBind()

                ' Load Recipient's data
                DataGridWorkSheet2.DataSource = recipientAfterSplitValuesTable
                DataGridWorkSheet2.DataBind()
            End If
        Catch
            Throw
        Finally
            recipientAfterSplitValuesTable = Nothing
            participantAfterSplitValuesTable = Nothing
        End Try
    End Sub

    Private Sub SetBeneficiaryData()
        Dim splitConfigurationRows As DataRow()
        Try
            ' Set beneficiary sessions
            Me.String_Benif_PersonD = cboBeneficiarySSNo.SelectedItem.Value
            For i As Integer = 0 To DatagridBenificiaryList.Items.Count - 1
                If DatagridBenificiaryList.Items(i).Cells(recipientIndexPersID).Text.ToString() = cboBeneficiarySSNo.SelectedItem.Value Then 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Changed the index access from hardcoded 1 to by a variable recipientIndexPersID
                    Me.string_RecptFundEventID = DatagridBenificiaryList.Items(i).Cells(recipientIndexFundEventID).Text.ToString() 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Changed the index access from hardcoded 6 to by a variable recipientIndexFundEventID
                    Exit For
                End If
            Next

            ' find out existing split details
            If Not Me.Session_datatable_dtPecentageCount Is Nothing Then
                dtPecentageCount = Me.Session_datatable_dtPecentageCount
                splitConfigurationRows = dtPecentageCount.Select(String.Format("PersId='{0}'", Me.String_Benif_PersonD))
                If splitConfigurationRows.Length > 0 Then
                    hdnSelectedPlanType.Value = splitConfigurationRows(0)("PlanType")
                End If
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

    Private Sub SetDefaultAmountPercetageSetting()
        TextBoxPercentageWorkSheet.Text = "0.00"
        TextBoxAmountWorkSheet.Text = "0.00"

        RadioButtonListSplitAmtType_Amount.Disabled = False
        TextBoxAmountWorkSheet.Enabled = False

        RadioButtonListSplitAmtType_Percentage.Disabled = False
        TextBoxPercentageWorkSheet.Enabled = True

        RadioButtonListSplitAmtType_Percentage.Checked = True
        RadioButtonListSplitAmtType_Amount.Checked = False
    End Sub

    Private Sub lnbBothPlans_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnbBothPlans.Click
        SetDefaultAmountPercetageSetting()
        ShowHideControls()
    End Sub

    Private Sub lnbRetirement_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnbRetirement.Click
        SetDefaultAmountPercetageSetting()
        ShowHideControls()
    End Sub

    Private Sub lnbSavings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnbSavings.Click
        SetDefaultAmountPercetageSetting()
        ShowHideControls()
    End Sub

    Private Function IsAllBeneficiarySplitDataExists() As Boolean
        Dim isAllExists As Boolean
        Try
            isAllExists = True
            dtPecentageCount = Me.Session_datatable_dtPecentageCount
            dtBenifAccount = Me.Session_datatable_dtBenifAccount

            If (HelperFunctions.isNonEmpty(dtPecentageCount) And HelperFunctions.isNonEmpty(dtBenifAccount)) Then
                For Each beneficiaryRow As DataRow In dtBenifAccount.Rows
                    If dtPecentageCount.Select(String.Format("PersId='{0}'", Convert.ToString(beneficiaryRow("id")))).Length = 0 Then
                        isAllExists = False
                        Exit For
                    End If
                Next
            End If

            Return isAllExists
        Catch
            Throw
        Finally

        End Try
    End Function

    Private Sub ResetSelectedPlan()
        Dim planType As String
        Dim recipientPersID As String
        Dim splitConfigurationRows() As DataRow
        Dim isPlanExists As Boolean
        Try
            planType = hdnSelectedPlanType.Value
            recipientPersID = Me.String_Benif_PersonD
            dtPecentageCount = Me.Session_datatable_dtPecentageCount
            isPlanExists = False

            ' delete split configuration details
            splitConfigurationRows = dtPecentageCount.Select(String.Format("PersId='{0}' and PlanType='{1}'", recipientPersID, planType))
            If splitConfigurationRows.Length > 0 Then
                splitConfigurationRows(0).Delete()
                dtPecentageCount.AcceptChanges()
                If dtPecentageCount.Rows.Count = 0 Then
                    dtPecentageCount = Nothing
                End If
                Me.Session_datatable_dtPecentageCount = dtPecentageCount
                isPlanExists = True
            End If

            If isPlanExists Then
                ' Delete Participant split data
                Me.Session_dataset_dsAllPartAccountsDetail = DeleteSplitAccountDetails(Me.Session_dataset_dsAllPartAccountsDetail, recipientPersID, planType, True)
                Me.Session_dataset_dsALLGroupAParticipantDetails = DeleteSplitAccountDetails(Me.Session_dataset_dsALLGroupAParticipantDetails, recipientPersID, planType, True)
                Me.Session_dataset_dsALLGroupBParticipantDetails = DeleteSplitAccountDetails(Me.Session_dataset_dsALLGroupBParticipantDetails, recipientPersID, planType, True)

                ' Delete Recipient split data
                Me.Session_dataset_dsAllRecipantAccountsDetail = DeleteSplitAccountDetails(Me.Session_dataset_dsAllRecipantAccountsDetail, recipientPersID, planType, False)
                Me.Session_dataset_dsALLGroupARecipantDetails = DeleteSplitAccountDetails(Me.Session_dataset_dsALLGroupARecipantDetails, recipientPersID, planType, False)
                Me.Session_dataset_dsALLGroupBRecipantDetails = DeleteSplitAccountDetails(Me.Session_dataset_dsALLGroupBRecipantDetails, recipientPersID, planType, False)

                'START: PPP | 09/27/2016 | YRS-AT-2529 | Showing user which plan got reset
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", String.Format(GetMessageFromResource("MESSAGE_QRDO_RESET"), planType.ToUpper()), MessageBoxButtons.OK)
                ShowModalPopupWithCustomMessage("QDRO", String.Format(GetMessageFromResource("MESSAGE_QRDO_RESET"), planType.ToUpper()), "ok")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'END: PPP | 09/27/2016 | YRS-AT-2529 | Showing user which plan got reset

                'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After resetting split information, defined fee must be reset
                If HelperFunctions.isNonEmpty(dtPecentageCount) Then
                    If chkApplyFees.Checked And (HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) AndAlso HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees)) Then
                        ResetFees()
                    End If
                Else
                    Me.ParticipantTotalBalanceAfterSplitManageFees = Nothing
                    Me.RecipientBalanceAfterSplitManageFees = Nothing
                End If
                'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After resetting split information, defined fee must be reset
            End If
        Catch
            Throw
        Finally
            splitConfigurationRows = Nothing
            recipientPersID = Nothing
            planType = Nothing
        End Try
    End Sub

    Private Function DeleteSplitAccountDetails(ByVal splitAccountDetails As DataSet, ByVal recipientPersID As String, ByVal planType As String, ByVal isParticipantDetails As Boolean) As DataSet
        Dim accountRows() As DataRow
        If planType.ToLower = "both" Then
            If isParticipantDetails Then
                accountRows = splitAccountDetails.Tables(0).Select(String.Format("BenfitPersId='{0}'", recipientPersID))
            Else
                accountRows = splitAccountDetails.Tables(0).Select(String.Format("PersID='{0}'", recipientPersID))
            End If
        Else
            If isParticipantDetails Then
                accountRows = splitAccountDetails.Tables(0).Select(String.Format("BenfitPersId='{0}' and PlanType='{1}'", recipientPersID, planType))
            Else
                accountRows = splitAccountDetails.Tables(0).Select(String.Format("PersID='{0}' and PlanType='{1}'", recipientPersID, planType))
            End If
        End If
        For Each accountRow As DataRow In accountRows
            accountRow.Delete()
        Next
        splitAccountDetails.AcceptChanges()
        'START: PPP | 09/27/2016 | YRS-AT-2529 | Following lines of code was causing "object reference null" error
        'If splitAccountDetails.Tables(0).Rows.Count = 0 Then
        '    splitAccountDetails = Nothing
        'End If
        'END: PPP | 09/27/2016 | YRS-AT-2529 | Following lines of code was causing "object reference null" error
        Return splitAccountDetails
    End Function

    Private Function IsSplitInValid() As Boolean
        'START: PPP | 12/12/2016 | YRS-AT-2990 | Droped participantAfterSplitValuesTable
        'Dim participantAfterSplitValuesTable, proposedSplitTable As DataTable
        Dim proposedSplitTable As DataTable
        'END: PPP | 12/12/2016 | YRS-AT-2990 | Droped participantAfterSplitValuesTable
        Dim balanceRows(), proposedSplitRows() As DataRow
        Dim accountCheckBox As CheckBox
        Dim accountType As String
        Dim selectedAccountsForProposedSplit As List(Of String)
        Dim totalOfSelectedAccountBalance, splitAmount, splitPercentage As Decimal
        Dim isAmountGoingBelowZero As Boolean
        Try
            'START: PPP | 12/12/2016 | YRS-AT-2990 | It was looking for after split session values which was wrong, it should check session variable which holds on screen loaded value 
            'If Me.Session_dataset_dsAllPartAccountsDetail Is Nothing Then
            If Not HelperFunctions.isNonEmpty(Me.Session_Dataset_PartAccountDetail) Then
                'END: PPP | 12/12/2016 | YRS-AT-2990 | It was looking for after split session values which was wrong, it should check session variable which holds on screen loaded value 
                isAmountGoingBelowZero = False
            Else
                'START: PPP | 12/12/2016 | YRS-AT-2990 | participantAfterSplitValuesTable variable is not required 
                '' Participant split account table contains negative value
                'dsAllPartAccountsDetail = Me.Session_dataset_dsAllPartAccountsDetail.Copy
                'l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail.Copy ' Original balance
                'participantAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable("Both", dsAllPartAccountsDetail, l_dataset_PartAccountDetail)
                l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail.Copy ' Holds on-screen loaded balance
                'END: PPP | 12/12/2016 | YRS-AT-2990 | participantAfterSplitValuesTable variable is not required 

                ' Fetch selected on screen account details
                selectedAccountsForProposedSplit = New List(Of String)()
                totalOfSelectedAccountBalance = 0
                For Each gridItem As DataGridItem In DataGridWorkSheets.Items
                    accountCheckBox = CType(gridItem.Cells(0).Controls(1), CheckBox)
                    If accountCheckBox.Checked Then
                        accountType = gridItem.Cells(1).Text.Trim()
                        For Each row As DataRow In l_dataset_PartAccountDetail.Tables(0).Rows
                            If accountType = row("AcctType") Then
                                selectedAccountsForProposedSplit.Add(accountType)
                                totalOfSelectedAccountBalance += Convert.ToDecimal(row("mnyBalance"))
                            End If
                        Next
                    End If
                Next

                ' If selected account total is greater than 0 then predict the ongoing bucketwise split amount
                If totalOfSelectedAccountBalance > 0 Then
                    If RadioButtonListSplitAmtType_Amount.Checked Then
                        If String.IsNullOrEmpty(TextBoxAmountWorkSheet.Text) Then
                            splitAmount = 0
                        Else
                            splitAmount = CType(TextBoxAmountWorkSheet.Text, Decimal)
                        End If
                        splitPercentage = splitAmount / totalOfSelectedAccountBalance
                    ElseIf RadioButtonListSplitAmtType_Percentage.Checked Then
                        splitPercentage = (TextBoxPercentageWorkSheet.Text) / 100
                        splitAmount = Math.Round(totalOfSelectedAccountBalance * splitPercentage, 2, MidpointRounding.AwayFromZero)
                    End If

                    ' Preparing proposed split table
                    proposedSplitTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareProposedSplitTable(l_dataset_PartAccountDetail, selectedAccountsForProposedSplit, splitPercentage, splitAmount)

                    'START: PPP | 12/12/2016 | YRS-AT-2990 | It is fetching balance as on date
                    ' Current Balance
                    l_dataset_PartAccountDetail = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getPartAccountDetailbyPlan(Me.string_FundEventID, TextboxBegDate.Text, DateTime.Today.ToString("MM/dd/yyyy"), "both")
                    'END: PPP | 12/12/2016 | YRS-AT-2990 | It is fetching balance as on date

                    ' Validate the split
                    For Each selectedAccountType As String In selectedAccountsForProposedSplit
                        proposedSplitRows = proposedSplitTable.Select(String.Format("AcctType='{0}'", selectedAccountType))
                        'START: PPP | 12/12/2016 | YRS-AT-2990 | Dropped participantAfterSplitValuesTable and using l_dataset_PartAccountDetail which holds current balance
                        'balanceRows = participantAfterSplitValuesTable.Select(String.Format("AcctType='{0}'", selectedAccountType))
                        balanceRows = l_dataset_PartAccountDetail.Tables(0).Select(String.Format("AcctType='{0}'", selectedAccountType))
                        'END: PPP | 12/12/2016 | YRS-AT-2990 | Dropped participantAfterSplitValuesTable and using l_dataset_PartAccountDetail which holds current balance
                        If proposedSplitRows.Length > 0 AndAlso balanceRows.Length > 0 Then
                            If IsBalanceLessThanProposedAmount("PersonalPreTax", proposedSplitRows(0), balanceRows(0)) Then
                                isAmountGoingBelowZero = True
                            ElseIf IsBalanceLessThanProposedAmount("PersonalPostTax", proposedSplitRows(0), balanceRows(0)) Then
                                isAmountGoingBelowZero = True
                            ElseIf IsBalanceLessThanProposedAmount("PersonalInterestBalance", proposedSplitRows(0), balanceRows(0)) Then
                                isAmountGoingBelowZero = True
                            ElseIf IsBalanceLessThanProposedAmount("YmcaPreTax", proposedSplitRows(0), balanceRows(0)) Then
                                isAmountGoingBelowZero = True
                            ElseIf IsBalanceLessThanProposedAmount("YmcaInterestBalance", proposedSplitRows(0), balanceRows(0)) Then
                                isAmountGoingBelowZero = True
                            End If

                            If isAmountGoingBelowZero Then
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If
            Return isAmountGoingBelowZero
        Catch
            Throw
        Finally
            l_dataset_PartAccountDetail = Nothing 'PPP | 12/12/2016 | YRS-AT-2990
            selectedAccountsForProposedSplit = Nothing
            accountType = Nothing
            accountCheckBox = Nothing
            'participantAfterSplitValuesTable = Nothing 'PPP | 12/12/2016 | YRS-AT-2990
            proposedSplitTable = Nothing
            balanceRows = Nothing
            proposedSplitRows = Nothing
        End Try
    End Function

    Private Function IsBalanceLessThanProposedAmount(ByVal bucketName As String, ByVal proposedSplitRow As DataRow, ByVal balanceRow As DataRow) As Boolean
        Dim tempProposedAmountHolder, tempBalanceAmountHolder As Decimal
        Dim isAmountGoingBelowZero As Boolean = False

        tempProposedAmountHolder = proposedSplitRow(bucketName)
        tempBalanceAmountHolder = balanceRow(bucketName)
        If (tempBalanceAmountHolder - tempProposedAmountHolder) < 0 Then
            isAmountGoingBelowZero = True
        End If

        Return isAmountGoingBelowZero
    End Function

    Private Function GetDistinctSplitedAccountTypes(ByVal planType As String, ByVal recipientFundEventID As String) As List(Of String)
        Dim distinctSplitedAccountTypes As List(Of String)
        Dim accountRows() As DataRow
        Try
            distinctSplitedAccountTypes = New List(Of String)()
            dsAllRecipantAccountsDetail = Me.Session_dataset_dsAllRecipantAccountsDetail
            If HelperFunctions.isNonEmpty(dsAllRecipantAccountsDetail) Then
                ' Split is defined
                If planType.ToLower() = "both" Then
                    accountRows = dsAllRecipantAccountsDetail.Tables(0).Select(String.Format("FundEventID='{0}'", recipientFundEventID))
                Else
                    accountRows = dsAllRecipantAccountsDetail.Tables(0).Select(String.Format("FundEventID='{0}' and PlanType='{1}'", recipientFundEventID, planType))
                End If
                If accountRows.Length > 0 Then
                    For Each accountRow As DataRow In accountRows
                        If Not distinctSplitedAccountTypes.Contains(accountRow("AcctType")) Then
                            distinctSplitedAccountTypes.Add(accountRow("AcctType"))
                        End If
                    Next
                End If
            End If
            Return distinctSplitedAccountTypes
        Catch
            Throw
        Finally
            accountRows = Nothing
            distinctSplitedAccountTypes = Nothing
        End Try
    End Function

    Private Function GetDistinctSplitedAccountTypes(ByVal data As DataTable) As List(Of String)
        Dim distinctSplitedAccountTypes As List(Of String)
        Dim accountType As String
        Try
            distinctSplitedAccountTypes = New List(Of String)()
            For Each row As DataRow In data.Rows
                accountType = row("AcctType")
                If Not distinctSplitedAccountTypes.Contains(accountType) Then
                    distinctSplitedAccountTypes.Add(accountType)
                End If
            Next
            Return distinctSplitedAccountTypes
        Catch
            Throw
        Finally
            accountType = Nothing
            distinctSplitedAccountTypes = Nothing
        End Try
    End Function

    Private Sub DrawBeneficiaryTable()
        Dim splitConfigurationTable As DataTable
        Dim beneficiaryTable As New DataTable

        Dim mainTable As System.Web.UI.HtmlControls.HtmlTable
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim beneficiaryPersID As String, beneficiaryFundEventID As String
        Try
            If Not Me.Session_datatable_dtBenifAccount Is Nothing And Not Me.Session_datatable_dtPecentageCount Is Nothing Then
                splitConfigurationTable = Me.Session_datatable_dtPecentageCount
                beneficiaryTable = Me.Session_datatable_dtBenifAccount

                mainTable = New HtmlTable()
                mainTable.Attributes.Add("width", "98%") 'PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 680 to 98%
                mainTable.Rows.Add(CreateBeneficiaryTableRow("SSNo.", "Last Name", "First Name", "Fund status", True))
                For Each beneficiary In beneficiaryTable.Rows
                    beneficiaryPersID = beneficiary("id")
                    beneficiaryFundEventID = beneficiary("RecpFundEventId")
                    If (splitConfigurationTable.Select(String.Format("PersId='{0}'", beneficiaryPersID)).Length > 0) Then
                        mainTable.Rows.Add(CreateBeneficiaryTableRow(beneficiary("SSNo"), beneficiary("LastName"), beneficiary("FirstName"), "QD", False))
                        mainTable.Rows.Add(CreateBeneficiarySplitDetailsRow(beneficiaryPersID, beneficiaryFundEventID, splitConfigurationTable))
                    End If
                Next

                divBeneficiaryTable.Controls.Add(mainTable)
            End If
        Catch
            Throw
        Finally
            beneficiaryPersID = Nothing
            mainTableCell = Nothing
            mainTableRow = Nothing
            mainTable = Nothing
            beneficiaryTable = Nothing
            splitConfigurationTable = Nothing
        End Try
    End Sub

    Private Function CreateBeneficiaryTableRow(ByVal ssn As String, ByVal lastName As String, ByVal firstName As String, ByVal fundStatus As String, ByVal isAddCSSClass As Boolean) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell
        Try
            mainTableRow = New HtmlTableRow
            If isAddCSSClass Then
                mainTableRow.Attributes.Add("class", "DataGrid_HeaderStyle")
            End If

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("align", "center")
            mainTableCell.Attributes.Add("style", "width:25%") 'PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added width
            mainTableCell.InnerText = ssn
            mainTableRow.Cells.Add(mainTableCell)

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("align", "center")
            mainTableCell.Attributes.Add("style", "width:25%") 'PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added width
            mainTableCell.InnerText = lastName
            mainTableRow.Cells.Add(mainTableCell)

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("align", "center")
            mainTableCell.Attributes.Add("style", "width:25%") 'PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added width
            mainTableCell.InnerText = firstName
            mainTableRow.Cells.Add(mainTableCell)

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("align", "center")
            mainTableCell.Attributes.Add("style", "width:25%") 'PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added width
            mainTableCell.InnerText = fundStatus
            mainTableRow.Cells.Add(mainTableCell)

            Return mainTableRow
        Catch
            Throw
        Finally
            mainTableCell = Nothing
            mainTableRow = Nothing
        End Try

    End Function

    Private Function CreateBeneficiarySplitDetailsRow(ByVal beneficiaryPersID As String, ByVal beneficiaryFundEventID As String, ByVal splitConfigurationTable As DataTable) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim accountTable As System.Web.UI.HtmlControls.HtmlTable
        Dim accountRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim accountCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim rows As DataRow()

        Dim planType As String
        Dim amount As Decimal, percentage As Decimal

        Dim beneficiaryAccountsTable As DataTable

        Dim accountType As String, personalPreTax As String, personalPostTax As String, personalInterest As String, ymcaTaxable As String, ymcaInterest As String, total As String

        'START: PPP | 11/28/2016 | YRS-AT-3145 & 3265
        Dim feeDetails As DataTable
        Dim retirementFee, savingsFee As Decimal
        Dim feeDetailsText As String

        Dim isRetirementPlanSplitExists, isSavingsPlanSplitExists As Boolean
        'END: PPP | 11/28/2016 | YRS-AT-3145 & 3265
        Try
            'START: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Handling fee
            retirementFee = 0
            savingsFee = 0
            If HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees) Then
                feeDetails = Me.RecipientBalanceAfterSplitManageFees
                For Each feeRow As DataRow In feeDetails.Rows
                    If Convert.ToString(feeRow("PersId")) = beneficiaryPersID Then
                        retirementFee = Convert.ToDecimal(feeRow("RetirementFee"))
                        savingsFee = Convert.ToDecimal(feeRow("SavingsFee"))
                        isRetirementPlanSplitExists = Convert.ToBoolean(feeRow("IsRetirementPlanSplitExists"))
                        isSavingsPlanSplitExists = Convert.ToBoolean(feeRow("IsSavingsPlanSplitExists"))
                        Exit For
                    End If
                Next
            End If
            'END: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Handling fee

            accountTable = New HtmlTable()
            accountTable.Attributes.Add("class", "DataGrid_Grid")
            accountTable.Attributes.Add("cellspacing", "0")
            accountTable.Attributes.Add("rules", "all")
            accountTable.Attributes.Add("border", "1")
            accountTable.Attributes.Add("style", "width:100%;border-collapse:collapse;")

            'Add headers
            'START: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Changed heading
            'accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_HeaderStyle", "Acct", "EmpTaxable", "EmpNon-Taxable", "EmpInterest", "YMCATaxable", "YMCAInterest", "Acct Total "))
            accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_HeaderStyle", "Acct", "Taxable", "Non-Taxable", "Interest", "YMCA Taxable", "YMCA Interest", "Acct. Total "))
            'END: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Changed heading

            rows = splitConfigurationTable.Select(String.Format("PersId='{0}'", beneficiaryPersID))
            For Each row As DataRow In rows
                planType = Convert.ToString(row("PlanType"))
                amount = Convert.ToDecimal(row("Amount"))
                percentage = Convert.ToDecimal(row("Percentage"))

                accountCell = New HtmlTableCell()
                accountCell.Attributes.Add("colspan", "7")
                accountCell.Attributes.Add("style", "text-indent: 20px;")

                'START: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Conditional display of fee
                If isRetirementPlanSplitExists And isSavingsPlanSplitExists And planType.ToLower() = "both" Then
                    feeDetailsText = String.Format("Fees: Retirement (${0}); Savings (${1})", retirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture), savingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
                ElseIf isRetirementPlanSplitExists And planType.ToLower() = "retirement" Then
                    feeDetailsText = String.Format("Fees: Retirement (${0})", retirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
                ElseIf isSavingsPlanSplitExists And planType.ToLower() = "savings" Then
                    feeDetailsText = String.Format("Fees: Savings (${0})", savingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
                End If
                'END: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Conditional display of fee

                If amount > 0 Then
                    'START: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Added fee
                    'accountCell.InnerText = String.Format("Plan: {0} (Split ${1})", planType, amount.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture))
                    accountCell.InnerHtml = String.Format("Plan: {0} (Split ${1})&nbsp;&nbsp;&nbsp;&nbsp;{2}", planType, amount.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture), feeDetailsText)
                    'END: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Added fee
                Else
                    'START: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Added fee
                    'accountCell.InnerText = String.Format("Plan: {0} (Split {1}%)", planType, percentage.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture))
                    accountCell.InnerHtml = String.Format("Plan: {0} (Split {1}%)&nbsp;&nbsp;&nbsp;&nbsp;{2}", planType, percentage.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture), feeDetailsText)
                    'END: PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Added fee
                End If

                accountRow = New HtmlTableRow()
                accountRow.Attributes.Add("class", "DataGrid_HeaderStyle")
                accountRow.Cells.Add(accountCell)
                accountTable.Rows.Add(accountRow)

                beneficiaryAccountsTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareBeneficiaryAfterSplitAccountTable(beneficiaryFundEventID, planType, Me.Session_dataset_dsAllRecipantAccountsDetail, Me.Session_Dataset_PartAccountDetail)
                If HelperFunctions.isNonEmpty(beneficiaryAccountsTable) Then
                    For rowCounter As Integer = 0 To beneficiaryAccountsTable.Rows.Count - 1
                        accountType = beneficiaryAccountsTable.Rows(rowCounter)("AcctType")
                        personalPreTax = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("PersonalPreTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        personalPostTax = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("PersonalPostTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        personalInterest = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("PersonalInterestBalance")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        ymcaTaxable = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("YMCAPreTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        ymcaInterest = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("YMCAInterestBalance")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                        total = Convert.ToDecimal(beneficiaryAccountsTable.Rows(rowCounter)("TotalTotal")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)

                        If (rowCounter Mod 2) = 0 Then
                            accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_NormalStyle", accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total))
                        Else
                            accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_AlternateStyle", accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total))
                        End If
                    Next
                End If
            Next

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("colspan", "4")
            mainTableCell.Controls.Add(accountTable)

            mainTableRow = New HtmlTableRow
            mainTableRow.Cells.Add(mainTableCell)

            Return mainTableRow
        Catch
            Throw
        Finally
            feeDetailsText = Nothing 'PPP | 11/28/2016 | YRS-AT-3145 & 3265 
            feeDetails = Nothing 'PPP | 11/28/2016 | YRS-AT-3145 & 3265 
            accountType = Nothing
            personalPreTax = Nothing
            personalPostTax = Nothing
            personalInterest = Nothing
            ymcaTaxable = Nothing
            ymcaInterest = Nothing
            total = Nothing
            beneficiaryAccountsTable = Nothing
            planType = Nothing
            rows = Nothing
            accountCell = Nothing
            accountRow = Nothing
            accountTable = Nothing
            mainTableCell = Nothing
            mainTableRow = Nothing
        End Try
    End Function

    Private Function CreateBeneficiarySplitRow(ByVal cssClass As String, ByVal accountType As String, ByVal personalPreTax As String, ByVal personalPostTax As String, ByVal personalInterest As String, ByVal ymcaTaxable As String, ByVal ymcaInterest As String, ByVal total As String) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim accountRow As System.Web.UI.HtmlControls.HtmlTableRow
        'Dim accountCell As System.Web.UI.HtmlControls.HtmlTableCell 'PPP | 01/04/2017 | YRS-AT-3145 & 3265
        Try
            accountRow = New HtmlTableRow
            accountRow.Attributes.Add("class", cssClass)

            'START: PPP | 01/04/2017 | YRS-AT-3145 & 3265 | Cell creation is ported out to method, so that it can be controled from one location
            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = accountType
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(accountType))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = personalPreTax
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(personalPreTax))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = personalPostTax
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(personalPostTax))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = personalInterest
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(personalInterest))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = ymcaTaxable
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(ymcaTaxable))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = ymcaInterest
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(ymcaInterest))

            'accountCell = New HtmlTableCell
            'accountCell.Attributes.Add("align", "center")
            'accountCell.InnerText = total
            'accountRow.Cells.Add(accountCell)
            accountRow.Cells.Add(CreateBeneficirySplitCell(total))
            'END: PPP | 01/04/2017 | YRS-AT-3145 & 3265 | Cell creation is ported out to method, so that it can be controled from one location

            Return accountRow
        Catch
            Throw
        Finally
            'accountCell = Nothing 'PPP | 01/04/2017 | YRS-AT-3145 & 3265
            accountRow = Nothing
        End Try
    End Function

    'START: PPP | 01/04/2017 | YRS-AT-3145 & 3265 | Method creates html table cell (<td>)
    Public Function CreateBeneficirySplitCell(ByVal data As String) As HtmlTableCell
        Dim accountCell As New System.Web.UI.HtmlControls.HtmlTableCell
        accountCell.Attributes.Add("align", "right")
        accountCell.InnerText = data
        Return accountCell
    End Function
    'END: PPP | 01/04/2017 | YRS-AT-3145 & 3265 | Method creates html table cell (<td>)

    Private Function GetSelectedAccountsTotal() As Decimal
        Dim beneficiaryItems As DataGridItem
        Dim accountCheckBox As CheckBox
        Dim finalTotal As Decimal = 0

        dtPartAccount = Me.Session_datatable_dtPartAccount
        For rowCounter = 0 To dtPartAccount.Rows.Count - 1
            beneficiaryItems = DataGridWorkSheets.Items(rowCounter)
            accountCheckBox = CType(beneficiaryItems.Cells(0).Controls(1), CheckBox)
            If accountCheckBox.Checked = True Then
                finalTotal += Math.Round(Convert.ToDecimal(dtPartAccount.Rows(rowCounter).Item("TotalTotal")), 2)
            End If
        Next

        Return finalTotal
    End Function

    Private Sub EnableSplitButtonSet(ByVal isEnabled As Boolean)
        ButtonSplit.Enabled = isEnabled
        ButtonAdjust.Enabled = isEnabled
        ButtonReset.Enabled = isEnabled
        btnShowBalance.Enabled = isEnabled
        ButtonDocumentSave.Enabled = isEnabled
        EnableDisableNextButton(isEnabled) 'MMR | 2016.11.23 | YRS-AT-3145 | Enabling Next button after split
    End Sub

    ' HandleDifferenceIssue will resolve difference of 0.0X 
    Private Sub HandleDifferenceIssue()
        Dim recipientPersID, recipientFundEventID As String
        Dim planType, accountType As String
        Dim asOnDateBalanceTable As DataTable
        Dim splitConfigurationRows(), recipientBalanceRows() As DataRow
        Dim recipientAfterSplitValuesTable As DataTable

        Dim splitAmount, splitPercentage As Decimal
        Dim selectedAccountTotal As Decimal
        Dim expectedBalance, recipientSplitedTotal, differenceAmount As Decimal

        'START: PPP | 09/30/2016 | YRS-AT-2529 | Added to resolve decimal difference found in total of A and B
        Dim recipientAfterSplitAValuesTable, recipientAfterSplitBValuesTable, mergedABGroup As DataTable, differencesTable As DataTable
        Dim participantATransaction, participantBTransaction, recipientATransaction, recipientBTransaction As DataTable
        'END: PPP | 09/30/2016 | YRS-AT-2529 | Added to resolve decimal difference found in total of A and B
        Try
            planType = hdnSelectedPlanType.Value
            recipientPersID = Me.String_Benif_PersonD
            recipientFundEventID = Me.string_RecptFundEventID
            dtPecentageCount = Me.Session_datatable_dtPecentageCount

            ' Find the split configuration
            splitConfigurationRows = dtPecentageCount.Select(String.Format("PersId='{0}' and PlanType='{1}'", recipientPersID, planType))
            If splitConfigurationRows.Length > 0 Then
                splitAmount = Convert.ToDecimal(splitConfigurationRows(0)("Amount"))
                splitPercentage = Convert.ToDecimal(splitConfigurationRows(0)("Percentage"))

                ' Prepare recipient after split account table
                l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail.Copy ' Original balance 'PPP | 09/30/2016 | YRS-AT-2529 | Added ".Copy" 
                dsAllRecipantAccountsDetail = Me.Session_dataset_dsAllRecipantAccountsDetail.Copy 'PPP | 09/30/2016 | YRS-AT-2529 | Added ".Copy" 
                recipientAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareBeneficiaryAfterSplitAccountTable(recipientFundEventID, planType, dsAllRecipantAccountsDetail, l_dataset_PartAccountDetail)

                asOnDateBalanceTable = FetchAsOnDateAccountRows(planType, recipientFundEventID, l_dataset_PartAccountDetail)

                If Not recipientAfterSplitValuesTable Is Nothing AndAlso Not asOnDateBalanceTable Is Nothing Then
                    selectedAccountTotal = 0
                    For Each balanceRow As DataRow In asOnDateBalanceTable.Rows
                        selectedAccountTotal += Convert.ToDecimal(balanceRow("mnyBalance"))
                    Next
                    If splitAmount > 0 Then
                        splitPercentage = splitAmount / selectedAccountTotal
                    Else
                        splitPercentage = splitPercentage / 100
                        splitAmount = Math.Round(selectedAccountTotal * splitPercentage, 2, MidpointRounding.AwayFromZero)
                    End If

                    asOnDateBalanceTable = CalculateExpectedBalanceTable(asOnDateBalanceTable, splitAmount, splitPercentage)
                    For Each balanceRow As DataRow In asOnDateBalanceTable.Rows
                        accountType = Convert.ToString(balanceRow("AcctType"))
                        expectedBalance = Convert.ToDecimal(balanceRow("ExpectedBalance"))

                        recipientBalanceRows = recipientAfterSplitValuesTable.Select(String.Format("AcctType='{0}'", accountType))
                        If recipientBalanceRows.Length > 0 Then
                            recipientSplitedTotal = Convert.ToDecimal(recipientBalanceRows(0)("TotalTotal"))
                            If expectedBalance <> recipientSplitedTotal Then
                                differenceAmount = expectedBalance - recipientSplitedTotal
                                PushDifferenceAmountInSplitedDetails(accountType, recipientPersID, differenceAmount)
                            End If
                        End If
                    Next

                    'START: PPP | 09/30/2016 | YRS-AT-2529 | Resolving A and B decimal issues
                    'At this stage A and B banalances have also been updated for differences detected at above code
                    ' But still need to check A and B for differences
                    dsAllRecipantAccountsDetail = Me.Session_dataset_dsAllRecipantAccountsDetail.Copy
                    recipientAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareBeneficiaryAfterSplitAccountTable(recipientFundEventID, planType, dsAllRecipantAccountsDetail, l_dataset_PartAccountDetail)

                    dsALLGroupARecipantDetails = Me.Session_dataset_dsALLGroupARecipantDetails.Copy
                    dsALLGroupBRecipantDetails = Me.Session_dataset_dsALLGroupBRecipantDetails.Copy

                    recipientAfterSplitAValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareBeneficiaryAfterSplitAccountTable(recipientFundEventID, planType, dsALLGroupARecipantDetails, l_dataset_PartAccountDetail)
                    recipientAfterSplitBValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareBeneficiaryAfterSplitAccountTable(recipientFundEventID, planType, dsALLGroupBRecipantDetails, l_dataset_PartAccountDetail)
                    mergedABGroup = GetABMergedTable(recipientAfterSplitAValuesTable, recipientAfterSplitBValuesTable)

                    'START: Applying here adjustment logic to correct A and B amounts
                    differencesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.GetAdjustedAmountTable(mergedABGroup, recipientAfterSplitValuesTable)
                    ' Retrieve data from session
                    participantATransaction = Me.Session_dataset_dsALLGroupAParticipantDetails.Tables(0)
                    participantBTransaction = Me.Session_dataset_dsALLGroupBParticipantDetails.Tables(0)
                    recipientATransaction = Me.Session_dataset_dsALLGroupARecipantDetails.Tables(0)
                    recipientBTransaction = Me.Session_dataset_dsALLGroupBRecipantDetails.Tables(0)

                    ' Adjust participant's A and B table
                    YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.AdjustAccountBalances(True, participantATransaction, participantBTransaction, differencesTable, recipientPersID, "QWPR", "QWIN")

                    ' Adjust recipient's A and B table
                    YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.AdjustAccountBalances(False, recipientATransaction, recipientBTransaction, differencesTable, recipientPersID, "QSPR", "QSIN")

                    ' Assign it back to session
                    Me.Session_dataset_dsALLGroupAParticipantDetails = AssignAdjustedTableBackToSession(Me.Session_dataset_dsALLGroupAParticipantDetails, participantATransaction)
                    Me.Session_dataset_dsALLGroupBParticipantDetails = AssignAdjustedTableBackToSession(Me.Session_dataset_dsALLGroupBParticipantDetails, participantBTransaction)
                    Me.Session_dataset_dsALLGroupARecipantDetails = AssignAdjustedTableBackToSession(Me.Session_dataset_dsALLGroupARecipantDetails, recipientATransaction)
                    Me.Session_dataset_dsALLGroupBRecipantDetails = AssignAdjustedTableBackToSession(Me.Session_dataset_dsALLGroupBRecipantDetails, recipientBTransaction)
                    'END: Applying here adjustment logic to correct A and B amounts
                    'END: PPP | 09/30/2016 | YRS-AT-2529 | Resolving A and B decimal issues
                End If
            End If
        Catch
            Throw
        Finally
            'START: PPP | 09/30/2016 | YRS-AT-2529
            differencesTable = Nothing
            participantATransaction = Nothing
            participantBTransaction = Nothing
            recipientATransaction = Nothing
            recipientBTransaction = Nothing

            recipientAfterSplitAValuesTable = Nothing
            recipientAfterSplitBValuesTable = Nothing
            mergedABGroup = Nothing
            'END: PPP | 09/30/2016 | YRS-AT-2529
            recipientFundEventID = Nothing
            recipientPersID = Nothing
            accountType = Nothing
            planType = Nothing
            recipientBalanceRows = Nothing
            splitConfigurationRows = Nothing
            recipientAfterSplitValuesTable = Nothing
            asOnDateBalanceTable = Nothing
        End Try
    End Sub

    ' FetchAsOnDateAccountRows provides balances of rows loaded and selected on screen 
    Private Function FetchAsOnDateAccountRows(ByVal planType As String, ByVal recipientFundEventID As String, ByVal originalParticipantBalance As DataSet) As DataTable
        Dim asOnDateBalanceTable As DataTable
        Dim rows As DataRow()
        Dim distinctSplitedAccountTypes As List(Of String)
        Try
            asOnDateBalanceTable = Nothing
            If Not originalParticipantBalance Is Nothing AndAlso Not originalParticipantBalance.Tables Is Nothing AndAlso originalParticipantBalance.Tables.Count > 0 Then
                asOnDateBalanceTable = originalParticipantBalance.Tables(0).Clone

                distinctSplitedAccountTypes = GetDistinctSplitedAccountTypes(planType, recipientFundEventID)
                If distinctSplitedAccountTypes.Count > 0 Then
                    For Each accountType As String In distinctSplitedAccountTypes
                        rows = originalParticipantBalance.Tables(0).Select(String.Format("AcctType='{0}'", accountType))
                        If rows.Length > 0 Then
                            asOnDateBalanceTable.ImportRow(rows(0))
                        End If
                    Next
                End If

            End If
            Return asOnDateBalanceTable
        Catch
            Throw
        Finally
            distinctSplitedAccountTypes = Nothing
            rows = Nothing
            asOnDateBalanceTable = Nothing
        End Try
    End Function

    ' CalculateExpectedBalanceTable provides expected after split total of each account types 
    Private Function CalculateExpectedBalanceTable(ByVal asOnDateBalanceTable As DataTable, ByVal splitAmount As Decimal, ByVal splitPercentage As Decimal) As DataTable
        Dim total, finalTotal, difference As Decimal
        finalTotal = 0
        asOnDateBalanceTable.Columns.Add("ExpectedBalance", GetType(System.Decimal))
        For Each row As DataRow In asOnDateBalanceTable.Rows
            total = Math.Round(Convert.ToDecimal(row("mnyBalance")) * splitPercentage, 2, MidpointRounding.AwayFromZero)
            row("ExpectedBalance") = total
            finalTotal += total
        Next
        If splitAmount > finalTotal Then
            difference = splitAmount - finalTotal
            asOnDateBalanceTable.Rows(0)("ExpectedBalance") = Convert.ToDecimal(asOnDateBalanceTable.Rows(0)("ExpectedBalance")) + difference
        End If
        asOnDateBalanceTable.AcceptChanges()
        Return asOnDateBalanceTable
    End Function

    Private Sub PushDifferenceAmountInSplitedDetails(ByVal accountType As String, ByVal recipientPersID As String, ByVal differenceAmount As String)
        Dim participantRows(), recipientRows() As DataRow
        Dim copyOfDifferenceAmount As Decimal

        ' Adjust in main tables
        dsAllPartAccountsDetail = Me.Session_dataset_dsAllPartAccountsDetail
        dsAllRecipantAccountsDetail = Me.Session_dataset_dsAllRecipantAccountsDetail

        participantRows = dsAllPartAccountsDetail.Tables(0).Select(String.Format("BenfitPersId='{0}' and AcctType='{1}'", recipientPersID, accountType))
        recipientRows = dsAllRecipantAccountsDetail.Tables(0).Select(String.Format("PersID='{0}' and AcctType='{1}'", recipientPersID, accountType))
        copyOfDifferenceAmount = differenceAmount
        ChangeRows(participantRows, recipientRows, copyOfDifferenceAmount)

        dsAllPartAccountsDetail.AcceptChanges()
        dsAllRecipantAccountsDetail.AcceptChanges()
        Me.Session_dataset_dsAllPartAccountsDetail = dsAllPartAccountsDetail
        Me.Session_dataset_dsAllRecipantAccountsDetail = dsAllRecipantAccountsDetail

        ' A table 
        dsALLGroupAParticipantDetails = Me.Session_dataset_dsALLGroupAParticipantDetails
        dsALLGroupARecipantDetails = Me.Session_dataset_dsALLGroupARecipantDetails

        participantRows = dsALLGroupAParticipantDetails.Tables(0).Select(String.Format("BenfitPersId='{0}' and AcctType='{1}'", recipientPersID, accountType))
        recipientRows = dsALLGroupARecipantDetails.Tables(0).Select(String.Format("PersID='{0}' and AcctType='{1}'", recipientPersID, accountType))
        copyOfDifferenceAmount = differenceAmount
        ChangeRows(participantRows, recipientRows, copyOfDifferenceAmount)

        dsALLGroupAParticipantDetails.AcceptChanges()
        dsALLGroupARecipantDetails.AcceptChanges()

        Me.Session_dataset_dsALLGroupAParticipantDetails = dsALLGroupAParticipantDetails
        Me.Session_dataset_dsALLGroupARecipantDetails = dsALLGroupARecipantDetails

        ' B table 
        If copyOfDifferenceAmount <> 0 Then
            dsALLGroupBParticipantDetails = Me.Session_dataset_dsALLGroupBParticipantDetails
            dsALLGroupBRecipantDetails = Me.Session_dataset_dsALLGroupBRecipantDetails

            participantRows = dsALLGroupBParticipantDetails.Tables(0).Select(String.Format("BenfitPersId='{0}' and AcctType='{1}'", recipientPersID, accountType))
            recipientRows = dsALLGroupBRecipantDetails.Tables(0).Select(String.Format("PersID='{0}' and AcctType='{1}'", recipientPersID, accountType))
            ChangeRows(participantRows, recipientRows, copyOfDifferenceAmount)

            dsALLGroupBParticipantDetails.AcceptChanges()
            dsALLGroupBRecipantDetails.AcceptChanges()

            Me.Session_dataset_dsALLGroupBParticipantDetails = dsALLGroupBParticipantDetails
            Me.Session_dataset_dsALLGroupBRecipantDetails = dsALLGroupBRecipantDetails
        End If
    End Sub

    Private Sub ChangeRows(ByRef participantRows() As DataRow, ByRef recipientRows() As DataRow, ByRef differenceAmount As Decimal)
        Dim amount As Decimal
        For counter As Integer = 0 To recipientRows.Length
            amount = Convert.ToDecimal(recipientRows(counter)("PersonalPreTax"))
            If amount > 0 Then
                If (amount + differenceAmount) > 0 Then
                    recipientRows(counter)("PersonalPreTax") = amount + differenceAmount
                    participantRows(counter)("PersonalPreTax") = Convert.ToDecimal(participantRows(counter)("PersonalPreTax")) - differenceAmount
                    differenceAmount = 0
                    Exit For
                End If
            End If

            amount = Convert.ToDecimal(recipientRows(counter)("PersonalPostTax"))
            If amount > 0 Then
                If (amount + differenceAmount) > 0 Then
                    recipientRows(counter)("PersonalPostTax") = amount + differenceAmount
                    participantRows(counter)("PersonalPostTax") = Convert.ToDecimal(participantRows(counter)("PersonalPostTax")) - differenceAmount
                    differenceAmount = 0
                    Exit For
                End If
            End If

            amount = Convert.ToDecimal(recipientRows(counter)("YmcaPreTax"))
            If amount > 0 Then
                If (amount + differenceAmount) > 0 Then
                    recipientRows(counter)("YmcaPreTax") = amount + differenceAmount
                    participantRows(counter)("YmcaPreTax") = Convert.ToDecimal(participantRows(counter)("YmcaPreTax")) - differenceAmount
                    differenceAmount = 0
                    Exit For
                End If
            End If
        Next
    End Sub
    'END: PPP | 08/24/2016 | YRS-AT-2529 

    'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488| Commented existing code and added method for enabling and disabling spouse control 
    'Start -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Enabling and diabling martial status and gender.
    'Private Sub EnableDisableGenderMaritalControl(ByVal bnFlag As Boolean)
    '    DropDownListGender.Enabled = bnFlag
    '    DropDownListMaritalStatus.Enabled = bnFlag
    'End Sub
    'End -Manthan Rajguru | 2016.08.16 | YRS-AT-2482| Enabling and diabling martial status and gender.

    Private Sub ManageEditableControls(ByVal bnFlag As Boolean)
        DropDownListGender.Enabled = bnFlag
        DropDownListMaritalStatus.Enabled = bnFlag
        chkSpouse.Enabled = bnFlag
    End Sub
    'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488| Commented existing code and added method for enabling and disabling spouse control

    'Start -Manthan Rajguru | 2016.08.26 | YRS-AT-2488| Added to validate for more than one spouse beneficiary
    Public Function CheckForSpouseRecipient() As Boolean
        Dim dtRecipientDetails As DataTable
        Dim blnIsRecipientSpouse As Boolean
        Dim selectedBeneficiaryID As String
        Try
            blnIsRecipientSpouse = False
            'START: MMR | 2016.09.12 | YRS-AT-2488 | Added to store selected beneficiary ID from Grid
            If DatagridBenificiaryList.Items.Count > 0 Then
                If Not DatagridBenificiaryList.SelectedIndex = -1 Then
                    selectedBeneficiaryID = DatagridBenificiaryList.SelectedItem.Cells(recipientIndexPersID).Text 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Replaced BENEFICIARY_ID with recipientIndexPersID 
                End If
            End If
            'END: MMR | 2016.09.12 | YRS-AT-2488 | Added to store selected beneficiary ID from Grid
            If Not Me.Session_datatable_dtBenifAccount Is Nothing Then
                dtRecipientDetails = DirectCast(Session_datatable_dtBenifAccount, DataTable)
                For intCounter As Integer = 0 To dtRecipientDetails.Rows.Count - 1
                    If Convert.ToBoolean(dtRecipientDetails.Rows(intCounter)("bitRecipientSpouse")) Then
                        'START: MMR | 2016.09.12 | YRS-AT-2488 | Added to check if selected beneficiary is already defined as spouse on edit
                        If ButtonAddBeneficiaryToList.Text = "Update Recipient" And selectedBeneficiaryID = dtRecipientDetails.Rows(intCounter)("id") Then 'PPP | 11/28/2016 | YRS-AT-3145 & 3265 | Replaced "Update To List"  by "Update Recipient"
                            ViewState("IsAlreadySpouse") = True
                        End If
                        'END: MMR | 2016.09.12 | YRS-AT-2488 | Added to check if selected beneficiary is already defined as spouse on edit
                        blnIsRecipientSpouse = True
                        Exit For
                    End If
                Next
            End If
            Return blnIsRecipientSpouse
        Catch
            Throw
        End Try
    End Function
    'End -Manthan Rajguru | 2016.08.26 | YRS-AT-2488| Added to validate for more than one spouse beneficiary

    'START: PPP | 09/13/2016 | YRS-AT-1973 | Procedures and functions to handle "Adjust" functionality
    Private Sub Adjust()
        Dim recipientSplitedValuesTable, participantAfterSplitValuesTable As DataTable
        Dim recipientAdjustedValuesTable As DataTable

        Dim isAdjustmentValid As Boolean
        Dim adjustedValuesTable As DataTable
        Dim recipientPersID As String

        Dim participantTransaction As DataTable, participantATransaction As DataTable, participantBTransaction As DataTable
        Dim recipientTransaction As DataTable, recipientATransaction As DataTable, recipientBTransaction As DataTable
        Try
            recipientPersID = Me.String_Benif_PersonD
            l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail.Copy ' Original balance

            ' Prepare current splited recipient table
            dsAllRecipantAccountsDetail = Me.Session_dataset_dsAllRecipantAccountsDetail
            recipientSplitedValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareBeneficiaryAfterSplitAccountTable(Me.string_RecptFundEventID, hdnSelectedPlanType.Value, dsAllRecipantAccountsDetail, l_dataset_PartAccountDetail)
            recipientAdjustedValuesTable = recipientSplitedValuesTable.Clone

            ' Collect adjusted screen balances
            recipientAdjustedValuesTable = GetNewAdjustedValuesTable(recipientAdjustedValuesTable)

            isAdjustmentValid = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.IsAdjustmentValid(recipientSplitedValuesTable, recipientAdjustedValuesTable)
            If isAdjustmentValid Then
                isAdjustmentValid = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.IsAdjustmentDoneAgainstPositiveValue(l_dataset_PartAccountDetail, recipientAdjustedValuesTable)
            End If

            ' Check for negative values after adjustment
            If isAdjustmentValid Then
                adjustedValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.GetAdjustedAmountTable(recipientSplitedValuesTable, recipientAdjustedValuesTable)

                ' Prepare participant table
                dsAllPartAccountsDetail = Me.Session_dataset_dsAllPartAccountsDetail
                participantAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable("Both", dsAllPartAccountsDetail, l_dataset_PartAccountDetail)

                isAdjustmentValid = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.IsAdjustmentLeavingPositiveValueInParticipantAccount(participantAfterSplitValuesTable, adjustedValuesTable)
            End If

            If Not isAdjustmentValid Then
                'START: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
                'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QDRO_SUM_OF_AMOUNTS_GIVEN_NOTEQUAL_TOBE_SPLIT"), MessageBoxButtons.Stop, False)
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_SUM_OF_AMOUNTS_GIVEN_NOTEQUAL_TOBE_SPLIT", "error")
                'END: PPP | 12/28/2016 | YRS-AT-3145 & 3265 | Moving messages to JQuery modal popup
            Else
                adjustedValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.GetAdjustedAmountTable(recipientSplitedValuesTable, recipientAdjustedValuesTable)

                ' Retrieve data from session
                participantTransaction = Me.Session_dataset_dsAllPartAccountsDetail.Tables(0)
                participantATransaction = Me.Session_dataset_dsALLGroupAParticipantDetails.Tables(0)
                participantBTransaction = Me.Session_dataset_dsALLGroupBParticipantDetails.Tables(0)
                recipientTransaction = Me.Session_dataset_dsAllRecipantAccountsDetail.Tables(0)
                recipientATransaction = Me.Session_dataset_dsALLGroupARecipantDetails.Tables(0)
                recipientBTransaction = Me.Session_dataset_dsALLGroupBRecipantDetails.Tables(0)

                ' Adjust participant table
                YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.AdjustAccountBalances(True, participantTransaction, adjustedValuesTable, recipientPersID, "QWPR", "QWIN")

                ' Adjust recipient table
                YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.AdjustAccountBalances(False, recipientTransaction, adjustedValuesTable, recipientPersID, "QSPR", "QSIN")

                ' Adjust participant's A and B table
                YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.AdjustAccountBalances(True, participantATransaction, participantBTransaction, adjustedValuesTable, recipientPersID, "QWPR", "QWIN")

                ' Adjust recipient's A and B table
                YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.AdjustAccountBalances(False, recipientATransaction, recipientBTransaction, adjustedValuesTable, recipientPersID, "QSPR", "QSIN")

                ' Assign it back to session
                Me.Session_dataset_dsAllPartAccountsDetail = AssignAdjustedTableBackToSession(Me.Session_dataset_dsAllPartAccountsDetail, participantTransaction)
                Me.Session_dataset_dsALLGroupAParticipantDetails = AssignAdjustedTableBackToSession(Me.Session_dataset_dsALLGroupAParticipantDetails, participantATransaction)
                Me.Session_dataset_dsALLGroupBParticipantDetails = AssignAdjustedTableBackToSession(Me.Session_dataset_dsALLGroupBParticipantDetails, participantBTransaction)
                Me.Session_dataset_dsAllRecipantAccountsDetail = AssignAdjustedTableBackToSession(Me.Session_dataset_dsAllRecipantAccountsDetail, recipientTransaction)
                Me.Session_dataset_dsALLGroupARecipantDetails = AssignAdjustedTableBackToSession(Me.Session_dataset_dsALLGroupARecipantDetails, recipientATransaction)
                Me.Session_dataset_dsALLGroupBRecipantDetails = AssignAdjustedTableBackToSession(Me.Session_dataset_dsALLGroupBRecipantDetails, recipientBTransaction)

                ShowHideControls()

                'START: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After adjusting split details, defined fee must be reset
                If chkApplyFees.Checked And (HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) AndAlso HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees)) Then
                    ResetFees()
                End If
                'END: PPP | 01/02/2017 | YRS-AT-3145 & 3265 | After adjusting split details, defined fee must be reset
            End If
        Catch
            Throw
        Finally
            recipientBTransaction = Nothing
            recipientATransaction = Nothing
            recipientTransaction = Nothing

            participantBTransaction = Nothing
            participantATransaction = Nothing
            participantTransaction = Nothing

            recipientPersID = Nothing
            adjustedValuesTable = Nothing

            recipientAdjustedValuesTable = Nothing
            recipientSplitedValuesTable = Nothing
            participantAfterSplitValuesTable = Nothing
        End Try
    End Sub

    ' Provides datatable which will hold screen values
    Private Function GetNewAdjustedValuesTable(ByVal recipientAdjustedValuesTable As DataTable) As DataTable
        Dim accountTypeIndex As Integer, accountTotalIndex As Integer
        Dim personPreTaxIndex As Integer, personPostTaxIndex As Integer, personInterestIndex As Integer
        Dim ymcaPreTaxIndex As Integer, ymcaInterestIndex As Integer
        Dim totalRow As DataRow, accountRow As DataRow
        Try
            ' Collect adjusted screen balances
            accountTypeIndex = 0
            personPreTaxIndex = 1
            personPostTaxIndex = 2
            personInterestIndex = 3
            ymcaPreTaxIndex = 5
            ymcaInterestIndex = 6
            accountTotalIndex = 8
            For Each gridRow As DataGridItem In DataGridWorkSheet2.Items
                accountRow = recipientAdjustedValuesTable.NewRow
                accountRow("AcctType") = gridRow.Cells(accountTypeIndex).Text.Trim
                accountRow("PersonalPreTax") = GetGridTextBoxValue(gridRow, personPreTaxIndex)
                accountRow("PersonalPostTax") = GetGridTextBoxValue(gridRow, personPostTaxIndex)
                accountRow("PersonalInterestBalance") = GetGridTextBoxValue(gridRow, personInterestIndex)
                accountRow("YMCAPreTax") = GetGridTextBoxValue(gridRow, ymcaPreTaxIndex)
                accountRow("YMCAInterestBalance") = GetGridTextBoxValue(gridRow, ymcaInterestIndex)
                accountRow("TotalTotal") = GetGridTextBoxValue(gridRow, accountTotalIndex)
                recipientAdjustedValuesTable.Rows.Add(accountRow)
            Next
            Return recipientAdjustedValuesTable
        Catch
            Throw
        Finally
            totalRow = Nothing
            accountRow = Nothing
        End Try
    End Function

    Private Function GetGridTextBoxValue(ByVal gridRow As DataGridItem, ByVal columnIndex As Integer) As Decimal
        Dim amount As Decimal = 0
        Dim amountTextBox As TextBox = CType(gridRow.Cells(columnIndex).Controls(1), TextBox)
        If Not amountTextBox Is Nothing Then
            amount = Math.Round(Convert.ToDecimal(amountTextBox.Text.Trim()), 2, MidpointRounding.AwayFromZero)
        End If
        Return amount
    End Function

    ' Provides total of row whose cell got edited by User
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetUIAdjustedRowTotal(ByVal empNonTaxable As String, ByVal empTaxable As String, ByVal empInterest As String, ByVal ymcaNonTaxable As String, ByVal ymcaInterest As String) As String
        Dim empNonTaxableValue As Decimal, empTaxableValue As Decimal, empInterestValue As Decimal, ymcaNonTaxableValue As Decimal, ymcaInterestValue As Decimal
        Dim total As Decimal
        Try
            empNonTaxableValue = Convert.ToDecimal(empNonTaxable)
            empTaxableValue = Convert.ToDecimal(empTaxable)
            empInterestValue = Convert.ToDecimal(empInterest)
            ymcaNonTaxableValue = Convert.ToDecimal(ymcaNonTaxable)
            ymcaInterestValue = Convert.ToDecimal(ymcaInterest)

            total = Math.Round(empNonTaxableValue + empTaxableValue + empInterestValue + ymcaNonTaxableValue + ymcaInterestValue, 2, MidpointRounding.AwayFromZero)

            Return total.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
        Catch ex As Exception
            HelperFunctions.LogException("GetUIAdjustedRowTotal", ex)
        End Try
    End Function

    Private Function AssignAdjustedTableBackToSession(ByVal sessionDataSet As DataSet, ByVal adjustedTable As DataTable) As DataSet
        sessionDataSet.Tables.Clear()
        sessionDataSet.Tables.Add(adjustedTable)
        Return sessionDataSet
    End Function
    'END: PPP | 09/13/2016 | YRS-AT-1973 | Procedures and functions to handle "Adjust" functionality

    'START: PPP | 09/28/2016 | YRS-AT-2529 
    ' It will determine which Plan has been splited till now, if Ret. is splited in between all ben. then it will return only Retirement, if Ret and Sav is splited among Ben.s then Both will be returned
    Private Function GetParticipantPlanToShowOnSummary() As String
        Dim planType, definedPlanType As String
        Dim isSavingsPlanExists, isRetirementPlanExists As Boolean
        Try
            planType = "Both"
            isSavingsPlanExists = False
            isRetirementPlanExists = False
            dtPecentageCount = Me.Session_datatable_dtPecentageCount
            If Not dtPecentageCount Is Nothing Then
                For Each row As DataRow In dtPecentageCount.Rows
                    definedPlanType = Convert.ToString(row("PlanType")).ToUpper
                    If definedPlanType = "RETIREMENT" Then
                        isRetirementPlanExists = True
                    ElseIf definedPlanType = "SAVINGS" Then
                        isSavingsPlanExists = True
                    ElseIf definedPlanType = "BOTH" Then
                        isRetirementPlanExists = True
                        isSavingsPlanExists = True
                    End If
                Next
            End If

            If isRetirementPlanExists And Not isSavingsPlanExists Then
                planType = "Retirement"
            ElseIf Not isRetirementPlanExists And isSavingsPlanExists Then
                planType = "Savings"
            End If
            Return planType
        Catch
            Throw
        Finally
            definedPlanType = Nothing
            planType = Nothing
        End Try
    End Function

    ' It will merge A and B balance into one table
    Private Function GetABMergedTable(ByVal groupATable As DataTable, ByVal groupBTable As DataTable) As DataTable
        Dim mergedTable As DataTable
        Dim mergedRows As DataRow()
        Dim accountType As String
        Dim personPreTax, personPostTax, personInterest, ymcaPreTax, ymcaInterest As Decimal
        Try
            mergedTable = groupATable.Copy
            For Each row As DataRow In groupBTable.Rows
                accountType = Convert.ToString(row("AcctType"))
                mergedRows = mergedTable.Select(String.Format("AcctType='{0}'", accountType))
                If Not mergedRows Is Nothing AndAlso mergedRows.Length > 0 Then
                    personPreTax = Convert.ToDecimal(row("PersonalPreTax"))
                    personPostTax = Convert.ToDecimal(row("PersonalPostTax"))
                    ymcaPreTax = Convert.ToDecimal(row("YmcaPreTax"))
                    personInterest = Convert.ToDecimal(row("PersonalInterestBalance"))
                    ymcaInterest = Convert.ToDecimal(row("YMCAInterestBalance"))

                    mergedRows(0)("PersonalPreTax") = Math.Round(Convert.ToDecimal(mergedRows(0)("PersonalPreTax")) + personPreTax, 2, MidpointRounding.AwayFromZero)
                    mergedRows(0)("PersonalPostTax") = Math.Round(Convert.ToDecimal(mergedRows(0)("PersonalPostTax")) + personPostTax, 2, MidpointRounding.AwayFromZero)
                    mergedRows(0)("PersonalInterestBalance") = Math.Round(Convert.ToDecimal(mergedRows(0)("PersonalInterestBalance")) + personInterest, 2, MidpointRounding.AwayFromZero)
                    mergedRows(0)("PersonalTotal") = Math.Round(Convert.ToDecimal(mergedRows(0)("PersonalTotal")) + personPreTax + personPostTax + personInterest, 2, MidpointRounding.AwayFromZero)
                    mergedRows(0)("YMCAPreTax") = Math.Round(Convert.ToDecimal(mergedRows(0)("YMCAPreTax")) + ymcaPreTax, 2, MidpointRounding.AwayFromZero)
                    mergedRows(0)("YMCAInterestBalance") = Math.Round(Convert.ToDecimal(mergedRows(0)("YMCAInterestBalance")) + ymcaInterest, 2, MidpointRounding.AwayFromZero)
                    mergedRows(0)("YMCATotal") = Math.Round(Convert.ToDecimal(mergedRows(0)("YMCATotal")) + ymcaPreTax + ymcaInterest, 2, MidpointRounding.AwayFromZero)
                    mergedRows(0)("TotalTotal") = Math.Round(Convert.ToDecimal(mergedRows(0)("TotalTotal")) + personPreTax + personPostTax + personInterest + ymcaPreTax + ymcaInterest, 2, MidpointRounding.AwayFromZero)
                    mergedTable.AcceptChanges()
                End If
            Next
            Return mergedTable
        Catch
            Throw
        Finally
            accountType = Nothing
            mergedRows = Nothing
            mergedTable = Nothing
        End Try
    End Function
    'END: PPP | 09/28/2016 | YRS-AT-2529 

    'START : MMR | 2016.11.23 | YRS-AT-3145 | Loading Non-retired QDRO Details
    Private Sub LoadNonRetiredQDRODetails()
        Dim strWSMessage As String
        Dim isSessionSet As Boolean = True 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Declared boolean variable
        Try 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Added try block to start log trace 
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", "Start") 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Start Logging performance trace
            ClearData()
            ClearControlsData()

            If Not Session("FundNo") Is Nothing Then
                Headercontrol.FundNo = Session("FundNo")
                Headercontrol.PageTitle = "QDRO Member Information"
            Else
                isSessionSet = False 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | setting boolean value to false if session value not set
            End If

            If Not Session("IsArchived") Is Nothing AndAlso Session("IsArchived") = True Then
                'EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, False)
                'EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees, False)
                'EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave, False)
                'EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.Status, False)
                'START: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Disabling all tabs with error message and setting boolean value to false if session value not set
                FreezeAllTabs()
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_PARTICIPANT_DATA_ARCHIVED", "error")
                Headercontrol.PageTitle = String.Empty
                Exit Sub
            ElseIf Session("IsArchived") Is Nothing Then
                isSessionSet = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", "session variable IsArchived is nothing") 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Logging performance trace
                'END: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Disabling all tabs with error message and setting boolean value to false if session value not set
            End If

            If Not Session("PersId") Is Nothing Then
                strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
                If strWSMessage <> "NoPending" Then
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", String.Format("IsValidPerson returns: {0}", strWSMessage)) 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Logging performance trace

                    'START: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Commented existing code and used ScriptManager.RegisterStartupScript to open javascript dialog box
                    'Page.ClientScript.RegisterStartupScript(Me.GetType(), "YRS", "openDialog('" + strWSMessage + "','Pers');", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", String.Format("openDialog('{0}', 'Pers');", strWSMessage), True)
                    'EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, False)
                    'EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees, False)
                    'EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave, False)
                    'EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.Status, False)
                    'END: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Commented existing code and used ScriptManager.RegisterStartupScript to open javascript dialog box
                    Headercontrol.PageTitle = String.Empty
                    Exit Sub
                End If
            Else
                isSessionSet = False 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | setting boolean value to false if session value not set
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", "session variable PersId is nothing") 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Logging performance trace
            End If
            Me.IsAccountLock = Nothing
            If Not Session("IsAccountLock") Is Nothing Then
                Me.IsAccountLock = Session("IsAccountLock")
            Else
                isSessionSet = False 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | setting boolean value to false if session value not set
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", "session variable IsAccountLock is nothing") 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Logging performance trace
            End If

            Me.Session_datatable_dtBenifAccount = Nothing
            LoadPersonalTab()

            If Not Session("FundEventID") Is Nothing Then
                Me.string_FundEventID = Session("FundEventID")
            Else
                isSessionSet = False 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | setting boolean value to false if session value not set
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", "session variable FundEventID is nothing") 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Logging performance trace
            End If

            If Not Session("PersId") Is Nothing Then
                Me.string_PersId = Session("PersId")
            Else
                isSessionSet = False
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", "session variable PersId is nothing") 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Logging performance trace
            End If

            If Not Session("PersSSID") Is Nothing Then
                Me.string_PersSSID = Session("PersSSID")
            Else
                isSessionSet = False 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | setting boolean value to false if session value not set
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", "session variable PersSSID is nothing") 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Logging performance trace
            End If

            If Not Session("RequestID") Is Nothing Then
                Me.String_QDRORequestID = Session("RequestID")
            Else
                isSessionSet = False 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | setting boolean value to false if session value not set
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", "session variable RequestID is nothing") 'MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Logging performance trace
            End If

            'START: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Disabling all tabs and showing generic error message if session value not set
            If Not isSessionSet Then
                FreezeAllTabs()
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_PARTICIPANT_DATA_LOADING_ERROR", "error")
                Exit Sub
            End If
            'END: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Disabling all tabs and showing generic error message if session value not set

            Call SetControlFocus(Me.TextBoxSSNo)
            PopcalendarRecDate.Enabled = True
            PopcalendarRecDate2.Enabled = True
            EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary, True)
            EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, False)
            EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees, False)
            EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave, False)
            EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.Status, False)
            TextBoxSSNo.Enabled = True
            ButtonAddNewBeneficiary.Enabled = False
            ButtonResetBeneficiary.Enabled = False
            ButtonEditBeneficiary.Enabled = False
            'ButtonDocumentSave.Enabled = False
            btnShowBalance.Enabled = False
            ButtonAddBeneficiaryToList.Text = "Save Recipient"
            ShowHidePreviousButton(False)
            EnableDisableNextButton(False)
            clearParticipantSession()

            LoadRecipientDetails()
            'START: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Added catch and finally block to end trace
        Catch
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("NonRetiredQdro-LoadNonRetiredQDRODetails", "End")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            'END: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Added catch and finally block to end trace
        End Try
    End Sub
    'END : MMR | 2016.11.23 | YRS-AT-3145 | Loading Non-retired QDRO Details

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Added Close button event to redirect page to find info screen
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            ClearDataTable()
            ClearSessionData()
            ClearSetSessionFromFindInfo() 'MMR | 2016.11.23 | YRS-AT-3145 | Clearing session values
            Session("Page") = "NonRetiredQdro"
            Response.Redirect("FindInfo.aspx?Name=NonRetiredQdro", False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("btnClose_Click", ex)
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Added Close button event to redirect page to find info screen

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Navigate to next tab on click of Next button in active QDRO tab strip
    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            HiddenFieldDirty.Value = "true" 'MMR | 2016.11.23 | YRS-AT-3145 | setting hidden field value
            IsClickedNext = True
            IsClickedPrevious = False
            If LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary Then
                ShowSplitAccountsTab()
            ElseIf LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts Then
                ShowFeesTab()
            ElseIf LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ManageFees AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ManageFees Then
                SaveAppliedFeesToTable()
                If Not Me.IsZeroFeeErrorDisplayed And Not Me.IsFeeGreaterThanBalance Then
                    ShowReviewAndSaveTab()
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnNext_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Navigate to next tab on click of Next button in active QDRO tab strip

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Each tab will be displayed by calling their methods
    Private Sub ShowFeesTab()
        Dim participantTable As DataTable, recipientTable As DataTable
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, False)
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees, True)
        SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees)
        EnableDisableNextButton(True)
        ShowHidePreviousButton(True)

        If HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) AndAlso HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees) Then
            participantTable = Me.ParticipantTotalBalanceAfterSplitManageFees
            recipientTable = Me.RecipientBalanceAfterSplitManageFees
        Else
            GetBalances(participantTable, recipientTable)
            CalculateDefaultFee(participantTable, recipientTable)
        End If
        ShowFees(participantTable, recipientTable)

        Me.ParticipantTotalBalanceAfterSplitManageFees = participantTable
        Me.RecipientBalanceAfterSplitManageFees = recipientTable
    End Sub

    Private Sub ShowReviewAndSaveTab()
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees, False)
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave, True)
        SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave)
        ShowHideNextButton(False)
        ShowHidePreviousButton(True)
        ShowHideSaveButton(True)
        'SaveAppliedFeesToTable()
        If Not Me.Session_datatable_dtPecentageCount Is Nothing Then
            LoadSummaryTab()
        End If
    End Sub

    Private Sub ShowSplitAccountsTab()
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary, False)
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, True)
        SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts)
        EnableDisableNextButton(False)
        ShowHidePreviousButton(True)
        If HelperFunctions.isNonEmpty(Session_datatable_dtBenifAccount) Then
            dtBenifAccount = Me.Session_datatable_dtBenifAccount
            cboBeneficiarySSNo.DataSource = dtBenifAccount
            cboBeneficiarySSNo.DataBind()
            DatagridBenificiaryList.SelectedIndex = Me.Session_ComboValue
            If DatagridBenificiaryList.SelectedIndex = -1 Then
                Me.String_Benif_PersonD = Me.DatagridBenificiaryList.Items(0).Cells(recipientIndexPersID).Text 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Changed the index access from hardcoded 1 to by a variable recipientIndexPersID
            Else
                Me.String_Benif_PersonD = Me.DatagridBenificiaryList.SelectedItem.Cells(recipientIndexPersID).Text 'PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Changed the index access from hardcoded 1 to by a variable recipientIndexPersID
            End If
            cboBeneficiarySSNo.SelectedValue = Me.String_Benif_PersonD
            SetBeneficiaryData()
            ShowHideControls()
            ShowOtherPlanReminderWarning()
        End If
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Each tab will be displayed by calling their methods

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Navigate to previous tab on click of Previous button in active QDRO tab strip
    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Dim selectedImageButton As ImageButton
        Dim selectedRowIndex As Integer
        Dim datagridRowNo As Integer
        Try
            IsClickedNext = False
            IsClickedPrevious = True

            If LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts Then
                EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, False)
                EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary, True)
                SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary)
                EnableDisableNextButton(True)
                ShowHidePreviousButton(False)
                If HelperFunctions.isNonEmpty(Session_datatable_dtBenifAccount) Then
                    PopulateBeneficiaryData(0)
                    dtBenifAccount = Me.Session_datatable_dtBenifAccount
                    selectedRowIndex = cboBeneficiarySSNo.SelectedIndex
                    For datagridRowNo = 0 To DatagridBenificiaryList.Items.Count - 1
                        DatagridBenificiaryList.Items(datagridRowNo).Font.Bold = False
                        selectedImageButton = DatagridBenificiaryList.Items(datagridRowNo).FindControl("Imagebutton1")
                        selectedImageButton.ImageUrl = "images\select.gif"
                    Next
                    If cboBeneficiarySSNo.SelectedIndex > -1 Then
                        DatagridBenificiaryList.Items(selectedRowIndex).Font.Bold = True
                        selectedImageButton = DatagridBenificiaryList.Items(selectedRowIndex).FindControl("Imagebutton1")
                        selectedImageButton.ImageUrl = "images\selected.gif"
                        If (dtBenifAccount.Rows(selectedRowIndex).Item("FlagNewBenf")) Then
                            PopulateBeneficiaryData(selectedRowIndex)
                            lockRecipientControls(False)
                            TextBoxSSNo.Enabled = False
                            ButtonEditBeneficiary.Enabled = True
                        Else
                            PopulateBeneficiaryData(selectedRowIndex)
                            lockRecipientControls(False)
                            TextBoxSSNo.Enabled = False
                        End If
                    Else
                        DatagridBenificiaryList.Items(0).Font.Bold = True
                    End If
                    ButtonAddNewBeneficiary.Enabled = True
                    ButtonResetBeneficiary.Enabled = False
                End If
            ElseIf LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ManageFees AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ManageFees Then
                SaveAppliedFeesToTable()
                If Not Me.IsZeroFeeErrorDisplayed Then
                    EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees, False)
                    EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, True)
                    SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts)
                    EnableDisableNextButton(True)
                    ShowHidePreviousButton(True)
                    ShowHideControls()
                End If
            ElseIf LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave Then
                EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave, False)
                EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees, True)
                SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees)
                EnableDisableNextButton(True)
                ShowHidePreviousButton(True)
                ShowHideSaveButton(False)
                ShowHideNextButton(True)
                ShowFeesTab()
            End If
        Catch ex As Exception
            HelperFunctions.LogException("btnPrevious_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Navigate to previous tab on click of Previous button in active QDRO tab strip

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Enable QDRO tab strip
    Private Sub EnableDisableQDROTabStrip(ByVal selectedIndex As Integer, ByVal bnFlag As Boolean)
        Me.QdroMemberActiveTabStrip.Items(selectedIndex).Enabled = bnFlag
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Enable QDRO tab strip

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Set selected index of QDRO tab strip
    Private Sub SelectQDROTabStrip(ByVal selectedIndex As Integer)
        Me.LIstMultiPage.SelectedIndex = selectedIndex
        Me.QdroMemberActiveTabStrip.SelectedIndex = selectedIndex
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Set selected index of QDRO tab strip

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Hiding or displaying previous button
    Private Sub ShowHidePreviousButton(ByVal bnFlag As Boolean)
        btnPrevious.Visible = bnFlag
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Hidingor displaying previous button

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Hiding or displaying previous button
    Private Sub ShowHideNextButton(ByVal bnFlag As Boolean)
        btnNext.Visible = bnFlag
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Hiding or displaying previous button

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Hiding or displaying previous button
    Private Sub EnableDisableNextButton(ByVal bnFlag As Boolean)
        btnNext.Enabled = bnFlag
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Hiding or displaying save button

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Hiding or displaying previous button
    Private Sub ShowHideSaveButton(ByVal bnFlag As Boolean)
        ButtonDocumentSave.Visible = bnFlag
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Hiding or displaying save button

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Clearing session values
    Private Sub ClearSetSessionFromFindInfo()
        Me.string_FundEventID = Nothing
        Me.string_PersId = Nothing
        Me.string_PersSSID = Nothing
        Me.String_QDRORequestID = Nothing
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Clearing session values

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Designing table for displaying participant and recipient balances after split
    Private Sub GetBalances(ByRef participantTotalBalanceAfterSplitManageFees As DataTable, ByRef recipientBalanceAfterSplitManageFees As DataTable)
        Dim participantSplitConfigurationBalancesTable As DataSet, participantAccountDetailTable As DataSet
        Dim splitConfigurationTable As DataTable, recipientDetailsTable As DataTable
        Dim splitConfigurationRows As DataRow()
        Dim recipientName, recipientPersID As String, recipientFundEventID As String
        Dim recipientID As Integer
        Dim isRetirementSplitExists, isSavingsSplitExists As Boolean
        Try
            If Not Me.Session_dataset_dsAllPartAccountsDetail Is Nothing And Not Me.Session_Dataset_PartAccountDetail Is Nothing Then
                participantSplitConfigurationBalancesTable = Me.Session_dataset_dsAllPartAccountsDetail
                participantAccountDetailTable = Me.Session_Dataset_PartAccountDetail

                ' Prepare participant details table for fees
                participantTotalBalanceAfterSplitManageFees = ShowParticipantBalanceAfterSplitManageFees(participantSplitConfigurationBalancesTable, participantAccountDetailTable, Me.string_PersSSID)
                participantTotalBalanceAfterSplitManageFees.Rows(0)("RetirementFee") = 0
                participantTotalBalanceAfterSplitManageFees.Rows(0)("SavingsFee") = 0
                participantTotalBalanceAfterSplitManageFees.Rows(0)("IsRetirementPlanSplitExists") = False
                participantTotalBalanceAfterSplitManageFees.Rows(0)("IsSavingsPlanSplitExists") = False

                If Not Me.Session_datatable_dtBenifAccount Is Nothing And Not Me.Session_datatable_dtPecentageCount Is Nothing Then
                    splitConfigurationTable = Me.Session_datatable_dtPecentageCount
                    recipientDetailsTable = Me.Session_datatable_dtBenifAccount

                    isRetirementSplitExists = False
                    isSavingsSplitExists = False

                    If Not splitConfigurationTable Is Nothing Then
                        For Each splitConfigurationRow As DataRow In splitConfigurationTable.Rows
                            Select Case Convert.ToString(splitConfigurationRow("PlanType")).ToLower()
                                Case "both"
                                    isRetirementSplitExists = True
                                    isSavingsSplitExists = True
                                    Exit For
                                Case "retirement"
                                    isRetirementSplitExists = True
                                Case "savings"
                                    isSavingsSplitExists = True
                            End Select
                        Next
                    End If
                    participantTotalBalanceAfterSplitManageFees.Rows(0)("IsRetirementPlanSplitExists") = isRetirementSplitExists
                    participantTotalBalanceAfterSplitManageFees.Rows(0)("IsSavingsPlanSplitExists") = isSavingsSplitExists

                    ' Prepare recipient details table for fees
                    recipientBalanceAfterSplitManageFees = CreateTableToDisplayBalance()
                    recipientID = 1
                    For Each recipient In recipientDetailsTable.Rows
                        recipientPersID = recipient("id")
                        splitConfigurationRows = splitConfigurationTable.Select(String.Format("PersId='{0}'", recipientPersID))
                        If splitConfigurationRows.Length > 0 Then
                            recipientFundEventID = recipient("RecpFundEventId")
                            recipientName = String.Format("{0} {1}", recipient("FirstName"), recipient("LastName"))
                            recipientBalanceAfterSplitManageFees.Rows.Add(ShowRecipientBalanceAfterSplitManageFees(recipientPersID, recipientFundEventID, recipientName, splitConfigurationRows, recipientBalanceAfterSplitManageFees.NewRow(), recipientID))
                            recipientID = recipientID + 1
                        End If
                    Next
                End If
            End If
        Catch
            Throw
        Finally
            recipientName = Nothing
            recipientPersID = Nothing
            recipientFundEventID = Nothing
            splitConfigurationRows = Nothing
            splitConfigurationTable = Nothing
            recipientDetailsTable = Nothing
            participantAccountDetailTable = Nothing
            participantSplitConfigurationBalancesTable = Nothing
        End Try
    End Sub
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Designing table for displaying participant and recipient balances after split

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Designing table for applying fees for participant and recipient
    Private Sub ShowFees(ByVal participantTable As DataTable, ByVal recipientTable As DataTable)
        Dim mainTableApplyFees As System.Web.UI.HtmlControls.HtmlTable
        Dim retirementTextBox, savingsTextBox As System.Web.UI.WebControls.TextBox
        Dim retirementTotalFeesLabel, savingsTotalFeesLabel, totalFeesLabel As System.Web.UI.WebControls.Label
        Dim retirementHiddenField, savingsHiddenField, totalHiddenField As System.Web.UI.WebControls.HiddenField
        Dim tableRow As System.Web.UI.HtmlControls.HtmlTableRow

        Dim isRetirementSplitExists, isSavingsSplitExists As Boolean
        Dim recipientTotalFeesLabel As System.Web.UI.WebControls.Label
        Dim retirementFee, savingsFee, retirementTotalFee, savingsTotalFee As Decimal

        Dim mainTableBalances As System.Web.UI.HtmlControls.HtmlTable
        Dim name, retirementBalance, savingsBalance, totalBalance As String

        Dim defaultRetirementFee, defaultSavingsFee As Decimal
        Dim defaultTotalRetirementFee, defaultTotalSavingsFee As Decimal
        Try
            If Not Me.Session_datatable_dtPecentageCount Is Nothing Then
                'populating participant balance area
                isRetirementSplitExists = False
                isSavingsSplitExists = False
                If HelperFunctions.isNonEmpty(participantTable) Then
                    name = participantTable.Rows(0)("Name").ToString()
                    retirementBalance = Convert.ToDecimal(participantTable.Rows(0)("Retirement")).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                    savingsBalance = Convert.ToDecimal(participantTable.Rows(0)("Savings")).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                    totalBalance = Convert.ToDecimal(participantTable.Rows(0)("Total")).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                End If

                'populating recipient balance area
                mainTableBalances = New HtmlTable()
                mainTableBalances.Attributes.Add("width", "100%")
                mainTableBalances.Attributes.Add("class", "DataGrid_Grid")
                mainTableBalances.Attributes.Add("cellspacing", "0")
                mainTableBalances.Attributes.Add("rules", "all")
                mainTableBalances.Attributes.Add("border", "1")
                mainTableBalances.Attributes.Add("style", "width:100%;border-collapse:collapse;")
                mainTableBalances.Rows.Add(CreateMainHeaderBalancesFeesTableRow("", "Retirement", "Savings", "Total", True))
                mainTableBalances.Rows.Add(CreateMergeHeaderBalancesFeesTableRow("Participant's Balances", True))
                mainTableBalances.Rows.Add(CreateBalancesFeesTableRow(name, retirementBalance, savingsBalance, totalBalance, True, "Balances"))
                mainTableBalances.Rows.Add(CreateMergeHeaderBalancesFeesTableRow("Recipient's Balances", True))
                If HelperFunctions.isNonEmpty(recipientTable) Then
                    For count As Integer = 0 To recipientTable.Rows.Count - 1
                        retirementBalance = Convert.ToDecimal(recipientTable.Rows(count)("Retirement")).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                        savingsBalance = Convert.ToDecimal(recipientTable.Rows(count)("Savings")).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                        totalBalance = Convert.ToDecimal(recipientTable.Rows(count)("Total")).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                        mainTableBalances.Rows.Add(CreateBalancesFeesTableRow(recipientTable.Rows(count)("Name"), retirementBalance, savingsBalance, totalBalance, True, "Balances"))
                    Next
                End If

                divBalances.Controls.Add(mainTableBalances)

                'Displaying fees
                retirementFee = Convert.ToDecimal(participantTable.Rows(0)("RetirementFee"))
                savingsFee = Convert.ToDecimal(participantTable.Rows(0)("SavingsFee"))
                retirementTotalFee = retirementFee
                savingsTotalFee = savingsFee
                isRetirementSplitExists = Convert.ToBoolean(participantTable.Rows(0)("IsRetirementPlanSplitExists"))
                isSavingsSplitExists = Convert.ToBoolean(participantTable.Rows(0)("IsSavingsPlanSplitExists"))
                defaultRetirementFee = Convert.ToDecimal(participantTable.Rows(0)("DefaultRetirementFee"))
                defaultSavingsFee = Convert.ToDecimal(participantTable.Rows(0)("DefaultSavingsFee"))
                defaultTotalRetirementFee = defaultRetirementFee
                defaultTotalSavingsFee = defaultSavingsFee

                retirementTextBox = GetFeeTextbox(String.Format("txtParticipantRetirementFee{0}", IIf(isRetirementSplitExists, String.Empty, "NoPlanExists")), retirementFee, isRetirementSplitExists)
                savingsTextBox = GetFeeTextbox(String.Format("txtParticipantSavingsFee{0}", IIf(isSavingsSplitExists, String.Empty, "NoPlanExists")), savingsFee, isSavingsSplitExists)

                Dim participantTotalFeesLabel As System.Web.UI.WebControls.Label = New Label()
                participantTotalFeesLabel.ID = "lblParticipantTotalFee"
                participantTotalFeesLabel.Attributes.Add("class", "DataGrid_NormalStyle")
                participantTotalFeesLabel.Text = (retirementFee + savingsFee).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)

                mainTableApplyFees = New HtmlTable()
                mainTableApplyFees.Attributes.Add("width", "100%")
                mainTableApplyFees.Attributes.Add("class", "DataGrid_Grid")
                mainTableApplyFees.Attributes.Add("cellspacing", "0")
                mainTableApplyFees.Attributes.Add("rules", "all")
                mainTableApplyFees.Attributes.Add("border", "1")
                mainTableApplyFees.Attributes.Add("style", "width:100%;border-collapse:collapse;")

                mainTableApplyFees.Rows.Add(CreateMainHeaderBalancesFeesTableRow("", "Retirement", "Savings", "Total", True))
                mainTableApplyFees.Rows.Add(CreateMergeHeaderBalancesFeesTableRow("Participant's Fees", True))

                tableRow = CreateBalancesFeesTableRow(participantTable.Rows(0)("Name").ToString(), retirementTextBox, savingsTextBox, participantTotalFeesLabel, True, "Fees")
                ' Default Fee
                retirementHiddenField = New HiddenField()
                retirementHiddenField.Value = defaultRetirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                retirementHiddenField.ID = String.Format("hdnParticipantDefaultRetirementFee{0}", IIf(isRetirementSplitExists, String.Empty, "NoPlanExists"))
                savingsHiddenField = New HiddenField()
                savingsHiddenField.Value = defaultSavingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                savingsHiddenField.ID = String.Format("hdnParticipantDefaultSavingsFee{0}", IIf(isSavingsSplitExists, String.Empty, "NoPlanExists"))
                savingsTextBox.Controls.Add(savingsHiddenField)
                totalHiddenField = New HiddenField()
                totalHiddenField.Value = (defaultRetirementFee + defaultSavingsFee).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                totalHiddenField.ID = "hdnParticipantDefaultTotalFee"
                participantTotalFeesLabel.Controls.Add(totalHiddenField)
                tableRow.Cells(0).Controls.Add(retirementHiddenField)
                tableRow.Cells(0).Controls.Add(savingsHiddenField)
                tableRow.Cells(0).Controls.Add(totalHiddenField)
                mainTableApplyFees.Rows.Add(tableRow)

                mainTableApplyFees.Rows.Add(CreateMergeHeaderBalancesFeesTableRow("Recipient's Fees", True))
                For Each recipientTableRow In recipientTable.Rows
                    isRetirementSplitExists = Convert.ToBoolean(recipientTableRow("IsRetirementPlanSplitExists"))
                    isSavingsSplitExists = Convert.ToBoolean(recipientTableRow("IsSavingsPlanSplitExists"))
                    retirementFee = Convert.ToDecimal(recipientTableRow("RetirementFee"))
                    savingsFee = Convert.ToDecimal(recipientTableRow("SavingsFee"))
                    defaultRetirementFee = Convert.ToDecimal(recipientTableRow("DefaultRetirementFee"))
                    defaultSavingsFee = Convert.ToDecimal(recipientTableRow("DefaultSavingsFee"))

                    retirementTextBox = GetFeeTextbox(String.Format("txtRecipientRetirementFee{0}{1}", recipientTableRow("ID").ToString(), IIf(isRetirementSplitExists, String.Empty, "NoPlanExists")), retirementFee, isRetirementSplitExists)
                    savingsTextBox = GetFeeTextbox(String.Format("txtRecipientSavingsFee{0}{1}", recipientTableRow("ID").ToString(), IIf(isSavingsSplitExists, String.Empty, "NoPlanExists")), savingsFee, isSavingsSplitExists)

                    recipientTotalFeesLabel = New Label()
                    recipientTotalFeesLabel.ID = String.Format("lblRecipientTotalFee{0}", recipientTableRow("ID").ToString())
                    recipientTotalFeesLabel.Attributes.Add("class", "DataGrid_NormalStyle")
                    recipientTotalFeesLabel.Text = (retirementFee + savingsFee).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)

                    tableRow = CreateBalancesFeesTableRow(recipientTableRow("Name").ToString(), retirementTextBox, savingsTextBox, recipientTotalFeesLabel, True, "Fees")
                    ' Default Fee
                    retirementHiddenField = New HiddenField()
                    retirementHiddenField.Value = defaultRetirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                    retirementHiddenField.ID = String.Format("hdnRecipientDefaultRetirementFee{0}{1}", recipientTableRow("ID").ToString(), IIf(isRetirementSplitExists, String.Empty, "NoPlanExists"))
                    savingsHiddenField = New HiddenField()
                    savingsHiddenField.Value = defaultSavingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                    savingsHiddenField.ID = String.Format("hdnRecipientDefaultSavingsFee{0}{1}", recipientTableRow("ID").ToString(), IIf(isSavingsSplitExists, String.Empty, "NoPlanExists"))
                    totalHiddenField = New HiddenField()
                    totalHiddenField.Value = (defaultRetirementFee + defaultSavingsFee).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                    totalHiddenField.ID = String.Format("hdnRecipientDefaultTotalFee{0}", recipientTableRow("ID").ToString())
                    tableRow.Cells(0).Controls.Add(retirementHiddenField)
                    tableRow.Cells(0).Controls.Add(savingsHiddenField)
                    tableRow.Cells(0).Controls.Add(totalHiddenField)
                    mainTableApplyFees.Rows.Add(tableRow)

                    retirementTotalFee = retirementTotalFee + retirementFee
                    savingsTotalFee = savingsTotalFee + savingsFee

                    defaultTotalRetirementFee = defaultTotalRetirementFee + defaultRetirementFee
                    defaultTotalSavingsFee = defaultTotalSavingsFee + defaultSavingsFee
                Next

                mainTableApplyFees.Rows.Add(CreateMergeHeaderBalancesFeesTableRow("Total Fees", True))
                retirementTotalFeesLabel = New Label()
                retirementTotalFeesLabel.ID = "lblRetirementTotalFees"
                retirementTotalFeesLabel.Attributes.Add("class", "DataGrid_NormalStyle")
                retirementTotalFeesLabel.Text = retirementTotalFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)

                savingsTotalFeesLabel = New Label()
                savingsTotalFeesLabel.ID = "lblSavingsTotalFees"
                savingsTotalFeesLabel.Attributes.Add("class", "DataGrid_NormalStyle")
                savingsTotalFeesLabel.Text = savingsTotalFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)

                totalFeesLabel = New Label()
                totalFeesLabel.ID = "lblTotalFees"
                totalFeesLabel.Attributes.Add("class", "DataGrid_NormalStyle")
                totalFeesLabel.Text = (retirementTotalFee + savingsTotalFee).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)

                tableRow = CreateBalancesFeesTableRow("", retirementTotalFeesLabel, savingsTotalFeesLabel, totalFeesLabel, True, "Fees")

                ' Totalof default fee
                retirementHiddenField = New HiddenField()
                retirementHiddenField.Value = defaultTotalRetirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                retirementHiddenField.ID = "hdnDefaultRetirementTotalFees"
                savingsHiddenField = New HiddenField()
                savingsHiddenField.Value = defaultTotalSavingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                savingsHiddenField.ID = "hdnDefaultSavingsTotalFees"
                totalHiddenField = New HiddenField()
                totalHiddenField.Value = (defaultTotalRetirementFee + defaultTotalSavingsFee).ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
                totalHiddenField.ID = "hdnDefaultTotalFees"
                tableRow.Cells(0).Controls.Add(retirementHiddenField)
                tableRow.Cells(0).Controls.Add(savingsHiddenField)
                tableRow.Cells(0).Controls.Add(totalHiddenField)
                mainTableApplyFees.Rows.Add(tableRow)

                divApplyFees.Controls.Add(mainTableApplyFees)
            End If
        Catch
            Throw
        Finally
            name = Nothing
            retirementBalance = Nothing
            savingsBalance = Nothing
            totalBalance = Nothing
            mainTableBalances = Nothing
            recipientTotalFeesLabel = Nothing
        End Try
    End Sub

    Private Function GetFeeTextbox(ByVal controlID As String, ByVal fee As Decimal, ByVal isEnabled As Boolean) As System.Web.UI.WebControls.TextBox
        Dim textBoxControl As System.Web.UI.WebControls.TextBox
        textBoxControl = New TextBox()
        textBoxControl.ID = controlID
        textBoxControl.Attributes.Add("width", "30px")
        textBoxControl.Attributes.Add("class", "TextBox_Normal_AmountRetiree")
        textBoxControl.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
        textBoxControl.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
        textBoxControl.Attributes.Add("onblur", "MaintainFeeTotal();")
        textBoxControl.Text = fee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture)
        textBoxControl.Enabled = isEnabled
        If isEnabled Then
            textBoxControl.Enabled = chkApplyFees.Checked
        End If
        Return textBoxControl
    End Function

    'END : MMR | 2016.11.30 | YRS-AT-3145 | Designing table for applying fees for participant and recipient

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Creating rows for displaying participant and recipient balances
    Private Function CreateMainHeaderBalancesFeesTableRow(ByVal participantName As String, ByVal retirementBalance As String, ByVal savingsBalance As String, ByVal totalBalance As String, ByVal isAddCSSClass As Boolean) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableRowBalances As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCellBalances As System.Web.UI.HtmlControls.HtmlTableCell

        mainTableRowBalances = New HtmlTableRow()
        If isAddCSSClass Then
            mainTableRowBalances.Attributes.Add("class", "DataGrid_HeaderStyle")
        End If

        mainTableCellBalances = New HtmlTableCell()
        mainTableCellBalances.Attributes.Add("width", "40%")
        mainTableCellBalances.Attributes.Add("align", "center")
        mainTableCellBalances.InnerText = participantName
        mainTableRowBalances.Cells.Add(mainTableCellBalances)

        mainTableCellBalances = New HtmlTableCell()
        mainTableCellBalances.Attributes.Add("width", "20%")
        mainTableCellBalances.Attributes.Add("align", "center")
        mainTableCellBalances.InnerText = retirementBalance
        mainTableRowBalances.Cells.Add(mainTableCellBalances)

        mainTableCellBalances = New HtmlTableCell()
        mainTableCellBalances.Attributes.Add("width", "20%")
        mainTableCellBalances.Attributes.Add("align", "center")
        mainTableCellBalances.InnerText = savingsBalance
        mainTableRowBalances.Cells.Add(mainTableCellBalances)

        mainTableCellBalances = New HtmlTableCell()
        mainTableCellBalances.Attributes.Add("width", "20%")
        mainTableCellBalances.Attributes.Add("align", "center")
        mainTableCellBalances.InnerText = totalBalance
        mainTableRowBalances.Cells.Add(mainTableCellBalances)

        Return mainTableRowBalances

    End Function
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Creating rows for displaying participant and recipient balances

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Creating rows for applying fees for participant and recipient
    Private Function CreateBalancesFeesTableRow(ByVal participantName As String, ByVal retirementBalance As Object, ByVal savingsBalance As Object, ByVal totalBalance As Object, ByVal isAddCSSClass As Boolean, ByVal sectionName As String) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableRowBalances As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCellBalances As System.Web.UI.HtmlControls.HtmlTableCell

        mainTableRowBalances = New HtmlTableRow()
        If isAddCSSClass Then
            mainTableRowBalances.Attributes.Add("class", "DataGrid_NormalStyle")
        End If
        If sectionName.Equals("Balances") Then
            mainTableCellBalances = New HtmlTableCell()
            mainTableCellBalances.Attributes.Add("width", "40%")
            mainTableCellBalances.Attributes.Add("align", "left")
            mainTableCellBalances.InnerText = participantName
            mainTableRowBalances.Cells.Add(mainTableCellBalances)

            mainTableCellBalances = New HtmlTableCell()
            mainTableCellBalances.Attributes.Add("width", "20%")
            mainTableCellBalances.Attributes.Add("align", "right")
            mainTableCellBalances.InnerText = retirementBalance
            mainTableRowBalances.Cells.Add(mainTableCellBalances)

            mainTableCellBalances = New HtmlTableCell()
            mainTableCellBalances.Attributes.Add("width", "20%")
            mainTableCellBalances.Attributes.Add("align", "right")
            mainTableCellBalances.InnerText = savingsBalance
            mainTableRowBalances.Cells.Add(mainTableCellBalances)

            mainTableCellBalances = New HtmlTableCell()
            mainTableCellBalances.Attributes.Add("width", "20%")
            mainTableCellBalances.Attributes.Add("align", "right")
            mainTableCellBalances.InnerText = totalBalance
            mainTableRowBalances.Cells.Add(mainTableCellBalances)
        ElseIf sectionName.Equals("Fees") Then
            mainTableCellBalances = New HtmlTableCell()
            mainTableCellBalances.Attributes.Add("width", "40%")
            mainTableCellBalances.Attributes.Add("align", "left")
            mainTableCellBalances.InnerText = participantName
            mainTableRowBalances.Cells.Add(mainTableCellBalances)

            mainTableCellBalances = New HtmlTableCell()
            mainTableCellBalances.Attributes.Add("width", "20%")
            mainTableCellBalances.Attributes.Add("align", "right")
            mainTableCellBalances.Controls.Add(retirementBalance)
            mainTableRowBalances.Cells.Add(mainTableCellBalances)

            mainTableCellBalances = New HtmlTableCell()
            mainTableCellBalances.Attributes.Add("width", "20%")
            mainTableCellBalances.Attributes.Add("align", "right")
            mainTableCellBalances.Controls.Add(savingsBalance)
            mainTableRowBalances.Cells.Add(mainTableCellBalances)

            mainTableCellBalances = New HtmlTableCell()
            mainTableCellBalances.Attributes.Add("width", "20%")
            mainTableCellBalances.Attributes.Add("align", "right")
            mainTableCellBalances.Controls.Add(totalBalance)
            mainTableRowBalances.Cells.Add(mainTableCellBalances)
        End If


        Return mainTableRowBalances

    End Function
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Creating rows for applying fees for participant and recipient

    'START : MMR | 2016.11.30 | YRS-AT-3145 | Creating rows for merging to display header section for participant and recipient
    Private Function CreateMergeHeaderBalancesFeesTableRow(ByVal participantName As String, ByVal isAddCSSClass As Boolean) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableRowBalances As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCellBalances As System.Web.UI.HtmlControls.HtmlTableCell

        mainTableRowBalances = New HtmlTableRow()
        If isAddCSSClass Then
            mainTableRowBalances.Attributes.Add("class", "DataGrid_HeaderStyle")
        End If

        mainTableCellBalances = New HtmlTableCell()
        mainTableCellBalances.Attributes.Add("align", "left")
        mainTableCellBalances.Attributes.Add("colspan", "4")
        mainTableCellBalances.InnerText = participantName
        mainTableRowBalances.Cells.Add(mainTableCellBalances)

        Return mainTableRowBalances

    End Function
    'END : MMR | 2016.11.30 | YRS-AT-3145 | Creating rows for merging to display header section for participant and recipient

    'START: PPP | 12/28/2016 | YRS-AT-3145 & YRS-AT-3265 
    Private Function ShowParticipantBalanceAfterSplitManageFees(ByRef participantTransactionDetail As DataSet, ByRef participantAccountDetail As DataSet, ByVal SSNo As String) As DataTable
        Dim participantDetail As DataSet
        Dim participantAccountWiseBalace As DataTable
        Dim participantTotalBalanceAfterSplit As DataTable
        Dim participantBalanceRow As DataRow
        Dim participantName, planTypeToDisplay As String
        Dim retirementBalance, savingsBalance, total As Decimal
        Try
            participantDetail = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.GetParticipantDetail(SSNo)
            If HelperFunctions.isNonEmpty(participantDetail) Then
                participantName = String.Format("{0} {1}", participantDetail.Tables(0).Rows(0)("FirstName").ToString(), participantDetail.Tables(0).Rows(0)("LastName").ToString())
            End If

            planTypeToDisplay = GetParticipantPlanToShowOnSummary()
            participantAccountWiseBalace = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable(planTypeToDisplay, participantTransactionDetail, participantAccountDetail)
            If HelperFunctions.isNonEmpty(participantAccountWiseBalace) Then
                retirementBalance = Convert.ToDecimal(IIf(Convert.IsDBNull(participantAccountWiseBalace.Compute("Sum(TotalTotal)", "PlanType = 'RETIREMENT'")), 0, participantAccountWiseBalace.Compute("Sum(TotalTotal)", "PlanType = 'RETIREMENT'")))
                savingsBalance = Convert.ToDecimal(IIf(Convert.IsDBNull(participantAccountWiseBalace.Compute("Sum(TotalTotal)", "PlanType = 'SAVINGS'")), 0, participantAccountWiseBalace.Compute("Sum(TotalTotal)", "PlanType = 'SAVINGS'")))
            End If

            total = retirementBalance + savingsBalance

            participantTotalBalanceAfterSplit = CreateTableToDisplayBalance()
            participantBalanceRow = participantTotalBalanceAfterSplit.NewRow()
            participantBalanceRow("Name") = participantName
            participantBalanceRow("Retirement") = retirementBalance
            participantBalanceRow("Savings") = savingsBalance
            participantBalanceRow("Total") = total
            participantTotalBalanceAfterSplit.Rows.Add(participantBalanceRow)

            Return participantTotalBalanceAfterSplit
        Catch
            Throw
        Finally
            participantName = Nothing
            planTypeToDisplay = Nothing
            participantBalanceRow = Nothing
            participantTotalBalanceAfterSplit = Nothing
            participantAccountWiseBalace = Nothing
            participantDetail = Nothing
        End Try
    End Function

    Private Function ShowRecipientBalanceAfterSplitManageFees(ByVal recipientPersID As String, ByVal recipientFundEventID As String, ByVal recipientName As String, ByRef splitConfigurationRows As DataRow(), ByRef recipientBalanceRow As DataRow, ByVal rowID As Integer) As DataRow
        Dim recipientAccountWiseBalance As DataTable
        Dim recipientPlanTypeRow As DataRow()
        Dim retirementBalance, savingsBalance, total As Decimal
        Dim isRetirementSplitExists, isSavingsSplitExists As Boolean
        Try
            recipientAccountWiseBalance = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareBeneficiaryAfterSplitAccountTable(recipientFundEventID, "Both", Me.Session_dataset_dsAllRecipantAccountsDetail, Me.Session_Dataset_PartAccountDetail)
            If HelperFunctions.isNonEmpty(recipientAccountWiseBalance) Then
                retirementBalance = Convert.ToDecimal(IIf(Convert.IsDBNull(recipientAccountWiseBalance.Compute("Sum(TotalTotal)", "PlanType = 'RETIREMENT'")), 0, recipientAccountWiseBalance.Compute("Sum(TotalTotal)", "PlanType = 'RETIREMENT'")))
                savingsBalance = Convert.ToDecimal(IIf(Convert.IsDBNull(recipientAccountWiseBalance.Compute("Sum(TotalTotal)", "PlanType = 'SAVINGS'")), 0, recipientAccountWiseBalance.Compute("Sum(TotalTotal)", "PlanType = 'SAVINGS'")))
            End If
            total = retirementBalance + savingsBalance

            ' Check which plan is exists
            isRetirementSplitExists = False
            isSavingsSplitExists = False
            If splitConfigurationRows.Length > 0 Then
                For Each splitConfigurationRow As DataRow In splitConfigurationRows
                    Select Case Convert.ToString(splitConfigurationRow("PlanType")).ToLower()
                        Case "both"
                            isRetirementSplitExists = True
                            isSavingsSplitExists = True
                        Case "retirement"
                            isRetirementSplitExists = True
                        Case "savings"
                            isSavingsSplitExists = True
                    End Select
                Next
            End If

            recipientBalanceRow("ID") = rowID
            recipientBalanceRow("PersID") = recipientPersID
            recipientBalanceRow("Name") = recipientName
            recipientBalanceRow("Retirement") = retirementBalance
            recipientBalanceRow("Savings") = savingsBalance
            recipientBalanceRow("Total") = total
            recipientBalanceRow("RetirementFee") = 0
            recipientBalanceRow("SavingsFee") = 0
            recipientBalanceRow("IsRetirementPlanSplitExists") = isRetirementSplitExists
            recipientBalanceRow("IsSavingsPlanSplitExists") = isSavingsSplitExists

            Return recipientBalanceRow
        Catch
            Throw
        Finally
            recipientPlanTypeRow = Nothing
            recipientAccountWiseBalance = Nothing
        End Try
    End Function

    Private Function CreateTableToDisplayBalance() As DataTable
        Dim balancesTable As New DataTable
        balancesTable.Columns.Add(New DataColumn("ID", GetType(Integer)))
        balancesTable.Columns.Add(New DataColumn("PersID", GetType(String)))
        balancesTable.Columns.Add(New DataColumn("Name", GetType(String)))
        balancesTable.Columns.Add(New DataColumn("Retirement", GetType(Decimal)))
        balancesTable.Columns.Add(New DataColumn("Savings", GetType(Decimal)))
        balancesTable.Columns.Add(New DataColumn("Total", GetType(Decimal)))
        balancesTable.Columns.Add(New DataColumn("IsRetirementPlanSplitExists", GetType(Boolean)))
        balancesTable.Columns.Add(New DataColumn("IsSavingsPlanSplitExists", GetType(Boolean)))
        balancesTable.Columns.Add(New DataColumn("RetirementFee", GetType(Decimal)))
        balancesTable.Columns.Add(New DataColumn("SavingsFee", GetType(Decimal)))
        balancesTable.Columns.Add(New DataColumn("DefaultRetirementFee", GetType(Decimal)))
        balancesTable.Columns.Add(New DataColumn("DefaultSavingsFee", GetType(Decimal)))
        Return balancesTable
    End Function

    Private Sub SaveAppliedFeesToTable()
        Dim totalFees() As String
        Dim participantFee(), recipientFee(), individualRecipientFee() As String
        Dim participantFeeTable, recipientFeeTable As DataTable
        Dim row() As DataRow
        Dim isFeeGreaterThanBalance As Boolean

        Me.IsZeroFeeErrorDisplayed = False
        Me.IsFeeGreaterThanBalance = False
        If chkApplyFees.Checked Then
            If HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) AndAlso HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees) AndAlso Not String.IsNullOrEmpty(hdnFees.Value) Then
                participantFeeTable = Me.ParticipantTotalBalanceAfterSplitManageFees
                recipientFeeTable = Me.RecipientBalanceAfterSplitManageFees

                totalFees = hdnFees.Value.Split("|")
                If totalFees.Count = 2 Then
                    'Accessing particicpant fees
                    participantFee = totalFees(0).Split("-")
                    If participantFee.Count = 2 Then
                        participantFeeTable.Rows(0)("RetirementFee") = Convert.ToDecimal(participantFee(0))
                        participantFeeTable.Rows(0)("SavingsFee") = Convert.ToDecimal(participantFee(1))
                    End If

                    'Accessing recipient fees
                    recipientFee = totalFees(1).Split(",")
                    For Each record As String In recipientFee
                        If Not String.IsNullOrEmpty(record.Trim) Then
                            individualRecipientFee = record.Split("-")
                            If individualRecipientFee.Count > 0 Then
                                row = recipientFeeTable.Select(String.Format("ID={0}", individualRecipientFee(0)))
                                If row.Count > 0 Then
                                    row(0)("RetirementFee") = Convert.ToDecimal(individualRecipientFee(1))
                                    row(0)("SavingsFee") = Convert.ToDecimal(individualRecipientFee(2))
                                End If
                            End If
                        End If
                    Next
                End If

                Me.ParticipantTotalBalanceAfterSplitManageFees = participantFeeTable
                Me.RecipientBalanceAfterSplitManageFees = recipientFeeTable

                If IsZeroFeeDefined(participantFeeTable, recipientFeeTable) Then
                    'MessageBox.Show(PlaceHolder1, "QDRO", GetMessageFromResource("MESSAGE_QRDO_FEES_NOT_DEFINED"), MessageBoxButtons.YesNo, False)
                    ShowModalPopupMessage("QDRO", "MESSAGE_QRDO_FEES_NOT_DEFINED", "infoYesNo")
                    Me.IsZeroFeeErrorDisplayed = True
                    ShowFeesTab()
                    Exit Sub
                End If

                If participantFeeTable.Rows(0)("Retirement") < participantFeeTable.Rows(0)("RetirementFee") Then
                    isFeeGreaterThanBalance = True
                ElseIf participantFeeTable.Rows(0)("Savings") < participantFeeTable.Rows(0)("SavingsFee") Then
                    isFeeGreaterThanBalance = True
                Else
                    For Each recipientRow As DataRow In recipientFeeTable.Rows
                        If recipientRow("Retirement") < recipientRow("RetirementFee") Then
                            isFeeGreaterThanBalance = True
                            Exit For
                        ElseIf recipientRow("Savings") < recipientRow("SavingsFee") Then
                            isFeeGreaterThanBalance = True
                            Exit For
                        End If
                    Next
                End If

                If isFeeGreaterThanBalance Then
                    ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_AFTER_FEE_NEGATIVEBALANCE", "error")
                    ShowFeesTab()
                    ShowHideSaveButton(False)
                    ShowHideNextButton(True)
                    Me.IsFeeGreaterThanBalance = True
                End If
            End If
            'START: MMR | 2017.01.20 | YRS-AT-3145 & 3265 | set fees to 0 when checkbox not selected 
        Else
            DoNotApplyFees()
            'END: MMR | 2017.01.20 | YRS-AT-3145 & 3265 | set fees to 0 when checkbox not selected 
        End If
    End Sub

    Private Sub DrawParticipantTable(ByVal participantDetails As DataSet)
        Dim mainTable As System.Web.UI.HtmlControls.HtmlTable
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim participantAfterSplitValuesTable As DataTable
        Dim planTypeToDisplay As String
        Try
            mainTable = New HtmlTable()
            mainTable.Attributes.Add("width", "98%")
            mainTable.Rows.Add(CreateBeneficiaryTableRow("SSNo.", "Last Name", "First Name", "Fund status", True))
            mainTable.Rows.Add(CreateBeneficiaryTableRow(participantDetails.Tables(0).Rows(0)("SSNo"), participantDetails.Tables(0).Rows(0)("LastName"), participantDetails.Tables(0).Rows(0)("FirstName"), "Active", False))

            planTypeToDisplay = GetParticipantPlanToShowOnSummary()
            participantAfterSplitValuesTable = YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.PrepareParticipantAfterSplitAccountTable(planTypeToDisplay, Me.Session_dataset_dsAllPartAccountsDetail, Me.Session_Dataset_PartAccountDetail)

            If Not participantAfterSplitValuesTable Is Nothing Then
                mainTable.Rows.Add(CreateParticipantAccountDetailsRow(participantAfterSplitValuesTable))
            End If

            divParticipantTable.Controls.Add(mainTable)
        Catch
            Throw
        Finally
            planTypeToDisplay = Nothing
            participantAfterSplitValuesTable = Nothing
            mainTableCell = Nothing
            mainTableRow = Nothing
            mainTable = Nothing
        End Try
    End Sub

    Private Function CreateParticipantAccountDetailsRow(ByVal participantAfterSplitValuesTable As DataTable) As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim mainTableCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim accountTable As System.Web.UI.HtmlControls.HtmlTable
        Dim accountRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim accountCell As System.Web.UI.HtmlControls.HtmlTableCell

        Dim accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total As String

        Dim feeDetails As DataTable
        Dim retirementFee, savingsFee As Decimal

        Dim isRetirementPlanSplitExists, isSavingsPlanSplitExists As Boolean
        Try
            retirementFee = 0
            savingsFee = 0
            isRetirementPlanSplitExists = False
            isSavingsPlanSplitExists = False
            If HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) Then
                feeDetails = Me.ParticipantTotalBalanceAfterSplitManageFees
                retirementFee = Convert.ToDecimal(feeDetails.Rows(0)("RetirementFee"))
                savingsFee = Convert.ToDecimal(feeDetails.Rows(0)("SavingsFee"))
                isRetirementPlanSplitExists = Convert.ToBoolean(feeDetails.Rows(0)("IsRetirementPlanSplitExists"))
                isSavingsPlanSplitExists = Convert.ToBoolean(feeDetails.Rows(0)("IsSavingsPlanSplitExists"))
            End If

            accountTable = New HtmlTable()
            accountTable.Attributes.Add("class", "DataGrid_Grid")
            accountTable.Attributes.Add("cellspacing", "0")
            accountTable.Attributes.Add("rules", "all")
            accountTable.Attributes.Add("border", "1")
            accountTable.Attributes.Add("style", "width:100%;border-collapse:collapse;")

            'Add headers
            accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_HeaderStyle", "Acct", "Taxable", "Non-Taxable", "Interest", "YMCA Taxable", "YMCA Interest", "Acct. Total "))

            accountCell = New HtmlTableCell()
            accountCell.Attributes.Add("colspan", "7")
            accountCell.Attributes.Add("style", "text-indent: 20px;")
            If isRetirementPlanSplitExists And isSavingsPlanSplitExists Then
                accountCell.InnerText = String.Format("Fees: Retirement (${0}); Savings (${1})", retirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture), savingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
            ElseIf isRetirementPlanSplitExists Then
                accountCell.InnerText = String.Format("Fees: Retirement (${0})", retirementFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
            ElseIf isSavingsPlanSplitExists Then
                accountCell.InnerText = String.Format("Fees: Savings (${0})", savingsFee.ToString("#0.00", System.Globalization.CultureInfo.InvariantCulture))
            End If

            accountRow = New HtmlTableRow()
            accountRow.Attributes.Add("class", "DataGrid_HeaderStyle")
            accountRow.Cells.Add(accountCell)
            accountTable.Rows.Add(accountRow)

            If HelperFunctions.isNonEmpty(participantAfterSplitValuesTable) Then
                For rowCounter As Integer = 0 To participantAfterSplitValuesTable.Rows.Count - 1
                    accountType = participantAfterSplitValuesTable.Rows(rowCounter)("AcctType")
                    personalPreTax = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("PersonalPreTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    personalPostTax = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("PersonalPostTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    personalInterest = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("PersonalInterestBalance")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    ymcaTaxable = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("YMCAPreTax")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    ymcaInterest = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("YMCAInterestBalance")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)
                    total = Convert.ToDecimal(participantAfterSplitValuesTable.Rows(rowCounter)("TotalTotal")).ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture)

                    If (rowCounter Mod 2) = 0 Then
                        accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_NormalStyle", accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total))
                    Else
                        accountTable.Rows.Add(CreateBeneficiarySplitRow("DataGrid_AlternateStyle", accountType, personalPreTax, personalPostTax, personalInterest, ymcaTaxable, ymcaInterest, total))
                    End If
                Next
            End If

            mainTableCell = New HtmlTableCell
            mainTableCell.Attributes.Add("colspan", "4")
            mainTableCell.Controls.Add(accountTable)

            mainTableRow = New HtmlTableRow
            mainTableRow.Cells.Add(mainTableCell)

            Return mainTableRow
        Catch
            Throw
        Finally
            accountType = Nothing
            personalPreTax = Nothing
            personalPostTax = Nothing
            personalInterest = Nothing
            ymcaTaxable = Nothing
            ymcaInterest = Nothing
            total = Nothing
            participantAfterSplitValuesTable = Nothing
            accountCell = Nothing
            accountRow = Nothing
            accountTable = Nothing
            mainTableCell = Nothing
            mainTableRow = Nothing
        End Try
    End Function

    Private Sub ShowModalPopupMessage(ByVal title As String, ByVal messageKey As String, ByVal type As String)
        Select Case type
            Case "info"
                If DivWarningMessage.InnerHtml.Trim.Length > 0 Then
                    DivWarningMessage.InnerHtml = String.Format("{0}<br />{1}", DivWarningMessage.InnerHtml, GetMessageFromResource(messageKey))
                Else
                    DivWarningMessage.InnerHtml = GetMessageFromResource(messageKey)
                End If
                DivWarningMessage.Style("display") = "normal"
            Case "infoYesNo"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", String.Format("ShowDialog('{0}', '{1}','{2}');", title, GetMessageFromResource(messageKey), type), True)
            Case "ok"
                If DivSuccessMessage.InnerHtml.Trim.Length > 0 Then
                    DivSuccessMessage.InnerHtml = String.Format("{0}<br />{1}", DivSuccessMessage.InnerHtml, GetMessageFromResource(messageKey))
                Else
                    DivSuccessMessage.InnerHtml = GetMessageFromResource(messageKey)
                End If
                DivSuccessMessage.Style("display") = "normal"
            Case "error"
                If DivMainMessage.InnerHtml.Trim.Length > 0 Then
                    DivMainMessage.InnerHtml = String.Format("{0}<br />{1}", DivMainMessage.InnerHtml, GetMessageFromResource(messageKey))
                Else
                    DivMainMessage.InnerHtml = GetMessageFromResource(messageKey)
                End If
                DivMainMessage.Style("display") = "normal"
        End Select
    End Sub

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

    Private Sub HideDivMessageControls()
        DivMainMessage.Style("display") = "none"
        DivWarningMessage.Style("display") = "none"
        DivSuccessMessage.Style("display") = "none"
        DivPlanWarningMessage.Style("display") = "none"
    End Sub

    Private Sub NeutraliseAllYesNoMessageFlags()
        Me.IsZeroFeeErrorDisplayed = False
        Me.IsFinalSaveWarning = False
        Me.IsDisbursementOrLoanExists = False
        Me.IsWithdrawalRequestStatusNotViewed = False
    End Sub

    Private Sub btnConfirmDialogYes_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogYes.Click
        If hdnRecipientForDeletion.Value.Trim.Length > 0 Then
            DeleteDataFromTable(hdnRecipientForDeletion.Value.Trim)
            LoadRecipientGrid(Me.Session_datatable_dtBenifAccount)
            ShowOtherPlanReminderWarning()
            ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_DELETED_SUCCESSFULLY", "ok")
        ElseIf Me.IsZeroFeeErrorDisplayed Then
            Me.IsZeroFeeErrorDisplayed = False
            DoNotApplyFees()
            If LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ManageFees AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.ManageFees Then
                If IsClickedNext Then
                    ShowReviewAndSaveTab()
                ElseIf IsClickedPrevious Then
                    EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees, False)
                    EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, True)
                    SelectQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts)
                    EnableDisableNextButton(True)
                    ShowHidePreviousButton(True)
                    ShowHideControls()
                End If
            End If
        ElseIf Me.IsFinalSaveWarning Then
            Me.IsFinalSaveWarning = False
            SaveNonRetiredSplit()
        ElseIf Me.IsVerifiedAddressWarning Then
            Me.IsVerifiedAddressWarning = False
            DocumentSave()
        ElseIf Me.IsDisbursementOrLoanExists Then
            Me.IsDisbursementOrLoanExists = False
            Split()
        ElseIf Me.IsWithdrawalRequestStatusNotViewed Then
            Me.IsWithdrawalRequestStatusNotViewed = False
            Response.Redirect("MainWebForm.aspx", False)
            ClearDataTable()
            ClearSessionData()
        Else
            SetUpControlsForNewRecipientAddition()
        End If
    End Sub

    Private Sub LoadRecipientGrid(ByVal data As DataTable)
        DatagridBenificiaryList.DataSource = data
        DatagridBenificiaryList.DataBind()
        DatagridBenificiaryList.SelectedIndex = 0

        cboBeneficiarySSNo.DataSource = data
        cboBeneficiarySSNo.DataBind()

        SetUpControlsForNewRecipientAddition()

        If Not HelperFunctions.isNonEmpty(data) Then
            EnableDisableNextButton(False)
        End If
    End Sub

    Private Sub LoadRecipientDetails()
        Dim beneficaryData As DataSet
        Dim row As DataRow
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
                    row("bitRecipientSpouse") = False
                    If Not Convert.IsDBNull(recipientRow("bitIsSpouse")) Then
                        row("bitRecipientSpouse") = Convert.ToBoolean(recipientRow("bitIsSpouse"))
                    End If
                    row("FlagNewBenf") = False
                    If Not Convert.IsDBNull(recipientRow("bitIsNew")) Then
                        row("FlagNewBenf") = Convert.ToBoolean(recipientRow("bitIsNew"))
                    End If
                    Me.string_RecptFundEventID = Guid.NewGuid().ToString()
                    row("RecpFundEventId") = Me.string_RecptFundEventID
                    dtBenifAccount.Rows.Add(row)
                Next
                Me.Session_datatable_dtBenifAccount = dtBenifAccount
                LoadRecipientGrid(dtBenifAccount)
                SetBeneficiaryData()
                Me.Session_ComboValue = cboBeneficiarySSNo.SelectedIndex
            Else
                SetUpControlsForNewRecipientAddition()
                EnableDisableNextButton(False)
            End If
        Catch
            Throw
        Finally
            row = Nothing
            beneficaryData = Nothing
        End Try
    End Sub

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

    Private Sub SetUpControlsForNewRecipientAddition()
        ButtonAddBeneficiaryToList.Enabled = False
        ButtonAddBeneficiaryToList.Text = "Save Recipient"
        ClearControls(True)
        lockRecipientControls(False)
        TextBoxSSNo.Enabled = True
        SetControlFocus(Me.TextBoxSSNo)
        If Not Me.Session_datatable_dtBenifAccount Is Nothing Then
            If Me.Session_datatable_dtBenifAccount.Rows.Count > 0 Then
                ButtonResetBeneficiary.Enabled = True
            End If
        End If
        ButtonEditBeneficiary.Enabled = False
        hdnSelectedPlanType.Value = String.Empty
        EnableDisableNextButton(True)
    End Sub

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
            If Not Me.Session_datatable_dtPecentageCount Is Nothing And Not String.IsNullOrEmpty(Me.String_Benif_PersonD) Then
                dtPecentageCount = Me.Session_datatable_dtPecentageCount
                splitConfigurationRows = dtPecentageCount.Select(String.Format("PersId='{0}'", Me.String_Benif_PersonD))
                If splitConfigurationRows.Length > 0 Then
                    For Each configRow In splitConfigurationRows
                        Select Case Convert.ToString(configRow("PlanType")).ToLower()
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
            If LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts Then
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
            ElseIf LIstMultiPage.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary AndAlso QdroMemberActiveTabStrip.SelectedIndex = YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary Then
                If Not isBothSplitExists And Not hdnSelectedPlanType.Value.Trim() = "" And Not hdnSelectedPlanType.Value.Trim().ToLower() = "both" Then
                    isMessageToBeDisplayed = True
                    If hdnSelectedPlanType.Value.ToLower() = "savings" Then
                        planTypeToCheckAccounts = "Retirement"
                    ElseIf hdnSelectedPlanType.Value.ToLower() = "retirement" Then
                        planTypeToCheckAccounts = "Savings"
                    End If
                End If
            End If

            ' Check for accounts in other plan for which we are showing warning, if accounts itself are not there then do not show message.
            If isMessageToBeDisplayed AndAlso Not String.IsNullOrEmpty(planTypeToCheckAccounts) AndAlso HelperFunctions.isNonEmpty(Me.Session_Dataset_PartAccountDetail) Then
                l_dataset_PartAccountDetail = Me.Session_Dataset_PartAccountDetail
                accountRows = l_dataset_PartAccountDetail.Tables(0).Select(String.Format("PlanType='{0}'", planTypeToCheckAccounts))
                If (accountRows.Length = 0) Then
                    isMessageToBeDisplayed = False 'No accounts exists so do not display message
                Else
                    'Check total amount
                    hasPositiveBalance = False
                    For Each accountRow As DataRow In accountRows
                        If Not Convert.IsDBNull(accountRow("mnyBalance")) Then
                            If Convert.ToDecimal(accountRow("mnyBalance")) > 0 Then
                                hasPositiveBalance = True
                                Exit For
                            End If
                        End If
                    Next
                    If Not hasPositiveBalance Then
                        isMessageToBeDisplayed = False
                    End If
                End If
            End If

            If isMessageToBeDisplayed Then
                DivPlanWarningMessage.Style("display") = "normal"
                DivPlanWarningMessage.InnerHtml = GetMessageFromResource("MESSAGE_QDRO_OTHER_PLAN_REMINDER")
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

    Private Sub CalculateDefaultFee(ByRef participantTable As DataTable, ByRef recipientTable As DataTable)
        Dim count As Integer
        Dim result, participant, recipient, tempFeeHolder As Decimal
        Dim isRetirementSplitExists, isSavingsSplitExists As Boolean
        Dim fee As Decimal

        fee = GetDefaultFeeAmount()
        count = recipientTable.Rows.Count + 1
        participant = 0
        recipient = 0
        If (fee > 0) Then
            result = fee / count
            result = Math.Round(result, 2)
            If (fee < (result * count)) Then
                participant = result - ((result * count) - fee)
            ElseIf (fee > (result * count)) Then
                participant = result + (fee - (result * count))
            Else
                participant = result
            End If
            recipient = result

            isRetirementSplitExists = participantTable.Rows(0)("IsRetirementPlanSplitExists")
            isSavingsSplitExists = participantTable.Rows(0)("IsSavingsPlanSplitExists")
            If isRetirementSplitExists And isSavingsSplitExists Then
                tempFeeHolder = participant / 2
                tempFeeHolder = Math.Round(tempFeeHolder, 2)
                If (participant < (tempFeeHolder * 2)) Then
                    participantTable.Rows(0)("DefaultRetirementFee") = tempFeeHolder
                    participantTable.Rows(0)("DefaultSavingsFee") = tempFeeHolder - ((tempFeeHolder * 2) - participant)
                ElseIf (participant > (tempFeeHolder * 2)) Then
                    participantTable.Rows(0)("DefaultRetirementFee") = tempFeeHolder
                    participantTable.Rows(0)("DefaultSavingsFee") = tempFeeHolder + (participant - (tempFeeHolder * 2))
                Else
                    participantTable.Rows(0)("DefaultRetirementFee") = tempFeeHolder
                    participantTable.Rows(0)("DefaultSavingsFee") = tempFeeHolder
                End If
            ElseIf isRetirementSplitExists Then
                participantTable.Rows(0)("DefaultRetirementFee") = participant
                participantTable.Rows(0)("DefaultSavingsFee") = 0
            ElseIf isSavingsSplitExists Then
                participantTable.Rows(0)("DefaultRetirementFee") = 0
                participantTable.Rows(0)("DefaultSavingsFee") = participant
            End If

            For Each row As DataRow In recipientTable.Rows
                isRetirementSplitExists = row("IsRetirementPlanSplitExists")
                isSavingsSplitExists = row("IsSavingsPlanSplitExists")
                If isRetirementSplitExists And isSavingsSplitExists Then
                    tempFeeHolder = recipient / 2
                    tempFeeHolder = Math.Round(tempFeeHolder, 2)
                    If (recipient < (tempFeeHolder * 2)) Then
                        row("DefaultRetirementFee") = tempFeeHolder
                        row("DefaultSavingsFee") = tempFeeHolder - ((tempFeeHolder * 2) - recipient)
                    ElseIf (recipient > (tempFeeHolder * 2)) Then
                        row("DefaultRetirementFee") = tempFeeHolder
                        row("DefaultSavingsFee") = tempFeeHolder + (recipient - (tempFeeHolder * 2))
                    Else
                        row("DefaultRetirementFee") = tempFeeHolder
                        row("DefaultSavingsFee") = tempFeeHolder
                    End If
                ElseIf isRetirementSplitExists Then
                    row("DefaultRetirementFee") = recipient
                    row("DefaultSavingsFee") = 0
                ElseIf isSavingsSplitExists Then
                    row("DefaultRetirementFee") = 0
                    row("DefaultSavingsFee") = recipient
                End If
            Next
        End If
    End Sub

    Private Sub ResetFees()
        Dim participantTable As DataTable, recipientTable As DataTable
        Try
            If Me.Session_datatable_dtPecentageCount Is Nothing Then
                participantTable = Nothing
                recipientTable = Nothing
            Else
                GetBalances(participantTable, recipientTable)
                CalculateDefaultFee(participantTable, recipientTable)

                If HelperFunctions.isNonEmpty(participantTable) And HelperFunctions.isNonEmpty(recipientTable) Then
                    participantTable.Rows(0)("RetirementFee") = participantTable.Rows(0)("DefaultRetirementFee")
                    participantTable.Rows(0)("SavingsFee") = participantTable.Rows(0)("DefaultSavingsFee")

                    For Each row As DataRow In recipientTable.Rows
                        row("RetirementFee") = row("DefaultRetirementFee")
                        row("SavingsFee") = row("DefaultSavingsFee")
                    Next
                End If
            End If

            Me.ParticipantTotalBalanceAfterSplitManageFees = participantTable
            Me.RecipientBalanceAfterSplitManageFees = recipientTable
            If chkApplyFees.Checked Then
                ShowModalPopupMessage("QDRO", "MESSAGE_QDRO_FEE_RESET", "info")
            End If
        Catch
            Throw
        Finally
            recipientTable = Nothing
            participantTable = Nothing
        End Try
    End Sub

    Private Sub DoNotApplyFees()
        Dim participantTable As DataTable, recipientTable As DataTable
        chkApplyFees.Checked = False
        If HelperFunctions.isNonEmpty(Me.ParticipantTotalBalanceAfterSplitManageFees) And HelperFunctions.isNonEmpty(Me.RecipientBalanceAfterSplitManageFees) Then
            participantTable = Me.ParticipantTotalBalanceAfterSplitManageFees
            recipientTable = Me.RecipientBalanceAfterSplitManageFees

            participantTable.Rows(0)("RetirementFee") = 0
            participantTable.Rows(0)("SavingsFee") = 0

            For Each row As DataRow In recipientTable.Rows
                row("RetirementFee") = 0
                row("SavingsFee") = 0
            Next

            Me.ParticipantTotalBalanceAfterSplitManageFees = participantTable
            Me.RecipientBalanceAfterSplitManageFees = recipientTable
        End If
    End Sub

    Private Function IsZeroFeeDefined(ByRef participantFeeTable As DataTable, ByRef recipientFeeTable As DataTable) As Boolean
        Dim result As Boolean = True
        If HelperFunctions.isNonEmpty(participantFeeTable) AndAlso HelperFunctions.isNonEmpty(recipientFeeTable) Then
            If participantFeeTable.Rows(0)("RetirementFee") > 0 Then
                result = False
            ElseIf participantFeeTable.Rows(0)("SavingsFee") > 0 Then
                result = False
            Else
                For Each recipientRow As DataRow In recipientFeeTable.Rows
                    If recipientRow("RetirementFee") > 0 Then
                        result = False
                        Exit For
                    ElseIf recipientRow("SavingsFee") > 0 Then
                        result = False
                        Exit For
                    End If
                Next
            End If
        End If
        Return result
    End Function

    Private Function GetDefaultFeeAmount() As Decimal
        Dim keyData As DataSet = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("QDRO_DEFAULT_PROCESSING_FEE")
        Dim defaultFee As Decimal = 0
        If HelperFunctions.isNonEmpty(keyData) Then
            defaultFee = Convert.ToDecimal(keyData.Tables(0).Rows(0)("Value"))
        End If
        Return defaultFee
    End Function
    'END: PPP | 12/28/2016 | YRS-AT-3145 & YRS-AT-3265

    'START: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Added to disable all tabs and hide previous and next button
    Private Sub FreezeAllTabs()
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.DefineBeneficiary, False)
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.SplitAccounts, False)
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ManageFees, False)
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.ReviewAndSave, False)
        EnableDisableQDROTabStrip(YMCAObjects.EnumNonRetiredQDROTabs.Status, False)
        ShowHidePreviousButton(False)
        ShowHideNextButton(False)
    End Sub
    'END: MMR | 2017.01.23 | YRS-AT-3145 & 3265 | Added to disable all tabs and hide previous and next button
    'START : PK | 02-28-2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim tooltip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            ButtonAddBeneficiaryToList.Enabled = False
            ButtonAddBeneficiaryToList.ToolTip = tooltip

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