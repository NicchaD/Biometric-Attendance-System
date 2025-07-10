//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
//using System.Data.SqlClient;
//using System.Globalization;
//using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for DelinquencyLettersDAClass.
	/// </summary>
	public class DelinquencyLettersDAClass
	{
		#region "*********Public Procedure***********"
		
		public static DataSet GetLetterType()
		{
			DataSet dsLetterType = null;
			Database db = null;
			
			DbCommand CommandGetLetterType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				CommandGetLetterType = db.GetStoredProcCommand("yrs_usp_Dl_GetLetterType");
				if (CommandGetLetterType ==null) return null;
				dsLetterType = new DataSet();
				db.LoadDataSet(CommandGetLetterType,dsLetterType,"LetterType");
				return dsLetterType;
			}
			catch
			{
				throw;
			}

		}

		public static DataSet GetLetter()
		{
			DataSet l_DataSet_LetterType=null;
			Database db=null;			
			DbCommand GetCommandWrapper=null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");				
				if(db==null) return null;
				GetCommandWrapper=db.GetStoredProcCommand("yrs_usp_Dl_GetLetterType");
				GetCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				
				if (GetCommandWrapper == null) return null;
				l_DataSet_LetterType = new DataSet();
				db.LoadDataSet(GetCommandWrapper, l_DataSet_LetterType,"Letter Type");
							

				return l_DataSet_LetterType;
			}
			catch
			{
				throw;
			}
		}



		
		public static DataSet GetDelinquentYmcas()
		{
			DataSet l_DataSet_DelinquentYmcas =null;
			Database db=null;
			DbCommand GetCommandWrapper = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;

				GetCommandWrapper=db.GetStoredProcCommand("yrs_usp_Dl_GetDelinquentYmcas");
				GetCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				if(GetCommandWrapper==null) return null;
				l_DataSet_DelinquentYmcas=new DataSet();
				db.LoadDataSet(GetCommandWrapper,l_DataSet_DelinquentYmcas,"YMCA");
				return l_DataSet_DelinquentYmcas;

			}
			catch
			{
				throw;
			}
		}
		
		public static DataSet GetYMCAList()
		{
			DataSet dsSearchYMCAList = null;
			Database db = null;
			DbCommand CommandSearchYMCAList = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				CommandSearchYMCAList = db.GetStoredProcCommand("yrs_usp_Dl_GetYMCAList");
				if (CommandSearchYMCAList ==null) return null;
				dsSearchYMCAList = new DataSet();
				db.LoadDataSet(CommandSearchYMCAList,dsSearchYMCAList,"YMCA List");
				return dsSearchYMCAList;
			}
			catch
			{
				throw;
			}

		}

		//	By Aparna YREN-3197 18/04/2007

