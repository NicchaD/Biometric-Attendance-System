//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	PaymentManagerDAClass.cs
// Author Name		:	Ragesh V.P
// Employee ID		:	34231	
// Email			:	ragesh_vp@3i-infotech.com
// Contact No		:	8736
// Creation Time	:	10/21/2005 11:55:28 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	
//*******************************************************************************
//Modification History
//*******************************************************************************
//Date          Modified by          Description
//*******************************************************************************
//05/08/2009    Amit			     Phase V Changes
//2009-11-26    Shashi Shekhar	     Chenges in getdisbursementswithoutfunding function.
//01/07/2010    Shashi               Migration related changes in Function InsertDisbursementFileTypes.
//28.09.2012	Priya Patil		     BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//02.07.2013	Shashank Patel	     BT:1456/YRS 5.0-1735:Read current active address at time of disbursement funding
//10.12.2013	Shashank Patel	     BT:2323/YRS 5.0-2266 - Data error appeard "Datatype nvarchar to numeric conversion error." 
//2015.09.16    Manthan Rajguru      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2018.04.06    Sanjay GS Rawat      YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//2018.04.11    Pramod P. Pokale     YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
//2018.11.16    Vinayan C            YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//2019.10.07    Sanjay GS(Rawat)     YRS-AT-4601 - YRS enh: State Withholding Project - Export file First Annuity Payroll 
//2019.10.07    Megha Lad		     YRS-AT-4601 - YRS enh: State Withholding Project - Export file First Annuity Payroll 
//2019.12.26    Megha Lad            YRS-AT-4676 - State Withholding - Validations for exporting file from Payment Manager (First Annuities)
//*******************************************************************************

using System;
//using System.IO;
using System.Data;
using System.Data.Common;
//using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Common;
//using System.Collections; 
//using System.Globalization;
using System.Collections.Generic; // SR | 2018.10.10 | adding reference for send Email & exception logging.

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for PaymentManagerDAClass.
	/// </summary>
	public class PaymentManagerDAClass
	{
		public PaymentManagerDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataTable CurrentCheckSeriesAll()
		{
			DataSet dsCurrentCheckSeries = null;
			DataTable dataTableCurrentCheckSeries;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_PM_CheckSeriesALL");
				if (getCommandWrapper == null) return null;
				dsCurrentCheckSeries = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsCurrentCheckSeries,"CheckSeries");
				dataTableCurrentCheckSeries = dsCurrentCheckSeries.Tables[0];
				return dataTableCurrentCheckSeries;
			}
			catch 
			{
				throw;
			}
		}

		public  static DataSet getDisbursementType()
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			DataSet l_DataSet = null;
			string [] l_TableNames;
			       
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
				if (l_DataBase == null) return null;
				//Phase V Changes-start
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_PM_GetDisbursementDetails");
				//Phase V Changes-start
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return null;
				l_DataSet = new DataSet ("GetDisbursementDetails");
				l_TableNames = new string [] {"disbursementtypes","AccoutDate"};
				l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet,l_TableNames);
				return l_DataSet;
			}
			catch
			{    
				throw;
			}
				
		}

		public static DataTable DisbursementOutputFileInfo(string parameter_string_disbursementId)
		{
			DataSet dsDisbursementOutputFileInfo = null;
			DataTable dataTableDisbursementOutputFileInfo;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				//proc changed by ruchi
				//getCommandWrapper = db.GetStoredProcCommandWrapper("dbo.ap_DisbursementOutputFileInfo");
				getCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_DisbursementOutputFileInfo");
				getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(getCommandWrapper,"@DisbursementID",DbType.String,parameter_string_disbursementId);
				if (getCommandWrapper == null) return null;
				dsDisbursementOutputFileInfo = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsDisbursementOutputFileInfo,"DisbursementOutputFileInfo");
				dataTableDisbursementOutputFileInfo = dsDisbursementOutputFileInfo.Tables[0];
				return dataTableDisbursementOutputFileInfo;
			}
			catch 
			{
				throw;
			}
		}

		public static DataTable DisbursementOutputFileAddlInfo(string parameter_string_disbursementId)
		{
			DataSet dsDisbursementOutputFileAddlInfo = null;
			DataTable dataTableDisbursementOutputFileAddlInfo;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_PM_DisbursementOutputFileAddlInfo");
				getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(getCommandWrapper,"@guiUniqueID",DbType.String,parameter_string_disbursementId);
				if (getCommandWrapper == null) return null;
						
				dsDisbursementOutputFileAddlInfo = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsDisbursementOutputFileAddlInfo,"DisbursementOutputFileAddlInfo");
				dataTableDisbursementOutputFileAddlInfo = dsDisbursementOutputFileAddlInfo.Tables[0];
				
				return dataTableDisbursementOutputFileAddlInfo;
			}
			catch 
			{
				throw;
			}
		}

		public static DataTable WithholdingsByDisbursement(string parameter_string_disbursementId)
		{
			DataSet dsWithholdingsByDisbursement = null;
			DataTable dataTableWithholdingsByDisbursement;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand ("dbo.ap_WithholdingsByDisbursement");
				getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(getCommandWrapper,"@DisbursementID",DbType.String,parameter_string_disbursementId);
				
				if (getCommandWrapper == null) return null;
						
				dsWithholdingsByDisbursement = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsWithholdingsByDisbursement,"WithholdingsByDisbursement");
				dataTableWithholdingsByDisbursement = dsWithholdingsByDisbursement.Tables[0];
				
				return dataTableWithholdingsByDisbursement;
			}
			catch 
			{
				throw;
			}
		}


		public static DataTable DisbursementDetailsbyRefundDisbursement(string parameter_string_disbursementId)
		{
			DataSet dsDisbursementDetailsbyRefundDisbursement = null;
			DataTable dataTableDisbursementDetailsbyRefundDisbursement;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
					//ap_DisbursementDetailsbyRefundDisbursement
				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_PM_DisbursementDetailsbyRefundDisbursement");
				getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(getCommandWrapper,"@DisbursementID",DbType.String,parameter_string_disbursementId);
				
				if (getCommandWrapper == null) return null;
						
				dsDisbursementDetailsbyRefundDisbursement = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsDisbursementDetailsbyRefundDisbursement,"DisbursementDetailsbyRefundDisbursement");
				dataTableDisbursementDetailsbyRefundDisbursement = dsDisbursementDetailsbyRefundDisbursement.Tables[0];
				
				return dataTableDisbursementDetailsbyRefundDisbursement;
			}
			catch 
			{
				throw;
			}
		}

		public static DataTable DisbursementWithholdingsbyRefundDisbursement (string parameter_string_disbursementId)
		{
			DataSet dsDisbursementWithholdingsbyRefundDisbursement = null;
			DataTable dataTableDisbursementWithholdingsbyRefundDisbursement;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.ap_DisbursementWithholdingsbyRefundDisbursement");
				getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(getCommandWrapper,"@DisbursementID",DbType.String,parameter_string_disbursementId);
				
				if (getCommandWrapper == null) return null;
						
				dsDisbursementWithholdingsbyRefundDisbursement = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsDisbursementWithholdingsbyRefundDisbursement,"DisbursementWithholdingsbyRefundDisbursement");
				dataTableDisbursementWithholdingsbyRefundDisbursement = dsDisbursementWithholdingsbyRefundDisbursement.Tables[0];
				
				return dataTableDisbursementWithholdingsbyRefundDisbursement;
			}
			catch 
			{
				throw;
			}
		}

		public static DataTable DisbursementRefundDeductions (string parameter_string_disbursementId)
		{
			DataSet dsDisbursementDeductions = null;
			DataTable dataTableDisbursementDeductions;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_PM_DisbursementDeductions");
				getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(getCommandWrapper,"@DisbursementID",DbType.String,parameter_string_disbursementId);
				
				if (getCommandWrapper == null) return null;
						
				dsDisbursementDeductions = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsDisbursementDeductions,"DisbursementDeductions");
				dataTableDisbursementDeductions = dsDisbursementDeductions.Tables[0];
				
				return dataTableDisbursementDeductions;
			}
			catch 
			{
				throw;
			}
		}


