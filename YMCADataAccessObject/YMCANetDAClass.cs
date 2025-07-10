/********************************************************************************************************************************
 Modification History
'********************************************************************************************************************************
'Modified By			Date					Description
'********************************************************************************************************************************
//NP/PP/SR              2009.05.18              Optimizing the YMCA Screen
//Manthan Rajguru       2015.09.16              YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************/
using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;   

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for YMCANetDAClass.
	/// </summary>
	public sealed class YMCANetDAClass
	{
		private YMCANetDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpEmailType()
		{
			DataSet dsLookUpEmailType = null;
			Database db = null;
			DbCommand commandLookUpEmailType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpEmailType = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAEmailType");
				if (commandLookUpEmailType ==null) return null;
				dsLookUpEmailType = new DataSet();
				dsLookUpEmailType.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpEmailType,dsLookUpEmailType,"Email Type");
				return dsLookUpEmailType;
			}
			catch
			{
				throw;
			}

		}


		public static DataSet SearchEmailInformation(string parameterSearchEmailGuid)
		{
			DataSet dsSearchEmail = null;
			Database db = null;
			DbCommand CommandSearchEmail = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchEmail = db.GetStoredProcCommand("yrs_usp_AMY_SearchYmcaGeneralEmail");
				if (CommandSearchEmail ==null) return null;

				db.AddInParameter(CommandSearchEmail,"@UniqueIdentifier_GuiUniqueId",DbType.String,parameterSearchEmailGuid);
				
				dsSearchEmail = new DataSet();
				dsSearchEmail.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchEmail,dsSearchEmail,"Email Information");

				return dsSearchEmail;
			}
			catch
			{
				throw;
			}
		}
	
		public static void InsertEmailInformation(DataSet parameterEmailInformation)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralEmail");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_EmailAddress",DbType.String,"Email Address",DataRowVersion.Current);
								
																												
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralEmail");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_EmailAddress",DbType.String,"Email Address",DataRowVersion.Current);
				
													
				//deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMAT_DeleteAccountTypes");
				// Defining The Delete Command Wrapper With parameters
				//deleteCommandWrapper.AddInParameter("@varchar_AcctType",DbType.String,"Acct. Type",DataRowVersion.Original);
								
				if (parameterEmailInformation != null)
				{
					db.UpdateDataSet(parameterEmailInformation,"Email Information" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}

			}
			catch
			{
				throw;
			}
		}
		
		// this is for add button click of General tab

		public static string InsertEmailInformationAdd(DataSet parameterEmailInformation)
		{
			string l_stringAtsEmailGuiUniqueId;
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralEmailAdd");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_EmailAddress",DbType.String,"Email Address",DataRowVersion.Current);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiUniqueId",DbType.String,100);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiEntityId",DbType.String,100);
				
																																
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralEmailInformationAdd");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Default);				
				db.AddInParameter(updateCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@varchar_EmailAddress",DbType.String,"Email Address",DataRowVersion.Default);
																																						
				
				if (parameterEmailInformation != null)
				{
					db.UpdateDataSet(parameterEmailInformation,"Email Schema" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				l_stringAtsEmailGuiUniqueId=(string)db.GetParameterValue(insertCommandWrapper,"@varchar_ReturnGuiUniqueId");
				
				return l_stringAtsEmailGuiUniqueId;
			}
			catch
			{
				throw;
			}
		}

		// this is for add button click of General tab

		public static void UpdateEmailInformationAdd(DataSet parameterEmailInformation)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	

				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMY_InsertYmcaGeneralEmailAdd");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_EmailAddress",DbType.String,"Email Address",DataRowVersion.Current);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiUniqueId",DbType.String,100);
				db.AddOutParameter(insertCommandWrapper,"@varchar_ReturnGuiEntityId",DbType.String,100);
				
																																
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMY_UpdateYmcaGeneralEmailInformationAdd");
				// Defining The Update Command Wrapper With parameters

				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiUniqueID",DbType.String,"guiUniqueID",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@uniqueIdentifier_GuiEntityId",DbType.String, "guiEntityID",DataRowVersion.Default);				
				db.AddInParameter(updateCommandWrapper,"@varchar_Type",DbType.String,"Type",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@bit_Primary",DbType.Int16,"Primary",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Default);
				db.AddInParameter(updateCommandWrapper,"@varchar_EmailAddress",DbType.String,"Email Address",DataRowVersion.Default);
																	
				if (parameterEmailInformation != null)
				{
					db.UpdateDataSet(parameterEmailInformation,"Email Schema" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch
			{
				throw;
			}
		}



	}
}
