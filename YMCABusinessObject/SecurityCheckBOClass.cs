//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for SecurityCheckBOClass.
	/// </summary>
	public class SecurityCheckBOClass
	{
		public SecurityCheckBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static int LookUpLoginAccessPermission(int paramLoggedUserKey,string paramItemCode)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.SecurityCheckDAClass.LookUpLoginAccessPermission(paramLoggedUserKey,paramItemCode);
			}
			catch
			{
				throw;
			}
		}

		//By Aparna -YREN-3115

		public static int GetLoginNotesUser(int paramLoggedUserKey)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.SecurityCheckDAClass.GetLoginNotesUser(paramLoggedUserKey);
			}
			catch
			{
				throw;
			}
		}

	}
}
