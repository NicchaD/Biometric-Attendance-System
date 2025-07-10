//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//NP/PP/SR             2009.05.18    Optimizing the YMCA Screen
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for YMCAAddressBusinessClass.
	/// </summary>
	public sealed class YMCAAddressBOClass
	{
		private YMCAAddressBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpAddressStateType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.LookUpAddressStateType();
			}
			catch
			{
				throw;
			}
		}

		public static DataSet LookUpAddressType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.LookUpAddressType();
			}
			catch
			{
				throw;
			}
		}


		
		public static DataSet LookUpAddressCountry()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.LookUpAddressCountry();
			}
			catch
			{
				throw;
			}
		}

		public static DataSet SearchAddressInformation(string parameterSearchAddressInformationGuid)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.SearchAddressInformation(parameterSearchAddressInformationGuid);
			}
			catch
			{
				throw;
			}
		}

		public static void InsertAddressInformation(DataSet parameterInsertAddressInformation)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.InsertAddressInformation(parameterInsertAddressInformation);
			}
			catch
			{
				throw;
			}
		}

		public static string InsertAddressInformationAdd(DataSet parameterInsertAddressInformation)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.InsertAddressInformationAdd(parameterInsertAddressInformation);
			}
			catch
			{
				throw;
			}
		}

		public static void UpdateAddressInformationAdd(DataSet parameterUpdateAddressInformation)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.UpdateAddressInformationAdd(parameterUpdateAddressInformation);
			}
			catch
			{
				throw;
			}
		}


		public static DataSet SearchCountryDesc(string parameterCountryAbbrev)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.SearchCountryDesc(parameterCountryAbbrev);
			}
			catch
			{
				throw;
			}
		}


		public static DataSet UncheckAddressPrimary(DataSet g_dataset_dsAddressInformation,bool ParamLastPrimary)
		{
			int i;
			
			DataRow l_datarow;
			try
			{
				if(ParamLastPrimary==true)
				{
					for(i=0;i<g_dataset_dsAddressInformation.Tables[0].Rows.Count;i++)
					{
						l_datarow=g_dataset_dsAddressInformation.Tables[0].Rows[i];
						l_datarow["Make this Primary"]=false;

					}
					l_datarow=g_dataset_dsAddressInformation.Tables[0].Rows[g_dataset_dsAddressInformation.Tables[0].Rows.Count-1];
					l_datarow["Make this Primary"]=true;
				}

				return g_dataset_dsAddressInformation;
			}
			catch
			{
				throw;
			}
		}


		public static DataSet UncheckAddressPrimaryUpdate(DataSet g_dataset_dsAddressInformation,bool ParamLastPrimary,Int16 RowSelected )
		{
			int i;
			
			DataRow l_datarow;
			try
			{
				if(ParamLastPrimary==true)
				{
					for(i=0;i<g_dataset_dsAddressInformation.Tables[0].Rows.Count;i++)
					{
						l_datarow=g_dataset_dsAddressInformation.Tables[0].Rows[i];
						l_datarow["Make this Primary"]=false;

					}
					l_datarow=g_dataset_dsAddressInformation.Tables[0].Rows[RowSelected];
					l_datarow["Make this Primary"]=true;
				}

				return g_dataset_dsAddressInformation;
			}
			catch
			{
				throw;
			}
		}
		
		public static DataSet GetStates()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.GetStates();
			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetAddressNotesReasonSource(string parameterNotesSourceReason)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.YMCAAddressDAClass.GetAddressNotesReasonSource(parameterNotesSourceReason);
	}
			catch
			{
				throw;
			}
		}
	}
}
