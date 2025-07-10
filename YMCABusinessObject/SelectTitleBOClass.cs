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
	/// Summary description for SelectTitleBOClass.
	/// </summary>
	public sealed class SelectTitleBOClass
	{
		private SelectTitleBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpTitle()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.SelectTitleDAClass.LookUpTitles();
			}
			catch
			{
				throw;
			}
		}


		public static DataSet SearchTitle(string parameterSearchShortDescription)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.SelectTitleDAClass.SearchTitle(parameterSearchShortDescription);
			}
			catch
			{
				throw;
			}

		}

	}
}
