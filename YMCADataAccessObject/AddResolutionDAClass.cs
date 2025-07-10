//*******************************************************************************
//Modification History
//*******************************************************************************
//	Date		        Author			    Description
//*******************************************************************************
//22-May-2012			Priya			    BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
//2015.09.16            Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

//using System;
//using System.Data;
//using System.Data.SqlClient;
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
	/// Summary description for AddResolutionDAClass.
	/// </summary>
	public sealed class AddResolutionDAClass
	{
		private AddResolutionDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpOptionType()
		{
			try
			{
				//Commentd by Aparna 14/03/2007 
//				DataSet datasetLookUpOptionType = new DataSet();
//				DataTable datatableLookUpOptiontype = new DataTable("Option Type");
////				DataTable datatableLookUpOptiontype = datasetLookUpOptionType.Tables.Add("OptionType");
//				DataRow datarowLookUpOptionType;
//				datatableLookUpOptiontype.Columns.Add("Participant %",typeof(Double));
//				datatableLookUpOptiontype.Columns.Add("YMCA %",typeof(Double));
////				datatableLookUpOptiontype.Columns.Add("Selection",typeof(Boolean));
//
//				datarowLookUpOptionType=datatableLookUpOptiontype.NewRow();			
//				datarowLookUpOptionType["Participant %"]=3.00;
//				datarowLookUpOptionType["YMCA %"]=4.20;				
////			    datarowLookUpOptionType["Selection"]=false;
//				datatableLookUpOptiontype.Rows.Add(datarowLookUpOptionType);
//
//
//				datarowLookUpOptionType=datatableLookUpOptiontype.NewRow();			
//				datarowLookUpOptionType["Participant %"]=4.00;
//				datarowLookUpOptionType["YMCA %"]=5.60;
////				datarowLookUpOptionType["Selection"]=false;
//				datatableLookUpOptiontype.Rows.Add(datarowLookUpOptionType);
//
//				datarowLookUpOptionType=datatableLookUpOptiontype.NewRow();			
//				datarowLookUpOptionType["Participant %"]=5.00;
//				datarowLookUpOptionType["YMCA %"]=7.00;
////				datarowLookUpOptionType["Selection"]=false;
//				datatableLookUpOptiontype.Rows.Add(datarowLookUpOptionType);
//				datasetLookUpOptionType.Tables.Add(datatableLookUpOptiontype);
//				return datasetLookUpOptionType;
//Commentd by Aparna 14/03/2007 
				DataSet datasetLookUpOptionType = null;
				Database db = null;
				DbCommand commandLookUpOptionType = null;
				try
				{
					db = DatabaseFactory.CreateDatabase("YRS");
					if (db==null) return null;
					commandLookUpOptionType = db.GetStoredProcCommand("yrs_usp_GetResolutionRates");
					if (commandLookUpOptionType ==null) return null;
					datasetLookUpOptionType = new DataSet();
					datasetLookUpOptionType.Locale = CultureInfo.InvariantCulture;
					db.LoadDataSet(commandLookUpOptionType,datasetLookUpOptionType,"Option Type");
					return datasetLookUpOptionType;
				}
				catch
				{
					throw;
				}


			}
			catch
			{	
				throw;
			}
		}


		public static DataSet LookUpVestingType()
		{
			DataSet dsLookUpVestingType = null;
			Database db = null;
			DbCommand commandLookUpVestingType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpVestingType = db.GetStoredProcCommand("yrs_usp_AMY_SearchVestingType");
				if (commandLookUpVestingType ==null) return null;
				dsLookUpVestingType = new DataSet();
				dsLookUpVestingType.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpVestingType,dsLookUpVestingType,"Vesting Type");
				return dsLookUpVestingType;
			}
			catch
			{
				throw;
			}

		}


		public static DataSet LookUpResolutionType()
		{
			DataSet dsLookUpResolutionType = null;
			Database db = null;
			DbCommand commandLookUpResolutionType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpResolutionType = db.GetStoredProcCommand("yrs_usp_AMY_SearchResolutionType");
				if (commandLookUpResolutionType ==null) return null;
				dsLookUpResolutionType = new DataSet();
				dsLookUpResolutionType.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpResolutionType,dsLookUpResolutionType,"Resolution Type");
				return dsLookUpResolutionType;
			}
			catch
			{
				throw;
			}

		}

		//Priya 08-June-2009 : YRS 5.0-779  Warning msg if new resolution effective date too far in future
		public static DataSet getConfigurationValue(string ParameterConfigKey)
		{
			try
			{
				return YMCACommonDAClass.getConfigurationValue(ParameterConfigKey);
			}
			catch
			{
				throw;
			}
		}
		//End 08-June-2009

	
		//22-May-2012			Priya			BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
		public static string GetYMCATransmitalDate(string paramYMCAuniqueId, string paramEffectiveDate )
       
		{
			Database db = null;
			DbCommand l_DBCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return string.Empty;

				l_DBCommandWrapper = db.GetStoredProcCommand("yrs_usp_ymca_GetYmcaTransmittalsDate");//("dbo.yrs_usp_RR_InsertDisbRefunds");
				
				if (l_DBCommandWrapper == null) return string.Empty;

				 db.AddInParameter(l_DBCommandWrapper, "@YmcaUniqueId", DbType.String, paramYMCAuniqueId);
				 db.AddInParameter(l_DBCommandWrapper, "@ResolutionEffDate", DbType.DateTime, paramEffectiveDate);
				 db.AddOutParameter(l_DBCommandWrapper, "@ResolutionDate", DbType.DateTime, 100);

				 db.ExecuteNonQuery(l_DBCommandWrapper);
				 return Convert.ToString(db.GetParameterValue(l_DBCommandWrapper, "@ResolutionDate"));

			 }
			 catch (Exception ex)
			 {
				 throw (ex);
			 }
				
		}
	}
}
