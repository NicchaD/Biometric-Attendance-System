//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	Ymca-YRS
// FileName			:	AddAdditionalAccountsBOClass.cs
// Author Name		:	Ruchi saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	10/13/2005 11:46:58 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified By			Date                Description
//Shagufta Chaudhari    2011.08.04          For BT-893 , YRS 5.0-1360 : Allow Lump Sum additional accts record
//prasad jadhav			2012.03.15          For (REOPEN)BT-990,YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************************************************************
using System.Data;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for AddAdditionalAccountsBOClass.
	/// </summary>
	public class AddAdditionalAccountsBOClass
	{
		public AddAdditionalAccountsBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <param name="parameterFundId"></param>
		/// <returns></returns>
		public static DataSet GetYmcaList(string parameterPersId,string parameterFundId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddAdditionalAccountsDAClass.GetYmcaList(parameterPersId, parameterFundId);
			}
			catch
			{
				throw;
			}
		}

		public static string AddAdditionalAccount(string parameterEmpEventId,string parameterYmcaId,string parameterAcctType,string parameterBasisCode,string parameterAddlPctg,string parameterEffDate,string parameterAddlContrib,string parameterTermDate)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddAdditionalAccountsDAClass.AddAdditionalAccount(parameterEmpEventId, parameterYmcaId,parameterAcctType,parameterBasisCode,parameterAddlPctg, parameterEffDate,parameterAddlContrib,parameterTermDate);
			}
			catch
			{
				throw;
			}
		}

		public static string UpdateAdditionalAccount(string parameterUniqueId,string parameterTermDate)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddAdditionalAccountsDAClass.UpdateAdditionalAccount(parameterUniqueId, parameterTermDate);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetAdditionalAccount(string parameterUniqueId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddAdditionalAccountsDAClass.GetAdditionalAccount(parameterUniqueId);
			}
			catch
			{
				throw;
			}
		}
		
		public static string ValidateDateForTDRenewal(string lcPersID , string dtmEffDate)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddAdditionalAccountsDAClass.ValidateDateForTDRenewal(lcPersID,dtmEffDate);
			}
			catch
			{
				throw;
			}
		}


        //Shagufta Chaudhari:2011.08.04:BT-893 
        public static string IsValidPayrollDateOverlap(string parameterYmcaId, string payrolldate)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.AddAdditionalAccountsDAClass.IsValidPayrollDateOverlap(parameterYmcaId, payrolldate);
            }
            catch
            {
                throw;
            }
        }
        //End: SC:2011.08.04:BT-893
		//Added by prasad 2012.03.15 For (REOPEN)BT-990,YRS 5.0-1531: Need to be able to update atsEmpElectives termination date
		public static string TdAccountDayLimit()
		{
			return YMCARET.YmcaDataAccessObject.AddAdditionalAccountsDAClass.TdAccountDayLimit();
		}

	}
}
