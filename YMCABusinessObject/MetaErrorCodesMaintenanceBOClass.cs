// Project Name		:	33156	
// FileName			:	RetirementEstimateUIProcess.cs
// Author Name		:	Sameer joshi	
// Employee ID		:	33156
// Email			:	sameer.joshi@3i-infotech.com
// Contact No		:	55928743
// Creation Time	:	6/1/2005 7:28:12 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//
// Changed by			:	Shefali Bharti
// Changed on			:	28-07-2005
// Change Description	:	Coding
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by          Date          Description
//*******************************************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
using System.Security.Permissions;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for MetaErrorCodesMaintenanceBOClass.
	/// </summary>
	public sealed class MetaErrorCodesMaintenanceBOClass
	{
		private MetaErrorCodesMaintenanceBOClass()
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
		public static DataSet LookupErrorCodes()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaErrorCodesMaintenanceDAClass.LookupErrorCodes());
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet SearchErrorCodes(string parameterErrorCodes)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaErrorCodesMaintenanceDAClass.SearchErrorCodes(parameterErrorCodes));
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
		public static void InsertErrorCodes(DataSet parameterErrorCodes)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.MetaErrorCodesMaintenanceDAClass.InsertErrorCodes(parameterErrorCodes);
			}
			catch
			{
				throw;
			}
		}
	}
}
