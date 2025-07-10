using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCa-YRS
// FileName			:	CashApplicationDAClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	10/24/2005 5:03:21 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modified By			Date             Desription
//*******************************************************************************
//Ashish Srivastava		12-Jan-2009		 for getting personal details in a batch,Remove
//Ashish Srivastava		29-Jan-2009		 Get loan Defaulted personal details
//Shashank Patel		10-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
//Manthan Rajguru       2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for CashApplicationDaClass.
	/// </summary>
	public class CashApplicationDaClass
	{
		public CashApplicationDaClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpYmca(string parameterYMCANo, string parameterYMCAName, string parameterCity, string parameterState)
		{
			DataSet dsYmcas = null;
			Database db= null;
			DbCommand commandLookUpYmca = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_CA_SearchYMCAS");


				if (commandLookUpYmca==null) return null;
				dsYmcas = new DataSet();
				commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.AddInParameter(commandLookUpYmca,"@varchar_YmcaNo",DbType.String,parameterYMCANo);
				db.AddInParameter(commandLookUpYmca,"@varchar_YmcaName",DbType.String,parameterYMCAName);
				db.AddInParameter(commandLookUpYmca,"@varchar_City",DbType.String,parameterCity);
				db.AddInParameter(commandLookUpYmca,"@varchar_StateType",DbType.String,parameterState);
                				
				db.LoadDataSet(commandLookUpYmca,dsYmcas,"Ymcas");
				return dsYmcas;
			}
			catch 
			{
				throw ;
			}

		}

		public static DataSet LookUpYmcaReceipts(string parameterYmcaId)
		{
			DataSet dsYmcaReceipts = null;
			Database db= null;
			DbCommand commandLookUpYmca = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_CA_SearchYMCAReceipts");
				if (commandLookUpYmca==null) return null;
				dsYmcaReceipts = new DataSet();
				commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]);
                db.AddInParameter(commandLookUpYmca, "@varchar_UniqueId", DbType.String, parameterYmcaId);
			
				
				db.LoadDataSet(commandLookUpYmca,dsYmcaReceipts,"YmcaReceipts");
				return dsYmcaReceipts;
			}
			catch 
			{
				throw ;
			}

		}

		public static DataSet LookUpYmcaTransmittals(string parameterYmcaId)
		{
			DataSet dsYmcaTransmittals = null;
			Database db= null;
			DbCommand commandLookUpYmca = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_CA_SearchYMCATransmittals");
				if (commandLookUpYmca==null) return null;
				dsYmcaTransmittals = new DataSet();
				commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.AddInParameter(commandLookUpYmca,"@varchar_UniqueId",DbType.String,parameterYmcaId);
			
				
				db.LoadDataSet(commandLookUpYmca,dsYmcaTransmittals,"YmcaTransmittals");
				return dsYmcaTransmittals;
			}
			catch 
			{
				throw ;
			}

		}
/* Commented by Ashish on 21-May-2008
		public static DataSet LookUpYmcaInterest(string parameterYmcaId)
		{
			DataSet dsYmcaInterest = null;
			Database db= null;
			DbCommand commandLookUpYmca = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_CA_SearchYMCAInterests");
				if (commandLookUpYmca==null) return null;
				dsYmcaInterest = new DataSet();
				commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				commandLookUpYmca.AddInParameter("@varchar_UniqueId",DbType.String,parameterYmcaId);
			
				
				db.LoadDataSet(commandLookUpYmca,dsYmcaInterest,"YmcaInterest");
				return dsYmcaInterest;
			}
			catch 
			{
				throw ;
			}

		}
*/
		public static string LookUpYmcaCredit(string parameterYmcaId)
		{
			String l_string_CreditAmt  = "";
			Database db= null;
			DbCommand commandLookUpYmca = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_CA_SearchYMCACreditBalance");
				if (commandLookUpYmca==null) return null;
				commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.AddInParameter(commandLookUpYmca,"@varchar_UniqueId",DbType.String,parameterYmcaId);
				db.AddOutParameter(commandLookUpYmca,"@CreditAmount",DbType.Double,40);
				db.ExecuteNonQuery(commandLookUpYmca);
				l_string_CreditAmt = Convert.ToString(db.GetParameterValue(commandLookUpYmca,"@CreditAmount"));
				return l_string_CreditAmt;
			}
			catch 
			{
				throw ;
			}

		}
		

		public static DataSet GetAccountingDate()
		{
			
			Database db= null;
			DbCommand commandGetAcctDate = null;
			DataSet ds = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				commandGetAcctDate = db.GetStoredProcCommand("yrs_usp_QDRO_getAcctDate");
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
				commandGetAcctDate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
				if (commandGetAcctDate==null) return null;
				ds = new DataSet();
				db.LoadDataSet(commandGetAcctDate, ds,"AcctDate");
				
				return ds;
			}
			catch 
			{
				throw ;
			}

		}

		//public static string UpdateYmcaTransmittals(string parameterUniqueid,double parameterAmount)
		//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
		//Commented by Ashish -01-Dec-2008
		//public static string UpdateYmcaTransmittals(string parameterUniqueid,double parameterAmount,string parameterPaidDate,Database parameterDatabase,  DbTransaction parameterTransaction)
		public static string UpdateYmcaTransmittals(string parameterUniqueid,double parameterAmount,Database parameterDatabase,  DbTransaction parameterTransaction)
		{
			//commented  by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
			//Database db= null;
			//commented  by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
			DbCommand commandUpdate = null;
			string l_string_output = "";
		
			try
			{
				////commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				if (parameterDatabase == null) return string.Empty; 
				//db= DatabaseFactory.CreateDatabase("YRS");
				
				//if(db==null) return null;
				commandUpdate = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_UpdateYmcaTransmittals");
				if (commandUpdate==null) return null;
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
				commandUpdate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_UniqueId", DbType.String, parameterUniqueid);
                parameterDatabase.AddInParameter(commandUpdate, "@numeric_Amount", DbType.Double, parameterAmount);
				//Commented by Ashish on 01-Dec-2008 for dtsPaidDate
//				if(!parameterPaidDate.Equals(string.Empty))
//				{
//					commandUpdate.AddInParameter("@varchar_PaidDate",DbType.String ,parameterPaidDate);
//				}

                parameterDatabase.AddOutParameter(commandUpdate,"@int_output", DbType.Int32, 1);
			
				//db.ExecuteNonQuery(commandUpdate);
				parameterDatabase.ExecuteNonQuery(commandUpdate,parameterTransaction);
                l_string_output = parameterDatabase.GetParameterValue(commandUpdate, "@int_output").ToString();
				return l_string_output;
			}
			catch 
			{
				throw ;
			}

		}

		////commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
		//public static string InsertYmcaCredit(string parameterYmcaId,string parameterUniqueId,double parameterAmount,string parameterDate,string parameterAcctDate)
		public static string InsertYmcaCredit(string parameterYmcaId,string parameterUniqueId,double parameterAmount,string parameterDate,string parameterAcctDate,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			
			//Database db= null;
			DbCommand commandUpdate = null;
			string l_string_output = "";
		
			try
			{
				////commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				if (parameterDatabase == null) return string.Empty; 
				//db= DatabaseFactory.CreateDatabase("YRS");
				
				//if(db==null) return null;
				//commandUpdate = db.GetStoredProcCommand("yrs_usp_CA_InsertYmcaCredits");
				commandUpdate = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_InsertYmcaCredits");
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				if (commandUpdate==null) return null;
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
				commandUpdate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_YmcaId", DbType.String, parameterYmcaId);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_UniqueId", DbType.String, parameterUniqueId);
                parameterDatabase.AddInParameter(commandUpdate, "@numeric_Amount", DbType.Double, parameterAmount);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_Date", DbType.String, parameterDate);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_AcctDate", DbType.String, parameterAcctDate);


                parameterDatabase.AddOutParameter(commandUpdate,"@int_output", DbType.Int32, 1);
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db.ExecuteNonQuery(commandUpdate);
				parameterDatabase.ExecuteNonQuery(commandUpdate,parameterTransaction);
                l_string_output = parameterDatabase.GetParameterValue(commandUpdate,"@int_output").ToString();
				return l_string_output;
			}
			catch 
			{
				throw ;
			}

		}

