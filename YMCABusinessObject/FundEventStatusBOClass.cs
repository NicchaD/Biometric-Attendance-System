//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Pramod P. Pokale     2017.09.21    YRS-AT-3631 - YRS enh: Data Corrections Tool -Admin screen function to allow manual update of Fund status from WD or WP to TM or PT (TrackIT 30950) 
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
using System.Security.Permissions;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for FundEventStatus.
	/// </summary>
	public sealed class FundEventStatus
	{
		private FundEventStatus()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookupInterestRate()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.FundEventStatusDAClass.LookupFundEventStatus());
			}
			catch 
			{
				throw;
			}
		}


		public static DataSet SearchFundEventStatus(string parameterFundEventStatus)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.FundEventStatusDAClass.SearchFundEventStatus(parameterFundEventStatus));
			}
			catch
			{
				throw;
			}

		}

		public static void InsertFundEventStatus(DataSet parameterFundEventStatus)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.FundEventStatusDAClass.InsertFundEventStatus(parameterFundEventStatus);
			}
			catch
			{
				throw;
			}
		}

        //START: PPP | 09/21/2017 | YRS-AT-3631 | Recommends fund event status for given fund event id
        /// <summary>
        /// Recommends fund event status for given fund event id based on balance, employment and annuities
        /// </summary>
        /// <param name="fundEventID">Fund Event ID</param>
        /// <returns>Recommended fund event status</returns>
        public static string GetRecommendedFundEventStatus(string fundEventID)
        {
            return YMCARET.YmcaDataAccessObject.FundEventStatusDAClass.GetRecommendedFundEventStatus(fundEventID);
        }
        //END: PPP | 09/21/2017 | YRS-AT-3631 | Recommends fund event status for given fund event id
    }
}
