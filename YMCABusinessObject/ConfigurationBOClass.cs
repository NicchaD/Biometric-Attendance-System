//****************************************************
//Modification History
//****************************************************
//Modified by           Date             Description
//****************************************************
//Benhan David          11/22/2018     YRS-AT-4133 -  YRS enh: Annuity Estimate calculator & Goal Calculator: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497) 
//*****************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace YMCARET.YmcaBusinessObject
{
    public class ConfigurationBOClass
    {
        /// <summary>
        /// Configuration property for RDB ADB planc change cut off date
        /// </summary>
        public static DateTime RDB_ADB_2019PlanChangeCutOffDate
        {
            get
            {
                DateTime dtReturnValue;
                try
                {
                    string stConfig = GetConfigValue("RDB_ADB_2019Plan_Change_CutOFF_Date");
                    if (string.IsNullOrEmpty(stConfig))
                    {
                        dtReturnValue = Convert.ToDateTime("1/1/2019");
                    }
                    else
                        dtReturnValue = Convert.ToDateTime(stConfig);
                }
                catch (Exception)
                {
                    dtReturnValue = Convert.ToDateTime("1/1/2019");
                }
                return dtReturnValue;
            }
        }

        /// <summary>
        /// Function to get config value from database by using key
        /// </summary>
        /// <param name="stKey">Configuration Key as string</param>
        /// <returns>Configuration Value as string</returns>
        private static string GetConfigValue(string stKey)
        {
            try
            {
                DataSet dsCutOffDate = YMCARET.YmcaDataAccessObject.YMCACommonDAClass.getConfigurationValue(stKey);
                if (dsCutOffDate.Tables.Count != 0 && dsCutOffDate.Tables[0].Rows.Count != 0)
                {
                    DataRow drCutOffDate = dsCutOffDate.Tables[0].Rows[0];
                    return drCutOffDate["Value"].ToString();
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
