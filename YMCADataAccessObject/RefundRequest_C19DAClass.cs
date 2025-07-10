//**************************************************************************************************************/
// Author: Manthan Rajguru
// Created on: 04/27/2020
// Summary of Functionality: Covid fucntionality data access layer
// Declared in Version: 20.8.1.2 | YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688) 

//**************************************************************************************************************
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
// Shiny C.                    | 05/05/2020   | 20.8.1.2        | YRS-AT-4874 -  COVID - Special withdrawal functionality needed in YRS processing screen due to CARE Act/COVID-19 (Track IT - 41688) 
// Megha Lad                   | 25/05/2020   | 20.8.1.2        | YRS-AT-4874 -  Save refund request processing (Resolve precision issue)
// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;

namespace YMCARET.YmcaDataAccessObject
{
    /// <summary>
    /// Summary description for RefundRequest_C19DAClass.
    /// </summary>
    public sealed class RefundRequest_C19DAClass
    {

        public static Decimal GetCovidAmountUsed(int fundNo)
        {
            Database db = null;
            DbCommand dbCommand = null;
            decimal covidAmountUsed = 0.00M;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return 0.00M;
                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_C19_GetCovidAmountUsed");
                if (dbCommand == null) return 0.00M;
                db.AddInParameter(dbCommand, "@INT_FundNo", DbType.Int32, fundNo);
                db.AddOutParameter(dbCommand, "@NUMERIC_CovidAmountExhausted", DbType.Double, 18);
                db.ExecuteNonQuery(dbCommand);
                if (db.GetParameterValue(dbCommand, "@NUMERIC_CovidAmountExhausted").GetType().ToString() != "System.DBNull")
                {
                    covidAmountUsed = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@NUMERIC_CovidAmountExhausted"));
                }
                return covidAmountUsed;

            }
            catch
            {
                throw;
            }
        }

