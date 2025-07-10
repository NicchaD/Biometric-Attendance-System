/*'****************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA-YRS
' FileName			:	AnnuityBeneficiaryDeathDAClass 
' Author Name		:	Vipul Patel 
' Employee ID		:	32900 
' Email				:	vipul.patel@3i-infotech.com
' Contact No		:	55928738
' Creation Time		:	10/11/2005 
' Program Specification Name	:	YMCA_PS_Annuity_Beneficiary_Death_Settlement.doc
' Unit Test Plan Name			:	
' Description					:	This will call multiple Procedures for Annuity Benficiary Settlement
'****************************************************************************************/
//Modification History
//****************************************************
// Date         Modified by     Description
//****************************************************
// 2007.10.09	Nikunj Patel	YSPS-3868 - Changing parameter to procedures from RetireeId to JointSurvivorId
// 2007.10.23	Nikunj Patel	YRPS-3868: Splitting the procedure for search to return only search results and adding another one to return the selected survivor's details.
//								Also cleaning up the code.
//2012.12.27    Dinesh Kanojia  FOR BT-1266,YRS 5.0-1698:Cross check SSN when entering a date of death - Add one out parameter to return status message from database.            
//2015.04.17    B. Jagadeesh    FOR BT-2570,YRS 5.0-2380:Added method to insert into print letter table.            
//2015.04.17    B. Jagadeesh    BT:2570 YRS 5.0-2380 To update the initial communication follow-up data into atsPrintLetters with IDMTrackingId
//2015.09.16    Manthan Rajguru YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2017.06.29    Santosh Bura    YRS-AT-2675 -  Annuity beneficiary restrictions. 
//****************************************************

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for AnnuityBeneficiaryDeathDAClass.
	/// </summary>
	public class AnnuityBeneficiaryDeathDAClass
	{

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
		public static DataSet LookUp_RetireePersonsList(string parameterSSN, string parameterLName ,string parameterFName)
		{
			DataSet l_dataset_Lookup_RetireePersonList = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_ABDS_Lookup_RetireePersonList");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_SSNo",DbType.String,parameterSSN);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_FirstName",DbType.String,parameterLName);
				db.AddInParameter(LookUpCommandWrapper, "@varchar_LastName",DbType.String,parameterFName);
			
				l_dataset_Lookup_RetireePersonList = new DataSet();
				db.LoadDataSet(LookUpCommandWrapper, l_dataset_Lookup_RetireePersonList,"r_MemberListForDeath");
				return l_dataset_Lookup_RetireePersonList;
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
			DataSet l_dataset_Lookup_AnnuityJointSurvivor = null;
			Database db = null;
			DbCommand LookUpCommandWrapper = null;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				LookUpCommandWrapper = db.GetStoredProcCommand("yrs_usp_ABDS_Lookup_AnnuityJointSurvivor");
				
				if (LookUpCommandWrapper == null) return null;

				db.AddInParameter(LookUpCommandWrapper, "@varchar_AnnuityId",DbType.String,parameterAnnuityId);

                l_dataset_Lookup_AnnuityJointSurvivor = new DataSet();
                db.LoadDataSet(LookUpCommandWrapper, l_dataset_Lookup_AnnuityJointSurvivor, "r_MemberListForDeath");
				return l_dataset_Lookup_AnnuityJointSurvivor;
			}
			catch 
			{
				throw;
			}
		}


		#region "CommentsinFoxproSave"
		/*
		*--------------------------------------------------------------------------------
		*-- fa.12.2.2003
		*--------------------------------------------------------------------------------
*-- On Save.
*--IF POPUP, HANDLE_POPUP()
*-- 1. Determine:
*--              PayPeriodsSinceDeath
*--              Max popup amount
*--              current payment
*--         calc paydelta = maxPay - currentPay
*--         calc UnderPay = paydelta * periodsSinceDeath     

*-- BEGIN TRANSACTION  (this is the juice right here. so we want to be sure it is all or nothing.)
*--  2. Update the JS.DeathDate field
*--  3. Update AtsBene.DeathDate (NOT YET! per Allen)
*--     Update AtsAnnuityCurrentValues
*-- 				mnyCurrentPayment = MaxPopupPayment
*--   				mnyYMCAPreTax = mnyYMCAPreTax + PayDelta
*--     Insert into AtsAnnuityAdjustments MaxPopup,Paydelta
*--  4. Handle_UnderPayment()
*--         Insert into AtsDisbursements (TotalUnderPayment,Supress)
*--			Insert into AtsDisbursementFunding
*--			Insert into AtsDisbursementAnnuities
*-- END TRANSACTION

*--IF LAST2DIE, HANDLE_LAST2DIE()
*-- 1. Determine:
*--              PayPeriodsSinceDeath
*--              PayAdjustPercent
*--              current payment
*--         calc paydelta = currentPay - (PayAdjustPercent * currentPay)
*--         calc OverPay = paydelta * periodsSinceDeath
*--ENDIF
																															 *--------------------------------------------------------------------------------
		*/
		#endregion

		public static int Process_BeneficiaryAnnuitySettlement(string parameterRetireeID, DateTime parameterBeneficiaryDeathDate )
		{
			// Probably this procedure is not Required ...... will delete it later.
			//SetDeathDate();
			//HandleAnnuityAdjustment();
						
			return 0;

		}
		//NP:PS:2007.10.09 - Passing Joint survivor Id instead of Retiree Id to handle death dates correctly. YRPS-3868
        //DineshK:2012.12.27: FOR BT-1266, YRS 5.0-1698:Cross check SSN when entering a date of death  

        public static int SetDeathDate(string parameterJointSurvivorID, DateTime parameterBeneficiaryDeathDate, string strParticipantSSNo, out string outpara_DisplayString, out string outpara_DeathNotify, Database db, DbTransaction transaction)  //SB| YRS-AT-2675 | 2017.06.29 | Added two new parameters (db,transaction) for applying transaction to current method (SetDeathDate)
		{
			/* Call  Procedure dbo.yrs_usp_ABDS_update_JointSurvivorAnnuities_deathDate with @datetime_BeneficiaryDeathDate datetime,@Varchar_RetireeID Varchar(20) */
			/* & Also update the textLog on the screen*/
			//DataSet l_dataset_Lookup_RetireePersonList = null;
			int int_returnStatus ; 
			int_returnStatus = 1 ;

			//Database db = null;  //SB| YRS-AT-2675 | 2017.06.29 | Since the parameter is passed through input so no need to create new Database object
			DbCommand UpdateCommandWrapper = null;
			outpara_DisplayString = "";
            outpara_DeathNotify = string.Empty;
			try 
			{
                //db = DatabaseFactory.CreateDatabase("YRS");   //SB| YRS-AT-2675 | 2017.06.29 | Since parameter is passed through input no need to initalise Database object
			
				if (db == null) return -1;

				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_ABDS_update_JointSurvivorAnnuities_DeathDate");
				
				if (UpdateCommandWrapper == null) return -1;

				db.AddInParameter(UpdateCommandWrapper, "@datetime_BeneficiaryDeathDate",DbType.String,parameterBeneficiaryDeathDate);
                db.AddInParameter(UpdateCommandWrapper, "@varchar_Participantssno", DbType.String, strParticipantSSNo);
				//UpdateCommandWrapper.AddInParameter("@Varchar_RetireeID",DbType.String,parameterRetireeID);
				db.AddInParameter(UpdateCommandWrapper, "@Varchar_JointSurvivorID", DbType.String, parameterJointSurvivorID);	//NP:PS:2007.10.09 - YSPS-3868
				db.AddOutParameter(UpdateCommandWrapper, "@varchar_DisplayString",DbType.String ,1000);
                //DineshK:2012.12.27: FOR BT-1266, YRS 5.0-1698: Cross check SSN when entering a date of death             
                db.AddOutParameter(UpdateCommandWrapper, "@cDeathNotifyOutPut", DbType.String, 1000);
                db.ExecuteNonQuery(UpdateCommandWrapper, transaction);  //SB| YRS-AT-2675 | 2017.06.29 | Added transaction object to the calling method
                //outpara_DisplayString = Convert.ToString( UpdateCommandWrapper.GetParameterValue("@varchar_DisplayString") );
                outpara_DisplayString = db.GetParameterValue(UpdateCommandWrapper, "@varchar_DisplayString").ToString();
                //DineshK:2012.12.27: FOR BT-1266, YRS 5.0-1698: Cross check SSN when entering a date of death             
                outpara_DeathNotify = db.GetParameterValue(UpdateCommandWrapper, "@cDeathNotifyOutPut").ToString();
				return int_returnStatus ;
			}
			catch 
			{
				throw; 
			}

		}

		//NP:PS:2007.10.09 - Passing Joint survivor Id instead of Retiree Id to handle death dates correctly. YRPS-3868
       public static int HandleAnnuityAdjustment(string parameterJointSurvivorID, DateTime parameterBeneficiaryDeathDate, out string outpara_DisplayString, out string outpara_ErrorMessage, Database db, DbTransaction transaction) //SB| YRS-AT-2675 | 2017.06.29 | Added two new parameters (db,transaction) for applying transaction to current method (HandleAnnuityAdjustment)
		{
			#region "Comment_FromFoxpro"
/*			//Check table "atsMetaAnnuityTypes" to verify if the AnnuityTpye passed by UI is a Valid one, if invalid Flag an error
			if ("bitPopup" = true)
				llResult = HandleAnnuityPopup();
				//Write this HandleAnnuityPopup at SQL Procedure 
				//This will also require call to another Procedure payPeriodsSinceDeath
			else if (bitLastToDie = true)
				llResult = HandleAnnuityLast2die(loAnnuityMeta.numJointSurvivorPctg);
			else Flag Error
*/		
			#endregion
			outpara_DisplayString = "" ;
			outpara_ErrorMessage = "" ;

			int int_returnStatus ; 
			int_returnStatus = 1 ;

            //Database db = null;  // SB| YRS-AT-2675 | 2017.06.29 | Since parameter is passed through input no need to initalise Database object
			DbCommand UpdateCommandWrapper = null;
			outpara_DisplayString = "";	
			try 
			{
                //db = DatabaseFactory.CreateDatabase("YRS");  //SB| YRS-AT-2675 | 2017.06.29 | Since parameter is passed through input no need to initalise Database object
			
				if (db == null) return -1;

				UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_ABDS_Main_HandleAnnuityAdjustment");
				
				if (UpdateCommandWrapper == null) return -1;

				db.AddInParameter(UpdateCommandWrapper, "@datetime_idjsdeathdate", DbType.String, parameterBeneficiaryDeathDate);
				//UpdateCommandWrapper.AddInParameter("@varchar_icRetireeId", DbType.String, parameterRetireeID);
				db.AddInParameter(UpdateCommandWrapper, "@varchar_icJointSurvivorId", DbType.String, parameterJointSurvivorID);	//NP:PS:2007.10.09 - YSPS-3868 - Passing JointSurvivorId instead of RetireeId.
				
				db.AddOutParameter(UpdateCommandWrapper, "@varchar_DisplayString",DbType.String ,4000);
				db.AddOutParameter(UpdateCommandWrapper, "@varchar_ErrorString",DbType.String ,1000);
				UpdateCommandWrapper.CommandTimeout=Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings ["MediumConnectionTimeOut"]); ;

                db.ExecuteNonQuery(UpdateCommandWrapper, transaction); // SB| YRS-AT-2675 | 2017.06.29 |  Added transaction object to the calling method
				//outpara_DisplayString = Convert.ToString( UpdateCommandWrapper.GetParameterValue("@varchar_DisplayString") );
				//outpara_ErrorMessage  = Convert.ToString( UpdateCommandWrapper.GetParameterValue("@varchar_ErrorString") );
                outpara_DisplayString = db.GetParameterValue(UpdateCommandWrapper, "@varchar_DisplayString").ToString();
                outpara_ErrorMessage = db.GetParameterValue(UpdateCommandWrapper, "@varchar_ErrorString").ToString();
                return int_returnStatus;
			}
			catch 
			{
				throw; 
			}

		}
        
        //JB BT:2570 YRS 5.0-2380 To insert the initial communication follow-up data into atsPrintLetters
        public static int InsertPrintLetters(string strAnnuityJointSurvivorsId, string strPersId, string strLettersCode)
        {
            int l_intPrintLettersId = -1;
            Database db = null;
            DbCommand InsertCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return l_intPrintLettersId;
                Guid guiPersId = Guid.Parse(strPersId);
                InsertCommandWrapper = db.GetStoredProcCommand("yrs_usp_InsertPrintLetters");
                if (InsertCommandWrapper == null) return l_intPrintLettersId;
                db.AddInParameter(InsertCommandWrapper, "@chvRefId", DbType.String, strAnnuityJointSurvivorsId);
                db.AddInParameter(InsertCommandWrapper, "@guiPersId", DbType.Guid, guiPersId);
                db.AddInParameter(InsertCommandWrapper, "@chvLettersCode", DbType.String, strLettersCode);

                db.AddOutParameter(InsertCommandWrapper, "@out_intUniqueId", DbType.Int32, 5);
                db.ExecuteNonQuery(InsertCommandWrapper);

                return Convert.ToInt32(db.GetParameterValue(InsertCommandWrapper, "@out_intUniqueId"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //JB BT:2570 YRS 5.0-2380 To update the initial communication follow-up data into atsPrintLetters with IDMTrackingId
        public static void UpdatePrintLetters(int intPrintLettersId, int intIDMTrackingId)
        {
            Database db = null;
            DbCommand UpdateCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return;
                UpdateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdatePrintLetters");
                if (UpdateCommandWrapper == null) return;
                db.AddInParameter(UpdateCommandWrapper, "@int_PrintLettersId", DbType.Int32, intPrintLettersId);
                db.AddInParameter(UpdateCommandWrapper, "@int_IDMTrackingId", DbType.Int32, intIDMTrackingId);

                db.ExecuteNonQuery(UpdateCommandWrapper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // START: SB | 06/29/2017 | YRS-AT-2675 | Method will update the death date also handle annutiy adjustments, transaction is used in the function.If any error invokes whole transaction will be rolled back else transaction will be commited 
        public static int SetDeathDateAndHandleAnnuityAdjustments(string jointSurvivorID, DateTime beneficiaryDeathDate, string participantSSNo, out string displayString, out string deathNotify, out string errorMessage)
        {
            Database db = null;
            DbConnection connection = null;
            //for maintaining transaction
            DbTransaction transaction = null;
            string deathDate = "";
            string anuityAdjustmentMessage = "";
            int returnStatus = 1;

            displayString = "";
            deathNotify = string.Empty;
            errorMessage = string.Empty;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");
                if (db == null) return -1;
                connection = db.CreateConnection();
                connection.Open();

                // Transaction object is created, which will rollback whole transaction if any error arises 
                transaction = connection.BeginTransaction();

                // in following function if the letter is already generated to the participant and the response it not yet received in this case the function will return 2 else it will return 1 and updates the death date
                returnStatus = SetDeathDate(jointSurvivorID, beneficiaryDeathDate, participantSSNo, out deathDate, out deathNotify, db, transaction);
                if (deathDate.Trim() == "2")
                {
                    displayString = deathDate.Trim();
                    transaction.Rollback(); //  transaction will be rolled back and hence no need to call annuitiy adjustments function as death notification is not successful 
                    return returnStatus;
                }
                //After updation of death death annuity adjustments function is called 
                returnStatus = HandleAnnuityAdjustment(jointSurvivorID, beneficiaryDeathDate, out anuityAdjustmentMessage, out errorMessage, db, transaction);
                displayString = string.Format("{0}{1}", deathDate, anuityAdjustmentMessage.Trim());

                transaction.Commit();
                return returnStatus;
            }
            catch
            {
                //Rollback
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally   //Close all connection related objects, Clear all the transaction related objects
            {
                if (connection != null)
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                connection = null;
                transaction = null;
                db = null;
            }
        }
        // END: SB | 06/29/2017 | YRS-AT-2675 | Method will update the death date also handle annutiy adjustments, transaction is used in the function.If any error invokes whole transaction will be rolled back else transaction will be commited 
    }
}

