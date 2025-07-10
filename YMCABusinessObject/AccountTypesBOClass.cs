//****************************************************
//Modification History
//****************************************************
//Modified by         Date           Description
//****************************************************
//Shubhrata           05/17/2007     Added a new method(GetAccountGroups) to fetch Account Groups from atsMetaAcctGroups
//Manthan Rajguru     2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for AccountTypes.
	/// </summary>
	public sealed class AccountTypes
	{
		private AccountTypes()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpAccountType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AccountTypesDAClass.LookUpAccountType();
			}
			catch
			{
				throw;
			}
		}

		public static void SaveAccountType(DataSet parameterInsertAccountType)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.AccountTypesDAClass.SaveAccountType(parameterInsertAccountType);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet SearchAccountType(string parameterSearchAccountType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AccountTypesDAClass.SearchAccountType(parameterSearchAccountType);
			}
			catch
			{
				throw;
			}

		}
		//Shubhrata May17th,2007 Plan Split Changes
		#region "Plan Split Changes"
		public static DataSet GetAccountGroups()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AccountTypesDAClass.GetAccountGroups();
			}
			catch
			{
				throw;
			}

		}
		#endregion
		//Shubhrata May17th,2007 Plan Split Changes
	}
}
