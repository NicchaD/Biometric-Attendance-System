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
	/// Summary description for UserGroupAdministrationBOClass.
	/// </summary>
	public  sealed class UserGroupAdministrationBOClass
	{
		public UserGroupAdministrationBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookupUsers(string parameterUserActive)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UserGroupAdministrationDAClass.LookupUsers(parameterUserActive);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookupUserGroups()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UserGroupAdministrationDAClass.LookupUserGroups();
			}
			catch
			{
				throw;
			}
		}
	}
}
