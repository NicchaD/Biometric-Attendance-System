//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for SplitFundsforSettlementDAClass.
	/// </summary>
	public class SplitFundsforSettlementDAClass
	{
		public SplitFundsforSettlementDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		//PRIYA AS ON 04-08-08
		public static DataSet LookUp_DeceasedInformation(string paramPersID,DateTime paramDeathDate,string paramActualSplit,string FundEventID,ref string errorMsg )
		{
			//string l_string_output;
			DataSet l_dataset_BeneficiariesList = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			IDbTransaction tran = null;
			IDbConnection cn = null;
			string l_string_output;
			string l_string_message;
			try

			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				
				//Get a connection and Open it
				cn = db.CreateConnection();
				cn.Open();

				//Get a Transaction from the Database
				tran = cn.BeginTransaction(IsolationLevel.Serializable);
				if (tran == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeathSplitFunds_GetNonRetBeneficiaryOptions");
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper, "@DeceasedPersId",DbType.String,paramPersID);
				db.AddInParameter(LookUpCommandWrapper, "@DeceasedDeathDate",DbType.String,paramDeathDate);
				db.AddInParameter(LookUpCommandWrapper, "@DeceasedFundEventID",DbType.String,FundEventID);	
				db.AddInParameter(LookUpCommandWrapper, "@PerformActualSplit",DbType.String,paramActualSplit);
				db.AddOutParameter(LookUpCommandWrapper, "@cOutput",DbType.String, 500);
				db.AddOutParameter(LookUpCommandWrapper, "@int_ReturnStatus",DbType.Int32, 2);
				
				//db.ExecuteNonQuery(LookUpCommandWrapper);
				
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				
				l_dataset_BeneficiariesList = new DataSet();
				
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_BeneficiariesList,"r_BeneficiariesList");
				
			
				//l_string_output = LookUpCommandWrapper.GetParameterValue("@int_ReturnStatus").ToString();
                l_string_output = db.GetParameterValue(LookUpCommandWrapper, "@int_ReturnStatus").ToString();
            
				if(l_string_output == "-1")
				{
					l_string_message = db.GetParameterValue(LookUpCommandWrapper, "@cOutput").ToString();
					tran.Rollback();
					errorMsg = l_string_message;
					
				}
				else
				{
					errorMsg = "";
				}
				return l_dataset_BeneficiariesList;
//TODOMIGRATION - Connection is not being closed here. Need to revisit this
			}
			catch
			{
				tran.Rollback();
				throw;
			}
		}
		public static DataSet LookUp_MemberListForSplitFunds(string parameterSSN, string parameterLName ,string parameterFName, string parameterFundNo)
		{
			DataSet l_dataset_Beneficiaries_LookUp_MemberListDeceased = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_BS_LookUp_MemberListForSplitFunds");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_SSN",DbType.String,parameterSSN);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_LName",DbType.String,parameterLName);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_FName",DbType.String,parameterFName);				
				db.AddInParameter(LookUpCommandWrapper, "@varchar_FundNo", DbType.String, parameterFundNo);
			
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

				l_dataset_Beneficiaries_LookUp_MemberListDeceased = new DataSet();
				
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Beneficiaries_LookUp_MemberListDeceased,"r_MemberListForDeceased");
				return l_dataset_Beneficiaries_LookUp_MemberListDeceased;
			}
			catch 
			{
				throw;
			}
		}

		public static string LookUp_FundEventIdForSplitFunds(string paramPersId)
		{
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			string stringPersId;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeathSplitFunds_GetDeceased_FundEventID");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_Persid",DbType.String,paramPersId);
							
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

				
				stringPersId = db.ExecuteScalar(LookUpCommandWrapper).ToString();
				
				return stringPersId;
			}
			catch 
			{
				throw;
			}
		}
	
		//
		//END PRIYA
	}
}
