//********************************************************************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified by         Date             Description
//-------------------------------------------------
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for CommonLookUpTablesDAClass.
	/// </summary>
	public sealed class CommonLookUpTablesDAClass
	{
		private CommonLookUpTablesDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpTables()
					{
						DataSet l_dataset_dsLookUpTables = null;
			            Database db = null;
						DbCommand l_commandLookUpTables = null;
			
						try
						{
							db = DatabaseFactory.CreateDatabase("YRS");
			
							if (db == null) return null;
			
							l_commandLookUpTables = db.GetStoredProcCommand("yrs_usp_LookupTables");
							
							if (l_commandLookUpTables == null) return null;
			
							l_dataset_dsLookUpTables = new DataSet();
							db.LoadDataSet(l_commandLookUpTables, l_dataset_dsLookUpTables,"Annuity Basis Types");
							System.AppDomain.CurrentDomain.SetData("DataSetAnnuityBasisTypes", l_dataset_dsLookUpTables);
							return l_dataset_dsLookUpTables;
						}
						catch 
						{
							throw;
						}

					}
	}
}
