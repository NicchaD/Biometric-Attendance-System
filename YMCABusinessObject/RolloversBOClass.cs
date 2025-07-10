//*******************************************************************************
//Modification History
//*******************************************************************************
//	Date		        Author			Description
//*******************************************************************************
//15-05-2012			Priya Patil		BT-1028,YRS 5.0-1577: Add new field to atsRollovers
//15-05-2014            Anudeep A       BT-1051:YRS 5.0-1618 :Enhancements to Roll In process
//2015.09.16            Manthan Rajguru YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2016.08.22            Manthan Rajguru YRS-AT-2980 -  YRS enh: FiratName, LastName, Addr1, Addr2 fields should not allow Accent Characters or Tilde Characters
//*******************************************************************************

using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for Rollovers.
	/// </summary>
	public class RolloversBOClass
	{
		public RolloversBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpPersons(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName,string parameterFormName)
		{
			try
			{
				return(RolloversDAClass.LookUpPerson(parameterSSNo, parameterFundNo,parameterLastName,parameterFirstName,parameterFormName));
					
			}
			catch
			{
				throw;
			}
		
		}

		public static DataSet LookUpEmploymentInfo(string parameterPersId,string parameterFundId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RolloversDAClass.LookUpEmploymentInfo(parameterPersId,parameterFundId );
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpGeneralInfo(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RolloversDAClass.LookUpGeneralInfo(parameterPersId);
			}	
			catch
			{
				throw;
			}
		}

		public static DataSet LookUpRolloverInfo(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RolloversDAClass.LookUpRolloverInfo(parameterPersId);
			}	
			catch
			{
				throw;
			}
		}


        public static string SaveRolloverInfo(string parameterPersId, string parameterFundId, string parameterInstName, string parameterStatus, string paramterdate, string paramaterInfoSource, string paramaterParticipantAccountNo, bool parameterGenerateLtr, DataSet parameterAddress, string parameterAddressId) //AA:14.05.2014 BT:1051 - YRS 5.0-1618 - updated to save the rollover data with account number and generate letter
		{
			try
			{
                return YMCARET.YmcaDataAccessObject.RolloversDAClass.SaveRolloverInfo(parameterPersId, parameterFundId, parameterInstName, parameterStatus, paramterdate, paramaterInfoSource, paramaterParticipantAccountNo, parameterGenerateLtr, parameterAddress,parameterAddressId); //AA:14.05.2014 BT:1051 - YRS 5.0-1618 - updated to save the rollover data with account number and generate letter
			}
			catch
			{
				throw;
			}
		}


		public static string EditRolloverInfo(string parameterGuiId,string parameterInstName,string paramterdate)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RolloversDAClass.EditRolloverInfo(parameterGuiId, parameterInstName, paramterdate);
			}
			catch
			{
				throw;
			}

		}
		public static DataSet GetActiveEmpEvent(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RolloversDAClass.GetActiveEmpEvent(parameterPersId);
			}
			catch
			{
				
				throw;
			}
		}
		//BT-1028,YRS 5.0-1577: Add new field to atsRollovers
		public static DataSet GetInfoSource()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RolloversDAClass.GetInfoSource();
			}
			catch
			{

				throw;
			}
		}
        //Start:Anudeep:15.05.2014 BT-1051:YRS 5.0-1618 :Added new functions to get institutions and cancel rollover
        public static DataSet GetRolloverInstitutions()
		{
			try
			{
                return YMCARET.YmcaDataAccessObject.RolloversDAClass.GetRolloverInstitutions();
			}
			catch
			{

				throw;
			}
		}

        public static string CancelRolloverRequest(string paramterRolloverId)
		{
			try
			{
                return YMCARET.YmcaDataAccessObject.RolloversDAClass.CancelRolloverRequest(paramterRolloverId);
			}
			catch
			{

				throw;
			}
		}
        //End:Anudeep:15.05.2014 BT-1051:YRS 5.0-1618 :Added new functions to get institutions and cancel rollover

        //Start - Manthan Rajguru | 2016.08.22 | YRS-AT-2980 | Added method to get rollin institution name and address
        public static DataTable GetInstitutionNameAndAddress(string strRollInID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RolloversDAClass.GetInstitutionNameAndAddress(strRollInID));
            }
            catch
            {
                throw;
            }
        }
        //End - Manthan Rajguru | 2016.08.22 | YRS-AT-2980 | Added method to get rollin institution name and address
    }
}
