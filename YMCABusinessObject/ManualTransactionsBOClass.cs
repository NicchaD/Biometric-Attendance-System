//'*******************************************************************************
//'Modification History
//'********************************************************************************************************************************
//'Modified By        Date            Description
//'********************************************************************************************************************************
//'Dilip Yadav :      30-July-09 :    To Implement the N-Tier AnnuityBasis Type logic based on Transaction date.
//'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//'********************************************************************************************************************************

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for ManualTransactions.
	/// </summary>
	public sealed class ManualTransactionsBOClass
	{
	
		public static DataSet LookUpParticipants(string paramSSN,string paramFundIdNo,string paramFirstName,string paramLastName)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ManualTransactionsDAClass.LookUpParticipants(paramSSN,paramFundIdNo, paramFirstName, paramLastName);

			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetTransactTypes()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ManualTransactionsDAClass.GetTransactTypes();
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getAcctTypes(string paramPersId,string paramFundEventId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ManualTransactionsDAClass.getAcctTypes(paramPersId,paramFundEventId);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetAccountingDate()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ManualTransactionsDAClass.GetAccountingDate();
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetTransactions(string paramPersId,string paramFundEventId,string paramAnnuityType,string paramAcctType,string paramTransactType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ManualTransactionsDAClass.GetTransactions(paramPersId,paramFundEventId,paramAnnuityType,paramAcctType,paramTransactType);
			}
			catch
			{
				throw;
			}
		}
		public static int SaveTransaction(string paramPersId,string paramYMCAId,string paramFundEventId,string paramAcctType,string paramTransType,string paramBasisType,decimal paramMonthComp,decimal paramPerPreTax,decimal paramperPostTax,decimal paramYMCAPreTax,DateTime paramRecdate,DateTime paramTransdate,DateTime paramFundDate,DateTime paramAcctDate,string paramNotes)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ManualTransactionsDAClass.SaveTransaction(paramPersId,paramYMCAId,paramFundEventId,paramAcctType,paramTransType,paramBasisType,paramMonthComp,paramPerPreTax,paramperPostTax,paramYMCAPreTax,paramRecdate,paramTransdate,paramFundDate,paramAcctDate,paramNotes);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getHistory(string paramPersId, string paramFundEventId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ManualTransactionsDAClass.getHistory(paramPersId,paramFundEventId);
			}
			catch
			{
				throw;
			}
		}
		   // ---- START : Added By Dilip Yadav : 31-July-09 : To Implement N-Tier Annuitybasis logic ---- 
		public static DataSet GetAnnuityBasisType(string paramTansDate, string paramAnnuityGroup)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ManualTransactionsDAClass.GetAnnuityBasisType(paramTansDate,paramAnnuityGroup);
			}
			catch
			{
				throw;
			}
		}
		// ---- END : Added By Dilip Yadav: 31-July-09 : To Implement N-Tier Annuitybasis logic ---- 
		
	}
}
