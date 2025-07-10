//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	VoidDisbursementVRManagerDAClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	7/26/2005 11:02:24 AM

// Description							:	DataAcess Class to provide functions interacting with database
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by           Date            Description
//*******************************************************************************
//Neeraj Singh		    22-Oct-2009     changed function InsertDisbursementWithholding to pass DisbusementID as parameter 
//Priya J			    30/12/2009		:BT-1078
//Manthan Rajguru       2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for VoidDisbursementVRManagerDAClass.
	/// </summary>
	public sealed class VoidDisbursementVRManagerDAClass
	{
		private VoidDisbursementVRManagerDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Method to fetch the records of the participants based on the criteria whether annuity only records are required or not.
		/// </summary>
		/// <param name="parameterFundIdNo"></param>
		/// <param name="parameterFName"></param>
		/// <param name="parameterLName"></param>
		/// <param name="parameterSSN"></param>
		/// <param name="parameterCheckNo"></param>
		/// <param name="parameterAnnuityOnly"></param>
		/// <returns>DataSet</returns>

		public static DataSet LookUpDisbursements(string parameterFundIdNo, string parameterFName, string parameterLName, string parameterSSN, string parameterCheckNo, int parameterAnnuityOnly)
		{
			DataSet l_dataset_dsLookUpDisbursements = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_LookUpDisbursements");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_FundIDNo",DbType.String,parameterFundIdNo);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FName", DbType.String, parameterFName);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_LName", DbType.String, parameterLName);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_SSN", DbType.String, parameterSSN);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_CheckNo", DbType.String, parameterCheckNo);
                db.AddInParameter(LookUpCommandWrapper, "@integer_AnnuityOnly", DbType.String, parameterAnnuityOnly);
				//LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationSettings.AppSettings ["ExtraLargeConnectionTimeOut"]); 
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
			
				l_dataset_dsLookUpDisbursements = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpDisbursements,"Disbursements");
				System.AppDomain.CurrentDomain.SetData("dsLookUpDisbursements", l_dataset_dsLookUpDisbursements);
				return l_dataset_dsLookUpDisbursements;
			}
			catch 
			{
				throw;
			}
		}

/// <summary>
/// Method to Get the details of the selected Participant
/// </summary>
/// <param name="parameterPersId"></param>
/// <param name="parameterAnnuityOnly"></param>
/// <returns></returns>

		public static DataSet GetDisbursementsByPersId(string parameterPersId,int parameterAnnuityOnly)
		{
			DataSet l_dataset_dsGetDisbursementsByPersId = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDGetDisbursementsByPersID");
				
				if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId", DbType.String, parameterPersId);
                db.AddInParameter(LookUpCommandWrapper, "@integer_AnnuityOnly", DbType.Int32, parameterAnnuityOnly);
				
				//LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["LargeConnectionTimeOut"]); 
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				l_dataset_dsGetDisbursementsByPersId = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsGetDisbursementsByPersId,"DisbursementsByPersId");
				System.AppDomain.CurrentDomain.SetData("dsDisbursementsByPersId", l_dataset_dsGetDisbursementsByPersId);
				return l_dataset_dsGetDisbursementsByPersId;
			}
			catch 
			{
				throw;
			}
		}


		/// <summary>
		/// Gets the list of status based on the type of activity user wants to perform
		/// </summary>
		/// <param name="parameterActivityType"></param>
		/// <returns></returns>
		public static DataSet GetDisbursementStatusTypes(string parameterActivityType)
		{
			DataSet l_dataset_DisbursementStatusTypes = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetDisbursementStatus");
				
				if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_ActivityType", DbType.String, parameterActivityType);
				//SHubhrata
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]); 
				l_dataset_DisbursementStatusTypes = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_DisbursementStatusTypes,"DisbursementsStatusTypes");
				
				return l_dataset_DisbursementStatusTypes;
			}
			catch 
			{
				throw;
			}
		}


/// <summary>
/// Method to get the Withholding info based on the selected Disbursement Id
/// </summary>
/// <param name="parameterDisbId"></param>
/// <returns></returns>


		public static DataSet GetWithholdingInfo( string parameterDisbId)
		{
			DataSet l_dataset_WithholdingInfo= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("ap_VDGetWithHoldingInfo");
				
				if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@DisbID", DbType.String, parameterDisbId);
				
				//SHubhrata
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]); 
				l_dataset_WithholdingInfo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_WithholdingInfo,"WithholdingInfo");
				System.AppDomain.CurrentDomain.SetData("dsWithholdingInfo", l_dataset_WithholdingInfo);
				return l_dataset_WithholdingInfo;
			}
			catch 
			{
				throw;
			}
		}

