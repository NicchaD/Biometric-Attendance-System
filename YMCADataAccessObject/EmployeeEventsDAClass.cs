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
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for EmployeeEventsDAClass.
	/// </summary>
	public sealed class EmployeeEventsDAClass
	{
		private EmployeeEventsDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookupEmployeeEventStatus()
		{
			DataSet dsLookUpEmployeeEventStatus = null;
			Database db = null;
			DbCommand  commandLookUpEmployeeEventStatus = null;
		
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
		
				if (db == null) return null;
		
				commandLookUpEmployeeEventStatus = db.GetStoredProcCommand ("yrs_usp_AMEES_LookupEmployeeEventStatus");
						
				if (commandLookUpEmployeeEventStatus == null) return null;
		
				dsLookUpEmployeeEventStatus = new DataSet();
				dsLookUpEmployeeEventStatus.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(commandLookUpEmployeeEventStatus, dsLookUpEmployeeEventStatus,"Employee Event Status");
						
				return dsLookUpEmployeeEventStatus;
			}
			catch 
			{
				throw;
			}

		}

		public static DataSet SearchEmployeeEventStatus(string parameterSearchEmployeeEventStatus)
		{
			DataSet dsSearchEmployeeEventStatus = null;
			Database db = null;
			DbCommand  SearchCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				SearchCommandWrapper = db.GetStoredProcCommand ("yrs_usp_AMEES_SearchEmployeeEventStatus");
				
				if (SearchCommandWrapper == null) return null;

				db.AddInParameter(SearchCommandWrapper,"@varchar_CodeValue",DbType.String,parameterSearchEmployeeEventStatus);
				//							
				dsSearchEmployeeEventStatus = new DataSet();
				dsSearchEmployeeEventStatus.Locale = CultureInfo.InvariantCulture;
				db.LoadDataSet(SearchCommandWrapper, dsSearchEmployeeEventStatus,"Employee Event Status");
				
				return dsSearchEmployeeEventStatus;
			}
			catch 
			{
				throw;
			}
		}

		public static void InsertEmployeeEventStatus(DataSet parameterEmployeeEventStatus)
		{
			Database db = null;
			DbCommand  insertCommandWrapper = null;
			DbCommand  updateCommandWrapper = null;
			DbCommand  deleteCommandWrapper = null;

			try
			{
				//DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
				db = DatabaseFactory.CreateDatabase("YRS");

				insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMEES_InsertEmployeeEventStatus");
				
				db.AddInParameter(insertCommandWrapper,"@varchar_CodeValue",DbType.String,"Emp. Event Type",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_ShortDescription",DbType.String,"Short Desc.",DataRowVersion.Current);
				db.AddInParameter(insertCommandWrapper,"@varchar_Description",DbType.String,"Long Desc." ,DataRowVersion.Current);
				

				updateCommandWrapper=db.GetStoredProcCommand ("yrs_usp_AMEES_UpdateEmployeeEventStatus");
				
			    db.AddInParameter(updateCommandWrapper,"@varchar_CodeValue",DbType.String,"Emp. Event Type",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_ShortDescription",DbType.String,"Short Desc.",DataRowVersion.Current);
				db.AddInParameter(updateCommandWrapper,"@varchar_Description",DbType.String,"Long Desc." ,DataRowVersion.Current);
				
				
				deleteCommandWrapper=db.GetStoredProcCommand ("yrs_usp_AMEES_DeleteEmployeeEventStatus");
				
				db.AddInParameter(deleteCommandWrapper,"@varchar_CodeValue",DbType.String,"Emp. Event Type",DataRowVersion.Original);
				

				// UpdateDataSet method has 6 parameters (Dataset,Table Name,
				//insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,
				//UpdateBehavior.Standard) as there is no delete fubctionality but method needs this parameter
				//so a reference of it is passed.

				if (parameterEmployeeEventStatus != null)
				{
					db.UpdateDataSet(parameterEmployeeEventStatus,"Employee Event Status" ,insertCommandWrapper,updateCommandWrapper,deleteCommandWrapper,UpdateBehavior.Standard);
				}
				
			}
			catch 
			{
				throw;
			}
		}

	}
}
