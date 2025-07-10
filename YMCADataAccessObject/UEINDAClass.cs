//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCa-YRS
// FileName			:	CashApplicationBOClass.cs
// Author Name		:	
// Employee ID		:	
// Email				:	
// Contact No		:	
// Creation Time		:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modified By			 Date            Desription
//*******************************************************************************
//Shashank Patel		19-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
//Manthan Rajguru       2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Bala                  2016.01.27       YRS-AT-2594 -  YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
//Pramod P. Pokale      2016.01.29       YRS-AT-2594 -  YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
//Anudeep A             2016.03.21       YRS-AT-2594 - YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
//*******************************************************************************
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
	/// Summary description for UEINDAClass.
	/// </summary>
	public class UEINDAClass
	{
		public UEINDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region Public Methods

		/// <summary>
		/// This Methods Gets Transact Records for Perticular Transmittal ID
		/// </summary>
		/// <param name="parameterTransmittalId"></param>
		/// <returns></returns>
		public DataSet GetTransactsRecords(string parameterTransmittalId)
		{
			DataSet dsTrasnsacts=null;
			Database db = null;
			DbCommand dbCommand = null;			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				dbCommand= db.GetStoredProcCommand("yrs_usp_UEIN_GetTransactionsDetails");
				dsTrasnsacts= new DataSet();
				db.AddInParameter(dbCommand,"@varchar_guiTransmittalId",DbType.String,parameterTransmittalId);
				
				db.LoadDataSet(dbCommand,dsTrasnsacts,"Transacts");

				return dsTrasnsacts;
			}
			catch(Exception ex)
			{
				throw ex;

			}
			finally
			{
				
                dbCommand=null;
				db=null;
			}
		}
