//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA - YRS
// FileName			:	RefundRequestBOClass.cs
// Author Name		:	SrimuruganG
// Employee ID		:	32365
// Email			:	srimurugan.ag@icici-infotech.com
// Contact No		:	8744
// Creation Time	:	8/15/2005 3:27:04 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By           Date(MM/DD/YYYY)    Remarks
//Shubhrata Tripathi    05/28/2007          Plan Split Changes
//Aparna Samala			24/10/2007			Check Annuity Existence,Get QDRO Member age
//Amit Nigam            2009.09.25 	        Modified as per the additional partial request for the reports-Amit 25-09-2009
//Nikunj Patel			2010.01.07			Merged code received from Bangalore team
//Neeraj Singh          2010.Nov.23         BT-664 : MRD Requirement 
//Shashi Shekhar        2011.04.13          YRS 5.0-877 : Changes to Banking Information maintenance.
//Shashi Shekhar        2011.05.24          BT-837 : Add New FedEx screen needs to be added for Withdrawal status updates.
//Shashi Shekhar        2011.05.25          BT-837 : Change method return type for return msg.
//Shashi Shekhar        2011-25-05          YRS 5.0-1298: added one parameter for Refund cancel reason code.
//Harshala Trimukhe     01-Mar-2012         BT:978-YRS 5.0-1519 - 'YMCA acct at request" amt on withdrawal screen is incorrect : Added new function GetBARequestedPIA
//Dinesh Kanojia        2013-03-19          Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
//Sanjay Rawat          2013.10.28          BT-2247/YRS 5.0-2229:Change the TD termination and 6 month resumption date
//Anudeep A             2015.05.05          BT-2699:YRS 5.0-2441 : Modifications for 403b Loans 
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Anudeep A             2015.11.16          YRS-AT-2639 - YRS enh: Withdrawals Phase2: Esign Sprint: Refund Process request prepopulate data if available - make it editable by ManagementTeam only
//chandrasekar.c        2016.01.11          YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
//Bala                  2016.01.19          YRS-AT-2398: Customer Service requires a Special Handling alert for officers.
//Anudeep               2016.02.17          YRS-AT-2640 - YRS enh: Withdrawals Phase2:Sprint2: allow AdminTool link to launch a prepopulated Yrs withdrawal page
//Chandra sekar.c       2016.05.27          YRS-AT-3014 - YRS enh: Configurable Withdrawals project
//Santosh Bura          2016.08.31          YRS-AT-3028 -  YRS enh-RMD Utility - married to married Spouse beneficiary adjustment(TrackIT 25233) 
//Sanjay GS Rawat       2016.09.23          YRS-AT-3164 - YRS enh: Hot Fix:special Hardship Withdrawal - how to allow contributions to continue temporarily 
//Pramod P. Pokale      2016.11.18          YRS-AT-3146 - YRS enh: Fees - Partial Withdrawal Processing fee 
//Pramod P. Pokale      2017.11.27          YRS-AT-3653 - YRS enh: HOT FIX: Validation adjustment for prior Cancelled SHIRA (with regards to ReHires)(TrackIT 29582) 
//Megha Lad             2019.06.10          YRS-AT-4461 - YRS bug: HOT FIX NEEDED- portability bug- request-disbursement is for less than amount request (TrackIT38439)
//Sanjay GS Rawat 		2019.07.17          YRS-AT-4498 - YRS enh: REQUIRED LIVE 1/1/2020 - Hardship Withdrawals Legal Changes 
//Sanjay GS Rawat 		2019.07.23          YRS-AT-3870 - YRS bug-non-taxable TD amount not available for hardship (TrackIT 32820) 
//Pooja Kumkar          2019.07.11          YRS-AT-2670 - YRS enh:Hacienda withholding message - Puerto Rico based YMCA orgs    
//Pooja Kumkar          2020.04.29          YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688)    
//*******************************************************************************

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
using System.Collections;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for RefundRequest.
	/// </summary>
	public class RefundRequest
	{
		

		private RefundRequest()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public static DataSet GetRequestStatus(string param_string_RequestStatus) 
		{
			DataSet l_DataSet = null;

			try
			{
				l_DataSet = RefundRequestDAClass.GetRequestStatus (param_string_RequestStatus); 

				if (l_DataSet != null)
				{
					l_DataSet.DataSetName = "RequestStatus";
				}

				return l_DataSet;
			}
			catch
			{
				throw;
			}
		}

		public static DataSet LookupPerson (string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName, string parameterFormName,string parameterCity, string parameterState)
		{
			DataSet l_DataSet = null;

			try
			{
                //STARTS Dinesh Kanojia : 2013-03-19 : Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                l_DataSet = FindInfoDAClass.LookUpPerson(parameterSSNo, parameterFundNo, parameterLastName, parameterFirstName, parameterFormName, parameterCity, parameterState,String.Empty,String.Empty);
                //l_DataSet = FindInfoDAClass.LookUpPerson(parameterSSNo, parameterFundNo, parameterLastName, parameterFirstName, parameterFormName, parameterCity, parameterState);
                //ENDS Dinesh Kanojia : 2013-03-19 : Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
				if (l_DataSet != null)
				{
					l_DataSet.DataSetName = "FindPerson";				
				}

				return l_DataSet;
			}

			catch 
			{
				throw;
			}
			
		}

		public static DataSet LookupMemberDetails (string parameterPersonID,string parameterFundEventID)
		{
			try
			{
				return RefundRequestDAClass.LookupMemberDetails (parameterPersonID,parameterFundEventID);
			}
			catch 
			{
				throw;
			}
		}

        //Start: Bala: 01/19/2019: YRS-AT-2398: Officers employement details.
        /// <summary>
        /// Provides officers employement details which describes employees eligibility to receive special handling.
        /// </summary>
        /// <param name="parameterPersonID"></param>
        /// <returns></returns>
        public static DataSet GetSpecialHandlingDetails(string parameterPersonID)
        {
            try
            {
                return RefundRequestDAClass.GetSpecialHandlingDetails(parameterPersonID);
            }
            catch
            {
                throw;
            }
        }
        //End: Bala: 01/19/2019: YRS-AT-2398: Officers employement details.

        //AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
        public static DataSet LookupMemberAccounts(string parameterFundID, bool parameterblnIncludeLoanAccts)
		{
			try
			{
                //AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
                return RefundRequestDAClass.LookupTransactionForRefunds(parameterFundID, parameterblnIncludeLoanAccts);
			}
			catch 
			{
				throw;
			}
		}
		public static DataTable GetFirstInstallment(string parameterRefundRequestID)
		{
			try
			{
				return RefundRequestDAClass.GetFirstInstallment (parameterRefundRequestID);
			}
			catch
			{
				throw;
			}

		}
		//Shubhrata Jan 22nd 2007
		public static DataTable SearchConfigurationMaintenance(string parameterConfigurationMaintenance)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaConfigMaintenanceDAClass.SearchConfigurationMaintenance(parameterConfigurationMaintenance).Tables[0]);
			}
			catch
			{
				throw;
			}

		}

		public static DataTable MemberRefundRequestDetails (string paramaeterPersonID, string parameterFundID)
		{
			try
			{
				return (RefundRequestDAClass.MemberRefundRequestDetails (paramaeterPersonID, parameterFundID));
			}
			catch 
			{
				throw ;
			}
		}

		public static DataTable MemberNotes(string paramaeterPersonID)
		{
			try
			{
				return (RefundRequestDAClass.MemberNotes (paramaeterPersonID));
			}
			catch 
			{
				throw;
			}
		}

		public static decimal GetCurrentPIA (string parameterFundID)
		{
			try
			{
			 	return (RefundRequestDAClass.GetCurrentPIA (parameterFundID));
			}
			catch 
			{
				throw ;
			}
		}

		public static decimal GetTerminatePIA (string parameterFundID)
		{
			try
			{
				return (RefundRequestDAClass.GetTerminatePIA(parameterFundID));
			}
			catch 
			{
				throw ;
			}
		}
         
		//BA Account YMCA PhaseV 03-04-2009
		public static decimal GetBACurrentPIA (string parameterFundID)
		{
			try
			{
				return (RefundRequestDAClass.GetBACurrentPIA (parameterFundID));
			}
			catch 
			{
				throw ;
			}
		}

        //Added By Harshala  BT 978
        public static decimal GetBARequestedPIA(string parameterFundID, string parameterRefRequestID)
        {
            try
            {
                return (RefundRequestDAClass.GetBARequestedPIA(parameterFundID, parameterRefRequestID));
            }
            catch
            {
                throw;
            }
        }

		public static decimal GetBATerminatePIA (string parameterFundID)
		{
			try
			{
				return (RefundRequestDAClass.GetBATerminatePIA(parameterFundID));
			}
			catch 
			{
				throw ;
			}
		}
      //Added By ganeswar on july-21-2009
		public static decimal GetBATerminatePIAAtRequest (string parameterFundID)
		{
			try
			{
				return (RefundRequestDAClass.GetBATerminatePIAAtRequest(parameterFundID));
			}
			catch 
			{
				throw ;
			}
		}
      //Added By ganeswar on july-21-2009

		//'BA Account YMCA PhaseV 03-04-2009

		public static DataTable GetRefundConfiguration ()
		{
			try
			{
				return (RefundRequestDAClass.GetRefundConfiguration ());
			}
			catch
			{
				throw;
			}
		}

		//by Aparna 18/04/2007 change in the proc used to get the refund configuration
		public static DataTable GetConfigurationCategoryWise (string parameterCategory)
		{
			try
			{
				return (RefundRequestDAClass.GetConfigurationCategoryWise(parameterCategory));
			}
			catch
			{
				throw;
			}
		}
		//by Aparna 18/04/2007

		public static decimal GetDistributionPeriod (int parameterPersonAge)
		{
			try
			{
				return (RefundRequestDAClass.GetDistributionPeriod (parameterPersonAge));
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet GetSchemaRefundTable () 
		{
			try
			{
				return (RefundRequestDAClass.GetSchemaRefundTable ());
			}
			catch 
			{
				throw;
			}
		}

		public static DataTable GetAccountBreakDown ()
		{
			try
			{
				return (RefundRequestDAClass.GetAccountBreakDown ());
			}
			catch 
			{
				throw;
			}

		}
		public static int GetLoanStatus(string parameterPersId)
		{
			
			try
			{
				return RefundRequestDAClass.GetLoanStatus(parameterPersId);
			}	
			catch
			{
				throw;
			}
		}

        //START: SR | 07/24/2019 | YRS-AT-4498 | A new method added to check Pre loan requirement for hardship withdrawal. 
        public static bool IsPreLoanRequiredForHardship(string parameterPersId, decimal tdAmount)
        {
            try
            {
                return RefundRequestDAClass.IsPreLoanRequiredForHardship(parameterPersId, tdAmount);
            }
            catch
            {
                throw;
            }
        }
        //END: SR | 07/24/2019 | YRS-AT-4498 | A new method added to check Pre loan requirement for hardship withdrawal.

		//Modified by Dilip to add flags for AM-Matched &  TM-Matched on 03-12-2009
		//public static ArrayList InsertRefunds (DataSet parameterDataset,DataSet parameterPartialDataset,bool bool_NotIncludeYMCAAcct, bool bool_NotIncludeYMCALegacyAcct)
		public static ArrayList InsertRefunds (DataSet parameterDataset,DataSet parameterPartialDataset,bool bool_NotIncludeYMCAAcct, bool bool_NotIncludeYMCALegacyAcct,bool Bool_NotIncludeAMMatched, bool Bool_NotIncludeTMMatched)
		{
		//Modified by Dilip to add flags for AM-Matched &  TM-Matched on 03-12-2009
			ArrayList ArrayList_RequestId = new ArrayList();
			
			try
			{
				//Added By Ganeswar Sahoo 29july 2009
				
				
				//Modified by Dilip to add flags for AM-Matched &  TM-Matched on 03-12-2009
				//ArrayList_RequestId=  RefundRequestDAClass.InsertRefunds(parameterDataset,parameterPartialDataset,bool_NotIncludeYMCAAcct,bool_NotIncludeYMCALegacyAcct);
				ArrayList_RequestId=  RefundRequestDAClass.InsertRefunds(parameterDataset,parameterPartialDataset,bool_NotIncludeYMCAAcct,bool_NotIncludeYMCALegacyAcct,Bool_NotIncludeAMMatched,Bool_NotIncludeTMMatched);
				//Modified by Dilip to add flags for AM-Matched &  TM-Matched on 03-12-2009  
				
				return ArrayList_RequestId;
				
			}
			catch 
			{
				throw;
			}
		}




	//Added By Ganeswar Sahoo 29july 2009 for insert for Partial Prorating logic
		public static void GetPartialRefundsRequest(string RefundRequestID,decimal Percentage,bool bool_IncludeYMCAAcct,bool bool_IncludeYMCALegacyAcct)
		{
			try
			{
						
			 // RefundRequestDAClass.GetPartialRefundsRequestData(RefundRequestID,Percentage,bool_IncludeYMCAAcct, bool_IncludeYMCALegacyAcct);
				
			}
			catch 
			{
				throw;
			}
		}

	
		
	//Added By Ganeswar Sahoo 29july 2009 for insert for Partial Prorating logic

	//Added By Ganeswar Sahoo 29july 2009 for insert for Partial Prorating logic

        //Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
        public static void CancelPendingRefundRequest(string parameterRefundRequest, string parameterRefCanResCode)
		{
			try
			{
                RefundRequestDAClass.CancelPendingRefundRequest(parameterRefundRequest, parameterRefCanResCode);
			}
			catch 
			{
				throw;
			}
		}

		//Added Ganeswar 10thApril09 for BA Account Phase-V /begin
		public static bool IncludeorExcludeYMCAMoney (string parameterAccountBreakdowntypes,string parameterAccountType,string parameterRefundRequestID)
		{
			try
			{
				return RefundRequestDAClass.IncludeorExcludeYMCAMoney (parameterAccountBreakdowntypes,parameterAccountType,parameterRefundRequestID);
			}
			catch
			{
				throw;
			}

		}
      //Added Ganeswar 10thApril09 for BA Account Phase-V /End

		public static DataTable GetQDRODetails (string parameterPersonID)
		{
			try
			{
				return (RefundRequestDAClass.GetQDRODetails (parameterPersonID));
			}
			catch 
			{
				throw;
			}
		}
        //AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
        public static DataSet LookupTransactionForRefunds(string parameterFundID, bool parameterblnIncludeLoanAccts)
		{
			try
			{
                //AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
                return RefundRequestDAClass.LookupTransactionForRefunds(parameterFundID, parameterblnIncludeLoanAccts);
			}
			catch
			{
				throw;
			}
		}
		public static DataTable GetRefundRequestDetails (string parameterRefundRequestID)
		{
			try
			{
				return RefundRequestDAClass.GetRefundRequestDetails(parameterRefundRequestID);
			}
			catch
			{
				throw;
			}
		}

		public static DataTable GetRefundRequestsDetails (string parameterRefundRequestID)
		{
			try
			{
				return RefundRequestDAClass.GetRefundRequestsDetails (parameterRefundRequestID);
			}
			catch
			{
				throw;
			}

		}

		public static DataTable GetRefundableDataTable (string paramterFundEventID)
		{
			DataTable	l_DataTable = null;
			
			try
			{
				l_DataTable = RefundRequestDAClass.GetDataTableRefundable (paramterFundEventID);

				if (l_DataTable == null) return null;

				foreach (DataRow l_DataRow in l_DataTable.Rows)
				{
					l_DataRow ["Emp.Total"] = Convert.ToDecimal (l_DataRow ["Taxable"]) + Convert.ToDecimal (l_DataRow ["Non-Taxable"]) + Convert.ToDecimal (l_DataRow ["Interest"]);
					l_DataRow ["YMCATotal"] = Convert.ToDecimal (l_DataRow ["YMCATaxable"]) + Convert.ToDecimal (l_DataRow ["YMCAInterest"]);
 
					l_DataRow ["Total"] = Convert.ToDecimal (l_DataRow ["Emp.Total"]) + Convert.ToDecimal (l_DataRow ["YMCATotal"]);

				}
			
				return l_DataTable;

			}
			catch 
			{
				throw;
			}

		}

		public static DataTable GetDeductions()
		{
			DataSet l_DataSet;
			try
			{
				l_DataSet = YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.GetDeductions ();

				if (l_DataSet != null)
				{
					return l_DataSet.Tables [0];
				}
				else 
					return null;
			}
			catch
			{
				throw;
			}
		}
		public static DataTable GetDetailsForIdxCreation(string p_StringFileName)
		{	
			try
			{
				return RefundRequestDAClass.GetDetailsForIdxCreation(p_StringFileName);
			}
			catch
			{
				throw;
			}
		}
		// By Aparna - 6th Feb-2007

		public static DataTable GetDetailsForIdxCreationForYmca(string p_StringFileName)
		{	
			try
			{
				return RefundRequestDAClass.GetDetailsForIdxCreationForYmca(p_StringFileName);
			}
			catch
			{
				throw;
			}
		}
		public static string GetVignettePath(string p_string_ServerName)
		{
			try
			{
				return RefundRequestDAClass.GetVignettePath(p_string_ServerName);
			}
			catch
			{
				throw;
			}
		}
		public static string GetServerName()
		{
			try
			{
				return RefundRequestDAClass.GetServerName();
			}
			catch
			{
				throw;
			}			
		}

        //Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
        public static string CancelDisbursementRefund(string parameterRefundRequestID, string parameterRefCanResCode)
		{
			try
			{
                return (RefundRequestDAClass.CancelDisbursementRefund(parameterRefundRequestID, parameterRefCanResCode));
			}
			catch 
			{
				throw;
			}
		}
		public static void ValidateRTBal5k (string parameterPersonID, string parameterFundEventID,out decimal parameterTDAmount,out decimal parameterTMAmount,out decimal parameterRTAmount)
		{
			try
			{
				RefundRequestDAClass.ValidateRTBal5k (parameterPersonID,parameterFundEventID,out parameterTDAmount,out parameterTMAmount,out parameterRTAmount);
			}
			catch 
			{
				throw;
			}
		}

	#region "Methods for Saving the Refund Process "


		public static DataSet GetRefundTransactionSchemas ()
		{
			try
			{
				return (RefundRequestDAClass.GetRefundTransactionSchemas ());
			}
			catch 
			{
				throw;
			}

		}

		public static string GetPersonBankingBeforeEffectiveDate (string parameterPersonID)
		{

			DataTable	l_DataTable;
			DataRow []	l_FoundRow;
			DataRow		l_DataRow;

			string l_QueryString = "";

			try
			{
				l_DataTable = YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.GetPersBankingBeforeEffDate ( DateTime.Now.ToString ());

				if (l_DataTable != null)
				{

					l_QueryString = @"PersID = '" + parameterPersonID + @"'";

					l_FoundRow = l_DataTable.Select (l_QueryString);

					if (l_FoundRow == null) return "U";

					if (l_FoundRow.Length < 1) return "U";
				
					l_DataRow = l_FoundRow [0];
					
					if (l_DataRow == null) return "U";

                    //Shashi Shekhar:13-Apr.2011:YRS 5.0-877 : Changes to Banking Information maintenance.
                    if (l_DataRow["CurrencyCode"].ToString() == null)
                    {
                        return "U";
                    }
                    else
                    {
                        return (l_DataRow["CurrencyCode"].ToString());
                    }

				}
				else
					return "U";

			}
			catch 
			{
				throw;
			}
			
		}


		public static DataTable GetTransactions (string parameterFundEventID)
		{
			try
			{
				return (RefundRequestDAClass.GetTransactions (parameterFundEventID));
			}
			catch 
			{
				throw;
			}
		}

		public static DataTable GetPartialWithdrawalTransaction (string parameterRefRequestID)
		{
			try
			{
				return (RefundRequestDAClass.GetPartialWithdrawalTransaction (parameterRefRequestID));
			}
			catch 
			{
				throw;
			}
		}
		public static DataTable GetTransactionsForMarket (string parameterFundEventId,string parameterRefRequestID)
		{
			try
			{
				return (RefundRequestDAClass.GetTransactionsForMarket (parameterFundEventId,parameterRefRequestID));
			}
			catch 
			{
				throw;
			}
		}
		public static DataTable GetTransactionsForMarketForSaveProcess (string parameterFundEventId,string parameterRefRequestID)
		{
			try
			{
				return (RefundRequestDAClass.GetTransactionsForMarketForSaveProcess (parameterFundEventId,parameterRefRequestID));
			}
			catch 
			{
				throw;
			}
		}


		public static DataTable GetTransactionType (string parameterFundEventID)
		{
			try
			{
				return (RefundRequestDAClass.GetTransactionType (parameterFundEventID));
			}
			catch 
			{
				throw;
			}
		}

		public static DataTable GetPayeeDetails (string parameterInstitutionName)
		{
			try
			{
				return (RefundRequestDAClass.GetPayeeDetails (parameterInstitutionName));
			}
			catch 
			{
				throw;
			}
		}
	//start-added one more parameter StatusType for Plan Split Changes
		
        //START | SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim 
		public static bool SaveRefundRequestProcess (DataSet parameterDataSet, string parameterPersonID, string parameterFundID, string paramterRefundType, bool parameterVested, bool parameterTerminated, bool parameterTookTDAccount,string parameterFundEventStatusType,DataSet parameterDataSetForNewRequests,bool parameterNeedsNewRequests,string parameterInitialRefRequestId,bool parameterIsMarket,decimal parameterTaxRate,string parameterPayeeName,string parameterRolloverinstitutionId,decimal parameterRollOverAmount,bool parameterIsActive,decimal parameterTaxable,decimal parameterNonTaxable, bool IRSOverride = false)
		{
			try
			{
				return (RefundRequestDAClass.SaveRefundRequestProcessRefunds(parameterDataSet, parameterPersonID, parameterFundID, paramterRefundType, parameterVested, parameterTerminated, parameterTookTDAccount,parameterFundEventStatusType,parameterDataSetForNewRequests, parameterNeedsNewRequests,parameterInitialRefRequestId,parameterIsMarket,parameterTaxRate,parameterPayeeName,parameterRolloverinstitutionId,parameterRollOverAmount,parameterIsActive,parameterTaxable,parameterNonTaxable,IRSOverride ));
				
			}
			catch 
			{
				throw;
			}
		}
        //END | SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for temperory Hardship for Louisiana flood victim 
	//end-added one more parameter StatusType for Plan Split Changes	
	#endregion
		//method added by ruchi to add the payee if it does not exist or the payee id if it exists

		public static void Get_RefundRolloverInstitutionID(string paramRolloverInstitutionName, out string outpara_RolloverInstitutionUniqueID )
		{
			/* Call  Procedure dbo.yrs_usp_BS_Get_RefundRolloverInstitutionID with Institution Name
				/* If it exists use it, else  INSERT new record & use that Unique ID  */
				
			try 
			{ 

				YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.Get_RefundRolloverInstitutionID(paramRolloverInstitutionName, out outpara_RolloverInstitutionUniqueID);

			}
			catch
			{
				throw;
			}
		}

		//method added by Ruchi to update tax in method PrintReports
		public static void UpdateTax (string paramaeterUniqueID, double parameterTax)
		{
			try
			{
				RefundRequestDAClass.UpdateTax(paramaeterUniqueID, parameterTax);
			}
			catch 
			{
				throw ;
			}
		}


		public static DataSet YMCARefundLetterHardship(string paramaetericPersID)
		{
			try
			{
				return (RefundRequestDAClass.YMCARefundLetterHardship(paramaetericPersID));
			}
			catch 
			{
				throw ;
			}
		}

		public static double GetPrincipal(string parameterPersId)
		{			
			try
			{
				return RefundRequestDAClass.GetPrincipal(parameterPersId);
			}	
			catch
			{
				throw;
			}
		}
//		//Added By Ganeswar For Partial withdraw on june 26th 2009
//		public static decimal GetPartialMonthDifference (string FundId,string Plantype)
//		{
//			try
//			{
//				return RefundRequestDAClass.GetPartialMonthDifference (FundId,Plantype);
//			}
//			catch 
//			{
//				throw;
//			}
//		}
		//Added By Ganeswar For Partial withdraw on june 26th 2009


		public static DateTime GetPartialMonthDifferenceDate(string parameterFundID,string PrameterPlanType )
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetPartialMonthDifferenceDate(parameterFundID,PrameterPlanType);
			}
			catch
			{
				throw;
			}
		}
		#region "Plan Split Changes"
		public static string getGUI_ID()
		{
			try
			{
				return YMCARET.YmcaBusinessObject.NonRetiredQDROBOClass.getGUI_ID();
			}
			catch
			{
				throw;
			}
		}
		public static string GetPlanType(string parameterGuiRefRequestsId)
		{			
			try
			{
				return RefundRequestDAClass.GetPlanType(parameterGuiRefRequestsId);
			}	
			catch
			{
				throw;
			}
		}	

		//Aparna 18/10/2007
		public static int CheckAnnuityExistence(string parameterPersid)
		{
			try
			{
				return RefundRequestDAClass.CheckAnnuityExistence(parameterPersid);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetAnnuityExistence(string parameterPersid,string parameterFundeventId)
		{
			try
			{
					return RefundRequestDAClass.GetAnnuityExistence(parameterPersid,parameterFundeventId);
			}
			catch
			{
				throw;
			}
		}
		public static int GetQDROParticipantAge(string parameterFundeventId)
		{
			try
			{
				return RefundRequestDAClass.GetQDROParticipantAge(parameterFundeventId);
			}
			catch
			{
				throw;
			}
		}

		//
		public static DataSet GetWithdrawalReportData(string parameterRefRequestID)
		{
			try
			{
				return RefundRequestDAClass.GetWithdrawalReportData(parameterRefRequestID);
			}
			catch
			{
				throw;
			}
		}
		#endregion // "Plan Split Changes"

        /// <summary>
        /// To get MRD records from Table atsMRDRecords for given FundEventID
        /// </summary>
        /// <param name="parameterFundEventId"></param>
        /// <returns></returns>
        public static DataTable GetMRDRecords(string parameterFundEventId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetMRDRecords(parameterFundEventId);
            }
            catch
            {
                throw;
            }
        }
        // START : SB | 08/31/2016 | YRS-AT-3028 | Method to get Recalculated RMD values for withdrawals
        public static DataSet GetRMDForWithdrawalProcessing(string parameterFundEventId) 
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetRMDForWithdrawalProcessing(parameterFundEventId);
            }
            catch
            {
                throw;
            }
        }
        // END : SB | 08/31/2016 | YRS-AT-3028 | Method to get Recalculated RMD values for withdrawals
        //--shashi-2011-06-24:  BT-837 : Add New FedEx screen needs to be added for Withdrawal status updates.-----------
        public static DataSet GetDisbRefRequest()
        {
            try
            {
                return RefundRequestDAClass.GetDisbRefRequest();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// To Update tracking No
        /// </summary>
        /// <param name="paramTrackingNo"></param>
        /// <param name="paramRefReqId"></param>
        public static string UpdateRefReqTrackingNo( string paramRefReqId, string paramTrackingNo)
        {
            try
            {
              return  RefundRequestDAClass.UpdateRefReqTrackingNo(paramRefReqId, paramTrackingNo);
            }
            catch
            {
                throw;
            }
        }
       
        /// <summary>
        /// To get the Refund request cancel reason code
        /// </summary>
        /// <returns></returns>
        public static DataSet GetRefCancelReasonCodes()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetRefCancelReasonCodes ();
            }
            catch
            {
                throw;
            }
        }

        //SR:2013.10.28:BT-2247/YRS 5.0-2229:Change the TD termination and 6 month resumption date
        /// <summary>
        /// To get TD Locking Period
        /// </summary>
        /// <param name="paramStrRefRequestId"></param>
        /// <returns>Dataset</returns>
        public static DataTable GetTDLockingPeriod(string paramStrRefRequestId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetTDLockingPeriod(paramStrRefRequestId);
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
            try
            {
                return YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetRefRequestOptionsVer2(paramStrRefRequestId);
            }
            catch
            {
                throw;
            }
        }

        //End: AA : YRS-AT-2639 Added to get referquest options stored from yrs website
        //Start- chandrasekar.c:2016.01.11:YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
        public static int GetExtendWithdrawalExpireDay()
        {
            DataSet dtExtendExpireRequestKey = null;
            int intRefundExtendExpireDays = 30;
            try
            {
                dtExtendExpireRequestKey = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("REFUND_EXTEND_EXPIRE_DAYS");
                if (IsNonEmpty(dtExtendExpireRequestKey))
                {
                    if (dtExtendExpireRequestKey.Tables[0].Rows[0]["Value"].ToString().Trim() != string.Empty)
                    {
                        intRefundExtendExpireDays = Convert.ToInt32(dtExtendExpireRequestKey.Tables[0].Rows[0]["Value"]);
                    }
                }
                return intRefundExtendExpireDays;
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateRefundRequestsExtendExpireDates(string paramaeterstrPersonID, string paramstrRefRequestId)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.RefundRequestDAClass.UpdateRefundRequestsExtendExpireDate(paramaeterstrPersonID, paramstrRefRequestId);
            }
            catch
            {
                throw;
            }
        }

        private static bool IsNonEmpty(DataSet ds)
        {
            if (ds == null)
                return false;
            else if (ds.Tables.Count == 0)
                return false;
            else if (ds.Tables[0].Rows.Count == 0)
                return false;

            return true;
        }
        //End- chandrasekar.c:2016.01.11:YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request

        //Start:AA:02.17.2016 YRS-AT-2640 Added to get the fundeventid for the given input refrequestid
        public static string GetFundEventIDFromRefrequestID(string ParameterRefrequestId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RefundRequestDAClass.GetFundEventIDFromRefrequestID(ParameterRefrequestId);

            }
            catch
            {
                throw;
            }
        }
        //End:AA:02.17.2016 YRS-AT-2640 Added to get the fundeventid for the given input refrequestid
        //START - 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project
        //START - ML| 2019.06.07 |YRS-AT-4461  - YRS bug: HOT FIX NEEDED- portability bug- request-disbursement is for less than amount request (TrackIT38439)
         // public static DataTable GetRefundRequestConfiguration(int intPersonAge, bool bIsPersonTerminate)        
        public static DataTable GetRefundRequestConfiguration(int intPersonAge, bool bIsPersonTerminate, bool bitApplyBALegacyCombinedMinAge = false)
        //START - ML| 2019.06.07 |YRS-AT-4461  - YRS bug: HOT FIX NEEDED- portability bug- request-disbursement is for less than amount request (TrackIT38439)
        {
            try
            {
                return (RefundRequestDAClass.GetRefundRequestConfiguration(intPersonAge, bIsPersonTerminate, bitApplyBALegacyCombinedMinAge)); // ML| 2019.06.05 |YRS-AT-4458 | New parameter to apply either BA_Legacy Combined Min Age Or Default age from atsMetaConfiguration.
            }
            catch
            {
                throw;
            }
        }
        //END - 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project

        //START: PPP | 11/18/2016 | YRS-AT-3146 | Validating fees against balances left after withdrawal, whether they are sufficient enough to deduct fee as per new fee charging hierarchy
        public static bool ValidateWithdrawalProcessingFee(string refundRequestID)
        {
            try
            {
                return RefundRequestDAClass.ValidateWithdrawalProcessingFee(refundRequestID);
            }
            catch
            {
                throw;
            }
        }
        //END: PPP | 11/18/2016 | YRS-AT-3146 | Validating fees against balances left after withdrawal, whether they are sufficient enough to deduct fee as per new fee charging hierarchy

        //START: PPP | 11/27/2017 | YRS-AT-3653 | Checks whether SHIRA request can be canceled or not
        public static bool IsSHIRARequestCancelable(string personID)
        {
            return RefundRequestDAClass.IsSHIRARequestCancelable(personID);
        }
        //END: PPP | 11/27/2017 | YRS-AT-3653 | Checks whether SHIRA request can be canceled or not

        //START: PK | 11/07/2019 | YRS-AT-2670 | Checks whether participant is from puerto rico ymca 
        public static bool IsYmcaPuertoRico(string fundEventID)
        {
            try
            {
                return RefundRequestDAClass.IsYmcaPuertoRico(fundEventID);
            }
            catch
            {
                throw;
            }
        }
        //END: PK | 11/07/2019 | YRS-AT-2670 | Checks whether participant is from puerto rico ymca

        //START: PK | 29/04/2020 | YRS-AT-4854 | Checks whether refund request is of Covid or not.
        public static bool IsRequestCovid(string refRequestID)
        {
            try
            {
                return RefundRequestDAClass.IsRequestCovid(refRequestID);
            }
            catch
            {
                throw;
            }
        }
        //END: PK | 29/04/2020 | YRS-AT-4854 | Checks whether refund request is of Covid or not.
    }
}
