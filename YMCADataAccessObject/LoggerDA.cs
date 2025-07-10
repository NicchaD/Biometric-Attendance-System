//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Anudeep A            2016.06.28    YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
//Manthan Rajguru      09.18.2017    YRS-AT-3665 -  YRS enh: Data Corrections Tool - Admin screen option to create a manual credit
//*****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMCAObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace YMCARET.YmcaDataAccessObject
{
   public class LoggerDA
    {
        public static void WriteLogDB(YMCAActionEntry actionEntry)
        {
            
            Database db = null;
            DbCommand dbCommand = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return ;
                dbCommand = db.GetStoredProcCommand("yrs_usp_InsertYRSActivityLog");
                if (dbCommand == null) return;
                db.AddInParameter(dbCommand, "@chvAction", DbType.String, actionEntry.Action);
                db.AddInParameter(dbCommand, "@chvActionBy", DbType.String, actionEntry.ActionBy);
                db.AddInParameter(dbCommand, "@chvData", DbType.String, actionEntry.Data);
                //Start:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
                //db.AddInParameter(dbCommand, "@guiEntityID", DbType.Guid, actionEntry.EntityId);
                db.AddInParameter(dbCommand, "@chvEntityID", DbType.String, actionEntry.EntityId);
                //Start:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
                db.AddInParameter(dbCommand, "@chvEntityType", DbType.String, actionEntry.EntityType.ToString());
                db.AddInParameter(dbCommand, "@chvModule", DbType.String, actionEntry.Module);
                db.AddInParameter(dbCommand, "@bitSuccess", DbType.Boolean, actionEntry.SuccessStatus);
                db.ExecuteNonQuery(dbCommand);                
            }
            catch
            {
                throw;
            }
        }

        //START: MMR | 09/18/2017 | YRS-AT-3665 | Entry into YRS Activity log
        /// <summary>
       /// Log action details into datasource
       /// </summary>
       /// <param name="actionEntry">Action Details</param>
       /// <param name="transaction">SQL Transaction handled by application</param>
        public static void WriteLogDB(YMCAActionEntry actionEntry, DbTransaction transaction)
        {
            Database db;
            DbCommand cmd;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return;
                cmd = db.GetStoredProcCommand("yrs_usp_InsertYRSActivityLog");
                if (cmd == null) return;
                db.AddInParameter(cmd, "@chvAction", DbType.String, actionEntry.Action);
                db.AddInParameter(cmd, "@chvActionBy", DbType.String, actionEntry.ActionBy);
                db.AddInParameter(cmd, "@chvData", DbType.String, actionEntry.Data);
                db.AddInParameter(cmd, "@chvEntityID", DbType.String, actionEntry.EntityId);
                db.AddInParameter(cmd, "@chvEntityType", DbType.String, actionEntry.EntityType.ToString());
                db.AddInParameter(cmd, "@chvModule", DbType.String, actionEntry.Module);
                db.AddInParameter(cmd, "@bitSuccess", DbType.Boolean, actionEntry.SuccessStatus);
                db.ExecuteNonQuery(cmd, transaction);
            }
            catch
            {
                throw;
            }
            finally
            {
                db = null;
                cmd = null;
            }
        }
        //END: MMR | 09/18/2017 | YRS-AT-3665 | Entry into YRS Activity log
   }
}
