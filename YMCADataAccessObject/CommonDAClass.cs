/************************************************************************************************************************/
// Author: Vinayan C
// Created on: 11/21/2018
// Summary of Functionality: Common class to define generic / common functions required in the YRS application, It is coupled with CommonBAClass.cs.
// Declared in Version: 20.6.0 | YRS-AT-4018 -  YRS enh: EFT Loans Project: “Update” YRS Maintenance: Person: Loan Tab 
//
/************************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name               | Date        | Version No    | Ticket
// ------------------------------------------------------------------------------------------------------
// 
/************************************************************************************************************************/

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
    public sealed class CommonDAClass
    {
        // START : VC | 2018.11.21 | YRS-AT-4018 -  Method used to check whether the participant loan is in progress or not. 
        public static int ValidatePIIRestrictions(string persId)
        {
            DbCommand command;
            Database database;
            DbConnection connection = null;
            try
            {
                database = DatabaseFactory.CreateDatabase("YRS");
                if (database == null) throw new Exception("Database object is null");
                
                connection = database.CreateConnection();
                connection.Open();
                if (connection == null) throw new Exception("Connection object is null");

                command = database.GetStoredProcCommand("yrs_usp_AMP_ValidatePIIRestrictions");
                if (command == null) throw new Exception("Database object is null");
                
                database.AddInParameter(command, "@VARCHAR_PersId", DbType.String, persId);
                database.AddOutParameter(command, "@INT_MessageCode", DbType.Int32, 10);
                
                database.ExecuteNonQuery(command);
                
                return Convert.IsDBNull(database.GetParameterValue(command, "@INT_MessageCode")) ? 0 : Convert.ToInt32(database.GetParameterValue(command, "@INT_MessageCode"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command = null;
                database = null;
                if (connection != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                connection = null;
            }
        }
        // END : VC | 2018.11.21 | YRS-AT-4018 -  Method used to check whether the participant loan is in progress or not.  
    }
}
