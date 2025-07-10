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
	/// Summary description for UpdateAddressDetailsBOClass.
	/// </summary>
	public class UpdateAddressDetailsBOClass
	{
		public UpdateAddressDetailsBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region Public Methods ----Ashutosh-----
		public static DataSet LookUpAddressInfo(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.LookUpPrimaryAddressInfo(parameterPersId);			
			}
			catch
			{
				throw;
			}
		}
		public static void UpdateParticipantAddress(DataSet parameterdsAddress)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateParticipantAddress( parameterdsAddress);
			}
			catch
			{
				throw;
			}
		}

		public static void UpdateTelephone(DataSet parameterdsAddress)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.ParticipantsInformationDAClass.UpdateTelephone(parameterdsAddress);
			}
			catch
			{
				throw;
			}
		}
		#endregion
	}
}