//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
		//public static string InsertYmcaAppliedRcpts(string parameterYmcaId,string parameterRcptId,string parameterTransmittalId,double parameterAmount,string parameterDate)
		public static string InsertYmcaAppliedRcpts(string parameterYmcaId,string parameterRcptId,string parameterTransmittalId,double parameterAmount,string parameterDate,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
			//Database db= null;
			DbCommand commandUpdate = null;
			string l_string_output = "";
		
			try
			{
				////commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db= DatabaseFactory.CreateDatabase("YRS");
				
				//if(db==null) return null;
				if (parameterDatabase == null) return string.Empty; 
				commandUpdate = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_InsertYmcaAppliedReceipts");
				if (commandUpdate==null) return null;
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
				commandUpdate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_YmcaId", DbType.String, parameterYmcaId);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_RcptId", DbType.String, parameterRcptId);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_TransmittalId", DbType.String, parameterTransmittalId);
                parameterDatabase.AddInParameter(commandUpdate, "@numeric_Amount", DbType.Double, parameterAmount);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_Date", DbType.String, parameterDate);



                parameterDatabase.AddOutParameter(commandUpdate, "@int_output", DbType.Int32, 1);
				parameterDatabase.ExecuteNonQuery(commandUpdate,parameterTransaction);
                l_string_output = parameterDatabase.GetParameterValue(commandUpdate, "@int_output").ToString();
				return l_string_output;
			}
			catch 
			{
				throw ;
			}

		}

		//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
		//public static string UpdateYmcaRcpts(string parameterUniqueId,string parameterTransmittalId,string parameterDate,string parameterAcctDate)
		public static string UpdateYmcaRcpts(string parameterUniqueId,string parameterTransmittalId,string parameterDate,string parameterAcctDate,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
			//Database db= null;
			DbCommand commandUpdate = null;
			string l_string_output = "";
		
			try
			{
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db= DatabaseFactory.CreateDatabase("YRS");
				
				//if(db==null) return null;
					if (parameterDatabase == null) return string.Empty; 
				commandUpdate = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_UpdateAtsYmcaRcpts");
				if (commandUpdate==null) return null;
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
				commandUpdate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_UniqueId", DbType.String, parameterUniqueId);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_TransmittalId", DbType.String, parameterTransmittalId);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_Date", DbType.String, parameterDate);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_AcctDate", DbType.String, parameterAcctDate);



                parameterDatabase.AddOutParameter(commandUpdate, "@int_output", DbType.Int32, 1);
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db.ExecuteNonQuery(commandUpdate);
				parameterDatabase.ExecuteNonQuery(commandUpdate,parameterTransaction);
                l_string_output = parameterDatabase.GetParameterValue(commandUpdate, "@int_output").ToString();
				return l_string_output;
			}
			catch 
			{
				throw ;
			}

		}

		//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
		//public static string InsertYMCACreditsOvrPay(string parameterYmcaId,string parameterTransmittalId,double parameterAmount,string parameterReceivedDate,string parameterReceivedAcctDate,string parameterYmcaRcptId)
		public static string InsertYMCACreditsOvrPay(string parameterYmcaId,string parameterTransmittalId,double parameterAmount,string parameterReceivedDate,string parameterReceivedAcctDate,string parameterYmcaRcptId,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
			//Database db= null;
			DbCommand commandUpdate = null;
			string l_string_output = "";
		
			try
			{
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db= DatabaseFactory.CreateDatabase("YRS");
				
				//if(db==null) return null;
					if (parameterDatabase == null) return string.Empty; 
				commandUpdate = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_InsertAtsYmcaCreditsOvrPay");
				if (commandUpdate==null) return null;
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
				commandUpdate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_YmcaId", DbType.String, parameterYmcaId);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_TransmittalId", DbType.String, parameterTransmittalId);
                parameterDatabase.AddInParameter(commandUpdate, "@numeric_Amount", DbType.Double, parameterAmount);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_ReceivedDate", DbType.String, parameterReceivedDate);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_ReceivedAcctDate", DbType.String, parameterReceivedAcctDate);
                parameterDatabase.AddInParameter(commandUpdate, "@varchr_YmcaRcptId", DbType.String, parameterYmcaRcptId);



                parameterDatabase.AddOutParameter(commandUpdate,"@int_output", DbType.Int32, 1);
				//db.ExecuteNonQuery(commandUpdate);
				parameterDatabase.ExecuteNonQuery(commandUpdate,parameterTransaction);
                l_string_output = parameterDatabase.GetParameterValue(commandUpdate,"@int_output").ToString();
				return l_string_output;
			}
			catch 
			{
				throw ;
			}

		}

