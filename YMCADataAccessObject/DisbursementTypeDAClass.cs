//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name			:	YMCA - YRS
// FileName				:	DisbursementTypeDAClass.cs
// Author Name			:	SrimuruganG
// Employee ID			:	32365
// Email				:	srimurugan.ag@icici-infotech.com
// Contact No			:	8744
// Creation Time		:	7/25/2005 2:38:44 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//****************************************************
//Modification History
//****************************************************
//Modified by         Date             Description
//****************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for DisbursementTypeDAClass.
	/// </summary>
	public sealed class DisbursementTypeDAClass
	{
		public DisbursementTypeDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void UpdateDataSet (DataSet disbursementTypesDataSet )
		{
			Database l_DataBase = null;

			DbCommand  InsertDBCommandWrapper = null; 
			DbCommand  UpdateDBCommandWrapper = null; 
			DbCommand  DeleteDBCommandWrapper = null;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (disbursementTypesDataSet == null) return;

				if (l_DataBase  == null) return;

				InsertDBCommandWrapper = l_DataBase.GetStoredProcCommand  ("yrs_usp_AMDT_InsertDisbursementType");

				if (InsertDBCommandWrapper != null)
				{
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@varchar_DisbursementType", DbType.String, "Disbursement Type", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@varchar_ShortDescription", DbType.String, "Short Description", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@varchar_LongDescription", DbType.String, "Description", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@varchar_GLAccountNumber", DbType.String, "GL Account No", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@varchar_CreatedBy", DbType.String, "CreatedBy", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@bit_Active", DbType.Int16, "Active", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@bit_Editable", DbType.Int16, "Editable", DataRowVersion.Current);
				}

				UpdateDBCommandWrapper = l_DataBase.GetStoredProcCommand ("yrs_usp_AMDT_UpdateDisbursementType");

				if (UpdateDBCommandWrapper != null)
				{
                    l_DataBase.AddInParameter(UpdateDBCommandWrapper, "@varchar_DisbursementType", DbType.String, "Disbursement Type", DataRowVersion.Current);
                    l_DataBase.AddInParameter(UpdateDBCommandWrapper, "@varchar_ShortDescription", DbType.String, "Short Description", DataRowVersion.Current);
                    l_DataBase.AddInParameter(UpdateDBCommandWrapper, "@varchar_LongDescription", DbType.String, "Description", DataRowVersion.Current);
                    l_DataBase.AddInParameter(UpdateDBCommandWrapper, "@varchar_GLAccountNumber", DbType.String, "GL Account No", DataRowVersion.Current);
                    l_DataBase.AddInParameter(UpdateDBCommandWrapper, "@varchar_CreatedBy", DbType.String, "CreatedBy", DataRowVersion.Current);
                    l_DataBase.AddInParameter(UpdateDBCommandWrapper, "@bit_Active", DbType.Int16, "Active", DataRowVersion.Current);
                    l_DataBase.AddInParameter(UpdateDBCommandWrapper, "@bit_Editable", DbType.Int16, "Editable", DataRowVersion.Current);
				}

				DeleteDBCommandWrapper = l_DataBase.GetStoredProcCommand ("yrs_usp_AMABT_DeleteDisbursement");

				if (DeleteDBCommandWrapper != null)
				{
                    l_DataBase.AddInParameter(DeleteDBCommandWrapper, "@varchar_DisbursementType", DbType.String, "Disbursement Type", DataRowVersion.Original);
				}

				l_DataBase.UpdateDataSet (disbursementTypesDataSet, "Disbursement Types", InsertDBCommandWrapper, UpdateDBCommandWrapper, DeleteDBCommandWrapper, UpdateBehavior.Standard);

			}
//			catch (SqlException sqlEx)
//			{
//				throw (sqlEx);
//			}
			catch
			{
				throw;
			}


		}

		public static DataSet LookupDisbursementTypes ()
		{
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;

			DataSet l_DataSet = null;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase == null) return null;

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand  ("yrs_usp_AMDT_LookupDisbursmentTypes");

				if (l_DBCommandWrapper == null) return null;

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@varchar_DisbursementType", DbType.String, "");

				l_DataSet = new DataSet ("Disbursement Types");

				l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet,"Disbursement Types");
				
				return (l_DataSet);
					
			}
//			catch (SqlException sqlEx)
//			{
//				throw (sqlEx);
//			}
			catch
			{
				throw;
			}

		}

		public static DataSet LookupDisbursementTypes (string searchDisbursementType)
		{
			Database l_DataBase = null;
			DbCommand  l_DBCommandWrapper = null;

			DataSet l_DataSet = null;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (l_DataBase == null) return null;

				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("yrs_usp_AMDT_LookupDisbursmentTypes");

				if (l_DBCommandWrapper == null) return null;

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@varchar_DisbursementType", DbType.String, searchDisbursementType);

				l_DataSet = new DataSet ("Disbursement Types");

				l_DataBase.LoadDataSet (l_DBCommandWrapper, l_DataSet,"Disbursement Types");
				
				return (l_DataSet);
					
			}
//			catch (SqlException sqlEx)
//			{
//				throw (sqlEx);
//			}
			catch
			{
				throw;
			}

		}



	}
}
