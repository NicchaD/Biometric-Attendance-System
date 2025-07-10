/*
*********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By			Date				Description
'********************************************************************************************************************************
'Ashutosh Patil			05-Mar-2007			FOR YREN-3028,YREN-3029 A common proc for Address related for Primary
											Or Secondary will be called.BO Class - SearchParicipantAddress
'Nikunj Patel			26-Jun-2007			Added code to pull Beneficiary information from the database for the new Beneficiary Types
'Nikunj Patel			15-Apr-2008			Added code to enable treatment of ML fund statues as either DA or DI based on user input
'Swopna                 27-May-2008         Added function to get fund event status of selected person
'Dilip Yadav            28-Oct-2009         YRS 5.0.921 : To provide the Priority handling 
'Priya					19/2/2010			YRS 5.0-988 : fetch value of newly added column paramAllowShareInfo as parameter
'Priya					5-April-2010		    YRS 5.0-1042 : fetch value of newly aaded column bitYmcaMailOptOut
'Priya					19-July-2010        Integration of 10-June-2010:	YRS 5.0-1104:Showing non-vested person as vested (Change parameter persid to fundeventid)
'Ashish					2010.07.21			YRS 5.0-1136,handle multiple fund events, this method return fundevent status for particular fund event Id						
'Sanjay                 2011.04.27          YRS 5.0-1292,Added new parameter in procedure DeathNotifyPrerequisites for FundStatus.
'bhavnaS                2011.07.11          YRS 5.0-1354, Add new bit field 	bitGoPaperless,	change caption of bit field YmcaMailOptOut		
'BhavnaS                2011.08.05          YRS 5.0-1380:BT:910 - here compute termination date to sort the  Employment tab on LookUpEmploymentInfo
'BhavnaS                2012.01.18          :YRS 5.0-1497 -Add edit button to modify the date of death
'prasad jadhav			2012.01.19			For BT-925,YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
'BhavnaS                2012.03.02			YRS 5.0-1432:New report of checks issued after date of death:BT:941
 Prasad jadhav			2012.03.12			For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
 Prasad jadhav			2012.04.10			For:BT:1018:YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page        
 prasad jadhav			2012.04.26			For BT-991,YRS 5.0-1530 - Need ability to reactivate a terminated employee
 Sanjay R.              2012.08.17          BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program
 Dinesh K               2012.12.27          FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
 DIneshk                2013.09.17          BT-2139:YRS 5.0-2165:RMD enhancements. 
 Anudeep A              2014.02.03          BT:2292:YRS 5.0-2248 - YRS Pin number 
 Shashank				2014.02.03			BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN
 Anudeep A              2014.02.05          BT:2316:YRS 5.0-2262 - Spousal Consent date in YRS
 Anudeep A              2014.02.16          BT:2291:YRS 5.0-2247 - Need bitflag that will not allow a participant to create a web account 
 Dinesh Kanojia         2014.02.18          BT-2139: YRS 5.0-2165:RMD enhancements 
 Shashank Patel         2014.11.20          BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
 Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
 Bala                   2016.01.19          YRS-AT-2398: Customer Service requires a Special Handling alert for officers.
 Pramod Prakash Pokale  2016.04.22          YRS-AT-2719 - Applying Fees and deductions to death payments - Part A.2 
 Chandra sekar.c        2016.07.05          YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
 Santosh Bura           2016.07.07          YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annuity beneficiary SSN
'Manthan Rajguru        2016.07.27          YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Manthan Rajguru        2016.08.30          YRS-AT-2488 -  YRS enh: PART 1 of 4:RMD's for alternate payees (QDRO recipients) (TrackIT 22284)
'Santosh Bura			2016.09.21			YRS-AT-3028 -  YRS enh-RMD Utility - married to married Spouse beneficiary adjustment(TrackIT 25233) 
'Santosh Bura			2016.10.13 			YRS-AT-3095 -  YRS enh-allow regenerate RMD for deceased participants (TrackIT 27024)
'Manthan Rajguru 	    2017.12.04		    YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
'Vinayan C              2018.08.01          YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab  
'Megha Lad              2019.01.17          YRS-AT-3157 - YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)  
//********************************************************************************************************************/

using System;
using System.Collections;
using System.Data;
using YMCARET.YmcaDataAccessObject;
using YMCAObjects;//VC | 2018.08.01 | YRS-AT-4018 - Using YMCAObjects namespace
using System.Collections.Generic; //ML | 2019.01.23 | YRS-AT-3157 | Provides generic collections

namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// Summary description for ParticipantsInformation.
    /// </summary>
    public sealed class ParticipantsInformationBOClass
    {
        public ParticipantsInformationBOClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>

        public static DataSet LookUpGeneralInfo(string parameterPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpGeneralInfo(parameterPersId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpPOAInfo(string parameterPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpPOAInfo(parameterPersId);
            }
            catch
            {
                throw;
            }

        }
        //Priya	19-July-2010        Integration of 10-June-2010:	YRS 5.0-1104:Showing non-vested person as vested (Change parameter persid to fundeventid)
        //Change parameter persid to fundeventid
        //public static int  IsVested(string parameterPersId)
        public static int IsVested(string paramFundEventId)
        {
            try
            {
                //return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.IsVested(parameterPersId);			
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.IsVested(paramFundEventId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpAddressInfo(string parameterPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpPrimaryAddressInfo(parameterPersId);
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// 05-Mar-2007 Ashutosh Patil YREN-3028,YREN-3029
        /// </summary>
        /// <param name="parameterPersId","parameterIsPrimary"></param>
        /// <returns></returns>
        public static DataSet SearchParicipantAddress(string parameterPerssId, int parameterIsPrimary)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.SearchParicipantAddress(parameterPerssId, parameterIsPrimary);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpSecondaryAddress(string parameterPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpSecondaryAddress(parameterPersId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <param name="parameterFundId"></param>
        /// <returns></returns>
        public static DataSet LookUpEmploymentInfo(string parameterPersId, string parameterFundId)
        {
            try
            {
                //BS:2011.08.05:YRS 5.0-1380:BT:910 - here compute termination date to sort the  Employment tab
                DataSet ds = YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpEmploymentInfo(parameterPersId, parameterFundId);
                DataColumn dc = new DataColumn("EffectiveTermdate", typeof(DateTime));
                dc.Expression = "ISNULL([Termdate],#" + Convert.ToString(DateTime.MaxValue) + "# )";
                ds.Tables[0].Columns.Add(dc);
                ds.AcceptChanges();
                return ds;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <param name="parameterFundId"></param>
        /// <returns></returns>
        public static DataSet LookUpFundEventInfo(string parameterPersId, string parameterFundId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpFundEventInfo(parameterPersId, parameterFundId);
            }
            catch
            {
                throw;
            }
        }

        public static bool UpdateService(string p_string_FundEventId, string p_string_VestingDate, string p_string_VestingReason, int p_integer_Paid, int p_integer_NonPaid)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateService(p_string_FundEventId, p_string_VestingDate, p_string_VestingReason, p_integer_Paid, p_integer_NonPaid);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>

        public static DataSet LookUpAddAccountInfo(string parameterPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpAddAccountInfo(parameterPersId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterStartDate"></param>
        /// <param name="parameterEndDate"></param>
        /// <param name="parameterPersId"></param>
        /// <param name="parameterFlag"></param>
        /// <returns></returns>
        public static DataSet LookUpAccountContributionInfo(string parameterStartDate, string parameterEndDate, string parameterFundEventID, bool parameterFlag)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpAccountContributionInfo(parameterStartDate, parameterEndDate, parameterFundEventID, parameterFlag);

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpActiveBeneficiariesInfo(string parameterPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpActiveBeneficiariesInfo(parameterPersId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpRetiredBeneficiariesInfo(string parameterPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpRetiredBeneficiariesInfo(parameterPersId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static ArrayList SetBeneficiaryAccess(string parameterPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.SetBeneficiaryAccess(parameterPersId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Method to update the general details of the participant
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <param name="parameterSSNo"></param>
        /// <param name="parameterLast"></param>
        /// <param name="parameterFirst"></param>
        /// <param name="parameterMiddle"></param>
        /// <param name="parameterSal"></param>
        /// <param name="parameterSuffix"></param>
        /// <param name="parameterGender"></param>
        /// <param name="parameterMaritalStatus"></param>
        /// <param name="parameterDOB"></param>
        /// <param name="parameterSpousalWaiver"></param>
        /// <returns></returns>
        /// 
        //Commented and added by dilip yadav : YRS 5.0.921 : Priority Handling
        //public static string UpdateGeneralInfo(string parameterPersId,string parameterSSNo,string parameterLast,string parameterFirst,string parameterMiddle,string parameterSal,string parameterSuffix,string parameterGender,string parameterMaritalStatus,string parameterDOB,string parameterSpousalWaiver)
        //Priya					5-April-2010		YRS 5.0-1042 : fetch value of newly aaded column bitYmcaMailOptOut
        //bhavnaS 2011.07.08 YRS 5.0-1354 :Change caption paramYMCAMailingOptOut
        //bhavnaS 2011.07.11 YRS 5.0-1354 :Add new bit field paramGoPaperless
        // public static string UpdateGeneralInfo(string parameterPersId, string parameterSSNo, string parameterLast, string parameterFirst, string parameterMiddle, string parameterSal, string parameterSuffix, string parameterGender, string parameterMaritalStatus, string parameterDOB, string parameterSpousalWaiver, string parameterPriority, string paramAllowShareInfo, string paramYMCAMailingOptOut)
        //{
        //BS:2012.01.18:YRS 5.0-1497 -Add edit button to modify the date of death
        //public static string UpdateGeneralInfo(string parameterPersId, string parameterSSNo, string parameterLast, string parameterFirst, string parameterMiddle, string parameterSal, string parameterSuffix, string parameterGender, string parameterMaritalStatus, string parameterDOB, string parameterSpousalWaiver, string parameterPriority, string paramAllowShareInfo, string  paramPersonalInfoSharingOptOut, string  paramGoPaperless)
        //{
        //prasad 2012.01.19 YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
        //Start: Bala: 01/19/2019: YRS-AT-2398: Parameter name change.
        //AA:2014.02.06:BT:2316 :YRS 5.0-2262 - Added for passing parameter to sp
        //public static string UpdateGeneralInfo(string parameterPersId, string parameterSSNo, string parameterLast, string parameterFirst, string parameterMiddle, string parameterSal, string parameterSuffix, string parameterGender, string parameterMaritalStatus, string parameterDOB, string parameterSpousalWaiver, string parameterPriority, string paramAllowShareInfo, string paramPersonalInfoSharingOptOut, string paramGoPaperless, string paramUpdatedDeceasedDate, Object paramMRD, string paramCannotLocateSpouse, string paramWebLockOut, string paramUpdationFrom) //Bala: 01/19/2019: YRS-AT-2398: Variable name change
        public static string UpdateGeneralInfo(string parameterPersId, string parameterSSNo, string parameterLast, string parameterFirst, string parameterMiddle, string parameterSal, string parameterSuffix, string parameterGender, string parameterMaritalStatus, string parameterDOB, string parameterSpousalWaiver, string parameterExhaustedDBSettle, string paramAllowShareInfo, string paramPersonalInfoSharingOptOut, string paramGoPaperless, string paramUpdatedDeceasedDate, Object paramMRD, string paramCannotLocateSpouse, string paramWebLockOut, string paramUpdationFrom)
        {
            //End: Bala: 01/19/2019: YRS-AT-2398: Parameter name change.
            try
            {
                //Commented and added by dilip yadav : YRS 5.0.921 : Priority Handling
                //Priya : Added on 19/2/2010 :YRS 5.0-988 : fetch value of newly added column paramAllowShareInfo as parameter
                //return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateGeneralInfo( parameterPersId, parameterSSNo, parameterLast, parameterFirst, parameterMiddle, parameterSal, parameterSuffix, parameterGender, parameterMaritalStatus, parameterDOB, parameterSpousalWaiver);
                //bhavnaS 2011.07.08 : YRS 5.0-1354 :change caption
                //bhavnaS 2011.07.11 : YRS 5.0-1354 :Add new param  paramGoPaperless
                //return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateGeneralInfo(parameterPersId, parameterSSNo, parameterLast, parameterFirst, parameterMiddle, parameterSal, parameterSuffix, parameterGender, parameterMaritalStatus, parameterDOB, parameterSpousalWaiver, parameterPriority, paramAllowShareInfo, paramYMCAMailingOptOut);
                //return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateGeneralInfo(parameterPersId, parameterSSNo, parameterLast, parameterFirst, parameterMiddle, parameterSal, parameterSuffix, parameterGender, parameterMaritalStatus, parameterDOB, parameterSpousalWaiver, parameterPriority, paramAllowShareInfo, paramPersonalInfoSharingOptOut, paramGoPaperless);
                //BS:2012.01.18:YRS 5.0-1497 -Add edit button to modify the date of death
                //prasad 2012.01.19 YRS 5.0-1400 : New parameter "paramMRD" passed to function
                //AA:2014.02.06:BT:2316 :YRS 5.0-2262 - Added for passing parameter to sp
                //AA:2014.02.16:BT:2291 :YRS 5.0-2247 - Added for passing parameter to sp
                //Start: Bala: 01/19/2019: YRS-AT-2398: Parameter name change.
                //return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateGeneralInfo(parameterPersId, parameterSSNo, parameterLast, parameterFirst, parameterMiddle, parameterSal, parameterSuffix, parameterGender, parameterMaritalStatus, parameterDOB, parameterSpousalWaiver, parameterPriority, paramAllowShareInfo, paramPersonalInfoSharingOptOut, paramGoPaperless, paramUpdatedDeceasedDate, paramMRD,paramCannotLocateSpouse,paramWebLockOut,paramUpdationFrom); 
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateGeneralInfo(parameterPersId, parameterSSNo, parameterLast, parameterFirst, parameterMiddle, parameterSal, parameterSuffix, parameterGender, parameterMaritalStatus, parameterDOB, parameterSpousalWaiver, parameterExhaustedDBSettle, paramAllowShareInfo, paramPersonalInfoSharingOptOut, paramGoPaperless, paramUpdatedDeceasedDate, paramMRD, paramCannotLocateSpouse, paramWebLockOut, paramUpdationFrom);
                //End: Bala: 01/19/2019: YRS-AT-2398: Parameter name change.
            }
            catch
            {
                throw;
            }

        }
        /// <summary>
        /// Method to get the Qdro Info
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet GetQDROInfo(string parameterPersId, string parameterQdroType)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetQDROInfo(parameterPersId, parameterQdroType);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Method to update QDRO Info for the participant
        /// </summary>
        /// <param name="parameterUniqueId"></param>
        /// <param name="parameterStatus"></param>
        /// <param name="parameterType"></param>
        /// <param name="parameterStatusDate"></param>
        /// <param name="parameterDraftDate"></param>
        /// <returns></returns>
        public static string UpdateQDROInfo(string parameterUniqueId, string parameterStatus, string parameterType, string parameterStatusDate, string parameterDraftDate, string persID ) //Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Passed parameter for Perss ID
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateQDROInfo(parameterUniqueId, parameterStatus, parameterType, parameterStatusDate, parameterDraftDate, persID); //Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Passed parameter for Perss ID
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to add Qdro Information
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <param name="parameterFundId"></param>
        /// <param name="parameterStatus"></param>
        /// <param name="parameterType"></param>
        /// <param name="parameterStatusDate"></param>
        /// <param name="parameterDraftDate"></param>
        /// <returns></returns>
        public static string AddQDROInfo(string parameterPersId, string parameterFundId, string parameterStatus, string parameterType, string parameterStatusDate, string parameterDraftDate)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.AddQDROInfo(parameterPersId, parameterFundId, parameterStatus, parameterType, parameterStatusDate, parameterDraftDate);
            }
            catch
            {
                throw;
            }
        }
        public static int DeathNotifyPrerequisites(string paramPersonOrFundEvent, string param_PersonOrFundEventID, DateTime paramDeathDate, out string l_string_Output_DR, out string l_string_Output_DA)
        {
            try
            {
                l_string_Output_DR = "";
                l_string_Output_DA = "";
                return (YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.DeathNotifyPrerequisites(paramPersonOrFundEvent, param_PersonOrFundEventID, paramDeathDate, out l_string_Output_DR, out  l_string_Output_DA));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramPersonOrFundEvent"></param>
        /// <param name="param_PersonOrFundEventID"></param>
        /// <param name="paramDeathDate"></param>
        /// <param name="l_string_Output"></param>
        /// <param name="paramTreatMLas">Forces ML to be treated as either Active death or Inactive Death. Allowed values are DA/DI</param>
        /// <returns></returns>
        //public static int DeathNotifyActions(string paramPersonOrFundEvent, string param_PersonOrFundEventID, DateTime paramDeathDate, out string l_string_Output, string paramTreatMLas)
        //{
        //BS:2012.03.02:BT-941,YRS 5.0-1432: add one out parameter for report
        //DineshK:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
        //Add DeathNotifystatus parameter to get fetch the message from database.
        public static int DeathNotifyActions(string paramPersonOrFundEvent, string param_PersonOrFundEventID, DateTime paramDeathDate, out string l_string_Output, out string l_string_DeathNotifyOutput, string paramTreatMLas, string strParticipantSSno, out string l_string_showreport)
        {
            try
            {
                l_string_Output = "";
                return (YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.DeathNotifyActions(paramPersonOrFundEvent, param_PersonOrFundEventID, paramDeathDate, out l_string_Output, out l_string_DeathNotifyOutput, paramTreatMLas, strParticipantSSno, out  l_string_showreport));
            }
            catch
            {
                throw;
            }

        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterSearchYMCANo"></param>
        /// <param name="parameterSearchYMCAName"></param>
        /// <param name="parameterSearchYMCACity"></param>
        /// <param name="parameterSearchYMCAState"></param>
        /// <returns></returns>
        public static DataSet SearchYMCAMetro(string parameterSearchYMCANo, string parameterSearchYMCAName, string parameterSearchYMCACity, string parameterSearchYMCAState)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.SearchYMCAMetro(parameterSearchYMCANo, parameterSearchYMCAName, parameterSearchYMCACity, parameterSearchYMCAState);
            }
            catch
            {
                throw;
            }
        }

        public static DataSet SearchYMCABranch(string parameterSearchYMCAId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.SearchYMCABranch(parameterSearchYMCAId);
            }
            catch
            {
                throw;
            }
        }

        public static DataSet SearchYMCABranchOnCriteria(string parameterSearchYMCANo, string parameterSearchYMCAName, string parameterSearchYMCACity, string parameterSearchYMCAState, string parameterYmcaId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.SearchYMCABranchOnCriteria(parameterSearchYMCANo, parameterSearchYMCAName, parameterSearchYMCACity, parameterSearchYMCAState, parameterYmcaId);
            }
            catch
            {
                throw;
            }
        }

        public static string SearchYMCAStatus(string parameterSearchYMCAId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.SearchYMCAStatus(parameterSearchYMCAId);
            }
            catch
            {
                throw;
            }
        }

        public static void InsertParticipantNotes(DataSet dsNotes)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.InsertParticipantNotes(dsNotes);
            }
            catch
            {
                throw;
            }
        }

        public static DataTable MemberNotes(string paramaeterPersonID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.MemberNotes(paramaeterPersonID));
            }
            catch
            {
                throw;
            }
        }

        public static string GetNotesById(string parameterUniqueId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetNotesById(parameterUniqueId);
            }
            catch
            {
                throw;
            }
        }


        public static void InsertBeneficiaries(DataSet dsBeneficiaries)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.InsertBeneficiaries(dsBeneficiaries);
            }
            catch
            {
                throw;
            }
        }

        //START: SB | 07/07/2016 | YRS-AT-2382 | Insert Beneficiary Changed SSN in  Audit Table
        public static void InsertBeneficiariesSSNChangeAuditRecord(DataSet dsChangedBeneficiariesSSNAuditRecord)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.InsertBeneficiariesSSNChangeAuditRecord(dsChangedBeneficiariesSSNAuditRecord);
            }
            catch
            {
                throw;
            }
        }
        //END : SB | 07/07/2016 | YRS-AT-2382 | Insert Beneficiary Changed SSN in  Audit Table
		// SB | 2016.09.21 |YRS-AT-3028 | Add new parameter to capture deceased beneficiary SSN
        public static bool InsertBeneficiaryNotes(string strReason, string dtmDOD, string strBenificiaryName, string strComments, bool bitImportant, string strGuiEntityId, string strTypeCode, string strBeneUniqueID, string BeneficarySSNo) 
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.InsertBeneficiaryNotes(strReason, dtmDOD, strBenificiaryName, strComments, bitImportant, strGuiEntityId, strTypeCode, strBeneUniqueID,BeneficarySSNo);
            }
            catch
            {
                throw;
            }
        }


        public static void InsertRetiredBeneficiaries(DataSet dsBeneficiaries)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.InsertRetiredBeneficiaries(dsBeneficiaries);
            }
            catch
            {
                throw;
            }
        }
        public static string AddEmployment(DataSet parameterDSEmployment)
        {
            try
            {
                return ParticipantsInformationDAClass.AddEmployment(parameterDSEmployment);
            }
            catch
            {
                throw;
            }
        }

        public static string AddAdditionalAccount(DataSet parameterDSAddAccount)
        {
            try
            {
                return ParticipantsInformationDAClass.AddAdditionalAccount(parameterDSAddAccount);
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetCanBeKilled(string parameterFundId)
        {
            try
            {
                return ParticipantsInformationDAClass.GetCanBeKilled(parameterFundId);
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateParticipantAddress(DataSet parameterdsAddress)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateParticipantAddress(parameterdsAddress);
            }
            catch
            {
                throw;
            }
        }
        public static void UpdateEmailAddress(DataSet parameterdsAddress)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateEmailAddress(parameterdsAddress);
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateTelephone(DataSet parameterdsAddress)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateTelephone(parameterdsAddress);
            }
            catch
            {
                throw;
            }
        }
        public static void UpdateEmailProperties(string paramUniqueId, string paramEntityid, bool paramBadEmail, bool paramUnsubscribed, bool paramtextOnly, bool paramBitPrimary, bool ParamBitActive, bool paramSecActive, string paramCreator, string paramUpdater)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateEmailProperties(paramUniqueId, paramEntityid, paramBadEmail, paramUnsubscribed, paramtextOnly, paramBitPrimary, ParamBitActive, paramSecActive, paramCreator, paramUpdater);
            }
            catch
            {
                throw;
            }
        }
        public static DataSet LookUpSecondaryEmailProperties(string paramPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpSecondaryEmailProperties(paramPersId);
            }
            catch
            {
                throw;
            }
        }
        public static DataSet LookUpPrimaryEmailProperties(string paramPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpPrimaryEmailProperties(paramPersId);
            }
            catch
            {
                throw;
            }
        }
        public static Int32 SuppressedJSAnnuitiesCount(string paramPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.SuppressedJSAnnuitiesCount(paramPersId);
            }
            catch
            {
                throw;
            }
        }

        //START: PPP | 04/22/2016 | YRS-AT-2719 | Provides annuties details
        public static DataSet GetSuppressedJSAnnuitiesDetails(string strPersID)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetSuppressedJSAnnuitiesDetails(strPersID);
            }
            catch
            {
                throw;
            }
        }
        //END: PPP | 04/22/2016 | YRS-AT-2719 | Provides annuties details

        public static DataTable UnSuppressJSAnnuitiesCount(string paramPersId, string paramXMLDeduction) // PPP | 04/28/2016 | YRS-AT-2719 | Passing deductions in XML format (paramXMLDeduction)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UnSuppressJSAnnuitiesCount(paramPersId, paramXMLDeduction); // PPP | 04/28/2016 | YRS-AT-2719 | Passing deductions in XML format (paramXMLDeduction)
            }
            catch
            {
                throw;
            }
        }

        public static DataSet LookUp_AMP_StateNames(string l_string_CountryCode)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUp_AMP_StateNames(l_string_CountryCode);
            }
            catch
            {
                throw;
            }
        }

        public static DataSet LookUpLoanDetails(int parameterLoanRequestId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpLoanDetails(parameterLoanRequestId);
            }
            catch
            {
                throw;
            }
        }

        public static DataSet LookUpLoanRequests(string parameterPersId, string source = "YRS")//VC | 2018.08.20 | YRS-AT-4018 | Added parameter to pass source
        {
            try
            {
                return YMCARET.YmcaBusinessObject.LoanInformationBOClass.LookUpLoanRequests(parameterPersId, source);//VC | 2018.08.20 | YRS-AT-4018 | Passing parameter source
            }
            catch
            {
                throw;
            }
        }

        //NP:PS:2007.06.26 - Adding code to pull appropriate Beneficiary types from the database
        //					for active participants
        public static DataSet getRelationShips()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.getRelationShips();
            }
            catch
            {
                throw;
            }
        }
        //NP:PS:2007.06.26 - Adding code to pull appropriate Beneficiary types from the database
        //					for active participants
        public static DataSet getBenefitGroups()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.getBenefitGroups();
            }
            catch
            {
                throw;
            }
        }
        //NP:PS:2007.06.26 - Adding code to pull appropriate Beneficiary types from the database
        //					for active participants
        public static DataSet getBenefitLevels()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.getBenefitLevels();
            }
            catch
            {
                throw;
            }
        }
        //NP:PS:2007.06.26 - Adding code to pull appropriate Beneficiary types from the database
        //					for active participants
        public static DataSet getBenefitTypes()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.getBenefitTypes();
            }
            catch
            {
                throw;
            }
        }

        //Ashish:2010.07.21,YRS 5.0-1136, handle multiple fund events, this method return fundevent status for particular fund event Id
        public static DataSet GetFundEventStatus(string parameterPersID, string parameterFundEventId)
        //public static DataSet GetFundEventStatus(string parameterUniqueId)//Swopna 27-May-2008 
        {
            try
            {
                //return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetFundEventStatus(parameterUniqueId);
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetFundEventStatus(parameterPersID, parameterFundEventId);
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateOnParticipantTermination(string GuiPersId)//Swopna 27-May-2008
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateOnParticipantTermination(GuiPersId);
            }
            catch
            {
                throw;
            }
        }


        #region  Code For account Lock/Unlock



        /// <summary>
        /// To insert the Locking info in table "AtsParticipantAccountLock"
        /// </summary>
        /// <param name="paramPersId">PersId of the participant for which an entry is being made.</param>
        /// <param name="paramAcctLock">Account Lock bit value</param>
        /// <param name="paramLockReasonCode">Code value indicating why a particular account was locked or unlocked </param>
        //public static void InsertLockInfo(string paramPersId, bool paramAcctLock, string paramLockReasonCode)
        //{
        //    try
        //    {
        //        YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.InsertLockInfo(paramPersId, paramAcctLock, paramLockReasonCode);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// To call the DA class method to read the LOCK REASON CODE.
        /// </summary>
        /// <param name="paramIsLock">Account Lock Status</param>
        /// <returns></returns>
        public static DataSet GetLockReasonDetails(string paramSSN)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetLockReasonDetails(paramSSN);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// call DA class method to update the account status in ats perss tabe
        /// </summary>
        /// <param name="paramPersId">Pers id</param>
        /// <param name="paramAcctStatus">Account status</param>
        public static void UpdateAccountLockStatus(string paramPersId, bool paramAcctStatus, string paramLockReasonCode)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateAccountLockStatus(paramPersId, paramAcctStatus, paramLockReasonCode);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// To call the DA class method to read the LOCK REASON CODE.
        /// </summary>
        /// <param name="paramIsLock">Account Lock Status</param>
        /// <returns></returns>
        public static DataSet GetReasonCodes()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetReasonCodes();
            }
            catch
            {
                throw;
            }
        }



        #endregion

        public static DataSet GetHeaderDetails(string paramId, string paramType)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetHeaderDetails(paramId, paramType);
            }
            catch
            {
                throw;
            }
        }
        //Added by Prasad jadhav 2012.03.12	For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
        public static Boolean transactionexist(string paramId, string terminatindate)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.transactionexist(paramId, terminatindate);
            }
            catch
            {
                throw;
            }
        }
        //Added by Prasad jadhav 2012.03.12	For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
        //prasad jadhav		2012.04.26		For BT-991,YRS 5.0-1530 - Need ability to reactivate a terminated employee
        public static Boolean updateFundEventEmpEvent(string termDate, string eventId, string ymcaId, bool tdcontractexist, bool UpadteFlag, out string FundEventStatusCode, out string FundEventStatusDesc)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.updateFundEventEmpEvent(termDate, eventId, ymcaId, tdcontractexist, UpadteFlag, out FundEventStatusCode, out FundEventStatusDesc);
            }
            catch
            {
                throw;
            }
        }
        //Added by prasad For:BT:1018:YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
        public static string deleteRecord(string persID)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.deleteRecord(persID);
            }
            catch
            {
                throw;
            }
        }

        //Added by Bhavna Shrivastava 2012.06.26	For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
        public static Boolean DiffYMCAOnSameTermDateExist(string paramId, string terminatindate)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.DiffYMCAOnSameTermDateExist(paramId, terminatindate);
            }
            catch
            {
                throw;
            }
        }

        //SR: 2012.08.17     BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program

        public static string InsertTerminationWatcher(string PersId, string FundEventId, string type, string PlanType, string Notes, bool isImportant)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.InsertTerminationWatcher(PersId, FundEventId, type, PlanType, Notes, isImportant);
            }
            catch
            {
                throw;
            }
        }



        public static DataSet GetTerminationWatcher(string paramSSNo, string paramFundNo)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetTerminationWatcher(paramSSNo, paramFundNo);
            }
            catch
            {
                throw;
            }

        }

        public static DataSet GetPersonTerminationWatcherDetail(string paramPersId, string paramFundEventId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetPersonTerminationWatcherDetail(paramPersId, paramFundEventId);
            }
            catch
            {
                throw;
            }

        }



        public static DataSet GetPlanType()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetPlanType();
            }
            catch
            {
                throw;
            }
        }

        //Ends, SR: 2012.08.17 - BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program

        /// <summary>
        /// Function Added by DInesh Kanojia on 09-17-2013 - BT-2139:YRS 5.0-2165:RMD enhancements. 
        /// Add and Modify person meta configuratio details.
        /// Dinesh Kanojia        2014.02.18          BT-2139: YRS 5.0-2165:RMD enhancements 
        /// </summary>
        /// <param name="strGuiPerssID"></param>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string ModifyPerssMetaConfiguration(string strGuiPerssID, string strKey, string strValue)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.ModifyPerssMetaConfiguration(strGuiPerssID, strKey, strValue);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  Function Added by DInesh Kanojia on 09-17-2013 - BT-2139:YRS 5.0-2165:RMD enhancements.  
        ///  Get Person meta configuration details 
        /// </summary>
        /// <param name="strGuiPerssID"></param>
        /// <param name="strConfigCode"></param>
        /// <returns></returns>
        public static DataSet GetPersonMetaConfigurationDetails(string strGuiPerssID, string strConfigCode)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetPersonMetaConfigurationDetails(strGuiPerssID, strConfigCode);
            }
            catch
            {
                throw;
            }
        }
        //Start:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To Update the PIN value
        public static string UpdatePIN(string strGuiPersId, string intPIN)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdatePIN(strGuiPersId, intPIN);
            }
            catch
            {
                throw;
            }
        }
        //End:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To Update the PIN value
        //Start:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To delete the PIN value
        public static string DeletePIN(string strGuiPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.DeletePIN(strGuiPersId);
            }
            catch
            {
                throw;
            }
        }
        //End:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To delete the PIN value


        //SP 2014.02.03   BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -Start
        /// <summary>
        /// This method returns true if SSN found in table else false
        /// </summary>
        /// <param name="strGuiPerssID">Person guiPersid</param>
        /// <param name="strSSN">Person SSN</param>
        /// <returns>String With person records details</returns>
        public static string CheckSSNExistToOtherPerson(string strGuiPerssID, string strSSN)
        {

            try
            {

                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.CheckSSNExistToOtherPerson(strGuiPerssID, strSSN);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// This method insert record into AtsYRSAuditLog table
        /// </summary>
        /// <param name="strModuleName"> Name of the Module</param>
        /// <param name="strGuiEntityId">GuiEntity Id like Persid</param>
        /// <param name="strEntityType">Type of entity like pers,ymca etc.</param>
        /// <param name="strColumn">Name of the column to which maintain Audit</param>
        /// <param name="strOldValue">Old value of the column</param>
        /// <param name="strNewValue">New value of the column</param>
        /// <param name="strDescription">Details for the reason for update the column value.</param>
        /// <returns>Returns the uniqueid of the column.</returns>
        public static long InsertAuditLog(string strModuleName, string strGuiEntityId, string strEntityType, string strColumn, string strOldValue, string strNewValue, string strDescription)
        {
            try
            {

                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.InsertAuditLog(strModuleName, strGuiEntityId, strEntityType, strColumn, strOldValue, strNewValue, strDescription);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method get all the records based on guientityid.
        /// </summary>
        /// <param name="strGuiEntityId"></param>
        /// <param name="strColumn"></param>
        /// <returns>Return the Dataset with all columns</returns>
        public static DataSet GetAtsYRSAuditLogDeatilsByGuiEntityID(string strGuiEntityId, string strColumn)
        {
            try
            {

                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetAtsYRSAuditLogDeatilsByGuiEntityID(strGuiEntityId, strColumn);
            }
            catch
            {
                throw;
            }
        }

        //SP 2014.02.03   BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -End

        //SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - Start
        /// <summary>
        /// This method get all the salutation codes which is active
        /// </summary>
        /// <returns></returns>
        public static DataSet GetSalutationCodes()
        {
            try
            {

                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetSalutationCodes();
            }
            catch
            {
                throw;
            }
        }
        //SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - End
        //START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
        public static DataSet GetQDROSplitDetailsByRequestId(string strQDRORequestId)
        {
            try
            {

                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetQDROSplitDetailsByRequestId(strQDRORequestId);
            }
            catch
            {
                throw;
            }
        }
        //END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
        
        //Start -  MMR | 2016.07.27 | YRS-AT-2560 | Getting System generated Phony SSN and validating for existing SSN in system  
        public static void GeneratePhonySSN(DataSet dsBeneficiaries)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GeneratePhonySSN(dsBeneficiaries);
            }
            catch
            {
                throw;
            }
        }
        //End -  MMR | 2016.07.27 | YRS-AT-2560 | Getting System generated Phony SSN and validating for existing SSN in system  
		// START : SB | 10/13/2016 | YRS-AT-3095 |  Function to check RMD of deceased participant need to be regenerate
        public static Boolean DeathNotifyActions_IsRMDRegenerateRequired(string FundEventID)
        {
            try
            {

                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.DeathNotifyActions_IsRMDRegenerateRequired(FundEventID);
            }
            catch
            {
                throw;
            }
        }
        // END : SB | 10/13/2016 | YRS-AT-3095 |  Function to check RMD of deceased participant need to be regenerate 

        //START: MMR | 2017.12.04 | YRS-AT-3756 | Added to get deceased beneficiary details
        public static DataSet GetDeceasedBeneficiary(string persId, string beneficiaryType)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetDeceasedBeneficiary(persId, beneficiaryType);
            }
            catch
            {
                throw;
            }
        }

        //END: MMR | 2017.12.04 | YRS-AT-3756 | Added to get deceased beneficiary details

        // START : VC | 2018.08.02 | YRS-AT-4018 -  Method used to list  WEB loans details
        public static DataSet GetWebLoanDetails(int loanNumber)
        {
            return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.GetWebLoanDetails(loanNumber);
           
        }
        // END : VC | 2018.08.02 | YRS-AT-4018 -  Method used to list  WEB loans details

        // START : VC | 2018.08.03 | YRS-AT-4018 -  Method used to approve a pending web loan 
        public static ReturnObject<bool> ApproveWebLoan(WebLoan webLoan, string personId, string participantEmail)
        {
            ReturnObject<bool> result;
            MailUtil mailClient;
            string approveMessage;
            try
            {
                result = new ReturnObject<bool>();
                mailClient = new MailUtil();

                //Approving of pending loan 
                approveMessage = YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.ApproveWebLoan(webLoan);
                if (string.IsNullOrEmpty(approveMessage))
                {
                    //If the approve loan process is success then send email
                    result = mailClient.SendLoanEmail(LoanStatus.APPROVED, webLoan.LoanOriginationId, webLoan.PaymentMethod, null);
                    if(result.Value == false)
                    {
                        result.MessageList.Add(approveMessage);
                    }             
                }
                else 
                {
                    result.Value = false;
                    result.MessageList = new System.Collections.Generic.List<string>();
                    result.MessageList.Add(approveMessage);
                }
                return result;
            }
            catch (Exception ex)
            {
                CommonClass.LogException("ApproveWebLoan()", ex);
                throw ex;
            }
            finally
            {
                approveMessage = null;
                result = null;
                mailClient = null;
            }
        }
        // END : VC | 2018.08.03 | YRS-AT-4018 -  Method used to approve a pending web loan 

        // START : VC | 2018.08.03 | YRS-AT-4018 -  Method used to decline a pending web loan 
        //Returning ReturnObject<bool> 
        public static ReturnObject<bool> DeclineWebLoan(WebLoan webLoan,string personId,string participantEmail)
        {
            ReturnObject<bool> result = new ReturnObject<bool>();
            MailUtil mailClient = new MailUtil();
            string declineMessage = "";
            try
            {
                //Declining of pending loan
                declineMessage = YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.DeclineWebLoan(webLoan);
                if (string.IsNullOrEmpty(declineMessage) && !String.IsNullOrEmpty(participantEmail))
                {
                    result = mailClient.SendLoanEmail(LoanStatus.DECLINED, webLoan.LoanOriginationId, webLoan.PaymentMethod, null);
                    if (result.Value == false)
                    {
                        result.MessageList.Add(declineMessage);
                    }  
                }
                else
                {
                    result.Value = false;
                    result.MessageList = new System.Collections.Generic.List<string>();
                    result.MessageList.Add(declineMessage);
                }
                return result;
            }
            catch (Exception ex)
            {
                CommonClass.LogException("DeclineWebLoan()", ex);
                throw ex;
            }
            finally
            {
                declineMessage = null;
            }
        }
        // END : VC | 2018.08.03 | YRS-AT-4018 -  Method used to decline a pending web loan 
                
        //START: ML | 2019.01.17 | YRS-3157 | Fetching LoanfreezeUnfeeze History data from database
        public static List<YMCAObjects.LoanFreezeUnfreezeHistory> LoanFreezeUnfreezeHistoryList(string loanDetailId)
        {
            return YMCARET.YmcaBusinessObject.LoanInformationBOClass.LoanFreezeUnfreezeHistoryList(loanDetailId);
        }
        //END: ML | 2019.01.17 | YRS-3157 | Fetching LoanfreezeUnfeeze History data from database
    }
}
