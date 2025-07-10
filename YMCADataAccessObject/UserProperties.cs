//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	UserProperties.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email			:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time	:	8/25/2005 6:36:14 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>.
//********************************************************************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified By			    Date				Description
//********************************************************************************************************************************
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
	/// Summary description for UserProperties.
	/// </summary>
	public sealed class UserProperties
	{
		public UserProperties()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet SearchGroupsForUser(string parameterUserId)
		{
			DataSet dsSearchGroupsForUser = null;
			Database db = null;
			DbCommand CommandSearchGroupsForUser = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				CommandSearchGroupsForUser = db.GetStoredProcCommand("yrs_usp_VRUGX_SelectGroups");
				if (CommandSearchGroupsForUser==null) return null;
				db.AddInParameter(CommandSearchGroupsForUser,"@varchar_UserId",DbType.String,parameterUserId);
				dsSearchGroupsForUser = new DataSet();
				dsSearchGroupsForUser.Locale=CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchGroupsForUser,dsSearchGroupsForUser,"UserGroupRef");
				System.AppDomain.CurrentDomain.SetData("DataSetGroupsForUser",dsSearchGroupsForUser);
				return dsSearchGroupsForUser;

			}
			catch
			{
				throw;
			}
		}
		public static void InsertUserDetails(string paramFirstName,string paramMiddleInitial,string paramLastName,string paramPhone, string paramExtn,string paramFax,string paramPassword,string paramActive,string paramUserName,string paramGroupList)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
		try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				insertCommandWrapper= db.GetStoredProcCommand("yrs_usp_VRU_InsertUserDetails");
				
				db.AddInParameter(insertCommandWrapper,"@varchar_FirstName",DbType.String,paramFirstName);
                db.AddInParameter(insertCommandWrapper, "@varchar_MiddleInitial", DbType.String, paramMiddleInitial);
                db.AddInParameter(insertCommandWrapper, "@varchar_LastName", DbType.String, paramLastName);
                db.AddInParameter(insertCommandWrapper, "@varchar_phone", DbType.String, paramPhone);
                db.AddInParameter(insertCommandWrapper, "@varchar_extn", DbType.String, paramExtn);

                db.AddInParameter(insertCommandWrapper, "@varchar_fax", DbType.String, paramFax);
                db.AddInParameter(insertCommandWrapper, "@varchar_password", DbType.String, paramPassword);
                db.AddInParameter(insertCommandWrapper, "@varchar_active", DbType.String, paramActive);
                db.AddInParameter(insertCommandWrapper, "@varchar_UserName", DbType.String, paramUserName);

                db.AddInParameter(insertCommandWrapper, "@varchar_GroupList", DbType.String, paramGroupList);
				
				db.ExecuteNonQuery(insertCommandWrapper);

			}	
			catch
			{
				throw;
			}
		}
		public static void UpdateUserDetails(int paramUserId,string paramFirstName,string paramMiddleInitial,string paramLastName,string paramPhone, string paramExtn,string paramFax,string paramPassword,string paramActive,string paramUserName,string paramGroupList)
		{
			Database db= null;
			DbCommand updateCommandWrapper = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_VRU_UpdateUserDetails");

				db.AddInParameter(updateCommandWrapper,"@integer_UserId",DbType.Int32,paramUserId);
                db.AddInParameter(updateCommandWrapper, "@varchar_FirstName", DbType.String, paramFirstName);
                db.AddInParameter(updateCommandWrapper, "@varchar_MiddleInitial", DbType.String, paramMiddleInitial);
                db.AddInParameter(updateCommandWrapper, "@varchar_LastName", DbType.String, paramLastName);
                db.AddInParameter(updateCommandWrapper, "@varchar_phone", DbType.String, paramPhone);

                db.AddInParameter(updateCommandWrapper, "@varchar_extn", DbType.String, paramExtn);
                db.AddInParameter(updateCommandWrapper, "@varchar_fax", DbType.String, paramFax);
                db.AddInParameter(updateCommandWrapper, "@varchar_password", DbType.String, paramPassword);
                db.AddInParameter(updateCommandWrapper, "@varchar_active", DbType.String, paramActive);
                db.AddInParameter(updateCommandWrapper, "@varchar_UserName", DbType.String, paramUserName);

                db.AddInParameter(updateCommandWrapper, "@varchar_GroupList", DbType.String, paramGroupList);
				db.ExecuteNonQuery(updateCommandWrapper);
			}
			catch
			{
				throw; 
			}
		}
		public static void DeleteUserDetails(int paramUserId)
		{
			Database db= null;
			DbCommand deleteCommandWrapper = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				deleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_VRU_DeleteUserDetails");
				db.AddInParameter(deleteCommandWrapper,"@integer_UserKey",DbType.Int32,paramUserId);
				db.ExecuteNonQuery(deleteCommandWrapper);
			}
			catch
			{
				throw;
			}
		}
	}
}
