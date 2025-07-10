//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
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
	/// Summary description for MetaVestingTypeCodeDAClass.
	/// </summary>
	public sealed class MetaVestingTypeCodeDAClass
	{
		private MetaVestingTypeCodeDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// function returning Dataset containing all rows of table 'AtsMetaVestingTypeCodes' columns "VestingTypeCode" and "Description"
		public static DataSet LookUpVestingTypeCode()
		{
			DataSet dsLookUpVestingTypeCode = null;
			Database db = null;
			DbCommand commandLookUpVestingTypeCode = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpVestingTypeCode = db.GetStoredProcCommand("yrs_usp_AMVTC_LookupVestingTypeCode");
				if (commandLookUpVestingTypeCode ==null) return null;
				dsLookUpVestingTypeCode = new DataSet();
				dsLookUpVestingTypeCode.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpVestingTypeCode,dsLookUpVestingTypeCode,"Vesting Type Code");
				return dsLookUpVestingTypeCode;
			}
			catch
			{
				throw;
			}

		}
		
		public static void InsertVestingTypeCodes(DataSet parameterInsertVestingTypeCode)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMVTC_InsertVestingTypeCode");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper, "@char_VestingTypeCode",DbType.String, "Vesting Type Code",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_ShortDescription",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Description",DbType.String,"Desc",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@int_EligMonth",DbType.Int32,"Elig Month",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@int_VestedMonth",DbType.Int32,"Vested Month",DataRowVersion.Current);

				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMVTC_UpdateVestingTypeCode");
				// Defining The Update Command Wrapper With parameters
				db.AddInParameter(updateCommandWrapper, "@char_VestingTypeCode",DbType.String, "Vesting Type Code",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper, "@varchar_ShortDescription",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_Description",DbType.String,"Desc",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@int_EligMonth",DbType.Int32,"Elig Month",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@int_VestedMonth",DbType.Int32,"Vested Month",DataRowVersion.Current);

				deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMVTC_DeleteVestingTypeCode");
				// Defining The Delete Command Wrapper With parameters
				db.AddInParameter(deleteCommandWrapper, "@char_VestingTypeCode",DbType.String,"Vesting Type Code",DataRowVersion.Original);
								
				if (parameterInsertVestingTypeCode != null)
				{
					db.UpdateDataSet(parameterInsertVestingTypeCode,"Vesting Type Code" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
			}
			catch
			{
				throw;
			}
		}
		public static DataSet SearchVestingTypeCode(string parameterSearchVestingTypeCode)
		{
			DataSet dsSearchVestingTypeCode = null;
			Database db = null;
			DbCommand CommandSearchVestingTypeCode = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchVestingTypeCode = db.GetStoredProcCommand("yrs_usp_AMVTC_SearchVestingTypeCode");
				if (CommandSearchVestingTypeCode ==null) return null;

				db.AddInParameter(CommandSearchVestingTypeCode, "@char_VestingtypeCode",DbType.String,parameterSearchVestingTypeCode);
				
				dsSearchVestingTypeCode = new DataSet();
				dsSearchVestingTypeCode.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchVestingTypeCode,dsSearchVestingTypeCode,"Vesting Type Code");

				return dsSearchVestingTypeCode;
			}
			catch
			{
				throw;
			}

		}

	}
}
