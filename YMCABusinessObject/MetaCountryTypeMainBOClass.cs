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
using System.Security.Permissions;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for MetaCountryTypeMain.
	/// </summary>
	public sealed class MetaCountryTypeMain
	{
		private MetaCountryTypeMain()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookupCountryTypes()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaCountryTypeDAClass.LookupCountryTypes());
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet SearchCountryTypes(string parameterCountryTypes)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaCountryTypeDAClass.SearchCountryTypes(parameterCountryTypes));
			}
			catch
			{
				throw;
			}

		}

		public static void InsertCountryTypes(DataSet parameterCountryTypes)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.MetaCountryTypeDAClass.InsertCountryTypes(parameterCountryTypes);
			}
			catch
			{
				throw;
			}
		}
	}
}
