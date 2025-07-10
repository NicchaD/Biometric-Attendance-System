//****************************************************
//Modification History
//****************************************************
//Modified by           Date            Description
//****************************************************
//Bhavna                june/23/2011    fetch and save History
//Bhavna                Aug/26/2011     Delete History   
//Bhavna                Aug/30/2011     Change in  Clear_MenuHistory() 
//Manthan Rajguru       2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
//using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// Summary description for YMCAMenuHistoryBOClass
    /// </summary>
    public class YMCAMenuHistoryBOClass
    {
        public YMCAMenuHistoryBOClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static DataSet Fetch_MenuHistory(int parameterUserId)
        {

            try
            {

                return (YMCARET.YmcaDataAccessObject.YMCAMenuHistoryDAClass.Fetch_MenuHistory(parameterUserId));
            }
            catch
            {
                throw;
            }

        }
        public static string Insert_MenuHistory(int parameterUserId, string parameterPageName, string parameterPageLink)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.YMCAMenuHistoryDAClass.Insert_MenuHistory(parameterUserId, parameterPageName, parameterPageLink);
            }
            catch
            { throw; }
        }
		public static void Clear_MenuHistory(int parameterUserId)
		{

			try
			{

				YMCARET.YmcaDataAccessObject.YMCAMenuHistoryDAClass.Clear_MenuHistory(parameterUserId);

			}
			catch
			{
				//throw;
			}

		}
    }
}