//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for VoluntaryRefundRequestDAClass.
	/// </summary>
	public class VoluntaryRefundRequestDAClass
	{
		public VoluntaryRefundRequestDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet GetRequestedAcctDetails(string paramRequestId)
		{
			Database db = null;
			DbCommand commandGetRequestedAcctDetails= null;
			DataSet dsGetAcctDetails =null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandGetRequestedAcctDetails = db.GetStoredProcCommand("yrs_usp_RR_GetRefundRequestDetail");
				if (commandGetRequestedAcctDetails==null) return null;
				db.AddInParameter(commandGetRequestedAcctDetails,"",DbType.String,paramRequestId);
				dsGetAcctDetails=new DataSet();
                db.LoadDataSet(commandGetRequestedAcctDetails,dsGetAcctDetails,"Table_RequestedAccts");
				return dsGetAcctDetails;

			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetCurrentAcctDetails(string paramFundEventId)
		{
			Database db = null;
			DbCommand commandGetCurrentAcctDetails = null;
			DataSet dsGetCurrentAcctDetails = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandGetCurrentAcctDetails= db.GetStoredProcCommand("yrs_usp_AT_TransSumForRefunds");
				if(commandGetCurrentAcctDetails==null) return null;
				db.AddInParameter(commandGetCurrentAcctDetails,"@FundEventidvchar",DbType.String,paramFundEventId);
				dsGetCurrentAcctDetails= new DataSet();
				db.LoadDataSet(commandGetCurrentAcctDetails,dsGetCurrentAcctDetails,"Table_CurrentAcctDetails");
				return dsGetCurrentAcctDetails;
			}
			catch
			{
				throw;
			}
		}

		public static DataSet getNonFundedAcctDetails(string paramFundEventId)
		{
			Database db = null;
			DbCommand commandGetNonFundedAcctDetails = null;
			DataSet dsGetNonFundedAcctDetails = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				commandGetNonFundedAcctDetails=db.GetStoredProcCommand("yrs_usp_AT_TransSumForRefunds_Unfuned");
				if (commandGetNonFundedAcctDetails==null) return null;
				db.AddInParameter(commandGetNonFundedAcctDetails,"@FundEventidvchar",DbType.String,paramFundEventId);
				dsGetNonFundedAcctDetails= new DataSet();
				db.LoadDataSet(commandGetNonFundedAcctDetails,dsGetNonFundedAcctDetails,"Table_NonFundedAcctDetails");
				return dsGetNonFundedAcctDetails;
			}
			catch
			{
				throw;
			}
		}
	}
}
