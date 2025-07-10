//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

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
	/// Summary description for DailyInterestProcessingDOClass.
	/// </summary>
	public class InterestProcessingDAClass
	{
		public InterestProcessingDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region Public Methods
		public static DataSet GetAcctDate()
		{
			
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;			
			DataSet l_DataSet=null;
			string [] l_TableNames;
			           			     
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
					
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_MEP_GetAcctDate");
				
				l_DataSet = new DataSet("GetAcctDate");
				l_TableNames = new string [] {"GetAcctDate" };
				l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet,l_TableNames);
						
				return l_DataSet;
				
			}
			catch
			{
				throw; 
			}


		}
		public static bool UpdateAcctDate(DateTime  p_DateTime_EndingDate,DateTime p_DateTime_StartingDate,DateTime p_DateTime_AcctEndDate)
		{
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase == null)  return false;
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("DBO.yrs_usp_MEP_UpdateAccountingDate");
				l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

				if (l_DBCommandWrapper == null) return false;

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@EndingDate", DbType.DateTime, p_DateTime_EndingDate);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@StartingDate", DbType.DateTime, p_DateTime_StartingDate);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@AcctEndDate", DbType.DateTime, p_DateTime_AcctEndDate);
				
				
				l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
				return true;
				
							
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet GetServicePostLogMonthly(string p_string_ProgramName , string string_ProcessName,DateTime p_datetime_acctDate)
		{
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;		
			DataSet l_DataSet = null;
			string [] l_TableNames;
			
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_MEP_CheckServicePostMonthly");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@ProgramName", DbType.String, p_string_ProgramName);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@ProcessName", DbType.String, string_ProcessName);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@began", DbType.DateTime, p_datetime_acctDate);
						                    
				l_TableNames = new string [] {"ServicePostMonthly"};

				l_DataSet = new DataSet(); 
				l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet,l_TableNames);
								
				if (l_DataSet == null)
				{
					// Log the exception
					return null;
				}
				else
				{
					return l_DataSet;
				}
			}
			catch
			{
				throw;
			}

		}
		public static bool ProcessRegularIntrest(DateTime ldBegDate, DateTime ldEndDate,string parameterProcessType)
		{
			
			try
			{
					
				Database l_DataBase = null;
			    DbCommand  l_DBCommandWrapper = null;			
							
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
														
				if (l_DataBase == null) return false;
								
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("Dbo.yrs_usp_MEP_INT_Regular");
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return false;
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@BegDate", DbType.DateTime, ldBegDate);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@EndDate", DbType.DateTime, ldEndDate);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@ProcessType", DbType.String, parameterProcessType);
								
				l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
			
				return true;
			}
			catch
			{
				throw;
			}
			
		}

		public static bool ProcessUnearnedMonthlyIntrest()
		{
			
			try
			{
					
				Database l_DataBase = null;
			    DbCommand  l_DBCommandWrapper = null;			
							
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
														
				if (l_DataBase == null) return false;
								
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("Dbo.yrs_usp_MEP_INT_Unfunded_Monthly");
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
				
				if (l_DBCommandWrapper == null) return false;
												
				l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
			
				return true;
			}
			catch
			{
				throw;
			}
			
		}
		public static DataSet GetDailyIntPostLog(DateTime p_datetime_RunDate)
		{
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;		
			DataSet l_DataSet = null;
			string [] l_TableNames;
			
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_MEP_GetDailyIntLog");

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@datetime_RunDate", DbType.DateTime, p_datetime_RunDate);
						                    
				l_TableNames = new string [] {"DailyIntPostLog"};

				l_DataSet = new DataSet(); 
				l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet,l_TableNames);
								
				if (l_DataSet == null)
				{
					// Log the exception
					return null;
				}
				else
				{
					return l_DataSet;
				}
			}
			catch
			{
				throw;
			}

		}
		public static string GetNewGuidString()
		{
			DataSet dsNewGuid = null;
			Database db = null;
			DbCommand  getCommandWrapper = null;
			string l_string_Guid = string.Empty; 

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_PAYROLL_GetNewGuid");
			
				if (getCommandWrapper == null) return null;
					
				dsNewGuid = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsNewGuid,"NewGuid");

				if (dsNewGuid !=  null)
				{
					l_string_Guid = dsNewGuid.Tables[0].Rows[0]["NewGuid"].ToString();   
					return l_string_Guid.Trim(); 
				}
				else
				{
					return string.Empty; 
				}
			}
			catch 
			{
				throw;
			}
		}
		public static DataSet getMetaOutputFileType(string p_string_search)
		{
			DataSet dsMetaOutputFileType = null;
			Database db = null;
			DbCommand  getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand ("yrs_usp_MetaOutputFileType");
				
				if (getCommandWrapper == null) return null;
					
				db.AddInParameter(getCommandWrapper,"@char_MetaOutputFileType",DbType.String,p_string_search);

				dsMetaOutputFileType = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsMetaOutputFileType,"MetaOutputFileType");
				
				if (dsMetaOutputFileType != null)
				{
					
					return dsMetaOutputFileType;
				}
				else
				{
					return null;
				}
			}
			catch 
			{
				throw;
			}
		}

		//Added by Ashish on 17-Dec-2008
		public static bool ProcessVestingDueToAge(DateTime ldMonthEndDate)
		{
			string l_str_OutPutMessage;
			try
			{
					
				Database l_DataBase = null;
			    DbCommand  l_DBCommandWrapper = null;			
							
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
														
				if (l_DataBase == null) return false;
								
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("Dbo.yrs_usp_MEP_UpdateVestingStatusByAge");
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return false;
				
				l_DataBase.AddInParameter(l_DBCommandWrapper,"@datetime_MonthEndDate",DbType.DateTime,ldMonthEndDate);
                l_DataBase.AddOutParameter(l_DBCommandWrapper, "@cOutput", DbType.String, 1000);
								
				l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
                l_str_OutPutMessage = l_DataBase.GetParameterValue(l_DBCommandWrapper,"@cOutput").ToString();
				if(l_str_OutPutMessage != "")
				{
					throw new Exception(l_str_OutPutMessage); 
				}
											
				
				return true;
			}
			catch
			{
				throw;
			}
			
		}
