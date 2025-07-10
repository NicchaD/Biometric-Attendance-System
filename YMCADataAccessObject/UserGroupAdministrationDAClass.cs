//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA YRS
// FileName			:	UserGroupAdministrationDAClass.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email				:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time		:	8/24/2005 10:04:07 AM
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
	/// Summary description for UserGroupAdministrationDAClass.
	/// </summary>
	public class UserGroupAdministrationDAClass
	{
		public UserGroupAdministrationDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookupUsers(string parameterUserActive)
		{
			DataSet dsLookUpUsers = null;
			Database db = null;
			DbCommand commandLookUpUsers = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpUsers = db.GetStoredProcCommand("yrs_usp_VRU_LookUpUsers");
				db.AddInParameter(commandLookUpUsers,"@varchar_UserActive",DbType.String,parameterUserActive);
				if (commandLookUpUsers==null) return null;

				dsLookUpUsers = new DataSet();
				dsLookUpUsers.Locale=CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpUsers,dsLookUpUsers,"Users");
				System.AppDomain.CurrentDomain.SetData("DataSetUsers",dsLookUpUsers);
				return dsLookUpUsers;

			}
			catch
			{
				throw;
			}
		
		}
		public static DataSet LookupUserGroups()
		{
			DataSet dsLookupUserGroups = null;
			Database db= null;
			DbCommand commandLookupUserGroups = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookupUserGroups=db.GetStoredProcCommand("yrs_usp_VRU_LookupUserGroups");
				if(commandLookupUserGroups==null) return null;

				dsLookupUserGroups=new DataSet();
				dsLookupUserGroups.Locale=CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookupUserGroups,dsLookupUserGroups,"Groups");
				System.AppDomain.CurrentDomain.SetData("DataSetUserGroups",dsLookupUserGroups);
				return dsLookupUserGroups;
			}
			catch
			{
				throw;
			}
		}
	}
}
