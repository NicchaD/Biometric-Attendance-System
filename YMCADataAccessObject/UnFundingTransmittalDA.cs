//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for UnFundingTransmittalDA.
	/// </summary>
	public class UnFundingTransmittalDA
	{
		public UnFundingTransmittalDA()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpYmcaTransmittals(int FunctionType,string parameterYMCANo,string parameterTransmittalNo,string parameterReceiptNo,DateTime TransmittalSDate,DateTime TransmittalEDate)
		{
			DataSet dsYmcaTransmittals = null;
			Database db= null;
			string [] l_TableNames;
			DbCommand commandLookUpYmca = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				if (FunctionType==0)
				{
					commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_UT_SearchYMCASTransmittals_UnFund");
					if (commandLookUpYmca==null) return null;
					
					dsYmcaTransmittals = new DataSet();
					
					commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

					db.AddInParameter(commandLookUpYmca,"@varchar_YmcaNo",DbType.String,parameterYMCANo);
					db.AddInParameter(commandLookUpYmca,"@varchar_TransmittalNo",DbType.String,parameterTransmittalNo);
					//commandLookUpYmca.AddInParameter("@varchar_TransmittalDate",DbType.String,parameterTransmittalDate);
					db.AddInParameter(commandLookUpYmca,"@varchar_ReceiptNo",DbType.String,parameterReceiptNo);
					db.AddInParameter(commandLookUpYmca,"@dateTime_TransmittalSDate",DbType.DateTime,TransmittalSDate);
					db.AddInParameter(commandLookUpYmca,"@dateTime_TransmittalEDate",DbType.DateTime,TransmittalEDate);
					
					l_TableNames = new string[]{"PaidDateCurrentMonth"} ;				
					db.LoadDataSet(commandLookUpYmca,dsYmcaTransmittals,l_TableNames);
					
					return dsYmcaTransmittals;
				}
				else if (FunctionType==1)
				{
					commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_UT_SearchYMCASTransmittals_Delete");
					if (commandLookUpYmca==null) return null;
					dsYmcaTransmittals = new DataSet();
					commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
					db.AddInParameter(commandLookUpYmca,"@varchar_YmcaNo",DbType.String,parameterYMCANo);
					db.AddInParameter(commandLookUpYmca,"@varchar_TransmittalNo",DbType.String,parameterTransmittalNo);
					//commandLookUpYmca.AddInParameter("@varchar_TransmittalDate",DbType.String,parameterTransmittalDate);
					db.AddInParameter(commandLookUpYmca,"@varchar_ReceiptNo",DbType.String,parameterReceiptNo);
					db.AddInParameter(commandLookUpYmca,"@dateTime_TransmittalSDate",DbType.DateTime,TransmittalSDate);
					db.AddInParameter(commandLookUpYmca,"@dateTime_TransmittalEDate",DbType.DateTime,TransmittalEDate);
					l_TableNames = new string[]{"PaidDateCurrentMonth"} ;				
					db.LoadDataSet(commandLookUpYmca,dsYmcaTransmittals,l_TableNames);
					return dsYmcaTransmittals;
				}
				else
				{return null;}
			}
			catch 
			{
				throw ;
			}


		}
		
		//		public static DataSet LookUpYmcaCredits(string parameterUniqueID)
		//		{
		//			DataSet dsYmcaCredits = null;
		//			Database db= null;
		//			DbCommand commandLookUpYmca = null;
		//
		//			try
		//			{
		//				db= DatabaseFactory.CreateDatabase("YRS");
		//				
		//				if(db==null) return null;
		//				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_UT_SearchYMCASCredits");
		//				if (commandLookUpYmca==null) return null;
		//				dsYmcaCredits = new DataSet();
		//				commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
		//				commandLookUpYmca.AddInParameter("@varchar_UniqueId",DbType.String,parameterUniqueID);
		//				db.LoadDataSet(commandLookUpYmca,dsYmcaCredits,"Ymcas");
		//				return dsYmcaCredits;
		//			}
		//			catch 
		//			{
		//				throw ;
		//			}
		//		}
		//		public static DataSet LookUpYmcaReceipts(string parameterUniqueID)
		//		{
		//			DataSet dsYmcaReceipts = null;
		//			Database db= null;
		//			DbCommand commandLookUpYmca = null;
		//
		//			try
		//			{
		//				db= DatabaseFactory.CreateDatabase("YRS");
		//				
		//				if(db==null) return null;
		//				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_UT_SearchYMCASReceipts");
		//				if (commandLookUpYmca==null) return null;
		//				dsYmcaReceipts = new DataSet();
		//				commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
		//				commandLookUpYmca.AddInParameter("@varchar_UniqueId",DbType.String,parameterUniqueID);
		//				db.LoadDataSet(commandLookUpYmca,dsYmcaReceipts,"Ymcas");
		//				return dsYmcaReceipts;
		//			}
		//			catch 
		//			{
		//				throw ;
		//			}
		//		}
		
		public static int CheckAcctBalance(string parameterUniqueId)
		{
			//			
			//			Database db= null;
			//			DbCommand commandCheckAcctBalance = null;
			//
			//			try
			//			{
			//				db= DatabaseFactory.CreateDatabase("YRS");
			//				
			//				if(db==null) return null;
			//				commandCheckAcctBalance = db.GetStoredProcCommand("yrs_usp_UT_CheckAcctBalance");
			//				if (commandCheckAcctBalance==null) return null;
			//				dsAcctBalance = new DataSet();
			//				commandCheckAcctBalance.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
			//				commandCheckAcctBalance.AddInParameter("@varchar_guiTransmittalId",DbType.String,parameterUniqueId);
			//				
			//				db.LoadDataSet(commandCheckAcctBalance,dsAcctBalance,"CheckBalance");
			//				return dsAcctBalance;
			//			}
			DbCommand l_DBCommandWrapper;
			Database db = null;
			int l_int_output = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_output;
				l_DBCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_UT_CheckAcctBalance");
				
				l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return l_int_output;
				
				db.AddInParameter(l_DBCommandWrapper,"@varchar_guiTransmittalId",DbType.String,parameterUniqueId);
				db.AddOutParameter(l_DBCommandWrapper,"@int_output",DbType.Int32,2);
				db.ExecuteNonQuery(l_DBCommandWrapper);
				l_int_output = Convert.ToInt32(db.GetParameterValue(l_DBCommandWrapper,"@int_output"));
				return l_int_output;
			}
			catch 
			{
				throw ;
			}
			


		}
		//Swopna11Aug08
		public static int CheckAppliedReceiptsCredits(string parameterTransmittalNo)
		{
			
			DbCommand l_DBCommandWrapper;
			Database db = null;
			int l_int_output = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_output;
				l_DBCommandWrapper = db.GetStoredProcCommand ("yrs_usp_UT_AppliedReceiptsCreditsTransmittals");
				
				l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return l_int_output;
				
				db.AddInParameter(l_DBCommandWrapper,"@varchar_TransmittalNo",DbType.String,parameterTransmittalNo);
				db.AddOutParameter(l_DBCommandWrapper,"@int_output",DbType.Int32,2);
				db.ExecuteNonQuery(l_DBCommandWrapper);
				l_int_output = Convert.ToInt32(db.GetParameterValue(l_DBCommandWrapper,"@int_output"));
				return l_int_output;
			}
			catch 
			{
				throw ;
			}
			


		}
		//Swopna11Aug08

		//		public static void SaveUnFundingTransmittals(string parameterYmcaId,string parameterUniqueId,double parameterAmount,string parameterTransmittalNo)
		//		{
		//			//for maintaining transaction
		//			Database l_Database = null;
		//			DbConnection l_IDbConnection = null;
		//			DbTransaction l_IDbTransaction = null;
		//			
		//			try
		//			{
		//				l_Database = DatabaseFactory.CreateDatabase ("YRS");
		//				l_IDbConnection =  l_Database.CreateConnection();
		//				l_IDbConnection.Open ();
		//				
		//				
		//				l_IDbTransaction = l_IDbConnection.BeginTransaction ();
		//				//dsYmcaCredits = LookUpYmcaCredits(parameterUniqueId);
		////				if(dsYmcaCredits.Tables(0).Rows.Count>0)
		////				{
		////					RestoreCreditRecord(parameterUniqueId,l_Database,l_IDbTransaction);
		////				}
		//				
		//				DeleteYmcaAppliedReceipts(parameterUniqueId,l_Database,l_IDbTransaction);				
		//				RestoreReceiptRecord(parameterUniqueId,l_Database,l_IDbTransaction);
		//				RestoreCreditRecord(parameterUniqueId,l_Database,l_IDbTransaction);
		//				UpdateTransmittalRecord(parameterUniqueId,l_Database,l_IDbTransaction);
		//				InsertTransmittalLog(parameterUniqueId,parameterAmount,parameterTransmittalNo,"UNFUND",l_Database,l_IDbTransaction);
		//				l_IDbTransaction.Commit();
		//				
		//
		//			}
		//			catch (Exception ex)
		//			{
		//				//Rollback
		//				if (l_IDbTransaction != null)
		//				{
		//					l_IDbTransaction.Rollback ();
		//				}
		//				throw ex;
		//			}
		//			finally
		//			{
		//				if (l_IDbConnection != null)
		//				{
		//					if (l_IDbConnection.State != ConnectionState.Closed)
		//					{
		//						l_IDbConnection.Close ();
		//					}
		//				}
		//			}
		//		}
		public static void SaveUnFundingTransmittals(string parameterYmcaId,string parameterUniqueId,double parameterAmount,string parameterTransmittalNo)
		{
			Database db = null;			
			DbTransaction DBTransaction = null;
			DbConnection DBconnectYRS = null;
			bool bool_TransactionStarted = false;
			
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");		
				
				DBconnectYRS = db.CreateConnection();
				DBconnectYRS.Open();
				DBTransaction =  DBconnectYRS.BeginTransaction();
				bool_TransactionStarted = true;
				//call methods
				UpdateOnUnFundTransmittal(parameterUniqueId,DBTransaction,db)	;				
				InsertTransmittalLog(parameterUniqueId,parameterAmount,parameterTransmittalNo,"UNFUND",db,DBTransaction);
				ServiceTimeAndVestingDAClass.RevertServiceAndVesting(parameterUniqueId,db,DBTransaction);  
				DBTransaction.Commit();
				bool_TransactionStarted=false;
				DBconnectYRS.Close();
			
			}
			
			catch
			{
				if (bool_TransactionStarted)
				{
					DBTransaction.Rollback();
					DBconnectYRS.Close();
				}
				throw;
			}
		}
		
		/// <summary>
		/// Created By : Paramesh K.
		/// Created On : Sept 19th 2008
		/// This method will check any UEIN Transmittal Exists
		/// </summary>
		/// <param name="transmittalUniqueId">transmittal ID</param>
		/// <returns> 
		///		1 -- Cannot UnFund the Transmittal, as it has a associated UEIN transmittal 
		///			 which was generated for multiple Transmittals
		///		2 -- Cannot UnFund the Transmittal, as it has a associated UEIN which is funded.
		///		3 -- Success, transmittal can be unfunded
		/// </returns>
		public static int CheckUEINTransmittalExists(string transmittalUniqueId)
		{
			DbCommand l_DBCommandWrapper;
			Database db = null;
			int l_int_output = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_output;
				l_DBCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_UT_CheckUEINTransmittal");
				
				l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return l_int_output;
				
				db.AddInParameter(l_DBCommandWrapper,"@varchar_guiTransmittalId",DbType.String,transmittalUniqueId);
				db.AddOutParameter(l_DBCommandWrapper,"@int_output",DbType.Int32,2);
				db.ExecuteNonQuery(l_DBCommandWrapper);
				l_int_output = Convert.ToInt32(db.GetParameterValue(l_DBCommandWrapper,"@int_output"));
				return l_int_output;
			}
			catch 
			{
				throw ;
			}
		}

		//		public static void DeleteYmcaAppliedReceipts(string parameterUniqueId, Database parameterDatabase,DbTransaction parameterTransaction)
		//		{
		//				
		//			DbCommand DeleteYmcaReceipts = null;
		//			
		//			try 
		//			{ 
		//								
		//				DeleteYmcaReceipts = parameterDatabase.GetStoredProcCommand("yrs_usp_UT_DeleteYmcaAppliedReceipts");
		//				DeleteYmcaReceipts.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
		//				if(DeleteYmcaReceipts!=null)
		//				{
		//					DeleteYmcaReceipts.AddInParameter("@varchar_TransmittalId",DbType.String,parameterUniqueId);
		//					
		//					parameterDatabase.ExecuteNonQuery(DeleteYmcaReceipts,parameterTransaction);
		//				}
		//			}
		//			catch
		//			{
		//				throw;
		//			}
		//		}
		
		//		public static void RestoreReceiptRecord(string parameterUniqueId, Database parameterDatabase,DbTransaction parameterTransaction)
		//		{
		//				
		//			DbCommand RestoreYmcaReceipt = null;
		//			
		//			try 
		//			{ 
		//								
		//				RestoreYmcaReceipt = parameterDatabase.GetStoredProcCommand("yrs_usp_UT_RestoreYmcaAppliedReceipts");
		//				RestoreYmcaReceipt.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
		//				if(RestoreYmcaReceipt!=null)
		//				{
		//					RestoreYmcaReceipt.AddInParameter("@varchar_TransmittalId",DbType.String,parameterUniqueId);
		//					
		//					parameterDatabase.ExecuteNonQuery(RestoreYmcaReceipt,parameterTransaction);
		//				}
		//			}
		//			catch
		//			{
		//				throw;
		//			}
		//		}
		//		public static void UpdateTransmittalRecord(string parameterUniqueId, Database parameterDatabase,DbTransaction parameterTransaction)
		//		{
		//				
		//			DbCommand UpdateYmcaTransmittal = null;
		//			
		//			try 
		//			{ 
		//								
		//				UpdateYmcaTransmittal = parameterDatabase.GetStoredProcCommand("yrs_usp_UT_UpdateYmcaTransmittals");
		//				UpdateYmcaTransmittal.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
		//				if(UpdateYmcaTransmittal!=null)
		//				{
		//					UpdateYmcaTransmittal.AddInParameter("@varchar_UniqueId",DbType.String,parameterUniqueId);
		//					
		//					parameterDatabase.ExecuteNonQuery(UpdateYmcaTransmittal,parameterTransaction);
		//				}
		//			}
		//			catch
		//			{
		//				throw;
		//			}
		//		}
		public static void InsertTransmittalLog(string parameterUniqueId,double parameterAmount,string parameterTransmittalNo,string parameterAction, Database db,DbTransaction pDBTransaction)
		{
				
			DbCommand InsertDBCommandWrapper = null;
			
			try 
			{ 
								
				InsertDBCommandWrapper = db.GetStoredProcCommand("yrs_usp_UT_InsertTransmittalAdjustmentLog");
				InsertDBCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if(InsertDBCommandWrapper!=null)
				{
					db.AddInParameter(InsertDBCommandWrapper,"@varchar_TransmittalId",DbType.String,parameterUniqueId);
					db.AddInParameter(InsertDBCommandWrapper,"@varchar_chvTransmittalNo",DbType.String,parameterTransmittalNo);
					db.AddInParameter(InsertDBCommandWrapper,"@numeric_Amount",DbType.Double,parameterAmount);
					db.AddInParameter(InsertDBCommandWrapper,"@varchar_chvAction",DbType.String,parameterAction);
					db.ExecuteNonQuery(InsertDBCommandWrapper,pDBTransaction);
				}
			}
			catch
			{
				throw;
			}
		}
		// this function called on unfunding a transmittal (By Swopna 26Jun,2008 )
		public static void UpdateOnUnFundTransmittal(string GuiTransmittalId,DbTransaction pDBTransaction, Database db)
		{
			
			DbCommand updateCommandWrapper = null;
			
			try
			{
																										
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UT_UpdateOnUnFundTransmittals");
				
				db.AddInParameter(updateCommandWrapper,"@varchar_TransmittalId",DbType.String,GuiTransmittalId);				
				updateCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["LargeConnectionTimeOut"]) ; 
										
				db.ExecuteNonQuery(updateCommandWrapper,pDBTransaction);
		
			}
			catch
			{
				throw;
			}
		}
		//		public static void RestoreCreditRecord(string parameterUniqueId, Database parameterDatabase,DbTransaction parameterTransaction)
		//		{
		//				
		//			DbCommand RestoreYmcaReceipt = null;
		//			
		//			try 
		//			{ 
		//								
		//				RestoreYmcaReceipt = parameterDatabase.GetStoredProcCommand("yrs_usp_UT_RestoreYmcaCredits");
		//				RestoreYmcaReceipt.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
		//				if(RestoreYmcaReceipt!=null)
		//				{
		//					RestoreYmcaReceipt.AddInParameter("@varchar_UniqueId",DbType.String,parameterUniqueId);
		//					
		//					parameterDatabase.ExecuteNonQuery(RestoreYmcaReceipt,parameterTransaction);
		//				}
		//			}
		//			catch
		//			{
		//				throw;
		//			}
		//		}
		
		public static DataSet ValidateTransmittal(string parameterUniqueId)
		{
			DataSet dsValidateTransmittal= null;
			Database db= null;
			DbCommand commandLookUpYmca = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_UT_ValidateTransmittals");
				if (commandLookUpYmca==null) return null;
				dsValidateTransmittal = new DataSet();
				commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.AddInParameter(commandLookUpYmca,"@varchar_guiTransmittalId",DbType.String,parameterUniqueId);
				db.LoadDataSet(commandLookUpYmca,dsValidateTransmittal,"ValidateTransmittal");
				return dsValidateTransmittal;
			}
			catch 
			{
				throw ;
			}
			finally
			{
				commandLookUpYmca.Dispose();
				commandLookUpYmca=null;
				db = null;
			}


		}
		//		public static void DeleteYmcaTransmittals(string parameterUniqueId, string parameterTransmittalNo, double parameterAmount)
		//		{
		//			Database db= null;	
		//			DbCommand DeleteDBCommandWrapper = null;
		//			
		//			try 
		//			{ 
		//				db= DatabaseFactory.CreateDatabase("YRS");				
		//				DeleteDBCommandWrapper = db.GetStoredProcCommand("yrs_usp_UT_DeleteYmcaTransmittals");
		//				DeleteDBCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
		//				if(DeleteDBCommandWrapper!=null)
		//				{
		//					DeleteDBCommandWrapper.AddInParameter("@varchar_guiTransmittalId",DbType.String,parameterUniqueId);
		//					DeleteDBCommandWrapper.AddInParameter("@varchar_chvTransmittalNo",DbType.String,parameterTransmittalNo);
		//					DeleteDBCommandWrapper.AddInParameter("@numeric_mnyAmount",DbType.Double,parameterAmount);
		//					db.ExecuteNonQuery(DeleteDBCommandWrapper);
		//				}
		//			}
		//			catch
		//			{
		//				throw;
		//			}
		//		}
		public static void DeleteYmcaTransmittals(DataTable parameterDeleteTransmittals)
		{
			Database parameterDatabase = null;
			DbCommand insertCommandWrapper = null;
			
			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection = null;
			
			try
			{
				
				
				parameterDatabase  = DatabaseFactory.CreateDatabase("YRS");

				if (parameterDatabase == null) return ; 
				l_IDbConnection =parameterDatabase.CreateConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return;
				
				l_IDbTransaction = l_IDbConnection.BeginTransaction();

	
				
				foreach(DataRow drow in parameterDeleteTransmittals.Rows)
				{
					insertCommandWrapper = null;
					insertCommandWrapper=parameterDatabase.GetStoredProcCommand("yrs_usp_UT_DeleteYmcaTransmittals");

					parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_guiTransmittalId",DbType.String,drow["UniqueId"]);
					parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_chvTransmittalNo",DbType.String,drow["TransmittalNo"]);
					parameterDatabase.AddInParameter(insertCommandWrapper,"@numeric_mnyAmount",DbType.Double,drow["AmtDue"]);
					
					parameterDatabase.ExecuteNonQuery(insertCommandWrapper,l_IDbTransaction);	
				}
				
				l_IDbTransaction.Commit();	
				
			}
			catch(SqlException SqlEx)
			{  
				l_IDbTransaction.Rollback();
				throw SqlEx;
			}
			catch (Exception ex)
			{
				l_IDbTransaction.Rollback();
				throw ex;
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
		
	}
}
