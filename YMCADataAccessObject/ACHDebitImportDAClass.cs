//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCa-YRS
// FileName			:	ACHDebitImportDAClass.cs
// Author Name		:	Ashish Srivastava
// Employee ID		:	51821
// Email			:	ashish.srivastava@3i-infotech.com
// Contact No		:	8609
// Creation Time	:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified By       Date            Desription
//********************************************************************************************************************************
//Ashish Srivastava	12-Jan-2009		Remove comma seperated Ymca parameter from function GetAchDebitMatchedTransmittals
//Ashish Srivastava 29-Jan-2009		Added logic for loan defaulted emails
//Manthan Rajguru   2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************************************************************

#region Using Namespace
using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data.Common;
#endregion

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for ACHDebitImportDAClass.
	/// </summary>
	public class ACHDebitImportDAClass
	{
		
		public ACHDebitImportDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region "Public Methods"
		/// <summary>
		/// This function Validate Batch ID
		/// </summary>
		/// <param name="parameterBatchId"></param>
		/// <returns>bool</returns>
		public int ValidateBatchID(string parameterBatchId)
		{
			DbCommand l_DBCommandWrapper=null;
			Database db = null;
			int l_int_count = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_count;
				l_DBCommandWrapper = db.GetStoredProcCommand ("yrs_usp_ACH_ValidateBatchId");
				
				l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return l_int_count;
				

				db.AddInParameter(l_DBCommandWrapper,"@varchar_batchid",DbType.String,parameterBatchId);
				db.AddOutParameter(l_DBCommandWrapper,"@int_count",DbType.Int32,2);
				db.ExecuteNonQuery(l_DBCommandWrapper);
				l_int_count = Convert.ToInt32(db.GetParameterValue(l_DBCommandWrapper,"@int_count"));
				
				return l_int_count;
				
			}
			
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				db=null;

			}
		
		}
		
		/// <summary>
		/// Get Total ACHDebit amount for batch ID
		/// </summary>
		/// <param name="parameterBatchId"></param>
		/// <returns>decimal</returns>
		public decimal GetACHAmount(string parameterBatchId)
		{
			decimal l_ACHAmount=0;

			
			DbCommand l_DBCommandWrapper;
			Database db = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db!=null)
				{
					l_DBCommandWrapper = db.GetStoredProcCommand ("yrs_usp_ACH_ValidateStatus");
				
					l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				
					db.AddInParameter(l_DBCommandWrapper,"@varchar_batchid",DbType.String,parameterBatchId);
				
					object l_object=db.ExecuteScalar(l_DBCommandWrapper);
					if(l_object !=null)
					{
						if (l_object.GetType().ToString() !="System.DBNull"  )
						{
							l_ACHAmount=Convert.ToDecimal(l_object); 
						}
					}
				}

				return l_ACHAmount;
				

			}
			
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				db=null;
			}
		}

