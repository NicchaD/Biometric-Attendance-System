/********************************************************************************************************************************
 Modification History
'********************************************************************************************************************************
'Modified By	   Date				Description
'********************************************************************************************************************************
//Bhavna           june/23/2011     fetch and save History 
//Bhavna           Aug/26/2011      Delete History  
//Bhavna           Aug/30/2011      Change in  Clear_MenuHistory() 
//Manthan Rajguru  2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
    /// <summary>
    /// Summary description for YMCAMenuHistoryClass
    /// </summary>
    public class YMCAMenuHistoryDAClass
    {

        public YMCAMenuHistoryDAClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }
      
        public static DataSet Fetch_MenuHistory(int parameterUserId)
        {
            Database db = null;
            DbCommand commandMenuHistory = null;
            DataSet l_dataset_MenuHistory = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) throw new Exception("Unable to create Database connection");
                commandMenuHistory = db.GetStoredProcCommand("yrs_usp_UT_FetchMenuHistory");
				if (commandMenuHistory == null) throw new Exception("Unable to access Stored Procedure");
                commandMenuHistory.CommandTimeout = 1200;
                if (commandMenuHistory == null) return null;
                db.AddInParameter(commandMenuHistory, "@intUserid", DbType.Int16, parameterUserId);

                l_dataset_MenuHistory = new DataSet();
                db.LoadDataSet(commandMenuHistory, l_dataset_MenuHistory, "atsUserTracking");

                return l_dataset_MenuHistory;

            }
            catch (Exception ex)
            {
                //throw ex;
				return l_dataset_MenuHistory = null;
            }
        }


        public static string Insert_MenuHistory(int parameterUserId, string parameterPageName, string parameterPageLink)
        {

            Database db = null;
            DbCommand commandMenuHistory = null;
            string l_string_ErrorString = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) throw new Exception("Unable to create Database connection");

                commandMenuHistory = db.GetStoredProcCommand("yrs_usp_UT_InsertUpdate_MenuHistory");

                if (commandMenuHistory == null) throw new Exception("Unable to access Stored Procedure");

                commandMenuHistory.CommandTimeout = 1200;
                //Add input parameters
                db.AddInParameter(commandMenuHistory, "@intUserid", DbType.Int16, parameterUserId);
                db.AddInParameter(commandMenuHistory, "@chvPageNameAndPath", DbType.String, parameterPageName);
                db.AddInParameter(commandMenuHistory, "@chvHyperLinkName", DbType.String, parameterPageLink);
                //Add output parameters
                db.AddOutParameter(commandMenuHistory, "@coutput", DbType.String, 1000);
                //Execute the command
                db.ExecuteNonQuery(commandMenuHistory);
                //Get the output values
                l_string_ErrorString = db.GetParameterValue(commandMenuHistory, "@coutput").ToString();

                return l_string_ErrorString;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
		public static void  Clear_MenuHistory(int parameterUserId)
		{
			Database db = null;
			DbCommand commandMenuHistory = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) throw new Exception("Unable to create Database connection");
				commandMenuHistory = db.GetStoredProcCommand("yrs_usp_UT_ClearMenuHistory");
				if (commandMenuHistory == null) throw new Exception("Unable to access Stored Procedure");
				commandMenuHistory.CommandTimeout = 1200;
				db.AddInParameter(commandMenuHistory, "@intUserid", DbType.Int16, parameterUserId);
				db.ExecuteNonQuery(commandMenuHistory);

			}
			catch (Exception ex)
			{
				//throw ex;
			}
		}



    }
}