//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	LoginDAClass.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email			:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time	:	10/19/2005 3:23:57 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			    Date			   Description
//*******************************************************************************
//Neeraj Singh              06/jun/2010        Enhancement for .net 4.0
//Neeraj Singh              07/jun/2010        review changes done
//Dinesh Kanojia            03/Mar/2013        BT: 1861: Maintaining release versions in a database
//Manthan Rajguru           2015.09.16         YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
    /// <summary>
    /// Summary description for LoginDAClass.
    /// </summary>
    public class LoginDAClass
    {
        public static DataSet LookUpUserCredentialsForLogin(string paramUserName)
        {
            Database db = null;
            System.Data.Common.DbCommand commandLookUpUserPwdForLogin = null;
            DataSet dsLookUpUserPwdForLogin = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                commandLookUpUserPwdForLogin = db.GetStoredProcCommand("dbo.yrs_usp_VRU_LookUpUserPwdForLogin");
                if (commandLookUpUserPwdForLogin == null) return null;
                dsLookUpUserPwdForLogin = new DataSet();
                db.AddInParameter(commandLookUpUserPwdForLogin, "@varchar_username", DbType.String, paramUserName);
                db.LoadDataSet(commandLookUpUserPwdForLogin, dsLookUpUserPwdForLogin, "LoginCredential");
                return dsLookUpUserPwdForLogin;

            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetServerDBVersion()
        {
            Database db = null;
            DbCommand commandGetServerDBVersion = null;
            DataSet dsGetServerDBVersion = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                //commandGetServerDBVersion=db.GetStoredProcCommand("yrs_usp_Get_Server_DB_Version");
                commandGetServerDBVersion = db.GetStoredProcCommand("yrs_usp_Get_Server_DB_Version_new");
                if (commandGetServerDBVersion == null) return null;
                dsGetServerDBVersion = new DataSet();
                db.LoadDataSet(commandGetServerDBVersion, dsGetServerDBVersion, "Server");
                return dsGetServerDBVersion;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetPatchRelaseVersions()
        {
            Database db = null;
            DbCommand commandGetPatchRelaseVersions = null;
            DataSet dsGetPatchRelaseVersions = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                //commandGetServerDBVersion=db.GetStoredProcCommand("yrs_usp_Get_Server_DB_Version");
                //Dinesh Kanojia                 03/Mar/2013        BT: 1861: Maintaining release versions in a database
                commandGetPatchRelaseVersions = db.GetStoredProcCommand("yrs_usp_Release_getpatch_release_version");
                if (commandGetPatchRelaseVersions == null) return null;
                dsGetPatchRelaseVersions = new DataSet();
                db.LoadDataSet(commandGetPatchRelaseVersions, dsGetPatchRelaseVersions, "Server");
                return dsGetPatchRelaseVersions;
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetGroupMembers(string paramGroupName)
        {
            Database db = null;
            DbCommand commandGroupMembers = null;
            DataSet dsGroupMembers = null;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                commandGroupMembers = db.GetStoredProcCommand("dbo.yrs_usp_Ln_GetGroupMembers");
                if (commandGroupMembers == null) return null;
                dsGroupMembers = new DataSet();
                db.AddInParameter(commandGroupMembers, "@varchar_GroupName", DbType.String, paramGroupName);
                db.LoadDataSet(commandGroupMembers, dsGroupMembers, "GroupMembers");
                return dsGroupMembers;

            }
            catch
            {
                throw;
            }
        }
        #region "START Dinesh Kanojia:03/Mar/2013:BT: 1861: Maintaining release versions in a database"
        public static DataSet GetOnlineUserInfo(string strUsername)
        {
            Database db = null;
            DbCommand commandOnlineUserInfo = null;
            DataSet dsOnlineUserInfo = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                //commandGetServerDBVersion=db.GetStoredProcCommand("yrs_usp_Get_Server_DB_Version");
                commandOnlineUserInfo = db.GetStoredProcCommand("yrs_usp_getLoginSessionInfo");
                db.AddInParameter(commandOnlineUserInfo, "@chvUsername ", DbType.String, strUsername);
                if (commandOnlineUserInfo == null) return null;
                dsOnlineUserInfo = new DataSet();
                db.LoadDataSet(commandOnlineUserInfo, dsOnlineUserInfo, "Server");
                return dsOnlineUserInfo;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetCurrentDayOnlineUserInfo()
        {
            Database db = null;
            DbCommand commandOnlineUserInfo = null;
            DataSet dsOnlineUserInfo = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                commandOnlineUserInfo = db.GetStoredProcCommand("yrs_usp_GetTodaySessions");
                if (commandOnlineUserInfo == null) return null;
                dsOnlineUserInfo = new DataSet();
                db.LoadDataSet(commandOnlineUserInfo, dsOnlineUserInfo, "Server");
                return dsOnlineUserInfo;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetOnlineUsers()
        {
            Database db = null;
            DbCommand commandOnlineUserInfo = null;
            DataSet dsOnlineUserInfo = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                commandOnlineUserInfo = db.GetStoredProcCommand("yrs_usp_getOnlineUsers");
                if (commandOnlineUserInfo == null) return null;
                dsOnlineUserInfo = new DataSet();
                db.LoadDataSet(commandOnlineUserInfo, dsOnlineUserInfo, "Server");
                return dsOnlineUserInfo;
            }
            catch
            {
                throw;
            }
        }

        public static string AddSessionInfo(string strSessionId, int iUserId, string strUsername, string strIPAddress, string strSessionStatus, DateTime dtmLoggedin, DateTime dtmLoggedOut, int iCreator, int iUpdater, string strHostname, bool bKillSession)
        {
            Database db = null;
            DbCommand AddCommandWrapper = null;
            String l_output;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_Login_AddSessionInfo");

                if (AddCommandWrapper == null) return null;

                db.AddInParameter(AddCommandWrapper, "@chvSessionId", DbType.String, strSessionId);
                db.AddInParameter(AddCommandWrapper, "@intUserId", DbType.Int32, iUserId);
                db.AddInParameter(AddCommandWrapper, "@chvUserName", DbType.String, strUsername);
                db.AddInParameter(AddCommandWrapper, "@chvIpAddress", DbType.String, strIPAddress);
                db.AddInParameter(AddCommandWrapper, "@chvSessionStatus", DbType.String, strSessionStatus);
                db.AddInParameter(AddCommandWrapper, "@dtmLoggedOn", DbType.DateTime, dtmLoggedin);
                db.AddInParameter(AddCommandWrapper, "@HostName", DbType.String, strHostname);
                db.AddInParameter(AddCommandWrapper, "@KillSession", DbType.Boolean, bKillSession);
                //db.AddInParameter(AddCommandWrapper, "@dtmLoggedOut", DbType.DateTime, dtmLoggedOut);
                db.AddInParameter(AddCommandWrapper, "@dtmCreator", DbType.Int32, iCreator);
                db.AddInParameter(AddCommandWrapper, "@dtmUpdater", DbType.Int32, iUpdater);
                db.AddOutParameter(AddCommandWrapper, "@int_outPut", DbType.Int32, 5);
                db.ExecuteNonQuery(AddCommandWrapper);

                l_output = db.GetParameterValue(AddCommandWrapper, "@int_outPut").ToString();

                return l_output;
            }
            catch
            {
                throw;
            }
        }

        public static string UpdateSessionInfo(string strSessionId, int iUserId, string strUsername, string strIPAddress, string strSessionStatus, DateTime dtmLoggedin, DateTime dtmLoggedOut, int iCreator, int iUpdater, string strHostname, bool bKillSession)
        {
            Database db = null;
            DbCommand UpdateCommandWrapper = null;
            String l_output;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_Login_UpdateSessionInfo");

                if (UpdateCommandWrapper == null) return null;

                db.AddInParameter(UpdateCommandWrapper, "@chvSessionId", DbType.String, strSessionId);
                db.AddInParameter(UpdateCommandWrapper, "@intUserId", DbType.Int32, iUserId);
                db.AddInParameter(UpdateCommandWrapper, "@chvUserName", DbType.String, strUsername);
                db.AddInParameter(UpdateCommandWrapper, "@chvIpAddress", DbType.String, strIPAddress);
                db.AddInParameter(UpdateCommandWrapper, "@chvSessionStatus", DbType.String, strSessionStatus);
                db.AddInParameter(UpdateCommandWrapper, "@HostName", DbType.String, strHostname);
                db.AddInParameter(UpdateCommandWrapper, "@KillSession", DbType.Boolean, bKillSession);
                //db.AddInParameter(UpdateCommandWrapper, "@dtmLoggedOn", DbType.DateTime, dtmLoggedin);
               // db.AddInParameter(UpdateCommandWrapper, "@dtmLoggedOut", DbType.DateTime, dtmLoggedOut);
                db.AddInParameter(UpdateCommandWrapper, "@dtmCreator", DbType.Int32, iCreator);
                db.AddInParameter(UpdateCommandWrapper, "@dtmUpdater", DbType.Int32, iUpdater);
               // db.AddOutParameter(UpdateCommandWrapper, "@int_outPut", DbType.Int32, 5);
                db.ExecuteNonQuery(UpdateCommandWrapper);

                l_output = "true";

                return l_output;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetUserDetails(string strUsername)
        {
            Database db = null;
            DbCommand commandGetUserdetails = null;
            DataSet dsGetUserDetails = null;
            try
            {

                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;
                //commandGetServerDBVersion=db.GetStoredProcCommand("yrs_usp_Get_Server_DB_Version");
                commandGetUserdetails = db.GetStoredProcCommand("yrs_usp_GetUserDetails");
                db.AddInParameter(commandGetUserdetails, "@usr_lanid", DbType.String, strUsername);
                if (commandGetUserdetails == null) return null;
                dsGetUserDetails = new DataSet();
                db.LoadDataSet(commandGetUserdetails, dsGetUserDetails, "Server");
                return dsGetUserDetails;
            }
            catch
            {
                throw;
            }
        }

        //END Dinesh Kanojia                 03/Mar/2013        BT: 1861: Maintaining release versions in a database
        #endregion
    }
}
