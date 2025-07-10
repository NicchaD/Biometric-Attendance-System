//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
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
	/// Summary description for AnnuityBasisTypesDAClass.
	/// </summary>
	/// 
	
	public sealed class AnnuityBasisTypesDAClass
	{

		private AnnuityBasisTypesDAClass ()
		{
		}

		// function returning Dataset containing all rows of table 'AtsMetaAnnuityBasisTypes' 
//		public static DataSet LookupAnnuityBasisTypes()
//		{
//			DataSet dsLookUpAnnuityBasisTypes = null;
//            Database db = null;
//			DbCommand commandLookUpAnnuityBasisTypes = null;
//
//			try
//			{
//				db = DatabaseFactory.CreateDatabase("YRS");
//
//				if (db == null) return null;
//
//				commandLookUpAnnuityBasisTypes = db.GetStoredProcCommand("yrs_usp_AMABT_LookupAnnuityBaisisTypes");
//				
//				if (commandLookUpAnnuityBasisTypes == null) return null;
//
//				dsLookUpAnnuityBasisTypes = new DataSet();
//				db.LoadDataSet(commandLookUpAnnuityBasisTypes, dsLookUpAnnuityBasisTypes,"Annuity Basis Types");
//				
//				return dsLookUpAnnuityBasisTypes;
//			}
//			catch 
//			{
//				throw;
//			}

//		}
	    //function returning dataset for the search against 'Annuisty Basis'.
		//The DataSet contains the rows where Annuity Basis= "%" + TextBoxVar + "%" of table 'AtsMetaAnnuityBasisTypes' 
		public static DataSet SearchAnnuityBasisTypes(string parameterSearchAnnuityBasisTypes)
		{
			DataSet dsSearchAnnuityBasisTypes = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMABT_SearchAnnuityBaisisTypes");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper, "@char_AnnuityBasisType",DbType.String,parameterSearchAnnuityBasisTypes);
//				SearchCommandWrapper.AddOutParameter("@integer_ReturnVal",DbType.Int32,10);
			
				dsSearchAnnuityBasisTypes = new DataSet();
				dsSearchAnnuityBasisTypes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchAnnuityBasisTypes,"Annuity Basis Types");
				
				return dsSearchAnnuityBasisTypes;
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

		public static void InsertAnnuityBasisTypes(DataSet parameterInsertAnnuityBasisTypes)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMABT_InsertAnnuityBaisisTypes");
                //Start:Changed by Anudeep:16.01.2013 changed column name to get actual value from datatable For Bt-1577 : Security Access Does not exists for "Annuity Basis Types" 
				db.AddInParameter(insertCommandWrapper, "@char_AnnuityBasisType",DbType.String,"Annuity Basis Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short[Desc]",DataRowVersion.Current);
                //End:Changed by Anudeep:16.01.2013 changed column name to get actual value from datatable For Bt-1577 : Security Access Does not exists for "Annuity Basis Types" 
				db.AddInParameter(insertCommandWrapper, "@varchar_LongDesc",DbType.String,"Long [Desc]",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@datetime_EffDate",DbType.DateTime,"Eff Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@datetime_TermDate",DbType.DateTime,"Term Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@int_AnnuityBasisPct",DbType.Int32,"Annuity BasisPct",DataRowVersion.Current);

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMABT_UpdateAnnuityBaisisTypes");
                //Start:Changed by Anudeep:16.01.2013 changed column name to get actual value from datatable For Bt-1577 : Security Access Does not exists for "Annuity Basis Types" 
                db.AddInParameter(updateCommandWrapper, "@char_AnnuityBasisType", DbType.String, "Annuity Basis Type", DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short[Desc]",DataRowVersion.Current);
                //End:Changed by Anudeep:16.01.2013 changed column name to get actual value from datatable For Bt-1577 : Security Access Does not exists for "Annuity Basis Types" 
                db.AddInParameter(updateCommandWrapper, "@varchar_LongDesc",DbType.String,"Long [Desc]",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@datetime_EffDate",DbType.DateTime,"Eff Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@datetime_TermDate",DbType.DateTime,"Term Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@int_AnnuityBasisPct",DbType.Int32,"Annuity BasisPct",DataRowVersion.Current);
				

				deleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMABT_DeleteAnnuityBaisisTypes");
				
				db.AddInParameter(deleteCommandWrapper, "@char_AnnuityBasisType",DbType.String,"Annuity Basis Type",DataRowVersion.Original);
				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterInsertAnnuityBasisTypes != null)
				{
					db.UpdateDataSet(parameterInsertAnnuityBasisTypes,"Annuity Basis Types" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}


	}
}
