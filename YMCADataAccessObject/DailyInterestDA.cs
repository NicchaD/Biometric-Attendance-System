//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for DailyInterestDA.
	/// </summary>
	public class DailyInterestDA
	{
		public DailyInterestDA()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpDailyInterest()
		{
			DataSet dsDailyInterest = null;
			Database db= null;
			string [] l_TableNames;//Added by Swopna,17 July 08
            
			DbCommand commandDailyInterest = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandDailyInterest = db.GetStoredProcCommand("yrs_usp_DI_SearchDailyInterest");
				if (commandDailyInterest==null) return null;
				dsDailyInterest = new DataSet();
				//commandDailyInterest.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
                commandDailyInterest.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				//Added/Commented by Swopna,17 July 08 Start
				//db.LoadDataSet(commandDailyInterest,dsDailyInterest,"DailyInterest");
				l_TableNames = new string[]{"DlyIntController","DlyIntLog"} ;
				db.LoadDataSet(commandDailyInterest,dsDailyInterest,l_TableNames);
				//Added/Commented by Swopna,17 July 08 End
				return dsDailyInterest;
			}
			catch 
			{
				throw ;
			}

		}
		//		public static void UpdateDailyInterest(string parametermode,string parameterdescription,string parameterstarttime,int parameteruniqueid)
		//		{
		//			
		//			Database db= null;
		//			DBCommandWrapper commandDailyInterest = null;
		//
		//			try
		//			{
		//				db= DatabaseFactory.CreateDatabase("YRS");
		//				
		//				//if(db==null) return null;
		//				commandDailyInterest = db.GetStoredProcCommandWrapper("yrs_usp_DI_UpdateDailyInterest");
		//				//if (commandDailyInterest==null) return null;
		//				commandDailyInterest.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationSettings.AppSettings ["MediumConnectionTimeOut"]); 
		//				commandDailyInterest.AddInParameter("@varchar_mode",DbType.String,parametermode);
		//				commandDailyInterest.AddInParameter("@varchar_description",DbType.String,parameterdescription);
		//				commandDailyInterest.AddInParameter("@varchar_starttime",DbType.String,parameterstarttime);
		//				commandDailyInterest.AddInParameter("@numeric_uniqueid",DbType.Int32,parameteruniqueid);
		//				db.ExecuteNonQuery(commandDailyInterest);
		//				
		//			}
		//			catch 
		//			{
		//				throw ;
		//			}
		//
		//		}
		public static void InsertOnModification(int parameteruniqueid,string SchedulerMode,DateTime DtmScheduledDate,DateTime DtmLastRunDate,string Description)
		{
			DbCommand l_DBCommandWrapper = null;
			Database db = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");				
																								
				l_DBCommandWrapper = db.GetStoredProcCommand("yrs_usp_DI_InsertOnModification_ProgramController");
				db.AddInParameter(l_DBCommandWrapper,"@numeric_uniqueid",DbType.Int32,parameteruniqueid);
				db.AddInParameter(l_DBCommandWrapper,"@varchar_SchedulerMode",DbType.String,SchedulerMode);
				db.AddInParameter(l_DBCommandWrapper,"@datetime_DtmScheduledDate",DbType.DateTime,DtmScheduledDate);
				db.AddInParameter(l_DBCommandWrapper,"@datetime_DtmLastRunDate",DbType.DateTime,DtmLastRunDate);
				db.AddInParameter(l_DBCommandWrapper,"@varchar_ChvDescription",DbType.String,Description);
					
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]) ;
				db.ExecuteNonQuery(l_DBCommandWrapper);	
			}
			catch
			{
				throw;
			}

		}
		//****************************************************Added by Amit 19-Nov-2008
		public static string getSendMailFlag()
		{
			string l_string_Flag = "0";
			try
			{
				DataSet l_dataset = YMCACommonDAClass.getConfigurationValue("DLY_INT_PROC_SENDSUCCESSMAIL");
				if (l_dataset.Tables.Count > 0)  
					if (l_dataset.Tables[0].Rows.Count > 0)						
						if (l_dataset.Tables[0].Rows[0]["Value"].ToString().Trim()!="")
							l_string_Flag = l_dataset.Tables[0].Rows[0]["Value"].ToString().Trim();

				return l_string_Flag;
			}
			catch 
			{
				throw;
			}		
		}
		public static string  getStartTime()
		{
			string l_string_StartTime = string.Empty;
			try
			{
				DataSet l_dataset = YMCACommonDAClass.getConfigurationValue("BUSINESS_START_TIME");
				if (l_dataset.Tables.Count > 0)  
					if (l_dataset.Tables[0].Rows.Count > 0)						
						l_string_StartTime = l_dataset.Tables[0].Rows[0]["Value"].ToString().Trim();

				return l_string_StartTime;
			}
			catch 
			{
				throw;
			}
		}
		public static string  getEndTime()
		{
			string l_string_EndTime = string.Empty;
			try
			{
				DataSet l_dataset = YMCACommonDAClass.getConfigurationValue("BUSINESS_END_TIME");
				if (l_dataset.Tables.Count > 0)  
					if (l_dataset.Tables[0].Rows.Count > 0)						
						l_string_EndTime = l_dataset.Tables[0].Rows[0]["Value"].ToString().Trim();

				return l_string_EndTime;
			}
			catch 
			{
				throw;
			}
		}
		public static void InsertDlyIntProcSendSuccessMail(int MailValue)
		{
			DbCommand insertCommandWrapper = null;
			Database db = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");	
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_DI_UpdateSendSuccessMailFlag");
				db.AddInParameter(insertCommandWrapper,"@bit_MailValue",DbType.Int16,MailValue);
				db.ExecuteNonQuery(insertCommandWrapper);
			}
			catch 
			{
				throw;
			}
		}
		//****************************************************Added by Amit 19-Nov-2008
	}
}
