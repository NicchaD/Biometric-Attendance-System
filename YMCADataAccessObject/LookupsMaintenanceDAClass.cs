//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	LookupsMaintenanceDAClass.cs
// Author Name		:	
// Employee ID		:	
// Email				:	
// Contact No		:	
// Creation Time		:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//
// Changed by			:	Ruchi Saxena
// Changed on			:	08/02/2005
// Change Description	:	Proc Name to be changed

// Changed by			:	Shefali Bharti
// Changed on			:	08/14/2005
// Change Description	:	Completed incomplete coding
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by         Date             Description
//*******************************************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Manthan Rajguru     2016.07.27       YRS-AT-2560 -  YRS enhancement-make SSN for beneficiaries an optional field.
//*****************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
//using System.Security.Permissions;
//using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for LookupsMaintenanceDAClass.
	/// </summary>
	public sealed class LookupsMaintenanceDAClass
	{
		private LookupsMaintenanceDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookupLookups()
		{
			DataSet dsLookUpLookups = null;
			Database db = null;
			DbCommand commandLookUpLookups = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				commandLookUpLookups = db.GetStoredProcCommand("yrs_usp_ALM_LookUpLookups");
						
				if (commandLookUpLookups == null) return null;
		
				dsLookUpLookups = new DataSet();
				dsLookUpLookups.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpLookups, dsLookUpLookups,"Lookups");
						
				return dsLookUpLookups;
			}
			catch 
			{
				throw;
			}

		}
		
		public static void InsertLookups(DataSet parameterInsertLookups)
		{
			Database db= null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_ALUM_InsertLookupsMaintenance");

				db.AddInParameter(insertCommandWrapper,"@char_Group",DbType.String,"Group",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_SubGroup1",DbType.String,"SubGroup1",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_SubGroup2",DbType.String,"SubGroup2",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@char_SubGroup3",DbType.String,"SubGroup3",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@int_CodeOrder",DbType.Int32,"Code Order",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_CodeValue",DbType.String,"Code Value",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_ShortDesc",DbType.String,"ShortDesc",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Desc",DbType.String,"Desc",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Editable",DbType.Int16,"Editable",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);

				updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_ALUM_UpdateLookupsMaintenance");

				db.AddInParameter(updateCommandWrapper,"@char_Group",DbType.String,"Group",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@char_SubGroup1",DbType.String,"SubGroup1",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@char_SubGroup2",DbType.String,"SubGroup2",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@char_SubGroup3",DbType.String,"SubGroup3",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@int_CodeOrder",DbType.Int32,"Code Order",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_CodeValue",DbType.String,"Code Value",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_ShortDesc",DbType.String,"ShortDesc",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Desc",DbType.String,"Desc",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Active",DbType.Int16,"Active",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Editable",DbType.Int16,"Editable",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@datetime_EffDate",DbType.DateTime,"Effective Date",DataRowVersion.Current);




				
				deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_ALUM_DeleteLookupsMaintenance");

				db.AddInParameter(deleteCommandWrapper,"@char_Group",DbType.String,"Group",DataRowVersion.Original);
				db.AddInParameter(deleteCommandWrapper,"@varchar_CodeValue",DbType.String,"Code Value",DataRowVersion.Original);
				

				if(parameterInsertLookups !=null) 
				{
					db.UpdateDataSet(parameterInsertLookups,"Lookups",insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
			}
			catch
			{
				throw;
			}
		}

		public static DataSet SearchLookups(string parameterSearchLookups)
		{
			DataSet dsSearchLookups = null;
			Database db = null;
			DbCommand SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_ALUM_SearchLookupsMaintenance");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper,"@char_Group",DbType.String,parameterSearchLookups);
				//							
				dsSearchLookups = new DataSet();
				dsSearchLookups.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchLookups,"Lookups");
				
				return dsSearchLookups;
			}
			catch 
			{
				throw;
			}
		}

        //Start - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Verify Phony SSN provided by user is valid or not.  
        public static Boolean IsValidPhonySSN(string strPhonySSN, string strBeneficiaryStatus, string strBeneficiaryId, string strBeneficiaryType)
        {
            Database db = null;
            DbCommand dbCommand = null;
            Boolean blnIsValidPhonySSN;
            try
            {                                                           
                    db = DatabaseFactory.CreateDatabase("YRS");
                    if (db == null) return false;
                    dbCommand = db.GetStoredProcCommand("yrs_usp_BF_IsValidPhonySSN");
                    if (dbCommand == null) return false;
                    db.AddInParameter(dbCommand, "@chvBeneId", DbType.String, strBeneficiaryId);
                    db.AddInParameter(dbCommand, "@chvSSN", DbType.String, strPhonySSN);
                    db.AddInParameter(dbCommand, "@chvAction_INSERT_UPDATE", DbType.String, strBeneficiaryStatus);
                    db.AddInParameter(dbCommand, "@chvBeneficiaryType", DbType.String, strBeneficiaryType);
                    db.AddOutParameter(dbCommand, "@bitValidSSN", DbType.Boolean, 10);
                    db.ExecuteNonQuery(dbCommand);
                    blnIsValidPhonySSN = Convert.ToBoolean(db.GetParameterValue(dbCommand, "@bitValidSSN"));
                
                return blnIsValidPhonySSN;
            }
            catch
            {
                throw;
            }
        }
        //End - Manthan Rajguru | 2016.07.27 | YRS-AT-2560 | Verify Phony SSN provided by user is valid or not.
        
    }
}
