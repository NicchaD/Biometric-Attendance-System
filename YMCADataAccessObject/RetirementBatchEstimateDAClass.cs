//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA YRS
// FileName			:	RetirementBatchEstimateDAClass.cs
// Author Name		:	Shashank Patel
// Employee ID		:	55381
// Email			:	shashank.patel@3i-infotech.com
// Contact No		:	8618
// Creation Time	:	22-Nov-2010
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:YRS 5.0-1365 : Need new batch processing option	
//************************************************************************************
//Modficiation History
//************************************************************************************
//Date			Modified By			Description
//************************************************************************************
//2012.02.29   Shashank Patel       Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant
//2012.07.23   Shashank Patel		BT-:1007 Batch Estimate observation.
//2014.10.10   Anudeep  Adusumilli  BT:2357 YRS 5.0-2285 - Need ability to exclude pre-eligible participants 
//2015.09.16   Manthan Rajguru      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//****************************************************************************************************

#region NameSpace

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Globalization;

#endregion

namespace YMCARET.YmcaDataAccessObject
{
    public class RetirementBatchEstimateDAClass
    {
        #region Constructor
        public RetirementBatchEstimateDAClass()
        {
        }
        #endregion

        //YRS 5.0-1365 : Need new batch processing option - Start 
        public static DataSet GetEstimatePersonInfo(string parameterFundNo, string parameterFirstName, string parameterLastName, string parameterSSNo, string parameterYmcaName)
        {

            DataSet dsLookUpPersons = null;
            Database db = null;
            DbCommand commandLookUpPersons = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_FindRetEstimatePerson");

                if (commandLookUpPersons == null) return null;

                commandLookUpPersons.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

                dsLookUpPersons = new DataSet();
                db.AddInParameter(commandLookUpPersons, "@varchar_FundIdNo", DbType.String, parameterFundNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_FName", DbType.String, parameterFirstName);
                db.AddInParameter(commandLookUpPersons, "@varchar_LName", DbType.String, parameterLastName);
                db.AddInParameter(commandLookUpPersons, "@varchar_SSN", DbType.String, parameterSSNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_YmcaName", DbType.String, parameterYmcaName);
                db.LoadDataSet(commandLookUpPersons, dsLookUpPersons, "Persons");
            }
            catch
            {
                throw;
            }
            finally
            {
                commandLookUpPersons.Dispose();
                commandLookUpPersons = null;
                db = null;
            }

            return dsLookUpPersons;
        }
        public static DataSet GetEstimatePersonInfo(string parameterFundNo, string parameterFirstName, string parameterLastName, string parameterSSNo, string parameterYmcaName, string parameterYmcaNo, string tcExcludeFundEvents)
        {

            DataSet dsLookUpPersons = null;
            Database db = null;
            DbCommand commandLookUpPersons = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_FindRetEstimatePerson");

                if (commandLookUpPersons == null) return null;

                commandLookUpPersons.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

                dsLookUpPersons = new DataSet();
                db.AddInParameter(commandLookUpPersons, "@varchar_FundIdNo", DbType.String, parameterFundNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_FName", DbType.String, parameterFirstName);
                db.AddInParameter(commandLookUpPersons, "@varchar_LName", DbType.String, parameterLastName);
                db.AddInParameter(commandLookUpPersons, "@varchar_SSN", DbType.String, parameterSSNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_YmcaName", DbType.String, parameterYmcaName);
                db.AddInParameter(commandLookUpPersons, "@varchar_YmcaNo", DbType.String, parameterYmcaNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_ExcludeFE", DbType.String, tcExcludeFundEvents); //AA:10.10.2014 BT:2357-YRS 5.0-2285 - Added to exclude the ra , pe fund events
                db.LoadDataSet(commandLookUpPersons, dsLookUpPersons, "Persons");
            }
            catch
            {
                throw;
            }
            finally
            {
                commandLookUpPersons.Dispose();
                commandLookUpPersons = null;
                db = null;
            }

            return dsLookUpPersons;
        }
        //2012.02.29   Shashank Patel       Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant
        //public static int InsertRetirementBatchEstimateParamater(string parameterRetireAge, string ParameterRetireDeathBenificary, string parameterRetireSal, string parameterRetirePlanType, string parameterRetireInterest, string parameterFutureSalEffectiveDate, int parameterUserID)
        //{
        //    DbConnection DBconnectYRS = null;
        //    Database db = null;
        //    DbCommand InsertCommandWrapper = null;
        //    DbTransaction DBTransaction = null;
        //    int success = 0;

        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");

        //        DBconnectYRS = db.CreateConnection();
        //        DBconnectYRS.Open();
        //        DBTransaction = DBconnectYRS.BeginTransaction();

        //        InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_InsertRetBatchEstimateParameter");
        //        db.AddInParameter(InsertCommandWrapper, "@integer_RetAge", DbType.Int32, parameterRetireAge);
        //        db.AddInParameter(InsertCommandWrapper, "@integer_RetDeathBenf", DbType.Int32, ParameterRetireDeathBenificary);
        //        db.AddInParameter(InsertCommandWrapper, "@integer_RetSal", DbType.Int32, parameterRetireSal);
        //        db.AddInParameter(InsertCommandWrapper, "@char_RetPlanType", DbType.String, parameterRetirePlanType);
        //        db.AddInParameter(InsertCommandWrapper, "@integer_RetIntRates", DbType.Int32, parameterRetireInterest);
        //        db.AddInParameter(InsertCommandWrapper, "@datetime_FutureSalEffective", DbType.String, parameterFutureSalEffectiveDate);
        //        db.AddInParameter(InsertCommandWrapper, "@integer_Usr_pk", DbType.Int32, parameterUserID);
        //        InsertCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

        //        success = db.ExecuteNonQuery(InsertCommandWrapper, DBTransaction);
        //        DBTransaction.Commit();
        //        DBconnectYRS.Close();

        //    }
        //    catch
        //    {
        //        throw;

        //    }
        //    finally
        //    {
        //        InsertCommandWrapper.Dispose();
        //        InsertCommandWrapper = null;
        //        db = null;
        //    }
        //    return success;
        //}

        //2012.02.29   Shashank Patel       Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant-End
        //public static int InsertRetirementBatchEstimate(DataSet parameterDsBatchEstimate, int parameterUserID)
        //{

        //    DbConnection DBconnectYRS = null;
        //    Database db = null;
        //    DbCommand InsertCommandWrapper = null;
        //    DbTransaction DBTransaction = null;
        //    int success = 0;

        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");

        //        DBconnectYRS = db.CreateConnection();
        //        DBconnectYRS.Open();
        //        DBTransaction = DBconnectYRS.BeginTransaction();

        //        InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_InsertRetBatchEstimateXML");
        //        db.AddInParameter(InsertCommandWrapper, "@integer_Usr_pk", DbType.Int32, parameterUserID);
        //        db.AddInParameter(InsertCommandWrapper, "@xml_BatchEstimate", DbType.String, parameterDetailsXml);
        //        InsertCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

        //        //success = db.UpdateDataSet(InsertCommandWrapper, DBTransaction);
        //        DBTransaction.Commit();
        //        DBconnectYRS.Close();

        //    }
        //    catch
        //    {
        //        throw;

        //    }
        //    finally
        //    {
        //        InsertCommandWrapper.Dispose();
        //        InsertCommandWrapper = null;
        //        db = null;
        //        DBconnectYRS.Dispose();
        //    }
        //    return success;

        //}
        public static int InsertRetirementBatchEstimate(string parameterDetailsXml,int parameterUserID)
        {

            DbConnection DBconnectYRS = null;
            Database db = null;
            DbCommand InsertCommandWrapper = null;
            DbTransaction DBTransaction = null;
            int success = 0;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();

                InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_InsertRetBatchEstimateXML");
                db.AddInParameter(InsertCommandWrapper, "@integer_Usr_pk", DbType.Int32, parameterUserID);
                db.AddInParameter(InsertCommandWrapper, "@xml_BatchEstimate", DbType.String, parameterDetailsXml);
                InsertCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                success = db.ExecuteNonQuery(InsertCommandWrapper, DBTransaction);
                DBTransaction.Commit();
                DBconnectYRS.Close();

            }
            catch
            {
                throw;

            }
            finally
            {
                InsertCommandWrapper.Dispose();
                InsertCommandWrapper = null;
                db = null;
                DBconnectYRS.Dispose();
            }
            return success;

        }
        //2012.02.29   Shashank Patel  :     Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant (remove parameter xmlfundeeventsids)
        public static int DeleteRetirementBatchEstimateByUserID(int parameterUserID)
        {

            DbConnection DBconnectYRS = null;
            Database db = null;
            DbCommand DeleteCommandWrapper = null;
            DbTransaction DBTransaction = null;
            int success = 0;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();

                DeleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeleteRetBatchEstimate");
                db.AddInParameter(DeleteCommandWrapper, "@integer_Usr_pk", DbType.Int32, parameterUserID);
                //2012.02.29   Shashank Patel       Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant
                //db.AddInParameter(DeleteCommandWrapper, "@xml_guiFundEventID", DbType.String, parameterXmlGuiFundEventId);
                DeleteCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                success = db.ExecuteNonQuery(DeleteCommandWrapper, DBTransaction);
                DBTransaction.Commit();
                DBconnectYRS.Close();

            }
            catch
            {
                throw;

            }
            finally
            {
                DeleteCommandWrapper.Dispose();
                DeleteCommandWrapper = null;
                db = null;
            }
            return success;

        }
        //2012.02.29   Shashank Patel       Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant
        //public static DataSet GetEstimateParameterByUserID(int parameterUserID)
        //{

        //    DataSet dsSearchEstimateParameter = null;
        //    Database db = null;
        //    DbCommand SearchCommandWrapper = null;

        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("YRS");
        //        if (db == null) return null;

        //        SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetBatchEstimateParameterByUser");

        //        if (SearchCommandWrapper == null) return null;

        //        db.AddInParameter(SearchCommandWrapper, "@integer_Usr_pk", DbType.Int32, parameterUserID);

        //        SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

        //        dsSearchEstimateParameter = new DataSet();
        //        dsSearchEstimateParameter.Locale = CultureInfo.InvariantCulture;
        //        db.LoadDataSet(SearchCommandWrapper, dsSearchEstimateParameter, "RetirementEstimateParamater");

        //        return dsSearchEstimateParameter;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        SearchCommandWrapper.Dispose();
        //        SearchCommandWrapper = null;
        //        db = null;
        //    }
        //}
        //2012.02.29   Shashank Patel       Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant-End

        public static DataSet GetBatchEstimateByUserID(int parameterUserID)
        {

            DataSet dsSearchEstimateParameter = null;
            Database db = null;
            DbCommand SearchCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetRetBatchEstimateByUser");

                if (SearchCommandWrapper == null) return null;

                db.AddInParameter(SearchCommandWrapper, "@integer_Usr_pk", DbType.Int32, parameterUserID);

                SearchCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                dsSearchEstimateParameter = new DataSet();
                dsSearchEstimateParameter.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(SearchCommandWrapper, dsSearchEstimateParameter, "RetirementEstimatePerson");

                return dsSearchEstimateParameter;
            }
            catch
            {
                throw;
            }
            finally
            {
                SearchCommandWrapper.Dispose();
                SearchCommandWrapper = null;
                db = null;
            }
        }

		//SP -2012.07.23 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
        public static int GetRetirementBatchListCount(int iUserId)
        {
            Database db = null;
            DbCommand CountCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return 0;
                CountCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetRetirementBatchListCount");
                if (CountCommandWrapper == null) return 0;
                CountCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(CountCommandWrapper, "@UserId", DbType.Int16, iUserId);
                db.AddOutParameter(CountCommandWrapper, "@VarCount", DbType.Int16, 4);
                db.ExecuteNonQuery(CountCommandWrapper);
                return Convert.ToInt16(db.GetParameterValue(CountCommandWrapper, "@VarCount"));
            }
            catch
            {
                throw;
            }
            finally
            {
                CountCommandWrapper.Dispose();
                CountCommandWrapper = null;
                db = null;
            }
        }
		//SP -2012.07.23: BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
        
        //YRS 5.0-1365 : Need new batch processing option -End
    }
}
