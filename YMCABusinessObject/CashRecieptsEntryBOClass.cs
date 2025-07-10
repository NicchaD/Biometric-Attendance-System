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
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for CashRecieptsEntry.
	/// </summary>
	public sealed class CashRecieptsEntry
	{
		public static DataSet LookUpYmca(string paramYmcaNo)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.CashReceiptsEntryDAClass.LookUpYmca(paramYmcaNo);
			}
			catch
			{
				throw;
			}
		}
		//Start- Added by Pankaj on 12th April 2006
		public static DataSet LookUpABANumAcctNum(string l_str_ABANum, string l_str_AcctNum)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.CashReceiptsEntryDAClass.LookUpABANumAcctNum(l_str_ABANum, l_str_AcctNum);
			}
			catch
			{
				throw;
			}
		}
		//End- Added by Pankaj on 12th April 2006
		public static DataSet LookUpPaymentTypes()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.CashReceiptsEntryDAClass.LookUpPaymentTypes();
			}
			catch
			{
				throw;
			}
		}
		public static void InsertCashReciept(string paramYMCAId,string paramSourceCode,decimal paramMnyAmount,DateTime paramRecieveDate,string paramRecieptId,DateTime paramRecieptIdDate,string paramComments)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.CashReceiptsEntryDAClass.InsertCashReciept(paramYMCAId,paramSourceCode,paramMnyAmount,paramRecieveDate,paramRecieptId,paramRecieptIdDate,paramComments);
			}
			catch
			{
				throw;
			}
		}
	}
}
