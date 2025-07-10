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
	/// Summary description for MetaPositionsMaintenanceDAClass.
	/// </summary>
	public class MetaPositionsMaintenanceDAClass
	{
		public MetaPositionsMaintenanceDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		// function returning Dataset containing all rows of table 'AtsMetaPositions' columns "Position Type" and "Description"
		public static DataSet LookUpPositionsMaintenance()
		{
			DataSet dsLookUpPositionsMaintenance = null;
			Database db = null;
			DbCommand commandLookUpPositionsMaintenance = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpPositionsMaintenance = db.GetStoredProcCommand("yrs_usp_AMPM_LookupPositionMaintenance");
				if (commandLookUpPositionsMaintenance ==null) return null;
				dsLookUpPositionsMaintenance = new DataSet();
				dsLookUpPositionsMaintenance.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpPositionsMaintenance,dsLookUpPositionsMaintenance,"Positions Maintenance");
				return dsLookUpPositionsMaintenance;
			}
			catch
			{
				throw;
			}

		}
		//function returning dataset for the search against 'Position Type'.
		//The DataSet contains the rows where Position Type= "%" + TextBoxValue + "%" of table 'AtsMetaPositions' 

		public static DataSet SearchPositionsMaintenance(string parameterSearchPositionsMaintenance)
		{
			DataSet dsSearchPositionsMaintenance = null;
			Database db = null;
			DbCommand CommandSearchPositionsMaintenance = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchPositionsMaintenance = db.GetStoredProcCommand("yrs_usp_AMPM_SearchPositionMaintenance");
				if (CommandSearchPositionsMaintenance ==null) return null;

				db.AddInParameter(CommandSearchPositionsMaintenance, "@varchar_PositionType",DbType.String,parameterSearchPositionsMaintenance);
				
				dsSearchPositionsMaintenance = new DataSet();
				dsSearchPositionsMaintenance.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchPositionsMaintenance,dsSearchPositionsMaintenance,"Positions Maintenance");

				return dsSearchPositionsMaintenance;
			}
			catch
			{
				throw;
			}

		}

		public static void InsertPositionsMaintenance(DataSet parameterInsertPositionsMaintenance)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				// Defining The Insert Command Wrapper With parameters
				//@char_PositionType,@varchar_ShortDesc,@varchar_Desc
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMPM_InsertPositionMaintenance");
				
				db.AddInParameter(insertCommandWrapper, "@char_PositionType",DbType.String, "Position Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short Desc",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Description",DbType.String,"Desc",DataRowVersion.Current);

				// Defining The Update Command Wrapper With parameters
				//@varchar_PositionType,@varchar_ShortDesc,@varchar_Desc and @integer_ReturnVal
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMPM_UpdatePositionMaintenance");
			
				db.AddInParameter(updateCommandWrapper, "@char_PositionType",DbType.String,"Position Type",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short Desc",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_Description",DbType.String,"Desc",DataRowVersion.Current);
				

				deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMPM_DeletePositionMaintenance");

				db.AddInParameter(deleteCommandWrapper, "@varchar_Positiontype",DbType.String,"Position Type", DataRowVersion.Original);
				
				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterInsertPositionsMaintenance != null)
				{
					db.UpdateDataSet(parameterInsertPositionsMaintenance,"Positions Maintenance" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Transactional);
				}
			}
			catch
			{
				throw;
			}
		}


	}
}
