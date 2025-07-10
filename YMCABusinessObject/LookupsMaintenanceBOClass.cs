//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Manthan Rajguru      2016.07.27    YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field. 
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for LookupsMaintenance.
	/// </summary>
	public sealed class LookupsMaintenanceBOClass
	{
		private LookupsMaintenanceBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookupLookups()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.LookupsMaintenanceDAClass.LookupLookups());
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet SearchLookups(string parameterLookups)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.LookupsMaintenanceDAClass.SearchLookups(parameterLookups));
			}
			catch
			{
				throw;
			}

		}
		
		public static void InsertLookups(DataSet parameterInsertLookups)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.LookupsMaintenanceDAClass.InsertLookups(parameterInsertLookups);
			}
			catch
			{
				throw;
			}
		}
        //Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Verify Phony SSN provided by user is valid or not.
        public static Boolean IsValidPhonySSN(string strPhonySSN, string strBeneficiaryStatus, string strBeneficiaryId, string strBeneficiaryType)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.LookupsMaintenanceDAClass.IsValidPhonySSN(strPhonySSN, strBeneficiaryStatus, strBeneficiaryId, strBeneficiaryType));
            }
            catch
            {
                throw;
            }
        }
        //End - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Verify Phony SSN provided by user is valid or not.
    }
}
