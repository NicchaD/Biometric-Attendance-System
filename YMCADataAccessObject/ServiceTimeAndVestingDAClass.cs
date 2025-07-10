//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCa-YRS
// FileName			:	CashApplicationBOClass.cs
// Author Name		:	
// Employee ID		:	
// Email				:	
// Contact No		:	
// Creation Time		:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			    Date				Description
//*******************************************************************************
//Shashank Patel		    19-Sep-2013		    BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
//Manthan Rajguru           2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

#region Using Namespace
using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data.Common; 
#endregion

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for ServiceTimeAndVestingDO.
	/// </summary>
	public class ServiceTimeAndVestingDAClass
	{
		public ServiceTimeAndVestingDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region public Methods
		/*
		public DataSet GetTransactAndFundEventsRecords(string parameterTransmittalId)
		{
			
				DataSet dsTrasnsacts=null;
				Database db = null;
				DbCommand dbCommandWrapper = null;			
				try
				{
					db = DatabaseFactory.CreateDatabase("YRS");
					dbCommandWrapper= db.GetStoredProcCommand("yrs_usp_UEIN_GetTransactionsDetails");
					dsTrasnsacts= new DataSet();
					dbCommandWrapper.AddInParameter("@varchar_guiTransmittalId",DbType.String,parameterTransmittalId);
				
					db.LoadDataSet(dbCommandWrapper,dsTrasnsacts,"TransactsFundEvent");

					return dsTrasnsacts;
				}
				catch(Exception ex)
				{
					throw ex;
				}
				finally
				{
				
				}
		
			
		}
		*/

		public static bool ServiceTimeVestingUpdate(Int64 parameterFundedTransmittalLogID)
		{
			bool l_success;
			Database db = null;
			DbCommand updateCommand = null;			
			DbConnection l_DbConnection = null;
			try
			{
				db  = DatabaseFactory.CreateDatabase("YRS");
                l_DbConnection = db.CreateConnection();
                l_DbConnection.Open();
				updateCommand=db.GetStoredProcCommand("yrs_usp_ACH_CASH_UpdateServiceAndVesting");
				updateCommand.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ; 

				db.AddInParameter(updateCommand,"@numeric_FundedTransmittalLogID",DbType.Int64 ,parameterFundedTransmittalLogID);
				db.AddOutParameter (updateCommand,"@bit_Success",DbType.Boolean,1 );
				db.ExecuteNonQuery(updateCommand);
				l_success=Convert.ToBoolean(db.GetParameterValue(updateCommand,"@bit_Success")); 
				return l_success;
			}
			catch(SqlException SqlEx)
			{  
				
				
				throw SqlEx;
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
                        l_DbConnection.Close();
					}
                    l_DbConnection = null;
				}
				db=null;
				
			}
		}

		public static void RevertServiceAndVesting(string parameterTransmittalID,Database parameterDatabase,DbTransaction parameterDbTransaction)
		{
			DbCommand dbCommand=null;
			try
			{
				
					dbCommand=parameterDatabase.GetStoredProcCommand("yrs_usp_UT_RevertServiceAndVesting");
					dbCommand.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]) ;
                    parameterDatabase.AddInParameter(dbCommand, "@varchar_UnFundedTransmittalID", DbType.String, parameterTransmittalID);


                    parameterDatabase.ExecuteNonQuery(dbCommand, parameterDbTransaction);   

				
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		//Shashank Patel		19-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal 
		#region Cash Application-Person

		/// <summary>
		/// This method update the person Paid and nonpaid service and well as vesting date
		/// </summary>
		/// <param name="parameterIntFundedTransmittalLogID"></param>
		/// <returns>Boolean</returns>
		public static bool ServiceTimeVestingUpdateForPerson(Int64 parameterIntFundedTransmittalLogID)
		{
			bool bSuccess;
			Database db = null;
			DbCommand updateCommand = null;
			DbConnection dbConnection = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				dbConnection = db.CreateConnection();
				dbConnection.Open();
				updateCommand = db.GetStoredProcCommand("yrs_usp_ACH_CASH_UpdateServiceAndVestingForPerson");
				updateCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

				db.AddInParameter(updateCommand, "@numeric_FundedTransmittalLogID", DbType.Int64, parameterIntFundedTransmittalLogID);
				db.AddOutParameter(updateCommand, "@bit_Success", DbType.Boolean, 1);
				db.ExecuteNonQuery(updateCommand);
				bSuccess = Convert.ToBoolean(db.GetParameterValue(updateCommand, "@bit_Success"));
				return bSuccess;
			}
			catch (SqlException SqlEx)
			{


				throw SqlEx;
			}
			catch (Exception ex)
			{

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
					dbConnection = null;
				}
				db = null;

			}
		} 
		#endregion
		//Shashank Patel		19-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal 
	}
}
