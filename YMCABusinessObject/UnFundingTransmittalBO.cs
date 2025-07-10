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
	/// Summary description for UnFundingTransmittalFormBO.
	/// </summary>
	public class UnFundingTransmittalBO
	{
		public UnFundingTransmittalBO()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpYmcaTransmittals(int FunctionType,string parameterYMCANo,string parameterTransmittalNo,string parameterReceiptNo,DateTime TransmittalSDate,DateTime TransmittalEDate)

		{
			
			try
			{
				return UnFundingTransmittalDA.LookUpYmcaTransmittals(FunctionType,parameterYMCANo,parameterTransmittalNo,parameterReceiptNo,TransmittalSDate,TransmittalEDate);
			}	
			catch
			{
				throw;
			}
		}
		
		public static void SaveUnFundingTransmittals(string parameterYmcaId,string parameterUniqueId,double parameterAmount,string parameterTransmittalNo)
		{
			try
			{
				UnFundingTransmittalDA.SaveUnFundingTransmittals(parameterYmcaId, parameterUniqueId,parameterAmount,parameterTransmittalNo);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Created By : Paramesh K.
		/// Created On : Sept 19th 2008
		/// This method will check any UEIN Transmittal Exists
		/// </summary>
		/// <param name="transmittalUniqueId">transmittal ID</param>
		/// <returns> integer value </returns>
		public static int CheckUEINTransmittalExists(string transmittalUniqueId)
		{
			try
			{
				return UnFundingTransmittalDA.CheckUEINTransmittalExists(transmittalUniqueId);
			}
			catch
			{
				throw;
			}
		}

		public static int CheckAcctBalance(string parameterUniqueId)
		{
			try
			{
				return UnFundingTransmittalDA.CheckAcctBalance(parameterUniqueId);
			}
			catch
			{
				throw;
			}
		}

		//Swopna11Aug08
		public static int CheckAppliedReceiptsCredits(string parameterTransmittalNo)
		{
			try
			{
				return UnFundingTransmittalDA.CheckAppliedReceiptsCredits(parameterTransmittalNo);
			}
			catch
			{
				throw;
			}
		}
		//Swopna11Aug08
		public static DataSet ValidateTransmittal(string parameterUniqueId)
		{
			try
			{
				return UnFundingTransmittalDA.ValidateTransmittal(parameterUniqueId);
			}
			catch
			{
				throw;
			}
		}
		
		public static void DeleteYmcaTransmittals(DataTable parameterDeleteTransmittals)
		{
			try
			{
				UnFundingTransmittalDA.DeleteYmcaTransmittals(parameterDeleteTransmittals);
			}
			catch
			{
				throw;
			}
		}
		
	}
}
