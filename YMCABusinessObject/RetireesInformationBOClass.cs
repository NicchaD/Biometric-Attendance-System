//'************************************************************************************
// Project Name		:	33156	
// FileName			:	RetirementEstimateUIProcess.cs
// Author Name		:	Sameer joshi	
// Employee ID		:	33156
// Email			:	sameer.joshi@3i-infotech.com
// Contact No		:	55928743
// Creation Time	:	6/1/2005 7:28:12 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//'************************************************************************************
//'Modification History
//'************************************************************************************
//'Modified By        Date            Description
//'************************************************************************************
//'Dilip Yadav        2009.09.08      YRS 5.0-852
//Priya				  05-April-2010	  YRS 5.0-1042:New "flag" value in Person/Retiree maintenance screen
//Sanjay R.		      01-Aug-2012     BT-753/YRS 5.0-1270 : purchase page
//Anudeep             13-JUN-2013     BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
//Anudeep             02.Jul.2013     Bt-1501:YRS 5.0-1745:Capture Beneficiary addresses 
//Manthan Rajguru     2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Pramod P. Pokale    2015.10.07      YRS-AT-2361: Need to prevent Duplicate banking records from being created 
//Manthan Rajguru 	  2017.12.04      YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
//'************************************************************************************

using System;
using YMCARET.YmcaDataAccessObject;
using System.Data;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for RetireesInformationBOClass.
	/// </summary>
	public class RetireesInformationBOClass
	{
		public RetireesInformationBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns></returns>

		public static DataSet LookUpAnnuityPaid(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpAnnuityPaid(parameterPersId);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns></returns>

		public static DataSet LookUpDisbursement(string parameterDisbursementId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpDisbursement(parameterDisbursementId);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// 
		
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns></returns>

		public static DataSet LookUpAnnuities(string parameterFundEventId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpAnnuities(parameterFundEventId);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns></returns>

		public static DataSet LookUpAnnuityInfo(string parameterAnnuityId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpAnnuityInfo(parameterAnnuityId);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns></returns>

		public static DataSet LookUpBeneficiaries(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpBeneficiaries(parameterPersId);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns></returns>

		public static DataSet LookUpBanks(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpBanks(parameterPersId);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// </summary>
		/// <param name="parameterPersId"></param>
		/// <returns></returns>
	    /*  Commented By Ashutosh Patil as on 17-01-2007 as Current Status of Payment Method will be done at front end
		public static DataSet LookUpRetireesStatus(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpRetireesStatus(parameterPersId);
			}
			catch
			{
				throw;
			}
		}
		*  Commented By Ashutosh Patil as on 17-01-2007 as Current Status of Payment Method  will be done at front end */
		/// <summary>
		/// 
		/// <param name="parameterPersId"></param>
		/// <returns></returns>

		public static DataSet LookUpFedWithDrawals(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpFedWithDrawals(parameterPersId);
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// 
		public static DataSet LookUpGenWithDrawals(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpGenWithDrawals(parameterPersId);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpAnnuityDetails(string parameterPersId,string parameterAnnuityId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpAnnuityDetails(parameterPersId,parameterAnnuityId);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpAdjustments(string parameterAnnuityId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpAdjustments(parameterAnnuityId);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet TaxEntityTypes()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.TaxEntityTypes();
			}
			catch
			{
				throw;
			}
		}
		public static DataSet MaritalTypes(int parameterIsFedTaxForMaritalStatus)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.MaritalTypes(parameterIsFedTaxForMaritalStatus);
			}
			catch
			{
				throw;
			}
		}

		// Start : added by Dilip yadav on 2009.09.08 for YRS 5.0-852
		public static DataSet GenderTypes()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.GenderTypes();
			}
			catch
			{
				throw;
			}
		}


        // Start : added by Dinesh Kanojia on 2012.12.10 for YRS 5.0-852
        public static DataSet POACategoryTypes()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.POACategoryTypes();
            }
            catch
            {
                throw;
            }
        }


        // Start : added by Dinesh Kanojia on 2013.01.31 for YRS 5.0-1697
        public static DataSet BenificiaryDeleteReasons()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.BenificiaryDeleteReasons();
            }
            catch
            {
                throw;
            }
        }

		// End : added by Dilip yadav on 2009.09.08 for YRS 5.0-852
		public static DataSet WithHoldingTypes()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.WithHoldingTypes();
			}
			catch
			{
				throw;
			}
		}//

		public static DataSet getGenCodes()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.getGenCodes();
			}
			catch
			{
				throw;
			}
		}

		public static void InsertRetireesFedWithdrawals(DataSet dsFedWithDrawals)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.InsertRetireesFedWithdrawals(dsFedWithDrawals);
			}
			catch
			{
				throw;
			}
		}
		public static void InsertRetireesGenWithdrawals(DataSet dsGenWithDrawals)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.InsertRetireesGenWithdrawals(dsGenWithDrawals);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getRelationShips()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.getRelationShips();
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getBenefitGroups()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.getBenefitGroups();
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getBenefitLevels()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.getBenefitLevels();
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getBenefitTypes()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.getBenefitTypes();
			}
			catch
			{
				throw;
			}
		}//

		public static void InsertBeneficiaries(DataSet dsBeneficiaries)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.InsertBeneficiaries(dsBeneficiaries);
			}
			catch
			{
				throw;
			}
		}
		public static void InsertRetireeNotes(DataSet dsNotes)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.InsertRetireeNotes(dsNotes);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getPOADetails(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.getPOADetails(parameterPersId);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getPOAAddrDtls(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.getPOAAddrDtls(parameterPersId);
			}
			catch
			{
				throw;
			}
		}
		public static void InsertPOADetails(string Persid,string EffectiveDate,string TerminationDate,string Name1,string Name2,string Comments,string Addr1,string Addr2,string Addr3,string City,string Country,string State,string Zip,string POACategory)
		{
			try
			{
                //YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.InsertPOADetails(Persid, EffectiveDate, TerminationDate, Name1, Name2, Comments, Addr1, Addr2, Addr3, City, Country, State, Zip, POACategory);
                //YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.InsertPOADetails(Persid, EffectiveDate, TerminationDate, Name1, Name2, Comments,Addr,POACategory);
			}
			catch
			{
				throw;
			}
		}
        public static void UpdatePOADetails(string Persid, string EffectiveDate, string TerminationDate, string Name1, string Name2, string Comments, string Addr1, string Addr2, string Addr3, string City, string Country, string State, string Zip, string guiuniqueid, string POACategory)
		{
			try
			{
                //YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.UpdatePOADetails(Persid, EffectiveDate, TerminationDate, Name1, Name2, Comments, Addr1, Addr2, Addr3, City, Country, State, Zip, guiuniqueid, POACategory);
			}
			catch
			{
				throw;
			}
		}
		//by Aparna --changing the parameter to the datatable
