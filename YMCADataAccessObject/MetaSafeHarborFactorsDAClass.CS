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
// Changed on			:	26-07-2005
// Change Description	:	coding
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by         Date             Description
//*******************************************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for MetaSafeHarborFactorsDAClass.
	/// </summary>
	public sealed class MetaSafeHarborFactorsDAClass
	{
		private MetaSafeHarborFactorsDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}



		// function returning Dataset containing all rows of table 'AtsMetaProjectedInterestRates' 
		public static DataSet LookupSafeHarborFactors()
		{
			DataSet dsLookUpSafeHarborFactors = null;
			Database db = null;
			DbCommand commandLookUpSafeHarborFactors = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				commandLookUpSafeHarborFactors = db.GetStoredProcCommand("yrs_usp_AMSHF_LookupSafeHarborFactors");
						
				if (commandLookUpSafeHarborFactors == null) return null;
		
				dsLookUpSafeHarborFactors = new DataSet();
				dsLookUpSafeHarborFactors.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpSafeHarborFactors, dsLookUpSafeHarborFactors,"Safe Harbor Factors");
						
				return dsLookUpSafeHarborFactors;
			}
			catch 
			{
				throw;
			}

		}
		//This method Insert values into the table 'AtsMetaAnnuityBasisTypes' 
		//on click of Add button followed by save button after entering data in the textboxes of UI
		// and also Update values into the table 'AtsMetaAnnuityBasisTypes' 
		//on click of edit button followed by save button after changing data in the textboxes of UI

		public static void InsertSafeHarborFactors(DataSet parameterSafeHarborFactor)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMSHF_InsertSafeHarborFactors");
				
				db.AddInParameter(insertCommandWrapper, "@char_FactorGroup",DbType.String,"Factor Group",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, " @datetime_RetireDateLow",DbType.DateTime," Retire Date Low",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@datetime_RetireDateHigh",DbType.DateTime,"Retire Date High",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@integer_AgeLow",DbType.Int32,"Age Low",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@integer_AgeHigh",DbType.Int32,"Age High",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@integer_Factor",DbType.Int32,"Factor",DataRowVersion.Current);
				

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMSHF_UpdateSafeHarborFactors");
				
				db.AddInParameter(updateCommandWrapper, "@char_FactorGroup",DbType.String,"Factor Group",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper, "@datetime_RetireDateLow",DbType.DateTime," Retire Date Low",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@datetime_RetireDateHigh",DbType.DateTime,"Retire Date High",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@integer_AgeLow",DbType.Int32,"Age Low",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@integer_AgeHigh",DbType.Int32,"Age High",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@integer_Factor",DbType.Int32,"Factor",DataRowVersion.Current);
				
				deleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMSHF_DeleteSafeHarborFactors");
				
				db.AddInParameter(deleteCommandWrapper, "@char_FactorGroup",DbType.String,"Factor Group",DataRowVersion.Original);
				db.AddInParameter(deleteCommandWrapper, "@integer_AgeLow",DbType.Int32,"Age Low",DataRowVersion.Original);
				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterSafeHarborFactor != null)
				{
					db.UpdateDataSet(parameterSafeHarborFactor,"Safe Harbor Factors" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}



	}
}
