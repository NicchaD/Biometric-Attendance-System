//****************************************************
//Modification History
//****************************************************
//Modified by         Date             Description
//****************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for MetaCountryTypeDAClass.
	/// </summary>
	public sealed class MetaCountryTypeDAClass
	{
		private MetaCountryTypeDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookupCountryTypes()
		{
			DataSet dsLookUpCountryTypes = null;
			Database db = null;
			DbCommand commandLookUpCountryTypes = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				commandLookUpCountryTypes = db.GetStoredProcCommand("yrs_usp_AMCT_LookupCountryTypes");
						
				if (commandLookUpCountryTypes == null) return null;
		
				dsLookUpCountryTypes = new DataSet();
				dsLookUpCountryTypes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpCountryTypes, dsLookUpCountryTypes,"Country Types");
						
				return dsLookUpCountryTypes;
			}
			catch 
			{
				throw;
			}

		}

		public static DataSet SearchCountryTypes(string parameterSearchCountryTypes)
		{
			DataSet dsSearchCountryTypes = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMCT_SearchCountryTypes");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper, "@varchar_Abbrev",DbType.String,parameterSearchCountryTypes);
				//							
				dsSearchCountryTypes = new DataSet();
				dsSearchCountryTypes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchCountryTypes,"Country Types");
				
				return dsSearchCountryTypes;
			}
			catch 
			{
				throw;
			}
		}

		public static void InsertCountryTypes(DataSet parameterCountryTypes)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMCT_InsertCountryTypes");
				
				db.AddInParameter(insertCommandWrapper, "@varchar_Abbrev",DbType.String,"Abbreviation",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_CodeValue",DbType.String,"Code Value",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Description",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_Editable",DbType.Int16,"Editable",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@datetime_EffDate",DbType.DateTime,"Eff Date",DataRowVersion.Current);
				

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMCT_UpdateCountryTypes");
				
				db.AddInParameter(updateCommandWrapper, "@varchar_Abbrev",DbType.String,"Abbreviation",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper, "@varchar_CodeValue",DbType.String,"Code Value",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_Description",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_Editable",DbType.Int16,"Editable",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@datetime_EffDate",DbType.DateTime,"Eff Date",DataRowVersion.Current);

				deleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMCT_DeleteCountryTypes");
				
				db.AddInParameter(deleteCommandWrapper, "@varchar_Abbrev",DbType.String,"Abbreviation",DataRowVersion.Original);
				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterCountryTypes != null)
				{
					db.UpdateDataSet(parameterCountryTypes,"Country Types" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}

	}
}
