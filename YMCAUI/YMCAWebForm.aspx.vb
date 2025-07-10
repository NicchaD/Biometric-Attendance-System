'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	YMCAWebForm.aspx.vb
' Author Name		:	Shefali Bharti
' Employee ID		:	33488
' Email				:	shefali.bharti@3i-infotech.com
' Contact No		:	8740
' Creation Time		:	5/17/2005 3:17:46 PM
' Program Specification Name	: Doc 3.1.3	
' Unit Test Plan Name			:	
' Description					: 
' Changed by        Changed on      Change Description
' Shefali Bharti    24.08.2005      Coding 
' Mohammed Hafiz    24.01.2006      Resolution Tab, Notes Tab, Contact Tab changes/bug solving.
' Mohammed Hafiz    15.02.2006      Adding a delete image in officer tab in the officer grid for each row instead of single delete button for following consistency as in Contact Tab
' Ragesh-34231      02/02/06        Cache to Session  
' Mohammed Hafiz    20-Nov-2006     YREN-2884
'*******************************************************************************
'Changed By:preeti On:9thFeb06 IssueId:YRST-2092/90 Mentioned For person module But found In YMCA. 
'Changed By: Rahul Nasa On: 20th Feb06 IssueId:YRST-2050 When adding resolution error message "Cast from string""to 'date' is not valid.
'Changed By: Rahul Nasa On: 01 Mar 06 IssueId:Missing Update Button.
'Modified By :Shubhrata On: 05 Mar 07 YREN-3128 FedTax Id can be editable only by an anuthorised user and hence to provide a button to enable
'to edit the FedTax Id
'************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'Aparna Samala      21/03/2007      YREN-3115
'Aparna Samala      19/03/2007      YREN-3127
'Ashutosh Patil     22-Jun-2007     New Guid for officer Details. When record is updated newly added Guid will be passed.    
'Aparna Samala      07/09/2007      Show YMCA Name along with YMCA No in header
'Aparna Samala      11/12/2007      Bug tracker 305 - object reference error
'Swopna Valappil    2-Jan-2008      YREN-4016
'Swopna Valappil    2-Jan-2008      YREN-3792
'Swopna Valappil    3-Jan-2008      YREN-4125
'Anil Gupta         01-Feb-2008      BT - 331
'Swopna             07-Apr-2008     To avoid 'specified argument out of range' error 
'Swopna             08-Apr-2008     Phase IV changes
'Swopna             29-Apr-2008     Bug Id 408
'Swopna             30-Apr-2008     Added 'Exit Sub' for code under W/T/M tab.This solves Bug Id-411,413.
'Swopna             30-Apr-2008     BugId-409,410
'Swopna             07-May-2008     BugId-408 Re-Opened
'Swopna             07-May-2008     Validation inserted on re-activation of YMCA;RangeValidator error message set in case of YMCA re-activation.
'Swopna             07-May-2008     BugId-414,412
'Swopna             08-May-2008     Few modifications for Phase IV.Bug Id -431,427
'Swopna             09-May-2008     Bug Id -430
'Swopna             12-May-2008     Reactivation process modifications;Bug Id 424
'Swopna             13-May-2008     Change in YMCA reactivation stored procedure
'Swopna             15-May-2008/16-May-2008     BT-426,BT-430(Re-opened),BT-425,BT-424(Re-opened)
'Swopna             23-May-2008     Messagebox buttons changed from OK to Stop under W/T/M tab only.
'Swopna             26-May-2008     Validation(check presence of any pending transmittal) before Withdrawal/Termination of YMCA.
'Swopna             30-May-2008     1) Validation-Effective date cannot be more than termination date(withdrawal/termination process)
'                                   2) certain modifications
'Swopna             09-June-2008     IssueId-YRS 5.0-435
'Swopna             11/12-June-2008 
'Swopna             19-June-2008    ButtonResoAdd.visible modified.In case resolution not added and save button clicked,W/T/M tab shown with error message.
'Paramesh K.        31-July-2008    Sending Mail alerts when YMCA is merged (YRS 5.0-488)
'Mohammed Hafiz     9-Sep-2008      YRS 5.0-488 revision of message in email.
'Priya              29-Jan-2009 : YRS 5.0-650 :is assn had an active resolution but the Withdrawn YMCA banner is still appearing in the General tab.
'Priya              27-Feb-2009 : YRS 5.0-707 - 2 Issues with termination of ymca's Second issue with termination date validations.
'Priya              02-March-2009 : YRS 5.0-517 - Remove the 6 months before/after validation in the withdrawal process.
'Priya              31-March-2009   YRS 5.0-707 
'Nikunj & Priya     2-April-2009    Cleaning up YMCA Maintenance page
'Nikunj             6-April-2009    Mail Util changes
'NP/PP/SR       2009.05.18      Optimizing the YMCA Screen
'Nikunj Patel   2009.06.02      Changing code to only check the existence of a resolution if we are adding a new YMCA
'Nikunj Patel   2009.06.03      BT-815 - Trimming Address, Phone and Email values in the UI. Converting Label controls to plain HTML text for unreferenced labels
'Sanjay Rawat   2009.06.02      Optimizing the Catch Block
'Dilip Yadav    03-July-2009    To Resolve Issue ID : BT - 839
'Dilip Yadav    08-July-2009    To Resolve Issue ID : BT - 838
'Nikunj Patel   21-Aug-2009     Issue reported by client where IDM link opened a blank window. Email dt. 2009.08.21. BT-916
'Dilip Yadav    18-Sep-2009     Note Tab : Restrict to display max 50 characters only in Notesgrid, while in view notes 
'                               it will show the complete text.As per mail received on 17-Sept-09 from client.
'Dilip Yadav    2009.10.28     To provide the feature of "Priority Handling" as per YRS 5.0.921
'Dilip Yadav    2009.11.09     To hide the prirority label while click on button OK. BT-1012,1013
'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Neeraj Singh                   16/Nov/2009         issue YRS 5.0-940 made changes in ButtonSaveParticipant_Click
'Neeraj Singh   16-dec-2009         'BugID 1044: Neeraj Singh --> throws error on adding new ymca
'Shashi Shekhar   2009.12.23      old call commented and added new call to eliminate the conflict of checksecurity access between server side and client side.
'Imran            2009.01.7      Application displaying both messages.
'Priya          02-March-2010       YRS 5.0-1018 :Security on Notes - Add Item button
'Shashi Shekhar 05-March-2010       YRS- 5.0-942 :Provide look up screen for Officer and Contacts
'Priya          17-March-2010       YRS 5.0-1017:Display withdrawal date as merged date for branches
'Priya          23-March-2010       BT-477: Application displaying "Date cannot be blank" message.
'Priya          23-March-2010       BT-478: Application shows Error page.
'Priya          9-April-2010        YRS 5.0-1050-Error occured when terminating a YMCAs participation
'Shashi Shekhar     2010-04-13       Changes made for Gemini-1051
'Priya          16-April-2010       YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
'Priya          22-April-2010       BT-521 :need to check later Put below if under HelperFunctions.isNonEmpty(g_DataSetYMCAGeneral) if to avoid nullrefference error.
'Priya          05-May-2010         BT-523:YMCA name & number clear in YMCA Information header
'Shashi Shekhar singh   09-June-2010  Changes done for YRS 5.0-1072
'Priya          2010-06-03          Changes made for enhancement in vs-2010 
'Shashi Shekhar  2010-07-19         Integrate Issue fixes and new features which was released in Maintenance VP5 code ( YRS 5.0-1072,YRS 5.0-1080.)
'Priya          23-July-2010        BT-537:While saving terminate date application shows Error page.
'Shashi Shekhar 27 july 2010        BT - 514 : When switching the YMCA'S Officer's & Contact's selection remains in the grid.
'Deven          16-Aug-2010         Added logic for directly showing YMCA detail when user comes
'Shashi Shekhar 24-Jan-2011         For YRS 5.0-1256, BT-716 : Termination date when adding a new resolution
'Priya			27-Jan-2011			BT-719  Previous Withdrawan YMCA details displaying.
'Shashi Shekhar 18 Feb 2011         For BT-671, YRS 5.0-1203 : Make officer or contact fund id a hyperlink
'Shashi Shekhar 14 June 2011        For BT-844, YRS 5.0-1334:Add field for Y-Relations Manager
'Bhavna Shrivastav 2011.08.11       'BS:2011.08.05:YRS 5.0-1380:BT:910 - reopen issue :here we kill all viewstate id that have already used while sorting        
'Prasad Jadhav     2011.08.26       For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
'prasad Jadhav     2011.11.01       For BT-909,YRS 5.0-1379 : New job position field in atsYmcaContacts
'prasad	Jadhav	   2012.01.06		For BT-971- for YRS 5.0-1503 : Replace job position code with job postion description on ymca contacts
'Bhavna Shrivastava2012.07.13       BT:998:YRS 5.0-1544 - enable update to 'Allow YERDI access' at all times
'Anudeep A         2012.09.26       BT:1050 :YRS 5.0-1621: new field and YRS display for when a Y started using Who's Where , Hubtype is enabled only while adding new ymca , 'Add YMCA' button is enabled only in listTab.
'Anudeep A         2013.01.09       Bt:1233:YRS 5.0-1688:YMCA edit security
'Anudeep A         2013.04.12       Bt-1690:YRS 5.0-1874:YMCA Name and Tax Number being erroneously updated
'Anudeep           2013.07.02       Bt-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Sanjay            2013.07.10       BT-2117/YRS 5.0-2152 : The link from the Employment tab to YMCA Maintenance is broken 
'Anudeep A         2013.10.29       BT-2278:YRS 5.0-2236 - After entering a YMCA # and hitting the enter key on the keyboard nothing happens.
'Anudeep A         2013.10.29       BT-2272:Unable to Modify and Add contact information in YMCA maintenance
'Vipul             13Aug14          UniqueSession-forMultiTabs
'Anudeep A         2014.09.26       BT:2440:YRS 5.0-2318 - Primary Active address record being created twice. 
'Manthan Rajguru   2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Gunanithi         2015.12.23       YRS-AT-1687: Remove hub indicator value as a restriction in the assn merge process
'Bala              2016.01.12       YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Manthan Rajguru   2016.02.03       YRS-AT-2334 -  Enhancement to YRS YMCA Maintenance-add a suspend participation option
'Manthan Rajguru   2016.02.03       YRS-AT-1687 -  Remove hub indicator value as a restriction in the assn merge process
'Manthan Rajguru   2016.02.03       YRS-AT-1686 -  Need merge date in the YMCA merger process screen and table 
'Sanjay            2016.02.16       YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Bala              2016.03.08       YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Manthan Rajguru   2016.03.08       YRS-AT-1686 -  Need merge date in the YMCA merger process screen and table 
'Manthan Rajguru   2016.04.05       YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Chandra sekar     2016.07.12       YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
'Chandra sekar     2018.05.23       YRS-AT-3270 - YRS enh-email notifications for updates made Contacts tab (TrackIT 27727)
'Santosh Bura      2018.05.24       YRS-AT-2818 - YRS enh: -in YMCA Maintenance, do not allow more than one LPA contact (TrackIT 25125) 
'Shilpa N          2019.03.20       YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'*********************************************************************************************************************

Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Threading
Imports System.Reflection
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.Security.Permissions
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports YMCARET.YmcaBusinessObject.MetaMessageBO
Imports YMCAObjects.MetaMessageList
'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
Imports Microsoft.Reporting.WebForms
Imports Microsoft.ReportingServices
'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)

Public Class YMCAWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("YMCAWebForm.aspx")
    'End issue id YRS 5.0-940





#Region "Declarations"


    Dim g_integer_count As New Integer
    Dim Page_Mode As String

    Dim g_DataSetYMCAGeneral As DataSet
    Dim g_dataset_dsTelephoneInformation As DataSet
    Dim g_dataset_dsEmailInformation As DataSet
    Dim g_dataset_dsAddressInformation As DataSet
    Dim g_DataTableYMCAGeneral As DataTable
    Dim g_DataSetYMCAOfficer As DataSet
    Dim g_DataTableYMCAOfficer As DataTable
    Dim g_DataSetYMCAContact As DataSet
    Dim g_DataTableYMCAContact As DataTable
    Dim g_DataSetYMCAResolution As DataSet
    Dim g_DataTableYMCAResolution As DataTable
    Dim g_DataSetYMCABranch As DataSet
    Dim g_DataSetYMCABankInfo As DataSet
    Dim g_DataSetYMCANotes As DataSet
    Dim dvYmcaNotes As DataView
    Dim g_bool_flagMetro As Boolean
    Dim g_integer_countContact As Integer
    Dim g_bool_DeleteOfficerFlag As Boolean
    Dim g_bool_UpdateFlagOfficer As Boolean
    Dim g_bool_AddFlagContact As Boolean
    Dim g_bool_UpdateFlagContact As Boolean
    Dim g_bool_UpdateFlagBankInfo As Boolean
    Dim g_bool_DeleteContactFlag As Boolean
    Dim g_bool_AddFlagBankInfo As Boolean
    Dim g_integer_countBankInfo As Integer
    Dim g_integer_countNotes As Integer

    Dim MessageBoxFlag As Boolean

    Protected WithEvents ValidationSummaryYMCA As System.Web.UI.WebControls.ValidationSummary
    Dim ResoFlagNotEntered As Boolean
    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    Dim objRDLCReportDataSource As New ReportDataSource
    Dim objRDLCReportParameter As New ReportParameter()
    Dim dictReportParameters As New Dictionary(Of String, String)
    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
#End Region '"Declarations"

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()


    End Sub

    Protected WithEvents TxtYMCANo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LinkButtonIDM As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TxtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtState As System.Web.UI.WebControls.TextBox
    Protected WithEvents YMCATabStrip As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents YMCAMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents DataGridYMCA As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridYMCAOfficer As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridYMCAContact As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridYMCABranches As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridYMCAResolutions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridYMCABankInfo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridYMCANotesHistory As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonAdress As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonTelephone As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEmail As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents MultiPageYMCA As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents TextBoxYMCANo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxYMCAName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxBoxYMCANo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFedTaxId As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxStateTaxId As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxEnrollmentDate As YMCAUI.DateUserControl
    Protected WithEvents DropDownPaymentMethod As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DropDownBillingMethod As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DropDownHubType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TextBoxMetroName As System.Web.UI.WebControls.TextBox
    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    Protected WithEvents ReportViewerWaivedParticipantList As Microsoft.Reporting.WebForms.ReportViewer
    Protected WithEvents ButtonWaivedProceed As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonWaivedCancel As System.Web.UI.WebControls.Button
    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    'changed from textbox to label for YRS:5.0-1621 on 2012.09.26 by Anudeep  -start
    'Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAdress2 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAdress3 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxGeneralCity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxStateName As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxGeneralState As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxZip As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCountry As System.Web.UI.WebControls.Label
    Protected WithEvents AddressWebUserControlYMCA As AddressUserControlNew
    Protected WithEvents TextBoxTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEmail As System.Web.UI.WebControls.Label
    'changed from textbox to label for YRS:5.0-1621 on 2012.09.26 by Anudeep -End

    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonMetro As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    ''Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    ''Protected WithEvents ButtonOfficersDelete As System.Web.UI.WebControls.Button
    'Rahul
    'changed from Button to imagebutton for YRS:5.0-1621 on 2012.09.26 by Anudeep -start
    Protected WithEvents ButtonOfficersUpdate As System.Web.UI.WebControls.ImageButton
    'changed from Button to imagebutton for YRS:5.0-1621 on 2012.09.26 -End
    'rahul
    Protected WithEvents ButtonOfficersAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonContactAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonContactDelete As System.Web.UI.WebControls.Button
    'Rahul
    Protected WithEvents ButtonContactUpdate As System.Web.UI.WebControls.Button
    'Rahul
    Protected WithEvents ButtonResoAdd As System.Web.UI.WebControls.Button
    'Rahul
    Protected WithEvents ButtonResoUpdate As System.Web.UI.WebControls.Button

    Protected WithEvents ButtonBankInfoUpdate As System.Web.UI.WebControls.Button
    'Rahul
    Protected WithEvents ButtonBankInfoAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonNotesView As System.Web.UI.WebControls.Button

    'Priya March-02-2010 YRS 5.0-1018 :Security on Notes - Add Item button
    'Protected WithEvents ButtonNotesAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonYMCANotesAdd As System.Web.UI.WebControls.Button
    'End YRS 5.0-1018

    Protected WithEvents LabelHubTypeErrMsg As System.Web.UI.WebControls.Label
    Protected WithEvents LabelResolutionErrMsg As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
    Protected WithEvents LabelMetroErrMsg As System.Web.UI.WebControls.Label
    Protected WithEvents LabelYMCANameErrMsg As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAddressErrMsg As System.Web.UI.WebControls.Label
    Protected WithEvents HiddenSecControlName As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HiddenText As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    'Shubhrata Mar 5th 2007,YREN 3128
    Protected WithEvents ButtonEditFedTaxId As System.Web.UI.WebControls.Button

    'Shubhrata

    'Swopna Phase IV changes 8 Apr,2008
    'Protected WithEvents ButtonWithdraw As System.Web.UI.WebControls.Button 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents LabelWithdrawDate As System.Web.UI.WebControls.Label
    'Protected WithEvents PopcalendarWithdrawDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents DateUserControlWithdrawDate As CustomControls.CalenderTextBox 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents LabelPayrollDate_W As System.Web.UI.WebControls.Label 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334
    Protected WithEvents DropdownPayrollDate_W As System.Web.UI.WebControls.DropDownList 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents LabelSummaryReport As System.Web.UI.WebControls.Label
    Protected WithEvents LabelYerdiAccess_W As System.Web.UI.WebControls.Label 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents DropdownYerdiAccess_W As System.Web.UI.WebControls.DropDownList
    Protected WithEvents RangeValidator1 As System.Web.UI.WebControls.RangeValidator

    'Protected WithEvents ButtonTerminate As System.Web.UI.WebControls.Button 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    'Protected WithEvents TextboxTerminationDate As System.Web.UI.WebControls.TextBox
    'Protected WithEvents PopcalendarTerminationDate As RJS.Web.WebControl.PopCalendar
    'Protected WithEvents LabelTerminationDate As System.Web.UI.WebControls.Label 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents LabelPayrollDate_T As System.Web.UI.WebControls.Label 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents DropdownPayrollDate_T As System.Web.UI.WebControls.DropDownList 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents LabelYerdiAccess_T As System.Web.UI.WebControls.Label 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents DropdownYerdiAccess_T As System.Web.UI.WebControls.DropDownList 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents DataGridSummaryReportBeforeTermination As System.Web.UI.WebControls.DataGrid 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents DatagridSummaryReportOnTerminate As System.Web.UI.WebControls.DataGrid 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents LabelEmpRecordTerminate As System.Web.UI.WebControls.Label 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    'Priya 27-Feb-2009 : YRS 5.0-707 - 2 Issues with termination of ymca's Second issue with termination date validations.
    'Protected WithEvents RangeValidator2 As System.Web.UI.WebControls.RangeValidator
    'End Priya 27-Feb-2009
    'Protected WithEvents ButtonMerge As System.Web.UI.WebControls.Button 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code

    'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents LabelSuspensionDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelEmpRecordSuspend As System.Web.UI.WebControls.Label
    Protected WithEvents DateUserControlSuspensionDate As CustomControls.CalenderTextBox
    Protected WithEvents LabelPayrollDate_S As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownPayrollDate_S As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelYerdiAccess_S As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownYerdiAccess_S As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelBeforeSuspend As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAfterSuspend As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridSummaryReportBeforeSuspend As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridSummaryReportOnSuspend As System.Web.UI.WebControls.DataGrid
    'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686

    Protected WithEvents LabelMetroDetails As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxMergeDate As System.Web.UI.WebControls.TextBox 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    'Protected WithEvents PopcalendarMergeDate As RJS.Web.WebControl.PopCalendar
    'Protected WithEvents TextboxMetroDetails As System.Web.UI.WebControls.TextBox 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents LabelMergeDate As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelYMCAName As System.Web.UI.WebControls.Label 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents DropDownYMCANo As System.Web.UI.WebControls.DropDownList

    Protected WithEvents DateUserControlMergeDate As CustomControls.CalenderTextBox 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents LabelPayrolldate_M As System.Web.UI.WebControls.Label 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents DropdownPayrolldate_M As System.Web.UI.WebControls.DropDownList 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents LabelYerdiAccess_M As System.Web.UI.WebControls.Label 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents DropdownYerdiAccess_M As System.Web.UI.WebControls.DropDownList 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code

    'Protected WithEvents ButtonReactivate As System.Web.UI.WebControls.Button 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents LabelReactivate As System.Web.UI.WebControls.Label
    'Protected WithEvents TextboxReactivate As System.Web.UI.WebControls.TextBox 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    'Protected WithEvents PopcalendarReactivate As RJS.Web.WebControl.PopCalendar 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Protected WithEvents DateUserControlReactivateDate As CustomControls.CalenderTextBox 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code

    Protected WithEvents LabelCurrentStatus As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridSummaryReport As System.Web.UI.WebControls.DataGrid
    'Swopna 20 June08
    Protected WithEvents DatagridSummaryReportOnWithdrawal As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelEmpRecord As System.Web.UI.WebControls.Label
    Protected WithEvents LabelBeforeWithdrawal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAfterWithdrawal As System.Web.UI.WebControls.Label
    'Swopna 20 June08
    'Start : Dilip : 03-July-09
    'Protected WithEvents DropdownWithdrawDate As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DropdownlistTermination As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DropdownlistMergeDate As System.Web.UI.WebControls.DropDownList
    'End : Dilip : 03-July-09 
    Protected WithEvents LabelResoNotEntered As System.Web.UI.WebControls.Label
    Protected WithEvents checkboxPriority As System.Web.UI.WebControls.CheckBox 'Added by Dilip yadav : YRS 5.0.921
    Protected WithEvents LabelPriority As System.Web.UI.WebControls.Label 'Added by Dilip yadav : YRS 5.0.921
    Protected WithEvents LabelPriorityHdr As System.Web.UI.WebControls.Label 'Added by Dilip yadav : YRS 5.0.921

    'Shashi Shekhar:For BT-844, YRS 5.0-1334:Add field for Y-Relations Manager
    Protected WithEvents txtRelationManager As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnAddRelationManager As System.Web.UI.WebControls.Button
    Protected WithEvents gvRelationManagers As System.Web.UI.WebControls.GridView
    Protected WithEvents hdrelationManagerName As System.Web.UI.WebControls.HiddenField
    Protected WithEvents lblRelationManagerName As System.Web.UI.WebControls.Label
    Protected intRelationManagerselectionLimit As Integer = 2
    'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
    Protected WithEvents HiddenFieldDirty As System.Web.UI.WebControls.HiddenField
    'Added by Anudeep for YRS:5.0-1621 on 2012.09.26
    Protected WithEvents lblEligibilityTrackerEnrollmentDate As System.Web.UI.WebControls.Label
    Protected WithEvents YMCA_Toolbar_WebUserControl1 As YMCAUI.YMCA_Toolbar_WebUserControl
    'Start: Bala: 19/01/2019: YRS-AT-2398: Adding Controls
    Protected WithEvents checkboxEligibleTrackingUser As System.Web.UI.WebControls.CheckBox
    Protected WithEvents checkboxYNAN As System.Web.UI.WebControls.CheckBox
    Protected WithEvents checkboxMidMajors As System.Web.UI.WebControls.CheckBox
    'End: Bala: 19/01/2019: YRS-AT-2398: Adding Controls

    'Start -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents tab_btnWithdrawClick As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents tab_btnTerminateClick As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents tab_btnSuspendClick As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents tab_btnMergeClick As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents tab_btnReactivateClick As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents hdnSelectedTab As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hdnOperationPerformed As System.Web.UI.WebControls.HiddenField
    'End -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686

    'Start -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686
    Protected WithEvents txtDateWithdraw As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateSuspend As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateMerge As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateReactivate As System.Web.UI.WebControls.TextBox
    'End -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686

    'Swopna Phase IV changes 8 Apr,2008

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

    Public Sub New()

    End Sub
#End Region

