//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	GroupPropertiesBOClass.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email			:	vartika.jain@3i-infotech.com	
// Contact No		:	8733
// Creation Time		:	9/12/2005 8:12:06 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by          Date          Description
//*******************************************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for GroupPropertiesBOClass.
	/// </summary>
	public sealed class GroupPropertiesBOClass
	{
		public GroupPropertiesBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpGroupProperties(string parameterGroupId,string parameterItemType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.GroupPropertiesDAClass.LookUpGroupProperties(parameterGroupId,parameterItemType);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpSecuredItems(string parameterItemType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.GroupPropertiesDAClass.LookUpSecuredItems(parameterItemType);
			}
			catch
			{
				throw;
			}
		}
		public static int InsertGroupDetails(string paramGroup,string paramDescription,string paramSecuredItems,string paramAccess)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.GroupPropertiesDAClass.InsertGroupDetails(paramGroup,paramDescription,paramSecuredItems,paramAccess);
			}
			catch
			{
				throw;
			}
		}
		public static void DeleteGroupDetails(int ParamGroupId)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.GroupPropertiesDAClass.DeleteGroupDetails(ParamGroupId);
			}
			catch
			{
				throw;
			}
		}
		public static void UpdateGroupDetails(int paramGroupId,string paramGroup,string paramDescription,string paramSecuredItems,string paramAccess)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.GroupPropertiesDAClass.UpdateGroupDetails(paramGroupId,paramGroup,paramDescription,paramSecuredItems,paramAccess);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetAddedGroupKey(string paramGroupName)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.GroupPropertiesDAClass.GetAddedGroupKey(paramGroupName);
			}
			catch
			{
				throw;
			}
		}
	}
}
