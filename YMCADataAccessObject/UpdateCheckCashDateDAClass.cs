//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	UpdateCheckCashDateDAClass.cs
// Author Name		:	Shashi Shekhar
// Employee ID		:	51426
// Email			:	shashi.singh@3i-infotech.com
// Contact No		:	
// Creation Time	:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			    Date				Description
//*******************************************************************************
// Shashi Shekhar           2010-06-03          Migration related changes.
// Shashi Shekhar           2010-07-07          Code review changes.
// Manthan Rajguru          2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System.Data ;
using System;
using System.Collections ;
using System.Text ;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for UpdateCheckCashDateDAClass.
	/// </summary>
	public class UpdateCheckCashDateDAClass
	{
	 
		public UpdateCheckCashDateDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}



		/// <summary>
		/// To get the Disbursement details
		/// </summary>
		/// <param name="parameterCheckNo">Disbursement No</param>
		/// <returns>DataSet</returns>
		public static DataSet GetCheckDetails( string  parameterCheckNo) 
		{
			DataSet dsGetCheckDetails = null;
			Database db= null;
			DbCommand  commandGetCheckDetails = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				
				commandGetCheckDetails = db.GetStoredProcCommand ("yrs_usp_UpdateCashedCheck_GetCheckDetails");
				
				if (commandGetCheckDetails==null) return null;
				commandGetCheckDetails.CommandTimeout = System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["ExtraLargeConnectionTimeOut"] );
				dsGetCheckDetails = new DataSet();
				db.AddInParameter(commandGetCheckDetails,"@varchar_CheckNo",DbType.String,parameterCheckNo);
				
				db.LoadDataSet(commandGetCheckDetails,dsGetCheckDetails,"Check");
				return dsGetCheckDetails;
			}
			catch 
			{
				throw ;
			}

		}


		
		/// <summary>
		/// To get the Check Details dataset
		/// </summary>
		/// <param name="alCheckNo">Disbursement No</param>
		/// <returns>Dataset</returns>
		public static DataSet GetCheckDetailsDataSet(ArrayList alCheckNo) 
		{  
			DataSet l_DisbursementExistingDataSet=new DataSet ();
			DataSet l_DisburseCurrentDataSet = new DataSet() ; 
			StringBuilder builder = new StringBuilder();
	
			try 
			{ 
				if (alCheckNo!=null)
				{
					foreach(string val in alCheckNo)
					{ 
						if (builder.Length < 1900) 
						{ 
							builder.Append( val + ","); 
						} 
						else 
						{ 
							builder.Append( val + ","); 
							builder = builder.Remove(builder.Length - 1, 1); 
							l_DisburseCurrentDataSet = GetCheckDetails(builder.ToString()); 
							l_DisbursementExistingDataSet.Merge(l_DisburseCurrentDataSet,false,MissingSchemaAction.Add); 	//Merging dataset 
							builder.Length=0   ; 
						} 
					} 
        
					if (builder!=null) 
					{ 
						builder = builder.Remove(builder.Length - 1, 1); 
						l_DisburseCurrentDataSet = GetCheckDetails(builder.ToString()); 
						l_DisbursementExistingDataSet.Merge(l_DisburseCurrentDataSet,false,MissingSchemaAction.Add); 	//Merging dataset 
						//int test=l_DisbursementExistingDataSet.Tables[0].Rows.Count;
						builder.Length=0  ; 
					} 
					return l_DisbursementExistingDataSet; 
				}
				return null;
			} 
			catch 
			{ 
				throw ;
			} 
			finally 
			{ 
				builder=null;
				l_DisbursementExistingDataSet=null;
				l_DisburseCurrentDataSet=null;
			} 
		} 






		/// <summary>
		///  Update bitPaid field of Disbursement No in table "atsDisbursementFunding"
		/// </summary>
		/// <param name="guiUniqueId">Unique Identifier</param>
		public static void UpdateCashedCheckPaid(string guiUniqueId)
		{
			Database db = null;
		    DbTransaction  l_IDbTransaction = null;
		    DbConnection 	 l_IDbConnection = null;
			DbCommand  UpdateCommandWrapper = null;
		
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return ;

				l_IDbConnection =db.CreateConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return;
				l_IDbTransaction = l_IDbConnection.BeginTransaction();
				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdateCashedcheck_UpdatebitPaid");
				if (UpdateCommandWrapper == null) return ;
				
				db.AddInParameter(UpdateCommandWrapper,"@varchar_guiUniqueID",DbType.String,guiUniqueId);
				db.ExecuteNonQuery(UpdateCommandWrapper,l_IDbTransaction);
				
				l_IDbTransaction.Commit();	
			}

			catch(SqlException SqlEx)
			{  
				l_IDbTransaction.Rollback();
				throw SqlEx;
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
						l_IDbConnection.Close ();
					}
					l_IDbConnection=null;
				}
				db=null;
				l_IDbTransaction=null;
			}

		}


		/// <summary>
		/// Update bitPaid field of Disbursement No in table "atsDisbursementFunding"
		/// </summary>
		/// <param name="alUniqueId"></param>
		public static void UpdateCashedChecks(ArrayList alUniqueId) 
		{  
			StringBuilder builder = new StringBuilder();
			try 
			{ 
				foreach(string val in alUniqueId)
				{ 
					if (builder.Length < 1900) 
					{ 
						builder.Append( val + ","); 
					} 
					else 
					{ 
						builder.Append( val + ","); 
						builder = builder.Remove(builder.Length - 1, 1); 
						UpdateCashedCheckPaid(builder.ToString()); 
						builder.Length=0   ; 
					} 
				} 
        
				if (builder!=null) 
				{ 
					builder = builder.Remove(builder.Length - 1, 1); 
					UpdateCashedCheckPaid(builder.ToString()); 
					builder.Length=0  ; 
				} 
				
			} 
			catch 
			{ 
				throw ;
			} 
			finally 
			{ 
				builder=null;
				alUniqueId=null;
		
			} 
		} 

        //Shashi Shekhar Singh:19-01-2010: For YRS 5.0-970
        /// <summary>
        /// To get the Outfut file details for cashed check date module
        /// </summary>
        /// <returns>Dataset</returns>
		public static DataSet GetExceptionMetaOutputFileType()
		{
			DataSet dsMetaOutputFileType = null;
			Database db = null;
			DbCommand  getCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                getCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetMetaOutputFileType");
                if (getCommandWrapper == null) return null;
                db.AddInParameter(getCommandWrapper, "@char_MetaOutputFileType", DbType.String, "UCCD");
                dsMetaOutputFileType = new DataSet();
                db.LoadDataSet(getCommandWrapper, dsMetaOutputFileType, "MetaOutputFileType");
                return dsMetaOutputFileType;
            }
            catch
            {
                throw;
            }
            
		}


		//----------------------------------------------------------------

		


	}
}