        //START : SC | 2020.04.05 | YRS-AT-4874 | Added COVID related methods for Refund Processing screen
        /// <summary>
        /// This procedure will fetch following COVID related amounts
        /// 1. Total Covid amount exhausted by the partipant
        /// 2. Covid amount requested at the time of request
        /// 3. Tax Rate requested at the time of request
        /// 4. Max Covid Limit which participant can avail
        /// </summary>
        public static DataTable GetCovidAmountsForProcessing(string parameterRefundRequestID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_C19_GetCovidAmountsForProcessing");
                if (l_DbCommand == null) return null;
                l_DataBase.AddInParameter(l_DbCommand, "@VARCHAR_RefundRequestID", DbType.String, parameterRefundRequestID);
                l_DataSet = new DataSet("COVID Amounts");
                l_TableNames = new string[] { "COVID Amounts" };
                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);
                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["COVID Amounts"]);
                }

                return null;
            }
            catch
            {
                throw;
            }
        }       

        /// <summary>
        /// Updates Taxable, Non-taxable and Tax rate in the Covid Transactions Table after save refund processing
        /// </summary>
        public static void UpdateCovidTransactactionAfterProcessing(string RefundRequestID, decimal CovidTaxableAmount, decimal CovidNonTaxableAmount, decimal CovidTaxRate)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_C19_UpdateCovidTransaction");
                l_DataBase.AddInParameter(l_DbCommand, "@type", DbType.String, "WITHDRAWAL");
                l_DataBase.AddInParameter(l_DbCommand, "@transactionReferenceId", DbType.String, RefundRequestID);
                l_DataBase.AddInParameter(l_DbCommand, "@covidTaxableAmount", DbType.Decimal, CovidTaxableAmount);
                l_DataBase.AddInParameter(l_DbCommand, "@covidNonTaxableAmount", DbType.Decimal, CovidNonTaxableAmount);
                l_DataBase.AddInParameter(l_DbCommand, "@covidTaxRate", DbType.Decimal, CovidTaxRate);
                l_DataBase.ExecuteNonQuery(l_DbCommand);
            }
            catch
            {
                throw;
            }
        }
        //END : SC | 2020.04.05 | YRS-AT-4874 | Added COVID related methods for Refund Processing screen


        //START | ML | 2020.05.25 | YRS-AT-4874 | Save refund request processing 
        /// <summary>
        /// Resolve precision issue for column intTaxRate for atsRefRequests, atsRefRequestPerPlan
        /// </summary>
        #region " Save Refund Request Process "
        //Start - Added one more paramater FundEventStatusType for Plan Split Changes
        //,DataSet parameterDataSetForNewRequests,bool parameterNeedsNewRequests,string parameterInitialRefRequestId
        public static bool SaveRefundRequestProcessRefunds(DataSet parameterDataSet, string parameterPersonID, string parameterFundID, string paramterRefundType, bool parameterVested, bool parameterTerminated, bool parameterTookTDAccount, string parameterFundEventStatusType, DataSet parameterDataSetForNewRequests, bool parameterNeedsNewRequests, string parameterInitialRefRequestId, bool parameterIsMarket, decimal parameterTaxRate, string parameterPayeeName, string parameterRolloverinstitutionId, decimal parameterRollOverAmount, bool parameterIsActive, decimal parameterTaxable, decimal parameterNonTaxable,
            bool IsIRSOverride = false) // SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim 
        {

            string l_DisbursementID = "";
            string l_TransactionID = "";

            string l_OldDisbursementID = "";
            string l_OldTransactionID = "";

            DataTable l_Refunds;
            DataTable l_Disbursements;
            DataTable l_DisbursementDetails;
            DataTable l_Transaction;
            DataTable l_DisbursementWithHold;
            DataTable l_DisbursementRefunds;
            //*************
            DataTable l_MarketDatatable;
            //*************
            DataTable l_MrdRecordsDisbursements;

            DataRow l_NewRefRequest;
            DataTable l_NewRefRequestDetails;


            DataTable l_RefundRequest;
            DataTable dtAtsMrdRecordsUpdate;  //SB | 08/31/2016 | YRS-AT-3028 | DataTable for recalculated MRD records
            Database l_Database = null;

            DbConnection l_DbConnection = null;
            DbTransaction l_DbTransaction = null;
            string l_string_RequestError = "";
            string l_string_RefRequestID = string.Empty;

            string l_SubTableRequestID = string.Empty;
            //string l_RefundRequestID =string.Empty;

            bool l_TransactionFlag = false;  //--  This falg is used to Keep whether Main transaction is happening r not.
            // B'cos, Disbursemnet, Transaction & Refund is Compulsory for this Transaction. 
            // Otherwise Rolback the Transaction.
            string planType = "";

            try
            {

                if (parameterNeedsNewRequests == true)
                {
                    if (parameterDataSetForNewRequests == null) return false;

                }
                if (parameterDataSet == null) return false;

                l_Disbursements = parameterDataSet.Tables["DisbursementsDataTable"];
                l_Transaction = parameterDataSet.Tables["TransactionsDataTable"];
                l_Refunds = parameterDataSet.Tables["RefundsDataTable"];
                l_DisbursementDetails = parameterDataSet.Tables["RefundsDataTable"];
                l_DisbursementWithHold = parameterDataSet.Tables["DisbursementWithholdingDataTable"];
                l_DisbursementRefunds = parameterDataSet.Tables["DisbursementRefundsDataTable"];

                l_RefundRequest = parameterDataSet.Tables["RefundRequestDataTable"];
                //*************
                l_MarketDatatable = parameterDataSet.Tables["MarketDataTable"];
                //***********

                //neeraj 17-11-2010 Bt-664 : MrdRecordsDisbursements
                l_MrdRecordsDisbursements = parameterDataSet.Tables["MrdRecordsDisbursements"];
                dtAtsMrdRecordsUpdate = parameterDataSet.Tables["AtsMrdRecordsWithDrawalUpdation"];  //SB | 08/31/2016 | YRS-AT-3028 | Adding Recalculated RMD values as a parameter to existing parameters
                //Anudeep:27.12.2012 Bt-1538 Hardship Refund Process Deadlock Situation
                if ((paramterRefundType.Trim() == "REG" || paramterRefundType.Trim() == "PERS") && (l_Refunds.Rows[0]["RefRequestsID"] != null))
                {
                    planType = GetPlanType(l_Refunds.Rows[0]["RefRequestsID"].ToString());
                }

                if ((l_Disbursements != null) && (l_Transaction != null) && (l_Refunds != null) && (l_RefundRequest != null))
                {

                    //  Now create connection to maintain Transaction.
                    l_Database = DatabaseFactory.CreateDatabase("YRS");

                    l_DbConnection = l_Database.CreateConnection();
                    l_DbConnection.Open();

                    if (l_Database == null) return false;

                    l_DbTransaction = l_DbConnection.BeginTransaction();


                    l_TransactionFlag = false;
                    // we need to cancel the existing request,create another request and then process the same in casse of HARD request
                    if (parameterNeedsNewRequests == true)
                    {

                        if (paramterRefundType.Trim() == "HARD")
                        {
                            DeletePendingRefundRequest(parameterInitialRefRequestId, l_Database, l_DbTransaction);
                        }
                        else
                        {
                            //Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.(Its Hard coded as "YRS" because its executing cancel proc for internal process)
                            CancelPendingRefundRequest(parameterInitialRefRequestId, l_Database, l_DbTransaction, "YRS");
                        }
                        l_NewRefRequest = parameterDataSetForNewRequests.Tables[0].Rows[0];


                        l_string_RequestError = InsertNewRefundRequests(l_NewRefRequest, parameterDataSetForNewRequests, l_Database, l_DbTransaction, IsIRSOverride); // SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim 


                        if (l_string_RequestError == "")
                        {
                            l_TransactionFlag = true;

                        }
                        else
                        {
                            l_TransactionFlag = false;
                        }
                        if (l_TransactionFlag == false)
                        {
                            l_DbTransaction.Rollback();
                            return false;	// Return with Error.
                        }
                    }

                    l_TransactionFlag = false;
                    if (parameterNeedsNewRequests == true)
                    {

                        //						l_NewRefRequestDetails = parameterDataSetForNewRequests.Tables[1];
                        //						foreach(DataRow l_NewRefRequestDetailsRow in l_NewRefRequestDetails.Rows)
                        //						{
                        //							if(l_NewRefRequestDetailsRow != null)
                        //							{
                        //								l_string_RequestError = InsertNewRefundRequestDetails(l_NewRefRequestDetailsRow,l_Database, l_DbTransaction);
                        //							}
                        //							
                        //							
                        //						}
                        //						if(l_string_RequestError == "")
                        //						{
                        //							l_TransactionFlag = true;
                        //							
                        //						}
                        //						else
                        //							
                        //						{
                        //							l_TransactionFlag = false;
                        //						}
                        //						if (l_TransactionFlag == false)
                        //						{
                        //							l_DbTransaction.Rollback ();
                        //							return false;	// Return with Error.
                        //						} 
                    }

                    l_TransactionFlag = false;
                    foreach (DataRow l_DisbursementDataRow in l_Disbursements.Rows)
                    {

                        if (l_DisbursementDataRow != null)
                        {
                            if (l_DisbursementDataRow["UniqueID"].GetType().ToString() == "System.DBNull")
                                l_OldDisbursementID = "";
                            else
                                l_OldDisbursementID = l_DisbursementDataRow["UniqueID"].ToString();


                            l_DisbursementID = InsertDisbursements(l_DisbursementDataRow, l_Database, l_DbTransaction);

                            // l_Disbursement ID is very important because all other tables are needed this UniqueID.
                            // So, If l_Disbursement ID is null then Ignore the Transaction.Otherwise gohead. 

                            if (l_DisbursementID.Trim() != string.Empty)
                            {
                                l_Disbursements = UpdateDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementsDataTable"], l_DisbursementID, l_OldDisbursementID);
                                l_DisbursementDetails = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementDetailsDataTable"], l_DisbursementID, l_OldDisbursementID);
                                l_Refunds = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["RefundsDataTable"], l_DisbursementID, l_OldDisbursementID);
                                l_DisbursementWithHold = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementWithholdingDataTable"], l_DisbursementID, l_OldDisbursementID);
                                l_DisbursementRefunds = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementRefundsDataTable"], l_DisbursementID, l_OldDisbursementID);
                                //neeraj 19-11-2010 BT-664
                                UpdateDisbursemnentIDInMrdRecordsDisbursement((DataTable)parameterDataSet.Tables["MrdRecordsDisbursements"], l_DisbursementID, l_OldDisbursementID);

                                l_TransactionFlag = true;
                                // Now Disbursement ID is updated in all corresponding DataTable
                            }

                        }


                    } // Updating Disbursement ID

                    // Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
                    if (l_TransactionFlag == false)
                    {
                        l_DbTransaction.Rollback();
                        return false;	// Return with Error.
                    }


                    //Now we need Transaction ID for Refunds DataTable, so wee needs to Get Transaction ID. 
                    l_TransactionFlag = false;
                    foreach (DataRow l_TransactionDataRow in l_Transaction.Rows)
                    {
                        if (l_TransactionDataRow != null)
                        {

                            if (l_TransactionDataRow["UniqueID"].GetType().ToString() == "System.DBNull")
                                l_OldTransactionID = "";
                            else
                                l_OldTransactionID = l_TransactionDataRow["UniqueID"].ToString();


                            l_TransactionID = InsertTransactions(l_TransactionDataRow, l_Database, l_DbTransaction);

                            if (l_TransactionID.Trim() != string.Empty)
                            {
                                l_Refunds = ApplyTransactionID(l_Refunds, l_TransactionID, l_OldTransactionID);
                                l_TransactionFlag = true;
                            }

                            // Now Transaction ID is updated in all corresponding DataTable
                        }

                    }

                    // Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
                    if (l_TransactionFlag == false)
                    {
                        l_DbTransaction.Rollback();
                        return false;	// Return with Error.
                    }


                    // Now we needs to RefundsID for Disbursement Refunds.
                    l_TransactionFlag = false;
                    foreach (DataRow l_RefundsDataRow in l_Refunds.Rows)
                    {

                        InsertAtsRefundsRecords(l_RefundsDataRow, parameterIsMarket, l_Database, l_DbTransaction);
                        l_TransactionFlag = true;
                    }

                    // Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
                    if (l_TransactionFlag == false)
                    {
                        l_DbTransaction.Rollback();
                        return false;	// Return with Error.
                    }


                    // Now  3 DataTables are succesfully Inserted Now Insert Remaining DataTable..

                    // Insert Disbursement Details.. 
                    if (l_DisbursementDetails != null)
                    {
                        foreach (DataRow l_DisbursementDetailsDataRow in l_DisbursementDetails.Rows)
                        {
                            InsertDisbursementDetails(l_DisbursementDetailsDataRow, l_Database, l_DbTransaction);
                        }

                    }

                    // Insert Disbursement WithHold Details. 
                    if (l_DisbursementWithHold != null)
                    {
                        foreach (DataRow l_WothHoldDataRow in l_DisbursementWithHold.Rows)
                        {
                            InsertDisbursementWithHold(l_WothHoldDataRow, l_Database, l_DbTransaction);
                        }
                    }


                    // Insert Disbursement Refunds Details. 
                    if (l_DisbursementRefunds != null)
                    {
                        foreach (DataRow l_DisbursementRefundsDataRow in l_DisbursementRefunds.Rows)
                        {
                            InsertDisbursementRefunds(l_DisbursementRefundsDataRow, l_Database, l_DbTransaction);
                        }
                    }


                    // Update RefundRequest
                    l_TransactionFlag = false;
                    if (l_RefundRequest != null)
                    {
                        foreach (DataRow l_RefundRequestDataRow in l_RefundRequest.Rows)
                        {
                            if (l_RefundRequestDataRow != null)
                            {
                                if (Convert.ToString(l_RefundRequestDataRow["RequestStatus"]).Trim() == "DISB")
                                {
                                    UpdateRefundRequests(l_RefundRequestDataRow, l_Database, l_DbTransaction);
                                    l_TransactionFlag = true;
                                }
                                //Added by Ashish on 21-Oct-2008 ,get RefRquestId for DLIN changes
                                l_string_RefRequestID = l_RefundRequestDataRow["UniqueId"].ToString();
                            }
                        }
                        /*added by ruchi to change the status to withdrawn when disbursements are made*/
                        //NP:IVP2:2008.10.23 - Removing out parameter that provided Fund Event status

                    }
                    //Insertion of negative and positive entries for Market Based Withdrawal
                    if (parameterIsMarket == true)
                    {
                        l_TransactionFlag = false;
                        if (l_MarketDatatable != null)
                        {
                            foreach (DataRow l_MarketDataRow in l_MarketDatatable.Rows)
                            {
                                InsertMarketData(l_MarketDataRow, parameterFundID, parameterPersonID, l_string_RefRequestID, parameterTaxRate, parameterPayeeName, l_DisbursementID, l_Database, l_DbTransaction);
                                l_TransactionFlag = true;
                            }

                            //Deffered payment  entry to do 20/11/2009 in atsRefDeferredPayment

                            InsertDefferedPaymentData(l_string_RefRequestID, l_Database, l_DbTransaction);

                            //Modified for insertion into atsRefRollOverInstitutionData 02-02-2010
                            //RefRolloverinstitiotions entry.
                            //							if (parameterRolloverinstitutionId.ToString()==string.Empty)
                            //							{
                            //								parameterRolloverinstitutionId=l_Disbursements.Rows[0]["PayeeEntityID"].ToString();
                            //							}
                            //							InsertRefRollOverInstitutionData(l_string_RefRequestID, parameterRolloverinstitutionId, parameterRollOverAmount,parameterIsActive,parameterTaxable,parameterNonTaxable,l_Database,l_DbTransaction);
                            //Modified for insertion into atsRefRollOverInstitutionData 02-02-2010
                            if (parameterRolloverinstitutionId.ToString() != string.Empty)
                            {
                                InsertRefRollOverInstitutionData(l_string_RefRequestID, parameterRolloverinstitutionId, parameterRollOverAmount, parameterIsActive, parameterTaxable, parameterNonTaxable, l_Database, l_DbTransaction);
                            }
                            //Modified for insertion into atsRefRollOverInstitutionData 02-02-2010





                        }


                    }
                    //Moved here to update the fundevent status after the market data is inserted-Amit Dec 30,2009

                    if (l_RefundRequest != null)
                    {
                        ChangeStatus(parameterPersonID, parameterFundID, paramterRefundType, parameterVested, parameterTerminated, parameterTookTDAccount, parameterFundEventStatusType, l_Database, l_DbTransaction);
                        UpdateStagingInterestRecord(parameterFundID, l_string_RefRequestID, l_Database, l_DbTransaction);

                        //START: PPP | 11/16/2016 | YRS-AT-3146 | Fee will be charged by account hierarchy if "WITHDRAWAL_FEE_PRORATION_SWITCH" key is not present in the DB or value is "OFF" else ("ON") by old proration wise through "yrs_usp_RR_Save_WithdrawalProcessingFee"
                        //InsertWithdrawalProcessingFee(l_string_RefRequestID, l_Database, l_DbTransaction); // SR | 2016.06.07 | YRS-AT-2962 | Insert withdrawal processing details
                        YMCACommonDAClass.ChargeFee(l_string_RefRequestID, YMCAObjects.Module.Withdrawal, l_Database, l_DbTransaction);
                        //END: PPP | 11/16/2016 | YRS-AT-3146 | Fee will be charged by account hierarchy if "WITHDRAWAL_FEE_PRORATION_SWITCH" key is not present in the DB or value is "OFF" else ("ON") by old proration wise through "yrs_usp_RR_Save_WithdrawalProcessingFee"
                    }
                    //Moved here to update the fundevent status after the market data is inserted-Amit Dec 30,2009
                    //Insertion of negative and positive entries for Market Based Withdrawal

                    //Priya P commented SR code of termiantion watcher to undo release from 12.7.0 patch
                    //SR:2012.08.06 :  BT-957/YRS 5.0-1484 : Termination Watcher
                    //Anudeep:30-10-2012 reverted Changes as per Observations 
                    //Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 

                    //Anudeep:27.12.2012 Bt-1538 Hardship Refund Process Deadlock Situation
                    if (paramterRefundType.Trim() == "REG" || paramterRefundType.Trim() == "PERS")
                    {
                        if (planType == "RETIREMENT&SAVINGS" || planType == "SAVINGS&RETIREMENT")
                        {
                            planType = "BOTH";
                        }
                        //AA:2014.03.25 : BT-957/YRS 5.0-1484 : Added Refrequest ID parameter to check unfunded transaction exists for the refunded accounts
                        TerminationWatcherDA.UpdateTerminationWatcher(parameterPersonID, parameterFundID, "Withdrawal", planType, l_DbTransaction, l_Database, l_string_RefRequestID);
                        //End, SR:2012.08.06 :  BT-957/YRS 5.0-1484 : Termination Watcher
                    }
                    // Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
                    if (l_TransactionFlag == false)
                    {
                        l_DbTransaction.Rollback();
                        return false;	// Return with Error.
                    }


                    // Update the Status.

                    // Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.

                    //1099 population
                    l_TransactionFlag = false;
                    if (l_Disbursements != null)
                    {
                        foreach (DataRow dr in l_Disbursements.Rows)
                        {
                            if (Convert.ToString(dr["PayeeEntityTypeCode"]).Trim().ToUpper() == "ROLINS")
                            {
                                paramterRefundType = "ROLINS";
                            }
                            else
                            {
                                paramterRefundType = paramterRefundType;
                            }
                            l_string_RequestError = Populate1099Values(Convert.ToString(dr["UniqueID"]), paramterRefundType, l_Database, l_DbTransaction);
                            if (l_string_RequestError == "") { l_TransactionFlag = true; }
                            else { l_TransactionFlag = false; }

                        }
                    }

                    if (l_TransactionFlag == false)
                    {
                        l_DbTransaction.Rollback();
                        return false;	// Return with Error.
                    }

                    // neeraj 17-11-2010 BT-664 : insert mrdrecord and disbursement linkages data
                    //IB: BT:681  Unable to process the Savings Plan refund. Shows error page.
                    // l_TransactionFlag = false;
                    if (l_MrdRecordsDisbursements != null)
                    {
                        //IB: BT:681  Unable to process the Savings Plan refund. Shows error page.
                        if (l_MrdRecordsDisbursements.Rows.Count > 0)
                            l_TransactionFlag = false;
                        //BT 729:- Application creating record in AtsMrdRecordsDisbursements with zero in mnyPaidAmount column.
                        //'2011.June.10       Imran       BT:853   -Mrd Linkage with rollover disbursement with paidvalue=0  
                        //foreach (DataRow dr in l_MrdRecordsDisbursements.Select("mnyPaidAmount>0"))
                        foreach (DataRow dr in l_MrdRecordsDisbursements.Rows)
                        {
                            l_string_RequestError = InsertMrdRecordsDisbursements(dr, l_Database, l_DbTransaction);
                            if (l_string_RequestError == "") { l_TransactionFlag = true; }
                            else { l_TransactionFlag = false; }

                        }
                    }

                    if (l_TransactionFlag == false)
                    {
                        l_DbTransaction.Rollback();
                        return false;	// Return with Error.
                    }
                    // START : SB | 08/31/2016 | YRS-AT-3028 | Checking the MRD Values changed or not, If Changed Updation method called
                    if (HelperFunctions.isNonEmpty(dtAtsMrdRecordsUpdate))
                    {
                        if (dtAtsMrdRecordsUpdate.Rows.Count > 0)
                            l_TransactionFlag = false;
                        foreach (DataRow drAtsMrdRecord in dtAtsMrdRecordsUpdate.Rows)
                        {
                            if (Convert.ToString(drAtsMrdRecord["Remarks"]).Trim().ToUpper() == "U")
                                UpdateAtsMrdRecords(Convert.ToInt32(drAtsMrdRecord["intuniqueid"].ToString()), Convert.ToDecimal(drAtsMrdRecord["mnyRmdTaxableAmount"].ToString()), Convert.ToDecimal(drAtsMrdRecord["mnyRmdNonTaxableAmount"].ToString()), l_Database, l_DbTransaction);
                            if (l_string_RequestError == "") { l_TransactionFlag = true; }
                            else { l_TransactionFlag = false; }

                        }
                    }

                    if (l_TransactionFlag == false)
                    {
                        l_DbTransaction.Rollback();
                        return false;	// Return with Error.
                    }
                    // END : SB | 08/31/2016 | YRS-AT-3028 | Checking the MRD Values changed or not, If Changed Updation method called

                    // All are DataTables are succesfully Updated Now, do commit Transaction
                    l_DbTransaction.Commit();
                    return true;


                }
                else
                    return true;


            }
            catch (SqlException sqlEx)
            {
                if (l_DbTransaction != null)
                {
                    l_DbTransaction.Rollback();
                }
                throw sqlEx;

            }
            catch (Exception ex)
            {
                if (l_DbTransaction != null)
                {
                    l_DbTransaction.Rollback();
                }
                throw ex;
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
        // START : SB | 08/31/2016 | YRS-AT-3028 | Updating MRD Records with new changed values  
        private static bool UpdateAtsMrdRecords(int MRDUniqueId, decimal mnyRMDTaxableAmount, decimal mnyRMDNonTaxableAmount, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand InsertCommandWrapper = null;
            try
            {
                if (parameterDatabase == null) return false;
                InsertCommandWrapper = parameterDatabase.GetStoredProcCommand("yrs_usp_RMD_UpdateParticipantRMDforSolePrimaryBeneficiary");
                InsertCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                if (InsertCommandWrapper == null) return false;

                parameterDatabase.AddInParameter(InsertCommandWrapper, "@intMRDUniqueId", DbType.Int32, MRDUniqueId);
                parameterDatabase.AddInParameter(InsertCommandWrapper, "@decRMDTaxableAmount", DbType.Decimal, mnyRMDTaxableAmount);
                parameterDatabase.AddInParameter(InsertCommandWrapper, "@decRMDNonTaxableAmount", DbType.Decimal, mnyRMDNonTaxableAmount);

                parameterDatabase.ExecuteNonQuery(InsertCommandWrapper, parameterTransaction);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        // END : SB | 08/31/2016 | YRS-AT-3028 | Updating MRD Records with new changed values
        //neeraj 17-11-2010 BT-664: insert data in AtsMrdRecordsDisbursements 
        private static string InsertMrdRecordsDisbursements(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return null;

                if (parameterDataRow == null) return null;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertMrdRecordsDisbursements");

                if (l_DbCommand == null) return null;

                parameterDatabase.AddInParameter(l_DbCommand, "@guiDisbursementID", DbType.String, Convert.ToString(parameterDataRow["guiDisbursementId"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@intMrdTrackingId", DbType.Int32, Convert.ToString(parameterDataRow["intMrdTrackingId"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@mnyPaidAmount", DbType.Decimal, Convert.ToString(parameterDataRow["mnyPaidAmount"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@mnyTaxablePaidAmount", DbType.Decimal, Convert.ToString(parameterDataRow["mnyTaxablePaidAmount"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@mnyNonTaxablePaidAmount", DbType.Decimal, Convert.ToString(parameterDataRow["mnyNonTaxablePaidAmount"]));

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);

                return "";
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

        //End - Added one more paramater FundEventStatusType for Plan Split Changes
        private static DataTable ApplyTransactionID(DataTable parameterDataTable, string parameterTransactionID, string parameterOldTransactionID)
        {
            DataRow[] l_FoundDataRow;
            string l_QueryString = "";

            try
            {
                if (parameterDataTable == null) return null;

                l_QueryString = "TransactID = '" + parameterOldTransactionID.Trim() + "'";

                l_FoundDataRow = parameterDataTable.Select(l_QueryString);

                if (l_FoundDataRow == null) return parameterDataTable;

                foreach (DataRow l_DataRow in l_FoundDataRow)
                {
                    l_DataRow["TransactID"] = parameterTransactionID;
                }

                return parameterDataTable;

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
        private static DataTable UpdateDisbursemnentID(DataTable parameterDataTable, string parameterDisbursementID, string parameterOldDisbursementID)
        {
            DataRow[] l_FoundDataRow;
            string l_QueryString = "";

            try
            {
                if (parameterDataTable == null) return null;

                l_QueryString = "UniqueId = '" + parameterOldDisbursementID.Trim() + "'";

                l_FoundDataRow = parameterDataTable.Select(l_QueryString);

                if (l_FoundDataRow == null) return parameterDataTable;

                foreach (DataRow l_DataRow in l_FoundDataRow)
                {
                    l_DataRow["UniqueId"] = parameterDisbursementID;
                }

                return parameterDataTable;

            }
            catch
            {
                throw;
            }
        }

        private static DataTable UpdateDisbursemnentIDInMrdRecordsDisbursement(DataTable parameterDataTable, string parameterDisbursementID, string parameterOldDisbursementID)
        {
            DataRow[] l_FoundDataRow;
            string l_QueryString = "";

            try
            {
                if (parameterDataTable == null) return null;

                l_QueryString = "guiDisbursementId = '" + parameterOldDisbursementID.Trim() + "'";

                l_FoundDataRow = parameterDataTable.Select(l_QueryString);

                if (l_FoundDataRow == null) return parameterDataTable;

                foreach (DataRow l_DataRow in l_FoundDataRow)
                {
                    l_DataRow["guiDisbursementId"] = parameterDisbursementID;
                }

                return parameterDataTable;

            }
            catch
            {
                throw;
            }
        }

        private static void UpdateRefundRequests(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return;

                if (parameterDataRow == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateRefundRequests_C19");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_UniqueID", DbType.String, Convert.ToString(parameterDataRow["UniqueID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_NonTaxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["NonTaxable"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_Taxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Taxable"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_Amount", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Gross Amt."]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_Tax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Tax"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_Deductions", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Deductions"]));

                // Change By:Preeti On:2Mar06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefundType", DbType.String, Convert.ToString(parameterDataRow["RefundTypeHeader"]));
                // Change By:Preeti On:2Mar06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_TaxRate", DbType.Decimal, Convert.ToString(parameterDataRow["TaxRate"]));

                //Added By Parveen To Set RolloverOptions and Rollover Amount on 15 Dec 2009
                if (parameterDataRow["RolloverOptions"].ToString().Length != 0)
                {
                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RolloverOptions", DbType.String, Convert.ToString(parameterDataRow["RolloverOptions"]));
                }
                else
                {
                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RolloverOptions", DbType.String, DBNull.Value);
                }
                if (parameterDataRow["FirstRolloverAmt"].ToString().Length != 0)
                {
                    parameterDatabase.AddInParameter(l_DbCommand, "@numeric_FirstRollover", DbType.Decimal, Convert.ToDecimal(parameterDataRow["FirstRolloverAmt"]));
                }
                else
                {
                    parameterDatabase.AddInParameter(l_DbCommand, "@numeric_FirstRollover", DbType.Decimal, DBNull.Value);
                }
                if (parameterDataRow["TotalRolloverAmt"].ToString().Length != 0)
                {
                    parameterDatabase.AddInParameter(l_DbCommand, "@numeric_TotalRollover", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TotalRolloverAmt"]));
                }
                else
                {
                    parameterDatabase.AddInParameter(l_DbCommand, "@numeric_TotalRollover", DbType.Decimal, DBNull.Value);
                }
                //Added By Parveen To Set RolloverOptions and Rollover Amount on 15 Dec 2009

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


            }

            catch
            {
                throw;
            }

        }


        private static void InsertDisbursementRefunds(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return;

                if (parameterDataRow == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertDisbRefunds");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefRequestID", DbType.String, Convert.ToString(parameterDataRow["RefRequestID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbursementID", DbType.String, Convert.ToString(parameterDataRow["DisbursementID"]));

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


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

            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return;

                if (parameterDataRow == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertDisbWithholding");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbursementID", DbType.String, Convert.ToString(parameterDataRow["DisbursementID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_WithholdingTypeCode", DbType.String, Convert.ToString(parameterDataRow["WithholdingTypeCode"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_Amount", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Amount"]));

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


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

            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return;

                if (parameterDataRow == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertDisbursementDetails");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbursementID", DbType.String, Convert.ToString(parameterDataRow["DisbursementID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AcctType", DbType.String, Convert.ToString(parameterDataRow["AcctType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AcctBreakdownType", DbType.String, Convert.ToString(parameterDataRow["AcctBreakdownType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@integer_SortOrder", DbType.Int32, Convert.ToInt32(parameterDataRow["SortOrder"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_TaxablePrincipal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxablePrincipal"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_TaxableInterest", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxableInterest"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_NonTaxablePrincipal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["NonTaxablePrincipal"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_TaxWithheldPrincipal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxWithheldPrincipal"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_TaxWithheldInterest", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxWithheldInterest"]));

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


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

        private static void InsertMarketData(DataRow parameterMarketDataRow, string parameterFundID, string parameterPersonID, string parameterRefRequestsID, decimal parameterTaxRate, string paramaterPayeeName, string parameterDisbursementId, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return;

                if (parameterMarketDataRow == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertMarketTransactions");

                if (l_DbCommand == null) return;

                // parameterDatabase.AddInParameter(l_DbCommand,"@varchar_DisbursementID", DbType.String, Convert.ToString (parameterDataRow["DisbursementID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundEventID", DbType.String, parameterFundID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PersID", DbType.String, parameterPersonID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AnnuityBasicTypeAll", DbType.String, Convert.ToString(parameterMarketDataRow["AnnuityBasisType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_PersonalPreTax", DbType.Decimal, Convert.ToDecimal(parameterMarketDataRow["Taxable"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_PersonalPostTax", DbType.Decimal, Convert.ToDecimal(parameterMarketDataRow["Non-Taxable"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_YmcaPreTax", DbType.Decimal, Convert.ToDecimal(parameterMarketDataRow["YMCATaxable"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_Plantype", DbType.String, Convert.ToString(parameterMarketDataRow["Plantype"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransactionRefID", DbType.String, Convert.ToString(parameterMarketDataRow["TransactionRefId"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefRequestsID", DbType.String, parameterRefRequestsID);
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_TaxRate ", DbType.Decimal, Convert.ToDecimal(parameterTaxRate));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PayeeName  ", DbType.String, Convert.ToString(paramaterPayeeName));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbursementID ", DbType.String, Convert.ToString(parameterDisbursementId));





                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


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

        private static void InsertDefferedPaymentData(string parameterRefRequestsID, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefdRequestDeferredPaymentSchedule");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefRequestsID", DbType.String, parameterRefRequestsID);
                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


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
        private static void InsertRefRollOverInstitutionData(string parameterRefRequestsID, string parameterRolloverinstitutionId, decimal parameterRollOverAmount, bool parameterIsActive, decimal parameterTaxable, decimal parameterNonTaxable, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefRollOverInstitution");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefRequestsID", DbType.String, parameterRefRequestsID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_guiRolloverinstitutionId", DbType.String, Convert.ToString(parameterRolloverinstitutionId));
                // parameterDatabase.AddInParameter(l_DbCommand,"@varchar_intDeferredPaymentRefNo", DbType.Int16,  parameterDeferredPaymentRefNo);
                parameterDatabase.AddInParameter(l_DbCommand, "@decimal_mnyRollOverAmount", DbType.Decimal, parameterRollOverAmount);
                parameterDatabase.AddInParameter(l_DbCommand, "@decimal_bitIsActive", DbType.Boolean, parameterIsActive);
                parameterDatabase.AddInParameter(l_DbCommand, "@decimal_mnyTaxablePercentage", DbType.Decimal, parameterTaxable);
                parameterDatabase.AddInParameter(l_DbCommand, "@decimal_mnyNonTaxablePercentage", DbType.Decimal, parameterNonTaxable);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


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

        private static void InsertAtsRefundsRecords(DataRow parameterDataRow, bool parameterIsMarket, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand;
            int int_Market = 0;


            try
            {

                if (parameterDatabase == null) return;

                if (parameterIsMarket == true)
                {
                    int_Market = 1;
                }

                if (parameterDataRow == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefunds_C19");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefRequestsID", DbType.String, Convert.ToString(parameterDataRow["RefRequestsID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AcctType", DbType.String, Convert.ToString(parameterDataRow["AcctType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_Taxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Taxable"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_NonTaxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["NonTaxable"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_Tax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Tax"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_TaxRate", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxRate"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_Payee", DbType.String, Convert.ToString(parameterDataRow["Payee"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RequestType", DbType.String, Convert.ToString(parameterDataRow["RequestType"]));

                //				if (Convert.ToString(parameterDataRow["TransactID"].ToString()="0"))
                //				{
                //					 parameterDatabase.AddInParameter(l_DbCommand,"@varchar_TransactID", DbType.String, System.DBNull.Value);
                //				}
                //				else
                //				{
                //					 parameterDatabase.AddInParameter(l_DbCommand,"@varchar_TransactID", DbType.String, Convert.ToString (parameterDataRow["TransactID"]));
                //				}

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransactID", DbType.String, Convert.ToString(parameterDataRow["TransactID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AnnuityBasisType", DbType.String, Convert.ToString(parameterDataRow["AnnuityBasisType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbursementID", DbType.String, Convert.ToString(parameterDataRow["DisbursementID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@int_Market", DbType.String, int_Market);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


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


        private static string InsertTransactions(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return string.Empty;

                if (parameterDataRow == null) return string.Empty;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertTransactionDetails");

                if (l_DbCommand == null) return string.Empty;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, Convert.ToString(parameterDataRow["PersID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundEventID", DbType.String, Convert.ToString(parameterDataRow["FundEventID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AcctType", DbType.String, Convert.ToString(parameterDataRow["AcctType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransactType", DbType.String, Convert.ToString(parameterDataRow["TransactType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AnnuityBasisType", DbType.String, Convert.ToString(parameterDataRow["AnnuityBasisType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransactionRefID", DbType.String, Convert.ToString(parameterDataRow["TransactionRefID"]));
                //Added by Hafiz on Sep 24th 2008
                //For inserting ‘current date’ TransactDate, this is in order to handle the new parameter added for QDRO.
                //*********
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransactDate", DbType.String, Convert.ToString(DateTime.Now.Date));
                //*********
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_MonthlyComp", DbType.Decimal, Convert.ToDecimal(parameterDataRow["MonthlyComp"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_PersonalPreTax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PersonalPreTax"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_PersonalPostTax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PersonalPostTax"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_YmcaPreTax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["YmcaPreTax"]));
                // parameterDatabase.AddInParameter(l_DbCommand,"@varchar_Creator", DbType.String, "YRS");

                parameterDatabase.AddOutParameter(l_DbCommand, "@varchar_UniqueID", DbType.String, 100);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);
                if (parameterDatabase.GetParameterValue(l_DbCommand, "@varchar_UniqueID").GetType().ToString() != "System.DBNull")
                    return Convert.ToString(parameterDatabase.GetParameterValue(l_DbCommand, "@varchar_UniqueID"));
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


        private static string InsertDisbursements(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return string.Empty;

                if (parameterDataRow == null) return string.Empty;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertDisbursements");

                if (l_DbCommand == null) return string.Empty;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PayeeEntityID", DbType.String, Convert.ToString(parameterDataRow["PayeeEntityID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PayeeAddrID", DbType.String, Convert.ToString(parameterDataRow["PayeeAddrID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PayeeEntityTypeCode", DbType.String, Convert.ToString(parameterDataRow["PayeeEntityTypeCode"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbursementType", DbType.String, Convert.ToString(parameterDataRow["DisbursementType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_IrsTaxTypeCode", DbType.String, Convert.ToString(parameterDataRow["IrsTaxTypeCode"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PaymentMethodCode", DbType.String, Convert.ToString(parameterDataRow["PaymentMethodCode"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_CurrencyCode", DbType.String, Convert.ToString(parameterDataRow["CurrencyCode"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PersID", DbType.String, Convert.ToString(parameterDataRow["PersID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_Rollover", DbType.String, Convert.ToString(parameterDataRow["Rollover"]));
                // parameterDatabase.AddInParameter(l_DbCommand,"@varchar_Creator", DbType.String, "YRS");
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_TaxableAmount", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxableAmount"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_NonTaxableAmount", DbType.Decimal, Convert.ToDecimal(parameterDataRow["NonTaxableAmount"]));

                parameterDatabase.AddOutParameter(l_DbCommand, "@varchar_UniqueID", DbType.String, 100);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);
                if (parameterDatabase.GetParameterValue(l_DbCommand, "@varchar_UniqueID").GetType().ToString() == "System.DBNull")
                    return String.Empty;
                else
                    return Convert.ToString(parameterDatabase.GetParameterValue(l_DbCommand, "@varchar_UniqueID"));

            }
            catch
            {
                throw;
            }


        }


        //***********************************************************************************************
        //* Okay We've taken away all the available Money so now Let's Change his Status
        //* A refund of Type "REG" means it's a Regular Refund (Terminated Participant)
        //* A refund of Type "VOL" means the Participant Opted to take one or more of thier
        //* Voluntary Accounts
        //* a refund of Type "HARD" means a Hardship Refund MUST be Active
        //***********************************************************************************************
        private static bool ChangeStatus(string parameterPersonID, string parameterFundID, string parameterRefundType, bool parameterVested, bool parameterTerminated, bool parameterTookTDAccount, string parameterFundEventStatusType, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand l_DbCommand = null;
            // outParameterUpdateFundStatus=string.Empty ;
            try
            {
                if (parameterDatabase == null) return false;
                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_ChangeFundEventStatus");
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                if (l_DbCommand == null) return false;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundEventID", DbType.String, parameterFundID);
                parameterDatabase.AddInParameter(l_DbCommand, "@char_chrStatusType", DbType.String, parameterFundEventStatusType);
                parameterDatabase.AddInParameter(l_DbCommand, "@char_RefundType", DbType.String, parameterRefundType);
                parameterDatabase.AddInParameter(l_DbCommand, "@bool_IsVested", DbType.Boolean, parameterVested);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, parameterPersonID);
                //NP:IVP2:2008.10.23 - Removing out parameter
                // parameterDatabase.AddOutParameter(l_DbCommand,"@varchar_out_FundStatus", DbType.String,6);	

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);
                //NP:IVP2:2008.10.23 - Removing out parameter
                // outParameterUpdateFundStatus=l_DbCommand.GetParameterValue("@varchar_out_FundStatus").ToString().Trim();  
                //Shubhrata Mar 28th,2007 YRSPS 4702
                if (parameterRefundType == "HARD")
                {

                    ChangeMemberElectives(parameterPersonID, parameterFundID, "TD", parameterDatabase, parameterTransaction);


                }
                //Shubhrata Mar 28th,2007 YRSPS 4702
                //				if (parameterTookTDAccount == true)
                //				{
                //					ChangeMemberElectives (parameterPersonID, parameterFundID, "TD", parameterDatabase, parameterTransaction);
                //				}

                return true;
            }
            catch
            {
                throw;
            }
        }
        /* Added by Ashish on 21-Oct-2008
           *When a withdrawal is processed, any DLIN records will be updated to ININ.
           *If subsequent contributions come in during the month, the interest program will create a new DLIN.
           *The update of the DLIN to ININ will only be required when an account is fully refunded.
           *Therefore, if a hardship refund is processed, DLIN transactions are not updated. 
         */
        //NP:IVP2:2008.10.23 - Removing the Fund event status from the parameter list as that can be identified from the fund id
        private static bool UpdateStagingInterestRecord(string parameterFundID, string parameterRefRequestID, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand = null;
            try
            {
                if (parameterDatabase == null) return false;
                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_PopulateDLINRecords");
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                if (l_DbCommand == null) return false;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundEventID", DbType.String, parameterFundID);
                //NP:IVP2:2008.10.23 - Removing the status type parameter since it can be obtained from the Fund Event Id
                // parameterDatabase.AddInParameter(l_DbCommand,"@char_chrStatusType", DbType.String, parameterUpdatedFundStatus);		
                parameterDatabase.AddInParameter(l_DbCommand, "@RefRequestID", DbType.String, parameterRefRequestID);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string InsertNewRefundRequests(DataRow parameterDataRow, DataSet parameterDataSetForNewRequests, Database parameterDatabase, DbTransaction parameterTransaction, bool IsIRSOverride = false)// SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim 
        {

            if (parameterDatabase == null) return string.Empty;

            DbCommand InsertDbCommand = null;
            DbCommand InsertDbCommand1 = null;

            DataTable l_NewRefRequestDetails = null;
            string l_stringSubtableId = string.Empty;
            string l_string_RequestError = string.Empty;


            try
            {

                if (parameterDataRow == null) return string.Empty;

                InsertDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefundRequestNew");


                if (InsertDbCommand != null)
                {

                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_UniqueID", DbType.String, Convert.ToString(parameterDataRow["UniqueID"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_FundEventID", DbType.String, Convert.ToString(parameterDataRow["FundEventID"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_PersID", DbType.String, Convert.ToString(parameterDataRow["PersID"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@dateTime_ExpireDate", DbType.DateTime, Convert.ToString(parameterDataRow["ExpireDate"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_RefundType", DbType.String, Convert.ToString(parameterDataRow["RefundType"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_RequestStatus", DbType.String, Convert.ToString(parameterDataRow["RequestStatus"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@dateTime_StatusDate", DbType.DateTime, Convert.ToDateTime(parameterDataRow["StatusDate"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@dateTime_RequestDate", DbType.DateTime, Convert.ToDateTime(parameterDataRow["RequestDate"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Amount", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Amount"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_ReleaseBlankType", DbType.String, Convert.ToString(parameterDataRow["ReleaseBlankType"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@dateTime_ReleaseSentDate", DbType.DateTime, Convert.ToDateTime(parameterDataRow["ReleaseSentDate"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@Integer_AddressID", DbType.Int32, Convert.ToInt32(parameterDataRow["AddressID"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_NonTaxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["NonTaxable"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Taxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Taxable"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Tax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Tax"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_TaxRate", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxRate"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PIA", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PIA"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_MinDistribution", DbType.Decimal, Convert.ToDecimal(parameterDataRow["MinDistribution"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_HardShipAmt", DbType.Decimal, Convert.ToDecimal(parameterDataRow["HardShipAmt"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Deductions", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Deductions"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PlanType", DbType.String, Convert.ToString(parameterDataRow["PlanType"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@bit_IRSOverride", DbType.Boolean, IsIRSOverride); // SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim 
                    parameterDatabase.ExecuteNonQuery(InsertDbCommand, parameterTransaction);


                    InsertDbCommand1 = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefundRequestPerPlan");

                    parameterDatabase.AddInParameter(InsertDbCommand1, "@varchar_RequestId", DbType.String, Convert.ToString(parameterDataRow["UniqueID"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@varchar_PlanType", DbType.String, Convert.ToString(parameterDataRow["PlanType"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@bitIsMarket", DbType.Boolean, false);
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@varchar_RefundType", DbType.String, Convert.ToString(parameterDataRow["RefundType"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@varchar_PaymentStatus", DbType.String, Convert.ToString(parameterDataRow["RequestStatus"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@dateTime_PaymentDate", DbType.DateTime, Convert.ToDateTime(parameterDataRow["RequestDate"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@decimal_Amount", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Amount"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@decimal_NonTaxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["NonTaxable"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@decimal_Taxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Taxable"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@decimal_Tax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Tax"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@decimal_TaxRate", DbType.Decimal, Convert.ToDecimal(parameterDataRow["TaxRate"]));
                    parameterDatabase.AddInParameter(InsertDbCommand1, "@decimal_PIA", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PIA"]));
                    parameterDatabase.AddOutParameter(InsertDbCommand1, "@varchar_UniqueID", DbType.String, 100);
                    parameterDatabase.ExecuteNonQuery(InsertDbCommand1, parameterTransaction);


                    if (parameterDatabase.GetParameterValue(InsertDbCommand1, "@varchar_UniqueID").GetType().ToString() != "System.DBNull")
                        l_stringSubtableId = Convert.ToString(parameterDatabase.GetParameterValue(InsertDbCommand1, "@varchar_UniqueID"));
                    else
                        l_stringSubtableId = String.Empty;


                    if (l_stringSubtableId == string.Empty)
                    {
                        l_string_RequestError = "Error while retrieving Refrequest per Plan record id.";
                        return l_string_RequestError;
                    }

                    l_NewRefRequestDetails = parameterDataSetForNewRequests.Tables[1];

                    foreach (DataRow l_NewRefRequestDetailsRow in l_NewRefRequestDetails.Rows)
                    {
                        if (l_NewRefRequestDetailsRow != null)
                        {
                            l_string_RequestError = InsertNewRefundRequestDetails(l_NewRefRequestDetailsRow, l_stringSubtableId, parameterDatabase, parameterTransaction);
                        }


                    }



                }
                return l_string_RequestError;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        private static string InsertNewRefundRequestDetails(DataRow parameterDataRow, string parameter_stringSubtableId, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            if (parameterDatabase == null) return string.Empty;

            DbCommand InsertDbCommand = null;

            try
            {

                if (parameterDataRow == null) return "";



                InsertDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefundRequestDetails");

                if (InsertDbCommand != null)
                {
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_RefRequestsID", DbType.String, Convert.ToString(parameterDataRow["RefRequestsID"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_SubTableRequestID", DbType.String, parameter_stringSubtableId);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_AcctType", DbType.String, Convert.ToString(parameterDataRow["AcctType"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PersonalPostTax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PersonalPostTax"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PersonalPreTax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PersonalPreTax"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PersonalInterest", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PersonalInterest"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PersonalTotal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PersonalTotal"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_YMCAPreTax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["YMCAPreTax"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_YMCAInterest", DbType.Decimal, Convert.ToDecimal(parameterDataRow["YMCAInterest"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_YMCATotal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["YMCATotal"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_AcctTotal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["AcctTotal"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_GrandTotal", DbType.Decimal, Convert.ToDecimal(parameterDataRow["GrandTotal"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_AcctBreakDownType", DbType.String, Convert.ToString(parameterDataRow["AcctBreakDownType"]));
                    parameterDatabase.AddInParameter(InsertDbCommand, "@integer_SortOrder", DbType.Int16, Convert.ToInt16(parameterDataRow["SortOrder"]));
                    parameterDatabase.ExecuteNonQuery(InsertDbCommand, parameterTransaction);
                    return "";
                }

                return "";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            //			catch
            //			{
            //				throw;
            //			}

        }


        public static string Populate1099Values(string parameterDisbursementId, string parameterRefundType, Database parameterDatabase, DbTransaction parameterTransaction)
        {


            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return "DataBase not found";

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_Refunds_populate1099Values");

                if (l_DbCommand == null) return "Procedure not found";

                l_DataBase.AddInParameter(l_DbCommand, "@DisbursementId", DbType.String, parameterDisbursementId);
                l_DataBase.AddInParameter(l_DbCommand, "@withdrawelType", DbType.String, parameterRefundType);


                l_DataBase.ExecuteNonQuery(l_DbCommand, parameterTransaction);


                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string GetPlanType(string parameterGuiRefRequestsId)
        {
            string l_string_PlanType = "";

            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetPlanChosen");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterGuiRefRequestsId);
                l_DataBase.AddOutParameter(l_DbCommand, "@Plan_Type", DbType.String, 150);
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataBase.ExecuteNonQuery(l_DbCommand);

                l_string_PlanType = l_DataBase.GetParameterValue(l_DbCommand, "@Plan_Type").ToString();
                return l_string_PlanType;
            }
            catch
            {
                throw;
            }
        }

        public static void DeletePendingRefundRequest(string parameterRefundRequestID, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand l_DbCommand = null;

            try
            {

                if (parameterDatabase == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_Delete_HARD_RefundRequest");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterRefundRequestID);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);

            }
            catch
            {
                throw;
            }
        }


        private static bool ChangeMemberElectives(string parameterPersonID, string parameterFundID, string parameterAccountType, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return false;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateMemberElectives");

                if (l_DbCommand == null) return false;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, parameterPersonID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AccountType", DbType.String, parameterAccountType);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);

                return true;

            }
            catch
            {
                throw;
            }

        }

       
        public static void CancelPendingRefundRequest(string parameterRefundRequestID, Database parameterDatabase, DbTransaction parameterTransaction, string parameterRefCanResCode)
        {
            DbCommand l_DbCommand = null;

            try
            {

                if (parameterDatabase == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_CancelRefundRequest");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterRefundRequestID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_Updator", DbType.String, "Yrs"); // User ID has to go here.
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefCanResCode", DbType.String, parameterRefCanResCode);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);

            }
            catch
            {
                throw;
            }
        }


        #endregion

        //END | ML | 2020.05.25 | YRS-AT-4874 | Save refund request processing 

    }
}
