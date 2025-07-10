//****************************************************
//Modification History
//****************************************************
//Modified by         Date           Description
//****************************************************
//Shubhrata           05/17/2007     Added a new method(GetAccountGroups) to fetch Account Groups from atsMetaAcctGroups
//Manthan Rajguru     2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
//using System.Data.SqlClient;
using System.Globalization;
//using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AccountTypesDAClass.
	/// </summary>
	public sealed class DeferredPayment
	{
		private DeferredPayment()
		{
		}
		public static DataSet GetDeferredPaymentList()
		{
			string[] strdataTablename;
			DataSet dsDeferredPaymentList = null;
			Database db = null;
			DbCommand commandLookUpAccountType = null;
			try
			{
				strdataTablename = new string[2] {"dtDefPaymentList","dtInstallmentTransactdetail"};
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpAccountType = db.GetStoredProcCommand("dbo.yrs_usp_DP_GetDeferredInstallmentList");
				if (commandLookUpAccountType ==null) return null;
				dsDeferredPaymentList = new DataSet();
				dsDeferredPaymentList.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpAccountType,dsDeferredPaymentList,strdataTablename);
				return dsDeferredPaymentList;
			}
			catch
			{
				throw;
			}

		}
	
		
		public static DataSet GetDeferredPaymentTableSchemas ()
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			DataSet l_DataSet = null;
			string [] l_TableNames;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase == null) return null;

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_DP_GetDeferredPaymentSchemas");

				if (l_DBCommandWrapper == null) return null;

				l_DataSet = new DataSet ("RefundTransactionSchemas");

				l_TableNames = new string []{"Transactions", "RefRollOverInstitution", "Disbursements", "DisbursementDetails", "DisbursementWithholding", "DisbursementRefunds", "ReissueTransaction","AtsRefunds"};

				l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet, l_TableNames);

				return l_DataSet;
					
			}
			catch
			{
				throw;
			}

		}


		public static string GetRolloverInstitutionID(string paramStrInstitutionName)
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			string guiInstitutionID = string.Empty;

			try
			{
				l_DataBase= DatabaseFactory.CreateDatabase("YRS");		
				if (l_DataBase == null) return null;

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("yrs_usp_BS_Get_RefundRolloverInstitutionID");
				if (l_DBCommandWrapper == null) return null;

				l_DataBase.AddInParameter (l_DBCommandWrapper,"@varchar_RolloverInstitutionName", DbType.String, paramStrInstitutionName);
				l_DataBase.AddOutParameter (l_DBCommandWrapper,"@varchar_RolloverInstitutionUniqueID",  DbType.String, 50) ;
					
				l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
				
				if (l_DataBase.GetParameterValue (l_DBCommandWrapper,"@varchar_RolloverInstitutionUniqueID").GetType().ToString()== "System.DBNull")
					return String.Empty;
				else
					return Convert.ToString (l_DataBase.GetParameterValue (l_DBCommandWrapper,"@varchar_RolloverInstitutionUniqueID"));
									
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public static bool SaveDeferredPayment (DataSet parameterDataSet)
		{
						
			DataTable l_AtsRefunds;
			DataTable l_Disbursements;
			DataTable l_DisbursementDetails;
			DataTable l_Transaction;
			DataTable l_DisbursementWithHold;
			DataTable l_DisbursementRefunds;
			DataTable l_RefRollOverInstitution;
			
			DataTable	l_RefundRequest;
			
			Database l_Database = null;

			DbConnection l_IDbConnection = null;

			DbTransaction l_IDbTransaction = null;
			//			string l_string_RequestError = "";
			string l_string_RefRequestID=string.Empty;

			string l_SubTableRequestID = string.Empty;
			//string l_RefundRequestID =string.Empty;
			string paramterRefundType= string.Empty;            						
			bool l_TransactionFlag = false;  //--  This falg is used to Keep whether Main transaction is happening r not.
			// B'cos, Disbursemnet, Transaction & Refund is Compulsory for this Transaction. 
			// Otherwise Rolback the Transaction.
			try
			{
				
				if (parameterDataSet == null) return false;
		
				l_Disbursements = parameterDataSet.Tables ["Disbursements"];
				l_Transaction = parameterDataSet.Tables ["Transactions"];
				l_RefundRequest = parameterDataSet.Tables ["RefundRequestDataTable"];
				
				l_DisbursementDetails = parameterDataSet.Tables ["DisbursementDetails"];
				l_DisbursementWithHold = parameterDataSet.Tables ["DisbursementWithholding"];
				l_DisbursementRefunds = parameterDataSet.Tables ["DisbursementRefunds"];

				l_RefRollOverInstitution = parameterDataSet.Tables ["RefRollOverInstitution"];
				l_AtsRefunds=parameterDataSet.Tables ["AtsRefunds"];
				
				if ((l_Disbursements != null) && l_DisbursementDetails != null && l_Transaction != null && l_AtsRefunds!=null) 
				{
					l_Database = DatabaseFactory.CreateDatabase ("YRS");

                    l_IDbConnection = l_Database.CreateConnection();// .GetConnection();
					
					l_IDbConnection.Open ();
					if (l_Database == null) return false; 
					
					l_IDbTransaction = l_IDbConnection.BeginTransaction ();					
					l_TransactionFlag = false;

					foreach (DataRow l_DisbursementDataRow in l_Disbursements.Rows)
					{
						if (InsertDisbursements (l_DisbursementDataRow, l_Database, l_IDbTransaction))
							l_TransactionFlag = true;
					} // Insert Disbursement Data

					// Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
					if (l_TransactionFlag == false)
					{
						l_IDbTransaction.Rollback ();
						return false;	// Return with Error.
					}

					l_TransactionFlag = false;
					foreach (DataRow l_TransactionDataRow in l_Transaction.Rows)
					{
						if (l_TransactionDataRow != null)
						{
							if (InsertTransactions (l_TransactionDataRow, l_Database, l_IDbTransaction))
								l_TransactionFlag = true;
						}

					}
					
					// Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
					if (l_TransactionFlag == false)
					{
						l_IDbTransaction.Rollback ();
						return false;	// Return with Error.
					}
					
					
					
					l_TransactionFlag = false;
					// Insert Disbursement Details.. 
					if (l_DisbursementDetails != null)
					{
						foreach (DataRow l_DisbursementDetailsDataRow in l_DisbursementDetails.Rows)
						{
							InsertDisbursementDetails (l_DisbursementDetailsDataRow, l_Database, l_IDbTransaction);
							l_TransactionFlag = true;
						}

					}
					// Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
					if (l_TransactionFlag == false)
					{
						l_IDbTransaction.Rollback ();
						return false;	// Return with Error.
					}

					l_TransactionFlag = false;
					// Insert atsRefunds records . 
					if (l_AtsRefunds != null)
					{
						foreach (DataRow l_AtsRefundsRow in l_AtsRefunds.Rows)
						{
							InsertAtsRefundsRecords(l_AtsRefundsRow, l_Database, l_IDbTransaction);
							l_TransactionFlag = true;
						}
					}
					// Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
					if (l_TransactionFlag == false)
					{
						l_IDbTransaction.Rollback ();
						return false;	// Return with Error.
					}
					
					// Insert Disbursement WithHold Details. 
					if (l_DisbursementWithHold != null && l_DisbursementWithHold.Rows.Count >0 )
					{
						l_TransactionFlag = false;
						foreach (DataRow l_WothHoldDataRow in l_DisbursementWithHold.Rows)
						{
							InsertDisbursementWithHold (l_WothHoldDataRow, l_Database, l_IDbTransaction);
							l_TransactionFlag = true;
						}
						// Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
						if (l_TransactionFlag == false)
						{
							l_IDbTransaction.Rollback ();
							return false;	// Return with Error.
						}

					}
					
					l_TransactionFlag = false;

					// Insert Disbursement Refunds Details. 
					if (l_DisbursementRefunds != null && l_DisbursementRefunds.Rows.Count > 0)
					{
						Int64 l_OldintInstallmentID =0, l_NewintInstallmentID =  0;
						foreach (DataRow l_DisbursementRefundsDataRow in l_DisbursementRefunds.Rows)
						{
							InsertDisbursementRefunds (l_DisbursementRefundsDataRow, l_Database, l_IDbTransaction);
							l_TransactionFlag = true;
							if (l_TransactionFlag == false)
							{
								l_IDbTransaction.Rollback ();
								return false;	// Return with Error.
							}
							

							if(l_OldintInstallmentID==0)
							{
								l_OldintInstallmentID = Convert.ToInt64(l_DisbursementRefundsDataRow["intRefDeferredPaymentID"]);
								UpdatePaymentStatus(l_OldintInstallmentID, l_Database, l_IDbTransaction);
								l_TransactionFlag = true;
							}
							else
							{
								l_NewintInstallmentID = Convert.ToInt64(l_DisbursementRefundsDataRow["intRefDeferredPaymentID"]);
								if(l_OldintInstallmentID != l_NewintInstallmentID)
								{
									l_OldintInstallmentID = l_NewintInstallmentID;
									UpdatePaymentStatus(l_OldintInstallmentID, l_Database, l_IDbTransaction);
									l_TransactionFlag = true;
								}

							}
							
						}
						
					}
					
					// Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
					if (l_TransactionFlag == false)
					{
						l_IDbTransaction.Rollback ();
						return false;	// Return with Error.
					}

					if (l_RefRollOverInstitution != null && l_RefRollOverInstitution.Rows.Count > 0)
					{
						l_TransactionFlag = false;
						foreach (DataRow drRefRollOverInstitution in l_RefRollOverInstitution.Rows)
						{
							InsertRefRollOverInstitutionData (drRefRollOverInstitution, l_Database, l_IDbTransaction);
							l_TransactionFlag = true;
						}

						// Check wherther Transaction is happened r not, Yes then Gohead, Rollback otherwise.
						if (l_TransactionFlag == false)
						{
							l_IDbTransaction.Rollback ();
							return false;	// Return with Error.
						}
					}
					

					if (l_Disbursements != null && l_Disbursements.Rows.Count >0)
					{
						l_TransactionFlag = false;
						foreach (DataRow dr in l_Disbursements.Rows)
						{
							if(Convert.ToString(dr["PayeeEntityTypeCode"]).Trim().ToUpper() == "ROLINS")
							{
								paramterRefundType = "ROLINS";
							}
							else 
							{
								paramterRefundType = "PERS";	//Need to verify
							}
							Populate1099Values(Convert.ToString(dr["UniqueID"]),paramterRefundType,l_Database,l_IDbTransaction);
							l_TransactionFlag = true;
												
						}
						if (l_TransactionFlag == false)
						{
						l_IDbTransaction.Rollback ();
						return false;	
						}
					}
//					l_IDbTransaction.Rollback ();
					
					l_IDbTransaction.Commit();
				}
				
				return true;
			}
				
			catch (Exception ex)
			{
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

		private static bool InsertDisbursements (DataRow parameterDataRow, Database parameterDatabase,  DbTransaction parameterTransaction)
		{

			DbCommand l_DBCommandWrapper;

			try
			{

				if (parameterDatabase == null) return false; 

				if (parameterDataRow == null) return false;

				l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_DP_InsertDisbursements");

				if (l_DBCommandWrapper == null) return false;

				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_PayeeEntityID", DbType.String, Convert.ToString (parameterDataRow["PayeeEntityID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_PayeeAddrID", DbType.String, Convert.ToString (parameterDataRow["PayeeAddrID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_PayeeEntityTypeCode", DbType.String, Convert.ToString (parameterDataRow["PayeeEntityTypeCode"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_DisbursementType", DbType.String, Convert.ToString (parameterDataRow["DisbursementType"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_IrsTaxTypeCode", DbType.String, Convert.ToString (parameterDataRow["IrsTaxTypeCode"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_PaymentMethodCode", DbType.String,Convert.ToString ( parameterDataRow["PaymentMethodCode"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_CurrencyCode", DbType.String, Convert.ToString (parameterDataRow["CurrencyCode"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_PersID", DbType.String, Convert.ToString (parameterDataRow["PersID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_Rollover", DbType.String, Convert.ToString (parameterDataRow["Rollover"]));
				//l_DBCommandWrapper.AddInParameter ("@varchar_Creator", DbType.String, "YRS");
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@numeric_TaxableAmount", DbType.Decimal, Convert.ToDecimal (parameterDataRow["TaxableAmount"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@numeric_NonTaxableAmount", DbType.Decimal, Convert.ToDecimal (parameterDataRow ["NonTaxableAmount"]));

				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_UniqueID", DbType.String,Convert.ToString (parameterDataRow["UniqueID"]));
				
				parameterDatabase.ExecuteNonQuery(l_DBCommandWrapper, parameterTransaction);
				return true;

			}
			catch 
			{
				throw; 
			}
			

		}

		private static void InsertDisbursementDetails (DataRow parameterDataRow, Database parameterDatabase,  DbTransaction parameterTransaction)
		{

			DbCommand l_DBCommandWrapper;

			try
			{

				if (parameterDatabase == null) return ; 

				if (parameterDataRow == null) return ;

				l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_RR_InsertDisbursementDetails");

				if (l_DBCommandWrapper == null) return ;

				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_DisbursementID", DbType.String, Convert.ToString (parameterDataRow["DisbursementID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_AcctType", DbType.String, Convert.ToString (parameterDataRow["AcctType"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_AcctBreakdownType", DbType.String, Convert.ToString(parameterDataRow["AcctBreakdownType"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@integer_SortOrder", DbType.Int32, Convert.ToInt32 (parameterDataRow["SortOrder"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@numeric_TaxablePrincipal", DbType.Decimal, Convert.ToDecimal (parameterDataRow["TaxablePrincipal"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@numeric_TaxableInterest", DbType.Decimal, Convert.ToDecimal ( parameterDataRow["TaxableInterest"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@numeric_NonTaxablePrincipal", DbType.Decimal, Convert.ToDecimal (parameterDataRow["NonTaxablePrincipal"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@numeric_TaxWithheldPrincipal", DbType.Decimal, Convert.ToDecimal (parameterDataRow["TaxWithheldPrincipal"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@numeric_TaxWithheldInterest", DbType.Decimal, Convert.ToDecimal (parameterDataRow["TaxWithheldInterest"]));

				parameterDatabase.ExecuteNonQuery (l_DBCommandWrapper, parameterTransaction);		
				

			}
			catch (Exception ex)
			{
				throw (ex);
			}

		}


		private static void InsertDisbursementWithHold (DataRow parameterDataRow, Database parameterDatabase,  DbTransaction parameterTransaction)
		{

			DbCommand l_DBCommandWrapper;

			try
			{

				if (parameterDatabase == null) return ; 

				if (parameterDataRow == null) return ;

				l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_RR_InsertDisbWithholding");

				if (l_DBCommandWrapper == null) return ;

				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_DisbursementID", DbType.String, Convert.ToString (parameterDataRow["DisbursementID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_WithholdingTypeCode", DbType.String, Convert.ToString (parameterDataRow["WithholdingTypeCode"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@numeric_Amount", DbType.Decimal, Convert.ToDecimal (parameterDataRow["Amount"]));

				parameterDatabase.ExecuteNonQuery (l_DBCommandWrapper, parameterTransaction);		
				

			}
			catch (Exception ex)
			{
				throw (ex);
			}

		}

		private static void InsertDisbursementRefunds (DataRow parameterDataRow, Database parameterDatabase,  DbTransaction parameterTransaction)
		{
			DbCommand l_DBCommandWrapper;

			try
			{			

				if (parameterDatabase == null) return ; 

				if (parameterDataRow == null) return ;

				l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_DP_InsertDisbRefunds");

				if (l_DBCommandWrapper == null) return ;

				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_RefRequestID", DbType.String, Convert.ToString (parameterDataRow["guiRefRequestID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_DisbursementID", DbType.String, Convert.ToString (parameterDataRow["guiDisbursementID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@bigint_intRefDeferredPaymentID", DbType.Int64, Convert.ToInt64 (parameterDataRow["intRefDeferredPaymentID"]));
				
				parameterDatabase.ExecuteNonQuery (l_DBCommandWrapper, parameterTransaction);		
				

			}
			catch (Exception ex)
			{
				throw (ex);
			}

		}

		private static bool InsertTransactions (DataRow parameterDataRow, Database parameterDatabase,  DbTransaction parameterTransaction)
		{

			DbCommand l_DBCommandWrapper;

			try
			{

				if (parameterDatabase == null) return false; 

				if (parameterDataRow == null) return false;

				l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand("dbo.yrs_usp_DP_InsertTransactionDetails");

				if (l_DBCommandWrapper == null) return false;

				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_PersonID", DbType.String, Convert.ToString (parameterDataRow["PersID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_FundEventID", DbType.String, Convert.ToString (parameterDataRow["FundEventID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_AcctType", DbType.String, Convert.ToString (parameterDataRow["AcctType"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_TransactType", DbType.String, Convert.ToString (parameterDataRow["TransactType"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_AnnuityBasisType", DbType.String, Convert.ToString (parameterDataRow["AnnuityBasisType"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_TransactionRefID", DbType.String,Convert.ToString ( parameterDataRow["TransactionRefID"]));
				//Added by Hafiz on Sep 24th 2008
				//For inserting ‘current date’ TransactDate, this is in order to handle the new parameter added for QDRO.
				//*********
				parameterDatabase.AddInParameter(l_DBCommandWrapper,"@varchar_TransactDate",DbType.String,Convert.ToString(DateTime.Now.Date));
				//*********
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@Numeric_MonthlyComp", DbType.Decimal, Convert.ToDecimal(parameterDataRow["MonthlyComp"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@Numeric_PersonalPreTax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PersonalPreTax"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@Numeric_PersonalPostTax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["PersonalPostTax"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@Numeric_YmcaPreTax", DbType.Decimal, Convert.ToDecimal(parameterDataRow["YmcaPreTax"]));
				//l_DBCommandWrapper.AddInParameter ("@varchar_Creator", DbType.String, "YRS");

                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_UniqueID", DbType.String, Convert.ToString(parameterDataRow["UniqueID"]));
							
				parameterDatabase.ExecuteNonQuery (l_DBCommandWrapper, parameterTransaction);
				return true;

			}
			catch (Exception ex)
			{
				throw (ex);
			}

		}

		private static void InsertAtsRefundsRecords (DataRow parameterDataRow, Database parameterDatabase,  DbTransaction parameterTransaction)
		{

			DbCommand l_DBCommandWrapper;
			
			

			try
			{

				if (parameterDatabase == null) return ; 

			

				if (parameterDataRow == null) return ;

				l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_DP_InsertRefunds");

				if (l_DBCommandWrapper == null) return ;

				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_RefRequestsID", DbType.String, Convert.ToString (parameterDataRow["RefRequestsID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_AcctType", DbType.String, Convert.ToString (parameterDataRow["AcctType"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@Numeric_Taxable", DbType.Decimal, Convert.ToDecimal(parameterDataRow["Taxable"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@Numeric_NonTaxable", DbType.Decimal, Convert.ToDecimal (parameterDataRow["NonTaxable"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@Numeric_Tax", DbType.Decimal, Convert.ToDecimal (parameterDataRow["Tax"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@integer_TaxRate", DbType.Decimal,Convert.ToDecimal ( parameterDataRow["TaxRate"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_Payee", DbType.String, Convert.ToString (parameterDataRow["Payee"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_RequestType", DbType.String, Convert.ToString (parameterDataRow["RequestType"]));
				                                 
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_TransactID", DbType.String, Convert.ToString (parameterDataRow["TransactID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_AnnuityBasisType", DbType.String, Convert.ToString (parameterDataRow["AnnuityBasisType"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@varchar_DisbursementID", DbType.String, Convert.ToString (parameterDataRow["DisbursementID"]));
				parameterDatabase.AddInParameter (l_DBCommandWrapper,"@int_DefInstallmentNo", DbType.Int32,Convert.ToInt32( parameterDataRow["DeferredInstallmentNo"]) );
									
				parameterDatabase.ExecuteNonQuery (l_DBCommandWrapper, parameterTransaction);		
				

			}
			catch (Exception ex)
			{
				throw (ex);
			}

		}

		
		private static void InsertRefRollOverInstitutionData (DataRow drRefRollOverInstitution , Database parameterDatabase,  DbTransaction parameterTransaction)
		{

			DbCommand l_DBCommandWrapper;

			try
			{
               
				if (parameterDatabase == null) return ; 

				l_DBCommandWrapper = parameterDatabase.GetStoredProcCommand ("dbo.yrs_usp_DP_UpdateRefRollOverInstitution");

				if (l_DBCommandWrapper == null) return ;

                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_RefRequestsID", DbType.String, Convert.ToString(drRefRollOverInstitution["guiRefRequestsID"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@varchar_guiRolloverinstitutionId", DbType.String, Convert.ToString(drRefRollOverInstitution["guiRolloverinstitutionId"]));
				//l_DBCommandWrapper.AddInParameter ("@varchar_intDeferredPaymentRefNo", DbType.Int16,  parameterDeferredPaymentRefNo);
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@decimal_mnyRollOverAmount", DbType.Decimal, Convert.ToDecimal(drRefRollOverInstitution["mnyRollOverAmount"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@decimal_bitIsActive", DbType.Boolean, Convert.ToBoolean(drRefRollOverInstitution["bitIsActive"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@decimal_mnyTaxablePercentage", DbType.Decimal, Convert.ToDecimal(drRefRollOverInstitution["mnyTaxablePercentage"]));
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@decimal_mnyNonTaxablePercentage", DbType.Decimal, Convert.ToDecimal(drRefRollOverInstitution["mnyNonTaxablePercentage"]));

				parameterDatabase.ExecuteNonQuery (l_DBCommandWrapper, parameterTransaction);		
				

			}
			catch (Exception ex)
			{
				throw (ex);
			}

		}

		public static string Populate1099Values(string parameterDisbursementId,string parameterRefundType,Database parameterDatabase,  DbTransaction parameterTransaction)
		{
			
			
			Database	l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
				if (l_DataBase == null) return "DataBase not found";

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_Refunds_populate1099Values");

				if (l_DBCommandWrapper == null) return "Procedure not found";

                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@DisbursementId", DbType.String, parameterDisbursementId);
                parameterDatabase.AddInParameter(l_DBCommandWrapper, "@withdrawelType", DbType.String, parameterRefundType);
				
				
				l_DataBase.ExecuteNonQuery(l_DBCommandWrapper,parameterTransaction);
				
			
				return "";
			}
            catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public static bool UpdatePaymentStatus(Int64 paramintInstallmentID,Database parameterDatabase,  DbTransaction parameterTransaction)
		{
			Database	l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
				if (l_DataBase == null) throw (new Exception("DataBase not found"));

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_DP_UpdatePaymentDate");

				if (l_DBCommandWrapper == null) throw (new Exception("Procedure not found"));

				parameterDatabase.AddInParameter(l_DBCommandWrapper,"@intInstallmentID", DbType.Int64,paramintInstallmentID);
				
				
				l_DataBase.ExecuteNonQuery(l_DBCommandWrapper,parameterTransaction);
				
			
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
				
			}
		}
	}



}
