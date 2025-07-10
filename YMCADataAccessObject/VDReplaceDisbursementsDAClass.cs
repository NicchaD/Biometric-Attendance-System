//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for VDReplaceDisbursementsDAClass.
	/// </summary>
	public sealed class VDReplaceDisbursementsDAClass
	{
		public VDReplaceDisbursementsDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
/// <summary>
/// To fetch the list of address based on the entity id that is passed
/// </summary>
/// <param name="parameterEntityId"></param>
/// <returns></returns>
		public static DataSet LookUpAddress(string parameterEntityId)
		{	
			DataSet l_dataset_Addresslist = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDGetAddressInfo");
				if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@EntityId", DbType.String, parameterEntityId);
				
				l_dataset_Addresslist = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Addresslist,"AddressList");
				System.AppDomain.CurrentDomain.SetData("dsAddressList", l_dataset_Addresslist);
				return l_dataset_Addresslist;
			}
			catch
			{
				throw;
			}


			
		}
		public static DataSet LookUpLatestAddress(string parameterEntityId)
		{	
			DataSet l_dataset_LatestAddress = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			string parameterUniqueID="";

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDReplaceGetAddress");
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@chv_entityid",DbType.String,parameterEntityId);
                db.AddOutParameter(LookUpCommandWrapper, "@chv_uniqueid", DbType.String, 100);
                l_dataset_LatestAddress = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_LatestAddress,"LatestAddress");
                parameterUniqueID = Convert.ToString(db.GetParameterValue(LookUpCommandWrapper, "@chv_uniqueid"));
				System.AppDomain.CurrentDomain.SetData("dsAddressList", l_dataset_LatestAddress);
				return l_dataset_LatestAddress;
			}
			catch
			{
				throw;
			}


			
		}

		public static string ReplaceDisbursement(string parameterDisbId,string parameterAddId)
		{
			String l_string_Output ;
			Database db = null;
			DbCommand ReplaceCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				
				ReplaceCommandWrapper = db.GetStoredProcCommand("yrs_usp_VDReplaceDisbursement");
				if (ReplaceCommandWrapper == null) return null;

				db.AddInParameter(ReplaceCommandWrapper,"@nDisbId",DbType.String,parameterDisbId);
                db.AddInParameter(ReplaceCommandWrapper, "@nAddrId", DbType.String, parameterAddId);

                db.AddOutParameter(ReplaceCommandWrapper, "@nOutput", DbType.Int32, 9);
				ReplaceCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
				db.ExecuteNonQuery(ReplaceCommandWrapper);
                l_string_Output = Convert.ToString(db.GetParameterValue(ReplaceCommandWrapper, "@nOutput"));
				return l_string_Output;
				
			}
			catch
			{
				throw;
			}


		}
		public static string ReplaceLoan(string parameterDisbId,string parameterAddId)
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
				l_DBCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_Void_LoanReplace");
				l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return "" ;
				db.AddInParameter(l_DBCommandWrapper,"@chv_DisbID",DbType.String,parameterDisbId);
                db.AddInParameter(l_DBCommandWrapper, "@chv_AddrID", DbType.String, parameterAddId);
                db.AddOutParameter(l_DBCommandWrapper, "@stringProcess", DbType.String, 100);
				db.ExecuteNonQuery (l_DBCommandWrapper,l_IDbTransaction);
                l_string_errormessage = Convert.ToString(db.GetParameterValue(l_DBCommandWrapper, "@stringProcess"));
				if (l_string_errormessage !="")
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
		public static string ReplaceCashOuts(string parameterDisbId,string parameterAddId,int parameterReplaceFees,out int parameterZeroFundingStatus)
		{
			DbCommand l_DBCommandWrapper;
			Database db = null;
			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection=null;
			string l_string_errormessage=" ";
			parameterZeroFundingStatus = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return "";

				
				l_IDbConnection = db.CreateConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return null;

				l_IDbTransaction = l_IDbConnection.BeginTransaction();
				l_DBCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_Void_CashOutsReplace");
				l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return "" ;
                db.AddInParameter(l_DBCommandWrapper, "@chv_DisbID", DbType.String, parameterDisbId);
                db.AddInParameter(l_DBCommandWrapper, "@chv_AddrID", DbType.String, parameterAddId);
                db.AddInParameter(l_DBCommandWrapper, "@numericReplaceFee", DbType.Int32, parameterReplaceFees);
                db.AddOutParameter(l_DBCommandWrapper, "@stringProcess", DbType.String, 100);
                db.AddOutParameter(l_DBCommandWrapper, "@bit_ZeroFundingStatus", DbType.Int32, 2);

				db.ExecuteNonQuery (l_DBCommandWrapper,l_IDbTransaction);
				l_string_errormessage = Convert.ToString(db.GetParameterValue(l_DBCommandWrapper,"@stringProcess"));
                parameterZeroFundingStatus = Convert.ToInt32(db.GetParameterValue(l_DBCommandWrapper, "@bit_ZeroFundingStatus"));
				if (l_string_errormessage !="")
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

	}
}
