//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for ACHDebitExportDAClass.
	/// </summary>
	public class ACHDebitExportDAClass
	{
		public ACHDebitExportDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet GetPendingACHDebits()
		{	DataSet l_DataSet_ACHDebits=null;
			Database db=null;
			
			DbCommand GetCommandWrapper=null;
			try
			{	db=DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				GetCommandWrapper=db.GetStoredProcCommand("yrs_usp_DisplayACHDebits");
				GetCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if (GetCommandWrapper == null) return null;
				l_DataSet_ACHDebits = new DataSet();
				db.LoadDataSet(GetCommandWrapper, l_DataSet_ACHDebits,"ACHDebits");
				
				return l_DataSet_ACHDebits;
			}
			catch
			{	throw;
			}
		}
		public static void DeleteACHDebits(string parameteruniqueid)
		{
				Database db = null;
			//DbCommand UpdateCommandWrapper = null;
			//DbCommand InsertCommandWrapper = null;
			DbCommand DeleteCommandWrapper = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return ;

				DeleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeleteACHDebit");
				
				if (DeleteCommandWrapper == null) return ;

				
				db.AddInParameter(DeleteCommandWrapper,"@varchar_guiUniqueId",DbType.String,parameteruniqueid);
				//DeleteCommandWrapper.AddInParameter("@varchar_guiUniqueId",DbType.Guid ,"uniqueid",DataRowVersion.Current);
				
				db.ExecuteNonQuery(DeleteCommandWrapper);	
				//********Code comment by ashutosh on 15 Sep 2006
				//db.UpdateDataSet(l_DataSetfordelete,"ACHDebits" ,InsertCommandWrapper,UpdateCommandWrapper,DeleteCommandWrapper,UpdateBehavior.Standard);
				//***********
				return;
				//if (parameteruniqueid != null)
				//{
					//db.UpdateDataSet(parameteruniqueid,"ACHDebits",deleteCommandWrapper,UpdateBehavior.Standard);
				//}
			}
			catch
			{
					throw;
			}
		}
		public static void UpdateOnExport(DataSet parameterdsExport,string batchid)
		{
			Database db = null;
						
			 DbCommand UpdateCommandWrapper = null;
			DbCommand InsertCommandWrapper = null;
			DbCommand DeleteCommandWrapper = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return ;

				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_ACH_UpdateUsingBatchId");
		
				if (UpdateCommandWrapper == null) return ;

				
				db.AddInParameter(UpdateCommandWrapper,"@varchar_guiUniqueId",DbType.Guid ,"uniqueid",DataRowVersion.Current);
				db.AddInParameter(UpdateCommandWrapper,"@varchar_batchid",DbType.String,batchid);

				db.UpdateDataSet(parameterdsExport,"ACHDebits" ,InsertCommandWrapper,UpdateCommandWrapper,DeleteCommandWrapper,UpdateBehavior.Standard);

				return;
			}
			catch
			{throw;
			}

		}
		public static string GetBatchId()
		{
			Database db=null;
			string batchid;
			DbCommand GetCommandWrapper=null;
			try
			{		db= DatabaseFactory.CreateDatabase("YRS");
					GetCommandWrapper = db.GetStoredProcCommand("yrs_usp_ACH_GetBatchId");
			
				if (GetCommandWrapper == null) return null;
				
				//db.lLoadDataSet(GetCommandWrapper, batchid,"ACHDebits");
				db.AddOutParameter(GetCommandWrapper,"@chvOutBatchId",DbType.String,20);
				db.ExecuteScalar(GetCommandWrapper);
				
				batchid=Convert.ToString(db.GetParameterValue(GetCommandWrapper,"@chvOutBatchId"));

				return batchid;
			
			
			}
			catch
			{throw;
			}
			
		}
		public static DataSet getMetaOutputFileType()
		{
			DataSet dsMetaOutputFileType = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaOutputFileType");
				
				if (getCommandWrapper == null) return null;
					
				db.AddInParameter(getCommandWrapper,"@char_MetaOutputFileType",DbType.String,"ACHDEB");

				dsMetaOutputFileType = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsMetaOutputFileType,"MetaOutputFileType");
				
				return dsMetaOutputFileType;
			}
			catch 
			{
				throw;
			}
		}

		public static  int UpdateACHDebits(string parameteruniqueid,double parameteramount,string parameterpaydate)
		{
			Database db = null;
			DbCommand UpdateCommandWrapper = null;
			int l_output;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				//if (db == null) return null;

				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateYmcaACHdebit");
				
				//if (UpdateCommandWrapper == null) return null;

				
				db.AddInParameter(UpdateCommandWrapper,"@varchar_uniqueid",DbType.String,parameteruniqueid);
				db.AddInParameter(UpdateCommandWrapper,"@numeric_amount",DbType.Double,parameteramount);

				db.AddInParameter(UpdateCommandWrapper,"@datetime_paymentdate",DbType.String,parameterpaydate);
				db.AddOutParameter(UpdateCommandWrapper,"@bool_date",DbType.Int32,1);
				db.ExecuteNonQuery(UpdateCommandWrapper);
				l_output = (int)(db.GetParameterValue(UpdateCommandWrapper,"@bool_date"));
				return l_output;
			}
			catch
			{	throw;
			}
		}


	}
}
