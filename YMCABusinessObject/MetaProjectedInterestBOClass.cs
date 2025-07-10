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
	/// Summary description for MetaProjectedInterest.
	/// </summary>
	public sealed class MetaProjectedInterest
	{
		private MetaProjectedInterest()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// function returning Dataset containing all rows of table 'AtsMetaAnnuityBasisTypes' 
		/// from the function of AnnuityBasisTypesDAClass.cs class
		/// </summary>
		/// <returns></returns>
				public static DataSet LookupProjectedInterestRate()
				{
					try
					{
						return (YMCARET.YmcaDataAccessObject.MetaProjectedInterestDAClass.LookupProjectedInterestRate());
					}
					catch 
					{
						throw;
					}
				}

		/// <summary>
		/// Function returning dataset for the search against 'Annuisty Basis'.
		/// </summary>
		/// <param name="parameterSearchAnnuityBasisTypes"></param>
		/// <returns></returns>
		public static DataSet SearchProjectedInterestRates(string parameterProjectedInterestRates)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaProjectedInterestDAClass.SearchProjectedInterestRate(parameterProjectedInterestRates));
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
		public static void InsertProjectedInterestRate(DataSet parameterProjectedInterestRate)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.MetaProjectedInterestDAClass.InsertProjectedInterestRate(parameterProjectedInterestRate);
			}
			catch
			{
				throw;
			}
		}

	}
}
