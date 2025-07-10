
#Region "Modification history"
'************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		    : YMCA-YRS
' FileName			    : RetireesInformationForm.aspx.vb
' Cache-Session         : Vipul 02Feb06
' Program Specification : YMCA_PS_Maintenance_Person(Retiree)
'************************************************************************************
'Modification History
'************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'Mohammed Hafiz     11-Dec-2006     YREN-2977
'Shubhrata          17-Aug-2006     YREN-2638 Reason:2 bank records with same effective date producing 2 checks   
'Shubhrata          30-Oct-2006     YREN-2799 if country code is CA then allow alphanumeric char in Zip,if US then allow only numeric for Zip  
'Ashutosh Patil     24-Jan-2007     YREN-2979
'Ashutosh Patil     29-Jan-2007     YREN-3028,YREN-3029
'Aparna Samala      05-Feb-2007     YREN-3037-Change the text on button
'Ashutosh Patil     12-Feb-2007     YREN-3028,YREN-3029  
'Ashutosh Patil     15-Feb-2007     YREN-3028,YREN-3029  
'Ashutosh Patil     23-Feb-2007     YREN-3028,YREN-3029 YREN -3079
'Ashutosh Patil     01-Mar-2007     Changing spellings and Address1 validations and City Validations
'Shubhrata          02-Mar-2007     YREN-3112 we are not supposed to include Unknown Marital Status for Federal Tax Withholding for all Tax purposes
'Ashutosh Patil     06-Mar-2007     YREN-3099
'Aparna  Samala     21-Mar-2007     YREN-3115
'Ashutosh Patil     22-Mar-2007     YREN-3222 
'Ashutosh Patil     23-Mar-2007     YREN-3222 
'Ashutosh Patil     25-Apr-2007     YREN-3298 
'Mohammed Hafiz     02-May-2007     YREN-3112
'Shubhrata          16-May-2007     Plan Split Changes add a hyperlink to allow the user to see Active Person's record in case of RT,RA,RD and DR status.
'Ashutsosh Patil    21-Jun-2007     YREN-3490 ==> SSNo Duplication Message Box
'Ashutosh Patil     09-Jul-2007     For Avoiding runtime error if after deletion of record of datagrid of                                             Beneficiaries' edit item button is directly  
'Priya              13-April-2010   As per Hafiz Mail Send on:12April-2010 :Issues identified with 7.4.2 code release
'prasad jadhav		2012.01.19		For BT-925,YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
'Shilpa N           2018.03.08      YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'**********************************************************************************************************************
'
'Name:Preeti Date:7thFeb06 IssueId:2053 Reason:Message Box text change
'Name:Preeti Date:20thFeb06 IssueId:YRST-2072 Reason:Unable To Add Beneficiary
'Name:Ashutosh Patil:10th To 11th Jan 2006 IssueId:YREN-2979 Reason:For Current Pay Method the most effective date 
'against the login date will be considered and status for that date will be displayed for Current Pay Method  
'This change will be in the Function SaveInfo and BankInfoShowCheck
'
'Nikunj Patel       26-Jun-2007     Plan Split changes - Rewriting code for efficiency and performance. 
'                                   Handling changes for Beneficiary Designation.
'                                   Removed code for active participants' Beneficiary information.
'Nikunj Patel       07-Aug-2007     Save and Cancel buttons were enabled even if no beneficiaries were selected for editing or deleting
'                                   The selected beneficiary is deselected when the Cancel button is pressed.
'Aparna Samala      05-Sep-2007     Modified added new buttons update and delete for the JS Beneficiary modification,changed the logic to show the pop up for Annuities
'Aparna Samala      13-Sep-2007     Check for Phony SSno in Annuities tab for Annuity Beneficiary.
'Nikunj Patel       03-Oct-2007     Updated code to handle the error code properly during Death Notification so that error messages are shown properly.
'Aparna Samala      11/10/2007      Added the code in the page load when the Yes of Message box in Annuity tab is pressed..no functinoality was there
'Aparna Samala      03/12/2007      Should not allow to edit beneficiary if there is no  beneficiary mentioned.
'Swopna Valappil    28-Dec-2007     On selecting an item from DataGridRetiredBeneficiaries,DataGridGeneralWithhold and DataGridFederalWithholding ,image of image button changes from select.gif to selected.gif.(Bug id 321)
'Swopna Valappil    07-Jan-2008     Problem with salutation pick up from database and matching the same with dropdownlist values solved.
'Swopna Valappil    07-Jan-2008     DataGridNotes_SortCommand added in response to YREN-4126
'Swopna Valappil    16-Jan-2008     YREN-4126 reopened.Notes in ascending order of date.
'Shubhrata          12-Feb-2008     YRPS-4614
'Mohammed Hafiz     21-Feb-2008     Handling null values in the chrEFTStatus field as reported by Purushottam while testing another issue.
'Mohammed Hafiz                     YRPS-4659 change is in the .aspx file
'Nikunj Patel       15-Apr-2008     Change in the calling of Death Notification process for ML status.
'Swopna             25-Apr-2008     Bug id-389 (Phase IV changes)
'Mohammed Hafiz     27-Mar-2008     YRPS-4704 - Should not Allow the Beneficiary code of SP if person is single
'Swopna             20-May-2008     BT-389(Part 1 -Address problem) 
'Swopna             20-May-2008     Include 'RPT',BT-434(ButtonActivateAsPrimary's causesvalidation property set to true, change made in .aspx file)
'Swopna             21-May-2008     YRPS-4704
'Nikunj Patel       23-Jun-2008     YRS-5.0:463 - Updates for Annuity Beneficiaries not being saved
'Swopna             30-Jul-2008     YRS-5.0:463 - Disable DOB field of annuity beneficiary
'Priya              10-Sep-2008     BT-546 - Updating code to check if death notification can be performed on the selected fund event
'Nikunj Patel       10-Sep-2008     BT-550 - Updating code to store the SSN number in Person Info session variable that is used on the UpdateBeneficiary page.
'Dilip              24-sep-2008              To refresh the Qdro Request information when new QDRO request is raised
'Priya              14-Nov-2008     Change IDM path as per Ragesh mail as on 11/13/2008
'Ashish             06-Apr-2009     Remove Fund Id from Vignette URL
'NP/PP/SR       	2009.05.18      Removing unused code and control references
'Dilip Yadav        2009.09.08      YRS 5.0-852
'Dilip Yadav        2009.09.21      To modify the code which allows only 50 characters in Notes grid while in View mode it will show the complete text.
'Dilip Yadav        2009.09.22      YRS 5.0.896 : Re-enable View Details in YRS Banking tab for Retirees 
'Dilip Yadav        01-Oct-2009     YRS 5.0.896 : To modify the code for newly added last column "Account Type"
'Dilip Yadav        08-Oct-2009     YRS 5.0-911 : Currency was not selected after check it should be same as first entry and should be changed again.
'Dilip Yadav        2009.10.28      To provide the feature of "Priority Handling" as per YRS 5.0.921
'Neeraj Singh                   18/Nov/2009         Added form name for security issue YRS 5.0-940 
'Sanjay Rawat       2010.1.04       YRS 5.0:635  
'Shashi Shekhar     2010.01.27       Added function RetrieveData() To update the bitIsArchived field in atsPerss table
'Priya              18-Feb-2010     YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
'Ashish Srivastava  2010.03.12      Thread being aborted error, if there is no active beneficiary
'Priya              24-March-2010   BT:479: While adding QDRO Request page not getting Refresh.
'Shashi Shekhar     26-March-2010   BT:488: Confirmation message needs to change in Person Maintenance screen.
'Priya              29-March-2010   BT-476:Allowing to delete Beneficiary even the User don't have rights to delete. 
'Priya				5-April-2010	YRS 5.0-1042 : fetch value of newly aaded column bitYmcaMailOptOut
' Shashi Shekhar	2010.04.07	    Handling DataArchive error message (concate sql error message with user defined error message returned from procedure)
' Shashi Shekhar    2010-04-12      Ref:mail-Issues identified with 7.4.2 code release - Internally identified #1 -- Remove ref variable as output ,used in  RetrieveData function
'Ashish Srivastava  2010-04-20      fix Issue YRS 5.0-1056 
'Priya/Ashish       21-April-2010   YRS 5.0-1056: Additional info when unsuppressing a JSANN annuity
'Sanjay R.          20-April-2010   YRS 5.0-1055 - for Null value assigning Gendertype as UNKNOWN
'Priya/Ashish       22-April-10     YRS 5.0-1056: Additional info when unsuppressing a JSANN annuity
'Priya              23-April-2010   BT -520: In Retiree maintenance page priority handling check box is not selected
'Sanjay Rawat       2010.05.10      YRS 5.0-1078 - To add beneficiary info for participants who have status of QD. 
'Priya              12-May-2010     YRS 5.0-1073 :commented  ShareInfoAllowed code
'Imran              15/06/2010      Changes for Enhancements
'Priya              19-July-2010    Integrated into enhancement 10-June-2010:YRS 5.0-1107:YMCA Mailing Opt-Out should appear on the Retiree screen too. 
'Imran              30-July-2010    YRS 5.0-1141 add Age format to the person maintanace screen.
'Imran              03-July-2010    BT-865 Age shows wrong after Deceased date entry.
'Priya              18-Aug-2010     YRS 5.0-1141:Please add Age to the person maintanace screen.
'Priya              06-Sep-2010     BT-599,YRS 5.0-1126:Address update for non-US/non-Canadian address.
'Shashi Shekhar:    26-Oct-2010:    For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
'Priya				10-Dec-2010:		YRS 5.0-1177/BT 588 Changes made as sate n country fill up with javascript in user control
'Shashi Shekhar     09- feb-2011    For YRS 5.0-1236 : Need ability to freeze/lock account
'shashi shekhar     18-feb-2011     For Mandatory field validation message needs to display first.
'Shashi             13-Apr.2011     YRS 5.0-877 : Changes to Banking Information maintenance.
'Sanjay R.		    2011.04.26      YRS 5.0-1292: A warning msg should appear for the participant whose annuity disursement after moth of death is not voided									
'Bhavna Shrivastav  2011.07.08      YRS 5.0-1354: change bit field caption bitYmcaMailOptOut into bitPersonalInfoSharingOptOut
'Bhavna Shrivastav  2011.07.11      YRS 5.0-1354: add new bit field  bitGoPaperLess into atsperss
'Bhavna Shrivastav  18 july 2011    YRS 5.0-1339 : handle X value on MaritalStatus Dropdown when record is non actuary on loadGeneraltab()
'Bhavna Shrivastav  19 july 2011    YRS 5.0-1339:handle X value on gender Dropdown
'shagufta           20 July 2011    For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance
'Bhavna Shrivastav  22 july 2011    Reopen POA Issue show all active poa based on termination date > than current date effective date < than current date, termination date is null ,l_DataSet_POA.  Handled BT-902 as well.
'Shagufta Chaudhari 27 July 2011    For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance(Disable 'Save' and 'Cancel' button)
'Bhavna Shrivastav  2011.08.11      Revert :YRS 5.0-1339
'prasad j           2011.08.10      For BT-919 YRS 5.0-1387 : Display IAT payment method on banking tab
'Prasad Jadhav      2011.08.26      For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
'Prasad Jadhav      2011.09.14      For BT-895,YRS 5.0-1364 : prompt user to if changes not saved 
'Prasad Jadhav      2011.09.27      For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
'BhavnS             2012.01.18      YRS 5.0-1497 -Add edit button to modify the date of death
'prasad jadhav		2012.01.20		For BT-925,YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
'prasad jadhav		2012.01.24		For BT-950,YRS 5.0-1469: Add link to Web Front End
'prasad jadhav		2012.01.31		For BT-950,YRS 5.0-1469: Add link to Web Front End
'Bhavna S		    2012.02.01		For BT-941,YRS 5.0-1432:New report of checks issued after date of death
'BhavnS             2012.03.03      YRS 5.0-1497 -Add edit button to modify the date of death
'BhavnS             2012.03.13      BT:993:-Deceased Date field getting enabled after updating the Deceased Date
'BhavnS             2012.03.21      For BT-1015,YRS 5.0-1557: Death Notification Button disappears
'Prasad jadhav		2012.04.10		For:BT:1018:YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
'Priya				28-May-2012		YRS 5.0-1576: update marital status if spouse beneficiary entered
'BhavnaS            2012.04.23	    YRS 5.0-1470: Link to Address Edit program from Person Maintenance & address code restructure
'BhavnaS            2012.07.18	    YRS 5.0-1470: Link to Address Edit program from Person Maintenance & address code restructure(remove selected index tab)
'Anudeep            2012.11.02      BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen 
'Anudeep            2012.11.14      Reverted changes made for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen 
'Dinesh K           2012.12.05      Bt-1454/YRS 5.0-1731:It is creating blank records in atstelephones table.  
'Dinesh K           2012.12.17      BT-1409/Miscellaneous Issues
'Dinesh K           2012.12.12      BT-1236/YRS 5.0-1685:Add Category/Type field to Power of attorney and allow 3 types
'Dinesh K           2012.12.27      BT-1266/YRS 5.0-1698:Cross check SSN when entering a date of death             
'Dinesh K           2013.02.12      BT-1261/YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
'Anudeep            2013.03.14      Changed Code for telephone Usercontrol
'Anudeep            01.07.2013 		BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Sanjay R.          2013.06.10      BT-2068/YRS 5.0-2121 - Changed doccode from 07DTHBENAP to DTHBENAP for Deathbenefitapplicationform
'Anudeep            2013.06.11      BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
'Anudeep            2013.06.21      BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            2013.06.24      Changed for not to occur error when beneficiaries not defined
'Anudeep            2013.07.02      Bt-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            2013.08.21      YRS 5.0-2162:Edit button for email address 
'Anudeep            2013.08.22      YRS 5.0-1862:Add notes record when user enters address in any module.
'Anudeep            2013.08.22      YRS 5.0-1769:Length of phone numbers
'Anudeep            2013.08.27      BT-1555:YRS 5.0-1769:Length of phone numbers
'Dineshk            2013.09.18      YRS 5.0-2204:Cannot see other annuity beneficiaries in Annuities display
'Anudeep            2013.09.24      Bt-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            2013.10.22      BT-2236:After modifying address save button gets disabled
'Anudeep            2013.10.21      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Anudeep            2013.10.23      BT-2264:YRS 5.0-2232:Notes entry in person maintanance screen.
'Anudeep            2013.11.06      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Anudeep            2013.11.07      BT:2190-YRS 5.0-2199:Create view mode for Power of Attorney display 
'Anudeep            2013.11.07      BT:2269-YRS 5.0-2234:Change labelling of Power of Attorney / POW 
'Anudeep            2013.11.15      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Shashank			2013.12.23		BT:2131/YRS 5.0-2161 : Modifications to Annuity Purchase process 
'Anudeep A          2014.02.03      BT:2292:YRS 5.0-2248 - YRS Pin number 
'Shashank			2014.02.05		BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN 
'Anudeep A          2014.02.13      BT:2316:YRS 5.0-2262 - Spousal Consent date in YRS
'Anudeep A          2014.02.14      BT:2292:YRS 5.0-2248 - YRS Pin number 
'Anudeep A          2014.02.16      BT:2291:YRS 5.0-2247 - Need bitflag that will not allow a participant to create a web account 
'Anudeep            2014.02.16      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.02.20      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.05.26      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Shashank           2014.06.13      BT-2571 : Error while death notification for Retiree QDRO particpant
'Anudeep            2014.06.11      BT:2554:YRS 5.0-2375 : Notes display if you have spaces in name.
'Anudeep            2014.06.24      BT:2582:Maintenance - Retiree has two secured entry to add beneficiaries. 
'Anudeep A          2014.08.13      BT-1409 : Miscellaneous Issues for Security Rights
'Anudeep A          2014.09.03      BT-1409 : Miscellaneous Issues for Security Rights
'Shashank P         2014.11.27      BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
'Anudeep            2015.05.05      BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite
'Anudeep            2015.26.06      BT:2906 : YRS 5.0-2547: YRS issue-cannot access participant account (when contingent Beneficiary exists)
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale   2015.10.07      YRS-AT-2361: Need to prevent Duplicate banking records from being created 
'Pramod P. Pokale   2015.10.12      YRS-AT-2588: implement some basic telephone number validation Rules
'Anudeep A          2015.10.07      YRS-AT-2361: Need to prevent Duplicate banking records from being created 
'Manthan Rajguru    2015.10.21      YRS-AT-2182: limit effective date for address updates -Do not allow address updates with an effective date in the future.
'Manthan Rajguru    2015.10.30      YRS-AT-2583:YRS enh: Person Maintenance - customize Error message for old legacy accounts (data in Ingres)
'Bala               2016.01.05      YRS-AT-1972: Special death processing required.
'Bala               2016.01.12      YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Bala               2016.01.19      YRS-AT-2398   Customer Service requires a Special Handling alert for officers
'Anudeep            2016.02.15      YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Manthan Rajguru    2016.02.24      YRS-AT-2328 - Deactivate secondary address
'Bala               2016.03.08      YRS-AT-1718 - Adding Notes - YMCA Maintenance
'Bala               2016.03.16      YRS-AT-2398  Customer Service requires a Special Handling alert for officers.
'Manthan Rajguru    2016.04.01      YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Manthan Rajguru    2016.04.05      YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Manthan Rajguru    2016.04.06      YRS-AT-2328 - Deactivate secondary address
'Pramod P. Pokale   2016.04.20      YRS-AT-2719 - Applying Fees and deductions to death payments - Part A.2 
'Santosh Bura       2016.07.07      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annuity beneficiaries SSN
'Manthan Rajguru    2016.07.18      YRS-AT-2919 -  YRS Enh: Beneficiary Details - nonPerson - should allow optional Date
'Manthan Rajguru    2016.07.27      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.     
'Pramod P. Pokale   2016.08.01      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annuity beneficiaries SSN
'Manthan Rajguru    2016.08.02      YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annuity beneficiaries SSN
' Santosh Bura		2016.09.21		YRS-AT-3028 -  YRS enh-RMD Utility - married to married Spouse beneficiary adjustment(TrackIT 25233)
'Manthan Rajguru    2016.10.07      YRS-AT-3062 -  YRS enh-Limit permissions for Exhausted RMD/DB Efforts box (TrackIT 26963)
'Manthan Rajguru    2016.10.14      YRS-AT-3063 -  YRS enh-generate warning for Exhausted DB/RMD Settlement Efforts (TrackIT 26868)
'Manthan rajguru    2017.04.18      YRS-AT-3384 - YRS Bug - Address changes reArch: Insert into atsAddrs and reset all prior record bit flags to zero 
'                                   YRS-AT-3385 -  YRS Bug - Address changes reArch: remove EffectiveDate from comparison code
'Santosh Bura       2017.10.11      YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'Manthan Rajguru 	2017.01.20		YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
'Santosh Bura       2018.01.11      YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'Manthan Rajguru    2018.05.03      YRS-AT-3941 - YRS enh: change "phony SSN" beneficiary label to "placeholder SSN" (TrackIT 33287)
'Manthan Rajguru    2018.05.22      YRS-AT-3716 -  YRS bug: cannot add address for non-person beneficiary (ex:trust) without TIN (TrackIT 31562)
'Vinayan C          2018.06.14      YRS-AT-4010 -  Sorting error - Notes Tab 
'Jayaram            2018.08.28      YRS-AT-4031 -  YRS enh: YRS enhancement-Hide Print button for web registration letter (TrackIT 33587)
'Manthan Rajguru    2018.11.23      YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab
'Benhan David       2018.11.20      YRS-AT-4136 - YRS enh: Person Maintenance screen: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
'Megha Lad          2019.09.20      YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing
'Pooja Kumkar       2019.10.10      YRS-AT-4598 -  YRS enh: State Withholding Project - Annuity Payroll Processing 
'Shilpa Nagargoje   2019.11.19      YRS-AT-4604 -  YRS enh: State Withholding Project - Annuities Paid
'Megha Lad          2019.11.27      YRS-AT-4719 - State Withholding - Additional text & warning messages for AL, CA and MA.
'**********************************************************************************************************************
#End Region

Imports System
Imports System.Math
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
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Text.RegularExpressions
Imports YMCAUI.ServiceManager
Imports System.Xml.Serialization
Imports System.Net
Imports System.IO
Public Class RetireesInformationWebForm
    Inherits System.Web.UI.Page

    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("RetireesInformationWebForm.aspx")
    'End issue id YRS 5.0-940
    Dim g_dataset_GeneralInfo As New DataSet
    Dim l_datatable As New DataTable
    Dim l_Str_Msg As String
    Dim l_ds_States As DataSet
    Dim l_DataSet_POA As New DataSet
    Dim l_dt_State As New DataTable
    Dim l_dt_State_filtered As New DataTable
    Dim dr As DataRow

    Public l_string_VignettePath As String '= System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_Participant"), Session("FundNo"))

    'Protected WithEvents ButtonRetireesInfoSave As System.Web.UI.WebControls.Button 'by Aparna 27/08/07
    Protected WithEvents ButtonSaveRetireeParticipants As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRetireesInfoCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRetireesInfoPHR As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRetireesInfoOK As System.Web.UI.WebControls.Button
    Dim l_dr_State As DataRow()
    Dim m_str_Pri_Address1 As String
    Dim m_str_Pri_Address2 As String
    Dim m_str_Pri_Address3 As String
    Dim m_str_Pri_City As String
    Dim m_str_Pri_Zip As String
    Dim m_str_Pri_CountryValue As String
    Dim m_str_Pri_StateValue As String
    Dim m_str_Pri_CountryText As String
    Dim m_str_Pri_StateText As String
    'BS:2012.03.09:-restructure Address Control:Add new ctrl in user control
    Dim m_str_Pri_EffDate As String
    Dim m_str_Sec_Address1 As String
    Dim m_str_Sec_Address2 As String
    Dim m_str_Sec_Address3 As String
    Dim m_str_Sec_City As String
    Dim m_str_Sec_Zip As String
    Dim m_str_Sec_CountryValue As String
    Dim m_str_Sec_StateValue As String
    Dim m_str_Sec_CountryText As String
    Dim m_str_Sec_StateText As String
    'BS:2012.03.09:-restructure Address Control:Add new ctrl in user control
    Dim m_str_Sec_EffDate As String
    Protected WithEvents HyperLinkViewParticipantsInfo As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ButtonSaveParticipants As System.Web.UI.WebControls.Button
    Protected WithEvents ReqGeneralDOB As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ReqValidatorAnnuityDOB As System.Web.UI.WebControls.RequiredFieldValidator 'by Aparna 04/09/2007
    Protected WithEvents ButtonAnnuitiesViewDetails As System.Web.UI.WebControls.Button 'by Aparna 04/09/2007
    Protected WithEvents ButtonUpdateJSBeneficiary As System.Web.UI.WebControls.Button 'by Aparna 04/09/2007
    Protected WithEvents ButtonCancelJSBeneficiary As System.Web.UI.WebControls.Button 'by Aparna 04/09/2007
    Protected WithEvents CheckboxSpouse As System.Web.UI.WebControls.CheckBox 'by Aparna 05/09/2007
    'Start: Bala: 01/19/2019: Control name and hiddenfield is added 
    'Protected WithEvents checkboxPriority As System.Web.UI.WebControls.CheckBox 'Added by Dilip yadav : YRS 5.0.921
    Protected WithEvents checkboxExhaustedDBSettle As System.Web.UI.WebControls.CheckBox
    Protected WithEvents HiddenFieldOfficerDetails As System.Web.UI.WebControls.HiddenField
    Protected WithEvents LabelSpecialHandling As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelPriority As System.Web.UI.WebControls.Label 'Added by Dilip yadav : YRS 5.0.921
    'Protected WithEvents LabelPriorityHdr As System.Web.UI.WebControls.Label 'Added by Dilip yadav : YRS 5.0.921
    Protected WithEvents LabelExhaustedDBSettleHdr As System.Web.UI.WebControls.Label
    'End: Bala: 01/19/2019: Control name and hiddenfield is added 
    Protected WithEvents ValidationSummaryRetirees As System.Web.UI.WebControls.ValidationSummary
    'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
    Protected WithEvents HiddenFieldDirty As System.Web.UI.WebControls.HiddenField
    'BS:2012.01.16:YRS 5.0-1497 -Add edit button to modify the date of death
    Protected WithEvents btnEditDeathDateRet As System.Web.UI.WebControls.Button
    Protected WithEvents HiddenFieldDeathDate As System.Web.UI.WebControls.HiddenField
    'Start: Bala: 01/05/2016: YRS-AT-1972: Added the Special Death Processing Required Checkbox.
    Protected WithEvents chkSpecialDeathProcess As System.Web.UI.WebControls.CheckBox
    'End: Bala: 01/05/2016: YRS-AT-1972: Added the Special Death Processing Required Checkbox.

    'START: PPP | 04/20/2016 | YRS-AT-2719 | Unsuppress annuity popup controls
    Protected WithEvents tdUnSuppressAnnuitySummary As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents dgUnSuppressAnnuity As System.Web.UI.WebControls.GridView
    Protected WithEvents divUnsppressErrorMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtUnSuppressAnnuityTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUnSuppressAnnuityNonTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUnSuppressAnnuityTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUnSuppressAnnuityMonths As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUnSuppressAnnuityGrossAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUnSuppressAnnuityDeductions As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUnSuppressAnnuityNetAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnUnSuppressAnnuitySave As System.Web.UI.WebControls.Button
    Protected WithEvents btnUnSuppressAnnuityCancel As System.Web.UI.WebControls.Button
    'END: PPP | 04/20/2016 | YRS-AT-2719 | Unsuppress annuity popup controls
    Protected WithEvents DivErrorMessage As System.Web.UI.HtmlControls.HtmlGenericControl 'MMR | 2018.11.23 | YRS-AT-3101 | Declared control to show PII error message

    'START: PK |10/10/2019 | YRS-AT-4598 |Declared control to display state withholding error message
    Protected WithEvents divUnsupressStateWithholdingWarning As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents trConfirmYesNo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trNoteUnsuppress As System.Web.UI.HtmlControls.HtmlTableRow
    'END : PK |10/10/2019 | YRS-AT-4598 |Declared control to display state withholding error message
    Dim m_str_Msg As String
    Dim m_str_Fund_Type As String
    Dim m_str_EFT_Status As String
    Dim g_isArchived As Boolean


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonDeathNotification As System.Web.UI.WebControls.Button
    Protected WithEvents txtboxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtboxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtboxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtboxLastName As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtboxCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtboxState As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnFind As System.Web.UI.WebControls.Button
    Protected WithEvents btnClear As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonAddBeneficiaries As System.Web.UI.WebControls.Button  'Anudeep:24.06.2014 BT-2582 : Commented
    Protected WithEvents EmailId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Unsubscribe As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TextOnly As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents BadEmail As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents IframeIDMView As System.Web.UI.HtmlControls.HtmlGenericControl
    'Rahul
    Protected WithEvents ButtonEditJSBeneficiary As System.Web.UI.WebControls.Button
    'Rahul
    Protected WithEvents ButtonActivateAsPrimary As System.Web.UI.WebControls.Button
    'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Declared Control for deactivate button
    Protected WithEvents btnDeactivateSecondaryAddrs As System.Web.UI.WebControls.Button
    'End - Manthan | 2016.02.24 | YRS-AT-2328 | Declared Control for deactivate button

    Protected WithEvents ButtonNotesAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonGeneralEdit As System.Web.UI.WebControls.Button
    'Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCurrentEFT As System.Web.UI.WebControls.Label
    'Rahul
    Protected WithEvents PopcalendarDOB As RJS.Web.WebControl.PopCalendar

    Protected WithEvents PopcalendarDeceased As RJS.Web.WebControl.PopCalendar

    Protected WithEvents PopcalendarDate As RJS.Web.WebControl.PopCalendar
    'Rahul
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonGeneralDOBEdit As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditAddress As System.Web.UI.WebControls.Button
    Protected WithEvents valCustomDOB As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents ButtonAnnuitiesDateDeceasedEdit As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonGeneralQDROPendingEdit As System.Web.UI.WebControls.Button
    'Commented by Anudeep:14.03.2013 for telephone usercontrol
    Protected WithEvents TextboxTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxEmailId As System.Web.UI.WebControls.TextBox
    '' Protected WithEvents ButtonEditJSBeneficiaries As System.Web.UI.WebControls.Button
    Protected WithEvents TabStripRetireesInformation As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageRetireesInformation As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents ButtonTelephone As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridBankInfoList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridFederalWithholding As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridAnnuities As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridAnnuitiesPaid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridBeneficiaries As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridBeneficiariesGroupName As System.Web.UI.WebControls.DataGrid
    Protected WithEvents GridViewRetireesAttorney As System.Web.UI.WebControls.GridView

    Protected WithEvents btnRetireesInfoSave As System.Web.UI.WebControls.Button
    Protected WithEvents btnRetireesInfoCancel As System.Web.UI.WebControls.Button
    Protected WithEvents btnRetireesInfoPHR As System.Web.UI.WebControls.Button
    Protected WithEvents btnRetireesInfoOK As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridGeneralWithhold As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridNotes As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelAnnuitiesSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelWelcome As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHeading As System.Web.UI.WebControls.Label

    Protected WithEvents ButtonGeneralSSNoEdit As System.Web.UI.WebControls.Button
    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Edit Annuity Beneficiary SSN button is added and Constant variables for Beneficiaries update
    Protected WithEvents ButtonAnnuitiesSSNoEdit As System.Web.UI.WebControls.Button
    Protected WithEvents lnkbtnViewAnnuitiesSSNUpdate As System.Web.UI.WebControls.LinkButton

    Private Const RETIREBENEF_UniqueID As Integer = 2
    Private Const RETIREBENEF_PERSID As Integer = 3
    Private Const RETIREBENEF_NAME As Integer = 6
    Private Const RETIREBENEF_NAME2 As Integer = 7
    Private Const RETIREBENEF_TaxID As Integer = 8
    Private Const RETIREBENEF_REL As Integer = 9
    Private Const RETIREBENEF_BDAY As Integer = 10
    Private Const RETIREBENEF_GROUPS As Integer = 11
    Private Const RETIREBENEF_LVL As Integer = 12
    Private Const RETIREBENEF_PCT As Integer = 15
    Private Const RETIREBENEF_TYPE As Integer = 16
    Private Const RETIREBENEF_New As Integer = 19
    Private Const RETIREBENEF_IsExistingAudit As Integer = 24
    ' // END : SB | 07/07/2016 | YRS-AT-2382 |  Edit Annuity Beneficiary SSN button is added and Constant variables for Beneficiaries update
    Protected WithEvents NotesFlag As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents LabelListFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelListFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelListLastName As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelListCity As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelState As System.Web.UI.WebControls.Label

    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox

    'Protected WithEvents txtSpouse As System.Web.UI.WebControls.TextBox 'by aparna 05/09/2007
    Protected WithEvents chkOldGuardNews As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxAddress2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxAddress3 As System.Web.UI.WebControls.TextBox 'Textbox1
    'Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox

    '  Protected WithEvents CheckBoxActive As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxBadEmail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxUnsubscribe As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxTextOnly As System.Web.UI.WebControls.CheckBox
    'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from Retiree and put on address control
    'Protected WithEvents CheckboxIsBadAddress As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents CheckboxSecIsBadAddress As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents DropdownlistSecCountry As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DropdownlistSecState As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DropdownlistCountry As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DropdownlistState As System.Web.UI.WebControls.DropDownList
    'Commented by Anudeep:14.03.2013 for telephone usercontrol
    Protected WithEvents TextBoxHome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxMobile As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSecHome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSecFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSecMobile As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonGeneralWithholdAdd As System.Web.UI.WebControls.Button
    'Rahul 02 Mar,06
    Protected WithEvents ButtonGeneralWithholdUpdate As System.Web.UI.WebControls.Button
    'Rahul 02 Mar,06
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
    Protected WithEvents TextBoxGeneralQDROStatus As System.Web.UI.WebControls.TextBox
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
    'Protected WithEvents TextBoxGeneralAddress1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelGeneralAddress2 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxGeneralAddress2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelGeneralAddress3 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxGeneralAddress3 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelGeneralCity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxGeneralCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelGeneralState As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxGeneralState As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelZip As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxZip As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelCountry As System.Web.UI.WebControls.Label
    Protected WithEvents LabelGeneralTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEmailId As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralEmailId As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPOA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralPOA As System.Web.UI.WebControls.TextBox

    Protected WithEvents ButtonBankingInfoAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonFederalWithholdAdd As System.Web.UI.WebControls.Button
    'Rahul 02 Mar,06
    Protected WithEvents ButtonFederalWithholdUpdate As System.Web.UI.WebControls.Button
    'Rahul 02 Mar,06

    Protected WithEvents ButtonSuppressedJSAnnuities As System.Web.UI.WebControls.Button
    Protected WithEvents TextboxSuppressAnnuityCount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSuppressedJSAnnuities As System.Web.UI.WebControls.Label


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
    Protected WithEvents TextBoxGroupNameCon3RetDB As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonBankingInfoUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonBankingInfoCheckPayment As System.Web.UI.WebControls.Button

    Protected WithEvents ButtonGeneralPOA As System.Web.UI.WebControls.Button
    Protected WithEvents Menu1 As skmMenu.Menu

    '------------------------------------------------------------------------
    'Protected WithEvents Label10 As System.Web.UI.WebControls.Label 'SP 2014.06.13 -BT-2571 -
    Protected WithEvents DataGridRetiredBeneficiaries As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelPercentage2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRetiredDBR As System.Web.UI.WebControls.Label
    Protected WithEvents LabelInsResR As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    ' Protected WithEvents Label6 As System.Web.UI.WebControls.Label 'SP 2014.06.13 -BT-2571 -
    Protected WithEvents LabelPrimaryR As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPrimaryR As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonPriR As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxPrimaryInsR As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonPriInsR As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCont1R As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCont1R As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont1R As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxCont1InsR As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont1InsR As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCont2R As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCont2R As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont2R As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxCont2InsR As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont2InsR As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCont3R As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCont3R As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont3R As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxCont3InsR As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont3InsR As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddRetired As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditRetired As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDeleteRetired As System.Web.UI.WebControls.Button
    Protected WithEvents d1 As System.Web.UI.WebControls.DataGrid
    ''' added on 29th nov by Vartika
    'Protected WithEvents TextBoxSecAddress1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecAddress2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecAddress3 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecZip As System.Web.UI.WebControls.TextBox
    'Commented by Anudeep:14.03.2013 for telephone usercontrol
    Protected WithEvents TextBoxSecTelephone As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecEmail As System.Web.UI.WebControls.TextBox
    'BS:2012.03.09:-restructure Address Control
    ' Protected WithEvents TextBoxSecEffDate As YMCAUI.DateUserControl
    'Protected WithEvents TextBoxPriEffDate As YMCAUI.DateUserControl
    'Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control

    Protected WithEvents HiddenSecControlName As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents LinkButtonIDM As System.Web.UI.WebControls.ImageButton
    'BS:2012.05.10:yrs:1470:enhancement of user control for person maintenance
    'Protected WithEvents AddressWebUserControl1 As YMCAUI.AddressWebUserControl
    'Protected WithEvents AddressWebUserControl2 As YMCAUI.AddressWebUserControl
    'Protected WithEvents AddressWebUserControl1 As YMCAUI.Enhance_AddressWebUserControl
    'Protected WithEvents AddressWebUserControl2 As YMCAUI.Enhance_AddressWebUserControl

    Protected WithEvents AddressWebUserControl1 As YMCAUI.AddressUserControlNew
    Protected WithEvents AddressWebUserControl2 As YMCAUI.AddressUserControlNew
    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    Protected WithEvents AddressWebUserControlAnn As YMCAUI.AddressUserControlNew

    'Anudeep Added by Anudeep:14.03.2013 for telephone usercontrol
    Protected WithEvents TelephoneWebUserControl1 As YMCAUI.Telephone_WebUserControl
    Protected WithEvents TelephoneWebUserControl2 As YMCAUI.Telephone_WebUserControl
    Protected WithEvents tbSeccontacts As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tbPricontacts As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents btnEditPrimaryContact As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnEditSecondaryContact As System.Web.UI.WebControls.ImageButton
    'Anudeep-2013.08.21 - YRS 5.0-2162:Edit button for email address 
    Protected WithEvents imgBtnEmail As System.Web.UI.WebControls.ImageButton
    'Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control


    '''


    'NOTE: The following placeholder declaration is required by the Web Form Designer.

    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Protected WithEvents LabelInsSavR As System.Web.UI.WebControls.Label
    'Protected WithEvents Label11 As System.Web.UI.WebControls.Label 'SP 2014.06.13 -BT-2571 -
    Protected WithEvents TextboxPrimaryInssavR As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCont1InssavR As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCont2InssavR As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCont3InssavR As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonPriInssavR As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCont1InssavR As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCont2InssavR As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCont3InssavR As System.Web.UI.WebControls.Button

    'Added by shashi for data archive on 2009-09-08
    Protected WithEvents LabelGenHdr As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAnnuitiesHdr As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonGetArchiveDataBack As System.Web.UI.WebControls.Button
    Protected WithEvents tdRetrieveData As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tblRetrieveData As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSuppress As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents chkShareInfoAllowed As System.Web.UI.WebControls.CheckBox
    'Added by priya as on 5-April-2010 : YRS 5.0-1042- New "flag" value in Person/Retiree maintenance screen
    'Protected WithEvents chkYMCAMailingOptOut As System.Web.UI.WebControls.CheckBox
    'Replaced by bhavnaS as on 2011.07.08 : YRS 10.1.1.4- Change caption
    Protected WithEvents chkPersonalInfoSharingOptOut As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkGoPaperless As System.Web.UI.WebControls.CheckBox
    Protected WithEvents rfvGender As System.Web.UI.WebControls.RequiredFieldValidator
    'Added by imran on 30-July-2010 : YRS 5.0-1141 add Age format to the person maintanace screen.
    Protected WithEvents TextBoxAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelBoxAge As System.Web.UI.WebControls.Label

    'Added by Shashi Shekhar:Feb 008 2011 for YRS 5.0-1236
    Protected WithEvents trLockResDisplay As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trLockResEdit As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblLockstatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblLockResDetail As System.Web.UI.WebControls.Label
    Protected WithEvents btnLockUnlock As System.Web.UI.WebControls.Button
    Protected WithEvents btnAcctLockEditRet As System.Web.UI.WebControls.Button
    Protected WithEvents ddlReasonCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblResCode As System.Web.UI.WebControls.Label
    Protected WithEvents HeaderControl As YMCA_Header_WebUserControl
    Protected WithEvents LabelCurrency As System.Web.UI.WebControls.Label
    Protected WithEvents LinkRefereshIDM As System.Web.UI.WebControls.ImageButton

    Protected WithEvents imgLock As System.Web.UI.WebControls.Image
    Protected WithEvents imgLockBeneficiary As System.Web.UI.WebControls.Image
    'Anudeep:21.10.2013:BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
    Protected WithEvents DivMainMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divErrorMsg As System.Web.UI.HtmlControls.HtmlGenericControl 'Dharmesh : 11/20/2018 : YRS-AT-4136 : display error message for participant who enrolled on or after 2019
    'AA:2014.02.03 - BT:2292:YRS 5.0-2248 - Added the Textbox value
    Protected WithEvents txtPIN As System.Web.UI.WebControls.TextBox
    'AA:2014.02.03 - BT:2316:YRS 5.0-2247 - Added the Checkbox 
    Protected WithEvents chkNoWebAcctCreate As System.Web.UI.WebControls.CheckBox
    'AA:2014.02.03 - BT:2316:YRS 5.0-2247 - Added the Linkbutton 
    Protected WithEvents lnkParticipantAddress As System.Web.UI.WebControls.LinkButton
    Protected WithEvents hdnUnSuppress As System.Web.UI.WebControls.HiddenField
    Public WithEvents stwListUserControl As StateWithholdingListingControl  'ML | 2019.09.20 | YRS-AT-4598 |State Withholding control declare as Public
    Public WithEvents lblNoteUnsuppress As System.Web.UI.WebControls.Label 'PK | 2019.24.10 |YRS-AT-4598 |Declared control to display a note on unsupress button

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

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

    ''SP 2014.12.03  BT-2310\YRS 5.0-2255: -Start
    Public Property OriginalBeneficiaries As DataSet
        Get
            If Not (ViewState("OriginalBeneficiary")) Is Nothing Then
                Return (CType(ViewState("OriginalBeneficiary"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            ViewState("OriginalBeneficiary") = Value
        End Set
    End Property
    Public Property IsModifiedPercentage As Boolean
        Get
            If Not (ViewState("IsModifiedPercentage")) Is Nothing Then
                Return (CType(ViewState("IsModifiedPercentage"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsModifiedPercentage") = Value
        End Set
    End Property
    ''SP 2014.12.03   BT-2310\YRS 5.0-2255: -End

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Property to store relationship
    Public Property RelationShipCheck As DataSet
        Get
            Return ViewState("RelationShipCheck")
        End Get
        Set(value As DataSet)
            ViewState("RelationShipCheck") = value
        End Set
    End Property
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Property to store relationship

    'START: MMR| 2016.10.12 | YRS-AT-3063 |Declared property to store ExhaustedDB/RMD settle option selected or not
    Private Property IsExhaustedDBRMDSettleOptionSelected As Boolean
        Get
            If Not (ViewState("IsExhaustedDBRMDSettleOptionSelectedRetiree")) Is Nothing Then
                Return (CType(ViewState("IsExhaustedDBRMDSettleOptionSelectedRetiree"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsExhaustedDBRMDSettleOptionSelectedRetiree") = Value
        End Set
    End Property
    'END: MMR| 2016.10.12 | YRS-AT-3063 |Declared property to store ExhaustedDB/RMD settle option selected or not
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
    'Public IsStatusDR As Boolean = False

    'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
    ''START: SB | 2017.10.18 | YRS-AT-3324 | Added property that will return the reason for restriction when prcoessing for Death Notification 
    'Private Property ReasonForRestriction() As String
    '    Get
    '        If Not Session("ReasonForRestriction") Is Nothing Then
    '            Return (CType(Session("ReasonForRestriction"), String))
    '        Else
    '            Return String.Empty
    '        End If

    '    End Get
    '    Set(ByVal Value As String)
    '        Session("ReasonForRestriction") = Value
    '    End Set
    'End Property

    '' Added property that will vaildate Death Notification is allowed or not for RMD eligible participants 
    'Public Property IsPersonEligibleForRMD() As Boolean
    '    Get
    '        If Session("DeathNotificationAllowedForRMDEligibleParticipants") Is Nothing Then
    '            Session("DeathNotificationAllowedForRMDEligibleParticipants") = Validation.IsRMDExist(YMCAObjects.Module.Death_Notification, Session("FundId"), Me.ReasonForRestriction)
    '        End If
    '        Return CType(Session("DeathNotificationAllowedForRMDEligibleParticipants"), Boolean)
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        Session("DeathNotificationAllowedForRMDEligibleParticipants") = Value
    '    End Set
    'End Property
    ''END: SB | 2017.10.18 | YRS-AT-3324 | Added property that will vaildate Death Notification is allowed or not for RMD eligible participants 
    'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 

    'START: MMR| 2011.11.22 | YRS-AT-4018 | Declared property to store participant PII Information updation allowed status.
    Public Property PIIInformationRestrictionMessageCode As Integer
        Get
            If Not (ViewState("PIIInformationRestrictionMessageCode")) Is Nothing Then
                Return (CType(ViewState("PIIInformationRestrictionMessageCode"), Integer))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState("PIIInformationRestrictionMessageCode") = Value
        End Set
    End Property

    Public Property PIIInformationRestrictionMessageText As String
        Get
            If Not (ViewState("PIIInformationRestrictionMessageText")) Is Nothing Then
                Return (CType(ViewState("PIIInformationRestrictionMessageText"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("PIIInformationRestrictionMessageText") = Value
        End Set
    End Property
    'END: MMR| 2011.11.22 | YRS-AT-4018 | Declared property to store participant PII Information updation allowed status.

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Vipul 01Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()
        'Vipul 01Feb06 Cache-Session

        'For achieving 508 Compliance -List
        'BS:2012:04.11:-Address Restructure
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        CheckReadOnlyMode()

        l_string_VignettePath = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_Participant"), Session("FundNo"))

        Dim dsRelationshipCode As New DataSet ' //SB | 07/07/2016 | YRS-AT-2382 | Dataset to hold relationship codes
        Dim popupScript As String = "<script language='javascript'>" & _
            "CallFrame('" + l_string_VignettePath + "'); </script>"

        'Page.RegisterStartupScript("IDMPopUpScript", popupScript)
        ClientScript.RegisterClientScriptBlock(Page.GetType(), "IDMPopUpScript", popupScript)

        LinkRefereshIDM.Attributes.Add("onclick", "CallFrame('" + l_string_VignettePath + "');")

        Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        Menu1.DataBind()
        InitializeAttributesOfServerControls()

        'Added by Dilip 20-09-2009 to refresh the Qdro Request information
        'Commented  & Added by Dilip Yadav : YRS 5.0.921
        'Priya : 24-March-2010: BT:479: While adding QDRO Request page not getting Refresh.
        'Commented If not i spost back code and uun comment RefreshQdroInfo() to refresh QDRO information after update and added new QDRO.
        RefreshQdroInfo()
        'If (Not Page.IsPostBack) Then
        '    RefreshQdroInfo()
        'End If
        'End 24-March-2010: BT:479

        Session("ISRetired") = True
        'Added by Dilip
        Try

            Dim strWSMessage As String


            ValidationSummaryRetirees.Enabled = True 'Added By Shashi Shekhar:2009-12-23

            ' ButtonGeneralQDROPendingEdit.Attributes.Add("onclick", "javascript: return NewWindow('AddEditQDRO.aspx','mywin','750','500','yes','center');")

            If Session("EnableSaveCancel") = True Then
                '                ButtonRetireesInfoSave.Enabled = True 'by Aparna 27/08/07
                Me.ButtonSaveRetireeParticipants.Enabled = True
                ButtonRetireesInfoCancel.Enabled = True
                'By Ashutosh Patil as on 25-Apr-2007
                'YREN-3298
                'ButtonRetireesInfoSave.Visible = True
                ButtonRetireesInfoCancel.Visible = True
                Me.MakeLinkVisible()

            Else
                Me.ButtonSaveRetireeParticipants.Enabled = False
                'ButtonRetireesInfoSave.Enabled = False
                'ButtonRetireesInfoCancel.Enabled = False
                'ButtonRetireesInfoSave.Visible = False
                'ButtonRetireesInfoCancel.Visible = False
            End If

            ' txtSpouse.Attributes.Add("OnBlur", "_OnBlur_TextBoxSpouse();")

            'TextBoxAnnuitiesSSNo.Attributes.Add("OnBlur", "_OnBlur_TextBoxSSNoAnnuitites();")
            'TextBoxGeneralSSNo.Attributes.Add("OnBlur", "_OnBlur_TextBoxSSNo();")
            'TextBoxZip.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
            '**************Code Commented by Shubhrata 09-05-06 YRST-2243
            'ButtonRetireesInfoSave.Attributes.Add("OnClick", "javascript:return _OnBlur_TextBoxSSNo();")
            'rahul
            '******************
            If Not IsPostBack Then
                SessionManager.SessionStateWithholding.LstSWHPerssDetail = Nothing ' ML | YRS-AT-4598 | Clear Session Variable.

                'START: MMR| 2011.11.22 | YRS-AT-4018 | Get participant PII Information updation allowed status.
                Me.PIIInformationRestrictionMessageCode = YMCARET.YmcaBusinessObject.CommonBOClass.ValidatePIIRestrictions(CType(Session("PersId"), String))
                If Me.PIIInformationRestrictionMessageCode <> 0 Then
                    Me.PIIInformationRestrictionMessageText = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(Me.PIIInformationRestrictionMessageCode, Nothing)
                End If
                'END: MMR| 2011.11.22 | YRS-AT-4018 | Get participant PII Information updation allowed status.
                ValidationSummaryRetirees.Enabled = False 'Added By Shashi Shekhar:2009-12-23

                'Me.IframeIDMView.Attributes("src") = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_Participant"), Session("FundNo"))
                'Shubhrata YRSPS 4614
                Session("Participant_Sort") = Nothing
                Session("DisplayNotes") = Nothing
                'Shubhrata YRSPS 4614
                'Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control
                getSecuredControls()
                'Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control
                'BS:2012:04.11:-Address Restructure
                InitializeMartialStatusDropDownList()
                InitializeGenderTypesDropDownList()
                ' // START : SB | 07/07/2016 | YRS-AT-2382 | Clearing Audit log record DataTable and Annuity changes any
                Session("AuditBeneficiariesTable") = Nothing
                Session("IsAnnuityBeneficiariesSSNChanged") = Nothing
                ' // END : SB | 07/07/2016 | YRS-AT-2382 | Clearing Audit log record DataTable and Annuity changes any
                'Shubhrata Mar 2nd,2007 YREN-3112
                'Session("IsFedTaxForMaritalStatus") = True  'commented by hafiz on 2-May-2007 for YREN-3112
                'Shubhrata Mar 2nd,2007 YREN-3112

                'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
                'ClearSession()           'SB | 10/18/2017 | YRS-AT-3324 | Clearing Session variable used to check RMD generated or not
                'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 

                Me.TextBoxGeneralDOB.Enabled = False
                Me.TextBoxGeneralSuffix.Enabled = False
                Me.TextBoxGeneralMiddleName.Enabled = False
                Me.TextBoxGeneralLastName.Enabled = False
                Me.TextBoxGeneralFirstName.Enabled = False
                Me.cboSalute.Enabled = False
                TextBoxGeneralDOB.Enabled = False

                'Me.TextBoxGeneralPOA.Enabled = False
                Me.TextBoxGeneralPOA.ReadOnly = True 'BS:2011.08.02:BT:911- TextBoxPOA should be readonly if multiple poa exist on  personmaintenance screen bcoz all editing/adding of Poa information handle on POAscreen so there is no need to enable Poa Textbox

                chkGeneralQDROPending.Enabled = False
                TextBoxGeneralQDROStatus.Enabled = False
                TextBoxGeneralQDROStatudDate.Enabled = False

                Me.TextBoxGeneralSSNo.Enabled = False

                'Vipul 01March06 YRS Enhancement#49  
                Me.TextBoxFundNo.Enabled = False
                'Vipul 01March06 YRS Enhancement#49  

                Me.ButtonBankingInfoCheckPayment.Enabled = False


                Dim g_dataset_dsAddressStateType As New DataSet
                Dim g_dataset_dsAddressCountry As New DataSet
                Dim InsertRowState As DataRow
                Dim InsertRowCountry As DataRow

                'Me.DropdownlistState.DataSource = Nothing
                'Code Commented by ashutosh on 25-June-06
                'g_dataset_dsAddressStateType = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressStateType()


                'Me.DropdownlistState.DataSource = g_dataset_dsAddressStateType
                'Me.DropdownlistState.DataMember = "State Type"
                'Me.DropdownlistState.DataTextField = "chvShortDescription"
                'Me.DropdownlistState.DataValueField = "chvCodeValue"
                'Me.DropdownlistState.DataBind()
                'Me.DropdownlistState.Items.Insert(0, "-Select State-")
                'Me.DropdownlistState.Items(0).Value = ""


                'Me.DropdownlistSecState.DataSource = g_dataset_dsAddressStateType
                'Me.DropdownlistSecState.DataMember = "State Type"
                'Me.DropdownlistSecState.DataTextField = "chvShortDescription"
                'Me.DropdownlistSecState.DataValueField = "chvCodeValue"
                'Me.DropdownlistSecState.DataBind()
                'Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
                'Me.DropdownlistSecState.Items(0).Value = ""
                'End Comment Ashutosh on 25 June 06
                'g_dataset_dsAddressCountry = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressCountry()
                'Ashutosh Patil as on 26-Mar-2007
                'YREN - 3028,YREN-3029
                'Me.DropdownlistCountry.DataSource = g_dataset_dsAddressCountry.Tables(0)
                'Me.DropdownlistCountry.DataMember = "Country"
                'Me.DropdownlistCountry.DataTextField = "chvDescription"
                'Me.DropdownlistCountry.DataValueField = "chvAbbrev"
                'Me.DropdownlistCountry.DataBind()
                'Me.DropdownlistCountry.Items.Insert(0, "-Select Country-")
                'Me.DropdownlistCountry.Items(0).Value = ""

                'Me.DropdownlistSecCountry.DataSource = g_dataset_dsAddressCountry.Tables(0)
                'Me.DropdownlistSecCountry.DataMember = "Country"
                'Me.DropdownlistSecCountry.DataTextField = "chvDescription"
                'Me.DropdownlistSecCountry.DataValueField = "chvAbbrev"
                'Me.DropdownlistSecCountry.DataBind()
                'Me.DropdownlistSecCountry.Items.Insert(0, "-Select Country-")
                'Me.DropdownlistSecCountry.Items(0).Value = ""
                Me.AddressWebUserControl1.EnableControls = False
                Me.AddressWebUserControl2.EnableControls = False
                'START: MMR | 2018.11.23 | YRS-AT-4018 | Set property value to allow address can be updated/Added or not
                If Me.PIIInformationRestrictionMessageCode <> 0 Then
                    Me.AddressWebUserControl1.IsPIIInformationAllowedToChange = False
                    Me.AddressWebUserControl2.IsPIIInformationAllowedToChange = False
                    Me.AddressWebUserControl1.PIIInformationRestrictionMessageTextAddressControl = Me.PIIInformationRestrictionMessageText
                    Me.AddressWebUserControl1.DivPIIErrorControlID = DivErrorMessage.ClientID
                    Me.AddressWebUserControl2.PIIInformationRestrictionMessageTextAddressControl = Me.PIIInformationRestrictionMessageText
                    Me.AddressWebUserControl2.DivPIIErrorControlID = DivErrorMessage.ClientID
                End If
                'END: MMR | 2018.11.23 | YRS-AT-4018 | Set property value to allow address can be updated/Added or not
                'Added by Anudeep:14.03.2013 for telephone usercontrol
                'Me.TelephoneWebUserControl1.EditPrimaryContact_Enabled = False
                ' Me.TelephoneWebUserControl2.EditSecondaryContact_Enabled = False

                TextBoxEmailId.Enabled = False
                LabelEmailId.Enabled = False
                Session("CurrentProcesstoConfirm") = ""
                Session("PhonySSNo") = Nothing

                ' // START : SB | 07/07/2016 | YRS-AT-2382 | Loading relationship code from database
                dsRelationshipCode = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getRelationShips()
                RelationShipCheck = dsRelationshipCode
                ' // END : SB | 07/07/2016 | YRS-AT-2382 | Loading relationship code from database

                Load_AllTabsData()
                GetWebAccountPrintConfigValue() 'JT | 2018.08.28 | YRS-AT-4031| Fetching configuration value
                tbPricontacts.Visible = False
                tbSeccontacts.Visible = False


                'Start:AA:2013.10.23 - Bt-2264:Commented below code and has been placed out of ispostback condition
                ''Start -> DineshK:2013.02.12:BT-1261/YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                'If Not Session("Reason") Is Nothing Then
                '    If Session("Reason").ToString.ToLower = "yes" Then
                '        Me.MultiPageRetireesInformation.SelectedIndex = 2
                '        Me.TabStripRetireesInformation.SelectedIndex = 2
                '        If Not Session("BenTableCellCollection") Is Nothing Then
                '            DeleteRetired(CType(Session("BenTableCellCollection"), TableCellCollection), CType(Session("BenItemIndex"), Integer))
                '            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Delete Beneficiary Reason Added Sucessfully.", MessageBoxButtons.OK)
                '        End If
                '    ElseIf Session("Reason").ToString.ToLower = "no" Then
                '        Me.MultiPageRetireesInformation.SelectedIndex = 2
                '        Me.TabStripRetireesInformation.SelectedIndex = 2
                '    End If
                '    Session("Reason") = Nothing
                '    Session("BenTableCellCollection") = Nothing
                '    Session("BenItemIndex") = Nothing
                'End If

                ''END -> DineshK:2013.02.12:BT-1261/YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                'End:AA:2013.10.23 - Bt-2264:Commented below code and has been placed out of ispostback condition

                'SR:2013.08.05 - YRS 5.0-2070 : Change the color of Beneficiary tab for Pending request.
                'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
                strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
                If strWSMessage <> "NoPending" Then

                    strWSMessage = (strWSMessage.Replace("<br/>", "\n")).Replace("<br>", "\n")
                    Me.TabStripRetireesInformation.Items(2).Text = "<asp:Label ID='lblBeneficiaries' CssClass='Label_Small'onmouseover='javascript: showToolTip(""" + strWSMessage + """,""Bene"");' onmouseout='javascript: hideToolTip();'><font color=red>Beneficiary</font></asp:Label>"

                    cboGeneralMaritalStatus.Enabled = False

                    imgLock.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Pers');")
                    imgLock.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                    imgLock.Visible = True

                    imgLockBeneficiary.Visible = True
                    imgLockBeneficiary.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Bene');")
                    imgLockBeneficiary.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                End If
                'End, SR:2013.08.05 - YRS 5.0-2070 : Change the color of Beneficiary tab for Pending request.
                Session("IsExhaustedDBRMDSettleRetiree") = Nothing 'MMR | 2016.10.14 | YRS-AT-3062 | Clearing session value
                'START : ML | 2019.09.20 | YRS-AT-4598 | Set Session Variable for State Withholding User Control
                stwListUserControl.PersonID = Session("PersId")
                stwListUserControl.STWDataSaveAtMainPage = True
                'END : ML | 2019.09.20 | YRS-AT-4598 | Set Session Variable for State Withholding User Control

            End If
            'Dineshk            2013.09.18      YRS 5.0-2204:Cannot see other annuity beneficiaries in Annuities display
            If DataGridAnnuities.Items.Count > 0 Then
                For iCount As Integer = 0 To DataGridAnnuities.Items.Count - 1
                    DataGridAnnuities.Items(iCount).Style.Add("font-weight", "normal")
                Next
            End If

            'SP :2014.02.05 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -start  
            If (Session("SessionTempSSNTable") IsNot Nothing) Then
                SetUpdatedSSN()
                If (IsSSNEdited()) Then
                    HiddenFieldDirty.Value = "true"
                End If
            End If
            'SP :2014.02.05 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -end 

            'Added by imran on 30-July-2010 : YRS 5.0-1141 add Age format to the person maintanace screen.
            'Added by imran on 03-July-2010 :  BT-865 Age shows wrong after Deceased date entry.
            'BS:2011.08.10:YRS 5.0-1339 - reopen issue
            'DK:2013.07.01:BT : 1499 Person Miscellanious
            'If Not TextBoxGeneralDOB.Text = String.Empty Then
            '    TextBoxAge.Text = CalculateAge(TextBoxGeneralDOB.Text, TextBoxGeneralDateDeceased.Text).ToString("00.00")
            '    If (Me.TextBoxAge.Text.IndexOf(".00") > -1) Then
            '        Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace(".00", "Y")
            '    Else
            '        Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace(".", "Y/") + "M"
            '    End If
            '    Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace("/00M", "").Trim
            'Else
            '    TextBoxAge.Text = String.Empty
            'End If

            '' // START : SB | 07/07/2016 | YRS-AT-2382 | Check AuditLog table is updated or not
            'If Session("IsAnnuityBeneficiariesSSNChanged") IsNot Nothing AndAlso Session("IsAnnuityBeneficiariesSSNChanged") Then
            '    SetUpdatedAnnuitySSN()
            '    HiddenFieldDirty.Value = "true"
            '    Session("IsAnnuityBeneficiariesSSNChanged") = Nothing
            'End If
            '' // END : SB | 07/07/2016 | YRS-AT-2382 | Check AuditLog table is updated or not

            If Not TextBoxGeneralDOB.Text = String.Empty Then
                LabelBoxAge.Text = CalculateAge(TextBoxGeneralDOB.Text, TextBoxGeneralDateDeceased.Text).ToString("00.00")
                If (Me.LabelBoxAge.Text.IndexOf(".00") > -1) Then
                    Me.LabelBoxAge.Text = Me.LabelBoxAge.Text.Replace(".00", "Y")
                Else
                    Me.LabelBoxAge.Text = Me.LabelBoxAge.Text.Replace(".", "Y/") + "M"
                End If
                Me.LabelBoxAge.Text = Me.LabelBoxAge.Text.Replace("/00M", "").Trim
            Else
                LabelBoxAge.Text = String.Empty
            End If


            'TextBoxAge.Text = CalculateAge(TextBoxGeneralDOB.Text, TextBoxGeneralDateDeceased.Text)
            'If (Me.TextBoxAge.Text.IndexOf(".00") > -1) Then
            '    Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace(".00", "Y")
            'Else
            '    Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace(".", "Y/") + "M"
            'End If
            If Session("blnAddBankingRetirees") = True Or Session("blnUpdateBankingRetirees") = True Or Session("blnShowBankingPage") = True Then
                Me.MultiPageRetireesInformation.SelectedIndex = 4
                Me.TabStripRetireesInformation.SelectedIndex = 4
                LoadBanksTab()

                Session("blnShowBankingPage") = Nothing
            End If

            If Session("blnAddFedWithHoldings") = True Or Session("blnUpdateFedWithHoldings") = True Then

                Me.MultiPageRetireesInformation.SelectedIndex = 5
                Me.TabStripRetireesInformation.SelectedIndex = 5
                LoadFedWithDrawalTab()
            End If
            'START: ML | 2019.09.20 | YRS-AT-4598 | Assign fundno to Statewithholding control and refresh automatically on data save
            stwListUserControl.Refresh()
            ValidateSTWvsFedtaxforMA()


            'END:  ML | 2019.09.20 | YRS-AT-4598 | Assign fundno to Statewithholding control and refresh automatically on data save

            If Session("DeathNotification") = True Then
                Me.MultiPageRetireesInformation.SelectedIndex = 0
                Me.TabStripRetireesInformation.SelectedIndex = 0
                '34231 Added Prcess code to direct the code.

                Session("CurrentProcesstoConfirm") = "DeathProcess"
                'Aparna IE7 08/10/2007
                'Shagufta:27 July 2011- For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance
                ButtonRetireesInfoPHR.Enabled = False
                ButtonRetireesInfoOK.Enabled = False
                'End:27 July 2011- For BT-751,YRS 5.0-1268
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you would like to enter a death date? Selecting Yes will launch death notification actions. Selecting No will cancel this action.", MessageBoxButtons.YesNo, False)
                Session("DeathNotification") = False
                Exit Sub
            End If

            'Ragesh 34231 Added Process code to direct the code in two process.


            If Session("CurrentProcesstoConfirm") = "DeathProcess" Then
                'Anudeep:28.02.2013:Resetting the session variable after completed
                Session("LastName") = Nothing
                Session("FirstName") = Nothing
                If Request.Form("Yes") = "Yes" Then
                    'Bhavna:Session comment of deathnotify function YRS 5.0-1432
                    'If DeathNotify() = False Then
                    '	Session("CurrentProcesstoConfirm") = ""
                    '	Exit Sub
                    'End If
                    'BS:2012.03.02:BT-941,YRS 5.0-1432
                    If FinalDeathNotificationUpdation() = False Then
                        Session("CurrentProcesstoConfirm") = ""
                        Exit Sub
                    End If
                    'If Session("MessageDisplayed") = True Then
                    '    Session("MessageDisplayed") = False
                    '    Exit Sub
                    'End If
                    'Me.TextBoxGeneralDateDeceased.Text = Session("DeathDate")
                ElseIf Request.Form("No") = "No" Then
                    Session("CurrentProcesstoConfirm") = ""
                    'Aparna IE7 08/10/2007
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Death notification cancelled.", MessageBoxButtons.OK)
                    Exit Sub
                End If

                'START: PPP | 04/20/2016 | YRS-AT-2719 | There wont be any request object in this case not checkin Request.Form("Yes")
                'ElseIf Session("CurrentProcesstoConfirm") = "UnSuppressProcess" Then
                ''Priya/Ashish: 21-April-2010:YRS 5.0-1056: Additional info when unsuppressing a JSANN annuity
                'rfvGender.Enabled = False
                'If Request.Form("Yes") = "Yes" Then
                '    UNSuppresseJSannuities()
                'Else
                '    Session("CurrentProcesstoConfirm") = ""
                'End If
                'END: PPP | 04/20/2016 | YRS-AT-2719 | There wont be any request object in this case not checkin Request.Form("Yes")
            Else
                'Priya/Ashish: 21-April-2010:YRS 5.0-1056: AdditionalInfo when unsuppressing a JSANN annuity
                rfvGender.Enabled = True
            End If

            'Start: Bala: 12/01/2016: YRS-AT-1718: Delete Notes.
            If Session("CurrentProcesstoConfirm") = "DeleteNotes" Then
                If Request.Form("Yes") = "Yes" Then
                    NotesManagement.DeleteNotes(Session("NotesToDelete"))
                    'Start:AA: 02.15.2016 YRS-AT-1718 Added to refresh the notes datagrid
                    Session("dtNotes") = Nothing
                    LoadNotesTab()
                    'End:AA: 02.15.2016 YRS-AT-1718 Added to refresh the notes datagrid
                    Session("CurrentProcesstoConfirm") = ""
                    Session("NotesToDelete") = ""
                ElseIf Request.Form("No") = "No" Then
                    Session("CurrentProcesstoConfirm") = ""
                    Exit Sub
                ElseIf Request.Form("Ok") = "Ok" Then
                    Session("CurrentProcesstoConfirm") = ""
                    Exit Sub
                End If
            End If
            'End: Bala: 12/01/2016: YRS-AT-1718: Delete Notes.

            '-------------------------------------------------------------------------------------------------
            'Shashi Shekhar:2009.09.08 Ref:(PS:YMCA PS Data Archive.Doc) - Adding code to receive Confirmation for data retrieved on Get Archived Data Back click.
            If Session("CurrentProcesstoConfirm") = "DataArchive" Then
                If Request.Form("Yes") = "Yes" Then
                    'If the user confirms 
                    'call function to retrieve the data
                    Session("CurrentProcesstoConfirm") = ""
                    RetrieveData() 'Shashi Shekhar:2010-01-27
                    Exit Sub
                ElseIf Request.Form("No") = "No" Then
                    'Nothing
                    Session("CurrentProcesstoConfirm") = ""
                    Exit Sub
                End If
            End If

            '--------------------------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------------------------
            'Shashi Shekhar:2011.02.09: For YRS 5.0-1236 :Need ability to freeze/lock account.
            If Session("CurrentProcesstoConfirm") = "AccountLock" Then
                If Request.Form("Yes") = "Yes" Then
                    'If the user confirms 
                    'call function to do locking or unlocking the account.
                    Session("CurrentProcesstoConfirm") = ""
                    UpdateAccountLockUnlock()
                    Exit Sub
                ElseIf Request.Form("No") = "No" Then
                    'Nothing
                    Session("CurrentProcesstoConfirm") = ""
                    trLockResEdit.Visible = False
                    trLockResDisplay.Visible = True
                    ddlReasonCode.Items.Clear()
                    Exit Sub
                End If
            End If

            '--------------------------------------------------------------------------------------------------


            If Session("blnAddGenWithHoldings") = True Or Session("blnUpdateGenWithDrawals") = True Then
                Me.MultiPageRetireesInformation.SelectedIndex = 6
                Me.TabStripRetireesInformation.SelectedIndex = 6
                LoadGenWithDrawalTab()
            End If

            'Start: Bala: 01/12/2016: YRS-AT-1718: Saving Notes
            If Session("blnAddNotes") = True Or Session("blnUpdateNotes") = True Or Session("Flag") = "AddNotes" Then
                'End: Bala: 01/12/2016: YRS-AT-1718: Saving Notes
                Me.MultiPageRetireesInformation.SelectedIndex = 7
                Me.TabStripRetireesInformation.SelectedIndex = 7
                LoadNotesTab()
            Else
                'by aparna to avoid rebinding of data when check box is clicked
                ' LoadNotesTab()
            End If
            'BS:2012.07.18:YRS 5.0-1470:-Refresh Notes field After adding from address verify popup
            'Anudeep:24.06.2013 Error occured for when redirecting retiree information to particiapant information
            'If Session("Flag") = "AddNotesByAddressVerify" Then
            If Session("AddNotesByAddressVerify") = True Then
                LoadNotesTab()
                'Anudeep:24.06.2013 Error occured for when redirecting retiree information to particiapant information
                'Session("Flag") = ""
                Session("AddNotesByAddressVerify") = Nothing
            End If
            'If Me.NotesFlag.Value = "Notes" Then
            '    'by Aparna -YREN-3115 09/03/2007
            '    Me.TabStripRetireesInformation.Items(7).Text = "<font color=orange>Notes</font>"
            'ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
            '    Me.TabStripRetireesInformation.Items(7).Text = "<font color=red>Notes</font>"
            'Else
            '    Me.TabStripRetireesInformation.Items(7).Text = "Notes"
            'End If



            If Session("Flag") = "AddBeneficiaries" Or Session("Flag") = "EditBeneficiaries" Or Session("Flag") = "EditedBeneficiaries" Then
                'TabStripParticipantsInformation.SelectedIndex = 4
                'MultiPageParticipantsInformation.SelectedIndex = 4
                'Priya				26-May-2012		YRS 5.0-1576: update marital status if spouse beneficiary entered
                If Not IsNothing(Session("MaritalStatus")) Then
                    cboGeneralMaritalStatus.SelectedValue = Session("MaritalStatus").ToString().Trim
                    Session("MaritalStatus") = Nothing
                End If
                'End YRS 5.0-1576: update marital status if spouse beneficiary entered
                LoadBeneficiariesTab()
                Session("Flag") = ""
                Session("icounter") = 0
            End If
            If Session("POA") = True Then
                'TextBoxGeneralPOA.Text = Session("POAName")
                'BT 706 2011.07.22 bhavnaS not need to session display all active POA
                'TextBoxPOA.Text = Session("POAName")
                'BT 706
                LoadPoADetails()
                'Anudeep:30.07.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                LoadNotesTab()
                Session("POA") = False
            End If
            'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.

            '34231 Ragesh to allow to show any errors in the suppress annuties process in the msgbox.
            If Request.Form("OK") = "OK" Then
                If Session("CurrentProcesstoConfirm") = "UnSuppressProcess" Then
                    Session("CurrentProcesstoConfirm") = ""

                    getSuppressJSAnnuityCount()

                    'Do not do anything.
                ElseIf Session("CurrentProcesstoConfirm") = "NoRecord_OnGeneral" Then
                    Session("Page") = "Person"
                    Session("blnCancel") = Nothing
                    Session("EnableSaveCancel") = Nothing
                    Session("PersId") = Nothing
                    Session("payment_Type") = Nothing
                    Session("ValidationMessage") = Nothing
                    Session("Flag") = Nothing
                    Session("icounter") = Nothing
                    Session("RetireesSort") = Nothing
                    Response.Redirect("FindInfo.aspx?Name=Person", False)
                    Session("CurrentProcesstoConfirm") = ""
                    'Bhavna:Session comment of deathnotify function YRS 5.0-1432
                    'Else  ''YRS 5.0-1292: A warning msg should appear with YES/NO option Hence OK option is no longer needed for finaldeathnotification.
                    'If Not Session("Call_FinalDeathNotificationUpdation") Is Nothing Then
                    'If Session("Call_FinalDeathNotificationUpdation") = "Yes" Then
                    'Session("Call_FinalDeathNotificationUpdation") = Nothing
                    'If FinalDeathNotificationUpdation() = False Then
                    '    Exit Sub
                    'End If
                    ''SR:2011.04.28 : YRS 5.0-1292: A warning msg should appear OK option for Non Retired Participant.Hence below code is added 
                    'If (Session("DNMessageforDR") <> "") Then
                    '	'MessageBox.Show(PlaceHolder1, "Note before you proceed.", Session("DNMessageforDR") + " Do you wish to proceed?", MessageBoxButtons.YesNo) ''SR:2011.04.27 - YRS 5.0 1292 : OK button replaced by YES/NO button
                    '	'BS:2012.02.20:BT-941,YRS 5.0-1432:- IsStatusDR set true to call ProcessReport() to open report: List_of_Annuity_Checks_Sent_to_Retirees_After_Death.rpt pass session value yes to perform deathnotification
                    '	IsStatusDR = True
                    '	Session("Call_FinalDeathNotificationUpdation") = "Yes"
                    '	'Exit Sub
                    'Else
                    '	Session("Call_FinalDeathNotificationUpdation") = Nothing
                    '	If FinalDeathNotificationUpdation() = False Then
                    '		Exit Sub
                    '	End If
                    'End If


                    'End If
                    'End If
                    'End Bhavna:Sesion comment of detahnotify function YRS 5.0-1432
                End If

            End If
            'Bhavna:Sesion comment of detahnotify function YRS 5.0-1432
            ''YRS 5.0-1292: A warning msg should appear with YES/NO option for retied participant Hence OK option is no longer needed for finaldeathnotification.
            'If Not Session("Call_FinalDeathNotificationUpdation") Is Nothing Then
            '	If Session("Call_FinalDeathNotificationUpdation") = "Yes" Then
            '		Session("Call_FinalDeathNotificationUpdation") = Nothing
            '		'If Request.Form("Yes") = "Yes" Then
            '		'	If FinalDeathNotificationUpdation() = False Then
            '		'		Exit Sub
            '		'	End If
            '		'End If
            '		'BS:2012.02.20:BT-941,YRS 5.0-1432:- to handle Request.Form("OK") = "OK" here we are checking IsStatusDR value then to FinalDeathNotificationUpdation to open  report: List_of_Annuity_Checks_Sent_to_Retirees_After_Death.rpt and handle request.Ok
            '		If IsStatusDR = True Then
            '			If FinalDeathNotificationUpdation() = False Then
            '				Exit Sub
            '			End If
            '		End If
            '	End If
            'End If
            'END Bhavna:Sesion comment of detahnotify function YRS 5.0-1432

            'by Aparna -If the SSNO is Phony then  11/10/2007
            If Session("UpdateJSbtn_Click") = True Then
                'Session("UpdateJSbtn_Click") = False   'NP:2008.06.23:YRS-5.0-463 - This session variable is being used later. It should only be set to false, when its work is done. i.e. in the IF segment.
                If Session("PhonySSNo") = "Phony_SSNo" Then
                    Session("PhonySSNo") = Nothing
                    Session("UpdateJSbtn_Click") = False    'NP:2008.06.23:YRS-5.0-463 - Setting the session variable to false if it was invalidated
                    If Request.Form("Yes") = "Yes" Then
                        Session("UpdateJSbtn_Click") = True     'NP:2008.06.23:YRS-5.0-463 - Setting the session variable to true since it needs to be saved back
                        UpdateJointSurvivorDetails()
                    ElseIf Request.Form("No") = "No" Then

                    End If
                End If
            End If

            ' // START : SB | 07/07/2016 | YRS-AT-2382 | Check AuditLog table is updated or not
            If Session("IsAnnuityBeneficiariesSSNChanged") IsNot Nothing AndAlso Session("IsAnnuityBeneficiariesSSNChanged") Then
                SetUpdatedAnnuitySSN()
                HiddenFieldDirty.Value = "true"
                Session("IsAnnuityBeneficiariesSSNChanged") = Nothing
            End If
            ' // END : SB | 07/07/2016 | YRS-AT-2382 | Check AuditLog table is updated or not

            'Start: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Added to verify whether the user action to restrict web account info
            If ViewState("chkNoWebAcctCreate_verify") IsNot Nothing Then
                If Request.Form("No") = "No" Then
                    chkNoWebAcctCreate.Checked = False
                End If
                ButtonSaveRetireeParticipants.Enabled = True
                ButtonRetireesInfoCancel.Enabled = True
                Me.HyperLinkViewParticipantsInfo.Visible = False
                ViewState("chkNoWebAcctCreate_verify") = Nothing
                Exit Sub
            End If
            'End: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Added to verify whether the user action to restrict web account info
            'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.
            'modified by Aparna to make sure which part of the page is calling up the Message box 11/10/2007
            Dim blnSaveAttempt As Boolean = False
            If Session("SaveRetireeParticipantsInfo") = True Then
                Session("SaveRetireeParticipantsInfo") = Nothing
                If Request.Form("Yes") = "Yes" Then
                    SaveInfo()
                    'AA:15.11.2013:BT:1455:YRS 5.0-1733: Setting true to variable for when first time save procedure is called.
                    blnSaveAttempt = True
                ElseIf Request.Form("No") = "No" Then
                    Me.ButtonSaveRetireeParticipants.Enabled = True
                    Me.ButtonRetireesInfoCancel.Enabled = True
                End If
            End If
            If Session("PhonySSNo") = "Not_Phony_SSNo" Then
                Session("PhonySSNo") = Nothing
                Call SetControlFocus(Me.TextBoxGeneralSSNo)
            End If

            'Priya/Ashish: 21-April-2010:YRS 5.0-1056: Additional info when unsuppressing a JSANN annuity
            If Session("CurrentProcesstoConfirm") <> "UnSuppressProcess" Then
                'START: MMR | 2016.10.14 | YRS-AT-3063 | Commented existing code and added condition to avoid data to be saved on checkboxExhaustedDBSettle checkbox selection
                'If Request.Form("Yes") = "Yes" Then
                If Request.Form("Yes") = "Yes" And Not IsExhaustedDBRMDSettleOptionSelected Then
                    'END: MMR | 2016.10.14 | YRS-AT-3063 | Commented existing code and added condition to avoid data to be saved on checkboxExhaustedDBSettle checkbox selection
                    'AA:15.11.2013:BT:1455:YRS 5.0-1733:Checking whether already save has been fired
                    If Not blnSaveAttempt Then
                        Me.SaveRetiredParticipantInfo()
                        blnSaveAttempt = True
                    End If
                End If
            End If
            'END YRS 5.0-1056: Additional info when unsuppressing a JSANN annuity

            'Shubhrata Plan Split Changes
            Me.MakeLinkVisible()
            'Shagufta:27 July 2011- For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance
            ButtonRetireesInfoPHR.Enabled = True
            ButtonRetireesInfoOK.Enabled = True
            'End:27 July 2011- For BT-751,YRS 5.0-1268
            'Shubhrata Plan Split Changes

            'Start:Anudeep:04.10.2013: BT-2236:After modifying address save button gets disabled
            If AddressWebUserControl1.ChangesExist <> "" Or AddressWebUserControl2.ChangesExist <> "" Then
                Me.ButtonSaveRetireeParticipants.Enabled = True
                Me.ButtonRetireesInfoCancel.Enabled = True
                'AA:2013.10.23 - Bt-2264: below code added for diasabling the "active as a primary" button if person's address has been modified
                Me.ButtonActivateAsPrimary.Enabled = False
                'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Disabling deactivate button
                Me.btnDeactivateSecondaryAddrs.Enabled = False
                'End - Manthan | 2016.02.24 | YRS-AT-2328 | Disabling deactivate button
            End If
            'End:Anudeep:04.10.2013: BT-2236:After modifying address save button gets disabled

            'Start:AA:2013.10.23 - Bt-2264: below code and has been placed from ispostback condition
            If Not Session("Reason") Is Nothing Then
                If Session("Reason").ToString.ToLower = "yes" Then
                    Me.MultiPageRetireesInformation.SelectedIndex = 2
                    Me.TabStripRetireesInformation.SelectedIndex = 2
                    If Not Session("BenTableCellCollection") Is Nothing Then
                        DeleteRetired(CType(Session("BenTableCellCollection"), TableCellCollection), CType(Session("BenItemIndex"), Integer))
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Delete Beneficiary Reason Added Sucessfully.", MessageBoxButtons.OK)
                    End If
                ElseIf Session("Reason").ToString.ToLower = "no" Then
                    Me.MultiPageRetireesInformation.SelectedIndex = 2
                    Me.TabStripRetireesInformation.SelectedIndex = 2
                End If
                Session("Reason") = Nothing
                Session("BenTableCellCollection") = Nothing
                Session("BenItemIndex") = Nothing
            End If

            'End:AA:2013.10.23 - Bt-2264: below code and has been placed from ispostback condition

            'AA:2014.02.03 - BT:2292:YRS 5.0-2248 - To load PIN and fill in textbox
            LoadUserPINdetails()
            'Start - Manthan Rajguru | 2016.08.02 |YRS-AT-2382 | Enabling button based on session value
            If Not Session("EnableEditJSBeneficiaryButton") Is Nothing Then
                If Session("EnableEditJSBeneficiaryButton") = True Then
                    ButtonEditJSBeneficiary.Enabled = True
                    Session("EnableEditJSBeneficiaryButton") = Nothing
                End If
            End If
            'End - Manthan Rajguru | 2016.08.02 |YRS-AT-2382 | Enabling button based on session value
            'START: MMR | 2016.10.12 | YRS-AT-3063 | To check and Uncheck checkboxExhaustedDBSettle based on yes/no option selcted in message box
            If IsExhaustedDBRMDSettleOptionSelected Then
                ButtonSaveRetireeParticipants.Enabled = True
                If Not Request.Form("Yes") Is Nothing AndAlso Convert.ToString(Request.Form("Yes")).ToUpper() = "YES" Then
                    checkboxExhaustedDBSettle.Checked = True
                ElseIf Not Request.Form("No") Is Nothing AndAlso Convert.ToString(Request.Form("No")).ToUpper() = "NO" Then
                    checkboxExhaustedDBSettle.Checked = False
                End If
            End If
            'END: MMR | 2016.10.12 | YRS-AT-3063 | To check and Uncheck checkboxExhaustedDBSettle based on yes/no option selcted in message box




        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonGeneralPOA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGeneralPOA.Click
        'Response.Redirect("RetireesPowerAttorneyWebForm.aspx")
        Try

            'Added by neeraj on 01-dec-2009 : issue is YRS 5.0-940 security Check

            Dim checkSecurity1 As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If checkSecurity1.Equals("True") Then
                'Anudeep:07.11.2013:BT:2190-YRS 5.0-2199: Commented below line and added another line to get view acess to POA in read-only mode
                'Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralPOA", Convert.ToInt32(Session("LoggedUserKey")))
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralPOA", Convert.ToInt32(Session("LoggedUserKey")), True)
                If Not checkSecurity.Equals("True") And Not checkSecurity.Equals("Read-Only") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'Anudeep:07.11.2013:BT:2190-YRS 5.0-2199: Added below code for setting the rights of poa in session variable for setting poa in view/edit mode.
                Session("POA_Rights") = checkSecurity
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity1, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Session("POA") = True
            Dim popupScript As String = "<script language='javascript'>" & _
                       "window.open('RetireesPowerAttorneyWebForm.aspx', 'CustomPopUp', " & _
                       "'width=790, height=620, menubar=no, Resizable=No,top=80,left=120, scrollbars=yes')" & _
                       "</script>"

            Page.RegisterStartupScript("PopupScript10", popupScript)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonDeathNotification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDeathNotification.Click
        Try
            'Shagufta:27 July 2011- For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance
            If (ButtonSaveRetireeParticipants.Enabled = True And ButtonRetireesInfoCancel.Enabled = True) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please save your data before you proceed", MessageBoxButtons.OK, False)
                Exit Sub
            End If
            'End:27 July 2011- For BT-751,YRS 5.0-1268

            'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
            'START: SB | 2017.07.28 | YRS-AT-3324 | Added method to validate death notification restricitions for RMD eliglible participants
            'If Not Me.IsPersonEligibleForRMD Then
            '    Dim index As Integer = Me.ReasonForRestriction.IndexOf("<br />")
            '    'For multiple reasons for restricition display message with exteneded height and width
            '    If index >= 0 Then
            '        MessageBox.Show(170, 300, 560, 175, PlaceHolder1, "YMCA-YRS", Me.ReasonForRestriction, MessageBoxButtons.Stop, False)
            '        Exit Sub
            '    End If
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", ReasonForRestriction, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            'END: SB | 2017.07.28 | YRS-AT-3324 | Added method to validate death notification restricitions for RMD eliglible participants
            'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 

            Dim l_ds_CanBeKilled As DataSet
            l_ds_CanBeKilled = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetCanBeKilled(Session("FundId"))
            If IsDBNull(l_ds_CanBeKilled.Tables(0).Rows(0).Item("DeathFundEventStatus")) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "The Status of this Fund Event: " + l_ds_CanBeKilled.Tables(0).Rows(0).Item("StatusType") + " Prohibits beginning the Death Notification Process", MessageBoxButtons.Stop)
            Else
                'shagufta-20 July 2011   For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance
                'ButtonRetireesInfoOK.Enabled = False
                'End: BT-751,YRS 5.0-1268
                'ButtonRetireesInfoSave.Enabled = True 'by Aparna 27/08/07
                'Shagufta:27 July 2011- For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance
                'Me.ButtonSaveRetireeParticipants.Enabled = True
                'ButtonRetireesInfoCancel.Enabled = True
                'End:27 July 2011- For BT-751,YRS 5.0-1268
                'Session("Flag") = "Death" by aparna 27/09/2007
                Me.MakeLinkVisible()
                g_dataset_GeneralInfo = ViewState("g_dataset_GeneralInfo")
                If Not IsDBNull(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Deathdate")) Then
                    Session("Date_Deceased") = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Deathdate")
                Else
                    Session("Date_Deceased") = Nothing
                End If
                Dim popupScript As String = "<script language='javascript'>" & _
                        "window.open('RetireesDeathDatesWebForm.aspx', 'CustomPopUp', " & _
                        "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                        "</script>"

                Page.RegisterStartupScript("PopupScript10", popupScript)
                'Anudeep:28.02.2013 Added lastname and firstname in session to show in death benefit application additional forms
                Session("LastName") = TextBoxGeneralLastName.Text
                Session("FirstName") = TextBoxGeneralFirstName.Text
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridBankInfoList_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridBankInfoList.ItemCommand
        If e.CommandName.ToLower = "view" Then
            EditBankingInfo(e.Item.Cells, e.Item.ItemIndex)
        End If
    End Sub

    'Added BY Dinesh Kanojia on 18/12/2012
    Private Sub EditBankingInfo(ByVal DatarowGrid As TableCellCollection, ByVal iIndex As Integer)
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim i_Image_ImagebuttonEdit As ImageButton

            i_Image_ImagebuttonEdit = DatarowGrid(0).FindControl("ImagebuttonEdit")
            'AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonBankingInfoUpdate", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'Shubhrata
            Session("EffDateUpdateBank") = True
            Session("EffDateAddBank") = Nothing
            'Shubhrata
            'Response.Redirect("UpdateBankInformationWebForm.aspx")
            Session("blnShowBankingPage") = True
            'Ashutosh******************
            ''Session("R_BankName") = Me.DataGridBankInfoList.SelectedItem.Cells(1).Text.Trim
            Dim i As Integer
            i = ViewState("DataGridBankInfoListId")
            '********
            'If i > 0 Then
            'If Not DataGridBankInfoList.SelectedItem Is Nothing And i > -1 Then
            ''Session("R_BankName") = Me.DataGridBankInfoList.SelectedItem.Cells(1).Text.Trim
            ''Session("R_BankABANumber") = Me.DataGridBankInfoList.SelectedItem.Cells(2).Text.Trim
            ''Session("R_BankAccountNumber") = Me.DataGridBankInfoList.SelectedItem.Cells(3).Text.Trim
            ''Session("R_BankPaymentMethod") = Me.DataGridBankInfoList.SelectedItem.Cells(5).Text.Trim
            ''Session("R_BankAccountType") = Me.DataGridBankInfoList.SelectedItem.Cells(6).Text.Trim
            ''Session("R_BankEffectiveDate") = Me.DataGridBankInfoList.SelectedItem.Cells(4).Text.Trim
            'Ashutosh**************************
            Session("R_BankName") = DatarowGrid(1).Text.Trim
            Session("R_BankABANumber") = DatarowGrid(2).Text.Trim
            Session("R_BankAccountNumber") = DatarowGrid(3).Text.Trim
            Session("R_BankPaymentMethod") = DatarowGrid(5).Text.Trim
            Session("R_BankAccountType") = DatarowGrid(6).Text.Trim
            Session("R_BankEffectiveDate") = DatarowGrid(4).Text.Trim
            Session("Sel_SelectBank_GuiUniqueiD") = DatarowGrid(7).Text.Trim
            'Ashutosh********End******************
            ' ViewState("DataGridBankInfoListId") = -1
            Dim popupScript As String
            ' If Me.DataGridBankInfoList.Items(i).Cells(3).Text.Trim = "N/A" Then
            If DatarowGrid(8).Text = "&nbsp;" Or DatarowGrid(1).Text.Trim = "CHECK" Then
                popupScript = "<script language='javascript'>" & _
                           "window.open('UpdateBankInfo.aspx?Index=" & i & "', 'CustomPopUp', " & _
                           "'width=800, height=400, menubar=no, Resizable=No,top=80,left=120, scrollbars=yes,status=1')" & _
                           "</script>"

            Else
                popupScript = "<script language='javascript'>" & _
                         "window.open('UpdateBankInfo.aspx?UniqueID=" & DatarowGrid(8).Text & "', 'CustomPopUp', " & _
                         "'width=800, height=400, menubar=no, Resizable=NO,top=80,left=120, scrollbars=yes,status=1')" & _
                         "</script>"
            End If


            Page.RegisterStartupScript("PopupScript2", popupScript)
            'End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonBankingInfoUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonBankingInfoUpdate.Click
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonBankingInfoUpdate", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'Shubhrata
            Session("EffDateUpdateBank") = True
            Session("EffDateAddBank") = Nothing
            'Shubhrata
            'Response.Redirect("UpdateBankInformationWebForm.aspx")
            Session("blnShowBankingPage") = True
            'Ashutosh******************
            ''Session("R_BankName") = Me.DataGridBankInfoList.SelectedItem.Cells(1).Text.Trim
            Dim i As Integer
            i = ViewState("DataGridBankInfoListId")
            '********
            'If i > 0 Then
            If Not DataGridBankInfoList.SelectedItem Is Nothing And i > -1 Then
                ''Session("R_BankName") = Me.DataGridBankInfoList.SelectedItem.Cells(1).Text.Trim
                ''Session("R_BankABANumber") = Me.DataGridBankInfoList.SelectedItem.Cells(2).Text.Trim
                ''Session("R_BankAccountNumber") = Me.DataGridBankInfoList.SelectedItem.Cells(3).Text.Trim
                ''Session("R_BankPaymentMethod") = Me.DataGridBankInfoList.SelectedItem.Cells(5).Text.Trim
                ''Session("R_BankAccountType") = Me.DataGridBankInfoList.SelectedItem.Cells(6).Text.Trim
                ''Session("R_BankEffectiveDate") = Me.DataGridBankInfoList.SelectedItem.Cells(4).Text.Trim
                'Ashutosh**************************
                Session("R_BankName") = Me.DataGridBankInfoList.Items(i).Cells(1).Text.Trim
                Session("R_BankABANumber") = Me.DataGridBankInfoList.Items(i).Cells(2).Text.Trim
                Session("R_BankAccountNumber") = Me.DataGridBankInfoList.Items(i).Cells(3).Text.Trim
                Session("R_BankPaymentMethod") = Me.DataGridBankInfoList.Items(i).Cells(5).Text.Trim
                Session("R_BankAccountType") = Me.DataGridBankInfoList.Items(i).Cells(6).Text.Trim
                Session("R_BankEffectiveDate") = Me.DataGridBankInfoList.Items(i).Cells(4).Text.Trim
                Session("Sel_SelectBank_GuiUniqueiD") = Me.DataGridBankInfoList.Items(i).Cells(7).Text.Trim
                'Ashutosh********End******************
                ' ViewState("DataGridBankInfoListId") = -1
                Dim popupScript As String
                ' If Me.DataGridBankInfoList.Items(i).Cells(3).Text.Trim = "N/A" Then
                If Me.DataGridBankInfoList.SelectedItem.Cells(8).Text = "&nbsp;" Or Me.DataGridBankInfoList.Items(i).Cells(1).Text.Trim = "CHECK" Then
                    popupScript = "<script language='javascript'>" & _
                               "window.open('UpdateBankInfo.aspx?Index=" & i & "', 'CustomPopUp', " & _
                               "'width=800, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes,status=1')" & _
                               "</script>"

                Else
                    popupScript = "<script language='javascript'>" & _
                             "window.open('UpdateBankInfo.aspx?UniqueID=" & Me.DataGridBankInfoList.Items(i).Cells(8).Text & "', 'CustomPopUp', " & _
                             "'width=800, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes,status=1')" & _
                             "</script>"
                End If


                Page.RegisterStartupScript("PopupScript2", popupScript)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub TabStripRetireesInformation_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabStripRetireesInformation.SelectedIndexChange
        'Code written by Atul Deo on 4th Oct 2005 
        Try
            Me.MultiPageRetireesInformation.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
            'START : PK| 10/10/2019 |YRS-AT-4598 | Refresh Unsupress Div
            If TabStripRetireesInformation.SelectedIndex = 0 Then
                LoadDataInUnSuppressGrid()
            End If
            If TabStripRetireesInformation.SelectedIndex = 5 Then
                stwListUserControl.PersonStateName = AddressWebUserControl1.DropDownListStateText
                stwListUserControl.PersonStateCode = AddressWebUserControl1.DropDownListStateValue
            End If
            'END : PK| 10/10/2019 |YRS-AT-4598 | Refresh Unsupress Div 
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

        ''If TabStripRetireesInformation.SelectedIndex = 3 Then

        ''    'LoadAnnuityPaidTab()
        ''End If
        ''If TabStripRetireesInformation.SelectedIndex = 1 Then
        ''    Me.MultiPageRetireesInformation.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
        ''    'LoadAnnuitiesTab()
        ''End If
        ''If TabStripRetireesInformation.SelectedIndex = 2 Then
        ''    Me.MultiPageRetireesInformation.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
        ''    'LoadBeneficiariesTab()
        ''End If
        ''If TabStripRetireesInformation.SelectedIndex = 4 Then
        ''    Me.MultiPageRetireesInformation.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
        ''    'LoadBanksTab()
        ''End If
        ''If TabStripRetireesInformation.SelectedIndex = 5 Then
        ''    Me.MultiPageRetireesInformation.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
        ''    'LoadFedWithDrawalTab()
        ''End If
        ''If TabStripRetireesInformation.SelectedIndex = 6 Then
        ''    Me.MultiPageRetireesInformation.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
        ''    'LoadGenWithDrawalTab()
        ''End If
        ''If TabStripRetireesInformation.SelectedIndex = 7 Then
        ''    Me.MultiPageRetireesInformation.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
        ''    'LoadNotesTab()
        ''End If
        ''If TabStripRetireesInformation.SelectedIndex = 0 Then
        ''    Me.MultiPageRetireesInformation.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex
        ''    'LoadGeneraltab()
        ''End If
    End Sub
    'Added By Dinesh Kanojia on 18/12/2012
    ' START: SN | 11/14/2019 | YRS-AT-4604 | Commented the below lines of code to discard the functionality of ShowDisbursementInfo.aspx page, added GetTaxWithHoldingDetails to show tax withholding details in popup.
    'Private Sub DataGridAnnuitiesPaid_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridAnnuitiesPaid.ItemCommand
    '    Try


    '        If e.CommandName.ToLower = "view" Then
    '            Dim popupScript As String = "<script language='javascript'>" & _
    '                          "window.open('ShowDisbursementInfo.aspx?DisbursementID=" & e.Item.Cells(9).Text & "', 'CustomPopUp', " & _
    '                          "'width=780, height=300, menubar=no, Resizable=No,top=120,left=120, scrollbars=yes')" & _
    '                          "</script>"
    '            Page.RegisterStartupScript("PopupScript1", popupScript)

    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    ' END: SN | 11/14/2019 | YRS-AT-4604 | Commented the below lines of code to discard the functionality of ShowDisbursementInfo.aspx page, added GetTaxWithHoldingDetails to show tax withholding details in popup.
    'End of code written by Atul Deo on 4th Sep 2005 for Annuities Paid.
    Private Sub DataGridAnnuitiesPaid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAnnuitiesPaid.SelectedIndexChanged
        'Session("DisbursementID") = Me.DataGridAnnuitiesPaid.SelectedItem.Cells(9).Text
        'Response.Redirect("ShowDisbursementInfo.aspx")
        'Response.Write("<script language=""javascript"">javascript: return NewWindow('ShowDisbursementInfo.aspx?DisbursementID=" & Me.DataGridAnnuitiesPaid.SelectedItem.Cells(9).Text & "','mywin','600','400','yes','center');</script>")

    End Sub
    Private Sub DataGridAnnuitiesPaid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridAnnuitiesPaid.ItemDataBound
        Try
            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("Imagebutton6")
            If (e.Item.ItemIndex = Me.DataGridAnnuitiesPaid.SelectedIndex And Me.DataGridAnnuitiesPaid.SelectedIndex >= 0) Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            e.Item.Cells(9).Visible = False

            If e.Item.ItemType <> ListItemType.Header Then
                e.Item.Cells(1).HorizontalAlign = HorizontalAlign.Center
                e.Item.Cells(4).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(5).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridAnnuities_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridAnnuities.ItemCommand
        Try

            'DataGridAnnuities.SelectedItem.ID = e.Item.ID
            'DataGridAnnuities.SelectedItem.Style.Add("font-weight", "bold")


            If e.CommandName.ToLower = "view" Then

                DataGridAnnuities.Items(e.Item.ItemIndex).Style.Add("font-weight", "bold")

                Dim popupScript As String = "<script language='javascript'>" & _
                                       "window.open('ShowAnnuityDetails.aspx?AnnuityID=" & e.Item.Cells(8).Text & "', 'CustomPopUp', " & _
                                       "'width=750, height=500, menubar=no, Resizable=No,top=80,left=120, scrollbars=yes')" & _
                                       "</script>"
                Page.RegisterStartupScript("PopupScript1", popupScript)
                'Dineshk            2013.09.18      YRS 5.0-2204:Cannot see other annuity beneficiaries in Annuities display
                LoadJointSurvivorDetails(e.Item.Cells(9).Text)
                'Me.ButtonAnnuitiesViewDetails.Visible = True
                'Me.ButtonAnnuitiesViewDetails.Enabled = True
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Commented By Dinesh On 18/12/2012
    'Dineshk            2013.09.18      YRS 5.0-2204:Cannot see other annuity beneficiaries in Annuities display
    Private Sub DataGridAnnuities_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAnnuities.SelectedIndexChanged
        'Dim popupScript As String = "<script language='javascript'>" & _
        '                                                 "window.open('ShowAnnuityDetails.aspx?AnnuityID=" & Me.DataGridAnnuities.SelectedItem.Cells(8).Text & "', 'CustomPopUp', " & _
        '                                                 "'width=700, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
        '                                                "</script>"
        'Page.RegisterStartupScript("PopupScript1", popupScript)
        Try
            'Dim i As Integer
            'For i = 0 To Me.DataGridAnnuities.Items.Count - 1
            '    Dim l_button_Select As ImageButton
            '    l_button_Select = DataGridAnnuities.Items(i).FindControl("Imagebutton6")
            '    If Not l_button_Select Is Nothing Then
            '        If i = DataGridAnnuities.SelectedIndex Then
            '            l_button_Select.ImageUrl = "images\selected.gif"
            '        Else
            '            l_button_Select.ImageUrl = "images\select.gif"
            '        End If
            '    End If

            'Next
            'by Aparna 04/09/2007
            'to load the details of the J & S Survivor for the Annuity selected.
            'LoadJointSurvivorDetails()
            'Dineshk            2013.09.18      YRS 5.0-2204:Cannot see other annuity beneficiaries in Annuities display
            DataGridAnnuities.SelectedItem.Style.Add("font-weight", "bold")
            Me.ButtonAnnuitiesViewDetails.Visible = True
            Me.ButtonAnnuitiesViewDetails.Enabled = True
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonBankingInfoAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonBankingInfoAdd.Click
        Dim l_string_Message As String
        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
        HiddenFieldDirty.Value = "true"
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonBankingInfoAdd", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            'Shubhrata
            Session("EffDateAddBank") = True
            Session("EffDateUpdateBank") = Nothing
            'Shubhrata
            Session("blnAddBankingRetirees") = True
            Session("EnableSaveCancel") = True

            Session("blnShowBankingPage") = True

            Dim msg1 As String
            msg1 = msg1 & "<Script Language='JavaScript'>"
            msg1 = msg1 & "window.open('UpdateBankInfo.aspx','UpdateBankInfo','width=800, height=500, menubar=no, Resizable=No,top=80,left=120, scrollbars=yes,status=1')"
            msg1 = msg1 & "</script>"
            Page.RegisterStartupScript("PopupScriptNew", msg1)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub
    'by Aparna 27/08/07
    'Private Sub ButtonRetireesInfoSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRetireesInfoSave.Click
    '    Dim l_string_Message As String
    '    Try
    '        If Me.IsValid Then
    '            Dim l_DateInFuture As Date
    '            Dim l_DateInPast As Date
    '            Dim l_PrimDate As Date
    '            Dim l_SecDate As Date
    '            Dim l_PrimaryDate As Date
    '            Dim l_ds_PrimaryAddress As DataSet
    '            Dim l_ds_SecondaryAddress As DataSet
    '            Dim l_SecondaryDate As Date
    '            l_DateInPast = Date.Now.AddYears(-1)
    '            l_DateInFuture = Date.Now.AddYears(1)

    '            If Not Session("Ds_PrimaryAddress") Is Nothing Then
    '                l_ds_PrimaryAddress = CType(Session("Ds_PrimaryAddress"), DataSet)
    '                'Primary Address section starts
    '                If l_ds_PrimaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
    '                    If (l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "System.DBNull" And l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "") Then
    '                        l_PrimaryDate = CType(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate"), Date)
    '                    Else
    '                        l_PrimaryDate = "02/02/1900"
    '                    End If
    '                Else
    '                    l_PrimaryDate = "02/02/1900"
    '                End If
    '            Else
    '                l_PrimaryDate = "02/02/1900"
    '            End If


    '            If Not Session("Ds_SecondaryAddress") Is Nothing Then
    '                l_ds_SecondaryAddress = CType(Session("Ds_SecondaryAddress"), DataSet)
    '                'Secondary Address section starts
    '                If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
    '                    If (l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "System.DBNull" And l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "") Then
    '                        l_SecondaryDate = CType(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate"), Date)
    '                    Else
    '                        l_SecondaryDate = "02/02/1900"
    '                    End If
    '                Else
    '                    l_SecondaryDate = "02/02/1900"
    '                End If
    '            Else
    '                l_SecondaryDate = "02/02/1900"
    '            End If


    '            If Me.TextBoxPriEffDate.Text <> "" AndAlso Me.TextBoxPriEffDate.Text <> l_PrimaryDate Then
    '                l_PrimaryDate = CType(Me.TextBoxPriEffDate.Text, Date)

    '                If Date.Compare(l_DateInPast, l_PrimaryDate) > 0 Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
    '                    Exit Sub
    '                End If

    '                If Date.Compare(l_PrimaryDate, l_DateInFuture) > 0 Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
    '                    Exit Sub
    '                End If

    '            End If



    '            If Me.TextBoxSecEffDate.Text <> "" AndAlso Me.TextBoxSecEffDate.Text <> l_SecondaryDate Then
    '                l_SecondaryDate = CType(Me.TextBoxSecEffDate.Text, Date)

    '                If Date.Compare(l_DateInPast, l_SecondaryDate) > 0 Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
    '                    Exit Sub
    '                End If

    '                If Date.Compare(l_SecondaryDate, l_DateInFuture) > 0 Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
    '                    Exit Sub
    '                End If

    '            End If
    '        End If


    '        'Ashutosh Patil as on 25-Apr-2007
    '        'YREN-3298
    '        If Me.ButtonEditAddress.Enabled = False Then
    '            'YREN - 3028,YREN-3029
    '            'Primary Address
    '            Call SetPrimaryAddressDetails()
    '            'Secondary Address
    '            Call SetSecondaryAddressDetails()

    '            'Added By Ashutosh Patil as on 23-Feb-2007 For YREN - 3029,YREN - 3028
    '            If Trim(m_str_Pri_Address1) = "" Then
    '                MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Primary Address.", MessageBoxButtons.Stop)
    '                Exit Sub
    '            End If

    '            If Trim(m_str_Pri_City) = "" Then
    '                MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Primary Address.", MessageBoxButtons.Stop)
    '                Exit Sub
    '            End If

    '            If Trim(m_str_Pri_Address1) <> "" Then
    '                If m_str_Pri_CountryText = "-Select Country-" And m_str_Pri_StateText = "-Select State-" Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
    '                    Exit Sub
    '                End If
    '            End If

    '            If (m_str_Pri_CountryText = "-Select Country-") Then
    '                If m_str_Pri_StateText = "-Select State-" Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
    '                    Exit Sub
    '                End If
    '            End If

    '            l_string_Message = ValidateCountrySelStateZip(m_str_Pri_CountryValue, m_str_Pri_StateValue, m_str_Pri_Zip)
    '            If l_string_Message <> "" Then
    '                MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
    '                Exit Sub
    '            End If

    '            If m_str_Pri_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Pri_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
    '                TextBoxZip.Text = ""
    '                MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
    '                Exit Sub
    '            End If

    '            'Secondary Address Validations
    '            'Ashutosh Patil as on 14-Mar-2007
    '            'YREN - 3028,YREN-3029
    '            If Trim(m_str_Sec_Address1) <> "" Then
    '                If Trim(m_str_Sec_City) = "" Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Secondary Address.", MessageBoxButtons.Stop)
    '                    Exit Sub
    '                End If
    '                If (m_str_Sec_CountryText = "-Select Country-") Then
    '                    If m_str_Sec_StateText = "-Select State-" Then
    '                        MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Secondary Address.", MessageBoxButtons.Stop)
    '                        Exit Sub
    '                    End If
    '                End If
    '            End If

    '            If Trim(m_str_Sec_Address2) <> "" Then
    '                If Trim(m_str_Sec_Address1) = "" Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
    '                    Exit Sub
    '                End If
    '            End If

    '            If Trim(m_str_Sec_Address3) <> "" Then
    '                If Trim(m_str_Sec_Address1) = "" Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
    '                    Exit Sub
    '                End If
    '            End If

    '            'Ashutosh Patil as on 25-Apr-2007
    '            'YREN-3298
    '            If m_str_Sec_City <> "" Then
    '                If Trim(m_str_Sec_Address1.ToString) = "" Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
    '                    Exit Sub
    '                End If
    '            End If

    '            If (m_str_Sec_CountryText <> "-Select Country-" Or m_str_Sec_StateText <> "-Select State-") Then
    '                If Trim(m_str_Sec_Address1) = "" Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
    '                    Exit Sub
    '                End If
    '            End If

    '            If Trim(m_str_Sec_Zip) <> "" Then
    '                If Trim(m_str_Sec_Address1) = "" Then
    '                    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
    '                    Exit Sub
    '                End If
    '            End If

    '            l_string_Message = ValidateCountrySelStateZip(m_str_Sec_CountryValue, m_str_Sec_StateValue, m_str_Sec_Zip)

    '            If l_string_Message <> "" Then
    '                MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
    '                Exit Sub
    '            End If

    '            If m_str_Sec_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Sec_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
    '                MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
    '                Exit Sub
    '            End If

    '        End If

    '        'Added By Ashutosh Patil as on 06-Jun-2007
    '        'Validations for Phony SSNo
    '        'YREN(-3490)
    '        If Me.TextBoxGeneralSSNo.Enabled = True Then
    '            l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.TextBoxGeneralSSNo.Text.Trim())

    '            If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
    '                Session("PhonySSNo") = "Not_Phony_SSNo"
    '                Call SetControlFocus(Me.TextBoxGeneralSSNo)
    '                MessageBox.Show(PlaceHolder1, "YMCA", "Invalid SSNo entered, Please enter a Valid SSNo.", MessageBoxButtons.Stop)
    '                Exit Sub
    '            ElseIf l_string_Message.ToString().Trim() = "Phony_SSNo" Then
    '                MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Phony SSNo ?", MessageBoxButtons.YesNo)
    '                Session("PhonySSNo") = "Phony_SSNo"
    '                Exit Sub
    '            ElseIf l_string_Message.ToString().Trim() = "No_Configuration_Key" Then
    '                Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
    '            End If
    '        End If
    '        SaveInfo()
    '        Me.ButtonRetireesInfoSave.Visible = True
    '      '' Me.ButtonSaveRetireeParticipants.Visible = False
    '        Me.ButtonSaveRetireeParticipants.Enabled = False
    '        Me.ButtonRetireesInfoCancel.Enabled = False
    '        Me.ButtonRetireesInfoOK.Enabled = True
    '        Me.MakeLinkVisible()
    '        Me.ButtonRetireesInfoOK.Enabled = True

    '    Catch sqlEx As SqlException
    '        Dim l_String_Exception_Message As String
    '        If sqlEx.Number = 60006 And sqlEx.Procedure.ToString = "yrs_usp_AMCM_SearchConfigurationMaintenance" Then
    '            l_String_Exception_Message = "No Key defined for Phony SSNo in AtsMetaConfiguration."
    '        Else
    '            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
    '        End If
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    Private Sub ButtonFederalWithholdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFederalWithholdAdd.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonFederalWithholdAdd", Convert.ToInt32(Session("LoggedUserKey")))

            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Session("blnAddFedWithHolding") = True
            'Dim msg1 As String
            'msg1 = msg1 & "<Script Language='JavaScript'>"
            'msg1 = msg1 & "window.open('UpdateFedWithHoldingInfo.aspx','map')"
            'msg1 = msg1 & "</script>"
            'Response.Write(msg1)
            Session("EnableSaveCancel") = True

            'Shubhrata Mar 2nd,2007 YREN-3112
            'Session("IsNotFedTaxForMaritalStatus") = True  'commented by hafiz on 2-May-2007 for YREN-3112
            Session("IsFedTaxForMaritalStatus") = True  'added by hafiz on 2-May-2007 for YREN-3112        
            'Shubhrata Mar 2nd,2007 YREN-3112

            Dim popupScript As String = "<script language='javascript'>" & _
                         "window.open('UpdateFedWithHoldingInfo.aspx', 'CustomPopUp', " & _
                         "'width=800, height=400, menubar=no, Resizable=No,top=80,left=120, scrollbars=yes')" & _
                         "</script>"


            Page.RegisterStartupScript("PopupScript2", popupScript)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonGeneralWithholdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGeneralWithholdAdd.Click
        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralWithholdAdd", Convert.ToInt32(Session("LoggedUserKey")))
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            Session("blnAddGenWithHolding") = True

            Session("EnableSaveCancel") = True
            Dim popupScript As String = "<script language='javascript'>" & _
                         "window.open('UpdateGenHoldings.aspx', 'CustomPopUp', " & _
                         "'width=800, height=400, menubar=no, resizable=No,top=80,left=120, scrollbars=yes')" & _
                         "</script>"


            Page.RegisterStartupScript("PopupScript3", popupScript)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'Start:Anudeep:24.06.2014 BT-2582 : Commented 
    'Private Sub ButtonAddBeneficiaries_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddBeneficiaries.Click
    '    Try

    '        'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
    '        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAddBeneficiaries", Convert.ToInt32(Session("LoggedUserKey")))

    '        If Not checkSecurity.Equals("True") Then
    '            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
    '            Exit Sub
    '        End If
    '        'End : YRS 5.0-940

    '        Session("blnAddBeneficiaries") = True

    '        Session("EnableSaveCancel") = True

    '        Session("MaritalStatus") = cboGeneralMaritalStatus.SelectedValue 'YRPS-4704

    '        Dim popupScript As String = "<script language='javascript'>" & _
    '                     "window.open('UpdateBeneficiaries.aspx', 'CustomPopUp', " & _
    '                     "'width=650, height=500, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
    '                     "</script>"
    '        Page.RegisterStartupScript("PopupScript4", popupScript)

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    'End:Anudeep:24.06.2014 BT-2582 : Commented 
    Private Sub ButtonNotesAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonNotesAdd.Click
        Try
            'Start:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonNotesAdd", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            'HiddenFieldDirty.Value = "true" 'Manthan | 2016.04.01 | YRS-AT-1718 | Commented to not display alert message when user clicks on close button after adding notes
            Session("Note") = ""
            'Start: Bala: YRS-AT-1718: 01/12/2016: Ading notes
            'Session("EnableSaveCancel") = True  'This is not required because we are saving ti directly to the database.
            'Session("blnAddNotes") = True
            Session("NotesEntityID") = Session("PersID")
            'End: Bala: YRS-AT-1718: 01/12/2016: Ading notes
            Dim popupScript As String = "<script language='javascript'>" & _
                      "window.open('UpdateNotes.aspx', 'CustomPopUp', " & _
                      "'width=800, height=400, menubar=no, resizable=No,top=80,left=120, scrollbars=yes')" & _
                      "</script>"
            Page.RegisterStartupScript("PopupScript2", popupScript)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub DataGridBankInfoList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridBankInfoList.SelectedIndexChanged
        Try
            'ViewState("DataGridBankInfoListId") = DataGridBankInfoList.SelectedIndex
            ''Page.RegisterStartupScript("PopupScript2", popupScript)
            ''Modifications Done for Change to Check Payment in Banking 
            'Dim dr As DataRow
            'Dim cache As CacheManager
            'Dim drExisting As DataRow
            'Dim dtValues As New DataTable
            'Dim drNew As DataRow
            'Dim l_dataset_Banks As New DataSet
            ''Added By Ashutosh Patil as on 12-01-2007
            'Dim l_button_Select As New ImageButton
            'Dim l_dgitem As DataGridItem
            ''aparna -YREN-3037 14/02/2007
            'ButtonBankingInfoUpdate.Enabled = True
            ''aparna -YREN-3037 14/02/2007
            ''Vipul 01Feb06 Cache-Session
            ''l_dataset_Banks = cache("BankingDtls")
            'l_dataset_Banks = Session("BankingDtls")
            ''Vipul 01Feb06 Cache-Session
            'drExisting = l_dataset_Banks.Tables(0).Rows(DataGridBankInfoList.SelectedIndex)
            'If drExisting("PaymentDesc") = "EFT" Then
            '    ButtonBankingInfoCheckPayment.Enabled = True
            'Else
            '    ButtonBankingInfoCheckPayment.Enabled = False
            'End If
            'If Not l_dataset_Banks.Tables(0).GetChanges Is Nothing Then
            '    DataGridBankInfoList.DataSource = l_dataset_Banks
            '    DataGridBankInfoList.DataBind()
            'End If
            '''dtValues.Clear()
            '''dtValues = l_dataset_Banks.Tables(0)
            ''''dtValues.Select("chrEFTStatus='0'")
            '''dtValues.Select("EffecDate=MAX(EffecDate)")
            '''If dtValues.Rows(0).Item(9) = "V" Then
            '''    Me.LabelCurrentEFT.Text = "Cuurent Pay Method: CHECK"
            '''End If
            ''Added By Ashutosh Patil as on 12-01-2007
            ''Start Ashutosh Patil chrEFTStatus where EffecDate=" &
            'For Each l_dgitem In Me.DataGridBankInfoList.Items
            '    l_button_Select = l_dgitem.FindControl("Imagebutton2")
            '    If l_dgitem.ItemIndex = Me.DataGridBankInfoList.SelectedIndex Then
            '        l_button_Select.ImageUrl = "images\selected.gif"
            '    Else
            '        l_button_Select.ImageUrl = "images\select.gif"
            '    End If
            'Next
            ''End Ashutosh Patil
            ''Session("R_BankName") = Me.DataGridBankInfoList.SelectedItem.Cells(1).Text.Trim
            ''Session("R_BankABANumber") = Me.DataGridBankInfoList.SelectedItem.Cells(2).Text.Trim
            ''Session("R_BankAccountNumber") = Me.DataGridBankInfoList.SelectedItem.Cells(3).Text.Trim
            ''Session("R_BankPaymentMethod") = Me.DataGridBankInfoList.SelectedItem.Cells(5).Text.Trim
            ''Session("R_BankAccountType") = Me.DataGridBankInfoList.SelectedItem.Cells(6).Text.Trim
            'Session("R_BankEffectiveDate") = Me.DataGridBankInfoList.SelectedItem.Cells(4).Text.Trim
            ''Dim popupScript As String

            ''If Me.DataGridBankInfoList.SelectedItem.Cells(7).Text = "&nbsp;" Then
            ''    popupScript = "<script language='javascript'>" & _
            ''                                       "window.open('UpdateBankInfo.aspx?Index=" & Me.DataGridBankInfoList.SelectedIndex & "', 'CustomPopUp', " & _
            ''                                       "'width=800, height=800, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
            ''                                       "</script>"
            ''Else
            ''    popupScript = "<script language='javascript'>" & _
            ''                                        "window.open('UpdateBankInfo.aspx?UniqueID=" & Me.DataGridBankInfoList.SelectedItem.Cells(7).Text & "', 'CustomPopUp', " & _
            ''                                        "'width=800, height=800, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
            ''                                        "</script>"
            ''End If
            ''Code Edited by ashutosh****on 08-05-06
            '''ViewState("DataGridBankInfoListId") = DataGridBankInfoList.SelectedIndex
            ''''Page.RegisterStartupScript("PopupScript2", popupScript)
            ''''Modifications Done for Change to Check Payment in Banking 
            '''Dim dr As DataRow
            '''Dim cache As CacheManager
            '''Dim drExisting As DataRow
            '''Dim drNew As DataRow
            '''Dim l_dataset_Banks As New DataSet
            '''cache = CacheFactory.GetCacheManager()
            ''''Vipul 01Feb06 Cache-Session
            ''''l_dataset_Banks = cache("BankingDtls")
            '''l_dataset_Banks = Session("BankingDtls")
            ''''Vipul 01Feb06 Cache-Session
            '''drExisting = l_dataset_Banks.Tables(0).Rows(DataGridBankInfoList.SelectedIndex)
            '''If drExisting("PaymentDesc") = "EFT" Then
            '''    ButtonBankingInfoCheckPayment.Enabled = True
            '''Else
            '''    ButtonBankingInfoCheckPayment.Enabled = False
            '''End If
            ''************************************************
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridBeneficiaries.SelectedIndexChanged
        Try
            Session("Name") = Me.DataGridBeneficiaries.SelectedItem.Cells(1).Text.Trim()
            Session("Name2") = Me.DataGridBeneficiaries.SelectedItem.Cells(2).Text.Trim
            Session("TaxID") = Me.DataGridBeneficiaries.SelectedItem.Cells(3).Text.Trim
            Session("Rel") = Me.DataGridBeneficiaries.SelectedItem.Cells(4).Text.Trim
            Session("Birthdate") = Me.DataGridBeneficiaries.SelectedItem.Cells(5).Text.Trim
            Session("Groups") = Me.DataGridBeneficiaries.SelectedItem.Cells(6).Text.Trim
            Session("Lvl") = Me.DataGridBeneficiaries.SelectedItem.Cells(7).Text.Trim
            Session("Pct") = Me.DataGridBeneficiaries.SelectedItem.Cells(8).Text.Trim
            Session("MaritalStatus") = cboGeneralMaritalStatus.SelectedValue 'YRPS-4704


            Dim popupScript As String
            If Me.DataGridBeneficiaries.SelectedItem.Cells(6).Text = "&nbsp;" Then
                popupScript = "<script language='javascript'>" & _
                         "window.open('UpdateBeneficiaries.aspx?Index=" & Me.DataGridBeneficiaries.SelectedIndex & "', 'CustomPopUp', " & _
                         "'width=650, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                         "</script>"
            Else
                popupScript = "<script language='javascript'>" & _
                          "window.open('UpdateBeneficiaries.aspx?UniqueID=" & Me.DataGridBeneficiaries.SelectedItem.Cells(9).Text & "', 'CustomPopUp', " & _
                          "'width=650, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                            "</script>"
            End If
            Page.RegisterStartupScript("PopupScript2", popupScript)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub DataGridFederalWithholding_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridFederalWithholding.SelectedIndexChanged
        ''Dim l_button_Select As ImageButton
        ''Try
        ''    l_button_Select = e.Item.FindControl("ImageButtonSelect")
        ''    If (e.Item.ItemIndex = Me.DataGridFederalWithholding.SelectedIndex And Me.DataGridFederalWithholding.SelectedIndex >= 0) Then
        ''        l_button_Select.ImageUrl = "images\selected.gif"
        ''    End If
        ''    e.Item.Cells(1).Visible = False
        ''    e.Item.Cells(2).Visible = False
        ''    'start of comment by hafiz on 24-Jan-2006
        ''    'e.Item.Cells(6).Visible = False
        ''    'end of comment by hafiz on 24-Jan-2006

        ''    'start of addition by hafiz on 24-Jan-2006
        ''    e.Item.Cells(5).Visible = False
        ''    'end of addition by hafiz on 24-Jan-2006

        ''Catch ex As Exception

        ''    Dim l_String_Exception_Message As String
        ''    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        ''    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        ''End Try
        '''Session("blnAddFedWithHolding") = True
        '''Put all the values in the grid in the session variables
        ''Session("cmbTaxEntity") = Me.DataGridFederalWithholding.SelectedItem.Cells(4).Text.Trim
        ''Session("cmbWithHolding") = Me.DataGridFederalWithholding.SelectedItem.Cells(3).Text.Trim
        ''Session("txtExemptions") = Me.DataGridFederalWithholding.SelectedItem.Cells(1).Text.Trim
        ''Session("txtAddlAmount") = Me.DataGridFederalWithholding.SelectedItem.Cells(2).Text.Trim
        ''Session("cmbMaritalStatus") = Me.DataGridFederalWithholding.SelectedItem.Cells(5).Text.Trim
        ''Dim popupScript As String
        ''If Me.DataGridFederalWithholding.SelectedItem.Cells(6).Text = "&nbsp;" Then
        ''    popupScript = "<script language='javascript'>" & _
        ''                                       "window.open('UpdateFedWithHoldingInfo.aspx?Index=" & Me.DataGridFederalWithholding.SelectedIndex & "', 'CustomPopUp', " & _
        ''                                       "'width=450, height=400, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
        ''                                       "</script>"
        ''Else
        ''    popupScript = "<script language='javascript'>" & _
        ''                                        "window.open('UpdateFedWithHoldingInfo.aspx?UniqueID=" & Me.DataGridFederalWithholding.SelectedItem.Cells(6).Text & "', 'CustomPopUp', " & _
        ''                                        "'width=450, height=400, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
        ''                                        "</script>"
        ''End If
        ''Page.RegisterStartupScript("PopupScript2", popupScript)
        'Getting the datarow corresponding to the UniqueID
        'Added by Swopna on 28 Dec,2007 in response to bug id 321
        Try

            '********************

            Dim l_dgitem As DataGridItem
            Dim l_button_Select As ImageButton
            For Each l_dgitem In Me.DataGridFederalWithholding.Items
                l_button_Select = CType(l_dgitem.FindControl("Imagebutton3"), ImageButton)
                If l_dgitem.ItemIndex = Me.DataGridFederalWithholding.SelectedIndex Then
                    l_button_Select.ImageUrl = "images\selected.gif"
                Else
                    l_button_Select.ImageUrl = "images\select.gif"
                End If
            Next
            '********************

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'Added By Dinesh Kanojia on 21/12/2012
    Private Sub DataGridFederalWithholding_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridFederalWithholding.ItemCommand
        If e.CommandName.ToLower = "edit" Then
            EditFederalWithHolding(e.Item.Cells, e.Item.ItemIndex)
        End If
    End Sub
    'Added By Dinesh Kanojia on 21/12/2012
    Private Sub DataGridGeneralWithhold_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridGeneralWithhold.ItemCommand
        If e.CommandName.ToLower = "edit" Then
            EditGeneralWithHolding(e.Item.Cells, e.Item.ItemIndex)
        End If
    End Sub

    Private Sub DataGridGeneralWithhold_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridGeneralWithhold.SelectedIndexChanged
        '''Session("cmbWithHoldingType") = Me.DataGridGeneralWithhold.SelectedItem.Cells(1).Text.Trim
        '''Session("txtAddAmount") = Me.DataGridGeneralWithhold.SelectedItem.Cells(2).Text.Trim
        '''Session("txtStartDate") = Me.DataGridGeneralWithhold.SelectedItem.Cells(3).Text.Trim
        '''Session("txtEndDate") = Me.DataGridGeneralWithhold.SelectedItem.Cells(4).Text.Trim
        '''Dim popupScript As String
        '''If Me.DataGridGeneralWithhold.SelectedItem.Cells(5).Text = "&nbsp;" Then
        '''    popupScript = "<script language='javascript'>" & _
        '''                                        "window.open('UpdateGenHoldings.aspx?Index=" & Me.DataGridGeneralWithhold.SelectedIndex & "', 'CustomPopUp', " & _
        '''                                        "'width=550, height=400, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
        '''                                        "</script>"
        '''Else
        '''    popupScript = "<script language='javascript'>" & _
        '''                                        "window.open('UpdateGenHoldings.aspx?UniqueID=" & Me.DataGridGeneralWithhold.SelectedItem.Cells(5).Text & "', 'CustomPopUp', " & _
        '''                                        "'width=550, height=400, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
        '''                                        "</script>"
        '''End If
        '''Page.RegisterStartupScript("PopupScript2", popupScript)
        'Added by Swopna on 28 Dec,2007 in response to bug id 321
        '********************
        Try
            Dim l_dgitem As DataGridItem
            Dim l_button_Select As ImageButton
            For Each l_dgitem In Me.DataGridGeneralWithhold.Items
                l_button_Select = CType(l_dgitem.FindControl("Imagebutton4"), ImageButton)
                If l_dgitem.ItemIndex = Me.DataGridGeneralWithhold.SelectedIndex Then
                    l_button_Select.ImageUrl = "images\selected.gif"
                Else
                    l_button_Select.ImageUrl = "images\select.gif"
                End If
            Next
            '********************

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'Added by Dinesh Kanojia on 21/12/2012
    Private Sub DataGridNotes_ItemCommad(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridNotes.ItemCommand
        Try
            'Save: Bala: 01/12/2016: YRS-AT-1718: Delete Notes
            'EditNotes(e.Item.Cells, e.Item.ItemIndex)
            If e.CommandName.ToLower = "delete" Then
                DeleteRetireeNotes(e.Item.Cells)
            Else
                'START: VC | 2018.06.14 | YRS-AT-4010 | Added condition to check index is not equal to -1
                'EditNotes(e.Item.Cells, e.Item.ItemIndex)
                If e.Item.ItemIndex <> -1 Then
                    EditNotes(e.Item.Cells, e.Item.ItemIndex)
                End If
                'END: VC | 2018.06.14 | YRS-AT-4010 | Added condition to check index is not equal to -1
            End If
            'End: Bala: 01/12/2016: YRS-AT-1718: Delete Notes
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'Added by Dinesh Kanojia on 21/12/2012

    'Start: Bala: 01/12/2016: YRS-AT-1718: Display confirmation message for delete note action
    ''' <summary>
    ''' Showing message box for delete confirmation
    ''' </summary>
    ''' <param name="dgRow"></param>
    ''' <remarks></remarks>
    Public Sub DeleteRetireeNotes(ByVal dgRow As TableCellCollection)
        Try
            Session("CurrentProcesstoConfirm") = "DeleteNotes"
            If dgRow(2).Text.ToString.ToLower() = Convert.ToString(Session("LoginId")).ToLower() Then 'Manthan Rajguru | 2016.04.05 | YRS-AT-1718 | Converting login ID to lower case from both the side to avoid any mismatches
                Session("NotesToDelete") = dgRow(4).Text.ToString
                'Start: Bala: 03.08.2016: YRS-AT-1718: Confimation message box.
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you want to delete this notes.", MessageBoxButtons.YesNo)
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you want to delete the following note?<br /><br />Date:" & dgRow(1).Text.ToString & "<br />Note:" & dgRow(3).Text.ToString, MessageBoxButtons.YesNo)
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
    'End: Bala: 01/12/2016: YRS-AT-1718: Display confirmation message for delete note action

    Private Sub EditNotes(ByVal Datagridrow As TableCellCollection, ByVal iIndex As Integer)

        Dim l_datatable_Notes As DataTable
        l_datatable_Notes = Session("DisplayNotes")
        '******
        Try
            'Commented/added for YRPS-4614
            '*******
            'Session("Note") = Me.DataGridNotes.SelectedItem.Cells(3).Text.Trim

            If l_datatable_Notes.Rows(iIndex)("Note").GetType.ToString <> "System.DBNull" Then
                Session("Note") = l_datatable_Notes.Rows(iIndex)("Note")
            Else
                Session("Note") = ""
            End If
            '*******

            'by aparna get the checkbox value  YREN-3115
            Dim l_checkbox As New CheckBox
            l_checkbox = Datagridrow(5).FindControl("CheckBoxImportant")

            If l_checkbox.Checked Then
                Session("BitImportant") = True
            Else
                Session("BitImportant") = False
            End If
            Dim popupScript As String
            If Datagridrow(4).Text = "&nbsp;" Then
                popupScript = "<script language='javascript'>" & _
                           "window.open('UpdateNotes.aspx?Index=" & iIndex & "', 'CustomPopUp', " & _
                           "'width=800, height=400, menubar=no, Resizable=NO,top=80,left=120, scrollbars=yes')" & _
                           "</script>"
            Else
                popupScript = "<script language='javascript'>" & _
                           "window.open('UpdateNotes.aspx?UniqueID=" & Datagridrow(4).Text & "', 'CustomPopUp', " & _
                           "'width=800, height=400, menubar=no, Resizable=NO,top=80,left=120, scrollbars=yes')" & _
                           "</script>"
            End If
            Page.RegisterStartupScript("PopupScript2", popupScript)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub DataGridNotes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridNotes.SelectedIndexChanged
        '******
        Dim l_datatable_Notes As DataTable
        l_datatable_Notes = Session("DisplayNotes")
        '******
        Try
            'Commented/added for YRPS-4614
            '*******
            'Session("Note") = Me.DataGridNotes.SelectedItem.Cells(3).Text.Trim

            Dim index As Integer
            index = Me.DataGridNotes.SelectedItem.ItemIndex
            If l_datatable_Notes.Rows(index)("Note").GetType.ToString <> "System.DBNull" Then
                Session("Note") = l_datatable_Notes.Rows(index)("Note")
            Else
                Session("Note") = ""
            End If
            '*******

            'by aparna get the checkbox value  YREN-3115
            Dim l_checkbox As New CheckBox
            l_checkbox = DataGridNotes.SelectedItem.FindControl("CheckBoxImportant")

            If l_checkbox.Checked Then
                Session("BitImportant") = True
            Else
                Session("BitImportant") = False
            End If
            Dim popupScript As String
            If Me.DataGridNotes.SelectedItem.Cells(4).Text = "&nbsp;" Then
                popupScript = "<script language='javascript'>" & _
                           "window.open('UpdateNotes.aspx?Index=" & Me.DataGridNotes.SelectedIndex & "', 'CustomPopUp', " & _
                           "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                           "</script>"
            Else
                popupScript = "<script language='javascript'>" & _
                           "window.open('UpdateNotes.aspx?UniqueID=" & Me.DataGridNotes.SelectedItem.Cells(4).Text & "', 'CustomPopUp', " & _
                           "'width=750, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                           "</script>"
            End If
            Page.RegisterStartupScript("PopupScript2", popupScript)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub ButtonAnnuitiesDateDeceasedEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAnnuitiesDateDeceasedEdit.Click
        Try
            Session("blnEditClick") = True
            Session("EnableSaveCancel") = True
            '        ButtonRetireesInfoSave.Enabled = True 
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Visible = True
            ' Me.ButtonRetireesInfoSave.Visible = False
            '' Me.ButtonSaveRetireeParticipants.Visible = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Visible = True
            Me.MakeLinkVisible()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonGeneralEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGeneralEdit.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralEdit", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Session("EnableSaveCancel") = True
            Me.TextBoxGeneralSuffix.Enabled = True
            Me.TextBoxGeneralMiddleName.Enabled = True
            Me.TextBoxGeneralLastName.Enabled = True
            Me.TextBoxGeneralFirstName.Enabled = True
            Me.cboSalute.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True

            ' ButtonRetireesInfoSave.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Visible = True
            ' Me.ButtonRetireesInfoSave.Visible = False
            '' Me.ButtonSaveRetireeParticipants.Visible = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Visible = True
            Me.MakeLinkVisible()
            ButtonGeneralEdit.Enabled = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonGeneralSSNoEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGeneralSSNoEdit.Click


        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralSSNoEdit", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            Session("EnableSaveCancel") = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            '        ButtonRetireesInfoSave.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'TextBoxGeneralSSNo.Enabled = True 'SP :2014.02.05 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -Start

            '        ButtonRetireesInfoSave.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Visible = True
            '       Me.ButtonRetireesInfoSave.Visible = False
            '' Me.ButtonSaveRetireeParticipants.Visible = True
            ButtonRetireesInfoCancel.Visible = True
            Me.MakeLinkVisible()
            'ButtonGeneralSSNoEdit.Enabled = False 'SP :2014.02.05 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -Start
            Dim popupScript As String = "<script language='javascript'>" & _
              "window.open('UpdateSSN.aspx?Name=retire&Mode=EditSSN', 'CustomPopUp', " & _
              "'width=600, height=300, menubar=no, Resizable=No,top=70,left=120, scrollbars=yes')" & _
             "</script>"
            Page.RegisterStartupScript("PopupScriptEditSSN", popupScript)
            'SP :2014.02.05 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN  -End
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | For handling Edit Annuity Beneficiary Button click 
    Private Sub ButtonAnnuitiesSSNoEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAnnuitiesSSNoEdit.Click
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAnnuitiesSSNoEdit", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If

            Me.ButtonSaveRetireeParticipants.Enabled = True
            Me.ButtonRetireesInfoCancel.Enabled = True
            Me.ButtonSaveRetireeParticipants.Visible = True
            Me.ButtonRetireesInfoCancel.Visible = True
            Me.MakeLinkVisible()
            Me.ButtonEditJSBeneficiary.Enabled = False 'Manthan Rajguru | 2016.08.02 |YRS-AT-2382 | Disabling Edit JS Beneficairy button on click of edit SSN button
            Dim popupScript As String = "<script language='javascript'>" & _
              "window.open('UpdateSSN.aspx?Name=retireAndAnnuityBeneficiary&Mode=EditSSN', 'CustomPopUp', " & _
              "'width=600, height=300, menubar=no, Resizable=No,top=70,left=120, scrollbars=yes')" & _
             "</script>"
            Page.RegisterStartupScript("PopupScriptEditSSN", popupScript)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | For handling Edit Annuity Beneficiary Button click 

    Private Sub ButtonGeneralDOBEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGeneralDOBEdit.Click
        Try
            'Added by neeraj on 01-Dec-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralSSNoEdit", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Me.ReqGeneralDOB.Enabled = True

            Session("EnableSaveCancel") = True
            'ButtonRetireesInfoSave.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            TextBoxGeneralDOB.Enabled = True
            Me.PopcalendarDate.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Visible = True
            'Me.ButtonRetireesInfoSave.Visible = False
            '' Me.ButtonSaveRetireeParticipants.Visible = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Visible = True

            PopcalendarDate.Enabled = True
            Me.MakeLinkVisible()
            ButtonGeneralDOBEdit.Enabled = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonRetireesInfoOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRetireesInfoOK.Click
        Try
            Session("Page") = "Person"
            Session("blnCancel") = Nothing
            Session("EnableSaveCancel") = Nothing
            Session("PersId") = Nothing
            Session("payment_Type") = Nothing
            Session("ValidationMessage") = Nothing
            Session("Flag") = Nothing
            Session("icounter") = Nothing
            Session("RetireesSort") = Nothing
            Session("EffDateUpdateBank") = Nothing
            Session("R_BankEffectiveDate") = Nothing
            Session("EffDateAddBank") = Nothing
            Session("DNMessageforDR") = Nothing

            'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
            'ClearSession()  'SB | 10/18/2017 | YRS-AT-3324 | Clearing Session variable used to check RMD generated or not
            'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
            'START : ML | 2019.09.20 | YRS-AT-4598 |Clear Session 
            SessionManager.SessionStateWithholding.LstSWHPerssDetail = Nothing
            Session("AnnuityAmount") = Nothing
            'END : ML | 2019.09.20 | YRS-AT-4598 |Clear Session 
            Response.Redirect("FindInfo.aspx?Name=Person", False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonRetireesInfoCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRetireesInfoCancel.Click

        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
        HiddenFieldDirty.Value = "false"
        ValidationSummaryRetirees.Enabled = False 'Added By Shashi Shekhar:2009-12-23

        Dim l_int_IsAddressPresent As Integer
        ButtonGeneralEdit.Enabled = True
        ButtonGeneralSSNoEdit.Enabled = True
        ButtonGeneralDOBEdit.Enabled = True
        btnAcctLockEditRet.Enabled = True

        Try
            Session("blnCancel") = True
            Session("Flag") = ""
            Session("dtReason") = Nothing
            ''Added by Anudeep:22.03.2013-For Resetting active as Primary 
            Session("bln_Activateprimary") = Nothing

            If Not TextBoxGeneralDOB.Text = String.Empty Then
                TextBoxGeneralDOB.Text = Session("DOB").ToString()
                LabelBoxAge.Text = CalculateAge(TextBoxGeneralDOB.Text, TextBoxGeneralDateDeceased.Text).ToString("00.00")
                If (Me.LabelBoxAge.Text.IndexOf(".00") > -1) Then
                    Me.LabelBoxAge.Text = Me.LabelBoxAge.Text.Replace(".00", "Y")
                Else
                    Me.LabelBoxAge.Text = Me.LabelBoxAge.Text.Replace(".", "Y/") + "M"
                End If
                Me.LabelBoxAge.Text = Me.LabelBoxAge.Text.Replace("/00M", "").Trim
            Else
                LabelBoxAge.Text = String.Empty
            End If

            'Me.AddressWebUserControl1.SetValidationsForSecondary()
            'If Session("blnAddBankingRetirees") = True Or Session("blnUpdateBankingRetirees") = True Then
            '    Me.MultiPageRetireesInformation.SelectedIndex = 4
            '    LoadBanksTab()

            'End If

            'Vipul 01Feb06 Cache-Session
            'If Not cache("BankingDtls") Is Nothing Then
            '    If Not CType(cache("BankingDtls"), DataSet).Tables(0).GetChanges Is Nothing Then
            '        If CType(cache("BankingDtls"), DataSet).Tables(0).GetChanges.Rows.Count > 0 Then
            '            Me.MultiPageRetireesInformation.SelectedIndex = 4
            '            LoadBanksTab()
            '        End If
            '    End If
            'End If
            If Session("blnCancel") = True Then
                LoadBanksTab()
            ElseIf Not Session("BankingDtls") Is Nothing Then
                If Not DirectCast(Session("BankingDtls"), DataSet).Tables(0).GetChanges Is Nothing Then
                    If DirectCast(Session("BankingDtls"), DataSet).Tables(0).GetChanges.Rows.Count > 0 Then
                        Me.MultiPageRetireesInformation.SelectedIndex = 4
                        LoadBanksTab()
                    End If
                End If

            End If
            'Vipul 01Feb06 Cache-Session

            'If Session("blnAddFedWithHoldings") = True Or Session("blnUpdateFedWithHoldings") = True Then
            '    Me.MultiPageRetireesInformation.SelectedIndex = 5
            '    LoadFedWithDrawalTab()
            'End If

            'Vipul 01Feb06 Cache-Session
            'If Not cache("FedWithDrawals") Is Nothing Then
            '    If Not CType(cache("FedWithDrawals"), DataSet).Tables(0).GetChanges Is Nothing Then
            '        If CType(cache("FedWithDrawals"), DataSet).Tables(0).GetChanges.Rows.Count > 0 Then
            '            Me.MultiPageRetireesInformation.SelectedIndex = 5
            '            LoadFedWithDrawalTab()
            '        End If
            '    End If
            'End If
            If Not Session("FedWithDrawals") Is Nothing Then
                If Not DirectCast(Session("FedWithDrawals"), DataSet).Tables(0).GetChanges Is Nothing Then
                    If DirectCast(Session("FedWithDrawals"), DataSet).Tables(0).GetChanges.Rows.Count > 0 Then
                        Me.MultiPageRetireesInformation.SelectedIndex = 5
                        LoadFedWithDrawalTab()
                    End If
                End If
            End If
            'Vipul 01Feb06 Cache-Session()

            'If Session("blnAddGenWithHoldings") = True Or Session("blnUpdateGenWithDrawals") = True Then
            '    Me.MultiPageRetireesInformation.SelectedIndex = 6
            '    LoadGenWithDrawalTab()
            'End If

            'Vipul 01Feb06 Cache-Session()
            'If Not cache("GenWithDrawals") Is Nothing Then
            '    If Not CType(cache("GenWithDrawals"), DataSet).Tables(0).GetChanges Is Nothing Then
            '        If CType(cache("GenWithDrawals"), DataSet).Tables(0).GetChanges.Rows.Count > 0 Then
            '            Me.MultiPageRetireesInformation.SelectedIndex = 6
            '            LoadGenWithDrawalTab()
            '        End If
            '    End If
            'End If
            If Not Session("GenWithDrawals") Is Nothing Then
                If Not DirectCast(Session("GenWithDrawals"), DataSet).Tables(0).GetChanges Is Nothing Then
                    If DirectCast(Session("GenWithDrawals"), DataSet).Tables(0).GetChanges.Rows.Count > 0 Then
                        Me.MultiPageRetireesInformation.SelectedIndex = 6
                        LoadGenWithDrawalTab()
                    End If
                End If
            End If
            'Vipul 01Feb06 Cache-Session()

            'If Session("blnAddBeneficiaries") = True Or Session("blnUpdateBeneficiaries") = True Then
            '    Me.MultiPageRetireesInformation.SelectedIndex = 2
            '    LoadBeneficiariesTab()
            'End If

            'Vipul 01Feb06 Cache-Session
            'If Not cache("BeneficiariesActive") Is Nothing Then
            '    If Not CType(cache("BeneficiariesActive"), DataSet).Tables(0).GetChanges Is Nothing Then
            '        If CType(cache("BeneficiariesActive"), DataSet).Tables(0).GetChanges.Rows.Count > 0 Then
            '            Me.MultiPageRetireesInformation.SelectedIndex = 6
            '            LoadBeneficiariesTab()
            '        End If
            '    End If
            'End If
            If Not Session("BeneficiariesRetired") Is Nothing Then
                If Not DirectCast(Session("BeneficiariesRetired"), DataSet).Tables(0).GetChanges Is Nothing Then
                    If DirectCast(Session("BeneficiariesRetired"), DataSet).Tables(0).GetChanges.Rows.Count > 0 Then
                        'NP:PS:2007.06.28 - On Cancel the user is still shown the modified dataset. We need to either reload the data from the Database or reject changes made to the dataset. Prefering the reject option as it saves a round trip.
                        DirectCast(Session("BeneficiariesRetired"), DataSet).Tables(0).RejectChanges()
                        DataGridRetiredBeneficiaries.SelectedIndex = -1 'NP:PS:2007.08.07 - Deselecting the beneficiary when Cancel button is hit.
                        LoadBeneficiariesTab()
                    End If
                End If
            End If
            'Vipul 01Feb06 Cache-Session 

            'If Session("blnAddNotes") = True Or Session("blnUpdateNotes") = True Then
            '    Me.MultiPageRetireesInformation.SelectedIndex = 7
            '    LoadNotesTab()
            'End If

            'Vipul 01Feb06 Cache-Session 
            'If Not cache("dtNotes") Is Nothing Then
            '    If Not CType(cache("dtNotes"), DataTable).GetChanges Is Nothing Then
            '        If CType(cache("dtNotes"), DataTable).GetChanges.Rows.Count > 0 Then
            '            Me.MultiPageRetireesInformation.SelectedIndex = 7
            '            LoadNotesTab()
            '        End If
            '    End If
            'End If
            'Priya 13-April-2010 :As per Hafiz Mail Send on:12April-2010 :Issues identified with 7.4.2 code release
            Session("blnCancel") = True
            If Not Session("dtNotes") Is Nothing Then
                If Not DirectCast(Session("dtNotes"), DataTable).GetChanges Is Nothing Then
                    If DirectCast(Session("dtNotes"), DataTable).GetChanges.Rows.Count > 0 Then
                        Me.MultiPageRetireesInformation.SelectedIndex = 7
                        LoadNotesTab()
                    End If
                End If
            End If
            'Vipul 01Feb06 Cache-Session 

            'Me.MultiPageRetireesInformation.SelectedIndex = 7
            LoadGeneraltab()


            Session("blnCancel") = False

            Me.TextBoxGeneralSuffix.Enabled = False
            Me.TextBoxGeneralMiddleName.Enabled = False
            Me.TextBoxGeneralLastName.Enabled = False
            Me.TextBoxGeneralFirstName.Enabled = False
            Me.cboSalute.Enabled = False
            TextBoxGeneralDOB.Enabled = False
            Me.PopcalendarDate.Enabled = False
            TextBoxGeneralSSNo.Enabled = False

            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'Me.TextBoxAddress1.ReadOnly = True
            'Me.TextBoxAddress2.ReadOnly = True
            'Me.TextBoxAddress3.ReadOnly = True
            'Me.TextBoxCity.ReadOnly = True
            'Me.DropdownlistState.Enabled = False
            'Me.TextBoxZip.ReadOnly = True
            'Me.DropdownlistCountry.Enabled = False
            'Me.AddressWebUserControl1.guiPerssId = Session("PersId")
            'Me.AddressWebUserControl1.IsPrimary = 1
            'Me.AddressWebUserControl1.ShowDataForParticipant = 1
            'Me.AddressWebUserControl1.ShowDataForParticipant()
            'By Ashutosh Patil as on 25-Apr-2007
            'YREN-3298
            If Not Session("AddressPresent") Is Nothing Then
                l_int_IsAddressPresent = Session("AddressPresent")
            Else
                l_int_IsAddressPresent = 0
            End If
            If l_int_IsAddressPresent = 0 Then
                'Me.AddressWebUserControl1.ClearControls = ""
            End If
            'Me.AddressWebUserControl2.guiPerssId = Session("PersId")
            'Me.AddressWebUserControl2.IsPrimary = 0
            'Me.AddressWebUserControl2.ShowDataForParticipant = 0
            'Me.AddressWebUserControl2.ShowDataForParticipant()
            Me.AddressWebUserControl1.EnableControls = True
            'By Ashutosh Patil as on 25-Apr-2007
            'YREN-3298
            If Not Session("AddressPresent") Is Nothing Then
                l_int_IsAddressPresent = Session("AddressPresent")
            Else
                l_int_IsAddressPresent = 0
            End If
            If l_int_IsAddressPresent = 0 Then
                'Me.AddressWebUserControl2.ClearControls = ""
            End If
            'Added by Anudeep:14.03.2013 for telephone usercontrol
            'Me.TelephoneWebUserControl1.EditPrimaryContact_Enabled = False
            'Me.TelephoneWebUserControl2.EditSecondaryContact_Enabled = False
            tbPricontacts.Visible = False
            tbSeccontacts.Visible = False
            'Commented by Anudeep:14.03.2013 for telephone 
            'Me.TextboxTelephone.ReadOnly = True
            'Me.TextBoxFax.ReadOnly = True
            'Me.TextBoxMobile.ReadOnly = True
            'Me.TextBoxHome.ReadOnly = True
            'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from Retiree and put on address control
            'Me.CheckboxIsBadAddress.Enabled = False
            'Me.CheckboxSecIsBadAddress.Enabled = False
            'Commented by Anudeep:14.03.2013 for telephone 
            'Me.TextBoxSecTelephone.ReadOnly = True
            'Me.TextBoxSecFax.ReadOnly = True
            'Me.TextBoxSecMobile.ReadOnly = True
            'Me.TextBoxSecHome.ReadOnly = True

            'Me.TextBoxEmailId.ReadOnly = True
            'Me.TextBoxEmailId.Enabled = True
            'Me.LabelEmailId.Enabled = True
            Me.ButtonEditAddress.Enabled = True

            ' Me.CheckBoxActive.Enabled = False
            Me.CheckboxBadEmail.Enabled = False
            '  Me.CheckboxSecBadEmail.Enabled = False
            'Me.CheckboxSecTextOnly.Enabled = False
            'Me.CheckboxSecUnsubscribe.Enabled = False
            Me.CheckboxTextOnly.Enabled = False
            Me.CheckboxUnsubscribe.Enabled = False

            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'Me.TextBoxSecAddress1.Enabled = False
            'Me.TextBoxSecAddress2.Enabled = False
            'Me.TextBoxSecAddress3.Enabled = False
            'Me.TextBoxSecCity.Enabled = False
            'Me.DropdownlistSecCountry.Enabled = False
            ''Me.TextBoxSecEmail.Enabled = False
            'Me.DropdownlistSecState.Enabled = False
            'Me.TextBoxSecZip.Enabled = False
            'Commented by Anudeep:14.03.2013 for telephone 
            'Me.TextBoxSecTelephone.Enabled = False
            'BS:2012.03.09:-restructure Address Control
            'Me.TextBoxSecEffDate.Enabled = False
            'Me.TextBoxPriEffDate.Enabled = False

            Me.AddressWebUserControl2.EnableControls = True
            'Me.ButtonRetireesInfoSave.Visible = True
            'Me.ButtonRetireesInfoSave.Enabled = False
            '' Me.ButtonSaveRetireeParticipants.Visible = False
            Me.ButtonSaveRetireeParticipants.Enabled = False
            Me.ButtonRetireesInfoCancel.Enabled = False
            Me.ButtonUpdateJSBeneficiary.Enabled = False
            Me.ButtonCancelJSBeneficiary.Enabled = False
            Me.ButtonRetireesInfoOK.Enabled = True
            'Start:AA:24.09.2013 : BT-1501: Added for disabling controls while pressing joint surviour cacle button
            Me.TextBoxAnnuitiesSSNo.Enabled = False
            Me.TextBoxAnnuitiesFirstName.Enabled = False
            Me.TextBoxAnnuitiesMiddleName.Enabled = False
            Me.TextBoxAnnuitiesLastName.Enabled = False
            Me.TextBoxAnnuitiesDOB.Enabled = False
            Me.TextBoxDateDeceased.Enabled = False
            Me.CheckboxSpouse.Enabled = False
            'End:AA:24.09.2013 : BT-1501: Added for disabling controls while pressing joint surviour cacle button

            'START: PPP | 08/01/2016 | YRS-AT-2382 | Enabling Annuities edit ssn and view ssn updates links on load
            Me.ButtonAnnuitiesSSNoEdit.Enabled = True
            Me.lnkbtnViewAnnuitiesSSNUpdate.Enabled = True
            'END: PPP | 08/01/2016 | YRS-AT-2382 | Enabling Annuities edit ssn and view ssn updates links on load

            Me.LoadAnnuitiesTab() 'by aparna

            Me.MakeLinkVisible()
            'By Ashutosh Patil as on 25-Apr-2007
            'YREN-3298
            'ButtonRetireesInfoCancel.Visible = False

            ' Session("EnableSaveCancel") = False

            'SP 2014.12.04 BT-2310\YRS 5.0-2255
            IsModifiedPercentage = False
            OriginalBeneficiaries = Nothing
            Me.IsExhaustedDBRMDSettleOptionSelected = False 'MMR | 2016.10.14 | YRS-AT-3063 | Resetting property value
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    ''Private Sub PopcalendarDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopcalendarDate.SelectionChanged
    ''    Try
    ''        System.DateTime.Today.AddYears(-21)
    ''        If Date.Compare(PopcalendarDate.SelectedDate, System.DateTime.Today.AddYears(-21)) = 1 Then
    ''            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Active participant must be atleast 21 years of age.", MessageBoxButtons.Stop)
    ''        Else
    ''            TextBoxGeneralDOB.Text = PopcalendarDate.SelectedDate
    ''        End If
    ''    Catch ex As Exception
    ''        Dim l_String_Exception_Message As String
    ''        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    ''        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    ''    End Try
    ''End Sub
    Private Sub ButtonRetireesInfoPHR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRetireesInfoPHR.Click
        Dim popupScript As String

        popupScript = "<script language='javascript'>" & _
                   "window.open('RetireesFrmMoverWebForm.aspx', 'CustomPopUp', " & _
                   "'width=800, height=500, menubar=no, Resizable=Yes,top=80,left=120, scrollbars=yes')" & _
                   "</script>"

        Page.RegisterStartupScript("PopupScript2", popupScript)
    End Sub

    Private Sub ButtonBankingInfoCheckPayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonBankingInfoCheckPayment.Click
        Dim drExisting As DataRow
        Dim drNew As DataRow
        Dim l_dataset_Banks As New DataSet
        Dim tmpCnt As Integer
        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
        HiddenFieldDirty.Value = "true"
        Try
            'cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session
            'l_dataset_Banks = cache("BankingDtls")
            '*********************Code Added by Ashutosh on 05-May-06****************
            'l_dataset_Banks = cache("BankingDtls")
            '*******************************************************
            l_dataset_Banks = Session("BankingDtls")
            'Vipul 01Feb06 Cache-Session

            Session("blnShowBankingPage") = True
            'Code added by Shubhrata YREN 2638 Aug 17th 2006
            Session("EffDateAddBank") = Nothing
            Session("EffDateUpdateBank") = Nothing
            Dim l_dataset_effdate As New DataSet
            Dim l_check_effdate As DateTime
            Dim l_fmt_date As String
            Dim l_datatable_effdate As New DataTable
            Dim drow As DataRow()
            l_check_effdate = DateTime.Now.Date.ToShortDateString()
            l_fmt_date = String.Format("{0:MM/dd/yyyy}", l_check_effdate)
            If Not Session("BankingDtls") Is Nothing Then
                l_dataset_effdate = DirectCast(Session("BankingDtls"), DataSet)
                If l_dataset_effdate.Tables(0).Rows.Count > 0 Then
                    l_datatable_effdate = l_dataset_effdate.Tables(0)
                    drow = l_datatable_effdate.Select("EffecDate= '" & l_fmt_date & "'")
                    If Not drow Is Nothing Then
                        If drow.Length > 0 Then
                            MessageBox.Show(Me.PlaceHolder1, "YMCA YRS", "Effective Date already exists", MessageBoxButtons.Stop)
                            Exit Sub
                        End If
                    End If
                End If
            End If
            'Code added by Shubhrata YREN 2638 Aug 17th 2006
            'If Not l_dataset_Banks Is Nothing And DataGridBankInfoList.SelectedIndex <> -1 Then '' Comented by sanjay Rawat 
            If Not l_dataset_Banks Is Nothing Then
                If l_dataset_Banks.Tables(0).Rows.Count > 0 Then
                    ''' commented by sanjay Rawat for Gemini 683
                    'drExisting = l_dataset_Banks.Tables(0).Rows(DataGridBankInfoList.SelectedIndex)
                    'Dim i As Integer
                    'For i = 0 To Me.DataGridBankInfoList.Items.Count - 1
                    '    'commented by hafiz on 9Aug2006 - YREN-2591
                    '    'If Me.DataGridBankInfoList.Items(i).Cells(7).Text.ToString() = drExisting(0).ToString() And Me.DataGridBankInfoList.Items(i).Cells(2).Text.ToString() = "N/A" Then
                    '    If Me.DataGridBankInfoList.Items(i).Cells(7).Text.ToString() = drExisting(8).ToString() And Me.DataGridBankInfoList.Items(i).Cells(2).Text.ToString() = "N/A" Then
                    '        Exit Sub
                    '    End If
                    'Next
                    ''Ends here 
                    drNew = l_dataset_Banks.Tables(0).NewRow
                    'For tmpCnt = 0 To drExisting.ItemArray.Length - 1
                    '    drNew(tmpCnt) = drExisting(tmpCnt)
                    'Next
                    'drNew("PersonID") = Session("PersId")
                    drNew(0) = System.DBNull.Value
                    drNew("PersonID") = Session("PersId")

                    'drNew("EFTDesc") = System.DBNull.Value
                    drNew("PaymentDesc") = "CHECK"
                    drNew("BankName") = "Check"

                    drNew("AccountNo") = "N/A"
                    drNew("BankABA#") = "N/A"
                    'drNew("AccountType") = "Checking"  'Added by Dilip Yadav 01-Oct-09 :  YRS 5.0.896
                    'Code modified by Shubhrata YREN 2638 Aug 17th 2006
                    'drNew("EffecDate") = DateTime.Now.Date.ToShortDateString()
                    drNew("EffecDate") = l_fmt_date
                    'Code added by Shubhrata YREN 2638 Aug 17th 2006
                    'Code Added By Ashutosh Patil on 16-01-2007 for YERN - 2979
                    drNew("dtmEffDate") = Today.Now
                    'Added by dilip yadav : 01.oct-09
                    If (l_dataset_Banks.Tables("Banks").Rows.Count > 0) Then
                        'Session("FundType") = l_dataset_Banks.Tables("Banks").Rows(0)(11).ToString().Trim
                        drNew("ParticipantType") = l_dataset_Banks.Tables("Banks").Rows(0)("ParticipantType").ToString().Trim
                    End If

                    drNew("chrEFTStatus") = "O"

                    'Commented by Dilip yadav : 08-Oct-09 : To resolve the issue YRS 5.0-911
                    'drNew("EFTDesc") = 22

                    l_dataset_Banks.Tables(0).Rows.Add(drNew)

                    ''Added by sanjay for Sorting
                    Dim BankDataView As DataView = l_dataset_Banks.Tables("Banks").DefaultView
                    BankDataView.Sort = "dtmEffDate DESC "
                    '''ends here 


                    DataGridBankInfoList.DataSource = BankDataView
                    DataGridBankInfoList.DataBind()

                    'Added By Ashutosh Patil as on 15-01-2007
                    ''l_dataset_Banks.Tables(0).Select("EffecDate=MAX(EffecDate)")
                    ''If l_dataset_Banks.Tables(0).Rows(0).Item(9) = "V" Then
                    ''    Me.LabelCurrentEFT.Text = "Current Pay Method: CHECK"
                    ''End If

                    'End If
                    '
                    'l_dataset_Banks.AcceptChanges()
                    'Vipul 01Feb06 Cache-Session
                    'cache.Add("BankingDtls", l_dataset_Banks)
                    Session("BankingDtls") = l_dataset_Banks
                    'Vipul 01Feb06 Cache-Session

                    'ButtonRetireesInfoSave.Enabled = True
                    'ButtonRetireesInfoSave.Visible = True
                    '' Me.ButtonSaveRetireeParticipants.Visible = True
                    Me.ButtonSaveRetireeParticipants.Enabled = True
                    ButtonRetireesInfoCancel.Enabled = True
                    ButtonRetireesInfoCancel.Visible = True
                    Session("payment_Type") = "Check"
                    Me.MakeLinkVisible()
                    'Code Added By Ashutosh Patil as on 15 Jan,2007 for YERN-2979
                    'Start Ashutosh Patil
                    Me.LabelCurrentEFT.Text = "Current Pay Method : Check"
                    'End Ashutosh Patil

                    ButtonBankingInfoCheckPayment.Enabled = False


                End If
            End If
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Sub DataGridRetiredBeneficiaries_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridRetiredBeneficiaries.ItemCommand
        Dim popupScript As String = String.Empty
        Dim strWSMessage As String
        Try
            If e.CommandName.ToLower = "edit" Then
                'Start:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditRetired", Convert.ToInt32(Session("LoggedUserKey")))

                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'START : Dharmesh : 11/20/2018 : YRS-AT-4136 : Added new view state variable to access it from button click of Add, so when click of add it will make the view state variable to True and display error message
                Dim bParticipantIsINSRES As Boolean = False
                Dim bParticipantIsRETIRE As Boolean = False
                Dim dictStringParam As Dictionary(Of String, String)
                Dim dtConfigurationKeyValueForEnrolledAfter2019 As DateTime
                divErrorMsg.Visible = False
                If (HelperFunctions.isNonEmpty(Session("AnnuityDetails"))) Then
                    Dim dsBeneficiaries As DataSet = DirectCast(Session("BeneficiariesRetired"), DataSet)
                    Dim drRows() As DataRow = dsBeneficiaries.Tables(0).Select("UniqueID='" & e.Item.Cells(2).Text & "'")
                    If drRows.Length > 0 Then
                        If drRows(0).Item(14) = "INSRES" Then
                            bParticipantIsINSRES = True
                        End If
                        If drRows(0).Item(14) = "RETIRE" Then
                            bParticipantIsRETIRE = True
                        End If
                    End If
                    'Check participant is enrolled on or after 1/1/2019 and not have c annuity and display error message "1007" with display msg : "This person has beneficiary type ‘INSRES’ but does not have Insured Reserve Annuity. Please delete the beneficiary (ies)."
                    If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) _
                        AndAlso Not (YMCARET.YmcaBusinessObject.RetirementBOClass.HasInsuredReserveAnnuity(CType(Session("AnnuityDetails"), DataSet))) _
                        AndAlso bParticipantIsINSRES = True Then
                        ShowErrorMessage(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019_IN_EDIT_BENEFICIARY_FOR_INSURED_RESERVES).DisplayText())
                        Exit Sub
                    End If
                    'Check participant is enrolled on or after 1/1/2019 and not have c annuity and display error message "1008" with display msg : "This person has beneficiary type ''RETIRE'' which is not allowed as this Participant was first enrolled on or after $$CutOffDate$$.  Please delete them."
                    If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) _
                        AndAlso Not (YMCARET.YmcaBusinessObject.RetirementBOClass.HasInsuredReserveAnnuity(CType(Session("AnnuityDetails"), DataSet))) _
                        AndAlso bParticipantIsRETIRE = True Then
                        ShowErrorMessage(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019_IN_EDIT_BENEFICIARY_FOR_RETIRE, YMCARET.YmcaBusinessObject.RetirementBOClass.GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText())
                        Exit Sub
                    End If
                End If
                'END : Dharmesh : 11/20/2018 : YRS-AT-4136 : Added new view state variable to access it from button click of Add, so when click of add it will make the view state variable to True and display error message

                'End:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
                EditRetired(e.Item.Cells, e.Item.ItemIndex)
                'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
                'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
                strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
                If strWSMessage <> "NoPending" Then

                    strWSMessage = (strWSMessage.Replace("<br/>", "\n")).Replace("<br>", "\n")
                    Me.TabStripRetireesInformation.Items(2).Text = "<asp:Label ID='lblBeneficiaries' CssClass='Label_Small'onmouseover='javascript: showToolTip(""" + strWSMessage + """,""Bene"");' onmouseout='javascript: hideToolTip();'><font color=red>Beneficiary</font></asp:Label>"

                    cboGeneralMaritalStatus.Enabled = False

                    imgLock.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Pers');")
                    imgLock.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                    imgLock.Visible = True

                    imgLockBeneficiary.Visible = True
                    imgLockBeneficiary.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Bene');")
                    imgLockBeneficiary.Attributes.Add("onmouseout", "javascript: hideToolTip();")

                Else
                    imgLock.Visible = False
                    imgLockBeneficiary.Visible = False
                    cboGeneralMaritalStatus.Enabled = True
                    Me.TabStripRetireesInformation.Items(2).Text = "Beneficiaries"

                End If
                'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            End If


            If e.CommandName.ToLower = "delete" Then
                'Start:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonDeleteRetired", Convert.ToInt32(Session("LoggedUserKey")))

                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'End:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
                'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
                'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
                strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
                If strWSMessage <> "NoPending" Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Invalid Beneficiary Operation", "openDialog('" + strWSMessage + "','Bene');", True)
                    strWSMessage = (strWSMessage.Replace("<br/>", "\n")).Replace("<br>", "\n")
                    Me.TabStripRetireesInformation.Items(2).Text = "<asp:Label ID='lblBeneficiaries' CssClass='Label_Small'onmouseover='javascript: showToolTip(""" + strWSMessage + """,""Bene"");' onmouseout='javascript: hideToolTip();'><font color=red>Beneficiary</font></asp:Label>"

                    cboGeneralMaritalStatus.Enabled = False

                    imgLock.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Pers');")
                    imgLock.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                    imgLock.Visible = True

                    imgLockBeneficiary.Visible = True
                    imgLockBeneficiary.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Bene');")
                    imgLockBeneficiary.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                    Exit Sub
                Else
                    imgLock.Visible = False
                    imgLockBeneficiary.Visible = False
                    cboGeneralMaritalStatus.Enabled = True
                    Me.TabStripRetireesInformation.Items(2).Text = "Beneficiaries"

                End If
                'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application

                'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - Start
                'Check beneficiary settle status 
                If e.Item.Cells(14).Text <> "&nbsp;" Then
                    If (e.Item.Cells(14).Text.Trim.Equals("SETTLD", StringComparison.CurrentCultureIgnoreCase)) Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DELETE_SETTLED_BENEFICIARY, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
                'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - End

                'Start -> DineshK:2013.02.12:BT-1261/YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion 
                If e.Item.Cells(2).Text <> "&nbsp;" Then
                    Session("Flag") = "DeleteBeneficiaries"
                    popupScript = "<script language='javascript'>" & _
                                    "window.open('DeleteBeneficiary.aspx?UniqueId=" + e.Item.Cells(2).Text + "','CustomPopUp', " & _
                                    "'width=800, height=400, menubar=no, Resizable=no,top=120,left=120, scrollbars=yes')" & _
                                    "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", popupScript)

                    End If

                    Session("BenTableCellCollection") = e.Item.Cells
                    Session("BenItemIndex") = e.Item.ItemIndex
                Else

                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please first save the added beneficiary.", MessageBoxButtons.Stop)
                End If
                'END -> DineshK:2013.02.12:BT-1261/YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion 
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridRetiredBeneficiaries_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridRetiredBeneficiaries.ItemDataBound
        ' // START : SB | 07/07/2016 | YRS-AT-2382 | For Non human and New Beneficiaries make "view ssn updates " label false
        Dim viewLink As System.Web.UI.WebControls.HyperLink
        Try
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                If Not IsRelationshipTypeIsHumanBenaficiary(RelationShipCheck, e.Item.Cells(RETIREBENEF_REL).Text) Then
                    viewLink = DirectCast(e.Item.Cells(RETIREBENEF_TaxID).Controls(3), System.Web.UI.WebControls.HyperLink)
                    viewLink.Visible = False
                ElseIf (e.Item.Cells(RETIREBENEF_IsExistingAudit).Text = "0") Then
                    viewLink = DirectCast(e.Item.Cells(RETIREBENEF_TaxID).Controls(3), System.Web.UI.WebControls.HyperLink)
                    viewLink.Visible = False
                End If

                If String.IsNullOrEmpty(e.Item.Cells(RETIREBENEF_UniqueID).Text.Replace("&nbsp;", "").Trim) And Not String.IsNullOrEmpty(e.Item.Cells(RETIREBENEF_New).Text.Replace("&nbsp;", "").Trim) Then
                    viewLink = DirectCast(e.Item.Cells(RETIREBENEF_TaxID).Controls(3), System.Web.UI.WebControls.HyperLink)
                    viewLink.Visible = False
            End If


            End If            
            'If e.Item.ItemType = ListItemType.Header Then
            '    e.Item.Cells(16).Text = "Type"
            'End If
            'e.Item.Cells(2).Visible = False
            'e.Item.Cells(3).Visible = False
            'e.Item.Cells(4).Visible = False
            'e.Item.Cells(5).Visible = False
            'e.Item.Cells(14).Visible = False
            'e.Item.Cells(13).Visible = False
            'If e.Item.ItemType <> ListItemType.Header Then
            '    e.Item.Cells(15).HorizontalAlign = HorizontalAlign.Right
            '    e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Center
            'End If
            ''Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            'e.Item.Cells(19).Visible = False

            ''SP 2014.11.27 BT-2310\YRS 5.0-2255 - Start
            'e.Item.Cells(20).Visible = False
            'e.Item.Cells(21).Visible = False
            'e.Item.Cells(22).Visible = False
            'e.Item.Cells(23).Visible = False
            ''SP 2014.11.27 BT-2310\YRS 5.0-2255 - End
            ' // END : SB | 07/07/2016 | YRS-AT-2382 | For Non human and New Beneficiaries make "view ssn updates " label false
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonEditAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditAddress.Click
        Try

            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditAddress", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'Me.TextBoxAddress1.ReadOnly = False
            'Me.TextBoxAddress2.ReadOnly = False
            'Me.TextBoxAddress3.ReadOnly = False
            'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from Retiree and put on address control
            'Me.CheckboxIsBadAddress.Enabled = True
            'Me.CheckboxSecIsBadAddress.Enabled = True
            'Me.TextBoxCity.ReadOnly = False
            'Me.DropdownlistState.Enabled = True
            'Me.TextBoxZip.ReadOnly = False
            Me.AddressWebUserControl1.EnableControls = True
            'Added by Anudeep:14.03.2013 for telephone usercontrol
            ' Me.TelephoneWebUserControl1.EditPrimaryContact_Enabled = True

            'ButtonEditAddress.Enabled = False
            'ButtonActivateAsPrimary.Enabled = False
            'Me.DropdownlistCountry.Enabled = True
            'Commented by Anudeep:14.03.2013 for telephone 
            Me.TextboxTelephone.ReadOnly = False
            Me.TextBoxHome.ReadOnly = False
            Me.TextBoxFax.ReadOnly = False
            Me.TextBoxMobile.ReadOnly = False
            Me.TextBoxSecTelephone.ReadOnly = False
            Me.TextBoxSecHome.ReadOnly = False
            Me.TextBoxSecFax.ReadOnly = False
            Me.TextBoxSecMobile.ReadOnly = False

            Me.TextBoxEmailId.ReadOnly = False
            Me.TextBoxEmailId.Enabled = True
            Me.LabelEmailId.Enabled = True
            'Me.ButtonEditAddress.Enabled = False
            'By Ashutosh Patil as on 25-Apr-2007
            'YREN-3298
            'Me.AddressWebUserControl1.SetValidationsForPrimary()
            'Dim l_ds_SecondaryAddress As DataSet
            'If Not Session("Ds_SecondaryAddress") Is Nothing Then
            '    l_ds_SecondaryAddress = CType(Session("Ds_SecondaryAddress"), DataSet)
            '    'Secondary Address section starts
            '    If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
            ' Me.CheckBoxActive.Enabled = True
            '    End If
            'End If
            Me.CheckboxBadEmail.Enabled = True
            '   Me.CheckboxSecBadEmail.Enabled = True
            '  Me.CheckboxSecTextOnly.Enabled = True
            ' Me.CheckboxSecUnsubscribe.Enabled = True
            Me.CheckboxTextOnly.Enabled = True
            Me.CheckboxUnsubscribe.Enabled = True

            'Me.TextBoxSecAddress1.Enabled = True
            'Me.TextBoxSecAddress2.Enabled = True
            'Me.TextBoxSecAddress3.Enabled = True
            'Me.TextBoxSecCity.Enabled = True
            'Me.DropdownlistSecCountry.Enabled = True
            'Me.TextBoxSecZip.Enabled = True
            Me.AddressWebUserControl2.EnableControls = True
            'Added by Anudeep:14.03.2013 for telephone usercontrol
            'Me.TelephoneWebUserControl2.EditSecondaryContact_Enabled = True

            'Me.TextBoxSecEmail.Enabled = True
            'Me.DropdownlistSecState.Enabled = True
            'Commented by Anudeep:14.03.2013 for telephone 
            Me.TextboxTelephone.ReadOnly = True
            Me.TextBoxHome.ReadOnly = True
            Me.TextBoxFax.ReadOnly = True
            Me.TextBoxMobile.ReadOnly = True
            Me.TextBoxSecTelephone.Enabled = True
            Me.TextBoxSecHome.Enabled = True
            Me.TextBoxSecFax.Enabled = True
            Me.TextBoxSecMobile.Enabled = True


            'Me.TextBoxSecEffDate.Enabled = True
            'Me.TextBoxPriEffDate.Enabled = True

            Me.ButtonRetireesInfoCancel.Enabled = True
            'Me.ButtonRetireesInfoSave.Enabled = True
            'Me.ButtonRetireesInfoSave.Visible = False
            '' Me.ButtonSaveRetireeParticipants.Visible = True
            Session("EnableSaveCancel") = True 'by aparna 11/10/2007
            Me.ButtonSaveRetireeParticipants.Enabled = True
            Me.MakeLinkVisible()
            'By Ashutosh Patil as on 25-Apr-2007
            'YREN-3298
            'Priya : 14/12/2010:Made changes in adddress user control
            'Call Me.AddressWebUserControl1.SetCountStZipCodeMandatoryOnSelection()
            'Priya 06-Sep-2010: YRS 5.0-1126:Address update for non-US/non-Canadian address.
            ValidationSummaryRetirees.Enabled = False
            'End 06-Sep-2010: YRS 5.0-1126
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub cboGeneralMaritalStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGeneralMaritalStatus.SelectedIndexChanged
        Try
            '            ButtonRetireesInfoSave.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Visible = True
            'Me.ButtonRetireesInfoSave.Visible = False
            '' Me.ButtonSaveRetireeParticipants.Visible = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Visible = True
            Me.MakeLinkVisible()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DropDownListGeneralGender_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListGeneralGender.SelectedIndexChanged
        Try
            'ButtonRetireesInfoSave.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Visible = True
            'Me.ButtonRetireesInfoSave.Visible = False
            '' Me.ButtonSaveRetireeParticipants.Visible = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Visible = True
            Me.MakeLinkVisible()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    'Priya 05-April-2010 : YRS 5.0-1042:New "flag" value in Person/Retiree maintenance screen
    'Commented OldGuardNews check box code to remove it from screen
    'Private Sub chkOldGuardNews_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOldGuardNews.CheckedChanged
    '    Try
    '        'ButtonRetireesInfoSave.Enabled = True
    '        ButtonRetireesInfoCancel.Enabled = True
    '        'ButtonRetireesInfoSave.Visible = True
    '        'Me.ButtonRetireesInfoSave.Visible = False
    '        '' Me.ButtonSaveRetireeParticipants.Visible = True
    '        Me.ButtonSaveRetireeParticipants.Enabled = True
    '        ButtonRetireesInfoCancel.Visible = True
    '        Me.MakeLinkVisible()
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'End YRS 5.0-1042

    Private Sub DataGridAnnuitiesPaid_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridAnnuitiesPaid.SortCommand
        Try
            If Not ViewState("l_dataset_AnnuityPaid") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                Dim l_dataset_AnnuityPaid As DataSet
                l_dataset_AnnuityPaid = ViewState("l_dataset_AnnuityPaid")
                dv = l_dataset_AnnuityPaid.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("RetireesSort") Is Nothing Then
                    If Session("RetireesSort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridAnnuitiesPaid.DataSource = Nothing
                Me.DataGridAnnuitiesPaid.DataSource = dv
                Me.DataGridAnnuitiesPaid.DataBind()
                Session("RetireesSort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    'Private Sub DropdownlistCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistCountry.SelectedIndexChanged
    '    'Code  By Ashutosh on 25-June-06
    '    ''Try
    '    ''    Dim l_DataSet_StateNames As DataSet
    '    ''    If Me.DropdownlistCountry.SelectedItem.Text = "" Then
    '    ''        'DropdownlistState.Items.Clear()
    '    ''        'DropdownlistState.Items.Insert(0, "-Select State-")
    '    ''        DropdownlistState.Items(0).Value = ""
    '    ''    Else
    '    ''        l_DataSet_StateNames = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUp_AMP_StateNames(DropdownlistCountry.SelectedValue.ToString().Trim())
    '    ''        DropdownlistState.Items.Clear()
    '    ''        DropdownlistState.DataSource = l_DataSet_StateNames.Tables(0)
    '    ''        DropdownlistState.DataTextField = "Description"
    '    ''        DropdownlistState.DataValueField = "chvcodevalue"
    '    ''        DropdownlistState.DataBind()
    '    ''        DropdownlistState.Items.Insert(0, "-Select State-")
    '    ''        DropdownlistState.Items(0).Value = ""
    '    ''    End If
    '    ''    ButtonRetireesInfoSave.Enabled = True
    '    ''    ButtonRetireesInfoCancel.Enabled = True
    '    ''    ButtonRetireesInfoSave.Visible = True
    '    ''    ButtonRetireesInfoCancel.Visible = True
    '    ''    ''DropdownlistSecState.Items.Insert(0, "-Select State-")
    '    ''Catch ex As Exception
    '    ''    Dim l_String_Exception_Message As String
    '    ''    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '    ''    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    ''End Try
    '    '***************
    'End Sub

    'Private Sub DropdownlistSecCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistSecCountry.SelectedIndexChanged


    '    '***************Code Commented by ashutosh on 25-June-06*********

    '    'Try
    '    '    Dim l_DataSet_StateNames As DataSet
    '    '    If Me.DropdownlistSecCountry.SelectedItem.Text = "" Then
    '    '        'DropdownlistSecState.Items.Clear()
    '    '        'DropdownlistSecState.DataSource = Nothing
    '    '        'DropdownlistSecState.Items.Insert(0, "-Select State-")
    '    '        DropdownlistSecState.Items(0).Value = ""
    '    '    Else
    '    '        l_DataSet_StateNames = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUp_AMP_StateNames(DropdownlistSecCountry.SelectedValue.ToString().Trim())
    '    '        DropdownlistSecState.Items.Clear()
    '    '        DropdownlistSecState.DataSource = l_DataSet_StateNames.Tables(0)
    '    '        DropdownlistSecState.DataTextField = "Description"
    '    '        DropdownlistSecState.DataValueField = "chvcodevalue"
    '    '        DropdownlistSecState.DataBind()
    '    '        DropdownlistSecState.Items.Insert(0, "-Select State-")
    '    '        DropdownlistSecState.Items(0).Value = ""
    '    '    End If
    '    '    ButtonRetireesInfoSave.Enabled = True
    '    '    ButtonRetireesInfoCancel.Enabled = True
    '    '    ButtonRetireesInfoSave.Visible = True
    '    '    ButtonRetireesInfoCancel.Visible = True
    '    '    ''DropdownlistSecState.Items.Insert(0, "-Select State-")
    '    'Catch ex As Exception
    '    '    Dim l_String_Exception_Message As String
    '    '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '    '    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    'End Try
    '    '*********End Ashu Code***********
    'End Sub

    'Private Sub DropdownlistState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistState.SelectedIndexChanged
    '	Try
    '		Me.ButtonRetireesInfoCancel.Enabled = True
    '		Me.ButtonRetireesInfoCancel.Visible = True
    '		Me.ButtonSaveRetireeParticipants.Enabled = True
    '		'Me.ButtonRetireesInfoSave.Enabled = True
    '		'Me.ButtonRetireesInfoSave.Visible = True
    '		Me.MakeLinkVisible()
    '		ViewState("SelectedSecState") = Nothing
    '		ViewState("SelectedState") = DropdownlistState.SelectedValue
    '		'Address_DropDownListState()
    '		Address_DropDownListStatePrimary()
    '	Catch ex As Exception
    '		Dim l_String_Exception_Message As String
    '		l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '		Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '	End Try
    'End Sub

    'Private Sub DropdownlistSecState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistSecState.SelectedIndexChanged
    '	Try
    '		Me.ButtonRetireesInfoCancel.Enabled = True
    '		Me.ButtonRetireesInfoCancel.Visible = True
    '		Me.ButtonSaveRetireeParticipants.Enabled = True
    '		'Me.ButtonRetireesInfoSave.Enabled = True
    '		'Me.ButtonRetireesInfoSave.Visible = True
    '		Me.MakeLinkVisible()
    '		ViewState("SelectedState") = Nothing
    '		ViewState("SelectedSecState") = DropdownlistSecState.SelectedValue
    '		'Address_DropDownListState()
    '		Address_DropDownListStateSecondary()
    '	Catch ex As Exception
    '		Dim l_String_Exception_Message As String
    '		l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '		Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '	End Try
    'End Sub

    Private Sub ButtonGeneralQDROPendingEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGeneralQDROPendingEdit.Click
        Try
            'Added by neeraj on 01-dec-2009 : issue is YRS 5.0-940 security Check

            Dim checkSecurity1 As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If checkSecurity1.Equals("True") Then
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralQDROPendingEdit", Convert.ToInt32(Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity1, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    ' START: PPP | 04/20/2016 | YRS-AT-2719 | Now this button event is not required
    'Private Sub ButtonSuppressedJSAnnuities_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSuppressedJSAnnuities.Click
    '    Dim l_string_PersId As String
    '    Dim l_SuppressJSAnnuitiesCnt As Int32
    '    Try
    '        'Page.Validate()
    '        'If Not Page.IsValid Then Exit Sub
    '        rfvGender.Enabled = False
    '        l_string_PersId = Session("PersId")

    '        Session("CurrentProcesstoConfirm") = "UnSuppressProcess"
    '        'Aparna IE7 08/10/2007
    '        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you want to unsuppress JS Annuities for this Retiree?", MessageBoxButtons.YesNo)


    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub
    ' END: PPP | 04/20/2016 | YRS-AT-2719 | Now this button event is not required

    'Rahul 02 Mar,06
    Private Sub DataGridFederalWithholding_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridFederalWithholding.ItemDataBound
        ''Dim l_button_Select As ImageButton

        ''Try
        ''    l_button_Select = e.Item.FindControl("ImageButtonSelect")
        ''    If (e.Item.ItemIndex = Me.DataGridFederalWithholding.SelectedIndex And Me.DataGridFederalWithholding.SelectedIndex >= 0) Then
        ''        l_button_Select.ImageUrl = "images\selected.gif"
        ''    End If

        ''    ''e.Item.Cells(1).Visible = False
        ''    ''e.Item.Cells(2).Visible = False

        ''    '''start of comment by hafiz on 24-Jan-2006
        ''    '''e.Item.Cells(6).Visible = False
        ''    '''end of comment by hafiz on 24-Jan-2006

        ''    '''start of addition by hafiz on 24-Jan-2006
        ''    ''e.Item.Cells(5).Visible = False
        ''    'end of addition by hafiz on 24-Jan-2006

        ''Catch ex As Exception
        ''    Dim l_String_Exception_Message As String
        ''    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        ''    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        ''End Try
    End Sub 'rahul 02 Mar,06

    Private Sub DataGridFederalWithholding_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridFederalWithholding.Load
    End Sub

    'Added By Dinesh Kanojia 21/12/2012
    Private Sub EditFederalWithHolding(ByVal Datagridrow As TableCellCollection, ByVal iIndex As Integer)
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check


            Dim i_Image_Edit As ImageButton

            i_Image_Edit = Datagridrow(0).FindControl("ImageDataGridFederalWithholdingEdit")
            'AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonFederalWithholdUpdate", Convert.ToInt32(Session("LoggedUserKey")))

            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'If Me.DataGridFederalWithholding.SelectedIndex <> -1 Then
            Session("cmbTaxEntity") = Datagridrow(4).Text.Trim
            Session("cmbWithHolding") = Datagridrow(3).Text.Trim
            Session("txtExemptions") = Datagridrow(1).Text.Trim
            Session("txtAddlAmount") = Datagridrow(2).Text.Trim
            Session("cmbMaritalStatus") = Datagridrow(5).Text.Trim

            'Shubhrata Mar 2nd,2007 YREN-3112
            'Session("IsNotFedTaxForMaritalStatus") = True 'commented by hafiz on 2-May-2007 as the issue was re-opened
            Session("IsFedTaxForMaritalStatus") = True  'added by hafiz on 2-May-2007 as the issue was re-opened
            'Shubhrata Mar 2nd,2007 YREN-3112

            Dim popupScript As String
            If Datagridrow(6).Text = "&nbsp;" Then
                popupScript = "<script language='javascript'>" & _
                     "window.open('UpdateFedWithHoldingInfo.aspx?Index=" & iIndex & "', 'CustomPopUp', " & _
                     "'width=800, height=400, menubar=no, Resizable=No,top=80,left=120, scrollbars=yes')" & _
                     "</script>"
            Else
                popupScript = "<script language='javascript'>" & _
                   "window.open('UpdateFedWithHoldingInfo.aspx?UniqueID=" & Datagridrow(6).Text & "', 'CustomPopUp', " & _
                   "'width=800, height=400, menubar=no, Resizable=No,top=80,left=120, scrollbars=yes')" & _
                   "</script>"
            End If



            Page.RegisterStartupScript("PopupScript2", popupScript)
            'Else
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
            'Exit Sub
            'End If
            ''''Getting the datarow corresponding to the UniqueID
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'Rahul 02 Mar,06
    Private Sub ButtonFederalWithholdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFederalWithholdUpdate.Click
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonFederalWithholdUpdate", Convert.ToInt32(Session("LoggedUserKey")))

            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If Me.DataGridFederalWithholding.SelectedIndex <> -1 Then
                Session("cmbTaxEntity") = Me.DataGridFederalWithholding.SelectedItem.Cells(4).Text.Trim
                Session("cmbWithHolding") = Me.DataGridFederalWithholding.SelectedItem.Cells(3).Text.Trim
                Session("txtExemptions") = Me.DataGridFederalWithholding.SelectedItem.Cells(1).Text.Trim
                Session("txtAddlAmount") = Me.DataGridFederalWithholding.SelectedItem.Cells(2).Text.Trim
                Session("cmbMaritalStatus") = Me.DataGridFederalWithholding.SelectedItem.Cells(5).Text.Trim

                'Shubhrata Mar 2nd,2007 YREN-3112
                'Session("IsNotFedTaxForMaritalStatus") = True 'commented by hafiz on 2-May-2007 as the issue was re-opened
                Session("IsFedTaxForMaritalStatus") = True  'added by hafiz on 2-May-2007 as the issue was re-opened
                'Shubhrata Mar 2nd,2007 YREN-3112

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
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
                Exit Sub
            End If
            ''''Getting the datarow corresponding to the UniqueID
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub    'Rahul 02 Mar,06

    Private Sub EditGeneralWithHolding(ByVal Datagridrow As TableCellCollection, ByVal iIndex As Integer)

        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
        HiddenFieldDirty.Value = "true"
        Try

            Dim i_Image_Edit As ImageButton

            i_Image_Edit = Datagridrow(0).FindControl("EditImageGeneralWithHold")

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            'AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralWithholdUpdate", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'If Me.DataGridGeneralWithhold.SelectedIndex <> -1 Then

            Session("cmbWithHoldingType") = Datagridrow(1).Text.Trim
            Session("txtAddAmount") = Datagridrow(2).Text.Trim
            Session("txtStartDate") = Datagridrow(3).Text.Trim
            Session("txtEndDate") = Datagridrow(4).Text.Trim
            Dim popupScript As String
            If Datagridrow(5).Text = "&nbsp;" Then
                popupScript = "<script language='javascript'>" & _
                   "window.open('UpdateGenHoldings.aspx?Index=" & iIndex & "', 'CustomPopUp', " & _
                   "'width=800, height=500, menubar=no, resizable=no,top=80,left=120, scrollbars=yes')" & _
                   "</script>"

            Else
                popupScript = "<script language='javascript'>" & _
                   "window.open('UpdateGenHoldings.aspx?UniqueID=" & Datagridrow(5).Text & "', 'CustomPopUp', " & _
                   "'width=800, height=500, menubar=no, resizable=no,top=80,left=120, scrollbars=yes')" & _
                   "</script>"
            End If

            Page.RegisterStartupScript("PopupScript2", popupScript)
            'Else
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
            'Exit Sub
            'End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'rahul 02 Mar,06
    Private Sub ButtonGeneralWithholdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGeneralWithholdUpdate.Click
        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
        HiddenFieldDirty.Value = "true"
        Try
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralWithholdUpdate", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If Me.DataGridGeneralWithhold.SelectedIndex <> -1 Then

                Session("cmbWithHoldingType") = Me.DataGridGeneralWithhold.SelectedItem.Cells(1).Text.Trim
                Session("txtAddAmount") = Me.DataGridGeneralWithhold.SelectedItem.Cells(2).Text.Trim
                Session("txtStartDate") = Me.DataGridGeneralWithhold.SelectedItem.Cells(3).Text.Trim
                Session("txtEndDate") = Me.DataGridGeneralWithhold.SelectedItem.Cells(4).Text.Trim
                Dim popupScript As String
                If Me.DataGridGeneralWithhold.SelectedItem.Cells(5).Text = "&nbsp;" Then
                    popupScript = "<script language='javascript'>" & _
                       "window.open('UpdateGenHoldings.aspx?Index=" & Me.DataGridGeneralWithhold.SelectedIndex & "', 'CustomPopUp', " & _
                       "'width=750, height=500, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
                       "</script>"

                Else
                    popupScript = "<script language='javascript'>" & _
                       "window.open('UpdateGenHoldings.aspx?UniqueID=" & Me.DataGridGeneralWithhold.SelectedItem.Cells(5).Text & "', 'CustomPopUp', " & _
                       "'width=750, height=500, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
                       "</script>"
                End If

                Page.RegisterStartupScript("PopupScript2", popupScript)
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
                Exit Sub
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub  'rahul 02 Mar,06

    Private Sub DataGridGeneralWithhold_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridGeneralWithhold.ItemDataBound
        ''Dim l_button_Select As ImageButton

        ''Try
        ''    l_button_Select = e.Item.FindControl("ImageButtonSelect")
        ''    If (e.Item.ItemIndex = Me.DataGridFederalWithholding.SelectedIndex And Me.DataGridFederalWithholding.SelectedIndex >= 0) Then
        ''        l_button_Select.ImageUrl = "images\selected.gif"
        ''    End If

        ''    e.Item.Cells(1).Visible = False
        ''    e.Item.Cells(2).Visible = False

        ''    'start of comment by hafiz on 24-Jan-2006
        ''    'e.Item.Cells(6).Visible = False
        ''    'end of comment by hafiz on 24-Jan-2006

        ''    'start of addition by hafiz on 24-Jan-2006
        ''    e.Item.Cells(5).Visible = False
        ''    'end of addition by hafiz on 24-Jan-2006

        ''Catch ex As Exception
        ''    Dim l_String_Exception_Message As String
        ''    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        ''    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        ''End Try
    End Sub

    Private Sub DataGridGeneralWithhold_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridGeneralWithhold.Load

    End Sub

    Private Sub MultiPageRetireesInformation_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MultiPageRetireesInformation.SelectedIndexChange

    End Sub

    Private Sub ButtonEditJSBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditJSBeneficiary.Click
        Try
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            'Commented by prasad 27-Sep-2011:For BT-895,YRS 5.0-1364 : prompt user to if changes not saved 
            'HiddenFieldDirty.Value = "true"
            'by Aparna 04/09/2007 -disable selection in the datagrid once the edit button is selected so that
            'the changes made are saved.

            Dim dgitem As DataGridItem
            'Dim l_ImageButton As ImageButton
            'For Each dgitem In Me.DataGridAnnuities.Items
            '	l_ImageButton = dgitem.FindControl("Imagebutton6")
            '	l_ImageButton.Enabled = False
            'Next
            Me.ReqValidatorAnnuityDOB.Enabled = True
            'Enabling the controls.
            Me.ButtonUpdateJSBeneficiary.Enabled = True
            Me.ButtonCancelJSBeneficiary.Enabled = True
            ' // START : SB | 07/07/2016 | YRS-AT-2382 | Cannot edit SSN here 
            Me.ButtonAnnuitiesSSNoEdit.Enabled = False
            Me.lnkbtnViewAnnuitiesSSNUpdate.Enabled = False
            ' Me.TextBoxAnnuitiesSSNo.Enabled = True  
            Me.TextBoxAnnuitiesSSNo.Enabled = False
            ' // END : SB | 07/07/2016 | YRS-AT-2382 | Cannot edit SSN here 
            Me.TextBoxAnnuitiesFirstName.Enabled = True
            Me.TextBoxAnnuitiesMiddleName.Enabled = True
            Me.TextBoxAnnuitiesLastName.Enabled = True
            'Commented/Added by Swopna -YRS 5.0-463-29 July08-Start
            'Me.TextBoxAnnuitiesDOB.Enabled = True
            Me.TextBoxAnnuitiesDOB.Enabled = False
            'Commented/Added by Swopna -YRS 5.0-463-29 July08-End
            Me.CheckboxSpouse.Enabled = True 'by Aparna  05/09/2007
            'Me.TextBoxDateDeceased.Enabled = True
            'Me.txtSpouse.Enabled = True
            'Me.ButtonRetireesInfoSave.Visible = True
            'Me.ButtonRetireesInfoSave.Visible = False
            '' Me.ButtonSaveRetireeParticipants.Visible = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            'Me.ButtonRetireesInfoSave.Enabled = True
            Me.ButtonRetireesInfoCancel.Enabled = True
            Me.ButtonRetireesInfoCancel.Visible = True

            'Commented/Added by Swopna -YRS 5.0-463-29 July08-Start
            'Me.PopcalendarDOB.Enabled = True
            Me.PopcalendarDOB.Enabled = False
            'Commented/Added by Swopna -YRS 5.0-463-29 July08-End

            Me.ButtonEditJSBeneficiary.Enabled = False
            Me.ButtonRetireesInfoOK.Enabled = False

            Me.MakeLinkVisible()
            'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            Me.AddressWebUserControlAnn.EnableControls = True
            'AA:BT:2306: YRS 5.0 2251  - Added to enable the control
            lnkParticipantAddress.Enabled = True
            ' Me.PopcalendarDeceased.Enabled = True
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'BS:2012:04:19:-restructure ActiveAsPrimary code
    Public Function ActiveAsPrimary()
        'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        Dim dr_PrimaryAddress As DataRow() = DirectCast(Session("Dr_PrimaryAddress"), DataRow())
        Dim dr_SecondaryAddress As DataRow() = DirectCast(Session("Dr_SecondaryAddress"), DataRow())
        'Added by Anudeep:14.03.2013 for telephone usercontrol
        Dim dt_PrimaryContact As DataTable = DirectCast(Session("dt_PrimaryContactInfo"), DataTable)
        Dim dt_SecondaryContact As DataTable = DirectCast(Session("dt_SecondaryContactInfo"), DataTable)

        'START: MMR | YRS-AT-3384 & YRS-AT-3385 | Interchanging Address ID when activating secondary address as primary address and primary address as secondary address to correctly insert old address id into log table
        Dim primaryUniqueID, secondaryUniqueID As String

        If dr_SecondaryAddress.Length > 0 AndAlso dr_PrimaryAddress.Length > 0 Then
            primaryUniqueID = Convert.ToString(dr_PrimaryAddress(0)("UniqueId"))
            secondaryUniqueID = Convert.ToString(dr_SecondaryAddress(0)("UniqueId"))

            dr_SecondaryAddress(0)("UniqueId") = primaryUniqueID
            dr_PrimaryAddress(0)("UniqueId") = secondaryUniqueID

            dr_SecondaryAddress(0).AcceptChanges()
            dr_PrimaryAddress(0).AcceptChanges()
        End If
        'END: MMR | YRS-AT-3384 & YRS-AT-3385 | Interchanging Address ID when activating secondary address as primary address and primary address as secondary address to correctly insert old address id into log table

        Session("Dr_PrimaryAddress") = dr_SecondaryAddress
        Session("Dr_SecondaryAddress") = dr_PrimaryAddress
        'Added by Anudeep:14.03.2013 for telephone usercontrol
        Session("dt_PrimaryContactInfo") = dt_SecondaryContact
        Session("dt_SecondaryContactInfo") = dt_PrimaryContact

        'Dim dt As DataTable = ds_PrimaryAddress1.Tables("EmailInfo")
        'If Not dt Is Nothing Then
        '    ds_PrimaryAddress1.Tables.Remove(dt)
        '    ds_SecondaryAddress1.Tables.Add(dt)
        'End If

        AddressWebUserControl1.LoadAddressDetail(dr_SecondaryAddress)
        'Added by Anudeep:14.03.2013 for telephone usercontrol
        TelephoneWebUserControl1.RefreshTelephoneDetail(dt_SecondaryContact)
        'RefreshPrimaryTelephoneDetail(ds_SecondaryAddress1)
        AddressWebUserControl2.LoadAddressDetail(dr_PrimaryAddress)
        'Added by Anudeep:14.03.2013 for telephone usercontrol
        TelephoneWebUserControl2.RefreshTelephoneDetail(dt_PrimaryContact)

        'Swap Notes detail
        Dim temp_Notes As String
        Dim temp_BitImp As String
        temp_Notes = AddressWebUserControl1.Notes
        AddressWebUserControl1.Notes = AddressWebUserControl2.Notes
        AddressWebUserControl2.Notes = temp_Notes

        temp_BitImp = AddressWebUserControl1.BitImp
        AddressWebUserControl1.BitImp = AddressWebUserControl2.BitImp
        AddressWebUserControl2.BitImp = temp_BitImp
        'Added by Anudeep:22.03.2013-For Checking active as Primary 
        Session("bln_Activateprimary") = True
        'Added by Anudeep:10.07.2013-For Checking active as Primary 
        AddressWebUserControl1.ChangesExist = "true"
        AddressWebUserControl2.ChangesExist = "true"
        'RefreshSecTelephoneDetail(ds_PrimaryAddress1)

        'Exit Function
        'Dim ds_PrimaryAddress As DataSet = DirectCast(Session("Ds_PrimaryAddress"), DataSet)
        'If Not ds_PrimaryAddress Is Nothing Then
        '	If Not ds_PrimaryAddress.Tables("AddressInfo") Is Nothing Then
        '		If ds_PrimaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
        '			'bitPrimary,bitActive
        '			ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary") = False
        '			ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive") = True
        '		End If
        '		'Add AddressInfo in DB

        '		If Not ds_PrimaryAddress.Tables("AddressInfo").GetChanges Is Nothing Then
        '			YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateParticipantAddress(ds_PrimaryAddress)
        '		End If
        '	End If
        '	If Not ds_PrimaryAddress.Tables("TelephoneInfo") Is Nothing Then
        '		'bitPrimary,bitActive
        '		Dim l_datarow As DataRow
        '		For Each l_datarow In ds_PrimaryAddress.Tables("TelephoneInfo").Rows
        '			l_datarow("bitPrimary") = False
        '			l_datarow("bitActive") = True
        '		Next
        '		'Add TelephoneInfo in DB
        '		If ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
        '			If Not ds_PrimaryAddress.Tables("TelephoneInfo").GetChanges Is Nothing Then
        '				YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(ds_PrimaryAddress)
        '			End If
        '		End If
        '	End If
        'End If

        'Dim ds_SecondaryAddress As DataSet = DirectCast(Session("Ds_SecondaryAddress"), DataSet)
        'If Not ds_SecondaryAddress Is Nothing Then
        '	If ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
        '		If Not ds_SecondaryAddress.Tables("AddressInfo") Is Nothing Then
        '			'bitPrimary,bitActive
        '			ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary") = True
        '			ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive") = True
        '			'Add AddressInfo in DB
        '			If Not ds_SecondaryAddress.Tables("AddressInfo").GetChanges Is Nothing Then
        '				YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateParticipantAddress(ds_SecondaryAddress)
        '			End If
        '		End If
        '	End If
        '	If ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
        '		'bitPrimary,bitActive
        '		Dim l_datarow As DataRow
        '		For Each l_datarow In ds_SecondaryAddress.Tables("TelephoneInfo").Rows
        '			l_datarow("bitPrimary") = True
        '			l_datarow("bitActive") = True
        '		Next
        '		'Add TelephoneInfo in DB
        '		If ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
        '			If Not ds_SecondaryAddress.Tables("TelephoneInfo").GetChanges Is Nothing Then
        '				YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(ds_SecondaryAddress)
        '			End If
        '		End If
        '	End If
        'End If
    End Function

    Private Sub ButtonActivateAsPrimary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonActivateAsPrimary.Click
        Try
            Dim l_stringMessage As String = String.Empty
            l_stringMessage = Me.ValidateAddress() 'by Aparna to avoid the blank primary address 12/11/2007
            If l_stringMessage <> String.Empty Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_stringMessage, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'BS:2012:04:19:-restructure address code
            'UpdateAddress(False, True, True, True)
            ActiveAsPrimary() 'BS:2012:04:19:-restructure address code
            ''Added by Anudeep:14.03.2013 for telephone usercontrol
            ButtonEditAddress.Enabled = False
            ButtonActivateAsPrimary.Enabled = False
            ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            btnEditPrimaryContact.Enabled = False
            btnEditSecondaryContact.Enabled = False
            AddressWebUserControl1.EnableControls = False
            AddressWebUserControl2.EnableControls = False
            HiddenFieldDirty.Value = "true" ' Manthan | 2016.04.06 | YRS-AT-2328 | Setting hidden field value to true to display confirm message when user clicks on close button without saving.
            Exit Sub
            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'AddressWebUserControl1.guiEntityId = Session("PersId")
            'AddressWebUserControl1.IsPrimary = 1
            'AddressWebUserControl1.ShowDataForParticipant = 1
            'AddressWebUserControl1.ShowDataForParticipant()
            'AddressWebUserControl2.guiEntityId = Session("PersId")
            'AddressWebUserControl2.IsPrimary = 0
            'AddressWebUserControl2.ShowDataForParticipant = 0
            'AddressWebUserControl2.ShowDataForParticipant()

            'LoadGeneraltab()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Added btnDeactivateSecondaryAddrs_click method to perform deactivation of secondary address 
    Private Sub btnDeactivateSecondaryAddrs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeactivateSecondaryAddrs.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnDeactivateRetireeSecondaryAddrs", Convert.ToInt32(Session("LoggedUserKey")))

        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            Exit Sub
        End If

        Dim dr_SecondaryAddress As DataRow() = DirectCast(Session("Dr_SecondaryAddress"), DataRow())
        dr_SecondaryAddress(0).Item("isActive") = False
        If Not Session("bln_Activateprimary") Is Nothing Then
            dr_SecondaryAddress(0).Item("isPrimary") = False
        End If
        Session("Dr_SecondaryAddress") = dr_SecondaryAddress
        AddressWebUserControl2.ClearControls() = ""
        btnDeactivateSecondaryAddrs.Enabled = False
        ButtonRetireesInfoCancel.Enabled = True
        ButtonActivateAsPrimary.Enabled = False
        ButtonSaveRetireeParticipants.Enabled = True
        HiddenFieldDirty.Value = "true" ' Manthan | 2016.04.06 | YRS-AT-2328 | Setting hidden field value to true to display confirm message when user clicks on close button without saving.
    End Sub
    'End - Manthan | 2016.02.24 | YRS-AT-2328 | Added btnDeactivateSecondaryAddrs_click method to perform deactivation of secondary address 
    Private Sub LinkButtonIDM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonIDM.Click
        Try
            'Dim l_string_VignettePath As String
            Dim l_string_FundNo As String
            'By Aparna Samala   02/01/2007
            '  l_string_VignettePath = Session("VignettePath")
            'l_string_VignettePath = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + "yrsResultsForm.aspx?fundid="

            '14-Nov-2008 Priya Change IDM path as per Ragesh mail as on 11/13/2008
            l_string_VignettePath = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_Participant"), Session("FundNo"))
            'End 14-Nov-2008

            l_string_FundNo = Session("FundNo")


            Dim popupScript As String = "<script language='javascript'>" & _
                "window.open('" + l_string_VignettePath + "', 'IDMPopUp', " & _
                "'width=1000, height=750, menubar=no, Resizable=Yes,top=0,left=0, scrollbars=yes')" & _
                "</script>"


            Page.RegisterStartupScript("IDMPopUpScript", popupScript)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    'Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control
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
            l_Int_UserId = Convert.ToInt32(Session("LoggedUserKey"))

            l_String_FormName = Session("FormName").ToString().Trim()

            ds_AllsecItems = YMCARET.YmcaBusinessObject.ControlSecurityBOClass.GetSecuredControlsOnForm(l_String_FormName)

            'Aparna -YREN 3115 13/03/2007
            'To Find if the user belongs to teh Notes Admin Group so that all the checkboxes in the notes grid be enabled
            Dim l_intLoggedUser As Integer
            l_intLoggedUser = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.GetLoginNotesUser(l_Int_UserId)

            If l_intLoggedUser = 1 Then
                Me.NotesGroupUser = True
            Else
                Me.NotesGroupUser = False
            End If

            'Aparna -YREN 3115 13/03/2007
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
                    '********Code by ashutosh on 06-May -06
                    Me.HiddenSecControlName.Value = l_string_controlNames
                    '***********************************************
                    'Response.Write(l_string_controlNames)
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'Code Written by Atul Deo on 4th Sep 2005 for Annuities Paid

    Public Sub LoadAnnuityPaidTab()
        Try
            Dim l_string_PersId As String
            l_string_PersId = Session("PersId")
            Dim l_dataset_AnnuityPaid As DataSet
            l_dataset_AnnuityPaid = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAnnuityPaid(l_string_PersId)
            ViewState("l_dataset_AnnuityPaid") = l_dataset_AnnuityPaid
            If (l_dataset_AnnuityPaid.Tables(0).Rows.Count > 0) Then
                'CommonModule.HideColumnsinDataGrid(l_dataset_AnnuityPaid, Me.DataGridAnnuitiesPaid, "")
                DataGridAnnuitiesPaid.DataSource = l_dataset_AnnuityPaid
                DataGridAnnuitiesPaid.DataBind()
                'DisbursementID
            End If
        Catch
            Throw
        End Try
    End Sub
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

    Public Sub LoadAnnuitiesTab()
        Dim l_string_PersId As String
        Dim l_string_FundId As String
        Dim l_dataset_AnnuityPaid As DataSet
        Dim l_string_AnnuityId As String
        Dim l_datarow As DataRow()
        Dim l_string_AnnuityJointSurvivorsID As String = String.Empty
        Dim dsAddress As DataSet
        Dim drAddressRow As DataRow()
        Try
            l_string_PersId = Session("PersId")
            l_string_FundId = Session("FundId")
            l_dataset_AnnuityPaid = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAnnuities(l_string_FundId)
            'Start:Anudeep01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            dsAddress = Address.GetAddressOfBeneficiariesByPerson(l_string_PersId, EnumEntityCode.PERSON)

            If Not dsAddress Is Nothing Then
                Session("BeneficiaryAddress") = dsAddress.Tables(0)
            End If

            Session("AnnuityDetails") = l_dataset_AnnuityPaid 'by Aparna 04/09/2007

            If (l_dataset_AnnuityPaid.Tables(0).Rows.Count > 0) Then
                DataGridAnnuities.DataSource = l_dataset_AnnuityPaid
                DataGridAnnuities.DataBind()
                Me.DataGridAnnuities.SelectedIndex = 0

                'Dim l_button_Select As ImageButton
                'l_button_Select = DataGridAnnuities.Items(0).FindControl("Imagebutton6")
                'If Not l_button_Select Is Nothing Then
                '    l_button_Select.ImageUrl = "images\selected.gif"
                'End If



                'load the details of the First row.
                l_string_AnnuityId = System.Convert.ToString(l_dataset_AnnuityPaid.Tables(0).Rows(0)("ID"))
                Session("AnnuityId") = l_string_AnnuityId
                l_string_AnnuityJointSurvivorsID = System.Convert.ToString(l_dataset_AnnuityPaid.Tables(0).Rows(0)("AnnuityJointSurvivorsID"))

                Session("AnnuityJointSurvivorsID") = l_string_AnnuityJointSurvivorsID
                ' // START : SB | 07/07/2016 | YRS-AT-2382 | inserting Annuity Beneficiaries in Aduit Log Record DataTable
                Session("AuditBeneficiariesTable") = CreateAuditTable()
                PrepareAuditTable()
                ' // END : SB | 07/07/2016 | YRS-AT-2382 | inserting Annuity Beneficiaries in Aduit Log Record DataTable
                l_datarow = l_dataset_AnnuityPaid.Tables("JSAnnuitiesDetails").Select("AnnuityJointSurvivorsID = '" + l_string_AnnuityJointSurvivorsID + "'")
                If l_datarow.Length > 0 Then
                    TextBoxAnnuitiesSSNo.Text = l_datarow(0)("SSNo").ToString()
                    TextBoxAnnuitiesFirstName.Text = l_datarow(0)("First").ToString()
                    TextBoxAnnuitiesMiddleName.Text = l_datarow(0)("Middle").ToString()
                    TextBoxAnnuitiesLastName.Text = l_datarow(0)("Last").ToString()
                    TextBoxAnnuitiesDOB.Text = l_datarow(0)("DateOfBirth").ToString()
                    TextBoxDateDeceased.Text = l_datarow(0)("Date Deceased").ToString()
                    If l_datarow(0).Item("Spouse") = "True" Then
                        Me.CheckboxSpouse.Checked = True
                    Else
                        Me.CheckboxSpouse.Checked = False
                    End If
                    'by aparna 12/09/2007
                    If TextBoxDateDeceased.Text <> String.Empty Then
                        ButtonEditJSBeneficiary.Enabled = False
                    Else
                        ButtonEditJSBeneficiary.Enabled = True
                    End If
                    ' ButtonEditJSBeneficiary.Enabled = True
                    'Anudeep:13.06.2013 Capturing Annuity Beneficiary Address
                    AddressWebUserControlAnn.IsPrimary = 1
                    AddressWebUserControlAnn.EntityCode = EnumEntityCode.PERSON.ToString()
                    If HelperFunctions.isNonEmpty(dsAddress) AndAlso HelperFunctions.isNonEmpty(dsAddress.Tables("Address")) Then
                        drAddressRow = dsAddress.Tables("Address").Select("BenSSNo='" & l_datarow(0)("SSNo").ToString() & "'")
                        AddressWebUserControlAnn.LoadAddressDetail(drAddressRow)
                    Else
                        AddressWebUserControlAnn.LoadAddressDetail(Nothing)
                    End If
                    ' // START : SB | 07/07/2016 | YRS-AT-2382 | If Annuity Beneficiaries exists then editSSN button and link button is Enabled
                    ButtonAnnuitiesSSNoEdit.Enabled = True
                    lnkbtnViewAnnuitiesSSNUpdate.Enabled = True
                    ' // END : SB | 07/07/2016 | YRS-AT-2382 | If Annuity Beneficiaries exists then editSSN button and link button is Enabled
                Else
                    'NP:2008.06.23:YRS-5.0-463 - If the annuity is not of type JS then clean out the beneficiary info.
                    TextBoxAnnuitiesSSNo.Text = ""
                    TextBoxAnnuitiesFirstName.Text = ""
                    TextBoxAnnuitiesMiddleName.Text = ""
                    TextBoxAnnuitiesLastName.Text = ""
                    TextBoxAnnuitiesDOB.Text = ""
                    TextBoxDateDeceased.Text = ""
                    CheckboxSpouse.Checked = False
                    'END: NP:2008.06.23 changes
                    ButtonEditJSBeneficiary.Enabled = False
                    'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    AddressWebUserControlAnn.LoadAddressDetail(Nothing)
                    ' // START : SB | 07/07/2016 | YRS-AT-2382 | If Annuity Beneficiaries doesn't exists then editSSN button and link button is Disabled
                    ButtonAnnuitiesSSNoEdit.Enabled = False
                    lnkbtnViewAnnuitiesSSNUpdate.Enabled = False
                    ' // END : SB | 07/07/2016 | YRS-AT-2382 | If Annuity Beneficiaries doesn't exists then editSSN button and link button is Disabled
                End If
                'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                AddressWebUserControlAnn.EnableControls = False
                'AA:BT:2306: YRS 5.0 2251  - Added to disable the control
                lnkParticipantAddress.Enabled = False
                'Dim l_dataset_Details As DataSet
                'l_dataset_Details = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpAnnuityInfo(l_string_AnnuityId)
                'If l_dataset_Details.Tables(0).Rows.Count > 0 Then
                '    TextBoxAnnuitiesSSNo.Text = IIf(IsDBNull(l_dataset_Details.Tables(0).Rows(0).Item("SSNo")), "", l_dataset_Details.Tables(0).Rows(0).Item("SSNo"))
                '    TextBoxAnnuitiesFirstName.Text = IIf(IsDBNull(l_dataset_Details.Tables(0).Rows(0).Item("First")), "", l_dataset_Details.Tables(0).Rows(0).Item("First"))
                '    TextBoxAnnuitiesMiddleName.Text = IIf(IsDBNull(l_dataset_Details.Tables(0).Rows(0).Item("Middle")), "", l_dataset_Details.Tables(0).Rows(0).Item("Middle"))
                '    TextBoxAnnuitiesLastName.Text = IIf(IsDBNull(l_dataset_Details.Tables(0).Rows(0).Item("Last")), "", l_dataset_Details.Tables(0).Rows(0).Item("Last"))
                '    TextBoxAnnuitiesDOB.Text = IIf(IsDBNull(l_dataset_Details.Tables(0).Rows(0).Item("DateOfBirth")), "", l_dataset_Details.Tables(0).Rows(0).Item("DateOfBirth"))
                '    TextBoxDateDeceased.Text = IIf(IsDBNull(l_dataset_Details.Tables(0).Rows(0).Item("Date Deceased")), "", l_dataset_Details.Tables(0).Rows(0).Item("Date Deceased"))
                '    If l_dataset_Details.Tables(0).Rows(0).Item("Spouse") = "True" Then
                '        txtSpouse.Text = "Y"
                '    Else
                '        txtSpouse.Text = "N"
                '    End If
                '    ButtonEditJSBeneficiary.Enabled = True
                'Else
                '    ButtonEditJSBeneficiary.Enabled = False
                'End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'Dineshk            2013.09.18      YRS 5.0-2204:Cannot see other annuity beneficiaries in Annuities display
    'Add optional parameter for Annuityjointsurvivorsid
    Public Sub LoadJointSurvivorDetails(Optional ByVal strAnnuityJointSurvivorsID As String = "")
        Dim l_string_AnnuityJointSurvivorsID As String = String.Empty
        Dim l_datarow As DataRow()
        Dim l_dataset_AnnuityDetails As DataSet
        Dim dtAddress As DataTable
        Dim drAddressRow As DataRow()
        Try
            l_string_AnnuityJointSurvivorsID = Me.DataGridAnnuities.SelectedItem.Cells(9).Text.ToString.Trim()
            'AA:24.09.2013:BT-1501:Added fro checking if strAnnuityJointSurvivorsID is emty then only assighn to the local variable else do not
            If strAnnuityJointSurvivorsID <> "" Then
                l_string_AnnuityJointSurvivorsID = strAnnuityJointSurvivorsID
            End If
            Session("AnnuityJointSurvivorsID") = l_string_AnnuityJointSurvivorsID
            If Not Session("AnnuityDetails") Is Nothing Then
                l_dataset_AnnuityDetails = Session("AnnuityDetails")
                l_datarow = l_dataset_AnnuityDetails.Tables("JSAnnuitiesDetails").Select("AnnuityJointSurvivorsID = '" + l_string_AnnuityJointSurvivorsID + "'")
                If l_datarow.Length > 0 Then
                    TextBoxAnnuitiesSSNo.Text = l_datarow(0)("SSNo").ToString()
                    TextBoxAnnuitiesFirstName.Text = l_datarow(0)("First").ToString()
                    TextBoxAnnuitiesMiddleName.Text = l_datarow(0)("Middle").ToString()
                    TextBoxAnnuitiesLastName.Text = l_datarow(0)("Last").ToString()
                    TextBoxAnnuitiesDOB.Text = l_datarow(0)("DateOfBirth").ToString()
                    TextBoxDateDeceased.Text = l_datarow(0)("Date Deceased").ToString()
                    If l_datarow(0).Item("Spouse") = "True" Then
                        Me.CheckboxSpouse.Checked = True
                    Else
                        Me.CheckboxSpouse.Checked = False
                    End If
                    'If Death Date is updated for the JS Beneficiary, further edit to the details should not be allowed.
                    'by Aparna 12/09/2007
                    If TextBoxDateDeceased.Text <> String.Empty Then
                        ButtonEditJSBeneficiary.Enabled = False
                    Else
                        ButtonEditJSBeneficiary.Enabled = True
                    End If

                    dtAddress = Session("BeneficiaryAddress")

                    If HelperFunctions.isNonEmpty(dtAddress) Then
                        drAddressRow = dtAddress.Select("BenSSNo='" & TextBoxAnnuitiesSSNo.Text & "'")
                        AddressWebUserControlAnn.LoadAddressDetail(drAddressRow)
                    Else
                        AddressWebUserControlAnn.LoadAddressDetail(Nothing)
                    End If

                Else
                    'NP:2008.06.23:YRS-5.0-463 - If the annuity is not of type JS then clean out the beneficiary info.
                    TextBoxAnnuitiesSSNo.Text = ""
                    TextBoxAnnuitiesFirstName.Text = ""
                    TextBoxAnnuitiesMiddleName.Text = ""
                    TextBoxAnnuitiesLastName.Text = ""
                    TextBoxAnnuitiesDOB.Text = ""
                    TextBoxDateDeceased.Text = ""
                    CheckboxSpouse.Checked = False
                    'END: NP:2008.06.23 changes
                    ButtonEditJSBeneficiary.Enabled = False
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function UpdateJointSurvivorDetails() As String

        Dim l_dataset_AnnuityDetails As DataSet
        Dim l_datarow As DataRow()
        Dim l_string_AnnuityId As String = String.Empty
        Dim l_bitSpouse As Integer
        'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        Dim dtAddress As DataTable
        Dim strBeneTaxNumber As String
        Dim l_string_ErrorMessage As String
        Dim drAddressRow As DataRow
        Dim drRow As DataRow()
        Try


            If Not Session("AnnuityJointSurvivorsID") Is Nothing Then
                l_string_AnnuityId = Session("AnnuityJointSurvivorsID")
                If Not Session("AnnuityDetails") Is Nothing Then
                    l_dataset_AnnuityDetails = Session("AnnuityDetails")
                    If l_dataset_AnnuityDetails.Tables("JSAnnuitiesDetails").Rows.Count > 0 Then

                        'Anudeep:14.06.2013 Validating Annuity beneficiary Address
                        If AddressWebUserControlAnn.Address1 <> "" Then
                            l_string_ErrorMessage = AddressWebUserControlAnn.ValidateAddress()
                            If l_string_ErrorMessage <> "" Then
                                Me.MakeLinkVisible()
                                MessageBox.Show(PlaceHolder1, "YMCA", l_string_ErrorMessage, MessageBoxButtons.OK)
                                Exit Function
                            End If
                        End If

                        l_datarow = l_dataset_AnnuityDetails.Tables("JSAnnuitiesDetails").Select("AnnuityJointSurvivorsID = '" + l_string_AnnuityId + "'")
                        strBeneTaxNumber = l_datarow(0)("SSNo").ToString().Trim()
                        If l_datarow(0)("SSNo").ToString().Trim() <> TextBoxAnnuitiesSSNo.Text.Trim() Then
                            l_datarow(0)("SSNo") = TextBoxAnnuitiesSSNo.Text.Trim()
                        End If

                        If l_datarow(0)("First").ToString().Trim() <> TextBoxAnnuitiesFirstName.Text.Trim() Then
                            l_datarow(0)("First") = TextBoxAnnuitiesFirstName.Text.Trim()
                        End If

                        If l_datarow(0)("Middle").ToString().Trim() <> TextBoxAnnuitiesMiddleName.Text.Trim() Then
                            l_datarow(0)("Middle") = TextBoxAnnuitiesMiddleName.Text.Trim()
                        End If

                        If l_datarow(0)("Last").ToString().Trim() <> TextBoxAnnuitiesLastName.Text.Trim() Then
                            l_datarow(0)("Last") = TextBoxAnnuitiesLastName.Text.Trim()
                        End If
                        If l_datarow(0)("DateOfBirth").ToString().Trim() <> TextBoxAnnuitiesDOB.Text.Trim() And TextBoxAnnuitiesDOB.Text <> "" Then
                            l_datarow(0)("DateOfBirth") = TextBoxAnnuitiesDOB.Text.Trim()
                        End If

                        If l_datarow(0)("Date Deceased").ToString().Trim() <> TextBoxDateDeceased.Text.Trim() Then
                            If Me.TextBoxDateDeceased.Text.Trim() = "" Then
                                l_datarow(0)("Date Deceased") = System.DBNull.Value
                            Else
                                l_datarow(0)("Date Deceased") = TextBoxDateDeceased.Text.Trim()
                            End If
                        End If

                        If CheckboxSpouse.Checked = True Then
                            l_bitSpouse = 1
                        Else
                            l_bitSpouse = 0
                        End If

                        If l_datarow(0)("Spouse") <> l_bitSpouse Then
                            l_datarow(0)("Spouse") = l_bitSpouse
                        End If
                        'If l_datarow(0).Item("Spouse") = "True" Then
                        '    l_bitSpouse = "Y"
                        'Else
                        '    l_bitSpouse = "N"
                        'End If

                        'If l_bitSpouse.Trim() <> txtSpouse.Text Then
                        '    If txtSpouse.Text.Trim() = "Y" Then
                        '        l_datarow(0)("Spouse") = 1
                        '    Else
                        '        l_datarow(0)("Spouse") = 0
                        '    End If

                        'End If
                        Session("AnnuityDetails") = l_dataset_AnnuityDetails
                        'Start:Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        If AddressWebUserControlAnn.Address1 <> "" And AddressWebUserControlAnn.City <> "" And
                            AddressWebUserControlAnn.DropDownListCountryValue <> "" Then

                            If Session("BeneficiaryAddress") Is Nothing Then
                                dtAddress = Address.CreateAddressDatatable()
                            Else
                                dtAddress = Session("BeneficiaryAddress")
                                drRow = dtAddress.Select("BenSSNo='" & strBeneTaxNumber & "'")
                            End If
                            If Not drRow Is Nothing AndAlso drRow.Length > 0 Then
                                drAddressRow = drRow(0)
                            Else
                                drAddressRow = dtAddress.NewRow()
                            End If
                            drAddressRow("addr1") = AddressWebUserControlAnn.Address1.Replace(",", "").Trim()
                            drAddressRow("addr2") = AddressWebUserControlAnn.Address2.Replace(",", "").Trim()
                            drAddressRow("addr3") = AddressWebUserControlAnn.Address3.Replace(",", "").Trim()
                            drAddressRow("city") = AddressWebUserControlAnn.City.Replace(",", "").Trim()
                            drAddressRow("state") = AddressWebUserControlAnn.DropDownListStateValue.Replace(",", "").Trim()
                            drAddressRow("zipCode") = AddressWebUserControlAnn.ZipCode.Replace(",", "").Replace("-", "").Trim()
                            drAddressRow("country") = AddressWebUserControlAnn.DropDownListCountryValue.Replace(",", "").Trim()
                            drAddressRow("isActive") = True
                            drAddressRow("isPrimary") = True
                            If AddressWebUserControlAnn.EffectiveDate <> String.Empty Then
                                drAddressRow("effectiveDate") = AddressWebUserControlAnn.EffectiveDate
                            Else
                                drAddressRow("effectiveDate") = System.DateTime.Now()
                            End If
                            drAddressRow("isBadAddress") = AddressWebUserControlAnn.IsBadAddress
                            drAddressRow("addrCode") = "HOME"
                            drAddressRow("entityCode") = EnumEntityCode.PERSON.ToString()
                            'Anudeep:22.08.2013-YRS 5.0-1862:Add notes record when user enters address in any module.
                            drAddressRow("Note") = "Beneficiary " + TextBoxAnnuitiesFirstName.Text.Trim + " " + TextBoxAnnuitiesLastName.Text.Trim + " " + AddressWebUserControlAnn.Notes 'AA:11.06.2014 BT:2554 - YRS 5.0-2375 - Added to trim the name while adding to the notes
                            drAddressRow("bitImportant") = AddressWebUserControlAnn.BitImp
                            drAddressRow("BenSSNo") = TextBoxAnnuitiesSSNo.Text
                            drAddressRow("PersID") = Session("PersId").ToString()
                            If drRow Is Nothing OrElse drRow.Length = 0 Then
                                dtAddress.Rows.Add(drAddressRow)
                            End If
                            Session("BeneficiaryAddress") = dtAddress
                        End If
                        'End:Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    End If
                End If
            Else
                LoadJointSurvivorDetails()
            End If

            'added by Aparna  -To have things at one place -11/10/2007
            'enable the selection in the datgrid
            Dim dgitem As DataGridItem
            'Dim l_ImageButton As ImageButton
            'For Each dgitem In Me.DataGridAnnuities.Items
            '    l_ImageButton = dgitem.FindControl("Imagebutton6")
            '    l_ImageButton.Enabled = True
            'Next

            Me.ButtonUpdateJSBeneficiary.Enabled = False
            Me.ButtonEditJSBeneficiary.Enabled = True
            Me.ButtonCancelJSBeneficiary.Enabled = False
            Session("EnableSaveCancel") = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            Me.TextBoxAnnuitiesSSNo.Enabled = False
            Me.TextBoxAnnuitiesFirstName.Enabled = False
            Me.TextBoxAnnuitiesMiddleName.Enabled = False
            Me.TextBoxAnnuitiesLastName.Enabled = False
            Me.TextBoxAnnuitiesDOB.Enabled = False
            Me.TextBoxDateDeceased.Enabled = False
            Me.ButtonRetireesInfoOK.Enabled = False
            'Me.txtSpouse.Enabled = False
            Me.CheckboxSpouse.Enabled = False 'by aparna 05/09/2007
            'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            AddressWebUserControlAnn.EnableControls = False
            'AA:BT:2306: YRS 5.0 2251  - Added to disable the control
            lnkParticipantAddress.Enabled = False
            'Anudeep:19.09.2013:Bt-2204:Commented below line for not setting read-only to true
            'AddressWebUserControlAnn.MakeReadonly = True

            'START: PPP | 08/01/2016 | YRS-AT-2382 | On annuity beneficiary save enable Edit and View SSN Updates link
            Me.ButtonAnnuitiesSSNoEdit.Enabled = True
            Me.lnkbtnViewAnnuitiesSSNUpdate.Enabled = True
            'END: PPP | 08/01/2016 | YRS-AT-2382 | On annuity beneficiary save enable Edit and View SSN Updates link
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''''Public Sub LoadBeneficiariesTab()
    '    Try
    '        Dim cache As CacheManager
    '        cache = CacheFactory.GetCacheManager()
    '        Dim l_string_PersId As String
    '        Dim l_dataset_Beneficiaries As DataSet
    '        l_string_PersId = Session("PersId")
    '        Dim dr As DataRow
    '        If Session("blnAddBeneficiaries") = True Or Session("blnUpdateBeneficiaries") = True And Not Session("blnCancel") = True Then
    '            l_dataset_Beneficiaries = cache("Beneficiaries")
    '            If l_dataset_Beneficiaries Is Nothing Then
    '                l_dataset_Beneficiaries = New DataSet
    '                Dim dt As New DataTable
    '                dt.Columns.Add("Name")
    '                dt.Columns.Add("Name2")
    '                dt.Columns.Add("TaxID")
    '                dt.Columns.Add("Rel")
    '                dt.Columns.Add("Birthdate")
    '                dt.Columns.Add("BeneficiaryTypeCode")
    '                dt.Columns.Add("Groups")
    '                dt.Columns.Add("Lvl")
    '                dt.Columns.Add("Pct")

    '                l_dataset_Beneficiaries.Tables.Add(dt)
    '            End If
    '            If Not (Session("blnUpdateBeneficiaries") = True) Then
    '                dr = l_dataset_Beneficiaries.Tables(0).NewRow
    '                dr("PersId") = l_string_PersId
    '                dr("Name") = Session("Name")
    '                dr("Name2") = Session("Name2")
    '                dr("TaxID") = Session("TaxID")
    '                dr("Rel") = Session("Rel")
    '                dr("Birthdate") = Session("Birthdate")
    '                dr("BeneficiaryTypeCode") = Session("Vrbeneficiaries.BeneficiaryTypeCode")

    '                dr("Groups") = Session("Groups")
    '                dr("Lvl") = Session("Lvl")
    '                dr("Pct") = Session("Pct")

    '                l_dataset_Beneficiaries.Tables(0).Rows.Add(dr)
    '            End If

    '            'After adding the row to the dataset reset the session variable
    '            Session("blnAddBeneficiaries") = False
    '            Session("blnUpdateBeneficiaries") = False

    '        Else
    '            If l_dataset_Beneficiaries Is Nothing Then
    '                l_dataset_Beneficiaries = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpBeneficiaries(l_string_PersId)
    '            Else

    '                l_dataset_Beneficiaries = cache("Beneficiaries")
    '            End If
    '        End If

    '        'If (l_dataset_Beneficiaries.Tables(0).Rows.Count > 0) Then
    '        DataGridBeneficiaries.DataSource = l_dataset_Beneficiaries
    '        DataGridBeneficiaries.DataBind()
    '        'End If
    '        cache.Add("Beneficiaries", l_dataset_Beneficiaries)
    '        Session("blnCancel") = False
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub
    Private Function BankInfoShowCheck(ByVal p_datatset_Bank As DataSet, ByVal p_bool_Apply As Boolean) As Boolean
        Dim l_bool_ShowCheck As Boolean
        'added by prasad on 10-08-2011 for YRS 5.0-1387 : Display IAT payment method on banking tab
        Dim l_bool_ShowIAT As Boolean
        Dim l_datarows_OK() As DataRow
        'Dim i As Integer
        Dim l_dataset_Retirees As DataSet
        Dim l_datatable_RetireeBank As DataTable
        Dim l_datarows As DataRow()
        Dim l_int_loopcount As Integer
        Dim l_string_PersId As String
        Dim l_date_EffDate As DateTime
        Dim l_datetime_Maindate As DateTime
        Dim l_datetime_Today As DateTime

        Try
            'start - added by hafiz on 11-Dec-2006 for YREN-2977
            'Modified,Added, and Commented by Hafiz and Ashutosh Patil for YREN-2979
            'l_string_PersId = Session("PersId")
            'Added By Ashutosh Patil as on 19-01-2007 for YREN-2979
            'added by prasad on 10-08-2011 for YRS 5.0-1387 : Display IAT payment method on banking tab
            l_bool_ShowIAT = False
            l_bool_ShowCheck = True
            l_datetime_Today = Date.Now
            If Not p_datatset_Bank Is Nothing Then
                l_datatable_RetireeBank = p_datatset_Bank.Tables(0)
                l_int_loopcount = l_datatable_RetireeBank.Rows.Count
                If l_int_loopcount = 0 Then
                    l_bool_ShowCheck = True
                Else
                    'l_datarows = l_datatable_RetireeBank.Select("Max(EffecDate) AND EffecDate <= '" & Today.Date() & "'")
                    Dim l_string_SearchCriteria As String
                    'Dim l_string_Maindate As String
                    'Added By Ashutosh Patil as on 23-Mar-2007
                    'YREN-3222
                    l_datatable_RetireeBank = IsCheckIsNullEffectiveDate(l_datatable_RetireeBank)

                    l_string_SearchCriteria = "dtmEffDate <= '" & l_datetime_Today & "'"
                    'Do While l_int_loopcount <> 0
                    'l_string_Maindate = l_datatable_RetireeBank.Compute("MAX(dtmEffDate)", l_string_SearchCriteria)
                    If IsDBNull(l_datatable_RetireeBank.Compute("MAX(dtmEffDate)", l_string_SearchCriteria)) Then
                        l_bool_ShowCheck = True
                        'Exit Do
                    Else
                        'Shifted By Ashutosh Patil as on 22-Mar-2007
                        'YREN - 2979 If in case MAX(dtmEffDate) is null i.e if it is not less than current 
                        'date or which can be equal to current date then it will not come in the else
                        'condition
                        l_datetime_Maindate = l_datatable_RetireeBank.Compute("MAX(dtmEffDate)", l_string_SearchCriteria)

                        l_datarows = l_datatable_RetireeBank.Select("dtmEffDate='" & l_datetime_Maindate & "' ")
                        If l_datarows.Length = 0 Then
                            l_bool_ShowCheck = True
                        Else
                            'If UCase(l_datarows(0)("PaymentDesc")) = "EFT" And l_datarows(0)("chrEFTStatus") = "O" Then 'fixing as reported by Purushottam on 21-Feb-2008 by Hafiz.
                            If l_datarows(0)("PaymentDesc").ToString.ToUpper() = "EFT" And l_datarows(0)("chrEFTStatus").ToString() = "O" Then   'handling null values in the field as reported by Purushottam on 21-Feb-2008 by Hafiz.
                                'Above statement updated by sanjay for YRS 5.0-874
                                l_bool_ShowCheck = False
                                Session("FundType") = l_datarows(0)("ParticipantType").ToString().Trim
                                'Commented By Ashutosh Patil as on 23-01-2007 for YREN-2979
                                'Exit Do
                                'ElseIf UCase(l_datarows(0)("PaymentDesc")) = "CHECK" Then
                                'l_bool_ShowCheck = True
                                'Exit Do
                                'added by prasad on 10-08-2011 for YRS 5.0-1387 : Display IAT payment method on banking tab
                            ElseIf l_datarows(0)("PaymentDesc").ToString.ToUpper() = "IAT" And l_datarows(0)("chrEFTStatus").ToString() = "O" Then
                                l_bool_ShowCheck = False
                                l_bool_ShowIAT = True
                            Else
                                l_bool_ShowCheck = True
                                'Commented By Ashutosh Patil as on 23-01-2007 for YREN-2979
                                'l_datetime_Today = l_datetime_Maindate
                                'l_string_SearchCriteria = "dtmEffDate < '" & l_datetime_Today & "'"
                            End If
                        End If
                    End If


                    'l_int_loopcount = l_int_loopcount - 1
                    'Loop


                    'For i = 0 To l_datatable_RetireeBank.Rows.Count - 1
                    '    If Trim(l_datatable_RetireeBank.Rows(i)(3) & "") = "N/A" Then 'if BankABA# field = "N/A" then
                    '        l_bool_ShowCheck = True
                    '        Exit For
                    '    Else
                    '        l_bool_ShowCheck = False
                    '    End If
                    'Next

                    'If l_bool_ShowCheck = False Then
                    '    l_datarows_OK = l_datatable_RetireeBank.Select("PaymentDesc='EFT' AND chrEFTStatus='O'")
                    '    If l_datarows_OK.Length = 0 Then
                    '        l_bool_ShowCheck = True
                    '    End If
                    'End If
                End If
            Else
                'Added By Ashutosh Patil as on 10-Jan-2007 For YREN-2979 
                'Start Ashutosh Patil
                'Commented By Ashutosh Patil as on 16-01-2007
                ''l_dataset_Retirees = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpRetireesStatus(l_string_PersId)
                ''If l_dataset_Retirees.Tables(0).Rows.Count = 0 Then
                ''    l_bool_ShowCheck = True
                ''ElseIf (l_dataset_Retirees.Tables(0).Rows.Count > 0 And (Not RTrim(l_dataset_Retirees.Tables(0).Rows(0)(9)).Equals("O"))) Then
                ''    l_bool_ShowCheck = True
                ''Else
                ''    l_bool_ShowCheck = False
                ''End If
                'End Ashutosh Patil
            End If

            If p_bool_Apply Then    'if apply is true then display status to the label shown on the banking tab
                If l_bool_ShowCheck = True Then
                    Me.LabelCurrentEFT.Text = "Current Pay Method : Check"
                    'Ashutosh Changed from visible to enabled
                    Me.ButtonBankingInfoCheckPayment.Enabled = False
                    If Not l_datarows Is Nothing Then
                        If l_datarows.Length > 0 Then
                            If l_datarows(0)("PaymentDesc").ToString.ToUpper() = "EFT" Then
                                Me.ButtonBankingInfoCheckPayment.Enabled = True
                            End If
                        End If
                    End If
                    'added by prasad on 10-08-2011 for YRS 5.0-1387 : Display IAT payment method on banking tab
                ElseIf l_bool_ShowIAT = True Then
                    Me.LabelCurrentEFT.Text = "Current Pay Method : IAT"
                    ButtonBankingInfoAdd.Enabled = False
                    Me.ButtonBankingInfoCheckPayment.Enabled = False
                Else
                    Me.LabelCurrentEFT.Text = "Current Pay Method : EFT"
                    Me.ButtonBankingInfoCheckPayment.Enabled = True
                End If
            End If

            BankInfoShowCheck = l_bool_ShowCheck
            'end - added by hafiz on 11-Dec-2006 for YREN-2977


            'Shashi shekhar Singh:13-Apr.2011:YRS 5.0-877 : Changes to Banking Information maintenance.

            If Not l_datarows Is Nothing Then
                If l_datarows.Length > 0 Then
                    If l_datarows(0)("ParticipantType").ToString.ToUpper() = "U" Then
                        Me.LabelCurrency.Text = "Currency: U.S."
                    ElseIf l_datarows(0)("ParticipantType").ToString.ToUpper() = "C" Then
                        Me.LabelCurrency.Text = "Currency: Canadian"
                    Else
                        Me.LabelCurrency.Text = ""
                    End If
                End If
            End If

            'end:YRS 5.0-877 : Changes to Banking Information maintenance.


        Catch
            Throw
        End Try

    End Function
    Public Sub LoadBanksTab()
        Try
            Dim i As Integer
            Dim bln_paymentTypeFlag As Boolean = False

            'added by hafiz on 11-Dec-2006 for YREN-2977
            Dim l_bool_ShowCheck As Boolean = False
            'Vipul 01Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session
            Dim l_string_PersId As String
            l_string_PersId = Session("PersId")
            Dim l_dataset_Banks As DataSet
            Dim dr As DataRow
            'Vipul 01Feb06 Cache-Session
            'l_dataset_Banks = cache("BankingDtls")
            l_dataset_Banks = Session("BankingDtls")
            'Vipul 01Feb06 Cache-Session

            If (Session("blnAddBankingRetirees") = True Or Session("blnUpdateBankingRetirees") = True) And Not Session("blnCancel") = True Then
                'Vipul 01Feb06 Cache-Session
                'l_dataset_Banks = cache("BankingDtls")
                l_dataset_Banks = Session("BankingDtls")
                'Vipul 01Feb06 Cache-Session
                If l_dataset_Banks Is Nothing Then
                    l_dataset_Banks = New DataSet
                    Dim dt As New DataTable
                    dt.Columns.Add("BankID")
                    dt.Columns.Add("BankName")
                    dt.Columns.Add("BankABA#")
                    dt.Columns.Add("AccountNo")
                    dt.Columns.Add("PaymentDesc")
                    dt.Columns.Add("EFTDesc")
                    dt.Columns.Add("EffecDate")
                    dt.Columns.Add("PersonID")

                    'added by hafiz on 10-Dec-2006 for YREN-2977
                    dt.Columns.Add("chrEFTStatus")
                    'Added by Ashutosh Patil as on 17-Jan-2007 for YREN-2979
                    dt.Columns.Add("dtmEffDate")

                    'Added by sanjay for Partcipant Type
                    dt.Columns.Add("ParticipantType")
                    l_dataset_Banks.Tables.Add(dt)
                End If
                If Not (Session("blnUpdateBankingRetirees") = True) Then
                    'Start:AA:10.27.2015 YRS-AT-2361 Changed the validation before adding bank into dataset
                    'If Session("SelectBank_BankName") <> "" Then
                    If IsBankValid() Then
                        'End:AA:10.27.2015 YRS-AT-2361 Changed the validation before adding bank into dataset
                        dr = l_dataset_Banks.Tables(0).NewRow
                        dr("BankName") = Session("SelectBank_BankName")
                        dr("BankABA#") = Session("SelectBank_BankABANumber")
                        dr("AccountNo") = Session("BankAccountNumber")
                        dr("EffecDate") = Session("BankEffectiveDate")
                        dr("PaymentDesc") = Session("BankPaymentMethod")
                        dr("EFTDesc") = Session("BankAccountType")
                        dr("PersonID") = Session("PersId")
                        dr("BankID") = Session("Sel_SelectBank_GuiUniqueiD")
                        dr("ParticipantType") = Session("ParticipantType") 'Added by sanjay for Fund Type
                        'added by hafiz on 10-Dec-2006 for YREN-2977
                        dr("chrEFTStatus") = String.Empty
                        dr("dtmEffDate") = Session("BankEffectiveDate")
                        dr("AccountType") = Session("BankAccountTypeText") 'Added by Dilip Yadav : YRS 5.0.896 : 22-Sep-2009
                        l_dataset_Banks.Tables(0).Rows.Add(dr)
                        'Code added by Ashutosh on 05-May-06***********
                        Session("SelectBank_BankName") = Nothing
                        Session("Sel_SelectBank_GuiUniqueiD") = Nothing
                        'Me.MultiPageRetireesInformation.SelectedIndex = 4
                        '   Me.TabStripRetireesInformation.SelectedIndex = 4
                        '***************************************
                        'start - added by hafiz on 11-Dec-2006 for YREN-2977
                        'l_bool_ShowCheck = BankInfoShowCheck(l_dataset_Banks, True)
                        'end - added by hafiz on 11-Dec-2006 for YREN-2977
                    End If
                End If

                'After adding the row to the dataset reset the session variable
                Session("blnAddBankingRetirees") = False
                ' Session("blnUpdateBankingRetirees") = False
            Else

                'If l_dataset_Banks Is Nothing Then
                If l_dataset_Banks Is Nothing Then
                    l_dataset_Banks = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpBanks(l_string_PersId)
                Else
                    If Not l_dataset_Banks.Tables(0).GetChanges Is Nothing Then
                        If Not l_dataset_Banks.Tables(0).GetChanges.Rows.Count > 0 Then
                            l_dataset_Banks = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpBanks(l_string_PersId)
                        End If
                    End If

                    If Session("blnCancel") = True Then
                        l_dataset_Banks = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpBanks(l_string_PersId)
                        ''Added by sanjay for Sorting
                        Dim BankDataView As DataView = l_dataset_Banks.Tables("Banks").DefaultView
                        BankDataView.Sort = "dtmEffDate DESC "
                        '''ends here 
                        DataGridBankInfoList.DataSource = BankDataView

                        DataGridBankInfoList.DataBind()
                        Session("blnCancel") = False
                        'dtValues.Select("chrEFTStatus='0'")
                        ''l_dataset_Banks.Tables(0).Select("EffecDate=MAX(EffecDate)")

                        ''If l_dataset_Banks.Tables(0).Rows(0).Item(9) = "V" Then
                        ''    Me.LabelCurrentEFT.Text = "Current Pay Method :CHECK"
                        ''End If
                    End If
                End If

                'Modifications for Update to Check Payment
                Dim drChange() As DataRow
                Dim tempCnt As Integer
                drChange = l_dataset_Banks.Tables(0).Select("PaymentDesc='CHECK' and AccountNo='N/A'")
                If drChange.Length > 0 Then
                    For tempCnt = 0 To drChange.Length - 1
                        drChange(tempCnt).Item("BankName") = "CHECK"
                        drChange(tempCnt).Item("BankABA#") = "N/A"
                    Next
                End If
                'l_dataset_Banks.AcceptChanges()

                'start - added by hafiz on 11-Dec-2006 for YREN-2977
                'Uncommented For Cancel=true Conition 
                l_bool_ShowCheck = BankInfoShowCheck(l_dataset_Banks, True)
                'end - added by hafiz on 11-Dec-2006 for YREN-2977
            End If

            If Page.IsPostBack = False Or Not l_dataset_Banks.Tables(0).GetChanges Is Nothing Or Session("blnUpdateBankingRetirees") = True Then

                ''Added by sanjay for sorting
                Dim BankDataView As DataView = l_dataset_Banks.Tables("Banks").DefaultView
                BankDataView.Sort = "dtmEffDate DESC "
                ''Ends here
                DataGridBankInfoList.DataSource = BankDataView
                DataGridBankInfoList.DataBind()
                Session("blnUpdateBankingRetirees") = False
                l_bool_ShowCheck = BankInfoShowCheck(l_dataset_Banks, True)
                ''Added by sanjay on 11/08/2009
                'If l_bool_ShowCheck = False Then
                '    ButtonBankingInfoCheckPayment.Enabled = True
                'Else
                '    ButtonBankingInfoCheckPayment.Enabled = False
                'End If
                ''ends here

                ' d1.DataSource = l_dataset_Banks
                ''d1.DataBind()

                'l_datarows_OK = l_dataset_Banks.Tables(0).Select("chrEFTStatus='O'")
                'If l_datarows_OK.Length = 0 Then
                '    l_bool_ShowCheck = True
                'End If

                ''l_dataset_Banks.Tables(0).Select("EffecDate=MAX(EffecDate)")
                ''If l_dataset_Banks.Tables(0).Rows(0).Item(9) = "V" Then
                ''    Me.LabelCurrentEFT.Text = "Current Pay Method :CHECK"
                ''End If
            End If

            'start - commented & modified by hafiz on 11-Dec-2006 for YREN-2977
            'For i = 0 To Me.DataGridBankInfoList.Items.Count - 1
            '    If Me.DataGridBankInfoList.Items(i).Cells(2).Text = "N/A" Then
            '        bln_paymentTypeFlag = True
            '        Exit For
            '    Else
            '        bln_paymentTypeFlag = False
            '    End If
            'Next
            'end - commented & modified by hafiz on 11-Dec-2006 for YREN-2977

            'commented & modified by hafiz on 11-Dec-2006 for YREN-2977
            'If bln_paymentTypeFlag = True Then
            'If bln_paymentTypeFlag = True Or l_bool_ShowCheck = True Then
            '    Me.LabelCurrentEFT.Text = "Current Pay Method : Check"
            '    'Ashutosh Changed from visible to enabled
            '    Me.ButtonBankingInfoCheckPayment.Enabled = False
            'Else
            '    Me.LabelCurrentEFT.Text = "Current Pay Method : EFT"
            'End If

            'Vipul 01Feb06 Cache-Session
            'cache.Add("BankingDtls", l_dataset_Banks)
            Session("BankingDtls") = l_dataset_Banks
            'Vipul 01Feb06 Cache-Session 
            Session("blnCancel") = False
        Catch
            Throw
        End Try
    End Sub

    Public Sub LoadFedWithDrawalTab()
        Try
            'Vipul 01Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session

            Dim dr As DataRow

            'LoadFedWithDrawalTab
            Dim l_string_PersId As String
            l_string_PersId = Session("PersId")
            Dim l_dataset_FedWith As DataSet

            'Vipul 01Feb06 Cache-Session
            'l_dataset_FedWith = cache("FedWithDrawals")
            l_dataset_FedWith = Session("FedWithDrawals")
            'Vipul 01Feb06 Cache-Session

            If Session("blnAddFedWithHoldings") = True Or Session("blnUpdateFedWithHoldings") = True And Not Session("blnCancel") = True Then

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
                'If l_dataset_FedWith Is Nothing Then
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

                'Else

                '    l_dataset_FedWith = cache("FedWithDrawals")
                'End If

            End If
            'If (l_dataset_FedWith.Tables(0).Rows.Count > 0) Then
            DataGridFederalWithholding.DataSource = l_dataset_FedWith
            DataGridFederalWithholding.DataBind()

            'End If

            Session("blnCancel") = False
            'Vipul 01Feb06 Cache-Session
            'cache.Add("FedWithDrawals", l_dataset_FedWith)
            Session("FedWithDrawals") = l_dataset_FedWith
            'Vipul 01Feb06 Cache-Session

            SetGrossAmountToSTWUserControl() ' ML |10.24.2019| YRS-AT-4598 | Assign GrossAmount to State Withholding Control

        Catch
            Throw
        End Try
    End Sub
 
    Public Sub LoadGenWithDrawalTab()
        Try
            'Vipul 01Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session
            Dim dr As DataRow
            Dim l_dataset_GenWith As DataSet


            Dim l_string_PersId As String
            l_string_PersId = Session("PersId")

            'Vipul 01Feb06 Cache-Session
            'l_dataset_GenWith = cache("GenWithDrawals")
            l_dataset_GenWith = Session("GenWithDrawals")
            'Vipul 01Feb06 Cache-Session

            If Session("blnAddGenWithHoldings") = True Or Session("blnUpdateGenWithDrawals") And Not Session("blnCancel") = True Then

                'Vipul 01Feb06 Cache-Session
                'l_dataset_GenWith = cache("GenWithDrawals")
                l_dataset_GenWith = Session("GenWithDrawals")
                'Vipul 01Feb06 Cache-Session

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
                If Not (Session("blnUpdateGenWithDrawals") = True) Then
                    dr = l_dataset_GenWith.Tables(0).NewRow
                    dr("Type") = Session("cmbWithHoldingType")
                    dr("Add'l Amount") = Session("txtAddAmount")
                    dr("Start Date") = Session("txtStartDate")
                    dr("End Date") = Session("txtEndDate")
                    dr("PersId") = l_string_PersId
                    'IB:on 07/06/2010 Require Guid
                    'dr("GenWithDrawalID") = ""
                    l_dataset_GenWith.Tables(0).Rows.Add(dr)
                End If

                'After adding the row to the dataset reset the session variable
                Session("blnAddGenWithHoldings") = False
                Session("blnUpdateGenWithDrawals") = False
            Else
                'If l_dataset_GenWith Is Nothing Then
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

                'l_dataset_GenWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpGenWithDrawals(l_string_PersId)
                'Else
                '    l_dataset_GenWith = cache("GenWithDrawals")
                'End If
            End If

            'If (l_dataset_GenWith.Tables(0).Rows.Count > 0) Then
            DataGridGeneralWithhold.DataSource = l_dataset_GenWith
            DataGridGeneralWithhold.DataBind()

            'End If

            'Vipul 01Feb06 Cache-Session
            'cache.Add("GenWithDrawals", l_dataset_GenWith)
            Session("GenWithDrawals") = l_dataset_GenWith
            'Vipul 01Feb06 Cache-Session
            Session("blnCancel") = False
            SetGrossAmountToSTWUserControl() ' ML |10.24.2019| YRS-AT-4598 | Assign GrossAmount to State Withholding Control
        Catch
            Throw
        End Try
    End Sub
    Public Sub LoadNotesTab()
        Dim l_str_notes As String

        Try
            'Vipul 01Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session
            Dim dr As DataRow
            Dim l_datatable_Notes As DataTable

            Dim l_string_PersId As String
            l_string_PersId = Session("PersId")

            Dim l_String_Search As String
            Dim l_datarow As DataRow()
            'Vipul 01Feb06 Cache-Session
            'l_datatable_Notes = cache("dtNotes")
            l_datatable_Notes = Session("dtNotes")
            'Vipul 01Feb06 Cache-Session

            'Start: Bala: 01/12/2016: This is handled in UpdateNotes.aspx page
            'If (Session("blnAddNotes") = True Or Session("blnUpdateNotes") = True) And Not Session("blnCancel") = True Then

            '    'Vipul 01Feb06 Cache-Session
            '    'l_datatable_Notes = cache("dtNotes")
            '    l_datatable_Notes = Session("dtNotes")
            '    'Vipul 01Feb06 Cache-Session

            '    If l_datatable_Notes Is Nothing Then
            '        l_datatable_Notes = New DataTable
            '        'By aparna -YREN-3115
            '        l_datatable_Notes.Columns.Add("UniqueID")
            '        l_datatable_Notes.Columns.Add("Note")
            '        l_datatable_Notes.Columns.Add("Creator")
            '        l_datatable_Notes.Columns.Add("Date")
            '        'By aparna -YREN-3115 08/03/2007 -New column
            '        l_datatable_Notes.Columns.Add("bitImportant")
            '    End If
            '    'If Not (Session("blnUpdateNotes") = True) Then
            '    'Commented And Modified By Ashutosh Patil as on 25-Apr-2007
            '    'For avoiding creation of new record in datagrid even when notes are not added 
            '    'Session("blnUpdateNotes") becomes True only when user selects notes from datagrid and if it update is permitted
            '    'if a new note is added then Session("Flag") becomes "AddNotes". 
            '    'once the notes are added or updated(if permitted) then Session("BitImportant") and Session("Note") are never empty. 
            '    'Validation can also be done using only Session("Note")

            '    If Session("Note") Is Nothing Then
            '        l_str_notes = String.Empty
            '    Else
            '        l_str_notes = Session("Note")
            '    End If
            '    'Priya 13-April-2010 Added if Session("Flag") =  "AddNotes" ::As per Hafiz Mail Send on:12April-2010 :Issues identified with 7.4.2 code release
            '    If Session("Flag") = "AddNotes" Then
            '        '    If Session("blnUpdateNotes") = True Or l_str_notes <> "" Then

            '        '        dr = l_datatable_Notes.NewRow
            '        '        'By aparna -YREN-3115
            '        '        dr("UniqueID") = Guid.NewGuid()
            '        '        dr("Note") = Session("Note")
            '        '        dr("PersonID") = l_string_PersId
            '        '        dr("Date") = Date.Now
            '        '        'By aparna -YREN-3115 08/03/2007
            '        '        dr("bitImportant") = Session("BitImportant")

            '        '        'Vipul - to fix the Creator Bug 04-Feb-06
            '        '        dr("Creator") = Session("LoginId")
            '        '        'Vipul - to fix the Creator Bug 04-Feb-06

            '        '        l_datatable_Notes.Rows.Add(dr)
            '        '    End If
            '    End If
            '    ''After adding the row to the dataset reset the session variable

            '    'Session("blnAddNotes") = False
            '    'Session("blnUpdateNotes") = False
            '    'Priya 13-April-2010 Assign Value  to  Session("Flag") = ""::As per Hafiz Mail Send on:12April-2010 :Issues identified with 7.4.2 code release
            '    Session("Flag") = ""
            'Else
            'If l_datatable_Notes Is Nothing Then

            If Session("Flag") = "AddNotes" Then
                Session("Flag") = ""
                l_datatable_Notes = YMCARET.YmcaBusinessObject.RefundRequest.MemberNotes(Session("PersID"))
            End If
            'End: Bala: 01/12/2016: This is handled in UpdateNotes.aspx page
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
            End If

            'l_datatable_Notes = YMCARET.YmcaBusinessObject.RefundRequest.MemberNotes(l_string_PersId)
            'Else
            '    l_datatable_Notes = cache("dtNotes")
            'End If
            'End If 'Bala: 01/12/2016: YRS-AT-1718: Add notes
            'Added by Swopna 16 Jan,2008 in response to YREN-4126 reopened
            '*************
            Dim dv As New DataView
            Dim SortExpressionNotes As String
            SortExpressionNotes = "Date DESC"
            dv = l_datatable_Notes.DefaultView
            dv.Sort = SortExpressionNotes
            'Shubhrata Feb 12th 2008 to handle sorting YRSPS 4614
            Session("Participant_Sort") = dv.Sort
            'Shubhrata Feb 12th 2008 to handle sorting YRSPS 4614
            '*************

            'START: VC | 2018.07.02 | YRS-AT-4010 | Commented existing code and Added code to hide the grid headers if it is empty.
            'If (l_datatable_Notes.Rows.Count > 0) Then
            '    NotesFlag.Value = "Notes"
            'Else
            '    NotesFlag.Value = "None"
            'End If
            ''If (l_datatable_Notes.Rows.Count > 0) Then
            'DataGridNotes.DataSource = l_datatable_Notes
            ''Shubhrata Feb 12th 2008 to handle sorting YRSPS 4614
            'Me.MakeDisplayNotesDataTable(l_datatable_Notes)
            ''Shubhrata Feb 12th 2008 to handle sorting YRSPS 4614

            'DataGridNotes.DataBind()
            ''End If
            ''By aparna -YREN-3115 

            NotesFlag.Value = "None"
            If (l_datatable_Notes.Rows.Count > 0) Then
                NotesFlag.Value = "Notes"
                DataGridNotes.DataSource = l_datatable_Notes
                Me.MakeDisplayNotesDataTable(l_datatable_Notes)
                DataGridNotes.DataBind()
            End If
            'END: VC | 2018.07.02 | YRS-AT-4010 | Commented existing code and Added code to hide the grid headers if it is empty.

            'If Me.NotesFlag.Value = "Notes" Then
            '    'by Aparna -YREN-3115 09/03/2007
            '    Me.TabStripRetireesInformation.Items(7).Text = "<font color=orange>Notes</font>"
            'ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
            '    Me.TabStripRetireesInformation.Items(7).Text = "<font color=red>Notes</font>"
            'Else
            '    Me.TabStripRetireesInformation.Items(7).Text = "Notes"
            'End If

            'check if the same user has marked anything as important 
            ' if yes then make notes tab as red
            'for the same user enable the checkbox in the datagrid


            'If Me.DataGridNotes.Items.Count > 0 Then
            '    If (l_datatable_Notes.Rows.Count > 0) Then
            '        'l_String_Search = " BitImportant = '" + True + "'"
            '        'l_datarow = l_datatable_Notes.Select(l_String_Search)
            '        'If l_datarow.Length > 0 Then
            '        '    NotesFlag.Value = "MarkedImportant"
            '        'End If
            '        For Each l_DataGridItem As DataGridItem In Me.DataGridNotes.Items
            '            Dim l_checkbox As New CheckBox
            '            l_checkbox = l_DataGridItem.FindControl("CheckBoxImportant")
            '            l_String_Search = " Creator = '" + l_DataGridItem.Cells(2).Text + "'"
            '            l_datarow = l_datatable_Notes.Select(l_String_Search)
            '            If l_datarow.Length > 0 Then
            '                If l_datarow(0)("Creator") = Session("LoginId") Then
            '                    If l_datarow(0)("bitImportant") = True Then
            '                        '  l_checkbox.Checked = True
            '                        '      Me.TabStripRetireesInformation.Items(7).Text = "<font color=red>Notes</font>"
            '                        NotesFlag.Value = "MarkedImportant"
            '                    End If
            '                    l_checkbox.Enabled = True
            '                End If
            '            End If
            '        Next
            '    End If
            'End If



            'Vipul 01Feb06 Cache-Session
            'cache.Add("dtNotes", l_datatable_Notes)
            Session("dtNotes") = l_datatable_Notes
            'Vipul 01Feb06 Cache-Session

            Session("blnCancel") = False
        Catch
            Throw
        End Try
    End Sub

    Public Sub LoadGeneraltab()
        Try
            'Shashi Shekhar:09- feb 2011: for YRS 5.0-1236 : Need ability to freeze/lock account
            'Make dynamic changes of control and label text according to saved value of Account Locking.
            GetLockUnlockReasonDetails()

            Dim g_dataset_dsAddressStateType As DataSet
            Dim g_dataset_dsAddressCountry As DataSet
            Dim InsertRowState As DataRow
            Dim InsertRowCountry As DataRow
            ''g_dataset_dsAddressStateType = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressStateType()
            'InsertRowState = g_dataset_dsAddressStateType.Tables(0).NewRow()
            'InsertRowState.Item("chvCodeValue") = String.Empty
            'InsertRowState.Item("chvShortDescription") = String.Empty
            ''If Not g_dataset_dsAddressStateType Is Nothing Then
            ''    g_dataset_dsAddressStateType.Tables(0).Rows.InsertAt(InsertRowState, 0)
            ''End If
            '***************Code Commented by ashutosh on 25-June-06*********
            'Me.DropdownlistState.DataSource = g_dataset_dsAddressStateType
            'Me.DropdownlistState.DataMember = "State Type"
            'Me.DropdownlistState.DataTextField = "chvShortDescription"
            'Me.DropdownlistState.DataValueField = "chvCodeValue"
            'Me.DropdownlistState.DataBind()
            'Me.DropdownlistState.Items.Insert(0, "-Select State-")
            'Me.DropdownlistState.Items(0).Value = ""

            'Me.DropdownlistSecState.DataSource = g_dataset_dsAddressStateType
            'Me.DropdownlistSecState.DataMember = "State Type"
            'Me.DropdownlistSecState.DataTextField = "chvShortDescription"
            'Me.DropdownlistSecState.DataValueField = "chvCodeValue"
            'Me.DropdownlistSecState.DataBind()
            'Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
            'Me.DropdownlistSecState.Items(0).Value = ""
            '***********End Ashutosh Code***********
            'g_dataset_dsAddressCountry = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressCountry()
            ''InsertRowCountry = g_dataset_dsAddressCountry.Tables(0).NewRow()
            ''InsertRowCountry.Item("chvAbbrev") = String.Empty
            ''InsertRowCountry.Item("chvDescription") = String.Empty

            ''If Not g_dataset_dsAddressCountry Is Nothing Then
            ''    g_dataset_dsAddressCountry.Tables(0).Rows.InsertAt(InsertRowCountry, 0)
            ''End If
            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'Me.DropdownlistCountry.DataSource = g_dataset_dsAddressCountry
            'Me.DropdownlistCountry.DataMember = "Country"
            'Me.DropdownlistCountry.DataTextField = "chvDescription"
            'Me.DropdownlistCountry.DataValueField = "chvAbbrev"
            'Me.DropdownlistCountry.DataBind()
            'Me.DropdownlistCountry.Items.Insert(0, "-Select Country-")
            'Me.DropdownlistCountry.Items(0).Value = ""

            'Me.DropdownlistSecCountry.DataSource = g_dataset_dsAddressCountry
            'Me.DropdownlistSecCountry.DataMember = "Country"
            'Me.DropdownlistSecCountry.DataTextField = "chvDescription"
            'Me.DropdownlistSecCountry.DataValueField = "chvAbbrev"
            'Me.DropdownlistSecCountry.DataBind()
            'Me.DropdownlistSecCountry.Items.Insert(0, "-Select Country-")
            'Me.DropdownlistSecCountry.Items(0).Value = ""
            'Me.TextBoxAddress1.Text = ""
            'Me.TextBoxAddress2.Text = ""
            'Me.TextBoxAddress3.Text = ""
            'Me.TextBoxCity.Text = ""
            'Me.TextBoxZip.Text = ""
            'BS:2012.03.09:-restructure Address Control
            'Me.TextBoxPriEffDate.Text = ""
            'Commented by Anudeep:14.03.2013 for telephone 
            'Me.TextboxTelephone.Text = ""
            'Me.TextBoxHome.Text = ""
            'Me.TextBoxMobile.Text = ""
            'Me.TextBoxFax.Text = ""

            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'Me.DropdownlistState.SelectedValue = ""
            'Me.DropdownlistCountry.SelectedValue = ""
            'Me.TextBoxSecAddress1.Text = ""
            'Me.TextBoxSecAddress2.Text = ""
            'Me.TextBoxSecAddress3.Text = ""
            'Me.TextBoxSecCity.Text = ""
            'Me.TextBoxSecZip.Text = ""
            'BS:2012.03.09:-restructure Address Control

            'Commented by Anudeep:14.03.2013 for telephone 
            'Me.TextBoxSecEffDate.Text = ""
            'Me.TextBoxSecTelephone.Text = ""
            'Me.TextBoxSecHome.Text = ""
            'Me.TextBoxSecMobile.Text = ""
            'Me.TextBoxSecFax.Text = ""

            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'Me.DropdownlistSecState.SelectedValue = ""
            'Me.DropdownlistSecCountry.SelectedValue = ""
            Me.TextBoxEmailId.Text = ""

            Dim l_string_PersId As String
            Dim l_string_FundId As String
            Dim l_dataset_AddressInfo As DataSet
            Dim dsAddress As DataSet
            ''Added by Anudeep:14.03.2013 for telephone usercontrol
            Dim l_datatable_ContactsInfo As DataTable
            Dim l_dataset_Poa As New DataSet

            Dim l_SuppressJSAnnuitiesCnt As Int32

            l_string_PersId = Session("PersId")
            l_string_FundId = Session("FundId")
            'Loading the data in General Tab of the Form
            'Loading the first Tab

            '''' code added by vartika on 18th Nov 2005
            Dim l_dataset_SecAddressInfo As DataSet
            ''Added by Anudeep:14.03.2013 for telephone usercontrol
            Dim l_datatable_SecContactInfo As DataTable
            Dim l_dataset_priEmailProps As DataSet
            Dim l_dataset_SecEmailProps As DataSet
            '''' 

            'Priya 05-April-2010 : YRS 5.0-1042:New "flag" value in Person/Retiree maintenance screen
            'Commented OldGuardNews check box code to remove it from screen
            'Dim integer_OldGuard As Integer
            'integer_OldGuard = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpOldGuardNews(l_string_PersId)
            'If integer_OldGuard = 0 Then
            '    Me.chkOldGuardNews.Checked = False
            'Else
            '    Me.chkOldGuardNews.Checked = True
            'End If
            'End YRS 5.0-1042

            g_dataset_GeneralInfo = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpGeneralInfo(l_string_PersId, l_string_FundId)

            'Ragesh  not to populate the general information if the datatable contains 0 reocrds.

            If g_dataset_GeneralInfo.Tables(0).Rows.Count = 0 Then

                Session("CurrentProcesstoConfirm") = "NoRecord_OnGeneral"
                'Aparna IE7 08/10/2007
                'START--Manthan Rajguru | 2015.10.30 | YRS-AT-2583 | Commented the exisiting code and changing it as per message provided in gemini
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Retiree Information does not exist for the current Fund Event, Please Verify.", MessageBoxButtons.OK)
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Legacy Account: please contact IT department for required information on this account (Ingres).", MessageBoxButtons.OK)
                'END--Manthan Rajguru | 2015.10.30 | YRS-AT-2583 | Commented the exisiting code and changing it as per message provided in gemini
                Exit Sub
            End If

            'Start: Bala: 01/05/2016: YRS-AT-1972: Initializing SpecialDeathProcess check box value.
            ViewState("InitialSpecialDeathProcessingRequiredBitFlag") = False
            Dim dsPersonConfiguration As DataSet
            dsPersonConfiguration = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetPersonMetaConfigurationDetails(Session("PersId").ToString(), "")
            If dsPersonConfiguration.Tables.Count > 0 Then
                If dsPersonConfiguration.Tables(0).Rows.Count > 0 Then
                    If Not String.IsNullOrEmpty(dsPersonConfiguration.Tables(0).Rows(0)("SpecialDeathProcess").ToString()) Then
                        If (Convert.ToBoolean(dsPersonConfiguration.Tables(0).Rows(0)("SpecialDeathProcess").ToString())) Then
                            ViewState("InitialSpecialDeathProcessingRequiredBitFlag") = True
                            chkSpecialDeathProcess.Checked = True
                        End If
                    End If
                End If
            End If
            'End: Bala: 01/05/2016: YRS-AT-1972: Initializing SpecialDeathProcess check box value.

            ViewState("g_dataset_GeneralInfo") = g_dataset_GeneralInfo
            If Not g_dataset_GeneralInfo Is Nothing Then

                'BS:2012.01.18:YRS 5.0-1497 -Add edit button to modify the date of death
                Dim l_strDBFundStatusType As String
                l_strDBFundStatusType = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundStatusType").ToString().Trim

                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsDeathDateUpdateBlock").ToString() = "0") Then

                    'btnEditDeathDateRet.Visible = True
                    'ButtonDeathNotification.Visible = False

                    'BS:2012.03.16:For BT-1015:YRS 5.0-1557:-here we have check fundevent status 
                    If (l_strDBFundStatusType = "DA" OrElse l_strDBFundStatusType = "DD" OrElse l_strDBFundStatusType = "DR" OrElse l_strDBFundStatusType = "DI" OrElse l_strDBFundStatusType = "DQ") Then

                        btnEditDeathDateRet.Visible = True
                        ButtonDeathNotification.Visible = False
                        'Start: Bala: 01/05/2016: Disabling special death process check box.
                        chkSpecialDeathProcess.Enabled = False
                        'End: Bala: 01/05/2016: Disabling special death process check box.
                    Else
                        btnEditDeathDateRet.Visible = False
                        ButtonDeathNotification.Visible = True
                    End If
                Else
                    btnEditDeathDateRet.Visible = False
                    ButtonDeathNotification.Visible = True
                End If


                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation").ToString() <> "") Then
                    'Commented by Swopna in response to Bug id 340
                    'Me.cboSalute.SelectedValue = CType(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation"), String).Trim()
                    'Added by Swopna in response to bug id 340 on 7 Jan 2008
                    '*****************
                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation").ToString().Trim().EndsWith(".") = True) Then
                        Me.cboSalute.SelectedValue = CType(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation"), String).Trim()
                    Else
                        Dim str As String
                        str = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation") + "."
                        Me.cboSalute.SelectedValue = str
                    End If
                    '*****************
                End If
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "") Then
                    Me.TextBoxGeneralFirstName.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName")
                End If

                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "") Then
                    Me.TextBoxGeneralLastName.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName")
                End If
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString() <> "") Then
                    Me.TextBoxGeneralMiddleName.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName")
                End If
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Suffix").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Suffix").ToString() <> "") Then
                    Me.TextBoxGeneralSuffix.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Suffix")
                End If
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("DOB").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("DOB").ToString() <> "") Then
                    Me.TextBoxGeneralDOB.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("DOB")
                    Session("DOB") = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("DOB")
                End If
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Deathdate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Deathdate").ToString() <> "") Then
                    Me.TextBoxGeneralDateDeceased.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Deathdate")
                End If
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("dtsretdate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("dtsretdate").ToString() <> "") Then
                    Me.TextBoxGeneralRetireDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("dtsretdate")
                End If

                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "") Then
                    Me.TextBoxGeneralSSNo.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo")
                    'SS:25 Feb 2011: For header change through user control :YRS 5.0-450 : Replace SSN with Fund Id on all screens.
                    HeaderControl.PageTitle = "Retiree Information"
                    HeaderControl.SSNo = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString.Trim


                End If
                'Vipul 01March06 YRS Enhancement#49 
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                    Me.TextBoxFundNo.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo")
                End If
                'Vipul 01March06 YRS Enhancement#49 

                'Start: Bala: YRS-AT-2398: Control and column name change
                ''START :Added by Dilip yadav : YRS 5.0.921
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Priority").ToString() = True) Then
                '    Me.checkboxPriority.Checked = True
                'Else
                '    Me.checkboxPriority.Checked = False
                'End If
                ''END :Added by Dilip yadav : YRS 5.0.921
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("ExhaustedDBSettle").ToString() = True) Then
                    Me.checkboxExhaustedDBSettle.Checked = True
                    Session("IsExhaustedDBRMDSettleRetiree") = True 'MMR | 2016.10.14 | YRS-AT-3062 | Setting property value
                Else
                    Me.checkboxExhaustedDBSettle.Checked = False
                End If
                'End: Bala: YRS-AT-2398: Control and column name change

                '2009.09.09 :Commented &  Modified by Dilip yadav : YRS 5.0.852
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "") Then
                '        Me.DropDownListGeneralGender.SelectedValue = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString().Trim()
                'End If



                'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
                '18-Feb-2010:Priya:YRS 5.0-988 : fetch value of newly aaded column bitShareInfoAllowed
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsShareInfoAllowed").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsShareInfoAllowed").ToString() <> "") Then
                '    chkShareInfoAllowed.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsShareInfoAllowed").ToString().Trim)
                'End If
                'End 18-Feb-2010
                'End 12-May-2010 YRS 5.0-1073


                'Priya 19-July-2010 Integrated into enhancement 10-June-2010:YRS 5.0-1107:YMCA Mailing Opt-Out should appear on the Retiree screen too. 
                'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
                '5-April-2010:Priya:YRS 5.0-1042- New "flag" value in Person/Retiree maintenance screen
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString() <> "") Then

                '  chkYMCAMailingOptOut.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString().Trim)

                'End If

                'End YRS 5.0-1042
                'bhavnaS 2011.07.08 YRS 5.0-1354 : change caption of Checkbox also replace item IsPersonalInfoSharingOptOut instead of IsYmcaMailOptOut
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString() <> "") Then
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsPersonalInfoSharingOptOut").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsPersonalInfoSharingOptOut").ToString() <> "") Then
                    'chkYMCAMailingOptOut.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString().Trim)
                    chkPersonalInfoSharingOptOut.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsPersonalInfoSharingOptOut").ToString().Trim)
                End If
                'End YRS 5.0-1354
                'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
                'End 19-July-2010 Integrated into enhancement 10-June-2010:YRS 5.0-1107:

                'bhavnaS 2011.07.11 YRS 5.0-1354 : change caption of Checkbox also replace item IsPersonalInfoSharingOptOut instead of IsYmcaMailOptOut
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsGoPaperless").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsGoPaperless").ToString() <> "") Then
                    chkGoPaperless.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsGoPaperless").ToString().Trim)
                End If
                'End YRS 5.0-1354
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "") Then
                '    Dim strGendertype As String
                '    strGendertype = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString().Trim()
                '    If (Me.DropDownListGeneralGender.Items.FindByValue(strGendertype) Is Nothing) Then
                '        Me.DropDownListGeneralGender.Items.Insert(0, strGendertype)
                '    End If
                '    Me.DropDownListGeneralGender.SelectedValue = strGendertype
                'Else '20-April-2010:Sanjay:YRS 5.0-1055 - for dbNull assigning Gendertype as UNKNOWN
                '    Dim strGendertype As String
                '    Try
                '        strGendertype = "U"
                '        If (Me.DropDownListGeneralGender.Items.FindByValue(strGendertype) Is Nothing) Then
                '            Me.DropDownListGeneralGender.Items.Insert(0, strGendertype)
                '        End If
                '        Me.DropDownListGeneralGender.SelectedValue = strGendertype
                '    Catch ex As Exception
                '        Me.DropDownListGeneralGender.SelectedValue = "U"
                '    End Try
                'End If
                'BS:2011.07.19:BT-852:YRS(5.0 - 1339) - handle 'X' value for non actuary data on marital status drpdwn
                If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
                    Dim strGendertype As String = "U"
                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).IsNull("Gender") = False And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> String.Empty) Then
                        strGendertype = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString().Trim()
                    End If
                    If (Me.DropDownListGeneralGender.Items.FindByValue(strGendertype) Is Nothing) Then
                        Me.DropDownListGeneralGender.Items.Insert(0, strGendertype)
                    End If
                    Me.DropDownListGeneralGender.SelectedValue = strGendertype
                Else
                    HelperFunctions.LogMessage("Error while initializing Gender code on General Information tab. Datatable GeneralInfo was found to be empty or not initialized.")
                End If
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "") Then
                '    Dim strGendertype As String
                '    strGendertype = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString().Trim()
                '    If (Me.DropDownListGeneralGender.Items.FindByValue(strGendertype) Is Nothing) Then
                '        Me.DropDownListGeneralGender.Items.Insert(0, strGendertype)
                '    End If
                '    Me.DropDownListGeneralGender.SelectedValue = strGendertype
                'Else
                '    Dim strGendertype As String
                '    Try
                '        strGendertype = "U"
                '        If (Me.DropDownListGeneralGender.Items.FindByValue(strGendertype) Is Nothing) Then
                '            Me.DropDownListGeneralGender.Items.Insert(0, strGendertype)
                '        End If
                '        Me.DropDownListGeneralGender.SelectedValue = strGendertype
                '    Catch ex As Exception
                '        Throw 'Me.DropDownListGeneralGender.SelectedValue = "U"
                '    End Try
                'End If
                'End YRS 5.0-1339


                '2009.09.09 : Modified by Dilip yadav : YRS 5.0.852

                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString() <> "") Then
                '    Try
                '        Me.cboGeneralMaritalStatus.SelectedValue = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString().Trim()
                '    Catch ex As Exception
                '        Me.cboGeneralMaritalStatus.SelectedValue = "U"
                '    End Try
                'Else '20-April-2010:Sanjay:YRS 5.0-1055 - for dbNull assigning Gendertype as UNKNOWN
                '    Try
                '        Me.cboGeneralMaritalStatus.SelectedValue = "U"
                '    Catch ex As Exception
                '        Me.cboGeneralMaritalStatus.SelectedValue = "U"
                '    End Try
                'End If

                'BS:2011.07.19:BT-852:YRS(5.0 - 1339) - handle 'X' value for non actuary data on marital status drpdwn
                If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
                    Dim strMaritalStatus As String = "U"
                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).IsNull("MaritalStatus") = False And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString() <> String.Empty) Then
                        strMaritalStatus = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString().Trim()
                    End If
                    If (Me.cboGeneralMaritalStatus.Items.FindByValue(strMaritalStatus) Is Nothing) Then
                        Me.cboGeneralMaritalStatus.Items.Insert(0, strMaritalStatus)
                    End If
                    Me.cboGeneralMaritalStatus.SelectedValue = strMaritalStatus
                Else
                    HelperFunctions.LogMessage("Error while initializing marital status on General Information tab. Datatable GeneralInfo was found to be empty or not initialized.")
                End If
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).IsNull("MaritalStatus") = False And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString() <> "") Then
                '    Dim strMaritalStatus As String
                '    strMaritalStatus = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString().Trim()
                '    If (Me.cboGeneralMaritalStatus.Items.FindByValue(strMaritalStatus) Is Nothing) Then
                '        Me.cboGeneralMaritalStatus.Items.Insert(0, strMaritalStatus)
                '    End If
                '    Me.cboGeneralMaritalStatus.SelectedValue = strMaritalStatus
                'Else
                '    Dim strMaritalStatus As String
                '    Try
                '        strMaritalStatus = "U"
                '        If (Me.cboGeneralMaritalStatus.Items.FindByValue(strMaritalStatus) Is Nothing) Then
                '            Me.cboGeneralMaritalStatus.Items.Insert(0, strMaritalStatus)
                '        End If
                '        Me.cboGeneralMaritalStatus.SelectedValue = strMaritalStatus
                '    Catch ex As Exception
                '        Throw 'Me.cboGeneralMaritalStatus.SelectedValue = "U"
                '    End Try
                'End If
                'End YRS 5.0-1339


                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType").ToString() <> "") Then
                '    Me.TextBoxQDROType.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType")
                'End If

                'Commented by Paramesh K. on Sept 25th 2008
                'As we are populating these information to the controls in the RefreshQdroInfo() method
                '******************************************
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "") Then
                '    If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() = "Pend" Then
                '        Me.chkGeneralQDROPending.Checked = True
                '    Else
                '        Me.chkGeneralQDROPending.Checked = False
                '    End If
                '    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "") Then
                '        Me.TextBoxGeneralQDROStatus.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus")
                '    End If
                '    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate").ToString() <> "") Then
                '        Me.TextBoxGeneralQDROStatudDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate")
                '    End If
                'End If
                '******************************************

                'Vipul 06Feb06 TextBoxGeneralRetireDate is already loaded in the previous statement
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("dtsretdate").ToString() <> "System.DBNull") Then
                '    Me.TextBoxGeneralRetireDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("dtsretdate")
                'End If
                'Vipul 06Feb06 TextBoxGeneralRetireDate is already loaded in the previous statement
            End If
            'Fetching The address Information
            l_dataset_AddressInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAddressInfo(l_string_PersId)
            'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
            ''Added by Anudeep:14.03.2013 for telephone usercontrol
            If l_dataset_AddressInfo.Tables.Count > 1 Then
                l_datatable_ContactsInfo = l_dataset_AddressInfo.Tables("TelephoneInfo")
            End If
            'Start:Anudeep01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            dsAddress = Address.GetAddressByEntity(l_string_PersId, EnumEntityCode.PERSON)

            Session("Dr_PrimaryAddress") = dsAddress.Tables("Address").Select("isPrimary = True")
            Session("Dr_SecondaryAddress") = dsAddress.Tables("Address").Select("isPrimary = False")
            'Session("Dt_EmailAddress") = dsAddress.Tables("EmailInfo")
            Session("Dt_EmailAddress") = l_dataset_AddressInfo.Tables("EmailInfo")
            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            Me.AddressWebUserControl1.guiEntityId = l_string_PersId
            Me.AddressWebUserControl1.IsPrimary = 1
            Me.AddressWebUserControl2.guiEntityId = l_string_PersId
            Me.AddressWebUserControl2.IsPrimary = 0
            'Ashutosh Patil as on 25-Apr-2007
            'YREN-3298
            Me.AddressWebUserControl1.IsMaintenanceScreen = True
            'BS:2012:04.11:-Address Restructure
            'Start:Anudeep01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            AddressWebUserControl1.LoadAddressDetail(dsAddress.Tables("Address").Select("isPrimary = True"))
            AddressWebUserControl2.LoadAddressDetail(dsAddress.Tables("Address").Select("isPrimary = False"))
            'Me.AddressWebUserControl1.SetValidationsForPrimary()
            AddressWebUserControl1.Rights = "ButtonGeneralAddress"
            'Inserting contact details in UserControl
            Session("dt_PrimaryContactInfo") = l_datatable_ContactsInfo
            'Added by Anudeep:14.03.2013 for telephone usercontrol
            TelephoneWebUserControl1.RefreshTelephoneDetail(l_datatable_ContactsInfo)
            Me.TelephoneWebUserControl1.Visible = True
            btnEditPrimaryContact.Enabled = True
            btnEditPrimaryContact.Visible = True
            AddressWebUserControl1.EnableControls = True
            'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
            'RefreshPrimaryTelephoneDetail(l_dataset_AddressInfo)
            Me.CheckboxBadEmail.Enabled = False
            Me.CheckboxUnsubscribe.Enabled = False
            Me.CheckboxTextOnly.Enabled = False
            Me.TextBoxEmailId.Enabled = False
            Me.LabelEmailId.Enabled = False
            RefreshEmailDetail(l_dataset_AddressInfo)
            '        If l_dataset_AddressInfo.Tables("AddressInfo").Rows.Count > 0 Then
            'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString() <> "") Then
            '	'Me.TextBoxAddress1.Text = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString().Trim
            'End If
            'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString() <> "") Then
            '	'Me.TextBoxAddress2.Text = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString().Trim
            'End If
            'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString() <> "") Then
            '	'Me.TextBoxAddress3.Text = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString().Trim
            'End If
            'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString() <> "") Then
            '	'Me.TextBoxCity.Text = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString().Trim
            'End If
            'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString() <> "") Then
            '	Try
            '		'ViewState("SelectedState") = Trim(IIf(IsDBNull(l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State")), "", l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State")))
            '		'Me.DropdownlistState.SelectedValue = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim
            '		'By Ashutosh Patil as on 22-Feb-2007
            '		'YREN-3079
            '		If IsDBNull(l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State")) = True Or l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State") = "" Or l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State") = "  " Then
            '			ViewState("SelectedState") = Nothing
            '		Else
            '			ViewState("SelectedState") = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("State")
            '		End If
            '	Catch ex As Exception
            '		'Me.DropdownlistState.SelectedValue = ""
            '	End Try
            'End If
            'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString() <> "") Then
            '	'Me.TextBoxZip.Text = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString().Trim
            'End If
            ''***************Code Commented by ashutosh on 25-June-06*********uncommented by hafiz on 12Oct2006
            ''Ashutosh Patil as on 26-Mar-2007
            ''YREN - 3028,YREN-3029
            ''Address_DropDownListStatePrimary()
            'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString().Trim <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString().Trim <> "") Then
            '	Try
            '		'Me.DropdownlistCountry.SelectedValue = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString.Trim
            '	Catch ex As Exception
            '		'Me.DropdownlistCountry.SelectedValue = ""
            '	End Try
            'End If
            ''****End Ashu*********
            'If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "") Then
            '	Dim l_date As Date

            '	Me.m_str_Pri_EffDate = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("EffDate")
            'End If
            '            If (l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "") Then
            '                If Convert.ToBoolean(l_dataset_AddressInfo.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) = True Then
            '                    Me.CheckboxIsBadAddress.Checked = True
            '                Else
            '                    CheckboxIsBadAddress.Checked = False
            '                End If
            '            End If

            '        End If
            '        If l_dataset_AddressInfo.Tables("EmailInfo").Rows.Count > 0 Then
            '            If Not l_dataset_AddressInfo.Tables("EmailInfo") Is Nothing Then
            '                If (l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("EmailAddress").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("EmailAddress").ToString() <> "") Then
            '                    Me.TextBoxEmailId.Text = l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("EmailAddress").ToString().Trim
            '                    Me.EmailId.Value = Me.TextBoxEmailId.Text
            '                End If
            '                If (l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("IsBadAddress").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("IsBadAddress").ToString() <> "") Then
            '                    If Convert.ToBoolean(l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("IsBadAddress")) = True Then
            '                        CheckboxBadEmail.Checked = True
            '                        Me.BadEmail.Value = True
            '                    Else
            '                        CheckboxBadEmail.Checked = False
            '                        Me.BadEmail.Value = False
            '                    End If
            '                End If

            '                If (l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed").ToString() <> "") Then
            '                    If Convert.ToBoolean(l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed")) = True Then
            '                        CheckboxUnsubscribe.Checked = True
            '                        Me.Unsubscribe.Value = True
            '                    Else
            '                        CheckboxUnsubscribe.Checked = False
            '                        Me.Unsubscribe.Value = False
            '                    End If
            '                End If

            '                If (l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("IsTextOnly").ToString() <> "System.DBNull" And l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("IsTextOnly").ToString() <> "") Then
            '                    If Convert.ToBoolean(l_dataset_AddressInfo.Tables("EmailInfo").Rows(0).Item("IsTextOnly")) = True Then
            '                        CheckboxTextOnly.Checked = True
            '                        Me.TextOnly.Value = True
            '                    Else
            '                        CheckboxTextOnly.Checked = False
            '                        Me.TextOnly.Value = False
            '                    End If
            '                End If
            '            End If
            '        End If
            '        Dim l_datarow As DataRow

            '        If l_dataset_AddressInfo.Tables("TelephoneInfo").Rows.Count > 0 Then
            '            If Not l_dataset_AddressInfo.Tables("TelephoneInfo") Is Nothing Then
            '                For Each l_datarow In l_dataset_AddressInfo.Tables("TelephoneInfo").Rows
            '                    If (l_datarow("PhoneType").ToString() <> "System.DBNull" And l_datarow("PhoneType").ToString() <> "") Then
            '                        If l_datarow("PhoneType").ToString.ToUpper.Trim() = "HOME" Then
            '                            TextBoxHome.Text = l_datarow("PhoneNumber").ToString().Trim
            '                        End If
            '                        If l_datarow("PhoneType").ToString.ToUpper.Trim() = "OFFICE" Then
            '                            Me.TextboxTelephone.Text = l_datarow("PhoneNumber").ToString().Trim
            '                        End If
            '                        If l_datarow("PhoneType").ToString.ToUpper.Trim() = "FAX" Then
            '                            Me.TextBoxFax.Text = l_datarow("PhoneNumber").ToString().Trim
            '                        End If
            '                        If l_datarow("PhoneType").ToString.ToUpper.Trim() = "MOBILE" Then
            '                            Me.TextBoxMobile.Text = l_datarow("PhoneNumber").ToString().Trim
            '                        End If
            '                    End If
            '                Next
            '            End If
            '        End If

            'l_dataset_Poa = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpPOAInfo(l_string_PersId)
            'If l_dataset_Poa.Tables("POAInfo").Rows.Count > 0 Then
            '    If Not (IsDBNull(l_dataset_Poa.Tables("POAInfo").Rows(0).Item("Name1"))) Then
            '        TextBoxGeneralPOA.Text = l_dataset_Poa.Tables("POAInfo").Rows(0).Item("Name1").ToString().Trim
            '    End If
            'End If
            LoadPoADetails()


            '''' code added by vartika on 18th Nov 2005
            '' Fetching the secondary address information
            l_dataset_SecAddressInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpSecondaryAddress(l_string_PersId)

            'BS:2012:04.11:-Address Restructure
            If HelperFunctions.isEmpty(l_dataset_SecAddressInfo) Then
                Me.ButtonActivateAsPrimary.Enabled = False
                'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Disabling deactivate button
                Me.btnDeactivateSecondaryAddrs.Enabled = False
                'End - Manthan | 2016.02.24 | YRS-AT-2328 | Disabling deactivate button
            Else
                Me.ButtonActivateAsPrimary.Enabled = True
                'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Enabling deactivate button
                Me.btnDeactivateSecondaryAddrs.Enabled = True
                'End - Manthan | 2016.02.24 | YRS-AT-2328 | Enabling deactivate button
            End If

            'BS:2012:04.11:-Address Restructure

            'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
            AddressWebUserControl2.Rights = "ButtonGeneralAddress"
            'Added by Anudeep:14.03.2013 for telephone usercontrol
            If l_dataset_SecAddressInfo.Tables.Count > 1 Then
                l_datatable_SecContactInfo = l_dataset_SecAddressInfo.Tables("TelephoneInfo")
            End If
            Session("dt_SecondaryContactInfo") = l_datatable_SecContactInfo
            TelephoneWebUserControl2.RefreshTelephoneDetail(l_datatable_SecContactInfo)
            Me.TelephoneWebUserControl2.Visible = True
            'RefreshSecTelephoneDetail(l_dataset_SecAddressInfo)
            AddressWebUserControl2.EnableControls = True
            btnEditSecondaryContact.Enabled = True
            btnEditSecondaryContact.Visible = True
            'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
            ''Ashutosh Patil as on 26-Mar-2007
            ''YREN - 3028,YREN-3029
            'If Not l_dataset_SecAddressInfo Is Nothing Then
            '	If Not l_dataset_SecAddressInfo.Tables.Count = 0 Then
            '		If l_dataset_SecAddressInfo.Tables("AddressInfo").Rows.Count > 0 Then
            '			If l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString <> "System.DBNull" And l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString <> "" Then
            '				'Me.TextBoxSecAddress1.Text = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Address1").ToString().Trim
            '			End If
            '			If l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString <> "System.DBNull" And l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString <> "" Then
            '				'Me.TextBoxSecAddress2.Text = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Address2").ToString().Trim
            '			End If
            '			If l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString <> "System.DBNull" And l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString <> "" Then
            '				'Me.TextBoxSecAddress3.Text = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Address3").ToString().Trim
            '			End If
            '			If l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString <> "System.DBNull" And l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString <> "" Then
            '				'Me.TextBoxSecCity.Text = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("City").ToString().Trim
            '			End If
            '			If l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString <> "System.DBNull" And l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString <> "" Then
            '				Try
            '					'***************Code Added by ashutosh on 25-June-06*********
            '					'ViewState("SelectedSecState") = Trim(IIf(IsDBNull(l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("State")), "", l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("State")))
            '					'Me.DropdownlistSecState.SelectedValue = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("State").ToString().Trim
            '					'Commented And Added By Ashutosh Patil as on 23-Feb-07
            '					'YREN-3079
            '					If IsDBNull(l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("State")) = True Or l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("State") = "" Or l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("State") = "  " Then
            '						ViewState("SelectedSecState") = Nothing
            '					Else
            '						ViewState("SelectedSecState") = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("State")
            '					End If
            '					'*********End Ashu Code***********
            '				Catch ex As Exception
            '					'Me.DropdownlistSecState.SelectedValue = ""
            '				End Try
            '			End If

            '			'Commented By Ashutosh Patil as on 31-Jan-2007 FOR YREN -3028,3029
            '			'Address_DropDownListStateSecondary()
            '			'Added Ashutosh Patil as on 14-Feb-2007 FOR YREN -3028,3029
            '			'Ashutosh Patil as on 26-Mar-2007
            '			'YREN - 3028,YREN-3029
            '			If Not ViewState("SelectedSecState") Is Nothing And Page.IsPostBack = True Then
            '				'Address_DropDownListStateSecondary()
            '			End If

            '			If l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString <> "System.DBNull" And l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString <> "" Then
            '				'Me.TextBoxSecZip.Text = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Zip").ToString().Trim
            '			End If
            '			'***************Code Commented by ashutosh on 25-June-06*********uncommented by hafiz on 12Oct2006
            '			If l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString <> "System.DBNull" And l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString <> "" Then
            '				Try
            '					'Me.DropdownlistSecCountry.SelectedValue = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("Country").ToString().Trim
            '				Catch ex As Exception
            '					'Me.DropdownlistSecCountry.SelectedValue = ""
            '				End Try
            '			End If
            '			'*********End Ashu Code***********
            '			If Not l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("EffDate") Is Nothing Then
            '				If (l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "System.DBNull" And l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "") Then


            '					m_str_Sec_EffDate = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("EffDate")
            '				End If
            '			End If
            '			If (l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "System.DBNull" And l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "") Then
            '				If Convert.ToBoolean(l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) = True Then
            '					Me.CheckboxSecIsBadAddress.Checked = True
            '				Else
            '					Me.CheckboxSecIsBadAddress.Checked = False
            '				End If
            '			End If

            '		End If
            '	End If
            '	'Address_DropDownListState() 'For First time filling of States Ashutosh
            'End If

            '	'Added By Ashutosh Patil as on 13-Feb-07 for YREN-3028,YREN-3029
            '	'Start Ashutosh Patil
            '	'Ashutosh Patil as on 26-Mar-2007
            '	'YREN - 3028,YREN-3029
            '	If Page.IsPostBack = False Then
            '		'Address_DropDownListStateSecondary()
            '	End If
            '	'End Ashutosh Patil

            '	Dim l_secdatarow As DataRow

            '	If l_dataset_SecAddressInfo.Tables("TelephoneInfo").Rows.Count > 0 Then
            '		If Not l_dataset_SecAddressInfo.Tables("TelephoneInfo") Is Nothing Then
            '			For Each l_secdatarow In l_dataset_SecAddressInfo.Tables("TelephoneInfo").Rows
            '				If (l_secdatarow("PhoneType").ToString() <> "System.DBNull" And l_secdatarow("PhoneType").ToString() <> "") Then
            '					If l_secdatarow("PhoneType").ToString.ToUpper.Trim() = "HOME" Then
            '						TextBoxSecHome.Text = l_secdatarow("PhoneNumber").ToString().Trim
            '					End If
            '					If l_secdatarow("PhoneType").ToString.ToUpper.Trim() = "OFFICE" Then
            '						Me.TextBoxSecTelephone.Text = l_secdatarow("PhoneNumber").ToString().Trim
            '					End If
            '					If l_secdatarow("PhoneType").ToString.ToUpper.Trim() = "FAX" Then
            '						Me.TextBoxSecFax.Text = l_secdatarow("PhoneNumber").ToString().Trim
            '					End If
            '					If l_secdatarow("PhoneType").ToString.ToUpper.Trim() = "MOBILE" Then
            '						Me.TextBoxSecMobile.Text = l_secdatarow("PhoneNumber").ToString().Trim
            '					End If
            '				End If
            '			Next
            '		End If
            '	End If


            'NP:BT-550:2008.09.10 - Adding code to set Person_Info to just the SSN
            Session("Person_Info") = Me.TextBoxGeneralSSNo.Text

            '---------------------------------------------------------------------------------------------------
            'Shashi Shekhar:2009.08.08 Ref:(PS:YMCA PS Data Archive.Doc) - Adding code to change general header  and Annuity Paid header in Annuity Paid Tab in case if person's data has been archived
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsArchived").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsArchived").ToString() <> "") Then
                g_isArchived = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsArchived").ToString().Trim)
            End If

            If g_isArchived = True Then
                Session("IsDataArchived") = "Yes"
                tdRetrieveData.Visible = True
                tblRetrieveData.Visible = True

                ''-----------------------------------------------------------------------------------------------------------------
                ''Shashi Shekhar: 26-Oct-2010: For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                '    LabelHdr.Text = Me.TextBoxGeneralLastName.Text + ", " + Me.TextBoxGeneralFirstName.Text + ", Fund No: " + g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim + " " + "(Archived)"
                'Else
                '    LabelHdr.Text = Me.TextBoxGeneralLastName.Text + ", " + Me.TextBoxGeneralFirstName.Text + " " + " (Archived)"
                'End If
                ''------------------------------------------------------------------------------------------------------------------
                LabelGenHdr.Text = " - Transaction & Disbursement data have been archived."
                LabelAnnuitiesHdr.Text = " - Transaction & Disbursement data have been archived."
            Else
                Session("IsDataArchived") = "No"
                tdRetrieveData.Visible = False
                tblRetrieveData.Visible = False
                ''-----------------------------------------------------------------------------------------------------------------
                ''Shashi Shekhar: 26-Oct-2010: For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                '    LabelHdr.Text = Me.TextBoxGeneralLastName.Text + ", " + Me.TextBoxGeneralFirstName.Text + ", Fund No: " + g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim
                'Else
                '    LabelHdr.Text = Me.TextBoxGeneralLastName.Text + ", " + Me.TextBoxGeneralFirstName.Text
                'End If
                ''----------------------------------------------------------------------------------------------------------------

                LabelGenHdr.Text = ""
                LabelAnnuitiesHdr.Text = ""
            End If

            '----------------------------------------------------------------------------------------------------

            'Start: Bala: YRS-AT-2398: Control name change
            ''START Below lines are Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921
            ''Reverted changes by Anudeep on 2012.11.14 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen Changed from 'Proirity Handling' to 'Exhausted DB settlement efforts' 
            'If (checkboxPriority.Checked = True) Then
            '    LabelPriorityHdr.Visible = True
            'Else
            '    LabelPriorityHdr.Visible = False
            'End If

            ''END Above lines are Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921
            If (checkboxExhaustedDBSettle.Checked = True) Then
                LabelExhaustedDBSettleHdr.Visible = True
            Else
                LabelExhaustedDBSettleHdr.Visible = False
            End If
            'End: Bala: YRS-AT-2398: Control name change

            'NP:BT-550:2008.09.10 - Commenting this since the value going into Person_Info is the full header instead of just the SSN
            'Session("Person_Info") = LabelHdr.Text

            ' 34231 Ragesh Added to Spurress JS annuities.
            getSuppressJSAnnuityCount()

            'SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-Start
            CreateTempSSNDatatableForUpdate(TextBoxGeneralSSNo.Text.Trim())
            'SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-End

            If Not String.Empty.IsNullOrEmpty(g_dataset_GeneralInfo.Tables(0).Rows(0)("WebLockOut").ToString().Trim) Then
                chkNoWebAcctCreate.Checked = g_dataset_GeneralInfo.Tables(0).Rows(0)("WebLockOut").ToString().Trim()
            Else
                chkNoWebAcctCreate.Checked = False
            End If

            'Start: Bala: 01/19/2019: YRS-AT-2398: Alert Message.
            If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo.Tables("SpecialHandlingDetails")) Then 'Bala: 03.16.2016: Check if the table has no records.
                If (g_dataset_GeneralInfo.Tables("SpecialHandlingDetails").Rows(0).Item("RequireSpecialHandling").ToString() = True) Then
                    Me.LabelSpecialHandling.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PART_MAINT_SPECIAL_HANDLING).DisplayText
                    Me.LabelSpecialHandling.Visible = True
                    Me.HiddenFieldOfficerDetails.Value = g_dataset_GeneralInfo.Tables("SpecialHandlingDetails").Rows(0).Item("EmploymentDetail").ToString()
                End If
            End If
            'End: Bala: 01/19/2019: YRS-AT-2398: Alert Message.
        Catch
            Throw
        End Try
    End Sub

    Function getSuppressJSAnnuityCount()
        Dim l_string_PersId As String
        Dim l_SuppressJSAnnuitiesCnt As Int32
        Try
            l_string_PersId = Session("PersId")
            ' 34231 Ragesh Added to Spurress JS annuities.
            l_SuppressJSAnnuitiesCnt = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.SuppressedJSAnnuitiesCount(l_string_PersId)

            If l_SuppressJSAnnuitiesCnt > 0 Then
                Me.ButtonSuppressedJSAnnuities.Visible = True
                Me.tblSuppress.Visible = True
                Me.TextboxSuppressAnnuityCount.Visible = True
                Me.LabelSuppressedJSAnnuities.Visible = True
                Me.TextboxSuppressAnnuityCount.Text = l_SuppressJSAnnuitiesCnt

                LoadDataInUnSuppressGrid() 'PPP | 04/20/2016 | YRS-AT-2719 | Loading data in Unsuppress annuity popup 
            Else
                Me.TextboxSuppressAnnuityCount.Visible = False
                Me.ButtonSuppressedJSAnnuities.Visible = False
                Me.tblSuppress.Visible = False
                Me.LabelSuppressedJSAnnuities.Visible = False
                Me.TextboxSuppressAnnuityCount.Text = l_SuppressJSAnnuitiesCnt
            End If
        Catch
            Throw
        End Try
    End Function

    Public Function UpadateGeneralInfo() As String
        Try
            Dim l_string_Output As String
            Dim l_string_MaritalStatus As String
            Dim l_string_Gender As String
            'Start: Bala: 01/19/2019: YRS-AT-2398 Variable name change
            ''Start : Added By Dilip Yadav : 2009.10.28
            'Dim l_bool_Priority As Boolean
            'If (checkboxPriority.Checked = True) Then
            '    l_bool_Priority = True
            'Else
            '    l_bool_Priority = False
            'End If
            ''End : Added By Dilip Yadav : 2009.10.28
            Dim l_bool_ExhaustedDBSettle As Boolean
            If (checkboxExhaustedDBSettle.Checked = True) Then
                l_bool_ExhaustedDBSettle = True
            Else
                l_bool_ExhaustedDBSettle = False
            End If
            'End: Bala: 01/19/2019: YRS-AT-2398 Variable name change

            'Start : Commented and added by Dilip yadav on 2009.09.08 for YRS 5.0-852
            'If Me.DropDownListGeneralGender.SelectedValue = "Female" Then
            '    l_string_Gender = "F"
            'ElseIf Me.DropDownListGeneralGender.SelectedValue = "Male" Then
            '    l_string_Gender = "M"
            'ElseIf Me.DropDownListGeneralGender.SelectedValue = "Unknown" Then
            '    l_string_Gender = "U"
            'Else
            '    l_string_Gender = ""
            'End If
            l_string_Gender = Me.DropDownListGeneralGender.SelectedValue
            'End : Commented & added by Dilip yadav on 2009.09.08 for YRS 5.0-852



            'commented by hafiz on 2-May-2007 for YREN-3112
            'If Me.cboGeneralMaritalStatus.SelectedValue = "Married" Then
            '    l_string_MaritalStatus = "M"
            'ElseIf Me.cboGeneralMaritalStatus.SelectedValue = "Single" Then
            '    l_string_MaritalStatus = "S"
            'ElseIf Me.cboGeneralMaritalStatus.SelectedValue = "Unknown" Then
            '    l_string_MaritalStatus = "U"
            'Else
            '    l_string_MaritalStatus = ""
            'End If
            'commented by hafiz on 2-May-2007 for YREN-3112

            'added by hafiz on 2-May-2007 for YREN-3112
            l_string_MaritalStatus = Me.cboGeneralMaritalStatus.SelectedValue
            'added by hafiz on 2-May-2007 for YREN-3112

            'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
            'Priya : Added on 19/2/2010 :YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
            'Dim bool_AllowShareInfo As Boolean
            'bool_AllowShareInfo = chkShareInfoAllowed.Checked
            'End 19/2/2010

            'Priya              19-July-2010    Integrated into enhancement 10-June-2010:YRS 5.0-1107:YMCA Mailing Opt-Out should appear on the Retiree screen too. 
            ''5-April-2010:Priya:YRS 5.0-1042 : fetch value of newly aaded column bitYmcaMailOptOut
            'Dim bool_YMCAMailingOptOut As Boolean
            '         bool_YMCAMailingOptOut = chkYMCAMailingOptOut.Checked()
            'bhavnaS 2011.07.08 :YRS 5.0-1354  : Change caption
            Dim bool_PersonalInfoSharingOptOut As Boolean
            bool_PersonalInfoSharingOptOut = chkPersonalInfoSharingOptOut.Checked()

            'bhavnaS 2011.07.11 YRS 5.0-1354 : add new bit field bitGoPaperless
            Dim bool_GoPaperless As Boolean
            bool_GoPaperless = chkGoPaperless.Checked()


            'g_dataset_GeneralInfo = viewstate("g_dataset_GeneralInfo")
            'If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
            '    bool_YMCAMailingOptOut = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString().Trim)
            'End If
            'End YRS 5.0-988
            'End 12-May-2010 YRS 5.0-1073
            'End 19-July-2010 Integrated into enhancement 10-June-2010:YRS 5.0-1107:YMCA Mailing Opt-Out should appear on the Retiree screen too. 

            'Commented and added by dilip yadav : 29-Oct-09 : YRS 5.0.921
            'l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateGeneralInfo(Session("PersId"), TextBoxGeneralSSNo.Text, TextBoxGeneralLastName.Text, TextBoxGeneralFirstName.Text, TextBoxGeneralMiddleName.Text, cboSalute.SelectedValue, TextBoxGeneralSuffix.Text, l_string_Gender, l_string_MaritalStatus, TextBoxGeneralDOB.Text, "")


            'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
            'Priya              18-Feb-2010     YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
            'l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateGeneralInfo(Session("PersId"), TextBoxGeneralSSNo.Text, TextBoxGeneralLastName.Text, TextBoxGeneralFirstName.Text, TextBoxGeneralMiddleName.Text, cboSalute.SelectedValue, TextBoxGeneralSuffix.Text, l_string_Gender, l_string_MaritalStatus, TextBoxGeneralDOB.Text, "", l_bool_Priority, False, bool_YMCAMailingOptOut)
            'bhavnaS 2011.07.08 YRS 5.0-1354 : change bool parameter instead bool_PersonalInfoSharingOptOut of bool_YMCAMailingOptOut
            'bhavnaS 2011.07.11 YRS 5.0-1354 : Add new bit field bool_GoPaperless into atsperss
            'BS:2012.01.18:YRS 5.0-1497 -Add edit button to modify the date of death
            'prasad	2012.01.19	YRS 5.0-1400 : bitMrd parameter passed to function
            'AA:2014.02.13:BT:2316 :YRS 5.0-2262 - Added for passing cannotlocatespouse text value to pass as parameter to sp
            'AA:2014.02.16:BT:2291 :YRS 5.0-2247 - Added for passing WebLockOut text value to pass as parameter to sp
            'Start: Bala: 01/19/2016: YRS-AT-2398: Variable name change.
            'l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateGeneralInfo(Session("PersId"), TextBoxGeneralSSNo.Text, TextBoxGeneralLastName.Text, TextBoxGeneralFirstName.Text, TextBoxGeneralMiddleName.Text, cboSalute.SelectedValue, TextBoxGeneralSuffix.Text, l_string_Gender, l_string_MaritalStatus, TextBoxGeneralDOB.Text, "", l_bool_Priority, False, bool_PersonalInfoSharingOptOut, bool_GoPaperless, TextBoxGeneralDateDeceased.Text, g_dataset_GeneralInfo.Tables(0).Rows(0).Item("MRDRequested"), "", chkNoWebAcctCreate.Checked.ToString().Trim(), "RetireeInfo")
            l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateGeneralInfo(Session("PersId"), TextBoxGeneralSSNo.Text, TextBoxGeneralLastName.Text, TextBoxGeneralFirstName.Text, TextBoxGeneralMiddleName.Text, cboSalute.SelectedValue, TextBoxGeneralSuffix.Text, l_string_Gender, l_string_MaritalStatus, TextBoxGeneralDOB.Text, "", l_bool_ExhaustedDBSettle, False, bool_PersonalInfoSharingOptOut, bool_GoPaperless, TextBoxGeneralDateDeceased.Text, g_dataset_GeneralInfo.Tables(0).Rows(0).Item("MRDRequested"), "", chkNoWebAcctCreate.Checked.ToString().Trim(), "RetireeInfo") 'Bala: 01/19/2019: YRS-AT-2398: Varible name change.
            'End: Bala: 01/19/2016: YRS-AT-2398: Variable name change.
            'SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-Start
            If (IsSSNEdited()) Then
                Dim dtTempTable As DataTable = CType(Session("SessionTempSSNTable"), DataTable)
                If HelperFunctions.isNonEmpty(dtTempTable) Then
                    Dim strOldSSN, strReason As String
                    strReason = (dtTempTable.Rows(0)("Reason")).ToString()
                    strOldSSN = (dtTempTable.Rows(0)("OldSSN")).ToString()
                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertAuditLog("Retiree Maintenance", Session("PersId").ToString(), "PERSON", "chrSSNo", strOldSSN, TextBoxGeneralSSNo.Text.Trim(), strReason)
                End If
            End If

            'SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-End

            If (l_string_Output = "0") Then
                'TextBoxSSNo.Enabled = False
                'TextBoxLast.Enabled = False
                'TextBoxFirst.Enabled = False
                'TextBoxMiddle.Enabled = False
                'DropDownListSal.Enabled = False
                'TextBoxSuffix.Enabled = False
                'DropDownListGender.Enabled = False
                'DropDownListMaritalStatus.Enabled = False
                'TextBoxDOB.Enabled = False
                UpadateGeneralInfo = String.Empty
            ElseIf (l_string_Output <> "0" And l_string_Output <> "1") Then
                'Commented and Modified By Ashutosh Patil as on 21-Jun-2007
                'MessageBox.Show("YMCA-YRS", "SSNo. is Already in use for Participant -" + l_string_Output, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                'LoadGeneraltab()
                UpadateGeneralInfo = l_string_Output
                Exit Function
            End If
        Catch
            Throw
        End Try
    End Function
    Private Sub CalculateValues(ByVal dt As DataTable, ByVal benecategory As String)
        ' This function calculates the Percentages for each beneficiary type and then populates the appropriate textboxes
        ' with the correct values. It also displays an error message if the percentage values do not add up to 100 for 
        ' any particular beneficiary type.
        'NP:PS:2007.06.27 - Restructuring code for better performance
        'SP 2013.12.20  BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process  (changes from double to decimal)
        Dim lnPrim, lnCont1, lnCont2, lnCont3 As Decimal
        Dim lnPrimb, lnCont1b, lnCont2b, lnCont3b As Decimal
        Dim lnPrimc, lnCont1c, lnCont2c, lnCont3c As Decimal
        Dim i As Integer
        Dim lcPrimBnft, lcCont1Bnft, lcCont2Bnft, lcCont3Bnft As String
        Dim lcPrimMsg, lcCont1Msg, lcCont2Msg, lcCont3Msg As String
        Dim lcPrimBMsg, lcCont1BMsg, lcCont2BMsg, lcCont3BMsg As String
        Dim lcPrimCMsg, lcCont1CMsg, lcCont2CMsg, lcCont3CMsg As String

        For i = 0 To dt.Rows.Count - 1
            If benecategory = "R" And dt.Rows(i).RowState <> DataRowState.Deleted Then
                'Retirement retired db benefit
                If dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "RETIRE" Then
                    If dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "PRIM" Then
                        lnPrim = lnPrim + dt.Rows(i).Item("Pct")
                    End If
                    If dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" Then
                        Select Case dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim
                            Case "LVL1"
                                lnCont1 = lnCont1 + dt.Rows(i).Item("Pct")
                            Case "LVL2"
                                lnCont2 = lnCont2 + dt.Rows(i).Item("Pct")
                            Case "LVL3"
                                lnCont3 = lnCont3 + dt.Rows(i).Item("Pct")
                        End Select
                    End If
                End If  'End Retirement retired db calculations
                'Retirement Insured reserve benefit
                If dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "INSRES" Then
                    If dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "PRIM" Then
                        lnPrimb = lnPrimb + dt.Rows(i).Item("Pct")
                    End If
                    If dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" Then
                        Select Case dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim
                            Case "LVL1"
                                lnCont1b = lnCont1b + dt.Rows(i).Item("Pct")
                            Case "LVL2"
                                lnCont2b = lnCont2b + dt.Rows(i).Item("Pct")
                            Case "LVL3"
                                lnCont3b = lnCont3b + dt.Rows(i).Item("Pct")
                        End Select
                    End If
                End If  'End Retirement Insured reserve benefit calculations
                'Retirement Insured reserve Savings benefit
                If dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "INSSAV" Then
                    If dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "PRIM" Then
                        lnPrimc = lnPrimc + dt.Rows(i).Item("Pct")
                    End If
                    If dt.Rows(i).Item("Groups").ToString.ToUpper.Trim = "CONT" Then
                        Select Case dt.Rows(i).Item("Lvl").ToString.ToUpper.Trim
                            Case "LVL1"
                                lnCont1c = lnCont1c + dt.Rows(i).Item("Pct")
                            Case "LVL2"
                                lnCont2c = lnCont2c + dt.Rows(i).Item("Pct")
                            Case "LVL3"
                                lnCont3c = lnCont3c + dt.Rows(i).Item("Pct")
                        End Select
                    End If
                End If  'End Retirement Insured reserve Savings benefit calculations
            End If  'End If benecategory = "R" 
        Next

        'SP 2013.12.20  BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process  (remove round function)
        'Set percentage values (main)
        If lnPrim = 0 Then
            TextBoxPrimaryR.Text = "" : ButtonPriR.Enabled = False
        Else
            'lnPrim = Round(lnPrim, 2) : TextBoxPrimaryR.Text = Round(lnPrim, 2)
            lnPrim = lnPrim : TextBoxPrimaryR.Text = lnPrim
            ButtonPriR.Enabled = True
        End If
        If lnCont1 = 0 Then
            TextBoxCont1R.Text = "" : ButtonCont1R.Enabled = False
        Else
            'lnCont1 = Round(lnCont1, 2) : TextBoxCont1R.Text = lnCont1
            lnCont1 = lnCont1 : TextBoxCont1R.Text = lnCont1
            ButtonCont1R.Enabled = True
        End If
        If lnCont2 = 0 Then
            TextBoxCont2R.Text = "" : ButtonCont2R.Enabled = False
        Else
            'lnCont2 = Round(lnCont2, 2) : TextBoxCont2R.Text = lnCont2
            lnCont2 = lnCont2 : TextBoxCont2R.Text = lnCont2
            ButtonCont2R.Enabled = True
        End If
        If lnCont3 = 0 Then
            TextBoxCont3R.Text = "" : ButtonCont3R.Enabled = False
        Else
            'lnCont3 = Round(lnCont3, 2) : TextBoxCont3R.Text = lnCont3
            lnCont3 = lnCont3 : TextBoxCont3R.Text = lnCont3
            ButtonCont3R.Enabled = True
        End If

        'Set percentage values (retired ins res)
        If lnPrimb = 0 Then
            TextBoxPrimaryInsR.Text = "" : ButtonPriInsR.Enabled = False
        Else
            'lnPrimb = Round(lnPrimb, 2) : TextBoxPrimaryInsR.Text = lnPrimb
            lnPrimb = lnPrimb : TextBoxPrimaryInsR.Text = lnPrimb
            ButtonPriInsR.Enabled = True
        End If
        If lnCont1b = 0 Then
            TextBoxCont1InsR.Text = "" : ButtonCont1InsR.Enabled = False
        Else
            'lnCont1b = Round(lnCont1b, 2) : TextBoxCont1InsR.Text = lnCont1b
            lnCont1b = lnCont1b : TextBoxCont1InsR.Text = lnCont1b
            ButtonCont1InsR.Enabled = True
        End If
        If lnCont2b = 0 Then
            TextBoxCont2InsR.Text = "" : ButtonCont2InsR.Enabled = False
        Else
            'lnCont2b = Round(lnCont2b, 2) : TextBoxCont2InsR.Text = lnCont2b
            lnCont2b = lnCont2b : TextBoxCont2InsR.Text = lnCont2b
            ButtonCont2InsR.Enabled = True
        End If
        If lnCont3b = 0 Then
            TextBoxCont3InsR.Text = "" : ButtonCont3InsR.Enabled = False
        Else
            'lnCont3b = Round(lnCont3b, 2) : TextBoxCont3InsR.Text = lnCont3b
            lnCont3b = lnCont3b : TextBoxCont3InsR.Text = lnCont3b
            ButtonCont3InsR.Enabled = True
        End If

        'Set percentage values (retired ins sav)
        If lnPrimc = 0 Then
            TextboxPrimaryInssavR.Text = "" : ButtonPriInssavR.Enabled = False
        Else
            'lnPrimc = Round(lnPrimc, 2) : TextboxPrimaryInssavR.Text = lnPrimc
            lnPrimc = lnPrimc : TextboxPrimaryInssavR.Text = lnPrimc
            ButtonPriInssavR.Enabled = True
        End If
        If lnCont1c = 0 Then
            TextBoxCont1InssavR.Text = "" : ButtonCont1InssavR.Enabled = False
        Else
            'lnCont1c = Round(lnCont1c, 2) : TextBoxCont1InssavR.Text = lnCont1c
            lnCont1c = lnCont1c : TextBoxCont1InssavR.Text = lnCont1c
            ButtonCont1InssavR.Enabled = True
        End If
        If lnCont2c = 0 Then
            TextBoxCont2InssavR.Text = "" : ButtonCont2InssavR.Enabled = False
        Else
            'lnCont2c = Round(lnCont2c, 2) : TextBoxCont2InssavR.Text = lnCont2c
            lnCont2c = lnCont2c : TextBoxCont2InssavR.Text = lnCont2c
            ButtonCont2InssavR.Enabled = True
        End If
        If lnCont3c = 0 Then
            TextBoxCont3InssavR.Text = "" : ButtonCont3InssavR.Enabled = False
        Else
            'lnCont3c = Round(lnCont3c, 2) : TextBoxCont3InssavR.Text = lnCont3c
            lnCont3c = lnCont3c : TextBoxCont3InssavR.Text = lnCont3c
            ButtonCont3InssavR.Enabled = True
        End If

        lcPrimMsg = "Primary retired death benefit beneficiary must equal 100% percent."
        lcCont1Msg = "Contingent level 1 retired death benefit beneficiary must equal 100% percent."
        lcCont2Msg = "Contingent level 2 retired death benefit beneficiary must equal 100% percent."
        lcCont3Msg = "Contingent level 3 retired death benefit beneficiary must equal 100% percent."

        lcPrimBMsg = "Primary retired insured reserve beneficiary must equal 100% percent."
        lcCont1BMsg = "Contingent Level 1 retired insured reserve must equal 100% percent."
        lcCont2BMsg = "Contingent Level 2 retired insured reserve must equal 100% percent."
        lcCont3BMsg = "Contingent Level 3 retired insured reserve must equal 100% percent."

        lcPrimCMsg = "Primary retired insured reserve (Savings) beneficiary must equal 100% percent."
        lcCont1CMsg = "Contingent Level 1 retired insured reserve (Savings) must equal 100% percent."
        lcCont2CMsg = "Contingent Level 2 retired insured reserve (Savings) must equal 100% percent."
        lcCont3CMsg = "Contingent Level 3 retired insured reserve (Savings) must equal 100% percent."

        'Set Percentage values main
        Dim l_string_ValidationMessage As String = ""
        If lnPrim <> 0 And lnPrim <> 100 Then l_string_ValidationMessage += " " + lcPrimMsg
        If lnCont1 <> 0 And lnCont1 <> 100 Then l_string_ValidationMessage += " " + lcCont1Msg
        If lnCont2 <> 0 And lnCont2 <> 100 Then l_string_ValidationMessage += " " + lcCont2Msg
        If lnCont3 <> 0 And lnCont3 <> 100 Then l_string_ValidationMessage += " " + lcCont3Msg

        If lnPrimb <> 0 And lnPrimb <> 100 Then l_string_ValidationMessage += " " + lcPrimBMsg
        If lnCont1b <> 0 And lnCont1b <> 100 Then l_string_ValidationMessage += " " + lcCont1BMsg
        If lnCont2b <> 0 And lnCont2b <> 100 Then l_string_ValidationMessage += " " + lcCont2BMsg
        If lnCont3b <> 0 And lnCont3b <> 100 Then l_string_ValidationMessage += " " + lcCont3BMsg

        If lnPrimc <> 0 And lnPrimc <> 100 Then l_string_ValidationMessage += " " + lcPrimCMsg
        If lnCont1c <> 0 And lnCont1c <> 100 Then l_string_ValidationMessage += " " + lcCont1CMsg
        If lnCont2c <> 0 And lnCont2c <> 100 Then l_string_ValidationMessage += " " + lcCont2CMsg
        If lnCont3c <> 0 And lnCont3c <> 100 Then l_string_ValidationMessage += " " + lcCont3CMsg

        If l_string_ValidationMessage <> "" Then
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_ValidationMessage, MessageBoxButtons.OK)
            Session("ValidationMessage") = l_string_ValidationMessage
        Else
            Session("ValidationMessage") = Nothing
        End If
    End Sub

    Public Sub LoadBeneficiariesTab()

        Dim l_string_PersId As String
        Dim l_dataset_Retired As New DataSet
        Dim l_arraylist_Access As ArrayList
        Dim dsAddress As New DataSet
        Dim i As Integer

        l_string_PersId = Session("PersId")
        'Populate the appropriate beneficiary dataset and then use it to bind to the grid
        If Session("Flag") = "AddBeneficiaries" Or Session("Flag") = "EditBeneficiaries" Or Session("Flag") = "DeleteBeneficiaries" Then
            If Not Session("BeneficiariesRetired") Is Nothing Then
                l_dataset_Retired = Session("BeneficiariesRetired")

                'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
                'check beneficiary percentage is modified
                If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesRetired"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                    IsModifiedPercentage = 1
                End If
                'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
            End If
        Else
            If Session("Flag") = "Retire" Or Session("Flag") = "" Then
                l_dataset_Retired = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpRetiredBeneficiariesInfo(l_string_PersId)
                OriginalBeneficiaries = l_dataset_Retired.Copy() 'SP 2014.12.02  BT-2310\YRS 5.0-2255:

            End If
        End If
        ' Check if user can change the beneficiaries for this status type
        l_arraylist_Access = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.SetBeneficiaryAccess(l_string_PersId)
        If l_arraylist_Access.Item(1).ToString() = "1" Then
            DataGridRetiredBeneficiaries.Visible = True
            '                LabelNotSet.Visible = False
            Session("WithDrawn_member") = False
        Else
            l_dataset_Retired = Nothing
            '                LabelNotSet.Visible = True
            Session("WithDrawn_member") = True
        End If


        'ASHISH:2010.03.12:Added for Thread being aborted error when no active beneficiary
        'If HelperFunctions.isNonEmpty(l_dataset_Retired) Then
        '    Session("BeneficiariesRetired") = l_dataset_Retired
        'Else
        '    Session("BeneficiariesRetired") = Nothing
        'End If

        'Sanjay Rawat :2010.05.10:YRS 5.0 - 1078 :above commented line of ashish assign nothing to dataset even if it has structure defined 
        If l_dataset_Retired Is Nothing Then
            Session("BeneficiariesRetired") = Nothing
        Else
            If (l_dataset_Retired.Tables.Count >= 0) Then
                Session("BeneficiariesRetired") = l_dataset_Retired
                'AA:13.11.2013 :BT:1455:YRS 5.1 1733: Added below code to Hide the contingency level 2 percentage text box whwn there are no contingency level 2 beneficiary(ies) does not exists 
                If l_dataset_Retired.Tables(0).Select("Lvl = 'LVL2'").Length = 0 Then
                    LabelCont2R.Visible = False
                    TextBoxCont2InsR.Visible = False
                    TextBoxCont2InssavR.Visible = False
                    TextBoxCont2R.Visible = False
                Else
                    LabelCont2R.Visible = True
                    TextBoxCont2InsR.Visible = True
                    TextBoxCont2InssavR.Visible = True
                    TextBoxCont2R.Visible = True
                End If

                'AA:13.11.2013 :BT:1455:YRS 5.1 1733: Added below code to Hide the contingency level 3 percentage text box whwn there are no contingency level 3 beneficiary(ies) does not exists 
                If l_dataset_Retired.Tables(0).Select("Lvl = 'LVL3'").Length = 0 Then
                    LabelCont3R.Visible = False
                    TextBoxCont3InsR.Visible = False
                    TextBoxCont3InssavR.Visible = False
                    TextBoxCont3R.Visible = False
                Else
                    LabelCont3R.Visible = True
                    TextBoxCont3InsR.Visible = True
                    TextBoxCont3InssavR.Visible = True
                    TextBoxCont3R.Visible = True
                End If
            Else
                Session("BeneficiariesRetired") = Nothing
            End If
        End If
        'End of code for Sanjay Rawat :2010.05.10:YRS 5.0 - 1078




        'ASHISH:2010.03.12:Commented for Thread being aborted error when no active beneficiary
        'Session("BeneficiariesRetired") = l_dataset_Retired


        'NP:PS:2007.06.28 - Performing Final checks after load beneficiary
        If Session("BeneficiariesRetired") Is Nothing Then
            DataGridRetiredBeneficiaries.Visible = False
            HideRetiredParticipantsInformation()    'NP:PS:2007.06.20 - Factoring Code
        Else
            CalculateValues(l_dataset_Retired.Tables(0), "R")
            DataGridRetiredBeneficiaries.DataSource = Session("BeneficiariesRetired")
            DataGridRetiredBeneficiaries.DataBind()
        End If
        PrepareAuditTable()   '// SB | 07/07/2016 | YRS-AT-2382 | Adding Beneficiaires to Auditlog table
    End Sub

    ' This function hides all controls on the Beneficiaries tab related to retired participants
    Public Sub HideRetiredParticipantsInformation()
        'YSPS-3905:2007.10.15 - Rewriting entire section to make sure we do not miss any labels, buttons or textboxes from being hidden.
        LabelPercentage2.Visible = False
        LabelRetiredDBR.Visible = False
        LabelInsResR.Visible = False
        LabelInsSavR.Visible = False
        'SP 2014.06.13 BT-2571 -Start
        'Label6.Visible = False
        'Label10.Visible = False
        'Label11.Visible = False
        'SP 2014.06.13 BT-2571 -Start
        LabelPrimaryR.Visible = False
        TextBoxPrimaryR.Visible = False
        ButtonPriR.Visible = False
        TextBoxPrimaryInsR.Visible = False
        ButtonPriInsR.Visible = False
        TextboxPrimaryInssavR.Visible = False
        ButtonPriInssavR.Visible = False
        LabelCont1R.Visible = False
        TextBoxCont1R.Visible = False
        ButtonCont1R.Visible = False
        TextBoxCont1InsR.Visible = False
        ButtonCont1InsR.Visible = False
        TextBoxCont1InssavR.Visible = False
        ButtonCont1InssavR.Visible = False
        LabelCont2R.Visible = False
        TextBoxCont2R.Visible = False
        ButtonCont2R.Visible = False
        TextBoxCont2InsR.Visible = False
        ButtonCont2InsR.Visible = False
        TextBoxCont2InssavR.Visible = False
        ButtonCont2InssavR.Visible = False
        LabelCont3R.Visible = False
        TextBoxCont3R.Visible = False
        ButtonCont3R.Visible = False
        TextBoxCont3InsR.Visible = False
        ButtonCont3InsR.Visible = False
        TextBoxCont3InssavR.Visible = False
        ButtonCont3InssavR.Visible = False
        ButtonAddRetired.Visible = False
        ButtonEditRetired.Visible = False
        ButtonDeleteRetired.Visible = False
    End Sub
#Region "Beneficiary - Add/Edit/Delete Click Events"
    Private Sub ButtonAddRetired_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddRetired.Click
        Dim strWSMessage As String
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAddRetired", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'START : Dharmesh : 11/20/2018 : YRS-AT-4136 : Added validation for not having c annuity at the time of clicking Add beneficiary from the beneficiary main screen, also displaying the error message 1004 
            'Display text for 1004 : Participant was first enrolled on or after  $$CutOffDate$$, so no Retired Death Benefit is available. A beneficiary cannot be added.
            If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) _
                AndAlso Not (YMCARET.YmcaBusinessObject.RetirementBOClass.HasInsuredReserveAnnuity(CType(Session("AnnuityDetails"), DataSet))) Then
                ShowErrorMessage(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019, YMCARET.YmcaBusinessObject.RetirementBOClass.GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText())
                Exit Sub
            End If
            'END : Dharmesh : 11/20/2018 : YRS-AT-4136 : Added validation for not having c annuity at the time of clicking Add beneficiary from the beneficiary main screen, also displaying the error message 1004 
            'End : YRS 5.0-940
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
            strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
            If strWSMessage <> "NoPending" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Invalid Beneficiary Operation", "openDialog('" + strWSMessage + "','Bene');", True)
                strWSMessage = (strWSMessage.Replace("<br/>", "\n")).Replace("<br>", "\n")
                Me.TabStripRetireesInformation.Items(2).Text = "<asp:Label ID='lblBeneficiaries' CssClass='Label_Small'onmouseover='javascript: showToolTip(""" + strWSMessage + """,""Bene"");' onmouseout='javascript: hideToolTip();'><font color=red>Beneficiary</font></asp:Label>"

                cboGeneralMaritalStatus.Enabled = False

                imgLock.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Pers');")
                imgLock.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                imgLock.Visible = True

                imgLockBeneficiary.Visible = True
                imgLockBeneficiary.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Bene');")
                imgLockBeneficiary.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                Exit Sub
            Else
                imgLock.Visible = False
                imgLockBeneficiary.Visible = False
                cboGeneralMaritalStatus.Enabled = True
                Me.TabStripRetireesInformation.Items(2).Text = "Beneficiaries"
            End If
            'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application

            Session("EnableSaveCancel") = True

            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            Me.MakeLinkVisible()

            'ButtonRetireesInfoOK.enabled = False
            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session
            Dim l_string_FundStatus As String
            Dim _icounter As Integer
            _icounter = Session("icounter")
            _icounter = _icounter + 1
            Session("icounter") = _icounter
            If _icounter = 1 Then
                Session("Flag") = "AddBeneficiaries"
                Session("MaritalStatus") = cboGeneralMaritalStatus.SelectedValue 'YRPS-4704

                'Start - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750
                Dim msg1 As String = "<script language='javascript'>" & _
                "window.open('AddBeneficiary.aspx','CustomPopUp', " & _
                "'width=900, height=700, menubar=no, Resizable=No,top=120,left=120, scrollbars=yes')" & _
                 "</script>" 'MMR | 2017.12.04 | YRS-AT-3756 | Chnaged the window width and height to 900 and 700 from 850 and 650
                'End - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750

                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", msg1)
                End If
            Else
                'Vipul 01Feb06 Cache-Session
                'Me.DataGridRetiredBeneficiaries.DataSource = CType(Cache("BeneficiariesRetired"), DataSet)
                Me.DataGridRetiredBeneficiaries.DataSource = DirectCast(Session("BeneficiariesRetired"), DataSet)
                'Vipul 01Feb06 Cache-Session
                Me.DataGridRetiredBeneficiaries.DataBind()
                _icounter = 0
                Session("icounter") = _icounter
            End If

        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Added By Dinesh Kanojia on 18/12/2012
    Private Sub EditRetired(ByVal DatagridRow As TableCellCollection, ByVal iIndex As Integer)
        Try
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim l_image_Edit As ImageButton

            l_image_Edit = DatagridRow(1).FindControl("ImagebuttonEdit")

            Dim checkSecurity As String = SecurityCheck.Check_Authorization(l_image_Edit.ID, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            Dim _icounter As Integer
            'If Me.DataGridRetiredBeneficiaries.SelectedIndex <> -1 Then
            Session("Flag") = "EditBeneficiaries"
            '*********Ashutosh Code Commented By Ashutosh on 13-June-06*******
            'Session("Name") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(5).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(5).Text.Trim)
            'Session("Name2") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(6).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(6).Text.Trim)
            'Session("TaxID") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(7).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(7).Text.Trim)
            'Session("Rel") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(8).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(8).Text.Trim)
            'Session("Birthdate") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(9).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(9).Text.Trim)
            'Session("Groups") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text.Trim)
            'Session("Lvl") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text.Trim)
            'Session("Pct") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(14).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(14).Text.Trim)
            'Session("Type") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(15).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(15).Text.Trim)

            'Session("Name") = DatagridRow(6).Text().Replace("&nbsp;", "").Trim()
            'Session("Name2") = DatagridRow(7).Text().Replace("&nbsp;", "").Trim()
            'Session("TaxID") = DatagridRow(8).Text().Replace("&nbsp;", "").Trim()
            'Session("Rel") = DatagridRow(9).Text().Replace("&nbsp;", "").Trim()
            'Session("Birthdate") = DatagridRow(10).Text().Replace("&nbsp;", "").Trim()
            'Session("Groups") = DatagridRow(11).Text().Replace("&nbsp;", "").Trim()
            'Session("Lvl") = DatagridRow(12).Text().Replace("&nbsp;", "").Trim()
            'Session("Pct") = DatagridRow(15).Text().Replace("&nbsp;", "").Trim()
            'Session("Type") = DatagridRow(16).Text().Replace("&nbsp;", "").Trim()
            '************************End of Ashutosh*****************    
            _icounter = Session("icounter")
            _icounter = _icounter + 1
            Session("_icounter") = _icounter
            Dim popupScript As String
            If (_icounter = 1) Then
                Session("MaritalStatus") = cboGeneralMaritalStatus.SelectedValue 'YRPS-4704

                'Start - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750
                If (DatagridRow(2).Text = "&nbsp;" Or DatagridRow(2).Text = "") And (DatagridRow(19).Text <> "&nbsp;" Or DatagridRow(19).Text <> "") Then
                    popupScript = "<script language='javascript'>" & _
                    "window.open('AddBeneficiary.aspx?NewID=" & DatagridRow(19).Text & "', 'CustomPopUp', " & _
                    "'width=900, height=700, menubar=no, Resizable=no,top=120,left=120, scrollbars=no')" & _
                    "</script>" 'MMR | 2017.12.04 | YRS-AT-3756 | Chnaged the window width and height to 900 and 700 from 850 and 650
                ElseIf (DatagridRow(2).Text <> "&nbsp;" Or DatagridRow(2).Text <> "") And (DatagridRow(19).Text = "&nbsp;" Or DatagridRow(19).Text = "") Then
                    popupScript = "<script language='javascript'>" & _
                     "window.open('AddBeneficiary.aspx?UniqueId=" + DatagridRow(2).Text + "','CustomPopUp', " & _
                     "'width=900, height=700, menubar=no, Resizable=no,top=120,left=120, scrollbars=no')" & _
                     "</script>" 'MMR | 2017.12.04 | YRS-AT-3756 | Chnaged the window width and height to 900 and 700 from 850 and 650
                    'End - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750
                End If

                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", popupScript)

                End If
            Else

                _icounter = 0
                Session("icounter") = _icounter
            End If

            'NP:PS:2007.08.07 - The enable button code is moved here so that they are enabled only when the person is in edit mode.
            Session("EnableSaveCancel") = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            Me.MakeLinkVisible()
            'ButtonRetireesInfoOK.enabled = False
            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session

        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    'Added By Dinesh Kanojia on 18/12/2012
    Private Sub DeleteRetired(ByVal DatagridRow As TableCellCollection, ByVal iIndex As Integer)
        Try
            'START: MMR | 2017.12.04 | YRS-AT-3756 | Save Deleted Beneficiary details
            Dim reasonCode As String
            Dim deletedReasonTable As DataTable
            Dim deletedReasonView As DataView
            Dim deathDate As String
            'END: MMR | 2017.12.04 | YRS-AT-3756 | Save Deleted Beneficiary details
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            Session("Flag") = "DeleteBeneficiaries"
            Me.MakeLinkVisible()

            ' If Me.DataGridRetiredBeneficiaries.SelectedIndex <> -1 Then
            Dim Beneficiaries As DataSet
            Dim drRows As DataRow()
            Dim drUpdated As DataRow
            'Vipul 01Feb06 Cache-Session
            'Dim l_CacheManager As CacheManager
            'l_CacheManager = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session
            If DatagridRow(2).Text <> "&nbsp;" Then

                'Vipul 01Feb06 Cache-Session
                'Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
                Beneficiaries = DirectCast(Session("BeneficiariesRetired"), DataSet)
                'Vipul 01Feb06 Cache-Session

                Dim l_UniqueId As String
                l_UniqueId = DatagridRow(2).Text

                'START: MMR | 2017.12.04 | YRS-AT-3756 | Storing reason for death
                If Not Session("dtReason") Is Nothing Then
                    deletedReasonTable = Session("dtReason")
                    deletedReasonView = deletedReasonTable.DefaultView
                    deletedReasonView.RowFilter = String.Format("strBeneUniqueID = '{0}'", l_UniqueId)
                    reasonCode = deletedReasonView(0)("ReasonCode").ToString()
                    deathDate = deletedReasonView(0)("strDOD").ToString()
                End If
                'END: MMR | 2017.12.04 | YRS-AT-3756 | Storing reason for death

                If Not IsNothing(Beneficiaries) Then
                    drRows = Beneficiaries.Tables(0).Select("UniqueID='" & l_UniqueId & "'")
                    drUpdated = drRows(0)
                    'START: MMR | 2017.12.04 | YRS-AT-3756 | Added death reason code
                    drUpdated("DeathReason") = reasonCode
                    drUpdated("DeathDate") = deathDate
                    drUpdated.AcceptChanges()
                    'END: MMR | 2017.12.04 | YRS-AT-3756 | Added death reason code
                    drUpdated.Delete()
                    'Vipul 01Feb06 Cache-Session
                    'l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
                    Session("BeneficiariesRetired") = Beneficiaries
                    'Vipul 01Feb06 Cache-Session
                End If
            End If

            If DatagridRow(2).Text = "&nbsp;" Then
                'Vipul 01Feb06 Cache-Session
                'Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
                Beneficiaries = DirectCast(Session("BeneficiariesRetired"), DataSet)
                'Vipul 01Feb06 Cache-Session
                If Not IsNothing(Beneficiaries) Then

                    drUpdated = Beneficiaries.Tables(0).Rows(Me.DataGridRetiredBeneficiaries.SelectedIndex)
                    drUpdated.Delete()
                    'Vipul 01Feb06 Cache-Session
                    'l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
                    Session("BeneficiariesRetired") = Beneficiaries
                    'Vipul 01Feb06 Cache-Session
                End If
            End If
            LoadBeneficiariesTab()
            Session("Flag") = ""

            'NP:PS:2007.08.07 - The enable button code is moved here so that they are enabled only when the person is in edit mode.
            Session("EnableSaveCancel") = True
            'ButtonRetireesInfoOK.enabled = False
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True

        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonEditRetired_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditRetired.Click
        Try
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditRetired", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            Dim _icounter As Integer
            If Me.DataGridRetiredBeneficiaries.SelectedIndex <> -1 Then
                Session("Flag") = "EditBeneficiaries"
                '*********Ashutosh Code Commented By Ashutosh on 13-June-06*******
                'Session("Name") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(5).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(5).Text.Trim)
                'Session("Name2") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(6).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(6).Text.Trim)
                'Session("TaxID") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(7).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(7).Text.Trim)
                'Session("Rel") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(8).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(8).Text.Trim)
                'Session("Birthdate") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(9).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(9).Text.Trim)
                'Session("Groups") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(10).Text.Trim)
                'Session("Lvl") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(11).Text.Trim)
                'Session("Pct") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(14).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(14).Text.Trim)
                'Session("Type") = IIf(IsDBNull(DataGridRetiredBeneficiaries.SelectedItem.Cells(15).Text), "", DataGridRetiredBeneficiaries.SelectedItem.Cells(15).Text.Trim)

                Session("Name") = DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_NAME).Text().Replace("&nbsp;", "").Trim()
                Session("Name2") = DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_NAME2).Text().Replace("&nbsp;", "").Trim()
                Session("TaxID") = DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_TaxID).Text().Replace("&nbsp;", "").Trim()
                Session("Rel") = DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_REL).Text().Replace("&nbsp;", "").Trim()
                Session("Birthdate") = DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_BDAY).Text().Replace("&nbsp;", "").Trim()
                Session("Groups") = DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_GROUPS).Text().Replace("&nbsp;", "").Trim()
                Session("Lvl") = DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_LVL).Text().Replace("&nbsp;", "").Trim()
                Session("Pct") = DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_PCT).Text().Replace("&nbsp;", "").Trim()
                Session("Type") = DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_TYPE).Text().Replace("&nbsp;", "").Trim()
                '************************End of Ashutosh*****************    
                _icounter = Session("icounter")
                _icounter = _icounter + 1
                Session("_icounter") = _icounter
                Dim popupScript As String
                If (_icounter = 1) Then
                    Session("MaritalStatus") = cboGeneralMaritalStatus.SelectedValue 'YRPS-4704

                    'Start - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750
                    If Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(1).Text = "&nbsp;" Then
                        popupScript = "<script language='javascript'>" & _
                        "window.open('AddBeneficiary.aspx?Index=" & Me.DataGridRetiredBeneficiaries.SelectedIndex & "', 'CustomPopUp', " & _
                        "'width=900, height=700, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                        "</script>" 'MMR | 2017.12.04 | YRS-AT-3756 | Chnaged the window width and height to 900 and 700 from 850 and 650
                        'End - Manthan Rajguru | 2016.07.22 | YRS-AT-2560 | Changed the window width size - 750
                    Else
                        ' // START : SB | 07/07/2016 | YRS-AT-2382 | Replaced index 1 with constant defined as well as kept the string in String.Format 
                        'popupScript = "<script language='javascript'>" & _
                        ' "window.open('AddBeneficiary.aspx?UniqueId=" + Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(1).Text + "','CustomPopUp', " & _
                        ' "'width=750, height=650, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                        ' "</script>"
                        popupScript = String.Format("<script language='javascript'>window.open('AddBeneficiary.aspx?UniqueId={0}','CustomPopUp', 'width=900, height=700, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')</script>", Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_PERSID).Text) 'MMR | 2017.12.04 | YRS-AT-3756 | Chnaged the window width and height to 900 and 700 from 850 and 650
                        ' // END : SB | 07/07/2016 | YRS-AT-2382 | Replaced index 1 with constant defined as well as kept the string in String.Format 
                    End If

                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", popupScript)

                    End If
                Else

                    _icounter = 0
                    Session("icounter") = _icounter
                End If
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an Beneficiary record to be updated.", MessageBoxButtons.OK)
                Exit Sub
            End If

            'NP:PS:2007.08.07 - The enable button code is moved here so that they are enabled only when the person is in edit mode.
            Session("EnableSaveCancel") = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            Me.MakeLinkVisible()
            'ButtonRetireesInfoOK.enabled = False
            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session

        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonDeleteRetired_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDeleteRetired.Click
        Try
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            Session("Flag") = "DeleteBeneficiaries"
            Me.MakeLinkVisible()

            If Me.DataGridRetiredBeneficiaries.SelectedIndex <> -1 Then
                Dim Beneficiaries As DataSet
                Dim drRows As DataRow()
                Dim drUpdated As DataRow
                'Vipul 01Feb06 Cache-Session
                'Dim l_CacheManager As CacheManager
                'l_CacheManager = CacheFactory.GetCacheManager()
                'Vipul 01Feb06 Cache-Session
                If Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_PERSID).Text <> "&nbsp;" Then ' // SB | 07/07/2016 | YRS-AT-2382 | Replaced index number with constant

                    'Vipul 01Feb06 Cache-Session
                    'Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
                    Beneficiaries = DirectCast(Session("BeneficiariesRetired"), DataSet)
                    'Vipul 01Feb06 Cache-Session

                    Dim l_UniqueId As String
                    l_UniqueId = Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_PERSID).Text ' // SB | 07/07/2016 | YRS-AT-2382 | Replaced index number with constant
                    If Not IsNothing(Beneficiaries) Then
                        drRows = Beneficiaries.Tables(0).Select("UniqueID='" & l_UniqueId & "'")
                        drUpdated = drRows(0)
                        drUpdated.Delete()
                        'Vipul 01Feb06 Cache-Session
                        'l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
                        Session("BeneficiariesRetired") = Beneficiaries
                        'Vipul 01Feb06 Cache-Session
                    End If
                End If

                If Me.DataGridRetiredBeneficiaries.SelectedItem.Cells(RETIREBENEF_PERSID).Text = "&nbsp;" Then ' // SB | 07/07/2016 | YRS-AT-2382 | Replaced index number with constant
                    'Vipul 01Feb06 Cache-Session
                    'Beneficiaries = CType(l_CacheManager("BeneficiariesRetired"), DataSet)
                    Beneficiaries = DirectCast(Session("BeneficiariesRetired"), DataSet)
                    'Vipul 01Feb06 Cache-Session
                    If Not IsNothing(Beneficiaries) Then

                        drUpdated = Beneficiaries.Tables(0).Rows(Me.DataGridRetiredBeneficiaries.SelectedIndex)
                        drUpdated.Delete()
                        'Vipul 01Feb06 Cache-Session
                        'l_CacheManager.Add("BeneficiariesRetired", Beneficiaries)
                        Session("BeneficiariesRetired") = Beneficiaries
                        'Vipul 01Feb06 Cache-Session
                    End If
                End If
                LoadBeneficiariesTab()
                Session("Flag") = ""
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a record to be deleted.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            Me.DataGridRetiredBeneficiaries.SelectedIndex -= 1 'aparna 3/12/2007

            'NP:PS:2007.08.07 - The enable button code is moved here so that they are enabled only when the person is in edit mode.
            Session("EnableSaveCancel") = True
            'ButtonRetireesInfoOK.enabled = False
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True

        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
#End Region
#Region "Beneficiary - Retired - Equalize Functions"
#Region "Beneficiary - Retired - Primary"
    Private Sub ButtonPriR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPriR.Click
        Try
            ''ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False

            'Vipul 01Feb06 Cache-Session
            'Me.Equalize(Cache("BeneficiariesRetired"), "P", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "P", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "PRIM", "", "RETIRE")
            'Vipul 01Feb06 Cache-Session

            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesRetired"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
                Exit Sub
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonPriInssavR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPriInsR.Click
        Try
            ''ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False

            'Vipul 01Feb06 Cache-Session
            'Me.Equalize(Cache("BeneficiariesRetired"), "PB", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "PB", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "PRIM", "", "INSRES")
            'Vipul 01Feb06 Cache-Session
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesRetired"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonPriInsR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPriInssavR.Click
        Try
            ''ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False

            'Vipul 01Feb06 Cache-Session
            'Me.Equalize(Cache("BeneficiariesRetired"), "PB", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "PB", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "PRIM", "", "INSSAV")
            'Vipul 01Feb06 Cache-Session
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesRetired"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
#End Region
#Region "Beneficiary - Retired - Contingent Level 1"
    Private Sub ButtonCont1R_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont1R.Click
        Try
            ''ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False
            'Vipul 01Feb06 Cache-Session
            'Me.Equalize(Cache("BeneficiariesRetired"), "C1", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "C1", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "CONT", "1", "RETIRE")
            'Vipul 01Feb06 Cache-Session

            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesRetired"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCont1InsR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont1InsR.Click
        Try
            ''ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False

            'Vipul 01Feb06 Cache-Session
            'Me.Equalize(Cache("BeneficiariesRetired"), "C1B", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "C1B", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "CONT", "1", "INSRES")
            'Vipul 01Feb06 Cache-Session
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesRetired"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCont1InssavR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont1InssavR.Click
        Try
            ''ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False

            'Vipul 01Feb06 Cache-Session
            'Me.Equalize(Cache("BeneficiariesRetired"), "C1B", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "C1B", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "CONT", "1", "INSSAV")
            'Vipul 01Feb06 Cache-Session
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesRetired"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
#End Region
#Region "Beneficiary - Retired - Contingent Level 2"
    Private Sub ButtonCont2R_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont2R.Click
        Try
            'Vipul 01Feb06 Cache-Session
            'Me.Equalize(Cache("BeneficiariesRetired"), "C1", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "C2", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "CONT", "2", "RETIRE")
            'Vipul 01Feb06 Cache-Session
            'ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCont2InsR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont2InsR.Click
        Try

            'Vipul 01Feb06 Cache-Session
            'Me.Equalize(Cache("BeneficiariesRetired"), "C1B", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "C2B", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "CONT", "2", "INSRES")
            'Vipul 01Feb06 Cache-Session

            'ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCont2InssavR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont2InssavR.Click
        Try

            'Vipul 01Feb06 Cache-Session
            'Me.Equalize(Cache("BeneficiariesRetired"), "C1B", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "C2B", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "CONT", "2", "INSSAV")
            'Vipul 01Feb06 Cache-Session

            'ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            '            ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
#End Region
#Region "Beneficiary - Retired - Contingent Level 3"
    Private Sub ButtonCont3R_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont3R.Click
        Try
            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Me.Equalize(Cache("BeneficiariesRetired"), "C3", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "C3", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "CONT", "3", "RETIRE")
            'Vipul 01Feb06 Cache-Session

            'ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCont3InsR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont3InsR.Click
        Try

            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Me.Equalize(Cache("BeneficiariesRetired"), "C3B", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "C3B", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "CONT", "3", "INSRES")
            'Vipul 01Feb06 Cache-Session

            'ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCont3InssavR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont3InssavR.Click
        Try

            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Me.Equalize(Cache("BeneficiariesRetired"), "C3B", "R")
            'Me.Equalize(Session("BeneficiariesRetired"), "C3B", "R")
            Me.Equalize(Session("BeneficiariesRetired"), "CONT", "3", "INSSAV")
            'Vipul 01Feb06 Cache-Session

            'ButtonRetireesInfoOK.enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            'ButtonRetireesInfoSave.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoOK.Enabled = False
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
#End Region
#End Region
    'Private Sub Equalize(ByVal ds As DataSet, ByVal lcGroup As String, ByVal lcBeneficiaryCategory As String)
    '    Try
    '        Dim i As Integer
    '        Dim lncnt As Integer
    '        Dim dt As DataTable

    '        dt = ds.Tables(0)
    '        For i = 0 To dt.Rows.Count - 1
    '            If Not dt.Rows(i).RowState = DataRowState.Deleted Then


    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "P" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "C1" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "C2" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "C3" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                'Retirement Retired db Benefit
    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "P" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then

    '                    lncnt = lncnt + 1
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C1" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C2" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C3" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                'Retirement insured reserve benefit

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "PB" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C1B" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C2B" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
    '                    lncnt = lncnt + 1
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C3B" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
    '                    lncnt = lncnt + 1
    '                End If
    '            End If
    '        Next
    '        Dim lnVal1Portion As Double
    '        Dim lnValPortionTimesCountMinus1 As Double
    '        Dim lnVal1 As Double
    '        Dim lnVal2 As Double
    '        Dim lnDiffWith100 As Double
    '        If lncnt <> 0 Then
    '            'Aparna yren -2849
    '            lnVal1Portion = Convert.ToDouble(((100.0 / lncnt) * 10000) / 10000)
    '            lnValPortionTimesCountMinus1 = Convert.ToDouble((lnVal1Portion * (lncnt - 1) * 100000) / 100000)
    '            'Aparna yren -2849
    '            lnDiffWith100 = 100.0 - lnValPortionTimesCountMinus1

    '            lnVal1 = lnVal1Portion
    '            lnVal2 = lnDiffWith100
    '        End If

    '        Dim lnFoundCnt As Integer

    '        lnFoundCnt = 0
    '        For i = 0 To dt.Rows.Count - 1
    '            If Not dt.Rows(i).RowState = DataRowState.Deleted Then



    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "P" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna Samala -yren 2849
    '                    ' dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If


    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "C1" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    '   dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "C2" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "C3" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    '  dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                'Retirement Retired db Benefit

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "P" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna Samala -yren 2849
    '                    '  dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C1" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna Samala -yren 2849
    '                    'dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C2" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna Samala -yren 2849
    '                    'dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C3" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna Samala -yren 2849
    '                    'dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                'Retirement insured reserve benefit

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "PB" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna Samala -yren 2849
    '                    ' dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C1B" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna Samala -yren 2849
    '                    'dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C2B" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna Samala -yren 2849
    '                    'dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C3B" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna Samala -yren 2849
    '                    'dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 4), Round(lnVal1, 4))
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                ' dt.AcceptChanges()
    '                'ds.AcceptChanges()
    '            End If

    '        Next
    '        If lcBeneficiaryCategory = "A" Then

    '            'Vipul 01Feb06 Cache-Session
    '            'Cache.Add("BeneficiariesActive", ds)
    '            Session("BeneficiariesActive") = ds
    '            'Vipul 01Feb06 Cache-Session

    '            Me.DataGridActiveBeneficiaries.DataSource = ds
    '            Me.DataGridActiveBeneficiaries.DataBind()
    '            Me.CalculateValues(ds.Tables(0), "A")
    '        Else
    '            'Vipul 01Feb06 Cache-Session
    '            'Cache.Add("BeneficiariesRetired", ds)
    '            Session("BeneficiariesRetired") = ds
    '            'Vipul 01Feb06 Cache-Session
    '            Me.DataGridRetiredBeneficiaries.DataSource = ds
    '            Me.DataGridRetiredBeneficiaries.DataBind()
    '            Me.CalculateValues(ds.Tables(0), "R")
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    Private Sub Equalize(ByVal ds As DataSet, ByVal lcGroup As String, ByVal lcLevel As String, ByVal lcBeneficiaryType As String)
        ' NP:PS:2007.06.20 - Rewriting Equalize Function more efficiently
        ' This function is used to sum values over all beneficiaries of a specified type and then assign 
        ' 100% as equally among them as possible. The last beneficiary may receive a slightly higher or lower
        ' value depending on the rounding errors.
        Dim lncnt As Integer
        Dim dt As DataTable

        dt = ds.Tables(0)
        Dim rows As DataRow()
        Dim filterCondition As String = ""

        If lcGroup <> "" Then filterCondition += IIf(filterCondition <> "", " AND ", "") + " Groups = '" + lcGroup + "'"
        If lcLevel <> "" Then filterCondition += IIf(filterCondition <> "", " AND ", "") + " Lvl = 'LVL" + lcLevel + "'"
        If lcBeneficiaryType <> "" Then filterCondition += IIf(filterCondition <> "", " AND ", "") + " BeneficiaryTypeCode = '" + lcBeneficiaryType + "'"

        rows = dt.Select(filterCondition)

        lncnt = rows.Length
        ''SP 2013.12.20  BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process  (Replace double to decimal datatype)
        Dim lnVal1Portion As Decimal
        Dim lnValPortionTimesCountMinus1 As Decimal
        Dim lnVal1 As Decimal
        Dim lnVal2 As Decimal
        Dim lnDiffWith100 As Decimal
        If lncnt <> 0 Then
            'Aparna yren -2849
            lnVal1Portion = Math.Round(Convert.ToDecimal(((100.0 / lncnt) * 10000) / 10000), 5)
            lnValPortionTimesCountMinus1 = Convert.ToDecimal((lnVal1Portion * (lncnt - 1) * 100000) / 100000)
            'Aparna yren -2849
            lnDiffWith100 = 100.0 - lnValPortionTimesCountMinus1

            lnVal1 = lnVal1Portion
            lnVal2 = lnDiffWith100
        End If

        Dim i As Integer

        For i = 1 To rows.Length - 1
            rows(i).Item("Pct") = Round(lnVal1, 5)
        Next
        If rows.Length > 0 Then
            rows(0).Item("Pct") = Round(lnVal2, 5)
        End If

        Session("BeneficiariesRetired") = ds
        Me.DataGridRetiredBeneficiaries.DataSource = ds
        Me.DataGridRetiredBeneficiaries.DataBind()
        Me.CalculateValues(ds.Tables(0), "R")

    End Sub
    Private Function ProcessPhonySSNo() As String
        'Added By Ashutosh Patil
        'YREN-3490
        Dim l_string_Message As String
        Try
            If Me.ButtonEditAddress.Enabled = False Then
                ProcessPhonySSNo = "NotValidated"
                Dim l_AddressValidation As String = String.Empty
                Dim l_string_ErrorMessage As String = String.Empty

                l_AddressValidation = AddressValidation()
                l_string_ErrorMessage = l_AddressValidation
                If l_string_ErrorMessage <> "" Then

                    MessageBox.Show(PlaceHolder1, "YMCA", l_string_ErrorMessage, MessageBoxButtons.OK)
                    Exit Function
                End If
                'Call SetPrimaryAddressDetails()
                ''Secondary Address
                'Call SetSecondaryAddressDetails()

                ''Added By Ashutosh Patil as on 23-Feb-2007 For YREN - 3029,YREN - 3028
                'If Trim(m_str_Pri_Address1) = "" Then
                '	MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Primary Address.", MessageBoxButtons.Stop)
                '	''Me.ButtonRetireesInfoSave.Visible = False
                '	Exit Function
                'End If

                'If Trim(m_str_Pri_City) = "" Then
                '	MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Primary Address.", MessageBoxButtons.Stop)
                '	'Me.ButtonRetireesInfoSave.Visible = False
                '	Exit Function
                'End If

                'If Trim(m_str_Pri_Address1) <> "" Then
                '	If m_str_Pri_CountryText = "-Select Country-" And m_str_Pri_StateText = "-Select State-" Then
                '		MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
                '		'Me.ButtonRetireesInfoSave.Visible = False
                '		Exit Function
                '	End If
                'End If

                'If (m_str_Pri_CountryText = "-Select Country-") Then
                '	If m_str_Pri_StateText = "-Select State-" Then
                '		MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
                '		'Me.ButtonRetireesInfoSave.Visible = False
                '		Exit Function
                '	End If
                'End If

                'l_string_Message = ValidateCountrySelStateZip(m_str_Pri_CountryValue, m_str_Pri_StateValue, m_str_Pri_Zip)
                'If l_string_Message <> "" Then
                '	MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                '	'Me.ButtonRetireesInfoSave.Visible = False
                '	Exit Function
                'End If

                'If m_str_Pri_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Pri_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
                '	MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
                '	'Me.ButtonRetireesInfoSave.Visible = False
                '	Exit Function
                'End If

                ''Secondary Address Validations
                ''Ashutosh Patil as on 14-Mar-2007
                ''YREN - 3028,YREN-3029
                'If Trim(m_str_Sec_Address1) <> "" Then
                '	If Trim(m_str_Sec_City) = "" Then
                '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Secondary Address.", MessageBoxButtons.Stop)
                '		'Me.ButtonRetireesInfoSave.Visible = False
                '		Exit Function
                '	End If
                '	If (m_str_Sec_CountryText = "-Select Country-") Then
                '		If m_str_Sec_StateText = "-Select State-" Then
                '			MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Secondary Address.", MessageBoxButtons.Stop)
                '			'Me.ButtonRetireesInfoSave.Visible = False
                '			Exit Function
                '		End If
                '	End If
                'End If

                'If Trim(m_str_Sec_Address2) <> "" Then
                '	If Trim(m_str_Sec_Address1) = "" Then
                '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '		'Me.ButtonRetireesInfoSave.Visible = False
                '		Exit Function
                '	End If
                'End If

                'If Trim(m_str_Sec_Address3) <> "" Then
                '	If Trim(m_str_Sec_Address1) = "" Then
                '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '		'Me.ButtonRetireesInfoSave.Visible = False
                '		Exit Function
                '	End If
                'End If

                'If m_str_Sec_City <> "" Then
                '	If Trim(m_str_Sec_Address1.ToString) = "" Then
                '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '		'Me.ButtonRetireesInfoSave.Visible = False
                '		Exit Function
                '	End If
                'End If

                'If (m_str_Sec_CountryText <> "-Select Country-" Or m_str_Sec_StateText <> "-Select State-") Then
                '	If Trim(m_str_Sec_Address1) = "" Then
                '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '		'Me.ButtonRetireesInfoSave.Visible = False
                '		Exit Function
                '	End If
                'End If

                'If Trim(m_str_Sec_Zip) <> "" Then
                '	If Trim(m_str_Sec_Address1) = "" Then
                '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '		'Me.ButtonRetireesInfoSave.Visible = False
                '		Exit Function
                '	End If
                'End If

                'l_string_Message = ValidateCountrySelStateZip(m_str_Sec_CountryValue, m_str_Sec_StateValue, m_str_Sec_Zip)

                'If l_string_Message <> "" Then
                '	MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                '	'Me.ButtonRetireesInfoSave.Visible = False
                '	Exit Function
                'End If

                'If m_str_Sec_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Sec_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
                '	MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
                '	'Me.ButtonRetireesInfoSave.Visible = False
                '	Exit Function
                'End If
            End If

            ProcessPhonySSNo = "Validated"

        Catch ex As Exception
            Throw
        End Try
    End Function
    'Bhavna:Session comment of deathnotify function YRS 5.0-1432
    'Public Function DeathNotify() As Boolean
    '	Try
    '		'paramPersonOrFundEvent as String, param_PersonOrFundEventID as string , paramDeathDate  as  DateTime

    '		Dim l_int_returnStatus As Int16
    '		Dim l_string_Message As String
    '		Dim l_date_DeathDate As DateTime

    '		''SR:2011.04.28 - YRS 5.0-1292: two variables are defined to store value of output parameters for retires and non retired participants
    '		Dim l_string_returnStatusDR As String = ""
    '		Dim l_string_returnStatusDA As String = ""

    '		l_string_Message = ""
    '		l_date_DeathDate = Session("DeathDate")
    '		DeathNotify = True

    '		''Call the Validation for DeathNotifyPrerequisites
    '		'BS:2012.03.02:YRS 5.0-1432:BT:941:- no neeed to display annuity and disbursment display message 
    '		'l_int_returnStatus = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DeathNotifyPrerequisites("PERSON", Session("PersId"), l_date_DeathDate, l_string_returnStatusDR, l_string_returnStatusDA)
    '		'If l_int_returnStatus < 0 Then
    '		'	MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification failed due to error Network Error:" + Chr(13) + "Cannot Proceed", MessageBoxButtons.OK)
    '		'	'Session("MessageDisplayed") = True
    '		'	DeathNotify = False
    '		'	Exit Function
    '		'End If
    '		'If (l_string_returnStatus = "1") And (l_string_returnStatus.Length = 1) Then
    '		'    l_string_Message = "There are disbursment checks that have been issued after the death date." + Chr(13)
    '		'    l_string_Message += "Please run the: 'List of Annuity Checks Sent to Retirees After Death' report"
    '		'    'Aparna IE7 08/10/2007
    '		'    MessageBox.Show(PlaceHolder1, "Note before you proceed.", l_string_Message, MessageBoxButtons.OK)
    '		'    Session("Call_FinalDeathNotificationUpdation") = "Yes"
    '		'    'Session("MessageDisplayed") = True
    '		'    DeathNotify = False
    '		'    Exit Function
    '		'ElseIf (l_string_returnStatus <> "0") And (l_string_returnStatus.Length <> 1) Then '//lcResult<>"0" AND LEN(lcresult)>2
    '		'    '	There is some error text .. We are replicating what has been done in foxpro 
    '		'    'Aparna IE7 08/10/2007
    '		'    MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification failed due to error:" + Chr(13) + l_string_returnStatus, MessageBoxButtons.OK)
    '		'    'Session("MessageDisplayed") = True
    '		'    DeathNotify = False
    '		'    Exit Function
    '		'    'Put this Code on the UI 
    '		'End If

    '		'Commented and added below lines of code by SR : 2010.01.04 for gemini 635
    '		'BS:2012.03.02:YRS 5.0-1432:BT:941:- no neeed to display annuity and disbursment display message
    '		''SR:2011.04.28 - YRS 5.0-1292 
    '		'Session("DNMessageforDR") = l_string_returnStatusDR

    '		'If (l_string_returnStatusDA <> "") Then
    '		'	MessageBox.Show(PlaceHolder1, "Note before you proceed.", l_string_returnStatusDA, MessageBoxButtons.OK) ''SR:2011.04.27 - YRS 5.0 1292 : OK button For Non retired participants validation message
    '		'	Session("Call_FinalDeathNotificationUpdation") = "Yes"
    '		'	DeathNotify = False
    '		'	Exit Function
    '		'End If

    '		''If (l_string_returnStatusDR <> "") Then
    '		''	MessageBox.Show(PlaceHolder1, "Note before you proceed.", l_string_returnStatusDR + " Do you wish to proceed?", MessageBoxButtons.YesNo) ''SR:2011.04.27 - YRS 5.0 1292 : OK button replaced by YES/NO button
    '		''	Session("Call_FinalDeathNotificationUpdation") = "Yes"
    '		''	DeathNotify = False
    '		''	Exit Function
    '		''End If

    '		'BS:2012.03.02:YRS 5.0-1432:BT:941:- no neeed to display annuity and disbursment display message
    '		''BS:2012.02.18:BT-941,YRS 5.0-1432:- here this message "Annuity payments after the date of death exist and might need to be reversed." has been removed instead of this display report List_of_Annuity_Checks_Sent_to_Retirees_After_Death.rpt
    '		'If (l_string_returnStatusDR <> "") Then
    '		'	'BS:2012.02.20:BT-941,YRS 5.0-1432:-here we have set variable IsStatusDR = true which will be check on FinalDeathNotificationUpdation() to open report
    '		'	Me.IsStatusDR = True
    '		'	Session("Call_FinalDeathNotificationUpdation") = "Yes"
    '		'	Exit Function
    '		'End If
    '		''Ends,SR:2011.04.28 - YRS 5.0-1292 

    '		'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.
    '		''Call the routine to do the final Updation 
    '		If FinalDeathNotificationUpdation() = False Then
    '			DeathNotify = False
    '		End If
    '		'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.

    '		'Death notification failed due to error:' + CHR(13)
    '	Catch
    '		Throw
    '	End Try
    'End Function


    'BS:2012.02.01:BT-941,YRS 5.0-1432:- ProcessReport() use for pass parameter ReportName and FundNo. to report viewer
    Private Sub ProcessReport()
        Try

            'Set the Report Variables in Session
            'Session("strReportName") = "List_of_Annuity_Checks_Sent_to_Retirees_After_Death"
            Session("strReportName") = "List of Uncashed Checks and Payments Issued After the Date of Death"
            If Not TextBoxFundNo.Text = String.Empty Then
                Session("FundNo") = TextBoxFundNo.Text
            End If

            'Call ReportViewer.aspx 
            Me.OpenReportViewer()

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'BS:2012.02.01:BT-941,YRS 5.0-1432:- OpenReportViewer() use to open ReportViewer.aspx 
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
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.
    Private Function FinalDeathNotificationUpdation() As Boolean
        Try
            Dim l_string_returnStatus As String
            'BS:2012.03.02:BT-941,YRS 5.0-1432:declare out variable
            Dim l_string_showreport As String = String.Empty
            Dim l_string_returnDeathNotifystatus As String = String.Empty 'Dinesh K:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
            Dim l_int_returnStatus As Int16
            Dim l_string_Message As String
            Dim l_date_DeathDate As DateTime

            l_string_returnStatus = ""
            l_string_Message = ""
            l_string_returnDeathNotifystatus = ""
            FinalDeathNotificationUpdation = True
            l_date_DeathDate = Session("DeathDate")

            'IB:-BT:532 on 09/June/2010 validate page before Death Notifcation saving
            Page.Validate()
            If Page.IsValid() = False Then
                FinalDeathNotificationUpdation = False
                Exit Function
            End If
            'BS:2012.03.03:BT-941,YRS 5.0-1432:add new out variable for open report

            'Dinesh K:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death
            'Add Paramter
            l_int_returnStatus = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DeathNotifyActions("PERSON", Session("PersId"), l_date_DeathDate, l_string_returnStatus, l_string_returnDeathNotifystatus, String.Empty, Me.TextBoxGeneralSSNo.Text, l_string_showreport)  'NP:IVP1:2008.04.15 - Adding parameter value TreatMLAs to determine how to treat ML. However, only retired participants can get to this screen. So setting the value as string.empty
            If l_int_returnStatus < 0 Then
                'Session("MessageDisplayed") = True
                FinalDeathNotificationUpdation = False
                'Aparna IE7 08/10/2007
                MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification failed due to error Network Error:" + Chr(13) + "Cannot Proceed", MessageBoxButtons.OK)
                Exit Function
            End If

            'Dinesh K:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
            'Anudeep:21.06.2013 : FOR allowing to open report BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
            'If l_string_returnDeathNotifystatus <> "" Then
            '    MessageBox.Show(PlaceHolder1, "Death Notification Status.", Chr(13) + l_string_returnDeathNotifystatus, MessageBoxButtons.OK)
            '    FinalDeathNotificationUpdation = False
            '    Me.TextBoxGeneralDateDeceased.Text = Session("DeathDate")
            '    Exit Function
            'End If
            Dim strMessage As String = String.Empty
            If (l_string_returnStatus <> "") OrElse l_int_returnStatus = 0 Then 'NP:PS:2007.10.03 - Updating code to check for the correct status.
                'Session("MessageDisplayed") = True
                FinalDeathNotificationUpdation = False
                'Aparna IE7 08/10/2007
                'Anudeep:21.06.2013 : FOR allowing to open report BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
                'MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification failed due to error:" + Chr(13) + l_string_returnStatus, MessageBoxButtons.OK)
                If l_string_returnDeathNotifystatus <> "" Then
                    'Added by Dinesh Kanojia on 19/08/2013
                    'YRS 5.0-1698:Cross check SSN when entering a date of death
                    strMessage = "<b>Death notification failed due to error:</b><br/>" + l_string_returnStatus + "<br/>" + l_string_returnDeathNotifystatus
                    'MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification failed due to error:" + Chr(13) + l_string_returnStatus + "<br/>" + l_string_returnDeathNotifystatus, MessageBoxButtons.OK)
                    Page.RegisterStartupScript("Death Notification Status.", "<script language='javascript'> openDialogDeathNotify('" + strMessage + "');</script>")
                Else
                    'Added by Dinesh Kanojia on 19/08/2013
                    'YRS 5.0-1698:Cross check SSN when entering a date of death
                    strMessage = "<b>Death notification failed due to error:</b><br/>" + l_string_returnStatus
                    'MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification failed due to error:" + Chr(13) + l_string_returnStatus, MessageBoxButtons.OK)
                    Page.RegisterStartupScript("Death Notification Status.", "<script language='javascript'> openDialogDeathNotify('" + strMessage + "');</script>")
                End If
                Exit Function
            Else
                'Session("MessageDisplayed") = True
                FinalDeathNotificationUpdation = True
                'BS:2012.03.21:For BT-1015,YRS 5.0-1557:Clear Session
                Session("CurrentProcesstoConfirm") = ""
                'Aparna IE7 08/10/2007

                'Anudeep:21.06.2013 : FOR allowing to open report BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
                'MessageBox.Show(PlaceHolder1, "Death Notification Status.", "'Death notification succeeded'", MessageBoxButtons.OK)
                If l_string_returnDeathNotifystatus <> "" Then
                    'Added by Dinesh Kanojia on 19/08/2013
                    'YRS 5.0-1698:Cross check SSN when entering a date of death
                    strMessage = "<b>Death notification succeeded.</b>" + "<br/>" + l_string_returnDeathNotifystatus
                    'MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification succeeded." + Chr(13) + "<br/>" + "<br/>" + l_string_returnDeathNotifystatus, MessageBoxButtons.OK)
                    'Page.RegisterStartupScript("Death Notification Status.", "<script language='javascript'> openDialogDeathNotify('" + strMessage + "');</script>") 'Bala: 01/05/2016: YRS-AT-1972 : calling it once after message modification is done
                Else
                    'Added by Dinesh Kanojia on 19/08/2013
                    'YRS 5.0-1698:Cross check SSN when entering a date of death
                    strMessage = "<b>Death notification succeeded.</b>"
                    'MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification succeeded.", MessageBoxButtons.OK)
                    'Page.RegisterStartupScript("Death Notification Status.", "<script language='javascript'> openDialogDeathNotify('" + strMessage + "');</script>") 'Bala: 01/05/2016: YRS-AT-1972 : calling it once after message modification is done
                End If
                'Start: Bala: 01/05/2016: YRS-AT-1972: showing the message accordingly to the special death process.
                If chkSpecialDeathProcess.Checked Then
                    strMessage = String.Format("{0}<br/>Special death processing required. Please refer to the notes for further information.", strMessage)
                    MailUtil.SendMail(EnumEmailTemplateTypes.SPECIAL_DEATH_PROCESSING_REQUIRED, "", "", "", "", New Dictionary(Of String, String) From {{"FundNo", Session("FundNo").ToString()}, {"FirstName", TextBoxGeneralFirstName.Text}, {"LastName", TextBoxGeneralLastName.Text}}, "", Nothing, Web.Mail.MailFormat.Html)
                End If
                Page.RegisterStartupScript("Death Notification Status.", "<script language='javascript'> openDialogDeathNotify('" + strMessage + "');</script>")
                'End: Bala: 01/05/2016: YRS-AT-1972: showing the message accordingly to the special death process.
                Me.TextBoxGeneralDateDeceased.Text = Session("DeathDate")
                'BS:2012.03.02:BT-941,YRS 5.0-1432:- if l_string_showreport is zero  then report will be open it has been set by zero on proc level  
                If (l_string_showreport = "0") Then
                    ProcessReport() 'call ProcessReport to open report: List of Uncashed Checks and Payments Issued After the Date of Death.rpt
                End If
                'After Succesfully death notification completed and surviour is not death beneficiary the open death benefit application form
                'If Session("ShowDeathBenefitForm") = "Yes" Then
                '    'SaveFormDetails() 'save the details and open the db form
                'End If
                'End :Anudeep:Added follwing code for Bt-1303 YRS 5.0-1707:New Death Benefit Application form 
                Exit Function
            End If
        Catch
            Throw
        End Try
    End Function


    'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.
    'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.

    Public Sub updateAddressInfo(ByVal paramAddrId As String, ByVal paramEntityId As String, ByVal paramAddr1 As String, ByVal paramAddr2 As String, ByVal paramAddr3 As String, ByVal paramCity As String, ByVal paramStateType As String, ByVal paramCountry As String, ByVal paramZip As String, ByVal BitActive As Boolean, ByVal BitPrimary As Boolean, ByVal paramEffdate As String, ByVal paramCreator As String, ByVal paramUpdater As String)
        Try
            If Not (paramAddrId = "" And paramAddr1 = "" And paramAddr2 = "" And paramAddr3 = "" And paramCity = "" And paramCountry = "" And paramZip = "") Then
                'YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateParticipantAddress(paramAddrId, paramEntityId, paramAddr1, paramAddr2, paramAddr3, paramCity, paramStateType, paramCountry, paramZip, BitActive, BitPrimary, paramEffdate, paramCreator, paramUpdater)
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Sub UpdateTelephoneInfo(ByVal paramTelephoneId As String, ByVal paramEntityId As String, ByVal paramTelephone As String, ByVal BitActive As Boolean, ByVal BitPrimary As Boolean, ByVal paramEffdate As String, ByVal paramCreator As String, ByVal paramUpdater As String)
        Try
            If Not (paramTelephoneId = "" And paramTelephone = "") Then
                '' YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(paramTelephoneId, paramEntityId, paramTelephone, BitActive, BitPrimary, paramEffdate, paramCreator, paramUpdater)
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Sub UpdateEmailInfo(ByVal paramMailId As String, ByVal paramEntityId As String, ByVal paramEmail As String, ByVal BitActive As Boolean, ByVal BitPrimary As Boolean, ByVal paramEffdate As String, ByVal paramCreator As String, ByVal paramUpdater As String)
        Try
            If Not (paramMailId = "" And paramEmail = "") Then
                '' YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateEmailAddress(paramMailId, paramEntityId, paramEmail, BitActive, BitPrimary, paramEffdate, paramCreator, paramUpdater)
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Sub UpdateEmailProperties(ByVal paramUniqueId As String, ByVal paramEntityId As String, ByVal paramBadEmail As Boolean, ByVal paramUnsubscribed As Boolean, ByVal paramTextOnly As Boolean, ByVal BitActive As Boolean, ByVal BitPrimary As Boolean, ByVal paramSecondaryActive As Boolean, ByVal paramCreator As String, ByVal paramUpdater As String)
        Try
            If Not (paramUniqueId = "") Then
                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateEmailProperties(paramUniqueId, paramEntityId, paramBadEmail, paramUnsubscribed, paramTextOnly, BitActive, BitPrimary, paramSecondaryActive, paramCreator, paramUpdater)
            End If
        Catch
            Throw
        End Try
    End Sub


    Public Sub Load_AllTabsData()
        Try
            LoadAnnuityPaidTab()
            LoadAnnuitiesTab()
            LoadBeneficiariesTab()
            LoadBanksTab()
            LoadFedWithDrawalTab()
            LoadGenWithDrawalTab()
            LoadNotesTab()
            LoadGeneraltab()
            SetGrossAmountToSTWUserControl() ' ML |10.24.2019| YRS-AT-4598 | Assign GrossAmount to State Withholding Control
        Catch
            Throw
        End Try
    End Sub

    Function UNSuppresseJSannuities()
        Dim l_string_PersId As String
        Dim l_DataTable As DataTable
        Dim l_stringmessage As String

        'START: PPP | 04/20/2016 | YRS-AT-2719
        Dim diDeductions As Dictionary(Of String, Decimal)
        Dim strXML As String
        'END: PPP | 04/20/2016 | YRS-AT-2719
        Try
            l_string_PersId = Session("PersId")

            'START: PPP | 04/20/2016 | YRS-AT-2719 | Retrieving the deductions and converting it in XML format
            diDeductions = TryCast(HttpContext.Current.Session("JSAnnuityDeductions"), Dictionary(Of String, Decimal))
            If diDeductions Is Nothing Then
                diDeductions = New Dictionary(Of String, Decimal)()
            End If
            strXML = ConvertDeductionsIntoXML(diDeductions)

            'l_DataTable = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UnSuppressJSAnnuitiesCount(l_string_PersId)
            l_DataTable = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UnSuppressJSAnnuitiesCount(l_string_PersId, strXML)
            'END: PPP | 04/20/2016 | YRS-AT-2719 | Retrieving the deductions and converting it in XML format
            If Not l_DataTable Is Nothing Then
                If l_DataTable.Rows.Count > 0 Then
                    l_stringmessage = CType(l_DataTable.Rows(0)(0), String).Trim
                    'Aparna IE7 08/10/2007
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_stringmessage.Trim, MessageBoxButtons.OK)
                    'Priya/Ashish: 22-April-10:YRS 5.0-1056: Additional info when unsuppressing a JSANN annuity
                    'Add getSuppressJSAnnuityCount() method to disable button
                    getSuppressJSAnnuityCount()

                End If
            Else
                Session("CurrentProcesstoConfirm") = ""
                getSuppressJSAnnuityCount()
            End If

        Catch ex As Exception
            'Aparna IE7 08/10/2007
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", ex.Message.Trim, MessageBoxButtons.Stop)
        End Try
    End Function
    'BS:2012:04:19:-restructure address code
    'Private Sub UpdateAddress(ByVal bitPrimary1 As Boolean, ByVal bitActive1 As Boolean, ByVal bitPrimary2 As Boolean, ByVal bitActive2 As Boolean)
    '    Try
    '        '  If Me.CheckBoxActive.Checked = False Then
    '        ''This change is required to remove the checkbox to activate secondary as primary
    '        ''Now button will be used in its place and the this function will be called from two places Save button 
    '        '' and 
    '        Dim l_ds_PrimaryAddress As DataSet
    '        Dim l_ds_SecondaryAddress As DataSet
    '        Dim l_stringMessage As String = String.Empty

    '        'Ashutosh Patil as on 26-Mar-2007
    '        'YREN-3029, YREN - 3028	
    '        Call SetPrimaryAddressDetails()
    '        Call SetSecondaryAddressDetails()

    '        'l_stringMessage = Me.ValidataPrimaryAddress() 'by Aparna to avoid the blank primary address 12/11/2007
    '        'If l_stringMessage <> String.Empty Then
    '        '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_stringMessage, MessageBoxButtons.Stop)
    '        '    Exit Sub
    '        'End If

    '        Dim l_str_Country As String

    '        If Not Session("Ds_PrimaryAddress") Is Nothing Then
    '            l_ds_PrimaryAddress = DirectCast(Session("Ds_PrimaryAddress"), DataSet)
    '            'Primary Address section starts
    '            If l_ds_PrimaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
    '                If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address1").ToString.Trim() <> m_str_Pri_Address1 Then
    '                    'l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address1") = Me.TextBoxAddress1.Text
    '                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address1") = Me.m_str_Pri_Address1
    '                End If
    '                If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address2").ToString.Trim() <> m_str_Pri_Address2 Then
    '                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address2") = Me.m_str_Pri_Address2 'Me.TextBoxAddress2.Text
    '                End If
    '                If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address3").ToString.Trim <> m_str_Pri_Address3 Then
    '                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address3") = Me.m_str_Pri_Address3 'Me.TextBoxAddress3.Text
    '                End If
    '                If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("City").ToString.Trim <> m_str_Pri_City Then
    '                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("City") = Me.m_str_Pri_City  'Me.TextBoxCity.Text
    '                End If

    '                'commented and modified by hafiz on 22-Nov-2006 for YREN-2882
    '                'If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Country").ToString.Trim <> Me.DropdownlistCountry.SelectedValue And Me.DropdownlistCountry.SelectedValue <> "" Then
    '                If m_str_Pri_CountryValue = "-Select Country-" Then
    '                    m_str_Pri_CountryValue = ""
    '                End If
    '                '      Me.DropdownlistCountry.SelectedValue Then
    '                If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Country").ToString.Trim <> m_str_Pri_CountryValue Then '      Me.DropdownlistCountry.SelectedValue Then
    '                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Country") = m_str_Pri_CountryValue 'Me.DropdownlistCountry.SelectedValue
    '                End If

    '                'commented and modified by hafiz on 22-Nov-2006 for YREN-2882
    '                'If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim <> Me.DropdownlistState.SelectedValue And Me.DropdownlistState.SelectedValue <> "" Then
    '                If m_str_Pri_StateValue = "-Select State-" Then
    '                    m_str_Pri_StateValue = ""
    '                End If
    '                If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim <> m_str_Pri_StateValue Then
    '                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("State") = m_str_Pri_StateValue 'Me.DropdownlistState.SelectedValue
    '                End If

    '                If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Zip").ToString.Trim <> m_str_Pri_Zip Then
    '                    l_str_Country = String.Empty
    '                    l_str_Country = m_str_Pri_CountryValue
    '                    If l_str_Country.ToString = "CA" Then
    '                        l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Zip") = Replace(m_str_Pri_Zip, " ", "") 'Me.TextBoxZip.Text
    '                    Else
    '                        l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Zip") = m_str_Pri_Zip
    '                    End If
    '                End If
    '	If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString.Trim <> Me.m_str_Pri_EffDate Then
    '		l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate") = Me.m_str_Pri_EffDate
    '	End If
    '                If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary")) <> bitPrimary1 Then
    '                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary") = bitPrimary1
    '                End If
    '                If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive")) <> bitActive1 Then
    '                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive") = bitActive1
    '                End If
    '                If Not IsDBNull(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) Then
    '                    If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) <> Me.CheckboxIsBadAddress.Checked Then
    '                        If Me.CheckboxIsBadAddress.Checked = True Then
    '                            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = True
    '                        Else
    '                            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
    '                        End If
    '                    End If
    '                Else
    '                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
    '                End If
    '            Else
    '	'If Me.TextBoxAddress1.Text <> "" Or Me.TextBoxAddress2.Text <> "" Or Me.TextBoxAddress3.Text <> "" Or Me.TextBoxCity.Text <> "" Or Me.TextBoxZip.Text <> "" Or Me.TextBoxPriEffDate.Text <> "" Then
    '                'By Ashutosh Patil as on 25-Apr-2007
    '                'YREN-3298
    '	If m_str_Pri_Address1 <> "" Or m_str_Pri_Address2 <> "" Or m_str_Pri_Address3 <> "" Or m_str_Pri_City <> "" Or m_str_Pri_Zip <> "" Or Me.m_str_Pri_EffDate <> "" Then
    '		Dim l_PrimaryAddressRow As DataRow
    '		l_PrimaryAddressRow = l_ds_PrimaryAddress.Tables("AddressInfo").NewRow
    '		If m_str_Pri_CountryValue = "-Select Country-" Then
    '			m_str_Pri_CountryValue = ""
    '		End If
    '		If m_str_Pri_StateValue = "-Select State-" Then
    '			m_str_Pri_StateValue = ""
    '		End If
    '		l_PrimaryAddressRow.Item("EntityId") = Session("PersId")
    '		'Ashutosh Patil as on 26-Mar-2007
    '		'YREN - 3028,YREN-3029
    '		l_PrimaryAddressRow.Item("Address1") = m_str_Pri_Address1  'Me.TextBoxAddress1.Text
    '		l_PrimaryAddressRow.Item("Address2") = m_str_Pri_Address2 'Me.TextBoxAddress2.Text
    '		l_PrimaryAddressRow.Item("Address3") = m_str_Pri_Address3 'Me.TextBoxAddress3.Text
    '		l_PrimaryAddressRow.Item("City") = m_str_Pri_City  'Me.TextBoxCity.Text
    '		l_PrimaryAddressRow.Item("Country") = m_str_Pri_CountryValue  'Me.DropdownlistCountry.SelectedValue
    '		l_PrimaryAddressRow.Item("State") = m_str_Pri_StateValue  'Me.DropdownlistState.SelectedValue
    '		l_str_Country = String.Empty
    '		l_str_Country = m_str_Pri_CountryValue
    '		If l_str_Country.ToString = "CA" Then
    '			l_PrimaryAddressRow.Item("Zip") = Replace(m_str_Pri_Zip, " ", "")
    '		Else
    '			l_PrimaryAddressRow.Item("Zip") = m_str_Pri_Zip
    '		End If
    '		'l_PrimaryAddressRow.Item("Zip") = m_str_Pri_Zip  '
    '		l_PrimaryAddressRow.Item("EffDate") = Me.m_str_Pri_EffDate
    '		l_PrimaryAddressRow.Item("bitPrimary") = bitPrimary1
    '		l_PrimaryAddressRow.Item("bitActive") = bitActive1
    '		If Me.CheckboxIsBadAddress.Checked = True Then
    '			l_PrimaryAddressRow.Item("bitBadAddress") = True
    '		Else
    '			l_PrimaryAddressRow.Item("bitBadAddress") = False
    '		End If
    '		l_ds_PrimaryAddress.Tables("AddressInfo").Rows.Add(l_PrimaryAddressRow)
    '	End If

    '            End If
    '            If Not l_ds_PrimaryAddress.Tables("AddressInfo") Is Nothing Then
    '                If l_ds_PrimaryAddress.Tables("AddressInfo").Rows.Count > 0 Then

    '                    If Not l_ds_PrimaryAddress.Tables("AddressInfo").GetChanges Is Nothing Then
    '                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateParticipantAddress(l_ds_PrimaryAddress)
    '                    End If
    '                End If

    '            End If

    '            'Primary Address section ends
    '            'Primary Telephone section starts
    '            Dim l_datarow As DataRow
    '            Dim l_Bool_Home As Boolean = False
    '            Dim l_Bool_Work As Boolean = False
    '            Dim l_Bool_Fax As Boolean = False
    '            Dim l_Bool_Mobile As Boolean = False
    '            If l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
    '                If Not l_ds_PrimaryAddress.Tables("TelephoneInfo") Is Nothing Then
    '                    ''For Home Phone

    '                    For Each l_datarow In l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows
    '                        If (l_datarow("PhoneType").ToString() <> "System.DBNull" And l_datarow("PhoneType").ToString() <> "") Then
    '                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "HOME" Then
    '                                If l_datarow("PhoneNumber").ToString.Trim <> TextBoxHome.Text.Trim Then
    '                                    l_datarow("PhoneNumber") = TextBoxHome.Text.Trim
    '                                    'By Aparna -yren -3026
    '                                    l_datarow("EffDate") = Date.Now()
    '                                End If
    '                                l_Bool_Home = True
    '                            End If
    '                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "OFFICE" Then
    '                                If l_datarow("PhoneNumber").ToString.Trim <> Me.TextboxTelephone.Text.Trim Then
    '                                    l_datarow("PhoneNumber") = Me.TextboxTelephone.Text.Trim
    '                                    'By Aparna -yren -3026
    '                                    l_datarow("EffDate") = Date.Now()
    '                                End If
    '                                l_Bool_Work = True
    '                            End If
    '                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "FAX" Then
    '                                If l_datarow("PhoneNumber").ToString.Trim <> Me.TextBoxFax.Text Then
    '                                    l_datarow("PhoneNumber") = Me.TextBoxFax.Text
    '                                    'By Aparna -yren -3026
    '                                    l_datarow("EffDate") = Date.Now()
    '                                End If
    '                                l_Bool_Fax = True
    '                            End If
    '                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "MOBILE" Then
    '                                If l_datarow("PhoneNumber").ToString.Trim <> Me.TextBoxMobile.Text Then
    '                                    l_datarow("PhoneNumber") = Me.TextBoxMobile.Text
    '                                    'By Aparna -yren -3026
    '                                    l_datarow("EffDate") = Date.Now()
    '                                End If
    '                                l_Bool_Mobile = True
    '                            End If
    '                            If Convert.ToBoolean(l_datarow("bitPrimary")) <> bitPrimary1 Then
    '                                l_datarow("bitPrimary") = bitPrimary1
    '                                'By Aparna -yren -3026
    '                                l_datarow("EffDate") = Date.Now()
    '                            End If
    '                            If Convert.ToBoolean(l_datarow("bitActive")) <> bitActive1 Then
    '                                l_datarow("bitActive") = bitActive1
    '                                'By Aparna -yren -3026
    '                                l_datarow("EffDate") = Date.Now()
    '                            End If
    '                            'commented by aparna -yren -3026 Wrong check done
    '                            'If l_datarow("EffDate").tostring.trim <> Me.TextBoxPriEffDate.Text.ToString.Trim() Then
    '                            '    l_datarow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '                            'End If
    '                            'commented by aparna -yren -3026 Wrong check done


    '                        End If
    '                    Next
    '                    If l_Bool_Home = False And TextBoxHome.Text.Trim.Length > 0 Then
    '                        Dim l_HomeRow As DataRow
    '                        l_HomeRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '                        l_HomeRow("PhoneType") = "HOME"
    '                        l_HomeRow("PhoneNumber") = Me.TextBoxHome.Text.Trim
    '                        l_HomeRow("EntityId") = Session("PersId")
    '                        l_HomeRow("bitPrimary") = bitPrimary1
    '                        l_HomeRow("bitActive") = bitActive1
    '                        'By Aparna -yren -3026
    '                        l_HomeRow("EffDate") = Date.Now()
    '                        'l_HomeRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '                        l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
    '                    End If

    '                    If l_Bool_Work = False And TextboxTelephone.Text.Trim.Length > 0 Then
    '                        Dim l_WorkRow As DataRow
    '                        l_WorkRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '                        l_WorkRow("PhoneType") = "OFFICE"
    '                        l_WorkRow("PhoneNumber") = Me.TextboxTelephone.Text.Trim
    '                        l_WorkRow("EntityId") = Session("PersId")
    '                        l_WorkRow("bitPrimary") = bitPrimary1
    '                        l_WorkRow("bitActive") = bitActive1
    '                        'By Aparna -yren -3026
    '                        l_WorkRow("EffDate") = Date.Now()
    '                        ' l_WorkRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '                        l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
    '                    End If

    '                    If l_Bool_Fax = False And TextBoxFax.Text.Trim.Length > 0 Then
    '                        Dim l_FaxRow As DataRow
    '                        l_FaxRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '                        l_FaxRow("PhoneType") = "FAX"
    '                        l_FaxRow("PhoneNumber") = Me.TextBoxFax.Text.Trim
    '                        l_FaxRow("EntityId") = Session("PersId")
    '                        l_FaxRow("bitPrimary") = bitPrimary1
    '                        l_FaxRow("bitActive") = bitActive1
    '                        'By Aparna -yren -3026
    '                        l_FaxRow("EffDate") = Date.Now()
    '                        'l_FaxRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '                        l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
    '                    End If

    '                    If l_Bool_Mobile = False And TextBoxMobile.Text.Trim.Length > 0 Then
    '                        Dim l_MobileRow As DataRow
    '                        l_MobileRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '                        l_MobileRow("PhoneType") = "MOBILE"
    '                        l_MobileRow("PhoneNumber") = Me.TextBoxMobile.Text.Trim
    '                        l_MobileRow("EntityId") = Session("PersId")
    '                        l_MobileRow("bitPrimary") = bitPrimary1
    '                        l_MobileRow("bitActive") = bitActive1
    '                        'By Aparna -yren -3026
    '                        l_MobileRow("EffDate") = Date.Now()
    '                        'l_MobileRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '                        l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
    '                    End If

    '                End If
    '            Else
    '                If TextBoxHome.Text.Trim.Length > 0 Then
    '                    Dim l_HomeRow As DataRow
    '                    l_HomeRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '                    l_HomeRow("PhoneType") = "HOME"
    '                    l_HomeRow("PhoneNumber") = Me.TextBoxHome.Text.Trim
    '                    l_HomeRow("EntityId") = Session("PersId")
    '                    l_HomeRow("bitPrimary") = bitPrimary1
    '                    l_HomeRow("bitActive") = bitActive1
    '                    'By Aparna -yren -3026
    '                    l_HomeRow("EffDate") = Date.Now()
    '                    ' l_HomeRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '                    l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
    '                End If

    '                If TextboxTelephone.Text.Trim.Length > 0 Then
    '                    Dim l_WorkRow As DataRow
    '                    l_WorkRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '                    l_WorkRow("PhoneType") = "OFFICE"
    '                    l_WorkRow("PhoneNumber") = Me.TextboxTelephone.Text.Trim
    '                    l_WorkRow("EntityId") = Session("PersId")
    '                    l_WorkRow("bitPrimary") = bitPrimary1
    '                    l_WorkRow("bitActive") = bitActive1
    '                    'By Aparna -yren -3026
    '                    l_WorkRow("EffDate") = Date.Now()
    '                    'l_WorkRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '                    l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
    '                End If

    '                If TextBoxFax.Text.Trim.Length > 0 Then
    '                    Dim l_FaxRow As DataRow
    '                    l_FaxRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '                    l_FaxRow("PhoneType") = "FAX"
    '                    l_FaxRow("PhoneNumber") = Me.TextBoxFax.Text.Trim
    '                    l_FaxRow("EntityId") = Session("PersId")
    '                    l_FaxRow("bitPrimary") = bitPrimary1
    '                    l_FaxRow("bitActive") = bitActive1
    '                    'By Aparna -yren -3026
    '                    l_FaxRow("EffDate") = Date.Now()
    '                    ' l_FaxRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '                    l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
    '                End If

    '                If TextBoxMobile.Text.Trim.Length > 0 Then
    '                    Dim l_MobileRow As DataRow
    '                    l_MobileRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '                    l_MobileRow("PhoneType") = "MOBILE"
    '                    l_MobileRow("PhoneNumber") = Me.TextBoxMobile.Text.Trim
    '                    l_MobileRow("EntityId") = Session("PersId")
    '                    l_MobileRow("bitPrimary") = bitPrimary1
    '                    l_MobileRow("bitActive") = bitActive1
    '                    'By Aparna -yren -3026
    '                    l_MobileRow("EffDate") = Date.Now()
    '                    'l_MobileRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '                    l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
    '                End If

    '            End If
    '            If Not l_ds_PrimaryAddress.Tables("TelephoneInfo") Is Nothing Then
    '                If l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
    '                    If Not l_ds_PrimaryAddress.Tables("TelephoneInfo").GetChanges Is Nothing Then
    '                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(l_ds_PrimaryAddress)
    '                    End If
    '                End If
    '            End If


    '            ''Primary Telephone Section ends
    '            ''Email Section Starts

    '            If l_ds_PrimaryAddress.Tables("EmailInfo").Rows.Count > 0 Then
    '                If l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("EmailAddress").ToString.Trim <> Me.TextBoxEmailId.Text Then
    '                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("EmailAddress") = Me.TextBoxEmailId.Text
    '                End If
    '                If Not IsDBNull(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress")) Then
    '                    If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress")) <> CheckboxBadEmail.Checked Then
    '                        If Me.CheckboxBadEmail.Checked = True Then
    '                            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress") = True
    '                        Else
    '                            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress") = False
    '                        End If
    '                    End If
    '                Else
    '                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress") = False
    '                End If
    '                If Not IsDBNull(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed")) Then
    '                    If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed")) <> Me.CheckboxUnsubscribe.Checked Then
    '                        If Me.CheckboxUnsubscribe.Checked = True Then
    '                            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed") = True
    '                        Else
    '                            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed") = False
    '                        End If
    '                    End If
    '                Else
    '                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed") = False
    '                End If

    '                If Not IsDBNull(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly")) Then
    '                    If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly")) <> Me.CheckboxTextOnly.Checked Then
    '                        If Me.CheckboxTextOnly.Checked = True Then
    '                            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly") = True
    '                        Else
    '                            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly") = False
    '                        End If
    '                    End If
    '                Else
    '                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly") = False
    '                End If


    '                If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("bitActive")) <> True Then
    '                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("bitActive") = True
    '                End If
    '                If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("bitPrimary")) <> True Then
    '                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("bitPrimary") = True
    '                End If
    '                If IsDBNull(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("EffDate")) Then
    '                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("EffDate") = DateTime.Now
    '                End If


    '            Else
    '                If Me.TextBoxEmailId.Text.Trim.Length > 0 Then
    '                    Dim l_PrimaryEmailRow As DataRow
    '                    l_PrimaryEmailRow = l_ds_PrimaryAddress.Tables("EmailInfo").NewRow
    '                    l_PrimaryEmailRow.Item("EntityId") = Session("PersId")
    '                    l_PrimaryEmailRow.Item("EmailAddress") = Me.TextBoxEmailId.Text
    '                    If Me.CheckboxBadEmail.Checked = True Then
    '                        l_PrimaryEmailRow.Item("IsBadAddress") = True
    '                    Else
    '                        l_PrimaryEmailRow.Item("IsBadAddress") = False
    '                    End If
    '                    If Me.CheckboxUnsubscribe.Checked = True Then
    '                        l_PrimaryEmailRow.Item("IsUnsubscribed") = True
    '                    Else
    '                        l_PrimaryEmailRow.Item("IsUnsubscribed") = False
    '                    End If
    '                    If Me.CheckboxTextOnly.Checked = True Then
    '                        l_PrimaryEmailRow.Item("IsTextOnly") = True
    '                    Else
    '                        l_PrimaryEmailRow.Item("IsTextOnly") = False
    '                    End If

    '                    l_PrimaryEmailRow.Item("EffDate") = DateTime.Now()
    '                    l_PrimaryEmailRow.Item("bitPrimary") = True
    '                    l_PrimaryEmailRow.Item("bitActive") = True

    '                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows.Add(l_PrimaryEmailRow)
    '                End If

    '            End If
    '            If Not l_ds_PrimaryAddress.Tables("EmailInfo") Is Nothing Then
    '                If l_ds_PrimaryAddress.Tables("EmailInfo").Rows.Count > 0 Then
    '                    If Not l_ds_PrimaryAddress.Tables("EmailInfo").GetChanges Is Nothing Then
    '                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateEmailAddress(l_ds_PrimaryAddress)
    '                    End If
    '                End If
    '            End If



    '            ''email section ends

    '        End If 'Session is nothing
    '        If Not Session("Ds_SecondaryAddress") Is Nothing Then
    '            l_ds_SecondaryAddress = DirectCast(Session("Ds_SecondaryAddress"), DataSet)
    '            'Secondary Address section starts
    '            If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
    '                'Ashutosh Patil as on 26-Mar-2007
    '                'YREN - 3028,YREN-3029
    '                If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address1").ToString.Trim <> m_str_Sec_Address1 Then
    '                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address1") = m_str_Sec_Address1 ' Me.TextBoxSecAddress1.Text
    '                End If
    '                If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address2").ToString.Trim <> m_str_Sec_Address2 Then
    '                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address2") = m_str_Sec_Address2 ' Me.TextBoxSecAddress2.Text
    '                End If
    '                If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address3").ToString.Trim <> m_str_Sec_Address3 Then
    '                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address3") = m_str_Sec_Address3 'Me.TextBoxSecAddress3.Text
    '                End If
    '                If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("City").ToString.Trim <> m_str_Sec_City Then
    '                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("City") = m_str_Sec_City 'Me.TextBoxSecCity.Text
    '                End If
    '                If m_str_Sec_CountryValue = "-Select Country-" Then
    '                    m_str_Sec_CountryValue = ""
    '                End If
    '                If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Country").ToString.Trim <> m_str_Sec_CountryValue Then 'Me.DropdownlistSecCountry.SelectedValue Then
    '                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Country") = m_str_Sec_CountryValue 'Me.DropdownlistSecCountry.SelectedValue
    '                End If
    '                If m_str_Sec_StateValue = "-Select State-" Then
    '                    m_str_Sec_StateValue = ""
    '                End If
    '                If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim <> m_str_Sec_StateValue Then
    '                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("State") = m_str_Sec_StateValue ' Me.DropdownlistSecState.SelectedValue
    '                End If
    '                If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Zip").ToString.Trim <> m_str_Sec_Zip Then
    '                    'l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Zip") = m_str_Sec_Zip 'Me.TextBoxSecZip.Text
    '                    l_str_Country = String.Empty
    '                    l_str_Country = m_str_Sec_CountryValue
    '                    If l_str_Country.ToString = "CA" Then
    '                        l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Zip") = Replace(m_str_Sec_Zip, " ", "")
    '                    Else
    '                        l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Zip") = m_str_Sec_Zip
    '                    End If
    '                End If
    '	If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString.Trim <> m_str_Sec_EffDate Then
    '		l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate") = m_str_Sec_EffDate
    '	End If
    '                If Convert.ToBoolean(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary")) <> bitPrimary2 Then
    '                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary") = bitPrimary2
    '                End If
    '                If Convert.ToBoolean(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive")) <> bitActive2 Then
    '                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive") = bitActive2
    '                End If
    '                If Not IsDBNull(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) Then
    '                    If Convert.ToBoolean(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) <> Me.CheckboxSecIsBadAddress.Checked Then
    '                        If Me.CheckboxSecIsBadAddress.Checked = True Then
    '                            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = True
    '                        Else
    '                            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
    '                        End If
    '                    End If
    '                Else
    '                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
    '                End If


    '            Else
    '                'If Me.TextBoxSecAddress1.Text <> "" Or Me.TextBoxSecAddress2.Text <> "" Or Me.TextBoxSecAddress3.Text <> "" Or Me.TextBoxSecCity.Text <> "" Or Me.TextBoxSecZip.Text <> "" Or Me.TextBoxSecEffDate.Text <> "" Then
    '                'Ashutosh Patil as on 26-Mar-2007
    '                'YREN - 3028,YREN-3029
    '	If m_str_Sec_Address1 <> "" Or m_str_Sec_Address2 <> "" Or m_str_Sec_Address3 <> "" Or m_str_Sec_City <> "" Or m_str_Sec_Zip <> "" Or m_str_Sec_EffDate <> "" Then
    '		Dim l_SecondaryAddressRow As DataRow
    '		l_SecondaryAddressRow = l_ds_SecondaryAddress.Tables("AddressInfo").NewRow
    '		If m_str_Sec_CountryValue = "-Select Country-" Then
    '			m_str_Sec_CountryValue = ""
    '		End If
    '		If m_str_Sec_StateValue = "-Select State-" Then
    '			m_str_Sec_StateValue = ""
    '		End If
    '		l_SecondaryAddressRow.Item("EntityId") = Session("PersId")
    '		'Ashutosh Patil as on 26-Mar-2007
    '		'YREN - 3028,YREN-3029
    '		l_SecondaryAddressRow.Item("EntityId") = Session("PersId")
    '		l_SecondaryAddressRow.Item("Address1") = m_str_Sec_Address1	'Me.TextBoxSecAddress1.Text
    '		l_SecondaryAddressRow.Item("Address2") = m_str_Sec_Address2	'Me.TextBoxSecAddress2.Text
    '		l_SecondaryAddressRow.Item("Address3") = m_str_Sec_Address3	' Me.TextBoxSecAddress3.Text
    '		l_SecondaryAddressRow.Item("City") = m_str_Sec_City	'Me.TextBoxSecCity.Text
    '		l_SecondaryAddressRow.Item("Country") = m_str_Sec_CountryValue 'Me.DropdownlistSecCountry.SelectedValue
    '		l_SecondaryAddressRow.Item("State") = m_str_Sec_StateValue 'Me.DropdownlistSecState.SelectedValue
    '		l_str_Country = String.Empty
    '		l_str_Country = m_str_Sec_CountryValue
    '		If l_str_Country.ToString = "CA" Then
    '			l_SecondaryAddressRow.Item("Zip") = Replace(m_str_Sec_Zip, " ", "")	'Me.TextBoxSecZip.Text        
    '		Else
    '			l_SecondaryAddressRow.Item("Zip") = m_str_Sec_Zip
    '		End If
    '		l_SecondaryAddressRow.Item("EffDate") = m_str_Sec_EffDate
    '		l_SecondaryAddressRow.Item("bitPrimary") = bitPrimary2
    '		l_SecondaryAddressRow.Item("bitActive") = bitActive2
    '		If Me.CheckboxSecIsBadAddress.Checked = True Then
    '			l_SecondaryAddressRow.Item("bitBadAddress") = True
    '		Else
    '			l_SecondaryAddressRow.Item("bitBadAddress") = False
    '		End If
    '		l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Add(l_SecondaryAddressRow)
    '		'Added By Ashutosh Patil as on 15-Feb-2007
    '		'YREN-3028,YREN-3029
    '		'If user selects or adds only state or country for address updation(rest fields are not added like email)
    '		'then in that case also that particular country or state is updated 
    '	Else
    '		Dim l_SecondaryAddressRow As DataRow
    '		Dim int_country_state As Integer = 0
    '		If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count = 0 Then
    '			'Ashutosh Patil as on 26-Mar-2007
    '			'YREN - 3028,YREN-3029
    '			'If Me.DropdownlistSecCountry.SelectedItem.Text <> "" And Me.DropdownlistSecCountry.SelectedItem.Text <> "-Select Country-" Then
    '			'    int_country_state = 1
    '			'End If
    '			'If Me.DropdownlistSecState.SelectedItem.Text <> "" And Me.DropdownlistSecState.SelectedItem.Text <> "-Select State-" Then
    '			'    int_country_state = 1
    '			'End If
    '			If m_str_Sec_CountryText <> "" And m_str_Sec_CountryText <> "-Select Country-" Then
    '				int_country_state = 1
    '			ElseIf m_str_Sec_StateText <> "" And m_str_Sec_StateText <> "-Select State-" Then
    '				int_country_state = 1
    '			End If
    '			If m_str_Sec_CountryValue = "-Select Country-" Then
    '				m_str_Sec_CountryValue = ""
    '			End If
    '			If m_str_Sec_StateValue = "-Select State-" Then
    '				m_str_Sec_StateValue = ""
    '			End If
    '			If int_country_state = 1 Then
    '				l_SecondaryAddressRow = l_ds_SecondaryAddress.Tables("AddressInfo").NewRow
    '				l_SecondaryAddressRow.Item("EntityId") = Session("PersId")
    '				'Ashutosh Patil as on 26-Mar-2007
    '				'YREN - 3028,YREN-3029
    '				l_SecondaryAddressRow.Item("EntityId") = Session("PersId")
    '				l_SecondaryAddressRow.Item("Address1") = m_str_Sec_Address1	'Me.TextBoxSecAddress1.Text
    '				l_SecondaryAddressRow.Item("Address2") = m_str_Sec_Address2	'Me.TextBoxSecAddress2.Text
    '				l_SecondaryAddressRow.Item("Address3") = m_str_Sec_Address3	' Me.TextBoxSecAddress3.Text
    '				l_SecondaryAddressRow.Item("City") = m_str_Sec_City	'Me.TextBoxSecCity.Text
    '				l_str_Country = String.Empty
    '				l_str_Country = m_str_Sec_CountryValue
    '				If l_str_Country.ToString = "CA" Then
    '					l_SecondaryAddressRow.Item("Zip") = Replace(m_str_Sec_Zip, " ", "")	'Me.TextBoxSecZip.Text        
    '				Else
    '					l_SecondaryAddressRow.Item("Zip") = m_str_Sec_Zip
    '				End If
    '				l_SecondaryAddressRow.Item("State") = m_str_Sec_StateValue 'Me.DropdownlistSecState.SelectedValue
    '				l_SecondaryAddressRow.Item("Zip") = m_str_Sec_Zip 'Me.TextBoxSecZip.Text
    '				l_SecondaryAddressRow.Item("EffDate") = m_str_Sec_EffDate
    '				l_SecondaryAddressRow.Item("bitPrimary") = bitPrimary2
    '				l_SecondaryAddressRow.Item("bitActive") = bitActive2
    '				If Me.CheckboxSecIsBadAddress.Checked = True Then
    '					l_SecondaryAddressRow.Item("bitBadAddress") = True
    '				Else
    '					l_SecondaryAddressRow.Item("bitBadAddress") = False
    '				End If
    '				l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Add(l_SecondaryAddressRow)
    '			End If
    '			int_country_state = 0
    '		End If

    '	End If

    '            End If
    '            If Not l_ds_SecondaryAddress.Tables("AddressInfo") Is Nothing Then
    '                If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
    '                    If Not l_ds_SecondaryAddress.Tables("AddressInfo").GetChanges Is Nothing Then
    '                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateParticipantAddress(l_ds_SecondaryAddress)
    '                    End If
    '                End If
    '            End If


    '            'SecondaryAddress section ends
    '            'Primary Telephone section starts
    '            Dim l_datarow As DataRow
    '            Dim l_Bool_Home As Boolean = False
    '            Dim l_Bool_Work As Boolean = False
    '            Dim l_Bool_Fax As Boolean = False
    '            Dim l_Bool_Mobile As Boolean = False
    '            If l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
    '                If Not l_ds_SecondaryAddress.Tables("TelephoneInfo") Is Nothing Then

    '                    ''For Home Phone
    '                    For Each l_datarow In l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows
    '                        If (l_datarow("PhoneType").ToString() <> "System.DBNull" And l_datarow("PhoneType").ToString() <> "") Then
    '                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "HOME" Then
    '                                If l_datarow("PhoneNumber").ToString.Trim <> TextBoxSecHome.Text.Trim Then
    '                                    l_datarow("PhoneNumber") = TextBoxSecHome.Text.Trim
    '                                    'By Aparna -yren -3026
    '                                    l_datarow("EffDate") = Date.Now()
    '                                End If
    '                                l_Bool_Home = True
    '                            End If
    '                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "OFFICE" Then
    '                                If l_datarow("PhoneNumber").ToString.Trim <> Me.TextBoxSecTelephone.Text.Trim Then
    '                                    l_datarow("PhoneNumber") = Me.TextBoxSecTelephone.Text.Trim
    '                                    'By Aparna -yren -3026
    '                                    l_datarow("EffDate") = Date.Now()
    '                                End If
    '                                l_Bool_Work = True
    '                            End If
    '                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "FAX" Then
    '                                If l_datarow("PhoneNumber").ToString.Trim <> Me.TextBoxSecFax.Text Then
    '                                    l_datarow("PhoneNumber") = Me.TextBoxSecFax.Text
    '                                    'By Aparna -yren -3026
    '                                    l_datarow("EffDate") = Date.Now()
    '                                End If
    '                                l_Bool_Fax = True
    '                            End If
    '                            If l_datarow("PhoneType").ToString.ToUpper.Trim() = "MOBILE" Then
    '                                If l_datarow("PhoneNumber").ToString.Trim <> Me.TextBoxSecMobile.Text Then
    '                                    l_datarow("PhoneNumber") = Me.TextBoxSecMobile.Text
    '                                    'By Aparna -yren -3026
    '                                    l_datarow("EffDate") = Date.Now()
    '                                End If

    '                                l_Bool_Mobile = True
    '                            End If
    '                            If Convert.ToBoolean(l_datarow("bitPrimary")) <> bitPrimary2 Then
    '                                l_datarow("bitPrimary") = bitPrimary2
    '                                'By Aparna -yren -3026
    '                                l_datarow("EffDate") = Date.Now()
    '                            End If
    '                            If Convert.ToBoolean(l_datarow("bitActive")) <> bitActive2 Then
    '                                l_datarow("bitActive") = bitActive2
    '                                'By Aparna -yren -3026
    '                                l_datarow("EffDate") = Date.Now()
    '                            End If
    '                            'Commented by Aparna yren -3026 -Wrong check done
    '                            'If l_datarow("EffDate").tostring.trim <> Me.TextBoxSecEffDate.Text.ToString.Trim() Then
    '                            '    l_datarow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '                            'End If
    '                            'Commented by Aparna yren -3026 -Wrong check done
    '                        End If
    '                    Next
    '                    If l_Bool_Home = False And TextBoxSecHome.Text.Trim.Length > 0 Then
    '                        Dim l_HomeRow As DataRow
    '                        l_HomeRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '                        l_HomeRow("PhoneType") = "HOME"
    '                        l_HomeRow("PhoneNumber") = Me.TextBoxSecHome.Text.Trim
    '                        l_HomeRow("EntityId") = Session("PersId")
    '                        l_HomeRow("bitPrimary") = bitPrimary2
    '                        l_HomeRow("bitActive") = bitActive2
    '                        'By Aparna -Yren- 3026
    '                        l_HomeRow("EffDate") = Date.Now()
    '                        'l_HomeRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '                        l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
    '                    End If

    '                    If l_Bool_Work = False And TextBoxSecTelephone.Text.Trim.Length > 0 Then
    '                        Dim l_WorkRow As DataRow
    '                        l_WorkRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '                        l_WorkRow("PhoneType") = "OFFICE"
    '                        l_WorkRow("PhoneNumber") = Me.TextBoxSecTelephone.Text.Trim
    '                        l_WorkRow("EntityId") = Session("PersId")
    '                        l_WorkRow("bitPrimary") = bitPrimary2
    '                        l_WorkRow("bitActive") = bitActive2
    '                        'By Aparna -YREN -3026
    '                        l_WorkRow("EffDate") = Date.Now()
    '                        'l_WorkRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '                        l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
    '                    End If

    '                    If l_Bool_Fax = False And TextBoxSecFax.Text.Trim.Length > 0 Then
    '                        Dim l_FaxRow As DataRow
    '                        l_FaxRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '                        l_FaxRow("PhoneType") = "FAX"
    '                        l_FaxRow("PhoneNumber") = Me.TextBoxSecFax.Text.Trim
    '                        l_FaxRow("EntityId") = Session("PersId")
    '                        l_FaxRow("bitPrimary") = bitPrimary2
    '                        l_FaxRow("bitActive") = bitActive2
    '                        'By Aparna YREN -3026
    '                        l_FaxRow("EffDate") = Date.Now()
    '                        'l_FaxRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '                        l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
    '                    End If

    '                    If l_Bool_Mobile = False And TextBoxSecMobile.Text.Trim.Length > 0 Then
    '                        Dim l_MobileRow As DataRow
    '                        l_MobileRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '                        l_MobileRow("PhoneType") = "MOBILE"
    '                        l_MobileRow("PhoneNumber") = Me.TextBoxSecMobile.Text.Trim
    '                        l_MobileRow("EntityId") = Session("PersId")
    '                        l_MobileRow("bitPrimary") = bitPrimary2
    '                        l_MobileRow("bitActive") = bitActive2
    '                        'By Aparna -YREN-3026
    '                        l_MobileRow("EffDate") = Date.Now()
    '                        'l_MobileRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '                        l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
    '                    End If

    '                End If
    '            Else
    '                If l_Bool_Home = False And TextBoxSecHome.Text.Trim.Length > 0 Then
    '                    Dim l_HomeRow As DataRow
    '                    l_HomeRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '                    l_HomeRow("PhoneType") = "HOME"
    '                    l_HomeRow("PhoneNumber") = Me.TextBoxSecHome.Text.Trim
    '                    l_HomeRow("EntityId") = Session("PersId")
    '                    l_HomeRow("bitPrimary") = bitPrimary2
    '                    l_HomeRow("bitActive") = bitActive2
    '                    'By Aparna YREN-3026
    '                    l_HomeRow("EffDate") = Date.Now()
    '                    'l_HomeRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '                    l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
    '                End If

    '                If TextBoxSecTelephone.Text.Trim.Length > 0 Then
    '                    Dim l_WorkRow As DataRow
    '                    l_WorkRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '                    l_WorkRow("PhoneType") = "OFFICE"
    '                    l_WorkRow("PhoneNumber") = Me.TextBoxSecTelephone.Text.Trim
    '                    l_WorkRow("EntityId") = Session("PersId")
    '                    l_WorkRow("bitPrimary") = bitPrimary2
    '                    l_WorkRow("bitActive") = bitActive2
    '                    'By Aparna YREN-3026
    '                    l_WorkRow("EffDate") = Date.Now()
    '                    'l_WorkRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '                    l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
    '                End If

    '                If TextBoxSecFax.Text.Trim.Length > 0 Then
    '                    Dim l_FaxRow As DataRow
    '                    l_FaxRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '                    l_FaxRow("PhoneType") = "FAX"
    '                    l_FaxRow("PhoneNumber") = Me.TextBoxSecFax.Text.Trim
    '                    l_FaxRow("EntityId") = Session("PersId")
    '                    l_FaxRow("bitPrimary") = bitPrimary2
    '                    l_FaxRow("bitActive") = bitActive2
    '                    'BY Aparna YREN-3026
    '                    l_FaxRow("EffDate") = Date.Now()
    '                    'l_FaxRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '                    l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
    '                End If

    '                If TextBoxSecMobile.Text.Trim.Length > 0 Then
    '                    Dim l_MobileRow As DataRow
    '                    l_MobileRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '                    l_MobileRow("PhoneType") = "MOBILE"
    '                    l_MobileRow("PhoneNumber") = Me.TextBoxSecMobile.Text.Trim
    '                    l_MobileRow("EntityId") = Session("PersId")
    '                    l_MobileRow("bitPrimary") = bitPrimary2
    '                    l_MobileRow("bitActive") = bitActive2
    '                    'By Aparna YREN-3026
    '                    l_MobileRow("EffDate") = Date.Now()
    '                    'l_MobileRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '                    l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
    '                End If
    '            End If
    '            If Not l_ds_SecondaryAddress.Tables("TelephoneInfo") Is Nothing Then
    '                If l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
    '                    If Not l_ds_SecondaryAddress.Tables("TelephoneInfo").GetChanges Is Nothing Then
    '                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(l_ds_SecondaryAddress)
    '                    End If
    '                End If
    '            End If


    '            ''SecondaryTelephone Section ends


    '        End If 'Session is nothing


    '        '  Else ''''The section below will execute when the adderesses are swapped..

    '        '''Dim l_ds_PrimaryAddress As DataSet
    '        '''Dim l_ds_SecondaryAddress As DataSet

    '        '''If Not Session("Ds_PrimaryAddress") Is Nothing Then
    '        '''    l_ds_PrimaryAddress = CType(Session("Ds_PrimaryAddress"), DataSet)
    '        '''    'Primary Address section starts
    '        '''    If l_ds_PrimaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
    '        '''        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address1").ToString.Trim() <> Me.TextBoxAddress1.Text Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address1") = Me.TextBoxAddress1.Text
    '        '''        End If
    '        '''        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address2").ToString.Trim() <> Me.TextBoxAddress2.Text Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address2") = Me.TextBoxAddress2.Text
    '        '''        End If
    '        '''        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address3").Tostring.trim <> Me.TextBoxAddress3.Text Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Address3") = Me.TextBoxAddress3.Text
    '        '''        End If
    '        '''        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("City").tostring.trim <> Me.TextBoxCity.Text Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("City") = Me.TextBoxCity.Text
    '        '''        End If

    '        '''        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Country").toString.Trim <> Me.DropdownlistCountry.SelectedValue Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Country") = Me.DropdownlistCountry.SelectedValue
    '        '''        End If
    '        '''        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("State").ToString.Trim <> Me.DropdownlistState.SelectedValue Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("State") = Me.DropdownlistState.SelectedValue
    '        '''        End If
    '        '''        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Zip").Tostring.trim <> Me.TextBoxZip.Text Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("Zip") = Me.TextBoxZip.Text
    '        '''        End If
    '        '''        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString.trim <> Me.TextBoxPriEffDate.Text Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate") = Me.TextBoxPriEffDate.Text
    '        '''        End If
    '        '''        If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary")) <> False Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary") = False
    '        '''        End If
    '        '''        If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive")) <> True Then
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive") = True
    '        '''        End If
    '        '''        If Not IsDBNull(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) Then
    '        '''            If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) <> Me.CheckboxIsBadAddress.Checked Then
    '        '''                If Me.CheckboxIsBadAddress.Checked = True Then
    '        '''                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = True
    '        '''                Else
    '        '''                    l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
    '        '''                End If
    '        '''            End If
    '        '''        Else
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
    '        '''        End If


    '        '''    Else
    '        '''        If Me.TextBoxAddress1.Text <> "" Or Me.TextBoxAddress2.Text <> "" Or Me.TextBoxAddress3.Text <> "" Or Me.TextBoxCity.Text <> "" Or Me.TextBoxZip.Text <> "" Or Me.TextBoxPriEffDate.Text <> "" Then
    '        '''            Dim l_PrimaryAddressRow As DataRow
    '        '''            l_PrimaryAddressRow = l_ds_PrimaryAddress.Tables("AddressInfo").NewRow
    '        '''            l_PrimaryAddressRow.Item("EntityId") = Session("PersId")
    '        '''            l_PrimaryAddressRow.Item("Address1") = Me.TextBoxAddress1.Text
    '        '''            l_PrimaryAddressRow.Item("Address2") = Me.TextBoxAddress2.Text
    '        '''            l_PrimaryAddressRow.Item("Address3") = Me.TextBoxAddress3.Text
    '        '''            l_PrimaryAddressRow.Item("City") = Me.TextBoxCity.Text
    '        '''            l_PrimaryAddressRow.Item("Country") = Me.DropdownlistCountry.SelectedValue
    '        '''            l_PrimaryAddressRow.Item("State") = Me.DropdownlistState.SelectedValue
    '        '''            l_PrimaryAddressRow.Item("Zip") = Me.TextBoxZip.Text
    '        '''            l_PrimaryAddressRow.Item("EffDate") = Me.TextBoxPriEffDate.Text
    '        '''            l_PrimaryAddressRow.Item("bitPrimary") = False
    '        '''            l_PrimaryAddressRow.Item("bitActive") = True
    '        '''            If Me.CheckboxIsBadAddress.Checked = True Then
    '        '''                l_PrimaryAddressRow.Item("bitBadAddress") = True
    '        '''            Else
    '        '''                l_PrimaryAddressRow.Item("bitBadAddress") = False
    '        '''            End If
    '        '''            l_ds_PrimaryAddress.Tables("AddressInfo").Rows.Add(l_PrimaryAddressRow)
    '        '''        End If

    '        '''    End If
    '        '''    If Not l_ds_PrimaryAddress.Tables("AddressInfo") Is Nothing Then
    '        '''        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows.Count > 0 Then

    '        '''            If Not l_ds_PrimaryAddress.Tables("AddressInfo").GetChanges Is Nothing Then
    '        '''                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateParticipantAddress(l_ds_PrimaryAddress)
    '        '''            End If
    '        '''        End If

    '        '''    End If

    '        '''    'Primary Address section ends
    '        '''    'Primary Telephone section starts
    '        '''    Dim l_datarow As DataRow
    '        '''    Dim l_Bool_Home As Boolean = False
    '        '''    Dim l_Bool_Work As Boolean = False
    '        '''    Dim l_Bool_Fax As Boolean = False
    '        '''    Dim l_Bool_Mobile As Boolean = False
    '        '''    If l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
    '        '''        If Not l_ds_PrimaryAddress.Tables("TelephoneInfo") Is Nothing Then
    '        '''            ''For Home Phone

    '        '''            For Each l_datarow In l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows
    '        '''                If (l_datarow("PhoneType").ToString() <> "System.DBNull" And l_datarow("PhoneType").ToString() <> "") Then
    '        '''                    If l_datarow("PhoneType").ToString.ToUpper.Trim() = "HOME" Then
    '        '''                        If l_datarow("PhoneNumber").tostring.trim <> TextBoxHome.Text.Trim Then
    '        '''                            l_datarow("PhoneNumber") = TextBoxHome.Text.Trim
    '        '''                        End If
    '        '''                        l_Bool_Home = True
    '        '''                    End If
    '        '''                    If l_datarow("PhoneType").ToString.ToUpper.Trim() = "WORK" Then
    '        '''                        If l_datarow("PhoneNumber").tostring.trim <> Me.TextBoxTelephone.Text.Trim Then
    '        '''                            l_datarow("PhoneNumber") = Me.TextBoxTelephone.Text.Trim
    '        '''                        End If
    '        '''                        l_Bool_Work = True
    '        '''                    End If
    '        '''                    If l_datarow("PhoneType").ToString.ToUpper.Trim() = "FAX" Then
    '        '''                        If l_datarow("PhoneNumber").tostring.trim <> Me.TextBoxFax.Text Then
    '        '''                            l_datarow("PhoneNumber") = Me.TextBoxFax.Text
    '        '''                        End If
    '        '''                        l_Bool_Fax = True
    '        '''                    End If
    '        '''                    If l_datarow("PhoneType").ToString.ToUpper.Trim() = "MOBILE" Then
    '        '''                        If l_datarow("PhoneNumber").tostring.trim <> Me.TextBoxMobile.Text Then
    '        '''                            l_datarow("PhoneNumber") = Me.TextBoxMobile.Text
    '        '''                        End If
    '        '''                        l_Bool_Mobile = True
    '        '''                    End If
    '        '''                    If Convert.ToBoolean(l_datarow("bitPrimary")) <> False Then
    '        '''                        l_datarow("bitPrimary") = False
    '        '''                    End If
    '        '''                    If Convert.ToBoolean(l_datarow("bitActive")) <> True Then
    '        '''                        l_datarow("bitActive") = True
    '        '''                    End If

    '        '''                    If l_datarow("EffDate").tostring.trim <> Me.TextBoxPriEffDate.Text.ToString.Trim() Then
    '        '''                        l_datarow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '        '''                    End If


    '        '''                End If
    '        '''            Next
    '        '''            If l_Bool_Home = False And TextBoxHome.Text.Trim.Length > 0 Then
    '        '''                Dim l_HomeRow As DataRow
    '        '''                l_HomeRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '        '''                l_HomeRow("PhoneType") = "HOME"
    '        '''                l_HomeRow("PhoneNumber") = Me.TextBoxHome.Text.Trim
    '        '''                l_HomeRow("EntityId") = Session("PersId")
    '        '''                l_HomeRow("bitPrimary") = False
    '        '''                l_HomeRow("bitActive") = True
    '        '''                l_HomeRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '        '''                l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
    '        '''            End If

    '        '''            If l_Bool_Work = False And TextBoxTelephone.Text.Trim.Length > 0 Then
    '        '''                Dim l_WorkRow As DataRow
    '        '''                l_WorkRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '        '''                l_WorkRow("PhoneType") = "WORK"
    '        '''                l_WorkRow("PhoneNumber") = Me.TextBoxTelephone.Text.Trim
    '        '''                l_WorkRow("EntityId") = Session("PersId")
    '        '''                l_WorkRow("bitPrimary") = False
    '        '''                l_WorkRow("bitActive") = True
    '        '''                l_WorkRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '        '''                l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
    '        '''            End If

    '        '''            If l_Bool_Fax = False And TextBoxFax.Text.Trim.Length > 0 Then
    '        '''                Dim l_FaxRow As DataRow
    '        '''                l_FaxRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '        '''                l_FaxRow("PhoneType") = "FAX"
    '        '''                l_FaxRow("PhoneNumber") = Me.TextBoxFax.Text.Trim
    '        '''                l_FaxRow("EntityId") = Session("PersId")
    '        '''                l_FaxRow("bitPrimary") = False
    '        '''                l_FaxRow("bitActive") = True
    '        '''                l_FaxRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '        '''                l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
    '        '''            End If

    '        '''            If l_Bool_Mobile = False And TextBoxMobile.Text.Trim.Length > 0 Then
    '        '''                Dim l_MobileRow As DataRow
    '        '''                l_MobileRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '        '''                l_MobileRow("PhoneType") = "MOBILE"
    '        '''                l_MobileRow("PhoneNumber") = Me.TextBoxMobile.Text.Trim
    '        '''                l_MobileRow("EntityId") = Session("PersId")
    '        '''                l_MobileRow("bitPrimary") = False
    '        '''                l_MobileRow("bitActive") = True
    '        '''                l_MobileRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '        '''                l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
    '        '''            End If

    '        '''        End If
    '        '''    Else
    '        '''        If TextBoxHome.Text.Trim.Length > 0 Then
    '        '''            Dim l_HomeRow As DataRow
    '        '''            l_HomeRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '        '''            l_HomeRow("PhoneType") = "HOME"
    '        '''            l_HomeRow("PhoneNumber") = Me.TextBoxHome.Text.Trim
    '        '''            l_HomeRow("EntityId") = Session("PersId")
    '        '''            l_HomeRow("bitPrimary") = False
    '        '''            l_HomeRow("bitActive") = True
    '        '''            l_HomeRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '        '''            l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
    '        '''        End If

    '        '''        If TextBoxTelephone.Text.Trim.Length > 0 Then
    '        '''            Dim l_WorkRow As DataRow
    '        '''            l_WorkRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '        '''            l_WorkRow("PhoneType") = "WORK"
    '        '''            l_WorkRow("PhoneNumber") = Me.TextBoxTelephone.Text.Trim
    '        '''            l_WorkRow("EntityId") = Session("PersId")
    '        '''            l_WorkRow("bitPrimary") = False
    '        '''            l_WorkRow("bitActive") = True
    '        '''            l_WorkRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '        '''            l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
    '        '''        End If

    '        '''        If TextBoxFax.Text.Trim.Length > 0 Then
    '        '''            Dim l_FaxRow As DataRow
    '        '''            l_FaxRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '        '''            l_FaxRow("PhoneType") = "FAX"
    '        '''            l_FaxRow("PhoneNumber") = Me.TextBoxFax.Text.Trim
    '        '''            l_FaxRow("EntityId") = Session("PersId")
    '        '''            l_FaxRow("bitPrimary") = False
    '        '''            l_FaxRow("bitActive") = True
    '        '''            l_FaxRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '        '''            l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
    '        '''        End If

    '        '''        If TextBoxMobile.Text.Trim.Length > 0 Then
    '        '''            Dim l_MobileRow As DataRow
    '        '''            l_MobileRow = l_ds_PrimaryAddress.Tables("TelephoneInfo").NewRow
    '        '''            l_MobileRow("PhoneType") = "MOBILE"
    '        '''            l_MobileRow("PhoneNumber") = Me.TextBoxMobile.Text.Trim
    '        '''            l_MobileRow("EntityId") = Session("PersId")
    '        '''            l_MobileRow("bitPrimary") = False
    '        '''            l_MobileRow("bitActive") = True
    '        '''            l_MobileRow("EffDate") = Me.TextBoxPriEffDate.Text.ToString.Trim()
    '        '''            l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
    '        '''        End If

    '        '''    End If
    '        '''    If Not l_ds_PrimaryAddress.Tables("TelephoneInfo") Is Nothing Then
    '        '''        If l_ds_PrimaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
    '        '''            If Not l_ds_PrimaryAddress.Tables("TelephoneInfo").GetChanges Is Nothing Then
    '        '''                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(l_ds_PrimaryAddress)
    '        '''            End If
    '        '''        End If
    '        '''    End If


    '        '''    ''Primary Telephone Section ends
    '        '''    ''Email Section Starts

    '        '''    If l_ds_PrimaryAddress.Tables("EmailInfo").Rows.Count > 0 Then
    '        '''        If l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("EmailAddress").tostring.Trim <> Me.TextBoxEmailId.Text Then
    '        '''            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("EmailAddress") = Me.TextBoxEmailId.Text
    '        '''        End If
    '        '''        If Not IsDBNull(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress")) Then
    '        '''            If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress")) <> CheckboxBadEmail.Checked Then
    '        '''                If Me.CheckboxBadEmail.Checked = True Then
    '        '''                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress") = True
    '        '''                Else
    '        '''                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress") = False
    '        '''                End If
    '        '''            End If
    '        '''        Else
    '        '''            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress") = False
    '        '''        End If
    '        '''        If Not IsDBNull(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed")) Then
    '        '''            If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed")) <> Me.CheckboxUnsubscribe.Checked Then
    '        '''                If Me.CheckboxUnsubscribe.Checked = True Then
    '        '''                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed") = True
    '        '''                Else
    '        '''                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed") = False
    '        '''                End If
    '        '''            End If
    '        '''        Else
    '        '''            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed") = False
    '        '''        End If

    '        '''        If Not IsDBNull(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly")) Then
    '        '''            If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly")) <> Me.CheckboxTextOnly.Checked Then
    '        '''                If Me.CheckboxTextOnly.Checked = True Then
    '        '''                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly") = True
    '        '''                Else
    '        '''                    l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly") = False
    '        '''                End If
    '        '''            End If
    '        '''        Else
    '        '''            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly") = False
    '        '''        End If


    '        '''        If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("bitActive")) <> True Then
    '        '''            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("bitActive") = True
    '        '''        End If
    '        '''        If Convert.ToBoolean(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("bitPrimary")) <> True Then
    '        '''            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("bitPrimary") = True
    '        '''        End If
    '        '''        If IsDBNull(l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("EffDate")) Then
    '        '''            l_ds_PrimaryAddress.Tables("EmailInfo").Rows(0).Item("EffDate") = DateTime.Now
    '        '''        End If


    '        '''    Else
    '        '''        If Me.TextBoxEmailId.Text.Trim.Length > 0 Then
    '        '''            Dim l_PrimaryEmailRow As DataRow
    '        '''            l_PrimaryEmailRow = l_ds_PrimaryAddress.Tables("EmailInfo").NewRow
    '        '''            l_PrimaryEmailRow.Item("EntityId") = Session("PersId")
    '        '''            l_PrimaryEmailRow.Item("EmailAddress") = Me.TextBoxEmailId.Text
    '        '''            If Me.CheckboxBadEmail.Checked = True Then
    '        '''                l_PrimaryEmailRow.Item("IsBadAddress") = True
    '        '''            Else
    '        '''                l_PrimaryEmailRow.Item("IsBadAddress") = False
    '        '''            End If
    '        '''            If Me.CheckboxUnsubscribe.Checked = True Then
    '        '''                l_PrimaryEmailRow.Item("IsUnsubscribed") = True
    '        '''            Else
    '        '''                l_PrimaryEmailRow.Item("IsUnsubscribed") = False
    '        '''            End If
    '        '''            If Me.CheckboxTextOnly.Checked = True Then
    '        '''                l_PrimaryEmailRow.Item("IsTextOnly") = True
    '        '''            Else
    '        '''                l_PrimaryEmailRow.Item("IsTextOnly") = False
    '        '''            End If

    '        '''            l_PrimaryEmailRow.Item("EffDate") = DateTime.Now()
    '        '''            l_PrimaryEmailRow.Item("bitPrimary") = True
    '        '''            l_PrimaryEmailRow.Item("bitActive") = True

    '        '''            l_ds_PrimaryAddress.Tables("EmailInfo").Rows.Add(l_PrimaryEmailRow)
    '        '''        End If

    '        '''    End If
    '        '''    If Not l_ds_PrimaryAddress.Tables("EmailInfo") Is Nothing Then
    '        '''        If l_ds_PrimaryAddress.Tables("EmailInfo").Rows.Count > 0 Then
    '        '''            If Not l_ds_PrimaryAddress.Tables("EmailInfo").GetChanges Is Nothing Then
    '        '''                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateEmailAddress(l_ds_PrimaryAddress)
    '        '''            End If
    '        '''        End If
    '        '''    End If



    '        '''    ''email section ends

    '        '''End If 'Session is nothing
    '        '''If Not Session("Ds_SecondaryAddress") Is Nothing Then
    '        '''    l_ds_SecondaryAddress = CType(Session("Ds_SecondaryAddress"), DataSet)
    '        '''    'Secondary Address section starts
    '        '''    If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
    '        '''        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address1").tostring.trim <> Me.TextBoxSecAddress1.Text Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address1") = Me.TextBoxSecAddress1.Text
    '        '''        End If
    '        '''        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address2").tostring.trim <> Me.TextBoxSecAddress2.Text Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address2") = Me.TextBoxSecAddress2.Text
    '        '''        End If
    '        '''        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address3").tostring.trim <> Me.TextBoxSecAddress3.Text Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Address3") = Me.TextBoxSecAddress3.Text
    '        '''        End If
    '        '''        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("City").tostring.trim <> Me.TextBoxSecCity.Text Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("City") = Me.TextBoxSecCity.Text
    '        '''        End If

    '        '''        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Country").tostring.Trim <> Me.DropdownlistSecCountry.SelectedValue Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Country") = Me.DropdownlistSecCountry.SelectedValue
    '        '''        End If
    '        '''        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("State").tostring.trim <> Me.DropdownlistSecState.SelectedValue Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("State") = Me.DropdownlistSecState.SelectedValue
    '        '''        End If
    '        '''        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Zip").tostring.trim <> Me.TextBoxSecZip.Text Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("Zip") = Me.TextBoxSecZip.Text
    '        '''        End If
    '        '''        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").tostring.Trim <> Me.TextBoxSecEffDate.Text Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate") = Me.TextBoxSecEffDate.Text
    '        '''        End If
    '        '''        If Convert.ToBoolean(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary")) <> True Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitPrimary") = True
    '        '''        End If
    '        '''        If Convert.ToBoolean(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive")) <> True Then
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitActive") = True
    '        '''        End If
    '        '''        If Not IsDBNull(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) Then
    '        '''            If Convert.ToBoolean(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) <> Me.CheckboxSecIsBadAddress.Checked Then
    '        '''                If Me.CheckboxSecIsBadAddress.Checked = True Then
    '        '''                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = True
    '        '''                Else
    '        '''                    l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
    '        '''                End If
    '        '''            End If
    '        '''        Else
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress") = False
    '        '''        End If


    '        '''    Else
    '        '''        If Me.TextBoxSecAddress1.Text <> "" Or Me.TextBoxSecAddress2.Text <> "" Or Me.TextBoxSecAddress3.Text <> "" Or Me.TextBoxSecCity.Text <> "" Or Me.TextBoxSecZip.Text <> "" Or Me.TextBoxSecEffDate.Text <> "" Then
    '        '''            Dim l_SecondaryAddressRow As DataRow
    '        '''            l_SecondaryAddressRow = l_ds_SecondaryAddress.Tables("AddressInfo").NewRow
    '        '''            l_SecondaryAddressRow.Item("EntityId") = Session("PersId")
    '        '''            l_SecondaryAddressRow.Item("Address1") = Me.TextBoxSecAddress1.Text
    '        '''            l_SecondaryAddressRow.Item("Address2") = Me.TextBoxSecAddress2.Text
    '        '''            l_SecondaryAddressRow.Item("Address3") = Me.TextBoxSecAddress3.Text
    '        '''            l_SecondaryAddressRow.Item("City") = Me.TextBoxSecCity.Text
    '        '''            l_SecondaryAddressRow.Item("Country") = Me.DropdownlistSecCountry.SelectedValue
    '        '''            l_SecondaryAddressRow.Item("State") = Me.DropdownlistSecState.SelectedValue
    '        '''            l_SecondaryAddressRow.Item("Zip") = Me.TextBoxSecZip.Text
    '        '''            l_SecondaryAddressRow.Item("EffDate") = Me.TextBoxSecEffDate.Text
    '        '''            l_SecondaryAddressRow.Item("bitPrimary") = True
    '        '''            l_SecondaryAddressRow.Item("bitActive") = True
    '        '''            If Me.CheckboxSecIsBadAddress.Checked = True Then
    '        '''                l_SecondaryAddressRow.Item("bitBadAddress") = True
    '        '''            Else
    '        '''                l_SecondaryAddressRow.Item("bitBadAddress") = False
    '        '''            End If
    '        '''            l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Add(l_SecondaryAddressRow)
    '        '''        End If

    '        '''    End If
    '        '''    If Not l_ds_SecondaryAddress.Tables("AddressInfo") Is Nothing Then
    '        '''        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
    '        '''            If Not l_ds_SecondaryAddress.Tables("AddressInfo").GetChanges Is Nothing Then
    '        '''                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateParticipantAddress(l_ds_SecondaryAddress)
    '        '''            End If
    '        '''        End If
    '        '''    End If


    '        '''    'SecondaryAddress section ends
    '        '''    'Primary Telephone section starts
    '        '''    Dim l_datarow As DataRow
    '        '''    Dim l_Bool_Home As Boolean = False
    '        '''    Dim l_Bool_Work As Boolean = False
    '        '''    Dim l_Bool_Fax As Boolean = False
    '        '''    Dim l_Bool_Mobile As Boolean = False
    '        '''    If l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
    '        '''        If Not l_ds_SecondaryAddress.Tables("TelephoneInfo") Is Nothing Then

    '        '''            ''For Home Phone
    '        '''            For Each l_datarow In l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows
    '        '''                If (l_datarow("PhoneType").ToString() <> "System.DBNull" And l_datarow("PhoneType").ToString() <> "") Then
    '        '''                    If l_datarow("PhoneType").ToString.ToUpper.Trim() = "HOME" Then
    '        '''                        If l_datarow("PhoneNumber").tostring.trim <> TextBoxSecHome.Text.Trim Then
    '        '''                            l_datarow("PhoneNumber") = TextBoxSecHome.Text.Trim
    '        '''                        End If
    '        '''                        l_Bool_Home = True
    '        '''                    End If
    '        '''                    If l_datarow("PhoneType").ToString.ToUpper.Trim() = "WORK" Then
    '        '''                        If l_datarow("PhoneNumber").tostring.Trim <> Me.TextBoxSecTelephone.Text.Trim Then
    '        '''                            l_datarow("PhoneNumber") = Me.TextBoxSecTelephone.Text.Trim
    '        '''                        End If
    '        '''                        l_Bool_Work = True
    '        '''                    End If
    '        '''                    If l_datarow("PhoneType").ToString.ToUpper.Trim() = "FAX" Then
    '        '''                        If l_datarow("PhoneNumber").tostring.Trim <> Me.TextBoxSecFax.Text Then
    '        '''                            l_datarow("PhoneNumber") = Me.TextBoxSecFax.Text
    '        '''                        End If
    '        '''                        l_Bool_Fax = True
    '        '''                    End If
    '        '''                    If l_datarow("PhoneType").ToString.ToUpper.Trim() = "MOBILE" Then
    '        '''                        If l_datarow("PhoneNumber").tostring.trim <> Me.TextBoxSecMobile.Text Then
    '        '''                            l_datarow("PhoneNumber") = Me.TextBoxSecMobile.Text
    '        '''                        End If

    '        '''                        l_Bool_Mobile = True
    '        '''                    End If
    '        '''                    If Convert.ToBoolean(l_datarow("bitPrimary")) <> True Then
    '        '''                        l_datarow("bitPrimary") = True
    '        '''                    End If
    '        '''                    If Convert.ToBoolean(l_datarow("bitActive")) <> True Then
    '        '''                        l_datarow("bitActive") = True
    '        '''                    End If

    '        '''                    If l_datarow("EffDate").tostring.trim <> Me.TextBoxSecEffDate.Text.ToString.Trim() Then
    '        '''                        l_datarow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '        '''                    End If

    '        '''                End If
    '        '''            Next
    '        '''            If l_Bool_Home = False And TextBoxSecHome.Text.Trim.Length > 0 Then
    '        '''                Dim l_HomeRow As DataRow
    '        '''                l_HomeRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '        '''                l_HomeRow("PhoneType") = "HOME"
    '        '''                l_HomeRow("PhoneNumber") = Me.TextBoxSecHome.Text.Trim
    '        '''                l_HomeRow("EntityId") = Session("PersId")
    '        '''                l_HomeRow("bitPrimary") = True
    '        '''                l_HomeRow("bitActive") = True
    '        '''                l_HomeRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '        '''                l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
    '        '''            End If

    '        '''            If l_Bool_Work = False And TextBoxSecTelephone.Text.Trim.Length > 0 Then
    '        '''                Dim l_WorkRow As DataRow
    '        '''                l_WorkRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '        '''                l_WorkRow("PhoneType") = "WORK"
    '        '''                l_WorkRow("PhoneNumber") = Me.TextBoxSecTelephone.Text.Trim
    '        '''                l_WorkRow("EntityId") = Session("PersId")
    '        '''                l_WorkRow("bitPrimary") = True
    '        '''                l_WorkRow("bitActive") = True
    '        '''                l_WorkRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '        '''                l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
    '        '''            End If

    '        '''            If l_Bool_Fax = False And TextBoxSecFax.Text.Trim.Length > 0 Then
    '        '''                Dim l_FaxRow As DataRow
    '        '''                l_FaxRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '        '''                l_FaxRow("PhoneType") = "FAX"
    '        '''                l_FaxRow("PhoneNumber") = Me.TextBoxSecFax.Text.Trim
    '        '''                l_FaxRow("EntityId") = Session("PersId")
    '        '''                l_FaxRow("bitPrimary") = True
    '        '''                l_FaxRow("bitActive") = True
    '        '''                l_FaxRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '        '''                l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
    '        '''            End If

    '        '''            If l_Bool_Mobile = False And TextBoxSecMobile.Text.Trim.Length > 0 Then
    '        '''                Dim l_MobileRow As DataRow
    '        '''                l_MobileRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '        '''                l_MobileRow("PhoneType") = "MOBILE"
    '        '''                l_MobileRow("PhoneNumber") = Me.TextBoxSecMobile.Text.Trim
    '        '''                l_MobileRow("EntityId") = Session("PersId")
    '        '''                l_MobileRow("bitPrimary") = True
    '        '''                l_MobileRow("bitActive") = True
    '        '''                l_MobileRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '        '''                l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
    '        '''            End If

    '        '''        End If
    '        '''    Else
    '        '''        If l_Bool_Home = False And TextBoxSecHome.Text.Trim.Length > 0 Then
    '        '''            Dim l_HomeRow As DataRow
    '        '''            l_HomeRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '        '''            l_HomeRow("PhoneType") = "HOME"
    '        '''            l_HomeRow("PhoneNumber") = Me.TextBoxSecHome.Text.Trim
    '        '''            l_HomeRow("EntityId") = Session("PersId")
    '        '''            l_HomeRow("bitPrimary") = True
    '        '''            l_HomeRow("bitActive") = True
    '        '''            l_HomeRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '        '''            l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_HomeRow)
    '        '''        End If

    '        '''        If TextBoxSecTelephone.Text.Trim.Length > 0 Then
    '        '''            Dim l_WorkRow As DataRow
    '        '''            l_WorkRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '        '''            l_WorkRow("PhoneType") = "WORK"
    '        '''            l_WorkRow("PhoneNumber") = Me.TextBoxSecTelephone.Text.Trim
    '        '''            l_WorkRow("EntityId") = Session("PersId")
    '        '''            l_WorkRow("bitPrimary") = True
    '        '''            l_WorkRow("bitActive") = True
    '        '''            l_WorkRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '        '''            l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_WorkRow)
    '        '''        End If

    '        '''        If TextBoxFax.Text.Trim.Length > 0 Then
    '        '''            Dim l_FaxRow As DataRow
    '        '''            l_FaxRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '        '''            l_FaxRow("PhoneType") = "FAX"
    '        '''            l_FaxRow("PhoneNumber") = Me.TextBoxSecFax.Text.Trim
    '        '''            l_FaxRow("EntityId") = Session("PersId")
    '        '''            l_FaxRow("bitPrimary") = True
    '        '''            l_FaxRow("bitActive") = True
    '        '''            l_FaxRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '        '''            l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_FaxRow)
    '        '''        End If

    '        '''        If TextBoxSecMobile.Text.Trim.Length > 0 Then
    '        '''            Dim l_MobileRow As DataRow
    '        '''            l_MobileRow = l_ds_SecondaryAddress.Tables("TelephoneInfo").NewRow
    '        '''            l_MobileRow("PhoneType") = "MOBILE"
    '        '''            l_MobileRow("PhoneNumber") = Me.TextBoxSecMobile.Text.Trim
    '        '''            l_MobileRow("EntityId") = Session("PersId")
    '        '''            l_MobileRow("bitPrimary") = True
    '        '''            l_MobileRow("bitActive") = True
    '        '''            l_MobileRow("EffDate") = Me.TextBoxSecEffDate.Text.ToString.Trim()
    '        '''            l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Add(l_MobileRow)
    '        '''        End If
    '        '''    End If
    '        '''    If Not l_ds_SecondaryAddress.Tables("TelephoneInfo") Is Nothing Then
    '        '''        If l_ds_SecondaryAddress.Tables("TelephoneInfo").Rows.Count > 0 Then
    '        '''            If Not l_ds_SecondaryAddress.Tables("TelephoneInfo").GetChanges Is Nothing Then
    '        '''                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(l_ds_SecondaryAddress)
    '        '''            End If
    '        '''        End If
    '        '''    End If


    '        '''    ''SecondaryTelephone Section ends


    '        '''End If 'Session is nothing




    '        '''End If
    '        'Me.ButtonActivateAsPrimary.Enabled = False
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'BS:2012:03:26:-restructure address code
    'Start BS:2012:04:19:-restructure address code
    Public Function AddNewRowTelephoneInfo(ByVal ContactNo As String, ByVal ContactType As String,
    ByVal bitPrimary As Boolean, ByVal bitActive As Boolean,
    ByRef dtContacts As DataTable)
        Dim l_Row As DataRow
        l_Row = dtContacts.NewRow
        l_Row("PhoneType") = ContactType '"HOME"
        l_Row("PhoneNumber") = ContactNo 'Me.TextBoxHome.Text.Trim
        l_Row("EntityId") = Session("PersId")
        l_Row("bitPrimary") = bitPrimary
        l_Row("bitActive") = bitActive
        l_Row("EffDate") = Date.Now()
        dtContacts.Rows.Add(l_Row)
    End Function
    Public Function UpdateRowTelephoneInfo(ByVal ContactNo As String, ByVal ContactType As String,
    ByVal bitPrimary As Boolean, ByVal bitActive As Boolean,
    ByRef l_Row As DataRow)
        l_Row("PhoneType") = ContactType    '"HOME"
        l_Row("PhoneNumber") = ContactNo    'Me.TextBoxHome.Text.Trim
        l_Row("EntityId") = Session("PersId")
        l_Row("bitPrimary") = bitPrimary
        l_Row("bitActive") = bitActive
        l_Row("EffDate") = Date.Now()
    End Function
    Public Function UpdateTelephoneDetail(ByVal ContactType As String,
      ByVal ContactNo As String, ByVal bitPrimary As String,
      ByVal bitActive As String, ByRef dtContacts As DataTable)
        Dim l_TelephonRow As DataRow()
        Dim l_string_No As String = ContactNo

        l_TelephonRow = dtContacts.Select("PhoneType='" + ContactType + "'")


        'DK:20012:12:05: - BT ID : 1454 : - Desc : YRS 5.0-1731:It is creating blank records in atstelephones table. 

        If l_TelephonRow.Length > 0 Then
            If Not (l_TelephonRow(0)("PhoneNumber").ToString.Trim = Trim(ContactNo) And l_TelephonRow(0)("PhoneType").ToString.Trim = Trim(ContactType)) Then
                UpdateRowTelephoneInfo(l_string_No, ContactType, bitPrimary, bitActive, l_TelephonRow(0))
            End If
        Else
            If Not String.IsNullOrEmpty(l_string_No) Then
                AddNewRowTelephoneInfo(l_string_No, ContactType, bitPrimary, bitActive, dtContacts)
            End If
        End If


        'Code Commneted by Dinesh Kanojia for bug 1454.
        'If l_TelephonRow.Length > 0 Then
        '	'If l_TelephonRow(0)("PhoneNumber").ToString.Trim <> ContactNo Then
        '	UpdateRowTelephoneInfo(l_string_No, ContactType, bitPrimary, bitActive, l_TelephonRow(0))
        'Else
        '	AddNewRowTelephoneInfo(l_string_No, ContactType, bitPrimary, bitActive, l_ds_Address)
        'End If

    End Function
    'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
    'Private Sub UpdateAddress(ByVal bitPrimary As Boolean, ByVal bitActive As Boolean,
    'ByVal l_ds_Address() As DataRow, ByRef AddressWebUserCtrl As AddressUserControlNew)

    '    AddressWebUserCtrl.UpdateAddressDetail

    'End Sub
    Private Function GetAddressTable(ByVal bitPrimary As Boolean, ByRef AddressWebUserCtrl As AddressUserControlNew) As DataTable
        'Start:Anudeep01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        Return AddressWebUserCtrl.GetAddressTable()
    End Function
    Private Sub UpdateTelephone(ByVal bitPrimary As Boolean, ByVal bitActive As Boolean, ByVal Home As String, ByVal Telephone As String, ByVal Fax As String, ByVal Mobile As String, ByVal dtContacts As DataTable)
        Try
            'Commented by Anudeep:14.03.2013 for telephone 
            If Not dtContacts Is Nothing Then
                'Telephone
                UpdateTelephoneDetail("HOME", Home, bitPrimary, bitActive, dtContacts)
                UpdateTelephoneDetail("OFFICE", Telephone, bitPrimary, bitActive, dtContacts)
                UpdateTelephoneDetail("MOBILE", Mobile, bitPrimary, bitActive, dtContacts)
                UpdateTelephoneDetail("FAX", Fax, bitPrimary, bitActive, dtContacts)
                'Add TelephoneInfo in DB
                If HelperFunctions.isNonEmpty(dtContacts) Then
                    If Not dtContacts Is Nothing Then
                        If dtContacts.Rows.Count > 0 Then
                            If Not dtContacts.GetChanges Is Nothing Then
                                Dim l_ds_Contacts As New DataSet()
                                l_ds_Contacts.Tables.Add(dtContacts.Copy())
                                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(l_ds_Contacts)
                                'YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(l_ds_Address)
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
    Private Sub UpdateEmailInfo()

        'Email Infomation 
        Dim l_dt_Email As DataTable

        If Not Session("Dt_EmailAddress") Is Nothing Then
            l_dt_Email = DirectCast(Session("Dt_EmailAddress"), DataTable)
            If l_dt_Email.Rows.Count > 0 Then
                If l_dt_Email.Rows(0).Item("EmailAddress").ToString.Trim <> Me.TextBoxEmailId.Text Then
                    l_dt_Email.Rows(0).Item("EmailAddress") = Me.TextBoxEmailId.Text
                End If
                If Not IsDBNull(l_dt_Email.Rows(0).Item("IsBadAddress")) Then
                    If Convert.ToBoolean(l_dt_Email.Rows(0).Item("IsBadAddress")) <> CheckboxBadEmail.Checked Then
                        If Me.CheckboxBadEmail.Checked = True Then
                            l_dt_Email.Rows(0).Item("IsBadAddress") = True
                        Else
                            l_dt_Email.Rows(0).Item("IsBadAddress") = False
                        End If
                    End If
                Else
                    l_dt_Email.Rows(0).Item("IsBadAddress") = False
                End If
                If Not IsDBNull(l_dt_Email.Rows(0).Item("IsUnsubscribed")) Then
                    If Convert.ToBoolean(l_dt_Email.Rows(0).Item("IsUnsubscribed")) <> Me.CheckboxUnsubscribe.Checked Then
                        If Me.CheckboxUnsubscribe.Checked = True Then
                            l_dt_Email.Rows(0).Item("IsUnsubscribed") = True
                        Else
                            l_dt_Email.Rows(0).Item("IsUnsubscribed") = False
                        End If
                    End If
                Else
                    l_dt_Email.Rows(0).Item("IsUnsubscribed") = False
                End If

                If Not IsDBNull(l_dt_Email.Rows(0).Item("IsTextOnly")) Then
                    If Convert.ToBoolean(l_dt_Email.Rows(0).Item("IsTextOnly")) <> Me.CheckboxTextOnly.Checked Then
                        If Me.CheckboxTextOnly.Checked = True Then
                            l_dt_Email.Rows(0).Item("IsTextOnly") = True
                        Else
                            l_dt_Email.Rows(0).Item("IsTextOnly") = False
                        End If
                    End If
                Else
                    l_dt_Email.Rows(0).Item("IsTextOnly") = False
                End If


                If Convert.ToBoolean(l_dt_Email.Rows(0).Item("bitActive")) <> True Then
                    l_dt_Email.Rows(0).Item("bitActive") = True
                End If
                If Convert.ToBoolean(l_dt_Email.Rows(0).Item("bitPrimary")) <> True Then
                    l_dt_Email.Rows(0).Item("bitPrimary") = True
                End If
                If IsDBNull(l_dt_Email.Rows(0).Item("EffDate")) Then
                    l_dt_Email.Rows(0).Item("EffDate") = DateTime.Now
                End If


            Else
                If Me.TextBoxEmailId.Text.Trim.Length > 0 Then
                    Dim l_EmailRow As DataRow
                    l_EmailRow = l_dt_Email.NewRow
                    l_EmailRow.Item("EntityId") = Session("PersId")
                    l_EmailRow.Item("EmailAddress") = Me.TextBoxEmailId.Text
                    If Me.CheckboxBadEmail.Checked = True Then
                        l_EmailRow.Item("IsBadAddress") = True
                    Else
                        l_EmailRow.Item("IsBadAddress") = False
                    End If
                    If Me.CheckboxUnsubscribe.Checked = True Then
                        l_EmailRow.Item("IsUnsubscribed") = True
                    Else
                        l_EmailRow.Item("IsUnsubscribed") = False
                    End If
                    If Me.CheckboxTextOnly.Checked = True Then
                        l_EmailRow.Item("IsTextOnly") = True
                    Else
                        l_EmailRow.Item("IsTextOnly") = False
                    End If

                    l_EmailRow.Item("EffDate") = DateTime.Now()
                    l_EmailRow.Item("bitPrimary") = True
                    l_EmailRow.Item("bitActive") = True

                    l_dt_Email.Rows.Add(l_EmailRow)
                End If

            End If
            If HelperFunctions.isNonEmpty(l_dt_Email) Then
                'If Not l_ds_Email Is Nothing Then
                '	If l_ds_Email.Rows.Count > 0 Then
                If Not l_dt_Email.GetChanges Is Nothing Then
                    Dim ds As New DataSet
                    ds.Tables.Add(l_dt_Email.Copy())
                    ds.Tables(0).TableName = "EmailInfo"
                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateEmailAddress(ds)
                End If
                '	End If
            End If
        End If

    End Sub
    'End BS:2012:04:19:-restructure address code

    Public Sub SaveInfo()

        Dim dsBanking As New DataSet
        Dim dsFedWithDrawals As New DataSet
        Dim dsGenWithDrawals As New DataSet
        Dim dsBeneficiaries As New DataSet
        Dim blnChanged As Boolean
        '---------------------------------------------------
        Dim l_dataset_Active As DataSet
        Dim l_dataset_Retire As DataSet
        Dim l_dataset_Retirees As DataSet
        Dim l_string_PersId As String
        Dim l_string_PhonySSNo As String = ""
        Dim l_string_SSNOExist As String = String.Empty
        Dim dtAddress As New DataTable
        Dim drPrimaryAddressTable As DataTable
        Dim drSecondaryAddressTable As DataTable
        Dim dtAnnuityBeneficariesAddress As DataTable
        Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
        Dim isStateWithholdingDataSave As Boolean 
        Try


        l_string_PersId = Session("PersId")

        'Vipul 01Feb06 Cache-Session
        'Dim Cache As CacheManager
        'Cache = CacheFactory.GetCacheManager()
        'Vipul 01Feb06 Cache-Session

        'Vipul 01Feb06 Cache-Session
        'If Not Cache("BeneficiariesActive") Is Nothing Then
        '    l_dataset_Active = Cache("BeneficiariesActive")

        'Added By Ashutosh Patil as on 06-Jun-2007
        'Validations for Address if Phony SSNo is validated properly but and secondary Address is not entered properly
        'YREN-3490  
        'Start Ashutosh Patil on 06-Jun-2007
        If Session("PhonySSNo") = "Phony_SSNo" Then
            Session("PhonySSNo") = Nothing
            If Me.ButtonEditAddress.Enabled = False Then
                l_string_PhonySSNo = ProcessPhonySSNo()
                If l_string_PhonySSNo.ToString = "NotValidated" Then
                    Exit Sub
                End If
            End If
        End If
        'End Ashutosh Patil on 06-Jun-2007

        If Not Session("BeneficiariesActive") Is Nothing Then
            l_dataset_Active = Session("BeneficiariesActive")
            If l_dataset_Active.Tables(0).Rows.Count > 0 Then
                CalculateValues(l_dataset_Active.Tables(0), "A")
            End If

        End If
        If Not Session("BeneficiariesRetired") Is Nothing Then
            l_dataset_Retire = Session("BeneficiariesRetired")
            If l_dataset_Retire.Tables(0).Rows.Count > 0 Then
                CalculateValues(l_dataset_Retire.Tables(0), "R")
            End If
        End If

        '---------------------------------------
        'Commented and Shifted By Ashutosh Patil as on 21-Jun-2007
        'Insertion or Deletion only in Session("ValidationMessage")    
        'If Me.chkOldGuardNews.Checked Then
        '    YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertOldGuardNews(Session("PersId"), Session("LoginId"))
        'Else
        '    YMCARET.YmcaBusinessObject.RetireesInformationBOClass.DeleteOldGuardNews(Session("PersId"))
        'End If

        'START: PPP | 2015.10.07 | YRS-AT-2361 | Checking here for duplicate bank records, if exists then aborting the save action
        If (YMCARET.YmcaBusinessObject.RetireesInformationBOClass.IsDuplicateBankRecordExists(TryCast(Session("BankingDtls"), DataSet), Session("PersId"))) Then
            MessageBox.Show(PlaceHolder1, "YMCA", "Banking record already exists with this same effective date. Please reload this screen to view latest banking details.", MessageBoxButtons.Stop)
            Exit Sub
        End If
        'END: PPP | 2015.10.07 | YRS-AT-2361 | Checking here for duplicate bank records, if exists then aborting the save action

        If Session("ValidationMessage") Is Nothing Then
            'Vipul 01Feb06 Cache-Session
            'dsBanking = Cache("BankingDtls")
            'By Ashutosh Patil as on 21-Jun-2007
            'Check for Duplication of SSNo
            'UpadateGeneralInfo()

            'YRPS-4704 - Start
            Dim dsRetiredBeneficiaries As New DataSet
            Dim dtRetiredBeneficariesAddress As DataTable
            'Commented by Swopna 21May08,YRPS-4704------start
            'If cboGeneralMaritalStatus.SelectedValue <> "M" Then
            '    Dim dRows As DataRow()

            '    dsRetiredBeneficiaries = Session("BeneficiariesRetired")
            '    If Not dsRetiredBeneficiaries Is Nothing Then
            '        If dsRetiredBeneficiaries.Tables.Count > 0 Then
            '            If dsRetiredBeneficiaries.Tables(0).Rows.Count > 0 Then
            '                dRows = dsRetiredBeneficiaries.Tables(0).Select("Rel='SP'")
            '                If dRows.Length > 0 Then
            '                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Cannot select Marital Status other than Married as one of the beneficiary is defined as Spouse. ", MessageBoxButtons.Stop)
            '                    Exit Sub
            '                End If
            '            End If
            '        End If
            '    End If
            'End If
            'Commented by Swopna 21May08,YRPS-4704------end
            'YRPS-4704 - End

            l_string_SSNOExist = UpadateGeneralInfo()
            If l_string_SSNOExist <> "" Then
                'MessageBox.Show("YMCA-YRS", "SSNo. is Already in use for Participant -" + l_string_SSNOExist, MessageBoxButtons.Stop)
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "SSNo. is Already in use for Participant -" + l_string_SSNOExist, MessageBoxButtons.Stop)
                Exit Sub
            End If

            'Priya 05-April-2010 : YRS 5.0-1042:New "flag" value in Person/Retiree maintenance screen
            'Commented OldGuardNews check box code to remove it from screen
            'If Me.chkOldGuardNews.Checked Then
            '    YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertOldGuardNews(Session("PersId"), Session("LoginId"))
            'Else
            '    YMCARET.YmcaBusinessObject.RetireesInformationBOClass.DeleteOldGuardNews(Session("PersId"))
            'End If
            'End YRS 5.0-1042

            dsBanking = Session("BankingDtls")

            dsFedWithDrawals = Session("FedWithDrawals")
            If Not dsFedWithDrawals Is Nothing Then
                If Not dsFedWithDrawals.Tables(0).GetChanges Is Nothing Then
                    If dsFedWithDrawals.Tables(0).GetChanges.Rows.Count > 0 Then
                        blnChanged = True
                    End If
                End If

                YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertRetireesFedWithdrawals(dsFedWithDrawals)

                dsFedWithDrawals.AcceptChanges()
                If blnChanged = True Then
                    dsFedWithDrawals = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpFedWithDrawals(Session("PersId"))
                    blnChanged = False
                End If
                'Vipul 01Feb06 Cache-Session
                'Cache.Add("FedWithDrawals", dsFedWithDrawals)
                Session("FedWithDrawals") = dsFedWithDrawals
                'Vipul 01Feb06 Cache-Session
                Me.DataGridFederalWithholding.DataSource = dsFedWithDrawals
                Me.DataGridFederalWithholding.DataBind()
            End If

                'START : ML | 2019.11.25 | YRS-AT-4598 | Save State Withholding Data
                'If Statewithholding data save from main page flag is True, Statewithholding session is non empty and  bitactive is false (Means data get changed and not saved in DB)
                isStateWithholdingDataSave = False
                If (stwListUserControl.STWDataSaveAtMainPage) And (HelperFunctions.isNonEmpty(SessionManager.SessionStateWithholding.LstSWHPerssDetail.FirstOrDefault)) Then
                    LstSWHPerssDetail = SessionManager.SessionStateWithholding.LstSWHPerssDetail
                    If (LstSWHPerssDetail.FirstOrDefault.bitActive = False) Then
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("RetireeInformationWebForm", "Save State Withholding START")
                        isStateWithholdingDataSave = YMCARET.YmcaBusinessObject.StateWithholdingBO.SavePersStateTaxdetails(LstSWHPerssDetail.FirstOrDefault)
                        SessionManager.SessionStateWithholding.LstSWHPerssDetail = Nothing
                    End If
                End If
                'END : ML | 2019.11.25 | YRS-AT-4598 | Save State Withholding Data

            'Vipul 01Feb06 Cache-Session
            'dsGenWithDrawals = Cache("GenWithDrawals")
            dsGenWithDrawals = Session("GenWithDrawals")
            'Vipul 01Feb06 Cache-Session

            If Not dsGenWithDrawals Is Nothing Then
                If Not dsGenWithDrawals.Tables(0).GetChanges Is Nothing Then
                    If dsGenWithDrawals.Tables(0).GetChanges.Rows.Count > 0 Then
                        blnChanged = True
                    End If
                End If

                YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertRetireesGenWithdrawals(dsGenWithDrawals)
                dsGenWithDrawals.AcceptChanges()
                If blnChanged = True Then
                    dsGenWithDrawals = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpGenWithDrawals(Session("PersId"))
                    blnChanged = False
                End If
                'Vipul 01Feb06 Cache-Session
                'Cache.Add("GenWithDrawals", dsGenWithDrawals)
                Session("GenWithDrawals") = dsGenWithDrawals

                'Vipul 01Feb06 Cache-Session
                Me.DataGridGeneralWithhold.DataSource = dsGenWithDrawals
                Me.DataGridGeneralWithhold.DataBind()

            End If

            dsRetiredBeneficiaries = Session("BeneficiariesRetired")

            If Not dsRetiredBeneficiaries Is Nothing Then
                'Name:Preeti Date:20thFeb06 IssueId:YRST-2072 Reason:Unable To Add Beneficiary Start
                'Added If condition
                If dsRetiredBeneficiaries.Tables(0).Rows.Count <> 0 Then

                    'SP 2014.11.26 BT-2310\YRS 5.0-2255: - Start
                    ''This code has moved here because we need to update the deletion reason of beneficiary in atsbeneficiaries table before deletion of beneficiary.
                    'Start:Anudeep:02.04.2013:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion  
                    If Not Session("dtReason") Is Nothing Then
                        Dim dtReason As DataTable
                        dtReason = Session("dtReason")
                        For Each drReason As DataRow In dtReason.Rows
							' SB | 2016.09.21 | YRS-AT-3028 -  Add new parameter to capture deceased beneficiary SSN
                            YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertBeneficiaryNotes(drReason("strReason").ToString(), drReason("strDOD"), drReason("strBeneficiaryName").ToString(), drReason("strComments").ToString(), drReason("bImp").ToString(), Session("PersId").ToString(), 1, drReason("strBeneUniqueID").ToString(), drReason("strBeneficiarySSNo").ToString())
                        Next
                        Session("dtReason") = Nothing
                        'Start:AA:2013.10.23 - Bt-2264: commented below code for not getting the notes until complete save is done
                        'Session("dtNotes") = Nothing
                        LoadNotesTab()
                    End If
                    'End:Anudeep:02.04.2013:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion  
                    'SP 2014.11.26 BT-2310\YRS 5.0-2255: - End

                    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Getting only Updated SSN values for inserting in Audit DataTable 
                    If Not Session("AuditBeneficiariesTable") Is Nothing Then
                        Dim dtBeneficiariesSSNchanges As DataTable = DirectCast(Session("AuditBeneficiariesTable"), DataTable)
                            Dim bIsRowDeleted As Boolean = False
                        If HelperFunctions.isNonEmpty(dtBeneficiariesSSNchanges) Then
                            Do
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

                        Dim dsBeneficiarySSN As DataSet = New DataSet("BeneficiarySSNChanged")

                        ' Creating Audit Log entries for edited SSN
                        If dtBeneficiariesSSNchanges.Rows.Count > 0 Then
                            dsBeneficiarySSN.Tables.Add(dtBeneficiariesSSNchanges)
                            dsBeneficiarySSN.Tables(0).TableName = "Audit"
                            YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertBeneficiariesSSNChangeAuditRecord(dsBeneficiarySSN)
                            Session("AuditBeneficiariesTable") = Nothing
                            dsBeneficiarySSN.Clear()
                        End If
                    End If
                    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Getting only Updated SSN values for inserting in Audit DataTable 
                    ' START | Manthan Rajguru | 2016.07.26 | YRS-AT-2560 | Getting System Generated Phony SSN and assigning it to dataset
                    If dsRetiredBeneficiaries.HasChanges Then
                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GeneratePhonySSN(dsRetiredBeneficiaries)
                    End If
                    ' END | Manthan Rajguru | 2016.07.26 | YRS-AT-2560 | Getting System Generated Phony SSN and assigning it to dataset

                    If dsRetiredBeneficiaries.HasChanges Then
                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertRetiredBeneficiaries(dsRetiredBeneficiaries)
                        dsRetiredBeneficiaries.AcceptChanges()
                    End If

                    'SP 2014.11.26 BT-2310\YRS 5.0-2255: - Start
                    ''This code has moved up before beneficiary upation/deletion/addtion

                    ''Start:Anudeep:02.04.2013:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion  
                    ''If Not Session("strReason") Is Nothing Then
                    ''    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertBeneficiaryNotes(Session("strReason").ToString(), Session("dtmDOD"), Session("strBeneficiaryName").ToString(), Session("strComments").ToString(), Session("bImp").ToString(), Session("PersId").ToString(), 1)
                    ''End If

                    ''End:Anudeep:02.04.2013:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion  
                    'SP 2014.11.26 BT-2310\YRS 5.0-2255: - End

                    'Vipul 01Feb06 Cache-Session
                    'Cache.Add("BeneficiariesRetired", dsRetiredBeneficiaries)
                    Session("BeneficiariesRetired") = dsRetiredBeneficiaries
                    'Vipul 01Feb06 Cache-Session

                    Session("Flag") = "Retire"
                    LoadBeneficiariesTab()
                    Session("Flag") = ""
                End If
                'Name:Preeti Date:20thFeb06 IssueId:YRST-2072 Reason:Unable To Add Beneficiary End
            End If
            'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 

            dtRetiredBeneficariesAddress = Session("BeneficiaryAddress")

            If Not dtRetiredBeneficariesAddress Is Nothing Then
                For Each drAddress As DataRow In dtRetiredBeneficariesAddress.Rows
                    If Not String.IsNullOrEmpty(drAddress("NewId").ToString()) Then
                        Dim drBeneRow As DataRow()
                        drBeneRow = dsRetiredBeneficiaries.Tables(0).Select("NewId = '" + drAddress("NewId").ToString() + "'")
                        If drBeneRow.Length > 0 Then
                            drAddress("BeneID") = drBeneRow(0)("UniqueId")
                        End If
                    End If
                Next
                ' START | Manthan Rajguru | 2016.07.26 | YRS-AT-2560 | Assigning system generated phony SSN for new beneficiary added
                For Each drAddress As DataRow In dtRetiredBeneficariesAddress.Rows
                    If Not String.IsNullOrEmpty(drAddress("BeneID").ToString()) Then 'BenSSNo
                        Dim drBeneRow As DataRow()
                        drBeneRow = dsRetiredBeneficiaries.Tables(0).Select("UniqueID = '" + drAddress("BeneID").ToString() + "'")
                        'START: MMR | 2018.05.22 | YRS-AT-3716 | Commented existing code and added condition to check for NULL value
                        'If drBeneRow.Length > 0 AndAlso (drBeneRow(0)("TaxID") <> drAddress("BenSSNo")) Then
                        '    drAddress("BenSSNo") = drBeneRow(0)("TaxID")
                        'End If
                        If drBeneRow.Length > 0 Then
                            Dim taxId As String = String.Empty
                            Dim benSSNo As String = String.Empty

                            If Not IsDBNull(drBeneRow(0)("TaxID")) Then
                                taxId = drBeneRow(0)("TaxID").ToString()
                            End If
                            If Not IsDBNull(drAddress("BenSSNo")) Then
                                benSSNo = drAddress("BenSSNo").ToString()
                            End If

                            If taxId <> benSSNo Then
                                drAddress("BenSSNo") = taxId
                            End If
                        End If
                        'END: MMR | 2018.05.22 | YRS-AT-3716 | Commented existing code and added condition to check for NULL value
                    End If
                Next
                ' End | Manthan Rajguru | 2016.07.26 | YRS-AT-2560 | Assigning system generated phony SSN for new beneficiary added
                'Address.SaveAddressOfBeneficiariesByPerson(dtRetiredBeneficariesAddress)
            End If
            'If Not Session("BeneficiaryAddress") Is Nothing Then
            '    dtRetiredBeneficariesAddress = Session("BeneficiaryAddress")
            '    Address.SaveAddressOfBeneficiariesByPerson(dtRetiredBeneficariesAddress)
            'End If

            'Vipul 01Feb06 Cache-Session
            'dsBanking = Cache("BankingDtls")
            dsBanking = Session("BankingDtls")
            'Vipul 01Feb06 Cache-Session

            If Not dsBanking Is Nothing Then
                If Not dsBanking.Tables(0).GetChanges Is Nothing Then
                    If dsBanking.Tables(0).GetChanges.Rows.Count > 0 Then
                        blnChanged = True
                    End If
                End If

                YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertBankingInfo(dsBanking)

                'Added By Ashutosh Patil as on 11-Jan-2007 For YREN-2979 
                'Start Ashutosh Patil
                If Not Session("payment_Type") Is Nothing Then
                    If Session("payment_Type") = "Check" Then
                        'Commented By Ashutosh Patil as on 16-01-2007
                        'Me.LabelCurrentEFT.Text = "Current Pay Method : Check"
                        Me.ButtonBankingInfoCheckPayment.Visible = False 'Commented by Dilip yadav : 01-Oct-09

                    Else
                        'Commented By Ashutosh Patil as on 16-01-2007
                        'Me.LabelCurrentEFT.Text = "Current Pay Method : EFT"
                    End If
                Else
                    'Commented By Ashutosh Patil as on 16-01-2007
                    'l_dataset_Retirees = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpRetireesStatus(l_string_PersId)
                    'If l_dataset_Retirees.Tables(0).Rows.Count = 0 Then
                    '    Me.LabelCurrentEFT.Text = "Current Pay Method : Check"
                    'ElseIf (l_dataset_Retirees.Tables(0).Rows.Count > 0 And (Not RTrim(l_dataset_Retirees.Tables(0).Rows(0)(9)).Equals("O"))) Then
                    '    Me.LabelCurrentEFT.Text = "Current Pay Method : Check"
                    'Else
                    '    Me.LabelCurrentEFT.Text = "Current Pay Method : EFT"
                    'End If
                End If
                'End Ashutosh Patil

                dsBanking.AcceptChanges()
                If blnChanged = True Then
                    dsBanking = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpBanks(Session("PersId"))
                    blnChanged = False
                End If
                'Vipul 01Feb06 Cache-Session
                'Cache.Add("BankingDtls", dsBanking)
                Session("BankingDtls") = dsBanking
                'Vipul 01Feb06 Cache-Session

                ''Me.DataGridBankInfoList.DataSource = dsBanking
                ''Me.DataGridBankInfoList.DataBind()
                '''Added By Ashutosh Patil as on 11-Jan-2007 For YREN-2979 
                '''Start Ashutosh Patil
                ''Dim dtValues As New DataTable
                ''dtValues.Clear()
                ''dtValues = dsBanking.Tables(0)
                '''dtValues.Select("chrEFTStatus='0'")
                ''dtValues.Select("EffecDate=MAX(EffecDate)")
                ''If dtValues.Rows(0).Item(9) = "V" Then
                ''    Me.LabelCurrentEFT.Text = "CHECK"
                ''End If
                'End Ashutosh Patil
            End If


            Dim dtNotes As New DataTable
            Dim dsNotes As New DataSet
            'Vipul 01Feb06 Cache-Session
            'If Not Cache("dtNotes") Is Nothing Then
            '    dtNotes = CType(Cache("dtNotes"), DataTable).Copy
            If Not Session("dtNotes") Is Nothing Then
                dtNotes = DirectCast(Session("dtNotes"), DataTable).Copy
                'Vipul 01Feb06 Cache-Session
                dsNotes.Tables.Add(dtNotes)
            Else
                dtNotes = Nothing
            End If


            If Not dtNotes Is Nothing Then
                If Not dtNotes.GetChanges Is Nothing Then
                    If dtNotes.GetChanges.Rows.Count > 0 Then
                        blnChanged = True
                    End If
                End If

                YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertRetireeNotes(dsNotes)
                dsNotes.AcceptChanges()
                If blnChanged = True Then
                    dtNotes = YMCARET.YmcaBusinessObject.RefundRequest.MemberNotes(Session("PersId"))
                    blnChanged = False
                End If
                'Shubhrata Feb 12th 2008
                Session("Participant_Sort") = Nothing
                'Shubhrata Feb 12th 2008
                'Vipul 01Feb06 Cache-Session
                'Cache.Add("dtNotes", dtNotes)
                Session("dtNotes") = dtNotes
                'Vipul 01Feb06 Cache-Session
                Me.NotesFlag.Value = "Notes"
                Me.DataGridNotes.DataSource = dtNotes
                'Shubhrata YRSPS 4614
                Me.MakeDisplayNotesDataTable()
                'Shubhrata YRSPS 4614
                Me.DataGridNotes.DataBind()

            End If
            Dim l_string_AnnuityId As String
            'Commented BY Aparna  04/09/2007
            'If Not Session("AnnuityId") Is Nothing Then
            '    l_string_AnnuityId = Session("AnnuityId")
            '    If Not TextBoxAnnuitiesSSNo.Text.Trim = "" Then
            '        YMCARET.YmcaBusinessObject.RetireesInformationBOClass.UpdateJSBeneficiaries(l_string_AnnuityId, TextBoxAnnuitiesSSNo.Text.Trim, TextBoxAnnuitiesDOB.Text.Trim, TextBoxDateDeceased.Text.Trim, TextBoxAnnuitiesMiddleName.Text.Trim, TextBoxAnnuitiesFirstName.Text.Trim, txtSpouse.Text.Trim.ToUpper, TextBoxAnnuitiesLastName.Text.Trim)
            '    End If

            'End If
            'Dim paramDate As String

            'If Session("blnEditClick") = True Then
            '    YMCARET.YmcaBusinessObject.RetireesInformationBOClass.UpdateAnnuityJointSurvivors(Session("AnnuityId"), TextBoxAnnuitiesSSNo.Text, TextBoxAnnuitiesDOB.Text, TextBoxDateDeceased.Text, TextBoxAnnuitiesMiddleName.Text, TextBoxAnnuitiesFirstName.Text, txtSpouse.Text, TextBoxAnnuitiesLastName.Text)
            '    Session("blnEditClick") = False
            'End If
            Dim l_dataset_AnnuityDetails As DataSet
            Dim l_datatable_JSAnnuityDetails As DataTable
            If CType(Session("UpdateJSbtn_Click"), Boolean) = True Then
                If Not Session("AnnuityDetails") Is Nothing Then
                    l_dataset_AnnuityDetails = Session("AnnuityDetails")
                    l_datatable_JSAnnuityDetails = l_dataset_AnnuityDetails.Tables("JSAnnuitiesDetails")
                    If l_datatable_JSAnnuityDetails.Rows.Count > 0 Then
                        YMCARET.YmcaBusinessObject.RetireesInformationBOClass.UpdateAnnuityJointSurvivors(l_dataset_AnnuityDetails)
                        Session("UpdateJSbtn_Click") = False
                    End If
                End If
            End If

            Address.SaveAddressOfBeneficiariesByPerson(dtRetiredBeneficariesAddress)

            LoadAnnuitiesTab()

            dtNotes = Nothing
            dsNotes = Nothing
            'Updating the general info
            'By Ashutosh Patil as on 21-Jun-2007
            'Check for Duplication of SSNo
            'Commented by Anudeep for duplication
            'l_string_SSNOExist = UpadateGeneralInfo()
            'If l_string_SSNOExist <> "" Then
            '    'MessageBox.Show("YMCA-YRS", "SSNo. is Already in use for Participant -" + l_string_SSNOExist, MessageBoxButtons.Stop)
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "SSNo. is Already in use for Participant -" + l_string_SSNOExist, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            'UpadateGeneralInfo()

            'updating the address information
            'BS:2012:04:19:-restructure code
            'UpdateAddress(True, True, False, True)
            'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
            AddressWebUserControl1.AddrCode = "HOME"
            AddressWebUserControl2.AddrCode = "HOME"
            AddressWebUserControl1.EntityCode = EnumEntityCode.PERSON.ToString()
            AddressWebUserControl2.EntityCode = EnumEntityCode.PERSON.ToString()
            If Not Session("Dr_PrimaryAddress") Is Nothing Then
                Dim dr_PrimaryAddress As DataRow() = DirectCast(Session("Dr_PrimaryAddress"), DataRow())
                Dim dt_PrimaryContact As DataTable = DirectCast(Session("dt_PrimaryContactInfo"), DataTable)
                'Changed by Anudeep:20.03.2013 For telephone popup
                'UpdateAddress(True, True, dr_PrimaryAddress, AddressWebUserControl1)
                'Start:Anudeep01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                drPrimaryAddressTable = GetAddressTable(True, AddressWebUserControl1)
                If TextBoxHome.Visible Then
                    UpdateTelephone(True, True, TextBoxHome.Text, TextboxTelephone.Text, TextBoxFax.Text, TextBoxMobile.Text, dt_PrimaryContact)
                End If
                '  UpdateAddress(True, True, ds_PrimaryAddress, AddressWebUserControl1,
                'TextBoxHome.Text,
                'TextboxTelephone.Text,
                'TextBoxFax.Text,
                'TextBoxMobile.Text)
            End If
            dtAddress = drPrimaryAddressTable.Clone
            If Not Session("Dr_SecondaryAddress") Is Nothing Then
                Dim dr_SecondaryAddress As DataRow() = DirectCast(Session("Dr_SecondaryAddress"), DataRow())
                Dim dt_SecondaryContact As DataTable = DirectCast(Session("dt_SecondaryContactInfo"), DataTable)
                'Changed by Anudeep:20.03.2013 For telephone popup
                'UpdateAddress(True, True, dr_PrimaryAddress, AddressWebUserControl2)
                'Start:Anudeep01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses
                drSecondaryAddressTable = GetAddressTable(True, AddressWebUserControl2)
                'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Adding new record in datatable for secondary address
                If btnDeactivateSecondaryAddrs.Enabled = False And dr_SecondaryAddress.Length > 0 Then
                    Dim drNewRow As DataRow
                    drNewRow = dtAddress.NewRow()
                    For Each colname As DataColumn In dtAddress.Columns
                        If dr_SecondaryAddress(0).Table.Columns.Contains(colname.ColumnName) Then
                            drNewRow.Item(colname.ColumnName) = dr_SecondaryAddress(0).Item(colname.ColumnName)
                        End If
                    Next
                    dtAddress.Rows.Add(drNewRow)
                End If
                'End - Manthan | 2016.02.24 | YRS-AT-2328 | Adding new record in datatable for secondary address
                If TextBoxSecHome.Visible Then
                    UpdateTelephone(False, True, TextBoxSecHome.Text, TextBoxSecTelephone.Text, TextBoxSecFax.Text, TextBoxSecMobile.Text, dt_SecondaryContact)
                End If

                ' UpdateAddress(False, True, ds_SecondaryAddress, AddressWebUserControl2,
                'TextBoxSecHome.Text,
                'TextBoxSecTelephone.Text,
                'TextBoxSecFax.Text,
                'TextBoxSecMobile.Text)
            End If
            'Start:Anudeep01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            If HelperFunctions.isNonEmpty(drPrimaryAddressTable) Then
                dtAddress.ImportRow(drPrimaryAddressTable.Rows(0))
            End If
            If HelperFunctions.isNonEmpty(drSecondaryAddressTable) Then
                dtAddress.ImportRow(drSecondaryAddressTable.Rows(0))
            End If
            Address.SaveAddress(dtAddress)

            'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
            'Start:Added by Anudeep:22.03.2013-For Checking active as Primary 
            'Start:Anudeep:22.08.2013-YRS 5.0-1862:Add notes record when user enters address in any module.
            If Not Session("bln_Activateprimary") Is Nothing Then
                Dim dt_PrimaryContact As DataTable = DirectCast(Session("dt_PrimaryContactInfo"), DataTable)
                TelephoneWebUserControl1.UpdateTelphoneDetail(dt_PrimaryContact, True)
                Dim dt_SecondaryContact As DataTable = DirectCast(Session("dt_SecondaryContactInfo"), DataTable)
                TelephoneWebUserControl2.UpdateTelphoneDetail(dt_SecondaryContact, False)
                Session("bln_Activateprimary") = Nothing
                Dim l_str_notes As String
                Dim dr As DataRow
                Dim l_datatable_Notes As DataTable
                l_datatable_Notes = Session("dtNotes")
                If l_datatable_Notes Is Nothing Then
                    l_datatable_Notes = New DataTable
                    l_datatable_Notes.Columns.Add("UniqueID")
                    l_datatable_Notes.Columns.Add("Note")
                    l_datatable_Notes.Columns.Add("Creator")
                    l_datatable_Notes.Columns.Add("Date")
                    l_datatable_Notes.Columns.Add("bitImportant")
                End If
                dr = l_datatable_Notes.NewRow
                dr("UniqueID") = Guid.NewGuid()
                dr("Note") = Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_ACTIVATE_AS_PRIMARY_NOTE
                dr("PersonID") = Session("PersId")
                dr("Date") = Date.Now
                dr("bitImportant") = False
                l_datatable_Notes.Rows.Add(dr)
                If Not l_datatable_Notes Is Nothing Then
                    YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertRetireeNotes(l_datatable_Notes.DataSet)
                End If
            End If
            'End:Anudeep01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            Session("dtNotes") = Nothing
            LoadNotesTab()
            'End:Anudeep:22.08.2013-YRS 5.0-1862:Add notes record when user enters address in any module.
            'End:Added by Anudeep:22.03.2013-For Checking active as Primary 
            UpdateEmailInfo()



            Session("EnableSaveCancel") = False
            Me.TextBoxAnnuitiesSSNo.Enabled = False
            Me.TextBoxAnnuitiesFirstName.Enabled = False
            Me.TextBoxAnnuitiesMiddleName.Enabled = False
            Me.TextBoxAnnuitiesLastName.Enabled = False
            Me.TextBoxAnnuitiesDOB.Enabled = False
            Me.TextBoxDateDeceased.Enabled = False
            'Me.txtSpouse.Enabled = False
            Me.CheckboxSpouse.Enabled = False 'by aparna 05/09/2007
            Me.tbPricontacts.Visible = False
            Me.tbSeccontacts.Visible = False
            Me.PopcalendarDOB.Enabled = False
            Me.PopcalendarDeceased.Enabled = False

            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'Me.TextBoxAddress1.ReadOnly = True
            'Me.TextBoxAddress2.ReadOnly = True
            'Me.TextBoxAddress3.ReadOnly = True
            'Me.TextBoxCity.ReadOnly = True
            'Me.DropdownlistState.Enabled = False
            'Me.TextBoxZip.ReadOnly = True
            'Me.DropdownlistCountry.Enabled = False
            Me.AddressWebUserControl1.EnableControls = False
            'Added by Anudeep:14.03.2013 for telephone usercontrol
            'Me.TelephoneWebUserControl1.EditPrimaryContact_Enabled = False

            'Changed by Anudeep:14.03.2013 for telephone usercontrol
            'Me.TextboxTelephone.ReadOnly = True
            'Me.TextBoxFax.ReadOnly = True
            'Me.TextBoxMobile.ReadOnly = True
            'Me.TextBoxHome.ReadOnly = True
            'Me.TextBoxSecTelephone.ReadOnly = True
            'Me.TextBoxSecFax.ReadOnly = True
            'Me.TextBoxSecMobile.ReadOnly = True
            'Me.TextBoxSecHome.ReadOnly = True
            'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from Retiree and put on address control
            'Me.CheckboxSecIsBadAddress.Enabled = False
            'Me.CheckboxIsBadAddress.Enabled = False
            Me.TextBoxEmailId.ReadOnly = True
            Me.TextBoxEmailId.Enabled = True
            Me.ButtonEditAddress.Enabled = True

            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'Me.TextBoxSecAddress1.Enabled = False
            'Me.TextBoxSecAddress2.Enabled = False
            'Me.TextBoxSecAddress3.Enabled = False
            'Me.TextBoxSecCity.Enabled = False
            'Me.DropdownlistSecCountry.Enabled = False
            '' Me.TextBoxSecEmail.Enabled = False
            'Me.DropdownlistSecState.Enabled = False
            ''Commented by Aparna 17/01/2007
            ''  Me.TextBoxSecTelephone.Enabled = False
            ''Commented by Aparna 17/01/2007
            'Me.TextBoxSecZip.Enabled = False
            Me.AddressWebUserControl2.EnableControls = False
            'Added by Anudeep:14.03.2013 for telephone usercontrol
            'Me.TelephoneWebUserControl2.EditSecondaryContact_Enabled = False
            '  Me.CheckBoxActive.Enabled = False
            Me.CheckboxBadEmail.Enabled = False
            ' Me.CheckboxSecBadEmail.Enabled = False
            ' Me.CheckboxSecTextOnly.Enabled = False
            ' Me.CheckboxSecUnsubscribe.Enabled = False
            Me.CheckboxTextOnly.Enabled = False
            Me.CheckboxUnsubscribe.Enabled = False
            'BS:2012.03.09:-restructure Address Control
            'Me.TextBoxSecEffDate.Enabled = False
            'Me.TextBoxPriEffDate.Enabled = False


            Me.TextBoxGeneralSuffix.Enabled = False
            Me.TextBoxGeneralMiddleName.Enabled = False
            Me.TextBoxGeneralLastName.Enabled = False
            Me.TextBoxGeneralFirstName.Enabled = False
            Me.cboSalute.Enabled = False
            TextBoxGeneralDOB.Enabled = False
            Me.PopcalendarDate.Enabled = False

            TextBoxGeneralSSNo.Enabled = False
            Me.ButtonRetireesInfoCancel.Enabled = False
            'Me.ButtonRetireesInfoCancel.Visible = False
            'Ashutosh Patil as on 26-Mar-2007
            'YREN - 3028,YREN-3029
            'Me.ButtonRetireesInfoSave.Enabled = False
            'Me.ButtonRetireesInfoSave.Visible = True
            '' Me.ButtonSaveRetireeParticipants.Visible = False
            Me.ButtonSaveRetireeParticipants.Enabled = False
            Me.MakeLinkVisible()
            'Start: Bala: 01/05/2016: YRS-AT-1972: Saving special death process bit flag.
            If Not ViewState("InitialSpecialDeathProcessingRequiredBitFlag") = chkSpecialDeathProcess.Checked Then
                ViewState("InitialSpecialDeathProcessingRequiredBitFlag") = chkSpecialDeathProcess.Checked
                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.ModifyPerssMetaConfiguration(Session("PersId").ToString(), "IsSpecialDeathProcessingRequired", chkSpecialDeathProcess.Checked.ToString())
            End If
            'End: Bala: 01/05/2016: YRS-AT-1972: Saving special death process bit flag.
            LoadGeneraltab()
            'Added by prasad Jadhav on 15-Sep-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "false"
            IsExhaustedDBRMDSettleOptionSelected = False 'MMR | 2016.11.03 | YRS-AT-3063 | Resetting property value
        Else
            Dim l_string_ValidationMessage As String
            l_string_ValidationMessage = Session("ValidationMessage")
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_ValidationMessage, MessageBoxButtons.Stop)
            'Anudeep:06.11.2013:BT-1455:YRS 5.0-1733:Enabling the save button after displaying validation message
            Me.ButtonSaveRetireeParticipants.Enabled = True
            Me.ButtonRetireesInfoCancel.Enabled = True
            Session("EnableSaveCancel") = True
            Exit Sub
        End If

        'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
        'ClearSession()  'SB | 10/18/2017 | YRS-AT-3324 | Clearing Session variable used to check RMD generated or not
        'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 


        Catch ex As Exception
            HelperFunctions.LogException("SaveInfo", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            isStateWithholdingDataSave = Nothing
            LstSWHPerssDetail = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("AddStatewithholdingtax", "Save END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub
    '#Region "DropdownCode"
    'Sub Address_DropDownListState()
    '	Dim dataset_States As DataSet
    '	Dim datarow_state As DataRow()
    '	Dim strCountryCode As String
    '	strCountryCode = String.Empty
    '	If Session("States") Is Nothing Or Page.IsPostBack = False Then
    '		dataset_States = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()

    '		Me.DropdownlistState.DataSource = dataset_States.Tables(0)
    '		Me.DropdownlistState.DataTextField = "chvDescription"
    '		Me.DropdownlistState.DataValueField = "chvcodevalue"
    '		Me.DropdownlistState.DataBind()
    '		Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '		Me.DropdownlistState.Items(0).Value = ""
    '		Me.DropdownlistSecState.DataSource = dataset_States.Tables(0)
    '		Me.DropdownlistSecState.DataMember = "State Type"
    '		Me.DropdownlistSecState.DataTextField = "chvDescription"
    '		Me.DropdownlistSecState.DataValueField = "chvcodevalue"
    '		Me.DropdownlistSecState.DataBind()
    '		Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '		Me.DropdownlistSecState.Items(0).Value = ""
    '		Session("States") = dataset_States
    '	Else
    '		dataset_States = DirectCast(Session("States"), DataSet)
    '		'Added By Ashutosh Patil as on 14-Feb-2007
    '		'To avoid error for arguments related
    '		Me.DropdownlistState.DataSource = dataset_States.Tables(0)
    '		Me.DropdownlistState.DataTextField = "chvDescription"
    '		Me.DropdownlistState.DataValueField = "chvcodevalue"
    '		Me.DropdownlistState.DataBind()
    '		Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '		Me.DropdownlistSecState.Items(0).Value = ""
    '	End If
    '	If dataset_States.Tables(0).Rows.Count > 0 Then
    '		If Not ViewState("SelectedState") Is Nothing Then
    '			Me.DropdownlistState.SelectedValue = CType(ViewState("SelectedState"), String)
    '			datarow_state = dataset_States.Tables(0).Select("chvcodevalue='" & DropdownlistState.SelectedValue.Trim() & "' ")
    '			If datarow_state.Length > 0 Then
    '				strCountryCode = datarow_state(0)("chvCountryCode")
    '			End If

    '			'If strCountryCode.Trim().Length = 0 Then
    '			'    strCountryCode = Me.DropdownlistCountry.SelectedValue
    '			'End If

    '			Address_DropDownCountry(strCountryCode, "1")
    '		End If
    '		If Not ViewState("SelectedSecState") Is Nothing Then
    '			Me.DropdownlistSecState.SelectedValue = CType(ViewState("SelectedSecState"), String)
    '			strCountryCode = String.Empty
    '			datarow_state = dataset_States.Tables(0).Select("chvcodevalue='" & DropdownlistSecState.SelectedValue.Trim() & "' ")
    '			If datarow_state.Length > 0 Then
    '				strCountryCode = datarow_state(0)("chvCountryCode")
    '			End If

    '			If strCountryCode.Trim().Length = 0 Then
    '				strCountryCode = Me.DropdownlistSecState.SelectedValue
    '			End If

    '			Address_DropDownCountry(strCountryCode, "2")
    '		End If
    '	End If

    'End Sub
    'Sub Address_DropDownListStateSecondary()
    '	Dim dataset_States As DataSet
    '	Dim datarow_state As DataRow()
    '	Dim strCountryCode As String = String.Empty
    '	Try
    '		If Session("States") Is Nothing Or Page.IsPostBack = False Then
    '			dataset_States = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()

    '			Me.DropdownlistSecState.DataSource = dataset_States.Tables(0)
    '			Me.DropdownlistSecState.DataMember = "State Type"
    '			Me.DropdownlistSecState.DataTextField = "chvDescription"
    '			Me.DropdownlistSecState.DataValueField = "chvcodevalue"
    '			Me.DropdownlistSecState.DataBind()
    '			Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '			Me.DropdownlistSecState.Items(0).Value = ""

    '			Session("States") = dataset_States
    '		Else
    '			dataset_States = DirectCast(Session("States"), DataSet)
    '			'Added By Ashutosh Patil as on 12-Feb-2007
    '			'Error occured during change from Activate Secondary to Primary
    '			Me.DropdownlistSecState.DataSource = dataset_States.Tables(0)
    '			Me.DropdownlistSecState.DataMember = "State Type"
    '			Me.DropdownlistSecState.DataTextField = "chvDescription"
    '			Me.DropdownlistSecState.DataValueField = "chvcodevalue"
    '			Me.DropdownlistSecState.DataBind()
    '		End If

    '		If dataset_States.Tables(0).Rows.Count > 0 Then
    '			If Not ViewState("SelectedSecState") Is Nothing Then
    '				Me.DropdownlistSecState.SelectedValue = CType(ViewState("SelectedSecState"), String)
    '				strCountryCode = String.Empty
    '				datarow_state = dataset_States.Tables(0).Select("chvcodevalue='" & DropdownlistSecState.SelectedValue.Trim() & "' ")
    '				If datarow_state.Length > 0 Then
    '					strCountryCode = datarow_state(0)("chvCountryCode")
    '				End If

    '				'If strCountryCode.Trim().Length = 0 Then
    '				'    strCountryCode = Me.DropdownlistSecState.SelectedValue
    '				'End If

    '				Address_DropDownCountry(strCountryCode, "2")
    '			End If
    '		End If
    '	Catch
    '		Throw
    '	End Try
    'End Sub
    'Sub Address_DropDownListStatePrimary()
    '	Dim dataset_States As DataSet
    '	Dim datarow_state As DataRow()
    '	Dim strCountryCode As String = String.Empty
    '	Try
    '		If Session("States") Is Nothing Or Page.IsPostBack = False Then
    '			dataset_States = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()

    '			Me.DropdownlistState.DataSource = dataset_States.Tables(0)
    '			Me.DropdownlistState.DataTextField = "chvDescription"
    '			Me.DropdownlistState.DataValueField = "chvcodevalue"
    '			Me.DropdownlistState.DataBind()
    '			Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '			Me.DropdownlistState.Items(0).Value = ""
    '			Session("States") = dataset_States
    '		Else
    '			dataset_States = DirectCast(Session("States"), DataSet)
    '			'Added By Ashutosh Patil as on 12-Feb-2007
    '			'Error occured during change from Activate Secondary to Primary
    '			Me.DropdownlistState.DataSource = dataset_States.Tables(0)
    '			Me.DropdownlistState.DataTextField = "chvDescription"
    '			Me.DropdownlistState.DataValueField = "chvcodevalue"
    '			Me.DropdownlistState.DataBind()
    '			Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '			Me.DropdownlistState.Items(0).Value = ""
    '		End If

    '		If dataset_States.Tables(0).Rows.Count > 0 Then
    '			If Not ViewState("SelectedState") Is Nothing Then
    '				Me.DropdownlistState.SelectedValue = CType(ViewState("SelectedState"), String)
    '				datarow_state = dataset_States.Tables(0).Select("chvcodevalue='" & DropdownlistState.SelectedValue.Trim() & "' ")
    '				If datarow_state.Length > 0 Then
    '					strCountryCode = datarow_state(0)("chvCountryCode")
    '				End If

    '				'If strCountryCode.Trim().Length = 0 Then
    '				'    strCountryCode = Me.DropdownlistCountry.SelectedValue
    '				'End If

    '				Address_DropDownCountry(strCountryCode, "1")
    '			End If
    '		End If
    '	Catch
    '		Throw
    '	End Try
    'End Sub
    'Sub Address_DropDownCountry(ByVal strCountryCode As String, ByVal strDdl As String)

    '	If strDdl = "1" Then
    '		If Not ViewState("SelectedState") Is Nothing Then
    '			'By Ashutosh Patil as on 29-Jan-2007 for YREN - 3028,3029
    '			'If strCountryCode = "US" Or strCountryCode = "CA" Then
    '			'Commented By Ashutosh Patil as on 12-Feb-2007 For YREN - 3028, YREN - 3029
    '			'There will be common filter for all states
    '			'If strCountryCode = "US" Or strCountryCode = "CA" Then
    '			l_ds_States = DirectCast(Session("States"), DataSet)
    '			l_dt_State = l_ds_States.Tables(0)
    '			l_dr_State = l_dt_State.Select("chvCountryCode='" & strCountryCode & "' ")
    '			l_dt_State_filtered = l_dt_State.Clone()
    '			For Each dr In l_dr_State
    '				l_dt_State_filtered.ImportRow(dr)
    '			Next
    '			Me.DropdownlistState.DataSource() = l_dt_State_filtered
    '			Me.DropdownlistState.DataBind()
    '			Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '			Me.DropdownlistState.Items(0).Value = ""
    '			If ViewState("SelectedState") <> "" Then
    '				Me.DropdownlistState.SelectedValue = CType(ViewState("SelectedState"), String)
    '				'Added By Ashutosh Patil as on 14-Feb-2007 For YREN - 3028, YREN - 3029
    '				Me.DropdownlistCountry.SelectedValue = strCountryCode
    '			End If
    '			'End If
    '			' ViewState("SelectedState") = Nothing
    '		Else
    '			Me.DropdownlistCountry.SelectedValue = ""
    '		End If
    '		Call SetAttributes(Me.TextBoxZip, Me.DropdownlistCountry.SelectedValue.ToString)
    '		Exit Sub
    '	End If
    '	If strDdl = "2" Then
    '		If Not ViewState("SelectedSecState") Is Nothing Then
    '			'By Ashutosh Patil as on 29-Jan-2007 for YREN - 3028,3029
    '			'If strCountryCode = "US" Or strCountryCode = "CA" Then
    '			'Commented By Ashutosh Patil as on 12-Feb-2007 For YREN - 3028, YREN - 3029
    '			'There will be common filter for all states
    '			'If strCountryCode = "US" Or strCountryCode = "CA" Then
    '			l_ds_States = DirectCast(Session("States"), DataSet)
    '			l_dt_State = l_ds_States.Tables(0)
    '			l_dr_State = l_dt_State.Select("chvCountryCode='" & strCountryCode & "' ")
    '			l_dt_State_filtered = l_dt_State.Clone()
    '			For Each dr In l_dr_State
    '				l_dt_State_filtered.ImportRow(dr)
    '			Next
    '			Me.DropdownlistSecState.DataSource() = l_dt_State_filtered
    '			Me.DropdownlistSecState.DataBind()
    '			Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '			Me.DropdownlistSecState.Items(0).Value = ""
    '			If ViewState("SelectedSecState") <> "" Then
    '				Me.DropdownlistSecState.SelectedValue = CType(ViewState("SelectedSecState"), String)
    '				'Added By Ashutosh Patil as on 12-Feb-2007 For YREN - 3028, YREN - 3029
    '				Me.DropdownlistSecCountry.SelectedValue = strCountryCode
    '			End If
    '			'End If
    '		Else
    '			Me.DropdownlistSecCountry.SelectedValue = ""
    '		End If
    '	End If
    '	Call SetAttributes(Me.TextBoxSecZip, Me.DropdownlistSecCountry.SelectedValue.ToString)
    'End Sub
    '#End Region

    'Private Sub DropdownlistCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistCountry.SelectedIndexChanged
    '	Try

    '		TextBoxZip.Text = ""
    '		l_ds_States = DirectCast(Session("States"), DataSet)
    '		l_dt_State = l_ds_States.Tables(0)

    '		'Shubhrata YREN 2779
    '		If Me.DropdownlistCountry.SelectedValue = "US" Then
    '			If Me.TextBoxZip.Text.GetType.FullName.ToString = "System.String" Then
    '				Me.TextBoxZip.Text = ""
    '			End If
    '		End If
    '		' By Ashutosh Patil as on 25-Jan-2007, and on 29-jan-2007
    '		' Note : By Default the maxlength for All Countries is 10 before adding this function
    '		' YREN - 3028,YREN - 3029
    '		'Commented By Ashutosh Patil as on 12-Feb-2007 For YREN - 3028, YREN - 3029
    '		'There will be common filter for all countries
    '		'If strCountryCode = "US" Or strCountryCode = "CA" Then
    '		Call SetAttributes(TextBoxZip, DropdownlistCountry.SelectedValue.ToString)
    '		'If DropdownlistCountry.SelectedValue.ToString = "CA" Or DropdownlistCountry.SelectedValue.ToString = "US" Then
    '		l_dr_State = l_dt_State.Select("chvCountryCode='" & DropdownlistCountry.SelectedValue.ToString & "'")
    '		'End If
    '		l_dt_State_filtered = l_dt_State.Clone()
    '		For Each dr In l_dr_State
    '			l_dt_State_filtered.ImportRow(dr)
    '		Next
    '		Me.DropdownlistState.DataSource() = l_dt_State_filtered
    '		Me.DropdownlistState.DataBind()
    '		Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '		Me.DropdownlistState.Items(0).Value = ""
    '	Catch ex As Exception
    '		Dim l_String_Exception_Message As String
    '		l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '		Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '	End Try
    '	'Shubhrata YREN 2779
    'End Sub

    'Private Sub DropdownlistSecCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistSecCountry.SelectedIndexChanged
    '	Try


    '		'By Ashutosh Patil as on 25-Jan-2007, and on 29-jan-2007
    '		TextBoxSecZip.Text = ""
    '		l_ds_States = DirectCast(Session("States"), DataSet)
    '		l_dt_State = l_ds_States.Tables(0)

    '		'Shubhrata YREN 2779
    '		If Me.DropdownlistSecCountry.SelectedValue = "US" Then
    '			If Me.TextBoxSecZip.Text.GetType.FullName.ToString = "System.String" Then
    '				Me.TextBoxSecZip.Text = ""
    '			End If
    '		End If
    '		' By Ashutosh Patil as on 25-Jan-2007
    '		' Note : By Default the maxlength for All Countries is 10 before adding this function
    '		' YREN - 3028,YREN - 3029
    '		'Commented By Ashutosh Patil as on 12-Feb-2007 For YREN - 3028, YREN - 3029
    '		'There will be common filter for all countries
    '		'If strCountryCode = "US" Or strCountryCode = "CA" Then
    '		Call SetAttributes(TextBoxSecZip, DropdownlistSecCountry.SelectedValue.ToString)
    '		'If DropdownlistSecCountry.SelectedValue.ToString = "CA" Or DropdownlistSecCountry.SelectedValue.ToString = "US" Then
    '		l_dr_State = l_dt_State.Select("chvCountryCode='" & DropdownlistSecCountry.SelectedValue.ToString & "'")
    '		'End If
    '		l_dt_State_filtered = l_dt_State.Clone()
    '		For Each dr In l_dr_State
    '			l_dt_State_filtered.ImportRow(dr)
    '		Next
    '		Me.DropdownlistSecState.DataSource() = l_dt_State_filtered
    '		Me.DropdownlistSecState.DataBind()
    '		Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '		Me.DropdownlistSecState.Items(0).Value = ""

    '	Catch ex As Exception
    '		Dim l_String_Exception_Message As String
    '		l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '		Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '	End Try
    '	'Shubhrata YREN 2779
    'End Sub
    ''By aparna -YREN-3115
    Protected Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim l_datatable_Notes As New DataTable
            Dim l_string_Uniqueid As String
            Dim l_String_Search As String
            Dim l_datarow As DataRow()
            Dim l_datarowUpdated As DataRow
            l_datatable_Notes = Session("dtNotes")
            'Vipul 01Feb06 Cache-Session
            Dim l_checkbox As CheckBox = CType(sender, CheckBox)
            Dim dgItem As DataGridItem = CType(l_checkbox.NamingContainer, DataGridItem)


            If dgItem.Cells(4).Text.ToUpper <> "&NBSP;" Then
                l_string_Uniqueid = dgItem.Cells(4).Text
                l_String_Search = " UniqueID = '" + l_string_Uniqueid + "'"

                l_datarow = l_datatable_Notes.Select(l_String_Search)
                l_datarowUpdated = l_datarow(0)

                If l_checkbox.Checked = True Then
                    Me.NotesFlag.Value = "MarkedImportant"
                    l_datarowUpdated("bitImportant") = 1
                Else
                    l_datarowUpdated("bitImportant") = 0
                End If
                Session("dtNotes") = l_datatable_Notes
                l_datarow = Nothing
                l_datarow = l_datatable_Notes.Select("bitImportant = 1")
                If l_datarow.Length > 0 Then
                    Me.NotesFlag.Value = "MarkedImportant"
                Else
                    Me.NotesFlag.Value = "Notes"
                End If
                Me.DataGridNotes.DataSource = l_datatable_Notes
                'Shubhrata YRSPS 4614
                Me.MakeDisplayNotesDataTable()
                'Shubhrata YRSPS 4614
                Me.DataGridNotes.DataBind()

                ''Me.ButtonRetireesInfoSave.Visible = True
                'Me.ButtonRetireesInfoSave.Visible = False
                '' Me.ButtonSaveRetireeParticipants.Visible = True
                Me.ButtonSaveRetireeParticipants.Enabled = True
                'Me.ButtonRetireesInfoSave.Enabled = True
                Me.MakeLinkVisible()
            End If



        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Function SetAttributes(ByVal txtZipCode As TextBox, ByVal CountryValue As String)
        '************************************************************************************************************
        ' Author            : Ashutosh Patil 
        ' Created On        : 25-Jan-2007
        ' Desc              : This function will set max length of Zip Code against the respective countries
        '                     For USA-9,CANADA-6,OTHER COUNTRIES - Default is 10
        ' Related To        : YREN-3029,YREN-3028
        ' Modifed By        : 
        ' Modifed On        :
        ' Reason For Change : 
        '************************************************************************************************************
        Try
            If CountryValue = "US" Then
                txtZipCode.MaxLength = 9
                txtZipCode.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
            ElseIf CountryValue = "CA" Then
                txtZipCode.MaxLength = 6
                txtZipCode.Attributes.Add("onkeypress", "javascript:ValidateAlphaNumeric();")
            Else
                txtZipCode.MaxLength = 10
                txtZipCode.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Function
    'Private Function ValidateCountrySelStateZip(ByVal CountryValue As String, ByVal StateValue As String, ByVal str_Zip As String) As String
    '	'**********************************************************************************************************************
    '	' Author            : Ashutosh Patil 
    '	' Created On        : 25-Jan-2007
    '	' Desc              : This function will validate for the state and Zip Code selected against country USA and Canada
    '	'                     FOR USA and CANADA - State and Zip Code is mandatory
    '	' Related To        : YREN-3029,YREN-3028
    '	' Modifed By        : 
    '	' Modifed On        :
    '	' Reason For Change : 
    '	'***********************************************************************************************************************
    '	Dim l_Str_Msg As String = ""

    '	Try
    '		If (CountryValue = "US" Or CountryValue = "CA") And StateValue = "" Then
    '			l_Str_Msg = "Please select state"
    '		ElseIf (CountryValue = "US" Or CountryValue = "CA") And str_Zip = "" Then
    '			l_Str_Msg = "Please enter Zip Code"
    '		ElseIf CountryValue = "US" And (Len(str_Zip) <> 5 And Len(str_Zip) <> 9) Then
    '			l_Str_Msg = "Invalid Zip Code format"
    '		ElseIf CountryValue = "CA" And (Len(str_Zip) <> 7) Then
    '			l_Str_Msg = "Invalid Zip Code format"
    '		End If
    '		If CountryValue = "US" And l_Str_Msg <> "" Then
    '			l_Str_Msg = l_Str_Msg & " for United States"
    '		ElseIf CountryValue = "CA" And l_Str_Msg <> "" Then
    '			l_Str_Msg = l_Str_Msg & " for Canada"
    '		End If
    '		ValidateCountrySelStateZip = l_Str_Msg
    '	Catch ex As Exception
    '		Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '	End Try
    'End Function
    Private Function ValidatePhonySSNo(ByVal paramstr_MetaconfigKey As String) As String

        Dim l_str_MeaConfigValue As String
        Dim l_str_SSNoConfigValue As String
        Dim l_str_MetaConfigKey As String
        Dim l_int_MetaConfigKey As Integer
        Dim l_bool_TextSSNoMatchesConfigValue As Boolean = True

        l_str_MetaConfigKey = paramstr_MetaconfigKey
        l_str_MeaConfigValue = l_str_MetaConfigKey
        l_str_SSNoConfigValue = Me.TextBoxGeneralSSNo.Text.Trim()


        Try
            If Me.TextBoxGeneralSSNo.Text.Trim <> "" Then
                For l_int_MetaConfigKey = 0 To l_str_MeaConfigValue.Length - 1
                    If l_str_MeaConfigValue.ToCharArray.GetValue(l_int_MetaConfigKey) <> l_str_SSNoConfigValue.ToCharArray.GetValue(l_int_MetaConfigKey) Then
                        l_bool_TextSSNoMatchesConfigValue = False
                        Exit For
                    End If
                Next
            End If

            If l_bool_TextSSNoMatchesConfigValue = False Then
                ValidatePhonySSNo = "NotMatched"
            Else
                ValidatePhonySSNo = String.Empty
            End If

            Return ValidatePhonySSNo

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub DataGridNotes_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridNotes.ItemDataBound
        Try
            'Shubhrata Feb 12th 2008 to handle sorting YRSPS 4614
            'Below line commented by Dilip Yadav : 21-Sep-09
            'Dim l_datatable_Notes As DataTable
            'If Not Session("DisplayNotes") Is Nothing Then
            '    l_datatable_Notes = Session("DisplayNotes")
            'End If
            'If e.Item.ItemIndex <> -1 Then
            '    If l_datatable_Notes.Rows(e.Item.ItemIndex).IsNull("Note") = False Then
            '        If l_datatable_Notes.Rows(e.Item.ItemIndex)("Note").ToString.Length > 50 Then
            '            e.Item.Cells(3).Text = l_datatable_Notes.Rows(e.Item.ItemIndex)("Note").ToString.Substring(0, 50)
            '        Else
            '            e.Item.Cells(3).Text = l_datatable_Notes.Rows(e.Item.ItemIndex)("Note")
            '        End If
            '    End If
            'End If
            'Shubhrata Feb 12th 2008 to handle sorting

            'Added by Dilip Yadav : 21-Sep-09
            If e.Item.Cells(3).Text.Length > 100 Then
                e.Item.Cells(3).Text = e.Item.Cells(3).Text.Substring(0, 100)
            End If

            Dim l_checkbox As New CheckBox
            Dim l_linkbutton As New LinkButton 'Bala: 01/12/2016: YRS-AT-1718: Create link button control for disabling / enabling accordingly
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
                l_checkbox = e.Item.FindControl("CheckBoxImportant")
                If l_checkbox.Checked Then
                    Me.NotesFlag.Value = "MarkedImportant"
                End If
                If e.Item.Cells(2).Text.Trim = Session("LoginId") Or NotesGroupUser = True Then
                    l_checkbox.Enabled = True
                End If
                'Start: Bala: 01/12/2016: YRS-AT-1718: Create link button control for disabling / enabling accordingly
                l_linkbutton = e.Item.FindControl("DeleteNotes")
                If Not e.Item.Cells(2).Text.Trim.ToLower() = Convert.ToString(Session("LoginId")).ToLower() Then 'Manthan Rajguru | 2016.04.05 | YRS-AT-1718 | Converting login ID to lower case from both the side to avoid any mismatches
                    l_linkbutton.Enabled = False
                End If
                'End: Bala: 01/12/2016: YRS-AT-1718: Create link button control for disabling / enabling accordingly
            End If

            If Me.NotesFlag.Value = "Notes" Then
                'by Aparna -YREN-3115 09/03/2007
                Me.TabStripRetireesInformation.Items(7).Text = "<font color=orange>Notes</font>"
            ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
                Me.TabStripRetireesInformation.Items(7).Text = "<font color=red>Notes</font>"
            Else
                Me.TabStripRetireesInformation.Items(7).Text = "Notes"
            End If
            CheckReadOnlyMode()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Private Function IsCheckIsNullEffectiveDate(ByVal l_dt_EffDate As DataTable) As DataTable
        'Added By Ashutosh Patil as on 23-Mar-2007
        'YREN-3222
        Dim l_dr_EffDate As DataRow
        Dim l_date As Date
        Try
            If l_dt_EffDate.Rows.Count > 0 Then
                For Each l_dr_EffDate In l_dt_EffDate.Rows
                    If IsDBNull(l_dr_EffDate("dtmEffDate")) = True Then
                        l_dr_EffDate("dtmEffDate") = "01/01/1900"
                        l_dt_EffDate.AcceptChanges()
                    End If
                Next
            End If
            IsCheckIsNullEffectiveDate = l_dt_EffDate
            Return IsCheckIsNullEffectiveDate
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Sub SetPrimaryAddressDetails()
        Try
            m_str_Pri_Address1 = AddressWebUserControl1.Address1
            m_str_Pri_Address2 = AddressWebUserControl1.Address2
            m_str_Pri_Address3 = AddressWebUserControl1.Address3
            m_str_Pri_City = AddressWebUserControl1.City
            m_str_Pri_Zip = AddressWebUserControl1.ZipCode
            m_str_Pri_StateValue = AddressWebUserControl1.DropDownListStateValue
            m_str_Pri_CountryValue = AddressWebUserControl1.DropDownListCountryValue
            m_str_Pri_CountryText = AddressWebUserControl1.DropDownListCountryText
            m_str_Pri_StateText = AddressWebUserControl1.DropDownListStateText
            m_str_Pri_EffDate = AddressWebUserControl1.EffectiveDate
        Catch
            Throw
        End Try
    End Sub
    Private Sub SetSecondaryAddressDetails()
        Try
            m_str_Sec_Address1 = AddressWebUserControl2.Address1
            m_str_Sec_Address2 = AddressWebUserControl2.Address2
            m_str_Sec_Address3 = AddressWebUserControl2.Address3
            m_str_Sec_City = AddressWebUserControl2.City
            m_str_Sec_Zip = AddressWebUserControl2.ZipCode
            m_str_Sec_StateValue = AddressWebUserControl2.DropDownListStateValue
            m_str_Sec_CountryValue = AddressWebUserControl2.DropDownListCountryValue
            m_str_Sec_CountryText = AddressWebUserControl2.DropDownListCountryText
            m_str_Sec_StateText = AddressWebUserControl2.DropDownListStateText
            m_str_Sec_EffDate = AddressWebUserControl2.EffectiveDate
        Catch
            Throw
        End Try
    End Sub


    '	Shubhrata May 16th 2007 Plan Split Changes
    '	This function sets the visibility of the link(that takes to Retirees Screen) as true for RT,RA,RD and DR and DD status 
    Private Function MakeLinkVisible()
        Try
            Dim l_string_FundStatusType As String = ""
            If Not Session("FundStatusType") Is Nothing Then
                l_string_FundStatusType = CType(Session("FundStatusType"), String)
            End If
            If l_string_FundStatusType.Trim <> "" Then
                'Commented by Swopna -Phase IV changes (Bug id-389)-25 Apr,2008******start
                'If l_string_FundStatusType.Trim.ToUpper = "RT" Or l_string_FundStatusType.Trim.ToUpper = "RA" Or l_string_FundStatusType.Trim.ToUpper = "RD" Or l_string_FundStatusType.Trim.ToUpper = "DR" Or l_string_FundStatusType.Trim.ToUpper = "DD" Then
                '    ' If 'Me.ButtonRetireesInfoSave.Enabled = False And Me.ButtonSaveRetireeParticipants.Enabled = False Then
                '    If Me.ButtonSaveRetireeParticipants.Enabled = False Then
                '        Me.HyperLinkViewParticipantsInfo.Visible = True
                '    Else
                '        Me.HyperLinkViewParticipantsInfo.Visible = False
                '    End If

                'Else
                '    Me.HyperLinkViewParticipantsInfo.Visible = False
                'End If
                'Commented by Swopna -Phase IV changes (Bug id-389)-25 Apr,2008******end
                'Swopna -Phase IV changes(Bug id-389) -25 Apr,2008******start
                Select Case (l_string_FundStatusType.Trim.ToUpper)
                    Case "RT", "RA", "RD", "DR", "DD", "RE", "RP", "RDNP", "RPT" '----Added RPT on 20May08
                        If Me.ButtonSaveRetireeParticipants.Enabled = False Then
                            Me.HyperLinkViewParticipantsInfo.Visible = True
                        Else
                            Me.HyperLinkViewParticipantsInfo.Visible = False
                        End If
                    Case Else
                        Me.HyperLinkViewParticipantsInfo.Visible = False
                End Select
                'Swopna -Phase IV changes (Bug id-389)-25 Apr,2008******end
            Else
                Me.HyperLinkViewParticipantsInfo.Visible = False
            End If
        Catch
            Throw
        End Try
    End Function

    'Shubhrata May 16th 2007 Plan Split Changes
    'BS:2012.01.18:YRS 5.0-1497 -Add edit button to modify the date of death--This method is validate updated death date value if date is in valid format ,updated death date has to be come with in original death month and year on server side same as done on client side
    Private Function IsValidDeathDate() As String
        Dim CurrentDeathDate, NewDeathDate As DateTime
        Dim CurrentMonth, NewMonth, CurrentYear, NewYear As String
        Dim l_date As Date
        If (HiddenFieldDeathDate.Value = String.Empty OrElse TextBoxGeneralDateDeceased.Text = String.Empty) Then
            HelperFunctions.LogMessage( _
             String.Format("Error Occur While Editing Death Date Value {0}", HiddenFieldDeathDate.Value.ToString()))
            Return "Internal Error : Please contact the IT support"
        End If
        If IsDate(HiddenFieldDeathDate.Value.ToString()) = False Then
            HelperFunctions.LogMessage( _
             String.Format("Error Find in IsValidDeathDate() Invalid Date format of Original Death Date {0}", HiddenFieldDeathDate.Value.ToString()))
            Return "Internal Error : Please contact the IT support"
        End If

        If IsDate(TextBoxGeneralDateDeceased.Text.ToString()) = False Then
            HelperFunctions.LogMessage( _
            String.Format("Error Find in IsValidDeathDate() Invalid Date format of Updated Death Date {0}", TextBoxGeneralDateDeceased.Text.ToString()))
            Return False
        End If
        l_date = System.DateTime.Today
        If TextBoxGeneralDateDeceased.Text <> String.Empty Then
            If (Date.Compare(Convert.ToDateTime(TextBoxGeneralDateDeceased.Text), l_date) > 0) Then
                Return "The Deceased Date cannot be greater than today's date."
            End If
        End If
        CurrentDeathDate = Convert.ToDateTime(HiddenFieldDeathDate.Value.ToString())
        NewDeathDate = Convert.ToDateTime(TextBoxGeneralDateDeceased.Text.ToString())

        CurrentMonth = CurrentDeathDate.Month()
        CurrentYear = CurrentDeathDate.Year()

        NewMonth = NewDeathDate.Month()
        NewYear = NewDeathDate.Year()

        If (CurrentMonth <> NewMonth OrElse CurrentYear <> NewYear) Then
            Return "The new death date must be in the same month and year of the existing date of death " + CurrentDeathDate + ""
        End If
    End Function

    Private Sub ButtonSaveRetireeParticipants_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSaveRetireeParticipants.Click

        ValidationSummaryRetirees.Enabled = True  'Added By Shashi Shekhar:2009-12-23
        Dim l_string_Message As String
        Dim dtTelephone As New DataTable
        Dim drTelephone As DataRow
        Dim stTelephoneErrors As String 'PPP | 2015.10.12 | YRS-AT-2588
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            ButtonGeneralEdit.Enabled = True
            ButtonGeneralSSNoEdit.Enabled = True
            ButtonGeneralDOBEdit.Enabled = True
            btnAcctLockEditRet.Enabled = True
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            'BS:2012.01.17:YRS 5.0-1497 -Add edit button to modify the date of death :--Call IsValidDeathDate() return message
            If (HiddenFieldDeathDate.Value <> String.Empty) Then
                Dim strVerifyOutputMessage As String = IsValidDeathDate()
                If strVerifyOutputMessage <> Nothing OrElse strVerifyOutputMessage <> String.Empty Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", strVerifyOutputMessage, MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If

            If Me.IsValid Then

                ''BS:2012:04.24:YRS 5.0-1470:BT:951:check before saving data adrress is verify or not and validation for notes detail
                'If AddressWebUserControl1.QAS_Matched() = False Then
                '	MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please verify primary address detail", MessageBoxButtons.Stop)
                '	Exit Sub
                'End If
                'If AddressWebUserControl2.QAS_Matched() = False Then
                '	MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please verify secondary address detail", MessageBoxButtons.Stop)
                '	Exit Sub
                'End If

                'If AddressWebUserControl1.NotesAdded() = False Then
                '	MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please enter primary address notes detail", MessageBoxButtons.Stop)
                '	Exit Sub
                'End If
                'If AddressWebUserControl2.NotesAdded() = False Then
                '	MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please enter secondary address notes detail", MessageBoxButtons.Stop)
                '	Exit Sub
                'End If

                Session("SaveRetireeParticipantsInfo") = True 'by Aparna 11/10/2007 to check for the Request("Yes") to b set 
                Dim l_DateInFuture As Date
                Dim l_DateInPast As Date
                Dim l_PrimDate As Date
                Dim l_SecDate As Date
                Dim l_PrimaryDate As Date
                Dim l_ds_PrimaryAddress As DataSet
                Dim l_ds_SecondaryAddress As DataSet
                Dim l_SecondaryDate As Date
                Dim l_int_isBeneficiaryRequired As Integer
                Dim l_string_PersId As String = String.Empty
                Dim l_string_ErrorMessage As String = String.Empty '----------Added by Swopna BugId-389,20May08


                '   Me.ButtonRetireesInfoSave.Visible = False by Aparna 27/08/2007
                Me.MakeLinkVisible()

                'Added by Swopna BugId-389,20May08---------start  
                Dim l_AddressValidation As String
                l_AddressValidation = AddressValidation()
                l_string_ErrorMessage = l_AddressValidation
                If l_string_ErrorMessage <> "" Then
                    'Start -- Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Commented existing code and changed the message box button from OK to Stop
                    'MessageBox.Show(PlaceHolder1, "YMCA", l_string_ErrorMessage, MessageBoxButtons.OK)
                    MessageBox.Show(PlaceHolder1, "YMCA", l_string_ErrorMessage, MessageBoxButtons.Stop)
                    'End -- Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Commented existing code and changed the message box button from OK to Stop
                    Exit Sub
                End If
                'Added by Swopna BugId-389,20May08---------end
                If TextboxTelephone.Visible And (AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA") Then
                    'START: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(TextboxTelephone.Text.Trim(), TextBoxHome.Text.Trim(), TextBoxMobile.Text.Trim(), TextBoxFax.Text.Trim())
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        Session("EnableSaveCancel") = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'If TextboxTelephone.Text.Trim() <> "" Then
                    '    If TextboxTelephone.Text.Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxHome.Text.Trim() <> "" Then
                    '    If TextBoxHome.Text.Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxMobile.Text.Trim() <> "" Then
                    '    If TextBoxMobile.Text.Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxFax.Text.Trim() <> "" Then
                    '    If TextBoxFax.Text.Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'End If
                    'END: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                ElseIf HelperFunctions.isNonEmpty(Session("dt_PrimaryContactInfo")) And (AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA") Then
                    dtTelephone = Session("dt_PrimaryContactInfo")
                    'START: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(dtTelephone)
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        Session("EnableSaveCancel") = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'For Each drTelephone In dtTelephone.Rows
                    '    'Anudeep:27.08.2013 -BT-1555:YRS 5.0-1769:To check trimmed value for length
                    '    If drTelephone("PhoneNumber").ToString().Trim().Length > 0 And drTelephone("PhoneNumber").ToString().Trim().Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", drTelephone("PhoneType").ToString() + " number must be 10 digits.", MessageBoxButtons.Stop)
                    '        'Anudeep:22.08.2013-YRS 5.0-1769:Length of phone numbers
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'Next
                    'END: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                End If

                If TextBoxSecTelephone.Visible And (AddressWebUserControl2.DropDownListCountryValue = "US" Or AddressWebUserControl2.DropDownListCountryValue = "CA") Then
                    'START: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(TextBoxSecTelephone.Text.Trim(), TextBoxSecHome.Text.Trim(), TextBoxSecMobile.Text.Trim(), TextBoxSecFax.Text.Trim())
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        Session("EnableSaveCancel") = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'If TextBoxSecTelephone.Text.Trim() <> "" Then
                    '    If TextBoxSecTelephone.Text.Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxSecHome.Text.Trim() <> "" Then
                    '    If TextBoxSecHome.Text.Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxSecMobile.Text.Trim() <> "" Then
                    '    If TextBoxSecMobile.Text.Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Mobile No must be 10 digits.", MessageBoxButtons.Stop)
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxSecFax.Text.Trim() <> "" Then
                    '    If TextBoxSecFax.Text.Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'End If
                    'END: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                ElseIf HelperFunctions.isNonEmpty(Session("dt_SecondaryContactInfo")) And (AddressWebUserControl2.DropDownListCountryValue = "US" Or AddressWebUserControl2.DropDownListCountryValue = "CA") Then
                    dtTelephone = Session("dt_SecondaryContactInfo")
                    'START: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(dtTelephone)
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        Session("EnableSaveCancel") = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'For Each drTelephone In dtTelephone.Rows
                    '    'Anudeep:27.08.2013 -BT-1555:YRS 5.0-1769:To check trimmed value for length
                    '    If drTelephone("PhoneNumber").ToString().Trim().Length > 0 And drTelephone("PhoneNumber").ToString().Trim().Length <> 10 Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA", drTelephone("PhoneType").ToString() + " number must be 10 digits.", MessageBoxButtons.Stop)
                    '        'Anudeep:22.08.2013-YRS 5.0-1769:Length of phone numbers
                    '        Session("EnableSaveCancel") = True
                    '        Exit Sub
                    '    End If
                    'Next
                    'END: PPP | 2015.10.12 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                End If

                'by Aparna -make the user to save the changes 05/09/2007
                If Me.ButtonUpdateJSBeneficiary.Enabled = True Then
                    MessageBox.Show(PlaceHolder1, "YMCA", "Please Update/Cancel the modifications in Annuity JS details to continue.", MessageBoxButtons.OK)
                    Me.MakeLinkVisible()
                    Exit Sub
                End If

                'Added by Swopna 21May08,YRPS-4704----Start
                Dim dsRetiredBeneficiaries As New DataSet
                If cboGeneralMaritalStatus.SelectedValue <> "M" Then
                    Dim dRows As DataRow()

                    dsRetiredBeneficiaries = Session("BeneficiariesRetired")
                    If Not dsRetiredBeneficiaries Is Nothing Then
                        If dsRetiredBeneficiaries.Tables.Count > 0 Then
                            If dsRetiredBeneficiaries.Tables(0).Rows.Count > 0 Then
                                dRows = dsRetiredBeneficiaries.Tables(0).Select("Rel='SP'")
                                If dRows.Length > 0 Then
                                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Cannot select Marital Status other than Married as one of the beneficiary is defined as Spouse. ", MessageBoxButtons.Stop)
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                End If

                'START: PPP | 2015.10.12 | YRS-AT-2588 | Commented repeated code
                'If TextboxTelephone.Visible And (AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA") Then
                '    If TextboxTelephone.Text.Trim() <> "" Then
                '        If TextboxTelephone.Text.Length <> 10 Then
                '            'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
                '            Session("EnableSaveCancel") = True
                '            Exit Sub
                '        End If
                '    End If

                '    If TextBoxHome.Text.Trim() <> "" Then
                '        If TextBoxHome.Text.Length <> 10 Then
                '            'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
                '            'Anudeep:24.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            Session("EnableSaveCancel") = True
                '            Exit Sub
                '        End If
                '    End If

                '    If TextBoxMobile.Text.Trim() <> "" Then
                '        If TextBoxMobile.Text.Length <> 10 Then
                '            'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
                '            'Anudeep:24.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            Session("EnableSaveCancel") = True
                '            Exit Sub
                '        End If
                '    End If

                '    If TextBoxFax.Text.Trim() <> "" Then
                '        If TextBoxFax.Text.Length <> 10 Then
                '            'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
                '            'Anudeep:24.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            Session("EnableSaveCancel") = True
                '            Exit Sub
                '        End If
                '    End If
                'End If

                'If TextBoxSecTelephone.Visible And (AddressWebUserControl2.DropDownListCountryValue = "US" Or AddressWebUserControl2.DropDownListCountryValue = "CA") Then
                '    If TextBoxSecTelephone.Text.Trim() <> "" Then
                '        If TextBoxSecTelephone.Text.Length <> 10 Then
                '            'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
                '            'Anudeep:24.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            Session("EnableSaveCancel") = True
                '            Exit Sub
                '        End If
                '    End If

                '    If TextBoxSecHome.Text.Trim() <> "" Then
                '        If TextBoxSecHome.Text.Length <> 10 Then
                '            'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
                '            'Anudeep:24.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            Session("EnableSaveCancel") = True
                '            Exit Sub
                '        End If
                '    End If

                '    If TextBoxSecMobile.Text.Trim() <> "" Then
                '        If TextBoxSecMobile.Text.Length <> 10 Then
                '            'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
                '            'Anudeep:24.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            Session("EnableSaveCancel") = True
                '            Exit Sub
                '        End If
                '    End If

                '    If TextBoxSecFax.Text.Trim() <> "" Then
                '        If TextBoxSecFax.Text.Length <> 10 Then
                '            'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
                '            'Anudeep:24.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '            Session("EnableSaveCancel") = True
                '            Exit Sub
                '        End If
                '    End If
                'End If
                'END: PPP | 2015.10.12 | YRS-AT-2588 | Commented repeated code
                'Added by Swopna 21May08,YRPS-4704----End

                'commented by Aparna -YRPS -4187 16/12/2007  Need to check the existence of beneficiary and allow to save
                'BS:2012.03.13:BT:993:-After "Retired primary beneficiary not defined" prompt TextBoxGeneralDateDeceased should be  disable 
                'BS:2012.01.20:YRS 5.0-1497
                Session("EnableSaveCancel") = False
                ButtonRetireesInfoCancel.Enabled = False
                TextBoxGeneralDateDeceased.Enabled = False
                Me.MakeLinkVisible()
                'NP:PS:2007.09.04 - Adding code to perform validation for primary Retired beneficiary
                If Trim(Me.TextBoxPrimaryR.Text) = "" Then
                    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Retired primary beneficiary required.", MessageBoxButtons.OK)
                    'Exit Sub
                    l_string_PersId = Session("PersId")
                    l_int_isBeneficiaryRequired = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.IsBeneficiaryRequired(l_string_PersId)
                    'Dharmesh : 11/20/2018 : YRS-AT-4136 : Check participant is enrolled on or after 1/1/2019 then don't show up the warning message to the user in benenficiary page
                    If l_int_isBeneficiaryRequired = 1 AndAlso YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Retired primary beneficiary not defined.Do you want to continue ?", MessageBoxButtons.YesNo)
                        Exit Sub
                    End If
                End If

                Me.SaveRetiredParticipantInfo()

                'commented by Aparna -YRPS -4187 16/12/2007 
                '    l_DateInPast = Date.Now.AddYears(-1)
                '    l_DateInFuture = Date.Now.AddYears(1)

                '    If Not Session("Ds_PrimaryAddress") Is Nothing Then
                '        l_ds_PrimaryAddress = CType(Session("Ds_PrimaryAddress"), DataSet)
                '        'Primary Address section starts
                '        If l_ds_PrimaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
                '            If (l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "System.DBNull" And l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "") Then
                '                l_PrimaryDate = CType(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate"), Date)
                '            Else
                '                l_PrimaryDate = "02/02/1900"
                '            End If
                '        Else
                '            l_PrimaryDate = "02/02/1900"
                '        End If
                '    Else
                '        l_PrimaryDate = "02/02/1900"
                '    End If


                '    If Not Session("Ds_SecondaryAddress") Is Nothing Then
                '        l_ds_SecondaryAddress = CType(Session("Ds_SecondaryAddress"), DataSet)
                '        'Secondary Address section starts
                '        If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
                '            If (l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "System.DBNull" And l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate").ToString() <> "") Then
                '                l_SecondaryDate = CType(l_ds_SecondaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate"), Date)
                '            Else
                '                l_SecondaryDate = "02/02/1900"
                '            End If
                '        Else
                '            l_SecondaryDate = "02/02/1900"
                '        End If
                '    Else
                '        l_SecondaryDate = "02/02/1900"
                '    End If


                '    If Me.TextBoxPriEffDate.Text <> "" AndAlso Me.TextBoxPriEffDate.Text <> l_PrimaryDate Then
                '        l_PrimaryDate = CType(Me.TextBoxPriEffDate.Text, Date)

                '        If Date.Compare(l_DateInPast, l_PrimaryDate) > 0 Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
                '            ' Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If

                '        If Date.Compare(l_PrimaryDate, l_DateInFuture) > 0 Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
                '            'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If

                '    End If



                '    If Me.TextBoxSecEffDate.Text <> "" AndAlso Me.TextBoxSecEffDate.Text <> l_SecondaryDate Then
                '        l_SecondaryDate = CType(Me.TextBoxSecEffDate.Text, Date)

                '        If Date.Compare(l_DateInPast, l_SecondaryDate) > 0 Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
                '            '  Me.ButtonRetireesInfoSave.Visible = False ''by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If

                '        If Date.Compare(l_SecondaryDate, l_DateInFuture) > 0 Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
                '            ' Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If

                '    End If
                'End If

                ''YREN - 3028,YREN-3029
                ''Primary Address
                ''By Ashutosh Patil as on 25-Apr-2007
                ''YREN-3298
                'If Me.ButtonEditAddress.Enabled = False Then
                '    Call SetPrimaryAddressDetails()

                '    'Secondary Address
                '    Call SetSecondaryAddressDetails()

                '    'Added By Ashutosh Patil as on 23-Feb-2007 For YREN - 3029,YREN - 3028
                '    If Trim(m_str_Pri_Address1) = "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Primary Address.", MessageBoxButtons.Stop)
                '        ' Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '        Me.MakeLinkVisible()
                '        Exit Sub
                '    End If

                '    If Trim(m_str_Pri_City) = "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Primary Address.", MessageBoxButtons.Stop)
                '        'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '        Me.MakeLinkVisible()
                '        Exit Sub
                '    End If

                '    If Trim(m_str_Pri_Address1) <> "" Then
                '        If m_str_Pri_CountryText = "-Select Country-" And m_str_Pri_StateText = "-Select State-" Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
                '            '  Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If
                '    End If

                '    If (m_str_Pri_CountryText = "-Select Country-") Then
                '        If m_str_Pri_StateText = "-Select State-" Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
                '            ' Me.ButtonRetireesInfoSave.Visible = False'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If
                '    End If

                '    l_string_Message = ValidateCountrySelStateZip(m_str_Pri_CountryValue, m_str_Pri_StateValue, m_str_Pri_Zip)
                '    If l_string_Message <> "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                '        'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '        Me.MakeLinkVisible()
                '        Exit Sub
                '    End If

                '    If m_str_Pri_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Pri_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
                '        'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '        Me.MakeLinkVisible()
                '        Exit Sub
                '    End If

                '    'Secondary Address Validations
                '    'Ashutosh Patil as on 14-Mar-2007
                '    'YREN - 3028,YREN-3029
                '    If Trim(m_str_Sec_Address1) <> "" Then
                '        If Trim(m_str_Sec_City) = "" Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Secondary Address.", MessageBoxButtons.Stop)
                '            'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If
                '        If (m_str_Sec_CountryText = "-Select Country-") Then
                '            If m_str_Sec_StateText = "-Select State-" Then
                '                MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Secondary Address.", MessageBoxButtons.Stop)
                '                'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '                Me.MakeLinkVisible()
                '                Exit Sub
                '            End If
                '        End If
                '    End If

                '    If Trim(m_str_Sec_Address2) <> "" Then
                '        If Trim(m_str_Sec_Address1) = "" Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '            'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If
                '    End If

                '    If Trim(m_str_Sec_Address3) <> "" Then
                '        If Trim(m_str_Sec_Address1) = "" Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '            'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If
                '    End If

                '    If m_str_Sec_City <> "" Then
                '        If Trim(m_str_Sec_Address1.ToString) = "" Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '            'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If
                '    End If

                '    If (m_str_Sec_CountryText <> "-Select Country-" Or m_str_Sec_StateText <> "-Select State-") Then
                '        If Trim(m_str_Sec_Address1) = "" Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '            'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If
                '    End If

                '    If Trim(m_str_Sec_Zip) <> "" Then
                '        If Trim(m_str_Sec_Address1) = "" Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '            'Me.ButtonRetireesInfoSave.Visible = False  'by Aparna 27/08/07
                '            Me.MakeLinkVisible()
                '            Exit Sub
                '        End If
                '    End If

                '    l_string_Message = ValidateCountrySelStateZip(m_str_Sec_CountryValue, m_str_Sec_StateValue, m_str_Sec_Zip)

                '    If l_string_Message <> "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                '        'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                '        Me.MakeLinkVisible()
                '        Exit Sub
                '    End If

                '    If m_str_Sec_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Sec_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
                '        'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07

                '        Me.MakeLinkVisible()
                '        Exit Sub

                '    End If
                'End If

                ''Added By Ashutosh Patil as on 06-Jun-2007
                ''Validations for Phony SSNo
                ''YREN-3490  

                'If Me.TextBoxGeneralSSNo.Enabled = True Then
                '    l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.TextBoxGeneralSSNo.Text.Trim())

                '    If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
                '        Session("PhonySSNo") = "Not_Phony_SSNo"
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Invalid SSNo entered, Please enter a Valid SSNo.", MessageBoxButtons.Stop)
                '        Exit Sub
                '    ElseIf l_string_Message.ToString().Trim() = "Phony_SSNo" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Phony SSNo ?", MessageBoxButtons.YesNo)
                '        Session("PhonySSNo") = "Phony_SSNo"
                '        Exit Sub
                '    ElseIf l_string_Message.ToString().Trim() = "No_Configuration_Key" Then
                '        Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
                '    End If
            End If
            'Call SaveInfo()
            'Me.ButtonRetireesInfoSave.Visible = True 'by Aparna 27/08/07
            '' Me.ButtonSaveRetireeParticipants.Visible = False
            If Not Session("ValidationMessage") Is Nothing Then
                'Anudeep:06.11.2013:BT-1455:YRS 5.0-1733:Enabling the save button after displaying validation message
                Me.ButtonSaveRetireeParticipants.Enabled = True
                Me.ButtonRetireesInfoCancel.Enabled = True
                Session("EnableSaveCancel") = True
            Else
                Me.ButtonSaveRetireeParticipants.Enabled = False
                Me.ButtonRetireesInfoCancel.Enabled = False
                Me.ButtonRetireesInfoOK.Enabled = True
                Session("EnableSaveCancel") = False
            End If
            'by Aparna 04/09/2007
            Me.ReqGeneralDOB.Enabled = False 'disabling the Required field validator.
            Me.IsExhaustedDBRMDSettleOptionSelected = False ' MMR | 2016.10.14 | YRS-AT-3063 | Resetting property value

        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            If sqlEx.Number = 60006 And sqlEx.Procedure.ToString = "yrs_usp_AMCM_SearchConfigurationMaintenance" Then
                'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                'l_String_Exception_Message = "No Key defined for Phony SSNo in AtsMetaConfiguration."
                l_String_Exception_Message = "No Key defined for Placeholder SSNo in AtsMetaConfiguration."
                'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
            Else
                l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            End If
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub SaveRetiredParticipantInfo()
        Dim l_DateInFuture As Date
        Dim l_DateInPast As Date
        Dim l_PrimDate As Date
        Dim l_SecDate As Date
        Dim l_PrimaryDate As Date
        Dim l_dr_PrimaryAddress As DataRow()
        Dim l_dr_SecondaryAddress As DataRow()
        Dim l_SecondaryDate As Date
        Dim l_string_Message As String = String.Empty
        Dim l_AddressValidation As String = String.Empty
        Dim l_string_ErrorMessage As String = String.Empty
        Try
            Page.Validate()
            If Me.IsValid Then
                l_DateInPast = Date.Now.AddYears(-1)
                l_DateInFuture = Date.Now.AddYears(1)

                If Not Session("Dr_PrimaryAddress") Is Nothing Then
                    l_dr_PrimaryAddress = DirectCast(Session("Dr_PrimaryAddress"), DataRow())
                    'Primary Address section starts
                    If l_dr_PrimaryAddress.Length > 0 Then
                        If (l_dr_PrimaryAddress(0).Item("effectiveDate").ToString() <> "System.DBNull" And l_dr_PrimaryAddress(0).Item("effectiveDate").ToString() <> "") Then
                            l_PrimaryDate = CType(l_dr_PrimaryAddress(0).Item("effectiveDate"), Date)
                        Else
                            l_PrimaryDate = "02/02/1900"
                        End If
                    Else
                        l_PrimaryDate = "02/02/1900"
                    End If
                Else
                    l_PrimaryDate = "02/02/1900"
                End If


                If Not Session("Dr_SecondaryAddress") Is Nothing Then
                    l_dr_SecondaryAddress = DirectCast(Session("Dr_SecondaryAddress"), DataRow())
                    'Secondary Address section starts
                    If l_dr_SecondaryAddress.Length > 0 Then
                        If (l_dr_SecondaryAddress(0).Item("effectiveDate").ToString() <> "System.DBNull" And l_dr_SecondaryAddress(0).Item("effectiveDate").ToString() <> "") Then
                            l_SecondaryDate = CType(l_dr_SecondaryAddress(0).Item("effectiveDate"), Date)
                        Else
                            l_SecondaryDate = "02/02/1900"
                        End If
                    Else
                        l_SecondaryDate = "02/02/1900"
                    End If
                Else
                    l_SecondaryDate = "02/02/1900"
                End If


                If Me.m_str_Pri_EffDate <> "" AndAlso Me.m_str_Pri_EffDate <> l_PrimaryDate Then
                    l_PrimaryDate = CType(Me.m_str_Pri_EffDate, Date)

                    If Date.Compare(l_DateInPast, l_PrimaryDate) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
                        ' Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                        Me.MakeLinkVisible()
                        Exit Sub
                    End If

                    If Date.Compare(l_PrimaryDate, l_DateInFuture) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
                        'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                        Me.MakeLinkVisible()
                        Exit Sub
                    End If

                End If



                If Me.m_str_Sec_EffDate <> "" AndAlso Me.m_str_Sec_EffDate <> l_SecondaryDate Then
                    l_SecondaryDate = CType(Me.m_str_Sec_EffDate, Date)

                    If Date.Compare(l_DateInPast, l_SecondaryDate) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
                        '  Me.ButtonRetireesInfoSave.Visible = False ''by Aparna 27/08/07
                        Me.MakeLinkVisible()
                        Exit Sub
                    End If

                    If Date.Compare(l_SecondaryDate, l_DateInFuture) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
                        ' Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
                        Me.MakeLinkVisible()
                        Exit Sub
                    End If

                End If
            End If

            'If Me.ButtonEditAddress.Enabled = False Then
            '    'Call SetPrimaryAddressDetails()

            '    'Secondary Address
            '    'Call SetSecondaryAddressDetails()

            'l_AddressValidation = AddressValidation()
            'l_string_ErrorMessage = l_AddressValidation
            'If l_string_ErrorMessage <> "" Then
            '    Me.MakeLinkVisible()
            '    MessageBox.Show(PlaceHolder1, "YMCA", l_string_ErrorMessage, MessageBoxButtons.OK)
            '    Exit Sub
            'End If

            'If TextboxTelephone.Visible And (AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA") Then
            '    If TextboxTelephone.Text.Trim() <> "" Then
            '        If TextboxTelephone.Text.Length <> 10 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
            '            Exit Sub
            '        End If
            '    End If

            '    If TextBoxHome.Text.Trim() <> "" Then
            '        If TextBoxHome.Text.Length <> 10 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
            '            Exit Sub
            '        End If
            '    End If

            '    If TextBoxMobile.Text.Trim() <> "" Then
            '        If TextBoxMobile.Text.Length <> 10 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
            '            Exit Sub
            '        End If
            '    End If

            '    If TextBoxFax.Text.Trim() <> "" Then
            '        If TextBoxFax.Text.Length <> 10 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
            '            Exit Sub
            '        End If
            '    End If
            'End If

            'If TextBoxSecTelephone.Visible And (AddressWebUserControl2.DropDownListCountryValue = "US" Or AddressWebUserControl2.DropDownListCountryValue = "CA") Then
            '    If TextBoxSecTelephone.Text.Trim() <> "" Then
            '        If TextBoxSecTelephone.Text.Length <> 10 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
            '            Exit Sub
            '        End If
            '    End If

            '    If TextBoxSecHome.Text.Trim() <> "" Then
            '        If TextBoxSecHome.Text.Length <> 10 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
            '            Exit Sub
            '        End If
            '    End If

            '    If TextBoxSecMobile.Text.Trim() <> "" Then
            '        If TextBoxSecMobile.Text.Length <> 10 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
            '            Exit Sub
            '        End If
            '    End If

            '    If TextBoxSecFax.Text.Trim() <> "" Then
            '        If TextBoxSecFax.Text.Length <> 10 Then
            '        MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'Added By Ashutosh Patil as on 23-Feb-2007 For YREN - 3029,YREN - 3028
            'If Trim(m_str_Pri_Address1) = "" Then
            '	MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Primary Address.", MessageBoxButtons.Stop)
            '	' Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '	Me.MakeLinkVisible()
            '	Exit Sub
            'End If

            '    '    If TextBoxSecFax.Text.Trim() <> "" Then
            '    '        If TextBoxSecFax.Text.Length <> 10 Then
            '    '            MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
            '    '            Exit Sub
            '    '        End If
            '    '    End If
            '    'End If
            '    'Added By Ashutosh Patil as on 23-Feb-2007 For YREN - 3029,YREN - 3028
            '    'If Trim(m_str_Pri_Address1) = "" Then
            '    '	MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Primary Address.", MessageBoxButtons.Stop)
            '    '	' Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '	Me.MakeLinkVisible()
            '    '	Exit Sub
            '    'End If

            '    'If Trim(m_str_Pri_City) = "" Then
            '    '	MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Primary Address.", MessageBoxButtons.Stop)
            '    '	'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '	Me.MakeLinkVisible()
            '    '	Exit Sub
            '    'End If

            '    'If Trim(m_str_Pri_Address1) <> "" Then
            '    '	If m_str_Pri_CountryText = "-Select Country-" And m_str_Pri_StateText = "-Select State-" Then
            '    '		MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
            '    '		'  Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '		Me.MakeLinkVisible()
            '    '		Exit Sub
            '    '	End If
            '    'End If

            '    'If (m_str_Pri_CountryText = "-Select Country-") Then
            '    '	If m_str_Pri_StateText = "-Select State-" Then
            '    '		MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
            '    '		' Me.ButtonRetireesInfoSave.Visible = False'by Aparna 27/08/07
            '    '		Me.MakeLinkVisible()
            '    '		Exit Sub
            '    '	End If
            '    'End If

            '    'l_string_Message = ValidateCountrySelStateZip(m_str_Pri_CountryValue, m_str_Pri_StateValue, m_str_Pri_Zip)
            '    'If l_string_Message <> "" Then
            '    '	MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
            '    '	'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '	Me.MakeLinkVisible()
            '    '	Exit Sub
            '    'End If

            '    'If m_str_Pri_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Pri_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
            '    '	MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
            '    '	'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '	Me.MakeLinkVisible()
            '    '	Exit Sub
            '    'End If

            '    ''Secondary Address Validations
            '    ''Ashutosh Patil as on 14-Mar-2007
            '    ''YREN - 3028,YREN-3029
            '    'If Trim(m_str_Sec_Address1) <> "" Then
            '    '	If Trim(m_str_Sec_City) = "" Then
            '    '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Secondary Address.", MessageBoxButtons.Stop)
            '    '		'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '		Me.MakeLinkVisible()
            '    '		Exit Sub
            '    '	End If
            '    '	If (m_str_Sec_CountryText = "-Select Country-") Then
            '    '		If m_str_Sec_StateText = "-Select State-" Then
            '    '			MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Secondary Address.", MessageBoxButtons.Stop)
            '    '			'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '			Me.MakeLinkVisible()
            '    '			Exit Sub
            '    '		End If
            '    '	End If
            '    'End If

            '    'If Trim(m_str_Sec_Address2) <> "" Then
            '    '	If Trim(m_str_Sec_Address1) = "" Then
            '    '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
            '    '		'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '		Me.MakeLinkVisible()
            '    '		Exit Sub
            '    '	End If
            '    'End If

            '    'If Trim(m_str_Sec_Address3) <> "" Then
            '    '	If Trim(m_str_Sec_Address1) = "" Then
            '    '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
            '    '		'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '		Me.MakeLinkVisible()
            '    '		Exit Sub
            '    '	End If
            '    'End If

            '    'If m_str_Sec_City <> "" Then
            '    '	If Trim(m_str_Sec_Address1.ToString) = "" Then
            '    '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
            '    '		'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '		Me.MakeLinkVisible()
            '    '		Exit Sub
            '    '	End If
            '    'End If

            '    'If (m_str_Sec_CountryText <> "-Select Country-" Or m_str_Sec_StateText <> "-Select State-") Then
            '    '	If Trim(m_str_Sec_Address1) = "" Then
            '    '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
            '    '		'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '		Me.MakeLinkVisible()
            '    '		Exit Sub
            '    '	End If
            '    'End If

            '    'If Trim(m_str_Sec_Zip) <> "" Then
            '    '	If Trim(m_str_Sec_Address1) = "" Then
            '    '		MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
            '    '		'Me.ButtonRetireesInfoSave.Visible = False  'by Aparna 27/08/07
            '    '		Me.MakeLinkVisible()
            '    '		Exit Sub
            '    '	End If
            '    'End If

            '    'l_string_Message = ValidateCountrySelStateZip(m_str_Sec_CountryValue, m_str_Sec_StateValue, m_str_Sec_Zip)

            '    'If l_string_Message <> "" Then
            '    '	MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
            '    '	'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07
            '    '	Me.MakeLinkVisible()
            '    '	Exit Sub
            '    'End If

            '    'If m_str_Sec_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Sec_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
            '    '	MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
            '    '	'Me.ButtonRetireesInfoSave.Visible = False 'by Aparna 27/08/07

            '    '	Me.MakeLinkVisible()
            '    '	Exit Sub

            '    'End If
            'End If
            'End If

            'Added By Ashutosh Patil as on 06-Jun-2007
            'Validations for Phony SSNo
            'YREN-3490  

            'If Me.TextBoxGeneralSSNo.Enabled = True Then ''SP :2014.02.05 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN
            If (IsSSNEdited()) Then 'SP :2014.02.05 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN
                l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.TextBoxGeneralSSNo.Text.Trim())

                If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
                    Session("PhonySSNo") = "Not_Phony_SSNo"
                    MessageBox.Show(PlaceHolder1, "YMCA", "Invalid SSNo entered, Please enter a Valid SSNo.", MessageBoxButtons.Stop)
                    Exit Sub
                ElseIf l_string_Message.ToString().Trim() = "Phony_SSNo" Then
                    'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                    'MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Phony SSNo ?", MessageBoxButtons.YesNo)
                    MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Placeholder SSNo ?", MessageBoxButtons.YesNo)
                    'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                    Session("PhonySSNo") = "Phony_SSNo"
                    Exit Sub
                ElseIf l_string_Message.ToString().Trim() = "No_Configuration_Key" Then
                    'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                    'Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
                    Throw New Exception("No Key defined for Placeholder SSNo in AtsMetaConfiguration.")
                    'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 

                End If
            End If

            'START : ML | 2019.11.26 |YRS-AT-4598 | State Tax Withholding MA validation 
            ValidateSTWvsFedtaxforMA()
            'END : ML |2019.11.26 | YRS-AT-4598 | State Tax Withholding MA validation 

            Call SaveInfo()
            'Me.ButtonRetireesInfoSave.Visible = True 'by Aparna 27/08/07
            '' Me.ButtonSaveRetireeParticipants.Visible = False
            Me.ButtonSaveRetireeParticipants.Enabled = False
            Me.ButtonRetireesInfoCancel.Enabled = False
            Me.ButtonRetireesInfoOK.Enabled = True
            Me.MakeLinkVisible()
            'by Aparna 04/09/2007
            Me.ReqGeneralDOB.Enabled = False 'disabling the Required field validator. 
        Catch sqlEx As SqlException
            Dim l_String_Exception_Message As String
            If sqlEx.Number = 60006 And sqlEx.Procedure.ToString = "yrs_usp_AMCM_SearchConfigurationMaintenance" Then
                'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                'l_String_Exception_Message = "No Key defined for Phony SSNo in AtsMetaConfiguration."
                l_String_Exception_Message = "No Key defined for Placeholder SSNo in AtsMetaConfiguration."
                'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
            Else
                l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            End If
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message, False)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)


        End Try
    End Sub

    'by Aparna 04/09/2007 -show details of the annuity selected in the datagrid after this button is clicked
    Private Sub ButtonAnnuitiesViewDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAnnuitiesViewDetails.Click
        Try
            Dim popupscript As String = "<script language='javascript'>" & _
                       "window.open('showannuitydetails.aspx?annuityid=" & Me.DataGridAnnuities.SelectedItem.Cells(8).Text & "', 'custompopup', " & _
                       "'width=750, height=500, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
                       "</script>"


            Page.RegisterStartupScript("popupscript1", popupscript)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'by Aparna 05/09/2007
    'update the Dataset with the values modified for the JS Beneficiary.
    Private Sub ButtonUpdateJSBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUpdateJSBeneficiary.Click
        ' // START: SB | 07/07/2016 | YRS-AT-2382 | Moved entire code to new function "UpdateAnnuityBeneficiary"  
        'Dim dgitem As DataGridItem
        ''Dim l_ImageButton As ImageButton
        'Dim l_string_Message As String = String.Empty
        ''Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
        'HiddenFieldDirty.Value = "true"
        'Try
        '    Session("UpdateJSbtn_Click") = True 'by Aparna 11/10/2007
        '    'enable the Reqd field validator
        '    'check if the DOB is greater than today.
        '    If TextBoxAnnuitiesDOB.Text <> String.Empty Then
        '        Dim l_dateofbirth As DateTime = Convert.ToDateTime(TextBoxAnnuitiesDOB.Text)
        '        If l_dateofbirth >= Convert.ToDateTime(DateTime.Today) Then
        '            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Date of birth cannot be greater than today.", MessageBoxButtons.Stop)
        '            Exit Sub
        '        End If
        '    End If

        '    If Me.TextBoxAnnuitiesSSNo.Text <> String.Empty Then
        '        l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.TextBoxAnnuitiesSSNo.Text.Trim())

        '        If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
        '            Session("PhonySSNo") = "Not_Phony_SSNo"
        '            MessageBox.Show(PlaceHolder1, "YMCA", "Invalid SSNo entered, Please enter a Valid SSNo.", MessageBoxButtons.Stop)
        '            Exit Sub
        '        ElseIf l_string_Message.ToString().Trim() = "Phony_SSNo" Then
        '            MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Phony SSNo ?", MessageBoxButtons.YesNo)
        '            Session("PhonySSNo") = "Phony_SSNo"
        '            Exit Sub
        '        ElseIf l_string_Message.ToString().Trim() = "No_Configuration_Key" Then
        '            Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
        '        End If
        '    Else
        '        MessageBox.Show(PlaceHolder1, "YMCA", "Please enter a Valid SSNo.", MessageBoxButtons.Stop)
        '        Exit Sub
        '    End If

        '    l_string_Message = UpdateJointSurvivorDetails()
        '    'by Aaprna 11/10/2007
        '    If l_string_Message <> String.Empty Then
        '        MessageBox.Show(PlaceHolder1, "YMCA", "Error Occured while updating the data.", MessageBoxButtons.Stop)
        '        Exit Sub
        '    End If
        '    'commented by aparna 
        '    ''enable the selection in the datgrid
        '    'For Each dgitem In Me.DataGridAnnuities.Items
        '    '    l_ImageButton = dgitem.FindControl("Imagebutton6")
        '    '    l_ImageButton.Enabled = True
        '    'Next
        '    'Session("UpdateJSbtn_Click") = True
        '    'Me.ButtonUpdateJSBeneficiary.Enabled = False
        '    'Me.ButtonEditJSBeneficiary.Enabled = True
        '    'Me.ButtonCancelJSBeneficiary.Enabled = False
        '    'Session("EnableSaveCancel") = True
        '    'Me.ButtonSaveRetireeParticipants.Enabled = True
        '    'Me.TextBoxAnnuitiesSSNo.Enabled = False
        '    'Me.TextBoxAnnuitiesFirstName.Enabled = False
        '    'Me.TextBoxAnnuitiesMiddleName.Enabled = False
        '    'Me.TextBoxAnnuitiesLastName.Enabled = False
        '    'Me.TextBoxAnnuitiesDOB.Enabled = False
        '    'Me.TextBoxDateDeceased.Enabled = False
        '    'Me.ButtonRetireesInfoOK.Enabled = False
        '    ''Me.txtSpouse.Enabled = False
        '    'Me.CheckboxSpouse.Enabled = False 'by aparna 05/09/2007


        '    'Me.PopcalendarDOB.Enabled = False
        '    'Me.PopcalendarDeceased.Enabled = False
        'Catch ex As Exception
        '    Dim l_String_Exception_Message As String
        '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
        '    Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        'End Try
        UpdateAnnuityBeneficiary()
        ' // END: SB | 07/07/2016 | YRS-AT-2382 | Moved entire code to new function "UpdateAnnuityBeneficiary"  
    End Sub

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | For Handling Button Update JS Annuity Button click 
    Private Sub UpdateAnnuityBeneficiary()
        Dim dgitem As DataGridItem
        'Dim l_ImageButton As ImageButton
        Dim l_string_Message As String = String.Empty
        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
        HiddenFieldDirty.Value = "true"
        Try
            Session("UpdateJSbtn_Click") = True 'by Aparna 11/10/2007
            'enable the Reqd field validator
            'check if the DOB is greater than today.
            If TextBoxAnnuitiesDOB.Text <> String.Empty Then
                Dim l_dateofbirth As DateTime = Convert.ToDateTime(TextBoxAnnuitiesDOB.Text)
                If l_dateofbirth >= Convert.ToDateTime(DateTime.Today) Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Date of birth cannot be greater than today.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If

            If Me.TextBoxAnnuitiesSSNo.Text <> String.Empty Then
                l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.TextBoxAnnuitiesSSNo.Text.Trim())

                If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
                    Session("PhonySSNo") = "Not_Phony_SSNo"
                    MessageBox.Show(PlaceHolder1, "YMCA", "Invalid SSNo entered, Please enter a Valid SSNo.", MessageBoxButtons.Stop)
                    Exit Sub
                ElseIf l_string_Message.ToString().Trim() = "Phony_SSNo" Then
                    'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                    'MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Phony SSNo ?", MessageBoxButtons.YesNo)
                    MessageBox.Show(PlaceHolder1, "YMCA", "Are you sure you want to continue with Placeholder SSNo ?", MessageBoxButtons.YesNo)
                    'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                    Session("PhonySSNo") = "Phony_SSNo"
                    Exit Sub
                ElseIf l_string_Message.ToString().Trim() = "No_Configuration_Key" Then
                    'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                    'Throw New Exception("No Key defined for Phony SSNo in AtsMetaConfiguration.")
                    Throw New Exception("No Key defined for Placeholder SSNo in AtsMetaConfiguration.")
                    'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
                End If
            Else
                MessageBox.Show(PlaceHolder1, "YMCA", "Please enter a Valid SSNo.", MessageBoxButtons.Stop)
                Exit Sub
            End If

            l_string_Message = UpdateJointSurvivorDetails()
            'by Aaprna 11/10/2007
            If l_string_Message <> String.Empty Then
                MessageBox.Show(PlaceHolder1, "YMCA", "Error Occured while updating the data.", MessageBoxButtons.Stop)
                Exit Sub
            End If
            'commented by aparna 
            ''enable the selection in the datgrid
            'For Each dgitem In Me.DataGridAnnuities.Items
            '    l_ImageButton = dgitem.FindControl("Imagebutton6")
            '    l_ImageButton.Enabled = True
            'Next
            'Session("UpdateJSbtn_Click") = True
            'Me.ButtonUpdateJSBeneficiary.Enabled = False
            'Me.ButtonEditJSBeneficiary.Enabled = True
            'Me.ButtonCancelJSBeneficiary.Enabled = False
            'Session("EnableSaveCancel") = True
            'Me.ButtonSaveRetireeParticipants.Enabled = True
            'Me.TextBoxAnnuitiesSSNo.Enabled = False
            'Me.TextBoxAnnuitiesFirstName.Enabled = False
            'Me.TextBoxAnnuitiesMiddleName.Enabled = False
            'Me.TextBoxAnnuitiesLastName.Enabled = False
            'Me.TextBoxAnnuitiesDOB.Enabled = False
            'Me.TextBoxDateDeceased.Enabled = False
            'Me.ButtonRetireesInfoOK.Enabled = False
            ''Me.txtSpouse.Enabled = False
            'Me.CheckboxSpouse.Enabled = False 'by aparna 05/09/2007


            'Me.PopcalendarDOB.Enabled = False
            'Me.PopcalendarDeceased.Enabled = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | For Handling Button Update JS Annuity Button click 
    Private Sub ButtonCancelJSBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancelJSBeneficiary.Click
        Dim dgitem As DataGridItem
        'Dim l_ImageButton As ImageButton
        Try
            'load the original values back in the textboxes.
            LoadJointSurvivorDetails()
            'enable the selection in the datgrid
            'For Each dgitem In Me.DataGridAnnuities.Items
            '    l_ImageButton = dgitem.FindControl("Imagebutton6")
            '    l_ImageButton.Enabled = True
            'Next
            Me.ReqGeneralDOB.Enabled = False
            Me.ButtonEditJSBeneficiary.Enabled = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            Me.ButtonUpdateJSBeneficiary.Enabled = False
            Me.ButtonCancelJSBeneficiary.Enabled = False

            Me.TextBoxAnnuitiesSSNo.Enabled = False
            Me.TextBoxAnnuitiesFirstName.Enabled = False
            Me.TextBoxAnnuitiesMiddleName.Enabled = False
            Me.TextBoxAnnuitiesLastName.Enabled = False
            Me.TextBoxAnnuitiesDOB.Enabled = False
            Me.TextBoxDateDeceased.Enabled = False
            Me.ButtonRetireesInfoOK.Enabled = True
            'Me.txtSpouse.Enabled = False
            Me.CheckboxSpouse.Enabled = False 'by aparna 05/09/2007

            Me.PopcalendarDOB.Enabled = False
            Me.PopcalendarDeceased.Enabled = False
            'Added by prasad on 27-Sep-2011 for YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "false"

            AddressWebUserControlAnn.EnableControls = False
            'AA:BT:2306: YRS 5.0 2251  - Added to disable the control
            lnkParticipantAddress.Enabled = False

            'START: PPP | 08/01/2016 | YRS-AT-2382 | On cancelling annuity beneficiary change enable Edit and View SSN Updates link
            Me.ButtonAnnuitiesSSNoEdit.Enabled = True
            Me.lnkbtnViewAnnuitiesSSNUpdate.Enabled = True
            'END: PPP | 08/01/2016 | YRS-AT-2382 | On cancelling annuity beneficiary change enable Edit and View SSN Updates link
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    Private Function ValidateAddress() As String
        Dim l_string_Message As String = String.Empty
        Try
            'Added By Ashutosh Patil as on 23-Feb-2007 For YREN - 3029,YREN - 3028
            'Call SetPrimaryAddressDetails()
            'Call SetSecondaryAddressDetails()

            l_string_Message = AddressWebUserControl1.ValidateAddress()
            If l_string_Message <> "" Then
                Me.MakeLinkVisible()
                Return l_string_Message
            End If

            'FOR Secondary Address
            l_string_Message = AddressWebUserControl2.ValidateAddress()
            If l_string_Message <> "" Then
                Me.MakeLinkVisible()
                Return l_string_Message
            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Added by Swopna on 28 Dec,2007 in response to bug id 321
    '********************
    Private Sub DataGridRetiredBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRetiredBeneficiaries.SelectedIndexChanged
        Try
            Dim l_dgitem As DataGridItem
            Dim l_button_Select As ImageButton
            For Each l_dgitem In Me.DataGridRetiredBeneficiaries.Items
                l_button_Select = CType(l_dgitem.FindControl("Imagebutton7"), ImageButton)
                If l_dgitem.ItemIndex = Me.DataGridRetiredBeneficiaries.SelectedIndex Then
                    l_button_Select.ImageUrl = "images\selected.gif"
                Else
                    l_button_Select.ImageUrl = "images\select.gif"
                End If
            Next

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    '*******************
    'Added by Swopna on 7 Jan,2008 in response to YREN-4126
    '********************
    Private Sub DataGridNotes_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridNotes.SortCommand
        Try

            Dim l_dt_Notes As DataTable
            l_dt_Notes = DirectCast(Session("dtNotes"), DataTable)

            Me.DataGridNotes.SelectedIndex = -1
            If Not l_dt_Notes Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression

                dv = l_dt_Notes.DefaultView
                dv.Sort = SortExpression
                If Not Session("Participant_Sort") Is Nothing Then
                    If Session("Participant_Sort").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Session("Participant_Sort") = dv.Sort
                Me.DataGridNotes.DataSource = Nothing
                Me.DataGridNotes.DataSource = dv
                'Shubhrata YRSPS 4614
                Me.MakeDisplayNotesDataTable()
                'Shubhrata YRSPS 4614
                Me.DataGridNotes.DataBind()

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)


        End Try
    End Sub

    'Shubhrata To have the data grid work properly even after we sort it. YRPS 4614 Feb 12th 2008
    Private Function MakeDisplayNotesDataTable(Optional ByVal parameterDataTable As DataTable = Nothing)
        Dim dv As New DataView
        Dim l_dr_notes As DataRow
        Dim l_dt_new_notes As New DataTable
        Dim j As Integer = 0
        Dim l_datatable_Notes As New DataTable
        Try
            If parameterDataTable Is Nothing Then
                l_datatable_Notes = Session("dtNotes")
            Else
                l_datatable_Notes = parameterDataTable
            End If

            If l_datatable_Notes Is Nothing Then Exit Function
            dv = l_datatable_Notes.DefaultView()
            If Not Session("Participant_Sort") Is Nothing Then
                dv.Sort = Session("Participant_Sort")
            Else
                dv.Sort = "Date DESC"
            End If
            l_dt_new_notes = l_datatable_Notes.Clone()
            For j = 0 To dv.Count - 1
                l_dr_notes = l_dt_new_notes.NewRow()
                l_dr_notes("UniqueID") = dv.Item(j)("UniqueID")
                l_dr_notes("Note") = dv.Item(j)("Note")
                l_dr_notes("PersonID") = dv.Item(j)("PersonID")
                l_dr_notes("Date") = dv.Item(j)("Date")
                l_dr_notes("bitImportant") = dv.Item(j)("bitImportant")
                l_dr_notes("Creator") = dv.Item(j)("Creator")
                l_dt_new_notes.Rows.Add(l_dr_notes)
            Next

            Session("DisplayNotes") = l_dt_new_notes
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Shubhrata To have the data grid work properly even after we sort it. YRPS 4614 Feb 12th 2008
    'Swopna BugId-389 20 May,2008------start
    Public Function AddressValidation() As String
        Dim l_string_Message As String = String.Empty
        Try
            'Added By Ashutosh Patil as on 23-Feb-2007 For YREN - 3029,YREN - 3028
            'Call SetPrimaryAddressDetails()
            'Call SetSecondaryAddressDetails()

            'Priya 10-Dec-2010: YRS 5.0-1177/BT 588 Changes made as sate n country fill up with javascript in user control
            l_string_Message = AddressWebUserControl1.ValidateAddress()
            If l_string_Message <> "" Then
                Me.MakeLinkVisible()
                Return l_string_Message
            End If

            'FOR Secondary Address
            l_string_Message = AddressWebUserControl2.ValidateAddress()
            If l_string_Message <> "" Then
                Me.MakeLinkVisible()
                Return l_string_Message
            End If


        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Function
    'Swopna BugId-389 20 May,2008------end

    'Added by Dilip 20-09-2009 to refresh the Qdro Request information
    Public Sub RefreshQdroInfo()
        Try
            g_dataset_GeneralInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpGeneralInfo(Session("PersId"))

            'Added by Paramesh K. on sept 25th 2008
            '**************************************
            'As we need to display only the Retired QDRO information 

            'Looping through the QDRO data and finding the Retired QDRO request
            'and populating info to the appropriate controls
            For intRow As Integer = 0 To g_dataset_GeneralInfo.Tables("GeneralInfo").Rows.Count - 1
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroType").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroType").ToString() <> "") Then
                    'Checking QDRO Type
                    If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroType").ToString().ToUpper() = "RETIRED" Then
                        'Retired QDRO
                        If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString() <> "") Then
                            'QDRO Pending Checkbox
                            If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString.ToUpper() = "PENDING" Then
                                Me.chkGeneralQDROPending.Checked = True
                            Else
                                Me.chkGeneralQDROPending.Checked = False
                            End If
                            'QDRO Status
                            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString() <> "") Then
                                Me.TextBoxGeneralQDROStatus.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus")
                            End If
                            'Qdro Status Date
                            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatusDate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatusDate").ToString() <> "") Then
                                Me.TextBoxGeneralQDROStatudDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatusDate")
                            End If
                        End If
                        Exit For
                    End If
                End If
                'Priya 23-April-2010:BT -520: In Retiree maintenance page priority handling check box is not selected
                'Priority Handling : Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("priority") = True) Then
                '    Me.checkboxPriority.Checked = True
                'Else
                '    Me.checkboxPriority.Checked = False
                'End If
                'End BT -520
            Next

            'Commented By Paramesh K.
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType").ToString() <> "") Then
            '    Me.TextBoxGeneralQDROStatus.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType")
            'End If
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "") Then
            '    If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString.ToUpper() = "PENDING" Then
            '        Me.chkGeneralQDROPending.Checked = True
            '    Else
            '        Me.chkGeneralQDROPending.Checked = False
            '    End If
            '    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "") Then
            '        Me.TextBoxGeneralQDROStatudDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus")
            '    End If
            '    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate").ToString() <> "") Then
            '        Me.TextBoxGeneralQDROStatudDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate")
            '    End If

            'End If

            '**************************************

        Catch sqlEx As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    '-------End Added by Dilip 20-09-2009 
    'Reverted changes by Anudeep on 2012.11.14 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen Changed from 'Proirity Handling' to 'Exhausted DB settlement efforts' 
    'START : Added By Dilip yadav : 29-Oct-09 : YRS 5.0.921
    'Start: Bala: 01/19/2019: YRS-AT-2398: Control name change
    'Private Sub checkboxPriority_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkboxPriority.CheckedChanged
    Private Sub checkboxExhaustedDBSettle_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkboxExhaustedDBSettle.CheckedChanged
        'End: Bala: 01/19/2019: YRS-AT-2398: Control name change
        'START: MMR | 2016.10.07 | YRS-AT-3062 | Added to check if user has access over control or not and showing message if user has no rights over the control
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("CheckboxExhaustedDBSettleRetiree", Convert.ToInt32(Session("LoggedUserKey")))

        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            If Session("IsExhaustedDBRMDSettleRetiree") = True Then
                checkboxExhaustedDBSettle.Checked = True
            Else
                checkboxExhaustedDBSettle.Checked = False
            End If
            HiddenFieldDirty.Value = "false" ' MMR | 2016.10.14 | YRS-AT-3062 | Setting hidden field value to false to not display confirm message when user clicks on close button.
            Exit Sub
        End If
            'END: MMR | 2016.10.07 | YRS-AT-3062 | Added to check if user has access over control or not and showing message if user has no rights over the control

            'START: MMR | 2016.10.14 | YRS-AT-3063 | Displaying message based on Death date exist or not of participant
        If checkboxExhaustedDBSettle.Checked = True Then
            IsExhaustedDBRMDSettleOptionSelected = True
            If Not String.IsNullOrEmpty(TextBoxGeneralDateDeceased.Text) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_EXHAUSTEDDBRMDSETTLEMENTEFFORTS_DECEASED, MessageBoxButtons.YesNo)
            ElseIf TextBoxGeneralDateDeceased.Text = "" Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_EXHAUSTEDDBRMDSETTLEMENTEFFORTS_ALIVE, MessageBoxButtons.YesNo)
            End If
            'END: MMR | 2016.10.14 | YRS-AT-3063 | Displaying message based on Death date exist or not of participant
        End If
        ButtonSaveRetireeParticipants.Enabled = True
        Me.HyperLinkViewParticipantsInfo.Visible = False
        Me.ButtonRetireesInfoCancel.Enabled = True
    End Sub
    'END : Added By Dilip yadav : 29-Oct-09 : YRS 5.0.921

    Private Sub ButtonGetArchiveDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGetArchiveDataBack.Click
        Try
            Session("CurrentProcesstoConfirm") = "DataArchive"
            'Shashi Shekhar:2010-03-26: change message for BT-488
            MessageBox.Show(PlaceHolder1, "Data Archive", "Are you sure you want to get the archived data back?", MessageBoxButtons.YesNo)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'Shashi Shekhar:2010-01-27: To update the bitIsArchived field in atsPerss table
    'To Update the record in database
    Private Function RetrieveData()
        Dim list As New ArrayList
        Dim errorMsg As String
        Try

            If Not Session("PersId") = Nothing Then
                list.Add(Session("PersId").ToString().Trim())
                'Shashi Shekhar:2010-04-07:Adding one parameter for error message returned from procedure as output parameter
                errorMsg = YMCARET.YmcaBusinessObject.DataArchiveBOClass.RetrieveData(list)
            End If


            ' MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Records have been updated successfully.", MessageBoxButtons.OK)
            tdRetrieveData.Visible = False
            tblRetrieveData.Visible = False
            Session("IsDataArchived") = "No"
            LabelGenHdr.Text = "General"
            LabelAnnuitiesHdr.Text = ""

            ''-----------------------------------------------------------------------------------------------------------------
            ''Shashi Shekhar: 26-Oct-2010: For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
            '	LabelHdr.Text = Me.TextBoxGeneralLastName.Text + ", " + Me.TextBoxGeneralFirstName.Text + ", Fund No: " + g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim
            'Else
            '	LabelHdr.Text = Me.TextBoxGeneralLastName.Text + ", " + Me.TextBoxGeneralFirstName.Text
            'End If
            ''-----------------------------------------------------------------------------------------------------------------

        Catch ex As Exception
            ' Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            'Shashi Shekhar:2010-04-07 :Handling DataArchive error message (concate sql error message with user defined error message returned from procedure)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            list = Nothing
        End Try

    End Function
    'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
    'Priya              18-Feb-2010     YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
    'Private Sub chkShareInfoAllowed_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkShareInfoAllowed.CheckedChanged
    '    ButtonSaveRetireeParticipants.Enabled = True
    '    ButtonRetireesInfoCancel.Enabled = True
    '    Me.HyperLinkViewParticipantsInfo.Visible = False
    'End Sub

    'Priya:19-July-2010    Integrated into enhancement 10-June-2010:YRS 5.0-1107:YMCA Mailing Opt-Out should appear on the Retiree screen too. 
    'uncomment chkYMCAMailingOptOut
    '5-April-2010:Priya:YRS 5.0-988 : fetch value of newly aaded column bitShareInfoAllowed
    'Private Sub chkYMCAMailingOptOut_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkYMCAMailingOptOut.CheckedChanged
    '    ButtonSaveRetireeParticipants.Enabled = True
    '    ButtonRetireesInfoCancel.Enabled = True
    '    Me.HyperLinkViewParticipantsInfo.Visible = False
    'End Sub
    'bhavanS:2011.07.08 :YRS 10.1.1.4 :Change caption of chkPersonalInfoSharingOptOut instead of chkYMCAMailingOptOut
    Private Sub chkPersonalInfoSharingOptOut_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPersonalInfoSharingOptOut.CheckedChanged
        ButtonSaveRetireeParticipants.Enabled = True
        ButtonRetireesInfoCancel.Enabled = True
        Me.HyperLinkViewParticipantsInfo.Visible = False
    End Sub
    'bhavanS:2011.07.11 :YRS 5.0-1354 :Add new bit field chkGoPaperless into atsperss 
    Private Sub chkGoPaperless_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGoPaperless.CheckedChanged
        ButtonSaveRetireeParticipants.Enabled = True
        ButtonRetireesInfoCancel.Enabled = True
        Me.HyperLinkViewParticipantsInfo.Visible = False
    End Sub
    'End 12-May-2010 YRS 5.0-1073
    'End 19-July-2010:Integrated into enhancement 10-June-2010:YRS 5.0-1107:YMCA Mailing Opt-Out should appear on the Retiree screen too. 
    'Added by imran on 30-July-2010 : YRS 5.0-1141 add Age format to the person maintanace screen.
    Private Function CalculateAge(ByVal parameterDOB As String, ByVal parameterDOD As String) As Decimal
        Dim numTotalNumberofDays As Decimal
        Dim numAge As Decimal
        Dim numReminder As Decimal
        Try


            If parameterDOB = String.Empty Then Return 0
            'numTotalNumberofDays = DateDiff(DateInterval.Day, CType(parameterDOB, DateTime), Now.Date)
            numTotalNumberofDays = DateDiff(DateInterval.Day, CType(parameterDOB, DateTime), IIf(parameterDOD = String.Empty, Now.Date, parameterDOD))
            numReminder = (numTotalNumberofDays Mod 365.2524)
            If numReminder > 0 Then
                numAge = Math.Floor((numTotalNumberofDays - numReminder) / 365.2425) + (Math.Floor(numReminder / 30.5) / 100)
            Else
                numAge = Math.Floor((numTotalNumberofDays - numReminder) / 365.2425)
            End If
            CalculateAge = numAge
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#Region "AccountLock/Unlock functions"

    Public Sub GetLockUnlockReasonDetails()

        Dim l_dsLockResDetails As DataSet
        Try

            trLockResEdit.Visible = False
            trLockResDisplay.Visible = True
            If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).IsNull("SSNo") = False AndAlso g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "") Then
                    l_dsLockResDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetLockReasonDetails(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString().Trim)
                End If
            End If


            If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
                If Not l_dsLockResDetails Is Nothing Then
                    lblLockstatus.Text = "Status: "
                    If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows.Count > 0 Then

                        If (l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).IsNull("IsLock") = False AndAlso l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("IsLock").ToString() <> "") Then

                            If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("IsLock") = True Then
                                btnLockUnlock.Text = "Unlock Account"
                                lblLockstatus.Text = lblLockstatus.Text + "<span style='color:Gray'>Locked</span>"
                                lblLockResDetail.Text = "Reason: "
                                lblLockResDetail.Text = lblLockResDetail.Text + "<span style='color:Gray'>" + l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString().Trim + "</span>"
                            Else
                                btnLockUnlock.Text = "Lock Account"
                                lblLockstatus.Text = lblLockstatus.Text + "<span style='color:Gray'>UnLocked</span>"
                                lblLockResDetail.Text = ""

                            End If
                        End If
                    Else
                        btnLockUnlock.Text = "Lock Account"
                        lblLockstatus.Text = lblLockstatus.Text + "<span style='color:Gray'>UnLocked</span>"
                        lblLockResDetail.Text = ""
                    End If

                End If
            End If

        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try


    End Sub

    Private Sub btnAcctLockEditRet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAcctLockEditRet.Click
        Try
            trLockResDisplay.Visible = False
            trLockResEdit.Visible = True

            If lblLockstatus.Text.Trim.ToLower = "status: <span style='color:Gray'>locked</span>" Then
                ddlReasonCode.Visible = False
                lblResCode.Text = ""
            Else
                ddlReasonCode.Visible = True
                PopulateLockReasonList()
            End If
            'btnAcctLockEditRet.Enabled = False
        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
    Private Sub btnLockUnlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLockUnlock.Click
        Try
            If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).IsNull("IsLocked") = False AndAlso g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsLocked").ToString() <> "") Then
                    If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsLocked") = False Then
                        'SS:18-feb-2011: for BT-755 Mandatory field validation message needs to display first.
                        If ddlReasonCode.SelectedItem.Value.Trim = "select" Then
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select reason code from list.", MessageBoxButtons.OK)
                            Exit Sub
                        End If
                    End If
                End If
            End If



            Session("CurrentProcesstoConfirm") = "AccountLock"
            btnAcctLockEditRet.Enabled = True
            'Shashi Shekhar:2010-03-26: change message for BT-488
            MessageBox.Show(PlaceHolder1, "Account Lock/Unlock", "Are you sure you want to Lock/Unlock this account?", MessageBoxButtons.YesNo)

        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    'Populating reason drip down on the basis of current lock status (If Lock then reason will be for Unlock and vice-versa)
    Public Sub PopulateLockReasonList()
        Try

            Dim l_dsReadReasonCode As DataSet
            l_dsReadReasonCode = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetReasonCodes()
            ddlReasonCode.DataSource = l_dsReadReasonCode.Tables("GetReasonCode")
            ddlReasonCode.DataTextField = "chvShortDesc"
            ddlReasonCode.DataValueField = "chvReasonCode"
            ddlReasonCode.DataBind()
            ddlReasonCode.Items.Insert(0, New ListItem("-Select-", "select"))
        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    'Status update in atsPerss and making entry for atsParticipantLock table
    Public Sub UpdateAccountLockUnlock()

        Dim l_boolAcctStatus As Boolean
        Try
            If Not Session("PersId") = Nothing Then
                If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
                    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).IsNull("IsLocked") = False AndAlso g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsLocked").ToString() <> "") Then
                        If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsLocked") = True Then
                            l_boolAcctStatus = False
                            YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateAccountLockStatus(Session("PersId").ToString.Trim, l_boolAcctStatus, "UNLOCK")
                        Else
                            l_boolAcctStatus = True
                            YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateAccountLockStatus(Session("PersId").ToString.Trim, l_boolAcctStatus, ddlReasonCode.SelectedItem.Value.Trim)
                        End If
                    End If
                    GetLockUnlockReasonDetails() 'Call this metod to referesh the data with latest one.
                End If
            End If



        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub




#End Region

    Private Sub LoadPoADetails()
        'BT 706 :Add by BhavnaS : POA name does not show up on the Bene Info page,display all active poa 

        'reopened Issue on 2011.07.22
        Dim builder As New StringBuilder
        l_DataSet_POA = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpPOAInfo(Session("PersId").ToString())
        If HelperFunctions.isNonEmpty(l_DataSet_POA) Then
            'Dinesh K:2012.12.12:BT-1236/YRS 5.0-1685:Add Category/Type field to Power of attorney and allow 3 types
            'Anudeep:07.11.2013-BT:2269-YRS 5.0-2234:Added to show the Button text without POA
            ButtonGeneralPOA.Text = "Show/Edit all (" + l_DataSet_POA.Tables("POAInfo").Rows.Count.ToString() + ")"

            'For Each dr As DataRow In l_DataSet_POA.Tables("POAInfo").Rows
            '    If dr.IsNull("Name1") = True OrElse dr.Item("Name1") = "" Then Continue For
            '    If builder.Length > 0 Then builder.Append(", ")
            '    builder.Append(dr("Name1").ToString())
            'Next
        End If

        'TextBoxGeneralPOA.Text = builder.ToString()
        'End BT 706 reopen Issue
    End Sub
    'BS:2012.01.18:YRS 5.0-1497 -Add edit button to modify the date of death
    Private Sub btnEditDeathDateRet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditDeathDateRet.Click
        Try
            If TextBoxGeneralDateDeceased.Text <> String.Empty Then
                HiddenFieldDeathDate.Value = TextBoxGeneralDateDeceased.Text
            End If
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnEditDeathDateRet", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            Session("EnableSaveCancel") = True
            Me.ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            TextBoxGeneralDateDeceased.Enabled = True
            Me.MakeLinkVisible()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'Start: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite
    'prasad 2012.01.24 For BT-950,YRS 5.0-1469: Add link to Web Front End
    '<System.Web.Services.WebMethod()> _
    'Public Shared Function DisplayLoginCredentialInformation() As Array
    '    Dim l_dataset_loginCredential As DataSet
    '    Dim dr As DataRow
    '    Dim returnInformation(0 To 4) As String
    '    Dim l_logincredential As DataTable
    '    Try
    '        'prasad 2012.01.31	For BT-950,YRS 5.0-1469: Add link to Web Front End
    '        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonWebFrontEnd", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
    '        If Not checkSecurity.Equals("True") Then
    '            returnInformation(0) = "2" '"You are not authorized to view data"
    '            Return returnInformation
    '        End If

    '        If Not HttpContext.Current.Session("PersId") Is Nothing Then
    '            l_dataset_loginCredential = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpGeneralInfo(HttpContext.Current.Session("PersId"))
    '            If l_dataset_loginCredential Is Nothing Then
    '                returnInformation(0) = "1" '"Data not available"
    '                Return returnInformation
    '            End If

    '            l_logincredential = l_dataset_loginCredential.Tables("LoginCredential")
    '            If HelperFunctions.isEmpty(l_logincredential) Then
    '                returnInformation(0) = "1" '"Data not available"
    '                Return returnInformation
    '            End If

    '            dr = l_logincredential.Rows(0)
    '            ' BT:1031 New change for Gemini ID YRS 5.0-1561 
    '            'Added orelse instead of or it will give error if dr value is null

    '            If dr.IsNull("username") = False OrElse dr.Item("username") <> "" Then
    '                returnInformation(1) = dr.Item("username").ToString
    '            End If
    '            If dr.IsNull("password") = False OrElse dr.Item("password") <> "" Then
    '                returnInformation(2) = dr.Item("password").ToString
    '            End If
    '            If dr.IsNull("question") = False OrElse dr.Item("question") <> "" Then
    '                returnInformation(3) = dr.Item("question").ToString
    '            End If
    '            If dr.IsNull("answer") = False OrElse dr.Item("answer") <> "" Then
    '                returnInformation(4) = dr.Item("answer")
    '            End If
    '            returnInformation(0) = "0" 'successful data
    '        End If
    '        Return returnInformation
    '        'prasad 2012.01.31	For BT-950,YRS 5.0-1469: Add link to Web Front End
    '    Catch ex As Exception
    '        HelperFunctions.LogException("WebFrontEnd", ex)
    '        ' BT:1031 New change for Gemini ID YRS 5.0-1561 
    '        'if error occured then make returnInformation(1),(2),(3) = nothing
    '        returnInformation(1) = Nothing
    '        returnInformation(2) = Nothing
    '        returnInformation(3) = Nothing
    '        returnInformation(4) = Nothing
    '        returnInformation(0) = "3" '"Error occured. Please contact IT support."
    '        Return returnInformation
    '    End Try
    'End Function
    <System.Web.Services.WebMethod()> _
    Public Shared Function PrintForm() As String
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String
        Dim Participant As New RetireesInformationWebForm
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonWebFrontEndPrintRetire", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                Return "1"
            End If
            HttpContext.Current.Session("strReportName") = "WebIdLetterOleN"
            'Start : Anudeep:28.11.2012 - added for Bt-1026/YRS 5.0-1629 : Web frontend letters to IDM.


            l_StringReportName = "WebIdLetterOleN"
            l_stringDocType = "WEBREGLT"
            l_ArrListParamValues.Add(CType(HttpContext.Current.Session("Person_Info"), String).ToString.Trim)
            l_string_OutputFileType = "Participant_" & l_stringDocType
            l_StringErrorMessage = Participant.CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)

            HttpContext.Current.Session("strReportName") = l_StringReportName
            'Error occurs in copying in idm then returns the message
            If Not l_StringErrorMessage Is Nothing Then
                Return l_StringErrorMessage
            End If
        Catch ex As Exception
            HelperFunctions.LogException("WebFrontEnd", ex)
        End Try
    End Function
    'Added by prasad 2012.03.13 For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
    'Public Function updateInfo()
    '	Dim oldfundStatus, Termdate, YmcaId, FundEventId, newFundStatusCode, newFundStatusDesc As String
    '	Dim l_dataset_AddAccount, l_dataset_Employment As DataSet
    '	Dim findrowaccount() As DataRow
    '	Dim returnval As Boolean
    '	Dim tdcontractexist As Boolean = False
    '	Dim dataviewemp As DataView
    '	Dim terminationdate As Date
    '	l_dataset_Employment = CType(Session("l_dataset_Employment"), DataSet)
    '	l_dataset_AddAccount = CType(Session("l_dataset_AddAccount"), DataSet)
    '	dataviewemp = l_dataset_Employment.Tables(0).DefaultView
    '	dataviewemp.Sort = "Termdate DESC"
    '	If Not IsDBNull(dataviewemp(0)("Termdate")) And Not IsDBNull(dataviewemp(0)("YmcaId")) And Not IsDBNull(dataviewemp(0)("FundEventId")) Then
    '		Termdate = dataviewemp(0)("Termdate").ToString
    '		YmcaId = dataviewemp(0)("YmcaId").ToString
    '		FundEventId = dataviewemp(0)("FundEventId").ToString
    '	End If
    '	If l_dataset_AddAccount.Tables(0).Rows.Count <> 0 Then
    '		If Not Termdate Is Nothing And Not YmcaId Is Nothing Then
    '			findrowaccount = l_dataset_AddAccount.Tables(0).Select("AccountType='TD' AND TerminationDate='" + Termdate + "' AND YmcaId='" + YmcaId + "'")
    '			If findrowaccount.Length <> 0 Then
    '				tdcontractexist = True
    '			End If
    '		End If
    '	End If
    '	If Not CType(Session("FundStatusType"), String) Is Nothing Then
    '		oldfundStatus = Session("FundStatusType").ToString
    '		returnval = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.updateFundEventEmpEvent(Termdate, FundEventId, YmcaId, tdcontractexist, 1, newFundStatusCode, newFundStatusDesc)
    '	End If

    '	If returnval Then
    '		Session("FundStatusType") = newFundStatusCode
    '		Session("FundStatus") = newFundStatusDesc
    '		LoadEmploymentTab()
    '		If tdcontractexist Then
    '			LoadAdditionalAccountsTab()
    '		End If
    '		MessageBox.Show(PlaceHolder1, "YMCA-YRS", String.Format(Resources.ParticipantsInformation.REACTIVE_SUCCESS, oldfundStatus, newFundStatusCode), MessageBoxButtons.OK)
    '		ButtonReactivate.Visible = False
    '	Else
    '		Throw New Exception("Error While reactivation process.")
    '	End If
    'End Function

    'BS:2012.06.15:BT:991:YRS 5.0-1530:

    <System.Web.Services.WebMethod()> _
    Public Shared Function GetWebAccountDetails()
        Try
            Dim strUserId As String, checkSecurity As String
            strUserId = HttpContext.Current.Session("PersId")
            If System.Configuration.ConfigurationSettings.AppSettings("WebAcctInfo_WS_Path") Is Nothing Then
                Return "WebAcctInfo_WS_Path key does not exists in configuration file"
            End If
            If System.Configuration.ConfigurationSettings.AppSettings("WebAcctInfo_WS_Path").ToString.Trim() = "" Then
                Return "WebAcctInfo_WS_Path key value is empty in configuration file"
            End If
            If Not String.IsNullOrEmpty(strUserId) Then
                checkSecurity = SecurityCheck.Check_Authorization("RetireesInformationWebForm.aspx", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    Return "error|" + checkSecurity
                End If
                checkSecurity = SecurityCheck.Check_Authorization("ButtonWebFrontEndRetiree", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    Return "error|" + checkSecurity
                End If
                Dim proxy As New AdminWebAccountInfo
                Dim strPasAdminDB As String = proxy.GetUserDetails(strUserId)
                Dim serializer As New XmlSerializer(GetType(adminConsoleValDb), New XmlRootAttribute("adminConsoleVal"))
                Dim objWebAcctDetails As adminConsoleValDb = TryCast(serializer.Deserialize(New StringReader(strPasAdminDB)), adminConsoleValDb)
                Return objWebAcctDetails
            Else
                Return "error|Session might have expired please re-login."
            End If
        Catch ex As Exception
            HelperFunctions.LogException("participantinformation_GetWebAccountDetails", ex)
            Return ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function GetAdminActivity()
        Dim objStrHtml As New StringBuilder()
        Dim strHtml As String
        Dim strUserId As String
        Dim gvActivity As New GridView
        Dim strRetValue As String = String.Empty
        Dim objStrWriter As New StringWriter(objStrHtml)
        Dim objHtmlTextWriter As New HtmlTextWriter(objStrWriter)
        Dim strErrorMessage As String
        Try
            strUserId = HttpContext.Current.Session("PersId")
            If Not String.IsNullOrEmpty(strUserId) Then
                Dim proxy As New AdminWebAccountInfo()
                Dim dsActivity As DataSet = proxy.GetAdminActivityLog(strUserId, strErrorMessage)
                If strErrorMessage IsNot String.Empty Then
                    If dsActivity IsNot Nothing Then
                        gvActivity.DataSource = dsActivity
                        gvActivity.CssClass = "DataGrid_Grid"
                        gvActivity.RowStyle.CssClass = "DataGrid_NormalStyle"
                        gvActivity.SelectedRowStyle.CssClass = "DataGrid_SelectedStyle"
                        gvActivity.HeaderStyle.CssClass = "DataGrid_HeaderStyle"
                        gvActivity.DataBind()
                        gvActivity.RenderControl(objHtmlTextWriter)
                        strHtml = WebUtility.HtmlEncode(objStrHtml.ToString)
                        strRetValue = WebUtility.HtmlDecode(strHtml)
                    End If
                    Return strRetValue
                Else
                    Return strErrorMessage
                End If
            Else
                Return "error|Session might have expired please re-login."
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ParicipantInformation_getAdminAcctivity", ex)
            Return ex.Message
        End Try
    End Function
    <System.Web.Services.WebMethod()> _
    Public Shared Function UnlockWebAccount()
        Try
            Dim strUserId As String, Name As String, checkSecurity As String
            strUserId = HttpContext.Current.Session("PersId")
            Name = HttpContext.Current.Session("LoginId")
            If Not String.IsNullOrEmpty(Name.Trim) Then
                checkSecurity = SecurityCheck.Check_Authorization("btnRetWebLockUnLock", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    Return "error|" + checkSecurity
                End If
                Dim proxy As New AdminWebAccountInfo()
                Return proxy.UnLockAccount(strUserId, Name)
            Else
                Return "error|Session might have expired please re-login."
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ParicipantInformation_UnlockWebAccount", ex)
            Return "error|" + ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function LockWebAccount()
        Try
            Dim strUserId As String, Name As String, checkSecurity As String
            strUserId = HttpContext.Current.Session("PersId")
            Name = HttpContext.Current.Session("LoginId")
            If Not String.IsNullOrEmpty(Name.Trim) Then
                checkSecurity = SecurityCheck.Check_Authorization("btnRetWebLockUnLock", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    Return "error|" + checkSecurity
                End If
                Dim proxy As New AdminWebAccountInfo()
                Return proxy.LockAccount(strUserId, Name)
            Else
                Return "error|Session might have expired please re-login."
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ParicipantInformation_LockWebAccount", ex)
            Return "error|" + ex.Message
        End Try
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function SendMailTempPass()
        Try
            Dim strUserId As String, Name As String, checkSecurity As String
            strUserId = HttpContext.Current.Session("PersId")
            Name = HttpContext.Current.Session("LoginId")
            If Not String.IsNullOrEmpty(Name.Trim) Then
                checkSecurity = SecurityCheck.Check_Authorization("btnRetWebSendTempPass", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    Return "error|" + checkSecurity
                End If
                Dim proxy As New AdminWebAccountInfo()
                Return proxy.SendTempPasswordEmail(strUserId, Name)
            Else
                Return "error|Session might have expired please re-login"
            End If
        Catch ex As Exception
            HelperFunctions.LogException("ParicipantInformation_SendMailTempPass", ex)
            Return "error|" + ex.Message
        End Try
    End Function

    Public Class adminConsoleValDb

        Public Property accountLocked() As Boolean
            Get
                Return m_accountLocked
            End Get
            Set(value As Boolean)
                m_accountLocked = value
            End Set
        End Property
        Private m_accountLocked As Boolean
        Public Property accountLockedMsg() As String
            Get
                Return m_accountLockedMsg
            End Get
            Set(value As String)
                m_accountLockedMsg = value
            End Set
        End Property
        Private m_accountLockedMsg As String
        Public Property email2() As String
            Get
                Return m_email2
            End Get
            Set(value As String)
                m_email2 = value
            End Set
        End Property
        Private m_email2 As String
        Public Property errMessage() As String
            Get
                Return m_errMessage
            End Get
            Set(value As String)
                m_errMessage = value
            End Set
        End Property
        Private m_errMessage As String
        Public Property lastBadLoginMsg() As String
            Get
                Return m_lastBadLoginMsg
            End Get
            Set(value As String)
                m_lastBadLoginMsg = value
            End Set
        End Property
        Private m_lastBadLoginMsg As String
        Public Property lastGoodLoginMsg() As String
            Get
                Return m_lastGoodLoginMsg
            End Get
            Set(value As String)
                m_lastGoodLoginMsg = value
            End Set
        End Property
        Private m_lastGoodLoginMsg As String
        Public Property message() As String
            Get
                Return m_message
            End Get
            Set(value As String)
                m_message = value
            End Set
        End Property
        Private m_message As String
        Public Property questionAnswer() As String
            Get
                Return m_questionAnswer
            End Get
            Set(value As String)
                m_questionAnswer = value
            End Set
        End Property
        Private m_questionAnswer As String
        Public Property questionID() As System.Nullable(Of Integer)
            Get
                Return m_questionID
            End Get
            Set(value As System.Nullable(Of Integer))
                m_questionID = value
            End Set
        End Property
        Private m_questionID As System.Nullable(Of Integer)
        Public Property strQuestion() As String
            Get
                Return m_strQuestion
            End Get
            Set(value As String)
                m_strQuestion = value
            End Set
        End Property
        Private m_strQuestion As String
        Public Property UserId() As String
            Get
                Return m_UserId
            End Get
            Set(value As String)
                m_UserId = value
            End Set
        End Property
        Private m_UserId As String
        Public Property UserName() As String
            Get
                Return m_UserName
            End Get
            Set(value As String)
                m_UserName = value
            End Set
        End Property
        Private m_UserName As String
    End Class
    'End: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite

    Public Sub RefreshEmailDetail(ByVal dsAddress As DataSet)
        If dsAddress IsNot Nothing AndAlso HelperFunctions.isNonEmpty(dsAddress.Tables("EmailInfo")) Then
            'Email Information
            If Not dsAddress.Tables("EmailInfo") Is Nothing Then
                If (dsAddress.Tables("EmailInfo").Rows(0).Item("EmailAddress").ToString() <> "System.DBNull" And dsAddress.Tables("EmailInfo").Rows(0).Item("EmailAddress").ToString() <> "") Then
                    Me.TextBoxEmailId.Text = dsAddress.Tables("EmailInfo").Rows(0).Item("EmailAddress").ToString().Trim
                    'Me.EmailId.Value = Me.TextBoxEmailId.Text
                End If
                If (dsAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress").ToString() <> "System.DBNull" And dsAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress").ToString() <> "") Then
                    If Convert.ToBoolean(dsAddress.Tables("EmailInfo").Rows(0).Item("IsBadAddress")) = True Then
                        CheckboxBadEmail.Checked = True
                        'Me.BadEmail.Value = True
                    Else
                        CheckboxBadEmail.Checked = False
                        'Me.BadEmail.Value = False
                    End If
                End If
                If (dsAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed").ToString() <> "System.DBNull" And dsAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed").ToString() <> "") Then
                    If Convert.ToBoolean(dsAddress.Tables("EmailInfo").Rows(0).Item("IsUnsubscribed")) = True Then
                        CheckboxUnsubscribe.Checked = True
                        'Me.Unsubscribe.Value = True
                    Else
                        CheckboxUnsubscribe.Checked = False
                        'Me.Unsubscribe.Value = False
                    End If
                End If
                If (dsAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly").ToString() <> "System.DBNull" And dsAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly").ToString() <> "") Then
                    If Convert.ToBoolean(dsAddress.Tables("EmailInfo").Rows(0).Item("IsTextOnly")) = True Then
                        CheckboxTextOnly.Checked = True
                        Me.TextOnly.Value = True
                    Else
                        CheckboxTextOnly.Checked = False
                        Me.TextOnly.Value = False
                    End If
                End If
            End If
        End If
    End Sub
    'Start :'Commented by Anudeep:14.03.2013 for telephone usercontrol
    Public Sub RefreshPrimaryTelephoneDetail(ByVal dsContacts As DataTable)
        'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from Retiree and put on address control
        'If dsAddress IsNot Nothing AndAlso HelperFunctions.isNonEmpty(dsAddress.Tables("AddressInfo")) Then

        '	If (dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "System.DBNull" And dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "") Then
        '		If Convert.ToBoolean(dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) = True Then
        '			CheckboxIsBadAddress.Checked = True
        '		Else
        '			CheckboxIsBadAddress.Checked = False
        '		End If
        '	End If
        'End If
        'Telephone
        If dsContacts IsNot Nothing AndAlso HelperFunctions.isNonEmpty(dsContacts) Then
            Dim l_datarow As DataRow
            For Each l_datarow In dsContacts.Rows
                If (l_datarow.IsNull("PhoneType") = False AndAlso l_datarow("PhoneType").ToString() <> "") Then
                    Select Case l_datarow("PhoneType").ToString.ToUpper.Trim()
                        Case "HOME"
                            TextBoxHome.Text = l_datarow("PhoneNumber").ToString().Trim
                        Case "OFFICE"
                            TextboxTelephone.Text = l_datarow("PhoneNumber").ToString().Trim
                        Case "FAX"
                            TextBoxFax.Text = l_datarow("PhoneNumber").ToString().Trim
                        Case "MOBILE"
                            TextBoxMobile.Text = l_datarow("PhoneNumber").ToString().Trim
                    End Select
                End If
            Next
        End If
    End Sub
    Public Sub RefreshSecTelephoneDetail(ByVal dtContacts As DataTable)
        'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from Retiree and put on address control
        'If Not dsAddress Is Nothing Then
        '	If HelperFunctions.isNonEmpty(dsAddress.Tables("AddressInfo")) Then
        '		If (dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "System.DBNull" And dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "") Then
        '			If Convert.ToBoolean(dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) = True Then
        '				Me.CheckboxSecIsBadAddress.Checked = True
        '			Else
        '				Me.CheckboxSecIsBadAddress.Checked = False
        '			End If
        '		End If
        '	End If
        '	'Address_DropDownListState()
        'End If
        If HelperFunctions.isNonEmpty(dtContacts) Then
            Dim l_secdatarow As DataRow
            For Each l_secdatarow In dtContacts.Rows
                If (l_secdatarow.IsNull("PhoneType") = False AndAlso l_secdatarow("PhoneType").ToString() <> "") Then
                    Select Case l_secdatarow("PhoneType").ToString.ToUpper.Trim()
                        Case "HOME"
                            TextBoxSecHome.Text = l_secdatarow("PhoneNumber").ToString().Trim
                        Case "OFFICE"
                            TextBoxSecTelephone.Text = l_secdatarow("PhoneNumber").ToString().Trim
                        Case "FAX"
                            TextBoxSecFax.Text = l_secdatarow("PhoneNumber").ToString().Trim
                        Case "MOBILE"
                            TextBoxSecMobile.Text = l_secdatarow("PhoneNumber").ToString().Trim
                    End Select
                End If
            Next
        End If
    End Sub
    'End :Commented by Anudeep:14.03.2013 for telephone usercontrol
    'Public Sub RefreshAddressDetail(ByVal dsAddress As DataSet, ByVal AddressUserCtrl As AddressWebUserControl)
    '	If dsAddress IsNot Nothing Then
    '		'Address
    '		If HelperFunctions.isNonEmpty(dsAddress.Tables("AddressInfo")) Then
    '			With dsAddress.Tables("AddressInfo").Rows(0)
    '				AddressUserCtrl.Address1 = .Item("Address1").ToString().Trim
    '				AddressUserCtrl.Address2 = .Item("Address2").ToString().Trim
    '				AddressUserCtrl.Address3 = .Item("Address3").ToString().Trim
    '				AddressUserCtrl.City = .Item("City").ToString().Trim
    '				AddressUserCtrl.DropDownListStateValue = .Item("State").ToString().Trim
    '				AddressUserCtrl.DropDownListCountryValue = .Item("Country").ToString().Trim
    '				AddressUserCtrl.ZipCode = .Item("Zip").ToString().Trim
    '				AddressUserCtrl.EffDate = .Item("EffDate").ToString().Trim
    '			End With
    '		End If
    '	End If
    'End Sub

    Private Sub InitializeAttributesOfServerControls()
        'Shashi Shekhar:2009-12-23:Commented old call and added new call to eliminate the conflict of checksecurity access between server side and client side.
        '---------------------------------------------------------------------------------------------------------------------------------------------
        'Me.ButtonGeneralDOBEdit.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonGeneralDOBEdit');")
        Me.ButtonGeneralDOBEdit.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonGeneralDOBEdit.ID))

        'Me.ButtonGeneralSSNoEdit.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonGeneralSSNoEdit');")
        Me.ButtonGeneralSSNoEdit.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonGeneralSSNoEdit.ID))

        'Me.ButtonGeneralEdit.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonGeneralEdit');")
        Me.ButtonGeneralEdit.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonGeneralEdit.ID))




        'Me.ButtonGeneralWithholdAdd.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonGeneralWithholdAdd');")
        Me.ButtonGeneralWithholdAdd.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonGeneralWithholdAdd.ID))

        'Me.ButtonNotesAdd.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonNotesAdd');")
        'Start:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
        'Me.ButtonNotesAdd.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonNotesAdd.ID))

        'Me.ButtonGeneralWithholdUpdate.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonGeneralWithholdUpdate');")
        Me.ButtonGeneralWithholdUpdate.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonGeneralWithholdUpdate.ID))

        'Me.ButtonBankingInfoAdd.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonBankingInfoAdd');")
        'Me.ButtonBankingInfoAdd.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonBankingInfoAdd.ID))

        'Me.ButtonFederalWithholdAdd.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonFederalWithholdAdd');")
        'Me.ButtonFederalWithholdAdd.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonFederalWithholdAdd.ID))

        'Me.ButtonGeneralWithholdUpdate.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonGeneralWithholdUpdate');")
        Me.ButtonGeneralWithholdUpdate.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonGeneralWithholdUpdate.ID))

        'Me.ButtonBankingInfoUpdate.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonBankingInfoUpdate');")
        Me.ButtonBankingInfoUpdate.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonBankingInfoUpdate.ID))

        'Me.ButtonFederalWithholdUpdate.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonFederalWithholdUpdate');")
        Me.ButtonFederalWithholdUpdate.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonFederalWithholdUpdate.ID))

        'Me.ButtonDeathNotification.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonDeathNotification');")
        Me.ButtonDeathNotification.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonDeathNotification.ID))

        'Me.ButtonAddRetired.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonAddRetired');")
        'Me.ButtonAddRetired.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonAddRetired.ID))
        'End:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
        'Me.ButtonEditRetired.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonEditRetired');")
        Me.ButtonEditRetired.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonEditRetired.ID))

        'Priya 29-March-2010:BT-476:Allowing to delete Beneficiary even the User don't have rights to delete. 
        Me.ButtonDeleteRetired.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonDeleteRetired.ID))
        'End 29-March-2010 :BT-476

        'This Function open new window on valid autherization
        Me.ButtonGeneralQDROPendingEdit.Attributes.Add("onclick", "javascript:return CheckAccess1('ButtonGeneralQDROPendingEdit');")

        'Shashi Shekhar:2010-01-27
        'Me.ButtonGetArchiveDataBack.Attributes.Add("onClick", "javascript:return CheckAccess('ButtonGetArchiveDataBack');") 'Shashi Shekhar:2009.09.09- This check was added so that other validations are not called from this button. Alternatively, the button should have causesValidation property set to False.
        Me.ButtonGetArchiveDataBack.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonGetArchiveDataBack.ID))
        '---------------------------------------------------------------------------------------------------------------------------------------------------

        'Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control


        ' Me.ButtonAddBeneficiaries.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonAddBeneficiaries');")


        'Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control
        'Response.Write(Session("VignettePath"))

        'Response.Write(Session("FundNo"))

        'Shubhrata YREN 2779 Oct 30th 2006
        'Commented By Ashutosh Patil as on 25-Jan-2007 YREN-3028,YREN-3029
        'Start Ashutosh Patil
        ''If Me.DropdownlistCountry.SelectedValue = "US" Then
        ''    Me.TextBoxZip.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
        ''Else
        ''    Me.TextBoxZip.Attributes.Add("OnKeyPress", "javascript:ValidateAlphaNumeric();")
        ''End If
        ''If Me.DropdownlistSecCountry.SelectedValue = "US" Then
        ''    Me.TextBoxSecZip.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
        ''Else
        ''    Me.TextBoxSecZip.Attributes.Add("OnKeyPress", "javascript:ValidateAlphaNumeric();")
        ''End If
        '' End Ashutosh Patil
        'Shubhrata YREN 2779 Oct 30th 2006
        'Added By Ashutosh Patil as on 25-Jan-2007 YREN-3028,YREN-3029
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3029, YREN - 3028	
        'Call SetAttributes(Me.TextBoxZip, Me.DropdownlistCountry.SelectedValue.ToString)
        'Call SetAttributes(Me.TextBoxSecZip, Me.DropdownlistSecCountry.SelectedValue.ToString)
        Me.TextBoxEmailId.Attributes.Add("onblur", "javascript:ValidateForm();")
        'Start:Commented by Anudeep:14.03.2013 for telephone usercontrol
        Me.TextboxTelephone.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxHome.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxFax.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxMobile.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")

        Me.TextBoxSecTelephone.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxSecHome.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxSecFax.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxSecMobile.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")

        'Me.TextboxTelephone.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxTelephone);")
        'Me.TextBoxHome.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxHome);")
        'Me.TextBoxFax.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxFax);")
        'Me.TextBoxMobile.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxMobile);")

        'ENd :Commented by Anudeep:14.03.2013 for telephone usercontrol
        'Ashutosh Patil as on 26-Mar-2007
        'YREN - 3028,YREN-3029
        'Me.TextBoxCity.Attributes.Add("OnKeyPress", "javascript:ValidateAlphaNumericForCity();")
        'Me.TextBoxSecCity.Attributes.Add("OnKeyPress", "javascript:ValidateAlphaNumericForCity();")

        'Start :Commented by Anudeep:14.03.2013 for telephone usercontrol
        'Me.TextBoxSecTelephone.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxSecTelephone);")
        'Me.TextBoxSecHome.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxSecHome);")
        'Me.TextBoxSecFax.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxSecFax);")
        'Me.TextBoxSecMobile.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxSecMobile);")
        'Start:Anudeep:08.04.2013 Bt1555:YRS 5.0-1769:Length of phone numbers
        'Dim l_dataset_SecAddressInfo, l_dataset_AddressInfo As DataSet
        'l_dataset_SecAddressInfo = Session("Ds_SecondaryAddress")
        'l_dataset_AddressInfo = Session("Ds_PrimaryAddress")
        'If HelperFunctions.isNonEmpty(l_dataset_AddressInfo) Then
        '    Dim Country As String = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0)("Country").ToString()
        '    If Not (Country = "US" Or Country = "CA") Then
        '        TextboxTelephone.MaxLength = 14
        '        TextBoxHome.MaxLength = 14
        '        TextBoxFax.MaxLength = 14
        '        TextBoxMobile.MaxLength = 14
        '        Me.TextboxTelephone.Attributes.Remove("onblur")
        '        Me.TextBoxHome.Attributes.Remove("onblur")
        '        Me.TextBoxFax.Attributes.Remove("onblur")
        '        Me.TextBoxMobile.Attributes.Remove("onblur")
        '    Else
        '        TextboxTelephone.MaxLength = 10
        '        TextBoxHome.MaxLength = 10
        '        TextBoxFax.MaxLength = 10
        '        TextBoxMobile.MaxLength = 10
        'Me.TextboxTelephone.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxTelephone,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        'Me.TextBoxHome.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxHome,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        'Me.TextBoxFax.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxFax,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        'Me.TextBoxMobile.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxMobile,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")

        Me.TextboxTelephone.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxTelephone,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        Me.TextBoxHome.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxHome,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        Me.TextBoxFax.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxFax,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        Me.TextBoxMobile.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxMobile,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        '    End If
        'End If
        'If HelperFunctions.isNonEmpty(l_dataset_SecAddressInfo) Then
        '    Dim Country As String = l_dataset_SecAddressInfo.Tables("AddressInfo").Rows(0)("Country").ToString()
        '    If Not (Country = "US" Or Country = "CA") Then
        '        TextBoxSecTelephone.MaxLength = 15
        '        TextBoxSecHome.MaxLength = 15
        '        TextBoxSecFax.MaxLength = 15
        '        TextBoxSecMobile.MaxLength = 15
        '        Me.TextBoxSecTelephone.Attributes.Remove("onblur")
        '        Me.TextBoxSecHome.Attributes.Remove("onblur")
        '        Me.TextBoxSecFax.Attributes.Remove("onblur")
        '        Me.TextBoxSecMobile.Attributes.Remove("onblur")
        '    Else
        '        TextBoxSecTelephone.MaxLength = 10
        '        TextBoxSecHome.MaxLength = 10
        '        TextBoxSecFax.MaxLength = 10
        '        TextBoxSecMobile.MaxLength = 10
        'Me.TextBoxSecTelephone.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxSecTelephone,document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value);")
        'Me.TextBoxSecHome.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxSecHome,document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value);")
        'Me.TextBoxSecFax.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxSecFax,document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value);")
        'Me.TextBoxSecMobile.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxSecMobile,document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value);")

        Me.TextBoxSecTelephone.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxSecTelephone,document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value);")
        Me.TextBoxSecHome.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxSecHome,document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value);")
        Me.TextBoxSecFax.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxSecFax,document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value);")
        Me.TextBoxSecMobile.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxSecMobile,document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value);")
        '    End If
        'End If
        'End:Anudeep:08.04.2013 Bt1555:YRS 5.0-1769:Length of phone numbers
        'End :Commented  ANudeep for telephone popup usercontrol

        'Shashi Shekhar:09- feb 2011: For YRS 5.0-1236 : Need ability to freeze/lock account
        Me.btnAcctLockEditRet.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, btnAcctLockEditRet.ID))

        'BS:2012.01.16:YRS 5.0-1497 -Add edit button to modify the date of death
        Me.btnEditDeathDateRet.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, btnEditDeathDateRet.ID))
    End Sub

    Private Sub InitializeMartialStatusDropDownList()
        'added by hafiz on 2-May-2007 for YREN-3112
        Dim l_dataset_MaritalTypes As DataSet
        l_dataset_MaritalTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.MaritalTypes(0)
        Me.cboGeneralMaritalStatus.DataSource = l_dataset_MaritalTypes.Tables(0)
        Me.cboGeneralMaritalStatus.DataTextField = "Description"
        Me.cboGeneralMaritalStatus.DataValueField = "Code"
        Me.cboGeneralMaritalStatus.DataBind()
    End Sub

    Private Sub InitializeGenderTypesDropDownList()
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
    End Sub
    Public Sub EnableEmail()
        If CheckboxBadEmail.Checked Then
            Me.TextBoxEmailId.Enabled = False
            Me.LabelEmailId.Enabled = False
        Else
            Me.TextBoxEmailId.Enabled = True
            Me.LabelEmailId.Enabled = True
        End If
    End Sub

    Private Sub CheckboxBadEmail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxBadEmail.CheckedChanged
        EnableEmail()
    End Sub
    'Start: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form
    '#Region "Death Notification"
    '    'Saves the js annuities information into deathbenefitapplicationform table
    '    Public Function SaveBeneficiaryOptions() As String
    '        Dim strdecsdPersID As String
    '        Dim strdecsdFundeventID As String
    '        Dim StrBeneficiaryName As String
    '        Dim strBeneficiarySSNo As String
    '        Dim strBeneficiaryType As String
    '        Dim decJSAnnuityAmount As Decimal
    '        Dim decRetPlan As Decimal
    '        Dim decPrincipalGuaranteeAnnuity_RP As Decimal
    '        Dim decSavPlan As Decimal
    '        Dim decPrincipalGuaranteeAnnuity_SP As Decimal
    '        Dim decDeathBenefit As Decimal
    '        Dim decAnnuityMFromRP As Decimal
    '        Dim decFirstMAnnuityFromRP As Decimal
    '        Dim decAnnuityCFromRP As Decimal
    '        Dim decFirstCAnnuityFromRP As Decimal
    '        Dim decLumpSumFromNonHumanBen As Decimal
    '        Dim decAnnuityMFromSP As Decimal
    '        Dim decFirstMAnnuityFromSP As Decimal
    '        Dim decAnnuityCFromSP As Decimal
    '        Dim decFirstCAnnuityFromSP As Decimal
    '        Dim decAnnuityMFromRDB As Decimal
    '        Dim decFirstMAnnuityFromRDB As Decimal
    '        Dim decAnnuityFromJSAndRDB As Decimal
    '        Dim decFirstAnnuityFromJSAndRDB As Decimal
    '        Dim decAnnuityMFromResRemainingOfRP As Decimal
    '        Dim decFirstMAnnuityFromResRemainingOfRP As Decimal
    '        Dim decAnnuityCFromResRemainingOfRP As Decimal
    '        Dim decFirstCAnnuityFromResRemainingOfRP As Decimal
    '        Dim decAnnuityMFromResRemainingOfSP As Decimal
    '        Dim decFirstMAnnuityFromResRemainingOfSP As Decimal
    '        Dim decAnnuityCFromResRemainingOfSP As Decimal
    '        Dim decFirstCAnnuityFromResRemainingOfSP As Decimal
    '        Dim intMonths As Integer
    '        'Initially all sections are set to false
    '        Dim blnActiveDeathBenfit As Boolean = False
    '        Dim blnSection1Visible As Boolean = False
    '        Dim blnSection2Visible As Boolean = False
    '        Dim blnSection3Visible As Boolean = False
    '        Dim blnSection4Visible As Boolean = False
    '        Dim blnSection5Visible As Boolean = False
    '        Dim blnSection6Visible As Boolean = False
    '        Dim blnSection7Visible As Boolean = False
    '        Dim blnSection8Visible As Boolean = False
    '        Dim blnSection9Visible As Boolean = False
    '        Dim blnSection10Visible As Boolean = False
    '        Dim blnSection11Visible As Boolean = False
    '        Dim blnSection12Visible As Boolean = False
    '        Dim strReturnStatus As String
    '        Dim l_ds_jsdata As DataSet



    '        Try
    '            'Getting the persid and fundevent id from session variables
    '            strdecsdPersID = Session("PersId").ToString().Trim
    '            strdecsdFundeventID = Session("FundId").ToString().Trim
    '            'get the js name and js amount from database and storing in variables
    '            l_ds_jsdata = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.PopulateJSAnnuities(strdecsdFundeventID)
    '            If HelperFunctions.isNonEmpty(l_ds_jsdata) Then
    '                StrBeneficiaryName = IIf(l_ds_jsdata.Tables(0).Rows(0)("Output_txtFirstName").ToString().Trim() = "&nbsp;", "", l_ds_jsdata.Tables(0).Rows(0)("Output_txtFirstName").ToString().Trim()) + " " + IIf(l_ds_jsdata.Tables(0).Rows(0)("Output_txtLastName").ToString().Trim() = "&nbsp;", "", l_ds_jsdata.Tables(0).Rows(0)("Output_txtLastName").ToString().Trim())
    '                If Not String.IsNullOrEmpty(l_ds_jsdata.Tables(0).Rows(0)("Output_txtTotalMonthlyAnnuityAmount").ToString()) Or l_ds_jsdata.Tables(0).Rows(0)("Output_txtTotalMonthlyAnnuityAmount").ToString() <> "&nbsp;" Then
    '                    decJSAnnuityAmount = decJSAnnuityAmount + l_ds_jsdata.Tables(0).Rows(0)("Output_txtTotalMonthlyAnnuityAmount")
    '                End If
    '            End If
    '            ' storing details in database 
    '            strReturnStatus = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.SaveDeathBenefitCalculatorFormDetails(strdecsdPersID, strdecsdFundeventID, _
    '            StrBeneficiaryName, _
    '            decJSAnnuityAmount, _
    '            decRetPlan, _
    '            decPrincipalGuaranteeAnnuity_RP, _
    '            decSavPlan, _
    '            decPrincipalGuaranteeAnnuity_SP, _
    '            decDeathBenefit, _
    '            decAnnuityMFromRP, _
    '            decFirstMAnnuityFromRP, _
    '            decAnnuityCFromRP, _
    '            decFirstCAnnuityFromRP, _
    '            decLumpSumFromNonHumanBen, _
    '            decAnnuityMFromSP, _
    '            decFirstMAnnuityFromSP, _
    '            decAnnuityCFromSP, _
    '            decFirstCAnnuityFromSP, _
    '            decAnnuityMFromRDB, _
    '            decFirstMAnnuityFromRDB, _
    '            decAnnuityFromJSAndRDB, _
    '            decFirstAnnuityFromJSAndRDB, _
    '            decAnnuityMFromResRemainingOfRP, _
    '            decFirstMAnnuityFromResRemainingOfRP, _
    '            decAnnuityCFromResRemainingOfRP, _
    '            decFirstCAnnuityFromResRemainingOfRP, _
    '            decAnnuityMFromResRemainingOfSP, _
    '            decFirstMAnnuityFromResRemainingOfSP, _
    '            decAnnuityCFromResRemainingOfSP, _
    '            decFirstCAnnuityFromResRemainingOfSP, _
    '            intMonths, _
    '            blnActiveDeathBenfit,
    '            blnSection1Visible, _
    '            blnSection2Visible, _
    '            blnSection3Visible, _
    '            blnSection4Visible, _
    '            blnSection5Visible, _
    '            blnSection6Visible, _
    '            blnSection7Visible, _
    '            blnSection8Visible, _
    '            blnSection9Visible, _
    '            blnSection10Visible, _
    '            blnSection11Visible, _
    '            blnSection12Visible)
    '            'from database it returns the appid
    '            Return strReturnStatus
    '        Catch
    '            Throw
    '        End Try

    '    End Function

    '    'It gets the details which has selected forms and details by users and save into database
    '    Public Sub SaveFormDetails()
    '        Dim intDBAppFormID As Integer = 0
    '        Dim intMetaDBAdditionalFormID As Integer
    '        Dim chvAdditionalText As String
    '        Dim strFormlist As String
    '        Dim strForms() As String
    '        Dim chvText As String
    '        Dim intUniqueID As Integer
    '        Dim dtDeathBenefitFormReqdDocs As New DataTable
    '        Dim drDeathBenefitFormReqdDocs As DataRow
    '        Dim DataSet_AdditionalForms As DataSet
    '        Try

    '            dtDeathBenefitFormReqdDocs.Columns.Add("intDBAppFormID")
    '            dtDeathBenefitFormReqdDocs.Columns.Add("intMetaDBAdditionalFormID")
    '            dtDeathBenefitFormReqdDocs.Columns.Add("chvAdditionalText")
    '            'Gets the intDBFormId From the Main table after process
    '            intDBAppFormID = Convert.ToInt32(SaveBeneficiaryOptions())


    '            If Not Session("Formlist") Is Nothing And Session("Formlist") <> "" Then
    '                DataSet_AdditionalForms = Session("DataSet_AdditionalForms")
    '                'Gets the form uniqueid For form "A copy of the death certificate for" to add User name in additional text for this form
    '                For i As Integer = 0 To DataSet_AdditionalForms.Tables(0).Rows.Count - 1
    '                    If DataSet_AdditionalForms.Tables(0).Rows(i).Item("chvText").ToString().Trim() = "A copy of the death certificate for" Then
    '                        intUniqueID = DataSet_AdditionalForms.Tables(0).Rows(i).Item("intUniqueID")
    '                        Exit For
    '                    End If
    '                Next
    '                'gets the concatinated string from session and store into variable
    '                strFormlist = Session("Formlist")
    '                If strFormlist.Contains("$$") Then
    '                    strForms = strFormlist.Split("$$")
    '                Else
    '                    ReDim strForms(0)
    '                    strForms(0) = strFormlist
    '                End If
    '                ' Splits and extract the form id and additional text and inserts into the datatable
    '                For i As Integer = 0 To strForms.Length - 1
    '                    If Not strForms(i) = "" Then
    '                        If strForms(i).Contains(",") Then
    '                            intMetaDBAdditionalFormID = Convert.ToInt32(strForms(i).Substring(0, strForms(i).IndexOf(",")))

    '                            If intMetaDBAdditionalFormID = intUniqueID Then
    '                                'concatinates the user name with additional text for the respective unique id
    '                                chvAdditionalText = TextBoxGeneralFirstName.Text + " " + TextBoxGeneralLastName.Text

    '                                chvAdditionalText = chvAdditionalText + "," + strForms(i).Substring(strForms(i).IndexOf(",") + 1, strForms(i).Length - (strForms(i).IndexOf(",") + 1))
    '                            Else
    '                                chvAdditionalText = strForms(i).Substring(strForms(i).IndexOf(",") + 1, strForms(i).Length - (strForms(i).IndexOf(",") + 1))
    '                            End If
    '                        Else
    '                            intMetaDBAdditionalFormID = Convert.ToInt32(strForms(i))
    '                            If intMetaDBAdditionalFormID = intUniqueID Then
    '                                'concatinates the user name with additional text for the respective unique id
    '                                chvAdditionalText = TextBoxGeneralFirstName.Text + " " + TextBoxGeneralLastName.Text

    '                            Else
    '                                chvAdditionalText = ""
    '                            End If
    '                        End If
    '                        ' the extracted values are stored in datarow and inserted in atable
    '                        drDeathBenefitFormReqdDocs = dtDeathBenefitFormReqdDocs.NewRow()
    '                        drDeathBenefitFormReqdDocs("intDBAppFormID") = intDBAppFormID
    '                        drDeathBenefitFormReqdDocs("intMetaDBAdditionalFormID") = intMetaDBAdditionalFormID
    '                        drDeathBenefitFormReqdDocs("chvAdditionalText") = chvAdditionalText
    '                        dtDeathBenefitFormReqdDocs.Rows.Add(drDeathBenefitFormReqdDocs)
    '                    End If
    '                Next
    '                'Finnaly datatable is sent to database for storing form details
    '                If dtDeathBenefitFormReqdDocs.Rows.Count <> 0 Then
    '                    YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.SaveDeathFormDetails(dtDeathBenefitFormReqdDocs)
    '                End If
    '            End If
    '            'To view the death benefit applicaion form and store in idm folder as pdf files
    '            ViewDeathBenefitFormAndCopyIdm(intDBAppFormID)

    '        Catch
    '            Throw
    '        Finally
    '            Session("Formlist") = Nothing
    '            Session("ShowDeathBenefitForm") = Nothing
    '            Session("DataSet_AdditionalForms") = Nothing
    '            dtDeathBenefitFormReqdDocs = Nothing
    '        End Try
    '    End Sub
    '    'Calling report viewer and exportinf into pdf and storing idx values
    '    Public Sub ViewDeathBenefitFormAndCopyIdm(ByVal intDBAppFormID As Integer)
    '        Dim l_stringDocType As String = String.Empty
    '        Dim l_StringReportName As String = String.Empty
    '        Dim l_ArrListParamValues As New ArrayList
    '        Dim l_string_OutputFileType As String = String.Empty
    '        Dim l_StringErrorMessage As String = String.Empty
    '        Try

    '            Session("strReportName_1") = "Death Benefit Application"
    '            Session("intDBAppFormID") = intDBAppFormID
    '            'Calling reportviewer_1.aspx to open report in another window and not to overlap with previous report
    '            Dim popupScript As String = "<script language='javascript'>" & _
    '            "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp_db', " & _
    '            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
    '            "</script>"
    '            If (Not Me.IsStartupScriptRegistered("PopupScriptdb")) Then
    '                Page.RegisterStartupScript("PopupScriptdb", popupScript)
    '            End If

    '        l_stringDocType = "DTHBENAP" 'SR:2013.06.10: BT-2068/YRS 5.0-2121 - Changed doccode from 07DTHBENAP to DTHBENAP for Deathbenefitapplicationform
    '            l_StringReportName = "Death Benefit Application"
    '            l_ArrListParamValues.Add(intDBAppFormID)
    '            l_string_OutputFileType = "DeathBenefit_" + l_stringDocType
    '            'Copies report into idm convert into pdf and stores information idx 
    '            l_StringErrorMessage = CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
    '            If Not l_StringErrorMessage Is String.Empty Then
    '                MessageBox.Show(PlaceHolder1, "IDM Error", l_StringErrorMessage, MessageBoxButtons.Stop, False)
    '            End If
    '        Catch
    '            Throw
    '        End Try
    '    End Sub
    'Start: AA:2015.17.04 - BT:2824 : YRS 5.0-2499: Uncommented because this method is called by Printform method
    Private Function CopyToIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String) As String
        Dim l_StringErrorMessage As String
        Dim IDM As New IDMforAll
        Try
            'gets the columns for idm and stored in session varilable 
            If IDM.DatatableFileList(False) Then
                Session("FTFileList") = IDM.SetdtFileList
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

            If Not Session("PersId") Is Nothing Then
                IDM.PersId = DirectCast(Session("PersId"), String)
            End If

            IDM.DocTypeCode = l_StringDocType
            IDM.OutputFileType = l_string_OutputFileType
            IDM.ReportName = l_StringReportName.ToString().Trim & ".rpt"
            IDM.ReportParameters = l_ArrListParamValues

            l_StringErrorMessage = IDM.ExportToPDF()
            l_ArrListParamValues.Clear()

            Session("FTFileList") = IDM.SetdtFileList


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
            Return l_StringErrorMessage

        Catch
            Throw
        Finally
            IDM = Nothing
        End Try
    End Function
    'End: AA:2015.17.04 - BT:2824 : YRS 5.0-2499: Uncommented because this method is called by Printform method
    '#End Region
    'End: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form
    'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
    Private Sub btnEditPrimaryContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditPrimaryContact.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralAddress", Convert.ToInt32(Session("LoggedUserKey")))
        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            Exit Sub
        End If
        tbPricontacts.Visible = True
        btnEditPrimaryContact.Visible = False
        Dim dt_PrimaryContact As DataTable = DirectCast(Session("dt_PrimaryContactInfo"), DataTable)
        RefreshPrimaryTelephoneDetail(dt_PrimaryContact)
        Me.TelephoneWebUserControl1.Visible = False
        Me.ButtonActivateAsPrimary.Enabled = False
        Me.ButtonSaveRetireeParticipants.Enabled = True
        Me.ButtonRetireesInfoCancel.Enabled = True
        Me.CheckboxBadEmail.Enabled = True
        Me.CheckboxUnsubscribe.Enabled = True
        Me.CheckboxTextOnly.Enabled = True
        Me.TextBoxEmailId.Enabled = True
        Me.TextBoxEmailId.ReadOnly = False
        Me.LabelEmailId.Enabled = True
        Me.MakeLinkVisible()
    End Sub
    'SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-Start
    Private Sub CreateTempSSNDatatableForUpdate(ByVal strOldSSN As String)
        Dim dtSSNUpdate As DataTable
        'In this method Session("FundNo") is used which has been set on find info screen.
        Try
            Session("SessionTempSSNTable") = Nothing
            dtSSNUpdate = New DataTable()
            dtSSNUpdate.Columns.Add("OldSSN")
            dtSSNUpdate.Columns.Add("NewSSN")
            dtSSNUpdate.Columns.Add("Reason")
            dtSSNUpdate.Columns.Add("FundNo")

            Dim dr As DataRow = dtSSNUpdate.NewRow()
            dr("OldSSN") = strOldSSN
            dr("FundNo") = Session("FundNo")
            dr("NewSSN") = String.Empty
            dr("Reason") = String.Empty
            dtSSNUpdate.Rows.Add(dr)
            dtSSNUpdate.AcceptChanges()
            Session("SessionTempSSNTable") = dtSSNUpdate
        Catch ex As Exception

        End Try
    End Sub

    Private Function IsSSNEdited() As Boolean
        Dim dtSSNUpdate As DataTable
        Dim bSSNEdited
        Try
            bSSNEdited = False
            If (Session("SessionTempSSNTable") IsNot Nothing) Then
                dtSSNUpdate = CType(Session("SessionTempSSNTable"), DataTable)
                If (HelperFunctions.isNonEmpty(dtSSNUpdate)) Then
                    'check if new ssn is not empty and  new ssn is not equals to old ssn
                    If ((Not String.IsNullOrEmpty(dtSSNUpdate.Rows(0)("NewSSN"))) And dtSSNUpdate.Rows(0)("NewSSN") <> dtSSNUpdate.Rows(0)("OldSSN")) Then
                        bSSNEdited = True
                    End If
                End If
            End If
            Return bSSNEdited
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub SetUpdatedSSN()
        Dim dtSSNUpdate As DataTable
        Try
            If (Session("SessionTempSSNTable") IsNot Nothing) Then
                dtSSNUpdate = DirectCast(Session("SessionTempSSNTable"), DataTable)
                If (HelperFunctions.isNonEmpty(dtSSNUpdate) And Not String.IsNullOrEmpty(Convert.ToString(dtSSNUpdate.Rows(0)("NewSSN")))) Then
                    TextBoxGeneralSSNo.Text = Convert.ToString(dtSSNUpdate.Rows(0)("NewSSN"))
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | checking is annuity ssn is updated and replaced in textbox and dataset
    Private Sub SetUpdatedAnnuitySSN()
        Dim dtSSNUpdate As DataTable
        Dim drChangedSSNo As DataRow()
        Dim strBeneficiaryUniqueId As String
        Try
            strBeneficiaryUniqueId = CType(Session("AnnuityJointSurvivorsID"), String)
            dtSSNUpdate = CType(Session("AuditBeneficiariesTable"), DataTable)
            drChangedSSNo = dtSSNUpdate.Select(String.Format("UniqueId='{0}'", strBeneficiaryUniqueId))
            If ((Not String.IsNullOrEmpty(drChangedSSNo(0)("NewSSN"))) And drChangedSSNo(0)("NewSSN") <> drChangedSSNo(0)("OldSSN")) Then
                TextBoxAnnuitiesSSNo.Text = Convert.ToString(drChangedSSNo(0)("NewSSN"))
                ButtonSaveRetireeParticipants.Enabled = True
                UpdateAnnuityBeneficiary()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | checking is annuity ssn is updated and replaced in textbox and dataset

    'SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-End
    Private Sub btnEditSecondaryContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditSecondaryContact.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralAddress", Convert.ToInt32(Session("LoggedUserKey")))
        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            Exit Sub
        End If
        tbSeccontacts.Visible = True
        btnEditSecondaryContact.Visible = False
        Dim dt_SecondaryContact As DataTable = DirectCast(Session("dt_SecondaryContactInfo"), DataTable)
        RefreshSecTelephoneDetail(dt_SecondaryContact)
        Me.TelephoneWebUserControl2.Visible = False
        Me.ButtonActivateAsPrimary.Enabled = False
        Me.ButtonSaveRetireeParticipants.Enabled = True
        Me.ButtonRetireesInfoCancel.Enabled = True
        Me.MakeLinkVisible()
        'Start:Anudeep-2013.08.21 - YRS 5.0-2162:Edit button for email address 
        Me.CheckboxBadEmail.Enabled = True
        Me.CheckboxUnsubscribe.Enabled = True
        Me.CheckboxTextOnly.Enabled = True
        Me.TextBoxEmailId.Enabled = True
        Me.TextBoxEmailId.ReadOnly = False
        Me.LabelEmailId.Enabled = True
        'End:Anudeep-2013.08.21 - YRS 5.0-2162:Edit button for email address 
    End Sub
    'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
    'Start:Anudeep-2013.08.21 - YRS 5.0-2162:Edit button for email address 
    Private Sub imgBtnEmail_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnEmail.Click
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonGeneralAddress", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            tbPricontacts.Visible = True
            btnEditPrimaryContact.Visible = False
            Dim dt_PrimaryContact As DataTable = DirectCast(Session("dt_PrimaryContactInfo"), DataTable)
            RefreshPrimaryTelephoneDetail(dt_PrimaryContact)
            Me.TelephoneWebUserControl1.Visible = False
            Me.ButtonActivateAsPrimary.Enabled = False
            Me.ButtonSaveRetireeParticipants.Enabled = True
            Me.ButtonRetireesInfoCancel.Enabled = True
            Me.CheckboxBadEmail.Enabled = True
            Me.CheckboxUnsubscribe.Enabled = True
            Me.CheckboxTextOnly.Enabled = True
            Me.TextBoxEmailId.Enabled = True
            Me.TextBoxEmailId.ReadOnly = False
            Me.LabelEmailId.Enabled = True
            Me.MakeLinkVisible()
        Catch
            Throw
        End Try
    End Sub
    'End:Anudeep-2013.08.21 - YRS 5.0-2162:Edit button for email address 

    'Start:AA:2014.02.03 - BT:2292:YRS 5.0-2248
#Region "PIN NUMBER"
    'Load PIN from session and fill in textbox
    Public Sub LoadUserPINdetails()
        Dim arrPINdtls(1) As String
        Try
            arrPINdtls = Session("PersPIN")
            If arrPINdtls IsNot Nothing AndAlso arrPINdtls(1).Trim().Length = 4 Then
                txtPIN.Text = arrPINdtls(1)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'On click of Edit fills the Jquery pop-up with the PIN
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetPIN() As String
        Dim arrPINdtls(1) As String
        Dim strPin As String
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonRetPINno", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
        Try
            If Not checkSecurity.Equals("True") Then
                Return checkSecurity
            End If
            arrPINdtls = HttpContext.Current.Session("PersPIN")
            If arrPINdtls IsNot Nothing Then
                strPin = arrPINdtls(1)
            End If
            Return strPin
        Catch ex As Exception
            HelperFunctions.LogException("RetireInformation - GetPIN", ex)
            Return ex.Message
        End Try
    End Function
    'Updates the PIN 
    <System.Web.Services.WebMethod()> _
    Public Shared Function UpdatePIN(ByVal strPIN As String) As String
        Dim strReturnPIN As String
        Dim arrPINdtls(1) As String
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonRetPINno", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
        Try
            If Not checkSecurity.Equals("True") Then
                Return checkSecurity
            End If
            If strPIN <> String.Empty Then
                strReturnPIN = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdatePIN(HttpContext.Current.Session("PersId"), strPIN)
            End If
            If strReturnPIN.Trim.Length = 4 Then
                arrPINdtls(1) = strReturnPIN
                HttpContext.Current.Session("PersPIN") = arrPINdtls
            End If
            Return strReturnPIN
        Catch ex As Exception
            HelperFunctions.LogException("RetireInformation - UpdatePIN", ex)
            Return ex.Message
        End Try
    End Function
    'Deletes the PIN value
    <System.Web.Services.WebMethod()> _
    Public Shared Function DeletePIN() As String
        Dim arrPINdtls(1) As String
        Dim strRetStatus As String
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonRetPINno", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
        Try
            If Not checkSecurity.Equals("True") Then
                Return checkSecurity
            End If
            arrPINdtls = HttpContext.Current.Session("PersPIN")
            If arrPINdtls IsNot Nothing Then
                strRetStatus = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DeletePIN(HttpContext.Current.Session("PersId"))
                strRetStatus = strRetStatus.Trim()
                If strRetStatus = "1" Then
                    arrPINdtls(1) = ""
                    HttpContext.Current.Session("PersPIN") = arrPINdtls
                End If
            Else
                Return "PIN does not exists."
            End If
            Return strRetStatus
        Catch ex As Exception
            HelperFunctions.LogException("ParticpantInformtion - DeletePIN", ex)
            Return ex.Message
        End Try
    End Function
#End Region
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim dsBenefeciaries As DataSet
        'Dim isContingentMessageExists As Boolean 'SP 2014.12.03 BT-2310\YRS 5.0-2255:
        Try
            DivMainMessage.Visible = False
            'Anudeep:21.10.2013:BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
            'If TextBoxCont2A.Text IsNot String.Empty Or TextBoxCont3A.Text IsNot String.Empty Or TextBoxCont2InsA.Text IsNot String.Empty Or TextBoxCont3InsA.Text IsNot String.Empty Then
            dsBenefeciaries = Session("BeneficiariesRetired")
            If HelperFunctions.isNonEmpty(dsBenefeciaries) Then
                If dsBenefeciaries.Tables(0).Select("Groups = 'CONT' And Lvl IN ('LVL2','LVL3')").Length > 0 Then
                    ShowMessage(Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_CONTINGENT_LVL2_LVL3_NOT_EXISTS)
                    'Else
                    '    DivMainMessage.Visible = False
                End If
                'Else
                '    DivMainMessage.Visible = False
            End If

            'START : BD : 2018.11.20 : YRS-AT-4136 For Particpant Enrolled on or after 1/1/2019 validate beneficiary type
            If HelperFunctions.isNonEmpty(dsBenefeciaries) Then
                If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Session("FundId").ToString().Trim) Then
                    DisplayMessageBasedOnInsuredReserveAnnuityAndBeneficiaryType(dsBenefeciaries)
                End If
            End If
            'END : BD : 2018.11.20 : YRS-AT-4136 For Particpant Enrolled on or after 1/1/2019 validate beneficiary type
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            If (IsModifiedPercentage) Then
                ShowMessage(Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DEATH_CALC_RUN)
                'DivMainMessage.Visible = True
                'If String.IsNullOrEmpty(DivMainMessage.InnerHtml.Trim) Then
                '    DivMainMessage.InnerHtml = Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DEATH_CALC_RUN
                'Else
                '    DivMainMessage.InnerHtml = "</br>" + Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DEATH_CALC_RUN
                'End If
                'ElseIf isContingentMessageExists Then
                '    DivMainMessage.Visible = True
                '    'AA 2015.26.06      BT:2906 : YRS 5.0-2547: Commented because it already gets set in beneficiary section
                '    'DivMainMessage.Visible = Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_CONTINGENT_LVL2_LVL3_NOT_EXISTS
                'Else
                '    DivMainMessage.Visible = False
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
        Catch
            Throw
        End Try
    End Sub
    'AA:16.02.2014:BT:2291:YRS 5.0 2247 : Added to below code to check whether already web account exists for the participant.
    Private Sub chkNoWebAcctCreate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNoWebAcctCreate.CheckedChanged
        Dim l_dataset_loginCredential As DataSet
        Dim l_logincredential As DataTable
        Try
            If chkNoWebAcctCreate.Checked Then
                If Not Session("PersId") Is Nothing Then
                    'Start: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Added code to check the user has created web account using webservice instead of database
                    'l_dataset_loginCredential = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpGeneralInfo(Session("PersId"))
                    'If Not l_dataset_loginCredential Is Nothing Then
                    '    l_logincredential = l_dataset_loginCredential.Tables("LoginCredential")
                    '    If HelperFunctions.isNonEmpty(l_logincredential) Then
                    '        MessageBox.Show(PlaceHolder1, "YMCA_YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_RESTRICT_WEB_ACCT_INFO, MessageBoxButtons.YesNo)
                    '        chkNoWebAcctCreate.Checked = False
                    '        Exit Sub
                    '    End If
                    'End If
                    Dim proxy As New AdminWebAccountInfo()
                    Dim strPasAdminDB As String = proxy.GetUserDetails(Session("PersId"))
                    Dim serializer As New XmlSerializer(GetType(adminConsoleValDb), New XmlRootAttribute("adminConsoleVal"))
                    Dim objWebAcctDetails As adminConsoleValDb = TryCast(serializer.Deserialize(New StringReader(strPasAdminDB)), adminConsoleValDb)

                    If String.IsNullOrEmpty(objWebAcctDetails.UserName) Then
                        MessageBox.Show(PlaceHolder1, "Restrict Web Acct. Confirmation", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_RESTRICT_WEB_ACCT_INFO_NOT_EXISTS, MessageBoxButtons.YesNo)
                    Else
                        MessageBox.Show(PlaceHolder1, "Restrict Web Acct. Confirmation", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_RESTRICT_WEB_ACCT_INFO_EXISTS, MessageBoxButtons.YesNo)
                    End If
                    ViewState("chkNoWebAcctCreate_verify") = "true"
                    'End: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Added code to check the user has created web account using webservice instead of database
                End If
            End If
            ButtonSaveRetireeParticipants.Enabled = True
            ButtonRetireesInfoCancel.Enabled = True
            Me.HyperLinkViewParticipantsInfo.Visible = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'Start:Anudeep:16.02.2013:BT:2306:YRS 5.0-2251 : Added code to fill the participant address in address control
    Private Sub lnkParticipantAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkParticipantAddress.Click
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
                AddressWebUserControlAnn.LoadAddressDetail(dr_PrimaryAddress)
                AddressWebUserControlAnn.Notes = "Address has been added / updated from participant address."
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'End:Anudeep:16.02.2013:BT:2306:YRS 5.0-2251 : Added code to fill the participant address in address control


    'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
    Private Function IsBeneficiaryPercentageChanged(ByVal dsOriginalBeneficiaries As DataSet, ByVal dsModifiedBeneficiaires As DataSet, ByVal persId As String) As Boolean

        Dim dsBenefitFollowupBeneficiaries As DataSet
        Dim isChanged As Boolean
        Try
            dsBenefitFollowupBeneficiaries = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetFollowupBeneficiariesDetails(persId)

            If HelperFunctions.isEmpty(dsBenefitFollowupBeneficiaries) Then
                isChanged = False
                Exit Function
            End If

            If HelperFunctions.isNonEmpty(dsBenefitFollowupBeneficiaries) AndAlso HelperFunctions.isEmpty(dsOriginalBeneficiaries) Then
                isChanged = False
                Exit Function
            ElseIf HelperFunctions.isNonEmpty(dsBenefitFollowupBeneficiaries) AndAlso HelperFunctions.isNonEmpty(dsOriginalBeneficiaries) Then

                If HelperFunctions.isNonEmpty(dsModifiedBeneficiaires) Then
                    Dim drFoundRows As DataRow()
                    Dim drOriginal As DataRow
                    For Each drUpdated As DataRow In dsModifiedBeneficiaires.Tables(0).Rows
                        If (drUpdated.RowState = DataRowState.Modified) Then

                            drFoundRows = GetBenficiaryRows(dsOriginalBeneficiaries.Tables(0), drUpdated("uniqueid").ToString)

                            If drFoundRows.Length > 0 Then

                                drOriginal = drFoundRows(0)
                                'checking percentage mismatch
                                If Convert.ToDecimal(drOriginal("Pct")) <> Convert.ToDecimal(drUpdated("Pct")) Then
                                    Dim drDthBeneRows As DataRow()
                                    Dim drDthBeneRow As DataRow
                                    Dim strFilterExpression As String = "ISNULL(BeneficiaryFirstName,'')='" + drOriginal("Name").ToString.Trim().Replace("'", "''") + "' AND ISNULL(BeneficiaryLastName,'')='" + drOriginal("Name2").ToString.Trim.Replace("'", "''") + "' AND ISNULL(BeneficiaryTaxNumber,'')='" + drOriginal("TaxID").ToString.Trim + "'"
                                    drDthBeneRows = dsBenefitFollowupBeneficiaries.Tables(0).Select(strFilterExpression)
                                    If drDthBeneRows.Length > 0 Then
                                        isChanged = True
                                        Exit For
                                    End If
                                Else
                                    IsModifiedPercentage = False
                                End If

                            End If

                        End If
                    Next

                End If

            End If

        Catch
            Throw
        End Try
        Return isChanged

    End Function

    Private Function GetBenficiaryRows(ByVal dtBeneficiaries As DataTable, ByVal strguiBeneficiaryUniqueid As String) As DataRow()
        Return dtBeneficiaries.Select("uniqueid = '" + strguiBeneficiaryUniqueid + "'")
    End Function
    'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End

    'START: PPP | 2015.10.12 | YRS-AT-2588 | Validates all four telephone box and retrieves error message from AtsMetaMessages
    Private Function ValidateTelephoneNumbers(ByVal stOffice As String, ByVal stHome As String, ByVal stMobile As String, ByVal stFax As String) As String
        Dim lsErrorList As List(Of String)
        Dim stTempErrorHolder, stErrors As String

        lsErrorList = New List(Of String)
        If Not String.IsNullOrEmpty(stOffice) And stOffice.Length > 0 Then
            stTempErrorHolder = Validation.Telephone(stOffice, YMCAObjects.TelephoneType.Office)
            If (Not String.IsNullOrEmpty(stTempErrorHolder)) Then
                lsErrorList.Add(stTempErrorHolder)
            End If
        End If

        If Not String.IsNullOrEmpty(stHome) And stHome.Length > 0 Then
            stTempErrorHolder = Validation.Telephone(stHome, YMCAObjects.TelephoneType.Home)
            If (Not String.IsNullOrEmpty(stTempErrorHolder)) Then
                lsErrorList.Add(stTempErrorHolder)
            End If
        End If

        If Not String.IsNullOrEmpty(stMobile) And stMobile.Length > 0 Then
            stTempErrorHolder = Validation.Telephone(stMobile, YMCAObjects.TelephoneType.Mobile)
            If (Not String.IsNullOrEmpty(stTempErrorHolder)) Then
                lsErrorList.Add(stTempErrorHolder)
            End If
        End If

        If Not String.IsNullOrEmpty(stFax) And stFax.Length > 0 Then
            stTempErrorHolder = Validation.Telephone(stFax, YMCAObjects.TelephoneType.Fax)
            If (Not String.IsNullOrEmpty(stTempErrorHolder)) Then
                lsErrorList.Add(stTempErrorHolder)
            End If
        End If

        stErrors = String.Empty
        If lsErrorList.Count > 0 Then
            For Each item As String In lsErrorList
                stErrors = String.Format("{0}{1}{2}", stErrors, item, "<br />")
            Next
        End If

        ValidateTelephoneNumbers = stErrors
    End Function

    Private Function ValidateTelephoneNumbers(ByVal dtTelephoneNumbers As DataTable) As String
        Dim stOffice As String, stHome As String, stMobile As String, stFax As String

        stOffice = String.Empty
        stHome = String.Empty
        stMobile = String.Empty
        stFax = String.Empty
        For Each drTelephone In dtTelephoneNumbers.Rows
            Select Case drTelephone("PhoneType").ToString()
                Case "Office"
                    stOffice = drTelephone("PhoneNumber").ToString().Trim()
                Case "Home"
                    stHome = drTelephone("PhoneNumber").ToString().Trim()
                Case "Mobile"
                    stMobile = drTelephone("PhoneNumber").ToString().Trim()
                Case "Fax"
                    stFax = drTelephone("PhoneNumber").ToString().Trim()
            End Select
        Next

        ValidateTelephoneNumbers = ValidateTelephoneNumbers(stOffice, stHome, stMobile, stFax)
    End Function
    'END: PPP | 2015.10.12 | YRS-AT-2588 | Validates all four telephone box and retrieves error message from AtsMetaMessages

    'Start:AA:10.27.2015 YRS-AT-2361 Added a validations check before adding a bank into dataset
    Private Function IsBankValid() As Boolean
        Dim strBankEffectiveDate As String
        Dim dsBanks As DataSet
        Try
            If Session("SelectBank_BankName") Is Nothing Then
                Return False 'Return false if bank name does not exists
            End If

            If Session("BankEffectiveDate") Is Nothing Then
                Return False 'Return false if effective date is blank
            End If

            dsBanks = Session("BankingDtls")
            strBankEffectiveDate = Session("BankEffectiveDate").ToString

            If String.IsNullOrEmpty(Session("SelectBank_BankName").ToString) Then
                Return False 'Return false if bank name does not exists
            End If

            If String.IsNullOrEmpty(strBankEffectiveDate) Then
                Return False 'Return false if effective date is blank
            End If

            If HelperFunctions.isNonEmpty(dsBanks) Then
                If dsBanks.Tables(0).Columns.Contains("EffecDate") AndAlso dsBanks.Tables(0).Select("EffecDate = '" + strBankEffectiveDate + "'").Length > 0 Then
                    Return False 'Return false if already a record exists for the same effective date
                End If
            End If

            Return True
        Catch
            Throw
        End Try
    End Function
    'End:AA:10.27.2015 YRS-AT-2361 Added a validations check before adding a bank into dataset
    'Start: Bala: 01/05/2016: YRS-AT-1972: Special death processing required check box event.

    'START: SN | 11/14/2019 | YRS-AT-4604 | Added webmethod for ajax call to show taxWithholding details in popup.
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetTaxWithHoldingDetails(ByVal requestedDisbursementId As String)
        Try
            If Not String.IsNullOrEmpty(requestedDisbursementId) Then
                Dim dsWithHoldings As DataSet
                dsWithHoldings = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpDisbursement(requestedDisbursementId)

                Dim withHoldingsList As List(Of YMCAObjects.WithHoldings) = New List(Of YMCAObjects.WithHoldings)()
                For i As Integer = 0 To dsWithHoldings.Tables(0).Rows.Count - 1
                    Dim withHoldingDetails As YMCAObjects.WithHoldings = New YMCAObjects.WithHoldings()
                    withHoldingDetails.Amount = dsWithHoldings.Tables(0).Rows(i)("Amount").ToString()
                    withHoldingDetails.WithHoldingType = dsWithHoldings.Tables(0).Rows(i)("WithHolding Type").ToString()
                    withHoldingsList.Add(withHoldingDetails)
                Next
                Return withHoldingsList
            Else
                Return Nothing
            End If

        Catch ex As Exception
            HelperFunctions.LogException("RetireesInformationWebForm_GetTaxWithHoldingDetails", ex)
        End Try
    End Function
    'END: SN | 11/14/2019 | YRS-AT-4604 | Added webmethod for ajax call to show taxWithholding details in popup.

    Private Sub chkSpecialDeathProcess_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSpecialDeathProcess.CheckedChanged
        Try
            ButtonSaveRetireeParticipants.Enabled = True
        Catch ex As Exception
            HelperFunctions.LogException("chkSpecialDeathProcess_CheckedChanged", ex)
            Throw ex
        End Try
    End Sub
    'End: Bala: 01/05/2016: YRS-AT-1972: Special death processing required check box event.

    'START: PPP | 04/20/2016 | YRS-AT-2719 - Applying Fees and deductions to death payments - Part A.2 
    Private Sub LoadDataInUnSuppressGrid()
        Dim dsData As DataSet
        Dim bIsGenderError As Boolean
        Dim bIsFederalTaxDefined As Boolean
        Dim bHasPrimaryAddress As Boolean
        Dim bHasPurchaseDate As Boolean
        Dim bHasFirstCheck As Boolean

        Dim dTaxableAmount As Decimal
        Dim dNonTaxableAmount As Decimal
        Dim dTotal As Decimal
        Dim iMonths As Integer
        Dim dGrossAmount As Decimal
        Dim dTotalWithholdings As Decimal
        Dim dNetAmount As Decimal
        Dim bIsStateTaxDefined As Boolean 'PK|10/10/2019|YRS-AT-4598| Variable Declaration
        Try
            dsData = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetSuppressedJSAnnuitiesDetails(Convert.ToString(Session("PersId")))
            If HelperFunctions.isNonEmpty(dsData) Then
                bIsGenderError = Convert.ToBoolean(IIf(Convert.IsDBNull(dsData.Tables(0).Rows(0)("Gender")), "false", dsData.Tables(0).Rows(0)("Gender")))
                bIsFederalTaxDefined = Convert.ToBoolean(IIf(Convert.IsDBNull(dsData.Tables(0).Rows(0)("FederalTaxDefined")), "false", dsData.Tables(0).Rows(0)("FederalTaxDefined")))
                bHasPrimaryAddress = Convert.ToBoolean(IIf(Convert.IsDBNull(dsData.Tables(0).Rows(0)("PrimaryAddress")), "false", dsData.Tables(0).Rows(0)("PrimaryAddress")))
                bHasPurchaseDate = Convert.ToBoolean(IIf(Convert.IsDBNull(dsData.Tables(0).Rows(0)("PurchaseDate")), "false", dsData.Tables(0).Rows(0)("PurchaseDate")))
                bHasFirstCheck = Convert.ToBoolean(IIf(Convert.IsDBNull(dsData.Tables(0).Rows(0)("FirstCheck")), "false", dsData.Tables(0).Rows(0)("FirstCheck")))
                bIsStateTaxDefined = Convert.ToBoolean(IIf(Convert.IsDBNull(dsData.Tables(0).Rows(0)("StateTaxDefined")), "false", dsData.Tables(0).Rows(0)("StateTaxDefined"))) 'PK|10/10/2019|YRS-AT-4598| Added one more field to check for state tax withholding

                If (bIsGenderError Or bIsFederalTaxDefined Or bHasPrimaryAddress Or bHasPurchaseDate Or bHasFirstCheck Or bIsStateTaxDefined) Then 'PK| 10/10/2019 |YRS-AT-4598 | Checking condition for state withholding
                    tdUnSuppressAnnuitySummary.Style("display") = "none"
                    btnUnSuppressAnnuitySave.Style("display") = "none"
                    'START : PK| 10/10/2019 |YRS-AT-4598 | Disabling div and button when error message is not for state withholding 
                    divUnsppressErrorMessage.InnerHtml = String.Empty
                    divUnsupressStateWithholdingWarning.Style("display") = "none"
                    trConfirmYesNo.Style("Visibility") = "hidden"
                    trNoteUnsuppress.Style("Visibility") = "hidden"
                    'END : PK| 10/10/2019 |YRS-AT-4598 | Disabling div and button when error message is not for state withholding 

                    If bIsGenderError Then
                        'divUnsppressErrorMessage.InnerHtml = "The gender of the participant is marked as unknown. Please update the gender, save the record and then try to unsuppress the annuity."
                        divUnsppressErrorMessage.InnerHtml = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_UNSUPRESS_GENDER).DisplayText 'PK|10/10/2019 |YRS-AT-4598 |Commented above line to display message from database
                    ElseIf bIsFederalTaxDefined Then
                        'divUnsppressErrorMessage.InnerHtml = "Annuitant does not have federal tax withholding."
                        divUnsppressErrorMessage.InnerHtml = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_UNSUPRESS_FEDERALTAX).DisplayText 'PK|10/10/2019 |YRS-AT-4598 |Commented above line to display message from database
                    ElseIf bHasPrimaryAddress Then
                        'divUnsppressErrorMessage.InnerHtml = "Annuitant does not have a primary address."
                        divUnsppressErrorMessage.InnerHtml = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_UNSUPRESS_ADDRESS).DisplayText 'PK|10/10/2019 |YRS-AT-4598 |Commented above line to display message from database
                    ElseIf bHasPurchaseDate Then
                        'divUnsppressErrorMessage.InnerHtml = "Annuity has no purchase date."
                        divUnsppressErrorMessage.InnerHtml = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_UNSUPRESS_PURCHASEDATE).DisplayText 'PK|10/10/2019 |YRS-AT-4598 |Commented above line to display message from database
                    ElseIf bHasFirstCheck Then
                        'divUnsppressErrorMessage.InnerHtml = "Annuity has 0 firstcheck months. Try again after payroll has been run."
                        divUnsppressErrorMessage.InnerHtml = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_UNSUPRESS_FIRSTCHECK).DisplayText 'PK|10/10/2019 |YRS-AT-4598 |Commented above line to display message from database
                        'START : PK| 10/10/2019 |YRS-AT-4598 | Displaying block message for state withholding 
                    ElseIf bIsStateTaxDefined Then
                        divUnsppressErrorMessage.Style("display") = "none"
                        btnUnSuppressAnnuityCancel.Style("display") = "none"
                        divUnsupressStateWithholdingWarning.Style("display") = "block"
                        divUnsupressStateWithholdingWarning.InnerHtml = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_UNSUPRESS_STATEWITHHOLDING).DisplayText
                        trConfirmYesNo.Style("Visibility") = "visible"
                        'END : PK| 10/10/2019 |YRS-AT-4598 | Displaying block message for state withholding  
                    End If

                    divUnsppressErrorMessage.Style("height") = "80px;"
                    btnUnSuppressAnnuityCancel.Text = "  Close  "
                Else
                    divUnsppressErrorMessage.InnerHtml = String.Empty
                    divUnsppressErrorMessage.Style.Remove("height")
                    btnUnSuppressAnnuityCancel.Text = "  Cancel  "
                    divUnsupressStateWithholdingWarning.Style("display") = "none" 'PK | 2019.24.10 |YRS-AT-4598 |Declared control to display a note on unsupress button
                    trConfirmYesNo.Style("Visibility") = "hidden" 'PK | 2019.24.10 |YRS-AT-4598 |Declared control to display a note on unsupress button
                    ' Fetch other details
                    If (dsData.Tables.Count > 1) Then
                        dNetAmount = Convert.ToDecimal(IIf(Convert.IsDBNull(dsData.Tables(1).Rows(0)("NetAmount")), "0.00", dsData.Tables(1).Rows(0)("NetAmount")))
                        If dNetAmount < 0 Then
                            tdUnSuppressAnnuitySummary.Style("display") = "none"
                            btnUnSuppressAnnuitySave.Style("display") = "none"

                            divUnsppressErrorMessage.InnerHtml = "Insufficient annuity amount to apply withholdings."
                        Else
                            txtUnSuppressAnnuityTaxable.Text = Convert.ToString(IIf(Convert.IsDBNull(dsData.Tables(1).Rows(0)("TaxableAmount")), "0.00", dsData.Tables(1).Rows(0)("TaxableAmount")))
                            txtUnSuppressAnnuityNonTaxable.Text = Convert.ToString(IIf(Convert.IsDBNull(dsData.Tables(1).Rows(0)("NonTaxableAmount")), "0.00", dsData.Tables(1).Rows(0)("NonTaxableAmount")))
                            txtUnSuppressAnnuityTotal.Text = Convert.ToString(IIf(Convert.IsDBNull(dsData.Tables(1).Rows(0)("Total")), "0.00", dsData.Tables(1).Rows(0)("Total")))
                            txtUnSuppressAnnuityMonths.Text = Convert.ToString(IIf(Convert.IsDBNull(dsData.Tables(1).Rows(0)("Months")), "0.00", dsData.Tables(1).Rows(0)("Months")))
                            txtUnSuppressAnnuityGrossAmount.Text = Convert.ToString(IIf(Convert.IsDBNull(dsData.Tables(1).Rows(0)("GrossAmount")), "0.00", dsData.Tables(1).Rows(0)("GrossAmount")))
                            txtUnSuppressAnnuityDeductions.Text = Convert.ToString(IIf(Convert.IsDBNull(dsData.Tables(1).Rows(0)("TotalWithholdings")), "0.00", dsData.Tables(1).Rows(0)("TotalWithholdings")))
                            txtUnSuppressAnnuityNetAmount.Text = Convert.ToString(IIf(Convert.IsDBNull(dsData.Tables(1).Rows(0)("NetAmount")), "0.00", dsData.Tables(1).Rows(0)("NetAmount")))

                            dgUnSuppressAnnuity.DataSource = dsData.Tables(2)
                            dgUnSuppressAnnuity.DataBind()
                            tdUnSuppressAnnuitySummary.Style("display") = "normal"
                            btnUnSuppressAnnuitySave.Style("display") = "normal"
                            'START : PK | 2019.24.10 |YRS-AT-4598 |Display not on unsupress button
                            btnUnSuppressAnnuityCancel.Style("display") = "normal"
                            lblNoteUnsuppress.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_STATEAMOUNT_ALERT).DisplayText
                            trNoteUnsuppress.Style("Visibility") = "visible"
                            'END :PK | 2019.24.10 |YRS-AT-4598 |Display not on unsupress button
                        End If
                    End If
                End If
            End If

            Session("JSAnnuityDeductions") = Nothing
        Catch ex As Exception
            Throw ex
        Finally
            dsData = Nothing
        End Try
    End Sub

    Protected Sub dgUnSuppressAnnuity_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Dim chk As CheckBox
        Dim hdnCodeValue As HiddenField
        Dim txtDeductionAmount As TextBox
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                hdnCodeValue = TryCast(e.Row.FindControl("hdnCodeValue"), HiddenField)
                txtDeductionAmount = TryCast(e.Row.FindControl("txtUnSuppressAnnuityDeductionAmount"), TextBox)
                chk = TryCast(e.Row.FindControl("chkUnSuppressAnnuityDeduction"), CheckBox)
                If chk IsNot Nothing AndAlso hdnCodeValue IsNot Nothing AndAlso txtDeductionAmount IsNot Nothing Then
                    If (hdnCodeValue.Value.Trim() <> "PRCSTS") Then
                        chk.Attributes.Add("onClick", String.Format("javascript:return DeductAmount(this, '{0}', '{1}');", hdnCodeValue.Value, txtDeductionAmount.Text))
                    Else
                        chk.Attributes.Add("onClick", String.Format("javascript:return EnableFundCost(this, '{0}');", txtDeductionAmount.ClientID))
                        txtDeductionAmount.Attributes.Add("onBlur", String.Format("DeductFundCostAmount('{0}', this);", hdnCodeValue.Value.Trim()))
                        'DeductFundCostAmount
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    <System.Web.Services.WebMethod(True)> _
    Public Shared Function UpdateJSAnnuityDeduction(ByVal bIsDeductionSelected As Boolean, ByVal strWithholdings As String, ByVal strNetAmount As String, ByVal strDeductionAmount As String, ByVal strCode As String) As String
        Dim dWithholdings As Decimal
        Dim dNetAmount As Decimal
        Dim dDeductionAmount As Decimal
        Dim dExistingDeductionAmount As Decimal
        Dim diDeductions As Dictionary(Of String, Decimal)
        Try
            diDeductions = TryCast(HttpContext.Current.Session("JSAnnuityDeductions"), Dictionary(Of String, Decimal))
            If diDeductions Is Nothing Then
                diDeductions = New Dictionary(Of String, Decimal)
            End If
            dWithholdings = Convert.ToDecimal(strWithholdings)
            dNetAmount = Convert.ToDecimal(strNetAmount)
            dDeductionAmount = Convert.ToDecimal(strDeductionAmount)

            If bIsDeductionSelected And Not diDeductions.ContainsKey(strCode) Then
                dWithholdings = dWithholdings + dDeductionAmount
                dNetAmount = dNetAmount - dDeductionAmount
                diDeductions.Add(strCode, dDeductionAmount)
            ElseIf bIsDeductionSelected And diDeductions.ContainsKey(strCode) Then
                diDeductions.TryGetValue(strCode, dExistingDeductionAmount)
                If dExistingDeductionAmount >= dDeductionAmount Then
                    dWithholdings = dWithholdings - (dExistingDeductionAmount - dDeductionAmount)
                    dNetAmount = dNetAmount + (dExistingDeductionAmount - dDeductionAmount)
                ElseIf dExistingDeductionAmount < dDeductionAmount Then
                    dWithholdings = dWithholdings + (dDeductionAmount - dExistingDeductionAmount)
                    dNetAmount = dNetAmount - (dDeductionAmount - dExistingDeductionAmount)
                End If
                diDeductions(strCode) = dDeductionAmount
            Else
                dWithholdings = dWithholdings - dDeductionAmount
                dNetAmount = dNetAmount + dDeductionAmount
                If (diDeductions.ContainsKey(strCode)) Then
                    diDeductions.Remove(strCode)
                End If
            End If

            HttpContext.Current.Session("JSAnnuityDeductions") = diDeductions
            Return String.Format("{0}|{1}", dWithholdings.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture), dNetAmount.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture))
        Catch ex As Exception
            HelperFunctions.LogException("UpdateJSAnnuityDeduction", ex)
        Finally
            diDeductions = Nothing
        End Try
    End Function

    <System.Web.Services.WebMethod(True)> _
    Public Shared Function CancelUnSuppressJSAnnuity(ByVal strWithholdings As String, ByVal strNetAmount As String) As String
        Dim dWithholdings As Decimal
        Dim dNetAmount As Decimal
        Dim diDeductions As Dictionary(Of String, Decimal)
        Try
            diDeductions = TryCast(HttpContext.Current.Session("JSAnnuityDeductions"), Dictionary(Of String, Decimal))
            If diDeductions Is Nothing Then
                diDeductions = New Dictionary(Of String, Decimal)
            End If
            dWithholdings = Convert.ToDecimal(strWithholdings)
            dNetAmount = Convert.ToDecimal(strNetAmount)

            For Each strKey As String In diDeductions.Keys
                dWithholdings = dWithholdings - diDeductions(strKey)
                dNetAmount = dNetAmount + diDeductions(strKey)
            Next
            HttpContext.Current.Session("JSAnnuityDeductions") = Nothing
            Return String.Format("{0}|{1}", dWithholdings.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture), dNetAmount.ToString("#0.00;;0", System.Globalization.CultureInfo.InvariantCulture))
        Catch ex As Exception
            HelperFunctions.LogException("CancelUnSuppressJSAnnuity", ex)
        Finally
            diDeductions = Nothing
        End Try
    End Function

    Private Function ConvertDeductionsIntoXML(ByVal diDeductions As Dictionary(Of String, Decimal)) As String
        Dim mainElement As XElement
        Dim childElement As XElement
        Try
            mainElement = New XElement("root")
            For Each pair As KeyValuePair(Of String, Decimal) In diDeductions
                childElement = New XElement("deduction")
                childElement.SetAttributeValue("code", pair.Key)
                childElement.SetAttributeValue("amount", pair.Value)
                mainElement.Add(childElement)
            Next
            Return mainElement.ToString()
        Catch ex As Exception
            Throw ex
        Finally
            childElement = Nothing
            mainElement = Nothing
        End Try
    End Function

    Protected Sub btnUnSuppressAnnuitySave_Click(sender As Object, e As EventArgs) Handles btnUnSuppressAnnuitySave.Click
        Session("CurrentProcesstoConfirm") = "UnSuppressProcess"
        rfvGender.Enabled = False ' This condition was copied from old code 
        UNSuppresseJSannuities()
    End Sub
    'END: PPP | 04/20/2016 | YRS-AT-2719 - Applying Fees and deductions to death payments - Part A.2 

    ' // START : SB | 07/07/2016 | YRS-AT-2382 |  Creating Auditlog Table
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
    ' // END : SB | 07/07/2016 | YRS-AT-2382 |  Creating Auditlog Table

    ' // START : SB | 07/07/2016 | YRS-AT-2382 |  Preparing Common Aduit table for beneficiaries
    Private Sub PrepareAuditTable()
        Try
            Dim dtSSNUpdate As DataTable
            If Session("AuditBeneficiariesTable") Is Nothing Then
                Session("AuditBeneficiariesTable") = CreateAuditTable()
            End If
            dtSSNUpdate = Session("AuditBeneficiariesTable")
            CreateAnnuityBeneficiesForAuditTable(dtSSNUpdate)
            CreateBeneficiariesForAuditTable(dtSSNUpdate)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 |  Preparing Common Aduit table for beneficiaries 
    ' // START : SB | 07/07/2016 | YRS-AT-2382 |  Inserting Annuities in Audit Table
    Private Sub CreateAnnuityBeneficiesForAuditTable(ByVal dtSSNUpdate As DataTable)
        Dim dtExistingTable As DataTable
        Dim dtExistingDataSet As DataSet
        Dim drAuditRow As DataRow
        Try
            If (Session("AnnuityDetails") IsNot Nothing) Then
                dtExistingDataSet = CType(Session("AnnuityDetails"), DataSet)
                dtExistingTable = dtExistingDataSet.Tables(1)
                If dtExistingTable.Rows.Count > 0 Then
                    For Each drExistingRow As DataRow In dtExistingTable.Rows
                        If Not drExistingRow.RowState = DataRowState.Deleted AndAlso dtSSNUpdate.Select(String.Format("UniqueId='{0}'", Convert.ToString(drExistingRow("AnnuityJointSurvivorsID")))).Length = 0 Then
                            drAuditRow = dtSSNUpdate.NewRow()
                            drAuditRow("ModuleName") = "Retire Maintenance"
                            drAuditRow("UniqueId") = drExistingRow("AnnuityJointSurvivorsID")
                            drAuditRow("EntityType") = "AnnuityBeneficiary"
                            drAuditRow("chvColumn") = "chrSSNo"
                            drAuditRow("OldSSN") = drExistingRow("SSNo")
                            drAuditRow("NewSSN") = String.Empty
                            drAuditRow("Reason") = String.Empty
                            drAuditRow("IsEdited") = "False"
                            dtSSNUpdate.Rows.Add(drAuditRow)
                        End If
                    Next
                End If
            End If
            Session("AuditBeneficiariesTable") = dtSSNUpdate
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 |  Inserting Annuities in Audit Table

    ' // START : SB | 07/07/2016 | YRS-AT-2382 |  Inserting Beneficiaries in Audit Table
    Private Sub CreateBeneficiariesForAuditTable(ByVal dtSSNUpdate As DataTable)
        Dim dtExistingTable As DataTable
        Dim dtExistingDataSet As DataSet
        Dim drAuditRow As DataRow
        Try
            If (Session("BeneficiariesRetired") IsNot Nothing) Then
                dtExistingDataSet = CType(Session("BeneficiariesRetired"), DataSet)
                dtExistingTable = dtExistingDataSet.Tables(0)
                For Each drExistingRow As DataRow In dtExistingTable.Rows
                    If Not drExistingRow.RowState = DataRowState.Deleted AndAlso dtSSNUpdate.Select(String.Format("UniqueId='{0}'", Convert.ToString(drExistingRow("UniqueId")))).Length = 0 Then
                        drAuditRow = dtSSNUpdate.NewRow()
                        drAuditRow("ModuleName") = "Retire Maintenance"
                        drAuditRow("UniqueId") = drExistingRow("UniqueId")
                        drAuditRow("EntityType") = "Beneficiary"
                        drAuditRow("chvColumn") = "chrSSNo"
                        drAuditRow("OldSSN") = drExistingRow("TaxID")
                        drAuditRow("NewSSN") = String.Empty
                        drAuditRow("Reason") = String.Empty
                        drAuditRow("IsEdited") = "False"
                        dtSSNUpdate.Rows.Add(drAuditRow)
                    End If
                Next
            End If
            Session("AuditBeneficiariesTable") = dtSSNUpdate
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' // END : SB | 07/07/2016 | YRS-AT-2382 |  Inserting Beneficiaries in Audit Table

    ' // START : SB | 07/07/2016 | YRS-AT-2382 |  To check Beneficiary Human or not
    Private Function IsRelationshipTypeIsHumanBenaficiary(ByVal dsRelationShips As DataSet, ByVal drRelationship As String) As Boolean
        Dim drFoundRows As DataRow()
        Dim isHumanBeneficiary As Boolean
        Try
            isHumanBeneficiary = True
            If (drRelationship IsNot Nothing) Then
                drFoundRows = dsRelationShips.Tables(0).Select(String.Format("CodeValue='{0}'", drRelationship))
                If drFoundRows.Length > 0 AndAlso Convert.ToString(drFoundRows(0)("Category")).Trim = EnumBeneficiaryTypes.NHBENE.ToString() Then
                    isHumanBeneficiary = False
                End If
            End If
            Return isHumanBeneficiary
        Catch
            Throw
        End Try
    End Function
    ' // END : SB | 07/07/2016 | YRS-AT-2382 |  To check Beneficiary Human or not

    'START: JT | 2018.08.28 | YRS-AT-4031 | Fetching HIDE_WEB_ACCOUNT_PRINT value and storing into property
    Public Property IsPrintButtonToBeHidden() As Boolean
        Get
            If Not ViewState("IsPrintButtonToBeHidden") Is Nothing Then
                Return CType(ViewState("IsPrintButtonToBeHidden"), Boolean)
            Else
                Return True
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsPrintButtonToBeHidden") = Value
        End Set
    End Property

    ' To hide print button
    Public Function GetWebAccountPrintConfigValue()
        Dim metaConfigurationKey As DataSet = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("HIDE_WEB_ACCOUNT_PRINT")
        Me.IsPrintButtonToBeHidden = True
        If HelperFunctions.isNonEmpty(metaConfigurationKey) AndAlso HelperFunctions.isNonEmpty(metaConfigurationKey.Tables(0)) Then
            Me.IsPrintButtonToBeHidden = CType(metaConfigurationKey.Tables(0).Rows(0)("Value").ToString(), Boolean)
        End If
    End Function
    'END: JT | 2018.08.28 | YRS-AT-4031 | Fetching HIDE_WEB_ACCOUNT_PRINT value and storing into property

    'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
    ''START: SB | 2017.08.03 | YRS-AT-3324 | Clearing session variables 
    'Public Sub ClearSession()
    '    Session("ReasonForRestriction") = Nothing
    '    Session("DeathNotificationAllowedForRMDEligibleParticipants") = Nothing
    'End Sub
    ''END: SB | 2017.08.03 | YRS-AT-3324 | Clearing session variables 
    'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 

    'START : BD : 2018.11.20 : YRS-AT-4136 : Method to show message in Div and Method to check insured reserve annuity exists or not
    Private Sub ShowMessage(stMessage As String)
        DivMainMessage.InnerHtml = IIf(String.IsNullOrEmpty(DivMainMessage.InnerHtml.Trim), "", DivMainMessage.InnerHtml + "</br>")
        DivMainMessage.InnerHtml = DivMainMessage.InnerHtml + stMessage
        DivMainMessage.Visible = True
    End Sub

    ''' <summary>
    ''' Validate insured reserve and beneficiary type and display message
    ''' - If annuity Type is C only INSRES is allowed
    ''' - If annuity type is not C then Retiree/ INSRES are not allowed
    ''' </summary>
    ''' <param name="dsBenefeciaries"></param>
    ''' <remarks></remarks>
    Private Sub DisplayMessageBasedOnInsuredReserveAnnuityAndBeneficiaryType(dsBenefeciaries As DataSet)
        If (YMCARET.YmcaBusinessObject.RetirementBOClass.HasInsuredReserveAnnuity(CType(Session("AnnuityDetails"), DataSet))) Then
            If dsBenefeciaries.Tables(0).Select("BeneficiaryTypeCode = 'RETIRE'").Length > 0 Then
                ShowMessage(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PART_MAINT_RETIREE_NOT_ALLOWED, YMCARET.YmcaBusinessObject.RetirementBOClass.GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText)
            End If
        Else
            If dsBenefeciaries.Tables(0).Select("BeneficiaryTypeCode IN ('RETIRE','INSRES')").Length > 0 Then
                ShowMessage(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PART_MAINT_RETIREE_INSRES_NOT_ALLOWED, YMCARET.YmcaBusinessObject.RetirementBOClass.GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText)
            End If
        End If
    End Sub
    'END : BD : 2018.11.20 : YRS-AT-4136 : Method to show message in Div and Method to check insured reserve annuity exists or not
    Private Sub ShowErrorMessage(stMessage As String)
        divErrorMsg.InnerHtml = IIf(String.IsNullOrEmpty(divErrorMsg.InnerHtml.Trim), "", divErrorMsg.InnerHtml + "</br>")
        divErrorMsg.InnerHtml = divErrorMsg.InnerHtml + stMessage
        divErrorMsg.Visible = True
    End Sub
    'START: Shilpa N | 03/05/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim toolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            hdnUnSuppress.Value = toolTip
            For Each row In DataGridNotes.Items
                Dim lnkbtndelete As LinkButton = (TryCast((TryCast(row, TableRow)).Cells(6).Controls(1), LinkButton))
                Dim chkmarkimp As CheckBox = (TryCast((TryCast(row, TableRow)).Cells(5).Controls(1), CheckBox))
                lnkbtndelete.Enabled = False
                lnkbtndelete.ToolTip = toolTip
                chkmarkimp.Enabled = False
                chkmarkimp.ToolTip = toolTip

            Next
        End If
        'END: Shilpa N | 03/05/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    End Sub
    'START : ML |10.24.2019| YRS-AT-4598 | Assign GrossAmount And FederalAmount to State Withholding Control
    Public Sub SetGrossAmountToSTWUserControl()
        Dim totalWithholdAmount As Double
        Dim totalAnnuityAmount As Double
        Dim totalFederalAmount As Double
        Dim dsGenWithdrawals As DataSet
        Dim dsAnnuityDetails As DataSet
        Dim dsFedWithdrawals As DataSet
        Try
            If (Not Session("GenWithDrawals") Is Nothing) Then
                dsGenWithdrawals = Session("GenWithDrawals")
            Else
                dsGenWithdrawals = Nothing
            End If
            If (Not Session("AnnuityDetails") Is Nothing) Then
                dsAnnuityDetails = Session("AnnuityDetails")
            Else
                dsAnnuityDetails = Nothing
            End If
            If (Not Session("FedWithDrawals") Is Nothing) Then
                dsFedWithdrawals = Session("FedWithDrawals")
            Else
                dsFedWithdrawals = Nothing
            End If

            If (HelperFunctions.isNonEmpty(dsAnnuityDetails)) Then
                totalAnnuityAmount = dsAnnuityDetails.Tables(0).AsEnumerable().Sum(Function(x) Convert.ToDouble(x("Current Payment")))
                Session("AnnuityAmount") = totalAnnuityAmount
                If (Me.TextBoxGeneralRetireDate.Text <> String.Empty) Then
                    Dim dtretireedate As DateTime = Convert.ToDateTime(Me.TextBoxGeneralRetireDate.Text)
                    totalWithholdAmount = YMCARET.YmcaBusinessObject.RetirementBOClass.GetTotalWithHoldingAmount(dsFedWithdrawals, dsGenWithdrawals, 0, totalAnnuityAmount, Me.TextBoxGeneralRetireDate.Text)
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
        End Try
    End Sub
    ' State Tax Withholding MA validation 
    Public Sub ValidateSTWvsFedtaxforMA()
        Dim message As String
        Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
        If (Not SessionManager.SessionStateWithholding.LstSWHPerssDetail Is Nothing) Then
            If (SessionManager.SessionStateWithholding.LstSWHPerssDetail.Count > 0) Then
                LstSWHPerssDetail = SessionManager.SessionStateWithholding.LstSWHPerssDetail
                If (Not YMCARET.YmcaBusinessObject.StateWithholdingBO.ValidateFedTaxVSStateTaxInputDetailForMA(LstSWHPerssDetail.FirstOrDefault, Me.stwFederalAmount, Me.stwFederalType, message)) Then
                    ShowErrorMessage(message)
                    ButtonSaveRetireeParticipants.Enabled = False
                    Exit Sub
                Else
                    ButtonSaveRetireeParticipants.Enabled = True
                End If
            End If
        End If
    End Sub
    'END : ML |10.24.2019|YRS-AT-4598 | Assign GrossAmount And FederalAmount to State Withholding Control
End Class