//		public static DataSet GetConfigurationCategoryWise(string parameterCategory)
//		{
//			DataSet dsGetConfigDetails =null;
//			Database db= null;
//			DBCommandWrapper GetConfigDetails =null;
//
//			try
//			{
//				db= DatabaseFactory.CreateDatabase("YRS");
//				if(db==null) return null;
//				GetConfigDetails =db.GetStoredProcCommandWrapper("yrs_usp_GetConfigurationCategoryWise");
//				GetConfigDetails.AddInParameter("@chvConfigCategoryCode",DbType.String,parameterCategory);
//				if (GetConfigDetails==null) return null;
//				dsGetConfigDetails=new DataSet();
//				db.LoadDataSet(GetConfigDetails,dsGetConfigDetails,"Configuration Details");
//
//
//			}
//			catch
//			{
//				throw;
//			}
//		}


		public static DataSet GetContactAndOfficerList(string parameterYmcaNos)
		{
			DataSet dsList = null;
			Database db = null;
			

			DbCommand CommandGetList = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				 CommandGetList = db.GetStoredProcCommand("yrs_usp_Dl_GetContactAndOfficerList");
				if (CommandGetList ==null) return null;
					dsList = new DataSet();
				 db.AddInParameter(CommandGetList,"@varchar_YmcaNos",DbType.String,parameterYmcaNos);
				db.LoadDataSet(CommandGetList,dsList,"List");

				return dsList;
			}
			catch
			{
				throw;
			}

		}

		//aparna
		public static DataSet Get15thBusinessDay()
		{
			DataSet dsDate = null;
			Database db=null;

			DbCommand CommandGetDate = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;

				CommandGetDate=db.GetStoredProcCommand("yrs_usp_Dl_Get15thBusinessDay");
				if (CommandGetDate==null)return null;
				dsDate = new DataSet();
				db.LoadDataSet(CommandGetDate,dsDate,"Date");
				return dsDate;		
				
			}
			catch
			{
				throw;
			}

		}

		public static DataSet GetList(string parameterYmcaNos,int parameterSno)
		{
			DataSet dsList = null;
			Database db = null;
			

			DbCommand CommandGetList = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandGetList = db.GetStoredProcCommand("yrs_usp_Dl_GetDetailInfo");
				CommandGetList.CommandTimeout=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
				if (CommandGetList ==null) return null;
				dsList = new DataSet();
				db.AddInParameter(CommandGetList,"@varchar_YmcaNo",DbType.String,parameterYmcaNos);
                db.AddInParameter(CommandGetList,"@int_Number",DbType.Int32,parameterSno);
					
				db.LoadDataSet(CommandGetList,dsList,"List");

				return dsList;
			}
			catch
			{
				throw;
			}

		}

		public static DataSet GetYmcasPayRollDates(string parameterYmcaNo)
		{
			DataSet dsList = null;
			Database db = null;
			

			DbCommand CommandGetList = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				CommandGetList = db.GetStoredProcCommand ("yrs_usp_Dl_GetYmcasPayRollDates");				
				if (CommandGetList ==null) return null;
				dsList = new DataSet();
				CommandGetList.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.AddInParameter(CommandGetList,"@varchar_YmcaNo",DbType.String,parameterYmcaNo);			
				db.LoadDataSet(CommandGetList,dsList,"List");
				return dsList;
			}
			catch
			{
				throw;
			}

		}

		public static DataSet GetPayRollDatesFor9thBusDay(string parameterYmcaNo)
		{
			
			DataSet dsList = null;
			Database db = null;		
			DbCommand CommandGetList = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				CommandGetList = db.GetStoredProcCommand("yrs_usp_Dl_GetPayRollDatesFor9thBusDay");				
				if (CommandGetList ==null) return null;
				dsList = new DataSet();
				CommandGetList.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				db.AddInParameter(CommandGetList,"@varchar_YmcaNo",DbType.String,parameterYmcaNo);			
				db.LoadDataSet(CommandGetList,dsList,"List");
				return dsList;
			}
			catch
			{
				throw;
			}

		}
		
		public static DataSet GetDelinquentYmcasFor9thBusDay()
		{
			DataSet l_DataSet_DelinquentYmcas =null;
			Database db=null;
			DbCommand GetCommandWrapper = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;

				GetCommandWrapper=db.GetStoredProcCommand("yrs_usp_Dl_GetDelinquentYmcasFor9thBusDay");
				GetCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 

				if(GetCommandWrapper==null) return null;
				l_DataSet_DelinquentYmcas=new DataSet();
				db.LoadDataSet(GetCommandWrapper,l_DataSet_DelinquentYmcas,"YMCA");
				return l_DataSet_DelinquentYmcas;
			}
			catch
			{
				throw;
			}
		}

		//Aparna -27/09/2006
		public static DataSet getMetaOutputFileType()
		{
			DataSet dsMetaOutputFileType = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");			
				if (db == null) return null;
				getCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaOutputFileType");				
				if (getCommandWrapper == null) return null;					
				db.AddInParameter(getCommandWrapper,"@char_MetaOutputFileType",DbType.String,"DLTTR");
				dsMetaOutputFileType = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsMetaOutputFileType,"MetaOutputFileType");				
				return dsMetaOutputFileType;
			}
			catch 
			{
				throw;
			}
		}

		//aparna -14/12/2006
		//yrs_usp_Dl_SchemaAtsDelinquencyCRData
		public static DataSet GetSchemaAtsDelinquencyCRData()
		{
			Database l_DataBase =null;
			DbCommand SelectCommandWrapper =null;
			DataSet l_dataset = null;
			string l_TableName;
			try
			{
				l_DataBase= DatabaseFactory.CreateDatabase("YRS");
				SelectCommandWrapper=l_DataBase.GetStoredProcCommand("yrs_usp_Dl_SchemaAtsDelinquencyCRData");
				SelectCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if(SelectCommandWrapper==null)return null;									
					l_dataset = new DataSet ("AtsDelinquencyCRDataTable");
					l_TableName = "AtsDelinquencyCRData";
					l_DataBase.LoadDataSet (SelectCommandWrapper, l_dataset, l_TableName);
					return l_dataset; 
				
			}
			catch
			{
					throw;
			}
		}
		public static void DeleteReportData(string parameterUserId,string parameterIPAddress)
		{
			Database db = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");				
				deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_Dl_DeleteReportData");
				deleteCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if(deleteCommandWrapper!=null)
				{
                    db.AddInParameter(deleteCommandWrapper,"@varchar_UserId", DbType.String, parameterUserId);
                    db.AddInParameter(deleteCommandWrapper,"@varchar_IpAddress", DbType.String, parameterIPAddress);
					db.ExecuteNonQuery(deleteCommandWrapper);
				}				
			}
			catch
			{
				throw;
			}

		}

		public static void InsertReportData(DataSet parameterReportData)
		{   
			Database parameterDatabase = null;

			DbCommand insertCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

			DbTransaction l_IDbTransaction = null;
			DbConnection l_IDbConnection = null;
			try
			{
				parameterDatabase  = DatabaseFactory.CreateDatabase("YRS");

				if (parameterDatabase == null) return ;
                l_IDbConnection = parameterDatabase.CreateConnection();//.GetConnection();
				l_IDbConnection.Open();
				if (l_IDbConnection == null) return;
				l_IDbTransaction = l_IDbConnection.BeginTransaction();


				insertCommandWrapper=parameterDatabase.GetStoredProcCommand("yrs_usp_Dl_InsertReportData");
				insertCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); 
				if(insertCommandWrapper!=null)
				{
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_YmcaNo", DbType.String, "chrYmcaNO", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_YmcaName", DbType.String, "chvYMCAName", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_ActiveResolution", DbType.String, "chvActiveResolution", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@int_NoOfPayrolls", DbType.Int32, "intNoofPayrolls", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@dtm_EarliestPayrollDate", DbType.String, "dtmEarliestPayRollDate", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_AdditionalAccounts", DbType.String, "chvAdditionalAccounts", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_UserId", DbType.String, "chvUserId", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_IpAddress", DbType.String, "chvIpAddress", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_Selected", DbType.String, "chvSelected", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@int_NoOfEmployees", DbType.Int32, "intNoOfEmployees", DataRowVersion.Current);
                    parameterDatabase.AddInParameter(insertCommandWrapper,"@varchar_MissingContType", DbType.String, "chvMissingContType", DataRowVersion.Current);
					
				
					parameterDatabase.UpdateDataSet(parameterReportData,"AtsDelinquencyCRData",insertCommandWrapper,UpdateCommandWrapper,deleteCommandWrapper,l_IDbTransaction);					
					l_IDbTransaction.Commit();					
										
				}


												
				
			}
			catch
			{
				l_IDbTransaction.Rollback();
				throw;
			}
			finally
			{
				if (l_IDbConnection != null)
				{
					if (l_IDbConnection.State != ConnectionState.Closed)
					{
						l_IDbConnection.Close ();
					}
				}
			}
		}




		#endregion "**************End Public Methods***************************"
	}
}
