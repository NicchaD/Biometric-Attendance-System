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
	/// Summary description for UserPermissionsBOClass.
	/// </summary>
	public sealed class UserPermissionsBOClass
	{
		public UserPermissionsBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpPermissions(string parameterSecuredItemCode)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UserPermissionsDACLass.LookUpPermissions(parameterSecuredItemCode);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpAllSecuredItems(string parameterItemType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UserPermissionsDACLass.LookUpAllSecuredItems(parameterItemType);
			}
			catch
			{
				throw;
			}
		}
		public static void UpdateGroupPermissions(int paramSecuredItemCode,string paramGroupList,string paramAccessList)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.UserPermissionsDACLass.UpdateGroupPermissions(paramSecuredItemCode,paramGroupList,paramAccessList);
			}
			catch
			{
				throw;
			}
		}
	}
}
