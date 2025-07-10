//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
using System.Security.Permissions;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for MetaPositionsMaintenance.
	/// </summary>
	public sealed class MetaPositionsMaintenanceBOClass
	{
		private MetaPositionsMaintenanceBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		// function accessing the LookUpPositions Function of the DataAccess layer and returning the same dataset to the UI layer
		public static DataSet LookUpPositionsMaintenance()
		{
			try
			{
				return(YMCARET.YmcaDataAccessObject.MetaPositionsMaintenanceDAClass.LookUpPositionsMaintenance());
			}
			catch
			{
				throw;
			}
		}


		public static DataSet SearchPositionsMaintenance(string parameterSearchPositionsMaintenance)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.MetaPositionsMaintenanceDAClass.SearchPositionsMaintenance(parameterSearchPositionsMaintenance);
			}
			catch
			{
				throw;
			}
		}


		/// <summary>
		///	This method Insert values into the table 'AtsMetaPositions' 
		///	on click of Add button followed by save button after entering data in the textboxes of UI
		/// and also Update values into the table 'AtsMetaPositions' 
		///	on click of edit button followed by save button after changing data in the textboxes of UI
		/// </summary>
		/// <param name="parameterAnnuityBasisTypes"></param>
		public static void InsertPositionsMaintenance(DataSet parameterInsertPositionsMaintenance)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.MetaPositionsMaintenanceDAClass.InsertPositionsMaintenance(parameterInsertPositionsMaintenance);
			}
			catch
			{
				throw;
			}
		}

	}
}
