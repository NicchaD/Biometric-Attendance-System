//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMVA-YRS
// FileName			:	GroupPropertiesDAClass.cs
// Author Name		:	
// Employee ID		:	
// Email				:	
// Contact No		:	
// Creation Time		:	
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
	/// Summary description for GroupPropertiesDAClass.
	/// </summary>
	public sealed class GroupPropertiesDAClass
	{
		public GroupPropertiesDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpGroupProperties(string parameterGroupId,string parameterItemType)
		{
			Database db = null;
			DataSet dsLookUpGroupProperties = null;
			DbCommand  commandLookUpGroupProperties = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandLookUpGroupProperties=db.GetStoredProcCommand ("yrs_usp_VRSI_LookUpGroupProperties");
				if(commandLookUpGroupProperties==null) return null;
				db.AddInParameter(commandLookUpGroupProperties,"@varchar_GroupId",DbType.String,parameterGroupId);
				db.AddInParameter(commandLookUpGroupProperties,"@varchar_ItemTypecd",DbType.String,parameterItemType);
				dsLookUpGroupProperties = new DataSet();
				db.LoadDataSet(commandLookUpGroupProperties,dsLookUpGroupProperties,"GroupProperties");
				System.AppDomain.CurrentDomain.SetData("DataSetGroupProperties",dsLookUpGroupProperties);
				return dsLookUpGroupProperties;
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpSecuredItems(string parameterItemType)
		{
			Database db = null;
			DataSet dsLookUpSecuredItems = null;
			DbCommand  commandLookUpSecuredItems = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandLookUpSecuredItems=db.GetStoredProcCommand ("yrs_usp_VRSIT_LookUpSecuredItems");
				if(commandLookUpSecuredItems== null) return null;
				db.AddInParameter(commandLookUpSecuredItems,"@varchar_ItemTypecd",DbType.String,parameterItemType);
				dsLookUpSecuredItems= new DataSet();
				db.LoadDataSet(commandLookUpSecuredItems,dsLookUpSecuredItems,"SecuredItems");
				System.AppDomain.CurrentDomain.SetData("DataSetSecuredItems",dsLookUpSecuredItems);
				return dsLookUpSecuredItems;

			}
			catch
			{
				throw;
			}
		}
		public static int InsertGroupDetails(string paramGroup,string paramDescription,string paramSecuredItems,string paramAccess)
		{
			Database db = null;
			DbCommand  commandInsertGroupDetails = null;
			int output_err;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				commandInsertGroupDetails=db.GetStoredProcCommand ("yrs_usp_VRSA_InsertGroupDetails");
                db.AddInParameter(commandInsertGroupDetails,"@Varchar_Group",DbType.String,paramGroup);
				db.AddInParameter(commandInsertGroupDetails,"@varchar_Desc",DbType.String,paramDescription);
				db.AddInParameter(commandInsertGroupDetails,"@varchar_SecuredItems",DbType.String,paramSecuredItems);
				db.AddInParameter(commandInsertGroupDetails,"@varchar_Access",DbType.String,paramAccess);
				db.AddOutParameter(commandInsertGroupDetails,"@integer_Output",DbType.Int16,6);
				db.ExecuteNonQuery(commandInsertGroupDetails);
				output_err =Convert.ToInt16(db.GetParameterValue(commandInsertGroupDetails,"@integer_Output"));
				return output_err;
			}
			catch
			{
				throw;
			}
		}

		public static void DeleteGroupDetails(int ParamGroupId)
		{
			Database db = null;
			DbCommand  commandDeleteGroupDetails = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				commandDeleteGroupDetails=db.GetStoredProcCommand ("yrs_usp_VRUG_DeleteGroupDetails");
                db.AddInParameter(commandDeleteGroupDetails,"@integer_GroupKey",DbType.Int32,ParamGroupId);
				db.ExecuteNonQuery(commandDeleteGroupDetails);
			}
			catch
			{
				throw;
			}
		}
		public static void UpdateGroupDetails(int paramGroupId,string paramGroup,string paramDescription,string paramSecuredItems,string paramAccess)
		{
			Database db = null;
			DbCommand  commandUpdateGroupDetails = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				commandUpdateGroupDetails=db.GetStoredProcCommand ("yrs_usp_VRSA_UpdateGroupDetails");
				db.AddInParameter(commandUpdateGroupDetails,"@Integer_GroupId",DbType.Int32,paramGroupId);
				db.AddInParameter(commandUpdateGroupDetails,"@Varchar_Group",DbType.String,paramGroup);
				db.AddInParameter(commandUpdateGroupDetails,"@varchar_Desc",DbType.String,paramDescription);
				db.AddInParameter(commandUpdateGroupDetails,"@varchar_SecuredItems",DbType.String,paramSecuredItems);
				db.AddInParameter(commandUpdateGroupDetails,"@varchar_Access",DbType.String,paramAccess);
				db.ExecuteNonQuery(commandUpdateGroupDetails);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetAddedGroupKey(string paramGroupName)
		{
			Database db = null;
			DbCommand  commandGetAddedGroupKey = null;
			DataSet dsGetAddedGroupKey=null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandGetAddedGroupKey=db.GetStoredProcCommand ("yrs_usp_VRUG_LookUpAddedGroupkey");
				db.AddInParameter(commandGetAddedGroupKey,"@varchar_Group",DbType.String,paramGroupName);
                dsGetAddedGroupKey= new DataSet();
				db.LoadDataSet(commandGetAddedGroupKey,dsGetAddedGroupKey,"GrpKey");
				return dsGetAddedGroupKey;

			}
			catch
			{
				throw;
			}
		}
	}
}
