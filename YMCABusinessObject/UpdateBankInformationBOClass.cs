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
	/// Summary description for UpdateBankInformationBOClass.
	/// </summary>
	public sealed class UpdateBankInformationBOClass
	{
		private UpdateBankInformationBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		
		public static DataSet LookUpPaymentMethod()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UpdateBankInformationDAClass.LookUpPaymentMethod();
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetEffDate(string parameterEntityId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UpdateBankInformationDAClass.GetEffDate(parameterEntityId);
			}
			catch
			{
				throw;
			}
		}
			

		public static DataSet LookUpAccountType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.UpdateBankInformationDAClass.LookUpAccountType();
			}
			catch
			{
				throw;
			}
		}
	}
}
