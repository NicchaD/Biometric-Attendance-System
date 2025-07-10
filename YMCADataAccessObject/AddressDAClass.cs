//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	AddressDAClass.cs
// Author Name		:	Anudeep Adusumilli
// Employee ID		:	56556
// Email			:	
// Contact No		:	
// Creation Date	:	5/30/2013
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	To Retireve or to save the address
//*******************************************************************************
/*
*********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By            Date			Description
'********************************************************************************************************************************
 Anudeep                24.09.2013      BT-1501:YRS 5.0-1745:Capture Beneficiary addresses
 Anudeep                4.07.2014       BT-1051:YRS 5.0-1618 :Enhancements to Roll In process
 Manthan Rajguru        2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
   public sealed class AddressDAClass
    {
       /// <summary>
        /// To get the address with respect to entityId 
       /// </summary>
       /// <param name="strEntityID"></param>
       /// <param name="strEntityCode"></param>
       /// <returns></returns>
       public static DataSet GetAddressByEntity(string strEntityID, string strEntityCode)
        {
            DataSet l_dataset_dsLookUpAddressInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_GetByEntityId");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_guiEntityId", DbType.String, strEntityID);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_chvEntityCode", DbType.String, strEntityCode);


                l_dataset_dsLookUpAddressInfo = new DataSet();
                l_TableNames = new string[] { "Address" };
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAddressInfo, l_TableNames);
                return l_dataset_dsLookUpAddressInfo;
            }
            catch
            {
                throw;
            }
        }
       /// <summary>
       /// To save the address 
       /// </summary>
       /// <param name="parameterdsAddress"></param>
        public static string SaveAddress(DataSet parameterdsAddress)
        {
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            string strNewAddressID = string.Empty;
           try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
               
               if (db == null) return ""; 
               
               DBconnectYRS = db.CreateConnection();
               
               DBconnectYRS.Open();
               
               DBTransaction = DBconnectYRS.BeginTransaction(IsolationLevel.ReadUncommitted);

               strNewAddressID = SaveAddress(parameterdsAddress, DBTransaction, db);

                
               DBTransaction.Commit();
               return strNewAddressID;
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

       /// <summary>
        /// To save the address in the Transaction 
       /// </summary>
       /// <param name="parameterdsAddress"></param>
       /// <param name="dbTransaction"></param>
       /// <param name="db"></param>
       /// <returns></returns>
        public static string SaveAddress(DataSet parameterdsAddress,DbTransaction dbTransaction,Database db)
        {

            DbCommand AddCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand DeleteCommandWrapper = null;
            string strNewAddressID = string.Empty;

            try
            {                

                if (db == null) return null;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_SaveAddress");

                if (AddCommandWrapper == null) return null;

                db.AddInParameter(AddCommandWrapper, "@varchar_guiUniqueId", DbType.String, "UniqueId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_guiEntityId", DbType.String, "guiEntityId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address1", DbType.String, "addr1", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address2", DbType.String, "addr2", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address3", DbType.String, "addr3", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_City", DbType.String, "city", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_StateType", DbType.String, "state", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Zip", DbType.String, "zipCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Active", DbType.Boolean, "isActive", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Primary", DbType.Boolean, "isPrimary", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@date_EffectiveDate", DbType.String, "effectiveDate", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_BadAddress", DbType.Boolean, "isBadAddress", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_EntityCode", DbType.String, "entityCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_AddrCode", DbType.String, "addrCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Note", DbType.String, "Note", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bitImportant", DbType.String, "bitImportant", DataRowVersion.Current);
                db.AddOutParameter(AddCommandWrapper, "@Address_NewId", DbType.String, 100);

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
                
                db.UpdateDataSet(parameterdsAddress, "Address", AddCommandWrapper, UpdateCommandWrapper, DeleteCommandWrapper, dbTransaction);
                strNewAddressID = db.GetParameterValue(AddCommandWrapper, "@Address_NewId").ToString();

                return strNewAddressID;
            }
            catch
            {
                throw;
            }
        }
       /// <summary>
        ///  To Get the address by UniqueId
       /// </summary>
       /// <param name="strAddressID"></param>
       /// <returns></returns>
        public static DataSet GetAddressByID(string strAddressID)
        {
            DataSet l_dataset_dsLookUpAddressInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_GetbyAddressID");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_guiPersId", DbType.String, strAddressID);
                
                l_dataset_dsLookUpAddressInfo = new DataSet();
                l_TableNames = new string[] { "Address" };
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAddressInfo, l_TableNames);
                return l_dataset_dsLookUpAddressInfo;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetBeneficiariesAddress(string strPersID, string strSSNO, string strFirstname, string strLastname)
        {
            DataSet l_dataset_dsLookUpAddressInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_GetBeneficiaryAddress");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@paramSSNO", DbType.String, strSSNO);
                db.AddInParameter(LookUpCommandWrapper, "@paramPersId", DbType.String, strPersID);
                db.AddInParameter(LookUpCommandWrapper, "@paramFirstName", DbType.String, strFirstname);
                db.AddInParameter(LookUpCommandWrapper, "@paramLastName", DbType.String, strLastname);


                l_dataset_dsLookUpAddressInfo = new DataSet();
                l_TableNames = new string[] { "Address" };
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAddressInfo, l_TableNames);
                return l_dataset_dsLookUpAddressInfo;
            }
            catch
            {
                throw;
            }
        }
       /// <summary>
        ///  To Get the address of beneficiaries For a participant
       /// </summary>
       /// <param name="strPersID"></param>
       /// <param name="strEntityCode"></param>
       /// <returns></returns>
        public static DataSet GetAddessForBeneficiariesOfPerson(string strPersID, string strEntityCode)
        {
            DataSet l_dataset_dsLookUpAddressInfo = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_GetForBeneficiariesOfPerson");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_guiPersId", DbType.String, strPersID);
                db.AddInParameter(LookUpCommandWrapper, "@varchar_chvEntityCode", DbType.String, strEntityCode);


                l_dataset_dsLookUpAddressInfo = new DataSet();
                l_TableNames = new string[] { "Address" };
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsLookUpAddressInfo, l_TableNames);
                return l_dataset_dsLookUpAddressInfo;
            }
            catch
            {
                throw;
            }
        }

        public static void SaveAddressOfBeneficiariesByPerson(DataSet dsAddress)
        {
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return;

                DBconnectYRS = db.CreateConnection();

                DBconnectYRS.Open();

                DBTransaction = DBconnectYRS.BeginTransaction(IsolationLevel.ReadUncommitted);

                SaveAddressOfBeneficiariesByPerson(dsAddress, DBTransaction, db);

                DBTransaction.Commit();
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
        public static void SaveAddressOfBeneficiariesByPerson(DataSet dsAddress, DbTransaction DBTransaction, Database db)
        {
            DbCommand AddCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand DeleteCommandWrapper = null;
            string strNewAddressID = string.Empty;
            
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return;

                if (DBTransaction == null) return;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_SaveBeneficiaryAddress");

                if (AddCommandWrapper == null) return;

                db.AddInParameter(AddCommandWrapper, "@varGuiAddressId", DbType.String, "UniqueId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varGuiBeneficiaryMappingId", DbType.String, "guiEntityId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address1", DbType.String, "addr1", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address2", DbType.String, "addr2", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address3", DbType.String, "addr3", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_City", DbType.String, "city", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_StateType", DbType.String, "state", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Zip", DbType.String, "zipCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@date_EffectiveDate", DbType.String, "effectiveDate", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_BadAddress", DbType.Boolean, "isBadAddress", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_EntityCode", DbType.String, "entityCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_AddrCode", DbType.String, "addrCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Note", DbType.String, "Note", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bitImportant", DbType.Boolean, "bitImportant", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varSSNO", DbType.String, "BenSSNo", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@VarOldSSNo", DbType.String, "oldBenSSNo", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varGuiPersId", DbType.String, "PersID", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varBeneficiaryId", DbType.String, "BeneID", DataRowVersion.Current);
                

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_SaveBeneficiaryAddress");

                if (UpdateCommandWrapper == null) return;

                db.AddInParameter(UpdateCommandWrapper, "@varGuiAddressId", DbType.String, "UniqueId", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varGuiBeneficiaryMappingId", DbType.String, "guiEntityId", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Address1", DbType.String, "addr1", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Address2", DbType.String, "addr2", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Address3", DbType.String, "addr3", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_City", DbType.String, "city", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_StateType", DbType.String, "state", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Zip", DbType.String, "zipCode", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@date_EffectiveDate", DbType.String, "effectiveDate", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bit_BadAddress", DbType.Boolean, "isBadAddress", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_EntityCode", DbType.String, "entityCode", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_AddrCode", DbType.String, "addrCode", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Note", DbType.String, "Note", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@bitImportant", DbType.Boolean, "bitImportant", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varSSNO", DbType.String, "BenSSNo", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@VarOldSSNo", DbType.String, "oldBenSSNo", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varGuiPersId", DbType.String, "PersID", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@varBeneficiaryId", DbType.String, "BeneID", DataRowVersion.Current);

                UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.UpdateDataSet(dsAddress, "Address", AddCommandWrapper, UpdateCommandWrapper, DeleteCommandWrapper, DBTransaction);

            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetAddressForYMCA(string guiYmcaID)
        {
            DataSet dsSearchAddressInformation = null;
            Database db = null;
            DbCommand CommandSearchAddressInformation = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                CommandSearchAddressInformation = db.GetStoredProcCommand("yrs_usp_Addrs_SearchYmcaGeneralAddressInformation");
                if (CommandSearchAddressInformation == null) return null;

                db.AddInParameter(CommandSearchAddressInformation, "@UniqueIdentifier_GuiUniqueId", DbType.String, guiYmcaID);

                dsSearchAddressInformation = new DataSet();
                dsSearchAddressInformation.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(CommandSearchAddressInformation, dsSearchAddressInformation, "Address Information");

                return dsSearchAddressInformation;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetCountryList()
        {
            DataSet dsLookUpAddressCountry = null;
            Database db = null;
            DbCommand commandLookUpAddressCountry = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                commandLookUpAddressCountry = db.GetStoredProcCommand("yrs_usp_AMY_SearchYMCAAddressCountry");
                if (commandLookUpAddressCountry == null) return null;
                dsLookUpAddressCountry = new DataSet();
                dsLookUpAddressCountry.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(commandLookUpAddressCountry, dsLookUpAddressCountry, "Country");
                return dsLookUpAddressCountry;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetState()
        {
            DataSet dsGetStates = null;
            Database db = null;
            DbCommand CommandGetStates = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                CommandGetStates = db.GetStoredProcCommand("dbo.yrs_usp_AMY_GetStates");
                if (CommandGetStates == null) return null;

                dsGetStates = new DataSet();
                dsGetStates.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(CommandGetStates, dsGetStates, "YMCA States");

                return dsGetStates;
            }
            catch
            {
                throw;
            }
        }
       //AA:2013.09.24:BT-1501:Handling the getting reason from address class and getting reasons for beneficiary address change
        public static DataSet GetAddressNotesReasonSource(string parameterNotesSourceReason,string parameterNotesSourceFor)
        {
            DataSet dsAddNoteReasonSources = null;
            Database db = null;
            DbCommand CommandAddNoteReasonSources = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                CommandAddNoteReasonSources = db.GetStoredProcCommand("dbo.yrs_usp_AMY_GetAddressNotesReasonSource");
                if (CommandAddNoteReasonSources == null) return null;

                db.AddInParameter(CommandAddNoteReasonSources, "@Varchar_NotesType", DbType.String, parameterNotesSourceReason);
                //AA:2013.09.24:BT-1501:Handling the getting reason from address class and getting reasons for beneficiary address change
                db.AddInParameter(CommandAddNoteReasonSources, "@Varchar_NotesFor", DbType.String, parameterNotesSourceFor);
                dsAddNoteReasonSources = new DataSet();
                dsAddNoteReasonSources.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(CommandAddNoteReasonSources, dsAddNoteReasonSources, "NotesReasonSources");

                return dsAddNoteReasonSources;
            }
            catch
            {
                throw;
            }
        }
        public static void SaveAddressForYMCA(DataSet ds)
        {
            Database db = null;
            DbCommand insertCommand = null;
            DbCommand updateCommand = null;
            DbCommand deleteCommand = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                DBconnectYRS = db.CreateConnection();
                DBconnectYRS.Open();
                DBTransaction = DBconnectYRS.BeginTransaction();

                insertCommand = db.GetStoredProcCommand("yrs_usp_Addrs_InsertYmcaGeneralAddressInformationAdd");
                updateCommand = db.GetStoredProcCommand("yrs_usp_Addrs_UpdateYmcaGeneralAddressInformationAdd");

                db.AddOutParameter(insertCommand, "@uniqueIdentifier_GuiUniqueID", DbType.String, 1000);
                db.AddInParameter(insertCommand, "@uniqueIdentifier_GuiEntityId", DbType.String, "guiEntityID", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_Type", DbType.String, "addrCode", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@bit_Primary", DbType.Boolean, "isPrimary", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@bit_Active", DbType.Boolean, "isActive", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@datetime_EffDate", DbType.DateTime, "effectiveDate", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_addr1", DbType.String, "addr1", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_addr2", DbType.String, "addr2", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_addr3", DbType.String, "addr3", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_city", DbType.String, "city", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@char_state", DbType.String, "state", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@char_zip", DbType.String, "zipCode", DataRowVersion.Current);
                db.AddInParameter(insertCommand, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
                insertCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                db.AddInParameter(updateCommand, "@uniqueIdentifier_GuiUniqueID", DbType.String, "UniqueID", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@uniqueIdentifier_GuiEntityId", DbType.String, "guiEntityID", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_Type", DbType.String, "addrCode", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@bit_Primary", DbType.Boolean, "isPrimary", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@bit_Active", DbType.Boolean, "isActive", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@datetime_EffDate", DbType.DateTime, "effectiveDate", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_addr1", DbType.String, "addr1", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_addr2", DbType.String, "addr2", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_addr3", DbType.String, "addr3", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_city", DbType.String, "city", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_state", DbType.String, "state", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@char_zip", DbType.String, "zipCode", DataRowVersion.Current);
                db.AddInParameter(updateCommand, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
                updateCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);

                db.UpdateDataSet(ds, ds.Tables[0].TableName, insertCommand, updateCommand, deleteCommand, DBTransaction);

                //db.ExecuteNonQuery(CommandUpdateInsert,pDBTransaction);
                DBTransaction.Commit();
                string ymcaId = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ymcaId = ds.Tables[0].Rows[0]["guiEntityID"].ToString();
                }
                DataSet refreshedData = GetAddressForYMCA(ymcaId);
                if (refreshedData != null && refreshedData.Tables.Count > 0)
                {
                    DataTable dt = refreshedData.Tables[0];
                    refreshedData.Tables.Remove(dt);
                    ds.Tables.RemoveAt(0);
                    ds.Tables.Add(dt);
                }
            }
            catch
            {
                if (DBTransaction != null) DBTransaction.Rollback();
                throw;
            }
            finally
            {
                if (DBTransaction != null) DBTransaction.Dispose();
                if (DBconnectYRS != null)
                {
                    if (DBconnectYRS.State != ConnectionState.Closed) DBconnectYRS.Close();
                    DBconnectYRS.Dispose();
                    DBconnectYRS = null;
                }
                insertCommand = null;
                updateCommand = null;
                deleteCommand = null;
                db = null;
            }
        }

        //Start: Anudeep:04.07.2014:BT-1051:YRS 5.0-1618 : Added code to save the rolloverinstitution address
        /// <summary>
        /// To save the insitution address 
        /// </summary>
        /// <param name="parameterdsAddress"></param>
        public static string SaveInstitutionAddress(DataSet parameterdsAddress)
        {
            Database db = null;
            DbTransaction DBTransaction = null;
            DbConnection DBconnectYRS = null;
            string strNewAddressID = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return "";

                DBconnectYRS = db.CreateConnection();

                DBconnectYRS.Open();

                DBTransaction = DBconnectYRS.BeginTransaction(IsolationLevel.ReadUncommitted);

                strNewAddressID = SaveInstitutionAddress(parameterdsAddress, DBTransaction, db);


                DBTransaction.Commit();
                return strNewAddressID;
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

        /// <summary>
        /// To save the address in the Transaction 
        /// </summary>
        /// <param name="parameterdsAddress"></param>
        /// <param name="dbTransaction"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string SaveInstitutionAddress(DataSet parameterdsAddress, DbTransaction dbTransaction, Database db)
        {

            DbCommand AddCommandWrapper = null;
            DbCommand UpdateCommandWrapper = null;
            DbCommand DeleteCommandWrapper = null;
            string strNewAddressID = string.Empty;

            try
            {

                if (db == null) return null;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_Addr_SaveInstitutionAddress");

                if (AddCommandWrapper == null) return null;

                db.AddInParameter(AddCommandWrapper, "@varchar_guiUniqueId", DbType.String, "UniqueId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_guiEntityId", DbType.String, "guiEntityId", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address1", DbType.String, "addr1", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address2", DbType.String, "addr2", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Address3", DbType.String, "addr3", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_City", DbType.String, "city", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_StateType", DbType.String, "state", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Zip", DbType.String, "zipCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Country", DbType.String, "country", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Active", DbType.Boolean, "isActive", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Primary", DbType.Boolean, "isPrimary", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@date_EffectiveDate", DbType.String, "effectiveDate", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_BadAddress", DbType.Boolean, "isBadAddress", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_EntityCode", DbType.String, "entityCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_AddrCode", DbType.String, "addrCode", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_Note", DbType.String, "Note", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@bit_Important", DbType.String, "bitImportant", DataRowVersion.Current);
                db.AddOutParameter(AddCommandWrapper, "@Address_NewId", DbType.String, 100);

                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                db.UpdateDataSet(parameterdsAddress, "Address", AddCommandWrapper, UpdateCommandWrapper, DeleteCommandWrapper, dbTransaction);
                strNewAddressID = db.GetParameterValue(AddCommandWrapper, "@Address_NewId").ToString();

                return strNewAddressID;
            }
            catch
            {
                throw;
            }
        }
        //End: Anudeep:04.07.2014:BT-1051:YRS 5.0-1618 : Added code to save the rolloverinstitution address

    }
}
