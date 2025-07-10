//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	LoanInformationBOClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8642
// Creation Time		:	4/10/2006 10:04:52 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	Business class for LoanInformation form
//*******************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified By			Date				Description
//********************************************************************************************************************************
//Ashish Srivastava		08-08-2008			Added new function (GetPersonLatestYmcaId) for issue YRS 5.0-489
//Ashish Srivastava		17-Mar-2009			Generate Reamortization Letter for issue YRS5.0 679
//Nikunj Patel			2009.04.22			Changing code to create a procedure to fetch the Email Id of the TRANSM YMCA Contact for the specified YMCA Id
//Dilip Yadav           2009.09.02          To implement n-tier logic for loand amount and fee deducation based on loan PS _ Phase V_Part3
//Priya Jawale/Md Hafiz	2010.02.03/			YRS 5.0-1007 : LFPR Transactions not created during th loan process with RT money only
//						2010.02.12			integrating the missing fix in part 5 as fixed in part 4
//Shagufta             15 July 2011         For YRS 5.0-1320,BT-829:letter when loan is paid off
//Ashish Srivastava     2011.11.07          YRS 5.0-1322 Include after tax money from RT account
//Shashank Patel		2014.01.15			BT-2314\YRS 5.0-2260 - YRS generating blank loan payoff letters 
//Anudeep A             2014.08.26          BT:2625 :YRS 5.0-2405-Consistent screen header sections 
//Anudeep A             2015.03.12          BT:2699:YRS 5.0-2441 : Modifications for 403b Loans 
//DInesh K              2015.04.08          BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
//Anudeep A             2015.05.05          BT-2699:YRS 5.0-2441 : Modifications for 403b Loans 
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Anudeep A             2015.10.26          YRS-AT-2533 - Loan Modifications (Default) Project: two new YRS letters
//Manthan Rajguru       2016.01.11          YRS-AT-2578 - YRS enh: one cent Rollover Account, divide by zero error for Loan Amortizations
//Pramod Prakash Pokale 2016.01.27          YRS-AT-2578 - YRS enh: one cent Rollover Account, divide by zero error for Loan Amortizations
//Manthan Rajguru       2016.02.18          YRS-AT-2603: YRS enh: Withdrawals email - add participants name to eliminate 'forwarded' email confusion
//Anudeep A             2016.04.26          YRS-AT-2831 - YRS enh: automate 'Default' of Loans (TrackIT 25242)
//Pramod Prakash Pokale 2016.07.12          YRS-AT-2578 - YRS enh: one cent Rollover Account, divide by zero error for Loan Amortizations
//Pramod Prakash Pokale 2017.01.27          YRS-AT-3123 - YRS bug: Loan amortization/reAmortization via fn_splitAmount for low balance accounts 
//Manthan Rajguru       2018.03.04          YRS-AT-3929 -  YRS REPORTS: EFT: Loans ( FIRST EFT PROJECT) -create new "Loan Acknowledgement" Email for EFT/direct deposit 
//Vinayan C             2018.06.21          YRS-AT-3190 -  YRS enh: add warning message for Loan Re-amortization (to prevent duplicate re-amortize efforts
//Vinayan C             2018.07.19          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
//Santosh Bura          2018.07.24          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
//Vinayan C             2018.08.20          YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab
//Manthan Rajguru       2018.07.23          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
//Manthan Rajguru       2018.10.11          YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//Megha Lad             2019.01.08          YRS-AT-4244-  YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462) 
//Pooja K               2019.01.15          YRS-AT-2573 -  YRS enh: add dropdown for first payment due date for Loan ReAmortization (TrackIT 23592) 
//Megha Lad             2019.01.17          YRS-AT-3157-  YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)
//Manthan R             2019.01.23      	YRS-AT-4118 -  YRS enh-unsuspended loans when reamortized must be extended by the suspension period(Track it 35417)
//Shiny C.              2020.04.10          YRS-AT-4852 -  COVID - YRS changes needed for LOANS due to CARE Act (Track IT - 41693) 
//Shiny C.              2020.04.20          YRS-AT-4853 - COVID - YRS changes for Loan Limit Enhancement, due to CARE Act (Track IT - 41693) 
//********************************************************************************************************************************
using System.Data;