/// <summary>
/// To change the status of disbursement if it is not "REISSUE", "REPLACE", "REVERSE"
/// </summary>
/// <param name="parameterDisbId"></param>
/// <param name="parameterActionType"></param>
/// <param name="parameterNotes"></param>
/// <returns></returns>
		public static string ChangeDisbursementStatus(string parameterDisbId,string parameterActionType,string parameterNotes)
		{
			String l_string_Output ;
			Database db = null; 
			string strNewDisbursementID; 

			DbCommand ChangeStatusCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				//ChangeStatusCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDChangeDisbursementStatus");
				ChangeStatusCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDVoidDisbursement");
				
				if (ChangeStatusCommandWrapper == null) return null;

                db.AddInParameter(ChangeStatusCommandWrapper, "@nDisbID", DbType.String, parameterDisbId);
                db.AddInParameter(ChangeStatusCommandWrapper, "@sActionType", DbType.String, parameterActionType);
                db.AddInParameter(ChangeStatusCommandWrapper, "@sNotes", DbType.String, parameterNotes);

                db.AddOutParameter(ChangeStatusCommandWrapper, "@nOutput", DbType.Int32, 9);
				//Priya 27-July-09 PhaseV part II
                db.AddOutParameter(ChangeStatusCommandWrapper, "@NewDisbursementID", DbType.String, 100);
				//End 27-July-2009
				db.ExecuteNonQuery(ChangeStatusCommandWrapper);
                l_string_Output = Convert.ToString(db.GetParameterValue(ChangeStatusCommandWrapper, "@nOutput"));

				//Priya 27-July-09 PhaseV part II
				strNewDisbursementID = Convert.ToString(db.GetParameterValue(ChangeStatusCommandWrapper,"@NewDisbursementID"));
				
				if(( l_string_Output == "1") || (l_string_Output == "5"))
				{
                    return l_string_Output;
				}
				else
				{
					return strNewDisbursementID;
				}
				//return l_string_Output;
				//End Priya 27-July-09 PhaseV part II
				
			}
			catch
			{
				throw;
			}


		}



		public static DataSet GetRelatedDisbursement( string parameterDisbId)
		{
			DataSet l_dataset_RelatedDisbursement= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("ap_VDGetRelatedDisbursements");
				
				if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@DisbID", DbType.String, parameterDisbId);
				
				l_dataset_RelatedDisbursement = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_RelatedDisbursement,"RelatedDisbursement");
				System.AppDomain.CurrentDomain.SetData("dsRelatedDisb", l_dataset_RelatedDisbursement);
				return l_dataset_RelatedDisbursement;
			}
			catch 
			{
				throw;
			}
		}
