//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AnnuityPayrollOutSourceDAClass.
	/// </summary>
	public class AnnuityPayrollOutSourceDAClass
	{
		public AnnuityPayrollOutSourceDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet GetLatestPayroll()
		{
			DataSet l_dataset_Payroll = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
    			if (db == null) return null;
    			LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_GetLatesPayrollFile");

				if (LookUpCommandWrapper == null) return null;
				l_dataset_Payroll = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Payroll,"r_MemberListForDeath");
				return l_dataset_Payroll;
			}
			catch 
			{
				throw;
			}
		}
	}
}
