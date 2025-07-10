//****************************************************
//Modification History
//****************************************************
//Modified by           Date          Description
//****************************************************
//NP/PP/SR              2009.05.18    Optimizing the YMCA Screen
//Manthan Rajguru       2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//****************************************************

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for YMCATelephoneBusinessClass.
	/// </summary>
	public sealed class YMCATelephoneBOClass
	{
		private YMCATelephoneBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpTelephoneType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCATelephoneDAClass.LookUpTelephoneType();
			}
			catch
			{
				throw;
			}
		}


		public static DataSet SearchTelephoneInformation(string parameterSearchTelephoneInformationGuid)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCATelephoneDAClass.SearchTelephoneInformation(parameterSearchTelephoneInformationGuid);
			}
			catch
			{
				throw;
			}
		}
		
		public static void InsertTelephoneInformation(DataSet parameterInsertTelephoneInformation)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCATelephoneDAClass.InsertTelephoneInformation(parameterInsertTelephoneInformation);
			}
			catch
			{
				throw;
			}
		}

		public static string InsertTelephoneInformationAdd(DataSet parameterInsertTelephoneInformation)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCATelephoneDAClass.InsertTelephoneInformationAdd(parameterInsertTelephoneInformation);
			}
			catch
			{
				throw;
			}
		}

		public static void UpdateTelephoneInformationAdd(DataSet parameterUpdateTelephoneInformation)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCATelephoneDAClass.UpdateTelephoneInformationAdd(parameterUpdateTelephoneInformation);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet UncheckAddressPrimary(DataSet g_dataset_dsTelephoneInformation,bool ParamLastPrimary)
		{
			int i;
			
			DataRow l_datarow;
			try
			{
				if(ParamLastPrimary==true)
				{
					for(i=0;i<g_dataset_dsTelephoneInformation.Tables[0].Rows.Count;i++)
					{
						l_datarow=g_dataset_dsTelephoneInformation.Tables[0].Rows[i];
						l_datarow["Primary"]=false;

					}
					l_datarow=g_dataset_dsTelephoneInformation.Tables[0].Rows[g_dataset_dsTelephoneInformation.Tables[0].Rows.Count-1];
					l_datarow["Primary"]=true;
				}

				return g_dataset_dsTelephoneInformation;
			}
			catch
			{
				throw;
			}
		}
		

		public static DataSet UncheckTelephonePrimaryUpdate(DataSet g_dataset_dsTelephoneInformation,bool ParamLastPrimary,Int16 RowSelected )
		{
			int i;
			
			DataRow l_datarow;
			try
			{
				if(ParamLastPrimary==true)
				{
					for(i=0;i<g_dataset_dsTelephoneInformation.Tables[0].Rows.Count;i++)
					{
						l_datarow=g_dataset_dsTelephoneInformation.Tables[0].Rows[i];
						l_datarow["Primary"]=false;

					}
					l_datarow=g_dataset_dsTelephoneInformation.Tables[0].Rows[RowSelected];
					l_datarow["Primary"]=true;
				}

				return g_dataset_dsTelephoneInformation;
			}
			catch
			{
				throw;
			}
		}


	}
}