#Region "Properties"

    '1. Define Property 
    Public Property Session(sname As String) As Object
        Get
            Return MyBase.Session(Me.uniqueSessionId + sname)
        End Get
        Set(value As Object)
            MyBase.Session(Me.uniqueSessionId + sname) = value
        End Set
    End Property

    ' 2. Vipul 13Aug14 UniqueSession-forMultiTabs

    Public Property uniqueSessionId As String
        Get
            If ViewState("Sessionname") = Nothing Then
                Dim genrandom As Random = New Random()
                ViewState("Sessionname") = genrandom.Next
            End If
            Return ViewState("Sessionname").ToString()
        End Get
        Set(ByVal Value As String)
            ViewState("Sessionname") = Value
        End Set
    End Property     'Vipul 13Aug14 UniqueSession-forMultiTabs  /e

    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    Public Property ShowWaivedParticipantsList As Boolean
        Get
            If Not (ViewState("ShowWaivedParticipantsList")) Is Nothing Then
                Return (DirectCast(ViewState("ShowWaivedParticipantsList"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("ShowWaivedParticipantsList") = Value
        End Set
    End Property

    Public Property IsProceedWaived As Boolean
        Get
            If Not (ViewState("IsProceedWaived")) Is Nothing Then
                Return (DirectCast(ViewState("IsProceedWaived"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsProceedWaived") = Value
        End Set
    End Property
    Public Property WarningMessage As String
        Get
            If Not (ViewState("strWarningMessage")) Is Nothing Then
                Return (DirectCast(ViewState("strWarningMessage"), String))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("strWarningMessage") = Value
        End Set
    End Property
    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)

    Private Property NotesGroupUser() As Boolean
        Get
            If Not (Session("NotesGroupUser")) Is Nothing Then
                Return (DirectCast(Session("NotesGroupUser"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("NotesGroupUser") = Value
        End Set
    End Property

    Private Property YMCASearchList() As DataSet
        Get
            'If IsNothing(Session("YMCA List")) Then
            If HelperFunctions.isEmpty(Session("YMCA List")) Then
                Dim l_dataset_dsYMCAList As DataSet = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAList(Me.TextBoxYMCANo.Text, Me.TextBoxName.Text, Me.TextBoxCity.Text, Me.TextBoxState.Text)
                Session("YMCA List") = l_dataset_dsYMCAList
            End If
            Return DirectCast(Session("YMCA List"), DataSet)
        End Get
        Set(ByVal Value As DataSet)
            Session("YMCA List") = Value
        End Set
    End Property
#End Region

    Private Sub AddResolution(ByVal objRess As Resolution)
        'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
        Dim intOldParticipantResolution As Integer
        Dim intNewParticipantResolution As Integer
        Me.ShowWaivedParticipantsList = False
        'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)

        g_DataSetYMCAResolution = Session("YMCA Resolution")
        If (g_DataSetYMCAResolution.Tables(0).Rows.Count > 0) Then
            For Each dr As DataRow In g_DataSetYMCAResolution.Tables(0).Rows
                If (dr.IsNull("Term. Date")) Then
                    '----------------------------------------------------------------------------------------------------
                    'Shashi Shekhar: 24-01-2011: For YRS 5.0-1256, BT-716 : Termination date when adding a new resolution
                    'dr("Term. Date") = objRess.EffectiveDatec
                    dr("Term. Date") = objRess.EffectiveDate.AddDays(-1)
                    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
                    'If Participant Resolution is changed from Shared Resolution to Fully Resolution of YMCA then 
                    'List of Waived Participants will be display as Popup window
                    intOldParticipantResolution = Convert.ToInt32(dr("Part.%"))
                    intNewParticipantResolution = objRess.ParticipantPerc.Trim
                    If intOldParticipantResolution > 0 And intNewParticipantResolution = 0 Then
                        Me.ShowWaivedParticipantsList = True
                    End If
                    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
                    '-----------------------------------------------------------------------------------------------------
                End If
            Next
        End If

        Dim InsertRowResolution As DataRow
        InsertRowResolution = g_DataSetYMCAResolution.Tables(0).NewRow
        ' Assign the values.
        InsertRowResolution.Item("guiYmcaID") = Session("GuiUniqueId")
        InsertRowResolution.Item("Eff. Date") = objRess.EffectiveDate
        If objRess.TermDate.ToString = String.Empty Then
            InsertRowResolution.Item("Term. Date") = System.DBNull.Value
        Else
            InsertRowResolution.Item("Term. Date") = objRess.TermDate
        End If

        InsertRowResolution.Item("Vesting Type") = objRess.VestingType.Trim
        InsertRowResolution.Item("Vesting Desc") = objRess.VestingTypeDesc.Trim
        InsertRowResolution.Item("Resolution Type") = objRess.ResolutionType.Trim
        InsertRowResolution.Item("Resolution Desc") = objRess.ResolutionTypeDesc.Trim
        InsertRowResolution.Item("Part.%") = objRess.ParticipantPerc.Trim
        InsertRowResolution.Item("YMCA%") = objRess.YMCAPerc.Trim

        If InsertRowResolution.IsNull("S.Scale%") Then
            InsertRowResolution.Item("S.Scale%") = 0
        Else
            InsertRowResolution.Item("S.Scale%") = objRess.SlidingScalePerc.Trim
        End If
        If InsertRowResolution.IsNull("Add'l YMCA%") Then
            InsertRowResolution.Item("Add'l YMCA%") = 0
        Else
            InsertRowResolution.Item("Add'l YMCA%") = objRess.AddlYMCAPerc.Trim
        End If

        ' Insert the row into Table.
        g_DataSetYMCAResolution.Tables(0).Rows.Add(InsertRowResolution)
        Session("YMCA Resolution") = g_DataSetYMCAResolution
        'bind grid
        PopulateResolution()
    End Sub
    Private Sub AddContacts(ByVal objContacts As Contacts)
        Dim l_DataRowContact As DataRow
        g_DataSetYMCAContact = Session("YMCA Contact")
        If Not IsNothing(g_DataSetYMCAContact) Then

            l_DataRowContact = GetRowForUpdation(g_DataSetYMCAContact.Tables(0), "guiUniqueId", objContacts.ContactId)
            If IsNothing(l_DataRowContact) Then
                l_DataRowContact = g_DataSetYMCAContact.Tables(0).NewRow
                l_DataRowContact.Item("guiUniqueId") = Guid.NewGuid() 'by Aparna 31/08/2007
                l_DataRowContact.Item("guiYmcaID") = Session("GuiUniqueId")
                ' Insert the row into Table.
                g_DataSetYMCAContact.Tables(0).Rows.Add(l_DataRowContact)
            End If
            ' Assign the values.
            'changed by prasad
            ''l_DataRowContact.Item("Type") = objContacts.Type
            l_DataRowContact.Item("Type") = objContacts.TypeValue

            l_DataRowContact.Item("Contact Name") = objContacts.Name
            l_DataRowContact.Item("Phone No") = objContacts.Telephone

            '-------------------Start: Shashi Shekhar:2010-03-12:For YRS-5.0-942-------------------

            If (objContacts.ExtNo = String.Empty Or objContacts.ExtNo = Nothing) Then
                l_DataRowContact.Item("Extn No") = System.DBNull.Value
            Else
                l_DataRowContact.Item("Extn No") = objContacts.ExtNo 'CType(Session("OfficerExtnNo"), String)
            End If

            'l_DataRowContact.Item("Extn No") = objContacts.ExtNo
            l_DataRowContact.Item("Effective Date") = objContacts.EffectiveDate
            l_DataRowContact.Item("Email") = objContacts.Email
            'added by Hafiz on 20-Nov-2006 for YREN-2884
            l_DataRowContact.Item("ContactNotes") = objContacts.Note


            '-------------------Start: Shashi Shekhar:2010-03-08:For YRS-5.0-942-------------------

            If (objContacts.FundNo = String.Empty Or objContacts.FundNo = Nothing) Then
                l_DataRowContact.Item("Fund No") = System.DBNull.Value
            Else
                l_DataRowContact.Item("Fund No") = objContacts.FundNo
            End If

            l_DataRowContact.Item("First Name") = objContacts.FirstName
            l_DataRowContact.Item("Middle Name") = objContacts.MiddleName
            l_DataRowContact.Item("Last Name") = objContacts.LastName

            '-------------------End: Shashi Shekhar:2010-03-08:For YRS-5.0-942-------------------
            'Added by prasad for YRS 5.0-1379 : New job position field in atsYmcaContacts
            l_DataRowContact.Item("Title") = objContacts.Title
            l_DataRowContact.Item("TypeCode") = objContacts.Type
            'Added by prasad for YRS 5.0-1503 : Replace job position code with job postion description on ymca contacts
            l_DataRowContact.Item("TitleDescription") = objContacts.TitleDescription
            Session("YMCA Contact") = g_DataSetYMCAContact


            PopulateContact()
        End If
    End Sub
    Private Sub AddOfficer(ByVal objOfficer As Officer)
        Dim l_DataRowOfficer As DataRow

        g_DataSetYMCAOfficer = CType(Session("YMCA Officer"), DataSet)
        If Not IsNothing(g_DataSetYMCAOfficer) Then

            l_DataRowOfficer = GetRowForUpdation(g_DataSetYMCAOfficer.Tables(0), "guiUniqueId", objOfficer.OfficerId)

            If IsNothing(l_DataRowOfficer) Then
                l_DataRowOfficer = g_DataSetYMCAOfficer.Tables(0).NewRow
                'New Guid for Update Record functionality.
                l_DataRowOfficer.Item("guiUniqueID") = Guid.NewGuid()
                l_DataRowOfficer.Item("guiYmcaID") = Session("GuiUniqueId")
                'Insert the row into Table.
                g_DataSetYMCAOfficer.Tables(0).Rows.Add(l_DataRowOfficer)
            End If
            'Assign the values.
            l_DataRowOfficer.Item("Name") = objOfficer.Name 'CType(Session("OfficerName"), String)

            'Code change Shashi Shekhar:2010-03-05:For YRS-5.0-942
            l_DataRowOfficer.Item("Title") = objOfficer.Title 'CType(Session("TitleType"), String)
            l_DataRowOfficer.Item("chvPositionTitleCode") = objOfficer.TitleType    'CType(Session("TitleType"), String)

            l_DataRowOfficer.Item("Phone No") = objOfficer.Telephone 'CType(Session("OfficerTelephone"), String)

            If (objOfficer.ExtnNo = String.Empty Or objOfficer.ExtnNo = Nothing) Then
                l_DataRowOfficer.Item("Extn No") = System.DBNull.Value
            Else
                l_DataRowOfficer.Item("Extn No") = objOfficer.ExtnNo 'CType(Session("OfficerExtnNo"), String)
            End If

            ' l_DataRowOfficer.Item("Extn No") = objOfficer.ExtnNo 'CType(Session("OfficerExtnNo"), String)
            '' l_DataRowOfficer.Item("Extn No") = objOfficer.ExtnNo 'CType(Session("OfficerExtnNo"), String)
            l_DataRowOfficer.Item("Email") = objOfficer.Email 'CType(Session("OfficerEmail"), String)
            l_DataRowOfficer.Item("Effective Date") = CType(objOfficer.EffectiveDate, Date).ToShortDateString() 'CType(Session("OfficerEffDate"), Date).ToShortDateString()

            '-------------------Start: Shashi Shekhar:2010-03-05:For YRS-5.0-942-------------------

            If (objOfficer.FundNo = String.Empty Or objOfficer.FundNo = Nothing) Then
                l_DataRowOfficer.Item("Fund No") = System.DBNull.Value
            Else
                l_DataRowOfficer.Item("Fund No") = objOfficer.FundNo
            End If

            l_DataRowOfficer.Item("First Name") = objOfficer.FirstName
            l_DataRowOfficer.Item("Middle Name") = objOfficer.MiddleName
            l_DataRowOfficer.Item("Last Name") = objOfficer.LastName

            '-------------------End: Shashi Shekhar:2010-03-05:For YRS-5.0-942-------------------

            Session("YMCA Officer") = g_DataSetYMCAOfficer
            PopulateOfficer()
        End If

    End Sub
    Private Sub AddBank(ByVal objBank As Bank)
        Dim l_DataRowBankInfo As DataRow

        g_DataSetYMCABankInfo = DirectCast(Session("YMCA BankInfo"), DataSet)
        If Not g_DataSetYMCABankInfo Is Nothing Then


            l_DataRowBankInfo = GetRowForUpdation(g_DataSetYMCABankInfo.Tables(0), "guiUniqueId", objBank.Id)
            'g_DataSetYMCABankInfo.Tables(0).Rows.Item(objBank.BankId)

            If IsNothing(l_DataRowBankInfo) Then       'If IsNothing(l_DataRowBankInfo) Then
                l_DataRowBankInfo = g_DataSetYMCABankInfo.Tables(0).NewRow
                l_DataRowBankInfo.Item("guiYmcaID") = Session("GuiUniqueId")
                l_DataRowBankInfo.Item("guiUniqueId") = Guid.NewGuid()
                ' Insert the row into Table.
                g_DataSetYMCABankInfo.Tables(0).Rows.Add(l_DataRowBankInfo)

            End If
            ' Assign the values.
            l_DataRowBankInfo.Item("Account #") = objBank.AccountNumber 'DirectCast(Session("BankAccountNumber"), String)
            l_DataRowBankInfo.Item("Name") = objBank.Name 'DirectCast(Session("SelectBank_BankName"), String)
            l_DataRowBankInfo.Item("Bank ABA#") = objBank.ABANumber 'DirectCast(Session("SelectBank_BankABANumber"), String)
            l_DataRowBankInfo.Item("Payment Method") = objBank.PaymentMethod 'DirectCast(Session("BankPaymentMethod"), String)
            l_DataRowBankInfo.Item("Account Type") = objBank.AccountType 'DirectCast(Session("BankAccountType"), String)
            l_DataRowBankInfo.Item("Effective Date") = objBank.EffectiveDate 'DirectCast(Session("BankEffectiveDate"), Date).ToShortDateString()
            l_DataRowBankInfo.Item("guiBankId") = objBank.BankId
        End If
        Session("YMCA BankInfo") = g_DataSetYMCABankInfo
        PopulateBank()

    End Sub
    Private Sub AddNote(ByVal objNotes As Notes)
        Dim InsertRowNotes As DataRow
        g_DataSetYMCANotes = Session("YMCA Notes")
        If Not IsNothing(g_DataSetYMCANotes) Then

            InsertRowNotes = g_DataSetYMCANotes.Tables(0).NewRow
            ' Assign the values.
            InsertRowNotes.Item("guiUniqueID") = Guid.NewGuid()
            InsertRowNotes.Item("guiEntityID") = Session("GuiUniqueId")
            InsertRowNotes.Item("First Line of Notes") = objNotes.NotesText 'DirectCast(Session("NotesText"), String)
            InsertRowNotes.Item("Date") = Date.Now.ToShortDateString()
            InsertRowNotes.Item("Creator") = MyBase.Session("LoginId")
            InsertRowNotes.Item("bitImportant") = objNotes.BitImportant ' Session("BitImportant")

            g_DataSetYMCANotes.Tables(0).Rows.Add(InsertRowNotes)
        End If

        Session("YMCA Notes") = g_DataSetYMCANotes
        PopulateNote()
    End Sub
    Private Sub AddMetro(ByVal objMetro As Metro)
        Dim dsYMCAGeneral As DataSet
        Dim drYMCA As DataRow
        dsYMCAGeneral = Session("YMCA General")
        If dsYMCAGeneral Is Nothing Then Exit Sub
        If HelperFunctions.isEmpty(dsYMCAGeneral) Then Exit Sub
        drYMCA = dsYMCAGeneral.Tables(0).Rows(0)
        If drYMCA("MetroGuiUniqueId").ToString().ToLower() <> objMetro.MetroGuiUniqueiD.ToLower() Then
            Me.TextBoxMetroName.Text = objMetro.YmcaMetroName
            drYMCA("MetroGuiUniqueId") = objMetro.MetroGuiUniqueiD
            drYMCA("chvYmcaMetroName") = TextBoxMetroName.Text.Trim
        End If
    End Sub
    Private Sub HandlePopupActions(ByVal objPopUpResult As PopupResult)
        'EnableSaveCancelButtons()
        Select Case objPopUpResult.Page
            Case "RESOLUTION"
                If objPopUpResult.Action = PopupResult.ActionTypes.ADD Then
                    AddResolution(DirectCast(objPopUpResult.State, Resolution))
                End If
            Case "ADDRESS"
                If objPopUpResult.Action = PopupResult.ActionTypes.ADD Then
                    PopulateAddress()
                End If
            Case "METRO"
                If objPopUpResult.Action = PopupResult.ActionTypes.ADD Then
                    AddMetro(DirectCast(objPopUpResult.State, Metro))
                End If
            Case "TELEPHONE"
                If objPopUpResult.Action = PopupResult.ActionTypes.ADD Then
                    PopulateTelephone()
                End If
            Case "EMAIL"
                If objPopUpResult.Action = PopupResult.ActionTypes.ADD Then
                    PopulateEmail()
                End If
            Case "OFFICER"
                If objPopUpResult.Action = PopupResult.ActionTypes.ADD Then
                    AddOfficer(DirectCast(objPopUpResult.State, Officer))
                End If
            Case "CONTACTS"
                If objPopUpResult.Action = PopupResult.ActionTypes.ADD Then
                    AddContacts(DirectCast(objPopUpResult.State, Contacts))
                End If
            Case "BANK"
                If objPopUpResult.Action = PopupResult.ActionTypes.ADD Then
                    AddBank(DirectCast(objPopUpResult.State, Bank))
                End If
            Case "NOTES"
                If objPopUpResult.Action = PopupResult.ActionTypes.ADD Then
                    AddNote(DirectCast(objPopUpResult.State, Notes))
                End If
            Case Else
        End Select
    End Sub

#Region "Load Tabs"
    Public Sub LoadData(ByVal GuiUniqueId As String)
        'General
        g_DataSetYMCAGeneral = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAGeneral(GuiUniqueId)
        Session("YMCA General") = g_DataSetYMCAGeneral
        'Telephone of general tab
        g_dataset_dsTelephoneInformation = YMCARET.YmcaBusinessObject.YMCATelephoneBOClass.SearchTelephoneInformation(GuiUniqueId)
        Session("Telephone Information") = g_dataset_dsTelephoneInformation
        'Address Information
        'g_dataset_dsAddressInformation = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.SearchAddressInformation(GuiUniqueId)
        g_dataset_dsAddressInformation = Address.GetAddressForYMCA(GuiUniqueId)
        Session("Address Information") = g_dataset_dsAddressInformation
        'Email of general tab
        g_dataset_dsEmailInformation = YMCARET.YmcaBusinessObject.YMCANetBOClass.SearchEmailInformation(GuiUniqueId)
        Session("Email Information") = g_dataset_dsEmailInformation
        'Officer tab
        g_DataSetYMCAOfficer = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAOfficer(GuiUniqueId)
        Session("YMCA Officer") = g_DataSetYMCAOfficer
        'Contact Tab
        g_DataSetYMCAContact = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAContact(GuiUniqueId)
        Session("YMCA Contact") = g_DataSetYMCAContact
        'Branch
        g_DataSetYMCABranch = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCABranch(GuiUniqueId)
        Session("YMCA Branch") = g_DataSetYMCABranch
        'Resolution's Tab
        g_DataSetYMCAResolution = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAResolution(GuiUniqueId)
        Session("YMCA Resolution") = g_DataSetYMCAResolution
        'Bank Tab
        g_DataSetYMCABankInfo = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCABankInfo(GuiUniqueId)
        Session("YMCA BankInfo") = g_DataSetYMCABankInfo
        'Notes
        g_DataSetYMCANotes = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCANote(GuiUniqueId)
        Session("YMCA Notes") = g_DataSetYMCANotes
    End Sub
#End Region

#Region "Populate Controls"
    Private Sub PopulateAllTabs()
        PopulateGeneralTab()
        PopulateAddress()
        PopulateTelephone()
        'PopulateLabelCurrentStatus() 'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 | Commented old code
        PopulateWTSMTab()
        PopulateEmail()
        PopulateOfficer()
        PopulateContact()
        PoplulateBranch()
        PopulateResolution()
        PopulateBank()
        PopulateNote()
    End Sub
    Private Sub PopulateGeneralTab()

        Dim l_DataRowYMCAGeneral As DataRow
        g_DataSetYMCAGeneral = DirectCast(Session("YMCA General"), DataSet)
        'Shashi Shekhar:  For YRS 5.0-1334
        txtRelationManager.Text = String.Empty
        Me.hdrelationManagerName.Value = String.Empty
        TextBoxYMCAName.Text = String.Empty
        TextBoxBoxYMCANo.Text = String.Empty
        TextBoxFedTaxId.Text = String.Empty
        TextBoxStateTaxId.Text = String.Empty
        TextBoxEnrollmentDate.Text = String.Empty
        DropDownPaymentMethod.SelectedValue = ""
        DropDownBillingMethod.SelectedValue = ""
        DropDownHubType.SelectedValue = ""
        TextBoxMetroName.Text = String.Empty
        TextBoxMetroName.Enabled = False
        ButtonMetro.Enabled = False
        YMCATabStrip.Items(4).Enabled = False
        'Added for YRS:5.0-1621 on 2012.09.26 
        DropDownHubType.Enabled = True
        If HelperFunctions.isNonEmpty(g_DataSetYMCAGeneral) Then
            l_DataRowYMCAGeneral = g_DataSetYMCAGeneral.Tables("YMCA General").Rows.Item(0)
            'if gone to any of the popup then the textboxes and dropdown control values r retrieved from session variables else from the dataset.
            Me.TextBoxYMCAName.Text = DirectCast(l_DataRowYMCAGeneral("chvymcaname"), String).Trim
            If Not l_DataRowYMCAGeneral.IsNull("chrymcano") Then
                Me.TextBoxBoxYMCANo.Text = DirectCast(l_DataRowYMCAGeneral("chrymcano"), String).Trim
            End If

            'START : Added by Dilip yadav : YRS 5.0.921
            If l_DataRowYMCAGeneral.IsNull("Priority") Then
                Me.checkboxPriority.Checked = False
            Else
                Me.checkboxPriority.Checked = Convert.ToBoolean(l_DataRowYMCAGeneral("Priority"))
            End If
            'END : Added by Dilip yadav : YRS 5.0.921

            'Start: Bala: 01/19/2019: YRS-AT-2398: Enabling true or false for checkbox ExhaustedDBSettle
            If l_DataRowYMCAGeneral.IsNull("EligibleTrackingUser") Then
                Me.checkboxEligibleTrackingUser.Checked = False
            Else
                Me.checkboxEligibleTrackingUser.Checked = Convert.ToBoolean(l_DataRowYMCAGeneral("EligibleTrackingUser"))
            End If
            If l_DataRowYMCAGeneral.IsNull("YNAN") Then
                Me.checkboxYNAN.Checked = False
            Else
                Me.checkboxYNAN.Checked = Convert.ToBoolean(l_DataRowYMCAGeneral("YNAN"))
            End If
            If l_DataRowYMCAGeneral.IsNull("MidMajors") Then
                Me.checkboxMidMajors.Checked = False
            Else
                Me.checkboxMidMajors.Checked = Convert.ToBoolean(l_DataRowYMCAGeneral("MidMajors"))
            End If
            'End: Bala: 01/19/2019: YRS-AT-2398: Enabling true or false for checkbox ExhaustedDBSettle
            If Not l_DataRowYMCAGeneral.IsNull("chrtaxnumberfederal") Then
                Me.TextBoxFedTaxId.Text = DirectCast(l_DataRowYMCAGeneral("chrtaxnumberfederal"), String).Trim
            End If
            If Not l_DataRowYMCAGeneral.IsNull("chrtaxnumberstate") Then
                Me.TextBoxStateTaxId.Text = DirectCast(l_DataRowYMCAGeneral("chrtaxnumberstate"), String).Trim
            End If
            If Not l_DataRowYMCAGeneral.IsNull("dtsEntryDate") Then
                Me.TextBoxEnrollmentDate.Text = CType(l_DataRowYMCAGeneral("dtsEntryDate"), String).Trim
            End If
            'Added for YRS:5.0-1621 on 2012.09.26 by Anudeep -start
            If Not l_DataRowYMCAGeneral.IsNull("dtsEligTrackEnrollDate") Then
                Me.lblEligibilityTrackerEnrollmentDate.Text = CType(l_DataRowYMCAGeneral("dtsEligTrackEnrollDate"), String).Trim
            End If
            'Added for YRS:5.0-1621 on 2012.09.26 by Anudeep -End
            If Not l_DataRowYMCAGeneral.IsNull("chvDefaultPaymentCode") Then
                Me.DropDownPaymentMethod.SelectedValue = DirectCast(l_DataRowYMCAGeneral("chvDefaultPaymentCode"), String).Trim
            End If
            If Not l_DataRowYMCAGeneral.IsNull("chvBillingMethodCode") Then
                Me.DropDownBillingMethod.SelectedValue = DirectCast(l_DataRowYMCAGeneral("chvBillingMethodCode"), String).Trim.ToUpper
            End If
            If Not l_DataRowYMCAGeneral.IsNull("Chrhubind") Then
                Me.DropDownHubType.SelectedValue = DirectCast(l_DataRowYMCAGeneral("Chrhubind"), String).Trim
                'Added for YRS:5.0-1621 on 2012.09.26
                DropDownHubType.Enabled = False
            End If
            'By Aparna -YREN-2854 -22/01/2007
            If Me.DropDownHubType.SelectedValue = "B" Then
                Me.TextBoxMetroName.Enabled = True
                Me.ButtonMetro.Enabled = True
                If Not l_DataRowYMCAGeneral.IsNull("chvYmcaMetroName") Then TextBoxMetroName.Text = l_DataRowYMCAGeneral("chvYmcaMetroName").ToString().Trim
            End If
            If Me.DropDownHubType.SelectedValue = "M" Then
                Me.YMCATabStrip.Items(4).Enabled = True
            End If

            'Shashi Shekhar:17-June-2011: for YRS 5.0-1334:Add field for Y-Relations Manager
            If Not l_DataRowYMCAGeneral.IsNull("RelationManager") Then
                Me.txtRelationManager.Text = l_DataRowYMCAGeneral("RelationManager").ToString.Trim
                Me.hdrelationManagerName.Value = l_DataRowYMCAGeneral("RelationManager").ToString.Trim
            End If

        End If
    End Sub
    Private Sub PopulateAddress()
        Dim l_dataset_States As DataSet
        Dim l_datatable_States As DataTable
        Dim l_datarow_States As DataRow

        Dim l_ds_AddressInformation As DataSet = DirectCast(Session("Address Information"), DataSet)
        Dim l_dr_Address As DataRow()
        Dim dr As DataRow

        'TextBoxAddress1.Text = String.Empty
        'TextBoxAdress2.Text = String.Empty
        'TextBoxAdress3.Text = String.Empty
        'TextBoxGeneralCity.Text = String.Empty
        'TextBoxGeneralState.Text = String.Empty
        'TextBoxZip.Text = String.Empty
        'TextBoxCountry.Text = String.Empty
        'TextBoxStateName.Text = String.Empty
        AddressWebUserControlYMCA.LoadAddressDetail(Nothing)

        If HelperFunctions.isNonEmpty(l_ds_AddressInformation) Then
            l_dr_Address = l_ds_AddressInformation.Tables(0).Select("isPrimary = 1 AND isActive = 1")
            If l_dr_Address.Length = 0 Then
                dr = l_ds_AddressInformation.Tables(0).Rows(0)
            Else
                dr = l_dr_Address(0)
            End If
            'If Not dr.IsNull("Address") Then TextBoxAddress1.Text = DirectCast(dr("Address"), String).Trim
            'If Not dr.IsNull("Address 2") Then TextBoxAdress2.Text = DirectCast(dr("Address 2"), String).Trim
            'If Not dr.IsNull("Address 3") Then TextBoxAdress3.Text = DirectCast(dr("Address 3"), String).Trim
            'If Not dr.IsNull("City") Then TextBoxGeneralCity.Text = DirectCast(dr("City"), String).Trim
            'If Not dr.IsNull("State") Then TextBoxGeneralState.Text = DirectCast(dr("State"), String).Trim
            'If Not dr.IsNull("Zip") Then TextBoxZip.Text = DirectCast(dr("Zip"), String).Trim
            'If Not dr.IsNull("Country") Then TextBoxCountry.Text = DirectCast(dr("Country"), String).Trim
            AddressWebUserControlYMCA.LoadAddressDetail(l_dr_Address)
            'TextBoxStateName.Text = dr("State")
            'l_dataset_States = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()
            'l_dataset_States = Address.GetState()
            'If Not l_dataset_States Is Nothing > 0 Then
            '    l_datatable_States = l_dataset_States.Tables(0)
            '    If HelperFunctions.isNonEmpty(l_datatable_States) Then
            '        Me.TextBoxStateName.Text = String.Empty
            '        For Each l_datarow_States In l_datatable_States.Rows
            '            'If l_datarow_States.IsNull("chvCodeValue") = False AndAlso _
            '            '        dr.IsNull("State") AndAlso _
            '            If l_datarow_States("chvCodeValue") = dr("State") AndAlso _
            '                    l_datarow_States("chvCodeValue").ToString <> "" Then
            '                Me.TextBoxStateName.Text = l_datarow_States("chvDescription")
            '            End If
            '        Next
            '    End If
            'End If
        End If
    End Sub
    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    'Private Sub PopulateLabelCurrentStatus()
    '    LabelSummaryReport.Visible = False
    '    LabelBeforeWithdrawal.Visible = False
    '    LabelAfterWithdrawal.Visible = False

    '    Dim l_ds_DataSetStatus As DataSet
    '    l_ds_DataSetStatus = YMCARET.YmcaBusinessObject.YMCABOClass.GetStatus(DirectCast(Session("GuiUniqueId"), String))
    '    If HelperFunctions.isEmpty(l_ds_DataSetStatus) Then Exit Sub
    '    If HelperFunctions.isEmpty(l_ds_DataSetStatus.Tables("AtsYmcaEntrants")) Then Exit Sub
    '    If HelperFunctions.isEmpty(l_ds_DataSetStatus.Tables("atsYmcas")) Then Exit Sub


    '    Dim l_int_AllowYerdiAccess As Integer
    '    If (ButtonSave.Enabled = False) Then
    '        If CType(l_ds_DataSetStatus.Tables("AtsYmcaEntrants").Rows(0).Item("bitAllowYerdiAccess"), Boolean) = True Then
    '            l_int_AllowYerdiAccess = 1
    '        ElseIf CType(l_ds_DataSetStatus.Tables("AtsYmcaEntrants").Rows(0).Item("bitAllowYerdiAccess"), Boolean) = False Then
    '            l_int_AllowYerdiAccess = 0
    '        End If
    '    Else

    '        'If DropdownYerdiAccess_T.SelectedValue = "" Then
    '        '    If CType(l_ds_DataSetStatus.Tables("AtsYmcaEntrants").Rows(0).Item("bitAllowYerdiAccess"), Boolean) = True Then
    '        '        l_int_AllowYerdiAccess = 1
    '        '    ElseIf CType(l_ds_DataSetStatus.Tables("AtsYmcaEntrants").Rows(0).Item("bitAllowYerdiAccess"), Boolean) = False Then
    '        '        l_int_AllowYerdiAccess = 0
    '        '    End If
    '        'Else
    '        '    l_int_AllowYerdiAccess = DropdownYerdiAccess_T.SelectedValue
    '        'End If      
    '    End If

    '    'initialize all textboxes to default values
    '    LabelCurrentStatus.Text = ""

    '    'Start : Commented & Added by Dilip : 09-July-2009 : BT - 238
    '    'DropdownWithdrawDate.SelectedValue = String.Empty 
    '    'END : Commented & Added by Dilip : 09-July-2009 : BT - 238
    '    'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
    '    'DropdownYerdiAccess_W.SelectedValue = ""
    '    'DropdownYerdiAccess_T.SelectedValue = 0 
    '    'End YRS 5.0-873
    '    'ButtonWithdraw.Enabled = True
    '    'Priya 23-July-2010  BT-537:While saving terminate date application shows Error page.
    '    'Added if condition If session for terminate ymca is not true or nothing then only set selscted value 
    '    If Session("TerminateYMCA") = Nothing AndAlso Session("TerminateYMCA") = False Then
    '        '14-july-09
    '        'DropdownlistTermination.SelectedValue = String.Empty
    '        'ButtonTerminate.Enabled = True
    '        '-To Do Enabled Terminated controls and populate data
    '    End If
    '    'End 23-July-2010 BT-537

    '    'TextboxReactivate.Text = String.Empty 
    '    'ButtonReactivate.Enabled = False

    '    'TextboxMetroDetails.Text = String.Empty 
    '    '14-July-09
    '    'DropdownlistMergeDate.SelectedValue = String.Empty 
    '    'Priya:17-March-2010:YRS 5.0-1017:Display withdrawal date as merged date for branches
    '    'DropDownYMCANo.SelectedValue = String.Empty 

    '    'End 17-March-2010:YRS 5.0-1017

    '    'ButtonMerge.Enabled = True

    '    Dim drYMCAEntrants As DataRow = l_ds_DataSetStatus.Tables("AtsYmcaEntrants").Rows(0)

    '    If l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).IsNull("guiYMCAMetroID") = False AndAlso _
    '            l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).Item("guiYMCAMetroID").ToString() <> "" Then
    '        LabelCurrentStatus.Text = "Merged YMCA"
    '        'start :Commented and added by Dilip : 09-July-09 : BT - 838
    '        'DropdownWithdrawDate.Enabled = False 
    '        'Call function to set Dropdown values.
    '        'setDropdown(DropdownWithdrawDate, drYMCAEntrants("dtswithdrawaldate").ToString())
    '        'End : Commented and added by Dilip : 09-July-09 : BT - 838

    '        'Priya:17-March-2010:YRS 5.0-1017:Display withdrawal date as merged date for branches
    '        'setDropdown(DropdownlistMergeDate, drYMCAEntrants("dtswithdrawaldate").ToString())
    '        Dim strMetroYMCAId As String
    '        strMetroYMCAId = String.Empty
    '        If (l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).Item("guiYMCAMetroID").ToString() <> "" AndAlso Not l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).Item("guiYMCAMetroID").ToString() Is Nothing) Then
    '            strMetroYMCAId = l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).Item("guiYMCAMetroID").ToString()
    '        End If

    '        'PopulateDropDownYMCANo(strMetroYMCAId.Trim)
    '        'DropDownYMCANo.SelectedValue = l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).Item("guiYMCAMetroID").ToString()
    '        'GetMetroYMCAUniqueId() 
    '        'TextBoxMetroName.Text = TextboxMetroDetails.Text.Trim 
    '        'DropDownYMCANo.Enabled = False 
    '        'LabelMetroDetails.Enabled = False 
    '        'End YRS 5.0-1017

    '        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
    '        'DropdownYerdiAccess_W.SelectedValue = l_int_AllowYerdiAccess.ToString()
    '        'DropdownYerdiAccess_T.SelectedValue = l_int_AllowYerdiAccess.ToString()
    '        'New Added Priya:16-April-2010
    '        'DropdownYerdiAccess_T.Enabled = False 
    '        'LabelYerdiAccess_T.Enabled = False
    '        'End YRS 5.0-873

    '        'ButtonWithdraw.Enabled = False 
    '        'ButtonTerminate.Enabled = False 
    '        'ButtonReactivate.Enabled = False 
    '        'ButtonMerge.Enabled = False 
    '        ButtonResoAdd.Enabled = False
    '    ElseIf drYMCAEntrants.IsNull("dtsterminationdate") = False AndAlso _
    '        drYMCAEntrants.Item("dtsterminationdate").ToString() <> "" Then
    '        LabelCurrentStatus.Text = "Terminated YMCA"
    '        'ButtonWithdraw.Enabled = False 

    '        'Start : Commented and added by Dilip : 09-July-09 : BT - 838
    '        'setDropdown(DropdownWithdrawDate, drYMCAEntrants("dtswithdrawaldate").ToString()) 
    '        'END : Commented and added by Dilip : 09-July-09 : BT - 838

    '        'ButtonTerminate.Enabled = False 
    '        '14-July-09
    '        'DropdownlistTermination.Enabled = False 
    '        '14-July-09
    '        'Call setdropdown function
    '        'setDropdown(DropdownlistTermination, drYMCAEntrants("dtsterminationdate").ToString()) 
    '        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
    '        'DropdownYerdiAccess_W.SelectedValue = l_int_AllowYerdiAccess.ToString()
    '        'DropdownYerdiAccess_T.Enabled = True 
    '        'LabelYerdiAccess_T.Enabled = True 
    '        ' YRS 5.0-873
    '        'DropdownYerdiAccess_T.SelectedValue = l_int_AllowYerdiAccess.ToString() 
    '        If CType(Session("ReactivateYMCA"), Boolean) = True Then
    '            'ButtonReactivate.Enabled = True 
    '            'LabelReactivate.Enabled = True
    '            'TextboxReactivate.Enabled = True
    '            'PopcalendarReactivate.Enabled = True 
    '            'TextboxReactivate.Text = Session("ReactivateDate")
    '        Else
    '            'TextboxReactivate.Text = String.Empty
    '            'ButtonReactivate.Enabled = True 
    '            'Priya 29-Jan-2009 YRS 5.0-650 :is assn had an active resolution but the Withdrawn YMCA banner is still appearing in the General tab.
    '            If Not Session("ReactivateDate") = Nothing Then
    '                'If (TextboxReactivate.Text <> "") Then
    '                TextBoxEnrollmentDate.Text = Session("ReactivateDate")
    '            End If
    '        End If
    '        'End 29-Jan-2009
    '    End If
    '    'Swopna 12 May,2008
    '    'TextboxMetroDetails.Text = String.Empty 
    '    '14-July-09
    '    'DropdownlistMergeDate.SelectedValue = String.Empty 
    '    'ButtonMerge.Enabled = False 
    '    'ElseIf drYMCAEntrants.IsNull("dtswithdrawaldate") = False AndAlso drYMCAEntrants.Item("dtswithdrawaldate").ToString() <> "" Then
    '    LabelCurrentStatus.Text = "Withdrawn YMCA"
    '    'ButtonWithdraw.Enabled = False
    '    'Start : Commented and added by Dilip : 09-July-09 : BT - 838
    '    'setDropdown(DropdownWithdrawDate, drYMCAEntrants("dtswithdrawaldate").ToString())
    '    'ENDS : Commented and added by Dilip : 09-July-09 : BT - 838

    '    'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
    '    'DropdownYerdiAccess_W.SelectedValue = l_int_AllowYerdiAccess.ToString()
    '    'LabelYerdiAccess_T.Enabled = True
    '    'DropdownYerdiAccess_T.Enabled = True
    '    'DropdownYerdiAccess_T.SelectedValue = l_int_AllowYerdiAccess.ToString()
    '    ' YRS 5.0-873

    '    'LabelTerminationDate.Enabled = False
    '    'Priya 23-July-2010  BT-537:While saving terminate date application shows Error page.
    '    'Added if condition If session for terminate ymca is not true or nothing then only set selscted value
    '    'If Session("TerminateYMCA") = Nothing AndAlso Session("TerminateYMCA") = False Then
    '    '    DropdownlistTermination.Enabled = False
    '    '    '14-July-09
    '    '    DropdownlistTermination.SelectedValue = String.Empty
    '    '    ButtonTerminate.Enabled = True
    '    'End If
    '    'End 
    '    'End 23-July-2010  BT-537:
    '    If CType(Session("ReactivateYMCA"), Boolean) = True Then
    '        'ButtonReactivate.Enabled = True 
    '        'LabelReactivate.Enabled = True 
    '        'TextboxReactivate.Enabled = True 
    '        'PopcalendarReactivate.Enabled = True 
    '        'TextboxReactivate.Text = Session("ReactivateDate")
    '    Else
    '        'TextboxReactivate.Text = String.Empty
    '        'ButtonReactivate.Enabled = True
    '        'Priya 29-Jan-2009 YRS 5.0-650 :is assn had an active resolution but the Withdrawn YMCA banner is still appearing in the General tab.
    '        If Not Session("ReactivateDate") = Nothing Then
    '            'If (TextboxReactivate.Text <> "") Then
    '            TextBoxEnrollmentDate.Text = Session("ReactivateDate")
    '        End If
    '    End If
    '    'End 29-Jan-2009
    '    'End If

    '    'TextboxMetroDetails.Text = String.Empty
    '    '14-July-09
    '    'DropdownlistMergeDate.SelectedValue = String.Empty
    '    'ButtonMerge.Enabled = False
    '    'End If

    '    If LabelCurrentStatus.Text = "" Then
    '        'DropdownWithdrawDate.Enabled = False 
    '        ' DropdownlistTermination.Enabled = False
    '        'DropdownYerdiAccess_T.Enabled = False
    '        ' LabelYerdiAccess_T.Enabled = False
    '    End If

    'End Sub
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code

    Private Sub PopulateOfficer()
        Dim ds As DataSet = DirectCast(Session("YMCA Officer"), DataSet)
        If Not IsNothing(ds) Then
            DataGridYMCAOfficer.DataSource = ds
            DataGridYMCAOfficer.DataBind()
            DataGridYMCAOfficer.SelectedIndex = -1 'SHASHI SHEKHAR : 27 JULY 2010 : FOR BT - 514
        End If
    End Sub
    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added Method to populate W/T/S/M tabs
    Private Sub PopulateWTSMTab()

        LabelSummaryReport.Visible = False
        LabelBeforeWithdrawal.Visible = False
        LabelAfterWithdrawal.Visible = False
        Me.WTSM_Operation = ""

        Dim l_ds_DataSetStatus As DataSet
        l_ds_DataSetStatus = YMCARET.YmcaBusinessObject.YMCABOClass.GetStatus(DirectCast(Session("GuiUniqueId"), String))
        'If HelperFunctions.isEmpty(l_ds_DataSetStatus) Then Exit Sub
        'If HelperFunctions.isEmpty(l_ds_DataSetStatus.Tables("AtsYmcaEntrants")) Then Exit Sub
        'If HelperFunctions.isEmpty(l_ds_DataSetStatus.Tables("atsYmcas")) Then Exit Sub

        Dim strMetroYMCAId As String = ""
        If HelperFunctions.isEmpty(l_ds_DataSetStatus) = False Then
            strMetroYMCAId = String.Empty
            If (l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).Item("guiYMCAMetroID").ToString() <> "" AndAlso Not l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).Item("guiYMCAMetroID").ToString() Is Nothing) Then
                strMetroYMCAId = l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).Item("guiYMCAMetroID").ToString()
            End If
        End If

        LabelCurrentStatus.Text = ""

        DateUserControlWithdrawDate.Enabled = True
        DateUserControlSuspensionDate.Enabled = True
        DateUserControlWithdrawDate.Visible = True
        txtDateWithdraw.Visible = False
        DateUserControlWithdrawDate.Text = ""
        DateUserControlSuspensionDate.Text = ""
        DateUserControlMergeDate.Text = ""
        DateUserControlReactivateDate.Text = ""

        DropdownPayrollDate_W.SelectedIndex = 0
        DropdownPayrollDate_T.SelectedIndex = 0
        DropdownPayrollDate_S.SelectedIndex = 0
        DropdownPayrolldate_M.SelectedIndex = 0

        DropdownYerdiAccess_W.SelectedValue = 0
        DropdownYerdiAccess_T.SelectedValue = 0
        DropdownYerdiAccess_S.SelectedValue = 0
        DropdownYerdiAccess_M.SelectedValue = 0
        DropDownYMCANo.SelectedValue = ""

        DropdownPayrollDate_W.Enabled = True
        DropdownPayrollDate_T.Enabled = True
        DropdownPayrollDate_S.Enabled = True

        DropdownYerdiAccess_W.Enabled = True
        DropdownYerdiAccess_T.Enabled = True
        DropdownYerdiAccess_S.Enabled = True

        'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-1687 | Allowing merging of Metro YMCA along with independent YMCA
        If (DropDownHubType.SelectedValue = "") Or (DropDownHubType.SelectedValue = "M" Or DropDownHubType.SelectedValue = "I") Then
            PopulateDropDownYMCANo("")
            DropdownPayrolldate_M.Enabled = True
            DropdownYerdiAccess_M.Enabled = True
            DateUserControlMergeDate.Enabled = True
        End If
        'End - Manthan Rajguru | 2016.02.03 | YRS-AT-1687 | Allowing merging of Metro YMCA along with independent YMCA
        Populatedates(DropdownPayrollDate_W)
        Populatedates(DropdownPayrollDate_T)
        Populatedates(DropdownPayrollDate_S)
        Populatedates(DropdownPayrolldate_M)

        DateUserControlReactivateDate.Enabled = False
        DateUserControlReactivateDate.Visible = False
        txtDateReactivate.Visible = True

        DateUserControlSuspensionDate.Visible = True
        txtDateSuspend.Visible = False

        DateUserControlMergeDate.Visible = True
        txtDateMerge.Visible = False

        ButtonSave.Enabled = True

        hdnSelectedTab.Value = "#tabContent_Withdraw"
        ApplyWTSM_SelectedMenuStyle("tab_btnWithdrawClick")

        If Session("YMCAWithdrawn") Is Nothing AndAlso Session("YMCASuspend") Is Nothing AndAlso Session("YMCATermination") Is Nothing Then 'Manthan Rajguru | 2016.02.16 | YRS-AT-2334 & 1686 | Checking session boolean value to hide datagrid
            HideSummaryDatagrid(DataGridSummaryReport)
            HideSummaryDatagrid(DatagridSummaryReportOnWithdrawal)

            HideSummaryDatagrid(DataGridSummaryReportBeforeSuspend)
            HideSummaryDatagrid(DatagridSummaryReportOnSuspend)

            HideSummaryDatagrid(DataGridSummaryReportBeforeTermination)
            HideSummaryDatagrid(DatagridSummaryReportOnTerminate)
            LabelEmpRecord.Text = ""
            LabelEmpRecordSuspend.Text = ""
            LabelEmpRecordTerminate.Text = ""

        End If

        If HelperFunctions.isEmpty(l_ds_DataSetStatus) Then Exit Sub
        Dim drYMCAEntrants As DataRow = l_ds_DataSetStatus.Tables("AtsYmcaEntrants").Rows(0)
        Dim drYMCAS As DataRow = l_ds_DataSetStatus.Tables("AtsYmcas").Rows(0)
        'Susepnd
        If drYMCAEntrants.IsNull("dtsEffSuspendDate") = False AndAlso drYMCAEntrants.Item("dtsEffSuspendDate").ToString() <> "" Then
            LabelCurrentStatus.Text = "Suspended YMCA"

            DropDownYMCANo.Enabled = False
            DropdownPayrolldate_M.Enabled = False
            DateUserControlMergeDate.Visible = False
            txtDateMerge.Visible = True
            DropdownYerdiAccess_M.Enabled = False

            DateUserControlWithdrawDate.Enabled = True
            DropdownPayrollDate_W.Enabled = True
            DropdownYerdiAccess_W.Enabled = True
            Populatedates(DropdownPayrollDate_W)

            DropdownPayrollDate_T.Enabled = True
            DropdownYerdiAccess_T.Enabled = True
            Populatedates(DropdownPayrollDate_T)

            DateUserControlReactivateDate.Enabled = True
            DateUserControlReactivateDate.Visible = True
            txtDateReactivate.Visible = False

            DateUserControlSuspensionDate.Visible = False
            txtDateSuspend.Visible = True
            DropdownPayrollDate_S.Enabled = False
            DropdownYerdiAccess_S.Enabled = False
            If Not Convert.IsDBNull(drYMCAEntrants.Item("dtsEffSuspendDate")) Then
                txtDateSuspend.Text = Convert.ToDateTime(drYMCAEntrants.Item("dtsEffSuspendDate")).ToString("MM/dd/yyyy")
            End If
            setDropdown(DropdownPayrollDate_S, drYMCAEntrants("dtswithdrawaldate").ToString())
            setDropdown(DropdownYerdiAccess_S, Convert.ToBoolean(drYMCAEntrants("bitAllowYerdiAccess").ToString()))
            hdnSelectedTab.Value = "#tabContent_Suspend"
            ApplyWTSM_SelectedMenuStyle("tab_btnSuspendClick")

        End If

        'Withdrawal
        Dim l_bool_NotSuspended As Boolean = (drYMCAEntrants.Item("dtsEffSuspendDate").ToString() = "" AndAlso drYMCAEntrants.Item("dtswithdrawaldate").ToString() <> "")
        Dim l_bool_YMCAMerged As Boolean = (drYMCAS.Item("guiYMCAMetroID").ToString() <> "")
        If (l_bool_NotSuspended = True Or drYMCAEntrants.Item("dtsEffwithdrawalDate").ToString() <> "") AndAlso l_bool_YMCAMerged = False Then
            LabelCurrentStatus.Text = "Withdrawn YMCA"
            DateUserControlWithdrawDate.Visible = False
            txtDateWithdraw.Visible = True
            DropdownPayrollDate_W.Enabled = False
            DropdownYerdiAccess_W.Enabled = False
            If Not Convert.IsDBNull(drYMCAEntrants.Item("dtsEffwithdrawalDate")) Then
                txtDateWithdraw.Text = Convert.ToDateTime(drYMCAEntrants.Item("dtsEffwithdrawalDate")).ToString("MM/dd/yyyy")
            End If
            setDropdown(DropdownPayrollDate_W, drYMCAEntrants("dtswithdrawaldate").ToString())
            setDropdown(DropdownYerdiAccess_W, Convert.ToBoolean(drYMCAEntrants("bitAllowYerdiAccess").ToString()))
            DropDownYMCANo.Enabled = False
            DateUserControlMergeDate.Visible = False
            txtDateMerge.Visible = True
            DropdownPayrolldate_M.Enabled = False
            DropdownYerdiAccess_M.Enabled = False

            DropdownPayrollDate_T.Enabled = True
            DropdownYerdiAccess_T.Enabled = True
            Populatedates(DropdownPayrollDate_T)
            DateUserControlReactivateDate.Enabled = True
            DateUserControlReactivateDate.Visible = True
            txtDateReactivate.Visible = False
            DateUserControlSuspensionDate.Visible = False
            txtDateSuspend.Visible = True
            DropdownPayrollDate_S.Enabled = False
            DropdownYerdiAccess_S.Enabled = False

            hdnSelectedTab.Value = "#tabContent_Withdraw"
            ApplyWTSM_SelectedMenuStyle("tab_btnWithdrawClick")
            If Session("YMCAWithdrawn") Is Nothing Then 'Manthan Rajguru | 2016.02.16 | YRS-AT-2334 & 1686 | Checking session boolean value to hide datagrid
                HideSummaryDatagrid(DataGridSummaryReport)
                HideSummaryDatagrid(DatagridSummaryReportOnWithdrawal)
            End If
        End If

        'Merge
        If l_bool_YMCAMerged = True Then
            LabelCurrentStatus.Text = "Merged YMCA"
            PopulateDropDownYMCANo(strMetroYMCAId.Trim)
            DropDownYMCANo.SelectedValue = l_ds_DataSetStatus.Tables("atsYmcas").Rows(0).Item("guiYMCAMetroID").ToString()
            DateUserControlWithdrawDate.Visible = False
            txtDateWithdraw.Visible = True
            DropdownPayrollDate_W.Enabled = False
            DropdownYerdiAccess_W.Enabled = False

            DropdownPayrollDate_T.Enabled = False
            DropdownYerdiAccess_T.Enabled = False

            DateUserControlSuspensionDate.Visible = False
            txtDateSuspend.Visible = True
            DropdownPayrollDate_S.Enabled = False
            DropdownYerdiAccess_S.Enabled = False

            DateUserControlReactivateDate.Visible = False
            txtDateReactivate.Visible = True

            setDropdown(DropdownPayrolldate_M, drYMCAEntrants("dtswithdrawaldate").ToString())
            If Not Convert.IsDBNull(drYMCAEntrants.Item("dtsEffMergeDate")) Then
                txtDateMerge.Text = Convert.ToDateTime(drYMCAEntrants.Item("dtsEffMergeDate")).ToString("MM/dd/yyyy")
            End If
            setDropdown(DropdownYerdiAccess_M, Convert.ToBoolean(drYMCAEntrants("bitAllowYerdiAccess").ToString()))
            DropDownYMCANo.Enabled = False
            DateUserControlMergeDate.Visible = False
            txtDateMerge.Visible = True
            DropdownPayrolldate_M.Enabled = False
            DropdownYerdiAccess_M.Enabled = False
            hdnSelectedTab.Value = "#tabContent_Merge"
            ApplyWTSM_SelectedMenuStyle("tab_btnMergeClick")
        End If

        If drYMCAEntrants.IsNull("dtsTerminationdate") = False AndAlso drYMCAEntrants.Item("dtsTerminationdate").ToString() <> "" Then
            LabelCurrentStatus.Text = "Terminated YMCA"
            DateUserControlWithdrawDate.Visible = False
            txtDateWithdraw.Visible = True
            DropdownPayrollDate_W.Enabled = False
            DropdownYerdiAccess_W.Enabled = False

            setDropdown(DropdownPayrollDate_T, drYMCAEntrants("dtsTerminationdate").ToString())
            setDropdown(DropdownYerdiAccess_T, Convert.ToBoolean(drYMCAEntrants("bitAllowYerdiAccess").ToString()))
            DropdownPayrollDate_T.Enabled = False
            DropdownYerdiAccess_T.Enabled = False

            DropDownYMCANo.Enabled = False
            DateUserControlMergeDate.Visible = False
            txtDateMerge.Visible = True
            DropdownPayrolldate_M.Enabled = False
            DropdownYerdiAccess_M.Enabled = False

            DateUserControlSuspensionDate.Enabled = False
            DropdownPayrollDate_S.Enabled = False
            DropdownYerdiAccess_S.Enabled = False
            If Not Convert.IsDBNull(drYMCAEntrants.Item("dtsEffSuspendDate")) Then
                txtDateSuspend.Text = Convert.ToDateTime(drYMCAEntrants.Item("dtsEffSuspendDate")).ToString("MM/dd/yyyy")
                setDropdown(DropdownPayrollDate_S, drYMCAEntrants("dtswithdrawaldate").ToString())
            End If
            setDropdown(DropdownYerdiAccess_S, Convert.ToBoolean(drYMCAEntrants("bitAllowYerdiAccess").ToString()))

            hdnSelectedTab.Value = "#tabContent_Terminate"
            DateUserControlReactivateDate.Enabled = True
            DateUserControlReactivateDate.Visible = True
            txtDateReactivate.Visible = False
            ApplyWTSM_SelectedMenuStyle("tab_btnTerminateClick")

        End If
    End Sub

    Private Sub ApplyWTSM_SelectedMenuStyle(ByVal SelectedMenuName As String)
        tab_btnTerminateClick.Attributes.Add("class", "tabNotSelected1")
        tab_btnMergeClick.Attributes.Add("class", "tabNotSelected1")
        tab_btnWithdrawClick.Attributes.Add("class", "tabNotSelected1")
        tab_btnSuspendClick.Attributes.Add("class", "tabNotSelected1")
        tab_btnReactivateClick.Attributes.Add("class", "tabNotSelected1")

        If (SelectedMenuName = "tab_btnWithdrawClick") Then
            tab_btnWithdrawClick.Attributes.Add("class", "tabSelected1")
        ElseIf (SelectedMenuName = "tab_btnTerminateClick") Then
            tab_btnTerminateClick.Attributes.Add("class", "tabSelected1")
        ElseIf (SelectedMenuName = "tab_btnSuspendClick") Then
            tab_btnSuspendClick.Attributes.Add("class", "tabSelected1")
        ElseIf (SelectedMenuName = "tab_btnMergeClick") Then
            tab_btnMergeClick.Attributes.Add("class", "tabSelected1")
        ElseIf (SelectedMenuName = "tab_btnReactivateClick") Then
            tab_btnReactivateClick.Attributes.Add("class", "tabSelected1")
        End If
    End Sub
    'End -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added Method to populate W/T/S/M tabs


    Private Sub PopulateContact()
        Dim ds As DataSet = DirectCast(Session("YMCA Contact"), DataSet)
        If Not IsNothing(ds) Then
            Me.DataGridYMCAContact.DataSource = ds
            Me.DataGridYMCAContact.DataBind()
            DataGridYMCAContact.SelectedIndex = -1 'SHASHI SHEKHAR : 27 JULY 2010 : FOR BT - 514
        End If
    End Sub
    Private Sub PoplulateBranch()
        Dim ds As DataSet = DirectCast(Session("YMCA Branch"), DataSet)
        If Not IsNothing(ds) Then
            Me.DataGridYMCABranches.DataSource = ds
            Me.DataGridYMCABranches.DataBind()
        End If
    End Sub
    Private Sub PopulateResolution()
        Dim ds As DataSet = DirectCast(Session("YMCA Resolution"), DataSet)
        If Not IsNothing(ds) Then
            DataGridYMCAResolutions.DataSource = ds
            DataGridYMCAResolutions.DataBind()
        End If
    End Sub
    Private Sub PopulateBank()
        Dim ds As DataSet = DirectCast(Session("YMCA BankInfo"), DataSet)
        If Not IsNothing(ds) Then
            Me.DataGridYMCABankInfo.DataSource = ds
            Me.DataGridYMCABankInfo.DataBind()
        End If
    End Sub
    Private Sub PopulateNote()
        g_DataSetYMCANotes = DirectCast(Session("YMCA Notes"), DataSet)
        If HelperFunctions.isNonEmpty(g_DataSetYMCANotes) Then
            dvYmcaNotes = g_DataSetYMCANotes.Tables(0).DefaultView
            dvYmcaNotes.Sort = "Date Desc"
            Dim l_datarow As DataRow()
            l_datarow = g_DataSetYMCANotes.Tables(0).Select("bitImportant = 1")
            If l_datarow.Length > 0 Then
                Me.YMCATabStrip.Items(7).Text = "<font color=red>Notes</font>"
            Else
                Me.YMCATabStrip.Items(7).Text = "<font color=orange>Notes</font>"
            End If
        Else
            YMCATabStrip.Items(7).Text = "Notes"
        End If

        Me.DataGridYMCANotesHistory.DataSource = dvYmcaNotes
        Me.DataGridYMCANotesHistory.DataBind()
    End Sub
    Private Sub PopulateTelephone()
        g_dataset_dsTelephoneInformation = DirectCast(Session("Telephone Information"), DataSet)
        TextBoxTelephone.Text = String.Empty
        If HelperFunctions.isNonEmpty(g_dataset_dsTelephoneInformation) Then
            Dim dr As DataRow()
            dr = g_dataset_dsTelephoneInformation.Tables(0).Select("Primary = 1 And Active = 1")
            If dr.Length > 0 Then
                If Not dr(0).IsNull("Telephone") Then TextBoxTelephone.Text = DirectCast(dr(0)("Telephone"), String).Trim
            Else
                TextBoxTelephone.Text = Convert.ToString(g_dataset_dsTelephoneInformation.Tables(0).Rows(0)("Telephone")).Trim
            End If
        End If

    End Sub
    Private Sub PopulateEmail()
        g_dataset_dsEmailInformation = DirectCast(Session("Email Information"), DataSet)
        TextBoxEmail.Text = String.Empty
        If HelperFunctions.isNonEmpty(g_dataset_dsEmailInformation) Then
            Dim dr As DataRow()
            dr = g_dataset_dsEmailInformation.Tables(0).Select("Primary = 1 And Active = 1")
            If dr.Length > 0 Then
                If Not dr(0).IsNull("Email Address") Then TextBoxEmail.Text = DirectCast(dr(0)("Email Address"), String).Trim
            Else
                TextBoxEmail.Text = Convert.ToString(g_dataset_dsEmailInformation.Tables(0).Rows(0)("Email Address")).Trim
            End If
        End If

    End Sub
#End Region

#Region "All Events"
    'Anudeep:29.10.2013:BT:2272:Removed Me.Load for not occuring second time post back
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load ', Me.Load
        If MyBase.Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Try

            'Shashi shekhar:21 June-2011: For YRS 5.0-1334: 
            'Check first if key is defined in web.config file then get the value from there else get default value.
            If System.Web.Configuration.WebConfigurationManager.AppSettings("RelationManagerSelectionLimit") IsNot Nothing Then
                If (ConfigurationManager.AppSettings.Get("RelationManagerSelectionLimit").ToString().Trim <> "") Then
                    intRelationManagerselectionLimit = CType(ConfigurationManager.AppSettings.Get("RelationManagerSelectionLimit").ToString().Trim, Int32)
                End If
            Else
                intRelationManagerselectionLimit = 2
            End If
            '--------------------------------------------------------------------------------------------------------------


            If Page.IsPostBack = False Then
                Clear_session()

            End If

            If Not IsNothing(Session("PopUpAction")) Then
                Dim objPopUpResult As PopupResult = Session("PopUpAction")
                Session("PopUpAction") = Nothing
                HandlePopupActions(objPopUpResult)
            End If

            SetAttributes()
            'Swopna 15May08
            LabelResoNotEntered.Text = String.Empty
            'Swopna 15May08
            'Swopna Phase IV changes 9 Apr,2008 
            If Session("ReactivateYMCA") Is Nothing Then
                If LabelCurrentStatus.Text = "Terminated YMCA" Or LabelCurrentStatus.Text = "Withdrawn YMCA" Or LabelCurrentStatus.Text = "Merged YMCA" Then
                    ButtonResoAdd.Visible = False
                ElseIf LabelCurrentStatus.Text = "" Then
                    ButtonResoAdd.Visible = True : ButtonResoAdd.Enabled = True
                End If
            Else
                ButtonResoAdd.Visible = True : ButtonResoAdd.Enabled = True
            End If
            'Swopna-7 May,2008-Bug Id 414,8 May 2008 Bug Id 431---------end
            If Me.IsPostBack Then
                'Swopna 12 May,2008-start
                If CType(Session("ReactivateYMCA"), Boolean) = True Then
                    'Session("ReactivateDate") = TextboxReactivate.Text
                    Session("ReactivateDate") = DateUserControlReactivateDate.Text
                End If
                'Swopna 12 May,2008-end
                If Not Session("Error") Is Nothing Then
                    If Session("Error") = True Then
                        Session("Error") = Nothing
                    End If
                Else

                    'Start : Commented by Dilip : 09-July-09 : BT - 838 - Removing the 1st of the month check
                    'Called during withdrawal process
                    'Check whether entered withdrawal date is first of month or not.Proceed on the basis of user's response. 
                    If Not Session("Withdrawn") Is Nothing AndAlso _
                        Session("Withdrawn") = True Then
                        'Dim arr As Array = TextboxWithdrawDate.Text.Split("/")
                        'If arr.Length > 1 AndAlso arr(1) > 1 Then
                        '    If Session("FirstOfMonth_W") <> "True" AndAlso Session("FirstOfMonth_W") <> TextboxWithdrawDate.Text.Trim() Then
                        '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "You have entered a date that is not the first of the month. Do you wish to continue using this date?", MessageBoxButtons.YesNo)
                        '        Session("FirstOfMonth_W") = "True"
                        '        Exit Sub
                        '    End If
                        'End If
                        'If Session("FirstOfMonth_W") = "True" Then
                        '    If Request.Form("YES") = "Yes" Then
                        '        Session("FirstOfMonth_W") = TextboxWithdrawDate.Text
                        '    ElseIf Request.Form("NO") = "No" Then
                        '        arr(1) = "01"
                        '        TextboxWithdrawDate.Text = arr(0) + "/" + arr(1) + "/" + arr(2)
                        '        Session("FirstOfMonth_W") = TextboxWithdrawDate.Text.Trim()
                        '    Else
                        '        Session("FirstOfMonth_W") = Nothing
                        '    End If
                        '    Exit Sub
                        'End If

                        'Priya 02-March-2009 : YRS 5.0-517 - Remove the 6 months before/after validation in the withdrawal process.                    
                        If Session("6MonthValidation") = "True" Then
                            If Request.Form("YES") = "Yes" Then
                                Session("6MonthValidation") = DropdownPayrollDate_W.SelectedValue
                                LoadWTSMRGrid(hdnSelectedTab.Value)
                                ButtonSave_Click(Me, e)
                                Exit Sub
                            ElseIf Request.Form("NO") = "No" Then
                                LoadWTSMRGrid(hdnSelectedTab.Value)
                                Session("6MonthValidation") = "False"
                            Else    'Default case
                                Session("6MonthValidation") = Nothing
                            End If
                        End If
                        '    'End 02-March-2009
                        '    'END : Commented by Dilip : 09-July-09 : BT - 838

                    End If
                    'End

                    'Called during termination
                    'Check whether entered termination date is first of month or not.Proceed on the basis of user's response.
                    'If Not Session("TerminateYMCA") Is Nothing AndAlso Session("TerminateYMCA") = True Then
                    'Dlip - Removing the 1st of the month validation
                    'Dim arrT As Array = TextboxTerminationDate.Text.Trim().Split("/")
                    'If arrT.Length > 1 AndAlso arrT(1) > 1 Then
                    '    If Session("FirstOfMonth_T") <> "True" AndAlso Session("FirstOfMonth_T") <> TextboxTerminationDate.Text.Trim Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "You have entered a date that is not the first of the month. Do you wish to continue using this date?", MessageBoxButtons.YesNo)
                    '        Session("FirstOfMonth_T") = "True"
                    '        Exit Sub
                    '    End If

                    '    If Session("FirstOfMonth_T") = "True" Then
                    '        If Request.Form("YES") = "Yes" Then
                    '            Session("FirstOfMonth_T") = TextboxTerminationDate.Text.Trim()
                    '        ElseIf Request.Form("No") = "No" Then
                    '            arrT(1) = "01"
                    '            TextboxTerminationDate.Text = arrT(0) + "/" + arrT(1) + "/" + arrT(2)
                    '            Session("FirstOfMonth_T") = TextboxTerminationDate.Text.Trim()
                    '        Else
                    '            Session("FirstOfMonth_T") = Nothing
                    '        End If
                    '        Exit Sub
                    '    End If
                    'End If


                    If Session("6MonthValidation_T") = "True" Then
                        If Request.Form("YES") = "Yes" Then
                            '14-July-09
                            Session("6MonthValidation_T") = DropdownPayrollDate_T.SelectedValue
                            LoadWTSMRGrid(hdnSelectedTab.Value)
                            ButtonSave_Click(Me, e)
                            Exit Sub
                        ElseIf Request.Form("NO") = "No" Then
                            LoadWTSMRGrid(hdnSelectedTab.Value)
                            Session("6MonthValidation_T") = "False"
                        Else    'Default case
                            Session("6MonthValidation_T") = Nothing
                        End If
                    End If
                    'End If
                End If
            End If
            'Swopna 26May08--start
            'For withdrawal process
            If Not Session("ProcessProceed") = Nothing Then
                If Request.Form("YES") = "Yes" And Session("ProcessProceed") = True Then
                    Session("ProcessProceed") = False
                    ButtonSave_Click(Me, e)
                    LoadWTSMRGrid(hdnSelectedTab.Value)
                    'Priya 12-March-2009 YRS 5.0-517 - Remove the 6 months before/after validation in the withdrawal process.
                ElseIf Request.Form("NO") = "No" And Session("ProcessProceed") = True Then
                    Session("ProcessProceed") = Nothing
                    LoadWTSMRGrid(hdnSelectedTab.Value) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added to load selected tab
                    'End 12-March-2009
                End If
            End If
            'For termination process
            If Not Session("TermProcessProceed") = Nothing Then
                If Request.Form("YES") = "Yes" And Session("TermProcessProceed") = True Then
                    Session("TermProcessProceed") = False
                    LoadWTSMRGrid(hdnSelectedTab.Value) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added to load selected tab
                    ButtonSave_Click(Me, e)
                    'Priya 31-March-2009:YRS 5.0-707: added else if conditon, if request is no then it will set session variable to nothing.
                ElseIf Request.Form("NO") = "No" And Session("TermProcessProceed") = True Then
                    Session("TermProcessProceed") = Nothing
                    LoadWTSMRGrid(hdnSelectedTab.Value) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added to load selected tab
                End If
            End If
            'Swopna 26May08--end
            'Swopna Phase IV changes 9 Apr,2008
            If Not Me.IsPostBack Then

                PopulateYMCARelationManagers() 'Shashi Shekhar: For BT-844, YRS 5.0-1334:Add field for Y-Relations Manager 
                getSecuredControls()
                PopulatePaymentMethod()
                PopulateBillingMethod()

                Session("BoolEditFlag") = False

                'coming after Metro popup
                g_bool_flagMetro = Session("Metro")

                'By Aparna -23/01/2007 
                'If DirectCast(Session("Metro"), Boolean) = True Then
                '    'EnableSaveCancelButtons()
                'End If

                '13-May-2009
                If IsNothing(Session("PopUpAction")) Then
                    'if not ispostback
                    If Page_Mode = String.Empty Then
                        If HelperFunctions.isNonEmpty(YMCASearchList) Then
                            EnableAllTabs()
                        Else
                            DisableAllTabsExcept(0)
                        End If
                    Else
                        EnableAllTabs()
                    End If
                End If
                '13-May-2009
                'Added for YRS:5.0-1621 on 2012.09.26 by Anudeep
                ButtonAdd.Enabled = True
            Else
                'if deleting the row from Officer Tab
                g_bool_DeleteOfficerFlag = CType(Session("DeleteOfficerFlag"), Boolean)
                If g_bool_DeleteOfficerFlag = True Then
                    g_bool_DeleteOfficerFlag = False
                    Session("DeleteOfficerFlag") = g_bool_DeleteOfficerFlag
                    If Request.Form("Yes") = "Yes" Then
                        DeleteOfficerSub()
                        'EnableSaveCancelButtons()
                    Else
                        Me.DataGridYMCAOfficer.SelectedIndex = -1
                        'EnableSaveCancelButtons()
                    End If
                End If

                'if deleting the row from Contact Tab
                g_bool_DeleteContactFlag = CType(Session("DeleteContactFlag"), Boolean)
                If g_bool_DeleteContactFlag = True Then
                    g_bool_DeleteContactFlag = False
                    Session("DeleteContactFlag") = g_bool_DeleteContactFlag
                    If Request.Form("Yes") = "Yes" Then
                        DeleteContactSub(Session("ContactIndex"))
                        'EnableSaveCancelButtons()
                    Else
                        'EnableSaveCancelButtons()
                    End If
                End If

                If Request.Form("OK") = "OK" Then
                    If CType(Session("TermDateIsDBNull"), Boolean) = True Then
                        Session("TermDateIsDBNull") = False
                    End If
                    LoadWTSMRGrid(hdnSelectedTab.Value) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added to load selected tab
                End If
            End If
            Me.LabelHubTypeErrMsg.Visible = False
            Me.LabelResolutionErrMsg.Visible = False
            Me.LabelMetroErrMsg.Visible = False
            Me.LabelAddressErrMsg.Visible = False
            Me.LabelYMCANameErrMsg.Visible = False

            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            'Start: Added by Deven for Seamless Login logic, 16 Aug. 2010

            If Not MyBase.Session("Seamless_From") Is Nothing And Not Page.IsPostBack Then
                'Added Anudeep for YRS:5.0-1621 on 2012.09.26
                ButtonAdd.Enabled = True

                If Convert.ToString(MyBase.Session("Seamless_From")).ToLower = "ymcamaintenance" Then
                    If Not MyBase.Session("Seamless_YMCANo") Is Nothing Then
                        Me.TextBoxYMCANo.Text = Convert.ToString(MyBase.Session("Seamless_YMCANo"))
                    ElseIf Not MyBase.Session("Seamless_guiYMCAID") Is Nothing Then
                        Dim l_dtYMCADataTable As DataTable
                        l_dtYMCADataTable = YMCARET.YmcaBusinessObject.YMCABOClass.GetYMCANoByGuiID(Convert.ToString(MyBase.Session("Seamless_guiYMCAID")))
                        If Not l_dtYMCADataTable Is Nothing Then
                            If l_dtYMCADataTable.Rows.Count > 0 Then
                                Me.TextBoxYMCANo.Text = Convert.ToString(l_dtYMCADataTable.Rows(0)("YMCANo"))
                            End If
                        End If
                    End If
                    MyBase.Session("Seamless_YMCANo") = Nothing
                    MyBase.Session("Seamless_From") = Nothing
                    MyBase.Session("Seamless_guiYMCAID") = Nothing
                    MyBase.Session("Seamless_FromLandingPage") = Nothing 'SR:2013.07.10 - BT-2117/YRS 5.0-2152 : The link from the Employment tab to YMCA Maintenance is broken 
                    If Not String.IsNullOrEmpty(Me.TextBoxYMCANo.Text.Trim()) Then
                        ButtonFind_Click(sender, e)
                        If Not DataGridYMCA.DataSource Is Nothing Then
                            Dim l_dtGridDataSource As DataTable
                            l_dtGridDataSource = DirectCast(DataGridYMCA.DataSource, DataTable)
                            If l_dtGridDataSource.Rows.Count = 1 Then
                                DataGridYMCA.SelectedIndex = 0
                                DataGridYMCA_SelectedIndexChanged(sender, e)
                            End If
                        End If
                    End If
                End If
            End If
            'End: Seamless Login logic
            'Start: Bala: 01/12/2016: YRS-AT-1718: Delete and Save Notes
            If Session("Flag") = "DeleteNotes" Then
                If Request.Form("Yes") = "Yes" Then
                    NotesManagement.DeleteNotes(Session("NotesToDelete"))
                    LoadData(Session("GuiUniqueId"))
                    PopulateNote()
                    Session("NotesToDelete") = ""
                    Session("CurrentProcesstoConfirm") = ""
                ElseIf Request.Form("No") = "No" Then
                    Session("CurrentProcesstoConfirm") = ""
                    Exit Sub
                ElseIf Request.Form("Ok") = "Ok" Then
                    Session("CurrentProcesstoConfirm") = ""
                    Exit Sub
                End If
            ElseIf Session("Flag") = "AddNotes" Then
                LoadData(Session("GuiUniqueId"))
                PopulateNote()
                Session("Flag") = ""
            End If
            'End: Bala: 01/12/2016: YRS-AT-1718: Delete and Save Notes
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub YMCATabStrip_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YMCATabStrip.SelectedIndexChange
        Try
            'Added for YRS:5.0-1621 on 2012.09.26 by Anudeep
            ButtonAdd.Enabled = False

            If Me.YMCATabStrip.SelectedIndex = 0 Then
                PopulateYMCAList()
                'Added  for YRS:5.0-1621 on 2012.09.26 by Anudeep
                ButtonAdd.Enabled = True

            End If
            If Me.DataGridYMCA.SelectedIndex = 0 Then
                Session("l_string_Ymcaname") = Me.DataGridYMCA.SelectedItem.Cells(4).Text.Trim
            End If

            Me.MultiPageYMCA.SelectedIndex = Me.YMCATabStrip.SelectedIndex
            If Me.YMCATabStrip.SelectedIndex = 0 And YmcaInformationHasChanged() Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Please Save/Cancel before moving to a new item.", MessageBoxButtons.OK)
            End If
            'Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security
            If YMCATabStrip.SelectedIndex = 1 Then
                If ButtonEditFedTaxId.Enabled Then
                    EnableDisableControls(False)
                End If
            End If

            If YMCATabStrip.SelectedIndex = 9 Then
                ClientScript.RegisterStartupScript(GetType(Page), "TabChange1", "TabDisplay('" & hdnSelectedTab.Value.ToString & "');", True)
                'If DateUserControlReactivateDate.Text.ToString().Trim() <> "" Then
                '    ApplyWTSM_SelectedMenuStyle("tab_btnReactivateClick")
                'End If
            End If
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonMetro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonMetro.Click
        Try
            Dim popupScript As String = "<script language='javascript'>" & "window.open('YMCAMetrowebForm.aspx', 'YMCAYRS', " & "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScriptRR")) Then
                Page.RegisterStartupScript("PopupScriptRR", popupScript)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonAdress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdress.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAdress", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim popupScript As String = "<script language='javascript'>" & "window.open('YMCAAddressWebForm.aspx?UniqueSessionID=" + uniqueSessionId + "', 'YMCAYRS', " & "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScriptRR")) Then
                Page.RegisterStartupScript("PopupScriptRR", popupScript)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonTelephone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTelephone.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonTelephone", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            Dim popupScript As String = "<script language='javascript'>" & "window.open('YMCATelephoneWebForm.aspx?UniqueSessionID=" + uniqueSessionId + "', 'YMCAYRS', " & "'width=800, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScriptRR")) Then
                Page.RegisterStartupScript("PopupScriptRR", popupScript)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEmail.Click
        Try
            Dim popupScript As String = "<script language='javascript'>" & "window.open('YMCANetWebForm.aspx?UniqueSessionID=" + uniqueSessionId + "', 'YMCAYRS', " & "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScriptRR")) Then
                Page.RegisterStartupScript("PopupScriptRR", popupScript)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Dim l_DataSetYMCAList As DataSet
        Dim l_DataRowYMCAList As DataRow
        Dim l_StringGuiUniqueiD As String

        'Priya:23-March-2010:BT-477: Application displaying "Date cannot be blank" message.
        ValidationSummaryYMCA.Enabled = False

        Try
            uniqueSessionId = Nothing
            Clear_session()
            If Me.TextBoxYMCANo.Text = String.Empty And Me.TextBoxName.Text = String.Empty And Me.TextBoxCity.Text = String.Empty And Me.TextBoxState.Text = String.Empty Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Please Enter a search value", MessageBoxButtons.OK)
                'Priya:23-March-2010:BT-478: Application shows Error page.
                DisableAllTabsExcept(0)
                l_DataSetYMCAList = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAList(Me.TextBoxYMCANo.Text, Me.TextBoxName.Text, Me.TextBoxCity.Text, Me.TextBoxState.Text)
                YMCASearchList = l_DataSetYMCAList
                PopulateYMCAList()
                'End BT-478: Application shows Error page.
                Exit Sub
            End If


            DisableAllTabsExcept(0)
            l_DataSetYMCAList = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAList(Me.TextBoxYMCANo.Text, Me.TextBoxName.Text, Me.TextBoxCity.Text, Me.TextBoxState.Text)
            YMCASearchList = l_DataSetYMCAList
            PopulateYMCAList()

            If HelperFunctions.isEmpty(l_DataSetYMCAList) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "No matching records found", MessageBoxButtons.OK)
                Exit Sub
            End If
            'BT-477: Application displaying "Date cannot be blank" message.
            ValidationSummaryYMCA.Enabled = True
            'End  BT-477
            Me.DataGridYMCA.SelectedIndex = 0

            If HelperFunctions.isNonEmpty(l_DataSetYMCAList) Then
                If Not DataGridYMCA.SelectedItem Is Nothing Then
                    l_DataRowYMCAList = HelperFunctions.GetRowForUpdation(l_DataSetYMCAList.Tables(0), "guiUniqueId", DataGridYMCA.SelectedItem.Cells(1).Text)
                    l_StringGuiUniqueiD = l_DataRowYMCAList.Item("guiUniqueId")
                    'storing the Guid in session to be used in address popup 
                    Session("GuiUniqueId") = l_StringGuiUniqueiD
                    If l_DataRowYMCAList.Item("Type") = "M" Then
                        Me.YMCATabStrip.Items(4).Enabled = True
                    Else
                        Me.YMCATabStrip.Items(4).Enabled = False
                    End If
                    'Populating all textboxes and dropdowns and checkboxes of general tab 
                    PopulateDataIntoGeneralControls(l_StringGuiUniqueiD)
                    EnableAllTabs()
                    'Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security            
                    EnableDisableControls(False)
                End If
            End If
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxYMCANo.Text = String.Empty
            Me.TextBoxName.Text = String.Empty
            Me.TextBoxCity.Text = String.Empty
            Me.TextBoxState.Text = String.Empty
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonOfficersAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOfficersAdd.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonOfficersAdd", Convert.ToInt32(MyBase.Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            g_bool_UpdateFlagOfficer = False
            Session("BoolUpdateFlagOfficer") = g_bool_UpdateFlagOfficer
            g_bool_DeleteOfficerFlag = False
            Session("DeleteOfficerFlag") = g_bool_DeleteOfficerFlag

            'EnableSaveCancelButtons()

            '  Dim msg1 As String = "<script language='javascript'>" & _
            '"window.open('SearchOfficer.aspx','CustomPopUp1', " & _
            '"'width=790, height=490, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
            '"</script>"

            '  Response.Write(msg1)

            Dim msg2 As String = "<script language='javascript'>" & _
            "window.open('AddOfficerWebForm.aspx?UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
            "'width=810, height=572, menubar=no, titlebar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ');" & _
            "</script>"

            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", msg2)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonContactAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonContactAdd.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonContactAdd", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            g_bool_AddFlagContact = True
            Session("BoolAddFlagContact") = g_bool_AddFlagContact
            g_bool_UpdateFlagContact = False
            Session("BoolUpdateFlagContact") = g_bool_UpdateFlagContact
            g_bool_DeleteContactFlag = False
            Session("DeleteContactFlag") = g_bool_DeleteContactFlag

            'EnableSaveCancelButtons()

            Dim msg3 As String = "<script language='javascript'>" & _
                        "window.open('AddContactWebForm.aspx?UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
                        "'width=810, height=570, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
                        "</script>"

            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", msg3)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonResoAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonResoAdd.Click
        Dim l_datarow As DataRow
        Dim i As Integer
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonResoAdd", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'EnableSaveCancelButtons()
            g_DataSetYMCAResolution = DirectCast(Session("YMCA Resolution"), DataSet) 'Session("YMCAResolution Schema"), DataSet)
            If HelperFunctions.isNonEmpty(g_DataSetYMCAResolution) Then
                For i = 0 To g_DataSetYMCAResolution.Tables(0).Rows.Count - 1
                    l_datarow = g_DataSetYMCAResolution.Tables(0).Rows(i)
                    If l_datarow.Item("Term. Date").GetType.ToString = "System.DBNull" Then
                        Session("TermDateIsDBNull") = True
                        Session("SingleResolution") = True
                        Session("ResoPopupEffDate") = CType(l_datarow.Item("Eff. Date"), Date)

                    End If
                Next

                If CType(Session("TermDateIsDBNull"), Boolean) = True Then
                    Session("TermDateIsDBNull") = False
                Else
                    Dim ArrTermDate(g_DataSetYMCAResolution.Tables(0).Rows.Count) As Date
                    'Finding the latest date from Term Date Dataset and putting in the session for Effective date in the Resolution popup at add item of resolution
                    For i = 0 To g_DataSetYMCAResolution.Tables(0).Rows.Count - 1
                        l_datarow = g_DataSetYMCAResolution.Tables(0).Rows(i)
                        If l_datarow.Item("Term. Date").GetType.ToString = "System.DBNull" Then
                            ArrTermDate(i) = "01/01/1900"
                        Else
                            ArrTermDate(i) = l_datarow.Item("Term. Date")
                        End If
                    Next
                    Array.Sort(ArrTermDate)
                    'Putting into th session for populating effective date of Resolution popup at the add item click
                    Session("ResoPopupEffDate") = ArrTermDate(g_DataSetYMCAResolution.Tables(0).Rows.Count)
                End If
            End If
            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Dim Pop1 As String = "<script language='javascript'>" & _
                            "window.open('AddResolutionWebForm.aspx?UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
                            "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
                        "</script>"
                Page.RegisterStartupScript("PopupScript2", Pop1)
            End If
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonBankInfoAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBankInfoAdd.Click

        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonBankInfoAdd", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            g_bool_AddFlagBankInfo = True
            Session("BoolAddFlagBankInfo") = g_bool_AddFlagBankInfo
            g_bool_UpdateFlagBankInfo = False
            Session("BoolUpdateFlagBankInfo") = g_bool_UpdateFlagBankInfo
            Session("BankInfoUpdate") = g_bool_UpdateFlagBankInfo

            Me.ButtonBankInfoAdd.Enabled = True
            'EnableSaveCancelButtons()

            Dim pop5 As String = "<script language='javascript'>" & _
                        "window.open('UpdateBankInformationWebForm.aspx?UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
                        "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
                        "</script>"

            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", pop5)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Priya March 02 2010 YRS 5.0-1018 : Security on Notes - Add Item button
    'Private Sub ButtonNotesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNotesAdd.Click
    Private Sub ButtonYMCANotesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonYMCANotesAdd.Click
        Try
            'EnableSaveCancelButtons()
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            'Priya march 02 2010 :YRS 5.0-1018:Security on Notes - Add Item button
            ' Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonNotesAdd", Convert.ToInt32(Session("LoggedUserKey")))
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonYMCANotesAdd", Convert.ToInt32(MyBase.Session("LoggedUserKey")))
            'End YRS 5.0-1018
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            'Start: Bala: 01/12/2016: YRS-AT-1718: Add notes
            'Dim pop6 As String = "<script language='javascript'>" & _
            '        "window.open('AddNotes.aspx?UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
            '        "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
            '       "</script>"
            Session("NotesEntityID") = Session("GuiUniqueId")
            Dim pop6 As String = "<script language='javascript'>" & _
                      "window.open('UpdateNotes.aspx?UniqueSessionID=" + uniqueSessionId + "', 'CustomPopUp', " & _
                      "'width=800, height=400, menubar=no, resizable=No,top=80,left=120, scrollbars=yes')" & _
                      "</script>"
            'End: Bala: 01/12/2016: YRS-AT-1718: Add notes
            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", pop6)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -start
    'Private Sub ButtonNotesView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNotesView.Click
    '    Dim msg1 As String
    '    msg1 = ""

    '    Try
    '        If Not DataGridYMCANotesHistory.SelectedItem Is Nothing Then
    '            Dim noteId As String = String.Empty
    '            noteId = DataGridYMCANotesHistory.SelectedItem.Cells(0).Text.Trim()

    '            'DisableSaveCancelButtons()

    '            msg1 = msg1 & "<Script Language='JavaScript'>"
    '            msg1 = msg1 & "window.open('AddNotes.aspx?NoteId=" & noteId & "','map');"
    '            msg1 = msg1 & "</script>"

    '            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
    '                Page.RegisterStartupScript("PopupScript2", msg1)
    '            End If
    '        Else
    '            If DataGridYMCANotesHistory.Items.Count > 0 Then
    '                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '                Exit Sub
    '            Else
    '                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "There are no items to view.", MessageBoxButtons.OK)
    '                Exit Sub
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -End
    Private Sub ButtonAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click

        Me.TextBoxBoxYMCANo.Attributes.Add("onblur", "javascript:FormatYMCANoGeneral();")
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAdd", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'Me.LabelHdr.Text = "" 'Added by priya 16-April-2009 Phase-V
            LabelCurrentStatus.Text = "" 'Added by Swopna 19 Jun08
            'Added by Anudeep for YRS:5.0-1621 on 2012.09.26
            ButtonAdd.Enabled = False

            Clear_session()
            Session("GuiUniqueId") = Nothing

            LoadData(String.Empty)
            PopulateAllTabs()
            EnableAllTabs()
            Me.MultiPageYMCA.SelectedIndex = 1
            Me.YMCATabStrip.SelectedIndex = 1
            DataGridYMCA.SelectedIndex = -1
            TextBoxBoxYMCANo.ReadOnly = False
            TextBoxStateTaxId.ReadOnly = False

            'EnableSaveCancelButtons()

            Page_Mode = "ADD"

            TextBoxFedTaxId.Text = String.Empty
            TextBoxStateTaxId.Text = String.Empty

            Dim drYMCA As DataRow
            Dim dsYMCAGeneral As DataSet = Session("YMCA General")
            If Not dsYMCAGeneral Is Nothing AndAlso dsYMCAGeneral.Tables.Count > 0 Then
                drYMCA = dsYMCAGeneral.Tables(0).NewRow()
                drYMCA("guiUniqueId") = Guid.NewGuid()
                Session("guiUniqueId") = drYMCA("guiUniqueId")
                dsYMCAGeneral.Tables(0).Rows.Add(drYMCA)
            End If
            'Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security
            EnableDisableControls(True)
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Try

            If Me.IsProceedWaived = False Then 'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
                'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
                Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(MyBase.Session("LoggedUserKey")))

                'Priya:9-April-2010:YRS 5.0-1050-Error occured when terminating a YMCAs participation
                Page.Validate()
                'End YRS 5.0-1050
                'Added by imran on 2009.01.07 For Application displaying both messages.
                LoadWTSMRGrid(hdnSelectedTab.Value) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added to load selected tab when saving information in W/T/S/M tab at the time when validation error is there in page
                If Not Page.IsValid Then
                    Exit Sub
                End If
                'Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security
                'Me.ButtonEditFedTaxId.Enabled = True
                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'End : YRS 5.0-940
                'START Below lines are Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921

                If (checkboxPriority.Checked = True) Then
                    LabelPriorityHdr.Visible = True
                Else
                    LabelPriorityHdr.Visible = False
                End If
                'END Above lines are Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921
                'Perform Generic YMCA validations
                If TextBoxBoxYMCANo.Text.Trim = String.Empty Then
                    MessageBox.Show(PlaceHolder1, "", "YMCA number cannot be blank.", MessageBoxButtons.OK)
                    Exit Sub
                End If
                If Me.TextBoxYMCAName.Text = String.Empty Then
                    Me.LabelYMCANameErrMsg.Visible = True
                    Me.YMCATabStrip.SelectedIndex = 1 : Me.MultiPageYMCA.SelectedIndex = 1
                    Exit Sub
                End If
                If Me.AddressWebUserControlYMCA.Address1 = String.Empty Then
                    Me.LabelAddressErrMsg.Visible = True
                    Me.YMCATabStrip.SelectedIndex = 1 : Me.MultiPageYMCA.SelectedIndex = 1
                    Exit Sub
                End If
                If Me.DropDownHubType.SelectedValue = String.Empty Then
                    Me.LabelHubTypeErrMsg.Visible = True
                    Me.YMCATabStrip.SelectedIndex = 1 : Me.MultiPageYMCA.SelectedIndex = 1
                    Exit Sub
                End If
                If Me.DropDownHubType.SelectedValue = "B" Then
                    Dim ds As DataSet = Session("YMCA General")
                    Dim drYMCA As DataRow = ds.Tables(0).Rows(0)
                    If drYMCA.IsNull("MetroGuiUniqueId") OrElse drYMCA("MetroGuiUniqueId").ToString() = String.Empty Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "This YMCA has been marked as a Branch but the Metro YMCA has not been selected", MessageBoxButtons.OK, True)
                        Me.YMCATabStrip.SelectedIndex = 1 : Me.MultiPageYMCA.SelectedIndex = 1
                        Exit Sub
                    End If
                End If
                g_DataSetYMCAResolution = DirectCast(Session("YMCA Resolution"), DataSet)
                'checking wheather resolution is entered or not - This check is to be performed only for new YMCA
                If NewYMCAIsBeingAdded() Then
                    If HelperFunctions.isEmpty(g_DataSetYMCAResolution) Then
                        Me.LabelResolutionErrMsg.Visible = True
                        ResoFlagNotEntered = True
                        Me.YMCATabStrip.SelectedIndex = 1 : Me.MultiPageYMCA.SelectedIndex = 1
                        Exit Sub
                    End If
                End If
                'Perform validations for YMCA Withdraw/Terminate/Merge/Reactivate

                'Start --Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added condition to check which activity needs to be saved based on property value and loading respective W/T/S/M/ tab through LoadWSTMRGrid method
                If Me.WTSM_Operation = "#tabContent_Withdraw" Then

                    If IsValidYMCAWithdrawal() = False Then
                        LoadWTSMRGrid(hdnSelectedTab.Value)
                        Exit Sub
                    End If
                    Session("Withdrawn") = True
                End If
                'Start -- Manthan Rajguru| YRS-AT-2334 & 1686|
                If Me.WTSM_Operation = "#tabContent_Suspend" Then

                    If IsValidYMCASuspend() = False Then
                        LoadWTSMRGrid(hdnSelectedTab.Value)
                        Exit Sub
                    End If
                    Session("Suspend") = True
                End If

                If Me.WTSM_Operation = "#tabContent_Terminate" Then

                    If IsValidYMCATermination() = False Then
                        LoadWTSMRGrid(hdnSelectedTab.Value)
                        Exit Sub
                    End If
                    Session("TerminateYMCA") = True

                End If
                If Me.WTSM_Operation = "#tabContent_Reactivate" Then

                    If IsValidYMCAReactivation() = False Then
                        LoadWTSMRGrid(hdnSelectedTab.Value)
                        Exit Sub
                    End If
                    Session("ReactivateYMCA") = True

                End If
                If Me.WTSM_Operation = "#tabContent_Merge" Then

                    If IsValidYMCAMerge() = False Then
                        LoadWTSMRGrid(hdnSelectedTab.Value)
                        Exit Sub
                    End If
                    Session("MergeYMCA") = True

                End If
                'End --Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added condition to check which activity needs to be saved based on property value and loading respective W/T/S/M/ tab through LoadWSTMRGrid method
            End If  'END: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)

            'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
            Dim dtWaivedParticipants As New DataTable
            If Me.ShowWaivedParticipantsList = True And Me.IsProceedWaived = False Then
                dtWaivedParticipants = SSRSDataset.WaivedParticipants(Session("guiUniqueId"))
                If HelperFunctions.isNonEmpty(dtWaivedParticipants) Then
                    LoadWaivedParticipants(dtWaivedParticipants)
                    ClientScript.RegisterStartupScript(Me.[GetType](), "OpenWaivedParticipantslist", "OpenWaivedParticipantslistDialog();", True)
                    Exit Sub
                End If
            End If
            'END: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)

            g_DataSetYMCAGeneral = DirectCast(Session("YMCA General"), DataSet)

            SaveGeneralTabInformation()
            SaveResolutionInformation(Session("guiUniqueId"))
            SaveYMCAOfficerInformation(Session("guiUniqueId"))
            SaveYMCAContactInformation(Session("guiUniqueId"))
            SaveYMCANotes(Session("guiUniqueId"))
            SaveYMCABankInformation(Session("guiUniqueId"))

            'Swopna Phase IV changes 8 Apr,2008***start
            ''''''''''''''''''''''''''''
            'Withdraw/Terminate/Reactivate/Merge
            ''''''''''''''''''''''''''''
            'Populate resolution tab,W/T/M tab with values after modification

            ''On addition of new active resolution by an active YMCA,insert new record and update termination date of existing resolution.  
            If Session("TerminateYMCA") = Nothing And Session("MergeYMCA") = Nothing And Session("ReactivateYMCA") = Nothing And Session("Withdrawn") = Nothing Then
                If (Not Page_Mode = "ADD") And CType(Session("UpdateActiveResolution"), Boolean) = True Then
                    Session("UpdateActiveResolution") = Nothing
                    'Swopna BT-430-----start
                    Session("SingleResolution") = Nothing
                    'Swopna BT-430-----end
                    YMCARET.YmcaBusinessObject.YMCABOClass.InsertYMCAResolution(g_DataSetYMCAResolution)
                End If
            End If

            'Update on Withdrawal of YMCA 
            'Start --Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added method to load respective W/T/S/M tab through LoadWSTMRGrid method after saving process
            If Not Session("Withdrawn") = Nothing AndAlso Session("Withdrawn") = True Then
                If PerformYMCAWithdrawal() = False Then
                    LoadWTSMRGrid(hdnSelectedTab.Value)
                    Exit Sub
                Else
                    LoadWTSMRGrid(hdnSelectedTab.Value)
                End If
                Me.WTSM_Operation = ""
                'End If

                'Start -- Manthan Rajguru | 2016.02.03 | Added for performing YMCA Suspend               
            ElseIf Not Session("Suspend") = Nothing AndAlso Session("Suspend") = True Then
                If PerformYMCASuspend() = False Then
                    LoadWTSMRGrid(hdnSelectedTab.Value)
                    Exit Sub
                Else
                    LoadWTSMRGrid(hdnSelectedTab.Value)
                End If
                Me.WTSM_Operation = ""
                'End If
                'Start -- Manthan Rajguru | 2016.02.03 | Added for performing YMCA Suspend               

                ''Update On Termination Of Ymca
            ElseIf Not Session("TerminateYMCA") = Nothing AndAlso Session("TerminateYMCA") = True Then
                If PerformYMCATermination() = False Then
                    LoadWTSMRGrid(hdnSelectedTab.Value)
                    Exit Sub
                Else
                    LoadWTSMRGrid(hdnSelectedTab.Value)
                End If
                Me.WTSM_Operation = ""
                'End If

            ElseIf Not Session("ReactivateYMCA") = Nothing Then
                If Session("ReactivateYMCA") = True Then

                    If PerformYMCAReactivation() = False Then
                        LoadWTSMRGrid(hdnSelectedTab.Value)
                        Exit Sub
                    Else
                        LoadWTSMRGrid(hdnSelectedTab.Value)
                    End If
                    Me.WTSM_Operation = ""
                    'Start --Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added method to load respective W/T/S/M tab through LoadWSTMRGrid method after saving process

                    'Start --Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
                    'Else
                    'LabelReactivate.Enabled = False 
                    'TextboxReactivate.Enabled = False 
                    'PopcalendarReactivate.Enabled = False
                    'Swopna 8 May,2008-In order to restrict user to modify withdrawal/termination date
                    'ButtonWithdraw.Enabled = False 
                    'ButtonTerminate.Enabled = False 
                    'End --Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
                    'Swopna 8 May,2008
                End If
                'End If

            ElseIf Not Session("MergeYMCA") Is Nothing AndAlso Session("MergeYMCA") = True Then
                If PerformYMCAMerge() = False Then
                    LoadWTSMRGrid(hdnSelectedTab.Value)
                    Exit Sub
                End If
                Me.WTSM_Operation = ""
            End If

            Me.DataGridYMCAOfficer.SelectedIndex = -1
            Me.DataGridYMCAContact.SelectedIndex = -1
            Me.DataGridYMCABankInfo.SelectedIndex = -1
            Me.DataGridYMCANotesHistory.SelectedIndex = -1
            Me.DataGridYMCAResolutions.SelectedIndex = -1
            PopulateAllTabs()
            'Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security
            'Me.TextBoxFedTaxId.Enabled = False
            'DisableSaveCancelButtons()
            Page_Mode = String.Empty
            'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
            Me.ShowWaivedParticipantsList = False
            Me.IsProceedWaived = False
            'END: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "false"
            'Start - Manthan Rajguru | 2016.02.16 | YRS-AT-2334 & 1686 | Setting session value to nothing
            Session("YMCATermination") = Nothing
            Session("YMCAWithdrawn") = Nothing
            Session("YMCASuspend") = Nothing
            'End - Manthan Rajguru | 2016.02.16 | YRS-AT-2334 & 1686 | Setting session value to nothing
        Catch secEx As System.Security.SecurityException
            If MessageBoxFlag = False Then
                Response.Redirect("ErrorPageForm.aspx", False)
            End If
        Catch ex As SqlException
            If ex.Number = 60010 Then
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Duplicate YMCA No.?", MessageBoxButtons.OK)
                MessageBoxFlag = True
            End If

            'START: SB | 2018.05.23 | YRS-AT-2818 | Show below error when adding more than one LPA contacts in contact tab.
            If ex.Number = 60012 Then
                MessageBox.Show(PlaceHolder1, "LPA Already Exist", "A Local Plan Admin already exists for this YMCA. Delete or change the Type of the existing contact before adding a new one.", MessageBoxButtons.Stop)
                MessageBoxFlag = True
            End If
            'END: SB | 2018.05.23 | YRS-AT-2818 | Show below error when adding more than one LPA contacts in contact tab.

            If MessageBoxFlag = False Then
                Dim l_String_Exception_Message As String
                l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
                Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
            End If
        Catch ex As Exception
            If MessageBoxFlag = False Then
                Dim l_String_Exception_Message As String
                l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
                Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            End If
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "false"
            Page_Mode = String.Empty

            Me.MultiPageYMCA.SelectedIndex = 0
            Me.YMCATabStrip.SelectedIndex = 0

            If HelperFunctions.isEmpty(YMCASearchList) Then
                DisableAllTabsExcept(0)
            End If

            LabelResoNotEntered.Text = String.Empty
            'LabelWithdrawDate.Enabled = False 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
            'Start : Commented and added by Dilip : 09-July-09 : BT - 838
            'Start -- Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
            'DropdownWithdrawDate.Items.Clear()
            'DropdownWithdrawDate.Items.Add(New ListItem("---Select---", ""))
            'DropdownWithdrawDate.SelectedValue = String.Empty
            'DropdownWithdrawDate.Enabled = False
            'End -- Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
            'END : Commented and added by Dilip : 09-July-09 : BT - 838
            LabelYerdiAccess_W.Enabled = False
            'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
            'DropdownYerdiAccess_W.SelectedValue = ""
            'DropdownYerdiAccess_W.Enabled = False
            'End YRS 5.0-873
            'Added by Anudeep for YRS:5.0-1621 on 2012.09.26
            ButtonAdd.Enabled = True

            'LabelTerminationDate.Enabled = False 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
            '14-July-09
            'Start -- Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
            'DropdownlistTermination.Items.Clear()
            'DropdownlistTermination.Items.Add(New ListItem("---Select---", ""))
            'DropdownlistTermination.SelectedValue = String.Empty
            'DropdownlistTermination.Enabled = False
            'LabelYerdiAccess_T.Enabled = False 
            'DropdownYerdiAccess_T.SelectedValue = 0 
            'End -- Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
            'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
            'DropdownYerdiAccess_T.Enabled = False
            'End YRS 5.0-873

            'Start -- Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
            'LabelMetroDetails.Enabled = False 
            'TextboxMetroDetails.Text = String.Empty 
            'TextboxMetroDetails.Enabled = False 
            'DropDownYMCANo.Enabled = False 
            'LabelMergeDate.Enabled = False 
            'DropdownlistMergeDate.Enabled = False 

            'TextboxReactivate.Enabled = False 
            'LabelReactivate.Enabled = False 
            'TextboxReactivate.Enabled = False 
            'PopcalendarReactivate.Enabled = False
            'End -- Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code

            'Swopna 9 May,2008-------start 
            ClearSessionForWTM()
            'Clear_session()
            'Priya 14-April-2009 PhaseV changes made to clease session after click on cancel button.
            Session("ResoPopupEffDate") = Nothing
            'End Priya
            'PopulateYMCAList()
            Dim strUniqueId As String
            If DataGridYMCA.SelectedItem Is Nothing Then
                strUniqueId = String.Empty
            Else
                strUniqueId = DataGridYMCA.SelectedItem.Cells(1).Text
            End If
            LoadData(strUniqueId)
            Session("guiUniqueId") = strUniqueId
            PopulateAllTabs()
            'PopulateLabelCurrentStatus() 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code

            'Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security
            EnableDisableControls(False)
            'DisableSaveCancelButtons()
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ClearSessionForWTM()
        Session("Withdrawn") = Nothing
        Session("TerminateYMCA") = Nothing
        Session("MergeYMCA") = Nothing
        Session("ReactivateYMCA") = Nothing
        Session("FirstOfMonth_W") = Nothing
        Session("FirstOfMonth_T") = Nothing
        Session("Error") = Nothing
        Session("ProcessProceed") = Nothing
        Session("TermProcessProceed") = Nothing
        'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
        Me.ShowWaivedParticipantsList = False
        Me.IsProceedWaived = False
        'END: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    End Sub

    'Added by Dilip : 15-July-09 : To set the dropdownlist
    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the logic to get value in dropdown
    Private Sub setDropdown(ByRef drpdwn As DropDownList, ByVal val As Object) 'ByVal actionDataRow As DataRow, ByVal actiontype As String)
        If val.GetType() = GetType(String) Then
            drpdwn.Items.Clear()
            drpdwn.Items.Add(New ListItem("---Select---", ""))
            If val <> String.Empty Then
                drpdwn.Items.Add(Convert.ToDateTime(val).ToString("MM/dd/yyyy"))
            End If
            drpdwn.SelectedIndex = drpdwn.Items.Count - 1
        ElseIf val.GetType() = GetType(Boolean) Then
            drpdwn.SelectedValue = IIf(val = True, 1, 0)
        End If
        'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the logic to get value in dropdown
    End Sub
    ' Added by Dilip : 08-July-09 : To populate the dates.
    Private Sub Populatedates(ByVal drpdwn As DropDownList)
        Dim l_dataset_dsPayrolldate As DataSet
        Dim strYMCAID As String = Session("guiUniqueId")
        'If Not strStage = "Merged YMCA" And Not strStage = "Terminated YMCA" And Not strStage = "Suspended YMCA" Then

        If ViewState("Payrolldates") Is Nothing Then
            l_dataset_dsPayrolldate = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAPayrolldate(strYMCAID)
            ViewState("Payrolldates") = l_dataset_dsPayrolldate
        End If

        l_dataset_dsPayrolldate = ViewState("Payrolldates")

        If (Not l_dataset_dsPayrolldate Is Nothing) AndAlso l_dataset_dsPayrolldate.Tables.Count > 0 AndAlso l_dataset_dsPayrolldate.Tables(0).Rows.Count > 0 Then
            drpdwn.Items.Clear()
            drpdwn.DataSource = l_dataset_dsPayrolldate
            drpdwn.DataMember = "YMCAPayrolldate"
            drpdwn.DataTextField = "Payrolldate"
            drpdwn.DataValueField = "Payrolldate"
            drpdwn.DataBind()
            'Else
            Dim li As ListItem = New ListItem("---Select---", "")
            drpdwn.Items.Insert(0, li)
            drpdwn.SelectedIndex = 0
        End If
        ' End If
    End Sub

    Private Sub DataGridYMCA_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridYMCA.ItemDataBound
        Try
            e.Item.Cells(1).Visible = False
            e.Item.Cells(2).Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    'Commented By Dilip : 03-July-09 : Issue ID : 839 : Logic is changed and handled through .aspx page itself.
    'Private Sub DataGridYMCAOfficer_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridYMCAOfficer.ItemDataBound
    '    Try
    '        If e.Item.Cells.Count > 10 Then
    '            If e.Item.Cells(11).Text.Trim <> "" AndAlso e.Item.Cells(11).Text.Trim <> "&nbsp;" Then
    '                e.Item.Cells(11).Text = String.Format("{0:MM/dd/yyyy}", Date.Parse(e.Item.Cells(11).Text))
    '            End If
    '        End If
    '        e.Item.Cells(3).Visible = False
    '        e.Item.Cells(4).Visible = False
    '        'e.Item.Cells(5).Visible = False
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    Private Sub DropDownHubType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropDownHubType.SelectedIndexChanged
        Try
            If Me.DropDownHubType.SelectedValue = "B" Then
                Me.TextBoxMetroName.Enabled = True
                Me.ButtonMetro.Enabled = True
            Else
                Me.TextBoxMetroName.Text = String.Empty
                Me.TextBoxMetroName.Enabled = False
                Me.ButtonMetro.Enabled = False
                Dim ds As DataSet = Session("YMCA General")
                Dim drYMCA As DataRow = ds.Tables(0).Rows(0)
                drYMCA("MetroGuiUniqueId") = DBNull.Value
                drYMCA("chvYmcaMetroName") = DBNull.Value
            End If

            If Me.DropDownHubType.SelectedValue = "M" Then
                Me.YMCATabStrip.Items(4).Enabled = True
            Else
                Me.YMCATabStrip.Items(4).Enabled = False
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -Start
    Private Sub DataGridYMCANotesHistory_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridYMCANotesHistory.ItemCommand
        Try
            If e.CommandName = "View" Then

                If Not e.Item Is Nothing Then
                    Dim msg1 As String
                    msg1 = ""
                    'Start: Bala: 01/12/2016: YRS-AT-1718
                    Dim noteId As String = String.Empty
                    noteId = e.Item.Cells(0).Text.Trim()
                    'START | SR | 2016.02.16 | YRS-AT-1718 Display full notes on view click
                    'Session("Note") = e.Item.Cells(4).Text.Trim()
                    If (HelperFunctions.isEmpty(g_DataSetYMCANotes)) Then
                        g_DataSetYMCANotes = DirectCast(Session("YMCA Notes"), DataSet)
                    End If
                    Session("Note") = Convert.ToString(g_DataSetYMCANotes.Tables(0).Select("guiUniqueId ='" & noteId.Trim() & "'")(0)("First Line of Notes"))
                    'END | SR | 2016.02.16 | YRS-AT-1718 Display full notes on view click
                    Dim l_checkbox As CheckBox
                    l_checkbox = e.Item.Cells(5).FindControl("CheckBoxImportant")
                    Session("BitImportant") = IIf(l_checkbox.Checked, True, False)
                    'End: Bala: 01/12/2016: YRS-AT-1718
                    'DisableSaveCancelButtons()
                    'Start: Bala: 01/12/2016: YRS-AT-1718
                    'msg1 = msg1 & "<Script Language='JavaScript'>"
                    ''msg1 = msg1 & "window.open('AddNotes.aspx?NoteId=" & noteId & "','map');"
                    'msg1 = msg1 & "window.open('AddNotes.aspx?NoteId=" & noteId + "&UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
                    '    "'width=750, height=400, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ');"
                    'msg1 = msg1 & "</script>"
                    msg1 = "<Script Language='JavaScript'>window.open('UpdateNotes.aspx?UniqueSessionID=" + uniqueSessionId + "&UniqueID=" + noteId + "','CustomPopUp', " & _
                        "'width=750, height=400, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ');</script>"
                    'End: Bala: 01/12/2016: YRS-AT-1718
                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", msg1)
                    End If
                End If
                'Start: Bala: 01/12/2016: YRS-AT-1718
            ElseIf e.CommandName.ToLower = "delete" Then
                DeleteYMCANotes(e.Item.Cells)
                'End: Bala: 01/12/2016: YRS-AT-1718
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -End
    'Start: Bala: 01/12/2016: YRS-AT-1718: Delete Notes
    Public Sub DeleteYMCANotes(ByVal DatagridRow As TableCellCollection)
        Try
            Session("Flag") = "DeleteNotes"
            If DatagridRow(3).Text.ToString.ToLower() = Convert.ToString(MyBase.Session("LoginId")).ToLower() Then 'Manthan Rajguru | 2016.04.05 | YRS-AT-1718 | Converting login ID to lower case from both the side to avoid any mismatches
                Session("NotesToDelete") = DatagridRow(0).Text.ToString
                'Start: Bala: 03.08.2016: YRS-AT-1718: Confimation message box.
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you want to delete this notes.", MessageBoxButtons.YesNo)
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you want to delete the following note?<br /><br />Date:" & DatagridRow(2).Text.ToString & "<br />Note:" & DatagridRow(4).Text.ToString, MessageBoxButtons.YesNo)
                'End: Bala: 03.08.2016: YRS-AT-1718: Confimation message box.
            End If
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'End: Bala: 01/12/2016: YRS-AT-1718: Delete Notes
    'Commented By Dilip : 06-July-09 : Issue ID : 839 : Logic is changed and handled through .aspx page itself.
    'Private Sub DataGridYMCAContact_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridYMCAContact.ItemDataBound
    '    Try
    '        e.Item.Cells(3).Visible = False
    '        e.Item.Cells(4).Visible = False
    '        e.Item.Cells(11).Visible = False
    '        If e.Item.Cells(10).Text.Trim <> "" AndAlso e.Item.Cells(10).Text.Trim <> "&nbsp;" Then
    '            e.Item.Cells(10).Text = String.Format("{0:MM/dd/yyyy}", Date.Parse(e.Item.Cells(10).Text))
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    Private Sub DataGridYMCANotesHistory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridYMCANotesHistory.ItemDataBound
        Try
            ''START: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.
            If e.Item.Cells(4).Text.Length > 50 Then
                e.Item.Cells(4).Text = e.Item.Cells(4).Text.Substring(0, 50)
            End If
            ''END: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.

            Dim l_checkbox As New CheckBox
            Dim l_linkbutton As New LinkButton 'Bala: 01/12/2016: YRS-AT-1718: Create link button control for disabling / enabling accordingly
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                l_checkbox = e.Item.FindControl("CheckBoxImportant")
                If e.Item.Cells(2).Text.Trim = MyBase.Session("LoginId") Or NotesGroupUser = True Then
                    l_checkbox.Enabled = True
                End If
                'Start: Bala: 01/12/2016: YRS-AT-1718: Create link button control for disabling / enabling accordingly
                l_linkbutton = e.Item.FindControl("DeleteNotes")
                If Not e.Item.Cells(3).Text.Trim.ToLower() = Convert.ToString(MyBase.Session("LoginId")).ToLower() Then 'Manthan Rajguru | 2016.04.05 | YRS-AT-1718 | Converting login ID to lower case from both the side to avoid any mismatches
                    l_linkbutton.Enabled = False
                End If
                'End: Bala: 01/12/2016: YRS-AT-1718: Create link button control for disabling / enabling accordingly
            End If
            CheckReadOnlyMode() 'Shilpa N | 03/20/2019 | Calling the method to check read only access.
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try
            Clear_session()
            If Me.MultiPageYMCA.SelectedIndex = 0 Then
                Session("List_Index") = Nothing
                Session("l_string_Ymcaname") = Nothing
                Response.Redirect("MainWebForm.aspx", False)
            Else
                Me.YMCATabStrip.SelectedIndex = 0
                Me.MultiPageYMCA.SelectedIndex = 0
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 
                ButtonAdd.Enabled = True

                If Not (Me.TextBoxYMCANo.Text = "" And Me.TextBoxName.Text = "" And Me.TextBoxCity.Text = "" And Me.TextBoxState.Text = "") Then
                    PopulateYMCAList()
                    If Not Session("List_Index") Is Nothing Then
                        Me.DataGridYMCA.SelectedIndex = Session("List_Index")
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridYMCAContact_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridYMCAContact.ItemCommand
        Try
            If e.CommandName = "Select" Then
                Session("ContactUpdate") = True
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonContactUpdate", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If

                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -start
                If e.Item.ItemIndex <> -1 Then

                    'DisableSaveCancelButtons()

                    Dim contactid As String = String.Empty
                    contactid = e.Item.Cells(0).Text.Trim()


                    Dim msg5 As String = "<script language='javascript'>" & _
                                "window.open('AddContactWebForm.aspx?ContactId=" + contactid + "&UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
                                "'width=810, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
                                "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", msg5)
                    End If
                End If
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -End
            ElseIf e.CommandName = "DeleteSelect" Then
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonContactDelete", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                Dim strTemp As String
                strTemp = e.Item.Cells(0).Text.Replace("&nbsp;", "")

                Session("ContactUniqueId") = strTemp
                Session("ContactIndex") = e.Item.ItemIndex

                g_bool_DeleteContactFlag = True
                Session("DeleteContactFlag") = g_bool_DeleteContactFlag
                g_bool_UpdateFlagContact = False
                Session("BoolUpdateFlagContact") = g_bool_UpdateFlagContact
                g_bool_AddFlagContact = False
                Session("BoolAddFlagContact") = g_bool_AddFlagContact

                'DisableSaveCancelButtons()
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you sure you want to delete this Contact ?", MessageBoxButtons.YesNo)
            End If
            'Shashi Shekhar:18 Feb 2011: ForBT-670,YRS 5.0-1202 : hyperlink from employment record to ymca maintenance
            If e.CommandName.ToLower = "fundno" Then
                Dim l_FundNo As String = e.CommandArgument.ToString().Trim

                ' Dim l_urlLanding As String = System.Configuration.ConfigurationSettings.AppSettings("YRSLandingPage") + "YRSlandingpage.aspx?PageType=personmaintenance&DataType=FundNo&Value=" + l_FundNo
                Dim l_urlLanding As String = System.Configuration.ConfigurationSettings.AppSettings("YRSLandingPage") + "?PageType=personmaintenance&DataType=FundNo&Value=" + l_FundNo
                Response.Redirect(l_urlLanding, False)

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub DataGridYMCAOfficer_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridYMCAOfficer.ItemCommand
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check

            'End : YRS 5.0-940
            If e.CommandName = "Select" Then
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -start
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonOfficersUpdate", Convert.ToInt32(MyBase.Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                Session("Select") = True
                If e.Item.ItemIndex <> -1 Then
                    'DisableSaveCancelButtons()
                    Dim officerId As String = String.Empty
                    officerId = e.Item.Cells(0).Text.Trim()

                    Dim msg4 As String = "<script language='javascript'>" & _
                                        "window.open('AddOfficerWebForm.aspx?OfficerId= " + officerId + "&UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
                                        "'width=810, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
                                        "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", msg4)
                    End If
                End If
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -End
            ElseIf e.CommandName = "DeleteSelect" Then
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -start
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonOfficersDelete", Convert.ToInt32(MyBase.Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -End
                Dim strTemp As String
                strTemp = e.Item.Cells(0).Text.Replace("&nbsp;", "")
                Session("OfficerUniqueId") = strTemp

                g_bool_DeleteOfficerFlag = True
                Session("DeleteOfficerFlag") = g_bool_DeleteOfficerFlag
                g_bool_UpdateFlagOfficer = False
                Session("BoolUpdateFlagOfficer") = g_bool_UpdateFlagOfficer
                'DisableSaveCancelButtons()
                MessageBox.Show(PlaceHolder1, "Please Confirm", "Are you sure you want to delete this Officer?", MessageBoxButtons.YesNo)
            End If
            'Shashi Shekhar:18 Feb 2011: ForBT-670,YRS 5.0-1202 : hyperlink from employment record to ymca maintenance
            If e.CommandName.ToLower = "fundno" Then
                Dim l_FundNo As String = e.CommandArgument.ToString().Trim
                Dim l_urlLanding As String = System.Configuration.ConfigurationSettings.AppSettings("YRSLandingPage") + "?PageType=personmaintenance&DataType=FundNo&Value=" + l_FundNo
                Response.Redirect(l_urlLanding, False)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -start
    'Private Sub ButtonOfficersUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOfficersUpdate.Click
    '    Dim l_string_Temp As String
    '    Try
    '        'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
    '        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonOfficersUpdate", Convert.ToInt32(Session("LoggedUserKey")))
    '        If Not checkSecurity.Equals("True") Then
    '            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
    '            Exit Sub
    '        End If
    '        'End : YRS 5.0-940

    '        If Session("Select") = True Then

    '            Dim paramIndex As Integer
    '            If Me.DataGridYMCAOfficer.SelectedIndex <> -1 Then
    '                'DisableSaveCancelButtons()
    '                Dim officerId As String = String.Empty
    '                officerId = DataGridYMCAOfficer.SelectedItem.Cells(0).Text.Trim()

    '                Dim msg4 As String = "<script language='javascript'>" & _
    '                                    "window.open('AddOfficerWebForm.aspx?OfficerId= " & officerId & "','CustomPopUp', " & _
    '                                    "'width=810, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
    '                                    "</script>"

    '                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
    '                    Page.RegisterStartupScript("PopupScript2", msg4)
    '                End If
    '            Else
    '                If DataGridYMCAOfficer.Items.Count > 0 Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '                    Exit Sub
    '                Else
    '                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "There are no items to update.", MessageBoxButtons.OK)
    '                    Exit Sub
    '                End If
    '            End If
    '        Else
    '            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '            Exit Sub
    '        End If
    '    Catch ex As SqlException
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    'Private Sub ButtonContactUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonContactUpdate.Click
    '    Dim l_string_Temp As String
    '    If Session("ContactUpdate") = True Then
    '        Try
    '            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
    '            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonContactUpdate", Convert.ToInt32(Session("LoggedUserKey")))

    '            If Not checkSecurity.Equals("True") Then
    '                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
    '                Exit Sub
    '            End If
    '            'End : YRS 5.0-940

    '            If Me.DataGridYMCAContact.SelectedIndex <> -1 Then

    '                'DisableSaveCancelButtons()

    '                Dim contactid As String = String.Empty
    '                contactid = DataGridYMCAContact.SelectedItem.Cells(0).Text.Trim()


    '                Dim msg5 As String = "<script language='javascript'>" & _
    '                            "window.open('AddContactWebForm.aspx?ContactId= " & contactid & "','CustomPopUp', " & _
    '                            "'width=810, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
    '                            "</script>"

    '                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
    '                    Page.RegisterStartupScript("PopupScript2", msg5)
    '                End If
    '            Else
    '                If DataGridYMCAContact.Items.Count > 0 Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '                    Exit Sub
    '                Else
    '                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "There are no items to update.", MessageBoxButtons.OK)
    '                    Exit Sub
    '                End If
    '            End If

    '        Catch ex As SqlException
    '            Dim l_String_Exception_Message As String
    '            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
    '        Catch ex As Exception
    '            Dim l_String_Exception_Message As String
    '            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '        End Try
    '    Else
    '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '        Exit Sub
    '    End If
    'End Sub

    'Private Sub ButtonResoUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonResoUpdate.Click
    '    If Session("ResolutionUpdate") = True Then
    '        Try
    '            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
    '            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonResoUpdate", Convert.ToInt32(Session("LoggedUserKey")))

    '            If Not checkSecurity.Equals("True") Then
    '                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
    '                Exit Sub
    '            End If
    '            'End : YRS 5.0-940
    '            If Me.DataGridYMCAResolutions.SelectedIndex <> -1 Then

    '                Session("ResoPopupEffDate") = Nothing
    '                'DisableSaveCancelButtons()

    '                Dim msg7 As String = "<script language='javascript'>" & _
    '                                    "window.open('AddResolutionWebForm.aspx','CustomPopUp', " & _
    '                                    "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
    '                                    "</script>"

    '                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
    '                    Page.RegisterStartupScript("PopupScript2", msg7)
    '                End If
    '            Else
    '                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '                Exit Sub
    '            End If

    '        Catch ex As SqlException
    '            Dim l_String_Exception_Message As String
    '            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
    '        Catch ex As Exception
    '            Dim l_String_Exception_Message As String
    '            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '        End Try
    '    Else
    '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '        Exit Sub
    '    End If
    'End Sub
    'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -start
    Private Sub DataGridYMCAResolutions_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridYMCAResolutions.ItemCommand
        Try

            If e.CommandName = "Select" Then
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -start
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonResoUpdate", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If

                If e.Item.ItemIndex > -1 Then
                    If DataGridYMCAResolutions.Items(e.Item.ItemIndex).Cells(2).Text.ToUpper = "&NBSP;" Then
                        Me.ButtonResoUpdate.Enabled = True
                    Else
                        MessageBox.Show(PlaceHolder1, "Please Confirm", "Terminated Resolution records cannot be modified.", MessageBoxButtons.OK)
                        Exit Sub
                    End If
                End If

                Session("ResolutionUpdate") = True

                'End : YRS 5.0-940
                If e.Item.ItemIndex <> -1 Then

                    Session("ResoPopupEffDate") = Nothing
                    'DisableSaveCancelButtons()

                    Dim msg7 As String = "<script language='javascript'>" & _
                                        "window.open('AddResolutionWebForm.aspx?UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
                                        "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
                                        "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", msg7)
                    End If
                End If
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -End
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -start
    'Private Sub ButtonBankInfoUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonBankInfoUpdate.Click
    '    'If Session("BankInfoUpdate") = True Then
    '    Try
    '        'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
    '        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonBankInfoUpdate", Convert.ToInt32(Session("LoggedUserKey")))

    '        If Not checkSecurity.Equals("True") Then
    '            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
    '            Exit Sub
    '        End If
    '        'End : YRS 5.0-940

    '        If Me.DataGridYMCABankInfo.SelectedIndex <> -1 Then


    '            'DisableSaveCancelButtons()

    '            Dim bankid As String = String.Empty
    '            bankid = DataGridYMCABankInfo.SelectedItem.Cells(0).Text.Trim()

    '            Dim msg8 As String = "<script language='javascript'>" & _
    '                                "window.open('UpdateBankInformationWebForm.aspx?bankId=" & bankid & "','CustomPopUp', " & _
    '                                "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
    '                                "</script>"

    '            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
    '                Page.RegisterStartupScript("PopupScript2", msg8)
    '            End If
    '        Else
    '            If DataGridYMCABankInfo.Items.Count > 0 And DataGridYMCABankInfo.SelectedIndex = -1 Then
    '                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '                Exit Sub
    '            Else
    '                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "There are no items to update.", MessageBoxButtons.OK)
    '                Exit Sub
    '            End If
    '        End If

    '    Catch ex As SqlException
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    '    'Else
    '    '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '    '    Exit Sub
    '    'End If
    'End Sub
    'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -End
    Private Sub DataGridYMCABankInfo_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridYMCABankInfo.ItemCommand
        Try
            If e.CommandName = "Select" Then
                Session("BankInfoUpdate") = True
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -start
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonBankInfoUpdate", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'End : YRS 5.0-940

                If e.Item.ItemIndex <> -1 Then


                    'DisableSaveCancelButtons()

                    Dim bankid As String = String.Empty
                    bankid = e.Item.Cells(0).Text.Trim()

                    Dim msg8 As String = "<script language='javascript'>" & _
                                        "window.open('UpdateBankInformationWebForm.aspx?bankId=" + bankid + "&UniqueSessionID=" + uniqueSessionId + "','CustomPopUp', " & _
                                        "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars = yes ')" & _
                                        "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", msg8)
                    End If
                End If
                'Added by Anudeep for YRS:5.0-1621 on 2012.09.26 -End
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub LinkButtonIDM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonIDM.Click
        Try
            g_DataSetYMCAGeneral = Session("YMCA General")  'NP:2009.08.21:BT-916 Initialize the global variable from session
            Dim l_string_VignettePath1 As String

            If HelperFunctions.isNonEmpty(g_DataSetYMCAGeneral) Then
                '14-Nov-2008 Priya Change IDM path as per Ragesh mail as on 11/13/2008
                l_string_VignettePath1 = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_YMCA"), Convert.ToString(g_DataSetYMCAGeneral.Tables(0).Rows(0)("chrymcano")))
                'End 14-Nov-2008
            End If

            Dim popupScript As String = "<script language='javascript'>" & _
                                           "window.open('" + l_string_VignettePath1 + "', 'IDMPopUp', " & _
                                           "'width=1000, height=750, menubar=no, Resizable=Yes,top=0,left=0, scrollbars=yes')" & _
                                           "</script>"
            'NP:2009.08.21:BT-916 Changes to handle situations where url was not generated properly
            If popupScript <> String.Empty Then
                Page.RegisterStartupScript("IDMPopUpScript", popupScript)
            Else
                MessageBox.Show(PlaceHolder1, "IDM Link", "Unable to generate IDM link for current YMCA", MessageBoxButtons.Stop)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
