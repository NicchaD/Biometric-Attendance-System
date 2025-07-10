//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for ControlSecurityDAClass.
	/// </summary>
	public class ControlSecurityDAClass
	{
		public ControlSecurityDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet GetSecuredControlsOnForm(string paramFormName)
		{
			Database db = null;
			DbCommand CommandGetSecuredControlsOnForm= null;
			DataSet dsGetSecuredControlsOnForm = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null)return null;

				CommandGetSecuredControlsOnForm=db.GetStoredProcCommand("yrs_usp_SFC_GetFormsSecuredControls");
				if (CommandGetSecuredControlsOnForm == null) return null;
				
				db.AddInParameter(CommandGetSecuredControlsOnForm,"@varchar_FormName",DbType.String,paramFormName);
                dsGetSecuredControlsOnForm = new DataSet();
				
				db.LoadDataSet(CommandGetSecuredControlsOnForm,dsGetSecuredControlsOnForm,"ControlsOnForm");
				return dsGetSecuredControlsOnForm;
				
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetControlNames(int paramControlId)
		{
			Database db = null;
			DbCommand CommandGetControlNames = null;
			DataSet ds_GetControlNames = null;
			try
			{
				db=DatabaseFactory.CreateDatabase("YRS");
				if(db==null)return null;
				CommandGetControlNames=db.GetStoredProcCommand("yrs_usp_Security_GetControlNames");
				if(CommandGetControlNames==null) return null;
				db.AddInParameter(CommandGetControlNames,"@integer_ControlId",DbType.Int32,paramControlId);

				ds_GetControlNames= new DataSet();
				db.LoadDataSet(CommandGetControlNames,ds_GetControlNames,"ControlNames");
				return ds_GetControlNames;
			}
			catch
			{
				throw;
			}
		}
	}
}
