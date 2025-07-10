//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	UserPermissionsDACLass.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email				:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time		:	8/31/2005 11:13:20 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			    Date				Description
//*******************************************************************************
//Neeraj Singh              06/jun/2010         Enhancement for .net 4.0
//Neeraj Singh              07/jun/2010         review changes done
//Manthan Rajguru           2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for UserPermissionsDACLass.
	/// </summary>
	public sealed class UserPermissionsDACLass
	{
		public UserPermissionsDACLass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpPermissions(string parameterSecuredItemCode)
		{
			Database db = null;
			DataSet dsLookUpPermissions = null;
			DbCommand commandLookUpPermissions=null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandLookUpPermissions = db.GetStoredProcCommand("yrs_usp_VRSA_LookUpGroupPermissions");
				if(commandLookUpPermissions==null) return null;
				db.AddInParameter(commandLookUpPermissions,"@varchar_SecuredItemCode",DbType.String,parameterSecuredItemCode);
				dsLookUpPermissions=new DataSet();
				dsLookUpPermissions.Locale=CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpPermissions,dsLookUpPermissions,"GroupPermissions");
				System.AppDomain.CurrentDomain.SetData("DataSetGroupPermissions",dsLookUpPermissions);
				return dsLookUpPermissions;
			}
			catch
			{
				throw;
			}

		}
		public static DataSet LookUpAllSecuredItems(string parameterItemType)
		{
			Database db = null;
            DataSet dsLookUpAllSecuredItems=null;
			DbCommand commandLookUpAllSecuredItems=null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandLookUpAllSecuredItems=db.GetStoredProcCommand("yrs_usp_VRSI_LookUpAllSecuredItems");
				db.AddInParameter(commandLookUpAllSecuredItems,"@varchar_ItemTypeCode",DbType.String,parameterItemType);
				dsLookUpAllSecuredItems = new DataSet();
                dsLookUpAllSecuredItems.Locale=CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpAllSecuredItems,dsLookUpAllSecuredItems,"AllItems");
				System.AppDomain.CurrentDomain.SetData("DataSetAllItems",dsLookUpAllSecuredItems);
				return dsLookUpAllSecuredItems;
			}
			catch
			{
				throw;
			}
		}
		public static void UpdateGroupPermissions(int paramSecuredItemCode,string paramGroupList,string paramAccessList)
		{
			Database db = null;
			DbCommand commandUpdateGroupPermissions = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				commandUpdateGroupPermissions=db.GetStoredProcCommand("yrs_usp_VRSA_UpdateGroupPermissions");
                db.AddInParameter(commandUpdateGroupPermissions, "@Integer_SecuredItemCode", DbType.Int32, paramSecuredItemCode);
                db.AddInParameter(commandUpdateGroupPermissions, "@Varchar_GroupList", DbType.String, paramGroupList);
                db.AddInParameter(commandUpdateGroupPermissions, "@varchar_Access", DbType.String, paramAccessList);
				db.ExecuteNonQuery(commandUpdateGroupPermissions);
			}
			catch
			{
				throw;
			}
		}
	}
}