#End Region     '"All Events"

#Region "Custom Methods"
    Private Sub PopulatePaymentMethod()
        Dim InsertRowPaymentMethod As DataRow
        Dim g_dataset_dsGeneralPaymentMethod As DataSet
        Try
            g_dataset_dsGeneralPaymentMethod = YMCARET.YmcaBusinessObject.YMCABOClass.LookUpGeneralPaymentMethod()
            InsertRowPaymentMethod = g_dataset_dsGeneralPaymentMethod.Tables(0).NewRow()
            InsertRowPaymentMethod.Item("chvcodevalue") = String.Empty
            InsertRowPaymentMethod.Item("chvshortdescription") = String.Empty
            If Not g_dataset_dsGeneralPaymentMethod Is Nothing Then
                g_dataset_dsGeneralPaymentMethod.Tables(0).Rows.InsertAt(InsertRowPaymentMethod, 0)
            End If
            Me.DropDownPaymentMethod.DataSource = g_dataset_dsGeneralPaymentMethod
            Me.DropDownPaymentMethod.DataMember = "General Payment Method"
            Me.DropDownPaymentMethod.DataTextField = "chvshortdescription"
            Me.DropDownPaymentMethod.DataValueField = "chvcodevalue"
            Me.DropDownPaymentMethod.DataBind()
        Catch
            Throw
        End Try
    End Sub

    Private Sub PopulateBillingMethod()
        Dim InsertRowBillingMethod As DataRow
        Dim g_dataset_dsGeneralBillingMethod As DataSet
        Try
            g_dataset_dsGeneralBillingMethod = YMCARET.YmcaBusinessObject.YMCABOClass.LookUpGeneralHubType()
            InsertRowBillingMethod = g_dataset_dsGeneralBillingMethod.Tables(0).NewRow()
            InsertRowBillingMethod.Item("chvCodeValue") = String.Empty
            InsertRowBillingMethod.Item("chvShortDescription") = String.Empty
            If Not g_dataset_dsGeneralBillingMethod Is Nothing Then
                g_dataset_dsGeneralBillingMethod.Tables(0).Rows.InsertAt(InsertRowBillingMethod, 0)
            End If
            Me.DropDownBillingMethod.DataSource = g_dataset_dsGeneralBillingMethod
            Me.DropDownBillingMethod.DataMember = "General Hub Type"
            Me.DropDownBillingMethod.DataTextField = "chvShortDescription"
            Me.DropDownBillingMethod.DataValueField = "chvCodeValue"
            Me.DropDownBillingMethod.DataBind()
        Catch
            Throw
        End Try
    End Sub

    Private Sub SetAttributes()
        Try


            'Shashi Shekhar:2009-12-23:Commented old call and added new call to eliminate the conflict of checksecurity access between server side and client side.
            '---------------------------------------------------------------------------------------------------------------------------------------------
            '' Dim SecurityCheckString As String = "javascript:if(CheckAccess('{0}')==false)return false;"

            ''Me.ButtonAdress.Attributes.Add("onclick", String.Format(SecurityCheckString, ButtonAdress.ID))
            Me.ButtonAdress.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonAdress.ID))

            'Me.ButtonTelephone.Attributes.Add("onclick", "javascript:CheckAccess('ButtonTelephone');")
            Me.ButtonTelephone.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonTelephone.ID))

            'Me.ButtonSave.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonSave');")
            Me.ButtonSave.Attributes.Add("onclick", "javascript:if(CheckAccess('" + ButtonSave.ID + "')==false){return false;}else{ ShowProcessingDialog(); }") 'AA:2014.09.26-BT:2440:YRS 5.0-2318 - Modified for the displaying the a progress dialog on save button click

            'Added for client side security check as per hafiz comments on security review doc
            'Priya 02-March-2010 YRS 5.0-1018:Security on Notes - Add Item button
            ' Me.ButtonNotesAdd.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonNotesAdd.ID))
            Me.ButtonYMCANotesAdd.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonYMCANotesAdd.ID))
            'Priya YRS 5.0-1018
            '---------------------------------------------------------------------------------------------------------------------------------------------


            Me.TextBoxEnrollmentDate.RequiredDate = True
            Me.TextBoxFedTaxId.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
            Me.TextBoxYMCANo.Attributes.Add("onblur", "javascript:FormatYMCANo();")


            Me.ButtonEmail.Attributes.Add("onclick", "javascript:return CheckAccessEMail('ButtonEmail');")
            Me.ButtonOfficersAdd.Attributes.Add("onclick", "javascript: return CheckAccessOfficersAdd('ButtonOfficersAdd');")
            Me.ButtonContactAdd.Attributes.Add("onclick", "javascript:return CheckAccessContactsAdd('ButtonContactAdd');")
            Me.ButtonBankInfoAdd.Attributes.Add("onclick", "javascript:return CheckAccessBankInfoAdd('ButtonBankInfoAdd');")
            Me.ButtonResoAdd.Attributes.Add("onclick", "javascript:return CheckAccessResoAdd('ButtonResoAdd');")
            'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -Start
            ' Me.ButtonOfficersUpdate.Attributes.Add("onclick", "javascript:return CheckAccessOfficersUpdate('ButtonOfficersUpdate');")
            'Me.ButtonContactUpdate.Attributes.Add("onclick", "javascript:return CheckAccessContactsUpdate('ButtonContactUpdate');")
            ' Me.ButtonBankInfoUpdate.Attributes.Add("onclick", "javascript:return CheckAccessBankInfoUpdate('ButtonBankInfoUpdate');")
            ' Me.ButtonResoUpdate.Attributes.Add("onclick", "javascript:return CheckAccessResoUpdate('ButtonResoUpdate');")
            'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -End
            'Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security            
            Me.TextBoxEnrollmentDate.Attributes.Add("onchange", "javascript:EnableSaveCancel();")
            Dim TextBoxUCDate As TextBox
            TextBoxUCDate = Me.TextBoxEnrollmentDate.FindControl("TextBoxUCDate")
            If Not TextBoxUCDate Is Nothing Then
                TextBoxUCDate.Attributes.Add("onchange", "javascript:EnableSaveCancel();")
            End If
            Me.TextBoxBoxYMCANo.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
            Me.TextBoxYMCAName.Attributes.Add("onchange", "javascript:EnableSaveCancel();")
            Me.TextBoxFedTaxId.Attributes.Add("onchange", "javascript:EnableSaveCancel();")
            Me.TextBoxStateTaxId.Attributes.Add("onchange", "javascript:EnableSaveCancel();")
            Me.TextBoxBoxYMCANo.Attributes.Add("onchange", "javascript:EnableSaveCancel();")
            Me.DropDownBillingMethod.Attributes.Add("onchange", "javascript:EnableSaveCancel();")
            Me.DropDownHubType.Attributes.Add("onchange", "javascript:EnableSaveCancel();")
            Me.DropDownPaymentMethod.Attributes.Add("onchange", "javascript:EnableSaveCancel();")
            Me.ButtonFind.Attributes.Add("onclick", "javascript:FormatYMCANo();")
            Me.ButtonEditFedTaxId.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonEditFedTaxId');")

            Me.checkboxPriority.Attributes.Add("onclick", "javascript:EnableSaveCancel();")  'Added by Dilip yadav: YRS 5.0.921

            'Anudeep:29.10.2013-BT-2278:YRS 5.0-2236 :Added to initiate start searching YMCA (i.e simulating find button) when ever Enter key / Carriage return key is pressed 
            Me.TextBoxYMCANo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
            Me.TextBoxCity.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
            Me.TextBoxState.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
            Me.TextBoxName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")

            'Start: Bala: 19/01/2019: YRS-AT-2398: Adding attributes to YNAN, MidMajor, Eligible Tracking User checkbox control.
            Me.checkboxYNAN.Attributes.Add("onclick", "javascript:EnableSaveCancel();")
            Me.checkboxEligibleTrackingUser.Attributes.Add("onclick", "javascript:EnableSaveCancel();")
            Me.checkboxMidMajors.Attributes.Add("onclick", "javascript:EnableSaveCancel();")
            'End: Bala: 19/01/2019: YRS-AT-2398: Adding attributes to YNAN, MidMajor, Eligible Tracking User checkbox control.
        Catch
            Throw
        End Try
    End Sub

    Public Sub getSecuredControls()
        Try
            Dim l_Int_UserId As Integer
            Dim l_String_FormName As String
            Dim l_control_Id As Integer
            Dim ds_AllsecItems As DataSet

            Dim l_int_access As Integer
            Dim l_string_controlNames As String
            Dim l_ds_ctrlNames As DataSet
            l_string_controlNames = ""
            l_Int_UserId = Convert.ToInt32(MyBase.Session("LoggedUserKey"))

            l_String_FormName = MyBase.Session("FormName").ToString().Trim()

            'To Find if the user belongs to teh Notes Admin Group so that all the checkboxes in the notes grid be enabled
            Dim l_intLoggedUser As Integer
            l_intLoggedUser = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.GetLoginNotesUser(l_Int_UserId)

            If l_intLoggedUser = 1 Then
                Me.NotesGroupUser = True
            Else
                Me.NotesGroupUser = False
            End If

            ds_AllsecItems = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetSecuredControlsOnForm(l_String_FormName)

            If Not ds_AllsecItems Is Nothing Then
                If Not ds_AllsecItems.Tables(0).Rows.Count = 0 Then
                    For Each dr As DataRow In ds_AllsecItems.Tables(0).Rows
                        l_control_Id = Convert.ToInt32(dr("sfc_ControlId"))
                        l_ds_ctrlNames = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetControlNames(l_control_Id)
                        l_int_access = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.LookUpLoginAccessPermission(l_Int_UserId, l_ds_ctrlNames.Tables(0).Rows(0)(0).ToString().Trim())

                        If l_int_access = 0 Or l_int_access = 1 Then
                            l_string_controlNames = l_string_controlNames + l_ds_ctrlNames.Tables(0).Rows(0)(0).ToString().Trim() + ","
                        End If

                    Next
                    Me.HiddenSecControlName.Value = l_string_controlNames
                End If
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Sub DeleteOfficerSub()
        Dim l_DataRow As DataRow
        Try
            Dim l_dataset_dsYMCAList As DataSet = YMCASearchList
            g_DataSetYMCAOfficer = DirectCast(Session("YMCA Officer"), DataSet)
            If Not g_DataSetYMCAOfficer Is Nothing Then
                l_DataRow = GetRowForUpdation(g_DataSetYMCAOfficer.Tables(0), "guiUniqueId", DirectCast(Session("OfficerUniqueId"), String))
                l_DataRow.Delete()
            End If
            Session("YMCA Officer") = g_DataSetYMCAOfficer
            PopulateOfficer()
        Catch
            Throw
        End Try
    End Sub

    Private Function PopulateDataIntoGeneralControls(ByVal GuiUniqueId As String) As Boolean
        Try
            'PopulateLabelCurrentStatus()

            LoadData(GuiUniqueId)
            PopulateAllTabs()
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Private Function GetRowForUpdation(ByVal p_datatable As DataTable, ByVal p_string_name As String, ByVal p_string_key As String) As DataRow
        Dim l_datarows As DataRow()
        Dim l_datarow As DataRow
        Try
            l_datarows = p_datatable.Select(p_string_name + "='" + p_string_key + "'")
            If l_datarows.Length > 0 Then
                l_datarow = l_datarows(0)
            End If
            Return l_datarow
        Catch
            Throw
        End Try
    End Function

    Public Sub DeleteContactSub(ByVal parameterSelectedIndex As Integer)
        Dim l_DataRow As DataRow
        Dim l_ArrRows As DataRow()

        Try
            Dim l_dataset_dsYMCAList As DataSet = YMCASearchList()
            g_DataSetYMCAContact = DirectCast(Session("YMCA Contact"), DataSet)

            If Not g_DataSetYMCAContact Is Nothing Then
                g_integer_countContact = parameterSelectedIndex
                l_ArrRows = g_DataSetYMCAContact.Tables(0).Select("guiUniqueId='" & Session("ContactUniqueId").ToString() & "'")
                If l_ArrRows.Length > 0 Then
                    l_ArrRows(0).Delete()
                Else
                    l_DataRow = g_DataSetYMCAContact.Tables(0).Rows(g_integer_countContact)
                    If Not l_DataRow Is Nothing Then
                        g_DataSetYMCAContact.Tables(0).Rows.Remove(l_DataRow)
                    End If
                End If
                Session("ContactUniqueId") = Nothing
                Session("ContactIndex") = Nothing 'Code Added By Ashutosh on 08-june-06
            End If

            Session("YMCA Contact") = g_DataSetYMCAContact
            PopulateContact()
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
            Dim l_bitImportant As Double
            g_DataSetYMCANotes = DirectCast(Session("YMCA Notes"), DataSet)

            Dim l_checkbox As CheckBox = DirectCast(sender, CheckBox)
            Dim dgItem As DataGridItem = DirectCast(l_checkbox.NamingContainer, DataGridItem)

            If dgItem.Cells(4).Text.ToUpper <> "&NBSP;" Then
                l_string_Uniqueid = dgItem.Cells(0).Text
                l_String_Search = " guiUniqueID = '" + l_string_Uniqueid + "'"

                l_datarow = g_DataSetYMCANotes.Tables(0).Select(l_String_Search)
                l_datarowUpdated = l_datarow(0)

                If l_checkbox.Checked = True Then
                    l_datarowUpdated("bitImportant") = 1
                Else
                    l_datarowUpdated("bitImportant") = 0
                End If

                PopulateNote()
                'EnableSaveCancelButtons()
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Public Sub PopulateYMCAList()
        Try
            Dim l_dataset_dsYMCAList As DataSet = YMCASearchList
            Me.DataGridYMCA.Visible = True
            Me.DataGridYMCA.DataSource = l_dataset_dsYMCAList.Tables(0)
            Me.DataGridYMCA.DataBind()
        Catch sqlEx As SqlException
            If sqlEx.Number = 60006 Then
                Me.DataGridYMCA.DataSource = Nothing
                Me.DataGridYMCA.DataBind()
                Me.DataGridYMCA.Visible = False
                MessageBox.Show(PlaceHolder1, "Please Confirm", "No records found.", MessageBoxButtons.OK)
            Else
                Response.Redirect("ErrorPageForm.aspx", False)
            End If
        Catch secEx As System.Security.SecurityException
            Response.Redirect("ErrorPageForm.aspx", False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

#Region "Clear Session"
    Public Sub Clear_session()
        Try
            Session("YMCA General") = Nothing
            Session("YMCA Officer") = Nothing
            Session("YMCA Contact") = Nothing
            Session("YMCA Branch") = Nothing
            Session("YMCA Resolution") = Nothing
            Session("YMCA BankInfo") = Nothing
            Session("YMCA Notes") = Nothing
            Session("YMCA List") = Nothing 'Shashi Shekhar singh:09-June-2010:For YRS 5.0-1072

            Session("l_string_Ymcaname") = Nothing

            Session("Metro") = Nothing
            Session("DeleteOfficerFlag") = Nothing
            Session("DeleteContactFlag") = Nothing
            Session("TermDateIsDBNull") = Nothing
            Session("dataset_index") = Nothing
            Session("GuiUniqueId") = Nothing
            Session("GeneralYMCAName") = Nothing
            Session("GeneralYMCANo") = Nothing
            Session("GeneralFederalTaxId") = Nothing
            Session("GeneralEnrollmentDate") = Nothing
            Session("GeneralPaymentMethod") = Nothing
            Session("GeneralBillingMethod") = Nothing
            Session("GeneralHubType") = Nothing

            Session("OfficerUniqueId") = Nothing
            Session("ContactIndex") = Nothing
            Session("ContactUnqiueId") = Nothing
            Session("BankInfoIndex") = Nothing
            Session("NotesIndex") = Nothing
            Session("Metro") = Nothing

            Session("PopUpAction") = Nothing
            Session("Withdrawn") = Nothing
            Session("TerminateYMCA") = Nothing
            Session("MergeYMCA") = Nothing
            Session("ReactivateYMCA") = Nothing
            Session("MetroYMCAs") = Nothing
            Session("MetroUniqueId") = Nothing
            Session("UpdateActiveResolution") = Nothing
            Session("FirstOfMonth_W") = Nothing
            Session("FirstOfMonth_T") = Nothing
            Session("Error") = Nothing
            Session("SingleResolution") = Nothing
            Session("ProcessProceed") = Nothing
            Session("TermProcessProceed") = Nothing
            Session("ActiveToTerminate") = Nothing
            Session("6MonthValidation") = Nothing
            'Start:Anudeep:12.04.2013 - YRS 5.0-1874:YMCA Name and Tax Number being erroneously updated
            Session("6MonthValidation_T") = Nothing
            Session("Address Information") = Nothing
            Session("BankAccountNumber") = Nothing
            Session("BankAccountType") = Nothing
            Session("BankEffectiveDate") = Nothing
            Session("BankInfoUpdate") = Nothing
            Session("BankPaymentMethod") = Nothing
            Session("BitImportant") = Nothing
            Session("BoolAddFlagBankInfo") = Nothing
            Session("BoolAddFlagContact") = Nothing
            Session("BoolEditFlag") = Nothing
            Session("BoolUpdateFlagBankInfo") = Nothing
            Session("BoolUpdateFlagContact") = Nothing
            Session("BoolUpdateFlagOfficer") = Nothing
            Session("ContactUpdate") = Nothing
            Session("Email Information") = Nothing
            Session("List_Index") = Nothing
            Session("NotesGroupUser") = Nothing
            Session("NotesText") = Nothing
            Session("OfficerEffDate") = Nothing
            Session("OfficerEmail") = Nothing
            Session("OfficerExtnNo") = Nothing
            Session("OfficerName") = Nothing
            Session("OfficerTelephone") = Nothing
            Session("ReactivateDate") = Nothing
            Session("ResolutionUpdate") = Nothing
            Session("ResoPopupEffDate") = Nothing
            'SR:2013.07.10 - BT-2117/YRS 5.0-2152 : The link from the Employment tab to YMCA Maintenance is broken 
            If MyBase.Session("Seamless_FromLandingPage") <> "True" Then
                MyBase.Session("Seamless_From") = Nothing
                MyBase.Session("Seamless_guiYMCAID") = Nothing
                MyBase.Session("Seamless_YMCANo") = Nothing
            End If
            'End,'SR:2013.07.10 - BT-2117/YRS 5.0-2152 : The link from the Employment tab to YMCA Maintenance is broken 
            MyBase.Session("Seamless_FromLandingPage") = Nothing
            Session("Select") = Nothing
            Session("SelectBank_BankABANumber") = Nothing
            Session("SelectBank_BankName") = Nothing
            Session("Telephone Information") = Nothing
            Session("TitleType") = Nothing
            Session("YMCAContact Schema") = Nothing
            Session("YMCAGeneralUniqueID") = Nothing
            Session("YMCANotes Schema") = Nothing
            Session("YMCAOfficer Schema") = Nothing
            Session("YMCAResolution Schema") = Nothing
            'End:Anudeep:12.04.2012 - YRS 5.0-1874:YMCA Name and Tax Number being erroneously updated
            If Not Session("SessionManager") Is Nothing Then
                Dim l_sm As YMCAUI.SessionManager.SessionHandler = DirectCast(Session("SessionManager"), YMCAUI.SessionManager.SessionHandler)
                l_sm.YMCAMaintenance = Nothing
                Session("SessionManager") = l_sm
            End If

            ''Shashi Shekhar"2010-03-12:Yrs-5.0-942:Nullfy the YMCAMaintenance module objects in session manager
            'Dim SM As YMCAUI.SessionManager.SessionHandler
            'SM.YMCAMaintenance = Nothing
            'Session("SessionManager") = SM

        Catch
            Throw
        End Try
    End Sub
#End Region '"Clear Session"
#End Region '"Custom Methods"

#Region "Sorting in Grids"

    Sub SortCommand_OnClick(ByVal Source As Object, ByVal e As DataGridSortCommandEventArgs)
        Try
            Dim dv As New DataView
            Dim SortExpression As String
            SortExpression = e.SortExpression
            Dim dg As DataGrid = DirectCast(Source, DataGrid)

            Dim ds As DataSet
            Select Case dg.ID
                Case "DataGridYMCAOfficer"
                    ds = Session("YMCA Officer")
                Case "DataGridYMCAContact"
                    ds = Session("YMCA Contact")
                Case "DataGridYMCABranches"
                    ds = Session("YMCA Branch")
                Case "DataGridYMCAResolutions"
                    ds = Session("YMCA Resolution")
                Case "DataGridYMCANotesHistory"
                    ds = Session("YMCA Notes")
                Case "DataGridYMCABankInfo"
                    ds = Session("YMCA BankInfo")
                Case "DataGridYMCA"
                    ds = YMCASearchList
                Case Else
                    Throw New Exception("Sorting is not handling in " & dg.ID)
            End Select

            dv = ds.Tables(0).DefaultView
            If Not ViewState(dg.ID) Is Nothing Then
                If ViewState(dg.ID).ToString.Trim.EndsWith("ASC") Then
                    dv.Sort = "[" + SortExpression + "] DESC"
                Else
                    dv.Sort = "[" + SortExpression + "] ASC"
                End If
            Else
                dv.Sort = "[" + SortExpression + "] ASC"
            End If

            dg.DataSource = Nothing
            dg.DataSource = dv
            dg.DataBind()
            ViewState(dg.ID) = dv.Sort
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

#End Region

#Region "Grids-Selected-Index-Changed"
    Private Sub DataGridYMCA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridYMCA.SelectedIndexChanged
        Dim l_DataSetYMCAList As DataSet
        Dim l_DataTableYMCAList As DataTable
        Dim l_DataRowYMCAList As DataRow
        Dim l_StringGuiUniqueiD As String
        Dim l_StringType As String
        Dim l_String
        Dim l_string_Ymcaname As String

        Try
            uniqueSessionId = Nothing
            'BS:2011.08.05:YRS 5.0-1380:BT:910 - reopen issue :here we kill all viewstate id that have already used while sorting  
            ViewState("DataGridYMCAOfficer") = Nothing
            ViewState("DataGridYMCAContact") = Nothing
            ViewState("DataGridYMCABranches") = Nothing
            ViewState("DataGridYMCAResolutions") = Nothing
            ViewState("DataGridYMCANotesHistory") = Nothing
            ViewState("DataGridYMCABankInfo") = Nothing
            ViewState("DataGridYMCA") = Nothing
            ViewState("Payrolldates") = Nothing 'Manthan | YRS-AT-2334
            hdrelationManagerName.Value = String.Empty 'Shashi Shekhar:For YRS 5.0-1334: clear the hidden field control
            Clear_session()
            Session("List_Index") = Me.DataGridYMCA.SelectedIndex
            'Priya 27-Jan-2011 : BT-719  Previous Withdrawan YMCA details displaying.
            'Showing fund status grid on termination of ymca
            If Session("Withdrawn") = Nothing AndAlso Session("TerminateYMCA") = Nothing Then
                LabelEmpRecord.Text = ""
                DataGridSummaryReport.DataSource = Nothing
                DataGridSummaryReport.DataBind()
                DatagridSummaryReportOnWithdrawal.DataSource = Nothing
                DatagridSummaryReportOnWithdrawal.DataBind()
            End If
            'END 27-Jan-2011 : BT-719  Previous Withdrawan YMCA details displaying.
            l_DataSetYMCAList = YMCASearchList
            'Go to General Tab
            Me.MultiPageYMCA.SelectedIndex = 1
            Me.YMCATabStrip.SelectedIndex = 1
            'selected row
            'Then Select active row from data grid - Preeti IssueId:YRST-2092/90
            g_integer_count = DataGridYMCA.SelectedIndex
            Session("dataset_index") = g_integer_count
            'Added by Anudeep for YRS:5.0-1621 on 2012.09.26
            ButtonAdd.Enabled = False

            'Now select row for selected guid- Preeti IssueId:YRST-2092/90
            Dim arrDr() As DataRow = l_DataSetYMCAList.Tables(0).Select("guiUniqueId ='" & Me.DataGridYMCA.SelectedItem.Cells(1).Text.Trim() & "'")
            If arrDr.Length > 0 Then
                l_DataRowYMCAList = arrDr(0)
            Else
                Exit Sub
            End If

            'Changed By:preeti On:9thFeb06 IssueId:YRST-2092/90 Mentioned For person module But found In YMCA. End
            l_StringGuiUniqueiD = l_DataRowYMCAList.Item("guiUniqueId")

            'storing the Guid in session to be used in address popup 
            Session("GuiUniqueId") = l_StringGuiUniqueiD

            If l_DataRowYMCAList.Item("Type") = "M" Then
                Me.YMCATabStrip.Items(4).Enabled = True
            Else
                Me.YMCATabStrip.Items(4).Enabled = False
            End If
            PopulateDataIntoGeneralControls(l_StringGuiUniqueiD)
            'Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security            
            EnableDisableControls(False)
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Commented by Anudeep for YRS
    'Private Sub DataGridYMCAResolutions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridYMCAResolutions.SelectedIndexChanged
    '    If DataGridYMCAResolutions.SelectedIndex > -1 Then
    '        If DataGridYMCAResolutions.Items(DataGridYMCAResolutions.SelectedIndex).Cells(2).Text.ToUpper = "&NBSP;" Then
    '            Me.ButtonResoUpdate.Enabled = True
    '        Else
    '            MessageBox.Show(PlaceHolder1, "Please Confirm", "Terminated Resolution records cannot be modified.", MessageBoxButtons.OK)
    '            Me.ButtonResoUpdate.Enabled = False
    '        End If
    '    End If
    'End Sub
    'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 -Start
    Private Sub DataGridYMCANotesHistory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridYMCANotesHistory.SelectedIndexChanged
        Try
            g_integer_countNotes = DataGridYMCANotesHistory.SelectedIndex
            Session("NotesIndex") = g_integer_countNotes
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub SetSelectedImageOfDataGrid(ByVal sender As System.Object, ByVal e As System.EventArgs, ByVal RadioButtonName As String)
        Try
            Dim i As Integer
            Dim dg As DataGrid = DirectCast(sender, DataGrid)

            For i = 0 To dg.Items.Count - 1
                If dg.Items(i).ItemType = ListItemType.AlternatingItem OrElse dg.Items(i).ItemType = ListItemType.Item OrElse dg.Items(i).ItemType = ListItemType.SelectedItem Then
                    Dim l_button_Select As ImageButton

                    'l_button_Select = DirectCast(DataGridYMCAContact.Items(i).FindControl(RadioButtonName), ImageButton)
                    l_button_Select = DirectCast(dg.Items(i).FindControl(RadioButtonName), ImageButton)
                    If Not l_button_Select Is Nothing Then
                        If i = dg.SelectedIndex Then
                            l_button_Select.ImageUrl = "images\selected.gif"
                        Else
                            l_button_Select.ImageUrl = "images\select.gif"
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region


    'Priya 27-Feb-2009 : YRS 5.0-517 Remove the 6 months before/after validation in the withdrawal process.
    'Start : Commented by Dilip : 09-July-09 : BT - 838
    '#Region "Validation of 6 month"
    Private Function ValidateBeforeAfter6Month(ByVal strDate As String) As String
        Dim strMessage As String
        Try
            Dim date6MonthBefore As Date
            Dim date6MonthAfter As Date

            date6MonthAfter = System.DateTime.Today.Date.AddMonths(6).ToShortDateString()
            date6MonthBefore = System.DateTime.Today.Date.AddMonths(-6).ToShortDateString()

            If (strDate < date6MonthBefore) Then
                'strMessage = "The date you selected is more than six months in the past.  Do you wish to continue?"
                strMessage = GetMessageByTextMessageNo(MESSAGE_YMCA_DATE_MORETHAN_SIXMONTHS_PAST_ERROR)
            ElseIf (strDate > date6MonthAfter) Then
                'strMessage = "The date you selected is more than six months in the future.  Do you wish to continue?"
                strMessage = GetMessageByTextMessageNo(MESSAGE_YMCA_DATE_MORETHAN_SIXMONTHS_FUTURE_ERROR)
            Else
                strMessage = ""
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return strMessage

    End Function
    '#End Region
    'END : Commented by Dilip : 09-July-09 : BT - 838
    Private Sub ButtonEditFedTaxId_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditFedTaxId.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditFedTaxId", Convert.ToInt32(MyBase.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            'Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security
            EnableDisableControls(True)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    'Private Sub ButtonWithdraw_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonWithdraw.Click
    '    'Priya 6-April-2009 added function to Clear session of WTM
    '    ClearSessionForWTM()
    '    'End 6-April-2009

    '    LabelWithdrawDate.Enabled = True
    '    DropdownWithdrawDate.Enabled = True
    '    LabelYerdiAccess_W.Enabled = True

    '    'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
    '    'DropdownYerdiAccess_W.Enabled = True
    '    DropdownYerdiAccess_T.Enabled = True
    '    'End YRS 5.0-873

    '    'Start: Dilip - Bug Id - 838 - 08-july-09
    '    Populatedates(DropdownWithdrawDate)
    '    'End: Dilip - Bug Id - 838 - 08-july-09 
    '    'Swopna- Bug Id 408- 7 May,2008 ---------start
    '    ButtonTerminate.Enabled = False
    '    ButtonMerge.Enabled = False
    '    'Swopna- Bug Id 408- 7 May,2008 ---------end
    '    LabelTerminationDate.Enabled = False
    '    DropdownlistTermination.Enabled = False
    '    LabelYerdiAccess_T.Enabled = False
    '    'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
    '    'DropdownYerdiAccess_T.Enabled = False
    '    LabelYerdiAccess_T.Enabled = True
    '    'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y

    '    'EnableSaveCancelButtons()
    '    Session("Withdrawn") = True
    'End Sub

    'Private Sub ButtonTerminate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTerminate.Click

    '    'Priya 6-April-2009 added function to Clear session of WTM
    '    ClearSessionForWTM()
    '    'End 6-April-2009

    '    'Swopna 26May08---start
    '    If LabelCurrentStatus.Text = String.Empty Then
    '        Session("ActiveToTerminate") = True
    '    End If
    '    'Swopna 26May08---end
    '    'LabelWithdrawDate.Enabled = False 

    '    'Start : Commented and added by Dilip : 09-July-09 : BT - 838
    '    DropdownWithdrawDate.Enabled = False
    '    Populatedates(DropdownlistTermination)
    '    'End : Commented and added by Dilip : 09-July-09 : BT - 838


    '    LabelYerdiAccess_W.Enabled = False

    '    'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
    '    'DropdownYerdiAccess_W.Enabled = False

    '    'END YRS 5.0-873
    '    'Swopna 8 May,2008
    '    LabelReactivate.Enabled = False
    '    TextboxReactivate.Enabled = False
    '    PopcalendarReactivate.Enabled = False
    '    'Swopna 8 May,2008


    '    Dim g_DataSetActiveParticipants As DataSet
    '    Dim l_datatable_Active As DataTable

    '    Session("TerminateYMCA") = True
    '    g_DataSetActiveParticipants = YMCARET.YmcaBusinessObject.YMCABOClass.GetTotalActiveParticipants((DirectCast(Session("GuiUniqueId"), String)))
    '    l_datatable_Active = g_DataSetActiveParticipants.Tables("ActiveEmployees")
    '    If l_datatable_Active.Rows.Count <> 0 Then
    '        If l_datatable_Active.Rows(0).Item("Count") = 0 Then
    '            LabelTerminationDate.Enabled = True
    '            DropdownlistTermination.Enabled = True
    '            LabelYerdiAccess_T.Enabled = True
    '            DropdownYerdiAccess_T.Enabled = True
    '            'EnableSaveCancelButtons()
    '            'Swopna- Bug Id 408- 7 May,2008 ---------start
    '            'ButtonWithdraw.Enabled = False
    '            ButtonMerge.Enabled = False
    '            'Swopna- Bug Id 408- 7 May,2008 ---------end
    '        Else
    '            LabelTerminationDate.Enabled = True
    '            DropdownlistTermination.Enabled = True
    '            LabelYerdiAccess_T.Enabled = True
    '            DropdownYerdiAccess_T.Enabled = True
    '            'Commented by sanjay
    '            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Active Employees exist for this YMCA. Cannot Proceed!", MessageBoxButtons.OK)
    '            ''DisableSaveCancelButtons()
    '        End If
    '    Else 'case when only non-active employees present
    '        LabelTerminationDate.Enabled = True
    '        DropdownlistTermination.Enabled = True
    '        LabelYerdiAccess_T.Enabled = True
    '        DropdownYerdiAccess_T.Enabled = True
    '        'EnableSaveCancelButtons()
    '        'Swopna- Bug Id 408- 7 May,2008 ---------start
    '        'ButtonWithdraw.Enabled = False
    '        ButtonMerge.Enabled = False
    '        'Swopna- Bug Id 408- 7 May,2008 ---------end
    '    End If
    'End Sub
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code


    'Private Sub Summary_Report()
    Private Sub Summary_Report(ByVal l_withdraw_date As DateTime)
        Try
            Dim l_ds_NonParticipants As DataSet
            Dim l_dt_NonParticipants As DataTable

            'Start : Commented and added by Dilip : 09-July-09 : BT - 838           
            l_ds_NonParticipants = YMCARET.YmcaBusinessObject.YMCABOClass.GetParticipantStatusOnWithdrawal((DirectCast(Session("GuiUniqueId"), String)), DropdownPayrollDate_W.SelectedValue, l_withdraw_date) 'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed parameter name of payroll date of withdrawal
            'END : Commented and added by Dilip : 09-July-09 : BT - 838

            If DataGridSummaryReport.Items.Count > 0 Then
                DataGridSummaryReport.Visible = True
            Else
                DataGridSummaryReport.Visible = False
            End If

            DatagridSummaryReportOnWithdrawal.Visible = True

            If HelperFunctions.isNonEmpty(l_ds_NonParticipants) Then
                'If l_ds_NonParticipants.Tables.Count > 0 Then
                l_dt_NonParticipants = l_ds_NonParticipants.Tables("NonParticipantEmployees")
                'If l_dt_NonParticipants.Rows.Count > 0 Then
                Dim l_str As String
                Dim l_count As Integer
                Dim l_TotalActiveCount As Integer
                l_TotalActiveCount = 0
                For Each drow As DataRow In l_dt_NonParticipants.Rows
                    l_count = 0
                    l_str = DirectCast(drow("Fund Status"), String).Trim()
                    If l_str = "PE" Then
                        l_count = drow("Count")
                    ElseIf l_str = "RE" Then
                        l_count = drow("Count")
                    ElseIf l_str = "AE" Then
                        l_count = drow("Count")
                    ElseIf l_str = "RA" Then
                        l_count = drow("Count")
                    End If

                    l_TotalActiveCount = l_TotalActiveCount + l_count
                Next
                DatagridSummaryReportOnWithdrawal.DataSource = l_dt_NonParticipants
                DatagridSummaryReportOnWithdrawal.DataBind()

                If l_TotalActiveCount <> 0 Then
                    LabelEmpRecord.Text = "Only Employment event record was changed for " & l_TotalActiveCount & " " & "participant(s). Fundevent status is not changed due to multiple employments."
                    LabelEmpRecord.Visible = True
                End If
            Else
                DatagridSummaryReportOnWithdrawal.Visible = False
                'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Message displayed if no active persons exists
                If Not txtDateSuspend.Text = "" Then
                    LabelEmpRecord.Text = GetMessageByTextMessageNo(MESSAGE_YMCA_SUSPEND_WITHDRAWN)
                End If
                'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Message displayed if no active persons exists
            End If



            LabelSummaryReport.Visible = True
            LabelBeforeWithdrawal.Visible = True
            LabelAfterWithdrawal.Visible = True

        Catch
            Throw
        End Try

    End Sub
    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added summary report method for Suspended YMCA
    'Private Sub Suspend_Summary_Report()
    Private Sub Suspend_Summary_Report(ByVal l_suspend_date As DateTime)
        Try
            Dim l_ds_NonParticipants As DataSet
            Dim l_dt_NonParticipants As DataTable

            'Start : Commented and added by Dilip : 09-July-09 : BT - 838
            l_ds_NonParticipants = YMCARET.YmcaBusinessObject.YMCABOClass.GetParticipantStatusOnWithdrawal((DirectCast(Session("GuiUniqueId"), String)), DropdownPayrollDate_S.SelectedValue, l_suspend_date)
            'END : Commented and added by Dilip : 09-July-09 : BT - 838

            If DataGridSummaryReportBeforeSuspend.Items.Count > 0 Then
                DataGridSummaryReportBeforeSuspend.Visible = True
            Else
                DataGridSummaryReportBeforeSuspend.Visible = False
            End If

            DatagridSummaryReportOnSuspend.Visible = True

            If Not l_ds_NonParticipants Is Nothing Then
                If l_ds_NonParticipants.Tables.Count > 0 Then
                    l_dt_NonParticipants = l_ds_NonParticipants.Tables("NonParticipantEmployees")
                    If l_dt_NonParticipants.Rows.Count > 0 Then
                        Dim l_str As String
                        Dim l_count As Integer
                        Dim l_TotalActiveCount As Integer
                        l_TotalActiveCount = 0
                        For Each drow As DataRow In l_dt_NonParticipants.Rows
                            l_count = 0
                            l_str = DirectCast(drow("Fund Status"), String).Trim()
                            If l_str = "PE" Then
                                l_count = drow("Count")
                            ElseIf l_str = "RE" Then
                                l_count = drow("Count")
                            ElseIf l_str = "AE" Then
                                l_count = drow("Count")
                            ElseIf l_str = "RA" Then
                                l_count = drow("Count")
                            End If

                            l_TotalActiveCount = l_TotalActiveCount + l_count
                        Next
                        DatagridSummaryReportOnSuspend.DataSource = l_dt_NonParticipants
                        DatagridSummaryReportOnSuspend.DataBind()

                        If l_TotalActiveCount <> 0 Then
                            LabelEmpRecordSuspend.Text = "Only Employment event record was changed for " & l_TotalActiveCount & " " & "participant(s). Fundevent status is not changed due to multiple employments."
                            LabelEmpRecordSuspend.Visible = True
                        End If
                    Else
                        DatagridSummaryReportOnSuspend.Visible = False
                    End If
                End If
            End If

            'LabelSummaryReport.Visible = True
            'LabelBeforeSuspend.Visible = True
            'LabelAfterSuspend.Visible = True

        Catch
            Throw
        End Try

    End Sub
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added summary report method for Suspended YMCA

    Private Sub Termination_Summary_Report()
        Try
            Dim l_ds_TerminateParticipants As DataSet
            Dim l_dt_TerminateParticipants As DataTable
            'Start--Manthan Rajguru | 2016.02.11 | YRS-AT-2334 & 1686 | Checking Dropdownvalue and passing it to method
            Dim strPayrollDates As String = ""
            If Not DropdownPayrollDate_S.SelectedValue = "" Then
                strPayrollDates = DropdownPayrollDate_S.SelectedValue
            ElseIf Not DropdownPayrollDate_W.SelectedValue = "" Then
                strPayrollDates = DropdownPayrollDate_W.SelectedValue
            End If

            'l_ds_TerminateParticipants = YMCARET.YmcaBusinessObject.YMCABOClass.GetParticipantStatusOnTermination((DirectCast(Session("GuiUniqueId"), String)), DropdownPayrollDate_T.SelectedValue, DropdownPayrollDate_S.SelectedValue) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed parameter name for payroll date of termiantion and withdrawal 
            l_ds_TerminateParticipants = YMCARET.YmcaBusinessObject.YMCABOClass.GetParticipantStatusOnTermination((DirectCast(Session("GuiUniqueId"), String)), DropdownPayrollDate_T.SelectedValue, strPayrollDates)
            'End--Manthan Rajguru | 2016.02.11 | YRS-AT-2334 & 1686 | Checking Dropdownvalue and passing it to method


            If DataGridSummaryReportBeforeTermination.Items.Count > 0 Then
                DataGridSummaryReportBeforeTermination.Visible = True
            Else
                DataGridSummaryReportBeforeTermination.Visible = False
            End If


            DatagridSummaryReportOnTerminate.Visible = True

            If Not l_ds_TerminateParticipants Is Nothing Then
                If l_ds_TerminateParticipants.Tables.Count > 0 Then
                    l_dt_TerminateParticipants = l_ds_TerminateParticipants.Tables("TerminateEmployees")
                    If l_dt_TerminateParticipants.Rows.Count > 0 Then
                        Dim l_str As String
                        Dim l_count As Integer
                        Dim l_TotalActiveCount As Integer
                        l_TotalActiveCount = 0
                        For Each drow As DataRow In l_dt_TerminateParticipants.Rows
                            l_count = 0
                            l_str = DirectCast(drow("Fund Status"), String).Trim()
                            If l_str = "PE" Then
                                l_count = drow("Count")
                            ElseIf l_str = "RE" Then
                                l_count = drow("Count")
                            ElseIf l_str = "AE" Then
                                l_count = drow("Count")
                            ElseIf l_str = "RA" Then
                                l_count = drow("Count")
                            ElseIf l_str = "NP" Then
                                l_count = drow("Count")
                            ElseIf l_str = "PENP" Then
                                l_count = drow("Count")
                            ElseIf l_str = "RDNP" Then
                                l_count = drow("Count")
                            End If
                            'PE','RE','AE','RA','NP','PENP','RDNP'
                            l_TotalActiveCount = l_TotalActiveCount + l_count
                        Next
                        'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed grid name for termiantion 
                        DatagridSummaryReportOnTerminate.DataSource = l_dt_TerminateParticipants
                        DatagridSummaryReportOnTerminate.DataBind()
                        'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed grid name for termiantion 

                        If l_TotalActiveCount <> 0 Then
                            LabelEmpRecordTerminate.Text = "Only Employment event record was changed for " & l_TotalActiveCount & " " & "participant(s). Fundevent status is not changed due to multiple employments."
                            LabelEmpRecordTerminate.Visible = True
                        End If
                    Else
                        DatagridSummaryReportOnTerminate.Visible = False
                    End If
                End If
            End If

            'LabelSummaryReport.Visible = True
            'LabelBeforeWithdrawal.Visible = True
            'LabelAfterWithdrawal.Visible = True

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    'Private Sub ButtonMerge_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonMerge.Click


    '    'Only an independent YMCA can be merged.
    '    'Added by Swopna, BugId-408, 29Apr2008---------start
    '    'Session("Withdrawn") = Nothing
    '    'Session("TerminateYMCA") = Nothing
    '    'Session("ReactivateYMCA") = Nothing
    '    'Priya 6-April-2009 added function to Clear session of WTM
    '    ClearSessionForWTM()
    '    'End 6-April-2009

    '    LabelTerminationDate.Enabled = False
    '    DropdownlistTermination.Enabled = False
    '    LabelYerdiAccess_T.Enabled = False
    '    DropdownYerdiAccess_T.Enabled = False

    '    LabelWithdrawDate.Enabled = False

    '    'Start : Commented and added by Dilip : 09-July-09 : BT - 838
    '    DropdownWithdrawDate.Enabled = False
    '    Populatedates(DropdownlistMergeDate)
    '    'END : Commented and added by Dilip : 09-July-09 : BT - 838


    '    LabelYerdiAccess_W.Enabled = False
    '    'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
    '    'DropdownYerdiAccess_W.Enabled = False
    '    'End YRS 5.0-873

    '    'Added by Swopna, BugId-408, 29Apr2008---------end
    '    'Swopna- Bug Id 408- 7 May,2008 ---------start
    '    'ButtonWithdraw.Enabled = False
    '    ButtonTerminate.Enabled = False
    '    'Swopna- Bug Id 408- 7 May,2008 ---------end
    '    'START: Gunanithi - 23-Dec-2015 - YRS-AT-1687 - Allowing merging of Metro YMCA along with independent YMCA
    '    'If DropDownHubType.SelectedValue = "M" Or DropDownHubType.SelectedValue = "B" Then
    '    '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Only an independent YMCA can be merged.", MessageBoxButtons.OK)
    '    '    ButtonWithdraw.Enabled = True
    '    '    ButtonTerminate.Enabled = True
    '    'ElseIf DropDownHubType.SelectedValue = "I" Then
    '    '    PopulateDropDownYMCANo("")
    '    '    LabelMergeDate.Enabled = True
    '    '    DropdownlistMergeDate.Enabled = True
    '    '    Session("MergeYMCA") = True
    '    'End If

    '    If DropDownHubType.SelectedValue = "M" Or DropDownHubType.SelectedValue = "I" Then
    '        PopulateDropDownYMCANo("")
    '        LabelMergeDate.Enabled = True
    '        DropdownlistMergeDate.Enabled = True
    '        Session("MergeYMCA") = True
    '    End If
    '    'END: Gunanithi - 23-Dec-2015 - YRS-AT-1687 - Allowing merging of Metro YMCA along with independent YMCA

    '    'EnableSaveCancelButtons()
    'End Sub

    'Private Sub ButtonReactivate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonReactivate.Click
    '    'Priya 6-April-2009 added function to Clear session of WTM
    '    ClearSessionForWTM()
    '    'End 6-April-2009
    '    ButtonResoAdd.Enabled = True
    '    LabelReactivate.Enabled = True
    '    TextboxReactivate.Enabled = True
    '    PopcalendarReactivate.Enabled = True
    '    LabelTerminationDate.Enabled = False
    '    'Swopna 9 May,2008
    '    DropdownlistTermination.Enabled = False
    '    LabelYerdiAccess_T.Enabled = False
    '    DropdownYerdiAccess_T.Enabled = False
    '    'Swopna 9 May,2008
    '    'Swopna 15 May,2008
    '    ButtonReactivate.Enabled = False
    '    ButtonTerminate.Enabled = False
    '    'Swopna 15 May,2008
    '    Session("ReactivateYMCA") = True
    '    'EnableSaveCancelButtons()
    'End Sub

    'Private Sub DropDownYMCANo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownYMCANo.SelectedIndexChanged
    '    'GetMetroYMCAUniqueId()
    'End Sub
    'End -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    'Populate dropdown with YMCA numbers in case of ButtonMerge click
    'Priya:17-March-2010:YRS 5.0-1017:Display withdrawal date as merged date for branches
    'Added parameter to pass metro id to get details of metroymca


    Private Sub PopulateDropDownYMCANo(ByVal strMetroYMCAId As String)
        Dim l_ds_DataSetGetMetroYMCAs As DataSet
        l_ds_DataSetGetMetroYMCAs = YMCARET.YmcaBusinessObject.YMCABOClass.GetMetroYMCAs(strMetroYMCAId)
        Session("MetroYMCAs") = l_ds_DataSetGetMetroYMCAs
        Dim dr As DataRow
        dr = l_ds_DataSetGetMetroYMCAs.Tables(0).NewRow
        'Priya 23-5-2010 assigbn system.null value to control
        dr("guiuniqueid") = System.DBNull.Value
        dr("chrYmcaNo") = "-Select No-"
        dr("chvYmcaName") = "-YMCA Name-"
        dr("DisplayYMCAName") = "--Select--"
        'LabelMetroDetails.Enabled = True 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old 
        'Start -- Manthan Rajguru | 2016.03.08 | YRS-AT-1686 | Added code to delete same YMCA from dropdown which is selected in screen to perform merge process
        For i As Int32 = 0 To l_ds_DataSetGetMetroYMCAs.Tables(0).Rows.Count - 1
            If Session("GuiUniqueId") IsNot Nothing AndAlso (l_ds_DataSetGetMetroYMCAs.Tables(0).Rows(i)("guiuniqueid").ToString().ToUpper() = Convert.ToString(Session("GuiUniqueId")).ToUpper()) Then
                l_ds_DataSetGetMetroYMCAs.Tables(0).Rows(i).Delete()
                l_ds_DataSetGetMetroYMCAs.AcceptChanges()
                Exit For
            End If
        Next
        l_ds_DataSetGetMetroYMCAs.Tables(0).Rows.InsertAt(dr, 0)
        'End -- Manthan Rajguru | 2016.03.08 | YRS-AT-1686 | Added code to delete same YMCA from dropdown which is selected in screen to perform merge process
        DropDownYMCANo.Enabled = True
        DropDownYMCANo.DataSource = l_ds_DataSetGetMetroYMCAs.Tables(0)
        DropDownYMCANo.DataTextField = "DisplayYMCAName"
        DropDownYMCANo.DataValueField = "guiuniqueid"
        DropDownYMCANo.DataBind()
    End Sub

    'Start -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    'Private Sub GetMetroYMCAUniqueId() 'Used to populate corresponding YMCA name and collect its guiUniqueId when a YMCA no. is selected from DropDownYMCANo 

    '    Dim l_string_SelectedYmcaUniqueId As String = ""
    '    Dim l_ds_DataSetGetMetroYMCAs As DataSet
    '    Dim l_arrRow As DataRow()

    '    l_string_SelectedYmcaUniqueId = Me.DropDownYMCANo.SelectedValue()
    '    Session("MetroUniqueId") = l_string_SelectedYmcaUniqueId 'to be used in update statement

    '    l_ds_DataSetGetMetroYMCAs = Session("MetroYMCAs")

    '    l_arrRow = l_ds_DataSetGetMetroYMCAs.Tables(0).Select("guiuniqueid='" & Session("MetroUniqueId").ToString() & "'")
    '    'TextboxMetroDetails.Text = l_arrRow(0)("chvYmcaName").ToString()
    '    LabelYMCAName.Text = l_arrRow(0)("chvYmcaName").ToString()
    'End Sub

    'End -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    Private Sub ResolutionAfterWTM()
        'Resolution tab refreshed after withdrawal/termination/reactivate/merge of YMCA
        g_DataSetYMCAResolution = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAResolution(DirectCast(Session("GuiUniqueId"), String))
        If Not g_DataSetYMCAResolution Is Nothing Then
            g_DataTableYMCAResolution = g_DataSetYMCAResolution.Tables(0)
            Me.DataGridYMCAResolutions.DataSource = g_DataTableYMCAResolution
            Me.DataGridYMCAResolutions.DataBind()
            Me.DataGridYMCAResolutions.SelectedIndex = -1
        End If
    End Sub
    Private Sub UpdateActiveResolution(ByVal EffectiveDate As Date)
        Dim l_ds_YMCAResolution As New DataSet
        If Not Page_Mode = "ADD" Then
            Dim l_arrRowResoUpdate As DataRow()
            l_ds_YMCAResolution = Session("YMCA Resolution")
            For Each dr As DataRow In l_ds_YMCAResolution.Tables(0).Rows
                If (dr.IsNull("Term. Date")) Then
                    dr("Term. Date") = EffectiveDate
                    Exit For
                End If
            Next
            Session("YMCA Resolution") = l_ds_YMCAResolution
        End If
    End Sub
    'This procedure checks if withdrawal date /termination date is greater than active resolution's effective date.
    Private Function Validation_WithdrawalTerminationDateFailed(ByVal wtDate As DateTime) As Boolean
        Try
            Dim i As Integer
            Dim l_datarow As DataRow
            Dim dsYMCAResolution As DataSet

            dsYMCAResolution = DirectCast(Session("YMCA Resolution"), DataSet)
            For i = 0 To dsYMCAResolution.Tables(0).Rows.Count - 1
                l_datarow = dsYMCAResolution.Tables(0).Rows(i)
                If l_datarow.IsNull("Term. Date") = True Then
                    If CType(l_datarow.Item("Eff. Date"), DateTime) > wtDate Then
                        Return True
                    ElseIf CType(l_datarow.Item("Eff. Date"), DateTime) <= wtDate Then
                        Return False
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Created By : Paramesh K.
    'Created On : July 31st 2008
    'Description : This function used to send emails alerts to the finance & loan department after merging YMCA
    Private Sub SendMail(ByVal ToEmailAdd As String, ByVal dsInformation As DataSet, ByVal IsMailForParticipants As Boolean)

        Dim l_drRow As DataRow
        Dim objMail As MailUtil
        Dim l_str_msg As String
        Dim l_str_oldYmcaNo As String
        Dim l_str_NewYmcaNo As String
        Try

            objMail = New MailUtil
            objMail.Subject = "A YMCA has merged with another"
            objMail.FromMail = ToEmailAdd
            objMail.ToMail = ToEmailAdd
            objMail.MailFormatType = Mail.MailFormat.Html

            'Getting old YMCA No from the dataset
            l_drRow = dsInformation.Tables(0).Select("guiUniqueID ='" & Convert.ToString(Session("GuiUniqueId")) & "'")(0)
            l_str_oldYmcaNo = l_drRow("chrYmcaNo")

            'Getting New YMCA No from the dataset
            'l_drRow = dsInformation.Tables(0).Select("guiUniqueID ='" & Convert.ToString(Session("MetroUniqueId")) & "'")(0)
            l_drRow = dsInformation.Tables(0).Select("guiUniqueID ='" & DropDownYMCANo.SelectedValue & "'")(0)
            l_str_NewYmcaNo = l_drRow("chrYmcaNo")

            'Body text
            '14-July-09
            l_str_msg = String.Format("YMCA No.{0} has merged with YMCA No.{1} effective from {2}.", l_str_oldYmcaNo, l_str_NewYmcaNo, DropdownPayrolldate_M.SelectedValue)

            'Mail to Loan Department with list of participants who has open loans
            If IsMailForParticipants Then
                '14-July-09
                l_str_msg = String.Format("YMCA No. {1} is the surviving entity. All loans under YMCA No. {0} must be reamortized to YMCA No. {1}’s pay schedule.<br> <br> ", l_str_oldYmcaNo, l_str_NewYmcaNo, DropdownPayrolldate_M.SelectedValue)

                If dsInformation.Tables(1).Rows.Count = 0 Then
                    l_str_msg &= "There are no participants with open loans"
                Else
                    l_str_msg &= "List of Participants that have open loan at YMCA No." & l_str_oldYmcaNo
                    l_str_msg &= "<table width='50%' border='1' >"
                    'Heading for Fund Number & Name
                    l_str_msg &= "<tr><td align='center'><B>Fund Id Number</B></td><td align='center'><B>Name</B></td></tr>"
                    'Adding participants
                    For Each drRow As DataRow In dsInformation.Tables(1).Rows
                        l_str_msg &= "<tr>"
                        l_str_msg &= "<td>" & drRow("FundNo") & "</td>"
                        l_str_msg &= "<td>" & drRow("Name") & "</td>"
                        l_str_msg &= "<tr>"
                    Next
                    l_str_msg &= "</table>"
                End If
            End If
            objMail.MailMessage = l_str_msg
            'Sending Mail
            objMail.Send()
        Catch ex As Exception
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", ex.Message, MessageBoxButtons.Stop)
        Finally
            l_drRow = Nothing
            objMail = Nothing
        End Try
    End Sub

    'Returns the new YMCA Id returned from the Database
    Private Function SaveGeneralTabInformation() As Boolean

        Dim dsYMCAGeneral As DataSet = Session("YMCA General")
        If dsYMCAGeneral Is Nothing Then Return False

        PersistYMCAGeneralTab()
        YMCARET.YmcaBusinessObject.YMCABOClass.SaveYMCAGeneral(dsYMCAGeneral)

        Dim YMCAId As String
        If HelperFunctions.isEmpty(dsYMCAGeneral) Then Throw New Exception("Unable to save YMCA General Information")
        YMCAId = dsYMCAGeneral.Tables(0).Rows(0)("guiUniqueId").ToString()
        If YMCAId = String.Empty Then Throw New Exception("YMCA Id not returned after saving")
        Session("guiUniqueId") = YMCAId
        'Replace the YMCA Number in the address dataset with the one we just 
        'received and then save all the address information
        Dim dr As DataRow
        For Each dr In g_dataset_dsAddressInformation.Tables(0).Rows
            If dr.RowState = DataRowState.Added Then
                dr.Item("guiEntityID") = Session("guiUniqueId")
            End If
        Next
        'YMCARET.YmcaBusinessObject.YMCABOClass.SaveYMCAGeneralAddressInformation(g_dataset_dsAddressInformation)
        Address.SaveAddressForYMCA(g_dataset_dsAddressInformation)

        'Save all Telephone Infomation from
        Dim dr_Telephone As DataRow
        For Each dr_Telephone In g_dataset_dsTelephoneInformation.Tables(0).Rows
            If dr_Telephone.RowState = DataRowState.Added Then
                dr_Telephone.Item("guiEntityID") = Session("guiUniqueId")
            End If
        Next
        YMCARET.YmcaBusinessObject.YMCABOClass.SaveYMCAGeneralTelephoneInformation(g_dataset_dsTelephoneInformation)

        'Save All Email Information
        Dim dr_Email As DataRow
        For Each dr_Email In g_dataset_dsEmailInformation.Tables(0).Rows
            If dr_Email.RowState = DataRowState.Added Then
                dr_Email.Item("guiEntityID") = Session("guiUniqueId")
            End If
        Next
        YMCARET.YmcaBusinessObject.YMCABOClass.SaveYMCAGeneralEmailInformation(g_dataset_dsEmailInformation)

        Return True
    End Function
    Private Function SaveResolutionInformation(ByVal newYMCAId As String) As Boolean
        Dim dsResolutions As DataSet
        dsResolutions = DirectCast(Session("YMCA Resolution"), DataSet) 'DirectCast(Session("YMCAResolution Schema"), DataSet)

        If HelperFunctions.isEmpty(dsResolutions) Then Return False

        'Replace the YMCA Number in the address dataset with the one we just 
        'received and then save all the address information
        Dim dr As DataRow
        For Each dr In dsResolutions.Tables(0).Rows
            If dr.RowState = DataRowState.Added Then
                dr.Item("guiYmcaID") = newYMCAId
            End If
        Next

        YMCARET.YmcaBusinessObject.YMCABOClass.InsertYMCAResolution(dsResolutions)

        PopulateResolution()
        Me.DataGridYMCAResolutions.SelectedIndex = -1 ' Bugtracker -305 17/12/2007
        Return True
    End Function
    Private Function SaveYMCAOfficerInformation(ByVal newYMCAId As String) As Boolean
        Dim dsOfficers As DataSet
        'START : Chandra sekar | 2018.05.23 | YRS-AT-3270 | Declared to host officer data
        Dim contactsForEmailNotification As DataSet
        Dim isNewOfficer As Boolean = False
        'END : Chandra sekar | 2018.05.23 | YRS-AT-3270 | Declared to host officer data
        dsOfficers = DirectCast(Session("YMCA Officer"), DataSet) 'DirectCast(Session("YMCAOfficer Schema"), DataSet)
        If dsOfficers Is Nothing Then Return False

        contactsForEmailNotification = dsOfficers.Copy() 'Chandra sekar | 2018.05.23 | YRS-AT-3270 | Declared to host officer data
        'Replace the YMCA Number in the officers dataset with the one we just 
        'received and then save all the officers information
        Dim dr As DataRow
        For Each dr In dsOfficers.Tables(0).Rows
            If dr.RowState = DataRowState.Added Then
                dr.Item("guiYMCAID") = newYMCAId
                isNewOfficer = True 'Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for new officer made on Officer tab (TrackIT 27727)
            End If
        Next
        YMCARET.YmcaBusinessObject.YMCABOClass.SaveYMCAOfficers(dsOfficers)
        'START : Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for new officer made on Officer tab (TrackIT 27727)
        If isNewOfficer Then
            SendEmailNotificationOnNewOfficer(contactsForEmailNotification) 'Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for new officer made on Officer tab (TrackIT 27727)
        End If
        'END : Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for new officer made on Officer tab (TrackIT 27727)
        PopulateOfficer()
        Me.DataGridYMCAOfficer.SelectedIndex = -1 ' by aparna 05/12/2007
        Return True
    End Function
    Private Function SaveYMCAContactInformation(ByVal newYMCAId As String) As Boolean
        Dim dsContacts As DataSet
        dsContacts = DirectCast(Session("YMCA Contact"), DataSet) 'DirectCast(Session("YMCAContact Schema"), DataSet)
        'START : Chandra sekar | 2018.05.23 | YRS-AT-3270 | Declared to host contacts data
        Dim contactsForEmailNotification As DataSet
        Dim isNewLPA As Boolean = False
        'END : Chandra sekar | 2018.05.23 | YRS-AT-3270 | Declared to host contacts data
        If dsContacts Is Nothing Then Return False

        contactsForEmailNotification = dsContacts.Copy() 'Chandra sekar | 2018.05.23 | YRS-AT-3270 | Copying contacts information to retain each rows RowState value
        'Replace the YMCA Number in the contacts dataset with the one we just 
        'received and then save all the contacts information
        Dim dr As DataRow
        For Each dr In dsContacts.Tables(0).Rows
            If dr.RowState = DataRowState.Added Then
                dr.Item("guiYMCAID") = newYMCAId
                'START:Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for add new LPA made on Contacts tab (TrackIT 27727)
                If dr.Item("TypeCode") = "TRANSM" Then
                    isNewLPA = True
                End If
                'END:Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for add new LPA made on Contacts tab (TrackIT 27727)
            End If
        Next

        YMCARET.YmcaBusinessObject.YMCABOClass.SaveYMCAContacts(dsContacts)
        'START:Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for add new LPA made on Contacts tab (TrackIT 27727)
        If isNewLPA Then
            SendEmailNotificationOnNewContact(contactsForEmailNotification)
        End If
        'END : Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for add new LPA made on Contacts tab (TrackIT 27727)
        PopulateContact()
        Me.DataGridYMCAContact.SelectedIndex = -1 ' by aparna 05/12/2007

        Return True
    End Function
    Private Function SaveYMCABankInformation(ByVal newYMCAId As String) As Boolean
        Dim dsBankInformation As DataSet
        dsBankInformation = DirectCast(Session("YMCA BankInfo"), DataSet)
        If dsBankInformation Is Nothing Then Return False
        Dim dr As DataRow
        For Each dr In dsBankInformation.Tables(0).Rows
            If dr.RowState = DataRowState.Added Then
                dr.Item("guiYMCAID") = newYMCAId
            End If
        Next
        YMCARET.YmcaBusinessObject.YMCABOClass.InsertYMCABankInfo(dsBankInformation)
        dsBankInformation = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCABankInfo(newYMCAId)

        Session("YMCA BankInfo") = dsBankInformation

        PopulateBank()
        DataGridYMCABankInfo.SelectedIndex = -1
        Return True
    End Function
    Private Function SaveYMCANotes(ByVal newYMCAId As String) As Boolean
        Dim dsNotes As DataSet
        dsNotes = DirectCast(Session("YMCA Notes"), DataSet) 'DirectCast(Session("YMCANotes Schema"), DataSet)
        If dsNotes Is Nothing Then Return False

        'Replace the YMCA Id in the notes dataset with the one we just 
        'received and then save all the notes information
        Dim dr As DataRow
        For Each dr In dsNotes.Tables(0).Rows
            If dr.RowState = DataRowState.Added Then
                dr.Item("guiEntityID") = newYMCAId
            End If
        Next
        YMCARET.YmcaBusinessObject.YMCABOClass.SaveYMCANotes(dsNotes)

        PopulateNote()
        DataGridYMCANotesHistory.SelectedIndex = -1
        Return True
    End Function
    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the message text and getting validation message from database
    Private Function IsValidYMCAWithdrawal() As Boolean

        Dim strMsgError As String = ""

        If DateUserControlWithdrawDate.Text = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_WITHDRAWAL_DATE_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a withdrawal date.", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Session("Error") = True
            Return False
        End If

        'Start : Commented and added by Dilip : 09-July-09 : BT - 838
        'If DropdownWithdrawDate.SelectedValue = String.Empty Then
        If DropdownPayrollDate_W.SelectedValue = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_PAYROLL_DATE_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Select a withdrawal date.", MessageBoxButtons.Stop)          
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Session("Error") = True
            Return False
        End If


        'END : Commented and added by Dilip : 09-July-09 : BT - 838

        'Session("GuiUniqueId")=In AtsYmcas table,guiuniqueid of YMCA.This value is refered as guiYmcaid in atsymcaentrants and atsymcaress table.
        'Swopna 30May08---start
        'Start : Commented and added by Dilip : 09-July-09 : BT - 838
        'If Validation_WithdrawalTerminationDateFailed(CType(DropdownWithdrawDate.SelectedValue, DateTime)) = True Then
        If Validation_WithdrawalTerminationDateFailed(CType(DropdownPayrollDate_W.SelectedValue, DateTime)) = True Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_PAYROLL_GREATERTHAN_RESOLUTION_DATE_ERROR)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Return False
        End If
        'END : Commented and added by Dilip : 09-July-09 : BT - 838
        'Swopna 30May08---end

        'Start : Commented and added by Dilip : 09-July-09 : BT - 838
        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
        'Priya:16-April-2010: YRS 5.0-873:Added if condition "ButtonWithdraw.Enabled = True" 
        'If (ButtonWithdraw.Enabled = True) Then
        '    Dim strWithdrawDateMessage As String
        '    strWithdrawDateMessage = ValidateBeforeAfter6Month(DropdownWithdrawDate.SelectedValue)
        '    If (strWithdrawDateMessage <> String.Empty AndAlso Session("6MonthValidation") <> DropdownWithdrawDate.SelectedValue) Then
        '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", strWithdrawDateMessage, MessageBoxButtons.YesNo)
        '        Session("6MonthValidation") = "True"
        '        Return False
        '    End If
        'End If
        'END : Commented and added by Dilip : 09-July-09 : BT - 838

        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
        If Me.DropdownYerdiAccess_W.SelectedValue = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_SELECTION_YERDIACCESS_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Select a value against Yerdi Access Required", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Return False
        End If
        'If Me.DropdownYerdiAccess_W.SelectedValue = String.Empty Then
        '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Select a value against Yerdi Access Required", MessageBoxButtons.Stop)
        '    Return False
        'End If
        'End YRS 5.0-873
        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
        'Priya:16-April-2010: YRS 5.0-873:Added if condition "ButtonWithdraw.Enabled = True" 
        'If (ButtonWithdraw.Enabled = True) Then
        'Swopna 26May08 Additional validation---start
        'Priya 31-March-2009 :YRS 5.0-707
        Dim strErrorMessage As String = ""
        Dim l_ds_DataSetTransmittal As DataSet
        'Start : Commented and added by Dilip : 09-July-09 : BT - 838
        'l_ds_DataSetTransmittal = YMCARET.YmcaBusinessObject.YMCABOClass.SearchTransmittals(DirectCast(Session("GuiUniqueId"), String), DropdownWithdrawDate.SelectedValue)
        l_ds_DataSetTransmittal = YMCARET.YmcaBusinessObject.YMCABOClass.SearchTransmittals(DirectCast(Session("GuiUniqueId"), String), DropdownPayrollDate_W.SelectedValue)
        'END : Commented and added by Dilip : 09-July-09 : BT - 838

        If Session("ProcessProceed") Is Nothing AndAlso _
            Not l_ds_DataSetTransmittal Is Nothing Then

            If Not l_ds_DataSetTransmittal.Tables("EarlierTransmittals") Is Nothing AndAlso _
                        l_ds_DataSetTransmittal.Tables("EarlierTransmittals").Rows.Count <> 0 Then
                Dim l_string_LastClosedPayPeriod As String = ""
                If Not l_ds_DataSetTransmittal.Tables("EarlierTransmittals").Rows(0).IsNull(0) Then
                    l_string_LastClosedPayPeriod = DirectCast(l_ds_DataSetTransmittal.Tables("EarlierTransmittals").Rows(0)(0), String)
                End If
                strErrorMessage = "The last transmittal was received on " & l_string_LastClosedPayPeriod & "." & "<BR>"
            End If

            'Priya 03-March-2009 : YRS 5.0-707 - 2 Issues with termination of ymca's. Should be able to terminate their resolution 
            'Priya 31-March-2009 : YRS 5.0-707 - make to warning message
            If Not l_ds_DataSetTransmittal.Tables("Transmittals") Is Nothing AndAlso _
                    l_ds_DataSetTransmittal.Tables("Transmittals").Rows.Count > 0 AndAlso _
                        l_ds_DataSetTransmittal.Tables("Transmittals").Rows(0)(0) >= 1 Then
                strErrorMessage = strErrorMessage & "Transmittals prior to the withdrawal date have not been received.<BR>"
            End If
        End If

        'Priya 31-March-2209
        If (strErrorMessage <> String.Empty) Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strErrorMessage & "<BR>" & "Do you want to continue with withdrawal process?", MessageBoxButtons.YesNo)
            Session("ProcessProceed") = True
            Return False
        End If
        'End If

        'End 03-March-2009
        Return True

    End Function
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the message text and getting validation message from database

    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added new Validation method for suspending process of YMCA and getting validation message from database
    Private Function IsValidYMCASuspend() As Boolean
        Dim strMsgError As String = ""
        If DateUserControlSuspensionDate.Text = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_SUSPENSION_DATE_ERROR)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Session("Error") = True
            Return False
        End If
        If DropdownPayrollDate_S.SelectedValue = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_PAYROLL_DATE_ERROR)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Session("Error") = True
            Return False
        End If
        If Validation_WithdrawalTerminationDateFailed(CType(DropdownPayrollDate_S.SelectedValue, DateTime)) = True Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_PAYROLL_GREATERTHAN_RESOLUTION_DATE_ERROR)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Return False
        End If
        If Me.DropdownYerdiAccess_S.SelectedValue = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_SELECTION_YERDIACCESS_ERROR)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Return False
        End If
        Dim strErrorMessage As String = ""
        Dim l_ds_DataSetTransmittal As DataSet
        l_ds_DataSetTransmittal = YMCARET.YmcaBusinessObject.YMCABOClass.SearchTransmittals(DirectCast(Session("GuiUniqueId"), String), DropdownPayrollDate_S.SelectedValue)
        If Session("ProcessProceed") Is Nothing AndAlso _
           Not l_ds_DataSetTransmittal Is Nothing Then

            If Not l_ds_DataSetTransmittal.Tables("EarlierTransmittals") Is Nothing AndAlso _
                        l_ds_DataSetTransmittal.Tables("EarlierTransmittals").Rows.Count <> 0 Then
                Dim l_string_LastClosedPayPeriod As String = ""
                If Not l_ds_DataSetTransmittal.Tables("EarlierTransmittals").Rows(0).IsNull(0) Then
                    l_string_LastClosedPayPeriod = DirectCast(l_ds_DataSetTransmittal.Tables("EarlierTransmittals").Rows(0)(0), String)
                End If
                strErrorMessage = "The last transmittal was received on " & l_string_LastClosedPayPeriod & "." & "<BR>"
            End If
            If Not l_ds_DataSetTransmittal.Tables("Transmittals") Is Nothing AndAlso _
                   l_ds_DataSetTransmittal.Tables("Transmittals").Rows.Count > 0 AndAlso _
                       l_ds_DataSetTransmittal.Tables("Transmittals").Rows(0)(0) >= 1 Then
                strErrorMessage = strErrorMessage & "Transmittals prior to the Suspension date have not been received.<BR>"
            End If
        End If
        If (strErrorMessage <> String.Empty) Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strErrorMessage & "<BR>" & "Do you want to continue with Supsend process?", MessageBoxButtons.YesNo)
            Session("ProcessProceed") = True
            Return False
        End If
        Return True
    End Function
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added new Validation method for suspending process of YMCA and getting validation message from database

    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added new Validation method for performing YMCA Suspension 
    Private Function PerformYMCASuspend() As Boolean
        Dim l_DataSetSummaryReportBefore As DataSet
        Dim l_datatable_Active As DataTable
        Dim l_suspend_Date As DateTime?
        l_DataSetSummaryReportBefore = YMCARET.YmcaBusinessObject.YMCABOClass.GetTotalActiveParticipants((DirectCast(Session("GuiUniqueId"), String)))
        l_datatable_Active = l_DataSetSummaryReportBefore.Tables("ActiveEmployees")
        DataGridSummaryReportBeforeSuspend.DataSource = l_datatable_Active
        DataGridSummaryReportBeforeSuspend.DataBind()

        Session("Suspend") = Nothing
        'YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnSuspendingYMCA(DropdownPayrollDate_S.SelectedValue, DirectCast(Session("GuiUniqueId"), String), DropdownYerdiAccess_S.SelectedValue, "S", DateUserControlSuspensionDate.Text) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for effective date ,yerdi access,and type       
        l_suspend_Date = YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnSuspendingYMCA(DropdownPayrollDate_S.SelectedValue, DirectCast(Session("GuiUniqueId"), String), DropdownYerdiAccess_S.SelectedValue, "S", DateUserControlSuspensionDate.Text) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for effective date ,yerdi access,and type       
        ResolutionAfterWTM()
        Session("YMCASuspend") = True 'Manthan Rajguru | 2016.02.16 | YRS-AT-2334 & 1686 | Setting session value to true
        PopulateWTSMTab()
        If (Not l_suspend_Date Is Nothing) Then
            'Suspend_Summary_Report()
            Suspend_Summary_Report(l_suspend_Date.Value)
        End If

        Session("ProcessProceed") = Nothing
        Session("6MonthValidation") = Nothing
        Return True
    End Function
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added new Validation method for performing YMCA Suspension 

    Private Function PerformYMCAWithdrawal() As Boolean

        'Swopna 20June08---Start
        Dim l_DataSetSummaryReportBefore As DataSet
        Dim l_datatable_Active As DataTable
        Dim l_withdraw_Date As DateTime?
        l_DataSetSummaryReportBefore = YMCARET.YmcaBusinessObject.YMCABOClass.GetTotalActiveParticipants((DirectCast(Session("GuiUniqueId"), String)))
        l_datatable_Active = l_DataSetSummaryReportBefore.Tables("ActiveEmployees")
        DataGridSummaryReport.DataSource = l_datatable_Active
        DataGridSummaryReport.DataBind()
        'Swopna 20June08---End

        'Swopna 26May08 Additional validation---end
        Session("Withdrawn") = Nothing
        'Summary_Report()----commented by Swopna 20 June08
        'Procedure to update fundevents & terminate emp records;update active employment status(change fund event status and terminate active employment records)

        'Start : Commented and added by Dilip : 09-July-09 : BT - 838
        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
        'YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnWithdrawingYMCA(DropdownWithdrawDate.SelectedValue, DirectCast(Session("GuiUniqueId"), String), DropdownYerdiAccess_W.SelectedValue)
        'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for effective date ,yerdi access,and type
        'YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnWithdrawingYMCA(DropdownWithdrawDate.SelectedValue, DirectCast(Session("GuiUniqueId"), String), DropdownYerdiAccess_T.SelectedValue)
        'YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnWithdrawingYMCA(DropdownPayrollDate_W.SelectedValue, DirectCast(Session("GuiUniqueId"), String), DropdownYerdiAccess_W.SelectedValue, "W", DateUserControlWithdrawDate.Text) 'Manthan Rajguru | 2016.02.12 | YRS-AT-2334 & 1686 |Changed the parameter value for allow yerdi access
        l_withdraw_Date = YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnWithdrawingYMCA(DropdownPayrollDate_W.SelectedValue, DirectCast(Session("GuiUniqueId"), String), DropdownYerdiAccess_W.SelectedValue, "W", DateUserControlWithdrawDate.Text) 'Manthan Rajguru | 2016.02.12 | YRS-AT-2334 & 1686 |Changed the parameter value for allow yerdi access
        'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for effective date ,yerdi access,and type

        'END YRS 5.0-873
        'END : Commented and added by Dilip : 09-July-09 : BT - 838

        'Swopna- Bug Id-412-7 May,2008----------start
        'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
        'ButtonWithdraw.Enabled = False
        'ButtonTerminate.Enabled = True
        'ButtonReactivate.Enabled = True
        'ButtonMerge.Enabled = False

        'LabelWithdrawDate.Enabled = False       

        'Start : Commented and added by Dilip : 09-July-09 : BT - 838
        'DropdownWithdrawDate.Enabled = False
        'End : Commented and added by Dilip : 09-July-09 : BT - 838
        'LabelYerdiAccess_W.Enabled = False

        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
        'DropdownYerdiAccess_W.Enabled = False
        'End YRS 5.0-873
        'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
        'Swopna- Bug Id-412-7 May,2008----------end
        ResolutionAfterWTM()
        'Summary report to be displayed in case of withdrawn YMCA
        'Swopna 30May08--start
        'In General tab,show the changes made

        'Swopna 30May08--end        
        Session("YMCAWithdrawn") = True 'Manthan Rajguru | 2016.02.16 | YRS-AT-2334 & 1686 | Setting session value to true
        PopulateWTSMTab()
        'Summary_Report() '----added by Swopna 20 June08
        If (Not l_withdraw_Date Is Nothing) Then
            Summary_Report(l_withdraw_Date.Value)
        End If

        'PopulateLabelCurrentStatus() 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code


        Session("ProcessProceed") = Nothing
        'Priya 12-March-2009 YRS 5.0-517 - Remove the 6 months before/after validation in the withdrawal process.
        Session("6MonthValidation") = Nothing
        'End priya
        Return True

    End Function

    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the message text and getting validation message from database
    Private Function IsValidYMCATermination() As Boolean
        Dim strMsgError As String = ""
        '14-July-09
        'If DropdownlistTermination.SelectedValue = String.Empty Then
        If DropdownPayrollDate_T.SelectedValue = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_PAYROLL_DATE_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a final payroll date.", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Session("Error") = True
            Return False
        End If

        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
        'Added if condition "ButtonTerminate.Enabled = True"
        'If (ButtonTerminate.Enabled = True) Then
        'Start : Commented and added by Dilip : 09-July-09 : BT - 838
        Dim strTerminationDateMessage As String
        strTerminationDateMessage = ValidateBeforeAfter6Month(DropdownPayrollDate_T.SelectedValue)
        If (strTerminationDateMessage <> String.Empty AndAlso Session("6MonthValidation_T") <> DropdownPayrollDate_T.SelectedValue) Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strTerminationDateMessage, MessageBoxButtons.YesNo)
            Session("6MonthValidation_T") = "True"
            Return False
        End If
        'END : Commented and added by Dilip : 09-July-09 : BT - 838
        'End If
        'end YRS 5.0-873
        If DropdownYerdiAccess_T.SelectedValue = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_SELECTION_YERDIACCESS_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Select a value against Yerdi Access Required", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Return False
        End If
        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
        'Added if condition "ButtonTerminate.Enabled = True"
        'If (ButtonTerminate.Enabled = True) Then
        If (DropdownPayrollDate_T.Enabled = True) Then
            'Priya 31-March-2009 placed validation above all warning message
            'case when a withdrawn YMCA is terminated
            ''Start : Commented and added by Dilip : 09-July-09 : BT - 838
            If LabelCurrentStatus.Text = "Withdrawn YMCA" AndAlso _
                CType(DropdownPayrollDate_W.SelectedValue, DateTime) > CType(DropdownPayrollDate_T.SelectedValue, DateTime) Then
                strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_TERMINATIONDATE_MORETHAN_WITHDRAWALDATE_ERROR)
                ' Above change is done on - 14-July-09
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Final payroll date for termination should be greater than or equal to final payroll date of withdrawal.", MessageBoxButtons.Stop)
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
                Return False
            End If
            'END : Commented and added by Dilip : 09-July-09 : BT - 838
            'End 31-March-2009

            'Swopna 26May08 Additional validation---start
            'If we are terminating an Active YMCA, we need to perform the validations which are performed at the time of Withdrawal
            If Not Session("ActiveToTerminate") Is Nothing AndAlso _
                CType(Session("ActiveToTerminate"), Boolean) = True Then

                'Swopna 30May08---start
                '14-July-09
                If Validation_WithdrawalTerminationDateFailed(CType(DropdownPayrollDate_T.SelectedValue, DateTime)) = True Then
                    strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_PAYROLL_GREATERTHAN_RESOLUTION_DATE_ERROR)
                    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Selected final payroll date is greater than active resolution's effective date.", MessageBoxButtons.Stop)
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
                    Return False
                End If
                'Swopna 30May08---end
                Dim l_ds_TransmittalTerminate As DataSet
                Dim strTerminationErrorMessage As String = ""
                '14-July-09
                l_ds_TransmittalTerminate = YMCARET.YmcaBusinessObject.YMCABOClass.SearchTransmittals(DirectCast(Session("GuiUniqueId"), String), DropdownPayrollDate_T.SelectedValue)

                If Session("TermProcessProceed") Is Nothing Then
                    If Not l_ds_TransmittalTerminate Is Nothing Then
                        If Not l_ds_TransmittalTerminate.Tables("EarlierTransmittals") Is Nothing Then
                            If l_ds_TransmittalTerminate.Tables("EarlierTransmittals").Rows.Count <> 0 Then
                                Dim l_string_LastClosedPayPeriodTerm As String = ""
                                If Not l_ds_TransmittalTerminate.Tables("EarlierTransmittals").Rows(0).IsNull(0) Then
                                    l_string_LastClosedPayPeriodTerm = DirectCast(l_ds_TransmittalTerminate.Tables("EarlierTransmittals").Rows(0)(0), String)
                                End If
                                strTerminationErrorMessage = "The last transmittal was received on " & l_string_LastClosedPayPeriodTerm & ".<BR>"
                            End If
                        End If
                    End If
                    'Priya 30-March-2009 YRS 5.0-707  Remove stop message and put warning message.
                    If Not l_ds_TransmittalTerminate Is Nothing Then
                        If Not l_ds_TransmittalTerminate.Tables("Transmittals") Is Nothing Then
                            If l_ds_TransmittalTerminate.Tables("Transmittals").Rows.Count > 0 Then
                                If l_ds_TransmittalTerminate.Tables("Transmittals").Rows(0)(0) >= 1 Then
                                    strTerminationErrorMessage = strTerminationErrorMessage & "Transmittals prior to the termination date have not been received.<BR>"
                                End If
                            End If
                        End If
                    End If
                    If strTerminationErrorMessage <> String.Empty Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", strTerminationErrorMessage & "<BR>" & "Do you want to continue with termination process?", MessageBoxButtons.YesNo)
                        'Priya assign value to session variable
                        Session("TermProcessProceed") = True
                        Return False
                    End If
                ElseIf CType(Session("TermProcessProceed"), Boolean) = False Then
                    'Process proceed
                    Session("ActiveToTerminate") = Nothing
                End If
            End If
        End If

        'Swopna 26May08 Additional validation---end
        Return True
    End Function
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the message text and getting validation message from database
    'Priya 25-Jan2011 BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)


    Private Function PerformYMCATermination() As Boolean
        Dim l_DataSetSummaryReportBefore As DataSet
        Dim l_datatable_BeforeTerminate As DataTable
        DataGridSummaryReportBeforeTermination.Visible = True
        l_DataSetSummaryReportBefore = YMCARET.YmcaBusinessObject.YMCABOClass.GetTotalBeforeTerminateParticipants((DirectCast(Session("GuiUniqueId"), String)))
        l_datatable_BeforeTerminate = l_DataSetSummaryReportBefore.Tables("BeforeTerminationEmployees")
        DataGridSummaryReportBeforeTermination.DataSource = l_datatable_BeforeTerminate
        DataGridSummaryReportBeforeTermination.Visible = True
        DataGridSummaryReportBeforeTermination.DataBind()
        Session("YMCATermination") = True 'Manthan Rajguru | 2016.02.16 | YRS-AT-2334 & 1686 | Setting session value to true
        Dim l_integer_update As Integer
        If LabelCurrentStatus.Text = "Withdrawn YMCA" Then 'case when a withdrawn YMCA is terminated
            l_integer_update = 1
            Session("Withdrawn") = Nothing
            '14-July-09
            YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnTerminationOfYmca(DropdownPayrollDate_T.SelectedValue, DirectCast(Session("GuiUniqueId"), String), l_integer_update, DropdownYerdiAccess_T.SelectedValue) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for Dropdown of yerdi and payroll date       

            Session("TerminateYMCA") = Nothing '--------11 June 08 
            'In General tab,show the changes made
            'PopulateLabelCurrentStatus() 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
            PopulateWTSMTab()
            Session("TermProcessProceed") = Nothing
        ElseIf LabelCurrentStatus.Text = "" Then 'case when an active YMCA is terminated
            l_integer_update = 2
            Session("Withdrawn") = Nothing
            '14-July-09
            YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnTerminationOfYmca(DropdownPayrollDate_T.SelectedValue, DirectCast(Session("GuiUniqueId"), String), l_integer_update, DropdownYerdiAccess_T.SelectedValue) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for Dropdown of yerdi and payroll date       

            Session("TerminateYMCA") = Nothing '--------11 June 08 
            'In General tab,show the changes made
            'PopulateLabelCurrentStatus()
            PopulateWTSMTab()
            ResolutionAfterWTM()
            Session("TermProcessProceed") = Nothing
            'Start --Manthan Rajguru | 2016.02.11 |YRS-AT-2334 | case when a Suspended YMCA is terminated
        ElseIf LabelCurrentStatus.Text = "Suspended YMCA" Then
            l_integer_update = 3
            Session("Withdrawn") = Nothing
            YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnTerminationOfYmca(DropdownPayrollDate_T.SelectedValue, DirectCast(Session("GuiUniqueId"), String), l_integer_update, DropdownYerdiAccess_T.SelectedValue) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for Dropdown of yerdi and payroll date       

            Session("TerminateYMCA") = Nothing
            PopulateWTSMTab()
            ResolutionAfterWTM()
            Session("TermProcessProceed") = Nothing
            'End --Manthan Rajguru | 2016.02.11 |YRS-AT-2334 | case when a Suspended YMCA is terminated
        ElseIf LabelCurrentStatus.Text = "Terminated YMCA" Then
            l_integer_update = 1
            Session("Withdrawn") = Nothing
            '14-July-09
            YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnTerminationOfYmca(DropdownPayrollDate_T.SelectedValue, DirectCast(Session("GuiUniqueId"), String), l_integer_update, DropdownYerdiAccess_T.SelectedValue) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for Dropdown of yerdi and payroll date       

            Session("TerminateYMCA") = Nothing '--------11 June 08 
            'In General tab,show the changes made
            'PopulateLabelCurrentStatus()
            PopulateWTSMTab()
            Session("TermProcessProceed") = Nothing
        End If
        Termination_Summary_Report() 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Calling Report for termiantion

        'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
        'ButtonWithdraw.Enabled = False
        'ButtonTerminate.Enabled = False
        'ButtonReactivate.Enabled = True
        'ButtonMerge.Enabled = False

        'LabelTerminationDate.Enabled = False
        'DropdownlistTermination.Enabled = False
        'LabelYerdiAccess_T.Enabled = False
        'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code

        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y
        'DropdownYerdiAccess_T.Enabled = False
        'Priya:16-April-2010: YRS 5.0-873:Allowing YERDI access for a withdrawn or terminated Y



        Return True

    End Function

    'Priya 25-Jan2011 BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)

    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the message text and getting validation message from database
    Private Function IsValidYMCAReactivation() As Boolean
        Dim strMsgError As String = ""

        If DateUserControlReactivateDate.Text = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_REACTIVATION_DATE_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Enter reactivation date.", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Session("Error") = True
            Return False
        End If

        Return True
    End Function
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the message text and getting validation message from database

    Private Function PerformYMCAReactivation() As Boolean
        'On reactivating a withdrawn/terminated YMCA
        'Before saving this part,check whether an active resolution is present.If yes,save this part else do not save it.
        Dim l_int_currentStatus As Integer
        Dim l_Payroll_Date As Date
        If LabelCurrentStatus.Text = "Withdrawn YMCA" Then
            l_int_currentStatus = 1
            l_Payroll_Date = DropdownPayrollDate_W.SelectedValue
        ElseIf LabelCurrentStatus.Text = "Terminated YMCA" Then
            l_int_currentStatus = 2
            'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added to check active resolution is present for suspended YMCA
        ElseIf LabelCurrentStatus.Text = "Suspended YMCA" Then
            l_int_currentStatus = 3
            l_Payroll_Date = DropdownPayrollDate_S.SelectedValue
        End If
        'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added to check active resolution is present for suspended YMCA

        Dim dsYMCAResolution As DataSet

        dsYMCAResolution = DirectCast(Session("YMCA Resolution"), DataSet)

        If dsYMCAResolution Is Nothing OrElse dsYMCAResolution.Tables(0) Is Nothing _
            OrElse dsYMCAResolution.Tables(0).Rows.Count = 0 OrElse dsYMCAResolution.Tables(0).Rows(0).IsNull("Term. Date") = False Then
            LabelResoNotEntered.Text = "Resolution not entered"
            Me.YMCATabStrip.SelectedIndex = 9
            Me.MultiPageYMCA.SelectedIndex = 9
            'LabelReactivate.Enabled = True
            'TextboxReactivate.Enabled = True
            'PopcalendarReactivate.Enabled = True
            'Swopna 12 May,2008
            Return False
        End If

        Session("ReactivateYMCA") = Nothing

        'YMCARET.YmcaBusinessObject.YMCABOClass.InsertRecordOnReactivation(TextboxReactivate.Text, DropDownPaymentMethod.SelectedValue, DropDownBillingMethod.SelectedValue, DirectCast(Session("GuiUniqueId"), String))
        'Insert procedure to update fund events of employees in case of reactivation of withdrawn YMCA
        'Added/commmented -Swopna(13May08) -Stored proc on update of fund status modified in order to get correct no. of employees working in a particular YMCA.

        'Start : Commented and added by Dilip : 09-July-09 : BT - 838
        YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnReActivatingYMCA(DateUserControlReactivateDate.Text, DropDownPaymentMethod.SelectedValue, DropDownBillingMethod.SelectedValue, DirectCast(Session("GuiUniqueId"), String), l_int_currentStatus, l_Payroll_Date) 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for Reactivate date  
        'End: Commented and added by Dilip : 09-July-09 : BT - 838
        'Swopna- Bug Id-412-7 May,2008----------start
        'ButtonWithdraw.Enabled = True
        'ButtonTerminate.Enabled = True
        'ButtonReactivate.Enabled = False
        'ButtonMerge.Enabled = True
        'Swopna- Bug Id-412-7 May,2008----------end
        'Swopna 15May08
        LabelResoNotEntered.Text = String.Empty
        'Swopna 15May08
        'Swopna 30May08--start
        'In General tab,show the changes made
        'PopulateLabelCurrentStatus()
        PopulateWTSMTab()

        'Swopna 30May08--end
        ResolutionAfterWTM()
        'LabelReactivate.Enabled = False
        'TextboxReactivate.Enabled = False
        'PopcalendarReactivate.Enabled = False
        Return True

    End Function


    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the message text and getting validation message from database
    Private Function IsValidYMCAMerge() As Boolean
        Dim strMsgError As String = ""
        If DropDownYMCANo.SelectedValue = "" Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_SELECTION_MERGING_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Select a Y that this Y is merging into.", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Return False
        End If

        If DateUserControlMergeDate.Text = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_MERGE_DATE_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a merge date.", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Session("Error") = True
            Return False
        End If

        '14-July-09
        'If DropdownlistMergeDate.SelectedValue = String.Empty Then
        If DropdownPayrolldate_M.SelectedValue = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_PAYROLL_DATE_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a final payroll date.", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Session("Error") = True
            Return False
        End If

        If Me.DropdownYerdiAccess_M.SelectedValue = String.Empty Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_SELECTION_YERDIACCESS_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Select a value against Yerdi Access Required", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Return False
        End If

        Dim dsFinance As DataSet
        dsFinance = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("FINDEPT_EMAILID")
        If dsFinance Is Nothing OrElse dsFinance.Tables.Count = 0 OrElse dsFinance.Tables(0).Rows.Count = 0 Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_EMAILADDRESS_FINANCEDEPT_NOTFOUND_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Email Address of Finance Department not found in configuration table", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Return False
        End If
        'Sending mail to Loan Department
        'Send this email only if there are any open loans with the merged YMCA.
        dsFinance = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("TDLOANS_DEFAULT_TO_EMAILID")
        If dsFinance.Tables(0).Rows.Count = 0 Then
            strMsgError = GetMessageByTextMessageNo(MESSAGE_YMCA_EMAILADDRESS_TDLOANS_NOTFOUND_ERROR)
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Email Address of TD Loans not found in configuration table", MessageBoxButtons.Stop)
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strMsgError, MessageBoxButtons.Stop)
            Return False
        End If

        Return True
    End Function
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Changed the message text and getting validation message from database

    Private Function PerformYMCAMerge() As Boolean
        'On merging YMCA update AtsYmcaEntrants,AtsYmcaRess,AtsYmcas tables.

        Session("MergeYMCA") = Nothing
        '14-July-09
        'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for effective date,yerdi access and merge date
        'YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnMergingYMCA(DropdownlistMergeDate.SelectedValue, DirectCast(Session("GuiUniqueId"), String), DirectCast(Session("MetroUniqueId"), String))        
        YMCARET.YmcaBusinessObject.YMCABOClass.UpdateOnMergingYMCA(DropdownPayrolldate_M.SelectedValue, DirectCast(Session("GuiUniqueId"), String), DropDownYMCANo.SelectedValue, DateUserControlMergeDate.Text)
        'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added parameter for effective date,yerdi access and merge date
        'Created By Paramesh K. on July 31st 2008 [Issue No. 488]
        'Sending Mail alerts after merging YMCA
        '********************
        Dim dsFinance As DataSet
        Dim dsPersons As DataSet
        Dim l_string_ToAddress1 As String = String.Empty
        Dim l_string_ToAddress2 As String = String.Empty
        'Dim l_str_msg As String = String.Empty
        Dim objMail As New MailUtil

        'Checking mail service is on or off
        If objMail.MailService = True Then  ' l_str_msg <> "No_Mail_Service" Then
            'Mail service is on
            'Calling Buiness for getting the Participants information to send mails
            'dsPersons = YMCARET.YmcaBusinessObject.YMCABOClass.GetParticipantsHavingOpenLoans(Convert.ToString(Session("GuiUniqueId")), Convert.ToString(Session("MetroUniqueId")))
            dsPersons = YMCARET.YmcaBusinessObject.YMCABOClass.GetParticipantsHavingOpenLoans(Convert.ToString(Session("GuiUniqueId")), DropDownYMCANo.SelectedValue)

            'Sending mail to finance department
            dsFinance = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("FINDEPT_EMAILID")
            If dsFinance.Tables(0).Rows.Count = 0 Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Email Address of Finance Department not found in configuration table", MessageBoxButtons.Stop)
            Else
                l_string_ToAddress1 = dsFinance.Tables(0).Rows(0)("Value")

                'Mail to Finance department
                SendMail(l_string_ToAddress1, dsPersons, False)
            End If

            'Sending mail to Loan Department
            'Send this email only if there are any open loans with the merged YMCA.
            If dsPersons.Tables(1).Rows.Count > 0 Then
                dsFinance = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("TDLOANS_DEFAULT_TO_EMAILID")
                If dsFinance.Tables(0).Rows.Count = 0 Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Email Address of TD Loans not found in configuration table", MessageBoxButtons.Stop)
                Else
                    l_string_ToAddress2 = dsFinance.Tables(0).Rows(0)("Value")

                    'Mail to Loan Department
                    SendMail(l_string_ToAddress2, dsPersons, True)
                End If
            End If
        End If
        '********************
        objMail = Nothing

        ResolutionAfterWTM()
        ''ButtonMerge.Enabled = False
        ' ''Swopna- Bug Id-412-7 May,2008----------start
        ''ButtonWithdraw.Enabled = False
        ''ButtonTerminate.Enabled = False
        ''ButtonReactivate.Enabled = False
        'Swopna- Bug Id-412-7 May,2008----------end
        'Swopna- Bug Id-427-8 May,2008----------start
        'LabelMetroDetails.Enabled = False
        'DropDownYMCANo.Enabled = False
        'Swopna- Bug Id-427-8 May,2008----------end
        'LabelMergeDate.Enabled = False
        'DropdownlistMergeDate.Enabled = False
        'In General tab,show the changes made
        'PopulateLabelCurrentStatus() 'Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
        PopulateWTSMTab()

        'In order to populate hub type and metro name in General tab****
        Dim l_DataSetYMCAGeneral As DataRow
        g_DataSetYMCAGeneral = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAGeneral(DirectCast(Session("GuiUniqueId"), String))
        g_DataTableYMCAGeneral = g_DataSetYMCAGeneral.Tables("YMCA General")
        If HelperFunctions.isNonEmpty(g_DataTableYMCAGeneral) Then
            l_DataSetYMCAGeneral = g_DataTableYMCAGeneral.Rows.Item(0)
            Me.DropDownHubType.SelectedValue = DirectCast(l_DataSetYMCAGeneral("Chrhubind"), String).Trim
            Me.TextBoxMetroName.Text = DirectCast(l_DataSetYMCAGeneral("chvYmcaMetroName"), String).Trim

        End If
        'In order to populate hub type and metro name in General tab****

        Return True
    End Function
