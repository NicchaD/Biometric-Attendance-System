//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for UpdateBankInformationDAClass.
	/// </summary>
	public sealed class UpdateBankInformationDAClass
	{
		private UpdateBankInformationDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpPaymentMethod()
		{
			DataSet dsLookUpPaymentMethod = null;
			Database db = null;
			DbCommand  commandLookUpPaymentMethod = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpPaymentMethod = db.GetStoredProcCommand("yrs_usp_AMY_SearchPaymentMethod");
				if (commandLookUpPaymentMethod ==null) return null;
				dsLookUpPaymentMethod = new DataSet();
				dsLookUpPaymentMethod.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpPaymentMethod,dsLookUpPaymentMethod,"Payment Method");
				return dsLookUpPaymentMethod;
			}
			catch
			{
				throw;
			}

		}

		public static DataSet GetEffDate(string parameterEntityId)
		{
			DataSet l_DataSet_RetBankInfo;
			DbCommand l_DBCommandWrapper;
			Database db = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				l_DBCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_RetBnkInfo_GetEffDate");
				l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(l_DBCommandWrapper,"@uniqueIdentifier_guiEntityID",DbType.String,parameterEntityId);
				if (l_DBCommandWrapper == null) return null; 
				l_DataSet_RetBankInfo = new DataSet();
				db.LoadDataSet(l_DBCommandWrapper,l_DataSet_RetBankInfo,"GetEffDate");
			    return l_DataSet_RetBankInfo;
			}
			
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public static DataSet LookUpAccountType()
		{
			DataSet dsLookUpAccountType = null;
			Database db = null;
			DbCommand commandLookUpAccountType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpAccountType = db.GetStoredProcCommand("yrs_usp_AMY_SearchAccountType");
				if (commandLookUpAccountType ==null) return null;
				dsLookUpAccountType = new DataSet();
				dsLookUpAccountType.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpAccountType,dsLookUpAccountType,"Account Type");
				return dsLookUpAccountType;
			}
			catch
			{
				throw;
			}

		}

	}
}
