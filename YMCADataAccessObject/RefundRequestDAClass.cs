//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA YRS
// FileName			:	RefundRequestDAClass.cs
// Author Name		:	SrimuruganG
// Employee ID		:	32365
// Email				:	srimurugan.ag@icici-infotech.com
// Contact No			:	8744
// Creation Time		:	8/16/2005 4:26:01 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//Modification History :
//Modified Date			Modified By		Description
//10-Oct-2006			Hafiz			Hafiz on 11Oct2006 - YREN-2698
//02-May-2007           Shubhrata		YREN-3326 While disbursing the Refund Type SHIRA the status was not updated to WD

//28-May-2007			Shubhrata		Plan Split Changes
//04-June-2007			Aparna Samala	YREN-3491
//*******************************************************************************
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By           Date					Remarks
//Aparna Samala			24/10/2007				Check Annuity Existence,Get QDRO Member age
//28-Mar-2008			Shubhrata		 YRSPS 4702
//21-Oct-2008			Ashish Srivastava  Integrate DLIN changes,get RefRequestId ,
//										create UpdateStagingInterestRecord method,
//										Add out parameter in ChangeStatus Method for getting updated FundStatus
//Nikunj Patel			23-Oct-2008			Removed the out parameter from the ChangeStatus Method.
//Amit Nigam			2009.06.09		Adding Command.Timeout value
//07/22/2009			Ashish Srivastava	Merged code received from Bangalore for YRS5.0- 829.
//Amit Nigam            2009.09.25 	        Modified as per the additional partial request for the reports-Amit 25-09-2009
//08-Dec-2009           Parveen Kumar       'Set the RefundType for the atsRefRequests Table
//15-Dec-2009			Parveen Kumar       To Set RolloverOptions and Rollover Amount incase of Rollover
//17-Dec-2009			Ashish Srivastava   Merged code received from Bangalore 2009.12.17
//30-Dec-2009           Amit Kumar Nigam    Moved here to update the fundevent status after the market data is inserted
//02-Feb-2010           Amit Kumar Nigam    Modified for insertion into atsRefRollOverInstitutionData 02-02-2010
//2010.05.07            Ashish Srivastava	Resolve cashout lock Issue
//2010.05.08            Priya Jawale        BT-554 : IDX and PDF files are not copied in proper destination for 9th business day. 
//2010.Nov.23           Neeraj Singh        BT-664 : MRD Requirement 
//2010.11.29            Imran               BT:681  Unable to process the Savings Plan refund. Shows error page.
//2010.02.01            Imran               BT:729 Application creating record in AtsMrdRecordsDisbursements with zero in mnyPaidAmount column.
//Shashi Shekhar        2011.05.24          BT-837 : Add New FedEx screen needs to be added for Withdrawal status updates.
//Shashi Shekhar        2011.05.25          BT-837 : Merge two method and done validation part in Procedure
//Shashi Shekhar        2011-25-05          YRS 5.0-1298: added one parameter for Refund cancel reason code.
//Imran                '2011.06.10          BT:853   -Mrd Linkage with rollover disbursement with paidvalue=0  
//Imran                 2011.11.23          BT:948-  YRS 5.0-1457:Handle Taxble-Nontaxble pro-rated
//Harshala Trimukhe     01-Mar-2012         BT:978-YRS 5.0-1519 - 'YMCA acct at request" amt on withdrawal screen is incorrect : Added new function GetBARequestedPIA
//Sanjay R.             2012.08.06          BT-957/YRS 5.0-1484 : Termination Watcher
//Sanjeev G             2012.10.16          BT-960/YRS 5.0-1489 : Cash Out
//Priya P				2012-10-18			commented SR code of termiantion watcher to undo release from 12.7.0 patch
//Anudeep               2012-10-30          YRS 5.0-1484 : Termination Watcher: reverted Changes as per Observations 
//Anudeep               2012-11-05          Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
//Anudeep               2012-12-27          Bt-1538 Hardship Refund Process Deadlock Situation
//Sanjay Rawat          2013.10.28          BT-2247/YRS 5.0-2229:Change the TD termination and 6 month resumption date
//Anudeep               2014.03.25          BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program
//Dinesh Kanojia        2014.12.09          BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
//Anudeep A             2015.05.05          BT-2699:YRS 5.0-2441 : Modifications for 403b Loans 
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Anudeep A             2015.11.16          YRS-AT-2639 - YRS enh: Withdrawals Phase2: Esign Sprint: Refund Process request prepopulate data if available - make it editable by ManagementTeam only
//chandrasekar.c        2016.01.11          YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
//Bala                  2016.01.19          YRS-AT-2398: Customer Service requires a Special Handling alert for officers
//Anudeep               2016.02.17          YRS-AT-2640 - YRS enh: Withdrawals Phase2:Sprint2: allow AdminTool link to launch a prepopulated Yrs withdrawal page
//Sanjay                2016.03.02          YRS-AT-2281 - Fund status after SHIRA request is canceled (Withdrawals) (TrackIT 18650)
//Sanjay                2016.03.02          YRS-AT-1257 - Fund status erroneously updated to AE when Withdrawal request cancelled 
//Chandra sekar.c       2016.05.27          YRS-AT-3014 - YRS enh: Configurable Withdrawals project
//Sanjay GS Rawat       2016.06.06          YRS-AT-2962 - YRS enh: Configurable Withdrawals project 
//Santosh Bura          2016.08.31          YRS-AT-3028 -  YRS enh-RMD Utility - married to married Spouse beneficiary adjustment(TrackIT 25233) 
//Sanjay GS Rawat       2016.09.23          YRS-AT-3164 - YRS enh: Hot Fix:special Hardship Withdrawal - how to allow contributions to continue temporarily 
//Pramod P. Pokale      2016.11.16          YRS-AT-3146 - YRS enh: Fees - Partial Withdrawal Processing fee 
//Pramod P. Pokale      2017.11.27          YRS-AT-3653 - YRS enh: HOT FIX: Validation adjustment for prior Cancelled SHIRA (with regards to ReHires)(TrackIT 29582) 
//Megha Lad             2019.06.10          YRS-AT-4461 - YRS bug: HOT FIX NEEDED- portability bug- request-disbursement is for less than amount request (TrackIT38439)
//Sanjay GS Rawat 		2019.07.17          YRS-AT-4498 - YRS enh: REQUIRED LIVE 1/1/2020 - Hardship Withdrawals Legal Changes 
//Sanjay GS Rawat 		2019.07.23          YRS-AT-3870 - YRS bug-non-taxable TD amount not available for hardship (TrackIT 32820) 
//Pooja Kumkar          2019.07.11          YRS-AT-2670 - YRS enh:Hacienda withholding message - Puerto Rico based YMCA orgs  
//Pooja Kumkar          2020.04.29          YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688)    
//*******************************************************************************

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using YMCAObjects;

namespace YMCARET.YmcaDataAccessObject
{
    /// <summary>
    /// Summary description for RefundRequestDAClass.
    /// </summary>
    public sealed class RefundRequestDAClass
    {

        private DataTable m_DataTable_Transaction;
        private string m_string_PersonID;
        private string m_string_FundID;
        private DateTime m_DateTime_StatusDate;

        /// <summary>
        /// This properti is used to Get / Set the PersonID
        /// </summary>
        private string PersonID
        {
            get
            {
                return m_string_PersonID;
            }
            set
            {
                m_string_PersonID = value;
            }
        }

        /// <summary>
        /// This method is used to Get / Set the Status Date of Refund Request.
        /// </summary>
        private DateTime StatusDate
        {
            get
            {
                return m_DateTime_StatusDate;
            }

            set
            {
                m_DateTime_StatusDate = value;
            }
        }

        /// <summary>
        /// This property is used to Get / Set the Fund ID.
        /// </summary>
        private string FundID
        {
            get
            {
                return m_string_FundID;
            }
            set
            {
                m_string_FundID = value;
            }
        }


        private RefundRequestDAClass()
        {
            //
            // TODO: Add constructor logic here
            //



        }

        public static DataSet GetRequestStatus(string param_string_RequestStatus)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_Refund_RequestStatus");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RequestStatus", DbType.String, param_string_RequestStatus);

