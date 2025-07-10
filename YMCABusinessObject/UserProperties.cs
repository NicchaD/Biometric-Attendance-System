//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	UserProperties.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email				:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time		:	9/8/2005 1:56:59 PM
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
		public static DataSet SearchGroupsForUser(string parameterSearchGroupsForUser)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UserProperties.SearchGroupsForUser(parameterSearchGroupsForUser);
          	}
			catch
			{
				throw;
			}
		}
		public static void InsertUserDetails(string paramFirstName,string paramMiddleInitial,string paramLastName,string paramPhone, string paramExtn,string paramFax,string paramPassword,string paramActive,string paramUserName,string paramGroupList)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.UserProperties.InsertUserDetails(paramFirstName,paramMiddleInitial,paramLastName,paramPhone,paramExtn,paramFax,paramPassword,paramActive,paramUserName,paramGroupList);
			}
			catch
			{
				throw;
			}
		}
		public static void UpdateUserDetails(int paramUserId,string paramFirstName,string paramMiddleInitial,string paramLastName,string paramPhone, string paramExtn,string paramFax,string paramPassword,string paramActive,string paramUserName,string paramGroupList)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.UserProperties.UpdateUserDetails(paramUserId,paramFirstName,paramMiddleInitial,paramLastName,paramPhone,paramExtn,paramFax,paramPassword,paramActive,paramUserName,paramGroupList);
			}
			catch
			{
				throw;
			}
		}
		public static void DeleteUserDetails(int paramUserId)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.UserProperties.DeleteUserDetails(paramUserId);
			}
			catch
			{
				throw;
			}
		}
	}
}