//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
		//public static string UpdatePaymentInterest(string parameterTransmittalId,string parameterDate,string parameterAcctDate,string parameterUniqueId)
		public static string UpdatePaymentInterest(string parameterTransmittalId,string parameterDate,string parameterAcctDate,string parameterUniqueId,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
			//Database db= null;
			DbCommand commandUpdate = null;
			string l_string_output = "";
		
			try
			{
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db= DatabaseFactory.CreateDatabase("YRS");
				
				//if(db==null) return null;
				if (parameterDatabase == null) return string.Empty; 
				commandUpdate = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_UpdatePaymentInterest");
				if (commandUpdate==null) return null;
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
				commandUpdate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_TransmittalId", DbType.String, parameterTransmittalId);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_UsedDate", DbType.String, parameterDate);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_AcctDate", DbType.String, parameterAcctDate);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_UniqueId", DbType.String, parameterUniqueId);



                parameterDatabase.AddOutParameter(commandUpdate,"@int_output", DbType.Int32, 1);
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db.ExecuteNonQuery(commandUpdate);
				parameterDatabase.ExecuteNonQuery(commandUpdate,parameterTransaction);
                l_string_output = parameterDatabase.GetParameterValue(commandUpdate,"@int_output").ToString();
				return l_string_output;
			}
			catch 
			{
				throw ;
			}

		}

