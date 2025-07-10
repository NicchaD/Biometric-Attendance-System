//****************************************************
//Modification History
//****************************************************
//Modified by         Date             Description
//****************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for MetaProjectedInterestDAClass.
	/// </summary>
	public sealed class MetaProjectedInterestDAClass
	{
		private MetaProjectedInterestDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// function returning Dataset containing all rows of table 'AtsMetaProjectedInterestRates' 
				public static DataSet LookupProjectedInterestRate()
				{
					DataSet dsLookUpProjectedInterestRate = null;
		            Database db = null;
					DbCommand commandLookUpProjectedInterestRate = null;
		
					try
					{
						db = DatabaseFactory.CreateDatabase("YRS");
		
						if (db == null) return null;
		
						commandLookUpProjectedInterestRate = db.GetStoredProcCommand("yrs_usp_AMPIR_LookupProjectedInterestRates");
						
						if (commandLookUpProjectedInterestRate == null) return null;
		
						dsLookUpProjectedInterestRate = new DataSet();
						dsLookUpProjectedInterestRate.Locale = CultureInfo.InvariantCulture;
						db.LoadDataSet(commandLookUpProjectedInterestRate, dsLookUpProjectedInterestRate,"Projected Interest Rates");
						
						return dsLookUpProjectedInterestRate;
					}
					catch 
					{
						throw;
					}

				}


		//function returning dataset for the search against 'Annuisty Basis'.
		//The DataSet contains the rows where Annuity Basis= "%" + TextBoxVar + "%" of table 'AtsMetaAnnuityBasisTypes' 
		public static DataSet SearchProjectedInterestRate(string parameterSearchProjectedInterestRates)
		{
			DataSet dsSearchProjectedInterestRate = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMPIR_SearchProjectedInterestRates");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper, "@varchar_InterestYear",DbType.String,parameterSearchProjectedInterestRates);
//				SearchCommandWrapper.AddOutParameter("@integer_ReturnVal",DbType.Int32,10);
			
				dsSearchProjectedInterestRate = new DataSet();
				dsSearchProjectedInterestRate.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchProjectedInterestRate,"Projected Interest Rates");
				
				return dsSearchProjectedInterestRate;
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

		public static void InsertProjectedInterestRate(DataSet parameterInsertProjectedInterestRate)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMPIR_InsertProjectedInterestRate");
				
				db.AddInParameter(insertCommandWrapper, "@integer_InterestYear",DbType.Int32,"Interest Year",DataRowVersion.Current);
				//there was a space before "Interest Rate" i.e. " Interest Rate" due to which stored proc was throwing error.removed by Anita on June6 2007
				db.AddInParameter(insertCommandWrapper, "@numeric_InterestRate",DbType.Int32,"Interest Rate",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@dateTime_InterestEndDate",DbType.DateTime,"Interest end date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMPIR_UpdateProjectedInterestRates");
				
				db.AddInParameter(updateCommandWrapper, "@integer_InterestYear",DbType.Int32,"Interest Year",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper, "@numeric_InterestRate",DbType.Int32,"Interest Rate",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@dateTime_InterestEndDate",DbType.DateTime,"Interest end date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);
				
				deleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMPIR_DeleteProjectedInterestRate");
				
				db.AddInParameter(deleteCommandWrapper, "@integer_InterestYear",DbType.Int32,"Interest Year",DataRowVersion.Original);
				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterInsertProjectedInterestRate != null)
				{
					db.UpdateDataSet(parameterInsertProjectedInterestRate,"Projected Interest Rates" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}



	}
}
