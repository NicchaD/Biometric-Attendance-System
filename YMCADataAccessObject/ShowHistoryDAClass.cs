//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	ShowHistoryDAClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	8/2/2005 11:42:19 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	Data Access class to provide functions for Show History pop up of Void Disbursement VR Manager
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			    Date				Description
//*******************************************************************************
//Manthan Rajguru           2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for ShowHistoryDAClass.
	/// </summary>
	public sealed class ShowHistoryDAClass
	{
		public ShowHistoryDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

/// <summary>
/// Method to get the history of the disbursements based on the Disbursement Id
/// </summary>
/// <param name="parameterDisbId"></param>
/// <returns> Dataset dsDisbursementHistory</returns>
		public static DataSet GetDisbursementHistory(string parameterDisbId)
		{
			DataSet dsDisbursementHistory = null;
			Database db = null;
			DbCommand GetHistoryCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				GetHistoryCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetDisbursementHistory");
				
				if (GetHistoryCommandWrapper == null) return null;

				db.AddInParameter(GetHistoryCommandWrapper, "@varchar_DisbId",DbType.String,parameterDisbId);
				
				
			
				dsDisbursementHistory = new DataSet();
				db.LoadDataSet(GetHistoryCommandWrapper, dsDisbursementHistory,"DisbursementHistory");
				
				return dsDisbursementHistory;
			}
			catch 
			{
				throw;
			}
		}
	}
}