namespace YMCARET.YmcaBusinessObject 
{
using System;
using System.Collections.Generic;
	/// <summary>
	/// Summary description for LoanInformationBOClass.
	/// </summary>
	public class LoanInformationBOClass 
	{
		public LoanInformationBOClass() 
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// Method to call the DA Method to fetch list of eligible participants
		/// </summary>
		/// <param name="parameterSSNo"></param>
		/// <param name="parameterFundNo"></param>
		/// <param name="parameterLastName"></param>
		/// <param name="parameterFirstName"></param>
		/// <returns>DataSet</returns>
		/// 
		//		static string [] m_string_AnnuityBasisType ;
		//		public static string[] AnnuityBasisType
		//		{   
		//			get
		//			{
		//				return m_string_AnnuityBasisType;
		//			}
		//			set
		//			{
		//				m_string_AnnuityBasisType= value;
		//			}
		//		}
		public static DataSet LookUpPerson(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName) 
		{
				
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.LookUpPerson(parameterSSNo, parameterFundNo, parameterLastName, parameterFirstName);
			}
			catch 
			{
				throw;
			}
		}
		/// <summary>
		/// To fetch general details of a person using RefundRequest Business class
		/// </summary>
		/// <param name="parameterPersonID"></param>
		/// <returns>DataSet</returns>
		public static DataSet LookupMemberDetails (string parameterPersonID,string parameterFundEventID) 
		{
			try 
			{
				//				LoanInformationWrapperClass objWrapperClass = new LoanInformationWrapperClass();
				//				objWrapperClass.ProrateLoanAmount(parameterFundEventID);
				return RefundRequest.LookupMemberDetails (parameterPersonID,parameterFundEventID);
			}
			catch 
			{
				throw;
			}
		}
		/// <summary>
		/// To fetch account details of the participant using RefundRequest Business Class
		/// </summary>
		/// <param name="parameterFundID"></param>
		/// <returns>DataSet</returns>
        //AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
        public static DataSet LookupMemberAccounts(string parameterFundID, bool parameterblnIncludeLoanAccts) 
		{
			try 
			{
                //AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
				return RefundRequest.LookupTransactionForRefunds (parameterFundID,parameterblnIncludeLoanAccts);
			}
			catch 
			{
				throw;
			}
		}
		/// <summary>
		/// To fetch member notes using RefundRequest Business Class
		/// 
		/// </summary>
		/// <param name="paramaeterPersonID"></param>
		/// <returns>DataTable</returns>
		public static DataTable MemberNotes(string paramaeterPersonID) 
		{
			try 
			{
				return (RefundRequest.MemberNotes (paramaeterPersonID));
			}
			catch 
			{
				throw;
			}
		}
		/// <summary>
		/// To Get Current PIA Value usin RefundRequest Business Class
		/// </summary>
		/// <param name="parameterFundID"></param>
		/// <returns>decimal</returns>
		public static decimal GetCurrentPIA (string parameterFundID) 
		{
			try 
			{
				return (RefundRequest.GetCurrentPIA (parameterFundID));
			}
			catch 
			{
				throw ;
			}
		}
		/// <summary>
		/// To get Termination PIA using RefundRequest Business Class
		/// </summary>
		/// <param name="parameterFundID"></param>
		/// <returns>decimal</returns>
		public static decimal GetTerminatePIA (string parameterFundID) 
		{
			try 
			{
				return (RefundRequest.GetTerminatePIA(parameterFundID));
			}
			catch 
			{
				throw ;
			}
		}
		public static DataTable GetDeductions() 
		{
			try 
			{
				return (YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetDeductions().Tables[0]);
			}
			catch 
			{
				throw ;
			}
		}
		/// <summary>
		/// To get the Loan Requests 
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns>Dataset</returns>
        public static DataSet LookUpLoanRequests(string parameterPersId, string source = "YRS") //VC | 2018.08.20 | YRS-AT-4018 | Added parameter to pass source
		{
			try 
			{
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.LookUpLoanRequests(parameterPersId, source);//VC | 2018.08.20 | YRS-AT-4018 | Passing parameter source
			}
			catch 
			{
				throw;
			}

		}
		public static DataSet GetRequestSchema() 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetRequestSchema();
			}
			catch 
			{
				throw;
			}

		}
		public static string GetLoanDetailsForLoanOptions(string parameterLoanRequestId) 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanDetailsForLoanOptions(parameterLoanRequestId);
			}
			catch 
			{
				throw;
			}

		}
        //START: MMR | 2019.01.23 | YRS-AT-4118 | Commented existing code and added parameter for YMCAID and first payment date
        //public static DataSet GetLoanDetailsForCloseLoan(string parameterLoanRequestId, int parameterStatus, string parameterUnsuspenddate, out int parameterMessage) // 'AA:26.08.2014-BT:2625 :changed the datatype of varabile to get message from database
        public static DataSet GetLoanDetailsForCloseLoan(string parameterLoanRequestId, int parameterStatus, string parameterUnsuspenddate, string ymcaId, string firstPaymentDate, out int parameterMessage)
		//END: MMR | 2019.01.23 | YRS-AT-4118 | Commented existing code and added parameter for YMCAID and first payment date
        {
			try 
			{
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanDetailsForCloseLoan(parameterLoanRequestId, parameterStatus, parameterUnsuspenddate, ymcaId, firstPaymentDate, out parameterMessage); //MMR | 2019.01.23 | YRS-AT-4118 | Added parameter for YMCAID and first payment date
			}
			catch 
			{
				throw;
			}

		}
		/// <summary>
		/// Method to add a new loan request
		/// </summary>
		/// <param name="parameterdsLoanRequest"></param>
		public static void AddRequest(DataSet parameterdsLoanRequest) 
		{
			try 
			{
				YMCARET.YmcaDataAccessObject.LoanInformationDAClass.AddRequest(parameterdsLoanRequest);
			}
			catch 
			{
				throw;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <param name="parameterFundId"></param>
		/// <param name="parameterAmount"></param>
		/// <param name="parameterDataSet"></param>
		/// <returns></returns>
		public static string SaveProcessingData(string parameterPersId,int parameterLoanRequestId,string parameterFundId,double parameterAmount,DataSet parameterDataSet) 
		{
			try 
			{
				LoanInformationWrapperClass objWrapperClass = new LoanInformationWrapperClass();
				if(objWrapperClass.ProrateLoanAmount(parameterFundId,parameterAmount,parameterPersId) == true) 
				{

                    return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.SaveProcessingData(parameterPersId, parameterLoanRequestId, parameterFundId, parameterAmount, parameterDataSet, objWrapperClass.LoanTransactions, objWrapperClass.LoanFeeTransactions, objWrapperClass.LoanBreakdown);
                }
				else 
				{
					return "Error";
				}
			}
			catch 
			{
				throw;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterLoanRequestId"></param>
		public static void CancelPendingLoanRequest(int parameterLoanRequestId) 
		{
			try 
			{
				YMCARET.YmcaDataAccessObject.LoanInformationDAClass.CancelPendingLoanRequest(parameterLoanRequestId);
			}
			catch 
			{
				throw;
			}
		}

        //START: MMR | 2018.10.11 | YRS-AT-3101 | Commneted existing code to cancel loan with DISB request and added to cancel loan with PEND/DISB Request
        public static YMCAObjects.ReturnObject<bool> CancelLoanRequest(int loanRequestId, string personID, string paymentMethodCode, string loanNumber)
        {
            YMCAObjects.ReturnObject<bool> result, emailResult;
            MailUtil mailClient;
            string errorMessage;
            try
            {
                result = new YMCAObjects.ReturnObject<bool>();
                emailResult = new YMCAObjects.ReturnObject<bool>();
                result.MessageList = new List<string>();
                mailClient = new MailUtil();

                //Cancel operation
                errorMessage = YMCARET.YmcaDataAccessObject.LoanInformationDAClass.CancelLoanRequest(loanRequestId);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    result.MessageList.Add(YMCAObjects.MetaMessageList.MESSAGE_LOAN_CANCELLED.ToString());
                    //Sending email if cancelation is success
                    emailResult = mailClient.SendLoanEmail(YMCAObjects.LoanStatus.CANCEL, loanNumber, paymentMethodCode, null);
                    if (emailResult.MessageList != null && emailResult.MessageList.Count > 0) result.MessageList.AddRange(emailResult.MessageList);
                }
                else
                {
                    result.Value = false;
                    result.MessageList.Add(errorMessage);
                }
                return result;
            }
            catch
            {
                throw;
            }
            finally 
            {
                errorMessage = null;
                result = null;
                mailClient = null;
            }
        }
        //END: MMR | 2018.10.11 | YRS-AT-3101 | Commneted existing code to cancel loan with DISB request and added to cancel loan with PEND/DISB Request
        // Start | SC | 2020.04.10 | YRS-AT-4852 | Added reason code for loan suspend
        //public static void CancelLoanUpdateAmortization(string parameterLoanStatus, string parameterRequestField, int parameterLoanRequestId, string parameterSuspendDate, string parameterUnSuspendDate, string parameterDefaultDate, DataSet parameterCreateNewRequest, double parameterPayOffAmount, int intOffseteventReason) 
		public static void CancelLoanUpdateAmortization(string parameterLoanStatus, string parameterRequestField, int parameterLoanRequestId, string parameterSuspendDate, string parameterUnSuspendDate, string parameterDefaultDate, DataSet parameterCreateNewRequest, double parameterPayOffAmount, int intOffseteventReason, string parameterSuspendReasonCode="") 
		{
			try 
			{
                //YMCARET.YmcaDataAccessObject.LoanInformationDAClass.CancelLoanUpdateAmortization(parameterLoanStatus, parameterRequestField, parameterLoanRequestId, parameterSuspendDate, parameterUnSuspendDate, parameterDefaultDate, parameterCreateNewRequest, parameterPayOffAmount, intOffseteventReason);
                YMCARET.YmcaDataAccessObject.LoanInformationDAClass.CancelLoanUpdateAmortization(parameterLoanStatus, parameterRequestField, parameterLoanRequestId, parameterSuspendDate, parameterUnSuspendDate, parameterDefaultDate, parameterCreateNewRequest, parameterPayOffAmount, intOffseteventReason, parameterSuspendReasonCode);
			}
			catch 
			{
				throw;
			}
		}
        // End | SC | 2020.04.10 | YRS-AT-4852 | Added reason code for loan suspend
		public static void LoanProcessingAfterReamortization(int parameterLoanRequestId) 
		{
			try 
			{
				YMCARET.YmcaDataAccessObject.LoanInformationDAClass.LoanProcessingAfterReamortization(parameterLoanRequestId);
			}
			catch 
			{
				throw;
			}
		}
		public static void SavePayOffDetails(int parameterLoanRequestId,double parameterPayOffAmt) 
		{
			try 
			{
				YMCARET.YmcaDataAccessObject.LoanInformationDAClass.SavePayOffDetails(parameterLoanRequestId,parameterPayOffAmt);
			}
			catch 
			{
				throw;
			}
		}

		public static void InsertDisbursementsEarlyLoanPayOff(string  parameterPersId,int parameterLoanRequestId) 
		{
			try 
			{
				YMCARET.YmcaDataAccessObject.LoanInformationDAClass.InsertDisbursementsEarlyLoanPayOff(parameterPersId,parameterLoanRequestId);
			}
			catch 
			{
				throw;
			}
		}
		public static int IsLoanPayOffValid(int parameterLoanRequestId) 
		{
			
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.IsLoanPayOffValid(parameterLoanRequestId);
			}	
			catch 
			{
				throw;
			}
		}
        public static int IsLoanUnpaidFromCureperiod(int parameterLoanRequestId, out string parameterMessage) 
		{
			
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.IsLoanUnpaidFromCureperiod(parameterLoanRequestId,out parameterMessage);
			}	
			catch 
			{
				throw;
			}
		}
		public static int IsActiveMultipleYMCA(int parameterLoanRequestId) 
		{
			
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.IsActiveMultipleYMCA(parameterLoanRequestId);
			}
            catch
            {
                throw;
            }
        }
        public static int IsValidSuspension(string parameterLoanRequestId)
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.IsValidSuspension(parameterLoanRequestId);
        }

        // Start | SC | 2020.04.10 | YRS-AT-4852 | Added method to Populate dropdown list with reasons of loan suspension
        public static DataSet GetLoanSuspendReasons(string parameterLoanRequestId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanSuspendReasons(parameterLoanRequestId);
            }
            catch
            {
                throw;
            }
        }
        // End | SC | 2020.04.10 | YRS-AT-4852 | Added method to Populate dropdown list with reasons of loan suspension
        
		public static int IsLoanCancelValid(int parameterLoanRequestId,string parameterLoanStatus) 
		{
			
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.IsLoanCancelValid(parameterLoanRequestId,parameterLoanStatus);
			}	
			catch 
			{
				throw;
			}
		}
        //Start: AA:04.26.2016 YRS-AT-2831 removed variable as the output parameter is not necesary
		//public static string GetDefaultDate(int parameterLoanRequestId,out int int_fundresult) 
        public static string GetDefaultDate(int parameterLoanRequestId)
        //End: AA:04.26.2016 YRS-AT-2831 removed variable as the output parameter is not necesary
		{
			
			try 
			{
                //Start: AA:04.26.2016 YRS-AT-2831 removed variable as the output parameter is not necesary
				//return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetDefaultDate(parameterLoanRequestId,out int_fundresult);
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetDefaultDate(parameterLoanRequestId);
                //End: AA:04.26.2016 YRS-AT-2831 removed variable as the output parameter is not necesary
			}	
			catch 
			{
				throw;
			}
		}
		public static DataSet GetDefaultAmount(int parameterLoanRequestId,string paramaterDefaultDate) 
		{
			
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetDefaultAmount(parameterLoanRequestId,paramaterDefaultDate);
			}	
			catch 
			{
				throw;
			}
		}

        //START : SC | 2020.04.20 | YRS-AT-4853 | Get the maximum loan amount based on participant if applied for covid loan or regular loan
        //public static string GetLoanMaximumAmount()
        public static string GetLoanMaximumAmount(int parameterFundID) 
		{
			try 
			{
				//return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanMaximumAmount();
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanMaximumAmount(parameterFundID);
			}	
			catch 
			{
				throw;
			}
		}
        //END : SC | 2020.04.20 | YRS-AT-4853 | Get the maximum loan amount based on participant if applied for covid loan or regular loan

		public static int CheckForLoanId(int parameterLoanRequestId) 
		{
			
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.CheckForLoanId(parameterLoanRequestId);
			}	
			catch 
			{
				throw;
			}
		}
		public static int CheckNullTransactionRefId(string parameterFundEventID) 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.CheckNullTransactionRefId(parameterFundEventID);

			}
			catch 
			{
				throw;
			}
		}

		public static DataSet GetYMCAId(string parameterPersId) 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetYMCAId(parameterPersId);
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet GetYMCAInformation(string parameterYMCAID) 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetYMCAInformation(parameterYMCAID);
			}
			catch 
			{
				throw;
			}
		}
		public static string GetPersonLatestYmcaId(string parameterPersID) 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetPersonLatestYmcaId(parameterPersID); 
			}
			catch(Exception ex) 
			{
				throw ex;
			}
		}
		//Added by Ashish 27-Jan-2009 Start
		public static int ValidateQDRORequestPending(string parameterFundEventID) 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.ValidateQDRORequestPending(parameterFundEventID);

			}
			catch 
			{
				throw;
			}
		}
		//Added by Ashish 27-Jan-2009 End
		//Added by Ashish 17-Mar-2009 for issue YRS 5.0-679 ,Start

		//Added by Ashish 17-Mar-2009,  Start
		public static int GetReAmortizeLoanNumber(int paraOrgiLoanNumber) 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetReAmortizeLoanNumber(paraOrgiLoanNumber);
			}
			catch 
			{
				throw;
			}
		}
		public static int GetReAmortizeReason(string paraYmcaID) 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetReAmortizeReason(paraYmcaID);
			}
			catch 
			{
				throw;
			}
		}
		//Added by Ashish 17-Mar-2009,  End
		//Added by Ashish 17-Mar-2009 for issue YRS 5.0-679 ,End
		public class LoanInformationWrapperClass 
		{
			string [] m_string_AnnuityBasisType ;
			double l_double_RequestedAmount;
			//DataRow l_dr_LoanFeeTransaction;
			DataTable l_dt_LoanTransactions;
			DataTable l_dt_LoanAcctBreakdown;
			DataTable l_dt_LoanFeeTransactions;
			public string[] AnnuityBasisType 
			{   
				get 
				{
					return m_string_AnnuityBasisType;
				}
				set 
				{
					m_string_AnnuityBasisType= value;
				}
			}
		
			public double RequestedAmount 
			{   
				get 
				{
					return l_double_RequestedAmount;
				}
				set 
				{
					l_double_RequestedAmount= value;
				}
			}
			public DataTable LoanTransactions 
			{   
				get 
				{
					return l_dt_LoanTransactions;
				}
				set 
				{
					l_dt_LoanTransactions = value;
				}
			}
			public DataTable LoanFeeTransactions 
			{   
				get 
				{
					return l_dt_LoanFeeTransactions;
				}
				set 
				{
					l_dt_LoanFeeTransactions = value;
				}
			}
			public DataTable LoanBreakdown 
			{   
				get 
				{
					return l_dt_LoanAcctBreakdown;
				}
				set 
				{
					l_dt_LoanAcctBreakdown = value;
				}
			}
			public LoanInformationWrapperClass() 
			{

			}
			# region "YMCA Phase 4"
			private  void CreateAnnuityBasisTypes() 
			{ 
				DataTable l_AnnuityBasisTypeDataTable; 
				int l_Counter; 
				string [] l_AnnuityBasisType=null; 
				//				l_AnnuityBasisType=new string []{"","",""};
				try 
				{ 
					l_AnnuityBasisTypeDataTable = YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.LookUpAnnuityBasisTypes(); 
					if (l_AnnuityBasisTypeDataTable != null) 
					{ 
						l_AnnuityBasisType=new string [l_AnnuityBasisTypeDataTable.Rows.Count];
						l_Counter = 0; 
						// TODO: NotImplemented statement: ICSharpCode.SharpRefactory.Parser.AST.VB.ReDimStatement 
						foreach (DataRow l_DataRow in l_AnnuityBasisTypeDataTable.Rows) 
						{ 
							if (l_DataRow["AnnuityBasisType"].GetType().ToString().Trim() != "System.DBNull") 
							{ 
								l_AnnuityBasisType[l_Counter] = Convert.ToString(l_DataRow["AnnuityBasisType"]); 
							} 
							l_Counter += 1; 
						} 
					} 
					this.AnnuityBasisType = l_AnnuityBasisType; 
				} 
				catch 
				{ 
					throw; 
				} 
			}
			
			public  bool ProrateLoanAmount(string parameterFundEventID,double parameterAmount,string parameterPersId) 
			{
				DataSet l_ds_TDRTBalance = new DataSet();
				DataTable l_dt_CalculatedTDRTBalance = new DataTable();
				DataTable l_dt_CalculatedRTBalance = new DataTable();
				DataTable l_dt_PrepareTableforTransacts = new DataTable();
				DataTable l_dt_PrepareTableforLoanBreakdown = new DataTable();
                DataTable l_dt_GetLoanReammortzationMinThreshold; // Manthan Rajguru | 2016.01.11 | YRS-AT-2578 | Datatable created to store data from AtsMetaConfiguration table
				DataColumn dc ;
				DataRow l_dr_PrepareTableforTransacts;
							
				double l_double_RequestedAmount = parameterAmount;
				double l_double_RequestAmount_Left ;
				double l_double_TotalAmount ;
				double l_double_PercUsed = 0 ;
                //Ashish 2011.11.07 YRS 5.0-1322
                double l_double_TotalPreTaxAmount;
                double l_double_TotalPostTaxAmount;
                double dblLoanReammortzationMinThreshold; // Manthan Rajguru | 2016.01.11 | YRS-AT-2578 | Variable declared to store minimum threshold value.			    
				
				try 
				{
					CreateAnnuityBasisTypes();
					l_ds_TDRTBalance = YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetTDRTBalance(parameterFundEventID);

					if(l_ds_TDRTBalance.Tables[0] != null) 
					{
						l_dt_CalculatedTDRTBalance = l_ds_TDRTBalance.Tables[0];
					}
					
					if(l_ds_TDRTBalance.Tables.Count > 1) 
					{
						if(l_ds_TDRTBalance.Tables[1]!= null) 
						{
							l_dt_CalculatedRTBalance = l_ds_TDRTBalance.Tables[1];
						}
					}
					
					 
					//							//firstly we need to deduct the fee amount 

					l_dt_CalculatedTDRTBalance = PrepareTableforLoanFees(l_dt_CalculatedTDRTBalance,parameterPersId,parameterFundEventID,l_dt_CalculatedRTBalance);
                    //ASHISH:2011.11.07 YRS 5.0-1322
                    //l_double_TotalAmount = Convert.ToDouble(l_dt_CalculatedTDRTBalance.Compute("SUM(PersonalPreTax)",""));
                    l_double_TotalAmount = Convert.ToDouble(l_dt_CalculatedTDRTBalance.Compute("SUM(PersonalPreTax)+SUM(PersonalPostTax)", ""));
                    l_double_TotalPreTaxAmount = Convert.ToDouble(l_dt_CalculatedTDRTBalance.Compute("SUM(PersonalPreTax)", ""));
                    l_double_TotalPostTaxAmount = Convert.ToDouble(l_dt_CalculatedTDRTBalance.Compute("SUM(PersonalPostTax)", ""));
					if(l_double_TotalAmount != 0) 
					{
						if(l_double_TotalAmount < l_double_RequestedAmount) 
						{
							l_double_RequestedAmount = l_double_TotalAmount;
							//todo break ;
						}
						l_double_RequestAmount_Left = l_double_RequestedAmount;

						dc = new DataColumn("PersId", System.Type.GetType("System.String"));
						l_dt_PrepareTableforTransacts.Columns.Add(dc);
						dc = new DataColumn("FundId", System.Type.GetType("System.String"));
						l_dt_PrepareTableforTransacts.Columns.Add(dc);
						dc = new DataColumn("AccountType", System.Type.GetType("System.String"));
						l_dt_PrepareTableforTransacts.Columns.Add(dc);
						dc = new DataColumn("TransactType", System.Type.GetType("System.String"));
						l_dt_PrepareTableforTransacts.Columns.Add(dc);
						dc = new DataColumn("AnnuityBasisType", System.Type.GetType("System.String"));
						l_dt_PrepareTableforTransacts.Columns.Add(dc);
                        //ASHISH:2011.11.07 YRS 5.0-1322 commentd
                        //dc = new DataColumn("LoanAmount", System.Type.GetType("System.Double"));
                        //l_dt_PrepareTableforTransacts.Columns.Add(dc);
						dc = new DataColumn("Percentage", System.Type.GetType("System.Double"));
						l_dt_PrepareTableforTransacts.Columns.Add(dc);
						dc = new DataColumn("TransactionRefId", System.Type.GetType("System.String"));
						l_dt_PrepareTableforTransacts.Columns.Add(dc);
						dc = new DataColumn("AcctBalance", System.Type.GetType("System.Double"));
						l_dt_PrepareTableforTransacts.Columns.Add(dc);
				        //ASHISH:2011.11.07 YRS 5.0-1322                     
                        dc = new DataColumn("PreTaxLoanAmount", System.Type.GetType("System.Double"));
                        l_dt_PrepareTableforTransacts.Columns.Add(dc);
                        dc = new DataColumn("PostTaxLoanAmount", System.Type.GetType("System.Double"));
                        l_dt_PrepareTableforTransacts.Columns.Add(dc);
              
						foreach(DataRow dr in l_dt_CalculatedTDRTBalance.Rows) 
						{
							//if(Convert.ToDouble(dr["PersonalPreTax"])!=0) 
                            if (Convert.ToDouble(dr["PersonalPreTax"]) != 0 || Convert.ToDouble(dr["PersonalPostTax"]) != 0)
							{
								l_dr_PrepareTableforTransacts =  l_dt_PrepareTableforTransacts.NewRow();
								l_dr_PrepareTableforTransacts["PersId"] = parameterPersId;
								l_dr_PrepareTableforTransacts["FundId"] = parameterFundEventID;
								l_dr_PrepareTableforTransacts["AccountType"] = Convert.ToString(dr["AccountType"]);
								if(Convert.ToString(dr["MoneyType"])== "PR") 
								{
									l_dr_PrepareTableforTransacts["TransactType"] = "LWPR";
								}
								else if(Convert.ToString(dr["MoneyType"])== "IN") 
								{
									l_dr_PrepareTableforTransacts["TransactType"] = "LWIN";
								}
								l_dr_PrepareTableforTransacts["AnnuityBasisType"] = Convert.ToString(dr["AnnuityBasisType"]);
                                //ASHISH:2011.11.07 YRS 5.0-1322
								//l_dr_PrepareTableforTransacts["AcctBalance"] = Convert.ToDouble(dr["PersonalPreTax"]);
                                //l_dr_PrepareTableforTransacts["LoanAmount"] = Math.Round(((Convert.ToDouble(dr["PersonalPreTax"]) / l_double_TotalAmount) * l_double_RequestedAmount), 2);
                                l_dr_PrepareTableforTransacts["AcctBalance"] = Convert.ToDouble(dr["PersonalPreTax"]) + Convert.ToDouble(dr["PersonalPostTax"]);
                                //START: PPP | 01/27/2017 | YRS-AT-3123 | It calculates Loan partion of individual account against raw calculated % instead of rounding off it to 4 decimal which is being saved in LoanBreakDown table
                                //l_dr_PrepareTableforTransacts["PostTaxLoanAmount"] = Math.Round(((Convert.ToDouble(dr["PersonalPostTax"]) / l_double_TotalAmount) * l_double_RequestedAmount), 2);
                                //l_dr_PrepareTableforTransacts["PreTaxLoanAmount"] = Math.Round(((Convert.ToDouble(dr["PersonalPreTax"]) / l_double_TotalAmount) * l_double_RequestedAmount), 2);
                                l_dr_PrepareTableforTransacts["PostTaxLoanAmount"] = Math.Round((Math.Round((Convert.ToDouble(dr["PersonalPostTax"]) / l_double_TotalAmount), 4) * l_double_RequestedAmount), 2);
                                l_dr_PrepareTableforTransacts["PreTaxLoanAmount"] = Math.Round((Math.Round((Convert.ToDouble(dr["PersonalPreTax"]) / l_double_TotalAmount), 4) * l_double_RequestedAmount), 2);
                                //END: PPP | 01/27/2017 | YRS-AT-3123 | It calculates Loan partion of individual account against raw calculated % instead of rounding off it to 4 decimal which is being saved in LoanBreakDown table

								
                                l_dr_PrepareTableforTransacts["Percentage"] = 0;
								l_dr_PrepareTableforTransacts["TransactionRefId"] = Convert.ToString(dr["TransactionRefId"]);
                                l_dt_PrepareTableforTransacts.Rows.Add(l_dr_PrepareTableforTransacts);
							}
						}

						l_dt_PrepareTableforTransacts.AcceptChanges();
						this.LoanTransactions = l_dt_PrepareTableforTransacts;
						dc = new DataColumn("AccountType", System.Type.GetType("System.String"));
						l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
						dc = new DataColumn("AnnuityBasisType", System.Type.GetType("System.String"));
						l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
						dc = new DataColumn("TransactionRefId", System.Type.GetType("System.String"));
						l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
						dc = new DataColumn("AcctBalance", System.Type.GetType("System.Double"));
                        //ASHISH:2011.11.07 YRS 5.0-1322
                        //l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
                        //dc = new DataColumn("LoanAmount", System.Type.GetType("System.Double"));
						l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
						dc = new DataColumn("Percentage", System.Type.GetType("System.Double"));
						l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
                        //ASHISH:2011.11.07 YRS 5.0-1322
                        dc = new DataColumn("PersPreTaxPct", System.Type.GetType("System.Double"));
                        l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
                        dc = new DataColumn("PersPostTaxPct", System.Type.GetType("System.Double"));
                        l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
                        dc = new DataColumn("PreTaxLoanAmount", System.Type.GetType("System.Double"));
                        l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
                        dc = new DataColumn("PostTaxLoanAmount", System.Type.GetType("System.Double"));
                        l_dt_PrepareTableforLoanBreakdown.Columns.Add(dc);
						this.LoanBreakdown = l_dt_PrepareTableforLoanBreakdown  ;

						this.PrepareLoanAccountBreakdownTable();
						l_dt_PrepareTableforLoanBreakdown = this.LoanBreakdown ;
						if(l_dt_PrepareTableforLoanBreakdown != null) 
						{
                            //Start -- Manthan Rajguru | 2016.01.11 | YRS-AT-2578 |  If minimum threshold value is not defined or has null value then default 0.1 value will be taken.
                            l_dt_GetLoanReammortzationMinThreshold = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("LOAN_REAMRT_MIN_ACT_THRESHOLD");
                            if (l_dt_GetLoanReammortzationMinThreshold != null && l_dt_GetLoanReammortzationMinThreshold.Rows != null && l_dt_GetLoanReammortzationMinThreshold.Rows.Count > 0)
                            {
                                dblLoanReammortzationMinThreshold = Convert.ToDouble(l_dt_GetLoanReammortzationMinThreshold.Rows[0]["Value"]);
                            }
                            else
                            {
                                dblLoanReammortzationMinThreshold = 0.1; //Manthan Rajguru | 2016.01.11 | YRS-AT-2578 | Default value.
                            }
                            //End -- Manthan Rajguru | 2016.01.11 | YRS-AT-2578 |  If minimum threshold value is not defined or has null value then default 0.1 value will be taken.

                            //Start -- PPP | 2016.01.27 | YRS-AT-2578 | Stop entry into transacts table if loan amt less than minimum threshold value as per Accttype
                            DataRow[] deletedRow, deletedTransactsRows;
                            deletedRow = l_dt_PrepareTableforLoanBreakdown.Select(string.Format("(PostTaxLoanAmount + PreTaxLoanAmount) < {0}", dblLoanReammortzationMinThreshold.ToString()));
                            if (deletedRow != null && deletedRow.Length > 0)
                            {
                                foreach (DataRow row in deletedRow)
                                {
                                    //START: PPP | 2016.07.12 | YRS-AT-2578 | Adding TransactionRefId also in search box to delete only low balanced RT account 
                                    // deletedTransactsRows = l_dt_PrepareTableforTransacts.Select(string.Format("AnnuityBasisType='{0}' and AccountType='{1}'", row["AnnuityBasisType"], row["AccountType"]));
                                    deletedTransactsRows = l_dt_PrepareTableforTransacts.Select(string.Format("AnnuityBasisType='{0}' and AccountType='{1}' and TransactionRefId='{2}'", row["AnnuityBasisType"], row["AccountType"], row["TransactionRefId"]));
                                    //END: PPP | 2016.07.12 | YRS-AT-2578 | Adding TransactionRefId also in search box to delete only low balanced RT account 
                                    if (deletedTransactsRows != null && deletedTransactsRows.Length > 0)
                                    {
                                        foreach (DataRow transactsRow in deletedTransactsRows)
                                        {
                                            transactsRow.Delete();
                                        }
                                    }
                                    row.Delete();
                                }
                                l_dt_PrepareTableforTransacts.AcceptChanges();
                                l_dt_PrepareTableforLoanBreakdown.AcceptChanges();
                                this.LoanTransactions = l_dt_PrepareTableforTransacts;
                                this.LoanBreakdown = l_dt_PrepareTableforLoanBreakdown;
                            }
                            //End -- PPP | 2016.01.27 | YRS-AT-2578 | Stop entry into transacts table if loan amt less than minimum threshold value as per Accttype

							foreach(DataRow dr in l_dt_PrepareTableforLoanBreakdown.Rows) 
							{
                                //ASHISH:2011.11.07 YRS 5.0-1322
                                //dr["Percentage"] = Math.Round((Convert.ToDouble(dr["LoanAmount"])/parameterAmount),4);
                                dr["Percentage"] = Math.Round(((Convert.ToDouble(dr["PostTaxLoanAmount"]) + Convert.ToDouble(dr["PreTaxLoanAmount"])) / parameterAmount), 4);
                              
                                dr["PersPostTaxPct"] = Math.Round((Convert.ToDouble(dr["PostTaxLoanAmount"]) /(Convert.ToDouble(dr["PostTaxLoanAmount"])+Convert.ToDouble(dr["PreTaxLoanAmount"])) ), 13);
                                dr["PersPreTaxPct"] = Math.Round((1-Convert.ToDouble(dr["PersPostTaxPct"])), 13);

							}
                           
                            
							l_double_PercUsed  = Math.Round(Convert.ToDouble(l_dt_PrepareTableforLoanBreakdown.Compute("SUM(Percentage)","")),4);
                            //
							//l_double_RequestAmount_Left = Math.Round(Convert.ToDouble(l_dt_PrepareTableforLoanBreakdown.Compute("SUM(LoanAmount)","")),2);
                            l_double_RequestAmount_Left = Math.Round(Convert.ToDouble(l_dt_PrepareTableforLoanBreakdown.Compute("SUM(PreTaxLoanAmount)+SUM(PostTaxLoanAmount)", "")), 2);

							if((l_double_PercUsed !=1) ||(l_double_RequestAmount_Left != parameterAmount)) 
							{
								if(l_double_RequestAmount_Left < parameterAmount) 
								{
                                    //ASHISH:2011.11.07 YRS 5.0-1322
                                    //l_dt_PrepareTableforTransacts.Rows[0]["LoanAmount"]= Math.Round((Convert.ToDouble(l_dt_PrepareTableforTransacts.Rows[0]["LoanAmount"])+(parameterAmount-l_double_RequestAmount_Left)),2);
                                    l_dt_PrepareTableforTransacts.Rows[0]["PreTaxLoanAmount"] = Math.Round((Convert.ToDouble(l_dt_PrepareTableforTransacts.Rows[0]["PreTaxLoanAmount"]) + (parameterAmount - l_double_RequestAmount_Left)), 2);
								}
								if(l_double_RequestAmount_Left > parameterAmount) 
								{
                                    //ASHISH:2011.11.07 YRS 5.0-1322
                                    //l_dt_PrepareTableforTransacts.Rows[0]["LoanAmount"]= Math.Round((Convert.ToDouble(l_dt_PrepareTableforTransacts.Rows[0]["LoanAmount"])-(l_double_RequestAmount_Left-parameterAmount)),2);
                                    l_dt_PrepareTableforTransacts.Rows[0]["PreTaxLoanAmount"] = Math.Round((Convert.ToDouble(l_dt_PrepareTableforTransacts.Rows[0]["PreTaxLoanAmount"]) - (l_double_RequestAmount_Left - parameterAmount)), 2);
								}

								if( l_double_PercUsed < 1) 
								{
									l_dt_PrepareTableforTransacts.Rows[0]["Percentage"] = 
										Math.Round(Convert.ToDouble(l_dt_PrepareTableforTransacts.Rows[0]["Percentage"]) + Math.Round((1-l_double_PercUsed),4),4);
								}
								if( l_double_PercUsed > 1) 
								{
									l_dt_PrepareTableforTransacts.Rows[0]["Percentage"] = Math.Round((Convert.ToDouble(l_dt_PrepareTableforTransacts.Rows[0]["Percentage"]) -(l_double_PercUsed-1)),4);
								}
								foreach(DataRow dr in l_dt_PrepareTableforLoanBreakdown.Rows) 
								{
                                    //ASHISH:2011.11.07 YRS 5.0-1322
                                    //dr["LoanAmount"] = 0;
                                    //dr["AcctBalance"] = 0;
                                    dr["PreTaxLoanAmount"] = 0;
                                    dr["PostTaxLoanAmount"] = 0;
                                    dr["AcctBalance"] = 0;
								}
								this.PrepareLoanAccountBreakdownTable();
                                //ASHISH:2011.11.07 YRS 5.0-1322
                                foreach (DataRow dr in l_dt_PrepareTableforLoanBreakdown.Rows)
                                {
                                    dr["PersPostTaxPct"] = Math.Round((Convert.ToDouble(dr["PostTaxLoanAmount"]) / (Convert.ToDouble(dr["PostTaxLoanAmount"]) + Convert.ToDouble(dr["PreTaxLoanAmount"]))), 13);
                                    dr["PersPreTaxPct"] = Math.Round((1 - Convert.ToDouble(dr["PersPostTaxPct"])), 13);

                                }
							}
							else 
							{
								l_dt_PrepareTableforLoanBreakdown.AcceptChanges();
								this.LoanBreakdown = l_dt_PrepareTableforLoanBreakdown ;

							}
						}
						return true;
					}
					else 
					{
						return false;
					}
					
				

				}
				catch 
				{
					throw;
				}
			}

			public static double GetLoanFees() 
			{
				double l_double_LoanFeeAmount = 0;
				DataTable l_dt_GetLoanFees;
				try 
				{
					l_dt_GetLoanFees = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("TDLOANS_PROCESSING FEE");
					l_double_LoanFeeAmount = Convert.ToDouble(l_dt_GetLoanFees.Rows[0]["Value"]);
					if(l_double_LoanFeeAmount == 0 ) 
					{
						l_double_LoanFeeAmount = 50;
					}
					return l_double_LoanFeeAmount; 
				}
				catch 
				{
					throw;
				}
			}
			//
			public  DataTable PrepareTableforLoanFees(DataTable parameterDataTable,string parameterPersId,string parameterFundEventID,DataTable parameterRTBalance) 
			{
				//1.The order by which the fee will be removed from the account(s) will be as follows:  
				//a.pre-96 TD principal b.pre-96 TD interest c.post-96 TD principal d.post-96 TD interest
				//e.RT principal f.RT interest.
				//The money will be withdrawn from those sources in that order until the $50 amount has 
				//been achieved.  If this order of deductions results in a deduction from an RT account, 
				//the process will deduct the fee from the RT account with the highest balance. 

				// ----------------------------------
				// Added by DY : 2009.09.02 : To implement n-tier logic : Now order of fee deduction is extended.
				// like PRE96-PR, .....................
//				string l_QueryString ;
				DataColumn dc ;
				double l_double_LoanFeeAmount = 0;
				
				int i = 0;
				DataTable l_dt_PrepareTableforLoanFees = new DataTable();
				DataRow l_dr_UpdateTableForTransacts;
				DataRow l_dr_PrepareTableforLoanFees;
//				DataRow[] dr_Fee;
//				DataRow[] dr_Roll;
//				DataRow[] dr_CheckTransactionID;
				DataRow[] dr_annuityGroup;
				DataRow[] dr_RTannuityGroup;
				try 
				{
					l_double_LoanFeeAmount = GetLoanFees();
					this.RequestedAmount = l_double_LoanFeeAmount;
					        #region 'DKY : pre-post code'
					if(parameterDataTable != null)
					{
						l_dt_PrepareTableforLoanFees = parameterDataTable.Clone();
     					// start : 2009.09.03 : Dilip Yadav
//						if ( parameterDataTable != null  )						
//						{
							//PRE - POST
							dr_annuityGroup = parameterDataTable.Select("AnnuityBasisGroup = 'PRE' OR AnnuityBasisGroup = 'PST' ");							
							if(dr_annuityGroup.Length > 0)
							{
									for(i=0;i< dr_annuityGroup.Length;i++)
									{
										// To check if still Fee existed or not
										if(this.RequestedAmount > 0)
										{
											string l_AnnuityBasisType = string.Empty ;
											string l_MoneyType = string.Empty;
											string l_AccountType = string.Empty;

											l_AnnuityBasisType = dr_annuityGroup[i]["AnnuityBasisType"].ToString();
											l_MoneyType = dr_annuityGroup[i]["MoneyType"].ToString();
											l_AccountType = dr_annuityGroup[i]["AccountType"].ToString();
											l_dr_UpdateTableForTransacts = GetDataRow(l_AnnuityBasisType, l_MoneyType, l_AccountType,parameterDataTable, Convert.ToString(dr_annuityGroup[i]["TransactionRefID"]));
											l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
									
											if(l_MoneyType == "PR")
												this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_annuityGroup[i],l_dr_UpdateTableForTransacts,"LFPR",this.RequestedAmount);
											else
												this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_annuityGroup[i],l_dr_UpdateTableForTransacts,"LFIN",this.RequestedAmount);

											l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
											parameterDataTable.AcceptChanges();						
										}
										else
										{
											break;
										}
									}
							  }		//YRS 5.0-1007 : LFPR Transactions not created during th loan process with RT money only Uncomment by priya 1007
								#endregion 'DKY : pre-post code'

							#region 'DKY-ROL'
							//ROL
							if (parameterRTBalance != null  )						
							{
								dr_RTannuityGroup = parameterRTBalance.Select("AnnuityBasisGroup = 'ROL' ");							
								if(dr_RTannuityGroup.Length > 0)
								{
									for(i=0; i<dr_RTannuityGroup.Length; i++)
									{
										if(this.RequestedAmount > 0)
										{
											string l_AnnuityBasisType = string.Empty ;
											string l_MoneyType = string.Empty;
											string l_AccountType = string.Empty;

											l_AnnuityBasisType = dr_RTannuityGroup[i]["AnnuityBasisType"].ToString();
											l_MoneyType = dr_RTannuityGroup[i]["MoneyType"].ToString();
											l_AccountType = dr_RTannuityGroup[i]["AccountType"].ToString();
											l_dr_UpdateTableForTransacts = GetDataRow(l_AnnuityBasisType, l_MoneyType, l_AccountType,parameterRTBalance, Convert.ToString(dr_RTannuityGroup[i]["TransactionRefID"]));
											l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
									
											if(l_MoneyType == "PR")
												this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_RTannuityGroup[i],l_dr_UpdateTableForTransacts,"LFPR",this.RequestedAmount);
											else
												this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_RTannuityGroup[i],l_dr_UpdateTableForTransacts,"LFIN",this.RequestedAmount);

											l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
											parameterDataTable.AcceptChanges();						
										}
										else
										{
											break;
										}
									}
								}
							}
       					}     
						#endregion 
                      				
