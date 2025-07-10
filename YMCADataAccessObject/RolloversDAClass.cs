//****************************************************
//Modification History
//*******************************************************************************
//Author			Date                Description
//*******************************************************************************
//Neeraj Singh      06/jun/2010         Enhancement for .net 4.0
//Neeraj Singh      07/jun/2010         review changes done
//Priya Patil		15-05-2012			BT-1028,YRS 5.0-1577: Add new field to atsRollovers
//Anudeep A         15-05-2014          BT-1051:YRS 5.0-1618 :Enhancements to Roll In process    
//Manthan Rajguru   2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Manthan Rajguru   2016.08.22          YRS-AT-2980 -  YRS enh: FiratName, LastName, Addr1, Addr2 fields should not allow Accent Characters or Tilde Characters
//*******************************************************************************
using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for RolloversDAClass.
	/// </summary>
	public class RolloversDAClass
	{
		public RolloversDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpPerson(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName,string parameterFormName)
		{
			DataSet dsLookUpPersons = null;
			Database db= null;
			DbCommand commandLookUpPersons = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				if(db==null) return null;
				commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_FindPerson");
				if (commandLookUpPersons==null) return null;
				commandLookUpPersons.CommandTimeout = 1200;
				dsLookUpPersons = new DataSet();
				db.AddInParameter(commandLookUpPersons,"@varchar_SSN",DbType.String,parameterSSNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_FundIdNo", DbType.String, parameterFundNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_LName", DbType.String, parameterLastName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FName", DbType.String, parameterFirstName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FormName", DbType.String, parameterFormName);
				db.LoadDataSet(commandLookUpPersons,dsLookUpPersons,"Persons");
				return dsLookUpPersons;
			}
			catch 
			{
				throw ;
			}

		}

		public static DataSet LookUpEmploymentInfo(string parameterPersId,string parameterFundId)
		{
			DataSet l_dataset_dsLookUpEmploymentInfo = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_SearchParticipantEmployment");
				
				if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId", DbType.String, parameterPersId);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_FundId", DbType.String, parameterFundId);
			
				l_dataset_dsLookUpEmploymentInfo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpEmploymentInfo,"EmploymentInfo");
				return l_dataset_dsLookUpEmploymentInfo;
			}
			catch 
			{
				throw;
			}
		}


		public static DataSet LookUpGeneralInfo(string parameterPersId)
		{
			DataSet l_dataset_dsLookUpGeneralInfo = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_SearchParticipantGeneral");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_gui_UniqueId",DbType.String,parameterPersId);
				
			
				l_dataset_dsLookUpGeneralInfo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpGeneralInfo,"GeneralInfo");
				return l_dataset_dsLookUpGeneralInfo;
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet LookUpRolloverInfo(string parameterPersId)
		{
			DataSet l_dataset_dsLookUpRolloverInfo = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_SearchParticipantRollover");
				
				if (LookUpCommandWrapper == null) return null;
				LookUpCommandWrapper.CommandTimeout = 1200;
				db.AddInParameter(LookUpCommandWrapper,"@varchar_gui_UniqueId",DbType.String,parameterPersId);
				
			
				l_dataset_dsLookUpRolloverInfo = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpRolloverInfo,"RolloverInfo");
				return l_dataset_dsLookUpRolloverInfo;
			}
			catch 
			{
				throw;
			}
		}


        public static string SaveRolloverInfo(string parameterPersId, string parameterFundId, string parameterInstName, string parameterStatus, string parameterDate, string paramaterInfoSource, string paramaterParticipantAccountNo, bool parameterGenerateLtr, DataSet parameterAddress, string parameterAddressId) //AA:14.05.2014 BT:1051 - YRS 5.0-1618 - updated to save the rollover data with account number and generate letter
        {

            Database db = null;
            DbCommand SaveCommandWrapper = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            String strOutput;
            string parameterInstitutionId = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                SaveCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_SaveParticipantRollover");

                if (SaveCommandWrapper == null) return null;

                DBconnectYRS = db.CreateConnection();

                DBconnectYRS.Open();

                DBTransaction = DBconnectYRS.BeginTransaction(IsolationLevel.ReadUncommitted);

                GetRefundRolloverInstitutionID(parameterInstName, out parameterInstitutionId, DBTransaction, db);

                if (parameterAddressId == string.Empty && parameterAddress != null && parameterAddress.Tables.Count != 0 && parameterAddress.Tables[0].Rows.Count > 0 && parameterInstitutionId != string.Empty)
                {
                    parameterAddress.Tables[0].Rows[0]["guiEntityId"] = parameterInstitutionId;
                    parameterAddressId = AddressDAClass.SaveInstitutionAddress(parameterAddress, DBTransaction, db);
                }

                db.AddInParameter(SaveCommandWrapper, "@varchar_PersId", DbType.String, parameterPersId);
                db.AddInParameter(SaveCommandWrapper, "@varchar_FundId", DbType.String, parameterFundId);
                db.AddInParameter(SaveCommandWrapper, "@varchar_InstName", DbType.String, parameterInstName);
                db.AddInParameter(SaveCommandWrapper, "@varchar_Status", DbType.String, parameterStatus);
                db.AddInParameter(SaveCommandWrapper, "@varchar_RcvdDate", DbType.String, parameterDate);
                db.AddInParameter(SaveCommandWrapper, "@varchar_InfoSource", DbType.String, paramaterInfoSource);
                db.AddInParameter(SaveCommandWrapper, "@varchar_AccountNo", DbType.String, paramaterParticipantAccountNo);//AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to save the rollover data with account number 
                db.AddInParameter(SaveCommandWrapper, "@bitGenerateltr", DbType.Boolean, parameterGenerateLtr);//AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to save the rollover data with generate letter
                db.AddInParameter(SaveCommandWrapper, "@guiInstitutionId", DbType.String, parameterInstitutionId);//AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to save the rollover data with generate letter
                db.AddInParameter(SaveCommandWrapper, "@guiAddressID", DbType.String, parameterAddressId);//AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to save the rollover data with generate letter
                db.AddOutParameter(SaveCommandWrapper, "@guiNewRolloverID", DbType.String, 36); //AA:14.05.2014 BT:1051 - YRS 5.0-1618 - Added to return the institution id
                db.ExecuteNonQuery(SaveCommandWrapper, DBTransaction);
                strOutput = db.GetParameterValue(SaveCommandWrapper, "@guiNewRolloverID").ToString();
                DBTransaction.Commit();
                return strOutput;
            }
            catch
            {
                if (DBTransaction != null)
                    DBTransaction.Rollback();
                throw;
            }
            finally
            {
                if (DBconnectYRS != null)
                    DBconnectYRS.Close();

                DBTransaction = null;
                DBconnectYRS = null;
                db = null;
            }
        }


		public static string EditRolloverInfo(string parameterGuiId,string parameterInstName,string paramterdate)
		{
			
			Database db = null;
			DbCommand SaveCommandWrapper = null;
			String l_string_output;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SaveCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_EditParticipantRollover");
				
				if (SaveCommandWrapper == null) return null;

                db.AddInParameter(SaveCommandWrapper, "@varchar_guid", DbType.String, parameterGuiId);

                db.AddInParameter(SaveCommandWrapper, "@varchar_InstName", DbType.String, parameterInstName);

                db.AddInParameter(SaveCommandWrapper, "@varchar_RcvdDate", DbType.String, paramterdate);
                db.AddOutParameter(SaveCommandWrapper, "@varchar_Output", DbType.String, 1);

				db.ExecuteNonQuery(SaveCommandWrapper);
                l_string_output = db.GetParameterValue(SaveCommandWrapper, "@varchar_Output").ToString();
				return l_string_output;
				
			
				
				
			}
			catch 
			{
				throw;
			}
		}


		public static DataSet GetActiveEmpEvent(string parameterPersId)
		{
			DataSet l_dataset_dsLookUpActiveEmpEvent = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_GetActiveEmpEvent");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_gui_UniqueId",DbType.String,parameterPersId);
				
			
				l_dataset_dsLookUpActiveEmpEvent = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpActiveEmpEvent,"ActiveEmpEventInfo");
			
				return l_dataset_dsLookUpActiveEmpEvent;
			}
			catch 
			{
				throw;
			}
		}
		//BT-1028,YRS 5.0-1577: Add new field to atsRollovers
		public static DataSet GetInfoSource()
		{
			DataSet l_dataset_dsInfoSource = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");

				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_GetInfoSource");
				if (LookUpCommandWrapper == null) return null;
							
				l_dataset_dsInfoSource = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsInfoSource, "InfoSource");

				return l_dataset_dsInfoSource;
			}
			catch
			{
				throw;
			}
		}

        //Start:Anudeep:15.05.2014 BT-1051:YRS 5.0-1618 :Added new functions to get institutions and cancel rollover
        public static DataSet GetRolloverInstitutions()
        {
            DataSet dsRolloverInstitutions = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_GetInstitutions");
                if (LookUpCommandWrapper == null) return null;

                dsRolloverInstitutions = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsRolloverInstitutions, "Institutions");

                return dsRolloverInstitutions;
            }
            catch
            {
                throw;
            }
        }

        public static string CancelRolloverRequest(string paramterRolloverId)
        {
            String strOutput = string.Empty;
            Database db = null;
            DbCommand CancelCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                CancelCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_CancelRequest");
                if (CancelCommandWrapper == null) return null;
                db.AddInParameter(CancelCommandWrapper, "@guiRolloverId", DbType.String, paramterRolloverId);
                db.AddOutParameter(CancelCommandWrapper, "@varchar_Output", DbType.String, 1);
                db.ExecuteNonQuery(CancelCommandWrapper);
                strOutput = db.GetParameterValue(CancelCommandWrapper, "@varchar_Output").ToString();
                return strOutput;
                
            }
            catch
            {
                throw;
            }
        }

        private static int GetRefundRolloverInstitutionID(string paramRolloverInstitutionName, out string outpara_RolloverInstitutionUniqueID, DbTransaction DbTransaction, Database db)
        {
            int int_returnStatus;
            int_returnStatus = 1;
            DbCommand CommandWrapperGetRolloverInstitutionID = null;
            outpara_RolloverInstitutionUniqueID = "";
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return -1;

                CommandWrapperGetRolloverInstitutionID = db.GetStoredProcCommand("yrs_usp_BS_Get_RefundRolloverInstitutionID");

                if (CommandWrapperGetRolloverInstitutionID == null) return -1;

                db.AddInParameter(CommandWrapperGetRolloverInstitutionID, "@varchar_RolloverInstitutionName", DbType.String, paramRolloverInstitutionName);
                db.AddOutParameter(CommandWrapperGetRolloverInstitutionID, "@varchar_RolloverInstitutionUniqueID", DbType.String, 1000);

                db.ExecuteNonQuery(CommandWrapperGetRolloverInstitutionID, DbTransaction);
                outpara_RolloverInstitutionUniqueID = Convert.ToString(db.GetParameterValue(CommandWrapperGetRolloverInstitutionID, "@varchar_RolloverInstitutionUniqueID"));

                return int_returnStatus;


            }

            catch
            {
                throw;
            }
        }

        //End:Anudeep:15.05.2014 BT-1051:YRS 5.0-1618 :Added new functions to get institutions and cancel rollover

        //Start - Manthan Rajguru | 2016.08.22 | YRS-AT-2980 | Added method to get rollin institution name and address
        public static DataTable GetInstitutionNameAndAddress(string strRollInID)
        {
            DataSet dsRollInDetails = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_RollIn_GetInstitutionNameAndAddress");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varRollInID", DbType.String, strRollInID);


                dsRollInDetails = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, dsRollInDetails, "RollInDetails");
                return dsRollInDetails.Tables["RollInDetails"];
            }
            catch
            {
                throw;
            }
        }
        //End - Manthan Rajguru | 2016.08.22 | YRS-AT-2980 | Added method to get rollin institution name and address
    }
}
