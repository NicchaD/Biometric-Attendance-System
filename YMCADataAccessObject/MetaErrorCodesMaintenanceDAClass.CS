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
// Changed on			:	28-05-2005
// Change Description	:	Coding
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by         Date             Description
//*******************************************************************************
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
	/// Summary description for MetaErrorCodesMaintenanceDAClass.
	/// </summary>
	public sealed class MetaErrorCodesMaintenanceDAClass
	{
		private MetaErrorCodesMaintenanceDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// function returning Dataset containing all rows of table 'AtsMetaProjectedInterestRates' 
		public static DataSet LookupErrorCodes()
		{
			DataSet dsLookUpErrorCodes = null;
			Database db = null;
			DbCommand commandLookUpErrorCodes = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				commandLookUpErrorCodes = db.GetStoredProcCommand("yrs_usp_AMEC_LookupErrorCodes");
						
				if (commandLookUpErrorCodes == null) return null;
		
				dsLookUpErrorCodes = new DataSet();
				dsLookUpErrorCodes.Locale=CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpErrorCodes, dsLookUpErrorCodes,"Error Codes");
						
				return dsLookUpErrorCodes;
			}
			catch 
			{
				throw;
			}

		}


		//function returning dataset for the search against 'Annuisty Basis'.
		//The DataSet contains the rows where Annuity Basis= "%" + TextBoxVar + "%" of table 'AtsMetaAnnuityBasisTypes' 
		public static DataSet SearchErrorCodes(string parameterErrorCodes)
		{
			DataSet dsSearchErrorCodes = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMEC_SearchErrorCodes");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper, "@char_ErrorCode",DbType.String,parameterErrorCodes);
				//				SearchCommandWrapper.AddOutParameter("@integer_ReturnVal",DbType.Int32,10);
			
				dsSearchErrorCodes = new DataSet();
				dsSearchErrorCodes.Locale=CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchErrorCodes,"Error Codes");
				
				return dsSearchErrorCodes;
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

		public static void InsertErrorCodes(DataSet parameterErrorCodes)
		{
			Database db = null;
			DbCommand insertCommandWrapper = null;
			DbCommand updateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMEC_InsertErrorCodes");
				
				db.AddInParameter(insertCommandWrapper, "@char_ErrorCode",DbType.String,"Error Code",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@char_Descrpition",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@char_Catagory",DbType.String,"Category",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper, "@char_ErrorLevel",DbType.String,"Error Level",DataRowVersion.Current);
				

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMEC_UpdateErrorCodes");
				
				db.AddInParameter(updateCommandWrapper, "@char_ErrorCode",DbType.String,"Error Code",DataRowVersion.Original);
				db.AddInParameter(updateCommandWrapper, "@char_Descrpition",DbType.String,"Description",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@char_Catagory",DbType.String,"Category",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper, "@char_ErrorLevel",DbType.String,"Error Level",DataRowVersion.Current);
				
				deleteCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMEC_DeleteErrorCodes");
				
				db.AddInParameter(deleteCommandWrapper, "@char_ErrorCode",DbType.String,"Error Code",DataRowVersion.Original);
				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterErrorCodes != null)
				{
					db.UpdateDataSet(parameterErrorCodes,"Error Codes" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}
	}
}