/*
		public DataSet GetYMCANameandNos(string parameterYmcaNos, string parameterBatchId)
		{
			DataSet dsList = null;
			Database db = null;
			

			DbCommand CommandGetList = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandGetList = db.GetStoredProcCommand("yrs_usp_ACH_Import_GetYmcaNameandId");
				if (CommandGetList ==null) return null;
				dsList = new DataSet();
				
				CommandGetList.AddInParameter("@varchar_YmcaNos",DbType.String,parameterYmcaNos);
				CommandGetList.AddInParameter("@varchar_BatchId",DbType.String,parameterBatchId);
				
				db.LoadDataSet(CommandGetList,dsList,"List");

				return dsList;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
*/

		/// <summary>
		/// Get YMCA No wise Amount and data
		/// </summary>
		/// <param name="parameterBatchId"></param>
		/// <returns>DataSet</returns>
		public DataSet GetACHDebitDetails(string parameterBatchId)
		{
			
			DataSet dsList = null;
			Database db = null;			
			

			DbCommand CommandGetList = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandGetList = db.GetStoredProcCommand("yrs_usp_ACHImport_GetACHDebitDetails");
				if (CommandGetList ==null) return null;
				dsList = new DataSet();
				
				
				db.AddInParameter(CommandGetList,"@varchar_BatchId",DbType.String,parameterBatchId);
				
				db.LoadDataSet(CommandGetList,dsList,"List");

				return dsList;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Save Records into DB
		/// </summary>
		/// <param name="parameterStatus"></param>
		public  void InsertAndUpdateAtsYmcaRcpts(DataSet parameterStatus)
		{   
			Database db = null;
			DbCommand insertCommandWrapper = null;			
			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection = null;
			
			try
			{
				
				
				db  = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return ; 
				l_IDbConnection =db.CreateConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return;
				//l_IDbTransaction = l_IDbConnection.BeginTransaction(IsolationLevel.Serializable);
				l_IDbTransaction = l_IDbConnection.BeginTransaction();

				
				
				foreach(DataRow l_drow_YmcaAchDebit in parameterStatus.Tables[0].Rows)
				{
					insertCommandWrapper = null;
					insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_ACH_Import_InsertSeggregatedRecpts");
					insertCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
					db.AddInParameter(insertCommandWrapper,"@varchar_BatchId",DbType.String,l_drow_YmcaAchDebit["REFNO"]);	
					db.AddInParameter(insertCommandWrapper,"@varchar_YmcaNo",DbType.String,l_drow_YmcaAchDebit["YMCANO"]);	
					db.AddInParameter(insertCommandWrapper,"@varchar_YmcaId",DbType.String,l_drow_YmcaAchDebit["UNIQUEID"]);
					db.AddInParameter(insertCommandWrapper,"@varchar_chvRcptSourceCode",DbType.String,l_drow_YmcaAchDebit["SOURCE"]);
					//insertCommandWrapper.AddInParameter("@numeric_mnyAmount",DbType.Double,l_drow_YmcaAchDebit["AMOUNT"]);
					db.AddInParameter(insertCommandWrapper,"@varchar_dtsRecievedAcctDate",DbType.Date,l_drow_YmcaAchDebit["PaymentDate"]);				
					db.AddInParameter(insertCommandWrapper,"@varchar_dtsReceivedDate",DbType.Date,l_drow_YmcaAchDebit["Recdate"]);
					db.AddInParameter(insertCommandWrapper,"@varchar_chvReceiptID",DbType.String,l_drow_YmcaAchDebit["REFNO"]);
					db.AddInParameter(insertCommandWrapper,"@bit_selected",DbType.Boolean,l_drow_YmcaAchDebit["Selected"]);
			
					db.ExecuteNonQuery(insertCommandWrapper,l_IDbTransaction);
					
					//db.ExecuteNonQuery(insertCommandWrapper);
						
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
				l_IDbConnection=null;
				}
				db=null;
				l_IDbTransaction=null;
			}
		}


		/// <summary>
		/// This function get Matched transmittals
		/// </summary>
		
		/// <param name="strBatchId"></param>
		/// <returns>DataSet</returns>
		public DataSet GetAchDebitMatchedTransmittals(string parameterBatchId)
		{
			
			DataSet dsList = null;
			Database db = null;			
			

			DbCommand CommandGetList = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandGetList = db.GetStoredProcCommand("yrs_usp_ACHImport_GetACHDebitTransmittalDetails");
				if (CommandGetList ==null) return null;
				dsList = new DataSet();				
				
				db.AddInParameter(CommandGetList,"@var_BatchId",DbType.String,parameterBatchId);
				
				db.LoadDataSet(CommandGetList,dsList,"MatchedTransmittals");

				return dsList;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public DataSet SavePostAndApplyReceipts(DataSet parameterDataSetACHDebit,DataSet parameterDataSetMatchedTransmittal,DateTime parameterFundedDate,string paraBatchID,DataTable paraDataTableFundedTransmittallog,ref Int64 fundedTransmittallogId )
		{
			Database l_Database = null;
			DbCommand dbCommandWrapper = null;			
			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection = null;
			//bool l_bool_flag=true;
			//DataTable l_dtPersonalDetails=null;
			DataSet l_dsPersonalDetails=null;
			UEINDAClass objUEINDAClass=null;
			string l_string_Output=string.Empty ;
			try
			{
				l_Database  = DatabaseFactory.CreateDatabase("YRS");
				l_IDbConnection =l_Database.CreateConnection();
				l_IDbConnection.Open();
				l_IDbTransaction = l_IDbConnection.BeginTransaction();
				SetAchStatusOfUnSelectYmca(parameterDataSetACHDebit,l_Database,l_IDbTransaction);
				
				System.Diagnostics.Debug.WriteLine("{0}:Start Matched transmittals Loop", DateTime.Now.ToString());
				foreach(DataRow dtRowMatchedTransmittal in parameterDataSetMatchedTransmittal.Tables["MatchedTransmittals"].Rows )
				{
					if(dtRowMatchedTransmittal["ValidRecord"].ToString() =="1")
					{
						dbCommandWrapper = null;
						dbCommandWrapper=l_Database.GetStoredProcCommand("yrs_usp_ACHImport_SavePostAndAppliedReceipts");
						dbCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;
                        l_Database.AddInParameter(dbCommandWrapper, "@varchar_ACHUniqueId", DbType.String, dtRowMatchedTransmittal["AchUniqueId"]);
                        l_Database.AddInParameter(dbCommandWrapper, "@varchar_TransmittalUniqueId", DbType.String, dtRowMatchedTransmittal["guiTransmittalId"]);
                        l_Database.AddInParameter(dbCommandWrapper, "@varchar_YmcaUniqueId", DbType.String, dtRowMatchedTransmittal["YmcaUniqueId"]);
                        l_Database.AddInParameter(dbCommandWrapper, "@numeric_ReceiptAmount", DbType.Double, dtRowMatchedTransmittal["ReceiptAmount"]);
                        l_Database.AddInParameter(dbCommandWrapper, "@bit_Selected", DbType.Boolean, dtRowMatchedTransmittal["Selected"]);
                        l_Database.AddInParameter(dbCommandWrapper, "@varchar_BatchId", DbType.String, dtRowMatchedTransmittal["BatchId"]);
                        l_Database.AddInParameter(dbCommandWrapper, "@datetime_FundedDate", DbType.DateTime, parameterFundedDate);
					
						l_Database.ExecuteNonQuery( dbCommandWrapper,l_IDbTransaction);
						//Commented by Ashish on 13-Jan-2009 
//						l_dsPersonalDetails=CashApplicationDaClass.SelectPersonDetails(dtRowMatchedTransmittal["guiTransmittalId"].ToString() , l_Database,l_IDbTransaction);
//						if (l_bool_flag)
//						{
//							l_dtPersonalDetails=l_dsPersonalDetails.Tables[0].Clone();
//							l_bool_flag=false;
//  
//						}
//						if (l_dsPersonalDetails.Tables[0].Rows.Count>0)
//						{
//							l_dtPersonalDetails.ImportRow( l_dsPersonalDetails.Tables[0].Rows[0]);
//							l_dtPersonalDetails.AcceptChanges(); 
//						}
					}
 
				}

//				System.Diagnostics.Debug.WriteLine("{0}:End Matched transmittals Loop", DateTime.Now.ToString() );

//				if(parameterDataTableNewTransaction.Rows.Count > 0 &&  parameterDataTableNewTransmittal.Rows.Count >0)
//				{
//						UEINDAClass ueinDAClass=new UEINDAClass();
//					ueinDAClass.SaveNewTransactRecords(parameterDataTableNewTransaction,db,l_IDbTransaction); 
//					ueinDAClass.SaveNewTransmittalRecords( parameterDataTableNewTransmittal,db, l_IDbTransaction);
// 
//				}
				objUEINDAClass=new UEINDAClass(); 
				//Call logging routine for funded transmittal 
				if(paraDataTableFundedTransmittallog!=null)
				{
					//System.Diagnostics.Debug.WriteLine("{0}:Save Fundinglog Start", DateTime.Now.ToString());
					fundedTransmittallogId=objUEINDAClass.SaveTransmittalFunding(paraBatchID,"ACH Debit",paraDataTableFundedTransmittallog,"ACHDebit",l_Database,l_IDbTransaction);
					//System.Diagnostics.Debug.WriteLine("{0}:Save Fundinglog End", DateTime.Now.ToString());
					//Call UEIN Routine
					if(fundedTransmittallogId !=0)
					{
						//System.Diagnostics.Debug.WriteLine("{0}:UEIN Calculation Start", DateTime.Now.ToString());
						l_string_Output=objUEINDAClass.GenerateUEIN(fundedTransmittallogId,parameterFundedDate.Date,l_Database,l_IDbTransaction); 
						//System.Diagnostics.Debug.WriteLine("{0}:UEIN Calculation End", DateTime.Now.ToString());
						if (l_string_Output.ToString()  != string.Empty ) 
						{ 
							throw new Exception(l_string_Output);
						} 
						//Get personal details
						l_dsPersonalDetails=CashApplicationDaClass.SelectPersonDetails(fundedTransmittallogId , l_Database,l_IDbTransaction);
//						if(l_dsPersonalDetails!=null)
//						{
//							if(l_dsPersonalDetails.Tables.Count >0)
//							{
//								l_dtPersonalDetails=l_dsPersonalDetails.Tables["PersonDetails"];
//							}
//						}
					}
				}
////System.Diagnostics.Debug.WriteLine("{0}:Service Update Start", DateTime.Now.ToString());
//				dbCommandWrapper=l_Database.GetStoredProcCommand("yrs_usp_ACH_CASH_UpdateServiceAndVesting");
//				dbCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
//
//				dbCommandWrapper.AddInParameter("@numeric_FundedTransmittalLogID",DbType.Int64 ,fundedTransmittallogId);
//				dbCommandWrapper.AddOutParameter ("@bit_Success",DbType.Boolean,1 );
//				l_Database.ExecuteNonQuery(dbCommandWrapper,l_IDbTransaction);
//				//l_success=Convert.ToBoolean(dbCommandWrapper.GetParameterValue("@bit_Success")); 
////System.Diagnostics.Debug.WriteLine("{0}:Service Update End", DateTime.Now.ToString());
				l_IDbTransaction.Commit() ;	
				return l_dsPersonalDetails;
			}
			catch(SqlException SqlEx)
			{  
				l_IDbTransaction.Rollback();
				throw SqlEx;
			}
			catch(Exception ex )
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
					l_IDbConnection=null;
					
				}
				l_Database=null;
				l_IDbTransaction=null;
			}
		}
		//Commented by Ashish on 01-Oct-2008
