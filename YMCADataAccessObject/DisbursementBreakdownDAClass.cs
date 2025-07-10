//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	DisbursementBreakdownDAClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	8/12/2005 10:52:34 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	DA Class for breakdown form which gets opened from VR manager on Save Click.
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by         Date             Description
//*******************************************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for DisbursementBreakdownDAClass.
	/// </summary>
	public sealed class DisbursementBreakdownDAClass
	{
		public DisbursementBreakdownDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
/// <summary>
/// To fetch the list of transactions
/// </summary>
/// <param name="parameterPersId"></param>
/// <param name="parameterFundEventId"></param>
/// <returns></returns>
		public static DataSet LookUpTransacts(string parameterPersId, string parameterFundEventId)
		{
			DataSet l_dataset_dsLookUpTransacts = null;
			Database db = null;
			DbCommand  LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand ("yrs_usp_GetRefundTransactions");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@PersID",DbType.String,parameterPersId);
				db.AddInParameter(LookUpCommandWrapper,"@FundEventID",DbType.String,parameterFundEventId);
				
				
			
				l_dataset_dsLookUpTransacts = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpTransacts,"Transacts");
				//System.AppDomain.CurrentDomain.SetData("dsTransacts", l_dataset_dsLookUpTransacts);
				return l_dataset_dsLookUpTransacts;
			}
			catch 
			{
				throw;
			}
		}
/// <summary>
/// to fetch the list of disbursement details for the selected transacrion
/// </summary>
/// <param name="parameterPersId"></param>
/// <param name="parameterDisbId"></param>
/// <param name="parameterFundEventId"></param>
/// <returns></returns>
		public static DataSet LookUpDisbursementDetails(string parameterPersId, string parameterDisbId,string parameterFundEventId,string parameterDate)
		{
			DataSet l_dataset_dsLookUpDisbursementDetails = null;
			Database db = null;
		    DbCommand  LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetDisbursementDetails");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@PersID",DbType.String,parameterPersId);
				db.AddInParameter(LookUpCommandWrapper,"@DisbID",DbType.String,parameterDisbId);
				db.AddInParameter(LookUpCommandWrapper,"@FundEventID",DbType.String,parameterFundEventId);
				db.AddInParameter(LookUpCommandWrapper,"@Tdate",DbType.String, parameterDate) ;
				
			
				l_dataset_dsLookUpDisbursementDetails = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpDisbursementDetails,"DisbDetails");
				//System.AppDomain.CurrentDomain.SetData("dsDetails", l_dataset_dsLookUpDisbursementDetails);
				return l_dataset_dsLookUpDisbursementDetails;
			}
			catch 
			{
				throw;
			}
		}
/// <summary>
/// to update disbursement detail in case of REPLACE
/// </summary>
/// <param name="parameterXml"></param>
/// <returns></returns>

		public static string UpdateDisbursement(string parameterXml)
		{
			String l_string_Output ;
			Database db = null;
		    DbCommand  UpdateCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDUpdateDisbursementDetails");
				if (UpdateCommandWrapper == null) return null;

				db.AddInParameter(UpdateCommandWrapper,"@sXML",DbType.String,parameterXml);
				db.AddOutParameter(UpdateCommandWrapper,"@nRet",DbType.Int32,9);
				db.ExecuteNonQuery(UpdateCommandWrapper);
				l_string_Output = Convert.ToString(db.GetParameterValue(UpdateCommandWrapper,"@nRet"));
				return l_string_Output;
				
			}
			catch
			{
				throw;
			}


		}
/// <summary>
/// to add transactions for reissue
/// </summary>
/// <param name="parameterXml"></param>
/// <param name="parameterDisbId"></param>
/// <param name="parameterPersId"></param>
/// <returns></returns>

		public static string AddTransactsForReissue(string parameterXml,string parameterDisbId,string parameterPersId)
		{
			String l_string_Output ;
			Database db = null;
			DbCommand  AddTransactsCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				AddTransactsCommandWrapper = db.GetStoredProcCommand ("yrs_usp_VDAddTransactsForReIssue");
				if (AddTransactsCommandWrapper == null) return null;

				db.AddInParameter(AddTransactsCommandWrapper,"@sXML",DbType.String,parameterXml);
				db.AddInParameter(AddTransactsCommandWrapper,"@DisbID",DbType.String,parameterDisbId);
				db.AddInParameter(AddTransactsCommandWrapper,"@PersID",DbType.String,parameterPersId);
			
				db.AddOutParameter(AddTransactsCommandWrapper,"@nRet",DbType.Int32,9);
				db.ExecuteNonQuery(AddTransactsCommandWrapper);
				l_string_Output = Convert.ToString(db.GetParameterValue(AddTransactsCommandWrapper,"@nRet"));
				return l_string_Output;
				
			}
			catch
			{
				throw;
			}


		}
