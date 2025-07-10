//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	VoidandTransferAnnuityDAClass.cs
// Author Name		:	Sanjay Rawat
// Employee ID		:	51193
// Email	    	:	sanjay.singh@3i-infotech.com
// Contact No		:	8637
// Creation Time	:	16/09/2013
// Description	    :	DataAcess Class for Void and Transfer Annuity
//*******************************************************************************
//Modified By        Date            Description
//*******************************************************************************
//Sanjay R.          2014.02.06      YRS 5.0-1328 : Get Output parameter value for validation..
//Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace YMCARET.YmcaDataAccessObject
{
	
	public sealed class VoidAndTransferAnnuityDAClass
	{
        private VoidAndTransferAnnuityDAClass()
		{
			
		}

        #region InsertPersDtls
        //		'***************************************************************************************************//
        //		'Class Name                : VoidandTransferAnnuityDAClass               Used In     : YMCAUI       //
        //		'Created By                : Sanjay Singh                                Modified On :				//
        //		'Modified By               :                                                                        //
        //		'Modify Reason             :                                                                        //
        //		'Constructor Description   :                                                                        //
        //		'Event Description         :This event will return deceased retiree(s) based on selected criteria   //
        //		'***************************************************************************************************//
        
        public static DataSet LookUpPerson(string paramStrSSNo, string paramStrFundNo, string paramStrLastName, string paramStrFirstName, string paramStrFormName)
        {
            DataSet dsLookUpPersons = null;
            Database db = null;
            DbCommand commandLookUpPersons = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandLookUpPersons = db.GetStoredProcCommand("yrs_usp_VTA_FindPerson");

                if (commandLookUpPersons == null) return null;
                              
                commandLookUpPersons.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
                dsLookUpPersons = new DataSet();
                db.AddInParameter(commandLookUpPersons, "@varchar_SSN", DbType.String, paramStrSSNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_FundIdNo", DbType.String, paramStrFundNo);
                db.AddInParameter(commandLookUpPersons, "@varchar_LName", DbType.String, paramStrLastName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FName", DbType.String, paramStrFirstName);
                db.AddInParameter(commandLookUpPersons, "@varchar_FormName", DbType.String, paramStrFormName);               
                db.LoadDataSet(commandLookUpPersons, dsLookUpPersons, "Persons");
                return dsLookUpPersons;
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #region InsertPersDtls
        //		'***************************************************************************************************//
        //		'Class Name                : VoidandTransferAnnuityDAClass               Used In     : YMCAUI       //
        //		'Created By                : Sanjay Singh                                Modified On :				//
        //		'Modified By               :                                                                        //
        //		'Modify Reason             :                                                                        //
        //		'Constructor Description   :                                                                        //
        //		'Event Description         :This event will insert the Payee details in the atsperss table   //
        //		'***************************************************************************************************//
        public static void SavePayeeDtls(Database db, DbTransaction dbTransact, string paramStrPersId, string paramStrSSNo, string paramStrLastName, string paramStrFirstName, string paramStrMiddleName, string paramStrSalutation, string paramStrSuffix, string paramStrBirthdate, string paramStrMaritalCode, string paramStrEmailId, string paramStrPhoneNo)
        {
            DbCommand insertCommandWrapper = null;            
          
            try
            {   
                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_VTA_InsertPersDtls");
                db.AddInParameter(insertCommandWrapper, "@varchar_UniqueId", DbType.String, paramStrPersId);
                db.AddInParameter(insertCommandWrapper, "@varchar_SSNO", DbType.String, paramStrSSNo);
                db.AddInParameter(insertCommandWrapper, "@varchar_LastName", DbType.String, paramStrLastName);
                db.AddInParameter(insertCommandWrapper, "@varchar_FirstName", DbType.String, paramStrFirstName);
                db.AddInParameter(insertCommandWrapper, "@varchar_MiddleName", DbType.String, paramStrMiddleName);
                db.AddInParameter(insertCommandWrapper, "@varchar_SalutationCode", DbType.String, paramStrSalutation);
                db.AddInParameter(insertCommandWrapper, "@varchar_SuffixTitle", DbType.String, paramStrSuffix);
                db.AddInParameter(insertCommandWrapper, "@varchar_BirthDate", DbType.DateTime, paramStrBirthdate);
                db.AddInParameter(insertCommandWrapper, "@chvMaritalCode", DbType.String, paramStrMaritalCode);
                db.AddInParameter(insertCommandWrapper, "@varchar_EMail", DbType.String, paramStrEmailId );
                db.AddInParameter(insertCommandWrapper, "@varchar_PhoneNo", DbType.String, paramStrPhoneNo);
                db.ExecuteNonQuery(insertCommandWrapper, dbTransact);
                                          
            }
            catch
            {              
                throw;                            
            }
            
        }
        #endregion


        #region Check SSN
        //		'***************************************************************************************************//
        //		'Class Name                : VoidandTransferAnnuityDAClass               Used In     : YMCAUI       //
        //		'Created By                : Sanjay Singh                                Modified On :				//
        //		'Modified By               :                                                                        //
        //		'Modify Reason             :                                                                        //
        //		'Constructor Description   :                                                                        //
        //		'Event Description         :This event will check for existing SSN in the atsperss table   //
        //		'***************************************************************************************************//
        public static int CheckForExistingSSN(string paramStrSSNo)
        {
            DbCommand insertCommandWrapper = null;
            Database db = null;
            int SSNExist = 0;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_BI_LookUp_ExistingSSNo");
                db.AddInParameter(insertCommandWrapper, "@char_SSNo", DbType.String, paramStrSSNo);
                db.ExecuteScalar(insertCommandWrapper);
                object l_object= db.ExecuteScalar(insertCommandWrapper);
					if (l_object.GetType().ToString() !="System.DBNull"  )
                    {
						SSNExist=Convert.ToInt16 (l_object); 
					}				

				return SSNExist;
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region Get unvoided and unpaid Disbursements
        //		'***************************************************************************************************//
        //		'Class Name                : VoidandTransferAnnuityDAClass               Used In     : YMCAUI       //
        //		'Created By                : Sanjay Singh                                Modified On :				//
        //		'Modified By               :                                                                        //
        //		'Modify Reason             :                                                                        //
        //		'Constructor Description   :                                                                        //
        //		'Event Description         : This event will return unvoided and unpaid Disbursements               //
        //		'***************************************************************************************************//
        public static DataSet GetDisbursementsByPersId(string paramStrPersId)
        {
            DataSet l_dataset_dsGetDisbursementsByPersId = null;
            Database db = null;
            DbCommand LookUpCommandWrapper = null;
            l_dataset_dsGetDisbursementsByPersId = new DataSet();

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_VTA_GetDisbursementsByPersID");

                if (LookUpCommandWrapper == null) return null;

                db.AddInParameter(LookUpCommandWrapper, "@varchar_PersId", DbType.String, paramStrPersId);               
                              
                LookUpCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsGetDisbursementsByPersId, "DisbursementsByPersId");
                System.AppDomain.CurrentDomain.SetData("dsDisbursementsByPersId", l_dataset_dsGetDisbursementsByPersId);
                return l_dataset_dsGetDisbursementsByPersId;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        
        #region Void and Transfer annuity disbursement
        //		'***************************************************************************************************//
        //		'Class Name                : VoidandTransferAnnuityDAClass               Used In     : YMCAUI       //
        //		'Created By                : Sanjay Singh                                Modified On :				//
        //		'Modified By               :                                                                        //
        //		'Modify Reason             :                                                                        //
        //		'Constructor Description   :                                                                        //
        //		'Event Description         : This event will Void and Transfer annuity disbursement                 //
        //		'***************************************************************************************************//
        public static string VoidandTransferAnnuityDisbursements(Database db,DbTransaction dbTransact,List<Dictionary<string, string>> paramLstDisbursementIDs, string paramStrPersId,string ParamStrAddrsId)
        {
                      
            string l_string_Output = string.Empty;
            string strValidation = string.Empty;
            DbCommand dbVoidandTransherCommandWrapper = null;           
            string parameterStrDisbId = string.Empty;             
            try
            {
                foreach (Dictionary<string, string> item in paramLstDisbursementIDs)
                {
                    parameterStrDisbId = item["DisbId"];
                    
                    dbVoidandTransherCommandWrapper = db.GetStoredProcCommand("yrs_usp_VTA_VoidAndTransferAnnuity");
                    //if (dbVoidandTransherCommandWrapper == null) return null;
                    db.AddInParameter(dbVoidandTransherCommandWrapper, "@varchar_DisbId", DbType.String, parameterStrDisbId);
                    db.AddInParameter(dbVoidandTransherCommandWrapper, "@varchar_PersId", DbType.String, paramStrPersId);
                    db.AddInParameter(dbVoidandTransherCommandWrapper, "@varchar_AddrId", DbType.String, ParamStrAddrsId);
                    db.AddOutParameter(dbVoidandTransherCommandWrapper, "@varchar_Output", DbType.String, 3000);

                    db.ExecuteNonQuery(dbVoidandTransherCommandWrapper, dbTransact);
                    //Start:SR:2014.02.06 : YRS 5.0-1328 : Get Output parameter value for validation.
                    strValidation = Convert.ToString(db.GetParameterValue(dbVoidandTransherCommandWrapper, "@varchar_Output"));

                    if (strValidation != "")
                    {
                        l_string_Output += strValidation + " <br/>";
                    }
                }

                if (!string.IsNullOrEmpty(l_string_Output.Trim()))
                {
                    l_string_Output = l_string_Output.Substring(0, l_string_Output.Length - 5);
                }
                //End:SR:2014.02.06 : YRS 5.0-1328 : Get Output parameter value for validation.
                return l_string_Output;
            }
            catch
            {
                throw;
            }
        }

        #endregion

     #region SaveandTransferAnnuity
        //		'*******************************************************************************************************************//
        //		'Class Name                : VoidandTransferAnnuityDAClass               Used In     : YMCAUI                       //
        //		'Created By                : Sanjay Singh                                Modified On :				                //
        //		'Modified By               :                                                                                        //
        //		'Modify Reason             :                                                                                        //
        //		'Constructor Description   :                                                                                        //
        //		'Event Description         :This event will save and Transfer annuity to new/existing Payee in the atsperss table   //
        //		'*******************************************************************************************************************//
        public static string SaveandTransferAnnuity(string paramPersId, string paramSSNo, string paramLastName, string paramFirstName, string paramMiddleName, string paramSalutation, string paramSuffix, string paramBirthdate, string paramMaritalCode, List<Dictionary<string, string>> lstDisbursementIDs, string paramAddressId, Boolean paramPayeeExist, string paramStrEmailId, string paramStrPhoneNo)
        {
            Database db = null;
            DbConnection dbConnect = null;
            DbTransaction dbTransact = null;
            string strOutput = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                dbConnect = db.CreateConnection();
                dbConnect.Open();
                dbTransact = dbConnect.BeginTransaction(IsolationLevel.ReadUncommitted);      

                if (paramPayeeExist == false)
                {
                    SavePayeeDtls(db, dbTransact,paramPersId, paramSSNo, paramLastName, paramFirstName, paramMiddleName, paramSalutation, paramSuffix, paramBirthdate, paramMaritalCode,paramStrEmailId,paramStrPhoneNo);                
                }

                strOutput = VoidandTransferAnnuityDisbursements(db, dbTransact, lstDisbursementIDs, paramPersId, paramAddressId);
                              
                dbTransact.Commit();
                return strOutput;
            }
            catch
            {
                if (dbTransact != null)
                    dbTransact.Rollback();
                throw;
            }
            finally
            {
                if (dbConnect != null)
                    dbConnect.Close();
                dbTransact = null;
                dbConnect = null;
                db = null;
            }
        }

    #endregion


    }
}
