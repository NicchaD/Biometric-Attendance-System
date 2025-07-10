/********************************************************************************************************************************
 Modification History
'********************************************************************************************************************************
'Modified By			Date					Description
'********************************************************************************************************************************
'Ashutosh Patil			16-May-2007				To Get The Details like MailService, UseDefault and 
												FromEmail,ToEmaial,BCCEamiail For Particular ConfigCategoryCode
'Manthan Rajguru        2015.09.16              YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Shilpa N               03/19/2019              YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
 * '********************************************************************************************************************************/

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
using System.Security.Permissions;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for MetaConfigMaintenance.
	/// </summary>
	public sealed class MetaConfigMaintenance
	{
		private MetaConfigMaintenance()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookupConfigurationMaintenance()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaConfigMaintenanceDAClass.LookupConfigurationMaintenance());
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// Function returning dataset for the search against 'Annuisty Basis'. It will throw error if key doesnot exist.
		/// </summary>
		/// <param name="parameterSearchAnnuityBasisTypes"></param>
		/// <returns></returns>
		public static DataSet SearchConfigurationMaintenance(string parameterConfigurationMaintenance)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaConfigMaintenanceDAClass.SearchConfigurationMaintenance(parameterConfigurationMaintenance));
			}
			catch
			{
				throw;
			}

		}
        //START : Shilpa N | 03/14/2019 | YRS-AT-4248 | Created method to get the value of Configuration Key and handled if it is absent.
        /// <summary>
        ///	This method will get the value of key from AtsMetaConfiguration table. If key does not exist it will return empty string.
        /// </summary>
        public static string GetConfigurationKeyValue(string paramKey)
        {
            string paramKeyValue;
            try
            {
                paramKeyValue = SearchConfigurationMaintenance(paramKey).Tables[0].Rows[0]["Value"].ToString();
                return paramKeyValue;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("GetConfigurationKeyValue", String.Format("Found Value for the {0}: {1} ", paramKey, paramKeyValue));
            }
            catch
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("GetConfigurationKeyValue", String.Format("Key does not exist for {0} : ",paramKey));
                return string.Empty;
            }
        }
        //END : Shilpa N | 03/14/2019 | YRS-AT-4248 | Created method to get the value of Configuration Key and handled if it is absent.
		
        /// <summary>
		///	This method Insert values into the table 'AtsMetaProjectedInterestRates' 
		///	on click of Add button followed by save button after entering data in the textboxes of UI
		/// and also Update values into the table 'AtsMetaProjectedInterestRates' 
		///	on click of edit button followed by save button after changing data in the textboxes of UI
		/// </summary>
		/// <param name="parameterAnnuityBasisTypes"></param>
		public static void InsertConfigurationMaintenance(DataSet parameterConfigurationMaintenance)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.MetaConfigMaintenanceDAClass.InsertConfigurationMaintenance(parameterConfigurationMaintenance);
			}
			catch
			{
				throw;
			}
		}



	}
}
