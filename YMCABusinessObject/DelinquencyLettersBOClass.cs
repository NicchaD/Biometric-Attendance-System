//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using System.Web;
using System.Globalization;
using System.Security.Permissions;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for DelinquencyLettersBOClass.
	/// </summary>
	public class DelinquencyLettersBOClass
	{
		public DelinquencyLettersBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region "**********************Public Methods************************"
		
		public static DataSet GetLetterType()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetLetterType();

			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetLetter()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetLetterType();

			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetYMCAList()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetYMCAList();

			}
			catch
			{
				throw;
			}

		}

		public static DataSet GetDelinquentYmcas()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetDelinquentYmcas();
			
			}
			catch
			{
				throw;
			}

		}

		public static DataSet GetDelinquentYmcasFor9thBusDay()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetDelinquentYmcasFor9thBusDay();

			}
			catch
			{
				throw;
			}
		}	  

		public static DataSet GetContactAndOfficerList(string parameterYmcaNos)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetContactAndOfficerList(parameterYmcaNos);

			}
			catch
			{
				throw;
			}
		}
//aparna
		public static DataSet Get15thBusinessDay()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.Get15thBusinessDay();
			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetList(string parameterYmcaNos,int paramererSno)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetList(parameterYmcaNos,paramererSno);

			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetYmcasPayRollDates(string parameterYmcaNo)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetYmcasPayRollDates(parameterYmcaNo);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetPayRollDatesFor9thBusDay(string parameterYmcaNo)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetPayRollDatesFor9thBusDay(parameterYmcaNo);

			}
			catch
			{
				throw;
			}
		}

		//aparna 27/09/2006

		public static DataSet MetaOutputFileType()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.getMetaOutputFileType()); 
				
			}
			catch
			{
				throw;
			}

		}
		//To get schema of report table
		//aparna -14/12/2006
		public static DataSet GetSchemaAtsDelinquencyCRData()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.GetSchemaAtsDelinquencyCRData();
			}
			catch
			{
				throw;
			}
		}

		//To insert into the temporary table
		public static void InsertReportData(DataSet parameterReportData)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.InsertReportData(parameterReportData);
			}
			catch
			{
				throw;
			}
		}

		public static void DeleteReportData(string parameterUserId,string parameterIPAddress)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.DelinquencyLettersDAClass.DeleteReportData(parameterUserId,parameterIPAddress);
			}
			catch
			{
				throw;
			}

		}
		#endregion "**************End Public Methods***************************"
	}
}
