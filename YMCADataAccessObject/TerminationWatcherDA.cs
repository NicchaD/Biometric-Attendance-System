//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	TerminationWatcherDA.cs
// Author Name		:	Priya Patil
// Employee ID		:	37786
// Email			:	priya.jawale@3i-infotech.com
// Contact No		:	8416
// Creation Time	:	8/25/2012 6:36:14 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*****************************************************************************************************************************
//Chnaged by				Date					Description
//*****************************************************************************************************************************
//Anudeep                   07.11.2012             Adding Processid While processing records and Returning Processid for Report Genaration
//Anudeep                   20-11-2012             Stored Procdure Name Changed Because all Termination watcher Procs has been start with TW
//Anudeep                   17.01.2013             To correct the Proc name "yrs_usp_TW_GetPersonPalntype" to "yrs_usp_TW_GetPersonPlantype"
//Anudeep                   2014.03.25             BT-957/YRS 5.0-1484:New Utility to replace Refund Watcher program
//Manthan Rajguru           2015.09.16             YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************************************************************************************
using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AddBeneficiaryBOClass.
	/// </summary>
	public class TerminationWatcherDA
	{

		       #region Constructor
        public TerminationWatcherDA()
        { }  
        #endregion
		public static DataSet SearchPerson(string strFundNo, string strFirstName, string strLastName, string strSSNo)
        {
			DataSet dsLookUpPersons = null;
			Database db = null;
			DbCommand commandLookUpPersons = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_TW_LookUp_MemberListForTerminationWatcher");
				//commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_TW_SearchPerson");

				if (commandLookUpPersons == null) return null;

				commandLookUpPersons.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
				
				dsLookUpPersons = new DataSet();

				db.AddInParameter(commandLookUpPersons, "@varchar_FundNo", DbType.String, strFundNo);
				db.AddInParameter(commandLookUpPersons, "@varchar_FName", DbType.String, strFirstName);
				db.AddInParameter(commandLookUpPersons, "@varchar_LName", DbType.String, strLastName);
				db.AddInParameter(commandLookUpPersons, "@varchar_SSN", DbType.String, strSSNo);
								
				db.LoadDataSet(commandLookUpPersons, dsLookUpPersons, "Persons");
				return dsLookUpPersons;
			}
			catch
			{
				throw;
			}

        }

		public static string SaveTerminationWatcherData(string strguiPersId, string strguiFundEventId, string strchrType, string strchvPlanType, string strchvSource)
		{
			string strRetuenValue = string.Empty;


			Database db = null;
			DbCommand insertCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_InsertTerminationWatcher");


				db.AddInParameter(insertCommandWrapper, "@guiPersId", DbType.String, strguiPersId);
				db.AddInParameter(insertCommandWrapper, "@guiFundEventId", DbType.String, strguiFundEventId);
				db.AddInParameter(insertCommandWrapper, "@chrType", DbType.String, strchrType);
				db.AddInParameter(insertCommandWrapper, "@chvPlanType", DbType.String, strchvPlanType);
				db.AddInParameter(insertCommandWrapper, "@chvSource", DbType.String, strchvSource);
				db.AddOutParameter(insertCommandWrapper, "@outReturnValue", DbType.Int32, 10);

								
				 db.ExecuteNonQuery(insertCommandWrapper);

				 strRetuenValue = Convert.ToString(db.GetParameterValue(insertCommandWrapper, "@outReturnValue"));

				return strRetuenValue;

			}
			catch
			{
				throw;
			}
			
		}

		public static DataSet ListPerson(string strType)
		{
			DataSet dsListPersons = null;
			Database db = null;
			DbCommand commandListPersons = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandListPersons = db.GetStoredProcCommand("yrs_usp_TW_ListTerminationWatcherPersons");

				if (commandListPersons == null) return null;

				commandListPersons.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

				dsListPersons = new DataSet();

				db.AddInParameter(commandListPersons, "@varchar_Type", DbType.String, strType);


				db.LoadDataSet(commandListPersons, dsListPersons, "Persons");
				return dsListPersons;
			}
			catch
			{
				throw;
			}

		}

		public static string UpdateTerminationWatcherData(string strUniqueId, string strPlanType)
		{
			string strRetuenValue = string.Empty;


			Database db = null;
			DbCommand updateCommandWrapper = null;





			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_UpdatePlanTypeTerminationWatcher");


				db.AddInParameter(updateCommandWrapper, "@intUniqueId", DbType.String, strUniqueId);
				db.AddInParameter(updateCommandWrapper, "@chvPlanType", DbType.String, strPlanType);
				db.AddOutParameter(updateCommandWrapper, "@outReturnValue", DbType.Int32, 10);


				db.ExecuteNonQuery(updateCommandWrapper);

				strRetuenValue = Convert.ToString(db.GetParameterValue(updateCommandWrapper, "@outReturnValue"));

				return strRetuenValue;

			}
			catch
			{
				throw;
			}

		}

		public static string DeleteTerminationWatcherData(string strUniqueId)
		{
			string strRetuenValue = string.Empty;


			Database db = null;
			DbCommand updateCommandWrapper = null;
			DbTransaction tran = null;
			DbConnection cn = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return "-1";
				//Get a connection and Open it
				cn = db.CreateConnection();
				cn.Open();

				//Get a Transaction from the Database
				tran = cn.BeginTransaction(IsolationLevel.Serializable);
				if (tran == null) return "-1";
				

				//updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_DeleteTerminationWatcher");
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_DeleteTerminationWatcherPersons");


				//db.AddInParameter(updateCommandWrapper, "@intUniqueId", DbType.String, strUniqueId);
				db.AddInParameter(updateCommandWrapper, "@TerminationWatcherId", DbType.String, strUniqueId);
				
				db.AddOutParameter(updateCommandWrapper, "@outReturnValue", DbType.Int32, 10);


				db.ExecuteNonQuery(updateCommandWrapper);

				strRetuenValue = Convert.ToString(db.GetParameterValue(updateCommandWrapper, "@outReturnValue"));

				if (strRetuenValue != "1")
				{
					tran.Rollback();
				}
				else
				{
					tran.Commit();
				}
				return strRetuenValue;

				
			}
			catch (Exception ex)
			{
				if (tran != null) tran.Rollback();
				if (cn != null) cn.Close();
				throw ex;
			}

		}

		public static DataSet ListNotes(string strTerminationWatcherID)
		{
			DataSet dsListNotes = null;
			Database db = null;
			DbCommand commandListNotes = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandListNotes = db.GetStoredProcCommand("yrs_usp_TW_ListTerminationNotes");

				if (commandListNotes == null) return null;

				commandListNotes.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

				dsListNotes = new DataSet();

				db.AddInParameter(commandListNotes, "@TerminationWatcherID", DbType.String, strTerminationWatcherID);


				db.LoadDataSet(commandListNotes, dsListNotes, "Notes");
				return dsListNotes;
			}
			catch
			{
				throw;
			}

		}


		public static string SaveTerminationWatcherNotes( string strTerminationWatcherId,string strguiPersId, string strNotes, Boolean boolImportant)
		{
			string strRetuenValue = string.Empty;


			Database db = null;
			DbCommand insertCommandWrapper = null;





			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_InsertTerminationNotes");


				db.AddInParameter(insertCommandWrapper, "@TerminationWatcherID", DbType.String, strTerminationWatcherId);
				db.AddInParameter(insertCommandWrapper, "@Varchar_Notes", DbType.String, strNotes);
				db.AddInParameter(insertCommandWrapper, "@guiPersID", DbType.String, strguiPersId);
				db.AddInParameter(insertCommandWrapper, "@bitImportant", DbType.Boolean, boolImportant);
				
				db.AddOutParameter(insertCommandWrapper, "@outReturnValue", DbType.Int32, 10);


				db.ExecuteNonQuery(insertCommandWrapper);

				strRetuenValue = Convert.ToString(db.GetParameterValue(insertCommandWrapper, "@outReturnValue"));

				return strRetuenValue;

			}
			catch
			{
				throw;
			}

		}


		public static DataSet ListPurgeRecord(string strType)
		{
			DataSet dsListNotes = null;
			Database db = null;
			DbCommand commandListNotes = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandListNotes = db.GetStoredProcCommand("yrs_usp_TW_ListEligiblePurgePersons");

				if (commandListNotes == null) return null;

				commandListNotes.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

				dsListNotes = new DataSet();

				db.AddInParameter(commandListNotes, "@varchar_Type", DbType.String, strType);


				db.LoadDataSet(commandListNotes, dsListNotes, "PurgeList");
				return dsListNotes;
			}
			catch
			{
				throw;
			}

		}
		 
		

		public static string PurgeTerminationWatcherData( List<string> LTerminationIds)
		{
			string strRetuenValue = string.Empty;
			string strUniqueId  = string.Empty;
			Database db = null;
			DbCommand updateCommandWrapper = null;
			DbTransaction tran = null;
			DbConnection cn = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return "-1";

				//Get a connection and Open it
				cn = db.CreateConnection();
				cn.Open();

				//Get a Transaction from the Database
				tran = cn.BeginTransaction(IsolationLevel.Serializable);
				if (tran == null) return "-1";

				foreach (string item in LTerminationIds)
				{
					strUniqueId = item;




					updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_DeleteTerminationWatcherPersons");


					db.AddInParameter(updateCommandWrapper, "@TerminationWatcherId", DbType.String, strUniqueId);

					db.AddOutParameter(updateCommandWrapper, "@outReturnValue", DbType.Int32, 10);


					db.ExecuteNonQuery(updateCommandWrapper);

					strRetuenValue = Convert.ToString(db.GetParameterValue(updateCommandWrapper, "@outReturnValue"));

					if (strRetuenValue != "1")
					{
						tran.Rollback();
						return strRetuenValue;


					}

				}

				tran.Commit();
				return strRetuenValue;
			}

			catch (Exception ex)
			{
				if (tran != null) tran.Rollback();
				if (cn != null) cn.Close();
				throw ex;
			}

		}

		
        
		public static DataSet ListProcessedRecord(string strType)
		{
			DataSet dsListProcessedRecords = null;
			Database db = null;
			DbCommand commandListProcessedRecords = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandListProcessedRecords = db.GetStoredProcCommand("yrs_usp_TW_ListProcessedPersons");

				if (commandListProcessedRecords == null) return null;

				commandListProcessedRecords.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

				dsListProcessedRecords = new DataSet();

				db.AddInParameter(commandListProcessedRecords, "@varchar_Type", DbType.String, strType);


				db.LoadDataSet(commandListProcessedRecords, dsListProcessedRecords, "PurgeList");
				return dsListProcessedRecords;
			}
			catch
			{
				throw;
			}

		}


		public static string[] ProcessTerminationWatcherData(List<string> LTerminationIds)
		{
			string strRetuenValue = string.Empty;
			string strUniqueId = string.Empty;
            string strProcessid; //'Anudeep:06-nov-2012 Changes made For Process Report
            String[] Strings = new String[3];
			Database db = null;
			DbCommand updateCommandWrapper = null;
			DbTransaction tran = null;
			DbConnection cn = null;
    try
				{
            
			db = DatabaseFactory.CreateDatabase("YRS");

            if (db == null) 
            {
                Strings[0] = "-1";
                Strings[1] = null;//'Anudeep:06-nov-2012 Changes made For Process Report
                return Strings; 
            }

			//Get a connection and Open it
			cn = db.CreateConnection();
			cn.Open();

			//Get a Transaction from the Database
			tran = cn.BeginTransaction(IsolationLevel.Serializable);
            if (tran == null) { 
                Strings[0] = "-1";
                Strings[1] = null;//'Anudeep:06-nov-2012 Changes made For Process Report
                return Strings; 
            }
            strProcessid = GetProcessId();//'Anudeep:06-nov-2012 Changes made For Process Report
            
			foreach (string item in LTerminationIds)
			{
				strUniqueId = item;

				

					updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_ProcessTerminationWatcherPersons");


					db.AddInParameter(updateCommandWrapper, "@TerminationWatcherId", DbType.String, strUniqueId);

                    db.AddInParameter(updateCommandWrapper, "@chvProcessId", DbType.String, strProcessid);//'Anudeep:06-nov-2012 Changes made For Process Report

					db.AddOutParameter(updateCommandWrapper, "@outReturnValue", DbType.Int32, 10);


					db.ExecuteNonQuery(updateCommandWrapper);

					strRetuenValue = Convert.ToString(db.GetParameterValue(updateCommandWrapper, "@outReturnValue"));

					if (strRetuenValue != "1")
					{
						tran.Rollback();
                        Strings[0] = strRetuenValue;
						return Strings;
					}
					
					
				

			}
			tran.Commit();
            Strings[0] = strRetuenValue; //Returning the output return value for confirmation
            Strings[1] = strProcessid;  //Returning Batchid For Report generation
            return Strings;//'Anudeep:06-nov-2012 Changes made For Process Report
			}

				catch (Exception ex)
				{
					//Error encountered, rollback the transaction
					if (tran != null) tran.Rollback();
					if (cn != null) cn.Close();
					throw ex;
					//return -99;
				} 
		}


		public static DataSet ListPlanType()
		{
			DataSet dsListPlanType = null;
			Database db = null;
			DbCommand commandListPlanType = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandListPlanType = db.GetStoredProcCommand("yrs_usp_TW_GetPlanType");

				if (commandListPlanType == null) return null;

				commandListPlanType.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

				dsListPlanType = new DataSet();

			
				db.LoadDataSet(commandListPlanType, dsListPlanType, "PlanTypeList");
				return dsListPlanType;
			}
			catch
			{
				throw;
			}

		}


		public static DataSet ListInvalidRecord(string strType)
		{
			DataSet dsListInvalidRecords = null;
			Database db = null;
			DbCommand commandListInvalidRecords = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				commandListInvalidRecords = db.GetStoredProcCommand("yrs_usp_TW_ListInvalidPersons");

				if (commandListInvalidRecords == null) return null;

				commandListInvalidRecords.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

				dsListInvalidRecords = new DataSet();

				db.AddInParameter(commandListInvalidRecords, "@varchar_Type", DbType.String, strType);


				db.LoadDataSet(commandListInvalidRecords, dsListInvalidRecords, "PurgeList");
				return dsListInvalidRecords;
			}
			catch
			{
				throw;
			}

		}

		public static DataSet GetApplicantPlanType(string strFundEventId)
		{
			DataSet dsApplicantPlanType = null;
			Database db = null;
			DbCommand commandListApplicantPlanType = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;
                //Changed By Anudeep:17.01.2013 to correct the Proc name "yrs_usp_TW_GetPersonPalntype" to "yrs_usp_TW_GetPersonPlantype"
                commandListApplicantPlanType = db.GetStoredProcCommand("yrs_usp_TW_GetPersonPlantype");

				if (commandListApplicantPlanType == null) return null;

				commandListApplicantPlanType.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

				dsApplicantPlanType = new DataSet();

				db.AddInParameter(commandListApplicantPlanType, "@varchar_FundEventID", DbType.String,strFundEventId);


				db.LoadDataSet(commandListApplicantPlanType, dsApplicantPlanType, "ApplicantPlantType");
				return dsApplicantPlanType;
			}
			catch
			{
				throw;
			}

		}



        //2012.08.07 SR : BT-957/YRS 5.0-1484 : Termination Watcher -Start
        /// <summary>
        /// This methods update the processing date in yrs_AtsTerminationWatcher 
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>Boolean</returns>
        //AA:2014.03.25 - BT-957/YRS 5.0-1484:Added optional parameter of refrequest id for getting unfunded transactions balance as per refund requested accounts
        public static void UpdateTerminationWatcher(string PersId, string FundEventId, string Type, string PlanType, DbTransaction pDBTransaction, Database db,string strRefrequestId = null)
        {

            DbCommand updateCommandWrapper = null;

            try
            {
                //Anudeep:20-11-2012  Stored Procdure Name Changed Because all Termination watcher Procs has been start with TW
                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_TW_UpdateTerminationWatcher");
                // Defining The Update Command Wrapper With parameters

                db.AddInParameter(updateCommandWrapper, "@guiPersId", DbType.String, PersId);
                db.AddInParameter(updateCommandWrapper, "@guiFundEventId", DbType.String, FundEventId);
                db.AddInParameter(updateCommandWrapper, "@Type", DbType.String, Type);
                db.AddInParameter(updateCommandWrapper, "@PlanType", DbType.String, PlanType);
                db.AddInParameter(updateCommandWrapper, "@guiRefRequestId", DbType.String, strRefrequestId);
                updateCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.ExecuteNonQuery(updateCommandWrapper, pDBTransaction);

            }
            catch
            {
                throw;
            }
        }

        //2012.08.07 SR : BT-957/YRS 5.0-1484 : Termination Watcher -End

        //'Start 'Anudeep:06-nov-2012 Changes made For Process Report 

        public static string GetProcessId()
        {
            DbCommand commandGetProcessId = null;
            Database db = null;
            string strProcessid =string.Empty ;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandGetProcessId = db.GetStoredProcCommand("yrs_usp_TW_GetProcessId");
                if (commandGetProcessId == null) return null;
                db.AddOutParameter(commandGetProcessId, "@chvProcessId", DbType.Int32, 32);
                db.ExecuteNonQuery(commandGetProcessId);
                strProcessid = Convert.ToString(db.GetParameterValue(commandGetProcessId, "@chvProcessId"));
                return strProcessid;
            }
            catch
            {
                throw;
            }
            finally
            {
                commandGetProcessId = null;
                db = null;
                strProcessid = string.Empty;
            }
        }


        public  DataSet GetProcessReportName(string key)
        {
            DbCommand commandGetProcessReport = null;
            Database db = null;
            DataSet  dsProcessReport = null ;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandGetProcessReport = db.GetStoredProcCommand("dbo.yrs_usp_AtsMetaConfiguration_Get");
                commandGetProcessReport.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
                if (commandGetProcessReport == null) { return null; }

                dsProcessReport = new DataSet();

                db.AddInParameter(commandGetProcessReport, "@key", DbType.String, key);

                db.LoadDataSet(commandGetProcessReport, dsProcessReport, "TWProcessReport");

                return dsProcessReport;
            }
            catch
            {
                throw;
            }
            finally
            {
                commandGetProcessReport = null;
                db = null;
                dsProcessReport = null;
            }
        }
        //END'Anudeep:06-nov-2012 Changes made For Process Report
















	}
}
