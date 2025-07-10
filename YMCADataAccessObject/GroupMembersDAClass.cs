//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMVA-YRS
// FileName			:	GroupMembersDAClass.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email				:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time		:	8/29/2005 11:06:59 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			         Date				 Description
//*******************************************************************************
//Neeraj Singh                   06/jun/2010         Enhancement for .net 4.0
//Neeraj Singh                   07/jun/2010         review changes done
//Manthan Rajguru                2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System;
using System.Data;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for GroupMembersDAClass.
	/// </summary>
	public sealed class GroupMembersDAClass
	{
		public GroupMembersDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet SearchMembersForGroup(string parameterGroupId)
		{
			DataSet dsMembersForGroup = null;
			Database db= null;
			DbCommand  commandSearchMembersForGroup = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandSearchMembersForGroup = db.GetStoredProcCommand ("yrs_usp_VRUGX_SelectMembers");
				if (commandSearchMembersForGroup==null) return null;
				db.AddInParameter(commandSearchMembersForGroup,"@varchar_GroupId",DbType.String,parameterGroupId);
				dsMembersForGroup = new DataSet();
				dsMembersForGroup.Locale=CultureInfo.InvariantCulture;
				db.LoadDataSet(commandSearchMembersForGroup,dsMembersForGroup,"UserGroupRef");
				System.AppDomain.CurrentDomain.SetData("DataSetMembersForGroup",dsMembersForGroup);
				return dsMembersForGroup;
			}
			catch
			{
				throw;
			}
		}

		public static void UpdateGroupMembers(int parameterGroupId, string parameterUserList)
		{
			Database db = null;
			DbCommand  commandUpdateGroupMembers = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				commandUpdateGroupMembers=db.GetStoredProcCommand ("yrs_usp_VRU_UpdateGroupMembers");
				db.AddInParameter(commandUpdateGroupMembers,"@integer_GroupId",DbType.Int32,parameterGroupId);
				db.AddInParameter(commandUpdateGroupMembers,"@varchar_UserList",DbType.String,parameterUserList);
				db.ExecuteNonQuery(commandUpdateGroupMembers);
			}
			catch
			{
				throw;
			}
		}
	}
}
