//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS Project	
// FileName			:	DailyInterestDAClass.cs
// Author Name		:	Swopna
// Employee ID		:	
// Email			:	
// Contact No		:	
// Creation Time		:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	
//*******************************************************************************
//	Date		Changed By			Description
//*******************************************************************************
//	2008.11.20	Mohammed Hafiz		Made necessary changes during the review of Amit's code.
//	2008.12.05	Mohammed Hafiz		HR : 2008.12.05 Added a new parameter for storing the flag to send email notification.
//  2015.09.16  Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************
using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for DailyInterestDA.
	/// </summary>
	public class DailyInterestDAClass
	{
		public DailyInterestDAClass()
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
				commandDailyInterest.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				//Added/Commented by Swopna,17 July 08 Start				
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
		//HR : 2008.12.05
		public static void InsertOnModification(int parameteruniqueid, string SchedulerMode, DateTime DtmScheduledDate, DateTime DtmLastRunDate, string Description, bool boolSendsuccessNotification)
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
				db.AddInParameter(l_DBCommandWrapper,"@bit_SendSuccessNotification",DbType.Boolean,boolSendsuccessNotification);
					
				l_DBCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["SmallConnectionTimeOut"]) ;
				db.ExecuteNonQuery(l_DBCommandWrapper);	
			}
			catch
			{
				throw;
			}

		}
		//****************************************************Added by Amit 19-Nov-2008
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
		//****************************************************Added by Amit 19-Nov-2008
	}
}
