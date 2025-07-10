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
	/// Summary description for MetaSafeHarborFactorsBOClass.
	/// </summary>
	public sealed class MetaSafeHarborFactorsBOClass
	{
		private MetaSafeHarborFactorsBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// function returning Dataset containing all rows of table 'AtsMetaSafeHarborFactors' 
		/// from the function of MetaSafeHarborFactorsDAClass.CS class
		/// </summary>
		/// <returns></returns>
		public static DataSet LookupSafeHarborFactors()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaSafeHarborFactorsDAClass.LookupSafeHarborFactors());
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		///	This method Insert values into the table 'AtsMetaSafeHarborFactors' 
		///	on click of Add button followed by save button after entering data in the textboxes of UI
		/// and also Update values into the table 'AtsMetaSafeHarborFactors' 
		///	on click of edit button followed by save button after changing data in the textboxes of UI
		/// </summary>
		/// <param name="parameterAnnuityBasisTypes"></param>
		public static void InsertSafeHarborFactors(DataSet parameterSafeHarborFactors)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.MetaSafeHarborFactorsDAClass.InsertSafeHarborFactors(parameterSafeHarborFactors);
			}
			catch
			{
				throw;
			}
		}

	}
}
