/*************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	
' Author Name		:	Dhananjay Prajapti 
' Employee ID		:	33338	
' Email				:	dhananjay.prajapati@3i-infotech.com
' Contact No		:	55928713
' Creation Time		:	17/10/2005 
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	This form is used from safeharborDisbursments Data Access Layer 
'****************************************************************************************/
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for SafeHarborDisbursementDAClass.
	/// </summary>
	public class SafeHarborDisbursementDAClass
	{
		public SafeHarborDisbursementDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region "Look up for Ref Request"
		public DataSet LookupRefRequests(string l_string_RefRequestsDate,string l_string_disbursements_type)
		{
			DataSet dsLookUpRefRequests = null;
			Database db = null;
			DbCommand commandLookUpRefRequests = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;

				commandLookUpRefRequests = db.GetStoredProcCommand("yrs_usp_Lookup_vrRefRequests");

				if (commandLookUpRefRequests == null) return null;
				db.AddInParameter(commandLookUpRefRequests,"@l_string_RefRequest_Date", DbType.String, l_string_RefRequestsDate);
				db.AddInParameter(commandLookUpRefRequests,"@l_string_disbursements_type", DbType.String, l_string_disbursements_type);
			
				commandLookUpRefRequests.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
		
				dsLookUpRefRequests = new DataSet();
				dsLookUpRefRequests.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpRefRequests, dsLookUpRefRequests,"RefRequests");
						
				return dsLookUpRefRequests;
			}
			catch (Exception ex)
			{
				throw  ex;
			}

		}
		#endregion
        
		#region "Update Ref Request Details"
		public bool UpdateRefRequests(int l_int_UniqueID)
		{
		
			Database db = null;
			DbCommand commandUpdateRefRequests = null;
            //Added by Ashish for migration
            int l_RowsAffected = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return false;
		
				commandUpdateRefRequests = db.GetStoredProcCommand("yrs_usp_Update_RefRequests");
						
				if (commandUpdateRefRequests == null) return false;

                db.AddInParameter(commandUpdateRefRequests, "@int_UniqueId", DbType.Int32, l_int_UniqueID);
				commandUpdateRefRequests.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
                //db.ExecuteNonQuery(commandUpdateRefRequests);
                //TODOMIGRATION -need to verify this row affected chnages work or not
                l_RowsAffected=db.ExecuteNonQuery(commandUpdateRefRequests);
				//l_string_output = commandUpdateRefRequests.GetParameterValue("@varchar_Output").ToString();
				//if(commandUpdateRefRequests. RowsAffected > 0)
                if (l_RowsAffected > 0)
					return true; // update 
				else
					return false; //Update fail

															 
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for number of member had died"
		public DataSet LookupMemberHasDied(string l_string_RefRequest_PersID)
		{
			DataSet dsLookupMemberHasDied = null;
			Database db = null;
			DbCommand commandLookupMemberHasDied = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookupMemberHasDied = db.GetStoredProcCommand("yrs_usp_Lookup_MemberHasDied");

				if (commandLookupMemberHasDied == null) return null;

				db.AddInParameter(commandLookupMemberHasDied,"@l_string_RefRequest_PersID", DbType.String, l_string_RefRequest_PersID);

				dsLookupMemberHasDied = new DataSet();
				dsLookupMemberHasDied.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookupMemberHasDied, dsLookupMemberHasDied, "MemberHasDied"); 
				return dsLookupMemberHasDied;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for Reemployed"
		public DataSet LookupReEmployed(int l_int_RefRequest_PersID)
		{
			DataSet dsLookupReEmployed = null;
			Database db = null;
			DbCommand commandLookupReEmployed = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookupReEmployed = db.GetStoredProcCommand("yrs_usp_Lookup_ReEmployed");

				if (commandLookupReEmployed == null) return null;
				db.AddInParameter(commandLookupReEmployed,"@l_int_RefRequest_PersID", DbType.Int32, l_int_RefRequest_PersID);

				commandLookupReEmployed.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsLookupReEmployed = new DataSet();
				dsLookupReEmployed.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookupReEmployed, dsLookupReEmployed, "ReEmployed");
				return dsLookupReEmployed;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for PendingQDRO"
		public DataSet LookupPendingQDRO(string l_string_RefRequest_PersID)
		{
			DataSet dsLookupPendingQDRO = null;
			Database db = null;
			DbCommand commandLookupPendingQDRO = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookupPendingQDRO = db.GetStoredProcCommand("yrs_usp_Lookup_PendingQDRO");

				if (commandLookupPendingQDRO == null) return null;
				db.AddInParameter(commandLookupPendingQDRO,"@l_string_RefRequest_PersID", DbType.String, l_string_RefRequest_PersID);

				commandLookupPendingQDRO.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsLookupPendingQDRO = new DataSet();
				dsLookupPendingQDRO.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookupPendingQDRO, dsLookupPendingQDRO, "PendingQDRO");
				return dsLookupPendingQDRO;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for GetVestingStatus"
		public DataSet LookupVestingStatus(string l_string_RefRequest_FundEventID)
		{
			DataSet dsLookupVestingStatus = null;
			Database db = null;
			DbCommand commandLookupVestingStatus = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookupVestingStatus = db.GetStoredProcCommand("yrs_usp_Lookup_VestingStatus");

				if (commandLookupVestingStatus == null) return null;
				db.AddInParameter(commandLookupVestingStatus,"@l_string_RefRequest_FundEventID", DbType.String, l_string_RefRequest_FundEventID);

				commandLookupVestingStatus.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsLookupVestingStatus = new DataSet();
				dsLookupVestingStatus.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookupVestingStatus, dsLookupVestingStatus, "VestingStatus");
				return dsLookupVestingStatus;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for LookupCurrentBalance"
		public DataSet LookupCurrentBalance(string l_string_RefRequest_FundEventID)
		{
			DataSet dsLookupVestingStatus = null;
			Database db = null;
			DbCommand commandLookupVestingStatus = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookupVestingStatus = db.GetStoredProcCommand("Ap_Sel_TransSumForRefunds");

				if (commandLookupVestingStatus == null) return null;
				db.AddInParameter(commandLookupVestingStatus,"@FundEventidvchar", DbType.String, l_string_RefRequest_FundEventID);

				commandLookupVestingStatus.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsLookupVestingStatus = new DataSet();
				dsLookupVestingStatus.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookupVestingStatus, dsLookupVestingStatus, "TransferSumForRefund");
				return dsLookupVestingStatus;

			}
			catch (Exception ex)
			{
				throw ex;
			}
 
		}
		#endregion
		#region "Look up for LookupAccountType"
		public string LookupAccountType(string l_string_AccountType)
		{
			Database db = null;
			DbCommand commandLookupAccountType = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookupAccountType = db.GetStoredProcCommand("yrs_usp_Lookup_IsBasicAcct");

				if (commandLookupAccountType == null) return null;
				db.AddInParameter(commandLookupAccountType,"@l_varchar_tcAcctType", DbType.String, l_string_AccountType);
				db.AddOutParameter(commandLookupAccountType,"@l_varchar_ReturnValue",DbType.String, 1);

				commandLookupAccountType.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				db.ExecuteNonQuery(commandLookupAccountType);
				return db.GetParameterValue(commandLookupAccountType,"@l_varchar_ReturnValue").ToString();
	

			}
			catch (Exception ex)
			{
				throw ex;
			}
 
		}
		#endregion
		
		#region "Look up for LookBalanceCheck"
		public DataSet LookupBalanceCheck(string l_string_RefRequest_FundEventID,string l_string_VestingFlg)
		{
			DataSet dsLookupBalanceCheck = null;
			Database db = null;
			DbCommand commandLookupBalanceCheck = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookupBalanceCheck = db.GetStoredProcCommand("yrs_usp_Lookup_BalanceCheck");

				if (commandLookupBalanceCheck == null) return null;
				db.AddInParameter(commandLookupBalanceCheck,"@l_varchar_FundEventID", DbType.String, l_string_RefRequest_FundEventID);
				db.AddInParameter(commandLookupBalanceCheck,"@l_varchar_lVestedFlg", DbType.String, l_string_VestingFlg);
 
				commandLookupBalanceCheck.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsLookupBalanceCheck = new DataSet();
				dsLookupBalanceCheck.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookupBalanceCheck, dsLookupBalanceCheck, "BalanceCheck");
				return dsLookupBalanceCheck;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for Update BalanceCheck"
		public bool Update_BalanceCheck(string l_string_RefRequest_UnqiueID)
		{
		
			DbCommand commandUpdateBalanceCheck = null;
			Database db = null;
            //Added by Ashish for migration
            int l_RowsAffected = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return false; //False : indicate the Update Fail

				commandUpdateBalanceCheck = db.GetStoredProcCommand("yrs_usp_Update_BalanceCheck");

				if (commandUpdateBalanceCheck == null) return false;//False : indicate the Update Fail
				db.AddInParameter(commandUpdateBalanceCheck,"@l_varchar_UniqueID", DbType.String, l_string_RefRequest_UnqiueID);
                //l_string_output = commandUpdateRefRequests.GetParameterValue("@varchar_Output").ToString();
                //TODOMIGRATION -need to verify this row affected chnages work or not
				//db.ExecuteNonQuery(commandUpdateBalanceCheck);

                l_RowsAffected = db.ExecuteNonQuery(commandUpdateBalanceCheck);
				//if(commandUpdateBalanceCheck.RowsAffected > 0)
                if (l_RowsAffected > 0)
					return true; // update 
				else
					return false; //Update fail
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for update status"
		public bool Update_Status(string l_string_FundEventID)
		{
		
			DbCommand commandUpdateStatus = null;
			Database db = null;
            //Added by Ashish for migration
            int l_RowsAffected = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return false; //False : indicate the Update Fail

				commandUpdateStatus = db.GetStoredProcCommand("yrs_usp_Update_Status");

				if (commandUpdateStatus == null) return false;//False : indicate the Update Fail
				db.AddInParameter(commandUpdateStatus,"@l_varchar_FundEventID", DbType.String, l_string_FundEventID);

                //TODOMIGRATION -need to verify this row affected chnages work or not
				//db.ExecuteNonQuery(commandUpdateStatus);
               l_RowsAffected= db.ExecuteNonQuery(commandUpdateStatus);
				//l_string_output = commandUpdateRefRequests.GetParameterValue("@varchar_Output").ToString();
               if (l_RowsAffected > 0)
					return true; // update 
				else
					return false; //Update fail
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for LookupMetaOutputFileTypes"
		public DataSet LookupMetaOutputFileTypes()
		{
			DataSet dsLookupMetaOutputFileTypes= null;
			Database db = null;
			DbCommand commandMetaOutputFileTypes = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandMetaOutputFileTypes = db.GetStoredProcCommand("yrs_usp_Lookup_MetaOutputFileTypes");

				if (commandMetaOutputFileTypes == null) return null;
				
				commandMetaOutputFileTypes.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsLookupMetaOutputFileTypes = new DataSet();
				dsLookupMetaOutputFileTypes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandMetaOutputFileTypes, dsLookupMetaOutputFileTypes, "MetaOutputFileTypes");
				return dsLookupMetaOutputFileTypes;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for ReEmployed"
		public DataSet LookupReEmployed(string l_string_RefRequest_PersID)
		{
			DataSet dsLookupReEmployed = null;
			Database db = null;
			DbCommand commandLookupReEmployed = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookupReEmployed = db.GetStoredProcCommand("yrs_usp_Lookup_ReEmployed");

				if (commandLookupReEmployed == null) return null;
				db.AddInParameter(commandLookupReEmployed,"@l_string_RefRequest_PersID", DbType.String, l_string_RefRequest_PersID);

				commandLookupReEmployed.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsLookupReEmployed = new DataSet();
				dsLookupReEmployed.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookupReEmployed, dsLookupReEmployed, "ReEmployed");
				return dsLookupReEmployed;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region"Lookup  to getdate"
		public string GetAccountDate()
		{
			Database db = null;
			DbCommand commandLookupAccountDate = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookupAccountDate = db.GetStoredProcCommand("yrs_usp_Lookup_AccountDate");

				if (commandLookupAccountDate == null) return null;
				//commandLookupAccountType.AddInParameter("@l_varchar_tcAcctType", DbType.String, l_string_AccountType);
				db.AddOutParameter(commandLookupAccountDate,"@l_varchar_Date", DbType.DateTime,30);

				db.ExecuteNonQuery(commandLookupAccountDate);
				return db.GetParameterValue(commandLookupAccountDate,"@l_varchar_Date").ToString();


			}
			catch (Exception ex)
			{
				throw ex;
			}
 

		}
		#endregion
		#region "Look up for LookupAvailableTransaction"
		public DataSet LookupAvailableTransaction(string l_string_FundEventID)
		{
			DataSet dsLookupAvailableTransaction= null;
			Database db = null;
			DbCommand commandAvailableTransaction = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandAvailableTransaction = db.GetStoredProcCommand("yrs_usp_Lookup_AvailableTransaction");

				if (commandAvailableTransaction == null) return null;
				db.AddInParameter(commandAvailableTransaction,"@l_varchar_FundEventID", DbType.String, l_string_FundEventID);

				commandAvailableTransaction.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsLookupAvailableTransaction = new DataSet();
				dsLookupAvailableTransaction.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandAvailableTransaction, dsLookupAvailableTransaction, "AvailableTransaction");
				return dsLookupAvailableTransaction;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for LookupMetaAnnuityBasisTypes"
		public DataSet LookupMetaAnnuityBasisTypes()
		{
			DataSet dsLookupMetaAnnuityBasisTypes= null;
			Database db = null;
			DbCommand commandMetaAnnuityBasisTypes = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandMetaAnnuityBasisTypes = db.GetStoredProcCommand("yrs_usp_Lookup_MetaAnnuityBasisTypes");

				if (commandMetaAnnuityBasisTypes == null) return null;
				
				commandMetaAnnuityBasisTypes.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 


				dsLookupMetaAnnuityBasisTypes = new DataSet();
				dsLookupMetaAnnuityBasisTypes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandMetaAnnuityBasisTypes, dsLookupMetaAnnuityBasisTypes, "MetaAnnuityBasisTypes");
				return dsLookupMetaAnnuityBasisTypes;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for LookupAddressHistory"
		public DataSet LookupAddressHistory(string l_string_AddressHistoryId)
		{
			DataSet dsLookupAddressHistory= null;
			Database db = null;
			DbCommand commandAddressHistory = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandAddressHistory = db.GetStoredProcCommand("yrs_usp_Lookup_AddressHistory");

				if (commandAddressHistory == null) return null;
				db.AddInParameter(commandAddressHistory,"@l_varchar_AddressID", DbType.String, l_string_AddressHistoryId);

				commandAddressHistory.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsLookupAddressHistory = new DataSet();
				dsLookupAddressHistory.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandAddressHistory, dsLookupAddressHistory, "AddressHistory");
				return dsLookupAddressHistory;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for LookupPersBankingBeforeEffDate"
		public DataSet LookupPersBankingBeforeEffDate(DateTime  l_datetime_Date)
		{
			DataSet dsPersBankingBeforeEffDate= null;
			Database db = null;
			DbCommand commandPersBankingBeforeEffDate = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandPersBankingBeforeEffDate = db.GetStoredProcCommand("ap_PersBankingBeforeEffDate");

				if (commandPersBankingBeforeEffDate == null) return null;
				db.AddInParameter(commandPersBankingBeforeEffDate,"@MaxEffDate", DbType.DateTime, l_datetime_Date);


				dsPersBankingBeforeEffDate = new DataSet();
				//dsPersBankingBeforeEffDate.Locale = CultureInfo.InvariantCulture;
				commandPersBankingBeforeEffDate.CommandTimeout = Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); ;
				db.LoadDataSet(commandPersBankingBeforeEffDate, dsPersBankingBeforeEffDate, "BankingBeforeEffDate");
				return dsPersBankingBeforeEffDate;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for LookupPersBankingBeforeEffDate"
		public DataSet LookupViewRefRequests(string l_string_RefRequest_FundEventID)
		{
			DataSet dsViewRefRequests= null;
			Database db = null;
			DbCommand commandViewRefRequests = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandViewRefRequests = db.GetStoredProcCommand("yrs_usp_Lookupview_RefRequests");

				if (commandViewRefRequests == null) return null;
				db.AddInParameter(commandViewRefRequests,"@l_string_FundEventID", DbType.String, l_string_RefRequest_FundEventID);

				commandViewRefRequests.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsViewRefRequests = new DataSet();
				dsViewRefRequests.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandViewRefRequests, dsViewRefRequests, "ViewRefRequests");
				return dsViewRefRequests;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for LookupViewRefRequestsDetails"
		public DataSet LookupViewRefRequestsDetails(string l_string_RefRequest_DetailsPK)
		{
			DataSet dsRefRequestsDetails= null;
			Database db = null;
			DbCommand commandRefRequestsDetails = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandRefRequestsDetails = db.GetStoredProcCommand("yrs_usp_Lookupview_RefRequestDetails");

				if (commandRefRequestsDetails == null) return null;
				db.AddInParameter(commandRefRequestsDetails,"@l_string_lcRefRequestDetailsPK", DbType.String, l_string_RefRequest_DetailsPK);

				commandRefRequestsDetails.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsRefRequestsDetails = new DataSet();
				dsRefRequestsDetails.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandRefRequestsDetails, dsRefRequestsDetails, "ViewRefRequestsDetails");
				return dsRefRequestsDetails;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for LookupRTransactions"
		public DataSet LookupRTransactions(string l_string_RefRequest_FundEventPK)
		{
			DataSet dsTransactions= null;
			Database db = null;
			DbCommand commandTransactions = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandTransactions = db.GetStoredProcCommand("yrs_usp_Lookupview_Transactions");

				if (commandTransactions == null) return null;
				db.AddInParameter(commandTransactions,"@l_string_lcFundEventPK", DbType.String, l_string_RefRequest_FundEventPK);

				commandTransactions.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsTransactions = new DataSet();
				dsTransactions.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandTransactions, dsTransactions, "ViewTransactions");
				return dsTransactions;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for Disbursements"
		public DataSet LookupDisbursements()
		{
			DataSet dsDisbursements= null;
			Database db = null;
			DbCommand commandDisbursements = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandDisbursements = db.GetStoredProcCommand("yrs_usp_Lookupview_r_Disbursements");

				if (commandDisbursements == null) return null;
				
				commandDisbursements.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsDisbursements = new DataSet();
				dsDisbursements.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandDisbursements, dsDisbursements, "r_Disbursements");
				return dsDisbursements;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for DisbursementsDetails"
		public DataSet LookupDisbursementsDetails(string l_string_RefRequest_DisbursementIDFK)
		{
			DataSet dsDisbursementsDetails= null;
			Database db = null;
			DbCommand commandDisbursementsDetails = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandDisbursementsDetails = db.GetStoredProcCommand("yrs_usp_Lookupview_r_DisbursementsDetails");

				if (commandDisbursementsDetails == null) return null;
				db.AddInParameter(commandDisbursementsDetails,"@l_string_lcDisbursementIDFK", DbType.String, l_string_RefRequest_DisbursementIDFK);

				commandDisbursementsDetails.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsDisbursementsDetails = new DataSet();
				dsDisbursementsDetails.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandDisbursementsDetails, dsDisbursementsDetails, "DisbursementsDetails");
				return dsDisbursementsDetails;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for DisbursementsDetailsWithHolding"
		public DataSet LookupDisbursementsDetailsWithHolding(string l_string_RefRequest_DistPK)
		{
			DataSet dsDisbursementsDetailsWithHolding= null;
			Database db = null;
			DbCommand commandDisbursementsDetailsWithHolding = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandDisbursementsDetailsWithHolding = db.GetStoredProcCommand("yrs_usp_Lookupview_r_DisbursementsWithHolding");

				if (commandDisbursementsDetailsWithHolding == null) return null;
				db.AddInParameter(commandDisbursementsDetailsWithHolding,"@l_string_lcDistPK", DbType.String, l_string_RefRequest_DistPK);

				commandDisbursementsDetailsWithHolding.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsDisbursementsDetailsWithHolding = new DataSet();
				dsDisbursementsDetailsWithHolding.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandDisbursementsDetailsWithHolding, dsDisbursementsDetailsWithHolding, "DisbursementsDetailsWithHolding");
				return dsDisbursementsDetailsWithHolding;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for DisbursementsRefunds"
		public DataSet LookupDisbursementsRefunds()
		{
			DataSet dsDisbursementsRefunds= null;
			Database db = null;
			DbCommand commandDisbursementsRefunds = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandDisbursementsRefunds = db.GetStoredProcCommand("yrs_usp_Lookupview_r_DisbursementsRefund");

				if (commandDisbursementsRefunds == null) return null;
				
				commandDisbursementsRefunds.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsDisbursementsRefunds = new DataSet();
				dsDisbursementsRefunds.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandDisbursementsRefunds, dsDisbursementsRefunds, "DisbursementsRefunds");
				return dsDisbursementsRefunds;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for DisbursementsFunding"
		public DataSet LookupDisbursementsFunding()
		{
			DataSet dsDisbursementsFunding= null;
			Database db = null;
			DbCommand commandDisbursementsFunding = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandDisbursementsFunding = db.GetStoredProcCommand("yrs_usp_Lookupview_r_DisbursementsFunding");

				if (commandDisbursementsFunding == null) return null;
				
				commandDisbursementsFunding.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				dsDisbursementsFunding = new DataSet();
				dsDisbursementsFunding.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandDisbursementsFunding, dsDisbursementsFunding, "DisbursementsFunding");
				return dsDisbursementsFunding;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "FunsDisbursement"
		public bool UpdateFundDisbursement()
		{
			Database db = null;
			DbCommand commandUpdateFundDisbursement = null;
            //Added by Ashish for migration
            int l_RowsAffected = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return false;
		
				commandUpdateFundDisbursement = db.GetStoredProcCommand("yrs_usp_Update_fundDisbursement");
						
				if (commandUpdateFundDisbursement == null) return false;

				//commandUpdateRefRequests.AddInParameter("@int_UniqueId",DbType.Int32,l_int_UniqueID);
				commandUpdateFundDisbursement.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
                //TODOMIGRATION -need to verify this row affected chnages work or not
				//db.ExecuteNonQuery(commandUpdateFundDisbursement);
                l_RowsAffected=db.ExecuteNonQuery(commandUpdateFundDisbursement);
				//l_string_output = commandUpdateRefRequests.GetParameterValue("@varchar_Output").ToString();
				//if(commandUpdateFundDisbursement.RowsAffected > 0)
                if (l_RowsAffected > 0)
					return true; // update 
				else
					return false; //Update fail

															 
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		#endregion

	}
}
