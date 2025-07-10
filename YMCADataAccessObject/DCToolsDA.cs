//**************************************************************************************************************/
// Author: Manthan Rajguru
// Created on: 09/13/2017
// Summary of Functionality: DC Tools data access layer
// Declared in Version: 20.4.0 | YRS-AT-3665 - YRS enh: Data Corrections Tool - Admin screen option to create a manual credit

//**************************************************************************************************************
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
// Pramod Prakash Pokale       | 09/18/2017   |	  20.4.0        | YRS-AT-3631 - YRS enh: Data Corrections Tool -Admin screen function to allow manual update of Fund status from WD or WP to TM or PT (TrackIT 30950) 
// Sanjay GS Rawat             | 09/18/2017   |	  20.4.0        | YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity
// Santosh Bura                | 09/18/2017   |	  20.4.0        | YRS-AT-3541 -  YRS enh: Data Corrections Tool -Admin screen function to allow modification of RDB amounts (excessive data corrections) 
// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
    public class DCToolsDA
    {

        #region "YMCA Credit"
        /// <summary>
        /// Provides YMCA credit details such as YMCA No, name and credit amount available with YMCA
        /// </summary>
        /// <param name="ymcaID">Ymca ID</param>
        /// <returns>Ymca Credit Details</returns>
        public static DataTable GetYMCACreditDetails(string ymcaID)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                
                cmd = db.GetStoredProcCommand("yrs_usp_DCTools_YC_GetYMCACredits");
                if (cmd == null) return null;
                
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@VARCHAR_YmcaID", DbType.String, ymcaID);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "YMCACreditDetails");
                return ds.Tables["YMCACreditDetails"];
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }

        /// <summary>
        /// Saves given Ymca credit amount in data source
        /// </summary>
        /// <param name="data">Ymca Credit Details</param>
        /// <param name="activityLogEntry">Activity Log Details</param>
        /// <returns></returns>
        public static Boolean SaveYMCACredits(YMCAObjects.DCTools.YMCACredit data, YMCAObjects.YMCAActionEntry activityLogEntry)
        {
            Database db = null;
            DbCommand cmd = null;
            DbTransaction transaction = null;
            DbConnection connection = null;
            string xmlData, notes, finalNotes, ymcaCreditID;
            try
            {
                xmlData = CommonUtilities.Utilities.SerializeToXML(data);
                notes = string.Format(data.SystemNotes, data.AddedCredit, data.ReceivedDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                finalNotes = string.Format("{0} {1}", notes, data.UserNotes);

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;

                //Get a connection and Open it
                connection = db.CreateConnection();
                connection.Open();

                transaction = connection.BeginTransaction();
                if (transaction == null) return false;

                cmd = db.GetStoredProcCommand("yrs_usp_DCTools_YC_SaveYMCACredits");
                if (cmd == null) return false;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@XML_YmcaCreditDetail", DbType.String, xmlData);
                db.AddOutParameter(cmd, "@UNIQUEIDENTIFIER_YMCACreditUniqueID", DbType.String, 1000);
                db.ExecuteNonQuery(cmd, transaction);
                ymcaCreditID = Convert.ToString(db.GetParameterValue(cmd, "@UNIQUEIDENTIFIER_YMCACreditUniqueID"));
                activityLogEntry.Data = String.Format("<DC><YmcaCreditID>{0}</YmcaCreditID><ReceivedDate>{1}</ReceivedDate><CreditAmount>${2}</CreditAmount><CreditSource>{3}</CreditSource><ReceivedAccDate>{4}</ReceivedAccDate></DC>", ymcaCreditID, data.ReceivedDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture), data.AddedCredit, data.CreditSourceCode, data.ReceivedAccountingDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));

                //Insert notes after saving YMCA Credit
                NotesDAClass.InsertNotes(data.YMCAID, finalNotes, false, transaction);

                //Insert into YRS Activity log after entry into notes
                LoggerDA.WriteLogDB(activityLogEntry, transaction);

                //Commit the transaction if everything was fine
                transaction.Commit();
                
                return true;
            }
            catch
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                xmlData = string.Empty;
                notes = string.Empty;
                finalNotes = string.Empty;
                ymcaCreditID = string.Empty;
                connection = null;
                transaction = null;
                cmd = null;
                db = null;
            }
        }

        /// <summary>
        /// Validating credit amount and return error message no and threshold amount to be displayed in message .
        /// </summary>
        /// <param name="creditAmount">Credit Amount</param>
        /// <returns> Error message No and Threshold amount in datatable</returns>
        public static DataTable ValidateCreditAmount(decimal creditAmount)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_DCTools_YC_ValidateCreditAmount");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@NUMERIC_CreditAmount", DbType.Decimal, creditAmount);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "YMCACreditValidationDetails");
                return ds.Tables["YMCACreditValidationDetails"];
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }
        #endregion

        #region "Fund Status Change"
        //START: PPP | 09/18/2017 | YRS-AT-3631 | DC Tools - change fund event's Get and Save methods
        /// <summary>
        /// Provides details of fund status and balance for the given Fund Event ID
        /// </summary>
        /// <param name="fundEventID">Fund Event ID</param>
        /// <returns>Details of Fund and list of available fund status</returns>
        public static DataSet GetFundStatusDetails(string fundEventID)
        {
            Database db;
            DbCommand cmd;
            DataSet ds;
            //YMCAObjects.DCTools.FundStatus data;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;
                cmd = db.GetStoredProcCommand("yrs_usp_DCTools_FS_GetFundStatus");
                if (cmd == null) return null;
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@VARCHAR_FundEventID", DbType.String, fundEventID);
                db.ExecuteNonQuery(cmd);

                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "YMCACreditDetails");

                //data = null;
                //if (HelperFunctions.isNonEmpty(ds))
                //{
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        data = new YMCAObjects.DCTools.FundStatus();

                //    }
                //}

                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds = null;
                cmd = null;
                db = null;
            }
        }

        /// <summary>
        /// Updates fund status from old to new for the given person details
        /// </summary>
        /// <param name="data">Person Details</param>
        /// <param name="activityLogDetails">Log Details</param>
        /// <returns>Success Status (True / False)</returns>
        public static Boolean SaveFundStatusChanges(YMCAObjects.DCTools.FundStatus data, YMCAObjects.YMCAActionEntry activityLogDetails)
        {
            Database db = null;
            DbCommand cmd = null;
            DbTransaction transaction = null;
            DbConnection connection = null;
            string xml;
            string notes;
            try
            {
                xml = CommonUtilities.Utilities.SerializeToXML(data);
                notes = string.Format("{0} {1}", String.Format(data.SystemNotes, data.OldFundEventStatus, data.NewFundEventStatus), string.IsNullOrEmpty(data.UserNotes) ? String.Empty : data.UserNotes);

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;
                //Get a connection and Open it
                connection = db.CreateConnection();
                connection.Open();

                transaction = connection.BeginTransaction();
                if (transaction == null) return false;

                cmd = db.GetStoredProcCommand("yrs_usp_DCTools_FS_SaveFundStatus");
                if (cmd == null) return false;
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@XML_FundStatusDetail", DbType.String, xml);
                db.ExecuteNonQuery(cmd, transaction);

                //Insert notes after saving YMCA Credit
                NotesDAClass.InsertNotes(data.PersID, notes, false, transaction);

                //Insert into YRS Activity log after entry into notes
                LoggerDA.WriteLogDB(activityLogDetails, transaction);

                //Commit the transaction if everything was fine
                transaction.Commit();
                return true;
            }
            catch
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                xml = string.Empty;
                notes = string.Empty; connection = null;
                transaction = null;
                cmd = null;
                db = null;
            }
        }
        //END: PPP | 09/18/2017 | YRS-AT-3631 | DC Tools - change fund event's Get and Save methods
        #endregion

        #region "Reverse Annuity"
        // START | SR | 09/18/2017   |	YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity
        /// <summary>
        /// Method to validate selected annuities for following
        /// '1. Annuity is void & reversed
        /// '2. QDRO split has not been performed using Annuity.
        /// </summary>
        /// <param name="YMCAId"></param>
        /// <returns></returns>
        public static string ValidateAnnuity(string DisbursementId)
        {
            string AnnuityStatus = "0";
            Database db = null;
            DbCommand cmdInsertYMCACredits = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;
                cmdInsertYMCACredits = db.GetStoredProcCommand("yrs_usp_DCTools_RA_ValidateAnnuity");
                if (cmdInsertYMCACredits == null) return null;
                cmdInsertYMCACredits.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmdInsertYMCACredits, "@VARCHAR_DisbursementId", DbType.String, DisbursementId);
                db.AddOutParameter(cmdInsertYMCACredits, "@INT_AnnuityStatus", DbType.String, 40);
                db.ExecuteNonQuery(cmdInsertYMCACredits);
                AnnuityStatus = Convert.ToString(db.GetParameterValue(cmdInsertYMCACredits, "@INT_AnnuityStatus"));
                return AnnuityStatus;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// This method will return annuitants (1) all annuities (group by disbursement) which are not in paid status. (2) All annuities without any grouping        
        /// </summary>
        /// <param name="YMCAId"></param>
        /// <returns></returns>
        public static DataSet LookUpAnnuities(string fundEventId)
        {
            DataSet dsLookUpAnnuities = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string[] tableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_DCTools_RA_Annuities");
                if (LookUpCommandWrapper == null) return null;                
                db.AddInParameter(LookUpCommandWrapper, "@VARCHAR_FundeventId", DbType.String, fundEventId);
                dsLookUpAnnuities = new DataSet();
                tableNames = new string[] { "Annuities", "AnnuitiesDetails" };
                db.LoadDataSet(LookUpCommandWrapper, dsLookUpAnnuities, tableNames);
                
                return dsLookUpAnnuities;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method will return recommended fundstatus based on annuitant retiree & transaction status.
        /// </summary>
        /// <param name="YMCAId"></param>
        /// <returns></returns>
        public static string RecommendedFundEventStatus(string DisbursementId)
        {
            string FundStatus = "0";
            Database db = null;
            DbCommand cmdFundStatus = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;
                cmdFundStatus = db.GetStoredProcCommand("yrs_usp_DCTools_RA_RecommendedFundEventStatus");
                if (cmdFundStatus == null) return null;
                cmdFundStatus.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmdFundStatus, "@VARCHAR_DisbursementId", DbType.String, DisbursementId);
                db.AddOutParameter(cmdFundStatus, "@VARCHAR_RecommendedFundStatus", DbType.String, 40);
                db.ExecuteNonQuery(cmdFundStatus);
                FundStatus = Convert.ToString(db.GetParameterValue(cmdFundStatus, "@VARCHAR_RecommendedFundStatus"));
                return FundStatus;
            }
            catch
            {
                throw;
            }

        }
        
        /// <summary>
        /// This method will reverse selected annuity, insert Notes & activity Logs.
        /// </summary>
        /// <param name="YMCAId"></param>
        /// <returns></returns>
        public static string ReverseAnnuity(string DisbursementId, string FundStatus, string Notes, string PersId, YMCAObjects.YMCAActionEntry activityLogEntry)
        {
            Database db = null;
            DbCommand cmdReverseAnnuity = null;
            DbTransaction tranReverseAnnuity = null;
            DbConnection connection = null;
            string outPut = "";

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return "";

                //Get a connection and Open it
                connection = db.CreateConnection();
                connection.Open();

                tranReverseAnnuity = connection.BeginTransaction();
                if (tranReverseAnnuity == null) return "";

                cmdReverseAnnuity = db.GetStoredProcCommand("yrs_usp_DCTools_RA_ReverseAnnuities");
                if (cmdReverseAnnuity == null) return "";
                cmdReverseAnnuity.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmdReverseAnnuity, "@VARCHAR_DisbursementId", DbType.String, DisbursementId);
                db.AddInParameter(cmdReverseAnnuity, "@VARCHAR_FundStatus", DbType.String, FundStatus);                
                db.AddOutParameter(cmdReverseAnnuity, "@VARCHAR_Output", DbType.String, 1000);
                db.ExecuteNonQuery(cmdReverseAnnuity, tranReverseAnnuity);
                outPut = Convert.ToString(db.GetParameterValue(cmdReverseAnnuity, "@VARCHAR_Output"));

                // If error occurs while executing reverse annuity, rollback changes and display error message to user.
                if (!(string.IsNullOrEmpty(outPut.Trim()))) {
                    tranReverseAnnuity.Rollback();
                    return outPut;
                }
                              
                //Insert notes after annuity reverse
                NotesDAClass.InsertNotes(PersId, Notes, false, tranReverseAnnuity);

                //Insert into YRS Activity log after entry into notes
                LoggerDA.WriteLogDB(activityLogEntry, tranReverseAnnuity);

                //Commit the transaction if everything was fine
                tranReverseAnnuity.Commit();
                return outPut;
            }
            catch
            {
                if (tranReverseAnnuity != null)
                {
                    tranReverseAnnuity.Rollback();
                }
                throw;
            }
            finally
            {
                if (tranReverseAnnuity != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                connection = null;
                tranReverseAnnuity = null;
                db = null;
            }

        }
        // END | SR | 09/18/2017   |	YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity

        #endregion

        #region "Remaining Death Benfit"
        // START | SB | 09/18/2017 | YRS-AT-3541 - YRS enh: Data Corrections Tool -Edit Remaining Death Benefit amount
        /// <summary>
        /// Provides purchased annuity details along with Personal details, remaining death benefit total and system note
        /// </summary>
        /// <param name="fundEventID"> Fund Event Id </param>
        /// <param name="errorCode"> errorCode </param>
        /// <returns>Annuities list, participant's information, remaining death benefit total and system note. </returns>
        public static DataSet GetAnnuitiesInformation(string fundEventID,out int errorCode)
        {
            DataSet annuitiesInformation = new DataSet();
            Database db = null;
            DbCommand cmd = null;
            errorCode = 0;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;
                cmd = db.GetStoredProcCommand("yrs_usp_DCTools_DB_GetAnnuitiesDetail");
                if (cmd == null) return null;
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@VARCHAR_FundeventId", DbType.String, fundEventID);
                db.AddOutParameter(cmd, "@INT_ErrorCode", DbType.Int32, 100);
                db.ExecuteNonQuery(cmd);
                db.LoadDataSet(cmd, annuitiesInformation, "AnnuitiesList");
                errorCode = Convert.IsDBNull(db.GetParameterValue(cmd, "@INT_ErrorCode"))? 0 : Convert.ToInt32(db.GetParameterValue(cmd, "@INT_ErrorCode"));
                return annuitiesInformation;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd = null;
                db = null;
                annuitiesInformation = null;
            }
        }

        /// <summary>
        /// Edits remaining death benefit amount with notes and logs the activity in data source.
        /// </summary>
        /// <param name="data">Remaining death benefit details</param>
        /// <param name="activityLogEntry">Activity log details</param>
        /// <returns></returns>
        public static Boolean EditRemainingDeathBenefitAmount(YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount data, YMCAObjects.YMCAActionEntry activityLogEntry)
        {
            Database db = null;
            DbCommand cmd = null;
            DbTransaction transaction = null;
            DbConnection connection = null;
            string xmlData,notes, transactID ;
            try
            {
                xmlData = CommonUtilities.Utilities.SerializeToXML(data);
                notes = string.Format("{0} {1}", data.SystemNotes, data.UserNotes);

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;

                //Get a connection and Open it
                connection = db.CreateConnection();
                connection.Open();

                // Begin transaction
                transaction = connection.BeginTransaction();
                if (transaction == null) return false;

                cmd = db.GetStoredProcCommand("yrs_usp_DCTools_DB_SaveRemainingDeathBenefit");
                if (cmd == null) return false;
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@XML_EditRDBAmountDetails", DbType.String, xmlData);
                db.AddOutParameter(cmd, "@UNIQUEIDENTIFIER_TransactID", DbType.String,1000 );

                db.ExecuteNonQuery(cmd, transaction);
                //get the DBTransact Id inserted in death benefit transacts table 
                transactID = Convert.ToString(db.GetParameterValue(cmd, "@UNIQUEIDENTIFIER_TransactID"));
                activityLogEntry.Data = String.Format("<DC><OldRemainingDeathBenefitAmount>${0}</OldRemainingDeathBenefitAmount><NewRemainingDeathBenefitAmount>${1}</NewRemainingDeathBenefitAmount><DeathBenefitTransacts UniqueID=\"{2}\" Date=\"{3}\" Type=\"{4}\" Amount=\"${5}\" /></DC>", data.OldAmount, data.NewAmount, transactID,data.TransactDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture), data.TransactType, data.NewDifferenceAmount);
               
                //Insert into atsnotes after updating Remaining Death Benefit amount
                NotesDAClass.InsertNotes(data.PerssID, notes, false, transaction);

                //Insert into YRS Activity log after entry into notes
                LoggerDA.WriteLogDB(activityLogEntry, transaction);

                //Commit the transaction if everything was fine
                transaction.Commit();
                return true;
            }
            catch
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                connection = null;
                transaction = null;
                cmd = null;
                db = null;
                xmlData = null;
                notes = null;
            }

        }
        // END | SB | 09/18/2017 | YRS-AT-3541 - YRS enh: Data Corrections Tool -Edit Remaining Death Benefit amount
        #endregion 
    }
}
