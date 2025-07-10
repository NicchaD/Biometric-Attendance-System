//****************************************************
//Modification History
//****************************************************
// Date             Modified by         Description
//-------------------------------------------------
// 2009.05.18       NP/PP/SR            Optimizing the YMCA Screen
// 2015.09.16       Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//****************************************************
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
	/// Summary description for YMCATelephoneDAClass.
	/// </summary>
	public sealed class YMCATelephoneDAClass
	{
		private YMCATelephoneDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpTelephoneType()
		{
			DataSet dsLookUpATelephoneType = null;
			Database db = null;
			DbCommand commandLookUpTelephoneType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpTelephoneType = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCATelephoneType");
				if (commandLookUpTelephoneType ==null) return null;
				dsLookUpATelephoneType = new DataSet();
				dsLookUpATelephoneType.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpTelephoneType,dsLookUpATelephoneType,"Telephone Type");
				return dsLookUpATelephoneType;
			}
			catch
			{
				throw;
			}

		}


		public static DataSet SearchTelephoneInformation(string parameterSearchTelephoneGuid)
		{
			DataSet dsSearchTelephone = null;
			Database db = null;
			DbCommand CommandSearchTelephone = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchTelephone = db.GetStoredProcCommand("yrs_usp_AMY_SearchYmcaGeneralTelephone");
				if (CommandSearchTelephone ==null) return null;

				db.AddInParameter(CommandSearchTelephone,"@UniqueIdentifier_GuiUniqueId",DbType.String,parameterSearchTelephoneGuid);
				
				dsSearchTelephone = new DataSet();
				dsSearchTelephone.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchTelephone,dsSearchTelephone,"Telephone Information");

				return dsSearchTelephone;
			}
			catch
			{
				throw;
			}
		}

		public static void InsertTelephoneInformation(DataSet parameterTelephoneInformation)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralTelephone");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Telephone",DbType.String,"Telephone",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_ext",DbType.String,"Ext.",DataRowVersion.Current);
				
																												
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralTelephone");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Telephone",DbType.String,"Telephone",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Ext",DbType.String,"Ext.",DataRowVersion.Current);
													
				//deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMAT_DeleteAccountTypes");
				// Defining The Delete Command Wrapper With parameters
				//deleteCommandWrapper.AddInParameter("@varchar_AcctType",DbType.String,"Acct. Type",DataRowVersion.Original);
								
				if (parameterTelephoneInformation != null)
				{
					db.UpdateDataSet(parameterTelephoneInformation,"Telephone Information" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}

			}
			catch
			{
				throw;
			}
		}



		// this is for add button click of General tab

		public static string InsertTelephoneInformationAdd(DataSet parameterTelephoneInformation)
		{
			string l_stringAtsTelephoneGuiUniqueId;
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralTelephoneAdd");
				// Defining The Insert Command Wrapper With parameters
                db.AddInParameter(insertCommandWrapper, "@varchar_Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_Primary", DbType.Int16, "Primary", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@bit_Active", DbType.Int16, "Active", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_Telephone", DbType.String, "Telephone", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_ext", DbType.String, "Ext.", DataRowVersion.Current);
                db.AddOutParameter(insertCommandWrapper, "@varchar_ReturnGuiUniqueId", DbType.String, 100);
                db.AddOutParameter(insertCommandWrapper, "@varchar_ReturnGuiEntityId", DbType.String, 100);
				
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralTelephoneInformationAdd");
				// Defining The Update Command Wrapper With parameters

                db.AddInParameter(updateCommandWrapper, "@uniqueIdentifier_GuiEntityId", DbType.String, "guiEntityID", DataRowVersion.Original);
                db.AddInParameter(updateCommandWrapper, "@uniqueIdentifier_GuiUniqueID", DbType.String, "guiUniqueID", DataRowVersion.Original);
                db.AddInParameter(updateCommandWrapper, "@varchar_Type", DbType.String, "Type", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@bit_Primary", DbType.Int16, "Primary", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@bit_Active", DbType.Int16, "Active", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@datetime_EffDate", DbType.DateTime, "Effective Date", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Telephone", DbType.String, "Telephone", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Ext", DbType.String, "Ext.", DataRowVersion.Current);
																		
				//deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMAT_DeleteAccountTypes");
				// Defining The Delete Command Wrapper With parameters
				//deleteCommandWrapper.AddInParameter("@varchar_AcctType",DbType.String,"Acct. Type",DataRowVersion.Original);
								
				if (parameterTelephoneInformation != null)
				{
					db.UpdateDataSet(parameterTelephoneInformation,"Telephone Schema" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				l_stringAtsTelephoneGuiUniqueId=(string)db.GetParameterValue(insertCommandWrapper,"@varchar_ReturnGuiUniqueId");
				
				return l_stringAtsTelephoneGuiUniqueId;
			}
			catch
			{
				throw;
			}
		}


		// this is for add button click of General tab

		public static void UpdateTelephoneInformationAdd(DataSet parameterTelephoneInformation)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralTelephoneAdd");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Telephone",DbType.String,"Telephone",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_ext",DbType.String,"Ext.",DataRowVersion.Current);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiUniqueId",DbType.String,100);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiEntityId",DbType.String,100);
																																
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralTelephoneInformationAdd");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Telephone",DbType.String,"Telephone",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Ext",DbType.String,"Ext.",DataRowVersion.Current);
													
				if (parameterTelephoneInformation != null)
				{
					db.UpdateDataSet(parameterTelephoneInformation,"Telephone Schema" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch
			{
				throw;
			}
		}


	}
}
