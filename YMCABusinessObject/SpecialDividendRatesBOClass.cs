//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for SpecialDividendRatesBOClass.
	/// </summary>
	public class SpecialDividendRatesBOClass
	{
		public SpecialDividendRatesBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet GetExperienceDividendData()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.SpecialDividendRatesDAClass.GetExperienceDividendData());
			}
			catch 
			{
				throw;
			}
		}
		public static void DeleteExpDividendDate(string parameterUniqueId)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.SpecialDividendRatesDAClass.DeleteExpDividendDate(parameterUniqueId);
			}
			catch 
			{
				throw;
			}
		}
		public static void InsertUpdateExperienceDividendData(DataSet parameterdsExpDividendData)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.SpecialDividendRatesDAClass.InsertUpdateExperienceDividendData(parameterdsExpDividendData);			}
			catch
			{

			}
		}

	}
}
