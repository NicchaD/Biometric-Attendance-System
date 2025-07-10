//*******************************************************************************
//	Date        Author			    Description
//*******************************************************************************
//22-May-2012	Priya			    BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
//2015.09.16    Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for AddResolutionBOClass.
	/// </summary>
	public sealed class AddResolutionBOClass
	{
		private AddResolutionBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}



		public static DataSet LookUpOptionType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddResolutionDAClass.LookUpOptionType();
			}
			catch
			{
				throw;
			}
		}

		public static DataSet LookUpVestingType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddResolutionDAClass.LookUpVestingType();
			}
			catch
			{
				throw;
			}
		}


		public static DataSet LookUpResolutionType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddResolutionDAClass.LookUpResolutionType();
			}
			catch
			{
				throw;
			}
		}
		//Priya 08-June-2009 : YRS 5.0-779  Warning msg if new resolution effective date too far in future
		public static DataSet getConfigurationValue(string ParameterConfigKey)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddResolutionDAClass.getConfigurationValue(ParameterConfigKey);
			}
			catch
			{
				throw;
			}
		}
		//End 08-June-2009

		
				//22-May-2012			Priya			BT-1029, YRS 5.0-1582 - Validate new Resolution effective date
		public static string GetYMCATransmitalDate(string paramYMCAuniqueId, string paramEffectiveDate)
		{ 
			try
			{
				return YMCARET.YmcaDataAccessObject.AddResolutionDAClass.GetYMCATransmitalDate( paramYMCAuniqueId,  paramEffectiveDate);
			}
			catch
			{
				throw;
			}
		}
		

	}
}