#Region "General Tabstrip helper function"
    Private Function EnableDisableAllTabs(ByVal status As Boolean)
        Dim t As Microsoft.Web.UI.WebControls.TabItem
        For Each t In YMCATabStrip.Items
            t.Enabled = status
        Next
    End Function
    Private Function EnableAllTabs()
        EnableDisableAllTabs(True)
    End Function
    Private Function DisableAllTabs()
        EnableDisableAllTabs(False)
    End Function
    Private Function DisableAllTabsExcept(ByVal index As Integer)
        If YMCATabStrip.Items.Count > index Then
            DisableAllTabs()
            YMCATabStrip.Items(index).Enabled = True

        End If
    End Function
#End Region

    Private Sub SetSaveCancelButtons(ByVal status As Boolean)
        Me.ButtonSave.Enabled = status
        Me.ButtonCancel.Enabled = status
        'Commented by Anudeep for YRS:5.0-1621 on 2012.09.26 
        ' Me.ButtonAdd.Enabled = Not status
        Me.ButtonOk.Enabled = Not status
    End Sub
    Private Sub DisableSaveCancelButtons()
        SetSaveCancelButtons(False)
    End Sub
    Private Sub EnableSaveCancelButtons()
        SetSaveCancelButtons(True)
    End Sub
    Private Function YmcaInformationHasChanged() As Boolean
        If Not IsNothing(Session("YMCA General")) AndAlso DirectCast(Session("YMCA General"), DataSet).HasChanges = True Then
            Return True
        End If
        If Not IsNothing(Session("Address Information")) AndAlso DirectCast(Session("Address Information"), DataSet).HasChanges = True Then
            Return True
        End If
        If Not IsNothing(Session("YMCA Officer")) AndAlso DirectCast(Session("YMCA Officer"), DataSet).HasChanges = True Then
            Return True
        End If
        If Not IsNothing(Session("YMCA Contact")) AndAlso DirectCast(Session("YMCA Contact"), DataSet).HasChanges = True Then
            Return True
        End If
        If Not IsNothing(Session("YMCA Branch")) AndAlso DirectCast(Session("YMCA Branch"), DataSet).HasChanges = True Then
            Return True
        End If
        If Not IsNothing(Session("YMCA Resolution")) AndAlso DirectCast(Session("YMCA Resolution"), DataSet).HasChanges = True Then
            Return True
        End If
        If Not IsNothing(Session("YMCA BankInfo")) AndAlso DirectCast(Session("YMCA BankInfo"), DataSet).HasChanges = True Then
            Return True
        End If
        If Not IsNothing(Session("YMCA Notes")) AndAlso DirectCast(Session("YMCA Notes"), DataSet).HasChanges = True Then
            Return True
        End If
        'Start -- Manthan Rajguru| YRS-AT-2334 & 1686 |Commented old code       
        'If Not Session("Withdrawn") = Nothing AndAlso Session("Withdrawn") = True Then
        '    Return True
        'End If
        'If Not Session("TerminateYMCA") = Nothing AndAlso Session("TerminateYMCA") = True Then
        '    Return True
        'End If
        'If Not Session("ReactivateYMCA") = Nothing AndAlso Session("ReactivateYMCA") = True Then
        '    Return True
        'End If
        'If Not Session("MergeYMCA") Is Nothing AndAlso Session("MergeYMCA") = True Then
        '    Return True
        'End If
        'If Not Session("DefaultActive") = Nothing AndAlso Session("DefaultActive") = True Then
        '    Return True
        'End If
        'End -- Manthan Rajguru| YRS-AT-2334 & 1686|Commented old code       
        If Not IsNothing(Session("Telephone Information")) AndAlso DirectCast(Session("Telephone Information"), DataSet).HasChanges = True Then
            Return True
        End If
        If Not IsNothing(Session("Email Information")) AndAlso DirectCast(Session("Email Information"), DataSet).HasChanges = True Then
            Return True
        End If
        If Not Me.WTSM_Operation = "" Then
            Return True
        End If
        Return False
    End Function
    Private Function NewYMCAIsBeingAdded() As Boolean
        Dim ds As DataSet
        If Session("YMCA General") Is Nothing Then Return False
        ds = DirectCast(Session("YMCA General"), DataSet)
        If HelperFunctions.isEmpty(ds) Then Return False
        Dim dr As DataRow
        For Each dr In ds.Tables(0).Rows
            If dr.RowState = DataRowState.Added Then Return True
        Next
        Return False
    End Function

