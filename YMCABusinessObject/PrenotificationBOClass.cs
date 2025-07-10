//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//prasad Jadhav          BT:645   2011-10-04  for YRS 5.0-632 : Test database output files need word "test" in them.
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//****************************************************

using System;
using YMCARET.YmcaDataAccessObject;
using System.Data;
using System.Collections;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for Prenotification.
	/// </summary>
	public class Prenotification
	{
		public Prenotification()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet getPrenotificationLogs()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.PreNotificationDAClass.getPrenotificationLogs()); 
			}
			catch
			{
				throw;
			}

		}
		public static DataSet getPrenotificationList()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.PreNotificationDAClass.getPrenotificationList()); 
			}
			catch
			{
				throw;
			}

		}

		public static DataSet MetaOutputFileType()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.PreNotificationDAClass.getMetaOutputFileType()); 
				
			}
			catch
			{
				throw;
			}

		}

		public static Boolean  UpdateEFTStatus(DataSet parameterPrenotificationLists,DateTime parameter_date_began,Boolean Parameter_ProcessLog)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.PreNotificationDAClass.UpdatePrenotificationStatus(parameterPrenotificationLists); 
				if (Parameter_ProcessLog) 
				{
					YMCARET.YmcaDataAccessObject.PreNotificationDAClass.UpdatePrenotificationLogs(parameter_date_began); 
				}
				return true;
				
			}
			catch
			{
				throw;
			}
		}

		public static Boolean  SaveEFTStatus(DataSet parameterPrenotificationLists)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.PreNotificationDAClass.UpdatePrenotificationStatus(parameterPrenotificationLists); 
					
				return true;
				
			}
			catch
			{
				throw;
			}
		}

		public static Boolean  SaveAndInsertEFTStatusValues(DataSet parameter_PrenotificationLists,DateTime parameter_Date_began, String parameter_String_OutputDirectory,String parameter_String_OutputFileName)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.PreNotificationDAClass.UpdatePrenotificationStatus(parameter_PrenotificationLists); 
				YMCARET.YmcaDataAccessObject.PreNotificationDAClass.InsertPrenotificationLog(parameter_String_OutputDirectory,parameter_String_OutputFileName);  
				YMCARET.YmcaDataAccessObject.PreNotificationDAClass.InsertPrenotificationOutPutFile(parameter_Date_began); 
					
				return true;
				
			}
			catch
			{
				throw;
			}

		}
        //Added by prasad YRS 5.0-632 : Test database output files need word "test" in them.
        public static  string GetPreNoteMode()
        {
            return   YMCARET.YmcaDataAccessObject.PreNotificationDAClass.getPreNoteModeFromDatabase();
        }


	}
}
