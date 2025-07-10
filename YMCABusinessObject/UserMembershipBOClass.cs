//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	UserMembershipBOClass.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email				:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time		:	8/26/2005 6:54:22 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
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
	/// Summary description for UserMembershipBOClass.
	/// </summary>
	public sealed class UserMembershipBOClass
	{
		public UserMembershipBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet SearchGroupsForUser(string parameterSearchGroupsForUser)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UserMembershipDAClass.SearchGroupsForUser(parameterSearchGroupsForUser);
			}
			catch
			{
				throw;
			}
		}
		public static void UpdateUserMembership(int parameterUserId,string paramGroupList)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.UserMembershipDAClass.UpdateUserMembership(parameterUserId,paramGroupList);
			}
			catch
			{
				throw;
			}
		}
	}
}
