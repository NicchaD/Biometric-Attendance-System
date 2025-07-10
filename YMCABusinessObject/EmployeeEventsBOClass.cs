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
	/// Summary description for EmployeeEventsBusinessClass.
	/// </summary>
	public sealed class EmployeeEventsBOClass
	{
		private EmployeeEventsBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookupEmployeeEventStatus()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.EmployeeEventsDAClass.LookupEmployeeEventStatus());
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet SearchEmployeeEventStatus(string parameterEmployeeEventStatus)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.EmployeeEventsDAClass.SearchEmployeeEventStatus(parameterEmployeeEventStatus));
			}
			catch
			{
				throw;
			}

		}

		public static void InsertEmployeeEventStatus(DataSet parameterEmployeeEventStatus)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.EmployeeEventsDAClass.InsertEmployeeEventStatus(parameterEmployeeEventStatus);
			}
			catch
			{
				throw;
			}
		}

	}
}
