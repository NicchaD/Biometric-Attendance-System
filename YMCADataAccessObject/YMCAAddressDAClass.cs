//****************************************************
//Modification History
//****************************************************
//Date              Modified by         Description
//*****************************************************
//2009.05.18        NP/PP/SR            Optimizing the YMCA Screen
//2015.09.16        Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for YMCAAddressDAClass.
	/// </summary>
	public sealed class YMCAAddressDAClass
	{
		private YMCAAddressDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpAddressStateType()
		{
			DataSet dsLookUpAddressStateType = null;
			Database db = null;
			DbCommand commandLookUpAddressStateType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpAddressStateType = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAAddressState");
				if (commandLookUpAddressStateType ==null) return null;
				dsLookUpAddressStateType = new DataSet();
				dsLookUpAddressStateType.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpAddressStateType,dsLookUpAddressStateType,"State Type");
				return dsLookUpAddressStateType;
			}
			catch
			{
				throw;
			}

		}

		public static DataSet LookUpAddressType()
		{
			DataSet dsLookUpAddressType = null;
			Database db = null;
			DbCommand commandLookUpAddressType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpAddressType = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAAddressType");
				if (commandLookUpAddressType ==null) return null;
				dsLookUpAddressType = new DataSet();
				dsLookUpAddressType.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpAddressType,dsLookUpAddressType,"Type");
				return dsLookUpAddressType;
			}
			catch
			{
				throw;
			}

		}



		public static DataSet LookUpAddressCountry()
		{
			DataSet dsLookUpAddressCountry = null;
			Database db = null;
			DbCommand commandLookUpAddressCountry = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpAddressCountry = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAAddressCountry");
				if (commandLookUpAddressCountry ==null) return null;
				dsLookUpAddressCountry = new DataSet();
				dsLookUpAddressCountry.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpAddressCountry,dsLookUpAddressCountry,"Country");
				return dsLookUpAddressCountry;
			}
			catch
			{
				throw;
			}

		}


			public static DataSet SearchAddressInformation(string parameterSearchAddressInformationGuid)
		{
			DataSet dsSearchAddressInformation = null;
			Database db = null;
			DbCommand CommandSearchAddressInformation = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchAddressInformation = db.GetStoredProcCommand("yrs_usp_AMY_SearchYmcaGeneralAddressInformation");
				if (CommandSearchAddressInformation ==null) return null;

				db.AddInParameter(CommandSearchAddressInformation,"@UniqueIdentifier_GuiUniqueId",DbType.String,parameterSearchAddressInformationGuid);
				
				dsSearchAddressInformation = new DataSet();
				dsSearchAddressInformation.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchAddressInformation,dsSearchAddressInformation,"Address Information");

				return dsSearchAddressInformation;
			}
			catch
			{
				throw;
			}

		}

		public static void InsertAddressInformation(DataSet parameterAddressInformation)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralAddressInformation");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Primary",DbType.Int16,"Make this Primary",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_addr1",DbType.String,"Address",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_addr2",DbType.String,"Address 2",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_addr3",DbType.String,"Address 3",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_city",DbType.String,"City",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_state",DbType.String,"State",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_zip",DbType.String,"Zip",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Country",DbType.String,"Country",DataRowVersion.Current);
																												
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralAddressInformation");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Primary",DbType.Int16,"Make this Primary",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_addr1",DbType.String,"Address",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_addr2",DbType.String,"Address 2",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_addr3",DbType.String,"Address 3",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_city",DbType.String,"City",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@char_state",DbType.String,"State",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@char_zip",DbType.String,"Zip",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Country",DbType.String,"Country",DataRowVersion.Current);
													
				//deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMAT_DeleteAccountTypes");
				// Defining The Delete Command Wrapper With parameters
				//deleteCommandWrapper.AddInParameter("@varchar_AcctType",DbType.String,"Acct. Type",DataRowVersion.Original);
								
				if (parameterAddressInformation != null)
				{
					db.UpdateDataSet(parameterAddressInformation,"Address Information" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}

			}
			catch
			{
				throw;
			}
		}

