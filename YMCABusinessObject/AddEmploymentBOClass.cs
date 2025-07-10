//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for AddEmploymentBOClass.
	/// </summary>
	public class AddEmploymentBOClass
	{
		public AddEmploymentBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string AddEmployment(string parameterPersId,string parameterYmcaId,string parameterFundId,string parameterBranchId,string parameterHireDate,string parameterTermDate,string parameterEligDate,string parameterPositionType,Int32 parameterProfessional,Int32 parameterSalaried,Int32 parameterFullTime,Int32 parameterPriorService, string parameterStatusType, string parameterStatusDate,string parameterEnrollmentDate,Int32 parameterActive)
		{
			try
			{
				return AddEmploymentDAClass.AddEmployment(parameterPersId, parameterYmcaId, parameterFundId, parameterBranchId, parameterHireDate, parameterTermDate, parameterEligDate,parameterPositionType,parameterProfessional, parameterSalaried,parameterFullTime, parameterPriorService, parameterStatusType, parameterStatusDate, parameterEnrollmentDate, parameterActive);
			}
			catch
			{
				throw;
			}
		}

		public static string UpdateEmployment(string parameterUniqueId,string parameterHireDate,string parameterTermDate, string parameterEnrollmentDate, int parameterPriorService)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.AddEmploymentDAClass.UpdateEmployment(parameterUniqueId, parameterHireDate,parameterTermDate, parameterEnrollmentDate,parameterPriorService);
			}
			catch
			{
				throw;
			}
		}
	}
}