//						// END : 2009.09.03 : Dilip Yadav	
						// -------------------------------------------------------------

						#region 'DKY : Commented by Dilip yadav : 14-Sep-09 to implement the logic of n-tier annuity'
//						//1
//						dr_Fee = parameterDataTable.Select("AnnuityBasisType = 'PRE96' AND MoneyType = 'PR' AND AccountType = 'TD'"); 	
//						if(dr_Fee.Length > 0)
//						{
//							for(i=0;i< dr_Fee.Length;i++)
//							{
//								l_dr_UpdateTableForTransacts = GetDataRow("PRE96","PR","TD",parameterDataTable,Convert.ToString(dr_Fee[i]["TransactionRefID"]));
//								l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
//								this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_Fee[i],l_dr_UpdateTableForTransacts,"LFPR",this.RequestedAmount);
//								l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
//								parameterDataTable.AcceptChanges();						
//							}
//						}
//
//						//2
//						if(this.RequestedAmount > 0 )
//						{
//							dr_Fee = parameterDataTable.Select("AnnuityBasisType = 'PRE96' AND MoneyType = 'IN' AND AccountType = 'TD'"); 	
//							if(dr_Fee.Length > 0)
//							{
//								for(i=0;i< dr_Fee.Length;i++)
//								{
//									l_dr_UpdateTableForTransacts = GetDataRow("PRE96","IN","TD",parameterDataTable,Convert.ToString(dr_Fee[i]["TransactionRefID"]));
//									l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
//									this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_Fee[i],l_dr_UpdateTableForTransacts,"LFIN",this.RequestedAmount);
//									l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
//									parameterDataTable.AcceptChanges();							
//								}
//							}
//						}
//
//						//3
//						if(this.RequestedAmount > 0 )
//						{
//							dr_Fee = parameterDataTable.Select("AnnuityBasisType = 'PST96' AND MoneyType = 'PR' AND AccountType = 'TD'"); 	
//							if(dr_Fee.Length > 0)
//							{
//								for(i=0;i< dr_Fee.Length;i++)
//								{
//									l_dr_UpdateTableForTransacts = GetDataRow("PST96","PR","TD",parameterDataTable,Convert.ToString(dr_Fee[i]["TransactionRefID"]));
//									l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
//									this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_Fee[i],l_dr_UpdateTableForTransacts,"LFPR",this.RequestedAmount);
//									l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
//									parameterDataTable.AcceptChanges();							
//								}
//							}
//						}
//
//						//4
//						if(this.RequestedAmount > 0 )
//						{
//							dr_Fee = parameterDataTable.Select("AnnuityBasisType = 'PST96' AND MoneyType = 'IN' AND AccountType = 'TD'"); 	
//							if(dr_Fee.Length > 0)
//							{
//								for(i=0;i< dr_Fee.Length;i++)
//								{
//									l_dr_UpdateTableForTransacts = GetDataRow("PST96","IN","TD",parameterDataTable,Convert.ToString(dr_Fee[i]["TransactionRefID"]));
//									l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
//									this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_Fee[i],l_dr_UpdateTableForTransacts,"LFIN",this.RequestedAmount);
//									l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
//									parameterDataTable.AcceptChanges();							
//								}
//							}
//						}
						#endregion 'DKY : Commented by Dilip yadav : 14-Sep-09 to implement the logic of n-tier annuity'
	
	                  	#region "DKY: Commented by Dilip yadav : 05-Oct-09 to implement the logic of n-tier annuity"

						//calculation for RT begins here
