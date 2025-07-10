// Project Name		:	33156	
// FileName			:	RetirementEstimateUIProcess.cs
// Author Name		:	Sameer joshi	
// Employee ID		:	33156
// Email			:	sameer.joshi@3i-infotech.com
// Contact No		:	55928743
// Creation Time	:	6/1/2005 7:28:12 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//********************************************************************************************************************************
//Modified By        Date            Description
//********************************************************************************************************************************
//Sanjay Rawat       28-Jul-2009     To Add Fund Type in InsertBankingInfo()
//Dilip Yadav        2009.09.08      YRS 5.0.852
//Priya				 05-April-2010   YRS 5.0-1042:New "flag" value in Person/Retiree maintenance screen
//Imran              15-June-2010    Changes for Enhancements
//Sanjay R.		     01-Aug-2012      BT-753/YRS 5.0-1270 : purchase page
//Anudeep            13-JUN-2013      BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
//Anudeep            10-Jul-2013      BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
//Anudeep            14-Oct-2013      BT:2236-After modifying address save button gets disabled
//Manthan Rajguru    2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Bala               2016.01.19       YRS-At-2398: Customer Service requires a Special Handling alert for officers
//Manthan Rajguru    2017.12.04		  YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
//********************************************************************************************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
    /// <summary>
    /// Summary description for RetireesInformationDAClass.
    /// </summary>
    public class RetireesInformationDAClass
    {
        public RetireesInformationDAClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //	/// </summary>
        //	/// <param name="parameterPersId"></param>
        //	/// <returns></returns>
        public static DataSet LookUpAnnuityPaid(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpAnnuityPaid = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AnnuityPaid");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@PersGUID", DbType.String, parameterPersId);


                l_dataset_dsLookUpAnnuityPaid = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAnnuityPaid, "AnnuityPaid");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpAnnuityPaid;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        //	/// </summary>
        //	/// <param name="parameterPersId"></param>
        //	/// <returns></returns>
        public static DataSet LookUpDisbursement(string parameterDisbursementId)
        {
            DataSet l_dataset_Disbursement = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_DisbursementWithHolding");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@DisbursementID", DbType.String, parameterDisbursementId);


                l_dataset_Disbursement = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_Disbursement, "Disbursement");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_Disbursement;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        //	/// </summary>
        //	/// <param name="parameterPersId"></param>
        //	/// <returns></returns>
        public static DataSet LookUpAnnuities(string parameterFundEventId)
        {
            DataSet l_dataset_dsLookUpAnnuities = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");


                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Annuities");

                if (LookUpCommandWrapper == null) return null;

                //db.AddInParameter(LookUpCommandWrapper,"@PersGUID",DbType.String,parameterPersId);
                //by Apanra 04/09/2007 -Retrieve teh details using the FundeventId
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FundeventId", DbType.String, parameterFundEventId);

                l_dataset_dsLookUpAnnuities = new DataSet();
                l_TableNames = new string[] { "Annuities", "JSAnnuitiesDetails" };

                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAnnuities, l_TableNames);
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpAnnuities;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 

        //	/// </summary>
        //	/// <param name="parameterPersId"></param>
        //	/// <returns></returns>
        public static DataSet LookUpAnnuityInfo(string parameterAnnuityId)
        {
            DataSet l_dataset_dsLookUpAnnuities = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_AnnuityJointSurvivorsDtls");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@AnnuityGUID", DbType.String, parameterAnnuityId);


                l_dataset_dsLookUpAnnuities = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAnnuities, "AnnuityInfo");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpAnnuities;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// //	/// </summary>
        //	/// <param name="parameterPersId"></param>
        //	/// <returns></returns>
        public static DataSet LookUpBeneficiaries(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpBeneficiaries = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantRetiredBeneficiaries");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId", DbType.String, parameterPersId);


                l_dataset_dsLookUpBeneficiaries = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpBeneficiaries, "Beneficiaries");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpBeneficiaries;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 

        ///  </summary>
        //	<param name="parameterPersId"></param>
        //	 <returns></returns>
        public static DataSet LookUpBanks(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpBanks = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RetireesBanks");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@PersGUID", DbType.String, parameterPersId);


                l_dataset_dsLookUpBanks = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpBanks, "Banks");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpBanks;
            }
            catch
            {
                throw;
            }
        }
        //	<param name="parameterPersId"></param>
        //	 <returns></returns>
        /*  Commented By Ashutosh Patil as on 17-01-2007 as Current Status of Payment Method  will be done at front end
        public static DataSet LookUpRetireesStatus(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpBanks = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db= DatabaseFactory.CreateDatabase("YRS");
			
                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RetireesStatus");
				
                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper,"@PersGUID",DbType.String,parameterPersId);
				
			
                l_dataset_dsLookUpBanks = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpBanks,"Retirees");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpBanks;
            }
            catch 
            {
                throw;
            }
        }
        */



        /// <summary>
        /// 

        public static DataSet LookUpFedWithDrawals(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpFedWithDrawals = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_fedtaxwithholding");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@PersGUID", DbType.String, parameterPersId);


                l_dataset_dsLookUpFedWithDrawals = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpFedWithDrawals, "FedWithDrawals");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpFedWithDrawals;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet LookUpGenWithDrawals(string parameterPersId)
        {
            DataSet l_dataset_dsLookUpGenWithDrawals = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GeneralTaxwithholding");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@PersGUID", DbType.String, parameterPersId);


                l_dataset_dsLookUpGenWithDrawals = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpGenWithDrawals, "GeneralWithDrawals");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpGenWithDrawals;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet LookUpAnnuityDetails(string parameterPersId, string parameterAnnuityId)
        {
            DataSet l_dataset_dsLookUpAnnuityDetails = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AnnuitiesWithCurrentValues ");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@PersGUID", DbType.String, parameterPersId);
                db.AddInParameter(LookUpCommandWrapper, "@AnnuityGUID", DbType.String, parameterAnnuityId);

                l_dataset_dsLookUpAnnuityDetails = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAnnuityDetails, "AnnuityDetails");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpAnnuityDetails;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet LookUpAdjustments(string parameterAnnuityId)
        {
            DataSet l_dataset_dsLookUpAdjustments = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AnnuityAdjustments");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@AnnuityGUID", DbType.String, parameterAnnuityId);


                l_dataset_dsLookUpAdjustments = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAdjustments, "Adjustments");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpAdjustments;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet TaxEntityTypes()
        {
            DataSet l_dataset_dsTaxEntityTypes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_TaxEntityTypes");

                if (LookUpCommandWrapper == null) return null;



                l_dataset_dsTaxEntityTypes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsTaxEntityTypes, "TaxEntityTypes");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsTaxEntityTypes;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet MaritalTypes(int parameterIsFedTaxForMaritalStatus)
        {
            DataSet l_dataset_dsMaritalTypes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_MaritalStatusTypes");

                if (LookUpCommandWrapper == null) return null;

                //Shubhrata Mar2nd,2007 YREN-3112
                db.AddInParameter(LookUpCommandWrapper, "@bit_IsFedTaxForMaritalStatus", DbType.Int32, parameterIsFedTaxForMaritalStatus);
                //Shubhrata Mar2nd,2007 YREN-3112		

                l_dataset_dsMaritalTypes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsMaritalTypes, "MaritalTypes");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsMaritalTypes;
            }
            catch
            {
                throw;
            }
        }//

        // Start : added by Dilip yadav on 2009.09.08 for YRS 5.0-852
        public static DataSet GenderTypes()
        {
            DataSet l_dataset_dsGenderTypes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GenderTypes");

                if (LookUpCommandWrapper == null) return null;

                l_dataset_dsGenderTypes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsGenderTypes, "GenderTypes");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsGenderTypes;
            }
            catch
            {
                throw;
            }
        }



        // Start : added by Dinesh Kanojia on 2012.12.10 for YRS 5.0-1685
        public static DataSet POACategoryTypes()
        {
            DataSet l_dataset_dsPOACategoryTypes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_POACategoryType");

                if (LookUpCommandWrapper == null) return null;

                l_dataset_dsPOACategoryTypes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsPOACategoryTypes, "POACategoryType");
                return l_dataset_dsPOACategoryTypes;
            }
            catch
            {
                throw;
            }
        }


        // Start : added by Dinesh Kanojia on 2013.01.31 for YRS 5.0-1697
        public static DataSet BenificiaryDeleteReasons()
        {
            DataSet l_dataset_dsBenificiaryDeleteReasons = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BenficiaryDeleteReason");

                if (LookUpCommandWrapper == null) return null;

                l_dataset_dsBenificiaryDeleteReasons = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsBenificiaryDeleteReasons, "DeleteReason");
                return l_dataset_dsBenificiaryDeleteReasons;
            }
            catch
            {
                throw;
            }
        }


        // End : added by Dilip yadav on 2009.09.08 for YRS 5.0-852

        public static DataSet WithHoldingTypes()
        {
            DataSet l_dataset_dsWithHoldTypes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_WithHoldingTypes");

                if (LookUpCommandWrapper == null) return null;



                l_dataset_dsWithHoldTypes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsWithHoldTypes, "WithHoldTypes");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsWithHoldTypes;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet getGenCodes()
        {
            DataSet l_dataset_dsWithHoldCodes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GeneralWithholdingCodes");

                if (LookUpCommandWrapper == null) return null;



                l_dataset_dsWithHoldCodes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsWithHoldCodes, "WithHoldCodes");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsWithHoldCodes;
            }
            catch
            {
                throw;
            }
        }

        public static void InsertRetireesFedWithdrawals(DataSet dsFedWithDrawals)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

            try
            {
                //DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_FP_InsertFedWithdrawals");

                //IB: on 07/06/2010 require Guid datatype for persid
                //db.AddInParameter(insertCommandWrapper,"@Persid",DbType.String,"Persid",DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@Persid", DbType.Guid, "Persid", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@MaritalStatusCode", DbType.String, "Marital Status", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@Exemptions", DbType.Int32, "Exemptions", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@Amount", DbType.Double, "Add'l Amount", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@WithHoldingType", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@TaxEntityType", DbType.String, "Tax Entity", DataRowVersion.Current);




                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateFedtaxWithholding");

                db.AddInParameter(updateCommandWrapper, "@MaritalStatusCode", DbType.String, "Marital Status", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@Exemptions", DbType.Int32, "Exemptions", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@Amount", DbType.Decimal, "Add'l Amount", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@WithHoldingType", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@TaxEntityType", DbType.String, "Tax Entity", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@UniqueID", DbType.Guid, "FedWithdrawalID", DataRowVersion.Current);

                if (dsFedWithDrawals != null)
                {
                    db.UpdateDataSet(dsFedWithDrawals, "FedWithDrawals", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }

            }
            catch
            {
                throw;
            }
        }
        public static void InsertRetireesGenWithdrawals(DataSet dsGenWithDrawals)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

            try
            {
                //DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_FP_InsertGeneralWithDrawals");

                //db.AddInParameter(insertCommandWrapper,"@Persid",DbType.String,"PersID",DataRowVersion.Current); 
                db.AddInParameter(insertCommandWrapper, "@Persid", DbType.Guid, "PersID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@StartDate", DbType.String, "Start Date", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@Amount", DbType.Double, "Add'l Amount", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@EndDate", DbType.String, "End Date", DataRowVersion.Current);





                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_FP_UpdateGeneralWithDrawals");

                db.AddInParameter(updateCommandWrapper, "@Uniqueid", DbType.Guid, "GenWithdrawalID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@Persid", DbType.Guid, "PersID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@Amount", DbType.Double, "Add'l Amount", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@StartDate", DbType.String, "Start Date", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@EndDate", DbType.String, "End Date", DataRowVersion.Current);



                // UpdateDataSet method has 6 parameters (Dataset,Table Name,
                //insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
                //UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
                //so a reference of it is passed.

                if (dsGenWithDrawals != null)
                {
                    db.UpdateDataSet(dsGenWithDrawals, "GeneralWithDrawals", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }

            }
            catch
            {
                throw;
            }
        }
        public static DataSet getRelationShips()
        {
            DataSet l_dataset_dsRelationTypes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RelationshipTypes");

                if (LookUpCommandWrapper == null) return null;



                l_dataset_dsRelationTypes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsRelationTypes, "RelationshipTypes");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsRelationTypes;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet getBenefitGroups()
        {
            DataSet l_dataset_dsBenefitGroups = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BenefitGroupTypes");

                if (LookUpCommandWrapper == null) return null;



                l_dataset_dsBenefitGroups = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsBenefitGroups, "BenefitGroupTypes");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsBenefitGroups;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet getBenefitLevels()
        {
            DataSet l_dataset_dsBenefitLevels = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BeneficiaryLevel");

                if (LookUpCommandWrapper == null) return null;



                l_dataset_dsBenefitLevels = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsBenefitLevels, "BeneficiaryLevel");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsBenefitLevels;
            }
            catch
            {
                throw;
            }
        }

        //NP:PS:2007.06.20 - Adding parameter @ParticipantType to stored procedure yrs_usp_BenefitTypes
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

                db.AddInParameter(LookUpCommandWrapper, "@ParticipantType", DbType.String, "DR");

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

        public static void InsertBeneficiaries(DataSet dsBeneficiaries)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

            try
            {
                //DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_InsertBeneficiaries");

                db.AddInParameter(insertCommandWrapper, "@BeneficiaryName1", DbType.String, "Name", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@BeneficiaryName2", DbType.String, "Name2", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@RelationShipCode", DbType.String, "Rel", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@BeneficiaryGroupCode", DbType.String, "Groups", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@BeneficiaryTaxNumber", DbType.String, "TaxID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@BeneficiaryLevelCode", DbType.String, "Lvl", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@BenefitPCTG", DbType.Double, "Pct", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@BirthDate", DbType.String, "Birthdate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@Persid", DbType.String, "PersId", DataRowVersion.Current);




                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateBeneficiaries");

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

                //deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeleteBeneficiaries");
                deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addrs_DeleteBeneficiary");
                db.AddInParameter(deleteCommandWrapper, "@guiBeneficiaryId", DbType.String, "UniqueID", DataRowVersion.Original);
                //db.AddInParameter(deleteCommandWrapper, "@guiMappingId", DbType.String, "MappingID", DataRowVersion.Original);
                //db.AddInParameter(deleteCommandWrapper, "@UniqueID", DbType.String, "UniqueID", DataRowVersion.Original);


                // UpdateDataSet method has 6 parameters (Dataset,Table Name,
                //insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
                //UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
                //so a reference of it is passed.

                if (dsBeneficiaries != null)
                {
                    db.UpdateDataSet(dsBeneficiaries, "Beneficiaries", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }

            }
            catch
            {
                throw;
            }
        }
        public static void InsertRetireeNotes(DataSet parameterYMCAInsertNotes)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;




            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_InsertYMCANotes");
                // Defining The Insert Command Wrapper With parameters
                //db.AddInParameter(insertCommandWrapper,"@guid_guiUniqueID",DbType.Guid,"UniqueID",DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_guiUniqueID", DbType.Guid, "UniqueID", DataRowVersion.Current);
                //IB:Persid datatype is Guid in datatable
                //db.AddInParameter(insertCommandWrapper,"@varchar_PersonID",DbType.String, "PersonID",DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_PersonID", DbType.Guid, "PersonID", DataRowVersion.Current);

                db.AddInParameter(insertCommandWrapper, "@varchar_NoteTypeCode", DbType.String, 1);
                db.AddInParameter(insertCommandWrapper, "@text_Note", DbType.String, "Note", DataRowVersion.Current);
                //by Aparna -YREN-3115 08/03/2007
                db.AddInParameter(insertCommandWrapper, "@bit_Important", DbType.Boolean, "bitImportant", DataRowVersion.Current);

                //by Aparna -02/01/2007 -taken from database
                //db.AddInParameter(insertCommandWrapper,"@datetime_Date",DbType.DateTime,"Date",DataRowVersion.Current);

                //Vipul - to fix the Creator Bug 04-Feb-06
                //db.AddInParameter(insertCommandWrapper,"@varchar_Creator",DbType.String ,"Creator",DataRowVersion.Current);
                //Vipul - to fix the Creator Bug 04-Feb-06
                //by Aparna -02/01/2007
                //				deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_DeleteYMCAContact");
                //				// Defining The Delete Command Wrapper With parameters
                //				deleteCommandWrapper.AddInParameter("@uniqueIdentifier_guiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Original);
                //				deleteCommandWrapper.AddInParameter("@uniqueIdentifier_guiYmcaID",DbType.String,"guiYmcaID",DataRowVersion.Original);
                //	



                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYMCANotes");
                updateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                if (updateCommandWrapper != null)
                {

                    db.AddInParameter(updateCommandWrapper, "@varchar_guiUniqueID", DbType.Guid, "UniqueID", DataRowVersion.Current);
                    //db.AddInParameter(updateCommandWrapper,"@varchar_PersonID",DbType.String,"PersonID",DataRowVersion.Current);
                    db.AddInParameter(updateCommandWrapper, "@bit_Important", DbType.Boolean, "bitImportant", DataRowVersion.Current);



                    //by Aparna  YREN-3115 9/03/2007
                }

                if (deleteCommandWrapper != null)
                {
                    //nothing
                }

                if (parameterYMCAInsertNotes != null)
                {
                    db.UpdateDataSet(parameterYMCAInsertNotes, "Member Notes", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }

            }
            catch
            {
                throw;
            }
        }

        public static DataSet getPOADetails(string parameterPersId)
        {
            DataSet l_dataset_POADtls = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_POA_Dtls");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@PersID", DbType.String, parameterPersId);

                l_dataset_POADtls = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_POADtls, "POA");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_POADtls;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet getPOAAddrDtls(string parameterPersId)
        {
            DataSet l_dataset_POAAddrDtls = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_POAAddr_Dtls");

                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@PersID", DbType.String, parameterPersId);


                l_dataset_POAAddrDtls = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_POAAddrDtls, "POAAddr");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_POAAddrDtls;
            }
            catch
            {
                throw;
            }
        }
        public static string InsertPOADetails(string Persid, string EffectiveDate, string TerminationDate, string Name1, string Name2, string Comments, string AddrUniqueId, string POACategory, DbTransaction dbTransaction, Database db)
        {
            
            DbCommand insertCommandWrapper = null;
            string strPOAUniqueId;
            try
            {
            

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_FP_InsertPOA");
                // Defining The Insert Command Wrapper With parameters
                db.AddInParameter(insertCommandWrapper, "@Persid", DbType.String, Persid);
                if (TerminationDate == "")
                {
                    db.AddInParameter(insertCommandWrapper, "@TerminationDate", DbType.DateTime, System.DBNull.Value);
                }
                else
                {
                    db.AddInParameter(insertCommandWrapper, "@TerminationDate", DbType.DateTime, TerminationDate);
                }
                db.AddInParameter(insertCommandWrapper, "@EffectiveDate", DbType.DateTime, EffectiveDate);
                db.AddInParameter(insertCommandWrapper, "@Name1", DbType.String, Name1);
                db.AddInParameter(insertCommandWrapper, "@Name2", DbType.String, Name2);
                db.AddInParameter(insertCommandWrapper, "@Comments", DbType.String, Comments);
                //db.AddInParameter(insertCommandWrapper, "@Addr1", DbType.String, Addr1);
                //db.AddInParameter(insertCommandWrapper, "@Addr2", DbType.String, Addr2);
                //db.AddInParameter(insertCommandWrapper, "@Addr3", DbType.String, Addr3);
                //db.AddInParameter(insertCommandWrapper, "@City", DbType.String, City);
                //db.AddInParameter(insertCommandWrapper, "@State", DbType.String, State);
                //db.AddInParameter(insertCommandWrapper, "@Country", DbType.String, Country);
                //db.AddInParameter(insertCommandWrapper, "@Zip", DbType.String, Zip);
                db.AddInParameter(insertCommandWrapper, "@AddrUniqueId", DbType.String, AddrUniqueId);
                db.AddInParameter(insertCommandWrapper, "@PoaCategory", DbType.String, POACategory);
                db.AddOutParameter(insertCommandWrapper, "@newPOAId", DbType.String, 100);

                //				@Persid varchar(255),
                //				@EffectiveDate DateTime,
                //				@TerminationDate  DateTime,
                //				@Name1 varchar(100),
                //				@Name2  varchar(100),
                //				@Comments varchar(255),
                //				@Addr1 varchar(255),
                //				@Addr2 varchar(255),
                //				@Addr3 varchar(255),
                //				@City varchar(100),
                //				@State varchar(40),
                //				@State varchar(15),
                //				@Country varchar(50)



                db.ExecuteNonQuery(insertCommandWrapper,dbTransaction);
                strPOAUniqueId = db.GetParameterValue(insertCommandWrapper, "@newPOAId").ToString();
                return strPOAUniqueId;
            }
            catch
            {
                throw;
            }
        }
        //public static void UpdatePOADetails(string Persid, string EffectiveDate, string TerminationDate, string Name1, string Name2, string Comments, string Addr1, string Addr2, string Addr3, string City, string Country, string State, string Zip, string guiuniqueid, string PoaCategory)
        public static void UpdatePOADetails(string Persid, string EffectiveDate, string TerminationDate, string Name1, string Name2, string Comments, string AddrUniqueId, string guiuniqueid, string PoaCategory, DbTransaction dbTransaction, Database db)
        {
            
            DbCommand updateCommandWrapper = null;
            try
            {
                

                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdatePOA");
                // Defining The Insert Command Wrapper With parameters
                db.AddInParameter(updateCommandWrapper, "@Persid", DbType.String, Persid);
                db.AddInParameter(updateCommandWrapper, "@EffectiveDate", DbType.DateTime, EffectiveDate);

                if (TerminationDate == "")
                {
                    db.AddInParameter(updateCommandWrapper, "@TerminationDate", DbType.DateTime, System.DBNull.Value);
                }
                else
                {
                    db.AddInParameter(updateCommandWrapper, "@TerminationDate", DbType.DateTime, TerminationDate);
                }

                db.AddInParameter(updateCommandWrapper, "@Name1", DbType.String, Name1);
                db.AddInParameter(updateCommandWrapper, "@Name2", DbType.String, Name2);
                db.AddInParameter(updateCommandWrapper, "@Comments", DbType.String, Comments);
                //db.AddInParameter(updateCommandWrapper, "@Addr1", DbType.String, Addr1);
                //db.AddInParameter(updateCommandWrapper, "@Addr2", DbType.String, Addr2);
                //db.AddInParameter(updateCommandWrapper, "@Addr3", DbType.String, Addr3);
                //db.AddInParameter(updateCommandWrapper, "@City", DbType.String, City);
                //db.AddInParameter(updateCommandWrapper, "@State", DbType.String, State);
                //db.AddInParameter(updateCommandWrapper, "@Country", DbType.String, Country);
                //db.AddInParameter(updateCommandWrapper, "@Zip", DbType.String, Zip);
                db.AddInParameter(updateCommandWrapper, "@AddrUniqueId", DbType.String, AddrUniqueId);
                db.AddInParameter(updateCommandWrapper, "@GuiUniqueID", DbType.String, guiuniqueid);
                db.AddInParameter(updateCommandWrapper, "@PoaCategory", DbType.String, PoaCategory);

                db.ExecuteNonQuery(updateCommandWrapper,dbTransaction);

            }
            catch
            {
                throw;
            }
        }
        //by Aparna 04/09/2007
        public static void UpdateAnnuityJointSurvivors(DataSet parameterAnnuityJointSurvivors)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;
            //			IDbTransaction l_IDbTransaction = null;
            //			IDbConnection l_IDbConnection = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                //				l_IDbConnection =db.GetConnection();
                //				l_IDbConnection.Open();
                //				if (l_IDbConnection == null) return;
                //				l_IDbTransaction = l_IDbConnection.BeginTransaction();

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateAnnuityJointSurvivorsDtls");
                UpdateCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                if (UpdateCommandWrapper != null)
                {
                    db.AddInParameter(UpdateCommandWrapper, "@guiAnnuityJointSurvivorsID", DbType.Guid, "AnnuityJointSurvivorsID", DataRowVersion.Current);
                    db.AddInParameter(UpdateCommandWrapper, "@SSNO", DbType.String, "SSNO", DataRowVersion.Current);
                    db.AddInParameter(UpdateCommandWrapper, "@BirthtDate", DbType.DateTime, "DateOfBirth", DataRowVersion.Current);
                    db.AddInParameter(UpdateCommandWrapper, "@DeathDate", DbType.DateTime, "Date Deceased", DataRowVersion.Current);
                    db.AddInParameter(UpdateCommandWrapper, "@LastName", DbType.String, "Last", DataRowVersion.Current);
                    db.AddInParameter(UpdateCommandWrapper, "@MiddleName", DbType.String, "Middle", DataRowVersion.Current);
                    db.AddInParameter(UpdateCommandWrapper, "@FirstName", DbType.String, "First", DataRowVersion.Current);
                    db.AddInParameter(UpdateCommandWrapper, "@bitspouse", DbType.Boolean, "Spouse", DataRowVersion.Current);
                    db.UpdateDataSet(parameterAnnuityJointSurvivors, "JSAnnuitiesDetails", insertCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                    //l_IDbTransaction.Commit();		

                }
            }
            catch
            {
                throw;
            }
        }
        public static void UpdateAnnuityJointSurvivors(string AnnuityGUID, string SSNO, string BirthtDate, string DeathDate, string MiddleName, string FirstName, string bitspouse, string LastName)
        {
            Database db = null;
            DbCommand updateCommandWrapper = null;




            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateAnnuityJointSurvivorsDtls");
                // Defining The Insert Command Wrapper With parameters
                db.AddInParameter(updateCommandWrapper, "@AnnuityGUID", DbType.String, AnnuityGUID);
                db.AddInParameter(updateCommandWrapper, "@SSNO", DbType.String, SSNO);
                if (BirthtDate.Trim().Length != 0)
                {
                    db.AddInParameter(updateCommandWrapper, "@BirthtDate", DbType.DateTime, BirthtDate);
                }
                else
                {
                    db.AddInParameter(updateCommandWrapper, "@BirthtDate", DbType.DateTime, System.DBNull.Value);
                }
                if (DeathDate.Trim().Length != 0)
                {
                    db.AddInParameter(updateCommandWrapper, "@DeathDate", DbType.DateTime, DeathDate);

                }
                else
                {
                    db.AddInParameter(updateCommandWrapper, "@DeathDate", DbType.DateTime, System.DBNull.Value);
                }

                db.AddInParameter(updateCommandWrapper, "@LastName", DbType.String, LastName);
                db.AddInParameter(updateCommandWrapper, "@MiddleName", DbType.String, MiddleName);
                db.AddInParameter(updateCommandWrapper, "@FirstName", DbType.String, FirstName);
                if (bitspouse.Equals("Y"))
                {
                    db.AddInParameter(updateCommandWrapper, "@bitspouse", DbType.String, 1);
                }
                else
                {
                    db.AddInParameter(updateCommandWrapper, "@bitspouse", DbType.String, 0);
                }



                db.ExecuteNonQuery(updateCommandWrapper);


            }
            catch
            {
                throw;
            }
        }

        public static void InsertBankingInfo(DataSet dsBanking)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

            try
            {
                //DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_InsertRetireeBankInfo");
                db.AddInParameter(insertCommandWrapper, "@uniqueIdentifier_guiEntityID", DbType.String, "PersonID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_BankAcctNumber", DbType.String, "AccountNo", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_BankName", DbType.String, "BankName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@char_BankAbaNumber", DbType.String, "BankABA#", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_PaymentMethod", DbType.String, "PaymentDesc", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_AccountType", DbType.String, "EFTDesc", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@DateTime_EffDate", DbType.DateTime, "EffecDate", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_ParticipantType", DbType.String, "ParticipantType", DataRowVersion.Current);  //Added by sanjay for Fund Type




                //added by hafiz on 9Aug2006 - YREN-2591
                db.AddInParameter(insertCommandWrapper, "@guiBankId", DbType.String, "BankId", DataRowVersion.Current);

                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateRetireesBankInfo");
                db.AddInParameter(updateCommandWrapper, "@uniqueIdentifier_guiUniqueID", DbType.String, "UniqueID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@uniqueIdentifier_guiEntityID", DbType.String, "PersonID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_BankAcctNumber", DbType.String, "AccountNo", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_BankName", DbType.String, "BankName", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@char_BankAbaNumber", DbType.String, "BankABA#", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_PaymentMethod", DbType.String, "PaymentDesc", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_AccountType", DbType.String, "EFTDesc", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@DateTime_EffDate", DbType.DateTime, "EffecDate", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@uniqueIdentifier_guiBankID", DbType.String, "BankID", DataRowVersion.Current);

                // UpdateDataSet method has 6 parameters (Dataset,Table Name,
                //insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
                //UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
                //so a reference of it is passed.

                if (dsBanking != null)
                {
                    db.UpdateDataSet(dsBanking, "Banks", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }

            }
            catch
            {
                throw;
            }
        }
        public static DataSet PHRDetails()
        {
            DataSet l_dataset_dsPHRDetails = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_PHRDetails");

                if (LookUpCommandWrapper == null) return null;


                l_dataset_dsPHRDetails = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsPHRDetails, "PHRDetails");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsPHRDetails;
            }
            catch
            {
                throw;
            }
        }


        public static int LookUpOldGuardNews(string parameterPersId)
        {
            Database db = null;
            DbCommand CommandLookUpOldGuardNews = null;
            int int_output;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                CommandLookUpOldGuardNews = db.GetStoredProcCommand("yrs_usp_AMP_LookUpOldGuardNews");
                db.AddInParameter(CommandLookUpOldGuardNews, "@varchar_PersId", DbType.String, parameterPersId);
                db.AddOutParameter(CommandLookUpOldGuardNews, "@integer_Count", DbType.Int16, 6);
                db.ExecuteNonQuery(CommandLookUpOldGuardNews);
                int_output = Convert.ToInt16(db.GetParameterValue(CommandLookUpOldGuardNews, "@integer_Count"));
                return int_output;
            }
            catch
            {
                throw;
            }
        }
        //Priya 05-April-2010 : YRS 5.0-1042:New "flag" value in Person/Retiree maintenance screen
        //Commented OldGuardNews check box code to remove it from screen
        //		public static void InsertOldGuardNews(string parameterPersId, string parameterCreator)
        //		{
        //			Database db= null;
        //			DbCommand CommandInsertOldGuardNews = null;
        //			try
        //			{
        //				db= DatabaseFactory.CreateDatabase("YRS");
        //				CommandInsertOldGuardNews=db.GetStoredProcCommand("yrs_usp_AMP_InsertOldGuardNews");
        //				CommandInsertOldGuardNews.AddInParameter("@varchar_PersId",DbType.String,parameterPersId);
        //				CommandInsertOldGuardNews.AddInParameter("@varchar_Creator",DbType.String,parameterCreator);
        //				db.ExecuteNonQuery(CommandInsertOldGuardNews);
        //			}
        //			catch
        //			{
        //				throw;
        //			}
        //		}
        //
        //		public static void DeleteOldGuardNews(string parameterPersId)
        //		{
        //			Database db= null;
        //			DbCommand CommandDeleteOldGuardNews = null;
        //			try
        //			{
        //				db= DatabaseFactory.CreateDatabase("YRS");
        //				CommandDeleteOldGuardNews=db.GetStoredProcCommand("yrs_usp_AMP_DeleteOldGuardNews");
        //				CommandDeleteOldGuardNews.AddInParameter("@varchar_PersId",DbType.String,parameterPersId);
        //				db.ExecuteNonQuery(CommandDeleteOldGuardNews);
        //			}
        //			catch
        //			{
        //				throw;
        //			}
        //		}


        //Method to update Js Beneficiaries

        public static void UpdateJSBeneficiaries(string parameterAnnuityID, string parameterSSNo, string parameterBirthDate, string parameterDeathDate, string parameterLastName, string parameterFirstName, string parameterMiddleName, char parameterSpouse)
        {

            Database db = null;
            DbCommand UpdateCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return;

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateAnnuityJointSurvivorsDtls");
                db.AddInParameter(UpdateCommandWrapper, "@varchar_AnnuityID", DbType.String, parameterAnnuityID);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_SSNo", DbType.String, parameterSSNo);
                db.AddInParameter(UpdateCommandWrapper, "@datetime_BirthDate", DbType.String, parameterBirthDate);
                db.AddInParameter(UpdateCommandWrapper, "@datetime_DeathDate", DbType.String, parameterDeathDate);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_LastName", DbType.String, parameterLastName);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_FirstName", DbType.String, parameterFirstName);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_MiddleName", DbType.String, parameterMiddleName);
                db.AddInParameter(UpdateCommandWrapper, "@bit_Spouse", DbType.String, parameterSpouse);
                if (UpdateCommandWrapper == null) return;




                db.ExecuteNonQuery(UpdateCommandWrapper);


            }
            catch
            {
                throw;
            }
        }
        public static DataSet LookUpGeneralInfo(string parameterPersId, string parameterFundId)
        {
            DataSet l_dataset_dsLookUpgeneralInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMR_SearchRetireeGeneral");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_gui_UniqueId", DbType.String, parameterPersId);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_gui_FundId", DbType.String, parameterFundId);

                l_dataset_dsLookUpgeneralInfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpgeneralInfo, new string[] {"GeneralInfo", "SpecialHandlingDetails"});
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsLookUpgeneralInfo;
            }
            catch
            {
                throw;
            }
        }


        public static int IsBeneficiaryRequired(string parameterpersid)
        {
            Database db = null;
            DbCommand SelectCommandWrapper = null;
            int intReturnValue;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                SelectCommandWrapper = db.GetStoredProcCommand("yrs_usp_IsBeneficiaryRequired");
                db.AddInParameter(SelectCommandWrapper, "@guiPersid", DbType.String, parameterpersid);
                db.AddOutParameter(SelectCommandWrapper, "@intReturnValue", DbType.Int32, 32);
                db.ExecuteNonQuery(SelectCommandWrapper);
                intReturnValue = Convert.ToInt32(db.GetParameterValue(SelectCommandWrapper, "@intReturnValue"));
                return intReturnValue;



            }
            catch
            {
                throw;
            }
        }

        //SR:01-Aug-2012:BT-753/YRS 5.0-1270 : Use description field in place of short decription for general witholding codes

        public static DataSet getGenCodeWithDescription()
        {
            DataSet l_dataset_dsWithHoldCodes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GeneralWithholdingCodeWithDescription");

                if (LookUpCommandWrapper == null) return null;



                l_dataset_dsWithHoldCodes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsWithHoldCodes, "WithHoldCodes");
                //System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
                return l_dataset_dsWithHoldCodes;
            }
            catch
            {
                throw;
            }
        }

        //End :SR:01-Aug-2012:BT-753/YRS 5.0-1270 : Use description field in place of short decription for general witholding codes

        //Start: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form
        //Checking in database with respect to persid that person has surviour who is not primary deathbeneficiary
        public static bool GetJSBeneficiaries(string strPersId)
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            bool boolOutput;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return false;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetJSBeneficiaries");
                if (LookUpCommandWrapper == null) return false;
                db.AddInParameter(LookUpCommandWrapper, "@guiPersid", DbType.String, strPersId);
                db.AddOutParameter(LookUpCommandWrapper, "@boolOutput", DbType.Boolean, 8);
                db.ExecuteNonQuery(LookUpCommandWrapper);
                boolOutput = Convert.ToBoolean(db.GetParameterValue(LookUpCommandWrapper, "@boolOutput"));
                return boolOutput;
            }
            catch
            {
                throw;
            }
        }
        //End: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form

        //Anudeep:2013.06.13 BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
        public static string CheckBeneficiaryExistenceInOtherModules(string strSSNO, string strParticipantSSNo)
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string strOutput;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_CheckBeneficiaryExistenceInOtherModules");
                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@varchar_ssno", DbType.String, strSSNO);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_Participantssno", DbType.String, strParticipantSSNo);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_source", DbType.String, "DBEN");
                db.AddOutParameter(LookUpCommandWrapper, "@varchar_messageOutPut", DbType.String, 1500);
                db.ExecuteNonQuery(LookUpCommandWrapper);
                strOutput = db.GetParameterValue(LookUpCommandWrapper, "@varchar_messageOutPut").ToString();
                return strOutput;
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
    

        public static void InsertPOA(DataSet dsPoaDetails)
        {
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            string strNewAddressId = string.Empty;
            DataSet l_DataSet_UniqueID = new DataSet();
            DataRow drPoaRow;
            DataSet dsAddress = new DataSet();
            string strPOAUniqueId;
            try
            {
                l_DataSet_UniqueID = BeneficiarySettlementDAClass.LookUp_GetUniqueID();

                if (l_DataSet_UniqueID.Tables[0].Rows.Count > 0)
                {
                    strNewAddressId = l_DataSet_UniqueID.Tables[0].Rows[0][0].ToString();
                }

                if (dsPoaDetails != null )
                {
                    if (dsPoaDetails.Tables.Count > 0)
                    {
                        if (dsPoaDetails.Tables[0].Rows.Count > 0)
                        {
                            drPoaRow = dsPoaDetails.Tables[0].Rows[0];
                        }
                        else
                        {
                            return;
                        }
                        
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return;
                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction(IsolationLevel.ReadUncommitted);
                
                strPOAUniqueId = InsertPOADetails(drPoaRow["PersId"].ToString(), drPoaRow["EffectiveDate"].ToString(), drPoaRow["TerminationDate"].ToString(), drPoaRow["Name1"].ToString(), drPoaRow["Name2"].ToString(), drPoaRow["Comments"].ToString(), strNewAddressId, drPoaRow["POACategory"].ToString(),DBTransaction,db);

                dsPoaDetails.Tables[1].Rows[0]["UniqueId"] = strNewAddressId;
                dsPoaDetails.Tables[1].Rows[0]["guiEntityId"] = strPOAUniqueId;
                dsAddress.Tables.Add(dsPoaDetails.Tables[1].Copy());   
                
                AddressDAClass.SaveAddress(dsAddress,DBTransaction,db);
                
                DBTransaction.Commit();
            }
            catch
            {
                if (DBTransaction != null)
                    DBTransaction.Rollback();
                throw;
            }
            finally
            {
                if (DBconnectYRS != null)
                    DBconnectYRS.Close();

                DBTransaction = null;
                DBconnectYRS = null;
                db = null;
            }
        }

        public static void UpdatePOA(DataSet dsPoaDetails)
        {
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            string strNewAddressId = string.Empty;
            DataSet l_DataSet_UniqueID = new DataSet();
            DataRow drPoaRow;
            DataSet dsAddress = new DataSet();
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction(IsolationLevel.ReadUncommitted);

                drPoaRow = dsPoaDetails.Tables[0].Rows[0];
                dsAddress.Tables.Add(dsPoaDetails.Tables[1].Copy());
                
                strNewAddressId = AddressDAClass.SaveAddress(dsAddress, DBTransaction, db);
                //AA:14.10.2013:BT:2236-Added addrs UniqueId to populate the addrs uniqueid if addrs has not modified also
                if (strNewAddressId != "")
                {
                    drPoaRow["AddrsUniqueID"] = strNewAddressId;
                }
                UpdatePOADetails(drPoaRow["PersId"].ToString(), drPoaRow["EffectiveDate"].ToString(), drPoaRow["TerminationDate"].ToString(), drPoaRow["Name1"].ToString(), drPoaRow["Name2"].ToString(), drPoaRow["Comments"].ToString(), drPoaRow["AddrsUniqueID"].ToString(), drPoaRow["POAUniqueID"].ToString(), drPoaRow["POACategory"].ToString(), DBTransaction, db);
                
                DBTransaction.Commit();
            }
            catch
            {
                if (DBTransaction != null)
                    DBTransaction.Rollback();
                throw;
            }
            finally
            {
                if (DBconnectYRS != null)
                    DBconnectYRS.Close();

                DBTransaction = null;
                DBconnectYRS = null;
                db = null;
            }
        }

        //START: MMR | 2017.12.04 | YRS-AT-3756 | Added to get deceased beneficiary details
        public static DataSet GetDeceasedBeneficiary(string persId, string beneficiaryType)
        {
            DataSet deceasedBeneficiary = null;
            Database db = null;
            DbCommand cmd = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;
                cmd = db.GetStoredProcCommand("yrs_usp_RMD_GetDeceasedBeneficiary");
                if (cmd == null) return null;
                db.AddInParameter(cmd, "@VARCHAR_PersId", DbType.String, persId);
                db.AddInParameter(cmd, "@VARCHAR_beneficiaryType", DbType.String, beneficiaryType);
                db.ExecuteNonQuery(cmd);
                deceasedBeneficiary = new DataSet();
                db.LoadDataSet(cmd, deceasedBeneficiary, "DeceasedBeneficiary");
                return deceasedBeneficiary;
            }
            catch
            {
                throw;
            }
        }
        //END: MMR | 2017.12.04 | YRS-AT-3756 | Added to get deceased beneficiary details
    }

}

    /// 