#Region "Persistence Mechanism"
    Protected Overrides Function SaveViewState() As Object
        Session("Address Information") = g_dataset_dsAddressInformation
        Session("Telephone Information") = g_dataset_dsTelephoneInformation
        Session("Email Information") = g_dataset_dsEmailInformation
        ViewState("Page_Mode") = Page_Mode
        Return MyBase.SaveViewState()
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        g_dataset_dsAddressInformation = DirectCast(Session("Address Information"), DataSet)
        g_dataset_dsTelephoneInformation = DirectCast(Session("Telephone Information"), DataSet)
        g_dataset_dsEmailInformation = DirectCast(Session("Email Information"), DataSet)
        Page_Mode = ViewState("Page_Mode")
    End Sub
#End Region

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        PersistYMCAGeneralTab()
        g_DataSetYMCAGeneral = Session("YMCA General")

        If HelperFunctions.isNonEmpty(g_DataSetYMCAGeneral) Then
            If g_DataSetYMCAGeneral.Tables(0).Rows(0).RowState = DataRowState.Added Then
                LabelHdr.Text = "-- New YMCA"
            Else
                LabelHdr.Text = ""
                If (Not g_DataSetYMCAGeneral.Tables(0).Rows(0).IsNull("chvymcaname")) AndAlso (g_DataSetYMCAGeneral.Tables(0).Rows(0)("chvymcaname").ToString() <> String.Empty) Then
                    Dim dr As DataRow = g_DataSetYMCAGeneral.Tables(0).Rows(0)
                    LabelHdr.Text = String.Format(" -- {0}, YMCA No: {1}", dr("chvymcaname").ToString().Trim, dr("chrymcano").ToString().Trim) 'by aparna 07/09/2007
                    Session("GeneralYMCANo") = dr("chrymcano").ToString().Trim 'Shashi Shekhar:2010-03-09:for YRS-5.0-942
                End If
                '22-April-2010: BT-521 :Priya :need to check later Put below if under HelperFunctions.isNonEmpty(g_DataSetYMCAGeneral) if to avoid nullrefference error.
                'START Below lines are Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921
                LabelPriorityHdr.Visible = False
                If (MultiPageYMCA.SelectedIndex <> 0) Then
                    Dim dr As DataRow = g_DataSetYMCAGeneral.Tables(0).Rows(0)
                    Dim dv As DataView = g_DataSetYMCAGeneral.Tables(0).DefaultView
                    dv.RowStateFilter = DataViewRowState.OriginalRows

                    'BugID 1044: Neeraj Singh 01-Dec-2009
                    If HelperFunctions.isNonEmpty(dv) AndAlso dv.Item(0).Row.Item("Priority") Then
                        LabelPriorityHdr.Visible = True
                    Else
                        LabelPriorityHdr.Visible = False
                    End If
                    'BugID 1044
                End If
                'END Above lines are Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921
                'End BT-521 : 22-April-2010

            End If
        Else
            'Priya : 05-May-2010: BT-523:YMCA name & number clear in YMCA Information header
            If (DataGridYMCA.SelectedIndex < 0) Then
                LabelHdr.Text = ""
                Session("GeneralYMCANo") = Nothing 'Shashi Shekhar:2010-03-09:for YRS-5.0-942
            End If
            'End 05-May-2010: BT-523
        End If
        'BT-521 : 22-April-2010: Priya :need to check later Put below if under HelperFunctions.isNonEmpty(g_DataSetYMCAGeneral) if to avoid nullrefference error.
        ''START Below lines are Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921
        'LabelPriorityHdr.Visible = False
        'If (MultiPageYMCA.SelectedIndex <> 0) Then
        '    Dim dr As DataRow = g_DataSetYMCAGeneral.Tables(0).Rows(0)
        '    Dim dv As DataView = g_DataSetYMCAGeneral.Tables(0).DefaultView
        '    dv.RowStateFilter = DataViewRowState.OriginalRows

        '    'BugID 1044: Neeraj Singh 01-Dec-2009
        '    If HelperFunctions.isNonEmpty(dv) AndAlso dv.Item(0).Row.Item("Priority") Then
        '        LabelPriorityHdr.Visible = True
        '    Else
        '        LabelPriorityHdr.Visible = False
        '    End If
        '    'BugID 1044
        'End If

        ''END Above lines are Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921
        'End BT-521 : 22-April-2010

        If YmcaInformationHasChanged() Then
            EnableSaveCancelButtons()
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
        Else

            DisableSaveCancelButtons()

        End If
        'SetSelectedImageOfDataGrid(DataGridYMCABankInfo, Nothing, "ImageButtonSelect")
        'SetSelectedImageOfDataGrid(DataGridYMCANotesHistory, Nothing, "Imagebutton5")
        'SetSelectedImageOfDataGrid(DataGridYMCAResolutions, Nothing, "Imagebutton3")
        'SetSelectedImageOfDataGrid(DataGridYMCAContact, Nothing, "Imagebutton2")
        'SetSelectedImageOfDataGrid(DataGridYMCAOfficer, Nothing, "Imagebutton1")
    End Sub
    Private Sub PersistYMCAGeneralTab()
        Dim dsYMCAGeneral As DataSet
        Dim drYMCA As DataRow
        dsYMCAGeneral = Session("YMCA General")
        If dsYMCAGeneral Is Nothing Then Exit Sub
        'guiUniqueID
        'This table is expected to contain only one record. If it contains more than one we will only take the first one
        If HelperFunctions.isEmpty(dsYMCAGeneral) Then Exit Sub
        'Anudeep A:2013.03.04-Bt:1233:YRS 5.0-1688:YMCA edit security
        Page.Validate()
        If Not Page.IsValid Then
            Exit Sub
        End If
        drYMCA = dsYMCAGeneral.Tables(0).Rows(0) 'GetRowForUpdation(dsYMCAGeneral.Tables(0), "guiUniqueID", Convert.ToString(Session("YMCAGeneralUniqueID")))

        If drYMCA("chvymcaname").ToString.Trim <> TextBoxYMCAName.Text.Trim Then
            drYMCA("chvymcaname") = TextBoxYMCAName.Text.Trim
        End If

        'BugID 1044: Neeraj Singh 01-Dec-2009
        'Start : Added By Dilip Yadav : 2009.10.30
        If drYMCA("Priority").ToString() <> checkboxPriority.Checked.ToString() Then
            'If Convert.ToBoolean(drYMCA("Priority")) <> checkboxPriority.Checked Then
            drYMCA("Priority") = checkboxPriority.Checked
        End If
        'End : Added By Dilip Yadav : 2009.10.30
        'EndBugID 1044
        'Start : Bala01/19/2019: : YRS-AT-2398: Reassigning values.
        If drYMCA("EligibleTrackingUser").ToString() <> checkboxEligibleTrackingUser.Checked.ToString() Then
            drYMCA("EligibleTrackingUser") = checkboxEligibleTrackingUser.Checked
        End If
        If drYMCA("YNAN").ToString() <> checkboxYNAN.Checked.ToString() Then
            drYMCA("YNAN") = checkboxYNAN.Checked
        End If
        If drYMCA("MidMajors").ToString() <> checkboxMidMajors.Checked.ToString() Then
            drYMCA("MidMajors") = checkboxMidMajors.Checked
        End If
        'End : Bala01/19/2019: : YRS-AT-2398: Reassigning values.
        If drYMCA("chrymcano").ToString.Trim <> TextBoxBoxYMCANo.Text.Trim Then
            drYMCA("chrymcano") = TextBoxBoxYMCANo.Text.Trim
        End If
        If drYMCA("chrtaxnumberfederal").ToString.Trim <> TextBoxFedTaxId.Text.Trim Then
            drYMCA("chrtaxnumberfederal") = Me.TextBoxFedTaxId.Text.Trim
        End If
        If drYMCA("chrtaxnumberstate").ToString.Trim <> TextBoxStateTaxId.Text.Trim Then
            drYMCA("chrtaxnumberstate") = TextBoxStateTaxId.Text.Trim
        End If
        Dim dbDate, uiDate As String
        If drYMCA.IsNull("dtsEntryDate") = False Then
            dbDate = CType(drYMCA("dtsEntryDate"), DateTime).ToString("MM/dd/yyyy")
        Else
            dbDate = String.Empty
        End If
        If TextBoxEnrollmentDate.Text.trim = String.Empty Then
            uiDate = String.Empty
        Else
            uiDate = CType(TextBoxEnrollmentDate.Text.Trim, DateTime).ToString("MM/dd/yyyy")
        End If
        If dbDate <> uiDate Then
            drYMCA("dtsEntryDate") = Me.TextBoxEnrollmentDate.Text.Trim
        End If

        If drYMCA("chvDefaultPaymentCode").ToString.Trim.ToUpper <> Me.DropDownPaymentMethod.SelectedValue Then
            If Me.DropDownPaymentMethod.SelectedIndex = 0 Then
                drYMCA("chvDefaultPaymentCode") = String.Empty
            Else
                drYMCA("chvDefaultPaymentCode") = Me.DropDownPaymentMethod.SelectedValue
            End If
        End If
        If drYMCA("chvBillingMethodCode").ToString.Trim.ToUpper <> Me.DropDownBillingMethod.SelectedValue Then
            If Me.DropDownBillingMethod.SelectedIndex = 0 Then
                drYMCA("chvBillingMethodCode") = String.Empty
            Else
                drYMCA("chvBillingMethodCode") = Me.DropDownBillingMethod.SelectedValue
            End If
        End If
        If drYMCA("Chrhubind").ToString.Trim.ToUpper <> Me.DropDownHubType.SelectedValue Then
            If Me.DropDownHubType.SelectedIndex = 0 Then
                drYMCA("Chrhubind") = String.Empty
            Else
                drYMCA("Chrhubind") = Me.DropDownHubType.SelectedValue
            End If
        End If

        'Shashi Shekhar:17-June-2011:for YRS 5.0-1334:Add field for Y-Relations Manager
        If drYMCA("RelationManager").ToString.Trim <> hdrelationManagerName.Value.Trim Then
            drYMCA("RelationManager") = hdrelationManagerName.Value.Trim
        End If

        Session("YMCA General") = dsYMCAGeneral
    End Sub

    Private Sub MultiPageYMCA_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MultiPageYMCA.SelectedIndexChange

    End Sub

    'Start -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code
    'Private Sub DropdownYerdiAccess_T_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownYerdiAccess_T.SelectedIndexChanged
    '    'If (ButtonWithdraw.Enabled = False And ButtonTerminate.Enabled = True) Then
    '    '	'Priya 25-Jan2011 BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)
    '    '	'Session("Withdrawn") = True
    '    '	Session("TerminateYMCA") = True
    '    'ElseIf (ButtonWithdraw.Enabled = False And ButtonTerminate.Enabled = False) Then
    '    '	''Priya 25-Jan2011 BT-697 YRS 5.0-1235 : Allow termination of YMCA from active (skipping withdrawal)
    '    '	''Session("TerminateYMCA") = True
    '    '	Session("TerminateYMCA") = Nothing
    '    '	Session("Withdrawn") = Nothing
    '    'End If

    '    'BS:2012.07.13:BT:998:YRS 5.0-1544:allow yerdi access for both withdrawal and termination if user click withdrawal it will ask for withdrawal related validation using session and if user will click termination it will ask for termination validation
    '    If DropdownWithdrawDate.SelectedValue <> String.Empty AndAlso DropdownlistTermination.SelectedValue = String.Empty Then
    '        Session("Withdrawn") = True
    '    ElseIf DropdownWithdrawDate.SelectedValue <> String.Empty AndAlso DropdownlistTermination.SelectedValue <> String.Empty Then
    '        Session("TerminateYMCA") = True
    '    End If
    'End Sub
    'End -Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Commented old code