//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
		//public static string InsertYmcaCreditsRcpts(string parameterYmcaId,string parameterTransmittalId,double parameterAmount,string parameterDate,string parameterAcctDate,string parameterYmcaRcptId)
		public static string InsertYmcaCreditsRcpts(string parameterYmcaId,string parameterTransmittalId,double parameterAmount,string parameterDate,string parameterAcctDate,string parameterYmcaRcptId,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations			
			//Database db= null;
			DbCommand commandUpdate = null;
			string l_string_output = "";
		
			try
			{
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db= DatabaseFactory.CreateDatabase("YRS");
				
				//if(db==null) return null;
				if (parameterDatabase == null) return string.Empty; 
				commandUpdate = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_InsertYmcaCreditsRcpts");
				if (commandUpdate==null) return null;
				
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
				commandUpdate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 
				//Vipul 20 Dec 06 Timeout Gemini Issue # 3006 
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_YmcaId", DbType.String, parameterYmcaId);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_TransmittalId", DbType.String, parameterTransmittalId);
                parameterDatabase.AddInParameter(commandUpdate, "@numeric_Amount", DbType.Double, parameterAmount);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_ReceivedDate", DbType.String, parameterDate);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_ReceivedAcctDate", DbType.String, parameterAcctDate);
				
				//start of comment by hafiz on 28Jan06
				//commandUpdate.AddInParameter("@varchar_UniqueId",DbType.String, parameterYmcaRcptId);
				//end of comment by hafiz on 28Jan06
				
				//start of code add by hafiz on 28Jan06
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_YmcaRcptId", DbType.String, parameterYmcaRcptId);
				//end of code add by hafiz on 28Jan06

                parameterDatabase.AddOutParameter(commandUpdate, "@int_output", DbType.Int32, 1);
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db.ExecuteNonQuery(commandUpdate);
				parameterDatabase.ExecuteNonQuery(commandUpdate,parameterTransaction);
                l_string_output = parameterDatabase.GetParameterValue(commandUpdate, "@int_output").ToString();
				return l_string_output;
			}
			catch 
			{
				throw ;
			}

		}

		//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations

		//public static DataSet SelectPersonDetails(string parameterTransmittalId)
		public static DataSet SelectPersonDetails(Int64 parameterFundedTransmittalLogId,Database parameterDatabase,DbTransaction parameterTransaction)
		{
			DataSet dsPersonDetails = null;
			//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
			//Database db= null;
			//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
			DbCommand commandLookUpYmca = null;

			try
			{
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db= DatabaseFactory.CreateDatabase("YRS");
				
				//if(db==null) return null;
				if (parameterDatabase == null) return null; 
				commandLookUpYmca = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_SelectPersonDetails");
				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				if (commandLookUpYmca==null) return null;
				dsPersonDetails = new DataSet();				

				commandLookUpYmca.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 			
				parameterDatabase.AddInParameter(commandLookUpYmca,"@ProcessId",DbType.Int64,parameterFundedTransmittalLogId);
				//Added by Ashish 27-Jan-2009
				string []stTableNames={"LoanpaidPersonDetails","LoanDefaultedPersonDetails"};

				//commented and added by Shubhrata Dec 26th 2006 YREN 3006 to maintain transacations
				//db.LoadDataSet(commandLookUpYmca,dsPersonDetails,"PersonDetails");
				parameterDatabase.LoadDataSet(commandLookUpYmca,dsPersonDetails,stTableNames,parameterTransaction);
				return dsPersonDetails;
			}
			catch 
			{
				throw ;
			}

		}
		#region Commented By Ashish on 16 MAy 2008
		//Shubhrata YREN 3006 Dec 22nd 2006- to maintain transacation
		/*
		public static DataTable SaveTransmittals(DataTable parameterDataTableTransmittals, DataRow parameterDataRowReceipts, DataRow parameterDataRowInterest, double parameterTotAppliedRcpts, double parameterDoubleTotInterest, string parameterStringAccountingDate)
		{
			//for maintaining transaction
			Database l_Database = null;
			DbConnection l_IDbConnection = null;
			DbTransaction l_IDbTransaction = null;
			
			DataSet l_Dataset = new DataSet();
			DataTable l_datatable = null;
			double l_double_PayAmount = 0.0;
			double l_double_TotalPaid = 0.0;
			double l_double_InterestPaid = 0.0;
			string l_string_Output = "";
			string l_string_UniqueId = "";
			
			bool l_bool_flag = true;
			
			DataRow l_datarow_transmittal = null;

			DateTime l_string_Date; 
			l_string_Date =System.DateTime.Now;
			try
			{
				l_Database = DatabaseFactory.CreateDatabase ("YRS");
				l_IDbConnection =  l_Database.CreateConnection();
				l_IDbConnection.Open ();

				if (l_Database == null) return null; 
				
				l_IDbTransaction = l_IDbConnection.BeginTransaction ();
							
			
				for (int i = 0; i <= parameterDataTableTransmittals.Rows.Count - 1; i++) 
				{ 
					l_datarow_transmittal = parameterDataTableTransmittals.Rows[i];

					if (Convert.ToBoolean(l_datarow_transmittal["Slctd"]) == true) 
					{ 
						l_string_UniqueId = l_datarow_transmittal["UniqueId"].ToString();
						
						l_double_PayAmount = Convert.ToDouble(l_datarow_transmittal["TotAppliedRcpts"]) + Convert.ToDouble(l_datarow_transmittal["TotAppliedCredit"]) + Convert.ToDouble(l_datarow_transmittal["TotAppliedInterest"]); 
						
						l_string_Output = UpdateYmcaTransmittals(l_string_UniqueId, l_double_PayAmount,l_Database,l_IDbTransaction); 
						if (l_string_Output != "0") 
						{ 
							throw new Exception("Error while saving in yrs_usp_CA_UpdateYmcaTransmittals");
						} 

						if ((Convert.ToDecimal(l_datarow_transmittal["TotAppliedCredit"]) - Convert.ToDecimal(l_datarow_transmittal["OrgAppliedCredit"]) > 0)) 
						{ 							
							l_string_Output = InsertYmcaCredit(l_datarow_transmittal["YmcaId"].ToString(), l_string_UniqueId, -1 * (Convert.ToDouble(l_datarow_transmittal["TotAppliedCredit"]) - Convert.ToDouble(l_datarow_transmittal["OrgAppliedCredit"])), Convert.ToString(l_string_Date),parameterStringAccountingDate,l_Database,l_IDbTransaction); 
							if (l_string_Output != "0") 
							{ 
								throw new Exception("Error while saving in yrs_usp_CA_InsertYmcaCredits");
							} 
						} 

						if ((Convert.ToDecimal(l_datarow_transmittal["TotAppliedRcpts"]) - Convert.ToDecimal(l_datarow_transmittal["OrgAppliedRcpts"]) > 0)) 
						{ 
							l_string_Output = InsertYmcaAppliedRcpts(l_datarow_transmittal["YmcaId"].ToString(), parameterDataRowReceipts["UniqueId"].ToString(), l_string_UniqueId, (Convert.ToDouble(l_datarow_transmittal["TotAppliedRcpts"]) - Convert.ToDouble(l_datarow_transmittal["OrgAppliedRcpts"])),Convert.ToString(l_string_Date),l_Database,l_IDbTransaction); 
							if (l_string_Output != "0") 
							{ 
								throw new Exception("Error while saving in yrs_usp_CA_InsertYmcaAppliedReceipts");
							} 
						} 

						l_double_TotalPaid = l_double_TotalPaid + (Convert.ToDouble(l_datarow_transmittal["TotAppliedRcpts"]) - Convert.ToDouble(l_datarow_transmittal["OrgAppliedRcpts"])); 

						l_double_InterestPaid = l_double_InterestPaid + (Convert.ToDouble(l_datarow_transmittal["TotAppliedInterest"]) - Convert.ToDouble(l_datarow_transmittal["OrgAppliedInterest"])); 
						
						l_Dataset = SelectPersonDetails(l_string_UniqueId,l_Database,l_IDbTransaction); 
						if (l_bool_flag == true) 
						{ 
							l_datatable = l_Dataset.Tables[0].Clone(); 
							l_bool_flag = false; 
						} 
						if (l_Dataset.Tables[0].Rows.Count > 0) 
						{ 
							l_datatable.ImportRow(l_Dataset.Tables[0].Rows[0]); 
							l_datatable.AcceptChanges(); 
						} 
					} 
				}//loop end

				if (l_string_UniqueId == null)
				{
					l_string_UniqueId = "";
				}

				//if string id is null then set it to ""
				if (parameterTotAppliedRcpts > 0) 
				{ 
					if (parameterDataRowReceipts != null)
					{
						l_string_Output = UpdateYmcaRcpts(parameterDataRowReceipts["UniqueId"].ToString(), l_string_UniqueId, Convert.ToString(l_string_Date), parameterStringAccountingDate, l_Database, l_IDbTransaction); 
						if (l_string_Output != "0") 
						{ 
							throw new Exception("Error while saving in yrs_usp_CA_UpdateAtsYmcaRcpts");
						} 

						if (Convert.ToDecimal(parameterDataRowReceipts["Amount"]) > 0) 
						{ 
							l_string_Output = YMCARET.YmcaDataAccessObject.CashApplicationDaClass.InsertYMCACreditsOvrPay(parameterDataTableTransmittals.Rows[0]["YmcaId"].ToString(), l_string_UniqueId, Convert.ToDouble(parameterDataRowReceipts["Amount"]), Convert.ToString(l_string_Date), parameterStringAccountingDate, parameterDataRowReceipts["UniqueId"].ToString(), l_Database, l_IDbTransaction); 
							if (l_string_Output != "0") 
							{ 
								throw new Exception("Error while saving in yrs_usp_CA_InsertAtsYmcaCreditsOvrPay");
							} 
						} 
					}
				}
									
				if (parameterDoubleTotInterest > 0)
				{ 
					if (parameterDataRowInterest != null) 
					{ 
						l_string_Output = UpdatePaymentInterest(l_string_UniqueId, Convert.ToString(l_string_Date), parameterStringAccountingDate, Convert.ToString(parameterDataRowInterest["UniqueId"]), l_Database, l_IDbTransaction); 
						if (l_string_Output != "0") 
						{ 
							throw new Exception("Error while saving in yrs_usp_CA_UpdatePaymentInterest");
						} 					 

						if (Convert.ToDecimal(parameterDataRowInterest["Amount"]) > 0) 
						{ 
							l_string_Output = InsertYmcaCreditsRcpts(parameterDataTableTransmittals.Rows[0]["YmcaId"].ToString(), l_string_UniqueId, Convert.ToDouble(parameterDataRowInterest["Amount"]), Convert.ToString(l_string_Date), parameterStringAccountingDate, Convert.ToString(parameterDataRowInterest["UniqueId"]),l_Database,l_IDbTransaction); 
							if (l_string_Output != "0") 
							{ 
								throw new Exception("Error while saving in yrs_usp_CA_InsertYmcaCreditsRcpts");
							} 
						} 
					}
				}				

				l_IDbTransaction.Commit();
				return l_datatable;

			}
			catch (Exception ex)
			{
				//Rollback
				if (l_IDbTransaction != null)
				{
					l_IDbTransaction.Rollback ();
				}
				throw ex;
			}
			finally
			{
				if (l_IDbConnection != null)
				{
					if (l_IDbConnection.State != ConnectionState.Closed)
					{
						l_IDbConnection.Close ();
					}
				}
			}
		}
		//Shubhrata YREN 3006 Dec 22nd 2006- to maintain transacation
		*/
		#endregion
		public static DataSet SaveTransmittals(DataTable parameterDataTableTransmittals, DataRow parameterDataRowReceipts, double parameterTotAppliedRcpts, string parameterStringAccountingDate,DataTable paraDataTableFundedTransmittalLog,ref Int64 fundedTransmittalLogId)
		{
			//for maintaining transaction
			Database l_Database = null;
			DbConnection l_IDbConnection = null;
			DbTransaction l_IDbTransaction = null;
			
			DataSet l_dsPersonalDetails =null;
			//Commented by Ashish 29-Jan-2009
			//DataTable l_dtPersonalDetails = null;
			double l_double_PayAmount = 0.0;
			double l_double_TotalPaid = 0.0;
			//double l_double_InterestPaid = 0.0;
			string l_string_Output = "";
			string l_string_UniqueId = "";
			string l_string_PaidDate=string.Empty ;
			//bool l_bool_flag = true;
			bool l_bool_fundedFlag = false;

			
			DataRow l_datarow_transmittal = null;

			DateTime l_string_Date; 
			l_string_Date =System.DateTime.Now;
			UEINDAClass objUEINDAClass=null;
			
			try
			{
				l_Database = DatabaseFactory.CreateDatabase ("YRS");
				l_IDbConnection =  l_Database.CreateConnection();
				l_IDbConnection.Open ();

				if (l_Database == null) return null; 
				
				l_IDbTransaction = l_IDbConnection.BeginTransaction ();
							
			
				for (int i = 0; i <= parameterDataTableTransmittals.Rows.Count - 1; i++) 
				{ 
					l_datarow_transmittal = parameterDataTableTransmittals.Rows[i];

					if (Convert.ToBoolean(l_datarow_transmittal["Slctd"]) == true) 
					{ 
						l_string_UniqueId = l_datarow_transmittal["UniqueId"].ToString();
						
						l_double_PayAmount = Convert.ToDouble(l_datarow_transmittal["TotAppliedRcpts"]) + Convert.ToDouble(l_datarow_transmittal["TotAppliedCredit"]) ; 
						
						if(Math.Round(Convert.ToDecimal(l_datarow_transmittal["Balance"]),2)==0 && l_datarow_transmittal["FundedDate"].ToString()!=string.Empty && l_datarow_transmittal["dtmPaidDate"].ToString()!=string.Empty )
						{
							l_string_PaidDate=l_datarow_transmittal["dtmPaidDate"].ToString();
							l_bool_fundedFlag=true;

						}
						// Commented by Ashish on 01-Dec-2008
						//l_string_Output = UpdateYmcaTransmittals(l_string_UniqueId, l_double_PayAmount,l_string_PaidDate,l_Database,l_IDbTransaction); 
						l_string_Output = UpdateYmcaTransmittals(l_string_UniqueId, l_double_PayAmount,l_Database,l_IDbTransaction); 
						if (l_string_Output != "0") 
						{ 
							throw new Exception("Error while saving in yrs_usp_CA_UpdateYmcaTransmittals");
						} 

						if ((Convert.ToDecimal(l_datarow_transmittal["TotAppliedCredit"]) - Convert.ToDecimal(l_datarow_transmittal["OrgAppliedCredit"]) > 0)) 
						{ 							
							l_string_Output = InsertYmcaCredit(l_datarow_transmittal["YmcaId"].ToString(), l_string_UniqueId, -1 * (Convert.ToDouble(l_datarow_transmittal["TotAppliedCredit"]) - Convert.ToDouble(l_datarow_transmittal["OrgAppliedCredit"])), Convert.ToString(l_string_Date),parameterStringAccountingDate,l_Database,l_IDbTransaction); 
							if (l_string_Output != "0") 
							{ 
								throw new Exception("Error while saving in yrs_usp_CA_InsertYmcaCredits");
							} 
						} 

						if ((Convert.ToDecimal(l_datarow_transmittal["TotAppliedRcpts"]) - Convert.ToDecimal(l_datarow_transmittal["OrgAppliedRcpts"]) > 0)) 
						{ 
							l_string_Output = InsertYmcaAppliedRcpts(l_datarow_transmittal["YmcaId"].ToString(), parameterDataRowReceipts["UniqueId"].ToString(), l_string_UniqueId, (Convert.ToDouble(l_datarow_transmittal["TotAppliedRcpts"]) - Convert.ToDouble(l_datarow_transmittal["OrgAppliedRcpts"])),Convert.ToString(l_string_Date),l_Database,l_IDbTransaction); 
							if (l_string_Output != "0") 
							{ 
								throw new Exception("Error while saving in yrs_usp_CA_InsertYmcaAppliedReceipts");
							} 
						} 
//						// update fundeddate
//						if(Convert.ToDecimal(l_datarow_transmittal["Balance"])==0 && l_datarow_transmittal["FundedDate"].ToString()!=string.Empty)
//						{
//							UpdateTransactionFundedDate(l_string_UniqueId,Convert.ToDateTime(l_datarow_transmittal["FundedDate"]).Date,l_Database,l_IDbTransaction); 
//						}
						l_double_TotalPaid = l_double_TotalPaid + (Convert.ToDouble(l_datarow_transmittal["TotAppliedRcpts"]) - Convert.ToDouble(l_datarow_transmittal["OrgAppliedRcpts"])); 

						//l_double_InterestPaid = l_double_InterestPaid + (Convert.ToDouble(l_datarow_transmittal["TotAppliedInterest"]) - Convert.ToDouble(l_datarow_transmittal["OrgAppliedInterest"])); 
						//commented by Ashish on 13-Jan-2009
//						l_Dataset = SelectPersonDetails(l_string_UniqueId,l_Database,l_IDbTransaction); 
//						if (l_bool_flag == true) 
//						{ 
//							l_datatable = l_Dataset.Tables[0].Clone(); 
//							l_bool_flag = false; 
//						} 
//						if (l_Dataset.Tables[0].Rows.Count > 0) 
//						{ 
//							l_datatable.ImportRow(l_Dataset.Tables[0].Rows[0]); 
//							l_datatable.AcceptChanges(); 
//						} 
					} 
				}//loop end

				if (l_string_UniqueId == null)
				{
					l_string_UniqueId = "";
				}

				//if string id is null then set it to ""
				if (parameterTotAppliedRcpts > 0) 
				{ 
					if (parameterDataRowReceipts != null)
					{
						l_string_Output = UpdateYmcaRcpts(parameterDataRowReceipts["UniqueId"].ToString(), l_string_UniqueId, Convert.ToString(l_string_Date), parameterStringAccountingDate, l_Database, l_IDbTransaction); 
						if (l_string_Output != "0") 
						{ 
							throw new Exception("Error while saving in yrs_usp_CA_UpdateAtsYmcaRcpts");
						} 

						if (Convert.ToDecimal(parameterDataRowReceipts["Amount"]) > 0) 
						{ 
							l_string_Output = YMCARET.YmcaDataAccessObject.CashApplicationDaClass.InsertYMCACreditsOvrPay(parameterDataTableTransmittals.Rows[0]["YmcaId"].ToString(), l_string_UniqueId, Convert.ToDouble(parameterDataRowReceipts["Amount"]), Convert.ToString(l_string_Date), parameterStringAccountingDate, parameterDataRowReceipts["UniqueId"].ToString(), l_Database, l_IDbTransaction); 
							if (l_string_Output != "0") 
							{ 
								throw new Exception("Error while saving in yrs_usp_CA_InsertAtsYmcaCreditsOvrPay");
							} 
						} 
					}
				}
				//Save FundedTransmittal Log
				objUEINDAClass=new UEINDAClass();
				if(paraDataTableFundedTransmittalLog!=null)
				{
					if(paraDataTableFundedTransmittalLog.Rows.Count >0)
					{
						fundedTransmittalLogId=objUEINDAClass.SaveTransmittalFunding(null,"Cash Application",paraDataTableFundedTransmittalLog,"CASH",l_Database,l_IDbTransaction); 
						//Call UEIN routine
						if(l_bool_fundedFlag && fundedTransmittalLogId !=0)
						{
							l_string_Output=objUEINDAClass.GenerateUEIN(fundedTransmittalLogId,Convert.ToDateTime(l_string_PaidDate).Date,l_Database,l_IDbTransaction);  
							if (l_string_Output.ToString()!=string.Empty ) 
							{ 
								throw new Exception(l_string_Output);
							} 
							l_dsPersonalDetails=SelectPersonDetails(fundedTransmittalLogId,l_Database,l_IDbTransaction);
							//Commented by Ashish 29-Jan-2009
//							if(l_dsPersonalDetails!=null)
//							{
//								if(l_dsPersonalDetails.Tables.Count >0)
//								{
//									l_dtPersonalDetails=l_dsPersonalDetails.Tables["PersonDetails"];
//								}
//							}

						}
					}
				}
//				if(parameterDataTableNewTransaction.Rows.Count > 0 &&  parameterDataTableNewTransmittal.Rows.Count >0)
//				{
//					UEINDAClass ueinDAClass=new UEINDAClass();
//					ueinDAClass.SaveNewTransactRecords(parameterDataTableNewTransaction,l_Database,l_IDbTransaction); 
//					ueinDAClass.SaveNewTransmittalRecords( parameterDataTableNewTransmittal,l_Database, l_IDbTransaction);
// 
//				}	
			
//				if (parameterDoubleTotInterest > 0)
//				{ 
//					if (parameterDataRowInterest != null) 
//					{ 
//						l_string_Output = UpdatePaymentInterest(l_string_UniqueId, Convert.ToString(l_string_Date), parameterStringAccountingDate, Convert.ToString(parameterDataRowInterest["UniqueId"]), l_Database, l_IDbTransaction); 
//						if (l_string_Output != "0") 
//						{ 
//							throw new Exception("Error while saving in yrs_usp_CA_UpdatePaymentInterest");
//						} 					 
//
//						if (Convert.ToDecimal(parameterDataRowInterest["Amount"]) > 0) 
//						{ 
//							l_string_Output = InsertYmcaCreditsRcpts(parameterDataTableTransmittals.Rows[0]["YmcaId"].ToString(), l_string_UniqueId, Convert.ToDouble(parameterDataRowInterest["Amount"]), Convert.ToString(l_string_Date), parameterStringAccountingDate, Convert.ToString(parameterDataRowInterest["UniqueId"]),l_Database,l_IDbTransaction); 
//							if (l_string_Output != "0") 
//							{ 
//								throw new Exception("Error while saving in yrs_usp_CA_InsertYmcaCreditsRcpts");
//							} 
//						} 
//					}
//				}				

				l_IDbTransaction.Commit();
				return l_dsPersonalDetails;

			}
			catch (Exception ex)
			{
				//Rollback
				if (l_IDbTransaction != null)
				{
					l_IDbTransaction.Rollback ();
				}
				throw ex;
			}
			finally
			{
				if (l_IDbConnection != null)
				{
					if (l_IDbConnection.State != ConnectionState.Closed)
					{
						l_IDbConnection.Close ();
					}
				}
				l_IDbConnection=null;
				l_IDbTransaction=null;
				l_Database=null;
			}
		}
		//Shubhrata YREN 3006 Dec 22nd 2006- to maintain transacation
		
		public static void UpdateTransactionFundedDate(string parameterTransmittalId,DateTime parameterFundedDate,Database parameterDatabase,DbTransaction parameterIDbTransaction)
		{
			DbCommand commandUpdate = null;
			try
			{
				
				commandUpdate=parameterDatabase.GetStoredProcCommand("yrs_usp_CA_UpdateTransactFundedDate");
				commandUpdate.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;
                parameterDatabase.AddInParameter(commandUpdate, "@datetime_fundedDate", DbType.DateTime, parameterFundedDate);
                parameterDatabase.AddInParameter(commandUpdate, "@varchar_guiTransmittalId", DbType.String, parameterTransmittalId);
				parameterDatabase.ExecuteNonQuery(commandUpdate,parameterIDbTransaction); 

 
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				commandUpdate=null;
			}
		}

		//Shashank Patel		10-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
		#region Cash Application - Person
		

		/// <summary>
		/// This method returns participant based on selcted YMCA
		/// </summary>
		/// <param name="parameterStrGuiYmcaID">GuiYmcaID</param>
		/// <param name="parameterStrSSN">StrSSN</param>
		/// <param name="parameterStrFundNo">StrFundNo</param>
		/// <param name="parameterStrFirstName">StrFirstName</param>
		/// <param name="parameterStrLastName">StrLastName</param>
		/// <returns></returns>
		public static DataSet GetParticipantsByYmcaID(string parameterStrGuiYmcaID, string parameterStrSSN, string parameterStrFundNo, string parameterStrFirstName, string parameterStrLastName)
		{
			DataSet dsPerson = null;
			Database db = null;
			DbCommand commandLookUpYmca = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;
				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_CA_SearchPersonInYMCATransmittals");


				if (commandLookUpYmca == null) return null;
				dsPerson = new DataSet();
				commandLookUpYmca.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				db.AddInParameter(commandLookUpYmca, "@varchar_UniqueId", DbType.String, parameterStrGuiYmcaID);
				db.AddInParameter(commandLookUpYmca, "@varchar_SSN", DbType.String, parameterStrSSN);
				db.AddInParameter(commandLookUpYmca, "@varchar_FundIDNo", DbType.String, parameterStrFundNo);
				db.AddInParameter(commandLookUpYmca, "@varchar_FName", DbType.String, parameterStrFirstName);
				db.AddInParameter(commandLookUpYmca, "@varchar_LName", DbType.String, parameterStrLastName);

				db.LoadDataSet(commandLookUpYmca, dsPerson, "Person");
				return dsPerson;
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// This method returns the all transmittal (only Un-Funded) In which based on person's GuiFundEventID
		/// </summary>
		/// <param name="parameterStrGuiYmcaID">GuiYmcaID</param>
		/// <param name="parameterStrGuiFundEventID">GuiFundEventID</param>
		/// <returns></returns>
		public static DataSet GetTransmittalsByFundID(string parameterStrGuiYmcaID, string parameterStrGuiFundEventID)
		{
			DataSet dsTransmittal = null;
			Database db = null;
			DbCommand dbcommand = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;
				dbcommand = db.GetStoredProcCommand("yrs_usp_CA_SearchYMCATransmittalsByFundId");


				if (dbcommand == null) return null;
				dsTransmittal = new DataSet();
				dbcommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				db.AddInParameter(dbcommand, "@guiYmcaID", DbType.String, parameterStrGuiYmcaID);
				db.AddInParameter(dbcommand, "@guiFundEventID", DbType.String, parameterStrGuiFundEventID);

				db.LoadDataSet(dbcommand, dsTransmittal, "Transmittal");
				return dsTransmittal;
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// This method returns the transaction based on transmittal GuiUniqueID and GuiFundEventID
		/// </summary>
		/// <param name="parameterStrGuiYmcaID">GuiYmcaID</param>
		/// <param name="parameterStrGuiFundEventID">GuiFundEventID</param>
		/// <param name="parameterStrGuiTransmittalID">GuiTransmittalID</param>
		/// <returns></returns>
		public static DataSet GetTransactionsByTransmittalID(string parameterStrGuiYmcaID, string parameterStrGuiFundEventID, string parameterStrGuiTransmittalID)
		{
			DataSet dsTransaction = null;
			Database db = null;
			DbCommand dbcommand = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;
				dbcommand = db.GetStoredProcCommand("yrs_usp_CA_SearchTransactionsByTransmittalID");


				if (dbcommand == null) return null;
				dsTransaction = new DataSet();
				dbcommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				db.AddInParameter(dbcommand, "@guiYmcaID", DbType.String, parameterStrGuiYmcaID);
				db.AddInParameter(dbcommand, "@guiFundEventID", DbType.String, parameterStrGuiFundEventID);
				db.AddInParameter(dbcommand, "@guiTransmittalID", DbType.String, parameterStrGuiTransmittalID);

				db.LoadDataSet(dbcommand, dsTransaction, "Transaction");
				return dsTransaction;
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// This method is returns the transmittals detail Header information like mnypaid amount,total transmittal amount etc.
		/// </summary>
		/// <param name="parameterStrGuiTransmittalID">GuiTransmittalID</param>
		/// <returns>Dataset </returns>
		public static DataSet GetTransmittalDetailsByTransmittalID(string parameterStrGuiTransmittalID)
		{
			DataSet dsTransaction = null;
			Database db = null;
			DbCommand dbcommand = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;
				dbcommand = db.GetStoredProcCommand("yrs_usp_CA_GetTransmittalsBalanceDetails");


				if (dbcommand == null) return null;
				dsTransaction = new DataSet();
				dbcommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				db.AddInParameter(dbcommand, "@guiTransmittalID", DbType.String, parameterStrGuiTransmittalID);

				db.LoadDataSet(dbcommand, dsTransaction, "Transaction");
				return dsTransaction;
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// This method update the mnypaid amount in atsymcatransamittal table
		/// </summary>
		/// <param name="parameterStrGuiTransmittalid"></param>
		/// <param name="parameterDblAmount"></param>
		/// <param name="parameterDatabase"></param>
		/// <param name="parameterDbTransaction"></param>
		/// <returns></returns>
		public static string UpdateYmcaTransmittalAmount(string parameterStrGuiTransmittalid, double parameterDblAmount, Database parameterDatabase, DbTransaction parameterDbTransaction)
		{
			
			DbCommand commandUpdate = null;
			string strOutput = "";

			try
			{
				
				if (parameterDatabase == null) return string.Empty;

				commandUpdate = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_UpdateYmcaTransmittalAmount");
				if (commandUpdate == null) return null;
				commandUpdate.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				parameterDatabase.AddInParameter(commandUpdate, "@guiTransmittalId", DbType.String, parameterStrGuiTransmittalid);
				parameterDatabase.AddInParameter(commandUpdate, "@amount", DbType.Double, parameterDblAmount);

				parameterDatabase.AddOutParameter(commandUpdate, "@output", DbType.Int32, 1);

				parameterDatabase.ExecuteNonQuery(commandUpdate, parameterDbTransaction);
				strOutput = parameterDatabase.GetParameterValue(commandUpdate, "@output").ToString();
				return strOutput;
			}
			catch
			{
				throw;
			}

		}
		/// <summary>
		/// This method is used to fund the transaction for a person in a 
		/// transmittal and also generate UEIN & Update Vesting serivce
		/// </summary>
		/// <param name="parameterDataTableTransmittals"></param>
		/// <param name="parameterDataRowReceipts"></param>
		/// <param name="parameterDataTableTransactions"></param>
		/// <param name="parameterdoubleTotAppliedRcpts"></param>
		/// <param name="parameterStringAccountingDate"></param>
		/// <param name="paraDataTableFundedTransmittalLog"></param>
		/// <param name="fundedIntTransmittalLogId"></param>
		/// <returns></returns>
		public static DataSet SaveTransmittals(DataTable parameterDataTableTransmittals, DataRow parameterDataRowReceipts, DataTable parameterDataTableTransactions, double parameterdoubleTotAppliedRcpts, string parameterStringAccountingDate, DataTable paraDataTableFundedTransmittalLog, ref Int64 fundedIntTransmittalLogId)
		{
			//for maintaining transaction
			Database dbDatabase = null;
			DbConnection dbConnection = null;
			DbTransaction dbTransaction = null;

			DataSet dsPersonalDetails = null;
			double dblPayAmount = 0.0;
			string strOutput = "";
			string strGuiTransmittalId = "";
			string strPaidDate = string.Empty;
			bool bfundedFlag = false;


			DataRow drTransmittal = null;

			DateTime dtmTodayDate;
			dtmTodayDate = System.DateTime.Now;
			UEINDAClass objUEINDAClass = null;

			try
			{
				dbDatabase = DatabaseFactory.CreateDatabase("YRS");
				dbConnection = dbDatabase.CreateConnection();
				dbConnection.Open();

				if (dbDatabase == null) return null;

				dbTransaction = dbConnection.BeginTransaction();


					drTransmittal = parameterDataTableTransmittals.Rows[0];

					if (Convert.ToBoolean(drTransmittal["Slctd"]) == true)
					{
						strGuiTransmittalId = drTransmittal["UniqueId"].ToString();

						dblPayAmount = Convert.ToDouble(drTransmittal["TotAppliedRcpts"]) + Convert.ToDouble(drTransmittal["TotAppliedCredit"]) ;

						strPaidDate = drTransmittal["dtmPaidDate"].ToString();
						bfundedFlag = true;

						if (dblPayAmount > 0)
						{
							strOutput = UpdateYmcaTransmittalAmount(strGuiTransmittalId, dblPayAmount, dbDatabase, dbTransaction);
							if (strOutput != "0")
							{
								throw new Exception("Error while saving in yrs_usp_CA_UpdateYmcaTransmittalAmount");
							}
						}
						if ((Convert.ToDecimal(drTransmittal["TotAppliedCredit"]) >0 ))// - Convert.ToDecimal(l_datarow_transmittal["OrgAppliedCredit"]) > 0))
						{
							strOutput = InsertYmcaCredit(drTransmittal["YmcaId"].ToString(), strGuiTransmittalId, -1 * (Convert.ToDouble(drTransmittal["TotAppliedCredit"])) , Convert.ToString(dtmTodayDate), parameterStringAccountingDate, dbDatabase, dbTransaction);
							if (strOutput != "0")
							{
								throw new Exception("Error while saving in yrs_usp_CA_InsertYmcaCredits");
							}
						}

						if ((Convert.ToDecimal(drTransmittal["TotAppliedRcpts"]) >0)) //- Convert.ToDecimal(l_datarow_transmittal["OrgAppliedRcpts"]) > 0))
						{
							strOutput = InsertYmcaAppliedRcpts(drTransmittal["YmcaId"].ToString(), parameterDataRowReceipts["UniqueId"].ToString(), strGuiTransmittalId, (Convert.ToDouble(drTransmittal["TotAppliedRcpts"])), Convert.ToString(dtmTodayDate), dbDatabase, dbTransaction);
							if (strOutput != "0")
							{
								throw new Exception("Error while saving in yrs_usp_CA_InsertYmcaAppliedReceipts");
							}
						}

					}
						
				//'update applied receipt bit =1 and insert overpay credit amount
				if (parameterdoubleTotAppliedRcpts > 0)
				{
					if (parameterDataRowReceipts != null)
					{
						strOutput = UpdateYmcaRcpts(parameterDataRowReceipts["UniqueId"].ToString(), strGuiTransmittalId, Convert.ToString(dtmTodayDate), parameterStringAccountingDate, dbDatabase, dbTransaction);
						if (strOutput != "0")
						{
							throw new Exception("Error while saving in yrs_usp_CA_UpdateAtsYmcaRcpts");
						}

						if (Convert.ToDecimal(parameterDataRowReceipts["Amount"]) > 0)
						{
							strOutput = YMCARET.YmcaDataAccessObject.CashApplicationDaClass.InsertYMCACreditsOvrPay(parameterDataTableTransmittals.Rows[0]["YmcaId"].ToString(), strGuiTransmittalId, Convert.ToDouble(parameterDataRowReceipts["Amount"]), Convert.ToString(dtmTodayDate), parameterStringAccountingDate, parameterDataRowReceipts["UniqueId"].ToString(), dbDatabase, dbTransaction);
							if (strOutput != "0")
							{
								throw new Exception("Error while saving in yrs_usp_CA_InsertAtsYmcaCreditsOvrPay");
							}
						}
					}
				}
				//Save FundedTransmittal Log
				objUEINDAClass = new UEINDAClass();
				if (paraDataTableFundedTransmittalLog != null)
				{
					if (paraDataTableFundedTransmittalLog.Rows.Count > 0)
					{
						//Save the Fund Detail header
						fundedIntTransmittalLogId = objUEINDAClass.SaveTransmittalFunding(null, "Cash Application", paraDataTableFundedTransmittalLog.Rows[0],parameterDataTableTransactions, "CASH", dbDatabase, dbTransaction);
						
						//Call UEIN routine
						if (bfundedFlag && fundedIntTransmittalLogId != 0)
						{
							//Generate UEIN
							strOutput = objUEINDAClass.GenerateUEINForPerson(fundedIntTransmittalLogId, Convert.ToDateTime(strPaidDate).Date, dbDatabase, dbTransaction);
							
							if (strOutput.ToString() != string.Empty)
							{
								throw new Exception(strOutput);
							}
							//Select funded Loan transaction person details for sending email.
							dsPersonalDetails = SelectIndividualPersonDetails(fundedIntTransmittalLogId, dbDatabase, dbTransaction);

						}
					}
				}
				dbTransaction.Commit();
				return dsPersonalDetails;

			}
			catch (Exception ex)
			{
				//Rollback
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
				dbConnection = null;
				dbTransaction = null;
				dbDatabase = null;
			}
		}

		/// <summary>
		/// This method returns the loan paid off (ie, re-paid loan) and default loan paid person's details in current transmittal
		/// </summary>
		/// <param name="parameterIntFundedTransmittalLogId"></param>
		/// <param name="parameterDatabase"></param>
		/// <param name="parameterDbTransaction"></param>
		/// <returns></returns>
		public static DataSet SelectIndividualPersonDetails(Int64 parameterIntFundedTransmittalLogId, Database parameterDatabase, DbTransaction parameterDbTransaction)
		{
			DataSet dsPersonDetails = null;
			DbCommand dbCommand = null;

			try
			{
				if (parameterDatabase == null) return null;
				dbCommand = parameterDatabase.GetStoredProcCommand("yrs_usp_CA_SelectIndividualPersonDetails");
				if (dbCommand == null) return null;
				dsPersonDetails = new DataSet();

				dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				parameterDatabase.AddInParameter(dbCommand, "@ProcessId", DbType.Int64, parameterIntFundedTransmittalLogId);
				string[] stTableNames = { "LoanpaidPersonDetails", "LoanDefaultedPersonDetails" };

				parameterDatabase.LoadDataSet(dbCommand, dsPersonDetails, stTableNames, parameterDbTransaction);
				return dsPersonDetails;
			}
			catch
			{
				throw;
			}

		}

		#endregion
		//Shashank Patel		10-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal 
	}
}
