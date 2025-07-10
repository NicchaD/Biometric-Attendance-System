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
	/// Summary description for YMCANetBusinessClass.
	/// </summary>
	public sealed class YMCANetBOClass
	{
		private YMCANetBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpEmailType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCANetDAClass.LookUpEmailType();
			}
			catch
			{
				throw;
			}
		}

		public static DataSet SearchEmailInformation(string parameterSearchEmailInformationGuid)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCANetDAClass.SearchEmailInformation(parameterSearchEmailInformationGuid);
			}
			catch
			{
				throw;
			}
		}


		public static void InsertEmailInformation(DataSet parameterInsertEmailInformation)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCANetDAClass.InsertEmailInformation(parameterInsertEmailInformation);
			}
			catch
			{
				throw;
			}
		}
		

		public static string InsertEmailInformationAdd(DataSet parameterInsertEmailInformation)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCANetDAClass.InsertEmailInformationAdd(parameterInsertEmailInformation);
			}
			catch
			{
				throw;
			}
		}

		public static void UpdateEmailInformationAdd(DataSet parameterUpdateEmailInformation)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCANetDAClass.UpdateEmailInformationAdd(parameterUpdateEmailInformation);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet UncheckEmailPrimary(DataSet g_dataset_dsEmailInformation,bool ParamLastPrimary)
		{
			int i;
			
			DataRow l_datarow;
			try
			{
				if(ParamLastPrimary==true)
				{
					for(i=0;i<g_dataset_dsEmailInformation.Tables[0].Rows.Count;i++)
					{
						l_datarow=g_dataset_dsEmailInformation.Tables[0].Rows[i];
						l_datarow["Primary"]=false;

					}
					l_datarow=g_dataset_dsEmailInformation.Tables[0].Rows[g_dataset_dsEmailInformation.Tables[0].Rows.Count-1];
					l_datarow["Primary"]=true;
				}

				return g_dataset_dsEmailInformation;
			}
			catch
			{
				throw;
			}
		}


		public static DataSet UncheckEmailPrimaryUpdate(DataSet g_dataset_dsEmailInformation,bool ParamLastPrimary,Int16 RowSelected )
		{
			int i;
			
			DataRow l_datarow;
			try
			{
				if(ParamLastPrimary==true)
				{
					for(i=0;i<g_dataset_dsEmailInformation.Tables[0].Rows.Count;i++)
					{
						l_datarow=g_dataset_dsEmailInformation.Tables[0].Rows[i];
						l_datarow["Primary"]=false;

					}
					l_datarow=g_dataset_dsEmailInformation.Tables[0].Rows[RowSelected];
					l_datarow["Primary"]=true;
				}

				return g_dataset_dsEmailInformation;
			}
			catch
			{
				throw;
			}
		}
		
	}
}
