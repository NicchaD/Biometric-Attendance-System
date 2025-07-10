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
	/// Summary description for MetaAnnuityTypesMain.
	/// </summary>
	public sealed class MetaAnnuityTypesMain
	{
		private MetaAnnuityTypesMain()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookupAnnuityTypes()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaAnnuityTypesMainDAClass.LookupAnnuityTypes());
			}
			catch 
			{
				throw;
			}
		}


		public static DataSet SearchAnnuityTypes(string parameterAnnuityTypes)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.MetaAnnuityTypesMainDAClass.SearchAnnuityTypes(parameterAnnuityTypes));
			}
			catch
			{
				throw;
			}

		}

		public static void InsertAnnuityTypes(DataSet parameterAnnuityTypes)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.MetaAnnuityTypesMainDAClass.InsertAnnuityTypes(parameterAnnuityTypes);
			}
			catch
			{
				throw;
			}
		}
	}
}
