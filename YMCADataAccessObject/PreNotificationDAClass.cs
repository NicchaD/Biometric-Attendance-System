//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//prasad Jadhav       2011-10-04     For BT 645 for YRS 5.0-632 : Test database output files need word "test" in them.
//Manthan Rajguru     2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//****************************************************

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for PreNotificationDAClass.
	/// </summary>
	public class PreNotificationDAClass
	{
		public PreNotificationDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//Function returning dataset for the List of records which are necessary for pre Notifcation process.
		//The DataSet contains the rows where EFTValid = 0  AND  EftStatus <> 'O' 
		public static DataSet getPrenotificationList()
		{
			DataSet dsPrenotificationList = null;
			Database db = null;
			DbCommand  getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand ("yrs_usp_prenotificationlist");
				
				if (getCommandWrapper == null) return null;
						
				dsPrenotificationList = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsPrenotificationList,"Pre Notification List");
				
				return dsPrenotificationList;
			}
			catch 
			{
				throw;
			}
		}

		//Function returning dataset for the List of records from the Log.
		//The DataSet contains the rows where programname = 'PRENOTIFICATION' and processname = 'VERIFICATION' and ended is null 
		public static DataSet getPrenotificationLogs()
		{
			DataSet dsPrenotificationLogs = null;
			Database db = null;
			DbCommand  getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand ("yrs_usp_cPreNot");
				
				if (getCommandWrapper == null) return null;
						
				dsPrenotificationLogs = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsPrenotificationLogs,"Pre Notification Logs");
				
				return dsPrenotificationLogs;
			}
			catch 
			{
				throw;
			}
		}


		//Function returning dataset for the pre defined settings in Meta Output File type table. 
		//The DataSet contains the rows where outputfiletype = 'EFT'

		public static DataSet getMetaOutputFileType()
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
					
				db.AddInParameter(getCommandWrapper,"@char_MetaOutputFileType",DbType.String,"EFTPRE");

				dsMetaOutputFileType = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsMetaOutputFileType,"MetaOutputFileType");
				
				return dsMetaOutputFileType;
			}
			catch 
			{
				throw;
			}
		}

		public static void  UpdatePrenotificationStatus(DataSet parameterUpdatePreNotificationList)
		{
			//DataSet dsPrenotificationList = null;
			Database db = null;
			DbCommand  InsertCommandWrapper = null;
			DbCommand  UpdateCommandWrapper = null;
			DbCommand  DeleteCommandWrapper = null;
            
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
              	UpdateCommandWrapper = db.GetStoredProcCommand ("yrs_usp_UpdatePreNotificationStatus");
				db.AddInParameter(UpdateCommandWrapper,"@UniqueID",DbType.Guid ,"UniqueID",DataRowVersion.Current);
				db.AddInParameter(UpdateCommandWrapper,"@char_EFTSTATUS",DbType.String ,"Eft Status",DataRowVersion.Current);
				db.AddInParameter(UpdateCommandWrapper,"@char_EFTText",DbType.String ,"EftText",DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper,"@datetime_EfftProcessDate",DbType.DateTime ,"EftProcessDate",DataRowVersion.Current);
				db.AddInParameter(UpdateCommandWrapper,"@Selected",DbType.Boolean ,"Selected",DataRowVersion.Current);
			
				if (parameterUpdatePreNotificationList != null)
				db.UpdateDataSet(parameterUpdatePreNotificationList,"Pre Notification List" ,InsertCommandWrapper,UpdateCommandWrapper,DeleteCommandWrapper,UpdateBehavior.Standard);
			}
			catch 
			{
				throw;
			}
		}

		public static void UpdatePrenotificationLogs(DateTime  parameter_datetime_began)
		{
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase != null) 
				{

					l_DBCommandWrapper = l_DataBase.GetStoredProcCommand  ("yrs_usp_UpdatePreNotificationLogs");

					if (l_DBCommandWrapper != null) 
					{
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@datetime_began", DbType.DateTime, parameter_datetime_began);
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@datetime_Ended", DbType.DateTime, System.DateTime.Now);
						l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
					}
				}
									
			}
			catch 
			{
				throw;
			}
		}

		public static void InsertPrenotificationOutPutFile(DateTime  parameter_datetime_began)
		{
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase != null) 
				{

					l_DBCommandWrapper = l_DataBase.GetStoredProcCommand  ("yrs_usp_InsertPreNotificationLogs");

					if (l_DBCommandWrapper != null) 
					{
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@datetime_began", DbType.DateTime, parameter_datetime_began);
												
						l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
					}
				}
									
			}
			catch
			{
				throw;
			}
		}

		public static void InsertPrenotificationLog(string Parameter_String_OutputFileLocation,string Parameter_String_FileName)
		{
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase != null) 
				{

					l_DBCommandWrapper = l_DataBase.GetStoredProcCommand  ("yrs_usp_InsertPreNotificationOutPutFile");

					if (l_DBCommandWrapper != null) 
					{
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@OutputFileLocation", DbType.String, Parameter_String_OutputFileLocation);
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@OutputFileName", DbType.String, Parameter_String_FileName);
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@OutputFileType", DbType.String, "EFTPRE");
						
						l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);
					}
				}
									
			}
			catch 
			{
				
				throw;
			}
		}

        // Added by prasad YRS 5.0-632 : Test database output files need word "test" in them.
        public static string getPreNoteModeFromDatabase()
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            IDbConnection DBconnectYRS = null;
            DataSet ds = null;
            string String_Test_Production = string.Empty;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                DBconnectYRS = l_DataBase.CreateConnection();
                DBconnectYRS.Open();

                if (l_DataBase == null) throw new Exception("Unable to initialize database object");
                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_AtsMetaConfiguration_by_ConfigCategoryCode");
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
                if (l_DBCommandWrapper == null) throw new Exception("Unable to initialize command wrapper object");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@ConfigCategoryCode", DbType.String, "EDIISA");
                ds = new DataSet("EDI Configuration Values");
                l_DataBase.LoadDataSet(l_DBCommandWrapper, ds, "EDI CONFIGURATION VALUES");
                //return ds;
                DataRow[] dr = ds.Tables[0].Select("[Key]=" + "'EDI_ISA_15'");
                if (dr.Length == 0 || dr[0].IsNull("Value"))
                {
                    throw new Exception("The value for the key EDI_ISA_15 needs to be defined in the Configuration table");
                }
                switch (dr[0]["Value"].ToString().ToUpper())
                {
                    case "TEST":
                        String_Test_Production = "TEST";
                        break;
                    case "PRODUCTION":
                        String_Test_Production = "PRODUCTION";
                        break;
                    default:
                        throw new Exception("The value for the key EDI_ISA_15 is not a valid value.");

                }
                return String_Test_Production; 
            }
            catch
            {
                throw;

            }
            finally
            {
                if (DBconnectYRS != null) DBconnectYRS.Close();
            }
        }

	}
}
