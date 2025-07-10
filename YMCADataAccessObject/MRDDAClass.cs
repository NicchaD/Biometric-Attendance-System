//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	MRDDAClass.cs
// Author Name		:	
// Employee ID		:	
// Email			:	
// Contact No		:	
// Description		:	Data access class for MRD form
//*******************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified By			Date				Description
//********************************************************************************************************************************
//Sanjay R.             2014.01.29          MRD Enhancement
//Anudeep A.            2014.05.07          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages
//Dinesh k              2014.06.10          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages
//Sanjay R              2014.07.18          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages
//Anudeep A             2015.01.29          BT:2139:YRS 5.0-2165 - RMD enhancements   
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Anudeep Adusumilli    2015.10.21          YRS-AT-2614 - YRS: files for IDM - .idx filename needs to match .pdf filename
//Sanjay GS Rawat       2016.04.05          YRS-AT-2843 - YRS enh-RMD Utility should use J&S table if primary bene is spouse more than 10 yrs younger (TrackIT 25233)
//Chandra sekar         2016.10.17          YRS-AT-2476 - Change to RMD utility - generate one release blank instead of two if same participant (TrackIT 22029)
//Manthan Rajguru       2016.10.27          YRS-AT-2922 -  YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)
//Chandra sekar         2016.11.01          YRS-AT-2922 - YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)
//Santosh Bura			2016.10.24 			YRS-AT-2685 -  YRS enh-save RMD Print Letters batches and add wording near "close" button (TrackIT 24380)   
//Santosh Bura			2016.12.05 			YRS-AT-3203 -  YRS enh: RMD Utility distinguish cashout candidates PHASE 2 OF 2 (TrackIT 26224)
//Manthan Rajguru       2017.04.27          YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977)    
//Santosh Bura			2017.04.11 			YRS-AT-3400 -  YRS enh: due MAY 2017 - RMD Print Letters- Letter to Non-respondents (new screen). (2 of 3 tickets) (TrackIT 29186)
//Santosh Bura			2017.04.25 			YRS-AT-3401 -  RMD Print Letters- Satisfied but not elected (new screen)
//Santosh Bura          2017.07.28          YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
//Santosh Bura          2018.01.11          YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
//Pramod P. Pokale      2018.10.03          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
//********************************************************************************************************************************
using System;
using System.IO;
using System.Data;
using System.Security.Permissions;
using System.Collections;
using System.Globalization;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
    public class MRDDAClass
    {
        //AA:05.07.2014 BT:2434:YRS 5.0-2315 - Added parameter to get the records as per the year
        public static DataSet GetMRDRecords(int intRMDYear, int month, string fundNo) //MMR | 2016.10.27 | YRS-AT-2922 | Passed parameters to get records by RMD due date and fund no alongwith RMD Year
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_GetMRDRecords");
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@intRMDYear", DbType.Int32, intRMDYear);
                //START: MMR | 2016.10.27 | YRS-AT-2922 | Added parameters to get records by RMD due date and fund no
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@INT_Month", DbType.Int32, month);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@VARCHAR_FundNo", DbType.String, fundNo);
                //END: MMR | 2016.10.27 | YRS-AT-2922 | Added parameters to get records by RMD due date and fund no
                if (l_DBCommandWrapper == null) return null;
                l_DataSet = new DataSet("GetMRDRecords");
                l_TableNames = new string[] { "MRD" };
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                return l_DataSet;
            }
            catch
            {
                throw;
            }

        }

        public static DataSet GetBatchMRDRecords()
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RMDBatch_GetAtsMrdRecords");
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DBCommandWrapper == null) return null;
                l_DataSet = new DataSet("GetBatchMRDRecords");
                l_TableNames = new string[] { "ProcessDate", "BatchRecords" };
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                return l_DataSet;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetRMDRecordsForBatchProcess()
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RMDBatch_GetRMDRecordsForBatchProcess");
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DBCommandWrapper == null) return null;
                l_DataSet = new DataSet("GetBatchMRDRecords");
                l_TableNames = new string[] { "ProcessDate", "BatchRecords", "BatchRecordsDisplay" }; // CS | 2016.10.17 |  YRS-AT-2476 | BatchRecordsDisplay holds Combined MRD Records (Retirement and Savings) as Both Plan"
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                return l_DataSet;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetBatchNonEligibleMRDRecords(string[] strParam = null)
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RMDBatch_GetNonEligibleMrdRecords");

                if (strParam != null)
                {
                    if (strParam.Length > 0)
                    {
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@IsLocked", DbType.String, strParam[0].ToLower().Trim());
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@IsInsufficient", DbType.String, strParam[1].ToLower().Trim());
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@IsPriorRMD", DbType.String, strParam[2].ToLower().Trim());
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@IsNotEnrolled", DbType.String, strParam[3].ToLower().Trim());
                        l_DataBase.AddInParameter(l_DBCommandWrapper, "@IsFilter", DbType.String, strParam[4].ToLower().Trim());
                    }
                }

                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DBCommandWrapper == null) return null;
                l_DataSet = new DataSet("GetNonEligibleRMDRecords");
                l_TableNames = new string[] { "ProcessDate", "BatchRecords" };
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                return l_DataSet;
            }
            catch
            {
                throw;
            }
        }

        public static int SaveCurrentMRD(DateTime Processdate)
        {
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            DbCommand CommandUpdate = null;
            int success;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();

                CommandUpdate = db.GetStoredProcCommand("yrs_usp_MRD_InsertMRDProcess");
                db.AddInParameter(CommandUpdate, "@date_Began", DbType.DateTime, Processdate);
                CommandUpdate.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                success = db.ExecuteNonQuery(CommandUpdate, DBTransaction);
                DBTransaction.Commit();
                DBconnectYRS.Close();
                return success;
            }
            catch
            {
                throw;
            }
        }

        public static string GenerateMRDRecords(DateTime dtGenerateMRD)
        {
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            DbCommand CommandUpdate = null;
            string strmessage;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();
                CommandUpdate = db.GetStoredProcCommand("yrs_usp_Create_MRD_Records");
                db.AddInParameter(CommandUpdate, "@ProcessDate", DbType.DateTime, dtGenerateMRD);
                db.AddOutParameter(CommandUpdate, "@strMessage", DbType.String, 1000);
                CommandUpdate.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.ExecuteNonQuery(CommandUpdate, DBTransaction);
                DBTransaction.Commit();
                DBconnectYRS.Close();
                strmessage = db.GetParameterValue(CommandUpdate, "@strMessage").ToString();
                return strmessage;
            }
            catch
            {
                throw;
            }
        }

        #region Added By Sanjeev Gupta 14th Oct 2011 BT-925 Regenerate RMD
        public static DataSet GetGeneratedMRDRecords(string parameterFundID)
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_FetchGeneratedRMDDetails");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@varchar_FundID", DbType.String, parameterFundID);
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DBCommandWrapper == null) return null;
                l_DataSet = new DataSet("GetGeneratedMRD");
                l_TableNames = new string[] { "GeneratedMRD" };
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                return l_DataSet;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetRegeneratedRMDRecords(int iProcessYear, string stFundID, out string stMessage)
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                stMessage = "";
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null)
                    return null;

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RegenerateRMDRecords");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@YEAR", DbType.Int32, iProcessYear);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@uiFundEventID", DbType.String, stFundID);
                l_DataBase.AddOutParameter(l_DBCommandWrapper, "@strMessage", DbType.String, 1000);
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                if (l_DBCommandWrapper == null)
                    return null;

                l_DataSet = new DataSet("GetRegenerateMRD");
                l_TableNames = new string[] { "RegenerateMRD" };
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                stMessage = l_DataBase.GetParameterValue(l_DBCommandWrapper, "@strMessage").ToString();

                return l_DataSet;
            }
            catch
            {
                throw;
            }
            finally
            {
                l_DataBase = null;
                l_DBCommandWrapper = null;
                l_TableNames = null;
            }
        }

        public static int SaveRegeneratedMRD(string stXMLRegenMRD, string stFundID, out string stMessage)
        {
            Database db = null;
            DbConnection DBconnectYRS = null;
            DbCommand CommandUpdate = null;
            int success;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                // SR | BT-124 : YRS-AT-2843 | Provide appropriate name to procedure
                //CommandUpdate = db.GetStoredProcCommand("yrs_usp_ConfirmRegeneratedRMDRecords");
                CommandUpdate = db.GetStoredProcCommand("yrs_usp_RMD_SaveRegeneratedRMDRecords");
                // SR | BT-124 : YRS-AT-2843 | Provide appropriate name to procedure
                db.AddInParameter(CommandUpdate, "@xmlRegeneratedMRD", DbType.String, stXMLRegenMRD);
                db.AddInParameter(CommandUpdate, "@stFundEventID", DbType.String, stFundID);
                db.AddOutParameter(CommandUpdate, "@strMessage", DbType.String, 1000);

                CommandUpdate.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                stMessage = db.GetParameterValue(CommandUpdate, "@strMessage").ToString();
                success = db.ExecuteNonQuery(CommandUpdate);

                DBconnectYRS.Close();

                return success;
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
                DBconnectYRS = null;
                CommandUpdate = null;
            }
        }

        public static DataSet GetDisbursementDetails(string stFundEventID, int iYear, string stPlanType)
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null)
                    return null;

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("yrs_usp_FetchDisbursementDetails");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@uiFundEventID", DbType.String, stFundEventID);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@iYEAR", DbType.Int32, iYear);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@stPlanType", DbType.String, stPlanType);
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DBCommandWrapper == null)
                    return null;

                l_DataSet = new DataSet("GetDisbursementDetails");
                l_TableNames = new string[] { "GetDisbursementDetails" };
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                return l_DataSet;
            }
            catch
            {
                throw;
            }
        }
        #endregion



        #region Added By Sanjeev Gupta on 17th Feb 2012 for BT-1000
        public static void GetRegenYearByTermDate(string stFundID, out Int32 iStartYear)
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_GetRegenYearByTermDate");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@uiFundEventID", DbType.String, stFundID);
                l_DataBase.AddOutParameter(l_DBCommandWrapper, "@StartYear", DbType.Int32, 4);

                l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);

                //l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                iStartYear = Convert.ToInt32(l_DataBase.GetParameterValue(l_DBCommandWrapper, "@StartYear"));
            }
            catch
            {
                throw;
            }
            finally
            {
                l_DataBase = null;
                l_DBCommandWrapper = null;
            }
        }
        #endregion



        #region "Added by Dinesh Knaojia on 17th Oct 2013 for gemini 2165"

        /// <summary>
        /// Added by Dinesh
        /// </summary>
        /// <returns></returns>
        public static DataSet GetMRDBatchProcessRecords()
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_rpt_GetProcessMRD");
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DBCommandWrapper == null) return null;
                l_DataSet = new DataSet("GetProcessMRD");
                l_TableNames = "MRDBatchProcessRecords";
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                return l_DataSet;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Added by Dinesh Kanojia
        /// </summary>
        /// <param name="strGuiPerssID"></param>
        /// <param name="strConfigCode"></param>
        /// <returns></returns>
        public static DataSet GetPersonMetaConfigurationDetails(string strGuiPerssID, string strConfigCode)
        {
            try
            {
                Database db = null;
                DbCommand LookUpCommandWrapper = null;
                DataSet dsPerssMetaConfiguration = null;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetPerssMetaConfigurationDetails");
                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@guiPerssID", DbType.String, strGuiPerssID);
                //db.AddInParameter(LookUpCommandWrapper, "@chvConfigCategoryCode", DbType.String, strConfigCode);
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsPerssMetaConfiguration = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsPerssMetaConfiguration, "PerssMetaConfiguration");
                return dsPerssMetaConfiguration;
            }
            catch
            {
                throw;
            }
        }



        public static DataSet GetBatchRMDForInitialLetter()
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            DataSet dsBatchRMDForInitialLetter = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RMDBatch_GetBatchRMDForInitialLetter");
                if (LookUpCommandWrapper == null) return null;
                //Start: AA:02.04.2015 BT:2139 - YRS 5.0-2165:RMD enhancements
                //db.ExecuteNonQuery(LookUpCommandWrapper);
                LookUpCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                //End: AA:02.04.2015 BT:2139 - YRS 5.0-2165:RMD enhancements
                dsBatchRMDForInitialLetter = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsBatchRMDForInitialLetter, "BatchRMDForInitialLetter");
                return dsBatchRMDForInitialLetter;
            }
            catch
            {
                throw;
            }
        }


        public static DataSet GetBatchRMDForPrintReport(string strProcessDate)
        {
            try
            {
                Database db = null;
                DbCommand LookUpCommandWrapper = null;
                DataSet dsBatchRMDForPrintReport = null;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetBatchRMDForPrintReport");
                if (LookUpCommandWrapper == null) return null;
                db.AddInParameter(LookUpCommandWrapper, "@dtmMrdProcessDate", DbType.Date, Convert.ToDateTime(strProcessDate));
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsBatchRMDForPrintReport = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsBatchRMDForPrintReport, "BatchRMDForPrintReport");
                return dsBatchRMDForPrintReport;
            }
            catch
            {
                throw;
            }
        }

        //Start-SR:2014.01.29:BT2139: YRS 5.0-2165:RMD enhancements 
        public static string SaveRMDInitialLetterData(string paramStrPersId)
        {
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            DbCommand CommandUpdate = null;
            string strmessage;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();
                CommandUpdate = db.GetStoredProcCommand("yrs_usp_UpdateAtsMetaDetails");
                db.AddInParameter(CommandUpdate, "@chvKey", DbType.String, "RMDInitialLetterDate");
                db.AddInParameter(CommandUpdate, "@chvValue", DbType.String, DateTime.Now.ToString());
                db.AddInParameter(CommandUpdate, "@guiPersID", DbType.String, paramStrPersId);
                db.AddOutParameter(CommandUpdate, "@cOutValue", DbType.String, 1000);
                CommandUpdate.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.ExecuteNonQuery(CommandUpdate, DBTransaction);
                DBTransaction.Commit();
                DBconnectYRS.Close();
                strmessage = db.GetParameterValue(CommandUpdate, "@cOutValue").ToString();
                return strmessage;
            }
            catch
            {
                throw;
            }
        }
        //End-SR:2014.01.29:BT2139: YRS 5.0-2165:RMD enhancements 

        #endregion

        //Start:AA:05.07.2014 BT:2434:YRS 5.0-2315 - Added functions
        public static DataSet GetGeneratedRMDYears()
        {
            try
            {
                Database db = null;
                DbCommand LookUpCommandWrapper = null;
                DataSet dsGeneratedRMDYears = null;

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetGeneratedRMDYears");
                if (LookUpCommandWrapper == null) return null;
                db.ExecuteNonQuery(LookUpCommandWrapper);
                dsGeneratedRMDYears = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsGeneratedRMDYears, "GeneratedRMDYears");
                return dsGeneratedRMDYears;
            }
            catch
            {
                throw;
            }
        }
        public static Boolean IsAllowedToGenerateRMDForCurrentYear()
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            Boolean blnReturnStatus = false;
            DataSet dsGeneratedRMDYears = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_IsAllowToGenerateRMDForCurrentYear");
                if (LookUpCommandWrapper == null) return false;
                dsGeneratedRMDYears = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsGeneratedRMDYears, "IsAllowToGenerate");

                if (dsGeneratedRMDYears != null && dsGeneratedRMDYears.Tables.Count > 0 && dsGeneratedRMDYears.Tables[0].Rows.Count > 0)
                {
                    if (dsGeneratedRMDYears.Tables[0].Rows[0]["IsAllowToGenerate"].ToString() == "0")
                        blnReturnStatus = false;
                    else if (dsGeneratedRMDYears.Tables[0].Rows[0]["IsAllowToGenerate"].ToString() == "1")
                        blnReturnStatus = true;
                }
                return blnReturnStatus;
            }
            catch
            {
                throw;
            }
        }
        public static String GetLastRMDProcessedDate()
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            String strLastProcessedDate = null;
            DataSet dsRMDLastProcessed = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetLastRMDProcessedDate");
                if (LookUpCommandWrapper == null) return null;
                dsRMDLastProcessed = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsRMDLastProcessed, "IsAllowToGenerate");

                if (dsRMDLastProcessed != null && dsRMDLastProcessed.Tables[0].Rows.Count > 0)
                {
                    strLastProcessedDate = dsRMDLastProcessed.Tables[0].Rows[0]["LastProcessedGroup"].ToString();
                }
                return strLastProcessedDate;
            }
            catch
            {
                throw;
            }
        }
        public static void PrintInitialLetters()
        {
            try
            {

            }
            catch
            {

                throw;
            }
        }
        public static DataSet GetBatchRMDForFollowupLetter()
        {
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            DataSet dsBatchRMDForInitialLetter = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetBatchRMDForFollowupLetter");
                if (LookUpCommandWrapper == null) return null;
                //Start: AA:02.04.2015 BT:2139 - YRS 5.0-2165:RMD enhancements
                //db.ExecuteNonQuery(LookUpCommandWrapper);
                LookUpCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                //End: AA:02.04.2015 BT:2139 - YRS 5.0-2165:RMD enhancements
                dsBatchRMDForInitialLetter = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsBatchRMDForInitialLetter, "BatchRMDForInitialLetter");
                return dsBatchRMDForInitialLetter;
            }
            catch
            {

                throw;
            }
        }


        public static DataSet GetStagingDetails(string strbatchId, string strProcessName)
        {
            Database db = null;
            DbCommand dbIDMCommand = null;
            DataSet dsIDMTrackingLogDetails = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                dbIDMCommand = db.GetStoredProcCommand("yrs_usp_Process_GetStagingDetails");
                if (dbIDMCommand == null) return null;
                db.AddInParameter(dbIDMCommand, "@chvBatchId", DbType.String, strbatchId);
                db.AddInParameter(dbIDMCommand, "@chvProcessName", DbType.String, strProcessName);
                db.ExecuteNonQuery(dbIDMCommand);
                dsIDMTrackingLogDetails = new DataSet();
                db.LoadDataSet(dbIDMCommand, dsIDMTrackingLogDetails, "StagingLogsDetails");
                return dsIDMTrackingLogDetails;
            }
            catch
            {
                throw;
            }
        }

        public static string InsertStagingLogs(string strBatchId, string strFundEventId, string strRefId, string strLinkingId, string strFundno, string strProcessName)
        {
            Database db = null;
            DbCommand dbIDMCommand = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            string strmessage = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();

                if (db == null) return null;
                dbIDMCommand = db.GetStoredProcCommand("yrs_usp_InsertStagingLogs");
                if (dbIDMCommand == null) return null;
                db.AddInParameter(dbIDMCommand, "@chvBatchId", DbType.String, strBatchId);
                db.AddInParameter(dbIDMCommand, "@guiFundEventId", DbType.String, strFundEventId);
                db.AddInParameter(dbIDMCommand, "@chvRefId", DbType.String, strRefId);
                db.AddInParameter(dbIDMCommand, "@chvLinkingId", DbType.String, strLinkingId);
                db.AddInParameter(dbIDMCommand, "@intFundno", DbType.String, strFundno);
                db.AddInParameter(dbIDMCommand, "@chvProcessName", DbType.String, strProcessName);
                dbIDMCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                strmessage = Convert.ToString(db.ExecuteNonQuery(dbIDMCommand, DBTransaction));
                DBTransaction.Commit();
                DBconnectYRS.Close();
                return strmessage;
            }
            catch
            {
                throw;
            }
        }


        public static DataSet GetNextBatchId(DateTime dtRMDProcessDate)
        {
            DataSet dsBatchId = null;
            Database db = null;
            DbCommand CommandGetBatchId = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                CommandGetBatchId = db.GetStoredProcCommand("yrs_usp_RMDProcess_GetNextBatchID");
                db.AddInParameter(CommandGetBatchId, "@dtRMDProcessDate", DbType.Date, dtRMDProcessDate);
                if (CommandGetBatchId == null) return null;
                dsBatchId = new DataSet();
                db.LoadDataSet(CommandGetBatchId, dsBatchId, "BatchId");
                return dsBatchId;

            }
            catch
            {
                throw;
            }
        }

        //START: MMR | 2017.05.05 | YRS-AT-3205 | Changed return type of method from string to int
        //public static string InsertPrintLetters(string strRefId, string strPersId, string strLettersCode,string strChvNotes)    // SB | 2016.12.05 | YRS-AT-3203 | Added new parameter to describe plan type in print letter
        public static int InsertPrintLetters(string strRefId, string strPersId, string strLettersCode,string strChvNotes)
        //END: MMR | 2017.05.05 | YRS-AT-3205 | Changed return type of method from string to int
        {
            Database db = null;
            DbCommand dbIDMCommand = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            //START: MMR | 2017.05.05 | YRS-AT-3205 | Commented existing code and declared variable with type int to store printletter id
            //string strmessage = string.Empty;
            int printLetterID;
            //END: MMR | 2017.05.05 | YRS-AT-3205 | Commented existing code and declared variable with type int to store printletter id
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();
                //START: MMR | 2017.05.05 | YRS-AT-3205 | Returning 0 instead of null if object is null
                //if (db == null) return null;
                if (db == null) return 0;
                //END: MMR | 2017.05.05 | YRS-AT-3205 | Returning 0 instead of null if object is null
                dbIDMCommand = db.GetStoredProcCommand("yrs_usp_InsertPrintLetters");
                //START: MMR | 2017.05.05 | YRS-AT-3205 | Returning 0 instead of null if object is null
                //if (dbIDMCommand == null) return null;
                if (dbIDMCommand == null) return 0;
                //END: MMR | 2017.05.05 | YRS-AT-3205 | Returning 0 instead of null if object is null
                db.AddInParameter(dbIDMCommand, "@chvRefId", DbType.String, strRefId);
                db.AddInParameter(dbIDMCommand, "@guiPersId", DbType.String, strPersId);
                db.AddInParameter(dbIDMCommand, "@chvLettersCode", DbType.String, strLettersCode);
                db.AddInParameter(dbIDMCommand, "@VARCHAR_Notes", DbType.String, strChvNotes);                    // SB | 2016.12.05 | YRS-AT-3203 | Added new parameter to describe plan type in print letter
                db.AddOutParameter(dbIDMCommand, "@out_intUniqueId", DbType.Int32, 5); //MMR | 2017.05.05 | YRS-AT-3205 | Added output parameter for print letter id
                dbIDMCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                //strmessage = Convert.ToString(db.ExecuteNonQuery(dbIDMCommand, DBTransaction)); //MMR | 2017.05.05 | YRS-AT-3205 | Commented existing code
                //START: MMR | 2017.05.05 | YRS-AT-3205 | setting print letter id in variable from output variable
                db.ExecuteNonQuery(dbIDMCommand, DBTransaction);
                printLetterID = Convert.ToInt32(db.GetParameterValue(dbIDMCommand, "@out_intUniqueId"));
                //END: MMR | 2017.05.05 | YRS-AT-3205 | setting print letter id in variable from output variable
                DBTransaction.Commit();
                DBconnectYRS.Close();
                return printLetterID; //MMR | 2017.05.05 | YRS-AT-3205 | returning print letter id to calling method
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetCloseRMDDetails()
        {
            DataSet dsCloseRMDDetails = null;
            Database db = null;
            DbCommand CommandGetBatchId = null;
            string[] strTableName = new string[2];
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                CommandGetBatchId = db.GetStoredProcCommand("yrs_usp_RMDBatch_GetCloseRMDDetails");
                if (CommandGetBatchId == null) return null;
                dsCloseRMDDetails = new DataSet();
                strTableName[0] = "Generate";
                strTableName[1] = "Process";
                db.LoadDataSet(CommandGetBatchId, dsCloseRMDDetails, strTableName);
                return dsCloseRMDDetails;

            }
            catch
            {
                throw;
            }
        }


        //AA: 2015.10.21 YRS-AT-2614 - Added optional parameter access the YRS_IDM dataconfig which will use sql authonication for FT folder files as these files will take machine id for the windows authentication
        public static DataSet GetAtsTemp(string strBatchId, string strModule,bool blnSqlAuth = false)
        {
            DataSet dsCloseRMDDetails = null;
            Database db = null;
            DbCommand CommandGetBatchId = null;
            DataSet dttemp = new DataSet();
            try
            {
                //Start:AA: 2015.10.21 YRS-AT-2614 - Added access the YRS_IDM dataconfig which will use sql authonication for FT folder files as these files will take machine id for the windows authentication
                if (blnSqlAuth)
                {
                    db = DatabaseFactory.CreateDatabase("YRS_IDM");
                }
                else
                {
                    db = DatabaseFactory.CreateDatabase("YRS");
                }
                //End:AA: 2015.10.21 YRS-AT-2614 - Added access the YRS_IDM dataconfig which will use sql authonication for FT folder files as these files will take machine id for the windows authentication
                if (db == null) return null;
                CommandGetBatchId = db.GetStoredProcCommand("yrs_usp_GetAtsBatchCreationLogs");
                db.AddInParameter(CommandGetBatchId, "@chvBatchId", DbType.String, strBatchId);
                db.AddInParameter(CommandGetBatchId, "@chvModule", DbType.String, strModule);
                if (CommandGetBatchId == null) return null;
                dsCloseRMDDetails = new DataSet();

                db.LoadDataSet(CommandGetBatchId, dsCloseRMDDetails, "sample");

                string data = Convert.ToString(dsCloseRMDDetails.Tables[0].Rows[0][1]);
                dttemp.ReadXml(new System.IO.StringReader(data));

                return dttemp;
            }
            catch
            {
                throw;
            }
        }        

        public static DataSet GetAllTempBatchID()
        {
            DataSet dsALLRMDBatchID = null;
            Database db = null;
            DbCommand CommandGetBatchId = null;
            string[] tableNames; //MMR | 2016.10.27 | YRS-AT-2922 | Declared array variable to store table names
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                //START: MMR | 2016.10.27 | YRS-AT-2922 | Commented existing procedure and added new procedure to get batch process details
                //CommandGetBatchId = db.GetStoredProcCommand("yrs_usp_GetAllBatchId");                                           
                CommandGetBatchId = db.GetStoredProcCommand("yrs_usp_RMD_GetAllProcessedBatchId");
                //END: MMR | 2016.10.27 | YRS-AT-2922 | Commented existing procedure and added new procedure to get batch process details
                if (CommandGetBatchId == null) return null;
                dsALLRMDBatchID = new DataSet();
                //START: MMR | 2016.10.27 | YRS-AT-2922 | Commented existing code and added table names and passed to loaddataset method to store data
                tableNames = new string[] { "RMDYear", "RMDDueDate", "BatchId" };
                //db.LoadDataSet(CommandGetBatchId, dsALLRMDBatchID, "dsALLRMDBatchID");
                db.LoadDataSet(CommandGetBatchId, dsALLRMDBatchID, tableNames);
                //END: MMR | 2016.10.27 | YRS-AT-2922 | Commented existing code and added table names and passed to loaddataset method to store data
                return dsALLRMDBatchID;
            }
            catch
            {
                throw;
            }
        }

        //AA: 2015.10.21 YRS-AT-2614 - Added optional parameter to access the YRS_IDM dataconfig which will use sql authonication for FT folder files as these files will take machine id for the windows authentication
        public static string InsertAtsTemp(string strBatchId, string strModule, DataSet strData, bool blnSqlAuth = false)
        {
            Database db = null;
            DbCommand dbIDMCommand = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            string strmessage = string.Empty;
            DataSet dtTemp = new DataSet("dssample");
            string strreadFile = "";

            try
            {
                //Start:AA: 2015.10.21 YRS-AT-2614 - Added access the YRS_IDM dataconfig which will use sql authonication for FT folder files as these files will take machine id for the windows authentication
                if (blnSqlAuth)
                {
                    db = DatabaseFactory.CreateDatabase("YRS_IDM");
                }
                else
                {
                    db = DatabaseFactory.CreateDatabase("YRS");
                }
                //Start:AA: 2015.10.21 YRS-AT-2614 - Added access the YRS_IDM dataconfig which will use sql authonication for FT folder files as these files will take machine id for the windows authentication
                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();

                System.IO.StringWriter objstrWriter = new StringWriter();
                strData.WriteXml(objstrWriter);
                objstrWriter.ToString();

                strreadFile = objstrWriter.ToString();

                if (db == null) return null;
                dbIDMCommand = db.GetStoredProcCommand("yrs_usp_InsertAtsBatchCreationLogs");
                if (dbIDMCommand == null) return null;
                db.AddInParameter(dbIDMCommand, "@chvBatchId", DbType.String, strBatchId);
                db.AddInParameter(dbIDMCommand, "@chvModule", DbType.String, strModule);
                db.AddInParameter(dbIDMCommand, "@chvData", DbType.String, strreadFile);

                dbIDMCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                strmessage = Convert.ToString(db.ExecuteNonQuery(dbIDMCommand, DBTransaction));

                DBTransaction.Commit();
                DBconnectYRS.Close();
                return strmessage;
            }
            catch
            {
                throw;
            }
        }

        //End:AA:05.07.2014 BT:2434:YRS 5.0-2315 - Added functions

        //START: PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | Creating overload method of InsertAtsTemp by changing type of strData from DataSet to string
        //                                                 It will give flexibility of saving XML prepared by individual modules directly into system
        public static string InsertAtsTemp(string strBatchId, string strModule, string strData, bool blnSqlAuth = false)
        {
            Database db = null;
            DbCommand dbIDMCommand = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            string strMessage;
            try
            {
                //Start:AA: 2015.10.21 YRS-AT-2614 - Added access the YRS_IDM dataconfig which will use sql authonication for FT folder files as these files will take machine id for the windows authentication
                if (blnSqlAuth)
                {
                    db = DatabaseFactory.CreateDatabase("YRS_IDM");
                }
                else
                {
                    db = DatabaseFactory.CreateDatabase("YRS");
                }
                //Start:AA: 2015.10.21 YRS-AT-2614 - Added access the YRS_IDM dataconfig which will use sql authonication for FT folder files as these files will take machine id for the windows authentication
                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();

                if (db == null) return null;
                dbIDMCommand = db.GetStoredProcCommand("yrs_usp_InsertAtsBatchCreationLogs");
                if (dbIDMCommand == null) return null;
                db.AddInParameter(dbIDMCommand, "@chvBatchId", DbType.String, strBatchId);
                db.AddInParameter(dbIDMCommand, "@chvModule", DbType.String, strModule);
                db.AddInParameter(dbIDMCommand, "@chvData", DbType.String, strData);

                dbIDMCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);

                strMessage = Convert.ToString(db.ExecuteNonQuery(dbIDMCommand, DBTransaction));

                DBTransaction.Commit();
                //DBconnectYRS.Close(); //PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | Commented original method code. To handle Rollback in case any error occures, open connection is required
                return strMessage;
            }
            catch
            {
                DBTransaction.Rollback(); //PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | In case of error, rollback all changes
                throw;
            }
            //START: PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | Close connection if it is open in finally block and also mark all objects as null
            finally
            {
                if (DBconnectYRS != null)
                {
                    if (DBconnectYRS.State != ConnectionState.Closed)
                    {
                        DBconnectYRS.Close();
                    }
                    DBconnectYRS = null;
                }
                strMessage = null;
                db = null;
                dbIDMCommand = null;
                DBTransaction = null;
            }
            //END: PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | Close connection if it is open in finally block and also mark all objects as null
        }
        //END: PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | Creating overload method of InsertAtsTemp by changing type of strData from DataSet to string


        //Star-SR:2014.07.18 - BT:2434:YRS 5.0-2315-Added procedure to retieve RMD Process log
        public static DataSet GetRMDProcessLog()
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RMD_Get_RMDProcessLog");
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DBCommandWrapper == null) return null;
                l_DataSet = new DataSet("GetRMDProcessLog");
                l_TableNames = new string[] {"RMDProcessLog"};
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                return l_DataSet;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetRMDStatistics()
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            string[] l_TableNames;
            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");
                if (l_DataBase == null) return null;
                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_RMD_Get_RMDStatistics");
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                if (l_DBCommandWrapper == null) return null;
                l_DataSet = new DataSet("GetRMDStatistics");
                l_TableNames = new string[] { "ProcessDate","RMDStatistics" };
                l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames);
                return l_DataSet;
            }
            catch
            {
                throw;
            }
        }

        //End-SR:2014.07.18 - BT:2434:YRS 5.0-2315 - Added procedure to retieve RMD Process log

        // START | SR | 2016.04.05 | YRS-AT-2843 - YRS enh-RMD Utility should use J&S table if primary bene is spouse more than 10 yrs younger (TrackIT 25233)

        public static string UpdateParticipantRMDforSolePrimaryBeneficiary(int paramMRDUniqueId, decimal paramRMDTaxableAmount, decimal paramRMDNonTaxableAmount)
        {
            Database db = null;
            DbCommand dbIDMCommand = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            string strmessage = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();

                if (db == null) return null;
                dbIDMCommand = db.GetStoredProcCommand("yrs_usp_RMD_UpdateParticipantRMDforSolePrimaryBeneficiary");
                if (dbIDMCommand == null) return null;
                db.AddInParameter(dbIDMCommand, "@intMRDUniqueId", DbType.Int32, paramMRDUniqueId);
                db.AddInParameter(dbIDMCommand, "@decRMDTaxableAmount", DbType.Decimal, paramRMDTaxableAmount);
                db.AddInParameter(dbIDMCommand, "@decRMDNonTaxableAmount", DbType.Decimal, paramRMDNonTaxableAmount);
                dbIDMCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                strmessage = Convert.ToString(db.ExecuteNonQuery(dbIDMCommand, DBTransaction));
                DBTransaction.Commit();
                DBconnectYRS.Close();
                return strmessage;
            }
            catch
            {
                throw;
            }
        }
        // END | SR | 2016.04.05 | YRS-AT-2843 - YRS enh-RMD Utility should use J&S table if primary bene is spouse more than 10 yrs younger (TrackIT 25233)

        // START | SB | 2016.10.24 | YRS-AT-2685 - Get the whole batchid's for RMD reprint letters and details for selected batchid
        public static DataSet GetRMDLetterBatchIDList(string ModuleName)
        {
            DataSet ReprintLetterRecords = null;
            Database db = null;
            DbCommand cmdGetBatchId = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                cmdGetBatchId = db.GetStoredProcCommand("yrs_usp_GetAllProcessedBatchId");
                db.AddInParameter(cmdGetBatchId, "@VARCHAR_ModuleName", DbType.String, ModuleName);
               
                if (cmdGetBatchId == null) return null;
                ReprintLetterRecords = new DataSet();

                db.LoadDataSet(cmdGetBatchId, ReprintLetterRecords, "dsRePrintRMDBatchID");

                return ReprintLetterRecords;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetRMDLetterDetailsByBatchId(string BatchId, string ModuleName)
        {
            DataSet RMDLetterBatchDetails = null;
            Database db = null;
            DbCommand cmdGetBatchId = null;
            DataSet RMDLetterDetails = new DataSet();
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                cmdGetBatchId = db.GetStoredProcCommand("yrs_usp_GetRMDReprintLetterBatchDetails");
                db.AddInParameter(cmdGetBatchId, "@VARCHAR_BatchIdWithType", DbType.String, BatchId);
                db.AddInParameter(cmdGetBatchId, "@VARCHAR_Module", DbType.String, ModuleName);
                if (cmdGetBatchId == null) return null;
                RMDLetterBatchDetails = new DataSet();

                db.LoadDataSet(cmdGetBatchId, RMDLetterBatchDetails, "ReprintLetterDetails");

                string data = Convert.ToString(RMDLetterBatchDetails.Tables[0].Rows[0][0]);
                RMDLetterDetails.ReadXml(new System.IO.StringReader(data));

                return RMDLetterDetails;
            }
            catch
            {
                throw;
            }
        }
        // END | SB | 2016.10.24 | YRS-AT- 2685  - Get the whole batchid's for RMD reprint letters and details for selected batchid

        //START: CS | 2016.10.17 |  YRS-AT-2476 | Change to RMD utility - generate one release blank instead of two if same participant (TrackIT 22029)
        public static DataSet GetMrdRecordPlanWise(string batchId, string moduleName, bool isSqlAuth = false)
        {
            DataSet mrdRecords = null;
            Database db = null;
            DbCommand getBatchMRDDetails = null;
            try
            {
                if (isSqlAuth)
                {
                    db = DatabaseFactory.CreateDatabase("YRS_IDM");
                }
                else
                {
                    db = DatabaseFactory.CreateDatabase("YRS");
                }
                if (db == null) return null;
                getBatchMRDDetails = db.GetStoredProcCommand("yrs_usp_RMDBatch_GetLogsPlanWise");
                db.AddInParameter(getBatchMRDDetails, "@VARCHAR_BatchId", DbType.String, batchId);
                db.AddInParameter(getBatchMRDDetails, "@VARCHAR_Module", DbType.String, moduleName);
                if (getBatchMRDDetails == null) return null;
                mrdRecords = new DataSet();
                db.LoadDataSet(getBatchMRDDetails, mrdRecords, "BatchMRDRecords");
                return mrdRecords;
            }
            catch
            {
                throw;
            }
        }
        //END: CS | 2016.10.17 |  YRS-AT-2476 | Change to RMD utility - generate one release blank instead of two if same participant (TrackIT 22029)

        //START: MMR | 2016.10.27 |  YRS-AT-2922 | Added to get RMDBatch Process status details
        public static DataSet GetMrdRecordProcessStatus(string batchID, int year, string dueDate, bool isMarchClosed, bool isDecemberClosed)
        {
            DataSet mrdProcessStatus = null;
            Database db = null;
            DbCommand getBatchMRDProcessStatus = null;
            string[] TableNames;
            try
            {
                {
                    db = DatabaseFactory.CreateDatabase("YRS");
                }
                if (db == null) return null;
                getBatchMRDProcessStatus = db.GetStoredProcCommand("yrs_usp_GetRMDProcessStatus");
                db.AddInParameter(getBatchMRDProcessStatus, "@VARCHAR_BatchId", DbType.String, batchID);
                db.AddInParameter(getBatchMRDProcessStatus, "@INT_RMDYear", DbType.Int32, year);
                db.AddInParameter(getBatchMRDProcessStatus, "@VARCHAR_DueDate", DbType.String, dueDate);
                db.AddInParameter(getBatchMRDProcessStatus, "@BIT_IsMarchClosed", DbType.Boolean, isMarchClosed);
                db.AddInParameter(getBatchMRDProcessStatus, "@BIT_IsDecemberClosed", DbType.Boolean, isDecemberClosed);
                if (getBatchMRDProcessStatus == null) return null;
                mrdProcessStatus = new DataSet();
                TableNames = new string[] { "BatchMRDRecords", "ArrErrorDataList" };
                db.LoadDataSet(getBatchMRDProcessStatus, mrdProcessStatus, TableNames);
                return mrdProcessStatus;
            }
            catch
            {
                throw;
            }
        }
        //END: MMR | 2016.10.27 |  YRS-AT-2922 | Added to get RMDBatch Process status details

        //START: CS | 2016.11.01 |  YRS-AT-2922 | YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)
        public static void InsertNotProcessedAndNonEligibleParticipants(DateTime processdate)
        {
            Database db = null;
            DbTransaction transaction = null;
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                connection = db.CreateConnection();
                connection.Open();

                transaction = connection.BeginTransaction();

                command = db.GetStoredProcCommand("yrs_usp_RMD_InsertNotProcessedAndNonEligibleParticipants");
                db.AddInParameter(command, "@DATE_ProcessDate", DbType.DateTime, processdate);
                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                db.ExecuteNonQuery(command, transaction);

                transaction.Commit();

                connection.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                command = null;
                connection = null;
                transaction = null;
                db = null;
            }
        }
        //END: CS | 2016.11.01 |  YRS-AT-2922 | YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)

        //START: MMR | 2017.04.24 | YRS-AT-3205 | Get special QD participants with first RMD due in December for initial and follow-up letters
        # region "RMD Special QDRO"
        public static DataSet GetRMDSpecialQDInitialLetterParticipants()
        {
            Database db;
            DbCommand command;
            DataSet data;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                command = db.GetStoredProcCommand("yrs_usp_RMDBatch_SpecialQDInitialLetter");
                if (command == null) return null;
                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                data = new DataSet();
                db.LoadDataSet(command, data, "RMDSpecialQDParticipantsInitialLetter");
                return data;
            }
            catch
            {
                throw;
            }
            finally
            {
                data = null;
                command = null;
                db = null;
            }
        }

        public static DataSet GetRMDSpecialQDFollowupLetterParticipants()
        {
            Database db;
            DbCommand command;
            DataSet data;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                command = db.GetStoredProcCommand("yrs_usp_RMDBatch_SpecialQDFollowupLetter");
                if (command == null) return null;
                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                data = new DataSet();
                db.LoadDataSet(command, data, "RMDSpecialQDParticipantsFollowupLetter");
                return data;
            }
            catch
            {
                throw;
            }
            finally
            {
                data = null;
                command = null;
                db = null;
            }
        }
        # endregion

        public static DataSet GetIDMDetailsForReprinting(string batchID)
        {
            Database db;
            DbCommand command;
            DataSet data;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                command = db.GetStoredProcCommand("yrs_usp_RMDBatch_GetIDMDetailsForReprinting");
                db.AddInParameter(command, "@VARCHAR_BatchID", DbType.String, batchID);
                if (command == null) return null;
                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                data = new DataSet();
                db.LoadDataSet(command, data, "RMDLettersDetailsForReprint");
                return data;
            }
            catch
            {
                throw;
            }
            finally
            {
                data = null;
                command = null;
                db = null;
            }
        }
        //END: MMR | 2017.04.24 | YRS-AT-3205 | Get special QD participants with first RMD due in December for initial and follow-up letters

        //START: SB | 2017.04.11 |  YRS-AT-3400 & 3401
        #region "ReminderLetter"
        //To get the RMD Non-Respondends Annnual Letter Participants 
        public static DataSet GetRMDReminderLettersForNonRespondent()
        {
            Database db;
            DbCommand command;
            DataSet data;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                command = db.GetStoredProcCommand("yrs_usp_RMDBatch_ForNonRespondents");
                if (command == null) return null;
                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                data = new DataSet();
                db.LoadDataSet(command, data, "BatchRMDForRMDReminderLetters");
                return data;
            }
            catch
            {
                throw;
            }
            finally
            {
                data = null;
                command = null;
                db = null;
            }
        }

        //To get the RMD Non-Respondends Annnual Letter Participants 
        public static DataSet GetRMDReminderLettersForAnnualNotElected()
        {
            Database db;
            DbCommand command;
            DataSet data;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                command = db.GetStoredProcCommand("yrs_usp_RMDBatch_SatisfiedRMDsWithAnnualRMDUnselected");
                if (command == null) return null;
                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                data = new DataSet();
                db.LoadDataSet(command, data, "BatchRMDForRMDReminderLetters");
                return data;
            }
            catch
            {
                throw;
            }
            finally
            {
                data = null;
                command = null;
                db = null;
            }
        }
        #endregion
        //END: SB | 2017.04.11 |  YRS-AT-3400 & 3401

        //START: SB | 2017.07.26 | YRS-AT-3324 - Function to check if RMD need's to be generate/regenerate when processing withdrawal, death notification and death settlement
        #region "WithDrawal / Death Notification / Death Settlement Restrictions "
        //START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | Added additional parameter to function for checking deceased participants mrd records uptill deceased year
        //public static DataSet IsRMDGenerationRequired(string moduleName, string fundEventID)
        public static DataSet IsRMDGenerationRequired(string moduleName, string fundEventID, string deceasedDate)
        //END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 |Added additional parameter to function for checking deceased participants mrd records uptill deceased year
        {
            Database db = null;
            DbCommand command = null;
            DataSet dsErrorNos;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null; 
              
                command = db.GetStoredProcCommand("yrs_usp_RMD_Validate_RMDEligibility");
                if (command == null) return null;

                db.AddInParameter(command, "@VARCHAR_ModuleName", DbType.String, moduleName);
                db.AddInParameter(command, "@VARCHAR_FundEventId", DbType.String, fundEventID);
                db.AddInParameter(command, "@DATE_DeceasedDate", DbType.String, deceasedDate);  //SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | Added additional parameter to function for checking deceased participants mrd records uptill deceased year

                dsErrorNos = new DataSet();
                db.LoadDataSet(command, dsErrorNos, "ErrorMessage");

                return dsErrorNos;
			
            }
            catch
            {
                throw;
            }
            finally
            {
                dsErrorNos = null;
                command = null;
                db = null;
            }
        }
        #endregion
        //END: SB | 2017.07.26 | YRS-AT-3324 - Function to check if RMD need's to be generate/regenerate when processing withdrawal, death notification and death settlement
    }
}
