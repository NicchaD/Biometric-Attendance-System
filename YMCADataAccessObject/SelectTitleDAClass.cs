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
	/// Summary description for SelectTitleDAClass.
	/// </summary>
	public class SelectTitleDAClass
	{
		public SelectTitleDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpTitles()
		{
			DataSet dsLookUpTitles = null;
			Database db = null;
			DbCommand commandLookUpTitles = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpTitles = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAOfficerTittles");
				if (commandLookUpTitles ==null) return null;
				dsLookUpTitles = new DataSet();
				dsLookUpTitles.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpTitles,dsLookUpTitles,"Title");
				return dsLookUpTitles;
			}
			catch
			{
				throw;
			}

		}


		public static DataSet SearchTitle(string parameterSearchShortDescription)
		{
			DataSet dsSearchTitle = null;
			Database db = null;
			DbCommand CommandSearchTitle = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchTitle = db.GetStoredProcCommand("yrs_usp_AMY_YMCASearchOfficerTitle");
				if (CommandSearchTitle ==null) return null;

				db.AddInParameter(CommandSearchTitle,"@varchar_ShortDescription",DbType.String,parameterSearchShortDescription);
				
				dsSearchTitle = new DataSet();
				dsSearchTitle.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchTitle,dsSearchTitle,"Title");

				return dsSearchTitle;
			}
			catch
			{
				throw;
			}

		}



	}
}
