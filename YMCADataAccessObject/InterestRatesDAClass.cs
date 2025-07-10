//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for InterestRatesDAClass.
	/// </summary>
	public sealed class InterestRatesDAClass
	{
		private InterestRatesDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		// function returning Dataset containing all rows of table 'AtsMetaInterestRates' 
		public static DataSet LookupInterestRate()
		{
			DataSet dsLookUpInterestRate = null;
			Database db = null;
			DbCommand  commandLookUpInterestRate = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				commandLookUpInterestRate = db.GetStoredProcCommand ("yrs_usp_AMIR_LookupInterestRates");
						
				if (commandLookUpInterestRate == null) return null;
		
				dsLookUpInterestRate = new DataSet();
				dsLookUpInterestRate.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpInterestRate, dsLookUpInterestRate,"Interest Rates");
						
				return dsLookUpInterestRate;
			}
			catch 
			{
				throw;
			}

		}

		//function returning dataset for the search against 'Annuisty Basis'.
		//The DataSet contains the rows where Annuity Basis= "%" + TextBoxVar + "%" of table 'AtsMetaAnnuityBasisTypes' 
		public static DataSet SearchInterestRate(string parameterSearchInterestRates)
		{
			DataSet dsSearchInterestRate = null;
			Database db = null;
			DbCommand  SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand ("yrs_usp_AMIR_SearchInterestRates");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper,"@char_AcctType",DbType.String,parameterSearchInterestRates);
				//				SearchCommandWrapper.AddOutParameter("@integer_ReturnVal",DbType.Int32,10);
			
				dsSearchInterestRate = new DataSet();
				dsSearchInterestRate.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchInterestRate,"Interest Rates");
				
				return dsSearchInterestRate;
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

		public static void InsertInterestRate(DataSet parameterInsertInterestRate)
		{
			Database db = null;
			DbCommand  insertCommandWrapper = null;
			DbCommand  updateCommandWrapper = null;
			DbCommand  deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand ("yrs_usp_AMIR_InsertInterestRates");
				
				db.AddInParameter(insertCommandWrapper,"@char_AcctType",DbType.String,"Accttype",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_Year",DbType.String,"Year",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_ShortDescription",DbType.String,"Short Desc",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Description",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@numeric_InterestRate",DbType.Int32,"Interest Rate",DataRowVersion.Current);
								

				updateCommandWrapper=db.GetStoredProcCommand ("yrs_usp_AMIR_UpdateInterestRates");
				
				db.AddInParameter(updateCommandWrapper,"@char_AcctType",DbType.String,"Accttype",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper,"@char_Year",DbType.String,"Year",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_ShortDescription",DbType.String,"Short Desc",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Description",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@numeric_InterestRate",DbType.Int32,"Interest Rate",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@char_Month",DbType.Int32,"Month",DataRowVersion.Current);
				
				deleteCommandWrapper=db.GetStoredProcCommand ("yrs_usp_AMIR_DeleteInterestRates");
				
				db.AddInParameter(deleteCommandWrapper,"@char_AcctType",DbType.String,"Accttype",DataRowVersion.Original);
				db.AddInParameter(deleteCommandWrapper,"@char_Year",DbType.String,"Year",DataRowVersion.Original);
				db.AddInParameter(deleteCommandWrapper,"@char_Month",DbType.String,"Month",DataRowVersion.Original);

				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterInsertInterestRate != null)
				{
					db.UpdateDataSet(parameterInsertInterestRate,"Interest Rates" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}

	}
}
