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
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for YMCAMetroBusinessClass.
	/// </summary>
	public class YMCAMetroBOClass
	{
		public YMCAMetroBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet SearchYMCAMetro(string parameterSearchYMCANo,string parameterSearchYMCAName,string parameterSearchYMCACity,string parameterSearchYMCAState)
		{

			try
			{
				return YMCARET.YmcaDataAccessObject.YMCAMetroDAClass.SearchYMCAMetro(parameterSearchYMCANo,parameterSearchYMCAName,parameterSearchYMCACity,parameterSearchYMCAState);
			}
			catch
			{
				throw;
			}
			
		}
		
	}
}
