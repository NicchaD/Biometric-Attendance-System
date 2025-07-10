//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	StateWithholdingDA.cs
// Author Name		:	Megha Lad
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	01/10/2019
// Description	    :	Data Access Class For State Withholding 
// Declared in Version : 20.7.0 | YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing
//**************************************************************************************************************
// MODIFICATION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using YMCAObjects;

namespace YMCARET.YmcaDataAccessObject
{
    public class StateWithholdingDAClass
    {
        /// <summary>
        ///Provides Applicable Matrix StateName wise for State Withholding Input Controls
        /// </summary>        
        /// <returns>List of Applicable Matrix for State Withholding Input Controls</returns>
        [CLSCompliant(false)]
        public static List<StateWithholdingInput> GetStateWiseInputList()
        {
            List<StateWithholdingInput> lstDataList;
            IDataReader dr = null;
            Database db;
            DbCommand cmd;
            StateWithholdingInput objSTWInput;
            try
            {
                lstDataList = new List<StateWithholdingInput>();
                db = DatabaseFactory.CreateDatabase("YRS");
                cmd = db.GetStoredProcCommand("yrs_usp_STW_GetStateWiseInputList");
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);                
                
                dr = db.ExecuteReader(cmd);
                if (null != dr)
                {
                    while (dr.Read())
                    {
                        objSTWInput = new StateWithholdingInput();
                        objSTWInput.chvStateCode = Convert.ToString(Convert.IsDBNull(dr["chvStateCode"]) ? string.Empty : dr["chvStateCode"]).Trim();
                        objSTWInput.chvStateName = Convert.ToString(Convert.IsDBNull(dr["chvStateName"]) ? string.Empty : dr["chvStateName"]).Trim();
                        objSTWInput.chvDisplayText = Convert.ToString(Convert.IsDBNull(dr["chvDisplayText"]) ? string.Empty : dr["chvDisplayText"]).Trim();
                        objSTWInput.bitAdditionalAmount = Convert.ToBoolean(dr["bitAdditionalAmount"]);
                        objSTWInput.bitDisbursementType = Convert.ToBoolean(dr["bitDisbursementType"]);
                        objSTWInput.bitFlatAmount = Convert.ToBoolean(dr["bitFlatAmount"]);
                        objSTWInput.bitMaritalStatusCode = Convert.ToBoolean(dr["bitMaritalStatusCode"]);
                        objSTWInput.bitNewYorkCityAmount = Convert.ToBoolean(dr["bitNewYorkCityAmount"]);
                        objSTWInput.bitPercentageofFederalWithholding = Convert.ToBoolean(dr["bitPercentageofFederalWithholding"]);
                        objSTWInput.bitNoOfExemption = Convert.ToBoolean(dr["bitNoOfExemption"]);
                        objSTWInput.bitStateTaxNotElected = Convert.ToBoolean(dr["bitStateTaxNotElected"]);
                        objSTWInput.bitYonkersAmount = Convert.ToBoolean(dr["bitYonkersAmount"]);
                        lstDataList.Add(objSTWInput);
                        objSTWInput = null;
                    }
                }
                return lstDataList;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Dispose();
                }
                lstDataList = null;
                objSTWInput = null;
                cmd = null;
                db = null;
            }
        }

        /// <summary>
        /// Saves person state wise tax details.
        /// </summary>
        /// <param name="data"></param>        
        /// <returns></returns>
         [CLSCompliant(false)]
        public static Boolean SavePersStateTaxdetails(YMCAObjects.StateWithholdingDetails objSTWPersonDetail, DbTransaction DBTransaction = null)
        {
            Database db = null;
            DbCommand cmd = null;            
            DbConnection connection = null;
            bool bitResult;
            try
            {
             
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return false;

                //Get a connection and Open it
                connection = db.CreateConnection();
                connection.Open();               

                cmd = db.GetStoredProcCommand("yrs_usp_STW_SavePersStateTaxdetails");
                if (cmd == null) return false;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@UNIQUEIDENTIFIER_guiPersID", DbType.String, objSTWPersonDetail.guiPersID);
                db.AddInParameter(cmd, "@VARCHAR_chvStateCode", DbType.String, objSTWPersonDetail.chvStateCode );
                db.AddInParameter(cmd, "@VARCHAR_chvDisbursementType", DbType.String, objSTWPersonDetail.chvDisbursementType );
                db.AddInParameter(cmd, "@BIT_bitStateTaxNotElected", DbType.Boolean , objSTWPersonDetail.bitStateTaxNotElected );
                db.AddInParameter(cmd, "@VARCHAR_chvMaritalStatusCode", DbType.String, objSTWPersonDetail.chvMaritalStatusCode );
                db.AddInParameter(cmd, "@INTEGER_intNoOfExemption", DbType.Int16 , objSTWPersonDetail.intNoOfExemption );
                db.AddInParameter(cmd, "@NUMERIC_numAdditionalAmount", DbType.Double, objSTWPersonDetail.numAdditionalAmount );
                db.AddInParameter(cmd, "@NUMERIC_numFlatAmount", DbType.Double, objSTWPersonDetail.numFlatAmount );
                db.AddInParameter(cmd, "@NUMERIC_numPercentageofFederalWithholding", DbType.Double, objSTWPersonDetail.numPercentageofFederalWithholding);
                db.AddInParameter(cmd, "@NUMERIC_numNewYorkCityAmount", DbType.Double, objSTWPersonDetail.numNewYorkCityAmount );
                db.AddInParameter(cmd, "@NUMERIC_numYonkersAmount", DbType.Double, objSTWPersonDetail.numYonkersAmount  );
                db.AddInParameter(cmd, "@NUMERIC_numComputedTaxAmount", DbType.Double, objSTWPersonDetail.numComputedTaxAmount);           
                db.AddInParameter(cmd, "@BIT_bitActive", DbType.Boolean , true );

                if (DBTransaction == null)                                  
                    bitResult = Convert.ToBoolean(db.ExecuteNonQuery(cmd));              
                else
                    bitResult = Convert.ToBoolean(db.ExecuteNonQuery(cmd, DBTransaction)); // Dat Saving from Annuity Puchase Screen
                      
                return bitResult;
            }
            catch
            {               
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }                
                connection = null;              
                cmd = null;
                db = null;
            }
        }

        /// <summary>
        /// Get details of state tax withholding details to gridview
        /// </summary>
        /// <param name="perssid"></param>       
        /// <returns></returns>
        [CLSCompliant(false)]
        public static List<StateWithholdingDetails> GetStateTaxDetails(string perssid)
        {
            List<StateWithholdingDetails> lstDataList;
            Database db;
            DbCommand cmd;
            StateWithholdingDetails objSTWPersonDetail;
            IDataReader dr = null;
            try
            {
                lstDataList = new List<StateWithholdingDetails>();
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_STW_GetStateTaxdetails");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                objSTWPersonDetail = new StateWithholdingDetails();
                db.AddInParameter(cmd, "@VARCHAR_guiPersID", DbType.String, perssid);
                dr = db.ExecuteReader(cmd);


                if (null != dr)
                {
                    while (dr.Read())
                    {
                        objSTWPersonDetail = new StateWithholdingDetails();

                        objSTWPersonDetail.chvStateCode = Convert.ToString(Convert.IsDBNull(dr["chvStateCode"]) ? string.Empty : dr["chvStateCode"]).Trim();
                        objSTWPersonDetail.guiPersID = Convert.ToString(Convert.IsDBNull(dr["guiPersID"]) ? string.Empty : dr["guiPersID"].ToString().ToUpper() ).Trim() ;
                        objSTWPersonDetail.chvDisbursementType = Convert.ToString(Convert.IsDBNull(dr["chvDisbursementType"]) ? string.Empty : dr["chvDisbursementType"]).Trim();
                        objSTWPersonDetail.bitStateTaxNotElected = Convert.ToBoolean(dr["bitStateTaxNotElected"]);
                       
                        objSTWPersonDetail.chvMaritalStatusCode = dr["chvMaritalStatusCode"].ToString();

                        if (Convert.IsDBNull(dr["chvMaritalStatusCode"]))
                            objSTWPersonDetail.chvMaritalStatusCode = null;
                        else
                            objSTWPersonDetail.chvMaritalStatusCode = dr["chvMaritalStatusCode"].ToString();

                        if (Convert.IsDBNull(dr["intNoOfExemption"]))
                            objSTWPersonDetail.intNoOfExemption = null;
                        else
                            objSTWPersonDetail.intNoOfExemption = Convert.ToInt32(dr["intNoOfExemption"]);

                        if (Convert.IsDBNull(dr["numAdditionalAmount"]))
                            objSTWPersonDetail.numAdditionalAmount = null;
                        else
                            objSTWPersonDetail.numAdditionalAmount = Convert.ToDouble(dr["numAdditionalAmount"]);

                        if (Convert.IsDBNull(dr["numFlatAmount"]))
                            objSTWPersonDetail.numFlatAmount = null;
                        else
                            objSTWPersonDetail.numFlatAmount = Convert.ToDouble(dr["numFlatAmount"]);

                        if (Convert.IsDBNull(dr["numPercentageofFederalWithholding"]))
                            objSTWPersonDetail.numPercentageofFederalWithholding = null;
                        else
                            objSTWPersonDetail.numPercentageofFederalWithholding = Convert.ToDouble(dr["numPercentageofFederalWithholding"]);

                        if (Convert.IsDBNull(dr["numNewYorkCityAmount"]))
                            objSTWPersonDetail.numNewYorkCityAmount = null;
                        else
                            objSTWPersonDetail.numNewYorkCityAmount = Convert.ToDouble(dr["numNewYorkCityAmount"]);

                        if (Convert.IsDBNull(dr["numYonkersAmount"]))
                            objSTWPersonDetail.numYonkersAmount = null;
                        else
                            objSTWPersonDetail.numYonkersAmount = Convert.ToDouble(dr["numYonkersAmount"]);

                        if (Convert.IsDBNull(dr["numComputedTaxAmount"]))
                            objSTWPersonDetail.numComputedTaxAmount = null;
                        else
                            objSTWPersonDetail.numComputedTaxAmount = Convert.ToDouble(dr["numComputedTaxAmount"]);
                       
                        objSTWPersonDetail.bitActive = Convert.ToBoolean(dr["bitActive"]);

                        lstDataList.Add(objSTWPersonDetail);
                        objSTWPersonDetail = null;
                    }
                }
                return lstDataList;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Dispose();
                }
                lstDataList = null;
                objSTWPersonDetail = null;
                cmd = null;
                db = null;
            }
        }

        /// <summary>
        ///  This will get person information to show on screen.
        /// </summary>
        /// <param name="perssid"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static StateWithholdingPersonDetails GetPersonDetails(string perssid)
        {
            StateWithholdingPersonDetails objSTWPersonDetail;
            Database db;
            DbCommand cmd;
            IDataReader dr = null;
            try
            {
                objSTWPersonDetail = new StateWithholdingPersonDetails();
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_STW_GetPersonStateTaxDetails");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                 
                db.AddInParameter(cmd, "@VARCHAR_guiPersID", DbType.String, perssid);
                dr = db.ExecuteReader(cmd);

                if (null != dr)
                {
                    while (dr.Read())
                    {
                        objSTWPersonDetail = new StateWithholdingPersonDetails();
                        objSTWPersonDetail.intFundNumber = Convert.ToInt32(Convert.IsDBNull(dr["intFundIdNo"]) ? null : dr["intFundIdNo"]);
                        objSTWPersonDetail.guiPersID = Convert.ToString(Convert.IsDBNull(dr["guiUniqueID"]) ? string.Empty : dr["guiUniqueID"]).Trim();
                        objSTWPersonDetail.chrStateCode = Convert.ToString(Convert.IsDBNull(dr["chrStateCode"]) ? string.Empty : dr["chrStateCode"]).Trim();
                        objSTWPersonDetail.chvStateName = Convert.ToString(Convert.IsDBNull(dr["chvStateName"]) ? string.Empty : dr["chvStateName"]).Trim();
                        objSTWPersonDetail.firstName = Convert.ToString(Convert.IsDBNull(dr["chvFirstName"]) ? string.Empty : dr["chvFirstName"]).Trim();
                        objSTWPersonDetail.lastName = Convert.ToString(Convert.IsDBNull(dr["chvLastName"]) ? string.Empty : dr["chvLastName"]).Trim();
                        if (Convert.IsDBNull(dr["numCurrentAnnuity"]))
                            objSTWPersonDetail.numCurrentAnnuity  = null;
                        else
                            objSTWPersonDetail.numCurrentAnnuity = Convert.ToDouble(dr["numCurrentAnnuity"]);

                    }
                }
                return objSTWPersonDetail;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Dispose();
                }
                objSTWPersonDetail = null;
                cmd = null;
                db = null;
            }
        }
    }
}
