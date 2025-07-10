//****************************************************
//Modification History
//****************************************************
//Modified by         Date           Description
//****************************************************
//Shubhrata           05/17/2007     Added a new method(GetAccountGroups) to fetch Account Groups from atsMetaAcctGroups
//Nikunj Patel		  2010.06.17	 Removing the use of a stored procedure by making the lookup and search functions common
//Manthan Rajguru     2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AccountTypesDAClass.
	/// </summary>
	public sealed class AccountTypesDAClass
	{
		private AccountTypesDAClass()
		{
		}

		//function returning dataset for the search against 'Account Types'.
		//The DataSet contains the rows where Account Types= TextBoxVar + "%" of table 'AtsMetaAcctTypes' 
		public static DataSet LookUpAccountType()
		{
			return SearchAccountType(string.Empty);
		}

		public static DataSet SearchAccountType(string parameterSearchAccountType)
		{
			DataSet dsSearchAccountType = null;
			Database db = null;
			DbCommand CommandSearchAccountType = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;

				CommandSearchAccountType = db.GetStoredProcCommand("yrs_usp_AMAT_SearchAccountTypes");
				if (CommandSearchAccountType ==null) return null;

				db.AddInParameter(CommandSearchAccountType, "@char_AccountType",DbType.String,parameterSearchAccountType);
				
				dsSearchAccountType = new DataSet();
				dsSearchAccountType.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(CommandSearchAccountType,dsSearchAccountType,"Account Type");

				return dsSearchAccountType;
			}
			catch
			{
				throw;
			}

		}

		//This method Insert values into the table 'AtsMetaAcctTypes' 
		//on click of Add button followed by save button after entering data in the textboxes of UI
		// and also Update values into the table 'AtsMetaAcctTypes' 
		//on click of edit button followed by save button after changing data in the textboxes of UI

		public static void SaveAccountType(DataSet parameterAccountType)
		{
			Database db= null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
	
				insertCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMAT_InsertAccountTypes");
				// Defining The Insert Command Wrapper With parameters
				db.AddInParameter(insertCommandWrapper, "@varchar_AcctType",DbType.String, "Acct. Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short Description",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Description",DbType.String," Long Desc",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@datetime_EffDate",DbType.String,"Effec. Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@datetime_TerminationDatet",DbType.String,"Termination Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@int_RefundPriorityLevel",DbType.Int32,"Refund Priority Level",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_BasicAcct",DbType.Int16,"Basic Acct",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_VestingRequired",DbType.Int16,"Vesting Required",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_SalaryRequired",DbType.Int16,"Salary Required",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_LumpSumsOK",DbType.Int16,"Lump Sum",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_EmployerMoney",DbType.Int16,"Employer Money",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_EmployeeMoney",DbType.Int16,"Employee Money",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_EmployeeTaxDefer",DbType.Int16,"Employer Tax Defer",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_IncludeForDeathBenefit",DbType.Int16,"Included Death Bene.",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_PlanType",DbType.String,"PlanType",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_AcctGroups",DbType.String,"AcctGroups",DataRowVersion.Current);
																												
				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMAT_UpdateAccountTypes");
				// Defining The Update Command Wrapper With parameters
				db.AddInParameter(updateCommandWrapper, "@varchar_AcctType",DbType.String, "Acct. Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short Description",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_Description",DbType.String," Long Desc",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@datetime_EffDate",DbType.String,"Effec. Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@datetime_TerminationDate",DbType.String,"Termination Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@int_RefundPriorityLevel",DbType.Int32,"Refund Priority Level",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_BasicAcct",DbType.Int16,"Basic Acct",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_VestingRequired",DbType.Int16,"Vesting Required",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_SalaryRequired",DbType.Int16,"Salary Required",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_LumpSumsOK",DbType.Int16,"Lump Sum",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_EmployerMoney",DbType.Int16,"Employer Money",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_EmployeeMoney",DbType.Int16,"Employee Money",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_EmployeeTaxDefer",DbType.Int16,"Employer Tax Defer",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_IncludeForDeathBenefit",DbType.Int16,"Included Death Bene.",DataRowVersion.Current);
						
				deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMAT_DeleteAccountTypes");
				// Defining The Delete Command Wrapper With parameters
				db.AddInParameter(deleteCommandWrapper, "@varchar_AcctType",DbType.String,"Acct. Type",DataRowVersion.Original);
								
				if (parameterAccountType != null)
				{
					db.UpdateDataSet(parameterAccountType,"Account Type" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}

			}
			catch
			{
				throw;
			}
		}
		//Shubhrata Plan Split Changes May 17th,2007
		#region "Plan Split Changes"
		public static DataSet GetAccountGroups()
		{
			DataSet l_dataset_AccountGroups = null;
			Database db= null;
			DbCommand GetCommandWrapper = null;
			 
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				GetCommandWrapper=db.GetStoredProcCommand("dbo.yrs_usp_AMAT_SelectAccountGroups");
				GetCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]); 
				if (GetCommandWrapper == null) return null;
				l_dataset_AccountGroups = new DataSet();
				db.LoadDataSet(GetCommandWrapper, l_dataset_AccountGroups,"MetaAccountGroups");
				
				return l_dataset_AccountGroups;

			}
			catch
			{
				throw;
			}
		}
		#endregion "Plan Split Changes" 
		//Shubhrata Plan Split Changes May 17th,2007
	}
}