/*

		/// <summary>
		/// This Method Save Newly Generated Transaction Records
		/// </summary>
		/// <param name="parameterDataTableNewTransact"></param>
		/// <param name="parameterDatabase"></param>
		/// <param name="parameterIDbTransaction"></param>
		public void SaveNewTransactRecords(DataTable parameterDataTableNewTransact,Database parameterDatabase,DbTransaction parameterIDbTransaction)
		{
			DbCommand dbCommandWrapper=null;
			try
			{
				foreach(DataRow dtTransactRow in parameterDataTableNewTransact.Rows)
				{
					dbCommandWrapper = null;
					dbCommandWrapper=parameterDatabase.GetStoredProcCommand("yrs_usp_UEIN_SaveTransactions");
					dbCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

					dbCommandWrapper.AddInParameter("@varchar_guiPersID",DbType.String, dtTransactRow["guiPersID"]);
					dbCommandWrapper.AddInParameter("@varchar_guiFundEventID",DbType.String, dtTransactRow["guiFundEventID"]);
					dbCommandWrapper.AddInParameter("@varchar_guiYmcaID",DbType.String, dtTransactRow["guiYmcaID"]);
					dbCommandWrapper.AddInParameter("@varchar_guiTransmittalID",DbType.String, dtTransactRow["guiTransmittalID"]);						
					dbCommandWrapper.AddInParameter("@varchar_AcctType",DbType.String, dtTransactRow["AcctType"]);	
					dbCommandWrapper.AddInParameter("@varchar_TransactType",DbType.String, dtTransactRow["TransactType"]);
					dbCommandWrapper.AddInParameter("@varchar_AnnuityBasisType",DbType.String, dtTransactRow["AnnuityBasisType"]);				
					dbCommandWrapper.AddInParameter("@decimal_PersonalPreTax",DbType.Double, dtTransactRow["PersonalPreTax"]);
					dbCommandWrapper.AddInParameter("@decimal_YmcaPreTax",DbType.Double, dtTransactRow["YmcaPreTax"]);
					dbCommandWrapper.AddInParameter("@datetime_dtsReceivedDate",DbType.DateTime, dtTransactRow["dtsReceivedDate"]);					
					dbCommandWrapper.AddInParameter("@datetime_dtsTransactDate",DbType.DateTime, dtTransactRow["dtsTransactDate"]);
					dbCommandWrapper.AddInParameter("@datetime_dtsAccountingDate",DbType.DateTime, dtTransactRow["dtsAccountingDate"]);

					parameterDatabase.ExecuteNonQuery(dbCommandWrapper,parameterIDbTransaction);   

				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>
		/// This Method Save Newly Generated Transmittal records 
		/// </summary>
		/// <param name="parameterDataTableNewTransmittal"></param>
		/// <param name="parameterDatabase"></param>
		/// <param name="parameterIDbTransaction"></param>
		public void SaveNewTransmittalRecords(DataTable parameterDataTableNewTransmittal,Database parameterDatabase,DbTransaction parameterIDbTransaction)
		{
			DbCommand dbCommandWrapper=null;
			try
			{
				foreach(DataRow dtTransmittalRow in parameterDataTableNewTransmittal.Rows)
				{
					dbCommandWrapper = null;
					dbCommandWrapper=parameterDatabase.GetStoredProcCommand("yrs_usp_UEIN_SaveTransmittal");
					dbCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
					
					dbCommandWrapper.AddInParameter("@varchar_guiTransmittalID",DbType.String, dtTransmittalRow["guiTransmittalID"]);
					dbCommandWrapper.AddInParameter("@varchar_guiYmcaID",DbType.String, dtTransmittalRow["guiYmcaID"]);	
					dbCommandWrapper.AddInParameter("@varchar_chvTransmittalSourceCode",DbType.String, dtTransmittalRow["chvTransmittalSourceCode"]);
					dbCommandWrapper.AddInParameter("@varchar_chvTransmittalNo",DbType.String, dtTransmittalRow["chvTransmittalNo"]);
					dbCommandWrapper.AddInParameter("@decimal_mnyAmtDue",DbType.Double, dtTransmittalRow["mnyAmtDue"]);								
					dbCommandWrapper.AddInParameter("@datetime_dtsTransmittalDate",DbType.DateTime, dtTransmittalRow["dtsTransmittalDate"]);
					dbCommandWrapper.AddInParameter("@datetime_dtsAccountingDate",DbType.DateTime, dtTransmittalRow["dtsAccountingDate"]);
									
					
					parameterDatabase.ExecuteNonQuery(dbCommandWrapper,parameterIDbTransaction);   

				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
*/
		public Int64 SaveTransmittalFunding(string parameterBatchID,string parameterDescription,DataTable parameterDataTableFundedTransmittal,string parameterModuleType,Database parameterDatabase,DbTransaction parameterTransaction )
		{
			
			DbCommand dbCommand = null;			
			Int64 l_logID=0;
			try
			{				
				dbCommand=parameterDatabase.GetStoredProcCommand("yrs_usp_ACHImport_SaveTransmittalFunding");
				dbCommand.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				if(parameterBatchID!=null)
				{
                    parameterDatabase.AddInParameter(dbCommand, "@varchar_BatchId", DbType.String, parameterBatchID);
				}
                parameterDatabase.AddInParameter(dbCommand, "@varchar_Description", DbType.String, parameterDescription);
                parameterDatabase.AddOutParameter(dbCommand, "@numeric_ID", DbType.Int64, 16);
				parameterDatabase.ExecuteNonQuery(dbCommand,parameterTransaction);
                l_logID = Convert.ToInt64(parameterDatabase.GetParameterValue(dbCommand, "@numeric_ID")); 

				if(l_logID!=0)
				{
					SaveTransmittalFundingDetails(l_logID,parameterDataTableFundedTransmittal,parameterModuleType,parameterDatabase,parameterTransaction);
					
				
				}
				else
				{
					throw new Exception("Unable to log into transmittal funding log table"); 
				
				}
				 

				return l_logID;

			}
			catch(SqlException SqlEx)
			{  
				
				throw SqlEx;
			}
			catch(Exception ex )
			{
				
				throw ex;
			}
			
		}
		public string  GenerateUEIN(Int64 parameterTransmittalFundedLogID,DateTime parameterFundedDate,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			DbCommand dbCommand = null;
			string l_string_OutPut;
			try
			{
				dbCommand=parameterDatabase.GetStoredProcCommand("yrs_usp_ACH_CASH_UnfudedDailyCompoundInterest");
				dbCommand.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;
                parameterDatabase.AddInParameter(dbCommand, "@TransmittalFundingDate", DbType.DateTime, parameterFundedDate.Date);
                parameterDatabase.AddInParameter(dbCommand, "@TransmittalFundingID", DbType.Int64, parameterTransmittalFundedLogID);
                parameterDatabase.AddOutParameter(dbCommand, "@cOutput", DbType.String, 1000);
				parameterDatabase.ExecuteNonQuery(dbCommand,parameterTransaction);
                l_string_OutPut = parameterDatabase.GetParameterValue(dbCommand, "@cOutput").ToString();
				return l_string_OutPut;

			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

        //START: Bala | 2016.01.27 | YRS-AT-2594 | Provides Unfunded UEIN transmittal details
        public DataSet GetUnfundedUEIN()
        {
            DataSet dsUEIN = null;
            Database db = null;
            DbCommand dbCommand = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                dbCommand = db.GetStoredProcCommand("yrs_usp_UT_GetUnfundedUEIN");
                dsUEIN = new DataSet();
                db.LoadDataSet(dbCommand, dsUEIN, "UnfundedUEIN");
                return dsUEIN;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand = null;
                db = null;
            }
        }
        //END: Bala | 2016.01.27 | YRS-AT-2594 | Provides Unfunded UEIN transmittal details

        //START: PPP | 2016.01.29 | YRS-AT-2594 | Sends email with selected UEIN transmittal to LPA's
        //End: AA:2016.03.21 :YRS-AT-2594 Commented below code as it will be not used
        //public bool SendEmailOfUnfundedUEINs(string strXMLOutput)
        //{
        //    //for maintaining transaction
        //    Database dbDatabase;
        //    DbCommand dbCommand;
        //    DbConnection dbConnection = null;
        //    bool bResult;
        //    try
        //    {
        //        dbDatabase = DatabaseFactory.CreateDatabase("YRS");
        //        dbConnection = dbDatabase.CreateConnection();
        //        dbConnection.Open();
        //        bResult = false;
        //        if (dbDatabase != null)
        //        {
        //            dbCommand = dbDatabase.GetStoredProcCommand("yrs_usp_UT_GenerateEmailsForUnfundedUEIN");
        //            dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
        //            dbDatabase.AddInParameter(dbCommand, "@XML_UEINs", DbType.String, strXMLOutput);
        //            dbDatabase.ExecuteNonQuery(dbCommand);
        //            bResult = true;
        //        }
        //        return bResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (dbConnection != null)
        //        {
        //            if (dbConnection.State != ConnectionState.Closed)
        //            {
        //                dbConnection.Close();
        //            }
        //        }
        //        dbConnection = null;
        //        dbDatabase = null;
        //    }
        //}
        //End: AA:2016.03.21 :YRS-AT-2594 Commented below code as it will be not used
        //END: PPP | 2016.01.29 | YRS-AT-2594 | Sends email with selected UEIN transmittal to LPA's
		#endregion

        #region Private Methods
		private void SaveTransmittalFundingDetails(Int64 parameterID,DataTable parameterDataTableFundedTransmittal,string parameterModuleType,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			DbCommand dbCommandInsert = null;
			try
			{
				if(parameterID > 0 && parameterDataTableFundedTransmittal.Rows.Count > 0 )
				{
					foreach(DataRow dtRow in parameterDataTableFundedTransmittal.Rows)
					{
						dbCommandInsert = null;
						dbCommandInsert = parameterDatabase.GetStoredProcCommand("yrs_usp_ACHImport_SaveTransmittalFundingDetails");
						dbCommandInsert.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;
                        parameterDatabase.AddInParameter(dbCommandInsert, "@numeric_ID", DbType.Int64, parameterID);
                        parameterDatabase.AddInParameter(dbCommandInsert, "@varchar_TransmittalID", DbType.String, dtRow["guiTransmittalID"].ToString());
						if(!dtRow["guiRcptId"].ToString().Equals(string.Empty))
						{
                            parameterDatabase.AddInParameter(dbCommandInsert, "@varchar_RecptID", DbType.String, dtRow["guiRcptId"].ToString());
						}
						if(!dtRow["guiUeinTransmittalID"].ToString().Equals(string.Empty))
						{
                            parameterDatabase.AddInParameter(dbCommandInsert, "@varchar_guiUeinTransmittalID", DbType.String, dtRow["guiUeinTransmittalID"].ToString());
						}
                        parameterDatabase.AddInParameter(dbCommandInsert, "@varchar_ModuleType", DbType.String, parameterModuleType);
						parameterDatabase.ExecuteNonQuery(dbCommandInsert,parameterTransaction); 

					}
					
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		//Shashank Patel		19-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
		#region Cash Application - Person
		public Int64 SaveTransmittalFunding(string parameterBatchID, string parameterDescription, DataRow parameterDataRowFundedTransmittal, DataTable parameterDataTableFundedTransaction, string parameterModuleType, Database parameterDatabase, DbTransaction parameterTransaction)
		{

			DbCommand dbCommand = null;
			Int64 ilogID = 0;
			try
			{
				dbCommand = parameterDatabase.GetStoredProcCommand("yrs_usp_ACHImport_SaveTransmittalFunding");
				dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				if (parameterBatchID != null)
				{
					parameterDatabase.AddInParameter(dbCommand, "@varchar_BatchId", DbType.String, parameterBatchID);
				}
				parameterDatabase.AddInParameter(dbCommand, "@varchar_Description", DbType.String, parameterDescription);
				parameterDatabase.AddOutParameter(dbCommand, "@numeric_ID", DbType.Int64, 16);
				parameterDatabase.ExecuteNonQuery(dbCommand, parameterTransaction);
				ilogID = Convert.ToInt64(parameterDatabase.GetParameterValue(dbCommand, "@numeric_ID"));

				if (ilogID != 0)
				{
					SaveTransmittalFundingDetails(ilogID, parameterDataRowFundedTransmittal, parameterDataTableFundedTransaction, parameterModuleType, parameterDatabase, parameterTransaction);


				}
				else
				{
					throw new Exception("Unable to log into transmittal funding log table");

				}


				return ilogID;

			}
			catch (SqlException SqlEx)
			{

				throw SqlEx;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		/// <summary>
		/// This method is used for saving the details of transmittalId into atsTransmittalFundingDetails table
		/// </summary>
		/// <param name="parameterID"></param>
		/// <param name="parameterDataRowFundedTransmittal"></param>
		/// <param name="parameterDataTableFundedTransaction"></param>
		/// <param name="parameterModuleType"></param>
		/// <param name="parameterDatabase"></param>
		/// <param name="parameterTransaction"></param>
		/// <returns>Returns the identity value of table atsTransmittalFundingDetails</returns>
		private Int64 SaveTransmittalFundingDetails(Int64 parameterID, DataRow parameterDataRowFundedTransmittal,DataTable parameterDataTableFundedTransaction, string parameterModuleType, Database parameterDatabase, DbTransaction parameterTransaction)
		{
			DbCommand dbCommandInsert = null;
			Int64 ilogID = 0;
			try
			{
				if (parameterID > 0 && parameterDataRowFundedTransmittal !=null)
				{
					
						dbCommandInsert = null;
						dbCommandInsert = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_SaveTransmittalFundingDetails");
						dbCommandInsert.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
						parameterDatabase.AddInParameter(dbCommandInsert, "@numeric_ID", DbType.Int64, parameterID);
						parameterDatabase.AddInParameter(dbCommandInsert, "@varchar_TransmittalID", DbType.String, parameterDataRowFundedTransmittal["guiTransmittalID"].ToString());
						
						if (!parameterDataRowFundedTransmittal["guiRcptId"].ToString().Equals(string.Empty))
						{
							parameterDatabase.AddInParameter(dbCommandInsert, "@varchar_RecptID", DbType.String, parameterDataRowFundedTransmittal["guiRcptId"].ToString());
						}
						
						if (!parameterDataRowFundedTransmittal["guiUeinTransmittalID"].ToString().Equals(string.Empty))
						{
							parameterDatabase.AddInParameter(dbCommandInsert, "@varchar_guiUeinTransmittalID", DbType.String, parameterDataRowFundedTransmittal["guiUeinTransmittalID"].ToString());
						}
						
						parameterDatabase.AddInParameter(dbCommandInsert, "@varchar_ModuleType", DbType.String, parameterModuleType);
						parameterDatabase.AddOutParameter(dbCommandInsert, "@output", DbType.Int64, 16);
						
						parameterDatabase.ExecuteNonQuery(dbCommandInsert, parameterTransaction);
						
						ilogID = Convert.ToInt64(parameterDatabase.GetParameterValue(dbCommandInsert, "@output"));
						
						if (ilogID != 0)
						{
							SaveTransmittalFundingPersonDetails(ilogID, parameterDataTableFundedTransaction, parameterDatabase, parameterTransaction);
						}
						else
						{
							throw new Exception("Unable to log into transmittal funding details log table");
						}

				}
				return ilogID;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// This method is used for saving the selected transaction(s) into atsTransmittalFundingPersonDetails table
		/// </summary>
		/// <param name="parameterID"></param>
		/// <param name="parameterDataTableFundedTransaction"></param>
		/// <param name="parameterDatabase"></param>
		/// <param name="parameterTransaction"></param>
		private void SaveTransmittalFundingPersonDetails(Int64 parameterID, DataTable parameterDataTableFundedTransaction, Database parameterDatabase, DbTransaction parameterTransaction)
		{
			DbCommand dbCommandInsert = null;
			try
			{
				if (parameterID > 0 && parameterDataTableFundedTransaction.Rows.Count > 0)
				{
					foreach (DataRow drrow in parameterDataTableFundedTransaction.Rows)
					{

						dbCommandInsert = null;
						dbCommandInsert = parameterDatabase.GetStoredProcCommand("YRS_USP_CA_InsertTransmittalPersonFundingDetails");
						dbCommandInsert.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
						parameterDatabase.AddInParameter(dbCommandInsert, "@intFundingDetailsId", DbType.Int64, parameterID);
						parameterDatabase.AddInParameter(dbCommandInsert, "@guiFundEventId", DbType.String, drrow["FundEventId"].ToString());
						parameterDatabase.AddInParameter(dbCommandInsert, "@guiTransactionId", DbType.String, drrow["UniqueId"].ToString());

						parameterDatabase.ExecuteNonQuery(dbCommandInsert, parameterTransaction);

					}

				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>
		/// This method is used to generate the UEIN transaction(s) for a person for a selected transmittal
		/// </summary>
		/// <param name="parameterIntTransmittalFundedLogID">idendity column of Atsfundingdetails table</param>
		/// <param name="parameterDateTimeFundedDate">Funded date</param>
		/// <param name="parameterDatabase"> Database object for maintain transaction</param>
		/// <param name="parameterDbTransaction">Transaction object</param>
		/// <returns>string</returns>
		public string GenerateUEINForPerson(Int64 parameterIntTransmittalFundedLogID, DateTime parameterDateTimeFundedDate, Database parameterDatabase, DbTransaction parameterDbTransaction)
		{
			DbCommand dbCommand = null;
			string strOutPut;
			try
			{
				dbCommand = parameterDatabase.GetStoredProcCommand("yrs_usp_ACH_CASH_UnfudedDailyCompoundInterestForPerson");
				dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				parameterDatabase.AddInParameter(dbCommand, "@TransmittalFundingDate", DbType.DateTime, parameterDateTimeFundedDate.Date);
				parameterDatabase.AddInParameter(dbCommand, "@TransmittalFundingID", DbType.Int64, parameterIntTransmittalFundedLogID);
				parameterDatabase.AddOutParameter(dbCommand, "@cOutput", DbType.String, 1000);
				parameterDatabase.ExecuteNonQuery(dbCommand, parameterDbTransaction);
				strOutPut = parameterDatabase.GetParameterValue(dbCommand, "@cOutput").ToString();
				return strOutPut;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		//Shashank Patel		19-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
	}


	
}