/*
		public Int64 SaveTransmittalFunding(string parameterBatchID,string parameterDescription,DataTable parameterDataTableFundedTransmittal,string parameterModuleType,Database parameterDatabase,DbTransaction parameterTransaction )
		{
			
//			Database db = null;
			DbCommand dbCommandWrapper = null;			
//			DbTransaction l_IDbTransaction = null;
//			DbConnection l_IDbConnection = null;
			Int64 l_logID=0;
			try
			{
				
//				db  = DatabaseFactory.CreateDatabase("YRS");
//				l_IDbConnection =db.CreateConnection();
//				l_IDbConnection.Open();
//				l_IDbTransaction = l_IDbConnection.BeginTransaction();
				dbCommandWrapper=parameterDatabase.GetStoredProcCommand("yrs_usp_ACHImport_SaveTransmittalFunding");
				dbCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				if(parameterBatchID!=null)
				{
					dbCommandWrapper.AddInParameter("@varchar_BatchId",DbType.String, parameterBatchID);
				}
				dbCommandWrapper.AddInParameter("@varchar_Description",DbType.String, parameterDescription);
				dbCommandWrapper.AddOutParameter ("@numeric_ID",DbType.Int64,16 );
				parameterDatabase.ExecuteNonQuery(dbCommandWrapper,parameterTransaction);
				l_logID=Convert.ToInt64(dbCommandWrapper.GetParameterValue("@numeric_ID")); 

				if(l_logID!=0)
				{
					SaveTransmittalFundingDetails(l_logID,parameterDataTableFundedTransmittal,parameterModuleType,parameterDatabase,parameterTransaction);
					//l_IDbTransaction.Commit();
				
				}
				 

			return l_logID;

			}
			catch(SqlException SqlEx)
			{  
				//l_IDbTransaction.Rollback();
				throw SqlEx;
			}
			catch(Exception ex )
			{
				//l_IDbTransaction.Rollback();
				throw ex;
			}
			finally
			{
//				if (l_IDbConnection != null)
//				{
//					if (l_IDbConnection.State != ConnectionState.Closed)
//					{
//						l_IDbConnection.Close ();
//					}
//					l_IDbConnection=null;
//					
//				}
				//db=null;
				//l_IDbTransaction=null;
			}
		}


*/
		
		#endregion

		#region "Private Methods"
		private void SetAchStatusOfUnSelectYmca(DataSet parameterDataSetACHDebit,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			
			DbCommand commandUpdate = null;
			try
			{
				if(parameterDataSetACHDebit!=null)
				{
					if(parameterDataSetACHDebit.Tables[0].Rows.Count>0)
					{
						DataRow [] dtRows= parameterDataSetACHDebit.Tables[0].Select("Selected='0'");
						if(dtRows.Length>0)
						{
							foreach(DataRow dtRow in dtRows)
							{
								commandUpdate = null;
								commandUpdate=parameterDatabase.GetStoredProcCommand("yrs_usp_ACHImport_SetStatusOfUnSelectYmca ");
								commandUpdate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;
                                parameterDatabase.AddInParameter(commandUpdate, "@varchar_YmcaUniqueId", DbType.String, dtRow["UNIQUEID"].ToString());
                                parameterDatabase.AddInParameter(commandUpdate, "@varchar_BatchId", DbType.String, dtRow["REFNO"].ToString());
                                parameterDatabase.AddInParameter(commandUpdate, "@dateTime_PaymentDate", DbType.DateTime, dtRow["PaymentDate"].ToString());
								parameterDatabase.ExecuteNonQuery(commandUpdate,parameterTransaction); 

							}
						}
					}
				}
 
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				commandUpdate=null;
			}
		}
		//Commented by Ashish on 01-Oct-2008
