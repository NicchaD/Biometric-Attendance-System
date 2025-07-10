/*************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	
' Author Name		:	Vipul Patel 
' Employee ID		:	32900 .... 
' Email				:	vipul.patel@3i-infotech.com
' Contact No		:	55928738
' Creation Time		:	10/05/2005 
' Program Specification Name	:	YMCA PS 3.13.1
' Unit Test Plan Name			:	
' Description					:	This form is used to Annuity Beneficiary Settlement
'****************************************************************************************/
//Modification History
//****************************************************************************************
//Date          Modified By            Description
//****************************************************************************************
//2007.10.22	Nikunj Patel	        YRPS-3868: Changing return status values to identify if we were sucessful or not in the process.
//2007.10.23	Nikunj Patel	        YRPS-3868: Splitting the procedure for search to return only search results and adding another one to return the selected survivor's details.
//								        Also cleaning up the code.
//2012.12.27    Dinesh Kanojia          FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death - Add one out parameter to return status message from database.            
//2015.04.17    Jagadeesh Bollabathini  FOR BT-2570,YRS 5.0-2380:Added method to insert into print letter table.            
//2015.04.17    Jagadeesh Bollabathini  FOR BT-2570,YRS 5.0-2380:Added method to update IDMTrackingId into atsPrintLetters for the follow-up data
//2015.09.16    Manthan Rajguru         YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2017.06.23    Santosh Bura            YRS-AT-2675 -  Annuity beneficiary restrictions. 
/****************************************************************************************/
using System;
using System.Data ;
using YMCARET.YmcaDataAccessObject;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for AnnuityBeneficiaryDeathBusinessClass.
	/// </summary>
	public class AnnuityBeneficiaryDeathBOClass
	{

		private const int RETURN_STATUS_DEFAULT = 1;

		/// <summary>
		/// Method for Lookup of RetireePersonsList. This procedure queries the database for a list of retirees and their annuity types.
		/// </summary>
		/// <param name="parameterSSNo">SSN Prefix that needs to be searched for. This will be used as is without modifications for queries.</param>
		/// <param name="parameterLName">First name prefix to search for</param>
		/// <param name="parameterFName">First name prefix to search for</param>
		/// <returns>DataSet contain the search results. The dataset contains a table with the following columns: 
		///		chrRetireeSSNO as "SS No."
		///		chvRetireeLastName as "Last Name" 
		///		chvRetireeFirstName as "First Name"
		///		chrAnnuityType as "Annuity Type"
		///		guiAnnuityID
		///</returns>
		public static DataSet LookUp_RetireePersonsList(string parameterSSNo,string parameterLName , string parameterFName )
		{	
			try
			{
				return (YMCARET.YmcaDataAccessObject.AnnuityBeneficiaryDeathDAClass.LookUp_RetireePersonsList(parameterSSNo, parameterLName, parameterFName)) ;
			}
			catch 
			{
				throw;
			}
		}

		/// <summary>
		/// Method for Lookup of JointSurvivor Annuity information. This procedure queries the database for the joint survivor information for a particular annuity.
		/// </summary>
		/// <param name="parameterAnnuityId">Unique Annuity Id whose Joint Survivor information is to be fetched</param>
		/// <returns>The dataset contains a table with the following columns: 
		///		chrRetireeSSNO as "SS No.",  
		///		chvRetireeLastName as "Last Name",  
		///		chvRetireeFirstName as "First Name",  
		///		chrAnnuityType as "Annuity Type",  
		///		chrSSNo ,  
		///		chvLastName ,  
		///		chvFirstName ,  
		///		chvMiddleName,  
		///		convert(varchar(10),dtmDeathDate,101) as "dtmDeathDate",  
		///		guiRetireeID,  
		///		guiAnnuityJointSurvivorsID,  
		///		dtsRetDate,  
		///		chvRetireeMiddleName,  
		///		guiAnnuityID,  
		///		dtmPurchaseDate  
		///</returns>
		public static DataSet LookUp_AnnuityJointSurvivor(string parameterAnnuityId)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.AnnuityBeneficiaryDeathDAClass.LookUp_AnnuityJointSurvivor(parameterAnnuityId));
			}
			catch 
			{
				throw;
			}
		}

		//NP:PS:2007.10.09 - YSPS-3868 - Passing JointSurvivorId instead of RetireeId
        //DineshK:2012.12.27: FOR BT-1266, YRS 5.0-1698:Cross check SSN when entering a date of death             
		/// <summary>
		/// Method for Processing BeneficiaryAnnuitySettlement
		/// </summary>
		/// <param name="parameterJointSurvivorID">Joint Survivor Id of the survivor whose death is being notified</param>
		/// <param name="parameterBeneficiaryDeathDate">Date on which the Joint Survivor died</param>
		/// <int>DataSet</returns>
        public static int Process_BeneficiaryAnnuitySettlement(string parameterJointSurvivorID, DateTime parameterBeneficiaryDeathDate, string strParticipantSSNo, out string outpara_DisplayString, out string str_ErrorMessage, out string outpara_DeathNotify)
		{
			try
			{
                // START: SB | 06/14/2017 | YRS-AT-2675 | Following lines are moved into DA layer and being executed under a DB transaction to handle daabase failures
                //int int_ReturnStatus_SetDeathDate = RETURN_STATUS_DEFAULT;
                //int int_ReturnStatus_HandleAnnuityAdjustment = RETURN_STATUS_DEFAULT;	//NP:2007.10.22 - YRPS-3868: Adding a separate status to obtain the return status of the second process.
                //string char_DisplayStringSetDeathDate = "";
                //string char_DisplayStringHandleAnuityAdj = "";
                //// Foxpro Continues if there is an error updating the date, which is wrong. Will do it exactly as how foxpro does
                //int_ReturnStatus_SetDeathDate = YMCARET.YmcaDataAccessObject.AnnuityBeneficiaryDeathDAClass.SetDeathDate(parameterJointSurvivorID, parameterBeneficiaryDeathDate, strParticipantSSNo, out char_DisplayStringSetDeathDate, out outpara_DeathNotify);
                //int_ReturnStatus_HandleAnnuityAdjustment = YMCARET.YmcaDataAccessObject.AnnuityBeneficiaryDeathDAClass.HandleAnnuityAdjustment(parameterJointSurvivorID, parameterBeneficiaryDeathDate, out char_DisplayStringHandleAnuityAdj, out str_ErrorMessage);
                //outpara_DisplayString = char_DisplayStringSetDeathDate + char_DisplayStringHandleAnuityAdj.Trim();
                ////NP:2007.10.22 - YRPS-3868: Returning the return value of a process that has failed
                //if (int_ReturnStatus_SetDeathDate != RETURN_STATUS_DEFAULT) return int_ReturnStatus_SetDeathDate;
                //return int_ReturnStatus_HandleAnnuityAdjustment;

                return YMCARET.YmcaDataAccessObject.AnnuityBeneficiaryDeathDAClass.SetDeathDateAndHandleAnnuityAdjustments(parameterJointSurvivorID, parameterBeneficiaryDeathDate, strParticipantSSNo, out outpara_DisplayString, out outpara_DeathNotify, out str_ErrorMessage);
                // END: SB | 06/14/2017 | YRS-AT-2675 | Following lines are moved into DA layer and being executed under a DB transaction to handle daabase failures
            }
			catch 
			{
				throw;
			}
		}

        //2015.04.17    Jagadeesh Bollabathini  FOR BT-2570,YRS 5.0-2380:Added method to insert into print letter table.            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strAnnuityJointSurvivorsId"></param>
        /// <param name="intIDMTrackingId"></param>
        public static int InsertPrintLetters(string strAnnuityJointSurvivorsId, string strPersId, string strLettersCode)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.AnnuityBeneficiaryDeathDAClass.InsertPrintLetters(strAnnuityJointSurvivorsId, strPersId, strLettersCode);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //2015.04.17    Jagadeesh Bollabathini  FOR BT-2570,YRS 5.0-2380:Added method to update IDMTrackingId into atsPrintLetters for the follow-up data
        public static void UpdatePrintLetters(int intPrintLettersId, int intIDMTrackingId)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.AnnuityBeneficiaryDeathDAClass.UpdatePrintLetters(intPrintLettersId, intIDMTrackingId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
	}
}