/// <summary>
/// To perform the reversal in case there is no break down
/// </summary>
/// <param name="parameterDisbId"></param>

		public static  string DoReversal(string parameterDisbId,string paramNewDisbursementID)
		{
			
			Database db = null;
				string l_string_Output;
			DbCommand DoReversalCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				DoReversalCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDDReverse");
				if (DoReversalCommandWrapper == null) return null;

				db.AddInParameter(DoReversalCommandWrapper,"@varchar_DisbID",DbType.String,parameterDisbId);
				
				db.AddOutParameter(DoReversalCommandWrapper,"@varchar_Output",DbType.String,2);
				
				db.ExecuteNonQuery(DoReversalCommandWrapper);

                l_string_Output = Convert.ToString(db.GetParameterValue(DoReversalCommandWrapper, "@varchar_Output"));
				return l_string_Output;
				
			}
			catch
			{
				throw;
			}


		}
		public static int IsCashOut(string parameterDisbId)
		{
			Database db=null;
			int l_IsCashOut=0;
			DbCommand GetCommandWrapper=null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				GetCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_CashOut_Exists");
				db.AddInParameter(GetCommandWrapper,"@chv_DisbursementId",DbType.String,parameterDisbId);
                db.AddOutParameter(GetCommandWrapper, "@bit_IsCashOut", DbType.Int32, 2);
				if (GetCommandWrapper == null) return 0;
				db.ExecuteScalar(GetCommandWrapper);
                l_IsCashOut = Convert.ToInt32(db.GetParameterValue(GetCommandWrapper, "@bit_IsCashOut"));
				return l_IsCashOut;
						
			}
			catch
			{
				throw;
			}
			
		}
		public static DataSet GetDeductions()
		{
			DataSet l_dataset_Deductions = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_CashOut_GetDeductions");
				
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
		public static string DoLoanReversal(string parameterDisbId)
		{
			DbCommand l_DBCommandWrapper;
			Database db = null;
			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection=null;
			string l_string_errormessage=" ";
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return "";

				
				l_IDbConnection = db.CreateConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return null;

				l_IDbTransaction = l_IDbConnection.BeginTransaction();
				l_DBCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_Void_LoanReverse");
				l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return "" ;
                db.AddInParameter(l_DBCommandWrapper, "@varchar_DisbursementId", DbType.String, parameterDisbId);
                db.AddOutParameter(l_DBCommandWrapper, "@stringProcess", DbType.String, 100);
				db.ExecuteNonQuery (l_DBCommandWrapper,l_IDbTransaction);
				l_string_errormessage = Convert.ToString(db.GetParameterValue(l_DBCommandWrapper,"@stringProcess"));
				if (l_string_errormessage != "")
				{
					l_IDbTransaction.Rollback();
				}
					else
				{
					l_IDbTransaction.Commit();
				}
					
				return l_string_errormessage;	
			}
			catch(SqlException SqlEx)
			{
				if (l_IDbTransaction != null)
				{
					l_IDbTransaction.Rollback ();
				}
				throw SqlEx;
			}
			catch(Exception ex)
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

/// <summary>
/// To Reissue when breakdown is not there
/// </summary>
/// <param name="parameterDisbId"></param>
		public static  string  DoReissue(string parameterDisbId,string paramNewDisbursementID)
		{
			
			Database db = null;
			DbCommand DoReissueCommandWrapper = null;
			string l_string_Output;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				DoReissueCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDDReissue");
				if (DoReissueCommandWrapper == null) return null ;

                db.AddInParameter(DoReissueCommandWrapper, "@varchar_DisbID", DbType.String, parameterDisbId);
				//Priya 27-July-2009
                db.AddInParameter(DoReissueCommandWrapper, "@varchar_NewDisbursementID", DbType.String, paramNewDisbursementID);
				//Priya 27-July-2009 Added new parameter
                db.AddOutParameter(DoReissueCommandWrapper, "@varchar_Output", DbType.String, 2);
				
				db.ExecuteNonQuery(DoReissueCommandWrapper);

				l_string_Output = Convert.ToString(db.GetParameterValue(DoReissueCommandWrapper,"@varchar_Output"));
				return l_string_Output;
				
			}
			catch
			{
				throw;
			}


		}

		/// <summary>
		/// Added by imran 0n 20/08/2009
		/// Get Withholdings List By DisbursementId
		/// </summary>
		/// <param name="disbursementId"></param>
		public static DataTable WithholdingsByDisbursement(string parameter_string_disbursementId)
		{
	
			try
			{
				

				DataSet dsWithholdingsByDisbursement = null;
				DataTable dataTableWithholdingsByDisbursement;
				Database db = null;
				DbCommand getCommandWrapper = null;

				try
				{
					db= DatabaseFactory.CreateDatabase("YRS");
			
					if (db == null) return null;

					getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_VDWithholdingsByDisbursement");
                    getCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
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
			catch 
			{
				throw;
			}
		}



		/// <summary>
		/// added on 25/08/2009 by imran
		/// To Void Withdrawal Reissue 
		/// 
		/// </summary>
		/// <param name="parameterDisbId"></param>
		public static  string  DoVoidWithdrawalReissue(string parameterDisbId)
		{
			
			Database db = null;
			DbCommand DoReissueCommandWrapper = null;
			string l_string_Output;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				DoReissueCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReissueWithdrawal");
				

				if (DoReissueCommandWrapper == null) return null ;

				db.AddInParameter(DoReissueCommandWrapper,"@DisbId",DbType.String,parameterDisbId);
                db.AddOutParameter(DoReissueCommandWrapper, "@sOutput", DbType.String, 1000);
				
				db.ExecuteNonQuery(DoReissueCommandWrapper);

                l_string_Output = Convert.ToString(db.GetParameterValue(DoReissueCommandWrapper, "@sOutput"));
				return l_string_Output;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}


		}

//		public static  string  DoVoidReverse(string parameterDisbId,string parameterType)
//		{
//			
//			Database db = null;
//			DbCommand DoReverseCommandWrapper = null;
//			string l_string_Output="";
//		
//			try
//			{
//				db = DatabaseFactory.CreateDatabase("YRS");
//				if(db == null) return null;
//				
//				if (parameterType=="REFUND")
//					DoReverseCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReverseWithdrawal");
//				if (parameterType=="TDLOAN")
//					DoReverseCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReverseLoan");	
//				if (parameterType=="ANNUITY")
//					DoReverseCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReverseAnnuity");	
//
//				if (DoReverseCommandWrapper == null) return null ;
//
//				DoReverseCommandWrapper.AddInParameter("@DisbId",DbType.String,parameterDisbId);
//				DoReverseCommandWrapper.AddOutParameter("@sOutput",DbType.String,1000);
//				
//				db.ExecuteNonQuery(DoReverseCommandWrapper);
//
//				l_string_Output = Convert.ToString(DoReverseCommandWrapper.GetParameterValue("@sOutput"));
//				
//
//				return l_string_Output;
//				
//			}
//
//		
//			catch(Exception ex)
//			{
//				
//				throw ex;
//			}
//			
//			
//
//
//		}


		/// <summary>
		/// Added on 1/10/2009 by imran
		/// To Void Withdrawal Rissue
		/// 
		/// </summary>
		/// <param name="Array parameterDisbId"></param>
		/// <param name="Dataset Disbursement details"></param>
		public static  string  DoVoidWithdrawalRissue(string[] parameterListDisbId,DataSet parameterDataSet)
		{
			
			Database db = null;
			DbCommand DoRissueCommandWrapper = null;
			string l_string_Output="";
			
			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
			
				l_IDbConnection = db.CreateConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return null;

				l_IDbTransaction = l_IDbConnection.BeginTransaction();


				if(parameterDataSet!=null)
				{
					if(parameterDataSet.Tables.Count>0)
					{
						foreach(DataRow l_DataRow  in parameterDataSet.Tables["DisbursementDetails"].Rows ) 
						{
							DoRissueCommandWrapper = db.GetStoredProcCommand ("yrs_usp_VDAddDisbursementDetailForReIssue  ");

							if (DoRissueCommandWrapper == null) return null ;
				

							db.AddInParameter(DoRissueCommandWrapper,"@DisbursementID",DbType.String,Convert.ToString (l_DataRow["DisbursementID"]) );
                            db.AddInParameter(DoRissueCommandWrapper, "@chrAcctType", DbType.String, Convert.ToString(l_DataRow["chrAcctType"]));
                            db.AddInParameter(DoRissueCommandWrapper, "@chrAcctBreakDownType", DbType.String, Convert.ToString(l_DataRow["chrAcctBreakDownType"]));
                            db.AddInParameter(DoRissueCommandWrapper, "@TaxablePrincipal", DbType.Decimal, Convert.ToDecimal(l_DataRow["TaxablePrincipal"].ToString() != "" ? l_DataRow["TaxablePrincipal"] : "0"));
                            db.AddInParameter(DoRissueCommandWrapper, "@TaxableInterest", DbType.Decimal, Convert.ToDecimal(l_DataRow["TaxableInterest"].ToString() != "" ? l_DataRow["TaxableInterest"] : "0"));
                            db.AddInParameter(DoRissueCommandWrapper, "@NonTaxablePrincipal", DbType.Decimal, Convert.ToDecimal(l_DataRow["NonTaxablePrincipal"].ToString() != "" ? l_DataRow["NonTaxablePrincipal"] : "0"));
							//1/10/2009 For Adding Parameter
                            db.AddInParameter(DoRissueCommandWrapper, "@TaxWithheldPrincipal", DbType.Decimal, Convert.ToDecimal(l_DataRow["WithheldPrincipal"].ToString() != "" ? l_DataRow["WithheldPrincipal"] : "0"));
                            db.AddInParameter(DoRissueCommandWrapper, "@TaxWithheldInterest", DbType.Decimal, Convert.ToDecimal(l_DataRow["WithheldInterest"].ToString() != "" ? l_DataRow["WithheldInterest"] : "0"));
                            db.AddOutParameter(DoRissueCommandWrapper, "@sOutput", DbType.String, 1000);
						

							db.ExecuteNonQuery(DoRissueCommandWrapper,l_IDbTransaction);
                            l_string_Output = Convert.ToString(db.GetParameterValue(DoRissueCommandWrapper, "@sOutput"));
					
						}
					}

					if(l_string_Output!=String.Empty)
					{
						l_IDbTransaction.Rollback ();
						return l_string_Output;
					}
				}

				for (int i=1; i<parameterListDisbId.Length;i++)
				{
					DoRissueCommandWrapper = null;
					DoRissueCommandWrapper = db.GetStoredProcCommand ("yrs_usp_VD_ReissueWithdrawal");
					if (DoRissueCommandWrapper == null) return null ;

					db.AddInParameter(DoRissueCommandWrapper,"@DisbId",DbType.String,parameterListDisbId.GetValue(i));
					db.AddOutParameter(DoRissueCommandWrapper,"@sOutput",DbType.String,1000);
					db.ExecuteNonQuery(DoRissueCommandWrapper,l_IDbTransaction);

					l_string_Output = Convert.ToString(db.GetParameterValue(DoRissueCommandWrapper,"@sOutput"));

					if(l_string_Output!=String.Empty)
					{
						l_IDbTransaction.Rollback ();
						return l_string_Output;
					}

				}

								
				l_IDbTransaction.Commit();

				return l_string_Output;
				
			}

		
			catch(Exception ex)
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


		/// <summary>
		/// Added on 6/10/2009 by imran
		/// To Void Withdrawal Replace
		/// 
		/// </summary>
		/// <param name="parameterDisbId"></param>
		/// <param name="Dataset WithHolding"></param>
		public static  string  DoVoidWithdrawalReplace(string[] parameterListDisbId,DataSet parameterdedDataSet,DataSet parameterdisbusementdtlDataset,string parameterType)
		{
			
			Database db = null;
			DbCommand DoReplaceCommandWrapper = null;
			string l_string_Output="";
			string l_string_NewDisbID;

			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
			
				l_IDbConnection = db.CreateConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return null;

				l_IDbTransaction = l_IDbConnection.BeginTransaction();


				if(parameterdisbusementdtlDataset!=null)
				{
					if(parameterdisbusementdtlDataset.Tables.Count>0)
					{
						foreach(DataRow l_DataRow  in parameterdisbusementdtlDataset.Tables["DisbursementDetails"].Rows ) 
						{
							DoReplaceCommandWrapper = db.GetStoredProcCommand ("yrs_usp_VDAddDisbursementDetailForReIssue");

							if (DoReplaceCommandWrapper == null) return null ;
				

							db.AddInParameter(DoReplaceCommandWrapper,"@DisbursementID",DbType.String,Convert.ToString (l_DataRow["DisbursementID"]) );
							db.AddInParameter(DoReplaceCommandWrapper,"@chrAcctType",DbType.String,Convert.ToString (l_DataRow["chrAcctType"]) );
							db.AddInParameter(DoReplaceCommandWrapper,"@chrAcctBreakDownType",DbType.String,Convert.ToString (l_DataRow["chrAcctBreakDownType"]) );
							db.AddInParameter(DoReplaceCommandWrapper,"@TaxablePrincipal",DbType.Double,Convert.ToDouble (l_DataRow["TaxablePrincipal"].ToString()!="" ? l_DataRow["TaxablePrincipal"] : "0"  ) );
							db.AddInParameter(DoReplaceCommandWrapper,"@TaxableInterest",DbType.Double,Convert.ToDouble (l_DataRow["TaxableInterest"].ToString()!="" ? l_DataRow["TaxableInterest"] : "0"  ) );
							db.AddInParameter(DoReplaceCommandWrapper,"@NonTaxablePrincipal",DbType.Double,Convert.ToDouble (l_DataRow["NonTaxablePrincipal"].ToString()!="" ? l_DataRow["NonTaxablePrincipal"] : "0"  ) );
							//10/2009 For Adding Parameter
							db.AddInParameter(DoReplaceCommandWrapper,"@TaxWithheldPrincipal",DbType.Decimal,Convert.ToDecimal (l_DataRow["WithheldPrincipal"].ToString()!="" ? l_DataRow["WithheldPrincipal"] : "0"  ) );
							db.AddInParameter(DoReplaceCommandWrapper,"@TaxWithheldInterest",DbType.Decimal,Convert.ToDecimal (l_DataRow["WithheldInterest"].ToString()!="" ? l_DataRow["WithheldInterest"] : "0"  ) );
							db.AddOutParameter(DoReplaceCommandWrapper,"@sOutput",DbType.String,1000);
						

							db.ExecuteNonQuery(DoReplaceCommandWrapper,l_IDbTransaction);
							l_string_Output = Convert.ToString(db.GetParameterValue(DoReplaceCommandWrapper,"@sOutput"));
					
						}
					}

					if(l_string_Output!=String.Empty)
					{
						l_IDbTransaction.Rollback ();
						return l_string_Output;
					}
				}

				for (int i=1; i<parameterListDisbId.Length;i++)
				{
					DoReplaceCommandWrapper = null;
					if (parameterType=="REFUND")
						DoReplaceCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReplaceWithdrawal");
					if (parameterType=="TDLOAN")
						DoReplaceCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReplaceLoan");	
					if (parameterType=="ANNUITY")
						DoReplaceCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReplaceAnnuity");	

					if (DoReplaceCommandWrapper == null) return null ;

					db.AddInParameter(DoReplaceCommandWrapper,"@nDisbID",DbType.String,parameterListDisbId.GetValue(i));
					//DoReplaceCommandWrapper.AddInParameter("@nAddrID",DbType.String,parameterDisbId);
                    db.AddOutParameter(DoReplaceCommandWrapper, "@sOutput", DbType.String, 1000);
                    db.AddOutParameter(DoReplaceCommandWrapper, "@varchar_NewDisbID", DbType.String, 100);

                    db.ExecuteNonQuery(DoReplaceCommandWrapper, l_IDbTransaction);

					l_string_Output = Convert.ToString(db.GetParameterValue(DoReplaceCommandWrapper,"@sOutput"));
					l_string_NewDisbID = Convert.ToString(db.GetParameterValue(DoReplaceCommandWrapper,"@varchar_NewDisbID"));

					if(l_string_Output!=String.Empty)
					{
						l_IDbTransaction.Rollback ();
						return l_string_Output;
					}
				
					if(parameterdedDataSet.Tables.Count>0)
					{
						foreach(DataRow l_DataRow  in parameterdedDataSet.Tables["Deductions"].Rows ) 
						{
							// below line commented by NEERAJ on 22-OCT-2009: to pass disbursementID as parameter
							//l_DataRow["UniqueID"] = Convert.ToString(l_string_NewDisbID);
							//LoanInformationDAClass.InsertDisbursementWithholding(l_DataRow,db,l_IDbTransaction);
							LoanInformationDAClass.InsertDisbursementWithholding(l_DataRow,db,l_IDbTransaction,l_string_NewDisbID);
							//Neeraj 22-OCT-2009 : END				
						}
					}
				
				}
				l_IDbTransaction.Commit();

				return l_string_Output;
				
			}

		
			catch(Exception ex)
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


		/// <summary>
		/// Added on 6/10/2009 by imran
		/// To Void Reverse
		/// 
		/// </summary>
		/// <param name="parameterListDisbId">Array of DisbId</param>
		/// <param name="parameterRefunddtlDataset">Dataset For Refund Detail</param>
		/// <param name="Type">Type</param>
		/// <param name="Status">Status</param>
		public static  string  DoVoidReverse(string[] parameterListDisbId,DataSet parameterRefunddtlDataset,string parameterType,string parastatus,int Intereststatus,string parameterFundstatus )
		{
			
			Database db = null;
			DbCommand DoReverseCommandWrapper = null;
			string l_string_Output="";
			

			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
			
				l_IDbConnection = db.CreateConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return null;

				l_IDbTransaction = l_IDbConnection.BeginTransaction();


				if(parameterRefunddtlDataset!=null)
				{
					if(parameterRefunddtlDataset.Tables.Count>0)
					{
						foreach(DataRow l_DataRow  in parameterRefunddtlDataset.Tables["RefundDetails"].Rows ) 
						{
							DoReverseCommandWrapper = db.GetStoredProcCommand ("yrs_usp_VDAddRefundDetail");

							if (DoReverseCommandWrapper == null) return null ;
				

							db.AddInParameter(DoReverseCommandWrapper,"@TransactID",DbType.String,Convert.ToString (l_DataRow["TransactID"]) );
                            db.AddInParameter(DoReverseCommandWrapper, "@DisbursementID", DbType.String, Convert.ToString(l_DataRow["DisbursementID"]));
                            db.AddInParameter(DoReverseCommandWrapper, "@chrAcctType", DbType.String, Convert.ToString(l_DataRow["chrAcctType"]));
                            //db.AddInParameter(DoReverseCommandWrapper, "@chrAcctBreakDownType", DbType.String, Convert.ToString(l_DataRow["chrAcctBreakDownType"]));
                            db.AddInParameter(DoReverseCommandWrapper, "@TaxablePrincipal", DbType.Double, Convert.ToDouble(l_DataRow["TaxablePrincipal"].ToString() != "" ? l_DataRow["TaxablePrincipal"] : "0"));
                            db.AddInParameter(DoReverseCommandWrapper, "@TaxableInterest", DbType.Double, Convert.ToDouble(l_DataRow["TaxableInterest"].ToString() != "" ? l_DataRow["TaxableInterest"] : "0"));
                            db.AddInParameter(DoReverseCommandWrapper, "@NonTaxablePrincipal", DbType.Double, Convert.ToDouble(l_DataRow["NonTaxablePrincipal"].ToString() != "" ? l_DataRow["NonTaxablePrincipal"] : "0"));
                            db.AddInParameter(DoReverseCommandWrapper, "@RefRequestsID", DbType.String, Convert.ToString(l_DataRow["RefRequestsID"]));
                            db.AddInParameter(DoReverseCommandWrapper, "@Status", DbType.String, parastatus);

                            db.AddOutParameter(DoReverseCommandWrapper, "@sOutput", DbType.String, 1000);
						

							db.ExecuteNonQuery(DoReverseCommandWrapper,l_IDbTransaction);
                            l_string_Output = Convert.ToString(db.GetParameterValue(DoReverseCommandWrapper, "@sOutput"));
					
						}
					}

					if(l_string_Output!=String.Empty)
					{
						l_IDbTransaction.Rollback ();
						return l_string_Output;
					}
					
				}

				//Added on 27/10/2009 By imran For Void Withdrawal
				if (parameterType!="ANNUITY") 
				{
					DoReverseCommandWrapper = null;
					if (parameterType=="REFUND")
						DoReverseCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReverseWithdrawal");
					if (parameterType=="TDLOAN")
						DoReverseCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReverseLoan");	

					if (DoReverseCommandWrapper == null) return null ;

					db.AddInParameter(DoReverseCommandWrapper,"@DisbId",DbType.String,parameterListDisbId.GetValue(1));
					//Added by imran on 21102009 for intereset process
                    db.AddInParameter(DoReverseCommandWrapper, "@Intereststatus", DbType.Int32, Intereststatus);
					//Added by imran on 23/11/2009 
                    db.AddInParameter(DoReverseCommandWrapper, "@FundStatus", DbType.String, parameterFundstatus);
                    db.AddOutParameter(DoReverseCommandWrapper, "@sOutput", DbType.String, 1000);

				
					db.ExecuteNonQuery(DoReverseCommandWrapper,l_IDbTransaction);

                    l_string_Output = Convert.ToString(db.GetParameterValue(DoReverseCommandWrapper, "@sOutput"));
		
					if(l_string_Output!=String.Empty)
					{
						l_IDbTransaction.Rollback ();
						return l_string_Output;
					}

				}
				else
				{

					for (int i=1; i<parameterListDisbId.Length;i++)
					{
						DoReverseCommandWrapper = null;
						if (parameterType=="ANNUITY")
							DoReverseCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_ReverseAnnuity");	

						if (DoReverseCommandWrapper == null) return null ;

                        db.AddInParameter(DoReverseCommandWrapper, "@DisbId", DbType.String, parameterListDisbId.GetValue(i));
                        db.AddOutParameter(DoReverseCommandWrapper, "@sOutput", DbType.String, 1000);
				
						db.ExecuteNonQuery(DoReverseCommandWrapper,l_IDbTransaction);

                        l_string_Output = Convert.ToString(db.GetParameterValue(DoReverseCommandWrapper, "@sOutput"));
		
						if(l_string_Output!=String.Empty)
						{
							l_IDbTransaction.Rollback ();
							return l_string_Output;
						}
				
					
				
					}
				}

				l_IDbTransaction.Commit();

				return l_string_Output;
				
			}

		
			catch(Exception ex)
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


		/// <summary>
		/// Method to get the Withholding info based on the selected Disbursement Id
		/// </summary>
		/// <param name="parameterDisbId"></param>
		/// <returns></returns>


		public static DataSet GetWithholdingReverseInfo( string parameterRefRequestId)
		{
			DataSet l_dataset_WithholdingInfo= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDGetWithHoldingInfo");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@RefRequestID",DbType.String,parameterRefRequestId);
				
				//SHubhrata
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]); 
				l_dataset_WithholdingInfo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_WithholdingInfo,"WithholdingInfo");
				System.AppDomain.CurrentDomain.SetData("dsWithholdingInfo", l_dataset_WithholdingInfo);
				return l_dataset_WithholdingInfo;
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// Method to get the DisbursementInfo info based on the selected Disbursement Id
		/// </summary>
		/// <param name="parameterDisbId"></param>
		/// <returns></returns>


		public static DataSet GetDisbursementInfo( string parameterDisbursementId)
		{
			DataSet l_dataset_WithholdingInfo= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDGetDisbursementfo");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@DisbID",DbType.String,parameterDisbursementId);
				
				//SHubhrata
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]); 
				l_dataset_WithholdingInfo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_WithholdingInfo,"WithholdingInfo");
				System.AppDomain.CurrentDomain.SetData("dsWithholdingInfo", l_dataset_WithholdingInfo);
				return l_dataset_WithholdingInfo;
			}
			catch 
			{
				throw;
			}
		}


		/// <summary>
		/// Method to get the Get Disbursement WithHoldingInfo info based on the selected Disbursement Ids
		/// </summary>
		/// <param name="parameterDisbId"></param>
		/// <returns></returns>


		public static DataSet GetDisbursementWithHoldingInfo( string parameterDisbursementId)
		{
			DataSet l_dataset_WithholdingInfo= null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VD_GetDisbursementWithHoldingInfo");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@StrDisbID",DbType.String,parameterDisbursementId);
				
				//SHubhrata
				LookUpCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]); 
				l_dataset_WithholdingInfo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_WithholdingInfo,"WithholdingInfo");
				System.AppDomain.CurrentDomain.SetData("dsWithholdingInfo", l_dataset_WithholdingInfo);
				return l_dataset_WithholdingInfo;
			}
			catch 
			{
				throw;
			}
		}



		public static DateTime getAccountingDate()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.NonRetiredQDRODAClass.getAccountingDate();
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
		/// <returns></returns>

		public static DataSet GetFundStatusByPersId(string parameterPersId,string parameterRequestId)
		{
			DataSet l_dataset_dsStatus = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDReverseGetStatus");
				
				if (LookUpCommandWrapper == null) return null;
				db.AddInParameter(LookUpCommandWrapper,"@PersID",DbType.String,parameterPersId);
				db.AddInParameter(LookUpCommandWrapper,"@RequestID",DbType.String,parameterRequestId);
				l_dataset_dsStatus = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsStatus,"Status");
				System.AppDomain.CurrentDomain.SetData("Status", l_dataset_dsStatus);
				return l_dataset_dsStatus;
			}
			catch 
			{
				throw;
			}

			
		}


		public static DataSet GetDeductionsList()
		{
			DataSet l_dataset_Deductions = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDGetDeductions");
				
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


		/// <summary>
		/// Added by Priya 0n 30/12/2009 :BT-1078
		/// Get Existing Withholdings List By DisbursementId
		/// </summary>
		/// <param name="disbursementId"></param>
		public static DataTable ExistingWithholdingsByDisbursement(string strRefRequestId)
		{
	
			try
			{
				

				DataSet dsExistingWithholdingsByDisbursement = null;
				DataTable dtExistingWithholdingsByDisbursement;
				Database db = null;
				DbCommand getCommandWrapper = null;

				try
				{
					db= DatabaseFactory.CreateDatabase("YRS");
			
					if (db == null) return null;

					getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_VDGetExistingWithholdingsByDisbursement");
					getCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
					db.AddInParameter(getCommandWrapper,"@RefRequestId",DbType.String,strRefRequestId);
				
					if (getCommandWrapper == null) return null;
						
					dsExistingWithholdingsByDisbursement = new DataSet();
					db.LoadDataSet(getCommandWrapper, dsExistingWithholdingsByDisbursement,"ExistingWithholdingsByDisbursement");
					dtExistingWithholdingsByDisbursement = dsExistingWithholdingsByDisbursement.Tables[0];
				
					return dtExistingWithholdingsByDisbursement;
				}
				catch 
				{
					throw;
				}
			}
			catch 
			{
				throw;
			}
		}



	}
}
