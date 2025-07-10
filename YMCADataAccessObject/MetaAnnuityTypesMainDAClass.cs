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
	/// Summary description for MetaAnnuityTypesMainDAClass.
	/// </summary>
	public sealed class MetaAnnuityTypesMainDAClass
	{
		private MetaAnnuityTypesMainDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// function returning Dataset containing all rows of table 'AtsMetaProjectedInterestRates' 
		public static DataSet LookupAnnuityTypes()
		{
			DataSet dsLookUpAnuityTypes = null;
			Database db = null;
			DbCommand commandLookUpAnnuityTypes = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				commandLookUpAnnuityTypes = db.GetStoredProcCommand("yrs_usp_AMAT_LookupAnnuityTypes");
						
				if (commandLookUpAnnuityTypes == null) return null;
		
				dsLookUpAnuityTypes = new DataSet();
				dsLookUpAnuityTypes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpAnnuityTypes, dsLookUpAnuityTypes,"Annuity Types");
						
				return dsLookUpAnuityTypes;
			}
			catch 
			{
				throw;
			}

		}

		public static DataSet SearchAnnuityTypes(string parameterSearchAnnuityTypes)
		{
			DataSet dsSearchAnnuityTypes = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMAT_SearchAnnuityTypes");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper, "@char_AnnuityType",DbType.String,parameterSearchAnnuityTypes);
				//				SearchCommandWrapper.AddOutParameter("@integer_ReturnVal",DbType.Int32,10);
			
				dsSearchAnnuityTypes = new DataSet();
				dsSearchAnnuityTypes.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchAnnuityTypes,"Annuity Types");
				
				return dsSearchAnnuityTypes;
			}
			catch 
			{
				throw;
			}
		}

		public static void InsertAnnuityTypes(DataSet parameterAnnuityTypes)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMAT_InsertAnnuityTypes");
				
				db.AddInParameter(insertCommandWrapper, "@char_AnnuityType",DbType.String,"Annuity Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@char_AnnuityBaseType",DbType.String," Annuity Base Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_AnnuityCategoryCode",DbType.String,"Category Code",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short Description",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Description",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@integer_CodeOrder",DbType.Int32,"Code Order",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@datetime_EffDate",DbType.DateTime,"Eff Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@datetime_TerminationDate",DbType.DateTime,"Termination Date",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@numeric_JointSurvivorPctg",DbType.Decimal,"Joint Survivor Pctg",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@numeric_IncreasePctg",DbType.Int64,"Increase Pctg",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_Increasing",DbType.Int16,"Increasing",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_Popup",DbType.Int16,"Popup",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_LastToDie",DbType.Int16,"Last to Die",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_SSLeveling",DbType.Int16,"Ssleveling",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_JointSurvivor",DbType.Int16,"Joint Survivor",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@bit_InsuredReserve",DbType.Int16,"Ins Reserve",DataRowVersion.Current);

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMAT_UpdateAnnuityTypes");
				
				db.AddInParameter(updateCommandWrapper, "@char_AnnuityType",DbType.String,"Annuity Type",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper, "@char_AnnuityBaseType",DbType.String," Annuity Base Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_AnnuityCategoryCode",DbType.String,"Category Code",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short Description",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_Description",DbType.String,"Description",DataRowVersion.Current);
				
				db.AddInParameter(updateCommandWrapper, "@integer_CodeOrder",DbType.Int32,"Code Order",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@datetime_EffDate",DbType.DateTime,"Eff Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@datetime_TerminationDate",DbType.DateTime,"Termination Date",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@numeric_JointSurvivorPctg",DbType.Decimal,"Joint Survivor Pctg",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@numeric_IncreasePctg",DbType.Int64,"Increase Pctg",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_Increasing",DbType.Int16,"Increasing",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_Popup",DbType.Int16,"Popup",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_LastToDie",DbType.Int16,"Last to Die",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_SSLeveling",DbType.Int16,"Ssleveling",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_JointSurvivor",DbType.Int16,"Joint Survivor",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@bit_InsuredReserve",DbType.Int16,"Ins Reserve",DataRowVersion.Current);
				
				deleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMAT_DeleteAnnuityTypes");
				
				db.AddInParameter(deleteCommandWrapper, "@char_AnnuityType",DbType.String,"Annuity Type",DataRowVersion.Original);
				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterAnnuityTypes != null)
				{
					db.UpdateDataSet(parameterAnnuityTypes,"Annuity Types" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}




	}

}
