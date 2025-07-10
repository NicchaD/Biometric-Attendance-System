//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// Summary description for Login.
    /// </summary>
    public class Login
    {
        public Login()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static DataSet LookUpUserCredentialsForLogin(string paramUserName)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.LookUpUserCredentialsForLogin(paramUserName);
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetServerDBVersion()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.GetServerDBVersion();
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetGroupMembers(string paramGroupName)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.GetGroupMembers(paramGroupName);
            }
            catch
            {
                throw;
            }
        }


        public static DataSet GetPatchRelaseVersions()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.GetPatchRelaseVersions();
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetOnlineUserInfo(string strUsername)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.GetOnlineUserInfo(strUsername);
            }
            catch
            {
                throw;
            }
        }


        public static DataSet GetCurrentDayOnlineUserInfo()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.GetCurrentDayOnlineUserInfo();
            }
            catch
            {
                throw;
            }
        }


        public static DataSet GetOnlineUsers()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.GetOnlineUsers();
            }
            catch
            {
                throw;
            }
        }


        public static DataSet GetUserDetails(string strUsername)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.GetUserDetails(strUsername);
            }
            catch
            {
                throw;
            }
        }

        public static string AddSessionInfo(string strSessionId, int iUserId, string strUsername, string strIPAddress, string strSessionStatus, DateTime dtmLoggedin, DateTime dtmLoggedOut, int iCreator, int iUpdater, string strHostname, bool bKillSession)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.AddSessionInfo(strSessionId, iUserId, strUsername, strIPAddress, strSessionStatus, dtmLoggedin, dtmLoggedOut, iCreator, iUpdater, strHostname, bKillSession);
            }
            catch
            {
                throw;
            }
        }


        public static string UpdateSessionInfo(string strSessionId, int iUserId, string strUsername, string strIPAddress, string strSessionStatus, DateTime dtmLoggedin, DateTime dtmLoggedOut, int iCreator, int iUpdater, string strHostname, bool bKillSession)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.LoginDAClass.UpdateSessionInfo(strSessionId, iUserId, strUsername, strIPAddress, strSessionStatus, dtmLoggedin, dtmLoggedOut, iCreator, iUpdater, strHostname, bKillSession);
            }
            catch
            {
                throw;
            }
        }
    }
}
