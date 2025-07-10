/******************************************************************************************************************
 Copyright YMCA Retirement Fund All Rights Reserved. 
 
 Project Name		:	YMCADataAccessObject
 BOClassName		:	EDIExculsionListDAClass.cs
 Author Name		:	Ashutosh Patil
 Employee ID		:	36307
 Email				:	ashutosh.patil@3i-infotech.com
 Contact No			:	8568
 Creation Date		:	30-Apr-2007
******************************************************************************************************************** 
Modification History
********************************************************************************************************************
Modified By        Date            Description
********************************************************************************************************************
Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
********************************************************************************************************************/
using System;
using System.Data;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for EDIExculsionListDAClass.
	/// </summary>
	public class EDIExculsionListDAClass
	{
		public EDIExculsionListDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet GetEDIExlusionList()
		{
			DataSet l_dataset_dsGetEDIListInfo = null;
			Database db = null;
			DbCommand  LookUpCommandWrapper = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_PayrollExclusions_SelectParticipantsfromList");
				if (LookUpCommandWrapper == null) return null;
				l_dataset_dsGetEDIListInfo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsGetEDIListInfo,"EDIList");
				return l_dataset_dsGetEDIListInfo;
			}
			catch 
			{
				throw;
			}
		}
		public static void  InsertParticipantsintoList(DataSet DataSetEDIExlusionlist)
		{
			Database l_DataBase = null;
			//DBCommandWrapper l_DBCommandWrapper = null;
			DbConnection  DBconnectYRS;
			bool l_bool_TransactionStarted = false;
			DbTransaction  DBTransaction;

			

				
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");
				DBconnectYRS = l_DataBase.CreateConnection ();
				DBconnectYRS.Open ();
				DBTransaction =  DBconnectYRS.BeginTransaction (System.Data.IsolationLevel.ReadUncommitted);
				l_bool_TransactionStarted = true;

				try
				{
					if (l_DataBase != null) 
					{
						Database db = null;
						DbCommand  insertCommandWrapper = null;
						DbCommand  deleteCommandWrapper = null;
						DataTable l_dataTableInsert=null;
						DataTable l_dataTableDelete =null;
						DataRow[] l_datarow_EDIInsert=null;
						DataRow[] l_datarow_EDIDelete=null;

						db=DatabaseFactory.CreateDatabase("YRS");
						l_dataTableInsert = DataSetEDIExlusionlist.Tables[0];
						l_dataTableDelete = DataSetEDIExlusionlist.Tables[1];
						l_datarow_EDIInsert=l_dataTableInsert.Select("bitAdd=1");
						l_datarow_EDIDelete=l_dataTableDelete.Select("bitDelete=1");
				
						foreach (DataRow l_datarow_Delete in l_datarow_EDIDelete)
						{
							deleteCommandWrapper=db.GetStoredProcCommand ("yrs_usp_PayrollExclusions_RemoveParticipantsfromList");
							db.AddInParameter(deleteCommandWrapper,"@guiUniqueId",DbType.String,l_datarow_Delete["UniqueId"].ToString());
					
							if (deleteCommandWrapper != null)
							{
								db.ExecuteNonQuery(deleteCommandWrapper,DBTransaction); 
							}
					
						}
				
						foreach (DataRow l_datarow_Insert in l_datarow_EDIInsert)
						{
							insertCommandWrapper=db.GetStoredProcCommand ("yrs_usp_PayrollExclusions_InsertParticipantsintoList");
							db.AddInParameter(insertCommandWrapper,"@guiPersId",DbType.String,l_datarow_Insert["PerssId"].ToString());
							db.AddInParameter(insertCommandWrapper,"@strReason",DbType.String,l_datarow_Insert["Reason"].ToString());
							if (insertCommandWrapper != null)
							{
								db.ExecuteNonQuery(insertCommandWrapper,DBTransaction); 
								//return true;
							}
					
						}
						DBTransaction.Commit(); 
						DBconnectYRS.Close(); 
					}
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
	} 
	
}
