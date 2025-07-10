//********************************************************************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified by         Date             Description
//-------------------------------------------------
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//********************************************************************************************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AddEmploymentDAClass.
	/// </summary>
	public class AddEmploymentDAClass
	{
		public AddEmploymentDAClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public static string AddEmployment(string parameterPersId,string parameterYmcaId,string parameterFundId,string parameterBranchId,string parameterHireDate,string parameterTermDate,string parameterEligDate,string parameterPositionType,Int32 parameterProfessional,Int32 parameterSalaried,Int32 parameterFullTime,Int32 parameterPriorService, string parameterStatusType, string parameterStatusDate,string parameterEnrollmentDate,Int32 parameterActive)
		{
		
			Database db = null;
			DbCommand InsertCommandWrapper = null;
			String l_string_Output ;


			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_InsertParticipantEmployment");
				if (InsertCommandWrapper == null) return null;

				db.AddInParameter(InsertCommandWrapper, "@varchar_PersId",DbType.String,parameterPersId);
                db.AddInParameter(InsertCommandWrapper, "@varchar_YmcaId", DbType.String, parameterYmcaId);
                db.AddInParameter(InsertCommandWrapper, "@varchar_FundEventId", DbType.String, parameterFundId);
                db.AddInParameter(InsertCommandWrapper, "@varchar_BranchId", DbType.String, parameterBranchId);
                db.AddInParameter(InsertCommandWrapper, "@date_HireDate", DbType.String, parameterHireDate);
                db.AddInParameter(InsertCommandWrapper, "@date_TermDate", DbType.String, parameterTermDate);
                db.AddInParameter(InsertCommandWrapper, "@date_EligDate", DbType.String, parameterEligDate);
                db.AddInParameter(InsertCommandWrapper, "@varchar_PositionType", DbType.String, parameterPositionType);
                db.AddInParameter(InsertCommandWrapper, "@bit_Professional", DbType.Int32, parameterProfessional);
                db.AddInParameter(InsertCommandWrapper, "@bit_Salaried", DbType.Int32, parameterSalaried);
                db.AddInParameter(InsertCommandWrapper, "@bit_FullTime", DbType.Int32, parameterFullTime);
                db.AddInParameter(InsertCommandWrapper, "@int_PriorService", DbType.Int32, parameterPriorService);
                db.AddInParameter(InsertCommandWrapper, "@varchar_StatusType", DbType.String, parameterStatusType);
                db.AddInParameter(InsertCommandWrapper, "@date_StatusDate", DbType.String, parameterStatusDate);
                db.AddInParameter(InsertCommandWrapper, "@date_EnrollmentDate", DbType.String, parameterEnrollmentDate);
                db.AddInParameter(InsertCommandWrapper, "@bit_active", DbType.Int32, parameterActive);

                db.AddOutParameter(InsertCommandWrapper, "@int_output", DbType.String, 2);
				 
				db.ExecuteNonQuery(InsertCommandWrapper);
				//l_string_Output = Convert.ToString(InsertCommandWrapper.GetParameterValue("@int_output"));
                l_string_Output = db.GetParameterValue(InsertCommandWrapper, "@int_output").ToString();
				
				return l_string_Output;
			}
			catch 
			{
				throw;
			}
		}



		public static string UpdateEmployment(string parameterUniqueId,string parameterHireDate,string parameterTermDate, string parameterEnrollmentDate,int parameterPriorService)
		{
		
			Database db = null;
			DbCommand UpdateCommandWrapper = null;
			String l_string_Output ;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
				
				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_AMP_UpdateParticipantEmployment");
				if (UpdateCommandWrapper == null) return null;

				db.AddInParameter(UpdateCommandWrapper, "@varchar_UniqueId",DbType.String,parameterUniqueId);

                db.AddInParameter(UpdateCommandWrapper, "@date_HireDate", DbType.String, parameterHireDate);
                db.AddInParameter(UpdateCommandWrapper, "@date_TermDate", DbType.String, parameterTermDate);
                db.AddInParameter(UpdateCommandWrapper, "@date_EnrollmentDate", DbType.String, parameterEnrollmentDate);
                db.AddInParameter(UpdateCommandWrapper, "@int_PriorService", DbType.String, parameterPriorService);

                db.AddOutParameter(UpdateCommandWrapper, "@int_output", DbType.String, 2);
				 
				db.ExecuteNonQuery(UpdateCommandWrapper);
				//l_string_Output = Convert.ToString(UpdateCommandWrapper.GetParameterValue("@int_output"));
                l_string_Output = db.GetParameterValue(UpdateCommandWrapper, "@int_output").ToString();
				
				return l_string_Output;
			}
			catch 
			{
				throw;
			}
		}

	}
}
