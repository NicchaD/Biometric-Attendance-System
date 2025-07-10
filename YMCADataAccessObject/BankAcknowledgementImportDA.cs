//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCa-YRS
// FileName			:	BankAcknowledgementImportDA.cs
// Author Name		:	Santosh Bura
// Created on       :   11/20/2019
// Summary of Functionality: Includes all business logic for all actions performed from importing DAR and Reverse seed files from Bank
// Declared in Version: 20.7.0 | YRS-AT-4599 -  YRS enh: State Tax Withholding - New import screen for "First Annuity Payroll Processing" (DAR File)

//**************************************************************************************************************
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------
//**************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
    public class BankAcknowledgementImportDA
    {
        public BankAcknowledgementImportDA()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public static String PrepareImportBaseHeaderRecord(YMCAObjects.BankAcknowledgementHeaderImport DARImportFIleData) 
        {
            Database db;
            DbCommand dbCommand;
            string ImportHeaderID ;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_STW_Import_CreateHeaderRecords");

                if (dbCommand == null) return null;

                dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(dbCommand, "@VARCHAR_DARImportFIlePath", DbType.String, DARImportFIleData.DARFilePath);
                db.AddInParameter(dbCommand, "@VARCHAR_DARImportDisbursementType", DbType.String, DARImportFIleData.DisbursementType);
                db.AddInParameter(dbCommand, "@VARCHAR_DARImportSource", DbType.String, DARImportFIleData.Source);
                db.AddInParameter(dbCommand, "@VARCHAR_DARImportStatus", DbType.String, DARImportFIleData.Status);                
                db.AddOutParameter(dbCommand, "@INT_HeaderID", DbType.Int32, DARImportFIleData.UniqueID);

                db.ExecuteNonQuery(dbCommand);
                ImportHeaderID = Convert.ToString(db.GetParameterValue(dbCommand, "@INT_HeaderID"));
                return ImportHeaderID;

            }
            catch
            {
                throw;
            }
            
            finally
            {
                
                dbCommand = null;
                db = null;
            }
            
        }

        public static DataSet InsertImportBaseRecordsWithErrors(DataSet parameterDARFileData, int intImportBaseHeaderID)//, out string para_out_string_ExeceptionReason)
        {
            //para_out_string_ExeceptionReason = String.Empty;
            bool l_bool_return = false;
            DataSet l_DataSet = new DataSet();

            DataTable dtImportdata = new DataTable();
            dtImportdata.TableName = "ImportBaseRecords";

            Database l_DataBase = null;
            DbConnection l_DbConnection = null;
            DbTransaction l_DbTransaction = null;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                l_DbConnection = l_DataBase.CreateConnection();
                l_DbConnection.Open();

                if (l_DataBase == null) return null;

                l_DbTransaction = l_DbConnection.BeginTransaction();

                if (parameterDARFileData == null) return null;

                l_bool_return = PrepareImportBaseRecords(parameterDARFileData, intImportBaseHeaderID, l_DataBase, l_DbTransaction);//, out para_out_string_ExeceptionReason);

                ValidateImportBaseData(intImportBaseHeaderID, l_DataBase, l_DbTransaction);

                UpdateDARHeaderSummaryRecord(intImportBaseHeaderID, l_DataBase, l_DbTransaction);
                
                l_DbTransaction.Commit();


                if (l_bool_return)
                {
                    if (intImportBaseHeaderID != null)
                    {
                        l_DataSet = populateImportBaseRecords(intImportBaseHeaderID);
                    }
                }
                return l_DataSet;
            }
            catch
            {
                l_DbTransaction.Rollback();
                ReverseFeedImportDA.UpdateImportBaseHeaderStatus(intImportBaseHeaderID, "FAILED");               
                throw;
            }
            finally
            {
                if (l_DbConnection != null)
                {
                    if (l_DbConnection.State != ConnectionState.Closed)
                    {
                        l_DbConnection.Close();
                    }
                }
            }
        }

        private static void UpdateDARHeaderSummaryRecord(int parameterImportBaseHeaderID, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand cmd = null;
            try
            {
                if (parameterImportBaseHeaderID > 0)
                {

                    cmd = null;
                    cmd = parameterDatabase.GetStoredProcCommand("yrs_usp_STW_Import_UpdateHeaderSummary");
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                    parameterDatabase.AddInParameter(cmd, "@INT_ImportBaseHeaderID", DbType.Int32, parameterImportBaseHeaderID);

                    parameterDatabase.ExecuteNonQuery(cmd, parameterTransaction);

                }

            }
            catch
            {
                throw;
            }
            finally
            {
                cmd = null;
            }
        }

        public static void ValidateImportBaseData(int parameterImportBaseHeaderID, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand cmd = null;
            try
            {
                if (parameterImportBaseHeaderID > 0)
                {
                    cmd = null;
                    cmd = parameterDatabase.GetStoredProcCommand("yrs_usp_STW_Import_ValidateData");
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                    parameterDatabase.AddInParameter(cmd, "@INT_ImportProcessId", DbType.Int32, parameterImportBaseHeaderID);                               
                    parameterDatabase.ExecuteNonQuery(cmd, parameterTransaction);                    
                }
            }
            catch 
            {
                throw;
            }
            finally
            {
                cmd = null;
            }
        }

        public static bool PrepareImportBaseRecords(DataSet parameterDARFileData, int intImportBaseHeaderID, Database parameterDatabase, DbTransaction parameterTransaction)//, out string para_out_string_ExceptionReason)
        {
           
            DataTable l_DataTable = null;
            string strImportBaseID = "";
            bool isImportBaseRecordInserted = true;
          
            if (parameterDARFileData == null) return false ;
            
            try
            {

                l_DataTable = parameterDARFileData.Tables[0];

                if (l_DataTable == null) return false;

                foreach (DataRow l_DataRow in l_DataTable.Rows)
                {
                    if (l_DataRow != null)
                    {
                        strImportBaseID = InsertImportBaseRecords(l_DataRow, intImportBaseHeaderID, parameterDatabase, parameterTransaction);
                    }
                    if (strImportBaseID.Trim() == string.Empty)
                    {
                        isImportBaseRecordInserted = false;
                        break;
                       
                    }

                }

                return isImportBaseRecordInserted;
            }

            catch
            {
                throw;
            }
            
        }



        private static DataSet populateImportBaseRecords(int intImportBaseHeaderID)
        {
           
                Database l_DataBase = null;
                DataSet l_DataSet = null;
                string[] l_stringDataTableNames;

                DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("yrs_usp_STW_IMPORT_GetImportBaseRecords");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@INT_ImportBaseHeaderId", DbType.Int32, intImportBaseHeaderID);

                // Add the DataSet
                l_DataSet = new DataSet("ImportBaseRecords");

                l_stringDataTableNames = new string[] { "ImportBaseRecords", "ImportBaseHeaderRecords" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet);
                }

                return null;

            }
            catch
            {
                
                throw;
            }
        }

        private static string InsertImportBaseRecords(DataRow parameterDataRow, int parameterImportBaseHeaderID, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand;
           // double? stateTaxWithheld, localTaxWithheld;
            try
            {

                if (parameterDatabase == null) return string.Empty;

                if (parameterDataRow == null) return string.Empty;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("yrs_usp_STW_Import_CreateImportBaseRecords");

                if (l_DbCommand == null) return string.Empty;

                parameterDatabase.AddInParameter(l_DbCommand, "@INT_HeaderID", DbType.Int32, parameterImportBaseHeaderID);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_FundNo", DbType.String, parameterDataRow["FundNo"].ToString().Trim() );
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_SSNo", DbType.String, parameterDataRow["SSNo"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_FirstName", DbType.String, parameterDataRow["FirstName"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_LastName", DbType.String, parameterDataRow["LastName"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_TaxableAmount", DbType.String, parameterDataRow["TaxableAmount"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_NonTaxableAmount", DbType.String, parameterDataRow["NonTaxableAmount"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_TaxationAmount1", DbType.String, parameterDataRow["TaxableAmount"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_TaxationAmount2", DbType.String, parameterDataRow["NonTaxableAmount"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_TotalFederalAmount", DbType.String, parameterDataRow["FedTaxWithheld"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_StateCode", DbType.String, parameterDataRow["StateTaxCode"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_StateAmount", DbType.String, parameterDataRow["StateTaxWithheld"].ToString().Trim());

                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_LocalTaxCode", DbType.String, parameterDataRow["LocalTaxCode"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_LocalFlatAmount", DbType.String, parameterDataRow["LocalTaxWithheld"].ToString().Trim());

                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_OngoingDeductionCode1", DbType.String, parameterDataRow["DeductionsCode1"]);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_DeductionAmount1", DbType.String, parameterDataRow["MEDINSDeductions"]);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_OngoingDeductionCode2", DbType.String, parameterDataRow["DeductionsCode2"]);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_DeductionAmount2", DbType.String, parameterDataRow["PRCSTSFeeDeductions"]);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_OngoingDeductionCode3", DbType.String, parameterDataRow["DeductionsCode3"]);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_DeductionAmount3", DbType.String, parameterDataRow["NAFYRDeductions"]);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_OngoingDeductionCode4", DbType.String, parameterDataRow["DeductionsCode4"]);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_DeductionAmount4", DbType.String, parameterDataRow["IRSLVYDeductions"]);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_OngoingDeductionCode5", DbType.String, parameterDataRow["DeductionsCode5"]);
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_DeductionAmount5", DbType.String, parameterDataRow["LVLOVELDeductions"]);

                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_GrossAmount", DbType.String, parameterDataRow["GrossAmount"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_NetAmount", DbType.String, parameterDataRow["NetAmount"].ToString().Trim());

                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_PaymentMethodCode", DbType.String, parameterDataRow["PaymentMethodCode"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_CheckNumber", DbType.String, parameterDataRow["CheckNumber"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_CheckStatus", DbType.String, parameterDataRow["CheckStatus"].ToString().Trim());
                parameterDatabase.AddInParameter(l_DbCommand, "@VARCHAR_PayableDate", DbType.String, parameterDataRow["PayableDate"].ToString().Trim());
                parameterDatabase.AddOutParameter(l_DbCommand, "@VARCHAR_UniqueID", DbType.String,100);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);
                if (parameterDatabase.GetParameterValue(l_DbCommand, "@VARCHAR_UniqueID").GetType().ToString() != "System.DBNull")
                    return Convert.ToString(parameterDatabase.GetParameterValue(l_DbCommand, "@VARCHAR_UniqueID"));
                else
                    return String.Empty;

            }
            catch (SqlException sqlEx)
            {
                throw (sqlEx);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }


        /// <summary>
        /// Get error records against each record present in the csv file when file is imported 
        /// </summary>
        /// <param name="paramImportBaseHeaderId">Process id</param>
        /// <param name="paramFundId">Partiticpant fund id</param>
        /// <returns></returns>
         public static DataSet GetErrorDetailsForEachRecord(string paramImportBaseHeaderId, string paramImportBaseDetailID)
        {
            try
            {
                Database db = null;
                DbCommand LookUpCommandWrapper = null;
                DataSet dsErrorDetail = null;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_STW_Import_GetErrorDetails");
                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@INT_ImportBaseHeaderID", DbType.Int32, Convert.ToUInt32(paramImportBaseHeaderId));
                db.AddInParameter(LookUpCommandWrapper, "@INT_ImportBaseDetailID", DbType.Int32, Convert.ToUInt32(paramImportBaseDetailID));
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsErrorDetail = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsErrorDetail, "GetErrorDetail");
                return dsErrorDetail;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get pending records imported through DAR file for funding
        /// </summary>
        /// <returns>Get pending records else null</returns>
        public static DataSet GetPendingImportFilePresent()
        {
            Database db = null;
            DbCommand dbCommand = null;
            DataSet dsImportBaseList = null;
            string[] l_stringDataTableNames; 
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                dbCommand = db.GetStoredProcCommand("yrs_usp_STW_Import_GetPendingDARFileData");
                if (dbCommand == null) return null;

                dsImportBaseList = new DataSet();
                l_stringDataTableNames = new string[] { "ImportBaseRecords", "ImportBaseHeaderRecords" };
                db.LoadDataSet(dbCommand, dsImportBaseList, l_stringDataTableNames);
                
                return dsImportBaseList;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Method discards the imported csv file and return true after successfully deleting the records
        /// </summary>
        /// <param name="ImportBaseHeaderID"></param>
        /// <returns></returns>
        public static Boolean DiscardImportedDARFile(int ImportBaseHeaderID)
        {
            Database db = null;
            DbCommand cmd = null;
            DbTransaction transaction = null;
            DbConnection connection = null;
           
            try
            {
                
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;

                //Get a connection and Open it
                connection = db.CreateConnection();
                connection.Open();

                transaction = connection.BeginTransaction();
                if (transaction == null) return false;

                cmd = db.GetStoredProcCommand("yrs_usp_STW_Import_DiscardImportedDARFile");
                if (cmd == null) return false;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_ImportBaseHeaderID", DbType.Int32, ImportBaseHeaderID);
               
                db.ExecuteNonQuery(cmd, transaction);

                //Insert into YRS Activity log after entry into notes
               // LoggerDA.WriteLogDB(activityLogEntry, transaction);

                //Commit the transaction if everything was fine
                transaction.Commit();

                return true;
            }
            catch
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                
                connection = null;
                transaction = null;
                cmd = null;
                db = null;
            }
        }

        public static Boolean ProcessImportedDARFile(int ImportBaseHeaderID)
        {
            Database db = null;
            DbCommand cmd = null;
            DbTransaction transaction = null;
            DbConnection connection = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;

                //Get a connection and Open it
                connection = db.CreateConnection();
                connection.Open();

                transaction = connection.BeginTransaction();
                if (transaction == null) return false;

                cmd = db.GetStoredProcCommand("yrs_usp_STW_Import_SaveImportBaseFinal");
                if (cmd == null) return false;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_ImportBaseHeaderID", DbType.Int32, ImportBaseHeaderID);

                db.ExecuteNonQuery(cmd, transaction);

                //Commit the transaction if everything was fine
                transaction.Commit();
                return true;
            }
            catch
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    ReverseFeedImportDA.UpdateImportBaseHeaderStatus(ImportBaseHeaderID, "FAILED");
                 
                }
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }

                connection = null;
                transaction = null;
                cmd = null;
                db = null;
            }
        }
        public static DataSet GetDARFileImportReport(int intImportBaseHeaderID)
        {
            Database db = null;
            DbCommand dbCommand = null;
            DataSet dsSelectedDARFileRecords = null;
            string[] l_stringDataTableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                dbCommand = db.GetStoredProcCommand("yrs_usp_rpt_STW_Import_DARFileImport");
                if (dbCommand == null) return null;
                db.AddInParameter(dbCommand, "@INT_ImportBaseHeaderId", DbType.Int32, intImportBaseHeaderID);
                dsSelectedDARFileRecords = new DataSet();
                l_stringDataTableNames = new string[] { "ImportBaseRecords", "ErrorDetails" };
                db.LoadDataSet(dbCommand, dsSelectedDARFileRecords, l_stringDataTableNames);

                return dsSelectedDARFileRecords;
            }
            catch
            {
                throw;
            }
        }
    
       
    }
}
