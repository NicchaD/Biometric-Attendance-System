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
	/// Summary description for AnnuityBasisTypesBOClass.
	/// </summary>
	public sealed class AnnuityBasisTypes
	{
		/// <summary>
		/// Blank constructor.
		/// </summary>
		private AnnuityBasisTypes()
		{
		}

		/// <summary>
		/// function returning Dataset containing all rows of table 'AtsMetaAnnuityBasisTypes' 
		/// from the function of AnnuityBasisTypesDAClass.cs class
		/// </summary>
		/// <returns></returns>
//		public static DataSet LookupAnnuityBasisTypes()
//		{
//			try
//			{
//				return (YMCARET.YmcaDataAccessObject.AnnuityBasisTypesDAClass.LookupAnnuityBasisTypes ());
//			}
//			catch 
//			{
//				throw;
//			}
//		}

		
		/// <summary>
		/// Function returning dataset for the search against 'Annuisty Basis'.
		/// </summary>
		/// <param name="parameterSearchAnnuityBasisTypes"></param>
		/// <returns></returns>
		public static DataSet SearchAnnuityBasisTypes(string parameterSearchAnnuityBasisTypes)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.AnnuityBasisTypesDAClass.SearchAnnuityBasisTypes (parameterSearchAnnuityBasisTypes));
			}
			catch
			{
				throw;
			}

		}


		/// <summary>
		///	This method Insert values into the table 'AtsMetaAnnuityBasisTypes' 
		///	on click of Add button followed by save button after entering data in the textboxes of UI
		/// and also Update values into the table 'AtsMetaAnnuityBasisTypes' 
		///	on click of edit button followed by save button after changing data in the textboxes of UI
		/// </summary>
		/// <param name="parameterAnnuityBasisTypes"></param>
		public static void InsertAnnuityBasisTypes(DataSet parameterAnnuityBasisTypes)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.AnnuityBasisTypesDAClass.InsertAnnuityBasisTypes (parameterAnnuityBasisTypes);
			}
			catch
			{
				throw;
			}
		}

	}
}

