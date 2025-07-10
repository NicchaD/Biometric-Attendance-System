//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Pramod P. Pokale     2017.09.21    YRS-AT-3631 - YRS enh: Data Corrections Tool -Admin screen function to allow manual update of Fund status from WD or WP to TM or PT (TrackIT 30950) 
//*****************************************************

using System;
using System.Data;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for FundEventStatusDAClass.
	/// </summary>
	public sealed class FundEventStatusDAClass
	{
		private FundEventStatusDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		// function returning Dataset containing all rows of table 'AtsMetaInterestRates' 
		public static DataSet LookupFundEventStatus()
		{
			DataSet dsLookUpFundEventStatus = null;
			Database db = null;
			DbCommand  commandLookUpFundEventStatus = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				commandLookUpFundEventStatus = db.GetStoredProcCommand ("yrs_usp_AMFES_LookupFundEventStatus");
						
				if (commandLookUpFundEventStatus == null) return null;
		
				dsLookUpFundEventStatus = new DataSet();
				dsLookUpFundEventStatus.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpFundEventStatus, dsLookUpFundEventStatus,"Fund Event Status");
						
				return dsLookUpFundEventStatus;
			}
			catch 
			{
				throw;
			}

		}


		public static DataSet SearchFundEventStatus(string parameterSearchFundEventStatus)
		{
			DataSet dsSearchFundEventStatus = null;
			Database db = null;
			DbCommand  SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand ("yrs_usp_AMFES_SearchFundEventStatus");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper,"@varchar_FundStatusType",DbType.String,parameterSearchFundEventStatus);
							
				dsSearchFundEventStatus = new DataSet();
				dsSearchFundEventStatus.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchFundEventStatus,"Fund Event Status");
				
				return dsSearchFundEventStatus;
			}
			catch 
			{
				throw;
			}
		}


		public static void InsertFundEventStatus(DataSet parameterInsertFundEventStatus)
		{
			Database db = null;
		    DbCommand 	 insertCommandWrapper = null;
		    DbCommand 	 updateCommandWrapper = null;
		    DbCommand 	 deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand ("yrs_usp_AMFES_InsertFundEventStatus");
				
				db.AddInParameter(insertCommandWrapper,"@varchar_FundStatusType",DbType.String,"Status",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_ShortDescription",DbType.String,"ShortDesc",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Description",DbType.String,"Desc",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_CanChange",DbType.Int16,"Can Change",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Interest",DbType.Int16,"Interest",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Deposits",DbType.Int16,"Deposits",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_Service",DbType.Int16,"Service",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@bit_AutoTerm",DbType.Int16,"Autoterm",DataRowVersion.Current);
								

				updateCommandWrapper=db.GetStoredProcCommand("yrs_usp_AMFES_UpdateFundEventStatus");

				db.AddInParameter(updateCommandWrapper,"@varchar_FundStatusType",DbType.String,"Status",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_ShortDescription",DbType.String,"ShortDesc",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Description",DbType.String,"Desc",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_CanChange",DbType.Int16,"Can Change",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Interest",DbType.Int16,"Interest",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Deposits",DbType.Int16,"Deposits",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_Service",DbType.Int16,"Service",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@bit_AutoTerm",DbType.Int16,"Autoterm",DataRowVersion.Current);
								

				deleteCommandWrapper=db.GetStoredProcCommand ("yrs_usp_AMFES_DeleteFundEventStatus");
				
				db.AddInParameter(deleteCommandWrapper,"@varchar_FundStatusType",DbType.String,"Status",DataRowVersion.Original);
				

				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterInsertFundEventStatus != null)
				{
					db.UpdateDataSet(parameterInsertFundEventStatus,"Fund Event Status" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}

        //START: PPP | 09/21/2017 | YRS-AT-3631 | Recommends fund event status for given fund event id
        /// <summary>
        /// Recommends fund event status for given fund event id based on balance, employment and annuities
        /// </summary>
        /// <param name="fundEventID">Fund Event ID</param>
        /// <returns>Recommended fund event status</returns>
        public static string GetRecommendedFundEventStatus(string fundEventID)
        {
            Database db;
            DbCommand cmd;
            string recommendedFundEventStatus;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                cmd = db.GetStoredProcCommand("yrs_usp_GetRecommendedFundEventStatus");
                if (cmd == null) return null;

                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MediumConnectionTimeOut"]);
                db.AddInParameter(cmd, "@VARCHAR_FundEventID", DbType.String, fundEventID);
                db.AddOutParameter(cmd, "@VARCHAR_RecommendedFundEventStatus", DbType.String, 10);
                db.ExecuteNonQuery(cmd);
                recommendedFundEventStatus = db.GetParameterValue(cmd, "@VARCHAR_RecommendedFundEventStatus").ToString();
                return recommendedFundEventStatus;
            }
            catch
            {
                throw;
            }
            finally
            {
                recommendedFundEventStatus = null;
                cmd = null;
                db = null;
            }
        }
        //END: PPP | 09/21/2017 | YRS-AT-3631 | Recommends fund event status for given fund event id

	}
}
