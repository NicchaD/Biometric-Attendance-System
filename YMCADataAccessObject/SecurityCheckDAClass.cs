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
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for SecurityCheckDAClass.
	/// </summary>
	public class SecurityCheckDAClass
	{
		public SecurityCheckDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static int LookUpLoginAccessPermission(int paramLoggedUserKey,string paramItemCode)
		{
			Database db= null;
			DbCommand commandLookUpAccessPermission = null;
			int int_Output_Permission;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				commandLookUpAccessPermission=db.GetStoredProcCommand("yrs_usp_Security_LookupAccPermissions");
				
                db.AddInParameter(commandLookUpAccessPermission,"@integer_userId",DbType.Int32,paramLoggedUserKey);
                db.AddInParameter(commandLookUpAccessPermission, "@varchar_itemCode", DbType.String, paramItemCode);
                db.AddOutParameter(commandLookUpAccessPermission, "@integer_outpermission", DbType.Int16, 4);
				db.ExecuteNonQuery(commandLookUpAccessPermission);
				int_Output_Permission=Convert.ToInt32(db.GetParameterValue(commandLookUpAccessPermission,"@integer_outpermission"));
				return int_Output_Permission;			
			}
			catch
			{
				throw;
			}

		}
			//By Aparna Samala YREN-3115 13/03/2007

		public static int GetLoginNotesUser(int paramLoggedUserKey)
		{
			Database db= null;
			DbCommand commandLookUpAccessPermission = null;
			int int_Output_Permission;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				commandLookUpAccessPermission=db.GetStoredProcCommand("yrs_usp_GetLoginNotesUser");
				db.AddInParameter(commandLookUpAccessPermission,"@integer_userId",DbType.Int32,paramLoggedUserKey);
                db.AddOutParameter(commandLookUpAccessPermission, "@integer_Count", DbType.Int16, 4);
				db.ExecuteNonQuery(commandLookUpAccessPermission);

				int_Output_Permission=Convert.ToInt32(db.GetParameterValue(commandLookUpAccessPermission,"@integer_Count"));
				return int_Output_Permission;
					
				
			
			}
			catch
			{
				throw;
			}
		}
	}
}
