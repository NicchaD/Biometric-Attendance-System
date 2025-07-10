//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			    Date				Description
//*******************************************************************************
//Neeraj Singh              06/jun/2010         Enhancement for .net 4.0
//Neeraj Singh              07/jun/2010         review changes done
//Manthan Rajguru           2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Santosh Bura              2017.05.26          YRS-AT-3465 -  YRS enh: Rollovers allow Rollback for processing routing under transaction block 
//*******************************************************************************
using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for RollOverReceiptsDAClass.
	/// </summary>
	public class RollOverReceiptsDAClass
	{
		public RollOverReceiptsDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet GetRolloverData(string parameterPersId, string parameterRolloverId)
		{
			DataSet l_dataset_dsRolloverData = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_GetRolloverRcpts");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper,"@varchar_gui_UniqueId",DbType.String,parameterPersId);
				db.AddInParameter(LookUpCommandWrapper,"@varchar_gui_RolloverId",DbType.String,parameterRolloverId);				
			
				l_dataset_dsRolloverData = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsRolloverData,"RolloverInfo");
			
				return l_dataset_dsRolloverData;
			}
			catch 
			{
				throw;
			}
		}


		public static DataSet GetRolloverRcptsData( string parameterRolloverId)
		{
			DataSet l_dataset_dsRolloverRcptData = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_GetRcptsData");
				
				if (LookUpCommandWrapper == null) return null;

				
				db.AddInParameter(LookUpCommandWrapper,"@varchar_gui_UniqueId",DbType.String,parameterRolloverId);

				
			
				l_dataset_dsRolloverRcptData = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_dsRolloverRcptData,"RolloverRcptsInfo");
			
				return l_dataset_dsRolloverRcptData;
			}
			catch 
			{
				throw;
			}
		}



		public static string AddRolloverData( string paramPersId,string paramFundId,string paramYmcaId,string paramRollId,string paramCheckNum,string paramCheckDate,string paramCheckReceivedDate,string paramTaxable,string paramNonTaxable)
		{
			
			Database db = null;
			DbCommand AddCommandWrapper = null;      
            // START: SB | 2017.05.26 | YRS-AT-3465 | Added Transaction in the existng method, if any error occurs while processing everything will be rolled back
            DbTransaction transaction = null;
            DbConnection connection = null;
            // END: SB | 2017.05.26 | YRS-AT-3465 | Added Transaction in the existng method, if any error occurs while processing everything will be rolled back

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return null;

                // START: SB | 2017.05.26 | YRS-AT-3465 | Opening a database connection and using begin transaction method
                connection = db.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                // END: SB | 2017.05.26 | YRS-AT-3465 | Opening a database connection and using begin transaction method

                AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_Rollover_AddRolloverDetails");
                if (AddCommandWrapper == null) return null;
                
                db.AddInParameter(AddCommandWrapper, "@varchar_PersId", DbType.String, paramPersId);
                db.AddInParameter(AddCommandWrapper, "@varchar_FundId", DbType.String, paramFundId);
                db.AddInParameter(AddCommandWrapper, "@varchar_YmcaId", DbType.String, paramYmcaId);
                db.AddInParameter(AddCommandWrapper, "@varchar_RolloverId", DbType.String, paramRollId);
                db.AddInParameter(AddCommandWrapper, "@varchar_CheckNum", DbType.String, paramCheckNum);
                db.AddInParameter(AddCommandWrapper, "@date_CheckDate", DbType.String, paramCheckDate);
                db.AddInParameter(AddCommandWrapper, "@date_CheckReceivedDate", DbType.String, paramCheckReceivedDate);
                db.AddInParameter(AddCommandWrapper, "@num_Taxable", DbType.String, paramTaxable);
                db.AddInParameter(AddCommandWrapper, "@num_NonTaxable", DbType.String, paramNonTaxable);
                db.AddOutParameter(AddCommandWrapper, "@char_output", DbType.String, 1);

                // START: SB | 2017.05.26 | YRS-AT-3465 | Adding connection timeout to command wrapper, also adding transaction in processing 
                AddCommandWrapper.CommandTimeout = System.Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);  // SB | 2017.05.26 | YRS-AT-3465 | Adding connection time out
                //db.ExecuteNonQuery(AddCommandWrapper);
                db.ExecuteNonQuery(AddCommandWrapper, transaction);
                // END: SB | 2017.05.26 | YRS-AT-3465 | Adding connection timeout to command wrapper, also adding transaction in processing 

                string l_string_output;

                l_string_output = Convert.ToString(db.GetParameterValue(AddCommandWrapper, "@char_output"));
                transaction.Commit();  //SB | 2017.05.26 | YRS-AT-3465 | Commit transaction if no error occurs
                return l_string_output;
            }
            catch
            {
                // START: SB | 2017.05.26 | YRS-AT-3465 | Rolling back current transaction when error occurs as well as closing the connection. Adding fianlly block also which ensures connection is closed.
                if (transaction != null) 
                    transaction.Rollback();

                if (connection != null) 
                    connection.Close();
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                connection = null;
                transaction = null;
                db = null;
                AddCommandWrapper = null;
            }
            // END: SB | 2017.05.26 | YRS-AT-3465 | Rolling back current transaction when error occurs as well as closing the connection. Adding fianlly block also which ensures connection is closed.
		}
		//Priya 24-April-2009 YRS 5.0-738 : Please use Account date instead of Calender date for roll in checks validation.
		public static string GetAccountDate()
		{
			Database db = null;
			DbCommand AddCommandWrapper = null;
			string l_string_output;

			try
			{
				
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return null;
				AddCommandWrapper = db.GetStoredProcCommand("yrs_usp_Lookup_AccountDate");
				if (AddCommandWrapper == null) return null;
                db.AddOutParameter(AddCommandWrapper, "@l_varchar_Date", DbType.DateTime, 20);
				db.ExecuteNonQuery(AddCommandWrapper);

                l_string_output = Convert.ToString(db.GetParameterValue(AddCommandWrapper, "@l_varchar_Date"));
				return l_string_output;
			}
			catch
			{
				throw;
			}
		}
	}
}
