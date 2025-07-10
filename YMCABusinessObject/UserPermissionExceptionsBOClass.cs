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
	/// Summary description for UserPermissionExceptionsBOClass.
	/// </summary>
	public class UserPermissionExceptionsBOClass
	{
		public UserPermissionExceptionsBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpSecItems(string paramSearchChar,int paramUserId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UserPermissionExceptionsDAClass.LookUpSecItems(paramSearchChar,paramUserId);
			}
			catch
			{
				throw;
			}
		}
		public static void InsertUserAccExceptions(int paramUserId,int paramItemCode,int paramAccess)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.UserPermissionExceptionsDAClass.InsertUserAccExceptions(paramUserId,paramItemCode,paramAccess);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpUserAccExceptions(int paramUserId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UserPermissionExceptionsDAClass.LookUpUserAccExceptions(paramUserId);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpAllItems(string paramSearchChar)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UserPermissionExceptionsDAClass.LookUpAllItems(paramSearchChar);
			}
			catch
			{
				throw;
			}
		}
	}
}
