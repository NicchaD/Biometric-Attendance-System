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
using System.Globalization;
using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for ManualTransactionsDAClass.
	/// </summary>
	public sealed class ManualTransactionsDAClass
	{
		public ManualTransactionsDAClass()
		{
		}
		public static DataSet LookUpParticipants(string paramSSN,string paramFundIdNo,string paramFirstName,string paramLastName)
		{
			Database db = null;
			DataSet dsLookUpParticipants = null;
			DbCommand commandLookUpParticipants = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if (db==null)return null;
				commandLookUpParticipants=db.GetStoredProcCommand("ap_DentrySearchParticipants");
				if(commandLookUpParticipants==null) return null;
				db.AddInParameter(commandLookUpParticipants,"@sSSN",DbType.String,paramSSN);
				db.AddInParameter(commandLookUpParticipants,"@sFundIDNo",DbType.String,paramFundIdNo);
				db.AddInParameter(commandLookUpParticipants,"@sFName",DbType.String,paramFirstName);
				db.AddInParameter(commandLookUpParticipants,"@sLName",DbType.String,paramLastName);
				commandLookUpParticipants.CommandTimeout=1200;
				dsLookUpParticipants=new DataSet();
				dsLookUpParticipants.Locale=CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpParticipants,dsLookUpParticipants,"ManualTransactParticipants");
				System.AppDomain.CurrentDomain.SetData("ParticipantManualTransacts",dsLookUpParticipants);
				return dsLookUpParticipants;
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetTransactTypes()
		{
			Database db=null;
			DbCommand commandGetTransactTypes=null;
			DataSet dsTransactTypes = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
				commandGetTransactTypes=db.GetStoredProcCommand("yrs_usp_AMTT_LookUpTransactTypes");
				dsTransactTypes=new DataSet();
				db.LoadDataSet(commandGetTransactTypes,dsTransactTypes,"Transact_Types");
				System.AppDomain.CurrentDomain.SetData("DatasetTransactTypes",dsTransactTypes);
				return dsTransactTypes;
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getAcctTypes(string paramPersId,string paramFundEventId)
		{
			Database db=null;
			DbCommand commandGetAcctTypes=null;
			DataSet dsAcctTypes = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandGetAcctTypes=db.GetStoredProcCommand("yrs_usp_ATAMAT_GetAccountTypes");
				if (commandGetAcctTypes==null) return null;
				db.AddInParameter(commandGetAcctTypes,"@Unique_PersID",DbType.String,paramPersId);
				db.AddInParameter(commandGetAcctTypes,"@Unique_FundEventID",DbType.String,paramFundEventId);
				dsAcctTypes=new DataSet();
				db.LoadDataSet(commandGetAcctTypes,dsAcctTypes,"AcctTypes");
				System.AppDomain.CurrentDomain.SetData("DataSetAcctTypes",dsAcctTypes);
				return dsAcctTypes;
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetAccountingDate()
		{
			Database db=null;
			DbCommand commandGetAccountingDate=null;
			DataSet dsGetAcctDate = null; 
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandGetAccountingDate = db.GetStoredProcCommand("yrs_usp_FNGA_GetAccountingDate");
				if (commandGetAccountingDate==null) return null;
				dsGetAcctDate= new DataSet();
				db.LoadDataSet(commandGetAccountingDate,dsGetAcctDate,"Acct_Date");
				System.AppDomain.CurrentDomain.SetData("DataSetAcctDate",dsGetAcctDate);
				return dsGetAcctDate;

			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetTransactions(string paramPersId,string paramFundEventId,string paramAnnuityType,string paramAcctType,string paramTransactType)
		{
			Database db=null;
			DbCommand commandGetTransactions=null;
			DataSet dsGetTransactions = null; 
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null) return null;
				commandGetTransactions=db.GetStoredProcCommand("yrs_usp_ADGT_GetTransactions");
				if (commandGetTransactions==null) return null;
				db.AddInParameter(commandGetTransactions,"@PersID",DbType.String,paramPersId);
                db.AddInParameter(commandGetTransactions, "@FundEventID", DbType.String, paramFundEventId);
                db.AddInParameter(commandGetTransactions, "@AnnuityBasisType", DbType.String, paramAnnuityType);
                db.AddInParameter(commandGetTransactions, "@AcctType", DbType.String, paramAcctType);
                db.AddInParameter(commandGetTransactions, "@TransactType", DbType.String, paramTransactType);
				dsGetTransactions=new DataSet();
				db.LoadDataSet(commandGetTransactions,dsGetTransactions,"Transactions");
				System.AppDomain.CurrentDomain.SetData("DataSetTransactions",dsGetTransactions);
				return dsGetTransactions;
			}
			catch
			{
				throw;
			}
		}
																			
		public static int SaveTransaction(string paramPersId,string paramYMCAId,string paramFundEventId,string paramAcctType,string paramTransType,string paramBasisType,decimal paramMonthComp,decimal paramPerPreTax,decimal paramperPostTax,decimal paramYMCAPreTax,DateTime paramRecdate,DateTime paramTransdate,DateTime paramFundDate,DateTime paramAcctDate,string paramNotes)
		{
			Database db= null;
			DbCommand commandSaveTransacts = null;
			int l_integer_Output ;

			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				
				commandSaveTransacts=db.GetStoredProcCommand("ap_DentrySaveTransacts");
			
				db.AddInParameter(commandSaveTransacts,"@PersID",DbType.String,paramPersId);
				db.AddInParameter(commandSaveTransacts,"@YMCAID",DbType.String,paramYMCAId);
				db.AddInParameter(commandSaveTransacts,"@FundEventId",DbType.String,paramFundEventId);

				db.AddInParameter(commandSaveTransacts,"@AcctType",DbType.String,paramAcctType);
				db.AddInParameter(commandSaveTransacts,"@TransType",DbType.String,paramTransType);
				db.AddInParameter(commandSaveTransacts,"@BasisType",DbType.String,paramBasisType);
				
				db.AddInParameter(commandSaveTransacts,"@MonthComp",DbType.Decimal,paramMonthComp);
				db.AddInParameter(commandSaveTransacts,"@PerPreTax",DbType.Decimal,paramPerPreTax);
				db.AddInParameter(commandSaveTransacts,"@PerPostTax",DbType.Decimal,paramperPostTax);
				db.AddInParameter(commandSaveTransacts,"@YMCAPreTax",DbType.Decimal,paramYMCAPreTax);

				db.AddInParameter(commandSaveTransacts,"@RecDate",DbType.DateTime,paramRecdate);
				db.AddInParameter(commandSaveTransacts,"@TransDate",DbType.DateTime,paramTransdate);
				db.AddInParameter(commandSaveTransacts,"@FundDate",DbType.DateTime,paramFundDate);
				db.AddInParameter(commandSaveTransacts,"@AcctDate",DbType.DateTime,paramAcctDate);
				db.AddInParameter(commandSaveTransacts,"@Notes",DbType.String,paramNotes);
				db.AddOutParameter(commandSaveTransacts,"@Output",DbType.Int32,9);
				
				db.ExecuteNonQuery(commandSaveTransacts);

				l_integer_Output = Convert.ToInt32(db.GetParameterValue(commandSaveTransacts,"@Output"));
				
				return l_integer_Output;
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getHistory(string paramPersId, string paramFundEventId)
		{
			Database db=null;
			DbCommand commandGetHistory = null;
			DataSet dsGetHistory = null; 
			try
			{

				db=DatabaseFactory.CreateDatabase("YRS");
				if(db == null) return null;
				commandGetHistory=db.GetStoredProcCommand("ap_DentryGetTransactsNotes");
				if(commandGetHistory == null) return null;
				db.AddInParameter(commandGetHistory,"@PersID",DbType.String,paramPersId);
				db.AddInParameter(commandGetHistory,"@FundEventID",DbType.String,paramFundEventId);
				dsGetHistory=new DataSet();
				db.LoadDataSet(commandGetHistory,dsGetHistory,"History");
				System.AppDomain.CurrentDomain.SetData("DataSet_History",dsGetHistory);
				return dsGetHistory;
		
			}
			catch
			{
				throw;
			}
		}
		
		// ---- START : Added By Dilip Y : 31-July-09 : To Implement N-Tier Annuitybasis logic ----
		     #region "GetAnnuityBasisType"	
		public static DataSet GetAnnuityBasisType(string paramTansDate, string paramAnnuityGroup)
		{
           	Database db = null;
			DbCommand commandGetAnnuityBasisType = null;
			DataSet dsAnnuityBasisType = null;

			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db==null) return null;
             	commandGetAnnuityBasisType = db.GetStoredProcCommand("yrs_usp_GetAnnuityBasisType");
				if(commandGetAnnuityBasisType==null) return null;
                db.AddInParameter(commandGetAnnuityBasisType,"@TransDate",DbType.String,paramTansDate);
                db.AddInParameter(commandGetAnnuityBasisType, "@AnnuityGroup", DbType.String, paramAnnuityGroup);
				dsAnnuityBasisType = new DataSet();
				db.LoadDataSet(commandGetAnnuityBasisType,dsAnnuityBasisType,"Annuity_Types");
				//System.AppDomain.CurrentDomain.SetData("DatasetAnnuityType",dsAnnuityBasisType);
				return dsAnnuityBasisType;
			}
			catch
			{
				throw;
			}

		}
		#endregion		
		 // ---- END : Added By Dilip Y : 31-July-09 : To Implement N-Tier Annuitybasis logic ---- 

	}
}
