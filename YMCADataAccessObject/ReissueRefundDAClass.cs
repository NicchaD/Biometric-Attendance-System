//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA YRS
// FileName			:	RefundRequestDAClass.cs
// Author Name		:	SrimuruganG
// Employee ID		:	32365
// Email				:	srimurugan.ag@icici-infotech.com
// Contact No			:	8744
// Creation Time		:	10/26/2005 4:26:01 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//********************************************************************************************************************************
//Modified By        Date            Description
//Aparna Samala	    01-Aug-2007     YREN-3591-new column added to atsreissuetransacts.
//Priya				08-Feb-2010		YRS 5.0-1013 : Withdrawal re-issue rollover checks with no deductions
//Priya             11-June-2010    Made enhancement chnages
//Priya             11/30/2010      BT-592,YRS 5.0-1171 : Disbursement Ref Id not filled in 
//Priya				14-Feb-2011 	BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2
//Manthan Rajguru   2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************************************************************

using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
    {

    /// <summary>
    /// Summary description for ReissueRefundDAClass.
    /// </summary>
    public sealed class ReissueRefundDAClass
        {
        public ReissueRefundDAClass()
            {
            }

        public static DataTable GetRefundReissuesDetails()
            {

            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
                {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetRefundReissueDetails");

                if (l_DBCommandWrapper == null) return null;

                // Connection TimeOut.
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);



                l_DataSet = new DataSet("RefundReissues");

                l_TableNames = new string[] { "RefundReissues" };

                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);


                if (l_DataSet != null)
                    {
                    return l_DataSet.Tables["RefundReissues"];
                    }
                else
                    return null;


                }
            catch
                {
                throw;
                }

            }


        /// <summary>
        ///		This method will return all deatails (deduction, Sum of the fund and other Details)
        /// </summary>
        /// <param name="parameterPersonID"></param>
        /// <param name="parameterDisbursementID"></param>
        /// <param name="parameterFundID"></param>
        /// <returns></returns>
        /// 
        //public static DataSet GetMemberReissueDetails (string parameterPersonID, string parameterDisbursementID, string parameterFundID)
        //Priya 21 sep 09
        public static DataSet GetMemberReissueDetails(string parameterPersonID, string parameterDisbursementID, string parameterRefRequestID)
            {
            Database l_DataBase = null;

            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;

            string[] l_TableNames;

            try
                {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetDeductionDetails");

                if (l_DBCommandWrapper == null) return null;

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@varchar_DisbursementID", DbType.String, parameterDisbursementID);
                //Priya 07-Sep-09 added parameter persid to get deduction details
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@Varchar_PersID", DbType.String, parameterPersonID);


                // Conection Time out
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataSet = new DataSet("MemberReIssues");


                //****************************************************************************************************
                // Load Deduction Details 
                l_TableNames = new string[] { "Deductions" };

                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);

                //****************************************************************************************************


                //****************************************************************************************************


                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_TransSumForReissue");

                if (l_DBCommandWrapper == null) return l_DataSet;

                //l_DBCommandWrapper.AddInParameter ("@varchar_FundEventID", DbType.String, parameterRefRequestID);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@refRequestId", DbType.String, parameterRefRequestID);


                // Conection Time out
                //l_DBCommandWrapper.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationSettings.AppSettings ["LargeConnectionTimeOut"]);

                l_TableNames = new string[] { "Current","MRDAcctBalance" };

                // CurrentAccounts			- Table Name which will return all tRansaction belong to the fund event ID
                // Current					- Table Name which will return the transaction Sum..

                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);

                return l_DataSet;


                }
            catch
                {
                throw;
                }

            }
        //Removed method GetReissueTransactions()

        //		public static DataTable GetReissueTransactions (string parameterFundEventID)

        #region " Save Refund Request Re-Issues "

		

        public static bool SaveRefundReIssue(DataSet parameterDataSet)
            {

            string l_DisbursementID = string.Empty;
            string l_OldDisbursementID = string.Empty;
			string strError = string.Empty;
            DataTable l_Disbursements, l_DisbursementDetails, l_DisbursementWithHold, l_DisbursementRefunds;

            Database l_Database = null;
            DbConnection l_IDbConnection = null;
            DbTransaction l_IDbTransaction = null;

            bool l_TransactionFlag = false;  //This flag is used to keep track of whether DisbursemnetId 
            //is empty or not, if it is empty then rollback transaction and return false

            try
                {
                if (parameterDataSet == null) return false;

                l_Disbursements = parameterDataSet.Tables["DisbursementsDataTable"];
                l_DisbursementDetails = parameterDataSet.Tables["RefundsDataTable"];
                l_DisbursementWithHold = parameterDataSet.Tables["DisbursementWithholdingDataTable"];
                l_DisbursementRefunds = parameterDataSet.Tables["DisbursementRefundsDataTable"];

                if (l_Disbursements == null) return false;

                //  Now create connection to maintain Transaction.
                l_Database = DatabaseFactory.CreateDatabase("YRS");

                l_IDbConnection = l_Database.CreateConnection(); //.GetConnection();
                l_IDbConnection.Open();

                if (l_Database == null) return false;

                l_IDbTransaction = l_IDbConnection.BeginTransaction();

                foreach (DataRow l_DisbursementDataRow in l_Disbursements.Rows)
                    {
                    l_TransactionFlag = false;
                    if (l_DisbursementDataRow["UniqueID"].GetType().ToString() == "System.DBNull")
                        l_OldDisbursementID = "";
                    else
                        l_OldDisbursementID = l_DisbursementDataRow["UniqueID"].ToString();

                    l_DisbursementID = InsertDisbursements(l_DisbursementDataRow, l_Database, l_IDbTransaction);

                    // l_Disbursement ID is very important because all other tables are needed this UniqueID.
                    // So, If l_Disbursement ID is null then rollback Transaction.Otherwise gohead. 

                    if (l_DisbursementID.Trim() != string.Empty)
                        {
                        l_DisbursementDataRow["UniqueID"] = l_DisbursementID;
                        l_DisbursementDetails = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementDetailsDataTable"], l_DisbursementID, l_OldDisbursementID);

                        //by Aparna 25/07/07
                        l_DisbursementWithHold = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementWithholdingDataTable"], l_DisbursementID, l_OldDisbursementID);
                        l_DisbursementRefunds = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementRefundsDataTable"], l_DisbursementID, l_OldDisbursementID);

                        l_TransactionFlag = true;
                        // Now Disbursement ID is updated in all corresponding DataTable
                        }

                    //Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
                    if (l_TransactionFlag == false)
                        {
                        l_IDbTransaction.Rollback();
                        return false;
                        }


                    } // Updating Disbursement ID

                //Commented by priya,PhaseV Part III no need to save records into reissueTransaction table while reisuue disbursement
                //Removed code to Insert ReIssue transaction.
                // Removed code to Insert Refund Request. 
                // Removed code to Insert Refund  
                //END Priya comments

                // Now  3 DataTables are succesfully Inserted Now Insert Remaining DataTable..
                // Insert Disbursement Details.. 
                //PP:2010.02.09:YRS 5.0-1013 : Withdrawal re-issue rollover checks with no deductions,added one another if condition and set l_TransactionFlag = false inside if condition
                //l_TransactionFlag = false;
                if (l_DisbursementDetails != null && (l_DisbursementDetails.Rows.Count > 0))
                    {
                    foreach (DataRow l_DisbursementDetailsDataRow in l_DisbursementDetails.Rows)
                        {
                        InsertDisbursementDetails(l_DisbursementDetailsDataRow, l_Database, l_IDbTransaction);
                        }
                    }


                // Insert Disbursement WithHold Details. 
                //PP:2010.02.09:YRS 5.0-1013 : Withdrawal re-issue rollover checks with no deductions,added one another if condition and set l_TransactionFlag = false inside if condition
                if (l_DisbursementWithHold != null && (l_DisbursementWithHold.Rows.Count > 0))
                    {
                    foreach (DataRow l_WithHoldDataRow in l_DisbursementWithHold.Rows)
                        {
                        InsertDisbursementWithHold(l_WithHoldDataRow, l_Database, l_IDbTransaction);
                        }
                    }
				

                // Insert Disbursement Refunds Details. 
                ////PP:2010.02.09:YRS 5.0-1013 : Withdrawal re-issue rollover checks with no deductions,added one another if condition and set l_TransactionFlag = false inside if condition
                if (l_DisbursementRefunds != null && (l_DisbursementRefunds.Rows.Count > 0))
                    {
                    foreach (DataRow l_DisbursementRefundsDataRow in l_DisbursementRefunds.Rows)
                        {
                        if (l_DisbursementRefundsDataRow["DisbursementID"].ToString().Trim() != string.Empty)
                            {
                            InsertDisbursementRefunds(l_DisbursementRefundsDataRow, l_Database, l_IDbTransaction, Convert.ToString(l_Disbursements.Rows[0]["PersID"]));
                            }
                        }
                    }
				//16-Feb-2011 Priya	BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2
				//Insert MRDReissue records 
				if (l_DisbursementRefunds != null && l_DisbursementRefunds.Rows.Count > 0)
					{
					if (l_DisbursementRefunds.Rows[0]["RefRequestID"].ToString().Trim() != string.Empty)
						{
						strError = InsertMRDReissue(l_Database, l_IDbTransaction, Convert.ToString(l_DisbursementRefunds.Rows[0]["RefRequestID"]));

						if (strError != String.Empty)
							{
							throw new Exception(strError);
							}
						}
					}
                //Insert into ats1099Disbursement Table
                //PP:2010.02.09:YRS 5.0-1013 : Withdrawal re-issue rollover checks with no deductions,
                if (l_Disbursements != null && (l_Disbursements.Rows.Count > 0))
                    {
                    foreach (DataRow l_DisbursementRow in l_Disbursements.Rows)
                        {
                        Insert1099Disbursements(l_Database, l_IDbTransaction, l_DisbursementRow["UniqueID"].ToString().Trim());
                        }
                    }


                // All are DataTanles are succesfully Updated Now, do commit Transaction
                l_IDbTransaction.Commit();
				//l_IDbTransaction.Rollback();
                return true;
                }

            catch (SqlException sqlEx)
                {
                if (l_IDbTransaction != null)
                    {
                    l_IDbTransaction.Rollback();
                    }
                throw sqlEx;

                }
            catch (Exception ex)
                {
                if (l_IDbTransaction != null)
                    {
                    l_IDbTransaction.Rollback();
                    }
                throw ex;
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
        private static void Insert1099Disbursements(Database parameterDatabase, DbTransaction parameterTransaction, string disbursementID)
            {
            DbCommand l_DBCommandWrapper = null;

            try
                {
                if (parameterDatabase == null) return;

                //l_DBCommandWrapper = parameterDatabase.GetStoredProcCommandWrapper ("dbo.yrs_usp_Refunds_populate1099Values");
                l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_VD_Insert1099Disbursements");

                if (l_DBCommandWrapper == null) return;

                if (l_DBCommandWrapper != null)
                    {
                    parameterDatabase.AddInParameter(l_DBCommandWrapper, "@DisbursementId", DbType.String, disbursementID);
                    //l_DBCommandWrapper.AddInParameter ("@withdrawelType", DbType.String, "REF");
                    parameterDatabase.ExecuteNonQuery(l_DBCommandWrapper, parameterTransaction);
                    }

                }
            catch
                {
                throw;
                }
            }


        private static DataTable ApplyDisbursemnentID(DataTable parameterDataTable, string parameterDisbursementID, string parameterOldDisbursementID)
            {
            DataRow[] l_FoundDataRow;
            string l_QueryString = "";

            try
                {
                if (parameterDataTable == null) return null;

                l_QueryString = "DisbursementID = '" + parameterOldDisbursementID.Trim() + "'";

                l_FoundDataRow = parameterDataTable.Select(l_QueryString);

                if (l_FoundDataRow == null) return parameterDataTable;

                foreach (DataRow l_DataRow in l_FoundDataRow)
                    {
                    l_DataRow["DisbursementID"] = parameterDisbursementID;
                    }

                return parameterDataTable;

                }
            catch
                {
                throw;
                }
            }


        private static void InsertDisbursementRefunds(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction, string PersID)
            {
            DbCommand l_DBCommandWrapper;

            try
                {

                if (parameterDatabase == null) return;

                if (parameterDataRow == null) return;

                l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand("yrs_usp_VD_InsertDisbRefunds");//("dbo.yrs_usp_RR_InsertDisbRefunds");

                if (l_DBCommandWrapper == null) return;

                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_RefRequestID", DbType.String, Convert.ToString(parameterDataRow["RefRequestID"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_DisbursementID", DbType.String, Convert.ToString(parameterDataRow["DisbursementID"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_PersID", DbType.String, PersID);

                parameterDatabase.ExecuteNonQuery(l_DBCommandWrapper, parameterTransaction);


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


        private static void InsertDisbursementWithHold(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
            {

            DbCommand l_DBCommandWrapper;

            try
                {

                if (parameterDatabase == null) return;

                if (parameterDataRow == null) return;

                l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertDisbWithholding");

                if (l_DBCommandWrapper == null) return;

                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_DisbursementID", DbType.String, Convert.ToString(parameterDataRow["DisbursementID"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_WithholdingTypeCode", DbType.String, Convert.ToString(parameterDataRow["WithholdingTypeCode"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@numeric_Amount", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Amount"]));

                parameterDatabase.ExecuteNonQuery(l_DBCommandWrapper, parameterTransaction);


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



        private static void InsertDisbursementDetails(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
            {

            DbCommand l_DBCommandWrapper;

            try
                {

                if (parameterDatabase == null) return;

                if (parameterDataRow == null) return;

                l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertDisbursementDetails");

                if (l_DBCommandWrapper == null) return;

                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_DisbursementID", DbType.String, Convert.ToString(parameterDataRow["DisbursementID"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_AcctType", DbType.String, Convert.ToString(parameterDataRow["AcctType"]));
                //l_DBCommandWrapper.AddInParameter ("@varchar_AcctBreakdownType", DbType.Decimal, Convert.ToDecimal(parameterDataRow["AcctBreakdownType"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_AcctBreakdownType", DbType.String, Convert.ToString(parameterDataRow["AcctBreakdownType"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@integer_SortOrder", DbType.Int32, Convert.ToInt32(parameterDataRow["SortOrder"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@numeric_TaxablePrincipal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxablePrincipal"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@numeric_TaxableInterest", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxableInterest"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@numeric_NonTaxablePrincipal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["NonTaxablePrincipal"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@numeric_TaxWithheldPrincipal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxWithheldPrincipal"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@numeric_TaxWithheldInterest", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxWithheldInterest"]));

                parameterDatabase.ExecuteNonQuery(l_DBCommandWrapper, parameterTransaction);


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


        //Removed code of updateRefundRequest sp:yrs_usp_RR_UpdateRefundRequests
        //Removed code of InsertRefunds sp:yrs_usp_RR_InsertRefunds
        //Removed code of InsertRefundRequests sp:yrs_usp_RR_InsertRefundRequest
        //Removed code of InsertTransactions sp:yrs_usp_RR_InsertTransactionDetails
        //Removed code of InsertReissueTransactions sp:yrs_usp_RR_InsertReissueTransaction
        //Removed code of ApplyRefRequestID 

        private static string InsertDisbursements(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
            {

            DbCommand l_DBCommandWrapper;

            try
                {

                if (parameterDatabase == null) return string.Empty;

                if (parameterDataRow == null) return string.Empty;

                l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertDisbursements");

                if (l_DBCommandWrapper == null) return string.Empty;

                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_PayeeEntityID", DbType.String, Convert.ToString(parameterDataRow["PayeeEntityID"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_PayeeAddrID", DbType.String, Convert.ToString(parameterDataRow["PayeeAddrID"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_PayeeEntityTypeCode", DbType.String, Convert.ToString(parameterDataRow["PayeeEntityTypeCode"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_DisbursementType", DbType.String, Convert.ToString(parameterDataRow["DisbursementType"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_IrsTaxTypeCode", DbType.String, Convert.ToString(parameterDataRow["IrsTaxTypeCode"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_PaymentMethodCode", DbType.String, Convert.ToString(parameterDataRow["PaymentMethodCode"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_CurrencyCode", DbType.String, Convert.ToString(parameterDataRow["CurrencyCode"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_PersID", DbType.String, Convert.ToString(parameterDataRow["PersID"]));
                //Priya 11/30/2010 BT-592,YRS 5.0-1171 : Disbursement Ref Id not filled in 
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@Varchar_DisbursementRefID", DbType.String, Convert.ToString(parameterDataRow["DisbursementRefID"]));
                //End 11/30/2010 BT-592,YRS 5.0-1171 : Disbursement Ref Id not filled in 
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_Rollover", DbType.String, Convert.ToString(parameterDataRow["Rollover"]));
                //l_DBCommandWrapper.AddInParameter ("@varchar_Creator", DbType.String, "YRS");
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@numeric_TaxableAmount", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxableAmount"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@numeric_NonTaxableAmount", DbType.Decimal, Convert.ToDecimal(parameterDataRow["NonTaxableAmount"]));

                parameterDatabase.AddOutParameter(l_DBCommandWrapper, "@varchar_UniqueID", DbType.String, 100);

                parameterDatabase.ExecuteNonQuery(l_DBCommandWrapper, parameterTransaction);

                //return Convert.ToString (l_DBCommandWrapper.GetParameterValue ("@varchar_UniqueID"));
                return Convert.ToString(parameterDatabase.GetParameterValue(l_DBCommandWrapper, "@varchar_UniqueID"));

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

        #endregion
        //Priya 07-Sep-09
        public static DataSet GetReissueDisbursementDetails(string RefRequestsID)
            {
            //yrs_usp_VD_GetReissueDisbursementDetails
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            DbCommand l_DBCommandWrapper = null;
            try
                {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_VD_GetReissueDisbursementDetails");

                if (l_DBCommandWrapper == null) return null;

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@RefRequestID", DbType.String, RefRequestsID);

                // connection Time out.
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataSet = new DataSet();

                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, "DisbursementDetails");

                return l_DataSet;
                }
            catch (Exception ex)
                {
				    throw ex;
                }
            }

        //Priya : Made enhancement chnages
        public static DataSet GetRefundSchemas()
            {
            //Database l_DataBase = null;
            //DbCommand l_DBCommandWrapper = null;			
            DataSet l_DataSet = new DataSet();//= null;
            //string [] l_TableNames;

            try
                {
                l_DataSet.Tables.Add("Transactions");
                l_DataSet.Tables.Add("RolloverInstitutions");
                l_DataSet.Tables.Add("Disbursements");
                l_DataSet.Tables.Add("DisbursementDetails");
                l_DataSet.Tables.Add("DisbursementWithholding");
                l_DataSet.Tables.Add("DisbursementRefunds");

                //create structure for transaction table.
                l_DataSet.Tables["Transactions"].Columns.Add("UniqueID");
                l_DataSet.Tables["Transactions"].Columns.Add("PersID");
                l_DataSet.Tables["Transactions"].Columns.Add("FundEventID");
                l_DataSet.Tables["Transactions"].Columns.Add("YmcaID");
                l_DataSet.Tables["Transactions"].Columns.Add("AcctType");
                l_DataSet.Tables["Transactions"].Columns.Add("TransactType");
                l_DataSet.Tables["Transactions"].Columns.Add("AnnuityBasisType");
                l_DataSet.Tables["Transactions"].Columns.Add("MonthlyComp");
                l_DataSet.Tables["Transactions"].Columns.Add("PersonalPreTax");
                l_DataSet.Tables["Transactions"].Columns.Add("PersonalPostTax");
                l_DataSet.Tables["Transactions"].Columns.Add("YmcaPreTax");
                l_DataSet.Tables["Transactions"].Columns.Add("ReceivedDate");
                l_DataSet.Tables["Transactions"].Columns.Add("AccountingDate");
                l_DataSet.Tables["Transactions"].Columns.Add("TransactDate");
                l_DataSet.Tables["Transactions"].Columns.Add("FundedDate");
                l_DataSet.Tables["Transactions"].Columns.Add("TransmittalID");
                l_DataSet.Tables["Transactions"].Columns.Add("TransactionRefID");
                l_DataSet.Tables["Transactions"].Columns.Add("Created");
                l_DataSet.Tables["Transactions"].Columns.Add("Creator");
                l_DataSet.Tables["Transactions"].Columns.Add("Updated");
                l_DataSet.Tables["Transactions"].Columns.Add("Updater");


                //create structure for RolloverInstitutions table
                l_DataSet.Tables["RolloverInstitutions"].Columns.Add("UniqueID");
                l_DataSet.Tables["RolloverInstitutions"].Columns.Add("InstitutionName1");
                l_DataSet.Tables["RolloverInstitutions"].Columns.Add("InstitutionName2");
                l_DataSet.Tables["RolloverInstitutions"].Columns.Add("PlanType");

                //create structure for Disbursements table
                l_DataSet.Tables["Disbursements"].Columns.Add("UniqueID");
                l_DataSet.Tables["Disbursements"].Columns.Add("PayeeEntityID");
                l_DataSet.Tables["Disbursements"].Columns.Add("PayeeAddrID");
                l_DataSet.Tables["Disbursements"].Columns.Add("PayeeEntityTypeCode");
                l_DataSet.Tables["Disbursements"].Columns.Add("DisbursementType");
                l_DataSet.Tables["Disbursements"].Columns.Add("IrsTaxTypeCode");
                l_DataSet.Tables["Disbursements"].Columns.Add("TaxableAmount");
                l_DataSet.Tables["Disbursements"].Columns.Add("NonTaxableAmount");
                l_DataSet.Tables["Disbursements"].Columns.Add("PaymentMethodCode");
                l_DataSet.Tables["Disbursements"].Columns.Add("CurrencyCode");
                l_DataSet.Tables["Disbursements"].Columns.Add("BankID");
                l_DataSet.Tables["Disbursements"].Columns.Add("DisbursementNumber");
                l_DataSet.Tables["Disbursements"].Columns.Add("PersID");
                l_DataSet.Tables["Disbursements"].Columns.Add("DisbursementRefID");
                l_DataSet.Tables["Disbursements"].Columns.Add("Rollover");
                l_DataSet.Tables["Disbursements"].Columns.Add("Created");
                l_DataSet.Tables["Disbursements"].Columns.Add("Creator");
                l_DataSet.Tables["Disbursements"].Columns.Add("PersBankingID");

                //create structure for DisbursementDetails table
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("UniqueID");
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("DisbursementID");
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("AcctType");
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("AcctBreakdownType");
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("SortOrder");
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("TaxablePrincipal");
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("TaxableInterest");
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("NonTaxablePrincipal");
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("TaxWithheldPrincipal");
                l_DataSet.Tables["DisbursementDetails"].Columns.Add("TaxWithheldInterest");


                //create structure for DisbursementWithholding table
                l_DataSet.Tables["DisbursementWithholding"].Columns.Add("UniqueID");
                l_DataSet.Tables["DisbursementWithholding"].Columns.Add("DisbursementID");
                l_DataSet.Tables["DisbursementWithholding"].Columns.Add("WithholdingTypeCode");
                l_DataSet.Tables["DisbursementWithholding"].Columns.Add("Amount");
                l_DataSet.Tables["DisbursementWithholding"].Columns.Add("Created");
                l_DataSet.Tables["DisbursementWithholding"].Columns.Add("Creator");

                //create structure for DisbursementRefunds table
                l_DataSet.Tables["DisbursementRefunds"].Columns.Add("UiqueID");
                l_DataSet.Tables["DisbursementRefunds"].Columns.Add("RefRequestID");
                l_DataSet.Tables["DisbursementRefunds"].Columns.Add("DisbursementID");
                l_DataSet.Tables["DisbursementRefunds"].Columns.Add("Voided");
                l_DataSet.Tables["DisbursementRefunds"].Columns.Add("ActionType");
                l_DataSet.Tables["DisbursementRefunds"].Columns.Add("ReissuedDisbursements");

                //l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

                //if (l_DataBase == null) return null;

                //l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_VD_GetRefundSchemas");

                //if (l_DBCommandWrapper == null) return null;

                //l_DataSet = new DataSet ("RefundTransactionSchemas");

                ////l_TableNames = new string []{"Transactions", "RolloverInstitutions", "Disbursements", "DisbursementDetails", "DisbursementWithholding", "DisbursementRefunds", "ReissueTransaction"};
                //l_TableNames = new string []{"Transactions", "RolloverInstitutions", "Disbursements", "DisbursementDetails", "DisbursementWithholding", "DisbursementRefunds"};

                //l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet, l_TableNames);

                return l_DataSet;

                }
            catch
                {
                throw;
                }

            }

        //End 07-Sep-09

		//14-Feb-2011 Priya	BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2
		public static string InsertMRDReissue(Database parameterDatabase, DbTransaction parameterTransaction, string RefRequestID)
        {
		string l_string_Output = string.Empty;
			try
				{
				 DbCommand l_DBCommandWrapper;
          

                if (parameterDatabase == null) return string.Empty;

                
                l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand("yrs_usp_RR_InsertMRDRecordsForReissue");//("dbo.yrs_usp_RR_InsertDisbRefunds");

                if (l_DBCommandWrapper == null) return string.Empty;

				parameterDatabase.AddInParameter(l_DBCommandWrapper, "@refRequestId", DbType.String, RefRequestID);
				parameterDatabase.AddOutParameter(l_DBCommandWrapper, "@sOutput", DbType.String, 1000);

                parameterDatabase.ExecuteNonQuery(l_DBCommandWrapper, parameterTransaction);
				l_string_Output = Convert.ToString(parameterDatabase.GetParameterValue(l_DBCommandWrapper, "@sOutput"));
				

				return l_string_Output;
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

		//End 14-Feb-2011 Priya	BT-694:YRS 5.0-1217: New MRD Procesing - Phase 2



        }
    }
