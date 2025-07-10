//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	UserPermissionExceptionsDAClass.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email			:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time	:	10/4/2005 11:14:27 AM
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
	/// Summary description for UserPermissionExceptionsDAClass.
	/// </summary>
	public class UserPermissionExceptionsDAClass
	{
		public UserPermissionExceptionsDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpSecItems(string paramSearchChar,int paramUserId)
		{
			Database db = null;
			DbCommand commandLookUpSecItems= null;
			DataSet dsLookUpSecItems= null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandLookUpSecItems=db.GetStoredProcCommand("yrs_usp_VRS_PerEx_LookupSecuredItems");
				if(commandLookUpSecItems==null) return null;
				db.AddInParameter(commandLookUpSecItems,"@varchar_SearchChar",DbType.String,paramSearchChar);
                db.AddInParameter(commandLookUpSecItems, "@integer_userId", DbType.Int16, paramUserId);
                dsLookUpSecItems= new DataSet();
				db.LoadDataSet(commandLookUpSecItems,dsLookUpSecItems,"PerExSecItems");
				return dsLookUpSecItems;

			}
			catch
			{
				throw;
			}
		}
		public static void InsertUserAccExceptions(int paramUserId,int paramItemCode,int paramAccess)
		{
			Database db=null;
			DbCommand commandInsertUserAccExceptions = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				commandInsertUserAccExceptions=db.GetStoredProcCommand("yrs_usp_VRUAE_InsertUserAccExceptions");
				db.AddInParameter(commandInsertUserAccExceptions,"@integer_UserId",DbType.Int16,paramUserId);
                db.AddInParameter(commandInsertUserAccExceptions, "@integer_ItemCode", DbType.Int16, paramItemCode);
                db.AddInParameter(commandInsertUserAccExceptions, "@integer_Access", DbType.Int16, paramAccess);
                db.ExecuteNonQuery(commandInsertUserAccExceptions);

			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpUserAccExceptions(int paramUserId)
		{
			Database db=null;
			DbCommand commandLookUpUserAccExceptions = null;
			DataSet dsLookUpUserAccExceptions=null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpUserAccExceptions=db.GetStoredProcCommand("yrs_usp_VRUAE_LookupAccExceptions");
				if (commandLookUpUserAccExceptions==null) return null;
				db.AddInParameter(commandLookUpUserAccExceptions,"@integer_UserId",DbType.Int16,paramUserId);
				dsLookUpUserAccExceptions= new DataSet();
				db.LoadDataSet(commandLookUpUserAccExceptions,dsLookUpUserAccExceptions,"AccExceptions");
				return dsLookUpUserAccExceptions;
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpAllItems(string paramSearchChar)
		{
			Database db = null;
			DbCommand commandLookAllItems= null;
			DataSet dsLookUpAllItems= null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandLookAllItems=db.GetStoredProcCommand("yrs_usp_VRS_PerEx_LookupAllSecuredItems");
				if(commandLookAllItems==null) return null;
				db.AddInParameter(commandLookAllItems,"@varchar_SearchChar",DbType.String,paramSearchChar);
                dsLookUpAllItems= new DataSet();
				db.LoadDataSet(commandLookAllItems,dsLookUpAllItems,"AllItems");
				return dsLookUpAllItems;

			}
			catch
			{
				throw;
			}
		}
	}
}
