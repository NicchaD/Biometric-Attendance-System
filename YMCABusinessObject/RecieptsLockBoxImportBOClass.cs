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
	/// Summary description for RecieptsLockBoxImport.
	/// </summary>
	public class RecieptsLockBoxImport
	{
		public static DataSet CheckFileImported(string paramFileName)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ReceiptsLockBoxImportDAClass.CheckFileImported(paramFileName);
			}
			catch
			{
				throw;
			}
		}
		public static void InsertImportedFile(string paramFileName)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.ReceiptsLockBoxImportDAClass.InsertImportedFile(paramFileName);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpYMCA(string paramYmcaNo)
		{
			try
			{
				return YMCARET.YmcaBusinessObject.CashRecieptsEntry.LookUpYmca(paramYmcaNo);
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
				return YMCARET.YmcaBusinessObject.CashRecieptsEntry.LookUpABANumAcctNum(l_str_ABANum, l_str_AcctNum);
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
				return YMCARET.YmcaBusinessObject.CashRecieptsEntry.LookUpPaymentTypes();
			}
			catch
			{
				throw;
			}
		}
		public static void UpdateProcessLog(string paramFileName)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.ReceiptsLockBoxImportDAClass.UpdateProcessLog(paramFileName);
			}
			catch
			{
				throw;
			}
		}
		public static void InsertRecieptLockBox(string paramYMCAId,string paramSourceCode,decimal paramMnyAmount,DateTime paramRecieveDate,string paramRecieptId,DateTime paramRecieptIdDate,string paramComments)
		{
			try
			{
				YMCARET.YmcaBusinessObject.CashRecieptsEntry.InsertCashReciept(paramYMCAId,paramSourceCode,paramMnyAmount,paramRecieveDate,paramRecieptId,paramRecieptIdDate,paramComments);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getMetaOutputFileType()
		{
			try
			{
			return YMCARET.YmcaDataAccessObject.ReceiptsLockBoxImportDAClass.getMetaOutputFileType();
			}
			catch
			{
				throw;
			}
			
		}
	}
}
