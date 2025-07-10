//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for DailyInterestBO.
	/// </summary>
	public class DailyInterestBO
	{
		public DailyInterestBO()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpDailyInterest()
		{
			try
			{
				return DailyInterestDA.LookUpDailyInterest();
			}	
			catch
			{
				throw;
			}
		}
//		public static void UpdateDailyInterest(string parametermode,string parameterdescription,string parameterstarttime,int parameteruniqueid)
//		{
//			try
//			{
//				DailyInterestDA.UpdateDailyInterest(parametermode,parameterdescription,parameterstarttime,parameteruniqueid);
//			}	
//			catch
//			{
//				throw;
//			}
//		}
		public static void InsertOnModification(int parameteruniqueid,string SchedulerMode,DateTime DtmScheduledDate,DateTime DtmLastRunDate,string Description)
		{
			try
			{
				 YMCARET.YmcaDataAccessObject.DailyInterestDA.InsertOnModification(parameteruniqueid,SchedulerMode,DtmScheduledDate,DtmLastRunDate,Description);
			}
			catch
			{
				throw;
			}

		}
		//****************************************************Added by Amit 19-Nov-2008
		public static string getSendMailFlag()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DailyInterestDA.getSendMailFlag();
                					
			}
			catch 
			{
				throw;
			}
		}

		public static string getStartTime()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DailyInterestDA.getStartTime();
                					
			}
			catch 
			{
				throw;
			}
		}
		public static string getEndTime()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DailyInterestDA.getEndTime();
					
			}
			catch 
			{
				throw;
			}
		}
		public static void InsertDlyIntProcSendSuccessMail(int MailValue)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.DailyInterestDA.InsertDlyIntProcSendSuccessMail(MailValue);
			}
			catch 
			{
				throw;
			}
		}
		//****************************************************Added by Amit 19-Nov-2008

	}
}
