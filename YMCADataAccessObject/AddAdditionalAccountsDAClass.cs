//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	Ymca-YRS
// FileName			:	AddAdditionalAccountsDAClass.cs
// Author Name		:	Ruchi saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	10/13/2005 11:46:20 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified By			 Date				Description
//********************************************************************************************************************************
//Shagufta Chaudhari     2011.08.04         For BT-893, YRS 5.0-1360 : Allow Lump Sum additional accts record
//prasad jadhav			 2012.03.15			For (REOPEN)BT-990,YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
//Manthan Rajguru        2015.09.16         YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************************************************************
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AddAdditionalAccountsDAClass.
	/// </summary>
	public class AddAdditionalAccountsDAClass
	{
		public AddAdditionalAccountsDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
/// <summary>
/// 
/// </summary>
/// <param name="parameterPersId"></param>
/// <param name="parameterFundId"></param>
/// <returns></returns>
		public static DataSet GetYmcaList(string parameterPersId,string parameterFundId)
		{
			DataSet l_dataset_YmcaList = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_GetYMCAList");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId",DbType.String,parameterPersId);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_FundId",DbType.String,parameterFundId);
			
				l_dataset_YmcaList = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_YmcaList,"YmcaList");
				
				return l_dataset_YmcaList;
			}
			catch 
			{
				throw;
			}
		}


		public static string AddAdditionalAccount(string parameterEmpEventId,string parameterYmcaId,string parameterAcctType,string parameterBasisCode,string parameterAddlPctg,string parameterEffDate,string parameterAddlContrib,string parameterTermDate)
		{
			
			Database db = null;
			DbCommand AddCommandWrapper = null;
			String l_output;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_AddAdditionalAccount");
				
				if (AddCommandWrapper == null) return null;

				
				db.AddInParameter(AddCommandWrapper, "@varchar_EmpEventId",DbType.String,parameterEmpEventId);
				db.AddInParameter(AddCommandWrapper, "@varchar_YmcaId",DbType.String,parameterYmcaId);
				db.AddInParameter(AddCommandWrapper, "@varchar_AcctType",DbType.String,parameterAcctType);
				db.AddInParameter(AddCommandWrapper, "@varchar_BasisCode",DbType.String,parameterBasisCode);
				db.AddInParameter(AddCommandWrapper, "@numeric_AddlPctg",DbType.Double,parameterAddlPctg);
				db.AddInParameter(AddCommandWrapper, "@numeric_AddlContibution",DbType.Double,parameterAddlContrib);
				db.AddInParameter(AddCommandWrapper, "@datetime_EffectiveDate",DbType.String,parameterEffDate);
				db.AddInParameter(AddCommandWrapper, "@datetime_TerminationDate",DbType.String,parameterTermDate);
				db.AddOutParameter(AddCommandWrapper, "@int_output",DbType.Int32,1);
				db.ExecuteNonQuery(AddCommandWrapper);
				//l_output = Convert.ToString(AddCommandWrapper.GetParameterValue("@int_output"));
                l_output = db.GetParameterValue(AddCommandWrapper, "@int_output").ToString();
				return l_output;
			}
			catch 
			{
				throw;
			}
		}

		public static string UpdateAdditionalAccount(string parameterUniqueId,string parameterTermDate)
		{
			
			Database db = null;
			DbCommand UpdateCommandWrapper = null;
			String l_output;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateAdditionalAccount");
				
				if (UpdateCommandWrapper == null) return null;

				
				db.AddInParameter(UpdateCommandWrapper, "@varchar_UniqueId",DbType.String,parameterUniqueId);
				db.AddInParameter(UpdateCommandWrapper, "@datetime_TerminationDate",DbType.String,parameterTermDate);
				
				db.AddOutParameter(UpdateCommandWrapper, "@int_output",DbType.Int32,1);
				db.ExecuteNonQuery(UpdateCommandWrapper);
				//l_output = Convert.ToString(UpdateCommandWrapper.GetParameterValue("@int_output"));
                l_output = db.GetParameterValue(UpdateCommandWrapper, "@int_output").ToString();
				return l_output;
			}
			catch 
			{
				throw;
			}
		}


		public static DataSet GetAdditionalAccount(string parameterUniqueId)
		{
			DataSet l_dataset_AddAccount = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SelectAdditionalAccount");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_UniqueId",DbType.String,parameterUniqueId);
				
			
				l_dataset_AddAccount = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_AddAccount,"AddAccount");
				
				return l_dataset_AddAccount;
			}
			catch 
			{
				throw;
			}
		}

        public static string ValidateDateForTDRenewal(string lcPersID, string dtmEffDate)
		{
			DbCommand LookUpCommandWrapper = null;
			Database db = null;
			string l_output;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_ValidateDateForTDRenewal");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_lcPersID",DbType.String,lcPersID);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_dtmEffDate",DbType.Date ,DateTime.Parse(dtmEffDate));
				db.AddOutParameter(LookUpCommandWrapper, "@Varchar_OutMsg",DbType.String,100); 

				db.ExecuteNonQuery(LookUpCommandWrapper);

				//l_output = Convert.ToString(LookUpCommandWrapper.GetParameterValue("@Varchar_OutMsg"));
                l_output = db.GetParameterValue(LookUpCommandWrapper, "@Varchar_OutMsg").ToString();

				return l_output;
				
			}
			catch 
			{
				throw;
			}
		}


        //Shagufta Chaudhari:2011.08.04:BT-893 
        public static string IsValidPayrollDateOverlap(string parameterYmcaId, string payrolldate)
        {
            DbCommand LookUpCommandWrapper = null;
            Database db = null;
            string l_output;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_YMCA_IsValidPayrollDateOverlap");
                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@guiYmcaid", DbType.String, parameterYmcaId);
                db.AddInParameter(LookUpCommandWrapper, "@payrolldate", DbType.String, payrolldate);
                db.AddOutParameter(LookUpCommandWrapper, "@coutput", DbType.String, 1000);
                db.ExecuteNonQuery(LookUpCommandWrapper);
                l_output = db.GetParameterValue(LookUpCommandWrapper, "@coutput").ToString();
                return l_output;
            }
            catch
            {
                throw;
            }
        }
        //End: SC:2011.08.04:BT-893
		//Added by prasad:2012.03.15 For (REOPEN)BT-990,YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
		public static string  TdAccountDayLimit()
		{

			DbCommand LookUpCommandWrapper = null;
			Database db = null;
			string l_output;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Get_TDAcct_DaysLimit");
				if (LookUpCommandWrapper == null) return null;
				db.AddOutParameter(LookUpCommandWrapper, "@dayslimit", DbType.String, 1000);
				db.ExecuteNonQuery(LookUpCommandWrapper);
				l_output = db.GetParameterValue(LookUpCommandWrapper, "@dayslimit").ToString();
				return l_output;
	}
			catch
			{
				throw;
}
		}

	}
}
