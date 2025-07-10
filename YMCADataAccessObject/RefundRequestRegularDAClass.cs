//****************************************************
//Modification History
//****************************************************
//Modified by           Date            Description
//****************************************************
//Imran 	            29-Sep-2010     YRS 5.0-1181 :Add validation for Hardhship withdrawal - No YMCA contact
//Manthan Rajguru       2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************
using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for RefundRequestRegularDAClass.
	/// </summary>
	public class RefundRequestRegularDAClass
	{
		public RefundRequestRegularDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public static DataSet GetTermDate(string parameterFundId)
		{
			DataSet l_dataset_dsTermDate = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RR_FindTermDate");
				
				if (LookUpCommandWrapper == null) return null;

				
				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundId",DbType.String,parameterFundId);
			
				l_dataset_dsTermDate = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsTermDate,"TermDateInfo");
				//System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
				return l_dataset_dsTermDate;
			}
			catch 
			{
				throw;
			}
		}

		public static string GetMinDistPeriods(double parameterAge)
		{
			
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			String l_Period;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RR_FindMinDistPeriods");
				
				if (LookUpCommandWrapper == null) return null;

				
				db.AddInParameter(LookUpCommandWrapper,"@numeric_Age",DbType.Double,parameterAge);
			
			
				l_Period = db.ExecuteScalar(LookUpCommandWrapper).ToString();
				//System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
				return l_Period;
			}
			catch 
			{
				throw;
			}
		}


		public static DataSet GetDeductions()
		{
			DataSet l_dataset_Deductions = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RR_GetDeductions");
				
				if (LookUpCommandWrapper == null) return null;
				l_dataset_Deductions = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Deductions,"Deductions");
				//System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
				return l_dataset_Deductions;
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet GetAddress(string parameterPersId)
		{
			DataSet l_dataset_Address = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RR_GetParticipantAddress");
				
				if (LookUpCommandWrapper == null) return null;

				
				db.AddInParameter(LookUpCommandWrapper,"@varchar_gui_UniqueId",DbType.String,parameterPersId);
			
				l_dataset_Address = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Address,"Address");
				
				return l_dataset_Address;
			}
			catch 
			{
				throw;
			}
		}
		public static DataSet LookUpTransacts(string paramFundEventId)
		{
			DataSet l_DataSet_Transacts = null;
			Database db = null;
			DbCommand LookUpTransactsCommandWrapper = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				LookUpTransactsCommandWrapper=db.GetStoredProcCommand("yrs_usp_LookupvrTransacts");
				if(LookUpTransactsCommandWrapper==null) return null;
				db.AddInParameter(LookUpTransactsCommandWrapper,"@UniqueIdentifier_FundEventid",DbType.String,paramFundEventId);
				l_DataSet_Transacts= new DataSet();
				db.LoadDataSet(LookUpTransactsCommandWrapper,l_DataSet_Transacts,"Table_LookUpTransacts");
				return l_DataSet_Transacts;
			}
			catch
			{
				throw;
			}
		}
		public static string LookUpYMCAIds()
		{
			Database db = null;
			DbCommand LookUpYMCAIdsCommandWrapper = null;
			
			try
			{
				string str_ymcaid= null;
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				LookUpYMCAIdsCommandWrapper=db.GetStoredProcCommand("yrs_usp_LookupAtsYmcasGuid");
				if(LookUpYMCAIdsCommandWrapper==null) return null;
                db.AddOutParameter(LookUpYMCAIdsCommandWrapper, "@UniqueIdentifier_Guid", DbType.String, 100);
				 db.ExecuteNonQuery(LookUpYMCAIdsCommandWrapper);
                 str_ymcaid = db.GetParameterValue(LookUpYMCAIdsCommandWrapper, "@UniqueIdentifier_Guid").ToString();
				return str_ymcaid;
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpAnnuityBasisTypes()
		{
			DataSet l_DataSet_LookUpAnnuityBasisTypes = null;
			Database db = null;
			DbCommand LookUpAnnuityBasisTypesCommandWrapper = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				LookUpAnnuityBasisTypesCommandWrapper=db.GetStoredProcCommand("yrs_usp_LookupAnnuityBasisType");
				if(LookUpAnnuityBasisTypesCommandWrapper == null) return null;
				l_DataSet_LookUpAnnuityBasisTypes= new DataSet();
				db.LoadDataSet(LookUpAnnuityBasisTypesCommandWrapper,l_DataSet_LookUpAnnuityBasisTypes,"Table_AnnuityBasisTypes");
				return l_DataSet_LookUpAnnuityBasisTypes;

			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetRefundable(string paramFundEventId)
		{
			DataSet l_DataSet_Refunds = null;
			Database db = null;
			DbCommand LookUpTransactsCommandWrapper = null;
			
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;

				LookUpTransactsCommandWrapper=db.GetStoredProcCommand("yrs_usp_RR_Sel_TransSumForRefunds");
				
				if(LookUpTransactsCommandWrapper==null) return null;

                db.AddInParameter(LookUpTransactsCommandWrapper, "@FundEventidvchar", DbType.String, paramFundEventId);
				
				l_DataSet_Refunds = new DataSet("RefundableDataSet");
				
				db.LoadDataSet(LookUpTransactsCommandWrapper,l_DataSet_Refunds,"Refundable");
				
				return l_DataSet_Refunds;
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetPersBankingBeforeEffDate(string paramEffDate)
		{
			DataSet l_DataSet_GetPersBanking= null;
		
			Database db = null;
			DbCommand GetPersBankingCommandWrapper = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				GetPersBankingCommandWrapper=db.GetStoredProcCommand("ap_PersBankingBeforeEffDate");
				if(GetPersBankingCommandWrapper==null) return null;
				db.AddInParameter(GetPersBankingCommandWrapper,"@MaxEffDate",DbType.DateTime,paramEffDate);
					l_DataSet_GetPersBanking = new DataSet();
				//db.LoadDataSet(GetPersBankingCommandWrapper,l_DataSet_GetPersBanking,"PersBanking");
				db.LoadDataSet(GetPersBankingCommandWrapper, l_DataSet_GetPersBanking,"PersBanking");
				return l_DataSet_GetPersBanking;

			}
			catch
			{
				throw;
			}
		}
		//Priya 03-june-08
		public static int ValidateDailyInterest()//(string paramPersId ,string paramFundEventId)
		{					
			Database db = null;
			DbCommand ValidateDailyInterestWrapper = null;
			int returnValue ;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return 0;
				ValidateDailyInterestWrapper = db.GetStoredProcCommand("yrs_usp_DI_ValidateLastRunDate");
				if(ValidateDailyInterestWrapper == null) return 0;

				//ValidateDailyInterestWrapper.AddInParameter("@PersId",DbType.String,paramPersId);
				//ValidateDailyInterestWrapper.AddInParameter("@FundedEventId",DbType.String,paramFundEventId);
				db.AddOutParameter(ValidateDailyInterestWrapper,"@ReturnValue",DbType.Int32,4);
				db.ExecuteScalar(ValidateDailyInterestWrapper);
				returnValue =Convert.ToInt32(db.GetParameterValue(ValidateDailyInterestWrapper,"@ReturnValue"));
				
				//returnValue = Convert.ToInt16(db.ExecuteScalar(ValidateDailyInterestWrapper));
				
				return returnValue;
			}
			catch
			{
				throw;
			}
		}
		public static int ValidateLastPayrollDate(string paramPersId ,string paramFundEventId)
		{					
			Database db = null;
			DbCommand ValidateLastPayrollDateWrapper = null;
			int returnValue ;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return 0;
				ValidateLastPayrollDateWrapper = db.GetStoredProcCommand("yrs_usp_RR_ValidateLastPayrollDate");
				if(ValidateLastPayrollDateWrapper == null) return 0;

				db.AddInParameter(ValidateLastPayrollDateWrapper,"@PersId",DbType.String,paramPersId);
				db.AddInParameter(ValidateLastPayrollDateWrapper,"@FundedEventId",DbType.String,paramFundEventId);
				returnValue = Convert.ToInt16(db.ExecuteScalar(ValidateLastPayrollDateWrapper));
				
				return returnValue;
			}
			catch
			{
				throw;
			}
		}
		//end of priya 03-june-08

		//Priya as on 22-Jan-2009 : YRS 5.0-666  added to get 6 month for hardship withdrawal
		public static DataSet getConfigurationValue(string ParameterConfigKey)
		{
			try
			{
				return YMCACommonDAClass.getConfigurationValue(ParameterConfigKey);
			}
			catch
			{
				throw;
			}
		}
		//End 22-Jan-2009
        //IB: Added on 29/Sep/2010 YRS 5.0-1181 :Add validation for Hardhship withdrawal - No YMCA contact
        public static DataSet ValidateRefundProcess(string paramRequestId)
        {
            Database db = null;
            DbCommand ValidateRefundProcessWrapper = null;
            DataSet l_DataSet_YmcaContact=null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                ValidateRefundProcessWrapper = db.GetStoredProcCommand("Yrs_usp_RefundProcessValidation");
                if (ValidateRefundProcessWrapper == null) return null;
                db.AddInParameter(ValidateRefundProcessWrapper, "@RequestId", DbType.String, paramRequestId);
                l_DataSet_YmcaContact = new DataSet();
                db.LoadDataSet(ValidateRefundProcessWrapper, l_DataSet_YmcaContact, "YmcaContact");
                return l_DataSet_YmcaContact;
            }
            catch
            {
                throw;
            }
        }
	}
}
