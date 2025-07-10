//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

//using System;
//using System.Data;
//using System.Globalization;
//using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
//using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Common;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for SelectBankDAClass.
	/// </summary>
	public sealed class SelectBankDAClass
	{
		public SelectBankDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet SearchYMCASelectBank(string parameterSearchBankName,string parameterSearchBankABANumber)
		{
			DataSet dsSearchYMCASelectBank = null;
			Database db = null;
			DbCommand CommandSearchYMCASelectBank= null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCASelectBank = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCASelectBank");
				if (CommandSearchYMCASelectBank ==null) return null;

				db.AddInParameter(CommandSearchYMCASelectBank,"@varchar_BankName",DbType.String,parameterSearchBankName);
				db.AddInParameter(CommandSearchYMCASelectBank,"@char_BankAbaNumber",DbType.String,parameterSearchBankABANumber);
				
				dsSearchYMCASelectBank = new DataSet();
				dsSearchYMCASelectBank.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCASelectBank,dsSearchYMCASelectBank,"YMCA Select Bank");

				return dsSearchYMCASelectBank;
			}
			catch
			{
				throw;
			}

		}
	}
}
