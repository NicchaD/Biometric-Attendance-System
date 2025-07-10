//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS Project	
// FileName			:	DailyInterestBOClass.cs
// Author Name		:	Swopna
// Employee ID		:	
// Email			:	
// Contact No		:	
// Creation Time	:	
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
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for DailyInterestBOClass.
	/// </summary>
	public class DailyInterestBOClass
	{
		public DailyInterestBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpDailyInterest()
		{
			try
			{
				return DailyInterestDAClass.LookUpDailyInterest();
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
		//HR : 2008.12.05
		public static void InsertOnModification(int parameteruniqueid, string SchedulerMode, DateTime DtmScheduledDate, DateTime DtmLastRunDate, string Description, bool boolSendSuccessNotification)
		{
			try
			{
				 YMCARET.YmcaDataAccessObject.DailyInterestDAClass.InsertOnModification(parameteruniqueid, SchedulerMode, DtmScheduledDate, DtmLastRunDate, Description, boolSendSuccessNotification);
			}
			catch
			{
				throw;
			}

		}
		//****************************************************Added by Amit 19-Nov-2008
		public static string getStartTime()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DailyInterestDAClass.getStartTime();
                					
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
				return YMCARET.YmcaDataAccessObject.DailyInterestDAClass.getEndTime();
					
			}
			catch 
			{
				throw;
			}
		}		
		//****************************************************Added by Amit 19-Nov-2008

	}
}
