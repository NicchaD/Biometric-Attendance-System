//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	CashReceiptsEntryDAClass.cs
// Author Name		:	Vartika Jain
// Employee ID		:	33495
// Email			:	vartika.jain@3i-infotech.com
// Contact No		:	8733
// Creation Time		:	9/29/2005 12:30:40 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by          Date          Description
//*******************************************************************************
//Manthan Rajguru     2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for CashReceiptsEntryDAClass.
	/// </summary>
	public sealed class CashReceiptsEntryDAClass
	{
		public static DataSet LookUpYmca(string paramYmcaNo)
		{	
			Database db= null;
			DbCommand commandLookUpYmca = null;
			DataSet dsLookUpYmca = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpYmca = db.GetStoredProcCommand("yrs_usp_AY_SelectYmca");
				if(commandLookUpYmca==null) return null;
				db.AddInParameter(commandLookUpYmca,"@varchar_YmcaNo",DbType.String,paramYmcaNo);
				dsLookUpYmca= new DataSet();
				db.LoadDataSet(commandLookUpYmca,dsLookUpYmca,"ay_selectYmca");
				return dsLookUpYmca;
			}
			catch
			{
				throw;
			}
		}
		//Start - Added by Pankaj on 12th April 2006
		public static DataSet LookUpABANumAcctNum(string l_str_ABANum, string l_str_AcctNum)
		{
			Database db= null;
			DbCommand commandLookUpABANumAcctNum = null;
			DataSet dsLookUpABANumAcctNum = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpABANumAcctNum = db.GetStoredProcCommand("yrs_usp_Lock_SelectYmcaNumAndName");
				if(commandLookUpABANumAcctNum==null) return null;
				db.AddInParameter(commandLookUpABANumAcctNum,"@varchar_YmcaABANum",DbType.String,l_str_ABANum);
				db.AddInParameter(commandLookUpABANumAcctNum,"@varchar_YmcaAcctNum",DbType.String,l_str_AcctNum);
				dsLookUpABANumAcctNum= new DataSet();
				db.LoadDataSet(commandLookUpABANumAcctNum,dsLookUpABANumAcctNum,"ay_selectYmca");
				return dsLookUpABANumAcctNum;
			}
			catch
			{
				throw;
			}
		
		}
		//End - Added by Pankaj on 12th April 2006

		public static DataSet LookUpPaymentTypes()
		{
			Database db= null;
			DbCommand commandLookUpPaymentTypes = null;
			DataSet dsLookUpPaymentTypes = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandLookUpPaymentTypes = db.GetStoredProcCommand("yrs_usp_AML_LookupPaymentTypes");
				if(commandLookUpPaymentTypes==null) return null;
				dsLookUpPaymentTypes = new DataSet();
				db.LoadDataSet(commandLookUpPaymentTypes,dsLookUpPaymentTypes,"PaymentTypes");
				System.AppDomain.CurrentDomain.SetData("dataSet_PaymentTypes",dsLookUpPaymentTypes);
				return dsLookUpPaymentTypes;
			}
			catch
			{
				throw;
			}
		}
		public static void InsertCashReciept(string paramYMCAId,string paramSourceCode,decimal paramMnyAmount,DateTime paramRecieveDate,string paramRecieptId,DateTime paramRecieptIdDate,string paramComments)
		{
			Database db= null;
			DbCommand commandInsertCashReciept = null;
			
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				commandInsertCashReciept=db.GetStoredProcCommand("yrs_usp_AYR_InsertReciepts");
				
				db.AddInParameter(commandInsertCashReciept,"@varchar_YMCAId",DbType.String,paramYMCAId);
				db.AddInParameter(commandInsertCashReciept,"@varchar_SourceCode",DbType.String,paramSourceCode);
				db.AddInParameter(commandInsertCashReciept,"@numeric_mnyAmount",DbType.Decimal,paramMnyAmount);
				db.AddInParameter(commandInsertCashReciept,"@datetime_RecieveDate",DbType.DateTime,paramRecieveDate);
				db.AddInParameter(commandInsertCashReciept,"@varchar_RecieptId",DbType.String,paramRecieptId);
				db.AddInParameter(commandInsertCashReciept,"@datetime_recieptIdDate",DbType.DateTime,paramRecieptIdDate);
				db.AddInParameter(commandInsertCashReciept,"@varchar_comments",DbType.String,paramComments);

				db.ExecuteNonQuery(commandInsertCashReciept);
			}
			catch
			{
				throw;
			}
		}

	}
}
