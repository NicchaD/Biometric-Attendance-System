//****************************************************
//Modification History
//****************************************************
//Modified by         Date            Description
//****************************************************
//Manthan Rajguru     2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for ReceiptsLockBoxImportDAClass.
	/// </summary>
	public class ReceiptsLockBoxImportDAClass
	{
		public static DataSet CheckFileImported(string paramFileName)
		{
			Database db = null;
			DbCommand  CommandCheckFileImported = null;
			DataSet dsCheckFileImported = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				CommandCheckFileImported=db.GetStoredProcCommand ("yrs_usp_APL_CheckFileAlreadyImported");
				if(CommandCheckFileImported==null) return null;
				db.AddInParameter(CommandCheckFileImported,"@varchar_FileName",DbType.String,paramFileName);
				dsCheckFileImported= new DataSet();
				db.LoadDataSet(CommandCheckFileImported,dsCheckFileImported,"AlreadyInported");
				return dsCheckFileImported;

			}
			catch
			{
				throw;
			}
		}
		public static void InsertImportedFile(string paramFileName)
		{
			Database db = null;
			DbCommand  CommandInsertFileImported = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				CommandInsertFileImported=db.GetStoredProcCommand ("yrs_usp_APL_InsertImportedFile");
				db.AddInParameter(CommandInsertFileImported,"@varchar_FileName",DbType.String,paramFileName);
				db.ExecuteNonQuery(CommandInsertFileImported);

			}
			catch
			{
				throw;
			}
		}
		public static void UpdateProcessLog(string paramFileName)
		{
			Database db = null;
			DbCommand  CommandUpdateProcessLog = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				CommandUpdateProcessLog = db.GetStoredProcCommand ("yrs_usp_APL_PostImportedFile");
				db.AddInParameter(CommandUpdateProcessLog,"@varchar_FileName",DbType.String,paramFileName);
				db.ExecuteNonQuery(CommandUpdateProcessLog);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getMetaOutputFileType()
		{
			DataSet dsMetaOutputFileType = null;
			Database db = null;
			DbCommand  getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand ("yrs_usp_MetaOutputFileType");
				
				if (getCommandWrapper == null) return null;
					
				db.AddInParameter(getCommandWrapper,"@char_MetaOutputFileType",DbType.String,"LOKBOX");

				dsMetaOutputFileType = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsMetaOutputFileType,"MetaOutputFileType");
				
				return dsMetaOutputFileType;
			}
			catch 
			{
				throw;
			}
		}

	}
}
