//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Sanjay Singh         2015.12.22    YRS-AT-2252 - Annuity Estimate (Website/YRS) -needs to check the current Annual Limits table 
//Anudeep A            2016.06.28    YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
//*****************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMCAObjects;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace YMCARET.YmcaBusinessObject
{
    public class LoggerBO
    {


        public static void WriteLog(YMCAActionEntry actionEntry)
        {
            DataSet dsMetaconfiguration = null;
            string strLogStore = string.Empty;
            //CheckLogIsEnable -- atsMetaconfiguration
            //if log is enable then check where logging should be done.(FILE / DB)
            //call the private method(s) for file and DB
            try
            {
                dsMetaconfiguration = new DataSet();
                try
                {
                    dsMetaconfiguration = MetaConfigMaintenance.SearchConfigurationMaintenance("ACTIVITY_LOG_TYPE");
                }
                catch (Exception)
                {
                   
                }
                if (dsMetaconfiguration == null || 
                    dsMetaconfiguration.Tables.Count == 0 || 
                    dsMetaconfiguration.Tables[0].Rows.Count == 0
                    ) 
                { 
                    throw new Exception("'ACTIVITY_LOG_TYPE' key not defined in atsMetaConfiguration"); 
                }
                strLogStore = dsMetaconfiguration.Tables[0].Rows[0]["Value"].ToString();
                if (strLogStore == "FILE")
                {
                    WriteLogFile(actionEntry);
                }
                else if (strLogStore == "DB")
                {
                    WriteLogDB(actionEntry);
                }
            }
            catch (Exception)
            {
                
                throw;
            }

        }
        //Start:AA 06.28.2016 YRS AT-2830 Changed below line as entry into DB should be available to all pages of YRS UI
        //private static void WriteLogDB(YMCAActionEntry actionEntry)
        public static void WriteLogDB(YMCAActionEntry actionEntry)
        //End:AA 06.28.2016 YRS AT-2830 Changed below line as entry into DB should be available to all pages of YRS UI
        {
            try
            {
                YMCARET.YmcaDataAccessObject.LoggerDA.WriteLogDB(actionEntry);
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        private static void WriteLogFile(YMCAActionEntry actionEntry)
        {
            try
            {
                Logger.Write("Action :"+ actionEntry.Action
                    + System.Environment.NewLine + "ActionBy : " + actionEntry.ActionBy
                    + System.Environment.NewLine + "EntityId : " + actionEntry.EntityId
                    + System.Environment.NewLine + "EntityType : " + actionEntry.EntityType.ToString()
                    + System.Environment.NewLine + "Details : " + System.Environment.NewLine + actionEntry.Data
                    + System.Environment.NewLine + "Module : " + actionEntry.Module
                    + System.Environment.NewLine + "SuccessStatus : " + actionEntry.SuccessStatus
                    , "ActivityLog");
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        // START | 2015.12.22 | Sanjay Singh | YRS-AT-2252 - Add function to Log information 
        public static bool LogMessage(string paraMessage,TraceEventType logtype)
        {
            try
            {
                Logger.Write(paraMessage, "Application", 0, 0, logtype);
                return true;
            }
            catch 
            {
                return false;
            }
        }
        // END | 2015.12.22 | Sanjay Singh | YRS-AT-2252 - Add function to Log information 

    }
}
 