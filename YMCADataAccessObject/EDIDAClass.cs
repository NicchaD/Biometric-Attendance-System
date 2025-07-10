// Change History
//****************************************************
//Modification History
//****************************************************
// Modified by      Date        Description
//****************************************************
// Sanjay R.        2010.06.28  Enhancement changes(Parameter Attribute DbType.String to DbType.guid)
// Manthan Rajguru  2015.09.16  YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//****************************************************

using System;
using System.Data;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.SqlClient;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for EDIDAClass.
	/// </summary>
	public class EDIDAClass
	{
		//by Aparna 18/06/2007
		//const string EDI_CONFIGURATION_KEY_MODE = "EDI_MODE";
		const string EDI_CONFIGURATION_CATEGORY_CODE = "EDIISA";
		public EDIDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

			
		public static DataSet GetEDIOutSourceList(string ParameterDisbursementType)
		{
			DataSet l_DataSet_CheckList =null;
			Database db=null;
			DbCommand  GetCommandWrapper = null;
			string[]l_TableNames;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				//yrs_usp_EDI_GetCheckCurrentMonthlyData
				GetCommandWrapper=db.GetStoredProcCommand ("yrs_usp_EDI_GetCheckOutsourceList");
				db.AddInParameter(GetCommandWrapper,"@varchar_DisbursementType",DbType.String,ParameterDisbursementType);
                GetCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 

				if(GetCommandWrapper==null) return null;
				l_DataSet_CheckList=new DataSet();
				l_TableNames = new string [] {"AtsPayroll Dates" ,"EDIUS List"};
				db.LoadDataSet(GetCommandWrapper,l_DataSet_CheckList,l_TableNames);
				return l_DataSet_CheckList;

			}
			catch
			{
				throw;
			}
		}

		public static void UpdateEDIProcessData(DataSet ParameterPayrolllist,string ParameterDisbursementType,string ParameterProcessId, int ParameterBatchNo,string ParameterEDIOutputfileId)
		{
			
			Database db=null;
			DbCommand  insertCommandWrapper = null;
			DbCommand  UpdateCommandWrapper = null;
			DbCommand  deleteCommandWrapper=null;
			DbTransaction l_IDbTransaction = null;
		 	DbConnection l_IDbConnection = null;
			DbCommand  l_DBCommandWrapper;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return ;
			
				l_IDbConnection =db.CreateConnection ();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return;
				l_IDbTransaction = l_IDbConnection.BeginTransaction();

				UpdateCommandWrapper=db.GetStoredProcCommand("yrs_usp_EDI_UpdateEDIProcessingList");
				UpdateCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if(UpdateCommandWrapper !=null)
				{
					db.AddInParameter(UpdateCommandWrapper,"@varchar_DisbursementType",DbType.String,ParameterDisbursementType);
                    db.AddInParameter(UpdateCommandWrapper, "@guiProcessId", DbType.String, ParameterProcessId); 
                    db.AddInParameter(UpdateCommandWrapper, "@guiOutputFileId", DbType.String, ParameterEDIOutputfileId); 
					db.AddInParameter(UpdateCommandWrapper,"@guiDisbursementId",DbType.Guid,"DisbursmentID",DataRowVersion.Current);
					db.AddInParameter(UpdateCommandWrapper,"@bitProcessed",DbType.Int32,"bitProcessed",DataRowVersion.Current);
					db.AddInParameter(UpdateCommandWrapper,"@bitExcluded",DbType.Int32,"bitExcluded",DataRowVersion.Current);
					//UpdateCommandWrapper.AddInParameter("@bitPayrollProcessed",DbType.String,ParameterPayrollProcessed);
					db.AddInParameter(UpdateCommandWrapper,"@numBatchNo",DbType.Int32,ParameterBatchNo);					
					db.UpdateDataSet(ParameterPayrolllist,"EDIUS List",insertCommandWrapper,UpdateCommandWrapper,deleteCommandWrapper,l_IDbTransaction);
					
					// Start to another command wrapper object to call the insertion operation.
					
					l_DBCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_EDI_InsertEDIBatchRecord");
					l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
					db.AddInParameter(l_DBCommandWrapper,"@varchar_DisbursementType",DbType.String,ParameterDisbursementType);
                    db.AddInParameter(l_DBCommandWrapper, "@guiProcessId", DbType.String , ParameterProcessId); 
                    db.AddInParameter(l_DBCommandWrapper, "@guiOutputFileId", DbType.String, ParameterEDIOutputfileId); 
					db.AddInParameter(l_DBCommandWrapper,"@numBatchNo",DbType.Int32,ParameterBatchNo);					
					db.ExecuteNonQuery(l_DBCommandWrapper,l_IDbTransaction);

					l_IDbTransaction.Commit();		

				}	
			}

			catch(SqlException SqlEx)
			{  
				l_IDbTransaction.Rollback();
				throw SqlEx;
			}
			catch
			{
				l_IDbTransaction.Rollback();
				throw;
			}
			finally
			{
				if (l_IDbConnection != null)
				{
					if (l_IDbConnection.State != ConnectionState.Closed)
					{
						l_IDbConnection.Close ();
					}
				}
			}
		}

		public static DataSet GetEDIProcessData(string ParameterDisbursementType)
		{
			DataSet l_Dataset_EDIProcessdata =null;
			Database db = null;
			DbCommand  GetCommandWrapper = null;
			string[] l_TableNames ;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				//yrs_usp_EDI_GetCheckCurrentMonthlyData
				GetCommandWrapper = db.GetStoredProcCommand ("yrs_usp_EDI_GetEDIUSPayrolldata");
				db.AddInParameter(GetCommandWrapper,"@varchar_DisbursementType",DbType.String,ParameterDisbursementType);
				GetCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

				if (GetCommandWrapper==null) return null;
				l_Dataset_EDIProcessdata = new DataSet();
				l_TableNames = new string [] {"EDIUS List","FedTaxWithholdingRO", "Withholdings","YTDWithholdings", "OutPutfilePath"};
				db.LoadDataSet(GetCommandWrapper,l_Dataset_EDIProcessdata,l_TableNames);
				return l_Dataset_EDIProcessdata;

			}
			catch
			{
				throw;
			}
		}

		public static bool InsertOutPutFileTypes(DataSet DatasetOutPutFileTypes )
		{
			Database l_DataBase = null;

			DbCommand  InsertDBCommandWrapper = null; 
			DbCommand  UpdateDBCommandWrapper = null; 
			DbCommand  DeleteDBCommandWrapper = null;
			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection = null;
			
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (DatasetOutPutFileTypes == null) return false;
				l_IDbConnection =l_DataBase.CreateConnection ();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return false;
				l_IDbTransaction = l_IDbConnection.BeginTransaction();

				if (l_DataBase  == null) return false;

//				InsertDBCommandWrapper = l_DataBase.GetStoredProcCommandWrapper ("dbo.yrs_usp_Payroll_InsertDisbursementFiles");
//				if (InsertDBCommandWrapper != null)
//				{					
//					InsertDBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ExtraLargeConnectionTimeOut"]);
//					InsertDBCommandWrapper.AddInParameter ("@DisbursementID", DbType.String, "DisbursementID" , DataRowVersion.Current);
//					InsertDBCommandWrapper.AddInParameter ("@OutputFileID", DbType.String, "GuiUniqueID" , DataRowVersion.Current);
//					
//					l_DataBase.UpdateDataSet(DatasetdisbursementFileTypes, "DisbursementFiles", InsertDBCommandWrapper, UpdateDBCommandWrapper, DeleteDBCommandWrapper,P_DBTransaction);
//					
//				}
//				else
//				{
//					return false;
//				}

				InsertDBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_Payroll_InsertDisbursementOutputFiles");
				
				if (InsertDBCommandWrapper != null)
				{
					InsertDBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@UniqueID", DbType.Guid , "GuiUniqueID", DataRowVersion.Current);  //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.06.28 for migration
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileLocation", DbType.String, "chvOutputFileLocation", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileName", DbType.String, "chvOutputFileName", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileType", DbType.String, "chrOutputFileType", DataRowVersion.Current);
					
					l_DataBase.UpdateDataSet(DatasetOutPutFileTypes, "OutPutfilePath", InsertDBCommandWrapper, UpdateDBCommandWrapper, DeleteDBCommandWrapper,l_IDbTransaction);
					l_IDbTransaction.Commit();
				}
				else
				{
					return false;
				}

				return true;

			}
			catch(SqlException SqlEx)
			{  
				l_IDbTransaction.Rollback();
				throw SqlEx;
			}
			catch
			{
				l_IDbTransaction.Rollback();
				throw;
			}
			finally
			{
				if (l_IDbConnection != null)
				{
					if (l_IDbConnection.State != ConnectionState.Closed)
					{
						l_IDbConnection.Close ();
					}
				}
			}
		}
			
