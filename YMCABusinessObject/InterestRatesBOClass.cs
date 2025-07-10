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
using System.Security.Permissions;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for InterestRatesBusinessClass.
	/// </summary>
	public sealed class InterestRatesBOClass
	{
		private InterestRatesBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// function returning Dataset containing all rows of table 'AtsMetaInterestRates' 
		/// from the function of InterestRatesDAClass.cs class
		/// </summary>
		/// <returns></returns>
		public static DataSet LookupInterestRate()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.InterestRatesDAClass.LookupInterestRate());
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// Function returning dataset for the search against 'Acct Type'.
		/// </summary>
		/// <param name="parameterSearchAnnuityBasisTypes"></param>
		/// <returns></returns>
		public static DataSet SearchInterestRates(string parameterInterestRates)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.InterestRatesDAClass.SearchInterestRate(parameterInterestRates));
			}
			catch
			{
				throw;
			}

		}


		/// <summary>
		///	This method Insert values into the table 'AtsMetaProjectedInterestRates' 
		///	on click of Add button followed by save button after entering data in the textboxes of UI
		/// and also Update values into the table 'AtsMetaProjectedInterestRates' 
		///	on click of edit button followed by save button after changing data in the textboxes of UI
		/// </summary>
		/// <param name="parameterAnnuityBasisTypes"></param>
		public static void InsertInterestRate(DataSet parameterInterestRate)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.InterestRatesDAClass.InsertInterestRate(parameterInterestRate);
			}
			catch
			{
				throw;
			}
		}
		

	}
}
