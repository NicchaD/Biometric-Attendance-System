//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Collections; 
using System.Globalization;
namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for SpecialDividendRatesDAClass.
	/// </summary>
	public sealed class SpecialDividendRatesDAClass
	{
		public SpecialDividendRatesDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet GetExperienceDividendData()
		{
			DataSet dsExperienceDividendData = null;
			Database db = null;
			DbCommand GetCommandWrapper = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				GetCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_ExpDividend_GetExperienceDividendData");
						
				if (GetCommandWrapper == null) return null;
		
				dsExperienceDividendData = new DataSet();
				dsExperienceDividendData.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(GetCommandWrapper,dsExperienceDividendData,"SpecialDividendData");
						
				return dsExperienceDividendData;
			}
			catch 
			{
				throw;
			}

		}
		public static void DeleteExpDividendDate(string parameterUniqueId)
		{
			Database db = null;
			DbCommand DeleteCommandWrapper = null;
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return ;

				DeleteCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_ExpDividend_DeleteExpDividendData");
				
				if (DeleteCommandWrapper == null) return ;

				db.AddInParameter(DeleteCommandWrapper,"@varchar_UniqueId",DbType.String,parameterUniqueId);
								
				db.ExecuteNonQuery(DeleteCommandWrapper);	
				
				
			}
			catch
			{
				throw;
			}
		}
		public static void InsertUpdateExperienceDividendData(DataSet parameterdsExpDividendDate)
		{
			Database db = null;
					
			DbCommand AddCommandWrapper = null;
			DbCommand UpdateCommandWrapper = null;
			DbCommand deleteCommandWrapper = null;
			
		
			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return ;

				UpdateCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_ExpDividend_InsertUpdateExpDividendData");
				
				if (UpdateCommandWrapper == null) return ;

				
				db.AddInParameter(UpdateCommandWrapper,"@varchar_guiUniqueId",DbType.String,"UniqueId",DataRowVersion.Current);
				db.AddInParameter(UpdateCommandWrapper,"@varchar_PaymentDate",DbType.String,"PayRollDate",DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@numeric_Percentage", DbType.String, "Percentage", DataRowVersion.Current);
                db.AddInParameter(UpdateCommandWrapper, "@char_Status", DbType.String, "Status", DataRowVersion.Current);
//				UpdateCommandWrapper.AddInParameter("@varchar_CreationDate",DbType.String,"CompletedOn",DataRowVersion.Current);
//				UpdateCommandWrapper.AddInParameter("@varchar_CreatedBy",DbType.String,"CompletedBy",DataRowVersion.Current);
//				
				UpdateCommandWrapper.CommandTimeout = System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"] );

				AddCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_ExpDividend_InsertUpdateExpDividendData");
				if(AddCommandWrapper == null) return ;
				
				
				db.AddInParameter(AddCommandWrapper,"@varchar_guiUniqueId",DbType.String,"UniqueId",DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@varchar_PaymentDate", DbType.String, "PayRollDate", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@numeric_Percentage", DbType.String, "Percentage", DataRowVersion.Current);
                db.AddInParameter(AddCommandWrapper, "@char_Status", DbType.String, "Status", DataRowVersion.Current);
//				AddCommandWrapper.AddInParameter("@varchar_CreationDate",DbType.String,"CompletedOn",DataRowVersion.Current);
//				AddCommandWrapper.AddInParameter("@varchar_CreatedBy",DbType.String,"CompletedBy",DataRowVersion.Current);

				
				AddCommandWrapper.CommandTimeout = System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"] );

				db.UpdateDataSet(parameterdsExpDividendDate, "SpecialDividendData" ,AddCommandWrapper,UpdateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				
			
				
				
				return ;
			}
			catch 
			{
				throw;
			}
		}

	}
}