                l_DataSet = new DataSet("RequestStatus");

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, "RequestStatus");

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

        public static DataSet LookupMemberDetails(string parameterPersonID, string parameterFundEventID)
        {

            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            string[] l_TableNames;

            DataSet l_DataSet = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_AP_GetMemberDetails");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, parameterPersonID);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundEventID", DbType.String, parameterFundEventID);

                l_DataSet = new DataSet("Member Details");

                l_TableNames = new string[] { "Member Details", "Member Address", "Member Employment" }; 


                // To set the connenction Time Out. 
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                return l_DataSet;

            }
            catch
            {
                throw;
            }
        }

        //Start: Bala: YRS-AT-2398: 01/19/2019: Provides details which describes participant's eligibility to receive Special Handling
        /// <summary>
        /// Provides details which describes participant's eligibility to receive Special Handling
        /// </summary>
        /// <param name="parameterPersonID"></param>
        /// <returns></returns>
        public static DataSet GetSpecialHandlingDetails(string parameterPersonID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetSpecialHandlingOfficerDetails");
                if (l_DbCommand == null) return null;
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_gui_UniqueId", DbType.String, parameterPersonID);
                l_DataSet = new DataSet("SpecialHandlingDetails");
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, "SpecialHandlingDetails");
                return l_DataSet;
            }
            catch
            {
                throw;
            }
        }
        //End: Bala: YRS-AT-2398: 01/19/2019: Provides details which describes participant's eligibility to receive Special Handling

        //AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
        public static DataSet LookupTransactionForRefunds(string parameterFundID, bool parameterblnIncludeLoanAccts)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_AT_TransSumForRefunds");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@FundEventidvchar", DbType.String, parameterFundID);
                //AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
                l_DataBase.AddInParameter(l_DbCommand, "@bitIncludeLoanAccts", DbType.Boolean, parameterblnIncludeLoanAccts);

                // Conection Time out
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataSet = new DataSet("Member Contribution");

                // Load Accounts for Paid 
                l_TableNames = new string[] { "AccountForPaid", "AccountForPaidTotal" };

                // AccountForPaid		- Table Name which will return all tRansaction belong to the fund event ID
                // AccountForPaidTotal	- Table Name which will return the transaction Sum..

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);


                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_AT_TransSumForRefunds_Unfuned");

                if (l_DbCommand == null) return l_DataSet;

                l_DataBase.AddInParameter(l_DbCommand, "@FundEventidvchar", DbType.String, parameterFundID);


                // Load Accounts for Non Paid 
                l_TableNames = new string[] { "AccountForNonPaid", "AccountForNonPaidTotal" };

                // AccountForNonPaid		- Table Name which will return all tRansaction belong to the fund event ID
                // AccountForNonPaidTotal	- Table Name which will return the transaction Sum..

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                return l_DataSet;

            }
            catch
            {
                throw;
            }

        }

        public static DataTable GetFirstInstallment(string paramaeterRefundRequestID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetFirstInstallment");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@RefundRequestID", DbType.String, paramaeterRefundRequestID);


                // Connection Timeout
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataSet = new DataSet("First Installment");

                l_TableNames = new string[] { "First Installment" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["First Installment"]);
                }

                return null;


            }
            catch
            {
                throw;
            }
        }

        public static DataTable MemberRefundRequestDetails(string paramaeterPersonID, string parameterFundID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetRefundRequestsPartial");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, paramaeterPersonID);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);

                // Connection Timeout
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataSet = new DataSet("Member Refund Details");

                l_TableNames = new string[] { "Member Refund Details" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Member Refund Details"]);
                }

                return null;


            }
            catch
            {
                throw;
            }
        }
        //Added by Ashish 2010.05.07 resolve cashout lock issue ,Start
        /// <summary>
        /// This Method used to get RefundRequets Details(CashOutProcessBO class)
        /// </summary>
        /// <param name="paramaeterPersonID"></param>
        /// <param name="parameterFundID"></param>
        /// <param name="paraDB"></param>
        /// <param name="paraDbTransaction"></param>
        /// <returns></returns>

        public static DataTable MemberRefundRequestDetails(string paramaeterPersonID, string parameterFundID, Database paraDB, DbTransaction paraDbTransaction)
        {

            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {


                if (paraDB == null) return null;

                l_DbCommand = paraDB.GetStoredProcCommand("dbo.yrs_usp_RR_GetRefundRequestsPartial");

                if (l_DbCommand == null) return null;

                paraDB.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, paramaeterPersonID);
                paraDB.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);

                // Connection Timeout
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataSet = new DataSet("Member Refund Details");

                l_TableNames = new string[] { "Member Refund Details" };

                paraDB.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames, paraDbTransaction);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Member Refund Details"]);
                }

                return null;


            }
            catch
            {
                throw;
            }
        }


        public static DataTable MemberNotes(string paramaeterPersonID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_AN_GetNotes");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, paramaeterPersonID);

                l_DataSet = new DataSet("Member Notes");

                l_TableNames = new string[] { "Member Notes" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Member Notes"]);
                }

                return null;


            }
            catch
            {
                throw;
            }
        }

        public static decimal GetCurrentPIA(string parameterFundID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            decimal l_DecimalAmount = 0.00M;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return 0.00M;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_CalculateCurrentPIA");

                if (l_DbCommand == null) return 0.00M;


                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);
                l_DataBase.AddOutParameter(l_DbCommand, "@numeric_PIAAmount", DbType.Double, 18);



                l_DataBase.ExecuteNonQuery(l_DbCommand);

                if (l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount").GetType().ToString() != "System.DBNull")
                {
                    l_DecimalAmount = Convert.ToDecimal(l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount"));
                }


                return l_DecimalAmount;

            }
            catch
            {
                throw;
            }
        }


        public static decimal GetTerminatePIA(string parameterFundID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            decimal l_DecimalAmount = 0.00M;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return 0.00M;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_CalculateTerminatePIA");

                if (l_DbCommand == null) return 0.00M;


                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);
                l_DataBase.AddOutParameter(l_DbCommand, "@numeric_PIAAmount", DbType.Double, 18);

                l_DataBase.ExecuteNonQuery(l_DbCommand);
                if (l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount").GetType().ToString() != "System.DBNull")
                {
                    l_DecimalAmount = Convert.ToDecimal(l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount"));
                }




                return l_DecimalAmount;
            }
            catch
            {
                throw;
            }
        }

        //BA Account YMCA PhaseV 03-04-2009
        public static decimal GetBACurrentPIA(string parameterFundID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            decimal l_DecimalAmount = 0.00M;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return 0.00M;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_CalculateBACurrentPIA");

                if (l_DbCommand == null) return 0.00M;


                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);
                l_DataBase.AddOutParameter(l_DbCommand, "@numeric_PIAAmount", DbType.Double, 18);



                l_DataBase.ExecuteNonQuery(l_DbCommand);

                if (l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount").GetType().ToString() != "System.DBNull")
                {
                    l_DecimalAmount = Convert.ToDecimal(l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount"));
                }


                return l_DecimalAmount;

            }
            catch
            {
                throw;
            }
        }

        //Added By Harshala  BT 978 
        public static decimal GetBARequestedPIA(string parameterFundID, string parameterRefRequestID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            decimal l_DecimalAmount = 0.00M;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return 0.00M;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_CalculateBARequestedPIA");

                if (l_DbCommand == null) return 0.00M;


                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefRequestID", DbType.String, parameterRefRequestID);
                l_DataBase.AddOutParameter(l_DbCommand, "@numeric_PIAAmount", DbType.Double, 18);



                l_DataBase.ExecuteNonQuery(l_DbCommand);

                if (l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount").GetType().ToString() != "System.DBNull")
                {
                    l_DecimalAmount = Convert.ToDecimal(l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount"));
                }


                return l_DecimalAmount;

            }
            catch
            {
                throw;
            }
        }

        //Added By ganeswar on july-21-2009
        public static decimal GetBATerminatePIAAtRequest(string parameterFundID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            decimal l_DecimalAmount = 0.00M;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return 0.00M;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_CalculateBACurrentRequestPIA");

                if (l_DbCommand == null) return 0.00M;


                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);
                l_DataBase.AddOutParameter(l_DbCommand, "@numeric_PIAAmount", DbType.Double, 18);



                l_DataBase.ExecuteNonQuery(l_DbCommand);

                if (l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount").GetType().ToString() != "System.DBNull")
                {
                    l_DecimalAmount = Convert.ToDecimal(l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount"));
                }


                return l_DecimalAmount;

            }
            catch
            {
                throw;
            }
        }
        //Added By ganeswar on july-21-2009



        public static decimal GetBATerminatePIA(string parameterFundID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            decimal l_DecimalAmount = 0.00M;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return 0.00M;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_CalculateBATerminatePIA");

                if (l_DbCommand == null) return 0.00M;


                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);
                l_DataBase.AddOutParameter(l_DbCommand, "@numeric_PIAAmount", DbType.Double, 18);

                l_DataBase.ExecuteNonQuery(l_DbCommand);
                if (l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount").GetType().ToString() != "System.DBNull")
                {
                    l_DecimalAmount = Convert.ToDecimal(l_DataBase.GetParameterValue(l_DbCommand, "@numeric_PIAAmount"));
                }
                return l_DecimalAmount;
            }
            catch
            {
                throw;
            }
        }
        //BA Account YMCA PhaseV 03-04-2009


        public static DataTable GetRefundConfiguration()
        {

            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_GetRefundConfiguration");

                if (l_DbCommand == null) return null;

                l_DataSet = new DataSet("Refund Configuration");

                l_TableNames = new string[] { "Refund Configuration" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Refund Configuration"]);
                }

                return null;


            }
            catch
            {
                throw;
            }
        }
        public static DataTable GetConfigurationCategoryWise(string parameterCategory)
        {

            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_GetConfigurationCategoryWise");
                l_DataBase.AddInParameter(l_DbCommand, "@chvConfigCategoryCode", DbType.String, parameterCategory);

                if (l_DbCommand == null) return null;

                l_DataSet = new DataSet("Refund Configuration");

                l_TableNames = new string[] { "Refund Configuration" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Refund Configuration"]);
                }

                return null;


            }
            catch
            {
                throw;
            }
        }


        //by Aparna 18/04/2007
        //		public static DataTable GetConfigurationCategoryWise(string parameterCategory)
        //		{
        //			
        //
        //			DataSet l_dsGetConfigDetails =null;
        //			Database l_DataBase= null;
        //			DbCommand l_DbCommand =null;
        //			string [] l_TableNames;
        //
        //			try
        //			{
        //				l_DataBase= DatabaseFactory.CreateDatabase("YRS");
        //				if(l_DataBase==null) return null;
        //				l_DbCommand =l_DataBase.GetStoredProcCommand("yrs_usp_GetConfigurationCategoryWise");
        //				l_DbCommand.AddInParameter("@chvConfigCategoryCode",DbType.String,parameterCategory);
        //		
        //				if (l_DbCommand == null) return null;
        //				l_dsGetConfigDetails = new DataSet ("Refund Configuration");
        //				l_TableNames = new string []{"Configuration Details"};
        //				l_DataBase.LoadDataSet (l_DbCommand, l_dsGetConfigDetails, l_TableNames);
        //
        //				if (l_dsGetConfigDetails != null) 
        //				{
        //					return (l_dsGetConfigDetails.Tables ["Configuration Details"]);
        //				}
        //					
        //				return null;
        //			}
        //			catch
        //			{
        //				throw;
        //			}
        //		}


        public static decimal GetDistributionPeriod(int parameterPersonAge)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            decimal l_DecimalPeriod = 0.00M;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return 0.00M;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_VMMD_GetDistributionPeriod");

                if (l_DbCommand == null) return 0.00M;


                l_DataBase.AddInParameter(l_DbCommand, "@integer_PersonAge", DbType.Int16, parameterPersonAge);
                l_DataBase.AddOutParameter(l_DbCommand, "@decimal_DistrbutionPeriod", DbType.Double, 5);

                l_DataBase.ExecuteNonQuery(l_DbCommand);
                if (l_DataBase.GetParameterValue(l_DbCommand, "@decimal_DistrbutionPeriod").GetType().ToString() != "System.DBNull")
                {
                    l_DecimalPeriod = Convert.ToDecimal(l_DataBase.GetParameterValue(l_DbCommand, "@decimal_DistrbutionPeriod"));
                }


                return l_DecimalPeriod;

            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        ///		This method to get the Schema of Refund Tables
        /// </summary>
        /// <returns></returns>
        public static DataSet GetSchemaRefundTable()
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_GetSchemaTables");

                if (l_DbCommand == null) return null;

                l_DataSet = new DataSet("Refund Tables");

                //l_TableNames = new string[] { "atsRefunds", "atsRefRequests", "atsRefRequestDetails" };
                //Added By SG: 2012.08.07: BT-960
                l_TableNames = new string[] { "atsRefunds", "atsRefRequests", "atsRefRequestDetails", "RefundProcessingDetails" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                return l_DataSet;

            }
            catch
            {
                throw;
            }

        }

        public static DataTable GetAccountBreakDown()
        {

            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_AMAB_GetAccountBreakDown");

                if (l_DbCommand == null) return null;

                l_DataSet = new DataSet("AccountBreakDown");

                l_TableNames = new string[] { "AccountBreakDown" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["AccountBreakDown"]);
                }

                return null;
            }
            catch
            {
                throw;
            }

        }
        //Modified by Dilip to add flags for AM-Matched &  TM-Matched on 03-12-2009
        //public static ArrayList InsertRefunds (DataSet parameterDatasetFirst,DataSet parameterDatasetSecond, bool bool_NotIncludeYMCAAcct, bool bool_NotIncludeYMCALegacyAcct)
        public static ArrayList InsertRefunds(DataSet parameterDatasetFirst, DataSet parameterDatasetSecond, bool bool_NotIncludeYMCAAcct, bool bool_NotIncludeYMCALegacyAcct, bool Bool_NotIncludeAMMatched, bool Bool_NotIncludeTMMatched)
        {
            //Modified by Dilip to add flags for AM-Matched &  TM-Matched on 03-12-2009

            string l_RefundRequest = "";
            ArrayList ArrayList_RequestId = new ArrayList();
            bool l_bool_TransactionAlive = false;
            Double double_ProRatedPercentage = 0;
            Double FirstInstallment = 0;

            Database l_Database = null;
            DbConnection l_DbConnection = null;
            DbTransaction l_DbTransaction = null;

            string l_SubTableRequestID = string.Empty;

            try
            {
                l_Database = DatabaseFactory.CreateDatabase("YRS");
                if (l_Database == null) return ArrayList_RequestId;

                l_DbConnection = l_Database.CreateConnection();
                l_DbConnection.Open();

                l_DbTransaction = l_DbConnection.BeginTransaction();
                l_bool_TransactionAlive = true;
                //New Modification for Market Based Withdrawal Amit Nigam 11/Nov/2009   
                string string_RefrequestId = string.Empty;
                //l_RefundRequest=System.Guid.NewGuid().ToString();


                l_RefundRequest = InsertRefundRequests(parameterDatasetFirst, parameterDatasetSecond, l_Database, l_DbTransaction);

                if (parameterDatasetFirst.Tables.Count > 0)
                {

                    l_SubTableRequestID = InsertRefundRequestsPerPlan(parameterDatasetFirst, l_RefundRequest, l_Database, l_DbTransaction);

                    //				l_RefundRequest = InsertRefundRequests (parameterDatasetFirst,l_Database,l_DbTransaction);

                    ArrayList_RequestId.Add(l_RefundRequest);
                    if (l_RefundRequest != "")
                    {
                        InsertRefundRequestDetails(parameterDatasetFirst, l_RefundRequest, l_SubTableRequestID, l_Database, l_DbTransaction);

                        double_ProRatedPercentage = Convert.ToDouble(parameterDatasetFirst.Tables[0].Rows[0]["ProRatedPercentage"]);
                        FirstInstallment = Convert.ToDouble(parameterDatasetFirst.Tables[0].Rows[0]["FirstInstPercentage"]);

                        if (parameterDatasetFirst.Tables[0].Rows[0]["RefundType"].ToString().Trim() == "PART")
                        {
                            GetPartialRefundsRequestData(l_RefundRequest, l_SubTableRequestID, double_ProRatedPercentage, bool_NotIncludeYMCAAcct, bool_NotIncludeYMCALegacyAcct, Bool_NotIncludeAMMatched, Bool_NotIncludeTMMatched, l_Database, l_DbTransaction);
                        }

                        else if (Convert.ToBoolean(parameterDatasetFirst.Tables[0].Rows[0]["IsMarket"]) == true)
                        {
                            GetPartialRefundsRequestData(l_RefundRequest, l_SubTableRequestID, FirstInstallment, bool_NotIncludeYMCAAcct, bool_NotIncludeYMCALegacyAcct, Bool_NotIncludeAMMatched, Bool_NotIncludeTMMatched, l_Database, l_DbTransaction);
                        }
                    }
                    else
                    {
                        throw new Exception("Pending Request already exists");
                    }
                }

                if (parameterDatasetSecond.Tables.Count > 0)
                {
                    //l_RefundRequest = InsertRefundRequests (parameterDatasetSecond,l_Database,l_DbTransaction);
                    l_SubTableRequestID = InsertRefundRequestsPerPlan(parameterDatasetSecond, l_RefundRequest, l_Database, l_DbTransaction);
                    ArrayList_RequestId.Add(l_RefundRequest);
                    if (l_RefundRequest != "")
                    {

                        //InsertRefundRequestDetails (parameterDatasetSecond, l_RefundRequest, l_Database, l_DbTransaction);	
                        InsertRefundRequestDetails(parameterDatasetSecond, l_RefundRequest, l_SubTableRequestID, l_Database, l_DbTransaction);

                        double_ProRatedPercentage = Convert.ToDouble(parameterDatasetSecond.Tables[0].Rows[0]["ProRatedPercentage"]);
                        FirstInstallment = Convert.ToDouble(parameterDatasetSecond.Tables[0].Rows[0]["FirstInstPercentage"]);

                        if (parameterDatasetSecond.Tables[0].Rows[0]["RefundType"].ToString().Trim() == "PART")
                        {
                            GetPartialRefundsRequestData(l_RefundRequest, l_SubTableRequestID, double_ProRatedPercentage, bool_NotIncludeYMCAAcct, bool_NotIncludeYMCALegacyAcct, Bool_NotIncludeAMMatched, Bool_NotIncludeTMMatched, l_Database, l_DbTransaction);
                        }
                        else if (Convert.ToBoolean(parameterDatasetSecond.Tables[0].Rows[0]["IsMarket"]) == true)
                        {
                            GetPartialRefundsRequestData(l_RefundRequest, l_SubTableRequestID, FirstInstallment, bool_NotIncludeYMCAAcct, bool_NotIncludeYMCALegacyAcct, Bool_NotIncludeAMMatched, Bool_NotIncludeTMMatched, l_Database, l_DbTransaction);
                        }
                    }
                    else
                    {
                        throw new Exception("Pending Request already exists");
                    }
                }


                l_DbTransaction.Commit();
                l_bool_TransactionAlive = false;

                l_DbConnection.Close();

                return ArrayList_RequestId;
            }
            catch
            {
                if (l_bool_TransactionAlive == true)
                {
                    l_DbTransaction.Rollback();
                    l_DbConnection.Close();
                }
                throw;
            }

        }

        public static string InsertRefundRequestsPerPlan(DataSet parameterDataSet, string string_RefrequestId, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            //Database l_DataBase = null;
            if (parameterDatabase == null) return string.Empty;

            DbCommand InsertDbCommand = null;
            DbCommand UpdateDbCommand = null;
            DbCommand DeleteDbCommand = null;

            try
            {
                //l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

                if (parameterDataSet == null) return string.Empty;

                //if (l_DataBase  == null) return string.Empty;

                //InsertDbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_InsertRefundRequest");
                InsertDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefundRequestPerPlan");


                if (InsertDbCommand != null)
                {
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_RequestId", DbType.String, string_RefrequestId);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_PlanType", DbType.String, "PlanType", DataRowVersion.Default);
                    //Added By SG: 2012.06.25: BT-960
                    //if (parameterDataSet.Tables[0].Rows[0]["RefundType"].ToString() == "CASH")
                    if (parameterDataSet.Tables[0].Rows[0]["RefundType"].ToString() == "CASH" 
                        || parameterDataSet.Tables[0].Rows[0]["RefundType"].ToString() == "SHIRA"
                        || parameterDataSet.Tables[0].Rows[0]["RefundType"].ToString() == "REG")    //Added By SG: 2012.31.12: BT-1511
                    {
                        parameterDatabase.AddInParameter(InsertDbCommand, "@bitIsMarket", DbType.Boolean, false);
                    }
                    else
                    {
                        parameterDatabase.AddInParameter(InsertDbCommand, "@bitIsMarket", DbType.Boolean, "IsMarket", DataRowVersion.Default);
                    }
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_RefundType", DbType.String, "RefundType", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_PaymentStatus", DbType.String, "RequestStatus", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@dateTime_PaymentDate", DbType.DateTime, "StatusDate", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Amount", DbType.Decimal, "Amount", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_NonTaxable", DbType.Decimal, "NonTaxable", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Taxable", DbType.Decimal, "Taxable", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Tax", DbType.Decimal, "Tax", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_TaxRate", DbType.Decimal, "TaxRate", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PIA", DbType.Decimal, "PIA", DataRowVersion.Default);
                    parameterDatabase.AddOutParameter(InsertDbCommand, "@varchar_UniqueID", DbType.String, 100);


                }


                //UpdateDbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_UpdateRefundRequest");
                UpdateDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateRefundRequestPerPlan");

                if (UpdateDbCommand != null)
                {
                    // this is the Dummy Wrapper for .NET ET.
                }

                //DeleteDbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_DeleteRefundRequest");
                DeleteDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_DeleteRefundRequestPerPlan");

                if (DeleteDbCommand != null)
                {
                    // this is the Dummy Wrapper for .NET ET.
                }

                //l_DataBase.UpdateDataSet (parameterDataSet, "atsRefRequests", InsertDbCommand, UpdateDbCommand, DeleteDbCommand, UpdateBehavior.Standard);
                parameterDatabase.UpdateDataSet(parameterDataSet, "atsRefRequests", InsertDbCommand, UpdateDbCommand, DeleteDbCommand, parameterTransaction);

                if (parameterDatabase.GetParameterValue(InsertDbCommand, "@varchar_UniqueID").GetType().ToString() != "System.DBNull")
                    return Convert.ToString(parameterDatabase.GetParameterValue(InsertDbCommand, "@varchar_UniqueID"));
                else
                    return String.Empty;

            }
            catch
            {
                throw;
            }

        }


        public static string InsertRefundRequests(DataSet parameterDataSet, DataSet parameterDatasetSecond, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            //Database l_DataBase = null;
            if (parameterDatabase == null) return string.Empty;

            DbCommand InsertDbCommand = null;
            DbCommand UpdateDbCommand = null;
            DbCommand DeleteDbCommand = null;
            DataRow l_Import_dataRow = null;
            DataRow l_dataRow = null;
            DataTable l_DataTable = null;
            string string_PlanType = null; ;
            string string_ProcessId = null;
            Double decimal_FirstInstallement = 0.0;
            Double decimal_DefferedPayment = 0.0;




            try
            {

                if (parameterDatasetSecond != null)
                {
                    if (parameterDatasetSecond.Tables.Count > 0)
                    {

                        l_DataTable = parameterDatasetSecond.Tables[0].Clone();

                        l_Import_dataRow = parameterDatasetSecond.Tables[0].Rows[0];
                        l_DataTable.ImportRow(l_Import_dataRow);
                        l_dataRow = l_DataTable.Rows[0];
                    }
                }

                if (l_Import_dataRow == null)
                {
                    l_DataTable = parameterDataSet.Tables[0].Clone();

                    l_Import_dataRow = parameterDataSet.Tables[0].Rows[0];
                    l_DataTable.ImportRow(l_Import_dataRow);
                    l_dataRow = l_DataTable.Rows[0];

                }
                else
                    if (parameterDataSet.Tables.Count > 0)
                    {
                        l_dataRow["NonTaxable"] = Convert.ToDecimal(l_dataRow["NonTaxable"]) + Convert.ToDecimal(parameterDataSet.Tables[0].Rows[0]["NonTaxable"]);
                        l_dataRow["Taxable"] = Convert.ToDecimal(l_dataRow["Taxable"]) + Convert.ToDecimal(parameterDataSet.Tables[0].Rows[0]["Taxable"]);
                        l_dataRow["Tax"] = Convert.ToDecimal(l_dataRow["Tax"]) + Convert.ToDecimal(parameterDataSet.Tables[0].Rows[0]["Tax"]);
                        l_dataRow["Amount"] = Convert.ToDecimal(l_dataRow["Amount"]) + Convert.ToDecimal(parameterDataSet.Tables[0].Rows[0]["Amount"]);
                        //08-Dec-2009   Parveen Kumar 'Set the RefundType for the atsRefRequests Table
                        if (l_dataRow["PlanType"].ToString().ToUpper() == "SAVINGS")
                        {
                            l_dataRow["RefundType"] = parameterDataSet.Tables[0].Rows[0]["RefundType"].ToString();
                            l_dataRow["PlanType"] = "BOTH";
                        }
                        //08-Dec-2009   Parveen Kumar 'Set the RefundType for the atsRefRequests Table


                    }



                //l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

                if (parameterDataSet == null) return string.Empty;

                //if (l_DataBase  == null) return string.Empty;

                //InsertDbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_InsertRefundRequest");
                InsertDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefundRequest");


                if (InsertDbCommand != null)
                {

                    //Commented By SG: 2012.07.31: BT-960
                    if (l_DataTable.Rows[0]["RefundType"].ToString() == "CASH" 
                        || l_DataTable.Rows[0]["RefundType"].ToString() == "SHIRA"
                        || l_DataTable.Rows[0]["RefundType"].ToString() == "REG")   //Added By SG: 2012.31.12: BT-1511
                    {
                        string_PlanType = l_dataRow["PlanType"].ToString();
                        decimal_FirstInstallement = 0.0;
                        decimal_DefferedPayment = 0.0;
                    }
                    else
                    {
                        string_PlanType = l_dataRow["PlanType"].ToString();
                        string_ProcessId = l_dataRow["ProcessId"].ToString();
                        decimal_FirstInstallement = Convert.ToDouble(l_dataRow["FirstInstPercentage"]);
                        decimal_DefferedPayment = Convert.ToDouble(l_dataRow["DefferedPmtPercentage"]);

                    }


                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_FundEventID", DbType.Guid, l_dataRow["FundEventID"]);  // SR:2010.07.09
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_PersID", DbType.Guid, l_dataRow["PersID"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@dateTime_ExpireDate", DbType.DateTime, l_dataRow["ExpireDate"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_RefundType", DbType.String, l_dataRow["RefundType"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_RequestStatus", DbType.String, l_dataRow["RequestStatus"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@dateTime_StatusDate", DbType.DateTime, l_dataRow["StatusDate"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@dateTime_RequestDate", DbType.DateTime, l_dataRow["RequestDate"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Amount", DbType.Decimal, l_dataRow["Amount"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_ReleaseBlankType", DbType.String, l_dataRow["ReleaseBlankType"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@dateTime_ReleaseSentDate", DbType.DateTime, l_dataRow["ReleaseSentDate"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@Integer_AddressID", DbType.Int32, l_dataRow["AddressID"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_NonTaxable", DbType.Decimal, l_dataRow["NonTaxable"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Taxable", DbType.Decimal, l_dataRow["Taxable"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Tax", DbType.Decimal, l_dataRow["Tax"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_TaxRate", DbType.Decimal, l_dataRow["TaxRate"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PIA", DbType.Decimal, l_dataRow["PIA"]);
                    //parameterDatabase.AddInParameter(InsertDbCommand,"@varchar_Creater", DbType.String, "Creator", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_MinDistribution", DbType.Decimal, l_dataRow["MinDistribution"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_HardShipAmt", DbType.Decimal, l_dataRow["HardShipAmt"]);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_Deductions", DbType.Decimal, l_dataRow["Deductions"]);
                    //Added a parameter to insert the Plan type in atsrefrequests as per the change requested in Partial withdrawal. 29/06/2009
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_PlanType", DbType.String, string_PlanType);
                    //Added a parameter to insert the Plan type in atsrefrequests as per the change requested in Partial withdrawal. 29/06/2009

                    //Added as per the additional partial request for the reports-Amit 25-09-2009
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_FirstInstallement", DbType.Decimal, decimal_FirstInstallement);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_DefferedPayment", DbType.Decimal, decimal_DefferedPayment);






                    //Added as per the additional partial request for the reports-Amit 25-09-2009

                    parameterDatabase.AddOutParameter(InsertDbCommand, "@varchar_UniqueID", DbType.String, 100);


                }
                //
                //				//UpdateDbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_UpdateRefundRequest");
                //				UpdateDbCommand = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_RR_UpdateRefundRequest");
                //				
                //				if (UpdateDbCommand != null)
                //				{
                //					// this is the Dummy Wrapper for .NET ET.
                //				}
                //							
                //				//DeleteDbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_DeleteRefundRequest");
                //				DeleteDbCommand = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_RR_DeleteRefundRequest");
                //				
                //				if (DeleteDbCommand != null)
                //				{
                //					// this is the Dummy Wrapper for .NET ET.
                //				}

                //l_DataBase.UpdateDataSet (parameterDataSet, "atsRefRequests", InsertDbCommand, UpdateDbCommand, DeleteDbCommand, UpdateBehavior.Standard);
                //parameterDatabase.UpdateDataSet (parameterDataSet, "atsRefRequests", InsertDbCommand, UpdateDbCommand, DeleteDbCommand, parameterTransaction);
                parameterDatabase.ExecuteNonQuery(InsertDbCommand, parameterTransaction);

                if (parameterDatabase.GetParameterValue(InsertDbCommand, "@varchar_UniqueID").GetType().ToString() != "System.DBNull")
                    return Convert.ToString(parameterDatabase.GetParameterValue(InsertDbCommand, "@varchar_UniqueID"));
                else
                    return String.Empty;

            }
            catch
            {
                throw;
            }

        }


        private static string InsertRefundRequestDetails(DataSet parameterDataSet, string parameterRefundRequestID, string l_SubTableRequestID, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            //Database l_DataBase = null;
            if (parameterDatabase == null) return string.Empty;

            DbCommand InsertDbCommand = null;
            DbCommand UpdateDbCommand = null;
            DbCommand DeleteDbCommand = null;

            try
            {
                //l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

                if (parameterDataSet == null) return "";

                //if (l_DataBase  == null) return ;

                parameterDataSet = UpdateTableWithRefundRequestID(parameterDataSet, parameterRefundRequestID);

                //InsertDbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_InsertRefundRequestDetails");
                InsertDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefundRequestDetails");

                if (InsertDbCommand != null)
                {
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_RefRequestsID", DbType.Guid, "RefRequestsID", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_SubTableRequestID", DbType.String, l_SubTableRequestID);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_AcctType", DbType.String, "AcctType", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PersonalPostTax", DbType.Decimal, "PersonalPostTax", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PersonalPreTax", DbType.Decimal, "PersonalPreTax", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PersonalInterest", DbType.Decimal, "PersonalInterest", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_PersonalTotal", DbType.Decimal, "PersonalTotal", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_YMCAPreTax", DbType.Decimal, "YMCAPreTax", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_YMCAInterest", DbType.Decimal, "YMCAInterest", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_YMCATotal", DbType.Decimal, "YMCATotal", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_AcctTotal", DbType.Decimal, "AcctTotal", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@decimal_GrandTotal", DbType.Decimal, "GrandTotal", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_AcctBreakDownType", DbType.String, "AcctBreakDownType", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@integer_SortOrder", DbType.Int16, "SortOrder", DataRowVersion.Current);

                }

                //UpdateDbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_UpdateRefundRequest");
                UpdateDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateRefundRequest");

                if (UpdateDbCommand != null)
                {
                    // this is the Dummy Wrapper for .NET ET.
                }

                //DeleteDbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_RR_DeleteRefundRequest");
                DeleteDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_DeleteRefundRequest");

                if (DeleteDbCommand != null)
                {
                    // this is the Dummy Wrapper for .NET ET.
                }

                //l_DataBase.UpdateDataSet (parameterDataSet, "atsRefRequestDetails", InsertDbCommand, UpdateDbCommand, DeleteDbCommand, UpdateBehavior.Standard);
                parameterDatabase.UpdateDataSet(parameterDataSet, "atsRefRequestDetails", InsertDbCommand, UpdateDbCommand, DeleteDbCommand, parameterTransaction);

                return "";
            }
            catch
            {
                throw;
            }
            //			catch
            //			{
            //				throw;
            //			}

        }

        private static DataSet UpdateTableWithRefundRequestID(DataSet parameterDataSet, string paramaterRefundRequestID)
        {
            DataTable l_DataTable = null;
            //DataRow l_DataRow = null;.

            if (parameterDataSet == null) return null;

            try
            {
                l_DataTable = parameterDataSet.Tables["atsRefRequestDetails"];

                if (l_DataTable == null) return null;

                foreach (DataRow l_DataRow in l_DataTable.Rows)
                {
                    l_DataRow["RefRequestsID"] = paramaterRefundRequestID;
                }

                return parameterDataSet;

            }
            catch
            {
                throw;
            }
        }



        //Added to insert the unique id of the atsRefRequestPerPlan in this table-Amit Kumar Nigam-12-11-2009	
        //public static void GetPartialRefundsRequestData(string RefundRequestID,double Percentage,bool bool_NotIncludeYMCAAcct,bool bool_NotIncludeYMCALegacyAcct,Database parameter_Database, DbTransaction parameterTransaction)
        //--Commented By Dilip on 03-12-2009 for AM-Matched & TM-Matched
        //--public static void GetPartialRefundsRequestData(string RefundRequestID,string parameterSubTableRequestID,double Percentage,bool bool_NotIncludeYMCAAcct,bool bool_NotIncludeYMCALegacyAcct,Database parameter_Database, DbTransaction parameterTransaction)
        //--Added By Dilip on 03-12-2009 for AM-Matched & TM-Matched
        public static void GetPartialRefundsRequestData(string RefundRequestID, string parameterSubTableRequestID, double Percentage, bool bool_NotIncludeYMCAAcct, bool bool_NotIncludeYMCALegacyAcct, bool Bool_NotIncludeAMMatched, bool Bool_NotIncludeTMMatched, Database parameter_Database, DbTransaction parameterTransaction)
        {
            //--Added By Dilip on 03-12-2009 for AM-Matched & TM-Matched
            //Added to insert the unique id of the atsRefRequestPerPlan in this table-Amit Kumar Nigam-12-11-2009	

            //Database	l_DataBase = null;
            //string []	l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                parameter_Database = DatabaseFactory.CreateDatabase("YRS");

                //if (l_DataBase == null) return null;

                l_DbCommand = parameter_Database.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefdRequestTransactions");

                //if (l_DbCommand == null) return null;

                parameter_Database.AddInParameter(l_DbCommand, "@varchar_RefRequestsID", DbType.String, RefundRequestID);
                //Added to insert the unique id of the atsRefRequestPerPlan in this table-Amit Kumar Nigam-12-11-2009	
                parameter_Database.AddInParameter(l_DbCommand, "@varchar_RefRequestPerPlanID", DbType.String, parameterSubTableRequestID);
                //--Added to insert the unique id of the atsRefRequestPerPlan in this table-Amit Kumar Nigam-12-11-2009	
                parameter_Database.AddInParameter(l_DbCommand, "@decimal_Percentage", DbType.Double, Percentage);
                parameter_Database.AddInParameter(l_DbCommand, "@BitNotIncludeYMCAAcct", DbType.Boolean, bool_NotIncludeYMCAAcct);
                parameter_Database.AddInParameter(l_DbCommand, "@BitNotIncludeYMCALegacyAcct ", DbType.Boolean, bool_NotIncludeYMCALegacyAcct);
                //--Added By Dilip on 03-12-2009 for AM-Matched & TM-Matched
                parameter_Database.AddInParameter(l_DbCommand, "@BitNotIncludeAMMatched ", DbType.Boolean, Bool_NotIncludeAMMatched);
                parameter_Database.AddInParameter(l_DbCommand, "@BitNotIncludeTMMatched ", DbType.Boolean, Bool_NotIncludeTMMatched);
                //--Added By Dilip on 03-12-2009 for AM-Matched & TM-Matched
                parameter_Database.AddOutParameter(l_DbCommand, "@cOutput", DbType.String, 100);
                parameter_Database.ExecuteNonQuery(l_DbCommand, parameterTransaction);
            }
            catch
            {
                throw;
            }

        }




        //DeletePendingRefundRequest
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

        //Added Ganeswar 10thApril09 for BA Account Phase-V /begin
        public static bool IncludeorExcludeYMCAMoney(string parameterAccountBreakdowntypes, string parameterAccountType, string parameterRefundRequestID)
        {

            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            try
            {

                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null)
                {
                    throw new Exception("Connection to the SQL Server is not Established");

                }

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_IncludeorExcludeYMCAMoneyStatus");

                if (l_DbCommand == null) return false;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_AccountBreakdowntypes", DbType.String, parameterAccountBreakdowntypes);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_AccountType", DbType.String, parameterAccountType);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterRefundRequestID);
                l_DataBase.AddOutParameter(l_DbCommand, "@integer_AccountExists", DbType.Int16, 4);

                l_DataBase.ExecuteNonQuery(l_DbCommand);

                if (Convert.ToInt16(l_DataBase.GetParameterValue(l_DbCommand, "@integer_AccountExists")) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                throw;
            }


        }
        //Added Ganeswar 10thApril09 for BA Account Phase-V /begin

        //Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
        public static void CancelPendingRefundRequest(string parameterRefundRequestID, string parameterRefCanResCode)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_CancelRefundRequest");

                if (l_DbCommand == null) return;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterRefundRequestID);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_Updator", DbType.String, "Yrs"); // User ID has to go here.
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefCanResCode", DbType.String, parameterRefCanResCode);

                l_DataBase.ExecuteNonQuery(l_DbCommand);

            }
            catch
            {
                throw;
            }
        }

        //Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
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


        public static DataSet GetDisbursementRefundsAndActions(string parameterRefundRequestID)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetDisbursementRefundsandActions");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterRefundRequestID);

                // Add the DataSet
                l_DataSet = new DataSet("DisbursementRefund");


                l_stringDataTableNames = new string[] { "Refund", "Action" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                return l_DataSet;

            }
            catch
            {
                throw;
            }

        }


        public static bool IsDisbursementsExists(string parameterDisbursementID, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand = null;

            try
            {
                if (parameterDatabase == null) return false;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_GetDisbursements");

                if (l_DbCommand == null) return false;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbursementID", DbType.String, parameterDisbursementID);
                parameterDatabase.AddOutParameter(l_DbCommand, "@integer_DisbursementID", DbType.Int16, 4);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);

                if (Convert.ToInt16(parameterDatabase.GetParameterValue(l_DbCommand, "@integer_DisbursementID")) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch
            {
                throw;
            }

        }

        public static DataTable GetRefundRequestDetails(string parameterRefundRequestID)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetRefundRequestDetail");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterRefundRequestID);

                // To set the connenction Time Out. 
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                // Add the DataSet
                l_DataSet = new DataSet("DisbursementRefund");

                l_stringDataTableNames = new string[] { "Refunds" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Refunds"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }


        public static DataTable GetRefundRequestDetails(string parameterRefundRequestID, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {

                if (parameterDatabase == null) return null;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_GetRefundRequestDetail");

                if (l_DbCommand == null) return null;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterRefundRequestID);

                // To set the connenction Time Out. 
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                // Add the DataSet
                l_DataSet = new DataSet("DisbursementRefund");

                l_stringDataTableNames = new string[] { "Refunds" };

                parameterDatabase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames, parameterTransaction);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Refunds"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }


        public static DataTable GetRefundRequestsDetails(string parameterRefundRequestID)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetRefundRequestsDetails");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterRefundRequestID);

                // To set the connenction Time Out. 
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);


                // Add the DataSet
                l_DataSet = new DataSet("RefundRequestDetails");

                l_stringDataTableNames = new string[] { "RefundsRequests" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["RefundsRequests"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }

        //Added by Ashish 2010.05.07 resolve cashout lock issue 
        /// <summary>
        /// This method get refund requestdetails for cashout module
        /// </summary>
        /// <param name="parameterRefundRequestID"></param>
        /// <param name="paraDB"></param>
        /// <param name="paraDbTransaction"></param>
        /// <returns></returns>
        public static DataTable GetRefundRequestsForCashOut(string parameterRefundRequestID, Database paraDB, DbTransaction paraDbTransaction)
        {

            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {


                if (paraDB == null) return null;

                l_DbCommand = paraDB.GetStoredProcCommand("dbo.yrs_usp_RR_GetRefundRequestsDetails");

                if (l_DbCommand == null) return null;

                paraDB.AddInParameter(l_DbCommand, "@varchar_RefundRequestID", DbType.String, parameterRefundRequestID);

                // To set the connenction Time Out. 
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);


                // Add the DataSet
                l_DataSet = new DataSet("RefundRequestDetails");

                l_stringDataTableNames = new string[] { "RefundsRequests" };

                paraDB.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames, paraDbTransaction);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["RefundsRequests"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }


        public static DataTable GetDisbursementDetails(string parameterDisbursementID)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {

                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetDisbursementDetails");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_DisbursementID", DbType.String, parameterDisbursementID);

                // Add the DataSet
                l_DataSet = new DataSet("DisbursementRefund");

                l_stringDataTableNames = new string[] { "Disbursement" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Disbursement"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }


        public static DataTable GetDisbursementDetails(string parameterDisbursementID, Database parameterDatabase, DbTransaction parameterTranasction)
        {

            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {

                if (parameterDatabase == null) return null;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_GetDisbursementDetails");

                if (l_DbCommand == null) return null;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbursementID", DbType.String, parameterDisbursementID);

                // Add the DataSet
                l_DataSet = new DataSet("DisbursementRefund");

                l_stringDataTableNames = new string[] { "Disbursement" };

                parameterDatabase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames, parameterTranasction);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Disbursement"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }


        public static DataTable GetTransactionDetails(string parameterTransactionID)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {

                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetTransactionActs");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_TransactionID", DbType.String, parameterTransactionID);

                // Add the DataSet
                l_DataSet = new DataSet("DisbursementRefund");

                l_stringDataTableNames = new string[] { "Transaction" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Transaction"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }


        public static DataTable GetTransactionDetails(string parameterTransactionID, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {

                if (parameterDatabase == null) return null;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_GetTransactionActs");

                if (l_DbCommand == null) return null;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransactionID", DbType.String, parameterTransactionID);

                // Add the DataSet
                l_DataSet = new DataSet("DisbursementRefund");

                l_stringDataTableNames = new string[] { "Transaction" };

                parameterDatabase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames, parameterTransaction);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Transaction"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }

        public static int GetLoanStatus(string parameterPersId)
        {
            DbCommand l_DbCommand;
            Database db = null;
            int l_int_count = 0;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return l_int_count;
                l_DbCommand = db.GetStoredProcCommand("dbo.yrs_usp_RR_GetLoanStatus");

                l_DbCommand.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DbCommand == null) return l_int_count;


                db.AddInParameter(l_DbCommand, "@varchar_guiPersId", DbType.String, parameterPersId);
                db.AddOutParameter(l_DbCommand, "@int_count", DbType.Int32, 2);
                db.ExecuteNonQuery(l_DbCommand);
                l_int_count = Convert.ToInt32(db.GetParameterValue(l_DbCommand, "@int_count"));
                return l_int_count;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
 		
		//START: SR | 07/24/2019 | YRS-AT-4498 | Added new method to check Pre loan requirement for Hardship.
        public static bool IsPreLoanRequiredForHardship(string parameterPersId, decimal tdAmount)
        {
            DbCommand dbCommand;
            Database db = null;
            bool isHardshipWithLoanAllowed = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;
                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_RR_IsPreLoanRequiredForHardship");

                dbCommand.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (dbCommand == null) return false;

                db.AddInParameter(dbCommand, "@VARCHAR_guiPersId", DbType.String, parameterPersId);
                db.AddInParameter(dbCommand, "@NUMERIC_TDAmount", DbType.Decimal, tdAmount);
                db.AddOutParameter(dbCommand, "@BIT_IsHardshipWithLoanAllowed", DbType.Boolean, 10);              
                db.ExecuteNonQuery(dbCommand);
                isHardshipWithLoanAllowed = Convert.ToBoolean (db.GetParameterValue(dbCommand, "@BIT_IsHardshipWithLoanAllowed"));
                return isHardshipWithLoanAllowed;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        //END: SR | 07/24/2019 | YRS-AT-4498 | Added new method to check Pre loan requirement for Hardship.
        

        public static void InsertTransaction(DataSet parameterDataSet)
        {

            Database l_DataBase = null;

            DbCommand InsertDbCommand = null;
            DbCommand UpdateDbCommand = null;
            DbCommand DeleteDbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (parameterDataSet == null) return;

                if (l_DataBase == null) return;

                InsertDbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertTransaction");

                if (InsertDbCommand != null)
                {
                    l_DataBase.AddInParameter(InsertDbCommand, "@varchar_FundEventID", DbType.String, "FundEventID", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@varchar_PersID", DbType.String, "PersID", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@varchar_YmcaID", DbType.String, "YmcaID", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@varchar_AcctType", DbType.String, "AcctType", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@varchar_TransactType", DbType.String, "TransactType", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@varchar_AnnuityBasisType", DbType.String, "AnnuityBasisType", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@Numeric_MonthlyComp", DbType.Decimal, "MonthlyComp", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@Numeric_PersonalPreTax", DbType.Decimal, "PersonalPreTax", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@Numeric_PersonalPostTax", DbType.Decimal, "PersonalPostTax", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@Numeric_YmcaPreTax", DbType.Decimal, "YmcaPreTax", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@DateTime_TransactDate", DbType.DateTime, "TransactDate", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@varchar_TransmittalID", DbType.String, "TransmittalID", DataRowVersion.Default);
                    l_DataBase.AddInParameter(InsertDbCommand, "@varchar_TransactionRefID", DbType.String, "TransactionRefID", DataRowVersion.Default);
                }

                UpdateDbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateRefundRequest");

                if (UpdateDbCommand != null)
                {
                    // this is the Dummy Wrapper for .NET ET.
                }

                DeleteDbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_DeleteRefundRequest");

                if (DeleteDbCommand != null)
                {
                    // this is the Dummy Wrapper for .NET ET.
                }

                l_DataBase.UpdateDataSet(parameterDataSet, "Transaction", InsertDbCommand, UpdateDbCommand, DeleteDbCommand, UpdateBehavior.Standard);

            }
            catch
            {
                throw;
            }
        }


        public static void InsertTransaction(DataSet parameterDataSet, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand InsertDbCommand = null;
            DbCommand UpdateDbCommand = null;
            DbCommand DeleteDbCommand = null;

            try
            {

                if (parameterDataSet == null) return;

                if (parameterDatabase == null) return;

                InsertDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertTransaction");

                if (InsertDbCommand != null)
                {
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_FundEventID", DbType.String, "FundEventID", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_PersID", DbType.String, "PersID", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_YmcaID", DbType.String, "YmcaID", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_AcctType", DbType.String, "AcctType", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_TransactType", DbType.String, "TransactType", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_AnnuityBasisType", DbType.String, "AnnuityBasisType", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@Numeric_MonthlyComp", DbType.Decimal, "MonthlyComp", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@Numeric_PersonalPreTax", DbType.Decimal, "PersonalPreTax", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@Numeric_PersonalPostTax", DbType.Decimal, "PersonalPostTax", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@Numeric_YmcaPreTax", DbType.Decimal, "YmcaPreTax", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@DateTime_TransactDate", DbType.DateTime, "TransactDate", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_TransmittalID", DbType.String, "TransmittalID", DataRowVersion.Default);
                    parameterDatabase.AddInParameter(InsertDbCommand, "@varchar_TransactionRefID", DbType.String, "TransactionRefID", DataRowVersion.Default);
                }

                UpdateDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateRefundRequest");

                if (UpdateDbCommand != null)
                {
                    // this is the Dummy Wrapper for .NET ET.
                }

                DeleteDbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_DeleteRefundRequest");

                if (DeleteDbCommand != null)
                {
                    // this is the Dummy Wrapper for .NET ET.
                }

                parameterDatabase.UpdateDataSet(parameterDataSet, "Transaction", InsertDbCommand, UpdateDbCommand, DeleteDbCommand, parameterTransaction);

            }
            catch
            {
                throw;
            }
        }



        public static void InsertTransaction(DataTable parameterDataTable, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand = null;

            try
            {

                if (parameterDataTable == null) return;

                if (parameterDatabase == null) return;

                foreach (DataRow l_DataRow in parameterDataTable.Rows)
                {
                    l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertTransaction");

                    if (l_DbCommand == null) continue;

                    if (l_DataRow == null) continue;

                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundEventID", DbType.String, Convert.ToString(l_DataRow["FundEventID"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PersID", DbType.String, Convert.ToString(l_DataRow["PersID"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_YmcaID", DbType.String, Convert.ToString(l_DataRow["YmcaID"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AcctType", DbType.String, Convert.ToString(l_DataRow["AcctType"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransactType", DbType.String, Convert.ToString(l_DataRow["TransactType"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AnnuityBasisType", DbType.String, Convert.ToString(l_DataRow["AnnuityBasisType"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_MonthlyComp", DbType.Decimal, Convert.ToDecimal(l_DataRow["MonthlyComp"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_PersonalPreTax", DbType.Decimal, Convert.ToDecimal(l_DataRow["PersonalPreTax"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_PersonalPostTax", DbType.Decimal, Convert.ToDecimal(l_DataRow["PersonalPostTax"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_YmcaPreTax", DbType.Decimal, Convert.ToDecimal(l_DataRow["YmcaPreTax"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@DateTime_TransactDate", DbType.DateTime, Convert.ToDateTime(l_DataRow["TransactDate"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransmittalID", DbType.String, Convert.ToString(l_DataRow["TransmittalID"]));
                    parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransactionRefID", DbType.String, Convert.ToString(l_DataRow["TransactionRefID"]));

                    parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);

                }


            }
            catch
            {
                throw;
            }
        }


        public static void InsertDisbursement(string parameterDisbursementID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertDisbursement");

                if (l_DbCommand == null) return;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_DisbursementID", DbType.String, parameterDisbursementID);

                l_DataBase.ExecuteNonQuery(l_DbCommand);

            }
            catch
            {
                throw;
            }
        }


        public static void InsertDisbursement(string parameterDisbursementID, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand = null;

            try
            {

                if (parameterDatabase == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertDisbursement");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbursementID", DbType.String, parameterDisbursementID);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);

            }
            catch
            {
                throw;
            }
        }



        public static DataSet GetMemberFundEvent(string parameterPersonID, string parameterFundID)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {

                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetMemeberFundEvent");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, parameterPersonID);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);

                // Add the DataSet
                l_DataSet = new DataSet("MemberFund");

                l_stringDataTableNames = new string[] { "FundEvent", "FundEventHistory" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                return l_DataSet;

            }
            catch
            {
                throw;
            }

        }


        public static DataSet GetMemberFundEvent(string parameterPersonID, string parameterFundID, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {

                if (parameterDatabase == null) return null;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_GetMemeberFundEvent");

                if (l_DbCommand == null) return null;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, parameterPersonID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);


                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                // Add the DataSet
                l_DataSet = new DataSet("MemberFund");

                l_stringDataTableNames = new string[] { "FundEvent", "FundEventHistory" };

                parameterDatabase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames, parameterTransaction);

                return l_DataSet;

            }
            catch
            {
                throw;
            }

        }


        public static void UpdateFundEventStatus(string parameterFundEventID, string parameterStatusType, DateTime parameterStatusDate)
        {

            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateMemberFundEvent");

                if (l_DbCommand == null) return;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundEventID);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_StatusType", DbType.String, parameterStatusType);
                l_DataBase.AddInParameter(l_DbCommand, "@dateTime_StatusDate", DbType.DateTime, parameterStatusDate);

                l_DataBase.ExecuteNonQuery(l_DbCommand);

            }
            catch
            {
                throw;
            }
        }


        public static void UpdateFundEventStatus(string parameterFundEventID, string parameterStatusType, DateTime parameterStatusDate, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand = null;

            try
            {
                if (parameterDatabase == null) return;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateMemberFundEvent");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundEventID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_StatusType", DbType.String, parameterStatusType);
                parameterDatabase.AddInParameter(l_DbCommand, "@dateTime_StatusDate", DbType.DateTime, parameterStatusDate);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);

            }
            catch
            {
                throw;
            }
        }



        public static DataTable GetQDRODetails(string parameterPersonID)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetQDRODetails");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, parameterPersonID);

                // Add the DataSet
                l_DataSet = new DataSet("QDRORefunds");

                l_stringDataTableNames = new string[] { "QDRO" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["QDRO"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }


        public static DataSet GetRefundTransactionSchemas()
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetRefundTransactionSchemas");

                if (l_DbCommand == null) return null;

                l_DataSet = new DataSet("RefundTransactionSchemas");

                //neeraj 16-11-2010 : added table name MrdRecordsDisbursements
                l_TableNames = new string[] { "Transactions", "RolloverInstitutions", "Disbursements", "DisbursementDetails", "DisbursementWithholding", "DisbursementRefunds", "ReissueTransaction", "MrdRecordsDisbursements" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                return l_DataSet;

            }
            catch
            {
                throw;
            }

        }

        public static DataTable GetTransactions(string parameterFundEventID)
        {


            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetTransaction");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundEventID", DbType.String, parameterFundEventID);

                // Add the DataSet
                l_DataSet = new DataSet("Transactions");

                l_stringDataTableNames = new string[] { "Transactions" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Transactions"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }

        public static DataTable GetPartialWithdrawalTransaction(string parameterRefRequestID)
        {


            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetPartialWithdrawalTransaction ");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefRequestID", DbType.String, parameterRefRequestID);

                // Add the DataSet
                l_DataSet = new DataSet("Transactions");

                l_stringDataTableNames = new string[] { "Transactions" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Transactions"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }


        public static DataTable GetTransactionsForMarket(string parameterFundEventId, string parameterRefRequestID)
        {


            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetAllWithdrawalTransactionPlanWise");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundEventid", DbType.String, parameterFundEventId);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefRequestID", DbType.String, parameterRefRequestID);

                // Add the DataSet
                l_DataSet = new DataSet("Transactions");

                l_stringDataTableNames = new string[] { "Transactions" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Transactions"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }
        public static DataTable GetTransactionsForMarketForSaveProcess(string parameterFundEventId, string parameterRefRequestID)
        {


            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetAllWithdrawalTransaction");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundEventid", DbType.String, parameterFundEventId);
                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefRequestID", DbType.String, parameterRefRequestID);

                // Add the DataSet
                l_DataSet = new DataSet("Transactions");

                l_stringDataTableNames = new string[] { "Transactions" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Transactions"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }

        public static DataTable GetTransactionType(string parameterFundEventID)
        {


            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetTransactionType");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundEventID", DbType.String, parameterFundEventID);

                // Add the DataSet
                l_DataSet = new DataSet("TransactionType");

                l_stringDataTableNames = new string[] { "TransactionType" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["TransactionType"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }
        public static DataTable GetPayeeDetails(string parameterInstitutionName)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetPayeeDetails");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_InstitutionName", DbType.String, parameterInstitutionName);

                // Add the DataSet
                l_DataSet = new DataSet("PayeeDetails");

                l_stringDataTableNames = new string[] { "PayeeDetails" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["PayeeDetails"]);
                }

                return null;

            }
            catch
            {
                throw;
            }


        }



        public static DataTable GetDetailsForIdxCreation(string p_StringReportName)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetReportDetails");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_lcReportName", DbType.String, p_StringReportName);
                l_DataBase.AddOutParameter(l_DbCommand, "@varchar_ErrorMessage", DbType.String, 300);

                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                // Add the DataSet
                l_DataSet = new DataSet("IdxDataSet");
                //p_StringErrorMessage = l_DbCommand.GetParameterValue("@varchar_ErrorMessage").ToString(); 
                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, "IdxDataSet");

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["IdxDataSet"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }
        public static DataTable GetDetailsForIdxCreationForYmca(string p_StringReportName)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetReportDetailsForYMCA");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_lcReportName", DbType.String, p_StringReportName);
                l_DataBase.AddOutParameter(l_DbCommand, "@varchar_ErrorMessage", DbType.String, 300);

                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                // Add the DataSet
                l_DataSet = new DataSet("IdxDataSet");
                //p_StringErrorMessage = l_DbCommand.GetParameterValue("@varchar_ErrorMessage").ToString(); 
                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, "IdxDataSet");

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["IdxDataSet"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }
        public static string GetVignettePath(string p_string_ServerName)
        {
            string l_string_FilePath = "";

            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("yrs_usp_GetServerLookUp");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@chrServerName", DbType.String, p_string_ServerName);
                //2010.05.08 : Priya Jawale: BT-554 : IDX and PDF files are not copied in proper destination for 9th business day. 
                //chnages inparameter to add parameter
                l_DataBase.AddOutParameter(l_DbCommand, "@chvFilePath", DbType.String, 1000);
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataBase.ExecuteNonQuery(l_DbCommand);

                l_string_FilePath = l_DataBase.GetParameterValue(l_DbCommand, "@chvFilePath").ToString();
                return l_string_FilePath;
            }
            catch
            {
                throw;
            }
        }
        public static string GetServerName()
        {
            Database l_DataBase = null;

            DbCommand l_DbCommand = null;
            string l_string_ServerName = "";
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("ap_GetServerName");

                if (l_DbCommand == null) return null;

                l_DataBase.AddOutParameter(l_DbCommand, "@cServerName", DbType.String, 1000);
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataBase.ExecuteNonQuery(l_DbCommand);

                l_string_ServerName = l_DataBase.GetParameterValue(l_DbCommand, "@cServerName").ToString();
                return l_string_ServerName;
            }
            catch
            {
                throw;
            }
        }
        public static DataTable GetDataTableRefundable(string parameterFundID)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_TransSumForRefundable");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);

                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                // Add the DataSet
                l_DataSet = new DataSet("RefundableDataSet");

                l_stringDataTableNames = new string[] { "Refundable" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Refundable"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }


        public static DataTable GetDataTableRefundable(string parameterFundID, Database parameterDatabase, DbTransaction paramaterTransaction)
        {

            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                //l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

                if (parameterDatabase == null) return null;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_TransSumForRefundable");

                if (l_DbCommand == null) return null;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);

                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                // Add the DataSet
                l_DataSet = new DataSet("RefundableDataSet");

                l_stringDataTableNames = new string[] { "Refundable" };

                parameterDatabase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames, paramaterTransaction);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["Refundable"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }



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
                        TerminationWatcherDA.UpdateTerminationWatcher(parameterPersonID, parameterFundID, "Withdrawal", planType, l_DbTransaction, l_Database,l_string_RefRequestID);
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
                    if(HelperFunctions.isNonEmpty(dtAtsMrdRecordsUpdate))
                    {
                        if(dtAtsMrdRecordsUpdate.Rows.Count>0)
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

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateRefundRequests");

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

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_InsertRefunds");

                if (l_DbCommand == null) return;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_RefRequestsID", DbType.String, Convert.ToString(parameterDataRow["RefRequestsID"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_AcctType", DbType.String, Convert.ToString(parameterDataRow["AcctType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_Taxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Taxable"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_NonTaxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["NonTaxable"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@Numeric_Tax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Tax"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@integer_TaxRate", DbType.Int32, Convert.ToInt32(parameterDataRow["TaxRate"]));
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

        #endregion




        public static DataTable GetNewBalanceDataTable(string parameterFundID, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DataTable l_DataTable = null;

            try
            {
                l_DataTable = GetDataTableRefundable(parameterFundID, parameterDatabase, parameterTransaction);

                if (l_DataTable == null) return null;

                foreach (DataRow l_DataRow in l_DataTable.Rows)
                {
                    l_DataRow["Emp.Total"] = Convert.ToDecimal(l_DataRow["Taxable"]) + Convert.ToDecimal(l_DataRow["Non-Taxable"]) + Convert.ToDecimal(l_DataRow["Interest"]);
                    l_DataRow["YMCATotal"] = Convert.ToDecimal(l_DataRow["YMCATaxable"]) + Convert.ToDecimal(l_DataRow["YMCAInterest"]);

                    l_DataRow["Total"] = Convert.ToDecimal(l_DataRow["Emp.Total"]) + Convert.ToDecimal(l_DataRow["YMCATotal"]);

                }

                return l_DataTable;

            }
            catch
            {
                throw;
            }

        }

        private static bool ChangeStatusEmployeeFundEvent(string parameterPersonID, string parameterFundID, string parameterStatusType, string parameterCheckStatus, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand l_DbCommand;

            try
            {

                if (parameterDatabase == null) return false;

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateMemberFundStatus");

                if (l_DbCommand == null) return false;

                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_PersonID", DbType.String, parameterPersonID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundID);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_StatusType", DbType.String, parameterStatusType);
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_CheckStatusType", DbType.String, parameterCheckStatus);

                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);

                return true;

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





        #region " Cancel Refund request "


        /***********************************************************************************************
		* This Sgment is used to Cancel a Request for a Refund Disbursement from the Refunds Screen
		* All of the Disbursements associated with the Refund Request must be UnFunded
		* The DisbursementActions Table will have a row Added to it for each Disbursement Request
		* The Transaction Table will have entries added to reverse the Refund
		* Interest will be assesed for the account and added if needed
		* The Refund Request will have it's Status Changed to Cancelled
		* The Fund Event will have it's Status Updates
		* User will be Notified to determine if Participant's Personal Resolution needs to be Updated
		***********************************************************************************************/

        //Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
        public static string CancelDisbursementRefund(string parameterRefundRequestID, string parameterRefCanResCode)
        {
            RefundRequestDAClass m_RefundRequestDAClass;
            m_RefundRequestDAClass = new RefundRequestDAClass();

            //Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
            return (m_RefundRequestDAClass.CancelDisbursementRefundRequest(parameterRefundRequestID, parameterRefCanResCode));
        }

        //Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
        public string CancelDisbursementRefundRequest(string parameterRefundRequestID, string parameterRefCanResCode)
        {
            DataSet l_Dataset_Disbursement = null;
            DataTable l_DataTable_Refund = null;

            Database l_Database = null;
            DbTransaction l_DbTransaction = null;
            DbConnection l_DbConnection = null;

            string l_string_ErrorMessage;


            try
            {

                this.m_DataTable_Transaction = null;

                l_Dataset_Disbursement = RefundRequestDAClass.GetDisbursementRefundsAndActions(parameterRefundRequestID);

                if (l_Dataset_Disbursement == null) return "There is no Disbursement Details available for the given Refund Request.";

                l_DataTable_Refund = l_Dataset_Disbursement.Tables["Refund"];

                if (l_DataTable_Refund != null)
                {
                    if (l_DataTable_Refund.Rows.Count < 1) return "There is no Disbursement Details available for the given Refund Request.";


                    // Now Start with Transaction.
                    l_Database = DatabaseFactory.CreateDatabase("YRS");

                    if (l_Database == null) return "Database Connection failure.";

                    l_DbConnection = l_Database.CreateConnection();

                    if (l_DbConnection == null) return "Database Connection failure.";

                    l_DbConnection.Open();

                    l_DbTransaction = l_DbConnection.BeginTransaction();

                    if (l_DbTransaction == null) return "Database Connection failure.";

                    // End of Transaction part.


                    // Check for all refund is Unfunded are not. 
                    // Else Show the Error message with Disbursement ID

                    // if the string is not empty then, there are Funded disbursement is there. 
                    l_string_ErrorMessage = AllDisbursementUnpaid(l_DataTable_Refund, l_Database, l_DbTransaction);

                    if (l_string_ErrorMessage != "")
                    {
                        return l_string_ErrorMessage;
                    }

                    // Yes.. all the Checks are through.
                    // Now start Reverse transaction. 

                    foreach (DataRow l_DataRow_Refund in l_DataTable_Refund.Rows)
                    {
                        ReverseTransactions(l_DataRow_Refund["RefRequestID"].ToString(), l_DataRow_Refund["DisbursementID"].ToString(), l_Database, l_DbTransaction);
                    }

                    // Now update the AtsDisbursementActions

                    foreach (DataRow l_DataRow_Refund in l_DataTable_Refund.Rows)
                    {
                        InsertDisbursement(l_DataRow_Refund["DisbursementID"].ToString(), l_Database, l_DbTransaction);
                    }

                    //neeraj 18-11-2010 BT-664 : create -ve entry in [AtsMrdRecordsDisbursements] table
                    foreach (DataRow l_DataRow_Refund in l_DataTable_Refund.Rows)
                    {
                        l_string_ErrorMessage = UpdateMrdRecordsDisbursements(l_DataRow_Refund["DisbursementID"].ToString(), l_Database, l_DbTransaction);
                    }


                    if (l_string_ErrorMessage != "")
                    {
                        l_DbTransaction.Rollback();
                        return l_string_ErrorMessage;
                    }
                    //end BT-664


                    // Reset all refunds. 

                    //l_string_ErrorMessage = this.ResetFundEventStatus (this.PersonID, this.FundID, this.StatusDate, l_Database, l_DbTransaction); // -By Aparna  30/08/2007
                    //Set the Fundevent Status to the previous value. -By Aparna  30/08/2007
                    
                    //START | SR | 2016.03.02 | YRS-AT-1257/YRS-AT-2281
                    //l_string_ErrorMessage = this.ResetFundEventStatus(this.FundID, parameterRefundRequestID, l_Database, l_DbTransaction);
                    l_string_ErrorMessage = this.ResetFundEventStatus(this.FundID, l_Database, l_DbTransaction);
                    //END | SR | 2016.03.02 | YRS-AT-1257/YRS-AT-2281

                    if (l_string_ErrorMessage != "")
                    {
                        return l_string_ErrorMessage;
                    }

                    //Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
                    CancelPendingRefundRequest(parameterRefundRequestID, l_Database, l_DbTransaction, parameterRefCanResCode);

                    l_DbTransaction.Commit();

                    return "";

                }
                else
                    return "There is no Disbursement Details available for the given Refund Request.";

            }
            catch
            {
                if (l_DbTransaction != null)
                {
                    l_DbTransaction.Rollback();
                }

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

        private string AllDisbursementUnpaid(DataTable paramaterDisbursementTable, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            //DataRow l_DataRow = null;

            bool l_DisbursementFlag = false;
            string l_String_Message = "";

            try
            {

                if (paramaterDisbursementTable == null) return l_String_Message;

                if (paramaterDisbursementTable.Rows.Count < 1) return l_String_Message;

                l_String_Message = "";

                foreach (DataRow l_DataRow in paramaterDisbursementTable.Rows)
                {
                    l_DisbursementFlag = RefundRequestDAClass.IsDisbursementsExists(l_DataRow["DisbursementID"].ToString().Trim(), parameterDatabase, parameterTransaction);

                    if (l_DisbursementFlag == true)
                    {
                        l_String_Message += @"\n Check : " + l_DataRow["DisbursementID"].ToString().Trim();
                    }
                }

                if (l_String_Message != "")
                {
                    return ("There are folowing Disbursements that have been funded for this Refund Request \n" + l_String_Message);
                }

                return l_String_Message;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ReverseTransactions(string parameterRefundRequestID, string parameterDisbursementID, Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DataTable l_DataTable_RefundRequests = null;
            DataTable l_DataTable_Disbursement = null;
            DataTable l_DataTable_TransActs = null;

            DataSet l_DataSet_Transaction = null;

            try
            {
                // Get the Employe details.
                l_DataTable_RefundRequests = GetRefundRequestDetails(parameterRefundRequestID, parameterDatabase, parameterTransaction);

                // If there is no records then Return with error message.
                if (l_DataTable_RefundRequests == null) return "Unable to Locate the Refund Request";

                if (l_DataTable_RefundRequests.Rows.Count < 1) return "Unable to Locate the Refund Request";

                // Set the Refund Request Details, which is used to pass the paramter to 
                // ResetFundEvent Method.

                this.PersonID = l_DataTable_RefundRequests.Rows[0]["PersID"].ToString();
                this.FundID = l_DataTable_RefundRequests.Rows[0]["FundEventID"].ToString();
                this.StatusDate = Convert.ToDateTime(l_DataTable_RefundRequests.Rows[0]["StatusDate"]);


                //Get the Disbursement details. 

                l_DataTable_Disbursement = RefundRequestDAClass.GetDisbursementDetails(parameterDisbursementID, parameterDatabase, parameterTransaction);

                if (l_DataTable_Disbursement == null) return "Unable to Locate the Distribution Details for the Refund Request.";

                if (l_DataTable_Disbursement.Rows.Count < 1) return "Unable to Locate the Distribution Details for the Refund Request.";


                // Get the Transaction Details

                foreach (DataRow l_DataRow in l_DataTable_Disbursement.Rows)
                {
                    //l_DataTable_TransActs = YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetTransactionDetails (l_DataRow ["TransactionRefID"].ToString ());

                    l_DataTable_TransActs = GetTransactionDetails(l_DataRow["TransactID"].ToString(), parameterDatabase, parameterTransaction);

                    if (l_DataTable_TransActs == null) continue;

                    if (this.m_DataTable_Transaction == null)
                    {
                        this.m_DataTable_Transaction = l_DataTable_TransActs.Clone();
                        this.m_DataTable_Transaction.TableName = "Transaction";
                    }


                    // Add into Transaction Table.
                    //this.AddIntoTransactionTable (l_DataRow);
                    if (l_DataTable_TransActs.Rows.Count > 0)
                        AddIntoTransactionTable(l_DataTable_TransActs.Rows[0]);
                }

                if (this.m_DataTable_Transaction != null)
                {
                    l_DataSet_Transaction = new DataSet("Transaction");

                    l_DataSet_Transaction.Tables.Add(this.CopyDataTable(this.m_DataTable_Transaction));

                    //InsertTransaction (l_DataSet_Transaction, parameterDatabase, parameterTransaction);

                    InsertTransaction(m_DataTable_Transaction, parameterDatabase, parameterTransaction);

                    this.m_DataTable_Transaction = null;
                }


            }
            catch
            {
                throw;
            }
            return "";
        }

        private DataTable CopyDataTable(DataTable parameterDataTable)
        {
            DataTable l_DataTable;

            if (parameterDataTable == null) return null;

            l_DataTable = parameterDataTable.Clone();

            foreach (DataRow l_DataRow in l_DataTable.Rows)
            {
                l_DataTable.ImportRow(l_DataRow);
            }

            return l_DataTable;

        }

        private void AddIntoTransactionTable(DataRow parameterDataRow)
        {
            DataRow l_DataRow;

            if (parameterDataRow == null) return;

            try
            {
                if (this.m_DataTable_Transaction == null) return;

                l_DataRow = this.m_DataTable_Transaction.NewRow();

                l_DataRow["PersID"] = parameterDataRow["PersID"];
                l_DataRow["FundEventID"] = parameterDataRow["FundEventID"];
                l_DataRow["YmcaID"] = parameterDataRow["YmcaID"];
                l_DataRow["AcctType"] = parameterDataRow["AcctType"];
                l_DataRow["TransactType"] = parameterDataRow["TransactType"];
                l_DataRow["AnnuityBasisType"] = parameterDataRow["AnnuityBasisType"];
                l_DataRow["MonthlyComp"] = parameterDataRow["MonthlyComp"];
                l_DataRow["PersonalPreTax"] = Convert.ToDouble(parameterDataRow["PersonalPreTax"].ToString()) * -1.00;
                l_DataRow["PersonalPostTax"] = Convert.ToDouble(parameterDataRow["PersonalPostTax"]) * -1.00;
                l_DataRow["YmcaPreTax"] = Convert.ToDouble(parameterDataRow["YmcaPreTax"]) * -1.00;
                l_DataRow["TransactDate"] = parameterDataRow["TransactDate"];
                l_DataRow["TransmittalID"] = parameterDataRow["TransmittalID"];
                l_DataRow["TransactionRefID"] = parameterDataRow["TransactionRefID"];

                this.m_DataTable_Transaction.Rows.Add(l_DataRow);

            }
            catch
            {
                throw;
            }
        }

        //by Aparna Modify Fundevent status -When WD Update to TM When RD Update to RT -30/08/2007

        //START | SR | 2016.03.02 | YRS-AT-1257/YRS-AT-2281 | Reset fundstatus based on current FundeventTransitionrules recorded in atsFundEventsTransitionRules table 
        private string ResetFundEventStatus(string parameterFundEventID,  Database parameterDatabase, DbTransaction parameterTransaction)
        {
            DbCommand l_DbCommand = null;
            try
            {

                if (parameterDatabase == null) return "";

                l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_ResetFundEventStatus");
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DbCommand == null) return "";
                parameterDatabase.AddInParameter(l_DbCommand, "@chvFundEventId", DbType.String, parameterFundEventID);
                parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);
                return "";
            }
            catch
            {
                throw;
            }
        }

        //private string ResetFundEventStatus(string parameterFundEventID, string parameterRefundRequestID, Database parameterDatabase, DbTransaction parameterTransaction)
        //{

        //    DbCommand l_DbCommand = null;

        //    try
        //    {


        //        if (parameterDatabase == null) return "";

        //        l_DbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateFundEventStatus");
        //        l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

        //        if (l_DbCommand == null) return "";

        //        parameterDatabase.AddInParameter(l_DbCommand, "@varchar_FundID", DbType.String, parameterFundEventID);
        //        parameterDatabase.AddInParameter(l_DbCommand, "@varhcar_RefRequestId", DbType.String, parameterRefundRequestID);

        //        parameterDatabase.ExecuteNonQuery(l_DbCommand, parameterTransaction);
        //        return "";

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //END | SR | 2016.03.02 | YRS-AT-1257/YRS-AT-2281 | Reset fundstatus based on current FundeventTransitionrules recorded in atsFundEventsTransitionRules table 

        //	//	private string ResetFundEventStatus (string parameterPersonID, string parameterFundID, DateTime parameterStatusDate, Database parameterDatabase, DbTransaction parameterTransaction)
        //		{
        //			DataSet l_DataSet_MemberFund = null;
        //
        //			DataTable l_DataTable_FundEvent = null;
        //			DataTable l_DataTable_FundEventHistory = null;
        //
        //			DateTime	l_DateTime_FundEventDate;
        //			DataRow		l_DataRow_Fund;
        //
        //			try
        //			{
        //				l_DataSet_MemberFund = GetMemberFundEvent (parameterPersonID, parameterFundID, parameterDatabase, parameterTransaction);
        //
        //				if (l_DataSet_MemberFund == null) return "Unable to get Fund Events. ";
        //
        //				l_DataTable_FundEvent = l_DataSet_MemberFund.Tables ["FundEvent"];
        //
        //				l_DataTable_FundEventHistory = l_DataSet_MemberFund.Tables ["FundEventHistory"];
        //
        //				if (l_DataTable_FundEvent == null) return "Unable to get Fund Events. ";
        //
        //
        //				// Check for the status Type.
        //
        //				l_DataRow_Fund = l_DataTable_FundEvent.Rows [0];
        //
        //				if (l_DataRow_Fund == null) return "Unable to get Fund Events. ";
        //					
        //				
        //				switch (l_DataRow_Fund ["chrStatusType"].ToString ().Trim ())
        //				{
        //					case "AE":
        //						return "";
        //						
        //					case "PE":
        //						return "";
        //
        //					case "DA":
        //						return "";
        //
        //					case "DD":
        //						return "";
        //
        //					case "DR":
        //						return "";
        //						
        //					case "RD":
        //						return "";
        //				}
        //
        //				foreach (DataRow l_DataRow in l_DataTable_FundEventHistory.Rows)
        //				{					
        //					l_DateTime_FundEventDate = Convert.ToDateTime (l_DataRow ["StatusDate"]);
        //
        //					if (DateTime.Compare (l_DateTime_FundEventDate, parameterStatusDate) >= 0)
        //					{
        //						UpdateFundEventStatus (l_DataRow["UniqueID"].ToString (), l_DataRow["StatusType"].ToString (), l_DateTime_FundEventDate, parameterDatabase, parameterTransaction);
        //						break;
        //					}
        //				}
        //
        //				return "";
        //					
        //			}
        //			catch (Exception ex)
        //			{
        //				throw ex;
        //			}
        //		}


        #endregion

        /* Added By Ruchi For Updation of Tax in the method PrintReports*/
        public static void UpdateTax(string parameterUniqueId, double parameterTax)
        {

            Database l_DataBase = null;
            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateTax");

                if (l_DbCommand == null) return;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_UniqueID", DbType.String, parameterUniqueId);
                l_DataBase.AddInParameter(l_DbCommand, "@numeric_Tax", DbType.String, parameterTax);

                // To set the connenction Time Out. 
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataBase.ExecuteNonQuery(l_DbCommand);
                return;

            }
            catch
            {
                throw;
            }
        }
        public static DataSet YMCARefundLetterHardship(string paramaetericPersID)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_rpt_YMCARefundLetterHardship");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@guiPerId", DbType.String, paramaetericPersID);

                // To set the connenction Time Out. 
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataSet = new DataSet("RefundLetterHardship");

                l_stringDataTableNames = new string[] { "LetterHardship" };

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

        public static double GetPrincipal(string parameterPersId)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            double l_Double_Principal = 0.0;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return 0.0;

                l_DbCommand = l_DataBase.GetStoredProcCommand("yrs_usp_RR_GetAvailHDAmt");

                if (l_DbCommand == null) return 0.0;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_guiPersId", DbType.String, parameterPersId);
                l_DataBase.AddOutParameter(l_DbCommand, "@float_principalAmt", DbType.Double, 15);
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataBase.ExecuteNonQuery(l_DbCommand);

                l_Double_Principal = Convert.ToDouble(l_DataBase.GetParameterValue(l_DbCommand, "@float_principalAmt"));
                return l_Double_Principal;
            }
            catch
            {
                throw;
            }
        }

        //added by Shubhrata Nov 25th 2006- to accomodate CashOuts
        //Added By Ganeswar For Partial withdraw on june 26th 2009
        //////////////		public static decimal GetPartialMonthDifference (string parameterFundID,string PrameterPlanType )
        //////////////		{
        //////////////		{
        //////////////			Database l_DataBase = null;
        //////////////			DbCommand l_DbCommand = null;			
        //////////////		
        //////////////			decimal l_DecimalAmount = 0.00M;
        //////////////
        //////////////			try
        //////////////			{
        //////////////				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");
        //////////////
        //////////////				if (l_DataBase == null) return  0.00M;;
        //////////////
        //////////////				l_DbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_QDRO_GetPartialRequestByMonth");
        //////////////
        //////////////				if (l_DbCommand == null) return  0.00M;;
        //////////////
        //////////////						
        //////////////				 l_DataBase.AddInParameter(l_DbCommand,"@varchar_FundID", DbType.String, parameterFundID);
        //////////////				 l_DataBase.AddInParameter(l_DbCommand,"@PlanType",  DbType.String, PrameterPlanType) ;
        //////////////				l_DataBase.AddOutParameter(l_DbCommand,"@numeric_Monthdiff", DbType.Double, 18) ;
        //////////////				l_DataBase.ExecuteNonQuery (l_DbCommand);
        //////////////				if(l_DataBase.GetParameterValue(l_DbCommand,"@numeric_Monthdiff").GetType().ToString()!= "System.DBNull" )
        //////////////				{
        //////////////					l_DecimalAmount =  Convert.ToDecimal (l_DataBase.GetParameterValue(l_DbCommand,"@numeric_Monthdiff"));	
        //////////////				}
        //////////////				return l_DecimalAmount;
        //////////////			}
        //////////////			catch 
        //////////////			{
        //////////////				throw ;
        //////////////			}
        //////////////		}
        //////////////		}

        public static DateTime GetPartialMonthDifferenceDate(string parameterFundID, string PrameterPlanType)
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            DateTime GetPartialMonthDifferenceDate;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");


                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RR_GetPartialRequestByMonth");
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FundID", DbType.String, parameterFundID);
                db.AddInParameter(LookUpCommandWrapper, "@PlanType", DbType.String, PrameterPlanType);

                GetPartialMonthDifferenceDate = Convert.ToDateTime(db.ExecuteScalar(LookUpCommandWrapper));
                return GetPartialMonthDifferenceDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Added By Ganeswar For Partial withdraw on june 26th 2009
        # region "CASHOUTS"
        public static string InsertRefunds(DataSet parameterDataset, Database l_Database, DbTransaction l_DbTransaction)
        {
            string l_RefundRequest = "";
            string l_SubTableRequestID = "";


            try
            {

                l_RefundRequest = InsertRefundRequests(parameterDataset, null, l_Database, l_DbTransaction);


                if (l_RefundRequest != "")
                {
                    l_SubTableRequestID = InsertRefundRequestsPerPlan(parameterDataset, l_RefundRequest, l_Database, l_DbTransaction);

                    if (l_SubTableRequestID != "")
                    {
                        InsertRefundRequestDetails(parameterDataset, l_RefundRequest, l_SubTableRequestID, l_Database, l_DbTransaction);

                    }
                    else
                    {
                        throw new Exception("Pending Request already exists");
                    }


                }
                else
                {
                    throw new Exception("Pending Request already exists");
                }


                return l_RefundRequest;
            }
            catch
            {
                throw;
            }

        }

        public static bool SaveRefundRequestProcess(DataSet parameterDataSet, string parameterPersonID, string parameterFundID, string paramterRefundType, bool parameterVested, bool parameterTerminated, bool parameterTookTDAccount, string parameterStatusType, Database l_Database, DbTransaction l_DbTransaction, out string para_out_string_ExceptionReason)
        {

            string l_DisbursementID = "";
            string l_TransactionID = "";

            string l_OldDisbursementID = "";
            string l_OldTransactionID = "";
            string l_string_RefRequestID = string.Empty;

            DataTable l_Refunds = null;
            DataTable l_Disbursements = null;
            DataTable l_DisbursementDetails = null;
            DataTable l_Transaction = null;
            DataTable l_DisbursementWithHold = null;
            DataTable l_DisbursementRefunds = null;

            //Added By SG: 2012.08.30: BT-960
            DataTable l_RMDDisbursement = null;

            DataTable l_RefundRequest = null;
            para_out_string_ExceptionReason = "";

            bool l_TransactionFlag = false;  //--  This falg is used to Keep whether Main transaction is happening r not.
            // B'cos, Disbursemnet, Transaction & Refund is Compulsory for this Transaction. 
            // Otherwise Rolback the Transaction.

            try
            {
                WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Initiate Process is started.");
                if (parameterDataSet == null) return false;

                l_Disbursements = parameterDataSet.Tables["DisbursementsDataTable"];
                l_Transaction = parameterDataSet.Tables["TransactionsDataTable"];
                l_Refunds = parameterDataSet.Tables["RefundsDataTable"];
                l_DisbursementDetails = parameterDataSet.Tables["RefundsDataTable"];
                l_DisbursementWithHold = parameterDataSet.Tables["DisbursementWithholdingDataTable"];
                l_DisbursementRefunds = parameterDataSet.Tables["DisbursementRefundsDataTable"];

                l_RefundRequest = parameterDataSet.Tables["RefundRequestDataTable"];

                //Added By SG: 2012.08.30: BT-960
                l_RMDDisbursement = parameterDataSet.Tables["RMDDisbursementDataTable"];



                if ((l_Disbursements != null) && (l_Transaction != null) && (l_Refunds != null) && (l_RefundRequest != null))
                {

                    //  Now create connection to maintain Transaction.


                    l_TransactionFlag = false;
                    foreach (DataRow l_DisbursementDataRow in l_Disbursements.Rows)
                    {
                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Loop for disbursement.");
                        if (l_DisbursementDataRow != null)
                        {
                            if (l_DisbursementDataRow["UniqueID"].GetType().ToString() == "System.DBNull")
                                l_OldDisbursementID = "";
                            else
                                l_OldDisbursementID = l_DisbursementDataRow["UniqueID"].ToString();

                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: InsertDisbursements.");
                            l_DisbursementID = InsertDisbursements(l_DisbursementDataRow, l_Database, l_DbTransaction);
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: InsertDisbursements.");
                            // l_Disbursement ID is very important because all other tables are needed this UniqueID.
                            // So, If l_Disbursement ID is null then Ignore the Transaction.Otherwise gohead. 

                            if (l_DisbursementID.Trim() != string.Empty)
                            {
                                l_Disbursements = UpdateDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementsDataTable"], l_DisbursementID, l_OldDisbursementID);
                                l_DisbursementDetails = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementDetailsDataTable"], l_DisbursementID, l_OldDisbursementID);
                                l_Refunds = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["RefundsDataTable"], l_DisbursementID, l_OldDisbursementID);
                                l_DisbursementWithHold = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementWithholdingDataTable"], l_DisbursementID, l_OldDisbursementID);
                                l_DisbursementRefunds = ApplyDisbursemnentID((DataTable)parameterDataSet.Tables["DisbursementRefundsDataTable"], l_DisbursementID, l_OldDisbursementID);

                                //Added By SG: 2012.08.30: BT-960
                                if (l_RMDDisbursement != null)
                                    WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Update RMD disbursements.");
                                    l_RMDDisbursement = UpdateDisbursemnentIDInMrdRecordsDisbursement((DataTable)parameterDataSet.Tables["RMDDisbursementDataTable"], l_DisbursementID, l_OldDisbursementID);
                                    WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Update RMD disbursements.");

                                l_TransactionFlag = true;
                                // Now Disbursement ID is updated in all corresponding DataTable
                            }

                        }
                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Loop for disbursement.");

                    } // Updating Disbursement ID

                    // Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
                    if (l_TransactionFlag == false)
                    {
                        para_out_string_ExceptionReason = "Unable to create disbursements records";

                        return false;	// Return with Error.
                    }


                    //Now we need Transaction ID for Refunds DataTable, so wee needs to Get Transaction ID. 
                    l_TransactionFlag = false;
                    foreach (DataRow l_TransactionDataRow in l_Transaction.Rows)
                    {
                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Loop for Transactions.");
                        if (l_TransactionDataRow != null)
                        {

                            if (l_TransactionDataRow["UniqueID"].GetType().ToString() == "System.DBNull")
                                l_OldTransactionID = "";
                            else
                                l_OldTransactionID = l_TransactionDataRow["UniqueID"].ToString();


                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Insert into Transaction details.");
                            l_TransactionID = InsertTransactions(l_TransactionDataRow, l_Database, l_DbTransaction);
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Insert into Transaction details.");

                            if (l_TransactionID.Trim() != string.Empty)
                            {
                                l_Refunds = ApplyTransactionID(l_Refunds, l_TransactionID, l_OldTransactionID);
                                l_TransactionFlag = true;
                            }

                            // Now Transaction ID is updated in all corresponding DataTable
                        }
                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Loop for Transactions.");
                    }

                    // Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
                    if (l_TransactionFlag == false)
                    {
                        para_out_string_ExceptionReason = "Unable to create transacts records";
                        return false;	// Return with Error.
                    }


                    // Now we needs to RefundsID for Disbursement Refunds.
                    l_TransactionFlag = false;
                    foreach (DataRow l_RefundsDataRow in l_Refunds.Rows)
                    {
                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Loop for Refund Records.");
                        // Modified to accept the Intallement Number.
                        InsertAtsRefundsRecords(l_RefundsDataRow, false, l_Database, l_DbTransaction);
                        //InsertRefunds (l_RefundsDataRow, l_Database, l_DbTransaction);
                        l_TransactionFlag = true;
                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Loop for Refund Records.");
                    }

                    // Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
                    if (l_TransactionFlag == false)
                    {

                        para_out_string_ExceptionReason = "Unable to create refund records";
                        return false;	// Return with Error.
                    }


                    // Now  3 DataTables are succesfully Inserted Now Insert Remaining DataTable..

                    // Insert Disbursement Details.. 
                    if (l_DisbursementDetails != null)
                    {
                        foreach (DataRow l_DisbursementDetailsDataRow in l_DisbursementDetails.Rows)
                        {
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Loop for Insert Disbursement Details.");
                            InsertDisbursementDetails(l_DisbursementDetailsDataRow, l_Database, l_DbTransaction);
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Loop for Insert Disbursement Details.");

                        }

                    }

                    // Insert Disbursement WithHold Details. 
                    if (l_DisbursementWithHold != null)
                    {
                        foreach (DataRow l_WothHoldDataRow in l_DisbursementWithHold.Rows)
                        {
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Loop for Insert Disbursement Withholding.");
                            InsertDisbursementWithHold(l_WothHoldDataRow, l_Database, l_DbTransaction);
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Loop for Insert Disbursement Withholding.");
                        }
                    }


                    // Insert Disbursement Refunds Details. 
                    if (l_DisbursementRefunds != null)
                    {
                        foreach (DataRow l_DisbursementRefundsDataRow in l_DisbursementRefunds.Rows)
                        {
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Loop for Insert Disbursement Refunds.");
                            InsertDisbursementRefunds(l_DisbursementRefundsDataRow, l_Database, l_DbTransaction);
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Loop for Insert Disbursement Refunds.");
                        }
                    }

                    //Added By SG: 2012.08.30: BT-960: Insert Disbursement Refunds Details. 
                    if (l_RMDDisbursement != null)
                    {
                        foreach (DataRow l_RMDDisbursementDataRow in l_RMDDisbursement.Rows)
                        {
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Loop for Insert RMD Disbursement.");
                            InsertMrdRecordsDisbursements(l_RMDDisbursementDataRow, l_Database, l_DbTransaction);
                            WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Loop for Insert RMD Disbursement.");
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
                                    WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Loop for Update Refund Request.");
                                    UpdateRefundRequests(l_RefundRequestDataRow, l_Database, l_DbTransaction);
                                    l_TransactionFlag = true;
                                    WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Loop for Update Refund Request.");
                                }
                            }
                            //Added by Ashish on 21-Oct-2008 ,get RefRquestId for DLIN changes
                            l_string_RefRequestID = l_RefundRequestDataRow["UniqueId"].ToString();
                        }
                        /*added by ruchi to change the status to withdrawn when disbursements are made*/
                        //NP:IVP2:2008.10.23 - Removing out parameter for fund event status
                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Change status.");
                        ChangeStatus(parameterPersonID, parameterFundID, paramterRefundType, parameterVested, parameterTerminated, parameterTookTDAccount, parameterStatusType, l_Database, l_DbTransaction);
                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Change status.");

                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Start: Update stagging InterestRecords.");
                        UpdateStagingInterestRecord(parameterFundID, l_string_RefRequestID, l_Database, l_DbTransaction);
                        WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Update stagging InterestRecords.");
                    }
                    // Check wherther Transaction is happened r not, 
                    if (l_TransactionFlag == false)
                    {
                        para_out_string_ExceptionReason = "Unable to update refund requests status";
                        return false;	// Return with Error.
                    }

                    //1099 population -- Added By SG: 2012.10.16: BT-960
                    l_TransactionFlag = false;
                    if (l_Disbursements != null)
                    {
                        foreach (DataRow dr in l_Disbursements.Rows)
                        {
                            paramterRefundType = Convert.ToString(dr["PayeeEntityTypeCode"]).Trim().ToUpper();
                            Populate1099Values(l_DisbursementID, paramterRefundType, l_Database, l_DbTransaction);

                            l_TransactionFlag = true;
                        }
                    }

                    if (l_TransactionFlag == false)
                    {
                        para_out_string_ExceptionReason = "Unable to insert tax details in update refund requests status";
                        return false;	// Return with Error.
                    }
                    // All are DataTables are succesfully Updated Now
                    return true;
                }
                else
                    return true;


                WebPerformanceTracer.LogPerformanceTrace("SaveRefundRequestProcess Method", "Finish: Initiate Process is started.");
            }
            catch (SqlException sqlEx)
            {

                throw sqlEx;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                //Added By SG: 2012.08.30: BT-960
                l_Refunds.Dispose();
                l_Disbursements.Dispose();
                l_DisbursementDetails.Dispose();
                l_Transaction.Dispose();
                l_DisbursementWithHold.Dispose();
                l_DisbursementRefunds.Dispose();
                l_RefundRequest.Dispose();

                if (l_RMDDisbursement != null)
                    l_RMDDisbursement.Dispose();
            }


        }


        # endregion //CASHOUTS
        # region "Plan Split"
        #region " Change Status "

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

        private static bool ChangeStatus1(string parameterPersonID, string parameterFundID, string parameterRefundType, bool parameterVested, bool parameterTerminated, bool parameterTookTDAccount, string parameterFundEventStatusType, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DataTable l_NewBalanceDataTable = null;

            decimal l_Decimal_EmployeeTotal;
            decimal l_Decimal_YMCATotal;

            decimal l_Decimal_EmployeePreTax;
            decimal l_Decimal_EmployeePostTax;
            decimal l_Decimal_EmployeeInterest;

            decimal l_Decimal_YMCATaxable;
            decimal l_Decimal_YMCAInterest;


            try
            {
                // Get the New Balance from the database.
                l_NewBalanceDataTable = GetNewBalanceDataTable(parameterFundID, parameterDatabase, parameterTransaction);

                if (l_NewBalanceDataTable != null)
                {
                    l_Decimal_EmployeePreTax = Convert.ToDecimal(l_NewBalanceDataTable.Compute("SUM (Taxable)", ""));
                    l_Decimal_EmployeePostTax = Convert.ToDecimal(l_NewBalanceDataTable.Compute("SUM ([Non-Taxable])", ""));
                    l_Decimal_EmployeeInterest = Convert.ToDecimal(l_NewBalanceDataTable.Compute("SUM (Interest)", ""));

                    l_Decimal_YMCATaxable = Convert.ToDecimal(l_NewBalanceDataTable.Compute("SUM (YMCATaxable)", ""));
                    l_Decimal_YMCAInterest = Convert.ToDecimal(l_NewBalanceDataTable.Compute("SUM (YMCAInterest)", ""));

                    l_Decimal_EmployeeTotal = l_Decimal_EmployeePreTax + l_Decimal_EmployeePostTax + l_Decimal_EmployeeInterest;
                    l_Decimal_YMCATotal = l_Decimal_YMCATaxable + l_Decimal_YMCAInterest;


                    //code commented & updated by hafiz on 11Oct2006 - YREN-2698
                    //if ((parameterRefundType.Trim () == "REG") || (parameterRefundType.Trim () == "CASH") || (parameterRefundType.Trim () == "SPEC") || (parameterRefundType.Trim () == "DISAB") || (parameterRefundType.Trim () == "PERS"))
                    //commented and added by Shubhrata ---to accomodate CashOut -- 27th Nov 2006
                    //Modified by SHubhrata YREN-3326 we will remove this if condition
                    //if ((parameterRefundType.Trim () == "REG") || (parameterRefundType.Trim () == "CASH") || (parameterRefundType.Trim () == "SPEC") || (parameterRefundType.Trim () == "DISAB") || (parameterRefundType.Trim () == "PERS") || (parameterRefundType.Trim () == "MILLE"))
                    if (parameterRefundType.Trim() == "VOL")
                    {
                        //** Took a Refund of Voluntary Accounts

                        if ((Decimal.Compare((l_Decimal_EmployeeTotal + l_Decimal_YMCATotal), (decimal)0.01) < 0) && (parameterTerminated == true))
                        {
                            //	REPLACE	chrStatusType WITH "WD", dtmStatusDate WITH DATE() IN r_MemberFundevent

                            //ChangeStatusEmployeeFundEvent (parameterPersonID, parameterFundID, "WD", "WD", parameterDatabase, parameterTransaction);
                            // By Aparna Samala 04/06/2007 YREN-3491
                            ChangeStatusEmployeeFundEvent(parameterPersonID, parameterFundID, "WD", "", parameterDatabase, parameterTransaction);

                        }
                        else if (Decimal.Compare((l_Decimal_EmployeeTotal + l_Decimal_YMCATotal), (decimal)0.01) < 0) // We need Check for the StatusType = "PE" - But this will done by SP itself
                        {
                            // REPLACE	chrStatusType WITH "WD", dtmStatusDate WITH DATE() IN r_MemberFundevent
                            // By Aparna Samala 04/06/2007 YREN-3491
                            ChangeStatusEmployeeFundEvent(parameterPersonID, parameterFundID, "WD", "PE", parameterDatabase, parameterTransaction);
                            //ChangeStatusEmployeeFundEvent (parameterPersonID, parameterFundID, "WD", "PE", parameterDatabase, parameterTransaction);
                            // By Aparna Samala 04/06/2007 YREN-3491
                            //  Change the Employment Elective
                            //ChangeMemberElectives (parameterPersonID, parameterFundID, parameterAccountType, parameterDatabase, parameterTransaction);
                            ChangeMemberElectives(parameterPersonID, parameterFundID, "", parameterDatabase, parameterTransaction);

                        }
                    }
                    //start-added one more if clause for Status Type RT Plan Split Changes
                    //For status RT if at the end of the refund they still have money in any account, the status remains RT.  
                    //If at the end of the refund their accounts are all zero, the status becomes RD (Retired).

                    else if (parameterFundEventStatusType.Trim().ToUpper() == "RT")
                    {
                        if (decimal.Compare(l_Decimal_EmployeeTotal + l_Decimal_YMCATotal, (decimal)0.01) > 0)
                        {
                            ChangeStatusEmployeeFundEvent(parameterPersonID, parameterFundID, "RT", "", parameterDatabase, parameterTransaction);
                        }
                        if (decimal.Compare(l_Decimal_EmployeeTotal + l_Decimal_YMCATotal, (decimal)0.01) < 0)
                        {
                            ChangeStatusEmployeeFundEvent(parameterPersonID, parameterFundID, "RD", "", parameterDatabase, parameterTransaction);
                        }
                    }
                    else
                    //||(parameterRefundType.Trim () == "SHIRA"))
                    //if ((parameterRefundType.Trim () == "REG") || (parameterRefundType.Trim () == "CASH") || (parameterRefundType.Trim () == "SPEC") || (parameterRefundType.Trim () == "DISAB") || (parameterRefundType.Trim () == "PERS") || (parameterRefundType.Trim () == "MILLE")||(parameterRefundType.Trim () == "CSHOUT"))
                    {

                        if (decimal.Compare(l_Decimal_EmployeeTotal + l_Decimal_YMCATotal, (decimal)0.01) < 0)
                        {
                            //**Took a Full Refund Participant has WithDrawn

                            // 	REPLACE	chrStatusType WITH "WD",dtmStatusDate WITH DATE() IN r_MemberFundevent
                            ChangeStatusEmployeeFundEvent(parameterPersonID, parameterFundID, "WD", "", parameterDatabase, parameterTransaction);

                        }
                        else if ((decimal.Compare(l_Decimal_EmployeeTotal, (decimal)0.01) < 0) && (decimal.Compare(l_Decimal_YMCATotal, (decimal)0.00) > 0) && (parameterVested == false))
                        {
                            //	** Took a Full Refund Not Vested and Money Remains on the YMCA Side is now Withdrawn

                            //	REPLACE	chrStatusType WITH "WD", dtmStatusDate WITH DATE() IN r_MemberFundevent

                            ChangeStatusEmployeeFundEvent(parameterPersonID, parameterFundID, "WD", "", parameterDatabase, parameterTransaction);

                        }
                        else if ((decimal.Compare(l_Decimal_EmployeeTotal, (decimal)0.01) < 0) && (decimal.Compare(l_Decimal_YMCATotal, (decimal)0.00) > 0) && (parameterVested == true))
                        {
                            //** Took a Full Refund Vested and Money Remains on the YMCA Side is now Deferred

                            //REPLACE	chrStatusType WITH "DF", dtmStatusDate WITH DATE() IN r_MemberFundevent

                            ChangeStatusEmployeeFundEvent(parameterPersonID, parameterFundID, "DF", "", parameterDatabase, parameterTransaction);

                        }



                    }



                    // Check for Whether Employee took TD Account, if so.. alter Database.

                    if (parameterTookTDAccount == true)
                    {
                        ChangeMemberElectives(parameterPersonID, parameterFundID, "TD", parameterDatabase, parameterTransaction);
                    }

                    return true;

                }
                else
                    return false;


            }
            catch
            {
                throw;
            }

        }

        #endregion "Change Status"
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

        public static string InsertNewRefundRequests(DataRow parameterDataRow, DataSet parameterDataSetForNewRequests, Database parameterDatabase, DbTransaction parameterTransaction, bool IsIRSOverride=false)// SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim 
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

        //aparna 18/10/2007

        public static int CheckAnnuityExistence(string parameterPersid)
        {
            Database db = null;
            int l_output;
            DbCommand SelectCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                SelectCommandWrapper = db.GetStoredProcCommand("yrs_usp_RR_CheckAnnuityExistence");


                db.AddInParameter(SelectCommandWrapper, "@varchar_PersId", DbType.String, parameterPersid);
                db.AddOutParameter(SelectCommandWrapper, "@OutparameterAnnuity", DbType.Int32, 0);
                db.ExecuteNonQuery(SelectCommandWrapper);
                l_output = (int)(db.GetParameterValue(SelectCommandWrapper, "@OutparameterAnnuity"));
                return l_output;

            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetAnnuityExistence(string parameterPersid, string parameterFundeventId)
        {
            Database l_DataBase = null;
            DbCommand SelectCommandWrapper = null;
            DataSet l_DataSet = null;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                SelectCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetAnnuityExistence");

                if (SelectCommandWrapper == null) return null;
                l_DataBase.AddInParameter(SelectCommandWrapper, "@varchar_PersId", DbType.String, parameterPersid);
                l_DataBase.AddInParameter(SelectCommandWrapper, "@varchar_FundeventId", DbType.String, parameterFundeventId);
                SelectCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                l_DataSet = new DataSet("AnnuityExistence");

                l_DataBase.LoadDataSet(SelectCommandWrapper, l_DataSet, "AnnuityCheck");

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
        public static int GetQDROParticipantAge(string parameterFundeventId)
        {
            Database db = null;
            int l_output;
            DbCommand SelectCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                SelectCommandWrapper = db.GetStoredProcCommand("yrs_usp_RR_GetQDROParticipantAge");


                db.AddInParameter(SelectCommandWrapper, "@varchar_guiFundeventId", DbType.String, parameterFundeventId);
                db.AddOutParameter(SelectCommandWrapper, "@OutputParameterAge", DbType.Int32, 0);
                db.ExecuteNonQuery(SelectCommandWrapper);
                l_output = (int)(db.GetParameterValue(SelectCommandWrapper, "@OutputParameterAge"));
                return l_output;

            }
            catch
            {
                throw;
            }
        }
        public static void ValidateRTBal5k(string parameterPersonID, string parameterFundEventID, out decimal parameterTDAmount, out decimal parameterTMAmount, out decimal parameterRTAmount)
        {
            Database db = null;

            DbCommand l_DbCommand = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                l_DbCommand = db.GetStoredProcCommand("dbo.yrs_usp_RR_Validate_RTBal5K");


                db.AddInParameter(l_DbCommand, "@guiPersId", DbType.String, parameterPersonID);
                db.AddInParameter(l_DbCommand, "@guiFundEventId", DbType.String, parameterFundEventID);
                db.AddOutParameter(l_DbCommand, "@out_intSum_TD", DbType.Decimal, 4);
                db.AddOutParameter(l_DbCommand, "@out_intSum_TM", DbType.Decimal, 4);
                db.AddOutParameter(l_DbCommand, "@out_intSum_OldRT", DbType.Decimal, 4);

                db.ExecuteNonQuery(l_DbCommand);

                parameterTDAmount = Convert.ToDecimal(db.GetParameterValue(l_DbCommand, "out_intSum_TD"));
                parameterTMAmount = Convert.ToDecimal(db.GetParameterValue(l_DbCommand, "@out_intSum_TM"));
                parameterRTAmount = Convert.ToDecimal(db.GetParameterValue(l_DbCommand, "@out_intSum_OldRT"));


            }
            catch
            {
                throw;
            }

        }
        public static DataSet GetWithdrawalReportData(string parameterRefRequestID)
        {
            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("yrs_usp_RR_GetWithdrawalReportData");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefRequestID", DbType.String, parameterRefRequestID);

                // Add the DataSet
                l_DataSet = new DataSet("atsRefundRequest");

                l_stringDataTableNames = new string[] { "atsRefundRequest", "atsRefundRequestDetails" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                return l_DataSet;

            }
            catch
            {
                throw;
            }

        }



        #endregion //Plan Split


        /// <summary>
        /// To get MRD records from Table atsMRDRecords for given FundEventID
        /// </summary>
        /// <param name="parameterFundEventId"></param>
        /// <returns></returns>
        public static DataTable GetMRDRecords(string parameterFundEventId)
        {


            Database l_DataBase = null;
            DataSet l_DataSet = null;
            string[] l_stringDataTableNames;

            DbCommand l_DbCommand = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetAtsMrdRecords");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@FundEventID", DbType.String, parameterFundEventId);

                // Add the DataSet
                l_DataSet = new DataSet("MRDRecords");

                l_stringDataTableNames = new string[] { "MRDRecords" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_stringDataTableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["MRDRecords"]);
                }

                return null;

            }
            catch
            {
                throw;
            }

        }

        //neeraj singh 18-11-2010 BT-664 : 
        /// <summary>
        /// Create -ve entries in AtsMrdRecordsDisbursements table on cancelation of processed refund
        /// </summary>
        /// <param name="parameterPersid"></param>
        /// <returns></returns>
        public static string UpdateMrdRecordsDisbursements(string parameterDisbursementId, Database parameterDatabase, DbTransaction parameterTransaction)
        {

            DbCommand SelectCommandWrapper = null;
            try
            {

                if (parameterDatabase == null) return null;


                SelectCommandWrapper = parameterDatabase.GetStoredProcCommand("yrs_usp_RR_InsertNegativeMrdRecordsDisbursements");
                if (SelectCommandWrapper == null) return null;


                parameterDatabase.AddInParameter(SelectCommandWrapper, "@guiDisbursementID", DbType.String, parameterDisbursementId);
                parameterDatabase.ExecuteNonQuery(SelectCommandWrapper, parameterTransaction);
                return "";

            }
            catch
            {
                throw;
            }
        }


        //------Shashi Shekhar: Added for Update Ref Req tracking No-----------
        public static DataSet GetDisbRefRequest()
        {
            DataSet dsRefReq = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetDisbRefRequest");

                if (LookUpCommandWrapper == null) return null;
                dsRefReq = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsRefReq, "GetDisbRefReq");
                return dsRefReq;
            }
            catch
            {
                throw;
            }

        }




        /// <summary>
        /// To Update the Tracking No
        /// </summary>
        /// <param name="paramRefReqId"></param>
        /// <param name="paramTrackingNo"></param>
        /// <returns>Validation Msg</returns>
        public static string UpdateRefReqTrackingNo(string paramRefReqId, string paramTrackingNo)
        {
            Database db = null;
            DbTransaction l_IDbTransaction = null;
            DbConnection l_IDbConnection = null;
            DbCommand UpdateCommandWrapper = null;
            string l_string_Errormessage = string.Empty;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                l_IDbConnection = db.CreateConnection();
                l_IDbConnection.Open();
                if (l_IDbConnection == null) return null;
                l_IDbTransaction = l_IDbConnection.BeginTransaction();
                UpdateCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_RR_UpdateRefReqTrackingNo");
                if (UpdateCommandWrapper == null) return null;


                db.AddInParameter(UpdateCommandWrapper, "@varchar_RefundRequestID", DbType.String, paramRefReqId);
                db.AddInParameter(UpdateCommandWrapper, "@chvOverNightDelivery", DbType.String, paramTrackingNo); // User ID has to go here.
                db.AddOutParameter(UpdateCommandWrapper, "@cOutput", DbType.String, 1000);
                db.ExecuteNonQuery(UpdateCommandWrapper);

                l_IDbTransaction.Commit();

                l_string_Errormessage = db.GetParameterValue(UpdateCommandWrapper, "@cOutput").ToString();
                return l_string_Errormessage;
            }

            catch (SqlException SqlEx)
            {
                //Shashi Shekhar:2010-04-08
                if (UpdateCommandWrapper != null)
                {
                    l_string_Errormessage = db.GetParameterValue(UpdateCommandWrapper, "@cOutput").ToString();
                }

                l_IDbTransaction.Rollback();
                l_string_Errormessage = l_string_Errormessage + System.Environment.NewLine + SqlEx.Message;
                throw new Exception(l_string_Errormessage, SqlEx);
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
                        l_IDbConnection.Close();
                    }
                    l_IDbConnection = null;
                }
                db = null;
                l_IDbTransaction = null;
            }

        }



        /// <summary>
        /// To get the Refund request cancel reason code from master
        /// </summary>
        /// <returns></returns>
        public static DataSet GetRefCancelReasonCodes()
        {
            DataSet l_dataset_dsGetReasonCodes = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RR_GetRefCancelReasonCodes");

                if (LookUpCommandWrapper == null) return null;
                l_dataset_dsGetReasonCodes = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsGetReasonCodes, "GetReasonCode");
                return l_dataset_dsGetReasonCodes;
            }
            catch
            {
                throw;
            }
        }

        //SR:2013.10.28 : BT-2247/YRS 5.0-2229:Change the TD termination and 6 month resumption date
        /// <summary>
        /// To get TD Locking Period
        /// </summary>
        /// <param name="paramStrRefRequestId"></param>
        /// <returns>Dataset</returns>

        public static DataTable GetTDLockingPeriod(string paramStrRefRequestId)
        {
            Database l_DataBase = null;
            DbCommand l_DbCommand = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return null;

                l_DbCommand = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_GetTDLockingPeriod");

                if (l_DbCommand == null) return null;

                l_DataBase.AddInParameter(l_DbCommand, "@varchar_RefRequestId", DbType.String, paramStrRefRequestId);
                // Connection Timeout
                l_DbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                l_DataSet = new DataSet("LockingPeriod");

                l_TableNames = new string[] { "LockingPeriod" };

                l_DataBase.LoadDataSet(l_DbCommand, l_DataSet, l_TableNames);

                if (l_DataSet != null)
                {
                    return (l_DataSet.Tables["LockingPeriod"]);
                }

                return null;


            }
            catch
            {
                throw;
            }
        }
        //End, SR:2013.10.28 : BT-2247/YRS 5.0-2229:Change the TD termination and 6 month resumption date

        //Start: AA : YRS-AT-2639 Added to get referquest options stored from yrs website
        public static DataTable GetRefRequestOptionsVer2(string paramStrRefRequestId)
        {
            Database db = null;
            DbCommand DbCommand = null;
            DataSet ds = null;           

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                DbCommand = db.GetStoredProcCommand("dbo.yrs_usp_RR_GetRefRequestOptionsVer2");
                if (DbCommand == null) return null;

                db.AddInParameter(DbCommand, "@guiRefRequestId", DbType.String, paramStrRefRequestId);              

                ds = new DataSet("RefRequestOptions");
                

                db.LoadDataSet(DbCommand, ds, "RefRequestOptions");

                if (ds != null)
                {
                    return (ds.Tables["RefRequestOptions"]);
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //End: AA : YRS-AT-2639 Added to get referquest options stored from yrs website    

        //Start- chandrasekar.c:2016.01.11:YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
        public static void UpdateRefundRequestsExtendExpireDate(string paramaeterPersonID, string paramStrRefRequestId)
        {
            Database l_DataBase = null;
            DbTransaction l_IDbTransaction = null;
            DbConnection l_IDbConnection = null;
            DbCommand UpdateCommandWrapper = null;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                l_IDbConnection = l_DataBase.CreateConnection();
                l_IDbConnection.Open();
                if (l_DataBase == null) return;
                l_IDbTransaction = l_IDbConnection.BeginTransaction();
                UpdateCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RR_RefRequestsForExtendExpireDate");

                if (UpdateCommandWrapper == null) return;

                l_DataBase.AddInParameter(UpdateCommandWrapper, "@varchar_RefRequestID", DbType.String, paramStrRefRequestId);
                l_DataBase.AddInParameter(UpdateCommandWrapper, "@varchar_PersID", DbType.String, paramaeterPersonID);
                
               
                // To set the connenction Time Out. 
                UpdateCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                l_DataBase.ExecuteNonQuery(UpdateCommandWrapper);
                l_IDbTransaction.Commit();
                return;
            }
            catch (SqlException SqlEx)
            {
                //Shashi Shekhar:2010-04-08
                l_IDbTransaction.Rollback();
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
                        l_IDbConnection.Close();
                    }
                    l_IDbConnection = null;
                }
                l_DataBase = null;
                l_IDbTransaction = null;
            }
        }
        //End- chandrasekar.c:2016.01.11:YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request


        //Start:AA:02.17.2016 YRS-AT-2640 Added to get the fundeventid for the given input refrequestid
        public static string GetFundEventIDFromRefrequestID(string ParameterRefrequestId)
        {
            string strFundEventID = "";
            Database db = null;
            DbCommand getCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return "-1";
                getCommandWrapper = db.GetStoredProcCommand("yrs_usp_LPage_GetPersonDetails");
                if (getCommandWrapper == null) return "-1";
                db.AddInParameter(getCommandWrapper, "@varRefrequestId", DbType.String, ParameterRefrequestId);
                db.AddOutParameter(getCommandWrapper, "@varOutFundEventID", DbType.String, 50);
                db.ExecuteNonQuery(getCommandWrapper);
                strFundEventID = Convert.ToString(db.GetParameterValue(getCommandWrapper, "@varOutFundEventID"));
                return strFundEventID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //End:AA:02.17.2016 YRS-AT-2640 Added to get the fundeventid for the given input refrequestid
        //START - 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project
        //START - ML| 2019.06.07 |YRS-AT-4461  - YRS bug: HOT FIX NEEDED- portability bug- request-disbursement is for less than amount request (TrackIT38439)
        //public static DataTable GetRefundRequestConfiguration(int intPersonAge, bool bIsPersonTerminate)
        public static DataTable GetRefundRequestConfiguration(int intPersonAge, bool bIsPersonTerminate, bool bitApplyBALegacyCombinedMinAge)
        //END - ML| 2019.06.07 |YRS-AT-4461  - YRS bug: HOT FIX NEEDED- portability bug- request-disbursement is for less than amount request (TrackIT38439)
        {

            Database db = null;
            DbCommand getCommandWrapper = null;
            DataSet ds = null;
            string[] l_TableNames;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                getCommandWrapper = db.GetStoredProcCommand("yrs_usp_Ret_Get_PartialWithdrawalConfigLimits");
                if (getCommandWrapper == null) return null;
                db.AddInParameter(getCommandWrapper, "@intPersonAge", DbType.Int32, intPersonAge);
                db.AddInParameter(getCommandWrapper, "@bitIsPersonTerminate", DbType.Boolean, bIsPersonTerminate);
                //START - ML| 2019.06.07 |YRS-AT-4461 - New parameter to apply either BA_Legacy Combined Min Age Or Default age from atsMetaConfiguration
                db.AddInParameter(getCommandWrapper, "@BIT_ApplyBALegacyCombinedMinAge", DbType.Boolean, bitApplyBALegacyCombinedMinAge);
                //END - ML| 2019.06.07 |YRS-AT-4461 - New parameter to apply either BA_Legacy Combined Min Age Or Default age from atsMetaConfiguration

                ds = new DataSet("Partial Withdrawal Configuration");
                l_TableNames = new string[] { "Partial Withdrawal Configuration" };
                db.LoadDataSet(getCommandWrapper, ds, l_TableNames);
                if (ds != null)
                {
                    return (ds.Tables["Partial Withdrawal Configuration"]);
                }
                return null;
            }
            catch
            {
                throw;
            }
        }
        //END - 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project

        //START: PPP | 11/16/2016 | YRS-AT-3146 - YRS enh: Fees - Partial Withdrawal Processing fee
        // -- yrs_usp_RR_Save_WithdrawalProcessingFee will be called directly from SP if the key "WITHDRAWAL_FEE_PRORATION_SWITCH" value is "ON"
        //// START | SR | 2016.06.06 | YRS-AT-2962 - YRS enh: Get Configured Withdrawals details    
        //// Insert withdrawal Processing Fees
        //private static string InsertWithdrawalProcessingFee(string paramStrRefrequestId, Database parameterDatabase, DbTransaction parameterTransaction)
        //{
        //    DbCommand dbCommand;
        //    try
        //    {
        //        if (parameterDatabase == null) return null;
        //        dbCommand = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_RR_Save_WithdrawalProcessingFee");
        //        if (dbCommand == null) return null;
        //        parameterDatabase.AddInParameter(dbCommand, "@chvRefRequestId", DbType.String, paramStrRefrequestId);
        //        parameterDatabase.ExecuteNonQuery(dbCommand, parameterTransaction);
        //        return "";
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        throw (sqlEx);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}
        //// END | SR | 2016.06.06 | YRS-AT-2962 - YRS enh: Get Configured Withdrawals details        
        //END: PPP | 11/16/2016 | YRS-AT-3146 - YRS enh: Fees - Partial Withdrawal Processing fee
        

        // START : SB | 08/31/2016 | YRS-AT-3028 | To Reterive recalculated RMD values 
        public static DataSet GetRMDForWithdrawalProcessing(string parameterFundEventId)
        {

            Database dbRMDWithdrawal = null;
            DataSet dsRMDWithdrawalDetails = null;
            string[] stringTableNames;

            DbCommand InsertCommandWrapper = null;

            try
            {
                dbRMDWithdrawal = DatabaseFactory.CreateDatabase("YRS");

                if (dbRMDWithdrawal == null) return null;

                InsertCommandWrapper = dbRMDWithdrawal.GetStoredProcCommand("dbo.yrs_usp_RR_Get_RMDRecordsForWithdrawalProcess");

                if (InsertCommandWrapper == null) return null;

                dbRMDWithdrawal.AddInParameter(InsertCommandWrapper, "@UNIQUEIDENTIFIER_FundEventID", DbType.String, parameterFundEventId);

                // Add the DataSet
                dsRMDWithdrawalDetails = new DataSet("MRDRecords");

                stringTableNames = new string[] { "MRDRecords","AtsMRDRecordsUpdation" };

                dbRMDWithdrawal.LoadDataSet(InsertCommandWrapper, dsRMDWithdrawalDetails, stringTableNames);

                if (dsRMDWithdrawalDetails != null)
                {
                    return (dsRMDWithdrawalDetails);
                }

                return null;

            }
            catch
            {
                throw;
            }

    }
        // END : SB | 08/31/2016 | YRS-AT-3028 | To Reterive recalculated RMD values 

        //START: PPP | 11/18/2016 | YRS-AT-3146 | Validating fees against balanaces left after withdrawal, whether they are sufficient enough to deduct fee as per new fee charging hierarchy
        public static bool ValidateWithdrawalProcessingFee(string refundRequestID)
        {
            DbCommand command;
            Database database = null;
            try
            {
                database = DatabaseFactory.CreateDatabase("YRS");
                if (database == null) return false;
                command = database.GetStoredProcCommand("dbo.yrs_usp_RR_ValidateWithdrawalProcessingFee");
                if (command == null) return false;
                database.AddInParameter(command, "@VARCHAR_RefRequestID", DbType.String, refundRequestID);
                database.AddOutParameter(command, "@BIT_IsValid", DbType.Boolean, 1);
                database.ExecuteNonQuery(command);
                return Convert.ToBoolean(database.GetParameterValue(command, "@BIT_IsValid"));
            }
            catch
            {
                throw;
            }
        }
        //END: PPP | 11/18/2016 | YRS-AT-3146 | Validating fees against balanaces left after withdrawal, whether they are sufficient enough to deduct fee as per new fee charging hierarchy

        //START: PPP | 11/27/2017 | YRS-AT-3653 | Checks whether SHIRA request can be canceled or not
        public static bool IsSHIRARequestCancelable(string personID)
        {
            Database database;
            DbCommand command;
            try
            {
                database = DatabaseFactory.CreateDatabase("YRS");
                if (database == null) return false;
                command = database.GetStoredProcCommand("dbo.yrs_usp_RR_IsSHIRARequestCancelable");
                if (command == null) return false;
                database.AddInParameter(command, "@VARCHAR_PersID", DbType.String, personID);
                database.AddOutParameter(command, "@BIT_IsCancelable", DbType.Boolean, 1);
                database.ExecuteNonQuery(command);
                return Convert.ToBoolean(database.GetParameterValue(command, "@BIT_IsCancelable"));
            }
            catch
            {
                throw;
            }
            finally
            {
                command = null;
                database = null;
            }
        }
        //END: PPP | 11/27/2017 | YRS-AT-3653 | Checks whether SHIRA request can be canceled or not

        //START: PK | 11/07/2019 | YRS-AT-2670 | Checks whether participant is from puerto rico ymca 
        public static bool IsYmcaPuertoRico(string fundEventID)
        {
            Database database;
            DbCommand command;
            try
            {
                database = DatabaseFactory.CreateDatabase("YRS");
                if (database == null) return false;
                command = database.GetStoredProcCommand("dbo.yrs_usp_IsYmcaPuertoRico");
                if (command == null) return false;
                database.AddInParameter(command, "@UNIQUEIDENTIFIER_FundEventID", DbType.String, fundEventID);
                database.AddOutParameter(command, "@BIT_IsPuertoRico", DbType.Boolean, 0);
                database.ExecuteNonQuery(command);
                return Convert.ToBoolean(database.GetParameterValue(command, "@BIT_IsPuertoRico"));
            }
            catch
            {
                throw;
            }
            finally
            {
                command = null;
                database = null;
            }
        }
        //END: PK | 11/07/2019 | YRS-AT-2670 | Checks whether participant is from puerto rico ymca

        //START: PK | 29/04/2020 | YRS-AT-4854 | Checks whether refund request is of Covid or not.  
        public static bool IsRequestCovid(string refRequestID)
        {
            Database database;
            DbCommand command;
            try
            {
                database = DatabaseFactory.CreateDatabase("YRS");
                if (database == null) return false;
                command = database.GetStoredProcCommand("dbo.yrs_usp_RR_IsRequestCOVID");
                if (command == null) return false;
                database.AddInParameter(command, "@VARCHAR_RefRequestId", DbType.String, refRequestID);
                database.AddOutParameter(command, "@BIT_IsCovid", DbType.Boolean, 0);
                database.ExecuteNonQuery(command);
                return Convert.ToBoolean(database.GetParameterValue(command, "@BIT_IsCovid"));
            }
            catch
            {
                throw;
            }
            finally
            {
                command = null;
                database = null;
            }
        }
        //END: PK | 29/04/2020 | YRS-AT-4854 | Checks whether refund request is of Covid or not.

    }
}