//						if(parameterRTBalance != null) 
//						{
//							if(parameterRTBalance.Rows.Count > 0 ) 
//							{
//								for(int j=0;j<parameterRTBalance.Rows.Count;j++)
//								{
//								NewTransactionRefId :
//									if(j< parameterRTBalance.Rows.Count)
//									{
//										if(this.RequestedAmount > 0 ) 
//										{   
//											l_QueryString = "AccountType = 'RT' AND TransactionRefID = '" + Convert.ToString(parameterRTBalance.Rows[j]["TransactionRefID"]) + "'";
//											if(l_dt_PrepareTableforLoanFees != null) 
//											{
//												dr_CheckTransactionID = l_dt_PrepareTableforLoanFees.Select(l_QueryString);
//												if(dr_CheckTransactionID.Length> 0){j++; goto NewTransactionRefId;};
//											}
//											dr_Roll = parameterDataTable.Select(l_QueryString); 	
//											if(dr_Roll.Length > 0) 
//											{
//												l_QueryString = "AccountType = 'RT' AND TransactionRefID = '" + Convert.ToString(parameterRTBalance.Rows[j]["TransactionRefID"]) + "' AND MoneyType = 'PR'";
//												dr_Roll = parameterDataTable.Select(l_QueryString);
//																				
//												if(dr_Roll.Length > 0) 
//												{
//													for(int k=0; k< dr_Roll.Length;k++) 
//													{
//														if(this.RequestedAmount > 0) 
//														{
//															if(Convert.ToDouble(dr_Roll[k]["PersonalPreTax"])!= 0) 
//															{
//																l_dr_UpdateTableForTransacts = GetDataRow(Convert.ToString(dr_Roll[k]["AnnuityBasisType"]),"PR","RT",parameterDataTable,Convert.ToString(dr_Roll[k]["TransactionRefID"]));
//																l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
//																this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_Roll[k],l_dr_UpdateTableForTransacts,"LFPR",this.RequestedAmount);
//																l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
//																parameterDataTable.AcceptChanges();
//															}														 
//														}
//													}
//												}													 
//											}
//											if(this.RequestedAmount > 0 ) 
//											{
//												l_QueryString = "AccountType = 'RT' AND TransactionRefID = '" + Convert.ToString(parameterRTBalance.Rows[j]["TransactionRefID"]) + "' AND MoneyType = 'IN'";
//												dr_Roll = parameterDataTable.Select(l_QueryString); 
//												if(dr_Roll.Length > 0 ) 
//												{
//													for(int k=0; k< dr_Roll.Length;k++) 
//													{
//														if(Convert.ToDouble(dr_Roll[k]["PersonalPreTax"])!= 0) 
//														{
//															l_dr_UpdateTableForTransacts = GetDataRow(Convert.ToString(dr_Roll[k]["AnnuityBasisType"]),"IN","RT",parameterDataTable,Convert.ToString(dr_Roll[k]["TransactionRefID"]));
//															l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
//															this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_Roll[k],l_dr_UpdateTableForTransacts,"LFIN",this.RequestedAmount);
//															l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
//															parameterDataTable.AcceptChanges();
//														}
//													}
//												}
//											}									 
//										}
//									}
//								}
//							}						
//						}
												#endregion
						//}	//YRS 5.0-1007 : LFPR Transactions not created during th loan process with RT money only Commented by priya 1007 
												
					#region 'DKY Old commented code'		
					//						if(this.RequestedAmount > 0 )
					//						{
					//							dr_Fee = parameterDataTable.Select("AnnuityBasisType = 'ROLL' AND MoneyType = 'PR'"); 	
					//							if(dr_Fee.Length > 0)
					//							{
					//								for(i=0;i< dr_Fee.Length;i++)
					//								{
					//									l_dr_UpdateTableForTransacts = GetDataRow("ROLL","PR",Convert.ToString(dr_Fee[i]["AccountType"]),parameterDataTable,Convert.ToString(dr_Fee[i]["TransactionRefID"]));
					//									l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
					//									this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_Fee[i],l_dr_UpdateTableForTransacts,"LFPR",this.RequestedAmount);
					//									l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
					//									parameterDataTable.AcceptChanges();
					//							
					//								}
					//							}
					//						}
					//
					//						if(this.RequestedAmount > 0 )
					//						{
					//							dr_Fee = parameterDataTable.Select("AnnuityBasisType = 'ROLL' AND MoneyType = 'IN'"); 	
					//							if(dr_Fee.Length > 0)
					//							{
					//								for(i=0;i< dr_Fee.Length;i++)
					//								{
					//									l_dr_UpdateTableForTransacts = GetDataRow("ROLL","IN",Convert.ToString(dr_Fee[i]["AccountType"]),parameterDataTable,Convert.ToString(dr_Fee[i]["TransactionRefID"]));
					//									l_dr_PrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
					//									this.DoCalculationForLoanFeeDeduction(l_dr_PrepareTableforLoanFees,dr_Fee[i],l_dr_UpdateTableForTransacts,"LFIN",this.RequestedAmount);
					//									l_dt_PrepareTableforLoanFees.Rows.Add(l_dr_PrepareTableforLoanFees);
					//									parameterDataTable.AcceptChanges();
					//							
					//								}
					//							}
					//						}
					//					}
										#endregion 

					// we will add fund event id and pers id 
					dc = new DataColumn("PersId", System.Type.GetType("System.String"));
					l_dt_PrepareTableforLoanFees.Columns.Add(dc);
					dc = new DataColumn("FundId", System.Type.GetType("System.String"));
					l_dt_PrepareTableforLoanFees.Columns.Add(dc);
					foreach(DataRow dr in l_dt_PrepareTableforLoanFees.Rows) 
					{
						dr["PersId"] = parameterPersId;
						dr["FundId"] = parameterFundEventID;

					}
					l_dt_PrepareTableforLoanFees.AcceptChanges();
					this.LoanFeeTransactions = l_dt_PrepareTableforLoanFees;
					return parameterDataTable;
				}
				catch 
				{
					throw;
				}
			}
			public  DataRow GetDataRow(string parameterAnnuityBasisType,string parameterMoneyType,string parameterAcctType,DataTable parameterDataTable,string parameterTransactionRefID) 
			{
				string l_QueryString = "";
				DataRow[]  l_FoundRow ;

				try 
				{
					if (parameterTransactionRefID != "") 
					{
						l_QueryString = "MoneyType = '" + parameterMoneyType.Trim().ToUpper() + "' AND AnnuityBasisType = '" + parameterAnnuityBasisType + "' AND TransactionRefID  = '" + parameterTransactionRefID + "' AND AccountType = '" + parameterAcctType + "'";
					}
					else 
					{
						l_QueryString = "MoneyType = '" + parameterMoneyType.Trim().ToUpper() + "' AND AnnuityBasisType = '" + parameterAnnuityBasisType + "' AND AccountType = '" + parameterAcctType + "'";
					}
					
					l_FoundRow = parameterDataTable.Select(l_QueryString);
					if(l_FoundRow.Length > 0) 
					{
						return l_FoundRow[0];
					}
					else 
					{
						return null;
					}

				}
				catch 
				{
					throw;
				}
			}
			public  DataRow GetDataRow(string parameterAnnuityBasisType,string parameterAcctType,DataTable parameterDataTable,string parameterTransactionRefID) 
			{
				string l_QueryString = "";
				DataRow[]  l_FoundRow ;

				try 
				{
					if (parameterTransactionRefID != "") 
					{
						l_QueryString = " AnnuityBasisType = '" + parameterAnnuityBasisType + "' AND TransactionRefID  = '" + parameterTransactionRefID + "' AND AccountType = '" + parameterAcctType + "'";
					}
					else 
					{
						l_QueryString = " AnnuityBasisType = '" + parameterAnnuityBasisType + "' AND AccountType = '" + parameterAcctType + "'";
					}
					
					l_FoundRow = parameterDataTable.Select(l_QueryString);
					if(l_FoundRow.Length > 0) 
					{
						return l_FoundRow[0];
					}
					else 
					{
						return null;
					}

				}
				catch 
				{
					throw;
				}
			}
		
			public  DataRow GetDataRow(string parameterAcctType,string parameterTransactionRefId,DataTable parameterDataTable) 
			{
				string l_QueryString = "";
				DataRow[]  l_FoundRow ;

				try 
				{
					if (parameterTransactionRefId != "") 
					{
						l_QueryString = "AccountType = '" + parameterAcctType + "'AND TransactionRefID = '" + parameterTransactionRefId + "'";
					}
					else 
					{
						l_QueryString = "AccountType = '" + parameterAcctType + "'";
					}
					
					l_FoundRow = parameterDataTable.Select(l_QueryString);
					if(l_FoundRow.Length > 0) 
					{
						return l_FoundRow[0];
					}
					else 
					{
						return null;
					}

				}
				catch 
				{
					throw;
				}
			}
			public  void DoCalculationForLoanFeeDeduction(DataRow parameterPrepareTableforLoanFees,DataRow parameterLoanFee,DataRow parameterTransact,string parameterMoneyType,double parameterLoanFeeAmount) 
			{
			
				try 
				{
					//parameterPrepareTableforLoanFees = l_dt_PrepareTableforLoanFees.NewRow();
					parameterPrepareTableforLoanFees["AccountType"] = parameterLoanFee["AccountType"];
					parameterPrepareTableforLoanFees["MoneyType"] = parameterMoneyType;
					parameterPrepareTableforLoanFees["AnnuityBasisType"] = parameterLoanFee["AnnuityBasisType"];
					parameterPrepareTableforLoanFees["TransactionRefID"] = parameterLoanFee["TransactionRefID"];
					if(parameterLoanFeeAmount <= Convert.ToDouble(parameterLoanFee["PersonalPreTax"])) 
					{
						parameterPrepareTableforLoanFees["PersonalPreTax"] = parameterLoanFeeAmount;
						parameterTransact["PersonalPreTax"] = Math.Round((Convert.ToDouble(parameterLoanFee["PersonalPreTax"]) - parameterLoanFeeAmount),2);
						parameterLoanFeeAmount = 0;
					}
					else 
					{
						parameterPrepareTableforLoanFees["PersonalPreTax"] = Math.Round(Convert.ToDouble(parameterLoanFee["PersonalPreTax"]),2);
						parameterLoanFeeAmount = Math.Round((parameterLoanFeeAmount - Convert.ToDouble(parameterLoanFee["PersonalPreTax"])),2);
						parameterTransact["PersonalPreTax"] = 0;
						
					}
                    //ASHISH:2011.11.07-YRS 5.0-1322
                    // if fee amount yet not adjusted with personal pretax monet then adjusted with personal post tax money
                    if (Convert.ToDouble(parameterLoanFee["PersonalPostTax"]) > 0 && parameterLoanFeeAmount > 0)
                    {
                        if (parameterLoanFeeAmount <= Convert.ToDouble(parameterLoanFee["PersonalPostTax"]))
                        {
                            parameterPrepareTableforLoanFees["PersonalPostTax"] = parameterLoanFeeAmount;
                            parameterTransact["PersonalPostTax"] = Math.Round((Convert.ToDouble(parameterLoanFee["PersonalPostTax"]) - parameterLoanFeeAmount), 2);
                            parameterLoanFeeAmount = 0;
                        }
                        else
                        {
                            parameterPrepareTableforLoanFees["PersonalPostTax"] = Math.Round(Convert.ToDouble(parameterLoanFee["PersonalPostTax"]), 2);
                            parameterLoanFeeAmount = Math.Round((parameterLoanFeeAmount - Convert.ToDouble(parameterLoanFee["PersonalPostTax"])), 2);
                            parameterTransact["PersonalPostTax"] = 0;
                        }
                    }

					this.RequestedAmount = parameterLoanFeeAmount;
										

				}
				catch 
				{
					throw;
				}
			}
			public void PrepareLoanAccountBreakdownTable() 
			{
				string[] l_AnnuityBasisTypeArray;
				DataTable l_dt_PrepareTableforTransacts;
				DataTable l_dt_PrepareTableforLoanBreakdown;
				DataRow[] dr_Roll; 
				DataRow l_dr_PrepareTableforLoanBreakdown;
					
				int i = 0;
				try 
				{
					l_AnnuityBasisTypeArray = m_string_AnnuityBasisType;
					l_dt_PrepareTableforTransacts = this.LoanTransactions ;
					l_dt_PrepareTableforLoanBreakdown = this.LoanBreakdown ;
					if(l_dt_PrepareTableforTransacts != null) 
					{ 
						if(l_AnnuityBasisTypeArray != null) 
						{
							//1
							foreach (string l_AnnuityBasisType in l_AnnuityBasisTypeArray) 
							{ 
								//2
								if (l_AnnuityBasisType != null) 
								{ 
									//3
									dr_Roll = l_dt_PrepareTableforTransacts.Select("AnnuityBasisType = '" + l_AnnuityBasisType + "'");
									if(dr_Roll.Length > 0) 
									{
										//4
								
										for(i= 0; i< dr_Roll.Length; i++) 
										{
											//5
											l_dr_PrepareTableforLoanBreakdown = GetDataRow(Convert.ToString(dr_Roll[i]["AnnuityBasisType"]),Convert.ToString(dr_Roll[i]["AccountType"]),l_dt_PrepareTableforLoanBreakdown,Convert.ToString(dr_Roll[i]["TransactionRefID"]));
											if(l_dr_PrepareTableforLoanBreakdown!= null) 
											{
												l_dr_PrepareTableforLoanBreakdown["AcctBalance"] =  Math.Round((Convert.ToDouble(l_dr_PrepareTableforLoanBreakdown["AcctBalance"])+ Convert.ToDouble(dr_Roll[i]["AcctBalance"])),2);
                                                ////ASHISH:2011.11.07 YRS 5.0-1322
                                                //l_dr_PrepareTableforLoanBreakdown["LoanAmount"] = Math.Round((Convert.ToDouble(l_dr_PrepareTableforLoanBreakdown["LoanAmount"])+ Convert.ToDouble(dr_Roll[i]["LoanAmount"])),2);
                                                l_dr_PrepareTableforLoanBreakdown["PostTaxLoanAmount"] = Math.Round((Convert.ToDouble(l_dr_PrepareTableforLoanBreakdown["PostTaxLoanAmount"]) + Convert.ToDouble(dr_Roll[i]["PostTaxLoanAmount"])), 2);
                                                l_dr_PrepareTableforLoanBreakdown["PreTaxLoanAmount"] = Math.Round((Convert.ToDouble(l_dr_PrepareTableforLoanBreakdown["PreTaxLoanAmount"]) + Convert.ToDouble(dr_Roll[i]["PreTaxLoanAmount"])), 2);
												l_dr_PrepareTableforLoanBreakdown["Percentage"] = Math.Round((Convert.ToDouble(l_dr_PrepareTableforLoanBreakdown["Percentage"])+ Convert.ToDouble(dr_Roll[i]["Percentage"])),4);
											}
											else 
											{
												l_dr_PrepareTableforLoanBreakdown = l_dt_PrepareTableforLoanBreakdown.NewRow();
												l_dr_PrepareTableforLoanBreakdown["AccountType"] = Convert.ToString(dr_Roll[i]["AccountType"]);
												l_dr_PrepareTableforLoanBreakdown["AnnuityBasisType"] = Convert.ToString(dr_Roll[i]["AnnuityBasisType"]);
												l_dr_PrepareTableforLoanBreakdown["TransactionRefId"] = Convert.ToString(dr_Roll[i]["TransactionRefId"]);
												l_dr_PrepareTableforLoanBreakdown["AcctBalance"] = Convert.ToDouble(dr_Roll[i]["AcctBalance"]);
                                                //ASHISH:2011.11.07 YRS 5.0-1322
												//l_dr_PrepareTableforLoanBreakdown["LoanAmount"] = Convert.ToDouble(dr_Roll[i]["LoanAmount"]);
                                                l_dr_PrepareTableforLoanBreakdown["PreTaxLoanAmount"] = Convert.ToDouble(dr_Roll[i]["PreTaxLoanAmount"]);
                                                l_dr_PrepareTableforLoanBreakdown["PostTaxLoanAmount"] = Convert.ToDouble(dr_Roll[i]["PostTaxLoanAmount"]);
												l_dr_PrepareTableforLoanBreakdown["Percentage"] = Convert.ToDouble(dr_Roll[i]["Percentage"]);
												l_dt_PrepareTableforLoanBreakdown.Rows.Add(l_dr_PrepareTableforLoanBreakdown);
											}
									
										}//5
									}//4

								}//3
							}//2
						}//1
						l_dt_PrepareTableforLoanBreakdown.AcceptChanges();
						this.LoanBreakdown = l_dt_PrepareTableforLoanBreakdown;	
					}
				}
				catch 
				{
					throw;
				}
			}

            
            # endregion 
		}
		
		public static string GetYMCAEmailId(string parameterYmcaId) 
		{
			try 
			{
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetYMCAEmailId(parameterYmcaId);
			} 
			catch 
			{
				throw;
			}
		}
        //"code Added by-shagufta chaudhari '15-07-2011':BT-829"
		//SP 2014.01.15 BT-2314\YRS 5.0-2260 - YRS generating blank loan payoff letters 
		//Added New Parameter parameterGuiYMCAId
		public static int IsActiveYMCAEmployement(string parameterguiPersId, string parameterGuiYMCAId)
        {

            try
            {
				return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.IsActiveYMCAEmployement(parameterguiPersId, parameterGuiYMCAId);
            }

            catch
            {
                throw;
            }
        }
        //Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a method to update freezedate
        public static decimal[] GetOffsetAmount(int parameterLoanRequestId, string paramaterOffsetDate, string parameterLoanStatus, out string strEmpstatus, out string strAge)
        {

            try
            {
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetOffsetAmount(parameterLoanRequestId, paramaterOffsetDate, parameterLoanStatus, out strEmpstatus, out strAge);
            }
            catch
            {
                throw;
            }
        }

        public static bool IsLoanOffsetValid(int parameterLoanRequestId, out int intOffseteventReason)
        {

            try
            {
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.IsLoanOffsetValid(parameterLoanRequestId, out intOffseteventReason);
            }
            catch
            {
                throw;
            }
        }

        //AA:10.26.2015  YRS-AT-2533 : changed to get the default and offset dates
        //public static string GetOffsetDate(int parameterLoanRequestId,string parameterLoanStatus) 
        public static Dictionary<string, string> GetOffsetAndDefaultDates(int parameterLoanRequestId, string parameterLoanStatus) 
		{
			
			try 
			{
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetOffsetAndDefaultDates(parameterLoanRequestId, parameterLoanStatus); //AA:10.26.2015  YRS-AT-2533 : changed to get the default and offset dates
			}	
			catch 
			{
				throw;
			}
		}

        
        //End: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added a method to update freezedate		
	

        //Start: DInesh K 2015.04.08 BT:2699: YRS 5.0-2441 : Modifications for 403b Loans

        /// <summary>
        /// Get List of Loans (Ageing,Default, Offset and Freeze)
        /// </summary>
        /// <returns></returns>
        public static DataSet GetLoanDetailsForUtility()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanDetailsForUtility();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update Freeze Loan to UnFreeze
        /// </summary>
        /// <param name="strLoanRequestId">Requried Loand Details Id to Unfreeze Loand</param>
        /// <param name="strFreezeDate">Required to capture Freeze Date to Mark Loan as Freeze</param>
        /// <param name="decLoanFrozenAmount">Required to capture Frozen amount for Freezed Loans</param>
        //Start -- Manthan Rajguru | 2016.02.18 | YRS-AT-2603 | Added parameter to method to get first and last name
        //START: ML |2019.01.17| YRS-AT-3157 | Added new output variable(loanNo,paymentCode) ,which will be used to send email. Removed not required variables
        //public static void UpdateLoanFreezeUnFreeze(int intLoanDetailsId, string strFreezeDate, decimal decLoanFrozenAmount, out string strEmailAddrs, out string strThresholdDays, out string strFirstName, out string strLastName)        
        public static void UpdateLoanFreezeUnFreeze(int intLoanDetailsId, string strFreezeDate, decimal decLoanFrozenAmount, out string loanNo, out string paymentCode)
        //END: ML |2019.01.17| YRS-AT-3157 | Added new output variable(loanNo,paymentCode) ,which will be used to send email. Removed not required variables
        {
            try
            {
                //START: ML |2019.01.17| YRS-AT-3157 | Added new output variable(loanNo,paymentCode) ,which will be used to send email. Removed not required variables
                //YMCARET.YmcaDataAccessObject.LoanInformationDAClass.UpdateLoanFreezeUnFreeze(intLoanDetailsId, strFreezeDate, decLoanFrozenAmount, out strEmailAddrs, out strThresholdDays, out strFirstName, out strLastName); 
                YMCARET.YmcaDataAccessObject.LoanInformationDAClass.UpdateLoanFreezeUnFreeze(intLoanDetailsId, strFreezeDate, decLoanFrozenAmount, out loanNo, out paymentCode);
                //END: ML |2019.01.17| YRS-AT-3157 | Added new output variable(loanNo,paymentCode) ,which will be used to send email. Removed not required variables
            }
         //End -- Manthan Rajguru | 2016.02.18 | YRS-AT-2603 | Added parameter to method to get first and last name
            catch
            {
                throw;
            }
        }

        //End: DInesh K 2015.04.08 BT:2699: YRS 5.0-2441 : Modifications for 403b Loans

        //START: MMR | 2018.03.04 | YRS-AT-3929 | Get loan information for email content which will be sent to participant if payment mode is EFT
        /// <summary>
        /// This method will return Loan information for email content
        /// </summary>
        /// <param name="loanRequestId">Loan Request ID</param>
        /// <returns>Loan Information Table</returns>
        public static DataTable GetLoanInformationForMail(int loanRequestId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanInformationForMail(loanRequestId);
            }
            catch
            {
                throw;
            }
        }
        //END: MMR | 2018.03.04 | YRS-AT-3929 | Get loan information for email content which will be sent to participant if payment mode is EFT

        //START :VC | 2018.06.21 | YRS-AT-3190 |  To check whether is loan is recently reamortized or not
        /// <summary>
        /// Indicates loan was reamortized earlier.
        /// </summary>
        /// <param name="loanRequestId">Loan Request ID</param>
        /// <returns>True indicates loan recently amortized</returns>
        public static bool IsLoanReamortizedEarlier(int loanRequestId)
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.IsLoanReamortizedEarlier(loanRequestId);
        }
        //END : VC | 2018.06.21 | YRS-AT-3190 |  To check whether is loan is recently reamortized or not

        //START: VC | 2018.07.19 | YRS-AT-4017 | Provides loan details
        public static DataTable GetWebLoans(YMCAObjects.WebLoan webLoan)
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetWebLoans(webLoan);
        }
        //END: VC | 2018.07.19 | YRS-AT-4017 | Provides loan details

        //START : SB | 07/25/2018 | YRS-AT-4017 | Loan Admin Console - Get and Save prime interest rate, get loan statistics
        /// <summary>
        ///  Provides prime interest rate of loan
        /// </summary>
        /// <returns>Current prime interest rate</returns>
        public static DataSet GetPrimeRate()
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetPrimeRate();
        }

        /// <summary>
        /// Updates prime rate from old interest to new interest rate for loan  in loan admin 
        /// </summary>
        /// <param name="paramrate">New Interest rate</param>
        public static void SavePrimeRate(string primeRate)
        {
            YMCARET.YmcaDataAccessObject.LoanInformationDAClass.SavePrimeRate(primeRate);
        }
       
        /// <summary>
        /// Provides all loan statistics 
        /// </summary>
        /// <returns>contains count of all loan statuses</returns>
        public static DataSet GetLoanStatistics()
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanStatistics();
        }
        //END : SB | 07/25/2018 | YRS-AT-4017 | Loan Admin Console - Get and Save prime interest rate, get loan statistics

        //START: SB | 07/25/2018 | YRS-AT-4017 | Loan Admin Console - Get exceptions list for loan admin console
        public static DataTable GetLoanExceptions()
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanExceptions();
        }
        //END: SB | 07/25/2018 | YRS-AT-4017 | Loan Admin Console - Get exceptions list for loan admin console
      
        //START: MMR | 2018.07.23 | YRS-AT-4017 | Get List of loans for Processing
        /// <summary>
        /// This method will return List of loans for processing
        /// </summary>
        /// <returns>LoansProcessing Table</returns>
        public static DataTable GetLoansForProcessing()
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoansForProcessing();
        }
        //END: MMR | 2018.07.23 | YRS-AT-4017 | Get List of loans for Processing

        //START: MMR | 2018.09.12 | YRS-AT-4017 | Get loan validation Reason
        /// <summary>
        /// This method will return loan validation reason.
        /// </summary>
        /// <param name="loanRequestId">Loan Request Id</param>
        /// <returns>Reason</returns>
        public static string GetLoanValidationReason(int loanRequestId)
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetLoanValidationReason(loanRequestId);
        }
        //END: MMR | 2018.09.12 | YRS-AT-4017 | Get loan validation Reason

        //START: ML | 2019.01.08 | YRS-AT-4244 | YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462) 
        /// <summary>
        /// This method will return boolen value as OND Status is updated or not.
        /// </summary>
        /// <param name="ondData"></param>
        /// <returns></returns>
        public static Boolean  UpdateONDStatus(string  ondData)
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.UpdateONDStatus(ondData);
        }
        //END: ML | 2019.01.08 | YRS-AT-4244 | YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462) 


        //START: PK | 01.15.2019 | YRS-AT-2573 | Get list for first payment date 
        public static DataTable GetListForFirstPaymentDate(int LoanRequestId, string YmcaID)
        {
            return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetListForFirstPaymentDate(LoanRequestId,YmcaID);
        }
        //END: PK | 01.15.2019 | YRS-AT-2573 | Get list for first payment date

        //START: ML | 2019.01.17 | YRS-AT-3157 | Fetching loan freeze unfreeze history
        public static List<YMCAObjects.LoanFreezeUnfreezeHistory> LoanFreezeUnfreezeHistoryList(string loanDetailId)
        {
            List<YMCAObjects.LoanFreezeUnfreezeHistory> historys;
            YMCAObjects.LoanFreezeUnfreezeHistory history;
            DataTable loanFreezeUnfreezeHistory;
            try
            {
                historys = new List<YMCAObjects.LoanFreezeUnfreezeHistory>();

                loanFreezeUnfreezeHistory = YMCARET.YmcaDataAccessObject.LoanInformationDAClass.LoanFreezeUnfreezeHistoryList(loanDetailId);
                //Get Data in Datatable and convert it into List
                if (loanFreezeUnfreezeHistory.Rows.Count > 0)
                {
                    foreach (DataRow row in loanFreezeUnfreezeHistory.Rows)
                    {
                        history = new YMCAObjects.LoanFreezeUnfreezeHistory();
                        history.DateTime = row["StatusDate"].ToString();
                        history.Reason = row["chvReason"].ToString();
                        history.Status = row["chvStatus"].ToString();
                        
                        if (!Convert.IsDBNull(row["chvCreator"]))
                        {
                            history.User = row["chvCreator"].ToString();
                        }
                        
                        if (Convert.IsDBNull(row["mnyAmount"]))
                        {
                            history.Amount = "";
                        }
                        else
                        {
                            history.Amount = Convert.ToDecimal(row["mnyAmount"]).ToString("N2");
                        }
                        historys.Add(history);
                    }
                }
                return historys;
            }
            catch
            {
                throw;
            }
            finally
            {
                history = null;
                historys = null;
                loanFreezeUnfreezeHistory = null;
            }
        }
        //END: ML | 2019.01.17 | YRS-AT-3157 | Fetching loan freeze unfreeze history
        //START: MMR | 2019.01.23 | YRS-AT-4118 | Get active YMCA Details
        public static DataSet GetActiveYmcaDetails(string loanRequestID)
        {
                return YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetActiveYmcaDetails(loanRequestID);
            
        }
        //END: MMR | 2019.01.23 | YRS-AT-4118 | Get active YMCA Details

        // Start | SC | 2020.04.10 | YRS-AT-4852 | Check if Loan suspended due to COVID
        public static bool IsLoanSuspendedDueToCovid(int loanRequestId)
        {
            bool bIsCovidTransaction = false;
            DataSet dsCovidTransactions;
            try
            {               
                dsCovidTransactions=YMCARET.YmcaDataAccessObject.LoanInformationDAClass.GetCovidTransactions(loanRequestId, "LOANSUSPND");

                if (dsCovidTransactions != null && dsCovidTransactions.Tables["CovidTransactions"].Rows.Count > 0)
                    bIsCovidTransaction = true;
                return bIsCovidTransaction;
            }
            catch
            {
                throw;
            }

        }
        // Start | SC | 2020.04.10 | YRS-AT-4852 | Check if Loan suspended due to COVID
    }
}
