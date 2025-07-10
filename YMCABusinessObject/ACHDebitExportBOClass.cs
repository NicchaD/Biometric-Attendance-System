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
	/// Summary description for ACHDebitExportBOClass.
	/// </summary>
	public class ACHDebitExportBOClass
	{
		public ACHDebitExportBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet MetaOutputFileType()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.ACHDebitExportDAClass.getMetaOutputFileType()); 
				
			}
			catch
			{
				throw;
			}

		}
		public static DataSet GetPendingACHDebits()
		{
			try
			{	return YMCARET.YmcaDataAccessObject.ACHDebitExportDAClass.GetPendingACHDebits();
			}
			catch
			{	throw;
			}
		}
		public static int UpdateACHDebits(string parameteruniqueid,double parameteramount,string parameterpaydate)
		{	return YMCARET.YmcaDataAccessObject.ACHDebitExportDAClass.UpdateACHDebits( parameteruniqueid,parameteramount,parameterpaydate);
		}
		//public static void DeleteACHDebits(string parameteruniqueid)
		//{	YMCARET.YmcaDataAccessObject.ACHDebitExportDAClass.DeleteACHDebits(parameteruniqueid);

		//}
		
			public static void DeleteACHDebits(string parameteruniqueid)
			{	YMCARET.YmcaDataAccessObject.ACHDebitExportDAClass.DeleteACHDebits(parameteruniqueid);

			}
		public static void UpdateOnExport(DataSet parameterdsExport,string batchid)
		{YMCARET.YmcaDataAccessObject.ACHDebitExportDAClass.UpdateOnExport(parameterdsExport,batchid);
		}
		public static string GetBatchId()
		{
				return YMCARET.YmcaDataAccessObject.ACHDebitExportDAClass.GetBatchId();
		}


	}
}
