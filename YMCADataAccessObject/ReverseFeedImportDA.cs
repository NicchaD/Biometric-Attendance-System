//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	ReverseFeedImportBO.cs
// Author Name		:	Pooja Kumkar
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	12/27/2019
// Description	    :	Data access class for reverse feed import
// Declared in Version : 20.7.2 | YRS-AT-4641 -  YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed)  
//**************************************************************************************************************
// MODIFICATION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************/
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
    public class ReverseFeedImportDA
    {
        public static DataSet GetDataInGrid(int exportBaseProcessId)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_STW_Import_ReverseFeed_CompareFiles");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_ImportBaseHeaderId", DbType.Int32, exportBaseProcessId);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "ReverseFeedDetails");
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }

        public static DataSet GetDataInExceptionTabGrid(int importBaseProcessId)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            string[] l_stringDataTableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_STW_Import_GetExceptionInReverseFeed");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_ImportBaseHeaderID", DbType.Int32, importBaseProcessId);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                l_stringDataTableNames = new string[] { "ReverseFeedExceptionDetails", "ReverseFeedFileError" };
                db.LoadDataSet(cmd, ds, l_stringDataTableNames);
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }

        public static DataSet GetHeaderBaseID()
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            string[] l_stringDataTableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_STW_Import_GetHeaderIDInReverseFeed");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                l_stringDataTableNames = new string[] { "ReverseFeedPageLevelDetails", "ImportBaseSummaryRecords" };
                db.LoadDataSet(cmd, ds, l_stringDataTableNames);
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }
        public static DataSet GetDataExceptionList(int importBaseProcessId, int baseId,int FundNo)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_STW_IMPORT_GetExceptionErrorDetails");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_ImportBaseHeaderID", DbType.Int32, importBaseProcessId);
                db.AddInParameter(cmd, "@INT_ImportBaseDetailID", DbType.Int32, baseId);
                db.AddInParameter(cmd, "@INT_ImportBaseFundNo", DbType.Int32, FundNo);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "ExceptionList");
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }
        public static DataSet GetDifferenceData(int exportBaseProcessId, int importedHeaderId, int FundId)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_STW_Import_ReverseFeed_GetDifference");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_ImportBaseHeaderId", DbType.Int32, exportBaseProcessId);
                db.AddInParameter(cmd, "@INT_ExportBaseHeaderId", DbType.Int32, importedHeaderId);
                db.AddInParameter(cmd, "@INT_FundId", DbType.Int32, FundId);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "ReverseFeedDifferenceDetails");
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }
        public static int UpdateImportBaseHeaderStatus(int importedHeaderId, string status)
        {
            Database db;
            DbCommand cmd;
            string newStatus = String.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                cmd = db.GetStoredProcCommand("dbo.yrs_usp_STW_Import_UpdateHeaderStatus");
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_ImportBaseHeaderID", DbType.Int32, importedHeaderId);
                db.AddInParameter(cmd, "@VARCHAR_chvStatus", DbType.String, status);
                int i = db.ExecuteNonQuery(cmd);
                return i;

            }
            catch
            {
                throw;
            }
            finally
            {
                cmd = null;
                db = null;
            }
        }
        public static Boolean DiscardReverseFeed(int ImportBaseHeaderID)
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

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_STW_Import_DiscardReverseFeed");
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

        public static DataSet GetDataForPrintList(int ImportBaseHeaderID)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            string[] l_stringDataTableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_rpt_STW_Import_ReverseFeed_CompareFiles");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_ImportBaseHeaderId", DbType.Int32, ImportBaseHeaderID);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                l_stringDataTableNames = new string[] { "dsSelectedReverseFeedFileRecords", "dsselectedReverseFeedFileErrorlist" };
                db.LoadDataSet(cmd, ds, l_stringDataTableNames);
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }
    }
}
