#Region "Modification History"
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	ParticipantsInformation.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@3i-infotech.com
' Contact No		:	8751
' Creation Time		:	6/13/2005 12:40:52 PM
' Program Specification Name	:	Activities > Maintenance > Find Person (Form Name: “frmdemem.scx") 
' Unit Test Plan Name			:	
' Description					:	Participants Information 
'
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Vartika Jain       13-Jul-2005     Adding the names of buttons in tabs and coding their click event.
'Ruchi Saxena        6-Aug-2005     Coding
'Vartika Jain       15-Nov-2005     Bug Fixing
'Rahul Nasa         14-Jan-2006     Validation on First and Last Name.
'Vipul Patel        03-Feb-2006     Removal of Cache and replacing it with Session
'Preeti             07-Feb-2006     IssueId:2053 Reason:Message Box text change
'Preeti             08-Feb-2006     IssueId:2060 Reason:Equal function does not work on Beneficiary Contingent Level 2
'Preeti             08-Feb-2006     Changes on HTML<!-- Issue :YRST-2052 Reason:Name of Creater of Note does not display upon Save. Reduced Width to 550->
'Preeti             10-Feb-2006     Issue :YMCA-1858 
'Rahul Nasa         22-Feb-2006     Ymca no. is not displayed in DatagridAdditionalAccounts
'Shubhrata          29-Sep-2006     YREN-2703 to check whether a participant is vested or not
'Shubhrata          30-Oct-2006     YREN-2779 if country code is CA then allow alphanumeric char in Zip,if US then allow only numeric for Zip 
'Mohammed Hafiz     22-Nov-2006     YREN-2882
'Shubhrata          29-Nov-2006     the Sec Country was not getting picked up from database.
'Shubhrata          28-Dec-2006     YREN-3012 Multiple issues with the Loans Tab 
'Shubhrata          08-Jan-2007     Changes in PayOFF amount as stated in Ragesh' mail dated Jan 6th 2007 WE need 2 more fields Saved PayOff amount and Computed on 
'Aparna Samala      12-Jan-2007     YREN-3026
'Ashutosh Patil     31-Jan-2007     YREN-3028,YREN-3029
'Ashutosh Patil     12-Feb-2007     YREN-3028,YREN-3029
'Ashutosh Patil     15-Feb-2007     YREN-3028,YREN-3029 
'Ashutosh Patil     23-Feb-2007     YREN-3028,YREN-3029 YREN -3079
'Nikunj Patel       27-Feb-2007     Found &nbsp; in textboxes on the Add Beneficiary page when going in to edit an existing Beneficiary when string.empty was expected
'Ashutosh Patil     01-Mar-2007     Changing spellings and Address1 validations and City Validations
'Ashutosh Patil     06-Mar-2007     YREN-3099
'Shubhrata          06-Mar-2007     The YMCA nO for which the loan is deducted should be displayed on LoansTab 
'Aparna Samala      22-Mar-2007     YREN-3115
'Ashutosh Patil     30-Mar-2007     YREN-3203,YREN-3205
'Ashutosh Patil     25-Apr-2007     YREN-3298
'Shubhrata          11-May-2007     Plan Split Changes
'Ashutosh Patil     06-Jun-2007     YREN-3490
'Ashutosh Patil     21-Jun-2007     YREN-3490 ==> SSNo Duplication Message Box
'Nikunj Patel       27-Jun-2007     Updated code to handle different Beneficiary types for Plan split. 
'                                   Removed code for Retired beneficiaries
'                                   Refactored code for efficiency and performance
'Ashutosh Patil     09-Jul-2007     For Avoiding runtime error if after deletion of record of datagrid of Beneficiaries
'                                   edit item button is directly clicked    
'Nikunj Patel       07-Aug-2007     Save and Cancel buttons were enabled even if no beneficiaries were selected for editing or deleting
'                                   The selected beneficiary is deselected when the Cancel button is pressed.
'Aparna Samala      29-Aug-2007     Showing Account conrtibutions based on the plan type
'Aparna Samala      12-Sep-2007     TO make the death notification process work similiar to that in the retirees page
'Aparna Samala      04-Oct-2007     Aligning the datagrids in the account contribution tab
'Nikunj Patel       15-Oct-2007     Hiding the buttons to equate Savings plan percentages for beneficary. Issue YSPS-3905
'Aparna Samala      19-Nov-2007      YRPS-4051 
'Swopna Valappil    27-Dec-2007     Problem with salutation pick up from database and matching the same with dropdownlist values solved.
'Swopna Valappil    08-Jan-2008     On selecting a value from datagridparticipantnotes,correct note is displayed (Code added/commented in response to YREN-4126)
'Swopna Valappil    11-Jan-2008     ButtonSavePayoffAmount enabled on page load. 
'Swopna Valappil    16-Jan-2008     YREN-4126 reopened.Notes in ascending order of date.
'Swopna Valappil    08-Feb-2008     YRPS-4614(Problem-Complete note could not be viewed)
'Shubhrata          12-Feb-2008     YRPS-4614
'Swopna             08-Apr-2008     Phase IV changes 
'Mohammed Hafiz     28-Mar-2008     YRPS-4558 - Fixed Part-2 of the gemini issue. 
' & Shubhrata
'Mohammed Hafiz     27-Mar-2008     YRPS-4704 - Should not Allow the Beneficiary code of SP if person is single
'Swopna Valappil    19-May-2008     BT-394
'Swopna Valappil    20-May-2008     Include 'RPT',BT-403,BT-434(ButtonActivateAsPrimary's causesvalidation property set to true, change made in .aspx file)
'Swopna Valappil    21-May-2008     YRPS-4704
'Swopna Valappil    27-May-2008     Additional Phase IV Part I Requirement
'Priya              01-Jun-2008     YRS 5.0-429 showing total percentage of account breakdown grid
'Swopna Valappil    09-Jun-2008     ButtonTerminateParticipation disable.
'Nikunj Patel       09-Jul-2008     YRS-5.0-464 Code changed to prompt for NP Participants for their death status. 
'                                   Also prompt for NP and ML will only be displayed if basic account balance is < 10,000
'Nikunj Patel       10-Sep-2008     BT-544 - Beneficiary balances not being displayed properly. MD account was not being included in the list and the formatting for -ve amounts was incorrect.
'                                   BT-550 - Updating code to store the SSN number in Person Info session variable that is used on the UpdateBeneficiary page.
'Priya              06-Oct-2008     YRS 5.0-424; YRS 5.0-478 Commented if condition
'Nikunj Patel       17-Oct-2008     BT-631 - Delete of beneficiary was not being allowed. It was firing a validation on another screen that prevented the delete.
'Priya              14-Nov-2008     Change IDM path as per Ragesh mail as on 11/13/2008
'Mohammed Hafiz     19-Dec-2008     Correcting the IDM path - fundid was repeatedly concatenated.
'Dilip Yadav        2009.09.08      YRS 5.0-852
'Dilip Yadav        2009.09.09      YRS 5.0-852 ( Removed hard code for gender type )
'Dilip Yadav        2009.09.21      To modify the code which allows only 50 characters in Notes grid while in View mode it will show the complete text.
'Dilip Yadav        2009.10.28      To provide the feature of "Priority Handling" as per YRS 5.0.921
'Dilip Yadav        2009.11.11      To enable 4 fields Title,Professional,Exempt,FullTime in employement Tab while update as per YRS 5.0.941
'Neeraj Singh       12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Neeraj Singh       13/Nov/2009     issue YRS 5.0-940 made changes in  event ButtonSaveParticipants_Click
'Neeraj Singh       13/Nov/2009     issue YRS 5.0-940 made changes in ButtonSaveParticipant_Click
'Sanjay Rawat       2009.12.16      YRS 5.0:865  
'Sanjay Rawat       2010.1.04       YRS 5.0:635  
'Shashi Shekhar      23-dec-2009     Change code for adding validation Summary and resolve conflict between clint side and server side call for check security
'Shashi Shekhar:    2010-01-27: Added function RetrieveData() To update the bitIsArchived field in atsPerss table
'Priya              18-Feb-2010     YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
'Ashish Srivastava  2010.03.12      Thread being aborted error, if there is no active beneficiary
'Priya              24-March-2010   BT:479: While adding QDRO Request page not getting Refresh.
'Shashi Shekhar     26-March-2010   BT:488: Confirmation message needs to change in Person Maintenance screen.
'Priya			    5-April-2010	    YRS 5.0-1042 : fetch value of newly aaded column bitYmcaMailOptOut
'Shashi Shekhar	    2010.04.07	    Handling DataArchive error message (concate sql error message with user defined error message returned from procedure)
'Shashi Shekhar     2010-04-12      Ref:mail-Issues identified with 7.4.2 code release - Internally identified #1 -- Remove ref variable as output ,used in  RetrieveData function
'Priya/Ashish       21-April-2010   YRS 5.0-1056: Additional info when unsuppressing a JSANN annuity
'Sanjay R.         20-April-2010    YRS 5.0-1055 - for Null value assigning Gendertype as UNKNOWN
'Priya              29-April-2010   BT-521:Css is not applied on beneficiary tab for member and status label
'Sanjay Rawat       2010.05.10      YRS 5.0-1078 - To add beneficiary info for participants who have status of QD. 
'Priya              12-May-2010     YRS 5.0-1073 :commented  ShareInfoAllowed code
'Imran              03-June-2010    Changes for Enhancements -8
'Imran              08-June-2010    BT:532 :Updating Gender Code as "U" in database.
'Priya              19-July-2010    Integrate changes made on 10-June-2010 for YRS 5.0-1104:Showing non-vested person as vested
'Ashish Srivastava  2010.07.21      YRS 5.0-1136,handle show/hide Retiree information link for those prticipant which having multiple fundevents
'Imran              30-July-2010    YRS 5.0-1141 add Age format to the person maintanace screen.
'Imran              03-July-2010    BT-865 Age shows wrong after Deceased date entry.
'Priya              18-Aug-2010     YRS 5.0-1141:Please add Age to the person maintanace screen.
'Imran              23-Aug-2010     BT-568 Age shows wrong for exact one year.
'Priya              06-Sep-2010:    BT-599,YRS 5.0-1126:Address update for non-US/non-Canadian address.
'Shashi Shekhar     28-Oct-2010     For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
'Shashi Shekhar     14-Dec-2010     For BT-700 Unable to change Hire and enrollment date for Retired Active participant And   ExceptionPolicy.HandleException(ex, "Exception Policy") within catch block.
'Priya				10-Dec-2010:	YRS 5.0-1177/BT 588 Changes made as sate n country fill up with javascript in user control
'Shashi Shekhar     09- dec-2011    For YRS 5.0-1236 : Need ability to freeze/lock account
'Shashi Shekhar     18 Feb 2011     ForBT-670,YRS 5.0-1202 : hyperlink from employment record to ymca maintenance
'Shashi Shgekhar    11 Apr 2011     for YRS 5.0-1280 : ability to sort records in grid on employment tab
'Sanjay R.		    2011.04.26      YRS 5.0-1292: A warning msg should appear for the participant whose annuity disursement after moth of death is not voided									
'Shashi Shekhar     26 Apr 2011     for BT-824 :Additional Accounts details grid sorting not working.
'bhavna S           8 july 2011     YRS 5.0-1354 : change caption of bool 'YMCA Mailing Opt-out' into 'Personal Info Sharing Opt-out'
'bhavna S           11 july 2011    YRS 5.0-1354 : new bit field/checkbox added to Person Maintenance,field name 'Go Paperless' with bitGoPaperless
'bhavna S           18 july 2011    YRS 5.0-1339 : handle X value on MaritalStatus Dropdown when record is non actuary on loadGeneraltab()
'bhavna S           19 july 2011    Reopen POA Issue show all active poa based on termination date > than current date effective date < than current date, termination date is null ,l_DataSet_POA
'bhavna S           19 july 2011    YRS 5.0-1339:handle X value on gender Dropdown
'shagufta           20 July 2011    For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance
'Shagufta Chaudhari 20 July 2011    For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance
'Bhavna	Shrivastava	22 July 2011	BT-706 Optimized code by removing duplication of code.  Handled BT-902 as well.
'Shagufta Chaudhari 27 July 2011    For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance(Disable 'Save' and 'Cancel' button)
'Bhavna Shrivastava 2011.08.01      For BT-911: save and cancel button should be diasble while poa entry bcoz poa entry updated on poa screen no need to 
'Bhavna Shrivastava 2011.08.01      Revert :YRS 5.0-1339
'Prasad Jadhav      2011.08.26      For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
'Prasad Jadhav      2011.09.12      For BT-895,YRS 5.0-1364 : prompt user to if changes not saved 
'BhavnS             2012.01.18      YRS 5.0-1497 -Add edit button to modify the date of death
'prasad jadhav		2012.01.19		For BT-925,YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
'prasad jadhav		2012.01.24		For BT-950,YRS 5.0-1469: Add link to Web Front End
'prasad jadhav		2012.01.31		For BT-950,YRS 5.0-1469: Add link to Web Front End
'BhavnS             2012.02.02      YRS 5.0-1497 -Add edit button to modify the date of death
'Shashank Patel     2012.02.07      Made changes to rename contribution type monthly to Dollar
'BhavnS             2012.02.18      For BT-941,YRS 5.0-1432:New report of checks issued after date of death
'prasad jadhav		2012.02.17		BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
'BhavnS             2012.03.03      For BT-941,YRS 5.0-1432:New report of checks issued after date of death
'BhavnS             2012.03.13		BT:993:-Deceased Date field getting enabled after updating the Deceased Date
'BhavnS             2012.03.21     For BT-1015,YRS 5.0-1557: Death Notification Button disappears 
'Prasad jadhav		2012.03.12		For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
'Prasad jadhav		2012.04.10		For:BT:1018:YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
'prasad jadhav		2012.04.26		For BT-991,YRS 5.0-1530 - Need ability to reactivate a terminated employee
'Priya Patil        2012.05.08      BT:1031 New change for Gemini ID YRS 5.0-1561
'Priya				26-May-2012		YRS 5.0-1576: update marital status if spouse beneficiary entered
'BhavnaS			2012.06.20		BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
'BhavnaS			2012.06.21		BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
'BhavnaS			2012.06.26		BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
'BhavnaS			2012.04.23     For BT-951,YRS 5.0-1470: Link to Address Edit program from Person Maintenance & address code Restructure
'BhavnaS			2012.07.18     For BT-951,YRS 5.0-1470: Link to Address Edit program from Person Maintenance & address code Restructure(removed selectedindex tab)
'Sanjay R           2012.08.17     BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program
'Priya Patil		2012.10.18		Make termiantion watcher button visible false 
'Anudeep            2012.10.23      Reverted changes as per observations listed in bug tracker for yrs.5.0-1484
'Anudeep            2012.11.02      BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen 
'Anudeep            2012.11.06     Changes Made as Per Observations listed in bugtraker For yrs 5.0-1484 on 06-nov 2012
'Anudeep            2012.11.14      Reverted changes made for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen 
'Anudeep            2012.11.28      Added for Bt-1026/YRS 5.0-1629 : Web frontend letters to IDM.
'Anudeep            2012.12.04      Changed Code to show the termination watcher button who have plan balances
'Anudeep            2012.12.04      Bt-1026/YRS 5.0-1629:Code changes to copy report into IDM folder
'Dinesh K           2012.12.05      Bt-1454/YRS 5.0-1731:It is creating blank records in atstelephones table.  
'Dinesh K           2012.12.17      BT-1409/Miscellaneous Issues
'Dinesh K           2012.12.12      BT-1236/YRS 5.0-1685:Add Category/Type field to Power of attorney and allow 3 types
'Dinesh K           2012.12.27      BT-1266/YRS 5.0-1698:Cross check SSN when entering a date of death             
'Anudeep            2013.01.29      YRS 5.0-1852:Error with account balance display
'Dinesh k           2013.02.04      BT-1649 changes merge for 12.9.2 patch.
'Dinesh K           2013.02.12      BT-1261/YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
'Anudeep            14.03.2013      Changed Code for telephone Usercontrol
'Sanjay R.          2013.06.10      BT-2068/YRS 5.0-2121 - Changed doccode from 07DTHBENAP to DTHBENAP for Deathbenefitapplicationform
'Anudeep            2013.06.21      BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death 
'Anudeep            2013.06.21      BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            2013.06.24      Changed for not to occur error when beneficiaries not defined
'Anudeep            2013.07.02      Bt-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Sanjay R.          2013.08.05      YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Anudeep            2013.08.21      YRS 5.0-2162 :  Edit button for email address 
'Anudeep            2013.08.22      YRS 5.0-1862:Add notes record when user enters address in any module.
'Anudeep            2013.08.23      BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            26.08.2013      YRS 5.0-2162:Edit button for email address 
'Anudeep            2013.08.27      YRS 5.0-2070 : Change to check if not pending records
'Anudeep            2013.08.27      BT-1555:YRS 5.0-1769:Length of phone numbers
'DInesh.k           2013.09.17      BT-2139:YRS 5.0-2165:RMD enhancements. 
'Anudeep            2013.10.04      BT-2236:After modifying address save button gets disabled
'Anudeep            2013.10.21      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Anudeep            2013.10.23      BT-2264:YRS 5.0-2232:Notes entry in person maintanance screen.
'Anudeep            2013.11.06      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Anudeep            2013.11.07      BT:2190-YRS 5.0-2199:Create view mode for Power of Attorney display 
'Anudeep            2013.11.07      BT:2269-YRS 5.0-2234:Change labelling of Power of Attorney / POW 
'Anudeep            2013.11.15      BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
'Sanjay R            2013.11.19      BT-2139:YRS 5.0-2165:RMD enhancements. 
'Shashank			2013.12.19		BT:2131/YRS 5.0-2161 : Modifications to Annuity Purchase process 
'Anudeep A          2014.02.03      BT:2292:YRS 5.0-2248 - YRS Pin number 
'Shashank			2014.02.04		BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN 	
'Anudeep A          2014.02.13      BT:2316:YRS 5.0-2262 - Spousal Consent date in YRS
'Anudeep A          2014.02.14      BT:2292:YRS 5.0-2248 - YRS Pin number 
'Anudeep A          2014.02.16      BT:2291:YRS 5.0-2247 - Need bitflag that will not allow a participant to create a web account 
'Dinesh Kanojia     2014.02.18      BT-2139: YRS 5.0-2165:RMD enhancements 
'Anudeep A          2014.02.18      BT:2316:YRS 5.0-2262 - Spousal Consent date in YRS
'Anudeep A          2014.02.20      BT:2316:YRS 5.0-2262 - Spousal Consent date in YRS
'Shashank P         2014.02.24      BT:2316:YRS 5.0-2262 - Spousal Consent date in YRS
'Anudeep            25.03.2014      BT:957: YRS 5.0-1484 -Termination watcher changes Re-Work (YRS 5.0-1484)
'Anudeep            26.03.2014      BT:2489: YRS 5.0-2349 - Error message while updating address. 
'Anudeep            01.04.2014      BT:2464:YRS 5.0-1484: Termination watcher changes(Re-Work).
'Anudeep            01.04.2014      BT:2489: YRS 5.0-2349 - Error message while updating address. 
'Shashank           2014.04.29      BT-2525 : Handle multiple records updated for RMDTaxRate
'Dinesh k           2014.05.15      BT-2537 : YRS 5.0-2370 - Change to validation on RMD tax rate 
'Anudeep A          2014.08.13      BT-1409 : Miscellaneous Issues for Security Rights
'shashank           2014.08.17      BT-2623:YRS 5.0-2399 -  change to spousal consent enforcement
'Anudeep A          2014.09.03      BT-1409 : Miscellaneous Issues for Security Rights
'Shashank P         2014.11.20      BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
'Shashank P         2014.12.18      BT-2734\YRS 5.0-2454:cannot lookup participant in YRS
'Anudeep A          2015.11.03      BT-2699:YRS 5.0-2441 : Modifications for 403b Loans 
'Anudeep            2015.05.05      BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite
'Anudeep            2015.13.05      BT:2680 : YRS 5.0-2428:Enhancement-for deceased participants, do not require spousal consent or cannot locate spouse
'Anudeep            2015.26.06      BT:2906 : YRS 5.0-2547: YRS issue-cannot access participant account (when contingent Beneficiary exists)
'Sanjay S.          2015.07.13      BT-2699:YRS 5.0-2441 : Modifications for 403b Loans 
'Anudeep A.         2015.09.15      YRS AT-2441 : Modifications for 403b Loans 
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale   2015.10.10      YRS-AT-2588: implement some basic telephone number validation Rules
'Manthan Rajguru    2015.10.21      YRS-AT-2182: limit effective date for address updates -Do not allow address updates with an effective date in the future. 
'Bala               2016.01.05      YRS-AT-1972: Special death processing required.
'Bala               2016.01.12      YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Bala               2016.01.19      YRS-AT-2398  Customer Service requires a Special Handling alert for officers.s
'Anudeep            2016.02.15      YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Manthan Rajguru    2016.02.24      YRS-AT-2328 - Deactivate secondary address
'Bala               2016.03.08      YRS-AT-1718 - Adding Notes - YMCA Maintenance
'Bala               2016.03.16      YRS-AT-2398  Customer Service requires a Special Handling alert for officers.
'Manthan Rajguru    2016.04.05      YRS-AT-1718 -  Adding Notes - YMCA Maintenance
'Manthan Rajguru    2016.04.06      YRS-AT-2328 - Deactivate secondary address
'Santosh Bura       2016.07.07      YRS-AT-2382 -  YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annty bene SSN(TrackIT 19856)
'Manthan Rajguru    2016.07.18      YRS-AT-2919 -  YRS Enh: Beneficiary Details - nonPerson - should allow optional Date
'Manthan Rajguru    2016.07.27      YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Santosh Bura 		2016.09.21		YRS-AT-3028 -  YRS enh-RMD Utility - married to married Spouse beneficiary adjustment(TrackIT 25233)
'Manthan Rajguru    2016.10.07      YRS-AT-3062 -  YRS enh-Limit permissions for Exhausted RMD/DB Efforts box (TrackIT 26963)
'Manthan Rajguru    2016.10.14      YRS-AT-3063 -  YRS enh-generate warning for Exhausted DB/RMD Settlement Efforts (TrackIT 26868)
'Santosh Bura 		2016.10.13		YRS-AT-3095 -  YRS enh-allow regenerate RMD for deceased participants (TrackIT 27024) 
'Santosh Bura		2017.03.23 		YRS-AT-2606 -  YRS bug- modification of enrollment date should also update participation date (TrackIT 23896)
'Manthan rajguru    2017.04.18      YRS-AT-3384 - YRS Bug - Address changes reArch: Insert into atsAddrs and reset all prior record bit flags to zero 
'                                   YRS-AT-3385 -  YRS Bug - Address changes reArch: remove EffectiveDate from comparison code 
'Pramod P. Pokale   2017.06.01      YRS-AT-3460 -  YRS Enh: Display Warning Message before Recalculating Loan Payoff 
'Santosh Bura       2017.07.28      YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'Manthan Rajguru 	2017.12.04		YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
'Santosh Bura       2018.01.11      YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
'Manthan Rajguru    2018.04.06      YRS-AT-3935 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for Participant Information "Loans" tab, "Loan Details" section (TrackIT 33024)
'Manthan Rajguru    2018.05.04      YRS-AT-3941 - YRS enh: change "phony SSN" beneficiary label to "placeholder SSN" (TrackIT 33287)
'Manthan Rajguru    2018.05.22      YRS-AT-3716 -  YRS bug: cannot add address for non-person beneficiary (ex:trust) without TIN (TrackIT 31562)
'Santosh Bura		2018.06.21 		YRS-AT-3858 -  YRS bug: validation for RMD flag (Requested Annual MRD checkbox
'Sanjay GS Rawat	2018.07.12 		YRS-AT-3858 -  YRS bug: validation for RMD flag (Requested Annual MRD checkbox
'Vinayan C          2018.07.20      YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'Vinayan C          2018.08.01      YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab   
'Jayaram            2018.08.28      YRS-AT-4031 -  YRS enh: YRS enhancement-Hide Print button for web registration letter (TrackIT 33587)
'Manthan Rajguru    2018.11.23      YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab
'Pooja k            2019.01.10      YRS-AT-4241 -  YRS enh: EFT Loans Maint.:Person Maintenance screen, Loans tab, show both old and new marital statuses. (Track it 36374) 
'Megha Lad          2019.01.17      YRS-AT-3157 -  YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)
'Shilpa N           2019.03.05      YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'Shiny C.           2020.04.20      YRS-AT-4853 - COVID - YRS changes for Loan Limit Enhancement, due to CARE Act (Track IT - 41693) 
'*******************************************************************************************************************************************
#End Region

Imports System.Math
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Text.RegularExpressions
Imports YMCAUI.ServiceManager
Imports System.Xml.Serialization
Imports System.Net
Imports System.IO
'START: VC | 2018.08.20 | YRS-AT-4018 - Importing MetaMessageBo class
Imports YMCARET.YmcaBusinessObject.MetaMessageBO
Imports YMCAObjects.MetaMessageList
'END: VC | 2018.08.20 | YRS-AT-4018 -  Importing MetaMessageBo class

Public Class ParticipantsInformation
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("FindInfo.aspx?Name=Person")
    'End issue id YRS 5.0-940

#Region "Global Declaration"
    Const mConst_DG_AcctType_Index As Integer = 0
    '  Const mConst_DGIndexOfIsBasicAcct As Integer = 1
    Const mConst_DG_Taxable_Index As Integer = 1
    Const mConst_DG_NonTaxable_Index As Integer = 2
    Const mConst_DG_Interest_Index As Integer = 3
    Const mConst_DG_EmpTotal_Index As Integer = 4
    Const mConst_DG_YmcaTaxable_Index As Integer = 5
    Const mConst_DG_YmcaInterest_Index As Integer = 6
    Const mConst_DG_YmcaTotal_Index As Integer = 7
    Const mConst_DG_Total_Index As Integer = 8
    Const mConst_DG_PlanType_Index As Integer = 9
    ' Const mConst_DataGridFundedAcctContributionsIndexOfAcctGroup As Integer = 11
    ' // START : SB | 07/07/2016 | YRS-AT-2382 |   Constant variables for Beneficiaries update
    Const BENEFICIARY_UniqueID As Integer = 2
    Const BENEFICIARY_TaxID As Integer = 8
    Const BENEFICIARY_REL As Integer = 9
    Const BENEFICIARY_New As Integer = 20
    Const BENEFICIARY_IsExistingAudit As Integer = 25
    ' // END : SB | 07/07/2016 | YRS-AT-2382 |   Constant variables for Beneficiaries update
    'START: MMR | 2018.04.06 | YRS-AT-3935 | Defining local variable for cell index in datagrid for loans tab
    Const mConst_DataGridLoansIndexOfPersonId As Integer = 1
    Const mConst_DataGridLoansIndexOfEmpEventId As Integer = 2
    Const mConst_DataGridLoansIndexOfYMCAId As Integer = 3
    Const mConst_DataGridLoansIndexOfLoanNumber As Integer = 4
    Const mConst_DataGridLoansIndexOfTDBalance As Integer = 5
    Const mConst_DataGridLoansIndexOfRequestedAmount As Integer = 6
    Const mConst_DataGridLoansIndexOfPaymentMethodCode As Integer = 7
    Const mConst_DataGridLoansIndexOfRequestDate As Integer = 8
    Const mConst_DataGridLoansIndexOfRequestStatus As Integer = 9
    'START: VC | 2018.08.01 | YRS-AT-4018 | Commented existing code and added code to store Loans grid column indexes
    Const mConst_DataGridLoansIndexOfApplication As Integer = 10
    Const mConst_DataGridLoansIndexOfLoanRequestId As Integer = 11
    Const mConst_DataGridLoansIndexOfOriginalLoanNumber As Integer = 12
    Const mConst_DataGridLoansIndexOfPersBankingEFTId As Integer = 13
    Const mConst_gvPaymentMethodHistoryIndexOfAttemptedDate As Integer = 6 'VC | 2018.10.11 | YRS-AT-4018 | Setting index of Attempted Date into variable
    'END: VC | 2018.08.01 | YRS-AT-4018 | Commented existing code and added code to store Loans grid column indexes
    'END: MMR | 2018.04.06 | YRS-AT-3935 | Defining local variable for cell index in datagrid for loans tab
#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents DataGridLoans As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TabStripParticipantsInformation As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageParticipantsInformation As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSal As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListSal As System.Web.UI.WebControls.DropDownList
    '    Protected WithEvents LinkButtonIDM As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LinkButtonIDM As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IframeIDMView As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents LinkRefereshIDM As System.Web.UI.WebControls.ImageButton
    Protected WithEvents TextboxIFrame As System.Web.UI.WebControls.TextBox
    ''Protected WithEvents TextBoxSecState As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFirst As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirst As System.Web.UI.WebControls.TextBox
    'BS:2012.03.09:-restructure Address Control
    'Protected WithEvents TextBoxEffDate As YMCAUI.DateUserControl
    Protected WithEvents LabelMiddle As System.Web.UI.WebControls.Label
    Protected WithEvents lblPoaDetails As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMiddle As System.Web.UI.WebControls.TextBox
    'BS:2012.03.09:-restructure Address Control
    'Protected WithEvents TextBoxSecEffDate As YMCAUI.DateUserControl
    Protected WithEvents LabelLast As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLast As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonActivateAsPrimary As System.Web.UI.WebControls.Button
    'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Declared Control for deactivate button
    Protected WithEvents btnDeactivateSecondaryAddrs As System.Web.UI.WebControls.Button
    'End - Manthan | 2016.02.24 | YRS-AT-2328 | Declared Control for deactivate button
    Protected WithEvents CheckboxBadEmail As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxUnsubscribe As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxTextOnly As System.Web.UI.WebControls.CheckBox
    'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from participant and put on address control
    'Protected WithEvents CheckboxIsBadAddress As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents CheckboxSecIsBadAddress As System.Web.UI.WebControls.CheckBox
    ''''Protected WithEvents CheckboxSecBadEmail As System.Web.UI.WebControls.CheckBox
    ''''Protected WithEvents CheckboxSecUnsubscribe As System.Web.UI.WebControls.CheckBox
    ''''Protected WithEvents CheckboxSecTextOnly As System.Web.UI.WebControls.CheckBox

    'Protected WithEvents DropdownlistSecCountry As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DropdownlistSecState As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelSuffix As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSuffix As System.Web.UI.WebControls.TextBox
    '  Protected WithEvents ButtonActivateAsPrimary As System.Web.UI.WebControls.Button
    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGender As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListGender As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelMaritalStatus As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListMaritalStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelDOB As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxDOB As YMCAUI.DateUserControl
    Protected WithEvents valCustomDOB As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents ReqCustomDOB As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents LabelParticipationDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxParticipationDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDeceasedDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxDeceasedDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSpousealWiver As System.Web.UI.WebControls.Label
    ''Protected WithEvents TextBoxSpousealWiver As YMCAUI.DateUserControl
    Protected WithEvents PopcalendarSpousealWiver As RJS.Web.WebControl.PopCalendar
    Protected WithEvents TextBoxSpousealWiver As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelQDROPending As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBoxQDROPending As System.Web.UI.WebControls.CheckBox
    'Start: Bala: 01/19/2019: YRS-AT-2398: Control name change and hiddenfield is added 
    'Protected WithEvents checkboxPriority As System.Web.UI.WebControls.CheckBox 'Added by Dilip yadav : YRS 5.0.921
    Protected WithEvents checkboxExhaustedDBSettle As System.Web.UI.WebControls.CheckBox
    Protected WithEvents HiddenFieldOfficerDetails As System.Web.UI.WebControls.HiddenField
    Protected WithEvents LabelSpecialHandling As System.Web.UI.WebControls.Label
    Protected WithEvents LabelExhaustedDBSettleHdr As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelPriority As System.Web.UI.WebControls.Label 'Added by Dilip yadav : YRS 5.0.921
    'Protected WithEvents LabelPriorityHdr As System.Web.UI.WebControls.Label 'Added by Dilip yadav : YRS 5.0.921
    'End: Bala: 01/19/2019: YRS-AT-2398: Control name change and hiddenfield is added 
    Protected WithEvents LabelQDRODraftDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxQDRODraftDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelQDROType As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxQDROType As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelQDROStatusDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxQDROStatusDate As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelAddress1 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelAddress2 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelAddress3 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress3 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelState As System.Web.UI.WebControls.Label
    'Protected WithEvents DropdownlistState As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents LabelZip As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxZip As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelCountry As System.Web.UI.WebControls.Label
    'Protected WithEvents DropdownlistCountry As System.Web.UI.WebControls.DropDownList
    'Start:Commented by Anudeep:12.03.2013-For Telephone Usercontrol
    Protected WithEvents LabelTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxHome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxMobile As System.Web.UI.WebControls.TextBox
    'End:Commented by Anudeep:12.03.2013-For Telephone Usercontrol
    Protected WithEvents LabelEmailId As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEmailId As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPOA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPOA As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelVested As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxVested As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelYear As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYear As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelMonth As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonth As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelYearTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYearTotal As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelPrimary As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxPrimary As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelCont1 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCont1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelCont2 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCont2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelCont3 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCont3 As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelRetiredPrimary As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxRetiredPrimary As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelRetiredCont1 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxRetiredCont1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelRetiredCont2 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxRetiredCont2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelRetiredCont3 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxRetiredCont3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents RadioButtonList1 As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents RadioButtonFunded As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadiobuttonTransaction As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelMonthTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonthTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSaveParticipant As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridParticipantEmployment As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridAdditionalAccounts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridPartAcctContribution As System.Web.UI.WebControls.DataGrid
    Protected WithEvents GridViewRetireesAttorney As System.Web.UI.WebControls.GridView
    'Vipul 03Mar06 Fixing the Totals truncate problem Issue # 
    'Protected WithEvents DatagridTotal As System.Web.UI.WebControls.DataGrid
    'Vipul 03Mar06 Fixing the Totals truncate problem Issue # 
    'Aparna 29/08/2007 Plan Slipt -Show Retirement and Saving Plans Separately.
    Protected WithEvents DataGridSavingAccntContributions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridRetirementAccntContributions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridAcctTotal As System.Web.UI.WebControls.DataGrid
    'Aparna 29/08/2007 
    Protected WithEvents DataGridParticipantNotes As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonAddEmployment As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditEmploymentService As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddItem As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAccountUpdateItem As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEdit As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditSSno As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditDOB As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDeathNotification As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditQDRO As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditAddress As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonTelephone As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEmail As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPOA As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddItemNotes As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPHR As System.Web.UI.WebControls.Button
    'Protected WithEvents TextBoxPTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPNonTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPInterest As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxYTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxYInterest As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxYTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTotal As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonView As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNotSet As System.Web.UI.WebControls.Label
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonUpdateEmployment As System.Web.UI.WebControls.Button
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Protected WithEvents DataGridActiveBeneficiaries As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelPercentage1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPrimaryA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPrimaryA As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonPriA As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxPrimaryInsA As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonPriInsA As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCont1A As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCont1A As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont1A As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxCont1InsA As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont1InsA As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCont2A As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCont2A As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont2A As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxCont2InsA As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont2InsA As System.Web.UI.WebControls.Button
    Protected WithEvents LabelCont3A As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCont3A As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont3A As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxCont3InsA As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonCont3InsA As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddActive As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonEditActive As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDeleteActive As System.Web.UI.WebControls.Button
    Protected WithEvents NotesFlag As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents EmailId As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Unsubscribe As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TextOnly As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents BadEmail As System.Web.UI.HtmlControls.HtmlInputHidden
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Protected WithEvents TextBoxSecAddress1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecAddress2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecAddress3 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents DropDownListSecState As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents TextBoxSecZip As System.Web.UI.WebControls.TextBox
    'Protected WithEvents DropDownListSecCountry As System.Web.UI.WebControls.DropDownList
    'Start:Added by Anudeep:12.03.2013-For Telephone Usercontrol
    Protected WithEvents TextBoxSecTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSecHome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSecMobile As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSecFax As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecEmail As System.Web.UI.WebControls.TextBox
    'End:Added by Anudeep:12.03.2013-For Telephone Usercontrol
    Protected WithEvents PopCalendar2 As RJS.Web.WebControl.PopCalendar
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents RegularExpressionValidator2 As System.Web.UI.WebControls.RegularExpressionValidator
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Protected WithEvents TextboxLoanNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLoanAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxLoanAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxPayrollFrequency As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayrollFrequency As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLoanTerm As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxLoanTerm As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLoanStatus As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxLoanStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLoanNumber As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPayoffAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPayoffAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSpousalConsentReceivedDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLoanApprovalDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelInterestRate As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxInterestRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFirstPaymentDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPaymentAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPaymentAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFinalPaymentDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLoanOriginationFee As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxLoanOriginationFee As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPaymentHistory As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridPaymentHistory As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonLoanReport As System.Web.UI.WebControls.Button
    Protected WithEvents TextboxFirstPaymentDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxLoanApprovedDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxSpousalConsentReceivedDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxFinalPaymentDate As System.Web.UI.WebControls.TextBox
    'anita may 9th
    Protected WithEvents HiddenSecControlName As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HiddenText As System.Web.UI.HtmlControls.HtmlInputHidden
    'anita may 9th
    'Shubhrata yren 3012 jan 8th 2006
    Protected WithEvents ButtonSavePayoffAmount As System.Web.UI.WebControls.Button
    Protected WithEvents LabelSavedPayoffAmount As System.Web.UI.WebControls.Label
    Protected WithEvents LabelComputeOn As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxSavedPayoffAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxComputeOn As System.Web.UI.WebControls.TextBox
    Protected WithEvents PanelPayOffAmount As System.Web.UI.WebControls.Panel
    Protected WithEvents RegularExpressionValidator3 As System.Web.UI.WebControls.RegularExpressionValidator
    'Shubhrata yren 3012 jan 8th 2006
    'Shubhrata YREN-3131 Mar06th,2007
    Protected WithEvents LabelYmcaNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxYmcaNo As System.Web.UI.WebControls.TextBox
    'Shubhrata YREN-3131 Mar06th,2007
    'Protected WithEvents AddressWebUserControl1 As AddressWebUserControl
    'Protected WithEvents AddressWebUserControl2 As AddressWebUserControl
    'Protected WithEvents AddressWebUserControl1 As NewAddressUserControl
    'Protected WithEvents AddressWebUserControl2 As NewAddressUserControl
    Protected WithEvents AddressWebUserControl1 As AddressUserControlNew
    Protected WithEvents AddressWebUserControl2 As AddressUserControlNew
    Protected WithEvents ButtonSaveParticipants As System.Web.UI.WebControls.Button
    'Shubhrata Plan Split Changes
    Protected WithEvents HyperLinkViewRetireesInfo As System.Web.UI.WebControls.HyperLink
    'Shubhrata Plan Split Changes
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Protected WithEvents CustomvalidatorDate As System.Web.UI.WebControls.CustomValidator
    'Swopna 24 Mar,2008
    Protected WithEvents DataGridLoanAccountBreakdown As System.Web.UI.WebControls.DataGrid
    'Swopna 24 Mar,2008
    'Swopna 27 May,2008
    Protected WithEvents ButtonTerminateParticipation As System.Web.UI.WebControls.Button
    'shashi 28th aug 2009 for data retrieve
    Protected WithEvents tdRetrieveData As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tbltermPart As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents LabelGenHdr As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAccContHdr As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonGetArchiveDataBack As System.Web.UI.WebControls.Button
    'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
    'Priya 17/2/2010 yrs 5.0-988 
    'Protected WithEvents lblShareInfoAllowed As System.Web.UI.WebControls.Label
    'Protected WithEvents chkShareInfoAllowed As System.Web.UI.WebControls.CheckBox
    'Endyrs 5.0-988
    'End YRS 5.0-1073
    'Added by priya as on 5-April-2010 : YRS 5.0-1042- New "flag" value in Person/Retiree maintenance screen
    'Protected WithEvents chkYMCAMailingOptOut As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents lblYMCAMailingOptOut As System.Web.UI.WebControls.Label
    'End YRS 5.0-1042
    'Changed by bhavnaS on 2011.07.08 :YRS 10.1.1.4- change caption
    Protected WithEvents chkPersonalInfoSharingOptOut As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblPersonalInfoSharingOptOut As System.Web.UI.WebControls.Label
    Protected WithEvents chkGoPaperless As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblGoPaperless As System.Web.UI.WebControls.Label
    'End YRS 10.1.1.4
    'Swopna 27 May,2008
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents ValidationSummaryParticipants As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents rfvGender As System.Web.UI.WebControls.RequiredFieldValidator
    'Added by imran on 30-July-2010 : YRS 5.0-1141 add Age format to the person maintanace screen.
    Protected WithEvents TextBoxAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents labelBoxAge As System.Web.UI.WebControls.Label
    'Added by Shashi Shekhar:Feb 008 2011 for YRS 5.0-1236
    Protected WithEvents trLockResDisplay As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trLockResEdit As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblLockstatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblLockResDetail As System.Web.UI.WebControls.Label
    Protected WithEvents btnLockUnlock As System.Web.UI.WebControls.Button
    Protected WithEvents btnSaveReason As System.Web.UI.WebControls.Button
    Protected WithEvents btnCancelReason As System.Web.UI.WebControls.Button
    Protected WithEvents btnAcctLockEdit As System.Web.UI.WebControls.Button
    Protected WithEvents ddlReasonCode As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lnkbtnYMCANo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblResCode As System.Web.UI.WebControls.Label
    Protected WithEvents updContacts As System.Web.UI.UpdatePanel
    Protected WithEvents updGridContact As System.Web.UI.UpdatePanel
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
    Protected WithEvents HiddenFieldDirty As System.Web.UI.WebControls.HiddenField

    'BS:2012.01.16:YRS 5.0-1497 -Add edit button to modify the date of death
    Protected WithEvents btnEditDeathDatePer As System.Web.UI.WebControls.Button
    Protected WithEvents HiddenFieldDeathDate As System.Web.UI.WebControls.HiddenField
    'prasad jadhav on	2012.01.19:For BT-925,YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
    Protected WithEvents chkMRD As System.Web.UI.WebControls.CheckBox
    'Added by prasad on 2012.03.12:For BT-991,YRS 5.0-1530 : Need ability to reactivate a terminated employee
    Protected WithEvents ButtonReactivate As System.Web.UI.WebControls.Button
    'BS:2012.06.18:BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
    Protected WithEvents hd_emp_reactive As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hd_tdcontractexist As System.Web.UI.WebControls.HiddenField
    'SR:2012.08.17-BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program
    Protected WithEvents ButtonTerminationWatcher As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents ddlPlanType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents gvPersonDetails As System.Web.UI.WebControls.GridView
    Protected WithEvents imgEdit As System.Web.UI.WebControls.ImageButton
    Protected WithEvents tdPoa As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label

    Protected WithEvents btnSave As System.Web.UI.WebControls.Button
    Protected WithEvents btn_Ok_Message As System.Web.UI.WebControls.Button
    Protected WithEvents btnCancel As System.Web.UI.WebControls.Button
    Protected WithEvents btnAdd As System.Web.UI.WebControls.Button
    Protected WithEvents btnOk As System.Web.UI.WebControls.Button

    Protected WithEvents gvContacts As System.Web.UI.WebControls.GridView

    Protected WithEvents btnEditPrimaryContact As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnEditSecondaryContact As System.Web.UI.WebControls.ImageButton

    Protected WithEvents lblOfficeContact As System.Web.UI.WebControls.Label
    Protected WithEvents lblHomeContact As System.Web.UI.WebControls.Label
    Protected WithEvents lblMobile As System.Web.UI.WebControls.Label
    Protected WithEvents lblFax As System.Web.UI.WebControls.Label

    'Protected WithEvents lblSecoffice As System.Web.UI.WebControls.Label
    'Protected WithEvents lblSecHome As System.Web.UI.WebControls.Label
    'Protected WithEvents lblSecMobile As System.Web.UI.WebControls.Label
    'Protected WithEvents lblSecFax As System.Web.UI.WebControls.Label
    'Added by Anudeep:12.03.2013-For Telephone Usercontrol 
    Protected WithEvents TelephoneWebUserControl1 As Telephone_WebUserControl
    Protected WithEvents TelephoneWebUserControl2 As Telephone_WebUserControl
    Protected WithEvents tbPricontacts As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tbSeccontacts As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents imgLock As System.Web.UI.WebControls.Image
    Protected WithEvents imgLockBeneficiary As System.Web.UI.WebControls.Image
    'Anudeep-2013.08.21 - YRS 5.0-2162:Edit button for email address 
    Protected WithEvents imgBtnEmail As System.Web.UI.WebControls.ImageButton
    'Anudeep:21.10.2013:BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
    Protected WithEvents DivMainMessage As System.Web.UI.HtmlControls.HtmlGenericControl

    'DInesh.k 2013.09.17 - YRS 5.0-2165:RMD enhancements. 
    Protected WithEvents lblRMDTax As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRMDTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnEditRMDTAX As System.Web.UI.WebControls.Button
    Protected WithEvents tblRMDTAX As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tdRMDTax As System.Web.UI.HtmlControls.HtmlTableCell
    'AA:2014.02.03 - BT:2292:YRS 5.0-2248 - Added the Textbox value
    Protected WithEvents txtPIN As System.Web.UI.WebControls.TextBox
    'AA:2014.02.03 - BT:2316:YRS 5.0-2262 - Added the Checkbox and label
    Protected WithEvents chkCannotLocateSpouse As System.Web.UI.WebControls.CheckBox
    'Start: Bala: 01/05/2016: YRS-AT-1972: Added the Special Death Processing Required Checkbox.
    Protected WithEvents chkSpecialDeathProcess As System.Web.UI.WebControls.CheckBox
    'End: Bala: 01/05/2016: YRS-AT-1972: Added the Special Death Processing Required Checkbox.
    Protected WithEvents lblCannotLocateSpouse As System.Web.UI.WebControls.Label
    'AA:2014.02.03 - BT:2316:YRS 5.0-2247 - Added the Checkbox and label
    Protected WithEvents chkNoWebAcctCreate As System.Web.UI.WebControls.CheckBox
    'AA:2015.03.11  BT-2699:YRS 5.0-2441 : Added fields for freezing default loan
    Protected WithEvents txtDefaultAmt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFrozenOn As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPhantomInt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCompFrozenAmt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtComputedAmt As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnFreeze As System.Web.UI.WebControls.Button
    Protected WithEvents btnUnFreeze As System.Web.UI.WebControls.Button
    Protected WithEvents tblFreeze As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblPayOff As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblOffsetReason As System.Web.UI.WebControls.Label
    Protected WithEvents lblOffsetReasonText As System.Web.UI.WebControls.Label
    Protected WithEvents divPayOffConfirmationMessage As System.Web.UI.HtmlControls.HtmlGenericControl ' PPP | 06/01/2017 | YRS-AT-3460 | A Div control placed on divPayOffConfirmation dialog box to contain dynamic message 

    'START: MMR | 2018.04.06 | YRS-AT-3935 | Declaring Controls
    Protected WithEvents txtPaymentMethod As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPaymentStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAccountType As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAccountNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBankABA As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblPaymentMethod As System.Web.UI.WebControls.Label
    Protected WithEvents lblPaymentStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccountType As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccountNumber As System.Web.UI.WebControls.Label
    Protected WithEvents lblBankABA As System.Web.UI.WebControls.Label
    Protected WithEvents gvPaymentMethodHistory As System.Web.UI.WebControls.GridView
    Protected WithEvents divPaymentMethodHistory As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents btnClose As System.Web.UI.WebControls.Button
    Protected WithEvents imgPaymentDetailsHistory As System.Web.UI.HtmlControls.HtmlImage
    'END: MMR | 2018.04.06 | YRS-AT-3935 | Declaring Controls

    'START: VC | 2018.08.01 | YRS-AT-4018 | Declaring Controls
    Protected WithEvents trLoanAccountDetailsBody As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPaymentHistoryHeader As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trPaymentHistoryBody As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trLoanAccountDetails As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtWebLoanNumber As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebYMCA As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebRequestedAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebApplicationStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMaxAvailableLoanAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebStatusDateTime As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebRequestedTerm As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebApplicationDateTime As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebPayrollFrequency As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebLoanPurpose As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebPaymentMethod As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebONDRequested As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebBankName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebBankABA As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebAccount As System.Web.UI.WebControls.TextBox
    'START: PK | 01/10/2019 | YRS-AT-4241 | This line is commented for proper naming, change of txtWebMaritalStatus to txtOldWebMaritalStatus and additon of txtNewWebMaritalStatus
    'Protected WithEvents txtWebMaritalStatus As System.Web.UI.WebControls.TextBox 
    Protected WithEvents txtOldWebMaritalStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNewWebMaritalStatus As System.Web.UI.WebControls.TextBox
    'END: PK | 01/10/2019 | YRS-AT-4241 | This line is commented for proper naming, change of txtWebMaritalStatus to txtOldWebMaritalStatus and additon of txtNewWebMaritalStatus
    Protected WithEvents txtWebAccountType As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebDocreceivedDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents popCalendarDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents lblWebDocreceivedDate As System.Web.UI.WebControls.Label
    Protected WithEvents trEFTBankInfo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEFTAccountInfo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtBoxONDRequested As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDefaultedLoanDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents trLoanReport As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lnkReturnToLoanAdmin As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkReturnToLoanAdminExpand As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtWebReasonNotes As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblWebDocCode As System.Web.UI.WebControls.Label
    Protected WithEvents btnApproveWebLoan As System.Web.UI.HtmlControls.HtmlButton
    Protected WithEvents btnDeclineWebLoan As System.Web.UI.HtmlControls.HtmlButton
    Protected WithEvents btnYesLoanApprove As System.Web.UI.WebControls.Button
    Protected WithEvents btnConfirmDeclineWebLoan As System.Web.UI.HtmlControls.HtmlButton
    Protected WithEvents ddlDeclineReason As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtDeclineNotes As System.Web.UI.HtmlControls.HtmlTextArea
    Protected WithEvents DivSuccessAndErrorMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DivEmailSuccessErrorMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents gvApplicationStatusHistory As System.Web.UI.WebControls.GridView
    Protected WithEvents lblBankName As System.Web.UI.WebControls.Label
    Protected WithEvents txtBankName As System.Web.UI.WebControls.TextBox
    'END: VC | 2018.08.01 | YRS-AT-4018 | Declaring Controls
    ' START : SC | 2020.04.20 | YRS-AT-4853 | Added new textboxes for Loan type for YRS & Web loan details
    Protected WithEvents lblLoanType As System.Web.UI.WebControls.Label
    Protected WithEvents txtLoanType As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblWebLoanType As System.Web.UI.WebControls.Label
    Protected WithEvents txtWebLoanType As System.Web.UI.WebControls.TextBox
    ' END : SC | 2020.04.20 | YRS-AT-4853 | Added new textboxes for Loan type for YRS & Web loan details
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Dim g_dataset_GeneralInfo As New DataSet
    Dim l_DataSet_POA As New DataSet
    Dim g_String_Exception_Message As String
    Dim _icounter As Integer
    Dim l_Str_Msg As String
    'Dim reg As Regex
    Dim l_ds_States As DataSet
    Dim l_dt_State As New DataTable
    Dim l_dt_State_filtered As New DataTable
    Dim dr As DataRow
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
    'BS:2012.03.09:-restructure Address Control:Add new Control into User control
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
    'BS:2012.03.09:-restructure Address Control:Add new Control into User control
    Dim m_str_Sec_EffDate As String
    Dim m_str_Msg As String
    Dim g_isArchived As Boolean
    Public l_string_VignettePath As String '= System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_Participant"), Session("FundNo"))





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
    'SP 2014.04.29 : BT-2525  -Start
    Public Property RMDTaxRate As String
        Get
            If Not (ViewState("RMDTaxRate")) Is Nothing Then
                Return (CType(ViewState("RMDTaxRate"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("RMDTaxRate") = Value
        End Set
    End Property
    'SP 2014.04.29 : BT-2525  -End

    ''SP 2014.11.20  BT-2310\YRS 5.0-2255: -Start
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
    ''SP 2014.11.20  BT-2310\YRS 5.0-2255: -End

    'START: MMR| 2016.10.12 | YRS-AT-3063 | Declared property to store ExhaustedDBRMD settle option selected or not
    Private Property IsExhaustedDBRMDSettleOptionSelected As Boolean
        Get
            If Not (ViewState("IsExhaustedDBRMDSettleOptionSelected")) Is Nothing Then
                Return (CType(ViewState("IsExhaustedDBRMDSettleOptionSelected"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsExhaustedDBRMDSettleOptionSelected") = Value
        End Set
    End Property
    'END: MMR| 2016.10.12 | YRS-AT-3063 | Declared property to store ExhaustedDBRMD settle option selected or not

    'START: VC| 2018.24.08 | YRS-AT-4018 | Declared property to store Approve confirmation message
    Public Property ApproveConfirmationMessage() As String
        Get
            If Not (ViewState("ApproveConfirmationMessage")) Is Nothing Then
                Return (CType(ViewState("ApproveConfirmationMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("ApproveConfirmationMessage") = Value
        End Set
    End Property
    'END: VC| 2018.24.08 | YRS-AT-4018 | Declared property to store Approve confirmation message

    'START: VC| 2018.24.08 | YRS-AT-4018 | Declared property to store Approve button access rights
    Public Property ApproveButtonAuthorization() As String
        Get
            If Not (ViewState("ApproveButtonAuthorization")) Is Nothing Then
                Return (CType(ViewState("ApproveButtonAuthorization"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("ApproveButtonAuthorization") = Value
        End Set
    End Property
    'END: VC| 2018.24.08 | YRS-AT-4018 | Declared property to store Approve button access rights

    'START: VC| 2018.24.08 | YRS-AT-4018 | Declared property to store Decline button access rights
    Public Property DeclineButtonAuthorization() As String
        Get
            If Not (ViewState("DeclineButtonAuthorization")) Is Nothing Then
                Return (CType(ViewState("DeclineButtonAuthorization"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("DeclineButtonAuthorization") = Value
        End Set
    End Property
    'END: VC| 2018.24.08 | YRS-AT-4018 | Declared property to store Decline button access rights

    'START: VC| 2018.13.11 | YRS-AT-4018 | Declared property to store Decline button access rights
    Public Property ProcessInProgress() As String
        Get
            If Not (ViewState("ProcessInProgress")) Is Nothing Then
                Return (CType(ViewState("ProcessInProgress"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("ProcessInProgress") = Value
        End Set
    End Property
    'END: VC| 2018.13.11 | YRS-AT-4018 | Declared property to store Decline button access rights


#Region "Constant Values"
    'By Ashutosh Patil as on 30-Mar-2007
    'YREN-3203,YREN-3205
    Const m_const_int_UniqueId As Integer = 1
    Const m_const_int_PersonId As Integer = 2
    Const m_const_int_YmcaId As Integer = 3
    Const m_const_int_FundEventId As Integer = 4
    Const m_const_int_HireDate As Integer = 5
    Const m_const_int_BasicPaymentDate As Integer = 6
    Const m_const_int_Termdate As Integer = 7
    Const m_const_int_EligibilityDate As Integer = 8
    Const m_const_int_Professional As Integer = 9
    Const m_const_int_Salaried As Integer = 10
    Const m_const_int_FullTime As Integer = 11
    Const m_const_int_PriorService As Integer = 12
    Const m_const_int_StatusType As Integer = 13
    Const m_const_int_StatusDate As Integer = 14
    Const m_const_int_Status As Integer = 15
    Const m_const_int_YmcaName As Integer = 16
    Const m_const_int_YmcaNo As Integer = 17
    Const m_const_int_PositionType As Integer = 18
    Const m_const_int_PositionDesc As Integer = 19
    Const m_const_int_BranchId As Integer = 20
    Const m_const_int_Active As Integer = 21
    Const m_const_int_BranchName As Integer = 22
    Const m_const_int_AddAcctUniqueId As Integer = 1
    Const m_const_int_AddAcctEmpEventId As Integer = 2
    Const m_const_int_AddAcctYmcaId As Integer = 3
    Const m_const_int_AddAcctAccountType As Integer = 4
    Const m_const_int_AddAcctYmcaNo As Integer = 5
    Const m_const_int_AddAcctBasisCode As Integer = 6
    Const m_const_int_AddAcctDescriptions As Integer = 7
    Const m_const_int_AddAcctContributionPer As Integer = 8
    Const m_const_int_AddAcctContribution As Integer = 9
    Const m_const_int_AddAcctEffectiveDate As Integer = 10
    Const m_const_int_AddAcctTerminationDate As Integer = 11
    Const m_const_int_AddAcctHireDate As Integer = 12
    Const m_const_int_AddAcctEnrollmentDate As Integer = 13
#End Region
    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Property to store relationship
    Public Property RelationShipCheck As DataSet
        Get
            Return ViewState("RelationShipCheck")
        End Get
        Set(value As DataSet)
            ViewState("RelationShipCheck") = value
        End Set
    End Property
    ' // END: SB | 07/07/2016 | YRS-AT-2382 |   Constant variables for Beneficiaries update

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

    ''Added property that will vaildate Death Notification is allowed or not for RMD eligible participants 
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
    ''END: SB | 2017.10.18 | YRS-AT-3324 | Added property that will return the reason for restriction when prcoessing for Death Notification 
    'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 

    'START: SR| 2018.07.12 | YRS-AT-3858 | Declared property to store participant RMD eligibility.
    Private Property IsRMDEligible As Boolean
        Get
            If Not (ViewState("IsRMDEligible")) Is Nothing Then
                Return (CType(ViewState("IsRMDEligible"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsRMDEligible") = Value
        End Set
    End Property
    'END: SR| 2018.07.12 | YRS-AT-3858 | Declared property to store participant RMD eligibility.

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

    'Public IsStatusDR As Boolean = False
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strWSMessage As String
        Dim dsRelationshipCode As New DataSet ' //SB | 07/07/2016 | YRS-AT-2382 | Dataset to hold relationship codes
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        l_string_VignettePath = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_Participant"), Session("FundNo"))

        Dim popupScript As String = "<script language='javascript'>" & _
            "CallFrame('" + l_string_VignettePath + "'); </script>"

        'Page.RegisterStartupScript("IDMPopUpScript", popupScript)
        ClientScript.RegisterClientScriptBlock(Page.GetType(), "IDMPopUpScript", popupScript)

        LinkRefereshIDM.Attributes.Add("onclick", "CallFrame('" + l_string_VignettePath + "');")

        'Put user code to initialize the page here
        Menu1.DataSource = Server.MapPath("SimpleXML.xml")
        Menu1.DataBind()
        'PanelPayOffAmount.BorderStyle = BorderStyle.Double
        'anita may 9th
        InitializeAttributesOfServerControls()

        'BS:2012.01.16:YRS 5.0-1497 -Add edit button to modify the date of death:- Security Checking of control
        Me.btnEditDeathDatePer.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, btnEditDeathDatePer.ID))
        'Shashi Shekhar:09- feb 2011: For YRS 5.0-1236 : Need ability to freeze/lock account
        Me.btnAcctLockEdit.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, btnAcctLockEdit.ID))
        TextBoxDate.Attributes.Add("onChange", "if (typeof(Page_ClientValidate) == 'function') { if (Page_ClientValidate() == false) { SetTextBox(this, '" & Now.ToShortDateString() & "'); return false; } }")
        'TextBoxDeceasedDate.Attributes.Add("onChange", " getAge();")
        'If Session("LoggedUserKey") Is Nothing Then
        '    Response.Redirect("Login.aspx", False)
        'End If
        Try


            ValidationSummaryParticipants.Enabled = True 'Added By Shashi Shekhar:2009-12-23
            'START : Commented  & Added by Dilip Yadav : YRS 5.0.921
            'Priya : 24-March-2010: BT:479: While adding QDRO Request page not getting Refresh.
            'Commented If not i spost back code and uun comment RefreshQdroInfo() to refresh QDRO information after update and added new QDRO.
            RefreshQdroInfo()
            'If (Not Page.IsPostBack) Then
            '    RefreshQdroInfo()
            'End If
            'End 24-March-2010: BT:479 
            'END : DY : 5.0.921
            Session("ISRetired") = False
            If Not IsPostBack Then
                'START: MMR| 2011.11.22 | YRS-AT-4018 | Get participant PII Information updation allowed status.
                Me.PIIInformationRestrictionMessageCode = YMCARET.YmcaBusinessObject.CommonBOClass.ValidatePIIRestrictions(CType(Session("PersId"), String))
                If Me.PIIInformationRestrictionMessageCode <> 0 Then
                    Me.PIIInformationRestrictionMessageText = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(Me.PIIInformationRestrictionMessageCode, Nothing)
                End If
                'END: MMR| 2011.11.22 | YRS-AT-4018 | Get participant PII Information updation allowed status.
                'Swopna 27May08-----start

                ''Me.IframeIDMView.Attributes("src") = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_Participant"), Session("FundNo"))
                'Me.IframeIDMView.Attributes.Add("href", System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_Participant"), Session("FundNo")))

                Dim l_ds_FundEventStatus As DataSet
                Dim l_string_FundEventStatus As String
                'Ashish:2010.07.21, YRS 5.0-1136,handle when multiple fund event exist ,Start
                'l_ds_FundEventStatus = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetFundEventStatus(CType(Session("PersId"), String))
                l_ds_FundEventStatus = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetFundEventStatus(CType(Session("PersId"), String), CType(Session("FundId"), String))
                'Ashish:2010.07.21, YRS 5.0-1136,handle when multiple fund event exist ,End
                l_string_FundEventStatus = CType(l_ds_FundEventStatus.Tables("FundEventStatus").Rows(0)("FundStatusType"), String)
                'HR: to fix issue reported by Prashant on 16-Jul-2008
                Session("FundStatusType") = l_string_FundEventStatus.ToUpper().Trim()
                If l_string_FundEventStatus <> "" Then
                    Select Case (l_string_FundEventStatus.ToUpper().Trim())
                        Case "NP", "PENP", "RDNP", "ML", "PEML"
                            Me.ButtonTerminateParticipation.Visible = True
                            Me.tbltermPart.Visible = True
                        Case Else
                            Me.ButtonTerminateParticipation.Visible = False
                            Me.tbltermPart.Visible = False
                    End Select
                End If

                ' // START : SB | 07/07/2016 | YRS-AT-2382 | Loading relationship code from database
                dsRelationshipCode = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.getRelationShips()
                RelationShipCheck = dsRelationshipCode
                ' // END : SB | 07/07/2016 | YRS-AT-2382 | Loading relationship code from database

                'Swopna 27May08-----end
                'Shubhrata YRSPS 4614
                Session("Participant_Sort") = Nothing
                Session("DisplayNotes") = Nothing
                Session("AuditBeneficiariesTable") = Nothing    ' //  SB | 07/07/2016 | YRS-AT-2382 | Clearing auditlog data table

                'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
                'ClearSession()  'SB | 10/18/2017 | YRS-AT-3324 | Clearing Session variable used to check RMD generated or not
                'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 

                'Shubhrata YRSPS 4614
                ''Me.TextBoxEffDate.RequiredDate = True
                ''Me.TextBoxEffDate.Enabled = False
                ''Me.TextBoxSecEffDate.RequiredDate = True
                ''Me.TextBoxSecEffDate.Enabled = False
                'anita may 9th
                getSecuredControls()
                'anita may 9th
                InitializeMartialStatusDropDownList()
                InitializeGenderTypesDropDownList()
                TextBoxDate.Text = DateAndTime.DateString() 'Date on the account contribution tab to bring balances
                'Vipul 27Feb06 fixing Notes Tab Color Problem Issue # YMCA-2127
                'Move code from Not Postback to general
                'If Me.NotesFlag.Value = "Notes" Then
                '    Me.TabStripParticipantsInformation.Items(5).Text = "<font color=orange>Notes</font>"
                'Else
                '    Me.TabStripParticipantsInformation.Items(5).Text = "Notes"
                'End If
                'Vipul 27Feb06 fixing Notes Tab Color Problem Issue # YMCA-2127
                LoadGeneraltab()


                LoadEmploymentTab()
                LoadAccountContributionsTab(sender, e)
                LoadAdditionalAccountsTab()
                LoadBeneficiariesTab()
                LoadNotesTab()
                'Me.LoadLoansTab()'VC| 2018.05.09 | YRS-AT-4018 | Commented code to avoid multiple execution

                GetWebAccountPrintConfigValue() 'JT | 2018.08.28 | YRS-AT-4031 | Fetching configuration value

                'commented by Aparna -YREN-3115 Moved to LoadNotesTab()
                ''Vipul 27Feb06 fixing Notes Tab Color Problem Issue # YMCA-2127
                ''Move code from Not Postback to general
                'If Me.NotesFlag.Value = "Notes" Then
                '    Me.TabStripParticipantsInformation.Items(5).Text = "<font color=orange>Notes</font>"
                'ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
                '    Me.TabStripParticipantsInformation.Items(5).Text = "<font color=red>Notes</font>"
                'Else
                '    Me.TabStripParticipantsInformation.Items(5).Text = "Notes"
                'End If
                '*********Code changed by ashutosh on 23-June-06****
                'ViewState("SelectedState") = Nothing
                Session("PhonySSNo") = Nothing
                '***************
                'Vipul 27Feb06 fixing Notes Tab Color Problem Issue # YMCA-2127

                'divTerminationWatcher.Visible = False

                'Start:AA:2013.10.23 - Bt-2264:Commented below code and has been placed out of ispostback condition
                ''Start -> DineshK:2013.02.12:BT-1261/YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                'If Not Session("Reason") Is Nothing Then
                '    If Session("Reason").ToString.ToLower = "yes" Then
                '        Me.MultiPageParticipantsInformation.SelectedIndex = 4
                '        Me.TabStripParticipantsInformation.SelectedIndex = 4
                '        If Not Session("BenTableCellCollection") Is Nothing Then
                '            DeleteActiveBeneficairy(CType(Session("BenTableCellCollection"), TableCellCollection), CType(Session("BenItemIndex"), Integer))
                '            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Delete beneficiary reason added sucessfully.", MessageBoxButtons.OK)
                '        End If
                '    ElseIf Session("Reason").ToString.ToLower = "no" Then
                '        Me.MultiPageParticipantsInformation.SelectedIndex = 4
                '        Me.TabStripParticipantsInformation.SelectedIndex = 4
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

                    Me.TabStripParticipantsInformation.Items(4).Text = "<asp:Label ID='lblBeneficiaries' CssClass='Label_Small'onmouseover='javascript: showToolTip(""" + strWSMessage + """,""Bene"");' onmouseout='javascript: hideToolTip();'><font color=red>Beneficiaries</font></asp:Label>"

                    DropDownListMaritalStatus.Enabled = False

                    imgLock.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Pers');")
                    imgLock.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                    imgLock.Visible = True

                    imgLockBeneficiary.Visible = True
                    imgLockBeneficiary.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Bene');")
                    imgLockBeneficiary.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                End If
                'End, SR:2013.08.05 - YRS 5.0-2070 : Change the color of Beneficiary tab for Pending request.
                Session("IsExhaustedDBRMDSettle") = Nothing 'MMR | 2016.10.14 | YRS-AT-3062 | Clearing session value
            End If
            'Priya/Ashish       21-April-2010   YRS 5.0-1056: Additional info when unsuppressing a JSANN annuity
            If Not Session("FundStatusType") Is Nothing Then
                If Session("FundStatusType") = "BF" OrElse Session("FundStatusType") = "WD" Then
                    rfvGender.Enabled = False
                Else
                    rfvGender.Enabled = True
                End If
            End If
            'End 21-April-2010
            'Added by imran on 30-July-2010 : YRS 5.0-1141 add Age format to the person maintanace screen.
            'Added by imran on 03-July-2010 :  BT-865 Age shows wrong after Deceased date entry.
            'Added by Imran on 23-Aug-2010  : BT-568 Age shows wrong for exact one year.
            'BS:2011.08.10:YRS 5.0-1339 - reopen issue
            If Not TextBoxDOB.Text = String.Empty Then
                labelBoxAge.Text = CalculateAge(TextBoxDOB.Text, TextBoxDeceasedDate.Text).ToString("00.00")
                If (Me.labelBoxAge.Text.IndexOf(".00") > -1) Then
                    Me.labelBoxAge.Text = Me.labelBoxAge.Text.Replace(".00", "Y")
                Else
                    Me.labelBoxAge.Text = Me.labelBoxAge.Text.Replace(".", "Y/") + "M"
                End If
                Me.labelBoxAge.Text = Me.labelBoxAge.Text.Replace("/00M", "").Trim
            Else
                labelBoxAge.Text = String.Empty
            End If


            'TextBoxAge.Text = CalculateAge(TextBoxDOB.Text, TextBoxDeceasedDate.Text).ToString("00.00")
            'If (Me.TextBoxAge.Text.IndexOf(".00") > -1) Then
            '    Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace(".00", "Y")
            'Else
            '    Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace(".", "Y/") + "M"
            'End If
            'Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace("/00M", "").Trim
            If Not IsPostBack Then
                ValidationSummaryParticipants.Enabled = False 'Added By Shashi Shekhar:2009-12-23
                Me.ButtonOK.Enabled = True
                Me.TextBoxDOB.Enabled = False
                'START : VC | 2018.09.05 | YRS-AT-4018 -  Commented to remove disabled property
                'Me.TextboxFirstPaymentDate.Enabled = False
                'Me.TextboxFinalPaymentDate.Enabled = False
                'Me.TextboxSpousalConsentReceivedDate.Enabled = False
                'Me.TextboxLoanApprovedDate.Enabled = False
                'END : VC | 2018.09.05 | YRS-AT-4018 -  Commented to remove disabled property
                '' Me.PopCalendarDate.Enabled = False
                Me.TextBoxSuffix.Enabled = False
                Me.TextBoxMiddle.Enabled = False
                Me.TextBoxLast.Enabled = False
                Me.TextBoxFirst.Enabled = False
                Me.DropDownListSal.Enabled = False
                ''Me.DropDownListMaritalStatus.Enabled = False
                ''Me.DropDownListGender.Enabled = False
                Me.TextBoxDeceasedDate.Enabled = False
                Me.RadioButtonList1.Items(1).Selected = True
                Me.TextBoxDOB.Enabled = False
                Me.TextBoxFundNo.Enabled = False
                Me.TextBoxParticipationDate.Enabled = False
                'BS:2012.03.09:-restructure Address Control
                'Me.TextBoxEffDate.Enabled = False
                'Me.TextBoxSecEffDate.Enabled = False
                'Me.TextBoxPOA.Enabled = False 
                Me.TextBoxPOA.ReadOnly = True 'BS:2011.08.02:BT:911- TextBoxPOA should be readonly if multiple poa exist and all editing/adding of Poa information handle on POAscreen so there is no need to enable Poa Textbox
                Me.TextBoxQDRODraftDate.Enabled = False
                Me.TextBoxQDROStatusDate.Enabled = False
                Me.TextBoxQDROType.Enabled = False
                Me.CheckBoxQDROPending.Enabled = False
                Me.TextBoxSSNo.Enabled = False
                ''Me.TextBoxSpousealWiver.Enabled = False
                'start:BS:2012.04.26:YRS 5.0-1470: && restructure Address control
                Me.AddressWebUserControl1.EnableControls = False
                'Me.AddressWebUserControl1.EnableControls = True
                'Me.TelephoneWebUserControl1.EditPrimaryContact_Enabled = False
                'Commented by DInesh Kanojia Contacts
                'Start:Commented by Anudeep:12.03.2013-For Telephone Usercontrol
                tbPricontacts.Visible = False
                tbSeccontacts.Visible = False
                'btnEditPrimaryContact.Visible = False
                'btnEditSecondaryContact.Visible = False
                TextBoxTelephone.Enabled = False
                TextBoxHome.Enabled = False
                TextBoxFax.Enabled = False
                TextBoxMobile.Enabled = False
                TextBoxSecTelephone.Enabled = False
                TextBoxSecHome.Enabled = False
                TextBoxSecFax.Enabled = False
                TextBoxSecMobile.Enabled = False

                'End:Commented by Anudeep:12.03.2013-For Telephone Usercontrol

                TextBoxEmailId.Enabled = False
                LabelEmailId.Enabled = False
                'start:BS:2012.04.26:YRS 5.0-1470: && restructure Address control
                Me.AddressWebUserControl2.EnableControls = False
                'Me.TelephoneWebUserControl2.EditSecondaryContact_Enabled = False
                'Me.AddressWebUserControl2.EnableControls = True
                'Start:Commented by Anudeep:12.03.2013-For Telephone Usercontrol
                'TextBoxSecEmail.Enabled = False
                TextBoxSecTelephone.Enabled = False
                TextBoxSecHome.Enabled = False
                TextBoxSecFax.Enabled = False
                TextBoxSecMobile.Enabled = False
                EditAddressAndContacts()
                'End:Commented by Anudeep:12.03.2013-For Telephone Usercontrol
                'Added by prasad for BT 991 YRS 5.0-1530 - Need ability to reactivate a terminated employee
                If Not Session("FundStatusType") Is Nothing Then
                    If Session("FundStatusType") = "TM" OrElse Session("FundStatusType") = "RT" OrElse Session("FundStatusType") = "PT" Then
                        ButtonReactivate.Visible = True
                        ButtonReactivate.Enabled = True
                        'BS:2012.06.19:BT:991:YRS 5.0-1530 :- Add session which will be check again on page load when flags pass to client side
                        Session("ReactivationStart") = "True"
                    End If
                End If
                'Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Added code to change label text for birth date if person is non-human
                If Not Session("FundStatusType") Is Nothing Then
                    If Session("FundStatusType") = "BF" And DropDownListGender.SelectedValue = "X" And DropDownListMaritalStatus.SelectedValue = "X" Then
                        LabelDOB.Text = "Estd. Date"
                    Else
                        LabelDOB.Text = "DOB"
                    End If
                End If
                'End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Added code to change label text for birth date if person is non-human
            End If

            'SP :2014.02.04 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -start  
            If (Session("SessionTempSSNTable") IsNot Nothing) Then
                SetUpdatedSSN()
                If (IsSSNEdited()) Then
                    HiddenFieldDirty.Value = "true"
                End If
            End If
            'SP :2014.02.04 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -end  

            'START: VC | 2018.07.20 | YRS-AT-4017 | Code to load loan tab while redirecting from Loan Admin page
            ''START: VC | 2018.09.05 | YRS-AT-4018 | Commented existing code and added new code to check not postback
            'If Session("FlagLoans") = "Loans" Then
            '    Session("FlagLoans") = ""
            '    Session("Flag") = ""
            '    TabStripParticipantsInformation.SelectedIndex = 7
            '    MultiPageParticipantsInformation.SelectedIndex = 7
            '    LoadLoansTab()
            'End If
            ''END: VC | 2018.07.20 | YRS-AT-4017 | Code to load loan tab while redirecting from Loan Admin page

            If Not IsPostBack Then
                If (Session("FlagLoans") IsNot Nothing) Then
                    Session("FlagLoans") = Nothing
                    Session("Flag") = ""
                    TabStripParticipantsInformation.SelectedIndex = 7
                    MultiPageParticipantsInformation.SelectedIndex = 7
                    LoadLoansTab()
                    lnkReturnToLoanAdmin.Visible = True 'VC | 2018.08.07 | YRS-AT-4018 | Code to show redirect to Loan Admin link
                    lnkReturnToLoanAdminExpand.Visible = True 'VC | 2018.09.04 | YRS-AT-4018 | Code to show redirect to Loan Admin link
                Else
                    LoadLoansTab()
                End If
            End If
            ApproveDeclineAuthorization() 'VC | 2018.10.31 | YRS-AT-4018 | Calling method to set authorization property to viewstate
            'END: VC | 2018.09.05 | YRS-AT-4018 | Commented existing code and added new code to check not postback

            If Session("Flag") = "AddEmployment" Or Session("Flag") = "EditEmployment" Or Session("Flag") = "Edited" Then
                TabStripParticipantsInformation.SelectedIndex = 1
                MultiPageParticipantsInformation.SelectedIndex = 1
                LoadEmploymentTab()
                Session("Flag") = ""
                Session("icounter") = 0
            Else
                ViewState("DS_Sort_Employment") = Session("l_dataset_Employment")
            End If

            If Session("Flag") = "AddAccounts" Or Session("Flag") = "EditAccounts" Or Session("Flag") = "EditedAccounts" Then
                TabStripParticipantsInformation.SelectedIndex = 2
                MultiPageParticipantsInformation.SelectedIndex = 2
                LoadAdditionalAccountsTab()
                Session("Flag") = ""
                Session("icounter") = 0
            Else
                'Dim l_dataset_AddAccounts As DataSet
                'If Not viewstate("DS_Sort_Employment")Is Nothing Then
                '    l_dataset_AddAccounts = Session("l_dataset_AddAccount")
                '    If l_dataset_AddAccounts.Tables("AddAccountInfo").Rows.Count > 0 Then
                '        Me.DataGridAdditionalAccounts.DataSource = l_dataset_AddAccounts
                '        Me.DataGridAdditionalAccounts.DataBind()
                '    End If
                'End If
            End If
            If Session("Flag") = "AddBeneficiaries" Or Session("Flag") = "EditBeneficiaries" Or Session("Flag") = "EditedBeneficiaries" Then
                TabStripParticipantsInformation.SelectedIndex = 4
                MultiPageParticipantsInformation.SelectedIndex = 4
                'Priya				26-May-2012		YRS 5.0-1576: update marital status if spouse beneficiary entered
                If Not IsNothing(Session("MaritalStatus")) Then
                    DropDownListMaritalStatus.SelectedValue = Session("MaritalStatus").ToString().Trim
                    Session("MaritalStatus") = Nothing
                End If
                'End YRS 5.0-1576: update marital status if spouse beneficiary entered
                LoadBeneficiariesTab()
                Session("Flag") = ""
                Session("icounter") = 0
            End If
            If Session("Flag") = "AddNotes" Then
                TabStripParticipantsInformation.SelectedIndex = 5
                MultiPageParticipantsInformation.SelectedIndex = 5
                LoadNotesTab()
                Session("Flag") = ""
                'Start: Bala: 01/12/2016: YRS-AT-1718: Add Notes.
                ''by aparna YREN-3115 
                'Me.ButtonView.Enabled = False
                'ButtonSaveParticipant.Enabled = True
                'ButtonCancel.Enabled = True
                'ButtonOK.Enabled = False
                Me.ButtonView.Enabled = True
                ButtonSaveParticipant.Enabled = False
                ButtonCancel.Enabled = False
                ButtonOK.Enabled = True
                'End: Bala: 01/12/2016: YRS-AT-1718: Add Notes.
                Me.MakeLinkVisible()
            End If



            ''Added by prasad 2012.03.13 For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
            'If Not Session("Flag") Is Nothing Then
            '	If CType(Session("Flag"), String) = "Reactivate" Then
            '		If Request.Form("Yes") = "Yes" Then
            '			updateInfo()
            '			Session("Flag") = ""
            '		End If
            '	End If
            'End If
            'BS:2012.06.15:BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
            'BS:2012.07.18:YRS 5.0-1470:-Refresh Notes field After adding from address verify popup
            'Anudeep:24.06.2013 Error occured for when redirecting retiree information to particiapant information
            'If Session("Flag") = "AddNotesByAddressVerify" Then
            If Session("AddNotesByAddressVerify") = True Then
                LoadNotesTab()
                'Session("Flag") = ""
                'Anudeep:24.06.2013 Error occured for when redirecting retiree information to particiapant information
                'Me.ButtonView.Enabled = False
                'ButtonSaveParticipant.Enabled = True
                'ButtonCancel.Enabled = True
                'ButtonOK.Enabled = False
                'Me.MakeLinkVisible()
                'Anudeep:24.06.2013 Error occured for when redirecting retiree information to particiapant information
                Session("AddNotesByAddressVerify") = Nothing
            End If
            'Added by prasad 2012.03.13 For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
            If Not Session("Flag") Is Nothing Then
                'here check request for employee reactivation
                If CType(Session("Flag"), String) = "IsEmpReactivate" Then
                    If Request.Form("Yes") = "Yes" Then
                        'BS:2012.06.21:-reopen :BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
                        If CType(Session("IsTDContract"), String) = "True" Then
                            performTDValidation()
                            Exit Sub
                        Else
                            Dim empAdd As FetchEmploymentAdditional = EmpReactivation()
                            UpdateFundEventEmpElectives(empAdd.Termdate, empAdd.FundEventId, empAdd.YmcaId, False, empAdd.newFundStatusCode, empAdd.newFundStatusDesc)
                            Session("Flag") = ""
                            Exit Sub

                        End If

                    End If
                End If
                'here check request for Td contract reactivation
                If CType(Session("Flag"), String) = "IsTdReactivate" Then
                    If Request.Form("Yes") = "Yes" Then
                        Dim empAdd As FetchEmploymentAdditional = EmpReactivation()
                        UpdateFundEventEmpElectives(empAdd.Termdate, empAdd.FundEventId, empAdd.YmcaId, empAdd.tdcontractexist, empAdd.newFundStatusCode, empAdd.newFundStatusDesc)
                        Session("Flag") = ""
                        Exit Sub
                    End If
                    If Request.Form("No") = "No" Then
                        Dim empAdd As FetchEmploymentAdditional = EmpReactivation()
                        UpdateFundEventEmpElectives(empAdd.Termdate, empAdd.FundEventId, empAdd.YmcaId, False, empAdd.newFundStatusCode, empAdd.newFundStatusDesc)
                        Session("Flag") = ""
                        Exit Sub
                    End If
                End If
            End If
            'By Aparna -14/03/2007
            'If Me.NotesFlag.Value = "Notes" Then
            '    Me.TabStripParticipantsInformation.Items(5).Text = "<font color=orange>Notes</font>"
            'ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
            '    Me.TabStripParticipantsInformation.Items(5).Text = "<font color=red>Notes</font>"
            'Else
            '    Me.TabStripParticipantsInformation.Items(5).Text = "Notes"
            'End If
            'By Aparna -14/03/2007
            'To make death notification process work same as in Retirees Webform
            'Start of Change by Aparna 2/09/2007
            If CType(Session("DeathNotification"), Boolean) = True Then
                Dim Termdate As DateTime
                Dim Deathdate As DateTime
                Me.MultiPageParticipantsInformation.SelectedIndex = 0
                Me.TabStripParticipantsInformation.SelectedIndex = 0
                Dim l_string_FundStatusType As String = ""
                If Not Session("FundStatusType") Is Nothing Then
                    l_string_FundStatusType = DirectCast(Session("FundStatusType"), String)
                End If
                'NP:IVP1:2008.04.15 - Changes related to Phase IV - Part 1 changes, Prompt for ML status
                'Session("CurrentProcesstoConfirm") = "DeathProcess"
                ''removed top n left values to avoid mesaagebox breaking in IE7 -Aparna 04/10/2007
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you would like to enter a death date? Selecting Yes will launch death notification actions. Selecting No will cancel this action.", MessageBoxButtons.YesNo)
                'NP:YRS-5.0-464:2008.07.09 - Include NP in the prompt and do not prompt if basic account balance is less than $10,000
                'SR:2009.12.18:YRS-5.0-865 - Logic changed - For ML and NP do not check the balance. If the date of death is earlier to the termination date then the person is considered to be DA. Else prompt for the message and obtain status from user.
                If (l_string_FundStatusType = "ML" Or l_string_FundStatusType = "NP" Or l_string_FundStatusType = "PEML") Then
                    If l_string_FundStatusType = "ML" Then 'The prompt about active employment should only appear if the status is ML. 

                        Session("CurrentProcesstoConfirm") = "DeathMLQuestion"

                        ''SR:2009.12.16 - YRS 5.0:865
                        If (Not Session("Termdate") Is Nothing) AndAlso (Not Session("Deathdate") Is Nothing AndAlso Session("Termdate").ToString() <> String.Empty AndAlso Session("Deathdate").ToString() <> String.Empty) Then
                            Termdate = CType(Session("Termdate"), DateTime)
                            Deathdate = CType(Session("Deathdate"), DateTime)
                        Else
                            Session("CurrentProcesstoConfirm") = ""
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Death notification cancelled. Unable to identify Termination/Death Date.", MessageBoxButtons.Stop)
                            Exit Sub
                        End If

                        If (Deathdate < Termdate) Then
                            Session("DeathMLQuestion") = "DA"
                            Session("CurrentProcesstoConfirm") = "DeathProcess"
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you would like to enter a death date? Selecting Yes will launch death notification actions. Selecting No will cancel this action.", MessageBoxButtons.YesNo)
                            Session("DeathNotification") = False
                            Exit Sub
                        Else
                            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Person was on Military Leave. Were they actively employed at the time of death?", MessageBoxButtons.YesNo)
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Was participant still on Military Leave at the time of death?", MessageBoxButtons.YesNo)
                        End If

                    Else 'If the person's status is NP, their deceased status goes directly to DI. They are not eligible for the $10,000 death benefit
                        Session("DeathMLQuestion") = "DI"   'NP:YRS-5.0-464:2008.07.09 - We have made the decision that this is going to be an inactive death.
                        Session("CurrentProcesstoConfirm") = "DeathProcess"
                        'removed top n left values to avoid mesaagebox breaking in IE7 -Aparna 04/10/2007
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you would like to enter a death date? Selecting Yes will launch death notification actions. Selecting No will cancel this action.", MessageBoxButtons.YesNo)  '' SR:2010.01.08 FOR GEMIN 865
                    End If
                Else
                    'This is not required. This variable is used to force final fund event status to change to either DA or DI and is only checked for NP and ML participants. For all others it is expected to be empty or null.  'Session("DeathMLQuestion") = "DI"   'NP:YRS-5.0-464:2008.07.09 - We have made the decision that this is going to be an inactive death.
                    'Shagufta:27 July 2011- For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance(Disable 'Save' and 'Cancel' button)
                    ButtonPHR.Enabled = False
                    ButtonOK.Enabled = False
                    'End:27 July 2011- For BT-752,YRS 5.0-1268
                    Session("CurrentProcesstoConfirm") = "DeathProcess"
                    'removed top n left values to avoid mesaagebox breaking in IE7 -Aparna 04/10/2007
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you would like to enter a death date? Selecting Yes will launch death notification actions. Selecting No will cancel this action.", MessageBoxButtons.YesNo)
                End If

                Session("DeathNotification") = False
                Exit Sub
            End If

            If Session("CurrentProcesstoConfirm") = "DeathMLQuestion" Then
                If Request.Form("Yes") = "Yes" Then
                    Session("DeathMLQuestion") = "DA"
                ElseIf Request.Form("No") = "No" Then
                    Session("DeathMLQuestion") = "DI"
                End If
                Session("CurrentProcesstoConfirm") = "DeathProcess"
                'removed top n left values to avoid mesaagebox breaking in IE7 -Aparna 04/10/2007
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Are you sure you would like to enter a death date? Selecting Yes will launch death notification actions. Selecting No will cancel this action.", MessageBoxButtons.YesNo)
                Exit Sub
            End If


            '-------------------------------------------------------------------------------------------------
            'Shashi Shekhar:2009.09.02 Ref:(PS:YMCA PS Data Archive.Doc) - Adding code to receive Confirmation for data retrieved on Get Archived Data Back click.
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

            If Session("CurrentProcesstoConfirm") = "DeathProcess" Then
                'Anudeep:28.02.2013:Resetting the session variable after completed
                Session("LastName") = Nothing
                Session("FirstName") = Nothing
                If Request.Form("Yes") = "Yes" Then

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
                    'removed top n left values to avoid mesaagebox breaking in IE7 -Aparna 04/10/2007
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Death notification cancelled.", MessageBoxButtons.OK)
                    Exit Sub
                End If
            End If

            'Start: Bala: 12/01/2016: YRS-AT-1718: Delete Notes.
            If Session("CurrentProcesstoConfirm") = "DeleteNotes" Then
                If Request.Form("Yes") = "Yes" Then
                    NotesManagement.DeleteNotes(Session("NotesToDelete"))
                    'Start:AA: 02.15.2016 YRS-AT-1718 Added to refresh the notes datagrid
                    Session("dtNotes") = Nothing
                    LoadNotesTab()
                    'End:AA: 02.15.2016 YRS-AT-1718 Added to refresh the notes datagrid
                    Session("NotesToDelete") = ""
                    Session("CurrentProcesstoConfirm") = ""
                ElseIf Request.Form("No") = "No" Then
                    Session("CurrentProcesstoConfirm") = ""
                    Exit Sub
                ElseIf Request.Form("Ok") = "Ok" Then
                    Session("CurrentProcesstoConfirm") = ""
                    Exit Sub
                End If
            End If
            'End: Bala: 12/01/2016: YRS-AT-1718: Delete Notes.
            'Start: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Added to verify whether the user action to restrict web account info
            If ViewState("chkNoWebAcctCreate_verify") IsNot Nothing Then
                If Request.Form("Yes") = "Yes" Then
                Else
                    chkNoWebAcctCreate.Checked = False
                End If
                ViewState("chkNoWebAcctCreate_verify") = Nothing
                Exit Sub
            End If
            'End: AA:2015.05.05 - BT:2824 : YRS 5.0-2499:Added to verify whether the user action to restrict web account info
            'If Session("Flag") = "Death" Then
            '    'DeathNotify()
            '    If DeathNotify() = False Then
            '        Session("Flag") = ""
            '        Exit Sub
            '    End If
            '    Session("Flag") = ""
            '    If Not Session("DeathDate") Is Nothing Then
            '        TextBoxDeceasedDate.Text = Session("DeathDate")
            '    End If

            'End If
            'End of Change by Aparna 12/09/2007

            If Session("POA") = True Then
                'BT 706 2011.07.18 bhavnaS not need to session display all active POA
                'TextBoxPOA.Text = Session("POAName")
                'BT 706
                LoadPoADetails()
                'Anudeep:30.07.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
                LoadNotesTab()
                Session("POA") = False
            End If
            'Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added initiate freeze or unfreeze process
            If Session("Freeze_Unfreeze_Process") IsNot Nothing Then
                If Request.Form("Yes") = "Yes" Then
                    If Session("Freeze_Unfreeze_Process") = "Freeze" Then
                        UpdateFreezeDetails(Convert.ToDecimal(txtDefaultAmt.Text), Convert.ToDecimal(txtPhantomInt.Text), DateTime.Now.ToString())
                    ElseIf Session("Freeze_Unfreeze_Process") = "Unfreeze" Then
                        UpdateFreezeDetails(0.0, 0.0, Nothing)
                    End If
                End If
                Session("Freeze_Unfreeze_Process") = Nothing
                Exit Sub
            End If
            'End: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added initiate freeze or unfreeze process
            'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.
            'Bhavna:Session comment of deathnotify function YRS 5.0-1432
            'If Request.Form("OK") = "OK" Then
            '	If Not Session("Call_FinalDeathNotificationUpdation") Is Nothing Then
            '		If Session("Call_FinalDeathNotificationUpdation") = "Yes" Then
            '			'Session("Call_FinalDeathNotificationUpdation") = Nothing
            '			'If FinalDeathNotificationUpdation() = False Then
            '			'    Exit Sub
            '			'End If
            '			''SR:2011.04.28 : YRS 5.0-1292: A warning msg should appear OK option for Non Retired Participant.Hence below code is added 
            '			If (Session("ParticipantDNMsgforDR") <> "") Then
            '				'MessageBox.Show(PlaceHolder1, "Note before you proceed.", Session("ParticipantDNMsgforDR") + " Do you wish to proceed?", MessageBoxButtons.YesNo) ''SR:2011.04.27 - YRS 5.0 1292 : OK button replaced by YES/NO button
            '				'BS:2012.02.20:BT-941,YRS 5.0-1432:- IsStatusDR set true to call ProcessReport() to open report: List_of_Annuity_Checks_Sent_to_Retirees_After_Death.rpt pass session value yes to perform deathnotification
            '				IsStatusDR = True
            '				Session("Call_FinalDeathNotificationUpdation") = "Yes"
            '				'Exit Sub
            '			Else
            '				Session("Call_FinalDeathNotificationUpdation") = Nothing
            '				If FinalDeathNotificationUpdation() = False Then
            '					Exit Sub
            '				End If
            '			End If
            '		End If
            '	End If
            'End If
            'Bhavna:Session comment of deathnotify function YRS 5.0-1432
            'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.

            ''YRS 5.0-1292: A warning msg should appear with YES/NO option for retied participant Hence OK option is no longer needed for finaldeathnotification.
            'Bhavna:Session comment of deathnotify function YRS 5.0-1432
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
            'Bhavna:Session comment of deathnotify function YRS 5.0-1432
            ''Ends here 

            'START: MMR | 2016.10.14 | YRS-AT-3063 | Commented existing code and added condition to avoid data to be saved on checkboxExhaustedDBSettle checkbox selection
            'If Request.Form("Yes") = "Yes" Then
            '    SaveInfo()
            'End If
            If Request.Form("Yes") = "Yes" Then
                If IsExhaustedDBRMDSettleOptionSelected = True Then
                    IsExhaustedDBRMDSettleOptionSelected = False
                Else
                    SaveInfo()
                End If
            End If
            'END: MMR | 2016.10.14 | YRS-AT-3063 | Commented existing code and added condition to avoid data to be saved on checkboxExhaustedDBSettle checkbox selection

            'Shubhrata Plan Split Changes
            If Session("PhonySSNo") = "Not_Phony_SSNo" Then
                Session("PhonySSNo") = Nothing
                Call SetControlFocus(Me.TextBoxSSNo)
            End If
            Me.MakeLinkVisible()
            'Shagufta:27 July 2011- For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance(Disable 'Save' and 'Cancel' button)
            ButtonPHR.Enabled = True
            ButtonOK.Enabled = True
            'End:27 July 2011- For BT-751,YRS 5.0-1268
            'Shubhrata Plan Split Changes

            'BS:2012.06.15:BT:991:YRS 5.0-1530:- set flag value into client side for display confirm and ok dialog box
            If Session("ReactivationStart") = "True" Then
                SetFlagEmpAndTdReactivation()
            End If
            'Start:Anudeep:04.10.2013: BT-2236:After modifying address save button gets disabled
            If AddressWebUserControl1.ChangesExist <> "" Or AddressWebUserControl2.ChangesExist <> "" Then
                If ButtonSaveParticipant.Visible Then
                    Me.ButtonSaveParticipant.Enabled = True
                Else
                    Me.ButtonSaveParticipants.Enabled = True
                End If
                Me.ButtonCancel.Enabled = True
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
                    Me.MultiPageParticipantsInformation.SelectedIndex = 4
                    Me.TabStripParticipantsInformation.SelectedIndex = 4
                    If Not Session("BenTableCellCollection") Is Nothing Then
                        DeleteActiveBeneficairy(CType(Session("BenTableCellCollection"), TableCellCollection), CType(Session("BenItemIndex"), Integer))
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Delete beneficiary reason added sucessfully.", MessageBoxButtons.OK)
                    End If
                ElseIf Session("Reason").ToString.ToLower = "no" Then
                    Me.MultiPageParticipantsInformation.SelectedIndex = 4
                    Me.TabStripParticipantsInformation.SelectedIndex = 4
                End If
                Session("Reason") = Nothing
                Session("BenTableCellCollection") = Nothing
                Session("BenItemIndex") = Nothing
            End If
            'End:AA:2013.10.23 - Bt-2264: below code and has been placed from ispostback condition

            'AA:2014.02.03 - BT:2292:YRS 5.0-2248 - To load PIN and fill in textbox
            LoadUserPINdetails()
            'START: MMR | 2016.10.12 | YRS-AT-3063 | To check and Uncheck checkboxExhaustedDBSettle based on yes/no option selcted in message box
            If IsExhaustedDBRMDSettleOptionSelected Then
                If Not Request.Form("Yes") Is Nothing AndAlso Convert.ToString(Request.Form("Yes")).ToUpper() = "YES" Then
                    ButtonSaveParticipant.Enabled = True
                    checkboxExhaustedDBSettle.Checked = True
                ElseIf Not Request.Form("No") Is Nothing AndAlso Convert.ToString(Request.Form("No")).ToUpper() = "NO" Then
                    checkboxExhaustedDBSettle.Checked = False
                End If
            End If
            'END: MMR | 2016.10.12 | YRS-AT-3063 | To check and Uncheck checkboxExhaustedDBSettle based on yes/no option selcted in message box
            CheckReadOnlyMode() 'Shilpa N | 03/28/2019 | YRS-AT-4248 | Calling the method to check read only.
        Catch sqlEx As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)
            'BS:2011.12.20:-not suppose to catch in sub method 
            ExceptionPolicy.HandleException(sqlEx, "Exception Policy")
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try


    End Sub



    Private Sub TabStripParticipantsInformation_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabStripParticipantsInformation.SelectedIndexChange
        Try
            Session("Participant_Sort") = Nothing
            ''Calling the load of other tabs as and when the index changes
            Me.MultiPageParticipantsInformation.SelectedIndex = Me.TabStripParticipantsInformation.SelectedIndex
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

    Private Sub btnSaveReason_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveReason.Click
        'Save pop data into session for reasons
    End Sub

    Private Sub btnCancelReason_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelReason.Click
        'close pop
    End Sub


    Private Sub ButtonEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click

        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEdit", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            ButtonEdit.Enabled = False
            '''' code added by Vartika on 19th Nov 2005
            Me.TextBoxFirst.ReadOnly = False
            Me.TextBoxLast.ReadOnly = False
            Me.TextBoxSuffix.ReadOnly = False
            Me.TextBoxMiddle.ReadOnly = False
            Me.DropDownListSal.Enabled = True
            ''''

            TextBoxFirst.Enabled = True
            TextBoxLast.Enabled = True
            TextBoxMiddle.Enabled = True
            DropDownListSal.Enabled = True
            TextBoxSuffix.Enabled = True

            ButtonSaveParticipant.Enabled = True
            ButtonOK.Enabled = False
            ButtonCancel.Enabled = True
            Me.MakeLinkVisible()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub
    'anita may 9th
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
                    'Response.Write(l_string_controlNames)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'anita may 9th
    Private Sub ButtonEditAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditAddress.Click
        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditAddress", Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            Dim l_datatable_ContactsInfo As DataTable
            Dim l_dataset_SecContactInfo As DataTable

            'End : YRS 5.0-940
            'start:BS:2012.04.26:YRS 5.0-1470: && restructure Address control
            'Me.TextBoxEffDate.Enabled = True
            'Me.TextBoxSecEffDate.Enabled = True
            'Ashutosh Patil as on 26-Mar-2007
            'YREN-3028,YREN-3029
            'TextBoxAddress1.ReadOnly = False
            'TextBoxAddress2.ReadOnly = False
            'TextBoxAddress3.ReadOnly = False
            'TextBoxCity.ReadOnly = False
            'DropdownlistState.Enabled = True
            'TextBoxZip.ReadOnly = False
            'DropdownlistCountry.Enabled = True
            btnEditPrimaryContact.Visible = True
            btnEditSecondaryContact.Visible = True

            Me.AddressWebUserControl1.MakeReadonly = False
            Me.AddressWebUserControl1.EnableControls = True
            'Added by Anudeep:12.03.2013-For Telephone Usercontrol
            'Me.TelephoneWebUserControl1.EditPrimaryContact_Enabled = True
            'Me.TelephoneWebUserControl2.EditSecondaryContact_Enabled = True
            btnEditPrimaryContact.Visible = True
            btnEditSecondaryContact.Visible = True
            ButtonEditAddress.Enabled = False
            ButtonActivateAsPrimary.Enabled = False
            'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Disabling deactivate button
            btnDeactivateSecondaryAddrs.Enabled = False
            'End - Manthan | 2016.02.24 | YRS-AT-2328 | Disabling deactivate button
            'Commented By Dinesh Contacts
            TextBoxTelephone.ReadOnly = False
            TextBoxHome.ReadOnly = False
            TextBoxFax.ReadOnly = False
            TextBoxMobile.ReadOnly = False
            TextBoxSecTelephone.ReadOnly = False
            TextBoxSecHome.ReadOnly = False
            TextBoxSecFax.ReadOnly = False
            TextBoxSecMobile.ReadOnly = False
            TextBoxEmailId.ReadOnly = False
            'Ashutosh Patil as on 26-Mar-2007
            'YREN-3028,YREN-3029
            'TextBoxAddress1.Enabled = True
            'TextBoxAddress2.Enabled = True
            'TextBoxAddress3.Enabled = True
            'TextBoxCity.Enabled = True
            'TextBoxZip.Enabled = True


            'Commented By Dinesh Kanojia Contacts

            TextBoxTelephone.Enabled = True
            TextBoxHome.Enabled = True
            TextBoxMobile.Enabled = True
            TextBoxFax.Enabled = True
            TextBoxSecTelephone.Enabled = True
            TextBoxSecHome.Enabled = True
            TextBoxSecMobile.Enabled = True
            TextBoxSecFax.Enabled = True

            TextBoxEmailId.Enabled = True
            LabelEmailId.Enabled = True
            tbPricontacts.Visible = True
            btnEditPrimaryContact.Visible = False
            Dim dt_PrimaryContact As DataTable = DirectCast(Session("dt_PrimaryContactInfo"), DataTable)
            RefreshPrimaryTelephoneDetail(dt_PrimaryContact)
            Me.TelephoneWebUserControl1.Visible = False
            'Ashutosh Patil as on 26-Mar-2007
            'YREN-3028,YREN-3029
            'Me.TextBoxSecAddress1.Enabled = True
            'Me.TextBoxSecAddress2.Enabled = True
            'Me.TextBoxSecAddress3.Enabled = True
            'Me.TextBoxSecCity.Enabled = True
            'Me.DropdownlistSecCountry.Enabled = True
            '''  Me.TextBoxSecEmail.Enabled = True
            'Me.DropdownlistSecState.Enabled = True
            'Me.TextBoxSecTelephone.Enabled = True
            'TextBoxSecHome.Enabled = True
            'TextBoxSecFax.Enabled = True
            'TextBoxSecMobile.Enabled = True
            'Ashutosh Patil as on 26-Mar-2007
            'YREN-3028,YREN-3029
            'Me.TextBoxSecZip.Enabled = True
            ButtonEditAddress.Enabled = False
            'By Ashutosh Patil as on 25-Apr-2007
            'YREN-3298
            'Me.AddressWebUserControl1.SetValidationsForPrimary()
            'Dim l_ds_SecondaryAddress As DataSet
            'If Not Session("Ds_SecondaryAddress") Is Nothing Then
            '	l_ds_SecondaryAddress = CType(Session("Ds_SecondaryAddress"), DataSet)
            '	'Secondary Address section starts
            '	If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
            '		Me.ButtonActivateAsPrimary.Enabled = True
            '	End If
            'End If

            Me.AddressWebUserControl2.MakeReadonly = False
            Me.AddressWebUserControl2.EnableControls = True
            'Me.CheckboxIsBadAddress.Enabled = True
            'Me.CheckboxSecIsBadAddress.Enabled = True

            Me.CheckboxBadEmail.Enabled = True
            ''Me.CheckboxSecBadEmail.Enabled = True
            ''Me.CheckboxSecTextOnly.Enabled = True
            ''Me.CheckboxSecUnsubscribe.Enabled = True
            Me.CheckboxTextOnly.Enabled = True
            Me.CheckboxUnsubscribe.Enabled = True
            'Me.TextBoxSecEffDate.Enabled = True
            'Me.TextBoxEffDate.Enabled = True
            Me.ButtonSaveParticipant.Enabled = True
            Me.ButtonSaveParticipant.Visible = False
            Me.ButtonSaveParticipants.Visible = True
            Me.ButtonSaveParticipants.Enabled = True
            Me.ButtonCancel.Enabled = True
            Me.MakeLinkVisible()
            'By Ashutosh Patil as on 25-Apr-2007
            'YREN-3298
            'Priya 06-Sep-2010: YRS 5.0-1126:Address update for non-US/non-Canadian address.
            'Call Me.AddressWebUserControl1.SetCountStZipCodeMandatoryOnSelection()
            ValidationSummaryParticipants.Enabled = False
            ''end:BS:2012.03.14:YRS 5.0-1470:BT:951:-
            'End 06-Sep-2010: YRS 5.0-1126
            'end:BS:2012.04.26:YRS 5.0-1470: && restructure Address control
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonEditDOB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditDOB.Click
        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditDOB", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            TextBoxDOB.Enabled = True
            ButtonEditDOB.Enabled = False
            ''PopcalendarDate.Enabled = True
            Me.MakeLinkVisible()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub ButtonEditSSno_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditSSno.Click
        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditSSno", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            'SP :2014.02.04 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -Start
            'TextBoxSSNo.Enabled = True
            'TextBoxSSNo.ReadOnly = False

            Dim popupScript As String = "<script language='javascript'>" & _
              "window.open('UpdateSSN.aspx?Name=person&Mode=EditSSN', 'CustomPopUp', " & _
              "'width=600, height=300, menubar=no, Resizable=No,top=70,left=120, scrollbars=yes')" & _
             "</script>"
            Page.RegisterStartupScript("PopupScript10", popupScript)
            'SP :2014.02.04 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN  -End
            ButtonOK.Enabled = False
            ' ButtonEditSSno.Enabled = False 'SP :2014.02.04 BT:2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN
            Me.MakeLinkVisible()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonPOA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPOA.Click
        Try
            'Added by neeraj on 01-dec-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity1 As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If checkSecurity1.Equals("True") Then
                'Anudeep:07.11.2013:BT:2190-YRS 5.0-2199: Commented below line and added another line to get view acess to POA in read-only mode
                'Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonPOA", Convert.ToInt32(Session("LoggedUserKey")))
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonPOA", Convert.ToInt32(Session("LoggedUserKey")), True)
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
            ''ButtonOK.Enabled = False
            'BS:2011.07.29:BT:911-no need to handle ButtonSaveParticipant,ButtonCancel  disable/enable because there is no criteria for saving into database ,it will handle on poa screen
            'ButtonSaveParticipant.Enabled = True
            'ButtonCancel.Enabled = True
            Session("POA") = True
            Me.MakeLinkVisible()
            Dim popupScript As String = "<script language='javascript'>" & _
                                             "window.open('RetireesPowerAttorneyWebForm.aspx', 'CustomPopUp', " & _
                                             "'width=790, height=620, menubar=no, Resizable=No,top=70,left=120, scrollbars=yes')" & _
                                            "</script>"
            Page.RegisterStartupScript("PopupScript10", popupScript)
        Catch sqlEx As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Sub ButtonDeathNotification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDeathNotification.Click
        Try
            'Shagufta:27 July 2011- For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance
            If (ButtonSaveParticipant.Enabled = True And ButtonCancel.Enabled = True) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please save your data before you proceed", MessageBoxButtons.OK, False)
                Exit Sub
            End If
            'End:27 July 2011- For BT-751,YRS 5.0-1268
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity1 As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If checkSecurity1.Equals("True") Then
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonDeathNotification", Convert.ToInt32(Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity1, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

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
                'ButtonOK.Enabled = False
                'End: BT-751,YRS 5.0-1268
                'Shagufta:27 July 2011- For BT-751,YRS 5.0-1268 : buttons disabled in Person Maintenance(Disable 'Save' and 'Cancel' button)
                'ButtonSaveParticipant.Enabled = True
                'ButtonCancel.Enabled = True
                'End:27 July 2011- For BT-751,YRS 5.0-1268
                ' Session("Flag") = "Death" commented by Aparna 27/09/2007
                Me.MakeLinkVisible()
                'BS:2012.03.16:For BT-1015:YRS 5.0-1557:-using Session passing Death date value into Death Notification Pop up if date exist but notification not process
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
                Session("LastName") = TextBoxLast.Text
                Session("FirstName") = TextBoxFirst.Text
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
    Private Sub ButtonAddEmployment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddEmployment.Click
        Try
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session
            Dim l_string_FundStatus As String
            l_string_FundStatus = Session("FundStatus")
            'Commented/Added by Swopna -BT-394-19May08-------start
            'If l_string_FundStatus <> "PRE-ELIGIBLE" Then
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Employment records can be added for Pre-Eligible Participants only.", MessageBoxButtons.OK)
            If l_string_FundStatus <> "PRE-ELIGIBLE" And l_string_FundStatus <> "RETIRED-REEMPLOY PE" Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Employment records can be added for Pre-Eligible/Retired ReEmployed Participants only.", MessageBoxButtons.OK)
                'Commented/Added by Swopna -BT-394-19May08-------end
            Else
                _icounter = Session("icounter")
                _icounter = _icounter + 1
                Session("icounter") = _icounter
                If _icounter = 1 Then
                    Session("Ymca") = "Metro"
                    Session("Flag") = "AddEmployment"
                    Dim msg1 As String = "<script language='javascript'>" & _
                    "window.open('AddEmployment.aspx','CustomPopUp_AddEmp', " & _
                    "'width=750, height=550, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                     "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", msg1)
                    End If
                Else
                    Me.DataGridParticipantEmployment.SelectedIndex = -1
                    'Vipul 01Feb06 Cache-Session
                    'Me.DataGridParticipantEmployment.DataSource = CType(Cache("l_dataset_Employment"), DataSet)
                    'viewstate("DS_Sort_Employment") = CType(Cache("l_dataset_Employment"), DataSet)
                    Me.DataGridParticipantEmployment.DataSource = DirectCast(Session("l_dataset_Employment"), DataSet)
                    ViewState("DS_Sort_Employment") = DirectCast(Session("l_dataset_Employment"), DataSet)
                    'Vipul 01Feb06 Cache-Session

                    Me.DataGridParticipantEmployment.DataBind()
                    _icounter = 0
                    Session("icounter") = _icounter
                End If
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
    Private Sub ButtonAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddItem.Click
        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAddItem", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
            Dim i As Integer
            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            Dim l_bool_ActiveFlag As Boolean
            l_bool_ActiveFlag = False
            If Me.DataGridParticipantEmployment.Items.Count = 0 Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Participant has no active employment at a YMCA.", MessageBoxButtons.OK)
            Else
                For i = 0 To Me.DataGridParticipantEmployment.Items.Count - 1
                    If Me.DataGridParticipantEmployment.Items(i).Cells(m_const_int_Active).Text.ToUpper.Trim <> "INACTIVE" Then 'If Me.DataGridParticipantEmployment.Items(i).Cells(11).Text.ToUpper.Trim <> "INACTIVE" Then
                        l_bool_ActiveFlag = True
                        Exit For
                    End If
                Next
                'Ashutosh Patil as on 30-Mar-2007
                'YREN-3203,YREN-3205
                Dim l_dataset_AddAccoounts As DataSet
                Dim l_datatable_AddAccoount As DataTable
                Dim l_date As DateTime
                l_dataset_AddAccoounts = Session("l_dataset_Employment")
                l_datatable_AddAccoount = l_dataset_AddAccoounts.Tables(0).Copy
                'Hire Date
                l_date = Me.GetDate(l_datatable_AddAccoount, "HireDate")
                Session("HireDate") = DateValue(Month(l_date) & "/1/" & Year(l_date))
                'Enrollment Date
                l_date = Me.GetDate(l_datatable_AddAccoount, "BasicPaymentDate")
                Session("EnrollmentDate") = l_date
                Session("EmploymentAccountDataTable") = l_datatable_AddAccoount
                If l_bool_ActiveFlag Then
                    _icounter = Session("icounter")
                    _icounter = _icounter + 1
                    Session("icounter") = _icounter
                    If _icounter = 1 Then
                        Session("Flag") = "AddAccounts"
                        Dim popupScript As String = "<script language='javascript'>" & _
                            "window.open('AddAdditionalaccounts.aspx', 'CustomPopUp', " & _
                            "'width=800, height=450, menubar=no, Resizable=NO,top=120,left=120, scrollbars=yes')" & _
                            "</script>"
                        If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                            Page.RegisterStartupScript("PopupScript2", popupScript)
                        End If
                    Else
                        Me.DataGridAdditionalAccounts.SelectedIndex = -1
                        'Vipul 01Feb06 Cache-Session
                        'Me.DataGridAdditionalAccounts.DataSource = CType(Cache("l_dataset_AddAccount"), DataSet)
                        'viewstate("DS_Sort_AddnlAccts") = CType(Cache("l_dataset_AddAccount"), DataSet)
                        Me.DataGridAdditionalAccounts.DataSource = DirectCast(Session("l_dataset_AddAccount"), DataSet)
                        ViewState("DS_Sort_AddnlAccts") = DirectCast(Session("l_dataset_AddAccount"), DataSet)
                        'Vipul 01Feb06 Cache-Session
                        Me.DataGridAdditionalAccounts.DataBind()
                        _icounter = 0
                        Session("icounter") = _icounter
                    End If
                Else
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Participant has no active employment at a YMCA.", MessageBoxButtons.OK)
                End If
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
    Private Sub ButtonAddItemNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddItemNotes.Click
        Try
            'Start:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAddItemNotes", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
            'Start: Bala: YRS-AT-1718: 01/12/2016: Adding notes
            ''Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            'HiddenFieldDirty.Value = "true"
            'ButtonSaveParticipant.Enabled = True 
            'ButtonCancel.Enabled = True
            'ButtonOK.Enabled = False
            Session("NotesEntityID") = Session("PersId")
            'End: Bala: YRS-AT-1718: 01/12/2016: Adding notes
            Me.MakeLinkVisible()
            Session("Note") = ""
            ' Session("Flag") = "AddNotes"
            Dim popupScript As String = "<script language='javascript'>" & _
                                                    "window.open('UpdateNotes.aspx', 'CustomPopUp', " & _
                                                    "'width=800, height=400, menubar=no, Resizable=No,top=120,left=120, scrollbars=yes')" & _
                                                    "</script>"
            Page.RegisterStartupScript("PopupScript2", popupScript)
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
    Public Sub LoadEmploymentTab()
        Try
            Dim l_string_PersId As String
            Dim l_string_FundId As String
            'Vipul 01Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager()
            l_string_PersId = Session("PersId")
            l_string_FundId = Session("FundId")
            Dim l_dataset_Employment As DataSet
            Dim l_dataset_FundEvent As DataSet
            If Session("Flag") = "AddEmployment" Or Session("Flag") = "EditEmployment" Or Session("Flag") = "Edited" Then
                ' DataGridParticipantEmployment.SelectedIndex = -1
                'Vipul 01Feb06 Cache-Session
                'Me.DataGridParticipantEmployment.DataSource = Cache("l_dataset_Employment")
                'viewstate("DS_Sort_Employment") = cache("l_dataset_Employment")
                Me.DataGridParticipantEmployment.DataSource = Session("l_dataset_Employment")
                ViewState("DS_Sort_Employment") = Session("l_dataset_Employment")
                'Vipul 01Feb06 Cache-Session
                Me.DataGridParticipantEmployment.DataBind()
            Else
                l_dataset_Employment = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpEmploymentInfo(l_string_PersId, l_string_FundId)
                ' If Not l_dataset_Employment.Tables(0) Is Nothing Then
                'Vipul 01Feb06 Cache-Session()
                'Cache.Add("l_dataset_Employment", l_dataset_Employment)
                Session("l_dataset_Employment") = l_dataset_Employment
                'Vipul 01Feb06 Cache-Session
                If (l_dataset_Employment.Tables("EmploymentInfo").Rows.Count > 0) Then
                    DataGridParticipantEmployment.SelectedIndex = -1
                    Me.DataGridParticipantEmployment.DataSource = l_dataset_Employment
                    ViewState("DS_Sort_Employment") = l_dataset_Employment
                    Me.DataGridParticipantEmployment.DataBind()
                End If
                'Shubhrata July 14th YREN 2509
                Dim l_dataset_originaldate As DataSet
                Dim l_datatable_originaldate As DataTable
                Dim l_originaldate As DateTime
                l_dataset_originaldate = Session("l_dataset_Employment")
                l_datatable_originaldate = l_dataset_Employment.Tables(0).Copy
                'Commented By Ashutosh Patil as on 29-Mar-2007
                'YREN-3203,YREN-3205
                'l_orghiredate = Me.GetMinHigherDate(l_datatable_orghiredate)
                l_originaldate = Me.GetMinDate(l_datatable_originaldate)
                Session("l_orghiredate") = l_originaldate
                Session("MinHireDate") = l_originaldate
                Session("EmploymentDataTable") = l_datatable_originaldate
                'Shubhrata July 14th YREN 2509
                'Ashutosh Patil as on 30-Mar-2007
                'YREN-3203,YREN-3205
                Dim l_dataset_AddAccoounts As DataSet
                Dim l_datatable_AddAccoount As DataTable
                Dim l_date As DateTime
                l_dataset_AddAccoounts = Session("l_dataset_Employment")
                l_datatable_AddAccoount = l_dataset_AddAccoounts.Tables(0).Copy
                'Hire Date
                l_date = Me.GetDate(l_datatable_AddAccoount, "HireDate")
                Session("HireDate") = DateValue(Month(l_date) & "/1/" & Year(l_date))
                'Enrollment Date
                l_date = Me.GetDate(l_datatable_AddAccoount, "BasicPaymentDate")
                Session("EnrollmentDate") = l_date
                l_date = Me.GetMaxDate(l_datatable_AddAccoount, "HireDate")
                Session("MaxHireDate") = l_date
                Session("EmploymentAccountDataTable") = l_datatable_AddAccoount
                'Termdate  
                'SR:2009.12.16  For Gemini 865
                Session("TermDate") = GetMaxDate(l_datatable_originaldate, "TermDate")
                If Session("TermDate") = CType("1/1/1922", System.DateTime) Then
                    Session("TermDate") = String.Empty
                End If
                'Session("TermDate") = l_datatable_originaldate.Compute("MAX(TermDate)", "").ToString()
            End If
            l_dataset_FundEvent = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpFundEventInfo(l_string_PersId, l_string_FundId)
            'added by hafiz on 14-Nov-2006
            Session("FundEventInfo") = l_dataset_FundEvent
            Me.TextBoxMonth.Text = "0"
            Me.TextBoxYear.Text = "0"
            Me.TextBoxYearTotal.Text = "0"
            Me.TextBoxMonthTotal.Text = "0"
            Me.TextBoxVested.Text = "No"
            If l_dataset_FundEvent.Tables("FundEventInfo").Rows.Count > 0 Then
                If l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("Paid").ToString() <> "System.DBNull" And l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("Paid").ToString() <> "" Then
                    Me.TextBoxMonth.Text = Me.CalculateYearMonth("MONTH", Convert.ToInt32(l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("Paid")))
                    Me.TextBoxYear.Text = Me.CalculateYearMonth("YEAR", Convert.ToInt32(l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("Paid")))
                End If
                If l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("NonPaid").ToString() <> "System.DBNull" And l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("NonPaid").ToString() <> "" Then
                    Me.TextBoxMonthTotal.Text = Me.CalculateYearMonth("MONTH", Convert.ToInt32(l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("NonPaid")))
                    Me.TextBoxYearTotal.Text = Me.CalculateYearMonth("YEAR", Convert.ToInt32(l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("NonPaid")))
                End If
                'Commented by Shubhrata YREN 2703
                'If l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("VestingDate").ToString() = "System.DBNull" Or l_dataset_FundEvent.Tables("FundEventInfo").Rows(0).Item("VestingDate").ToString() = "" Then
                '    Me.TextBoxVested.Text = "No"
                'Else
                '    Me.TextBoxVested.Text = "Yes"
                'End If
            End If
            'Priya:19-July-2010: Integrate changes made on 10-June-2010 for YRS 5.0-1104:Showing non-vested person as vested
            Dim Isvested As Integer
            If Not IsNothing(Session("FundId")) Then
                Isvested = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.IsVested(Session("FundId"))
                If Isvested = 0 Then
                    Me.TextBoxVested.Text = "Yes"
                ElseIf Isvested = 1 Then
                    Me.TextBoxVested.Text = "No"
                End If
            End If
            ''Added By Shubhrata Sep 29th 2006 YREN 2703--to check whether a participant is vested or not
            'Dim l_isvested As Integer
            'If Not IsNothing(Session("FundId")) Then
            'End If
            'l_isvested = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.IsVested(l_string_PersId)
            'If l_isvested = 0 Then
            '    Me.TextBoxVested.Text = "Yes"
            'ElseIf l_isvested = 1 Then
            '    Me.TextBoxVested.Text = "No"
            'End If
            'End integration of YRS 5.0-1104

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
    '*************************************************************************
    'To Calculate The years and moths for Service Paid and Non Paid
    Function CalculateYearMonth(ByVal l_string_Type As String, ByVal l_integer_Months As Integer) As Integer
        l_string_Type = Trim(l_string_Type.ToUpper())
        Dim l_integer_ReturnValue As Integer

        If l_integer_Months >= 0 Then
            If l_string_Type = "YEAR" Then
                l_integer_ReturnValue = Convert.ToInt32(Floor(l_integer_Months / 12))
            Else
                l_integer_ReturnValue = Convert.ToInt32(l_integer_Months Mod 12)
            End If
        Else
            If l_string_Type = "YEAR" Then
                l_integer_ReturnValue = Convert.ToInt32(Floor(l_integer_Months / 12))
            Else
                l_integer_ReturnValue = Convert.ToInt32(l_integer_Months Mod -12)

            End If
        End If

        Return l_integer_ReturnValue
    End Function
    '*******************************************************************************************************
    'METHOD TO LOAD ADDITIONAL ACCOUNTS TAB
    Public Sub LoadAdditionalAccountsTab()
        Dim l_dataset_AddAccount As DataSet
        Dim l_string_PersId As String
        'Vipul 01Feb06 Cache-Session
        'Dim cache As CacheManager
        'cache = CacheFactory.GetCacheManager()
        Try
            l_string_PersId = Session("PersId")
            If Session("Flag") = "AddAccounts" Or Session("Flag") = "EditAccounts" Or Session("Flag") = "EditedAccounts" Then
                ' DataGridAdditionalAccounts.SelectedIndex = -1

                'Vipul 01Feb06 Cache-Session
                'Me.DataGridAdditionalAccounts.DataSource = Cache("l_dataset_AddAccount")
                'viewstate("DS_Sort_AddnlAccts") = Cache("l_dataset_AddAccount")
                Me.DataGridAdditionalAccounts.DataSource = Session("l_dataset_AddAccount")
                ViewState("DS_Sort_AddnlAccts") = Session("l_dataset_AddAccount")
                'Vipul 01Feb06 Cache-Session
                Me.DataGridAdditionalAccounts.DataBind()
                Me.DataGridAdditionalAccounts.SelectedIndex = -1
            Else
                l_dataset_AddAccount = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAddAccountInfo(l_string_PersId)
                ' If Not l_dataset_AddAccount.Tables(0) Is Nothing Then
                'Vipul 01Feb06 Cache-Session
                'Cache.Add("l_dataset_AddAccount", l_dataset_AddAccount)
                Session("l_dataset_AddAccount") = l_dataset_AddAccount
                'Vipul 01Feb06 Cache-Session
                'Priya 06-Oct-08 YRS 5.0-478 Commented "if" condition
                ' If l_dataset_AddAccount.Tables("AddAccountInfo").Rows.Count > 0 Then
                ' DataGridAdditionalAccounts.SelectedIndex = -1
                Me.DataGridAdditionalAccounts.DataSource = l_dataset_AddAccount
                ViewState("DS_Sort_AddnlAccts") = l_dataset_AddAccount
                Me.DataGridAdditionalAccounts.DataBind()
                'End If
                ' End If
                ''Ashutosh Patil as on 30-Mar-2007
                ''YREN-3203,YREN-3205
                'Dim l_dataset_AddAccoounts As DataSet
                'Dim l_datatable_AddAccoount As DataTable
                'Dim l_date As DateTime
                'l_dataset_AddAccoounts = Session("l_dataset_AddAccount")
                'l_datatable_AddAccoount = l_dataset_AddAccoounts.Tables(0).Copy
                ''Hire Date
                'l_date = Me.GetDate(l_datatable_AddAccoount, "HireDate")
                'Session("HireDate") = DateValue(Month(l_date) & "/1/" & Year(l_date))
                ''Enrollment Date
                'l_date = Me.GetDate(l_datatable_AddAccoount, "EnrollmentDate")
                'Session("EnrollmentDate") = l_date
                'Session("EmploymentAccountDataTable") = l_datatable_AddAccoount
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
    'Public Sub LoadAccountContributionsTab1(ByVal Sender As Object, ByVal e As EventArgs)
    '    Dim l_datetime_Startdate As Date = Convert.ToDateTime(TextBoxDate.Text)
    '    Dim l_dataset_AccountContributions As DataSet
    '    Dim l_string_PersId As String
    '    Dim l_integer_index As Integer
    '    Dim l_decimal_EmpTaxable As Decimal
    '    Dim l_decimal_EmpNonTaxable As Decimal
    '    Dim l_decimal_EmpInterest As Decimal
    '    Dim l_decimal_YmcaTaxable As Decimal
    '    Dim l_decimal_YmcaInterest As Decimal
    '    Dim l_string_FundEventId As String 'by Aparna 28/08/2007

    '    Try
    '        l_string_PersId = Session("PersId")
    '        'by Aparna 28/08/2007
    '        'If Me.RadioButtonList1.Items(0).Selected = True Then
    '        '    l_dataset_AccountContributions = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAccountContributionInfo("01/01/1900", l_datetime_Startdate, l_string_PersId, True)
    '        'Else
    '        '    l_dataset_AccountContributions = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAccountContributionInfo("01/01/1900", l_datetime_Startdate, l_string_PersId, False)
    '        'End If

    '        'To retrive the account details based on the fundeventId not on the Persid.
    '        'As a person can have more than one fundevents all the Accounts are coming up when selected through persid.
    '        l_string_FundEventId = Session("FundID")
    '        If Me.RadioButtonList1.Items(0).Selected = True Then
    '            l_dataset_AccountContributions = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAccountContributionInfo("01/01/1900", l_datetime_Startdate, l_string_FundEventId, True)
    '        Else
    '            l_dataset_AccountContributions = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAccountContributionInfo("01/01/1900", l_datetime_Startdate, l_string_FundEventId, False)
    '        End If
    '        'by Aparna 28/08/2007

    '        If Not l_dataset_AccountContributions Is Nothing Then
    '            'If Not l_dataset_AccountContributions.Tables(0) Is Nothing Then
    '            'DataGridPartAcctContribution.SelectedIndex = -1
    '            Me.DataGridPartAcctContribution.DataSource = l_dataset_AccountContributions
    '            viewstate("DS_Sort_AcctContrib") = l_dataset_AccountContributions
    '            Me.DataGridPartAcctContribution.DataBind()
    '            While l_integer_index < l_dataset_AccountContributions.Tables("AcctContInfo").Rows.Count
    '                l_decimal_EmpTaxable = l_decimal_EmpTaxable + Convert.ToDecimal(l_dataset_AccountContributions.Tables("AcctContInfo").Rows(l_integer_index).Item("EmpTaxable"))
    '                l_decimal_EmpNonTaxable = l_decimal_EmpNonTaxable + Convert.ToDecimal(l_dataset_AccountContributions.Tables("AcctContInfo").Rows(l_integer_index).Item("EmpNonTaxable"))
    '                l_decimal_EmpInterest = l_decimal_EmpInterest + Convert.ToDecimal(l_dataset_AccountContributions.Tables("AcctContInfo").Rows(l_integer_index).Item("EmpInterest"))
    '                l_decimal_YmcaInterest = l_decimal_YmcaInterest + Convert.ToDecimal(l_dataset_AccountContributions.Tables("AcctContInfo").Rows(l_integer_index).Item("YmcaInterest"))
    '                l_decimal_YmcaTaxable = l_decimal_YmcaTaxable + Convert.ToDecimal(l_dataset_AccountContributions.Tables("AcctContInfo").Rows(l_integer_index).Item("YmcaTaxable"))
    '                l_integer_index = l_integer_index + 1
    '            End While
    '            Dim str_EmpTaxable As String
    '            Dim str_EmpNonTaxable As String
    '            Dim str_EmpInterest As String
    '            Dim str_YmcaInterest As String
    '            Dim str_YmcaTaxable As String
    '            Dim str_PTotal As String
    '            Dim str_YTotal As String
    '            Dim str_total As String

    '            str_EmpTaxable = FormatCurrency(l_decimal_EmpTaxable)
    '            str_EmpNonTaxable = FormatCurrency(l_decimal_EmpNonTaxable)
    '            str_EmpInterest = FormatCurrency(l_decimal_EmpInterest)
    '            str_YmcaInterest = FormatCurrency(l_decimal_YmcaInterest)

    '            str_YmcaTaxable = FormatCurrency(l_decimal_YmcaTaxable)
    '            str_PTotal = FormatCurrency(l_decimal_EmpTaxable + l_decimal_EmpNonTaxable + l_decimal_EmpInterest)
    '            str_YTotal = FormatCurrency(l_decimal_YmcaTaxable + l_decimal_YmcaInterest)
    '            str_total = FormatCurrency(l_decimal_EmpTaxable + l_decimal_EmpNonTaxable + l_decimal_EmpInterest + l_decimal_YmcaTaxable + l_decimal_YmcaInterest)

    '            'Vipul 06March06 ... Replaced Textboxes for Totals with Grid (GridTotal)
    '            'TextBoxPTaxable.Text = str_EmpTaxable
    '            'TextBoxPInterest.Text = str_EmpInterest
    '            'TextBoxPNonTaxable.Text = str_EmpNonTaxable

    '            'TextBoxYTaxable.Text = str_YmcaTaxable
    '            'TextBoxYInterest.Text = str_YmcaInterest
    '            'TextBoxPTotal.Text = str_PTotal
    '            'TextBoxYTotal.Text = str_YTotal
    '            'TextBoxTotal.Text = str_total

    '            'Vipul 06March06 ... Replaced Textboxes for Totals with Grid (GridTotal)
    '            'The following code has been deleted from HTML 
    '            ' <tr>
    '            '	<td>
    '            '		<DIV  style="OVERFLOW: auto; WIDTH: 695px; HEIGHT: 50px; TEXT-ALIGN: left">
    '            '			<table  width="720" cellpadding="0" cellspacing="1">
    '            '				<tr>
    '            '					<td align="center">
    '            '						<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 140px; TEXT-ALIGN: left">
    '            '						</DIV>
    '            '					</td>
    '            '				</tr>
    '            '				<tr >
    '            '					<td class="Table_WithBorder"><asp:Label Runat="server" Width="80" CssClass="Label_Small" id="Label10">Total</asp:Label> </td>
    '            '					<td class="Table_WithBorder"><asp:TextBox Runat="server" Width="77" CssClass="TextBox_Normal_Amount" id="TextBoxPTaxable"
    '            '							textalign="right" ReadOnly="True"></asp:TextBox></td>
    '            '					<td class="Table_WithBorder"><asp:TextBox Runat="server" Width="86" CssClass="TextBox_Normal_Amount" id="TextBoxPNonTaxable"
    '            '							ReadOnly="True"></asp:TextBox></td>
    '            '					<td class="Table_WithBorder"><asp:TextBox Runat="server" Width="72" CssClass="TextBox_Normal_Amount" id="TextBoxPInterest"
    '            '							ReadOnly="True"></asp:TextBox></td>
    '            '					<td class="Table_WithBorder"><asp:TextBox Runat="server" Width="54" CssClass="TextBox_Normal_Amount" id="TextBoxPTotal" ReadOnly="True"></asp:TextBox></td>
    '            '					<td class="Table_WithBorder"><asp:TextBox Runat="server" Width="80" CssClass="TextBox_Normal_Amount" id="TextBoxYTaxable"
    '            '							ReadOnly="True"></asp:TextBox></td>
    '            '					<td class="Table_WithBorder"><asp:TextBox Runat="server" Width="82" CssClass="TextBox_Normal_Amount" id="TextBoxYInterest"
    '            '							ReadOnly="True"></asp:TextBox></td>
    '            '					<td class="Table_WithBorder"><asp:TextBox Runat="server" Width="63" CssClass="TextBox_Normal_Amount" id="TextBoxYTotal" ReadOnly="True"></asp:TextBox></td>
    '            '					<td class="Table_WithBorder"><asp:TextBox Runat="server" Width="62" CssClass="TextBox_Normal_Amount" id="TextBoxTotal" ReadOnly="True"></asp:TextBox></td>
    '            '				</tr>
    '            '			</table>
    '            '		</DIV>
    '            '	</td>
    '            '</tr>
    '            'Vipul 06March06 ... Replaced Textboxes for Totals with Grid (GridTotal)


    '            'Vipul 03Mar06 Fixing the Totals truncate problem Issue # 
    '            Dim l_datatable_AccountTotals As New DataTable
    '            l_datatable_AccountTotals = l_dataset_AccountContributions.Tables("AcctContInfo").Clone

    '            l_datatable_AccountTotals.Columns(1).DataType = GetType(System.String)
    '            l_datatable_AccountTotals.Columns(2).DataType = GetType(System.String)
    '            l_datatable_AccountTotals.Columns(3).DataType = GetType(System.String)
    '            l_datatable_AccountTotals.Columns(4).DataType = GetType(System.String)
    '            l_datatable_AccountTotals.Columns(5).DataType = GetType(System.String)
    '            l_datatable_AccountTotals.Columns(6).DataType = GetType(System.String)
    '            l_datatable_AccountTotals.Columns(7).DataType = GetType(System.String)
    '            l_datatable_AccountTotals.Columns(8).DataType = GetType(System.String)


    '            Dim l_DataRowtotals As DataRow
    '            l_DataRowtotals = l_datatable_AccountTotals.NewRow


    '            l_DataRowtotals.Item(1) = str_EmpTaxable
    '            l_DataRowtotals.Item(2) = str_EmpNonTaxable
    '            l_DataRowtotals.Item(3) = str_EmpInterest

    '            l_DataRowtotals.Item(4) = str_PTotal
    '            l_DataRowtotals.Item(5) = str_YmcaTaxable
    '            l_DataRowtotals.Item(6) = str_YmcaInterest

    '            l_DataRowtotals.Item(7) = str_YTotal
    '            l_DataRowtotals.Item(8) = str_total

    '            l_datatable_AccountTotals.Rows.Add(l_DataRowtotals)

    '            Me.DatagridTotal.DataSource = l_datatable_AccountTotals
    '            Me.DatagridTotal.DataBind()
    '            'Vipul 03Mar06 Fixing the Totals truncate problem Issue # 
    '            ' End If
    '        End If
    '    Catch sqlEx As SqlException

    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)




    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    'by Aparna 28/08/2007
    Public Sub LoadAccountContributionsTab(ByVal Sender As Object, ByVal e As EventArgs)
        Dim l_datetime_Startdate As Date
        Dim l_dataset_AccountContributions As DataSet
        Dim l_string_PersId As String
        Dim l_integer_index As Integer
        Dim l_decimal_EmpTaxable As Decimal
        Dim l_decimal_EmpNonTaxable As Decimal
        Dim l_decimal_EmpInterest As Decimal
        Dim l_decimal_YmcaTaxable As Decimal
        Dim l_decimal_YmcaInterest As Decimal
        Dim l_string_FundEventId As String 'by Aparna 28/08/2007
        Dim l_Datatable_AcctContributionSavings As New DataTable
        Dim l_Datatable_AcctContributionRetirement As New DataTable
        Dim l_Datatable_AcctSavingsTotal As New DataTable
        Dim l_datarowarray_RetirementPlan As DataRow()
        Dim l_datarowarray_SavingsPlan As DataRow()
        Dim i As Integer
        Dim l_String_PlanType As String = ""
        Dim l_datatable_RetirementPlanTotal As DataTable
        Dim l_datatable_SavingsPlanTotal As DataTable
        Dim l_datatable_GrandTotal As New DataTable
        Try

            '-----------------------------------------------------------------------------------------------
            'Shashi Shekhar:2009.08.28 Ref:(PS:YMCA PS Data Archive.Doc) - Adding code to change Account Contribution header in case if person's data has been archived.
            If g_isArchived = True Then
                LabelAccContHdr.Text = " - Transaction & Disbursement data have been archived."
            Else
                LabelAccContHdr.Text = ""
            End If
            '------------------------------------------------------------------------------------------------

            l_datetime_Startdate = Convert.ToDateTime(TextBoxDate.Text)
            l_string_PersId = Session("PersId")

            'To retrive the account details based on the fundeventId not on the Persid.
            'As a person can have more than one fundevents all the Accounts are coming up when selected through persid.
            l_string_FundEventId = Session("FundID")
            If Me.RadioButtonList1.Items(0).Selected = True Then
                l_dataset_AccountContributions = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAccountContributionInfo("01/01/1900", l_datetime_Startdate, l_string_FundEventId, True)
            Else
                l_dataset_AccountContributions = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAccountContributionInfo("01/01/1900", l_datetime_Startdate, l_string_FundEventId, False)
            End If

            If Not l_dataset_AccountContributions Is Nothing Then

                ' The DataTable for Retirement Plan is being populated here 
                l_Datatable_AcctContributionRetirement = l_dataset_AccountContributions.Tables("AccountContributions").Clone()
                'by aparna
                l_datatable_GrandTotal = l_dataset_AccountContributions.Tables("AccountContributions").Clone()
                l_datarowarray_RetirementPlan = l_dataset_AccountContributions.Tables("AccountContributions").Select("PlanType = 'RETIREMENT' OR PlanType IS NULL")    'NP:IVP2:2008.09.10 - Adding condition for NULL also in this check since those are to be treated as Retirement plan by default
                If l_datarowarray_RetirementPlan.Length > 0 Then
                    For i = 0 To l_datarowarray_RetirementPlan.Length - 1
                        l_Datatable_AcctContributionRetirement.ImportRow(l_datarowarray_RetirementPlan(i))
                    Next
                Else
                    Dim drow_dummy_retirementplan As DataRow
                    drow_dummy_retirementplan = l_Datatable_AcctContributionRetirement.NewRow()
                    l_Datatable_AcctContributionRetirement.Rows.Add(drow_dummy_retirementplan)
                End If
                Session("RetirementPlanAcctContribution") = l_Datatable_AcctContributionRetirement

                'The DataTable for Savings Plan(the funded accout contributions) is being populated here 
                l_Datatable_AcctContributionSavings = l_dataset_AccountContributions.Tables("AccountContributions").Clone()
                l_datarowarray_SavingsPlan = l_dataset_AccountContributions.Tables("AccountContributions").Select("PlanType = 'SAVINGS'")
                If l_datarowarray_SavingsPlan.Length > 0 Then
                    For i = 0 To l_datarowarray_SavingsPlan.Length - 1
                        l_Datatable_AcctContributionSavings.ImportRow(l_datarowarray_SavingsPlan(i))
                    Next
                Else
                    Dim drow_dummy_SavingsPlan As DataRow
                    drow_dummy_SavingsPlan = l_Datatable_AcctContributionSavings.NewRow()
                    l_Datatable_AcctContributionSavings.Rows.Add(drow_dummy_SavingsPlan)
                End If

                'Now Add the Totals to the respective datatables and bind them to the Corresponding datagrids

                l_datatable_RetirementPlanTotal = l_dataset_AccountContributions.Tables("AcctContributionsTotal").Clone
                l_datatable_SavingsPlanTotal = l_dataset_AccountContributions.Tables("AcctContributionsTotal").Clone
                For Each drow As DataRow In l_dataset_AccountContributions.Tables("AcctContributionsTotal").Rows
                    If Not IsDBNull(drow("PlanType")) Then
                        l_String_PlanType = drow("PlanType")
                    End If
                    If l_String_PlanType <> "" Then
                        Select Case (l_String_PlanType.Trim.ToUpper)
                            Case "RETIREMENT"
                                l_datatable_RetirementPlanTotal.ImportRow(drow)
                            Case "SAVINGS"
                                l_datatable_SavingsPlanTotal.ImportRow(drow)
                        End Select
                    End If
                Next

                'Bind the datagrids
                'retirement plan funded account contribution data grid 

                Me.DataGridRetirementAccntContributions.DataSource = AddTotalIntoTable(l_Datatable_AcctContributionRetirement, l_datatable_RetirementPlanTotal)
                Me.DataGridRetirementAccntContributions.DataBind()

                Me.DataGridSavingAccntContributions.DataSource = AddTotalIntoTable(l_Datatable_AcctContributionSavings, l_datatable_SavingsPlanTotal)
                Me.DataGridSavingAccntContributions.DataBind()

                Me.DatagridAcctTotal.DataSource = l_dataset_AccountContributions.Tables("AcctContributionsGrandTotal")
                Me.DatagridAcctTotal.DataBind()


                Me.SetSelectedIndex(Me.DataGridRetirementAccntContributions, l_Datatable_AcctContributionRetirement)
                Me.SetSelectedIndex(Me.DataGridSavingAccntContributions, l_Datatable_AcctContributionSavings)

                Me.SetSelectedIndex(Me.DatagridAcctTotal, l_dataset_AccountContributions.Tables("AcctContributionsGrandTotal"))
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function AddTotalIntoTable(ByVal paramaterSourceDataTable As DataTable, ByVal parameterTotalDataTable As DataTable) As DataTable

        Dim l_DataRow As DataRow
        Dim l_TotalDataRow As DataRow

        Try
            If parameterTotalDataTable Is Nothing Then Return paramaterSourceDataTable

            If Not paramaterSourceDataTable Is Nothing Then

                '' Add a empty row in to DataTable

                l_DataRow = paramaterSourceDataTable.NewRow
                paramaterSourceDataTable.Rows.Add(l_DataRow)

                l_DataRow = paramaterSourceDataTable.NewRow
                paramaterSourceDataTable.Rows.Add(l_DataRow)

                'Shubhrata May22nd,2007 
                If parameterTotalDataTable.Rows.Count > 0 Then
                    l_TotalDataRow = parameterTotalDataTable.Rows.Item(0)
                End If


                l_DataRow = paramaterSourceDataTable.NewRow

                If Not l_TotalDataRow Is Nothing Then

                    l_DataRow("Acct") = "Total"
                    '  l_DataRow("YmcaID") = "0"

                    If (l_TotalDataRow("EmpTaxable").GetType.ToString = "System.DBNull") Then
                        l_DataRow("EmpTaxable") = "0.00"
                    Else
                        l_DataRow("EmpTaxable") = l_TotalDataRow("EmpTaxable")
                    End If

                    If (l_TotalDataRow("EmpNonTaxable").GetType.ToString = "System.DBNull") Then
                        l_DataRow("EmpNonTaxable") = "0.00"
                    Else
                        l_DataRow("EmpNonTaxable") = l_TotalDataRow("EmpNonTaxable")
                    End If

                    If (l_TotalDataRow("EmpInterest").GetType.ToString = "System.DBNull") Then
                        l_DataRow("EmpInterest") = "0.00"
                    Else
                        l_DataRow("EmpInterest") = l_TotalDataRow("EmpInterest")
                    End If

                    If (l_TotalDataRow("EmpTotal").GetType.ToString = "System.DBNull") Then
                        l_DataRow("EmpTotal") = "0.00"
                    Else
                        l_DataRow("EmpTotal") = l_TotalDataRow("EmpTotal")
                    End If

                    If (l_TotalDataRow("YMCATaxable").GetType.ToString = "System.DBNull") Then
                        l_DataRow("YMCATaxable") = "0.00"
                    Else
                        l_DataRow("YMCATaxable") = l_TotalDataRow("YMCATaxable")
                    End If

                    If (l_TotalDataRow("YMCAInterest").GetType.ToString = "System.DBNull") Then
                        l_DataRow("YMCAInterest") = "0.00"
                    Else
                        l_DataRow("YMCAInterest") = l_TotalDataRow("YMCAInterest")
                    End If

                    If (l_TotalDataRow("YMCATotal").GetType.ToString = "System.DBNull") Then
                        l_DataRow("YMCATotal") = "0.00"
                    Else
                        l_DataRow("YMCATotal") = l_TotalDataRow("YMCATotal")
                    End If

                    If (l_TotalDataRow("AcctTotal").GetType.ToString = "System.DBNull") Then
                        l_DataRow("AcctTotal") = "0.00"
                    Else
                        l_DataRow("AcctTotal") = l_TotalDataRow("AcctTotal")
                    End If

                Else
                    l_DataRow("Acct") = "Total"
                    ' l_DataRow("YmcaID") = "0"
                    l_DataRow("EmpTaxable") = "0.00"
                    l_DataRow("EmpNonTaxable") = "0.00"
                    l_DataRow("EmpInterest") = "0.00"
                    l_DataRow("EmpTotal") = "0.00"
                    l_DataRow("YMCATaxable") = "0.00"
                    l_DataRow("YMCAInterest") = "0.00"
                    l_DataRow("YMCATotal") = "0.00"
                    l_DataRow("AcctTotal") = "0.00"

                End If
                paramaterSourceDataTable.Rows.Add(l_DataRow)
            End If

            Return paramaterSourceDataTable

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Function

    Private Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        LoadAccountContributionsTab(sender, e)
    End Sub

    Private Sub TextBoxDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxDate.TextChanged
        'Added by Swopna -BT-403---start
        If TextBoxDate.Text = String.Empty Then
            TextBoxDate.Text = System.DateTime.Today()
        End If
        'Added by Swopna -BT-403---end
        LoadAccountContributionsTab(sender, e)
    End Sub
    Private Function ProcessPhonySSNo() As String
        'Added By Ashutosh Patil
        'YREN-3490
        Dim l_string_Message As String
        Try
            If Me.ButtonEditAddress.Enabled = False Then
                ProcessPhonySSNo = "NotValidated"




                Call SetPrimaryAddressDetails()
                'Secondary Address
                Call SetSecondaryAddressDetails()
                'Priya 
                l_string_Message = AddressWebUserControl1.ValidateAddress()
                If l_string_Message <> "" Then
                    Me.ButtonSaveParticipant.Visible = False
                    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                    Exit Function
                End If
                l_string_Message = AddressWebUserControl2.ValidateAddress()
                If l_string_Message <> "" Then
                    Me.ButtonSaveParticipant.Visible = False
                    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                    Exit Function
                End If

                'Added By Ashutosh Patil as on 23-Feb-2007 For YREN - 3029,YREN - 3028
                'If Trim(m_str_Pri_Address1) = "" Then
                '    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Primary Address.", MessageBoxButtons.Stop)
                '    Me.ButtonSaveParticipant.Visible = False
                '    Exit Function
                'End If

                'If Trim(m_str_Pri_City) = "" Then
                '    MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Primary Address.", MessageBoxButtons.Stop)
                '    Me.ButtonSaveParticipant.Visible = False
                '    Exit Function
                'End If

                'If Trim(m_str_Pri_Address1) <> "" Then
                '    If m_str_Pri_CountryText = "-Select Country-" And m_str_Pri_StateText = "-Select State-" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
                '        Me.ButtonSaveParticipant.Visible = False
                '        Exit Function
                '    End If
                'End If

                'If (m_str_Pri_CountryText = "-Select Country-") Then
                '    If m_str_Pri_StateText = "-Select State-" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop)
                '        Me.ButtonSaveParticipant.Visible = False
                '        Exit Function
                '    End If
                'End If

                'l_string_Message = ValidateCountrySelStateZip(m_str_Pri_CountryValue, m_str_Pri_StateValue, m_str_Pri_Zip)
                'If l_string_Message <> "" Then
                '    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                '    Me.ButtonSaveParticipant.Visible = False
                '    Exit Function
                'End If

                'If m_str_Pri_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Pri_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
                '    MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
                '    Me.ButtonSaveParticipant.Visible = False
                '    Exit Function
                'End If

                'Secondary Address Validations
                'Ashutosh Patil as on 14-Mar-2007
                'YREN - 3028,YREN-3029
                'If Trim(m_str_Sec_Address1) <> "" Then
                '    If Trim(m_str_Sec_City) = "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please enter City for Secondary Address.", MessageBoxButtons.Stop)
                '        Me.ButtonSaveParticipant.Visible = False
                '        Exit Function
                '    End If
                '    If (m_str_Sec_CountryText = "-Select Country-") Then
                '        If m_str_Sec_StateText = "-Select State-" Then
                '            MessageBox.Show(PlaceHolder1, "YMCA", "Please select either State or Country for Secondary Address.", MessageBoxButtons.Stop)
                '            Me.ButtonSaveParticipant.Visible = False
                '            Exit Function
                '        End If
                '    End If
                'End If

                'If Trim(m_str_Sec_Address2) <> "" Then
                '    If Trim(m_str_Sec_Address1) = "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '        Me.ButtonSaveParticipant.Visible = False
                '        Exit Function
                '    End If
                'End If

                'If Trim(m_str_Sec_Address3) <> "" Then
                '    If Trim(m_str_Sec_Address1) = "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '        Me.ButtonSaveParticipant.Visible = False
                '        Exit Function
                '    End If
                'End If

                'If m_str_Sec_City <> "" Then
                '    If Trim(m_str_Sec_Address1.ToString) = "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '        Me.ButtonSaveParticipant.Visible = False
                '        Exit Function
                '    End If
                'End If

                'If (m_str_Sec_CountryText <> "-Select Country-" Or m_str_Sec_StateText <> "-Select State-") Then
                '    If Trim(m_str_Sec_Address1) = "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '        Me.ButtonSaveParticipant.Visible = False
                '        Exit Function
                '    End If
                'End If

                'If Trim(m_str_Sec_Zip) <> "" Then
                '    If Trim(m_str_Sec_Address1) = "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)
                '        Me.ButtonSaveParticipant.Visible = False
                '        Exit Function
                '    End If
                'End If

                'l_string_Message = ValidateCountrySelStateZip(m_str_Sec_CountryValue, m_str_Sec_StateValue, m_str_Sec_Zip)

                'If l_string_Message <> "" Then
                '    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                '    Me.ButtonSaveParticipant.Visible = False
                '    Exit Function
                'End If

                'If m_str_Sec_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Sec_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
                '    MessageBox.Show(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop)
                '    Me.ButtonSaveParticipant.Visible = False
                '    Exit Function
                'End If
            End If

            ProcessPhonySSNo = "Validated"

        Catch ex As Exception
            Throw
        End Try
    End Function
    'BS:2012.01.20:YRS 5.0-1497 -Add edit button to modify the date of death--This method is validate updated death date value if date is in valid format ,updated death date has to be come with in original death month and year on server side same as done on client side
    Private Function IsValidDeathDate() As String
        Dim CurrentDeathDate, NewDeathDate As DateTime
        Dim CurrentMonth, NewMonth, CurrentYear, NewYear As String
        Dim l_date As Date
        If (HiddenFieldDeathDate.Value = String.Empty OrElse TextBoxDeceasedDate.Text = String.Empty) Then
            HelperFunctions.LogMessage( _
             String.Format("Error Occur While Editing Death Date Value {0}", HiddenFieldDeathDate.Value.ToString()))
            Return "Internal Error : Please contact the IT support"
        End If
        If IsDate(HiddenFieldDeathDate.Value.ToString()) = False Then
            HelperFunctions.LogMessage( _
             String.Format("Error Find in IsValidDeathDate() Invalid Date format of Original Death Date {0}", HiddenFieldDeathDate.Value.ToString()))
            Return "Internal Error : Please contact the IT support"
        End If

        If IsDate(TextBoxDeceasedDate.Text.ToString()) = False Then
            HelperFunctions.LogMessage( _
            String.Format("Error Find in IsValidDeathDate() Invalid Date format of Updated Death Date {0}", TextBoxDeceasedDate.Text.ToString()))
            Return False
        End If
        l_date = System.DateTime.Today
        If TextBoxDeceasedDate.Text <> String.Empty Then
            If (Date.Compare(Convert.ToDateTime(TextBoxDeceasedDate.Text), l_date) > 0) Then
                Return "The Deceased Date cannot be greater than today's date."
            End If
        End If
        CurrentDeathDate = Convert.ToDateTime(HiddenFieldDeathDate.Value.ToString())
        NewDeathDate = Convert.ToDateTime(TextBoxDeceasedDate.Text.ToString())

        CurrentMonth = CurrentDeathDate.Month()
        CurrentYear = CurrentDeathDate.Year()

        NewMonth = NewDeathDate.Month()
        NewYear = NewDeathDate.Year()

        If (CurrentMonth <> NewMonth OrElse CurrentYear <> NewYear) Then
            Return "The new death date must be in the same month and year of the existing date of death " + CurrentDeathDate + ""
        End If
    End Function

    Private Sub ButtonSaveParticipant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSaveParticipant.Click
        ValidationSummaryParticipants.Enabled = True 'Added by shashi
        Dim l_string_Message As String = ""
        Dim dtTelephone As New DataTable
        Dim drTelephone As DataRow
        Dim stTelephoneErrors As String 'PPP | 2015.10.10 | YRS-AT-2588
        Try

            Me.MakeLinkVisible()
            ButtonEdit.Enabled = True
            ButtonEditAddress.Enabled = True
            ButtonEditDOB.Enabled = True
            ButtonEditSSno.Enabled = True
            'btnAcctLockEdit.Enabled = True
            'BS:2012.01.17:YRS 5.0-1497 -Add edit button to modify the date of death :--Call IsValidDeathDate() return message
            If (HiddenFieldDeathDate.Value <> String.Empty) Then
                Dim strVerifyOutputMessage As String = IsValidDeathDate()
                If strVerifyOutputMessage <> Nothing OrElse strVerifyOutputMessage <> String.Empty Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", strVerifyOutputMessage, MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If


            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim controlsecurity As String = SecurityCheck.Check_Authorization("ButtonSaveParticipant", Convert.ToInt32(Session("LoggedUserKey")))

            If Not controlsecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", controlsecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If


            If Me.IsValid Then
                Dim l_DateInFuture As Date
                Dim l_DateInPast As Date
                Dim l_PrimDate As Date
                Dim l_SecDate As Date
                Dim l_PrimaryDate As Date
                Dim l_dr_PrimaryAddress As DataRow()
                Dim l_dr_SecondaryAddress As DataRow()
                Dim l_SecondaryDate As Date


                'Start-SR:2013.11.19 : YRS 5.0-2165 : RMD Tax Rate should be greater then 10%

                'Start:  Dinesh k           2014.05.15      BT-2537 : YRS 5.0-2370 - Change to validation on RMD tax rate 
                'If TextBoxRMDTax.Text.Trim <> String.Empty Then
                '    If Convert.ToDecimal(TextBoxRMDTax.Text.Trim) < 10.0 Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "RMD Tax Rate cannot be less then 10.", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'End If

                If TextBoxRMDTax.Text.Trim <> String.Empty Then
                    If Convert.ToDecimal(TextBoxRMDTax.Text.Trim) < 0.0 Or Convert.ToDecimal(TextBoxRMDTax.Text.Trim) > 100.0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "RMD Tax Withholding(%) should be between 0 to 100.", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If

                'End:  Dinesh k           2014.05.15      BT-2537 : YRS 5.0-2370 - Change to validation on RMD tax rate 
                'End-SR:2013.11.19 : YRS 5.0-2165 : RMD Tax Rate should be greater then 10%

                l_DateInPast = Date.Now.AddYears(-1)
                l_DateInFuture = Date.Now.AddYears(1)

                If Not Session("Dr_PrimaryAddress") Is Nothing Then

                    l_dr_PrimaryAddress = DirectCast(Session("Dr_PrimaryAddress"), DataRow())
                    'Primary Address section starts
                    If l_dr_PrimaryAddress.Length > 0 Then
                        If (l_dr_PrimaryAddress(0).Item("effectiveDate").ToString() <> "System.DBNull" And l_dr_PrimaryAddress(0).Item("effectiveDate").ToString() <> "") Then
                            'l_PrimaryDate = DirectCast(l_ds_PrimaryAddress.Tables("AddressInfo").Rows(0).Item("EffDate"), Date)
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



                'AA:06.02.2014:BT:2316:YRS 5.0 2262 : Added below code for 
                'validating spousal wavier date and cannot locate spouse for a married person 
                'whose primary spouse beneficiary percentage is less than 100%
                If Not ValidateSpousalWavierAndCannotLocateSpouse() Then
                    ButtonSaveParticipant.Enabled = True
                    ButtonCancel.Enabled = True
                    Exit Sub
                End If


                'BS:2012.03.09:-restructure Address Control
                'If Me.TextBoxEffDate.Text <> "" AndAlso Me.TextBoxEffDate.Text <> l_PrimaryDate Then
                '	l_PrimaryDate = CType(Me.TextBoxEffDate.Text, Date)

                If m_str_Pri_EffDate <> "" AndAlso Me.m_str_Pri_EffDate <> l_PrimaryDate Then
                    l_PrimaryDate = CType(m_str_Pri_EffDate, Date)

                    If Date.Compare(l_DateInPast, l_PrimaryDate) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
                        Exit Sub
                    End If

                    If Date.Compare(l_PrimaryDate, l_DateInFuture) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
                        Exit Sub
                    End If

                End If



                If Me.m_str_Sec_EffDate <> "" AndAlso Me.m_str_Sec_EffDate <> l_SecondaryDate Then
                    l_SecondaryDate = CType(Me.m_str_Sec_EffDate, Date)

                    If Date.Compare(l_DateInPast, l_SecondaryDate) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
                        Exit Sub
                    End If

                    If Date.Compare(l_SecondaryDate, l_DateInFuture) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
                        Exit Sub
                    End If

                End If


                'Start -- Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Commented existing code and added code to validate address and display message
                'If Me.ButtonEditAddress.Enabled = False Then
                '    'Primary Address
                '    Call SetPrimaryAddressDetails()
                '    'Secondary Address
                '    Call SetSecondaryAddressDetails()
                '    l_string_Message = AddressWebUserControl1.ValidateAddress()
                '    If l_string_Message <> "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                '    l_string_Message = AddressWebUserControl2.ValidateAddress()
                '    If l_string_Message <> "" Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'End If              
                l_string_Message = AddressWebUserControl1.ValidateAddress()
                If l_string_Message <> "" Then
                    ButtonSaveParticipant.Enabled = True
                    ButtonCancel.Enabled = True
                    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                l_string_Message = AddressWebUserControl2.ValidateAddress()
                If l_string_Message <> "" Then
                    ButtonSaveParticipant.Enabled = True
                    ButtonCancel.Enabled = True
                    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'End -- Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Commented existing code and added code to validate address and display message
                If (AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA") Then
                    'START: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    If TextBoxTelephone.Visible Then
                        stTelephoneErrors = ValidateTelephoneNumbers(TextBoxTelephone.Text.Trim(), TextBoxHome.Text.Trim(), TextBoxMobile.Text.Trim(), TextBoxFax.Text.Trim())
                        If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                            ButtonSaveParticipants.Enabled = True
                            ButtonCancel.Enabled = True
                            MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                            Exit Sub
                        End If

                        'If TextBoxTelephone.Text.Trim() <> "" Then
                        '    If TextBoxTelephone.Text.Length <> 10 Then
                        '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                        '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                        '        ButtonSaveParticipant.Enabled = True
                        '        ButtonCancel.Enabled = True
                        '        MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
                        '        Exit Sub
                        '    End If
                        'End If

                        'If TextBoxHome.Text.Trim() <> "" Then
                        '    If TextBoxHome.Text.Length <> 10 Then
                        '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                        '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                        '        ButtonSaveParticipant.Enabled = True
                        '        ButtonCancel.Enabled = True
                        '        MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
                        '        Exit Sub
                        '    End If
                        'End If

                        'If TextBoxMobile.Text.Trim() <> "" Then
                        '    If TextBoxMobile.Text.Length <> 10 Then
                        '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                        '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                        '        ButtonSaveParticipant.Enabled = True
                        '        ButtonCancel.Enabled = True
                        '        MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
                        '        Exit Sub
                        '    End If
                        'End If

                        'If TextBoxFax.Text.Trim() <> "" Then
                        '    If TextBoxFax.Text.Length <> 10 Then
                        '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                        '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                        '        ButtonSaveParticipant.Enabled = True
                        '        ButtonCancel.Enabled = True
                        '        MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
                        '        Exit Sub
                        '    End If
                        'End If
                        'END: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    ElseIf HelperFunctions.isNonEmpty(Session("dt_PrimaryContactInfo")) And (AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA") Then
                        dtTelephone = Session("dt_PrimaryContactInfo")
                        'START: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                        stTelephoneErrors = ValidateTelephoneNumbers(dtTelephone)
                        If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                            ButtonSaveParticipants.Enabled = True
                            ButtonCancel.Enabled = True
                            MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                            Exit Sub
                        End If

                        'For Each drTelephone In dtTelephone.Rows
                        '    'Anudeep:27.08.2013 -BT-1555:YRS 5.0-1769:To check trimmed value for length
                        '    If drTelephone("PhoneNumber").ToString().Trim().Length > 0 And drTelephone("PhoneNumber").ToString().Trim().Length <> 10 Then
                        '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                        '        ButtonSaveParticipant.Enabled = True
                        '        ButtonCancel.Enabled = True
                        '        MessageBox.Show(PlaceHolder1, "YMCA", drTelephone("PhoneType").ToString() + " number must be 10 digits.", MessageBoxButtons.Stop)
                        '        Exit Sub
                        '    End If
                        'Next
                        'END: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    End If

                End If

                If TextBoxSecTelephone.Visible And (AddressWebUserControl2.DropDownListCountryValue = "US" Or AddressWebUserControl2.DropDownListCountryValue = "CA") Then
                    'START: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(TextBoxSecTelephone.Text.Trim(), TextBoxSecHome.Text.Trim(), TextBoxSecMobile.Text.Trim(), TextBoxSecFax.Text.Trim())
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        ButtonSaveParticipants.Enabled = True
                        ButtonCancel.Enabled = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'If TextBoxSecTelephone.Text.Trim() <> "" Then
                    '    If TextBoxSecTelephone.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipant.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxSecHome.Text.Trim() <> "" Then
                    '    If TextBoxSecHome.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipant.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxSecMobile.Text.Trim() <> "" Then
                    '    If TextBoxSecMobile.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipant.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxSecFax.Text.Trim() <> "" Then
                    '    If TextBoxSecFax.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipant.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If
                    'END: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                ElseIf HelperFunctions.isNonEmpty(Session("dt_SecondaryContactInfo")) And (AddressWebUserControl2.DropDownListCountryValue = "US" Or AddressWebUserControl2.DropDownListCountryValue = "CA") Then
                    dtTelephone = Session("dt_SecondaryContactInfo")
                    'START: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(dtTelephone)
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        ButtonSaveParticipants.Enabled = True
                        ButtonCancel.Enabled = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'For Each drTelephone In dtTelephone.Rows
                    '    'Anudeep:27.08.2013 -BT-1555:YRS 5.0-1769:To check trimmed value for length
                    '    If drTelephone("PhoneNumber").ToString().Trim().Length > 0 And drTelephone("PhoneNumber").ToString().Trim().Length <> 10 Then
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipant.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", drTelephone("PhoneType").ToString() + " number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'Next
                    'END: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                End If
                'Added By Ashutosh Patil as on 06-Jun-2007
                'Validations for Phony SSNo
                'YREN-3490  

                'If Me.TextBoxSSNo.Enabled = True Then 'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -Commented
                If IsSSNEdited() Then 'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -added
                    l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.TextBoxSSNo.Text.Trim())

                    If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
                        Session("PhonySSNo") = "Not_Phony_SSNo"
                        Call SetControlFocus(Me.TextBoxSSNo)
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
                'Added by Swopna-21May08-YRPS-4704 - Start
                Dim dsActiveBeneficiaries As New DataSet
                If DropDownListMaritalStatus.SelectedValue <> "M" Then
                    Dim dRows As DataRow()
                    dsActiveBeneficiaries = Session("BeneficiariesActive")
                    If Not dsActiveBeneficiaries Is Nothing Then
                        If dsActiveBeneficiaries.Tables.Count > 0 Then
                            If dsActiveBeneficiaries.Tables(0).Rows.Count > 0 Then
                                dRows = dsActiveBeneficiaries.Tables(0).Select("Rel='SP'")
                                If dRows.Length > 0 Then
                                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Cannot select Marital Status other than Married as one of the beneficiary is defined as Spouse. ", MessageBoxButtons.Stop)
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                End If
                'Added by Swopna-21May08-YRPS-4704 - end
                'by aparna 14/12/2007
                'BS:2012.03.13:BT:993:-After "Active primary beneficiary not defined" prompt TextBoxDateDeceased should be  disable 
                'BS:2012.01.20:YRS 5.0-1497

                TextBoxDeceasedDate.Enabled = False
                TextBoxDeceasedDate.ReadOnly = True
                If Me.TextBoxPrimaryA.Text = "" And Session("WithDrawn_member") = False Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Active primary beneficiary not defined.Do you want to continue?", MessageBoxButtons.YesNo)
                    Exit Sub
                End If
                ButtonCancel.Enabled = False
                ButtonOK.Enabled = True
                Me.MakeLinkVisible()



                SaveInfo()
            End If
            If Not Session("ValidationMessage") Is Nothing Then
                'Anudeep:06.11.2013:BT-1455:YRS 5.0-1733:Enabling the save button after displaying validation message
                Me.ButtonSaveParticipants.Enabled = False
                Me.ButtonSaveParticipants.Visible = False
                Me.ButtonSaveParticipant.Visible = True
                Me.ButtonSaveParticipant.Enabled = True
                Me.ButtonCancel.Enabled = True
            Else
                Me.ButtonSaveParticipants.Visible = False
                Me.ButtonSaveParticipants.Enabled = False
                Me.ButtonSaveParticipant.Visible = True
            End If
            Me.IsExhaustedDBRMDSettleOptionSelected = False 'MMR | 2016.10.14 | YRS-AT-3063 | Resetting property value
        Catch ex As SqlException
            If ex.Number = 60006 And ex.Procedure.ToString = "yrs_usp_AMCM_SearchConfigurationMaintenance" Then
                'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                'g_String_Exception_Message = "No Key defined for Phony SSNo in AtsMetaConfiguration."
                g_String_Exception_Message = "No Key defined for Placeholder SSNo in AtsMetaConfiguration."
                'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
            Else
                g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            End If
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)

        End Try

    End Sub

    Public Function UpadateGeneralInfo() As String
        Try
            'If TextBoxSSNo.Text.Length <> 9 Then
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "SSNo must be nine digits long.", MessageBoxButtons.OK)
            'Else
            Dim l_string_Output As String
            Dim l_string_MaritalStatus As String
            Dim l_string_Gender As String

            'Start: Bala: 01/19/2016: YRS-AT-AT-2398: Variable name change.
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
            'End: Bala: 01/19/2016: YRS-AT-AT-2398: Variable name change.
            'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
            'Priya : Added on 19/2/2010 :YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
            'Dim bool_AllowShareInfo As Boolean
            'bool_AllowShareInfo = chkShareInfoAllowed.Checked
            'End 19/2/2010
            'End  12-May-2010 YRS 5.0-1073

            '5-April-2010:Priya:YRS 5.0-1042 : fetch value of newly added column bitYmcaMailOptOut
            ' Dim bool_YmcaMailOptOut As Boolean
            ' bool_YmcaMailOptOut = chkYmcaMailOptOut.Checked
            'End YRS 5.0-988

            '11-july-2011:bhavna:YRS 5.0-1354 : change CheckBox caption bitPersonalInfoSharingOptOut
            Dim bool_PersonalInfoSharingOptOut As Boolean
            bool_PersonalInfoSharingOptOut = chkPersonalInfoSharingOptOut.Checked
            'End YRS 5.0-1354


            '11-july-2011:bhavna:YRS 5.0-1354 : Add new bit field  bitGoPaperless into atsperss
            Dim bool_GoPaperless As Boolean
            bool_GoPaperless = chkGoPaperless.Checked
            'End YRS 5.0-1354



            'Start : Commented & added by Dilip yadav on 2009.09.09 for YRS 5.0-852
            'If Me.DropDownListGender.SelectedValue = "Female" Then
            '    l_string_Gender = "F"
            'ElseIf Me.DropDownListGender.SelectedValue = "Male" Then
            '    l_string_Gender = "M"
            'ElseIf Me.DropDownListGender.SelectedValue = "Unknown" Then
            '    l_string_Gender = "U"
            'Else
            '    l_string_Gender = ""
            'End If
            l_string_Gender = Me.DropDownListGender.SelectedValue
            'End : Commented & added by Dilip yadav on 2009.09.09 for YRS 5.0-852

            'commented by hafiz on 2-May-2007 for YREN-3112
            'If Me.DropDownListMaritalStatus.SelectedValue = "Married" Then
            '    l_string_MaritalStatus = "M"
            'ElseIf Me.DropDownListMaritalStatus.SelectedValue = "Single" Then
            '    l_string_MaritalStatus = "S"
            'ElseIf Me.DropDownListMaritalStatus.SelectedValue = "Unknown" Then
            '    l_string_MaritalStatus = "U"
            'Else
            '    l_string_MaritalStatus = ""
            'End If
            'commented by hafiz on 2-May-2007 for YREN-3112

            'added by hafiz on 2-May-2007 for YREN-3112
            l_string_MaritalStatus = Me.DropDownListMaritalStatus.SelectedValue
            'added by hafiz on 2-May-2007 for YREN-3112

            'Commented and added by dilip yadav : 29-Oct-09 : YRS 5.0.921
            'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
            'l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateGeneralInfo(Session("PersId"), TextBoxSSNo.Text, TextBoxLast.Text, TextBoxFirst.Text, TextBoxMiddle.Text, DropDownListSal.SelectedValue, TextBoxSuffix.Text, l_string_Gender, l_string_MaritalStatus, TextBoxDOB.Text, TextBoxSpousealWiver.Text)
            'l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateGeneralInfo(Session("PersId"), TextBoxSSNo.Text, TextBoxLast.Text, TextBoxFirst.Text, TextBoxMiddle.Text, DropDownListSal.SelectedValue, TextBoxSuffix.Text, l_string_Gender, l_string_MaritalStatus, TextBoxDOB.Text, TextBoxSpousealWiver.Text, l_bool_Priority, False, bool_YmcaMailOptOut)
            'End 12-May-2010 YRS 5.0-1073

            'bhavnaS 11.july.2011 YRS 5.0-1354  : replace one bit caption bool_YmcaMailOptOut and new bit field bool_GoPaperless
            'BS:2012.01.18:YRS 5.0-1497 -here we pass parameter TextBoxDeceasedDate.Text for updated death date
            'prasad:2012.01.19:For BT-925,YRS 5.0-1400 : Passed additional parameter 'chkMRD.Checked'            
            'AA:2014.02.06:BT:2316 :YRS 5.0-2262 - Added for passing cannotlocatespouse text value to pass as parameter to sp
            'AA:2014.02.16:BT:2291 :YRS 5.0-2247 - Added for passing WebLockOut text value to pass as parameter to sp
            'Start: Bala: 19/01/2016: YRS-AT-2398: Varialble name change.
            'l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateGeneralInfo(Session("PersId"), TextBoxSSNo.Text, TextBoxLast.Text, TextBoxFirst.Text, TextBoxMiddle.Text, DropDownListSal.SelectedValue, TextBoxSuffix.Text, l_string_Gender, l_string_MaritalStatus, TextBoxDOB.Text, TextBoxSpousealWiver.Text, l_bool_Priority, False, bool_PersonalInfoSharingOptOut, bool_GoPaperless, TextBoxDeceasedDate.Text, chkMRD.Checked, lblCannotLocateSpouse.Text.Replace("(", "").Replace(")", ""), chkNoWebAcctCreate.Checked.ToString().Trim(), "ParticipantInfo")
            l_string_Output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateGeneralInfo(Session("PersId"), TextBoxSSNo.Text, TextBoxLast.Text, TextBoxFirst.Text, TextBoxMiddle.Text, DropDownListSal.SelectedValue, TextBoxSuffix.Text, l_string_Gender, l_string_MaritalStatus, TextBoxDOB.Text, TextBoxSpousealWiver.Text, l_bool_ExhaustedDBSettle, False, bool_PersonalInfoSharingOptOut, bool_GoPaperless, TextBoxDeceasedDate.Text, chkMRD.Checked, lblCannotLocateSpouse.Text.Replace("(", "").Replace(")", ""), chkNoWebAcctCreate.Checked.ToString().Trim(), "ParticipantInfo")
            'End: Bala: 19/01/2016: YRS-AT-2398: Varialble name change.
            'End 11.july.2011 YRS 5.0-1354

            If (l_string_Output = "0") Then
                TextBoxSSNo.Enabled = False
                TextBoxLast.Enabled = False
                TextBoxFirst.Enabled = False
                TextBoxMiddle.Enabled = False
                DropDownListSal.Enabled = False
                TextBoxSuffix.Enabled = False
                'DropDownListGender.Enabled = False
                'DropDownListMaritalStatus.Enabled = False
                TextBoxDOB.Enabled = False
                ''PopcalendarDate.Enabled = False
                Session("General") = True
                'TextBoxSpousealWiver.Enabled = False
                'Start Ashutosh Patil as on 21-Jun-2007 YREN -3490
                UpadateGeneralInfo = String.Empty
                'End Ashutosh Patil as on 21-Jun-2007
            ElseIf (l_string_Output <> "0" And l_string_Output <> "1") Then
                'Start Ashutosh Patil as on 21-Jun-2007 YREN -3490
                Session("General") = False
                UpadateGeneralInfo = l_string_Output
                'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "SSNo. is Already in use for Participant -" + l_string_Output, MessageBoxButtons.OK)
                'LoadGeneraltab()
                'End Ashutosh Patil as on 21-Jun-2007
            End If
            'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-Start
            If (IsSSNEdited()) Then
                Dim dtTempTable As DataTable = CType(Session("SessionTempSSNTable"), DataTable)
                If HelperFunctions.isNonEmpty(dtTempTable) Then
                    Dim strOldSSN, strReason As String
                    strReason = (dtTempTable.Rows(0)("Reason")).ToString()
                    strOldSSN = (dtTempTable.Rows(0)("OldSSN")).ToString()
                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertAuditLog("Person Maintenance", Session("PersId").ToString(), "PERSON", "chrSSNo", strOldSSN, TextBoxSSNo.Text.Trim(), strReason)
                End If
            End If

            'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-End
            'End If
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
    End Function



    Public Sub LoadGeneraltab()
        'Shashi Shekhar: 09- feb 2011: For YRS 5.0-1236 : Need ability to freeze/lock account
        'Make dynamic changes of control and label text according to saved value of Account Locking.
        GetLockUnlockReasonDetails()

        'Start: DInesh.k 2013.09.17 BT-2139:YRS 5.0-2165:RMD enhancements.
        'Start: Bala: 01/05/2016: YRS-AT-1972: Initializing SpecialDeathProcess check box value.
        ViewState("InitialSpecialDeathProcessingRequiredBitFlag") = False
        'End: Bala: 01/05/2016: YRS-AT-1972: Initializing SpecialDeathProcess check box value.
        Dim dsPersonConfiguration As DataSet
        dsPersonConfiguration = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetPersonMetaConfigurationDetails(Session("PersId").ToString(), "RMD")
        If dsPersonConfiguration.Tables.Count > 0 Then
            If dsPersonConfiguration.Tables(0).Rows.Count > 0 Then
                'Dinesh Kanojia     2014.02.18      BT-2139: YRS 5.0-2165:RMD enhancements - To get updated mrdtaxrate for an participant.
                'START : SB | 06/21/2018 | YRS-AT-3858 | If the particpant is Eligible for RMD then only display Requested Annual MRD checkbox and RMD Tax Withholding (%) textbox to the participant
                'If Not String.IsNullOrEmpty(dsPersonConfiguration.Tables(0).Rows(0)("RMDTAXRATE").ToString().ToUpper) Then
                If (Convert.ToBoolean(dsPersonConfiguration.Tables(0).Rows(0)("IsRMDCheckboxVisible"))) Then
                    'END : SB | 06/21/2018 | YRS-AT-3858 | If the particpant is Eligible for RMD then only display Requested Annual MRD checkbox and RMD Tax Withholding (%) textbox to the participant
                    lblRMDTax.Visible = True
                    tblRMDTAX.Visible = True
                    TextBoxRMDTax.Text = dsPersonConfiguration.Tables(0).Rows(0)("RMDTAXRATE")
                    chkMRD.Visible = True
                    RMDTaxRate = TextBoxRMDTax.Text.Trim() ' SP 2014.04.29 BT-2525
                End If

                IsRMDEligible = Convert.ToBoolean(dsPersonConfiguration.Tables(0).Rows(0)("IsRMDEligible")) 'SR | 07/12/2018 | YRS-AT-3858 | Get paticipant RMD eligibility flag to decide whether allow participant to enroll in annual RMD program or not

                'Start: Bala: 01/05/2016: YRS-AT-1972: Initializing SpecialDeathProcess check box value.
                If Not String.IsNullOrEmpty(dsPersonConfiguration.Tables(0).Rows(0)("SpecialDeathProcess").ToString()) Then
                    If (Convert.ToBoolean(dsPersonConfiguration.Tables(0).Rows(0)("SpecialDeathProcess").ToString())) Then
                        ViewState("InitialSpecialDeathProcessingRequiredBitFlag") = True
                        chkSpecialDeathProcess.Checked = True
                    End If
                End If
                'End: Bala: 01/05/2016: YRS-AT-1972: Initializing SpecialDeathProcess check box value.
            End If
        End If

        'End: DInesh.k 2013.09.17 BT-2139:YRS 5.0-2165:RMD enhancements. 
        'NP:2012.04.11 - This method was not doing anything useful - LoadDropdowns()
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3028,YREN-3029
        'Me.TextBoxAddress1.Text = ""
        'Me.TextBoxAddress2.Text = ""
        'Me.TextBoxAddress3.Text = ""
        'Me.TextBoxCity.Text = ""
        'Me.TextBoxZip.Text = ""
        'Me.DropdownlistState.SelectedValue = ""
        'Me.DropdownlistCountry.SelectedValue = ""
        'BS:2012.03.09:-restructure Address Control
        'Me.TextBoxEffDate.Text = ""
        'Commneted By Dinesh Kanojia Contacts
        'Me.TextBoxTelephone.Text = ""
        'Me.TextBoxHome.Text = ""
        'Me.TextBoxMobile.Text = ""
        'Me.TextBoxFax.Text = ""
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3028,YREN-3029
        'Me.TextBoxSecAddress1.Text = ""
        'Me.TextBoxSecAddress2.Text = ""
        'Me.TextBoxSecAddress3.Text = ""
        'Me.TextBoxSecCity.Text = ""
        'Me.TextBoxSecZip.Text = ""
        'Me.DropdownlistSecState.SelectedValue = ""
        'Me.DropdownlistSecCountry.SelectedValue = ""
        'BS:2012.03.09:-restructure Address Control
        'Me.TextBoxSecEffDate.Text = ""

        'Commneted By Dinesh Kanojia Contacts
        'Me.TextBoxSecTelephone.Text = ""
        'Me.TextBoxSecHome.Text = ""
        'Me.TextBoxSecMobile.Text = ""
        'Me.TextBoxSecFax.Text = ""
        Me.TextBoxEmailId.Text = ""
        Dim l_string_PersId As String
        Dim l_dataset_AddressInfo As DataSet
        'Added by Anudeep:12.03.2013-For Telephone Usercontrol
        Dim l_datatable_ContactsInfo As DataTable
        Dim dsAddress As DataSet
        l_string_PersId = Session("PersId")

        Dim l_dataset_Poa As DataSet

        'Loading the data in General Tab of the Form
        'Loading the first Tab

        '''' code added by vartika on 18th Nov 2005
        Dim l_dataset_SecAddressInfo As DataSet
        Dim l_datatable_SecContactInfo As DataTable
        'Dim l_dataset_priEmailProps As DataSet
        'Dim l_dataset_SecEmailProps As DataSet
        g_dataset_GeneralInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpGeneralInfo(l_string_PersId)
        'BS:2012.03.16:For BT-1015:YRS 5.0-1557:-assign data set to viewstate for  passing Death date value into Death Notification Pop up if date exist but notification not process
        ViewState("g_dataset_GeneralInfo") = g_dataset_GeneralInfo
        If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
            'BS:2012.01.16:YRS 5.0-1497 :- Here we have set visibilty mode of btnEditDeathDate,ButtonDeathNotification according to IsDeathDateUpdateBlock flag  this flag will be set by perform some validation  from database like beneficiary a/c creation,settlemet process,date is null etc. 
            Dim l_strDBFundStatusType As String
            l_strDBFundStatusType = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundStatusType").ToString().Trim
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsDeathDateUpdateBlock").ToString() = "0") Then
                'BS:2012.03.16:For BT-1015:YRS 5.0-1557:-here we are checking fundevent status for death notification only if death date exist but notification not done
                If (l_strDBFundStatusType = "DA" OrElse l_strDBFundStatusType = "DD" OrElse l_strDBFundStatusType = "DR" _
                  OrElse l_strDBFundStatusType = "DI" OrElse l_strDBFundStatusType = "DQ") Then
                    btnEditDeathDatePer.Visible = True
                    ButtonDeathNotification.Visible = False
                    'Start:AA:05/14/2015 BT:2680 YRS 5.0-2428: Added to disable cannot locate spouse and spousal waiver date
                    TextBoxSpousealWiver.Enabled = False
                    chkCannotLocateSpouse.Enabled = False
                    'End:AA:05/14/2015 BT:2680 YRS 5.0-2428: Added to disable cannot locate spouse and spousal waiver date
                    'Start: Bala: 01/05/2016: Disabling special death process check box.
                    chkSpecialDeathProcess.Enabled = False
                    'End: Bala: 01/05/2016: Disabling special death process check box.
                Else
                    btnEditDeathDatePer.Visible = False
                    ButtonDeathNotification.Visible = True
                End If
            Else
                btnEditDeathDatePer.Visible = False
                ButtonDeathNotification.Visible = True
            End If

            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation").ToString() <> "") Then
                'code by Swopna 
                'Commented by Swopna in response to bug id 340
                'Me.DropDownListSal.SelectedValue = CType(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation"), String).Trim()
                'Added by Swopna in response to bug id 340 on 27 Dec 2007(Modified on 14 Jan,2008)
                '*****************                  
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation").ToString().Trim.EndsWith(".") = True) Then
                    Me.DropDownListSal.SelectedValue = DirectCast(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation"), String).Trim()

                Else
                    Dim str As String
                    str = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Salutation") + "."
                    Me.DropDownListSal.SelectedValue = str
                    '*****************
                End If
            End If
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString() <> "") Then
                Me.TextBoxFirst.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FirstName").ToString().Trim
            End If

            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString() <> "") Then
                Me.TextBoxLast.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("LastName").ToString().Trim
            End If
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString() <> "") Then
                Me.TextBoxMiddle.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MiddleName").ToString().Trim
            End If
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Suffix").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Suffix").ToString() <> "") Then
                Me.TextBoxSuffix.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Suffix").ToString().Trim
            End If

            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("DOB").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("DOB").ToString() <> "") Then
                Me.TextBoxDOB.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("DOB")
                Session("DOB") = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("DOB")
            End If
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Deathdate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Deathdate").ToString() <> "") Then
                Me.TextBoxDeceasedDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Deathdate")
            End If
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("ParticipationDate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("ParticipationDate").ToString() <> "") Then
                Me.TextBoxParticipationDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("ParticipationDate")
            End If

            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString() <> "") Then
                Me.TextBoxSSNo.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SSNo").ToString().Trim
                'SS:25 Feb 2011: For header change through user control :YRS 5.0-450 : Replace SSN with Fund Id on all screens.
                Headercontrol.PageTitle = "Participant Information"
                Headercontrol.SSNo = Me.TextBoxSSNo.Text
            End If

            'Start: Bala: 01/19/2016: YRS-AT-2398: Column change
            ''Added by Dilip yadav : YRS 5.0.921
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Priority").ToString() = True) Then
            '    Me.checkboxPriority.Checked = True
            'Else
            '    Me.checkboxPriority.Checked = False
            'End If
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("ExhaustedDBSettle").ToString() = True) Then
                Me.checkboxExhaustedDBSettle.Checked = True
                Session("IsExhaustedDBRMDSettle") = True 'MMR | 2016.10.14 | YRS-AT-3062 | Setting property value
            Else
                Me.checkboxExhaustedDBSettle.Checked = False
            End If
            'End: Bala: 01/19/2016: YRS-AT-2398: Column change

            'Name:Preeti Date:10th Feb 06 Issue :YMCA-1858 Start
            'NP:BT-550:2008.09.10 - Adding code to set Person_Info to just the SSN
            Session("Person_Info") = Me.TextBoxSSNo.Text
            Dim strSSN As String = Me.TextBoxSSNo.Text.Insert(3, "-")
            strSSN = strSSN.Insert(6, "-")

            'Shashi Shekhar:2009.08.28 Ref:(PS:YMCA PS Data Archive.Doc) - Adding code to change general header in case if person's data has been archived
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsArchived").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsArchived").ToString() <> "") Then
                g_isArchived = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsArchived").ToString().Trim)
            End If

            'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
            '18-Feb-2010:Priya:YRS 5.0-988 : fetch value of newly aaded column bitShareInfoAllowed
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsShareInfoAllowed").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsShareInfoAllowed").ToString() <> "") Then
            '    chkShareInfoAllowed.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsShareInfoAllowed").ToString().Trim)
            'End If
            'End 18-Feb-2010
            'End  12-May-2010 YRS 5.0-1073


            '5-April-2010:Priya:YRS 5.0-1042- New "flag" value in Person/Retiree maintenance screen
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString() <> "") Then
            '    chkYmcaMailOptOut.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsYmcaMailOptOut").ToString().Trim)
            'End If
            'End YRS 5.0-1042

            '2011.07.08 :bhavnaS YRS 5.0-1354 : replace IsPersonalInfoSharingOptOut instead of IsYmcaMailOptOut
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsPersonalInfoSharingOptOut").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsPersonalInfoSharingOptOut").ToString() <> "") Then
                chkPersonalInfoSharingOptOut.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsPersonalInfoSharingOptOut").ToString().Trim)
            End If
            'End YRS 2011.07.08

            '2011.07.11 :bhavnaS YRS 5.0-1354 : Add new bit Field GoPaperless into atsperss
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsGoPaperless").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsGoPaperless").ToString() <> "") Then
                chkGoPaperless.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("IsGoPaperless").ToString().Trim)
            End If
            'End YRS 2011.07.11
            '2012.01.19	:prasad jadhav YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MRDRequested").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MRDRequested").ToString() <> "") Then
                chkMRD.Checked = Convert.ToBoolean(g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MRDRequested").ToString().Trim)
            End If

            If g_isArchived = True Then
                Session("IsDataArchived") = "Yes"
                tdRetrieveData.Visible = True
                LabelGenHdr.Text = " - Transaction & Disbursement data have been archived."
                '-----------------------------------------------------------------------------------------------------------------
                'Shashi Shekhar: 28-Oct-2010: For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                    'LabelHdr.Text = Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ",SS#: " + strSSN + " " + "(Archived)"
                    ' LabelHdr.Text = Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ", Fund No: " + g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim() + " " + "(Archived)"
                End If
                '--------------------------------------------------------------------------------------------------------

            Else
                Session("IsDataArchived") = "No"
                tdRetrieveData.Visible = False
                LabelGenHdr.Text = ""
                '-----------------------------------------------------------------------------------------------------------------
                'Shashi Shekhar: 28-Oct-2010: For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                    '    LabelHdr.Text = Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ",SS#: " + strSSN
                    '  LabelHdr.Text = Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ", Fund No: " + g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim()
                End If
                '--------------------------------------------------------------------------
            End If

            'commented by shashi
            'LabelHdr.Text = Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ",SS#: " + strSSN

            '-----------------------------------------------------------------------------------------------

            'Start: Bala: 010/19/2016: YRS-AT-2398: Check box name change.
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
            'End: Bala: 010/19/2016: YRS-AT-2398: Check box name change.

            'Name:Preeti Date:10th Feb 06 Issue :YMCA-1858  End
            'NP:BT-550:2008.09.10 - Commenting this since the value going into Person_Info is the full header instead of just the SSN
            'Session("Person_Info") = LabelHdr.Text.Trim

            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                Me.TextBoxFundNo.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim

            End If
            '2009.09.09 : Modified by Dilip yadav : YRS 5.0.852
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "") Then
            '    Try
            '        Dim strGendertype As String
            '        strGendertype = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString().Trim()
            '        If (Me.DropDownListGender.Items.FindByValue(strGendertype) Is Nothing) Then
            '            Me.DropDownListGender.Items.Insert(0, strGendertype)
            '        Else
            '            Me.DropDownListGender.SelectedValue = strGendertype
            '        End If
            '        Me.DropDownListGender.SelectedValue = strGendertype
            '    Catch ex As Exception
            '        Throw
            '    End Try
            'Else 'Sanjay R.  20-April-2010  YRS 5.0-1055 - for Null value assigning Gendertype as UNKNOWN
            '    Dim strGendertype As String
            '    Try
            '        strGendertype = "U"
            '        If (Me.DropDownListGender.Items.FindByValue(strGendertype) Is Nothing) Then
            '            Me.DropDownListGender.Items.Insert(0, strGendertype)
            '        End If
            '        Me.DropDownListGender.SelectedValue = strGendertype
            '    Catch ex As Exception
            '        Me.DropDownListGender.SelectedValue = "U"
            '    End Try
            'End If
            'BS:2011.07.19:BT-852:YRS(5.0 - 1339)- assign X value when Gender is non human/non actuary
            If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
                Dim strGendertype As String = "U"
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).IsNull("Gender") = False And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> String.Empty) Then
                    strGendertype = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString().Trim()
                End If
                If (DropDownListGender.Items.FindByValue(strGendertype) Is Nothing) Then
                    DropDownListGender.Items.Insert(0, strGendertype)
                End If
                DropDownListGender.SelectedValue = strGendertype
            Else
                HelperFunctions.LogMessage("Error while initializing Gender code on General Information tab. Datatable GeneralInfo was found to be empty or not initialized.")
            End If

            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString() <> "") Then
            '    Try
            '        Dim strGendertype As String
            '        strGendertype = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("Gender").ToString().Trim()
            '        If (Me.DropDownListGender.Items.FindByValue(strGendertype) Is Nothing) Then
            '            Me.DropDownListGender.Items.Insert(0, strGendertype)
            '        End If
            '        Me.DropDownListGender.SelectedValue = strGendertype
            '    Catch ex As Exception
            '        Throw
            '    End Try
            'Else
            '    Dim strGendertype As String
            '    Try
            '        strGendertype = "U"
            '        Me.DropDownListGender.SelectedValue = strGendertype
            '    Catch ex As Exception
            '        Throw 'Me.DropDownListGender.SelectedValue = "U"
            '    End Try
            'End If
            'End YRS 5.0-1339 

            '2009.09.09 : Modified by Dilip yadav : YRS 5.0.852

            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString() <> "") Then
            '    Try
            '        Me.DropDownListMaritalStatus.SelectedValue = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString().Trim()
            '    Catch ex As Exception
            '        Me.DropDownListMaritalStatus.SelectedValue = "U"
            '    End Try
            'Else  'Sanjay R. 20-April-2010    YRS 5.0-1055 - for Null value assigning Gendertype as UNKNOWN
            '    Try
            '        Me.DropDownListMaritalStatus.SelectedValue = "U"
            '    Catch ex As Exception
            '        Me.DropDownListMaritalStatus.SelectedValue = "U"
            '    End Try
            'End If

            'BS:2011.07.19:BT-852:YRS(5.0 - 1339) - assign X value when MaritalStatus is non human/non actuary
            If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo) Then
                Dim strMaritalStatus As String = "U"
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).IsNull("MaritalStatus") = False And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString() <> String.Empty) Then
                    strMaritalStatus = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString().Trim()
                End If
                If (DropDownListMaritalStatus.Items.FindByValue(strMaritalStatus) Is Nothing) Then
                    DropDownListMaritalStatus.Items.Insert(0, strMaritalStatus)
                End If
                DropDownListMaritalStatus.SelectedValue = strMaritalStatus
            Else
                HelperFunctions.LogMessage("Error while initializing marital status on General Information tab. Datatable GeneralInfo was found to be empty or not initialized.")
            End If
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString() <> "") Then
            '    Try
            '        Dim strMaritalStatus As String
            '        strMaritalStatus = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("MaritalStatus").ToString().Trim()
            '        If (Me.DropDownListMaritalStatus.Items.FindByValue(strMaritalStatus) Is Nothing) Then
            '            Me.DropDownListMaritalStatus.Items.Insert(0, strMaritalStatus)
            '        End If
            '        Me.DropDownListMaritalStatus.SelectedValue = strMaritalStatus
            '    Catch ex As Exception
            '        Throw
            '    End Try
            'Else
            '    Dim strMaritalStatus As String
            '    Try
            '        strMaritalStatus = "U"
            '        If (Me.DropDownListMaritalStatus.Items.FindByValue(strMaritalStatus) Is Nothing) Then
            '            Me.DropDownListMaritalStatus.Items.Insert(0, strMaritalStatus)
            '        End If
            '        Me.DropDownListMaritalStatus.SelectedValue = strMaritalStatus
            '    Catch ex As Exception
            '        Throw 'Me.DropDownListMaritalStatus.SelectedValue = "U"
            '    End Try
            'End If
            'End YRS 5.0-1339 
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SpousealWaiver").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SpousealWaiver").ToString() <> "") Then
                If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SpousealWaiver").ToString() = "01/01/1900" Then
                    Me.TextBoxSpousealWiver.Text = ""
                Else
                    Me.TextBoxSpousealWiver.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SpousealWaiver").ToString()
                End If

            End If

            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SpousealWaiver").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SpousealWaiver").ToString() <> "") Then
                If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SpousealWaiver").ToString() = "01/01/1900" Then
                    Me.TextBoxSpousealWiver.Text = ""
                Else
                    Me.TextBoxSpousealWiver.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("SpousealWaiver").ToString()
                End If
            End If

            If TextBoxSpousealWiver.Text <> "" Then
                'TextBoxSpousealWiver.Enabled = False
            End If

            'Commented by Paramesh K. on Sept 25th 2008
            'As we are populating these information to the controls in the RefreshQdroInfo() method
            '******************************************
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType").ToString() <> "") Then
            '    Me.TextBoxQDROType.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType").ToString().Trim
            'End If
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "") Then
            '    If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString.ToUpper() = "PENDING" Then
            '        Me.CheckBoxQDROPending.Checked = True
            '    Else
            '        Me.CheckBoxQDROPending.Checked = False
            '    End If
            '    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "") Then
            '        Me.TextBoxQDROStatusDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString().Trim
            '    End If
            '    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate").ToString() <> "") Then
            '        Me.TextBoxQDRODraftDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate").ToString().Trim
            '    End If
            'End If
            '******************************************

        End If
        'Fetching The Primary address Information

        l_dataset_AddressInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAddressInfo(l_string_PersId)
        ''Added by Anudeep:12.03.2013-For Telephone Usercontrol
        'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
        l_datatable_ContactsInfo = l_dataset_AddressInfo.Tables("TelephoneInfo")
        'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        Me.AddressWebUserControl1.guiEntityId = l_string_PersId
        Me.AddressWebUserControl1.IsPrimary = 1
        Me.AddressWebUserControl2.guiEntityId = l_string_PersId
        Me.AddressWebUserControl2.IsPrimary = 0
        'Ashutosh Patil as on 25-Apr-2007
        'YREN-3298
        'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        Me.AddressWebUserControl1.IsMaintenanceScreen = True
        dsAddress = Address.GetAddressByEntity(l_string_PersId, EnumEntityCode.PERSON)
        'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        Session("Dr_PrimaryAddress") = dsAddress.Tables("Address").Select("isPrimary = True")
        'Session("Dt_EmailAddress") = dsAddress.Tables("EmailInfo")
        Session("Dt_EmailAddress") = l_dataset_AddressInfo.Tables("EmailInfo")
        Session("Dr_SecondaryAddress") = dsAddress.Tables("Address").Select("isPrimary = False")
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3028,YREN-3029    
        'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
        AddressWebUserControl1.LoadAddressDetail(dsAddress.Tables("Address").Select("isPrimary = True"))
        AddressWebUserControl2.LoadAddressDetail(dsAddress.Tables("Address").Select("isPrimary = False"))
        AddressWebUserControl1.Rights = "ButtonEditAddress"
        'Added by Anudeep:12.03.2013-For Telephone Usercontrol
        Session("dt_PrimaryContactInfo") = l_datatable_ContactsInfo
        TelephoneWebUserControl1.RefreshTelephoneDetail(l_datatable_ContactsInfo)
        'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
        'If HelperFunctions.isEmpty(l_datatable_ContactsInfo) Then
        '    btnEditPrimaryContact.Text = "Add Contacts"
        'Else
        '    btnEditSecondaryContact.Text = "Edit Contacts"
        'End If
        ' Code Added By Dinesh Kanojia ontacts.

        'gvContacts.DataSource = l_datatable_ContactsInfo
        'gvContacts.DataBind()

        RefreshEmailDetail(l_dataset_AddressInfo)

        '''' code added by vartika on 18th Nov 2005
        '' Fetching the secondary address information
        l_dataset_SecAddressInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpSecondaryAddress(l_string_PersId)

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
        'AddressWebUserControl2.RefreshAddressDetail(l_dataset_SecAddressInfo)
        'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
        AddressWebUserControl2.Rights = "ButtonEditAddress"
        'Start:Added by Anudeep:12.03.2013-For Telephone Usercontrol
        l_datatable_SecContactInfo = l_dataset_SecAddressInfo.Tables("TelephoneInfo")
        Session("dt_SecondaryContactInfo") = l_datatable_SecContactInfo
        TelephoneWebUserControl2.RefreshTelephoneDetail(l_datatable_SecContactInfo)
        btnEditPrimaryContact.Enabled = True
        btnEditSecondaryContact.Enabled = True
        'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
        'If HelperFunctions.isEmpty(l_datatable_SecContactInfo) Then
        '    btnEditSecondaryContact.Text = "Add Contacts"
        'Else
        '    btnEditSecondaryContact.Text = "Edit Contacts"
        'End If
        'RefreshSecTelephoneDetail(l_dataset_SecAddressInfo)
        'End:Added by Anudeep:12.03.2013-For Telephone Usercontrol

        LoadPoADetails()
        '2012-10-18 Priya Patil Make termiantion watcher button visible false commited code to not release termination watcher in 12.7.0
        'SR:2012.08.17 - BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program
        'ANudeep:23.10.2012 Reverted changes 
        ShowHideTerminationWatcherButton()
        BindPersonalTerminationWatcherDetails()
        'Anudeep:04.12.2012 Commented Code to show the termination watcher button who have plan balances
        'BindPlanType()
        'Ends, SR:2012.08.17 - BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program

        'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-Start
        CreateTempSSNDatatableForUpdate(TextBoxSSNo.Text.Trim())
        'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-End

        'AA:06.02.2014:BT:2316:YRS 5.0 2262 : Added below code for 
        'Populating the lblCannotLocateSpouse value if value is null or empty chkCannotLocateSpouse.Checked IS False else chkCannotLocateSpouse.Checked is True
        If Not String.Empty.IsNullOrEmpty(g_dataset_GeneralInfo.Tables(0).Rows(0)("CannotLocateSpouse").ToString()) Then
            chkCannotLocateSpouse.Checked = True
            lblCannotLocateSpouse.Text = "(" + g_dataset_GeneralInfo.Tables(0).Rows(0)("CannotLocateSpouse").ToString() + ")"
        Else
            chkCannotLocateSpouse.Checked = False
            lblCannotLocateSpouse.Text = String.Empty
        End If

        If Not String.Empty.IsNullOrEmpty(g_dataset_GeneralInfo.Tables(0).Rows(0)("WebLockOut").ToString().Trim) Then
            chkNoWebAcctCreate.Checked = g_dataset_GeneralInfo.Tables(0).Rows(0)("WebLockOut").ToString().Trim()
        Else
            chkNoWebAcctCreate.Checked = False
        End If

        'Start: Bala: 01/19/2019: YRS-AT-2398: Alert message.
        If HelperFunctions.isNonEmpty(g_dataset_GeneralInfo.Tables("SpecialHandlingDetails")) Then 'Bala: 03.16.2016: Check if the table has no records.
            If (g_dataset_GeneralInfo.Tables("SpecialHandlingDetails").Rows(0).Item("RequireSpecialHandling").ToString() = True) Then
                Me.LabelSpecialHandling.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PART_MAINT_SPECIAL_HANDLING).DisplayText
                Me.LabelSpecialHandling.Visible = True
                Me.HiddenFieldOfficerDetails.Value = g_dataset_GeneralInfo.Tables("SpecialHandlingDetails").Rows(0).Item("EmploymentDetail").ToString()
            End If
        End If
        'End: Bala: 01/19/2019: YRS-AT-2398: Alert message.
    End Sub

    'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-Start
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
        Dim bSSNEdited As Boolean
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

        End Try
    End Function

    Private Sub SetUpdatedSSN()
        Dim dtSSNUpdate As DataTable
        Try
            If (Session("SessionTempSSNTable") IsNot Nothing) Then
                dtSSNUpdate = DirectCast(Session("SessionTempSSNTable"), DataTable)
                If (HelperFunctions.isNonEmpty(dtSSNUpdate) And Not String.IsNullOrEmpty(Convert.ToString(dtSSNUpdate.Rows(0)("NewSSN")))) Then
                    TextBoxSSNo.Text = Convert.ToString(dtSSNUpdate.Rows(0)("NewSSN"))
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN-End

    'Bhavna:Session comment of deathnotify function YRS 5.0-1432
    'Public Function DeathNotify() As Boolean
    '	'BS:2011.12.20:-not suppose to catch in sub method
    '	' Try
    '	'paramPersonOrFundEvent as String, param_PersonOrFundEventID as string , paramDeathDate  as  DateTime
    '	'Dim l_string_returnStatus As String
    '	Dim l_int_returnStatus As Int16
    '	Dim l_string_Message As String
    '	Dim l_date_DeathDate As DateTime
    '	''SR:2011.04.28 - YRS 5.0-1292: two variables are defined to store value of output parameters for retires and non retired participants
    '	Dim l_string_returnStatusDR As String = ""
    '	Dim l_string_returnStatusDA As String = ""
    '	'l_string_returnStatus = ""
    '	l_string_Message = ""
    '	l_date_DeathDate = Session("DeathDate")
    '	DeathNotify = True

    '	''Call the Validation for DeathNotifyPrerequisites
    '	l_int_returnStatus = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DeathNotifyPrerequisites("PERSON", Session("PersId"), l_date_DeathDate, l_string_returnStatusDR, l_string_returnStatusDA)
    '	If l_int_returnStatus < 0 Then
    '		MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification failed due to error Network Error:" + Chr(13) + "Cannot Proceed", MessageBoxButtons.OK)
    '		DeathNotify = False
    '		Exit Function
    '	End If

    '	''SR:2011.04.28 - YRS 5.0-1292 
    '	Session("ParticipantDNMsgforDR") = l_string_returnStatusDR

    '	If (l_string_returnStatusDA <> "") Then
    '		MessageBox.Show(PlaceHolder1, "Note before you proceed.", l_string_returnStatusDA, MessageBoxButtons.OK) ''SR:2011.04.27 - YRS 5.0 1292 : OK button For Non retired participants validation message
    '		Session("Call_FinalDeathNotificationUpdation") = "Yes"
    '		DeathNotify = False
    '		Exit Function
    '	End If

    '	'If (l_string_returnStatusDR <> "") Then
    '	'	MessageBox.Show(PlaceHolder1, "Note before you proceed.", l_string_returnStatusDR + " Do you wish to proceed?", MessageBoxButtons.YesNo) ''SR:2011.04.27 - YRS 5.0 1292 : OK button replaced by YES/NO button
    '	'	Session("Call_FinalDeathNotificationUpdation") = "Yes"
    '	'	DeathNotify = False
    '	'	Exit Function
    '	'End If

    '	'BS:2012.02.18:BT-941,YRS 5.0-1432:- here this message "Annuity payments after the date of death exist and might need to be reversed." has been removed instead of this display report List_of_Annuity_Checks_Sent_to_Retirees_After_Death.rpt
    '	If (l_string_returnStatusDR <> "") Then
    '		'BS:2012.02.20:BT-941,YRS 5.0-1432:-here we have set variable IsStatusDR = true which will be check on FinalDeathNotificationUpdation() to open report
    '		IsStatusDR = True
    '		Session("Call_FinalDeathNotificationUpdation") = "Yes"
    '		Exit Function
    '	End If
    '	''Ends,SR:2011.04.28 - YRS 5.0-1292 


    '	'Commented by SR : 2010.01.04  FOR GEMINI 635.
    '	'If (l_string_returnStatus = "1") And (l_string_returnStatus.Length = 1) Then
    '	'    l_string_Message = "There are disbursment checks that have been issued after the death date." + Chr(13)
    '	'    l_string_Message += "Please run the: 'List of Annuity Checks Sent to Retirees After Death' report"
    '	'    MessageBox.Show(PlaceHolder1, "Note before you proceed.", l_string_Message, MessageBoxButtons.OK)
    '	'    'by Aparna 12/09/2007 -Make the death notification process similar to that in Retirees Page
    '	'    Session("Call_FinalDeathNotificationUpdation") = "Yes"
    '	'    DeathNotify = False
    '	'    Exit Function
    '	'ElseIf (l_string_returnStatus <> "0") And (l_string_returnStatus.Length <> 1) Then '//lcResult<>"0" AND LEN(lcresult)>2
    '	'    '	There is some error text .. We are replicating what has been done in foxpro 
    '	'    MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification failed due to error:" + Chr(13) + l_string_returnStatus, MessageBoxButtons.OK)
    '	'    Exit Function
    '	'    'Put this Code on the UI 
    '	'End If

    '	'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.
    '	''Call the routine to do the final Updation 
    '	'FinalDeathNotificationUpdation()

    '	If FinalDeathNotificationUpdation() = False Then
    '		DeathNotify = False

    '	End If
    '	Session("CurrentProcesstoConfirm") = ""	'by aparna 16/12/2007
    '	'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.
    '	'start BS:2011.12.20:-not suppose to catch in sub method 
    '	'      Catch sqlEx As SqlException
    '	'	Dim l_String_Exception_Message As String
    '	'	l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
    '	'	Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)
    '	'Catch ex As Exception
    '	'	Dim l_String_Exception_Message As String
    '	'	l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '	'	ExceptionPolicy.HandleException(ex, "Exception Policy")
    '	'	Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '	'End Try
    '	'end BS:2011.12.20:-error thread being abort 

    '	'Death notification failed due to error:' + CHR(13)

    'End Function
    'BS:2012.02.18:BT-941,YRS 5.0-1432:- ProcessReport() use for pass parameter ReportName and FundNo. to report viewer
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
    'BS:2012.02.18:BT-941,YRS 5.0-1432:- OpenReportViewer() use to open ReportViewer.aspx 
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
            Dim l_string_returnDeathNotifystatus As String 'Dinesh K:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
            Dim l_int_returnStatus As Int16
            Dim l_string_Message As String
            Dim l_date_DeathDate As DateTime
            Dim l_string_TreatMLAs As String    'NP:IVP1:2008.04.15 - Adding parameter to input from users how ML should be treated (DA or DI)
            'BS:2012.03.02:BT-941,YRS 5.0-1432:declare out variable
            Dim l_string_showreport As String = String.Empty
            l_string_returnStatus = ""
            l_string_returnDeathNotifystatus = String.Empty
            l_string_Message = ""
            FinalDeathNotificationUpdation = True
            l_date_DeathDate = Session("DeathDate")
            l_string_TreatMLAs = Session("DeathMLQuestion") 'NP:IVP1:2008.04.15 - Obtaining value for the parameter from Session
            'IB:-BT:532 on 09/June/2010 validate page before Death Notifcation saving
            Page.Validate()
            If Page.IsValid() = False Then
                FinalDeathNotificationUpdation = False
                Exit Function
            End If
            'BS:2012.03.03:BT-941,YRS 5.0-1432:add new out variable for open report
            'Dinesh K:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death     
            'Add Parameter
            l_int_returnStatus = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DeathNotifyActions("PERSON", Session("PersId"), l_date_DeathDate, l_string_returnStatus, l_string_returnDeathNotifystatus, l_string_TreatMLAs, Me.TextBoxSSNo.Text, l_string_showreport)     'NP:IVP1:2008.04.15 - Adding parameter to decide how to treat ML
            If l_int_returnStatus < 0 Then
                'removed top n left values to avoid mesaagebox breaking in IE7 -Aparna 04/10/2007
                MessageBox.Show(PlaceHolder1, "Death Notification Status.", "Death notification failed due to error Network Error:" + Chr(13) + "Cannot Proceed", MessageBoxButtons.OK)
                FinalDeathNotificationUpdation = False
                Exit Function
            End If

            'Dinesh K:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
            'Anudeep:21.06.2013 : FOR allowing to open report BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
            'If l_string_returnDeathNotifystatus <> "" Then
            '    MessageBox.Show(PlaceHolder1, "Death Notification Status.", Chr(13) + l_string_returnDeathNotifystatus, MessageBoxButtons.OK)
            '    FinalDeathNotificationUpdation = False
            '    Me.TextBoxDeceasedDate.Text = Session("DeathDate")
            '    Exit Function
            'End If

            'BS:2012.03.05:BT-941,YRS 5.0-1432:-here put l_int_returnStatus = 0
            'If (l_string_returnStatus <> "") And l_int_returnStatus  Then
            If (l_string_returnStatus <> "") And l_int_returnStatus = 0 Then '(l_string_returnStatus.Length <> 1) Then '//lcResult<>"0" AND LEN(lcresult)>2
                'removed top n left values to avoid mesaagebox breaking in IE7 -Aparna 04/10/2007
                Dim strMessage As String

                'Anudeep:21.06.2013 : FOR allowing to open report BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
                If l_string_returnDeathNotifystatus <> "" Then
                    'Added by Dinesh Kanojia on 19/08/2013
                    'YRS 5.0-1698:Cross check SSN when entering a date of death             
                    strMessage = "Death notification failed due to error:" + l_string_returnStatus + "<br/>" + l_string_returnDeathNotifystatus
                    Page.RegisterStartupScript("Death Notification Status.", "<script language='javascript'> openDialogDeathNotify('" + strMessage + "');</script>")
                Else
                    'Added by Dinesh Kanojia on 19/08/2013
                    'YRS 5.0-1698:Cross check SSN when entering a date of death             
                    strMessage = "Death notification failed due to error:" + l_string_returnStatus
                    Page.RegisterStartupScript("Death Notification Status.", "openDialogDeathNotify('" + strMessage + "');")
                End If
                FinalDeathNotificationUpdation = False
                Exit Function
            Else
                Dim strMessage As String
                'removed top n left values to avoid mesaagebox breaking in IE7 -Aparna 04/10/2007
                'Anudeep:21.06.2013 : FOR allowing to open report BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
                If l_string_returnDeathNotifystatus <> "" Then
                    'Added by Dinesh Kanojia on 19/08/2013
                    'YRS 5.0-1698:Cross check SSN when entering a date of death             
                    strMessage = "<b>Death notification succeeded.</b>" + "<br/>" + l_string_returnDeathNotifystatus
                    'Page.RegisterStartupScript("Death Notification Status.", "<script language='javascript'> openDialogDeathNotify('" + strMessage + "');</script>") 'Bala: 01/05/2016: YRS-AT-1972 : calling it once after message modification is done
                Else
                    strMessage = "<b>Death notification succeeded.</b>"
                    'Added by Dinesh Kanojia on 19/08/2013
                    'YRS 5.0-1698:Cross check SSN when entering a date of death             
                    'Page.RegisterStartupScript("Death Notification Status.", "<script language='javascript'> openDialogDeathNotify('" + strMessage + "');</script>") 'Bala: 01/05/2016: YRS-AT-1972 : calling it once after message modification is done
                End If
                'Start: Bala: 01/05/2016: YRS-AT-1972: showing the message accordingly to the special death process.
                If chkSpecialDeathProcess.Checked Then
                    strMessage = String.Format("{0}<br/>Special death processing required. Please refer to the notes for further information.", strMessage)
                    MailUtil.SendMail(EnumEmailTemplateTypes.SPECIAL_DEATH_PROCESSING_REQUIRED, "", "", "", "", New Dictionary(Of String, String) From {{"FundNo", Session("FundNo").ToString()}, {"FirstName", TextBoxFirst.Text}, {"LastName", TextBoxLast.Text}}, "", Nothing, Web.Mail.MailFormat.Html)
                End If
                ' // START : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of deceased participant to regenerate and display Note message in death notification
                If (IsRMDRegenerateRequired()) Then
                    strMessage = String.Format("{0} <br/>Note: {1}", strMessage, Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DEATHSETTLEMENT_RMDREGENERATIONMESSAGEWARNING)
                End If
                ' // END : SB | 10/13/2016 | YRS-AT-3095 | Check RMD of deceased participant to regenerate and display Note message in death notification
                Page.RegisterStartupScript("Death Notification Status.", "<script language='javascript'> openDialogDeathNotify('" + strMessage + "');</script>")
                'End: Bala: 01/05/2016: YRS-AT-1972: showing the message accordingly to the special death process.
                'MessageBox.Show(PlaceHolder1, "Death Notification Status.", "'Death notification succeeded'", MessageBoxButtons.OK)
                Me.TextBoxDeceasedDate.Text = Session("DeathDate")
                FinalDeathNotificationUpdation = True
                'BS:2012.03.21:For BT-1015,YRS 5.0-1557:Clear Session
                Session("CurrentProcesstoConfirm") = ""
                'BS:2012.03.02:BT-941,YRS 5.0-1432:- if l_string_showreport is zero  then report will be open it has been set by zero on proc level  
                If (l_string_showreport = "0") Then
                    ProcessReport() 'call ProcessReport to open report: List of Uncashed Checks and Payments Issued After the Date of Death.rpt
                End If

                'Start :Anudeep:Added follwing code for Bt-1303 YRS 5.0-1707:New Death Benefit Application form 
                'After Succesfully death notification completed and surviour is not death beneficiary the open death benefit application form
                'Commented by anudeep 08.05.2013-to not to show report in participant maintanance
                'If Session("ShowDeathBenefitForm") = "Yes" Then
                '    SaveFormDetails() 'save the details and open the db form
                'End If
                'End :Anudeep:Added follwing code for Bt-1303 YRS 5.0-1707:New Death Benefit Application form 
                Exit Function
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    'Vipul 18Dec05 .. To aviod the problem of multiple windows, when we need to display 2 messages to the user.
    Private Sub DataGridAdditionalAccounts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridAdditionalAccounts.ItemDataBound
        Try
            'Commented By Ashutosh Patil as on 30-Mar-2007
            'YREN-3203,YREN-3205
            'If e.Item.ItemType = ListItemType.Header Then
            '    e.Item.Cells(4).Text = "AccountType"
            '    'Rahul 22 Feb,06
            '    'e.Item.Cells(5).Text = "Ymca"
            '    e.Item.Cells(5).Text = "YmcaNo"
            '    'Rahul 22 Feb,06
            '    e.Item.Cells(7).Text = "ContractType"
            '    e.Item.Cells(8).Text = "Contribution%"
            '    e.Item.Cells(9).Text = "ContributionAmt"
            '    e.Item.Cells(10).Text = "EffectiveDate"
            '    e.Item.Cells(11).Text = "TerminationDate"
            'End If
            'e.Item.Cells(1).Visible = False
            'e.Item.Cells(2).Visible = False
            'e.Item.Cells(3).Visible = False
            'e.Item.Cells(6).Visible = False

            'If e.Item.ItemType <> ListItemType.Header Then
            '    e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right
            '    e.Item.Cells(9).HorizontalAlign = HorizontalAlign.Right
            'End If
            ''anita and shubhrata date apr19
            'If e.Item.ItemType <> ListItemType.Header Then

            '    If e.Item.Cells(9).Text.ToUpper <> "&NBSP;" Then
            '        Dim l_decimal_try As Decimal
            '        l_decimal_try = Convert.ToDecimal(e.Item.Cells(9).Text)
            '        e.Item.Cells(9).Text = FormatCurrency(l_decimal_try)
            '    End If
            'End If
            'Ashutosh Patil as on 30-Mar-2007
            'YREN-3203,YREN-3205

            '-----Shashi Shekhar Singh:26-04-2011:Commented below existing code for BT-824  
            'If e.Item.ItemType = ListItemType.Header Then
            '    e.Item.Cells(m_const_int_AddAcctAccountType).Text = "AccountType"
            '    e.Item.Cells(m_const_int_AddAcctYmcaNo).Text = "YmcaNo"
            '    e.Item.Cells(m_const_int_AddAcctDescriptions).Text = "ContractType"
            '    e.Item.Cells(m_const_int_AddAcctContributionPer).Text = "Contribution%"
            '    e.Item.Cells(m_const_int_AddAcctContribution).Text = "ContributionAmt"
            '    e.Item.Cells(m_const_int_AddAcctEffectiveDate).Text = "EffectiveDate"
            '    e.Item.Cells(m_const_int_AddAcctTerminationDate).Text = "TerminationDate"
            'End If
            '-----------------------------------------------------------------------------


            e.Item.Cells(m_const_int_AddAcctUniqueId).Visible = False
            e.Item.Cells(m_const_int_AddAcctEmpEventId).Visible = False
            e.Item.Cells(m_const_int_AddAcctYmcaId).Visible = False
            e.Item.Cells(m_const_int_AddAcctBasisCode).Visible = False
            e.Item.Cells(m_const_int_AddAcctHireDate).Visible = False
            e.Item.Cells(m_const_int_AddAcctEnrollmentDate).Visible = False

            If e.Item.ItemType <> ListItemType.Header Then
                e.Item.Cells(m_const_int_AddAcctContributionPer).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(m_const_int_AddAcctContribution).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(m_const_int_AddAcctEffectiveDate).HorizontalAlign = HorizontalAlign.Center
                e.Item.Cells(m_const_int_AddAcctTerminationDate).HorizontalAlign = HorizontalAlign.Center
            End If

            If e.Item.ItemType <> ListItemType.Header Then
                If e.Item.Cells(m_const_int_AddAcctContribution).Text.ToUpper <> "&NBSP;" Then
                    Dim l_decimal_try As Decimal
                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(m_const_int_AddAcctContribution).Text)
                    e.Item.Cells(m_const_int_AddAcctContribution).Text = FormatCurrency(l_decimal_try)
                End If
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


    'ItemCommand Added by Dinesh Kanojia 12/13/2012
    Private Sub DatagridAdditionalAccounts_ItemCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles DataGridAdditionalAccounts.ItemCommand
        Try
            If e.CommandName.ToLower = "edit" Then

                Dim Image_Edit_Button = e.Item.FindControl("ImageEditButtonAccounts")

                If Not Image_Edit_Button Is Nothing Then
                    'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
                    'AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
                    Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAccountUpdateItem", Convert.ToInt32(Session("LoggedUserKey")))
                    If Not checkSecurity.Equals("True") Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    'End : YRS 5.0-940
                End If
                EditVoluntaryAccounts(e.Item.Cells, e.Item.ItemIndex)

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub



    Private Sub ButtonAddItem_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles ButtonAddItem.Command
        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
        HiddenFieldDirty.Value = "true"
    End Sub



    'Function Created by Dinesh Kanojia on 12/13/2012
    Public Sub EditParticipantEmployement(ByVal DatagridRow As TableCellCollection, ByVal iIndex As Integer)
        Try

            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Session("DateValidation") = ""
            Me.MakeLinkVisible()

            Session("Flag") = "EditEmployment"
            Session("HireDateP") = DatagridRow(m_const_int_HireDate).Text
            'Session("UniqueIdP") = DatagridRow(1).Text
            Session("HireDateP") = DatagridRow(m_const_int_HireDate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(2).Text
            If DatagridRow(m_const_int_Termdate).Text <> "&nbsp;" Then  'If Me.DataGridParticipantEmployment.SelectedItem.Cells(3).Text <> "&nbsp;" Then
                Session("TermDateP") = DatagridRow(m_const_int_Termdate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(3).Text
            Else
                Session("TermDateP") = ""
            End If
            If DatagridRow(m_const_int_EligibilityDate).Text <> "&nbsp;" Then 'If Me.DataGridParticipantEmployment.SelectedItem.Cells(4).Text <> "&nbsp;" Then
                Session("EligibilityDateP") = DatagridRow(m_const_int_EligibilityDate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(4).Text
            Else
                Session("EligibilityDateP") = ""
            End If
            If DatagridRow(m_const_int_Professional).Text <> "&nbsp;" Then 'If Me.DataGridParticipantEmployment.SelectedItem.Cells(5).Text <> "&nbsp;" Then
                Session("ProfessionalP") = DatagridRow(m_const_int_Professional).Text
            Else
                Session("ProfessionalP") = "False"
            End If

            '------------------------------------------------------------------------------------------------------------------
            'Shashi Shekhar: 2010-12-14: For BT-700 Unable to change Hire and enrollment date for Retired Active participant
            If DatagridRow(m_const_int_Salaried).Text <> "&nbsp;" Then 'Me.DataGridParticipantEmployment.SelectedItem.Cells(6).Text <> "&nbsp;" Then
                'Added & Commented by Dilip yadav : 11-Nov-2009 : YRS 5.0.941 : To enable 4 fields
                'Session("ExemptP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_Active).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(6).Text
                Session("ExemptP") = DatagridRow(m_const_int_Salaried).Text
                '------------------------------------------------------------------------------------------------------
            Else
                Session("ExemptP") = "False"
            End If
            If DatagridRow(m_const_int_FullTime).Text <> "&nbsp;" Then 'Me.DataGridParticipantEmployment.SelectedItem.Cells(7).Text <> "&nbsp;" Then
                Session("FullTimeP") = DatagridRow(m_const_int_FullTime).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(7).Text
            Else
                Session("FullTimeP") = "False"
            End If

            If DatagridRow(m_const_int_PriorService).Text <> "&nbsp;" Then 'Me.DataGridParticipantEmployment.SelectedItem.Cells(8).Text <> "&nbsp;" Then
                Session("PriorServiceP") = DatagridRow(m_const_int_PriorService).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(8).Text
            Else
                Session("PriorServiceP") = "0"
            End If
            'Commented & Added by Dilip yadav : 13-Nov-2009 : YRS 5.0.941 : BT 1024
            'Session("StatusTypeP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_StatusType).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(9).Text
            Session("StatusTypeP") = Trim(DatagridRow(m_const_int_StatusType).Text)

            'Added by DILIP YADAV for issue id yrs 5.0.941
            'Session("StatusTypeP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_StatusType).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(9).Text
            Session("StatusTypeP") = Trim(DatagridRow(m_const_int_StatusType).Text)
            Session("StatusDateP") = DatagridRow(m_const_int_StatusDate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(10).Text

            Session("YmcaNameP") = DatagridRow(m_const_int_YmcaName).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(12).Text
            'Added by Dilip yadav : 13-Nov-2009 : YRS 5.0.941 : BT 1024
            Session("PositionTypeP") = DatagridRow(m_const_int_PositionType).Text
            Session("PositionDescP") = DatagridRow(m_const_int_PositionDesc).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(15).Text

            If DatagridRow(m_const_int_BasicPaymentDate).Text() <> "&nbsp;" Then 'If Me.DataGridParticipantEmployment.SelectedItem.Cells(16).Text() <> "&nbsp;" Then
                Session("BasicPaymentDateP") = DatagridRow(m_const_int_BasicPaymentDate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(16).Text
            Else
                Session("BasicPaymentDateP") = ""
            End If

            ' START: SB | 03/23/2017 | YRS-AT-2606 | YMCA Id is stored in session variable which is further used in adding or updating employments  
            If DatagridRow(m_const_int_YmcaId).Text() <> "&nbsp;" Then
                Me.SelectedYMCAID = DatagridRow(m_const_int_YmcaId).Text
            Else
                Me.SelectedYMCAID = ""
            End If
            ' END: SB | 03/23/2017 | YRS-AT-2606 | YMCA Id is stored in session variable which is further used in adding or updating employments


            _icounter = Session("icounter")
            _icounter = _icounter + 1
            Session("icounter") = _icounter
            If (_icounter = 1) Then
                Dim popupScript As String

                If DatagridRow(m_const_int_UniqueId).Text = "&nbsp;" Then 'If Me.DataGridParticipantEmployment.SelectedItem.Cells(1).Text = "&nbsp;" Then
                    '   popupScript = "<script language='javascript'>" & _
                    '"window.open('AddEmployment.aspx?Index=" & Me.DataGridParticipantEmployment.SelectedIndex & "', 'CustomPopUp_UpdateEmp', " & _
                    '"'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                    '"</script>"
                    popupScript = "<script language='javascript'>" & _
              "window.open('AddEmployment.aspx?Index=" & iIndex & "', 'CustomPopUp_UpdateEmp', " & _
              "'width=800, height=500, menubar=no, Resizable=No,top=120,left=120, scrollbars=yes')" & _
              "</script>"

                Else
                    '     popupScript = "<script language='javascript'>" & _
                    '"window.open('AddEmployment.aspx?UniqueIdP=" + Me.DataGridParticipantEmployment.SelectedItem.Cells(1).Text + "','CustomPopUp_UpdateEmp', " & _
                    '"'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                    '"</script>"
                    ' End If
                    popupScript = "<script language='javascript'>" & _
               "window.open('AddEmployment.aspx?UniqueIdP=" + DatagridRow(m_const_int_UniqueId).Text + "','CustomPopUp_UpdateEmp', " & _
               "'width=800, height=500, menubar=no, Resizable=no,top=120,left=120, scrollbars=yes')" & _
               "</script>"
                End If

                Page.RegisterStartupScript("PopupScript_UpdateEmployment", popupScript)
            Else

                'Vipul 01Feb06 Cache-Session
                'Me.DataGridParticipantEmployment.DataSource = CType(Cache("l_dataset_Employment"), DataSet)
                'viewstate("Ds_Sort_employment") = CType(Cache("l_dataset_Employment"), DataSet)
                Me.DataGridParticipantEmployment.DataSource = DirectCast(Session("l_dataset_Employment"), DataSet)
                ViewState("Ds_Sort_employment") = DirectCast(Session("l_dataset_Employment"), DataSet)
                'Vipul 01Feb06 Cache-Session

                Me.DataGridParticipantEmployment.DataBind()
                _icounter = 0
                Session("icounter") = _icounter
            End If
            'Else
            'Exit Sub
            'End If
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


    Public Sub LoadNotesTab()
        Try
            'Vipul 01Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager()
            Dim dr As DataRow
            Dim l_datatable_Notes As DataTable

            Dim l_string_PersId As String
            l_string_PersId = Session("PersId")
            'AA:2013.10.23 - Bt-2264: below code and has been placed from session("flag") condition
            l_datatable_Notes = Session("dtNotes")

            If Session("Flag") = "AddNotes" Then

                'Vipul 01Feb06 Cache-Session
                'l_datatable_Notes = Cache("dtNotes")
                'AA:2013.10.23 - Bt-2264:commented below code and has been placed top of session("flag") condition
                'l_datatable_Notes = Session("dtNotes")
                'Vipul 01Feb06 Cache-Session
                'Start: Bala: 01/12/2016: This is handled in UpdateNotes.aspx page
                'If l_datatable_Notes Is Nothing Then
                '    l_datatable_Notes = New DataTable
                '    'By aparna -YREN-3115 08/03/2007
                '    l_datatable_Notes.Columns.Add("UniqueID")
                '    'By aparna -YREN-3115 08/03/2007
                '    l_datatable_Notes.Columns.Add("Note")
                '    l_datatable_Notes.Columns.Add("Creator")
                '    l_datatable_Notes.Columns.Add("Date")
                '    'By aparna -YREN-3115 08/03/2007 -New column
                '    l_datatable_Notes.Columns.Add("bitImportant")

                'End If
                ''If Not (Session("blnUpdateNotes") = True) Then
                'dr = l_datatable_Notes.NewRow
                ''By aparna -YREN-3115 08/03/2007
                'dr("UniqueID") = Guid.NewGuid()
                ''By aparna -YREN-3115 08/03/2007
                'dr("Note") = Session("Note")
                'dr("PersonID") = l_string_PersId
                'dr("Date") = Date.Now
                ''By aparna -YREN-3115 08/03/2007
                'dr("bitImportant") = Session("BitImportant")
                ''Vipul - to fix the Creator Bug 04-Feb-06
                'dr("Creator") = Session("LoginId")
                ''Vipul - to fix the Creator Bug 04-Feb-06

                'l_datatable_Notes.Rows.Add(dr)
                'End If
                l_datatable_Notes = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.MemberNotes(l_string_PersId)
                'End: Bala: 01/12/2016: This is handled in UpdateNotes.aspx page

                'After adding the row to the dataset reset the session variable
                'Session("blnAddNotes") = False
                'Session("blnUpdateNotes") = False
            Else
                If l_datatable_Notes Is Nothing Then
                    l_datatable_Notes = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.MemberNotes(l_string_PersId)
                    'Dim l_dsNotes As New DataSet
                    'l_dsNotes.Tables.Add(l_datatable_Notes)
                    'viewstate("DS_Sort_Notes") = l_datatable_Notes
                Else

                    'Vipul 01Feb06 Cache-Session
                    'l_datatable_Notes = Cache("dtNotes")
                    l_datatable_Notes = Session("dtNotes")
                    'Dim l_dsNotes As New DataSet
                    'l_dsNotes.Tables.Add(l_datatable_Notes)
                    'viewstate("DS_Sort_Notes") = l_datatable_Notes
                    'Vipul 01Feb06 Cache-Session

                End If
            End If
            'Added by Swopna 16 Jan,2008 in response to YREN-4126 reopened
            '*************
            Dim dv As New DataView
            Dim l_dr_notes As DataRow
            Dim l_dt_new_notes As New DataTable
            Dim j As Integer = 0
            Dim SortExpressionNotes As String
            SortExpressionNotes = "Date DESC"
            dv = l_datatable_Notes.DefaultView
            dv.Sort = SortExpressionNotes
            Session("Participant_Sort") = dv.Sort
            '*************
            NotesFlag.Value = "None"
            If (l_datatable_Notes.Rows.Count > 0) Then
                NotesFlag.Value = "Notes"
                'DataGridParticipantNotes.SelectedIndex = -1
                DataGridParticipantNotes.DataSource = l_datatable_Notes
                'Shubhrata YRSPS4614
                Me.MakeDisplayNotesDataTable(l_datatable_Notes)
                'Shubhrata YRSPS4614
                'Session("DisplayNotes") = l_datatable_Notes

                DataGridParticipantNotes.DataBind()
            End If

            'by Aparna -YREN-3115 16/03/2007
            'If Me.NotesFlag.Value = "Notes" Then
            '    Me.TabStripParticipantsInformation.Items(5).Text = "<font color=orange>Notes</font>"
            'ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
            '    Me.TabStripParticipantsInformation.Items(5).Text = "<font color=red>Notes</font>"
            'Else
            '    Me.TabStripParticipantsInformation.Items(5).Text = "Notes"
            'End If


            'Vipul 01Feb06 Cache-Session
            'Cache.Add("dtNotes", l_datatable_Notes)
            Session("dtNotes") = l_datatable_Notes
            'Vipul 01Feb06 Cache-Session
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

    Private Sub ViewParticipantNotes(ByVal DatagridRow As TableCellCollection, ByVal iIndex As Integer)
        Try
            'If Me.DataGridParticipantNotes.SelectedIndex <> -1 Then
            Dim l_string_Notes As String
            ' Session("Note") = Me.DataGridParticipantNotes.SelectedItem.Cells(3).Text.Trim
            'anita
            Dim l_datatable_Notes As DataTable
            l_datatable_Notes = Session("DisplayNotes")
            'l_datatable_Notes = Session("dtNotes")
            ' Dim l_datarows_notes As DataRow()
            Dim l_date_notes As String
            Dim l_creator_notes As String
            Dim dr_notes As DataRow
            'by aparna get the checkbox value  YREN-3115
            Dim l_checkbox As New CheckBox
            l_date_notes = DatagridRow(1).Text
            l_creator_notes = DatagridRow(2).Text

            'Commneted By Dinesh Kanojia on 14/12/2012
            'l_checkbox = DataGridParticipantNotes.SelectedItem.FindControl("CheckBoxImportant")
            l_checkbox = DataGridParticipantNotes.Items(iIndex).FindControl("CheckBoxImportant")
            'Added by Swopna in respone to YREN-4126(8 Jan,2008)
            '************
            '--------Commented by Swopna on 8 Feb,2008 in response to YRPS-4614
            'Session("Note") = Me.DataGridParticipantNotes.SelectedItem.Cells(3).Text.Trim
            '************
            'Added by Swopna in respone to YRPS-4614(8 Feb,2008)
            '************
            'Commented By Dinesh Kanojia on 14/12/2012.
            'Dim index As Integer
            'index = Me.DataGridParticipantNotes.SelectedItem.ItemIndex

            If l_datatable_Notes.Rows(iIndex)("Note").GetType.ToString <> "System.DBNull" Then
                Session("Note") = l_datatable_Notes.Rows(iIndex)("Note")
            Else
                Session("Note") = ""
            End If
            '************
            If l_checkbox.Checked Then
                Session("BitImportant") = True
            Else
                Session("BitImportant") = False
            End If
            'Commented by Swopna in respone to YREN-4126(8 Jan,2008)
            '*********
            'For Each dr_notes In l_datatable_Notes.Rows
            '    If dr_notes("Date").ToString() = l_date_notes Then
            '        If dr_notes("Creator") = l_creator_notes Then
            '            Session("Note") = dr_notes("Note")
            '            Exit For
            '        End If

            '    End If

            'Next
            '**********

            ' l_datarows_notes = l_datatable_Notes.Select("Date='" & Convert.ToDateTime(l_string_notes_new) & "' And Creator='" & l_creator_notes & "'")
            'l_datarows_notes = l_datatable_Notes.Select("Date='" & l_string_notes_new & "' And Creator='" & l_creator_notes & "'")
            '  If l_datarows_notes.Length > 0 Then
            ' Session("Note") = l_datarows_notes(0)("Note")
            'End If

            'anita gemini
            Dim popupScript As String
            If DatagridRow(4).Text = "&nbsp;" Then 'If Me.DataGridParticipantNotes.SelectedItem.Cells(4).Text = "&nbsp;" Then
                'popupScript = "<script language='javascript'>" & _
                '                                   "window.open('UpdateNotes.aspx?Index=" & Me.DataGridParticipantNotes.SelectedIndex & "', 'CustomPopUp', " & _
                '                                   "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                '                                   "</script>"
                popupScript = "<script language='javascript'>" & _
                                                   "window.open('UpdateNotes.aspx?Index=" & iIndex & "', 'CustomPopUp', " & _
                                                   "'width=800, height=400, menubar=no, Resizable=NO,top=120,left=120, scrollbars=yes')" & _
                                                   "</script>"
            Else
                popupScript = "<script language='javascript'>" & _
                                                   "window.open('UpdateNotes.aspx?UniqueID=" & DatagridRow(4).Text & "', 'CustomPopUp', " & _
                                                   "'width=800, height=400, menubar=no, Resizable=No,top=120,left=120, scrollbars=yes')" & _
                                                   "</script>"

            End If


            Page.RegisterStartupScript("PopupScript2", popupScript)
            'Else
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select notes to display.", MessageBoxButtons.OK)
            'Exit Sub
            'End If
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
    Private Sub ButtonView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonView.Click

        Try
            If Me.DataGridParticipantNotes.SelectedIndex <> -1 Then
                Dim l_string_Notes As String
                ' Session("Note") = Me.DataGridParticipantNotes.SelectedItem.Cells(3).Text.Trim
                'anita
                Dim l_datatable_Notes As DataTable
                l_datatable_Notes = Session("DisplayNotes")
                'l_datatable_Notes = Session("dtNotes")
                ' Dim l_datarows_notes As DataRow()
                Dim l_date_notes As String
                Dim l_creator_notes As String
                Dim dr_notes As DataRow
                'by aparna get the checkbox value  YREN-3115
                Dim l_checkbox As New CheckBox
                l_date_notes = DataGridParticipantNotes.SelectedItem.Cells(1).Text
                l_creator_notes = DataGridParticipantNotes.SelectedItem.Cells(2).Text
                l_checkbox = DataGridParticipantNotes.SelectedItem.FindControl("CheckBoxImportant")
                'Added by Swopna in respone to YREN-4126(8 Jan,2008)
                '************
                '--------Commented by Swopna on 8 Feb,2008 in response to YRPS-4614
                'Session("Note") = Me.DataGridParticipantNotes.SelectedItem.Cells(3).Text.Trim
                '************
                'Added by Swopna in respone to YRPS-4614(8 Feb,2008)
                '************
                Dim index As Integer
                index = Me.DataGridParticipantNotes.SelectedItem.ItemIndex
                If l_datatable_Notes.Rows(index)("Note").GetType.ToString <> "System.DBNull" Then
                    Session("Note") = l_datatable_Notes.Rows(index)("Note")
                Else
                    Session("Note") = ""
                End If
                '************
                If l_checkbox.Checked Then
                    Session("BitImportant") = True
                Else
                    Session("BitImportant") = False
                End If
                'Commented by Swopna in respone to YREN-4126(8 Jan,2008)
                '*********
                'For Each dr_notes In l_datatable_Notes.Rows
                '    If dr_notes("Date").ToString() = l_date_notes Then
                '        If dr_notes("Creator") = l_creator_notes Then
                '            Session("Note") = dr_notes("Note")
                '            Exit For
                '        End If

                '    End If

                'Next
                '**********

                ' l_datarows_notes = l_datatable_Notes.Select("Date='" & Convert.ToDateTime(l_string_notes_new) & "' And Creator='" & l_creator_notes & "'")
                'l_datarows_notes = l_datatable_Notes.Select("Date='" & l_string_notes_new & "' And Creator='" & l_creator_notes & "'")
                '  If l_datarows_notes.Length > 0 Then
                ' Session("Note") = l_datarows_notes(0)("Note")
                'End If

                'anita gemini
                Dim popupScript As String
                If Me.DataGridParticipantNotes.SelectedItem.Cells(4).Text = "&nbsp;" Then
                    popupScript = "<script language='javascript'>" & _
                                                       "window.open('UpdateNotes.aspx?Index=" & Me.DataGridParticipantNotes.SelectedIndex & "', 'CustomPopUp', " & _
                                                       "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                                                       "</script>"
                Else
                    popupScript = "<script language='javascript'>" & _
                                                       "window.open('UpdateNotes.aspx?UniqueID=" & Me.DataGridParticipantNotes.SelectedItem.Cells(4).Text & "', 'CustomPopUp', " & _
                                                       "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                                                       "</script>"

                End If


                Page.RegisterStartupScript("PopupScript2", popupScript)
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select notes to display.", MessageBoxButtons.OK)
                Exit Sub
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

    Private Sub ButtonAddEmployment_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles ButtonAddEmployment.Command

    End Sub

    Private Sub DataGridPartAcctContribution_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridPartAcctContribution.ItemDataBound
        Try
            If e.Item.ItemType <> ListItemType.Header Then
                e.Item.Cells(0).HorizontalAlign = HorizontalAlign.Left
                e.Item.Cells(1).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(2).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(3).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(4).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(5).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right


            End If

            If e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(8).Text = "Account  Total"
            End If
            'anita and shubhrata date apr19
            If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" And e.Item.Cells(0).Text.ToUpper <> "ACCT" Then


                Dim l_decimal_try As Decimal
                l_decimal_try = Convert.ToDecimal(e.Item.Cells(1).Text)
                e.Item.Cells(1).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(2).Text)
                e.Item.Cells(2).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(3).Text)
                e.Item.Cells(3).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(4).Text)
                e.Item.Cells(4).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(5).Text)
                e.Item.Cells(5).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(6).Text)
                e.Item.Cells(6).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
                e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(8).Text)
                e.Item.Cells(8).Text = FormatCurrency(l_decimal_try)
            End If


            'anita and shubhrata date apr19
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub DataGridActiveBeneficiaries_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridActiveBeneficiaries.ItemDataBound
        ' // START : SB | 07/07/2016 | YRS-AT-2382 | For Non human and New Beneficiaries make "view ssn updates " label false
        Dim viewLink As System.Web.UI.WebControls.HyperLink
        Try
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                If Not IsRelationshipTypeIsHumanBenaficiary(RelationShipCheck, e.Item.Cells(BENEFICIARY_REL).Text) Then
                    viewLink = DirectCast(e.Item.Cells(BENEFICIARY_TaxID).Controls(3), System.Web.UI.WebControls.HyperLink)
                    viewLink.Visible = False
                ElseIf (e.Item.Cells(BENEFICIARY_IsExistingAudit).Text = "0") Then
                    viewLink = DirectCast(e.Item.Cells(BENEFICIARY_TaxID).Controls(3), System.Web.UI.WebControls.HyperLink)
                    viewLink.Visible = False
                End If

                If String.IsNullOrEmpty(e.Item.Cells(BENEFICIARY_UniqueID).Text.Replace("&nbsp;", "").Trim) And Not String.IsNullOrEmpty(e.Item.Cells(BENEFICIARY_New).Text.Replace("&nbsp;", "").Trim) Then
                    viewLink = DirectCast(e.Item.Cells(BENEFICIARY_TaxID).Controls(3), System.Web.UI.WebControls.HyperLink)
                    viewLink.Visible = False
                End If
            End If
            'e.Item.Cells(1).Visible = False
            'e.Item.Cells(2).Visible = False
            'e.Item.Cells(3).Visible = False
            'e.Item.Cells(4).Visible = False
            ''e.Item.Cells(10).Visible = False
            'e.Item.Cells(13).Visible = False
            'e.Item.Cells(14).Visible = False
            'If e.Item.ItemType <> ListItemType.Header Then
            '    e.Item.Cells(15).HorizontalAlign = HorizontalAlign.Right
            'End If


            'e.Item.Cells(2).Visible = False
            'e.Item.Cells(3).Visible = False
            'e.Item.Cells(4).Visible = False
            'e.Item.Cells(5).Visible = False
            ''e.Item.Cells(10).Visible = False
            'e.Item.Cells(14).Visible = False
            'e.Item.Cells(15).Visible = False
            'If e.Item.ItemType <> ListItemType.Header Then
            '    e.Item.Cells(16).HorizontalAlign = HorizontalAlign.Right
            'End If
            ''Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            'e.Item.Cells(20).Visible = False

            ''SP 2014.11.27 BT-2310\YRS 5.0-2255 - Start
            'e.Item.Cells(21).Visible = False
            'e.Item.Cells(22).Visible = False
            'e.Item.Cells(23).Visible = False
            'e.Item.Cells(24).Visible = False
            'SP 2014.11.27 BT-2310\YRS 5.0-2255 - End
            ' // END : SB | 07/07/2016 | YRS-AT-2382 | For Non human and New Beneficiaries make "view ssn updates " label false
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Public Sub LoadBeneficiariesTab()
        Try
            Dim l_string_PersId As String
            Dim l_dataset_Active As New DataSet
            Dim l_arraylist_Access As ArrayList
            Dim dsAddress As New DataSet
            'START: MMR | 2017.12.04 | YRS-AT-3756 | Declared object variable
            Dim beneficiaryOfDeceasedBeneficairy As New DataSet
            Dim beneficiaryTable As New DataTable
            'END: MMR | 2017.12.04| YRS-AT-3756 | Declared object variable
            Dim i As Integer

            l_string_PersId = Session("PersId")
            If Session("Flag") = "AddBeneficiaries" Or Session("Flag") = "EditBeneficiaries" Or Session("Flag") = "DeleteBeneficiaries" Then
                If Not Session("BeneficiariesActive") Is Nothing Then
                    l_dataset_Active = Session("BeneficiariesActive")

                    'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
                    'check beneficiary percentage is modified
                    If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesActive"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                        IsModifiedPercentage = 1
                    End If
                    'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End

                End If
            Else
                If Session("Flag") = "Active" Or Session("Flag") = "" Then
                    l_dataset_Active = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpActiveBeneficiariesInfo(l_string_PersId)
                    OriginalBeneficiaries = l_dataset_Active.Copy() 'SP 2014.12.02  BT-2310\YRS 5.0-2255:
                    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    dsAddress = Address.GetAddressOfBeneficiariesByPerson(l_string_PersId, EnumEntityCode.PERSON)
                    If Not dsAddress Is Nothing Then
                        Session("BeneficiaryAddress") = dsAddress.Tables(0)
                    End If
                End If
            End If
            ' Check if user can change the beneficiaries for this status type
            l_arraylist_Access = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.SetBeneficiaryAccess(l_string_PersId)
            If l_arraylist_Access.Item(0).ToString() = "1" Then
                DataGridActiveBeneficiaries.Visible = True
                LabelNotSet.Visible = False
                Session("WithDrawn_member") = False
            Else
                l_dataset_Active = Nothing
                LabelNotSet.Visible = True
                Session("WithDrawn_member") = True
            End If
            'ASHISH:2010.03.12:Added for Thread being aborted error, if there is no active beneficiary
            'If isNonEmpty(l_dataset_Active) Then
            '    Session("BeneficiariesActive") = l_dataset_Active
            'Else
            '    Session("BeneficiariesActive") = Nothing
            'End If

            'Sanjay Rawat :2010.05.10:YRS 5.0 - 1078 :above commented line of ashish assign nothing to dataset even if it has structure defined 
            If l_dataset_Active Is Nothing Then
                Session("BeneficiariesActive") = Nothing
            Else
                If (l_dataset_Active.Tables.Count >= 0) Then
                    Session("BeneficiariesActive") = l_dataset_Active
                    'AA:13.11.2013 :BT:1455:YRS 5.1 1733: Added below code to Hide the contingency level 2 percentage text box whwn there are no contingency level 2 beneficiary(ies) does not exists 
                    If l_dataset_Active.Tables(0).Select("Lvl = 'LVL2'").Length = 0 Then
                        LabelCont2A.Visible = False
                        TextBoxCont2InsA.Visible = False
                        TextBoxCont2A.Visible = False
                    Else
                        LabelCont2A.Visible = True
                        TextBoxCont2InsA.Visible = True
                        TextBoxCont2A.Visible = True
                    End If

                    'AA:13.11.2013 :BT:1455:YRS 5.1 1733: Added below code to Hide the contingency level 3 percentage text box whwn there are no contingency level 3 beneficiary(ies) does not exists 
                    If l_dataset_Active.Tables(0).Select("Lvl = 'LVL3'").Length = 0 Then
                        LabelCont3A.Visible = False
                        TextBoxCont3InsA.Visible = False
                        TextBoxCont3A.Visible = False
                    Else
                        LabelCont3A.Visible = True
                        TextBoxCont3InsA.Visible = True
                        TextBoxCont3A.Visible = True
                    End If
                Else
                    Session("BeneficiariesActive") = Nothing
                End If
            End If
            'End of code for sSanjay Rawat :2010.05.10:YRS 5.0 - 1078

            'ASHISH:2010.03.12:Commented for Thread being aborted error, if there is no active beneficiary
            'Session("BeneficiariesActive") = l_dataset_Active

            'NP:PS:2007.06.28 - Performing Final checks after load beneficiary
            If isNonEmpty(l_dataset_Active) Then
                CalculateValues(l_dataset_Active.Tables(0), "A")
            End If

            If Session("BeneficiariesActive") Is Nothing Then
                DataGridActiveBeneficiaries.Visible = False
                HideActiveParticipantsInformation()
            Else
                DataGridActiveBeneficiaries.Visible = True
                DataGridActiveBeneficiaries.DataSource = Session("BeneficiariesActive")
                ViewState("DS_Sort_ActiveBeneficiaries") = Session("BeneficiariesActive")
                DataGridActiveBeneficiaries.DataBind()
            End If

            PrepareAuditTable()   ' // SB | 07/07/2016 | YRS-AT-2382 | For Adding Beneficiaries in datatable to  maintain changed SSN updates
        Catch sqlEx As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            'START: VC| 2018.13.11 | YRS-AT-4018 | Log exceptions
            ExceptionPolicy.HandleException(sqlEx, "Exception Policy")
            HelperFunctions.LogException(sqlEx.Message, sqlEx.InnerException)
            'END: VC| 2018.13.11 | YRS-AT-4018 | Log exceptions
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            HelperFunctions.LogException(ex.Message, ex.InnerException) ' VC| 2018.13.11 | YRS-AT-4018 | Log exceptions
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub
    Public Sub HideActiveParticipantsInformation()
        TextBoxPrimaryA.Visible = False
        TextBoxCont1A.Visible = False
        TextBoxCont2A.Visible = False
        TextBoxCont3A.Visible = False
        ButtonPriA.Visible = False
        ButtonCont1A.Visible = False
        ButtonCont2A.Visible = False
        ButtonCont3A.Visible = False
        LabelPercentage1.Visible = False
        'Label9.Visible = False
        LabelPrimaryA.Visible = False
        LabelCont1A.Visible = False
        LabelCont2A.Visible = False
        LabelCont3A.Visible = False
        ButtonAddActive.Visible = False
        ButtonEditActive.Visible = False
        ButtonDeleteActive.Visible = False
        Label1.Visible = False
        TextBoxPrimaryInsA.Visible = False : ButtonPriInsA.Visible = False 'YSPS-3905:2007.10.15 - Hiding Equate buttons also
        TextBoxCont1InsA.Visible = False : ButtonCont1InsA.Visible = False 'YSPS:2007.10.15 - Hiding Equate buttons also
        TextBoxCont2InsA.Visible = False : ButtonCont2InsA.Visible = False 'YSPS:2007.10.15 - Hiding Equate buttons also
        TextBoxCont3InsA.Visible = False : ButtonCont3InsA.Visible = False 'YSPS:2007.10.15 - Hiding Equate buttons also
        Label7.Visible = False : Label8.Visible = False : LabelPercentage1.Visible = False 'YSPS:2007.10.15 - Hiding Labels also
    End Sub
#Region "Beneficiary - Active - Equalize Functions"
#Region "Beneficiary - Active - Primary"
    Private Sub ButtonPriA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPriA.Click
        Try
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
            'Me.Equalize(Session("BeneficiariesActive"), "P", "A")
            Me.Equalize(Session("BeneficiariesActive"), "PRIM", "", "MEMBER")
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"

            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesActive"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
                Exit Sub
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End

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
    Private Sub ButtonPriInsA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPriInsA.Click
        Try
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
            'Me.Equalize(Session("BeneficiariesActive"), "PB", "A")
            Me.Equalize(Session("BeneficiariesActive"), "PRIM", "", "SAVING")
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesActive"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
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
#End Region
#Region "Beneficiary - Active - Contingent Level 1"
    Private Sub ButtonCont1A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont1A.Click
        Try
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
            'Me.Equalize(Session("BeneficiariesActive"), "C1", "A")
            Me.Equalize(Session("BeneficiariesActive"), "CONT", "1", "MEMBER")

            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesActive"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
                Exit Sub
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
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
    Private Sub ButtonCont1InsA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont1InsA.Click
        Try
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
            'Me.Equalize(Session("BeneficiariesActive"), "C1B", "A")
            Me.Equalize(Session("BeneficiariesActive"), "CONT", "1", "SAVING")

            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            'check beneficiary percentage is modified
            If IsBeneficiaryPercentageChanged(dsModifiedBeneficiaires:=Session("BeneficiariesActive"), dsOriginalBeneficiaries:=OriginalBeneficiaries, persId:=Session("persid")) Then
                IsModifiedPercentage = 1
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End
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
#End Region
#Region "Beneficiary - Active - Contingent Level 2"
    Private Sub ButtonCont2A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont2A.Click
        Try
            'Me.Equalize(Session("BeneficiariesActive"), "C2", "A")
            Me.Equalize(Session("BeneficiariesActive"), "CONT", "2", "MEMBER")
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
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
    Private Sub ButtonCont2InsA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont2InsA.Click
        Try
            'Me.Equalize(Session("BeneficiariesActive"), "C2B", "A")
            Me.Equalize(Session("BeneficiariesActive"), "CONT", "2", "SAVING")
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
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
#End Region
#Region "Beneficiary - Active - Contingent Level 3"
    Private Sub ButtonCont3A_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont3A.Click
        Try
            'Me.Equalize(Session("BeneficiariesActive"), "C3", "A")
            Me.Equalize(Session("BeneficiariesActive"), "CONT", "3", "MEMBER")
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
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
    Private Sub ButtonCont3InsA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCont3InsA.Click
        Try
            'Me.Equalize(Session("BeneficiariesActive"), "C3B", "A")
            Me.Equalize(Session("BeneficiariesActive"), "CONT", "3", "SAVING")
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
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
#End Region
#End Region
    'Private Sub Equalize(ByVal ds As DataSet, ByVal lcGroup As String, ByVal lcBeneficiaryCategory As String)
    '    Try
    '        Dim i As Integer
    '        Dim lncnt As Integer
    '        Dim dt As DataTable
    '        'Vipul 01Feb06 Cache-Session
    '        'Dim Cache As CacheManager
    '        'Cache = CacheFactory.GetCacheManager()
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

    '            'Aparna    YREN-2849-Change precision to 8 decimals
    '            'lnVal1Portion = Convert.ToInt32((100.0 / lncnt) * 10000) / 10000
    '            'lnValPortionTimesCountMinus1 = Convert.ToInt32(lnVal1Portion * (lncnt - 1) * 100000) / 100000
    '            lnVal1Portion = Convert.ToDouble(((100.0 / lncnt) * 10000) / 10000)
    '            lnValPortionTimesCountMinus1 = Convert.ToDouble((lnVal1Portion * (lncnt - 1) * 100000) / 100000)
    '            'Aparna    YREN-2849-Change precision to 8 decimals
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
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If


    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "C1" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "C2" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "A" _
    '                And lcGroup = "C3" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                'Retirement Retired db Benefit

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "P" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C1" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C2" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C3" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "RETIRE" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                'Retirement insured reserve benefit

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "PB" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "PRIM" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C1B" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL1" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C2B" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL2" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
    '                    dt.Rows(i).Item("Pct") = IIf(lnFoundCnt = lncnt, Round(lnVal2, 8), Round(lnVal1, 8))
    '                End If

    '                If lcBeneficiaryCategory = "R" _
    '                And lcGroup = "C3B" _
    '                And dt.Rows(i).Item("BeneficiaryTypeCode").ToString.Trim.ToUpper = "INSRES" _
    '                And dt.Rows(i).Item("Groups").ToString.Trim.ToUpper = "CONT" _
    '                And IIf(IsDBNull(dt.Rows(i).Item("Lvl")), "", dt.Rows(i).Item("Lvl").ToString.Trim.ToUpper) = "LVL3" Then
    '                    lnFoundCnt = lnFoundCnt + 1
    '                    'Aparna    YREN-2849-Change precision to 8 decimals
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
    '            viewstate("DS_Sort_ActiveBeneficiaries") = ds
    '            Me.DataGridActiveBeneficiaries.DataBind()
    '            Me.CalculateValues(ds.Tables(0), "A")
    '        Else
    '            'Vipul 01Feb06 Cache-Session
    '            'Cache.Add("BeneficiariesRetired", ds)
    '            Session("BeneficiariesRetired") = ds
    '            'Vipul 01Feb06 Cache-Session

    '            Me.DataGridRetiredBeneficiaries.DataSource = ds
    '            Viewstate("DS_Sort_RetBen") = ds
    '            Me.DataGridRetiredBeneficiaries.DataBind()
    '            Me.CalculateValues(ds.Tables(0), "R")
    '        End If
    '    Catch sqlEx As SqlException

    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)


    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try

    'End Sub
    ' NP:PS:2007.06.18 - Rewriting the Equalize Function to make it simpler
    ' ds - The Dataset to work with     - DataSet Field ds.Tables(0)
    ' lcGroup - PRIM or CONT            - DataSet Field 'Groups'
    ' lcLevel - Level "", "1", "2", "3" - DataSet Field 'Lvl'
    ' lcBeneficiaryType - "MEMBER", "SAVING", "RETIRE", "INSRES", "INSSAV"  - DataSet Field 'BeneficiaryTypeCode'
    Private Sub Equalize(ByVal ds As DataSet, ByVal lcGroup As String, ByVal lcLevel As String, ByVal lcBeneficiaryType As String)
        Try
            Dim lncnt As Integer
            Dim dt As DataTable = ds.Tables(0)
            Dim rows As DataRow()
            Dim filterCondition As String = ""

            If lcGroup <> "" Then filterCondition += IIf(filterCondition <> "", " AND ", "") + " Groups = '" + lcGroup + "'"
            If lcLevel <> "" Then filterCondition += IIf(filterCondition <> "", " AND ", "") + " Lvl = 'LVL" + lcLevel + "'"
            If lcBeneficiaryType <> "" Then filterCondition += IIf(filterCondition <> "", " AND ", "") + " BeneficiaryTypeCode = '" + lcBeneficiaryType + "'"

            rows = dt.Select(filterCondition)

            lncnt = rows.Length
            'SP 2013.12.19 BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process (changed from Double Decimal)
            Dim lnVal1Portion As Decimal
            Dim lnValPortionTimesCountMinus1 As Decimal
            Dim lnVal1 As Decimal
            Dim lnVal2 As Decimal
            Dim lnDiffWith100 As Decimal
            If lncnt <> 0 Then

                'Aparna    YREN-2849-Change precision to 8 decimals
                'lnVal1Portion = Convert.ToInt32((100.0 / lncnt) * 10000) / 10000
                'lnValPortionTimesCountMinus1 = Convert.ToInt32(lnVal1Portion * (lncnt - 1) * 100000) / 100000
                'SP 2013.12.19 BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process (changed ToDouble ToDecimal and round Function)
                lnVal1Portion = Math.Round(Convert.ToDecimal(((100.0 / lncnt) * 10000) / 10000), 5)
                lnValPortionTimesCountMinus1 = Convert.ToDecimal((lnVal1Portion * (lncnt - 1) * 100000) / 100000)
                'Aparna    YREN-2849-Change precision to 8 decimals
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

            If lcBeneficiaryType = "MEMBER" Or lcBeneficiaryType = "SAVING" Then
                Session("BeneficiariesActive") = ds
                Me.DataGridActiveBeneficiaries.DataSource = ds
                ViewState("DS_Sort_ActiveBeneficiaries") = ds
                Me.DataGridActiveBeneficiaries.DataBind()
                Me.CalculateValues(ds.Tables(0), "A")
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
    Private Sub CalculateValues(ByVal dt As DataTable, ByVal benecategory As String)
        ' This function calculates the Percentages for each beneficiary type and then populates the appropriate textboxes
        ' with the correct values. It also displays an error message if the percentage values do not add up to 100 for 
        ' any particular beneficiary type.
        Try
            Dim lnPrim, lnCont1, lnCont2, lnCont3 As Decimal 'SP 2013.12.20  BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process  (changes from double to decimal)
            Dim lnPrimb, lnCont1b, lnCont2b, lnCont3b As Decimal
            Dim lnPrimc, lnCont1c, lnCont2c, lnCont3c As Decimal
            Dim i As Integer
            Dim lcPrimMsg, lcCont1Msg, lcCont2Msg, lcCont3Msg As String
            Dim lcPrimBMsg, lcCont1BMsg, lcCont2BMsg, lcCont3BMsg As String
            Dim lcPrimCMsg, lcCont1CMsg, lcCont2CMsg, lcCont3CMsg As String

            For i = 0 To dt.Rows.Count - 1
                'Calculate totals for Active Participants' Beneficiaries at Primary and Cotingent Levels 1, 2 and 3
                If benecategory = "A" And dt.Rows(i).RowState <> DataRowState.Deleted Then
                    'Member benefit totals
                    If dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "MEMBER" Then
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
                    End If  'End MEMBER benefit calculations
                    'Savings benefit totals
                    If dt.Rows(i).Item("BeneficiaryTypeCode").ToString.ToUpper.Trim = "SAVING" Then
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
                    End If  'End SAVING benefit calculations
                End If  'End If benecategory = "A" 
            Next

            'SP 2013.12.20  BT-2131/YRS 5.0-2161 : Modifications to Annuity Purchase process  (Remove round function)
            If benecategory = "A" Then  'Set percentage values (main)
                If lnPrim = 0 Then
                    TextBoxPrimaryA.Text = "" : ButtonPriA.Enabled = False
                Else
                    'lnPrim = Round(lnPrim, 2) : TextBoxPrimaryA.Text = lnPrim
                    lnPrim = lnPrim : TextBoxPrimaryA.Text = lnPrim
                    ButtonPriA.Enabled = True
                End If
                If lnCont1 = 0 Then
                    TextBoxCont1A.Text = "" : ButtonCont1A.Enabled = False
                Else
                    'lnCont1 = Round(lnCont1, 2) : TextBoxCont1A.Text = lnCont1
                    lnCont1 = lnCont1 : TextBoxCont1A.Text = lnCont1
                    ButtonCont1A.Enabled = True
                End If
                If lnCont2 = 0 Then
                    TextBoxCont2A.Text = "" : ButtonCont2A.Enabled = False
                Else
                    'lnCont2 = Round(lnCont2, 2) : TextBoxCont2A.Text = lnCont2
                    lnCont2 = lnCont2 : TextBoxCont2A.Text = lnCont2
                    ButtonCont2A.Enabled = True
                End If
                If lnCont3 = 0 Then
                    TextBoxCont3A.Text = "" : ButtonCont3A.Enabled = False
                Else
                    'lnCont3 = Round(lnCont3, 2) : TextBoxCont3A.Text = lnCont3
                    lnCont3 = lnCont3 : TextBoxCont3A.Text = lnCont3
                    ButtonCont3A.Enabled = True
                End If

                'Set percentage values (retired ins res)
                If lnPrimb = 0 Then
                    TextBoxPrimaryInsA.Text = "" : ButtonPriInsA.Enabled = False
                Else
                    'lnPrimb = Round(lnPrimb, 2) : TextBoxPrimaryInsA.Text = lnPrimb
                    lnPrimb = lnPrimb : TextBoxPrimaryInsA.Text = lnPrimb
                    ButtonPriInsA.Enabled = True
                End If
                If lnCont1b = 0 Then
                    TextBoxCont1InsA.Text = "" : ButtonCont1InsA.Enabled = False
                Else
                    'lnCont1b = Round(lnCont1b, 2) : TextBoxCont1InsA.Text = lnCont1b
                    lnCont1b = lnCont1b : TextBoxCont1InsA.Text = lnCont1b
                    ButtonCont1InsA.Enabled = True
                End If
                If lnCont2b = 0 Then
                    TextBoxCont2InsA.Text = "" : ButtonCont2InsA.Enabled = False
                Else
                    'lnCont2b = Round(lnCont2b, 2) : TextBoxCont2InsA.Text = lnCont2b
                    lnCont2b = lnCont2b : TextBoxCont2InsA.Text = lnCont2b
                    ButtonCont2InsA.Enabled = True
                End If
                If lnCont3b = 0 Then
                    TextBoxCont3InsA.Text = "" : ButtonCont3InsA.Enabled = False
                Else
                    'lnCont3b = Round(lnCont3b, 2) : TextBoxCont3InsA.Text = lnCont3b
                    lnCont3b = lnCont3b : TextBoxCont3InsA.Text = lnCont3b
                    ButtonCont3InsA.Enabled = True
                End If
            End If

            'TODO: Add for labels for the new Saving type of beneficiary
            lcPrimMsg = "Primary Member beneficiary must equal 100% percent."
            lcCont1Msg = "Contingent level 1 member beneficiary must equal 100% percent."
            lcCont2Msg = "Contingent level 2 member beneficiary must equal 100% percent."
            lcCont3Msg = "Contingent level 3 member beneficiary must equal 100% percent."

            lcPrimBMsg = "Primary Saving beneficiary must equal 100% percent."
            lcCont1BMsg = "Contingent Level 1 Saving beneficiary must equal 100% percent."
            lcCont2BMsg = "Contingent Level 2 Saving beneficiary must equal 100% percent."
            lcCont3BMsg = "Contingent Level 3 Saving beneficiary must equal 100% percent."

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
        Catch
            Throw
        End Try
    End Sub

    Private Sub DataGridParticipantNotes_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridParticipantNotes.ItemDataBound
        'Below lines are commented By Dilip Yadav : To modify the existing code
        'Dim l_datatable_Notes As DataTable
        'If Not Session("DisplayNotes") Is Nothing Then
        '    l_datatable_Notes = Session("DisplayNotes")
        'End If
        'commented by aparna -YREN-3115 9/03/2007
        ' e.Item.Cells(1).Visible = False
        ' e.Item.Cells(2).Visible = False
        Try
            'Below lines are commented & Added By Dilip Yadav : To modify the exising code
            'If e.Item.ItemIndex <> -1 Then
            '    If l_datatable_Notes.Rows(e.Item.ItemIndex).IsNull("Note") = False Then
            '        If l_datatable_Notes.Rows(e.Item.ItemIndex)("Note").ToString.Length > 50 Then
            '            e.Item.Cells(3).Text = l_datatable_Notes.Rows(e.Item.ItemIndex)("Note").ToString.Substring(0, 50)
            '        Else
            '            e.Item.Cells(3).Text = l_datatable_Notes.Rows(e.Item.ItemIndex)("Note")
            '        End If
            '    End If
            '           End If
            If e.Item.Cells(3).Text.Length > 100 Then
                e.Item.Cells(3).Text = e.Item.Cells(3).Text.Substring(0, 100)
            End If
            'By Aparna -yren- 3115 09/03/2007
            Dim l_checkbox As New CheckBox
            Dim l_linkbutton As New LinkButton 'Bala: 01/12/2016: YRS-AT-1718: Create link button control for disabling / enabling accordingly
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
                l_checkbox = e.Item.FindControl("CheckBoxImportant")
                If l_checkbox.Checked Then
                    Me.NotesFlag.Value = "MarkedImportant"
                End If
                If e.Item.Cells(2).Text.Trim = Session("LoginId") Or Me.NotesGroupUser = True Then
                    l_checkbox.Enabled = True
                End If
                'Start: Bala: 01/12/2016: YRS-AT-1718: Create link button control for disabling / enabling accordingly
                l_linkbutton = e.Item.FindControl("DeleteNotes")
                If Not e.Item.Cells(2).Text.Trim.ToLower() = Convert.ToString(Session("LoginId")).ToLower() Then 'Manthan Rajguru | 2016.04.05 | YRS-AT-1718 | Converting login ID to lower case from both the side to avoid any mismatches
                    l_linkbutton.Enabled = False
                End If
                'End: Bala: 01/12/2016: YRS-AT-1718: Create link button control for disabling / enabling accordingly

                'by Aparna -YREN-3115 16/03/2007
                If Me.NotesFlag.Value = "Notes" Then
                    Me.TabStripParticipantsInformation.Items(5).Text = "<font color=orange>Notes</font>"
                ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
                    Me.TabStripParticipantsInformation.Items(5).Text = "<font color=red>Notes</font>"
                Else
                    Me.TabStripParticipantsInformation.Items(5).Text = "Notes"
                End If
            End If
            CheckReadOnlyMode() 'Shilpa N | 03/18/2019 | YRS-AT-4248 | Calling the method 
            'By Aparna -yren- 3115 09/03/2007
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

#Region "Add/Edit/Delete Events for Beneficiaries"
    Private Sub ButtonAddActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddActive.Click
        Dim l_string_FundStatus As String

        Dim strWSMessage As String
        Try
            'Start:AA:2014.09.03 BT-1409 : Miscellaneous Issues for Security Rights
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAddActive", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End:AA:2014.09.03 BT-1409 : Miscellaneous Issues for Security Rights
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()



            'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
            strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
            If strWSMessage <> "NoPending" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "Invalid Beneficiary Operation", "openDialog('" + strWSMessage + "','Bene');", True)
                strWSMessage = (strWSMessage.Replace("<br/>", "\n")).Replace("<br>", "\n")
                Me.TabStripParticipantsInformation.Items(4).Text = "<asp:Label ID='lblBeneficiaries' CssClass='Label_Small'onmouseover='javascript: showToolTip(""" + strWSMessage + """,""Bene"");' onmouseout='javascript: hideToolTip();'><font color=red>Beneficiaries</font></asp:Label>"

                DropDownListMaritalStatus.Enabled = False

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
                DropDownListMaritalStatus.Enabled = True
                Me.TabStripParticipantsInformation.Items(4).Text = "Beneficiaries"
            End If
            'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            _icounter = Session("icounter")
            _icounter = _icounter + 1
            Session("icounter") = _icounter
            If _icounter = 1 Then

                Session("MaritalStatus") = DropDownListMaritalStatus.SelectedValue 'YRPS-4704

                Session("Flag") = "AddBeneficiaries"
                'start - Manthan Rajguru | 2016.07.22 | Changed the window width size - 750
                Dim msg1 As String = "<script language='javascript'>" & _
                "window.open('UpdateBeneficiaries.aspx','CustomPopUp', " & _
                "'width=900, height=700, menubar=no, status=yes, Resizable=no,top=120,left=120, scrollbars=yes')" & _
                 "</script>" 'MMR | 2017.12.04 | YRS-AT-3756 | Chnaged the window width and height to 900 and 700 from 850 and 650                
                'End - Manthan Rajguru |2016.07.22 | Changed the window width size - 750

                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", msg1)
                End If
                'SP 2014.02.24 Bt-2316\YRS 5.0-2262 - Spousal Consent date in YRS -Commented
                'AA:13.02.2014:BT:2316:YRS 5.0 2262 - Added below code to check whether cannotlocate spouse or spousal wavier date exists
                'If (TextBoxSpousealWiver.Text = "" And chkCannotLocateSpouse.Checked) Or _
                '    (TextBoxSpousealWiver.Text <> "" And Not chkCannotLocateSpouse.Checked) Then
                '    Session("Pers_spousalWavier_CannotLocateSpouse") = True
                'ElseIf (TextBoxSpousealWiver.Text = "" And Not chkCannotLocateSpouse.Checked) Or _
                '    (TextBoxSpousealWiver.Text <> "" And chkCannotLocateSpouse.Checked) Then
                '    Session("Pers_spousalWavier_CannotLocateSpouse") = False
                'End If
            Else
                'Vipul 01Feb06 Cache-Session
                'Me.DataGridActiveBeneficiaries.DataSource = CType(Cache("BeneficiariesActive"), DataSet)
                'viewstate("DS_Sort_ActiveBeneficiaries") = CType(Cache("BeneficiariesActive"), DataSet)
                Me.DataGridActiveBeneficiaries.DataSource = DirectCast(Session("BeneficiariesActive"), DataSet)
                ViewState("DS_Sort_ActiveBeneficiaries") = DirectCast(Session("BeneficiariesActive"), DataSet)
                'Vipul 01Feb06 Cache-Session

                Me.DataGridActiveBeneficiaries.DataBind()
                _icounter = 0
                Session("icounter") = _icounter
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

    ' Function Created by Dinesh Kanojia 12/13/2012.
    Private Sub EditActiveBeneficiary(ByVal DatagridRow As TableCellCollection, ByVal iIndex As Integer)
        Try
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            ''Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            'Start:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditActive", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
            ''End : YRS 5.0-940

            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Vipul 01Feb06 Cache-Session

            'If Me.DataGridActiveBeneficiaries.SelectedIndex <> -1 Then
            'Session("Flag") = "EditBeneficiaries"
            'Session("Name") = IIf(IsDBNull(DatagridRow(5).Text), "", DatagridRow(5).Text.Trim)
            'Session("Name2") = IIf(IsDBNull(DatagridRow(6).Text), "", DatagridRow(6).Text.Trim)
            'Session("TaxID") = IIf(IsDBNull(DatagridRow(7).Text), "", DatagridRow(7).Text.Trim)
            'Session("Rel") = IIf(IsDBNull(DatagridRow(8).Text), "", DatagridRow(8).Text.Trim)
            'Session("Birthdate") = IIf(IsDBNull(DatagridRow(9).Text), "", DatagridRow(9).Text.Trim)
            'Session("Groups") = IIf(IsDBNull(DatagridRow(11).Text), "", DatagridRow(11).Text.Trim)
            'Session("Lvl") = IIf(IsDBNull(DatagridRow(12).Text), "", DatagridRow(12).Text.Trim)
            'Session("Pct") = IIf(IsDBNull(DatagridRow(15).Text), "", DatagridRow(15).Text.Trim)
            'Session("Type") = IIf(IsDBNull(DatagridRow(10).Text), "", DatagridRow(10).Text.Trim)



            Session("Flag") = "EditBeneficiaries"
            'Session("Name") = IIf(IsDBNull(DatagridRow(6).Text), "", DatagridRow(6).Text.Trim)
            'Session("Name2") = IIf(IsDBNull(DatagridRow(7).Text), "", DatagridRow(7).Text.Trim)
            'Session("TaxID") = IIf(IsDBNull(DatagridRow(8).Text), "", DatagridRow(8).Text.Trim)
            'Session("Rel") = IIf(IsDBNull(DatagridRow(9).Text), "", DatagridRow(9).Text.Trim)
            'Session("Birthdate") = IIf(IsDBNull(DatagridRow(10).Text), "", DatagridRow(10).Text.Trim)
            'Session("Groups") = IIf(IsDBNull(DatagridRow(12).Text), "", DatagridRow(12).Text.Trim)
            'Session("Lvl") = IIf(IsDBNull(DatagridRow(13).Text), "", DatagridRow(13).Text.Trim)
            'Session("Pct") = IIf(IsDBNull(DatagridRow(16).Text), "", DatagridRow(16).Text.Trim)
            'Session("Type") = IIf(IsDBNull(DatagridRow(11).Text), "", DatagridRow(11).Text.Trim)


            ''_icounter = Session("icounter")
            ''_icounter = _icounter + 1
            ''Session("_icounter") = _icounter

            'NP - 2007.02.27 - Found &nbsp; in textboxes on the Add Beneficiary page when going in to edit an existing one
            ' The Bound Datagrid contains a space character by default if nothing else exists. If this is the case our Session variables are initialized with &nbsp; instead of string.empty.
            'If Session("Name") = "&nbsp;" Then Session("Name") = ""
            'If Session("Name2") = "&nbsp;" Then Session("Name2") = ""
            'If Session("TaxID") = "&nbsp;" Then Session("TaxID") = ""
            'If Session("Rel") = "&nbsp;" Then Session("Rel") = ""
            'If Session("Birthdate") = "&nbsp;" Then Session("Birthdate") = ""
            'If Session("Groups") = "&nbsp;" Then Session("Groups") = ""
            'If Session("Lvl") = "&nbsp;" Then Session("Lvl") = ""
            'If Session("Pct") = "&nbsp;" Then Session("Pct") = ""
            'If Session("Type") = "&nbsp;" Then Session("Type") = ""
            'END Changes by NP - 2007.02.27

            Session("MaritalStatus") = DropDownListMaritalStatus.SelectedValue 'YRPS-4704

            Dim popupScript As String
            ''If (_icounter = 1) Then
            'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            If (DatagridRow(2).Text = "&nbsp;" Or DatagridRow(2).Text = "") And (DatagridRow(20).Text <> "&nbsp;" Or DatagridRow(20).Text <> "") Then

                '   popupScript = "<script language='javascript'>" & _
                '"window.open('UpdateBeneficiaries.aspx?Index=" & Me.DataGridActiveBeneficiaries.SelectedIndex & "', 'CustomPopUp', " & _
                '"'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                '"</script>"

                'Start - Manthan Rajguru | 2016.07.22 | Changed the window width size - 750
                popupScript = "<script language='javascript'>" & _
          "window.open('UpdateBeneficiaries.aspx?NewID=" & DatagridRow(20).Text & "', 'CustomPopUp', " & _
          "'width=900, height=700, menubar=no, Resizable=no,top=120,left=120, status=yes,scrollbars=yes')" & _
          "</script>" 'MMR | 2017.12.04 | YRS-AT-3756 | Chnaged the window width and height to 900 and 700 from 850 and 650
                'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            ElseIf (DatagridRow(2).Text <> "&nbsp;" Or DatagridRow(2).Text <> "") And (DatagridRow(20).Text = "&nbsp;" Or DatagridRow(20).Text = "") Then

                popupScript = "<script language='javascript'>" & _
           "window.open('UpdateBeneficiaries.aspx?UniqueId=" + DatagridRow(2).Text + "','CustomPopUp', " & _
           "'width=900, height=700, menubar=no, status=yes, Resizable=no,top=120,left=120, scrollbars=yes')" & _
           "</script>" 'MMR | 2017.12.04 | YRS-AT-3756 | Chnaged the window width and height to 900 and 700 from 850 and 650
            End If
            'End - Manthan Rajguru | 2016.07.22 | Changed the window width size - 750
            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", popupScript)

            End If
            '''Else

            '''    _icounter = 0
            '''    Session("icounter") = _icounter
            '''End If
            'Else
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a Beneficiary record to be updated.", MessageBoxButtons.OK)
            'Exit Sub
            'End If
            'NP:PS:2007.08.07 - The enable button code is moved here so that they are enabled only when the person is in edit mode.
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()

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

    'Function Added by Dinesh Kanojia on 12/13/2012.
    Private Sub DeleteActiveBeneficairy(ByVal DatagridRow As TableCellCollection, ByVal iIndex As Integer)
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

            'If Me.DataGridActiveBeneficiaries.SelectedIndex <> -1 Then
            Dim Beneficiaries As DataSet

            'Vipul 01Feb06 Cache-Session
            'Dim l_CacheManager As CacheManager
            'l_CacheManager = CacheFactory.GetCacheManager()
            Dim drRows As DataRow()
            Dim drUpdated As DataRow

            If DatagridRow(2).Text <> "&nbsp;" Then 'If DatagridRow(1).Text <> "&nbsp;" Then

                'Vipul 01Feb06 Cache-Session
                'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)
                Beneficiaries = CType(Session("BeneficiariesActive"), DataSet)
                'Vipul 01Feb06 Cache-Session

                Dim l_UniqueId As String
                l_UniqueId = DatagridRow(2).Text ' DatagridRow(1).Text

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
                    'START: MMR | 2017.12.04 | YRS-AT-3756 | Added death reason code & Death date
                    drUpdated("DeathReason") = reasonCode
                    drUpdated("DeathDate") = deathDate
                    drUpdated.AcceptChanges()
                    'END: MMR | 2017.12.04 | YRS-AT-3756 | Added death reason code & Death date
                    drUpdated.Delete()

                    'Vipul 01Feb06 Cache-Session
                    'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)

                    Session("BeneficiariesActive") = Beneficiaries
                    'Vipul 01Feb06 Cache-Session

                End If
            End If

            If DatagridRow(2).Text = "&nbsp;" Then '  If DatagridRow(1).Text = "&nbsp;" Then

                'Vipul 01Feb06 Cache-Session
                'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)
                Beneficiaries = DirectCast(Session("BeneficiariesActive"), DataSet)
                'Vipul 01Feb06 Cache-Session

                If Not IsNothing(Beneficiaries) Then

                    ''drUpdated = Beneficiaries.Tables(0).Rows(Me.DataGridActiveBeneficiaries.SelectedIndex)
                    drUpdated = Beneficiaries.Tables(0).Rows(iIndex)
                    drUpdated.Delete()
                    'Vipul 01Feb06 Cache-Session
                    'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)

                    Session("BeneficiariesActive") = Beneficiaries
                    'Vipul 01Feb06 Cache-Session

                End If
            End If
            LoadBeneficiariesTab()
            Session("Flag") = ""
            'Else
            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a record to be deleted.", MessageBoxButtons.Stop)
            'Exit Sub
            'End If
            'Me.DataGridActiveBeneficiaries.SelectedIndex -= 1 'aparna 30/11/2007
            'NP:PS:2007.08.07 - The enable button code is moved here so that they are enabled only when the person is in edit mode.
            ButtonOK.Enabled = False
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True

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

#End Region

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try
            'Vipul 01Feb06 Cache-Session
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Flush()
            Session("Participant_Sort") = Nothing
            Session("Page") = "Person"
            Session("WithDrawn_member") = Nothing
            Session("Person_Info") = Nothing
            Session("ParticipantDNMsgforDR") = Nothing

            Session("TelephoneDetails") = Nothing
            Session("dt_PrimaryContactInfo") = Nothing
            Session("dt_SecondaryContactInfo") = Nothing

            'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
            'ClearSession()  'SB | 10/18/2017 | YRS-AT-3324 | Clearing Session variable used to check RMD generated or not
            'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 

            Response.Redirect("FindInfo.aspx?Name=Person", False)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Dim l_int_IsAddressPresent As Integer
        ValidationSummaryParticipants.Enabled = False 'Added By Shashi Shekhar:2009-12-23
        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
        HiddenFieldDirty.Value = "false"
        Try
            ButtonSaveParticipant.Enabled = False
            ButtonCancel.Enabled = False
            ButtonOK.Enabled = True
            ButtonEdit.Enabled = True
            ButtonEditAddress.Enabled = True
            ButtonEditDOB.Enabled = True
            ButtonEditSSno.Enabled = True
            tbPricontacts.Visible = False
            tbSeccontacts.Visible = False
            'btnEditPrimaryContact.Visible = False
            'btnEditSecondaryContact.Visible = False
            TelephoneWebUserControl1.Visible = True
            TelephoneWebUserControl2.Visible = True
            'btnAcctLockEdit.Enabled = True
            Session("Flag") = ""
            'Added by Anudeep:22.03.2013-For Resetting active as Primary 
            Session("bln_Activateprimary") = Nothing
            LoadGeneraltab()

            LoadEmploymentTab()
            LoadAccountContributionsTab(sender, e)
            LoadAdditionalAccountsTab()
            'Commented by Dinesh Kanojia
            'DataGridActiveBeneficiaries.SelectedIndex = -1  'NP:PS:2007.08.07 - Deselecting the beneficiary when Cancel button is hit.
            LoadBeneficiariesTab()
            'AA:2013.10.23 - Bt-2264: below code and has been placed for getting the notes from database
            Session("dtNotes") = Nothing
            LoadNotesTab()
            Me.MakeLinkVisible()


            Session("dtReason") = Nothing

            'Ashutosh Patil as on 26-Mar-2007
            'YREN-3028,YREN-3029
            'TextBoxAddress1.ReadOnly = True
            'TextBoxAddress2.ReadOnly = True
            'TextBoxAddress3.ReadOnly = True
            'TextBoxCity.ReadOnly = True
            'DropdownlistState.Enabled = False
            'TextBoxZip.ReadOnly = True
            'DropdownlistCountry.Enabled = False
            'Ashutosh Patil as on 26-Mar-2007
            'YREN-3028,YREN-3029
            'Me.AddressWebUserControl1.MakeReadonly = True
            Me.AddressWebUserControl1.EnableControls = False
            'Added by Anudeep:12.03.2013-For Telephone Usercontrol
            'Me.TelephoneWebUserControl1.EditPrimaryContact_Enabled = False

            'Commneted By Dinesh Kanojia Contacts
            'TextBoxTelephone.ReadOnly = True
            'TextBoxFax.ReadOnly = True
            'TextBoxHome.ReadOnly = True
            'TextBoxMobile.ReadOnly = True
            'TextBoxSecTelephone.ReadOnly = True
            'TextBoxSecFax.ReadOnly = True
            'TextBoxSecHome.ReadOnly = True
            'TextBoxSecMobile.ReadOnly = True
            TextBoxEmailId.ReadOnly = True
            'BS:2012.03.09:-restructure Address Control
            'Me.TextBoxEffDate.Enabled = False
            'Me.TextBoxSecEffDate.Enabled = False


            'Ashutosh Patil as on 26-Mar-2007
            'YREN-3028,YREN-3029
            Me.AddressWebUserControl2.EnableControls = False
            ' Me.TelephoneWebUserControl2.EditSecondaryContact_Enabled = False
            'Me.TextBoxSecAddress1.Enabled = False
            'Me.TextBoxSecAddress2.Enabled = False
            'Me.TextBoxSecAddress3.Enabled = False
            'Me.TextBoxSecCity.Enabled = False
            'Me.DropdownlistSecCountry.Enabled = False
            '''Me.TextBoxSecEmail.Enabled = False
            'Me.DropdownlistSecState.Enabled = False
            'Me.TextBoxSecZip.Enabled = False

            'Me.TextBoxSecTelephone.Enabled = False
            'TextBoxSecHome.Enabled = False
            'TextBoxSecFax.Enabled = False
            'TextBoxSecMobile.Enabled = False


            Me.ButtonEditAddress.Enabled = True

            Me.TextBoxDOB.Enabled = False

            Me.TextBoxSuffix.Enabled = False
            Me.TextBoxMiddle.Enabled = False
            Me.TextBoxLast.Enabled = False
            Me.TextBoxFirst.Enabled = False
            Me.DropDownListSal.Enabled = False
            'Me.DropDownListMaritalStatus.Enabled = False
            'Me.DropDownListGender.Enabled = False
            Me.TextBoxDeceasedDate.Enabled = False
            Me.RadioButtonList1.Items(0).Selected = True
            Me.TextBoxDOB.Enabled = False
            Me.TextBoxFundNo.Enabled = False
            Me.TextBoxParticipationDate.Enabled = False
            Me.TextBoxPOA.Enabled = False
            Me.TextBoxQDRODraftDate.Enabled = False
            Me.TextBoxQDROStatusDate.Enabled = False
            Me.TextBoxQDROType.Enabled = False
            Me.CheckBoxQDROPending.Enabled = False
            Me.TextBoxSSNo.Enabled = False

            ' Me.ButtonActivateAsPrimary.Enabled = False
            'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from participant and put on address control
            'Me.CheckboxIsBadAddress.Enabled = False
            'Me.CheckboxSecIsBadAddress.Enabled = False
            Me.CheckboxBadEmail.Enabled = False
            ' Me.CheckboxSecBadEmail.Enabled = False
            ' Me.CheckboxSecTextOnly.Enabled = False
            ' Me.CheckboxSecUnsubscribe.Enabled = False
            Me.CheckboxTextOnly.Enabled = False
            Me.CheckboxUnsubscribe.Enabled = False
            'BS:2012.03.09:-restructure Address Control
            'Me.TextBoxSecEffDate.Enabled = False
            'Me.TextBoxEffDate.Enabled = False
            Me.ButtonSaveParticipant.Visible = True
            Me.ButtonSaveParticipants.Visible = False
            Me.ButtonSaveParticipants.Enabled = False
            Me.MakeLinkVisible()
            'Dim Cache As CacheManager
            'Cache = CacheFactory.GetCacheManager()
            'Cache.Flush()
            'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
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

            EditAddressAndContacts()


            If Not TextBoxDOB.Text = String.Empty Then
                TextBoxDOB.Text = Session("DOB")
                labelBoxAge.Text = CalculateAge(TextBoxDOB.Text, TextBoxDeceasedDate.Text).ToString("00.00")
                If (Me.labelBoxAge.Text.IndexOf(".00") > -1) Then
                    Me.labelBoxAge.Text = Me.labelBoxAge.Text.Replace(".00", "Y")
                Else
                    Me.labelBoxAge.Text = Me.labelBoxAge.Text.Replace(".", "Y/") + "M"
                End If
                Me.labelBoxAge.Text = Me.labelBoxAge.Text.Replace("/00M", "").Trim
            Else
                labelBoxAge.Text = String.Empty
            End If
            'SP 2014.12.04 BT-2310\YRS 5.0-2255
            IsModifiedPercentage = False
            OriginalBeneficiaries = Nothing
            Me.IsExhaustedDBRMDSettleOptionSelected = False 'MMR | 2016.10.14 | YRS-AT-3063 | Resetting property value
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

    Private Sub ButtonEdit_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles ButtonEdit.Command

    End Sub

    Private Sub Popcalendar2_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopCalendar2.SelectionChanged
        TextBoxDate.Text = PopCalendar2.SelectedDate
        LoadAccountContributionsTab(sender, e)
    End Sub

    Private Sub DropDownListGender_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListGender.SelectedIndexChanged
        Try
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub DropDownListMaritalStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListMaritalStatus.SelectedIndexChanged
        Try
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    ''Private Sub PopcalendarDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopcalendarDate.SelectionChanged
    ''    Try
    ''        System.DateTime.Today.AddYears(-21)
    ''        If Date.Compare(PopcalendarDate.SelectedDate, System.DateTime.Today.AddYears(-21)) = 1 Then
    ''            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Active participant must be atleast 21 years of age.", MessageBoxButtons.Stop)
    ''        Else
    ''            TextBoxDOB.Text = PopcalendarDate.SelectedDate
    ''        End If
    ''    Catch ex As Exception
    ''        Dim l_String_Exception_Message As String
    ''        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    ''        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    ''    End Try
    ''End Sub

    Private Sub DataGridActiveBeneficiaries_ItemCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles DataGridActiveBeneficiaries.ItemCommand
        Dim popupScript As String = String.Empty
        Dim strWSMessage As String
        Try
            If e.CommandName.ToLower = "edit" Then
                Dim l_Eidt_button As ImageButton

                l_Eidt_button = e.Item.FindControl("ImageEditBenefeciarybutton")
                If Not l_Eidt_button Is Nothing Then

                    'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
                    Dim checkSecurity As String = SecurityCheck.Check_Authorization(l_Eidt_button.ID, Convert.ToInt32(Session("LoggedUserKey")))

                    If Not checkSecurity.Equals("True") Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    'End : YRS 5.0-940
                    EditActiveBeneficiary(e.Item.Cells, e.Item.ItemIndex)
                    ''SP 2014.02.24 Bt-2316\YRS 5.0-2262 - Spousal Consent date in YRS -Commented
                    'AA:13.02.2014:BT:2316:YRS 5.0 2262 - Added below code to check whether cannotlocate spouse or spousal wavier date exists
                    'If (TextBoxSpousealWiver.Text = "" And chkCannotLocateSpouse.Checked) Or _
                    '(TextBoxSpousealWiver.Text <> "" And Not chkCannotLocateSpouse.Checked) Then
                    '    Session("Pers_spousalWavier_CannotLocateSpouse") = True
                    'ElseIf (TextBoxSpousealWiver.Text = "" And Not chkCannotLocateSpouse.Checked) Or _
                    '    (TextBoxSpousealWiver.Text <> "" And chkCannotLocateSpouse.Checked) Then
                    '    Session("Pers_spousalWavier_CannotLocateSpouse") = False
                    'End If

                End If
                'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
                'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
                strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("PersId"))
                'AA:2013.08.05 - YRS 5.0-2070 : Change to check if not pending records
                If strWSMessage <> "NoPending" Then
                    strWSMessage = (strWSMessage.Replace("<br/>", "\n")).Replace("<br>", "\n")
                    Me.TabStripParticipantsInformation.Items(4).Text = "<asp:Label ID='lblBeneficiaries' CssClass='Label_Small'onmouseover='javascript: showToolTip(""" + strWSMessage + """,""Bene"");' onmouseout='javascript: hideToolTip();'><font color=red>Beneficiaries</font></asp:Label>"

                    DropDownListMaritalStatus.Enabled = False

                    imgLock.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Pers');")
                    imgLock.Attributes.Add("onmouseout", "javascript: hideToolTip();")
                    imgLock.Visible = True

                    imgLockBeneficiary.Visible = True
                    imgLockBeneficiary.Attributes.Add("onmouseover", "javascript: showToolTip('" + strWSMessage + "','Bene');")
                    imgLockBeneficiary.Attributes.Add("onmouseout", "javascript: hideToolTip();")

                Else
                    imgLock.Visible = False
                    imgLockBeneficiary.Visible = False
                    DropDownListMaritalStatus.Enabled = True
                    Me.TabStripParticipantsInformation.Items(4).Text = "Beneficiaries"
                End If
                'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            End If

            If e.CommandName.ToLower = "delete" Then
                'Start:AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonDeleteActive", Convert.ToInt32(Session("LoggedUserKey")))

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
                    Me.TabStripParticipantsInformation.Items(4).Text = "<asp:Label ID='lblBeneficiaries' CssClass='Label_Small'onmouseover='javascript: showToolTip(""" + strWSMessage + """,""Bene"");' onmouseout='javascript: hideToolTip();'><font color=red>Beneficiaries</font></asp:Label>"

                    DropDownListMaritalStatus.Enabled = False

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
                    DropDownListMaritalStatus.Enabled = True
                    Me.TabStripParticipantsInformation.Items(4).Text = "Beneficiaries"
                End If
                'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
                'Start -> DineshK:2013.02.12:BT-1261/YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
                Dim l_Delete_button As ImageButton
                l_Delete_button = e.Item.FindControl("ImageDeleteBenefeciarybutton")
                Session("Flag") = "DeleteBeneficiaries"
                If Not l_Delete_button Is Nothing Then
                    'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
                    checkSecurity = SecurityCheck.Check_Authorization(l_Delete_button.ID, Convert.ToInt32(Session("LoggedUserKey")))
                    If Not checkSecurity.Equals("True") Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    'End : YRS 5.0-940

                    'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - Start
                    'Check beneficiary settle status 
                    If e.Item.Cells(15).Text <> "&nbsp;" Then
                        If (e.Item.Cells(15).Text.Trim.Equals("SETTLD", StringComparison.CurrentCultureIgnoreCase)) Then
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DELETE_SETTLED_BENEFICIARY, MessageBoxButtons.Stop)
                            Exit Sub
                        End If
                    End If
                    'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - End

                    If e.Item.Cells(2).Text.Trim <> "&nbsp;" Then
                        popupScript = " <script language='javascript'>" & _
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
                End If
                'END -> DineshK:2013.02.12:BT-1261/YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
            End If

        Catch ex As Exception

        End Try
    End Sub



    Private Sub DataGridParticipantEmployment_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridParticipantEmployment.ItemCommand
        Try
            'Shashi Shekhar:18 Feb 2011: ForBT-670,YRS 5.0-1202 : hyperlink from employment record to ymca maintenance
            If e.CommandName.ToLower = "ymcano" Then
                Dim l_YMCANo As String = e.CommandArgument.ToString().Trim
                Dim l_urlLanding As String = System.Configuration.ConfigurationSettings.AppSettings("YRSLandingPage") + "?PageType=ymcamaintenance&DataType=ymcano&Value=" + l_YMCANo
                Response.Redirect(l_urlLanding, False)
            End If

            If e.CommandName.ToLower = "edit" Then

                Dim l_button_Edit As ImageButton
                l_button_Edit = e.Item.FindControl("ImageEditButtonEmployment")
                If Not l_button_Edit Is Nothing Then
                    'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
                    'AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
                    Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonUpdateEmployment", Convert.ToInt32(Session("LoggedUserKey")))
                    'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
                    HiddenFieldDirty.Value = "true"

                    If Not checkSecurity.Equals("True") Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    'End : YRS 5.0-940
                End If
                EditParticipantEmployement(e.Item.Cells, e.Item.ItemIndex)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub DataGridParticipantNotes_ItemCommand(ByVal sender As Object, ByVal E As DataGridCommandEventArgs) Handles DataGridParticipantNotes.ItemCommand
        Try

            If E.CommandName.ToLower = "view" Then
                ViewParticipantNotes(E.Item.Cells, E.Item.ItemIndex)
                'Start: Bala: 01/12/2016: YRS-AT-1718: Delete Notes
            ElseIf E.CommandName.ToLower = "delete" Then
                DeleteParticipantNotes(E.Item.Cells)
                'End: Bala: 01/12/2016: YRS-AT-1718: Delete Notes
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'Start: Bala: 01/12/2016: YRS-AT-1718: Display confirmation message for delete note action
    ''' <summary>
    ''' Showing message box for delete confirmation
    ''' </summary>
    ''' <param name="dgRow"></param>
    ''' <remarks></remarks>
    Public Sub DeleteParticipantNotes(ByVal dgRow As TableCellCollection)
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

    Private Sub DataGridParticipantNotes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridParticipantNotes.SelectedIndexChanged
        Try
            Dim i As Integer
            For i = 0 To Me.DataGridParticipantNotes.Items.Count - 1
                Dim l_button_Select As ImageButton
                l_button_Select = DataGridParticipantNotes.Items(i).FindControl("ImageButtonNotes")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridParticipantNotes.SelectedIndex Then
                        'BY Aparna YREN-3115 16/03/2007
                        Me.ButtonView.Enabled = True
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If

            Next
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    Private Sub ButtonOK_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles ButtonOK.Command

    End Sub

    Private Sub ButtonPHR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPHR.Click
        Dim popupScript As String

        popupScript = "<script language='javascript'>" & _
                                              "window.open('RetireesFrmMoverWebForm.aspx', 'CustomPopUp', " & _
                                              "'width=800, height=500, menubar=no, Resizable=No,top=80,left=120, scrollbars=yes')" & _
                                              "</script>"




        Page.RegisterStartupScript("PopupScript2", popupScript)
    End Sub
    Public Sub RefreshQdroInfo()
        Try
            g_dataset_GeneralInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpGeneralInfo(Session("PersId"))

            'Added by Paramesh K. on sept 25th 2008
            '**************************************
            'As we need to display only the Non-Retired QDRO information 

            'Looping through the QDRO data and finding the Non-Retired QDRO request
            'and populating info to the appropriate controls
            For intRow As Integer = 0 To g_dataset_GeneralInfo.Tables("GeneralInfo").Rows.Count - 1
                'Priority Handling : Added by Dilip yadav : 29-Oct-09 : YRS 5.0.921
                'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("priority") = True) Then
                '    Me.checkboxPriority.Checked = True
                'Else
                '    Me.checkboxPriority.Checked = False
                'End If
                'Non-Retired
                If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroType").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroType").ToString() <> "") Then

                    If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroType").ToString().ToUpper() = "NON-RETIRED" Then

                        'QDRO Type
                        Me.TextBoxQDROType.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroType")

                        If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString() <> "") Then
                            'QDRO Pending checkbox
                            If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString.ToUpper() = "PENDING" Then
                                Me.CheckBoxQDROPending.Checked = True
                            Else
                                Me.CheckBoxQDROPending.Checked = False
                            End If
                            'QDRO Status
                            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus").ToString() <> "") Then
                                Me.TextBoxQDROStatusDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroStatus")
                            End If
                            'QDRO Draft Date
                            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroDraftDate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroDraftDate").ToString() <> "") Then
                                Me.TextBoxQDRODraftDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(intRow).Item("QdroDraftDate")
                            End If
                        End If
                        Exit For
                    End If
                End If
            Next

            'Commented by Paramesh K. on Sept 25th 2008
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType").ToString() <> "") Then
            '    Me.TextBoxQDROType.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroType")
            'End If
            'If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "") Then
            '    If g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString.ToUpper() = "PENDING" Then
            '        Me.CheckBoxQDROPending.Checked = True
            '    Else
            '        Me.CheckBoxQDROPending.Checked = False
            '    End If
            '    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus").ToString() <> "") Then
            '        Me.TextBoxQDROStatusDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroStatus")
            '    End If
            '    If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate").ToString() <> "") Then
            '        Me.TextBoxQDRODraftDate.Text = g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("QdroDraftDate")
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
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Public Sub updateAddressInfo(ByVal paramAddrId As String, ByVal paramEntityId As String, ByVal paramAddr1 As String, ByVal paramAddr2 As String, ByVal paramAddr3 As String, ByVal paramCity As String, ByVal paramStateType As String, ByVal paramCountry As String, ByVal paramZip As String, ByVal BitActive As Boolean, ByVal BitPrimary As Boolean, ByVal paramEffdate As String, ByVal paramCreator As String, ByVal paramUpdater As String)
        Try
            'YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateParticipantAddress(paramAddrId, paramEntityId, paramAddr1, paramAddr2, paramAddr3, paramCity, paramStateType, paramCountry, paramZip, BitActive, BitPrimary, paramEffdate, paramCreator, paramUpdater)

        Catch Ex As SqlException
            Throw Ex
        End Try
    End Sub
    Public Sub UpdateTelephoneInfo(ByVal paramTelephoneId As String, ByVal paramEntityId As String, ByVal paramTelephone As String, ByVal BitActive As Boolean, ByVal BitPrimary As Boolean, ByVal paramEffdate As String, ByVal paramCreator As String, ByVal paramUpdater As String)
        Try
            ''  YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(paramTelephoneId, paramEntityId, paramTelephone, BitActive, BitPrimary, paramEffdate, paramCreator, paramUpdater)
        Catch Ex As SqlException
            Throw Ex
        End Try
    End Sub
    Public Sub UpdateEmailInfo(ByVal paramMailId As String, ByVal paramEntityId As String, ByVal paramEmail As String, ByVal BitActive As Boolean, ByVal BitPrimary As Boolean, ByVal paramEffdate As String, ByVal paramCreator As String, ByVal paramUpdater As String)
        Try
            '' YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateEmailAddress(paramMailId, paramEntityId, paramEmail, BitActive, BitPrimary, paramEffdate, paramCreator, paramUpdater)
        Catch Ex As SqlException
            Throw Ex
        End Try
    End Sub
    Public Sub UpdateEmailProperties(ByVal paramUniqueId As String, ByVal paramEntityId As String, ByVal paramBadEmail As Boolean, ByVal paramUnsubscribed As Boolean, ByVal paramTextOnly As Boolean, ByVal BitActive As Boolean, ByVal BitPrimary As Boolean, ByVal paramSecondaryActive As Boolean, ByVal paramCreator As String, ByVal paramUpdater As String)
        Try
            ''  YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateEmailProperties(paramUniqueId, paramEntityId, paramBadEmail, paramUnsubscribed, paramTextOnly, BitActive, BitPrimary, paramSecondaryActive, paramCreator, paramUpdater)
        Catch Ex As SqlException
            Throw Ex
        End Try
    End Sub

    'NP:IVP2:2008.09.10 - Changing the way in which the format code has been written to make use of inbuilt functionality
    Public Function FormatCurrency(ByVal paramNumber As Decimal) As String
        Try
            Return paramNumber.ToString("#,###,##0.00")
            'Dim n As String
            'Dim m As String()
            'Dim myNum As String
            ''Changed by Ruchi on 7th March,2006
            'Dim myDec As String
            ''end of change
            'Dim len As Integer
            'Dim i As Integer
            'Dim val As String
            'If paramNumber = 0 Then
            '    val = 0
            'Else
            '    n = paramNumber.ToString()
            '    m = (Math.Round(n * 100) / 100).ToString().Split(".")
            '    myNum = m(0).ToString()

            '    len = myNum.Length
            '    Dim fmat(len) As String
            '    For i = 0 To len - 1
            '        fmat(i) = myNum.Chars(i)
            '    Next
            '    Array.Reverse(fmat)
            '    For i = 1 To len - 1
            '        If i Mod 3 = 0 Then
            '            fmat(i + 1) = fmat(i + 1) & ","
            '        End If
            '    Next
            '    Array.Reverse(fmat)
            '    'start of changeas


            '    'end of change
            '    If m.Length = 1 Then
            '        val = String.Join("", fmat) + ".00"
            '    Else
            '        myDec = m(1).ToString
            '        If myDec.Length = 1 Then
            '            myDec = myDec + "0"
            '        End If
            '        val = String.Join("", fmat) + "." + myDec
            '    End If

            'End If

            'Return Val

        Catch ex As Exception
            Return paramNumber
        End Try

    End Function
    'Private Sub DropdownlistCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistCountry.SelectedIndexChanged
    '    Try
    '        'Shubhrata YREN 2779

    '        TextBoxZip.Text = ""
    '        l_ds_States = DirectCast(Session("States"), DataSet)
    '        l_dt_State = l_ds_States.Tables(0)

    '        If Me.DropdownlistCountry.SelectedValue = "US" Then
    '            If Me.TextBoxZip.Text.GetType.FullName.ToString = "System.String" Then
    '                Me.TextBoxZip.Text = ""
    '            End If
    '        End If
    '        ' By Ashutosh Patil as on 25-Jan-2007 and on 29-Jan-2007
    '        ' Note : By Default the maxlength for All Countries is 10 before adding this function
    '        ' YREN - 3028,YREN - 3029
    '        ' Modified by Ashutosh Patil as on 12-Feb-2007
    '        ' The state and Country dropwdown will filter each other 
    '        'Call SetAttributes(TextBoxZip, DropdownlistCountry.SelectedValue.ToString)
    '        'If DropdownlistCountry.SelectedValue.ToString = "CA" Or DropdownlistCountry.SelectedValue.ToString = "US" Then
    '        l_dr_State = l_dt_State.Select("chvCountryCode='" & DropdownlistCountry.SelectedValue.ToString & "'")
    '        'Else
    '        '    l_dr_State = l_dt_State.Select("chvCountryCode not in('US','CA')")
    '        'End If
    '        l_dt_State_filtered = l_dt_State.Clone()
    '        For Each dr In l_dr_State
    '            l_dt_State_filtered.ImportRow(dr)
    '        Next
    '        Me.DropdownlistState.DataSource() = l_dt_State_filtered
    '        Me.DropdownlistState.DataBind()
    '        Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '        Me.DropdownlistState.Items(0).Value = ""

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        ExceptionPolicy.HandleException(ex, "Exception Policy")
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    '    'Shubhrata YREN 2779
    '    '*****************Code Commented by Ashutosh on 23-June-06 the funtionality has changed
    '    ''Try
    '    ''    Dim l_DataSet_StateNames As DataSet
    '    ''    If Me.DropdownlistCountry.SelectedItem.Text = "" Then
    '    ''        ' DropdownlistState.Items.Clear()

    '    ''        DropdownlistState.SelectedValue = ""
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

    '    ''    ''DropdownlistSecState.Items.Insert(0, "-Select State-")
    '    ''Catch ex As Exception
    '    ''    Throw
    '    ''End Try
    '    '******************
    'End Sub

    'Private Sub DropdownlistSecCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistSecCountry.SelectedIndexChanged
    '    Try
    '        'Shubhrata YREN 2779
    '        TextBoxSecZip.Text = ""
    '        l_ds_States = DirectCast(Session("States"), DataSet)
    '        l_dt_State = l_ds_States.Tables(0)

    '        If Me.DropdownlistSecCountry.SelectedValue = "US" Then
    '            If Me.TextBoxSecZip.Text.GetType.FullName.ToString = "System.String" Then
    '                Me.TextBoxSecZip.Text = ""
    '            End If
    '        End If
    '        ' By Ashutosh Patil as on 25-Jan-2007
    '        ' Note : By Default the maxlength for All Countries is 10 before adding this function
    '        ' YREN - 3028,YREN - 3029
    '        ' Modified by Ashutosh Patil as on 12-Feb-2007
    '        ' The state and Country dropwdown will filter each other 
    '        'Call SetAttributes(TextBoxSecZip, DropdownlistSecCountry.SelectedValue.ToString)
    '        'If DropdownlistSecCountry.SelectedValue.ToString = "CA" Or DropdownlistSecCountry.SelectedValue.ToString = "US" Then
    '        l_dr_State = l_dt_State.Select("chvCountryCode='" & DropdownlistSecCountry.SelectedValue.ToString & "'")
    '        'Commeneted By Ashutosh Patil YREN - 3028,YREN - 3029 as on 12-Feb-2007
    '        'Else
    '        '    l_dr_State = l_dt_State.Select("chvCountryCode not in('US','CA')")
    '        'End If
    '        l_dt_State_filtered = l_dt_State.Clone()
    '        For Each dr In l_dr_State
    '            l_dt_State_filtered.ImportRow(dr)
    '        Next
    '        Me.DropdownlistSecState.DataSource() = l_dt_State_filtered
    '        Me.DropdownlistSecState.DataBind()
    '        Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '        Me.DropdownlistSecState.Items(0).Value = ""
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        ExceptionPolicy.HandleException(ex, "Exception Policy")
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    '    'Shubhrata YREN 2779
    '    'Code Commented by ashutosh on 24-June-06
    '    'Try
    '    '    Dim l_DataSet_StateNames As DataSet
    '    '    If Me.DropdownlistSecCountry.SelectedItem.Text = "" Then
    '    '        'DropdownlistSecState.Items.Clear()
    '    '        'DropdownlistSecState.DataSource = Nothing
    '    '        'DropdownlistSecState.Items.Insert(0, "-Select State-")
    '    '        'DropdownlistSecState.Items(0).Value = ""
    '    '        DropdownlistSecState.SelectedValue = ""
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

    '    '    ''DropdownlistSecState.Items.Insert(0, "-Select State-")
    '    'Catch ex As Exception
    '    '    Throw
    '    'End Try
    'End Sub

    Private Sub PopcalendarSpousealWiver_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopcalendarSpousealWiver.SelectionChanged
        Me.ButtonCancel.Enabled = True
        Me.ButtonSaveParticipant.Enabled = True
        Me.MakeLinkVisible()
    End Sub

    Private Sub DataGridActiveBeneficiaries_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridActiveBeneficiaries.SortCommand
        Try
            Dim l_ds_Sort_ActiveBeneficiaries As DataSet
            'Commented By Dinesh Kanojia
            'Me.DataGridActiveBeneficiaries.SelectedIndex = -1
            If Not ViewState("DS_Sort_ActiveBeneficiaries") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_ds_Sort_ActiveBeneficiaries = ViewState("DS_Sort_ActiveBeneficiaries")
                dv = l_ds_Sort_ActiveBeneficiaries.Tables(0).DefaultView
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
                Me.DataGridActiveBeneficiaries.DataSource = Nothing
                Me.DataGridActiveBeneficiaries.DataSource = dv
                Me.DataGridActiveBeneficiaries.DataBind()
                Session("Participant_Sort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridAdditionalAccounts_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridAdditionalAccounts.SortCommand
        Try
            Dim l_ds_Sort_AddnlAccts As DataSet
            Me.DataGridAdditionalAccounts.SelectedIndex = -1
            If Not ViewState("DS_Sort_AddnlAccts") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_ds_Sort_AddnlAccts = ViewState("DS_Sort_AddnlAccts")
                dv = l_ds_Sort_AddnlAccts.Tables(0).DefaultView
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
                Me.DataGridAdditionalAccounts.DataSource = Nothing
                Me.DataGridAdditionalAccounts.DataSource = dv
                Me.DataGridAdditionalAccounts.DataBind()
                Session("Participant_Sort") = dv.Sort

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridPartAcctContribution_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridPartAcctContribution.SortCommand
        Try
            Dim l_ds_Sort_AcctContrib As DataSet
            Me.DataGridPartAcctContribution.SelectedIndex = -1
            If Not ViewState("DS_Sort_AcctContrib") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_ds_Sort_AcctContrib = ViewState("DS_Sort_AcctContrib")
                dv = l_ds_Sort_AcctContrib.Tables(0).DefaultView
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
                Me.DataGridPartAcctContribution.DataSource = Nothing
                Me.DataGridPartAcctContribution.DataSource = dv
                Me.DataGridPartAcctContribution.DataBind()
                Session("Participant_Sort") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    'Private Sub DataGridParticipantEmployment_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridParticipantEmployment.SortCommand
    '    Try
    '        Dim l_ds_Sort_Emp As DataSet
    '        Me.DataGridParticipantEmployment.SelectedIndex = -1
    '        ' If Not viewstate("Ds_Sort_employment") Is Nothing Then
    '        If Not Session("l_dataset_Employment") Is Nothing Then    'Shashi Shgekhar:11.04.2011- for YRS 5.0-1280 : ability to sort records in grid on employment tab
    '            Dim dv As New DataView
    '            Dim SortExpression As String
    '            SortExpression = e.SortExpression
    '            l_ds_Sort_Emp = Session("l_dataset_Employment")
    '            dv = l_ds_Sort_Emp.Tables(0).DefaultView
    '            dv.Sort = SortExpression
    '            'Shashi Shgekhar:11.04.2011- Commented for YRS 5.0-1280 : ability to sort records in grid on employment tab
    '            'Session("Participant_Sort") = "DESC" 
    '            If Not Session("Participant_Sort") Is Nothing Then
    '                If Session("Participant_Sort").ToString.Trim.EndsWith("ASC") Then
    '                    dv.Sort = SortExpression + " DESC"
    '                Else
    '                    dv.Sort = SortExpression + " ASC"
    '                End If
    '            Else
    '                dv.Sort = SortExpression + " ASC"
    '            End If
    '            Me.DataGridParticipantEmployment.DataSource = Nothing
    '            Me.DataGridParticipantEmployment.DataSource = dv
    '            Me.DataGridParticipantEmployment.DataBind()
    '            Session("Participant_Sort") = dv.Sort
    '        End If
    '        'If Not Session("l_dataset_Employment") Is Nothing Then
    '        '    Dim dv As New DataView
    '        '    Dim SortExpression As String
    '        '    SortExpression = e.SortExpression
    '        '    l_ds_Sort_Emp = Session("l_dataset_Employment")
    '        '    dv = l_ds_Sort_Emp.Tables(0).DefaultView
    '        '    If Session("Participant_Sort") = "" Or Session("Participant_Sort") = Nothing Then
    '        '        Session("Participant_Sort") = " DESC"
    '        '    ElseIf Session("Participant_Sort") = " DESC" Then
    '        '        Session("Participant_Sort") = " ASC"
    '        '    ElseIf Session("Participant_Sort") = " ASC" Then
    '        '        Session("Participant_Sort") = " DESC"
    '        '    End If
    '        '    dv.Sort = SortExpression + Session("Participant_Sort")
    '        '    Me.DataGridParticipantEmployment.DataSource = Nothing
    '        '    Me.DataGridParticipantEmployment.DataSource = dv
    '        '    Me.DataGridParticipantEmployment.DataBind()
    '        'End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        ExceptionPolicy.HandleException(ex, "Exception Policy")
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    Private Sub DataGridParticipantNotes_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridParticipantNotes.SortCommand
        Try
            'Vipul 01Feb06 Cache-Session
            'Dim cache As CacheManager
            'cache = CacheFactory.GetCacheManager

            Dim l_dt_Notes As DataTable
            Dim l_dt_new_notes As DataTable
            Dim l_dr_notes As DataRow
            Dim j As Integer = 0
            'l_dt_Notes = CType(cache.GetData("dtNotes"), DataTable)
            l_dt_Notes = DirectCast(Session("dtNotes"), DataTable)
            'Vipul 01Feb06 Cache-Session
            If l_dt_Notes Is Nothing Then Exit Sub

            ' Me.DataGridParticipantNotes.SelectedIndex = -1
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
                Me.DataGridParticipantNotes.DataSource = Nothing
                Me.DataGridParticipantNotes.DataSource = dv
                'Shubhrata YRSPS 4614
                Me.MakeDisplayNotesDataTable()
                'Shubhrata YRSPS 4614
                Me.DataGridParticipantNotes.DataBind()

            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub MultiPageParticipantsInformation_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MultiPageParticipantsInformation.SelectedIndexChange
        Try

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)

        End Try
    End Sub

    'Function Added By Dinesh Kanojia on 12/13/2012.
    Private Sub EditVoluntaryAccounts(ByVal DatagridRow As TableCellCollection, ByVal iIndex As Integer)

        Try
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            'Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAccountUpdateItem", Convert.ToInt32(Session("LoggedUserKey")))

            'If Not checkSecurity.Equals("True") Then
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            ''End : YRS 5.0-940

            ' If DataGridAdditionalAccounts.SelectedIndex <> -1 Then
            _icounter = 0
            Session("icounter") = _icounter
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
            Session("Flag") = "EditAccounts"
            'Modfiied By Ashutosh Patil as on 30-Mar-2007
            'YREN-3203,YREN-3205
            Session("AccountType") = IIf(IsDBNull(DatagridRow(m_const_int_AddAcctAccountType).Text), "", DatagridRow(m_const_int_AddAcctAccountType).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(4).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(4).Text.Trim)

            'SP : Made changes to rename contribution type monthly to Dollar -Start
            'Session("BasisCode") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctDescriptions).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctDescriptions).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(7).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(7).Text.Trim) 'Commented by SP : to handle contract type iwth code instead of text
            Session("BasisCode") = IIf(IsDBNull(DatagridRow(m_const_int_AddAcctBasisCode).Text), "", DatagridRow(m_const_int_AddAcctBasisCode).Text)
            'SP : Made changes to rename contribution type monthly to Dollar -End
            Session("Contribution%") = IIf(IsDBNull(DatagridRow(m_const_int_AddAcctContributionPer).Text), "0.00", DatagridRow(m_const_int_AddAcctContributionPer).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(8).Text), "0.00", DataGridAdditionalAccounts.SelectedItem.Cells(8).Text.Trim)
            Session("ContributionAmt") = IIf(IsDBNull(DatagridRow(m_const_int_AddAcctContribution).Text), "0.00", DatagridRow(m_const_int_AddAcctContribution).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(9).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(9).Text.Trim)
            Session("EffectiveDate") = IIf(IsDBNull(DatagridRow(m_const_int_AddAcctEffectiveDate).Text), "", DatagridRow(m_const_int_AddAcctEffectiveDate).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(10).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(10).Text.Trim)
            Session("TerminationDate") = IIf(IsDBNull(DatagridRow(m_const_int_AddAcctTerminationDate).Text), "", DatagridRow(m_const_int_AddAcctTerminationDate).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(11).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(11).Text.Trim)
            Session("HireDate") = IIf(IsDBNull(DatagridRow(m_const_int_AddAcctHireDate).Text), "", DatagridRow(m_const_int_AddAcctHireDate).Text.Trim)
            Session("EnrollmentDate") = IIf(IsDBNull(DatagridRow(m_const_int_AddAcctEnrollmentDate).Text), "", DatagridRow(m_const_int_AddAcctEnrollmentDate).Text.Trim)

            _icounter = Session("icounter")
            _icounter = _icounter + 1
            Session("_icounter") = _icounter

            Dim popupScript As String
            If (_icounter = 1) Then
                'If Me.DataGridAdditionalAccounts.SelectedItem.Cells(1).Text = "&nbsp;" Then
                'Modfiied By Ashutosh Patil as on 30-Mar-2007
                'YREN-3203,YREN-3205
                If DatagridRow(m_const_int_AddAcctUniqueId).Text = "&nbsp;" Then
                    'Added by prasad 2012.02.17:BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
                    'popupScript = "<script language='javascript'>" & _
                    '"window.open('AddAdditionalaccounts.aspx?Index=" & Me.DataGridAdditionalAccounts.SelectedIndex & "&YmcaId=" + Me.DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctYmcaNo).Text & "', 'CustomPopUp', " & _
                    '"'width=750, height=450, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                    '"</script>"

                    popupScript = "<script language='javascript'>" & _
                    "window.open('AddAdditionalaccounts.aspx?Index=" & iIndex & "&YmcaId=" + DatagridRow(m_const_int_AddAcctYmcaNo).Text & "', 'CustomPopUp', " & _
                    "'width=800, height=450, menubar=no, Resizable=No,top=120,left=120, scrollbars=yes')" & _
                    "</script>"

                Else
                    '     popupScript = "<script language='javascript'>" & _
                    '"window.open('AddAdditionalaccounts.aspx?UniqueId=" + Me.DataGridAdditionalAccounts.SelectedItem.Cells(1).Text + "','CustomPopUp', " & _
                    '"'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                    '"</script>"
                    'Modfiied By Ashutosh Patil as on 30-Mar-2007
                    'YREN-3203,YREN-3205
                    'Added by prasad 2012.02.17:BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
                    popupScript = "<script language='javascript'>" & _
                      "window.open('AddAdditionalaccounts.aspx?UniqueId=" + DatagridRow(m_const_int_AddAcctUniqueId).Text + "&YmcaId=" + DatagridRow(m_const_int_AddAcctYmcaNo).Text + "','CustomPopUp', " & _
                      "'width=800, height=500, menubar=no, Resizable=No,top=120,left=120, scrollbars=yes')" & _
                      "</script>"
                End If

                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", popupScript)

                End If
            Else
                Dim l_dataset_AddAccount As DataSet

                'Vipul 01Feb06 Cache-Session
                'l_dataset_AddAccount = Cache("l_dataset_AddAccount")
                l_dataset_AddAccount = Session("l_dataset_AddAccount")
                'Vipul 01Feb06 Cache-Session

                Me.DataGridAdditionalAccounts.DataSource = l_dataset_AddAccount
                ViewState("DS_Sort_AddnlAccts") = l_dataset_AddAccount
                Me.DataGridAdditionalAccounts.DataBind()
                _icounter = 0
                Session("icounter") = _icounter

            End If
            'Else
            '    MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please select an account to update.", MessageBoxButtons.Stop)
            'End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try

    End Sub

    'Rahul 01, Mar ,06
    Private Sub ButtonAccountUpdateItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAccountUpdateItem.Click
        Try
            'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
            HiddenFieldDirty.Value = "true"
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonAccountUpdateItem", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940

            If DataGridAdditionalAccounts.SelectedIndex <> -1 Then
                _icounter = 0
                Session("icounter") = _icounter
                ButtonSaveParticipant.Enabled = True
                ButtonCancel.Enabled = True
                ButtonOK.Enabled = False
                Me.MakeLinkVisible()
                Session("Flag") = "EditAccounts"
                'Modfiied By Ashutosh Patil as on 30-Mar-2007
                'YREN-3203,YREN-3205
                Session("AccountType") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctAccountType).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctAccountType).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(4).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(4).Text.Trim)

                'SP : Made changes to rename contribution type monthly to Dollar -Start
                'Session("BasisCode") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctDescriptions).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctDescriptions).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(7).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(7).Text.Trim) 'Commented by SP : to handle contract type iwth code instead of text
                Session("BasisCode") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctBasisCode).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctBasisCode).Text)
                'SP : Made changes to rename contribution type monthly to Dollar -End
                Session("Contribution%") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctContributionPer).Text), "0.00", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctContributionPer).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(8).Text), "0.00", DataGridAdditionalAccounts.SelectedItem.Cells(8).Text.Trim)
                Session("ContributionAmt") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctContribution).Text), "0.00", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctContribution).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(9).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(9).Text.Trim)
                Session("EffectiveDate") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctEffectiveDate).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctEffectiveDate).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(10).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(10).Text.Trim)
                Session("TerminationDate") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctTerminationDate).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctTerminationDate).Text.Trim) 'IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(11).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(11).Text.Trim)
                Session("HireDate") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctHireDate).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctHireDate).Text.Trim)
                Session("EnrollmentDate") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctEnrollmentDate).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctEnrollmentDate).Text.Trim)

                _icounter = Session("icounter")
                _icounter = _icounter + 1
                Session("_icounter") = _icounter

                Dim popupScript As String
                If (_icounter = 1) Then
                    'If Me.DataGridAdditionalAccounts.SelectedItem.Cells(1).Text = "&nbsp;" Then
                    'Modfiied By Ashutosh Patil as on 30-Mar-2007
                    'YREN-3203,YREN-3205
                    If Me.DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctUniqueId).Text = "&nbsp;" Then
                        'Added by prasad 2012.02.17:BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
                        popupScript = "<script language='javascript'>" & _
                        "window.open('AddAdditionalaccounts.aspx?Index=" & Me.DataGridAdditionalAccounts.SelectedIndex & "&YmcaId=" + Me.DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctYmcaNo).Text & "', 'CustomPopUp', " & _
                        "'width=750, height=450, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                        "</script>"
                    Else
                        '     popupScript = "<script language='javascript'>" & _
                        '"window.open('AddAdditionalaccounts.aspx?UniqueId=" + Me.DataGridAdditionalAccounts.SelectedItem.Cells(1).Text + "','CustomPopUp', " & _
                        '"'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                        '"</script>"
                        'Modfiied By Ashutosh Patil as on 30-Mar-2007
                        'YREN-3203,YREN-3205
                        'Added by prasad 2012.02.17:BT-990, YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
                        popupScript = "<script language='javascript'>" & _
                          "window.open('AddAdditionalaccounts.aspx?UniqueId=" + Me.DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctUniqueId).Text + "&YmcaId=" + Me.DataGridAdditionalAccounts.SelectedItem.Cells(m_const_int_AddAcctYmcaNo).Text + "','CustomPopUp', " & _
                          "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                          "</script>"
                    End If

                    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                        Page.RegisterStartupScript("PopupScript2", popupScript)

                    End If
                Else
                    Dim l_dataset_AddAccount As DataSet

                    'Vipul 01Feb06 Cache-Session
                    'l_dataset_AddAccount = Cache("l_dataset_AddAccount")
                    l_dataset_AddAccount = Session("l_dataset_AddAccount")
                    'Vipul 01Feb06 Cache-Session

                    Me.DataGridAdditionalAccounts.DataSource = l_dataset_AddAccount
                    ViewState("DS_Sort_AddnlAccts") = l_dataset_AddAccount
                    Me.DataGridAdditionalAccounts.DataBind()
                    _icounter = 0
                    Session("icounter") = _icounter

                End If
            Else
                MessageBox.Show(Me.PlaceHolder1, "YMCA-YRS", "Please select an account to update.", MessageBoxButtons.Stop)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try



    End Sub
    'Rahul 01, Mar ,06

    'Private Sub DatagridTotal_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridTotal.ItemDataBound
    '    If e.Item.ItemType <> ListItemType.Header Then
    '        e.Item.Cells(1).HorizontalAlign = HorizontalAlign.Right
    '        e.Item.Cells(2).HorizontalAlign = HorizontalAlign.Right
    '        e.Item.Cells(3).HorizontalAlign = HorizontalAlign.Right
    '        e.Item.Cells(4).HorizontalAlign = HorizontalAlign.Right
    '        e.Item.Cells(5).HorizontalAlign = HorizontalAlign.Right
    '        e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Right
    '        e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
    '        e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right

    '    End If

    '    If e.Item.ItemType <> ListItemType.Header Then
    '        '    e.Item.Visible = False
    '        e.Item.Cells(0).Text = "Total"
    '        e.Item.Cells(0).HorizontalAlign = HorizontalAlign.Left
    '    End If


    'End Sub

    'BS:2012:03:26:-restructure address code
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
    'BS:2012:03:29:-restructure address code
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
    'BS:2012:03:30:-restructure address code
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
    'Start:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    'Private Sub UpdateAddress(ByVal bitPrimary As Boolean, ByVal bitActive As Boolean,
    'ByVal l_ds_Address() As DataRow, ByRef AddressWebUserCtrl As AddressUserControlNew)

    '    AddressWebUserCtrl.UpdateAddressDetail

    'End Sub
    Private Function GetAddressTable(ByVal bitPrimary As Boolean, ByRef AddressWebUserCtrl As AddressUserControlNew) As DataTable
        Return AddressWebUserCtrl.GetAddressTable()
    End Function
    'End:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    Public Sub UpdateTelephone(ByVal bitPrimary As Boolean, ByVal bitActive As Boolean,
        ByVal Home As String, ByVal Telephone As String, ByVal Fax As String, ByVal Mobile As String, ByVal dtContacts As DataTable)
        Try
            If Not dtContacts Is Nothing Then
                'Start:Commented by Anudeep:20.03.2013 For telephone popup

                'TelephoneWebCtrl.UpdateTelephoneDetail(l_ds_Address, bitPrimary, bitActive)


                UpdateTelephoneDetail("HOME", Home, bitPrimary, bitActive, dtContacts)
                UpdateTelephoneDetail("OFFICE", Telephone, bitPrimary, bitActive, dtContacts)
                UpdateTelephoneDetail("MOBILE", Mobile, bitPrimary, bitActive, dtContacts)
                UpdateTelephoneDetail("FAX", Fax, bitPrimary, bitActive, dtContacts)

                ''Add TelephoneInfo in DB
                If HelperFunctions.isNonEmpty(dtContacts) Then
                    If Not dtContacts.GetChanges Is Nothing Then
                        Dim l_ds_Contacts As New DataSet()
                        l_ds_Contacts.Tables.Add(dtContacts.Copy())
                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(l_ds_Contacts)
                        'YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateTelephone(l_ds_Address)
                    End If
                End If
                'End:Commented by Anudeep:20.03.2013 For telephone popup
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
    Private Sub UpdateEmailInfo()

        'Email Infomation 
        'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
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
                'If l_ds_Email.Rows.Count > 0 Then
                If Not l_dt_Email.GetChanges Is Nothing Then
                    'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    Dim ds As New DataSet
                    ds.Tables.Add(l_dt_Email.Copy())
                    ds.Tables(0).TableName = "EmailInfo"
                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateEmailAddress(ds)
                End If
                'End If
                'End If
            End If
        End If

    End Sub

    '  Private Function ValidationCheckAddressDetails() As String
    '      Dim l_string_Message As String = String.Empty
    'Try

    '	l_string_Message = AddressWebUserControl1.ValidateAddress()
    '	If l_string_Message <> "" Then
    '		Return l_string_Message
    '		Exit Function
    '	End If
    '	l_string_Message = AddressWebUserControl2.ValidateAddress()
    '	If l_string_Message <> "" Then
    '		Return l_string_Message

    '		Exit Function
    '	End If


    '	'If Trim(m_str_Pri_Address1) = "" Then
    '	'	l_string_Message = "Please enter Address1 for Primary Address."
    '	'	Return l_string_Message
    '	'End If

    '	''Ashutosh Patil as on 25-Apr-2007
    '	''YREN-3298
    '	'If Trim(m_str_Pri_City) = "" Then
    '	'	l_string_Message = "Please enter City for Primary Address."
    '	'	Return l_string_Message
    '	'	Exit Function
    '	'End If

    '	'If Trim(m_str_Pri_Address1) <> "" Then
    '	'	If m_str_Pri_CountryText = "-Select Country-" And m_str_Pri_StateText = "-Select State-" Then
    '	'		l_string_Message = "Please select either State or Country for Primary Address."
    '	'		Return l_string_Message
    '	'		Exit Function
    '	'	End If
    '	'End If

    '	'If (m_str_Pri_CountryText = "-Select Country-") Then
    '	'	If m_str_Pri_StateText = "-Select State-" Then
    '	'		l_string_Message = "Please select either State or Country for Primary Address."
    '	'		Return l_string_Message
    '	'		Exit Function
    '	'	End If
    '	'End If

    '	'l_string_Message = ValidateCountrySelStateZip(m_str_Pri_CountryValue, m_str_Pri_StateValue, m_str_Pri_Zip)
    '	'If l_string_Message <> "" Then
    '	'	Return l_string_Message
    '	'	Exit Function
    '	'End If

    '	'If m_str_Pri_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Pri_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
    '	'	TextBoxZip.Text = ""
    '	'	l_string_Message = "Invalid Zip Code format For Country Canada"
    '	'	Return l_string_Message
    '	'	Exit Function
    '	'End If

    '	''Secondary Address Validations
    '	''Ashutosh Patil as on 14-Mar-2007
    '	''YREN - 3028,YREN-3029
    '	'If Trim(m_str_Sec_Address1) <> "" Then
    '	'	If Trim(m_str_Sec_City) = "" Then
    '	'		l_string_Message = "Please enter City for Secondary Address."
    '	'		Return l_string_Message
    '	'		Exit Function
    '	'	End If
    '	'	If (m_str_Sec_CountryText = "-Select Country-") Then
    '	'		If m_str_Sec_StateText = "-Select State-" Then
    '	'			l_string_Message = "Please select either State or Country for Secondary Address."
    '	'			Return l_string_Message
    '	'			Exit Function
    '	'		End If
    '	'	End If
    '	'End If

    '	'If Trim(m_str_Sec_Address2) <> "" Then
    '	'	If Trim(m_str_Sec_Address1) = "" Then
    '	'		l_string_Message = "Please enter Address for Secondary Address."
    '	'		Return l_string_Message
    '	'		Exit Function
    '	'	End If
    '	'End If

    '	'If Trim(m_str_Sec_Address3) <> "" Then
    '	'	If Trim(m_str_Sec_Address1) = "" Then
    '	'		l_string_Message = "Please enter Address1 for Secondary Address."
    '	'		Return l_string_Message
    '	'		Exit Function
    '	'	End If
    '	'End If

    '	'If m_str_Sec_City <> "" Then
    '	'	If Trim(m_str_Sec_Address1.ToString) = "" Then
    '	'		l_string_Message = "Please enter Address1 for Secondary Address."
    '	'		Return l_string_Message
    '	'		Exit Function
    '	'	End If
    '	'End If

    '	'If (m_str_Sec_CountryText <> "-Select Country-" Or m_str_Sec_StateText <> "-Select State-") Then
    '	'	If Trim(m_str_Sec_Address1) = "" Then
    '	'		l_string_Message = "Please enter Address1 for Secondary Address."
    '	'		Return l_string_Message
    '	'		Exit Function
    '	'	End If
    '	'End If

    '	'If Trim(m_str_Sec_Zip) <> "" Then
    '	'	If Trim(m_str_Sec_Address1) = "" Then
    '	'		l_string_Message = "Please enter Address1 for Secondary Address."
    '	'		Return l_string_Message
    '	'		Exit Function
    '	'	End If
    '	'End If

    '	'l_string_Message = ValidateCountrySelStateZip(m_str_Sec_CountryValue, m_str_Sec_StateValue, m_str_Sec_Zip)

    '	'If l_string_Message <> "" Then
    '	'	Return l_string_Message
    '	'	Exit Function
    '	'End If

    '	'If m_str_Sec_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(m_str_Sec_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
    '	'	l_string_Message = "Invalid Zip Code format For Country Canada"
    '	'	Return l_string_Message
    '	'	Exit Function
    '	'End If

    'Catch ex As Exception
    '	Throw
    'End Try
    '  End Function

    Private Function ValidateAddress() As String
        Dim l_string_Message As String = String.Empty
        Try
            'Added By Ashutosh Patil as on 23-Feb-2007 For YREN - 3029,YREN - 3028
            Call SetPrimaryAddressDetails()
            Call SetSecondaryAddressDetails()

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
    'NP:2012.04.11 - This method does not seem to do anything useful
    'Public Sub LoadDropdowns()
    '	Try

    '		Dim g_dataset_dsAddressStateType As DataSet
    '		Dim g_dataset_dsAddressCountry As DataSet
    '		Dim InsertRowState As DataRow
    '		Dim InsertRowCountry As DataRow
    '		'Code Commented by Ashutosh on 23 June 
    '		''g_dataset_dsAddressStateType = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressStateType()
    '		'''InsertRowState = g_dataset_dsAddressStateType.Tables(0).NewRow()
    '		'''InsertRowState.Item("chvCodeValue") = String.Empty
    '		'''InsertRowState.Item("chvShortDescription") = String.Empty
    '		''''If Not g_dataset_dsAddressStateType Is Nothing Then
    '		''''    g_dataset_dsAddressStateType.Tables(0).Rows.InsertAt(InsertRowState, 0)
    '		''''End If
    '		''Me.DropdownlistState.DataSource = g_dataset_dsAddressStateType
    '		''Me.DropdownlistState.DataMember = "State Type"
    '		''Me.DropdownlistState.DataTextField = "chvShortDescription"
    '		''Me.DropdownlistState.DataValueField = "chvCodeValue"
    '		''Me.DropdownlistState.DataBind()
    '		''Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '		''Me.DropdownlistState.Items(0).Value = ""
    '		''Me.DropdownlistSecState.DataSource = g_dataset_dsAddressStateType
    '		''Me.DropdownlistSecState.DataMember = "State Type"
    '		''Me.DropdownlistSecState.DataTextField = "chvShortDescription"
    '		''Me.DropdownlistSecState.DataValueField = "chvCodeValue"
    '		''Me.DropdownlistSecState.DataBind()
    '		''Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '		''Me.DropdownlistSecState.Items(0).Value = ""
    '		'' *********on 23 June
    '		g_dataset_dsAddressCountry = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.LookUpAddressCountry()
    '		InsertRowCountry = g_dataset_dsAddressCountry.Tables(0).NewRow()
    '		InsertRowCountry.Item("chvAbbrev") = String.Empty
    '		InsertRowCountry.Item("chvDescription") = String.Empty
    '		If Not g_dataset_dsAddressCountry Is Nothing Then
    '			g_dataset_dsAddressCountry.Tables(0).Rows.InsertAt(InsertRowCountry, 0)
    '		End If
    '		'Ashutosh Patil as on 26-Mar-2007
    '		'YREN - 3028,YREN-3029
    '		'Me.DropdownlistCountry.DataSource = g_dataset_dsAddressCountry
    '		'Me.DropdownlistCountry.DataMember = "Country"
    '		'Me.DropdownlistCountry.DataTextField = "chvDescription"
    '		'Me.DropdownlistCountry.DataValueField = "chvAbbrev"
    '		'Me.DropdownlistCountry.DataBind()
    '		'Me.DropdownlistCountry.Items.Insert(0, "-Select Country-")
    '		'Me.DropdownlistCountry.Items(0).Value = ""

    '		'Me.DropdownlistSecCountry.DataSource = g_dataset_dsAddressCountry
    '		'Me.DropdownlistSecCountry.DataMember = "Country"
    '		'Me.DropdownlistSecCountry.DataTextField = "chvDescription"
    '		'Me.DropdownlistSecCountry.DataValueField = "chvAbbrev"
    '		'Me.DropdownlistSecCountry.DataBind()
    '		'Me.DropdownlistSecCountry.Items.Insert(0, "-Select Country-")
    '		'Me.DropdownlistSecCountry.Items(0).Value = ""
    '	Catch ex As Exception
    '		Throw
    '	End Try
    'End Sub
    Public Sub SaveInfo()
        Try

            ''''' Code added by Rahul Nasa on 14 Feb, 2006
            Dim l_bool_msg_flag As Boolean = True
            Dim l_bool_val_flag As Boolean = True
            Dim l_string_PhonySSNo As String = ""
            Dim l_string_SSNOExist As String = String.Empty
            'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            Dim dtActiveBeneficariesAddress As DataTable
            Dim dtAddress As New DataTable
            Dim dtPrimaryAddressTable As DataTable
            Dim dtSecondaryAddressTable As DataTable
            'Added By Ashutosh Patil as on 06-Jun-2007
            'Validations for Address if Phony SSNo is validated properly but and secondary Address is not entered properly
            'YREN-3490  
            'Start Ashutosh Patil YREN-3490
            If Session("PhonySSNo") = "Phony_SSNo" Then
                Session("PhonySSNo") = Nothing
                If Me.ButtonEditAddress.Enabled = False Then
                    l_string_PhonySSNo = ProcessPhonySSNo()
                    If l_string_PhonySSNo.ToString = "NotValidated" Then
                        Exit Sub
                    End If
                End If
            End If
            'End Ashutosh Patil YREN-3490


            If TextBoxFirst.Text = "" Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "First Name must be entered in order to Save.", MessageBoxButtons.Stop, False)
                l_bool_msg_flag = False
                l_bool_val_flag = False
            End If
            If l_bool_val_flag = True Then
                If TextBoxLast.Text = "" Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Last Name must be entered in order to Save.", MessageBoxButtons.Stop, False)
                    l_bool_msg_flag = False
                End If
            End If
            ''''' Code added by Rahul Nasa on 14 Feb, 2006
            'Dim l_bool_msg_flag As Boolean = True


            Me.ButtonEditAddress.Enabled = True
            Dim l_date As Date
            l_date = System.DateTime.Today
            Dim flag As Boolean
            flag = False

            'change by ruchi to allow adding of additional accounts if the participant is Pre- Eligible

            Dim l_string_FundStatus As String
            If Not Session("FundStatus") Is Nothing Then
                l_string_FundStatus = Session("FundStatus")
            Else
                l_string_FundStatus = String.Empty
            End If
            'by Aparna YRPS-4051 19/11/2007
            'If l_string_FundStatus <> "PRE-ELIGIBLE" Then
            If l_string_FundStatus = "ACTIVE" Then
                If Me.TextBoxDOB.Text <> String.Empty Then
                    If Date.Compare(Convert.ToDateTime(Me.TextBoxDOB.Text), l_date.AddYears(-21)) = 1 Then
                        'perform address saving for any case
                        AddressWebUserControl1.AddrCode = "HOME"
                        AddressWebUserControl2.AddrCode = "HOME"
                        AddressWebUserControl1.EntityCode = EnumEntityCode.PERSON.ToString()
                        AddressWebUserControl2.EntityCode = EnumEntityCode.PERSON.ToString()
                        'BS:2012:03:26:-restructure address code
                        'UpdateAddress(True, True, False, True) 'by aparna 19/11/07
                        'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        Dim dr_PrimaryAddress As DataRow() = DirectCast(Session("Dr_PrimaryAddress"), DataRow())
                        If Not dr_PrimaryAddress Is Nothing Then
                            Dim dt_PrimaryContact As DataTable = DirectCast(Session("dt_PrimaryContactInfo"), DataTable)

                            'Changed by Anudeep:20.03.2013 For telephone popup
                            'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
                            dtPrimaryAddressTable = GetAddressTable(True, AddressWebUserControl1)
                            If TextBoxHome.Visible Then
                                UpdateTelephone(True, True, TextBoxHome.Text, TextBoxTelephone.Text, TextBoxFax.Text, TextBoxMobile.Text, dt_PrimaryContact)
                            End If
                        End If
                        Dim dr_SecondaryAddress As DataRow() = DirectCast(Session("Dr_SecondaryAddress"), DataRow())
                        If Not dr_SecondaryAddress Is Nothing Then
                            Dim dt_SecondaryContact As DataTable = DirectCast(Session("dt_SecondaryContactInfo"), DataTable)
                            'Changed by Anudeep:20.03.2013 For telephone popup
                            dtSecondaryAddressTable = GetAddressTable(False, AddressWebUserControl2)
                            If TextBoxSecHome.Visible Then
                                UpdateTelephone(False, True, TextBoxSecHome.Text, TextBoxSecTelephone.Text, TextBoxSecFax.Text, TextBoxSecMobile.Text, dt_SecondaryContact)
                            End If
                            'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
                        End If
                        'Start:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        dtAddress = dtPrimaryAddressTable.Clone
                        If HelperFunctions.isNonEmpty(dtPrimaryAddressTable) Then
                            dtAddress.ImportRow(dtPrimaryAddressTable.Rows(0))
                        End If
                        If HelperFunctions.isNonEmpty(dtSecondaryAddressTable) Then
                            dtAddress.ImportRow(dtSecondaryAddressTable.Rows(0))
                        End If
                        Address.SaveAddress(dtAddress)
                        UpdateEmailInfo()
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Active participant must be atleast 21 years of age.", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                Else
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Date of birth cannot be blank for the Participant.", MessageBoxButtons.Stop)
                    Exit Sub
                End If

                'Added by Anudeep:22.03.2013-For Checking active as Primary
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
            End If
            'End:Anudeep:22.08.2013-YRS 5.0-1862:Add notes record when user enters address in any module.
            'End:Anudeep 01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            LoadNotesTab()

            'by Aparna YRPS-4051 19/11/2007


            If TextBoxSpousealWiver.Text <> "" Then
                If (Date.Compare(Convert.ToDateTime(TextBoxSpousealWiver.Text), l_date) > 0) Then
                    flag = True
                End If
            End If

            If flag And l_bool_msg_flag Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", "The Spousal Waiver date cannot be greater than today's date.", MessageBoxButtons.Stop)
                'Anudeep:11.11.2013:BT-1455:YRS 5.0-1733:Enabling the save button after displaying validation message
                Me.ButtonSaveParticipants.Enabled = False
                Me.ButtonSaveParticipants.Visible = False
                Me.ButtonSaveParticipant.Visible = True
                Me.ButtonSaveParticipant.Enabled = True
                Me.ButtonCancel.Enabled = True
                l_bool_msg_flag = False
                Exit Sub
            End If

            'If l_bool_msg_flag And Me.TextBoxPrimaryA.Text = "" And Session("WithDrawn_member") = False Then
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Active primary beneficiary required.", MessageBoxButtons.Stop)
            '    l_bool_msg_flag = False
            '    Exit Sub
            'End If
            If l_bool_msg_flag And Not Me.TextBoxPrimaryA.Text = "" Then
                If (((Convert.ToDecimal(Me.TextBoxPrimaryA.Text) < 100) Or (Convert.ToDecimal(Me.TextBoxPrimaryA.Text) > 100))) And Session("WithDrawn_member") = False Then
                    'Name:Preeti Date:7thFeb06 IssueId:2053 Reason:Message Box text change START
                    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Primary member beneficiary percents do not total 100%.", MessageBoxButtons.Stop)
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Primary member beneficiary must equal 100 percent.", MessageBoxButtons.Stop)
                    'Name:Preeti Date:7thFeb06 IssueId:2053 Reason:Message Box text change END
                    'Anudeep:11.11.2013:BT-1455:YRS 5.0-1733:Enabling the save button after displaying validation message
                    Me.ButtonSaveParticipants.Enabled = False
                    Me.ButtonSaveParticipants.Visible = False
                    Me.ButtonSaveParticipant.Visible = True
                    Me.ButtonSaveParticipant.Enabled = True
                    Me.ButtonCancel.Enabled = True
                    l_bool_msg_flag = False
                    Exit Sub
                End If
            End If

            If l_bool_msg_flag And Me.TextBoxCont1A.Text <> "" Then
                If (((Convert.ToDecimal(Me.TextBoxCont1A.Text) < 100) Or (Convert.ToDecimal(Me.TextBoxCont1A.Text) > 100))) And Session("WithDrawn_member") = False Then
                    'Name:Preeti Date:7thFeb06 IssueId:2053 Reason:Message Box text change START
                    'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Contingent Level 1 member beneficiary percents do not total 100%.", MessageBoxButtons.Stop)
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Contingent Level 1 member beneficiary must equal 100 percent.", MessageBoxButtons.Stop)
                    'Name:Preeti Date:7thFeb06 IssueId:2053 Reason:Message Box text change END
                    l_bool_msg_flag = False
                End If
            End If

            If l_bool_msg_flag And Me.TextBoxCont2A.Text <> "" Then
                If (((Convert.ToDecimal(Me.TextBoxCont2A.Text) < 100) Or (Convert.ToDecimal(Me.TextBoxCont2A.Text) > 100))) And Session("WithDrawn_member") = False Then
                    'Name:Preeti Date:7thFeb06 IssueId:2053 Reason:Message Box text change START
                    '                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Contingent Level 2 member beneficiary percents do not total 100%.", MessageBoxButtons.Stop)
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Contingent Level 2 member beneficiary must equal 100 percent.", MessageBoxButtons.Stop)
                    'Name:Preeti Date:7thFeb06 IssueId:2053 Reason:Message Box text change END
                    l_bool_msg_flag = False
                End If
            End If

            If l_bool_msg_flag And Me.TextBoxCont3A.Text <> "" Then
                If (((Convert.ToDecimal(Me.TextBoxCont3A.Text) < 100) Or (Convert.ToDecimal(Me.TextBoxCont3A.Text) > 100))) And Session("WithDrawn_member") = False Then
                    'Name:Preeti Date:7thFeb06 IssueId:2053 Reason:Message Box text change START
                    '                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Contingent Level 3 member beneficiary percents do not total 100%.", MessageBoxButtons.Stop)
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Contingent Level 3 member beneficiary must equal 100 percent.", MessageBoxButtons.Stop)
                    'Name:Preeti Date:7thFeb06 IssueId:2053 Reason:Message Box text change END
                    l_bool_msg_flag = False
                End If
            End If

            If l_bool_msg_flag Then


                Dim l_dataset_Active As DataSet
                'Vipul 01Feb06 Cache-Session
                'Dim Cache As CacheManager
                'Cache = CacheFactory.GetCacheManager()

                'If Not Cache("BeneficiariesActive") Is Nothing Then
                '    l_dataset_Active = Cache("BeneficiariesActive")
                '    CalculateValues(l_dataset_Active.Tables(0), "A")
                'End If
                If Not Session("BeneficiariesActive") Is Nothing Then
                    l_dataset_Active = Session("BeneficiariesActive")
                    CalculateValues(l_dataset_Active.Tables(0), "A")
                End If

                If Session("ValidationMessage") Is Nothing Then
                    Dim dsEmployment As DataSet
                    Dim dsAddAccount As DataSet
                    Dim dsActiveBeneficiaries As DataSet
                    Dim dsRetiredBeneficiaries As DataSet

                    Dim l_string_output As String
                    'Commented and Modified By Ashutosh Patil as on 21-Jun-2007
                    'Check for Duplication of SSNo
                    'Start Ashutosh Patil as on 21-Jun-2007 YREN-3490
                    'UpadateGeneralInfo()
                    'by aparna - check that date of birth is not blank 12-Nov-2007

                    'YRPS-4704 - Start
                    'Commented by Swopna 21May08,YRPS-4704------start
                    'If DropDownListMaritalStatus.SelectedValue <> "M" Then
                    '    Dim dRows As DataRow()

                    '    dsActiveBeneficiaries = Session("BeneficiariesActive")
                    '    If Not dsActiveBeneficiaries Is Nothing Then
                    '        If dsActiveBeneficiaries.Tables.Count > 0 Then
                    '            If dsActiveBeneficiaries.Tables(0).Rows.Count > 0 Then
                    '                dRows = dsActiveBeneficiaries.Tables(0).Select("Rel='SP'")
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

                    If Not TextBoxDOB.Text = String.Empty Then
                        l_string_SSNOExist = UpadateGeneralInfo()
                    Else
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Date of birth cannot be blank for the Participant.", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    'by aparna 12-Nov-2007

                    If l_string_SSNOExist <> "" Then
                        'MessageBox.Show("YMCA-YRS", "SSNo. is Already in use for Participant -" + l_string_SSNOExist, MessageBoxButtons.Stop)

                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", "SSNo. is Already in use for Participant -" + l_string_SSNOExist, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    'End Ashutosh Patil as on 21-Jun-2007
                    If Not Session("General") = False Then

                        'Vipul 01Feb06 Cache-Session
                        'dsEmployment = Cache("l_dataset_Employment")
                        dsEmployment = Session("l_dataset_Employment")
                        'Vipul 01Feb06 Cache-Session

                        If Not dsEmployment Is Nothing Then
                            l_string_output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.AddEmployment(dsEmployment)
                            dsEmployment.AcceptChanges()
                            'Vipul 01Feb06 Cache-Session
                            'Cache.Add("l_dataset_Employment", dsEmployment)
                            Session("l_dataset_Employment") = dsEmployment
                            'Vipul 01Feb06 Cache-Session

                            Session("Flag") = ""
                            LoadEmploymentTab()
                            ''Me.DataGridParticipantEmployment.DataSource = dsEmployment
                            ''Me.DataGridParticipantEmployment.DataBind()

                        End If

                        'Vipul 01Feb06 Cache-Session
                        'dsAddAccount = Cache("l_dataset_AddAccount")
                        dsAddAccount = Session("l_dataset_AddAccount")
                        'Vipul 01Feb06 Cache-Session

                        If Not dsAddAccount Is Nothing Then
                            l_string_output = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.AddAdditionalAccount(dsAddAccount)
                            dsAddAccount.AcceptChanges()

                            'Vipul 01Feb06 Cache-Session
                            'Cache.Add("l_dataset_AddAccount", dsAddAccount)
                            Session("l_dataset_AddAccount") = dsAddAccount
                            'Vipul 01Feb06 Cache-Session

                            Session("Flag") = ""
                            LoadAdditionalAccountsTab()
                            ''Me.DataGridAdditionalAccounts.DataSource = dsAddAccount
                            ''Me.DataGridAdditionalAccounts.DataBind()

                        End If

                        'Vipul 01Feb06 Cache-Session
                        'dsActiveBeneficiaries = Cache("BeneficiariesActive")
                        dsActiveBeneficiaries = Session("BeneficiariesActive")
                        'Vipul 01Feb06 Cache-Session

                        If Not dsActiveBeneficiaries Is Nothing Then

                            'SP 2014.12.01 BT-2310\YRS 5.0-2255: - Start
                            ''This code has moved here because we need to update the deletion reason of beneficiary in atsbeneficiaries table before deletion of beneficiary.
                            'Start:Anudeep:02.04.2013:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion  
                            'If Not Session("strReason") Is Nothing Then
                            '    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertBeneficiaryNotes(Session("strReason").ToString(), Session("dtmDOD"), Session("strBeneficiaryName").ToString(), Session("strComments").ToString(), Session("bImp").ToString(), Session("PersId").ToString(), 1)
                            'End If
                            If Not Session("dtReason") Is Nothing Then
                                Dim dtReason As DataTable
                                dtReason = Session("dtReason")
                                For Each drReason As DataRow In dtReason.Rows
									'SB | 2016.09.21 | YRS-AT-3028 -  Add new parameter to capture deceased beneficiary SSN
                                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertBeneficiaryNotes(drReason("strReason").ToString(), drReason("strDOD"), drReason("strBeneficiaryName").ToString(), drReason("strComments").ToString(), drReason("bImp").ToString(), Session("PersId").ToString(), 1, drReason("strBeneUniqueID").ToString(), drReason("strBeneficiarySSNo").ToString()) 
                                Next
                                Session("dtReason") = Nothing

                            End If
                            'End:Anudeep:02.04.2013:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion  
                            'SP 2014.12.01 BT-2310\YRS 5.0-2255: - End

                            ' // START : SB | 07/07/2016 | YRS-AT-2382 |  Getting only Updated SSN values for inserting in Audit DataTable  , inserting records in Audit Log
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

                                Dim dsBeneficiariesSSN As DataSet = New DataSet("dsBeneficiariesSSN")

                                If dtBeneficiariesSSNchanges.Rows.Count > 0 Then
                                    dsBeneficiariesSSN.Tables.Add(dtBeneficiariesSSNchanges)
                                    dsBeneficiariesSSN.Tables(0).TableName = "Audit"
                                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertBeneficiariesSSNChangeAuditRecord(dsBeneficiariesSSN)
                                    Session("AuditBeneficiariesTable") = Nothing
                                    dsBeneficiariesSSN.Clear()
                                End If
                            End If
                            ' // END : SB | 07/07/2016 | YRS-AT-2382 | Getting only Updated SSN values for inserting in Audit DataTable  , inserting records in Audit Log
                            ' START | Manthan Rajguru | 2016.07.26 | YRS-AT-2560 | Getting System Generated Phony SSN and assigning it to dataset
                            If dsActiveBeneficiaries.HasChanges Then
                                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GeneratePhonySSN(dsActiveBeneficiaries)
                            End If
                            ' END | Manthan Rajguru | 2016.07.26 | YRS-AT-2560 | Getting System Generated Phony SSN and assigning it to dataset

                            If dsActiveBeneficiaries.HasChanges Then
                                YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertBeneficiaries(dsActiveBeneficiaries)
                                dsActiveBeneficiaries.AcceptChanges()
                            End If

                            'Vipul 01Feb06 Cache-Session
                            'Cache.Add("BeneficiariesActive", dsActiveBeneficiaries)
                            Session("BeneficiariesActive") = dsActiveBeneficiaries
                            'Vipul 01Feb06 Cache-Session

                            Session("Flag") = "Active"

                            Session("Flag") = ""
                        End If

                        dtActiveBeneficariesAddress = Session("BeneficiaryAddress")

                        'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                        If Not dtActiveBeneficariesAddress Is Nothing Then
                            For Each drAddress As DataRow In dtActiveBeneficariesAddress.Rows
                                If Not String.IsNullOrEmpty(drAddress("NewId").ToString()) Then
                                    Dim drBeneRow As DataRow()
                                    drBeneRow = dsActiveBeneficiaries.Tables(0).Select("NewId = '" + drAddress("NewId").ToString() + "'")
                                    If drBeneRow.Length > 0 Then
                                        drAddress("BeneID") = drBeneRow(0)("UniqueId")
                                    End If
                                End If
                            Next
                            ' START | Manthan Rajguru | 2016.07.26 | YRS-AT-2560 | Assigning system generated phony SSN for new beneficiary added
                            For Each drAddress As DataRow In dtActiveBeneficariesAddress.Rows
                                If Not String.IsNullOrEmpty(drAddress("BeneID").ToString()) Then 'BenSSNo
                                    Dim drBeneRow As DataRow()
                                    drBeneRow = dsActiveBeneficiaries.Tables(0).Select("UniqueID = '" + drAddress("BeneID").ToString() + "'")
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


                            Address.SaveAddressOfBeneficiariesByPerson(dtActiveBeneficariesAddress)
                        End If

                        LoadBeneficiariesTab()

                        Dim dtNotes As New DataTable
                        Dim dsNotes As New DataSet
                        'Vipul 01Feb06 Cache-Session
                        'If Not Cache("dtNotes") Is Nothing Then
                        '    dtNotes = CType(Cache("dtNotes"), DataTable).Copy
                        If Not Session("dtNotes") Is Nothing Then
                            dtNotes = DirectCast(Session("dtNotes"), DataTable).Copy
                            'Vipul 01Feb06 Cache-Session
                            dsNotes.Tables.Add(dtNotes)
                            Session("Flag") = ""
                        Else
                            dtNotes = Nothing
                        End If

                        ''Commented by aparna -YREN-3115
                        'If Not dtNotes Is Nothing Then
                        '    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertParticipantNotes(dsNotes)
                        '    dsNotes.AcceptChanges()

                        '    'Vipul 01Feb06 Cache-Session
                        '    'Cache.Add("dtNotes", dtNotes)
                        '    Session("dtNotes") = dtNotes
                        '    'Vipul 01Feb06 Cache-Session

                        '    Me.DataGridParticipantNotes.DataSource = dtNotes
                        '    ' viewstate("DS_Sort_Notes") = dtNotes
                        '    Me.DataGridParticipantNotes.DataBind()

                        'End If
                        'Commented by aparna -YREN-3115
                        Dim blnChanged As Boolean = False
                        Dim l_string_PersId As String = Session("PersId")
                        If Not dtNotes Is Nothing Then
                            If Not dtNotes.GetChanges Is Nothing Then
                                If dtNotes.GetChanges.Rows.Count > 0 Then
                                    blnChanged = True
                                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertParticipantNotes(dsNotes)
                                    dsNotes.AcceptChanges()

                                End If
                            End If


                            If blnChanged = True Then
                                dtNotes = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.MemberNotes(l_string_PersId)
                                blnChanged = False
                            End If
                            LoadNotesTab()
                            Session("dtNotes") = dtNotes
                            'Shubhrata Feb 12th 2008
                            Session("Participant_Sort") = Nothing
                            'Shubhrata Feb 12th 2008
                            'by aparna -YREN-3115
                            NotesFlag.Value = "Notes"
                            Me.DataGridParticipantNotes.DataSource = dtNotes
                            'Shubhrata YRSPS 4614
                            Me.MakeDisplayNotesDataTable()
                            'Shubhrata YRSPS 4614
                            Me.DataGridParticipantNotes.DataBind()

                        End If

                        dtNotes = Nothing
                        dsNotes = Nothing
                        ButtonSaveParticipant.Enabled = False
                        ButtonCancel.Enabled = False
                        ButtonOK.Enabled = True
                        Me.MakeLinkVisible()

                        'BS:2012:03:26:-restructure address code
                        'UpdateAddress(True, True, False, True)
                        AddressWebUserControl1.AddrCode = "HOME"
                        AddressWebUserControl2.AddrCode = "HOME"
                        AddressWebUserControl1.EntityCode = EnumEntityCode.PERSON.ToString()
                        AddressWebUserControl2.EntityCode = EnumEntityCode.PERSON.ToString()
                        Dim dr_PrimaryAddress As DataRow() = DirectCast(Session("Dr_PrimaryAddress"), DataRow())
                        If Not dr_PrimaryAddress Is Nothing Then
                            Dim dt_PrimaryContact As DataTable = DirectCast(Session("dt_PrimaryContactInfo"), DataTable)

                            'Changed by Anudeep:20.03.2013 For telephone popup
                            'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
                            'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                            dtPrimaryAddressTable = GetAddressTable(True, AddressWebUserControl1)
                            If TextBoxHome.Visible Then
                                UpdateTelephone(True, True, TextBoxHome.Text, TextBoxTelephone.Text, TextBoxFax.Text, TextBoxMobile.Text, dt_PrimaryContact)
                            End If
                        End If
                        dtAddress = dtPrimaryAddressTable.Clone() 'Manthan | 2016.02.24 | YRS-AT-2328 | Deactivate secondary address

                        Dim dr_SecondaryAddress As DataRow() = DirectCast(Session("Dr_SecondaryAddress"), DataRow())
                        If Not dr_SecondaryAddress Is Nothing Then
                            Dim dt_SecondaryContact As DataTable = DirectCast(Session("dt_SecondaryContactInfo"), DataTable)
                            'Changed by Anudeep:20.03.2013 For telephone popup
                            'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                            dtSecondaryAddressTable = GetAddressTable(False, AddressWebUserControl2)
                            'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Adding new record in datatable for secondary address
                            If btnDeactivateSecondaryAddrs.Enabled = False And dtSecondaryAddressTable.Rows.Count = 0 And dr_SecondaryAddress.Length > 0 Then
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
                            'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
                        End If

                        'Start:Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses                        

                        If HelperFunctions.isNonEmpty(dtPrimaryAddressTable) Then
                            dtAddress.ImportRow(dtPrimaryAddressTable.Rows(0))
                        End If
                        If HelperFunctions.isNonEmpty(dtSecondaryAddressTable) Then
                            dtAddress.ImportRow(dtSecondaryAddressTable.Rows(0))
                        End If

                        Address.SaveAddress(dtAddress)

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
                        'End:Anudeep:22.08.2013-YRS 5.0-1862:Add notes record when user enters address in any module.

                        LoadNotesTab()
                        'End:Added by Anudeep:22.03.2013-For Checking active as Primary 
                        UpdateEmailInfo()
                    End If
                    tbPricontacts.Visible = False
                    tbSeccontacts.Visible = False
                    'btnEditPrimaryContact.Visible = False
                    'btnEditSecondaryContact.Visible = False
                    TelephoneWebUserControl1.Visible = True
                    TelephoneWebUserControl2.Visible = True

                Else
                    Dim l_string_ValidationMessage As String
                    l_string_ValidationMessage = Session("ValidationMessage")
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_ValidationMessage, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'Added by prasad Jadhav on 15-Sep-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
                HiddenFieldDirty.Value = "false"


                'Start DInesh.k 2013.09.17 BT-2139:YRS 5.0-2165:RMD enhancements. 
                If TextBoxRMDTax.Text.Trim <> String.Empty AndAlso Convert.ToDouble(RMDTaxRate) <> Convert.ToDouble(TextBoxRMDTax.Text.Trim) Then 'SP 2014.04.29 BT-2525 (adding and condition to avoid un-neccessary update)
                    'Dinesh Kanojia     2014.02.18      BT-2139: YRS 5.0-2165:RMD enhancements - remove unwanted parameter to update mrdtaxrate.
                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.ModifyPerssMetaConfiguration(Session("PersId").ToString(), "RMDTAXRATE", TextBoxRMDTax.Text.Trim)
                End If
                'END DInesh.k 2013.09.17 BT-2139:YRS 5.0-2165:RMD enhancements. 

                TextBoxRMDTax.Enabled = False 'SR:2013.11.19 : YRS 5.0-2165 - Disabled RMD tax after save process.


                'Ashutosh Patil as on 26-Mar-2007
                'YREN - 3028,YREN-3029
                'TextBoxAddress1.ReadOnly = True
                'TextBoxAddress2.ReadOnly = True
                'TextBoxAddress3.ReadOnly = True
                'TextBoxCity.ReadOnly = True
                'DropdownlistState.Enabled = False
                'TextBoxZip.ReadOnly = True
                'DropdownlistCountry.Enabled = False
                'start:BS:2012.04.26:YRS 5.0-1470: && restructure Address control
                Me.AddressWebUserControl1.EnableControls = False
                'Added by Anudeep:12.03.2013-For Telephone Usercontrol
                'Me.TelephoneWebUserControl1.EditPrimaryContact_Enabled = False
                'Commneted By Dinesh Kanojia Contacts
                'TextBoxTelephone.ReadOnly = True
                'TextBoxHome.ReadOnly = True
                'TextBoxFax.ReadOnly = True
                'TextBoxMobile.ReadOnly = True
                'TextBoxSecTelephone.ReadOnly = True
                'TextBoxSecHome.ReadOnly = True
                'TextBoxSecFax.ReadOnly = True
                'TextBoxSecMobile.ReadOnly = True
                TextBoxEmailId.ReadOnly = True
                'BS:2012.03.09:-restructure Address Control
                'Me.TextBoxEffDate.Enabled = False
                'Me.TextBoxSecEffDate.Enabled = False

                'Ashutosh Patil as on 26-Mar-2007
                'YREN - 3028,YREN-3029
                'Me.TextBoxSecAddress1.Enabled = False
                'Me.TextBoxSecAddress2.Enabled = False
                'Me.TextBoxSecAddress3.Enabled = False
                'Me.TextBoxSecCity.Enabled = False
                'Me.DropdownlistSecCountry.Enabled = False
                'Me.TextBoxSecZip.Enabled = False
                'Me.DropdownlistSecState.Enabled = False

                '  Me.TextBoxSecEmail.Enabled = False
                'Commented by Aparna -17/01/2007
                'Me.TextBoxSecTelephone.Enabled = False
                'TextBoxSecFax.Enabled = False
                'TextBoxSecHome.Enabled = False
                'TextBoxSecMobile.Enabled = False
                'Commented by Aparna -17/01/2007
                'start:BS:2012.04.26:YRS 5.0-1470: && restructure Address control
                Me.AddressWebUserControl2.EnableControls = False
                'Added by Anudeep:20.03.2013 For telephone popup
                'Me.TelephoneWebUserControl2.EditSecondaryContact_Enabled = False
                'Me.ButtonActivateAsPrimary.Enabled = False
                'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from participant and put on address control
                'Me.CheckboxSecIsBadAddress.Enabled = False
                'Me.CheckboxIsBadAddress.Enabled = False
                Me.CheckboxBadEmail.Enabled = False
                'Me.CheckboxSecBadEmail.Enabled = False
                'Me.CheckboxSecTextOnly.Enabled = False
                'Me.CheckboxSecUnsubscribe.Enabled = False
                Me.CheckboxTextOnly.Enabled = False
                Me.CheckboxUnsubscribe.Enabled = False
                'BS:2012.03.09:-restructure Address Control
                'Me.TextBoxSecEffDate.Enabled = False
                'Me.TextBoxEffDate.Enabled = False
                'AA:2013.10.23 - Bt-2264: below code and has been placed for getting the notes from database
                Session("dtNotes") = Nothing
                LoadNotesTab()
                'Start: Bala: 01/05/2016: YRS-AT-1972: Saving special death process bit flag.
                If Not ViewState("InitialSpecialDeathProcessingRequiredBitFlag") = chkSpecialDeathProcess.Checked Then
                    ViewState("InitialSpecialDeathProcessingRequiredBitFlag") = chkSpecialDeathProcess.Checked
                    YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.ModifyPerssMetaConfiguration(Session("PersId").ToString(), "IsSpecialDeathProcessingRequired", chkSpecialDeathProcess.Checked.ToString())
                End If
                'End: Bala: 01/05/2016: YRS-AT-1972: Saving special death process bit flag.
                LoadGeneraltab()
                'Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
                EditAddressAndContacts()
                Me.ButtonSaveParticipant.Enabled = False
                Me.ButtonSaveParticipants.Enabled = False
                'Anudeep:26.08.2013 - Bt-2132:YRS 5.0-2162 :  Edit button for email address 
            Else
                'Anudeep:06.11.2013:BT-1455:YRS 5.0-1733:Enabling the save button after displaying validation message
                Me.ButtonSaveParticipants.Enabled = False
                Me.ButtonSaveParticipants.Visible = False
                Me.ButtonSaveParticipant.Visible = True
                Me.ButtonSaveParticipant.Enabled = True
                Me.ButtonCancel.Enabled = True
            End If

            'START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 
            'ClearSession()  'SB | 10/18/2017 | YRS-AT-3324 | Clearing Session variable used to check RMD generated or not
            'END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | commented below lines of code as the below validation is shifted on death notification pop-up page. 

        Catch ex As Exception
            Throw
        End Try
    End Sub
    'BS:2012:04:04:-restructure ActiveAsPrimary code
    Public Function ActiveAsPrimary()
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

            'Dim dt As DataTable = DirectCast(Session("Dt_EmailAddress"), DataTable)
            'If Not dt Is Nothing Then
            '    dr_PrimaryAddress.Tables.Remove(dt)
            '    dr_SecondaryAddress.Tables.Add(dt)
            'End If
            'Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
            AddressWebUserControl1.LoadAddressDetail(dr_SecondaryAddress)
            'Added by Anudeep:12.03.2013-For Telephone Usercontrol
            TelephoneWebUserControl1.RefreshTelephoneDetail(dt_SecondaryContact)
            AddressWebUserControl2.LoadAddressDetail(dr_PrimaryAddress)
            'Added by Anudeep:12.03.2013-For Telephone Usercontrol
            TelephoneWebUserControl2.RefreshTelephoneDetail(dt_PrimaryContact)
            'RefreshSecTelephoneDetail(ds_PrimaryAddress1)

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
            Exit Function
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
        Dim l_string_message As String = String.Empty
        Try
            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonActivateAsPrimary", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            'BS:2012:03:26:-restructure address code
            'Call SetPrimaryAddressDetails()
            'Call SetSecondaryAddressDetails()
            l_string_message = Me.ValidateAddress()
            If l_string_message <> String.Empty Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_message, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'Priya 06-10-2006 YRS 5.0-424
            'Session("ActivateAsPrimary") = True
            'BS:2012:03:26:-restructure address code
            'UpdateAddress(False, True, True, True)
            ActiveAsPrimary()
            btnEditPrimaryContact.Enabled = False
            btnEditSecondaryContact.Enabled = False
            AddressWebUserControl1.EnableControls = False
            AddressWebUserControl2.EnableControls = False
            ButtonEditAddress.Enabled = False
            ButtonActivateAsPrimary.Enabled = False
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            HiddenFieldDirty.Value = "true" ' Manthan | 2016.04.06 | YRS-AT-2328 | Setting hidden field value to true to display confirm message when user clicks on close button without saving.
            Exit Sub

            If Not Session("ErrorMessage") Is Nothing Then
                l_string_message = Session("ErrorMessage")
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", l_string_message, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'Ashutosh Patil as on 19-Mar-2007
            'YREN-3029, YREN - 3028	
            'AddressWebUserControl1.guiPerssId = Session("PersId")
            'AddressWebUserControl1.IsPrimary = 1
            'AddressWebUserControl1.ShowDataForParticipant = 1
            'AddressWebUserControl1.ShowDataForParticipant()
            'AddressWebUserControl2.guiPerssId = Session("PersId")
            'AddressWebUserControl2.IsPrimary = 0
            'AddressWebUserControl2.ShowDataForParticipant = 0
            'AddressWebUserControl2.ShowDataForParticipant()
            'LoadGeneraltab()
            'Session("ActivateAsPrimary") = Nothing

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'Start - Manthan | 2016.02.24 | YRS-AT-2328 | Added btnDeactivateSecondaryAddrs_click method to perform deactivation of secondary address 
    Private Sub btnDeactivateSecondaryAddrs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeactivateSecondaryAddrs.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnDeactivatePersSecondaryAddrs", Convert.ToInt32(Session("LoggedUserKey")))

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
        ButtonCancel.Enabled = True
        ButtonActivateAsPrimary.Enabled = False
        ButtonSaveParticipant.Enabled = True
        HiddenFieldDirty.Value = "true" ' Manthan | 2016.04.06 | YRS-AT-2328 | Setting hidden field value to true to display confirm message when user clicks on close button without saving.
    End Sub
    'End - Manthan | 2016.02.24 | YRS-AT-2328 | Added btnDeactivateSecondaryAddrs_click method to perform deactivation of secondary address 
    Private Sub DataGridAdditionalAccounts_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAdditionalAccounts.Load

    End Sub

    Private Function LoadLoansTab()
        Dim l_string_PersId As String
        Dim l_dataset As DataSet
        Dim l_datarow As DataRow

        'START : VC | 2018.08.07 | YRS-AT-4018 | Declared a variable to store web loan status,Application and loan number
        Dim loanNumber As Integer
        Dim application As String
        Dim webLoanStatus As String
        Dim loanStatus As String
        Dim disbStatus As String
        'END : VC | 2018.08.07 | YRS-AT-4018 | Declared a variable to store web loan status,Application and loan number

        Dim isPayOffAmountCanBeSaved As Boolean ' PPP | 06/01/2017 | YRS-AT-3460 | Flag to determine payoff amount is allowed on screen to save
        Try
            ClearYRSAndWebLoanTextBoxes() ' VC | 2018.08.013 | YRS-AT-4018 | Calling method to clear text boxes
            isPayOffAmountCanBeSaved = False ' PPP | 06/01/2017 | YRS-AT-3460 | Defaulting it to false

            l_string_PersId = Session("PersId")
            'START : VC | 2018.08.01 | YRS-AT-4018 | Commented existing code and passing new parameter to existing method to list both YRS and WEB loans
            'l_dataset = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpLoanRequests(l_string_PersId)
            l_dataset = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpLoanRequests(l_string_PersId, LoanLocation.WEB)
            'END : VC | 2018.08.01 | YRS-AT-4018 | Commented existing code and passing new parameter to existing method to list both YRS and WEB loans


            '*********
            'anita gemini 2507
            Session("l_dataset") = l_dataset
            'anita gemini 2507
            Me.TabStripParticipantsInformation.Items(7).Enabled = False
            If Not l_dataset Is Nothing Then
                If Not l_dataset.Tables(0) Is Nothing Then
                    If l_dataset.Tables(0).Rows.Count > 0 Then
                        'Aparna -YREN -3024
                        Me.TabStripParticipantsInformation.Items(7).Enabled = True
                        'Aparna -YREN -3024
                        'Aparna Nov 7th 2006-to  show the Request status as in the datagridloans
                        'START : VC | 2018.10.24 | YRS-AT-4018 -  Commented existing code and added new code to set loan status
                        'Me.TextboxLoanStatus.Text = l_dataset.Tables(0).Rows(0)("RequestStatus").ToString
                        loanStatus = l_dataset.Tables(0).Rows(0)("RequestStatus").ToString
                        disbStatus = l_dataset.Tables(0).Rows(0)("DisbursementEFTStatus").ToString
                        SetDISBStatus(loanStatus, disbStatus)
                        'END : VC | 2018.10.24 | YRS-AT-4018 -  Commented existing code and added new code to set loan status
                        'Aparna Nov 7th 2006


                        If Not l_dataset.Tables(0) Is Nothing Then
                            If l_dataset.Tables(0).Rows.Count > 0 Then
                                Me.DataGridLoans.DataSource = l_dataset.Tables(0)
                                Me.DataGridLoans.DataBind()
                            End If

                            'by Aparna 17/01/2007

                            'Commented by Swopna on 11 Jan,2008(Reference:Email From-Hafiz,dated 10 Jan,2008,Subject-Maintenance Person - Loan Tab - Code changes)
                            '*********
                            'If Me.TextboxPayoffAmount.Text = "" Then
                            '    Me.ButtonSavePayoffAmount.Enabled = False
                            'Else
                            '    If Me.TextboxPayoffAmount.Text = "0" Then
                            '        Me.ButtonSavePayoffAmount.Enabled = False
                            '    Else
                            '        Me.ButtonSavePayoffAmount.Enabled = True
                            '    End If
                            'End If
                            '*********

                            'by Aparna 17/01/2007
                            'anita gemini 2507 to bring first row of datagridloans selected on loans tab

                            'START : VC | 2018.09.05 | YRS-AT-4018 -  Added code to select grid item if redirected from LAC
                            If (Session("LoanOriginationId") IsNot Nothing) Then
                                Dim rowIndex As Integer
                                Dim buttonSelect As ImageButton
                                For Each row As DataGridItem In DataGridLoans.Items
                                    rowIndex = row.ItemIndex
                                    buttonSelect = row.FindControl("Imagebutton3")
                                    If row.Cells(4).Text().ToString = Session("LoanOriginationId") Then
                                        DataGridLoans.SelectedIndex = rowIndex
                                        buttonSelect.ImageUrl = "images\selected.gif"
                                        Exit For
                                    Else
                                        buttonSelect.ImageUrl = "images\select.gif"
                                    End If
                                Next
                                Session("LoanOriginationId") = Nothing
                            End If
                            'END : VC | 2018.09.05 | YRS-AT-4018 -  Added code to select grid item if redirected from LAC
                            If DataGridLoans.SelectedIndex = -1 Then
                                'START : VC | 2018.08.02 | YRS-AT-4018 -  Commented existing code and added Code to display YRS or WEB loan details on first laod
                                'LoadMaxRequestDateLoanData()
                                If DataGridLoans.SelectedIndex = -1 Then
                                    DataGridLoans.SelectedIndex = 0
                                End If
                                application = CType(l_dataset.Tables(0).Rows(0)("Application"), String)
                                loanNumber = CType(l_dataset.Tables(0).Rows(0)("LoanNumber"), Integer)
                                webLoanStatus = CType(l_dataset.Tables(0).Rows(0)("RequestStatus"), String)
                                LoadLoansDetails(application, loanNumber, webLoanStatus)
                                'END : VC | 2018.08.02 | YRS-AT-4018 -  Code to display YRS or WEB loan details on first laod

                                'Shubhrata Jan 8th 2006 to populate savedpayoff amount and compute on
                                l_datarow = l_dataset.Tables(0).Rows(0)
                            ElseIf DataGridLoans.SelectedIndex >= 0 Then
                                'START : VC | 2018.08.08 | YRS-AT-4018 - Added code to display YRS or WEB loan details if selected index is not first one
                                'Me.TextboxLoanStatus.Text = l_dataset.Tables(0).Rows(DataGridLoans.SelectedIndex)("RequestStatus").ToString
                                loanStatus = l_dataset.Tables(0).Rows(DataGridLoans.SelectedIndex)("RequestStatus").ToString
                                disbStatus = l_dataset.Tables(0).Rows(DataGridLoans.SelectedIndex)("DisbursementEFTStatus").ToString
                                SetDISBStatus(loanStatus, disbStatus)
                                l_datarow = l_dataset.Tables(0).Rows(DataGridLoans.SelectedIndex)
                                application = CType(l_dataset.Tables(0).Rows(DataGridLoans.SelectedIndex)("Application"), String)
                                loanNumber = CType(l_dataset.Tables(0).Rows(DataGridLoans.SelectedIndex)("LoanNumber"), Integer)
                                webLoanStatus = CType(l_dataset.Tables(0).Rows(DataGridLoans.SelectedIndex)("RequestStatus"), String)
                                LoadLoansDetails(application, loanNumber, webLoanStatus)
                                'END : VC | 2018.08.08 | YRS-AT-4018 -  Code to display YRS or WEB loan details if selected index is not first one
                            End If
                            If application = LoanLocation.YRS Then 'VC | 2018.08.08 | YRS-AT-4018 -  Checking whether the application is YRS or not
                            'Added by Swopna on 11 Jan,2008(Reference:Email From-Hafiz,dated 10 Jan,2008,Subject-Maintenance Person - Loan Tab - Code changes)
                            '*********
                            'Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a to fill the phantom interest details
                            If Me.TextboxPayoffAmount.Text.Trim = "" Then
                                Me.ButtonSavePayoffAmount.Enabled = False
                            Else
                                'Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a to fill the phantom interest details
                                If Convert.ToDecimal(Me.TextboxPayoffAmount.Text) > 0 Then
                                    If TextboxLoanStatus.Text.ToLower = "paid" OrElse TextboxLoanStatus.Text.ToLower = "unspnd" Then
                                        Me.ButtonSavePayoffAmount.Enabled = True
                                        isPayOffAmountCanBeSaved = True ' PPP | 06/01/2017 | YRS-AT-3460 | Payoff amount is allowed to be set / edit
                                    Else
                                        Me.ButtonSavePayoffAmount.Enabled = False
                                    End If
                                Else
                                    Me.ButtonSavePayoffAmount.Enabled = False
                                End If
                            End If
                            '*********

                                If TextboxLoanStatus.Text.ToLower = "default" Then 'VC | 2018.10.11 | YRS-AT-4018 | Changed defalt into default
                                tblFreeze.Visible = True
                                tblPayOff.Visible = False
                            Else
                                tblFreeze.Visible = False
                                tblPayOff.Visible = True
                            End If
                            'End: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a to fill the phantom interest details

                            'anita gemini 2507 to bring first row of datagridloans selected on loans tab

                            If Not IsDBNull(l_datarow("SavedPayOffAmount")) Then
                                Me.TextboxSavedPayoffAmount.Text = Math.Round(l_datarow("SavedPayOffAmount"), 2)
                            End If
                            If Not IsDBNull(l_datarow("ComputeOn")) Then
                                Me.TextboxComputeOn.Text = l_datarow("ComputeOn")
                            End If

                                SetONDType(l_datarow, LoanLocation.YRS) ' VC | 2018.09.24 | YRS-AT-4018 -  Calling method to set ONDType

                            'Shubhrata Jan 8th 2006 to populate savedpayoff amount and compute on
                            'Aparna Yren-3024
                                'START: 'VC | 2018.09.05 | YRS-AT-4018 - To disable save button in YRS loan details section if application value is not YRS
                        Else
                                'If the applcation value is not YRS then save payoff amount button will be disabled
                                Me.ButtonSavePayoffAmount.Enabled = False
                            End If
                            'END: 'VC | 2018.09.05 | YRS-AT-4018 - To disable save button in YRS loan details section if application value is not YRS
                        Else
                            Me.TabStripParticipantsInformation.Items(7).Enabled = False
                            'Aparna Yren-3024
                        End If

                    End If
                End If
            End If

            'START: PPP | 06/01/2017 | YRS-AT-3460 | If payoff amount can be set / edit then decide which message to display on warning popup screen
            If isPayOffAmountCanBeSaved Then
                If TextboxSavedPayoffAmount.Text.Trim() <> "" AndAlso TextboxSavedPayoffAmount.Text.Trim() <> "0" Then
                    'Payoff amount is exists, user could try to save new payoff amount So show message accordingly
                    divPayOffConfirmationMessage.InnerText = Resources.ParticipantsInformation.MESSAGE_LOAN_PAYOFFAMOUNT_EXISTING_WARNING
                Else
                    'Payoff amount is not yer saved 
                    divPayOffConfirmationMessage.InnerText = Resources.ParticipantsInformation.MESSAGE_LOAN_PAYOFFAMOUNT_NEW_WARNING
                End If
            End If
            'END: PPP | 06/01/2017 | YRS-AT-3460 | If payoff amount can be set / edit then decide which message to display on warning popup screen
        Catch ex As Exception
            Throw
        End Try
    End Function
    'anita gemini 2507 to bring first row of datagridloans selected on loans tab
    Private Function LoadMaxRequestDateLoanData()
        Dim l_integer_LoanRequestId As Integer
        Dim l_dataset_Loan As DataSet
        Dim l_dataset As DataSet
        l_dataset = Session("l_dataset")
        Try
            ' l_integer_LoanRequestId = CType(E.Item.Cells(8).Text, Integer)

            If Not l_dataset.Tables(0) Is Nothing Then

                If l_dataset.Tables(0).Rows.Count > 0 Then
                    'START: VC | 2018.09.05 | YRS-AT-4018 - Commented exiting code and added code to select loan number of selected grid item
                    'DataGridLoans.SelectedIndex = 0
                    'l_integer_LoanRequestId = CType(l_dataset.Tables(0).Rows(0)("LoanRequestId"), Integer)
                    If DataGridLoans.SelectedIndex = -1 Then
                    DataGridLoans.SelectedIndex = 0
                    End If
                    l_integer_LoanRequestId = CType(l_dataset.Tables(0).Rows(DataGridLoans.SelectedIndex)("LoanRequestId"), Integer)
                    'END: VC | 2018.09.05 | YRS-AT-4018 - Commented exiting code and added code to select loan number of selected grid item
                    Session("LoanRequestIdForPayOff") = l_integer_LoanRequestId
                End If
            End If
            l_dataset_Loan = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpLoanDetails(l_integer_LoanRequestId)
            If Not l_dataset_Loan Is Nothing Then
                Me.LoadLoansAccountBreakdownDataGrid(l_dataset_Loan)
            End If

            If Not l_dataset_Loan Is Nothing Then
                If Not l_dataset_Loan.Tables("LoanAmortization") Is Nothing Then
                    If l_dataset_Loan.Tables("LoanAmortization").Rows.Count > 0 Then
                        Me.DataGridPaymentHistory.DataSource = l_dataset_Loan.Tables("LoanAmortization")
                        Me.DataGridPaymentHistory.DataBind()
                        lblMessage.Visible = False

                    End If
                Else
                    lblMessage.Visible = True
                End If

                If Not l_dataset_Loan.Tables("LoanDetails") Is Nothing Then
                    If l_dataset_Loan.Tables("LoanDetails").Rows.Count > 0 Then
                        Dim l_datarow As DataRow
                        l_datarow = l_dataset_Loan.Tables("LoanDetails").Rows(0)
                        If Not IsDBNull(l_datarow("Number")) Then
                            Me.TextboxLoanNumber.Text = l_datarow("Number")
                            Session("LoanNumber") = l_datarow("Number")
                        End If
                        If Not IsDBNull(l_datarow("Amount")) Then
                            'By Aparna -17/01/2007 Rounding off to 2 decimals
                            'Me.TextboxLoanAmount.Text =l_datarow("Amount")
                            Me.TextboxLoanAmount.Text = Math.Round(l_datarow("Amount"), 2)
                            Me.ButtonLoanReport.Enabled = True
                        Else
                            Me.ButtonLoanReport.Enabled = True
                        End If
                        If Not IsDBNull(l_datarow("InterestRatePercent")) Then
                            Me.TextboxInterestRate.Text = l_datarow("InterestRatePercent")
                        End If
                        If Not IsDBNull(l_datarow("PaymentAmount")) Then
                            'By Aparna -17/01/2007 Rounding off to 2 decimals
                            ' Me.TextboxPaymentAmount.Text = l_datarow("PaymentAmount")
                            Me.TextboxPaymentAmount.Text = Math.Round(l_datarow("PaymentAmount"), 2)
                        End If

                        ' Me.TextboxPayoffAmount.Text = l_datarow("")
                        If Not IsDBNull(l_datarow("PayrollFrequency")) Then
                            Me.TextboxPayrollFrequency.Text = l_datarow("PayrollFrequency")
                        End If
                        If Not IsDBNull(l_datarow("OriginationFeeAmount")) Then
                            'By Aparna -17/01/2007 Rounding off to 2 decimals
                            '  Me.TextboxLoanOriginationFee.Text = l_datarow("OriginationFeeAmount")
                            Me.TextboxLoanOriginationFee.Text = Math.Round(l_datarow("OriginationFeeAmount"), 2)
                        End If
                        If Not IsDBNull(l_datarow("Status")) Then
                            'YREN 2544
                            'If CType(l_datarow("Status"), String).ToUpper = "D" Then
                            '    Me.TextboxLoanStatus.Text = "Disbursed"
                            'End If
                            'If CType(l_datarow("Status"), String).ToUpper = "C" Then
                            '    Me.TextboxLoanStatus.Text = "Cancel"
                            'End If
                            'If CType(l_datarow("Status"), String).ToUpper = "P" Then
                            '    Me.TextboxLoanStatus.Text = "Pending"
                            'End If
                            'Commented by Aparna Nov 7th 2006-to  show the Request stauts as in the datagridloans
                            ' Me.TextboxLoanStatus.Text = l_datarow("Status")
                            'YREN 2544
                        End If
                        If Not IsDBNull(l_datarow("Term")) Then
                            Me.TextboxLoanTerm.Text = l_datarow("Term")
                        End If
                        If Not IsDBNull(l_datarow("ApprovedDate")) Then
                            Me.TextboxLoanApprovedDate.Text = l_datarow("ApprovedDate")
                        End If
                        If Not IsDBNull(l_datarow("SpousalSignatureReceivedDate")) Then
                            Me.TextboxSpousalConsentReceivedDate.Text = l_datarow("SpousalSignatureReceivedDate")
                        End If
                        If Not IsDBNull(l_datarow("FinalPaymentDate")) Then
                            Me.TextboxFinalPaymentDate.Text = l_datarow("FinalPaymentDate")
                        End If
                        If Not IsDBNull(l_datarow("FirstPaymentDate")) Then
                            Me.TextboxFirstPaymentDate.Text = l_datarow("FirstPaymentDate")
                        End If
                        'Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added to update freeze date and amount
                        If Not IsDBNull(l_datarow("LoandetailsId")) Then
                            Me.Session("LoandetailsId") = l_datarow("LoandetailsId")
                        End If
                        'END: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added to update freeze date and amount

                        'START : VC | 2018.08.06 | YRS-AT-4018 -  Code to load DefaultDate details
                        If Not IsDBNull(l_datarow("dtmDefaultDate")) Then
                            Me.txtDefaultedLoanDate.Text = l_datarow("dtmDefaultDate")
                        Else
                            Me.txtDefaultedLoanDate.Text = String.Empty
                    End If
                        'END : VC | 2018.08.06 | YRS-AT-4018 -  Code to load DefaultDate details
                        'START : SC | 2020.04.20 | YRS-AT-4853 - Display Loantype for YRS LOAN SECTION
                        If Not IsDBNull(l_datarow("LoanType")) Then
                            Me.txtLoanType.Text = l_datarow("LoanType")
                        End If
                        'END : SC | 2020.04.20 | YRS-AT-4853 - Display Loantype for YRS LOAN SECTION
                End If
                End If

                If Not l_dataset_Loan.Tables("LoanPayoffAmount") Is Nothing Then
                    If l_dataset_Loan.Tables("LoanPayoffAmount").Rows.Count > 0 Then
                        If Not IsDBNull(l_dataset_Loan.Tables("LoanPayoffAmount").Rows(0).Item("PayOffAmount")) Then
                            'by Aparna -17/01/2007 Rounding off to 2 decimal places
                            Me.TextboxPayoffAmount.Text = Math.Round(l_dataset_Loan.Tables("LoanPayoffAmount").Rows(0).Item("PayOffAmount"), 2)
                        End If
                    End If
                End If
                If Not l_dataset_Loan.Tables("LoanYmcaNo") Is Nothing Then
                    If l_dataset_Loan.Tables("LoanYmcaNo").Rows.Count > 0 Then
                        If Not IsDBNull(l_dataset_Loan.Tables("LoanYmcaNo").Rows(0).Item("YmcaNo")) Then
                            Me.TextboxYmcaNo.Text = l_dataset_Loan.Tables("LoanYmcaNo").Rows(0).Item("YmcaNo")
                        End If
                    End If
                End If

                'Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a to fill the phantom interest details
                LoadFreezedetails(l_dataset_Loan)
                'End: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a to fill the phantom interest details
                'Start: SR:2015.07.23 BT:2699:YRS 5.0-2441 : Added a to get loan offset reason
                LoadLoanOffsetreason(l_dataset_Loan)
                'End: AA:2015.07.23 BT:2699:YRS 5.0-2441 : Added a to get loan offset reason

                'START: MMR | 2018.04.06 | YRS-AT-3935 | Displaying payment method and Bank Details for selected loan number and also history details 
                If HelperFunctions.isNonEmpty(l_dataset_Loan.Tables("LoanPaymentDetails")) Then
                    DisplayPaymentDetailsForSelectedLoanNumber(l_dataset_Loan.Tables("LoanPaymentDetails"))
            End If

                If HelperFunctions.isNonEmpty(l_dataset_Loan.Tables("PaymentMethodDetails")) Then
                    ViewPaymentMethodHistory(l_dataset_Loan.Tables("PaymentMethodDetails"))
                Else
                    ShowHidePaymentMethodHistoryLink(True) 'VC | 2018.11.05 | YRS-AT-4018 - Changed parameter value from 'False' to 'True'
                End If
                'END: MMR | 2018.04.06 | YRS-AT-3935 | Displaying payment method and Bank Details for selected loan number and also history details
            End If
        Catch
            Throw
        End Try
    End Function
    ''anita gemini 2507 to bring first row of datagridloans selected on loans tab
    'Modified By Ashutosh Patil as on 29-Mar-2007
    'Function name changed from GetMinHigherDate to GetMinDate
    Private Function GetMinDate(ByVal paramdatatable As DataTable) As DateTime
        Dim l_Hiredate As DateTime
        Dim l_dataRow As DataRow
        Try
            If paramdatatable.Rows.Count > 0 Then
                l_dataRow = paramdatatable.Rows(0)
                If l_dataRow("HireDate").ToString() <> "" Then
                    l_Hiredate = CType(l_dataRow("HireDate").ToString(), System.DateTime)
                Else
                    l_Hiredate = CType("1/1/1922", System.DateTime)
                End If

                For Each l_dataRow In paramdatatable.Rows
                    If l_Hiredate > l_dataRow("HireDate").ToString() Then
                        'l_Hiredate = CType(l_dataRow("HireDate").ToString(), System.DateTime)
                        If IsDBNull(l_dataRow("HireDate")) = True Then
                            l_Hiredate = CType("1/1/1922", System.DateTime)
                        Else
                            l_Hiredate = CType(l_dataRow("HireDate").ToString(), System.DateTime)
                        End If
                    End If
                Next
            Else
                l_Hiredate = CType("1/1/1922", System.DateTime)
            End If

            GetMinDate = l_Hiredate

        Catch
            Throw
        End Try
    End Function
    Private Function GetMaxDate(ByVal paramdatatable As DataTable, ByVal paramColumnname As String) As DateTime
        Dim l_date As DateTime
        Dim l_dataRow As DataRow
        Try
            If paramdatatable.Rows.Count > 0 Then
                l_dataRow = paramdatatable.Rows(0)
                If l_dataRow("" & paramColumnname & "").ToString() <> "" Then
                    l_date = CType(l_dataRow("" & paramColumnname & "").ToString(), System.DateTime)
                Else
                    l_date = CType("1/1/1922", System.DateTime)
                End If

                For Each l_dataRow In paramdatatable.Rows
                    If l_dataRow("" & paramColumnname & "").ToString() <> String.Empty AndAlso l_date < CType(l_dataRow("" & paramColumnname & "").ToString(), System.DateTime) Then
                        'l_date = CType(l_dataRow("HireDate").ToString(), System.DateTime)
                        If IsDBNull(l_dataRow("" & paramColumnname & "")) = True Then
                            l_date = CType("1/1/1922", System.DateTime)
                        Else
                            l_date = CType(l_dataRow("" & paramColumnname & "").ToString(), System.DateTime)
                        End If
                    End If
                Next
            Else
                l_date = CType("1/1/1922", System.DateTime)
            End If

            GetMaxDate = l_date

        Catch
            Throw
        End Try
    End Function
    Private Function GetDate(ByVal paramdatatable As DataTable, ByVal paramstrColumn As String) As DateTime
        'Ashutosh Patil as on 30-Mar-2007
        'YREN-3203,YREN-3205
        Dim l_date As DateTime
        Dim l_dataRow As DataRow
        Try
            If paramdatatable.Rows.Count > 0 Then
                For Each l_dataRow In paramdatatable.Rows
                    If IsDBNull(l_dataRow("TermDate")) = True Then
                        If IsDBNull(l_dataRow("" & paramstrColumn & "")) = True Then
                            If paramstrColumn = "EnrollmentDate" Then
                                l_date = Session("HireDate")
                            Else
                                l_date = CType("1/1/1922", System.DateTime)
                            End If
                        Else
                            l_date = l_dataRow("" & paramstrColumn & "")
                        End If
                    End If
                Next
            Else
                l_date = CType("1/1/1922", System.DateTime)
            End If
            GetDate = l_date
        Catch
            Throw
        End Try
    End Function
    Private Sub DataGridLoans_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridLoans.ItemCommand
        Dim l_integer_LoanRequestId As Integer
        Dim l_dataset_Loan As DataSet
        Dim l_datarow_CurrentRow As DataRow()
        Dim l_DataSet_LoanRequests As DataSet
        Dim l_datarow As DataRow
        Dim isPayOffAmountCanBeSaved As Boolean ' PPP | 06/01/2017 | YRS-AT-3460 | Flag to determine payoff amount is allowed on screen to save
        'START : VC | 2018.08.06 | YRS-AT-4018 -  Declared variables to store loan number and application
        Dim loanNumber As Integer
        Dim application As String
        'END : VC | 2018.08.06 | YRS-AT-4018 -  Declared variables to store loan number and application
        Try
            isPayOffAmountCanBeSaved = False ' PPP | 06/01/2017 | YRS-AT-3460 | Defaulting it to false
            'START: MMR | 2018.04.06 | YRS-AT-3935 | Commented existing code and setting constant value instead of cell index value
            'l_integer_LoanRequestId = CType(e.Item.Cells(9).Text, Integer)

            application = CType(e.Item.Cells(mConst_DataGridLoansIndexOfApplication).Text, String) 'VC | 2018.08.06 | YRS-AT-4018 -  Added code to get selected loan Application vaue
            ClearYRSAndWebLoanTextBoxes() ' VC | 2018.08.013 | YRS-AT-4018 | Calling method to clear text boxes
            If (application = LoanLocation.YRS) Then 'VC | 2018.08.06 | YRS-AT-4018 -  Added condition to check whether it is YRS loan
                l_integer_LoanRequestId = CType(e.Item.Cells(mConst_DataGridLoansIndexOfLoanRequestId).Text, Integer)
                'END: MMR | 2018.04.06 | YRS-AT-3935 |Commented existing code and setting constant value instead of cell index value
            Session("LoanRequestIdForPayOff") = l_integer_LoanRequestId
            l_dataset_Loan = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpLoanDetails(l_integer_LoanRequestId)
            'Ymca Phase 4
            If Not l_dataset_Loan Is Nothing Then
                Me.LoadLoansAccountBreakdownDataGrid(l_dataset_Loan)
            End If
            'Ymca Phase 4
            'aparna yren-3024
            l_DataSet_LoanRequests = Session("l_dataset")
            If Not l_DataSet_LoanRequests Is Nothing Then
                If l_DataSet_LoanRequests.Tables(0).Rows.Count > 0 Then
                    l_datarow_CurrentRow = l_DataSet_LoanRequests.Tables(0).Select("LoanRequestId = '" & l_integer_LoanRequestId & "'")
                    l_datarow = l_datarow_CurrentRow(0)
                        SetONDType(l_datarow, LoanLocation.YRS) ' VC | 2018.09.24 | YRS-AT-4018 -  Calling method to set ONDType
                    If Not IsDBNull(l_datarow("RequestStatus")) Then
                            'START : VC | 2018.10.24 | YRS-AT-4018 -  Commented existing code and added new code to set loan status
                            'Me.TextboxLoanStatus.Text = l_datarow("RequestStatus").ToString
                            SetDISBStatus(l_datarow("RequestStatus").ToString, l_datarow("DisbursementEFTStatus").ToString)
                            'END : VC | 2018.10.24 | YRS-AT-4018 -  Commented existing code and added new code to set loan status
                    Else
                        Me.TextboxLoanStatus.Text = ""
                    End If

                    If Not IsDBNull(l_datarow("SavedPayOffAmount")) Then
                        Me.TextboxSavedPayoffAmount.Text = Math.Round(l_datarow("SavedPayOffAmount"), 2)
                    Else
                        Me.TextboxSavedPayoffAmount.Text = ""
                    End If
                    If Not IsDBNull(l_datarow("ComputeOn")) Then
                        Me.TextboxComputeOn.Text = l_datarow("ComputeOn")
                    Else
                        Me.TextboxComputeOn.Text = ""
                    End If
                End If
            End If

            'aparna yren-3024
            If Not l_dataset_Loan Is Nothing Then
                If Not l_dataset_Loan.Tables("LoanAmortization") Is Nothing Then
                    If l_dataset_Loan.Tables("LoanAmortization").Rows.Count > 0 Then
                        Me.DataGridPaymentHistory.DataSource = l_dataset_Loan.Tables("LoanAmortization")
                        Me.DataGridPaymentHistory.DataBind()
                        Me.ButtonLoanReport.Enabled = True
                        lblMessage.Visible = False
                    End If
                Else
                    Me.DataGridPaymentHistory.DataSource = Nothing
                    Me.DataGridPaymentHistory.DataBind()
                    Me.ButtonLoanReport.Enabled = False
                    lblMessage.Visible = True
                End If

                If Not l_dataset_Loan.Tables("LoanDetails") Is Nothing Then
                    If l_dataset_Loan.Tables("LoanDetails").Rows.Count > 0 Then
                        l_datarow = l_dataset_Loan.Tables("LoanDetails").Rows(0)
                        If Not IsDBNull(l_datarow("Number")) Then
                            Me.TextboxLoanNumber.Text = l_datarow("Number")
                            Session("LoanNumber") = l_datarow("Number")
                        End If
                        If Not IsDBNull(l_datarow("Amount")) Then
                            'By Aparna 17/01/2007 Rounding off to two decimals
                            Me.TextboxLoanAmount.Text = Math.Round(l_datarow("Amount"), 2)
                        End If
                        If Not IsDBNull(l_datarow("InterestRatePercent")) Then
                            Me.TextboxInterestRate.Text = l_datarow("InterestRatePercent")
                        End If
                        If Not IsDBNull(l_datarow("PaymentAmount")) Then
                            'By Aparna 17/01/2007 Rounding off to two decimals
                            Me.TextboxPaymentAmount.Text = Math.Round(l_datarow("PaymentAmount"), 2)
                        End If

                        ' Me.TextboxPayoffAmount.Text = l_datarow("")
                        If Not IsDBNull(l_datarow("PayrollFrequency")) Then
                            Me.TextboxPayrollFrequency.Text = l_datarow("PayrollFrequency")
                        End If
                        If Not IsDBNull(l_datarow("OriginationFeeAmount")) Then
                            'By Aparna 17/01/2007 Rounding off to two decimals
                            Me.TextboxLoanOriginationFee.Text = Math.Round(l_datarow("OriginationFeeAmount"), 2)
                        End If
                        'If Not IsDBNull(l_datarow("Status")) Then
                        '    If CType(l_datarow("Status"), String).ToUpper = "D" Then
                        '        Me.TextboxLoanStatus.Text = "Disbursed"
                        '    End If
                        '    If CType(l_datarow("Status"), String).ToUpper = "C" Then
                        '        Me.TextboxLoanStatus.Text = "Cancel"
                        '    End If
                        '    If CType(l_datarow("Status"), String).ToUpper = "P" Then
                        '        Me.TextboxLoanStatus.Text = "Pending"
                        '    End If
                        'End If
                        If Not IsDBNull(l_datarow("Term")) Then
                            Me.TextboxLoanTerm.Text = l_datarow("Term")
                        End If
                        If Not IsDBNull(l_datarow("ApprovedDate")) Then
                            Me.TextboxLoanApprovedDate.Text = l_datarow("ApprovedDate")
                        End If
                        If Not IsDBNull(l_datarow("SpousalSignatureReceivedDate")) Then
                            Me.TextboxSpousalConsentReceivedDate.Text = l_datarow("SpousalSignatureReceivedDate")
                        End If
                        If Not IsDBNull(l_datarow("FinalPaymentDate")) Then
                            Me.TextboxFinalPaymentDate.Text = l_datarow("FinalPaymentDate")
                        End If
                        If Not IsDBNull(l_datarow("FirstPaymentDate")) Then
                            Me.TextboxFirstPaymentDate.Text = l_datarow("FirstPaymentDate")
                        End If
                        'Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a to get loan detailsid to store the freeze value
                        If Not IsDBNull(l_datarow("LoandetailsId")) Then
                            Me.Session("LoandetailsId") = l_datarow("LoandetailsId")
                        End If
                        'End: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a to get loan detailsid to store the freeze value
                            'START : SC | 2020.04.20 | YRS-AT-4853 - Display Loantype for YRS LOAN SECTION
                            If Not IsDBNull(l_datarow("LoanType")) Then
                                Me.txtLoanType.Text = l_datarow("LoanType")
                            End If
                            'END : SC | 2020.04.20 | YRS-AT-4853 - Display Loantype for YRS LOAN SECTION
                            'START : VC | 2018.08.06 | YRS-AT-4018 -  Code to load Default date details while selecting loans grid item
                            If Not IsDBNull(l_datarow("dtmDefaultDate")) Then
                                Me.txtDefaultedLoanDate.Text = l_datarow("dtmDefaultDate")
                    Else
                                Me.txtDefaultedLoanDate.Text = String.Empty
                            End If
                            'END : VC | 2018.08.06 | YRS-AT-4018 -  Code to load Default date details while selecting loans grid item
                        Else
                        'aparna -yren-3024
                        Me.TextboxLoanNumber.Text = ""
                        Me.TextboxLoanAmount.Text = ""
                        Me.TextboxInterestRate.Text = ""
                        Me.TextboxPaymentAmount.Text = ""
                        Me.TextboxPayrollFrequency.Text = ""
                        Me.TextboxLoanStatus.Text = ""
                        Me.TextboxLoanTerm.Text = ""
                        Me.TextboxLoanOriginationFee.Text = ""
                        Me.TextboxLoanApprovedDate.Text = ""
                        Me.TextboxSpousalConsentReceivedDate.Text = ""
                        Me.TextboxFinalPaymentDate.Text = ""
                            Me.TextboxFirstPaymentDate.Text = ""
                            Me.txtLoanType.Text = "" ' SC | 2020.04.20 | YRS-AT-4853 - Display Loantype for YRS LOAN SECTION
                        'aparna -yren-3024
                    End If

                End If

                If Not l_dataset_Loan.Tables("LoanPayoffAmount") Is Nothing Then
                    If l_dataset_Loan.Tables("LoanPayoffAmount").Rows.Count > 0 Then
                        If Not IsDBNull(l_dataset_Loan.Tables("LoanPayoffAmount").Rows(0).Item("PayOffAmount")) Then
                            'By Aparna 17/01/2007 Rounding off to two decimals
                            Me.TextboxPayoffAmount.Text = Math.Round(l_dataset_Loan.Tables("LoanPayoffAmount").Rows(0).Item("PayOffAmount"), 2)
                        End If
                    End If
                End If
                'YRPS-4558
                If Not l_dataset_Loan.Tables("LoanYmcaNo") Is Nothing Then
                    If l_dataset_Loan.Tables("LoanYmcaNo").Rows.Count > 0 Then
                        If Not IsDBNull(l_dataset_Loan.Tables("LoanYmcaNo").Rows(0).Item("YmcaNo")) Then
                            Me.TextboxYmcaNo.Text = l_dataset_Loan.Tables("LoanYmcaNo").Rows(0).Item("YmcaNo")
                        End If
                    End If
                End If
            End If
            'by Aparna 17/01/2007
            'Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Modified a to fill the phantom interest details & update freeze date and amount
            If Me.TextboxPayoffAmount.Text.Trim = "" Then
                Me.ButtonSavePayoffAmount.Enabled = False
            Else
                If Convert.ToDecimal(Me.TextboxPayoffAmount.Text) > 0 Then
                    If TextboxLoanStatus.Text.ToLower = "paid" OrElse TextboxLoanStatus.Text.ToLower = "unspnd" Then
                        Me.ButtonSavePayoffAmount.Enabled = True
                        isPayOffAmountCanBeSaved = True ' PPP | 06/01/2017 | YRS-AT-3460 | Payoff amount is allowed to be set / edit
                    Else
                        Me.ButtonSavePayoffAmount.Enabled = False
                    End If
                Else
                    Me.ButtonSavePayoffAmount.Enabled = False
                End If
            End If
            'by Aparna 17/01/2007


            LoadFreezedetails(l_dataset_Loan)

                If TextboxLoanStatus.Text.ToLower = "default" Then 'VC | 2018.10.11 | YRS-AT-4018 | Changed defalt into default
                tblFreeze.Visible = True
                tblPayOff.Visible = False
            Else
                tblFreeze.Visible = False
                tblPayOff.Visible = True
            End If
            'End: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a to fill the phantom interest details & update freeze date and amount

            ''SR:2015.07.13 BT:2699:YRS 5.0-2441 : Added to fill the loan offset reason
            LoadLoanOffsetreason(l_dataset_Loan)
            ''SR:2015.07.13 BT:2699:YRS 5.0-2441 : Added to fill the loan offset reason

            'START: PPP | 06/01/2017 | YRS-AT-3460 | If payoff amount can be set / edit then decide which message to display on warning popup screen
            If isPayOffAmountCanBeSaved Then
                If TextboxSavedPayoffAmount.Text.Trim() <> "" AndAlso TextboxSavedPayoffAmount.Text.Trim() <> "0" Then
                    'Payoff amount is exists, user could try to save new payoff amount So show message accordingly
                    divPayOffConfirmationMessage.InnerText = Resources.ParticipantsInformation.MESSAGE_LOAN_PAYOFFAMOUNT_EXISTING_WARNING
                Else
                    'Payoff amount is not yer saved 
                    divPayOffConfirmationMessage.InnerText = Resources.ParticipantsInformation.MESSAGE_LOAN_PAYOFFAMOUNT_NEW_WARNING
                End If
            End If
            'END: PPP | 06/01/2017 | YRS-AT-3460 | If payoff amount can be set / edit then decide which message to display on warning popup screen
                'START: MMR | 2018.04.06 | YRS-AT-3935 | Displaying payment method and Bank Details for selected loan number and also history details
                If HelperFunctions.isNonEmpty(l_dataset_Loan.Tables("LoanPaymentDetails")) Then
                    DisplayPaymentDetailsForSelectedLoanNumber(l_dataset_Loan.Tables("LoanPaymentDetails"))
                End If

                If HelperFunctions.isNonEmpty(l_dataset_Loan.Tables("PaymentMethodDetails")) Then
                    ViewPaymentMethodHistory(l_dataset_Loan.Tables("PaymentMethodDetails"))
                Else
                    ShowHidePaymentMethodHistoryLink(True) 'VC | 2018.11.05 | YRS-AT-4018 - Changed parameter value from 'False' to 'True'
                End If
                'END: MMR | 2018.04.06 | YRS-AT-3935 | Displaying payment method and Bank Details for selected loan number and also history details

                'START : VC | 2018.08.06 | YRS-AT-4018 -  Code to handle ui controls if it is YRS or WEB loan
                trLoanAccountDetails.Visible = True
                trLoanAccountDetailsBody.Visible = True
                trPaymentHistoryHeader.Visible = True
                trPaymentHistoryBody.Visible = True
                trLoanReport.Visible = True
                'Getting selected loan number
                Dim selectedLoanNumber = e.Item.Cells(mConst_DataGridLoansIndexOfLoanNumber).Text
                If Not (String.IsNullOrEmpty(selectedLoanNumber)) And selectedLoanNumber <> "&nbsp;" Then
                    loanNumber = CType(e.Item.Cells(mConst_DataGridLoansIndexOfLoanNumber).Text, Integer)
                End If

                Dim webLoanStatus As String
                'Getting web loan status to load web loan details
                webLoanStatus = CType(e.Item.Cells(mConst_DataGridLoansIndexOfRequestStatus).Text, String)
                LoadWebLoanDetails(loanNumber, webLoanStatus, application)
                txtWebDocreceivedDate.ReadOnly = True
                popCalendarDate.Enabled = False
                btnApproveWebLoan.Disabled = True
                btnDeclineWebLoan.Disabled = True

            Else

                Dim webLoanStatus As String
                Dim selectedLoanNumber = e.Item.Cells(mConst_DataGridLoansIndexOfLoanNumber).Text
                Session("LoanOriginationId") = Nothing
                ClearYRSAndWebLoanTextBoxes()
                'Getting selected loan number
                If Not (String.IsNullOrEmpty(selectedLoanNumber)) And selectedLoanNumber <> "&nbsp;" Then
                    loanNumber = CType(e.Item.Cells(mConst_DataGridLoansIndexOfLoanNumber).Text, Integer)
                End If
                'Getting web loan status to load web loan details
                webLoanStatus = CType(e.Item.Cells(mConst_DataGridLoansIndexOfRequestStatus).Text, String)
                LoadWebLoanDetails(loanNumber, webLoanStatus, application)
                HandleControlsIfStatusIsPending(webLoanStatus, application)

                trLoanAccountDetails.Visible = False
                trLoanAccountDetailsBody.Visible = False
                trPaymentHistoryHeader.Visible = False
                trPaymentHistoryBody.Visible = False
                trLoanReport.Visible = False
                'Set payment method details grid as empty
                gvPaymentMethodHistory.DataSource = Nothing
                gvPaymentMethodHistory.DataBind()
                'END : VC | 2018.08.06 | YRS-AT-4018 -  Code to handle ui controls if it is YRS or WEB loan
            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub DataGridLoans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridLoans.ItemDataBound
        Try
            'By Aparna 17/01/2007
            Dim l_button_Select As ImageButton
            l_button_Select = e.Item.FindControl("ImageButton3")
            'If (e.Item.ItemIndex = Me.DataGridLoans.SelectedIndex And Me.DataGridLoans.SelectedIndex >= 0) Then
            '    l_button_Select.ImageUrl = "images\selected.gif"
            'End If
            If e.Item.ItemIndex = 0 Then
                l_button_Select.ImageUrl = "images\selected.gif"
            End If
            'By Aparna 17/01/2007
            'Shubhrata Dec 28th 2006  - Made the columns Bound in data grid
            'e.Item.Cells(1).Visible = False
            'e.Item.Cells(2).Visible = False
            'e.Item.Cells(3).Visible = False
            'e.Item.Cells(9).Visible = False
            ''Shubhrata Oct 13th 2006
            'e.Item.Cells(4).Visible = False
            'Aparna Nov 7th 2006-to include OriginalLoanNumber in the grid
            ' e.Item.Cells(10).Visible = False
            'Shubhrata Oct 13th 2006            
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'Private Sub LinkRefereshIDM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkRefereshIDM.Click
    '    IframeIDMView.Attributes.Add("src", "ErrorPageForm.aspx")
    'End Sub

    Private Sub LinkButtonIDM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonIDM.Click
        Try
            'Dim l_string_VignettePath As String
            Dim l_string_FundNo As String
            'By Aparna Samala -02/01/2007-To remove the session Session("VignettePath")
            'l_string_VignettePath = Session("VignettePath")
            ' l_string_VignettePath = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + "yrsResultsForm.aspx?fundid="
            '14-Nov-2008 Priya Change IDM path as per Ragesh mail as on 11/13/2008
            l_string_VignettePath = System.Configuration.ConfigurationSettings.AppSettings("Vignette_Path") + String.Format(System.Configuration.ConfigurationSettings.AppSettings("Vignette_Page_Participant"), Session("FundNo"))
            'End 14-Nov-2008
            l_string_FundNo = Session("FundNo")
            'Modified by Mohammed Hafiz on 19-Dec-2008
            Dim popupScript As String = "<script language='javascript'>" & _
             "window.open('" + l_string_VignettePath + "', 'IDMPopUp', " & _
             "'width=1000, height=750, menubar=no, Resizable=Yes,top=0,left=0, scrollbars=yes')" & _
             "</script>"


            Page.RegisterStartupScript("IDMPopUpScript", popupScript)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    '#Region "DropdownCode"
    '	'Sub Address_DropDownListState()
    '	'    Dim dataset_States As DataSet
    '	'    Dim datarow_state As DataRow()
    '	'    Dim strCountryCode As String
    '	'    strCountryCode = String.Empty
    '	'    If Session("States") Is Nothing Or Page.IsPostBack = False Then
    '	'        dataset_States = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()
    '	'        Me.DropdownlistState.DataSource = dataset_States.Tables(0)
    '	'        Me.DropdownlistState.DataTextField = "chvDescription"
    '	'        Me.DropdownlistState.DataValueField = "chvcodevalue"
    '	'        Me.DropdownlistState.DataBind()
    '	'        Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '	'        Me.DropdownlistState.Items(0).Value = ""
    '	'        Me.DropdownlistSecState.DataSource = dataset_States.Tables(0)
    '	'        Me.DropdownlistSecState.DataMember = "State Type"
    '	'        Me.DropdownlistSecState.DataTextField = "chvDescription"
    '	'        Me.DropdownlistSecState.DataValueField = "chvcodevalue"
    '	'        Me.DropdownlistSecState.DataBind()
    '	'        Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '	'        Me.DropdownlistSecState.Items(0).Value = ""
    '	'        Session("States") = dataset_States
    '	'    Else
    '	'        dataset_States = CType(Session("States"), DataSet)
    '	'        'Added By Ashutosh Patil as on 14-Feb-2007
    '	'        'To avoid error for arguments related
    '	'        Me.DropdownlistState.DataSource = dataset_States.Tables(0)
    '	'        Me.DropdownlistState.DataTextField = "chvDescription"
    '	'        Me.DropdownlistState.DataValueField = "chvcodevalue"
    '	'        Me.DropdownlistState.DataBind()
    '	'    End If
    '	'    If dataset_States.Tables(0).Rows.Count > 0 Then
    '	'        If Not ViewState("SelectedState") Is Nothing Then
    '	'            Try
    '	'                Me.DropdownlistState.SelectedValue = CType(ViewState("SelectedState"), String)
    '	'            Catch

    '	'            End Try

    '	'            datarow_state = dataset_States.Tables(0).Select("chvcodevalue='" & DropdownlistState.SelectedValue.Trim() & "' ")

    '	'            If datarow_state.Length > 0 Then
    '	'                strCountryCode = datarow_state(0)("chvCountryCode")
    '	'            End If

    '	'            'Commented By Ashutosh Patil as on 14-Feb-2007
    '	'            'YREN - 3028,3029
    '	'            ''If strCountryCode.Trim().Length = 0 Then
    '	'            ''    strCountryCode = Me.DropdownlistCountry.SelectedValue
    '	'            ''End If

    '	'            Address_DropDownCountry(strCountryCode, "1")
    '	'        End If
    '	'        If Not ViewState("SelectedSecState") Is Nothing Then
    '	'            Try
    '	'                Me.DropdownlistSecState.SelectedValue = CType(ViewState("SelectedSecState"), String)
    '	'            Catch

    '	'            End Try

    '	'            strCountryCode = String.Empty

    '	'            datarow_state = dataset_States.Tables(0).Select("chvcodevalue='" & DropdownlistSecState.SelectedValue.Trim() & "' ")
    '	'            If datarow_state.Length > 0 Then
    '	'                strCountryCode = datarow_state(0)("chvCountryCode")
    '	'            End If

    '	'            If strCountryCode.Trim().Length = 0 Then
    '	'                'Shubhrata Nov 29th 2006 Reason the Sec Country was not getting picked up from database.
    '	'                'strCountryCode = Me.DropdownlistSecState.SelectedValue
    '	'                strCountryCode = Me.DropdownlistSecCountry.SelectedValue
    '	'                'Shubhrata Nov 29th 2006 Reason the Sec Country was not getting picked up from database.
    '	'            End If

    '	'            Address_DropDownCountry(strCountryCode, "2")
    '	'        End If
    '	'    End If
    '	'End Sub
    '	'Sub Address_DropDownCountry(ByVal strCountryCode As String, ByVal strDdl As String)
    '	'    If strDdl = "1" Then
    '	'        If Not ViewState("SelectedState") Is Nothing Then
    '	'            ' ViewState("SelectedState") = Nothing
    '	'            'By Ashutosh Patil as on 29-Jan-2007 for YREN - 3028,3029
    '	'            'If strCountryCode = "US" Or strCountryCode = "CA" Then
    '	'            'By Ashutosh Patil as on 29-Jan-2007 for YREN - 3028,3029
    '	'            'Commented By Ashutosh Patil as on 12-Feb-2007 For YREN - 3028, YREN - 3029
    '	'            'There will be common filter for all states
    '	'            'If strCountryCode = "US" Or strCountryCode = "CA" Then
    '	'            l_ds_States = DirectCast(Session("States"), DataSet)
    '	'            l_dt_State = l_ds_States.Tables(0)
    '	'            l_dr_State = l_dt_State.Select("chvCountryCode='" & strCountryCode & "' ")
    '	'            l_dt_State_filtered = l_dt_State.Clone()
    '	'            For Each dr In l_dr_State
    '	'                l_dt_State_filtered.ImportRow(dr)
    '	'            Next
    '	'            Me.DropdownlistState.DataSource() = l_dt_State_filtered
    '	'            Me.DropdownlistState.DataBind()
    '	'            Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '	'            Me.DropdownlistState.Items(0).Value = ""
    '	'            'Added and Modified By Ashutosh Patil as on 23-Feb-2007
    '	'            'If ViewState("SelectedState") <> "" Then
    '	'            If Not ViewState("SelectedState") Is Nothing Then
    '	'                Me.DropdownlistState.SelectedValue = CType(ViewState("SelectedState"), String)
    '	'                'Added By Ashutosh Patil as on 14-Feb-2007 For YREN - 3028, YREN - 3029
    '	'                Me.DropdownlistCountry.SelectedValue = strCountryCode
    '	'            End If
    '	'            'Call SetAttributes(Me.TextBoxZip, Me.DropdownlistCountry.SelectedValue.ToString)
    '	'            Exit Sub
    '	'        End If
    '	'    End If
    '	'    If strDdl = "2" Then
    '	'        If Not ViewState("SelectedSecState") Is Nothing Then
    '	'            ' ViewState("SelectedSecState") = Nothing
    '	'            'By Ashutosh Patil as on 29-Jan-2007 for YREN - 3028,3029
    '	'            'By Ashutosh Patil as on 01-Feb-2007
    '	'            'YREN - 3028,YREN - 3029
    '	'            'Commented By Ashutosh Patil as on 12-Feb-2007 For YREN - 3028, YREN - 3029
    '	'            'There will be common filter for all states
    '	'            'If strCountryCode = "US" Or strCountryCode = "CA" Then
    '	'            l_ds_States = DirectCast(Session("States"), DataSet)
    '	'            l_dt_State = l_ds_States.Tables(0)
    '	'            l_dr_State = l_dt_State.Select("chvCountryCode='" & strCountryCode & "' ")
    '	'            l_dt_State_filtered = l_dt_State.Clone()
    '	'            For Each dr In l_dr_State
    '	'                l_dt_State_filtered.ImportRow(dr)
    '	'            Next
    '	'            Me.DropdownlistSecState.DataSource() = l_dt_State_filtered
    '	'            Me.DropdownlistSecState.DataBind()
    '	'            Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '	'            Me.DropdownlistSecState.Items(0).Value = ""
    '	'            'Added and Modified By Ashutosh Patil as on 23-Feb-2007
    '	'            'If ViewState("SelectedSecState") <> "" Then
    '	'            If Not ViewState("SelectedSecState") Is Nothing Then
    '	'                Me.DropdownlistSecState.SelectedValue = CType(ViewState("SelectedSecState"), String)
    '	'                'Added By Ashutosh Patil as on 12-Feb-2007 For YREN - 3028, YREN - 3029
    '	'                Me.DropdownlistSecCountry.SelectedValue = strCountryCode
    '	'            End If

    '	'        Else
    '	'            Me.DropdownlistSecCountry.SelectedValue = ""
    '	'        End If
    '	'    End If
    '	'    'Call SetAttributes(Me.TextBoxSecZip, Me.DropdownlistSecCountry.SelectedValue.ToString)

    '	'End Sub
    '	'Sub Address_DropDownListStateSecondary()
    '	'    Dim dataset_States As DataSet
    '	'    Dim datarow_state As DataRow()
    '	'    Dim strCountryCode As String = String.Empty
    '	'    Try
    '	'        If Session("States") Is Nothing Or Page.IsPostBack = False Then
    '	'            dataset_States = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()

    '	'            Me.DropdownlistSecState.DataSource = dataset_States.Tables(0)
    '	'            Me.DropdownlistSecState.DataMember = "State Type"
    '	'            Me.DropdownlistSecState.DataTextField = "chvDescription"
    '	'            Me.DropdownlistSecState.DataValueField = "chvcodevalue"
    '	'            Me.DropdownlistSecState.DataBind()
    '	'            Me.DropdownlistSecState.Items.Insert(0, "-Select State-")
    '	'            Me.DropdownlistSecState.Items(0).Value = ""

    '	'            Session("States") = dataset_States
    '	'        Else
    '	'            dataset_States = DirectCast(Session("States"), DataSet)
    '	'            'Added By Ashutosh Patil as on 14-Feb-2007
    '	'            'To avoid error for arguements related
    '	'            Me.DropdownlistSecState.DataSource = dataset_States.Tables(0)
    '	'            Me.DropdownlistSecState.DataMember = "State Type"
    '	'            Me.DropdownlistSecState.DataTextField = "chvDescription"
    '	'            Me.DropdownlistSecState.DataValueField = "chvcodevalue"
    '	'            Me.DropdownlistSecState.DataBind()
    '	'        End If

    '	'        If dataset_States.Tables(0).Rows.Count > 0 Then
    '	'            If Not ViewState("SelectedSecState") Is Nothing Then
    '	'                Me.DropdownlistSecState.SelectedValue = CType(ViewState("SelectedSecState"), String)
    '	'                strCountryCode = String.Empty
    '	'                datarow_state = dataset_States.Tables(0).Select("chvcodevalue='" & DropdownlistSecState.SelectedValue.Trim() & "' ")
    '	'                If datarow_state.Length > 0 Then
    '	'                    strCountryCode = datarow_state(0)("chvCountryCode")
    '	'                End If

    '	'                'If strCountryCode.Trim().Length = 0 Then
    '	'                '    strCountryCode = Me.DropdownlistSecState.SelectedValue
    '	'                'End If

    '	'                Address_DropDownCountry(strCountryCode, "2")
    '	'            End If
    '	'        End If
    '	'    Catch
    '	'        Throw
    '	'    End Try
    '	'End Sub
    '	'Sub Address_DropDownListStatePrimary()
    '	'    Dim dataset_States As DataSet
    '	'    Dim datarow_state As DataRow()
    '	'    Dim strCountryCode As String = String.Empty
    '	'    Try
    '	'        If Session("States") Is Nothing Or Page.IsPostBack = False Then
    '	'            dataset_States = YMCARET.YmcaBusinessObject.YMCAAddressBOClass.GetStates()
    '	'            Me.DropdownlistState.DataSource = dataset_States.Tables(0)
    '	'            Me.DropdownlistState.DataTextField = "chvDescription"
    '	'            Me.DropdownlistState.DataValueField = "chvcodevalue"
    '	'            Me.DropdownlistState.DataBind()
    '	'            Me.DropdownlistState.Items.Insert(0, "-Select State-")
    '	'            Me.DropdownlistState.Items(0).Value = ""
    '	'            Session("States") = dataset_States
    '	'        Else
    '	'            dataset_States = DirectCast(Session("States"), DataSet)
    '	'            'Added By Ashutosh Patil as on 12-Feb-2007
    '	'            'Error occured during change from Activate Secondary to Primary
    '	'            Me.DropdownlistState.DataSource = dataset_States.Tables(0)
    '	'            Me.DropdownlistState.DataTextField = "chvDescription"
    '	'            Me.DropdownlistState.DataValueField = "chvcodevalue"
    '	'            Me.DropdownlistState.DataBind()
    '	'        End If

    '	'        If dataset_States.Tables(0).Rows.Count > 0 Then
    '	'            If Not ViewState("SelectedState") Is Nothing Then
    '	'                Me.DropdownlistState.SelectedValue = CType(ViewState("SelectedState"), String)
    '	'                Me.DropdownlistState.DataSource = dataset_States.Tables(0)
    '	'                'Me.DropdownlistState.DataSource = l_dt_State_filtered
    '	'                datarow_state = dataset_States.Tables(0).Select("chvcodevalue='" & DropdownlistState.SelectedValue.Trim() & "' ")
    '	'                If datarow_state.Length > 0 Then
    '	'                    strCountryCode = datarow_state(0)("chvCountryCode")
    '	'                End If

    '	'                'If strCountryCode.Trim().Length = 0 Then
    '	'                '    strCountryCode = Me.DropdownlistCountry.SelectedValue
    '	'                'End If

    '	'                Address_DropDownCountry(strCountryCode, "1")
    '	'            End If
    '	'        End If
    '	'    Catch
    '	'        Throw
    '	'    End Try
    '	'End Sub


    '#End Region


    'Private Sub DropdownlistState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistState.SelectedIndexChanged
    '    '*******Code Added by ashutosh on 23-June-06
    '    ViewState("SelectedSecState") = Nothing
    '    ViewState("SelectedState") = DropdownlistState.SelectedValue
    '    'Address_DropDownListState()
    '    Address_DropDownListStatePrimary()
    '    '*********
    'End Sub

    'Private Sub DropdownlistSecState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistSecState.SelectedIndexChanged
    '    ViewState("SelectedState") = Nothing
    '    ViewState("SelectedSecState") = DropdownlistSecState.SelectedValue
    '    'Address_DropDownListState()
    '    Address_DropDownListStateSecondary() 'by aparna 03/09/2007
    'End Sub

    'Shubhrata YREN 2543 Jul 25th 2006
    Private Sub ButtonLoanReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonLoanReport.Click

        Try
            Session("strReportName") = "Loan Summary"

            Dim popupScript As String = "<script language='javascript'>" & _
            "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If

        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
    'Shubhrata YREN 2543 Jul 25th 2006 

    Private Sub ButtonEditEmploymentService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditEmploymentService.Click
        Try

            'Added by neeraj on 01-Dec-2009 : issue is YRS 5.0-940 security Check

            Dim checkSecurity1 As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If checkSecurity1.Equals("True") Then
                Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditEmploymentService", Convert.ToInt32(Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If
            Else
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity1, MessageBoxButtons.Stop)
                Exit Sub
            End If

            'End : YRS 5.0-940

            Dim popupScript As String = "<script language='javascript'>" & _
              "window.open('UpdateParticipantEmploymentService.aspx?', 'CustomPopUp_UpdateEmpService', " & _
               "'width=800, height=500, menubar=no, Resizable=no,top=120,left=120, scrollbars=yes')" & _
               "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript)
            End If

        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridPaymentHistory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridPaymentHistory.ItemDataBound

    End Sub


    Private Sub ButtonSavePayoffAmount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSavePayoffAmount.Click
        Try
            Dim l_integer_LoanRequestId As Integer
            l_integer_LoanRequestId = CType(Session("LoanRequestIdForPayOff"), Integer)
            YMCARET.YmcaBusinessObject.LoanInformationBOClass.SavePayOffDetails(l_integer_LoanRequestId, Me.TextboxPayoffAmount.Text)
            Me.LoadLoansTab()
        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub DataGridLoans_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridLoans.SelectedIndexChanged
        'by aparna -17/01/2007
        Dim l_dgitem As DataGridItem
        Dim l_button_Select As ImageButton
        Try
            For Each l_dgitem In Me.DataGridLoans.Items
                l_button_Select = l_dgitem.FindControl("Imagebutton3")
                If l_dgitem.ItemIndex = Me.DataGridLoans.SelectedIndex Then
                    l_button_Select.ImageUrl = "images\selected.gif"
                Else
                    l_button_Select.ImageUrl = "images\select.gif"
                End If
            Next
        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
        'by aparna -17/01/2007
    End Sub
    ''Private Function SetAttributes(ByVal txtZipCode As TextBox, ByVal CountryValue As String)
    ''    '************************************************************************************************************
    ''    ' Author            : Ashutosh Patil 
    ''    ' Created On        : 25-Jan-2007
    ''    ' Desc              : This function will set max length of Zip Code against the respective countries
    ''    '                     For USA-9,CANADA-6,OTHER COUNTRIES - Default is 10
    ''    ' Related To        : YREN-3029,YREN-3028
    ''    ' Modifed By        : 
    ''    ' Modifed On        :
    ''    ' Reason For Change : 
    ''    '************************************************************************************************************
    ''    Try
    ''        If CountryValue = "US" Then
    ''            txtZipCode.MaxLength = 9
    ''            txtZipCode.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
    ''        ElseIf CountryValue = "CA" Then
    ''            txtZipCode.MaxLength = 6
    ''            txtZipCode.Attributes.Add("onkeypress", "javascript:ValidateAlphaNumeric();")
    ''        Else
    ''            txtZipCode.MaxLength = 10
    ''            txtZipCode.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
    ''        End If
    ''    Catch ex As Exception
    ''        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    ''    End Try
    ''End Function
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
    '		ExceptionPolicy.HandleException(ex, "Exception Policy")
    '		Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '	End Try
    'End Function
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
    'By aparna -YREN-3115
    Protected Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim dv As New DataView
            Dim l_dr_notes As DataRow
            Dim l_dt_new_notes As New DataTable
            Dim j As Integer = 0
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
                Me.DataGridParticipantNotes.DataSource = l_datatable_Notes
                'Shubhrata YRSPS 4614
                Me.MakeDisplayNotesDataTable()
                'Shubhrata YRSPS 4614
                Me.DataGridParticipantNotes.DataBind()
                'Me.ButtonSaveParticipant.Visible = True
                Me.ButtonSaveParticipant.Visible = False
                Me.ButtonSaveParticipants.Visible = True
                Me.ButtonSaveParticipants.Enabled = True
                Me.MakeLinkVisible()
            End If

        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    Private Sub SetPrimaryAddressDetails()
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
    End Sub
    Private Sub SetSecondaryAddressDetails()
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
    End Sub
    Private Sub ButtonSaveParticipants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSaveParticipants.Click

        ValidationSummaryParticipants.Enabled = True 'Added By Shashi Shekhar:2009-12-23
        Dim l_string_Message As String = ""
        Dim dtTelephone As New DataTable
        Dim drTelephone As DataRow
        Dim stTelephoneErrors As String 'PPP | 2015.10.10 | YRS-AT-2588
        Try

            'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940


            Dim controlsecurity As String = SecurityCheck.Check_Authorization("ButtonSaveParticipant", Convert.ToInt32(Session("LoggedUserKey")))

            If Not controlsecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", controlsecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If


            If Me.IsValid Then
                'BS:2012:04.24:YRS 5.0-1470:BT:951:check before saving data adrress is verify or not and validation for notes detail
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

                'Start-SR:2013.11.19 : YRS 5.0-2165 : RMD Tax Rate should be greater then 10%
                'Start:  Dinesh k           2014.05.15      BT-2537 : YRS 5.0-2370 - Change to validation on RMD tax rate
                'If TextBoxRMDTax.Text.Trim <> String.Empty Then
                '    If Convert.ToDecimal(TextBoxRMDTax.Text.Trim) < 10.0 Then
                '        MessageBox.Show(PlaceHolder1, "YMCA", "RMD Tax Rate cannot be less then 10.", MessageBoxButtons.Stop)
                '        Exit Sub
                '    End If
                'End If

                If TextBoxRMDTax.Text.Trim <> String.Empty Then
                    If Convert.ToDecimal(TextBoxRMDTax.Text.Trim) < 0.0 Or Convert.ToDecimal(TextBoxRMDTax.Text.Trim) > 100.0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "RMD Tax Withholding(%) should be between 0 to 100.", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If

                'End:  Dinesh k           2014.05.15      BT-2537 : YRS 5.0-2370 - Change to validation on RMD tax rate 
                'End-SR:2013.11.19 : YRS 5.0-2165 : RMD Tax Rate should be greater then 10%


                Dim l_DateInFuture As Date
                Dim l_DateInPast As Date
                Dim l_PrimDate As Date
                Dim l_SecDate As Date
                Dim l_PrimaryDate As Date
                Dim l_dr_PrimaryAddress As DataRow()
                Dim l_dr_SecondaryAddress As DataRow()
                Dim l_SecondaryDate As Date


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

                'AA:06.02.2014:BT:2316:YRS 5.0 2262 : Added below code for 
                'validating spousal wavier date and cannot locate spouse for a married person 
                'whose primary spouse beneficiary percentage is less than 100%
                If Not ValidateSpousalWavierAndCannotLocateSpouse() Then
                    Exit Sub
                End If

                'BS:2012.03.09:-restructure Address Control
                'If Me.TextBoxEffDate.Text <> "" AndAlso Me.TextBoxEffDate.Text <> l_PrimaryDate Then
                '	l_PrimaryDate = CType(Me.TextBoxEffDate.Text, Date)
                If m_str_Pri_EffDate <> "" AndAlso Me.m_str_Pri_EffDate <> l_PrimaryDate Then
                    l_PrimaryDate = CType(Me.m_str_Pri_EffDate, Date)

                    If Date.Compare(l_DateInPast, l_PrimaryDate) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
                        Exit Sub
                    End If

                    If Date.Compare(l_PrimaryDate, l_DateInFuture) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Primary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
                        Exit Sub
                    End If
                End If



                If Me.m_str_Sec_EffDate <> "" AndAlso Me.m_str_Sec_EffDate <> l_SecondaryDate Then
                    l_SecondaryDate = CType(Me.m_str_Sec_EffDate, Date)

                    If Date.Compare(l_DateInPast, l_SecondaryDate) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year old. Do you wish to continue?", MessageBoxButtons.YesNo)
                        Exit Sub
                    End If

                    If Date.Compare(l_SecondaryDate, l_DateInFuture) > 0 Then
                        MessageBox.Show(PlaceHolder1, "YMCA", "Secondary Effective date is more than a year in future. Do you wish to continue?", MessageBoxButtons.YesNo)
                        Exit Sub
                    End If

                End If





                '            If Me.ButtonEditAddress.Enabled = False Then
                '                'Primary Address
                '                Call SetPrimaryAddressDetails()
                '                'Secondary Address
                '                Call SetSecondaryAddressDetails()


                l_string_Message = AddressWebUserControl1.ValidateAddress()
                If l_string_Message <> "" Then
                    Me.ButtonSaveParticipant.Visible = False
                    Me.MakeLinkVisible()
                    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                    Exit Sub
                End If
                l_string_Message = AddressWebUserControl2.ValidateAddress()
                If l_string_Message <> "" Then
                    Me.ButtonSaveParticipant.Visible = False
                    Me.MakeLinkVisible()
                    MessageBox.Show(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop)
                    Exit Sub
                End If

                If TextBoxTelephone.Visible And (AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA") Then
                    'START: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(TextBoxTelephone.Text.Trim(), TextBoxHome.Text.Trim(), TextBoxMobile.Text.Trim(), TextBoxFax.Text.Trim())
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        ButtonSaveParticipants.Enabled = True
                        ButtonCancel.Enabled = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'If TextBoxTelephone.Text.Trim() <> "" Then
                    '    If TextBoxTelephone.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxHome.Text.Trim() <> "" Then
                    '    If TextBoxHome.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxMobile.Text.Trim() <> "" Then
                    '    If TextBoxMobile.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxFax.Text.Trim() <> "" Then
                    '    If TextBoxFax.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If
                    'END: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                ElseIf HelperFunctions.isNonEmpty(Session("dt_PrimaryContactInfo")) And (AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA") Then
                    dtTelephone = Session("dt_PrimaryContactInfo")
                    'START: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(dtTelephone)
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        ButtonSaveParticipants.Enabled = True
                        ButtonCancel.Enabled = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'For Each drTelephone In dtTelephone.Rows
                    '    'Anudeep:27.08.2013 -BT-1555:YRS 5.0-1769:To check trimmed value for length
                    '    If drTelephone("PhoneNumber").ToString().Trim().Length > 0 And drTelephone("PhoneNumber").ToString().Trim().Length <> 10 Then
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", drTelephone("PhoneType").ToString() + " number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'Next
                    'END: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                End If

                If TextBoxSecTelephone.Visible And (AddressWebUserControl2.DropDownListCountryValue = "US" Or AddressWebUserControl2.DropDownListCountryValue = "CA") Then
                    'START: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(TextBoxSecTelephone.Text.Trim(), TextBoxSecHome.Text.Trim(), TextBoxSecMobile.Text.Trim(), TextBoxSecFax.Text.Trim())
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        ButtonSaveParticipants.Enabled = True
                        ButtonCancel.Enabled = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'If TextBoxSecTelephone.Text.Trim() <> "" Then
                    '    If TextBoxSecTelephone.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Office number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxSecHome.Text.Trim() <> "" Then
                    '    If TextBoxSecHome.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Home number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxSecMobile.Text.Trim() <> "" Then
                    '    If TextBoxSecMobile.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Mobile number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If

                    'If TextBoxSecFax.Text.Trim() <> "" Then
                    '    If TextBoxSecFax.Text.Length <> 10 Then
                    '        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", "Fax number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'End If
                    'END: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                ElseIf HelperFunctions.isNonEmpty(Session("dt_SecondaryContactInfo")) And (AddressWebUserControl2.DropDownListCountryValue = "US" Or AddressWebUserControl2.DropDownListCountryValue = "CA") Then
                    dtTelephone = Session("dt_SecondaryContactInfo")
                    'START: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                    stTelephoneErrors = ValidateTelephoneNumbers(dtTelephone)
                    If Not String.IsNullOrEmpty(stTelephoneErrors) Then
                        ButtonSaveParticipants.Enabled = True
                        ButtonCancel.Enabled = True
                        MessageBox.Show(PlaceHolder1, "YMCA", stTelephoneErrors, MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    'For Each drTelephone In dtTelephone.Rows
                    '    'Anudeep:27.08.2013 -BT-1555:YRS 5.0-1769:To check trimmed value for length
                    '    If drTelephone("PhoneNumber").ToString().Trim().Length > 0 And drTelephone("PhoneNumber").ToString().Trim().Length <> 10 Then
                    '        'Anudeep:23.08.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                    '        ButtonSaveParticipants.Enabled = True
                    '        ButtonCancel.Enabled = True
                    '        MessageBox.Show(PlaceHolder1, "YMCA", drTelephone("PhoneType").ToString() + " number must be 10 digits.", MessageBoxButtons.Stop)
                    '        Exit Sub
                    '    End If
                    'Next
                    'END: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                End If



                'If Me.TextBoxSSNo.Enabled = True Then 'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -Commented
                If IsSSNEdited() Then 'SP 2014.02.04 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -added
                    l_string_Message = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsValidSSNo(Me.TextBoxSSNo.Text.Trim())

                    If l_string_Message.ToString().Trim() = "Not_Phony_SSNo" Then
                        Session("PhonySSNo") = "Not_Phony_SSNo"
                        Call SetControlFocus(Me.TextBoxSSNo)
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
                'BS:2012.03.21:BT:993:-After "Active primary beneficiary not defined" prompt TextBoxDateDeceased should be  disable 

                TextBoxDeceasedDate.Enabled = False
                TextBoxDeceasedDate.ReadOnly = True

                If Me.TextBoxPrimaryA.Text = "" And Session("WithDrawn_member") = False Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Active primary beneficiary not defined.Do you want to continue?", MessageBoxButtons.YesNo)
                    Exit Sub
                End If
                ButtonCancel.Enabled = False
                ButtonOK.Enabled = True
                Me.MakeLinkVisible()


                SaveInfo()
            End If
            If Not Session("ValidationMessage") Is Nothing Then
                'Anudeep:06.11.2013:BT-1455:YRS 5.0-1733:Enabling the save button after displaying validation message
                Me.ButtonSaveParticipants.Enabled = False
                Me.ButtonSaveParticipants.Visible = False
                Me.ButtonSaveParticipant.Visible = True
                Me.ButtonSaveParticipant.Enabled = True
                Me.ButtonCancel.Enabled = True
            Else
                Me.ButtonSaveParticipants.Visible = False
                Me.ButtonSaveParticipants.Enabled = False
                Me.ButtonSaveParticipant.Visible = True
                Me.ButtonOK.Enabled = True
                Me.MakeLinkVisible()
            End If
        Catch ex As SqlException
            If ex.Number = 60006 And ex.Procedure.ToString = "yrs_usp_AMCM_SearchConfigurationMaintenance" Then
                'START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder
                'g_String_Exception_Message = "No Key defined for Phony SSNo in AtsMetaConfiguration."
                g_String_Exception_Message = "No Key defined for Placeholder SSNo in AtsMetaConfiguration."
                'END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder 
            Else
                g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            End If
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)

        End Try
    End Sub
    ' Shubhrata May 16th 2007 Plan Split Changes
    'This function sets the visibility of the link(that takes to Retirees Screen) as true for RT,RA,RD and DR status 
    Private Function MakeLinkVisible()
        Try
            Dim l_string_FundStatusType As String = ""
            If Not Session("FundStatusType") Is Nothing Then
                l_string_FundStatusType = CType(Session("FundStatusType"), String)
            End If
            'commented by Swopna Phase IV changes 8 Apr,2008**************start
            'If l_string_FundStatusType.Trim <> "" Then
            '    If l_string_FundStatusType.Trim.ToUpper = "RT" Or l_string_FundStatusType.Trim.ToUpper = "RA" Or l_string_FundStatusType.Trim.ToUpper = "RD" Or l_string_FundStatusType.Trim.ToUpper = "DR" Or l_string_FundStatusType.Trim.ToUpper = "DD" Then
            '        If Me.ButtonSaveParticipant.Enabled = False And Me.ButtonSaveParticipants.Enabled = False Then
            '            Me.HyperLinkViewRetireesInfo.Visible = True
            '        Else
            '            Me.HyperLinkViewRetireesInfo.Visible = False
            '        End If
            '    Else
            '        Me.HyperLinkViewRetireesInfo.Visible = False
            '    End If
            'Else
            '    Me.HyperLinkViewRetireesInfo.Visible = False
            'End If
            'commented by Swopna Phase IV changes 8 Apr,2008**************end
            'Swopna Phase IV changes 8 Apr,2008**************start
            'Allow/disallow toggle between screens
            If l_string_FundStatusType.Trim <> "" Then
                Select Case (l_string_FundStatusType)
                    Case "RT", "RA", "RD", "DD", "DR", "RP", "RE", "RDNP", "RPT" '----Added RPT on 20May08
                        If Me.ButtonSaveParticipant.Enabled = False And Me.ButtonSaveParticipants.Enabled = False Then
                            Me.HyperLinkViewRetireesInfo.Visible = True
                            'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
                            'Priya              18-Feb-2010     YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
                            'chkShareInfoAllowed.Visible = True
                            'lblShareInfoAllowed.Visible = True
                            'END YRS 5.0-988
                            'End 12-May-2010 YRS 5.0-1073
                        Else
                            Me.HyperLinkViewRetireesInfo.Visible = False
                            'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
                            'Priya              18-Feb-2010     YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed

                            'chkShareInfoAllowed.Visible = False
                            'lblShareInfoAllowed.Visible = False
                            'END YRS 5.0-988
                            'End 12-May-2010 YRS 5.0-1073
                        End If
                    Case Else
                        Me.HyperLinkViewRetireesInfo.Visible = False
                        'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
                        'Priya              18-Feb-2010     YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
                        'chkShareInfoAllowed.Visible = False
                        'lblShareInfoAllowed.Visible = False
                        'END YRS 5.0-988
                        'End 12-May-2010 YRS 5.0-1073
                End Select
            Else
                Me.HyperLinkViewRetireesInfo.Visible = False
                'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
                'Priya              18-Feb-2010     YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
                'chkShareInfoAllowed.Visible = False
                'lblShareInfoAllowed.Visible = False
                'END YRS 5.0-988
                'End 12-May-2010 YRS 5.0-1073
            End If
            'Swopna Phase IV changes 8 Apr,2008**************end
        Catch
            Throw
        End Try
    End Function

    'Shubhrata May 16th 2007 Plan Split Changes
#Region "General Utility Functions"
    Private Function isNonEmpty(ByRef ds As DataSet) As Boolean
        If ds Is Nothing Then Return False
        If ds.Tables.Count = 0 Then Return False
        If ds.Tables(0).Rows.Count = 0 Then Return False
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
    Private Function isEmpty(ByRef dv As DataView) As Boolean
        Return Not isNonEmpty(dv)
    End Function
    'dg = The datagrid to bind data to
    'ds = The dataset which contains the data
    'forceVisible = Whether the datagrid should be displayed if it does not contain any data
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef ds As DataSet, Optional ByVal forceVisible As Boolean = False)
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
    End Sub
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef dv As DataView, Optional ByVal forceVisible As Boolean = False)
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
    End Sub
#End Region


    Private Sub DataGridRetirementAccntContributions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridRetirementAccntContributions.ItemDataBound

        Try
            e.Item.Cells(mConst_DG_PlanType_Index).Visible = False
            If e.Item.ItemType <> ListItemType.Header Then
                '  e.Item.Cells(mConst_DG_PlanType_Index).Visible = False
                e.Item.Cells(mConst_DG_AcctType_Index).HorizontalAlign = HorizontalAlign.Left
                e.Item.Cells(mConst_DG_Taxable_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_NonTaxable_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_Interest_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_EmpTotal_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_YmcaTaxable_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_YmcaInterest_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_YmcaTotal_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_Total_Index).HorizontalAlign = HorizontalAlign.Right


            End If

            'If e.Item.ItemType = ListItemType.Header Then
            '    e.Item.Cells(8).Text = "Account  Total"
            'End If
            'anita and shubhrata date apr19
            If e.Item.Cells(mConst_DG_AcctType_Index).Text.ToUpper <> "&NBSP;" And e.Item.Cells(mConst_DG_AcctType_Index).Text.ToUpper <> "ACCOUNT" Then


                Dim l_decimal_ChangeFormatCurrency As Decimal
                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_Taxable_Index).Text)
                e.Item.Cells(mConst_DG_Taxable_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_NonTaxable_Index).Text)
                e.Item.Cells(mConst_DG_NonTaxable_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_Interest_Index).Text)
                e.Item.Cells(mConst_DG_Interest_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_EmpTotal_Index).Text)
                e.Item.Cells(mConst_DG_EmpTotal_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_YmcaTaxable_Index).Text)
                e.Item.Cells(mConst_DG_YmcaTaxable_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_YmcaInterest_Index).Text)
                e.Item.Cells(mConst_DG_YmcaInterest_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_YmcaTotal_Index).Text)
                e.Item.Cells(mConst_DG_YmcaTotal_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_Total_Index).Text)
                e.Item.Cells(mConst_DG_Total_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)
            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub DataGridSavingAccntContributions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridSavingAccntContributions.ItemDataBound
        Try
            e.Item.Cells(mConst_DG_PlanType_Index).Visible = False
            If e.Item.ItemType <> ListItemType.Header Then

                e.Item.Cells(mConst_DG_AcctType_Index).HorizontalAlign = HorizontalAlign.Left
                e.Item.Cells(mConst_DG_Taxable_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_NonTaxable_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_Interest_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_EmpTotal_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_YmcaTaxable_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_YmcaInterest_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_YmcaTotal_Index).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(mConst_DG_Total_Index).HorizontalAlign = HorizontalAlign.Right


            End If

            'If e.Item.ItemType = ListItemType.Header Then
            '    e.Item.Cells(8).Text = "Account  Total"
            'End If
            'anita and shubhrata date apr19
            If e.Item.Cells(mConst_DG_AcctType_Index).Text.ToUpper <> "&NBSP;" And e.Item.Cells(mConst_DG_AcctType_Index).Text.ToUpper <> "ACCOUNT" Then


                Dim l_decimal_ChangeFormatCurrency As Decimal
                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_Taxable_Index).Text)
                e.Item.Cells(mConst_DG_Taxable_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_NonTaxable_Index).Text)
                e.Item.Cells(mConst_DG_NonTaxable_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_Interest_Index).Text)
                e.Item.Cells(mConst_DG_Interest_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_EmpTotal_Index).Text)
                e.Item.Cells(mConst_DG_EmpTotal_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_YmcaTaxable_Index).Text)
                e.Item.Cells(mConst_DG_YmcaTaxable_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_YmcaInterest_Index).Text)
                e.Item.Cells(mConst_DG_YmcaInterest_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_YmcaTotal_Index).Text)
                e.Item.Cells(mConst_DG_YmcaTotal_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)

                l_decimal_ChangeFormatCurrency = Convert.ToDecimal(e.Item.Cells(mConst_DG_Total_Index).Text)
                e.Item.Cells(mConst_DG_Total_Index).Text = FormatCurrency(l_decimal_ChangeFormatCurrency)
            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Function SetSelectedIndex(ByVal parameterDataGrid As DataGrid, ByVal parameterDataTable As DataTable)
        Try
            If parameterDataTable Is Nothing Then
                parameterDataGrid.SelectedIndex = -1
            Else
                parameterDataGrid.SelectedIndex = (parameterDataTable.Rows.Count - 1)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function

    Private Sub DatagridAcctTotal_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridAcctTotal.ItemDataBound
        Try
            If e.Item.ItemType <> ListItemType.Header Then
                e.Item.Cells(1).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(2).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(3).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(4).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(5).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
                e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right
                'e.Item.Cells(8).Visible = False
            End If

            If e.Item.ItemType <> ListItemType.Header Then
                '    e.Item.Visible = False
                ' e.Item.Cells(0).Text = "Grand Total"
                e.Item.Cells(0).HorizontalAlign = HorizontalAlign.Left
            End If

            If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" And e.Item.Cells(0).Text.ToUpper.Trim() <> "" Then


                Dim l_decimal_try As Decimal
                l_decimal_try = Convert.ToDecimal(e.Item.Cells(1).Text)
                e.Item.Cells(1).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(2).Text)
                e.Item.Cells(2).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(3).Text)
                e.Item.Cells(3).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(4).Text)
                e.Item.Cells(4).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(5).Text)
                e.Item.Cells(5).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(6).Text)
                e.Item.Cells(6).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
                e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(8).Text)
                e.Item.Cells(8).Text = FormatCurrency(l_decimal_try)
            End If




        Catch ex As Exception
            Throw ex
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
    'YMCA Phase 4 
    Private Sub DataGridLoanAccountBreakdown_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridLoanAccountBreakdown.ItemDataBound
        e.Item.Cells(0).Font.Bold = True
    End Sub
    Private Sub LoadLoansAccountBreakdownDataGrid(ByVal parameterDataSet As DataSet)
        Try

            Dim l_ds_LoanAccBreakdown As DataSet
            Dim l_dt_LoanAccBreakdown As DataTable
            Dim l_dr_LoanAccBreakdown As DataRow
            Dim l_dr_LoanTotalRow As DataRow
            Dim dr As DataRow
            Dim dt As DataTable
            Dim l_decimal_TotalAccountBalance As Decimal = 0
            Dim l_decimal_TotalLoanAmounts As Decimal = 0
            'Priya 10-June-2008
            Dim l_decimal_TotalPercentage As Decimal = 0
            'Priya 10-June-2008

            Dim i As Integer = 0
            Dim k As Integer = 1
            Dim s As String
            l_ds_LoanAccBreakdown = parameterDataSet
            If Not l_ds_LoanAccBreakdown.Tables("LoanAccountBreakdown") Is Nothing Then
                l_dt_LoanAccBreakdown = l_ds_LoanAccBreakdown.Tables("LoanAccountBreakdown")
                For Each l_dr_LoanAccBreakdown In l_dt_LoanAccBreakdown.Rows
                    l_decimal_TotalAccountBalance = l_decimal_TotalAccountBalance + Convert.ToDecimal(l_dr_LoanAccBreakdown("Account Balances"))
                    l_decimal_TotalLoanAmounts = l_decimal_TotalLoanAmounts + Convert.ToDecimal(l_dr_LoanAccBreakdown("Loan Amounts"))
                    'Priya 10-June-2008
                    l_decimal_TotalPercentage = l_decimal_TotalPercentage + Convert.ToDecimal(l_dr_LoanAccBreakdown("Pct of Total"))
                    'Priya 10-June-2008
                    If Convert.ToString(l_dr_LoanAccBreakdown("Account Name")) = "RT ROLL" Then
                        s = CType(k, String)
                        l_dr_LoanAccBreakdown("Account Name") = "Rollover " + s
                        k = CType(s, Integer)
                        k = k + 1
                    End If


                Next

                l_dr_LoanTotalRow = l_dt_LoanAccBreakdown.NewRow()
                l_dr_LoanTotalRow("Account Name") = "Total"

                l_dr_LoanTotalRow("Account Balances") = l_decimal_TotalAccountBalance
                l_dr_LoanTotalRow("Loan Amounts") = l_decimal_TotalLoanAmounts
                'Priya 10-June-2008 
                l_dr_LoanTotalRow("Pct of Total") = l_decimal_TotalPercentage
                'Priya 10-June-2008

                l_dt_LoanAccBreakdown.Rows.Add(l_dr_LoanTotalRow)
                l_dt_LoanAccBreakdown.AcceptChanges()

                DataGridLoanAccountBreakdown.DataSource = l_dt_LoanAccBreakdown
                DataGridLoanAccountBreakdown.DataBind()
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'YMCA Phase 4 
    'YMCA Phase 4 Additional Requirement-27May08---start
    Private Sub ButtonTerminateParticipation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTerminateParticipation.Click
        Try
            YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.UpdateOnParticipantTermination(CType(Session("PersId"), String))

            'Swopna 9June08 Start
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Participant terminated.", MessageBoxButtons.OK)
            ButtonTerminateParticipation.Enabled = False
            'Swopna 9June08 End
        Catch sqlEx As SqlException

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'YMCA Phase 4 Additional Requirement-27May08---end
    'Reverted changes by Anudeep on 2012.11.14 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen Changed from 'Proirity Handling' to 'Exhausted DB settlement efforts' 
    'START : Added By Dilip yadav : 29-Oct-09 : YRS 5.0.921
    'Start: Bala: 19/01/2016: YRS-AT-2398: Control name change.
    'Private Sub checkboxPriority_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkboxPriority.CheckedChanged
    Private Sub checkboxExhaustedDBSettle_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkboxExhaustedDBSettle.CheckedChanged
        'End: Bala: 19/01/2016: YRS-AT-2398: Control name change.
        'START: MMR | 2016.10.07 | YRS-AT-3062 | Added to check if user has access over control or not and showing message if user has no rights over the control
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("CheckboxExhaustedDBSettlePerson", Convert.ToInt32(Session("LoggedUserKey")))

        If Not checkSecurity.Equals("True") Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            If Session("IsExhaustedDBRMDSettle") = True Then
                checkboxExhaustedDBSettle.Checked = True
            Else
                checkboxExhaustedDBSettle.Checked = False
            End If
            HiddenFieldDirty.Value = "false" ' MMR | 2016.10.14 | YRS-AT-3062 | Setting hidden field value to false to not display confirm message when user clicks on close button.
            Exit Sub
        End If
        'END: MMR | 2016.10.07 | YRS-AT-3062 | Added to check if user has access over control or not and showing message if user has no rights over the control
        'START: MMR | 2016.10.14 | YRS-AT-3063 | Displaying warning message based on Death date exist or not of participant
        If checkboxExhaustedDBSettle.Checked = True Then
            IsExhaustedDBRMDSettleOptionSelected = True           
            If Not String.IsNullOrEmpty(TextBoxDeceasedDate.Text) Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_EXHAUSTEDDBRMDSETTLEMENTEFFORTS_DECEASED, MessageBoxButtons.YesNo)
            ElseIf TextBoxDeceasedDate.Text = "" Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_EXHAUSTEDDBRMDSETTLEMENTEFFORTS_ALIVE, MessageBoxButtons.YesNo)
            End If
            'ENd: MMR | 2016.10.14 | YRS-AT-3063 | Displaying message based on Death date exist or not of participant
        End If
        ButtonSaveParticipant.Enabled = True
        ButtonCancel.Enabled = True
        Me.HyperLinkViewRetireesInfo.Visible = False
    End Sub
    'END : Added By Dilip yadav : 29-Oct-09 : YRS 5.0.921


    Private Sub ButtonGetArchiveDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGetArchiveDataBack.Click
        Session("CurrentProcesstoConfirm") = "DataArchive"
        'Shashi Shekhar:2010-03-26: change message for BT-488
        MessageBox.Show(PlaceHolder1, "Data Archive", "Are you sure you want to get the archived data back?", MessageBoxButtons.YesNo)
    End Sub

    'Shashi Shekhar:2010-01-27: To update the bitIsArchived field in atsPerss table
    'To Update the record in database
    Private Function RetrieveData()
        Dim list As New ArrayList
        Dim errorMsg As String 'Shashi Shekhar:2010-04-07
        Try
            Dim strSSN As String = Me.TextBoxSSNo.Text.Insert(3, "-")
            strSSN = strSSN.Insert(6, "-")


            If Not Session("PersId") = Nothing Then
                list.Add(Session("PersId").ToString().Trim())
                'Shashi Shekhar:2010-04-07:Adding one parameter for error message returned from procedure as output parameter
                errorMsg = YMCARET.YmcaBusinessObject.DataArchiveBOClass.RetrieveData(list)
            End If

            'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Records have been updated successfully.", MessageBoxButtons.OK)
            tdRetrieveData.Visible = False
            Session("IsDataArchived") = "No"
            LabelGenHdr.Text = "General"
            LabelAccContHdr.Text = ""
            '-----------------------------------------------------------------------------------------------------------------
            'Shashi Shekhar: 28-Oct-2010: For (BT-663 YRS 5.0-1197 - Replace SSN with Fund Id in title bar)
            If (g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "System.DBNull" And g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString() <> "") Then
                'LabelHdr.Text = Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ",SS#: " + strSSN
                'LabelHdr.Text = Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ", Fund No: " + g_dataset_GeneralInfo.Tables("GeneralInfo").Rows(0).Item("FundNo").ToString().Trim()
            End If
            '----------------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            ' Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            'Shashi Shekhar:2010-04-07:Handling DataArchive error message (concate sql error message with user defined error message returned from procedure)
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        Finally
            list = Nothing
        End Try

    End Function
    'Priya 12-May-2010 YRS 5.0-1073 :commented  ShareInfoAllowed code
    'Priya              18-Feb-2010     YRS 5.0-988 : fetch value of newly added column bitShareInfoAllowed
    'Private Sub chkShareInfoAllowed_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkShareInfoAllowed.CheckedChanged
    '    ButtonSaveParticipant.Enabled = True
    '    ButtonCancel.Enabled = True
    '    Me.HyperLinkViewRetireesInfo.Visible = False
    'End Sub
    'End 12-May-2010 YRS 5.0-1073 

    '5-April-2010:Priya:YRS 5.0-1042 : fetch value of newly aaded column bitShareInfoAllowed
    '2011.07.08 :bhavnaS YRS 5.0-1354 : replace caption
    Private Sub chkPersonalInfoSharingOptOut_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPersonalInfoSharingOptOut.CheckedChanged
        ButtonSaveParticipant.Enabled = True
        ButtonCancel.Enabled = True
        Me.HyperLinkViewRetireesInfo.Visible = False
    End Sub
    'End 2011.07.08 YRS 5.0-1354

    '2011.07.08 :bhavnaS YRS 5.0-1354 : replace caption
    Private Sub chkGoPaperless_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGoPaperless.CheckedChanged
        ButtonSaveParticipant.Enabled = True
        ButtonCancel.Enabled = True
        Me.HyperLinkViewRetireesInfo.Visible = False
    End Sub
    'End 2011.07.08 YRS 5.0-1354


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


            If Not l_dsLockResDetails Is Nothing Then
                lblLockstatus.Text = "Status: "
                If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows.Count > 0 Then

                    If (l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("IsLock").ToString() <> "System.DBNull" And l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("IsLock").ToString() <> "") Then

                        If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("IsLock") = True Then
                            btnLockUnlock.Text = "Unlock Account"
                            lblLockstatus.Text = lblLockstatus.Text + "<span style='color:Gray'>Locked</span>"
                            lblLockResDetail.Text = "Reason: "
                            lblLockResDetail.Text = lblLockResDetail.Text + "<span style='color:Gray'>" + l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString().Trim + "</apsn>"
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
        Catch ex As Exception
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try


    End Sub

    Private Sub btnAcctLockEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAcctLockEdit.Click
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

            'btnAcctLockEdit.Enabled = False

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

            'If ddlReasonCode.SelectedItem.Value.Trim = "select" Then
            '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select reason code from list.", MessageBoxButtons.OK)
            '    Exit Sub
            'End If
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


    ' Shashi Shgekhar    11 Apr 2011     for YRS 5.0-1280 : ability to sort records in grid on employment tab

    Sub SortCommand_OnClick(ByVal Source As Object, ByVal e As DataGridSortCommandEventArgs)
        Try
            Dim dv As New DataView
            Dim SortExpression As String
            SortExpression = e.SortExpression
            Dim dg As DataGrid = DirectCast(Source, DataGrid)

            Dim ds As DataSet
            Select Case dg.ID
                Case "DataGridParticipantEmployment"
                    ds = Session("l_dataset_Employment")
                    Me.DataGridParticipantEmployment.SelectedIndex = -1
                    'DataGridParticipantEmployment.SelectedIndex = -1
                    'Case "DatagridAdditionalAccounts"
                    '    ds = Session("l_dataset_AddAccount")
                    '    DataGridAdditionalAccounts.SelectedIndex = -1
                    'Case "DataGridYMCAContact"
                    '    ds = Session("YMCA Contact")
                    'Case "DataGridYMCABranches"
                    '    ds = Session("YMCA Branch")
                    'Case "DataGridYMCAResolutions"
                    '    ds = Session("YMCA Resolution")
                    'Case "DataGridYMCANotesHistory"
                    '    ds = Session("YMCA Notes")
                    'Case "DataGridYMCABankInfo"
                    '    ds = Session("YMCA BankInfo")
                    'Case "DataGridYMCA"
                    '    ds = YMCASearchList
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

    Private Sub LoadPoADetails()
        'BT 706 :Add by BhavnaS : POA name does not show up on the Bene Info page,display all active poa 
        'reopened Issue on 2011.07.22
        Dim builder As New StringBuilder
        l_DataSet_POA = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpPOAInfo(Session("PersId").ToString())
        'Dinesh K           2012.12.12      BT-1236/YRS 5.0-1685:Add Category/Type field to Power of attorney and allow 3 types
        'GridViewRetireesAttorney.DataSource = l_DataSet_POA
        'GridViewRetireesAttorney.DataBind()
        If HelperFunctions.isNonEmpty(l_DataSet_POA) Then
            'Dinesh.K:2012.12.12:BT-1236/YRS 5.0-1685:Add Category/Type field to Power of attorney and allow 3 types
            'Anudeep:07.11.2013-BT:2269-YRS 5.0-2234:Added to show the Button text without POA
            ButtonPOA.Text = "Show/Edit all (" + l_DataSet_POA.Tables("POAInfo").Rows.Count.ToString() + ")"
            'For Each dr As DataRow In l_DataSet_POA.Tables("POAInfo").Rows
            '    If dr.IsNull("Name1") = True OrElse dr.Item("Name1") = "" Then Continue For
            '    If builder.Length > 0 Then builder.Append(", ")
            '    builder.Append(dr("Name1").ToString())
            'Next
        End If
        'If Not builder.ToString() Is Nothing Then
        '    tdPoa.Visible = True
        '    'lblPoaDetails.Text = builder.ToString()
        'Else
        '    tdPoa.Visible = False
        'End If

        'End BT 706 reopen Issue
    End Sub
    'BS:2012.01.18:YRS 5.0-1497 -Add edit button to modify the date of death
    Private Sub btnEditDeathDatePer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditDeathDatePer.Click
        Try
            If TextBoxDeceasedDate.Text <> String.Empty Then
                HiddenFieldDeathDate.Value = TextBoxDeceasedDate.Text
            End If
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("btnEditDeathDatePer", Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            ButtonSaveParticipant.Enabled = True
            ButtonCancel.Enabled = True
            TextBoxDeceasedDate.Enabled = True
            TextBoxDeceasedDate.ReadOnly = False
            ButtonOK.Enabled = False
            Me.MakeLinkVisible()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Sub
    'prasad 2012.01.19 For BT-925,YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
    Private Sub chkMRD_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMRD.CheckedChanged
        ButtonSaveParticipant.Enabled = True
        ButtonCancel.Enabled = True
        Me.HyperLinkViewRetireesInfo.Visible = False
        'START : SR | 06/21/2018 | YRS-AT-3858 | Added a validation (put up an error message) when the user check this check box (mark’s as true) for cases where participant is not eligible for RMD.         
        ' Added a validation (put up an error message) when the user check this check box (mark’s as true) for cases where participant is not eligible for RMD. 
        If IsRMDEligible = False AndAlso chkMRD.Checked Then
            chkMRD.Checked = False
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "YMCA-YRS", "ShowCommonDialog('" + Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_NON_RMD_ELIGIBILE + "','YMCA-YRS');", True)
        End If
        'END : SR | 06/21/2018 | YRS-AT-3858 | Added a validation (put up an error message) when the user check this check box (mark’s as true) for cases where participant is not eligible for RMD. 
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
                checkSecurity = SecurityCheck.Check_Authorization("FindInfo.aspx?Name=Person", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
                If Not checkSecurity.Equals("True") Then
                    Return "error|" + checkSecurity
                End If
                checkSecurity = SecurityCheck.Check_Authorization("ButtonWebFrontEnd", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
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
                checkSecurity = SecurityCheck.Check_Authorization("btnPartWebLockUnLock", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
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
                checkSecurity = SecurityCheck.Check_Authorization("btnPartWebLockUnLock", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
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
                checkSecurity = SecurityCheck.Check_Authorization("btnPartWebSendTempPass", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
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

    'BS:2012.06.15:BT:991:YRS 5.0-1530:
    Public Class FetchEmploymentAdditional
        Public Termdate As String = String.Empty
        Public FundEventId As String = String.Empty
        Public newFundStatusCode As String = String.Empty
        Public newFundStatusDesc As String = String.Empty
        Public YmcaId As String = String.Empty
        Public tdcontractexist As Boolean = False
        Public emp_reactive As Boolean = False
        Public recordexist As Boolean = False
        Public diff_ymcaexist As Boolean = False
    End Class
    'BS:2012.06.15:BT:991:YRS 5.0-1530:
    Public Function EmpReactivation() As FetchEmploymentAdditional

        Try
            Dim empAdd As FetchEmploymentAdditional = New FetchEmploymentAdditional()

            ' Dim YmcaId, FundEventId As String 'SP 2014.12.18 BT-2734\YRS 5.0-2454  -Commented unnecessary code
            'BS:2012.06.26:BT:991:YRS 5.0-1530:-if different ymca exist on same termination date then display prompt:“Multiple employment records exist with the same termination date. Please contact IT for Data Correction.”
            If Not Session("TermDate") Is Nothing Then
                empAdd.diff_ymcaexist = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DiffYMCAOnSameTermDateExist(CType(Session("PersId"), String), CType(Session("TermDate"), String))
            End If

            If empAdd.diff_ymcaexist Then
                Return empAdd
            End If


            If Not Session("TermDate") Is Nothing Then
                empAdd.recordexist = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.transactionexist(CType(Session("PersId"), String), CType(Session("TermDate"), String))
            End If

            If empAdd.recordexist Then
                Return empAdd
            End If

            'SP 2014.12.18 BT-2734\YRS 5.0-2454
            'Added if & else condition to check if employment exist then initialized employment details else disable reactivate button
            If HelperFunctions.isNonEmpty(Session("l_dataset_Employment")) Then
                'SP 2014.12.18 BT-2734\YRS 5.0-2454  -Commented unnecessary code
                'Dim dataviewemp As DataView
                'dataviewemp = CType(Session("l_dataset_Employment"), DataSet).Tables(0).DefaultView
                'dataviewemp.Sort = "Termdate DESC"
                'If Not IsDBNull(dataviewemp(0)("YmcaId")) And Not IsDBNull(dataviewemp(0)("FundEventId")) Then
                '    YmcaId = dataviewemp(0)("YmcaId").ToString
                '    FundEventId = dataviewemp(0)("FundEventId").ToString
                'Else
                '    Throw New Exception("Error While reactivation process.")
                'End If
                'SP 2014.12.18 BT-2734\YRS 5.0-2454  -Commented unnecessary code

                Dim l_dataset_AddAccount, l_dataset_Employment As DataSet
                Dim findRowAccount() As DataRow

                Dim dataViewEmp As DataView
                Dim terminationDate As Date

                l_dataset_Employment = CType(Session("l_dataset_Employment"), DataSet)
                l_dataset_AddAccount = CType(Session("l_dataset_AddAccount"), DataSet)
                dataViewEmp = l_dataset_Employment.Tables(0).DefaultView
                dataViewEmp.Sort = "Termdate DESC"
                If Not IsDBNull(dataViewEmp(0)("Termdate")) And Not IsDBNull(dataViewEmp(0)("YmcaId")) And Not IsDBNull(dataViewEmp(0)("FundEventId")) Then
                    empAdd.Termdate = dataViewEmp(0)("Termdate").ToString
                    empAdd.YmcaId = dataViewEmp(0)("YmcaId").ToString
                    empAdd.FundEventId = dataViewEmp(0)("FundEventId").ToString
                    empAdd.emp_reactive = True

                End If
                If l_dataset_AddAccount.Tables(0).Rows.Count <> 0 Then
                    If Not empAdd.Termdate Is Nothing And Not empAdd.YmcaId Is Nothing Then
                        'BS:2012.06.25:--BT:991:YRS 5.0-1530:--lumsum basiscode will not be comes under td contract prompt
                        findRowAccount = l_dataset_AddAccount.Tables(0).Select("AccountType='TD' AND BasisCode<>'L' AND TerminationDate='" + empAdd.Termdate + "' AND YmcaId='" + empAdd.YmcaId + "'")
                        If findRowAccount.Length <> 0 Then
                            empAdd.tdcontractexist = True
                        End If
                    End If
                End If
            Else
                ButtonReactivate.Visible = False
            End If
            Return empAdd
        Catch
            Throw
        End Try
    End Function
    'BS:2012.06.15:BT:991:YRS 5.0-1530:
    Public Function UpdateFundEventEmpElectives(ByVal Termdate As String, ByVal FundEventId As String,
      ByVal YmcaId As String, ByVal tdcontractexist As Boolean, ByVal newFundStatusCode As String,
      ByVal newFundStatusDesc As String
      )
        Try


            Dim returnval As Boolean
            Dim oldfundStatus As String

            If Not CType(Session("FundStatusType"), String) Is Nothing Then
                oldfundStatus = Session("FundStatusType").ToString
                returnval = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.updateFundEventEmpEvent(Termdate, FundEventId, YmcaId, tdcontractexist, 1, newFundStatusCode, newFundStatusDesc)
            End If

            If returnval Then
                Session("FundStatusType") = newFundStatusCode
                Session("FundStatus") = newFundStatusDesc
                LoadEmploymentTab()
                If tdcontractexist Then
                    LoadAdditionalAccountsTab()
                End If
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", String.Format(Resources.ParticipantsInformation.REACTIVE_SUCCESS, oldfundStatus, newFundStatusCode), MessageBoxButtons.OK)
                ButtonReactivate.Visible = False
                Session("ReactivationStart") = ""
            Else
                Throw New Exception("Error While reactivation process.")
            End If
        Catch
            Throw
        End Try
    End Function
    'BS:2012.06.15:BT:991:YRS 5.0-1530:-set flag for client side validation
    Public Function SetFlagEmpAndTdReactivation()

        Try
            Dim empAdd As FetchEmploymentAdditional = EmpReactivation()

            Dim javaScript As New System.Text.StringBuilder()

            If Not Me.Page.ClientScript.IsClientScriptBlockRegistered("ArrayScript") Then
                Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ArrayScript", javaScript.ToString(), True)
            End If

            javaScript.Clear()
            javaScript.Append("$(document).ready(function() { initializeReactivateControl('" + Convert.ToString(empAdd.recordexist) + "', '" + Convert.ToString(empAdd.tdcontractexist) + "', '" + Convert.ToString(empAdd.emp_reactive) + "', '" + Convert.ToString(empAdd.diff_ymcaexist) + "');});")
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), Me.ClientID, javaScript.ToString(), True)

        Catch
            Throw
        End Try
    End Function
    'BS:2012.06.15:BT:991:YRS 5.0-1530:-validation for td contract
    Private Function performTDValidation() As Boolean
        MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.TD_CONTRACT, MessageBoxButtons.YesNo)
        Session("Flag") = "IsTdReactivate"
        'BS:2012.06.21:-reopen :BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
        Session("IsTDContract") = ""
        Exit Function
    End Function


    'Added by prasad 2012.03.13 For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
    Private Sub ButtonReactivate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonReactivate.Click

        'Dim recordexist As Boolean
        ''Added by prasad 2012.04.30 (Reopen)YRS 5.0-1530 - Need ability to reactivate a terminated employee
        'Dim YmcaId, FundEventId As String
        ''First check permission
        'Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonReactivate", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
        'If Not checkSecurity.Equals("True") Then
        '	MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
        '	Exit Sub
        'End If
        'If Not Session("TermDate") Is Nothing Then
        '	recordexist = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.transactionexist(CType(Session("PersId"), String), CType(Session("TermDate"), String))
        'End If
        ''Check record exist or not and show messagebox if exist.
        'If recordexist Then
        '	MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.RECORD_EXIST, MessageBoxButtons.Stop)
        '	Exit Sub
        'End If
        ''please check save button enabled or disabled
        'If ButtonSaveParticipant.Enabled Then
        '	MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.SAVE_CHANGES, MessageBoxButtons.Stop)
        '	Exit Sub
        'End If
        ''First check termination was in last month if yes then give yes/no message box to user
        'Dim dataviewemp As DataView
        'dataviewemp = CType(Session("l_dataset_Employment"), DataSet).Tables(0).DefaultView
        'dataviewemp.Sort = "Termdate DESC"
        'If Not IsDBNull(dataviewemp(0)("YmcaId")) And Not IsDBNull(dataviewemp(0)("FundEventId")) Then
        '	YmcaId = dataviewemp(0)("YmcaId").ToString
        '	FundEventId = dataviewemp(0)("FundEventId").ToString
        'Else
        '	Throw New Exception("Error While reactivation process.")
        'End If

        'Dim termdatemonth As Date
        'If Not Session("TermDate") Is Nothing Then
        '	termdatemonth = CType(Session("TermDate"), Date)
        '	If termdatemonth.Year < Date.Today.Year Then
        '		'No need to call SP prasad jadhav 2012.04.30 (Reopen)YRS 5.0-1530 - Need ability to reactivate a terminated employee
        '		Session("Flag") = "Reactivate"
        '		'Changed by prasad
        '		MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.TERMINATION_DATE_PRIOR_MONTH_YES_NO, MessageBoxButtons.YesNo)
        '		Exit Sub
        '		'Added by prasad 2012.04.30 (Reopen)YRS 5.0-1530 - Need ability to reactivate a terminated employee
        '	ElseIf termdatemonth.Year = Date.Today.Year And termdatemonth.Month < Date.Today.Month Then
        '		Session("Flag") = "Reactivate"
        '		MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.TERMINATION_DATE_PRIOR_MONTH_YES_NO, MessageBoxButtons.YesNo)
        '		Exit Sub
        '	Else
        '		updateInfo()
        '	End If
        'End If

        '''Priya 25-April2012\
        ''updateInfo()

        '

        'BS:2012.06.19:BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
        'please check save button enabled or disabled
        If ButtonSaveParticipant.Enabled Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.SAVE_CHANGES, MessageBoxButtons.Stop)
            Exit Sub
        End If

        Dim YmcaId, FundEventId As String
        Dim empAdd As FetchEmploymentAdditional = EmpReactivation()

        'from Client Side validation then update into database
        If hd_emp_reactive.Value = "True" AndAlso hd_tdcontractexist.Value = String.Empty Then
            UpdateFundEventEmpElectives(empAdd.Termdate, empAdd.FundEventId, empAdd.YmcaId, False, empAdd.newFundStatusCode, empAdd.newFundStatusDesc)
            Exit Sub
        ElseIf hd_emp_reactive.Value = "True" AndAlso hd_tdcontractexist.Value = "True" Then
            UpdateFundEventEmpElectives(empAdd.Termdate, empAdd.FundEventId, empAdd.YmcaId, empAdd.tdcontractexist, empAdd.newFundStatusCode, empAdd.newFundStatusDesc)
            Exit Sub
        End If


        'Server side validation if client script failed
        'BS:2012.06.26:BT:991:YRS 5.0-1530:-if different ymca exist on same termination date then display prompt:“Multiple employment records exist with the same termination date. Please contact IT for Data Correction.”
        If empAdd.diff_ymcaexist Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.DIFFRENT_YMCA_EXIST_ON_SAME_TERMDATE, MessageBoxButtons.Stop)
            Exit Sub
        End If

        If empAdd.recordexist Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.RECORD_EXIST, MessageBoxButtons.Stop)
            Exit Sub
        End If

        'Check client side hidden field blank but server side variable exist then display server side prompt
        If hd_emp_reactive.Value = String.Empty AndAlso empAdd.emp_reactive = True Then
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.EMPLOYEE_REACTIVATION, MessageBoxButtons.YesNo)
            Session("Flag") = "IsEmpReactivate"
            'BS:2012.06.21:-reopen :BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
            If empAdd.tdcontractexist = True Then
                Session("IsTDContract") = "True"
            End If
            Exit Sub
        End If
    End Sub


    'Added by prasad for:BT:1018:YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
    <System.Web.Services.WebMethod()> _
    Public Shared Function PrintForm() As String
        Dim Participant As New ParticipantsInformation
        Dim checkSecurity As String
        'Start Anudeep:28.11.2012 - added for Bt-1026/YRS 5.0-1629 : Web frontend letters to IDM.
        'Dim IDM As New IDMforAll
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String
        'End Anudeep:28.11.2012 - added for Bt-1026/YRS 5.0-1629 : Web frontend letters to IDM.
        Try
            checkSecurity = SecurityCheck.Check_Authorization("ButtonWebFrontEndPrint", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                Return "1"
            End If
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

        Catch
            Throw
        Finally
            l_stringDocType = Nothing
            l_StringReportName = Nothing
            l_ArrListParamValues = Nothing
            l_string_OutputFileType = Nothing
            Participant = Nothing
            'IDM = Nothing
            'END : Anudeep:28.11.2012 - added for Bt-1026/YRS 5.0-1629 : Web frontend letters to IDM. 
        End Try
    End Function
    'Added by prasad for:BT:1018:YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
    <System.Web.Services.WebMethod()> _
    Public Shared Function DeleteRecord() As String
        Try
            Dim returnmessage As String
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonWebFrontEndReset", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                Return "You are not authorized to view data"
            End If
            returnmessage = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.deleteRecord(HttpContext.Current.Session("PersId"))
            Return returnmessage
        Catch ex As Exception
            HelperFunctions.LogException("WebFrontEnd", ex)
        End Try
    End Function
    'BS:2012.03.14:YRS 5.0-1470:BT:951:-open verify popup

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
    Public Sub RefreshPrimaryTelephoneDetail(ByVal dtContacts As DataTable)
        '    'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from participant and put on address control
        '    'If dsAddress IsNot Nothing AndAlso HelperFunctions.isNonEmpty(dsAddress.Tables("AddressInfo")) Then

        '    '	If (dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "System.DBNull" And dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "") Then
        '    '		If Convert.ToBoolean(dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) = True Then
        '    '			CheckboxIsBadAddress.Checked = True
        '    '		Else
        '    '			CheckboxIsBadAddress.Checked = False
        '    '		End If
        '    '	End If

        '    'End If
        '    'Telephone

        '    'Code Commnetd By Dinesh Kanojia Contacts
        If HelperFunctions.isNonEmpty(dtContacts) Then
            Dim l_datarow As DataRow
            For Each l_datarow In dtContacts.Rows
                If (l_datarow.IsNull("PhoneType") = False AndAlso l_datarow("PhoneType").ToString() <> "") Then
                    Select Case l_datarow("PhoneType").ToString.ToUpper.Trim()
                        Case "HOME"
                            'lblHomeContact.Text = l_datarow("PhoneNumber").ToString().Trim
                            TextBoxHome.Text = l_datarow("PhoneNumber").ToString().Trim
                        Case "OFFICE"
                            'lblOfficeContact.Text = l_datarow("PhoneNumber").ToString().Trim
                            TextBoxTelephone.Text = l_datarow("PhoneNumber").ToString().Trim
                        Case "FAX"
                            'lblFax.Text = l_datarow("PhoneNumber").ToString().Trim
                            TextBoxFax.Text = l_datarow("PhoneNumber").ToString().Trim
                        Case "MOBILE"
                            'lblMobile.Text = l_datarow("PhoneNumber").ToString().Trim
                            TextBoxMobile.Text = l_datarow("PhoneNumber").ToString().Trim
                    End Select
                End If
            Next
        End If


        'If dtContacts IsNot Nothing AndAlso dtContacts.Rows.Count > 0 Then
        '    Dim l_datarow As DataRow
        '    For Each l_datarow In dtContacts.Rows
        '        If (l_datarow.IsNull("PhoneType") = False AndAlso l_datarow("PhoneType").ToString() <> "") Then
        '            Select Case l_datarow("PhoneType").ToString.ToUpper.Trim()
        '                Case "HOME"
        '                    lblHomeContact.Text += l_datarow("PhoneNumber").ToString().Trim + ","
        '                Case "OFFICE"
        '                    lblOfficeContact.Text += l_datarow("PhoneNumber").ToString().Trim + ","
        '                Case "FAX"
        '                    lblFax.Text += l_datarow("PhoneNumber").ToString().Trim + ","
        '                Case "MOBILE"
        '                    lblMobile.Text += l_datarow("PhoneNumber").ToString().Trim + ","
        '            End Select
        '			End If
        '    Next
        '    lblHomeContact.Text = "2564289623, 2568818812"
        '    lblOfficeContact.Text = "2564289622, 2568818710"
        '    'lblHomeContact.Text = lblHomeContact.Text.Substring(0, lblHomeContact.Text.Length - 1)
        '    'lblOfficeContact.Text = lblOfficeContact.Text.Substring(0, lblOfficeContact.Text.Length - 1)
        '    lblFax.Text = lblFax.Text.Substring(0, lblFax.Text.Length - 1)
        '    lblMobile.Text = lblMobile.Text.Substring(0, lblMobile.Text.Length - 1)

        '		End If

    End Sub
    Public Sub RefreshSecTelephoneDetail(ByVal dtContacts As DataTable)

        '    'BS:2012:04.24:YRS 5.0-1470:BT:951:BadAddress remove from participant and put on address control
        '    'If Not dsAddress Is Nothing Then
        '    '	If HelperFunctions.isNonEmpty(dsAddress.Tables("AddressInfo")) Then
        '    '		If (dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "System.DBNull" And dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress").ToString() <> "") Then
        '    '			If Convert.ToBoolean(dsAddress.Tables("AddressInfo").Rows(0).Item("bitBadAddress")) = True Then
        '    '				Me.CheckboxSecIsBadAddress.Checked = True
        '    '			Else
        '    '				Me.CheckboxSecIsBadAddress.Checked = False
        '    '			End If
        '    '		End If
        '    '	End If
        '    '	'Address_DropDownListState()
        '    'End If
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

    Private Sub CheckboxBadEmail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxBadEmail.CheckedChanged
        EnableEmail()
    End Sub
    Public Sub EnableEmail()
        If CheckboxBadEmail.Checked Then
            Me.TextBoxEmailId.Enabled = False
            LabelEmailId.Enabled = False
        Else
            Me.TextBoxEmailId.Enabled = True
            LabelEmailId.Enabled = True
        End If
    End Sub

    Private Sub InitializeAttributesOfServerControls()
        'AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
        'Me.ButtonAddItemNotes.Attributes.Add("onclick", "javascript:return CheckAccessNotes('ButtonAddItemNotes');")
        'Shashi Shekhar:2009-12-23:Commented old call and added new call to eliminate the conflict of checksecurity access between server side and client side.
        '---------------------------------------------------------------------------------------------------------------------------------------------
        'Me.ButtonSaveParticipant.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonSaveParticipant');")
        'Me.ButtonSaveParticipant.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonSaveParticipant.ID))
        'Me.ButtonPOA.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonPOA');")
        'Anudeep:07.11.2013:BT:2190-YRS 5.0-2199: Commented below code for not checking security with javascript.
        'Me.ButtonPOA.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonPOA.ID))
        'Me.ButtonEditSSno.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonEditSSno');")
        Me.ButtonEditSSno.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonEditSSno.ID))
        'Me.ButtonEditDOB.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonEditDOB');")
        Me.ButtonEditDOB.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonEditDOB.ID))
        'Me.ButtonDeathNotification.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonDeathNotification');")
        Me.ButtonDeathNotification.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonDeathNotification.ID))
        'Me.ButtonEdit.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonEdit');")
        Me.ButtonEdit.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonEdit.ID))
        'Me.ButtonEditAddress.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonEditAddress');")
        Me.ButtonEditAddress.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonEditAddress.ID))
        'Me.ButtonAddItem.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonAddItem');")
        'AA:2014.08.13: BT-1409-Miscellaneous Issues for Security Rights
        'Me.ButtonAddItem.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonAddItem.ID))
        'Me.ButtonAccountUpdateItem.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonAccountUpdateItem');")
        Me.ButtonAccountUpdateItem.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonAccountUpdateItem.ID))
        'Me.ButtonAddActive.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonAddActive');")

        ' Me.ButtonUpdateEmployment.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonUpdateEmployment');")
        Me.ButtonUpdateEmployment.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonUpdateEmployment.ID))
        'Me.ButtonEditActive.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonEditActive');")
        Me.ButtonEditActive.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonEditActive.ID))
        'Me.ButtonDeleteActive.Attributes.Add("onClick", "javascript:return CheckAccess('ButtonDeleteActive');") 'NP:BT-631:2008.10.17 - This check was added so that other validations are not called from this button. Alternatively, the button should have causesValidation property set to False.
        Me.ButtonDeleteActive.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonDeleteActive.ID))
        'Me.ButtonEditEmploymentService.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonEditEmploymentService');")
        Me.ButtonEditEmploymentService.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonEditEmploymentService.ID))
        'Shashi Shekhar:2010-01-27
        'Me.ButtonGetArchiveDataBack.Attributes.Add("onClick", "javascript:return CheckAccess('ButtonGetArchiveDataBack');") 'Shashi Shekhar:2009.09.03 - This check was added so that other validations are not called from this button. Alternatively, the button should have causesValidation property set to False.
        Me.ButtonGetArchiveDataBack.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, ButtonGetArchiveDataBack.ID))
        '-----------------------------------------------------------------------------------------------------------------------------------------------------
        'Me.ButtonSaveParticipant.Attributes.Add("onclick", "javascript:return CheckAccessParticipantSave('ButtonSaveParticipant');")
        Me.ButtonEditQDRO.Attributes.Add("onclick", "javascript:return CheckAccess1('ButtonEditQDRO');")
        'Me.ButtonEdit.Attributes.Add("onclick", "javascript:return CheckAccess('ButtonEdit');")
        'anita may 9th
        'Ashutosh Patil as on 06-Mar-2007
        'YREN-3099
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3028,YREN-3029
        'Me.TextBoxCity.Attributes.Add("OnKeyPress", "javascript:ValidateAlphaNumericForCity();")
        'Me.TextBoxSecCity.Attributes.Add("OnKeyPress", "javascript:ValidateAlphaNumericForCity();")
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
        ' End Ashutosh Patil
        'Added By Ashutosh Patil as on 25-Jan-2007 YREN-3028,YREN-3029
        'regExpZipCode.Enabled = False
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3028,YREN-3029
        'Call SetAttributes(Me.TextBoxZip, Me.DropdownlistCountry.SelectedValue.ToString)
        'Call SetAttributes(Me.TextBoxSecZip, Me.DropdownlistSecCountry.SelectedValue.ToString)
        'Shubhrata YREN 2779 Oct 30th 2006
        'ButtonEditQDRO.Attributes.Add("onclick", "javascript:return NewWindow('AddEditQDRO.aspx','mywin','730','500','yes','center');")
        'Me.ButtonSaveParticipant.Attributes.Add("onclick", "javascript:return _OnBlur_TextBoxSSNo();")
        Me.TextBoxSpousealWiver.Attributes.Add("onchange", "javascript:EnableControls();")
        'Start:Commented by Anudeep:12.03.2013-For Telephone Usercontrol
        Me.TextBoxTelephone.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxHome.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxFax.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxMobile.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxSecTelephone.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxSecHome.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxSecFax.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")
        Me.TextBoxSecMobile.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")

        'Start:Anudeep:08.04.2013 Bt1555:YRS 5.0-1769:Length of phone numbers
        'Dim l_dataset_SecAddressInfo, l_dataset_AddressInfo As DataSet
        'l_dataset_SecAddressInfo = Session("Ds_SecondaryAddress")
        'l_dataset_AddressInfo = Session("Ds_PrimaryAddress")
        'If HelperFunctions.isNonEmpty(l_dataset_AddressInfo) Then
        '    Dim Country As String = l_dataset_AddressInfo.Tables("AddressInfo").Rows(0)("Country").ToString()
        '    If Not (Country = "US" Or Country = "CA") Then
        '        TextBoxTelephone.MaxLength = 14
        '        TextBoxHome.MaxLength = 14
        '        TextBoxFax.MaxLength = 14
        '        TextBoxMobile.MaxLength = 14
        '        Me.TextBoxTelephone.Attributes.Remove("onblur")
        '        Me.TextBoxHome.Attributes.Remove("onblur")
        '        Me.TextBoxFax.Attributes.Remove("onblur")
        '        Me.TextBoxMobile.Attributes.Remove("onblur")
        '    Else
        'TextBoxTelephone.MaxLength = 10
        'TextBoxHome.MaxLength = 10
        'TextBoxFax.MaxLength = 10
        'TextBoxMobile.MaxLength = 10
        'Me.TextBoxTelephone.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxTelephone,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        'Me.TextBoxHome.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxHome,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        'Me.TextBoxFax.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxFax,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
        'Me.TextBoxMobile.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxMobile,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")

        Me.TextBoxTelephone.Attributes.Add("onclick", "javascript:return SetMaxLengthPhone(document.Form1.all.TextBoxTelephone,document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value);")
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
        'End:Commented by Anudeep:12.03.2013-For Telephone Usercontrol
        Me.TextBoxEmailId.Attributes.Add("onblur", "javascript:ValidateForm();")
        'BS:2012.01.16:YRS 5.0-1497 -Add edit button to modify the date of death:- Security Checking of control
        Me.btnEditDeathDatePer.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, btnEditDeathDatePer.ID))
        'Shashi Shekhar:09- feb 2011: For YRS 5.0-1236 : Need ability to freeze/lock account
        Me.btnAcctLockEdit.Attributes.Add("onclick", String.Format(HelperFunctions.SecurityCheckString, btnAcctLockEdit.ID))
        TextBoxDate.Attributes.Add("onChange", "if (typeof(Page_ClientValidate) == 'function') { if (Page_ClientValidate() == false) { SetTextBox(this, '" & Now.ToShortDateString() & "'); return false; } }")
        'TextBoxDeceasedDate.Attributes.Add("onChange", " getAge();")

        'DInesh.k 2013.11.11 - YRS 5.0-2165:RMD enhancements. 
        'Add validation for numeric
        Me.tblRMDTAX.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")

    End Sub

    Private Sub InitializeMartialStatusDropDownList()
        'added by hafiz on 2-May-2007 for YREN-3112
        Dim l_dataset_MaritalTypes As DataSet
        l_dataset_MaritalTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.MaritalTypes(0)
        Me.DropDownListMaritalStatus.DataSource = l_dataset_MaritalTypes.Tables(0)
        Me.DropDownListMaritalStatus.DataTextField = "Description"
        Me.DropDownListMaritalStatus.DataValueField = "Code"
        Me.DropDownListMaritalStatus.DataBind()
        'added by hafiz on 2-May-2007 for YREN-3112
    End Sub

    Private Sub InitializeGenderTypesDropDownList()
        'Start : added by Dilip yadav on 2009.09.08 for YRS 5.0-852
        Dim l_dataset_GenderTypes As DataSet
        l_dataset_GenderTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.GenderTypes()
        If HelperFunctions.isNonEmpty(l_dataset_GenderTypes) Then
            Me.DropDownListGender.DataSource = l_dataset_GenderTypes.Tables(0)
            Me.DropDownListGender.DataTextField = "Description"
            Me.DropDownListGender.DataValueField = "Code"
            Me.DropDownListGender.DataBind()
        End If
        'End :  by Dilip yadav on 2009.09.08 for YRS 5.0-852
    End Sub
#Region "Termination Watcher"

    'SR:2012.08.17 - BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program
    Private Sub ShowHideTerminationWatcherButton()
        Dim blnTerminationWatcher As Boolean
        'Dim g_dataset_dsMemberInfo As New DataSet
        'Start: Anudeep:04.12.2012 Changed Code to show the termination watcher button who have plan balances
        Dim dsPlanType As New DataSet
        'Start : Commented By Dineshk: 2013.02.04: BT-1649 changes merege for 12.9.2 patch.

        'Commented By Anudeep: 2013.01.29: BT-1649
        'g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetTerminationWatcher(TextBoxSSNo.Text, Session("FundNo").ToString())
        'If g_dataset_dsMemberInfo.Tables(0).Rows.Count > 0 Then
        '    Session("FundId") = g_dataset_dsMemberInfo.Tables(0).Rows(0)("FundUniqueId").ToString().Trim
        'End If
        If Not String.IsNullOrEmpty(Session("FundId")) Then
            dsPlanType = YMCARET.YmcaBusinessObject.TerminationWatcherBO.GetApplicantPlanType(Session("FundId").ToString().Trim())
            'Anudeep:25.03.2014-BT:957: YRS 5.0-1484 -Added for not occuring error.
            If HelperFunctions.isNonEmpty(dsPlanType) Then
                BindPlanType(dsPlanType)
                ButtonTerminationWatcher.Visible = True
            Else
                ButtonTerminationWatcher.Visible = False
            End If
        Else
            ButtonTerminationWatcher.Visible = False
        End If
        'End: Anudeep:04.12.2012 Changed Code to show the termination watcher button who have plan balances
        'End : Commented By Dineshk: 2013.02.04: BT-1649 changes merege for 12.9.2 patch.
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function LoadTerminationWatchers() As String
        Dim g_dataset_dsMemberInfo As New DataSet
        Try
            g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetTerminationWatcher(String.Empty, HttpContext.Current.Session("FundNo").ToString())
            If g_dataset_dsMemberInfo.Tables(0).Rows.Count > 0 Then
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Termination Watcher", ex)
        End Try
    End Function
    <System.Web.Services.WebMethod()> _
    Public Shared Function SaveTerminationWatcher(ByVal Type As String, ByVal PlanType As String, ByVal isImportant As String, ByVal Notes As String) As String
        Dim strSuccess As String = String.Empty
        Try
            strSuccess = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertTerminationWatcher(HttpContext.Current.Session("PersId").ToString(), HttpContext.Current.Session("FundId").ToString(), Type, PlanType, Notes, isImportant)
            Return strSuccess
            'If strSuccess = 1 Then
            '    'Session("TerminationWatcher") = "True"
            '    'MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "Termination Watcher added successfully.", MessageBoxButtons.OK, True)
            'End If
        Catch ex As Exception
            HelperFunctions.LogException("Termination Watcher", ex)
        End Try
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function Binddata() As PersonDetails()
        Try
            'Dim dt As New DataTable()
            Dim details As New List(Of PersonDetails)()
            Dim g_dataset_dsMemberInfo As New DataSet

            g_dataset_dsMemberInfo = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetPersonTerminationWatcherDetail(HttpContext.Current.Session("PersId").ToString(), HttpContext.Current.Session("FundId").ToString())
            If g_dataset_dsMemberInfo.Tables(0).Rows.Count > 0 Then
                For Each dtrow As DataRow In g_dataset_dsMemberInfo.Tables(0).Rows
                    Dim person As New PersonDetails()

                    person.Type = dtrow("Type").ToString()
                    person.PlanType = dtrow("PlanType").ToString()
                    person.Notes = dtrow("Notes").ToString()
                    person.Status = dtrow("Status").ToString() 'AA:01.04.2014 -BT:2464 - Added a column for sisplaying status termination watcher
                    If dtrow("Important").ToString() <> "" Then
                        person.Important = dtrow("Important").ToString()
                    Else
                        person.Important = "0"
                    End If

                    details.Add(person)
                Next

            End If

            Return details.ToArray()
        Catch ex As SqlException
            Throw
        End Try
    End Function

    Private Sub BindPersonalTerminationWatcherDetails()
        Dim dt As New DataTable()
        'Anudeep:06-nov-2012 Changes made For Process Report
        dt.Columns.Add("Watch Type")
        dt.Columns.Add("Plan")
        dt.Columns.Add("Notes")
        dt.Columns.Add("Status") 'AA:01.04.2014 -BT:2464 - Added a column for sisplaying status termination watcher
        'dt.Columns.Add("Important")
        dt.Rows.Add()
        gvPersonDetails.DataSource = dt
        gvPersonDetails.DataBind()

        gvPersonDetails.Rows(0).Visible = False
    End Sub

    Private Sub BindPlanType(ByVal dsPlantype As DataSet)
        Dim blnTerminationWatcher As Boolean
        'dsPlanType = 'YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetPlanType()
        'Anudeep:04.12.2012 Commented Code to show the termination watcher button who have plan balances
        'dsPlanType = YMCARET.YmcaBusinessObject.TerminationWatcherBO.GetApplicantPlanType(Session("FundId").ToString().Trim())
        ddlPlanType.DataSource = dsPlantype '.Tables("GetPlanType")
        ddlPlanType.DataTextField = "PlanType"
        ddlPlanType.DataValueField = "PlanType".ToUpper()
        ddlPlanType.DataBind()
        If dsPlantype.Tables(0).Rows.Count = 2 Then
            ddlPlanType.Items.Insert(2, New ListItem("BOTH", "BOTH"))
        End If
    End Sub
    'Ends, SR:2012.08.17 - BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program
#End Region
    'Start : Anudeep:28.11.2012 - added for Bt-1026/YRS 5.0-1629 : Web frontend letters to IDM.
    Private Function CopyToIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String) As String
        Dim l_StringErrorMessage As String
        Dim IDM As New IDMforAll
        Try
            'Anudeep 04.12.2012 Code changes to copy report into IDM folder
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

            'Start: Anudeep 04.12.2012 Code changes to copy report into IDM folder
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
            'End: Anudeep 04.12.2012 Code changes to copy report into IDM folder
            Return l_StringErrorMessage

        Catch
            Throw
        Finally
            IDM = Nothing
        End Try
    End Function
    'End : Anudeep:28.11.2012 - added for Bt-1026/YRS 5.0-1629 : Web frontend letters to IDM.
    'Start: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form
    'Commented by anudeep 08.05.2013-to not to show report in participant maintanance
#Region "Death Notification"
    ''Saves the js annuities information into deathbenefitapplicationform table
    'Public Function SaveBeneficiaryOptions() As String
    '    Dim strdecsdPersID As String
    '    Dim strdecsdFundeventID As String
    '    Dim StrBeneficiaryName As String
    '    Dim strBeneficiarySSNo As String
    '    Dim strBeneficiaryType As String
    '    Dim decJSAnnuityAmount As Decimal
    '    Dim decRetPlan As Decimal
    '    Dim decPrincipalGuaranteeAnnuity_RP As Decimal
    '    Dim decSavPlan As Decimal
    '    Dim decPrincipalGuaranteeAnnuity_SP As Decimal
    '    Dim decDeathBenefit As Decimal
    '    Dim decAnnuityMFromRP As Decimal
    '    Dim decFirstMAnnuityFromRP As Decimal
    '    Dim decAnnuityCFromRP As Decimal
    '    Dim decFirstCAnnuityFromRP As Decimal
    '    Dim decLumpSumFromNonHumanBen As Decimal
    '    Dim decAnnuityMFromSP As Decimal
    '    Dim decFirstMAnnuityFromSP As Decimal
    '    Dim decAnnuityCFromSP As Decimal
    '    Dim decFirstCAnnuityFromSP As Decimal
    '    Dim decAnnuityMFromRDB As Decimal
    '    Dim decFirstMAnnuityFromRDB As Decimal
    '    Dim decAnnuityFromJSAndRDB As Decimal
    '    Dim decFirstAnnuityFromJSAndRDB As Decimal
    '    Dim decAnnuityMFromResRemainingOfRP As Decimal
    '    Dim decFirstMAnnuityFromResRemainingOfRP As Decimal
    '    Dim decAnnuityCFromResRemainingOfRP As Decimal
    '    Dim decFirstCAnnuityFromResRemainingOfRP As Decimal
    '    Dim decAnnuityMFromResRemainingOfSP As Decimal
    '    Dim decFirstMAnnuityFromResRemainingOfSP As Decimal
    '    Dim decAnnuityCFromResRemainingOfSP As Decimal
    '    Dim decFirstCAnnuityFromResRemainingOfSP As Decimal
    '    Dim intMonths As Integer
    '    'Initially all sections are set to false
    '    Dim blnActiveDeathBenfit As Boolean = False
    '    Dim blnSection1Visible As Boolean = False
    '    Dim blnSection2Visible As Boolean = False
    '    Dim blnSection3Visible As Boolean = False
    '    Dim blnSection4Visible As Boolean = False
    '    Dim blnSection5Visible As Boolean = False
    '    Dim blnSection6Visible As Boolean = False
    '    Dim blnSection7Visible As Boolean = False
    '    Dim blnSection8Visible As Boolean = False
    '    Dim blnSection9Visible As Boolean = False
    '    Dim blnSection10Visible As Boolean = False
    '    Dim blnSection11Visible As Boolean = False
    '    Dim blnSection12Visible As Boolean = False
    '    Dim strReturnStatus As String
    '    Dim l_ds_jsdata As DataSet



    '    Try
    '        'Getting the persid and fundevent id from session variables
    '        strdecsdPersID = Session("PersId").ToString().Trim
    '        strdecsdFundeventID = Session("FundId").ToString().Trim
    '        'get the js name and js amount from database and storing in variables
    '        l_ds_jsdata = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.PopulateJSAnnuities(strdecsdFundeventID)
    '        If HelperFunctions.isNonEmpty(l_ds_jsdata) Then
    '            StrBeneficiaryName = IIf(l_ds_jsdata.Tables(0).Rows(0)("Output_txtFirstName").ToString().Trim() = "&nbsp;", "", l_ds_jsdata.Tables(0).Rows(0)("Output_txtFirstName").ToString().Trim()) + " " + IIf(l_ds_jsdata.Tables(0).Rows(0)("Output_txtLastName").ToString().Trim() = "&nbsp;", "", l_ds_jsdata.Tables(0).Rows(0)("Output_txtLastName").ToString().Trim())
    '            If Not String.IsNullOrEmpty(l_ds_jsdata.Tables(0).Rows(0)("Output_txtTotalMonthlyAnnuityAmount").ToString()) Or l_ds_jsdata.Tables(0).Rows(0)("Output_txtTotalMonthlyAnnuityAmount").ToString() <> "&nbsp;" Then
    '                decJSAnnuityAmount = decJSAnnuityAmount + l_ds_jsdata.Tables(0).Rows(0)("Output_txtTotalMonthlyAnnuityAmount")
    '            End If
    '        End If
    '        ' storing details in database 
    '        strReturnStatus = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.SaveDeathBenefitCalculatorFormDetails(strdecsdPersID, strdecsdFundeventID, _
    '        StrBeneficiaryName, _
    '        decJSAnnuityAmount, _
    '        decRetPlan, _
    '        decPrincipalGuaranteeAnnuity_RP, _
    '        decSavPlan, _
    '        decPrincipalGuaranteeAnnuity_SP, _
    '        decDeathBenefit, _
    '        decAnnuityMFromRP, _
    '        decFirstMAnnuityFromRP, _
    '        decAnnuityCFromRP, _
    '        decFirstCAnnuityFromRP, _
    '        decLumpSumFromNonHumanBen, _
    '        decAnnuityMFromSP, _
    '        decFirstMAnnuityFromSP, _
    '        decAnnuityCFromSP, _
    '        decFirstCAnnuityFromSP, _
    '        decAnnuityMFromRDB, _
    '        decFirstMAnnuityFromRDB, _
    '        decAnnuityFromJSAndRDB, _
    '        decFirstAnnuityFromJSAndRDB, _
    '        decAnnuityMFromResRemainingOfRP, _
    '        decFirstMAnnuityFromResRemainingOfRP, _
    '        decAnnuityCFromResRemainingOfRP, _
    '        decFirstCAnnuityFromResRemainingOfRP, _
    '        decAnnuityMFromResRemainingOfSP, _
    '        decFirstMAnnuityFromResRemainingOfSP, _
    '        decAnnuityCFromResRemainingOfSP, _
    '        decFirstCAnnuityFromResRemainingOfSP, _
    '        intMonths, _
    '        blnActiveDeathBenfit,
    '        blnSection1Visible, _
    '        blnSection2Visible, _
    '        blnSection3Visible, _
    '        blnSection4Visible, _
    '        blnSection5Visible, _
    '        blnSection6Visible, _
    '        blnSection7Visible, _
    '        blnSection8Visible, _
    '        blnSection9Visible, _
    '        blnSection10Visible, _
    '        blnSection11Visible, _
    '        blnSection12Visible)
    '        'from database it returns the appid
    '        Return strReturnStatus
    '    Catch
    '        Throw
    '    End Try

    'End Function

    ''It gets the details which has selected forms and details by users and save into database
    'Public Sub SaveFormDetails()
    '    Dim intDBAppFormID As Integer = 0
    '    Dim intMetaDBAdditionalFormID As Integer
    '    Dim chvAdditionalText As String
    '    Dim strFormlist As String
    '    Dim strForms() As String
    '    Dim chvText As String
    '    Dim intUniqueID As Integer
    '    Dim dtDeathBenefitFormReqdDocs As New DataTable
    '    Dim drDeathBenefitFormReqdDocs As DataRow
    '    Dim DataSet_AdditionalForms As DataSet
    '    Try

    '        dtDeathBenefitFormReqdDocs.Columns.Add("intDBAppFormID")
    '        dtDeathBenefitFormReqdDocs.Columns.Add("intMetaDBAdditionalFormID")
    '        dtDeathBenefitFormReqdDocs.Columns.Add("chvAdditionalText")
    '        'Gets the intDBFormId From the Main table after process
    '        intDBAppFormID = Convert.ToInt32(SaveBeneficiaryOptions())


    '        If Not Session("Formlist") Is Nothing And Session("Formlist") <> "" Then
    '            DataSet_AdditionalForms = Session("DataSet_AdditionalForms")
    '            'Gets the form uniqueid For form "A copy of the death certificate for" to add User name in additional text for this form
    '            For i As Integer = 0 To DataSet_AdditionalForms.Tables(0).Rows.Count - 1
    '                If DataSet_AdditionalForms.Tables(0).Rows(i).Item("chvText").ToString().Trim() = "A copy of the death certificate for" Then
    '                    intUniqueID = DataSet_AdditionalForms.Tables(0).Rows(i).Item("intUniqueID")
    '                    Exit For
    '                End If
    '            Next
    '            'gets the concatinated string from session and store into variable
    '            strFormlist = Session("Formlist")
    '            If strFormlist.Contains("$$") Then
    '                strForms = strFormlist.Split("$$")
    '            Else
    '                ReDim strForms(0)
    '                strForms(0) = strFormlist
    '            End If
    '            ' Splits and extract the form id and additional text and inserts into the datatable
    '            For i As Integer = 0 To strForms.Length - 1
    '                If Not strForms(i) = "" Then
    '                    If strForms(i).Contains(",") Then
    '                        intMetaDBAdditionalFormID = Convert.ToInt32(strForms(i).Substring(0, strForms(i).IndexOf(",")))

    '                        If intMetaDBAdditionalFormID = intUniqueID Then
    '                            'concatinates the user name with additional text for the respective unique id
    '                            chvAdditionalText = TextBoxFirst.Text + " " + TextBoxLast.Text

    '                            chvAdditionalText = chvAdditionalText + "," + strForms(i).Substring(strForms(i).IndexOf(",") + 1, strForms(i).Length - (strForms(i).IndexOf(",") + 1))
    '                        Else
    '                            chvAdditionalText = strForms(i).Substring(strForms(i).IndexOf(",") + 1, strForms(i).Length - (strForms(i).IndexOf(",") + 1))
    '                        End If
    '                    Else
    '                        intMetaDBAdditionalFormID = Convert.ToInt32(strForms(i))
    '                        If intMetaDBAdditionalFormID = intUniqueID Then
    '                            'concatinates the user name with additional text for the respective unique id
    '                            chvAdditionalText = TextBoxFirst.Text + " " + TextBoxLast.Text

    '                        Else
    '                            chvAdditionalText = ""
    '                        End If
    '                    End If
    '                    ' the extracted values are stored in datarow and inserted in atable
    '                    drDeathBenefitFormReqdDocs = dtDeathBenefitFormReqdDocs.NewRow()
    '                    drDeathBenefitFormReqdDocs("intDBAppFormID") = intDBAppFormID
    '                    drDeathBenefitFormReqdDocs("intMetaDBAdditionalFormID") = intMetaDBAdditionalFormID
    '                    drDeathBenefitFormReqdDocs("chvAdditionalText") = chvAdditionalText
    '                    dtDeathBenefitFormReqdDocs.Rows.Add(drDeathBenefitFormReqdDocs)
    '                End If
    '            Next
    '            'Finnaly datatable is sent to database for storing form details
    '            If dtDeathBenefitFormReqdDocs.Rows.Count <> 0 Then
    '                YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.SaveDeathFormDetails(dtDeathBenefitFormReqdDocs)
    '            End If
    '        End If
    '        'To view the death benefit applicaion form and store in idm folder as pdf files
    '        ViewDeathBenefitFormAndCopyIdm(intDBAppFormID)

    '    Catch
    '        Throw
    '    Finally
    '        Session("Formlist") = Nothing
    '        Session("ShowDeathBenefitForm") = Nothing
    '        Session("DataSet_AdditionalForms") = Nothing
    '        dtDeathBenefitFormReqdDocs = Nothing
    '    End Try
    'End Sub
    ''Calling report viewer and exportinf into pdf and storing idx values
    'Public Sub ViewDeathBenefitFormAndCopyIdm(ByVal intDBAppFormID As Integer)
    '    Dim l_stringDocType As String = String.Empty
    '    Dim l_StringReportName As String = String.Empty
    '    Dim l_ArrListParamValues As New ArrayList
    '    Dim l_string_OutputFileType As String = String.Empty
    '    Dim l_StringErrorMessage As String = String.Empty
    '    Try

    '        Session("strReportName_1") = "Death Benefit Application"
    '        Session("intDBAppFormID") = intDBAppFormID
    '        'Calling reportviewer_1.aspx to open report in another window and not to overlap with previous report
    '        Dim popupScript As String = "<script language='javascript'>" & _
    '        "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp_db', " & _
    '        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
    '        "</script>"
    '        If (Not Me.IsStartupScriptRegistered("PopupScriptdb")) Then
    '            Page.RegisterStartupScript("PopupScriptdb", popupScript)
    '        End If

    '        l_stringDocType = "07DTHBENAP"
    '        l_StringReportName = "Death Benefit Application"
    '        l_ArrListParamValues.Add(intDBAppFormID)
    '        l_string_OutputFileType = "DeathBenefit_" + l_stringDocType
    '        'Copies report into idm convert into pdf and stores information idx 
    '        l_StringErrorMessage = CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
    '        If Not l_StringErrorMessage Is String.Empty Then
    '            MessageBox.Show(PlaceHolder1, "IDM Error", l_StringErrorMessage, MessageBoxButtons.Stop, False)
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Sub
#End Region
    'End: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form

#Region "    -     Commented Code By Dinesh Kanojia 12/12/12     -      "

#Region "  ***   EmployeeMentTab   ***  "
    'Private Sub ButtonUpdateEmployment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUpdateEmployment.Click
    '    Try

    '        'Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
    '        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonUpdateEmployment", Convert.ToInt32(Session("LoggedUserKey")))
    '        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
    '        HiddenFieldDirty.Value = "true"

    '        If Not checkSecurity.Equals("True") Then
    '            MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
    '            Exit Sub
    '        End If
    '        'End : YRS 5.0-940

    '        ButtonSaveParticipant.Enabled = True
    '        ButtonCancel.Enabled = True
    '        ButtonOK.Enabled = False
    '        Session("DateValidation") = ""
    '        Me.MakeLinkVisible()
    '        'Vipul 01Feb06 Cache-Session
    '        'Dim Cache As CacheManager
    '        'Cache = CacheFactory.GetCacheManager()

    '        'Modified By Ashutosh Patil as on 29-Mar-2007
    '        'YREN-3203,YREN-3205

    '        If Me.DataGridParticipantEmployment.SelectedIndex <> -1 Then
    '            Session("Flag") = "EditEmployment"
    '            'Session("UniqueIdP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(1).Text
    '            Session("HireDateP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_HireDate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(2).Text
    '            If Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_Termdate).Text <> "&nbsp;" Then  'If Me.DataGridParticipantEmployment.SelectedItem.Cells(3).Text <> "&nbsp;" Then
    '                Session("TermDateP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_Termdate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(3).Text
    '            Else
    '                Session("TermDateP") = ""
    '            End If
    '            If Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_EligibilityDate).Text <> "&nbsp;" Then 'If Me.DataGridParticipantEmployment.SelectedItem.Cells(4).Text <> "&nbsp;" Then
    '                Session("EligibilityDateP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_EligibilityDate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(4).Text
    '            Else
    '                Session("EligibilityDateP") = ""
    '            End If
    '            If Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_Professional).Text <> "&nbsp;" Then 'If Me.DataGridParticipantEmployment.SelectedItem.Cells(5).Text <> "&nbsp;" Then
    '                Session("ProfessionalP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_Professional).Text
    '            Else
    '                Session("ProfessionalP") = "False"
    '            End If

    '            '------------------------------------------------------------------------------------------------------------------
    '            'Shashi Shekhar: 2010-12-14: For BT-700 Unable to change Hire and enrollment date for Retired Active participant
    '            If Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_Salaried).Text <> "&nbsp;" Then 'Me.DataGridParticipantEmployment.SelectedItem.Cells(6).Text <> "&nbsp;" Then
    '                'Added & Commented by Dilip yadav : 11-Nov-2009 : YRS 5.0.941 : To enable 4 fields
    '                'Session("ExemptP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_Active).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(6).Text
    '                Session("ExemptP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_Salaried).Text
    '                '------------------------------------------------------------------------------------------------------
    '            Else
    '                Session("ExemptP") = "False"
    '            End If
    '            If Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_FullTime).Text <> "&nbsp;" Then 'Me.DataGridParticipantEmployment.SelectedItem.Cells(7).Text <> "&nbsp;" Then
    '                Session("FullTimeP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_FullTime).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(7).Text
    '            Else
    '                Session("FullTimeP") = "False"
    '            End If

    '            If Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_PriorService).Text <> "&nbsp;" Then 'Me.DataGridParticipantEmployment.SelectedItem.Cells(8).Text <> "&nbsp;" Then
    '                Session("PriorServiceP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_PriorService).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(8).Text
    '            Else
    '                Session("PriorServiceP") = "0"
    '            End If
    '            'Commented & Added by Dilip yadav : 13-Nov-2009 : YRS 5.0.941 : BT 1024
    '            'Session("StatusTypeP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_StatusType).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(9).Text
    '            Session("StatusTypeP") = Trim(Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_StatusType).Text)

    '            'Added by DILIP YADAV for issue id yrs 5.0.941
    '            'Session("StatusTypeP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_StatusType).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(9).Text
    '            Session("StatusTypeP") = Trim(Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_StatusType).Text)
    '            Session("StatusDateP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_StatusDate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(10).Text

    '            Session("YmcaNameP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_YmcaName).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(12).Text
    '            'Added by Dilip yadav : 13-Nov-2009 : YRS 5.0.941 : BT 1024
    '            Session("PositionTypeP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_PositionType).Text
    '            Session("PositionDescP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_PositionDesc).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(15).Text

    '            If Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_BasicPaymentDate).Text() <> "&nbsp;" Then 'If Me.DataGridParticipantEmployment.SelectedItem.Cells(16).Text() <> "&nbsp;" Then
    '                Session("BasicPaymentDateP") = Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_BasicPaymentDate).Text 'Me.DataGridParticipantEmployment.SelectedItem.Cells(16).Text
    '            Else
    '                Session("BasicPaymentDateP") = ""
    '            End If



    '            _icounter = Session("icounter")
    '            _icounter = _icounter + 1
    '            Session("icounter") = _icounter
    '            If (_icounter = 1) Then
    '                Dim popupScript As String

    '                If Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_UniqueId).Text = "&nbsp;" Then 'If Me.DataGridParticipantEmployment.SelectedItem.Cells(1).Text = "&nbsp;" Then
    '                    popupScript = "<script language='javascript'>" & _
    '                 "window.open('AddEmployment.aspx?Index=" & Me.DataGridParticipantEmployment.SelectedIndex & "', 'CustomPopUp_UpdateEmp', " & _
    '                 "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
    '                 "</script>"
    '                Else
    '                    '     popupScript = "<script language='javascript'>" & _
    '                    '"window.open('AddEmployment.aspx?UniqueIdP=" + Me.DataGridParticipantEmployment.SelectedItem.Cells(1).Text + "','CustomPopUp_UpdateEmp', " & _
    '                    '"'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
    '                    '"</script>"
    '                    ' End If
    '                    popupScript = "<script language='javascript'>" & _
    '               "window.open('AddEmployment.aspx?UniqueIdP=" + Me.DataGridParticipantEmployment.SelectedItem.Cells(m_const_int_UniqueId).Text + "','CustomPopUp_UpdateEmp', " & _
    '               "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
    '               "</script>"
    '                End If

    '                Page.RegisterStartupScript("PopupScript_UpdateEmployment", popupScript)
    '            Else

    '                'Vipul 01Feb06 Cache-Session
    '                'Me.DataGridParticipantEmployment.DataSource = CType(Cache("l_dataset_Employment"), DataSet)
    '                'viewstate("Ds_Sort_employment") = CType(Cache("l_dataset_Employment"), DataSet)
    '                Me.DataGridParticipantEmployment.DataSource = DirectCast(Session("l_dataset_Employment"), DataSet)
    '                ViewState("Ds_Sort_employment") = DirectCast(Session("l_dataset_Employment"), DataSet)
    '                'Vipul 01Feb06 Cache-Session

    '                Me.DataGridParticipantEmployment.DataBind()
    '                _icounter = 0
    '                Session("icounter") = _icounter
    '            End If
    '        Else
    '            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select an item in the grid.", MessageBoxButtons.OK)
    '            Exit Sub
    '        End If
    '    Catch sqlEx As SqlException

    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)


    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        ExceptionPolicy.HandleException(ex, "Exception Policy")
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try

    'End Sub


    'Private Sub DataGridParticipantEmployment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridParticipantEmployment.SelectedIndexChanged
    '    Try
    '        Dim i As Integer
    '        For i = 0 To Me.DataGridParticipantEmployment.Items.Count - 1
    '            Dim l_button_Select As ImageButton
    '            l_button_Select = DataGridParticipantEmployment.Items(i).FindControl("ImageButtonEmployment")
    '            If Not l_button_Select Is Nothing Then
    '                If i = DataGridParticipantEmployment.SelectedIndex Then
    '                    l_button_Select.ImageUrl = "images\selected.gif"
    '                Else
    '                    l_button_Select.ImageUrl = "images\select.gif"
    '                End If
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        ExceptionPolicy.HandleException(ex, "Exception Policy")
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
#End Region

#Region "  ***   VoluntaryTab   ***  "
    ''Private Sub ButtonUpdateItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUpdateItem.Click

    ''    Try
    ''        ButtonSaveParticipant.Enabled = True
    ''        ButtonCancel.Enabled = True
    ''        ButtonOK.Enabled = False
    ''        Dim Cache As CacheManager
    ''        Cache = CacheFactory.GetCacheManager()
    ''        If Me.DataGridAdditionalAccounts.SelectedIndex <> -1 Then
    ''            Session("Flag") = "EditAccounts"
    ''            Session("AccountType") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(4).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(4).Text.Trim)
    ''            Session("BasisCode") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(7).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(7).Text.Trim)
    ''            Session("Contribution%") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(8).Text), "0.00", DataGridAdditionalAccounts.SelectedItem.Cells(8).Text.Trim)
    ''            Session("ContributionAmt") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(9).Text), "0.00", DataGridAdditionalAccounts.SelectedItem.Cells(9).Text.Trim)
    ''            Session("EffectiveDate") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(10).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(10).Text.Trim)
    ''            Session("TerminationDate") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(11).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(11).Text.Trim)



    ''            _icounter = Session("icounter")
    ''            _icounter = _icounter + 1
    ''            Session("_icounter") = _icounter
    ''            Dim popupScript As String
    ''            If (_icounter = 1) Then
    ''                If Me.DataGridAdditionalAccounts.SelectedItem.Cells(1).Text = "&nbsp;" Then
    ''                    popupScript = "<script language='javascript'>" & _
    ''                 "window.open('AddAdditionalaccounts.aspx?Index=" & Me.DataGridAdditionalAccounts.SelectedIndex & "', 'CustomPopUp', " & _
    ''                 "'width=750, height=450, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
    ''                 "</script>"
    ''                Else
    ''                    popupScript = "<script language='javascript'>" & _
    ''               "window.open('AddAdditionalaccounts.aspx?UniqueId=" + Me.DataGridAdditionalAccounts.SelectedItem.Cells(1).Text + "','CustomPopUp', " & _
    ''               "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
    ''               "</script>"
    ''                End If

    ''                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
    ''                    Page.RegisterStartupScript("PopupScript2", popupScript)

    ''                End If
    ''            Else
    ''                Dim l_dataset_AddAccount As DataSet
    ''                l_dataset_AddAccount = Cache("l_dataset_AddAccount")
    ''                Me.DataGridAdditionalAccounts.DataSource = l_dataset_AddAccount
    ''                Me.DataGridAdditionalAccounts.DataBind()
    ''                _icounter = 0
    ''                Session("icounter") = _icounter
    ''            End If
    ''        Else
    ''            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a record to be updated.", MessageBoxButtons.OK)
    ''            Exit Sub
    ''        End If
    ''    Catch sqlEx As SqlException

    ''        Dim l_String_Exception_Message As String
    ''        l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString().trim)
    ''        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)


    ''    Catch ex As Exception
    ''        Dim l_String_Exception_Message As String
    ''        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    ''        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    ''    End Try

    ''End Sub

    'Private Sub DataGridAdditionalAccounts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAdditionalAccounts.SelectedIndexChanged
    '    Try
    '        'rahul 01 Mar 06
    '        '_icounter = 0
    '        'Session("icounter") = _icounter
    '        'rahul 01 Mar 06
    '        Dim i As Integer
    '        For i = 0 To Me.DataGridAdditionalAccounts.Items.Count - 1
    '            Dim l_button_Select As ImageButton
    '            l_button_Select = DataGridAdditionalAccounts.Items(i).FindControl("ImageButtonAccounts")
    '            If Not l_button_Select Is Nothing Then
    '                If i = DataGridAdditionalAccounts.SelectedIndex Then
    '                    l_button_Select.ImageUrl = "images\selected.gif"
    '                Else
    '                    l_button_Select.ImageUrl = "images\select.gif"
    '                End If
    '            End If

    '        Next

    '        'rahul 01 Mar 06
    '        'ButtonSaveParticipant.Enabled = True
    '        'ButtonCancel.Enabled = True
    '        'ButtonOK.Enabled = False

    '        'Session("Flag") = "EditAccounts"
    '        'Session("AccountType") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(4).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(4).Text.Trim)
    '        'Session("BasisCode") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(7).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(7).Text.Trim)
    '        'Session("Contribution%") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(8).Text), "0.00", DataGridAdditionalAccounts.SelectedItem.Cells(8).Text.Trim)
    '        'Session("ContributionAmt") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(9).Text), "0.00", DataGridAdditionalAccounts.SelectedItem.Cells(9).Text.Trim)
    '        'Session("EffectiveDate") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(10).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(10).Text.Trim)
    '        'Session("TerminationDate") = IIf(IsDBNull(DataGridAdditionalAccounts.SelectedItem.Cells(11).Text), "", DataGridAdditionalAccounts.SelectedItem.Cells(11).Text.Trim)


    '        '_icounter = Session("icounter")
    '        '_icounter = _icounter + 1
    '        'Session("_icounter") = _icounter

    '        'Dim popupScript As String
    '        'If (_icounter = 1) Then
    '        '    If Me.DataGridAdditionalAccounts.SelectedItem.Cells(1).Text = "&nbsp;" Then
    '        '        popupScript = "<script language='javascript'>" & _
    '        '     "window.open('AddAdditionalaccounts.aspx?Index=" & Me.DataGridAdditionalAccounts.SelectedIndex & "', 'CustomPopUp', " & _
    '        '     "'width=750, height=450, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
    '        '     "</script>"
    '        '    Else
    '        '        popupScript = "<script language='javascript'>" & _
    '        '   "window.open('AddAdditionalaccounts.aspx?UniqueId=" + Me.DataGridAdditionalAccounts.SelectedItem.Cells(1).Text + "','CustomPopUp', " & _
    '        '   "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
    '        '   "</script>"
    '        '    End If

    '        '    If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
    '        '        Page.RegisterStartupScript("PopupScript2", popupScript)

    '        '    End If
    '        'Else
    '        '    Dim l_dataset_AddAccount As DataSet

    '        '    'Vipul 01Feb06 Cache-Session
    '        '    'l_dataset_AddAccount = Cache("l_dataset_AddAccount")
    '        '    l_dataset_AddAccount = Session("l_dataset_AddAccount")
    '        '    'Vipul 01Feb06 Cache-Session

    '        '    Me.DataGridAdditionalAccounts.DataSource = l_dataset_AddAccount
    '        '    viewstate("DS_Sort_AddnlAccts") = l_dataset_AddAccount
    '        '    Me.DataGridAdditionalAccounts.DataBind()
    '        '    _icounter = 0
    '        '    Session("icounter") = _icounter
    '        'End If
    '        'rahul 01 Mar 06
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        ExceptionPolicy.HandleException(ex, "Exception Policy")
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
#End Region

#Region "  ***   BeneficiaryTab   ***  "
    'Private Sub ButtonEditActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEditActive.Click
    '    Try
    '        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
    '        HiddenFieldDirty.Value = "true"
    '        ''Added by neeraj on 16-Nov-2009 : issue is YRS 5.0-940 security Check
    '        'Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditActive", Convert.ToInt32(Session("LoggedUserKey")))

    '        'If Not checkSecurity.Equals("True") Then
    '        '    MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
    '        '    Exit Sub
    '        'End If
    '        ''End : YRS 5.0-940

    '        'Vipul 01Feb06 Cache-Session
    '        'Dim Cache As CacheManager
    '        'Cache = CacheFactory.GetCacheManager()
    '        'Vipul 01Feb06 Cache-Session

    '        If Me.DataGridActiveBeneficiaries.SelectedIndex <> -1 Then
    '            Session("Flag") = "EditBeneficiaries"
    '            Session("Name") = IIf(IsDBNull(DataGridActiveBeneficiaries.SelectedItem.Cells(5).Text), "", DataGridActiveBeneficiaries.SelectedItem.Cells(5).Text.Trim)
    '            Session("Name2") = IIf(IsDBNull(DataGridActiveBeneficiaries.SelectedItem.Cells(6).Text), "", DataGridActiveBeneficiaries.SelectedItem.Cells(6).Text.Trim)
    '            Session("TaxID") = IIf(IsDBNull(DataGridActiveBeneficiaries.SelectedItem.Cells(7).Text), "", DataGridActiveBeneficiaries.SelectedItem.Cells(7).Text.Trim)
    '            Session("Rel") = IIf(IsDBNull(DataGridActiveBeneficiaries.SelectedItem.Cells(8).Text), "", DataGridActiveBeneficiaries.SelectedItem.Cells(8).Text.Trim)
    '            Session("Birthdate") = IIf(IsDBNull(DataGridActiveBeneficiaries.SelectedItem.Cells(9).Text), "", DataGridActiveBeneficiaries.SelectedItem.Cells(9).Text.Trim)
    '            Session("Groups") = IIf(IsDBNull(DataGridActiveBeneficiaries.SelectedItem.Cells(11).Text), "", DataGridActiveBeneficiaries.SelectedItem.Cells(11).Text.Trim)
    '            Session("Lvl") = IIf(IsDBNull(DataGridActiveBeneficiaries.SelectedItem.Cells(12).Text), "", DataGridActiveBeneficiaries.SelectedItem.Cells(12).Text.Trim)
    '            Session("Pct") = IIf(IsDBNull(DataGridActiveBeneficiaries.SelectedItem.Cells(15).Text), "", DataGridActiveBeneficiaries.SelectedItem.Cells(15).Text.Trim)
    '            Session("Type") = IIf(IsDBNull(DataGridActiveBeneficiaries.SelectedItem.Cells(10).Text), "", DataGridActiveBeneficiaries.SelectedItem.Cells(10).Text.Trim)
    '            ''_icounter = Session("icounter")
    '            ''_icounter = _icounter + 1
    '            ''Session("_icounter") = _icounter

    '            'NP - 2007.02.27 - Found &nbsp; in textboxes on the Add Beneficiary page when going in to edit an existing one
    '            ' The Bound Datagrid contains a space character by default if nothing else exists. If this is the case our Session variables are initialized with &nbsp; instead of string.empty.
    '            If Session("Name") = "&nbsp;" Then Session("Name") = ""
    '            If Session("Name2") = "&nbsp;" Then Session("Name2") = ""
    '            If Session("TaxID") = "&nbsp;" Then Session("TaxID") = ""
    '            If Session("Rel") = "&nbsp;" Then Session("Rel") = ""
    '            If Session("Birthdate") = "&nbsp;" Then Session("Birthdate") = ""
    '            If Session("Groups") = "&nbsp;" Then Session("Groups") = ""
    '            If Session("Lvl") = "&nbsp;" Then Session("Lvl") = ""
    '            If Session("Pct") = "&nbsp;" Then Session("Pct") = ""
    '            If Session("Type") = "&nbsp;" Then Session("Type") = ""
    '            'END Changes by NP - 2007.02.27

    '            Session("MaritalStatus") = DropDownListMaritalStatus.SelectedValue 'YRPS-4704

    '            Dim popupScript As String
    '            ''If (_icounter = 1) Then
    '            If Me.DataGridActiveBeneficiaries.SelectedItem.Cells(1).Text = "&nbsp;" Then

    '                popupScript = "<script language='javascript'>" & _
    '             "window.open('UpdateBeneficiaries.aspx?Index=" & Me.DataGridActiveBeneficiaries.SelectedIndex & "', 'CustomPopUp', " & _
    '             "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
    '             "</script>"
    '            Else

    '                popupScript = "<script language='javascript'>" & _
    '           "window.open('UpdateBeneficiaries.aspx?UniqueId=" + Me.DataGridActiveBeneficiaries.SelectedItem.Cells(1).Text + "','CustomPopUp', " & _
    '           "'width=750, height=500, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
    '           "</script>"
    '            End If

    '            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
    '                Page.RegisterStartupScript("PopupScript2", popupScript)

    '            End If
    '            '''Else

    '            '''    _icounter = 0
    '            '''    Session("icounter") = _icounter
    '            '''End If
    '        Else
    '            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a Beneficiary record to be updated.", MessageBoxButtons.OK)
    '            Exit Sub
    '        End If
    '        'NP:PS:2007.08.07 - The enable button code is moved here so that they are enabled only when the person is in edit mode.
    '        ButtonSaveParticipant.Enabled = True
    '        ButtonCancel.Enabled = True
    '        ButtonOK.Enabled = False
    '        Me.MakeLinkVisible()

    '    Catch sqlEx As SqlException

    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)


    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        ExceptionPolicy.HandleException(ex, "Exception Policy")
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try

    'End Sub
    'Private Sub ButtonDeleteActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDeleteActive.Click
    '    Try
    '        'Added by prasad Jadhav on 26-Aug-2011 :For BT-895,YRS 5.0-1364 : prompt user to if changes not saved
    '        HiddenFieldDirty.Value = "true"
    '        Session("Flag") = "DeleteBeneficiaries"
    '        Me.MakeLinkVisible()

    '        If Me.DataGridActiveBeneficiaries.SelectedIndex <> -1 Then
    '            Dim Beneficiaries As DataSet

    '            'Vipul 01Feb06 Cache-Session
    '            'Dim l_CacheManager As CacheManager
    '            'l_CacheManager = CacheFactory.GetCacheManager()

    '            Dim drRows As DataRow()
    '            Dim drUpdated As DataRow

    '            If Me.DataGridActiveBeneficiaries.SelectedItem.Cells(1).Text <> "&nbsp;" Then

    '                'Vipul 01Feb06 Cache-Session
    '                'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)
    '                Beneficiaries = CType(Session("BeneficiariesActive"), DataSet)
    '                'Vipul 01Feb06 Cache-Session

    '                Dim l_UniqueId As String
    '                l_UniqueId = Me.DataGridActiveBeneficiaries.SelectedItem.Cells(1).Text
    '                If Not IsNothing(Beneficiaries) Then
    '                    drRows = Beneficiaries.Tables(0).Select("UniqueID='" & l_UniqueId & "'")
    '                    drUpdated = drRows(0)
    '                    drUpdated.Delete()

    '                    'Vipul 01Feb06 Cache-Session
    '                    'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)

    '                    Session("BeneficiariesActive") = Beneficiaries
    '                    'Vipul 01Feb06 Cache-Session

    '                End If
    '            End If

    '            If Me.DataGridActiveBeneficiaries.SelectedItem.Cells(1).Text = "&nbsp;" Then

    '                'Vipul 01Feb06 Cache-Session
    '                'Beneficiaries = CType(l_CacheManager("BeneficiariesActive"), DataSet)
    '                Beneficiaries = DirectCast(Session("BeneficiariesActive"), DataSet)
    '                'Vipul 01Feb06 Cache-Session

    '                If Not IsNothing(Beneficiaries) Then

    '                    drUpdated = Beneficiaries.Tables(0).Rows(Me.DataGridActiveBeneficiaries.SelectedIndex)
    '                    drUpdated.Delete()
    '                    'Vipul 01Feb06 Cache-Session
    '                    'l_CacheManager.Add("BeneficiariesActive", Beneficiaries)

    '                    Session("BeneficiariesActive") = Beneficiaries
    '                    'Vipul 01Feb06 Cache-Session

    '                End If
    '            End If
    '            LoadBeneficiariesTab()
    '            Session("Flag") = ""
    '        Else
    '            MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select a record to be deleted.", MessageBoxButtons.Stop)
    '            Exit Sub
    '        End If
    '        Me.DataGridActiveBeneficiaries.SelectedIndex -= 1 'aparna 30/11/2007
    '        'NP:PS:2007.08.07 - The enable button code is moved here so that they are enabled only when the person is in edit mode.
    '        ButtonOK.Enabled = False
    '        ButtonSaveParticipant.Enabled = True
    '        ButtonCancel.Enabled = True

    '    Catch sqlEx As SqlException

    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(sqlEx.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?DBMessage=" + l_String_Exception_Message)


    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        ExceptionPolicy.HandleException(ex, "Exception Policy")
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub

    'Private Sub DataGridActiveBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridActiveBeneficiaries.SelectedIndexChanged
    '    Try
    '        Dim i As Integer
    '        For i = 0 To Me.DataGridActiveBeneficiaries.Items.Count - 1
    '            Dim l_button_Select As ImageButton
    '            l_button_Select = DataGridActiveBeneficiaries.Items(i).FindControl("Imagebutton1")
    '            If Not l_button_Select Is Nothing Then
    '                If i = DataGridActiveBeneficiaries.SelectedIndex Then
    '                    l_button_Select.ImageUrl = "images\selected.gif"
    '                Else
    '                    l_button_Select.ImageUrl = "images\select.gif"
    '                End If
    '            End If
    '        Next

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        ExceptionPolicy.HandleException(ex, "Exception Policy")
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
#End Region

#End Region
    'Start:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues
    Private Sub btnEditPrimaryContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditPrimaryContact.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditAddress", Convert.ToInt32(Session("LoggedUserKey")))
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
        Me.ButtonSaveParticipant.Enabled = True
        'Me.ButtonSaveParticipants.Visible = True
        'Me.ButtonSaveParticipants.Enabled = True
        Me.CheckboxBadEmail.Enabled = True
        Me.CheckboxUnsubscribe.Enabled = True
        Me.CheckboxTextOnly.Enabled = True
        Me.TextBoxEmailId.Enabled = True
        Me.TextBoxEmailId.ReadOnly = False
        Me.LabelEmailId.Enabled = True
        Me.ButtonCancel.Enabled = True
        Me.MakeLinkVisible()
    End Sub

    Private Sub btnEditSecondaryContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditSecondaryContact.Click
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditAddress", Convert.ToInt32(Session("LoggedUserKey")))
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
        Me.ButtonSaveParticipant.Enabled = True
        Me.ButtonSaveParticipant.Visible = False
        Me.ButtonSaveParticipants.Visible = True
        Me.ButtonSaveParticipants.Enabled = True
        Me.ButtonCancel.Enabled = True
        Me.MakeLinkVisible()
        'Start:Anudeep-2013.08.21 - YRS 5.0-2162:Edit button for email address 
        Me.LabelEmailId.Enabled = True
        Me.CheckboxBadEmail.Enabled = True
        Me.CheckboxUnsubscribe.Enabled = True
        Me.CheckboxTextOnly.Enabled = True
        Me.TextBoxEmailId.Enabled = True
        Me.TextBoxEmailId.ReadOnly = False
        'End:Anudeep-2013.08.21 - YRS 5.0-2162:Edit button for email address 
    End Sub

    Public Sub EditAddressAndContacts()
        Dim l_datatable_ContactsInfo As DataTable
        Dim l_dataset_SecContactInfo As DataTable
        'End : YRS 5.0-940
        'start:BS:2012.04.26:YRS 5.0-1470: && restructure Address control
        'Me.TextBoxEffDate.Enabled = True
        'Me.TextBoxSecEffDate.Enabled = True
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3028,YREN-3029
        'TextBoxAddress1.ReadOnly = False
        'TextBoxAddress2.ReadOnly = False
        'TextBoxAddress3.ReadOnly = False
        'TextBoxCity.ReadOnly = False
        'DropdownlistState.Enabled = True
        'TextBoxZip.ReadOnly = False
        'DropdownlistCountry.Enabled = True
        btnEditPrimaryContact.Visible = True
        btnEditSecondaryContact.Visible = True

        Me.AddressWebUserControl1.MakeReadonly = False
        Me.AddressWebUserControl1.EnableControls = True
        'Added by Anudeep:12.03.2013-For Telephone Usercontrol
        'Me.TelephoneWebUserControl1.EditPrimaryContact_Enabled = True
        'Me.TelephoneWebUserControl2.EditSecondaryContact_Enabled = True
        btnEditPrimaryContact.Visible = True
        btnEditSecondaryContact.Visible = True
        ButtonEditAddress.Enabled = False
        'ButtonActivateAsPrimary.Enabled = False
        'Commented By Dinesh Contacts
        TextBoxTelephone.ReadOnly = False
        TextBoxHome.ReadOnly = False
        TextBoxFax.ReadOnly = False
        TextBoxMobile.ReadOnly = False
        TextBoxSecTelephone.ReadOnly = False
        TextBoxSecHome.ReadOnly = False
        TextBoxSecFax.ReadOnly = False
        TextBoxSecMobile.ReadOnly = False
        Me.CheckboxBadEmail.Enabled = False
        Me.CheckboxUnsubscribe.Enabled = False
        Me.CheckboxTextOnly.Enabled = False
        Me.TextBoxEmailId.Enabled = False
        Me.LabelEmailId.Enabled = False
        'TextBoxEmailId.ReadOnly = False
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3028,YREN-3029
        'TextBoxAddress1.Enabled = True
        'TextBoxAddress2.Enabled = True
        'TextBoxAddress3.Enabled = True
        'TextBoxCity.Enabled = True
        'TextBoxZip.Enabled = True


        'Commented By Dinesh Kanojia Contacts

        TextBoxTelephone.Enabled = True
        TextBoxHome.Enabled = True
        TextBoxMobile.Enabled = True
        TextBoxFax.Enabled = True
        TextBoxSecTelephone.Enabled = True
        TextBoxSecHome.Enabled = True
        TextBoxSecMobile.Enabled = True
        TextBoxSecFax.Enabled = True

        'TextBoxEmailId.Enabled = True
        'LabelEmailId.Enabled = True
        'tbPricontacts.Visible = True
        'btnEditPrimaryContact.Visible = False
        'Dim dt_PrimaryContact As DataTable = DirectCast(Session("dt_PrimaryContactInfo"), DataTable)
        'RefreshPrimaryTelephoneDetail(dt_PrimaryContact)
        'Me.TelephoneWebUserControl1.Visible = False
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3028,YREN-3029
        'Me.TextBoxSecAddress1.Enabled = True
        'Me.TextBoxSecAddress2.Enabled = True
        'Me.TextBoxSecAddress3.Enabled = True
        'Me.TextBoxSecCity.Enabled = True
        'Me.DropdownlistSecCountry.Enabled = True
        '''  Me.TextBoxSecEmail.Enabled = True
        'Me.DropdownlistSecState.Enabled = True
        'Me.TextBoxSecTelephone.Enabled = True
        'TextBoxSecHome.Enabled = True
        'TextBoxSecFax.Enabled = True
        'TextBoxSecMobile.Enabled = True
        'Ashutosh Patil as on 26-Mar-2007
        'YREN-3028,YREN-3029
        'Me.TextBoxSecZip.Enabled = True
        ButtonEditAddress.Enabled = False
        'By Ashutosh Patil as on 25-Apr-2007
        'YREN-3298
        'Me.AddressWebUserControl1.SetValidationsForPrimary()
        'Dim l_ds_SecondaryAddress As DataSet
        'If Not Session("Ds_SecondaryAddress") Is Nothing Then
        '	l_ds_SecondaryAddress = CType(Session("Ds_SecondaryAddress"), DataSet)
        '	'Secondary Address section starts
        '	If l_ds_SecondaryAddress.Tables("AddressInfo").Rows.Count > 0 Then
        '		Me.ButtonActivateAsPrimary.Enabled = True
        '	End If
        'End If

        Me.AddressWebUserControl2.MakeReadonly = False
        Me.AddressWebUserControl2.EnableControls = True
        'Me.CheckboxIsBadAddress.Enabled = True
        'Me.CheckboxSecIsBadAddress.Enabled = True

        'Me.CheckboxBadEmail.Enabled = True
        ''Me.CheckboxSecBadEmail.Enabled = True
        ''Me.CheckboxSecTextOnly.Enabled = True
        ''Me.CheckboxSecUnsubscribe.Enabled = True
        'Me.CheckboxTextOnly.Enabled = True
        'Me.CheckboxUnsubscribe.Enabled = True
        'Me.TextBoxSecEffDate.Enabled = True
        'Me.TextBoxEffDate.Enabled = True
        'Me.ButtonSaveParticipant.Enabled = True
        'Me.ButtonSaveParticipant.Visible = False
        'Me.ButtonSaveParticipants.Visible = True
        'Me.ButtonSaveParticipants.Enabled = True
        'Me.ButtonCancel.Enabled = True
        Me.MakeLinkVisible()
        'By Ashutosh Patil as on 25-Apr-2007
        'YREN-3298
        'Priya 06-Sep-2010: YRS 5.0-1126:Address update for non-US/non-Canadian address.
        'Call Me.AddressWebUserControl1.SetCountStZipCodeMandatoryOnSelection()
        ValidationSummaryParticipants.Enabled = False
        ''end:BS:2012.03.14:YRS 5.0-1470:BT:951:-
        'End 06-Sep-2010: YRS 5.0-1126
        'end:BS:2012.04.26:YRS 5.0-1470: && restructure Address control
        'START: MMR | 2018.11.23 | YRS-AT-4018 | Set property value to allow address can be updated/Added or not
        If Me.PIIInformationRestrictionMessageCode <> 0 Then
            Me.AddressWebUserControl1.IsPIIInformationAllowedToChange = False
            Me.AddressWebUserControl2.IsPIIInformationAllowedToChange = False
            Me.AddressWebUserControl1.PIIInformationRestrictionMessageTextAddressControl = Me.PIIInformationRestrictionMessageText
            Me.AddressWebUserControl1.DivPIIErrorControlID = DivSuccessAndErrorMessage.ClientID
            Me.AddressWebUserControl2.PIIInformationRestrictionMessageTextAddressControl = Me.PIIInformationRestrictionMessageText
            Me.AddressWebUserControl2.DivPIIErrorControlID = DivSuccessAndErrorMessage.ClientID
        End If
        'END: MMR | 2018.11.23 | YRS-AT-4018 | Set property value to allow address can be updated/Added or not
    End Sub
    'End:Anudeep:03.4.2013:Bt-1409  Miscellaneous Issues

    'Start DInesh.k 2013.09.17 BT-2139:YRS 5.0-2165:RMD enhancements. 
    Private Sub btnEditRMDTAX_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditRMDTAX.Click
        ButtonSaveParticipant.Enabled = True
        ButtonCancel.Enabled = True
        TextBoxRMDTax.Enabled = True
        'Start:  Dinesh k           2014.05.15      BT-2537 : YRS 5.0-2370 - Change to validation on RMD tax rate 
        'Provide Decimal number validation.
        TextBoxRMDTax.Attributes.Add("onkeypress", "ValidateDecimalNumers(this);")
        'End:  Dinesh k           2014.05.15      BT-2537 : YRS 5.0-2370 - Change to validation on RMD tax rate 
        Me.HyperLinkViewRetireesInfo.Visible = False
    End Sub
    'End DInesh.k 2013.09.17 BT-2139:YRS 5.0-2165:RMD enhancements. 

    'Start:Anudeep-2013.08.21 - YRS 5.0-2162:Edit button for email address 
    Private Sub imgBtnEmail_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnEmail.Click
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonEditAddress", Convert.ToInt32(Session("LoggedUserKey")))
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
            Me.ButtonSaveParticipant.Enabled = True
            'Anudeep:26.08.2013 - YRS 5.0-2162:Edit button for email address 
            Me.ButtonSaveParticipants.Enabled = True
            Me.LabelEmailId.Enabled = True
            Me.CheckboxBadEmail.Enabled = True
            Me.CheckboxUnsubscribe.Enabled = True
            Me.CheckboxTextOnly.Enabled = True
            Me.TextBoxEmailId.Enabled = True
            Me.TextBoxEmailId.ReadOnly = False
            Me.ButtonCancel.Enabled = True
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
        Dim checkSecurity As String
        Try
            checkSecurity = SecurityCheck.Check_Authorization("ButtonPINno", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                Return checkSecurity
            End If
            arrPINdtls = HttpContext.Current.Session("PersPIN")
            If arrPINdtls IsNot Nothing Then
                strPin = arrPINdtls(1)
            End If
            Return strPin
        Catch ex As Exception
            HelperFunctions.LogException("ParticpantInformtion - GetPIN", ex)
            Return ex.Message
        End Try
    End Function
    'Updates the PIN 
    <System.Web.Services.WebMethod()> _
    Public Shared Function UpdatePIN(ByVal strPIN As String) As String
        Dim strReturnPIN As String
        Dim arrPINdtls(1) As String
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonPINno", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
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
            HelperFunctions.LogException("ParticpantInformtion - UpdatePIN", ex)
            Return ex.Message
        End Try
    End Function
    'Deletes the PIN value
    <System.Web.Services.WebMethod()> _
    Public Shared Function DeletePIN() As String
        Dim arrPINdtls(1) As String
        Dim strRetStatus As String
        Dim checkSecurity As String = SecurityCheck.Check_Authorization("ButtonPINno", Convert.ToInt32(HttpContext.Current.Session("LoggedUserKey")))
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
    'End:AA:2014.02.03 - BT:2292:YRS 5.0-2248


    'Start:AA:2014.02.03 - BT:2316:YRS 5.0 2262 : Added  below region for checking cannot locate spouse value and spousal vaiwer date and fill ing canno locate spouse date value
#Region "Cannot Locate Spouse"
    Private Sub chkCannotLocateSpouse_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCannotLocateSpouse.CheckedChanged
        Try
            ButtonSaveParticipant.Enabled = True
            ButtonSaveParticipant.Visible = True
            ButtonSaveParticipants.Enabled = False
            ButtonSaveParticipants.Visible = False
            ButtonCancel.Enabled = True
            Me.HyperLinkViewRetireesInfo.Visible = False
            If chkCannotLocateSpouse.Checked Then
                lblCannotLocateSpouse.Text = "(" + Now.Date.ToString("MM/dd/yyyy") + ")"
            ElseIf Not chkCannotLocateSpouse.Checked Then
                lblCannotLocateSpouse.Text = String.Empty
            End If
        Catch ex As Exception
            HelperFunctions.LogException("chkCannotLocateSpouse_CheckedChanged", ex)
            Throw ex
        End Try
    End Sub
    Public Function ValidateSpousalWavierAndCannotLocateSpouse() As Boolean
        Dim dsActiveBeneficiaries As New DataSet
        Dim dsGeneralInfo As New DataSet
        Dim dtBeneficiaryAddress As DataTable 'SP 2014.08.17 BT-2623:YRS 5.0-2399
        Dim strDeathDate As String = String.Empty
        Try
            Dim dRows As DataRow()
            dsActiveBeneficiaries = Session("BeneficiariesActive")
            dsGeneralInfo = ViewState("g_dataset_GeneralInfo")
            dtBeneficiaryAddress = Session("BeneficiaryAddress") 'SP 2014.08.17 BT-2623:YRS 5.0-2399
            'AA:26.03.2014 -BT:2489 :YRS 5.0-2349 - Added to check any changes has done to beneficiaries
            'AA:26.03.2014 -BT:2489 :YRS 5.0-2349 - modified spousal wavier date if 01/01/1900 exitsts treat it as empty string
            If (dsActiveBeneficiaries IsNot Nothing AndAlso dsActiveBeneficiaries.HasChanges) Or (dtBeneficiaryAddress IsNot Nothing AndAlso HelperFunctions.isNonEmpty(dtBeneficiaryAddress.GetChanges())) Or _
                (HelperFunctions.isNonEmpty(dsGeneralInfo) AndAlso _
                (dsGeneralInfo.Tables("GeneralInfo").Rows(0)("CannotLocateSpouse").ToString().Trim() <> lblCannotLocateSpouse.Text.Trim() OrElse _
                 Replace(dsGeneralInfo.Tables("GeneralInfo").Rows(0)("SpousealWaiver").ToString(), "01/01/1900", "") <> TextBoxSpousealWiver.Text.Trim() OrElse _
                 (dsGeneralInfo.Tables("GeneralInfo").Rows(0)("MaritalStatus").ToString().Trim() <> DropDownListMaritalStatus.SelectedValue.Trim() AndAlso _
                   dsGeneralInfo.Tables("GeneralInfo").Rows(0)("MaritalStatus").ToString().Trim().ToUpper = "M"))) Then
                'Start:AA:05/14/2015 BT:2680 YRS 5.0-2428: Added to check whether the particiapant is deceased or not 
                'if deceased there should be no validation for the cannot locate spouse or spousal wavier date
                strDeathDate = dsGeneralInfo.Tables("GeneralInfo").Rows(0).Item("Deathdate").ToString().Trim
                If strDeathDate.Trim <> String.Empty Then
                    Return True
                End If
                'End:AA:05/14/2015 BT:2680 YRS 5.0-2428: Added to check whether the particiapant is deceased or not 
                If (TextBoxSpousealWiver.Text.Trim() <> "" Or chkCannotLocateSpouse.Checked) Then
                    If (LabelNotSet.Visible) Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_CANNOT_LOCATE_SPOUSE_INVALID, MessageBoxButtons.Stop)
                        Return False
                    End If
                    If DropDownListMaritalStatus.SelectedValue <> "M" Then
                        MessageBox.Show(170, 350, 380, 150, PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_SPOUSALCONSENT_OTHER_THAN_MARRIED, MessageBoxButtons.Stop)
                        Return False
                    End If
                ElseIf DropDownListMaritalStatus.SelectedValue <> "M" Then
                    Return True
                End If

                If HelperFunctions.isNonEmpty(dsActiveBeneficiaries) Then
                    dRows = dsActiveBeneficiaries.Tables(0).Select("Rel='SP' And Groups = 'PRIM'")
                    If ((dRows.Length = 0) OrElse (dRows(0)("Pct") <> 100)) And _
                        ((TextBoxSpousealWiver.Text.Trim() <> "" And chkCannotLocateSpouse.Checked) Or _
                        (TextBoxSpousealWiver.Text.Trim() = "" And Not chkCannotLocateSpouse.Checked)) Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_CANNOT_LOCATE_SPOUSE_VALIDATION, MessageBoxButtons.Stop)
                        Return False
                    ElseIf ((dRows.Length > 0) AndAlso (dRows(0)("Pct") = 100)) And _
                            (TextBoxSpousealWiver.Text.Trim() <> "" Or chkCannotLocateSpouse.Checked) Then 'SP 2014.02.24 BT-2316\YRS 5.0-2262 Commenetd or condition  'Or chkCannotLocateSpouse.Checked) Then
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_SPOUSE_WITH_FULL_BENFIT, MessageBoxButtons.Stop)
                        Return False
                    End If
                ElseIf (Not LabelNotSet.Visible) And _
                    ((TextBoxSpousealWiver.Text.Trim() <> "" And chkCannotLocateSpouse.Checked) Or _
                      (TextBoxSpousealWiver.Text.Trim() = "" And Not chkCannotLocateSpouse.Checked)) Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_CANNOT_LOCATE_SPOUSE_VALIDATION, MessageBoxButtons.Stop)
                    Return False
                End If
            End If
            Return True
        Catch
            Throw
        End Try
    End Function

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
                                    Dim strFilterExpression As String = "ISNULL(BeneficiaryFirstName,'')='" + drOriginal("Name").ToString.Trim().Replace("'", "''") + "' AND BeneficiaryLastName='" + drOriginal("Name2").ToString.Trim.Replace("'", "''") + "' AND ISNULL(BeneficiaryTaxNumber,'')='" + drOriginal("TaxID").ToString.Trim + "'"
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

    Private Function GetBenficiaryRows(ByVal dtBeneficiaries As DataTable, ByVal guiBeneficiaryUniqueid As String) As DataRow()
        Return dtBeneficiaries.Select("uniqueid = '" + guiBeneficiaryUniqueid + "'")
    End Function
    'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End

#End Region
    'End:AA:2014.02.03 - BT:2316:YRS 5.0 2262 : Added  below region for checking cannot locate spouse value and spousal vaiwer date and fill ing canno locate spouse date value
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim dsBenefeciaries As DataSet
        Dim isContingentMessageExists As Boolean 'SP 2014.12.03 BT-2310\YRS 5.0-2255:
        'START : VC | 2018.08.10 | YRS-AT-4018 -  Declared variables
        Dim loanStatus As String
        Dim loanEmailStatus As String
        'END : VC | 2018.08.10 | YRS-AT-4018 -  Declared variables
        Try
            'Anudeep:21.10.2013:BT:1455:YRS 5.0-1733:Contingent Beneficiary levels
            'If TextBoxCont2A.Text IsNot String.Empty Or TextBoxCont3A.Text IsNot String.Empty Or TextBoxCont2InsA.Text IsNot String.Empty Or TextBoxCont3InsA.Text IsNot String.Empty Then
            dsBenefeciaries = Session("BeneficiariesActive")
            If HelperFunctions.isNonEmpty(dsBenefeciaries) Then
                If dsBenefeciaries.Tables(0).Select("Groups = 'CONT' And Lvl IN ('LVL2','LVL3')").Length > 0 Then
                    DivMainMessage.InnerHtml = Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_CONTINGENT_LVL2_LVL3_NOT_EXISTS
                    isContingentMessageExists = True
                Else
                    DivMainMessage.Visible = False
                End If
            Else
                DivMainMessage.Visible = False
            End If

            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-Start
            If (IsModifiedPercentage) Then
                DivMainMessage.Visible = True
                If String.IsNullOrEmpty(DivMainMessage.InnerHtml.Trim) Then
                    DivMainMessage.InnerHtml = Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DEATH_CALC_RUN
                Else
                    DivMainMessage.InnerHtml = "</br>" + Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_DEATH_CALC_RUN
                End If
            ElseIf isContingentMessageExists Then
                DivMainMessage.Visible = True
                'AA 2015.26.06      BT:2906 : YRS 5.0-2547: Commented because it already gets set in beneficiary section
                'DivMainMessage.InnerHtml = Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_CONTINGENT_LVL2_LVL3_NOT_EXISTS
            Else
                DivMainMessage.Visible = False
            End If
            'SP 2014.12.03 BT-2310\YRS 5.0-2255:-End

            'START : VC | 2018.08.10 | YRS-AT-4018 -  Code to show and hide notification div
            loanStatus = Session("LoanStatus")
            If HelperFunctions.isEmpty(loanStatus) Then
                DivSuccessAndErrorMessage.InnerHtml = ""
            End If
            Session("LoanStatus") = ""

            loanEmailStatus = Session("LoanEmailStatus")
            If HelperFunctions.isEmpty(loanEmailStatus) Then
                DivEmailSuccessErrorMessage.InnerHtml = ""
            End If
            Session("LoanEmailStatus") = ""
            'END : VC | 2018.08.10 | YRS-AT-4018 -  Code to show and hide notification div

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
            ButtonSaveParticipant.Enabled = True
            ButtonSaveParticipant.Visible = True
            ButtonSaveParticipants.Enabled = False
            ButtonSaveParticipants.Visible = False
            ButtonCancel.Enabled = True
            Me.HyperLinkViewRetireesInfo.Visible = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added freeze and unfreeze functionality
    Private Sub btnFreeze_Click(sender As Object, e As EventArgs) Handles btnFreeze.Click
        Dim decDefaultAmt, decPhantomAmt, decFrozenAmt As Decimal
        Dim dsLoandetails As DataSet
        Dim intLoanDetailsId As Integer
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            Session("Freeze_Unfreeze_Process") = "Freeze"
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_FREEZE_PHANTOMINT, MessageBoxButtons.YesNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnUnFreeze_Click(sender As Object, e As EventArgs) Handles btnUnFreeze.Click
        Dim decFrozenAmt As Decimal
        Dim dsLoandetails As DataSet
        Dim intLoanRequestId As Integer
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            Session("Freeze_Unfreeze_Process") = "Unfreeze"
            MessageBox.Show(PlaceHolder1, "YMCA-YRS", Resources.ParticipantsInformation.MESSAGE_PARTICIPANT_INFO_UNFREEZE_PHANTOMINT, MessageBoxButtons.YesNo)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub LoadFreezedetails(ByVal dsLoandetails As DataSet)
        Dim decDefaultAmt, decPhantomAmt As Decimal
        Try
            If HelperFunctions.isNonEmpty(dsLoandetails) Then
                If HelperFunctions.isNonEmpty(dsLoandetails.Tables("LoanPhantomIntDetails")) Then
                    txtDefaultAmt.Text = dsLoandetails.Tables("LoanPhantomIntDetails").Rows(0)("DefaultAmt").ToString()
                    txtPhantomInt.Text = dsLoandetails.Tables("LoanPhantomIntDetails").Rows(0)("PhantomInt").ToString()
                    txtCompFrozenAmt.Text = dsLoandetails.Tables("LoanPhantomIntDetails").Rows(0)("FrozenAmt").ToString()
                    If (txtDefaultAmt.Text.Trim <> "") Then
                        decDefaultAmt = txtDefaultAmt.Text
                    End If
                    If (txtPhantomInt.Text.Trim <> "") Then
                        decPhantomAmt = txtPhantomInt.Text
                    End If
                    txtComputedAmt.Text = (decDefaultAmt + decPhantomAmt).ToString()
                    If dsLoandetails.Tables("LoanPhantomIntDetails").Rows(0)("Freezedate").ToString.Trim <> "" Then
                        txtFrozenOn.Text = dsLoandetails.Tables("LoanPhantomIntDetails").Rows(0)("Freezedate").ToString()
                        btnFreeze.Visible = False
                        btnUnFreeze.Visible = True
                    Else
                        txtFrozenOn.Text = ""
                        btnFreeze.Visible = True
                        btnUnFreeze.Visible = False
                    End If
                Else
                    txtDefaultAmt.Text = ""
                    txtPhantomInt.Text = ""
                    txtCompFrozenAmt.Text = ""
                    txtComputedAmt.Text = ""
                    txtFrozenOn.Text = ""
                    btnFreeze.Visible = False
                    btnUnFreeze.Visible = False
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub UpdateFreezeDetails(ByRef decDefaultAmt As Decimal, ByRef decPhantomAmt As Decimal, ByRef strDateTime As String)
        Dim dsLoandetails As DataSet
        Dim intLoanDetailsId As Integer
        Dim decFrozenAmt As Decimal
        Dim intLoanRequestId As Integer
        Dim strResult As String
        Dim sender As Object
        Dim E As EventArgs
        Try
            decFrozenAmt = decDefaultAmt + decPhantomAmt
            intLoanDetailsId = CType(Session("LoandetailsId"), Integer)

            strResult = LoanClass.FreezeUnfreeze(intLoanDetailsId, decFrozenAmt, strDateTime)

            If strResult = "FREEZED" Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS Freeze Loan", Resources.ParticipantsInformation.MESSAGE_LOAN_HAS_FREEZED, MessageBoxButtons.OK)
            ElseIf strResult = "FREEZED_EMAIL_ERROR" Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS Freeze Loan", Resources.ParticipantsInformation.MESSAGE_LOAN_HAS_FREEZED_EMAIL_ERROR, MessageBoxButtons.Stop)
            ElseIf strResult = "UNFREEZED" Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS Unfreeze Loan", Resources.ParticipantsInformation.MESSAGE_LOAN_HAS_UNFREEZED, MessageBoxButtons.OK)
            ElseIf strResult = "UNFREEZED_EMAIL_ERROR" Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS Unfreeze Loan", Resources.ParticipantsInformation.MESSAGE_LOAN_HAS_UNFREEZED_EMAIL_ERROR, MessageBoxButtons.Stop)
            End If

            intLoanRequestId = CType(Session("LoanRequestIdForPayOff"), Integer)
            dsLoandetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpLoanDetails(intLoanRequestId)
            LoadFreezedetails(dsLoandetails)
            'AA:09.14.2015 YRS 5.0-2523 Load account contributions
            LoadAccountContributionsTab(sender, E)

        Catch
            Throw
        End Try
    End Sub
    'End: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added freeze and unfreeze functionality
    Public Sub LoadLoanOffsetreason(ByVal dsLoan As DataSet)
        'SR:2015.07.13 BT:2699:YRS 5.0-2441 : Added to fill the loan offset reason
        lblOffsetReason.Visible = False
        lblOffsetReasonText.Visible = False
        If Not dsLoan.Tables("LoanOffsetReason") Is Nothing Then
            If dsLoan.Tables("LoanOffsetReason").Rows.Count > 0 Then
                If Not IsDBNull(dsLoan.Tables("LoanOffsetReason").Rows(0).Item("Offsetreason")) Then
                    Me.lblOffsetReasonText.Text = dsLoan.Tables("LoanOffsetReason").Rows(0).Item("Offsetreason").ToString()
                    lblOffsetReason.Visible = True
                    lblOffsetReasonText.Visible = True
                End If
            End If
        End If
        'SR:2015.07.13 BT:2699:YRS 5.0-2441 : Added to fill the loan offset reason
    End Sub

    'START: PPP | 2015.10.10 | YRS-AT-2588 | Validates all four telephone box and retrieves error message from AtsMetaMessages
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
    End Function                        'START: PPP | 2015.10.10 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages

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
    'END: PPP | 2015.10.10 | YRS-AT-2588 | Validates all four telephone box and retrieves error message from AtsMetaMessages

    'Start: Bala: 01/05/2016: YRS-AT-1972: Special death processing required check box event.
    Private Sub chkSpecialDeathProcess_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSpecialDeathProcess.CheckedChanged
        Try
            ButtonSaveParticipant.Enabled = True
        Catch ex As Exception
            HelperFunctions.LogException("chkSpecialDeathProcess_CheckedChanged", ex)
            Throw ex
        End Try
    End Sub
    'End: Bala: 01/05/2016: YRS-AT-1972: Special death processing required check box event.

    ' // START : SB | 07/07/2016 | YRS-AT-2382 | Maintaing the Beneficiaries Record for SSN updates 
    Private Sub PrepareAuditTable()
        Dim dtSSNUpdate, dtExistingTable As DataTable
        Dim dtExistingDataSet As DataSet
        Dim iBeneficiaryCount As Integer
        Try
            If Session("AuditBeneficiariesTable") Is Nothing Then
                Session("AuditBeneficiariesTable") = CreateAuditTable()
            End If

            dtSSNUpdate = CType(Session("AuditBeneficiariesTable"), DataTable)

            If (Session("BeneficiariesActive") IsNot Nothing) Then
                dtExistingDataSet = CType(Session("BeneficiariesActive"), DataSet)
                dtExistingTable = dtExistingDataSet.Tables(0)
                iBeneficiaryCount = dtExistingTable.Rows.Count
                Dim dr As DataRow
                For Each drExisting As DataRow In dtExistingTable.Rows
                    If Not drExisting.RowState = DataRowState.Deleted AndAlso dtSSNUpdate.Select(String.Format("UniqueId='{0}'", Convert.ToString(drExisting("UniqueId")))).Length = 0 Then
                        dr = dtSSNUpdate.NewRow()
                        dr("ModuleName") = "Retire Maintenance"
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
    ' // END : SB | 07/07/2016 | YRS-AT-2382 | Maintaing the Beneficiaries Record for SSN updates 
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

    ' // START : SB | 10/13/2016 | YRS-AT-3095 |  Function to check RMD of deceased participant need to be regenerate
    Private Function IsRMDRegenerateRequired() As Boolean
        Dim IsRMDRegenerateRequiredTrueorFalse As Boolean
        Try
            IsRMDRegenerateRequiredTrueorFalse = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DeathNotifyActions_IsRMDRegenerateRequired(Session("FundId"))

            Return IsRMDRegenerateRequiredTrueorFalse

        Catch
            Throw
        End Try

    End Function
    ' // END : SB | 10/13/2016 | YRS-AT-3095 |  Function to check RMD of deceased participant need to be regenerate

    'START: MMR | 2018.04.06 | YRS-AT-3935 | Displaying payment method and Bank details for selected loan number 
    Private Sub DisplayPaymentDetailsForSelectedLoanNumber(ByVal loanPaymentTable As DataTable)
        Dim loanPaymentRow As DataRow

        loanPaymentRow = loanPaymentTable.Rows(0)
        If Convert.ToString(loanPaymentRow("PaymentMethodCode")).ToUpper = YMCAObjects.PaymentMethod.CHECK Then
            txtPaymentMethod.Text = IIf(IsDBNull(loanPaymentRow("PaymentMethodCode")), "", loanPaymentRow("PaymentMethodCode").ToString())
            lblAccountNumber.Visible = False
            lblAccountType.Visible = False
            lblBankABA.Visible = False
            lblPaymentStatus.Visible = False
            txtAccountNumber.Visible = False
            txtAccountType.Visible = False
            txtPaymentStatus.Visible = False
            txtBankABA.Visible = False
            'START: VC | 2018.09.26 | YRS-AT-4018 - Hiding bank name label and textbox
            lblBankName.Visible = False
            txtBankName.Visible = False
            'END: VC | 2018.09.26 | YRS-AT-4018 - Hiding bank name label and textbox
        ElseIf Convert.ToString(loanPaymentRow("PaymentMethodCode")).ToUpper = YMCAObjects.PaymentMethod.EFT Then
            lblAccountNumber.Visible = True
            lblAccountType.Visible = True
            lblBankABA.Visible = True
            lblPaymentStatus.Visible = True
            txtAccountNumber.Visible = True
            txtAccountType.Visible = True
            txtPaymentStatus.Visible = True
            txtBankABA.Visible = True
            'START: VC | 2018.09.26 | YRS-AT-4018 - Showing bank name area and setting bank name into textbox
            lblBankName.Visible = True
            txtBankName.Visible = True
            txtBankName.Text = IIf(IsDBNull(loanPaymentRow("BankName")), "", loanPaymentRow("BankName").ToString())
            'END: VC | 2018.09.26 | YRS-AT-4018 - Showing bank name area and setting bank name into textbox
            txtPaymentMethod.Text = IIf(IsDBNull(loanPaymentRow("PaymentMethodCode")), "", loanPaymentRow("PaymentMethodCode").ToString())
            txtAccountNumber.Text = IIf(IsDBNull(loanPaymentRow("AccountNumber")), "", loanPaymentRow("AccountNumber").ToString())
            txtAccountType.Text = IIf(IsDBNull(loanPaymentRow("AccountType")), "", loanPaymentRow("AccountType").ToString())
            txtPaymentStatus.Text = IIf(IsDBNull(loanPaymentRow("PaymentStatus")), "", loanPaymentRow("PaymentStatus").ToString())
            txtBankABA.Text = IIf(IsDBNull(loanPaymentRow("BankABANumber")), "", loanPaymentRow("BankABANumber").ToString())
        End If
    End Sub
    'END: MMR | 2018.04.06 | YRS-AT-3935 | Displaying payment method and Bank details for selected loan number 

    'START: MMR | 2018.04.06 | YRS-AT-3935 | Displaying payment method history details for selected loan number 
    Private Sub ViewPaymentMethodHistory(ByVal PaymentMethodHistoryTable As DataTable)
        'START: VC | 2018.11.05 | YRS-AT-4018 - Commented existing code and added new code to show the payment history link
        ''Show link if history details available for selected loan number and populate data in grid for display
        'If PaymentMethodHistoryTable.Rows.Count > 1 Then
        '    ShowHidePaymentMethodHistoryLink(True)
        '    gvPaymentMethodHistory.DataSource = PaymentMethodHistoryTable
        '    gvPaymentMethodHistory.DataBind()
        'Else
        '    'Hide link if no history details available for selected loan number and clear grid datasource
        '    ShowHidePaymentMethodHistoryLink(False)
        '    gvPaymentMethodHistory.DataSource = Nothing
        '    gvPaymentMethodHistory.DataBind()
        'End If
        ShowHidePaymentMethodHistoryLink(True)
        gvPaymentMethodHistory.DataSource = PaymentMethodHistoryTable
        gvPaymentMethodHistory.DataBind()
        'END: VC | 2018.11.05 | YRS-AT-4018 - Commented existing code and added new code to show the payment history link
    End Sub
    'END: MMR | 2018.04.06 | YRS-AT-3935 | Displaying payment method history details for selected loan number 

    'START: VC | 2018.10.11 | YRS-AT-4018 | Method will bind attempted date to grid from batch id
    Private Sub gvPaymentMethodHistory_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPaymentMethodHistory.RowDataBound
        Dim fileBatchID As String
        Dim row As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Dim attemptedDate As Date
        Dim paymentMethod As String
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                'Selecting batch id
                fileBatchID = Convert.ToString(row("EFTFileBatchID"))
                paymentMethod = Convert.ToString(row("PaymentMethod"))
                If Not String.IsNullOrEmpty(fileBatchID) Then
                    'Fetching attempted date from batch id
                    If fileBatchID.Length >= 8 Then
                        fileBatchID = fileBatchID.Substring(0, 8)
                        attemptedDate = DateTime.ParseExact(fileBatchID, "yyyyMMdd", Nothing)
                        'Inserting attempted date into grid
                        e.Row.Cells(mConst_gvPaymentMethodHistoryIndexOfAttemptedDate).Text = attemptedDate
                    End If
                End If
                'If payment mthod is check then set attempted date as N/A
                If paymentMethod = "CHECK" Then
                    e.Row.Cells(mConst_gvPaymentMethodHistoryIndexOfAttemptedDate).Text = "N/A"
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("gvPaymentMethodHistory_RowDataBound", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        End Try
    End Sub
    'END: VC | 2018.10.11 | YRS-AT-4018 | Method will bind attempted date to grid from batch id

    'START: MMR | 2018.04.06 | YRS-AT-3935 | Show/Hide payment method history details link
    Private Sub ShowHidePaymentMethodHistoryLink(ByVal blnFlag As Boolean)
        If blnFlag Then
            imgPaymentDetailsHistory.Style.Add("display", "normal")
        Else
            imgPaymentDetailsHistory.Style.Add("display", "none")
        End If
    End Sub
    'END: MMR | 2018.04.06 | YRS-AT-3935 | Show/Hide payment method history details link

    ' START: SB | 03/23/2017 | YRS-AT-2606 | Storing the value of Selected YMCA id  in Session Variable
    Public Property SelectedYMCAID() As String
        Get
            If Not Session("SelectedYMCAID") Is Nothing Then
                Return CType(Session("SelectedYMCAID"), String)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("SelectedYMCAID") = Value
        End Set
    End Property
    ' END: SB | 03/23/2017 | YRS-AT-2606 | Storing the value of Selected YMCA id  in Session Variable

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

    'START : VC | 2018.08.20 | YRS-AT-4018 - Method to display YRS or WEB loan details
    Private Sub LoadLoansDetails(ByVal application As String, ByVal loanNumber As String, ByVal webLoanStatus As String)
        If Application = LoanLocation.WEB Then
            ClearYRSAndWebLoanTextBoxes()
            LoadWebLoanDetails(loanNumber, webLoanStatus, application)
            trLoanAccountDetails.Visible = False
            trLoanAccountDetailsBody.Visible = False
            trPaymentHistoryHeader.Visible = False
            trPaymentHistoryBody.Visible = False
            trLoanReport.Visible = False
            HandleControlsIfStatusIsPending(webLoanStatus, application)

        Else
            LoadMaxRequestDateLoanData()
            LoadWebLoanDetails(loanNumber, webLoanStatus, application)
            trLoanAccountDetails.Visible = True
            trLoanAccountDetailsBody.Visible = True
            trPaymentHistoryHeader.Visible = True
            trPaymentHistoryBody.Visible = True
            trLoanReport.Visible = True
            txtWebDocreceivedDate.ReadOnly = True
            popCalendarDate.Enabled = False
            btnApproveWebLoan.Disabled = True
            btnDeclineWebLoan.Disabled = True
            HandleControlsIfStatusIsPending(webLoanStatus, application)

        End If
    End Sub
    'END : VC | 2018.08.20 | YRS-AT-4018 - Method to display YRS or WEB loan details

    'START : VC | 2018.09.24 | YRS-AT-4018 - Handle page controls if loan status is pending
    Private Sub HandleControlsIfStatusIsPending(ByVal webLoanStatus As String, ByVal application As String)
        'If loan status is pending then check for document received date
        If (webLoanStatus.ToUpper() = "PEND" And application.ToUpper() = "WEB") Then
            txtWebDocreceivedDate.ReadOnly = False
            popCalendarDate.Enabled = True
            If Not String.IsNullOrEmpty(txtWebDocreceivedDate.Text) Then
                btnApproveWebLoan.Disabled = False
            Else
                btnApproveWebLoan.Disabled = True
            End If
            btnDeclineWebLoan.Disabled = False
        Else
            'If loan status is not pending then buttons will be disabled
            txtWebDocreceivedDate.ReadOnly = True
            popCalendarDate.Enabled = False
            btnApproveWebLoan.Disabled = True
            btnDeclineWebLoan.Disabled = True
        End If
    End Sub
    'END : VC | 2018.09.24 | YRS-AT-4018 - Handle page controls if loan status is pending

    'START : VC | 2018.08.03 | YRS-AT-4018 -  Method to load web loan details
    Private Sub LoadWebLoanDetails(loanNumber As Integer, webLoanStatus As String, application As String)
        Dim webLoanDetails As DataSet
        Dim loanRequests As DataSet
        Dim currentRow As DataRow()
        Dim newRow As DataRow
        Dim declineReason As String

        Try

            PopulateDeclineReason()
            webLoanDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetWebLoanDetails(loanNumber)
            loanRequests = Session("l_dataset")
            If isNonEmpty(loanRequests) And loanNumber <> 0 Then
                currentRow = loanRequests.Tables(0).Select("LoanNumber = '" & loanNumber & "'")
                newRow = currentRow(0)

                If Not IsDBNull(newRow("LoanNumber")) Then
                    Me.txtWebLoanNumber.Text = newRow("LoanNumber").ToString()
                Else
                    Me.txtWebLoanNumber.Text = ""
                End If
                If Not IsDBNull(newRow("RequestedAmount")) Then
                    Me.txtWebRequestedAmount.Text = Math.Round(newRow("RequestedAmount"), 2)
                Else
                    Me.txtWebRequestedAmount.Text = ""
                End If
            End If

            If isNonEmpty(webLoanDetails) Then
                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvStatus")) Then
                    Me.txtWebApplicationStatus.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvStatus")
                Else
                    Me.txtWebApplicationStatus.Text = ""
                End If

                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("dtmLoanStatusDateChanged")) Then
                    Dim statusDateTime As Date
                    statusDateTime = webLoanDetails.Tables("LoanDetails").Rows(0).Item("dtmLoanStatusDateChanged")
                    txtWebStatusDateTime.Text = statusDateTime.ToString("MM/dd/yyyy hh:mm tt")
                Else
                    txtWebStatusDateTime.Text = ""
                End If
                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("intLoanTerm")) Then
                    txtWebRequestedTerm.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("intLoanTerm")
                Else
                    txtWebRequestedTerm.Text = ""
                End If
                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvPayrollFrequency")) Then
                    txtWebPayrollFrequency.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvPayrollFrequency")
                Else
                    txtWebPayrollFrequency.Text = ""
                End If

                SetONDType(webLoanDetails.Tables("LoanDetails").Rows(0), LoanLocation.WEB) ' VC | 2018.09.24 | YRS-AT-4018 -  Calling method to set ONDType

                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("mnyMaxLoan")) Then
                    txtMaxAvailableLoanAmount.Text = Math.Round(webLoanDetails.Tables("LoanDetails").Rows(0).Item("mnyMaxLoan"), 2)
                Else
                    txtMaxAvailableLoanAmount.Text = ""
                End If

                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("declineReason")) Then
                    declineReason = webLoanDetails.Tables("LoanDetails").Rows(0).Item("declineReason")
                Else
                    declineReason = ""
                End If

                If webLoanStatus = "DECLINED" AndAlso declineReason <> "Other" Then
                    txtWebReasonNotes.Text = declineReason
                Else
                    If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("comment")) Then
                        txtWebReasonNotes.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("comment")
                    Else
                        txtWebReasonNotes.Text = ""
                    End If
                End If
                HandleControlsIfStatusIsPending(webLoanStatus, application)

                trEFTBankInfo.Visible = False
                trEFTAccountInfo.Visible = False
                'START : SC | 2020.04.20 | YRS-AT-4853 - Display Loantype for WEB LOAN SECTION
                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("LoanType")) Then
                    txtWebLoanType.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("LoanType")
                Else
                    txtWebLoanType.Text = ""
                End If
                'END : SC | 2020.04.20 | YRS-AT-4853 - Display Loantype for WEB LOAN SECTION
                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("loanPaymentType")) Then
                    txtWebPaymentMethod.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("loanPaymentType")
                    If (webLoanDetails.Tables("LoanDetails").Rows(0).Item("loanPaymentType").ToString().ToUpper() = "EFT") Then

                        trEFTBankInfo.Visible = True
                        trEFTAccountInfo.Visible = True

                        If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvAccountType")) Then
                            txtWebAccountType.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvAccountType")
                        Else
                            txtWebAccountType.Text = ""
                        End If

                        If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvBankAccountNumber")) Then
                            txtWebAccount.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvBankAccountNumber")
                        Else
                            txtWebAccount.Text = ""
                        End If

                        If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("chrBankAbaNumber")) Then
                            txtWebBankABA.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("chrBankAbaNumber")
                        Else
                            txtWebBankABA.Text = ""
                        End If

                        If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvBankName")) Then
                            txtWebBankName.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("chvBankName")
                        Else
                            txtWebBankName.Text = ""
                        End If
                    End If
                Else
                    Me.txtWebPaymentMethod.Text = ""
                End If
                'START: PK | 01/10/2019 | YRS-AT-4241 | This line is commented for proper naming, change of txtWebMaritalStatus to txtOldWebMaritalStatus and additon of code for txtNewWebMaritalStatus
                'If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("MaritalStatus")) Then
                '    txtWebMaritalStatus.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("MaritalStatus") 
                'Else
                '    txtWebMaritalStatus.Text = ""
                'End If

                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("MaritalStatus")) Then
                    txtOldWebMaritalStatus.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("MaritalStatus").ToString().Trim
                Else
                    txtOldWebMaritalStatus.Text = ""
                End If

                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("NewMaritalStatus")) Then
                    txtNewWebMaritalStatus.Text = webLoanDetails.Tables("LoanDetails").Rows(0).Item("NewMaritalStatus").ToString().Trim
                Else
                    txtNewWebMaritalStatus.Text = ""
                End If
                'END: PK | 01/10/2019 | YRS-AT-4241 | This line is commented for proper naming, change of txtWebMaritalStatus to txtOldWebMaritalStatus and additon of code for txtNewWebMaritalStatus

                If Not IsDBNull(webLoanDetails.Tables("LoanDetails").Rows(0).Item("dtmLoanEsignDate")) Then
                    Dim applicationDateTime As Date
                    applicationDateTime = webLoanDetails.Tables("LoanDetails").Rows(0).Item("dtmLoanEsignDate")
                    txtWebApplicationDateTime.Text = applicationDateTime.ToString("MM/dd/yyyy hh:mm tt")
                Else
                    txtWebApplicationDateTime.Text = ""
                End If
                txtWebLoanPurpose.Text = "TD Loan"

            End If
            If isNonEmpty(webLoanDetails) Then
                If HelperFunctions.isNonEmpty(webLoanDetails.Tables("LoanDocumentDetails")) Then
                    If Not IsDBNull(webLoanDetails.Tables("LoanDocumentDetails").Rows(0).Item("dtmReceived")) Then
                        txtWebDocreceivedDate.Text = webLoanDetails.Tables("LoanDocumentDetails").Rows(0).Item("dtmReceived")
                    Else
                        txtWebDocreceivedDate.Text = ""
                    End If

                    If Not IsDBNull(webLoanDetails.Tables("LoanDocumentDetails").Rows(0).Item("chvDocName")) Then
                        lblWebDocreceivedDate.Text = webLoanDetails.Tables("LoanDocumentDetails").Rows(0).Item("chvDocName") + " Rec'd Date"
                    Else
                        lblWebDocreceivedDate.Text = "Rec'd Date"
                    End If

                    If Not IsDBNull(webLoanDetails.Tables("LoanDocumentDetails").Rows(0).Item("chvDocCode")) Then
                        lblWebDocCode.Text = webLoanDetails.Tables("LoanDocumentDetails").Rows(0).Item("chvDocCode")
                    Else
                        lblWebDocCode.Text = ""
                    End If
                    txtWebDocreceivedDate.Visible = True
                Else
                    txtWebDocreceivedDate.Visible = False
                End If
            End If
            If HelperFunctions.isNonEmpty(webLoanDetails.Tables("StatusHistory")) Then
                gvApplicationStatusHistory.DataSource = webLoanDetails.Tables("StatusHistory")
                gvApplicationStatusHistory.DataBind()
            End If

        Catch
            Throw
        End Try
    End Sub
    'END : VC | 2018.08.03 | YRS-AT-4018 -  Method to load web loan details

    'START : VC | 2018.10.31 | YRS-AT-4018 -  Method set authorization property of approve and decline button to viewstate
    Private Sub ApproveDeclineAuthorization()
        Dim checkSecurity As String
        Try
            ApproveConfirmationMessage = GetMessageByTextMessageNo(MESSAGE_LOAN_ADMIN_CONFIRM_APPROVE_LOAN)
            Page.ClientScript.RegisterHiddenField("hfApproveConfirmationMessage", ApproveConfirmationMessage.ToString())

            'START: Shilpa N | 03/28/2019 | YRS-AT-4248 | Commented the existing code, Corrected with the right button id to check security.  
            'checkSecurity = SecurityCheck.Check_Authorization("btnApproveWebLoan", Convert.ToInt32(Session("LoggedUserKey")))
            checkSecurity = SecurityCheck.Check_Authorization("btnApproveWebLoan@UserGroupId", Convert.ToInt32(Session("LoggedUserKey")))
            'END: Shilpa N | 03/28/2019 | YRS-AT-4248 | Commented the existing code, Corrected with the right button id to check security.
            ApproveButtonAuthorization = checkSecurity
            Page.ClientScript.RegisterHiddenField("hfApproveButtonAuthorization", ApproveButtonAuthorization.ToString())

            checkSecurity = SecurityCheck.Check_Authorization("btnDeclineWebLoan", Convert.ToInt32(Session("LoggedUserKey")))
            DeclineButtonAuthorization = checkSecurity
            Page.ClientScript.RegisterHiddenField("hfDeclineButtonAuthorization", DeclineButtonAuthorization.ToString())
            'START: VC| 2018.13.11 | YRS-AT-4018 | Setting  process in progress message into hidden field
            ProcessInProgress = GetMessageByTextMessageNo(MESSAGE_LOAN_ADMIN_LOAN_PROCESS_IN_PROGRESS)
            Page.ClientScript.RegisterHiddenField("hfProcessInProgress", ProcessInProgress.ToString())
            'END: VC| 2018.13.11 | YRS-AT-4018 | Setting  process in progress message into hidden field
        Catch
            Throw
        End Try

    End Sub
    'END : VC | 2018.10.31 | YRS-AT-4018 -  Method set authorization property of approve and decline button to viewstate

    'START : VC | 2018.09.24 | YRS-AT-4018 -  Method to set ONDType textbox value
    Private Sub SetONDType(ByVal dataRow As DataRow, ByVal application As String)
        Dim ondType As String
        If Not IsDBNull(dataRow("ONDType")) Then
            If (dataRow("ONDType") = "OND") Then
                ondType = "Yes"
            Else
                ondType = "No"
            End If
        Else
            ondType = "No"
        End If

        If application = LoanLocation.YRS Then
            txtBoxONDRequested.Text = ondType
        Else
            txtWebONDRequested.Text = ondType
        End If
    End Sub
    'END : VC | 2018.09.24 | YRS-AT-4018 -  Method to set ONDType textbox value

    'START : VC | 2018.10.24 | YRS-AT-4018 -  Method to set YRS loan status
    Private Sub SetDISBStatus(ByVal yrsStatus As String, ByVal disbStatus As String)
        If yrsStatus.ToUpper() = "DISB" AndAlso disbStatus.ToUpper() = "PROOF" Then
            TextboxLoanStatus.Text = yrsStatus + " / " + disbStatus
        Else
            TextboxLoanStatus.Text = yrsStatus
        End If
    End Sub
    'END : VC | 2018.10.24 | YRS-AT-4018 -  Method to set YRS loan status

    'START : VC | 2018.08.06 | YRS-AT-4018 -  Method to populate decline reason dropdown
    Private Sub PopulateDeclineReason()
        Dim dsReasons As DataSet
        Dim reasonDesciption As String

        If (Not Page.IsPostBack) Then
            dsReasons = YMCARET.YmcaBusinessObject.LookupsMaintenanceBOClass.SearchLookups("LACDeclineReason")
            If isNonEmpty(dsReasons) Then
                Dim dtNewReason As DataTable
                dtNewReason = New DataTable()
                dtNewReason.Columns.Add("Desc", GetType(System.String))
                dtNewReason.Rows.Add("------------Select One------------")

                For Each row As DataRow In dsReasons.Tables(0).Rows
                    reasonDesciption = row("Desc")
                    dtNewReason.Rows.Add(reasonDesciption)
                Next row
                ddlDeclineReason.DataSource = dtNewReason
                ddlDeclineReason.DataTextField = "Desc"
                ddlDeclineReason.DataValueField = "Desc"
                ddlDeclineReason.DataBind()
            End If
        End If
    End Sub
    'END : VC | 2018.08.06 | YRS-AT-4018 -  Method to populate decline reason dropdown

    'START : VC | 2018.08.03 | YRS-AT-4018 -  Method to do approve operations while clicking approve button
    Private Sub btnYesLoanApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnYesLoanApprove.Click
        Dim resultMessage As String
        Dim loanOriginationId As Integer
        Dim signatureReceivedDate As String
        Dim maritalStatus As String
        Dim newMaritalStatus As String 'PK | 01/10/2019 | YRS-AT-4241 | Method added for new marital status
        Dim docCode As String
        Dim checkSecurity As String
        Dim emailDatatable As DataTable
        Dim webLoan As YMCAObjects.WebLoan
        Dim paymentMethod As String
        Dim result As YMCAObjects.ReturnObject(Of Boolean)
        Dim participantEmail As String
        Dim persId As String
        Dim successMessage As String

        Try
            docCode = lblWebDocCode.Text
            'START: PK | 01/10/2019 | YRS-AT-4241 | This line is commented for proper naming, change of txtWebMaritalStatus to txtOldWebMaritalStatus and additon of code for txtNewWebMaritalStatus
            'maritalStatus = txtWebMaritalStatus.Text
            maritalStatus = txtOldWebMaritalStatus.Text
            newMaritalStatus = txtNewWebMaritalStatus.Text
            'END: PK | 01/10/2019 | YRS-AT-4241 | This line is commented for proper naming, change of txtWebMaritalStatus to txtOldWebMaritalStatus and additon of code for txtNewWebMaritalStatus
            signatureReceivedDate = txtWebDocreceivedDate.Text
            loanOriginationId = CType(DataGridLoans.SelectedItem.Cells(mConst_DataGridLoansIndexOfLoanNumber).Text, Integer)
            'Getting payment method
            paymentMethod = CType(DataGridLoans.SelectedItem.Cells(mConst_DataGridLoansIndexOfPaymentMethodCode).Text, String)
            'Populating webLoan object with decline details
            webLoan = ApproveLoan(loanOriginationId, signatureReceivedDate, maritalStatus, docCode, paymentMethod)
            persId = Session("PersId")
            result = New YMCAObjects.ReturnObject(Of Boolean)()
            'Getting email id of participant
            emailDatatable = DirectCast(Session("Dt_EmailAddress"), DataTable)
            If HelperFunctions.isNonEmpty(emailDatatable) Then
                If (emailDatatable.Rows(0).Item("EmailAddress").ToString() <> "System.DBNull" And emailDatatable.Rows(0).Item("EmailAddress").ToString() <> "") Then
                    participantEmail = emailDatatable.Rows(0).Item("EmailAddress").ToString().Trim
                End If
            End If
            'Approve operation 
            result = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.ApproveWebLoan(webLoan, persId, participantEmail)
            'Checking whether the MessageList count grater than zero 
            If result.MessageList.Count > 0 Then
                resultMessage = result.MessageList(0)
                'Checking whether the resultMessage contains numbers or not 
                If Integer.TryParse(resultMessage, Nothing) AndAlso result.Value = False Then
                    'To display error message
                    DivEmailSuccessErrorMessage.InnerHtml = GetMessageByTextMessageNo(resultMessage)
                    Session("LoanEmailStatus") = "error"
                    DivEmailSuccessErrorMessage.Attributes("class") = "error-msg"
                    'Display success message if approve process is success
                    If (result.MessageList.Count > 1) Then
                        DivSuccessAndErrorMessage.InnerHtml = GetMessageByTextMessageNo(MESSAGE_LOAN_ADMIN_LOAN_APPROVE_SUCCESS)
                        Session("LoanStatus") = "success"
                        DivSuccessAndErrorMessage.Attributes("class") = "success-msg"
                    End If
                Else
                    'To display success message of approve process and email sending
                    successMessage = GetMessageByTextMessageNo(MESSAGE_LOAN_ADMIN_LOAN_APPROVE_SUCCESS) + "</br>" + GetMessageByTextMessageNo(resultMessage)
                    DivSuccessAndErrorMessage.InnerHtml = successMessage
                    Session("LoanStatus") = "success"
                    DivSuccessAndErrorMessage.Attributes("class") = "success-msg"
                End If
                Session("LoanOriginationId") = loanOriginationId
                LoadLoansTab()

            End If

        Catch ex As Exception
            Dim exceptionMessage As String
            exceptionMessage = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + exceptionMessage)
        End Try
    End Sub
    'END : VC | 2018.08.03 | YRS-AT-4018 -  Method to do approve operations while clicking approve button

    'START : VC | 2018.08.03 | YRS-AT-4018 -  Method to do decline operations while clicking decline button
    Protected Sub btnConfirmDeclineWebLoan_ServerClick(sender As Object, e As EventArgs)
        Dim resultMessage As String
        Dim loanOriginationId As Integer
        Dim declineComment As String
        Dim declineReason As String
        Dim checkSecurity As String
        Dim emailDatatable As DataTable
        Dim webLoan As YMCAObjects.WebLoan
        Dim paymentMethod As String
        Dim result As YMCAObjects.ReturnObject(Of Boolean)
        Dim participantEmail As String
        Dim persId As String
        Dim successMessage As String
        Try
            declineReason = ddlDeclineReason.SelectedItem.Text
            declineComment = txtDeclineNotes.Value
            loanOriginationId = CType(DataGridLoans.SelectedItem.Cells(mConst_DataGridLoansIndexOfLoanNumber).Text, Integer)
            'Getting payment method
            paymentMethod = CType(DataGridLoans.SelectedItem.Cells(mConst_DataGridLoansIndexOfPaymentMethodCode).Text, String)
            'Populating webLoan object with decline details
            webLoan = DeclineLoan(loanOriginationId, declineComment, declineReason, paymentMethod)
            'Decline operation
            persId = Session("PersId")
            result = New YMCAObjects.ReturnObject(Of Boolean)()
            'Getting email id of participant
            emailDatatable = DirectCast(Session("Dt_EmailAddress"), DataTable)
            If HelperFunctions.isNonEmpty(emailDatatable) Then
                If (emailDatatable.Rows(0).Item("EmailAddress").ToString() <> "System.DBNull" And emailDatatable.Rows(0).Item("EmailAddress").ToString() <> "") Then
                    participantEmail = emailDatatable.Rows(0).Item("EmailAddress").ToString().Trim
                End If
            End If
            result = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.DeclineWebLoan(webLoan, persId, participantEmail)
            'Checking whether the MessageList count grater than zero 
            If result.MessageList.Count > 0 Then
                resultMessage = result.MessageList(0)
                'Checking whether the resultMessage Contains numbers
                If Integer.TryParse(resultMessage, Nothing) AndAlso result.Value = False Then
                    'To display error message
                    DivEmailSuccessErrorMessage.InnerHtml = GetMessageByTextMessageNo(resultMessage)
                    Session("LoanEmailStatus") = "error"
                    DivEmailSuccessErrorMessage.Attributes("class") = "error-msg"
                    'Display success message if decline process is success
                    If (result.MessageList.Count > 1) Then
                        DivSuccessAndErrorMessage.InnerHtml = GetMessageByTextMessageNo(MESSAGE_LOAN_ADMIN_LOAN_DECLINE_SUCCESS)
                        Session("LoanStatus") = "success"
                        DivSuccessAndErrorMessage.Attributes("class") = "success-msg"
                    End If
                Else

                    'To display success message of decline process and email sending
                    successMessage = GetMessageByTextMessageNo(MESSAGE_LOAN_ADMIN_LOAN_DECLINE_SUCCESS) + "</br>" + GetMessageByTextMessageNo(resultMessage)
                    DivSuccessAndErrorMessage.InnerHtml = successMessage
                    Session("LoanStatus") = "success"
                    DivSuccessAndErrorMessage.Attributes("class") = "success-msg"
                End If
                Session("LoanOriginationId") = loanOriginationId
                LoadLoansTab()
            End If

        Catch ex As Exception
            Dim exceptionMessage As String
            exceptionMessage = Server.UrlEncode(ex.Message.Trim.ToString())
            ExceptionPolicy.HandleException(ex, "Exception Policy")
            Response.Redirect("ErrorPageForm.aspx?Message=" + exceptionMessage)
        End Try
    End Sub
    'END : VC | 2018.08.03 | YRS-AT-4018 -  Method to do decline operations while clicking decline button

    

    'START : VC | 2018.08.06 | YRS-AT-4018 -  Method to redirect to loan admin page
    Protected Sub lnkReturnToLoanAdmin_Click(sender As Object, e As EventArgs)
        If (Session("LACTab") = "Search") Then
            Response.Redirect("SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=4&From=ParticipantsInformation", False)
        ElseIf (Session("LACTab") = "Exceptions") Then
            Response.Redirect("SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=3&From=ParticipantsInformation", False)
        Else
            Response.Redirect("SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=1", False)
        End If
        Session("LACTab") = Nothing
    End Sub
    'END : VC | 2018.08.06 | YRS-AT-4018 -  Method to redirect to loan admin page
    'START : VC | 2018.08.06 | YRS-AT-4018 -  Method to clear all textbox
    Protected Sub ClearYRSAndWebLoanTextBoxes()
        TextboxLoanNumber.Text = ""
        TextboxYmcaNo.Text = ""
        TextboxLoanAmount.Text = ""
        TextboxLoanStatus.Text = ""
        TextboxLoanApprovedDate.Text = ""
        TextboxSpousalConsentReceivedDate.Text = ""
        TextboxFirstPaymentDate.Text = ""
        TextboxInterestRate.Text = ""
        TextboxPaymentAmount.Text = ""
        TextboxFinalPaymentDate.Text = ""
        TextboxPayrollFrequency.Text = ""
        TextboxLoanTerm.Text = ""
        TextboxLoanOriginationFee.Text = ""
        txtPaymentMethod.Text = ""
        txtAccountType.Text = ""
        txtAccountNumber.Text = ""
        txtBankABA.Text = ""
        txtPaymentStatus.Text = ""
        txtDefaultedLoanDate.Text = ""
        txtBoxONDRequested.Text = ""
        lblOffsetReasonText.Text = ""
        TextboxPayoffAmount.Text = ""
        TextboxSavedPayoffAmount.Text = ""
        TextboxComputeOn.Text = ""
        txtLoanType.Text = "" ' SC | 2020.04.20 | YRS-AT-4853 - Display Loantype for YRS LOAN SECTION

        txtWebLoanNumber.Text = ""
        txtWebRequestedAmount.Text = ""
        txtWebApplicationStatus.Text = ""
        txtMaxAvailableLoanAmount.Text = ""
        txtWebStatusDateTime.Text = ""
        txtWebRequestedTerm.Text = ""
        txtWebApplicationDateTime.Text = ""
        txtWebPayrollFrequency.Text = ""
        txtWebLoanPurpose.Text = ""
        txtWebReasonNotes.Text = ""
        txtWebPaymentMethod.Text = ""
        txtWebONDRequested.Text = ""
        txtWebBankName.Text = ""
        txtWebBankABA.Text = ""
        txtWebAccount.Text = ""
        txtWebAccountType.Text = ""
        'START: PK | 01/10/2019 | YRS-AT-4241 | This line is commented for proper naming, change of txtWebMaritalStatus to txtOldWebMaritalStatus and additon of method for txtNewWebMaritalStatus
        'txtWebMaritalStatus.Text = ""
        txtOldWebMaritalStatus.Text = ""
        txtNewWebMaritalStatus.Text = ""
        'END: PK | 01/10/2019 | YRS-AT-4241 | This line is commented for proper naming, change of txtWebMaritalStatus to txtOldWebMaritalStatus and additon of method for txtNewWebMaritalStatus
        txtWebDocreceivedDate.Text = ""
        txtWebLoanType.Text = "" ' SC | 2020.04.20 | YRS-AT-4853 - Display Loantype for WEB LOAN SECTION
    End Sub
    'END : VC | 2018.08.06 | YRS-AT-4018 -  Method to clear all textbox

    'START : VC | 2018.08.24 | YRS-AT-4018 -  Method to set Webloan object to approve loan
    Private Function ApproveLoan(ByVal loanOriginationId As String, ByVal signatureReceivedDate As String, ByVal maritalStatus As String, ByVal docCode As String, ByVal paymentMethod As String) As YMCAObjects.WebLoan
        Dim webLoan As YMCAObjects.WebLoan = New YMCAObjects.WebLoan
        webLoan.LoanOriginationId = loanOriginationId
        webLoan.SignatureReceivedDate = signatureReceivedDate
        webLoan.MaritalStatus = maritalStatus
        webLoan.DocCode = docCode
        webLoan.PaymentMethod = paymentMethod
        Return webLoan
    End Function
    'END : VC | 2018.08.24 | YRS-AT-4018 -  Method to set Webloan object to approve loan

    'START : VC | 2018.08.24 | YRS-AT-4018 -  Method to set Webloan object to decline loan
    Private Function DeclineLoan(ByVal loanOriginationId As String, ByVal declineComment As String, ByVal declineReason As String, ByVal paymentMethod As String) As YMCAObjects.WebLoan
        Dim webLoan As YMCAObjects.WebLoan = New YMCAObjects.WebLoan
        webLoan.LoanOriginationId = loanOriginationId
        webLoan.DeclineComment = declineComment
        webLoan.DeclineReason = declineReason
        webLoan.PaymentMethod = paymentMethod
        Return webLoan
    End Function
    'END : VC | 2018.08.24 | YRS-AT-4018 -  Method to set Webloan object to decline loan

    'START: ML | 2019.01.17 | YRS-3157 | Get FeezeUnfeezeHistory data from database to display in popup
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetFreezeUnfreezHistory(ByVal requestedLoanDetailId As String) As List(Of YMCAObjects.LoanFreezeUnfreezeHistory)
        Try
            If Not String.IsNullOrEmpty(requestedLoanDetailId) Then 'MMR | 2019.01.30 | YRS-AT-3157 | Check if loan detail ID is empty or not
                Return YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LoanFreezeUnfreezeHistoryList(requestedLoanDetailId)
                'START: MMR | 2019.01.30 | YRS-AT-3157 | Return empty value if loan detail ID is empty
            Else
                Return Nothing
            End If
            'END: MMR | 2019.01.30 | YRS-AT-3157 | Return empty value if loan detail ID is empty
        Catch ex As Exception
            HelperFunctions.LogException("ParticipantInformation_GetFreezeUnfreezHistory", ex) 'MMR | 2019.01.30 | YRS-AT-3157 | Added to log exception
        End Try
    End Function
    'END: ML | 2019.01.17 | YRS-3157 | Get FeezeUnfeezeHistory data from database to display in popup  

    Private Sub DataGridParticipantEmployment_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles DataGridParticipantEmployment.ItemDataBound
       
    End Sub
    'START: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            Dim toolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
            ButtonAddEmployment.Enabled = False
            ButtonAddEmployment.ToolTip = toolTip
            For Each row In DataGridParticipantNotes.Items
                Dim lnkbtndelete As LinkButton = (TryCast((TryCast(row, TableRow)).Cells(6).Controls(1), LinkButton))
                Dim chkmarkimp As CheckBox = (TryCast((TryCast(row, TableRow)).Cells(5).Controls(1), CheckBox))
                lnkbtndelete.Enabled = False
                lnkbtndelete.ToolTip = toolTip
                chkmarkimp.Enabled = False
                chkmarkimp.ToolTip = toolTip
            Next
            ButtonReactivate.Enabled = False
            ButtonReactivate.ToolTip = toolTip
            ButtonSavePayoffAmount.Enabled = False
            ButtonSavePayoffAmount.ToolTip = toolTip
        End If
    End Sub
    'END: Shilpa N | 03/04/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

End Class
'SR:2012.08.17 - BT-957/YRS 5.0-1484:New Class is added to Loag termination watcher grid
Public Class PersonDetails
    Public Property Type() As String
        Get
            Return m_Type
        End Get
        Set(ByVal value As String)
            m_Type = value
        End Set
    End Property
    Private m_Type As String
    Public Property PlanType() As String
        Get
            Return m_PlanType
        End Get
        Set(ByVal value As String)
            m_PlanType = value
        End Set
    End Property
    Private m_PlanType As String
    Public Property Notes() As String
        Get
            Return m_notes
        End Get
        Set(ByVal value As String)
            m_notes = value
        End Set
    End Property
    Private m_notes As String

    Public Property Important() As Boolean
        Get
            Return m_important
        End Get
        Set(ByVal value As Boolean)
            m_important = value
        End Set
    End Property
    Private m_important As Boolean
    'AA:01.04.2014 -BT:2464 - Added a column for sisplaying status termination watcher
    Public Property Status() As String
        Get
            Return m_status
        End Get
        Set(ByVal value As String)
            m_status = value
        End Set
    End Property
    Private m_status As String

   

End Class
'Ends, SR:2012.08.17 - BT-957/YRS 5.0-1484:New Class is added to Loag termination watcher grid
