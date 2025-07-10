//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	DisbursementReversalDAClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	8/26/2005 12:25:01 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by          Date          Description
//*******************************************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for DisbursementReversalDAClass.
	/// </summary>
	public sealed class DisbursementReversalDAClass
	{
		public DisbursementReversalDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
/// <summary>
/// 
/// </summary>
/// <returns></returns>
		public static DataSet GetStatusList()
		{
			DataSet l_dataset_dsStatusList = null;
			Database db = null;
			DbCommand  LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDReverseGetFundEventList");
				
				if (LookUpCommandWrapper == null) return null;

				l_dataset_dsStatusList = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsStatusList,"StatusList");
				System.AppDomain.CurrentDomain.SetData("dsStatusList", l_dataset_dsStatusList);
				return l_dataset_dsStatusList;
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
/// <param name="parameterAnnuityOnly"></param>
/// <returns></returns>
		public static DataSet GetDisbursementsByPersId(string parameterPersId,int parameterAnnuityOnly)
		{
			DataSet l_dataset_dsGetDisbursementsByPersId = null;
			Database db = null;
			DbCommand  LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand ("yrs_usp_VDGetDisbursementsByPersID");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_PersId",DbType.String,parameterPersId);
				db.AddInParameter(LookUpCommandWrapper,"@integer_AnnuityOnly",DbType.Int32,parameterAnnuityOnly);
				
			
				l_dataset_dsGetDisbursementsByPersId = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsGetDisbursementsByPersId,"DisbursementsByPersId");
				System.AppDomain.CurrentDomain.SetData("dsDisbursementsByPersId", l_dataset_dsGetDisbursementsByPersId);
				return l_dataset_dsGetDisbursementsByPersId;
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

		public static DataSet GetStatusByPersId(string parameterPersId)
		{
			DataSet l_dataset_dsStatus = null;
			Database db = null;
			DbCommand  LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand ("yrs_usp_VDReverseSetComboStatus");
				
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@tcPersID",DbType.String,parameterPersId);
				l_dataset_dsStatus = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsStatus,"Status");
				System.AppDomain.CurrentDomain.SetData("Status", l_dataset_dsStatus);
				return l_dataset_dsStatus;
			}
			catch 
			{
				throw;
			}

			
		}

/// <summary>
/// 
/// </summary>
/// <param name="parameterRevDisb"></param>
/// <returns></returns>

		public static DataSet GetWithholdingInfoForRev(string parameterRevDisb)
		{
			DataSet l_dataset_dsWithholdingForRev = null;
			Database db = null;
			DbCommand  LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand ("ap_VDGetWithHoldingInfoForReversal");
				
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@DisbID",DbType.String,parameterRevDisb);
				l_dataset_dsWithholdingForRev = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsWithholdingForRev,"WithholdingInfoForRev");
				System.AppDomain.CurrentDomain.SetData("WithholdingInfoForRev", l_dataset_dsWithholdingForRev);
				return l_dataset_dsWithholdingForRev;
			}
			catch 
			{
				throw;
			}

			
		}

		
	}
}
