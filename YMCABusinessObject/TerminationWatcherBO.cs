//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	TerminationWatcherBO.cs
// Author Name		:	Priya Patil
// Employee ID		:	37786
// Email			:	priya.jawale@3i-infotech.com
// Contact No		:	8416
// Creation Time	:	8/25/2012 6:36:14 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Chnaged by				Date				    Description
//*******************************************************************************
//Anudeep                   07.11.2012              Adding Processid While processing records and Returning Processid for Report Genaration
//Manthan Rajguru           2015.09.16              YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************

using System;
using System.Data;
using System.Collections.Generic;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
/// <summary>
/// Summary description for AddBeneficiaryBOClass.
/// </summary>
	public class TerminationWatcherBO
	{
		        #region Constructor
        public TerminationWatcherBO()
        { }  
        #endregion
		public static DataSet SearchPerson(string strFundNo, string strFirstName, string strLastName, string strSSNo)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.SearchPerson( strFundNo,  strFirstName,  strLastName,  strSSNo));
            }
            catch
            {
                throw;
            }
        }

		public static string SaveTerminationWatcherData(string strguiPersId, string strguiFundEventId, string strchrType, string strchvPlanType, string strchvSource)
		{
			try 
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.SaveTerminationWatcherData (strguiPersId, strguiFundEventId,  strchrType,  strchvPlanType,  strchvSource));
			}
			catch 
			{ 
				throw;
			}

		}

		public static DataSet ListPerson(string strType)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.ListPerson(strType));
			}
			catch
			{
				throw;
			}
		}

		public static string UpdateTerminationWatcherData(string strUniqueId, string strPlanType)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.UpdateTerminationWatcherData( strUniqueId,  strPlanType));
			}
			catch
			{
				throw;
			}

		}

		public static string DeleteTerminationWatcherData(string strUniqueId)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.DeleteTerminationWatcherData(strUniqueId));
			}
			catch
			{
				throw;
			}

		}

		public static DataSet ListNotes(string strTerminationWatcherID)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.ListNotes(strTerminationWatcherID));
			}
			catch
			{
				throw;
			}
		}

		public static string SaveTerminationWatcherNotes(string strTerminationWatcherId, string strguiPersId, string strNotes, Boolean boolImportant)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.SaveTerminationWatcherNotes(  strTerminationWatcherId, strguiPersId,  strNotes,  boolImportant));
			}
			catch
			{
				throw;
			}

		}

		public static DataSet ListPurgeRecord(string strType)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.ListPurgeRecord(strType));
			}
			catch
			{
				throw;
			}
		}
		public static string PurgeTerminationWatcherData(List<string> LTerminationIds)

		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.PurgeTerminationWatcherData(LTerminationIds));
			}
			catch
			{
				throw;
			}
		}

		public static DataSet ListProcessedRecord(string strType)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.ListProcessedRecord(strType));
			}
			catch
			{
				throw;
			}
		}

        public static string[] ProcessTerminationWatcherData(List<string> LTerminationIds)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.ProcessTerminationWatcherData(LTerminationIds));
			}
			catch
			{
				throw;
			}
		}

		public static DataSet ListPlanType()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.ListPlanType());
			}
			catch
			{
				throw;
			}
		}

		public static DataSet ListInvalidRecord(string strType)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.ListInvalidRecord(strType));
			}
			catch
			{
				throw;
			}
		}

		public static DataSet GetApplicantPlanType(string strFundEventId)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.TerminationWatcherDA.GetApplicantPlanType(strFundEventId));
			}
			catch
			{
				throw;
			}
		}
        public static DataSet getConfigurationValue(string ParameterConfigKey)
        {
            try
            {
                return YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue(ParameterConfigKey);

            }
            catch
            {
                throw;
            }
        }
        //'Start Anudeep:06-nov-2012 Changes made For Process Report
        public static  DataSet GetProcessReportName(string key)
        {
            DataSet l_dataset_TWProcessReport = new DataSet();
            YMCARET.YmcaDataAccessObject.TerminationWatcherDA objTWDAClass = null;
            try
            {
                objTWDAClass = new YMCARET.YmcaDataAccessObject.TerminationWatcherDA();
                l_dataset_TWProcessReport = objTWDAClass.GetProcessReportName(key);
                return l_dataset_TWProcessReport;
            }
            catch
            {
                throw;
            }
            finally
            {
                l_dataset_TWProcessReport = null;
                objTWDAClass = null;
            }
        }
        //END 'Anudeep:06-nov-2012 Changes made For Process Report



























	}
}
