//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for ControlSecurityBOClass.
	/// </summary>
	public class ControlSecurityBOClass
	{
		public ControlSecurityBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet GetSecuredControlsOnForm(string paramFormName)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ControlSecurityDAClass.GetSecuredControlsOnForm(paramFormName);
   			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetControlNames(int paramControlId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ControlSecurityDAClass.GetControlNames(paramControlId);
			}
			catch
			{
				throw;
			}
		}
	}
}