/// <summary>
/// To change the disbursement status after reissue
/// </summary>
/// <param name="parameterDisbId"></param>
/// <param name="parameterActionType"></param>
/// <param name="parameterNotes"></param>
/// <returns></returns>
		public static string ChangeDisbursementStatus(string parameterDisbId,string parameterActionType,string parameterNotes)
		{
			String l_string_Output ;
			Database db = null;
			DbCommand  ChangeStatusCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				ChangeStatusCommandWrapper = db.GetStoredProcCommand ("yrs_usp_VDChangeDisbursementStatus");
				if (ChangeStatusCommandWrapper == null) return null;

				db.AddInParameter(ChangeStatusCommandWrapper,"@nDisbID",DbType.String,parameterDisbId);
				db.AddInParameter(ChangeStatusCommandWrapper,"@sActionType",DbType.String,parameterActionType);
				db.AddInParameter(ChangeStatusCommandWrapper,"@sNotes",DbType.String,parameterNotes);
			
				db.AddOutParameter(ChangeStatusCommandWrapper,"@nOutput",DbType.Int32,9);
				db.ExecuteNonQuery(ChangeStatusCommandWrapper);
				l_string_Output = Convert.ToString(db.GetParameterValue(ChangeStatusCommandWrapper,"@nOutput"));
				return l_string_Output;
				
			}
			catch
			{
				throw;
			}


		}
/// <summary>
/// 
/// </summary>
/// <param name="parameterXml"></param>
/// <param name="parameterDisbIds"></param>
/// <param name="parameterPersId"></param>
/// <param name="parameterStatus"></param>
/// <returns></returns>

		public static string AddTransactsForReversal(string parameterXml,string parameterDisbIds,string parameterPersId,string parameterStatus)
		{
			String l_string_Output ;
			Database db = null;
		    DbCommand  AddTransactsCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				AddTransactsCommandWrapper = db.GetStoredProcCommand ("yrs_usp_VDAddTransactsForReversal");
				if (AddTransactsCommandWrapper == null) return null;

				db.AddInParameter(AddTransactsCommandWrapper,"@sXML",DbType.String,parameterXml);
				db.AddInParameter(AddTransactsCommandWrapper,"@DisbIDs",DbType.String,parameterDisbIds);
				db.AddInParameter(AddTransactsCommandWrapper,"@PersID",DbType.String,parameterPersId);
				db.AddInParameter(AddTransactsCommandWrapper,"@sStatus",DbType.String,parameterStatus);
				db.AddOutParameter(AddTransactsCommandWrapper,"@nRet",DbType.Int32,9);
				db.ExecuteNonQuery(AddTransactsCommandWrapper);
				l_string_Output = Convert.ToString(db.GetParameterValue(AddTransactsCommandWrapper,"@nRet"));
				return l_string_Output;
				
			}
			catch
			{
				throw;
			}


		}
/// <summary>
/// 
/// </summary>
/// <param name="parameterDisbIds"></param>
/// <returns></returns>
		public static string UpdateDisbursementsForReversal(string parameterDisbIds)
		{
			String l_string_Output ;
			Database db = null;
		    DbCommand  ChangeStatusCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				ChangeStatusCommandWrapper = db.GetStoredProcCommand("ap_VDUpdateDisbursementsForReversal");
				if (ChangeStatusCommandWrapper == null) return null;

				db.AddInParameter(ChangeStatusCommandWrapper,"@DisbIDs",DbType.String,parameterDisbIds);
				
				db.AddOutParameter(ChangeStatusCommandWrapper,"@nRet",DbType.Int32,9);
				db.ExecuteNonQuery(ChangeStatusCommandWrapper);
				l_string_Output = Convert.ToString(db.GetParameterValue(ChangeStatusCommandWrapper,"@nRet"));
				return l_string_Output;
				
			}
			catch
			{
				throw;
			}


		}


		
	}
}