/* Commented by Ashish on 06-Oct-2008 

		public static DataSet GetDailyIntErrorLog(DateTime p_datetime_RunDate)
		{
			Database l_DataBase = null;
			DBCommandWrapper l_DBCommandWrapper = null;		
			DataSet l_DataSet = null;
			string [] l_TableNames;
			
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommandWrapper("dbo.yrs_usp_DlyInt_GetDlyIntErrorLog");
				
				l_DBCommandWrapper.AddInParameter("@datetime_LastRunDate",DbType.DateTime,p_datetime_RunDate);
						                    
				l_TableNames = new string [] {"DailyIntErrorLog"};

				l_DataSet = new DataSet(); 
				l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet,l_TableNames);
								
				if (l_DataSet == null)
				{
					// Log the exception
					return null;
				}
				else
				{
					return l_DataSet;
				}
			}
			catch
			{
				throw;
			}

		}

		public static bool SaveDlyIntErrorLog(Int64 parameterDlyIntErrorLogID,DateTime parameterRunDate,string parameterException)
		{
			try
			{
					
				Database l_DataBase = null;
				DBCommandWrapper l_DBCommandWrapper = null;			
							
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
														
				if (l_DataBase == null) return false;
								
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommandWrapper("dbo.yrs_usp_DlyInt_SaveDlyIntErrorLog");
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MediumConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return false;
				l_DBCommandWrapper.AddInParameter("@datetime_LastRunDate",DbType.DateTime,parameterRunDate);
				if(parameterDlyIntErrorLogID !=0)
				{
					l_DBCommandWrapper.AddInParameter("@numeric_ErrorLogID",DbType.Int64,parameterDlyIntErrorLogID);
				}
				l_DBCommandWrapper.AddInParameter("@varchar_Exception",DbType.String,parameterException);
								
				l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
			
				return true;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
*/
		#endregion
	}
}
