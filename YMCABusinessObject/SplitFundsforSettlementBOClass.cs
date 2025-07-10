//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using YMCARET.YmcaDataAccessObject;
using System.Data;
using System.Data.SqlClient;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for SplitFundsforSettlement.
	/// </summary>
	public class SplitFundsforSettlement
	{
		public SplitFundsforSettlement()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUp_BS_MemberListForDeceased(string paramSSNo, string paramLastName, string paramFirstName, string paramFundNo)
		{
			//DataSet d = new DataSet();
			//return d;
			try
			{
				return (YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_MemberListForDeceased(paramSSNo, paramLastName, paramFirstName, paramFundNo));
				//return (YMCARET.YmcaDataAccessObject.BeneficiarySettlementDAClass.LookUp_Beneficiaries_MemberListDeseased(paramSSNo, paramLastName, paramFirstName, paramFundNo)) ;	

			}
			catch 
			{
				throw;
			}
		}
		//PRIYA AS ON 04-08-08
		public static DataSet LookUp_BS_BeneficiariesPrimeSettle(string paramPersID,string paramStatus)
		{
			try
			{
				return YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_BeneficiariesPrimeSettle(paramPersID,paramStatus);

			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUp_DeceasedInformation(string paramPersID,DateTime paramDeathDate,string paramActualSplit, string FundEventID,ref string errorMsg)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.SplitFundsforSettlementDAClass.LookUp_DeceasedInformation(paramPersID,paramDeathDate,paramActualSplit,FundEventID,ref errorMsg ));

			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUp_MemberListForSplitFunds(string parameterSSN, string parameterLName ,string parameterFName, string parameterFundNo)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.SplitFundsforSettlementDAClass.LookUp_MemberListForSplitFunds(parameterSSN, parameterLName ,parameterFName, parameterFundNo));
			}
			catch
			{
				throw;
			}

		}
		public static string LookUp_FundEventIdForSplitFunds(string paramPersId)
		{
			try
			{
				return  YMCARET.YmcaDataAccessObject.SplitFundsforSettlementDAClass.LookUp_FundEventIdForSplitFunds(paramPersId);

					   
			}
			catch
			{
				throw;
			}
		}
		//END PRIYA
	}
}
