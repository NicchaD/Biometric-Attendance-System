
//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	ParticipantsInformationDAClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email			:	
// Contact No		:	8751
// Creation Time	:	7/15/2005 3:30:42 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
/*
*********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By			Date				Description
'********************************************************************************************************************************
'Ashutosh Patil			05-Mar-2007			FOR YREN-3028,YREN-3029 A common proc for Address related for Primary
											Or Secondary will be called.DA Class - SearchParicipantAddress
//Changes By:Preeti On:8th May 2006 For Active and Inactive pre retirement death benefit enhancement
//PS referred: YMCA -PS-DeathCalculatorEnhancement.doc
//Shubhrata Mar06th 2007,YREN-3131 to get YMCA no from which the loan amount is deducted.
// Nikunj Patel			26-Jun-2007			Added code to pull Beneficiary information from the database for the new Beneficiary Types
// Nikunj Patel			03-Oct-2007			Increasing the length from 4 to 1000
//Swopna                24-Mar-2008         Table added in Lookuploandetails
// Nikunj Patel			16-Apr-2008			Added code to enable treatment of ML status as either DA or DI based on user input
// Swopna               27-May-2008         Added function to get fund event status of selected person
// Dilip Yadav          28-Oct-09           YRS 5.0.921 : To provide the Priority handling 
// Dilip Yadav          11-Nov-2009         To enable 4 fields Title,Professional,Exempt,FullTime in employement Tab while update as per YRS 5.0.941
//Sanjay Rawat          08 DEc 2010         YRS 5.0 635 : TO provide message  for any non annuity (ANN)  or Special Dividend (EXP) disbursement within 30 days from death.  
//'Priya				19/2/2010			YRS 5.0-988 : fetch value of newly added column paramAllowShareInfo as parameter
//Priya					5-April-2010		YRS 5.0-1042 : fetch value of newly aaded column bitYmcaMailOptOut
//Imran                 03-June-2010        CHanges for Enhancements                                    
//Priya	                19-July-2010        Integration of 10-June-2010:	YRS 5.0-1104:Showing non-vested person as vested (Change parameter persid to fundeventid)
//Ashish			    2010.07.21			YRS 5.0-1136,handle multiple fund events, this method return fundevent status for particular fund event Id
//Sanjay                2011.04.27          YRS 5.0-1292,Added new parameter in procedure DeathNotifyPrerequisites for FundStatus.
//bhavnaS               2011.07.08          YRS 5.0-1354, Change bit field caption bitYmcaMailOptOut in participant Info page.
//bhavnaS               2011.07.11          YRS 5.0-1354, Add new bit field bitGoPaperless into atsperss
//BhavnaS               2012.01.18          :YRS 5.0-1497 -Add edit button to modify the date of death
//prasad				2012.01.19			YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
//prasad				2012.01.24			For BT-950,YRS 5.0-1469: Add link to Web Front End
//prasad				2012.02.08			FOR BT-990,YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
//BhavnaS                2012.03.02			YRS 5.0-1432:New report of checks issued after date of death:BT:941
Prasad jadhav			2012.03.12			For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
//Prasad jadhav			2012.04.10			For:BT:1018:YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page        
//prasad jadhav			2012.04.26			For BT-991,YRS 5.0-1530 - Need ability to reactivate a terminated employee
//prasad jadhav			2012.04.26			(Reopen)YRS 5.0-1530 - Need ability to reactivate a terminated employee
//BhavnaS               2012.06.11			BT:951:YRS 5.0-1470: Link to Address Edit program from Person Maintenance
//Sanjay R              2012.08.17          BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program
//Anudeep               2012.12.12          Changes made to show source of watcher 
//DineshK               2012.12.27          FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
//DIneshK               2013.06.10          BugID : 2074 : Improper validation while doing death notification.
//'Anudeep              2013.07.10          BT-1501:YRS 5.0-1745:Capture Beneficiary addresses  
//'DIneshk              2013.09.17          BT-2139:YRS 5.0-2165:RMD enhancements. 
//Anudeep A             2014.02.03          BT:2292:YRS 5.0-2248 - YRS Pin number 
//Shashank	            2014.02.03          BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN 
//Anudeep A             2014.02.05          BT:2316:YRS 5.0-2262 - Spousal Consent date in YRS
//Anudeep A             2014.02.16          BT:2291:YRS 5.0-2247 - Need bitflag that will not allow a participant to create a web account                 
//Dinesh Kanojia        2014.02.18          BT-2139: YRS 5.0-2165:RMD enhancements 
//Anudeep A             2014.04.01          BT:2464:YRS 5.0-1484: Termination watcher changes(Re-Work).
//Shashank Patel        2014.11.20          BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
//Anudeep A             2015.03.12          BT:2699:YRS 5.0-2441 : Modifications for 403b Loans 
//Anudeep A             2015.07.25          BT:2699:YRS 5.0-2441 : Modifications for 403b Loans   
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Bala                  2016.01.19          YRS-AT-2398: Customer Service requires a Special Handling alert for officers.
//Manthan Rajguru       2016.02.19          YRS-AT-2799 -  YRS enh: please remove old sql call in Stored Procedure yrs_usp_AMP_SearchParticipantGeneral
//Pramod Prakash Pokale 2016.04.22          YRS-AT-2719 -  Applying Fees and deductions to death payments - Part A.2 
//Chandra sekar.c       2016.07.05          YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
 //Santosh Bura         2016.07.07          YRS-AT-2382 -  Capture 'Reason' for change of beneficiary SSN and Annuity beneficiary SSN
'Manthan Rajguru        2016.07.27          YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
'Manthan Rajguru        2016.08.30          YRS-AT-2488 -  YRS enh: PART 1 of 4:RMD's for alternate payees (QDRO recipients) (TrackIT 22284) 
'Santosh Bura			2016.09.21 			YRS-AT-3028 -  YRS enh-RMD Utility - married to married Spouse beneficiary adjustment(TrackIT 25233) 
'Santosh Bura			2016.10.13 			YRS-AT-3095 -  YRS enh-allow regenerate RMD for deceased participants (TrackIT 27024)  
'Santosh Bura			2017.03.17 			YRS-AT-2606 -  YRS bug- modification of enrollment date should also update participation date (TrackIT 23896)   
'Manthan Rajguru 	    2017.12.04		    YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836)
'Manthan Rajguru        2018.04.06          YRS-AT-3935 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for Participant Information "Loans" tab, "Loan Details" section (TrackIT 33024) 
'Vinayan C              2018.08.01          YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab 
'*********************************************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using YMCAObjects; //VC | 2018.08.03 | YRS-AT-4018 -  Added namespace

namespace YMCARET.YmcaDataAccessObject
{
    /// <summary>
    /// Summary description for ParticipantsInformationDAClass.
    /// </summary>
    public sealed class ParticipantsInformationDAClass
    {
        private ParticipantsInformationDAClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// To get the General Info
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpGeneralInfo(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpgeneralInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            //Added by prasadFor BT-950,YRS 5.0-1469: Add link to Web Front End
            //string[] l_TableNames = new string[] { "GeneralInfo", "LoginCredential", "SpecialHandlingDetails" }; //Bala: 01/19/2019: YRS-AT-2398: Added special handling table.
            string[] l_TableNames = new string[] { "GeneralInfo", "SpecialHandlingDetails" }; //Manthan Rajguru | 2016.02.19 | YRS-AT-2799 | Removed table LoginCredential to avoid conflict while loading general information in person maintenance screen

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantGeneral");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_gui_UniqueId", DbType.String, parameterPersId);


                l_dataset_dsLookUpgeneralInfo = new DataSet();
                //Commented by prasad For BT-950,YRS 5.0-1469: Add link to Web Front End
                //db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpgeneralInfo, "GeneralInfo");
                //Added by prasad For BT-950,YRS 5.0-1469: Add link to Web Front End
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpgeneralInfo, l_TableNames);
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpgeneralInfo;
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
            DataSet l_dataset_dsLookUpPOAInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantPOA");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_gui_UniqueId", DbType.String, parameterPersId);


                l_dataset_dsLookUpPOAInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpPOAInfo, "POAInfo");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpPOAInfo;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// To get the Primary Address
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpPrimaryAddressInfo(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpAddressInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantPriActAddress");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_gui_UniqueId", DbType.String, parameterPersId);


                l_dataset_dsLookUpAddressInfo = new DataSet();
                l_TableNames = new string[] { "Address", "EmailInfo", "TelephoneInfo" };
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAddressInfo, l_TableNames);
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpAddressInfo;
            }
            catch
            {
                throw;
            }
        }




        /// <summary>
        /// To get the Primary or Secondary Address
        /// </summary>
        /// <param name="parameterPersId","parameterIsPrimary"></param>
        /// <returns></returns>
        public static DataSet SearchParicipantAddress(string parameterPerssId, int parameterIsPrimary)
        {
            DataSet l_dataset_dsSearchAddressInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantAddress");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_gui_UniqueId", DbType.String, parameterPerssId);
                db.AddInParameter(LookUpCommandWrapper, "@bit_Primary", DbType.Int32, parameterIsPrimary);

                l_dataset_dsSearchAddressInfo = new DataSet();
                l_TableNames = new string[] { "AddressInfo" };
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsSearchAddressInfo, l_TableNames);
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsSearchAddressInfo;
            }
            catch
            {
                throw;
            }
        }
        /// Code added by Vartika Jain on 18th Nov 2005
        /// <summary>
        /// To get the Secondary Address
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpSecondaryAddress(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpSecAddressInfo = null;
            Database db = null;
            DbCommand LookUpSecAddCommandWrapper = null;
            string[] l_SecTableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpSecAddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantSecAddress");

                if (LookUpSecAddCommandWrapper == null) return null;

                db.AddInParameter(LookUpSecAddCommandWrapper, "@varchar_gui_UniqueId", DbType.String, parameterPersId);


                l_dataset_dsLookUpSecAddressInfo = new DataSet();
                l_SecTableNames = new string[] { "AddressInfo", "TelephoneInfo" };
                db.LoadDataSet(LookUpSecAddCommandWrapper, l_dataset_dsLookUpSecAddressInfo, l_SecTableNames);

                return l_dataset_dsLookUpSecAddressInfo;
            }
            catch
            {
                throw;
            }
        }

        /// Code added by Vartika Jain on 18th Nov 2005
        /// <summary>
        /// To update the Address
        /// </summary>
        /// Changed by Ruchi on 20th March to pass dataset as parameter
        public static void UpdateParticipantAddress(DataSet parameterdsAddress)
        {
            Database db = null;


            DbCommand AddCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;


            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return;

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateMemAddress");

                if (UpdateCommandWrapper == null) return;

                db.AddInParameter(UpdateCommandWrapper, "@varchar_guiUniqueId", DbType.String, "UniqueId", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_guiEntityId", DbType.String, "guiEntityId", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Address1", DbType.String, "addr1", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Address2", DbType.String, "addr2", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Address3", DbType.String, "addr3", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_City", DbType.String, "city", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_StateType", DbType.String, "state", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Zip", DbType.String, "zipCode", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Active", DbType.Boolean, "isActive", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Primary", DbType.Boolean, "isPrimary", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@date_EffectiveDate", DbType.String, "effectiveDate", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_BadAddress", DbType.Boolean, "isBadAddress", DataRowVersion.Current);

                UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_AddMemAddress");
                if (AddCommandWrapper == null) return;


                db.AddInParameter(AddCommandWrapper, "@varchar_guiEntityId", DbType.String, "guiEntityId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address1", DbType.String, "addr1", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address2", DbType.String, "addr2", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address3", DbType.String, "addr3", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_City", DbType.String, "city", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_StateType", DbType.String, "state", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Zip", DbType.String, "zipCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Active", DbType.Boolean, "isActive", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Primary", DbType.Boolean, "isPrimary", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@date_EffectiveDate", DbType.String, "effectiveDate", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_BadAddress", DbType.Boolean, "isBadAddress", DataRowVersion.Default);


                //Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                db.UpdateDataSet(parameterdsAddress, "Address", AddCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);


                //db.AddInParameter(UpdateCommandWrapper, "@varchar_guiUniqueId", DbType.String, "UniqueId", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@varchar_guiEntityId", DbType.String, "EntityId", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@varchar_Address1", DbType.String, "Address1", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@varchar_Address2", DbType.String, "Address2", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@varchar_Address3", DbType.String, "Address3", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@varchar_City", DbType.String, "City", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@varchar_StateType", DbType.String, "State", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@varchar_Zip", DbType.String, "Zip", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@varchar_Country", DbType.String, "Country", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@bit_Active", DbType.Boolean, "bitActive", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@bit_Primary", DbType.Boolean, "bitPrimary", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@date_EffectiveDate", DbType.String, "EffDate", DataRowVersion.Current);
                //db.AddInParameter(UpdateCommandWrapper, "@bit_BadAddress", DbType.Boolean, "bitBadAddress", DataRowVersion.Current);

                //UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                //AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_AddMemAddress");
                //if (AddCommandWrapper == null) return;


                //db.AddInParameter(AddCommandWrapper, "@varchar_guiEntityId", DbType.String, "EntityId", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@varchar_Address1", DbType.String, "Address1", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@varchar_Address2", DbType.String, "Address2", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@varchar_Address3", DbType.String, "Address3", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@varchar_City", DbType.String, "City", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@varchar_StateType", DbType.String, "State", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@varchar_Zip", DbType.String, "Zip", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@varchar_Country", DbType.String, "Country", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@bit_Active", DbType.Boolean, "bitActive", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@bit_Primary", DbType.Boolean, "bitPrimary", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@date_EffectiveDate", DbType.String, "EffDate", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@bit_BadAddress", DbType.Boolean, "bitBadAddress", DataRowVersion.Current);


                //AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                //db.UpdateDataSet(parameterdsAddress, "AddressInfo", AddCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);




            }
            catch
            {
                throw;
            }
        }

        //START: SB | 07/07/2016 | YRS-AT-2382 | Insert Beneficiary Changed SSN in  Audit Table
        public static void InsertBeneficiariesSSNChangeAuditRecord(DataSet dsAuditEntries)
        {
            Database db = null;
            DbCommand AddCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return;
  
                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_InsertAtsYRSAuditLog");
                if (AddCommandWrapper == null) return;

                db.AddInParameter(AddCommandWrapper, "@chvModuleName", DbType.String, "ModuleName", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@guiEntityId", DbType.String, "UniqueID", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvEntityType", DbType.String, "EntityType", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvColumn", DbType.String, "chvColumn", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvOldValue", DbType.String, "OldSSN", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvNewValue", DbType.String, "NewSSN", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@chvDescription", DbType.String, "Reason", DataRowVersion.Current);
                db.AddOutParameter(AddCommandWrapper, "@bintUniqueID", DbType.Int32, 10);
                
                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                db.UpdateDataSet(dsAuditEntries, "Audit", AddCommandWrapper, null, null, UpdateBehavior.Standard);
            }
            catch
            {
                throw;
            }
        }
        //END : SB | 07/07/2016 | YRS-AT-2382 | Insert Beneficiary Changed SSN in  Audit Table

        //public static void UpdateParticipantAddress2(DataSet parameterdsAddress)
        //{
        //    Database db = null;


        //    DbCommand AddCommandWrapper = null;
        //    DbCommand UpdateCommandWrapper = null;
        //    DbCommand deleteCommandWrapper = null;


        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");

        //        if (db == null) return;

        //        UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateMemAddress");

        //        if (UpdateCommandWrapper == null) return;

        //        db.AddInParameter(UpdateCommandWrapper, "@varchar_guiUniqueId", DbType.String, "UniqueId", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@varchar_guiEntityId", DbType.String, "guiEntityId", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@varchar_Address1", DbType.String, "addr1", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@varchar_Address2", DbType.String, "addr2", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@varchar_Address3", DbType.String, "addr3", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@varchar_City", DbType.String, "city", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@varchar_StateType", DbType.String, "state", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@varchar_Zip", DbType.String, "zipCode", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@bit_Active", DbType.Boolean, "isActive", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@bit_Primary", DbType.Boolean, "isPrimary", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@date_EffectiveDate", DbType.String, "effectiveDate", DataRowVersion.Current);
        //        db.AddInParameter(UpdateCommandWrapper, "@bit_BadAddress", DbType.Boolean, "isBadAddress", DataRowVersion.Current);

        //        UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

        //        AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_AddMemAddress");
        //        if (AddCommandWrapper == null) return;


        //        db.AddInParameter(AddCommandWrapper, "@varchar_guiEntityId", DbType.String, "guiEntityId", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@varchar_Address1", DbType.String, "addr1", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@varchar_Address2", DbType.String, "addr2", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@varchar_Address3", DbType.String, "addr3", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@varchar_City", DbType.String, "city", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@varchar_StateType", DbType.String, "state", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@varchar_Zip", DbType.String, "zipCode", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@bit_Active", DbType.Boolean, "isActive", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@bit_Primary", DbType.Boolean, "isPrimary", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@date_EffectiveDate", DbType.String, "effectiveDate", DataRowVersion.Current);
        //        db.AddInParameter(AddCommandWrapper, "@bit_BadAddress", DbType.Boolean, "isBadAddress", DataRowVersion.Default);



        //        AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

        //        db.UpdateDataSet(parameterdsAddress, "Address", AddCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);




        //        return;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public static void UpdateTelephone(DataSet parameterdsAddress)
        {
            Database db = null;


            DbCommand AddCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;


            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return;

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateMemTelephone");

                if (UpdateCommandWrapper == null) return;


                db.AddInParameter(UpdateCommandWrapper, "@varchar_gui_UniqueId", DbType.String, "UniqueIdPhone", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_gui_EntityId", DbType.String, "EntityId", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Telephone", DbType.String, "PhoneNumber", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_TelephoneType", DbType.String, "PhoneType", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Active", DbType.Boolean, "bitActive", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Primary", DbType.Boolean, "bitPrimary", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@date_EffectiveDate", DbType.String, "EffDate", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Extension", DbType.String, "Ext", DataRowVersion.Current);

                UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                //BS:2012.05.29:BT:951:YRS 5.0-1470: Link to Address Edit program from Person Maintenance
                //AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_AddMemTelephone");

                //if (UpdateCommandWrapper == null) return;

                //db.AddInParameter(AddCommandWrapper, "@varchar_gui_EntityId", DbType.String, "EntityId", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@varchar_Telephone", DbType.String, "PhoneNumber", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@varchar_TelephoneType", DbType.String, "PhoneType", DataRowVersion.Current);
                //db.AddInParameter(AddCommandWrapper, "@bit_Active", DbType.Boolean, "bitActive", DataRowVersion.Current); 
                //db.AddInParameter(AddCommandWrapper, "@bit_Primary", DbType.Boolean, "bitPrimary", DataRowVersion.Current); db.AddInParameter(AddCommandWrapper, "@date_EffectiveDate", DbType.String, "EffDate", DataRowVersion.Current);

                //AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                //db.UpdateDataSet(parameterdsAddress, "TelephoneInfo", AddCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateMemTelephone");

                if (AddCommandWrapper == null) return;

                db.AddInParameter(AddCommandWrapper, "@varchar_gui_UniqueId", DbType.String, "UniqueIdPhone", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_gui_EntityId", DbType.String, "EntityId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Telephone", DbType.String, "PhoneNumber", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_TelephoneType", DbType.String, "PhoneType", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Active", DbType.Boolean, "bitActive", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Primary", DbType.Boolean, "bitPrimary", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@date_EffectiveDate", DbType.String, "EffDate", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Extension", DbType.String, "Ext", DataRowVersion.Current);

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.UpdateDataSet(parameterdsAddress, "TelephoneInfo", AddCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                return;
            }
            catch
            {
                throw;
            }
        }
        public static void UpdateEmailAddress(DataSet parameterdsAddress)
        {
            Database db = null;


            DbCommand AddCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;


            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return;

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateMemEmail");

                if (UpdateCommandWrapper == null) return;


                db.AddInParameter(UpdateCommandWrapper, "@varchar_gui_UniqueId", DbType.String, "UniqueIdMail", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_gui_EntityId", DbType.String, "EntityId", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Email", DbType.String, "EmailAddress", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Active", DbType.Boolean, "bitActive", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Primary", DbType.Boolean, "bitPrimary", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@date_EffectiveDate", DbType.String, "EffDate", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_BadEmailAddress", DbType.Boolean, "IsBadAddress", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Unsubscribed", DbType.Boolean, "IsUnsubscribed", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_TextOnly", DbType.Boolean, "IsTextOnly", DataRowVersion.Current);

                UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_AddMemEmail");

                if (AddCommandWrapper == null) return;



                db.AddInParameter(AddCommandWrapper, "@varchar_gui_EntityId", DbType.String, "EntityId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Email", DbType.String, "EmailAddress", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Active", DbType.Boolean, "bitActive", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Primary", DbType.Boolean, "bitPrimary", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@date_EffectiveDate", DbType.String, "EffDate", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_BadEmailAddress", DbType.Boolean, "IsBadAddress", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Unsubscribed", DbType.Boolean, "IsUnsubscribed", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_TextOnly", DbType.Boolean, "IsTextOnly", DataRowVersion.Current);

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.UpdateDataSet(parameterdsAddress, "EmailInfo", AddCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);




                return;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// To get the Employment Info
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <param name="parameterFundId"></param>
        /// <returns></returns>
        public static DataSet LookUpEmploymentInfo(string parameterPersId, string parameterFundId)
        {
            DataSet l_dataset_dsLookUpEmploymentInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantEmployment");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId", DbType.String, parameterPersId);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FundId", DbType.String, parameterFundId);

                l_dataset_dsLookUpEmploymentInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpEmploymentInfo, "EmploymentInfo");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpEmploymentInfo;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// To gate the fundevent info
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <param name="parameterFundId"></param>
        /// <returns></returns>
        public static DataSet LookUpFundEventInfo(string parameterPersId, string parameterFundId)
        {
            DataSet l_dataset_dsLookUpFundEventInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantFundEvent");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId", DbType.String, parameterPersId);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FundId", DbType.String, parameterFundId);

                l_dataset_dsLookUpFundEventInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpFundEventInfo, "FundEventInfo");
                return l_dataset_dsLookUpFundEventInfo;
            }
            catch
            {
                throw;
            }
        }

        public static bool UpdateService(string p_string_FundEventId, string p_string_VestingDate, string p_string_VestingReason, int p_integer_Paid, int p_integer_NonPaid)
        {
            Database db = null;
            DbCommand UpdateCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return false;

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_Person_UpdateService");

                if (UpdateCommandWrapper == null) return false;

                db.AddInParameter(UpdateCommandWrapper, "@p_varchar_FundEventId", DbType.String, p_string_FundEventId);
                db.AddInParameter(UpdateCommandWrapper, "@p_varchar_VestingDate", DbType.String, p_string_VestingDate);
                db.AddInParameter(UpdateCommandWrapper, "@p_varchar_VestingReason", DbType.String, p_string_VestingReason);
                db.AddInParameter(UpdateCommandWrapper, "@p_integer_Paid", DbType.String, p_integer_Paid);
                db.AddInParameter(UpdateCommandWrapper, "@p_integer_NonPaid", DbType.String, p_integer_NonPaid);
                db.ExecuteNonQuery(UpdateCommandWrapper);

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// To get the additional account information
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpAddAccountInfo(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpAddAccountInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantAdditionalAccounts");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId", DbType.String, parameterPersId);


                l_dataset_dsLookUpAddAccountInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAddAccountInfo, "AddAccountInfo");

                return l_dataset_dsLookUpAddAccountInfo;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// To get the account contributions for a pers id
        /// </summary>
        /// <param name="parameterStartDate"></param>
        /// <param name="parameterEndDate"></param>
        /// <param name="parameterPersId"></param>
        /// <param name="parameterFlag"></param>
        /// <returns></returns>
        public static DataSet LookUpAccountContributionInfo(string parameterStartDate, string parameterEndDate, string parameterFundEventID, bool parameterFlag)
        {
            DataSet l_dataset_dsLookUpAccountContributionInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string[] l_TableNames;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;
                if (parameterFlag == true)
                {
                    LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Sel_TransSumByAccountFundedDate");
                }
                else
                {
                    LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Sel_TransSumByAccountTransactDate");
                }


                if (LookUpCommandWrapper == null) return null;

                //db.AddInParameter(LookUpCommandWrapper,"@persidvchar",DbType.String,parameterPersId);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FundEventId", DbType.String, parameterFundEventID);
                db.AddInParameter(LookUpCommandWrapper, "@startdate", DbType.Date, parameterStartDate);
                db.AddInParameter(LookUpCommandWrapper, "@enddate", DbType.Date, parameterEndDate);

                // Load Accounts for Non Paid 
                l_TableNames = new string[] { "AccountContributions", "AcctContributionsTotal", "AcctContributionsGrandTotal" };

                l_dataset_dsLookUpAccountContributionInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAccountContributionInfo, l_TableNames);

                return l_dataset_dsLookUpAccountContributionInfo;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// To get the list of Beneficiaries
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet LookUpActiveBeneficiariesInfo(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpActiveBeneficiariesInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantActiveBeneficiaries");



                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId", DbType.String, parameterPersId);



                l_dataset_dsLookUpActiveBeneficiariesInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpActiveBeneficiariesInfo, "ActiveBeneficiaries");

                return l_dataset_dsLookUpActiveBeneficiariesInfo;
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
            DataSet l_dataset_dsLookUpRetiredBeneficiariesInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantRetiredBeneficiaries");



                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId", DbType.String, parameterPersId);



                l_dataset_dsLookUpRetiredBeneficiariesInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpRetiredBeneficiariesInfo, "RetiredBeneficiaries");

                return l_dataset_dsLookUpRetiredBeneficiariesInfo;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// To set beneficiary access
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static ArrayList SetBeneficiaryAccess(string parameterPersId)
        {

            Database db = null;
            DbCommand SetBeneficiaryCommandWrapper = null;
            String l_string_Access;
            ArrayList _arrlst = new ArrayList();

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                //start of comment by hafiz on 21Jun2006 as vipul has sent changes from client site
                //SetBeneficiaryCommandWrapper = db.GetStoredProcCommand("ap_Get_BenMaint_Eligibility");
                //end of comment by hafiz on 21Jun2006 as vipul has sent changes from client site

                //start of code by hafiz on 21Jun2006 as vipul has sent changes from client site
                SetBeneficiaryCommandWrapper = db.GetStoredProcCommand("yrs_ap_Get_BenMaint_Eligibility");
                //end of code by hafiz on 21Jun2006 as vipul has sent changes from client site			



                if (SetBeneficiaryCommandWrapper == null) return null;

                db.AddInParameter(SetBeneficiaryCommandWrapper, "@cPersID", DbType.String, parameterPersId);
                db.AddOutParameter(SetBeneficiaryCommandWrapper, "@nActiveBenMaintEligible", DbType.Int32, 2);
                db.AddOutParameter(SetBeneficiaryCommandWrapper, "@nRetiredBenMaintEligible", DbType.Int32, 2);
                db.AddOutParameter(SetBeneficiaryCommandWrapper, "@cOutput", DbType.Int32, 2);

                db.ExecuteNonQuery(SetBeneficiaryCommandWrapper);
                l_string_Access = Convert.ToString(db.GetParameterValue(SetBeneficiaryCommandWrapper, "@nActiveBenMaintEligible"));
                _arrlst.Add(l_string_Access);
                l_string_Access = Convert.ToString(db.GetParameterValue(SetBeneficiaryCommandWrapper, "@nRetiredBenMaintEligible"));
                _arrlst.Add(l_string_Access);

                return _arrlst;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update the Genaral Info details of the Participant
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
        // Commented and added by dilip Yadav : 28-Oct-09 : YRS 5.0.921 : To provide the Priority handling 
        //Priya					19/2/2010			YRS 5.0-988 : fetch value of newly added column paramAllowShareInfo as parameter
        //public static string UpdateGeneralInfo(string parameterPersId,string parameterSSNo,string parameterLast,string parameterFirst,string parameterMiddle,string parameterSal,string parameterSuffix,string parameterGender,string parameterMaritalStatus,string parameterDOB,string parameterSpousalWaiver)
        //bhavnaS 2011.07.08 YRS 5.0-1354 : Change Caption of paramYMCAMailingOptOut
        //bhavnaS 2011.07.11 YRS 5.0-1354 : Add new bit field paramGoPaperlesss
        //public static string UpdateGeneralInfo(string parameterPersId,string parameterSSNo,string parameterLast,string parameterFirst,string parameterMiddle,string parameterSal,string parameterSuffix,string parameterGender,string parameterMaritalStatus,string parameterDOB,string parameterSpousalWaiver,string parameterPriority , string paramAllowShareInfo,string paramYMCAMailingOptOut)
        //{
        //public static string UpdateGeneralInfo(string parameterPersId, string parameterSSNo, string parameterLast, string parameterFirst, string parameterMiddle, string parameterSal, string parameterSuffix, string parameterGender, string parameterMaritalStatus, string parameterDOB, string parameterSpousalWaiver, string parameterPriority, string paramAllowShareInfo, string paramPersonalInfoSharingOptOut, string paramGoPaperless)
        //{
        //BS:2012.01.18:YRS 5.0-1497 -Add edit button to modify the date of death
        //prasad 2012.01.19 YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
        //Start: Bala: 01/19/2019: YRS-AT-2398: Parameter name change
        //AA:2014.02.06:BT:2316 :YRS 5.0-2262 - Added parameter to add or update CannotLocateSpouse value in Atsmetaperssdetails
        //public static string UpdateGeneralInfo(string parameterPersId, string parameterSSNo, string parameterLast, string parameterFirst, string parameterMiddle, string parameterSal, string parameterSuffix, string parameterGender, string parameterMaritalStatus, string parameterDOB, string parameterSpousalWaiver, string parameterPriority, string paramAllowShareInfo, string paramPersonalInfoSharingOptOut, string paramGoPaperless, string paramUpdatedDeceasedDate, Object paramMRD, string paramCannotLocateSpouse, string paramWebLockOut,string paramUpdationFrom) //Bala: 01/19/2019: YRS-AT-2398: Column name change.
        public static string UpdateGeneralInfo(string parameterPersId, string parameterSSNo, string parameterLast, string parameterFirst, string parameterMiddle, string parameterSal, string parameterSuffix, string parameterGender, string parameterMaritalStatus, string parameterDOB, string parameterSpousalWaiver, string parameterExhaustedDBSettle, string paramAllowShareInfo, string paramPersonalInfoSharingOptOut, string paramGoPaperless, string paramUpdatedDeceasedDate, Object paramMRD, string paramCannotLocateSpouse, string paramWebLockOut, string paramUpdationFrom)
        {
            //End: Bala: 01/19/2019: YRS-AT-2398: Parameter name change
            Database db = null;
            DbCommand UpdateCommandWrapper = null;
            String l_string_Output;


            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateParticipantGeneral");



                if (UpdateCommandWrapper == null) return null;

                db.AddInParameter(UpdateCommandWrapper, "@varchar_gui_UniqueId", DbType.String, parameterPersId);

                db.AddInParameter(UpdateCommandWrapper, "@varchar_SSNo", DbType.String, parameterSSNo);
                db.AddInParameter(UpdateCommandWrapper, "@date_DOB", DbType.String, parameterDOB);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_LastName", DbType.String, parameterLast);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_FirstName", DbType.String, parameterFirst);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_MiddleName", DbType.String, parameterMiddle);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Salutation", DbType.String, parameterSal);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Suffix", DbType.String, parameterSuffix);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Gender", DbType.String, parameterGender);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_MaritalStatus", DbType.String, parameterMaritalStatus);
                db.AddInParameter(UpdateCommandWrapper, "@date_SpousalWaiver", DbType.String, parameterSpousalWaiver);
                //Below Line is added by Dilip Yadav : 28-Oct-09 : YRS 5.0.921 : To provide priority handling
                //Start: Bala: 01/19/2019: YRS-AT-2398: Column name change
                //db.AddInParameter(UpdateCommandWrapper, "@bit_Priority", DbType.Boolean, parameterPriority);
                db.AddInParameter(UpdateCommandWrapper, "@bitExhaustedDBSettle", DbType.Boolean, parameterExhaustedDBSettle);
                //End: Bala: 01/19/2019: YRS-AT-2398: Column name change
                //Priya	19/2/2010 YRS 5.0-988 : fetch value of newly added column paramAllowShareInfo as parameter
                db.AddInParameter(UpdateCommandWrapper, "@bit_ShareInfoAllowed", DbType.Boolean, paramAllowShareInfo);
                //Priya:5-April-2010:YRS 5.0-1042 : fetch value of newly aaded column bitYmcaMailOptOut
                //db.AddInParameter(UpdateCommandWrapper,"@bit_YmcaMailOptOut",DbType.Boolean,paramYMCAMailingOptOut);
                //bhavnaS 2011.07.08 :YRS 10.1.1.4 :change object and string name 
                db.AddInParameter(UpdateCommandWrapper, "@bit_PersonalInfoSharingOptOut", DbType.Boolean, paramPersonalInfoSharingOptOut);
                //bhavnaS 2011.07.11 : YRS 5.0-1354 :add new bit field bit_GoPaperless
                db.AddInParameter(UpdateCommandWrapper, "@bit_GoPaperless", DbType.Boolean, paramGoPaperless);
                //BS:2012.01.18:YRS 5.0-1497 -Add edit button to modify the date of death
                db.AddInParameter(UpdateCommandWrapper, "@date_UpdatedDeceasedDate", DbType.String, paramUpdatedDeceasedDate);
                //prasad 2012.01.19 YRS 5.0-1400 : Need abiltiy to regenerate MRD records for individual
                db.AddInParameter(UpdateCommandWrapper, "@bit_MRD", DbType.Boolean, paramMRD);
                //AA:2014.02.06:BT:2316 :YRS 5.0-2262 - Added parameter to sp
                db.AddInParameter(UpdateCommandWrapper, "@varchar_CannotLocateSpouse", DbType.String, paramCannotLocateSpouse);
                //AA:2014.02.06:BT:2391 :YRS 5.0-2247 - Added parameter to sp
                db.AddInParameter(UpdateCommandWrapper, "@varchar_WebLockOut", DbType.String, paramWebLockOut);

                db.AddInParameter(UpdateCommandWrapper, "@varchar_UpdationFrom", DbType.String, paramUpdationFrom);

                db.AddOutParameter(UpdateCommandWrapper, "@varchar_output", DbType.String, 200);

                UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.ExecuteNonQuery(UpdateCommandWrapper);
                l_string_Output = Convert.ToString(db.GetParameterValue(UpdateCommandWrapper, "@varchar_output"));



                return l_string_Output;
            }
            catch
            {
                throw;
            }
        }
        //Priya	19-July-2010        Integration of 10-June-2010:	YRS 5.0-1104:Showing non-vested person as vested (Change parameter persid to fundeventid)
        //Change parameter persid to fundeventid
        //public static int IsVested(string parameterPersId) old code
        public static int IsVested(string parameterFundEventId)
        {
            Database db = null;
            int l_isvested = 0;
            DbCommand GetCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                GetCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMPMIsVested");
                db.AddInParameter(GetCommandWrapper, "@varchar_FundEventId", DbType.String, parameterFundEventId);
                db.AddOutParameter(GetCommandWrapper, "@int_VestStatus", DbType.Int32, 2);
                if (GetCommandWrapper == null) return 0;
                db.ExecuteScalar(GetCommandWrapper);
                l_isvested = Convert.ToInt32(db.GetParameterValue(GetCommandWrapper, "int_VestStatus"));
                return l_isvested;

            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// To get the QDRO details for a particiapnt
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <returns></returns>
        public static DataSet GetQDROInfo(string parameterPersId, string parameterQdroType)
        {
            DataSet l_dataset_dsLookUpQdroInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantGeneralQDRO");

                if (LookUpCommandWrapper == null) return null;


                db.AddInParameter(LookUpCommandWrapper, "@varchar_gui_PersId", DbType.String, parameterPersId);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_QdroType", DbType.String, parameterQdroType);

                l_dataset_dsLookUpQdroInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpQdroInfo, "QdroInfo");
                return l_dataset_dsLookUpQdroInfo;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to update QDRO details based on the Unique Id
        /// </summary>
        /// <param name="parameterUniqueId"></param>
        /// <param name="parameterStatus"></param>
        /// <param name="parameterType"></param>
        /// <param name="parameterStatusDate"></param>
        /// <param name="parameterDraftDate"></param>
        /// <returns></returns>
        public static string UpdateQDROInfo(string parameterUniqueId, string parameterStatus, string parameterType, string parameterStatusDate, string parameterDraftDate, string persID) //Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Passed parameter for Perss ID
        {

            Database db = null;
            DbCommand UpdateCommandWrapper = null;
            String l_output;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateParticipantQDRO");

                if (UpdateCommandWrapper == null) return null;


                db.AddInParameter(UpdateCommandWrapper, "@varchar_gui_UniqueId", DbType.String, parameterUniqueId);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Type", DbType.String, parameterType);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Status", DbType.String, parameterStatus);
                db.AddInParameter(UpdateCommandWrapper, "@date_StatusDate", DbType.String, parameterStatusDate);
                db.AddInParameter(UpdateCommandWrapper, "@date_DraftDate", DbType.String, parameterDraftDate);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_PersID", DbType.String, persID); //Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Passed parameter for Perss ID
                db.AddOutParameter(UpdateCommandWrapper, "@int_output", DbType.Int32, 1);

                UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.ExecuteNonQuery(UpdateCommandWrapper);
                l_output = Convert.ToString(db.GetParameterValue(UpdateCommandWrapper, "@int_output"));

                return l_output;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Method to Add QDRO Details
        /// </summary>
        /// <param name="parameterPersId"></param>
        /// <param name="parameterFundId"></param>
        /// <param name="parameterStatus"></param>
        /// <param name="parameterType"></param>
        /// <param name="parameterStatusDate"></param>
        /// <param name="parameterDraftDate"></param>
        /// <returns></returns>

        public static string AddQDROInfo(string parameterPersId, string parameterFundId, string parameterType, string parameterStatus, string parameterStatusDate, string parameterDraftDate)
        {

            Database db = null;
            DbCommand AddCommandWrapper = null;
            String l_output;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_AddParticipantQDRO");

                if (AddCommandWrapper == null) return null;


                db.AddInParameter(AddCommandWrapper, "@varchar_gui_PersId", DbType.String, parameterPersId);
                db.AddInParameter(AddCommandWrapper, "@varchar_gui_FundId", DbType.String, parameterFundId);
                db.AddInParameter(AddCommandWrapper, "@varchar_Type", DbType.String, parameterType);
                db.AddInParameter(AddCommandWrapper, "@varchar_Status", DbType.String, parameterStatus);
                db.AddInParameter(AddCommandWrapper, "@string_StatusDate", DbType.String, parameterStatusDate);
                db.AddInParameter(AddCommandWrapper, "@string_DraftDate", DbType.String, parameterDraftDate);
                db.AddOutParameter(AddCommandWrapper, "@int_output", DbType.Int32, 1);

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.ExecuteNonQuery(AddCommandWrapper);
                l_output = Convert.ToString(db.GetParameterValue(AddCommandWrapper, "@int_output"));

                return l_output;
            }
            catch
            {
                throw;
            }
        }



        public static int DeathNotifyPrerequisites(string paramPersonOrFundEvent, string param_PersonOrFundEventID, DateTime paramDeathDate, out string l_string_Output_DR, out string l_string_Output_DA)
        {	//Method Added by Vipul Patel - 5th October
            //This methis is used by Death Notificaion to check if any pending annuity exist for the participant passed
            try
            {
                //lcResult = THIS.deathnotifycheckpre(lcID, ldDeathDate) // Foxpro Code / Start
                l_string_Output_DR = "";
                l_string_Output_DA = "";
                Database db = null;
                DbCommand CommandWrapperCheckPrerequisites = null;
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return -1;

                CommandWrapperCheckPrerequisites = db.GetStoredProcCommand("ap_Dth_Calc_PreReq");
                if (CommandWrapperCheckPrerequisites == null) return -1;

                db.AddInParameter(CommandWrapperCheckPrerequisites, "@cPersID", DbType.String, param_PersonOrFundEventID);
                db.AddInParameter(CommandWrapperCheckPrerequisites, "@dDeathDate", DbType.DateTime, paramDeathDate);
                //CommandWrapperCheckPrerequisites.AddOutParameter("@cOutput",DbType.String,4 );  -- commented by SR :2010.01.04 for Gemini 635
                db.AddOutParameter(CommandWrapperCheckPrerequisites, "@cOutput1", DbType.String, 1000);
                db.AddOutParameter(CommandWrapperCheckPrerequisites, "@cOutput2", DbType.String, 1000);

                db.ExecuteNonQuery(CommandWrapperCheckPrerequisites);

                l_string_Output_DR = db.GetParameterValue(CommandWrapperCheckPrerequisites, "@cOutput1").ToString();
                l_string_Output_DA = db.GetParameterValue(CommandWrapperCheckPrerequisites, "@cOutput2").ToString();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        //NP:IVP1:2008.04.15 - Adding parameter TreatMLas to instruct code on how to treat ML fund statuses.
        //BS:2012.03.02:BT-941,YRS 5.0-1432: add one out parameter for report and Add tran and commit statment
        public static int DeathNotifyActions(string paramPersonOrFundEvent, string param_PersonOrFundEventID, DateTime paramDeathDate, out string l_string_Output, out string l_string_DeathNotifyOutput, string paramTreatMLas, string strParticipantSSno, out string l_string_showreport)
        {
            //public static int DeathNotifyActions(string paramPersonOrFundEvent, string param_PersonOrFundEventID, DateTime paramDeathDate, out string l_string_Output, string paramTreatMLas)
            //{
            //Method Added by Vipul Patel - 5th October
            //This methis is used by Death Notification to Perform the DeathNotificationActions for the participant 
            //BS:2012.03.02:BT-941,YRS 5.0-1432: declare connection,transac,db variable
            DbConnection cn = null;
            DbTransaction tran = null;
            Database db = null;
            try
            {
                //BS:2012.03.02:BT-941,YRS 5.0-1432: assign variables
                int int_ReportOuput;
                l_string_showreport = "";



                //Database db = null;
                DbCommand CommandWrapperDeathNotifyActions = null;
                l_string_Output = "";
                l_string_DeathNotifyOutput = "";
                string l_string_StoredProcedureName = string.Empty;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return -1;


                //Get a connection and Open it//BS:2012.03.02:BT-941,YRS 5.0-1432:create conn and open it
                cn = db.CreateConnection();
                cn.Open();

                //Get a Transaction from the Database//BS:2012.03.02:BT-941,YRS 5.0-1432: tran begin
                tran = cn.BeginTransaction(IsolationLevel.Serializable);
                if (tran == null) return -1;




                //lcResult = THIS.DeathNotifyActions(lcDeathScope, lcID, ldDeathDate) / Start 
                if (paramPersonOrFundEvent.ToString() == "PERSON")
                {
                    //Changes By:Preeti On:8th May 2006 For Active and Inactive pre retirement death benefit enhancement
                    //Changed Procedure name . Calling new proc instead of ap_Dth_Upd_Person.
                    CommandWrapperDeathNotifyActions = db.GetStoredProcCommand("YRS_Dth_Upd_Person");
                    l_string_StoredProcedureName = "YRS_Dth_Upd_Person";
                    db.AddInParameter(CommandWrapperDeathNotifyActions, "@cPersID", DbType.String, param_PersonOrFundEventID);
                    db.AddInParameter(CommandWrapperDeathNotifyActions, "@varchar_Participantssno", DbType.String, strParticipantSSno);
                    db.AddInParameter(CommandWrapperDeathNotifyActions, "@dDeathDate", DbType.DateTime, paramDeathDate);
                    db.AddOutParameter(CommandWrapperDeathNotifyActions, "@cOutput", DbType.String, 1000);	//NP:PS:2007.10.03 - Increasing the length from 4 to 1000
                    db.AddInParameter(CommandWrapperDeathNotifyActions, "@cTreatMLAs", DbType.String, paramTreatMLas);	//NP:IVP1:2008.04.15 - Passing parameter to instruct code whether to treat ML as DA or DI
                    //DineshK:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
                    db.AddOutParameter(CommandWrapperDeathNotifyActions, "@cDeathNotifyOutPut", DbType.String, 1000);
                }
                else
                {
                    CommandWrapperDeathNotifyActions = db.GetStoredProcCommand("YRS_Dth_Upd_FundEvent");
                    l_string_StoredProcedureName = "YRS_Dth_Upd_FundEvent";

                    db.AddInParameter(CommandWrapperDeathNotifyActions, "@cFundEventID", DbType.String, param_PersonOrFundEventID);
                    db.AddInParameter(CommandWrapperDeathNotifyActions, "@dDeathDate", DbType.DateTime, paramDeathDate);
                    db.AddOutParameter(CommandWrapperDeathNotifyActions, "@cOutput", DbType.String, 1000);	//NP:PS:2007.10.03 - Increasing the length from 4 to 1000
                    db.AddInParameter(CommandWrapperDeathNotifyActions, "@cTreatMLAs", DbType.String, paramTreatMLas);	//NP:IVP1:2008.04.15 - Passing parameter to instruct code whether to treat ML as DA or DI
                    //DineshK:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
                    db.AddOutParameter(CommandWrapperDeathNotifyActions, "@cDeathNotifyOutPut", DbType.String, 1000);
                }
                if (CommandWrapperDeathNotifyActions == null) return -1;

                db.ExecuteNonQuery(CommandWrapperDeathNotifyActions);

                l_string_Output = db.GetParameterValue(CommandWrapperDeathNotifyActions, "@cOutput").ToString();
                //DineshK:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death             
                l_string_DeathNotifyOutput = db.GetParameterValue(CommandWrapperDeathNotifyActions, "@cDeathNotifyOutPut").ToString();

                //BS:2012.03.02:BT-941,YRS 5.0-1432:comment
                //if ((l_string_Output.ToString() != "") && (l_string_Output.ToString() != "System.DBNull"))
                //{
                //    l_string_Output = "Error executing stored procedure " + l_string_StoredProcedureName + @": \n" + l_string_Output;
                //    return 0;
                //}
                //else
                //{
                //    return 1;
                //}

                //BS:2012.03.02:BT-941,YRS 5.0-1432:
                //if death notification is successfull then  call report procedure otherwise rollback
                if (l_string_Output.Trim() == "")
                {

                    int_ReportOuput = GetRpt_UncashedChecksAfterDeath(param_PersonOrFundEventID, out   l_string_showreport, db, tran);//call report method
                    //if report call successfull then commit tran otherwise rollback death notification also
                    if (int_ReportOuput < 0)
                    {
                        tran.Rollback();
                        return -2;
                    }
                    else
                    {
                        tran.Commit();
                        return 1;
                    }
                }
                else
                {
                    // Code Commented by DInesh.k on 10/06/2013 : BugID 2076: Improper validation while doing death notification.
                    //DineshK:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death   //DineshK:2012.12.27:FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death   
                    //l_string_Output = "Error executing stored procedure " + l_string_StoredProcedureName + @": \n" + l_string_Output;
                    //l_string_DeathNotifyOutput = "Error executing stored procedure " + l_string_StoredProcedureName + @": \n" + l_string_DeathNotifyOutput;
                    tran.Rollback();
                    return 0;
                }
            }
            catch
            {
                throw;
            }

            finally
            {
                cn.Close();
                cn.Dispose();
                tran.Dispose();
                db = null;
            }

        }
        
        public static DataSet SearchYMCAMetro(string parameterSearchYMCANo, string parameterSearchYMCAName, string parameterSearchYMCACity, string parameterSearchYMCAState)
        {
            DataSet dsSearchYMCAMetro = null;
            Database db = null;
            DbCommand CommandSearchYMCAMetro = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                CommandSearchYMCAMetro = db.GetStoredProcCommand("yrs_usp_AMP_SearchYMCAMetro");
                if (CommandSearchYMCAMetro == null) return null;

                db.AddInParameter(CommandSearchYMCAMetro, "@char_YmcaNo", DbType.String, parameterSearchYMCANo);
                db.AddInParameter(CommandSearchYMCAMetro, "@varchar_YmcaName", DbType.String, parameterSearchYMCAName);
                db.AddInParameter(CommandSearchYMCAMetro, "@varchar_City", DbType.String, parameterSearchYMCACity);
                db.AddInParameter(CommandSearchYMCAMetro, "@char_StateType", DbType.String, parameterSearchYMCAState);
                dsSearchYMCAMetro = new DataSet();

                db.LoadDataSet(CommandSearchYMCAMetro, dsSearchYMCAMetro, "YMCA Metro");

                return dsSearchYMCAMetro;
            }
            catch
            {
                throw;
            }

        }

        public static DataSet SearchYMCABranch(string parameterSearchYMCAId)
        {
            DataSet dsSearchYMCABranch = null;
            Database db = null;
            DbCommand CommandSearchYMCAMetro = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                CommandSearchYMCAMetro = db.GetStoredProcCommand("yrs_usp_AMP_SearchYMCABranch");
                if (CommandSearchYMCAMetro == null) return null;

                db.AddInParameter(CommandSearchYMCAMetro, "@varchar_UniqueId", DbType.String, parameterSearchYMCAId);

                dsSearchYMCABranch = new DataSet();

                db.LoadDataSet(CommandSearchYMCAMetro, dsSearchYMCABranch, "YMCA Branch");

                return dsSearchYMCABranch;
            }
            catch
            {
                throw;
            }

        }

        public static DataSet SearchYMCABranchOnCriteria(string parameterSearchYMCANo, string parameterSearchYMCAName, string parameterSearchYMCACity, string parameterSearchYMCAState, string parameterYmcaId)
        {
            DataSet dsSearchYMCAMetro = null;
            Database db = null;
            DbCommand CommandSearchYMCAMetro = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                CommandSearchYMCAMetro = db.GetStoredProcCommand("yrs_usp_AMP_SearchYMCABranchOnCriteria");
                if (CommandSearchYMCAMetro == null) return null;

                db.AddInParameter(CommandSearchYMCAMetro, "@char_YmcaNo", DbType.String, parameterSearchYMCANo);
                db.AddInParameter(CommandSearchYMCAMetro, "@varchar_YmcaName", DbType.String, parameterSearchYMCAName);
                db.AddInParameter(CommandSearchYMCAMetro, "@varchar_City", DbType.String, parameterSearchYMCACity);
                db.AddInParameter(CommandSearchYMCAMetro, "@char_StateType", DbType.String, parameterSearchYMCAState);
                db.AddInParameter(CommandSearchYMCAMetro, "@varchar_UniqueId", DbType.String, parameterYmcaId);
                dsSearchYMCAMetro = new DataSet();

                db.LoadDataSet(CommandSearchYMCAMetro, dsSearchYMCAMetro, "YMCA Branches");

                return dsSearchYMCAMetro;
            }
            catch
            {
                throw;
            }

        }


        public static string SearchYMCAStatus(string parameterSearchYMCAId)
        {

            Database db = null;
            String l_string_status;
            DbCommand CommandSearchYMCAMetro = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                CommandSearchYMCAMetro = db.GetStoredProcCommand("yrs_usp_AMP_SearchYMCAStatus");
                if (CommandSearchYMCAMetro == null) return null;

                db.AddInParameter(CommandSearchYMCAMetro, "@varchar_UniqueId", DbType.String, parameterSearchYMCAId);
                db.AddOutParameter(CommandSearchYMCAMetro, "@int_output", DbType.Int32, 4);
                db.ExecuteNonQuery(CommandSearchYMCAMetro);
                l_string_status = Convert.ToString(db.GetParameterValue(CommandSearchYMCAMetro, "@int_output"));
                return l_string_status;

            }
            catch
            {
                throw;
            }

        }


        public static void InsertParticipantNotes(DataSet parameterYMCAInsertNotes)
        {
            Database parameterDatabase = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;
            //	IDbTransaction l_IDbTransaction = null;
            //	IDbConnection l_IDbConnection = null;



            try
            {
                //db=DatabaseFactory.CreateDatabase("YRS");

                parameterDatabase = DatabaseFactory.CreateDatabase("YRS");

                if (parameterDatabase == null) return;
                //				l_IDbConnection =parameterDatabase.GetConnection();
                //				l_IDbConnection.Open();
                //				if (l_IDbConnection == null) return;
                //				l_IDbTransaction = l_IDbConnection.BeginTransaction();

                insertCommandWrapper = parameterDatabase.GetStoredProcCommand("yrs_usp_AMY_InsertYMCANotes");
                insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                // Defining The Insert Command Wrapper With parameters
                if (insertCommandWrapper != null)
                {
                    //db.AddInParameter(insertCommandWrapper,"@guid_guiUniqueID",DbType.Guid,"UniqueID",DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@varchar_guiUniqueID", DbType.Guid, "UniqueID", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@varchar_PersonID", DbType.Guid, "PersonID", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@varchar_NoteTypeCode", DbType.String, 1);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@text_Note", DbType.String, "Note", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@bit_Important", DbType.Boolean, "bitImportant", DataRowVersion.Current);
                    //parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_Creator",DbType.String ,"Creator",DataRowVersion.Current);
                    //commented by aparna -02/01/2007--not required taken from database
                    //parameterDatabase.AddInParameter(insertCommandWrapper,"@datetime_Date",DbType.DateTime,"Date",DataRowVersion.Current);

                    //Vipul - to fix the Creator Bug 04-Feb-06
                    //parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_Creator",DbType.String ,"Creator",DataRowVersion.Current);
                    //Vipul - to fix the Creator Bug 04-Feb-06
                    //commented by aparna -02/01/2007

                }
                //
                //								deleteCommandWrapper = parameterDatabase.GetStoredProcCommand("yrs_usp_AMY_DeleteYMCAContact");
                //								// Defining The Delete Command Wrapper With parameters
                //								deleteCommandWrapper.AddInParameter("@uniqueIdentifier_guiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Original);
                //								deleteCommandWrapper.AddInParameter("@uniqueIdentifier_guiYmcaID",DbType.String,"guiYmcaID",DataRowVersion.Original);
                //					


                //by Aparna  YREN-3115 9/03/2007
                //TO update teh bitImportant value in the atsnotes table

                updateCommandWrapper = parameterDatabase.GetStoredProcCommand("yrs_usp_AMY_UpdateYMCANotes");
                updateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                if (updateCommandWrapper != null)
                {

                    parameterDatabase.AddInParameter(updateCommandWrapper, "@varchar_guiUniqueID", DbType.Guid, "UniqueID", DataRowVersion.Current);
                    //db.AddInParameter(UpdateCommandWrapper,"@varchar_PersonID",DbType.String,"PersonID",DataRowVersion.Current);
                    parameterDatabase.AddInParameter(updateCommandWrapper, "@bit_Important", DbType.Boolean, "bitImportant", DataRowVersion.Current);



                    //by Aparna  YREN-3115 9/03/2007
                }

                if (deleteCommandWrapper != null)
                {
                    //nothing
                }

                if (parameterYMCAInsertNotes != null)
                {
                    parameterDatabase.UpdateDataSet(parameterYMCAInsertNotes, "Member Notes", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                    //	parameterDatabase.UpdateDataSet(parameterYMCAInsertNotes,"Member Notes" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,l_IDbTransaction);
                    //	l_IDbTransaction.Commit();			
                }

            }

            catch
            {
                throw;
            }
            //			catch(SqlException SqlEx)
            //			{  
            //				l_IDbTransaction.Rollback();
            //				throw SqlEx;
            //			}
            //			catch
            //			{
            //				l_IDbTransaction.Rollback();
            //				throw;
            //			}
            //			finally
            //			{
            //				if (l_IDbConnection != null)
            //				{
            //					if (l_IDbConnection.State != ConnectionState.Closed)
            //					{
            //						l_IDbConnection.Close ();
            //					}
            //				}
            //			}
        }

        public static DataTable MemberNotes(string paramaeterPersonID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_AMP_GetNotes");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, paramaeterPersonID);

                l_DataSet = new DataSet("Member Notes");

                l_TableNames = new string[] { "Member Notes" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Member Notes"]);
                }

                return null;


            }
            catch
            {
                throw;
            }
        }
        public static string GetNotesById(string parameterUniqueId)
        {

            Database db = null;
            DbCommand GetCommandWrapper = null;
            String l_string_Output;


            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                GetCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_GetNotesById");
                if (GetCommandWrapper == null) return null;

                db.AddInParameter(GetCommandWrapper, "@varchar_UniqueId", DbType.String, parameterUniqueId);
                db.AddOutParameter(GetCommandWrapper, "@varchar_Notes", DbType.String, 500);

                db.ExecuteNonQuery(GetCommandWrapper);
                l_string_Output = Convert.ToString(db.GetParameterValue(GetCommandWrapper, "@varchar_Notes"));



                return l_string_Output;
            }
            catch
            {
                throw;
            }
        }

        public static void InsertBeneficiaries(DataSet dsBeneficiaries)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;
            string strNewUniqueId;
            try
            {
                //DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
                db = DatabaseFactory.CreateDatabase("YRS");
                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_InsertActiveBeneficiaries");

                //db.AddInParameter(insertCommandWrapper, "@BeneficiaryName1", DbType.String, "Name", DataRowVersion.Current);
                //db.AddInParameter(insertCommandWrapper, "@BeneficiaryName2", DbType.String, "Name2", DataRowVersion.Current);
                //db.AddInParameter(insertCommandWrapper, "@RelationShipCode", DbType.String, "Rel", DataRowVersion.Current);
                //db.AddInParameter(insertCommandWrapper, "@BeneficiaryGroupCode", DbType.String, "Groups", DataRowVersion.Current);
                ////NP:PS:2007.06.26 - Adding input parameter to allow beneficiaries of type SAVING to be added 
                //db.AddInParameter(insertCommandWrapper, "@BeneficiaryTypeCode", DbType.String, "BeneficiaryTypeCode", DataRowVersion.Current);
                //db.AddInParameter(insertCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, "TaxID", DataRowVersion.Current);
                //db.AddInParameter(insertCommandWrapper, "@BeneficiaryLevelCode", DbType.String, "Lvl", DataRowVersion.Current);
                //db.AddInParameter(insertCommandWrapper, "@BenefitPCTG", DbType.Double, "Pct", DataRowVersion.Current);
                //db.AddInParameter(insertCommandWrapper, "@BirthDate", DbType.String, "Birthdate", DataRowVersion.Current);
                //db.AddInParameter(insertCommandWrapper, "@Persid", DbType.String, "PersId", DataRowVersion.Current);

                //insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                foreach (DataRow dr in dsBeneficiaries.Tables[0].Rows)
                {
                    //Anudeep:12.07.2013 : Bt-1501:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (dr.RowState == DataRowState.Added || string.IsNullOrEmpty(dr["UniqueID"].ToString()))
                        {
                            insertCommandWrapper.Parameters.Clear();
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryName1", DbType.String, dr["Name"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryName2", DbType.String, dr["Name2"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RelationShipCode", DbType.String, dr["Rel"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryGroupCode", DbType.String, dr["Groups"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, dr["TaxID"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryLevelCode", DbType.String, dr["Lvl"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BenefitPCTG", DbType.Double, Convert.ToDouble(dr["Pct"].ToString()));
                            db.AddInParameter(insertCommandWrapper, "@BirthDate", DbType.String, dr["Birthdate"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@Persid", DbType.String, dr["PersId"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryTypeCode", DbType.String, dr["BeneficiaryTypeCode"].ToString());


                            //SP 2014.11.27 BT-2310\YRS 5.0-2255 - Start
                            db.AddInParameter(insertCommandWrapper, "@RepFirstName", DbType.String, string.IsNullOrEmpty(dr["RepFirstName"].ToString()) ? null : dr["RepFirstName"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RepLastName", DbType.String, string.IsNullOrEmpty(dr["RepLastName"].ToString()) ? null : dr["RepLastName"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RepSalutationCode", DbType.String, string.IsNullOrEmpty(dr["RepSalutation"].ToString()) ? null : dr["RepSalutation"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RepTelephoneNo", DbType.String, string.IsNullOrEmpty(dr["RepTelephone"].ToString()) ? null : dr["RepTelephone"].ToString());
                            //SP 2014.11.27 BT-2310\YRS 5.0-2255 - End
                            db.AddInParameter(insertCommandWrapper, "@INT_DeletedBeneficiaryID", DbType.Int32, dr["DeletedBeneficiaryID"]); //MMR | 2017.12.04 | YRS-AT-3756 | Added parameter for deleted beneficiary ID
                            db.AddOutParameter(insertCommandWrapper, "@stringUniqueID", DbType.String, 1000);
                            insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                            db.ExecuteNonQuery(insertCommandWrapper);
                            strNewUniqueId = db.GetParameterValue(insertCommandWrapper, "@stringUniqueID").ToString();
                            dr["UniqueID"] = strNewUniqueId;
                            dr.AcceptChanges();
                        }

                    }
                }

                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateActiveBeneficiaries");

                db.AddInParameter(updateCommandWrapper, "@BeneficiaryName1", DbType.String, "Name", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryName2", DbType.String, "Name2", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RelationShipCode", DbType.String, "Rel", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryGroupCode", DbType.String, "Groups", DataRowVersion.Current);
                //NP:PS:2007.06.26 - Adding input parameter to allow beneficiaries of type SAVING to be added
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryTypeCode", DbType.String, "BeneficiaryTypeCode", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, "TaxID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryLevelCode", DbType.String, "Lvl", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BenefitPCTG", DbType.Double, "Pct", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BirthDate", DbType.String, "Birthdate", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@stringPersid", DbType.String, "PersId", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@stringUniqueID", DbType.String, "UniqueID", DataRowVersion.Current);

                //SP 2014.11.27 BT-2310\YRS 5.0-2255 - Start
                db.AddInParameter(updateCommandWrapper, "@RepFirstName", DbType.String, "RepFirstName", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RepLastName", DbType.String, "RepLastName", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RepSalutationCode", DbType.String, "RepSalutation", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RepTelephoneNo", DbType.String, "RepTelephone", DataRowVersion.Current);
                //SP 2014.11.27 BT-2310\YRS 5.0-2255 - End
                db.AddInParameter(updateCommandWrapper, "@INT_DeletedBeneficiaryID", DbType.Int32, "DeletedBeneficiaryID", DataRowVersion.Current); //MMR | 2017.12.04 | YRS-AT-3756 | Added parameter for updating beneficiary of deceased beneficiary

                updateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                //Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                //deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeleteBeneficiaries");
                deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addrs_DeleteBeneficiary");
                db.AddInParameter(deleteCommandWrapper, "@guiBeneficiaryId", DbType.String, "UniqueID", DataRowVersion.Original);
                db.AddInParameter(deleteCommandWrapper, "@VARCHAR_ReasonCode", DbType.String, "DeathReason", DataRowVersion.Original); //MMR | 2017.12.04 | YRS-AT-3756 | Added parameter for reason of delete
                db.AddInParameter(deleteCommandWrapper, "@VARCHAR_DeathDate", DbType.String, "DeathDate", DataRowVersion.Original); //MMR | 2017.12.04 | YRS-AT-3756 | Added parameter for reason of delete
                //db.AddInParameter(deleteCommandWrapper, "@guiMappingId", DbType.String, "MappingID", DataRowVersion.Original);
                //db.AddInParameter(deleteCommandWrapper, "@UniqueID", DbType.String, "UniqueID", DataRowVersion.Original);

                deleteCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);


                // UpdateDataSet method has 6 parameters (Dataset,Table Name,
                //insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
                //UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
                //so a reference of it is passed.

                if (dsBeneficiaries != null)
                {
                    db.UpdateDataSet(dsBeneficiaries, "ActiveBeneficiaries", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                    dsBeneficiaries.AcceptChanges();
                }


            }
            catch
            {
                throw;
            }
        }
        public static string AddEmployment(DataSet parameterDSEmployment)
        {

            Database db = null;
            DbCommand InsertCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

            String l_string_Output;


            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;



                InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_InsertParticipantEmployment");
                if (InsertCommandWrapper == null) return null;

                db.AddInParameter(InsertCommandWrapper, "@varchar_PersId", DbType.String, "PersonId", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@varchar_YmcaId", DbType.String, "YmcaId", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@varchar_FundEventId", DbType.String, "FundEventId", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@varchar_BranchId", DbType.String, "BranchId", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@date_HireDate", DbType.String, "HireDate", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@date_TermDate", DbType.String, "TermDate", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@date_EligDate", DbType.String, "EligibilityDate", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@varchar_PositionType", DbType.String, "PositionType", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@bit_Professional", DbType.Int32, "Professional", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@bit_Salaried", DbType.Int32, "Salaried", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@bit_FullTime", DbType.Int32, "FullTime", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@int_PriorService", DbType.Int32, "PriorService", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@varchar_StatusType", DbType.String, "StatusType", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@date_StatusDate", DbType.String, "StatusDate", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@date_EnrollmentDate", DbType.String, "BasicPaymentDate", DataRowVersion.Current);
                db.AddInParameter(InsertCommandWrapper, "@bit_active", DbType.Int32, "Active", DataRowVersion.Current);

                db.AddOutParameter(InsertCommandWrapper, "@int_output", DbType.String, 2);

                InsertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateParticipantEmployment");
                if (UpdateCommandWrapper == null) return null;

                db.AddInParameter(UpdateCommandWrapper, "@varchar_UniqueId", DbType.String, "UniqueId", DataRowVersion.Current);

                db.AddInParameter(UpdateCommandWrapper, "@date_HireDate", DbType.String, "HireDate", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@date_TermDate", DbType.String, "TermDate", DataRowVersion.Current);

                //added by hafiz on 15-Nov-2006
                db.AddInParameter(UpdateCommandWrapper, "@int_PriorService", DbType.Int32, "PriorService", DataRowVersion.Current);

                db.AddInParameter(UpdateCommandWrapper, "@date_EnrollmentDate", DbType.String, "BasicPaymentDate", DataRowVersion.Current);
                //START : Added by Dilip yadav : 11-Nov-2009 : YRS 5.0.941
                db.AddInParameter(UpdateCommandWrapper, "@varchar_PositionType", DbType.String, "PositionType", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Professional", DbType.Int32, "Professional", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Salaried", DbType.Int32, "Salaried", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_FullTime", DbType.Int32, "FullTime", DataRowVersion.Current);
                // END : Added by Dilip yadav : 11-Nov-2009  : YRS 5.0.941

                db.AddOutParameter(UpdateCommandWrapper, "@int_output", DbType.String, 2);

                UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                //db.ExecuteNonQuery(InsertCommandWrapper);
                db.UpdateDataSet(parameterDSEmployment, "EmploymentInfo", InsertCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                l_string_Output = Convert.ToString(db.GetParameterValue(InsertCommandWrapper, "@int_output"));

                //START: SB | 2017.03.17 | YRS-AT-2606 | Updating Participation Date by Earliest  Enrollment Date
                if (l_string_Output == "0" || l_string_Output == "")
                {
                    if (HelperFunctions.isNonEmpty(parameterDSEmployment))
                    {
                        String l_string_fundEventId = parameterDSEmployment.Tables[0].Rows[0]["FundEventId"].ToString();

                        UpdateParticipationDate(l_string_fundEventId);
                    }
                }

                //END: SB | 2017.03.17 | YRS-AT-2606 | Updating Participation Date by Earliest  Enrollment Date

                return l_string_Output;
            }
            catch
            {
                throw;
            }
        }


        public static string AddAdditionalAccount(DataSet parameterDSAddAccount)
        {

            Database db = null;
            DbCommand AddCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

            String l_output;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_AddAdditionalAccount");

                if (AddCommandWrapper == null) return null;


                db.AddInParameter(AddCommandWrapper, "@varchar_EmpEventId", DbType.String, "EmpEventId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_YmcaId", DbType.String, "YmcaId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_AcctType", DbType.String, "AccountType", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_BasisCode", DbType.String, "BasisCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@numeric_AddlPctg", DbType.Double, "Contribution%", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@numeric_AddlContibution", DbType.Double, "Contribution", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@datetime_EffectiveDate", DbType.String, "EffectiveDate", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@datetime_TerminationDate", DbType.String, "TerminationDate", DataRowVersion.Current);
                db.AddOutParameter(AddCommandWrapper, "@int_output", DbType.Int32, 1);

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                l_output = Convert.ToString(db.GetParameterValue(AddCommandWrapper, "@int_output"));

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateAdditionalAccount");

                if (UpdateCommandWrapper == null) return null;


                db.AddInParameter(UpdateCommandWrapper, "@varchar_UniqueId", DbType.String, "UniqueId", DataRowVersion.Current);
                //Added by prasad 2012.02.08 FOR BT-990,YRS 5.0-1531 - Need to be able to update atsEmpElectives termination date
                db.AddInParameter(UpdateCommandWrapper, "@datetime_EffectiveDate", DbType.String, "EffectiveDate", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@datetime_TerminationDate", DbType.String, "TerminationDate", DataRowVersion.Current);
                db.AddOutParameter(UpdateCommandWrapper, "@int_output", DbType.Int32, 1);

                UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.UpdateDataSet(parameterDSAddAccount, "AddAccountInfo", AddCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                l_output = Convert.ToString(db.GetParameterValue(AddCommandWrapper, "@int_output"));



                return l_output;
            }
            catch
            {
                throw;
            }
        }




        public static bool InsertBeneficiaryNotes(string strReason, string strDOD, string strBenificiaryName, string strComments, bool bitImportant, string strGuiEntityId, string strTypeCode, string strBeneUniqueID,string BeneficarySSNo)
        {
            Database oDatabase = null;
            DbCommand oDbCommand = null;
            bool bRetvalue = false;
            try
            {
                oDatabase = DatabaseFactory.CreateDatabase("YRS");
                oDbCommand = oDatabase.GetStoredProcCommand("yrs_usp_InsertBeneficiaryNotes");
                oDatabase.AddInParameter(oDbCommand, "@chvReason", DbType.String, strReason.Trim());
                if (!string.IsNullOrEmpty(strDOD))
                {
                    oDatabase.AddInParameter(oDbCommand, "@dtmDOD", DbType.DateTime, Convert.ToDateTime(strDOD));
                }
                else
                {
                    oDatabase.AddInParameter(oDbCommand, "@dtmDOD", DbType.DateTime, null);
                }
                oDatabase.AddInParameter(oDbCommand, "@chvBenificaryName", DbType.String, strBenificiaryName.Trim());
                oDatabase.AddInParameter(oDbCommand, "@txtComments", DbType.String, strComments.Trim());
                oDatabase.AddInParameter(oDbCommand, "@bitImportant", DbType.Boolean, bitImportant);
                oDatabase.AddInParameter(oDbCommand, "@guiEntityID", DbType.String, strGuiEntityId.Trim());
                oDatabase.AddInParameter(oDbCommand, "@chvNoteTypeCode", DbType.String, strTypeCode.Trim());
                oDatabase.AddInParameter(oDbCommand, "@guiBeneUniqueID", DbType.String, strBeneUniqueID.Trim());
                oDatabase.AddInParameter(oDbCommand, "@VARCHAR_BeneSSNo", DbType.String, BeneficarySSNo.Trim()); // SR | 2016.09.21 | YRS-AT-3028 -  Add new parameter to capture deceased beneficiary SSN
                oDbCommand.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                oDatabase.ExecuteNonQuery(oDbCommand);
                bRetvalue = true;
            }
            catch
            {
                throw;
            }
            finally
            {
                oDatabase = null;
                oDbCommand.Dispose();
            }
            return bRetvalue;
        }

        public static void InsertRetiredBeneficiaries(DataSet dsBeneficiaries)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;
            string strNewUniqueId;

            try
            {
                //DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_InsertRetiredBeneficiaries");
                foreach (DataRow dr in dsBeneficiaries.Tables[0].Rows)
                {
                    //Anudeep:12.07.2013 : Bt-1501:BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (dr.RowState == DataRowState.Added || string.IsNullOrEmpty(dr["UniqueID"].ToString()))
                        {
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryName1", DbType.String, "Name", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryName2", DbType.String, "Name2", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@RelationShipCode", DbType.String, "Rel", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryGroupCode", DbType.String, "Groups", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, "TaxID", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryLevelCode", DbType.String, "Lvl", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BenefitPCTG", DbType.Double, "Pct", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BirthDate", DbType.String, "Birthdate", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@Persid", DbType.String, "PersId", DataRowVersion.Current);
                            //db.AddInParameter(insertCommandWrapper, "@BeneficiaryTypeCode", DbType.String, "BeneficiaryTypeCode", DataRowVersion.Current);
                            //insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                            insertCommandWrapper.Parameters.Clear();
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryName1", DbType.String, dr["Name"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryName2", DbType.String, dr["Name2"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RelationShipCode", DbType.String, dr["Rel"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryGroupCode", DbType.String, dr["Groups"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, dr["TaxID"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryLevelCode", DbType.String, dr["Lvl"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BenefitPCTG", DbType.Double, Convert.ToDouble(dr["Pct"].ToString()));
                            db.AddInParameter(insertCommandWrapper, "@BirthDate", DbType.String, dr["Birthdate"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@Persid", DbType.String, dr["PersId"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@BeneficiaryTypeCode", DbType.String, dr["BeneficiaryTypeCode"].ToString());

                            //SP 2014.11.27 BT-2310\YRS 5.0-2255 - Start
                            db.AddInParameter(insertCommandWrapper, "@RepFirstName", DbType.String, string.IsNullOrEmpty(dr["RepFirstName"].ToString()) ? null : dr["RepFirstName"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RepLastName", DbType.String, string.IsNullOrEmpty(dr["RepLastName"].ToString()) ? null : dr["RepLastName"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RepSalutationCode", DbType.String, string.IsNullOrEmpty(dr["RepSalutation"].ToString()) ? null : dr["RepSalutation"].ToString());
                            db.AddInParameter(insertCommandWrapper, "@RepTelephoneNo", DbType.String, string.IsNullOrEmpty(dr["RepTelephone"].ToString()) ? null : dr["RepTelephone"].ToString());
                            //SP 2014.11.27 BT-2310\YRS 5.0-2255 - End                            
                            db.AddInParameter(insertCommandWrapper, "@INT_DeletedBeneficiaryID", DbType.Int32, dr["DeletedBeneficiaryID"]); //MMR | 2017.12.04 | YRS-AT-3756 | Added parameter for deleted beneficiary ID

                            db.AddOutParameter(insertCommandWrapper, "@stringUniqueID", DbType.String, 1000);
                            insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                            db.ExecuteNonQuery(insertCommandWrapper);
                            strNewUniqueId = db.GetParameterValue(insertCommandWrapper, "@stringUniqueID").ToString();
                            dr["UniqueID"] = strNewUniqueId;
                            dr.AcceptChanges();
                        }
                    }
                }

                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateRetiredBeneficiaries");

                db.AddInParameter(updateCommandWrapper, "@BeneficiaryName1", DbType.String, "Name", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryName2", DbType.String, "Name2", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RelationShipCode", DbType.String, "Rel", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryGroupCode", DbType.String, "Groups", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, "TaxID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryLevelCode", DbType.String, "Lvl", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BenefitPCTG", DbType.Double, "Pct", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BirthDate", DbType.String, "Birthdate", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@stringPersid", DbType.String, "PersId", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@stringUniqueID", DbType.String, "UniqueID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@BeneficiaryType", DbType.String, "BeneficiaryTypeCode", DataRowVersion.Current);

                //SP 2014.11.27 BT-2310\YRS 5.0-2255 - Start
                db.AddInParameter(updateCommandWrapper, "@RepFirstName", DbType.String, "RepFirstName", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RepLastName", DbType.String, "RepLastName", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RepSalutationCode", DbType.String, "RepSalutation", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@RepTelephoneNo", DbType.String, "RepTelephone", DataRowVersion.Current);
                //SP 2014.11.27 BT-2310\YRS 5.0-2255 - End
                db.AddInParameter(updateCommandWrapper, "@INT_DeletedBeneficiaryID", DbType.Int32, "DeletedBeneficiaryID", DataRowVersion.Current); //MMR | 2017.12.04 | YRS-AT-3756 | Added parameter for updating beneficiary of deceased beneficiary

                updateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                //Anudeep:01.07.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                //deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeleteBeneficiaries");
                deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addrs_DeleteBeneficiary");
                db.AddInParameter(deleteCommandWrapper, "@guiBeneficiaryId", DbType.String, "UniqueID", DataRowVersion.Original);
                db.AddInParameter(deleteCommandWrapper, "@VARCHAR_ReasonCode", DbType.String, "DeathReason", DataRowVersion.Original); //MMR | 2017.12.04 | YRS-AT-3756 | Added parameter for reason of delete
                db.AddInParameter(deleteCommandWrapper, "@VARCHAR_DeathDate", DbType.String, "DeathDate", DataRowVersion.Original); //MMR | 2017.12.04 | YRS-AT-3756 | Added parameter for reason of delete
				//db.AddInParameter(deleteCommandWrapper, "@guiMappingId", DbType.String, "MappingID", DataRowVersion.Original);
                //db.AddInParameter(deleteCommandWrapper, "@UniqueID", DbType.String, "UniqueID", DataRowVersion.Original);

                deleteCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                // UpdateDataSet method has 6 parameters (Dataset,Table Name,
                //insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
                //UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
                //so a reference of it is passed.

                if (dsBeneficiaries != null)
                {
                    db.UpdateDataSet(dsBeneficiaries, "RetiredBeneficiaries", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                    dsBeneficiaries.AcceptChanges();
                }

            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetCanBeKilled(string parameterFundId)
        {
            DataSet l_dataset_dsCanBeKilled = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_GetCanBeKilled");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_FundId", DbType.String, parameterFundId);


                l_dataset_dsCanBeKilled = new DataSet();

                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsCanBeKilled, "CanBeKilled");

                return l_dataset_dsCanBeKilled;
            }
            catch
            {
                throw;
            }
        }



        public static void UpdateEmailProperties(string paramUniqueId, string paramEntityid, bool paramBadEmail, bool paramUnsubscribed, bool paramtextOnly, bool paramBitPrimary, bool ParamBitActive, bool paramSecActive, string paramCreator, string paramUpdater)
        {
            Database db = null;
            DbCommand CommandUpdateEmailProperties = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                CommandUpdateEmailProperties = db.GetStoredProcCommand("yrs_usp_AMP_updateEmailProperties");
                db.AddInParameter(CommandUpdateEmailProperties, "@varchar_gui_uniqueId", DbType.String, paramUniqueId);
                db.AddInParameter(CommandUpdateEmailProperties, "@varchar_gui_entityId", DbType.String, paramEntityid);
                db.AddInParameter(CommandUpdateEmailProperties, "@bit_bad_Email", DbType.Boolean, paramBadEmail);
                db.AddInParameter(CommandUpdateEmailProperties, "@bit_Unsubscribed", DbType.Boolean, paramUnsubscribed);
                db.AddInParameter(CommandUpdateEmailProperties, "@bit_TextOnly", DbType.Boolean, paramtextOnly);

                db.AddInParameter(CommandUpdateEmailProperties, "@bit_primary", DbType.Boolean, paramBitPrimary);
                db.AddInParameter(CommandUpdateEmailProperties, "@bit_active", DbType.Boolean, ParamBitActive);

                db.AddInParameter(CommandUpdateEmailProperties, "@bit_SecondaryActive", DbType.Boolean, paramSecActive);
                db.AddInParameter(CommandUpdateEmailProperties, "@varchar_Creator", DbType.String, paramCreator);
                db.AddInParameter(CommandUpdateEmailProperties, "@varchar_Updater", DbType.String, paramUpdater);

                CommandUpdateEmailProperties.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.ExecuteNonQuery(CommandUpdateEmailProperties);

            }
            catch
            {
                throw;
            }
        }
        public static DataSet LookUpPrimaryEmailProperties(string paramPersId)
        {
            Database db = null;
            DbCommand CommandLookUpPrimaryEmailProperties = null;
            DataSet dsLookUpPrimaryEmailProperties = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                CommandLookUpPrimaryEmailProperties = db.GetStoredProcCommand("yrs_usp_AMP_LookupPriEmailProperties");
                if (CommandLookUpPrimaryEmailProperties == null) return null;
                db.AddInParameter(CommandLookUpPrimaryEmailProperties, "@varchar_gui_entityId", DbType.String, paramPersId);
                dsLookUpPrimaryEmailProperties = new DataSet();
                db.LoadDataSet(CommandLookUpPrimaryEmailProperties, dsLookUpPrimaryEmailProperties, "PriEmailProps");
                return dsLookUpPrimaryEmailProperties;

            }
            catch
            {
                throw;
            }
        }
        public static DataSet LookUpSecondaryEmailProperties(string paramPersId)
        {
            Database db = null;
            DbCommand CommandLookUpSecondaryEmailProperties = null;
            DataSet dsLookUpSecondaryEmailProperties = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                CommandLookUpSecondaryEmailProperties = db.GetStoredProcCommand("yrs_usp_AMP_LookupSecEmailProperties");
                if (CommandLookUpSecondaryEmailProperties == null) return null;
                db.AddInParameter(CommandLookUpSecondaryEmailProperties, "@varchar_gui_entityId", DbType.String, paramPersId);
                dsLookUpSecondaryEmailProperties = new DataSet();
                db.LoadDataSet(CommandLookUpSecondaryEmailProperties, dsLookUpSecondaryEmailProperties, "SecEmailProps");
                return dsLookUpSecondaryEmailProperties;
            }
            catch
            {
                throw;
            }
        }

        public static Int32 SuppressedJSAnnuitiesCount(string paramPersId)
        {
            Database db = null;
            DbCommand CommandSuppressedJSAnnuitiesCount = null;
            DataSet dsSuppressedJSAnnuitiesCount = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return 0;
                CommandSuppressedJSAnnuitiesCount = db.GetStoredProcCommand("dbo.yrs_usp_Retire_unsuppressjsannuitiesCount");
                CommandSuppressedJSAnnuitiesCount.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                if (CommandSuppressedJSAnnuitiesCount == null) return 0;
                db.AddInParameter(CommandSuppressedJSAnnuitiesCount, "@guiPerId", DbType.String, paramPersId);
                dsSuppressedJSAnnuitiesCount = new DataSet();
                db.LoadDataSet(CommandSuppressedJSAnnuitiesCount, dsSuppressedJSAnnuitiesCount, "SuppressedJSAnnuitiesCount");
                if (dsSuppressedJSAnnuitiesCount != null)
                {
                    if (dsSuppressedJSAnnuitiesCount.Tables[0].Rows.Count > 0)
                    {
                        return System.Int32.Parse(dsSuppressedJSAnnuitiesCount.Tables[0].Rows[0][0].ToString());
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            catch
            {
                throw;
            }

        }

        public static DataTable UnSuppressJSAnnuitiesCount(string paramPersId, string paramXMLDeduction) // PPP | 04/28/2016 | YRS-AT-2719 | Accepting deductions in XML format (paramXMLDeduction)
        {
            Database db = null;
            DbCommand CommandSuppressedJSAnnuitiesCount = null;
            DataSet dsSuppressedJSAnnuitiesCount = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                CommandSuppressedJSAnnuitiesCount = db.GetStoredProcCommand("dbo.yrs_usp_Retire_unsuppressjsannuities");
                CommandSuppressedJSAnnuitiesCount.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (CommandSuppressedJSAnnuitiesCount == null) return null;
                db.AddInParameter(CommandSuppressedJSAnnuitiesCount, "@guiPerId", DbType.String, paramPersId);
                db.AddInParameter(CommandSuppressedJSAnnuitiesCount, "@xmlDeductions", DbType.Xml, paramXMLDeduction); // PPP | 04/28/2016 | YRS-AT-2719 | Passing deductions in XML format (paramXMLDeduction)
                dsSuppressedJSAnnuitiesCount = new DataSet();
                db.LoadDataSet(CommandSuppressedJSAnnuitiesCount, dsSuppressedJSAnnuitiesCount, "SuppressedJSAnnuitiesCount");
                if (dsSuppressedJSAnnuitiesCount != null)
                {
                    if (dsSuppressedJSAnnuitiesCount.Tables[0].Rows.Count > 0)
                    {
                        return dsSuppressedJSAnnuitiesCount.Tables[0];
                    }
                    else
                        return null;
                }
                else
                    return null;

            }
            catch
            {
                throw;
            }
        }


        public static DataSet LookUp_AMP_StateNames(string l_string_CountryCode)
        {
            DataSet l_dataset_LookUp_StateNames = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_LookUp_StateNames");


                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@varchar_CountryCode", DbType.String, l_string_CountryCode);

                l_dataset_LookUp_StateNames = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_LookUp_StateNames, "r_StateNames");
                return l_dataset_LookUp_StateNames;
            }
            catch
            {
                throw;
            }
        }//

        public static DataSet LookUpLoanDetails(int parameterLoanRequestId)
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            DataSet l_dataset = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_LoanGetDetails");


                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@int_LoanRequestId", DbType.Int16, parameterLoanRequestId);

                l_dataset = new DataSet();
                //last table added by Swopna on 24 Mar,2008
                //START: MMR | 2018.04.06 | YRS-AT-3935 | Commented existing code and added additional table to hold loan EFT payment and bank details
                //l_TableNames = new string[] { "LoanDetails", "LoanAmortization", "LoanPayoffAmount", "LoanYmcaNo", "LoanAccountBreakdown", "LoanPhantomIntDetails", "LoanOffsetReason" }; //AA:07.27.2015 BT:2699:YRS 5.0-2441 : Added extra table name to get loan offset reason
                l_TableNames = new string[] { "LoanDetails", "LoanAmortization", "LoanPayoffAmount", "LoanYmcaNo", "LoanAccountBreakdown", "LoanPhantomIntDetails", "LoanOffsetReason", "LoanPaymentDetails", "PaymentMethodDetails" }; 
                //END : MMR | 2018.04.06 | YRS-AT-3935 | Commented existing code and added additional table to hold loan EFT payment and bank details
                db.LoadDataSet(LookUpCommandWrapper, l_dataset, l_TableNames);
                return l_dataset;
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
            return RetireesInformationDAClass.getRelationShips();
        }
        //NP:PS:2007.06.26 - Adding code to pull appropriate Beneficiary types from the database
        //					for active participants
        public static DataSet getBenefitGroups()
        {
            return RetireesInformationDAClass.getBenefitGroups();
        }
        //NP:PS:2007.06.26 - Adding code to pull appropriate Beneficiary types from the database
        //					for active participants
        public static DataSet getBenefitLevels()
        {
            return RetireesInformationDAClass.getBenefitLevels();
        }

        //NP:PS:2007.06.26 - Adding code to pull appropriate Beneficiary types from the database
        //					for active participants
        public static DataSet getBenefitTypes()
        {
            DataSet l_dataset_dsBenefitTypes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BenefitTypes");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@ParticipantType", DbType.String, "DA");

                l_dataset_dsBenefitTypes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsBenefitTypes, "BenefitTypes");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsBenefitTypes;
            }
            catch
            {
                throw;
            }
        }
        //this function gets fund event status of selected person(Added on 27 May,2008 by Swopna)
        //Ashish:2010.07.21,YRS 5.0-1136, handle multiple fund events, this method return fundevent status for particular fund event Id 
        //public static DataSet GetFundEventStatus(string parameterUniqueId)
        public static DataSet GetFundEventStatus(string parameterPersID, string parameterFundEventID)
        {
            DataSet l_dataset_PersonInfo = null;
            Database db = null;
            string[] l_TableNames;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_GetFundEventStatus");
                db.AddInParameter(LookUpCommandWrapper, "@varchar_GuiPersId", DbType.String, parameterPersID);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_guiFundEventId", DbType.String, parameterFundEventID);
                if (LookUpCommandWrapper == null) return null;

                l_dataset_PersonInfo = new DataSet();
                l_TableNames = new string[] { "FundEventStatus" };
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_PersonInfo, l_TableNames);

                return l_dataset_PersonInfo;
            }
            catch
            {
                throw;
            }
        }
        // this function called on terminating an employee(By Swopna 27 May,2008 Phase IV)
        public static void UpdateOnParticipantTermination(string GuiPersId)
        {
            Database db = null;
            DbCommand updateCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateEmploymentRecord");

                db.AddInParameter(updateCommandWrapper, "@varchar_GuiPersId", DbType.String, GuiPersId);

                updateCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                db.ExecuteNonQuery(updateCommandWrapper);


            }
            catch
            {
                throw;
            }
        }


        #region  Code For account Lock/Unlock
        /// <summary>
        /// To read the LockReasonCode from Master table "atsMetaLockReasonCode"
        /// </summary>
        /// <param name="paramIsLock">Account Lock Status</param>
        /// <returns></returns>
        public static DataSet GetReasonCodes()
        {
            DataSet l_dataset_dsGetReasonCodes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_GetReasonCodes");

                if (LookUpCommandWrapper == null) return null;
                l_dataset_dsGetReasonCodes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsGetReasonCodes, "GetReasonCode");
                return l_dataset_dsGetReasonCodes;
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// To insert the Locking info in table "AtsParticipantAccountLock"
        /// </summary>
        /// <param name="paramPersId">PersId of the participant for which an entry is being made. </param>
        /// <param name="paramAcctLock">Account Lock bit value</param>
        /// <param name="paramLockReasonCode">Code value indicating why a particular account was locked or unlocked </param>
        //public static void InsertLockInfo(string paramPersId, bool paramAcctLock, string paramLockReasonCode)
        //{
        //    Database db = null;
        //    DbCommand CommandInsertLockReasonCode = null;
        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");
        //        CommandInsertLockReasonCode = db.GetStoredProcCommand("yrs_usp_AMY_InsertLockInfo");
        //        db.AddInParameter(CommandInsertLockReasonCode, "@uniqueIdentifier_persID", DbType.String, paramPersId);
        //        db.AddInParameter(CommandInsertLockReasonCode, "@bit_AcctLock", DbType.Boolean, paramAcctLock);
        //        db.AddInParameter(CommandInsertLockReasonCode, "@varchar_chvReasonCode", DbType.String, paramLockReasonCode);


        //        CommandInsertLockReasonCode.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

        //        db.ExecuteNonQuery(CommandInsertLockReasonCode);

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        /// <summary>
        /// To update the account status in ats perss tabe
        /// </summary>
        /// <param name="paramPersId">Pers id</param>
        /// <param name="paramAcctStatus">Account status</param>

        public static void UpdateAccountLockStatus(string paramPersId, bool paramAcctStatus, string paramLockReasonCode)
        {
            Database db = null;
            DbCommand CommandAccountLockStatus = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                CommandAccountLockStatus = db.GetStoredProcCommand("yrs_usp_Update_AccountLockStatus");
                db.AddInParameter(CommandAccountLockStatus, "@uniqueIdentifier_persID", DbType.String, paramPersId);
                db.AddInParameter(CommandAccountLockStatus, "@bit_AccountStatus", DbType.Boolean, paramAcctStatus);
                db.AddInParameter(CommandAccountLockStatus, "@varchar_chvReasonCode", DbType.String, paramLockReasonCode);
                CommandAccountLockStatus.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                db.ExecuteNonQuery(CommandAccountLockStatus);

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// To read the Account Lock reason details
        /// </summary>
        /// <param name="guiPersId">Pers Id</param>
        /// <returns></returns>
        public static DataSet GetLockReasonDetails(string paramSSN)
        {
            DataSet l_dataset_dsGetLockReasonDetails = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_GetLockReasonDetails");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_SSNO", DbType.String, paramSSN);

                l_dataset_dsGetLockReasonDetails = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsGetLockReasonDetails, "GetLockReasonDetails");
                return l_dataset_dsGetLockReasonDetails;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        public static DataSet GetHeaderDetails(string paramId, string paramType)
        {
            DataSet l_dataset_dsHeaderDetails = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_GetHeaderDetails");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_ParamID", DbType.String, paramId);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_ParamType", DbType.String, paramType);


                l_dataset_dsHeaderDetails = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsHeaderDetails, "GetHeaderDetails");
                return l_dataset_dsHeaderDetails;
            }
            catch
            {
                throw;
            }
        }

        //BS:2012.03.02:YRS 5.0-1432:BT:941:-Method use to open UncashedChecksAfterDeath report after death notification
        public static int GetRpt_UncashedChecksAfterDeath(string paramPersonOrFundEvent, out string l_string_showreport, Database db, DbTransaction tran)
        {
            try
            {
                l_string_showreport = "";
                DbCommand CommandWrapper = null;
                if (db == null) return -1;

                CommandWrapper = db.GetStoredProcCommand("yrs_usp_UT_GetRpt_UncashedChecksAfterDeath");
                if (CommandWrapper == null) return -1;

                db.AddInParameter(CommandWrapper, "@cPersID", DbType.String, paramPersonOrFundEvent);
                db.AddOutParameter(CommandWrapper, "@cOutput", DbType.String, 1000);

                db.ExecuteNonQuery(CommandWrapper, tran);

                l_string_showreport = db.GetParameterValue(CommandWrapper, "@cOutput").ToString();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        //Prasad jadhav 2012.03.12	For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
        public static Boolean transactionexist(string paramPersonId, string terminationdate)
        {
            try
            {
                Database db = null;
                DbCommand CommandWrapper = null;
                Boolean returnval;
                String spval;
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return true;
                CommandWrapper = db.GetStoredProcCommand("yrs_usp_Transaction_Exist_Or_Not");
                if (CommandWrapper == null) return true;
                db.AddInParameter(CommandWrapper, "@varchar_GuiPersId", DbType.String, paramPersonId);
                db.AddInParameter(CommandWrapper, "@date_TerminationDate", DbType.Date, string.IsNullOrEmpty(terminationdate) ? null : terminationdate); //AA:BT:2464 - Modified parameter from string to date for not getting error
                db.AddOutParameter(CommandWrapper, "@boolean_UnableDisable", DbType.Boolean, 1);
                db.ExecuteNonQuery(CommandWrapper);

                spval = db.GetParameterValue(CommandWrapper, "@boolean_UnableDisable").ToString();
                returnval = Boolean.Parse(spval);
                return returnval;
            }
            catch
            {
                throw;
            }
        }
        //Prasad jadhav 2012.03.12	For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
        //prasad jadhav	2012.04.26	For BT-991,YRS 5.0-1530 - Need ability to reactivate a terminated employee
        public static Boolean updateFundEventEmpEvent(string termDate, string eventId, string ymcaId, bool tdcontractexist, bool UpadteFlag, out string FundEventStatusCode, out string FundEventStatusDesc)
        {

            try
            {
                FundEventStatusCode = string.Empty;
                FundEventStatusDesc = string.Empty;
                Database db = null;
                DbCommand CommandWrapper = null;
                Boolean returnval;
                String spval;
                db = DatabaseFactory.CreateDatabase("YRS");
                //Changed  from true to false
                if (db == null) return false;
                CommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateFundEventStatus_EmpEvents");
                //Changed  from true to false
                if (CommandWrapper == null) return false;
                db.AddInParameter(CommandWrapper, "@termDate", DbType.String, termDate);
                db.AddInParameter(CommandWrapper, "@AtsFundEventsUniqueId", DbType.String, eventId);
                db.AddInParameter(CommandWrapper, "@YmcaUniqueId", DbType.String, ymcaId);
                db.AddInParameter(CommandWrapper, "@tdcontractexist", DbType.Boolean, tdcontractexist);
                db.AddInParameter(CommandWrapper, "@UpadteFlag", DbType.Boolean, UpadteFlag);
                db.AddOutParameter(CommandWrapper, "@successfailure", DbType.Boolean, 1);
                db.AddOutParameter(CommandWrapper, "@FundEventStatusCode", DbType.String, 10);
                db.AddOutParameter(CommandWrapper, "@FundEventStatusDesc", DbType.String, 100);
                db.ExecuteNonQuery(CommandWrapper);
                spval = db.GetParameterValue(CommandWrapper, "@successfailure").ToString();
                FundEventStatusCode = db.GetParameterValue(CommandWrapper, "@FundEventStatusCode").ToString();
                FundEventStatusDesc = db.GetParameterValue(CommandWrapper, "@FundEventStatusDesc").ToString();
                returnval = Boolean.Parse(spval);
                return returnval;
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
                Database db = null;
                DbCommand CommandWrapper = null;
                string retrunstring = string.Empty;
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return string.Empty;
                CommandWrapper = db.GetStoredProcCommand("yrs_usp_ymcaweb_DeletetUser");
                if (CommandWrapper == null) return string.Empty;
                db.AddInParameter(CommandWrapper, "@uniqueid", DbType.String, persID);
                db.AddOutParameter(CommandWrapper, "@outputstring", DbType.String, 200);
                db.ExecuteNonQuery(CommandWrapper);
                retrunstring = db.GetParameterValue(CommandWrapper, "@outputstring").ToString();
                return retrunstring;
            }
            catch
            {
                throw;
            }
        }

        //Bhavna Shrivastava 2012.06.26	For BT-991, YRS 5.0-1530 - Need ability to reactivate a terminated employee
        public static Boolean DiffYMCAOnSameTermDateExist(string paramPersonId, string terminationdate)
        {
            try
            {
                Database db = null;
                DbCommand CommandWrapper = null;
                Boolean returnval;
                String spval;
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return true;
                CommandWrapper = db.GetStoredProcCommand("yrs_usp_DiffYMCAOnSameTermDate_Exist_Or_Not");
                if (CommandWrapper == null) return true;
                db.AddInParameter(CommandWrapper, "@varchar_GuiPersId", DbType.String, paramPersonId);
                db.AddInParameter(CommandWrapper, "@date_TerminationDate", DbType.Date, string.IsNullOrEmpty(terminationdate) ? null : terminationdate); //AA:BT:2464 - Modified parameter from string to date for not getting error
                db.AddOutParameter(CommandWrapper, "@boolean_UnableDisable", DbType.Boolean, 1);
                db.ExecuteNonQuery(CommandWrapper);
                spval = db.GetParameterValue(CommandWrapper, "@boolean_UnableDisable").ToString();
                returnval = Boolean.Parse(spval);
                return returnval;
            }
            catch
            {
                throw;
            }
        }


        //SR: 2012.08.17 - BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program

        public static string InsertTerminationWatcher(string PersId, string FundEventId, string type, string PlanType, string Notes, bool isImportant)
        {

            Database db = null;
            DbCommand AddCommandWrapper = null;
            String l_output;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_InsertTerminationWatcher");
                if (AddCommandWrapper == null) return null;

                db.AddInParameter(AddCommandWrapper, "@guiPersId", DbType.String, PersId);
                db.AddInParameter(AddCommandWrapper, "@guiFundEventId", DbType.String, FundEventId);
                db.AddInParameter(AddCommandWrapper, "@chrType", DbType.String, type);
                db.AddInParameter(AddCommandWrapper, "@chvPlanType", DbType.String, PlanType);
                db.AddInParameter(AddCommandWrapper, "@chvSource", DbType.String, "Person");//Anudeep:12.12.2012 Changes made to show source of watcher from 'PERSON' to 'Person'
                db.AddInParameter(AddCommandWrapper, "@Varchar_Notes", DbType.String, Notes);
                db.AddInParameter(AddCommandWrapper, "@bitImportant", DbType.Boolean, isImportant);
                db.AddOutParameter(AddCommandWrapper, "@outReturnValue", DbType.Int32, 1);
                AddCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.ExecuteNonQuery(AddCommandWrapper);

                l_output = Convert.ToString(db.GetParameterValue(AddCommandWrapper, "@outReturnValue"));

                return l_output;

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
                Database db = null;
                DbCommand LookUpCommandWrapper = null;
                DataSet dsTerminationWatcher = null;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_LookUp_MemberListForTerminationWatcher");
                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_SSN", DbType.String, paramSSNo);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_LName", DbType.String, "");
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FName", DbType.String, "");
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FundNo", DbType.String, paramFundNo);
                db.ExecuteNonQuery(LookUpCommandWrapper);

                dsTerminationWatcher = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsTerminationWatcher, "GetMemberTerminationWatcher");
                return dsTerminationWatcher;
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
                Database db = null;
                DbCommand LookUpCommandWrapper = null;
                DataSet dsPersonDetail = null;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_GetPersonDetail");
                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@guiPersId", DbType.String, paramPersId);
                db.AddInParameter(LookUpCommandWrapper, "@guiFundEventId", DbType.String, paramFundEventId);
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsPersonDetail = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsPersonDetail, "GetPersonDetail");
                return dsPersonDetail;
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
                Database db = null;
                DbCommand LookUpCommandWrapper = null;
                DataSet dsPlanType = null;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_GetPlanType");
                if (LookUpCommandWrapper == null) return null;
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsPlanType = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsPlanType, "GetPlanType");
                return dsPlanType;
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
        /// //2014.02.18        Dinesh Kanojia      BT-2139: YRS 5.0-2165:RMD enhancements - Change Procedure name to Update MRD taxrate for an participant.
        /// </summary>
        /// <param name="strGuiPerssID"></param>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        // public static string ModifyPerssMetaConfiguration(string strGuiPerssID, string strKey, string strConfigCode, string strValue, string strDescription, bool bIsActive,string strAction)
        public static string ModifyPerssMetaConfiguration(string strGuiPerssID, string strKey, string strValue)
        {
            Database db = null;
            DbCommand AddCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateAtsMetaDetails");
                if (AddCommandWrapper == null) return null;

                db.AddInParameter(AddCommandWrapper, "@guiPersID", DbType.String, strGuiPerssID);
                db.AddInParameter(AddCommandWrapper, "@chvKey", DbType.String, strKey);
                db.AddInParameter(AddCommandWrapper, "@chvValue", DbType.String, strValue);
                db.AddOutParameter(AddCommandWrapper, "@cOutValue", DbType.String, 20);
                AddCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                db.ExecuteNonQuery(AddCommandWrapper);
                return Convert.ToString(db.GetParameterValue(AddCommandWrapper, "@cOutValue"));
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
                Database db = null;
                DbCommand LookUpCommandWrapper = null;
                DataSet dsPerssMetaConfiguration = null;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetPerssMetaConfigurationDetails");
                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@guiPerssID", DbType.String, strGuiPerssID);
                //db.AddInParameter(LookUpCommandWrapper, "@chvConfigCategoryCode", DbType.String, strConfigCode);
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsPerssMetaConfiguration = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsPerssMetaConfiguration, "PerssMetaConfiguration");
                return dsPerssMetaConfiguration;
            }
            catch
            {
                throw;
            }
        }
        //Start:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To Update the PIN value
        public static String UpdatePIN(string strGuiPersId, string intPIN)
        {
            Database db;
            DbCommand UpdateCommandWrapper;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return "0";
                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdatePersPIN");
                if (UpdateCommandWrapper == null) return "0";
                db.AddInParameter(UpdateCommandWrapper, "@guiPersId", DbType.String, strGuiPersId);
                db.AddInParameter(UpdateCommandWrapper, "@chrPIN", DbType.String, intPIN);
                db.AddOutParameter(UpdateCommandWrapper, "@chrRetPIN", DbType.String, 4);
                db.ExecuteNonQuery(UpdateCommandWrapper);
                return Convert.ToString(db.GetParameterValue(UpdateCommandWrapper, "@chrRetPIN"));
            }
            catch
            {
                throw;
            }
        }
        //End:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To Update the PIN value
        //Start:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To delete the PIN value
        public static String DeletePIN(string strGuiPersId)
        {
            Database db;
            DbCommand UpdateCommandWrapper;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return "0";
                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_DeletePersPIN");
                if (UpdateCommandWrapper == null) return "0";
                db.AddInParameter(UpdateCommandWrapper, "@guiPersId", DbType.String, strGuiPersId);
                db.AddOutParameter(UpdateCommandWrapper, "@chrRetStatus", DbType.String, 4);
                db.ExecuteNonQuery(UpdateCommandWrapper);
                return Convert.ToString(db.GetParameterValue(UpdateCommandWrapper, "@chrRetStatus"));
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
            Database db = null;
            DbCommand dbCommand = null;
            string strExists;
            try
            {
                strExists = string.Empty;
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) throw new Exception("Database object is null");

                dbCommand = db.GetStoredProcCommand("yrs_usp_AMP_CheckSSNExists");

                if (dbCommand == null) throw new Exception("Database object is null");

                db.AddInParameter(dbCommand, "@varchar_PersId", DbType.String, strGuiPerssID);
                db.AddInParameter(dbCommand, "@varchar_SSN", DbType.String, strSSN);
                db.AddOutParameter(dbCommand, "@varchar_Output", DbType.String, 500);

                db.ExecuteNonQuery(dbCommand);

                strExists = Convert.ToString(db.GetParameterValue(dbCommand, "@varchar_Output"));

                return strExists;
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
            Database db = null;
            DbCommand dbCommand = null;
            long lUniqueID;
            try
            {
                lUniqueID = 0;
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) throw new Exception("Database object is null");

                dbCommand = db.GetStoredProcCommand("yrs_usp_AMP_InsertAtsYRSAuditLog");

                if (dbCommand == null) throw new Exception("Database object is null");

                db.AddInParameter(dbCommand, "@chvModuleName", DbType.String, strModuleName);
                db.AddInParameter(dbCommand, "@guiEntityId", DbType.String, strGuiEntityId);
                db.AddInParameter(dbCommand, "@chvEntityType", DbType.String, strEntityType);
                db.AddInParameter(dbCommand, "@chvColumn", DbType.String, strColumn);
                db.AddInParameter(dbCommand, "@chvOldValue", DbType.String, strOldValue);
                db.AddInParameter(dbCommand, "@chvNewValue", DbType.String, strNewValue);
                db.AddInParameter(dbCommand, "@chvDescription", DbType.String, strDescription);
                db.AddOutParameter(dbCommand, "@bintUniqueID", DbType.Int64, 10);

                db.ExecuteNonQuery(dbCommand);

                lUniqueID = Convert.ToInt64(db.GetParameterValue(dbCommand, "@bintUniqueID"));

                return lUniqueID;
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
            Database db = null;
            DbCommand dbCommand = null;
            DataSet dsAuditLogDeatils = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) throw new Exception("Database object is null");

                dbCommand = db.GetStoredProcCommand("yrs_usp_AMP_GetAtsYRSAuditLogByGuiEntityID");

                if (dbCommand == null) throw new Exception("Database object is null");

                db.AddInParameter(dbCommand, "@guiEntityId", DbType.String, strGuiEntityId);
                db.AddInParameter(dbCommand, "@chvColumn", DbType.String, strColumn);

                dsAuditLogDeatils = new DataSet();

                db.LoadDataSet(dbCommand, dsAuditLogDeatils, "AuditLogDeatils");

                return dsAuditLogDeatils;
            }
            catch
            {
                throw;
            }
        }

        //SP 2014.02.03   BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -End

        //SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - Start
        public static DataSet GetSalutationCodes()
        {
            DataSet dsSalutationCodes = null;
            Database db = null;
            DbCommand dbCommand = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) throw new Exception("Database object is null");

                dbCommand = db.GetStoredProcCommand("yrs_usp_Gen_SalutationCode");

                if (dbCommand == null) throw new Exception("dbCommand object is null");

                dsSalutationCodes = new DataSet();
                db.LoadDataSet(dbCommand, dsSalutationCodes, "SalutationCodes");

                return dsSalutationCodes;
            }
            catch
            {
                throw;
            }
        }
        //SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN - End

        //START: PPP | 04/22/2016 | YRS-AT-2719 | Provides annuties details
        public static DataSet GetSuppressedJSAnnuitiesDetails(string strPersID)
        {
            DataSet ds = null;
            Database db = null;
            DbCommand cmd = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_GetSuppressedJSAnnuitiesDetails");
                if (cmd == null) return null;

                db.AddInParameter(cmd, "@VARCHAR_PersID", DbType.String, strPersID);

                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "JSAnnuitiesDetails");

                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }
        //END: PPP | 04/22/2016 | YRS-AT-2719 | Provides annuties details
        //START : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)
        public static DataSet GetQDROSplitDetailsByRequestId(string strQDRORequestId)
        {
            DataSet ds = null;
            Database db = null;
            DbCommand cmd = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_QDRO_GetSplitDetailsByRequestId");
                if (cmd == null) return null;

                db.AddInParameter(cmd, "@varchar_QRDORequestID", DbType.String, strQDRORequestId);
              
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "QDROSplitDetails");

                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }
        //END : Chandrasekar - 2016.07.05 - YRS-AT-2481 - YRS and/or PHR enhancement-store QDRO beneficiary info in participant's acct after split (TrackIT 22116)

        //Start - Manthan Rajguru |2016.07.27 | YRS-AT-2560 | Getting System generated Phony SSN and validating for existing SSN in system
        public static void GeneratePhonySSN(DataSet dsBeneficiaries)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DataView dv = dsBeneficiaries.Tables[0].DefaultView ;
            dv.RowStateFilter = DataViewRowState.OriginalRows;
            string strNewUniqueId;
            string strTaxID;
            string strBeneUniqueID;
            try
            {               
                db = DatabaseFactory.CreateDatabase("YRS");
                insertCommandWrapper = db.GetStoredProcCommand("YRS_USP_BF_SavePhonYSSN");

                foreach (DataRow dr in dsBeneficiaries.Tables[0].Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        strTaxID = dr["TaxID"].ToString();
                        strBeneUniqueID = dr["UniqueID"].ToString();
                        dv.RowFilter = "UniqueID =  '" + dr["UniqueID"].ToString() + "'";                       
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            if (strTaxID.Contains("P") && string.IsNullOrEmpty(strBeneUniqueID))
                            {
                                insertCommandWrapper.Parameters.Clear();                               
                                db.AddInParameter(insertCommandWrapper, "@chvBenID", DbType.String, "");
                                db.AddInParameter(insertCommandWrapper, "@chvExistingPhonySSN", DbType.String, "");
                                db.AddInParameter(insertCommandWrapper, "@chvNewPhonySSN", DbType.String, ((strTaxID == "P********" ? "" : strTaxID)));
                                db.AddInParameter(insertCommandWrapper, "@chvAction_INSERT_UPDATE", DbType.String, "INSERT");
                                db.AddOutParameter(insertCommandWrapper, "@chvPhonySSN", DbType.String, 1000);
                                insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                                db.ExecuteNonQuery(insertCommandWrapper);
                                strNewUniqueId = db.GetParameterValue(insertCommandWrapper, "@chvPhonySSN").ToString();
                                dr["TaxID"] = strNewUniqueId;                               
                            }
                            else if (strTaxID.Contains("P") &&  ! (string.IsNullOrEmpty(strBeneUniqueID)))
                            {                                
                                insertCommandWrapper.Parameters.Clear();
                                db.AddInParameter(insertCommandWrapper, "@chvBenID", DbType.String, strBeneUniqueID);
                                db.AddInParameter(insertCommandWrapper, "@chvExistingPhonySSN", DbType.String, "");
                                db.AddInParameter(insertCommandWrapper, "@chvNewPhonySSN", DbType.String, ((strTaxID == "P********" ? "" : strTaxID)));
                                db.AddInParameter(insertCommandWrapper, "@chvAction_INSERT_UPDATE", DbType.String, "UPDATE");
                                db.AddOutParameter(insertCommandWrapper, "@chvPhonySSN", DbType.String, 1000);
                                insertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                                db.ExecuteNonQuery(insertCommandWrapper);
                                strNewUniqueId = db.GetParameterValue(insertCommandWrapper, "@chvPhonySSN").ToString();               
                                if (dv[0]["TaxID"].ToString() != strNewUniqueId)
                                {
                                    dr["TaxID"] = strNewUniqueId;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        //End - Manthan Rajguru |2016.07.27 | YRS-AT-2560 | Getting System generated Phony SSN and validating for existing SSN in system
        // START : SB | 10/13/2016 | YRS-AT-3095 |  Function to check RMD of deceased participant need to be regenerate
        public static Boolean DeathNotifyActions_IsRMDRegenerateRequired(string FundEventID)
        {
            Database db = null;
            DbCommand AddCommandWrapper = null;
            Boolean RMDRegenerate;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return false;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_RMD_IsRMDRegenerateRequired");

                if (AddCommandWrapper == null) return false;


                db.AddInParameter(AddCommandWrapper, "@UNIQUEIDENTIFIER_FundEventId", DbType.String, FundEventID);
                db.AddOutParameter(AddCommandWrapper, "@BIT_RMDReGenerateYesOrNo", DbType.Boolean, 0);

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.ExecuteNonQuery(AddCommandWrapper);
                RMDRegenerate = Convert.ToBoolean(db.GetParameterValue(AddCommandWrapper, "@BIT_RMDReGenerateYesOrNo"));

                return RMDRegenerate;

            }
            catch
            {

                throw;
            }
            finally
            {
                db = null;
            }
        }
        // END : SB | 10/13/2016 | YRS-AT-3095 |  Function to check RMD of deceased participant need to be regenerate    


        //START: SB | 2017.03.17 | YRS-AT-2606 | Updating Participation Date by Earliest  Enrollment Date Function
        private static bool UpdateParticipationDate(string l_string_fundEventId)
        {
            Database db = null;
            DbCommand AddCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return false;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateParticipationDate");

                if (AddCommandWrapper == null) return false;

                db.AddInParameter(AddCommandWrapper, "@VARCHAR_GuiFundEventId", DbType.String, l_string_fundEventId);

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.ExecuteNonQuery(AddCommandWrapper);

                return true;
            }
            catch
            {
                throw;
            }
        }
        //END: SB | 2017.03.17 | YRS-AT-2606 | Updating Participation Date by Earliest  Enrollment Date Function		

        //START: MMR | 2017.12.04 | YRS-AT-3756 | Added to get deceased beneficiary details
        public static DataSet GetDeceasedBeneficiary(string persId, string beneficiaryType)
        {
            return RetireesInformationDAClass.GetDeceasedBeneficiary(persId, beneficiaryType);
        }
        //END: MMR | 2017.12.04 | YRS-AT-3756 | Added to get deceased beneficiary details


        // START : VC | 2018.08.02 | YRS-AT-4018 -  Method used to list  WEB loans details
        public static DataSet GetWebLoanDetails(int loanNumber)
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            DataSet dataset = null;
            string[] TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_WebLoanGetDetails");


                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@int_LoanNumber", DbType.Int32, loanNumber);

                dataset = new DataSet();
                TableNames = new string[] { "LoanDetails", "LoanDocumentDetails","StatusHistory" };

                db.LoadDataSet(LookUpCommandWrapper, dataset, TableNames);
                return dataset;
            }
            catch
            {

                throw;
            }
        }
        // END : VC | 2018.08.02 | YRS-AT-4018 -  Method used to list  WEB loans details

        // START : VC | 2018.08.03 | YRS-AT-4018 -  Method used to approve a pending web loan 
        public static string ApproveWebLoan(WebLoan webLoan)
        {
            DbCommand cmd = null;
            Database db = null;
            DbTransaction transaction = null;
            DbConnection connection = null;
            string message;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) throw new Exception("Database object is null");
                connection = db.CreateConnection();
                connection.Open();
                if (connection == null) throw new Exception("Connection object is null"); ;
                transaction = connection.BeginTransaction();
                cmd = db.GetStoredProcCommand("yrs_usp_LAC_ApproveWebLoan");
                if (cmd == null) throw new Exception("Database object is null");
                db.AddInParameter(cmd, "@INT_LoanOriginationId", DbType.Int32, webLoan.LoanOriginationId);
                db.AddInParameter(cmd, "@VARCHAR_SignatureReceivedDate", DbType.String, webLoan.SignatureReceivedDate);
                //START : VC | 2018.11.26 | YRS-AT-4018 | Commented code to remove parameters
                //db.AddInParameter(cmd, "@VARCHAR_MaritalStatus", DbType.String, webLoan.MaritalStatus);
                //db.AddInParameter(cmd, "@VARCHAR_DocCode", DbType.String, webLoan.DocCode);
                //END : VC | 2018.11.26 | YRS-AT-4018 | Commented code to remove parameters
                db.AddOutParameter(cmd, "@VARCHAR_Message", DbType.String, 4000);
                db.ExecuteNonQuery(cmd);

                message = db.GetParameterValue(cmd, "@VARCHAR_Message").ToString();
                if (string.IsNullOrEmpty(message))
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
                return message;
            }
            catch(Exception ex)
            {
               transaction.Rollback();
               throw ex;
            }
            finally
            {
                cmd = null;
                db = null;
                if (connection != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                    connection = null;
                }
                connection = null;
            }
        }
        // END : VC | 2018.08.03 | YRS-AT-4018 -  Method used to approve a pending web loan 


        // START : VC | 2018.08.03 | YRS-AT-4018 -  Method used to decline a pending web loan 
        public static string DeclineWebLoan(WebLoan webLoan)
        {
            DbCommand cmd = null;
            Database db = null;
            DbTransaction transaction = null;
            DbConnection connection = null;
            string message;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) throw new Exception("Database object is null");
                connection = db.CreateConnection();
                connection.Open();
                if (connection == null) throw new Exception("Connection object is null"); ;
                transaction = connection.BeginTransaction();
                cmd = db.GetStoredProcCommand("yrs_usp_LAC_DeclineWebLoan");
                if (cmd == null) throw new Exception("Database object is null");
                db.AddInParameter(cmd, "@INT_LoanOriginationId", DbType.Int32, webLoan.LoanOriginationId);
                db.AddInParameter(cmd, "@VARCHAR_Comment", DbType.String, webLoan.DeclineComment);
                db.AddInParameter(cmd, "@VARCHAR_Reason", DbType.String, webLoan.DeclineReason);
                db.AddOutParameter(cmd, "@VARCHAR_Message", DbType.String, 4000);
                db.ExecuteNonQuery(cmd);

                message = db.GetParameterValue(cmd, "@VARCHAR_Message").ToString();
                if (string.IsNullOrEmpty(message))
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
                return message;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                cmd = null;
                db = null;
                if (connection != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                    connection = null;
                }
                connection = null;
            }
        }
        // END : VC | 2018.08.03 | YRS-AT-4018 -  Method used to decline a pending web loan 
        
        }

}
