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
	/// Summary description for RolloverReciept.
	/// </summary>
	public class RolloverRecieptBOClass
	{
		public RolloverRecieptBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public static DataSet GetRolloverData(string parameterPersId, string parameterRolloverId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RollOverReceiptsDAClass.GetRolloverData(parameterPersId,parameterRolloverId);
			}
			catch
			{
				throw;	
			}
		}

		public static DataSet GetRolloverRcptsData( string parameterRolloverId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RollOverReceiptsDAClass.GetRolloverRcptsData(parameterRolloverId);
			}
			catch
			{
				throw;
			}

		}

		public static string AddRolloverData( string paramPersId,string paramFundId,string paramYmcaId,string paramRollId,string paramCheckNum,string paramCheckDate,string paramCheckReceivedDate,string paramTaxable,string paramNonTaxable)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RollOverReceiptsDAClass.AddRolloverData( paramPersId, paramFundId, paramYmcaId, paramRollId, paramCheckNum, paramCheckDate, paramCheckReceivedDate, paramTaxable, paramNonTaxable);
			}
			catch
			{
				throw;
			}
			
		}

		//Priya 24-April-2009 YRS 5.0-738 : Please use Account date instead of Calender date for roll in checks validation.
		public static string GetAccountDate()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RollOverReceiptsDAClass.GetAccountDate();
			}
			catch
			{
				throw;
			}
		}
	}
}