// this is for add button click of General tab

		public static string InsertAddressInformationAdd(DataSet parameterAddressInformation)
		{
			string l_stringAtsAddrsGuiUniqueId;
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralAddressInformationAdd");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Primary",DbType.Int16,"Make this Primary",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_addr1",DbType.String,"Address",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_addr2",DbType.String,"Address 2",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_addr3",DbType.String,"Address 3",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_city",DbType.String,"City",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_state",DbType.String,"State",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_zip",DbType.String,"Zip",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Country",DbType.String,"Country",DataRowVersion.Current);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiUniqueId",DbType.String,100);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiEntityId",DbType.String,100);
				
																												
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralAddressInformationAdd");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Primary",DbType.Int16,"Make this Primary",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_addr1",DbType.String,"Address",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_addr2",DbType.String,"Address 2",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_addr3",DbType.String,"Address 3",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_city",DbType.String,"City",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@char_state",DbType.String,"State",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@char_zip",DbType.String,"Zip",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Country",DbType.String,"Country",DataRowVersion.Current);
													
				//deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMAT_DeleteAccountTypes");
				// Defining The Delete Command Wrapper With parameters
				//deleteCommandWrapper.AddInParameter("@varchar_AcctType",DbType.String,"Acct. Type",DataRowVersion.Original);
								
				if (parameterAddressInformation != null)
				{
					db.UpdateDataSet(parameterAddressInformation,"Address Information" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
			l_stringAtsAddrsGuiUniqueId=(string)db.GetParameterValue(insertCommandWrapper,"@varchar_ReturnGuiUniqueId");
				
				return l_stringAtsAddrsGuiUniqueId;
			}
			catch
			{
				throw;
			}
		}



		// this is for add button click of General tab

		public static void UpdateAddressInformationAdd(DataSet parameterAddressInformation)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralAddressInformationAdd");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Primary",DbType.Int16,"Make this Primary",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_addr1",DbType.String,"Address",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_addr2",DbType.String,"Address 2",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_addr3",DbType.String,"Address 3",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_city",DbType.String,"City",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_state",DbType.String,"State",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_zip",DbType.String,"Zip",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Country",DbType.String,"Country",DataRowVersion.Current);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiUniqueId",DbType.String,100);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiEntityId",DbType.String,100);
				
																																
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralAddressInformationAdd");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@bit_Primary",DbType.Int16,"Make this Primary",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@varchar_addr1",DbType.String,"Address",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@varchar_addr2",DbType.String,"Address 2",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@varchar_addr3",DbType.String,"Address 3",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@varchar_city",DbType.String,"City",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@char_state",DbType.String,"State",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@char_zip",DbType.String,"Zip",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@varchar_Country",DbType.String,"Country",DataRowVersion.Default);
													
				if (parameterAddressInformation != null)
				{
					db.UpdateDataSet(parameterAddressInformation,"Address Schema" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch
			{
				throw;
			}
		}

		public static DataSet SearchCountryDesc(string parameterCountryAbbrev)
		{
			DataSet dsSearchYMCACountry = null;
			Database db = null;
			DbCommand CommandSearchYMCACountry = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchYMCACountry = db.GetStoredProcCommand("yrs_usp_AMY_SearchCountryDesc");
				if (CommandSearchYMCACountry ==null) return null;

				db.AddInParameter(CommandSearchYMCACountry,"@varchar_Abbrev",DbType.String,parameterCountryAbbrev);
				dsSearchYMCACountry = new DataSet();
				dsSearchYMCACountry.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchYMCACountry,dsSearchYMCACountry,"YMCA Country Desc");

				return dsSearchYMCACountry;
			}
			catch
			{
				throw;
			}
		

		}
		//Procedure made my ashutosh 23-June-06 for calling States with countrycode .
		//Now the functionality changed first state chooses then Accoring to state Country come..
		public static DataSet GetStates()
		{
			DataSet dsGetStates = null;
			Database db = null;
			DbCommand CommandGetStates = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandGetStates = db.GetStoredProcCommand("dbo.yrs_usp_AMY_GetStates");
				if (CommandGetStates ==null) return null;

				//CommandGetStates.AddInParameter("@chvCountryCode",DbType.String,parameterCountryCode);
				dsGetStates = new DataSet();
				dsGetStates.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandGetStates,dsGetStates,"YMCA States");

				return dsGetStates;
			}
			catch
			{
				throw;
			}
		}



		//BS:2012.05.29:BT:951:YRS 5.0-1470: Link to Address Edit program from Person Maintenance
		//Procedure made my ashutosh 29-MAY-12 for calling Reason and Source of Address Notes.
		public static DataSet GetAddressNotesReasonSource(string parameterNotesSourceReason)
		{
			DataSet dsAddNoteReasonSources = null;
			Database db = null;
			DbCommand CommandAddNoteReasonSources = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;

				CommandAddNoteReasonSources = db.GetStoredProcCommand("dbo.yrs_usp_AMY_GetAddressNotesReasonSource");
				if (CommandAddNoteReasonSources == null) return null;

				db.AddInParameter(CommandAddNoteReasonSources, "@Varchar_NotesType", DbType.String, parameterNotesSourceReason);
     			dsAddNoteReasonSources = new DataSet();
				dsAddNoteReasonSources.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandAddNoteReasonSources, dsAddNoteReasonSources, "NotesReasonSources");

				return dsAddNoteReasonSources;
	}
			catch
			{
				throw;
			}
		}


	}
}
