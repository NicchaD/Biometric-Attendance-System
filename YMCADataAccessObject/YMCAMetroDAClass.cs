//****************************************************
//Modification History
//****************************************************
//Modified by           Date          Description
//****************************************************
//Manthan Rajguru       2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
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
	/// Summary description for YMCAMetroDAClass.
	/// </summary>
	public sealed class YMCAMetroDAClass
	{
		private YMCAMetroDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet SearchYMCAMetro(string parameterSearchYMCANo,string parameterSearchYMCAName,string parameterSearchYMCACity,string parameterSearchYMCAState)
		{
			DataSet dsSearchYMCAMetro = null;
			Database db = null;
			DbCommand CommandSearchYMCAMetro = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCAMetro = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAMetro");
				if (CommandSearchYMCAMetro ==null) return null;

				db.AddInParameter(CommandSearchYMCAMetro,"@char_YmcaNo",DbType.String,parameterSearchYMCANo);
				db.AddInParameter(CommandSearchYMCAMetro,"@varchar_YmcaName",DbType.String,parameterSearchYMCAName);
				db.AddInParameter(CommandSearchYMCAMetro,"@varchar_City",DbType.String,parameterSearchYMCACity);
				db.AddInParameter(CommandSearchYMCAMetro,"@char_StateType",DbType.String,parameterSearchYMCAState);
				dsSearchYMCAMetro = new DataSet();
				dsSearchYMCAMetro.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCAMetro,dsSearchYMCAMetro,"YMCA Metro");

				return dsSearchYMCAMetro;
			}
			catch
			{
				throw;
			}

		}
	}
}
