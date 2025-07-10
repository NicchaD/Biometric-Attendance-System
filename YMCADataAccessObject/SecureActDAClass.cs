//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	SecureActDAClass.cs
// Author Name		:	Megha Lad
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	02/10/2020
// Description	    :	Data Access Class For Secure Act 
// Declared in Version : 20.8.0| YRS-AT-4769 - YRS enhancement-new Secure Act rule for Annuity Estimate YRS changes (TrackIt-  41078)
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
namespace YMCARET.YmcaDataAccessObject
{
    public class SecureActDAClass
    {
        public SecureActDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        /// <summary>
        /// Check Secure act New rule is applicable to not.
        /// </summary>
        /// <param name="data"></param>        
        /// <returns></returns>        
        public static Boolean IsSecureActApplicable(DateTime perssBirthDate,DateTime beneficiaryBirthDate,string beneficiaryRelationShipCode, DateTime cutOffDate, bool chronicallyIll)
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

                cmd = db.GetStoredProcCommand("yrs_usp_IsSecureActApplicable");
                if (cmd == null) return false;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@DATETIME_PerssBirthDate", DbType.String, perssBirthDate);
                db.AddInParameter(cmd, "@DATETIME_BeneficiaryBirthDate", DbType.String, beneficiaryBirthDate);
                db.AddInParameter(cmd, "@VARCHAR_BeneficiaryRelationShipCode", DbType.String, beneficiaryRelationShipCode.Trim());
                db.AddInParameter(cmd, "@DATETIME_CutoffDate", DbType.String, cutOffDate);
                db.AddInParameter(cmd, "@BIT_ChronicallyIll", DbType.Boolean, chronicallyIll);
                db.AddOutParameter(cmd, "@BIT_SecureActApplicable", DbType.Boolean,1);
                db.ExecuteNonQuery(cmd);
                bitResult = Convert.ToBoolean(db.GetParameterValue(cmd, "@BIT_SecureActApplicable"));              
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
        /// Get beneficiary Details(Birth Date, Relationship code)
        /// </summary>
        /// <param name="beneficiaryId">Beneficiary ID</param>
        /// <returns>Beneficiary details in Table</returns>
        public static DataTable GetBeneficiaryDetails(string beneficiarySSNo)
        {

            Database db;
            DbCommand cmd;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_SecuredAct_GetBeneficiaryDetails");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@VARCHAR_BeneficiarySSNo", DbType.String, beneficiarySSNo);
                db.ExecuteNonQuery(cmd);
                ds = new DataSet();
                db.LoadDataSet(cmd, ds, "BeneficiaryDetails");
                return ds.Tables["BeneficiaryDetails"];
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

    }
}
