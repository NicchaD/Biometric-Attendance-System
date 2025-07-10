/********************************************************************************************************************************
 Modification History
'********************************************************************************************************************************
'Modified By			Date					Description
'********************************************************************************************************************************
'Ashutosh Patil			16-May-2007				To Get The Details like MailService, UseDefault and 
												FromEmail,ToEmaial,BCCEamiail For Particular ConfigCategoryCode
'Nikunj Patel			2009.04.06				Moving code from MetaConfigMaintenanceDAClass to MailDAClass
'Manthan Rajguru        2015.09.16              YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'********************************************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for MetaConfigMaintenanceDAClass.
	/// </summary>
	public sealed class MetaConfigMaintenanceDAClass
	{
		private MetaConfigMaintenanceDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// function returning Dataset containing all rows of table 'AtsMetaProjectedInterestRates' 
		public static DataSet LookupConfigurationMaintenance()
		{
			DataSet dsLookUpConfigurationMaintenance = null;
			Database db = null;
			DbCommand commandLookUpConfigurationMaintenance = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				commandLookUpConfigurationMaintenance = db.GetStoredProcCommand("yrs_usp_AMCM_LookupConfigurationMaintenance");
						
				if (commandLookUpConfigurationMaintenance == null) return null;
		
				dsLookUpConfigurationMaintenance = new DataSet();
				dsLookUpConfigurationMaintenance.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpConfigurationMaintenance, dsLookUpConfigurationMaintenance,"Configuration Maintenance");
						
				return dsLookUpConfigurationMaintenance;
			}
			catch 
			{
				throw;
			}

		}

		//function returning dataset for the search against 'Annuisty Basis'.
		//The DataSet contains the rows where Annuity Basis= "%" + TextBoxVar + "%" of table 'AtsMetaAnnuityBasisTypes' 
		public static DataSet SearchConfigurationMaintenance(string parameterSearchConfigurationMaintenance)
		{
			DataSet dsSearchConfiguration = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMCM_SearchConfigurationMaintenance");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper, "@varchar_Key",DbType.String,parameterSearchConfigurationMaintenance);
				//				SearchCommandWrapper.AddOutParameter("@integer_ReturnVal",DbType.Int32,10);
			
				dsSearchConfiguration = new DataSet();
				dsSearchConfiguration.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchConfiguration,"Configuration Maintenance");
				
				return dsSearchConfiguration;
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

		public static void InsertConfigurationMaintenance(DataSet parameterConfigurationMaintenance)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMCM_InsertConfigurationMaintenance");
				
				db.AddInParameter(insertCommandWrapper, "@varchar_Key",DbType.String,"Key",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_ConfigCategoryCode",DbType.String," Category Code",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@char_DataType",DbType.String,"Data Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@vardchar_Value",DbType.String,"Value",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short Desc",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@varchar_Description",DbType.String,"Description",DataRowVersion.Current);

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMCM_UpdateConfigurationMaintenance");
				
				db.AddInParameter(updateCommandWrapper, "@varchar_Key",DbType.String,"Key",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper, "@varchar_ConfigCategoryCode",DbType.String," Category Code",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@char_DataType",DbType.String,"Data Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@vardchar_Value",DbType.String,"Value",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_ShortDescription",DbType.String,"Short Desc",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@varchar_Description",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@uniqueidentifier_KeywordID",DbType.String,"Keyword Id",DataRowVersion.Current);


				
				deleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMCM_DeleteConfigurationMaintenance");
				
				db.AddInParameter(deleteCommandWrapper, "@char_Key ",DbType.String,"Key",DataRowVersion.Original);
				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterConfigurationMaintenance != null)
				{
					db.UpdateDataSet(parameterConfigurationMaintenance,"Configuration Maintenance" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}

	}
}
