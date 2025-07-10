//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YRS-YMCA
// FileName			:	RefundBeneficiaryDAClass.cs
// Author Name		:	Vipul Patel
// Employee ID		:	32900
// Email			:	vipul.patel@3i-infotech.com
// Contact No		:	55928738
// Creation Time	:	10/19/2005 
// Program Specification Name	:	
// Unit Test Plan Name			:	
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by           Date         Description
//*******************************************************************************
// Sanjay R.            2014.05.07   YRS 5.0-2188 : RMDs for Beneficiaries 
// Manthan Rajguru      2015.09.16   YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Sanjay GS Rawat       2016.12.07 	 YRS-AT-3222 - YRS enh-allow regenerate RMD for deceased participants Phase 2 of 2 (TrackIT 27024) 
//Sanjay GS Rawat       2017.12.04   YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
//*******************************************************************************

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for RefundBeneficiaryDAClass.
	/// </summary>
	public class RefundBeneficiaryDAClass
	{
		public RefundBeneficiaryDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet Get_BeneficiaryBenefitDetails4Refund (string paramBeneficiaryBenefitOptionID)
		{
			Database db = null;
			DbCommand commandWrapperGetRefundDetails = null;
			DataSet dataset_BeneficiaryBenefitRefundDetails = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null)return null;
				commandWrapperGetRefundDetails = db.GetStoredProcCommand("dbo.yrs_usp_BS_Get_BeneficiaryBenefitDetails4Refund");

				if(commandWrapperGetRefundDetails==null) return null;
				dataset_BeneficiaryBenefitRefundDetails = new DataSet();
				db.AddInParameter(commandWrapperGetRefundDetails,"@varchar_BeneficiaryBenefitOptionID",DbType.String,paramBeneficiaryBenefitOptionID);
				
				/* This will return 3 tables 
				1. DeathBenefitOption
				2. BeneficiaryNameSSNDetails
				3. BeneficiaryRelationshipCode	*/
				string [] l_TableNames;
				l_TableNames = new string [] {"DeathBenefitOption", "BeneficiaryNameSSNDetails" ,"BeneficiaryRelationshipCode"};
				db.LoadDataSet(commandWrapperGetRefundDetails, dataset_BeneficiaryBenefitRefundDetails,l_TableNames );
	
				return dataset_BeneficiaryBenefitRefundDetails; 
				
			}
			catch
			{
				throw;
			}
		}


		public static int  Get_RefundRolloverInstitutionID(string paramRolloverInstitutionName, out string outpara_RolloverInstitutionUniqueID )
		{
			/* Call  Procedure dbo.yrs_usp_BS_Get_RefundRolloverInstitutionID with Institution Name
				/* If it exists use it, else  INSERT new record & use that Unique ID  */
				
			int int_returnStatus ; 
			int_returnStatus = 1 ;

			Database db = null;
			DbCommand CommandWrapperGetRolloverInstitutionID = null;
			outpara_RolloverInstitutionUniqueID = "";	
			try 
			{ 
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return -1;

				CommandWrapperGetRolloverInstitutionID = db.GetStoredProcCommand("yrs_usp_BS_Get_RefundRolloverInstitutionID") ;
				
				if (CommandWrapperGetRolloverInstitutionID == null) return -1;

				db.AddInParameter(CommandWrapperGetRolloverInstitutionID,"@varchar_RolloverInstitutionName",DbType.String,paramRolloverInstitutionName );
				db.AddOutParameter(CommandWrapperGetRolloverInstitutionID,"@varchar_RolloverInstitutionUniqueID",DbType.String ,1000);
			
				db.ExecuteNonQuery(CommandWrapperGetRolloverInstitutionID) ;
				outpara_RolloverInstitutionUniqueID = Convert.ToString( db.GetParameterValue(CommandWrapperGetRolloverInstitutionID,"@varchar_RolloverInstitutionUniqueID") );

				return int_returnStatus ;


			}

			catch
			{
				throw;
			}
		}

        #region InsertBeneficiaryRMDs

        //Start - SR:2014.05.07:YRS 5.0-2188: RMDs for Beneficiaries 
        public static DataSet GetBeneficiaryRMDs(string paramDeathBenefitOptionID, decimal paramTaxableAmount, decimal paramNonTaxableAmount, string paramRMDEligibledt)
        {
            DataSet dsBeneficiaryRMDs = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Dth_Settle_Get_BeneRMDRecords");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@guiBenefitOptionID", DbType.String, paramDeathBenefitOptionID);
                db.AddInParameter(LookUpCommandWrapper, "@numTaxableAmt", DbType.Decimal, paramTaxableAmount);
                db.AddInParameter(LookUpCommandWrapper, "@numNonTaxableAmt", DbType.Decimal, paramNonTaxableAmount);
                db.AddInParameter(LookUpCommandWrapper, "@dtmRMDEligibleDate", DbType.Date, paramRMDEligibledt);             

                dsBeneficiaryRMDs = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsBeneficiaryRMDs, "BeneficiaryRMDs");
                return dsBeneficiaryRMDs;
            }
            catch
            {
                throw;
            }
        }

        //public static string GetPersonRMDEligibledate(string paramPersID)
        //{
        //    string strRMDEligibledate = string.Empty ;
        //    Database db = null;
        //    DbCommand LookUpCommandWrapper = null;

        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");

        //        if (db == null) return null;

        //        LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Dth_Settle_Get_PersRMDEligibledate");

        //        if (LookUpCommandWrapper == null) return null;

        //        db.AddInParameter(LookUpCommandWrapper, "@guiPersID", DbType.String, paramPersID);
        //        db.AddOutParameter(LookUpCommandWrapper, "@strRMDEligibledate", DbType.String, 50);

        //        db.ExecuteNonQuery(LookUpCommandWrapper);
        //        strRMDEligibledate = Convert.ToString(db.GetParameterValue(LookUpCommandWrapper, "@strRMDEligibledate"));
        //        return strRMDEligibledate;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}     

        public static DataSet GetPersonRMDEligibledate(string paramPersID)
        {
            DataSet dsDecsdDetail = null;
            //string strRMDEligibledate = string.Empty ;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Dth_Settle_Get_PersRMDEligibledate");
                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@guiPersID", DbType.String, paramPersID);
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsDecsdDetail = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsDecsdDetail, "DecsdDetail");
                return dsDecsdDetail;
            }
            catch
            {
                throw;
            }
        }
      
       
        //		'***************************************************************************************************//
        //		'Class Name                : RefundBeneficiaryDAClass                    Used In     : YMCAUI       //
        //		'Created By                : Sanjay Singh                                Modified On :				//
        //		'Modified By               :                                                                        //
        //		'Modify Reason             :                                                                        //
        //		'Constructor Description   :                                                                        //
        //		'Event Description         :This event will insert the Beneficiary RMD details in the atsBeneficiaryRMDs table   //
        //		'***************************************************************************************************//
        public static void SaveBeneficiaryRMDs(string paramDecsdPersId,string DeathBenefitOptionID, DataSet dsRMDs)
        {
            DbCommand insertCommandWrapper = null;
            Database db = null;
            DbConnection dbConnect = null;
            DbTransaction dbTransact = null;


            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                dbConnect = db.CreateConnection();
                dbConnect.Open();
                dbTransact = dbConnect.BeginTransaction(IsolationLevel.ReadUncommitted);  

                foreach (DataRow drRMDs in dsRMDs.Tables[1].Rows)
                {

                    insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_Dth_Settle_Insert_BeneficiaryRMDs");
                    db.AddInParameter(insertCommandWrapper, "@guiBenefitOptionID", DbType.String, DeathBenefitOptionID);
                    db.AddInParameter(insertCommandWrapper, "@guiDecsdPersID", DbType.String, paramDecsdPersId);
                    db.AddInParameter(insertCommandWrapper, "@guiBenePersID", DbType.String, drRMDs["guiBenePersID"]);
                    db.AddInParameter(insertCommandWrapper, "@intYear", DbType.Int16, drRMDs["intYear"]);
                    db.AddInParameter(insertCommandWrapper, "@numFactor", DbType.Decimal, drRMDs["ExpectancyFactor"]);
                    db.AddInParameter(insertCommandWrapper, "@numTaxableAmount", DbType.Decimal, drRMDs["TaxableAmount"]);
                    db.AddInParameter(insertCommandWrapper, "@numNonTaxableAmount", DbType.Decimal, drRMDs["NonTaxableAmount"]);
                    db.AddInParameter(insertCommandWrapper, "@numRMDTaxableAmount", DbType.Decimal, drRMDs["RMDTaxableAmount"]);
                    db.AddInParameter(insertCommandWrapper, "@numRMDNonTaxableAmount", DbType.Decimal, drRMDs["RMDNonTaxableAmount"]);
                    db.ExecuteNonQuery(insertCommandWrapper);
                }

                dbTransact.Commit();
            }
            catch
            {
                if (dbTransact != null)
                    dbTransact.Rollback();
                throw;
            }
            finally
            {
                if (dbConnect != null)
                    dbConnect.Close();
                dbTransact = null;
                dbConnect = null;
                db = null;
            }
        }
        #endregion
        //End - SR:2014.05.07:YRS 5.0-2188: RMDs for Beneficiaries 

        //START | SR | 2016.12.07 | YRS-AT-3222 | Calculate RMD for beneficiary of QD participant
        public static DataSet GetBeneficiaryRMDsForAltPayee(string deathBenefitOptionID, decimal taxableAmount, decimal nonTaxableAmount)
        {
            DataSet beneficiaryRMDs = null;
            Database db = null;
            DbCommand lookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                lookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Dth_Settle_QDRO_Get_BeneRMDRecords");

                if (lookUpCommandWrapper == null) return null;

                db.AddInParameter(lookUpCommandWrapper, "@UNIQUEIDENTIFIER_BenefitOptionID", DbType.String, deathBenefitOptionID);
                db.AddInParameter(lookUpCommandWrapper, "@NUMERIC_TaxableAmount", DbType.Decimal, taxableAmount);
                db.AddInParameter(lookUpCommandWrapper, "@NUMERIC_NonTaxableAmount", DbType.Decimal, nonTaxableAmount);              

                beneficiaryRMDs = new DataSet();
                db.LoadDataSet(lookUpCommandWrapper, beneficiaryRMDs, "BeneficiaryRMDs");
                return beneficiaryRMDs;
            }
            catch
            {
                throw;
            }
        }
        //END | SR | 2016.12.07 | YRS-AT-3222 | Calculate RMD for beneficiary of QD participant

        //START | SR | 2017.12.04 | YRS-AT-3756 | Validate participant is RMD eligible or not
        public static DataSet ValidateParticipantRMDEligibility(string deathBenefitOptionID)
        {
            DataSet participantRMDEligibility = null;
            Database db = null;
            DbCommand lookUpCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                lookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Dth_Settle_ValidateParticipantRMDEligibility");

                if (lookUpCommandWrapper == null) return null;

                db.AddInParameter(lookUpCommandWrapper, "@UNIQUEIDENTIFIER_BenefitOptionID", DbType.String, deathBenefitOptionID);
                
                participantRMDEligibility = new DataSet();
                db.LoadDataSet(lookUpCommandWrapper, participantRMDEligibility, "ParticipantRMDEligibility");
                return participantRMDEligibility;
            }
            catch
            {
                throw;
            }
        }
        //END | SR | 2017.12.04 | YRS-AT-3756 | Validate participant is RMD eligible or not

	}
}