/*		private void SaveTransmittalFundingDetails(Int64 parameterID,DataTable parameterDataTableFundedTransmittal,string parameterModuleType,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			DbCommand commandInsert = null;
			try
			{
				if(parameterID>0 && parameterDataTableFundedTransmittal.Rows.Count >0 )
				{
						foreach(DataRow dtRow in parameterDataTableFundedTransmittal.Rows)
						{
							commandInsert = null;
							commandInsert=parameterDatabase.GetStoredProcCommand("yrs_usp_ACHImport_SaveTransmittalFundingDetails");
							commandInsert.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
							commandInsert.AddInParameter("@numeric_ID",DbType.Int64 ,parameterID );
							commandInsert.AddInParameter("@varchar_TransmittalID",DbType.String,dtRow["guiTransmittalID"].ToString() );
							if(!dtRow["guiRcptId"].ToString().Equals(string.Empty))
							{
								commandInsert.AddInParameter("@varchar_RecptID",DbType.String ,dtRow["guiRcptId"].ToString() );
							}
							if(!dtRow["guiUeinTransmittalID"].ToString().Equals(string.Empty))
							{
								commandInsert.AddInParameter("@varchar_guiUeinTransmittalID",DbType.String,dtRow["guiUeinTransmittalID"].ToString() );
							}
							commandInsert.AddInParameter("@varchar_ModuleType",DbType.String,parameterModuleType );
							parameterDatabase.ExecuteNonQuery(commandInsert,parameterTransaction); 

						}
					
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		*/
		#endregion
	}
}
