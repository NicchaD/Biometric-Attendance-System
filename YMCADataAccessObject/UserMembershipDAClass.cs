//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	UserMembershipDAClass.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email				:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time		:	8/26/2005 6:48:00 PM
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
	/// Summary description for UserMembershipDAClass.
	/// </summary>
	public class UserMembershipDAClass
	{
		public UserMembershipDAClass()
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
				if (db==null) return null;
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
		public static void UpdateUserMembership(int parameterUserId,string paramGroupList)
		{
			Database db=null;
			DbCommand commandUpdateUserMembership = null;
   			try
			{	
				db=DatabaseFactory.CreateDatabase("YRS");
				commandUpdateUserMembership = db.GetStoredProcCommand("yrs_usp_VRU_UpdateUserMembership");
				db.AddInParameter(commandUpdateUserMembership,"@integer_UserId",DbType.Int32,parameterUserId);
				db.AddInParameter(commandUpdateUserMembership,"@varchar_GroupList",DbType.String,paramGroupList);
				db.ExecuteNonQuery(commandUpdateUserMembership);
			}
			catch
			{
				throw;
			}
		}
	}
}