#Region "Landing Page Test"

    Public Sub PopulateYMCAForLanding()
        Dim l_DataSetYMCAList As DataSet
        Dim l_DataRowYMCAList As DataRow
        Dim l_StringGuiUniqueiD As String



        Clear_session()
        'Priya:23-March-2010:BT-477: Application displaying "Date cannot be blank" message.
        ValidationSummaryYMCA.Enabled = False

        DisableAllTabsExcept(0)
        l_DataSetYMCAList = YMCARET.YmcaBusinessObject.YMCABOClass.SearchYMCAList(Me.TextBoxYMCANo.Text, Me.TextBoxName.Text, Me.TextBoxCity.Text, Me.TextBoxState.Text)
        YMCASearchList = l_DataSetYMCAList
        PopulateYMCAList()

        If HelperFunctions.isEmpty(l_DataSetYMCAList) Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "No matching records found", MessageBoxButtons.OK)
            Exit Sub
        End If
        'BT-477: Application displaying "Date cannot be blank" message.
        ValidationSummaryYMCA.Enabled = True
        'End  BT-477
        Me.DataGridYMCA.SelectedIndex = 0

        If HelperFunctions.isNonEmpty(l_DataSetYMCAList) Then
            If Not DataGridYMCA.SelectedItem Is Nothing Then
                l_DataRowYMCAList = HelperFunctions.GetRowForUpdation(l_DataSetYMCAList.Tables(0), "guiUniqueId", DataGridYMCA.SelectedItem.Cells(1).Text)
                l_StringGuiUniqueiD = l_DataRowYMCAList.Item("guiUniqueId")
                'storing the Guid in session to be used in address popup 
                Session("GuiUniqueId") = l_StringGuiUniqueiD
                If l_DataRowYMCAList.Item("Type") = "M" Then
                    Me.YMCATabStrip.Items(4).Enabled = True
                Else
                    Me.YMCATabStrip.Items(4).Enabled = False
                End If
                'Populating all textboxes and dropdowns and checkboxes of general tab 
                PopulateDataIntoGeneralControls(l_StringGuiUniqueiD)
                EnableAllTabs()
            End If
        End If

    End Sub




    Public Sub LandingPageRedirection()
        Dim l_DataSetYMCAList As DataSet
        Dim l_DataTableYMCAList As DataTable
        Dim l_DataRowYMCAList As DataRow
        Dim l_StringGuiUniqueiD As String
        Dim l_StringType As String
        Dim l_String
        Dim l_string_Ymcaname As String

        Try
            Me.DataGridYMCA.SelectedIndex = 0
            Clear_session()
            Session("List_Index") = Me.DataGridYMCA.SelectedIndex
            'Priya 27-Jan-2011 : BT-719  Previous Withdrawan YMCA details displaying.
            'Showing fund status grid on termination of ymca
            If Session("Withdrawn") = Nothing AndAlso Session("TerminateYMCA") = Nothing Then
                LabelEmpRecord.Text = ""
                DataGridSummaryReport.DataSource = Nothing
                DataGridSummaryReport.DataBind()
                DatagridSummaryReportOnWithdrawal.DataSource = Nothing
                DatagridSummaryReportOnWithdrawal.DataBind()
            End If
            'END 27-Jan-2011 : BT-719  Previous Withdrawan YMCA details displaying.
            l_DataSetYMCAList = YMCASearchList
            'Go to General Tab
            Me.MultiPageYMCA.SelectedIndex = 1
            Me.YMCATabStrip.SelectedIndex = 1
            'selected row
            'Then Select active row from data grid - Preeti IssueId:YRST-2092/90
            g_integer_count = DataGridYMCA.SelectedIndex
            Session("dataset_index") = g_integer_count

            'Now select row for selected guid- Preeti IssueId:YRST-2092/90
            Dim arrDr() As DataRow = l_DataSetYMCAList.Tables(0).Select("guiUniqueId ='" & Me.DataGridYMCA.SelectedItem.Cells(1).Text.Trim() & "'")
            If arrDr.Length > 0 Then
                l_DataRowYMCAList = arrDr(0)
            Else
                Exit Sub
            End If

            'Changed By:preeti On:9thFeb06 IssueId:YRST-2092/90 Mentioned For person module But found In YMCA. End
            l_StringGuiUniqueiD = l_DataRowYMCAList.Item("guiUniqueId")

            'storing the Guid in session to be used in address popup 
            Session("GuiUniqueId") = l_StringGuiUniqueiD

            If l_DataRowYMCAList.Item("Type") = "M" Then
                Me.YMCATabStrip.Items(4).Enabled = True
            Else
                Me.YMCATabStrip.Items(4).Enabled = False
            End If
            PopulateDataIntoGeneralControls(l_StringGuiUniqueiD)
        Catch ex As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