//		public static void UpdateAnnuityJointSurvivors(string AnnuityGUID,string SSNO,string BirthtDate,string DeathDate,string MiddleName,string FirstName,string bitspouse,string LastName)
//		{
//			try
//			{
//				YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.UpdateAnnuityJointSurvivors(AnnuityGUID,SSNO,BirthtDate,DeathDate,MiddleName,FirstName,bitspouse,LastName);
//			}
//			catch
//			{
//				throw;
//			}
//		}

		public static void UpdateAnnuityJointSurvivors(DataSet parameterAnnuityJointSurvivors)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.UpdateAnnuityJointSurvivors(parameterAnnuityJointSurvivors);
			}
			catch
			{
				throw;
			}
		}


		public static void InsertBankingInfo(DataSet dsBanking)
		{
			try
			{
				YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.InsertBankingInfo(dsBanking);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet PHRDetails()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.PHRDetails();
			}
			catch
			{
				throw;
			}
		}
		//Priya 05-April-2010 : YRS 5.0-1042:New "flag" value in Person/Retiree maintenance screen
		//Commented OldGuardNews check box code to remove it from screen
//		public static void DeleteOldGuardNews(string parameterPersId)
//		{
//			try
//			{
//				YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.DeleteOldGuardNews(parameterPersId);
//			}
//			catch
//			{
//				throw;
//			}
//		}
//		public static void InsertOldGuardNews(string parameterPersId, string parameterCreator)
//		{
//			try
//			{
//				YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.InsertOldGuardNews(parameterPersId,parameterCreator);
//			}
//			catch
//			{
//				throw;
//			}
//		}
		public static int LookUpOldGuardNews(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpOldGuardNews(parameterPersId);
			}
			catch
			{
				throw;
			}
		}


		public static void  UpdateJSBeneficiaries(string AnnuityGUID,string SSNO,string BirthtDate,string DeathDate,string MiddleName,string FirstName,string bitspouse,string LastName)
		{
			try
			{
				 YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.UpdateAnnuityJointSurvivors( AnnuityGUID, SSNO, BirthtDate, DeathDate, MiddleName, FirstName, bitspouse, LastName);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet LookUpGeneralInfo(string parameterPersId, string parameterFundId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.LookUpGeneralInfo(parameterPersId,parameterFundId);
			}
			catch
			{
				
				throw;
			}
		}

		public static int IsBeneficiaryRequired(string parameterpersid)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.IsBeneficiaryRequired(parameterpersid);
			}
			catch
			{
				throw;
			}
		}


        //SR:01-Aug-2012:BT-753/YRS 5.0-1270 : Use description field in place of short decription for general witholding codes
        public static DataSet getGenCodeWithDescription()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.getGenCodeWithDescription();
            }
            catch
            {
                throw;
            }
        }
        //End :SR:01-Aug-2012:BT-753/YRS 5.0-1270 : Use description field in place of short decription for general witholding codes
        
        //Start: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form
        //Checking in database with respect to persid that person has surviour who is not primary deathbeneficiary
        public static bool GetJSBeneficiaries(string strPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.GetJSBeneficiaries(strPersId);
            }
            catch
            {
                throw;
            }
        }

        //Anudeep:13.06.2013 - BT-1261:YRS 5.0-1695:Need to capture 'reason' for beneficiary deletion
        public static string CheckBeneficiaryExistenceInOtherModules(string strSSNO, string strParticipantSSNo)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.CheckBeneficiaryExistenceInOtherModules(strSSNO, strParticipantSSNo);
            }
            catch
            {
                throw;
            }
        }

        public static void InsertPOA(DataSet dsPoaDetails)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.InsertPOA(dsPoaDetails);
            }
            catch
            {
                throw;
            }
        }
        public static void UpdatePOA(DataSet dsPoaDetails)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.UpdatePOA(dsPoaDetails);
            }
            catch
            {
                throw;
            }
        }
        //End: Anudeep:28.02.2013 :YRS 5.0-1707:New Death Benefit Application form

        //START: PPP | 2015.10.07 | YRS-AT-2361 | Checking for duplicate bank records on the basis of Effective date, 2 banks cannot be registered with same effective date
        public static bool IsDuplicateBankRecordExists(DataSet dsBanking, string stPerssID)
        {
            DataSet dsExistingBanks, dsNewBanks;
            DataRow[] drBank;
            bool bExists;
            try
            {
                bExists = false;

                dsExistingBanks = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpBanks(stPerssID);
                if (dsExistingBanks != null && dsExistingBanks.Tables.Count > 0 && dsExistingBanks.Tables[0].Rows.Count > 0)
                {
                    dsNewBanks = dsBanking.GetChanges(DataRowState.Added);
                    if (dsNewBanks != null && dsNewBanks.Tables.Count > 0 && dsNewBanks.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dsNewBanks.Tables[0].Rows)
                        {
                            drBank = dsExistingBanks.Tables[0].Select(string.Format("dtmEffDate='{0:MM/dd/yyyy}'", row["dtmEffDate"]));
                            if (drBank != null && drBank.Length > 0)
                            {
                                bExists = true;
                                break;
                            }
                        }
                    }
                }
                return bExists;
            }
            catch
            {
                throw;
            }
            finally
            {
                dsNewBanks = null;
                dsExistingBanks = null;
            }
        }
        //END: PPP | 2015.10.07 | YRS-AT-2361 | Checking for duplicate bank records on the basis of Effective date, 2 banks cannot be registered with same effective date

        //START: MMR | 2017.12.04 | YRS-AT-3756 | Added to get deceased beneficiary details
        public static DataSet GetDeceasedBeneficiary(string persId, string beneficiaryType)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RetireesInformationDAClass.GetDeceasedBeneficiary(persId, beneficiaryType);
            }
            catch
            {
                throw;
            }
        }
        //END: MMR | 2017.12.04 | YRS-AT-3756 | Added to get deceased beneficiary details
	}
	
	
}
