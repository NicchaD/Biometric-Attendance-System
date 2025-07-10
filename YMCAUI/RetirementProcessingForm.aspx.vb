
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	RetirementProcessingForm.aspx.vb
' Author Name		:	Hafiz,Prasanna Penumarthy, Asween,Ruchi Saxena(Notes and Beneficiaries tab),Dhananjay Prajapati(Gen and Fed Withholding tabs)
' Employee ID		:	33284,33733,33494,33338
' Email				:	prasannakumar.penumarthy@3i-infotech.com, ruchi.saxena@3i-infotech.com, 
' Contact No		:	8751
' Creation Time		:	5/17/2005 4:15:35 PM
' Program Specification Name	:	YMCA PS 3.5.doc
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Cache-Session                 : Hafiz 04Feb06
'************************************************************************************
'Modficiation History
'************************************************************************************
'Modified By		    Date	        Description
'************************************************************************************
' Mohammed Hafiz        04Feb06         Hafiz 04Feb06 Cache-Session
' Mohammed Hafiz        31Mar06         Enhancement Changes - Considering Termination Date for getting Annuity Types
' Mohammed Hafiz        01Aug06         YREN-2533
' Shubhrata Tripathi    02Mar07         YREN-3112 we are not supposed to include Unknown Marital Status for Federal Tax Withholding for all Tax purposes
' Asween                22Jun07         Plan Split Implementation
' Swopna                18-Jan-2008     YRPS-4535
' Nikunj Patel          2008.01.29      YRPS-4539 Changed code to load the Annuity beneficiary information when the page is first loaded. This is required so the safe harbor factors are pulled properly based on beneficiary DoB.
' Swopna                31-Jan-2008     YRPS-4535
' Mohammed Hafiz        3-Apr-2008      YRPS-4704
' Nikunj Patel          2008.05.15      BT-440 - Check whether Beneficiaries are defined properly or not before prompting for annuity purchase confirmation.
' Mohammed Hafiz        18-Dec-2008     YRS 5.0-614 Changing the message text for existing validation of last termination date.  
' Mohammed Hafiz        2009.01.06		YRS 5.0-636 
' Dilip Yadav           2009.09.18      To limit 50 characters in Note list grid as per mail received on 17-Sep-09
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 

'Ashish Srivastava      2009.11.17      change persID with fundEventID for calculating avg salary

'Neeraj Singh                   16/Nov/2009         issue YRS 5.0-940 made changes in  event ButtonSaveParticipants_Click
'Ashish Srivastava      2010.06.07      Migration changes
'Ashish Srivastava      2010.10.11      YRS 5.0-855,BT 624 and optimization of code
'Priya                  2010.11.17      YRS 5.0-1215 : Exact age vs nearest age annuity calculations
'Priya                  11/30/2010      BT-683:Wrong message shows when select new annuity
'Ashish Srivastava      2010.12.16      Both AnnuitySet in summary Tab
'Priya					12/20/2010		BT-710:Retirement Process screen message shows wrong commented text
'Shashi Shekhar         17 Feb 2011     For YRS 5.0-1236 : Need ability to freeze/lock account
'Sanket Vaidya          17 Feb 2011     For BT 665 : For disability requirement
'Sanket Vaidya          17 Feb 2011     For BT 756 : For disability requirement
'Shashi shekhar         28 Feb 2011     Replacing Header formating with user control (YRS 5.0-450 )
'Sanket Vaidya           24 Mar 2011     For YRS 5.0-1294,BT 794  : For disability requirement 
'Sanket Vaidya          12 Apr 2011     For BT-810 Error in Normal Retirement Processing after regular withdrawl
'Sanket vaidya          31 May 2011     YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero.
'Ashish Srivastava      2011.08.22      Resolve YRS 5.0-1345 :BT-859 Death benefit not available for QD participant
'Sanket Vaidya          2011.08.10      YRS 5.0-1329:J&S options available to non-spouse beneficiaries
'Ashish Srivastava      2011.11.03      BT-917 Application giving error "String was not recognized as a valid DateTime." for  RPT fund event. 
'Ashish Srivastava      2011.12.09      BT-877:Object Reference error in Retirement process on selection of Death Benefigt
'Ashish Srivastava      2011.12.09      BT-883/YRS 5.0-1353:Failing on second annutiy purchase if 1/1/2011 bal is zero
'Shashank Patel         2012.03.05      BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
'Shashank Patel         2012.05.18      BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary 
'Shashank Patel         2012.06.04      BT-975/YRS 5.0-1508 - Made chnages to handle saving beneficiary birthdate passed into annuity calculation
'Shashank Patel         2012.06.08      BT-975/YRS 5.0-1508 - Made chnages to handle index out bound error
'Shashank Patel			2012.07.03		BT-712/YRS 5.0-1246 : Handling DLIN records
'Shashank Patel			2012.07.16      BT-712/YRS 5.0-1246 : Handling DLIN records (additional internal chnages)
'Sanjay                 2012.07.19      BT-1054: Data for estimate has changed. Please re-calculate message need to be changed for retirement process.  
'Shashank Patel         2012.07.16		BT-753/YRS 5.0-1270 : purchase page
'Sanjay R.		        2012.08.01      BT-753/YRS 5.0-1270 : purchase page
'Sanjay R.              2012.08.06      BT-753/YRS 5.0-1270 : purchase page(If end date is provided then calculate gen withholdings till end date)
'Sajay R.               2012.08.08      BT-753/YRS 5.0-1270 : purchase page(Consider end date when last payroll date is greater then end date)
'Anudeep A              2012-09-22      BT-1126/Additional change YRS 5.0-1246 : : Handling DLIN records
'Anudeep A              2012-11-30      Bt-1462-Instead of Showing "***" is better to show a help Image
'Anudeep            	2012-06-23 		BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep                2013-07-11      BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep                2013-07-17      BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Sanjay R.              2013.08.05      YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Anudeep                2013.08.22      YRS 5.0-1862:Add notes record when user enters address in any module.
'Anudeep                2013.08.05      YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Anudeep                2013.10.29      BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Shashank				2013.12.13		BT-2326: Incorporating master page & exception logging.
'Shashank				2013.12.19		BT:2131/YRS 5.0-2161 : Modifications to Annuity Purchase process 
'Shashank				2013.12.26		BT-2340 -Address not getting cleared in Retirement Process - Annuity Benificiary tab
'Shashank				2012.12.26		BT-2111/YRS 5.0-2144:Rounding error on first check display (changed datatype from double to decimal)
'Anudeep                2014.02.16      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep                2014.02.19      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Shashank	            2014.02.20      BT:2436 :Retirement processing observations find attachment.
'Anudeep                2014.05.26      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep                2014.06.03      BT:2556:Problem in JS Annuity Purchase with Phony SSN beneficiary.
'Anudeep                2014.06.11      BT:2554:YRS 5.0-2375 : Notes display if you have spaces in name.
'Shashank               2014.09.17      BT-2529/YRS 5.0-2362 : Annuity Purchase not blocking J1 and J5 annuties completely
'Shashank               2014.12.09      BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
'Anudeep                2015.05.05      BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite
'Manthan Rajguru        2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod Prakash Pokale  2015.09.25      YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)
'Gunanithi G            2015.11.19      YRS-AT-2676: YRS enh: Annuity Estimate - change for Retired Death Benefit, use new effective date 1/1/2019 for calculations.
'Chandra sekar.c        2016.01.19      YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
'Chandra sekar.c        2016.03.01      YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)
'Anudeep A              2016.03.16      YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)
'Palanivel P            2016.03.22      YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)
'Chandra sekar.c        2016.04.08      YRS-AT-2915 - YRS Bug: Annuity purchase screen no longer pre-filling annuity beneficiary (TrackIT 25836)
'Manthan Rajguru        2016.07.18      YRS-AT-2919 -  YRS Enh: Beneficiary Details - nonPerson - should allow optional Date
'Santosh Bura           2016.07.21      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annty bene SSN(TrackIT 19856) 
'Pramod Prakash Pokale  2016.08.01      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annty bene SSN(TrackIT 19856)
'Manthan Rajguru        2016.08.02      YRS-AT-2560 - YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Sanjay Singh           2016.08.02      YRS-AT-2382 - Capture 'Reason' for change of beneficiary SSN and Annty bene SSN(TrackIT 19856)
'Manthan Rajguru        2016.09.06      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Manthan Rajguru        2017.03.03      YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012)
'Pramod Prakash Pokale  2017.03.08      YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012) 
'Santosh Bura           2017.03.09      YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012) 
'Manthan Rajguru        2017.03.24      YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012)  
'Sanjay GS Rawat        2017.04.07      YRS-AT-3390 - YRS bug: Annuity calculations Estimates Screen (TrackIT 24012) 
'Pramod Prakash Pokale  2017.05.02      YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012) \
'Manthan Rajguru        2018.05.03      YRS-AT-3941 - YRS enh: change "phony SSN" beneficiary label to "placeholder SSN" (TrackIT 33287)
'Benhan David           2018.11.01      YRS-AT-4135 - YRS enh: YRS Annuity Purchase screen: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
'Pooja K                2019.28.02      YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'Santosh Bura           2019.03.15      BT-12078    - Error message is thrown when user enter a notes information in Retirement prcoessing tab-- Cannot set Column 'bitImportant' to be null. Please use DBNull instead. 
'Megha Lad              2019.09.20      YRS-AT-4597 - YRS enh: State Withholding Project - First Annuity Payments (UI design)
'Pooja K                2019.09.20      YRS-AT-4597 - YRS enh: State Withholding Project - First Annuity Payments (UI design)
'Megha Lad              2019.11.27      YRS-AT-4719 - State Withholding - Additional text & warning messages for AL, CA and MA.
'**************************************************************************************************************************************************
#Region "Imports"
Imports YMCARET.YmcaBusinessObject
Imports System
Imports System.Math
Imports System.Text
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Drawing
Imports YMCAUI.Resources
Imports System.Web.Services 'MMR | 2017.03.03 | YRS-AT-2625 | Added namespace to use web attributes 

#End Region

Public Class RetirementProcessingForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    'Dim strFormName As String = New String("FindInfo.aspx?Name=Process") 'Shashi Shekhar:2009-12-17:Change strFormName value  
    Dim strFormName 'PK| 02.21.2019 | YRS-AT-4248| making strFormName as global variable
    'End issue id YRS 5.0-940

#Region " Declarations for variables"
    Public Enum enumMessageBoxType
        Javascript = 0
        DotNet = 1
    End Enum

    Dim BORetireeEstimate As New RetirementBOClass
    Dim tnFullBalance As Decimal
    Dim lcAnnuitiesListCursor As DataTable
    Dim lcAnnuitiesFullBalanceList As DataTable
    Dim dtAnnuitieslistComboTmp As New DataTable
    Dim ycRetireeBirthDay As String
    Protected WithEvents ButtonFormOK As System.Web.UI.WebControls.Button
    Protected WithEvents CheckBoxRetPlan As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxSavPlan As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxIncludeSSLevelling As System.Web.UI.WebControls.CheckBox
    ' Protected WithEvents YMCA_Toolbar_WebUserControl1 As YMCAUI.YMCA_Toolbar_WebUserControl
    Protected WithEvents RequiredFieldValidatorAnnuityBenSSNoRet As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents YMCA_Footer_WebUserControl1 As YMCAUI.YMCA_Footer_WebUserControl

    '2012.06.04  SP:  BT-975/YRS 5.0-1508 - Made changes -start
    Protected WithEvents RequiredFieldValidatorAnnuitySSNoSav As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredfieldvalidatorAnnuityLastNameSav As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredfieldvalidatorAnnuityFirstNameSav As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents RequiredfieldvalidatorAnnuityBirthDateSav As System.Web.UI.WebControls.RequiredFieldValidator 'PPP | 2015.09.25 | YRS-AT-2596
    'Protected WithEvents PopcalendarSaving As RJS.Web.WebControl.PopCalendar 'PPP | 2015.09.25 | YRS-AT-2596
    Protected WithEvents LabelRealtionSav As Label
    Protected WithEvents LabelNoBeneficiarySav As Label
    Protected WithEvents DropDownRelationShipSav As DropDownList
    Protected WithEvents ButtonClearBeneficiarySav As Button
    Protected WithEvents DataGridAnnuityBeneficiariesSav As DataGrid
    Protected WithEvents RequiredfieldvalidatornRelationShipSav As RequiredFieldValidator
    '2012.06.04  SP:  BT-975/YRS 5.0-1508 - Made changes  -end
    Protected WithEvents RequiredFieldValidatorAnnuitySSNoRet As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredfieldvalidatorAnnuityLastNameRet As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredfieldvalidatorAnnuityFirstNameRet As System.Web.UI.WebControls.RequiredFieldValidator
    'Protected WithEvents RequiredfieldvalidatorAnnuityBirthDateRet As System.Web.UI.WebControls.RequiredFieldValidator 'PPP | 2015.09.25 | YRS-AT-2596
    Protected WithEvents TextboxExistingAnnuities As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxCurrentPurchase As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxSSLevelling As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxLevellingBefore62 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxLevellingSSBenefit As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxLevellingAfter62 As System.Web.UI.WebControls.TextBox

    Dim monthlyTotal As Decimal = 0.0
    Dim regularTotal As Decimal = 0.0
    Dim dividendTotal As Decimal = 0.0
    Dim monthlyTotWith As Decimal = 0.0
    Dim regularTotWith As Decimal = 0.0
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ValidationSummary1 As System.Web.UI.WebControls.ValidationSummary
    'Protected WithEvents LabelErrorMessage As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelAnnuityBenefeciary As System.Web.UI.WebControls.Label
    Dim dividendTotWith As Decimal = 0.0
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonSelectRet As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSelectSav As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridFederalWithholding As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridGeneralWithholding As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridNotes As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TabStripRetireesInformation As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageRetirementProcessing As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents ButtonReCalculate As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAnnuitySelect As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuitySelectRet As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxAnnuitySelectSav As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxOldRetDate As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    'Anudeep 30.11.2012 Bt-1462-Instead of Showing "***" is better to show a help Image
    'Protected WithEvents LabelDBIncluded As System.Web.UI.WebControls.Label
    Protected WithEvents imgHelp As System.Web.UI.WebControls.Image

    Protected WithEvents LabelRetirementType As System.Web.UI.WebControls.Label
    'Protected WithEvents DropDownListRetirementType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblRetirementType As System.Web.UI.WebControls.Label

    Protected WithEvents LabelRetirementDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetirementDate As YMCAUI.DateUserControl

    Protected WithEvents LabelRetirementDeathBenefit As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetiredBenefit As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelTaxableRet As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTaxableRet As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTaxableSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelPercentageToUse As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPercentage As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelNonTaxableRet As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNonTaxableRet As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxNonTaxableSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelAmountToUse As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAmount As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelTotalPaymentRet As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTotalPaymentRet As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTotalPaymentSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelTotalReservesRet As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxReservesRet As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxReservesSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelSSSalary As System.Web.UI.WebControls.Label

    Protected WithEvents LabelSSBenefit As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSBenefit As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxSSNoRet As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSSNoSav As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxLastNameRet As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxLastNameSav As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxFirstNameRet As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxFirstNameSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelSSIncrease As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSIncrease As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelMiddleName As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxMiddleNameRet As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxMiddleNameSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelStatus As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxStatus As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelSSDecrease As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSDecrease As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelBirthDateRet As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBirthDateRet As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxBirthDateSav As System.Web.UI.WebControls.TextBox


    ' Retirement benefeciary 
    'commnetd existing code for issue -1508
    Protected WithEvents LabelRetirementBeneficiary As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSavingsBenefeciary As System.Web.UI.WebControls.Label

    Protected WithEvents LabelSSNo2Ret As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuitySSNoRet As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelLastName2Ret As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityLastNameRet As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFirstName2Ret As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityFirstNameRet As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelMiddleName2Ret As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityMiddleNameRet As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelBirthDate2Ret As System.Web.UI.WebControls.Label
    'START: PPP | 2015.09.25 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)
    'Protected WithEvents TextBoxAnnuityBirthDateRet As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxAnnuityBirthDateRet As CustomControls.CalenderTextBox
    'END: PPP | 2015.09.25 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)
    Protected WithEvents spnBoxAnnuityBirthDateSav As HtmlGenericControl
    Protected WithEvents spnBoxAnnuityBirthDateRet As HtmlGenericControl
    Protected WithEvents divRetbenef As HtmlGenericControl
    Protected WithEvents divSavbenef As HtmlGenericControl
    '2012.06.04  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
    'Added new code
    Protected WithEvents LabelRealtionRet As Label
    Protected WithEvents DropDownRelationShipRet As DropDownList
    Protected WithEvents DataGridAnnuityBeneficiaries As DataGrid
    Protected WithEvents LabelNoBeneficiary As Label
    Protected WithEvents RequiredfieldvalidatornRelationShipRet As RequiredFieldValidator
    'Protected WithEvents Popcalendar3 As RJS.Web.WebControl.PopCalendar 'PPP | 2015.09.25 | YRS-AT-2596
    'Protected WithEvents LabelEstimateDataChangedMessage As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonClearBeneficiary As Button
    'commnetd existing code for issue -1508
    'Protected WithEvents LabelSpouseRet As System.Web.UI.WebControls.Label
    'Protected WithEvents chkSpouseRet As System.Web.UI.WebControls.CheckBox

    ' Savings benefeciary 
    Protected WithEvents LabelSSNo2Sav As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuitySSNoSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelLastName2Sav As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityLastNameSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFirstName2Sav As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityFirstNameSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelMiddleName2Sav As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuityMiddleNameSav As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelBirthDate2Sav As System.Web.UI.WebControls.Label
    'START: PPP | 2015.09.25 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)
    'Protected WithEvents TextBoxAnnuityBirthDateSav As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxAnnuityBirthDateSav As CustomControls.CalenderTextBox
    'END: PPP | 2015.09.25 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)

    Protected WithEvents LabelSpouseSav As System.Web.UI.WebControls.Label
    Protected WithEvents chkSpouseSav As System.Web.UI.WebControls.CheckBox
    '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End
    '
    Protected WithEvents LabelGross As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonthlyGrossAnnuity As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelMonthlyGrossDB As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonthlyGrossDB As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelMonthlyGrossTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonthlyGrossTotal As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelWithheld As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonthlyWithheld As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelNet As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonthlyNetTotal As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelGross2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstCheckGrossAnnuity As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFirstCheckGrossDB As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstCheckGrossDB As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFirstCheckGrossTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstCheckGrossTotal As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelWithheld2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstCheckWithheld As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelNet2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstCheckNet As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents PopcalendarDate As RJS.Web.WebControl.PopCalendar
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Protected WithEvents DataGridActiveBeneficiaries As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridPurchase As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridWithheld As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelPercentage1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridRetiredBeneficiaries As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelPercentage2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRetiredDBR As System.Web.UI.WebControls.Label
    Protected WithEvents LabelInsResR As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPrimaryR As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPrimaryR As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonPriR As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCont1R As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCont1R As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont1R As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCont2R As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCont2R As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont2R As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCont3R As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCont3R As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont3R As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddRetired As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditRetired As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDeleteRetired As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNotSet As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonMoveBeneficiaries As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddItemNotes As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonView As System.Web.UI.WebControls.Button
    Protected WithEvents ListBoxPercentage As System.Web.UI.WebControls.ListBox
    Protected WithEvents ButtonPurchase As System.Web.UI.WebControls.Button
    Protected WithEvents DropDownListPercentage As System.Web.UI.WebControls.DropDownList

    Protected WithEvents NotesFlag As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    '**************************************************
    ''Code Added by Dhananjay on october 31 2005 
    '**************************************************
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddGeneralWithholding As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonUpdateGeneralWithholding As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoOfMonthsInFirstCheck As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNoOfMonthsInFirstCheckValue As System.Web.UI.WebControls.Label
    'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    'Protected WithEvents lblAnnuityMessage As System.Web.UI.WebControls.Label
    'Ashish:2010.12.16 
    Protected WithEvents rdbListAnnuityOptionRet As RadioButtonList
    Protected WithEvents rdbListAnnuityOptionSav As RadioButtonList
    Protected WithEvents pnlAllowAnnuityInSummaryTab As Panel
    Protected WithEvents lblSummarySelectedAnnuityRet As System.Web.UI.WebControls.Label
    Protected WithEvents lblSummarySelectedAnnuitySav As System.Web.UI.WebControls.Label
    'Anudeep:01.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    Protected WithEvents AddressWebUserControlRet As YMCAUI.AddressUserControlNew
    Protected WithEvents AddressWebUserControlSav As YMCAUI.AddressUserControlNew
    'Protected WithEvents TrIncludeSSLevelling As System.Web.UI.WebControls.TableRow
    ' Protected WithEvents TrIncludeSSLevellingValue As System.Web.UI.WebControls.TableRow
    Protected WithEvents imgLockBeneficiary As System.Web.UI.WebControls.Image
    'SP 2013.12.13 -Bt-2326
    Protected WithEvents ButtonYes As Button
    Protected WithEvents ButtonNo As Button 'MMR | 2017.03.16 | YRS-AT-2625 | Declared control
    Protected WithEvents ButtonCancelYes As Button
    Protected WithEvents lblMessage As Label
    'AA:2014.02.03 - BT:2316:YRS 5.0-2247 - Added the Linkbutton 
    Protected WithEvents lnkParticipantAddressRet As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkParticipantAddressSav As System.Web.UI.WebControls.LinkButton
    'START: MMR | 2017.03.06 | YRS-AT-2625 | Declared controls
    Protected WithEvents DatagridManualTransactionList As DataGrid
    Protected WithEvents lnkManualTransaction As HtmlAnchor
    Protected WithEvents hdnManualTransaction As HiddenField
    Protected WithEvents hdnSourceManualTransaction As HiddenField
    Protected WithEvents DivWarningMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents hdnMessage As HiddenField
    'END: MMR | 2017.03.06 | YRS-AT-2625 | Declared controls

    'START : ML | 2019.09.20 | YRS-AT-4597 |Declare controls 
    Protected WithEvents lblStateWithholdingMessage As Label
    Public WithEvents stwListUserControl As StateWithholdingListingControl
    Protected WithEvents divErrorMsg As System.Web.UI.HtmlControls.HtmlGenericControl
    'END : ML | 2019.09.20 | YRS-AT-4597 |Declare controls 
#Region "Global Declaration"
    'Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Declared constant to store datagrid cell index
    Const mConst_DG_ActBenef_CheckboxActBen_Index As Integer = 0
    Const mConst_DG_ActBenef_UniqueId_Index As Integer = 1
    Const mConst_DG_ActBenef_PersId_Index As Integer = 2
    Const mConst_DG_ActBenef_BenePersId_Index As Integer = 3
    Const mConst_DG_ActBenef_BeneFundEventId_Index As Integer = 4
    Const mConst_DG_ActBenef_Name_Index As Integer = 5
    Const mConst_DG_ActBenef_Name2_Index As Integer = 6
    Const mConst_DG_ActBenef_TaxID_Index As Integer = 7
    Const mConst_DG_ActBenef_Rel_Index As Integer = 8
    Const mConst_DG_ActBenef_Birthdate_Index As Integer = 9
    Const mConst_DG_ActBenef_BeneficiaryTypeCode_Index As Integer = 10
    Const mConst_DG_ActBenef_Groups_Index As Integer = 11
    Const mConst_DG_ActBenef_Lvl_Index As Integer = 12
    Const mConst_DG_ActBenef_DeathFundEventStatus_Index As Integer = 13
    Const mConst_DG_ActBenef_BeneficiaryStatusCode_Index As Integer = 14
    Const mConst_DG_ActBenef_Pct_Index As Integer = 15
    Const mConst_DG_ActBenef_PlanType_Index As Integer = 16
    Const mConst_DG_ActBenef_RepFirstName_Index As Integer = 17
    Const mConst_DG_ActBenef_RepLastName_Index As Integer = 18
    Const mConst_DG_ActBenef_RepSalutation_Index As Integer = 19
    Const mConst_DG_ActBenef_RepTelephone_Index As Integer = 20
    Const BENEFICIARY_SSNo As Integer = 2  'Manthan Rajguru | 2016.09.06 | YRS-AT-2560 | Defined constant to store cell index
    'End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Declared constant to store datagrid cell index
#End Region


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " All Properties "
    Public Property MonthsinCheckOne() As Integer
        Get
            If Not Session("RP_MonthsinCheckOne") Is Nothing Then
                Return CType(Session("RP_MonthsinCheckOne"), Integer)
            Else
                Return 1
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("RP_MonthsinCheckOne") = Value
        End Set
    End Property
    Public Property Notes() As DataTable
        Get
            Return CType(Session("dtNotes"), DataTable)
        End Get
        Set(ByVal Value As DataTable)
            Session("dtNotes") = Value
        End Set
    End Property
    Public ReadOnly Property guiFundEventId() As String
        Get
            If Not Session("FundId") Is Nothing Then
                Return CType(Session("FundId"), String)
            Else
                Return String.Empty
            End If
        End Get
    End Property
    Public Property PersId() As String
        Get
            If Not Session("RP_PersId") Is Nothing Then
                Return (CType(Session("RP_PersId"), String))
            Else
                Return String.Empty
            End If
        End Get

        Set(ByVal Value As String)
            Session("RP_PersId") = Value
            Session("PersId") = Value
        End Set
    End Property
    Public Property ElectiveAccounts() As DataSet
        Get
            If Not Session("RP_dsElectiveAccounts") Is Nothing Then
                Return (CType(Session("RP_dsElectiveAccounts"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("RP_dsElectiveAccounts") = Value
        End Set
    End Property
    Public Property BeneficiaryInfo() As DataSet
        Get
            If Not Session("RP_BeneficiaryInfo") Is Nothing Then
                Return (CType(Session("RP_BeneficiaryInfo"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("RP_BeneficiaryInfo") = Value
        End Set
    End Property
    Public Property PersonDetails() As DataSet
        Get
            Return CType(Session("PersonDetails"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("PersonDetails") = Value
        End Set
    End Property
    Public Property Person_Info() As String
        Get
            If Not Session("Person_Info") Is Nothing Then
                Return CType(Session("Person_Info"), String)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("Person_Info") = Value
        End Set
    End Property
    Public Property LoggedUserKey() As String
        Get
            If Not Session("LoggedUserKey") Is Nothing Then
                Return CType(Session("LoggedUserKey"), String)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("LoggedUserKey") = Value
        End Set
    End Property
    Public Property AnnuitySelected() As Boolean
        Get
            Return CType(Session("AnnuitySelected"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("AnnuitySelected") = Value
        End Set
    End Property
    Public Property SelectedPlan() As String
        Get
            Return CType(Session("SelectedPlan"), String)
        End Get
        Set(ByVal Value As String)
            Session("SelectedPlan") = Value
        End Set
    End Property
    Public Property Flag() As String
        Get
            Return CType(Session("Flag"), String)
        End Get
        Set(ByVal Value As String)
            Session("Flag") = Value
        End Set
    End Property
    Public Property LastSalPaidDate() As String
        Get
            If Not Session("LastSalPaidDate") Is Nothing Then
                Return Session("LastSalPaidDate")
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("LastSalPaidDate") = Value
        End Set
    End Property
    Public Property SSNo() As String
        Get
            Return CType(Session("SSNo"), String)
        End Get
        Set(ByVal Value As String)
            Session("SSNo") = Value
        End Set
    End Property
    Public Property RetEmpInfo() As DataSet
        Get
            Return CType(Session("dsRetEmpInfo"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("dsRetEmpInfo") = Value
        End Set
    End Property
    Public Property EndWorkDate() As String
        Get
            If Session("EndWorkDate") Is Nothing Then
                Return CType(Session("EndWorkDate"), String)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("EndWorkDate") = Value
        End Set
    End Property
    Public Property RetEstimateEmployment() As DataSet
        Get
            Return CType(Session("RetEstimateEmployment"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("RetEstimateEmployment") = Value
        End Set
    End Property
    Public Property ValidationMessage() As String
        Get
            If Not Session("ValidationMessage") Is Nothing Then
                Return CType(Session("ValidationMessage"), String)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("ValidationMessage") = Value
        End Set
    End Property
    Public Property blnAddFedWithHoldings() As Boolean
        Get
            Return CType(Session("blnAddFedWithHoldings"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("blnAddFedWithHoldings") = Value
        End Set
    End Property
    Public Property blnUpdateFedWithHoldings() As Boolean
        Get
            Return CType(Session("blnUpdateFedWithHoldings"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("blnUpdateFedWithHoldings") = Value
        End Set
    End Property
    Public Property blnAddGenWithHoldings() As Boolean
        Get
            Return CType(Session("blnAddGenWithHoldings"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("blnAddGenWithHoldings") = Value
        End Set
    End Property
    Public Property blnUpdateGenWithDrawals() As Boolean
        Get
            Return CType(Session("blnUpdateGenWithDrawals"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("blnUpdateGenWithDrawals") = Value
        End Set
    End Property
    Public Property SelectAnnuityCurrent() As DataSet
        Get
            Return CType(Session("ds_SelectAnnuity"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("ds_SelectAnnuity") = Value
        End Set
    End Property
    Public Property SelectAnnuityRetirement() As DataSet
        Get
            Return CType(Session("ds_SelectAnnuityRetirement"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("ds_SelectAnnuityRetirement") = Value
        End Set
    End Property
    Public Property SelectAnnuitySavings() As DataSet
        Get
            Return CType(Session("ds_SelectAnnuitySavings"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("ds_SelectAnnuitySavings") = Value
        End Set
    End Property
    Public Property blnPurchaseSuccessful() As Boolean
        Get
            Return CType(Session("blnSuccessful"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("blnSuccessful") = Value
        End Set
    End Property
    Public Property RetirementDate() As String
        Get
            Return CType(Session("TextBoxRetirementDate.Text"), String)
        End Get
        Set(ByVal Value As String)
            Session("TextBoxRetirementDate.Text") = Value
        End Set
    End Property
    'SANKET:03/24/2011 code for YRS 5.0-1294 
    Public Property TerminationDate() As String
        Get
            Return CType(Session("TerminationDate"), String)
        End Get
        Set(ByVal Value As String)
            Session("TerminationDate") = Value
        End Set
    End Property


    Public Property iCounter() As Integer
        Get
            Return CType(Session("iCounter"), Integer)
        End Get
        Set(ByVal Value As Integer)
            Session("iCounter") = Value
        End Set
    End Property
    Public Property Blank() As Boolean
        Get
            Return CType(Session("Blank"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("Blank") = Value
        End Set
    End Property
    Public Property BeneficiariesActive() As DataSet
        Get
            Return CType(Session("BeneficiariesActive"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("BeneficiariesActive") = Value
        End Set
    End Property
    Public Property BeneficiariesRetired() As DataSet
        Get
            Return CType(Session("BeneficiariesRetired"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("BeneficiariesRetired") = Value
        End Set
    End Property
    Public Property RetireType() As String
        Get
            Return CType(Session("RP_RetireType"), String)
        End Get
        Set(ByVal Value As String)
            Session("RP_RetireType") = Value
        End Set
    End Property
    'Sanket Vaidya          17 Feb 2011     For BT 756 : For disability requirement
    'Public Property CustomRetireType() As String
    '    Get
    '        Return CType(Session("RP_CustomRetireType"), String)
    '    End Get
    '    Set(ByVal Value As String)
    '        Session("RP_CustomRetireType") = Value
    '    End Set
    'End Property
    Public Property FedWithDrawals() As DataSet
        Get
            Return CType(Session("FedWithDrawals"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("FedWithDrawals") = Value
        End Set
    End Property
    Public Property GenWithDrawals() As DataSet
        Get
            Return CType(Session("GenWithDrawals"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("GenWithDrawals") = Value
        End Set
    End Property
    Public Property TaxEntityTypes() As DataSet
        Get
            Return CType(Session("TaxEntityTypes"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("TaxEntityTypes") = Value
        End Set
    End Property
    Public Property TaxFactors() As DataSet
        Get
            Return CType(Session("TaxFactors"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("TaxFactors") = Value
        End Set
    End Property
    Public Property WITHHOLDING_DEFAULT_EXEMPTIONS() As Double
        Get
            Return CType(Session("WITHHOLDING_DEFAULT_EXEMPTIONS"), Double)
        End Get
        Set(ByVal Value As Double)
            Session("WITHHOLDING_DEFAULT_EXEMPTIONS") = Value
        End Set
    End Property
    Public Property WITHHOLDING_DEFAULT_MARRIAGE_STATUS() As String
        Get
            Return CType(Session("WITHHOLDING_DEFAULT_MARRIAGE_STATUS"), String)
        End Get
        Set(ByVal Value As String)
            Session("WITHHOLDING_DEFAULT_MARRIAGE_STATUS") = Value
        End Set
    End Property
    Public Property WITHHOLDING_MAXIMUM_VAID_EXEMPTIONS() As Double
        Get
            Return CType(Session("WITHHOLDING_MAXIMUM_VAID_EXEMPTIONS"), Double)
        End Get
        Set(ByVal Value As Double)
            Session("WITHHOLDING_MAXIMUM_VAID_EXEMPTIONS") = Value
        End Set
    End Property
    Public Property LastPayrollDate() As String
        Get
            Return CType(Session("LastPayrollDate"), String)
        End Get
        Set(ByVal Value As String)
            Session("LastPayrollDate") = Value
        End Set
    End Property
    Public Property DataChanged() As Boolean
        Get
            Return CType(Session("RP_DataChanged"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("RP_DataChanged") = Value
        End Set
    End Property
    'To Get to know if the logged in User belongs to the notes Admin group
    Public Property NotesGroupUser() As Boolean
        Get
            If Not (Session("NotesGroupUser")) Is Nothing Then
                Return (CType(Session("NotesGroupUser"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("NotesGroupUser") = Value
        End Set
    End Property
    Public Property IsPrePlanSplitRetirement() As Boolean
        Get
            Return CType(Session("IsPrePlanSplitRetirement"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("IsPrePlanSplitRetirement") = Value
        End Set
    End Property
    Public Property FundEventStatus() As String
        Get
            Return Session("RE_FundEventStatus")
        End Get
        Set(ByVal Value As String)
            Session("RE_FundEventStatus") = Value
        End Set
    End Property
    'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    Public Property TransAfterExactAgeEffDate() As String
        Get
            Return Session("TransAfterExactAgeEffDate")
        End Get
        Set(ByVal Value As String)
            Session("TransAfterExactAgeEffDate") = Value
        End Set
    End Property
    'End 2010.11.17

    'Priya 2010.11.18 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    Public Property ExactAgeEffDate() As String
        Get
            Return Session("ExactAgeEffDate")
        End Get
        Set(ByVal Value As String)
            Session("ExactAgeEffDate") = Value
        End Set
    End Property

    Public Property SelectAnnuityRetirementExactAgeEffDate() As DataSet
        Get
            Return CType(Session("ds_SelectAnnuityRetirement_ExactAgeEffDate"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("ds_SelectAnnuityRetirement_ExactAgeEffDate") = Value
        End Set
    End Property
    Public Property SelectAnnuitySavingsExactAgeEffDate() As DataSet
        Get
            Return CType(Session("ds_SelectAnnuitySavings_ExactAgeEffDate"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("ds_SelectAnnuitySavings_ExactAgeEffDate") = Value
        End Set
    End Property
    Public Property SelectAnnuityCombinedRetExactAgeEffDate() As DataSet
        Get
            Return CType(Session("ds_SelectAnnuityRet_ExactAgeEffDate_Combined"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("ds_SelectAnnuityRet_ExactAgeEffDate_Combined") = Value
        End Set
    End Property
    Public Property SelectAnnuityCombinedSavExactAgeEffDate() As DataSet
        Get
            Return CType(Session("ds_SelectAnnuitySavings_ExactAgeEffDate_Combined"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("ds_SelectAnnuitySavings_ExactAgeEffDate_Combined") = Value
        End Set
    End Property
    'End 2010.11.17
    'ASHISH:2011.08.24 Added new property for YRS 5.0-1135
    Public Property OrgBenTypeIsQDRO() As Boolean
        Get
            If Session("OrgBenTypeIsQDRO") Is Nothing Then
                Return False
            Else
                Return CType(Session("OrgBenTypeIsQDRO"), Boolean)
            End If

        End Get
        Set(ByVal Value As Boolean)
            Session("OrgBenTypeIsQDRO") = Value
        End Set
    End Property

    'Property to set Retiree Birth Date is present or not
    Public Property RetireeBirthDatePresent() As Boolean
        Get
            If Session("RetireeBirthDatePresent") Is Nothing Then
                Return False
            Else
                Return CType(Session("RetireeBirthDatePresent"), Boolean)
            End If

        End Get
        Set(ByVal Value As Boolean)
            Session("RetireeBirthDatePresent") = Value
        End Set
    End Property
    'Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    Public Property BeneficiaryAddress() As DataTable
        Get
            Return CType(Session("BeneficiaryAddress"), DataTable)
        End Get
        Set(ByVal Value As DataTable)
            Session("BeneficiaryAddress") = Value
        End Set
    End Property
    'Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    'SP 2013.12.18 BT-2326 - Used to set Master Header Details 
    Public Property PersonInformationForMenuHeader() As String
        Get
            Return ViewState("PersonInformationForMenuHeader")
        End Get
        Set(ByVal Value As String)
            ViewState("PersonInformationForMenuHeader") = Value
        End Set
    End Property
    'Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 

    'Guna -2015-12-08 : YRS-AT-2676: fetching the configured restricted date  : START
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
    'Guna -2015-12-08 : YRS-AT-2676: fetching minimum age to retire  : START
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

    'Guna -2015-12-08 : YRS-AT-2676: fetching configured restriction date : START
    Public Property IsDeathBenefitAnnuityPurchaseRestricted() As Boolean
        Get
            If ViewState("IsDeathBenefitAnnuityPurchaseRestricted") Is Nothing Then
                ViewState("IsDeathBenefitAnnuityPurchaseRestricted") = RetirementBOClass.IsDeathBenefitAnnuityPurchaseRestricted(Me.MinimumAgeToRetire, Convert.ToDateTime(TextBoxBirthDateRet.Text), Me.DeathBenefitAnnuityPurchaseRestrictedDate)

            End If
            Return CType(ViewState("IsDeathBenefitAnnuityPurchaseRestricted"), Boolean)

        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsDeathBenefitAnnuityPurchaseRestricted") = Value
        End Set
    End Property

    'START: MMR | 2017.03.06 | YRS-AT-2625 | Declared property for manual transaction details
    Public Property ManualTransactionDetails() As DataSet
        Get
            If Session("ManualTransaction") Is Nothing Then
                Return Nothing
            Else
                Return CType(Session("ManualTransaction"), DataSet)
            End If

        End Get
        Set(ByVal Value As DataSet)
            Session("ManualTransaction") = Value
        End Set
    End Property
    'END: MMR | 2017.03.06 | YRS-AT-2625 | Declared property for manual transaction details

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
#End Region

#Region " Custom Methods "


#Region "ExactAgeAnnuity Method"
    'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    'It will set called annuity session value to calculate base on old logic or new logic i.e. exact age aeefective date
    Private Function CalcBasedOnRetirementDate() As String
        Dim dtmRetirementDate As DateTime
        Dim strCalledAnnuity As String = String.Empty
        Try
            dtmRetirementDate = TextBoxRetirementDate.Text.ToString()
            Me.ExactAgeEffDate = YMCARET.YmcaBusinessObject.RetirementBOClass.GetExactAgeEffectiveDate()

            If Not IsNothing(dtmRetirementDate) Then
                If Me.ExactAgeEffDate <> "" Or Not Me.ExactAgeEffDate Is Nothing Then

                    If dtmRetirementDate < Convert.ToDateTime(Me.ExactAgeEffDate) Then
                        'Session("CalledAnnuity") = "OLD"
                        strCalledAnnuity = "OLD"
                    ElseIf dtmRetirementDate >= Convert.ToDateTime(Me.ExactAgeEffDate) AndAlso dtmRetirementDate < (Convert.ToDateTime(Me.ExactAgeEffDate).AddMonths(6)) Then
                        strCalledAnnuity = "BOTH"
                        'Session("CalledAnnuity") = "BOTH"
                    ElseIf dtmRetirementDate >= (Convert.ToDateTime(Me.ExactAgeEffDate).AddMonths(6)) Then
                        strCalledAnnuity = "NEW"
                        'Session("CalledAnnuity") = "NEW"
                    End If

                    'If Session("CalledAnnuity") = Nothing Then
                    '    Throw New Exception("Need to check wich annuity is calculated")
                    'End If
                    If Not Session("CalledAnnuity") Is Nothing Then
                        If strCalledAnnuity <> Session("CalledAnnuity") Then
                            Session("SelectedCalledAnnuity_Sav") = Nothing
                            Session("SelectedCalledAnnuity_Ret") = Nothing
                        End If
                    End If
                    Session("CalledAnnuity") = strCalledAnnuity
                End If
            End If



            Return Convert.ToString(Session("CalledAnnuity"))

        Catch ex As Exception
            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,start

            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,end
            Throw ex
        End Try
    End Function 'End Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    'Priya 2010.11.18 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    'Private Sub calculateAnnuity_ExactAgeEffDate(ByVal planType As String)
    '    ' Clear the session variables.
    '    Session("TextBoxMonthlyGrossAnnuityRet.Text") = 0
    '    Session("TextBoxMonthlyGrossAnnuitySav.Text") = 0
    '    Session("TextBoxMonthlyGrossDBRet.Text") = 0
    '    Session("TextBoxMonthlyGrossTotalRet.Text") = 0
    '    Session("TextBoxMonthlyGrossTotalSav.Text") = 0
    '    TextBoxSSIncrease.Text = "0.00"
    '    ' We can introduce the Account Balance Check here.
    '    'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    '    TransAfterExactAgeEffDate = YMCARET.YmcaBusinessObject.RetirementBOClass.ValidationForAfterExactAgeEff(guiFundEventId)
    '    If TransAfterExactAgeEffDate = 1 Then
    '        'Calculate 2 aanuity menthod
    '        'LabelForTransactions = "Withrawals are exists after 1/1/2011."
    '    End If
    '    'End Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations


    '    If (planType = "B" Or planType = "R") And CheckBoxRetPlan.Checked Then
    '        GetProjectedAnnuities_ExactAgeEffDate("R")
    '    End If
    '    If (planType = "B" Or planType = "S") And CheckBoxSavPlan.Checked Then
    '        GetProjectedAnnuities_ExactAgeEffDate("S")
    '    End If

    '    Me.loadSSLevellingControls()
    '    displayPurchaseInfo()

    'End Sub
    'End Priya 2010.11.18 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    'Priya 2010.11.18 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    Private Sub GetProjectedAnnuities_ExactAgeEffDate(ByVal planType As String, ByVal para_RetireType As String)
        Dim dsRetEstimateEmployment As DataSet
        'dsRetEstimateEmployment = RetirementBOClass.getRetEstimateEmployment(Me.guiFundEventId, Me.RetireType, Me.RetirementDate)
        'ASHISH:2009.11.17 -Commented 
        'dsRetEstimateEmployment = RetirementBOClass.getEmploymentDetails(Me.guiFundEventId, Me.RetireType, Me.RetirementDate)

        Dim dsElectiveAccounts As DataSet
        Dim hasNoErrors As Boolean
        Dim combinedDataset As DataSet
        Dim errorMessage As String
        Dim businessLogicExactAge As RetirementBOClass

        '2012.05.25  SP:  BT-975/YRS 5.0-1508 
        Dim dsSelectedBeneficiary As DataSet

        Try

            dsElectiveAccounts = getElectiveAccounts(planType)

            '2012.06.04  SP:  YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary  -
            If planType.ToUpper() = "R" Then
                If HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiary")) Then
                    dsSelectedBeneficiary = CType(Session("dsSelectedParticipantBeneficiary"), DataSet).Copy()
                Else
                    dsSelectedBeneficiary = New DataSet()
                    dsSelectedBeneficiary.Tables.Add(BeneficiaryInfo.Tables(0).Clone())
                End If
            ElseIf planType.ToUpper() = "S" Then
                If HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiarySav")) Then
                    dsSelectedBeneficiary = CType(Session("dsSelectedParticipantBeneficiarySav"), DataSet).Copy()
                Else
                    dsSelectedBeneficiary = New DataSet()
                    dsSelectedBeneficiary.Tables.Add(BeneficiaryInfo.Tables(0).Clone())
                End If
            End If

            '2012.06.04  SP:  YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary  - 

            'Step 1. Calculate the Account Balances
            combinedDataset = New DataSet
            businessLogicExactAge = New RetirementBOClass
            'Commented by Ashish for phase V part III changes,start

            'hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
            ', ycRetireeBirthDay, "0", Me.RetirementDate, String.Empty, String.Empty, String.Empty _
            ', Me.PersId, Me.guiFundEventId, Me.RetireType, 0, dsElectiveAccounts, 0, combinedDataset, False, planType, "", errorMessage)
            'Commented by Ashish for phase V part III changes,end
            'Added by Ashish for phase V part III changes,start
            hasNoErrors = businessLogicExactAge.CalculateAccountBalances(ycRetireeBirthDay, Me.RetirementDate _
            , Me.PersId, Me.guiFundEventId, para_RetireType, planType, errorMessage)
            'Added by Ashish for phase V part III changes,end

            'Check if any error has been reported.  
            If Not hasNoErrors Then
                'ShowCustomMessage(errorMessage, enumMessageBoxType.DotNet)
                HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing)    'SP 2013.12.13 BT:2326
                Exit Sub
            End If

            'Added by Ashish for phase V part III changes
            'Find Max Annuitized factor and update g_dtAcctBalancesByBasis
            'START: PPP | 05/02/2017 | YRS-AT-2625 | In case Disability, annuity basis type of savings plan must be preserved, so passing retirement type as NORMAL
            'businessLogicExactAge.UpdateBasisTypeAsPerAnnuitizeFactor(para_RetireType, Me.RetirementDate)
            If para_RetireType.ToUpper() = "DISABL" Then
                If planType.ToUpper = "S" Then
                    businessLogicExactAge.UpdateBasisTypeAsPerAnnuitizeFactor("NORMAL", Me.RetirementDate)
                Else
                    businessLogicExactAge.UpdateBasisTypeAsPerAnnuitizeFactor(para_RetireType, Me.RetirementDate)
                End If
            Else
                businessLogicExactAge.UpdateBasisTypeAsPerAnnuitizeFactor(para_RetireType, Me.RetirementDate)
            End If
            'END: PPP | 05/02/2017 | YRS-AT-2625 | In case Disability, annuity basis type of savings plan must be preserved, so passing retirement type as NORMAL

            'Step 2. Calculate Death Benefits Only if Retirement Plan is selected.
            'No Cahnges 
            tnFullBalance = 0
            'ASHISH:2011.08.29 Resolve YRS 5.0-1345 :BT-859
            If planType = "R" And Me.OrgBenTypeIsQDRO = False Then
                'START : BD : 2018.11.01 : YRS-AT-4135 - No death benefit for particpants enrolled on or after 1/1/2019
                Dim stRDBwarningMessage As String = String.Empty
                tnFullBalance = businessLogicExactAge.GetRetiredDeathBenefit(para_RetireType, Convert.ToDateTime(RetirementDate), Convert.ToDateTime(TextBoxBirthDateRet.Text), Me.guiFundEventId, stRDBwarningMessage) 'BD YRS-AT-4135 - Adding fund event id to check 2019 plan rule change and getting the RDB message 
                tnFullBalance = Math.Round(tnFullBalance, 2)
                Session("RDBwarningMessage") = Nothing
                If Not String.IsNullOrEmpty(stRDBwarningMessage) Then
                    If Not businessLogicExactAge.IsEffectiveDateNull(Me.guiFundEventId) Then
                        Session("RDBwarningMessage") = stRDBwarningMessage
                    End If
                End If
                enableRetirementPlanControls(True)
                'END : BD : 2018.11.01 : YRS-AT-4135 - No death benefit for particpants enrolled on or after 1/1/2019
                Session("CalculatedDB_ExactAgeEffDate") = Math.Round(tnFullBalance, 2)
                'Ashish: 2011.0824
                ' TextBoxRetiredBenefit.Text = Math.Round(tnFullBalance, 2)
                'TextBoxAmount.Text = Math.Round(Convert.ToDecimal((TextBoxRetiredBenefit.Text) * (DropDownListPercentage.SelectedValue) / 100), 2)

                tnFullBalance = IIf(String.IsNullOrEmpty(TextBoxAmount.Text), 0, Convert.ToDecimal(TextBoxAmount.Text))
                'tnFullBalance = Math.Round((tnFullBalance * (DropDownListPercentage.SelectedValue) / 100), 2) 'Convert.ToDecimal(TextBoxAmount.Text)

                'If tnFullBalance > 0 Then
                '    LabelDBIncluded.Visible = True
                'Else
                '    LabelDBIncluded.Visible = False
                'End If
            End If

            'Step 3. Calculate the Annuities
            Dim dtAnnuitiesList As DataTable
            Dim dtAnnuitiesFullBalanceParam As DataTable
            Dim dtAnnuitiesParam As DataTable
            Dim benDOB As DateTime
            If planType = "R" Then
                If TextBoxAnnuityBirthDateRet.Text <> String.Empty Then
                    benDOB = Convert.ToDateTime(TextBoxAnnuityBirthDateRet.Text)
                Else
                    benDOB = New DateTime(1900, 1, 1)
                End If
            Else '2012.06.04   SP:   BT-975/YRS 5.0-1508 
                If TextBoxAnnuityBirthDateSav.Text <> String.Empty Then
                    benDOB = Convert.ToDateTime(TextBoxAnnuityBirthDateSav.Text)
                Else
                    benDOB = New DateTime(1900, 1, 1)
                End If
            End If

            'If TextBoxSSBenefit.Text.Trim() = String.Empty Then
            '    TextBoxSSBenefit.Text = "0"
            'End If
            'Call Exact age annuity methid of BO = CalculateAnnuitiesExactA
            Dim finalAnnuity As Double
            '2012.05.25  SP:  BT-975/YRS 5.0-1508 - Paasing selected beneficiary
            dtAnnuitiesList = businessLogicExactAge.CalculateAnnuitiesWithExactAge(0, para_RetireType _
            , benDOB, Convert.ToDateTime(TextBoxBirthDateRet.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
            , dsSelectedBeneficiary _
            , Convert.ToDecimal(TextBoxAmount.Text), Session("SSNo"), Session("PersID") _
            , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)

            'Commented by Ashish for phase V part III changes
            'If TextBoxAnnuitySelectRet.Text.Trim.ToUpper = "M" And CheckBoxRetPlan.Checked = True And planType = "R" Then
            '    TextBoxTotalPaymentRet.Text = Math.Round(finalAnnuity, 2).ToString("#0.00")
            'End If

            'If TextBoxAnnuitySelectSav.Text.Trim.ToUpper = "M" And CheckBoxSavPlan.Checked = True And planType = "S" Then
            '    TextBoxTotalPaymentSav.Text = Math.Round(finalAnnuity, 2).ToString("#0.00")
            'End If

            'Step 3.1 Get the Combo table
            Dim comboTable As DataTable
            Dim isFullBalancePassed As Boolean
            If tnFullBalance > 0 Then
                isFullBalancePassed = True
            Else
                isFullBalancePassed = False
            End If

            If (isFullBalancePassed And planType = "R") Then
                Dim dtAnnuitiesListWithDeathBenefit As DataTable
                '2012.05.25  SP:  BT-975/YRS 5.0-1508 - Paasing selected beneficiary
                dtAnnuitiesListWithDeathBenefit = businessLogicExactAge.CalculateAnnuitiesWithExactAge(tnFullBalance, para_RetireType _
                , benDOB, Convert.ToDateTime(TextBoxBirthDateRet.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                , dsSelectedBeneficiary _
                , Convert.ToDecimal(TextBoxAmount.Text), Session("SSNo"), Session("PersID") _
                , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                comboTable = RetirementBOClass.CreateComboTable(dtAnnuitiesList, dtAnnuitiesListWithDeathBenefit)
                Session("lcAnnuitiesListCursor_ExactAgeEffDate") = comboTable
                Session("dtAnnuitiesFullBalanceList_ExactAgeEffDate") = dtAnnuitiesListWithDeathBenefit
            Else
                Session("lcAnnuitiesListCursor_ExactAgeEffDate") = dtAnnuitiesList.Copy()
            End If

            'Step 4 SS levelling
            If (CheckboxIncludeSSLevelling.Checked = True) And (Convert.ToDouble(TextBoxSSBenefit.Text) > 0) Then
                'If Retirement Plan is selected then Attach the SS annuity to it
                If planType = "R" And CheckBoxRetPlan.Checked = True Then
                    dtAnnuitiesList = SetSSLeveling(dtAnnuitiesList, TextBoxAnnuitySelectRet.Text)
                    Session("lcAnnuitiesListCursor_ExactAgeEffDate") = SetSSLeveling(Session("lcAnnuitiesListCursor_ExactAgeEffDate"), TextBoxAnnuitySelectRet.Text)
                ElseIf planType = "S" And CheckBoxRetPlan.Checked = False Then 'If only Savings Plan is selected then attatch SS to it
                    dtAnnuitiesList = SetSSLeveling(dtAnnuitiesList, TextBoxAnnuitySelectSav.Text)
                    Session("lcAnnuitiesListCursor_ExactAgeEffDate") = SetSSLeveling(Session("lcAnnuitiesListCursor_ExactAgeEffDate"), TextBoxAnnuitySelectSav.Text)
                End If
            End If

            ''Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Need to add this session to Compare method according to that set Session("dtAnnuitieslistComboTmp") values whether is old or new
            'temparary shifted to CompaireAnnuities() function with comments
            If planType = "R" Then
                Session("dtAnnuityList_ExactAgeEffDate") = dtAnnuitiesList
            Else
                Session("dtAnnuityListSav_ExactAgeEffDate") = dtAnnuitiesList
            End If

            'Step 5 Calculate the Payments
            CalculatePayments_ExactAgeEffDate(planType)

            ''commented by Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Below commented code shifted to SetValues funcation to set values for 
            ''Step 6 Assigning values to the fields on purchase tab
            'Dim calculateDeathBenefitAnnuity As Boolean
            'If Convert.ToDecimal(TextBoxAmount.Text) = 0 Then
            '    calculateDeathBenefitAnnuity = False
            'Else
            '    calculateDeathBenefitAnnuity = True
            'End If

            'SetMonthlyValuesToPurchaseTab(calculateDeathBenefitAnnuity, planType)
            'SetFirstCheckValuesToPurchaseTab()
        Catch ex As Exception
            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,start

            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,end
            Throw ex
        End Try
    End Sub

    'Created by Sanket to calculate the Projected Annuities for disability
    Private Sub GetProjectedAnnuities_ExactAgeEffDate_Disability(ByVal planType As String, ByVal para_RetireType As String)
        Dim dsRetEstimateEmployment As DataSet

        Dim dsElectiveAccounts As DataSet
        Dim hasNoErrors As Boolean
        Dim combinedDataset As DataSet
        Dim errorMessage As String
        Dim businessLogicExactAge As RetirementBOClass
        Dim isYmcaLegacyAcctTotalExceed As Boolean = False
        Dim isYmcaAcctTotalExceed As Boolean = False
        Dim retireeBirthdate As String
        Dim projectedInterestRate As Double
        Dim excludedDataTable As New DataTable
        Dim employmentDetails As New DataTable

        '2012.05.25  SP:  BT-975/YRS 5.0-1508 
        Dim dsSelectedBeneficiary As DataSet
        Dim transactionDetails As DataSet 'MMR | 2017.03.06 | YRS-AT-2625 | Declared object variable

        'Dim employmentDetails As DataTable = Session("employmentSalaryInformation")
        Try

            dsElectiveAccounts = businessLogicExactAge.GetElectiveAccountsByPlan(Me.guiFundEventId, planType, TextBoxRetirementDate.Text)

            'Step 1. Calculate the Account Balances

            '2012.06.04  SP:  YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary  - 
            If planType.ToUpper() = "R" Then
                If HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiary")) Then
                    dsSelectedBeneficiary = CType(Session("dsSelectedParticipantBeneficiary"), DataSet).Copy()
                Else
                    dsSelectedBeneficiary = New DataSet()
                    dsSelectedBeneficiary.Tables.Add(BeneficiaryInfo.Tables(0).Clone())
                End If
            ElseIf planType.ToUpper() = "S" Then
                If HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiarySav")) Then
                    dsSelectedBeneficiary = CType(Session("dsSelectedParticipantBeneficiarySav"), DataSet).Copy()
                Else
                    dsSelectedBeneficiary = New DataSet()
                    dsSelectedBeneficiary.Tables.Add(BeneficiaryInfo.Tables(0).Clone())
                End If
            End If
            '2012.06.04  SP:  YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary  - 


            combinedDataset = New DataSet
            businessLogicExactAge = New RetirementBOClass
            'Commented by Ashish for phase V part III changes,start

            'hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
            ', ycRetireeBirthDay, "0", Me.RetirementDate, String.Empty, String.Empty, String.Empty _
            ', Me.PersId, Me.guiFundEventId, Me.RetireType, 0, dsElectiveAccounts, 0, combinedDataset, False, planType, "", errorMessage)
            'Commented by Ashish for phase V part III changes,end
            'Added by Ashish for phase V part III changes,start
            'hasNoErrors = businessLogicExactAge.CalculateAccountBalances(ycRetireeBirthDay, Me.RetirementDate _
            ', Me.PersId, Me.guiFundEventId, para_RetireType, planType, errorMessage)
            'Added by Ashish for phase V part III changes,end

            'Added by Sanket for disability
            If Not Session("RetEmpInfo") Is Nothing And TypeOf Session("RetEmpInfo") Is DataSet Then
                dsRetEstimateEmployment = DirectCast(Session("RetEmpInfo"), DataSet)
            End If

            'START: MMR | 2017.03.06 | YRS-AT-2625 | Adding manual transaction details to existing dataset for inclusion in computing average salary
            If dsRetEstimateEmployment.Tables.Contains("ManualTransactionDetails") Then
                dsRetEstimateEmployment.Tables.Remove("ManualTransactionDetails")
            End If
            If Not Me.ManualTransactionDetails Is Nothing Then
                transactionDetails = Me.ManualTransactionDetails
                dsRetEstimateEmployment.Tables.Add(transactionDetails.Tables("ManualTransactionDetails").Copy())
            End If
            'END: MMR | 2017.03.06 | YRS-AT-2625 | Adding manual transaction details to existing dataset for inclusion in computing average salary

            hasNoErrors = businessLogicExactAge.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
            , txtRetireeBirthday:=TextBoxBirthDateRet.Text _
            , txtRetirementDate:=TextBoxRetirementDate.Text, personID:=Me.PersId, fundeventID:=Me.guiFundEventId, retireType:=para_RetireType, projectedInterestRate:=0 _
            , dataSetElectiveAccounts:=dsElectiveAccounts, combinedDataSet:=combinedDataset, isEstimate:=False, planType:=planType _
            , fundStatus:=Me.FundEventStatus, errorMessage:=errorMessage, warningMessage:="" _
            , dtExcludedAccounts:=excludedDataTable, employmentDetails:=Nothing, isEsimateProjBal:=False _
            , para_MaxTerminationDate:="" _
            , isYmcaLegacyAcctTotalExceed:=False, isYmcaAcctTotalExceed:=False)

            'Check if any error has been reported.  
            If Not hasNoErrors Then
                errorMessage = getmessage(errorMessage)
                'ShowCustomMessage(errorMessage, enumMessageBoxType.DotNet)
                HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing)    'SP 2013.12.13 BT:2326
                Exit Sub
            End If

            'Compute per basistype Retirement plan balance assign into businessLogic.g_dtAcctBalancesByBasisType
            businessLogicExactAge.calculateFinalAmounts(combinedDataset.Tables(0), planType)

            'Added by Ashish for phase V part III changes
            'Find Max Annuitized factor and update g_dtAcctBalancesByBasis

            businessLogicExactAge.UpdateBasisTypeAsPerAnnuitizeFactor(para_RetireType, Me.RetirementDate)


            'Step 2. Calculate Death Benefits Only if Retirement Plan is selected.
            'No Cahnges 
            tnFullBalance = 0
            'ASHISH:2011.08.29 Resolve YRS 5.0-1345 :BT-859
            If planType = "R" And Me.OrgBenTypeIsQDRO = False Then
                'START : BD : 2018.11.01 : YRS-AT-4135 - No death benefit for particpants enrolled on or after 1/1/2019
                Dim stRDBwarningMessage As String = String.Empty
                tnFullBalance = businessLogicExactAge.GetRetiredDeathBenefit(para_RetireType, Convert.ToDateTime(RetirementDate), Convert.ToDateTime(TextBoxBirthDateRet.Text), Me.guiFundEventId, stRDBwarningMessage) 'BD YRS-AT-4135 - Adding fund event id to check 2019 plan rule change and getting the RDB message 
                tnFullBalance = Math.Round(tnFullBalance, 2)
                Session("RDBwarningMessage") = Nothing
                If Not String.IsNullOrEmpty(stRDBwarningMessage) Then
                    If Not businessLogicExactAge.IsEffectiveDateNull(Me.guiFundEventId) Then
                        Session("RDBwarningMessage") = stRDBwarningMessage
                    End If
                End If
                enableRetirementPlanControls(True)
                'END : BD : 2018.11.01 : YRS-AT-4135 - No death benefit for particpants enrolled on or after 1/1/2019
                Session("CalculatedDB_ExactAgeEffDate") = Math.Round(tnFullBalance, 2)

                ' TextBoxRetiredBenefit.Text = Math.Round(tnFullBalance, 2)
                'TextBoxAmount.Text = Math.Round(Convert.ToDecimal((TextBoxRetiredBenefit.Text) * (DropDownListPercentage.SelectedValue) / 100), 2)

                'tnFullBalance = Convert.ToDecimal(TextBoxAmount.Text)
                tnFullBalance = Math.Round((tnFullBalance * (DropDownListPercentage.SelectedValue) / 100), 2) 'Convert.ToDecimal(TextBoxAmount.Text)

                'If tnFullBalance > 0 Then
                '    LabelDBIncluded.Visible = True
                'Else
                '    LabelDBIncluded.Visible = False
                'End If
            End If

            'Step 3. Calculate the Annuities
            Dim dtAnnuitiesList As DataTable
            Dim dtAnnuitiesFullBalanceParam As DataTable
            Dim dtAnnuitiesParam As DataTable
            Dim benDOB As DateTime
            If planType = "R" Then
                If TextBoxAnnuityBirthDateRet.Text <> String.Empty Then
                    benDOB = Convert.ToDateTime(TextBoxAnnuityBirthDateRet.Text)
                Else
                    benDOB = New DateTime(1900, 1, 1)
                End If
            Else  '2012.06.04   SP:   BT-975/YRS 5.0-1508 
                If TextBoxAnnuityBirthDateSav.Text <> String.Empty Then
                    benDOB = Convert.ToDateTime(TextBoxAnnuityBirthDateSav.Text)
                Else
                    benDOB = New DateTime(1900, 1, 1)
                End If
            End If

            'If TextBoxSSBenefit.Text.Trim() = String.Empty Then
            '    TextBoxSSBenefit.Text = "0"
            'End If
            'Call Exact age annuity methid of BO = CalculateAnnuitiesExactA
            Dim finalAnnuity As Double
            'Changed by Sanket
            '2012.05.25  SP:  BT-975/YRS 5.0-1508 - Paasing selected beneficiary
            dtAnnuitiesList = businessLogicExactAge.CalculateAnnuitiesWithExactAgeForDisability(0, para_RetireType _
            , benDOB, Convert.ToDateTime(TextBoxBirthDateRet.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
            , dsSelectedBeneficiary _
            , Convert.ToDecimal(TextBoxAmount.Text), Session("SSNo"), Session("PersID") _
            , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)

            'Commented by Ashish for phase V part III changes
            'If TextBoxAnnuitySelectRet.Text.Trim.ToUpper = "M" And CheckBoxRetPlan.Checked = True And planType = "R" Then
            '    TextBoxTotalPaymentRet.Text = Math.Round(finalAnnuity, 2).ToString("#0.00")
            'End If

            'If TextBoxAnnuitySelectSav.Text.Trim.ToUpper = "M" And CheckBoxSavPlan.Checked = True And planType = "S" Then
            '    TextBoxTotalPaymentSav.Text = Math.Round(finalAnnuity, 2).ToString("#0.00")
            'End If

            'Step 3.1 Get the Combo table
            Dim comboTable As DataTable
            Dim isFullBalancePassed As Boolean
            If tnFullBalance > 0 Then
                isFullBalancePassed = True
            Else
                isFullBalancePassed = False
            End If

            If (isFullBalancePassed And planType = "R") Then
                Dim dtAnnuitiesListWithDeathBenefit As DataTable
                '2012.05.25  SP:  BT-975/YRS 5.0-1508 - Paasing selected beneficiary
                dtAnnuitiesListWithDeathBenefit = businessLogicExactAge.CalculateAnnuitiesWithExactAge(tnFullBalance, para_RetireType _
                , benDOB, Convert.ToDateTime(TextBoxBirthDateRet.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                , dsSelectedBeneficiary _
                , Convert.ToDecimal(TextBoxAmount.Text), Session("SSNo"), Session("PersID") _
                , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                comboTable = RetirementBOClass.CreateComboTable(dtAnnuitiesList, dtAnnuitiesListWithDeathBenefit)
                Session("lcAnnuitiesListCursor_ExactAgeEffDate") = comboTable
                Session("dtAnnuitiesFullBalanceList_ExactAgeEffDate") = dtAnnuitiesListWithDeathBenefit
            Else
                Session("lcAnnuitiesListCursor_ExactAgeEffDate") = dtAnnuitiesList.Copy()
            End If

            'Step 4 SS levelling
            If (CheckboxIncludeSSLevelling.Checked = True) And (Convert.ToDouble(TextBoxSSBenefit.Text) > 0) Then
                'If Retirement Plan is selected then Attach the SS annuity to it
                If planType = "R" And CheckBoxRetPlan.Checked = True Then
                    dtAnnuitiesList = SetSSLeveling(dtAnnuitiesList, TextBoxAnnuitySelectRet.Text)
                    Session("lcAnnuitiesListCursor_ExactAgeEffDate") = SetSSLeveling(Session("lcAnnuitiesListCursor_ExactAgeEffDate"), TextBoxAnnuitySelectRet.Text)
                ElseIf planType = "S" And CheckBoxRetPlan.Checked = False Then 'If only Savings Plan is selected then attatch SS to it
                    dtAnnuitiesList = SetSSLeveling(dtAnnuitiesList, TextBoxAnnuitySelectSav.Text)
                    Session("lcAnnuitiesListCursor_ExactAgeEffDate") = SetSSLeveling(Session("lcAnnuitiesListCursor_ExactAgeEffDate"), TextBoxAnnuitySelectSav.Text)
                End If
            End If

            ''Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Need to add this session to Compare method according to that set Session("dtAnnuitieslistComboTmp") values whether is old or new
            'temparary shifted to CompaireAnnuities() function with comments
            If planType = "R" Then
                Session("dtAnnuityList_ExactAgeEffDate") = dtAnnuitiesList
            Else
                Session("dtAnnuityListSav_ExactAgeEffDate") = dtAnnuitiesList
            End If

            'Step 5 Calculate the Payments
            CalculatePayments_ExactAgeEffDate(planType)

            ''commented by Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Below commented code shifted to SetValues funcation to set values for 
            ''Step 6 Assigning values to the fields on purchase tab
            'Dim calculateDeathBenefitAnnuity As Boolean
            'If Convert.ToDecimal(TextBoxAmount.Text) = 0 Then
            '    calculateDeathBenefitAnnuity = False
            'Else
            '    calculateDeathBenefitAnnuity = True
            'End If

            'SetMonthlyValuesToPurchaseTab(calculateDeathBenefitAnnuity, planType)
            'SetFirstCheckValuesToPurchaseTab()
        Catch ex As Exception
            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,start

            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,end
            Throw ex
        End Try
    End Sub



    'End Priya 2010.11.18 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
    Private Sub CalculatePayments_ExactAgeEffDate(ByVal planType As String)
        'It will calculate payments using exact age effective date logic.
        Try
            Dim calculateDeathBenefitAnnuity As Boolean
            If Convert.ToDecimal(TextBoxAmount.Text) = 0 Or planType = "S" Then
                calculateDeathBenefitAnnuity = False
            Else
                calculateDeathBenefitAnnuity = True
            End If

            ''commented by Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Below commented code shifted to SetValues funcation to set values for 
            ' Display reserve on the First Tab
            ' TextBoxReservesRet is used only to display the sum and not used any where
            'If planType = "R" Then
            '    Dim actBalRet As Decimal = RetirementBOClass.GetRetirementBalance(Me.guiFundEventId, Me.RetirementDate)
            '    Me.TextBoxReservesRet.Text = actBalRet
            'Else
            '    Dim actBalSav As Decimal = RetirementBOClass.GetSavingsBalance(Me.guiFundEventId, Me.RetirementDate)
            '    Me.TextBoxReservesSav.Text = actBalSav
            'End If

            Dim benDOB As DateTime
            If TextBoxAnnuityBirthDateRet.Text <> String.Empty Then
                benDOB = Convert.ToDateTime(TextBoxAnnuityBirthDateRet.Text)
            Else
                benDOB = New DateTime(1900, 1, 1)
            End If

            Dim dtFinalAnnuity As New DataTable
            Dim lcAnnuitiesListCursor As New DataTable
            lcAnnuitiesListCursor = Session("lcAnnuitiesListCursor_ExactAgeEffDate")

            Dim dtAnnuitiesList As New DataTable
            If planType = "R" Then
                dtAnnuitiesList = Session("dtAnnuityList_ExactAgeEffDate")
            Else
                dtAnnuitiesList = Session("dtAnnuityListSav_ExactAgeEffDate")
            End If

            ' SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect -(passing parameter retirement type)
            dtFinalAnnuity = RetirementBOClass.CalculatePayments(False, Convert.ToDecimal(TextBoxAmount.Text) _
                , benDOB, Convert.ToDateTime(ycRetireeBirthDay) _
                , Convert.ToDateTime(TextBoxRetirementDate.Text), dtAnnuitiesList, dtFinalAnnuity, planType, Session("PersID"), Me.guiFundEventId, Me.RetireType.ToUpper())

            If planType = "R" Then
                Session("dtAnnuityList_ExactAgeEffDate") = dtAnnuitiesList
            Else
                Session("dtAnnuityListSav_ExactAgeEffDate") = dtAnnuitiesList
            End If

            If planType = "R" And (Convert.ToDecimal(TextBoxAmount.Text) > 0) Then
                Dim dtDBAnnuities As New DataTable
                dtFinalAnnuity = Nothing
                dtDBAnnuities = Session("dtAnnuitiesFullBalanceList_ExactAgeEffDate")

                ' SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect -(passing parameter retirement type)
                dtFinalAnnuity = RetirementBOClass.CalculatePayments(True, Convert.ToDecimal(TextBoxAmount.Text) _
                    , benDOB, Convert.ToDateTime(ycRetireeBirthDay) _
                    , Convert.ToDateTime(TextBoxRetirementDate.Text), dtDBAnnuities, dtFinalAnnuity, planType, Session("PersID"), Me.guiFundEventId, Me.RetireType.ToUpper())
                Session("dtAnnuitiesFullBalanceList_ExactAgeEffDate") = dtDBAnnuities
            End If

            dtFinalAnnuity = Nothing

            ' SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect -(passing parameter retirement type)
            dtFinalAnnuity = RetirementBOClass.CalculatePayments(False, Convert.ToDecimal(TextBoxAmount.Text) _
                , benDOB, Convert.ToDateTime(ycRetireeBirthDay) _
                , Convert.ToDateTime(TextBoxRetirementDate.Text), lcAnnuitiesListCursor, dtFinalAnnuity, planType, Session("PersID"), Me.guiFundEventId, Me.RetireType.ToUpper())
            Session("lcAnnuitiesListCursor_ExactAgeEffDate") = lcAnnuitiesListCursor

            ' Get combo data 
            Dim dtComboData_ExactAge As New DataTable
            dtComboData_ExactAge = lcAnnuitiesListCursor

            'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Need to add this session to Compare method according to that set Session("dtAnnuitieslistComboTmp") values whether is old or new
            'temparary shifted to CompaireAnnuities() function with comments
            Session("dtAnnuitieslistComboTmp_ExactAgeEffDate") = Session("lcAnnuitiesListCursor_ExactAgeEffDate")

            ' Get Payment data 
            Dim dtPaymentData As New DataTable
            dtPaymentData = RetirementBOClass.GetPaymentData(dtComboData_ExactAge)
            If Me.FundEventStatus <> "QD" Then 'Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
                Dim beneficiaryBirthDate As String = String.Empty
                '2012.05.28 SP : YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary 
                If planType.ToUpper() = "S" Then '2012.06.04 SP : YRS 5.0-1508
                    If TextBoxAnnuityBirthDateSav.Text <> String.Empty And DropDownRelationShipSav.SelectedValue.ToUpper() <> "SP" Then
                        beneficiaryBirthDate = TextBoxAnnuityBirthDateSav.Text.Trim()
                    End If
                ElseIf planType.ToUpper() = "R" Then
                    If TextBoxAnnuityBirthDateRet.Text <> String.Empty And DropDownRelationShipRet.SelectedValue.ToUpper() <> "SP" Then
                        beneficiaryBirthDate = TextBoxAnnuityBirthDateRet.Text.Trim()
                    End If
                End If

                'beneficiaryBirthDate = RetirementBOClass.GetNonSpouseBeneficiaryBirthDateForProcessing(Me.PersId, planType)
                '2012.05.28 SP : YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary 
                If beneficiaryBirthDate <> String.Empty Then
                    Dim ageDiff As Int16
                    ageDiff = YMCARET.YmcaBusinessObject.RetirementBOClass.GetRetireeAndBeneficiaryAgeDiff(ycRetireeBirthDay, beneficiaryBirthDate, TextBoxRetirementDate.Text)
                    dtPaymentData = YMCARET.YmcaBusinessObject.RetirementBOClass.AvailJandSAnnuityOptions_RetProcessing(ageDiff, dtPaymentData)
                End If
                'End
                '----------------------------------------------------------------------------------
                'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
            ElseIf Me.FundEventStatus = "QD" And ((Session("IsDefaultBeneficiarySpouse") = True And planType.ToUpper() = "R") Or (Session("IsDefaultBeneficiarySpouseSav") = True And planType.ToUpper() = "S")) Then
                dtPaymentData = YMCARET.YmcaBusinessObject.RetirementBOClass.BlockJAnnuityOptions_RetProcessing(dtPaymentData)
                'ElseIf Me.FundEventStatus = "QD" And Session("IsDefaultBeneficiarySpouseSav") = True And planType.ToUpper() = "S" Then

                '    dtPaymentData = YMCARET.YmcaBusinessObject.RetirementBOClass.BlockJAnnuityOptions_RetProcessing(dtPaymentData)
            End If
            'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
            If HelperFunctions.isNonEmpty(dtPaymentData) Then

                Dim ds_SelectAnnuity As New DataSet
                ds_SelectAnnuity.Tables.Add(dtPaymentData)
                ds_SelectAnnuity.AcceptChanges()
                If planType = "R" Then
                    'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
                    'Me.SelectAnnuityRetirement = ds_SelectAnnuity
                    Me.SelectAnnuityRetirementExactAgeEffDate = ds_SelectAnnuity
                Else
                    'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
                    'Me.SelectAnnuitySavings = ds_SelectAnnuity
                    Me.SelectAnnuitySavingsExactAgeEffDate = ds_SelectAnnuity
                End If

                Me.RetirementDate = TextBoxRetirementDate.Text
                'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
                'Need to add this function to Compare method according highest value need to set textbox values and then set to session
                'Added to setValues function
                'Me.saveControlValuesInSessionVariables()

                'Call SetRetireType()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub SetSessionValuesBasedOnSelectedOption(ByVal PlanType As String, ByVal SelectedOption As String)
        Try
            If SelectedOption = "OLD" Then
                If PlanType = "R" Then
                    Session("dtAnnuityList") = Session("dtAnnuityList_BeforeExactAgeEffDate")
                    Session("dtAnnuitiesFullBalanceList") = Session("dtAnnuitiesFullBalanceList_BeforeExactAgeEffDate")
                    Session("CalculatedDB") = Session("CalculatedDB_BeforeExactAgeEffDate")
                    Session("SelectedCalledAnnuity_Ret") = "OLD"
                ElseIf PlanType = "S" Then
                    Session("dtAnnuityListSav") = Session("dtAnnuityListSav_BeforeExactAgeEffDate")
                    Session("SelectedCalledAnnuity_Sav") = "OLD"
                End If
                Session("dtAnnuitieslistComboTmp") = Session("dtAnnuitieslistComboTmp_BeforeExactAgeEffDate")
                Session("lcAnnuitiesListCursor") = Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate")
                'Priya 11/30/2010 BT-683:Wrong message shows when select new annuity
                'Priya 12/20/2010 BT-710:Retirement Process screen message shows wrong commented text
                'lblAnnuityMessage.Text = "Annuity computed using " & Me.ExactAgeEffDate & " balance"
                'lblAnnuityMessage.Text = ""
                'lblAnnuityMessage.Visible = True
                'End 11/30/2010 BT-683:Wrong message shows when select new annuity

            ElseIf SelectedOption = "NEW" Then

                If PlanType = "R" Then
                    Session("dtAnnuitiesFullBalanceList") = Session("dtAnnuitiesFullBalanceList_ExactAgeEffDate")
                    Session("dtAnnuityList") = Session("dtAnnuityList_ExactAgeEffDate")
                    Session("CalculatedDB") = Session("CalculatedDB_ExactAgeEffDate")
                    Session("SelectedCalledAnnuity_Ret") = "NEW"
                ElseIf PlanType = "S" Then
                    Session("dtAnnuityListSav") = Session("dtAnnuityListSav_ExactAgeEffDate")
                    Session("SelectedCalledAnnuity_Sav") = "NEW"
                End If
                Session("dtAnnuitieslistComboTmp") = Session("dtAnnuitieslistComboTmp_ExactAgeEffDate")
                Session("lcAnnuitiesListCursor") = Session("lcAnnuitiesListCursor_ExactAgeEffDate")
                'Priya 11/30/2010 BT-683:Wrong message shows when select new annuity
                'lblAnnuityMessage.Text = ""
                'lblAnnuityMessage.Visible = False
                'End 11/30/2010 BT-683:Wrong message shows when select new annuity
            End If
        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    Private Sub CompareAnnuities(ByVal dsExactAgeAnnuity As DataSet, ByVal dsBeforeExactAgeAnnuity As DataSet, ByVal planType As String)
        Dim strCalledAnnuity As String
        Try
            strCalledAnnuity = Session("CalledAnnuity")

            'If planType = "R" Then
            '    If Not Session("SelectedCalledAnnuity_Ret") Is Nothing Then
            '        strCalledAnnuity = Session("SelectedCalledAnnuity_Ret")
            '    Else
            '        strCalledAnnuity = Session("CalledAnnuity")
            '    End If
            'ElseIf planType = "S" Then
            '    If Not Session("SelectedCalledAnnuity_Sav") Is Nothing Then
            '        strCalledAnnuity = Session("SelectedCalledAnnuity_Sav")
            '    Else
            '        strCalledAnnuity = Session("CalledAnnuity")
            '    End If
            'End If


            'If Not Session("SelectedCalledAnnuity") Is Nothing Then
            '    strCalledAnnuity = Session("SelectedCalledAnnuity")
            'Else
            '    strCalledAnnuity = Session("CalledAnnuity")
            'End If

            If strCalledAnnuity = "OLD" Then
                If planType = "R" Then

                    SetSessionValuesBasedOnSelectedOption("R", "OLD")

                    'Session("dtAnnuityList") = Session("dtAnnuityList_BeforeExactAgeEffDate")
                    'Session("dtAnnuitiesFullBalanceList") = Session("dtAnnuitiesFullBalanceList_BeforeExactAgeEffDate")
                    'Session("CalculatedDB") = Session("CalculatedDB_BeforeExactAgeEffDate")
                    'Session("SelectedCalledAnnuity_Ret") = "OLD"
                ElseIf planType = "S" Then
                    SetSessionValuesBasedOnSelectedOption("S", "OLD")
                    'Session("dtAnnuityListSav") = Session("dtAnnuityListSav_BeforeExactAgeEffDate")
                    'Session("SelectedCalledAnnuity_Sav") = "OLD"
                End If
                'Session("dtAnnuitieslistComboTmp") = Session("dtAnnuitieslistComboTmp_BeforeExactAgeEffDate")
                'Session("lcAnnuitiesListCursor") = Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate")

                ''Session("SelectedCalledAnnuity") = "OLD"
                'SP 2013.12.13 BT-2326
                'lblAnnuityMessage.Text = "Annuity computed using " & Me.ExactAgeEffDate & " balance"
                'lblAnnuityMessage.Visible = True
                'SP 2013.12.13 BT-2326
                HelperFunctions.ShowMessageToUser("Annuity computed using " & Me.ExactAgeEffDate & " balance", EnumMessageTypes.Warning)

            ElseIf strCalledAnnuity = "NEW" Then
                If planType = "R" Then
                    SetSessionValuesBasedOnSelectedOption("R", "NEW")
                    'Session("dtAnnuitiesFullBalanceList") = Session("dtAnnuitiesFullBalanceList_ExactAgeEffDate")
                    'Session("dtAnnuityList") = Session("dtAnnuityList_ExactAgeEffDate")
                    'Session("CalculatedDB") = Session("CalculatedDB_ExactAgeEffDate")
                    'Session("SelectedCalledAnnuity_Ret") = "NEW"
                ElseIf planType = "S" Then
                    SetSessionValuesBasedOnSelectedOption("S", "NEW")
                    'Session("dtAnnuityListSav") = Session("dtAnnuityListSav_ExactAgeEffDate")
                    'Session("SelectedCalledAnnuity_Sav") = "NEW"
                End If

                'Session("dtAnnuitieslistComboTmp") = Session("dtAnnuitieslistComboTmp_ExactAgeEffDate")
                'Session("lcAnnuitiesListCursor") = Session("lcAnnuitiesListCursor_ExactAgeEffDate")
                'Session("SelectedCalledAnnuity") = "NEW"


                'lblAnnuityMessage.Text = ""
                'lblAnnuityMessage.Visible = False

            ElseIf strCalledAnnuity = "BOTH" Then

                If dsExactAgeAnnuity Is Nothing And dsBeforeExactAgeAnnuity Is Nothing Then
                    Exit Sub
                End If

                'Logic To compare Annuity and shows the details of annuity whichever is higher.
                Dim dsExactAgeAnnuity_Compare As New DataSet
                Dim dsBeforeExactAgeAnnuity_Compare As New DataSet
                Dim dsFinalAnnuity_AfterCompare As New DataSet
                Dim l_dt_FinalAnnuityAfterCompare As DataTable = Nothing



                dsExactAgeAnnuity_Compare = dsExactAgeAnnuity.Copy()
                dsBeforeExactAgeAnnuity_Compare = dsBeforeExactAgeAnnuity.Copy()
                l_dt_FinalAnnuityAfterCompare = New DataTable
                l_dt_FinalAnnuityAfterCompare = dsExactAgeAnnuity.Tables(0).Clone()

                'dsFinalAnnuity_AfterCompare = dsExactAgeAnnuity.Clone()
                If Not l_dt_FinalAnnuityAfterCompare.Columns.Contains("AnuityBforeAfter") Then
                    l_dt_FinalAnnuityAfterCompare.Columns.Add("AnuityBforeAfter")
                End If

                Dim iBefore As Integer
                Dim iAfter As Integer
                Dim strBeforeAnnuity As String
                Dim strAfterAnnuity As String
                Dim dr_Final As DataRow


                'Sanket Vaidya 12 Apr 2011 For BT-810 Error in Normal Retirement Processing after regular withdrawl
                If dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows.Count <> 0 And dsExactAgeAnnuity_Compare.Tables(0).Rows.Count <> 0 Then
                    'Below for loop check whether old annuity or Exact age effective age anuity amount is greater accoring to that it will select annuity and amount.
                    If dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows.Count = dsExactAgeAnnuity_Compare.Tables(0).Rows.Count Then

                        For iBefore = 0 To dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows.Count - 1

                            For iAfter = 0 To dsExactAgeAnnuity_Compare.Tables(0).Rows.Count - 1

                                strBeforeAnnuity = dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows(iBefore)("Annuity")
                                strAfterAnnuity = dsExactAgeAnnuity_Compare.Tables(0).Rows(iAfter)("Annuity")


                                If strBeforeAnnuity.Trim() = strAfterAnnuity.Trim() Then

                                    Dim decBeforeExactAgeAmt As Decimal
                                    Dim decExactAgeAmt As Decimal
                                    'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
                                    decBeforeExactAgeAmt = Convert.ToDecimal(IIf(dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows(iBefore)("Amount") = "*N/A", 0, dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows(iBefore)("Amount")))
                                    decExactAgeAmt = Convert.ToDecimal(IIf(dsExactAgeAnnuity_Compare.Tables(0).Rows(iAfter)("Amount") = "*N/A", 0, dsExactAgeAnnuity_Compare.Tables(0).Rows(iAfter)("Amount")))
                                    'decBeforeExactAgeAmt = Convert.ToDecimal(dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows(iBefore)("Amount"))
                                    'decExactAgeAmt = Convert.ToDecimal(dsExactAgeAnnuity_Compare.Tables(0).Rows(iAfter)("Amount"))


                                    ' Dim dr_Final As DataRow
                                    dr_Final = l_dt_FinalAnnuityAfterCompare.NewRow()
                                    Dim i As Integer
                                    Dim dr As DataRow
                                    If decBeforeExactAgeAmt > decExactAgeAmt Then
                                        'dr_Final = dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows(iBefore)
                                        dr = dsBeforeExactAgeAnnuity_Compare.Tables(0).NewRow()
                                        For i = 0 To dsBeforeExactAgeAnnuity_Compare.Tables(0).Columns.Count - 1
                                            dr_Final(i) = dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows(iBefore)(i)
                                        Next

                                        dr_Final("AnuityBforeAfter") = "OLD"


                                    ElseIf decBeforeExactAgeAmt <= decExactAgeAmt Then
                                        'dr_Final = dsExactAgeAnnuity_Compare.Tables(0).Rows(iAfter)
                                        dr = dsExactAgeAnnuity_Compare.Tables(0).NewRow()
                                        For i = 0 To dsExactAgeAnnuity_Compare.Tables(0).Columns.Count - 1
                                            dr_Final(i) = dsExactAgeAnnuity_Compare.Tables(0).Rows(iBefore)(i)
                                        Next

                                        dr_Final("AnuityBforeAfter") = "NEW"

                                    End If

                                    If dr_Final Is Nothing Then
                                        Throw New Exception("Error while computing annuity")
                                    End If

                                    l_dt_FinalAnnuityAfterCompare.Rows.Add(dr_Final)
                                End If
                            Next
                        Next
                    End If
                    'Sanket Vaidya 12 Apr 2011 For BT-810 Error in Normal Retirement Processing after regular withdrawl
                ElseIf dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows.Count = 0 And dsExactAgeAnnuity_Compare.Tables(0).Rows.Count > 0 Then
                    l_dt_FinalAnnuityAfterCompare = dsExactAgeAnnuity_Compare.Tables(0).Copy()
                    l_dt_FinalAnnuityAfterCompare.Columns.Add("AnuityBforeAfter")
                    Dim i As Integer
                    For i = 0 To l_dt_FinalAnnuityAfterCompare.Rows.Count - 1
                        l_dt_FinalAnnuityAfterCompare.Rows(i)("AnuityBforeAfter") = "NEW"
                    Next
                    'ASHISH:2011.12.09:YRS 5.0-1353 
                    Session("CalledAnnuity") = "NEW"
                ElseIf dsExactAgeAnnuity_Compare.Tables(0).Rows.Count = 0 And dsBeforeExactAgeAnnuity_Compare.Tables(0).Rows.Count > 0 Then
                    l_dt_FinalAnnuityAfterCompare = dsBeforeExactAgeAnnuity_Compare.Tables(0).Copy()
                    l_dt_FinalAnnuityAfterCompare.Columns.Add("AnuityBforeAfter")
                    Dim i As Integer
                    For i = 0 To l_dt_FinalAnnuityAfterCompare.Rows.Count - 1
                        l_dt_FinalAnnuityAfterCompare.Rows(i)("AnuityBforeAfter") = "OLD"
                    Next
                    'ASHISH:2011.12.09:YRS 5.0-1353 
                    Session("CalledAnnuity") = "OLD"
                Else
                    Throw New Exception("Invalid Annuity computation")
                End If


                dsFinalAnnuity_AfterCompare.Tables.Add(l_dt_FinalAnnuityAfterCompare)

                Dim drSelectAnnuityRow As DataRow()
                Dim selectedAnnuitySet As String = String.Empty
                'Dim finalSelectedAnnuitySet As String = String.Empty
                'Accordig to plan type Assign values to propertis to select in choose annuity popup.

                'Add logic for set session variable based on selected annuity from the higest vale set

                If planType = "R" Then
                    ' drSelectAnnuityRow = dsFinalAnnuity_AfterCompare.Tables(0).Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
                    drSelectAnnuityRow = dsFinalAnnuity_AfterCompare.Tables(0).Select("Annuity='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
                    If drSelectAnnuityRow.Length > 0 Then
                        selectedAnnuitySet = drSelectAnnuityRow(0)("AnuityBforeAfter").ToString()

                        'Assign value to session to select record into choose annuity popup

                        If Session("SelectedCalledAnnuity_Ret") Is Nothing Then
                            Session("SelectedCalledAnnuity_Ret") = selectedAnnuitySet
                        ElseIf Me.TransAfterExactAgeEffDate = "0" Then
                            Session("SelectedCalledAnnuity_Ret") = selectedAnnuitySet
                        End If




                    End If

                ElseIf planType = "S" Then
                    ' drSelectAnnuityRow = dsFinalAnnuity_AfterCompare.Tables(0).Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
                    drSelectAnnuityRow = dsFinalAnnuity_AfterCompare.Tables(0).Select("Annuity='" & TextBoxAnnuitySelectSav.Text.Trim() & "'")
                    If drSelectAnnuityRow.Length > 0 Then
                        selectedAnnuitySet = drSelectAnnuityRow(0)("AnuityBforeAfter").ToString()
                        'Assign value to session to select record into choose annuity popup

                        If Session("SelectedCalledAnnuity_Sav") Is Nothing Then
                            Session("SelectedCalledAnnuity_Sav") = selectedAnnuitySet
                        ElseIf Me.TransAfterExactAgeEffDate = "0" Then
                            Session("SelectedCalledAnnuity_Sav") = selectedAnnuitySet
                        End If


                    End If
                End If

                ''Accordig to plan type Assign values to propertis to select in choose annuity popup.
                If Me.TransAfterExactAgeEffDate = "0" Then
                    If planType = "R" Then
                        Me.SelectAnnuityCombinedRetExactAgeEffDate = dsFinalAnnuity_AfterCompare
                    ElseIf planType = "S" Then
                        Me.SelectAnnuityCombinedSavExactAgeEffDate = dsFinalAnnuity_AfterCompare
                    End If
                    'Else 'Me.TransAfterExactAgeEffDate = 1
                    'If planType = "R" Then
                    '    selectedAnnuitySet = Session("SelectedCalledAnnuity_Ret")
                    'ElseIf planType = "S" Then
                    '    selectedAnnuitySet = Session("SelectedCalledAnnuity_Sav")
                    'End If

                End If
                If planType = "R" Then
                    selectedAnnuitySet = Session("SelectedCalledAnnuity_Ret")
                ElseIf planType = "S" Then
                    selectedAnnuitySet = Session("SelectedCalledAnnuity_Sav")
                End If


                'Checks whether annuity is new or old according tio that assign session values to session which are used in purchase tab.
                If selectedAnnuitySet = "NEW" Then
                    If planType = "R" Then
                        SetSessionValuesBasedOnSelectedOption("R", "NEW")
                        'Session("dtAnnuitiesFullBalanceList") = Session("dtAnnuitiesFullBalanceList_ExactAgeEffDate")
                        'Session("dtAnnuityList") = Session("dtAnnuityList_ExactAgeEffDate")
                        'Session("CalculatedDB") = Session("CalculatedDB_ExactAgeEffDate")
                    Else
                        SetSessionValuesBasedOnSelectedOption("S", "NEW")
                        'Session("dtAnnuityListSav") = Session("dtAnnuityListSav_ExactAgeEffDate")
                    End If
                    'Session("dtAnnuitieslistComboTmp") = Session("dtAnnuitieslistComboTmp_ExactAgeEffDate")
                    'Session("lcAnnuitiesListCursor") = Session("lcAnnuitiesListCursor_ExactAgeEffDate")

                    'lblAnnuityMessage.Text = ""
                    'lblAnnuityMessage.Visible = False

                ElseIf selectedAnnuitySet = "OLD" Then

                    If planType = "R" Then
                        SetSessionValuesBasedOnSelectedOption("R", "OLD")
                        'Session("dtAnnuitiesFullBalanceList") = Session("dtAnnuitiesFullBalanceList_BeforeExactAgeEffDate")
                        'Session("dtAnnuityList") = Session("dtAnnuityList_BeforeExactAgeEffDate")
                        'Session("CalculatedDB") = Session("CalculatedDB_BeforeExactAgeEffDate")
                    Else
                        SetSessionValuesBasedOnSelectedOption("S", "OLD")
                        'Session("dtAnnuityListSav") = Session("dtAnnuityListSav_BeforeExactAgeEffDate")
                    End If
                    'Session("dtAnnuitieslistComboTmp") = Session("dtAnnuitieslistComboTmp_BeforeExactAgeEffDate")
                    'Session("lcAnnuitiesListCursor") = Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate")

                    'SP 2013.12.13 BT-2326
                    'lblAnnuityMessage.Text = "Annuity computed using " & Me.ExactAgeEffDate & " balance."
                    'lblAnnuityMessage.Visible = False
                    HelperFunctions.ShowMessageToUser("Annuity computed using " & Me.ExactAgeEffDate & " balance.", EnumMessageTypes.Warning)
                End If

                'Check whteher withdrawal is exists or not according to that shows message.
                'It will
                If Convert.ToDateTime(TextBoxRetirementDate.Text) >= Convert.ToDateTime(Me.ExactAgeEffDate) AndAlso Convert.ToDateTime(TextBoxRetirementDate.Text) < (Convert.ToDateTime(Me.ExactAgeEffDate).AddMonths(6)) Then
                    If Me.TransAfterExactAgeEffDate = "1" Then
                        'SP 2013.12.13 BT-2326
                        'lblAnnuityMessage.Visible = True
                        'If (lblAnnuityMessage.Text <> "") Then
                        'lblAnnuityMessage.Text = "<br/> Annuity purchase balance has been adjusted to include some transactions after the Retirement date."
                        'Else
                        'lblAnnuityMessage.Text = "Annuity purchase balance has been adjusted to include some transactions after the Retirement date."
                        'End If
                        HelperFunctions.ShowMessageToUser("Annuity purchase balance has been adjusted to include some transactions after the Retirement date. " & Me.ExactAgeEffDate & " balance.", EnumMessageTypes.Warning)
                    End If
                End If

            End If
        Catch ex As Exception
            Throw (ex)
        End Try

    End Sub

    Private Sub setValues(ByVal planType As String)
        'This function set values to text boxes.
        Dim dtComboData_ExactAge As New DataTable
        Try


            'FRom CalculatePayments_ExactAgeEffDate()
            ' Populate the data in the UI


            'Dim planType As String
            'If CheckBoxRetPlan.Checked = True Then
            '    planType = "R"
            'ElseIf CheckBoxSavPlan.Checked = True Then
            '    planType = "S"
            'End If

            'Set Reserves
            If planType = "R" Then
                Dim actBalRet As Decimal = RetirementBOClass.GetRetirementBalance(Me.guiFundEventId, Me.RetirementDate)
                Me.TextBoxReservesRet.Text = actBalRet
            Else
                Dim actBalSav As Decimal = RetirementBOClass.GetSavingsBalance(Me.guiFundEventId, Me.RetirementDate)
                Me.TextBoxReservesSav.Text = actBalSav
            End If


            'Sanket Vaidya 12 Apr 2011 For BT-810 Error in Normal Retirement Processing after regular withdrawl
            If Not Session("lcAnnuitiesListCursor") Is Nothing Then
                dtComboData_ExactAge = CType(Session("lcAnnuitiesListCursor"), DataTable)

                Dim drSelectedAnnuities As DataRow()
                Dim drSelectedAnnuity As DataRow
                Dim dblNonTaxable As Double
                Dim dblTaxable As Double
                Dim dblSSIncrease As Double = 0

                If planType = "R" Then
                    drSelectedAnnuities = dtComboData_ExactAge.Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
                Else
                    drSelectedAnnuities = dtComboData_ExactAge.Select("chrAnnuityType='" & TextBoxAnnuitySelectSav.Text.Trim() & "'")
                End If

                If drSelectedAnnuities.Length > 0 Then
                    drSelectedAnnuity = drSelectedAnnuities(0)
                    dblNonTaxable = Math.Round(Convert.ToDecimal(drSelectedAnnuity("mnyPersonalPostTaxCurrentPayment") + drSelectedAnnuity("mnyYmcaPostTaxCurrentPayment")), 2)
                    dblTaxable = Math.Round(Convert.ToDecimal(drSelectedAnnuity("mnyYmcaPreTaxCurrentPayment")) + Convert.ToDecimal(drSelectedAnnuity("mnyPersonalPreTaxCurrentPayment")), 2)


                    ''commented by Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
                    '1st need to compaire amounts then show to textboxes
                    'temparary shifted to CompaireAnnuities() function
                    If planType = "R" Then
                        TextBoxTaxableRet.Text = Math.Round(dblTaxable + dblSSIncrease, 2).ToString("#0.00")
                        TextBoxNonTaxableRet.Text = Math.Round(dblNonTaxable, 2).ToString("#0.00")
                        TextBoxTotalPaymentRet.Text = Math.Round(dblNonTaxable + (dblTaxable + dblSSIncrease), 2).ToString("#0.00")
                    Else
                        TextBoxTaxableSav.Text = Math.Round(dblTaxable + dblSSIncrease, 2).ToString("#0.00")
                        TextBoxNonTaxableSav.Text = Math.Round(dblNonTaxable, 2).ToString("#0.00")
                        TextBoxTotalPaymentSav.Text = Math.Round(dblNonTaxable + (dblTaxable + dblSSIncrease), 2).ToString("#0.00")
                    End If
                    'End Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
                End If
            End If
            'Shifted to Calculate annuities
            ''From GetProjectedAnnuities_ExactAgeEffDate
            ''Step 6 Assigning values to the fields on purchase tab
            'Dim calculateDeathBenefitAnnuity As Boolean
            'If Convert.ToDecimal(TextBoxAmount.Text) = 0 Then
            '    calculateDeathBenefitAnnuity = False
            'Else
            '    calculateDeathBenefitAnnuity = True
            'End If



            '''FROM CALCULARE ANUUITY
            'Me.loadSSLevellingControls()
            'displayPurchaseInfo()

            'SetMonthlyValuesToPurchaseTab(calculateDeathBenefitAnnuity, planType)
            'SetFirstCheckValuesToPurchaseTab()

            ''From CalculatePayamets.
            'Me.RetirementDate = TextBoxRetirementDate.Text
            'Me.saveControlValuesInSessionVariables()
            'Call SetRetireType()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region


#Region "Code for Account Balances and Get Projections"
    Private Sub GetProjectedAnnuities(ByVal planType As String)
        Dim dsRetEstimateEmployment As DataSet
        Dim dsElectiveAccounts As DataSet
        Dim hasNoErrors As Boolean
        Dim combinedDataset As DataSet
        Dim errorMessage As String
        Dim businessLogic As RetirementBOClass

        '2012.05.25  SP:  BT-975/YRS 5.0-1508 - 
        Dim dsSelectedBeneficiary As DataSet

        'dsRetEstimateEmployment = RetirementBOClass.getRetEstimateEmployment(Me.guiFundEventId, Me.RetireType, Me.RetirementDate)
        'ASHISH:2009.11.17 -Commented 
        'dsRetEstimateEmployment = RetirementBOClass.getEmploymentDetails(Me.guiFundEventId, Me.RetireType, Me.RetirementDate)

        Try
            dsElectiveAccounts = getElectiveAccounts(planType)
            'Step 1. Calculate the Account Balances

            '2012.06.04  SP:  YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary  - 
            If planType.ToUpper() = "R" Then
                If HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiary")) Then
                    dsSelectedBeneficiary = CType(Session("dsSelectedParticipantBeneficiary"), DataSet).Copy()
                Else
                    dsSelectedBeneficiary = New DataSet()
                    dsSelectedBeneficiary.Tables.Add(BeneficiaryInfo.Tables(0).Clone())
                End If
            ElseIf planType.ToUpper() = "S" Then
                If HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiarySav")) Then
                    dsSelectedBeneficiary = CType(Session("dsSelectedParticipantBeneficiarySav"), DataSet).Copy()
                Else
                    dsSelectedBeneficiary = New DataSet()
                    dsSelectedBeneficiary.Tables.Add(BeneficiaryInfo.Tables(0).Clone())
                End If
            End If
            '2012.06.04   SP:  YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary  -

            combinedDataset = New DataSet
            businessLogic = New RetirementBOClass
            'Commented by Ashish for phase V part III changes,start

            'hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
            ', ycRetireeBirthDay, "0", Me.RetirementDate, String.Empty, String.Empty, String.Empty _
            ', Me.PersId, Me.guiFundEventId, Me.RetireType, 0, dsElectiveAccounts, 0, combinedDataset, False, planType, "", errorMessage)
            'Commented by Ashish for phase V part III changes,end
            'Added by Ashish for phase V part III changes,start
            'hasNoErrors = businessLogic.CalculateAccountBalances(ycRetireeBirthDay, Me.RetirementDate _
            ', Me.PersId, Me.guiFundEventId, Me.RetireType, planType, errorMessage)
            'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            If Convert.ToDateTime(Me.RetirementDate) < Convert.ToDateTime(Me.ExactAgeEffDate) Then
                hasNoErrors = businessLogic.CalculateAccountBalances(ycRetireeBirthDay, Me.RetirementDate _
               , Me.PersId, Me.guiFundEventId, Me.RetireType, planType, errorMessage)
            Else
                hasNoErrors = businessLogic.CalculateAccountBalancesBeforeExactAgeEffDate(ycRetireeBirthDay, Me.RetirementDate _
                            , Me.PersId, Me.guiFundEventId, Me.RetireType, planType, errorMessage)
            End If
            'End priya
            'Added by Ashish for phase V part III changes,end

            'Check if any error has been reported.  
            If Not hasNoErrors Then
                'ShowCustomMessage(errorMessage, enumMessageBoxType.DotNet)
                HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing)    'SP 2013.12.13 BT:2326
                Exit Sub
            End If

            'Added by Ashish for phase V part III changes
            'Find Max Annuitized factor and update g_dtAcctBalancesByBasis
            businessLogic.UpdateBasisTypeAsPerAnnuitizeFactor(Me.RetireType, Me.RetirementDate)

            'Step 2. Calculate Death Benefits Only if Retirement Plan is selected.
            tnFullBalance = 0
            'ASHISH:2011.08.29 Resolve YRS 5.0-1345 :BT-859
            If planType = "R" And Me.OrgBenTypeIsQDRO = False Then
                'START : BD : 2018.11.01 : YRS-AT-4135 - No death benefit for particpants enrolled on or after 1/1/2019
                Dim stRDBwarningMessage As String = String.Empty
                tnFullBalance = businessLogic.GetRetiredDeathBenefit(RetireType, Convert.ToDateTime(RetirementDate), Convert.ToDateTime(TextBoxBirthDateRet.Text), Me.guiFundEventId, stRDBwarningMessage) 'BD YRS-AT-4135 - Adding fund event id to check 2019 plan rule change and getting the RDB message 
                tnFullBalance = Math.Round(tnFullBalance, 2)
                Session("RDBwarningMessage") = Nothing
                If Not String.IsNullOrEmpty(stRDBwarningMessage) Then
                    If Not businessLogic.IsEffectiveDateNull(Me.guiFundEventId) Then
                        Session("RDBwarningMessage") = stRDBwarningMessage
                    End If
                End If
                enableRetirementPlanControls(True)
                'END : BD : 2018.11.01 : YRS-AT-4135 - No death benefit for particpants enrolled on or after 1/1/2019
                Session("CalculatedDB_BeforeExactAgeEffDate") = Math.Round(tnFullBalance, 2)

                'TextBoxRetiredBenefit.Text = Math.Round(tnFullBalance, 2)
                'TextBoxAmount.Text = Math.Round(Convert.ToDecimal((TextBoxRetiredBenefit.Text) * (DropDownListPercentage.SelectedValue) / 100), 2)

                tnFullBalance = Math.Round((tnFullBalance * (DropDownListPercentage.SelectedValue) / 100), 2) 'Convert.ToDecimal(TextBoxAmount.Text)

                'If tnFullBalance > 0 Then
                '    LabelDBIncluded.Visible = True
                'Else
                '    LabelDBIncluded.Visible = False
                'End If
            End If

            'Step 3. Calculate the Annuities
            Dim dtAnnuitiesList As DataTable
            Dim dtAnnuitiesFullBalanceParam As DataTable
            Dim dtAnnuitiesParam As DataTable
            Dim benDOB As DateTime
            If planType = "R" Then
                If TextBoxAnnuityBirthDateRet.Text <> String.Empty Then
                    benDOB = Convert.ToDateTime(TextBoxAnnuityBirthDateRet.Text)
                Else
                    benDOB = New DateTime(1900, 1, 1)
                End If
            Else '2012.06.04   SP:   BT-975/YRS 5.0-1508 
                If TextBoxAnnuityBirthDateSav.Text <> String.Empty Then
                    benDOB = Convert.ToDateTime(TextBoxAnnuityBirthDateSav.Text)
                Else
                    benDOB = New DateTime(1900, 1, 1)
                End If
            End If

            'If TextBoxSSBenefit.Text.Trim() = String.Empty Then
            '    TextBoxSSBenefit.Text = "0"
            'End If

            Dim finalAnnuity As Double
            '2012.05.25  SP:  BT-975/YRS 5.0-1508 - Passing selected beneficiary
            dtAnnuitiesList = businessLogic.CalculateAnnuities(0, RetireType _
            , benDOB, Convert.ToDateTime(TextBoxBirthDateRet.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
            , dsSelectedBeneficiary, 0.0, 0.0 _
            , Convert.ToDecimal(TextBoxAmount.Text), Session("SSNo"), Session("PersID") _
            , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)

            'Commented by Ashish for phase V part III changes
            'If TextBoxAnnuitySelectRet.Text.Trim.ToUpper = "M" And CheckBoxRetPlan.Checked = True And planType = "R" Then
            '    TextBoxTotalPaymentRet.Text = Math.Round(finalAnnuity, 2).ToString("#0.00")
            'End If

            'If TextBoxAnnuitySelectSav.Text.Trim.ToUpper = "M" And CheckBoxSavPlan.Checked = True And planType = "S" Then
            '    TextBoxTotalPaymentSav.Text = Math.Round(finalAnnuity, 2).ToString("#0.00")
            'End If

            'Step 3.1 Get the Combo table
            Dim comboTable As DataTable
            Dim isFullBalancePassed As Boolean
            If tnFullBalance > 0 Then
                isFullBalancePassed = True
            Else
                isFullBalancePassed = False
            End If

            If (isFullBalancePassed And planType = "R") Then
                Dim dtAnnuitiesListWithDeathBenefit As DataTable
                '2012.05.25  SP:  BT-975/YRS 5.0-1508 - Passing selected beneficiary
                dtAnnuitiesListWithDeathBenefit = businessLogic.CalculateAnnuities(tnFullBalance, RetireType _
                , benDOB, Convert.ToDateTime(TextBoxBirthDateRet.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                , dsSelectedBeneficiary, 0.0, 0.0 _
                , Convert.ToDecimal(TextBoxAmount.Text), Session("SSNo"), Session("PersID") _
                , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                comboTable = RetirementBOClass.CreateComboTable(dtAnnuitiesList, dtAnnuitiesListWithDeathBenefit)
                Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate") = comboTable
                Session("dtAnnuitiesFullBalanceList_BeforeExactAgeEffDate") = dtAnnuitiesListWithDeathBenefit
            Else
                Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate") = dtAnnuitiesList.Copy()
            End If

            'Step 4 SS levelling
            If (CheckboxIncludeSSLevelling.Checked = True) And (Convert.ToDouble(TextBoxSSBenefit.Text) > 0) Then
                'If Retirement Plan is selected then Attach the SS annuity to it
                If planType = "R" And CheckBoxRetPlan.Checked = True Then
                    dtAnnuitiesList = SetSSLeveling(dtAnnuitiesList, TextBoxAnnuitySelectRet.Text)
                    Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate") = SetSSLeveling(Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate"), TextBoxAnnuitySelectRet.Text)
                ElseIf planType = "S" And CheckBoxRetPlan.Checked = False Then 'If only Savings Plan is selected then attatch SS to it
                    dtAnnuitiesList = SetSSLeveling(dtAnnuitiesList, TextBoxAnnuitySelectSav.Text)
                    Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate") = SetSSLeveling(Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate"), TextBoxAnnuitySelectSav.Text)
                End If
            End If

            If planType = "R" Then
                Session("dtAnnuityList_BeforeExactAgeEffDate") = dtAnnuitiesList
            Else
                Session("dtAnnuityListSav_BeforeExactAgeEffDate") = dtAnnuitiesList
            End If

            'Step 5 Calculate the Payments
            CalculatePayments(planType)

            ''commented by Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Below commented code shifted to SetValues funcation to set values for 
            'Step 6 Assigning values to the fields on purchase tab
            'Dim calculateDeathBenefitAnnuity As Boolean
            'If Convert.ToDecimal(TextBoxAmount.Text) = 0 Then
            '    calculateDeathBenefitAnnuity = False
            'Else
            '    calculateDeathBenefitAnnuity = True
            'End If
            'SetMonthlyValuesToPurchaseTab(calculateDeathBenefitAnnuity, planType)
            'SetFirstCheckValuesToPurchaseTab()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub CalculatePayments(ByVal planType As String)
        Dim calculateDeathBenefitAnnuity As Boolean
        Dim dtFinalAnnuity As DataTable = Nothing
        Dim lcAnnuitiesListCursor As DataTable = Nothing
        Dim benDOB As DateTime
        Dim dtComboData As DataTable = Nothing
        Dim dtAnnuitiesList As DataTable = Nothing

        Try

            If Convert.ToDecimal(TextBoxAmount.Text) = 0 Or planType = "S" Then
                calculateDeathBenefitAnnuity = False
            Else
                calculateDeathBenefitAnnuity = True
            End If

            ' Display reserve on the First Tab
            ' TextBoxReservesRet is used only to display the sum and not used any where
            'If planType = "R" Then
            '    Dim actBalRet As Decimal = RetirementBOClass.GetRetirementBalance(Me.guiFundEventId, Me.RetirementDate)
            '    Me.TextBoxReservesRet.Text = actBalRet
            'Else
            '    Dim actBalSav As Decimal = RetirementBOClass.GetSavingsBalance(Me.guiFundEventId, Me.RetirementDate)
            '    Me.TextBoxReservesSav.Text = actBalSav
            'End If


            If TextBoxAnnuityBirthDateRet.Text <> String.Empty Then
                benDOB = Convert.ToDateTime(TextBoxAnnuityBirthDateRet.Text)
            Else
                benDOB = New DateTime(1900, 1, 1)
            End If

            lcAnnuitiesListCursor = New DataTable
            lcAnnuitiesListCursor = Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate")
            dtAnnuitiesList = New DataTable
            If planType = "R" Then
                dtAnnuitiesList = Session("dtAnnuityList_BeforeExactAgeEffDate")
            Else
                dtAnnuitiesList = Session("dtAnnuityListSav_BeforeExactAgeEffDate")
            End If
            dtFinalAnnuity = New DataTable
            dtFinalAnnuity = RetirementBOClass.CalculatePayments(False, Convert.ToDecimal(TextBoxAmount.Text) _
                , benDOB, Convert.ToDateTime(ycRetireeBirthDay) _
                , Convert.ToDateTime(TextBoxRetirementDate.Text), dtAnnuitiesList, dtFinalAnnuity, planType, Session("PersID"), Me.guiFundEventId)

            If planType = "R" Then
                Session("dtAnnuityList_BeforeExactAgeEffDate") = dtAnnuitiesList
            Else
                Session("dtAnnuityListSav_BeforeExactAgeEffDate") = dtAnnuitiesList
            End If

            If planType = "R" And (Convert.ToDecimal(TextBoxAmount.Text) > 0) Then
                Dim dtDBAnnuities As New DataTable
                dtFinalAnnuity = Nothing
                dtDBAnnuities = Session("dtAnnuitiesFullBalanceList_BeforeExactAgeEffDate")
                dtFinalAnnuity = RetirementBOClass.CalculatePayments(True, Convert.ToDecimal(TextBoxAmount.Text) _
                    , benDOB, Convert.ToDateTime(ycRetireeBirthDay) _
                    , Convert.ToDateTime(TextBoxRetirementDate.Text), dtDBAnnuities, dtFinalAnnuity, planType, Session("PersID"), Me.guiFundEventId)
                Session("dtAnnuitiesFullBalanceList_BeforeExactAgeEffDate") = dtDBAnnuities
            End If

            dtFinalAnnuity = Nothing
            dtFinalAnnuity = RetirementBOClass.CalculatePayments(False, Convert.ToDecimal(TextBoxAmount.Text) _
                , benDOB, Convert.ToDateTime(ycRetireeBirthDay) _
                , Convert.ToDateTime(TextBoxRetirementDate.Text), lcAnnuitiesListCursor, dtFinalAnnuity, planType, Session("PersID"), Me.guiFundEventId)
            Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate") = lcAnnuitiesListCursor

            ' Get combo data 
            dtComboData = New DataTable
            dtComboData = lcAnnuitiesListCursor


            ''commented by Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Below commented code shifted to SetValues funcation to set values for 
            '' Populate the data in the UI
            'Dim drSelectedAnnuities As DataRow()
            'Dim drSelectedAnnuity As DataRow
            'Dim dblNonTaxable As Double
            'Dim dblTaxable As Double
            'Dim dblSSIncrease As Double = 0

            'If planType = "R" Then
            '    drSelectedAnnuities = dtComboData.Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
            'Else
            '    drSelectedAnnuities = dtComboData.Select("chrAnnuityType='" & TextBoxAnnuitySelectSav.Text.Trim() & "'")
            'End If

            'If drSelectedAnnuities.Length > 0 Then
            '    drSelectedAnnuity = drSelectedAnnuities(0)
            '    dblNonTaxable = Math.Round(Convert.ToDecimal(drSelectedAnnuity("mnyPersonalPostTaxCurrentPayment") + drSelectedAnnuity("mnyYmcaPostTaxCurrentPayment")), 2)
            '    dblTaxable = Math.Round(Convert.ToDecimal(drSelectedAnnuity("mnyYmcaPreTaxCurrentPayment")) + Convert.ToDecimal(drSelectedAnnuity("mnyPersonalPreTaxCurrentPayment")), 2)

            '    'If TextBoxSSBenefit.Text.Trim() <> "" And TextBoxSSBenefit.ReadOnly = False Then
            '    '    If CType(drSelectedAnnuity("bitSSLeveling"), Boolean) = True Then
            '    '        dblSSIncrease = drSelectedAnnuity("mnySSIncrease")
            '    '    Else
            '    '        dblSSIncrease = 0
            '    '    End If
            '    'Else
            '    '    dblSSIncrease = 0
            '    'End If

            '    If planType = "R" Then
            '        TextBoxTaxableRet.Text = Math.Round(dblTaxable + dblSSIncrease, 2).ToString("#0.00")
            '        TextBoxNonTaxableRet.Text = Math.Round(dblNonTaxable, 2).ToString("#0.00")
            '        TextBoxTotalPaymentRet.Text = Math.Round(dblNonTaxable + (dblTaxable + dblSSIncrease), 2).ToString("#0.00")
            '    Else
            '        TextBoxTaxableSav.Text = Math.Round(dblTaxable + dblSSIncrease, 2).ToString("#0.00")
            '        TextBoxNonTaxableSav.Text = Math.Round(dblNonTaxable, 2).ToString("#0.00")
            '        TextBoxTotalPaymentSav.Text = Math.Round(dblNonTaxable + (dblTaxable + dblSSIncrease), 2).ToString("#0.00")
            '    End If
            'End If

            Session("dtAnnuitieslistComboTmp_BeforeExactAgeEffDate") = Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate")

            ' Get Payment data 
            Dim dtPaymentData As New DataTable
            dtPaymentData = RetirementBOClass.GetPaymentData(dtComboData)
            '----------------------------------------------------------------------------------

            'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
            Dim beneficiaryBirthDate As String
            '2012.05.28 SP : YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary 
            If planType.ToUpper() = "R" Then '2012.06.04 SP : YRS 5.0-1508
                If TextBoxAnnuityBirthDateRet.Text <> String.Empty And DropDownRelationShipRet.SelectedValue.ToUpper() <> "SP" Then
                    beneficiaryBirthDate = TextBoxAnnuityBirthDateRet.Text.Trim()
                End If
            ElseIf planType.ToUpper() = "S" Then
                If TextBoxAnnuityBirthDateSav.Text <> String.Empty And DropDownRelationShipSav.SelectedValue.ToUpper() <> "SP" Then
                    beneficiaryBirthDate = TextBoxAnnuityBirthDateSav.Text.Trim()
                End If
            End If

            'beneficiaryBirthDate = RetirementBOClass.GetNonSpouseBeneficiaryBirthDateForProcessing(Me.PersId, planType)
            '2012.05.28 SP : YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary 
            If beneficiaryBirthDate <> String.Empty Then
                Dim ageDiff As Int16
                ageDiff = YMCARET.YmcaBusinessObject.RetirementBOClass.GetRetireeAndBeneficiaryAgeDiff(ycRetireeBirthDay, beneficiaryBirthDate, TextBoxRetirementDate.Text)
                dtPaymentData = YMCARET.YmcaBusinessObject.RetirementBOClass.AvailJandSAnnuityOptions_RetProcessing(ageDiff, dtPaymentData)
            End If
            'End
            Dim ds_SelectAnnuity As New DataSet
            ds_SelectAnnuity.Tables.Add(dtPaymentData)
            ds_SelectAnnuity.AcceptChanges()
            If planType = "R" Then
                Me.SelectAnnuityRetirement = ds_SelectAnnuity
            Else
                Me.SelectAnnuitySavings = ds_SelectAnnuity
            End If

            ''Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Shifted to  SEtValues()
            'Me.RetirementDate = TextBoxRetirementDate.Text
            'Me.saveControlValuesInSessionVariables()

            'Call SetRetireType()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function SetSSLeveling(ByVal dtAnnuitiesList As DataTable, ByVal selectedAnnuity As String) As DataTable
        Dim drRows As DataRow()
        Dim drRow As DataRow
        Dim dtSSLevel As DataTable

        Try
            If TextBoxSSBenefit.Text.Trim() <> "" And TextBoxSSBenefit.ReadOnly = False Then
                If TextBoxSSBenefit.Text.Trim() = String.Empty Then
                    TextBoxSSBenefit.Text = "0.00"
                End If

                If Convert.ToDouble(TextBoxSSBenefit.Text) > 0 Then
                    Dim ssReductionFactor As Decimal
                    Dim dsSSReductionFactor As DataSet = RetirementBOClass.getSSReductionFactor(TextBoxBirthDateRet.Text, TextBoxRetirementDate.Text)
                    If (dsSSReductionFactor.Tables.Count <> 0 And dsSSReductionFactor.Tables(0).Rows.Count <> 0) Then
                        ssReductionFactor = Convert.ToDecimal(dsSSReductionFactor.Tables(0).Rows(0)("numReductionFactor"))
                    Else
                        ssReductionFactor = 0
                    End If

                    Dim ssBenefit As Decimal = Convert.ToDouble(TextBoxSSBenefit.Text)
                    Dim lnLeveling As Decimal = Math.Round((ssBenefit * ssReductionFactor), 2)

                    TextBoxSSIncrease.Text = lnLeveling
                End If
            End If

            ' 1. Decide which plan will be used for SS leveling

            ' 2. Attach the SS leveling details
            Dim drSelectedAnnuity As DataRow() = dtAnnuitiesList.Select("chrAnnuityType='" & selectedAnnuity & "'")
            If drSelectedAnnuity.Length > 0 Then
                drSelectedAnnuity(0)("bitSSLeveling") = 1
                drSelectedAnnuity(0)("mnySSIncrease") = TextBoxSSIncrease.Text
                drSelectedAnnuity(0).AcceptChanges()
            End If

            '' Set up the SS levelling data
            'If CheckboxIncludeSSLevelling.Checked = True Then

            '    Dim selDR As DataRow
            '    If Not drSelectedAnnuityRow Is Nothing Then
            '        selDR = drSelectedAnnuityRow
            '    ElseIf Not drSelectedAnnuityRowSav Is Nothing Then
            '        selDR = drSelectedAnnuityRowSav
            '    End If

            '    selDR("mnySSIncrease") = TextBoxSSIncrease.Text
            '    selDR("bitSSLeveling") = 1

            '    selDR.AcceptChanges()
            'End Ifr

            'If TextBoxSSBenefit.Text.Trim() <> "" And TextBoxSSBenefit.ReadOnly = False Then
            '    If TextBoxSSBenefit.Text.Trim() = String.Empty Then
            '        TextBoxSSBenefit.Text = "0"
            '    End If
            '    If Convert.ToDouble(TextBoxSSBenefit.Text) > 0 Then
            '        dtSSLevel = plcAnnuitiesListCursor

            '        If dtSSLevel.Rows.Count > 0 Then
            '            drRows = dtSSLevel.Select("chrAnnuityType='MS'")
            '            If drRows.Length > 0 Then
            '                drRow = drRows(0)
            '                If Not drRow.Item("mnySSIncrease") = 0 Then
            '                    TextBoxSSIncrease.Text = Math.Round(Convert.ToDecimal(drRow.Item("mnySSIncrease")), 2).ToString
            '                End If
            '            End If
            '        End If
            '    End If
            'End If
            dtAnnuitiesList.AcceptChanges()
            SetSSLeveling = dtAnnuitiesList
        Catch
            Throw
        End Try
    End Function
    Private Sub SetMonthlyValuesToPurchaseTab(ByVal llFullBalancePassed As Boolean, ByVal planType As String)
        Dim dtAnnList As DataTable
        Dim drAnnRows As DataRow()
        Dim drAnnRow As DataRow
        Dim dblSSIncrease As Double = 0
        Dim dblMonthlyGrossAnnuity As Double = 0
        Dim strDBAnnuityTpe As String = String.Empty
        Try

            'Monthly Gross Annuity Amount
            If planType = "R" Then
                dtAnnList = Session("dtAnnuityList")
            Else
                dtAnnList = Session("dtAnnuityListSav")
            End If

            If Not dtAnnList Is Nothing Then
                If planType = "R" Then
                    drAnnRows = dtAnnList.Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
                Else
                    drAnnRows = dtAnnList.Select("chrAnnuityType='" & TextBoxAnnuitySelectSav.Text.Trim() & "'")
                End If

                If drAnnRows.Length > 0 Then
                    drAnnRow = drAnnRows(0)
                    If TextBoxSSBenefit.Text.Trim() <> "" And TextBoxSSBenefit.ReadOnly = False Then
                        If CType(drAnnRow("bitSSLeveling"), Boolean) = True Then
                            dblSSIncrease = drAnnRow("mnySSIncrease")
                        Else
                            dblSSIncrease = 0
                        End If
                    Else
                        dblSSIncrease = 0
                    End If

                    strDBAnnuityTpe = CType(drAnnRow("chrDBAnnuityType"), String)
                    dblMonthlyGrossAnnuity = dblSSIncrease + drAnnRow("mnyCurrentPayment")
                End If
            Else
                dblMonthlyGrossAnnuity = 0
            End If

            If planType = "R" Then
                Session("TextBoxMonthlyGrossAnnuityRet.Text") = Round(dblMonthlyGrossAnnuity, 2).ToString.Trim()
            Else
                Session("TextBoxMonthlyGrossAnnuitySav.Text") = Round(dblMonthlyGrossAnnuity, 2).ToString.Trim()
            End If

            'Monthly Death Benefit
            dblMonthlyGrossAnnuity = 0
            dtAnnList = Session("dtAnnuitiesFullBalanceList")
            If Not dtAnnList Is Nothing And llFullBalancePassed = True Then
                drAnnRows = dtAnnList.Select("chrAnnuityType='" & strDBAnnuityTpe.Trim() & "'")
                If drAnnRows.Length > 0 Then
                    drAnnRow = drAnnRows(0)
                    dblMonthlyGrossAnnuity = drAnnRow("mnyCurrentPayment")
                End If
            Else
                dblMonthlyGrossAnnuity = 0
            End If
            If planType = "R" Then
                Session("TextBoxMonthlyGrossDBRet.Text") = Round(dblMonthlyGrossAnnuity, 2).ToString.Trim()
            End If

            'Monthly Gross Total
            dblMonthlyGrossAnnuity = 0
            dtAnnList = Session("dtAnnuitieslistComboTmp")
            If Not dtAnnList Is Nothing Then
                drAnnRows = dtAnnList.Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
                If drAnnRows.Length > 0 Then
                    drAnnRow = drAnnRows(0)
                    dblMonthlyGrossAnnuity = dblSSIncrease + drAnnRow("mnyCurrentPayment")
                End If
            Else
                dblMonthlyGrossAnnuity = 0
            End If

            If planType = "R" Then
                Session("TextBoxMonthlyGrossTotalRet.Text") = Round(dblMonthlyGrossAnnuity, 2).ToString.Trim()
            Else
                Session("TextBoxMonthlyGrossTotalSav.Text") = Round(dblMonthlyGrossAnnuity, 2).ToString.Trim()
            End If

            Me.populateCheckValues()

            ' Get WithHolding amount
            TextBoxMonthlyWithheld.ReadOnly = True
            TextBoxMonthlyNetTotal.ReadOnly = True
            Dim l_double_WHAmount As Double
            l_double_WHAmount = GetWithHolding()
            TextBoxMonthlyWithheld.Text = l_double_WHAmount
            TextBoxMonthlyNetTotal.Text = Convert.ToDouble(TextBoxMonthlyGrossTotal.Text) - Convert.ToDouble(TextBoxMonthlyWithheld.Text)
        Catch
            Throw
        End Try
    End Sub
    Private Sub SetFirstCheckValuesToPurchaseTab() 'ByVal llFullBalancePassed As Boolean)
        Try
            'Calculate values for the Withheld TextBoxes
            Dim lnMonthsinCheckOne As Decimal

            TextBoxFirstCheckWithheld.ReadOnly = True
            TextBoxFirstCheckGrossAnnuity.ReadOnly = True
            TextBoxFirstCheckGrossDB.ReadOnly = True
            TextBoxFirstCheckGrossTotal.ReadOnly = True
            TextBoxFirstCheckNet.ReadOnly = True

            lnMonthsinCheckOne = GetMonthsinCheckOne()
            Me.MonthsinCheckOne = lnMonthsinCheckOne

            'TextBoxFirstCheckGrossAnnuity.Text = Convert.ToDecimal(TextBoxMonthlyGrossAnnuity.Text) * lnMonthsinCheckOne
            'TextBoxFirstCheckGrossDB.Text = Convert.ToDecimal(TextBoxMonthlyGrossDB.Text) * lnMonthsinCheckOne
            'TextBoxFirstCheckGrossTotal.Text = Convert.ToDecimal(TextBoxMonthlyGrossTotal.Text) * lnMonthsinCheckOne
            'TextBoxFirstCheckWithheld.Text = Convert.ToDecimal(TextBoxMonthlyWithheld.Text) * lnMonthsinCheckOne
            'TextBoxFirstCheckNet.Text = Convert.ToDecimal(TextBoxMonthlyNetTotal.Text) * lnMonthsinCheckOne
        Catch
            Throw
        End Try
    End Sub
    Private Sub calculateAnnuity(ByVal planType As String)
        ''SP 2013.12.13 BT-2326 -Adding try catch block
        Try


            ' Clear the session variables.
            Session("TextBoxMonthlyGrossAnnuityRet.Text") = 0
            Session("TextBoxMonthlyGrossAnnuitySav.Text") = 0
            Session("TextBoxMonthlyGrossDBRet.Text") = 0
            Session("TextBoxMonthlyGrossTotalRet.Text") = 0
            Session("TextBoxMonthlyGrossTotalSav.Text") = 0
            TextBoxSSIncrease.Text = "0.00"
            ' We can introduce the Account Balance Check here.
            'Priya 2010.11.17 Exact age annuity Effect
            TransAfterExactAgeEffDate = YMCARET.YmcaBusinessObject.RetirementBOClass.ValidationForAfterExactAgeEff(guiFundEventId)
            'If TransAfterExactAgeEffDate = 1 Then
            '    'Calculate 2 aanuity menthod
            'End If
            'End Priya 2010.11.17 Exact age annuity Effect



            'Priya 2010.11.19 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Called CalcBasedOnRetirementDate() to decide which annuity need to calculate ExactAgeEffDate or old
            Dim strCalledAnnuity As String
            If Me.RetireType.ToUpper() = "NORMAL" Then
                strCalledAnnuity = CalcBasedOnRetirementDate()
                If (planType = "B" Or planType = "R") And CheckBoxRetPlan.Checked Then
                    If strCalledAnnuity = "OLD" Then
                        GetProjectedAnnuities("R")
                        SetSessionValuesBasedOnSelectedOption("R", strCalledAnnuity)
                    ElseIf strCalledAnnuity = "NEW" Then
                        GetProjectedAnnuities_ExactAgeEffDate("R", Me.RetireType)
                        SetSessionValuesBasedOnSelectedOption("R", strCalledAnnuity)
                    ElseIf strCalledAnnuity = "BOTH" Then
                        GetProjectedAnnuities("R")
                        GetProjectedAnnuities_ExactAgeEffDate("R", Me.RetireType)
                        CompareAnnuities(SelectAnnuityRetirementExactAgeEffDate, SelectAnnuityRetirement, "R")
                    End If
                    'CompaireAnnuities(SelectAnnuityRetirementExactAgeEffDate, SelectAnnuityRetirement, "R")
                    setValues("R")
                    'GetProjectedAnnuities("R")
                End If
                If (planType = "B" Or planType = "S") And CheckBoxSavPlan.Checked Then
                    If strCalledAnnuity = "OLD" Then
                        GetProjectedAnnuities("S")
                        SetSessionValuesBasedOnSelectedOption("S", strCalledAnnuity)
                    ElseIf strCalledAnnuity = "NEW" Then
                        GetProjectedAnnuities_ExactAgeEffDate("S", Me.RetireType)
                        SetSessionValuesBasedOnSelectedOption("S", strCalledAnnuity)
                    ElseIf strCalledAnnuity = "BOTH" Then
                        GetProjectedAnnuities("S")
                        GetProjectedAnnuities_ExactAgeEffDate("S", Me.RetireType)
                        CompareAnnuities(SelectAnnuitySavingsExactAgeEffDate, SelectAnnuitySavings, "S")
                    End If
                    'CompaireAnnuities(SelectAnnuitySavingsExactAgeEffDate, SelectAnnuitySavings, "S")
                    setValues("S")
                    'GetProjectedAnnuities("S")
                End If
                'Sanket Vaidya          17 Feb 2011     For BT 665 : For disability requirement
            ElseIf Me.RetireType.ToUpper() = "DISABL" Then
                strCalledAnnuity = "NEW"
                Session("CalledAnnuity") = strCalledAnnuity
                If (planType = "B" Or planType = "R") And CheckBoxRetPlan.Checked Then
                    GetProjectedAnnuities_ExactAgeEffDate_Disability("R", Me.RetireType)
                    SetSessionValuesBasedOnSelectedOption("R", strCalledAnnuity)
                    setValues("R")
                End If
                If (planType = "B" Or planType = "S") And CheckBoxSavPlan.Checked Then
                    'START: PPP | 03/08/2017 | YRS-AT-2625 | Savings plan annuities for disability will be calculated using "disability factors"
                    GetProjectedAnnuities_ExactAgeEffDate("S", Me.RetireType)
                    'GetProjectedAnnuities_ExactAgeEffDate("S", "NORMAL")
                    'END: PPP | 03/08/2017 | YRS-AT-2625 | Savings plan annuities for disability will be calculated using "disability factors"
                    SetSessionValuesBasedOnSelectedOption("S", strCalledAnnuity)
                    setValues("S")
                End If

            End If

            'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculationsc
            'Called new method after using exact age functionality
            'If CheckBoxRetPlan.Checked Then
            '    CompaireAnnuities(SelectAnnuityRetirementExactAgeEffDate, SelectAnnuityRetirement, "R")
            '    setValues("R")
            'End If
            'If CheckBoxSavPlan.Checked Then
            '    Me.CompaireAnnuities(SelectAnnuitySavingsExactAgeEffDate, SelectAnnuitySavings, "S")
            '    setValues("S")
            'End If
            ''commented by Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Below commented code shifted to SetValues funcation to set values for 

            'Me.loadSSLevellingControls()
            'displayPurchaseInfo()




            'From GetProjectedAnnuities_ExactAgeEffDate
            'Step 6 Assigning values to the fields on purchase tab


            'TextBoxRetiredBenefit.Text = Math.Round(Session("CalculatedDB"), 2)
            'TextBoxAmount.Text = Math.Round(Convert.ToDecimal((TextBoxRetiredBenefit.Text) * (DropDownListPercentage.SelectedValue) / 100), 2)
            'ASHISH:2011.08.22 YRS 5.0-1345 :BT-859 Death benefit not available for QD participant
            If Me.OrgBenTypeIsQDRO = True Then
                TextBoxRetiredBenefit.Text = "0.0"
                TextBoxAmount.Text = "0.0"
                DropDownListPercentage.SelectedIndex = 0
            Else
                TextBoxRetiredBenefit.Text = Math.Round(Session("CalculatedDB"), 2)
                TextBoxAmount.Text = Math.Round(Convert.ToDecimal((TextBoxRetiredBenefit.Text) * (DropDownListPercentage.SelectedValue) / 100), 2)

            End If

            Dim calculateDeathBenefitAnnuity As Boolean
            If Convert.ToDecimal(TextBoxAmount.Text) = 0 Then
                calculateDeathBenefitAnnuity = False
                'Anudeep 30.11.2012 Bt-1462-Instead of Showing "***" is better to show a help Image
                'LabelDBIncluded.Visible = False
                imgHelp.Visible = False
            Else
                calculateDeathBenefitAnnuity = True
                'Anudeep 30.11.2012 Bt-1462-Instead of Showing "***" is better to show a help Image
                'LabelDBIncluded.Visible = True
                imgHelp.Visible = True
            End If


            ''FROM CALCULARE ANUUITY
            Me.loadSSLevellingControls()
            displayPurchaseInfo()
            'ASHISH:2011.12.09:YRS 5.0-1353
            strCalledAnnuity = Session("CalledAnnuity")
            'ASHISH:2010.12.16 for display old new method in summry tab
            If strCalledAnnuity = "BOTH" And Me.TransAfterExactAgeEffDate = "1" Then
                LoadCalledAnnuitySummaryTab()
                pnlAllowAnnuityInSummaryTab.Visible = True
            Else
                pnlAllowAnnuityInSummaryTab.Visible = False
            End If
            SetMonthlyValuesToPurchaseTab(calculateDeathBenefitAnnuity, planType)
            SetFirstCheckValuesToPurchaseTab()

            'From CalculatePayamets.
            Me.RetirementDate = TextBoxRetirementDate.Text
            Me.saveControlValuesInSessionVariables()
            'Call SetRetireType()

        Catch
            Throw
        End Try

    End Sub

    'SP 2014.09.17 -YRS 5.0-2362 -Start
    Private Sub SetJointSurviourAnnuityDefaultToM()
        Dim strPlan As String
        Dim bIsResetAnnutiy As Boolean
        Try

            Dim isRetBeneficiaryChanged As Boolean = IIf(ViewState("IsRetBenificiaryChanged") IsNot Nothing AndAlso ViewState("IsRetBenificiaryChanged") = True, True, False)
            Dim isSavBeneficiaryChanged As Boolean = IIf(ViewState("IsSavBenificiaryChanged") IsNot Nothing AndAlso ViewState("IsSavBenificiaryChanged") = True, True, False)

            If (TextBoxAnnuitySelectRet.Text.ToUpper.Trim.Contains("J") AndAlso isRetBeneficiaryChanged) AndAlso (TextBoxAnnuitySelectSav.Text.ToUpper.Trim.Contains("J") AndAlso isSavBeneficiaryChanged) Then
                TextBoxAnnuitySelectRet.Text = "M"
                TextBoxAnnuitySelectSav.Text = "M"
                strPlan = "both the plans"
                bIsResetAnnutiy = True
                Me.EnableDisableValidationBeneficiarySav(False)
                Me.EnableDisableValidationBeneficiary(False)
            ElseIf TextBoxAnnuitySelectRet.Text.ToUpper.Trim.Contains("J") AndAlso isRetBeneficiaryChanged Then
                TextBoxAnnuitySelectRet.Text = "M"
                strPlan = "Retirement plan"
                bIsResetAnnutiy = True

                Me.EnableDisableValidationBeneficiary(False)
            ElseIf TextBoxAnnuitySelectSav.Text.ToUpper.Trim.Contains("J") AndAlso isSavBeneficiaryChanged Then
                TextBoxAnnuitySelectSav.Text = "M"
                strPlan = "Savings plan"
                bIsResetAnnutiy = True
                Me.EnableDisableValidationBeneficiarySav(False)
            End If
            If (bIsResetAnnutiy = True) Then
                HelperFunctions.ShowMessageToUser(String.Format(getmessage("MESSAGE_RETIREMENT_PROCESSING_JOINT_SURVIOUR_ANNUITY_RESET_MESSAGE"), strPlan), EnumMessageTypes.Warning)
            End If
            ViewState("IsSavBenificiaryChanged") = False
            ViewState("IsRetBenificiaryChanged") = False
        Catch
            Throw
        End Try
    End Sub
    'SP 2014.09.17 -YRS 5.0-2362 -End
#End Region

#Region "Code for tabs"
    Private Sub LoadControls()
        Dim dsPersRetiree As New DataSet
        Dim dsRetEmpInfo As New DataSet
        'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
        'Dim dsRetEmpSalInfo As New DataSet
        Dim ycRetireeRetirementDate As String

        Me.Server.ScriptTimeout = 90000000
        Try
            'Loading dataset with Basic Retiree information
            dsPersRetiree = Me.PersonDetails

            'Loading dataset with Basic Retiree and beneficiary information (dsBeneficiaryInfo)
            'loading dataset with retiree's Employment information YMCA name, Start Work date and End Work Date (dsRetEmpInfo)
            If HelperFunctions.isNonEmpty(dsPersRetiree) _
                AndAlso dsPersRetiree.Tables(0).Rows(0).IsNull("guiUniqueID") = False _
                    AndAlso dsPersRetiree.Tables(0).Rows(0).Item("guiUniqueID").ToString() <> String.Empty Then

                Dim strSSN As String = Me.SSNo
                strSSN = strSSN.Insert(3, "-")
                strSSN = strSSN.Insert(6, "-")

                'Headercontrol.PageTitle = "Retirement Processing"
                'Headercontrol.SSNo = Me.SSNo.Trim

                'for the time being Left Label title ---because this session  Me.Person_Info  is based on this title which is used in multiple pages for diff-diff purpose

                Me.Person_Info = " for " _
                                & dsPersRetiree.Tables(0).Rows(0).Item("chvLastName").ToString() _
                                & ", " & dsPersRetiree.Tables(0).Rows(0).Item("chvFirstName").ToString() _
                                & " SS#: " & strSSN

                Me.PersId = dsPersRetiree.Tables(0).Rows(0).Item("guiUniqueID").ToString()
                Me.SSNo = dsPersRetiree.Tables(0).Rows(0).Item("chrSSNO").ToString()

                'YRPS-4704
                Session("MaritalStatus") = dsPersRetiree.Tables(0).Rows(0).Item("chvMaritalStatusCode").ToString()

                '-----------------------------------------------------------------------------------------------
                'Loading Employee and Beneficiary information into respective fields
                If dsPersRetiree.Tables(0).Rows(0).IsNull("dtmBirthDate") = False _
                    AndAlso dsPersRetiree.Tables(0).Rows(0).Item("dtmBirthDate").ToString() <> String.Empty Then
                    TextBoxBirthDateRet.Text = Convert.ToDateTime(dsPersRetiree.Tables(0).Rows(0).Item("dtmBirthDate")).ToShortDateString()
                End If

                ycRetireeBirthDay = dsPersRetiree.Tables(0).Rows(0).Item("dtmBirthDate").ToString

                TextBoxRetirementDate.BackColor = Color.LightYellow

                'Moving QDRO related code to the is Not Postback section of the page load event

                If Page.IsPostBack Then
                    dsRetEmpInfo = Me.RetEmpInfo
                End If

                If Not Page.IsPostBack Then
                    'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                    'dsRetEmpInfo = BORetireeEstimate.SearchRetEmpInfo(dsPersRetiree.Tables(0).Rows(0).Item("guiUniqueID").ToString())
                    dsRetEmpInfo = BORetireeEstimate.SearchRetEmpInfo(Me.guiFundEventId, Me.RetireType, IIf(String.IsNullOrEmpty(TextBoxRetirementDate.Text), "01/01/1900", TextBoxRetirementDate.Text)) 'MMR | 2017.03.09 | YRS-AT-2625 | Added parameter for retire type and retirement date
                    'Added by Sanket for Disability
                    Session("RetEmpInfo") = dsRetEmpInfo
                    If HelperFunctions.isEmpty(dsRetEmpInfo) AndAlso Me.OrgBenTypeIsQDRO = False Then
                        Session("RP_EmpHistoryInfoNotset") = True
                        'Call ShowCustomMessage("Unable to initialize employment history for the member."), enumMessageBoxType.DotNet)
                        Exit Sub
                    ElseIf HelperFunctions.isNonEmpty(dsRetEmpInfo) Then
                        'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 Start
                        'If dsRetEmpInfo.Tables(0).Rows(0).Item("End") Is DBNull.Value Then
                        '    'ASHISH:2009.11.17 -change persid with fundeventid
                        '    'dsRetEmpSalInfo = BORetireeEstimate.SearchRetEmpSalInfo( _
                        '    'dsPersRetiree.Tables(0).Rows(0).Item("guiUniqueID").ToString(), _
                        '    'dsRetEmpInfo.Tables(0).Rows(0).Item("YmcaID").ToString(), _
                        '    'Convert.ToDateTime(dsRetEmpInfo.Tables(0).Rows(0).Item("Start").ToString()).ToShortDateString(), _
                        '    '"")
                        '    dsRetEmpSalInfo = BORetireeEstimate.SearchRetEmpSalInfo( _
                        '    Me.guiFundEventId, _
                        '    dsRetEmpInfo.Tables(0).Rows(0).Item("YmcaID").ToString(), _
                        '    Convert.ToDateTime(dsRetEmpInfo.Tables(0).Rows(0).Item("Start").ToString()).ToShortDateString(), _
                        '    "")
                        'Else
                        '    'ASHISH:2009.11.17 -change persid with fundeventid
                        '    'dsRetEmpSalInfo = BORetireeEstimate.SearchRetEmpSalInfo( _
                        '    'dsPersRetiree.Tables(0).Rows(0).Item("guiUniqueID").ToString(), _
                        '    'dsRetEmpInfo.Tables(0).Rows(0).Item("YmcaID").ToString(), _
                        '    'Convert.ToDateTime(dsRetEmpInfo.Tables(0).Rows(0).Item("Start").ToString()).ToShortDateString(), _
                        '    'Convert.ToDateTime(dsRetEmpInfo.Tables(0).Rows(0).Item("End").ToString()).ToShortDateString())
                        '    dsRetEmpSalInfo = BORetireeEstimate.SearchRetEmpSalInfo( _
                        '    Me.guiFundEventId, _
                        '    dsRetEmpInfo.Tables(0).Rows(0).Item("YmcaID").ToString(), _
                        '    Convert.ToDateTime(dsRetEmpInfo.Tables(0).Rows(0).Item("Start").ToString()).ToShortDateString(), _
                        '    Convert.ToDateTime(dsRetEmpInfo.Tables(0).Rows(0).Item("End").ToString()).ToShortDateString())
                        'End If


                        'If Not dsRetEmpSalInfo.Tables(0).Rows.Count = 0 Then
                        '    If Not dsRetEmpSalInfo.Tables(1).Rows(0).Item("LastSalPaidMonth") Is DBNull.Value Then
                        '        ycRetireeRetirementDate = Convert.ToDateTime(Convert.ToDateTime(dsRetEmpSalInfo.Tables(1).Rows(0).Item("LastSalPaidMonth")).Month & "/01/" & _
                        '                                     Convert.ToDateTime(dsRetEmpSalInfo.Tables(1).Rows(0).Item("LastSalPaidMonth")).Year).ToString("MM/dd/yyyy")
                        '        Me.LastSalPaidDate = ycRetireeRetirementDate
                        '    End If
                        'End If
                        'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 END

                        If dsRetEmpInfo.Tables(1).Rows(0).IsNull("LastSalPaidMonth") = False Then
                            Dim d As DateTime = Convert.ToDateTime(dsRetEmpInfo.Tables(1).Rows(0).Item("LastSalPaidMonth"))
                            ycRetireeRetirementDate = Convert.ToDateTime(d.Month & "/01/" & d.Year).ToString("MM/dd/yyyy")
                            Me.LastSalPaidDate = ycRetireeRetirementDate
                        End If

                    End If
                End If
            End If

            If Me.OrgBenTypeIsQDRO = True Then
                Me.LastSalPaidDate = GetFirstDayOfMonth(Now().ToString("MM/dd/yyyy"))
            End If

            Call SetRetirementDate()

            'Loading Employment Information
            Me.RetEmpInfo = dsRetEmpInfo

            ' Load the controls for the first time.
            If Not IsPostBack Then
                Dim dsBeneficiaryInfo As New DataSet
                'dsBeneficiaryInfo = BORetireeEstimate.SearchRetEstInfo(Me.PersId)
                '2012.05.31 SP :1508
                'dsBeneficiaryInfo = BORetireeEstimate.getParticipantBeneficiaries(Me.PersId)

                'Me.BeneficiaryInfo = dsBeneficiaryInfo
                '2012.05.31 SP :1508



                loadAnnuityControls()
                calculateAnnuity("B")

                'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculationsc
                'Called new method after using exact age functionality


                'If CheckBoxRetPlan.Checked Then
                '    CompaireAnnuities(SelectAnnuityRetirementExactAgeEffDate, SelectAnnuityRetirement, "R")
                '    setValues("R")
                'End If
                'If CheckBoxSavPlan.Checked Then
                '    Me.CompaireAnnuities(SelectAnnuitySavingsExactAgeEffDate, SelectAnnuitySavings, "S")
                '    setValues("S")
                'End If
                'If CheckBoxRetPlan.Checked = True AndAlso CheckBoxSavPlan.Checked = True Then
                '    CompaireAnnuities(SelectAnnuityRetirementExactAgeEffDate, SelectAnnuityRetirement)
                'ElseIf CheckBoxRetPlan.Checked = True Then
                '    CompaireAnnuities(SelectAnnuityRetirementExactAgeEffDate, SelectAnnuityRetirement)
                'ElseIf CheckBoxSavPlan.Checked = True Then
                '    CompaireAnnuities(SelectAnnuitySavingsExactAgeEffDate, SelectAnnuitySavings)
                'End If
                'setValues()
                'End Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations

                'START: MMR | 2017.03.06 | YRS-AT-2625 | Loading manual transaction details
                If (Me.RetireType = "DISABL") Then
                    LoadManualTransactionForDisability(Me.guiFundEventId, Me.RetireType, TextBoxRetirementDate.Text)
                End If
                'END: MMR | 2017.03.06 | YRS-AT-2625 | Loading manual transaction details
            End If
        Catch
            Throw
        End Try
    End Sub
    Public Sub LoadBeneficiariesTab()
        Try
            Dim l_string_PersId As String
            Dim l_dataset_Active As New DataSet
            Dim l_dataset_Retired As New DataSet
            'Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            Dim l_dataset_BeneficiaryAddress As New DataSet

            l_string_PersId = Me.PersId
            If Me.Flag = "AddBeneficiaries" Or Me.Flag = "EditBeneficiaries" Or Me.Flag = "DeleteBeneficiaries" Then
                If Not Me.BeneficiariesActive Is Nothing Then
                    Me.Blank = False
                    Me.DataGridActiveBeneficiaries.DataSource = Me.BeneficiariesActive
                    Me.DataGridActiveBeneficiaries.DataBind()
                    l_dataset_Active = Me.BeneficiariesActive
                End If

                If Not Me.BeneficiariesRetired Is Nothing Then
                    Me.Blank = False
                    Me.DataGridRetiredBeneficiaries.DataSource = Me.BeneficiariesRetired
                    Me.DataGridRetiredBeneficiaries.DataBind()
                    l_dataset_Retired = Me.BeneficiariesRetired
                    CalculateValues(l_dataset_Retired.Tables(0), "R")
                End If
            Else
                If Me.Flag = "" Then
                    If Me.BeneficiariesActive Is Nothing Then
                        l_dataset_Active = RetirementBOClass.GetActiveBeneficiaries(l_string_PersId)
                        Me.BeneficiariesActive = l_dataset_Active
                    Else
                        l_dataset_Active = Me.BeneficiariesActive
                    End If

                    If Me.BeneficiariesRetired Is Nothing Then
                        l_dataset_Retired = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpRetiredBeneficiariesInfo(l_string_PersId)
                        Me.BeneficiariesRetired = l_dataset_Retired
                    Else
                        l_dataset_Retired = Me.BeneficiariesRetired
                    End If
                    'Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    If Me.BeneficiaryAddress Is Nothing Then
                        l_dataset_BeneficiaryAddress = Address.GetAddressOfBeneficiariesByPerson(l_string_PersId, EnumEntityCode.PERSON)
                        If Not l_dataset_BeneficiaryAddress Is Nothing Then
                            BeneficiaryAddress = l_dataset_BeneficiaryAddress.Tables(0)
                        End If

                    End If

                End If
                If Me.Flag = "Active" Or Me.Flag = "" Then
                    Me.BeneficiariesActive = l_dataset_Active

                    'If l_dataset_Active.Tables(0).Rows.Count <> 0 Then 'Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented code to not check tables count 
                    Me.Blank = False
                    If Not IsPostBack Then
                        Me.DataGridActiveBeneficiaries.DataSource = l_dataset_Active
                        Me.DataGridActiveBeneficiaries.DataBind()
                    End If
                    'Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented code to not bind columns to grid through datatable as bounded columns used
                    'Else
                    '    Me.Blank = True
                    '    Dim l_Active As New DataTable

                    '    l_Active.Columns.Add("Name")
                    '    l_Active.Columns.Add("Name2")
                    '    l_Active.Columns.Add("TaxID")
                    '    l_Active.Columns.Add("Rel")
                    '    l_Active.Columns.Add("Birthdate")
                    '    l_Active.Columns.Add("Groups")
                    '    l_Active.Columns.Add("Lvl")
                    '    l_Active.Columns.Add("Pct")
                    '    l_Active.Columns.Add("PlanType")
                    '    Dim dr As DataRow
                    '    DataGridActiveBeneficiaries.DataSource = l_Active
                    '    DataGridActiveBeneficiaries.DataBind()
                    'End If
                    'End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented code to not bind columns to grid through datatable as bounded columns used
                End If
                If Me.Flag = "Retire" Or Me.Flag = "" Then
                    If Me.BeneficiariesRetired Is Nothing Then
                        Me.BeneficiariesRetired = l_dataset_Retired
                    Else
                        l_dataset_Retired = Me.BeneficiariesRetired
                    End If

                    If l_dataset_Retired.Tables(0).Rows.Count <> 0 Then
                        Me.Blank = False
                        If Not IsPostBack Then
                            Me.DataGridRetiredBeneficiaries.DataSource = l_dataset_Retired
                            Me.DataGridRetiredBeneficiaries.DataBind()
                        End If
                        CalculateValues(l_dataset_Retired.Tables(0), "R")
                    Else
                        'Controls to be made invisible
                        Me.Blank = True
                        Dim l_Active As New DataTable

                        l_Active.Columns.Add("Name")
                        l_Active.Columns.Add("Name2")
                        l_Active.Columns.Add("TaxID")
                        l_Active.Columns.Add("Rel")
                        'Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented existing code and changed column name in datatable
                        'l_Active.Columns.Add("Birthdate")
                        l_Active.Columns.Add("Birth/Estd. Date")
                        'End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented existing code and changed column name in datatable
                        l_Active.Columns.Add("Groups")
                        l_Active.Columns.Add("Lvl")
                        l_Active.Columns.Add("Pct")
                        l_Active.Columns.Add("PlanType")

                        Dim dr As DataRow
                        DataGridRetiredBeneficiaries.DataSource = l_Active
                        DataGridRetiredBeneficiaries.DataBind()
                    End If
                End If
            End If

            Dim isActive As Boolean
            If Not l_dataset_Active Is Nothing Then
                If l_dataset_Active.Tables.Count > 0 Then
                    If l_dataset_Active.Tables(0).Rows.Count > 0 Then
                        isActive = True
                    End If
                End If
            End If

            ' If Primary and Retired benefeciaries are present 
            ' then disable ButtonMoveBeneficiaries
            If Not l_dataset_Retired Is Nothing And l_dataset_Retired.Tables.Count > 0 Then
                Dim dr As DataRow() = l_dataset_Retired.Tables(0).Select("BeneficiaryTypeCode = 'RETIRE' AND Groups='PRIM'")
                If dr.Length > 0 Then
                    ButtonMoveBeneficiaries.Enabled = False
                End If
            End If

            PrepareAuditTable()   ' // SB | 07/07/2016 | YRS-AT-2382 | For Adding Beneficiaries in datatable to  maintain changed SSN updates
        Catch
            Throw
        End Try
    End Sub
    Public Sub LoadNotesTab()
        Try
            Dim dr As DataRow
            Dim l_datatable_Notes As DataTable
            Dim l_string_PersId As String
            l_string_PersId = Me.PersId
            l_datatable_Notes = Me.Notes

            'START: SB | 2019.03.15 | BT-12078 | Commenting unwanted code, the below approch of saving data in datatable is used no where in application to store atsnotes in database
            'If Me.Flag = "AddNotes" Then
            '    If l_datatable_Notes Is Nothing Then
            '        l_datatable_Notes = New DataTable
            '        l_datatable_Notes.Columns.Add("UniqueId")
            '        l_datatable_Notes.Columns.Add("PersonId")
            '        l_datatable_Notes.Columns.Add("NoteType")
            '        l_datatable_Notes.Columns.Add("Date")
            '        l_datatable_Notes.Columns.Add("Creator")
            '        l_datatable_Notes.Columns.Add("Note")
            '        l_datatable_Notes.Columns.Add("bitImportant")
            '    End If

            '    dr = l_datatable_Notes.NewRow
            '    dr("UniqueId") = Guid.NewGuid()
            '    dr("PersonID") = l_string_PersId
            '    dr("Note") = Session("Note")
            '    dr("Date") = Date.Now
            '    dr("Creator") = Session("LoginId")
            '    dr("bitImportant") = Session("BitImportant")
            '    l_datatable_Notes.Rows.Add(dr)

            '    'After adding the row to the dataset reset the session variable()
            '    Session("blnAddNotes") = False
            '    Session("blnUpdateNotes") = False
            'Else
            'END: SB | 2019.03.15 | BT-12078 | Commenting unwanted code, the below approch of saving data in datatable is used no where in application to store atsnotes in database
                If l_datatable_Notes Is Nothing Then
                    l_datatable_Notes = YMCARET.YmcaBusinessObject.RefundRequest.MemberNotes(l_string_PersId)
                Else
                    If Not l_datatable_Notes.GetChanges Is Nothing Then
                        If Not l_datatable_Notes.GetChanges.Rows.Count > 0 Then
                            l_datatable_Notes = YMCARET.YmcaBusinessObject.RefundRequest.MemberNotes(l_string_PersId)
                        End If
                        If Session("blnCancel") = True Then
                            l_datatable_Notes = YMCARET.YmcaBusinessObject.RefundRequest.MemberNotes(l_string_PersId)
                            Session("blnCancel") = False
                        End If
                    End If
                'START: SB | 2019.03.15 | BT-12078 | After saving the notes in database, get the latest records form the database to display in notes grid
                If Session("NotesEntityID") = "" Then
                    l_datatable_Notes = YMCARET.YmcaBusinessObject.RefundRequest.MemberNotes(l_string_PersId)
                End If
                'END: SB | 2019.03.15 | BT-12078 | After saving the notes in database, get the latest records form the database to display in notes grid
            End If
            'End If  'SB | 2019.03.15 | BT-12078 | Commenting unwanted code, the below approch of saving data in datatable is used no where in application to store atsnotes in database

            If (l_datatable_Notes.Rows.Count > 0) Then
                Me.NotesFlag.Value = "Notes"
                'Commented by Dilip Yadav : 21-Sep-09 : Since same logic is handled in next statement.
                'DataGridNotes.DataSource = l_datatable_Notes
                'DataGridNotes.DataBind()
            End If

            DataGridNotes.DataSource = l_datatable_Notes
            'START: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.
            Session("DisplayNotes") = l_datatable_Notes
            'END: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.
            DataGridNotes.DataBind()

            If Me.NotesFlag.Value = "Notes" Then
                Me.TabStripRetireesInformation.Items(5).Text = "<font color=orange>Notes</font>"
            ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
                Me.TabStripRetireesInformation.Items(5).Text = "<font color=red>Notes</font>"
            Else
                Me.TabStripRetireesInformation.Items(5).Text = "Notes"
            End If

            Session("blnCancel") = False
            Me.Notes = l_datatable_Notes
        Catch
            Throw
        End Try
    End Sub
    Protected Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim l_datatable_Notes As New DataTable
            Dim l_string_Uniqueid As String
            Dim l_String_Search As String
            Dim l_datarow As DataRow()
            Dim l_datarowUpdated As DataRow
            l_datatable_Notes = Me.Notes
            Dim l_checkbox As CheckBox = CType(sender, CheckBox)
            Dim dgItem As DataGridItem = CType(l_checkbox.NamingContainer, DataGridItem)

            If dgItem.Cells(4).Text.ToUpper <> "&NBSP;" Then
                l_string_Uniqueid = dgItem.Cells(1).Text
                l_String_Search = " UniqueID = '" + l_string_Uniqueid + "'"

                l_datarow = l_datatable_Notes.Select(l_String_Search)
                l_datarowUpdated = l_datarow(0)

                If l_checkbox.Checked = True Then
                    Me.NotesFlag.Value = "MarkedImportant"
                    l_datarowUpdated("bitImportant") = 1
                Else
                    l_datarowUpdated("bitImportant") = 0
                End If
                Me.Notes = l_datatable_Notes
                Me.setNotesTabColor()
            End If

        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("Check_Clicked-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Public Sub LoadFedWithDrawalTab()
        'START : ML | 2019.09.20 | YRS-AT-4597 |Declare Variables 
        Dim totalAnnuityAmount As Double
        Dim totalWithholdAmount As Double
        Dim FederalWithholdAmount As Double
        'END : ML | 2019.09.20 | YRS-AT-4597 |Declare controls 
        Try
            Dim dr As DataRow
            Dim l_string_PersId As String
            l_string_PersId = Me.PersId
            Dim l_dataset_FedWith As DataSet
            l_dataset_FedWith = Me.FedWithDrawals

            If Me.blnAddFedWithHoldings = True Or Me.blnUpdateFedWithHoldings = True And Not Session("blnCancel") = True Then
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
                If Not (Me.blnUpdateFedWithHoldings = True) Then
                    dr = l_dataset_FedWith.Tables(0).NewRow
                    dr("Tax Entity") = Session("cmbTaxEntity")
                    dr("Type") = Session("cmbWithHolding")
                    dr("Add'l Amount") = Session("txtAddlAmount")
                    dr("Marital Status") = Session("cmbMaritalStatus")
                    dr("Exemptions") = Session("txtExemptions")
                    dr("persid") = l_string_PersId
                    l_dataset_FedWith.Tables(0).Rows.Add(dr)
                End If

                'After adding the row to the dataset reset the session variable()
                Me.blnAddFedWithHoldings = False
                Me.blnUpdateFedWithHoldings = False
            Else

                If l_dataset_FedWith Is Nothing Then
                    l_dataset_FedWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpFedWithDrawals(l_string_PersId)
                Else
                    If Not l_dataset_FedWith.Tables(0).GetChanges Is Nothing Then
                        If Not l_dataset_FedWith.Tables(0).GetChanges.Rows.Count > 0 Then
                            l_dataset_FedWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpFedWithDrawals(l_string_PersId)
                        End If
                        If Session("blnCancel") = True Then
                            l_dataset_FedWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpFedWithDrawals(l_string_PersId)
                            Session("blnCancel") = False
                        End If
                    End If
                End If
            End If

            DataGridFederalWithholding.DataSource = l_dataset_FedWith
            DataGridFederalWithholding.DataBind()

            Session("blnCancel") = False
            Me.FedWithDrawals = l_dataset_FedWith
            'START : ML | 2019.10.18 | YRS-AT-4597 |Set Annuity Amount property for Statewithholding UserControl
            totalAnnuityAmount = getAnnuityAmount()
            Session("AnnuityAmount") = totalAnnuityAmount
            totalWithholdAmount = RetirementBOClass.GetTotalWithHoldingAmount(Me.FedWithDrawals, Me.GenWithDrawals, Me.MonthsinCheckOne, totalAnnuityAmount, Me.RetirementDate)
            stwListUserControl.GrossAmount = totalAnnuityAmount - totalWithholdAmount
            If (HelperFunctions.isNonEmpty(Me.FedWithDrawals)) Then

                    Me.stwFederalAmount = RetirementBOClass.GetFedWithDrawalAmount(Me.FedWithDrawals, Me.MonthsinCheckOne, totalAnnuityAmount)
                Me.stwFederalType = Me.FedWithDrawals.Tables(0).Rows(0)("Type").ToString().ToUpper()

            Else
                Me.stwFederalAmount = Nothing
                Me.stwFederalType = String.Empty
            End If
            stwListUserControl.FederalAmount = Me.stwFederalAmount
            stwListUserControl.FederalType = Me.stwFederalType
            'END : ML | 2019.10.18 | YRS-AT-4597 |Set Annuity Amount property for Statewithholding UserControl
        Catch
            Throw
            'START : ML | 2019.10.18 | YRS-AT-4597 |Reset Variable
        Finally
            totalAnnuityAmount = Nothing
            totalWithholdAmount = Nothing
            'END : ML | 2019.10.18 | YRS-AT-4597 |Reset Variable
        End Try
    End Sub
    Public Sub LoadGenWithDrawalTab()
        Try
            Dim dr As DataRow
            Dim l_dataset_GenWith As DataSet
            Dim l_string_PersId As String
            l_string_PersId = Me.PersId
            l_dataset_GenWith = Session("GenWithDrawals")

            If Me.blnAddGenWithHoldings = True Or Me.blnUpdateGenWithDrawals And Not Session("blnCancel") = True Then
                If l_dataset_GenWith Is Nothing Then
                    l_dataset_GenWith = New DataSet
                    Dim dt As New DataTable("av_atsgenwithholdings")
                    dt.Columns.Add("Type")
                    dt.Columns.Add("Add'l Amount")
                    dt.Columns.Add("Start Date")
                    dt.Columns.Add("End Date")
                    dt.Columns.Add("PersId")
                    dt.Columns.Add("GenWithDrawalID")
                    l_dataset_GenWith.Tables.Add(dt)
                End If
                If Not (Me.blnUpdateGenWithDrawals = True) Then
                    dr = l_dataset_GenWith.Tables(0).NewRow
                    dr("Type") = Session("cmbWithHoldingType")
                    dr("Add'l Amount") = Session("txtAddAmount")
                    dr("Start Date") = Session("txtStartDate")
                    dr("End Date") = Session("txtEndDate")
                    dr("PersId") = l_string_PersId
                    'Ashish:2010.06.07 Commented 
                    'dr("GenWithDrawalID") = ""
                    l_dataset_GenWith.Tables(0).Rows.Add(dr)
                End If

                'After adding the row to the dataset reset the session variable()
                Me.blnAddGenWithHoldings = False
                Me.blnUpdateGenWithDrawals = False
            Else

                If l_dataset_GenWith Is Nothing Then
                    l_dataset_GenWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpGenWithDrawals(l_string_PersId)
                Else
                    If Not l_dataset_GenWith.Tables(0).GetChanges Is Nothing Then
                        If Not l_dataset_GenWith.Tables(0).GetChanges.Rows.Count > 0 Then
                            l_dataset_GenWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpGenWithDrawals(l_string_PersId)
                        End If
                        If Session("blnCancel") = True Then
                            l_dataset_GenWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpGenWithDrawals(l_string_PersId)
                            Session("blnCancel") = False
                        End If
                    End If
                End If

            End If
            DataGridGeneralWithholding.DataSource = Nothing
            DataGridGeneralWithholding.DataSource = l_dataset_GenWith
            DataGridGeneralWithholding.DataBind()

            Session("GenWithDrawals") = l_dataset_GenWith
            Session("blnCancel") = False
        Catch
            Throw
        End Try
    End Sub
    Public Sub LoadPurchaseTab()
        Try
            Me.ButtonReCalculate_Click(New Object, New EventArgs)
            SetFirstCheckValuesToPurchaseTab()
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

    Private Sub ShowCustomMessage(ByVal pstrMessage As String, ByVal pMessageBoxType As enumMessageBoxType)
        Dim alertWindow As String
        Try
            If pMessageBoxType = enumMessageBoxType.Javascript Then
                alertWindow = "<script language='javascript'>" & _
                            "alert('" & pstrMessage & "');" & _
                            "</script>"
                If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                    Page.RegisterStartupScript("alertWindow", alertWindow)
                End If
            ElseIf pMessageBoxType = enumMessageBoxType.DotNet Then
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", pstrMessage, MessageBoxButtons.OK)
                lblMessage.Text = pstrMessage
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','NO');", True) 'MMR | 2017.03.15 | YRS-AT-2625 | Terminating function with semi-colon to avoid any client-side error
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ShowCustomMessage(ByVal pstrMessage As String, ByVal pMessageBoxType As enumMessageBoxType, ByVal msgBoxButtons As MessageBoxButtons, Optional ByVal pstrMessageTitle As String = "")
        Dim alertWindow As String
        Try
            If pMessageBoxType = enumMessageBoxType.Javascript Then
                alertWindow = "<script language='javascript'>" & _
                            "alert('" & pstrMessage & "');" & _
                            "</script>"
                If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                    Page.RegisterStartupScript("alertWindow", alertWindow)
                End If
            ElseIf pMessageBoxType = enumMessageBoxType.DotNet Then
                If pstrMessageTitle.Trim.ToString() = String.Empty Then
                    pstrMessageTitle = "YMCA-YRS"
                End If
                'MessageBox.Show(PlaceHolder1, pstrMessageTitle, pstrMessage, msgBoxButtons)
                lblMessage.Text = pstrMessage
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "$(document).ready(function () {showDialog('ConfirmDialog','" & lblMessage.Text & "','NO');});", True) 'MMR | 2017.03.15 | YRS-AT-2625 | Terminating function with semi-colon to avoid any client-side error
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ShowCustomMessage(ByVal pstrMessage As String, ByVal pMessageBoxType As enumMessageBoxType, ByVal msgBoxButtons As MessageBoxButtons, ByVal topLeft As Boolean)
        Dim alertWindow As String
        Try
            Dim pstrMessageTitle As String = String.Empty
            If pMessageBoxType = enumMessageBoxType.Javascript Then
                alertWindow = "<script language='javascript'>" & _
                            "alert('" & pstrMessage & "');" & _
                            "</script>"
                If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                    Page.RegisterStartupScript("alertWindow", alertWindow)
                End If
            ElseIf pMessageBoxType = enumMessageBoxType.DotNet Then
                If pstrMessageTitle.Trim.ToString() = String.Empty Then
                    pstrMessageTitle = "YMCA-YRS"
                End If
                'MessageBox.Show(PlaceHolder1, pstrMessageTitle, pstrMessage, msgBoxButtons)
                lblMessage.Text = pstrMessage
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','NO');", True) 'MMR | 2017.03.15 | YRS-AT-2625 | Terminating function with semi-colon to avoid any client-side error
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub CalculateValues(ByVal dt As DataTable, ByVal benecategory As String)
        Try
            'SP 2013.12.19 - BT:2131/YRS 5.0-2161 : Modifications to Annuity Purchase process 
            'Converting all the datatype of varible(s) from Double to Decimal to resolve rounding issue.
            Dim lnPrim As Decimal
            Dim lnCont1 As Decimal
            Dim lnCont2 As Decimal
            Dim lnCont3 As Decimal
            Dim lnPrimb As Decimal
            Dim lnCont1b As Decimal
            Dim lnCont2b As Decimal
            Dim lnCont3b As Decimal
            Dim i As Integer
            Dim lcPrimBnft As String
            Dim lcCont1Bnft As String
            Dim lcCont2Bnft As String
            Dim lcCont3Bnft As String

            Dim lcPrimMsg As String
            Dim lcCont1Msg As String
            Dim lcCont2Msg As String
            Dim lcCont3Msg As String

            Dim lcPrimBMsg As String
            Dim lcCont1BMsg As String
            Dim lcCont2BMsg As String
            Dim lcCont3BMsg As String

            For i = 0 To dt.Rows.Count - 1
                If Not dt.Rows(i).RowState = DataRowState.Deleted Then


                    If benecategory = "A" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "PRIM" Then
                        lnPrim = lnPrim + dt.Rows(i).Item("Pct")
                    End If

                    If benecategory = "A" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" And dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim = "LVL1" Then
                        lnCont1 = lnCont1 + dt.Rows(i).Item("Pct")
                    End If

                    If benecategory = "A" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" And dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim = "LVL2" Then
                        lnCont2 = lnCont2 + dt.Rows(i).Item("Pct")
                    End If

                    If benecategory = "A" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" And dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim = "LVL3" Then
                        lnCont3 = lnCont3 + dt.Rows(i).Item("Pct")
                    End If

                    'Retirement retired db benefit

                    If benecategory = "R" And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "RETIRE" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "PRIM" Then
                        lnPrim = lnPrim + dt.Rows(i).Item("Pct")
                    End If

                    If benecategory = "R" And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "RETIRE" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" And dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim = "LVL1" Then
                        lnCont1 = lnCont1 + dt.Rows(i).Item("Pct")
                    End If

                    If benecategory = "R" And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "RETIRE" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" And dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim = "LVL2" Then
                        lnCont2 = lnCont2 + dt.Rows(i).Item("Pct")
                    End If

                    If benecategory = "R" And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "RETIRE" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" And dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim = "LVL3" Then
                        lnCont3 = lnCont3 + dt.Rows(i).Item("Pct")
                    End If


                    'Retirement Insured reserve benefit

                    If benecategory = "R" And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "INSRES" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "PRIM" Then
                        lnPrimb = lnPrimb + dt.Rows(i).Item("Pct")
                    End If

                    If benecategory = "R" And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "INSRES" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" And dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim = "LVL1" Then
                        lnCont1b = lnCont1b + dt.Rows(i).Item("Pct")
                    End If

                    If benecategory = "R" And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "INSRES" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" And dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim = "LVL2" Then
                        lnCont2b = lnCont2b + dt.Rows(i).Item("Pct")
                    End If

                    If benecategory = "R" And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "INSRES" And dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" And dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim = "LVL3" Then
                        lnCont3b = lnCont3b + dt.Rows(i).Item("Pct")
                    End If

                End If
            Next

            If benecategory = "R" Then
                'Set percentage values (main)
                If lnPrim = 0 Then
                    TextBoxPrimaryR.Text = ""
                    ButtonPriR.Enabled = False
                Else
                    TextBoxPrimaryR.Text = lnPrim
                    ButtonPriR.Enabled = True
                End If

                If lnCont1 = 0 Then
                    TextBoxCont1R.Text = ""
                    ButtonCont1R.Enabled = False
                Else
                    TextBoxCont1R.Text = lnCont1
                    ButtonCont1R.Enabled = True
                End If

                If lnCont2 = 0 Then
                    TextBoxCont2R.Text = ""
                    ButtonCont2R.Enabled = False
                Else
                    TextBoxCont2R.Text = lnCont2
                    ButtonCont2R.Enabled = True
                End If

                If lnCont3 = 0 Then
                    TextBoxCont3R.Text = ""
                    ButtonCont3R.Enabled = False
                Else
                    TextBoxCont3R.Text = lnCont3
                    ButtonCont3R.Enabled = True
                End If
            End If

            If benecategory = "A" Then
                'lcPrimBnft = "Primary member"'commented by Anudeep on 22-sep for BT-1126
                lcPrimBnft = getmessage("MESSAGE_RETIREMENT_PROCESSING_PRIMARY_MEMBER")
            Else
                'lcPrimBnft = "Primary retired death benefit"'commented by Anudeep on 22-sep for BT-1126
                lcPrimBnft = getmessage("MESSAGE_RETIREMENT_PROCESSING_PRIMARY_RETIRED_DEATH_BENIFIT")
            End If

            If benecategory = "A" Then
                'lcCont1Bnft = "Contingent level 1 member"'commented by Anudeep on 22-sep for BT-1126
                lcCont1Bnft = getmessage("MESSAGE_RETIREMENT_PROCESSING_CONTINGENT_LEVEL1_MEMBER")
            Else
                'lcCont1Bnft = "Contingent level 1 retired death benefit"'commented by Anudeep on 22-sep for BT-1126
                lcCont1Bnft = getmessage("MESSAGE_RETIREMENT_PROCESSING_CONTINGENT_LEVEL1_RETIRED")
            End If

            If benecategory = "A" Then
                'lcCont2Bnft = "Contingent level 2 member"'commented by Anudeep on 22-sep for BT-1126
                lcCont2Bnft = getmessage("MESSAGE_RETIREMENT_PROCESSING_CONTINGENT_LEVEL2_MEMBER")
            Else
                'lcCont2Bnft = "Contingent level 2 retired death benefit"'commented by Anudeep on 22-sep for BT-1126
                lcCont2Bnft = getmessage("MESSAGE_RETIREMENT_PROCESSING_CONTINGENT_LEVEL2_RETIRED")
            End If

            If benecategory = "A" Then
                'lcCont3Bnft = "Contingent level 3 member"'commented by Anudeep on 22-sep for BT-1126
                lcCont3Bnft = getmessage("MESSAGE_RETIREMENT_PROCESSING_CONTINGENT_LEVEL3_MEMBER")
            Else
                'lcCont3Bnft = "Contingent level 3 retired death benefit"'commented by Anudeep on 22-sep for BT-1126
                lcCont3Bnft = getmessage("MESSAGE_RETIREMENT_PROCESSING_CONTINGENT_LEVEL3_RETIRED")
            End If

            'lcPrimMsg = lcPrimBnft + " beneficiary percents do not total 100%."'commented by Anudeep on 22-sep for BT-1126
            lcPrimMsg = lcPrimBnft + getmessage("MESSAGE_RETIREMENT_PROCESSING_BENIFICIARY_PERCENTS")
            'lcCont1Msg = lcCont1Bnft + " beneficiary percents do not total 100%."'commented by Anudeep on 22-sep for BT-1126
            lcCont1Msg = lcCont1Bnft + getmessage("MESSAGE_RETIREMENT_PROCESSING_BENIFICIARY_PERCENTS")
            'lcCont2Msg = lcCont2Bnft + " beneficiary percents do not total 100%."
            lcCont2Msg = lcCont2Bnft + getmessage("MESSAGE_RETIREMENT_PROCESSING_BENIFICIARY_PERCENTS")
            'lcCont3Msg = lcCont3Bnft + " beneficiary percents do not total 100%."'commented by Anudeep on 22-sep for BT-1126
            lcCont3Msg = lcCont3Bnft + getmessage("MESSAGE_RETIREMENT_PROCESSING_BENIFICIARY_PERCENTS")


            'lcPrimBMsg = "Primary retired insured reserve beneficiary percents do not total 100%."'commented by Anudeep on 22-sep for BT-1126
            lcPrimBMsg = getmessage("MESSAGE_RETIREMENT_PROCESSING_PRIMARY_RETIRED_INSURED")
            'lcCont1BMsg = "Contingent Level 1 retired insured reserve percents do not total 100%."'commented by Anudeep on 22-sep for BT-1126
            lcCont1BMsg = getmessage("MESSAGE_RETIREMENT_PROCESSING_CONTINGENT_LEVEL1_RETIRED_INSURED")
            'lcCont2BMsg = "Contingent Level 2 retired insured reserve percents do not total 100%."'commented by Anudeep on 22-sep for BT-1126
            lcCont2BMsg = getmessage("MESSAGE_RETIREMENT_PROCESSING_CONTINGENT_LEVEL2_RETIRED_INSURED")
            'lcCont3BMsg = "Contingent Level 3 retired insured reserve percents do not total 100%."'commented by Anudeep on 22-sep for BT-1126
            lcCont3BMsg = getmessage("MESSAGE_RETIREMENT_PROCESSING_CONTINGENT_LEVEL3_RETIRED_INSURED")
            'Set Percentage values main
            Dim l_string_ValidationMessage As String
            If lnPrim <> 0 And lnPrim <> 100 Then
                If l_string_ValidationMessage = "" Then
                    l_string_ValidationMessage = lcPrimMsg
                Else
                    l_string_ValidationMessage = l_string_ValidationMessage + " " + lcPrimMsg
                End If
            End If

            If lnCont1 <> 0 And lnCont1 <> 100 Then
                If l_string_ValidationMessage = "" Then
                    l_string_ValidationMessage = lcCont1Msg
                Else
                    l_string_ValidationMessage = l_string_ValidationMessage + " " + lcCont1Msg
                End If
            End If

            If lnCont2 <> 0 And lnCont2 <> 100 Then
                If l_string_ValidationMessage = "" Then
                    l_string_ValidationMessage = lcCont2Msg
                Else
                    l_string_ValidationMessage = l_string_ValidationMessage + " " + lcCont2Msg
                End If
            End If

            If lnCont3 <> 0 And lnCont3 <> 100 Then
                If l_string_ValidationMessage = "" Then
                    l_string_ValidationMessage = lcCont3Msg
                Else
                    l_string_ValidationMessage = l_string_ValidationMessage + " " + lcCont3Msg
                End If
            End If

            If lnPrimb <> 0 And lnPrimb <> 100 Then
                If l_string_ValidationMessage = "" Then
                    l_string_ValidationMessage = lcPrimBMsg
                Else
                    l_string_ValidationMessage = l_string_ValidationMessage + " " + lcPrimBMsg
                End If
            End If

            If lnCont1b <> 0 And lnCont1b <> 100 Then
                If l_string_ValidationMessage = "" Then
                    l_string_ValidationMessage = lcCont1BMsg
                Else
                    l_string_ValidationMessage = l_string_ValidationMessage + " " + lcCont1BMsg
                End If
            End If

            If lnCont2b <> 0 And lnCont2b <> 100 Then
                If l_string_ValidationMessage = "" Then
                    l_string_ValidationMessage = lcCont2BMsg
                Else
                    l_string_ValidationMessage = l_string_ValidationMessage + " " + lcCont2BMsg
                End If
            End If

            If lnCont3b <> 0 And lnCont3b <> 100 Then
                If l_string_ValidationMessage = "" Then
                    l_string_ValidationMessage = lcCont3BMsg
                Else
                    l_string_ValidationMessage = l_string_ValidationMessage + " " + lcCont3BMsg
                End If
            End If
            If l_string_ValidationMessage <> "" Then
                Me.ValidationMessage = l_string_ValidationMessage
            Else
                Me.ValidationMessage = Nothing
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub Equalize(ByVal ds As DataSet, ByVal lcGroup As String, ByVal lcBeneficiaryCategory As String)
        Try
            Dim i As Integer
            Dim lncnt As Integer
            Dim dt As DataTable

            dt = ds.Tables(0)
            For i = 0 To dt.Rows.Count - 1
                If Not dt.Rows(i).RowState = DataRowState.Deleted Then


                    If lcBeneficiaryCategory = "A" _
                    And lcGroup = "P" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
                        lncnt = lncnt + 1
                    End If

                    If lcBeneficiaryCategory = "A" _
                    And lcGroup = "C1" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
                        lncnt = lncnt + 1
                    End If

                    If lcBeneficiaryCategory = "A" _
                    And lcGroup = "C2" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
                        lncnt = lncnt + 1
                    End If

                    If lcBeneficiaryCategory = "A" _
                    And lcGroup = "C3" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
                        lncnt = lncnt + 1
                    End If

                    'Retirement Retired db Benefit
                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "P" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then

                        lncnt = lncnt + 1
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C1" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
                        lncnt = lncnt + 1
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C2" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
                        lncnt = lncnt + 1
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C3" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
                        lncnt = lncnt + 1
                    End If

                    'Retirement insured reserve benefit

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "PB" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
                        lncnt = lncnt + 1
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C1B" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
                        lncnt = lncnt + 1
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C2B" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
                        lncnt = lncnt + 1
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C3B" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
                        lncnt = lncnt + 1
                    End If
                End If
            Next
            Dim lnVal1Portion As Double
            Dim lnValPortionTimesCountMinus1 As Double
            Dim lnVal1 As Double
            Dim lnVal2 As Double
            Dim lnDiffWith100 As Double
            If lncnt <> 0 Then


                lnVal1Portion = Convert.ToInt32((100.0 / lncnt) * 10000) / 10000
                lnValPortionTimesCountMinus1 = Convert.ToInt32(lnVal1Portion * (lncnt - 1) * 100000) / 100000

                lnDiffWith100 = 100.0 - lnValPortionTimesCountMinus1

                lnVal1 = lnVal1Portion
                lnVal2 = lnDiffWith100
            End If

            Dim lnFoundCnt As Integer

            lnFoundCnt = 0
            For i = 0 To dt.Rows.Count - 1
                If Not dt.Rows(i).RowState = DataRowState.Deleted Then



                    If lcBeneficiaryCategory = "A" _
                    And lcGroup = "P" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If


                    If lcBeneficiaryCategory = "A" _
                    And lcGroup = "C1" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    If lcBeneficiaryCategory = "A" _
                    And lcGroup = "C2" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    If lcBeneficiaryCategory = "A" _
                    And lcGroup = "C3" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    'Retirement Retired db Benefit

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "P" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C1" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C2" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C3" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    'Retirement insured reserve benefit

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "PB" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C1B" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C2B" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                    If lcBeneficiaryCategory = "R" _
                    And lcGroup = "C3B" _
                    And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
                    And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
                    And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
                        lnFoundCnt = lnFoundCnt + 1
                        dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
                    End If

                End If

            Next
            If lcBeneficiaryCategory = "A" Then
                Me.Flag = "EditBeneficiaries"
                Me.BeneficiariesActive = ds
                Me.Blank = False
                Me.DataGridActiveBeneficiaries.DataSource = ds
                Me.DataGridActiveBeneficiaries.DataBind()
            Else
                Me.Flag = "EditBeneficiaries"
                Me.BeneficiariesRetired = ds
                Me.Blank = False
                Me.DataGridRetiredBeneficiaries.DataSource = ds
                Me.DataGridRetiredBeneficiaries.DataBind()
                Me.CalculateValues(ds.Tables(0), "R")
            End If

        Catch
            Throw
        End Try
    End Sub
    Private Function ValidateFederalWithHoldingDetails() As Boolean
        Dim l_string_alertWindow As String
        Dim dsFedWithDrawals As DataSet
        Dim l_bool_Valid As Boolean = True
        Try
            dsFedWithDrawals = Me.FedWithDrawals

            If Not dsFedWithDrawals Is Nothing Then
                If dsFedWithDrawals.Tables.Count > 0 Then
                    If dsFedWithDrawals.Tables(0).Rows.Count = 0 Then
                        l_bool_Valid = False
                    End If
                Else
                    l_bool_Valid = False
                End If
            Else
                l_bool_Valid = False
            End If

            ValidateFederalWithHoldingDetails = l_bool_Valid

            If l_bool_Valid = False Then
                'commented by Anudeep on 22-sep for BT-1126
                'l_string_alertWindow = "<script language='javascript'>" & _
                '                                    "alert('Federal tax withholding information is required to purchase this annuity');" & _
                '                                    "</script>"
                l_string_alertWindow = "<script language='javascript'>" & _
                                                    "alert('" + getmessage("MESSAGE_RETIREMENT_PROCESSING_FEDARALTAXWITHOLDING_REQUIRED") + "');" & _
                                                    "</script>"
                If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                    Page.RegisterStartupScript("alertWindow", l_string_alertWindow)
                End If
            End If
        Catch ex As Exception
            ValidateFederalWithHoldingDetails = False
            Throw
        End Try
    End Function
    Private Function ValidateBenefeciaryDetails() As Boolean
        Dim l_dataset_Active As New DataSet
        Dim l_dataset_Retired As New DataSet
        Dim l_bool_Valid As Boolean = True
        Try
            l_dataset_Active = Me.BeneficiariesActive

            If Me.ValidationMessage <> "" Then
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", Me.ValidationMessage, MessageBoxButtons.Stop)
                'ShowCustomMessage(Me.ValidationMessage, enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(Me.ValidationMessage, EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                TabStripRetireesInformation.SelectedIndex = 3
                MultiPageRetirementProcessing.SelectedIndex = 3
                l_bool_Valid = False
            Else
                l_dataset_Retired = Me.BeneficiariesRetired
                If Not l_dataset_Active Is Nothing Then
                    CalculateValues(l_dataset_Retired.Tables(0), "R")
                End If
                If Me.ValidationMessage <> "" Then
                    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", Me.ValidationMessage, MessageBoxButtons.Stop)
                    'ShowCustomMessage(Me.ValidationMessage, enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                    HelperFunctions.ShowMessageToUser(Me.ValidationMessage, EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                    TabStripRetireesInformation.SelectedIndex = 3
                    MultiPageRetirementProcessing.SelectedIndex = 3
                    l_bool_Valid = False
                End If
            End If

            ValidateBenefeciaryDetails = l_bool_Valid
        Catch ex As Exception
            ValidateBenefeciaryDetails = False
            Throw
        End Try
    End Function
    Private Function yrsAnnuityPurchaseActions() As Boolean
        Dim i As Integer
        Dim alertWindow As String
        Dim dsSSMetaConfigDetails As New DataSet
        Dim lnSSLevelingAgeinMonths As Integer
        Dim ldSSReductionDate As String
        Dim yrsSSDate As String = String.Empty
        Dim yrsSSDateSav As String = String.Empty
        Dim amtDeathBenefitUsed As Decimal

        Dim drSelectedAnnuityRow As DataRow
        Dim drSelectedAnnuityRows As DataRow()

        Dim drSelectedAnnuityRowSav As DataRow
        Dim drSelectedAnnuityRowsSav As DataRow()

        Dim drSelectedAnnuityRowDB As DataRow

        Dim dtAnnuityJointSurvivors As New DataTable
        Dim drAnnuityJointSurvivors As DataRow
        'Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        Dim dtAnnuityJointSurvivorsAddress As New DataTable
        Dim drAnnuityJointSurvivorsAddress As DataRow()
        Dim drAddressRow As DataRow

        Dim drmnyPopupMaximumValues As DataRow()

        Dim l_bool_Return As Boolean

        Dim lcAnnuitiesListCursorSav As DataTable
        Dim dsPersonal As DataSet
        Try

            If ValidateBenefeciaryDetails() = False Then
                yrsAnnuityPurchaseActions = False
                Exit Function
            End If

            If ValidateFederalWithHoldingDetails() = False Then
                yrsAnnuityPurchaseActions = False
                Exit Function
            End If

            If Not TextBoxAmount.Text = "" Then
                amtDeathBenefitUsed = Convert.ToDecimal(Me.TextBoxAmount.Text)
            Else
                amtDeathBenefitUsed = 0
            End If

            ''''Create Table for Annuity Joint Survivors            
            dtAnnuityJointSurvivors.Columns.Add("guiUniqueID", System.Type.GetType("System.String"))
            dtAnnuityJointSurvivors.Columns.Add("chrSSNo", System.Type.GetType("System.String"))
            dtAnnuityJointSurvivors.Columns.Add("chvLastName", System.Type.GetType("System.String"))
            dtAnnuityJointSurvivors.Columns.Add("chvFirstName", System.Type.GetType("System.String"))
            dtAnnuityJointSurvivors.Columns.Add("chvMiddleName", System.Type.GetType("System.String"))
            dtAnnuityJointSurvivors.Columns.Add("dtmBirthDate", System.Type.GetType("System.DateTime"))
            dtAnnuityJointSurvivors.Columns.Add("dtmDeathDate", System.Type.GetType("System.DateTime"))
            dtAnnuityJointSurvivors.Columns.Add("bitSpouse", System.Type.GetType("System.Boolean"))
            dtAnnuityJointSurvivors.Columns.Add("planType", System.Type.GetType("System.String"))

            '' For retirement Plan
            If CheckBoxRetPlan.Checked Then
                lcAnnuitiesListCursor = Session("dtAnnuityList")
                If Not lcAnnuitiesListCursor.Columns.Contains("planType") Then
                    lcAnnuitiesListCursor.Columns.Add("planType", System.Type.GetType("System.String"))
                End If

                If amtDeathBenefitUsed > 0 Then
                    lcAnnuitiesFullBalanceList = Session("dtAnnuitiesFullBalanceList")
                    If Not lcAnnuitiesFullBalanceList.Columns.Contains("planType") Then
                        lcAnnuitiesFullBalanceList.Columns.Add("planType", System.Type.GetType("System.String"))
                    End If
                End If

                If lcAnnuitiesListCursor.Rows.Count > 0 Then

                    drSelectedAnnuityRows = lcAnnuitiesListCursor.Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text & "'")
                    If drSelectedAnnuityRows.Length > 0 Then

                        drSelectedAnnuityRow = drSelectedAnnuityRows(0)
                        'drSelectedAnnuityRow = lcAnnuitiesListCursor.Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text & "'").GetValue(0)

                        drSelectedAnnuityRow("planType") = "R"

                        'since popup maximum value is nothing but the M annuity value
                        drmnyPopupMaximumValues = lcAnnuitiesListCursor.Select("chrAnnuityType='M'")
                        If drmnyPopupMaximumValues.Length > 0 Then
                            drSelectedAnnuityRow("mnySurvivorRetiree") = drmnyPopupMaximumValues(0)("mnyCurrentPayment")
                        Else
                            drSelectedAnnuityRow("mnySurvivorRetiree") = Nothing
                        End If

                        'Get Death Benefeciary Row only if it has been used.
                        If amtDeathBenefitUsed > 0 Then
                            If Not lcAnnuitiesFullBalanceList Is Nothing Then
                                If lcAnnuitiesFullBalanceList.Rows.Count > 0 Then
                                    drSelectedAnnuityRowDB = lcAnnuitiesFullBalanceList.Select("chrAnnuityType='" & drSelectedAnnuityRow("chrDBAnnuityType").Trim() & "'").GetValue(0)
                                End If
                            End If

                            If Not drSelectedAnnuityRowDB Is Nothing Then
                                drSelectedAnnuityRowDB("planType") = "DB"
                            End If
                        End If


                        'Get Benefeciary Details
                        If drSelectedAnnuityRow("bitJointSurvivor") = True Then
                            drAnnuityJointSurvivors = dtAnnuityJointSurvivors.NewRow
                            drAnnuityJointSurvivors("guiUniqueID") = ""
                            drAnnuityJointSurvivors("chrSSNo") = TextBoxAnnuitySSNoRet.Text.Trim()
                            drAnnuityJointSurvivors("chvLastName") = TextBoxAnnuityLastNameRet.Text.Trim()
                            drAnnuityJointSurvivors("chvFirstName") = TextBoxAnnuityFirstNameRet.Text.Trim()
                            drAnnuityJointSurvivors("chvMiddleName") = TextBoxAnnuityMiddleNameRet.Text.Trim()
                            If TextBoxAnnuityBirthDateRet.Text.Trim() <> "" Then
                                drAnnuityJointSurvivors("dtmBirthDate") = Convert.ToDateTime(TextBoxAnnuityBirthDateRet.Text.Trim())
                            Else
                                drAnnuityJointSurvivors("dtmBirthDate") = ""
                            End If

                            '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
                            'Commented code for issue 1508
                            'If chkSpouseRet.Checked = True Then
                            '    drAnnuityJointSurvivors("bitSpouse") = "True"
                            'Else
                            '    drAnnuityJointSurvivors("bitSpouse") = "False"
                            'End If
                            If (DropDownRelationShipRet.SelectedValue.ToLower() = "sp") Then
                                drAnnuityJointSurvivors("bitSpouse") = "True"
                            Else
                                drAnnuityJointSurvivors("bitSpouse") = "False"
                            End If
                            '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End

                            drAnnuityJointSurvivors("planType") = "R"
                            dtAnnuityJointSurvivors.Rows.Add(drAnnuityJointSurvivors)
                            'Start:Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                            'Anudeep:29.10.2013 YRS 5.0-1745:Added validation to check whether the address exists or not
                            If Not BeneficiaryAddress Is Nothing And AddressWebUserControlRet.Address1 <> "" And AddressWebUserControlRet.City <> "" And AddressWebUserControlRet.DropDownListCountryValue <> "" Then
                                dtAnnuityJointSurvivorsAddress = BeneficiaryAddress
                                drAnnuityJointSurvivorsAddress = dtAnnuityJointSurvivorsAddress.Select("BenSSNo='" + TextBoxAnnuitySSNoRet.Text + "'")
                                If drAnnuityJointSurvivorsAddress.Length = 0 Then
                                    drAddressRow = dtAnnuityJointSurvivorsAddress.NewRow()

                                    drAddressRow("addr1") = AddressWebUserControlRet.Address1.Replace(",", "").Trim()
                                    drAddressRow("addr2") = AddressWebUserControlRet.Address2.Replace(",", "").Trim()
                                    drAddressRow("addr3") = AddressWebUserControlRet.Address3.Replace(",", "").Trim()
                                    drAddressRow("city") = AddressWebUserControlRet.City.Replace(",", "").Trim()
                                    drAddressRow("state") = AddressWebUserControlRet.DropDownListStateValue.Replace(",", "").Trim()
                                    drAddressRow("zipCode") = AddressWebUserControlRet.ZipCode.Replace(",", "").Replace("-", "").Trim()
                                    drAddressRow("country") = AddressWebUserControlRet.DropDownListCountryValue.Replace(",", "").Trim()
                                    drAddressRow("isActive") = True
                                    drAddressRow("isPrimary") = True
                                    If AddressWebUserControlRet.EffectiveDate <> String.Empty Then
                                        drAddressRow("effectiveDate") = AddressWebUserControlRet.EffectiveDate
                                    Else
                                        drAddressRow("effectiveDate") = System.DateTime.Now()
                                    End If
                                    drAddressRow("isBadAddress") = AddressWebUserControlRet.IsBadAddress
                                    drAddressRow("addrCode") = "HOME"
                                    drAddressRow("entityCode") = EnumEntityCode.PERSON.ToString()
                                    'Anudeep:2013.08.22-YRS 5.0-1862:Add notes record when user enters address in any module.
                                    If AddressWebUserControlRet.Notes <> "" Then
                                        drAddressRow("Note") = "Beneficiary " + TextBoxAnnuityFirstNameRet.Text.Trim + " " + TextBoxAnnuityLastNameRet.Text.Trim + " " + AddressWebUserControlRet.Notes ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                                    Else
                                        drAddressRow("Note") = "Beneficiary " + TextBoxAnnuityFirstNameRet.Text.Trim + " " + TextBoxAnnuityLastNameRet.Text.Trim + " " + "Address has been updated" ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                                    End If
                                    drAddressRow("bitImportant") = AddressWebUserControlRet.BitImp
                                    drAddressRow("BenSSNo") = TextBoxAnnuitySSNoRet.Text
                                    drAddressRow("PersID") = Session("PersId").ToString()
                                    dtAnnuityJointSurvivorsAddress.Rows.Add(drAddressRow)
                                    BeneficiaryAddress = dtAnnuityJointSurvivorsAddress
                                End If
                            End If
                        End If
                        'End:Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        'Get SS details
                        If drSelectedAnnuityRow("bitSSLeveling") = True Then
                            TextBoxAnnuitySelectRet.Text = TextBoxAnnuitySelectRet.Text & "S"
                            drSelectedAnnuityRow.AcceptChanges()
                            dsSSMetaConfigDetails = BORetireeEstimate.getSSMetaConfigDetails()
                            If Not dsSSMetaConfigDetails.Tables.Count = 0 Then
                                If Not dsSSMetaConfigDetails.Tables(0).Rows.Count = 0 Then
                                    For i = 0 To dsSSMetaConfigDetails.Tables(0).Rows.Count - 1
                                        If Not dsSSMetaConfigDetails.Tables(0).Rows(i).Item("chvKey") Is DBNull.Value Then
                                            If dsSSMetaConfigDetails.Tables(0).Rows(i).Item("chvKey") = "SS_REDUCTION_AGE_IN_MONTHS" Then
                                                lnSSLevelingAgeinMonths = Convert.ToDecimal(dsSSMetaConfigDetails.Tables(0).Rows(i).Item("chvValue"))
                                            End If
                                        Else
                                            lnSSLevelingAgeinMonths = 746
                                        End If
                                    Next
                                End If
                            End If

                            Dim l_datetime_BirthDate As DateTime
                            Dim l_datetime_ReductionDate As DateTime
                            l_datetime_BirthDate = TextBoxBirthDateRet.Text.Trim()
                            l_datetime_ReductionDate = DateAdd(DateInterval.Year, Math.Round((lnSSLevelingAgeinMonths + 1) / 12, 0), Convert.ToDateTime(TextBoxBirthDateRet.Text))
                            If Day(l_datetime_BirthDate) = 1 Then
                                ldSSReductionDate = DateAdd(DateInterval.Month, 2, l_datetime_ReductionDate)
                            Else
                                ldSSReductionDate = DateAdd(DateInterval.Month, 3, l_datetime_ReductionDate)
                            End If

                            ldSSReductionDate = Month(ldSSReductionDate) & "/01/" & Year(ldSSReductionDate)

                            yrsSSDate = ldSSReductionDate

                        End If ' End Get SS details
                    End If
                    '''''*********************************************************************
                Else
                    'commented by Anudeep on 22-sep for BT-1126
                    'alertWindow = "<script language='javascript'>" & _
                    '            "alert('Problem occured in Annuity calculation');" & _
                    '            "</script>"
                    alertWindow = "<script language='javascript'>" & _
                                "alert('" + getmessage("MESSAGE_RETIREMENT_PROCESSING_ANNUITY_CALUCULATION_PROBLEM") + "');" & _
                                "</script>"
                    If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                        Page.RegisterStartupScript("alertWindow", alertWindow)
                    End If
                    yrsAnnuityPurchaseActions = False
                    Me.blnPurchaseSuccessful = False
                    Exit Function
                End If
            End If

            '' For Savings Plan
            If CheckBoxSavPlan.Checked Then
                lcAnnuitiesListCursorSav = Session("dtAnnuityListSav")
                If Not lcAnnuitiesListCursorSav.Columns.Contains("planType") Then
                    lcAnnuitiesListCursorSav.Columns.Add("planType", System.Type.GetType("System.String"))
                End If

                If lcAnnuitiesListCursorSav.Rows.Count > 0 Then
                    drSelectedAnnuityRowsSav = lcAnnuitiesListCursorSav.Select("chrAnnuityType='" & TextBoxAnnuitySelectSav.Text & "'")
                    If drSelectedAnnuityRowsSav.Length > 0 Then
                        drSelectedAnnuityRowSav = drSelectedAnnuityRowsSav(0)
                        'drSelectedAnnuityRowSav = lcAnnuitiesListCursorSav.Select("chrAnnuityType='" & TextBoxAnnuitySelectSav.Text & "'").GetValue(0)

                        drSelectedAnnuityRowSav("planType") = "S"

                        'Get Popup values. Since popup maximum value is nothing but the M annuity value
                        drmnyPopupMaximumValues = lcAnnuitiesListCursorSav.Select("chrAnnuityType='M'")
                        If drmnyPopupMaximumValues.Length > 0 Then
                            drSelectedAnnuityRowSav("mnySurvivorRetiree") = drmnyPopupMaximumValues(0)("mnyCurrentPayment")
                        Else
                            drSelectedAnnuityRowSav("mnySurvivorRetiree") = Nothing
                        End If

                        'Get joint annuity survivor details
                        If drSelectedAnnuityRowSav("bitJointSurvivor") = True And IsPrePlanSplitRetirement = False Then
                            drAnnuityJointSurvivors = dtAnnuityJointSurvivors.NewRow
                            drAnnuityJointSurvivors("guiUniqueID") = ""

                            '2012.06.04  SP:  BT-975/YRS 5.0-1508 - 

                            drAnnuityJointSurvivors("chrSSNo") = TextBoxAnnuitySSNoSav.Text.Trim()
                            drAnnuityJointSurvivors("chvLastName") = TextBoxAnnuityLastNameSav.Text.Trim()
                            drAnnuityJointSurvivors("chvFirstName") = TextBoxAnnuityFirstNameSav.Text.Trim()
                            drAnnuityJointSurvivors("chvMiddleName") = TextBoxAnnuityMiddleNameSav.Text.Trim()
                            If TextBoxAnnuityBirthDateSav.Text.Trim() <> "" Then
                                drAnnuityJointSurvivors("dtmBirthDate") = Convert.ToDateTime(TextBoxAnnuityBirthDateSav.Text.Trim())
                            Else
                                drAnnuityJointSurvivors("dtmBirthDate") = ""
                            End If

                            'If chkSpouseSav.Checked = True Then
                            '	drAnnuityJointSurvivors("bitSpouse") = "True"
                            'Else
                            '	drAnnuityJointSurvivors("bitSpouse") = "False"
                            'End If

                            If (DropDownRelationShipSav.SelectedValue.ToLower() = "sp") Then
                                drAnnuityJointSurvivors("bitSpouse") = "True"
                            Else
                                drAnnuityJointSurvivors("bitSpouse") = "False"
                            End If


                            '2012.06.04  SP:  BT-975/YRS 5.0-1508

                            drAnnuityJointSurvivors("planType") = "S"
                            dtAnnuityJointSurvivors.Rows.Add(drAnnuityJointSurvivors)
                            'Start:Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                            'Anudeep:29.10.2013 YRS 5.0-1745:Added validation to check whether the address exists or not
                            If Not BeneficiaryAddress Is Nothing And AddressWebUserControlSav.Address1 <> "" And AddressWebUserControlSav.City <> "" And AddressWebUserControlSav.DropDownListCountryValue <> "" Then
                                dtAnnuityJointSurvivorsAddress = BeneficiaryAddress
                                drAnnuityJointSurvivorsAddress = dtAnnuityJointSurvivorsAddress.Select("BenSSNo='" + TextBoxAnnuitySSNoRet.Text + "'")
                                If drAnnuityJointSurvivorsAddress.Length = 0 Then
                                    drAddressRow = dtAnnuityJointSurvivorsAddress.NewRow()

                                    drAddressRow("addr1") = AddressWebUserControlSav.Address1.Replace(",", "").Trim()
                                    drAddressRow("addr2") = AddressWebUserControlSav.Address2.Replace(",", "").Trim()
                                    drAddressRow("addr3") = AddressWebUserControlSav.Address3.Replace(",", "").Trim()
                                    drAddressRow("city") = AddressWebUserControlSav.City.Replace(",", "").Trim()
                                    drAddressRow("state") = AddressWebUserControlSav.DropDownListStateValue.Replace(",", "").Trim()
                                    drAddressRow("zipCode") = AddressWebUserControlSav.ZipCode.Replace(",", "").Replace("-", "").Trim()
                                    drAddressRow("country") = AddressWebUserControlSav.DropDownListCountryValue.Replace(",", "").Trim()
                                    drAddressRow("isActive") = True
                                    drAddressRow("isPrimary") = True
                                    If AddressWebUserControlSav.EffectiveDate <> String.Empty Then
                                        drAddressRow("effectiveDate") = AddressWebUserControlSav.EffectiveDate
                                    Else
                                        drAddressRow("effectiveDate") = System.DateTime.Now()
                                    End If
                                    drAddressRow("isBadAddress") = AddressWebUserControlSav.IsBadAddress
                                    drAddressRow("addrCode") = "HOME"
                                    drAddressRow("entityCode") = EnumEntityCode.PERSON.ToString()
                                    'Anudeep:2013.08.22-YRS 5.0-1862:Add notes record when user enters address in any module.
                                    If AddressWebUserControlSav.Notes <> "" Then
                                        drAddressRow("Note") = "Beneficiary " + TextBoxAnnuityFirstNameSav.Text.Trim + " " + TextBoxAnnuityLastNameSav.Text.Trim + " " + AddressWebUserControlSav.Notes ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                                    Else
                                        drAddressRow("Note") = "Beneficiary " + TextBoxAnnuityFirstNameSav.Text.Trim + " " + TextBoxAnnuityLastNameSav.Text.Trim + " " + "Address has been updated" ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                                    End If
                                    drAddressRow("bitImportant") = AddressWebUserControlSav.BitImp
                                    drAddressRow("BenSSNo") = TextBoxAnnuitySSNoRet.Text
                                    drAddressRow("PersID") = Session("PersId").ToString()
                                    dtAnnuityJointSurvivorsAddress.Rows.Add(drAddressRow)
                                    BeneficiaryAddress = dtAnnuityJointSurvivorsAddress
                                End If
                            End If
                        End If
                        'End:Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        'Get SS Details
                        If drSelectedAnnuityRowSav("bitSSLeveling") = True Then
                            TextBoxAnnuitySelectSav.Text = TextBoxAnnuitySelectSav.Text & "S"
                            dsSSMetaConfigDetails = BORetireeEstimate.getSSMetaConfigDetails()
                            If dsSSMetaConfigDetails.Tables.Count <> 0 And dsSSMetaConfigDetails.Tables(0).Rows.Count <> 0 Then
                                For i = 0 To dsSSMetaConfigDetails.Tables(0).Rows.Count - 1
                                    If Not dsSSMetaConfigDetails.Tables(0).Rows(i).Item("chvKey") Is DBNull.Value Then
                                        If dsSSMetaConfigDetails.Tables(0).Rows(i).Item("chvKey") = "SS_REDUCTION_AGE_IN_MONTHS" Then
                                            lnSSLevelingAgeinMonths = Convert.ToDecimal(dsSSMetaConfigDetails.Tables(0).Rows(i).Item("chvValue"))
                                        End If
                                    Else
                                        lnSSLevelingAgeinMonths = 746
                                    End If
                                Next
                            End If

                            Dim l_datetime_BirthDate As DateTime = TextBoxBirthDateRet.Text.Trim()
                            Dim l_datetime_ReductionDate As DateTime
                            l_datetime_ReductionDate = DateAdd(DateInterval.Year, Math.Round((lnSSLevelingAgeinMonths + 1) / 12, 0), Convert.ToDateTime(TextBoxBirthDateRet.Text))
                            If Day(l_datetime_BirthDate) = 1 Then
                                ldSSReductionDate = DateAdd(DateInterval.Month, 2, l_datetime_ReductionDate)
                            Else
                                ldSSReductionDate = DateAdd(DateInterval.Month, 3, l_datetime_ReductionDate)
                            End If

                            ldSSReductionDate = Month(ldSSReductionDate) & "/01/" & Year(ldSSReductionDate)
                            yrsSSDateSav = ldSSReductionDate
                        End If ' End Get SS Details
                    End If

                    '''''*********************************************************************
                Else
                    'commented by Anudeep on 22-sep for BT-1126
                    'alertWindow = "<script language='javascript'>" & _
                    '   "alert('Problem occured in Annuity calculation');" & _
                    '   "</script>"
                    alertWindow = "<script language='javascript'>" & _
                      "alert('" + getmessage("MESSAGE_RETIREMENT_PROCESSING_ANNUITY_CALUCULATION_PROBLEM") + "');" & _
                      "</script>"
                    If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                        Page.RegisterStartupScript("alertWindow", alertWindow)
                    End If
                    yrsAnnuityPurchaseActions = False
                    Me.blnPurchaseSuccessful = False
                    Exit Function
                End If
            End If

            ' Pre 2008 Processing 
            ' 1. Club the Savings plan value into Retirement Plan and Delete Savings Plan values
            If IsPrePlanSplitRetirement Then
                If CheckBoxRetPlan.Checked Then
                    If Not drSelectedAnnuityRowSav Is Nothing Then
                        drSelectedAnnuityRow("mnyCurrentPayment") = drSelectedAnnuityRow("mnyCurrentPayment") + drSelectedAnnuityRowSav("mnyCurrentPayment")
                        drSelectedAnnuityRow("mnyPersonalPreTaxCurrentPayment") = drSelectedAnnuityRow("mnyPersonalPreTaxCurrentPayment") + drSelectedAnnuityRowSav("mnyPersonalPreTaxCurrentPayment")
                        drSelectedAnnuityRow("mnyPersonalPostTaxCurrentPayment") = drSelectedAnnuityRow("mnyPersonalPostTaxCurrentPayment") + drSelectedAnnuityRowSav("mnyPersonalPostTaxCurrentPayment")
                        drSelectedAnnuityRow("mnyYmcaPreTaxCurrentPayment") = drSelectedAnnuityRow("mnyYmcaPreTaxCurrentPayment") + drSelectedAnnuityRowSav("mnyYmcaPreTaxCurrentPayment")
                        drSelectedAnnuityRow("mnyYmcaPostTaxCurrentPayment") = drSelectedAnnuityRow("mnyYmcaPostTaxCurrentPayment") + drSelectedAnnuityRowSav("mnyYmcaPostTaxCurrentPayment")
                        drSelectedAnnuityRow("mnySSIncrease") = drSelectedAnnuityRow("mnySSIncrease") + drSelectedAnnuityRowSav("mnySSIncrease")
                        drSelectedAnnuityRow("mnySSDecrease") = drSelectedAnnuityRow("mnySSDecrease") + drSelectedAnnuityRowSav("mnySSDecrease")
                        drSelectedAnnuityRow("mnySurvivorRetiree") = drSelectedAnnuityRow("mnySurvivorRetiree") + drSelectedAnnuityRowSav("mnySurvivorRetiree")
                        drSelectedAnnuityRow.AcceptChanges()
                        drSelectedAnnuityRowSav = Nothing
                    End If
                Else
                    'Added by Anudeep on 22-sep for BT-1126
                    'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_SPLIT_RETIREMENT"), enumMessageBoxType.DotNet)
                    HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_SPLIT_RETIREMENT"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                    Exit Function
                End If
            End If

            '' Call the method to retire the person.            
            'Create Selected Annuity table
            Dim dtAnnuity As DataTable
            If Not lcAnnuitiesListCursor Is Nothing Then
                dtAnnuity = lcAnnuitiesListCursor.Clone()
            Else
                dtAnnuity = lcAnnuitiesListCursorSav.Clone()
            End If

            If Not drSelectedAnnuityRow Is Nothing Then
                dtAnnuity.ImportRow(drSelectedAnnuityRow)
            End If
            If Not drSelectedAnnuityRowDB Is Nothing Then
                dtAnnuity.ImportRow(drSelectedAnnuityRowDB)
            End If
            If Not drSelectedAnnuityRowSav Is Nothing Then
                dtAnnuity.ImportRow(drSelectedAnnuityRowSav)
            End If
            dtAnnuity.AcceptChanges()

            dsPersonal = Me.PersonDetails

            l_bool_Return = yrsRetire(dtAnnuity, amtDeathBenefitUsed, yrsSSDate, yrsSSDateSav, dtAnnuityJointSurvivors)

            If l_bool_Return = True Then
                'commented by Anudeep on 22-sep for BT-1126
                'alertWindow = "<script language='javascript'>" & _
                ' "alert('PURCHASE SUCCESSFUL');" & _
                ' "</script>"
                'SP 2013.12.13 : BT-2326
                'alertWindow = "<script language='javascript'>" & _
                ' "alert('" + getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_SUCCESFULL") + "');" & _
                ' "</script>"
                'If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                '    Page.RegisterStartupScript("alertWindow", alertWindow)
                'End If

                Me.blnPurchaseSuccessful = True
                yrsAnnuityPurchaseActions = True

                If HelperFunctions.isNonEmpty(dsPersonal) Then
                    Session("RetirementProcessPersonDetails") = dsPersonal.Tables(0).Rows(0).Item("intFundIdNo").ToString().Trim() + "|" _
                      + dsPersonal.Tables(0).Rows(0).Item("chvLastName").ToString().Trim() _
                      + ", " + dsPersonal.Tables(0).Rows(0).Item("chvFirstName").ToString().Trim()
                    Response.Redirect("MainWebForm.aspx", False)
                Else
                    HelperFunctions.LogException("yrsAnnuityPurchaseActions-dsPersonal is null-RetiremekntProcessing", Nothing)
                    HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_FAILED"), EnumMessageTypes.Error)
                End If
            Else
                Me.blnPurchaseSuccessful = False
                yrsAnnuityPurchaseActions = False
            End If

            ''''---------------------------------------------------------------------
        Catch
            'commented by Anudeep on 22-sep for BT-1126
            ''alertWindow = "<script language='javascript'>" & _
            ''    "alert('PURCHASE FAILED');" & _
            ''    "</script>"
            alertWindow = "<script language='javascript'>" & _
             "alert('" + getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_FAILED") + "');" & _
               "</script>"
            If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                Page.RegisterStartupScript("alertWindow", alertWindow)
            End If
            yrsAnnuityPurchaseActions = False
            Me.blnPurchaseSuccessful = False

            Throw
        End Try
    End Function
    Private Function yrsRetire(ByVal dtSelectedAnnuity As DataTable, ByVal yrsDeathBenefitUsed As Decimal, ByVal yrsSSDate As String, ByVal yrsSSDateSav As String, ByVal pdtAnnuityJointSurvivors As DataTable) As Boolean
        Dim l_dataset_genWithDrawal As DataSet
        Dim l_dataset_FedWithDrawal As DataSet
        Dim l_dataset_RetiredBen As DataSet
        Dim l_dataset_AnnuityJointSurvivors As New DataSet
        Dim l_dataset_BeneficiaryAddress As New DataSet
        Dim l_datable_BeneficiaryAddress As DataTable
        Dim l_datatable_Notes As DataTable
        Dim l_dataset_Notes As New DataSet
        Dim drRow As DataRow

        Dim blnChanged As Boolean
        Dim l_bool_Return As Boolean

        Dim strRetireType As String
        Try
            'Sanket Vaidya          17 Feb 2011     For BT 756 : For disability requirement
            'strRetireType = Me.CustomRetireType
            strRetireType = Me.RetireType

            'Get Withdrawal details
            l_dataset_FedWithDrawal = Me.FedWithDrawals
            l_dataset_genWithDrawal = Me.GenWithDrawals

            l_dataset_AnnuityJointSurvivors.Tables.Add(pdtAnnuityJointSurvivors)
            l_dataset_AnnuityJointSurvivors.Tables(0).TableName = "AnnuityJointSurvivors"

            'Get Notes details
            If Not Me.Notes Is Nothing Then
                l_datatable_Notes = Me.Notes.Clone
                If Not Me.Notes.GetChanges(DataRowState.Added) Is Nothing Then
                    If Me.Notes.GetChanges(DataRowState.Added).Rows.Count > 0 Then
                        For Each drRow In Me.Notes.GetChanges(DataRowState.Added).Rows
                            l_datatable_Notes.ImportRow(drRow)
                        Next
                    End If
                End If

                l_dataset_Notes.Tables.Add(l_datatable_Notes)
                l_dataset_Notes.Tables(0).TableName = "Member Notes"
            End If

            'Get  Retired Death Benefeciary Details
            l_dataset_RetiredBen = Me.BeneficiariesRetired
            If Not l_dataset_RetiredBen Is Nothing Then
                If l_dataset_RetiredBen.Tables.Count > 0 Then
                    If l_dataset_RetiredBen.Tables(0).Rows.Count > 0 Then
                        l_dataset_RetiredBen.Tables(0).TableName = "RetiredBeneficiaries"
                    End If
                End If
            End If
            'Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            l_datable_BeneficiaryAddress = BeneficiaryAddress
            If HelperFunctions.isNonEmpty(l_datable_BeneficiaryAddress) Then
                l_dataset_BeneficiaryAddress.Tables.Add(l_datable_BeneficiaryAddress.Copy())
                l_dataset_BeneficiaryAddress.Tables(0).TableName = "Address"
            End If


            Dim decTaxable As Decimal
            Dim decNonTaxable As Decimal
            Dim decRetiredBenefit As Decimal
            If CheckBoxRetPlan.Checked Then
                decTaxable = Convert.ToDecimal(TextBoxTaxableRet.Text)
                decNonTaxable = Convert.ToDecimal(TextBoxNonTaxableRet.Text)
                decRetiredBenefit = Convert.ToDecimal(TextBoxRetiredBenefit.Text)
            End If
            If CheckBoxSavPlan.Checked Then
                decTaxable = decTaxable + Convert.ToDecimal(TextBoxTaxableSav.Text)
                decNonTaxable = decNonTaxable + Convert.ToDecimal(TextBoxNonTaxableSav.Text)
            End If

            If (CheckboxIncludeSSLevelling.Checked = True) And (Convert.ToDouble(TextBoxSSBenefit.Text) > 0) Then
                decTaxable = decTaxable + Convert.ToDecimal(TextBoxSSIncrease.Text)
            End If

            Dim finalFundStatus As String = String.Empty
            If IsPrePlanSplitRetirement <> True Then
                Dim businessLogic As RetirementBOClass = New RetirementBOClass
                finalFundStatus = businessLogic.DecideFinalRetirementFundStatus(dtSelectedAnnuity, Me.PersId, Me.FundEventStatus, Session("RetirementBalance"), Session("SavingsBalance"))

            Else
                finalFundStatus = "RD"
            End If


            'START : SR | 2016.08.02 | YRS-AT-2382 |  Getting only Updated SSN values for inserting in Audit DataTable, inserting records in Audit Log
            Dim dsBeneficiariesSSN As DataSet
            If Not Session("AuditBeneficiariesTable") Is Nothing Then
                Dim dtBeneficiariesSSNchanges As DataTable = DirectCast(Session("AuditBeneficiariesTable"), DataTable)
                Dim bIsRowDeleted As Boolean = False
                If HelperFunctions.isNonEmpty(dtBeneficiariesSSNchanges) Then
                    Do
                        ' Delete beneficiaries whose SSN not changes
                        bIsRowDeleted = False
                        For Each row As DataRow In dtBeneficiariesSSNchanges.Rows
                            If Not Convert.ToBoolean(row("IsEdited")) Then
                                row.Delete()
                                bIsRowDeleted = True
                                Exit For
                            End If
                        Next

                        If (Not bIsRowDeleted) Then
                            Exit Do
                        End If
                    Loop Until False
                End If

                dsBeneficiariesSSN = New DataSet("dsBeneficiariesSSN")

                If dtBeneficiariesSSNchanges.Rows.Count > 0 Then
                    dsBeneficiariesSSN.Tables.Add(dtBeneficiariesSSNchanges)
                    dsBeneficiariesSSN.Tables(0).TableName = "Audit"
                    Session("AuditBeneficiariesTable") = Nothing
                End If
            End If
            'END : SR | 2016.08.02 | YRS-AT-2382 | Getting only Updated SSN values for inserting in Audit DataTable  , inserting records in Audit Log
			
            'START : ML |2019.11.27 |YRS-AT=4597 | Get data in variable from Session for STW Saving
            Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
            Dim objStateWithholdingDetails As YMCAObjects.StateWithholdingDetails
            Dim isStateWithholdingDataSave As Boolean
            isStateWithholdingDataSave = False
            LstSWHPerssDetail = Nothing
            objStateWithholdingDetails = Nothing

            If (stwListUserControl.STWDataSaveAtMainPage) And (HelperFunctions.isNonEmpty(SessionManager.SessionStateWithholding.LstSWHPerssDetail)) Then
                If (SessionManager.SessionStateWithholding.LstSWHPerssDetail.Count > 0) Then
                    objStateWithholdingDetails = SessionManager.SessionStateWithholding.LstSWHPerssDetail.FirstOrDefault
                End If
            End If
            'END : ML |2019.11.27 |YRS-AT=4597 | Get data in variable from Session for STW Saving

            l_bool_Return = RetirementBOClass.Purchase(Me.PersId, _
                                Me.RetirementDate, Me.ycRetireeBirthDay, strRetireType, _
                                Me.LoggedUserKey, _
                                decRetiredBenefit, yrsDeathBenefitUsed, _
                                decTaxable, decNonTaxable, _
                                Convert.ToDecimal(TextBoxSSBenefit.Text), yrsSSDate, yrsSSDateSav, _
                                TextBoxAnnuitySelectRet.Text, TextBoxAnnuitySelectSav.Text, _
                                l_dataset_FedWithDrawal, l_dataset_genWithDrawal, _
                                l_dataset_RetiredBen, l_dataset_BeneficiaryAddress, l_dataset_Notes, l_dataset_AnnuityJointSurvivors, _
                                dtSelectedAnnuity, Me.guiFundEventId, Me.FundEventStatus, finalFundStatus, IsPrePlanSplitRetirement, dsBeneficiariesSSN, objStateWithholdingDetails) ' ML |2019.11.27 |YRS-AT=4597 | Added new parameter to Save STW data

            If l_bool_Return = True Then
                'getting again back the federal withholding records if their were changes been made by the user
                If Not l_dataset_FedWithDrawal Is Nothing Then
                    If Not l_dataset_FedWithDrawal.Tables(0).GetChanges Is Nothing Then
                        If l_dataset_FedWithDrawal.Tables(0).GetChanges.Rows.Count > 0 Then
                            blnChanged = True
                        End If
                    End If
                    l_dataset_FedWithDrawal.AcceptChanges()
                    If blnChanged = True Then
                        l_dataset_FedWithDrawal = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpFedWithDrawals(Me.PersId)
                        blnChanged = False
                    End If
                    Me.FedWithDrawals = l_dataset_FedWithDrawal
                End If

                'getting again back the general withholding records if their were changes been made by the user
                blnChanged = False
                If Not l_dataset_genWithDrawal Is Nothing Then
                    If Not l_dataset_genWithDrawal.Tables(0).GetChanges Is Nothing Then
                        If l_dataset_genWithDrawal.Tables(0).GetChanges.Rows.Count > 0 Then
                            blnChanged = True
                        End If
                    End If
                    l_dataset_genWithDrawal.AcceptChanges()
                    If blnChanged = True Then
                        l_dataset_genWithDrawal = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpGenWithDrawals(Me.PersId)
                        blnChanged = False
                    End If
                    Me.GenWithDrawals = l_dataset_genWithDrawal
                End If


                Call ClearResources()
                yrsRetire = True
            Else
                yrsRetire = False
            End If
        Catch
            yrsRetire = False
            Throw
        End Try
    End Function
    Private Sub GetPersonDetails()
        Try
            Dim strFundEventId As String
            Dim dsPersRetiree As DataSet
            strFundEventId = Me.guiFundEventId
            Me.PersonDetails = BORetireeEstimate.GetPersonDetails(strFundEventId)
            dsPersRetiree = Me.PersonDetails
            Me.PersId = dsPersRetiree.Tables(0).Rows(0).Item("guiUniqueID").ToString()
            Me.SSNo = dsPersRetiree.Tables(0).Rows(0).Item("chrSSNO").ToString()
            PersonInformationForMenuHeader = (" - Fund ID : " _
             + dsPersRetiree.Tables(0).Rows(0).Item("intFundIdNo").ToString() _
             + " - " + dsPersRetiree.Tables(0).Rows(0).Item("chvLastName").ToString() _
             + ", " + dsPersRetiree.Tables(0).Rows(0).Item("chvFirstName").ToString())

            'START : ML |2019.09.20  |YRS-AT-4597 - Set property for State withholding control
            stwListUserControl.PersonID = dsPersRetiree.Tables(0).Rows(0).Item("guiUniqueID").ToString()
            stwListUserControl.STWDataSaveAtMainPage = True
            'END :  ML |2019.09.20  |YRS-AT-4597 - Set property  for State withholding control
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ClearResources()
        Session("RP_DataChanged") = Nothing
        Session("TextBoxAnnuityMiddleNameRet.Text") = Nothing
        Session("TextBoxAnnuitySSNoRet.Text") = Nothing
        Session("TextBoxBirthDateRet.Text") = Nothing
        Session("TextBoxAmount.Text") = Nothing
        Session("TextBoxFirstName.Text") = Nothing
        Session("TextBoxLastName.Text") = Nothing
        Session("TextBoxMiddleName.Text") = Nothing
        Session("TextBoxReservesRet.Text") = Nothing
        Session("TextBoxSSBenefit.Text") = Nothing
        Session("TextBoxSSIncrease.Text") = Nothing
        Session("TextBoxSSDecrease.Text") = Nothing
        Session("TextBoxSSSalary.Text") = Nothing
        Session("DropDownListPercentage.SelectedValue") = Nothing
        Session("DropDownListRetirementType.SelectedValue") = Nothing
        Session("Annuity_Type") = Nothing
        Session("Annuity_Desc") = Nothing
        Session("Taxable_Amount") = Nothing
        Session("NonTaxable") = Nothing
        Session("Amount") = Nothing
        Session("SSIncrease") = Nothing
        Session("SSDecrease") = Nothing
        Session("TextBoxRetiredBenefit.Text") = Nothing
        Session("TextBoxAnnuityBirthDate.Text") = Nothing
        Session("TextBoxAnnuityFirstNameRet.Text") = Nothing
        Session("TextBoxAnnuityLastNameRet.Text") = Nothing
        Session("Page") = Nothing
        Session("Name") = Nothing
        Session("Name2") = Nothing
        Session("TaxID") = Nothing
        Session("Rel") = Nothing
        Session("Birthdate") = Nothing
        Session("Groups") = Nothing
        Session("Lvl") = Nothing
        Session("Pct") = Nothing
        Session("_icounter") = Nothing
        Session("Person_Info") = Nothing
        Session("blnCancel") = Nothing
        Session("dtNotes") = Nothing
        Session("Note") = Nothing
        Session("blnAddNotes") = Nothing
        Session("blnUpdateNotes") = Nothing
        Session("blnAddFedWithHolding") = Nothing
        Session("EnableSaveCancel") = Nothing
        Session("FedWithDrawals") = Nothing
        Session("cmbTaxEntity") = Nothing
        Session("cmbWithHolding") = Nothing
        Session("txtAddlAmount") = Nothing
        Session("cmbMaritalStatus") = Nothing
        Session("txtExemptions") = Nothing
        Session("GenWithDrawals") = Nothing
        Session("cmbWithHoldingType") = Nothing
        Session("txtAddAmount") = Nothing
        Session("txtStartDate") = Nothing
        Session("txtEndDate") = Nothing
        Session("blnAddGenWithHolding") = Nothing
        Session("RP_PersId") = Nothing
        Session("RP_dsElectiveAccounts") = Nothing
        Session("RP_BeneficiaryInfo") = Nothing
        Session("PersonDetails") = Nothing
        Session("AnnuitySelected") = Nothing
        Session("Flag") = Nothing
        Session("LastSalPaidDate") = Nothing
        Session("SSNo") = Nothing
        Session("dsRetEmpInfo") = Nothing
        Session("EndWorkDate") = Nothing
        Session("RetEstimateEmployment") = Nothing
        Session("ValidationMessage") = Nothing
        Session("blnAddFedWithHoldings") = Nothing
        Session("blnUpdateFedWithHoldings") = Nothing
        Session("blnAddGenWithHoldings") = Nothing
        Session("ds_SelectAnnuity") = Nothing
        Session("TextBoxRetirementDate.Text") = Nothing
        Session("blnUpdateGenWithDrawals") = Nothing
        Session("iCounter") = Nothing
        Session("Blank") = Nothing
        Session("BeneficiariesActive") = Nothing
        Session("BeneficiariesRetired") = Nothing
        Session("IsFedTaxForMaritalStatus") = Nothing
        Session("PhonySSNo") = Nothing

        Session("BasicValidationFailed") = Nothing
        Session("BasicValidationMessage") = Nothing

        'YRPS-4704
        Session("MaritalStatus") = Nothing
        'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
        Session("TransAfterExactAgeEffDate") = Nothing
        Session("ExactAgeEffDate") = Nothing
        Session("CalledAnnuity") = Nothing

        Session("dtAnnuityList_ExactAgeEffDate") = Nothing
        Session("dtAnnuityListSav_ExactAgeEffDate") = Nothing
        Session("lcAnnuitiesListCursor_ExactAgeEffDate") = Nothing
        Session("dtAnnuitiesFullBalanceList_ExactAgeEffDate") = Nothing
        Session("dtAnnuitieslistComboTmp_ExactAgeEffDate") = Nothing

        Session("dtAnnuityList_BeforeExactAgeEffDate") = Nothing
        Session("dtAnnuityListSav_BeforeExactAgeEffDate") = Nothing
        Session("lcAnnuitiesListCursor_BeforeExactAgeEffDate") = Nothing
        Session("dtAnnuitiesFullBalanceList_BeforeExactAgeEffDate") = Nothing
        Session("dtAnnuitieslistComboTmp_BeforeExactAgeEffDate") = Nothing

        Session("ds_SelectAnnuityRetirement_ExactAgeEffDate") = Nothing
        Session("ds_SelectAnnuitySavings_ExactAgeEffDate") = Nothing

        Session("ds_SelectAnnuitySavings_ExactAgeEffDate_Combined") = Nothing
        Session("ds_SelectAnnuityRet_ExactAgeEffDate_Combined") = Nothing
        'Session("SelectedCalledAnnuity") = Nothing
        Session("CalculatedDB") = Nothing
        Session("CalculatedDB_BeforeExactAgeEffDate") = Nothing
        Session("CalculatedDB_ExactAgeEffDate") = Nothing
        Session("SelectedCalledAnnuity_Ret") = Nothing
        Session("SelectedCalledAnnuity_Sav") = Nothing
        'Added  by Sanket for disability
        Session("RetEmpInfo") = Nothing
        'SANKET:03/24/2011 for YRS 5.0-1294 
        Session("TerminationDate") = Nothing
        Session("MessageBox") = Nothing
        Session("BackDatedDisabilityRetirementQuestionAsked") = Nothing
        Session("ValidTerminationDateForDisability") = Nothing

        Session("RetireeBirthDatePresent") = Nothing
        Session("ExistingAnnuityPurchaseDtNotExists") = Nothing

        '2012.05.18  SP:  BT-975/YRS 5.0-1508
        Session("dsSelectedParticipantBeneficiary") = Nothing
        '2012.06.04  SP:  BT-975/YRS 5.0-1508
        Session("dsSelectedParticipantBeneficiarySav") = Nothing
        'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
        Session("IsDefaultBeneficiarySpouse") = Nothing
        Session("IsDefaultBeneficiarySpouseSav") = Nothing
        'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
        Session("RDBwarningMessage") = Nothing 'BD : 2018.11.01 : YRS-AT-4135 : Clear rdb warning message
        Me.ManualTransactionDetails = Nothing 'MMR | 2017.03.06 | YRS-AT-2625 | Clearing poperty value
        'START : ML | 2019.09.20 | YRS-AT-4597 |Clear Session 
        SessionManager.SessionStateWithholding.LstSWHPerssDetail = Nothing
        Session("AnnuityAmount") = Nothing
        'END : ML | 2019.09.20 | YRS-AT-4597 |Clear Session 
    End Sub
    'Private Sub SetRetireType()
    '    Dim lcRetireType As String
    '    If DropDownListRetirementType.SelectedItem.ToString.Trim.ToUpper() <> "" Then
    '        lcRetireType = DropDownListRetirementType.SelectedItem.ToString.Trim.ToUpper()
    '        If lcRetireType = "DISABLED" Then
    '            lcRetireType = "DISABL"
    '        Else
    '            lcRetireType = "NORMAL"
    '        End If

    '        Me.RetireType = lcRetireType
    '    Else
    '        If Me.RetireType <> "" Then
    '            If Me.RetireType = "NORMAL" Then
    '                DropDownListRetirementType.SelectedValue = "NORMAL"
    '            ElseIf Me.RetireType = "DISABL" Then
    '                DropDownListRetirementType.SelectedValue = "DISABLED"
    '            End If
    '        Else
    '            Me.RetireType = "NORMAL"
    '            DropDownListRetirementType.SelectedValue = "NORMAL"
    '        End If
    '    End If
    '    Me.CustomRetireType = "NORMAL"
    'End Sub

    'SANKET:03/24/2011 code for YRS 5.0-1294
    'Private Sub SetRetirementDateForDisability()
    '    Dim lastTermDate As DateTime
    '    lastTermDate = GetLastTerminationDate(Me.guiFundEventId)

    '    Me.TerminationDate = lastTermDate.ToString("MM/dd/yyyy")
    '    If (lastTermDate.Month = 12) Then
    '        If (lastTermDate.Day = 1) Then
    '            Me.RetirementDate = lastTermDate
    '        Else
    '            Me.RetirementDate = "1/1/" + Convert.ToString(lastTermDate.Year + 1)
    '        End If
    '        TextBoxRetirementDate.Text = Me.RetirementDate
    '    ElseIf (lastTermDate.Day > 1) Then
    '        Me.RetirementDate = Convert.ToString(lastTermDate.Month + 1) + "/1/" + Convert.ToString(lastTermDate.Year)
    '        TextBoxRetirementDate.Text = Me.RetirementDate
    '    ElseIf (lastTermDate.Day = 1) Then
    '        Me.RetirementDate = lastTermDate.ToString("MM/dd/yyyy")
    '        TextBoxRetirementDate.Text = Me.RetirementDate
    '    End If
    'End Sub

    'Previous : 1294
    'Private Sub SetRetirementDate()
    '    'Retirement Date selection / validation
    '    If TextBoxRetirementDate.Text.Trim() = "" Then
    '        If Me.RetirementDate = "" Then
    '            If Not Me.LastSalPaidDate Is Nothing Then
    '                TextBoxRetirementDate.Text = Me.LastSalPaidDate
    '                Me.RetirementDate = Me.LastSalPaidDate
    '            End If
    '        Else
    '            If Convert.ToDateTime(Me.RetirementDate) > Convert.ToDateTime(Me.LastSalPaidDate) Then
    '                TextBoxRetirementDate.Text = Me.LastSalPaidDate
    '                Me.RetirementDate = Me.LastSalPaidDate
    '            Else
    '                TextBoxRetirementDate.Text = Me.RetirementDate
    '            End If
    '        End If
    '    Else
    '        If Not Me.LastSalPaidDate Is Nothing Then
    '            If Convert.ToDateTime(TextBoxRetirementDate.Text) > Convert.ToDateTime(Me.LastSalPaidDate) Then
    '                Me.RetirementDate = Me.LastSalPaidDate
    '                TextBoxRetirementDate.Text = Me.LastSalPaidDate
    '                Me.DataChanged = True
    '            Else
    '                If Not Me.RetirementDate Is Nothing Then
    '                    If Me.RetirementDate <> TextBoxRetirementDate.Text.Trim() Then
    '                        Me.DataChanged = True
    '                    End If
    '                End If

    '                TextBoxRetirementDate.Text = GetFirstDayOfMonth(TextBoxRetirementDate.Text.Trim())
    '                Me.RetirementDate = TextBoxRetirementDate.Text.Trim()
    '            End If
    '        Else
    '            If Not Me.RetirementDate Is Nothing Then
    '                If Me.RetirementDate <> TextBoxRetirementDate.Text.Trim() Then
    '                    Me.DataChanged = True
    '                End If
    '            End If

    '            TextBoxRetirementDate.Text = GetFirstDayOfMonth(TextBoxRetirementDate.Text.Trim())
    '            Me.RetirementDate = TextBoxRetirementDate.Text.Trim()
    '        End If
    '    End If



    '    If Not Page.IsPostBack Then
    '        Session("PlanSplitDate") = RetirementBOClass.GetPlanSplitDate()
    '        Me.IsPrePlanSplitRetirement = (Convert.ToDateTime(Me.RetirementDate) < Session("PlanSplitDate"))
    '    End If

    'End Sub

    'SANKET:03/24/2011 code for YRS 5.0-1294 
    Private Sub SetRetirementDate()

        'SP 2013.12.13 BT-2326 -Added try catch block
        Try

            If (Me.RetireType = "NORMAL") Then
                'Retirement Date selection / validation
                If TextBoxRetirementDate.Text.Trim() = "" Then
                    If Me.RetirementDate = "" Then
                        If Not Me.LastSalPaidDate Is Nothing Then
                            TextBoxRetirementDate.Text = Me.LastSalPaidDate
                            Me.RetirementDate = Me.LastSalPaidDate
                        End If
                    Else
                        If Convert.ToDateTime(Me.RetirementDate) > Convert.ToDateTime(Me.LastSalPaidDate) Then
                            TextBoxRetirementDate.Text = Me.LastSalPaidDate
                            Me.RetirementDate = Me.LastSalPaidDate
                        Else
                            TextBoxRetirementDate.Text = Me.RetirementDate
                        End If
                    End If
                Else
                    If Not Me.LastSalPaidDate Is Nothing Then
                        If Convert.ToDateTime(TextBoxRetirementDate.Text) > Convert.ToDateTime(Me.LastSalPaidDate) Then
                            Me.RetirementDate = Me.LastSalPaidDate
                            TextBoxRetirementDate.Text = Me.LastSalPaidDate
                            Me.DataChanged = True
                        Else
                            If Not Me.RetirementDate Is Nothing Then
                                If Me.RetirementDate <> TextBoxRetirementDate.Text.Trim() Then
                                    Me.DataChanged = True
                                End If
                            End If

                            TextBoxRetirementDate.Text = GetFirstDayOfMonth(TextBoxRetirementDate.Text.Trim())
                            Me.RetirementDate = TextBoxRetirementDate.Text.Trim()
                        End If
                    Else
                        If Not Me.RetirementDate Is Nothing Then
                            If Me.RetirementDate <> TextBoxRetirementDate.Text.Trim() Then
                                Me.DataChanged = True
                            End If
                        End If

                        TextBoxRetirementDate.Text = GetFirstDayOfMonth(TextBoxRetirementDate.Text.Trim())
                        Me.RetirementDate = TextBoxRetirementDate.Text.Trim()
                    End If
                End If
                'SANKET:03/24/2011 code for YRS 5.0-1294 
            ElseIf (Me.RetireType = "DISABL") Then
                If TextBoxRetirementDate.Text.Trim() = "" Then
                    Dim lastTermDate As DateTime
                    lastTermDate = GetLastTerminationDate(Me.guiFundEventId)

                    Me.TerminationDate = lastTermDate.ToString("MM/dd/yyyy")
                    If (lastTermDate.Month = 12) Then
                        If (lastTermDate.Day = 1) Then
                            Me.RetirementDate = lastTermDate
                        Else
                            Me.RetirementDate = "1/1/" + Convert.ToString(lastTermDate.Year + 1)
                        End If
                        TextBoxRetirementDate.Text = Me.RetirementDate
                    ElseIf (lastTermDate.Day > 1) Then
                        Me.RetirementDate = Convert.ToString(lastTermDate.Month + 1) + "/1/" + Convert.ToString(lastTermDate.Year)
                        TextBoxRetirementDate.Text = Me.RetirementDate
                    ElseIf (lastTermDate.Day = 1) Then
                        Me.RetirementDate = lastTermDate.ToString("MM/dd/yyyy")
                        TextBoxRetirementDate.Text = Me.RetirementDate
                    End If
                    'Set the temination date
                    Session("ValidTerminationDateForDisability") = Me.RetirementDate
                End If
            End If

            If Not Page.IsPostBack Then
                Session("PlanSplitDate") = RetirementBOClass.GetPlanSplitDate()
                Me.IsPrePlanSplitRetirement = (Convert.ToDateTime(Me.RetirementDate) < Session("PlanSplitDate"))
            End If

        Catch
            Throw
        End Try

    End Sub
    Private Function GetLastTerminationDate(ByVal fundEventID As String) As DateTime
        Return RetirementBOClass.GetLastTerminationDate(fundEventID)
    End Function


    Private Function GetFirstDayOfMonth(ByVal pstrValue As String) As String
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            If IsDate(Convert.ToDateTime(pstrValue)) Then
                Return Convert.ToDateTime(Convert.ToDateTime(pstrValue.Trim()).Month & "/01/" & _
                  Convert.ToDateTime(pstrValue.Trim()).Year).ToString("MM/dd/yyyy")
            Else
                Return pstrValue
            End If

        Catch
            Throw
        End Try
    End Function
    Private Function GetWithHolding() As Double
        Dim dsFedWithDrawals As DataSet
        Dim dtFedWithDrawals As DataTable

        Dim dsGenWithDrawals As DataSet
        Dim dtGenWithDrawals As DataTable

        Dim drRow As DataRow
        Dim drRows As DataRow()

        Dim dsTaxEntityTypes As DataSet
        Dim dsTaxFactors As DataSet

        Dim l_double_ExemptionsDefault As Double
        Dim l_string_MarriageStatusDefault As String
        Dim l_double_ValidExemptionsMaximum As Double = 0

        Dim l_string_row_MaritalStatusCode As String
        Dim l_string_row_WithholdingType As String
        Dim l_double_row_Exemptions As Double
        Dim l_string_row_TaxEntityCode As String
        Dim l_double_row_AdditionalAmount As Double

        Dim l_integer_MetaTaxFactorCount As Integer = 0
        Dim l_double_CreditAdjustment As Double = 0
        Dim l_double_TaxPercent As Double = 0
        Dim l_double_Withholding As Double = 0
        Dim l_double_Fed_WithHolding As Double = 0
        Dim l_double_Tot_Withholding As Double = 0

        Dim l_double_TaxCalculationBase As Double
        Dim l_double_TotalTaxable As Double
        Dim l_double_Allowance As Double


        Dim l_integer_NoOfMonths As Integer

        Try
            If Not Me.FedWithDrawals Is Nothing Then
                dsFedWithDrawals = Me.FedWithDrawals
                If dsFedWithDrawals.Tables.Count > 0 Then
                    dtFedWithDrawals = dsFedWithDrawals.Tables(0)
                End If
            End If

            If Not Me.GenWithDrawals Is Nothing Then
                dsGenWithDrawals = Me.GenWithDrawals
                If dsGenWithDrawals.Tables.Count > 0 Then
                    dtGenWithDrawals = dsGenWithDrawals.Tables(0)
                End If
            End If

            If Not dtFedWithDrawals Is Nothing Then
                If dtFedWithDrawals.Rows.Count > 0 Then
                    '--------------Federal withholdings------------------
                    If Me.TaxEntityTypes Is Nothing Then
                        dsTaxEntityTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.TaxEntityTypes()
                    Else
                        Me.TaxEntityTypes = dsTaxEntityTypes
                    End If

                    If Me.TaxFactors Is Nothing Then
                        dsTaxFactors = YMCARET.YmcaBusinessObject.RetirementBOClass.TaxFactors()
                    Else
                        Me.TaxFactors = dsTaxFactors
                    End If

                    If Me.WITHHOLDING_DEFAULT_MARRIAGE_STATUS Is Nothing Then
                        Try
                            l_double_ExemptionsDefault = GetConfigValues("WITHHOLDING_DEFAULT_EXEMPTIONS")
                        Catch
                            l_double_ExemptionsDefault = 0
                        End Try
                        Me.WITHHOLDING_DEFAULT_EXEMPTIONS = l_double_ExemptionsDefault

                        Try
                            l_string_MarriageStatusDefault = GetConfigValues("WITHHOLDING_DEFAULT_MARRIAGE_STATUS")
                        Catch
                            l_string_MarriageStatusDefault = "M"
                        End Try
                        Me.WITHHOLDING_DEFAULT_MARRIAGE_STATUS = l_string_MarriageStatusDefault

                        '----------Formula (tax table) withholdings----------------------------
                        '----------Get number of maximum valid exemptions----------------------
                        Try
                            l_double_ValidExemptionsMaximum = GetConfigValues("WITHHOLDING_MAXIMUM_VAID_EXEMPTIONS")
                        Catch ex As Exception
                            l_double_ValidExemptionsMaximum = 50
                        End Try
                        Me.WITHHOLDING_MAXIMUM_VAID_EXEMPTIONS = l_double_ValidExemptionsMaximum
                    Else
                        l_double_ExemptionsDefault = Me.WITHHOLDING_DEFAULT_EXEMPTIONS
                        l_string_MarriageStatusDefault = Me.WITHHOLDING_DEFAULT_MARRIAGE_STATUS
                        l_double_ValidExemptionsMaximum = Me.WITHHOLDING_MAXIMUM_VAID_EXEMPTIONS
                    End If

                    If l_double_ValidExemptionsMaximum = 0 Then
                        l_double_ValidExemptionsMaximum = 50
                    End If

                    l_integer_NoOfMonths = Me.MonthsinCheckOne

                    If l_integer_NoOfMonths < 1 Then
                        l_integer_NoOfMonths = 1
                    End If

                    Dim l_double_Taxable As Double = 0
                    If CheckBoxRetPlan.Checked Then
                        l_double_Taxable += Convert.ToDouble(TextBoxTaxableRet.Text)
                    End If
                    If TextBoxTaxableSav.Text.Trim <> String.Empty Then
                        If CheckBoxSavPlan.Checked Then
                            l_double_Taxable += Convert.ToDouble(TextBoxTaxableSav.Text)
                        End If
                    End If

                    l_double_TotalTaxable = l_double_Taxable '/ l_integer_NoOfMonths

                    For Each drRow In dtFedWithDrawals.Rows

                        l_string_row_WithholdingType = drRow("Type")
                        l_string_row_WithholdingType = l_string_row_WithholdingType.Trim()

                        'modified by hafiz to handle null value on 01-Dec-2006 for issue reported thru mail from Ragesh(onsite)/George.
                        If drRow("Marital Status").GetType.ToString() = "System.DBNull" Then
                            l_string_row_MaritalStatusCode = ""
                        Else
                            l_string_row_MaritalStatusCode = drRow("Marital Status")
                        End If

                        l_string_row_MaritalStatusCode = l_string_row_MaritalStatusCode.Trim()

                        l_double_row_Exemptions = drRow("Exemptions")

                        l_string_row_TaxEntityCode = drRow("Tax Entity")
                        l_string_row_TaxEntityCode = l_string_row_TaxEntityCode.Trim()

                        If drRow("Add'l Amount").GetType.ToString() = "System.DBNull" Then
                            l_double_row_AdditionalAmount = 0
                        Else
                            l_double_row_AdditionalAmount = drRow("Add'l Amount")
                        End If

                        '------------------Determine which allowance to use NYState or Fed---------------------------
                        drRows = dsTaxEntityTypes.Tables(0).Select("TaxEntitytype='" & l_string_row_TaxEntityCode & "'")
                        If drRows.Length > 0 Then
                            l_double_Allowance = drRows(0)("ExemptionAllowance")
                        End If

                        Select Case l_string_row_WithholdingType
                            Case "DEFALT"
                                l_double_row_Exemptions = l_double_ExemptionsDefault
                                l_string_row_MaritalStatusCode = l_string_MarriageStatusDefault
                                l_double_row_AdditionalAmount = 0

                            Case "FLAT"
                                l_double_row_Exemptions = 0
                                If drRow("Add'l Amount").GetType.ToString() = "System.DBNull" Then
                                    l_double_row_AdditionalAmount = 0
                                Else
                                    l_double_row_AdditionalAmount = drRow("Add'l Amount")
                                End If

                            Case "FORMUL"
                                If drRow("Add'l Amount").GetType.ToString() = "System.DBNull" Then
                                    l_double_row_AdditionalAmount = 0
                                Else
                                    l_double_row_AdditionalAmount = drRow("Add'l Amount")
                                End If
                                l_double_row_Exemptions = drRow("Exemptions")

                            Case Else
                                Throw (New Exception("Invalid Withholding type encountered while getting the withheld amount."))

                        End Select

                        '--------------------See if these settings qualify for formula based withholdings-------------------
                        If l_string_row_WithholdingType <> "FLAT" And l_double_row_Exemptions <= l_double_ValidExemptionsMaximum Then
                            l_double_TaxCalculationBase = l_double_TotalTaxable - (l_double_row_Exemptions * l_double_Allowance)

                            '---------Set lnTaxCalculationBase to zero if lesser than zero--------------
                            If l_double_TaxCalculationBase <= 0 Then
                                l_double_TaxCalculationBase = 0
                            End If

                            '---------Check for Invalid marriage status-----------------------
                            If Not (l_string_row_MaritalStatusCode = "S" Or l_string_row_MaritalStatusCode = "M") Then
                                l_string_row_MaritalStatusCode = "S"
                            End If

                            drRows = dsTaxFactors.Tables(0).Select("TaxEntityCode='" & l_string_row_TaxEntityCode & "'" & _
                                                                   " AND MaritalStatusCode = '" & l_string_row_MaritalStatusCode & "'" & _
                                                                   " AND TaxableLow <" & l_double_TaxCalculationBase & _
                                                                   " AND TaxableHigh >=" & l_double_TaxCalculationBase)
                            l_integer_MetaTaxFactorCount = drRows.Length
                            If l_integer_MetaTaxFactorCount > 0 Then
                                l_double_TaxPercent = drRows(0)("TaxPercent")
                                l_double_CreditAdjustment = drRows(0)("CreditAdjustment")

                                l_double_Withholding = ((l_double_TaxCalculationBase * l_double_TaxPercent / 100) + l_double_CreditAdjustment)
                            End If

                            '---------------Flat Tax Calculation----------------------
                            l_double_row_AdditionalAmount = l_double_Withholding + l_double_row_AdditionalAmount
                        End If

                        l_double_Fed_WithHolding = l_double_Fed_WithHolding + l_double_row_AdditionalAmount
                    Next
                End If
            End If

            Dim l_string_row_StartDate As String
            Dim l_string_row_EndDate As String
            Dim l_double_row_Amount As Double
            Dim l_double_Gen_Withholding As Double = 0
            Dim annuityPurchaseDate As DateTime '2012.07.16	SP: BT-753/YRS 5.0-1270  -rename l_datetime_futureonemonth with annuityPurchaseDate 

            If Not dtGenWithDrawals Is Nothing Then
                If dtGenWithDrawals.Rows.Count > 0 Then
                    ' l_datetime_futureonemonth = DateTime.Today.AddMonths(1).AddDays(-1) '2012.07.16	SP: BT-753/YRS 5.0-1270 : purchase page -Commented (commented)
                    annuityPurchaseDate = Convert.ToDateTime(Me.RetirementDate)

                    '--------------General withholdings------------------
                    For Each drRow In dtGenWithDrawals.Rows
                        l_string_row_StartDate = drRow("Start Date")
                        If Not drRow("End Date") Is DBNull.Value Then
                            l_string_row_EndDate = drRow("End Date")
                        Else
                            l_string_row_EndDate = String.Empty
                        End If

                        l_double_row_Amount = drRow("Add'l Amount")

                        If Convert.ToDateTime(l_string_row_StartDate) <= annuityPurchaseDate Then '2012.07.16	SP: BT-753/YRS 5.0-1270  add equal to
                            'If l_string_row_EndDate.GetType.ToString() = "System.DBNull" Then - '2012.07.16	SP: BT-753/YRS 5.0-1270 : purchase page -Commented 
                            If String.IsNullOrEmpty(l_string_row_EndDate) Then
                                If l_double_row_Amount > 0 Then
                                    l_double_Gen_Withholding = l_double_Gen_Withholding + l_double_row_Amount
                                End If
                                'ElseIf l_string_row_EndDate.GetType.ToString() <> "System.DBNull" And l_string_row_EndDate <> String.Empty Then '2012.07.16	SP: BT-753/YRS 5.0-1270 : purchase page -Commented 
                            ElseIf Not (String.IsNullOrEmpty(l_string_row_EndDate)) Then
                                If Convert.ToDateTime(l_string_row_EndDate) > annuityPurchaseDate Then
                                    If l_double_row_Amount > 0 Then
                                        l_double_Gen_Withholding = l_double_Gen_Withholding + l_double_row_Amount
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            l_double_Tot_Withholding = Round(l_double_Fed_WithHolding + l_double_Gen_Withholding, 2)

            Return l_double_Tot_Withholding
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function GetMonthsinCheckOne() As Integer
        Dim dsLastPayrollDate As New DataSet
        Dim ldLastPayrollDate As String
        Dim lnMonthsinCheckOne As Decimal

        Try
            dsLastPayrollDate = BORetireeEstimate.getLastPayrollDate()
            If Not dsLastPayrollDate.Tables.Count = 0 Then
                If Not dsLastPayrollDate.Tables(0).Rows.Count = 0 Then
                    ldLastPayrollDate = Convert.ToDateTime(dsLastPayrollDate.Tables(0).Rows(0).Item("Month") & "/01/" & _
                         dsLastPayrollDate.Tables(0).Rows(0).Item("Year"))

                    Me.LastPayrollDate = ldLastPayrollDate

                    lnMonthsinCheckOne = Convert.ToDecimal(Convert.ToDateTime(ldLastPayrollDate).Year - Convert.ToDateTime(Me.RetirementDate).Year) * 12 + _
                          Convert.ToDecimal(Convert.ToDateTime(ldLastPayrollDate).Month - Convert.ToDateTime(Me.RetirementDate).Month) + 1

                    If lnMonthsinCheckOne < 1 Then
                        lnMonthsinCheckOne = 1
                    End If
                Else
                    lnMonthsinCheckOne = 1
                End If

                GetMonthsinCheckOne = lnMonthsinCheckOne
            End If
        Catch
            Throw
        End Try
    End Function
    Private Function GetConfigValues(ByVal p_string_Key As String)
        Dim dsConfigValues As DataSet
        Dim l_Value
        Try
            dsConfigValues = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.SearchConfigurationMaintenance(p_string_Key)
            If dsConfigValues.Tables.Count > 0 Then
                If dsConfigValues.Tables(0).Rows.Count > 0 Then
                    If Not dsConfigValues.Tables(0).Rows(0)("VALUE").GetType.ToString() = "System.DBNull" Then
                        If Not dsConfigValues.Tables(0).Rows(0)("VALUE") = String.Empty Then
                            l_Value = dsConfigValues.Tables(0).Rows(0)("VALUE")
                        End If
                    End If
                End If
            End If
            dsConfigValues = Nothing

            Return l_Value
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetNonSpouseBirthDate() As String
        'SP 2013.12.13 BT-2326 - Added try catch block
        Try
            Dim strBirthdate As String = String.Empty
            If HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiary")) Then
                Dim drrow As DataRow = CType(Session("dsSelectedParticipantBeneficiary"), DataSet).Tables(0).Rows(0)
                If drrow("BenBirthDate").ToString <> String.Empty Then
                    'strBirthdate=
                End If

            End If

        Catch
            Throw
        End Try

    End Function

#End Region

#Region " All Events "
    Private Sub ButtonSelectRet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSelectRet.Click
        Try
            Call SetRetirementDate()
            'Priya 2010.11.17 YRS 5.0-1215 : Exact age vs nearest age annuity calculations
            'Need to make changes according to compareannuity and on that basis WE will call Annuity.
            'Me.SelectAnnuityCurrent = Me.SelectAnnuityRetirement
            Me.SelectedPlan = "R"
            Session("SelectedAnnuity") = TextBoxAnnuitySelectRet.Text
            Dim popupScript As String
            popupScript = "<script language='javascript'>" & _
                "window.open('ChooseAnnuityForm.aspx', 'CustomPopUp', " & _
                "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                "</script>"
            Page.RegisterStartupScript("PopupScript2", popupScript)
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonSelectRet_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonSelectSav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSelectSav.Click
        Try
            Call SetRetirementDate()
            'Me.SelectAnnuityCurrent = Me.SelectAnnuitySavings
            Me.SelectedPlan = "S"
            Session("SelectedAnnuity") = TextBoxAnnuitySelectSav.Text
            Dim popupScript As String
            popupScript = "<script language='javascript'>" & _
                "window.open('ChooseAnnuityForm.aspx', 'CustomPopUp', " & _
                "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                "</script>"
            Page.RegisterStartupScript("PopupScript2", popupScript)
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonSelectSav_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonPriR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPriR.Click
        Try
            ButtonCancel.Enabled = True
            Me.Equalize(Me.BeneficiariesRetired, "P", "R")
        Catch sqlEx As SqlException
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonPriR_Click-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonPriR_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonCont1R_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont1R.Click
        Try
            ButtonCancel.Enabled = True
            Me.Equalize(Me.BeneficiariesRetired, "C1", "R")
        Catch sqlEx As SqlException
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonCont1R_Click-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonCont1R_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonCont2R_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont2R.Click
        Try
            Me.Equalize(Me.BeneficiariesRetired, "C1", "R")
            ButtonCancel.Enabled = True
        Catch sqlEx As SqlException
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonCont2R_Click-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonCont2R_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonCont3R_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont3R.Click
        Try
            Me.Equalize(Me.BeneficiariesRetired, "C3", "R")
            ButtonCancel.Enabled = True
        Catch sqlEx As SqlException
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonCont3R_Click-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonCont3R_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonAddRetired_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddRetired.Click
        Try
            Dim _icounter As Integer
            Dim strWSMessage As String
            'AA:2013.08.28 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
            strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersID"))
            If strWSMessage <> "NoPending" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Invalid Beneficiary Operation", "openDialog('" + strWSMessage + "','Bene');", True)
                Exit Sub
            End If
            'End, AA:2013.08.28 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            'START : Dharmesh : 11/20/2018 : YRS-AT-4135 Added validation for participant who enrolled on or after 2019 
            'Adding Annuity selected should be C type annuity, so other than C type annuity will not allow to add beneficiary.
            Dim dictStringParam As Dictionary(Of String, String)
            Dim dsAnnuityDetails As DataSet = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAnnuities(Me.guiFundEventId)

            'Adding new annuity row to check the insured reserve exists for validating in add / edit beneficiary
            dsAnnuityDetails = AddSelectedAnnuityTypeToDataSet(dsAnnuityDetails)

            If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Me.guiFundEventId) _
            AndAlso Not (YMCARET.YmcaBusinessObject.RetirementBOClass.HasInsuredReserveAnnuity(dsAnnuityDetails)) Then
                Dim dtConfigurationKeyValueForEnrolledAfter2019 As DateTime = YMCARET.YmcaBusinessObject.ConfigurationBOClass.RDB_ADB_2019PlanChangeCutOffDate
                dictStringParam = New Dictionary(Of String, String)
                dictStringParam.Add("CutOffDate", dtConfigurationKeyValueForEnrolledAfter2019.ToString("MMMM d, yyyy"))
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019, dictStringParam).DisplayText, EnumMessageTypes.Error, Nothing)
                Exit Sub
            End If
            Session("AnnuityDetails") = dsAnnuityDetails
            'END : Dharmesh : 11/20/2018 : YRS-AT-4135 Added validation for participant who enrolled on or after 2019 
            ButtonCancel.Enabled = True
            _icounter = Me.iCounter
            _icounter = _icounter + 1
            Me.iCounter = _icounter
            If _icounter = 1 Then
                Me.Flag = "AddBeneficiaries"
                ' // START : SB | 21/07/2016 | YRS-AT-2382 | Sending new id as querystring  
                'Dim msg1 As String = "<script language='javascript'>" & _
                '"window.open('AddBeneficiary.aspx','CustomPopUp_BeneficiariesRetired', " & _
                '"'width=750, height=650, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                ' "</script>"
                'Start - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750
                Dim msg1 As String = "<script language='javascript'>" & _
                "window.open('AddBeneficiary.aspx?Page=retireprocess','CustomPopUp_BeneficiariesRetired', " & _
                "'width=850, height=650, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                 "</script>"
                'End - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750
                ' // END : SB | 21/07/2016 | YRS-AT-2382 | Sending new id as querystring 
                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", msg1)
                End If
            Else
                Me.DataGridRetiredBeneficiaries.DataSource = Me.BeneficiariesRetired
                Me.DataGridRetiredBeneficiaries.DataBind()
                _icounter = 0
                Me.iCounter = _icounter
            End If

        Catch sqlEx As SqlException
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonAddRetired_Click-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonAddRetired_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonDeleteRetired_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDeleteRetired.Click
        Try

            ButtonCancel.Enabled = True
            'Me.Flag = "DeleteBeneficiaries"
            If Me.DataGridRetiredBeneficiaries.SelectedIndex <> -1 Then
                Me.Flag = "DeleteBeneficiaries"
                Dim Beneficiaries As DataSet
                Dim drRows As DataRow()
                Dim drUpdated As DataRow
                Dim strWSMessage As String
                'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
                'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
                strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersID"))
                If strWSMessage <> "NoPending" Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Invalid Beneficiary Operation", "openDialog('" + strWSMessage + "','Bene');", True)
                    Exit Sub
                End If
                'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application


                If Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text <> "&nbsp;" Then

                    Beneficiaries = Me.BeneficiariesRetired
                    Dim l_UniqueId As String
                    l_UniqueId = Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text
                    If Not IsNothing(Beneficiaries) Then
                        drRows = Beneficiaries.Tables(0).Select("UniqueID='" & l_UniqueId & "'")
                        drUpdated = drRows(0)
                        drUpdated.Delete()
                        Me.BeneficiariesRetired = Beneficiaries
                    End If
                End If

                If Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text = "&nbsp;" Then
                    Beneficiaries = Me.BeneficiariesRetired
                    If Not IsNothing(Beneficiaries) Then
                        drUpdated = Beneficiaries.Tables(0).Rows(Me.DataGridRetiredBeneficiaries.SelectedIndex)
                        drUpdated.Delete()
                        Me.BeneficiariesRetired = Beneficiaries
                    End If
                End If
                LoadBeneficiariesTab()
                DataGridRetiredBeneficiaries.SelectedIndex = -1  ' Reset the selected index 
                Me.Flag = ""
            Else
                'commented by Anudeep on 22-sep for BT-1126
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a Beneficiary to be deleted.", MessageBoxButtons.Stop)
                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENEFICIARY_DELETE"), enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENEFICIARY_DELETE"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
            End If
        Catch sqlEx As SqlException
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonDeleteRetired_Click-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonDeleteRetired_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub

    Private Sub ButtonEditRetired_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditRetired.Click
        Try
            Dim _icounter As Integer

            ButtonCancel.Enabled = True
            If Me.DataGridRetiredBeneficiaries.SelectedIndex <> -1 Then
                Me.Flag = "EditBeneficiaries"

                'START : Dharmesh : 11/20/2018 : YRS-AT-4135 :  Added validation for participant who enrolled on or after 2019 
                Dim bIsInsureBeneficiary As Boolean = False
                Dim bIsRETIREBeneficiary As Boolean = False
                Dim dictStringParam As Dictionary(Of String, String)
                Dim dtConfigurationKeyValueForEnrolledAfter2019 As DateTime
                Dim dsAnnuityDetails As DataSet = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAnnuities(Me.guiFundEventId)

                'Adding new annuity row to check the insured reserve exists for validating in add / edit beneficiary
                dsAnnuityDetails = AddSelectedAnnuityTypeToDataSet(dsAnnuityDetails)

                Dim dsBeneficiaries As DataSet = DirectCast(Session("BeneficiariesRetired"), DataSet)
                Dim drRows() As DataRow
                If (DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text = "&nbsp;" Or DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text = "") And (DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text <> "&nbsp;" Or DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text <> "") Then
                    drRows = dsBeneficiaries.Tables(0).Select("NewId='" & DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text & "'")
                Else
                    drRows = dsBeneficiaries.Tables(0).Select("UniqueId='" & DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text & "'")
                End If
                If drRows.Length > 0 Then
                    If drRows(0).Item(14) = "INSRES" Then
                        bIsInsureBeneficiary = True
                    ElseIf drRows(0).Item(14) = "RETIRE" Then
                        bIsRETIREBeneficiary = True
                    End If
                End If
                'Check participant is enrolled on or after 1/1/2019 and not have c annuity and display error message "1007" with display msg : "This person has beneficiary type ‘INSRES’ but does not have Insured Reserve Annuity. Please delete the beneficiary (ies)."
                'Adding Annuity selected should be C type annuity, so other than C type annuity will not allow to add beneficiary.
                If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) _
                    AndAlso Not (YMCARET.YmcaBusinessObject.RetirementBOClass.HasInsuredReserveAnnuity(dsAnnuityDetails)) _
                    AndAlso bIsInsureBeneficiary = True Then
                    HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019_IN_EDIT_BENEFICIARY_FOR_INSURED_RESERVES).DisplayText, EnumMessageTypes.Error, Nothing)
                    Exit Sub
                End If
                'Check participant is enrolled on or after 1/1/2019 and not have c annuity and display error message "1008" with display msg : "This person has beneficiary type ''RETIRE'' which is not allowed as this Participant was first enrolled on or after $$CutOffDate$$.  Please delete them."
                If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) _
                    AndAlso Not (YMCARET.YmcaBusinessObject.RetirementBOClass.HasInsuredReserveAnnuity(dsAnnuityDetails)) _
                    AndAlso bIsRETIREBeneficiary = True Then
                    HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019_IN_EDIT_BENEFICIARY_FOR_RETIRE, YMCARET.YmcaBusinessObject.RetirementBOClass.GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText, EnumMessageTypes.Error, Nothing)
                    Exit Sub
                End If
                Session("AnnuityDetails") = dsAnnuityDetails
                'END : Dharmesh : 11/20/2018 : YRS-AT-4135 :  Added validation for participant who enrolled on or after 2019 

                'added by anita on 04 june 2007 because when first name and SSN were entered as blank then while editing the values were populated as &nbsp; in the edit window
                'Start:Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                'If IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(5).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(5).Text.Trim) = "&nbsp;" Then
                '    Session("Name") = ""
                'Else
                '    Session("Name") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(5).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(5).Text.Trim)
                'End If
                ''added by anita on 04 june 2007 because when first name and SSN were entered as blank then while editing the values were populated as &nbsp; in the edit window

                'Session("Name2") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(6).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(6).Text.Trim)
                ''added by anita on 04 june 2007 because when first name and SSN were entered as blank then while editing the values were populated as &nbsp; in the edit window

                'If IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(7).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(7).Text.Trim) = "&nbsp;" Then
                '    Session("TaxID") = ""
                'Else
                '    Session("TaxID") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(7).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(7).Text.Trim)
                'End If
                ''added by anita on 04 june 2007 because when first name and SSN were entered as blank then while editing the values were populated as &nbsp; in the edit window

                'Session("Rel") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(8).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(8).Text.Trim)
                'Session("Birthdate") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(9).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(9).Text.Trim)
                'Session("Groups") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text.Trim)
                'Session("Lvl") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text.Trim)
                'Session("Pct") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(14).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(14).Text.Trim)
                'Session("Type") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(15).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(15).Text.Trim)
                _icounter = Me.iCounter
                _icounter = _icounter + 1
                Session("_icounter") = _icounter
                'Session("Person_Info") = Session("Name2") & "," & Session("Name")
                Session("Person_Info") = SSNo
                Dim popupScript As String
                If (_icounter = 1) Then
                    'If Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(1).Text = "&nbsp;" Then
                    If (DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text = "&nbsp;" Or DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text = "") And (DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text <> "&nbsp;" Or DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text <> "") Then
                        'popupScript = "<script language='javascript'>" & _
                        '"window.open('AddBeneficiary.aspx?Index=" & Me.DataGridRetiredBeneficiaries.SelectedIndex & "', 'CustomPopUp_BeneficiariesRetired', " & _
                        '"'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                        '"</script>"
                        ' // START : SB | 21/07/2016 | YRS-AT-2382 | Sending new parameter to identity from which page it is being called 
                        'popupScript = "<script language='javascript'>" & _
                        '"window.open('AddBeneficiary.aspx?NewID=" & DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text & "', 'CustomPopUp', " & _
                        '"'width=750, height=650, menubar=no, Resizable=no,top=120,left=120, scrollbars=no')" & _
                        '"</script>"
                        'Start - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750
                        popupScript = "<script language='javascript'>" & _
                        "window.open('AddBeneficiary.aspx?NewID=" & DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text & "&Page=retireprocess', 'CustomPopUp', " & _
                        "'width=850, height=650, menubar=no, Resizable=no,top=120,left=120, scrollbars=no')" & _
                        "</script>"
                        ' // END : SB | 21/07/2016 | YRS-AT-2382 | Sending new parameter to identity from which page it is being called 
                    Else
                        popupScript = "<script language='javascript'>" & _
                         "window.open('AddBeneficiary.aspx?UniqueId=" + Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text + "','CustomPopUp_BeneficiariesRetired', " & _
                         "'width=850, height=650, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                         "</script>"
                        'End - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750
                    End If
                    'End:Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", popupScript)

                    End If
                Else

                    _icounter = 0
                    Me.iCounter = _icounter
                End If
            Else
                'commented by Anudeep on 22-sep for BT-1126
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a Beneficiary to be updated.", MessageBoxButtons.OK)
                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENIFICIARY_EDIT"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENIFICIARY_EDIT"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
            End If
        Catch sqlEx As SqlException
            HelperFunctions.LogException("ButtonEditRetired_Click-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            HelperFunctions.LogException("ButtonEditRetired_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    'Adding new annuity row to check the insured reserve exists for validating in add / edit beneficiary
    Private Function AddSelectedAnnuityTypeToDataSet(ByVal dsAnnuity As DataSet) As DataSet
        If dsAnnuity Is Nothing Then
            Return dsAnnuity
        End If
        If ((CheckBoxRetPlan.Checked AndAlso TextBoxAnnuitySelectRet.Text.Trim = "C")) Then
            Dim drAddRow As DataRow
            Dim strNewId As String = Guid.NewGuid().ToString()
            drAddRow = dsAnnuity.Tables(0).NewRow
            drAddRow("Purchase date") = TextBoxRetirementDate.Text
            drAddRow("Annuity Source") = "RETIRE"
            drAddRow("Annuity Type") = TextBoxAnnuitySelectRet.Text
            drAddRow("Current Payment") = "0.00"
            drAddRow("Social Security Adj") = "0.00"
            drAddRow("Death Benefit") = "0.00"
            drAddRow("ID") = strNewId
            drAddRow("PlanType") = "RETIREMENT"
            drAddRow("AnnuityJointSurvivorsID") = strNewId
            drAddRow("bitInsuredReserve") = IsInsuredReserveAnnuity(TextBoxAnnuitySelectRet.Text)
            dsAnnuity.Tables(0).Rows.Add(drAddRow)
        End If
        If ((CheckBoxSavPlan.Checked AndAlso TextBoxAnnuitySelectSav.Text.Trim = "C")) Then
            Dim drAddRow As DataRow
            Dim strNewId As String = Guid.NewGuid().ToString()
            drAddRow = dsAnnuity.Tables(0).NewRow
            drAddRow("Purchase date") = TextBoxRetirementDate.Text
            drAddRow("Annuity Source") = "RETIRE"
            drAddRow("Annuity Type") = TextBoxAnnuitySelectSav.Text
            drAddRow("Current Payment") = "0.00"
            drAddRow("Social Security Adj") = "0.00"
            drAddRow("Death Benefit") = "0.00"
            drAddRow("ID") = strNewId
            drAddRow("PlanType") = "SAVING"
            drAddRow("AnnuityJointSurvivorsID") = strNewId
            drAddRow("bitInsuredReserve") = IsInsuredReserveAnnuity(TextBoxAnnuitySelectSav.Text)
            dsAnnuity.Tables(0).Rows.Add(drAddRow)
        End If
        Return dsAnnuity
    End Function
    Private Function IsInsuredReserveAnnuity(ByVal stAnnuityType As String) As Boolean
        If stAnnuityType = "C" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub ButtonMoveBeneficiaries_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonMoveBeneficiaries.Click
        Try
            Dim benefeciarySelected As Boolean = False
            ' Check if any benefeciary is selected.
            If DataGridActiveBeneficiaries.Items.Count > 0 Then
                For Each di As DataGridItem In DataGridActiveBeneficiaries.Items
                    benefeciarySelected = CType(di.FindControl("CheckboxActBen"), CheckBox).Checked
                    If benefeciarySelected = True Then
                        Exit For
                    End If
                Next
            End If

            ' If benefeciary is not selected do nothing.
            If benefeciarySelected = False Then
                'Added by Anudeep on 22-sep for BT-1126
                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_MOVE_BENIFICIARY"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_MOVE_BENIFICIARY"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            End If
            'START : BD : 2018.11.19 : YRS-AT-4135 
            'For Particpant Enrolled on or after 1/1/2019 
            ' - If annuity Type is Not C Move selected beneficiary is not allowed
            ' - If Annuity Type is C then Move selected beneficiary with Type "INSRES"
            Dim stBeneficiaryTypeCode As String
            Dim dsAnnuityDetails As DataSet = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAnnuities(Me.guiFundEventId)
            'Adding new annuity row to check the insured reserve exists for validating in add / edit beneficiary
            dsAnnuityDetails = AddSelectedAnnuityTypeToDataSet(dsAnnuityDetails)
            If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) Then
                If Not HasInsuredReserveAnnuity(dsAnnuityDetails) Then
                    HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ANNUITY_PURCHASE_DEATH_BENEFIT_NOT_ALLOWED, YMCARET.YmcaBusinessObject.RetirementBOClass.GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText, EnumMessageTypes.Error, Nothing)
                    Exit Sub
                Else
                    stBeneficiaryTypeCode = "INSRES" ' If Annuity type C exists then move beneficiary with type "INSRES"
                End If
            End If
            'END : BD : 2018.11.19 : YRS-AT-4135 

            Dim dsActiveBeneficiaries As DataSet
            Dim dsRetiredBeneficiaries As DataSet
            Dim dsTemp As New DataSet

            dsActiveBeneficiaries = Me.BeneficiariesActive
            dsRetiredBeneficiaries = Me.BeneficiariesRetired

            ' Create Temp table
            dsTemp.Tables.Add("Retired")
            dsTemp.Tables(0).Columns.Add("UniqueId")
            dsTemp.Tables(0).Columns.Add("PersId")
            dsTemp.Tables(0).Columns.Add("BenePersId")
            dsTemp.Tables(0).Columns.Add("BeneFundEventId")
            dsTemp.Tables(0).Columns.Add("Name")
            dsTemp.Tables(0).Columns.Add("Name2")
            dsTemp.Tables(0).Columns.Add("TaxID")
            dsTemp.Tables(0).Columns.Add("Rel")
            dsTemp.Tables(0).Columns.Add("Birthdate")
            dsTemp.Tables(0).Columns.Add("Groups")
            dsTemp.Tables(0).Columns.Add("Lvl")
            dsTemp.Tables(0).Columns.Add("DeathFundEventStatus")
            dsTemp.Tables(0).Columns.Add("BeneficiaryStatusCode")
            dsTemp.Tables(0).Columns.Add("Pct")
            dsTemp.Tables(0).Columns.Add("BeneficiaryTypeCode")
            'Start:Anudeep:28.07.2013:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            dsTemp.Tables(0).Columns.Add("NewID")
            ' Copy all the Retirement benefeciaries into the temp table


            '2014.12.01 BT-2310\YRS 5.0-2255 -Start
            'added new representative column to capture its details
            dsTemp.Tables(0).Columns.Add("RepFirstName")
            dsTemp.Tables(0).Columns.Add("RepLastName")
            dsTemp.Tables(0).Columns.Add("RepSalutation")
            dsTemp.Tables(0).Columns.Add("RepTelephone")
            '2014.12.01 BT-2310\YRS 5.0-2255 -End

            Dim dr As DataRow
            If Not dsRetiredBeneficiaries Is Nothing Then
                If dsRetiredBeneficiaries.Tables(0).Rows.Count > 0 Then
                    For Each drRetBen As DataRow In dsRetiredBeneficiaries.Tables(0).Rows
                        dr = dsTemp.Tables(0).NewRow
                        dr.Item("UniqueId") = drRetBen.Item("UniqueId")
                        dr.Item("PersId") = drRetBen.Item("PersId")
                        dr.Item("BenePersId") = drRetBen.Item("BenePersId")
                        dr.Item("BeneFundEventId") = drRetBen.Item("BeneFundEventId")

                        dr.Item("Name") = drRetBen.Item("Name").ToString().Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                        dr.Item("Name2") = drRetBen.Item("Name2").ToString().Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                        dr.Item("TaxID") = drRetBen.Item("TaxID")
                        dr.Item("Rel") = drRetBen.Item("Rel")
                        dr.Item("Birthdate") = drRetBen.Item("Birthdate")
                        dr.Item("Groups") = drRetBen.Item("Groups")
                        dr.Item("Lvl") = drRetBen.Item("Lvl")
                        dr.Item("DeathFundEventStatus") = drRetBen.Item("DeathFundEventStatus")
                        dr.Item("BeneficiaryStatusCode") = drRetBen.Item("BeneficiaryStatusCode")
                        dr.Item("Pct") = drRetBen.Item("Pct")
                        dr.Item("BeneficiaryTypeCode") = drRetBen.Item("BeneficiaryTypeCode")

                        '2014.12.01 BT-2310\YRS 5.0-2255 -Start
                        'copy representative column details
                        dr.Item("RepFirstName") = drRetBen.Item("RepFirstName")
                        dr.Item("RepLastName") = drRetBen.Item("RepLastName")
                        dr.Item("RepSalutation") = drRetBen.Item("RepSalutation")
                        dr.Item("RepTelephone") = drRetBen.Item("RepTelephone")
                        '2014.12.01 BT-2310\YRS 5.0-2255 -End

                        dr.Item("NewID") = Guid.NewGuid().ToString() 'PPP | 08/01/2016 | YRS-AT-2382 | While moving beneficiaries assign New ID

                        dsTemp.Tables(0).Rows.Add(dr)
                    Next
                    CalculateValues(dsTemp.Tables(0), "R")
                End If
            End If

            ' Get only selected Active Benefeciaries.
            If Not dsActiveBeneficiaries Is Nothing Then
                For Each di As DataGridItem In DataGridActiveBeneficiaries.Items
                    If CType(di.FindControl("CheckboxActBen"), CheckBox).Checked = True Then
                        'Check if it is not already been copied
                        If dsTemp.Tables(0).Select("UniqueId = '" + di.Cells(1).Text + "'").Length <= 0 Then
                            ' Copy the row to the temporary table
                            dr = dsTemp.Tables(0).NewRow
                            'Anudeep:17.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                            'dr.Item("UniqueId") = di.Cells(1).Text
                            dr.Item("PersId") = di.Cells(2).Text
                            dr.Item("BenePersId") = di.Cells(3).Text
                            dr.Item("BeneFundEventId") = di.Cells(4).Text
                            If di.Cells(5).Text = "&nbsp;" Then
                                dr.Item("Name") = String.Empty
                            Else
                                dr.Item("Name") = di.Cells(5).Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                            End If
                            If di.Cells(6).Text = "&nbsp;" Then
                                dr.Item("Name2") = String.Empty
                            Else
                                dr.Item("Name2") = di.Cells(6).Text.Trim ' AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                            End If
                            If di.Cells(7).Text = "&nbsp;" Then
                                dr.Item("TaxID") = String.Empty
                            Else
                                dr.Item("TaxID") = di.Cells(7).Text
                            End If

                            dr.Item("Rel") = di.Cells(8).Text
                            If di.Cells(9).Text = "&nbsp;" Then
                                dr.Item("Birthdate") = String.Empty
                            Else
                                dr.Item("Birthdate") = di.Cells(9).Text
                            End If
                            dr.Item("Groups") = di.Cells(11).Text
                            If di.Cells(12).Text = "&nbsp;" Then
                                dr.Item("Lvl") = String.Empty
                            Else
                                dr.Item("Lvl") = di.Cells(12).Text
                            End If

                            dr.Item("DeathFundEventStatus") = di.Cells(13).Text
                            dr.Item("BeneficiaryTypeCode") = IIf(String.IsNullOrEmpty(stBeneficiaryTypeCode), "RETIRE", stBeneficiaryTypeCode)  'BD : 2018.11.19 : YRS-AT-4135 : Set Beneficiary type code 
                            dr.Item("Pct") = di.Cells(15).Text

                            'Anudeep:17.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                            dr.Item("NewID") = Guid.NewGuid().ToString()

                            '2014.12.01 BT-2310\YRS 5.0-2255 -Start
                            If di.Cells(17).Text = "&nbsp;" Then
                                dr.Item("RepFirstName") = String.Empty
                            Else
                                dr.Item("RepFirstName") = di.Cells(17).Text.Trim
                            End If

                            If di.Cells(18).Text = "&nbsp;" Then
                                dr.Item("RepLastName") = String.Empty
                            Else
                                dr.Item("RepLastName") = di.Cells(18).Text.Trim
                            End If

                            If di.Cells(19).Text = "&nbsp;" Then
                                dr.Item("RepSalutation") = String.Empty
                            Else
                                dr.Item("RepSalutation") = di.Cells(19).Text.Trim
                            End If

                            If di.Cells(20).Text = "&nbsp;" Then
                                dr.Item("RepTelephone") = String.Empty
                            Else
                                dr.Item("RepTelephone") = di.Cells(20).Text.Trim
                            End If
                            '2014.12.01 BT-2310\YRS 5.0-2255 -End

                            dsTemp.Tables(0).Rows.Add(dr)
                        End If
                        ' Remove the selected row from dsActiveBeneficiaries
                        Dim drDel As DataRow()
                        drDel = dsActiveBeneficiaries.Tables(0).Select("UniqueId='" & di.Cells(1).Text & "'")
                        If drDel.Length > 0 Then
                            dsActiveBeneficiaries.Tables(0).Rows.Remove(drDel(0))
                        End If
                    End If
                Next
                CalculateValues(dsTemp.Tables(0), "R")
            End If

            ' Update the tempTable as the new Retired Benefeciaries table
            If Not dsTemp Is Nothing Then
                If dsTemp.Tables(0).Rows.Count > 0 Then
                    Me.Blank = False
                    DataGridRetiredBeneficiaries.Visible = True
                    DataGridRetiredBeneficiaries.DataSource = dsTemp
                    DataGridRetiredBeneficiaries.DataBind()
                    Me.BeneficiariesRetired = dsTemp
                End If
            End If

            ' Refresh the DataGridActiveBeneficiaries grid
            DataGridActiveBeneficiaries.DataSource = dsActiveBeneficiaries
            DataGridActiveBeneficiaries.DataBind()
            Me.BeneficiariesActive = dsActiveBeneficiaries

            ' If all the benefeciaries have been moved then hide ButtonMoveBeneficiaries
            If Me.BeneficiariesActive.Tables(0).Rows.Count <= 0 Then
                ButtonMoveBeneficiaries.Visible = False
            End If
        Catch sqlEx As SqlException
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonMoveBeneficiaries_Click-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonMoveBeneficiaries_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonAddItemNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddItemNotes.Click
        Try
            ButtonCancel.Enabled = True
            Session("Note") = ""
            Session("NotesEntityID") = Session("PersId")  ' SB | 2019.03.15 | BT-12078 | Pers id used when notes are added, earlier Notes cannot be added as NotesEntityID was missing 
            Dim popupScript As String = "<script language='javascript'>" & _
             "window.open('UpdateNotes.aspx', 'CustomPopUp', " & _
             "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
             "</script>"

            Page.RegisterStartupScript("PopupScript2", popupScript)

        Catch sqlEx As SqlException
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonAddItemNotes_Click-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonAddItemNotes_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonView.Click
        'SP 2013.12.13 BT-2326 -Below code has been moved on DataGridNotes selectedindexchanged
        'Try
        '         If Me.DataGridNotes.SelectedIndex <> -1 Then

        '             'Start : Added by dilip yadav : 2009.09.18
        '             Dim l_string_Notes As String
        '             Dim l_datatable_Notes As DataTable
        '             l_datatable_Notes = Session("DisplayNotes")

        '             Dim l_date_notes As String
        '             Dim l_creator_notes As String
        '             Dim dr_notes As DataRow

        '             Dim l_checkbox As New CheckBox
        '             l_date_notes = DataGridNotes.SelectedItem.Cells(1).Text
        '             l_creator_notes = DataGridNotes.SelectedItem.Cells(2).Text
        '             l_checkbox = DataGridNotes.SelectedItem.FindControl("CheckBoxImportant")

        '             Dim index As Integer
        '             index = Me.DataGridNotes.SelectedItem.ItemIndex
        '             If l_datatable_Notes.Rows(index)("Note").GetType.ToString <> "System.DBNull" Then
        '                 Session("Note") = l_datatable_Notes.Rows(index)("Note")
        '             Else
        '                 Session("Note") = ""
        '             End If

        '             If l_checkbox.Checked Then
        '                 Session("BitImportant") = True
        '             Else
        '                 Session("BitImportant") = False
        '             End If
        '             'End : Added by dilip yadav : 2009.09.18
        '             'Below line commented by dilip yadav : 2009.09.18 : At it shows max 50 characters as shown in grid.
        '             'Session("Note") = Me.DataGridNotes.SelectedItem.Cells(6).Text.Trim

        '             Dim popupScript As String
        '             If Me.DataGridNotes.SelectedItem.Cells(1).Text = "&nbsp;" Then
        '                 popupScript = "<script language='javascript'>" & _
        '                            "window.open('UpdateNotes.aspx?Index=" & Me.DataGridNotes.SelectedIndex & "', 'CustomPopUp', " & _
        '                            "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
        '                            "</script>"
        '             Else
        '                 popupScript = "<script language='javascript'>" & _
        '                            "window.open('UpdateNotes.aspx?UniqueID=" & Me.DataGridNotes.SelectedItem.Cells(1).Text & "', 'CustomPopUp', " & _
        '                            "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
        '                            "</script>"

        '             End If
        '             Page.RegisterStartupScript("PopupScript2", popupScript)
        '         Else
        '             'commented by Anudeep on 22-sep for BT-1126
        '             'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select notes to display.", MessageBoxButtons.OK)
        '	'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENEFICIARY_VIEW_NOTES"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
        '	HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENEFICIARY_VIEW_NOTES"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
        '         End If

        '     Catch sqlEx As SqlException
        ''SP 2013.12.13 BT-2326 -Start
        'HelperFunctions.LogException("ButtonView_Click-RetirementProcessing", sqlEx)
        'HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
        ''SP 2013.12.13 BT-2326 -End
        '     Catch ex As Exception
        ''SP 2013.12.13 BT-2326 -Start
        'HelperFunctions.LogException("ButtonView_Click-RetirementProcessing", ex)
        'HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        ''SP 2013.12.13 BT-2326 -End
        '     End Try
    End Sub
    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        ''SP 2013.12.13 BT-2326  -Added Try Catch block
        Try
            If TabStripRetireesInformation.SelectedIndex = 1 Then
                Session("blnAddFedWithHolding") = True

                Session("EnableSaveCancel") = True
                Session("IsFedTaxForMaritalStatus") = True
                Dim popupScript As String = "<script language='javascript'>" & _
                 "window.open('UpdateFedWithHoldingInfo.aspx', 'CustomPopUp', " & _
                 "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                 "</script>"
                Page.RegisterStartupScript("PopupScript2", popupScript)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ButtonAdd_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub ButtonAddGeneralWithholding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddGeneralWithholding.Click
        ''SP 2013.12.13 BT-2326  -Added Try Catch block
        Try
            If TabStripRetireesInformation.SelectedIndex = 2 Then
                Session("blnAddGenWithHolding") = True

                Session("EnableSaveCancel") = True
                Dim popupScript As String = "<script language='javascript'>" & _
                 "window.open('UpdateGenHoldings.aspx', 'CustomPopUp', " & _
                 "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                 "</script>"

                Page.RegisterStartupScript("PopupScript3", popupScript)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ButtonAddGeneralWithholding_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub ButtonUpdateGeneralWithholding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUpdateGeneralWithholding.Click
        'SP 2013.12.13 BT-2326 -Below code has been moved into DataGridGeneralWithholding selectedindexchanged
        '     Try
        '         If Me.DataGridGeneralWithholding.SelectedIndex <> -1 Then
        '             Session("cmbWithHoldingType") = Me.DataGridGeneralWithholding.SelectedItem.Cells(1).Text.Trim
        '             Session("txtAddAmount") = Me.DataGridGeneralWithholding.SelectedItem.Cells(2).Text.Trim
        '             Session("txtStartDate") = Me.DataGridGeneralWithholding.SelectedItem.Cells(3).Text.Trim
        '             Session("txtEndDate") = Me.DataGridGeneralWithholding.SelectedItem.Cells(4).Text.Trim
        '             Dim popupScript As String
        '             If Me.DataGridGeneralWithholding.SelectedItem.Cells(5).Text = "&nbsp;" Then
        '                 popupScript = "<script language='javascript'>" & _
        '                          "window.open('UpdateGenHoldings.aspx?Index=" & Me.DataGridGeneralWithholding.SelectedIndex & "', 'CustomPopUp', " & _
        '                          "'width=750, height=500, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
        '                          "</script>"

        '             Else
        '                 popupScript = "<script language='javascript'>" & _
        '                          "window.open('UpdateGenHoldings.aspx?UniqueID=" & Me.DataGridGeneralWithholding.SelectedItem.Cells(5).Text & "', 'CustomPopUp', " & _
        '                          "'width=750, height=500, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
        '                          "</script>"
        '             End If

        '             Page.RegisterStartupScript("PopupScript2", popupScript)
        '         Else
        '             'commented by Anudeep on 22-sep for BT-1126
        '             'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an General Withholding record to be updated.", MessageBoxButtons.OK)
        '	'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_UPDATE_GENERAL_WITHHOLDING"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
        '	HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_UPDATE_GENERAL_WITHHOLDING"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
        '         End If
        '     Catch ex As Exception
        ''SP 2013.12.13 BT-2326 -Start
        'HelperFunctions.LogException("ButtonUpdateGeneralWithholding_Click-RetirementProcessing", ex)
        'HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        ''SP 2013.12.13 BT-2326 -End
        '     End Try
    End Sub
    Private Sub ButtonUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUpdate.Click
        'SP 2013.12.13 BT-2326 comented this code is used on DataGridFederalWithholding selectedindexchanged 
        '     Try
        '         If Me.DataGridFederalWithholding.SelectedIndex <> -1 Then
        '             Session("cmbTaxEntity") = Me.DataGridFederalWithholding.SelectedItem.Cells(4).Text.Trim
        '             Session("cmbWithHolding") = Me.DataGridFederalWithholding.SelectedItem.Cells(3).Text.Trim
        '             Session("txtExemptions") = Me.DataGridFederalWithholding.SelectedItem.Cells(1).Text.Trim
        '             Session("txtAddlAmount") = Me.DataGridFederalWithholding.SelectedItem.Cells(2).Text.Trim
        '             Session("cmbMaritalStatus") = Me.DataGridFederalWithholding.SelectedItem.Cells(5).Text.Trim
        '             Session("IsFedTaxForMaritalStatus") = True
        '             Dim popupScript As String
        '             If Me.DataGridFederalWithholding.SelectedItem.Cells(6).Text = "&nbsp;" Then
        '                 popupScript = "<script language='javascript'>" & _
        '                            "window.open('UpdateFedWithHoldingInfo.aspx?Index=" & Me.DataGridFederalWithholding.SelectedIndex & "', 'CustomPopUp', " & _
        '                            "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
        '                            "</script>"
        '             Else
        '                 popupScript = "<script language='javascript'>" & _
        '                          "window.open('UpdateFedWithHoldingInfo.aspx?UniqueID=" & Me.DataGridFederalWithholding.SelectedItem.Cells(6).Text & "', 'CustomPopUp', " & _
        '                          "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
        '                          "</script>"
        '             End If



        '             Page.RegisterStartupScript("PopupScript2", popupScript)
        '         Else
        '             'commented by Anudeep on 22-sep for BT-1126
        '             'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an Federal Withholding record to be updated.", MessageBoxButtons.OK)

        '	'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_UPDATE_FEDERAL_WITHHOLDING"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
        '	HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_UPDATE_FEDERAL_WITHHOLDING"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
        '         End If
        '     Catch ex As Exception
        ''SP 2013.12.13 BT-2326 -Start
        'HelperFunctions.LogException("ButtonUpdate_Click-RetirementProcessing", ex)
        'HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        ''SP 2013.12.13 BT-2326 -End
        '     End Try
    End Sub
    Private Sub ButtonPurchase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPurchase.Click
        Dim alertWindow As String
        Dim ycRetireeRetirementDate As String
        Dim strFundEventId As String
        'Start:AA:03.14.2016 YRS-AT-2599 commented to show message when validation occurs
        'Dim l_string_Message As String = String.Empty
        'Dim isValid As Boolean = True 
        'End:AA:03.14.2016 YRS-AT-2599 commented to show message when validation occurs
        Dim l_integer_Count As Integer

        'Dim terminationDate As String
        Dim errorMessage As String
        'Dim l_MessageBoxButtons_Type As MessageBoxButtons 'AA:03.14.2016 YRS-AT-2599 commented to show message when validation occurs
        Dim strConfirmationMessage As String 'AA:03.14.2016 YRS-AT-2599 Added to give only confirmation message only once

        Try
            Session("MessageBox") = Nothing 'Anudeep:2014.06.03 BT:2556- Clearing session variable for calling message box
            '----SS:17 feb 2011: for YRS 5.0-1236---If Participants account is locked then restrict to process the annuity purchase.
            Dim dsPersRetiree As DataSet
            dsPersRetiree = Me.PersonDetails
            Dim l_SSN As String
            l_SSN = String.Empty

            If Not dsPersRetiree.Tables(0).Rows(0).Item("chrSSNo").ToString() Is DBNull.Value Then
                If Not dsPersRetiree.Tables(0).Rows(0).Item("chrSSNo").ToString() = String.Empty Then
                    l_SSN = dsPersRetiree.Tables(0).Rows(0).Item("chrSSNo").ToString().Trim()
                End If
            End If


            If Not dsPersRetiree.Tables(0).Rows(0).Item("bitAcctLocked").ToString() Is DBNull.Value Then
                If Not dsPersRetiree.Tables(0).Rows(0).Item("bitAcctLocked").ToString() = String.Empty Then
                    If dsPersRetiree.Tables(0).Rows(0).Item("bitAcctLocked") = True Then
                        Dim l_dsLockResDetails As DataSet
                        Dim l_reasonLock As String
                        If Not l_SSN = String.Empty Then
                            l_dsLockResDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetLockReasonDetails(l_SSN.ToString().Trim)
                        End If
                        Session("splitfundSSN") = Nothing
                        If Not l_dsLockResDetails Is Nothing Then
                            If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows.Count > 0 Then
                                If (l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "System.DBNull" And l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "") Then
                                    l_reasonLock = l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString().Trim
                                End If
                            End If
                        End If
                        If l_reasonLock = "" Then
                            'commented by Anudeep on 22-sep for BT-1126
                            'MessageBox.Show(PlaceHolder1, " YMCA - YRS", "Participant account is locked. Please refer to Customer Service Supervisor.", MessageBoxButtons.Stop, False)
                            'MessageBox.Show(PlaceHolder1, " YMCA - YRS", getmessage("MESSAGE_RETIREMENT_PROCESSING_PARTICIPANT_ACCOUNT_LOCKED"), MessageBoxButtons.Stop, False)
                            HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PARTICIPANT_ACCOUNT_LOCKED"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                        Else
                            'commented by Anudeep on 22-sep for BT-1126
                            'MessageBox.Show(PlaceHolder1, " YMCA - YRS", "Participant account is locked due to " + l_reasonLock + "." + " Please refer to Customer Service Supervisor.""), , MessageBoxButtons.Stop, False)
                            'MessageBox.Show(PlaceHolder1, " YMCA - YRS", String.Format(getmessage("MESSAGE_RETIREMENT_PROCESSING_PARTICIPANT_ACCOUNT_LOCKED_WITH_REASON"), l_reasonLock), MessageBoxButtons.Stop, False)
                            HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PARTICIPANT_ACCOUNT_LOCKED_WITH_REASON"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                        End If
                        Exit Sub
                    End If
                End If
            End If

            '-----------------------------------------







            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            End If
            'End : YRS 5.0-940

            If Me.DataChanged = True Then
                'commented by Anudeep on 22-sep for BT-1126
                'MessageBox.Show(PlaceHolder1, "Error", "The data has been changed. Please Re-Calculate before Purchasing", MessageBoxButtons.OK, True)
                'ShowCustomMessage("The data has been changed. Please Re-Calculate before Purchasing"), enumMessageBoxType.DotNet, MessageBoxButtons.OK, "Error")
                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_PARTICIPANT_DATA_HAS_CHANGED"), enumMessageBoxType.DotNet, MessageBoxButtons.OK, "Error")
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PARTICIPANT_DATA_HAS_CHANGED"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            End If

            strFundEventId = Me.guiFundEventId

            Call SetRetirementDate()
            ycRetireeRetirementDate = Me.RetirementDate

            'terminationDate = Session("TerminationDate")

            'Call the BO retirement validate method
            ' Get the fundstatus value for validation check
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            'If Not RetirementBOClass.IsRetirementValid(errorMessage, False, strFundEventId, ycRetireeRetirementDate, Me.RetireType, Me.PersId, Me.SSNo, terminationDate, Me.FundEventStatus, GetPlanType()) Then
            If Not RetirementBOClass.IsRetirementValid(errorMessage, False, strFundEventId, ycRetireeRetirementDate, Me.RetireType, Me.PersId, Me.SSNo, Me.FundEventStatus, GetPlanType()) Then
                'alertWindow = "<script language='javascript'>" & _
                '    "alert('" & errorMessage & "');" & _
                '    "</script>"
                'If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                '    Page.RegisterStartupScript("alertWindow", alertWindow)
                'End If
                errorMessage = combinemessage(errorMessage)
                Session("BasicValidationMessage") = errorMessage
                'Call ShowCustomMessage(errorMessage, enumMessageBoxType.DotNet)
                'HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            End If

            'NP:IVP1:2008.05.15 - BT-440 - Check whether Beneficiaries are defined properly or not.
            If ValidateBenefeciaryDetails() = False Then
                'The call to ValidateBeneficiaryDetails shows any error messages if applicable and returns false.
                'We simply need to jump out of this procedure.
                Exit Sub
            End If
            'END 2008.05.15

            'added by Hafiz on 10-Sep-2008 for Phase IV - Part 2
            errorMessage = ValidateLastPayrollDate()
            If errorMessage <> "" Then
                'Start: AA:03.14.2016 YRS-AT-2599 Added below code to give only confirmation message for pt and rpt
                If Me.FundEventStatus = "PT" Or Me.FundEventStatus = "RPT" Then
                    strConfirmationMessage = errorMessage
                Else
                    'End: AA:03.14.2016 YRS-AT-2599 Added below code to give only confirmation message for pt and rpt
                    'Call ShowCustomMessage(errorMessage, enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                    'SP 2013.12.19 - BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process   (commented to display page load event)
                    'HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                    Exit Sub
                End If
            End If

            'START : BD : 2018.11.19 : YRS-AT-4135 
            'For Particpant Enrolled on or after 1/1/2019
            ' - If Annuity is C, and participant have Retiree beneficiary do not allow purchase.
            Dim dsAnnuityDetails As DataSet = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAnnuities(Me.guiFundEventId)
            'Adding new annuity row to check the insured reserve exists for validating in add / edit beneficiary
            dsAnnuityDetails = AddSelectedAnnuityTypeToDataSet(dsAnnuityDetails)
            If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) Then
                If HasInsuredReserveAnnuity(dsAnnuityDetails) Then
                    If (HelperFunctions.isNonEmpty(Me.BeneficiariesRetired)) Then
                        If Me.BeneficiariesRetired.Tables(0).Select("BeneficiaryTypeCode = 'RETIRE'").Length > 0 Then
                            HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ANNUITY_PURCHASE_RETIREE_NOT_ALLOWED, YMCARET.YmcaBusinessObject.RetirementBOClass.GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText, EnumMessageTypes.Error, Nothing)
                            Exit Sub
                        End If
                    End If
                End If
            End If
            'END : BD : 2018.11.19 : YRS-AT-4135 
            ' START : SB | 03/08/2017 | YRS-AT-2625 | Checking if any withdrawals are there and doesnot satify paid services condition then exit function and do not allow to purchase annutiy 
            If (Me.RetireType = "DISABL") Then
                Dim hasSatisfiedPaidServiceAfterWithdrawal As Boolean = False
                Dim isPaidService As Boolean, strPlanType As String
                Dim isWithdrawn As Boolean = False

                strPlanType = Me.GetPlanType()
                Dim withdrawalDetails As DataSet

                If strPlanType Is Nothing Or strPlanType = "B" Or strPlanType = "R" Then
                    withdrawalDetails = RetirementBOClass.HasBasicMoneyWithdrawn(Me.guiFundEventId, Me.RetirementDate)
                    If (HelperFunctions.isNonEmpty(withdrawalDetails)) Then
                        hasSatisfiedPaidServiceAfterWithdrawal = Convert.ToBoolean(withdrawalDetails.Tables(0).Rows(0).Item("SatisfiedEligiblePaidService").ToString())
                        isWithdrawn = Convert.ToBoolean(withdrawalDetails.Tables(0).Rows(0).Item("IsWithdrawalExists").ToString())
                        isPaidService = hasSatisfiedPaidServiceAfterWithdrawal 'SR | 2017.04.07 | YRS-AT-3390 | Get Paid service.
                    End If

                    If (Not isWithdrawn) Then
                        'isPaidService = RetirementBOClass.IsPaidServiceValid(strPlanType, Me.guiFundEventId, False, Me.RetirementDate) // SR | 2017.04.07 | YRS-AT-3390 | commented old method to calculate Paid service based on existing transacts.
                        If (Not isPaidService) Then
                            HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCCESING_PURCHASE_PARTICIPANT_NO_60_MONTHS_PAID_SERVICE"), EnumMessageTypes.Error, Nothing)  'SP 2013.12.13 BT:2326
                            Exit Sub
                        End If
                    ElseIf (isWithdrawn And Not hasSatisfiedPaidServiceAfterWithdrawal) Then
                        HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_PARTICIPANT_WITHDRAWL_FUNDS"), EnumMessageTypes.Error, Nothing)
                        Exit Sub
                    End If
                End If
            End If
            ' END : SB | 03/08/2017 | YRS-AT-2625 | Checking if any withdrawals are there and doesnot satify paid services condition then exit function and do not allow to purchase annuity

            If CheckBoxRetPlan.Checked = True Then
                If TextBoxAnnuitySelectRet.Text.Trim.ToString() = "" Then
                    'commented by Anudeep on 22-sep for BT-1126
                    'l_string_Message = "Please select an annuity to purchase"
                    'Start:AA:03.16.2016 Changed to show message in error top of page
                    'l_string_Message = getmessage("MESSAGE_RETIREMENT_PROCESING_SELECT_ANNUITY")
                    'l_MessageBoxButtons_Type = MessageBoxButtons.OK
                    'isValid = False
                    HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESING_SELECT_ANNUITY"), EnumMessageTypes.Error, Nothing)
                    Exit Sub
                    'End:AA:03.16.2016 Changed to show message in error top of page
                End If
            ElseIf CheckBoxSavPlan.Checked = True Then
                If TextBoxAnnuitySelectSav.Text.Trim.ToString() = "" Then
                    'commented by Anudeep on 22-sep for BT-1126
                    'l_string_Message = "Please select an annuity to purchase"
                    'Start:AA:03.16.2016 Changed to show message in error top of page
                    'l_string_Message = getmessage("MESSAGE_RETIREMENT_PROCESING_SELECT_ANNUITY")
                    'l_MessageBoxButtons_Type = MessageBoxButtons.OK
                    'isValid = False
                    HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESING_SELECT_ANNUITY"), EnumMessageTypes.Error, Nothing)
                    Exit Sub
                    'End:AA:03.16.2016 Changed to show message in error top of page
                End If
            End If

            ' Check if the retirement date is Pre 2008
            ' If it is then check that both the plans are used to buy the same type of annuity
            If IsPrePlanSplitRetirement = True Then
                If TextBoxTotalPaymentRet.Text.Trim.ToString() <> "" And TextBoxTotalPaymentSav.Text.Trim.ToString() <> "" Then
                    If TextBoxAnnuitySelectRet.Text.Trim.ToString() <> TextBoxAnnuitySelectSav.Text.Trim.ToString() Then
                        'commented by Anudeep on 22-sep for BT-1126
                        'l_string_Message = "Please select similar type of annuity for both plans"
                        'Start:AA:03.16.2016 Changed to show message in error top of page
                        'l_string_Message = getmessage("MESSAGE_RETIREMENT_PROCESSING_SELECT_SIMILAR_ANNUITY")
                        'l_MessageBoxButtons_Type = MessageBoxButtons.OK
                        'isValid = False
                        HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_SELECT_SIMILAR_ANNUITY"), EnumMessageTypes.Error, Nothing)
                        Exit Sub
                    End If
                End If
            End If

            '''''''''''''''''''''''''''''''''
            If Me.isTaxWithheldMoreThanTaxableAmount = True Then
                'ShowCustomMessage("Tax withholding cannot exceed taxable amount of annuity. Please review federal withholding information and modify if necessary."), enumMessageBoxType.DotNet)
                'commented by Anudeep on 22-sep for BT-1126
                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_TAXWITHHOLDING_EXCEEDS"), enumMessageBoxType.DotNet)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_TAXWITHHOLDING_EXCEEDS"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            End If
            '''''''''''''''''''''''''''''''''
            If Me.isAnnuityAmountZero = True Then
                'commented by Anudeep on 22-sep for BT-1126
                'ShowCustomMessage("Can not buy zero annuity"), enumMessageBoxType.DotNet)
                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_ZERO_ANNUITY"), enumMessageBoxType.DotNet)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_ZERO_ANNUITY"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            End If
            '''''''''''''''''''''''''''''''''
            'START: PK | 10/04/2019 | YRS-AT-4597 | YRS enh: State Withholding Project - First Annuity Payments (UI design)
            If Me.isStateTaxWithheldMoreThanTaxableAmount = True Then
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_STATEWITHHOLDINGAMOUNT).DisplayText, EnumMessageTypes.Error)
                Exit Sub
            End If
            If Me.ValidateSTWvsFedtaxforMA = False Then
                Exit Sub
            End If

            'END: PK | 10/04/2019 | YRS-AT-4597 | YRS enh: State Withholding Project - First Annuity Payments (UI design)
            ''''''''''''''''''''''''
            '2012.07.03 SP : BT-712/YRS 5.0-1246 : Handling DLIN records -Start
            If RetirementBOClass.IsExistsDLINRecordBeforeRetirement(Me.guiFundEventId, Me.RetirementDate) = True Then
                'commented by Anudeep on 22-sep for BT-1126
                'ShowCustomMessage("Daily Interest records dated for the purchase date exist.  Please correct before proceeding."), enumMessageBoxType.DotNet)
                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_DAILY_INTEREST"), enumMessageBoxType.DotNet)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_DAILY_INTEREST"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            End If
            '''''''''''''''''''''''
            '2012.07.03 SP : BT-712/YRS 5.0-1246 : Handling DLIN records -End

            Dim strSSNValidationCode As String = String.Empty
            Dim bIsValidPhonySSN As Boolean 'Manthan Rajguru | 2016.07.28 | YRS-AT-2560 | Declaring Variable           
            Dim blnIsSelectedSSNMatchesTextBoxSSN As Boolean = False 'Manthan Rajguru | 2016.09.06 | YRS-AT-2560 | Declared boolean variable
            If CheckBoxRetPlan.Checked = True And Left(TextBoxAnnuitySelectRet.Text.Trim.ToString(), 1).ToUpper() = "J" Then '2012.05.08 SP :
                If Me.TextBoxAnnuitySSNoRet.Visible = True Then
                    'START: Manthan Rajguru | 2016.09.06 | YRS-AT-2560 | Added to validate existing phonySSN in system only if user has manually entered other than participants active member beneficiaries SSNo 
                    If DataGridAnnuityBeneficiaries.Items.Count > 0 Then
                        For counter As Integer = 0 To DataGridAnnuityBeneficiaries.Items.Count - 1
                            If TextBoxAnnuitySSNoRet.Text.Trim() = DataGridAnnuityBeneficiaries.Items(counter).Cells(BENEFICIARY_SSNo).Text.Trim Then
                                blnIsSelectedSSNMatchesTextBoxSSN = True
                                Exit For
                            End If
                        Next
                    End If
                    If Not blnIsSelectedSSNMatchesTextBoxSSN Then
                        'Start - Manthan Rajguru | 2016.07.28 | YRS-AT-2560 | Validating Existing phony SSN and showing error message
                        bIsValidPhonySSN = Validation.IsValidPhonySSN(Me.TextBoxAnnuitySSNoRet.Text().Trim(), "INSERT", "", "")
                        If Not bIsValidPhonySSN Then
                            'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                            'HelperFunctions.ShowMessageToUser("Phony SSN already exists", EnumMessageTypes.Error)
                            HelperFunctions.ShowMessageToUser("Placeholder SSN already exists", EnumMessageTypes.Error)
                            'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                            Exit Sub
                        End If
                        'End - Manthan Rajguru | 2016.07.28 | YRS-AT-2560 | Validating Existing phony SSN and showing error message 
                    End If
                    'END: Manthan Rajguru | 2016.09.06 | YRS-AT-2560 | Added to validate existing phonySSN in system only if user has manually entered other than participants active member beneficiaries SSNo 
                    strSSNValidationCode = YMCACommonBOClass.IsValidSSNo(Me.TextBoxAnnuitySSNoRet.Text().Trim())
                    If strSSNValidationCode = "Not_Phony_SSNo" Then
                        'Session("PhonySSNo") = "Not_Phony_SSNo"
                        TabStripRetireesInformation.SelectedIndex = 4
                        MultiPageRetirementProcessing.SelectedIndex = 4
                        'commented by Anudeep on 22-sep for BT-1126
                        'ShowCustomMessage("Invalid SSNo entered, Please enter a Valid SSNo."), enumMessageBoxType.DotNet)
                        'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_INVALIDSSNNO"), enumMessageBoxType.DotNet)
                        HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_INVALIDSSNNO"), EnumMessageTypes.Error, Nothing)   'SP 2013.12.13 BT:2326
                        Exit Sub
                    ElseIf strSSNValidationCode = "Phony_SSNo" Then
                        'commented by Anudeep on 22-sep for BT-1126
                        'ShowCustomMessage("Are you sure you want to continue with Phony SSNo ?"), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                        'Start: AA:03.14.2016 YRS-AT-2599 Added below code to give only confirmation message only once
                        'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_CONTINUE_PHONY_SSNO"), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                        'Session("PhonySSNo") = "Phony_SSNo"
                        If Not String.IsNullOrEmpty(strConfirmationMessage) Then
                            strConfirmationMessage += "##" + String.Format(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_CONTINUE_PHONY_SSNO"), "Retirement")
                        Else
                            strConfirmationMessage = String.Format(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_CONTINUE_PHONY_SSNO"), "Retirement")
                        End If
                        'End: AA:03.14.2016 YRS-AT-2599 Added below code to give only confirmation message only once
                        'Exit Sub
                    ElseIf strSSNValidationCode = "No_Configuration_Key" Then
                        'commented by Anudeep on 22-sep for BT-1126
                        'Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
                        'START: MMR | 05/03/2018 | YRS-AT-3941 | Replaced incorrect spelling in key from "RETIRIEMENT" to "RETIREMENT"
                        'Throw New Exception(getmessage("MESSAGE_RETIRIEMENT_PROCESSING_NO_KEY_PHONYSSNO"))
                        Throw New Exception(getmessage("MESSAGE_RETIREMENT_PROCESSING_NO_KEY_PHONYSSNO"))
                        'END: MMR | 05/03/2018 | YRS-AT-3941 | Replaced incorrect spelling in key from "RETIRIEMENT" to "RETIREMENT"
                    End If
                End If
            End If
            strSSNValidationCode = String.Empty
            blnIsSelectedSSNMatchesTextBoxSSN = False 'Manthan Rajguru | 2016.09.06 | YRS-AT-2560 | Reseting boolean value
            If CheckBoxSavPlan.Checked = True And Left(TextBoxAnnuitySelectSav.Text.Trim.ToString(), 1).ToUpper() = "J" Then '2012.05.08 SP :
                If Me.TextBoxAnnuitySSNoSav.Visible = True Then
                    'START: Manthan Rajguru | 2016.09.06 | YRS-AT-2560 | Added to validate existing phonySSN in system only if user has manually entered other than participants active member beneficiaries SSNo 
                    If DataGridAnnuityBeneficiariesSav.Items.Count > 0 Then
                        For counter As Integer = 0 To DataGridAnnuityBeneficiariesSav.Items.Count - 1
                            If TextBoxAnnuitySSNoSav.Text.Trim() = DataGridAnnuityBeneficiariesSav.Items(counter).Cells(BENEFICIARY_SSNo).Text.Trim Then
                                blnIsSelectedSSNMatchesTextBoxSSN = True
                                Exit For
                            End If
                        Next
                    End If
                    If Not blnIsSelectedSSNMatchesTextBoxSSN Then
                        'Start - Manthan Rajguru | 2016.07.28 | YRS-AT-2560 | Validating Existing phony SSN and showing error message
                        bIsValidPhonySSN = Validation.IsValidPhonySSN(Me.TextBoxAnnuitySSNoSav.Text().Trim(), "INSERT", "", "")
                        If Not bIsValidPhonySSN Then
                            'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                            'HelperFunctions.ShowMessageToUser("Phony SSN already exists", EnumMessageTypes.Error)
                            HelperFunctions.ShowMessageToUser("Placeholder SSN already exists", EnumMessageTypes.Error)
                            'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                            Exit Sub 'Start - Added code to stop purchase annuity process if not valid SSN
                        End If
                        'End - Manthan Rajguru | 2016.07.28 | YRS-AT-2560 | Validating Existing phony SSN and showing error message
                    End If
                    'END: Manthan Rajguru | 2016.09.06 | YRS-AT-2560 | Added to validate existing phonySSN in system only if user has manually entered other than participants active member beneficiaries SSNo 
                    strSSNValidationCode = YMCACommonBOClass.IsValidSSNo(Me.TextBoxAnnuitySSNoSav.Text().Trim())
                    If strSSNValidationCode = "Not_Phony_SSNo" Then
                        'Session("PhonySSNo") = "Not_Phony_SSNo"
                        TabStripRetireesInformation.SelectedIndex = 4
                        MultiPageRetirementProcessing.SelectedIndex = 4
                        'commented by Anudeep on 22-sep for BT-1126
                        'ShowCustomMessage("Invalid SSNo entered, Please enter a Valid SSNo."), enumMessageBoxType.DotNet)
                        'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_INVALIDSSNNO"), enumMessageBoxType.DotNet)
                        HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_INVALIDSSNNO"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                        Exit Sub
                    ElseIf strSSNValidationCode = "Phony_SSNo" Then
                        'commented by Anudeep on 22-sep for BT-1126
                        'ShowCustomMessage("Are you sure you want to continue with Phony SSNo ?""), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                        'Start: AA:03.14.2016 YRS-AT-2599 Added below code to give only confirmation message only once
                        'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_CONTINUE_PHONY_SSNO"), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                        'Session("PhonySSNo") = "Phony_SSNo"
                        'Exit Sub
                        If Not String.IsNullOrEmpty(strConfirmationMessage) Then
                            strConfirmationMessage += "##" + String.Format(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_CONTINUE_PHONY_SSNO"), "Savings")
                        Else
                            strConfirmationMessage = String.Format(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_CONTINUE_PHONY_SSNO"), "Savings")
                        End If
                        'End: AA:03.14.2016 YRS-AT-2599 Added below code to give only confirmation message only once
                    ElseIf strSSNValidationCode = "No_Configuration_Key" Then
                        'commented by Anudeep on 22-sep for BT-1126
                        'Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
                        'START: MMR | 05/03/2018 | YRS-AT-3941 | Replaced incorrect spelling in key from "RETIRIEMENT" to "RETIREMENT"
                        'Throw New Exception(getmessage("MESSAGE_RETIRIEMENT_PROCESSING_NO_KEY_PHONYSSNO"))
                        Throw New Exception(getmessage("MESSAGE_RETIREMENT_PROCESSING_NO_KEY_PHONYSSNO"))
                        'END: MMR | 05/03/2018 | YRS-AT-3941 | Replaced incorrect spelling in key from "RETIRIEMENT" to "RETIREMENT"
                    End If
                End If
            End If

            'Start:AA:03.14.2016 YRS-AT-2599 commented below code to give only confirmation message only once
            'If isValid Then
            'commented by Anudeep on 22-sep for BT-1126
            'l_string_Message = "Do you wish to purchase this annuity ?"

            'l_string_Message = getmessage("MESSAGE_RETIREMENT_PROCESING_HANDLE_MESSAGEBOX")
            'l_MessageBoxButtons_Type = MessageBoxButtons.YesNo
            'End:AA:03.14.2016 YRS-AT-2599 commented below code to give only confirmation message only once

            l_integer_Count = YMCARET.YmcaBusinessObject.RefundRequest.GetLoanStatus(Me.PersId)
            If l_integer_Count > 0 Then
                'Start: AA:03.14.2016 YRS-AT-2599 changed below code to give only confirmation message only once
                'l_string_Message = "Paid Loan Exists, " + l_string_Message
                If Not String.IsNullOrEmpty(strConfirmationMessage) Then
                    strConfirmationMessage += "##" + getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_PAID_LOAN")
                Else
                    strConfirmationMessage = getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_PAID_LOAN")
                End If
            End If
            'End If
            'End: AA:03.14.2016 YRS-AT-2599 Added below code to give only confirmation message only once

            'START: SB: YRS-AT-2625 - commented the code as warning needs to be converted to a error/stop message.
            'YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero
            'If (Me.RetireType = "DISABL") Then
            '    Dim isWithdrawn As Boolean
            '    Dim isPaidService As Boolean, strPlanType As String
            '    strPlanType = Me.GetPlanType()

            '    Dim withdrawalDetails As DataSet
            '    If strPlanType Is Nothing Or strPlanType = "B" Or strPlanType = "R" Then

            '        ' START : SB | 03/08/2017 | YRS-AT-2625 | Adding additional parameter required to get the paid services 
            '        '  isWithdrawn = RetirementBOClass.HasBasicMoneyWithdrawn(Me.guiFundEventId) 
            '        withdrawalDetails = RetirementBOClass.HasBasicMoneyWithdrawn(Me.guiFundEventId, Me.RetirementDate)
            '        If (HelperFunctions.isNonEmpty(withdrawalDetails)) Then
            '            isWithdrawn = Convert.ToBoolean(withdrawalDetails.Tables(0).Rows(0).Item("IsWithdrawalExists").ToString())
            '        End If
            '        ' END : SB | 03/08/2017 | YRS-AT-2625 | Adding additional parameter required to get the paid services 

            '        If (isWithdrawn) Then
            '            'Start: AA:03.14.2016 YRS-AT-2599 changed below code to give only confirmation message only once
            '            'Session("MessageBox") = "PaidServiceCheck"
            '            'commented by Anudeep on 22-sep for BT-1126
            '            'ShowCustomMessage("Participant has withdrawn funds from their basic accounts. Does the participant have the required 60 months of paid service since the withdrawal.""), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
            '            'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_PARTICIPANT_WITHDRAWL_FUNDS"), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
            '            If Not String.IsNullOrEmpty(strConfirmationMessage) Then
            '                strConfirmationMessage += "##" + getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_PARTICIPANT_WITHDRAWL_FUNDS")
            '            Else
            '                strConfirmationMessage = getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_PARTICIPANT_WITHDRAWL_FUNDS")
            '            End If
            '            'End: AA:03.14.2016 YRS-AT-2599 changed below code to give only confirmation message only once
            '            'Exit Sub
            '        Else
            '            isPaidService = RetirementBOClass.IsPaidServiceValid(strPlanType, Me.guiFundEventId, False, Me.RetirementDate)
            '            If (Not isPaidService) Then
            '                'commented by Anudeep on 22-sep for BT-1126
            '                'ShowCustomMessage("Participant does not have the required 60 months of paid service.  Disability process cannot proceed.""), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
            '                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCCESING_PURCHASE_PARTICIPANT_NO_60_MONTHS_PAID_SERVICE"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
            '                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCCESING_PURCHASE_PARTICIPANT_NO_60_MONTHS_PAID_SERVICE"), EnumMessageTypes.Error, Nothing)  'SP 2013.12.13 BT:2326

            '                Exit Sub
            '            End If
            '        End If
            '    End If
            'End If
            'End YRS 5.0-1035
            'END: SB: YRS-AT-2625 - commented the code as warning needs to be converted to a error/stop message.

            '2012.05.28  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
            If CheckBoxRetPlan.Checked = True And Left(TextBoxAnnuitySelectRet.Text.Trim.ToString(), 1).ToUpper() = "J" Then

                EnableDisableValidationBeneficiary(True)
            Else
                EnableDisableValidationBeneficiary(False)
            End If
            If CheckBoxSavPlan.Checked = True And Left(TextBoxAnnuitySelectSav.Text.Trim.ToString(), 1).ToUpper() = "J" Then

                EnableDisableValidationBeneficiarySav(True)
            Else
                EnableDisableValidationBeneficiarySav(False)
            End If

            Page.Validate()
            If Page.IsValid Then
                UpdateSelectedParticiapntBeneficary()
                UpdateSelectedParticiapntBeneficarySav()
            End If
            '2012.05.28  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End

            'SANKET:03/24/2011 code for YRS 5.0-1294 
            Session("MessageBox") = "Purchase"

            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_Message, MessageBoxButtons.YesNo)

            'SP 2013.12.13 BT:2326 -adding if else
            'Start: AA:03.14.2016 YRS-AT-2599 changed below code to give only confirmation message only once
            'If (IsValid) Then
            'ShowCustomMessage(l_string_Message, enumMessageBoxType.DotNet, l_MessageBoxButtons_Type)
            If Not String.IsNullOrEmpty(strConfirmationMessage) Then
                strConfirmationMessage += "##" + getmessage("MESSAGE_RETIREMENT_PROCESING_HANDLE_MESSAGEBOX") 'Header title of dialog box
                strConfirmationMessage += "##" + getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_CONFIRM_POINTS") 'Confirmation message
            Else
                'Start: palani:03.22.2016 YRS-AT-2599 Commented existng code and changed confirmation message
                'strConfirmationMessage = getmessage("MESSAGE_RETIREMENT_PROCESING_HANDLE_MESSAGEBOX") 'Confirmation message
                strConfirmationMessage = getmessage("MESSAGE_RETIREMENT_PROCESING_CONFIRM_MESSAGEBOX") 'Confirmation message
                'End: palani:03.22.2016 YRS-AT-2599 Commented existng code and changed confirmation message
            End If
            ShowCustomMessage(strConfirmationMessage, enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
            'Else
            'HelperFunctions.ShowMessageToUser(l_string_Message, EnumMessageTypes.Error, Nothing)    'SP 2013.12.13 BT:2326
            'End If
            'End: AA:03.14.2016 YRS-AT-2599 changed below code to give only confirmation message only once


            Exit Sub
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonPurchase_Click-RetirementProcessing", ex)
            'START : MMR | 05/10/2018 | YRS-AT-3941 | Showing custom error message for key not defined instead of sql execption error
            If ex.Message.Contains("yrs_usp_AMCM_SearchConfigurationMaintenance") Then
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_NO_KEY_PHONYSSNO"), EnumMessageTypes.Error, Nothing)
            Else
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            End If
            'END : MMR | 05/10/2018 | YRS-AT-3941 | Showing custom error message for key not defined instead of sql execption error
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            'SANKET:03/24/2011 code for YRS 5.0-1294 
            Session("MessageBox") = "CancelAndExit"
            Session("AnnuityDetails") = Nothing 'Dharmesh : 12/7/2018 : YRS-AT-4135 : Making the session empty for not adding mulitple maunual added record from the Process screen.
            'Session("blnCancelForm") = True
            'commented by Anudeep on 22-sep for BT-1126
            'ShowCustomMessage("Are you sure you want to cancel and lose changes ?""), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
            'SP 2014.02.20 BT-2436 -Commenetd
            'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_CANCEL_CHANGES"), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
            'SP 2014.02.20 BT-2436 -start
            lblMessage.Text = getmessage("MESSAGE_RETIREMENT_PROCESSING_CANCEL_CHANGES")
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "Open", "showDialog('ConfirmDialog','" & lblMessage.Text & "','Cancel');", True) 'MMR | 2017.03.15 | YRS-AT-2625 | Terminating function with semi-colon to avoid any client-side error
            'SP 2014.02.20 BT-2436 -End
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you want to cancel and lose changes?", MessageBoxButtons.YesNo, False)
            Exit Sub
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonCancel_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try

    End Sub
    Private Sub ButtonReCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonReCalculate.Click
        Try
            'SetRetirementDate()
            'loadAnnuityControls()
            Dim l_double_withheld As Double
            'ASHISH:2010.12.16 This code not need here it is already exists in calculateAnnuity() function
            'Dim l_double_Percent As Double = 0

            'If DropDownListPercentage.SelectedIndex <> -1 Then
            '    l_double_Percent = Convert.ToDouble(DropDownListPercentage.SelectedValue)
            'End If
            'TextBoxAmount.Text = Math.Round((Convert.ToDecimal(TextBoxRetiredBenefit.Text) * l_double_Percent) / 100, 2)
            'Session("TextBoxAmount.Text") = TextBoxAmount.Text

            '2012.05.28  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start

            '2012.07.19 SR :BT-1054  - Do not display the beneficiary information change message 
            'LabelEstimateDataChangedMessage.Visible = False
            'SP 2014.09.17 - YRS 5.0-2362-Start
            SetJointSurviourAnnuityDefaultToM()
            'SP 2014.09.17 - YRS 5.0-2362-End
            Page.Validate()
            If Page.IsValid Then
                UpdateSelectedParticiapntBeneficary()
                ViewState("BenificiaryBirthDate") = TextBoxAnnuityBirthDateRet.Text
                UpdateSelectedParticiapntBeneficarySav()
                ViewState("BenificiaryBirthDateSav") = TextBoxAnnuityBirthDateSav.Text
            End If
            '2012.05.28  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End



            Call SetRetirementDate()
            calculateAnnuity("B")





            l_double_withheld = GetWithHolding()
            TextBoxMonthlyWithheld.Text = l_double_withheld

            Me.DataChanged = False
            'NP:2008.04.25 - Perform all validations again after recalculation to ensure that annuity can be purchased
            Dim errorMessage As String
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 Remove termination date parameter
            'If Not RetirementBOClass.IsRetirementValid(errorMessage, False, Me.guiFundEventId, Me.RetirementDate, Me.RetireType, Me.PersId, Me.SSNo, Session("TerminationDate"), Me.FundEventStatus, GetPlanType()) Then
            If Not RetirementBOClass.IsRetirementValid(errorMessage, False, Me.guiFundEventId, Me.RetirementDate, Me.RetireType, Me.PersId, Me.SSNo, Me.FundEventStatus, GetPlanType()) Then
                errorMessage = combinemessage(errorMessage)
                'Session("BasicValidationMessage") = errorMessage
                Session("BasicValidationMessage") = Nothing
                'LabelErrorMessage.Text = errorMessage
                'HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            Else
                Session("BasicValidationMessage") = Nothing
                Session("BasicValidationFailed") = False
                ''LabelErrorMessage.Text = ""
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            Dim exmsg As String

            If (ex.Message.ToString().Contains("MESSAGE")) Then
                exmsg = getmessage(ex.Message.ToString())
            Else
                exmsg = ex.Message.Trim.ToString()
            End If
            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,End
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonReCalculate_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub ButtonFormOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFormOK.Click
        Try
            ClearResources()
            Response.Redirect("MainWebForm.aspx", False)

        Catch ex As Threading.ThreadAbortException 'SP 2013.12.13 BT-2326
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonFormOK_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub DropDownListPercentage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListPercentage.SelectedIndexChanged
        'SP 2013.12.13 BT-2326 - Added try Catch block
        Try
            Session("PercentageSelected") = True
            TextBoxAmount.Text = Math.Round((Convert.ToDecimal(TextBoxRetiredBenefit.Text) * DropDownListPercentage.SelectedValue) / 100, 2)
            Session("TextBoxAmount.Text") = TextBoxAmount.Text
            Session("DropDownListPercentage.SelectedValue") = DropDownListPercentage.SelectedValue
            'for alerting & restricting the user, if the user has made any changes and tried to purchase the annuity of a participant
            Me.DataChanged = True

        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DropDownListPercentage_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub


    'Private Sub DropDownListRetirementType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListRetirementType.SelectedIndexChanged
    '    SetRetireType()

    '    If Me.RetireType = "NORMAL" Then
    '        TextBoxSSBenefit.ReadOnly = False
    '    Else
    '        TextBoxSSBenefit.ReadOnly = True
    '        TextBoxSSBenefit.Text = "0.00"
    '    End If
    '    'for alerting & restricting the user, if the user has made any changes and tried to purchase the annuity of a participant
    '    Me.DataChanged = True
    'End Sub

    Private Sub DataGridRetiredBeneficiaries_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridRetiredBeneficiaries.ItemDataBound
        'Try
        '    If Me.Blank <> True Then
        '        If e.Item.ItemType = ListItemType.Header Then
        '            e.Item.Cells(15).Text = "Type"
        '        End If
        '        e.Item.Cells(1).Visible = False
        '        e.Item.Cells(2).Visible = False
        '        e.Item.Cells(3).Visible = False
        '        e.Item.Cells(4).Visible = False
        '        e.Item.Cells(13).Visible = False
        '        e.Item.Cells(12).Visible = False
        '        If e.Item.ItemType <> ListItemType.Header Then
        '            e.Item.Cells(14).HorizontalAlign = HorizontalAlign.Right
        '        End If
        '        e.Item.Cells(16).Visible = False

        '    Else
        '        e.Item.Cells(0).Visible = False
        '    End If

        'Catch ex As Exception
        '    Dim l_String_Exception_Message As String
        '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        '    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        'End Try
    End Sub
    Private Sub DataGridNotes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridNotes.SelectedIndexChanged

        Try
            'SP 2013.12.13 BT:2326 
            'Dim i As Integer
            'For i = 0 To Me.DataGridNotes.Items.Count - 1
            '    Dim l_button_Select As ImageButton
            '    l_button_Select = DataGridNotes.Items(i).FindControl("ImageButtonNotes")
            '    If Not l_button_Select Is Nothing Then
            '        If i = DataGridNotes.SelectedIndex Then
            '            l_button_Select.ImageUrl = "images\selected.gif"
            '            Me.ButtonView.Enabled = True
            '        Else
            '            l_button_Select.ImageUrl = "images\select.gif"
            '        End If
            '    End If
            'Next
            'The below code is for open popup screen for view
            If Me.DataGridNotes.SelectedIndex <> -1 Then

                'Start : Added by dilip yadav : 2009.09.18
                Dim l_string_Notes As String
                Dim l_datatable_Notes As DataTable
                l_datatable_Notes = Session("DisplayNotes")

                Dim l_date_notes As String
                Dim l_creator_notes As String
                Dim dr_notes As DataRow

                Dim l_checkbox As New CheckBox
                l_date_notes = DataGridNotes.SelectedItem.Cells(1).Text
                l_creator_notes = DataGridNotes.SelectedItem.Cells(2).Text
                l_checkbox = DataGridNotes.SelectedItem.FindControl("CheckBoxImportant")

                Dim index As Integer
                index = Me.DataGridNotes.SelectedItem.ItemIndex
                If l_datatable_Notes.Rows(index)("Note").GetType.ToString <> "System.DBNull" Then
                    Session("Note") = l_datatable_Notes.Rows(index)("Note")
                Else
                    Session("Note") = ""
                End If

                If l_checkbox.Checked Then
                    Session("BitImportant") = True
                Else
                    Session("BitImportant") = False
                End If
                'End : Added by dilip yadav : 2009.09.18
                'Below line commented by dilip yadav : 2009.09.18 : At it shows max 50 characters as shown in grid.
                'Session("Note") = Me.DataGridNotes.SelectedItem.Cells(6).Text.Trim

                Dim popupScript As String
                If Me.DataGridNotes.SelectedItem.Cells(1).Text = "&nbsp;" Then
                    popupScript = "<script language='javascript'>" & _
                      "window.open('UpdateNotes.aspx?Index=" & Me.DataGridNotes.SelectedIndex & "', 'CustomPopUp', " & _
                      "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                      "</script>"
                Else
                    popupScript = "<script language='javascript'>" & _
                      "window.open('UpdateNotes.aspx?UniqueID=" & Me.DataGridNotes.SelectedItem.Cells(1).Text & "', 'CustomPopUp', " & _
                      "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                      "</script>"

                End If
                Page.RegisterStartupScript("PopupScript2", popupScript)
            Else
                'commented by Anudeep on 22-sep for BT-1126
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select notes to display.", MessageBoxButtons.OK)
                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENEFICIARY_VIEW_NOTES"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENEFICIARY_VIEW_NOTES"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
            End If

        Catch sqlEx As SqlException
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DataGridNotes_SelectedIndexChanged-RetirementProcessing", sqlEx)
            HelperFunctions.ShowMessageToUser(sqlEx.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DataGridNotes_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub DataGridFederalWithholding_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridFederalWithholding.SelectedIndexChanged
        Try
            'SP 2013.12.13 BT:2326
            'Dim l_button_select As ImageButton
            'Dim i As Integer
            'For i = 0 To Me.DataGridFederalWithholding.Items.Count - 1
            '    If i = Me.DataGridFederalWithholding.SelectedIndex Then
            '        l_button_select = Me.DataGridFederalWithholding.Items(i).FindControl("ImageButtonSelect")
            '        l_button_select.ImageUrl = "images\selected.gif"
            '    Else
            '        l_button_select = Me.DataGridFederalWithholding.Items(i).FindControl("ImageButtonSelect")
            '        l_button_select.ImageUrl = "images\select.gif"
            '    End If
            'Next
            'This code is used to open popup screen in edit mode
            If Me.DataGridFederalWithholding.SelectedIndex <> -1 Then
                Session("cmbTaxEntity") = Me.DataGridFederalWithholding.SelectedItem.Cells(4).Text.Trim
                Session("cmbWithHolding") = Me.DataGridFederalWithholding.SelectedItem.Cells(3).Text.Trim
                Session("txtExemptions") = Me.DataGridFederalWithholding.SelectedItem.Cells(1).Text.Trim
                Session("txtAddlAmount") = Me.DataGridFederalWithholding.SelectedItem.Cells(2).Text.Trim
                Session("cmbMaritalStatus") = Me.DataGridFederalWithholding.SelectedItem.Cells(5).Text.Trim
                Session("IsFedTaxForMaritalStatus") = True
                Dim popupScript As String
                If Me.DataGridFederalWithholding.SelectedItem.Cells(6).Text = "&nbsp;" Then
                    popupScript = "<script language='javascript'>" & _
                      "window.open('UpdateFedWithHoldingInfo.aspx?Index=" & Me.DataGridFederalWithholding.SelectedIndex & "', 'CustomPopUp', " & _
                      "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                      "</script>"
                Else
                    popupScript = "<script language='javascript'>" & _
                       "window.open('UpdateFedWithHoldingInfo.aspx?UniqueID=" & Me.DataGridFederalWithholding.SelectedItem.Cells(6).Text & "', 'CustomPopUp', " & _
                       "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                       "</script>"
                End If



                Page.RegisterStartupScript("PopupScript2", popupScript)
            Else
                'commented by Anudeep on 22-sep for BT-1126
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an Federal Withholding record to be updated.", MessageBoxButtons.OK)

                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_UPDATE_FEDERAL_WITHHOLDING"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_UPDATE_FEDERAL_WITHHOLDING"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
            End If

        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DataGridFederalWithholding_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub DataGridGeneralWithholding_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridGeneralWithholding.SelectedIndexChanged
        Try
            'SP 2013.12.13 BT:2326
            'Dim l_Button_Select As ImageButton
            'Dim i As Integer
            'For i = 0 To Me.DataGridGeneralWithholding.Items.Count - 1
            '    If i = Me.DataGridGeneralWithholding.SelectedIndex Then
            '        l_Button_Select = Me.DataGridGeneralWithholding.Items(i).FindControl("Imagebutton3")
            '        l_Button_Select.ImageUrl = "images\selected.gif"
            '    Else
            '        l_Button_Select = Me.DataGridGeneralWithholding.Items(i).FindControl("Imagebutton3")
            '        l_Button_Select.ImageUrl = "images\select.gif"
            '    End If
            'Next
            'Below code for opening popup for edit mode
            If Me.DataGridGeneralWithholding.SelectedIndex <> -1 Then
                Session("cmbWithHoldingType") = Me.DataGridGeneralWithholding.SelectedItem.Cells(1).Text.Trim
                Session("txtAddAmount") = Me.DataGridGeneralWithholding.SelectedItem.Cells(2).Text.Trim
                Session("txtStartDate") = Me.DataGridGeneralWithholding.SelectedItem.Cells(3).Text.Trim
                Session("txtEndDate") = Me.DataGridGeneralWithholding.SelectedItem.Cells(4).Text.Trim
                Dim popupScript As String
                If Me.DataGridGeneralWithholding.SelectedItem.Cells(5).Text = "&nbsp;" Then
                    popupScript = "<script language='javascript'>" & _
                       "window.open('UpdateGenHoldings.aspx?Index=" & Me.DataGridGeneralWithholding.SelectedIndex & "', 'CustomPopUp', " & _
                       "'width=750, height=500, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
                       "</script>"

                Else
                    popupScript = "<script language='javascript'>" & _
                       "window.open('UpdateGenHoldings.aspx?UniqueID=" & Me.DataGridGeneralWithholding.SelectedItem.Cells(5).Text & "', 'CustomPopUp', " & _
                       "'width=750, height=500, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
                       "</script>"
                End If

                Page.RegisterStartupScript("PopupScript2", popupScript)
            Else
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_UPDATE_GENERAL_WITHHOLDING"), EnumMessageTypes.Error, Nothing)
            End If

        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DataGridGeneralWithholding_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub DataGridActiveBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridActiveBeneficiaries.SelectedIndexChanged
        Try
            Dim i As Integer
            For i = 0 To Me.DataGridActiveBeneficiaries.Items.Count - 1
                Dim l_button_Select As ImageButton
                l_button_Select = DataGridActiveBeneficiaries.Items(i).FindControl("Imagebutton1")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridActiveBeneficiaries.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If
            Next
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DataGridActiveBeneficiaries_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub DataGridRetiredBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRetiredBeneficiaries.SelectedIndexChanged
        'SP 2013.12.13 BT-2326 -Start
        Try
            Dim i As Integer
            For i = 0 To Me.DataGridRetiredBeneficiaries.Items.Count - 1
                Dim l_button_Select As ImageButton
                l_button_Select = DataGridRetiredBeneficiaries.Items(i).FindControl("Imagebutton2")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridRetiredBeneficiaries.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If
            Next
        Catch ex As Exception

            HelperFunctions.LogException("DataGridRetiredBeneficiaries_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
        'SP 2013.12.13 BT-2326 -End
    End Sub

    'Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented existing code and passing constant name to cells instead of index value
    'Private Sub DataGridActiveBeneficiaries_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridActiveBeneficiaries.ItemDataBound
    '	Try
    '		If Me.Blank <> True Then
    '			e.Item.Cells(1).Visible = False
    '			e.Item.Cells(2).Visible = False
    '			e.Item.Cells(3).Visible = False
    '			e.Item.Cells(4).Visible = False
    '			e.Item.Cells(10).Visible = False
    '			e.Item.Cells(13).Visible = False
    '			e.Item.Cells(14).Visible = False
    '			If e.Item.ItemType <> ListItemType.Header Then
    '				e.Item.Cells(15).HorizontalAlign = HorizontalAlign.Right
    '               End If
    '               '2014.12.01 BT-2310\YRS 5.0-2255 -Start
    '               'hide representative columns details
    '               e.Item.Cells(17).Visible = False
    '               e.Item.Cells(18).Visible = False
    '               e.Item.Cells(19).Visible = False
    '               e.Item.Cells(20).Visible = False
    '               '2014.12.01 BT-2310\YRS 5.0-2255 -End

    '		Else
    '			e.Item.Cells(0).Visible = False
    '		End If
    '	Catch ex As Exception
    '		'SP 2013.12.13 BT-2326 -Start
    '		HelperFunctions.LogException("DataGridActiveBeneficiaries_ItemDataBound-RetirementProcessing", ex)
    '		HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
    '		'SP 2013.12.13 BT-2326 -End
    '	End Try
    '   End Sub
    Private Sub DataGridActiveBeneficiaries_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridActiveBeneficiaries.ItemDataBound
        Try
            If Me.Blank <> True Then
                e.Item.Cells(mConst_DG_ActBenef_UniqueId_Index).Visible = False
                e.Item.Cells(mConst_DG_ActBenef_PersId_Index).Visible = False
                e.Item.Cells(mConst_DG_ActBenef_BenePersId_Index).Visible = False
                e.Item.Cells(mConst_DG_ActBenef_BeneFundEventId_Index).Visible = False
                e.Item.Cells(mConst_DG_ActBenef_BeneficiaryTypeCode_Index).Visible = False
                e.Item.Cells(mConst_DG_ActBenef_DeathFundEventStatus_Index).Visible = False
                e.Item.Cells(mConst_DG_ActBenef_BeneficiaryStatusCode_Index).Visible = False
                If e.Item.ItemType <> ListItemType.Header Then
                    e.Item.Cells(mConst_DG_ActBenef_Pct_Index).HorizontalAlign = HorizontalAlign.Right
                End If
                e.Item.Cells(mConst_DG_ActBenef_RepFirstName_Index).Visible = False
                e.Item.Cells(mConst_DG_ActBenef_RepLastName_Index).Visible = False
                e.Item.Cells(mConst_DG_ActBenef_RepSalutation_Index).Visible = False
                e.Item.Cells(mConst_DG_ActBenef_RepTelephone_Index).Visible = False

            Else
                e.Item.Cells(mConst_DG_ActBenef_CheckboxActBen_Index).Visible = False
            End If
        Catch ex As Exception
            HelperFunctions.LogException("DataGridActiveBeneficiaries_ItemDataBound-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented existing code and passing constant name to cells instead of index value
    'SP 2013.12.13 -Bt-2326 -start
    'This event is used to handle Jquery Message Yes button click
    Private Sub ButtonYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonYes.Click
        Try
            If Not Session("MessageBox") Is Nothing Then
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "CloseDialog();", True)
                HandleMessageBox(Session("MessageBox"), True)
                'Start:Anudeep:2014.06.03 BT:2556- Added below code to ask user whether to purchase annuity after confrming about phonny ssn or paidservicecheck
                'Start: AA:03.16.2016 YRS-AT-2016 Commented below code because this message will be shown at purchase and continue to purchase
                'ElseIf Session("PhonySSNo") = "Phony_SSNo" Then
                '    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "CloseDialog();", True)
                '    Session("MessageBox") = "Purchase"
                '    Dim l_string_Message As String
                '    l_string_Message = getmessage("MESSAGE_RETIREMENT_PROCESING_HANDLE_MESSAGEBOX")
                '    If (YMCARET.YmcaBusinessObject.RefundRequest.GetLoanStatus(Me.PersId)) > 0 Then
                '        l_string_Message = "Paid Loan Exists, " + l_string_Message
                '    End If
                '    ShowCustomMessage(l_string_Message, enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                'End: AA:03.16.2016 YRS-AT-2016 Commented below code because this message will be shown at purchase and continue to purchase
                'End:Anudeep:2014.06.03 BT:2556- Added below code to ask user whether to purchase annuity after confrming about phonny ssn or paidservicecheck                
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ButtonYes_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'SP 2013.12.13 -Bt-2326 -end

    'START: MMR | 2017.03.16 | YRS-AT-2625 | Added button click event for No button in message box
    Private Sub ButtonNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNo.Click
        Try
            If Not Session("MessageBox") Is Nothing Then
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "CloseDialog();", True)
                HandleMessageBox(Session("MessageBox"), False)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ButtonNo_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'END: MMR | 2017.03.16 | YRS-AT-2625 | Added button click event for No button in message box

    'SP 2014.02.20 -Bt-2436 -start
    Private Sub ButtonCancelYes_Click(sender As Object, e As System.EventArgs) Handles ButtonCancelYes.Click
        Try
            Session("MessageBox") = "CancelAndExit"
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "close", "CloseDialog();", True)
            HandleMessageBox(Session("MessageBox"), True)

        Catch ex As Exception
            HelperFunctions.LogException("ButtonCancelYes_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'SP 2014.02.20  -Bt-2436 -end

    'SP 2013.12.13 BT-2326 -Start
    ''This method checks if message box Yes button click, if user click on yes then return true else false
    Private Function IsMessageBoxYesClick() As Boolean
        Dim bClicked As Boolean
        Try
            bClicked = False
            For Each item As String In Request.Form
                If item.Equals("Yes") = True Then
                    bClicked = True
                    Exit For
                End If
            Next
            Return bClicked
        Catch
            Throw
        End Try
    End Function
    'SP 2013.12.13 BT-2326 -End

    'SANKET:03/24/2011 code for YRS 5.0-1294 
    'SP 2013.12.13 BT-2326 -Adding new parameter bYesClick
    ' to handle elase part of case "BackDatedDisabilityRetirement"
    Private Sub HandleMessageBox(ByVal ButtonName As String, ByVal bYesClick As Boolean)
        Try

            Select Case ButtonName
                Case "CancelAndExit"
                    If bYesClick = True Then
                        Session("RP_DataChanged") = False
                        disposeAndRedirectToFind()

                    End If
                    'Start: AA:03.16.2016 YRS-AT-2016 Commented below code because this message will be shown at purchase and continue to purchase
                    'Case "PaidServiceCheck"
                    '	If bYesClick = True Then ''SP 2013.12.13 BT-2326
                    '		Session("MessageBox") = "Purchase"
                    '		Dim l_string_Message As String
                    '		'commented by Anudeep on 22-sep for BT-1126
                    '		'l_string_Message = "Do you wish to purchase this annuity ?"
                    '		l_string_Message = getmessage("MESSAGE_RETIREMENT_PROCESING_HANDLE_MESSAGEBOX")
                    '		If (YMCARET.YmcaBusinessObject.RefundRequest.GetLoanStatus(Me.PersId)) > 0 Then
                    '			l_string_Message = "Paid Loan Exists, " + l_string_Message
                    '		End If
                    '		ShowCustomMessage(l_string_Message, enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                    '		Exit Sub
                    '	End If
                    'End: AA:03.16.2016 YRS-AT-2016 Commented below code because this message will be shown at purchase and continue to purchase
                Case "Purchase"
                    If bYesClick = True Then ''SP 2013.12.13 BT-2326
                        If yrsAnnuityPurchaseActions() = True Then
                            'for alerting & restricting the user, if the user has made any changes and tried to purchase the annuity of a participant
                            Session("RP_DataChanged") = False
                        End If
                    End If
                Case "BackDatedDisabilityRetirement"
                    If bYesClick = True Then ''SP 2013.12.13 BT-2326
                        Session("BackDatedDisabilityRetirementQuestionAsked") = "True"
                        Me.RetirementDate = TextBoxRetirementDate.Text.Trim()
                    Else
                        TextBoxRetirementDate.Text = Me.RetirementDate
                        Session("BackDatedDisabilityRetirementQuestionAsked") = "False" 'MMR | 2017.03.16 | YRS-AT-2625 | Setting session value from "Nothing" to "False" of click on NO button to not display message on postback
                        'START: MMR | 2017.03.16 | YRS-AT-2625 | Loading manual transaction details as original retirement set on click of no button in message box
                        If CheckBoxRetPlan.Checked = True Then
                            LoadManualTransactionForDisability(Me.guiFundEventId, Me.RetireType, TextBoxRetirementDate.Text)
                            hdnMessage.Value = ""
                        End If
                        'END: MMR | 2017.03.16 | YRS-AT-2625 | Loading manual transaction details as original retirement set on click of no button in message box
                    End If
                Case Else
            End Select

            Session("MessageBox") = Nothing

        Catch
            Throw
        End Try
    End Sub
    'Old - YRS 5.0-1035
    'Private Sub HandleMessageBox(ByVal ButtonName As String)
    '    Select Case ButtonName
    '        Case "CancelAndExit"
    '            If Request.Form("Yes") = "Yes" Then
    '                Session("RP_DataChanged") = False
    '                disposeAndRedirectToFind()
    '            End If
    '        Case "Purchase"
    '            If Request.Form("Yes") = "Yes" Then
    '                If yrsAnnuityPurchaseActions() = True Then
    '                    'for alerting & restricting the user, if the user has made any changes and tried to purchase the annuity of a participant
    '                    Session("RP_DataChanged") = False
    '                End If
    '            End If
    '        Case "BackDatedDisabilityRetirement"
    '            If Request.Form("Yes") = "Yes" Then
    '                Session("BackDatedDisabilityRetirementQuestionAsked") = "True"
    '                Me.RetirementDate = TextBoxRetirementDate.Text.Trim()
    '            Else
    '                TextBoxRetirementDate.Text = Me.RetirementDate
    '                Session("BackDatedDisabilityRetirementQuestionAsked") = Nothing
    '            End If
    '        Case Else
    '    End Select

    '    Session("MessageBox") = Nothing
    'End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim i As Integer
        Dim retType As String
        Dim strWSMessage As String
        retType = Request.QueryString.Get("RetType")

        'Sanket Vaidya          17 Feb 2011     For BT 665 : For disability requirement
        If retType = "Normal" Then
            Me.RetireType = "NORMAL"
            lblRetirementType.Text = "Normal"
            strFormName = "FindInfo.aspx?Name=Process&RetType=Normal" 'PK| 02.21.2019 | YRS-AT-4248| Depending on the page strFormName will take the url
        ElseIf retType = "Disability" Then
            Me.RetireType = "DISABL"
            lblRetirementType.Text = "Disabled"
            strFormName = "FindInfo.aspx?Name=Process&RetType=Disability" 'PK| 02.21.2019 | YRS-AT-4248| Depending on the page strFormName will take the url
        End If



        If Me.LoggedUserKey = "" Then
            Response.Redirect("Login.aspx", False)
        End If

        If Session("FundId") Is Nothing Or Session("FundId") = "" Then
            Response.Redirect("FindInfo.aspx?Name=Process&RetType=" + retType, False)
            Exit Sub
        End If

        'Redirectingto FindInfo page.If retiree birth date not present
        If Not Session("RetireeBirthDatePresent") Is Nothing Then
            If Me.RetireeBirthDatePresent = False Then
                Session("RetireeBirthDatePresent") = Nothing
                disposeAndRedirectToFind()
            End If
        End If

        'if employee history details not available then
        If Session("RP_EmpHistoryInfoNotset") = True Then
            Session("RP_EmpHistoryInfoNotset") = False
            Session("RP_EmpHistoryInfoNotset") = Nothing
            disposeAndRedirectToFind()
        End If
        'Ashish Bt-917
        If Not Session("ExistingAnnuityPurchaseDtNotExists") Is Nothing Then
            If Session("ExistingAnnuityPurchaseDtNotExists") = True Then
                Session("ExistingAnnuityPurchaseDtNotExists") = False
                disposeAndRedirectToFind()
            End If
        End If
        CheckReadOnlyMode() 'Shilpa N | 03/16/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
        If Not IsPostBack Then
            SessionManager.SessionStateWithholding.LstSWHPerssDetail = Nothing ' ML | YRS-AT-4597 | Clear Session Variable.
            'START: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.
            Session("DisplayNotes") = Nothing
            'END: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.
            Session("AnnuityDetails") = Nothing 'Dharmesh : 12/7/2018 : YRS-AT-4135 : Making the session empty for not adding mulitple maunual added record from the Process screen.
            Call ClearResources()

            'Call ClearResources() : not required already called above
            ListBoxPercentage.Items.Add(0)
            DropDownListPercentage.Items.Add(0)
            For i = 90 To 1 Step -1
                ListBoxPercentage.Items.Add(i)
                DropDownListPercentage.Items.Add(i)
            Next

            DropDownListPercentage.SelectedValue = 0
            ListBoxPercentage.SelectedValue = DropDownListPercentage.SelectedValue

            Dim l_intLoggedUser As Integer
            Dim l_Int_UserId As Integer
            l_Int_UserId = Convert.ToInt32(Session("LoggedUserKey"))
            l_intLoggedUser = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.GetLoginNotesUser(l_Int_UserId)

            If l_intLoggedUser = 1 Then
                Me.NotesGroupUser = True
            Else
                Me.NotesGroupUser = False
            End If

            Call GetPersonDetails()


            'Setting session variable to track if this is a QDRO beneficiary fund event
            Dim intQDRO As Int16
            Me.OrgBenTypeIsQDRO = False

            intQDRO = BORetireeEstimate.EligibleIsQDRO(Me.guiFundEventId)
            If intQDRO = 1 Then
                Me.OrgBenTypeIsQDRO = True
            End If
            'Ashish BT-917
            ValidateExistingAnnuityPurchaseDate(Me.PersId, Me.FundEventStatus)
            If (Session("ExistingAnnuityPurchaseDtNotExists") = True) Then
                'commented by Anudeep on 22-sep for BT-1126
                'Call ShowCustomMessage("Annuity details are missing for the participant."), enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                'Call ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_ANNUITY_DETAILS_MISSING"), enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_ANNUITY_DETAILS_MISSING"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            End If


            'SR:2013.08.05 - YRS 5.0-2070 : Change the color of Beneficiary tab for Pending request.
            'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
            strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
            If strWSMessage <> "NoPending" Then
                strWSMessage = (strWSMessage.Replace("<br/>", "\n")).Replace("<br>", "\n")
                imgLockBeneficiary.Visible = True
                imgLockBeneficiary.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Bene');")
                imgLockBeneficiary.Attributes.Add("onmouseout", "javascript: hideToolTip();")
            End If
            'End, SR:2013.08.05 - YRS 5.0-2070 : Change the color of Beneficiary tab for Pending request.


            Session("AuditBeneficiariesTable") = Nothing    'SR | 2016.08.02 | YRS-AT-2382 | Clearing auditlog data table
        End If

        'SANKET:03/24/2011 code for YRS 5.0-1294 
        'SP 2013.12.13 BT:2326 - Below Code is removed and handle on ButtonYes click Event

        ' If basic validation has failed then return to FindInfo page -
        'SP 2013.12.13 BT:2326 - Commented because it is already written in below
        'If Session("BasicValidationFailed") = True Then
        '	'LabelErrorMessage.Text = Session("BasicValidationMessage")
        '	HelperFunctions.ShowMessageToUser(Session("BasicValidationMessage"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
        'End If

        Me.Page.EnableViewState = True
        LabelNotSet.Visible = False

        Me.TextBoxRetirementDate.RequiredDate = True

        Try

            'Moved into above not postback block and changed condition to maintain equivalence
            If Me.AnnuitySelected = True Then
                TextBoxRetirementDate.Text = Me.RetirementDate
                Me.loadControlsFromSessionVariables()
            End If
            'Moved into above not postback block

            LoadBeneficiariesTab()
            If Not IsPostBack Then
                LoadNotesTab()
            End If

            'NP:YRPS-4539:2008.01.29 - Calling this to load the annuity beneficiaries information if it exists so that the different annuity values are properly computed
            If Not Page.IsPostBack Then
                'If this is the first time we are loading this page, populate the annuity beneficiaries information
                'Set the annuities selected as J type for both plans. This is required because the function checks for those values before loading beneficiaries
                '2012.05.31 SP :BT-975/YRS 5.0-1508 -Commented start
                'TextBoxAnnuitySelectRet.Text = "J1" : TextBoxAnnuitySelectSav.Text = "J1" 
                'Load joint survivors for both the Retirement plan and the Savings plan
                'loadJointAnnuityBenefeciaryTab(True, "R") : loadJointAnnuityBenefeciaryTab(True, "S")
                'Reset the annuity types to M which is the default
                'TextBoxAnnuitySelectRet.Text = "M" : TextBoxAnnuitySelectSav.Text = "M"
                'Hide joint survivors information from the tab
                'loadJointAnnuityBenefeciaryTab(False, "R") : loadJointAnnuityBenefeciaryTab(False, "S")
                '2012.05.31 SP :BT-975/YRS 5.0-1508 -Commented end
                '2012.05.24 SP : BT-975/YRS 5.0-1508 -Adde new
                Me.BindRelationShipDropDown()
                Me.BindBeneficiaryDetails(Me.PersId, "j", "R")
                Me.BindBeneficiaryDetails(Me.PersId, "j", "S")
                Me.EnableDisableValidationBeneficiarySav(False)
                Me.EnableDisableValidationBeneficiary(False)
                Me.AddressWebUserControlRet.LoadAddressDetail(Nothing)
                Me.AddressWebUserControlSav.LoadAddressDetail(Nothing)
                '2012.05.24 SP : BT-975/YRS 5.0-1508 -
            End If
            'END 2008.01.29
            'START: MMR | 2017.03.24 | YRS-AT-2625 | Commented as created common code
            ''START: MMR | 2017.03.06 | YRS-AT-2625 | Binding manual transaction details to grid from session on post back and hiding warning message on selection of manual transaction
            'If Page.IsPostBack Then
            '    If (Me.RetireType = "DISABL") Then
            '        'Dialog box should not open if only savings plan selected
            '        If CheckBoxRetPlan.Checked = False Then
            '            ResetManualTransactionDetails()
            '            DivWarningMessage.Style("display") = "none"
            '            'Manual transaction should be loaded for retirement plan type selection
            '        ElseIf CheckBoxRetPlan.Checked = True AndAlso TextBoxRetirementDate.Text.Trim() <> "" Then
            '            If hdnMessage.Value = "" Then
            '                TextBoxRetirementDate.Text = GetFirstDayOfMonth(TextBoxRetirementDate.Text.Trim())
            '                LoadManualTransactionForDisability(Me.guiFundEventId, Me.RetireType, TextBoxRetirementDate.Text)
            '            End If
            '        End If
            '        If hdnManualTransaction.Value = "3" Then
            '            DivWarningMessage.Style("display") = "none"
            '        End If
            '    End If
            'End If
            ''END: MMR | 2017.03.06 | YRS-AT-2625 | Binding manual transaction details to grid from session on post back and hiding warning message on selection of manual transaction
            'END: MMR | 2017.03.24 | YRS-AT-2625 | Commented as created common code

            'START: MMR | 2017.03.24 | YRS-AT-2625 | Added to set session property to nothing if retirement date is empty
            If Page.IsPostBack Then
                If TextBoxRetirementDate.Text.trim() = "" Then
                    ResetManualTransactionDetails()
                End If
            End If
            'END: MMR | 2017.03.24 | YRS-AT-2625 | Added to set session property to nothing if retirement date is empty

            'SANKET:03/24/2011 code for YRS 5.0-1294  
            If (Me.RetireType = "DISABL") Then
                If TextBoxRetirementDate.Text.Trim() <> String.Empty Then
                    TextBoxRetirementDate.Text = GetFirstDayOfMonth(TextBoxRetirementDate.Text.Trim())
                    'START: MMR | 2017.03.17 | YRS-AT-2625 | Resolving problem with confirmation message box (Not appearing) that was required to appear during backdated disability retirement
                    'If (Convert.ToDateTime(Me.TerminationDate) > Convert.ToDateTime(TextBoxRetirementDate.Text)) _
                    '    AndAlso Session("BackDatedDisabilityRetirementQuestionAsked") Is Nothing Then
                    '    'commented by Anudeep on 22-sep for BT-1126
                    '    'ShowCustomMessage("This specified retirement date is earlier than the employment termination date.  Are you sure you want to continue ?"), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                    '    ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_EMPLOYEMENT_TERMINATION_DATE"), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                    '    Session("MessageBox") = "BackDatedDisabilityRetirement"
                    '    Exit Sub
                    'End If
                    If (Convert.ToDateTime(Me.TerminationDate) > Convert.ToDateTime(TextBoxRetirementDate.Text)) Then
                        If Session("MessageBox") Is Nothing And (Session("BackDatedDisabilityRetirementQuestionAsked") = "False" Or Session("BackDatedDisabilityRetirementQuestionAsked") Is Nothing) Then
                            ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_EMPLOYEMENT_TERMINATION_DATE"), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                            Session("MessageBox") = "BackDatedDisabilityRetirement"
                            DisplayManualTransactionDetails() 'MMR | 2017.03.24 | YRS-AT-2625 | Added method to load manual transaction details based on backdated retirement date on click of yes button in message box
                            Exit Sub
                        End If
                        If (Not Session("MessageBox") Is Nothing Or Session("BackDatedDisabilityRetirementQuestionAsked") = "False") Then
                            Exit Sub
                        End If
                    End If
                    'END: MMR | 2017.03.17 | YRS-AT-2625 | Resolving problem with confirmation message box (Not appearing) that was required to appear during backdated disability retirement

                    'If user selects future date or greater than default termination date set for disability of person
                    If (Convert.ToDateTime(TextBoxRetirementDate.Text) > Convert.ToDateTime(Session("ValidTerminationDateForDisability"))) Then
                        Me.RetirementDate = Session("ValidTerminationDateForDisability")
                        TextBoxRetirementDate.Text = Me.RetirementDate
                    End If

                    'If there is no termination date present in db
                    If (Me.RetirementDate = Convert.ToDateTime("1/1/1900")) Then
                        Session("RP_EmpHistoryInfoNotset") = True
                        'commented by Anudeep on 22-sep for BT-1126
                        'Call ShowCustomMessage("No Termination date.Please check the employment record of person."), enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                        'Call ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_TERMINATION_DATE_DOESNOT_EXISTS"), enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                        HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_TERMINATION_DATE_DOESNOT_EXISTS"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                        Exit Sub
                    End If
                End If
            End If

            LoadControls()

            DisplayManualTransactionDetails() 'MMR | 2017.03.24 | YRS-AT-2625 | Called method to load manual transaction details on postback

            'Redirectingto FindInfo page.If retiree birth date not present
            If TextBoxBirthDateRet.Text <> String.Empty Then
                Me.RetireeBirthDatePresent = True
            Else
                'commented by Anudeep on 22-sep for BT-1126
                'ShowCustomMessage("Retiree Birth Date is missing."), enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_RETIREE_BIRTH_DATE_MISSING"), enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_RETIREE_BIRTH_DATE_MISSING"), EnumMessageTypes.Error, Nothing)  'SP 2013.12.13 BT:2326
                Me.RetireeBirthDatePresent = False
                Exit Sub
            End If

            If Session("RP_EmpHistoryInfoNotset") = True Then
                'commented by Anudeep on 22-sep for BT-1126
                'Call ShowCustomMessage("Unable to initialize employment history for the member."), enumMessageBoxType.DotNet)
                'Call ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_UNABLETO_INITIALIZE_EMPLOYEMENT"), enumMessageBoxType.DotNet)
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_UNABLETO_INITIALIZE_EMPLOYEMENT"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                Exit Sub
            End If

            Call SetRetirementDate()

            'If Not Page.IsPostBack Then 'commented on 6-Jan-2009 for YRS 5.0-636
            Dim errorMessage As String

            'Call the BO retirement validate method
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            'If Not RetirementBOClass.IsRetirementValid(errorMessage, False, Me.guiFundEventId, Me.RetirementDate, Me.RetireType, Me.PersId, Me.SSNo, Session("TerminationDate"), Me.FundEventStatus, GetPlanType()) Then


            If Not RetirementBOClass.IsRetirementValid(errorMessage, False, Me.guiFundEventId, Me.RetirementDate, Me.RetireType, Me.PersId, Me.SSNo, Me.FundEventStatus, GetPlanType()) Then
                errorMessage = combinemessage(errorMessage)
                Session("BasicValidationFailed") = True
                If Session("BasicValidationMessage") Is Nothing Then ' Handle null
                    Session("BasicValidationMessage") = String.Empty
                End If
                'commented by Hafiz on 12-Jan-2009
                'Session("BasicValidationMessage") = errorMessage + Session("BasicValidationMessage").ToString().Replace("Eligibility criteria is not met:", String.Empty)

                Session("BasicValidationMessage") = errorMessage
            Else
                Session("BasicValidationMessage") = Nothing
                Session("BasicValidationFailed") = False
                'LabelErrorMessage.Text = ""
            End If
            'End If 'commented on 6-Jan-2009 for YRS 5.0-636


            If Me.Flag = "AddBeneficiaries" Or Me.Flag = "EditBeneficiaries" Or Me.Flag = "EditedBeneficiaries" Then
                TabStripRetireesInformation.SelectedIndex = 3
                MultiPageRetirementProcessing.SelectedIndex = 3
                LoadBeneficiariesTab()
                Me.Flag = ""
                Me.iCounter = 0
            End If

            If Me.Flag = "AddNotes" Then
                TabStripRetireesInformation.SelectedIndex = 5
                MultiPageRetirementProcessing.SelectedIndex = 5
                LoadNotesTab()
                Me.Flag = ""
                ButtonCancel.Enabled = True
            End If

            If IsPostBack Then
                '2012.06.04 SP ::BT-975/YRS 5.0-1508
                If CheckBoxRetPlan.Checked And ButtonSelectRet.Enabled Then
                    HideBeneficiaryRetControls(True)
                Else
                    HideBeneficiaryRetControls(False)
                End If
                If CheckBoxSavPlan.Checked And ButtonSelectSav.Enabled Then
                    HideBeneficiarySavControls(True)
                Else
                    HideBeneficiarySavControls(False)
                End If
                '2012.06.04 SP ::BT-975/YRS 5.0-1508
                If Me.AnnuitySelected = True Then
                    'lblAnnuityMessage.Text = ""
                    'lblAnnuityMessage.Visible = False
                    ' Populate the values that the user has selected in ChooseAnnuityForm
                    If Me.SelectedPlan = "R" Then

                        Me.TextBoxAnnuitySelectRet.Text = Session("Annuity_Type")
                        Me.TextBoxTaxableRet.Text = Math.Round(Convert.ToDecimal(Session("Taxable_Amount") + Session("SSIncrease")), 2).ToString("#0.00") ' '
                        Me.TextBoxNonTaxableRet.Text = Math.Round(Convert.ToDecimal(Session("NonTaxable")), 2).ToString("#0.00") '
                        Me.TextBoxTotalPaymentRet.Text = Math.Round(Convert.ToDecimal(Session("Amount") + Session("SSIncrease")), 2).ToString("#0.00") '
                        If IsPrePlanSplitRetirement Then
                            If Me.CheckBoxSavPlan.Checked = True Then
                                Me.TextBoxAnnuitySelectSav.Text = Session("Annuity_Type")
                                Me.calculateAnnuity("S")
                            End If
                        End If
                        If Session("CalledAnnuity") = "BOTH" Then
                            If ExactAgeEffDate = String.Empty Or ExactAgeEffDate = "" Then
                                ExactAgeEffDate = "01/01/2011"
                            End If
                            If Session("SelectedCalledAnnuity_Ret") = "OLD" Then
                                'SP 2013.12.13 BT-2326
                                'lblAnnuityMessage.Text = "Annuity computed using " & Me.ExactAgeEffDate & " balance"
                                'lblAnnuityMessage.Visible = True
                                HelperFunctions.ShowMessageToUser("Annuity computed using " & Me.ExactAgeEffDate & " balance", EnumMessageTypes.Warning)
                            ElseIf Session("SelectedCalledAnnuity_Ret") = "NEW" Then
                                'lblAnnuityMessage.Text = ""
                                'lblAnnuityMessage.Visible = False
                            End If
                        End If
                    ElseIf Me.SelectedPlan = "S" Then
                        Me.TextBoxAnnuitySelectSav.Text = Session("Annuity_Type")
                        Me.TextBoxTaxableSav.Text = Math.Round(Convert.ToDecimal(Session("Taxable_Amount")), 2).ToString("#0.00") '
                        Me.TextBoxNonTaxableSav.Text = Math.Round(Convert.ToDecimal(Session("NonTaxable")), 2).ToString("#0.00")  '
                        Me.TextBoxTotalPaymentSav.Text = Math.Round(Convert.ToDecimal(Session("Amount")), 2).ToString("#0.00") '
                        If Session("CalledAnnuity") = "BOTH" Then
                            If ExactAgeEffDate = String.Empty Or ExactAgeEffDate = "" Then
                                ExactAgeEffDate = "01/01/2011"
                            End If
                            If Session("SelectedCalledAnnuity_Sav") = "OLD" Then
                                'SP 2013.12.13 BT-2326
                                'lblAnnuityMessage.Text = "Annuity computed using " & Me.ExactAgeEffDate & " balance."
                                'lblAnnuityMessage.Visible = True
                                HelperFunctions.ShowMessageToUser("Annuity computed using " & Me.ExactAgeEffDate & " balance", EnumMessageTypes.Warning)
                            ElseIf Session("SelectedCalledAnnuity_Sav") = "NEW" Then
                                'lblAnnuityMessage.Text = ""
                                'lblAnnuityMessage.Visible = False
                            End If
                        End If
                    End If
                    '2012.05.31 SP :BT-975/YRS 5.0-1508
                    'loadJointAnnuityBenefeciaryTab(Me.AnnuitySelected, Me.SelectedPlan)

                    Me.SelectedPlan = String.Empty
                    Me.AnnuitySelected = False

                    Call SetRetirementDate()
                    saveControlValuesInSessionVariables()


                End If

                If Not IsPostBack Then
                    TextBoxAnnuitySelectRet.Text = "M"
                    TextBoxAnnuitySelectSav.Text = "M"
                End If

                Me.setControlsAsReadOnly()
            End If

            If (Me.blnAddFedWithHoldings = True Or Me.blnUpdateFedWithHoldings = True) Or IsPostBack = False Then

                If (Me.blnAddFedWithHoldings = True Or Me.blnUpdateFedWithHoldings = True) Then
                    Me.MultiPageRetirementProcessing.SelectedIndex = 1
                    Me.TabStripRetireesInformation.SelectedIndex = 1
                End If

                LoadFedWithDrawalTab()

                'enabling/disabling the purchase tab according to the federal tax withholding data entered.
                'if Federal Tax withholding is entered with withholding type as "FEDEX" whose equivalent is "IRS" 
                'then only purchase tab is enabled else disabled
                Dim drRow As DataRow
                Dim l_dataset_FedWith As DataSet
                TabStripRetireesInformation.Items(6).Enabled = False

                If Not Me.FedWithDrawals Is Nothing Then
                    l_dataset_FedWith = Me.FedWithDrawals
                    If l_dataset_FedWith.Tables.Count > 0 Then
                        If l_dataset_FedWith.Tables(0).Rows.Count > 0 Then
                            For Each drRow In l_dataset_FedWith.Tables(0).Rows
                                If drRow("Tax Entity").GetType.ToString() <> "System.DBNull" Then
                                    If Trim(drRow("Tax Entity")) = "IRS" Then
                                        TabStripRetireesInformation.Items(6).Enabled = True
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
            End If
            'START: ML | 2019.09.20 | YRS-AT-4597 | Refresh Control to Load Grid
            If IsPostBack Then
                stwListUserControl.Refresh()
                If Me.ValidateSTWvsFedtaxforMA = False Then
                    Exit Sub
                End If
            End If
            'END:  ML | 2019.09.20 | YRS-AT-4597 | Refresh Control to Load Grid
            If (Me.blnAddGenWithHoldings = True Or Me.blnUpdateGenWithDrawals = True) Or IsPostBack = False Then
                If (Me.blnAddGenWithHoldings = True Or Me.blnUpdateGenWithDrawals = True) Then
                    Me.MultiPageRetirementProcessing.SelectedIndex = 2
                    Me.TabStripRetireesInformation.SelectedIndex = 2
                End If

                LoadGenWithDrawalTab()
            End If

            If Session("resetSSlevelingValues") = "True" Then
                resetSSlevelingValues()
            End If

            TextBoxAmount.Attributes.Add("onkeypress", "JavaScript:ValidateDecimal();")
            TextBoxSSBenefit.Attributes.Add("onkeypress", "JavaScript:ValidateDecimal();")
            TextBoxRetiredBenefit.Attributes.Add("onkeypress", "JavaScript:ValidateDecimal();")
            TextBoxRetirementDate.AutoPostBack = True

            Me.TextBoxSSBenefit.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            Me.TextBoxSSBenefit.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
            Me.TextBoxSSBenefit.Attributes.Add("OnPaste", "javascript:ValidateNumeric();")
            Me.TextBoxAmount.Attributes.Add("OnPaste", "javascript:ValidateNumeric();")

            'START: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)
            ''2012.05.28 SP :BT-975/YRS 5.0-1508 - 
            'Me.TextBoxAnnuityBirthDateRet.Attributes.Add("onchange", "javascript:PostBackCalendarBenef();")
            'Me.Popcalendar3.Attributes.Add("OnSelectionChanged", "javascript:PostBackCalendarBenef();") 'PPP | 2015.09.25 | YRS-AT-2596
            'Me.TextBoxAnnuityBirthDateSav.Attributes.Add("onchange", "javascript:PostBackCalendarBenef();")
            'Me.PopcalendarSaving.Attributes.Add("OnSelectionChanged", "javascript:PostBackCalendarBenef();") 'PPP | 2015.09.25 | YRS-AT-2596
            ''2012.05.28 SP :BT-975/YRS 5.0-1508 - 
            Me.TextBoxAnnuityBirthDateRet.Attributes.Add("onchange", "javascript:PostBackCalendarBenef(this);")
            Me.TextBoxAnnuityBirthDateSav.Attributes.Add("onchange", "javascript:PostBackCalendarBenef(this);")
            'END: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)
            ' Check if the user has changed the Retirement date across the PlanSplitDate
            If Page.IsPostBack Then

                If (IsPrePlanSplitRetirement = True And Convert.ToDateTime(RetirementDate) >= Session("PlanSplitDate")) _
                Or (IsPrePlanSplitRetirement = False And Convert.ToDateTime(RetirementDate) < Session("PlanSplitDate")) Then

                    Me.IsPrePlanSplitRetirement = (Convert.ToDateTime(Me.RetirementDate) < Session("PlanSplitDate"))

                    Me.SetRetirementDate()

                    Me.loadAnnuityControls()
                    '2012.05.31 SP :BT-975/YRS 5.0-1508
                    'If CheckBoxRetPlan.Checked = True Then
                    '	Me.loadJointAnnuityBenefeciaryTab(True, "R")
                    'End If
                    'If CheckBoxSavPlan.Checked = True Then
                    '	Me.loadJointAnnuityBenefeciaryTab(True, "S")
                    'End If
                    '2012.05.31 SP :BT-975/YRS 5.0-1508

                    Call ButtonReCalculate_Click(New Object, New System.EventArgs)

                    If CheckBoxRetPlan.Checked = False Then
                        Me.clearRetirementControls()
                    End If

                    If CheckBoxSavPlan.Checked = False Then
                        Me.clearSavingsControls()
                    End If
                Else
                    '' If age of the participant is >= 62 then disable SSLevelling controls
                    Dim age As Decimal = Convert.ToDecimal(DateDiff(DateInterval.Month, Convert.ToDateTime(TextBoxBirthDateRet.Text), Convert.ToDateTime(TextBoxRetirementDate.Text)) / 12).ToString()
                    If age >= 62 Then 'Or age < 55 Then
                        CheckboxIncludeSSLevelling.Checked = False
                        CheckboxIncludeSSLevelling.Enabled = False

                        Call CheckboxIncludeSSLevelling_CheckedChanged(New Object, New System.EventArgs)
                    Else
                        If CheckboxIncludeSSLevelling.Enabled = False Then
                            CheckboxIncludeSSLevelling.Enabled = True
                            CheckboxIncludeSSLevelling.Checked = False
                        End If
                    End If
                End If
            End If

            '2012.05.28 SP :BT-975/YRS 5.0-1508 - Start
            If ViewState("BenificiaryBirthDate") Is Nothing Then
                ViewState("BenificiaryBirthDate") = TextBoxAnnuityBirthDateRet.Text
            Else

                If ViewState("BenificiaryBirthDate") <> TextBoxAnnuityBirthDateRet.Text Then
                    'LabelEstimateDataChangedMessage.Visible = True
                    ViewState("IsRetBenificiaryChanged") = True  'SP 2014.09.17 YRS 5.0-2362
                    'HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENFICIARY_INFORMATION_CHANGED"), EnumMessageTypes.Information)
                    ViewState("BenificiaryBirthDate") = TextBoxAnnuityBirthDateRet.Text
                    DeselectedAnnuityBeneficiaryGrid()

                    'Else
                    '	LabelEstimateDataChangedMessage.Visible = False
                End If

            End If
            '2012.05.28 SP :BT-975/YRS 5.0-1508 - End

            '2012.06.04 SP :BT-975/YRS 5.0-1508 
            If ViewState("BenificiaryBirthDateSav") Is Nothing Then
                ViewState("BenificiaryBirthDateSav") = TextBoxAnnuityBirthDateSav.Text
            Else

                If ViewState("BenificiaryBirthDateSav") <> TextBoxAnnuityBirthDateSav.Text Then
                    'LabelEstimateDataChangedMessage.Visible = True
                    ViewState("IsSavBenificiaryChanged") = True 'SP 2014.09.17 YRS 5.0-2362
                    'HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENFICIARY_INFORMATION_CHANGED"), EnumMessageTypes.Information)
                    ViewState("BenificiaryBirthDateSav") = TextBoxAnnuityBirthDateSav.Text
                    DeselectedAnnuityBeneficiaryGridSav()

                    'Else
                    '    LabelEstimateDataChangedMessage.Visible = False
                End If

            End If
            '2012.06.04 SP :BT-975/YRS 5.0-1508 

            'Start, SP 2014.09.25 YRS5.0-2362 -Commented becuase its handle on purchse tab
            '2012.07.19 SR BT -1054  : Disable the validator for beneficiary details
            'If CheckBoxRetPlan.Checked = True And Left(TextBoxAnnuitySelectRet.Text.Trim.ToString(), 1).ToUpper() = "J" Then

            '    EnableDisableValidationBeneficiary(True)
            'Else
            '    EnableDisableValidationBeneficiary(False)
            'End If
            'If CheckBoxSavPlan.Checked = True And Left(TextBoxAnnuitySelectSav.Text.Trim.ToString(), 1).ToUpper() = "J" Then

            '    EnableDisableValidationBeneficiarySav(True)
            'Else
            '    EnableDisableValidationBeneficiarySav(False)
            'End If
            '2012.07.19 SR BT -1054  : Disable the validator for beneficiary details
            'End, SP 2014.09.25 YRS5.0-2362 -Commented becuase its handle on purchse tab

            If Not Session("BasicValidationMessage") Is Nothing Then
                'LabelErrorMessage.Text = Session("BasicValidationMessage")
                HelperFunctions.ShowMessageToUser(Session("BasicValidationMessage"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
            End If
            '2012.07.16 SP : BT-712/YRS 5.0-1246 : Handling DLIN records -Start
            If RetirementBOClass.IsExistsDLINRecordBeforeRetirement(Me.guiFundEventId, Me.RetirementDate) = True Then
                'commented by Anudeep on 22-sep for BT-1126
                'LabelErrorMessage.Text = LabelErrorMessage.Text + "<br/>Daily Interest records dated for the purchase date exist.  Please correct before proceeding."
                'LabelErrorMessage.Text = LabelErrorMessage.Text + "<br/>" + getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_DAILY_INTEREST")
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PURCHASE_DAILY_INTEREST"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
            End If
            '2012.07.16 SP : BT-712/YRS 5.0-1246 : Handling DLIN records -End

            'START : BD : 2018.11.01 : YRS-AT-4135 : Display Message if RDB is restricted
            If Not Session("RDBwarningMessage") Is Nothing Then
                HelperFunctions.ShowMessageToUser(Session("RDBwarningMessage"), EnumMessageTypes.Warning, Nothing)
            End If
            'END : BD : 2018.11.01 : YRS-AT-4135 : Display Message if RDB is restricted
            'Page.Validate()
            'If Not Page.IsValid Then
            '	LabelAnnuityBenefeciary.Text = "Annuity Benefeciary Tab"
            'Else
            '	LabelAnnuityBenefeciary.Text = String.Empty
            'End If            
            'SP Bt-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process -Start
            errorMessage = ValidateLastPayrollDate()
            If errorMessage <> "" Then
                'AA:03.14.2016 YRS-AT-2599 Added below code to show waring message but not as error message
                If Me.FundEventStatus = "PT" Or Me.FundEventStatus = "RPT" Then
                    HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Warning, Nothing)
                Else
                    HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing)
                End If
                Exit Sub
            End If
            'SP Bt-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process -End
           
        Catch ex As Exception
            Dim l_String_Exception_Message As String

            Dim exmsg As String

            If (ex.Message.ToString().Contains("MESSAGE")) Then
                exmsg = getmessage(ex.Message.ToString())
            Else
                exmsg = ex.Message.Trim.ToString()
            End If
            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,End
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("Page_Load-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(exmsg, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
        'SP 2013.12.13 BT:2326 The below code for redirect to mainpage has been moved into the YrsAnnuitypurchaseaction method
    End Sub
    Private Sub ValidateExistingAnnuityPurchaseDate(ByRef p_persid As String, ByVal p_fundStatus As String)
        ''SP 2013.12.13 BT-2326 -Added try catch block
        Try
            If ((p_fundStatus = "RT") Or (p_fundStatus = "RPT") Or
             (p_fundStatus = "RA") Or
             (p_fundStatus = "RDNP") Or
             (p_fundStatus = "RE")) Then
                Dim objRet As RetirementBOClass = New RetirementBOClass()
                objRet.GetPurchasedAnnuityDetails(p_persid)
                If (objRet.existingAnnuityPurchaseDate = String.Empty) Then
                    Session("ExistingAnnuityPurchaseDtNotExists") = True
                Else
                    Session("ExistingAnnuityPurchaseDtNotExists") = False
                End If

            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub TextBoxAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxAmount.TextChanged
        Dim decAmount As Decimal = 0.0
        Dim decRetiredDeathBenefit As Decimal = 0.0
        ''SP 2013.12.13 BT-2326 -Added try catch block
        Try

            If TextBoxAmount.Text.Trim.Length > 0 Then
                If IsNumeric(TextBoxAmount.Text.Trim()) Then
                    decAmount = Convert.ToDecimal(TextBoxAmount.Text.Trim())
                End If
            Else
                TextBoxAmount.Text = "0.00"
            End If

            If TextBoxRetiredBenefit.Text.Length > 0 Then
                If IsNumeric(TextBoxRetiredBenefit.Text.Trim()) Then
                    decRetiredDeathBenefit = Convert.ToDecimal(TextBoxRetiredBenefit.Text.Trim())
                End If
            End If

            If decAmount > 0 Then
                If Math.Round((decRetiredDeathBenefit * 90) / 100, 2) < decAmount Then
                    TextBoxAmount.Text = "0.00"
                    Session("TextBoxAmount.Text") = "0.00"

                    DropDownListPercentage.SelectedValue = 0
                    Session("DropDownListPercentage.SelectedValue") = DropDownListPercentage.SelectedValue
                    'commented by Anudeep on 22-sep for BT-1126
                    'MessageBox.Show(PlaceHolder1, "Error", "Amount to use cannot be greater than 90% of the Retirement Death Benefit.", MessageBoxButtons.Stop, True)
                    'ShowCustomMessage("Amount to use cannot be greater than 90% of the Retirement Death Benefit."), enumMessageBoxType.DotNet, MessageBoxButtons.Stop, "Error")
                    'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_RETIREMENT_DEATH_BENIFIT_AMOUNT"), enumMessageBoxType.DotNet, MessageBoxButtons.Stop, "Error")
                    HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_RETIREMENT_DEATH_BENIFIT_AMOUNT"), EnumMessageTypes.Error, Nothing) 'SP 2013.12.13 BT:2326
                    Exit Sub
                End If
            End If
            Session("PercentageSelected") = False


            If Not Session("PercentageSelected") = True And decAmount = 0 Then
                DropDownListPercentage.SelectedValue = 0
                Session("DropDownListPercentage.SelectedValue") = DropDownListPercentage.SelectedValue
            End If
            Session("TextBoxAmount.Text") = TextBoxAmount.Text.Trim()
            'for alerting & restricting the user, if the user has made any changes and tried to purchase the annuity of a participant
            Me.DataChanged = True

        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("TextBoxAmount_TextChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub TabStripRetireesInformation_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripRetireesInformation.SelectedIndexChange
        Dim dtAddress As DataTable
        Dim drAddress As DataRow()
        Try
            If TabStripRetireesInformation.SelectedIndex = 1 Then
                Me.MultiPageRetirementProcessing.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
                LoadFedWithDrawalTab()
            End If
            If TabStripRetireesInformation.SelectedIndex = 2 Then
                Me.MultiPageRetirementProcessing.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
                LoadGenWithDrawalTab()
            End If
            If TabStripRetireesInformation.SelectedIndex = 6 Then
                Me.MultiPageRetirementProcessing.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
                'START: MMR | 2017.03.06 | YRS-AT-2625 | Open manual transaction dialog if no manual transaction selected on click of purchase tab
                If hdnManualTransaction.Value = "2" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", String.Format("ShowTransactionDialog('{0}');", 2), True)
                Else
                    LoadPurchaseTab()
                    '2012.06.04 SP yrs5.0 -1508
                End If
                'END: MMR | 2017.03.06 | YRS-AT-2625 | Open manual transaction dialog if no manual transaction selected on click of purchase tab
                If CheckBoxRetPlan.Checked = True And Left(TextBoxAnnuitySelectRet.Text.Trim.ToString(), 1).ToUpper() = "J" Then

                    EnableDisableValidationBeneficiary(True)
                Else
                    EnableDisableValidationBeneficiary(False)
                End If
                If CheckBoxSavPlan.Checked = True And Left(TextBoxAnnuitySelectSav.Text.Trim.ToString(), 1).ToUpper() = "J" Then

                    EnableDisableValidationBeneficiarySav(True)
                Else
                    EnableDisableValidationBeneficiarySav(False)
                End If
                '2012.06.04 SP yrs5.0 -1508
            End If
            If TabStripRetireesInformation.SelectedIndex = 4 Then
                'Anudeep:04.07.2013 :BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                If TextBoxAnnuitySSNoRet.Text <> "" Then
                    dtAddress = BeneficiaryAddress
                    If HelperFunctions.isNonEmpty(dtAddress) Then
                        'Anudeep:2014.02.16:BT:2306:YRS 5.0-2251 : Changed logic for checking the the length of the address
                        drAddress = dtAddress.Select("BenSSNo='" & TextBoxAnnuitySSNoRet.Text & "'")
                        If drAddress.Length > 0 And AddressWebUserControlRet.Address1 = "" Then
                            AddressWebUserControlRet.LoadAddressDetail(drAddress)
                        End If
                    End If
                End If
                If TextBoxAnnuitySSNoSav.Text <> "" Then
                    If HelperFunctions.isNonEmpty(dtAddress) Then
                        'Anudeep:2014.02.16:BT:2306:YRS 5.0-2251 : Changed logic for checking the the length of the address
                        drAddress = dtAddress.Select("BenSSNo='" & TextBoxAnnuitySSNoSav.Text & "'")
                        If drAddress.Length > 0 And AddressWebUserControlSav.Address1 = "" Then
                            AddressWebUserControlSav.LoadAddressDetail(drAddress)
                        End If
                    End If
                End If
            End If

            Me.MultiPageRetirementProcessing.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("TabStripRetireesInformation_SelectedIndexChange-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub DataGridNotes_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridNotes.ItemDataBound
        Try
            ''START: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.
            If e.Item.Cells(6).Text.Length > 50 Then
                e.Item.Cells(6).Text = e.Item.Cells(6).Text.Substring(0, 50)
            End If
            ''END: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.
            Dim l_checkbox As New CheckBox
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                l_checkbox = e.Item.FindControl("CheckBoxImportant")
                If l_checkbox.Checked Then
                    Me.NotesFlag.Value = "MarkedImportant"

                End If

                If e.Item.Cells(5).Text.Trim = Session("LoginId") Or NotesGroupUser = True Then
                    'START: SB | 2019.03.15 | BT-12078 | Disable bit important checkbox, as checkbox changes are not saved in database 
                    ' l_checkbox.Enabled = True
                    'END: SB | 2019.03.15 | BT-12078 | Disable bit important checkbox, as checkbox changes are not saved in database 
                End If
            End If
            CheckReadOnlyMode() 'Shilpa N | 03/18/2019 | YRS-AT-2019 | Calling the method
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DataGridNotes_ItemDataBound-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub DatagridPurchase_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridPurchase.ItemDataBound
        Try
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    'SP 2013.12.19 Bt-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process  (increased cell index by 1 because second column is option(selected annuity))
                    monthlyTotal = monthlyTotal + CType(e.Item.Cells(2).Text, Double)
                    regularTotal = regularTotal + CType(e.Item.Cells(3).Text, Double)
                    dividendTotal = dividendTotal + CType(e.Item.Cells(4).Text, Double)
                Case ListItemType.Footer
                    e.Item.Cells(2).Text = monthlyTotal.ToString("#0.00")
                    e.Item.Cells(3).Text = regularTotal.ToString("#0.00")
                    e.Item.Cells(4).Text = dividendTotal.ToString("#0.00")
            End Select
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DatagridPurchase_ItemDataBound-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub DatagridWithheld_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridWithheld.ItemDataBound
        Try
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    'monthlyTotWith = monthlyTotal  - CType(e.Item.Cells(1).Text, Double)  '2012.07.16	SP : BT-753/YRS 5.0-1270 : purchase page -Commented
                    'regularTotWith = regularTotal - CType(e.Item.Cells(2).Text, Double)
                    'dividendTotWith = dividendTotal - CType(e.Item.Cells(3).Text, Double)
                    monthlyTotWith = monthlyTotWith + CType(e.Item.Cells(1).Text, Double)
                    regularTotWith = regularTotWith + CType(e.Item.Cells(2).Text, Double)
                    dividendTotWith = dividendTotWith + CType(e.Item.Cells(3).Text, Double)
                Case ListItemType.Footer
                    'e.Item.Cells(1).Text = monthlyTotWith.ToString("#0.00")  '2012.07.16 SP : BT-753/YRS 5.0-1270 : purchase page -Commented
                    'e.Item.Cells(2).Text = regularTotWith.ToString("#0.00")
                    'e.Item.Cells(3).Text = dividendTotWith.ToString("#0.00")
                    e.Item.Cells(1).Text = (monthlyTotal - monthlyTotWith).ToString("#0.00")
                    e.Item.Cells(2).Text = (regularTotal - regularTotWith).ToString("#0.00")
                    e.Item.Cells(3).Text = (dividendTotal - dividendTotWith).ToString("#0.00")
            End Select
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DatagridWithheld_ItemDataBound-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    Private Sub CheckBoxRetPlan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRetPlan.CheckedChanged
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            Me.enableRetirementPlanControls(CheckBoxRetPlan.Checked)
            If CheckBoxRetPlan.Checked = False And CheckBoxSavPlan.Checked = True Then
                CheckBoxSavPlan.Enabled = False
            ElseIf CheckBoxRetPlan.Checked = True And CheckBoxSavPlan.Checked = True Then
                CheckBoxSavPlan.Enabled = True
            End If
            Me.DataChanged = True 'SP 2014.09.24 YRS 5.0-2362
        Catch ex As Exception
            HelperFunctions.LogException("CheckBoxRetPlan_CheckedChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub CheckBoxSavPlan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxSavPlan.CheckedChanged
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            Me.enableSavingsPlanControls(CheckBoxSavPlan.Checked)
            If CheckBoxSavPlan.Checked = False And CheckBoxRetPlan.Checked = True Then
                CheckBoxRetPlan.Enabled = False
            ElseIf CheckBoxSavPlan.Checked = True And CheckBoxRetPlan.Checked = True Then
                CheckBoxRetPlan.Enabled = True
            End If
            Me.DataChanged = True 'SP 2014.09.24 YRS 5.0-2362
        Catch ex As Exception
            HelperFunctions.LogException("CheckBoxSavPlan_CheckedChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    Private Sub CheckboxIncludeSSLevelling_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckboxIncludeSSLevelling.CheckedChanged
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            If CheckboxIncludeSSLevelling.Checked = True Then
                'TextBoxSSBenefit.Enabled = True
                TextBoxSSBenefit.ReadOnly = False
                Me.DataChanged = True 'SP 2014.09.24 YRS 5.0-2362


            Else
                '''TextBoxSSBenefit.Enabled = False
                TextBoxSSBenefit.ReadOnly = True
                TextBoxSSBenefit.Text = "0.00"
                TextBoxSSIncrease.Text = "0.00"
                resetSSlevelingValues()
            End If
        Catch ex As Exception
            HelperFunctions.LogException("CheckboxIncludeSSLevelling_CheckedChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
#End Region

#Region " New Methods"
    Private Sub enableRetirementPlanControls(ByVal enable As Boolean)

        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            'CheckBoxRetPlan.Enabled = enable
            CheckBoxRetPlan.Checked = enable
            ButtonSelectRet.Enabled = enable

            'START : BD : 2018.11.01 : YRS-AT-4135 : Disable RDB controls on check box checked / Uncked in RDB is restricted
            If Not Session("RDBwarningMessage") Is Nothing Then
                enable = False
            End If
            'END : BD : 2018.11.01 : YRS-AT-4135 : Disable RDB controls on check box checked / Uncked in RDB is restricted
            TextBoxAmount.Enabled = enable
            DropDownListPercentage.Enabled = enable
            TextBoxRetiredBenefit.Enabled = enable
            If enable = False Then
                TextBoxRetiredBenefit.Text = "0.00"
                DropDownListPercentage.SelectedValue = 0
                TextBoxAmount.Text = "0.00"
            Else
                If Session("TextBoxRetiredBenefit.Text") Is Nothing Then
                    TextBoxRetiredBenefit.Text = "0.00"
                Else
                    TextBoxRetiredBenefit.Text = Session("TextBoxRetiredBenefit.Text")
                End If
            End If

            'ASHISH:2011.08.22 YRS 5.0-1345 :BT-859 Death benefit not available for QD participant
            If Me.OrgBenTypeIsQDRO = True Then
                TextBoxAmount.Enabled = False
                DropDownListPercentage.Enabled = False
                TextBoxRetiredBenefit.Enabled = False
                TextBoxRetiredBenefit.Text = "0.00"
                DropDownListPercentage.SelectedValue = 0
                TextBoxAmount.Text = "0.00"

                'Added by Gunanithi - YRS-AT-2676 - 08-Dec-2015, checking if the person's age is less than minimum age of retirement as of restricted date & accordingly enabling/disabling the death benefit to use fields. ----start
            ElseIf Me.IsDeathBenefitAnnuityPurchaseRestricted = True Then
                DropDownListPercentage.Enabled = False
                TextBoxAmount.Enabled = False
            End If
            'Gunanithi, YRS-AT-2676- 08-Dec-2015 ---------end

        Catch
            Throw
        End Try
    End Sub

    Private Sub enableSavingsPlanControls(ByVal enable As Boolean)
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            CheckBoxSavPlan.Checked = enable
            ButtonSelectSav.Enabled = enable
        Catch
            Throw
        End Try
    End Sub

    Private Sub clearRetirementControls()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            TextBoxAnnuitySelectRet.Text = "M"
            TextBoxTaxableRet.Text = String.Empty
            TextBoxNonTaxableRet.Text = String.Empty
            TextBoxTotalPaymentRet.Text = String.Empty
            TextBoxReservesRet.Text = String.Empty
        Catch
            Throw
        End Try
    End Sub

    Private Sub clearSavingsControls()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            TextBoxAnnuitySelectSav.Text = "M"
            TextBoxTaxableSav.Text = String.Empty
            TextBoxNonTaxableSav.Text = String.Empty
            TextBoxTotalPaymentSav.Text = String.Empty
            TextBoxReservesSav.Text = String.Empty
        Catch
            Throw
        End Try
    End Sub
    '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary (commented)-start
    '  Private Sub hideRetAnnuityBenefeciaryControls()
    '      ' Retirement Benefeciary Controls
    ''LabelRetirementBeneficiary.Visible = False
    '      LabelSSNo2Ret.Visible = False
    '      LabelLastName2Ret.Visible = False
    '      LabelFirstName2Ret.Visible = False
    '      LabelMiddleName2Ret.Visible = False
    'LabelBirthDate2Ret.Visible = False
    ''2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary
    '' LabelSpouseRet.Visible = False
    'DropDownRelationShipRet.Visible = False	' BT-975/YRS 5.0-1508 -added
    'RequiredfieldvalidatornRelationShipRet.Enabled = False ' BT-975/YRS 5.0-1508 -added
    '      TextBoxAnnuitySSNoRet.Visible = False
    '      TextBoxAnnuityLastNameRet.Visible = False
    '      TextBoxAnnuityFirstNameRet.Visible = False
    '      TextBoxAnnuityMiddleNameRet.Visible = False
    '      TextBoxAnnuityBirthDateRet.Visible = False
    ''chkSpouseRet.Visible = False '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary

    '      RequiredFieldValidatorAnnuitySSNoRet.Enabled = False
    '      RequiredfieldvalidatorAnnuityFirstNameRet.Enabled = False
    '      RequiredfieldvalidatorAnnuityLastNameRet.Enabled = False
    '      RequiredfieldvalidatorAnnuityBirthDateRet.Enabled = False
    '  End Sub

    'Private Sub hideSavAnnuityBenefeciaryControls()
    '    ' Savings Benefeciary Controls
    '    LabelSavingsBenefeciary.Visible = False
    '    LabelSSNo2Sav.Visible = False
    '    LabelLastName2Sav.Visible = False
    '    LabelFirstName2Sav.Visible = False
    '    LabelMiddleName2Sav.Visible = False
    '    LabelBirthDate2Sav.Visible = False
    '    LabelSpouseSav.Visible = False

    '    TextBoxAnnuitySSNoSav.Visible = False
    '    TextBoxAnnuityLastNameSav.Visible = False
    '    TextBoxAnnuityFirstNameSav.Visible = False
    '    TextBoxAnnuityMiddleNameSav.Visible = False
    '    TextBoxAnnuityBirthDateSav.Visible = False
    '    chkSpouseSav.Visible = False

    '    RequiredFieldValidatorAnnuitySSNoSav.Enabled = False
    '    RequiredfieldvalidatorAnnuityFirstNameSav.Enabled = False
    '    RequiredfieldvalidatorAnnuityLastNameSav.Enabled = False
    '    RequiredfieldvalidatorAnnuityBirthDateSav.Enabled = False

    'End Sub
    '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary (commented)  -end
    Private Sub setControlsAsReadOnly()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            TextBoxSSIncrease.ReadOnly = True
            TextBoxSSDecrease.ReadOnly = True
            TextBoxAnnuitySelectRet.ReadOnly = True '
            TextBoxAnnuitySelectSav.ReadOnly = True '

            TextBoxBirthDateRet.ReadOnly = True '
            TextBoxTaxableRet.ReadOnly = True '
            TextBoxNonTaxableRet.ReadOnly = True '
            TextBoxTotalPaymentRet.ReadOnly = True '
            TextBoxReservesRet.ReadOnly = True '

            TextBoxTaxableSav.ReadOnly = True '
            TextBoxNonTaxableSav.ReadOnly = True '
            TextBoxTotalPaymentSav.ReadOnly = True '
            TextBoxReservesSav.ReadOnly = True '
        Catch
            Throw
        End Try
    End Sub

    Private Sub loadControlsFromSessionVariables()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            TextBoxRetiredBenefit.Text = Session("TextBoxRetiredBenefit.Text")

            TextBoxAnnuityBirthDateRet.Text = Session("TextBoxAnnuityBirthDateRet.Text")
            TextBoxAnnuityFirstNameRet.Text = Session("TextBoxAnnuityFirstNameRet.Text")
            TextBoxAnnuityLastNameRet.Text = Session("TextBoxAnnuityLastNameRet.Text")
            TextBoxAnnuityMiddleNameRet.Text = Session("TextBoxAnnuityMiddleNameRet.Text")
            TextBoxAnnuitySSNoRet.Text = Session("TextBoxAnnuitySSNoRet.Text")

            '2012.06.04  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
            'Commented exisitng code for issue-1508
            'chkSpouseRet.Checked = Session("chkSpouseRet.Checked")

            TextBoxAnnuityBirthDateSav.Text = Session("TextBoxAnnuityBirthDateSav.Text")
            TextBoxAnnuityFirstNameSav.Text = Session("TextBoxAnnuityFirstNameSav.Text")
            TextBoxAnnuityLastNameSav.Text = Session("TextBoxAnnuityLastNameSav.Text")
            TextBoxAnnuityMiddleNameSav.Text = Session("TextBoxAnnuityMiddleNameSav.Text")
            TextBoxAnnuitySSNoSav.Text = Session("TextBoxAnnuitySSNoSav.Text")
            'chkSpouseSav.Checked = Session("chkSpouseSav.Checked")
            '2012.06.04  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End

            TextBoxBirthDateRet.Text = Session("TextBoxBirthDateRet.Text")
            TextBoxAmount.Text = Session("TextBoxAmount.Text")
            TextBoxReservesRet.Text = Session("TextBoxReservesRet.Text")
            TextBoxSSBenefit.Text = Session("TextBoxSSBenefit.Text")
            TextBoxSSIncrease.Text = Session("TextBoxSSIncrease.Text")

            DropDownListPercentage.SelectedValue = Session("DropDownListPercentage.SelectedValue")
            'DropDownListRetirementType.SelectedValue = Session("DropDownListRetirementType.SelectedValue")
            ListBoxPercentage.SelectedValue = DropDownListPercentage.SelectedValue

            TextBoxAnnuitySelectRet.Text = Session("TextBoxAnnuitySelectRet.Text")
            TextBoxTaxableRet.Text = Session("TextBoxTaxableRet.Text")
            TextBoxNonTaxableRet.Text = Session("TextBoxNonTaxableRet.Text")
            TextBoxTotalPaymentRet.Text = Session("TextBoxTotalPaymentRet.Text")

            TextBoxAnnuitySelectSav.Text = Session("TextBoxAnnuitySelectSav.Text")
            TextBoxTaxableSav.Text = Session("TextBoxTaxableSav.Text")
            TextBoxNonTaxableSav.Text = Session("TextBoxNonTaxableSav.Text")
            TextBoxTotalPaymentSav.Text = Session("TextBoxTotalPaymentSav.Text")
        Catch
            Throw
        End Try
    End Sub

    Private Sub saveControlValuesInSessionVariables()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            Session("TextBoxRetiredBenefit.Text") = TextBoxRetiredBenefit.Text
            Session("TextBoxAnnuityBirthDateRet.Text") = TextBoxAnnuityBirthDateRet.Text
            Session("TextBoxAnnuityFirstNameRet.Text") = TextBoxAnnuityFirstNameRet.Text
            Session("TextBoxAnnuityLastNameRet.Text") = TextBoxAnnuityLastNameRet.Text
            Session("TextBoxAnnuityMiddleNameRet.Text") = TextBoxAnnuityMiddleNameRet.Text
            Session("TextBoxAnnuitySSNoRet.Text") = TextBoxAnnuitySSNoRet.Text

            '2012.06.04  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
            'Commented existing code for issue 1508
            'Session("chkSpouseRet.Checked") = chkSpouseRet.Checked

            Session("TextBoxAnnuityBirthDateSav.Text") = TextBoxAnnuityBirthDateSav.Text
            Session("TextBoxAnnuityFirstNameSav.Text") = TextBoxAnnuityFirstNameSav.Text
            Session("TextBoxAnnuityLastNameSav.Text") = TextBoxAnnuityLastNameSav.Text
            Session("TextBoxAnnuityMiddleNameSav.Text") = TextBoxAnnuityMiddleNameSav.Text
            Session("TextBoxAnnuitySSNoSav.Text") = TextBoxAnnuitySSNoSav.Text
            'Session("chkSpouseSav.Checked") = chkSpouseSav.Checked
            '2012.06.04  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start

            Session("TextBoxBirthDateRet.Text") = TextBoxBirthDateRet.Text
            Session("TextBoxAmount.Text") = TextBoxAmount.Text

            Session("TextBoxReservesRet.Text") = TextBoxReservesRet.Text
            Session("TextBoxSSBenefit.Text") = TextBoxSSBenefit.Text
            Session("TextBoxSSIncrease.Text") = TextBoxSSIncrease.Text

            Session("DropDownListPercentage.SelectedValue") = DropDownListPercentage.SelectedValue
            'Session("DropDownListRetirementType.SelectedValue") = DropDownListRetirementType.SelectedValue

            Session("TextBoxAnnuitySelectRet.Text") = TextBoxAnnuitySelectRet.Text
            Session("TextBoxTaxableRet.Text") = TextBoxTaxableRet.Text
            Session("TextBoxNonTaxableRet.Text") = TextBoxNonTaxableRet.Text
            Session("TextBoxTotalPaymentRet.Text") = TextBoxTotalPaymentRet.Text

            Session("TextBoxAnnuitySelectSav.Text") = TextBoxAnnuitySelectSav.Text
            Session("TextBoxTaxableSav.Text") = TextBoxTaxableSav.Text
            Session("TextBoxNonTaxableSav.Text") = TextBoxNonTaxableSav.Text
            Session("TextBoxTotalPaymentSav.Text") = TextBoxTotalPaymentSav.Text
        Catch
            Throw
        End Try
    End Sub

    Private Sub getFundBalance()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            Session("RetirementBalance") = RetirementBOClass.GetRetirementBalance(Me.guiFundEventId, Me.RetirementDate)
            Session("SavingsBalance") = RetirementBOClass.GetSavingsBalance(Me.guiFundEventId, Me.RetirementDate)
        Catch
            Throw
        End Try
    End Sub

    Private Sub loadAnnuityControls()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            getFundBalance()
            ' If FundStatus is RT then apply 5000 check only if the participant has not previously retired with that plan
            ' otherwise check if any fund is left
            Dim businessLogic As RetirementBOClass = New RetirementBOClass
            Dim dtPurchasedAnnuity As DataTable = businessLogic.GetPurchasedAnnuityDetails(Session("PersID"))
            'as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
            'If Me.FundEventStatus = "RT" Then
            If Me.FundEventStatus = "RT" Or Me.FundEventStatus = "RPT" Then
                ' If (Convert.ToDateTime(businessLogic.RetirementDate) >= Session("PlanSplitDate")) Then
                If (Convert.ToDateTime(businessLogic.existingAnnuityPurchaseDate) >= Session("PlanSplitDate")) Then
                    If businessLogic.RetiredOnRetirementPlan Then
                        enableRetirementPlanControls(businessLogic.RetiredOnRetirementPlan And Session("RetirementBalance") > 0)
                        CheckBoxRetPlan.Enabled = (businessLogic.RetiredOnRetirementPlan And Session("RetirementBalance") > 0)
                    Else
                        enableRetirementPlanControls(Session("RetirementBalance") >= 5000)
                        CheckBoxRetPlan.Enabled = (Session("RetirementBalance") >= 5000)
                    End If

                    If businessLogic.RetiredOnSavingsPlan Then
                        enableSavingsPlanControls(businessLogic.RetiredOnSavingsPlan And Session("SavingsBalance") > 0)
                        CheckBoxSavPlan.Enabled = (businessLogic.RetiredOnSavingsPlan And Session("SavingsBalance") > 0)
                    Else
                        enableSavingsPlanControls(Session("SavingsBalance") >= 5000)
                        CheckBoxSavPlan.Enabled = (Session("SavingsBalance") >= 5000)
                    End If
                Else
                    enableRetirementPlanControls(Session("RetirementBalance") > 0)
                    CheckBoxRetPlan.Enabled = (Session("RetirementBalance") > 0)

                    enableSavingsPlanControls(Session("SavingsBalance") > 0)
                    CheckBoxSavPlan.Enabled = (Session("SavingsBalance") > 0)
                End If


            ElseIf Me.FundEventStatus = "RD" Or Me.FundEventStatus = "RP" Then
                If IsPrePlanSplitRetirement = False Then
                    ' It is assumed that the participant has retired on both plans and hence the 5000 check will not be applied on both the plans
                    ' Allow to retire if balance is present.
                    enableRetirementPlanControls(Session("RetirementBalance") > 0)
                    CheckBoxRetPlan.Enabled = (Session("RetirementBalance") > 0)

                    enableSavingsPlanControls(Session("SavingsBalance") > 0)
                    CheckBoxSavPlan.Enabled = (Session("SavingsBalance") > 0)
                Else ' PrePlanSplit
                    ' Retirement controls
                    Dim totalSum As Decimal = 0.0
                    totalSum = Session("RetirementBalance") + Session("SavingsBalance")
                    Dim enableRetirement As Boolean = (totalSum > 0)
                    enableRetirementPlanControls(enableRetirement)
                    CheckBoxRetPlan.Enabled = enableRetirement

                    ' Savings controls
                    enableSavingsPlanControls(False)
                    If Session("SavingsBalance") > 0 Then
                        TextBoxAnnuitySelectSav.Text = TextBoxAnnuitySelectRet.Text
                        CheckBoxSavPlan.Checked = True
                    Else
                        TextBoxAnnuitySelectSav.Text = String.Empty
                        CheckBoxSavPlan.Checked = False
                    End If

                    CheckBoxSavPlan.Enabled = False
                End If

            ElseIf IsPrePlanSplitRetirement = False Then ' Fund status is not RT then apply 5000 check on both plans
                ' Retirement controls
                Dim enableRetirement As Boolean = (Session("RetirementBalance") >= 5000)
                enableRetirementPlanControls(enableRetirement)
                CheckBoxRetPlan.Enabled = enableRetirement

                ' Savings controls
                Dim enableSavings As Boolean = Session("SavingsBalance") >= 5000
                enableSavingsPlanControls(enableSavings)
                CheckBoxSavPlan.Enabled = enableSavings

            Else
                ' Retirement controls
                Dim totalSum As Decimal = 0.0
                totalSum = Session("RetirementBalance") + Session("SavingsBalance")
                Dim enableRetirement As Boolean = (totalSum >= 5000)
                enableRetirementPlanControls(enableRetirement)
                CheckBoxRetPlan.Enabled = enableRetirement

                ' Savings controls
                'Dim enableSavings As Boolean = Session("SavingsBalance") >= 5000
                enableSavingsPlanControls(False)
                If Session("SavingsBalance") > 0 Then
                    TextBoxAnnuitySelectSav.Text = TextBoxAnnuitySelectRet.Text
                    CheckBoxSavPlan.Checked = True
                Else
                    TextBoxAnnuitySelectSav.Text = String.Empty
                    CheckBoxSavPlan.Checked = False
                End If
                CheckBoxSavPlan.Enabled = False
            End If

            ' If only one plan is available for retirement then disable the check box to De-Select it
            If CheckBoxRetPlan.Enabled = True And CheckBoxSavPlan.Enabled = False Then
                CheckBoxRetPlan.Enabled = False
            End If

            If CheckBoxRetPlan.Enabled = False And CheckBoxSavPlan.Enabled = True Then
                CheckBoxSavPlan.Enabled = False
            End If

            '' Check if SS anuity has been bought or not
            Session("ssAnnuityAlreadyBought") = False
            If dtPurchasedAnnuity.Rows.Count > 0 Then
                For Each dr As DataRow In dtPurchasedAnnuity.Rows
                    If dr("SSLeveling").ToString.Trim = True Then
                        Session("ssAnnuityAlreadyBought") = True
                        CheckboxIncludeSSLevelling.Checked = False
                        CheckboxIncludeSSLevelling.Enabled = False
                    End If
                Next
            End If



            '' If age of the participant is >= 62 then disable SSLevelling controls
            Dim age As Decimal = Convert.ToDecimal(DateDiff(DateInterval.Month, Convert.ToDateTime(TextBoxBirthDateRet.Text), Convert.ToDateTime(TextBoxRetirementDate.Text)) / 12).ToString()
            If age >= 62 Then 'Or age < 55 Then
                CheckboxIncludeSSLevelling.Checked = False
                CheckboxIncludeSSLevelling.Enabled = False

                Call CheckboxIncludeSSLevelling_CheckedChanged(New Object, New System.EventArgs)
            Else
                If CheckboxIncludeSSLevelling.Enabled = False Then
                    CheckboxIncludeSSLevelling.Enabled = True
                    CheckboxIncludeSSLevelling.Checked = False
                End If
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Function getElectiveAccounts(ByVal planType As String) As DataSet
        Try

            Dim dsElectiveAccounts As DataSet
            dsElectiveAccounts = BORetireeEstimate.SearchElectiveAccounts(Me.PersId)

            Dim electiveAccounts As DataSet
            electiveAccounts = dsElectiveAccounts.Clone()
            ' Get the Retirement accounts
            If planType = "R" Or planType = "B" Then
                For Each dr As DataRow In dsElectiveAccounts.Tables(0).Rows
                    If dr("PlanType") = "RETIREMENT" Then
                        electiveAccounts.Tables(0).ImportRow(dr)
                    End If
                Next
            End If

            ' Get the Savings accounts
            If planType = "S" Or planType = "B" Then
                For Each dr As DataRow In dsElectiveAccounts.Tables(0).Rows
                    If dr("PlanType") = "SAVINGS" Then
                        electiveAccounts.Tables(0).ImportRow(dr)
                    End If
                Next
            End If

            electiveAccounts.AcceptChanges()
            getElectiveAccounts = electiveAccounts

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub populateCheckValues()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            TextBoxMonthlyGrossAnnuity.Text = Convert.ToDecimal(Session("TextBoxMonthlyGrossAnnuityRet.Text")) + Convert.ToDecimal(Session("TextBoxMonthlyGrossAnnuitySav.Text"))
            TextBoxMonthlyGrossDB.Text = Convert.ToDecimal(Session("TextBoxMonthlyGrossDBRet.Text")) '+ Convert.ToDecimal(Session("TextBoxMonthlyGrossDBSav.Text"))
            TextBoxMonthlyGrossTotal.Text = Convert.ToDecimal(TextBoxMonthlyGrossAnnuity.Text) + Convert.ToDecimal(TextBoxMonthlyGrossDB.Text)
        Catch
            Throw
        End Try
    End Sub

    Private Sub setNotesTabColor()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            Me.NotesFlag.Value = String.Empty

            If Me.Notes.Rows.Count > 0 Then
                Me.NotesFlag.Value = "Notes"
            End If

            Dim drRows As DataRow() = Me.Notes.Select("bitImportant = 1")
            If drRows.Length > 0 Then
                Me.NotesFlag.Value = "MarkedImportant"
            End If

            Dim drRow As DataRow
            For Each drRow In Me.Notes.Rows
                If drRow("bitImportant") = 1 Then
                    Me.NotesFlag.Value = "MarkedImportant"
                    Exit For
                End If
            Next

            If Me.NotesFlag.Value = "Notes" Then
                Me.TabStripRetireesInformation.Items(5).Text = "<font color=orange>Notes</font>"
            ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
                Me.TabStripRetireesInformation.Items(5).Text = "<font color=red>Notes</font>"
            Else
                Me.TabStripRetireesInformation.Items(5).Text = "Notes"
            End If
        Catch
            Throw
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Display the most probable Benefeciary in the Joint Annuity TAB.
    ''' This benefeciary is only for Display purpose and can be changed by the user.
    ''' </summary>
    ''' <param name="annuitySelected"></param>
    ''' <param name="planSelected"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Asween]	8/30/2007	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    '  Private Sub loadJointAnnuityBenefeciaryTab(ByVal annuitySelected As Boolean, ByVal planSelected As String)
    '      Dim selectedRetirementAnnuity As String = TextBoxAnnuitySelectRet.Text.Trim().ToUpper()
    'Dim selectedSavingsAnnuity As String = TextBoxAnnuitySelectSav.Text.Trim().ToUpper()
    ''2012.05.28  SP:  BT-975/YRS 5.0-1508 - Add grid
    'Dim foundRows As DataRow()
    'Dim l_button_select As ImageButton
    ''2012.05.28  SP:  BT-975/YRS 5.0-1508 - Add grid

    '      If annuitySelected = True And (Left(selectedRetirementAnnuity, 1) = "J" Or Left(selectedSavingsAnnuity, 1) = "J") Then
    '          Dim dsBeneficiaryInfo As New DataSet

    '          'dsBeneficiaryInfo = RetirementBOClass.SearchRetEstInfo(Me.PersId)
    '	dsBeneficiaryInfo = RetirementBOClass.getParticipantBeneficiaries(Me.PersId)
    '	'2012.05.28  SP:  BT-975/YRS 5.0-1508 - Add grid
    '	If HelperFunctions.isNonEmpty(dsBeneficiaryInfo) Then
    '		foundRows = dsBeneficiaryInfo.Tables(0).Select("chvRelationshipCode in('ES','IN','TR')")
    '		For Each drRow As DataRow In foundRows
    '			dsBeneficiaryInfo.Tables(0).Rows.Remove(drRow)
    '		Next
    '		dsBeneficiaryInfo.AcceptChanges()
    '	End If
    '	'2012.05.28  SP:  BT-975/YRS 5.0-1508 - Add grid

    '	Dim drBenCollection As DataRow()
    '	Dim drBen As DataRow

    '	Me.BeneficiaryInfo = dsBeneficiaryInfo

    '	If dsBeneficiaryInfo.Tables(0).Rows.Count > 0 Then
    '		'show annuity beneficiary details for all the J & S Annuity types
    '		'If planSelected = "R" And Left(selectedRetirementAnnuity, 1) = "J" Then
    '		If planSelected = "R" Then
    '			If Left(selectedRetirementAnnuity, 1) = "J" Then
    '				'making controls visible for annuity beneficiary
    '				'LabelRetirementBeneficiary.Visible = True 'Commented for issue 1508

    '				LabelSSNo2Ret.Visible = True
    '				LabelLastName2Ret.Visible = True
    '				LabelFirstName2Ret.Visible = True
    '				LabelMiddleName2Ret.Visible = True
    '				LabelBirthDate2Ret.Visible = True
    '				'LabelSpouseRet.Visible = True'2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary
    '				'2012.05.18  SP:  BT-975/YRS 5.0-1508  -Added 
    '				RequiredfieldvalidatornRelationShipRet.Enabled = True
    '				DropDownRelationShipRet.Visible = True
    '				'2012.05.18  SP:  BT-975/YRS 5.0-1508  -Added

    '				TextBoxAnnuitySSNoRet.Visible = True
    '				TextBoxAnnuityLastNameRet.Visible = True
    '				TextBoxAnnuityFirstNameRet.Visible = True
    '				TextBoxAnnuityMiddleNameRet.Visible = True
    '				TextBoxAnnuityBirthDateRet.Visible = True

    '				RequiredFieldValidatorAnnuitySSNoRet.Enabled = True
    '				RequiredfieldvalidatorAnnuityFirstNameRet.Enabled = True
    '				RequiredfieldvalidatorAnnuityLastNameRet.Enabled = True
    '				RequiredfieldvalidatorAnnuityBirthDateRet.Enabled = True

    '				'chkSpouseRet.Visible = True '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary

    '				drBenCollection = dsBeneficiaryInfo.Tables(0).Select("chvRelationshipCode = 'SP' AND chvBeneficiaryTypeCode = 'MEMBER'")
    '				If drBenCollection.Length > 0 Then
    '					drBen = drBenCollection(0)
    '					If Not drBen("chrBeneficiaryTaxNumber").ToString() Is DBNull.Value Then
    '						If Not drBen("chrBeneficiaryTaxNumber").ToString() = String.Empty Then
    '							TextBoxAnnuitySSNoRet.Text = drBen("chrBeneficiaryTaxNumber").ToString()
    '						End If
    '					End If

    '					If Not drBen("BenLastName").ToString() Is DBNull.Value Then
    '						If Not drBen("BenLastName").ToString() = String.Empty Then
    '							TextBoxAnnuityLastNameRet.Text = drBen("BenLastName").ToString()
    '						End If
    '					End If

    '					If Not drBen("BenFirstName").ToString() Is DBNull.Value Then
    '						If Not drBen("BenFirstName").ToString() = String.Empty Then
    '							TextBoxAnnuityFirstNameRet.Text = drBen("BenFirstName").ToString()
    '						End If
    '					End If

    '					If Not drBen("BenBirthDate").ToString() Is DBNull.Value Then
    '						If Not drBen("BenBirthDate").ToString() = String.Empty Then
    '							TextBoxAnnuityBirthDateRet.Text = Convert.ToDateTime(drBen("BenBirthDate")).ToString("MM/dd/yyyy")
    '						End If
    '					End If

    '					'2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
    '					'If Not drBen("chvRelationshipCode").ToString() Is DBNull.Value Then
    '					'    If drBen("chvRelationshipCode").ToString.Trim.ToUpper() = "SP" Then
    '					'        chkSpouseRet.Checked = True
    '					'    End If
    '					'End If
    '					If Not drBen("chvRelationshipCode").ToString() Is DBNull.Value Then
    '						DropDownRelationShipRet.SelectedValue = drBen("chvRelationshipCode").ToString.Trim()
    '					End If
    '					Dim i As Int32
    '					While i < Me.DataGridAnnuityBeneficiaries.Items.Count
    '						l_button_select = New ImageButton
    '						l_button_select = Me.DataGridAnnuityBeneficiaries.Items(i).FindControl("ImageButtonAccounts")
    '						If Me.DataGridAnnuityBeneficiaries.Items(i).Cells(1).Text.Trim().ToUpper() = drBen("guiUniqueID").ToString().ToUpper() Then
    '							If Not l_button_select Is Nothing Then
    '								l_button_select.ImageUrl = "images\selected.gif"
    '							End If
    '						Else
    '							If Not l_button_select Is Nothing Then
    '								l_button_select.ImageUrl = "images\select.gif"
    '							End If

    '						End If
    '						i = i + 1
    '					End While
    '					'2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End
    '					'Else
    '					'	drBenCollection = dsBeneficiaryInfo.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg=100 AND chvBeneficiaryTypeCode = 'MEMBER'")
    '					'	If drBenCollection.Length > 0 Then
    '					'		drBen = drBenCollection(0)
    '					'	End If
    '				End If


    '				'Else
    '				'hideRetAnnuityBenefeciaryControls()
    '			End If
    '		End If

    '		'2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
    '		'Commented existing code for issue -1508
    '		' Check for Savings Plan
    '		'If planSelected = "S" And Left(selectedSavingsAnnuity, 1) = "J" Then
    '		'If planSelected = "S" Then
    '		'    'making controls visible for annuity beneficiary
    '		'    If Left(selectedSavingsAnnuity, 1) = "J" And IsPrePlanSplitRetirement = False Then
    '		'        LabelSavingsBenefeciary.Visible = True

    '		'        LabelSSNo2Sav.Visible = True
    '		'        LabelLastName2Sav.Visible = True
    '		'        LabelFirstName2Sav.Visible = True
    '		'        LabelMiddleName2Sav.Visible = True
    '		'        LabelBirthDate2Sav.Visible = True
    '		'        LabelSpouseSav.Visible = True

    '		'        TextBoxAnnuitySSNoSav.Visible = True
    '		'        TextBoxAnnuityLastNameSav.Visible = True
    '		'        TextBoxAnnuityFirstNameSav.Visible = True
    '		'        TextBoxAnnuityMiddleNameSav.Visible = True
    '		'        TextBoxAnnuityBirthDateSav.Visible = True
    '		'        chkSpouseSav.Visible = True

    '		'        RequiredFieldValidatorAnnuitySSNoSav.Enabled = True
    '		'        RequiredfieldvalidatorAnnuityFirstNameSav.Enabled = True
    '		'        RequiredfieldvalidatorAnnuityLastNameSav.Enabled = True
    '		'        RequiredfieldvalidatorAnnuityBirthDateSav.Enabled = True

    '		'        'Get the Savings benefeciary
    '		'        drBenCollection = dsBeneficiaryInfo.Tables(0).Select("chvRelationshipCode = 'SP' AND chvBeneficiaryTypeCode = 'SAVING'")
    '		'        If drBenCollection.Length > 0 Then
    '		'            drBen = drBenCollection(0)
    '		'        Else
    '		'            drBenCollection = dsBeneficiaryInfo.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg=100 AND chvBeneficiaryTypeCode = 'SAVING'")
    '		'            If drBenCollection.Length > 0 Then
    '		'                drBen = drBenCollection(0)
    '		'            End If
    '		'        End If

    '		'        ' Now check if there are no Savings Benefeciary then get the Retirement benefeciary
    '		'        If drBenCollection.Length <= 0 Then
    '		'            drBenCollection = dsBeneficiaryInfo.Tables(0).Select("chvRelationshipCode = 'SP' AND chvBeneficiaryTypeCode = 'MEMBER'")
    '		'            If drBenCollection.Length > 0 Then
    '		'                drBen = drBenCollection(0)
    '		'            Else
    '		'                drBenCollection = dsBeneficiaryInfo.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg=100 AND chvBeneficiaryTypeCode = 'MEMBER'")
    '		'                If drBenCollection.Length > 0 Then
    '		'                    drBen = drBenCollection(0)
    '		'                End If
    '		'            End If
    '		'        End If

    '		'        If drBenCollection.Length > 0 Then
    '		'            If Not drBen("chrBeneficiaryTaxNumber").ToString() Is DBNull.Value Then
    '		'                If Not drBen("chrBeneficiaryTaxNumber").ToString() = String.Empty Then
    '		'                    TextBoxAnnuitySSNoSav.Text = drBen("chrBeneficiaryTaxNumber").ToString()
    '		'                End If
    '		'            End If

    '		'            If Not drBen("BenLastName").ToString() Is DBNull.Value Then
    '		'                If Not drBen("BenLastName").ToString() = String.Empty Then
    '		'                    TextBoxAnnuityLastNameSav.Text = drBen("BenLastName").ToString()
    '		'                End If
    '		'            End If

    '		'            If Not drBen("BenFirstName").ToString() Is DBNull.Value Then
    '		'                If Not drBen("BenFirstName").ToString() = String.Empty Then
    '		'                    TextBoxAnnuityFirstNameSav.Text = drBen("BenFirstName").ToString()
    '		'                End If
    '		'            End If

    '		'            If Not drBen("BenBirthDate").ToString() Is DBNull.Value Then
    '		'                If Not drBen("BenBirthDate").ToString() = String.Empty Then
    '		'                    TextBoxAnnuityBirthDateSav.Text = Convert.ToDateTime(drBen("BenBirthDate")).ToString("MM/dd/yyyy")
    '		'                End If
    '		'            End If

    '		'            If Not drBen("chvRelationshipCode").ToString() Is DBNull.Value Then
    '		'                If drBen("chvRelationshipCode").ToString.Trim.ToUpper() = "SP" Then
    '		'                    chkSpouseSav.Checked = True
    '		'                End If
    '		'            End If
    '		'        End If
    '		'    Else
    '		'        hideSavAnnuityBenefeciaryControls()
    '		'    End If
    '		'End If
    '		'2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End
    '	End If

    '	'2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary-Start
    '	'Commented existing code for issue -1508
    '	'If IsPrePlanSplitRetirement Then
    '	'    LabelRetirementBeneficiary.Text = String.Empty
    '	'Else
    '	'    LabelRetirementBeneficiary.Text = "Retirement Plan"
    '	'End If
    '	'2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End

    '	'as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
    '	'If Me.FundEventStatus = "RT" Or Me.FundEventStatus = "RD" Or Me.FundEventStatus = "RP" Then
    '	If Me.FundEventStatus = "RT" Or Me.FundEventStatus = "RPT" Or Me.FundEventStatus = "RD" Or Me.FundEventStatus = "RP" Then
    '		Me.loadJointSurvivor(planSelected)
    '	End If
    'Else
    '	'hideRetAnnuityBenefeciaryControls()
    '	'hideSavAnnuityBenefeciaryControls() '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary 
    'End If
    '  End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' If a Joint Annuity survivor exists then Display him as the default survivor 
    ''' </summary>
    ''' <param name="planSelected"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Asween]	8/30/2007	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    ' Private Sub loadJointSurvivor(ByVal planSelected As String)

    '     Dim dsBeneficiaryInfo As New DataSet
    '     dsBeneficiaryInfo = RetirementBOClass.GetJointAnnuitySurvivors(Me.PersId)

    '     Dim drBenCollection As DataRow()
    '     Dim drBen As DataRow

    '     If dsBeneficiaryInfo Is Nothing Or dsBeneficiaryInfo.Tables.Count <= 0 Then
    '         Exit Sub
    '     End If

    '     If dsBeneficiaryInfo.Tables(0).Rows.Count > 0 Then
    '         'show annuity beneficiary details for all the J & S Annuity types
    '         Dim selectedRetirementAnnuity As String = TextBoxAnnuitySelectRet.Text.Trim().ToUpper()
    '         If planSelected = "R" And Left(selectedRetirementAnnuity, 1) = "J" Then
    '             drBen = dsBeneficiaryInfo.Tables(0).Rows(0)
    '             'If drBenCollection.Length > 0 Then
    '             If Not drBen("SSNo").ToString() Is DBNull.Value Then
    '                 If Not drBen("SSNo").ToString() = String.Empty Then
    '                     TextBoxAnnuitySSNoRet.Text = drBen("SSNo").ToString()
    '                 End If
    '             End If

    '             If Not drBen("LastName").ToString() Is DBNull.Value Then
    '                 If Not drBen("LastName").ToString() = String.Empty Then
    '                     TextBoxAnnuityLastNameRet.Text = drBen("LastName").ToString()
    '                 End If
    '             End If

    '             If Not drBen("FirstName").ToString() Is DBNull.Value Then
    '                 If Not drBen("FirstName").ToString() = String.Empty Then
    '                     TextBoxAnnuityFirstNameRet.Text = drBen("FirstName").ToString()
    '                 End If
    '             End If

    '             If Not drBen("MiddleName").ToString() Is DBNull.Value Then
    '                 If Not drBen("MiddleName").ToString() = String.Empty Then
    '                     TextBoxAnnuityMiddleNameRet.Text = drBen("MiddleName").ToString()
    '                 End If
    '             End If

    '             If Not drBen("BirthDate").ToString() Is DBNull.Value Then
    '                 If Not drBen("BirthDate").ToString() = String.Empty Then
    '                     TextBoxAnnuityBirthDateRet.Text = Convert.ToDateTime(drBen("BirthDate")).ToString("MM/dd/yyyy")
    '                 End If
    '             End If

    '	'2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
    '	'If Not drBen("Spouse").ToString() Is DBNull.Value Then
    '	'    If drBen("Spouse").ToString() = "1" Then
    '	'        chkSpouseRet.Checked = True
    '	'    End If
    '	'End If
    '	If Not drBen("Spouse").ToString() Is DBNull.Value Then
    '		If drBen("Spouse").ToString() = "1" Then
    '			DropDownRelationShipRet.SelectedValue = "SP"
    '		End If
    '	End If
    '	'2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End

    '         End If

    ''2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
    '         ' Check for Savings Plan
    ''         Dim selectedSavingsAnnuity As String = TextBoxAnnuitySelectSav.Text.Trim().ToUpper()
    ''         If planSelected = "S" And Left(selectedSavingsAnnuity, 1) = "J" Then
    ''             'Get the Savings benefeciary
    ''             drBen = dsBeneficiaryInfo.Tables(0).Rows(0)

    ''             'If drBenCollection.Length > 0 Then
    ''             If Not drBen("SSNo").ToString() Is DBNull.Value Then
    ''                 If Not drBen("SSNo").ToString() = String.Empty Then
    ''                     TextBoxAnnuitySSNoSav.Text = drBen("SSNo").ToString()
    ''                 End If
    ''             End If

    ''             If Not drBen("LastName").ToString() Is DBNull.Value Then
    ''                 If Not drBen("LastName").ToString() = String.Empty Then
    ''                     TextBoxAnnuityLastNameSav.Text = drBen("LastName").ToString()
    ''                 End If
    ''             End If

    ''             If Not drBen("FirstName").ToString() Is DBNull.Value Then
    ''                 If Not drBen("FirstName").ToString() = String.Empty Then
    ''                     TextBoxAnnuityFirstNameSav.Text = drBen("FirstName").ToString()
    ''                 End If
    ''             End If

    ''             If Not drBen("MiddleName").ToString() Is DBNull.Value Then
    ''                 If Not drBen("MiddleName").ToString() = String.Empty Then
    ''                     TextBoxAnnuityMiddleNameSav.Text = drBen("MiddleName").ToString()
    ''                 End If
    ''             End If

    ''             If Not drBen("BirthDate").ToString() Is DBNull.Value Then
    ''                 If Not drBen("BirthDate").ToString() = String.Empty Then
    ''                     TextBoxAnnuityBirthDateSav.Text = Convert.ToDateTime(drBen("BirthDate")).ToString("MM/dd/yyyy")
    ''                 End If
    ''             End If

    ''             If Not drBen("Spouse").ToString() Is DBNull.Value Then
    ''                 If drBen("Spouse").ToString() = "1" Then
    ''                     chkSpouseSav.Checked = True
    ''                 End If
    ''             End If
    ''End If
    ''2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End

    '     End If
    ' End Sub

    Private Sub loadSSLevellingControls()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            If (CheckboxIncludeSSLevelling.Checked = True) And (Convert.ToDouble(TextBoxSSBenefit.Text) > 0) Then

                ' If the age is less than 55 then dont allow SS leveling option
                Dim age As Decimal = Convert.ToDecimal(DateDiff(DateInterval.Month, Convert.ToDateTime(TextBoxBirthDateRet.Text), Convert.ToDateTime(TextBoxRetirementDate.Text)) / 12).ToString()
                If age < 55 Then
                    CheckboxIncludeSSLevelling.Checked = False
                    TextBoxSSBenefit.ReadOnly = True
                    Me.TextBoxSSBenefit.Text = "0.00"
                    Me.TextBoxSSIncrease.Text = "0.00"
                    Session("resetSSlevelingValues") = "True"
                    'commented by Anudeep on 22-sep for BT-1126
                    'ShowCustomMessage("The Social Security Leveling option is not available for persons under age 55."), enumMessageBoxType.DotNet, MessageBoxButtons.OK, True)
                    'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_SOCIAL_SECURITY_LEVEL_OPTION_UNAVAILABLE"), enumMessageBoxType.DotNet, MessageBoxButtons.OK, True)
                    HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_SOCIAL_SECURITY_LEVEL_OPTION_UNAVAILABLE"), EnumMessageTypes.Error, Nothing)    'SP 2013.12.13 BT:2326
                    Exit Sub
                End If

                'Dim existAnnutiies As Decimal = RetirementBOClass.GetExistingAnnuities(Me.guiFundEventId)
                Dim existAnnutyDetails As DataTable = RetirementBOClass.GetExistingAnnuities(Me.guiFundEventId)
                Dim existAnnutiies As Decimal = Convert.ToDecimal(existAnnutyDetails.Rows(0)("ExistingAnnuities").ToString())
                Dim existingSSLevelling As Decimal = Convert.ToDecimal(existAnnutyDetails.Rows(0)("SSLevelling").ToString())
                Dim existingSSBenefit As Decimal = Convert.ToDecimal(existAnnutyDetails.Rows(0)("SSBenefit").ToString())

                TextboxExistingAnnuities.Text = existAnnutiies.ToString("#0.00")
                Dim currentPurchase As Decimal = 0
                If CheckBoxRetPlan.Checked = True Then
                    currentPurchase = Convert.ToDecimal(TextBoxTotalPaymentRet.Text)
                End If
                If CheckBoxSavPlan.Checked = True Then
                    currentPurchase = currentPurchase + Convert.ToDecimal(TextBoxTotalPaymentSav.Text)
                End If
                TextboxCurrentPurchase.Text = currentPurchase
                'TextboxSSLevelling.Text = TextBoxSSIncrease.Text
                If existingSSLevelling > 0 Then
                    TextboxSSLevelling.Text = existingSSLevelling
                Else
                    TextboxSSLevelling.Text = TextBoxSSIncrease.Text
                End If
                TextboxLevellingBefore62.Text = existAnnutiies + currentPurchase + Convert.ToDecimal(TextboxSSLevelling.Text)
                'TextboxLevellingSSBenefit.Text = TextBoxSSBenefit.Text
                If existingSSBenefit > 0 Then
                    TextboxLevellingSSBenefit.Text = existingSSBenefit
                Else
                    TextboxLevellingSSBenefit.Text = TextBoxSSBenefit.Text
                End If
                Dim levellingAfter62 As Decimal = Convert.ToDecimal(TextboxLevellingBefore62.Text) - Convert.ToDecimal(TextboxLevellingSSBenefit.Text)
                TextboxLevellingAfter62.Text = levellingAfter62

                If levellingAfter62 < 0 Then
                    Me.CheckboxIncludeSSLevelling.Checked = False
                    'TextBoxSSBenefit.Enabled = False
                    TextBoxSSBenefit.ReadOnly = True
                    Me.TextBoxSSBenefit.Text = "0.00"
                    Me.TextBoxSSIncrease.Text = "0.00"
                    Session("resetSSlevelingValues") = "True"
                    'commented by Anudeep on 22-sep for BT-1126
                    'ShowCustomMessage("The amount of annuity is insufficient to allow for the Social Security leveling option."), enumMessageBoxType.DotNet, MessageBoxButtons.OK, True)
                    'ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_ANNUITY_INSUFFICIENT"), enumMessageBoxType.DotNet, MessageBoxButtons.OK, True)
                    HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_ANNUITY_INSUFFICIENT"), EnumMessageTypes.Error, Nothing)    'SP 2013.12.13 BT:2326
                    Exit Sub
                End If
            Else
                resetSSlevelingValues()
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub resetSSlevelingValues()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            ' Reset the controls
            TextboxExistingAnnuities.Text = "0.00"
            TextboxCurrentPurchase.Text = "0.00"
            TextboxSSLevelling.Text = "0.00"
            TextboxLevellingBefore62.Text = "0.00"
            TextboxLevellingSSBenefit.Text = "0.00"
            TextboxLevellingAfter62.Text = "0.00"

            ' Always display the basic data
            'Dim existAnnutiies As Decimal = RetirementBOClass.GetExistingAnnuities(Me.guiFundEventId)
            Dim existAnnutyDetails As DataTable = RetirementBOClass.GetExistingAnnuities(Me.guiFundEventId)
            Dim existAnnutiies As Decimal = Convert.ToDecimal(existAnnutyDetails.Rows(0)("ExistingAnnuities").ToString())
            Dim existingSSLevelling As Decimal = Convert.ToDecimal(existAnnutyDetails.Rows(0)("SSLevelling").ToString())
            Dim existingSSBenefit As Decimal = Convert.ToDecimal(existAnnutyDetails.Rows(0)("SSBenefit").ToString())

            TextboxExistingAnnuities.Text = existAnnutiies.ToString("#0.00")
            Dim currentPurchase As Decimal = 0
            If CheckBoxRetPlan.Checked = True Then
                If TextBoxTotalPaymentRet.Text.Trim() <> "" Then
                    currentPurchase = Convert.ToDecimal(TextBoxTotalPaymentRet.Text)
                End If
            End If
            If CheckBoxSavPlan.Checked = True Then
                If TextBoxTotalPaymentSav.Text.Trim() <> "" Then
                    currentPurchase = currentPurchase + Convert.ToDecimal(TextBoxTotalPaymentSav.Text)
                End If
            End If
            TextboxCurrentPurchase.Text = currentPurchase
            'TextboxSSLevelling.Text = TextBoxSSIncrease.Text
            If existingSSLevelling > 0 Then
                TextboxSSLevelling.Text = existingSSLevelling
            Else
                TextboxSSLevelling.Text = TextBoxSSIncrease.Text
            End If
            TextboxLevellingBefore62.Text = existAnnutiies + currentPurchase + Convert.ToDecimal(TextboxSSLevelling.Text)
            'TextboxLevellingSSBenefit.Text = TextBoxSSBenefit.Text
            If existingSSBenefit > 0 Then
                TextboxLevellingSSBenefit.Text = existingSSBenefit
            Else
                TextboxLevellingSSBenefit.Text = TextBoxSSBenefit.Text
            End If

            Dim levellingAfter62 As Decimal = Convert.ToDecimal(TextboxLevellingBefore62.Text) - Convert.ToDecimal(TextboxLevellingSSBenefit.Text)
            TextboxLevellingAfter62.Text = levellingAfter62

            Session("resetSSlevelingValues") = String.Empty
        Catch
            Throw
        End Try
    End Sub

    '2012.07.16	SP: BT-753/YRS 5.0-1270 : purchase page -start
    Private Sub BindWithHoldingGrid()
        Dim purchase As DataTable = New DataTable
        purchase.Columns.Add("Source")
        purchase.Columns.Add("MP")
        purchase.Columns.Add("RFC")
        purchase.Columns.Add("DFC")
        Dim dsFedWithDrawals As DataSet
        Dim dtFedWithDrawals As DataTable

        Dim dsGenWithDrawals As DataSet
        Dim dtGenWithDrawals As DataTable

        Dim drRow As DataRow
        Dim drRows As DataRow()

        Dim dsTaxEntityTypes As DataSet
        Dim dsTaxFactors As DataSet

        'SP 2012.12.26  BT-2111/YRS 5.0-2144:Rounding error on first check display (changed datatype from double to decimal)
        Dim l_decimal_ExemptionsDefault As Decimal
        Dim l_string_MarriageStatusDefault As String
        Dim l_decimal_ValidExemptionsMaximum As Decimal = 0

        Dim l_string_row_MaritalStatusCode As String
        Dim l_string_row_WithholdingType As String
        Dim l_decimal_row_Exemptions As Decimal
        Dim l_string_row_TaxEntityCode As String
        Dim l_decimal_row_AdditionalAmount As Decimal

        Dim l_integer_MetaTaxFactorCount As Integer = 0
        Dim l_decimal_CreditAdjustment As Decimal = 0
        Dim l_decimal_TaxPercent As Decimal = 0
        Dim l_decimal_Withholding As Decimal = 0
        Dim l_decimal_Fed_WithHolding As Decimal = 0
        Dim l_decimal_Tot_Withholding As Decimal = 0

        Dim l_decimal_TaxCalculationBase As Decimal
        Dim l_decimal_TotalTaxable As Decimal
        Dim l_decimal_Allowance As Decimal
        Dim drPurchase As DataRow
        Dim expDividends As Decimal = 0.0
        Dim monthsinCheckOne As Decimal = 0.0
        Dim l_integer_NoOfMonths As Integer
        Dim dsGeneralWithholdingType As DataSet

        Try
            If HelperFunctions.isNonEmpty(FedWithDrawals) Or HelperFunctions.isNonEmpty(GenWithDrawals) Then
                monthsinCheckOne = GetMonthsinCheckOne()
                expDividends = RetirementBOClass.GetExperienceDividends(Me.RetirementDate)
                'SR:01-Aug-2012:BT-753/YRS 5.0-1270 : Use description field in place of short decription for general witholding codes
                dsGeneralWithholdingType = RetireesInformationBOClass.getGenCodeWithDescription()
                'End,SR:01-Aug-2012:BT-753/YRS 5.0-1270 : Use description field in place of short decription for general witholding codes
            End If


            If Not Me.FedWithDrawals Is Nothing Then
                dsFedWithDrawals = Me.FedWithDrawals
                If dsFedWithDrawals.Tables.Count > 0 Then
                    dtFedWithDrawals = dsFedWithDrawals.Tables(0)
                End If
            End If

            If Not Me.GenWithDrawals Is Nothing Then
                dsGenWithDrawals = Me.GenWithDrawals
                If dsGenWithDrawals.Tables.Count > 0 Then
                    dtGenWithDrawals = dsGenWithDrawals.Tables(0)
                End If
            End If

            If Not dtFedWithDrawals Is Nothing Then
                If dtFedWithDrawals.Rows.Count > 0 Then
                    '--------------Federal withholdings------------------
                    If Me.TaxEntityTypes Is Nothing Then
                        dsTaxEntityTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.TaxEntityTypes()
                    Else
                        Me.TaxEntityTypes = dsTaxEntityTypes
                    End If

                    If Me.TaxFactors Is Nothing Then
                        dsTaxFactors = YMCARET.YmcaBusinessObject.RetirementBOClass.TaxFactors()
                    Else
                        Me.TaxFactors = dsTaxFactors
                    End If

                    If Me.WITHHOLDING_DEFAULT_MARRIAGE_STATUS Is Nothing Then
                        Try
                            l_decimal_ExemptionsDefault = GetConfigValues("WITHHOLDING_DEFAULT_EXEMPTIONS")
                        Catch
                            l_decimal_ExemptionsDefault = 0
                        End Try
                        Me.WITHHOLDING_DEFAULT_EXEMPTIONS = l_decimal_ExemptionsDefault

                        Try
                            l_string_MarriageStatusDefault = GetConfigValues("WITHHOLDING_DEFAULT_MARRIAGE_STATUS")
                        Catch
                            l_string_MarriageStatusDefault = "M"
                        End Try
                        Me.WITHHOLDING_DEFAULT_MARRIAGE_STATUS = l_string_MarriageStatusDefault

                        '----------Formula (tax table) withholdings----------------------------
                        '----------Get number of maximum valid exemptions----------------------
                        Try
                            l_decimal_ValidExemptionsMaximum = GetConfigValues("WITHHOLDING_MAXIMUM_VAID_EXEMPTIONS")
                        Catch ex As Exception
                            l_decimal_ValidExemptionsMaximum = 50
                        End Try
                        Me.WITHHOLDING_MAXIMUM_VAID_EXEMPTIONS = l_decimal_ValidExemptionsMaximum
                    Else
                        l_decimal_ExemptionsDefault = Me.WITHHOLDING_DEFAULT_EXEMPTIONS
                        l_string_MarriageStatusDefault = Me.WITHHOLDING_DEFAULT_MARRIAGE_STATUS
                        l_decimal_ValidExemptionsMaximum = Me.WITHHOLDING_MAXIMUM_VAID_EXEMPTIONS
                    End If

                    If l_decimal_ValidExemptionsMaximum = 0 Then
                        l_decimal_ValidExemptionsMaximum = 50
                    End If

                    l_integer_NoOfMonths = Me.MonthsinCheckOne

                    If l_integer_NoOfMonths < 1 Then
                        l_integer_NoOfMonths = 1
                    End If

                    Dim l_decimal_Taxable As Decimal = 0
                    If CheckBoxRetPlan.Checked Then
                        l_decimal_Taxable += Convert.ToDecimal(TextBoxTaxableRet.Text)
                    End If
                    If TextBoxTaxableSav.Text.Trim <> String.Empty Then
                        If CheckBoxSavPlan.Checked Then
                            l_decimal_Taxable += Convert.ToDecimal(TextBoxTaxableSav.Text)
                        End If
                    End If

                    l_decimal_TotalTaxable = l_decimal_Taxable

                    For Each drRow In dtFedWithDrawals.Rows
                        drPurchase = purchase.NewRow()
                        l_decimal_Fed_WithHolding = 0
                        l_string_row_WithholdingType = drRow("Type")
                        l_string_row_WithholdingType = l_string_row_WithholdingType.Trim()

                        If drRow("Marital Status").GetType.ToString() = "System.DBNull" Then
                            l_string_row_MaritalStatusCode = ""
                        Else
                            l_string_row_MaritalStatusCode = drRow("Marital Status")
                        End If

                        l_string_row_MaritalStatusCode = l_string_row_MaritalStatusCode.Trim()

                        l_decimal_row_Exemptions = drRow("Exemptions")

                        l_string_row_TaxEntityCode = drRow("Tax Entity")
                        l_string_row_TaxEntityCode = l_string_row_TaxEntityCode.Trim()

                        If drRow("Add'l Amount").GetType.ToString() = "System.DBNull" Then
                            l_decimal_row_AdditionalAmount = 0
                        Else
                            l_decimal_row_AdditionalAmount = drRow("Add'l Amount")
                        End If

                        '------------------Determine which allowance to use NYState or Fed---------------------------
                        drRows = dsTaxEntityTypes.Tables(0).Select("TaxEntitytype='" & l_string_row_TaxEntityCode & "'")
                        If drRows.Length > 0 Then
                            l_decimal_Allowance = drRows(0)("ExemptionAllowance")
                        End If

                        Select Case l_string_row_WithholdingType
                            Case "DEFALT"
                                l_decimal_row_Exemptions = l_decimal_ExemptionsDefault
                                l_string_row_MaritalStatusCode = l_string_MarriageStatusDefault
                                l_decimal_row_AdditionalAmount = 0

                            Case "FLAT"
                                l_decimal_row_Exemptions = 0
                                If drRow("Add'l Amount").GetType.ToString() = "System.DBNull" Then
                                    l_decimal_row_AdditionalAmount = 0
                                Else
                                    l_decimal_row_AdditionalAmount = drRow("Add'l Amount")
                                End If

                            Case "FORMUL"
                                If drRow("Add'l Amount").GetType.ToString() = "System.DBNull" Then
                                    l_decimal_row_AdditionalAmount = 0
                                Else
                                    l_decimal_row_AdditionalAmount = drRow("Add'l Amount")
                                End If
                                l_decimal_row_Exemptions = drRow("Exemptions")

                            Case Else
                                'commented by Anudeep on 22-sep for BT-1126
                                'Throw (New Exception("Invalid Withholding type encountered while getting the withheld amount."))
                                Throw (New Exception(getmessage("MESSAGE_RETIREMENT_PROCESSING_INVALID_WITHHOLDING")))

                        End Select

                        '--------------------See if these settings qualify for formula based withholdings-------------------
                        If l_string_row_WithholdingType <> "FLAT" And l_decimal_row_Exemptions <= l_decimal_ValidExemptionsMaximum Then
                            l_decimal_TaxCalculationBase = l_decimal_TotalTaxable - (l_decimal_row_Exemptions * l_decimal_Allowance)

                            '---------Set lnTaxCalculationBase to zero if lesser than zero--------------
                            If l_decimal_TaxCalculationBase <= 0 Then
                                l_decimal_TaxCalculationBase = 0
                            End If

                            '---------Check for Invalid marriage status-----------------------
                            If Not (l_string_row_MaritalStatusCode = "S" Or l_string_row_MaritalStatusCode = "M") Then
                                l_string_row_MaritalStatusCode = "S"
                            End If

                            drRows = dsTaxFactors.Tables(0).Select("TaxEntityCode='" & l_string_row_TaxEntityCode & "'" & _
                               " AND MaritalStatusCode = '" & l_string_row_MaritalStatusCode & "'" & _
                               " AND TaxableLow <" & l_decimal_TaxCalculationBase & _
                               " AND TaxableHigh >=" & l_decimal_TaxCalculationBase)
                            l_integer_MetaTaxFactorCount = drRows.Length
                            If l_integer_MetaTaxFactorCount > 0 Then
                                l_decimal_TaxPercent = drRows(0)("TaxPercent")
                                l_decimal_CreditAdjustment = drRows(0)("CreditAdjustment")

                                l_decimal_Withholding = ((l_decimal_TaxCalculationBase * l_decimal_TaxPercent / 100) + l_decimal_CreditAdjustment)
                            End If

                            '---------------Flat Tax Calculation----------------------
                            l_decimal_row_AdditionalAmount = l_decimal_Withholding + l_decimal_row_AdditionalAmount
                        End If

                        l_decimal_Fed_WithHolding = l_decimal_Fed_WithHolding + l_decimal_row_AdditionalAmount
                        If l_decimal_Fed_WithHolding > 0 Then
                            Dim str As String = (From r In dsTaxEntityTypes.Tables(0).AsEnumerable()
                              Where r.Field(Of String)("TaxEntitytype").Equals(l_string_row_TaxEntityCode, StringComparison.CurrentCultureIgnoreCase)
                              Select r.Field(Of String)("Description")).First()
                            drPurchase("Source") = str
                            drPurchase("MP") = l_decimal_Fed_WithHolding.ToString("#0.00")
                            drPurchase("RFC") = (Math.Round(l_decimal_Fed_WithHolding, 2) * monthsinCheckOne).ToString("#0.00")
                            drPurchase("DFC") = (Math.Round(l_decimal_Fed_WithHolding, 2) * expDividends).ToString("#0.00")
                            purchase.Rows.Add(drPurchase)
                        End If
                    Next
                End If
            End If

            Dim l_string_row_StartDate As String
            Dim l_string_row_EndDate As String = String.Empty
            Dim l_decimal_row_Amount As Decimal
            Dim l_decimal_Gen_Withholding As Decimal = 0
            Dim annuityPurchaseDate As DateTime
            Dim isApplicable As Boolean = False
            Dim intMonths As Integer

            If Not dtGenWithDrawals Is Nothing Then
                If dtGenWithDrawals.Rows.Count > 0 Then
                    annuityPurchaseDate = Convert.ToDateTime(Me.RetirementDate)

                    '--------------General withholdings------------------
                    For Each drRow In dtGenWithDrawals.Rows
                        drPurchase = purchase.NewRow()
                        l_decimal_Gen_Withholding = 0
                        isApplicable = False
                        l_string_row_StartDate = drRow("Start Date")
                        If Not drRow("End Date") Is DBNull.Value Then
                            l_string_row_EndDate = drRow("End Date")
                        Else
                            l_string_row_EndDate = String.Empty
                        End If

                        'SR:06-Aug-2012:BT-753/YRS 5.0-1270 : If end date is provided then count the number of months to calculate gen withholdings
                        If l_string_row_EndDate <> String.Empty Then
                            'SR:2012.08.08: BT-753/YRS 5.0-1270 : purchase page(Consider end date when last payroll date is greater then end date)
                            Dim dtEndDate As String
                            dtEndDate = l_string_row_EndDate
                            dtEndDate = Convert.ToDateTime(l_string_row_EndDate).Month & "/01/" & Convert.ToDateTime(l_string_row_EndDate).Year

                            If Convert.ToDateTime(Me.LastPayrollDate) >= Convert.ToDateTime(dtEndDate) Then
                                intMonths = DateDiff(DateInterval.Month, Convert.ToDateTime(Me.RetirementDate), Convert.ToDateTime(dtEndDate)) + 1
                            Else
                                intMonths = monthsinCheckOne
                            End If
                            'End, SR:2012.08.08: BT-753/YRS 5.0-1270 : purchase page(Consider end date when last payroll date is greater then end date)
                        End If
                        'End-SR:06-Aug-2012:BT-753/YRS 5.0-1270 : If end date is provided then count the number of months to calculate gen withholdings

                        l_decimal_row_Amount = drRow("Add'l Amount")

                        If Convert.ToDateTime(l_string_row_StartDate) <= annuityPurchaseDate Then
                            If String.IsNullOrEmpty(l_string_row_EndDate) Then
                                If l_decimal_row_Amount > 0 Then
                                    l_decimal_Gen_Withholding = l_decimal_Gen_Withholding + l_decimal_row_Amount
                                    isApplicable = True
                                End If
                            ElseIf Not (String.IsNullOrEmpty(l_string_row_EndDate)) Then
                                If Convert.ToDateTime(l_string_row_EndDate) > annuityPurchaseDate Then
                                    If l_decimal_row_Amount > 0 Then
                                        l_decimal_Gen_Withholding = l_decimal_Gen_Withholding + l_decimal_row_Amount
                                        isApplicable = True
                                    End If
                                End If
                            End If
                        End If
                        If isApplicable Then
                            Dim str As String = (From r In dsGeneralWithholdingType.Tables(0).AsEnumerable()
                              Where r.Field(Of String)("value").Equals(drRow("Type").ToString(), StringComparison.CurrentCultureIgnoreCase)
                              Select r.Field(Of String)("Description")).First()
                            drPurchase("Source") = "General Tax" + "(" + str + ")"
                            drPurchase("MP") = l_decimal_Gen_Withholding.ToString("#0.00")
                            drPurchase("DFC") = Math.Round(l_decimal_Gen_Withholding * expDividends, 2).ToString("#0.00")

                            'SR:06-Aug-2012:BT-753/YRS 5.0-1270 : If end date is provided then calculate gen withholdings till end date
                            If l_string_row_EndDate <> String.Empty Then
                                drPurchase("RFC") = Math.Round(l_decimal_Gen_Withholding * intMonths, 2).ToString("#0.00")
                            Else
                                drPurchase("RFC") = Math.Round(l_decimal_Gen_Withholding * monthsinCheckOne, 2).ToString("#0.00")
                            End If
                            'End-SR:06-Aug-2012:BT-753/YRS 5.0-1270 : If end date is provided then calculate gen withholdings till end date
                            purchase.Rows.Add(drPurchase)
                        End If
                    Next
                End If
            End If
            purchase.AcceptChanges()
            DatagridWithheld.DataSource = purchase
            DatagridWithheld.DataBind()
        Catch ex As Exception
            Throw
        End Try

    End Sub
    '2012.07.16	SP: BT-753/YRS 5.0-1270 : purchase page -end

    Private Sub displayPurchaseInfo()
        Dim purchase As DataTable = New DataTable
        purchase.Columns.Add("Source")
        purchase.Columns.Add("Option") 'SP 2013.12.19 BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process 
        purchase.Columns.Add("MP")
        purchase.Columns.Add("RFC")
        purchase.Columns.Add("DFC")

        Dim monthsinCheckOne As Decimal
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            monthsinCheckOne = GetMonthsinCheckOne()
            LabelNoOfMonthsInFirstCheckValue.Text = monthsinCheckOne.ToString()

            'START : ML | 2019.09.20 | YRS-AT-4597 | Alert Message for State Withholding Deducation
            lblStateWithholdingMessage.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_STATEAMOUNT_ALERT).DisplayText
            'END : ML | 2019.09.20 | YRS-AT-4597 | Alert Message for State Withholding Deducation
            Dim expDividends As Decimal = 0.0
            expDividends = RetirementBOClass.GetExperienceDividends(Me.RetirementDate)

            'Retirement plan
            'If CheckBoxRetPlan.Checked = True Then 
            If CheckBoxRetPlan.Checked = True Then
                'Sanket Vaidya 12 Apr 2011 For BT-810 Error in Normal Retirement Processing after regular withdrawl
                If Not Session("dtAnnuityList") Is Nothing Then
                    Dim drPurch As DataRow = purchase.NewRow()
                    Dim dtAnnList As DataTable = Session("dtAnnuityList")
                    Dim drAnnRows As DataRow() = dtAnnList.Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")

                    Dim pay As Decimal = 0.0
                    pay = drAnnRows(0)("mnyCurrentPayment")
                    ' Club Savings plan value if it is Pre2008 retirement
                    If IsPrePlanSplitRetirement = True And CheckBoxSavPlan.Checked = True Then
                        Dim dtAnnListSav As DataTable = Session("dtAnnuityListSav")
                        Dim drAnnRowsSav As DataRow() = dtAnnListSav.Select("chrAnnuityType='" & TextBoxAnnuitySelectSav.Text.Trim() & "'")
                        pay = pay + drAnnRowsSav(0)("mnyCurrentPayment")
                    End If
                    drPurch("Source") = "Retirement Plan"
                    drPurch("MP") = pay.ToString("#0.00")
                    drPurch("RFC") = (pay * monthsinCheckOne).ToString("#0.00")
                    drPurch("DFC") = (Math.Round(pay * expDividends, 2)).ToString("#0.00")
                    drPurch("Option") = TextBoxAnnuitySelectRet.Text.Trim() 'SP 2013.12.19 -BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process 
                    purchase.Rows.Add(drPurch)
                    purchase.AcceptChanges()
                End If
            End If

            'Savings Plan
            If CheckBoxSavPlan.Checked = True And IsPrePlanSplitRetirement = False Then
                'Sanket Vaidya 12 Apr 2011 For BT-810 Error in Normal Retirement Processing after regular withdrawl
                If Not Session("dtAnnuityListSav") Is Nothing Then
                    Dim drPurch As DataRow = purchase.NewRow()
                    Dim dtAnnList As DataTable = Session("dtAnnuityListSav")
                    Dim drAnnRows As DataRow() = dtAnnList.Select("chrAnnuityType='" & TextBoxAnnuitySelectSav.Text.Trim() & "'")
                    Dim pay As Decimal = 0.0
                    pay = drAnnRows(0)("mnyCurrentPayment")
                    drPurch("Source") = "Savings Plan"
                    drPurch("MP") = pay.ToString("#0.00")
                    drPurch("RFC") = (pay * monthsinCheckOne).ToString("#0.00")
                    drPurch("DFC") = (Math.Round(pay * expDividends, 2)).ToString("#0.00")
                    drPurch("Option") = TextBoxAnnuitySelectSav.Text.Trim() 'SP 2013.12.19 -BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process 
                    purchase.Rows.Add(drPurch)
                    purchase.AcceptChanges()
                End If
            End If

            'Death benefit
            Dim tnFullBalance As Decimal = 0
            tnFullBalance = Convert.ToDecimal(TextBoxAmount.Text)
            If tnFullBalance > 0 Then
                'Sanket Vaidya 12 Apr 2011 For BT-810 Error in Normal Retirement Processing after regular withdrawl
                If Not Session("dtAnnuitiesFullBalanceList") Is Nothing Then
                    Dim strDBAnnuityTpe As String
                    Dim drPurch As DataRow = purchase.NewRow()
                    Dim dtAnnList As DataTable = Session("dtAnnuitiesFullBalanceList")
                    Dim drAnnRows As DataRow() = dtAnnList.Select("chrAnnuityType='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
                    strDBAnnuityTpe = CType(drAnnRows(0)("chrDBAnnuityType"), String)
                    drAnnRows = dtAnnList.Select("chrAnnuityType='" & strDBAnnuityTpe.Trim() & "'")
                    Dim pay As Decimal = 0.0
                    pay = drAnnRows(0)("mnyCurrentPayment")
                    drPurch("Source") = "Death Benefit"
                    drPurch("MP") = pay.ToString("#0.00")
                    drPurch("RFC") = (pay * monthsinCheckOne).ToString("#0.00")
                    drPurch("DFC") = (Math.Round(pay * expDividends, 2)).ToString("#0.00")
                    purchase.Rows.Add(drPurch)
                    purchase.AcceptChanges()
                End If
            End If


            'SS levelling
            If (CheckboxIncludeSSLevelling.Checked = True) And (Convert.ToDouble(TextBoxSSBenefit.Text) > 0) Then
                Dim drPurch As DataRow = purchase.NewRow()
                drPurch("Source") = "Social Security Leveling"
                drPurch("MP") = TextBoxSSIncrease.Text
                drPurch("RFC") = (TextBoxSSIncrease.Text * monthsinCheckOne).ToString("#0.00")
                drPurch("DFC") = Math.Round(TextBoxSSIncrease.Text * expDividends, 2).ToString("#0.00")
                purchase.Rows.Add(drPurch)
                purchase.AcceptChanges()
            End If

            DatagridPurchase.DataSource = purchase
            DatagridPurchase.DataBind()

            '2012.07.16	SP: BT-753/YRS 5.0-1270 : purchase page - Start (-Commented for this issse and add new method BindWithHoldingGrid())
            'Dim withheld As DataTable
            'withheld = purchase.Clone()
            'Dim drWith As DataRow = withheld.NewRow()
            'Dim withholdingAmt As Double = 0.0
            'withholdingAmt = GetWithHolding()
            'drWith("Source") = "Tax Withheld"
            'drWith("MP") = withholdingAmt.ToString("#0.00")
            'drWith("RFC") = (withholdingAmt * monthsinCheckOne).ToString("#0.00")
            'drWith("DFC") = Math.Round(withholdingAmt * expDividends, 2).ToString("#0.00")
            'withheld.Rows.Add(drWith)
            'withheld.AcceptChanges()

            'DatagridWithheld.DataSource = withheld
            'DatagridWithheld.DataBind()

            Me.BindWithHoldingGrid()
            '2012.07.16	SP: BT-753/YRS 5.0-1270 : purchase page - End
        Catch
            Throw
        End Try

    End Sub

    Private Function isTaxWithheldMoreThanTaxableAmount() As Boolean
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            Dim decTaxable As Decimal
            If CheckBoxRetPlan.Checked Then
                decTaxable = Convert.ToDecimal(TextBoxTaxableRet.Text)
            End If

            If CheckBoxSavPlan.Checked Then
                decTaxable = decTaxable + Convert.ToDecimal(TextBoxTaxableSav.Text)
            End If

            If (CheckboxIncludeSSLevelling.Checked = True) And (Convert.ToDouble(TextBoxSSBenefit.Text) > 0) Then
                decTaxable = decTaxable + Convert.ToDecimal(TextBoxSSIncrease.Text)
            End If

            ' Check if the TaxableAmount is less than TaxWithheld
            Dim withholdingAmt As Double = 0.0
            withholdingAmt = GetWithHolding()
            If withholdingAmt > decTaxable Then
                Return True
            Else
                Return False
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function isAnnuityAmountZero() As Boolean
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            Dim decTaxable As Decimal
            If CheckBoxRetPlan.Checked Then
                decTaxable = Convert.ToDecimal(TextBoxTaxableRet.Text)
            End If

            If CheckBoxSavPlan.Checked Then
                decTaxable = decTaxable + Convert.ToDecimal(TextBoxTaxableSav.Text)
            End If

            If (CheckboxIncludeSSLevelling.Checked = True) And (Convert.ToDouble(TextBoxSSBenefit.Text) > 0) Then
                decTaxable = decTaxable + Convert.ToDecimal(TextBoxSSIncrease.Text)
            End If

            If decTaxable = 0 Then
                Return True
            Else
                Return False
            End If
        Catch
            Throw
        End Try
    End Function

    Private Sub disposeAndRedirectToFind()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Dim retType As String
        Try
            Call ClearResources()

            retType = Request.QueryString.Get("RetType")
            Session("Page") = "Person"
            Me.Page.Dispose()

            'Response.Redirect("FindInfo.aspx?Name=Process", False)

        Catch
            Throw
        End Try
        'Server.Transfer("FindInfo.aspx?Name=Process&RetType=" + retType, False)
        Response.Redirect("FindInfo.aspx?Name=Process&RetType=" + retType, False)
    End Sub
    'NP:IVP1:2008.04.28 - Adding function to get the plantype for computation on which various rules will be applied
    Private Function GetPlanType() As String
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            If Date.Parse(TextBoxRetirementDate.Text) < RetirementBOClass.GetPlanSplitDate() Then
                Return Nothing
            Else
                If CheckBoxRetPlan.Checked AndAlso CheckBoxSavPlan.Checked Then
                    Return "B"  'Validate for both plans
                ElseIf CheckBoxRetPlan.Checked AndAlso Not CheckBoxSavPlan.Checked Then
                    Return "R"  'Validate for retirement plan only
                ElseIf CheckBoxSavPlan.Checked AndAlso Not CheckBoxRetPlan.Checked Then
                    Return "S"  'Validate for savings plan only
                Else    'None of the plans were checked. Check and validate for both plans in this case
                    Return "B"
                End If
            End If
        Catch
            Throw
        End Try
    End Function

    'added by hafiz on 10-Sep-2008 for Phase IV - Part 2.
    Private Function ValidateLastPayrollDate() As String
        Dim l_string_LastPayRollDateValidation As String = ""

        Try
            If YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.ValidateLastPayrollDate(Me.PersId, Me.guiFundEventId) = 0 Then
                'commented by Anudeep on 22-sep for BT-1126
                'l_string_LastPayRollDateValidation = "Not all contributions have been received\funded, this process cannot be completed."
                'l_string_LastPayRollDateValidation = "We have not received the final contribution based upon the termination date.  Processing cannot continue until the final contribution has been received and funded."
                'START - Chandra sekar | 2016.03.01 | YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)
                If Me.FundEventStatus = "PT" Or Me.FundEventStatus = "RPT" Then
                    l_string_LastPayRollDateValidation = getmessage("MESSAGE_RETIREMENT_PROCESSING_TD_FINAL_CONTRIBUTION_NOT_RECIEVED")
                    'END - Chandra sekar | 2016.03.01 | YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)
                Else
                    l_string_LastPayRollDateValidation = getmessage("MESSAGE_RETIREMENT_PROCESSING_FINAL_CONTRIBUTION_NOT_RECIEVED")
                End If
            Else
                l_string_LastPayRollDateValidation = ""
            End If

            Return l_string_LastPayRollDateValidation

        Catch
            Throw
        End Try
    End Function
    Private Sub LoadCalledAnnuitySummaryTab()
        Dim drFoundRows As DataRow()
        Try
            If CheckBoxRetPlan.Checked Then
                If Not IsNothing(Me.SelectAnnuityRetirementExactAgeEffDate) AndAlso Not IsNothing(Me.SelectAnnuityRetirement) Then
                    If Me.SelectAnnuityRetirementExactAgeEffDate.Tables.Count > 0 AndAlso Me.SelectAnnuityRetirement.Tables.Count > 0 Then

                        drFoundRows = Me.SelectAnnuityRetirement.Tables(0).Select("Annuity='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
                        If drFoundRows.Length > 0 Then
                            rdbListAnnuityOptionRet.Items(0).Text = "OLD ($" + drFoundRows(0)("Amount").ToString() + ") Method"
                        End If
                        drFoundRows = Me.SelectAnnuityRetirementExactAgeEffDate.Tables(0).Select("Annuity='" & TextBoxAnnuitySelectRet.Text.Trim() & "'")
                        If drFoundRows.Length > 0 Then
                            rdbListAnnuityOptionRet.Items(1).Text = "NEW ($" + drFoundRows(0)("Amount").ToString() + ") Method"
                        End If
                        For Each rdblistitem As ListItem In rdbListAnnuityOptionRet.Items
                            If rdblistitem.Value = Session("SelectedCalledAnnuity_Ret") Then
                                rdblistitem.Selected = True

                                'Exit For
                            Else
                                rdblistitem.Selected = False
                            End If
                        Next
                        lblSummarySelectedAnnuityRet.Text = "Purchase Annuity '" + Me.TextBoxAnnuitySelectRet.Text + "' from Retirement Plan using"
                        rdbListAnnuityOptionRet.Visible = True
                        lblSummarySelectedAnnuityRet.Visible = True
                    End If
                End If
            Else
                rdbListAnnuityOptionRet.Visible = False
                lblSummarySelectedAnnuityRet.Text = ""
                lblSummarySelectedAnnuityRet.Visible = False
            End If
            If CheckBoxSavPlan.Checked Then
                If Not IsNothing(Me.SelectAnnuitySavingsExactAgeEffDate) AndAlso Not IsNothing(Me.SelectAnnuityRetirement) Then
                    If Me.SelectAnnuitySavingsExactAgeEffDate.Tables.Count > 0 AndAlso Me.SelectAnnuitySavings.Tables.Count > 0 Then

                        drFoundRows = Me.SelectAnnuitySavings.Tables(0).Select("Annuity='" & TextBoxAnnuitySelectSav.Text.Trim() & "'")
                        If drFoundRows.Length > 0 Then
                            rdbListAnnuityOptionSav.Items(0).Text = "OLD ($" + drFoundRows(0)("Amount").ToString() + ") Method"
                        End If
                        drFoundRows = Me.SelectAnnuitySavingsExactAgeEffDate.Tables(0).Select("Annuity='" & TextBoxAnnuitySelectSav.Text.Trim() & "'")
                        If drFoundRows.Length > 0 Then
                            rdbListAnnuityOptionSav.Items(1).Text = "NEW ($" + drFoundRows(0)("Amount").ToString() + ") Method"
                        End If
                        For Each rdblistitem As ListItem In rdbListAnnuityOptionSav.Items
                            If rdblistitem.Value = Session("SelectedCalledAnnuity_Sav") Then
                                rdblistitem.Selected = True
                                'Exit For
                            Else

                                rdblistitem.Selected = False
                            End If
                        Next
                        rdbListAnnuityOptionSav.Visible = True
                        lblSummarySelectedAnnuitySav.Text = "Purchase Annuity '" + Me.TextBoxAnnuitySelectSav.Text + "' from Saving Plan using"
                        lblSummarySelectedAnnuitySav.Visible = True

                    End If
                End If
            Else
                rdbListAnnuityOptionSav.Visible = False
                lblSummarySelectedAnnuitySav.Text = ""
                lblSummarySelectedAnnuitySav.Visible = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
    Private Sub BindRelationShipDropDown()

        Dim l_dataset_RelationShips As DataSet
        Dim dtRelationship As New DataTable
        Dim foundRows() As DataRow
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            l_dataset_RelationShips = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.getRelationShips()
            If (HelperFunctions.isNonEmpty(l_dataset_RelationShips)) Then
                dtRelationship = l_dataset_RelationShips.Tables(0).Clone()
            End If
            foundRows = l_dataset_RelationShips.Tables(0).Select("CodeValue not in('ES','IN','TR')")
            For Each drRow As DataRow In foundRows
                dtRelationship.ImportRow(drRow)
            Next
            DropDownRelationShipRet.DataSource = dtRelationship
            DropDownRelationShipRet.DataTextField = "Description"
            DropDownRelationShipRet.DataValueField = "CodeValue"
            DropDownRelationShipRet.DataBind()
            DropDownRelationShipRet.Items.Insert(0, "Select")
            DropDownRelationShipSav.DataSource = dtRelationship
            DropDownRelationShipSav.DataTextField = "Description"
            DropDownRelationShipSav.DataValueField = "CodeValue"
            DropDownRelationShipSav.DataBind()
            DropDownRelationShipSav.Items.Insert(0, "Select")
        Catch
            Throw
        End Try
    End Sub

    Private Sub BindBeneficiaryDetails(ByVal personId As String, ByVal selectedAnnuity As String, ByVal planType As String)

        Dim dsParticipantBeneficiaries As DataSet
        Dim dsParticipantBeneficiariesFinal As New DataSet
        Dim foundRows() As DataRow
        Dim drBenCollection() As DataRow
        Dim drBen As DataRow
        Dim dtParticipantBeneficiaries As New DataTable
        Dim l_button_select As ImageButton
        Dim i As Int32
        Try
            dsParticipantBeneficiaries = RetirementBOClass.getParticipantBeneficiaries(personId)
            dtParticipantBeneficiaries = dsParticipantBeneficiaries.Tables(0).Clone()
            foundRows = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode not in('ES','IN','TR') AND chvBeneficiaryTypeCode = 'MEMBER'")
            For Each drRow As DataRow In foundRows
                dtParticipantBeneficiaries.ImportRow(drRow)
            Next
            dtParticipantBeneficiaries.AcceptChanges()
            Dim drBenexi As DataRow
            If (Me.FundEventStatus = "RT" Or Me.FundEventStatus = "RPT" Or Me.FundEventStatus = "RD" Or Me.FundEventStatus = "RP") Then

                Dim dsBeneficiaryInfo As New DataSet
                Dim drexistingrow() As DataRow
                Dim newBen As DataRow
                dsBeneficiaryInfo = RetirementBOClass.GetJointAnnuitySurvivors(Me.PersId)
                If (HelperFunctions.isNonEmpty(dsBeneficiaryInfo)) Then
                    newBen = dsBeneficiaryInfo.Tables(0).Rows(0)
                    Dim strfilter As String
                    strfilter = IIf(newBen("Spouse").ToString().ToLower() = "true", "SP", "")
                    drexistingrow = dtParticipantBeneficiaries.Select("chrBeneficiaryTaxNumber ='" + newBen("SSNo").ToString + "'  AND BenLastName='" + newBen("LastName").ToString() + "' AND BenFirstName ='" + newBen("FirstName").ToString() + "' AND BenBirthDate='" + newBen("BirthDate").ToString() + "' AND chvRelationshipCode='" + strfilter + "'")
                    If (drexistingrow.Length > 0) Then
                        drBenexi = drexistingrow(0)
                        dtParticipantBeneficiaries.Rows.Remove(drexistingrow(0))
                        dtParticipantBeneficiaries.AcceptChanges()

                    End If
                    drBenexi = dtParticipantBeneficiaries.NewRow()
                    drBenexi("chrBeneficiaryTaxNumber") = newBen("SSNo")
                    drBenexi("BenLastName") = newBen("LastName")
                    drBenexi("BenFirstName") = newBen("FirstName")
                    drBenexi("BenBirthDate") = newBen("BirthDate")
                    If newBen("Spouse").ToString().ToLower() = "true" Then
                        drBenexi("chvRelationshipCode") = "SP"
                        drBenexi("Relationship") = "Spouse"
                    End If
                    drBenexi("guiUniqueID") = Guid.NewGuid().ToString()
                    dtParticipantBeneficiaries.Rows.Add(drBenexi)
                    dtParticipantBeneficiaries.AcceptChanges()
                End If
            End If
            dsParticipantBeneficiariesFinal.Tables.Add(dtParticipantBeneficiaries)
            Me.BeneficiaryInfo = dsParticipantBeneficiariesFinal

            If Left(selectedAnnuity, 1).ToLower = "j" And planType.ToLower() = "r" Then


                If (HelperFunctions.isNonEmpty(dtParticipantBeneficiaries)) Then
                    DataGridAnnuityBeneficiaries.Visible = True
                    DataGridAnnuityBeneficiaries.DataSource = dtParticipantBeneficiaries
                    DataGridAnnuityBeneficiaries.DataBind()
                    'HideBeneficiaryRetControls(True)
                    If Not (drBenexi Is Nothing) Then
                        drBen = drBenexi
                    Else
                        drBenCollection = dtParticipantBeneficiaries.Select("chvRelationshipCode = 'SP'")
                        If drBenCollection.Length > 0 Then
                            drBen = drBenCollection(0)
                        End If
                    End If
                    If Not (drBen Is Nothing) Then

                        AddSelectedParticipantBeneficiary(drBen)
                        'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor

                        If Not drBen("chvRelationshipCode").ToString() Is DBNull.Value Then

                            'If Me.FundEventStatus <> "QD" And drBen("chvRelationshipCode").ToString.Trim() <> "SP" Then Commented by Chandra sekar 2016.04.08  YRS-AT-2915 - YRS Bug: Annuity purchase screen no longer pre-filling annuity beneficiary (TrackIT 25836)
                            'Start-Chandra sekar.c  2016.04.08  YRS-AT-2915 - YRS Bug: Annuity purchase screen no longer pre-filling annuity beneficiary (TrackIT 25836)
                            If Me.FundEventStatus = "QD" And drBen("chvRelationshipCode").ToString.Trim() = "SP" Then
                                Session("IsDefaultBeneficiarySpouse") = True
                            Else
                                'End-Chandra sekar.c  2016.04.08   YRS-AT-2915 - YRS Bug: Annuity purchase screen no longer pre-filling annuity beneficiary (TrackIT 25836)
                                Session("IsDefaultBeneficiarySpouse") = False
                                If Not drBen("chrBeneficiaryTaxNumber").ToString() Is DBNull.Value Then
                                    If Not drBen("chrBeneficiaryTaxNumber").ToString() = String.Empty Then
                                        TextBoxAnnuitySSNoRet.Text = drBen("chrBeneficiaryTaxNumber").ToString()
                                    End If
                                End If

                                If Not drBen("BenLastName").ToString() Is DBNull.Value Then
                                    If Not drBen("BenLastName").ToString() = String.Empty Then
                                        TextBoxAnnuityLastNameRet.Text = drBen("BenLastName").ToString()
                                    End If
                                End If

                                If Not drBen("BenFirstName").ToString() Is DBNull.Value Then
                                    If Not drBen("BenFirstName").ToString() = String.Empty Then
                                        TextBoxAnnuityFirstNameRet.Text = drBen("BenFirstName").ToString()
                                    End If
                                End If

                                If Not drBen("BenBirthDate").ToString() Is DBNull.Value Then
                                    If Not drBen("BenBirthDate").ToString() = String.Empty Then
                                        TextBoxAnnuityBirthDateRet.Text = Convert.ToDateTime(drBen("BenBirthDate")).ToString("MM/dd/yyyy")
                                    End If
                                End If
                                If Not drBen("chvRelationshipCode").ToString() Is DBNull.Value Then
                                    DropDownRelationShipRet.SelectedValue = drBen("chvRelationshipCode").ToString.Trim()
                                End If
                                While i < Me.DataGridAnnuityBeneficiaries.Items.Count
                                    l_button_select = New ImageButton
                                    l_button_select = Me.DataGridAnnuityBeneficiaries.Items(i).FindControl("ImageButtonAccounts")
                                    If Me.DataGridAnnuityBeneficiaries.Items(i).Cells(1).Text.Trim().ToUpper() = drBen("guiUniqueID").ToString().ToUpper() Then
                                        If Not l_button_select Is Nothing Then
                                            l_button_select.ImageUrl = "images\selected.gif"
                                        End If
                                    Else
                                        If Not l_button_select Is Nothing Then
                                            l_button_select.ImageUrl = "images\select.gif"
                                        End If
                                    End If
                                    i = i + 1
                                End While



                            End If
                        End If 'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                    End If
                Else
                    DataGridAnnuityBeneficiaries.Visible = False
                    LabelNoBeneficiary.Visible = Not (HelperFunctions.isNonEmpty(dtParticipantBeneficiaries))
                End If
                'Else
                '	HideBeneficiaryRetControls(False)
            End If
            If Left(selectedAnnuity, 1).ToLower = "j" And planType.ToLower() = "s" Then
                If (HelperFunctions.isNonEmpty(dtParticipantBeneficiaries)) Then
                    DataGridAnnuityBeneficiariesSav.Visible = True
                    DataGridAnnuityBeneficiariesSav.DataSource = dtParticipantBeneficiaries
                    DataGridAnnuityBeneficiariesSav.DataBind()
                    'HideBeneficiarySavControls(True)
                    If Not (drBenexi Is Nothing) Then
                        drBen = drBenexi
                    Else
                        drBenCollection = dtParticipantBeneficiaries.Select("chvRelationshipCode = 'SP'")
                        If drBenCollection.Length > 0 Then
                            drBen = drBenCollection(0)
                        End If
                    End If
                    If Not (drBen Is Nothing) Then

                        AddSelectedParticipantBeneficiarySav(drBen)
                        'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor


                        'If Me.FundEventStatus <> "QD" And drBen("chvRelationshipCode").ToString.Trim() <> "SP" Then Commented by Chandra sekar 2016.04.08  YRS-AT-2915 - YRS Bug: Annuity purchase screen no longer pre-filling annuity beneficiary (TrackIT 25836)

                        'Start-Chandra sekar.c  2016.04.08  YRS-AT-2915 - YRS Bug: Annuity purchase screen no longer pre-filling annuity beneficiary (TrackIT 25836)
                        If Me.FundEventStatus = "QD" And drBen("chvRelationshipCode").ToString.Trim() = "SP" Then
                            Session("IsDefaultBeneficiarySpouseSav") = True
                        Else
                            'END-Chandra sekar.c  2016.04.08  YRS-AT-2915 - YRS Bug: Annuity purchase screen no longer pre-filling annuity beneficiary (TrackIT 25836)
                            Session("IsDefaultBeneficiarySpouseSav") = False
                            If Not drBen("chrBeneficiaryTaxNumber").ToString() Is DBNull.Value Then
                                If Not drBen("chrBeneficiaryTaxNumber").ToString() = String.Empty Then
                                    TextBoxAnnuitySSNoSav.Text = drBen("chrBeneficiaryTaxNumber").ToString()
                                End If
                            End If

                            If Not drBen("BenLastName").ToString() Is DBNull.Value Then
                                If Not drBen("BenLastName").ToString() = String.Empty Then
                                    TextBoxAnnuityLastNameSav.Text = drBen("BenLastName").ToString()
                                End If
                            End If

                            If Not drBen("BenFirstName").ToString() Is DBNull.Value Then
                                If Not drBen("BenFirstName").ToString() = String.Empty Then
                                    TextBoxAnnuityFirstNameSav.Text = drBen("BenFirstName").ToString()
                                End If
                            End If

                            If Not drBen("BenBirthDate").ToString() Is DBNull.Value Then
                                If Not drBen("BenBirthDate").ToString() = String.Empty Then
                                    TextBoxAnnuityBirthDateSav.Text = Convert.ToDateTime(drBen("BenBirthDate")).ToString("MM/dd/yyyy")
                                End If
                            End If
                            If Not drBen("chvRelationshipCode").ToString() Is DBNull.Value Then
                                DropDownRelationShipSav.SelectedValue = drBen("chvRelationshipCode").ToString.Trim()
                            End If
                            While i < Me.DataGridAnnuityBeneficiariesSav.Items.Count
                                l_button_select = New ImageButton
                                l_button_select = Me.DataGridAnnuityBeneficiariesSav.Items(i).FindControl("ImageButtonAccounts")
                                If Me.DataGridAnnuityBeneficiariesSav.Items(i).Cells(1).Text.Trim().ToUpper() = drBen("guiUniqueID").ToString().ToUpper() Then
                                    If Not l_button_select Is Nothing Then
                                        l_button_select.ImageUrl = "images\selected.gif"
                                    End If
                                Else
                                    If Not l_button_select Is Nothing Then
                                        l_button_select.ImageUrl = "images\select.gif"
                                    End If
                                End If
                                i = i + 1
                            End While

                        End If
                        'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                    End If
                Else
                    DataGridAnnuityBeneficiariesSav.Visible = False
                    LabelNoBeneficiarySav.Visible = Not (HelperFunctions.isNonEmpty(dtParticipantBeneficiaries))
                    'Else
                    '	HideBeneficiarySavControls(False)
                End If
            End If

        Catch
            Throw
        End Try
    End Sub
    Private Sub AddSelectedParticipantBeneficiary(ByVal paraRow As DataRow)
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            Dim dsParticipantBenificiary As New DataSet()
            If (Session("dsSelectedParticipantBeneficiary") Is Nothing) Then
                dsParticipantBenificiary.Tables.Add(BeneficiaryInfo.Tables(0).Clone())
            Else
                dsParticipantBenificiary = CType(Session("dsSelectedParticipantBeneficiary"), DataSet)
            End If

            dsParticipantBenificiary.Tables(0).Rows.Clear()
            dsParticipantBenificiary.Tables(0).ImportRow(paraRow)
            Session("dsSelectedParticipantBeneficiary") = dsParticipantBenificiary
        Catch
            Throw
        End Try
    End Sub

    Private Sub UpdateSelectedParticiapntBeneficary()
        Dim dsParticipantBenificiary As New DataSet()
        Dim dr As DataRow
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            If (HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiary"))) Then
                dsParticipantBenificiary = CType(Session("dsSelectedParticipantBeneficiary"), DataSet)
                If (TextBoxAnnuityBirthDateRet.Text.Trim() <> String.Empty) Then
                    dr = dsParticipantBenificiary.Tables(0).Rows(0)
                    dr("BenBirthDate") = TextBoxAnnuityBirthDateRet.Text.Trim()
                    dr("BenLastName") = TextBoxAnnuityLastNameRet.Text.Trim()
                    dr("BenFirstName") = TextBoxAnnuityFirstNameRet.Text.Trim()
                    dr("chvRelationshipCode") = DropDownRelationShipRet.SelectedValue
                    dr("chrBeneficiaryTaxNumber") = TextBoxAnnuitySSNoRet.Text.Trim()
                Else
                    dsParticipantBenificiary.Tables(0).Rows.Clear()
                End If
                dsParticipantBenificiary.Tables(0).AcceptChanges()
                Session("dsSelectedParticipantBeneficiary") = dsParticipantBenificiary
            Else
                Dim dtBenificiary As New DataTable
                dtBenificiary = Me.BeneficiaryInfo.Tables(0).Clone()
                dsParticipantBenificiary.Tables.Add(dtBenificiary)
                If (TextBoxAnnuityBirthDateRet.Text.Trim() <> String.Empty) Then
                    dr = dsParticipantBenificiary.Tables(0).NewRow()
                    dr("BenBirthDate") = TextBoxAnnuityBirthDateRet.Text.Trim()
                    dr("BenLastName") = TextBoxAnnuityLastNameRet.Text.Trim()
                    dr("BenFirstName") = TextBoxAnnuityFirstNameRet.Text.Trim()
                    dr("chvRelationshipCode") = DropDownRelationShipRet.SelectedValue
                    dr("chrBeneficiaryTaxNumber") = TextBoxAnnuitySSNoRet.Text.Trim()
                    dsParticipantBenificiary.Tables(0).Rows.Add(dr)
                    dsParticipantBenificiary.Tables(0).AcceptChanges()
                End If
                Session("dsSelectedParticipantBeneficiary") = dsParticipantBenificiary
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub DeselectedAnnuityBeneficiaryGrid()
        Try
            If (Me.DataGridAnnuityBeneficiaries.Visible) Then
                Dim l_button_select As ImageButton
                Dim i As Integer
                DataGridAnnuityBeneficiaries.SelectedIndex = -1
                While i < Me.DataGridAnnuityBeneficiaries.Items.Count
                    l_button_select = New ImageButton
                    l_button_select = Me.DataGridAnnuityBeneficiaries.Items(i).FindControl("ImageButtonAccounts")
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\select.gif"
                    End If
                    i = i + 1
                End While
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub EnableDisableValidationBeneficiary(ByVal isVisible As Boolean)
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            RequiredFieldValidatorAnnuitySSNoRet.Enabled = isVisible
            RequiredfieldvalidatorAnnuityLastNameRet.Enabled = isVisible
            RequiredfieldvalidatorAnnuityFirstNameRet.Enabled = isVisible
            'RequiredfieldvalidatorAnnuityBirthDateRet.Enabled = isVisible 'PPP | 2015.09.25 | YRS-AT-2596
            RequiredfieldvalidatornRelationShipRet.Enabled = isVisible
        Catch ex As Exception
            Throw
        End Try
    End Sub

    '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End
    '2012.06.04  SP:  BT-975/YRS 5.0-1508 -start
    Private Sub EnableDisableValidationBeneficiarySav(ByVal isVisible As Boolean)
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            RequiredFieldValidatorAnnuitySSNoSav.Enabled = isVisible
            RequiredfieldvalidatorAnnuityLastNameSav.Enabled = isVisible
            RequiredfieldvalidatorAnnuityFirstNameSav.Enabled = isVisible
            'RequiredfieldvalidatorAnnuityBirthDateSav.Enabled = isVisible 'PPP | 2015.09.25 | YRS-AT-2596
            RequiredfieldvalidatornRelationShipSav.Enabled = isVisible
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub AddSelectedParticipantBeneficiarySav(ByVal paraRow As DataRow)
        Dim dsParticipantBenificiary As New DataSet()
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            If (Session("dsSelectedParticipantBeneficiarySav") Is Nothing) Then
                dsParticipantBenificiary.Tables.Add(BeneficiaryInfo.Tables(0).Clone())
            Else
                dsParticipantBenificiary = CType(Session("dsSelectedParticipantBeneficiarySav"), DataSet)
            End If

            dsParticipantBenificiary.Tables(0).Rows.Clear()
            dsParticipantBenificiary.Tables(0).ImportRow(paraRow)
            Session("dsSelectedParticipantBeneficiarySav") = dsParticipantBenificiary
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub DeselectedAnnuityBeneficiaryGridSav()
        Try
            If (Me.DataGridAnnuityBeneficiariesSav.Visible) Then
                Dim l_button_select As ImageButton
                Dim i As Integer
                DataGridAnnuityBeneficiariesSav.SelectedIndex = -1
                While i < Me.DataGridAnnuityBeneficiariesSav.Items.Count
                    l_button_select = New ImageButton
                    l_button_select = Me.DataGridAnnuityBeneficiariesSav.Items(i).FindControl("ImageButtonAccounts")
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\select.gif"
                    End If
                    i = i + 1
                End While
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub HideBeneficiaryRetControls(ByVal isVisible As Boolean)
        'EnableDisableValidationBeneficiary(isVisible)
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            LabelRetirementBeneficiary.Visible = isVisible
            TextBoxAnnuityLastNameRet.Visible = isVisible
            TextBoxAnnuityMiddleNameRet.Visible = isVisible
            TextBoxAnnuityFirstNameRet.Visible = isVisible

            spnBoxAnnuityBirthDateRet.Visible = isVisible
            divRetbenef.Visible = isVisible
            'Popcalendar3.Visible = isVisible 'PPP | 2015.09.25 | YRS-AT-2596
            DropDownRelationShipRet.Visible = isVisible
            TextBoxAnnuitySSNoRet.Visible = isVisible
            LabelSSNo2Ret.Visible = isVisible
            LabelLastName2Ret.Visible = isVisible
            LabelFirstName2Ret.Visible = isVisible
            LabelMiddleName2Ret.Visible = isVisible
            LabelBirthDate2Ret.Visible = isVisible
            LabelRealtionRet.Visible = isVisible
            'LabelNoBeneficiary.Visible = Not isVisible
            ButtonClearBeneficiary.Visible = isVisible
            DataGridAnnuityBeneficiaries.Visible = isVisible
            'Anudeep:28.06.2013 : YRS 5.0-1745:Capture Beneficiary Address
            AddressWebUserControlRet.Visible = isVisible
            'Anudeep:16.02.2014 BT:2306: YRS 5.0-2251 : added to set visiblity
            lnkParticipantAddressRet.Visible = isVisible
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub HideBeneficiarySavControls(ByVal isVisible As Boolean)
        'EnableDisableValidationBeneficiarySav(isVisible)
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            LabelSavingsBenefeciary.Visible = isVisible
            TextBoxAnnuityLastNameSav.Visible = isVisible
            TextBoxAnnuityMiddleNameSav.Visible = isVisible
            TextBoxAnnuityFirstNameSav.Visible = isVisible
            'LabelNoBeneficiarySav.Visible = Not isVisible
            divSavbenef.Visible = isVisible
            spnBoxAnnuityBirthDateSav.Visible = isVisible
            'PopcalendarSaving.Visible = isVisible 'PPP | 2015.09.25 | YRS-AT-2596
            DropDownRelationShipSav.Visible = isVisible
            TextBoxAnnuitySSNoSav.Visible = isVisible
            LabelSSNo2Sav.Visible = isVisible
            LabelLastName2Sav.Visible = isVisible
            LabelFirstName2Sav.Visible = isVisible
            LabelMiddleName2Sav.Visible = isVisible
            LabelBirthDate2Sav.Visible = isVisible
            LabelRealtionSav.Visible = isVisible
            ButtonClearBeneficiarySav.Visible = isVisible
            DataGridAnnuityBeneficiariesSav.Visible = isVisible
            'Anudeep:28.06.2013 : YRS 5.0-1745:Capture Beneficiary Address
            AddressWebUserControlSav.Visible = isVisible
            'Anudeep:16.02.2014 BT:2306: YRS 5.0-2251 : added to set visiblity
            lnkParticipantAddressSav.Visible = isVisible
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UpdateSelectedParticiapntBeneficarySav()
        Dim dsParticipantBenificiary As New DataSet()
        Dim dr As DataRow
        'SP 2013.12.13 BT-2326 -Added try catch block
        Try
            If (HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiarySav"))) Then
                dsParticipantBenificiary = CType(Session("dsSelectedParticipantBeneficiarySav"), DataSet)
                If (TextBoxAnnuityBirthDateSav.Text.Trim() <> String.Empty) Then
                    dr = dsParticipantBenificiary.Tables(0).Rows(0)
                    dr("BenBirthDate") = TextBoxAnnuityBirthDateSav.Text.Trim()
                    dr("BenLastName") = TextBoxAnnuityLastNameSav.Text.Trim()
                    dr("BenFirstName") = TextBoxAnnuityFirstNameSav.Text.Trim()
                    dr("chvRelationshipCode") = DropDownRelationShipSav.SelectedValue
                    dr("chrBeneficiaryTaxNumber") = TextBoxAnnuitySSNoSav.Text.Trim()
                Else
                    dsParticipantBenificiary.Tables(0).Rows.Clear()
                End If
                dsParticipantBenificiary.Tables(0).AcceptChanges()
                Session("dsSelectedParticipantBeneficiarySav") = dsParticipantBenificiary
            Else
                Dim dtBenificiary As New DataTable
                dtBenificiary = Me.BeneficiaryInfo.Tables(0).Clone()
                dsParticipantBenificiary.Tables.Add(dtBenificiary)
                If (TextBoxAnnuityBirthDateSav.Text.Trim() <> String.Empty) Then
                    dr = dsParticipantBenificiary.Tables(0).NewRow()
                    dr("BenBirthDate") = TextBoxAnnuityBirthDateSav.Text.Trim()
                    dr("BenLastName") = TextBoxAnnuityLastNameSav.Text.Trim()
                    dr("BenFirstName") = TextBoxAnnuityFirstNameSav.Text.Trim()
                    dr("chvRelationshipCode") = DropDownRelationShipSav.SelectedValue
                    dr("chrBeneficiaryTaxNumber") = TextBoxAnnuitySSNoSav.Text.Trim()
                    dsParticipantBenificiary.Tables(0).Rows.Add(dr)
                    dsParticipantBenificiary.Tables(0).AcceptChanges()
                End If
                Session("dsSelectedParticipantBeneficiarySav") = dsParticipantBenificiary
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    '2012.06.04  SP:  BT-975/YRS 5.0-1508 -End
#End Region

    Private Sub rdbListAnnuityOptionRet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbListAnnuityOptionRet.SelectedIndexChanged
        Try
            Session("SelectedCalledAnnuity_Ret") = rdbListAnnuityOptionRet.SelectedValue.Trim().ToUpper()
            'calculateAnnuity("B")
            LoadPurchaseTab()
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("rdbListAnnuityOptionRet_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub

    Private Sub rdbListAnnuityOptionSav_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbListAnnuityOptionSav.SelectedIndexChanged
        Try
            Session("SelectedCalledAnnuity_Sav") = rdbListAnnuityOptionSav.SelectedValue.Trim().ToUpper()
            'calculateAnnuity("B")
            LoadPurchaseTab()
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("rdbListAnnuityOptionSav_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub


    '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -Start
    Private Sub DataGridAnnuityBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAnnuityBeneficiaries.SelectedIndexChanged
        Dim i As Integer
        Dim retirementType As String
        Dim l_button_select As ImageButton
        Dim drrow As DataRow()
        Dim dtAddress As DataTable
        Dim errorMessage As String 'Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
        Try
            drrow = Me.BeneficiaryInfo.Tables(0).Select("guiUniqueID ='" + Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(1).Text.Trim + "'")
            Me.AddSelectedParticipantBeneficiary(drrow(0))
            If (drrow.Length > 0) Then
                'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim.ToUpper() <> "&NBSP;" Then
                    If Me.FundEventStatus = "QD" And Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim() = "SP" Then
                        Session("IsDefaultBeneficiarySpouse") = True
                        TextBoxAnnuitySSNoRet.Text = String.Empty
                        TextBoxAnnuityFirstNameRet.Text = String.Empty
                        TextBoxAnnuityLastNameRet.Text = String.Empty
                        TextBoxAnnuityBirthDateRet.Text = String.Empty
                        TextBoxAnnuityMiddleNameRet.Text = String.Empty
                        DropDownRelationShipRet.SelectedIndex = -1
                        DeselectedAnnuityBeneficiaryGrid()
                        EnableDisableValidationBeneficiary(False)
                        errorMessage = combinemessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_BLOCK_JSANNUITY_FOR_QDRO"))
                        If errorMessage <> "" Then
                            HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing)

                        End If
                    Else
                        'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                        If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(2).Text.Trim.ToUpper() <> "&NBSP;" Then
                            TextBoxAnnuitySSNoRet.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(2).Text
                        Else
                            TextBoxAnnuitySSNoRet.Text = String.Empty
                        End If

                        If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(3).Text.Trim.ToUpper() <> "&NBSP;" Then
                            TextBoxAnnuityFirstNameRet.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(3).Text
                        Else
                            TextBoxAnnuityFirstNameRet.Text = String.Empty
                        End If
                        If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(4).Text.Trim.ToUpper() <> "&NBSP;" Then
                            TextBoxAnnuityLastNameRet.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(4).Text
                        Else
                            TextBoxAnnuityLastNameRet.Text = String.Empty
                        End If
                        If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(5).Text.Trim.ToUpper() <> "&NBSP;" Then
                            TextBoxAnnuityBirthDateRet.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(5).Text
                        Else
                            TextBoxAnnuityBirthDateRet.Text = String.Empty
                        End If
                        If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim.ToUpper() <> "&NBSP;" Then
                            DropDownRelationShipRet.SelectedValue = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim()
                            Session("IsDefaultBeneficiarySpouse") = False
                        End If
                        EnableDisableValidationBeneficiary(True)
                        'Anudeep:04.07.2013 :BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        If TextBoxAnnuitySSNoRet.Text <> "" Then
                            dtAddress = BeneficiaryAddress
                            If HelperFunctions.isNonEmpty(dtAddress) Then
                                AddressWebUserControlRet.LoadAddressDetail(dtAddress.Select("BenSSNo='" & TextBoxAnnuitySSNoRet.Text & "'"))
                            End If
                        End If


                        'Call ButtonReCalculate_Click(New Object, New System.EventArgs)
                        'LabelEstimateDataChangedMessage.Visible = True
                        'HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENFICIARY_INFORMATION_CHANGED"), EnumMessageTypes.Information)
                        ViewState("IsRetBenificiaryChanged") = True  'SP 2014.09.17 YRS 5.0-2362
                        While i < Me.DataGridAnnuityBeneficiaries.Items.Count
                            l_button_select = New ImageButton
                            l_button_select = Me.DataGridAnnuityBeneficiaries.Items(i).FindControl("ImageButtonAccounts")
                            If i = Me.DataGridAnnuityBeneficiaries.SelectedIndex Then
                                If Not l_button_select Is Nothing Then
                                    l_button_select.ImageUrl = "images\selected.gif"
                                End If
                            Else
                                If Not l_button_select Is Nothing Then
                                    l_button_select.ImageUrl = "images\select.gif"
                                End If

                            End If
                            i = i + 1
                        End While

                        Page.Validate()


                        ViewState("BenificiaryBirthDate") = TextBoxAnnuityBirthDateRet.Text
                    End If
                End If
            End If


        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DataGridAnnuityBeneficiaries_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub


    Private Sub DropDownRelationShipRet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownRelationShipRet.SelectedIndexChanged
        'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
        Dim errorMessage As String
        Try
            If Me.FundEventStatus = "QD" And DropDownRelationShipRet.SelectedValue = "SP" Then
                DropDownRelationShipRet.SelectedIndex = -1
                Session("IsDefaultBeneficiarySpouse") = True
                errorMessage = combinemessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_BLOCK_JSANNUITY_FOR_QDRO"))
                If errorMessage <> "" Then
                    HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing)
                    Exit Sub
                End If
            Else
                Session("IsDefaultBeneficiarySpouse") = False
                'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor

                If (DropDownRelationShipRet.SelectedIndex) Then
                    DeselectedAnnuityBeneficiaryGrid()
                    'LabelEstimateDataChangedMessage.Visible = True
                    'HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENFICIARY_INFORMATION_CHANGED"), EnumMessageTypes.Information)
                    ViewState("IsRetBenificiaryChanged") = True 'SP 2014.09.17 YRS 5.0-2362
                    EnableDisableValidationBeneficiary(True)
                    'Call ButtonReCalculate_Click(New Object, New System.EventArgs)
                End If
            End If
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DropDownRelationShipRet_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub

    Private Sub ButtonClearBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClearBeneficiary.Click
        Try
            DeselectedAnnuityBeneficiaryGrid()
            'LabelEstimateDataChangedMessage.Visible = True
            'HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENFICIARY_INFORMATION_CHANGED"), EnumMessageTypes.Information)
            TextBoxAnnuityLastNameRet.Text = String.Empty
            TextBoxAnnuityMiddleNameRet.Text = String.Empty
            TextBoxAnnuityFirstNameRet.Text = String.Empty
            TextBoxAnnuityBirthDateRet.Text = String.Empty
            DropDownRelationShipRet.SelectedIndex = -1
            TextBoxAnnuitySSNoRet.Text = String.Empty
            EnableDisableValidationBeneficiary(False)
            ViewState("BenificiaryBirthDate") = TextBoxAnnuityBirthDateRet.Text
            AddressWebUserControlRet.ClearControls = "" 'SP 2013.12.26 - BT-2340 -Address not getting cleared in Retirement Process - Annuity Benificiary tab
            ViewState("IsRetBenificiaryChanged") = True  'SP 2014.09.17 YRS 5.0-2362
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonClearBeneficiary_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try

    End Sub
    '2012.05.18  SP:  BT-975/YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -End
    '2012.06.04 SP : Start
    Private Sub DataGridAnnuityBeneficiariesSav_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAnnuityBeneficiariesSav.SelectedIndexChanged
        Dim i As Integer
        Dim retirementType As String
        Dim l_button_select As ImageButton
        Dim drrow As DataRow
        Dim dtAddress As DataTable
        Dim errorMessage As String 'Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
        Try

            drrow = Me.BeneficiaryInfo.Tables(0).NewRow()
            'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
            If Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(6).Text.Trim.ToUpper() <> "&NBSP;" Then
                If Me.FundEventStatus = "QD" And Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(6).Text.Trim() = "SP" Then
                    Session("IsDefaultBeneficiarySpouseSav") = True
                    TextBoxAnnuitySSNoSav.Text = String.Empty
                    TextBoxAnnuityFirstNameSav.Text = String.Empty
                    TextBoxAnnuityLastNameSav.Text = String.Empty
                    TextBoxAnnuityBirthDateSav.Text = String.Empty
                    TextBoxAnnuityMiddleNameSav.Text = String.Empty
                    DropDownRelationShipSav.SelectedIndex = -1
                    DeselectedAnnuityBeneficiaryGridSav()

                    EnableDisableValidationBeneficiarySav(False)

                    errorMessage = combinemessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_BLOCK_JSANNUITY_FOR_QDRO"))
                    If errorMessage <> "" Then
                        HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing)
                        Exit Sub
                    End If
                Else
                    'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                    drrow("guiUniqueID") = Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(1).Text.Trim()
                    If Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(2).Text.Trim.ToUpper() <> "&NBSP;" Then
                        TextBoxAnnuitySSNoSav.Text = Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(2).Text
                        drrow("chrBeneficiaryTaxNumber") = TextBoxAnnuitySSNoSav.Text.Trim()
                    Else
                        TextBoxAnnuitySSNoSav.Text = String.Empty
                    End If

                    If Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(3).Text.Trim.ToUpper() <> "&NBSP;" Then
                        TextBoxAnnuityFirstNameSav.Text = Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(3).Text
                        drrow("BenFirstName") = TextBoxAnnuityFirstNameSav.Text.Trim()
                    Else
                        TextBoxAnnuityFirstNameSav.Text = String.Empty
                    End If
                    If Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(4).Text.Trim.ToUpper() <> "&NBSP;" Then
                        TextBoxAnnuityLastNameSav.Text = Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(4).Text
                        drrow("BenLastName") = TextBoxAnnuityLastNameSav.Text.Trim()
                    Else
                        TextBoxAnnuityLastNameSav.Text = String.Empty
                    End If
                    If Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(5).Text.Trim.ToUpper() <> "&NBSP;" Then
                        TextBoxAnnuityBirthDateSav.Text = Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(5).Text
                        drrow("BenBirthDate") = TextBoxAnnuityBirthDateSav.Text.Trim()
                    Else
                        TextBoxAnnuityBirthDateSav.Text = String.Empty
                    End If
                    If Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(6).Text.Trim.ToUpper() <> "&NBSP;" Then
                        Session("IsDefaultBeneficiarySpouseSav") = False
                        DropDownRelationShipSav.SelectedValue = Me.DataGridAnnuityBeneficiariesSav.SelectedItem.Cells(6).Text.Trim()
                        drrow("chvRelationshipCode") = DropDownRelationShipSav.SelectedValue.Trim()
                    End If
                    EnableDisableValidationBeneficiarySav(True)
                    Me.AddSelectedParticipantBeneficiarySav(drrow)

                    'Anudeep:04.07.2013 :BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    If TextBoxAnnuitySSNoSav.Text <> "" Then
                        dtAddress = BeneficiaryAddress
                        If HelperFunctions.isNonEmpty(dtAddress) Then
                            'Anudeep:11.07.2013 :BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                            AddressWebUserControlSav.LoadAddressDetail(dtAddress.Select("BenSSNo='" & TextBoxAnnuitySSNoSav.Text & "'"))
                        End If

                    End If


                    'LabelEstimateDataChangedMessage.Visible = True
                    ' HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENFICIARY_INFORMATION_CHANGED"), EnumMessageTypes.Information)
                    ViewState("IsSavBenificiaryChanged") = True 'SP 2014.09.17 YRS 5.0-2362
                    'Call ButtonReCalculate_Click(New Object, New System.EventArgs)
                    While i < Me.DataGridAnnuityBeneficiariesSav.Items.Count
                        l_button_select = New ImageButton
                        l_button_select = Me.DataGridAnnuityBeneficiariesSav.Items(i).FindControl("ImageButtonAccounts")
                        If i = Me.DataGridAnnuityBeneficiariesSav.SelectedIndex Then
                            If Not l_button_select Is Nothing Then
                                l_button_select.ImageUrl = "images\selected.gif"
                            End If
                        Else
                            If Not l_button_select Is Nothing Then
                                l_button_select.ImageUrl = "images\select.gif"
                            End If

                        End If
                        i = i + 1
                    End While
                    Page.Validate()
                    ViewState("BenificiaryBirthDateSav") = TextBoxAnnuityBirthDateSav.Text
                End If

            End If

        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DataGridAnnuityBeneficiariesSav_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub

    Private Sub ButtonClearBeneficiarySav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClearBeneficiarySav.Click
        Try
            DeselectedAnnuityBeneficiaryGridSav()
            'LabelEstimateDataChangedMessage.Visible = True
            'HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENFICIARY_INFORMATION_CHANGED"), EnumMessageTypes.Information)
            TextBoxAnnuityLastNameSav.Text = String.Empty
            TextBoxAnnuityMiddleNameSav.Text = String.Empty
            TextBoxAnnuityFirstNameSav.Text = String.Empty
            TextBoxAnnuityBirthDateSav.Text = String.Empty
            DropDownRelationShipSav.SelectedIndex = -1
            TextBoxAnnuitySSNoSav.Text = String.Empty
            EnableDisableValidationBeneficiarySav(False)
            ViewState("BenificiaryBirthDateSav") = TextBoxAnnuityBirthDateSav.Text
            AddressWebUserControlSav.ClearControls = "" 'SP 2013.12.26 - BT-2340 -Address not getting cleared in Retirement Process - Annuity Benificiary tab
            ViewState("IsSavBenificiaryChanged") = True 'SP 2014.09.17 YRS 5.0-2362
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("ButtonClearBeneficiarySav_Click-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub


    Private Sub DropDownRelationShipSav_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownRelationShipSav.SelectedIndexChanged
        'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
        Dim errorMessage As String
        Try
            If Me.FundEventStatus = "QD" And DropDownRelationShipSav.SelectedValue = "SP" Then
                DropDownRelationShipSav.SelectedIndex = -1
                errorMessage = combinemessage(getmessage("MESSAGE_RETIREMENT_PROCESSING_BLOCK_JSANNUITY_FOR_QDRO"))
                Session("IsDefaultBeneficiarySpouseSav") = True
                If errorMessage <> "" Then
                    HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error, Nothing)
                    Exit Sub
                End If
            Else
                Session("IsDefaultBeneficiarySpouseSav") = False
                'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                If (DropDownRelationShipSav.SelectedIndex) Then
                    DeselectedAnnuityBeneficiaryGridSav()
                    'LabelEstimateDataChangedMessage.Visible = True
                    'HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENFICIARY_INFORMATION_CHANGED"), EnumMessageTypes.Information)
                    EnableDisableValidationBeneficiarySav(True)
                    'Call ButtonReCalculate_Click(New Object, New System.EventArgs)
                    ViewState("IsSavBenificiaryChanged") = True 'SP 2014.09.17 YRS 5.0-2362
                End If
            End If
        Catch ex As Exception
            'SP 2013.12.13 BT-2326 -Start
            HelperFunctions.LogException("DropDownRelationShipSav_SelectedIndexChanged-RetirementProcessing", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
            'SP 2013.12.13 BT-2326 -End
        End Try
    End Sub
    '2012.06.04 SP : END
    '2012.09.21 added by anudeep :start
    'gets the message from resource file
    Public Function getmessage(ByVal resourcemessage As String)
        Try

            Dim strMessage As String
            Try
                strMessage = GetGlobalResourceObject("RetirementMessages", resourcemessage).ToString()
            Catch ex As Exception
                HelperFunctions.LogException(resourcemessage + " key not found in resource file.", ex)
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim LabelModuleName As Label
        Try

            LabelModuleName = Master.FindControl("LabelModuleName")
            If Not LabelModuleName Is Nothing AndAlso Not String.IsNullOrEmpty(PersonInformationForMenuHeader) Then
                LabelModuleName.Text = "Activities > Retirement > Process > " + Request.QueryString.Get("RetType").Trim() + PersonInformationForMenuHeader
            End If
            'SP 2014.09.23  YRS 5.0-2362-Start
            If (ViewState("IsRetBenificiaryChanged") IsNot Nothing AndAlso ViewState("IsRetBenificiaryChanged") = True) OrElse (ViewState("IsSavBenificiaryChanged") IsNot Nothing AndAlso ViewState("IsSavBenificiaryChanged") = True) Then
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_BENFICIARY_INFORMATION_CHANGED"), EnumMessageTypes.Information)
                Exit Sub
            End If
            If (Me.DataChanged = True) Then
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_PROCESSING_PARTICIPANT_DATA_HAS_CHANGED"), EnumMessageTypes.Information)
                Exit Sub
            End If

            'SP 2014.09.23  YRS 5.0-2362-End
        Catch ex As Exception
            HelperFunctions.LogException("RetirementProcessing --> Page_PreRender ", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error, Nothing)
        End Try
    End Sub
    'Start:Anudeep:16.02.2013:BT:2306:YRS 5.0-2251 : Added code to fill the participant address in address control
    Private Sub lnkParticipantAddressRet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkParticipantAddressRet.Click
        Dim dr_PrimaryAddress As DataRow()
        Dim dsAddress As DataSet
        Try
            If Session("PersId") IsNot Nothing Then
                dsAddress = Address.GetAddressByEntity(Session("PersId").ToString(), EnumEntityCode.PERSON)
                If HelperFunctions.isNonEmpty(dsAddress) Then
                    dr_PrimaryAddress = dsAddress.Tables("Address").Select("isPrimary = True")
                End If
            End If
            If dr_PrimaryAddress IsNot Nothing AndAlso dr_PrimaryAddress.Length > 0 Then
                dr_PrimaryAddress(0)("UniqueId") = ""
                dr_PrimaryAddress(0)("guiEntityId") = ""
                dr_PrimaryAddress(0)("effectiveDate") = Today.ToShortDateString 'AA:26.05.2014 BT:2306:YRS 5.0-2251 - Beneficiary effective date changed to current date from particiapant effective date
                AddressWebUserControlRet.LoadAddressDetail(dr_PrimaryAddress)
                AddressWebUserControlRet.Notes = "Address has been added / updated from participant address."
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub lnkParticipantAddressSav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkParticipantAddressSav.Click
        Dim dr_PrimaryAddress As DataRow()
        Dim dsAddress As DataSet
        Try
            If Session("PersId") IsNot Nothing Then
                dsAddress = Address.GetAddressByEntity(Session("PersId").ToString(), EnumEntityCode.PERSON)
                If HelperFunctions.isNonEmpty(dsAddress) Then
                    dr_PrimaryAddress = dsAddress.Tables("Address").Select("isPrimary = True")
                End If
            End If
            If dr_PrimaryAddress IsNot Nothing AndAlso dr_PrimaryAddress.Length > 0 Then
                dr_PrimaryAddress(0)("UniqueId") = ""
                dr_PrimaryAddress(0)("guiEntityId") = ""
                dr_PrimaryAddress(0)("effectiveDate") = Today.ToShortDateString 'AA:26.05.2014 BT:2306:YRS 5.0-2251 - Beneficiary effective date changed to current date from participant effective date
                AddressWebUserControlSav.LoadAddressDetail(dr_PrimaryAddress)
                AddressWebUserControlSav.Notes = "Address has been added / updated from participant address."
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'End:Anudeep:16.02.2013:BT:2306:YRS 5.0-2251 : Added code to fill the participant address in address control

    ' // START : SR | 2016.08.02 | YRS-AT-2382 | Maintaing the Beneficiaries Record for SSN updates 
    Private Sub PrepareAuditTable()
        Dim dtSSNUpdate, dtExistingTable As DataTable
        Dim dtExistingDataSet As DataSet
        Dim iBeneficiaryCount As Integer
        Try
            If Session("AuditBeneficiariesTable") Is Nothing Then
                Session("AuditBeneficiariesTable") = CreateAuditTable()
            End If

            dtSSNUpdate = CType(Session("AuditBeneficiariesTable"), DataTable)

            If (Session("BeneficiariesRetired") IsNot Nothing) Then
                dtExistingDataSet = CType(Session("BeneficiariesRetired"), DataSet)
                dtExistingTable = dtExistingDataSet.Tables(0)
                iBeneficiaryCount = dtExistingTable.Rows.Count
                Dim dr As DataRow
                For Each drExisting As DataRow In dtExistingTable.Rows
                    If Not drExisting.RowState = DataRowState.Deleted AndAlso dtSSNUpdate.Select(String.Format("UniqueId='{0}'", Convert.ToString(drExisting("UniqueId")))).Length = 0 Then
                        dr = dtSSNUpdate.NewRow()
                        dr("ModuleName") = "Retirement Processing"
                        dr("UniqueId") = drExisting("UniqueId")
                        dr("EntityType") = "Beneficiary"
                        dr("chvColumn") = "chrSSNo"
                        dr("OldSSN") = drExisting("TaxID")
                        dr("NewSSN") = String.Empty
                        dr("Reason") = String.Empty
                        dr("IsEdited") = "False"
                        dtSSNUpdate.Rows.Add(dr)
                    End If
                Next
            End If
            Session("AuditBeneficiariesTable") = dtSSNUpdate
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function CreateAuditTable() As DataTable
        Dim dtSSNUpdate As DataTable
        Try
            dtSSNUpdate = New DataTable()
            dtSSNUpdate.Columns.Add("ModuleName")
            dtSSNUpdate.Columns.Add("UniqueId")
            dtSSNUpdate.Columns.Add("EntityType")
            dtSSNUpdate.Columns.Add("chvColumn")
            dtSSNUpdate.Columns.Add("OldSSN")
            dtSSNUpdate.Columns.Add("NewSSN")
            dtSSNUpdate.Columns.Add("Reason")
            dtSSNUpdate.Columns.Add("IsEdited")
            Return dtSSNUpdate
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Maintaing the Beneficiaries Record for SSN updates 

    'START: MMR | 2017.03.06 | YRS-AT-2625 | Displaying manual transaction list
    Private Sub LoadManualTransactionForDisability(ByVal fundEventId As String, ByVal retireeType As String, ByVal retirementDate As Date)
        Dim transactionList As New DataSet
        Dim warningMessage As String = ""
        ShowHideManualTransactionLink(False)
        If retireeType = "DISABL" Then
            If Not Me.ManualTransactionDetails Is Nothing AndAlso Me.RetirementDate = Convert.ToDateTime(TextBoxRetirementDate.Text) AndAlso hdnMessage.Value = "" Then 'START: MMR | 2017.03.16 | YRS-AT-2625 | Added validation - check if retirement date has not changed then reload MAPR grid from session
                transactionList = Me.ManualTransactionDetails
            Else
                transactionList = YMCARET.YmcaBusinessObject.RetirementBOClass.GetManualTransactions(fundEventId, retireeType, retirementDate)
            End If
            If HelperFunctions.isNonEmpty(transactionList) Then
                ShowHideManualTransactionLink(True)
                If hdnManualTransaction.Value <> "3" Then
                    hdnManualTransaction.Value = "2"
                    warningMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_MANUALTRANSACTION_EXISTS")
                    If Not String.IsNullOrEmpty(warningMessage) Then
                        DivWarningMessage.InnerHtml = warningMessage
                        DivWarningMessage.Style("display") = "normal"
                    End If
                End If
                DatagridManualTransactionList.DataSource = transactionList.Tables("ManualTransactionDetails")
                Me.ManualTransactionDetails = transactionList
            Else
                DatagridManualTransactionList.DataSource = Nothing
            End If
            DatagridManualTransactionList.DataBind()
        End If
    End Sub
    'END: MMR | 2017.03.06 | YRS-AT-2625 | Displaying manual transaction list

    'START: MMR | 2017.03.06 | YRS-AT-2625 | Show/Hide Manual Transaction link
    Private Sub ShowHideManualTransactionLink(ByVal isVisible As Boolean)
        lnkManualTransaction.Visible = isVisible
    End Sub
    'END: MMR | 2017.03.06 | YRS-AT-2625 | Show/Hide Manual Transaction link

    'START: MMR | 2017.03.06 | YRS-AT-2625 | Storing selected manual transaction for calculating estimates
    <WebMethod(True)> _
    Public Shared Function GetSelectedManualTransactions(ByVal uniqueIDs As String)
        Dim transactionList As New DataSet

        If Not HttpContext.Current.Session("ManualTransaction") Is Nothing Then
            transactionList = DirectCast(HttpContext.Current.Session("ManualTransaction"), DataSet)
            For Each transactionListRow As DataRow In transactionList.Tables("ManualTransactionDetails").Rows
                transactionListRow("Selected") = True
                If Not String.IsNullOrEmpty(uniqueIDs) AndAlso uniqueIDs.IndexOf(Convert.ToString(transactionListRow("UniqueId")), System.StringComparison.CurrentCultureIgnoreCase) >= 0 Then
                    transactionListRow("Selected") = False
                End If
            Next
            HttpContext.Current.Session("ManualTransaction") = transactionList
        End If
    End Function
    'END: MMR | 2017.03.06 | YRS-AT-2625 | Storing selected manual transaction for calculating estimates

    'START: MMR | 2017.03.06 | YRS-AT-2625 | Added to maintain hidden field value across postbacks
#Region "Local Variable Persistence mechanism"
    Protected Overrides Function SaveViewState() As Object
        Dim al As New ArrayList
        al.Add(StoreLocalVariablesToCache())
        al.Add(MyBase.SaveViewState())
        Return al
    End Function

    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        Dim al As ArrayList = DirectCast(savedState, ArrayList)
        InitializeLocalVariablesFromCache(al.Item(0))
        MyBase.LoadViewState(al.Item(1))
    End Sub

    Private Sub InitializeLocalVariablesFromCache(ByRef obj As Object)
        Try
            'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate
            hdnManualTransaction.Value = Session("ManualTransact")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function StoreLocalVariablesToCache() As Object
        Try
            'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
            Session("ManualTransact") = hdnManualTransaction.Value
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
    'END: MMR | 2017.03.06 | YRS-AT-2625 | Added to maintain hidden field value across postbacks

    'START: MMR | 2017.03.14 | YRS-AT-2625 | Resetting manual transaction details
    Private Sub ResetManualTransactionDetails()
        ShowHideManualTransactionLink(False)
        Me.ManualTransactionDetails = Nothing
        hdnManualTransaction.Value = 1
        DatagridManualTransactionList.DataSource = Nothing
    End Sub
    'END: MMR | 2017.03.14 | YRS-AT-2625 | Resetting manual transaction details

    'START: MMR | 2017.03.24 | YRS-AT-2625 | Added method to load manual transaction details on post back on change of retirement date
    Private Sub DisplayManualTransactionDetails()
        If Page.IsPostBack Then
            If (Me.RetireType = "DISABL") Then
                'Dialog box should not open if only savings plan selected
                If CheckBoxRetPlan.Checked = False Then
                    ResetManualTransactionDetails()
                    DivWarningMessage.Style("display") = "none"
                    'Manual transaction should be loaded for retirement plan type selection
                ElseIf CheckBoxRetPlan.Checked = True AndAlso TextBoxRetirementDate.Text.Trim() <> "" Then
                    If hdnMessage.Value = "" Then
                        LoadManualTransactionForDisability(Me.guiFundEventId, Me.RetireType, TextBoxRetirementDate.Text)
                    End If
                End If
                If hdnManualTransaction.Value = "3" Then
                    DivWarningMessage.Style("display") = "none"
                End If
            End If
        End If
    End Sub
    'END: MMR | 2017.03.24 | YRS-AT-2625 | Added method to load manual transaction details on post back on change of retirement date
    'START : BD : 2018.11.19 : YRS-AT-4135 : Method to check insured reserve annuity exists or not
    Private Function HasInsuredReserveAnnuity(dsAnnuityDetails As DataSet) As Boolean
        If HelperFunctions.isNonEmpty(dsAnnuityDetails) And dsAnnuityDetails.Tables(0).Select("bitInsuredReserve='True'").Length > 0 Then
            Return True
        End If
        Return False
    End Function
    'END : BD : 2018.11.19 : YRS-AT-4135 : Method to check insured reserve annuity exists or not   

    'START: Shilpa N | 03/15/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            ButtonAddItemNotes.Enabled = False
            ButtonAddItemNotes.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            For Each row In DataGridNotes.Items
                Dim chkmarkimp As CheckBox = (TryCast((TryCast(row, TableRow)).Cells(7).Controls(1), CheckBox))
                chkmarkimp.Enabled = False
                chkmarkimp.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText

            Next
        End If
    End Sub
    'END: Shilpa N | 03/15/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

    'START: PK | 10/04/2019 | YRS-AT-4597 | YRS enh: State Withholding Project - First Annuity Payments (UI design)
    Private Function getAnnuityAmount() As Double
        Dim totalAnnuityAmount As Double
        If CheckBoxRetPlan.Checked Then
            totalAnnuityAmount = Convert.ToDouble(TextBoxTotalPaymentRet.Text)
        End If

        If CheckBoxSavPlan.Checked Then
            totalAnnuityAmount = totalAnnuityAmount + Convert.ToDouble(TextBoxTotalPaymentSav.Text)
        End If

        If (CheckboxIncludeSSLevelling.Checked = True) And (Convert.ToDouble(TextBoxSSBenefit.Text) > 0) Then
            totalAnnuityAmount = totalAnnuityAmount + Convert.ToDouble(TextBoxSSIncrease.Text)
        End If

        Return totalAnnuityAmount
    End Function
    ' Check if the TaxableAmount is less than TaxWithheld
    Private Function isStateTaxWithheldMoreThanTaxableAmount() As Boolean
        Dim totalAnnuityAmount As Double
        Dim federalWithholdingAmt As Double
        Dim flatAmountStTax As Double
        Dim additionalAmountStTax As Double
        Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
        Try
            If (Not SessionManager.SessionStateWithholding.LstSWHPerssDetail Is Nothing) Then
                If (SessionManager.SessionStateWithholding.LstSWHPerssDetail.Count > 0) Then
                    LstSWHPerssDetail = SessionManager.SessionStateWithholding.LstSWHPerssDetail
                        If (Not LstSWHPerssDetail.FirstOrDefault.numFlatAmount Is Nothing) Or (Not LstSWHPerssDetail.FirstOrDefault.numAdditionalAmount Is Nothing) Then

                            totalAnnuityAmount = getAnnuityAmount()
                            federalWithholdingAmt = GetWithHolding()

                            If (LstSWHPerssDetail.FirstOrDefault.numFlatAmount > 0) Then
                                flatAmountStTax = LstSWHPerssDetail.FirstOrDefault.numFlatAmount
                                If (totalAnnuityAmount < (federalWithholdingAmt + flatAmountStTax)) Then
                                    Return True
                                End If
                            End If

                            If (LstSWHPerssDetail.FirstOrDefault.numAdditionalAmount > 0) Then
                                additionalAmountStTax = LstSWHPerssDetail.FirstOrDefault.numAdditionalAmount
                                If (totalAnnuityAmount < (federalWithholdingAmt + additionalAmountStTax)) Then
                                    Return True
                                End If
                            End If

                        End If
                End If
            End If
            Return False
        Catch
            Throw
        Finally
            totalAnnuityAmount = Nothing
            federalWithholdingAmt = Nothing
            flatAmountStTax = Nothing
            additionalAmountStTax = Nothing
            LstSWHPerssDetail = Nothing
        End Try
    End Function
    'START:  ML |2019.12.18 | YRS-AT-4719 | Validation for MA state
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
                        divErrorMsg.Style("display") = "block"
                        Return False
                    End If
                End If
            End If
        End If
        Return True
    End Function
    'END:  ML |2019.12.18 | YRS-AT-4719 | Validation for MA state
    Private Sub ShowErrorMessage(stMessage As String)
        divErrorMsg.InnerHtml = IIf(String.IsNullOrEmpty(divErrorMsg.InnerHtml.Trim), "", divErrorMsg.InnerHtml + "</br>")
        divErrorMsg.InnerHtml = divErrorMsg.InnerHtml + stMessage
        divErrorMsg.Visible = True
    End Sub
    'END: PK | 10/04/2019 | YRS-AT-4597  | YRS enh: State Withholding Project - First Annuity Payments (UI design)
End Class