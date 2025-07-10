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
	/// Summary description for MetaVestingTypeCode.
	/// </summary>
	public sealed class MetaVestingTypeCodeBOClass
	{
		public MetaVestingTypeCodeBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpVestingTypeCodes()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.MetaVestingTypeCodeDAClass.LookUpVestingTypeCode();
			}
			catch
			{
				throw;
			}
		}
		public static void InsertVestingTypeCodes(DataSet parameterInsertVestingTypeCode)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.MetaVestingTypeCodeDAClass.InsertVestingTypeCodes(parameterInsertVestingTypeCode);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet SearchVestingTypeCode(string parameterSearchVestingTypeCode)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.MetaVestingTypeCodeDAClass.SearchVestingTypeCode(parameterSearchVestingTypeCode);
			}
			catch
			{
				throw;
			}

		}

	}
}
