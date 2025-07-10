//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System.Data;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for VDReplaceDisbursementsBOClass.
	/// </summary>
	public sealed class VDReplaceDisbursementsBOClass
	{
		public VDReplaceDisbursementsBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static VDReplaceDisbursementsBOClass GetInstance()
		{
			
			return Nested.instance;
		}
    
		class Nested
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested()
			{
			}

			internal static readonly VDReplaceDisbursementsBOClass instance = new VDReplaceDisbursementsBOClass();
		}

		public static DataSet LookUpAddress(string parameterEntityId)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VDReplaceDisbursementsDAClass.LookUpAddress(parameterEntityId));
			}
			catch
			{
				throw;
			}
			
		}
		public static DataSet LookUpLatestAddress(string parameterEntityId)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VDReplaceDisbursementsDAClass.LookUpLatestAddress(parameterEntityId));
			}
			catch
			{
				throw;
			}
			
		}

		public static string ReplaceDisbursement(string parameterDisbid,string parameterAddId)
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.VDReplaceDisbursementsDAClass.ReplaceDisbursement(parameterDisbid,parameterAddId));
			}
			catch
			{
				throw;
			}
		}
		public static string ReplaceLoan(string parameterDisbId,string parameterAddId)
		{
			try
			{
				return  YMCARET.YmcaDataAccessObject.VDReplaceDisbursementsDAClass.ReplaceLoan(parameterDisbId,parameterAddId);
			}
			catch
			{
				throw;
			}
		}
		public static string ReplaceCashOuts(string parameterDisbId,string parameterAddId,int parameterReplaceFees,out int parameterZeroFundingStatus)
		{
			try
			{
				return  YMCARET.YmcaDataAccessObject.VDReplaceDisbursementsDAClass.ReplaceCashOuts(parameterDisbId,parameterAddId,parameterReplaceFees,out parameterZeroFundingStatus);
			}
			catch
			{
				throw;
			}
		}
	}
}