//		public  static DataSet getdisbursementswithoutfunding(string DisbursementType,string CheckBoxRefundWithDrawal)
//		{
//			Database l_DataBase = null;
//			DBCommandWrapper l_DBCommandWrapper = null;			
//			DataSet l_DataSet = null;
//			string [] l_TableNames;
//			       
//			try
//			{
//				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
//		
//				if (l_DataBase == null) return null;
//				//Phase V Changes-start
//				//changed the proc name		
//				//l_DBCommandWrapper = l_DataBase.GetStoredProcCommandWrapper("dbo.yrs_usp_PM_DisbursementsWithoutfunding");
//				l_DBCommandWrapper = l_DataBase.GetStoredProcCommandWrapper("dbo.yrs_usp_PM_loadDisbursements");
//				l_DBCommandWrapper.AddInParameter("@disbursementType",DbType.String,DisbursementType);
//				l_DBCommandWrapper.AddInParameter("@checkRefundWithDrawal",DbType.String,CheckBoxRefundWithDrawal);
//							
//					 
//				//Phase V Changes-start
//				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["LargeConnectionTimeOut"]);
//
//				if (l_DBCommandWrapper == null) return null;
//
//				l_DataSet = new DataSet ("loadDisbursements");
//								
//				l_TableNames = new string [] {"Disbursements", "DisbursementsNegative","DisbursementREPL"};// "Withhoidings",//
//				l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet, l_TableNames);
//									
//				return l_DataSet;
//				
//			}
//			catch
//			{    
//				throw;
//			}
//				
//		}

		//Added for Bug Id 786-Amit
		//28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 add new parameter for shira
        public static DataSet getdisbursementswithoutfunding(string DisbursementType, string checkRefundWithDrawalAll, string checkRefundWithDrawalHard, string checkRefundWithDrawalDeath, string checkRefundWithDrawalShira, string PaymentMethod) // SR | 2018.04.11 | YRS-AT-3101 | Added new parameter as PaymenType
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			DataSet l_DataSet = null;
			string [] l_TableNames;
			       
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
		
				if (l_DataBase == null) return null;
				//Phase V Changes-start
				//changed the proc name		
				//l_DBCommandWrapper = l_DataBase.GetStoredProcCommandWrapper("dbo.yrs_usp_PM_DisbursementsWithoutfunding");
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_PM_loadDisbursements");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@disbursementType", DbType.String, DisbursementType);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@checkRefundWithDrawalAll", DbType.String, checkRefundWithDrawalAll);
				l_DataBase.AddInParameter(l_DBCommandWrapper, "@checkRefundWithDrawalHard", DbType.String, checkRefundWithDrawalHard);
				//28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 add new parameter for shira
				l_DataBase.AddInParameter(l_DBCommandWrapper, "@checkRefundWithDrawalShira", DbType.String, checkRefundWithDrawalShira);
				l_DataBase.AddInParameter(l_DBCommandWrapper, "@checkRefundWithDrawalDeath", DbType.String, checkRefundWithDrawalDeath);
                
                // START | SR : 2018.04.11 | YRS-AT-3101 | A new parameter added to get disbursement record based on payment method code.
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@VARCHAR_PaymentType", DbType.String, PaymentMethod);
                // END | SR : 2018.04.11 | YRS-AT-3101 | A new parameter added to get disbursement record based on payment method code.
							
					 
				//Phase V Changes-start
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

				if (l_DBCommandWrapper == null) return null;

				l_DataSet = new DataSet ("loadDisbursements");
				
				
				 //Shashi Shekhar:2009-11-25 - Modified below code to return all disbursement data in one table 
				    //	l_TableNames = new string [] {"Disbursements", "DisbursementsNegative","DisbursementREPL"};// "Withhoidings",//
					l_TableNames = new string [] {"Disbursements", "DisbursementsNegative"};// "Withhoidings",//
				//End:Shashi Shekhar

				    l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet, l_TableNames);
									
				return l_DataSet;
				
			}
			catch
			{    
				throw;
			}
				
		}


		public  static DataSet GetOutPutFileforDisbursal()
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			DataSet l_DataSet = null;
			string [] l_TableNames;
			       
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
		
				if (l_DataBase == null) return null;
				

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_PM_GetOutPutFileforDisbursal");
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

				if (l_DBCommandWrapper == null) return null;

				l_DataSet = new DataSet ("DisbursementsWithoutfunding");
								
				l_TableNames = new string [] {"DisbursementFiles","OutPutfilePath"};
				
				l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet, l_TableNames);
									
				return l_DataSet;
				
			}
			catch
			{    
				throw;
			}
				
		}
		public  static DataTable GetfedtaxwithholdingsRO(string parameter_string_persid)
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			DataSet l_DataSet = null;
			string [] l_TableNames;
			       
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
		
				if (l_DataBase == null) return null;
				

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_PM_GetfedtaxwithholdingsRO");
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				l_DataBase.AddInParameter(l_DBCommandWrapper,"@Persid",DbType.String,parameter_string_persid);
				

				if (l_DBCommandWrapper == null) return null;

				l_DataSet = new DataSet ("DisbursementsWithoutfunding");
								
				l_TableNames = new string [] {"FedTaxWithholdingRO"};
				
				l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet, l_TableNames);
									
				return l_DataSet.Tables["FedTaxWithholdingRO"] ;
				
			}
			catch
			{    
				throw;
			}
				
		}


		public  static DataTable GetErroneousDisbursements(DateTime parameter_Date_created)
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			DataSet l_DataSet = null;
			string [] l_TableNames;
			       
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
		
				if (l_DataBase == null) return null;
				

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_PM_GetErroneousDisbursements");
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				l_DataBase.AddInParameter(l_DBCommandWrapper,"@dateTimeCreated",DbType.DateTime,parameter_Date_created);
				
				if (l_DBCommandWrapper == null) return null;

				l_DataSet = new DataSet ("ErroneousDisbursements");
								
				l_TableNames = new string [] {"ErroneousDisbursements"};
				
				l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet, l_TableNames);
									
				return l_DataSet.Tables["ErroneousDisbursements"] ;
				
			}
			catch
			{    
				throw;
			}
				
		}


		public static void InsertPaymentManagerExceptionLogs(string parameter_string_paramExpr1,string paramter_string_paramExpr2,string parameter_string_paramExpr3,string parameter_string_paramExpr4)
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase != null) 
				{

					l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("DBO.yrs_usp_PM_InsertManagerExceptionLogs");
					l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

					if (l_DBCommandWrapper != null) 
					{
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@chvProcessCode", DbType.String, parameter_string_paramExpr1);
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@guiKey", DbType.String, paramter_string_paramExpr2);
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@chvKeyCode", DbType.String, parameter_string_paramExpr3);
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@chvDescription", DbType.String, parameter_string_paramExpr4);
						l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
					}
				}
							
			}
			catch 
			{
				throw;
			}
		}

		public static bool UpdateRefundsAfterCheckRrun()
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;
			DbConnection DBconnectYRS;
			bool l_bool_TransactionStarted = false;
			DbTransaction DBTransaction;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");
				if (l_DataBase == null)  return false;
                DBconnectYRS = l_DataBase.CreateConnection();//.GetConnection();
				DBconnectYRS.Open ();
				DBTransaction =  DBconnectYRS.BeginTransaction (System.Data.IsolationLevel.ReadUncommitted);
				l_bool_TransactionStarted = true;
				try
				{

					if (l_DataBase != null) 
					{

						l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("Dbo.yrs_usp_PM_UpdateRefundsAfterCheckRrun");
						l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

						if (l_DBCommandWrapper != null) 
						{
						
							l_DataBase.ExecuteNonQuery(l_DBCommandWrapper,DBTransaction);
							DBTransaction.Commit(); 
							DBconnectYRS.Close(); 
							l_bool_TransactionStarted = false;
						}
					
					}

					return true;
									
				}
				catch (Exception ex)
				{
					if (l_bool_TransactionStarted) 
					{
						
						DBTransaction.Rollback(); 
						DBconnectYRS.Close(); 
						throw new Exception(ex.Message); 
					}
					throw;
				}
			}

			catch
			{
				throw;
			}

		}


		//Method to fetch the withholding details for TDLOAN
		// added by ruchi on 15th May,2006
		public static DataTable WithholdingDetailByTDLoanDisbursement(string parameter_string_disbursementId)
		{
			DataSet dsWithholdingDetailByTDLoanDisbursement = null;
			DataTable dataTableWithholdingDetailByTDLoanDisbursement;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_PM_GetLoanWithholding");
				getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(getCommandWrapper,"@varchar_DisbursementId",DbType.String,parameter_string_disbursementId);
				
				if (getCommandWrapper == null) return null;
						
				dsWithholdingDetailByTDLoanDisbursement = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsWithholdingDetailByTDLoanDisbursement,"DisbursementDetailsbyRefundDisbursement");
				dataTableWithholdingDetailByTDLoanDisbursement = dsWithholdingDetailByTDLoanDisbursement.Tables[0];
				
				return dataTableWithholdingDetailByTDLoanDisbursement;
			}
			catch 
			{
				throw;
			}
		}
		//	02.07.2013 SP: BT:1456/YRS 5.0-1735:Read current active address at time of disbursement funding -Start
		public static bool UpdateDisbursementAddressID(string stDisbursementID)
		{
			Database db = null;
			DbCommand dbCommand = null;
			int output;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				output = 0;
				if (db == null) return false;
				dbCommand = db.GetStoredProcCommand("yrs_usp_PM_UpdateDisbursementAddressID");

				//dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(dbCommand, "@guiDisbursementID", DbType.String, stDisbursementID);

				if (dbCommand == null) return false;


				output = db.ExecuteNonQuery(dbCommand);

				if (output > 0)
					return true;
				else
					return false;
			}
			catch
			{
				throw;
			}
		}
		//	02.07.2013 SP: BT:1456/YRS 5.0-1735:Read current active address at time of disbursement funding -End

		// START : SR | 2018.04.11 | YRS-AT-3101 | This method Retrieve Disbursement type for EFT payment method.
        public static DataSet GetEFTDisbursementType()
        {
            Database db = null;
            DbCommand Command = null;
            DataSet DisbursementType = null;
            string[] TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;               
                Command = db.GetStoredProcCommand("dbo.yrs_usp_PM_GetEFTDisbursementTypes");               
                Command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (Command == null) return null;
                DisbursementType = new DataSet("GetDisbursementDetails");
                TableNames = new string[] { "disbursementtypes" };
                db.LoadDataSet(Command, DisbursementType, TableNames);
                return DisbursementType;
            }
            catch
            {
                throw;
            }

        }
		// END : SR | 2018.04.11 | YRS-AT-3101 | This method Retrieve Disbursement type for EFT payment method.

		//START: PPP | 04/11/2018 | YRS-AT-3101 | Provides EFT disbursement details based on given status
        public static DataTable GetEFTDisbursements(string type, string status)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_PM_GetEFTDisbursements");
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(cmd, "@VARCHAR_DisbursementType", DbType.String, type);
                db.AddInParameter(cmd, "@VARCHAR_DisbursementEFTStatus", DbType.String, status);
                if (cmd == null) return null;

                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "EFTDisbursements");
                return ds.Tables["EFTDisbursements"];
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
        //END: PPP | 04/11/2018 | YRS-AT-3101 | Provides EFT disbursement details based on given status


	}
	public class PaymentManagerDAClassWrapperClass
	{
		Database l_DataBase = null;
		DbConnection DBconnectYRS;
		bool l_bool_TransactionStarted = false;
		DbTransaction DBTransaction;

		public PaymentManagerDAClassWrapperClass()
		{
			try
			{
				this.l_DataBase = DatabaseFactory.CreateDatabase("YRS");
				this.DBconnectYRS = l_DataBase.CreateConnection();
				this.DBconnectYRS.Open();
				this.DBTransaction = DBconnectYRS.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
				this.l_bool_TransactionStarted = true;
			}
			catch
			{
				throw;
			}

		}


		public bool InsertDisbursementFunding(string parameter_strting_DisbursementID,
													 string parameter_strting_DisbursementNumber,
													 double parameter_double_Amount,
													 DateTime parameter_datetime_IssuedDate,
													 string parameter_strting_InstrumentTypeCode,
													 string parameter_string_CheckSeries,
													 string parameter_string_CheckNbr,
													 string parameter_string_disbursementType,
													 bool parameter_bool_Fund,
														bool parameter_bool_bitPaid = false) //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
		{

			DbCommand l_DBCommandWrapper = null;


			try
			{
				l_DBCommandWrapper = this.l_DataBase.GetStoredProcCommand("Dbo.yrs_usp_PM_DisbursementFunding");
				l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

				if (l_DBCommandWrapper != null)
				{
					l_DataBase.AddInParameter(l_DBCommandWrapper, "@guiDisbursementID", DbType.String, parameter_strting_DisbursementID);
					l_DataBase.AddInParameter(l_DBCommandWrapper, "@DisbursementNumber", DbType.String, parameter_strting_DisbursementNumber);
				  //l_DataBase.AddInParameter(l_DBCommandWrapper, "@mnyAmount", DbType.String, parameter_double_Amount);//SP :2013.12.10  BT-2323/YRS 5.0-2266 - Commented
					l_DataBase.AddInParameter(l_DBCommandWrapper, "@mnyAmount", DbType.Decimal, parameter_double_Amount);//SP :2013.12.10  BT-2323/YRS 5.0-2266 - (changed the datatype from string to decimal)
					l_DataBase.AddInParameter(l_DBCommandWrapper, "@dtsIssuedDate", DbType.String, parameter_datetime_IssuedDate);
					l_DataBase.AddInParameter(l_DBCommandWrapper, "@chvInstrumentTypeCode", DbType.String, parameter_strting_InstrumentTypeCode);

					//l_DataBase.UpdateDataSet(DatasetdisbursementFileTypes, "DisbursementFiles", InsertDBCommandWrapper, UpdateDBCommandWrapper, DeleteDBCommandWrapper, UpdateBehavior.Standard);
					//PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
					l_DataBase.AddInParameter(l_DBCommandWrapper, "@bitPaid", DbType.Boolean, parameter_bool_bitPaid);

					l_DataBase.ExecuteNonQuery(l_DBCommandWrapper, this.DBTransaction);
				}

				//Not Update the checkseries values if the manual check has been raised.

				if (parameter_bool_Fund == false)
				{
					l_DBCommandWrapper = this.l_DataBase.GetStoredProcCommand("dbo.yrs_usp_Payroll_UpdateCheckSeriesVal");
					l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

					if (l_DBCommandWrapper != null)
					{
						l_DataBase.AddInParameter(l_DBCommandWrapper, "@intCurrentCheckNumber", DbType.String, parameter_string_CheckNbr);
						l_DataBase.AddInParameter(l_DBCommandWrapper, "@ChrCheckSeriesCode", DbType.String, parameter_string_CheckSeries);

						l_DataBase.ExecuteNonQuery(l_DBCommandWrapper, this.DBTransaction);
					}
				}

				//Insert the disbursment Relations.
				//InsertDisbursementFileTypes(parameter_Dataset_disbursementFileTypes,l_DataBase);

				if (parameter_string_disbursementType.Trim() == "ANN")
				{
					// Spress Payroll

					l_DBCommandWrapper = this.l_DataBase.GetStoredProcCommand("Dbo.yrs_usp_PM_Update_AnnuitiesSuppressPayroll");
					l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

					if (l_DBCommandWrapper != null)
					{
						l_DataBase.AddInParameter(l_DBCommandWrapper, "@guiDisbursementID", DbType.String, parameter_strting_DisbursementID);
						this.l_DataBase.ExecuteNonQuery(l_DBCommandWrapper, this.DBTransaction);
					}

				}


				if (parameter_string_disbursementType.Trim() == "TDLOAN")
				{
					// Spress Payroll

					l_DBCommandWrapper = this.l_DataBase.GetStoredProcCommand("Dbo.yrs_usp_PM_LoanSuppress");
					l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

					if (l_DBCommandWrapper != null)
					{
						l_DataBase.AddInParameter(l_DBCommandWrapper, "@guiDisbursementID", DbType.String, parameter_strting_DisbursementID);
						this.l_DataBase.ExecuteNonQuery(l_DBCommandWrapper, this.DBTransaction);
					}

				}

				return true;


			}
			catch (Exception ex)
			{
				if (this.l_bool_TransactionStarted)
				{

					this.DBTransaction.Rollback();
					this.DBconnectYRS.Close();
					this.l_bool_TransactionStarted = false;

				}
				throw new Exception(ex.Message);

			}


		}

		public bool InsertDisbursementFileTypes(DataSet DatasetdisbursementFileTypes)
		{
			//Database l_DataBase = null;

			DbCommand InsertDBCommandWrapper = null;
			DbCommand UpdateDBCommandWrapper = null;
			DbCommand DeleteDBCommandWrapper = null;

			try
			{
				//l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (DatasetdisbursementFileTypes == null) return false;

				if (l_DataBase == null) return false;

				InsertDBCommandWrapper = this.l_DataBase.GetStoredProcCommand("dbo.yrs_usp_Payroll_InsertDisbursementFiles");
				InsertDBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

				if (InsertDBCommandWrapper != null)
				{
					//Shashi Shekhar:01-07-2010:DataType change for migration related issues.
					l_DataBase.AddInParameter(InsertDBCommandWrapper, "@DisbursementID", DbType.Guid, "DisbursementID", DataRowVersion.Current);
					l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileID", DbType.Guid, "GuiUniqueID", DataRowVersion.Current);

					this.l_DataBase.UpdateDataSet(DatasetdisbursementFileTypes, "DisbursementFiles", InsertDBCommandWrapper, UpdateDBCommandWrapper, DeleteDBCommandWrapper, UpdateBehavior.Standard);

				}
				else
				{
					return false;
				}

				InsertDBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_Payroll_InsertDisbursementOutputFiles");
				InsertDBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

				if (InsertDBCommandWrapper != null)
				{
					l_DataBase.AddInParameter(InsertDBCommandWrapper, "@UniqueID", DbType.Guid, "GuiUniqueID", DataRowVersion.Current);
					l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileLocation", DbType.String, "chvOutputFileName", DataRowVersion.Current);
					l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileName", DbType.String, "chvOutputFileName", DataRowVersion.Current);
					l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileType", DbType.String, "chrOutputFileType", DataRowVersion.Current);

					this.l_DataBase.UpdateDataSet(DatasetdisbursementFileTypes, "OutPutfilePath", InsertDBCommandWrapper, UpdateDBCommandWrapper, DeleteDBCommandWrapper, UpdateBehavior.Standard);

				}
				else
				{
					throw new Exception("Error");
				}

				DBTransaction.Commit();
				DBconnectYRS.Close();
				this.l_bool_TransactionStarted = false;

				return true;

			}
			catch (Exception ex)
			{
				if (this.l_bool_TransactionStarted)
				{

					this.DBTransaction.Rollback();
					this.DBconnectYRS.Close();
					this.l_bool_TransactionStarted = false;
					throw new Exception(ex.Message);
				}
				throw new Exception(ex.Message);

			}
		}

		public void RevertTransactions()
		{
			try
			{
				this.DBTransaction.Rollback();
				this.DBconnectYRS.Close();
				this.l_bool_TransactionStarted = false;

			}
			catch
			{
				//Donot do Anything.
			}

		}


		public DataTable YTDDisbursementsByPayee(string parameter_string_persid, DateTime parameter_int32_EffectiveYear, string parameter_string_DisbursementType)
		{
			DataSet dsYTDDisbursementsByPayee = null;
			DataTable dataTableYTDDisbursementsByPayee;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;
				//Commented by aparna due to change in proc called -to include Experience dividends - 18/10/2006

				//getCommandWrapper = db.GetStoredProcCommandWrapper("dbo.ap_YTDDisbursementsByPayee");
				getCommandWrapper = db.GetStoredProcCommand("yrs_usp_PM_YTDDisbursementsByPayee");
				getCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(getCommandWrapper, "@EffectiveYear", DbType.Int32, parameter_int32_EffectiveYear.Year);
				db.AddInParameter(getCommandWrapper, "@PayeeID", DbType.String, parameter_string_persid);
				db.AddInParameter(getCommandWrapper, "@DisbursementType", DbType.String, parameter_string_DisbursementType);
				if (getCommandWrapper == null) return null;

				dsYTDDisbursementsByPayee = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsYTDDisbursementsByPayee, "YTDDisbursementsByPayee", this.DBTransaction);
				dataTableYTDDisbursementsByPayee = dsYTDDisbursementsByPayee.Tables[0];

				return dataTableYTDDisbursementsByPayee;
			}
			catch
			{
				throw;
			}
		}


		public DataTable YTDWithholdingsByPayee(string parameter_string_persid, DateTime parameter_int32_EffectiveYear, string parameter_string_DisbursmentType)
		{
			DataSet dsYTDWithholdingsByPayee = null;
			DataTable dataTableYTDWithholdingsByPayee;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;
				//APARNA -commented due to change in proc-18/10/2006
				//getCommandWrapper = db.GetStoredProcCommandWrapper("dbo.ap_YTDWithholdingsByPayee");
				getCommandWrapper = db.GetStoredProcCommand("yrs_usp_PM_YTDWithholdingsByPayee");

				getCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				db.AddInParameter(getCommandWrapper, "@EffectiveYear", DbType.Int32, parameter_int32_EffectiveYear.Year);
				db.AddInParameter(getCommandWrapper, "@PayeeID", DbType.String, parameter_string_persid);
				db.AddInParameter(getCommandWrapper, "@DisbursmentType", DbType.String, parameter_string_DisbursmentType);
				if (getCommandWrapper == null) return null;

				dsYTDWithholdingsByPayee = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsYTDWithholdingsByPayee, "YTDWithholdingsByPayee", this.DBTransaction);
				dataTableYTDWithholdingsByPayee = dsYTDWithholdingsByPayee.Tables[0];

				return dataTableYTDWithholdingsByPayee;
			}
			catch
			{
				throw;
			}
		}


		public bool Property_bool_TransactionStarted
		{
			get
			{
				return l_bool_TransactionStarted;
			}
			set
			{
				l_bool_TransactionStarted = value;
			}
		}

        // START: SR | 2018.04.06 | YRS-AT-3101 - Generate EFT batch id
        /// <summary>
        /// Generates Next EFT Batch Id for EFT Disbursement(s) tracking. 
        /// </summary>        
        /// <returns>BatchId</returns>
        public static string GenerateEFTBatchId()
        {
            Database db = null;
            DbCommand dbCommand = null;            
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
               
                if (db == null) return "";
                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_PM_GenerateEFTBatchID");
                dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);                
                db.AddOutParameter(dbCommand, "@VARCHAR_BatchId", DbType.String, 100);

                if (dbCommand == null) return "";
                db.ExecuteNonQuery(dbCommand);

                return db.GetParameterValue(dbCommand, "@VARCHAR_BatchId").ToString();
            }
            catch
            {
                throw;
            }
        }

		// SR | 2018.04.11 | YRS-AT-3101 | This method Update EFT Batch id and status in given Disbursement and inserting notes into AtsNotes table.
        /// <summary>
        /// Updates EFT Batch Id and status for disbursements and inserting notes into AtsNotes table
        /// </summary>
        /// <param name="disbursementID">Disbursement ID</param>
        /// <param name="batchID">Batch ID</param>
        public void UpdateEFTDisbursementDetails(string disbursementID, string batchID)
        {
            Database db = null;
            DbCommand dbCommand = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                               
                dbCommand = db.GetStoredProcCommand("yrs_usp_PM_UpdateEFTDisbursementDetails");
                dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(dbCommand, "@VARCHAR_guiDisbursementID", DbType.String, disbursementID);
                db.AddInParameter(dbCommand, "@VARCHAR_BatchId", DbType.String, batchID);
                                
                db.ExecuteNonQuery(dbCommand);
            }
            catch
            {
                throw;
            }
        }

        public static YMCAObjects.ReturnObject<bool> ApproveEFTPayment(string disbursementEFTId, string persBankingEFTID, string bankID, string disbursementID, double netAmount) //, DataRow drApproveEFTPayment)
        {
            Database db = null;
            DbCommand cmd = null;
            DbConnection connection = null;
            DbTransaction transaction;
            bool isTransactionStarted = false;
            bool isCompleted;
            string errorCode;
            try
            {
                isCompleted = false;
                errorCode = string.Empty;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return SetReturnValue(false, "MESSAGE_PM_SYSTEM_ERROR");
                connection = db.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                isTransactionStarted = true;
                try
                {
                    if (db != null)
                    {
                        cmd = db.GetStoredProcCommand("dbo.yrs_usp_PM_ApproveEFTPayment");
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                        db.AddInParameter(cmd, "@VARCHAR_DisbursementEFTID", DbType.String, disbursementEFTId);
                        db.AddInParameter(cmd, "@VARCHAR_PersBankingEFTID", DbType.String, persBankingEFTID);
                        db.AddInParameter(cmd, "@VARCHAR_DisbursementID", DbType.String, disbursementID);
                        db.AddInParameter(cmd, "@VARCHAR_BankID", DbType.String, bankID);
                        db.AddInParameter(cmd, "@NUMERIC_NetAmount", DbType.Double, netAmount);
                        db.AddOutParameter(cmd, "@VARCHAR_ErrorCode", DbType.String, 100);
                        db.AddOutParameter(cmd, "@BIT_IsCompleted", DbType.Boolean, 100);

                        if (cmd != null)
                        {
                            db.ExecuteNonQuery(cmd, transaction);
                            isCompleted = Convert.ToBoolean(db.GetParameterValue(cmd, "@BIT_IsCompleted"));
                            errorCode = db.GetParameterValue(cmd, "@VARCHAR_ErrorCode").ToString();
                            if (isCompleted)
                            {
                                transaction.Commit();
                            }
                            else
                            {
                             transaction.Rollback ();
                            }                           
                            connection.Close();
                            isTransactionStarted = false;
                        }
                    }
                    return SetReturnValue(isCompleted, errorCode);
                }
                catch (Exception ex)
                {
                    if (isTransactionStarted)
                    {
                        transaction.Rollback();
                        connection.Close();
                    }
                    ex = null;
                    return SetReturnValue(false, "MESSAGE_PM_SYSTEM_ERROR");
                }
            }
            catch (Exception ex)
            {
                ex = null;
                return SetReturnValue(false, "MESSAGE_PM_SYSTEM_ERROR");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();

                errorCode = null;
                transaction = null;
                connection = null;
                cmd = null;
                db = null;
            }
        }

        public static YMCAObjects.ReturnObject<bool> RejectEFTPayment(string disbursementEFTId, string persBankingEFTID, string bankID, string disbursementID) //, DataRow drApproveEFTPayment)
        {
            Database db = null;
            DbCommand cmd = null;
            DbConnection connection = null;
            DbTransaction transaction;
            bool isTransactionStarted = false;

            bool isCompleted;
            string errorCode;
            bool sendMailToCS=false; // SR | 2018.10.05 | YRS-AT-3101 | Defined variable to identify Mail send to CS is required or not  		
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return SetReturnValue(false, "MESSAGE_PM_SYSTEM_ERROR");
                connection = db.CreateConnection();//.GetConnection();
                connection.Open();
                transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                isTransactionStarted = true;
                try
                {
                    isCompleted = false;
                    errorCode = string.Empty;

                    if (db != null)
                    {
                        cmd = db.GetStoredProcCommand("Dbo.yrs_usp_PM_RejectEFTPayment");
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                        db.AddInParameter(cmd, "@VARCHAR_DisbursementEFTID", DbType.String, disbursementEFTId);
                        db.AddInParameter(cmd, "@VARCHAR_PersBankingEFTID", DbType.String, persBankingEFTID);
                        db.AddInParameter(cmd, "@VARCHAR_DisbursementID", DbType.String, disbursementID);
                        db.AddOutParameter(cmd, "@VARCHAR_ErrorCode", DbType.String, 100);
                        db.AddOutParameter(cmd, "@BIT_IsCompleted", DbType.Boolean, 100);
                        db.AddOutParameter(cmd, "@BIT_SendMailToCS", DbType.Boolean, 100);
                        

                        if (cmd != null)
                        {
                            db.ExecuteNonQuery(cmd, transaction);
                            isCompleted = Convert.ToBoolean(db.GetParameterValue(cmd, "@BIT_IsCompleted"));
                            errorCode = db.GetParameterValue(cmd, "@VARCHAR_ErrorCode").ToString();
                            sendMailToCS = Convert.ToBoolean(db.GetParameterValue(cmd, "@BIT_SendMailToCS")); // SR | 2018.10.05 | YRS-AT-3101 | Defined output parameter to identify Mail send to CS is required or not.  		
                            //drApproveEFTPayment["IsDatabaseUpdated"] = db.GetParameterValue(dbCommand, "@BIT_IsCompleted").ToString();
                            //drApproveEFTPayment["ReasonCode"] = db.GetParameterValue(dbCommand, "@VARCHAR_ErrorCode").ToString();
                            if (isCompleted)
                            {
                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();
                            }     
                            connection.Close();
                            isTransactionStarted = false;
                        }
                    }
                    return SetReturnValue(isCompleted, errorCode, sendMailToCS); // SR | 2018.10.05 | YRS-AT-3101 | If actual EFT rejection count equal or exceed configured value then set flag to sent mail to CS to assist participant.  
                }
                catch (Exception ex)
                {
                    if (isTransactionStarted)
                    {
                        //drApproveEFTPayment["IsDatabaseUpdated"] = "0";
                        //drApproveEFTPayment["ReasonCode"] = "Systen failure error";
                        transaction.Rollback();
                        connection.Close();
                        isTransactionStarted = false;
                    }
                    ex = null;
                    return SetReturnValue(false, "MESSAGE_PM_SYSTEM_ERROR", false); // SR | 2018.10.05 | YRS-AT-3101 | Set default value to FALSE for not sending mail to CS
                }
            }
            catch (Exception ex)
            {
                ex = null;
                return SetReturnValue(false, "MESSAGE_PM_SYSTEM_ERROR", false); // SR | 2018.10.05 | YRS-AT-3101 | Set default value to FALSE for not sending mail to CS
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();

                errorCode = null;
                transaction = null;
                connection = null;
                cmd = null;
                db = null;
            }
        }

        private static YMCAObjects.ReturnObject<bool> SetReturnValue(bool value, string message)
        {
            YMCAObjects.ReturnObject<bool> result = new YMCAObjects.ReturnObject<bool>();
            result.Value = value;
            if (!string.IsNullOrEmpty(message))
            {
                result.MessageList = new System.Collections.Generic.List<string>();
                result.MessageList.Add(message);
            }
            return result;
        }

        // START : SR | 2018.04.06 | YRS-AT-3101 - Create overloaded method to handle requirement of sending mail to CS to assist participant on multiple rejection.
        private static YMCAObjects.ReturnObject<bool> SetReturnValue(bool value, string message, bool isAllowedPaymentIterationCrossed)
        {
            YMCAObjects.ReturnObject<bool> result = SetReturnValue(value, message);
            if (!(isAllowedPaymentIterationCrossed))
            {
                if (result.MessageList == null )
                {
                    result.MessageList = new System.Collections.Generic.List<string>();
                }
                result.MessageList.Add("AllowedPaymentIterationNotCrossed");
            }
            return result;
        }
        // END : SR | 2018.04.06 | YRS-AT-3101 - Create overloaded method to handle requirement of sending mail to CS to assist participant on multiple rejection.

        // START : SR | 2018.10.10  |  YRS-AT-3101 | Get next EFT Batch Id
        public static DataSet GetNextEFTBatchId()
        {
            DataSet batchId = null;
            Database db = null;
            DbCommand getBatchId = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                getBatchId = db.GetStoredProcCommand("yrs_usp_GetNextEFTBatchID");                
                if (getBatchId == null) return null;
                batchId = new DataSet();
                db.LoadDataSet(getBatchId, batchId, "BatchId");
                return batchId;
            }
            catch
            {                
                throw;
            }
        }
        // END : SR | 2018.10.10  |  YRS-AT-3101 | Get next EFT Batch Id
                

        // START: SR | 2018.04.06 | YRS-AT-3101 - This procedure validate whether imported batch is already processed or yet to be processed.
        /// <summary>
        /// Get Approved/Paid Loan details to send mail to YMCA LPA
        /// </summary>        
        /// <returns>Boolean</returns>
        public static DataSet GetImportedBatchFileStatus(string eftFileBatchID) //VC | 2018.11.16 | YRS-AT-3101 | Changed return type from bool to Dataset
        {
            Database db;
            DbCommand dbCommand;
            DataSet validateImportedBatch; //VC | 2018.11.16 | YRS-AT-3101 | Declared dataset
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null; //VC | 2018.11.16 | YRS-AT-3101 | If database is null then return null
                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_Loan_ValidateImportedBatchStatus");
                if (dbCommand == null) return null; //VC | 2018.11.16 | YRS-AT-3101 | If databse command is null then return null
                dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(dbCommand, "@VARCHAR_EFTFileBatchID", DbType.String, eftFileBatchID);
                //START: VC | 2018.11.16 | YRS-AT-3101 | Commented existing code and added new code to return dataset
                //db.AddOutParameter(dbCommand, "@BIT_IsBatchProcesssed", DbType.Boolean, 10);
                //if (dbCommand == null) return false;
                //db.ExecuteNonQuery(dbCommand);
                //return isImportedBatchProcessed = Convert.ToBoolean(db.GetParameterValue(dbCommand, "@BIT_IsBatchProcesssed"));               
                validateImportedBatch = new DataSet();
                db.LoadDataSet(dbCommand, validateImportedBatch, "ValidateImportedBatch");
                return validateImportedBatch;
                //END: VC | 2018.11.16 | YRS-AT-3101 | Commented existing code and added new code to return dataset

            }
            catch
            {
                throw;
            }
            //START: VC | 2018.11.16 | YRS-AT-3101 | Created finally block to clear variables
            finally
            {
                validateImportedBatch = null;
                dbCommand = null;
                db = null;
            }
            //END: VC | 2018.11.16 | YRS-AT-3101 | Created finally block to clear variables
        }
        // END: SR | 2018.04.06 | YRS-AT-3101 - Get Approved/Paid Loan details to send mail to YMCA LPA.

        //START: SR | 2018.10.17 | YRS-AT-3101 | Provides EFT Non pending disbursement details based on given status
        public static DataTable GetEFTNonPendingDisbursements(string type)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_PM_GetEFTNonPendingdisbursements");
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(cmd, "@VARCHAR_DisbursementType", DbType.String, type);                
                if (cmd == null) return null;

                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "EFTDisbursements");
                return ds.Tables["EFTDisbursements"];
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
        //END: SR | 2018.10.17 | YRS-AT-3101 | Provides EFT Non pending disbursement details based on given status.

        // START: SR | 2019.10.07 | YRS-AT-4601 - YRS enh: State Withholding Project - Export file First Annuity Payroll 
        /// <summary>
        /// Create Entry in Export Base header  
        /// </summary>        
        /// <returns>ProcessID</returns>
        public int CreateExportBaseHeaderEntry(string source, string disbursementType, string status)
        {
            Database db = null;
            DbCommand dbCommand = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return 0;
                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_STW_Export_CreateBaseHeaderEntry");
                dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(dbCommand, "@VARCHAR_Source", DbType.String, source);
                db.AddInParameter(dbCommand, "@VARCHAR_DisbursementType", DbType.String, disbursementType);
                db.AddInParameter(dbCommand, "@DATE_PayrollDate", DbType.Date, DateTime.Now.Date);
                db.AddOutParameter(dbCommand, "@INT_ProcessId", DbType.Int32, 100);
                db.AddInParameter(dbCommand, "@VARCHAR_Status", DbType.String, status);
                
                if (dbCommand == null) return 0;
                db.ExecuteNonQuery(dbCommand);

                return Convert.ToInt32(db.GetParameterValue(dbCommand, "@INT_ProcessId"));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Prepare First Annuity Record For Export file.
        /// </summary>        
        /// <returns>BatchId</returns>
        public string CreateExportBaseEntryForFirstAnnuityPayment(string disbursementId, int processId)
        {
            Database db = null;
            DbCommand dbCommand = null;            
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return "";
                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_STW_Export_FirstAnnuity");
                dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(dbCommand, "@VARCHAR_DisbursementId", DbType.String, disbursementId);
                db.AddInParameter(dbCommand, "@INT_ProcessId", DbType.Int32, processId);
                db.AddOutParameter(dbCommand, "@VARCHAR_ErrorMessage", DbType.String, 1000);

                if (dbCommand == null) return "";
                db.ExecuteNonQuery(dbCommand,this.DBTransaction );

                return db.GetParameterValue(dbCommand, "@VARCHAR_ErrorMessage").ToString();
            }
            catch(Exception ex)
            {
                if (this.l_bool_TransactionStarted)
                {

                    this.DBTransaction.Rollback();
                    this.DBconnectYRS.Close();
                    this.l_bool_TransactionStarted = false;

                }
                throw ;
            }
        }
        
        /// <summary>
        /// Prepare First Annuity Record For Export file.
        /// </summary>        
        /// <returns>BatchId</returns>
        public DataSet GetFirstAnnuityExportFile(int processId, ref string errorMessage)
        {
            DataSet dsFirstAnnuityExportData = null;
            Database db = null;            
            DbCommand dbCommand = null;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager DA", String.Format("GetFirstAnnuityExportFile  - START"));
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager DA", String.Format("ProcessId" + processId)); 
                
                db = DatabaseFactory.CreateDatabase("YRS");
                dsFirstAnnuityExportData = new DataSet();
                if (db == null) return null;

                
                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_STW_Export_PrepareData");
                dbCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.AddInParameter(dbCommand, "@INT_PayrollProcessId", DbType.String, processId);            
                db.AddOutParameter(dbCommand, "@VARCHAR_ErrorMessage", DbType.String, 1000);                                
                if (dbCommand == null) return null;
                db.ExecuteNonQuery(dbCommand, this.DBTransaction);                              
                errorMessage = db.GetParameterValue(dbCommand, "@VARCHAR_ErrorMessage").ToString();
                
                dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_STW_Export_PrepareFileContent");                   
                db.AddInParameter(dbCommand, "@INT_PayrollProcessId", DbType.String, processId);
                if (dbCommand == null) return null;
                db.LoadDataSet(dbCommand, dsFirstAnnuityExportData, "dtFirstAnnuityExportData", this.DBTransaction);

                dbCommand = db.GetStoredProcCommand("yrs_usp_STW_Export_AnnuitiesSuppressPayroll");
                db.AddInParameter(dbCommand, "@INT_PayrollProcessId", DbType.String, processId);
                if (dbCommand == null) return null;
                db.ExecuteNonQuery(dbCommand, this.DBTransaction);

                //START : ML | YRS-AT-4676 | Get Errors and Warnning for current Batch
				dbCommand = db.GetStoredProcCommand("dbo.yrs_usp_GetExceptionLogDetails");
                db.AddInParameter(dbCommand, "@INT_PayrollProcessId", DbType.Int32, processId);
                db.AddInParameter(dbCommand, "@VARCHAR_chvSource", DbType.String , "EXPORT");
                //END : ML | YRS-AT-4676 | Get Errors and Warnning for current Batch
			    
                if (dbCommand == null) return null;
                db.LoadDataSet(dbCommand, dsFirstAnnuityExportData, "dtExpceptionLogDetails",this.DBTransaction );
               
				return dsFirstAnnuityExportData;
            }
            catch
            {
                if (this.l_bool_TransactionStarted)
                {

                    this.DBTransaction.Rollback();
                    this.DBconnectYRS.Close();
                    this.l_bool_TransactionStarted = false;

                }
                throw;
            }
            finally
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager DA", String.Format("GetFirstAnnuityExportFile  - END")); 
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
        }
       
        // END: SR | 2019.10.07 | YRS-AT-4601 - YRS enh: State Withholding Project - Export file First Annuity Payroll 

        //START: SR | 2019.12.31 | YRS-AT-4602 | update export file status.
        public void ChangeExportFileStatus(int processId, string exportStatus)
        {
            DbCommand dbCommandWrapper = null;
            Database dataBase = null;
            try
            {
                dataBase = DatabaseFactory.CreateDatabase("YRS");
                if (dataBase != null)
                {
                    dbCommandWrapper = dataBase.GetStoredProcCommand("dbo.yrs_usp_STW_Export_ModifyStatus");
                    dbCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                    if (dbCommandWrapper != null)
                    {
                        dataBase.AddInParameter(dbCommandWrapper, "@INT_ProcessId", DbType.Int32, processId);
                        dataBase.AddInParameter(dbCommandWrapper, "@VARCHAR_Status", DbType.String, exportStatus);
                        dataBase.ExecuteNonQuery(dbCommandWrapper,this.DBTransaction);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        //END: SR | 2019.12.31 | YRS-AT-4602 | update export file status.

	}
	
	}