#End Region

#Region "Y-Relation Managers"
    'Function to populate Ymca Relation Managers:  For BT-844, YRS 5.0-1334:Add field for Y-Relations Manager
    Private Sub PopulateYMCARelationManagers()
        Dim dsRelationManagers As DataSet
        dsRelationManagers = YMCARET.YmcaBusinessObject.YMCABOClass.GetYMCARelationManagers()
        If HelperFunctions.isNonEmpty(dsRelationManagers) Then
            If dsRelationManagers.Tables(0).Rows.Count > 0 Then
                gvRelationManagers.DataSource = dsRelationManagers
                gvRelationManagers.DataBind()
            End If
        End If
    End Sub

#End Region

    'Start:Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security
    Public Sub EnableDisableControls(ByVal boolValue As Boolean)
        Try
            Me.TextBoxFedTaxId.Enabled = boolValue
            Me.TextBoxYMCAName.Enabled = boolValue
            Me.TextBoxBoxYMCANo.Enabled = boolValue
            Me.TextBoxEnrollmentDate.Enabled = boolValue
            Me.ButtonEditFedTaxId.Enabled = Not boolValue
        Catch
            Throw
        End Try
    End Sub
    'End:Anudeep:09.01.2013-Bt-1233:YRS 5.0-1688:YMCA edit security

    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added method to load selected tab
    Public Sub LoadWTSMRGrid(ByVal tabName As String)
        ClientScript.RegisterStartupScript(GetType(Page), "TabChange1", "TabDisplay('" & tabName.ToString & "');", True)
    End Sub
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Added method to load selected tab

    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Declared property to set selected tab with change in controls in hidden field
    Public Property WTSM_Operation() As String
        Get
            Return hdnOperationPerformed.Value
        End Get
        Set(ByVal Value As String)
            hdnOperationPerformed.Value = Value
        End Set
    End Property
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Declared property to set selected tab with change in controls in hidden field

    'Start - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Declared method to hide all the datagrids
    Public Sub HideSummaryDatagrid(ByRef datagrid As DataGrid)
        Try
            If datagrid IsNot Nothing Then
                datagrid.DataSource = Nothing
                datagrid.DataBind()
                datagrid.Visible = False
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'End - Manthan Rajguru | 2016.02.03 | YRS-AT-2334 & 1686 | Declared method to hide all the datagrids

    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    Public Sub LoadWaivedParticipants(ByVal dtWaivedParticipants As DataTable)
        Try
            Me.WarningMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(MESSAGE_META_MESSAGES_WAIVED_PARTICIPANT_MESSAGE).DisplayText
            ViewState("dtWaivedParticipantslist") = dtWaivedParticipants
            If HelperFunctions.isNonEmpty(dtWaivedParticipants) Then
                If dtWaivedParticipants.Rows.Count > 0 Then
                    objRDLCReportDataSource = New ReportDataSource("dsWaivedParticipants", dtWaivedParticipants)
                    ReportViewerWaivedParticipantList.LocalReport.ReportPath = "Reports\WaivedParticipants.rdlc"
                    objRDLCReportParameter.Name = "rdlcWariningMessageText"
                    objRDLCReportParameter.Values.Add(Me.WarningMessage)
                    ReportViewerWaivedParticipantList.LocalReport.SetParameters(objRDLCReportParameter)
                    ReportViewerWaivedParticipantList.LocalReport.DataSources.Clear()
                    ReportViewerWaivedParticipantList.LocalReport.DataSources.Add(objRDLCReportDataSource)
                    ReportViewerWaivedParticipantList.LocalReport.Refresh()
                End If
            End If
        Catch ex As Exception
            Dim strExceptionMessage As String = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", strExceptionMessage))
        End Try
    End Sub

    Private Sub ButtonWaivedProceed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonWaivedProceed.Click
        Dim strErrorMessage As String
        Try
            Me.IsProceedWaived = True
            ButtonSave_Click(Me, e)
            strErrorMessage = GeneratePDFforIDM()
            If Not strErrorMessage Is Nothing Then
                CopyPDFToIDM()
            End If
        Catch ex As Exception
            Dim strExceptionMessage As String = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", strExceptionMessage))
        End Try
    End Sub

    Private Sub ButtonWaivedCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonWaivedCancel.Click
        Try
            ClientScript.RegisterStartupScript(GetType(Page), "CancelWaivedParticipantslist", "CancelWaivedParticipantsListDialog();", True)
        Catch ex As Exception
            Dim strExceptionMessage As String = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", strExceptionMessage))
        End Try
    End Sub

    Private Function GeneratePDFforIDM() As String
        Dim par_Arrylist As New ArrayList
        Dim IDMAll As New IDMforAll
        Dim strErrorMessage As String = String.Empty
        Dim intIDMTrackingId As String
        Try
            If Session("FTFileList") Is Nothing Then
                If IDMAll.DatatableFileList(False) Then
                    Session("FTFileList") = IDMAll.SetdtFileList
                End If
            End If
            If Not Session("FTFileList") Is Nothing Then
                IDMAll.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            End If
            dictReportParameters.Add("rdlcWariningMessageText", Me.WarningMessage)
            IDMAll.ReportDataTable = DirectCast(ViewState("dtWaivedParticipantslist"), DataTable)
            IDMAll.PreviewReport = True
            IDMAll.CreatePDF = True
            IDMAll.CreateIDX = True
            IDMAll.CopyFilesToIDM = True
            IDMAll.YMCAID = DirectCast(Session("GuiUniqueId"), String)
            IDMAll.DocTypeCode = YMCAObjects.IDMDocumentCodes.Waived_Participant_List
            IDMAll.OutputFileType = String.Format("WaivedParticipants_{0}", YMCAObjects.IDMDocumentCodes.Waived_Participant_List)
            IDMAll.ReportType = YMCAObjects.EnumReportType.RDLC.ToString()
            IDMAll.AppType = "A"
            IDMAll.ReportName = "WaivedParticipants"
            IDMAll.RDLCReportParameters = dictReportParameters
            strErrorMessage = IDMAll.ExportToPDF()
            If IDMAll.IDMTrackingId <> 0 Then
                intIDMTrackingId = IDMAll.IDMTrackingId
            End If
            HttpContext.Current.Session("FTFileList") = IDMAll.SetdtFileList
        Catch ex As Exception
            Dim strExceptionMessage As String = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", strExceptionMessage))
        End Try
        Return strErrorMessage
    End Function

    Private Sub CopyPDFToIDM()
        If Not Session("FTFileList") Is Nothing Then
            Try
                'Session("FTFileList") = IDMAll.SetdtFileList
                ' Call the calling of the ASPX to copy the file.
                Dim popupScriptCopytoServer As String = "<script language='javascript'>" & _
                "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupCopytoServer")) Then
                    Page.RegisterStartupScript("PopupCopytoServer", popupScriptCopytoServer)
                End If
                ClientScript.RegisterStartupScript(GetType(Page), "LoadWaivedParticipantslist", "CancelWaivedParticipantsListDialog();", True)
            Catch ex As Exception
                Dim strExceptionMessage As String = Server.UrlEncode(ex.Message.Trim.ToString())
                ExceptionPolicy.HandleException(ex, "Exception Policy")
                Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", strExceptionMessage))
            End Try
        End If
    End Sub
    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)

    'START: Chandra sekar | 2018.05.23 | YRS-AT-3270 | Sending email notification on addition of new contact
    Private Sub SendEmailNotificationOnNewContact(ByVal contactInformation As DataSet)
        Dim mailClient As MailUtil
        Dim dictParameters As Dictionary(Of String, String)
        Try
            For Each row As DataRow In contactInformation.Tables(0).Rows
                mailClient = New MailUtil
                If row.RowState = DataRowState.Added Then
                    If HelperFunctions.isNonEmpty(row.Item("TypeCode")) Then
                        If row.Item("TypeCode").ToString().Trim.ToUpper = "TRANSM" Then ' Sending email notification only for TypeCode (TRANSM) i.e LPA 
                            dictParameters = New Dictionary(Of String, String)
                            dictParameters.Add("YmcaNo", Session("GeneralYMCANo"))
                            dictParameters.Add("YmcaName", TextBoxYMCAName.Text)
                            If Convert.IsDBNull(row.Item("Fund No")) AndAlso String.IsNullOrEmpty(Convert.ToString(row.Item("Fund No"))) Then
                                dictParameters.Add("FundID", "N/A")
                            Else
                                dictParameters.Add("FundID", row.Item("Fund No"))
                            End If
                            dictParameters.Add("Name", String.Format("{0} {1}", row.Item("First Name"), row.Item("Last Name")))
                            dictParameters.Add("Role", row.Item("Type"))
                            dictParameters.Add("EffectiveDate", String.Format("{0:MM/dd/yyyy}", row.Item("Effective Date")))
                            If Convert.IsDBNull(row.Item("Phone No")) AndAlso String.IsNullOrEmpty(Convert.ToString(row.Item("Phone No"))) Then
                                dictParameters.Add("Phone", String.Empty)
                            Else
                                dictParameters.Add("Phone", HelperFunctions.TelephoneInFormat(row.Item("Phone No")))
                            End If
                            dictParameters.Add("EmailID", row.Item("Email"))
                            If mailClient IsNot Nothing Then
                                mailClient.SendMailMessage(EnumEmailTemplateTypes.YMCA_NEW_CONTACT_NOTIFICATION, "", "", "", "", dictParameters, "", Nothing, Mail.MailFormat.Html)
                            End If
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        Finally
            dictParameters = Nothing
            mailClient = Nothing
        End Try

    End Sub
    'END: Chandra sekar | 2018.05.23 | YRS-AT-3270 | Sending email notification on addition of new contact
    'START  Chandra sekar | 2018.05.23 | YRS-AT-3270 | Sending email notification on addition of new office only for CEO ,CHRO/HRVP
    Private Sub SendEmailNotificationOnNewOfficer(ByVal contactInformation As DataSet)
        Dim mailClient As MailUtil
        Dim positionTitleCode As String
        Dim dictParameters As Dictionary(Of String, String)
        Try
           
            For Each row As DataRow In contactInformation.Tables(0).Rows
                mailClient = New MailUtil
                If row.RowState = DataRowState.Added Then

                    If HelperFunctions.isNonEmpty(row.Item("chvPositionTitleCode")) Then
                        positionTitleCode = row.Item("chvPositionTitleCode").ToString().Trim.ToUpper
                        If positionTitleCode = "CEO" OrElse positionTitleCode = "CHRO" OrElse positionTitleCode = "HRVP" Then 
                            dictParameters = New Dictionary(Of String, String)
                            dictParameters.Add("YmcaNo", Session("GeneralYMCANo"))
                            dictParameters.Add("YmcaName", TextBoxYMCAName.Text)
                            If Convert.IsDBNull(row.Item("Fund No")) AndAlso String.IsNullOrEmpty(Convert.ToString(row.Item("Fund No"))) Then
                                dictParameters.Add("FundID", "N/A")
                            Else
                                dictParameters.Add("FundID", row.Item("Fund No"))
                            End If
                            dictParameters.Add("Name", String.Format("{0} {1}", row.Item("First Name"), row.Item("Last Name")))
                            dictParameters.Add("Role", row.Item("Title"))
                            dictParameters.Add("EffectiveDate", String.Format("{0:MM/dd/yyyy}", row.Item("Effective Date")))
                            If Convert.IsDBNull(row.Item("Phone No")) AndAlso String.IsNullOrEmpty(Convert.ToString(row.Item("Phone No"))) Then
                                dictParameters.Add("Phone", String.Empty)
                            Else
                                dictParameters.Add("Phone", HelperFunctions.TelephoneInFormat(row.Item("Phone No")))
                            End If
                            dictParameters.Add("EmailID", row.Item("Email"))
                            If mailClient IsNot Nothing Then
                                mailClient.SendMailMessage(EnumEmailTemplateTypes.YMCA_NEW_OFFICER_NOTIFICATION, "", "", "", "", dictParameters, "", Nothing, Mail.MailFormat.Html)
                            End If
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        Finally
            dictParameters = Nothing
            mailClient = Nothing
        End Try

    End Sub
    'END: Chandra sekar | 2018.05.23 | YRS-AT-3270 | Sending email notification on addition of new office only for CEO ,CHRO/HRVP

    'START: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim toolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText

            For Each row In DataGridYMCANotesHistory.Items
                Dim lnkbtndelete As LinkButton = (TryCast((TryCast(row, TableRow)).Cells(6).Controls(1), LinkButton))
                Dim chkmarkimp As CheckBox = (TryCast((TryCast(row, TableRow)).Cells(5).Controls(1), CheckBox))
                lnkbtndelete.Enabled = False
                lnkbtndelete.ToolTip = toolTip
                chkmarkimp.Enabled = False
                chkmarkimp.ToolTip = toolTip

            Next
        End If
    End Sub
    'END: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