//		public static string getEDIModeFromDatabase() 
//		{
//			
//			Database l_DataBase = null;
//			DBCommandWrapper l_DBCommandWrapper = null;			
//			IDbConnection DBconnectYRS = null;
//			DataSet ds = null;
//
//			try 
//			{
//				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
//				DBconnectYRS = l_DataBase.GetConnection();
//				DBconnectYRS.Open ();
//
//				if (l_DataBase == null) throw new Exception("Unable to initialize database object");
//				l_DBCommandWrapper = l_DataBase.GetStoredProcCommandWrapper ("dbo.yrs_usp_AtsMetaConfiguration_Get");
//				l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ExtraLargeConnectionTimeOut"]);
//				if (l_DBCommandWrapper == null) throw new Exception("Unable to initialize command wrapper object");
//				l_DBCommandWrapper.AddInParameter ("@key", DbType.String, EDI_CONFIGURATION_KEY_MODE);
//				ds = new DataSet ("EDI Configuration Values");
//				l_DataBase.LoadDataSet (l_DBCommandWrapper,ds, "EDI CONFIGURATION VALUES");
//
//			} 
//			finally 
//			{
//				if (DBconnectYRS != null) DBconnectYRS.Close(); 
//			}
//			if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
//			{
//				throw new Exception("The key EDI_MODE needs to be defined in the Database in the Configuration table");
//			}
//			DataRow[] dr = ds.Tables[0].Select("[Key]=" + "'EDI_MODE'");
//			if (dr.Length == 0 || dr[0].IsNull("Value") ) 
//			{
//				throw new Exception("The value for the key EDI_MODE needs to be defined in the Database in the Configuration table");
//			}
//			switch (dr[0]["Value"].ToString().ToUpper()) 
//			{
//				case "TEST": 
//					return "T";
//				case "PRODUCTION": 
//					return "P";
//				default: 
//					throw new Exception("The value for the key EDI_MODE is not a valid value.");
//			}
//		}
		// by Aparna 05/07/07
		public static DataSet getEDIModeFromDatabase() 
		{
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;			
			DbConnection DBconnectYRS = null;
			DataSet ds = null;

			try 
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = l_DataBase.CreateConnection();
				DBconnectYRS.Open ();

				if (l_DataBase == null) throw new Exception("Unable to initialize database object");
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand  ("dbo.yrs_usp_AtsMetaConfiguration_by_ConfigCategoryCode");
				l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) throw new Exception("Unable to initialize command wrapper object");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@ConfigCategoryCode", DbType.String, EDI_CONFIGURATION_CATEGORY_CODE);
				ds = new DataSet ("EDI Configuration Values");
				l_DataBase.LoadDataSet (l_DBCommandWrapper,ds, "EDI CONFIGURATION VALUES");
				return ds;
			} 
			catch
			{
				throw;

			}
			finally 
			{
				if (DBconnectYRS != null) DBconnectYRS.Close(); 
			}
					
			
		}
		
	}

}
