//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	LoanInformationDAClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8642
// Creation Time		:	4/10/2006 10:04:52 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	DataAcess class for LoanInformation form
//*******************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified By			Date				Description
//********************************************************************************************************************************
//Ashish Srivastava		08-08-2008			Added new function (GetPersonLatestYmcaId) for issue YRS 5.0-489
//Ashish Srivastava		27-01-2009			Added validation for QDRO Request is pending
//Ashish Srivastava		17-Mar-2009			Generate Reamortization Letter for issue YRS5.0 679
//Nikunj Patel			2009.04.22			Changing code to create a procedure to fetch the Email Id of the TRANSM YMCA Contact for the specified YMCA Id
//Neeraj Singh			22-Oct-2009			changed function InsertDisbursementWithholding to pass DisbusementID as parameter 
//Shagufta              15 July 2011         For YRS 5.0-1320,BT-829:letter when loan is paid off
//Ashish Srivastava     2011.11.07          YRS 5.0-1322:Include after tax money from RT account
//Shashank Patel		2014.01.15			BT-2314\YRS 5.0-2260 - YRS generating blank loan payoff letters 
//Anudeep A             2014.08.26          BT:2625 :YRS 5.0-2405-Consistent screen header sections 
//Anudeep A             2015.03.12          BT:2699:YRS 5.0-2441 : Modifications for 403b Loans 
//DInesh K              2015.04.08          BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Anudeep A             2015.10.26          YRS-AT-2533 - Loan Modifications (Default) Project: two new YRS letters
//Manthan Rajguru       2015.11.04          YRS-AT-2453: Need ability to select other YMCA association for use in Loan reAmortization
//Manthan Rajguru       2016.02.18          YRS-AT-2603: YRS enh: Withdrawals email - add participants name to eliminate 'forwarded' email confusion
//Anudeep A             2016.04.26          YRS-AT-2831 - YRS enh: automate 'Default' of Loans (TrackIT 25242)
//Manthan Rajguru       2018.03.04          YRS-AT-3929 -  YRS REPORTS: EFT: Loans ( FIRST EFT PROJECT) -create new "Loan Acknowledgement" Email for EFT/direct deposit
//Vinayan C             2018.06.21          YRS-AT-3190 -  YRS enh: add warning message for Loan Re-amortization (to prevent duplicate re-amortize efforts
//Vinayan C             2018.07.19          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
//Santosh Bura          2018.07.24          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
//Vinayan C             2018.08.20          YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab 
//Manthan Rajguru       2018.07.23          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
//Manthan Rajguru       2018.10.11          YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//Megha Lad             2019.01.08          YRS-AT-4244-  YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462) 
//Pooja K               2019.01.15          YRS-AT-2573 -  YRS enh: add dropdown for first payment due date for Loan ReAmortization (TrackIT 23592) 
//Pramod P. Pokale      2019.01.22          YRS-AT-2573 -  YRS enh: add dropdown for first payment due date for Loan ReAmortization (TrackIT 23592) 
//Megha Lad             2019.01.17          YRS-AT-3157-  YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)
//Manthan R             2019.01.23          YRS-AT-4118 -  YRS enh-unsuspended loans when reamortized must be extended by the suspension period(Track it 35417)
//Shiny C.              2020.04.10          YRS-AT-4852 -  COVID - YRS changes needed for LOANS due to CARE Act (Track IT - 41693) 
//Shiny C.              2020.04.20          YRS-AT-4853 - COVID - YRS changes for Loan Limit Enhancement, due to CARE Act (Track IT - 41693) 
//********************************************************************************************************************************
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Collections.Generic;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for LoanInformationDAClass.
	/// </summary>
    public class LoanInformationDAClass
    {
        public LoanInformationDAClass()
        {
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// Method to fetch the list of eligible persons to take a TD Loan
		/// </summary>
		/// <param name="parameterSSNo"></param>
		/// <param name="parameterFundNo"></param>
		/// <param name="parameterLastName"></param>
		/// <param name="parameterFirstName"></param>
		/// <returns>DataSet</returns>
        public static DataSet LookUpPerson(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName)
        {
			DataSet dsLookUpPersons = null;
			Database db= null;
			DbCommand commandLookUpPersons = null;

            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				
				commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_FindPerson");
				
				if (commandLookUpPersons==null) return null;

				
				commandLookUpPersons.CommandTimeout = System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["ExtraLargeConnectionTimeOut"] );


				dsLookUpPersons = new DataSet();
				db.AddInParameter(commandLookUpPersons,"@varchar_SSN",DbType.String,parameterSSNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_FundIdNo", DbType.String, parameterFundNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_LName", DbType.String, parameterLastName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FName", DbType.String, parameterFirstName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FormName", DbType.String, "LoanRequestAndProcessing");
			
				db.LoadDataSet(commandLookUpPersons,dsLookUpPersons,"Participants");
				return dsLookUpPersons;
			
			}
            catch
            {
				throw ;
			}

		}
		/// <summary>
		/// Method to fetch the Loan Request Data of a participant
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns>DataSet</returns>
        public static DataSet LookUpLoanRequests(string parameterPersId, string source = "YRS") //VC | 2018.08.20 | YRS-AT-4018 | Added parameter to pass source
        {
			DataSet dsLookUpLoanRequests = null;
			Database db= null;
			DbCommand commandLookUpLoanRequests = null;
			
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				
				commandLookUpLoanRequests = db.GetStoredProcCommand("yrs_usp_LoanGetRequests");
				
				if (commandLookUpLoanRequests==null) return null;
				commandLookUpLoanRequests.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
				dsLookUpLoanRequests = new DataSet();
                db.AddInParameter(commandLookUpLoanRequests, "@varchar_PersId", DbType.String, parameterPersId);
                db.AddInParameter(commandLookUpLoanRequests, "@varchar_Source", DbType.String, source);//VC | 2018.08.20 | YRS-AT-4018 | Added parameter to pass source
				
			
				db.LoadDataSet(commandLookUpLoanRequests,dsLookUpLoanRequests,"LoanRequests");
				return dsLookUpLoanRequests;
			
			}
            catch
            {
				throw ;
			}

		}


        public static void AddRequest(DataSet parameterdsLoanRequest)
        {
			
			Database db= null;
			DbCommand AddRequestCommandWrapper = null;
			DbCommand UpdateRequestCommandWrapper = null;
			DbCommand DeleteRequestCommandWrapper = null;
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return ;
				
				AddRequestCommandWrapper = db.GetStoredProcCommand("yrs_usp_LoanAddRequest");
				
				if (AddRequestCommandWrapper==null) return ;

			
				db.AddInParameter(AddRequestCommandWrapper,"@varchar_PersId",DbType.String,"PersId",DataRowVersion.Current);
				db.AddInParameter(AddRequestCommandWrapper,"@varchar_EmpEventId",DbType.String,"EmpEventId",DataRowVersion.Current);
				db.AddInParameter(AddRequestCommandWrapper,"@varchar_YmcaId",DbType.String,"YmcaId",DataRowVersion.Current);
				db.AddInParameter(AddRequestCommandWrapper,"@numeric_TDBalance",DbType.Double,"TDBalance",DataRowVersion.Current);
				db.AddInParameter(AddRequestCommandWrapper,"@numeric_AmtRequested",DbType.Double,"RequestedAmount",DataRowVersion.Current);
				db.AddInParameter(AddRequestCommandWrapper,"@chv_LoanNumber",DbType.String,"OriginalLoanNumber",DataRowVersion.Current);
			
				db.UpdateDataSet(parameterdsLoanRequest,"LoanRequests",AddRequestCommandWrapper,UpdateRequestCommandWrapper,DeleteRequestCommandWrapper,UpdateBehavior.Standard);
			
			}
            catch
            {
				throw ;
			}

		}
		/// <summary>
		/// To get the loan deductions
		/// </summary>
		/// <returns>DataSet</returns>
        public static DataSet GetDeductions()
        {
			DataSet l_dataset_Deductions = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_LoansGetDeductions");
				
				if (LookUpCommandWrapper == null) return null;
				l_dataset_Deductions = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Deductions,"Deductions");
				//System.AppDomain.CurrentDomain.SetData("dsLookUpGeneralInfo", l_dataset_dsLookUpgeneralInfo);
				return l_dataset_Deductions;
			}
            catch
            {
				throw;
			}
		}

        public static string GetLoanDetailsForLoanOptions(string parameterLoanRequestId)
        {
			
			Database db= null;
			DbCommand commandGetLoanOptions = null;
			string parameterSuspenddate="";
			
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return "";
				
				commandGetLoanOptions = db.GetStoredProcCommand("yrs_usp_LoanGetDetailsForOptions");
				
				if (commandGetLoanOptions==null) return "";
				commandGetLoanOptions.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
				db.AddInParameter(commandGetLoanOptions,"@int_LoanRequestId",DbType.String,parameterLoanRequestId);
				db.AddOutParameter(commandGetLoanOptions,"@dtm_Suspenddate",DbType.String,100);
				db.ExecuteNonQuery(commandGetLoanOptions);
				parameterSuspenddate=db.GetParameterValue(commandGetLoanOptions,"@dtm_Suspenddate").ToString();
				return parameterSuspenddate;
			
			}
            catch
            {
				throw ;
			}

		}
        public static DataSet GetLoanDetailsForCloseLoan(string parameterLoanRequestId, int parameterStatus, string parameterUnsuspenddate, string ymcaId, string firstPaymentDate, out int parametermessageno) //MMR | 2019.01.23 | YRS-AT-4118 | Added parameter for YMCAID (ymcaId) and first payment date (firstPaymentDate)
        {
			DataSet dsCloseLoanDetails = null;
			Database db= null;
			DbCommand commandCloseLoanDetails = null;
			string [] l_TableNames;
			parametermessageno=0;
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandCloseLoanDetails = db.GetStoredProcCommand("yrs_usp_LoanLoadCloseLoanDetails");
				if (commandCloseLoanDetails==null) return null;
				commandCloseLoanDetails.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
				dsCloseLoanDetails = new DataSet();
				db.AddInParameter(commandCloseLoanDetails,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
				db.AddInParameter(commandCloseLoanDetails,"@bit_status",DbType.Int32,parameterStatus);
				db.AddInParameter(commandCloseLoanDetails,"@dtm_unsuspenddate",DbType.String,parameterUnsuspenddate);
                //START : MMR | 2019.01.23 | YRS-AT-4118 | Added parameter for YMCAID and first payment date
                db.AddInParameter(commandCloseLoanDetails, "@VARCHAR_YmcaId", DbType.String, ymcaId);
                db.AddInParameter(commandCloseLoanDetails, "@VARCHAR_FirstPaymentDate", DbType.String, firstPaymentDate);
                //END : MMR | 2019.01.23 | YRS-AT-4118 | Added parameter for YMCAID and first payment date
                db.AddOutParameter(commandCloseLoanDetails, "@int_errormessage", DbType.String, 100); // 'AA:26.08.2014-BT:2625 :changed the datatype of varabile to get message from database
                //START: MMR | 2019.01.23 | YRS-AT-4118 | Commented existing code and removed table as data is being fetched from different source and added another table to get loan final payment date
				//l_TableNames = new string[]{"ReamortizedAmount","ReamortizedMonths","EmployeeActiveYMCA"}; // Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Added table name for storing active YMCA details
                //l_TableNames = new string[] { "ReamortizedAmount", "ReamortizedMonths","LoanFinalPaymentDate"};
				l_TableNames = new string[] { "ReamortizedAmount", "ReamortizedMonths","LoanFinalPaymentDate", "UnfundedLoanPayment" }; // SR | 2019.02.06 | YRS-AT-2920 | Added table name UnfundedLoanPayment to store Loan funded status (to display Error/Warning on screen) 
                //END: MMR | 2019.01.23 | YRS-AT-4118 | Commented existing code and removed table as data is being fetched from different source and added another table to get loan final payment date
                db.LoadDataSet(commandCloseLoanDetails, dsCloseLoanDetails, l_TableNames);
                parametermessageno = Convert.ToInt32(db.GetParameterValue(commandCloseLoanDetails, "@int_errormessage")); // 'AA:26.08.2014-BT:2625 :changed the datatype of varabile to get message from database
				return dsCloseLoanDetails;
			}
            catch
            {
				throw ;
			}

		}


        public static string SaveProcessingData1(string parameterPersId, int parameterLoanRequestId, string parameterFundId, double parameterAmount, DataSet parameterDataSet)
        {
			Database db = null;
			DbCommand SaveDisbursementCommandWrapper = null;
			
			DbTransaction l_DbTransaction = null;
			DbConnection l_DbConnection = null;

			string l_string_UniqueIdentifier;
			string l_string_Message;

            try
            {
				
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				
				l_DbConnection = db.CreateConnection();
				l_DbConnection.Open();
				if (l_DbConnection == null) return null;
				
				l_DbTransaction = l_DbConnection.BeginTransaction();

				SaveDisbursementCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_LoanProcessingWrapper");

				db.AddInParameter(SaveDisbursementCommandWrapper,"@varchar_PersId",DbType.String,parameterPersId );
				db.AddInParameter(SaveDisbursementCommandWrapper,"@varchar_FundId",DbType.String,parameterFundId );
				db.AddInParameter(SaveDisbursementCommandWrapper,"@int_LoanRequestId",DbType.Int16, parameterLoanRequestId);
				db.AddInParameter(SaveDisbursementCommandWrapper,"@numeric_LoanAmount",DbType.String,parameterAmount );
				db.AddOutParameter(SaveDisbursementCommandWrapper,"@varchar_DisbId",DbType.String,50);

				db.ExecuteNonQuery(SaveDisbursementCommandWrapper,l_DbTransaction);
				
				l_string_UniqueIdentifier =	db.GetParameterValue(SaveDisbursementCommandWrapper,"@varchar_DisbId").ToString();
				
				
				foreach(DataRow l_DataRow  in parameterDataSet.Tables["Deductions"].Rows ) 
				{
					// below line commented by NEERAJ on 22-OCT-2009: to pass disbursementID as parameter
					//l_DataRow["UniqueID"] = Convert.ToString(l_string_UniqueIdentifier);
					//InsertDisbursementWithholding(l_DataRow,db,l_DbTransaction);
					InsertDisbursementWithholding(l_DataRow,db,l_DbTransaction,l_string_UniqueIdentifier);
					//Neeraj 22-OCT-2009 : END				
				}

	
				l_DbTransaction.Commit();
				l_string_Message = "The Loan Request has been processed.";
				return l_string_Message;
			}
            catch (SqlException sqlEx)
            {
                if (l_DbTransaction != null)
                {
					l_DbTransaction.Rollback ();
				}
				throw sqlEx;

			}
            catch (Exception ex)
            {
                if (l_DbTransaction != null)
                {
					l_DbTransaction.Rollback ();
				}
				throw ex;
			}
            finally
            {
                if (l_DbConnection != null)
                {
                    if (l_DbConnection.State != ConnectionState.Closed)
                    {
						l_DbConnection.Close ();
					}
				}
			}
		}

        public static string SaveProcessingData(string parameterPersId, int parameterLoanRequestId, string parameterFundId, double parameterAmount, DataSet parameterDataSet, DataTable parameterLoanTransacts, DataTable parameterLoanFeeTransacts, DataTable parameterLoanAccountBreakdown)
        {
			Database db = null;
			DbCommand SaveDisbursementCommandWrapper = null;
			
			DbTransaction l_DbTransaction = null;
			DbConnection l_DbConnection = null;

			string l_string_UniqueIdentifier;
			string l_string_Message;
            bool isONDRequested = false;// VC | 2018.12.03 | YRS-AT-4017 | Declared variable.
			
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				
				l_DbConnection = db.CreateConnection();
				l_DbConnection.Open();
				if (l_DbConnection == null) return null;
				
				l_DbTransaction = l_DbConnection.BeginTransaction();
				
				// firstly records in atsDisbursements will be created 
				SaveDisbursementCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_LoanInsertDisbursements");

				//Priya 11-june-2008 to set command timeout period
				if(SaveDisbursementCommandWrapper == null) return null;
				SaveDisbursementCommandWrapper.CommandTimeout = System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"] );
				//Priya 11-june-2008s

				db.AddInParameter(SaveDisbursementCommandWrapper,"@varchar_PersId",DbType.String,parameterPersId );
				//db.AddInParameter(SaveDisbursementCommandWrapper,"@varchar_FundId",DbType.String,parameterFundId );
				db.AddInParameter(SaveDisbursementCommandWrapper,"@int_LoanRequestId",DbType.Int16, parameterLoanRequestId);
				db.AddInParameter(SaveDisbursementCommandWrapper,"@numeric_LoanAmount",DbType.String,parameterAmount );
				db.AddOutParameter(SaveDisbursementCommandWrapper,"@varchar_UniqueId",DbType.String,50);

				db.ExecuteNonQuery(SaveDisbursementCommandWrapper,l_DbTransaction);
				
				l_string_UniqueIdentifier =	db.GetParameterValue(SaveDisbursementCommandWrapper,"@varchar_UniqueId").ToString();
				
				
				foreach(DataRow l_DataRow  in parameterDataSet.Tables["Deductions"].Rows ) 
				{
                    //START: VC | 2018.12.03 | YRS-AT-4017 | If code value is OND then set isONDRequested variable as true.
                    if (l_DataRow["CodeValue"].ToString().Trim().ToUpper() == "OND")
                    {
                        isONDRequested = true;
                    }
                    //END: VC | 2018.12.03 | YRS-AT-4017 | If code value is OND then set isONDRequested variable as true.
					// below line commented by NEERAJ on 22-OCT-2009: to pass disbursementID as parameter
					//l_DataRow["UniqueID"] = Convert.ToString(l_string_UniqueIdentifier);
					//InsertDisbursementWithholding(l_DataRow,db,l_DbTransaction);
					InsertDisbursementWithholding(l_DataRow,db,l_DbTransaction,l_string_UniqueIdentifier);
					//Neeraj 22-OCT-2009 : END
				}
				//then records in atsTransacts will be created 
                foreach (DataRow l_DataRow in parameterLoanFeeTransacts.Rows)
                {
					InsertFeeTransacts(parameterLoanRequestId,l_DataRow,db,l_DbTransaction);
				}
                foreach (DataRow l_DataRow in parameterLoanTransacts.Rows)
                {
					InsertTransacts(parameterLoanRequestId,l_DataRow,db,l_DbTransaction);
				}
                foreach (DataRow l_DataRow in parameterLoanAccountBreakdown.Rows)
                {
                    InsertAccountBreakdown(parameterLoanRequestId, l_DataRow, db, l_DbTransaction, isONDRequested); // VC | 2018.12.03 | YRS-AT-4017 | Passing new variable isONDRequested.
				}
				//Priya 10-June-2008 call GenerateLoanAmortizationTransacts()
				GenerateLoanAmortizationTransacts(parameterLoanRequestId,db,l_DbTransaction);
				//Priya 10-June-2008
				l_DbTransaction.Commit();
				l_string_Message = "The Loan Request has been processed.";
				return l_string_Message;
				
			}
            catch (SqlException sqlEx)
            {
                if (l_DbTransaction != null)
                {
					l_DbTransaction.Rollback ();
				}
				throw sqlEx;

			}
            catch (Exception ex)
            {
                if (l_DbTransaction != null)
                {
					l_DbTransaction.Rollback ();
				}
				throw ex;
			}
            finally
            {
                if (l_DbConnection != null)
                {
                    if (l_DbConnection.State != ConnectionState.Closed)
                    {
						l_DbConnection.Close ();
					}
				}
			}
		}


		// commented by neeraj 22-OCT-2009: to pass DisbusementID as parameter 
		//public static void InsertDisbursementWithholding(DataRow parameterDataRow, Database parameterDatabase,  DbTransaction parameterTransaction) {
        public static void InsertDisbursementWithholding(DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction, String paramDisbursementID)
        {
			DbCommand l_DbCommand;
            try
            {
				if (parameterDatabase == null) return ; 

				if (parameterDataRow == null) return ;

				l_DbCommand = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_LoanInsertDisbursementWithholding");

				if (l_DbCommand == null) return ;


                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_DisbId", DbType.String, Convert.ToString(paramDisbursementID));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_WithholdingTypeCode", DbType.String, Convert.ToString(parameterDataRow["CodeValue"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_Amount", DbType.Double, Convert.ToDouble(parameterDataRow["Amount"]));

				
				
				parameterDatabase.ExecuteNonQuery (l_DbCommand, parameterTransaction);	
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

        public static void InsertDisbursementsEarlyLoanPayOff(string parameterPersId, int parameterLoanRequestId)
        {
			Database l_DataBase = null;
			DbCommand l_DbCommand=null;
            try
            {
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");
				if (l_DataBase == null) return; 
		
				l_DbCommand = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_Loan_InsertDisbursementsEarlyLoanPayOff");
				if (l_DbCommand == null) return ;

                l_DataBase.AddInParameter(l_DbCommand,"@varchar_PersId", DbType.String, parameterPersId);
                l_DataBase.AddInParameter(l_DbCommand, "@int_LoanRequestId", DbType.Int32, parameterLoanRequestId);

				l_DataBase.ExecuteNonQuery(l_DbCommand);	
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

        public static void CancelPendingLoanRequest(int parameterLoanRequestId)
        {
			DbCommand l_DbCommand;
			Database db = null;
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return;
				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_LoanCancelPendingRequest");

				if (l_DbCommand == null) return ;
				

				db.AddInParameter(l_DbCommand,"@int_LoanId",DbType.Int16,parameterLoanRequestId);
				
				
				
				db.ExecuteNonQuery (l_DbCommand);	
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
        //START: MMR | 2018.10.11 | YRS-AT-3101 | Commneted existing code to cancel loan with DISB request and added to cancel loan with PEND/DISB Request
        //public static string CancelDisbLoanRequest(int parameterLoanRequestId)
        //{
        //    DbCommand l_DbCommand;
        //    Database db = null;
        //    DbTransaction l_DbTransaction = null;
        //    DbConnection l_DbConnection=null;
        //    string l_string_errormessage=" ";
        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");
        //        if (db == null) return "";

				
        //        l_DbConnection = db.CreateConnection();
        //        l_DbConnection.Open();
        //        if (l_DbConnection == null) return null;

        //        l_DbTransaction = l_DbConnection.BeginTransaction();
        //        l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_LoanCancelDisbRequest");
        //        l_DbCommand.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
        //        if (l_DbCommand == null) return "" ;
        //        db.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int16,parameterLoanRequestId);
        //        db.AddOutParameter(l_DbCommand,"@char_errormessage",DbType.String,100);
        //        db.ExecuteNonQuery (l_DbCommand,l_DbTransaction);
        //        l_string_errormessage = Convert.ToString(db.GetParameterValue(l_DbCommand,"@char_errormessage"));
        //        if (l_string_errormessage != "")
        //        {
        //            l_DbTransaction.Rollback();
        //        }
        //        else
        //        {
        //            l_DbTransaction.Commit();
        //        }
					
        //        return l_string_errormessage;	
        //    }
        //    catch (SqlException SqlEx)
        //    {
        //        if (l_DbTransaction != null)
        //        {
        //            l_DbTransaction.Rollback ();
        //        }
        //        throw SqlEx;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (l_DbTransaction != null)
        //        {
        //            l_DbTransaction.Rollback ();
        //        }
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (l_DbConnection != null)
        //        {
        //            if (l_DbConnection.State != ConnectionState.Closed)
        //            {
        //                l_DbConnection.Close ();
        //            }
        //        }
        //    }
        //}

        public static string CancelLoanRequest(int loanRequestId)
        {
            DbCommand dbCommand;
			Database db = null;
            DbTransaction dbTransaction = null;
            DbConnection dbConnection = null;
            string errorMessage = " ";
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return "";
                dbConnection = db.CreateConnection();
                dbConnection.Open();
                if (dbConnection == null) return null;

                dbTransaction = dbConnection.BeginTransaction();
                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_Loan_CancelRequest");
                dbCommand.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (dbCommand == null) return "";
                db.AddInParameter(dbCommand, "@INT_LoanRequestId", DbType.Int16, loanRequestId);
                db.AddOutParameter(dbCommand, "@VARCHAR_ErrorMessage", DbType.String, 100);
                db.ExecuteNonQuery(dbCommand, dbTransaction);
                errorMessage = Convert.ToString(db.GetParameterValue(dbCommand, "@VARCHAR_ErrorMessage"));
                if (errorMessage != "")
                {
                    dbTransaction.Rollback();
				}
                else
                {
                    dbTransaction.Commit();
				}
					
                return errorMessage;
			}
            catch (SqlException SqlEx)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
				}
				throw SqlEx;
			}
            catch (Exception ex)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
				}
				throw ex;
			}
            finally
            {
                if (dbConnection != null)
                {
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
					}
				}
			}
		}
        //END: MMR | 2018.10.11 | YRS-AT-3101 | Commneted existing code to cancel loan with DISB request and added to cancel loan with PEND/DISB Request
        // Start | SC | 2020.04.10 | YRS-AT-4852 | Added reason code for loan suspend
        //public static void CancelLoanUpdateAmortization(string parameterLoanStatus, string parameterRequestField, int parameterLoanRequestId, string parameterSuspendDate, string parameterUnSuspendDate, string parameterDefaultDate, DataSet parameterCreateNewRequest, double parameterPayOffAmount, int intOffseteventReason)
        public static void CancelLoanUpdateAmortization(string parameterLoanStatus, string parameterRequestField, int parameterLoanRequestId, string parameterSuspendDate, string parameterUnSuspendDate, string parameterDefaultDate, DataSet parameterCreateNewRequest, double parameterPayOffAmount, int intOffseteventReason, string parameterSuspendReasonCode)
        // End | SC | 2020.04.10 | YRS-AT-4852 | Added reason code for loan suspend
        {
			DbCommand l_InsertCommandWrapper;
			DbCommand l_DbCommand;
			DbCommand UpdateRequestCommandWrapper = null;
			DbCommand DeleteRequestCommandWrapper = null;
			Database db = null;
			DbTransaction l_DbTransaction = null;
			DbConnection l_DbConnection=null;
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return;

				l_DbConnection = db.CreateConnection();
				l_DbConnection.Open();
				if (l_DbConnection == null) return ;

				l_DbTransaction = l_DbConnection.BeginTransaction();
                if (parameterLoanStatus == "DEFALT")
                {
					l_InsertCommandWrapper=db.GetStoredProcCommand ("dbo.yrs_usp_Loan_InsertDisbursementsEarlyLoanPayOff");
					l_InsertCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
					if (l_InsertCommandWrapper == null) return ;
                    db.AddInParameter(l_InsertCommandWrapper, "@int_LoanRequestId", DbType.Int32, parameterLoanRequestId);
                    db.AddInParameter(l_InsertCommandWrapper, "@chv_DefaultDate", DbType.String, parameterDefaultDate);
                    db.AddInParameter(l_InsertCommandWrapper, "@chvLoanStatus", DbType.String, parameterLoanStatus); //AA:15.04.2015 BT:2699:YRS 5.0-2441 : Added to check with loan status                    
                    db.ExecuteNonQuery(l_InsertCommandWrapper, l_DbTransaction);
				}
				//YREN 3014 Jan 12th 2006 Shubhrata 
                if (parameterLoanStatus == "UNSPND")
                {
					SavePayOffDetails(parameterLoanRequestId,parameterPayOffAmount,db,l_DbTransaction);
				}
                if (parameterLoanStatus == "TERM")
                {
					l_InsertCommandWrapper=db.GetStoredProcCommand ("dbo.yrs_usp_LoanAddRequest");
					l_InsertCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
					if (l_InsertCommandWrapper == null) return ;
					db.AddInParameter(l_InsertCommandWrapper,"@varchar_PersId",DbType.Guid,"guiPersid",DataRowVersion.Current);
                    db.AddInParameter(l_InsertCommandWrapper, "@varchar_EmpEventId", DbType.Guid, "guiEmpEventId", DataRowVersion.Current);
                    db.AddInParameter(l_InsertCommandWrapper, "@varchar_YmcaId", DbType.Guid, "guiYmcaid", DataRowVersion.Current);
					db.AddInParameter(l_InsertCommandWrapper,"@numeric_TDBalance",DbType.Double,"mnyTDBalance",DataRowVersion.Current);
					db.AddInParameter(l_InsertCommandWrapper,"@numeric_AmtRequested",DbType.Double,"mnyAmtRequested",DataRowVersion.Current);
					db.AddInParameter(l_InsertCommandWrapper,"@chv_LoanNumber",DbType.Int32,"chvOrigLoanNumber",DataRowVersion.Current);
                    db.AddInParameter(l_InsertCommandWrapper, "@SMALLDATETIME_LoanFirstPaymentDate", DbType.DateTime, "dtmLoanFirstPaymentDate", DataRowVersion.Current); //PK | 01.15.2019 | YRS-AT-2573 | added to fill first payment date
                    db.AddInParameter(l_InsertCommandWrapper, "@SMALLDATETIME_LoanFinalPaymentDate", DbType.DateTime, "dtmLoanFinalPaymentDate", DataRowVersion.Current); //MMR | 2019.01.23 | YRS-AT-4118 | added to fill final payment date
                    db.UpdateDataSet(parameterCreateNewRequest, "LoanNewRequests", l_InsertCommandWrapper, UpdateRequestCommandWrapper, DeleteRequestCommandWrapper, UpdateBehavior.Standard);
				}
				//YREN 3014 Jan 12th 2006 Shubhrata 
                //Start:AA:15.04.2015 BT:2699:YRS 5.0-2441 : Added to save offset details
                if (parameterLoanStatus == "OFFSET")
                {
                    l_InsertCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Loan_InsertDisbursementsEarlyLoanPayOff");
                    l_InsertCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                    if (l_InsertCommandWrapper == null) return;
                    db.AddInParameter(l_InsertCommandWrapper, "@int_LoanRequestId", DbType.Int32, parameterLoanRequestId);
                    db.AddInParameter(l_InsertCommandWrapper, "@chv_DefaultDate", DbType.String, parameterDefaultDate);
                    db.AddInParameter(l_InsertCommandWrapper, "@chvLoanStatus", DbType.String, parameterLoanStatus);
                    db.AddInParameter(l_InsertCommandWrapper, "@intOffseteventReason", DbType.String, intOffseteventReason); //AA:15.04.2015 BT:2699:YRS 5.0-2441 : Added to check with loan status
                    db.ExecuteNonQuery(l_InsertCommandWrapper, l_DbTransaction);
                }
                //End:AA:15.04.2015 BT:2699:YRS 5.0-2441 : Added to save offset details



				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_LoanUpdateAmortization");
				
				l_DbCommand.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DbCommand == null) return ;
				
				db.AddInParameter(l_DbCommand,"@chv_RequestStatus",DbType.String,parameterLoanStatus);
				db.AddInParameter(l_DbCommand,"@chv_RequestField",DbType.String,parameterRequestField);
				db.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int16,parameterLoanRequestId);
				db.AddInParameter(l_DbCommand,"@dtm_SuspendDate",DbType.String,parameterSuspendDate);
				db.AddInParameter(l_DbCommand,"@dtm_UnSuspendDate",DbType.String,parameterUnSuspendDate);
                db.AddInParameter(l_DbCommand, "@chvSuspendReasonCode", DbType.String, parameterSuspendReasonCode);// SC | 2020.04.10 | YRS-AT-4852 | Added reason code for update
				db.ExecuteNonQuery (l_DbCommand,l_DbTransaction);	
				l_DbTransaction.Commit();

			}
            catch (SqlException SqlEx)
            {
                if (l_DbTransaction != null)
                {
					l_DbTransaction.Rollback ();
				}
				throw SqlEx;
			}
            catch (Exception ex)
            {
                if (l_DbTransaction != null)
                {
					l_DbTransaction.Rollback ();
				}
				throw ex;
			}
            finally
            {
                if (l_DbConnection != null)
                {
                    if (l_DbConnection.State != ConnectionState.Closed)
                    {
						l_DbConnection.Close ();
					}
				}
			}
		}
        public static int IsLoanPayOffValid(int parameterLoanRequestId)
        {
			DbCommand l_DbCommand;
			Database db = null;
			int l_int_count = 0;
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_count;
				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_Loan_IsPayOffValid");
				
				l_DbCommand.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DbCommand == null) return l_int_count;
				

				db.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
				db.AddOutParameter(l_DbCommand,"@int_count",DbType.Int32,2);
				db.ExecuteNonQuery(l_DbCommand);
				l_int_count = Convert.ToInt32(db.GetParameterValue(l_DbCommand,"@int_count"));
				return l_int_count;
			}
			
            catch (Exception ex)
            {
				throw ex;
			}
		}
        public static int IsLoanUnpaidFromCureperiod(int parameterLoanRequestId, out string parameterMessage)
        {
			DbCommand l_DbCommand;
			Database db = null;
			int l_int_count = 0;
			parameterMessage="";
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_count;
                l_DbCommand = db.GetStoredProcCommand("dbo.yrs_usp_Loan_IsLoanUnpaidFromCureperiod");
				
				l_DbCommand.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DbCommand == null) return l_int_count;
				

				db.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
				db.AddOutParameter(l_DbCommand,"@bit_validflag",DbType.Int32,2);
				db.AddOutParameter(l_DbCommand,"@chv_errormessage",DbType.String,100);
				db.ExecuteNonQuery(l_DbCommand);
				l_int_count = Convert.ToInt32(db.GetParameterValue(l_DbCommand,"@bit_validflag"));
				parameterMessage=Convert.ToString(db.GetParameterValue(l_DbCommand,"@chv_errormessage"));
				return l_int_count;
				
			}
			
            catch (Exception ex)
            {
				throw ex;
			}
		}
        public static int IsActiveMultipleYMCA(int parameterLoanRequestId)
        {
			DbCommand l_DbCommand;
			Database db = null;
			int l_int_count = 0;
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_count;
				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_Loan_IsActiveMultipleYMCA");
				
				l_DbCommand.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DbCommand == null) return l_int_count;
				

				db.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
				//db.AddInParameter(l_DbCommand,"@varchar_fundeventid",DbType.String,parameterFundEventId);
				db.AddOutParameter(l_DbCommand,"@int_count",DbType.Int32,2);
				db.ExecuteNonQuery(l_DbCommand);
				l_int_count = Convert.ToInt32(db.GetParameterValue(l_DbCommand,"@int_count"));
				return l_int_count;
				
			}
			
            catch (Exception ex)
            {
				throw ex;
			}
		}
        public static int IsLoanCancelValid(int parameterLoanRequestId, string parameterLoanStatus)
        {
			DbCommand l_DbCommand;
			Database db = null;
			int l_int_count = 0;
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_count;
				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_Loan_IsValidCancel");
				
				l_DbCommand.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DbCommand == null) return l_int_count;
				

				db.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
				db.AddInParameter(l_DbCommand,"@chv_RequestStatus",DbType.String,parameterLoanStatus);
				db.AddOutParameter(l_DbCommand,"@bit_validvalue",DbType.Int32,2);
				db.ExecuteNonQuery(l_DbCommand);
				l_int_count = Convert.ToInt32(db.GetParameterValue(l_DbCommand,"@bit_validvalue"));
				return l_int_count;
				
			}
			
            catch (Exception ex)
            {
				throw ex;
			}
		}
        public static int IsValidSuspension(string parameterLoanRequestId)
        {
			Database db=null;
			int parameterStatus=0;
			DbCommand GetCommandWrapper=null;
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
				GetCommandWrapper = db.GetStoredProcCommand("yrs_usp_Loan_IsSuspendValid");
				db.AddInParameter(GetCommandWrapper,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
				db.AddOutParameter(GetCommandWrapper,"@int_status",DbType.Int32,2);
				if (GetCommandWrapper == null) return 0;
				db.ExecuteScalar(GetCommandWrapper);
				parameterStatus=Convert.ToInt32(db.GetParameterValue(GetCommandWrapper,"@int_status"));
                return parameterStatus;
						
			}
            catch
            {
				throw;
			}
			
		}
        // Start | SC | 2020.04.10 | YRS-AT-4852 | Added method to Populate dropdown list with reasons of loan suspension
        public static DataSet GetLoanSuspendReasons(string parameterLoanRequestId)
        {
            DataSet dsLoanSuspendReasons = null;
            Database db = null;
            DbCommand GetCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                GetCommandWrapper = db.GetStoredProcCommand("yrs_usp_Loan_SuspendReasons");
                if (GetCommandWrapper == null) return null;
                db.AddInParameter(GetCommandWrapper, "@int_LoanRequestId", DbType.String, parameterLoanRequestId);
                dsLoanSuspendReasons = new DataSet();
                dsLoanSuspendReasons.Locale = System.Globalization.CultureInfo.InvariantCulture;
                db.LoadDataSet(GetCommandWrapper, dsLoanSuspendReasons, "LoanSuspendReasons");
                return dsLoanSuspendReasons;
            }
            catch
            {
                throw;
            }
        }
        //End | SC | 2020.04.10 | YRS-AT-4852 | Added method to Populate dropdown list with reasons of loan suspension
                        
        //Start: AA:04.26.2016 YRS-AT-2831 removed variable as the output parameter is not necesary
        //public static string GetDefaultDate(int parameterLoanRequestId, out int int_fundresult)
        public static string GetDefaultDate(int parameterLoanRequestId)
        //End: AA:04.26.2016 YRS-AT-2831 removed variable as the output parameter is not necesary
        {
			Database db = null;
			string parameterDefaultDate = "";
            //int_fundresult=0; //AA:04.26.2016 YRS-AT-2831 Commented becasue as it is not neccesary
			DbCommand GetCommandWrapper=null;
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
				GetCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_LoanGetSelectedDefaultDate");
				db.AddInParameter(GetCommandWrapper,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
                db.AddOutParameter(GetCommandWrapper, "@chv_Defaultdate", DbType.String, 100);
				//Added By Ashutosh Patil as on 07-Feb-2007
				//YREN-3034 Point-1
                //db.AddOutParameter(GetCommandWrapper, "@bit_Loanfundresult", DbType.Int32, 2); //AA:04.26.2016 YRS-AT-2831 Commented becasue as it is not neccesary
				if (GetCommandWrapper == null) return "";
				db.ExecuteNonQuery(GetCommandWrapper);
				parameterDefaultDate = db.GetParameterValue(GetCommandWrapper,"@chv_Defaultdate").ToString();
                //int_fundresult=Convert.ToInt16(db.GetParameterValue(GetCommandWrapper,"@bit_Loanfundresult"));//AA:04.26.2016 YRS-AT-2831 Commented becasue as it is not neccesary
				return parameterDefaultDate;
				
						
			}
            catch
            {
				throw;
			}
			
		}

        //Start:AA:15.04.2015 BT:2699:YRS 5.0-2441 : modified to get pricnipal amount and cure period interest separetly
        public static DataSet GetDefaultAmount(int parameterLoanRequestId, string paramaterDefaultDate)
        {
			Database db = null;
            double[] paramaterDefaultAmount = new double[2];
            //double dblPrincipalAmt = 0.00;
            //double dblCurePeriodIntAmt = 0.00;            
			DbCommand GetCommandWrapper=null;
            DataSet dsDefaultAmount = new DataSet();
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
				GetCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Loan_GetDefaultAmount");
				db.AddInParameter(GetCommandWrapper,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
				db.AddInParameter(GetCommandWrapper,"@chv_Defaultdate",DbType.String,paramaterDefaultDate);
                //ASHISH:2011.11.15 YRS 5.0-1322
				//db.AddOutParameter(GetCommandWrapper,"@numeric_DefaultAmount",DbType.Double,100);
                //db.AddOutParameter(GetCommandWrapper, "@numeric_PrincipalAmount", DbType.Double, 100);
                //db.AddOutParameter(GetCommandWrapper, "@numeric_CurePeriodIntAmount", DbType.Double, 100);                
				if (GetCommandWrapper == null) return null;
				//db.ExecuteNonQuery(GetCommandWrapper);
                db.LoadDataSet(GetCommandWrapper, dsDefaultAmount, "DefaultAmount");
                //ASHISH:2011.11.15 YRS 5.0-1322
				//paramaterDefaultAmount = Convert.ToDouble(db.GetParameterValue(GetCommandWrapper,"@numeric_DefaultAmount"));
                //dblPrincipalAmt = Convert.ToDouble(db.GetParameterValue(GetCommandWrapper, "@numeric_PrincipalAmount"));
                //dblCurePeriodIntAmt = Convert.ToDouble(db.GetParameterValue(GetCommandWrapper, "@numeric_CurePeriodIntAmount"));                
                //paramaterDefaultAmount[0] = dblPrincipalAmt;
                //paramaterDefaultAmount[1] = dblCurePeriodIntAmt;
                return dsDefaultAmount;
						
			}
            catch
            {
				throw;
			}
			
		}
        //End:AA:15.04.2015 BT:2699:YRS 5.0-2441 : modified to get pricnipal amount and cure period interest separetly
        //START : SC | 2020.04.20 | YRS-AT-4853 | Get the maximum loan amount based on participant if applied for covid loan or regular loan
        //public static string GetLoanMaximumAmount()
        public static string GetLoanMaximumAmount(int parameterFundID)
        {
			DbCommand SelectCommandWrapper;
			Database db = null;
			string l_string_MaximumAmount;
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				SelectCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_LoanGetMaximumAmount");
                db.AddInParameter(SelectCommandWrapper, "@intFundID", DbType.Int32, parameterFundID); // SC | 2020.04.20 | YRS-AT-4853 | Addede Fund Id as input parameter
				db.AddOutParameter(SelectCommandWrapper,"@chvValue",DbType.String,50);

				db.ExecuteNonQuery(SelectCommandWrapper);
				
				l_string_MaximumAmount =db.GetParameterValue(SelectCommandWrapper,"@chvValue").ToString();
				return l_string_MaximumAmount;
			}
            catch (Exception ex)
            {
				throw ex;
			}
				
		}
        //END : SC | 2020.04.20 | YRS-AT-4853 | Get the maximum loan amount based on participant if applied for covid loan or regular loan
        public static int CheckForLoanId(int parameterLoanRequestId)
        {
			DbCommand l_DbCommand;
			Database db = null;
			int l_int_count = 0;
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_count;
				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_CheckForLoanDetails");
				
				l_DbCommand.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DbCommand == null) return l_int_count;
				

				db.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int16,parameterLoanRequestId);
				db.AddOutParameter(l_DbCommand,"@int_count",DbType.Int32,2);
				db.ExecuteNonQuery(l_DbCommand);
				l_int_count = Convert.ToInt32(db.GetParameterValue(l_DbCommand,"@int_count"));
				return l_int_count;
				
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


        public static DataSet GetYMCAId(string parameterPersId)
        {
			DataSet l_dataset_YmcaId = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Loan_SelectYMCAId");
				
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@varchar_PersId",DbType.String,parameterPersId);
				l_dataset_YmcaId = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_YmcaId,"YMcaId");
				
				return l_dataset_YmcaId;
			}
            catch
            {
				throw;
			}
		}
        public static DataSet GetRequestSchema()
        {
			DataSet l_dataset_GetRequestSchema = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_LoanGetRequestSchema");
				
				if (LookUpCommandWrapper == null) return null;
				
				l_dataset_GetRequestSchema = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_GetRequestSchema,"RequestSchema");
				
				return l_dataset_GetRequestSchema;
			}
            catch
            {
				throw;
			}
		}
        public static void SavePayOffDetails(int parameterLoanRequestId, double parameterPayOffAmt)
        {
			Database db = null;
			DbCommand UpdateCommandWrapper = null;
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return ;

				UpdateCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_LoanSavePayOffDetails");
				if (UpdateCommandWrapper == null) return ;
				db.AddInParameter(UpdateCommandWrapper,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
				db.AddInParameter(UpdateCommandWrapper,"@mny_PayOffAmt",DbType.Double,parameterPayOffAmt);
				
				db.ExecuteNonQuery(UpdateCommandWrapper);

			}
            catch
            {
				throw;
			}
		}
        public static void LoanProcessingAfterReamortization(int parameterLoanRequestId)
        {
			Database db = null;
			DbCommand UpdateCommandWrapper = null;
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return ;

				UpdateCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_LoanProcessingAfterReamortization");
				if (UpdateCommandWrapper == null) return ;
				db.AddInParameter(UpdateCommandWrapper,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
								
				db.ExecuteNonQuery(UpdateCommandWrapper);

			}
            catch
            {
				throw;
			}
		}
        public static void SavePayOffDetails(int parameterLoanRequestId, double parameterPayOffAmt, Database parameterDatabase, DbTransaction parameterTransaction)
        {
			//			Database db = null;
			DbCommand UpdateCommandWrapper = null;
            try
            {
				if (parameterDatabase == null) return ; 
			
				UpdateCommandWrapper = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_LoanSavePayOffDetails");
				if (UpdateCommandWrapper == null) return ;
                parameterDatabase.AddInParameter(UpdateCommandWrapper, "@int_LoanRequestId", DbType.Int32, parameterLoanRequestId);
                parameterDatabase.AddInParameter(UpdateCommandWrapper,"@mny_PayOffAmt", DbType.Double, parameterPayOffAmt);
				
				parameterDatabase.ExecuteNonQuery(UpdateCommandWrapper,parameterTransaction);

			}
            catch
            {
				throw;
			}
		}
        public static DataSet GetYMCAInformation(string parameterYMCAID)
        {
			DataSet l_dataset_YmcaInfo = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Loan_SelectYMCAInfo");
				
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@varchar_YMCAId",DbType.String,parameterYMCAID);
				l_dataset_YmcaInfo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_YmcaInfo,"YMCAInformation");
				
				return l_dataset_YmcaInfo;
			}
            catch
            {
				throw;
			}
		}
		
        //shagufta chaudhari-15-07-2011 BT-829
		//SP 2014.01.15 BT-2314\YRS 5.0-2260 - YRS generating blank loan payoff letters 
		//Added New Parameter parameterGuiYMCAId
        public static int IsActiveYMCAEmployement(string parameterguiPersId, string parameterGuiYMCAId )
        {
            DbCommand l_DbCommand;
            Database db = null;
            int l_int_count = 0;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return l_int_count;
                l_DbCommand = db.GetStoredProcCommand("dbo.yrs_usp_Loan_IsActiveYMCAEmployement");

                l_DbCommand.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DbCommand == null) return l_int_count;


                db.AddInParameter(l_DbCommand, "@guiPersId", DbType.String, parameterguiPersId);
				db.AddInParameter(l_DbCommand, "@guiYmcaID", DbType.String, parameterGuiYMCAId); //SP 2014.01.15 BT-2314\YRS 5.0-2260 - YRS generating blank loan payoff letters 
                //db.AddInParameter(l_DbCommand,"@varchar_fundeventid",DbType.String,parameterFundEventId);
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

        // START : VC | 2018.06.21 | YRS-AT-3190 | To check whether is loan is recently reamortized or not
        public static bool IsLoanReamortizedEarlier(int loanRequestId)
        {
            DbCommand cmd = null;
            Database db = null;
            bool isLoanReamortizedRecently;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) throw new Exception("Database object is null");
                cmd = db.GetStoredProcCommand("yrs_usp_Loan_IsLoanReamortizedEarlier");
                if (cmd == null) throw new Exception("Database object is null");
                db.AddInParameter(cmd, "@INT_LoanRequestID", DbType.Int32, loanRequestId);
                db.AddOutParameter(cmd, "@BIT_IsLoanReamortizedRecently", DbType.Boolean, 1);
                db.ExecuteNonQuery(cmd);

                isLoanReamortizedRecently = Boolean.Parse(db.GetParameterValue(cmd, "@BIT_IsLoanReamortizedRecently").ToString());
                return isLoanReamortizedRecently;
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
        // END : VC | 2018.06.21 | YRS-AT-3190 | To check whether is loan is recently reamortized or not

        //START : SB | 07/25/2018 | YRS-AT-4017 | Loan Admin Console - Get and Save prime interest rate, get loan statistics
        /// <summary>
        /// Updates prime rate from old interest to new interest rate for loan  in loan admin 
        /// </summary>
        /// <param name="paramrate">New Interest rate</param>
        public static void SavePrimeRate(string primeRate)
        {
            Database db;
            DbCommand cmd;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_LAC_SavePrimeRate");
                if (cmd == null) return;

                db.AddInParameter(cmd, "@VARCHAR_Rate", DbType.String, primeRate);
                db.ExecuteNonQuery(cmd);
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

        /// <summary>
        /// Provides prime interest rate of loan
        /// </summary>
        /// <returns>Current prime interest rate</returns>
        public static DataSet GetPrimeRate()
        {
            DbCommand cmd;
            Database db = null;
            DataSet rate = null;
            string[] tableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_LAC_GetPrimeRateAndHistory");
                if (cmd == null) return null;

                rate = new DataSet();
                tableNames = new string[] { "PrimeRate", "PrimeRateHist" };
                db.LoadDataSet(cmd, rate, tableNames);
                return rate;
            }
            catch
            {
                throw;
            }
            finally 
            {
                cmd = null;
                db = null;
                rate = null;
                tableNames=null;
            }
        }
        
        /// <summary>
        /// Provides all loan statistics 
        /// </summary>
        /// <returns>contains count of all loan statuses</returns>
        public static DataSet GetLoanStatistics()
        {
            DbCommand cmd;
            Database db = null;
            DataSet loanStatistics = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                cmd = db.GetStoredProcCommand("dbo.yrs_usp_LAC_GetLoansStatistics");

                if (cmd == null) return null;
                loanStatistics = new DataSet();
                db.LoadDataSet(cmd, loanStatistics, "LoanStatistics");
                return loanStatistics;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd = null;
                db = null;
                loanStatistics = null;
            }
        }
        //END : SB | 07/25/2018 | YRS-AT-4017 | Loan Admin Console - Get and Save prime interest rate, get loan statistics

        //START : SB | 07/25/2018 | YRS-AT-4017 | Loan Admin Console - Get exceptions list for loan admin console
        /// <summary>
        /// Get exceptions list for loan admin console
        /// </summary>
        /// <returns></returns>
        public static DataTable GetLoanExceptions()
        {
            DbCommand cmd;
            Database db = null;
            DataSet loanExceptions = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                cmd = db.GetStoredProcCommand("dbo.yrs_usp_LAC_GetLoanExceptions");

                if (cmd == null) return null;

                loanExceptions = new DataSet();
                db.LoadDataSet(cmd, loanExceptions, "LoanExceptions");
                return loanExceptions.Tables["LoanExceptions"];
            }
            catch (Exception )
            {
                throw;
            }
            finally
            {
                cmd = null;
                db = null;
                loanExceptions = null;
            }
        }
        //END : SB | 07/25/2018 | YRS-AT-4017 | Loan Admin Console - Get exceptions list for loan admin console

        #region "YRS 5.0-2441 : Modifications for 403b Loans"
        public static DataSet GetLoanDetailsForUtility()
        {
            DataSet l_dataset_Loaninfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Loan_GetLoanDetailsForUtility");
                if (LookUpCommandWrapper == null) return null;
                l_dataset_Loaninfo = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_Loaninfo, "YMCAInformation");
                return l_dataset_Loaninfo;
            }
            catch
            {
                throw;
            }
        }
        //Start -- Manthan Rajguru | 2016.02.18 | YRS-AT-2603 | Added parameter to method to get first and last name
        //START: ML | 2019.01.17 | YRS-AT-3157 | Added new output variable(loanNo,paymentCode), which will be used to send email. And removed not required variables
        //public static void UpdateLoanFreezeUnFreeze(int intLoanDetailsId, string strFreezeDate, decimal decLoanFrozenAmount, out string strEmailAddrs, out string strThresholdDays, out string strFirstName, out string strLastName) 
         public static void UpdateLoanFreezeUnFreeze(int intLoanDetailsId, string strFreezeDate, decimal decLoanFrozenAmount,out string loanNo, out string paymentCode)
        //END: ML | 2019.01.17 | YRS-AT-3157 | Added new output variable(loanNo,paymentCode), which will be used to send email. And removed not required variables
        {
            Database db = null;
            DbCommand dbCommand = null;            
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) throw new Exception("Database object is null");
                dbCommand = db.GetStoredProcCommand("yrs_usp_loan_UpdateLoanFreezeUnFreeze");
                if (dbCommand == null) throw new Exception("Database object is null");
                db.AddInParameter(dbCommand, "@intLoanDetailsId", DbType.Int32, intLoanDetailsId);
                db.AddInParameter(dbCommand, "@mnyFrozenAmt", DbType.Decimal, decLoanFrozenAmount);                
                db.AddOutParameter(dbCommand, "@chvEMailAddr", DbType.String, 70);
                db.AddOutParameter(dbCommand, "@chvThresholdDays", DbType.String, 250);
                //START: ML | 2019.01.17 | YRS-AT-3157 | Added new output variable(loanNo,paymentCode), which will be used to send email. And commented not required variables
                //db.AddOutParameter(dbCommand, "@chvFirstName", DbType.String, 30);
                //db.AddOutParameter(dbCommand, "@chvLastName", DbType.String, 20);             
                db.AddOutParameter(dbCommand, "@VARCHAR_LoanNumber", DbType.String, 50);
                db.AddOutParameter(dbCommand, "@VARCHAR_PaymentMethodCode", DbType.String, 6);
                //END: ML | 2019.01.17 | YRS-AT-3157 | Added new output variable(loanNo,paymentCode), which will be used to send email. And commented not required variables

                if (string.IsNullOrEmpty(strFreezeDate))
                {
                    db.AddInParameter(dbCommand, "@dtmFreezedate", DbType.DateTime, DBNull.Value);
                }
                else
                {
                    db.AddInParameter(dbCommand, "@dtmFreezedate", DbType.DateTime, strFreezeDate);
                }

                db.ExecuteNonQuery(dbCommand);

                //START: ML | 2019.01.17 | YRS-AT-3157 | Added new output variable(loanNo,paymentCode), which will be used to send email. And commented not required variables
                //strEmailAddrs = Convert.ToString(db.GetParameterValue(dbCommand, "@chvEMailAddr"));
                //strThresholdDays = Convert.ToString(db.GetParameterValue(dbCommand, "@chvThresholdDays"));
                //strFirstName = Convert.ToString(db.GetParameterValue(dbCommand, "@chvFirstName"));
                //strLastName = Convert.ToString(db.GetParameterValue(dbCommand, "@chvLastName"));

                loanNo = Convert.ToString(db.GetParameterValue(dbCommand, "@VARCHAR_LoanNumber"));
                paymentCode = Convert.ToString(db.GetParameterValue(dbCommand, "@VARCHAR_PaymentMethodCode"));
                //END: ML | 2019.01.17 | YRS-AT-3157 | Added new output variable(loanNo,paymentCode), which will be used to send email. And commented not required variables
            }
            //End -- Manthan Rajguru | 2016.02.18 | YRS-AT-2603 | Added parameter to method to get first and last name
            catch
            {
                throw;
            }
        }
        #endregion


		# region "YMCA PHASE 4"
        public static DataSet GetTDRTBalance(string parameterFundEventID)
        {
			DataSet l_dataset_TDRTBalance = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			string [] l_TableNames;
            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Loan_GetTDRTBalance");
				
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundEventID",DbType.String,parameterFundEventID);
				l_dataset_TDRTBalance = new DataSet();
				l_TableNames = new string[]{"TDRTBalance","RTBalance"} ;
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_TDRTBalance,"TDandRTBalance");
				
				return l_dataset_TDRTBalance;
			}
            catch
            {
				throw;
			}
		}
        public static int CheckNullTransactionRefId(string parameterFundEventID)
        {
			int l_int_TransRefIdIsNull = 0;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return 0;

				LookUpCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Loan_CheckNullTransactionRefId");
				
				if (LookUpCommandWrapper == null) return 0;
				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundEventID",DbType.String,parameterFundEventID);
				db.AddOutParameter(LookUpCommandWrapper,"@bit_TransRefIdIsNull",DbType.Int32,4);
				db.ExecuteNonQuery(LookUpCommandWrapper);
				l_int_TransRefIdIsNull = (int)(db.GetParameterValue(LookUpCommandWrapper,"@bit_TransRefIdIsNull"));
				
				return l_int_TransRefIdIsNull;
			}
            catch
            {
				throw;
			}
		}
        public static void InsertAccountBreakdown(int parameterLoanRequestId, DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction, bool isONDRequested) // VC | 2018.12.03 | YRS-AT-4017 | Accepting new variable isONDRequested.
        {
			DbCommand l_DbCommand;
            try
            {
				if (parameterDatabase == null) return ; 

				if (parameterDataRow == null) return ;

				l_DbCommand = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_Loan_InsertAtsLoanAccountBreakdown");

				if (l_DbCommand == null) return ;

                parameterDatabase.AddInParameter(l_DbCommand, "@int_LoanRequestId", DbType.Int32, parameterLoanRequestId);
                parameterDatabase.AddInParameter(l_DbCommand, "@char_AccountType", DbType.String, Convert.ToString(parameterDataRow["AccountType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@char_AnnuityBasisType", DbType.String, Convert.ToString(parameterDataRow["AnnuityBasisType"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@money_AcctBalance", DbType.Double, Convert.ToDouble(parameterDataRow["AcctBalance"]));
                //
                //parameterDatabase.AddInParameter(l_DbCommand, "@money_LoanAmount", DbType.Double, Convert.ToDouble(parameterDataRow["LoanAmount"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@money_LoanAmount", DbType.Double, Convert.ToDouble(Convert.ToDouble(parameterDataRow["PreTaxLoanAmount"]) + Convert.ToDouble(parameterDataRow["PostTaxLoanAmount"])));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_Percentage", DbType.Double, Convert.ToDouble(parameterDataRow["Percentage"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@varchar_TransactionRefID", DbType.String, Convert.ToString(parameterDataRow["TransactionRefID"]));
                //ASHISH:2011.11.07 YRS 5.0-1322
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_PreTaxPercentage", DbType.Double,  Convert.ToDouble(parameterDataRow["PersPreTaxPct"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@numeric_PostTaxPercentage", DbType.Double,  Convert.ToDouble(parameterDataRow["PersPostTaxPct"]));
                parameterDatabase.AddInParameter(l_DbCommand, "@BIT_IsONDRequested", DbType.Boolean, isONDRequested); // VC | 2018.12.03 | YRS-AT-4017 | Passing new input parameter OND requested or not.

				parameterDatabase.ExecuteNonQuery (l_DbCommand, parameterTransaction);	
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
        public static void InsertFeeTransacts(int parameterLoanRequestId, DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
        {
			DbCommand l_DbCommand;
            try
            {
				if (parameterDatabase == null) return ; 

				if (parameterDataRow == null) return ;

				l_DbCommand = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_LoanInsertFeeTransaction");

				if (l_DbCommand == null) return ;
				
				parameterDatabase.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId );
				parameterDatabase.AddInParameter(l_DbCommand,"@varchar_PersId",DbType.String,Convert.ToString (parameterDataRow["PersId"]) );
				parameterDatabase.AddInParameter(l_DbCommand,"@varchar_FundId",DbType.String,Convert.ToString (parameterDataRow["FundId"]) );
				parameterDatabase.AddInParameter(l_DbCommand,"@char_AccountType",DbType.String,Convert.ToString (parameterDataRow["AccountType"]).Trim() );
				parameterDatabase.AddInParameter(l_DbCommand,"@char_TransactType",DbType.String,Convert.ToString (parameterDataRow["MoneyType"]).Trim() );
				parameterDatabase.AddInParameter(l_DbCommand,"@char_AnnuityBasisType",DbType.String,Convert.ToString (parameterDataRow["AnnuityBasisType"]).Trim() );
				parameterDatabase.AddInParameter(l_DbCommand,"@money_PersonalPreTax",DbType.Double,parameterDataRow["PersonalPreTax"].ToString()!=string.Empty ? Convert.ToDouble (parameterDataRow["PersonalPreTax"]):0 );
				parameterDatabase.AddInParameter(l_DbCommand,"@varchar_TransactionRefID",DbType.String,Convert.ToString (parameterDataRow["TransactionRefID"]) );
                //ASHISH:2011.11.07 YRS 5.0-1322
                parameterDatabase.AddInParameter(l_DbCommand, "@money_PersonalPostTax", DbType.Double, parameterDataRow["PersonalPostTax"].ToString() != string.Empty ? Convert.ToDouble(parameterDataRow["PersonalPostTax"]):0);
				

				parameterDatabase.ExecuteNonQuery (l_DbCommand, parameterTransaction);	
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
        public static void InsertTransacts(int parameterLoanRequestId, DataRow parameterDataRow, Database parameterDatabase, DbTransaction parameterTransaction)
        {
			DbCommand l_DbCommand;
            try
            {
				if (parameterDatabase == null) return ; 

				if (parameterDataRow == null) return ;

				l_DbCommand = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_LoanInsertTransaction");

				if (l_DbCommand == null) return ;
				
				parameterDatabase.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId );
				parameterDatabase.AddInParameter(l_DbCommand,"@varchar_PersId",DbType.String,Convert.ToString (parameterDataRow["PersId"]) );
				parameterDatabase.AddInParameter(l_DbCommand,"@varchar_FundId",DbType.String,Convert.ToString (parameterDataRow["FundId"]) );
				parameterDatabase.AddInParameter(l_DbCommand,"@char_AccountType",DbType.String,Convert.ToString (parameterDataRow["AccountType"]).Trim() );
				parameterDatabase.AddInParameter(l_DbCommand,"@char_TransactType",DbType.String,Convert.ToString (parameterDataRow["TransactType"]).Trim() );
				parameterDatabase.AddInParameter(l_DbCommand,"@char_AnnuityBasisType",DbType.String,Convert.ToString (parameterDataRow["AnnuityBasisType"]).Trim() );
                //ASHISH:2011.11.07 YRS 5.0-1322
                //parameterDatabase.AddInParameter(l_DbCommand,"@money_PersonalPreTax",DbType.Double,Convert.ToDouble (parameterDataRow["LoanAmount"]) );
                parameterDatabase.AddInParameter(l_DbCommand, "@money_PersonalPreTax", DbType.Double, Convert.ToDouble(parameterDataRow["PreTaxLoanAmount"]));
				parameterDatabase.AddInParameter(l_DbCommand,"@varchar_TransactionRefID",DbType.String,Convert.ToString (parameterDataRow["TransactionRefID"]) );

                //ASHISH:2011.11.07 YRS 5.0-1322
                parameterDatabase.AddInParameter(l_DbCommand, "@money_PersonalPostTax", DbType.Double, Convert.ToDouble(parameterDataRow["PostTaxLoanAmount"]));
				

				parameterDatabase.ExecuteNonQuery (l_DbCommand, parameterTransaction);	
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
		

		//Priya -05-June-08 TDLoan enhancement 
        private static void GenerateLoanAmortizationTransacts(int parameterLoanRequestId, Database parameterDatabase, DbTransaction parameterTransaction)
        {//(int parameterLoanId)
			DbCommand l_DbCommand;
			string l_GetErrorValue = null;
			int l_int_GetLoanDetailsId;
			
            try
            {
				if (parameterDatabase == null) return ; 

				l_int_GetLoanDetailsId = GetLoanDetailsId(parameterDatabase ,parameterTransaction,parameterLoanRequestId);
				l_DbCommand = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_Loan_GenerateLoanAmortizationTransacts");
				
				if (l_DbCommand == null) return ;

				parameterDatabase.AddInParameter(l_DbCommand,"@intLoanId",DbType.Int32,l_int_GetLoanDetailsId);
                parameterDatabase.AddOutParameter(l_DbCommand, "@cOutput", DbType.String, 200);
				parameterDatabase.ExecuteNonQuery(l_DbCommand  ,parameterTransaction);
                l_GetErrorValue = parameterDatabase.GetParameterValue(l_DbCommand, "@cOutput").ToString();
				
                if (l_GetErrorValue.ToString() != "")
                {
					throw new Exception(l_GetErrorValue);
				}

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
		//Priya -05-June-08 TDLoan enhancement 
		//Priya -10-June-08 TDLoan enhancement 
        private static int GetLoanDetailsId(Database parameterDatabase, DbTransaction parameterTransaction, int parameterLoanRequestId)
        {
			DbCommand l_DbCommand;
			
			int l_int_LoanDetailsId = 0;
            try
            {
				
				if (parameterDatabase == null)  return 0;
				l_DbCommand = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_GetLoanDetailsId");
				if (l_DbCommand == null) return 0;
							

				parameterDatabase.AddInParameter(l_DbCommand,"@int_LoanRequestId",DbType.Int32,parameterLoanRequestId);
                parameterDatabase.AddOutParameter(l_DbCommand, "@int_LoanDetailsId", DbType.Int32, 0);
				parameterDatabase.ExecuteNonQuery(l_DbCommand,parameterTransaction);
                l_int_LoanDetailsId = Convert.ToInt32(parameterDatabase.GetParameterValue(l_DbCommand, "@int_LoanDetailsId"));
				return l_int_LoanDetailsId;
				
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
		//Priya -10-June-08 TDLoan enhancement 

		//Added by Ashish 08-08-20008 for issue YRS 5.0-489 Start
        public static string GetPersonLatestYmcaId(string parameterPersID)
        {
			DbCommand l_DbCommand;
			Database db = null;
			string l_YmcaId=null;
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
                if (db != null)
                {
					l_DbCommand = db.GetStoredProcCommand ("yrs_usp_Loan_GetPersLatestYmca");
				
					
				
					db.AddInParameter(l_DbCommand,"@varchar_guiPersID",DbType.String,parameterPersID);
				
					object l_object=db.ExecuteScalar(l_DbCommand);
                    if (l_object.GetType().ToString() != "System.DBNull")
                    {
						l_YmcaId=Convert.ToString(l_object); 
					}
				}

				return l_YmcaId;
				

			}
			
            catch (Exception ex)
            {
				throw ex;
			}
            finally
            {
				db=null;
			}
		}

		//Added by Ashish 08-08-20008 YRS 5.0-489 End

		//Added by Ashish 27-01-2009  Start
        public static int ValidateQDRORequestPending(string parameterFundEventID)
        {
			int l_int_QDRORequestPending = 0;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

            try
            {
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return 0;

				LookUpCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Loan_ValidateQDROPendingRequest");
				
				if (LookUpCommandWrapper == null) return 0;
				
				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundEventID",DbType.String,parameterFundEventID);
				
				db.AddOutParameter(LookUpCommandWrapper,"@bit_QDRORequestPending",DbType.Int32,4);
				db.ExecuteNonQuery(LookUpCommandWrapper);
				l_int_QDRORequestPending = (int)(db.GetParameterValue(LookUpCommandWrapper,"@bit_QDRORequestPending"));
				
				return l_int_QDRORequestPending;
			}
            catch
            {
				throw;
			}
		}
		//Added by Ashish 27-01-2009  End

		//Added by Ashish 17-Mar-2009 for issue YRS 5.0-679, Start 
		
        public static int GetReAmortizeLoanNumber(int paraOrgiLoanNumber)
        {
			DbCommand l_DbCommand;
			DbConnection l_DbConnection = null;
			Database db = null;
			int l_NewLoanNumber=0;
			
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				l_DbConnection =db.CreateConnection();
				l_DbConnection.Open();
				
				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_Loan_GetReamortizedLoanNumber");				
				
				db.AddInParameter(l_DbCommand,"@int_OrgiLoanNumber",DbType.Int32,paraOrgiLoanNumber);
				
				object l_object=db.ExecuteScalar(l_DbCommand);
                if (l_object != null)
                {
                    if (l_object.GetType().ToString() != "System.DBNull")
                    {
						l_NewLoanNumber=Convert.ToInt32(l_object); 
					}
				}

				return l_NewLoanNumber;
				

			}
			
            catch (Exception ex)
            {
				throw ex;
			}
            finally
            {
                if (l_DbConnection != null)
                {
                    if (l_DbConnection.State != ConnectionState.Closed)
                    {
						l_DbConnection.Close ();
					}
					l_DbConnection=null;
				}
				db=null;
			}
		}
        public static int GetReAmortizeReason(string paraYmcaID)
        {
			DbCommand l_DbCommand;
			DbConnection l_DbConnection = null;
			Database db = null;
			int l_ReamortizedReason=0;
			
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				l_DbConnection =db.CreateConnection();
				l_DbConnection.Open();
				
				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_Loan_GetReamortizedReason");				
				
				db.AddInParameter(l_DbCommand,"@varchar_YmcaID",DbType.String,paraYmcaID);
				
				object l_object=db.ExecuteScalar(l_DbCommand);
                if (l_object != null)
                {
                    if (l_object.GetType().ToString() != "System.DBNull")
                    {
						l_ReamortizedReason=Convert.ToInt32(l_object); 
					}
				}
				

				return l_ReamortizedReason;
				

			}
			
            catch (Exception ex)
            {
				throw ex;
			}
            finally
            {
                if (l_DbConnection != null)
                {
                    if (l_DbConnection.State != ConnectionState.Closed)
                    {
						l_DbConnection.Close ();
					}
					l_DbConnection=null;
				}
				db=null;
			}
		}
		//Added by Ashish 17-Mar-2009 for issue YRS 5.0-679, End

        public static string GetYMCAEmailId(string parameterYmcaID)
        {
			DbCommand l_DbCommand;
			DbConnection l_DbConnection = null;
			Database db = null;
			string returnValue = string.Empty;
			
            try
            {
				db = DatabaseFactory.CreateDatabase("YRS");
				l_DbConnection =db.CreateConnection();
				l_DbConnection.Open();
				
				l_DbCommand = db.GetStoredProcCommand ("dbo.yrs_usp_Loan_GetYMCAEmailID");
				db.AddInParameter(l_DbCommand,"@varchar_YmcaID", DbType.String, parameterYmcaID);
				
				object l_returnValue = db.ExecuteScalar(l_DbCommand);
                if (l_returnValue != null)
                {
                    if (l_returnValue.GetType().ToString() != "System.DBNull")
                    {
						returnValue = (string)l_returnValue;
					}
				}
				return returnValue;
			}
            catch (Exception ex)
            {
				throw ex;
			}
            finally
            {
                if (l_DbConnection != null)
                {
                    if (l_DbConnection.State != ConnectionState.Closed)
                    {
						l_DbConnection.Close ();
					}
					l_DbConnection=null;
				}
				db=null;
			}
		}
        //Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a method to update freezedate
        public static decimal[] GetOffsetAmount(int parameterLoanRequestId, string paramaterOffsetDate, string parameterLoanStatus, out string strEmpstatus, out string strAge)
        {
            Database db = null;
            decimal[] paramaterOffsetAmount = new decimal[2];
            decimal decPrincipalAmt = 0.00M;
            decimal decIntAmt = 0.00M;            
            DbCommand GetCommandWrapper = null;
            strEmpstatus = "";
            strAge = "";
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                GetCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Loan_GetOffsetAmount");
                db.AddInParameter(GetCommandWrapper, "@intLoanRequestId", DbType.Int32, parameterLoanRequestId);
                db.AddInParameter(GetCommandWrapper, "@chvOffsetDate", DbType.String, paramaterOffsetDate);
                db.AddInParameter(GetCommandWrapper, "@chvLoanStatus", DbType.String, parameterLoanStatus);                                
                db.AddOutParameter(GetCommandWrapper, "@numericPrincipalAmount", DbType.Double, 100);
                db.AddOutParameter(GetCommandWrapper, "@numericIntAmount", DbType.Double, 100);                
                db.AddOutParameter(GetCommandWrapper, "@chrEmpstatus", DbType.String, 100);
                db.AddOutParameter(GetCommandWrapper, "@chrAge", DbType.String, 100);
                if (GetCommandWrapper == null) return null;
                db.ExecuteNonQuery(GetCommandWrapper);                
                decPrincipalAmt = Convert.ToDecimal(db.GetParameterValue(GetCommandWrapper, "@numericPrincipalAmount"));
                decIntAmt = Convert.ToDecimal(db.GetParameterValue(GetCommandWrapper, "@numericIntAmount"));                
                strEmpstatus = Convert.ToString(db.GetParameterValue(GetCommandWrapper, "@chrEmpstatus"));
                strAge = Convert.ToString(db.GetParameterValue(GetCommandWrapper, "@chrAge"));
                paramaterOffsetAmount[0] = decPrincipalAmt;
                paramaterOffsetAmount[1] = decIntAmt;                           
                return paramaterOffsetAmount;

    }
            catch
            {
                throw;
            }

        }

        public static bool IsLoanOffsetValid(int parameterLoanRequestId, out int intOffseteventReason )
        {
            DbCommand l_DbCommand;
            Database db = null;
            bool bitOffsetEventOccured = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                intOffseteventReason = 0;
                if (db == null) return bitOffsetEventOccured;
                l_DbCommand = db.GetStoredProcCommand("dbo.yrs_usp_Loan_IsLoanOffsetValid");

                l_DbCommand.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DbCommand == null) return bitOffsetEventOccured;


                db.AddInParameter(l_DbCommand, "@int_LoanRequestId", DbType.Int32, parameterLoanRequestId);
                db.AddOutParameter(l_DbCommand, "@bitOffsetEventOccured", DbType.Boolean, 2);
                db.AddOutParameter(l_DbCommand, "@intOffseteventReason", DbType.Int32, 2);
                
                db.ExecuteNonQuery(l_DbCommand);
                bitOffsetEventOccured = Convert.ToBoolean(db.GetParameterValue(l_DbCommand, "@bitOffsetEventOccured"));
                intOffseteventReason = Convert.ToInt32(db.GetParameterValue(l_DbCommand, "@intOffseteventReason"));
                return bitOffsetEventOccured;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        //AA:10.26.2015  YRS-AT-2533 : changed to get the default and offset dates
        //public static string GetOffsetDate(int parameterLoanRequestId, string parameterLoanStatus)
        public static Dictionary<string,string> GetOffsetAndDefaultDates(int parameterLoanRequestId, string parameterLoanStatus)
        {
            Database db = null;
            string parameterOffsetDate = "";
            string parameterDefaultDate = "";   //AA:10.26.2015  YRS-AT-2533 : changed to get the default date
            Dictionary<string,string> dictDates = new Dictionary<string,string>(); //AA:10.26.2015  YRS-AT-2533 : changed to get the default and offset dates
            DbCommand GetCommandWrapper = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                GetCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_Loan_GetOffsetDate");
                db.AddInParameter(GetCommandWrapper, "@intLoanRequestId", DbType.Int32, parameterLoanRequestId);
                db.AddInParameter(GetCommandWrapper, "@chvLoanStatus", DbType.String ,parameterLoanStatus);
                db.AddOutParameter(GetCommandWrapper, "@chvOffsetdate", DbType.String, 100);
                db.AddOutParameter(GetCommandWrapper, "@chvDefaultDate", DbType.String, 100);   //AA:10.26.2015  YRS-AT-2533 : changed to get the default date
                if (GetCommandWrapper == null) return null;
                db.ExecuteNonQuery(GetCommandWrapper);
                parameterOffsetDate = Convert.ToString(db.GetParameterValue(GetCommandWrapper, "@chvOffsetdate"));
                //Start:AA:10.26.2015  YRS-AT-2533 : changed to get the default and offset dates
                parameterDefaultDate = Convert.ToString(db.GetParameterValue(GetCommandWrapper, "@chvDefaultDate"));               
                dictDates.Add("OffsetDate", parameterOffsetDate.Trim());
                dictDates.Add("DefaultDate", parameterDefaultDate.Trim());
                return dictDates;
                //End:AA:10.26.2015  YRS-AT-2533 : changed to get the default and offset dates
            }
            catch
            {
                throw;
            }

        }
        

        //End: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a method to update freezedate

        //START: MMR | 2018.03.04 | YRS-AT-3929 | Get loan information for email content which will be sent to participant if payment mode is EFT
        /// <summary>
        /// This method will return Loan information for email content
        /// </summary>
        /// <param name="parameterLoanRequestId">Loan Request ID</param>
        /// <returns>Loan Information Table</returns>
        public static DataTable GetLoanInformationForMail(int loanRequestId)
        {
            DataSet ds = null;
            Database db = null;
            DbCommand cmd = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                cmd = db.GetStoredProcCommand("yrs_usp_Loan_GetLoanDetailForEmail");
                if (cmd == null) return null;
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                ds = new DataSet();
                db.AddInParameter(cmd, "@INT_LoanRequestID", DbType.Int32, loanRequestId);
                db.LoadDataSet(cmd, ds, "loanInformationForMail");
                return ds.Tables["loanInformationForMail"];
            }
            catch
            {
                throw;
            }
        }
        //END: MMR | 2018.03.04 | YRS-AT-3929 | Get loan information for email content which will be sent to participant if payment mode is EFT

        //START: VC | 2018.07.19 | YRS-AT-4017 | Provides loan details
        public static DataTable GetWebLoans(YMCAObjects.WebLoan webLoan)
        {
            DataSet ds;
            Database db;
            DbCommand cmd;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_LAC_GetWebLoans");
                if (cmd == null) return null;

                ds = new DataSet();
                db.AddInParameter(cmd, "@VARCHAR_FullName", DbType.String, webLoan.FullName);
                db.AddInParameter(cmd, "@VARCHAR_StartDate", DbType.String, webLoan.StartDate);
                db.AddInParameter(cmd, "@VARCHAR_EndDate", DbType.String, webLoan.EndDate);
                db.AddInParameter(cmd, "@VARCHAR_FundId", DbType.String, webLoan.FundId);
                db.AddInParameter(cmd, "@VARCHAR_Amount", DbType.String, webLoan.Amount);
                db.AddInParameter(cmd, "@VARCHAR_Status", DbType.String, webLoan.Status);
                db.LoadDataSet(cmd, ds, "loanInformation");
                return ds.Tables["loanInformation"];
            }
            catch
            {
                throw;
            }
            finally 
            {
                ds = null;
                db = null;
                cmd = null;
            }
        }
        //END: VC | 2018.07.19 | YRS-AT-4017 | Provides loan details

        //START: MMR | 2018.07.23 | YRS-AT-4017 | Get List of loans for Processing
        /// <summary>
        /// Provides list of loans for processing
        /// </summary>
        /// <returns>Loans processing details</returns>
        public static DataTable GetLoansForProcessing()
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_LAC_GetLoansForProcessing");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "LoansForProcessing");
                return ds.Tables["LoansForProcessing"];
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
        //END: MMR | 2018.07.23 | YRS-AT-4017 | Get List of loans for Processing

        //START: MMR | 2018.09.12 | YRS-AT-4017 | Get loan validation Reason
        public static string GetLoanValidationReason(int loanRequestId)
        {
            Database db;
            DbCommand cmd;
            string reason;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_LAC_ValidateLoanRequest");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_LoanRequestId", DbType.Int32);
                db.AddOutParameter(cmd, "@VARCHAR_Reason", DbType.String, 300);
                db.ExecuteNonQuery(cmd);
                reason = Convert.ToString(db.GetParameterValue(cmd,"@VARCHAR_Reason"));
                return reason;
            }
            catch
            {
                throw;
            }
            finally
            {
                reason = string.Empty;
                cmd = null;
                db = null;
            }
        }
        //END: MMR | 2018.09.12 | YRS-AT-4017 | Get loan validation Reason

        //START: ML | 2019.01.08 | YRS-AT-4244 | YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462) 
        public static Boolean  UpdateONDStatus( string  ondData)
        {
            DbCommand command;
            DbTransaction transaction = null;
            DbConnection connection = null;
            Database db = null;
          
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;

                //Get a connection and Open it
                connection = db.CreateConnection();
                connection.Open();

                transaction = connection.BeginTransaction();
                if (transaction == null) return false;

                command = db.GetStoredProcCommand("dbo.yrs_usp_UpdateONDStatus");
                if (command == null) return false;

                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                db.AddInParameter(command, "@XML_LoanONDStatusDetails", DbType.Xml, ondData);
                db.ExecuteNonQuery(command, transaction);

                //Commit the transaction if everything was fine
                transaction.Commit();

                return true;
            }

            catch (Exception ex)
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
                ondData = string.Empty;               
                connection = null;
                transaction = null;
                command = null;
                db = null;
            }
        }
        //END: ML | 2019.01.08 | YRS-AT-4244 | YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462) 

        //START: ML | 2019.01.11 | YRS-3157 | Fetching loan freeze unfreeze history
        /// <summary>
        /// Provides loan freeze / unfreeze history
        /// </summary>
        /// <returns>Loans processing details</returns>
        public static DataTable LoanFreezeUnfreezeHistoryList(string loanDetailId)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_Loan_GetLoanHistory");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@INT_LoanDetailsID", DbType.Int32, loanDetailId);

                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "AtsLoanHistory");
                return ds.Tables["AtsLoanHistory"];
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
        //END: ML | 2019.01.11 | YRS-3157 | Fetching loan freeze unfreeze history

        //START: PK | 01.15.2019 | YRS-AT-2573 | Get list for first payment date
        public static DataTable GetListForFirstPaymentDate(int loanRequestId, string ymcaID)
        {
            DataSet ds;
            Database db;
            DbCommand cmd;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_Loan_GetListForFirstPaymentDate");
                if (cmd == null) return null;

                ds = new DataSet();
                db.AddInParameter(cmd, "@INT_LoanRequestID", DbType.Int32, loanRequestId);
                db.AddInParameter(cmd, "@VARCHAR_YmcaID", DbType.String, ymcaID);
                db.LoadDataSet(cmd, ds, "LoanFirstPaymentDate");
                return ds.Tables["LoanFirstPaymentDate"];
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                db = null;
                cmd = null;
            }
        }
        //END: PK | 01.15.2019 |YRS-AT-2573 | Get list for first payment date
        //START: MMR | 2019.01.23 | YRS-AT-4118 | Get active YMCA Details
        public static DataSet GetActiveYmcaDetails(string loanRequestID)
        {
            DataSet dsCloseLoanDetails = null;
            Database db = null;
            DbCommand commandCloseLoanDetails = null;
            string[] tableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                commandCloseLoanDetails = db.GetStoredProcCommand("yrs_usp_Loan_GetActiveYMCADetails");
                if (commandCloseLoanDetails == null) return null;
                commandCloseLoanDetails.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                dsCloseLoanDetails = new DataSet();
                db.AddInParameter(commandCloseLoanDetails, "@int_LoanRequestId", DbType.Int32, loanRequestID);
                tableNames = new string[] { "EmployeeActiveYMCA", "FirstPayDates" };
                db.LoadDataSet(commandCloseLoanDetails, dsCloseLoanDetails, tableNames);
                return dsCloseLoanDetails;
            }
            catch
            {
                throw;
            }
            finally
            {
                dsCloseLoanDetails = null;
                db = null;
                commandCloseLoanDetails = null;
                tableNames = null;
            }
        }
        //END: MMR | 2019.01.23 | YRS-AT-4118 | Get active YMCA Details

        // Start | SC | 2020.04.10 | YRS-AT-4852 | Get COVID Transactions for Loan request ID
        public static DataSet GetCovidTransactions(int loanRequestID, string transactionType)
        {
            DataSet dsCovidTransactions;
            Database db;
            DbCommand cmd;
           
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                dsCovidTransactions = new DataSet();
                cmd = db.GetStoredProcCommand("yrs_usp_CVD_GetCovidTransactions");
                if (cmd == null) return null;
                
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                db.AddInParameter(cmd, "@transactionReferenceId", DbType.String, Convert.ToString(loanRequestID));
                db.AddInParameter(cmd, "@type", DbType.String, transactionType);
                db.LoadDataSet(cmd, dsCovidTransactions, "CovidTransactions");
                return dsCovidTransactions;
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
                cmd = null;
                dsCovidTransactions = null;
            }
        }
        // End | SC | 2020.04.10 | YRS-AT-4852 | Get COVID Transactions for Loan request ID
	}

	#endregion
}