End Class
#Region "Popup Classes"
Public Class Resolution
    Public Sub New()
        MyBase.New()
    End Sub

    Public VestingType As String
    Public VestingTypeDesc As String
    Public SlidingScalePerc As String
    Public AddlYMCAPerc As String
    Public ResolutionType As String
    Public ResolutionTypeDesc As String
    Public ParticipantPerc As String
    Public YMCAPerc As String
    Public EffectiveDate As DateTime
    Private m_TermDate As String = Nothing
    Public Property TermDate() As String
        Get

            Return m_TermDate
        End Get
        Set(ByVal Value As String)
            m_TermDate = Value
        End Set
    End Property

End Class
Public Class PopupResult
    Public Sub New()
    End Sub
    Public Action As ActionTypes
    Public State As Object
    Public Page As String
    Public Enum ActionTypes
        ADD
        CANCEL
    End Enum
End Class
Public Class Metro
    Public MetroGuiUniqueiD As String
    Public YmcaMetroName As String
End Class
Public Class Telephone
    Public TelephoneNo As String
End Class
Public Class EmailAddress
    Public EmailAddress As String
End Class
Public Class Officer
    Public OfficerId As String
    Public Name As String
    Public Title As String
    Public TitleType As String
    Public Telephone As String
    Public ExtnNo As String
    Public Email As String
    Public EffectiveDate As String
    'Shashi Shekhar:2010-03-05:Added for Yrs-5.0-942
    Public FirstName As String
    Public MiddleName As String
    Public LastName As String
    Public FundNo As String
End Class
Public Class Contacts
    Public ContactId As String
    Public Type As String
    Public Name As String
    Public Telephone As String
    Public ExtNo As String
    Public EffectiveDate As String
    Public Email As String
    Public Note As String

    'Shashi Shekhar:2010-03-05:Added for Yrs-5.0-942
    Public FirstName As String
    Public MiddleName As String
    Public LastName As String
    'prasad:2011-10-31 for YRS 5.0-1379 : New job position field in atsYmcaContacts
    Public TypeValue As String
    Public FundNo As String
    'Added by prasad for YRS 5.0-1379 : New job position field in atsYmcaContacts
    Public Title As String
    'Added by prasad for YRS 5.0-1503 : New job position field in atsYmcaContacts
    Public TitleDescription As String
End Class
Public Class Bank
    Public Id As String
    Public BankId As String
    Public Name As String
    Public ABANumber As String
    Public AccountNumber As String
    Public PaymentMethod As String
    Public AccountType As String
    Public EffectiveDate As String
End Class
Public Class Notes
    Public NoteId As String
    Public BitImportant As Boolean
    Public NotesText As String
End Class
#End Region
