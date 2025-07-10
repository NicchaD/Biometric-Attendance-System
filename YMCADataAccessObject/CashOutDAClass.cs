//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		    :	YMCA YRS
// FileName			    :	CashOutBOClass.cs
// Author Name		    :	
// Employee ID		    :	
// Creation Time		:	
// Program Specification Name	:	
//*******************************************************************************
//Modification History :
//*******************************************************************************
//Modified Date		Modified By			    Description
//*******************************************************************************
//2010.05.07        Ashish Srivastava       Resolve Production cashout lock Issue       
// 2010.07.07       Sanjay R.               Enhancement changes(Parameter Attribute DbType.String to DbType.guid)
//2012.06.05        Sanjeev(SG)             BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//2012.09.06        Sanjeev(SG)             BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//2014.10.07        Shashank Patel          BT-2633\YRS 5.0-2403: Remove expiration dates from withdrawal requests in YRS 
//2015.06.15        Dinesh Kanojia          BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
//2015.06.23        Sanjay S.               BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
//2015.09.16        Manthan Rajguru         YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2015.10.21        Anudeep A               YRS-AT-2463 - Cashout utility for participants with two plans. One release blank rather than two per participant (TrackIT 21783)
//*******************************************************************************

using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections;
using System.Globalization;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
    /// <summary>
    /// Summary description for CashOutDAClass.
    /// </summary>
    public class CashOutDAClass
    {
        # region "declarations"
        private DbConnection m_IDbConnection = null;
        private DbTransaction m_IDbTransaction = null;
        private Database m_Database = null;

        # endregion //declarations

        public CashOutDAClass()
        {
            //
            // TODO: Add constructor logic here
            //			
        }

        # region "Properties"
        public DbConnection GetConnectionObject
        {
            get
            {
                return m_IDbConnection;
            }
        }
        public DbTransaction GetTransactionObject
        {
            get
            {
                return m_IDbTransaction;
            }
        }
        public Database GetDatabaseObject
        {
            get
            {
                return m_Database;
            }
        }
        # endregion //"Properties"

        public DataTable GetMemberEmploymentDetails(string parameterPersonID, string parameterFundEventId)
        {
            DataSet dsMemberEmploymentDetails = null;
            DataTable dataTableMemberEmploymentDetails;
            Database db = null;
            DbCommand getCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_AP_GetMemberEmployeeEvent");
                getCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(getCommandWrapper, "@varchar_PersonID", DbType.String, parameterPersonID);
                db.AddInParameter(getCommandWrapper, "@varchar_FundEventID", DbType.String, parameterFundEventId);
                if (getCommandWrapper == null) return null;

                dsMemberEmploymentDetails = new DataSet();
                db.LoadDataSet(getCommandWrapper, dsMemberEmploymentDetails, "EmploymentDetails");
                dataTableMemberEmploymentDetails = dsMemberEmploymentDetails.Tables[0];

                return dataTableMemberEmploymentDetails;
            }
            catch
            {
                throw;
            }
        }
        public bool BeginTransaction()
        {
            try
            {

                m_Database = DatabaseFactory.CreateDatabase("YRS");
                if (m_Database == null) return false;

                m_IDbConnection = m_Database.CreateConnection();
                m_IDbConnection.Open();

                m_IDbTransaction = m_IDbConnection.BeginTransaction();

                return true;
            }
            catch
            {
                if (m_IDbTransaction != null)
                {
                    m_IDbTransaction.Rollback();
                    m_IDbConnection.Close();
                }

                throw;
            }
        }

        public bool CommitTransaction()
        {

            try
            {
                if (m_IDbConnection != null)
                {
                    if (m_IDbTransaction != null)
                    {
                        m_IDbTransaction.Commit();
                        m_IDbConnection.Close();
                    }
                }

                return true;
            }
            catch
            {
                throw;
            }
        }
        public bool RollbackTransaction()
        {

            try
            {
                if (m_IDbConnection != null)
                {
                    if (m_IDbTransaction != null)
                    {
                        m_IDbTransaction.Rollback();
                        m_IDbConnection.Close();
                    }
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        //		public string InsertRefunds(DataSet parameterRefundRequest)
        //		{ 	
        //			try
        //			{
        //				string l_string_return = "";
        //				
        //				RefundRequestDAClass objRefundRequestDAClass = new RefundRequestDAClass();
        //
        //				objRefundRequestDAClass.ConnectionObject = m_IDbConnection;
        //				objRefundRequestDAClass.TransactionObject = m_IDbTransaction;
        //
        //			    l_string_return = objRefundRequestDAClass.InsertRefunds(parameterRefundRequest);	
        //				
        //				objRefundRequestDAClass = null;
        //
        //				return l_string_return;
        //			}
        //			catch
        //			{
        //				throw;
        //			}
        //		}
        //		public bool SaveRefundRequestProcessing(DataSet parameterRefundRequest,string parameterPersonID,string parameterFundID,string paramterRefundType,bool parameterVested,bool parameterTerminated,bool parameterTookTDAccount)
        //		{ 			
        //			try
        //			{
        //				bool l_bool_return = false;
        //
        //				RefundRequestDAClass objRefundRequestDAClass = new RefundRequestDAClass();
        //				
        //				l_bool_return = objRefundRequestDAClass.SaveRefundRequestProcess(parameterRefundRequest,parameterPersonID,parameterFundID,paramterRefundType,parameterVested,parameterTerminated,parameterTookTDAccount);	
        //				
        //				objRefundRequestDAClass = null;
        //
        //				return l_bool_return;
        //			}
        //			catch
        //			{
        //				throw;
        //			}
        //		}
        //		public static void  UpdateCashOutStatus(DataSet parameterstatus)
        //		{	
        //			Database db = null;
        //			DbCommand insertCommandWrapper = null;
        //			DbCommand UpdateCommandWrapper = null;
        //			DbCommand deleteCommandWrapper=null;
        //			try
        //			{
        //
        //				db = DatabaseFactory.CreateDatabase("YRS");
        //
        //							
        //				UpdateCommandWrapper=db.GetStoredProcCommand("yrs_usp_UpdateCashOutStatus");
        //				if(UpdateCommandWrapper!=null)
        //				{
        //					UpdateCommandWrapper.AddInParameter("@chv_RefRequestId",DbType.String,"RefRequestId",DataRowVersion.Current);
        //					UpdateCommandWrapper.AddInParameter("@chv_FundEventId",DbType.String,"FundEventId",DataRowVersion.Current);
        //					UpdateCommandWrapper.AddInParameter("@char_SSNo",DbType.String,"SSno",DataRowVersion.Current);				
        //					
        //					db.UpdateDataSet(parameterstatus,"ACHDebitsImport" ,insertCommandWrapper,UpdateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
        //					
        //					
        //					
        //					
        //				}
        //				
        //			}
        //			catch
        //			{	
        //				throw;
        //			}
        //			
        //		}


        public string InsertRefunds(DataSet parameterRefundRequest)
        {
            string l_string_RefRequestId = "";
            try
            {
                l_string_RefRequestId = RefundRequestDAClass.InsertRefunds(parameterRefundRequest, m_Database, m_IDbTransaction);

                return l_string_RefRequestId;
            }
            catch
            {
                throw;
            }
        }
        public bool SaveRefundRequestProcessing(DataSet parameterRefundRequest, string parameterPersonID, string parameterFundID, string paramterRefundType, bool parameterVested, bool parameterTerminated, bool parameterTookTDAccount, string parameterStatusType, out string para_out_string_ExeceptionReason)
        {
            try
            {
                bool l_bool_return = false;


                l_bool_return = RefundRequestDAClass.SaveRefundRequestProcess(parameterRefundRequest, parameterPersonID, parameterFundID, paramterRefundType, parameterVested, parameterTerminated, parameterTookTDAccount, parameterStatusType, m_Database, m_IDbTransaction, out para_out_string_ExeceptionReason);


                return l_bool_return;
            }
            catch
            {
                throw;
            }
        }

        //Added By SG: 2012.09.06: BT-960
        //public void UpdateCashOutStatus(string parameterRefRequestId, string parameterFundEventId, decimal parameterTotalAmount, decimal parameterWithheldTax, decimal parameterTerminationPIA, decimal parameterMaxPIAAmount, string parameterExceptionMessage, string parameterBatchId, string parameterPlanType)
        public void UpdateCashOutStatus(string parameterRefRequestId, string parameterFundEventId, decimal parameterTotalAmount, decimal parameterWithheldTax, decimal parameterTerminationPIA, decimal parameterMaxPIAAmount, string parameterExceptionMessage, string parameterBatchId, string parameterPlanType, string parameterCashOutReqType)
        {

            DbCommand UpdateCommandWrapper = null;
            try
            {



                UpdateCommandWrapper = m_Database.GetStoredProcCommand("dbo.yrs_usp_CashOut_UpdateCashOutLogForProcessedRequests");
                UpdateCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                if (UpdateCommandWrapper != null)
                {
                    m_Database.AddInParameter(UpdateCommandWrapper, "@chv_RefRequestId", DbType.String, parameterRefRequestId);
                    m_Database.AddInParameter(UpdateCommandWrapper, "@chv_FundEventId", DbType.String, parameterFundEventId);
                    m_Database.AddInParameter(UpdateCommandWrapper, "@numeric_TotalAmount", DbType.Decimal, parameterTotalAmount);
                    m_Database.AddInParameter(UpdateCommandWrapper, "@numeric_WithheldTax", DbType.Decimal, parameterWithheldTax);
                    m_Database.AddInParameter(UpdateCommandWrapper, "@numeric_TerminationPIA", DbType.Decimal, parameterTerminationPIA);
                    m_Database.AddInParameter(UpdateCommandWrapper, "@numeric_MaxPIAAmount", DbType.Decimal, parameterMaxPIAAmount);
                    m_Database.AddInParameter(UpdateCommandWrapper, "@chv_ExceptionReason", DbType.String, parameterExceptionMessage);
                    m_Database.AddInParameter(UpdateCommandWrapper, "@chv_BatchId", DbType.String, parameterBatchId);
                    //Added By SG: 2012.08.29: BT-960
                    m_Database.AddInParameter(UpdateCommandWrapper, "@chvPlanType", DbType.String, parameterPlanType);
                    //Added By SG: 2012.09.06: BT-960
                    m_Database.AddInParameter(UpdateCommandWrapper, "@CashOutReqType", DbType.String, parameterCashOutReqType);

                    m_Database.ExecuteNonQuery(UpdateCommandWrapper, m_IDbTransaction);




                }

            }
            catch
            {
                throw;
            }

        }
        # region "Call for UI"

        public DataSet GetAmountRange()
        {
            DataSet dsAmountRange = null;
            Database db = null;
            DbCommand CommandGetCashoutRange = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                CommandGetCashoutRange = db.GetStoredProcCommand("yrs_usp_Cashout_GetCashoutRange");
                if (CommandGetCashoutRange == null) return null;
                dsAmountRange = new DataSet();
                db.LoadDataSet(CommandGetCashoutRange, dsAmountRange, "AmountRange");
                return dsAmountRange;

            }
            catch
            {
                throw;
            }
        }

        public DataSet GetNextBatchId()
        {
            DataSet dsBatchId = null;
            Database db = null;
            DbCommand CommandGetBatchId = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                CommandGetBatchId = db.GetStoredProcCommand("yrs_usp_CashOut_GetNextBatchID");
                if (CommandGetBatchId == null) return null;
                dsBatchId = new DataSet();
                db.LoadDataSet(CommandGetBatchId, dsBatchId, "BatchId");
                return dsBatchId;

            }
            catch
            {
                throw;
            }
        }

        public DataSet GetEligibleParticipants(Double parameterLowerLimit, Double parameterUpperLimit, string parameterCashOutRptDesc, string parameterIPAddress, string parameterUserId)
        {

            DataSet dsEligibleParticipants = null;
            Database db = null;
            DbCommand CommandGetList = null;
            string[] l_stringDataTableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                CommandGetList = db.GetStoredProcCommand("yrs_usp_Cashout_SelectEligibleParticipants");
                CommandGetList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                if (CommandGetList == null) return null;
                dsEligibleParticipants = new DataSet();

                db.AddInParameter(CommandGetList, "@numeric_Lowerlimit", DbType.Double, parameterLowerLimit);
                db.AddInParameter(CommandGetList, "@numeric_Upperlimit", DbType.Double, parameterUpperLimit);
                db.AddInParameter(CommandGetList, "@chvCashOutRptDesc", DbType.String, parameterCashOutRptDesc);
                db.AddInParameter(CommandGetList, "@chvUserId", DbType.String, parameterUserId);
                db.AddInParameter(CommandGetList, "@chvIPAddress", DbType.String, parameterIPAddress);
                l_stringDataTableNames = new string[] { "EligibleParticipants" };
                db.LoadDataSet(CommandGetList, dsEligibleParticipants, l_stringDataTableNames);

                return dsEligibleParticipants;

            }
            catch
            {
                throw;

            }

        }

        

        //Added By SG: 2012.19.11: BT-960
        //public void DeleteReportData(string parameterUserId, string parameterIPAddress)
        public void DeleteReportData(string parameterUserId, string parameterIPAddress, string parameterBatchId)
        {

            Database db = null;
            DbCommand deleteCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_CashOut_DeleteReportData");
                if (deleteCommandWrapper != null)
                {

                    db.AddInParameter(deleteCommandWrapper, "@chvUserId", DbType.String, parameterUserId);
                    db.AddInParameter(deleteCommandWrapper, "@chvIPAddress", DbType.String, parameterIPAddress);
                    //deleteCommandWrapper.AddInParameter("@chvBatchId",DbType.String,parameterBatchId);
                    //Added By SG: 2012.19.11: BT-960
                    db.AddInParameter(deleteCommandWrapper, "@chvBatchId", DbType.String, parameterBatchId);
                    db.ExecuteNonQuery(deleteCommandWrapper);
                }
            }

            catch
            {
                throw;
            }

        }

        public void InsertReportData(DataSet parameterReportData)
        {
            Database parameterDatabase = null;
            DbCommand insertCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;
            DbTransaction l_IDbTransaction = null;
            DbConnection l_IDbConnection = null;
            try
            {
                parameterDatabase = DatabaseFactory.CreateDatabase("YRS");
                if (parameterDatabase == null) return;
                l_IDbConnection = parameterDatabase.CreateConnection();
                l_IDbConnection.Open();
                if (l_IDbConnection == null) return;
                l_IDbTransaction = l_IDbConnection.BeginTransaction();
                insertCommandWrapper = parameterDatabase.GetStoredProcCommand("yrs_usp_CashOut_InsertReportData");
                if (insertCommandWrapper != null)
                {
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@chvBatchId", DbType.String, "chvBatchId", DataRowVersion.Current);  //Changed parameter attribute DbType.String to  DbType.Guid by SR:2010.07.07 for migration
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@guiFundEventId", DbType.Guid, "guiFundEventId", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@intFundIdNo", DbType.Int32, "intFundIdNo", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@chrSSNo", DbType.String, "chrSSNo", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@chvLastName", DbType.String, "chvLastName", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@chvFirstName", DbType.String, "chvFirstName", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@chvMiddleName", DbType.String, "chvMiddleName", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@numPersonAge", DbType.Int32, "numPersonAge", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@mnyGrossRefundAmount", DbType.Decimal, "mnyGrossRefundAmount", DataRowVersion.Current);
                    //mnyTaxWithHeld
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@mnyTaxWithHeld", DbType.Decimal, "mnyTaxWithHeld", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@bitLookUp", DbType.Boolean, "bitLookUp", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@bitSelected", DbType.Boolean, "bitSelected", DataRowVersion.Current);
                    //insertCommandWrapper.AddInParameter("@bitProcessed",DbType.Int32,"bitProcessed",DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@intStdReportCount", DbType.Int32, "intStdReportCount", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@intActualReportCount", DbType.Int32, "intActualReportCount", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@chvCashOutRangeDesc", DbType.String, "chvCashOutRangeDesc", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@chvIPAddress", DbType.String, "chvIPAddress", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@chvUserId", DbType.String, "chvUserId", DataRowVersion.Current);
                    //aparna -yrps -4181
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@chvPlanType", DbType.String, "chvPlanType", DataRowVersion.Current);
                    //aparna -yrps -4181
                    parameterDatabase.UpdateDataSet(parameterReportData, "CashoutLogSchema", insertCommandWrapper, UpdateCommandWrapper, deleteCommandWrapper, l_IDbTransaction);
                    l_IDbTransaction.Commit();

                }

            }

            catch (SqlException SqlEx)
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
                        l_IDbConnection.Close();
                    }
                }
            }
        }

        //yrs_usp_Cashout_UpdateCashOutLog

        public void UpdateCashoutLogForRpts(DataSet parameterLogData)
        {
            Database l_DataBase = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand InsertCommandWrapper = null;
            DbCommand DeleteCommandWrapper = null;
            DbTransaction l_IDbTransaction = null;
            DbConnection l_IDbConnection = null;
            try
            {

                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return;
                l_IDbConnection = l_DataBase.CreateConnection();
                l_IDbConnection.Open();
                l_IDbTransaction = l_IDbConnection.BeginTransaction();
                UpdateCommandWrapper = l_DataBase.GetStoredProcCommand("yrs_usp_Cashout_UpdateCashOutLog");
                if (UpdateCommandWrapper == null) return;

                l_DataBase.AddInParameter(UpdateCommandWrapper, "@guiFundEventId", DbType.String, "guiFundEventId", DataRowVersion.Current);
                l_DataBase.AddInParameter(UpdateCommandWrapper, "@intStdReportCount", DbType.Int32, "intStdReportCount", DataRowVersion.Current);
                l_DataBase.AddInParameter(UpdateCommandWrapper, "@intActualReportCount", DbType.Int32, "intActualReportCount", DataRowVersion.Current);
                l_DataBase.AddInParameter(UpdateCommandWrapper, "@chvExceptionReason", DbType.String, "chvExceptionReason", DataRowVersion.Current);
                l_DataBase.AddInParameter(UpdateCommandWrapper, "@chvBatchId", DbType.String, "chvBatchId", DataRowVersion.Current);
                l_DataBase.UpdateDataSet(parameterLogData, "ReportData", InsertCommandWrapper, UpdateCommandWrapper, DeleteCommandWrapper, l_IDbTransaction);
                l_IDbTransaction.Commit();

                //l_DataBase.ExecuteNonQuery(UpdateCommandWrapper);

                //l_DataBase.UpdateDataSet(parameterCashoutLog,"CashoutLog" ,InsertCommandWrapper,UpdateCommandWrapper,DeleteCommandWrapper,UpdateBehavior.Standard);

                return;

            }

            catch (SqlException SqlEx)
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
                        l_IDbConnection.Close();
                    }
                }
            }

        }


        public void UpdateCashoutLog(int parameterintStdReportCount, int parameterintActualReportCount, string parameterchvExceptionReason, string parameterguiFundEventId)
        {
            Database l_DataBase = null;
            DbCommand UpdateCommandWrapper = null;
            // DbCommand InsertCommandWrapper = null;
            // DbCommand DeleteCommandWrapper = null;
            try
            {

                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return;
                UpdateCommandWrapper = l_DataBase.GetStoredProcCommand("yrs_usp_Cashout_UpdateCashOutLog");
                if (UpdateCommandWrapper == null) return;
                l_DataBase.AddInParameter(UpdateCommandWrapper, "@guiFundEventId", DbType.String, parameterguiFundEventId);
                l_DataBase.AddInParameter(UpdateCommandWrapper, "@intStdReportCount", DbType.Int32, parameterintStdReportCount);
                l_DataBase.AddInParameter(UpdateCommandWrapper, "@intActualReportCount", DbType.Int32, parameterintActualReportCount);
                l_DataBase.AddInParameter(UpdateCommandWrapper, "@chvExceptionReason", DbType.Int32, parameterchvExceptionReason);

                l_DataBase.ExecuteNonQuery(UpdateCommandWrapper);

                //l_DataBase.UpdateDataSet(parameterCashoutLog,"CashoutLog" ,InsertCommandWrapper,UpdateCommandWrapper,DeleteCommandWrapper,UpdateBehavior.Standard);

                return;

            }

            catch
            {

                throw;

            }

        }



        public DataSet GetCashoutLogSchema()
        {

            Database l_DataBase = null;

            DbCommand l_DBCommandWrapper = null;

            DataSet l_DataSet = null;

            string l_TableNames;

            try
            {

                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_Cashout_GetSchemaTables");

                if (l_DBCommandWrapper == null) return null;

                l_DataSet = new DataSet("CashoutTable");

                l_TableNames = "CashoutLogSchema";

                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);

                return l_DataSet;

            }

            catch
            {

                throw;

            }

        }

        public DataSet SelectReportData(string parameterIPAddress, string parameterUserId, string parameterBatchId)
        {

            Database l_database = null;

            DbCommand Select_DBCommandWrapper = null;

            DataSet l_DataSet = null;


            try
            {

                l_database = DatabaseFactory.CreateDatabase("YRS");

                if (l_database == null) return null;

                Select_DBCommandWrapper = l_database.GetStoredProcCommand("yrs_usp_CashOut_SelectDataForRpts");

                if (Select_DBCommandWrapper == null) return null;

                l_DataSet = new DataSet();

                l_database.AddInParameter(Select_DBCommandWrapper, "@chvIPAddress", DbType.String, parameterIPAddress);
                l_database.AddInParameter(Select_DBCommandWrapper, "@chvUserId", DbType.String, parameterUserId);
                l_database.AddInParameter(Select_DBCommandWrapper, "@chvBatchId", DbType.String, parameterBatchId);
                l_database.LoadDataSet(Select_DBCommandWrapper, l_DataSet, "ReportData");

                return l_DataSet;

            }

            catch
            {

                throw;

            }

        }

        //START: SG: 2012.06.05: BT-960
        public DataSet GetRequestedParticipantForProcessing(string parameterBatchID)
        {
            DataSet dsRequestedParticipants = null;
            Database db = null;
            DbCommand CommandGetList = null;
            string[] l_stringDataTableNames;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null)
                    return null;

                CommandGetList = db.GetStoredProcCommand("yrs_usp_Cashout_GetRequestedCOForProcessing");
                CommandGetList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                if (CommandGetList == null)
                    return null;

                dsRequestedParticipants = new DataSet();
                db.AddInParameter(CommandGetList, "@chvBatchId", DbType.String, parameterBatchID);
                l_stringDataTableNames = new string[] { "RequestedParticipants" };
                db.LoadDataSet(CommandGetList, dsRequestedParticipants, l_stringDataTableNames);

                return dsRequestedParticipants;
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
                CommandGetList.Dispose();
                l_stringDataTableNames = null;
            }
        }

        public void InsertLettersFor_0to50(string l_string_PersID, string l_string_FundEventID)
        {
            Database parameterDatabase = null;
            DbCommand insertCommandWrapper = null;
            DbConnection l_IDbConnection = null;

            try
            {
                parameterDatabase = DatabaseFactory.CreateDatabase("YRS");
                if (parameterDatabase == null)
                {
                    return;
                }

                l_IDbConnection = parameterDatabase.CreateConnection();
                l_IDbConnection.Open();

                if (l_IDbConnection == null)
                {
                    return;
                }

                insertCommandWrapper = parameterDatabase.GetStoredProcCommand("yrs_usp_Cashout_InsertLettersFor0to50");
                if (insertCommandWrapper != null)
                {
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@PersID", DbType.Guid, new Guid(l_string_PersID));
                    parameterDatabase.AddInParameter(insertCommandWrapper, "@FundEventID", DbType.Guid, new Guid(l_string_FundEventID));

                    parameterDatabase.ExecuteNonQuery(insertCommandWrapper);
                }

            }
            catch (SqlException SqlEx)
            {
                throw SqlEx;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (l_IDbConnection != null)
                {
                    if (l_IDbConnection.State != ConnectionState.Closed)
                    {
                        l_IDbConnection.Close();
                    }
                }
            }
        }

        public void CancelPendingCashoutRequest(string parameterBatchID, string parameterRefCanResCode)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null)
                    return;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_Cashout_CancelPendingRequests");

                if (l_DbCommand == null)
                    return;

                l_DataBase.AddInParameter(l_DbCommand, "@BatchID", DbType.String, parameterBatchID);
                l_DataBase.AddInParameter(l_DbCommand, "@Updator", DbType.String, "Yrs");
                l_DataBase.AddInParameter(l_DbCommand, "@CanReasonDesc", DbType.String, parameterRefCanResCode);

                l_DataBase.ExecuteNonQuery(l_DbCommand);
            }
            catch
            {
                throw;
            }
            finally
            {
                l_DataBase = null;
                l_DbCommand = null;
            }
        }
        //END: SG: 2012.06.05: BT-960

        //Added By SG: 2012.08.06: BT-960
        //public DataRow[] GetCashoutLoadDisbursements(string parameterStrFundIDNo, string parameterStrPlanType)
        //{
        //    DataSet dsRefundRequestProcessing = null;
        //    Database db = null;
        //    DbCommand CommandGetList = null;
        //    string[] l_stringDataTableNames;

        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");
        //        if (db == null)
        //            return null;

        //        CommandGetList = db.GetStoredProcCommand("yrs_usp_CashoutLoadDisbursements");
        //        CommandGetList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
        //        if (CommandGetList == null)
        //            return null;

        //        dsRefundRequestProcessing = new DataSet();
        //        db.AddInParameter(CommandGetList, "@intFundIdNo", DbType.Int32, Convert.ToInt32(parameterStrFundIDNo));
        //        db.AddInParameter(CommandGetList, "@chvPlanType", DbType.String, parameterStrPlanType);

        //        l_stringDataTableNames = new string[] { "RefundRequestProcessing" };
        //        db.LoadDataSet(CommandGetList, dsRefundRequestProcessing, l_stringDataTableNames);

        //        DataRow[] drProcessing = dsRefundRequestProcessing.Tables["RefundRequestProcessing"].Select();
        //        return drProcessing;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        db = null;
        //        CommandGetList.Dispose();
        //        l_stringDataTableNames = null;
        //    }
        //}
        //2015.10.21        Anudeep A               YRS-AT-2463 Added a new parameter to filter as per the cashout range
        public DataTable GetDataTableCashoutBatchRecords(string strCashoutRange)
        {
            DataSet dsBatchRecords = null;
            Database db = null;
            DbCommand CommandGetList = null;
            string[] l_stringDataTableNames;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null)
                    return null;

                CommandGetList = db.GetStoredProcCommand("yrs_usp_Cashout_FetchBatchRecords");
                CommandGetList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                if (CommandGetList == null)
                    return null;
                db.AddInParameter(CommandGetList, "@chvCashoutRange", DbType.String, strCashoutRange);//2015.10.21        Anudeep A               YRS-AT-2463 Added a new parameter to filter as per the cashout range
                dsBatchRecords = new DataSet();

                l_stringDataTableNames = new string[] { "BatchRecords" };
                db.LoadDataSet(CommandGetList, dsBatchRecords, l_stringDataTableNames);

                return dsBatchRecords.Tables[0];
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
                CommandGetList.Dispose();
                l_stringDataTableNames = null;
            }
        }

        # endregion //"Call for UI"

        #region Added for resolve locking Issue
        /// <summary>
        /// This method get Refund request details
        /// </summary>
        /// <param name="parameterPersonID"></param>
        /// <param name="parameterFundID"></param>
        /// <returns></returns>
        public DataTable LoadRefundRequestDetails(string parameterPersonID, string parameterFundID)
        {
            DataTable l_DataTable = null;
            try
            {
                l_DataTable = YMCARET.YmcaDataAccessObject.RefundRequestDAClass.MemberRefundRequestDetails(parameterPersonID, parameterFundID, m_Database, m_IDbTransaction);
                return l_DataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paraRefRequestID"></param>
        /// <returns></returns>
        public DataTable GetRefundRequestDetails(string paraRefRequestID)
        {
            DataTable l_DataTable = null;
            try
            {
                l_DataTable = YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetRefundRequestsForCashOut(paraRefRequestID, m_Database, m_IDbTransaction);
                return l_DataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paraRefRequestID"></param>
        /// <returns></returns>
        public DataTable GetDataTableRefundable(string paraFundEventID)
        {
            DataTable l_DataTable = null;
            try
            {
                l_DataTable = YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetNewBalanceDataTable(paraFundEventID, m_Database, m_IDbTransaction);
                return l_DataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion



		//START: Priya: 2012.10.31: BT-960
		public DataSet GetBatchDetails(string parameterBatchID)
		{
			DataSet dsBatchParticipants = null;
			Database db = null;
			DbCommand CommandGetList = null;
			string[] l_stringDataTableNames;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null)
					return null;

				CommandGetList = db.GetStoredProcCommand("yrs_usp_Cashout_FetchBatchDetails");
				CommandGetList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				if (CommandGetList == null)
					return null;

				dsBatchParticipants = new DataSet();
				db.AddInParameter(CommandGetList, "@chvBatchId", DbType.String, parameterBatchID);
				l_stringDataTableNames = new string[] { "BatchParticipants" };
				db.LoadDataSet(CommandGetList, dsBatchParticipants, l_stringDataTableNames);

				return dsBatchParticipants;
			}
			catch
			{
				throw;
			}
			finally
			{
				db = null;
				CommandGetList.Dispose();
				l_stringDataTableNames = null;
			}
		}

		public DataSet  getCashOutFormsLetters(string key)
		{
			DataSet dsCashOutFormLetterReportNames = null;
			Database db = null;
			DbCommand CommandGetList = null;
			string[] l_stringDataTableNames;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null)
					return null;




				CommandGetList = db.GetStoredProcCommand("dbo.yrs_usp_AtsMetaConfiguration_Get");
				CommandGetList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
				if (CommandGetList == null) { return null; }

				dsCashOutFormLetterReportNames = new DataSet();

				db.AddInParameter(CommandGetList, "@key", DbType.String, key);

				l_stringDataTableNames = new string[] { "CashOutFormLetterReportNames" };
				db.LoadDataSet(CommandGetList, dsCashOutFormLetterReportNames, l_stringDataTableNames);

				return dsCashOutFormLetterReportNames;
			}
			catch
			{
				throw;

			}
			finally
			{
				db = null;
				CommandGetList.Dispose();
				l_stringDataTableNames = null;
			}
		}


        public DataSet GetMemberProcessedAmount(string paramFundEventId, string paramRefRequedtID, string paramBatchId, string paramPlanType)
        {
            DataSet dsMemberProcessedAmount = null;
            Database db = null;
            DbCommand getCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Cashout_GetMemberProcessedAmount");
                getCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                
                db.AddInParameter(getCommandWrapper, "@guiFundEventID", DbType.String, paramFundEventId);
                db.AddInParameter(getCommandWrapper, "@guiRefRequestID", DbType.String, paramRefRequedtID);
                db.AddInParameter(getCommandWrapper, "@chvBatchId", DbType.String, paramBatchId);
                db.AddInParameter(getCommandWrapper, "@chvPlanType", DbType.String, paramPlanType);
               
                if (getCommandWrapper == null) return null;

                dsMemberProcessedAmount = new DataSet();
                db.LoadDataSet(getCommandWrapper, dsMemberProcessedAmount, "EmploymentDetails");             

                return dsMemberProcessedAmount;
            }
            catch
            {
                throw;
            }
        }


        public DataSet GetUnProcessIDM(string StrBatchId)
        {
            DataSet dsGetUnprocessIDM = null;
            Database db = null;
            DbCommand getCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                getCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetUnprocessIDM");
                getCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(getCommandWrapper, "@chvBatchId", DbType.String, StrBatchId);

                if (getCommandWrapper == null) return null;
                dsGetUnprocessIDM = new DataSet();
                db.LoadDataSet(getCommandWrapper, dsGetUnprocessIDM, "UnProcessIDM");

                return dsGetUnprocessIDM;
            }
            catch
            {
                throw;
            }
        }

        //SP 2014.10.07 BT-2633\YRS 5.0-2403: Remove expiration dates from withdrawal requests in YRS -Start
        public void ExpiredRefRequestsForSelectedPerson(string XmlSelectedPersDetails)
        {
            Database db = null;
            DbCommand dbCommand = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) throw new Exception("Database object is null");

                dbCommand = db.GetStoredProcCommand("yrs_usp_RR_ExpireRefRequestsForSelectedPersons");

                dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                db.AddInParameter(dbCommand, "@XMLPersDetails", DbType.String, XmlSelectedPersDetails);

                if (dbCommand == null) throw new Exception("Database Command object is null");

                db.ExecuteNonQuery(dbCommand);     
                
            }
            catch
            {
                throw;
            }
            
        }
        //SP 2014.10.07 BT-2633\YRS 5.0-2403: Remove expiration dates from withdrawal requests in YRS -End
        //Start: 2015.06.15        Dinesh Kanojia      BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
        public DataSet GetSpecialEligibleParticipants(string strFundiD)
        {
            DataSet dsEligibleParticipants = null;
            Database db = null;
            DbCommand CommandGetList = null;
            string[] l_stringDataTableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                CommandGetList = db.GetStoredProcCommand("yrs_usp_GetSpecialCashoutrecords");
                CommandGetList.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                if (CommandGetList == null) return null;
                dsEligibleParticipants = new DataSet();                
                db.AddInParameter(CommandGetList, "@chvFundId", DbType.String, strFundiD.Replace("\r", "").Replace("\n", ""));  //BT-2890: YRS 5.0-2523: changed to handle CRLF ("\r\n") 
                l_stringDataTableNames = new string[] { "SelectedBatchRecords" };
                db.LoadDataSet(CommandGetList, dsEligibleParticipants, l_stringDataTableNames);
                return dsEligibleParticipants;
            }
            catch
            {
                throw;
            }
        }
        //End: 2015.06.15        Dinesh Kanojia      BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
    }
}
