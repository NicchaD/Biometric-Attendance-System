//*******************************************************************************
//CHANGE HISTORY
//------------------------------------------------------------------
// Date			Modified by		Description
//------------------------------------------------------------------
// 2008.04.11	Nikunj Patel	Changes related to Phase IV - Part 1 Delivery
//								Added code to allow forcing of compuations for non-retired fundevents as either DA/DI
// 2008.07.09	Nikunj Patel	Adding call to stored procedure to identify basic account balance.
// 2010.01.06   Sanjay Rawat    Added function to determine the Market Based Amount.
// 2012.11.26   Sanjay Rawat    YRS 5.0-1707:Adding function to find no. of Payroll month since death.
// 2012.11.26   Sanjay Rawat    YRS 5.0-1707:New Death Benefit Application form 
// 2012.12.04   Anudeep Adusumilli YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up
// 2012.12.14   Sanjay Rawat    YRS 5.0-1707:To get the JS annuity details 
// 2013.06.26 - Sanjay Rawat    BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
// 2013.08.12 - Anudeep Adusumilli Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
// 2014.08.12 - Anudeep Adusumilli BT:2460:YRS 5.0-2331 YRS 5.0-2331 - Death Benefit Application Form 
// 2014.12.02   Shashank Patel  BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
// 2015.09.16   Manthan Rajguru YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
// 2015.09.16   Anudeep Adusumilli YRS-AT-2478 - Death Benefit Application to show taxable and non-taxable amounts(TrackIT 21695)
// 2016.08.05   Chandra Sekar  YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
// 2020.02.10   Manthan R       YRS-AT-4770 -  Death Calculator must restrict annuity options under SECURE Act (TrackIT-41080)
//*******************************************************************************

using System;
using System.Data ;
using System.Collections.Generic;
//using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for DeathBenefitsCalculator.
	/// </summary>
	public sealed class DeathBenefitsCalculatorBOClass
	{
		public DeathBenefitsCalculatorBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DeathBenefitsCalculatorBOClass GetInstance()
		{
			
			return Nested.instance;
		}
    
		class Nested
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested()
			{
			}

			internal static readonly DeathBenefitsCalculatorBOClass instance = new DeathBenefitsCalculatorBOClass();
		}

		//NP:PS:2007.09.05 - Adding code to handle parameter FundNo for searches
		public static DataSet LookUp_DeathCalc_MemberListForDeath(string lcSSNo,string parameterLName , string parameterFName, string parameterFundNo )
		{	
			
			try
			{
				return (YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.LookUp_DeathCalc_MemberListForDeath(lcSSNo, parameterLName, parameterFName, parameterFundNo)) ;
			}
			catch 
			{
				throw;
			}

		}

		//NP:IVP1:2008.04.11 - Adding parameter ForceCalculationsAs to the list to enable calculations as either DA or DI for non-retired fundevent statuses
		//	Valid values for parameterForceCalculationsAs are:
		//		DA: To consider as Active Death - (applied $10k death benefit)
		//		DI:	To consider as Inaction Death - (does not give $10k death benefit)
		//		Null or Empty: To use the default logic for computations
		public static DataSet Calculate_Death_Benefits(string parameterPersId, string parameterFundEventID , DateTime parameterDeathDate, string parameterFundStatus, string parameterForceCalculationsAs, out int l_int_returnStatus)
		{	
			
			try
			{
				//return (YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.LookUp_DeathCalc_MemberListForDeath(parameterPersId, parameterFundEventID, parameterDeathDate)) ;
                return (YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetDeathCalulation(parameterPersId, parameterFundEventID, parameterDeathDate, parameterFundStatus, parameterForceCalculationsAs, out l_int_returnStatus));
			}
			catch 
			{
				throw;
			}

		}

		public static string GetAnnuities(string parameterFundEventId) 
		{
			return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetAnnuities(parameterFundEventId);
		}

		//NP:2008.07.09 - Adding function to determine the account balance.
		public static decimal GetBasicAccountBalance(string parameterFundEventId) 
		{
			return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetBasicAccountBalance(parameterFundEventId);
		}


		//SR:2010.01.06 - Adding function to determine the Market Based Amount.
		public static decimal GetMarketBasedAmount(string parameterFundEventId,string ParameterFundStatus) 
		{
			return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetMarketBasedAmount(parameterFundEventId,ParameterFundStatus);
		}

        //SR:2012.11.26 - Adding function to find no. of Payroll month since death.
        public static string GetPayrollMonthsSinceDeath(DateTime  parameterDeathDate) 
		{
            return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetPayrollMonthsSinceDeath(parameterDeathDate);
		}

        //SR:2012.11.26 -   YRS 5.0-1707:New Death Benefit Application form 
        //SP 2014.12.04 BT-2310\YRS 5.0-2255: Added above parameters into below method "strRepFirstName, strRepLastName, strRepSalutation &strRepTelephone"
        public static string SaveDeathBenefitCalculatorFormDetails( string strdecsdPersID, 
                                                                    string strdecsdFundeventID, 
                                                                    string StrBeneficiaryName,                                                        
                                                                    decimal decJSAnnuityAmount, 
                                                                    decimal decRetPlan, 
                                                                    decimal decPrincipalGuaranteeAnnuity_RP, 
                                                                    decimal decSavPlan, 
                                                                    decimal decPrincipalGuaranteeAnnuity_SP,
                                                                    decimal decRetiredDeathBenefit, 
                                                                    decimal decAnnuityMFromRP, 
                                                                    decimal decFirstMAnnuityFromRP, 
                                                                    decimal decAnnuityCFromRP, 
                                                                    decimal decFirstCAnnuityFromRP, 
                                                                    decimal decLumpSumFromNonHumanBen,
                                                                    decimal decAnnuityMFromSP, 
                                                                    decimal decFirstMAnnuityFromSP, 
                                                                    decimal decAnnuityCFromSP, 
                                                                    decimal decFirstCAnnuityFromSP, 
                                                                    decimal decAnnuityMFromRDB, 
                                                                    decimal decFirstMAnnuityFromRDB, 
                                                                    decimal decAnnuityFromJSAndRDB, 
                                                                    decimal decFirstAnnuityFromJSAndRDB, 
                                                                    decimal decAnnuityMFromResRemainingOfRP, 
                                                                    decimal decFirstMAnnuityFromResRemainingOfRP, 
                                                                    decimal decAnnuityCFromResRemainingOfRP, 
                                                                    decimal decFirstCAnnuityFromResRemainingOfRP, 
                                                                    decimal decAnnuityMFromResRemainingOfSP, 
                                                                    decimal decFirstMAnnuityFromResRemainingOfSP, 
                                                                    decimal decAnnuityCFromResRemainingOfSP, 
                                                                    decimal decFirstCAnnuityFromResRemainingOfSP, 
                                                                    int intMonths, 
                                                                    bool blnActiveDeathBenfit,
                                                                    bool blnSection1Visible, 
                                                                    bool blnSection2Visible, 
                                                                    bool blnSection3Visible, 
                                                                    bool blnSection4Visible, 
                                                                    bool blnSection5Visible, 
                                                                    bool blnSection6Visible, 
                                                                    bool blnSection7Visible, 
                                                                    bool blnSection8Visible, 
                                                                    bool blnSection9Visible, 
                                                                    bool blnSection10Visible, 
                                                                    bool blnSection11Visible, 
                                                                    bool blnSection12Visible,
                                                                    bool blnCopyIDM,
                                                                    bool blnFollowUp,
                                                                    string strAddressID, 
                                                                    string StrBeneficiaryFirstName,
                                                                    string StrBeneficiaryLastName,
                                                                    string StrBeneficiarySSNo
                                                                    ,string strRepFirstName
                                                                    ,string strRepLastName 
                                                                    ,string strRepSalutation
                                                                    ,string  strRepTelephone ,
                                                                    decimal decRetTaxable,
                                                                    decimal decRetNonTaxable,
                                                                    decimal decSavTaxable,
                                                                    decimal decSavNonTaxable,
                                                                    //START - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
                                                                    decimal decPrincipalGuaranteeAnnuityRetTaxableAmt,
                                                                    decimal decPrincipalGuaranteeAnnuityRetNonTaxableAmt,
                                                                    decimal decPrincipalGuaranteeAnnuitySavTaxableAmt,
                                                                    decimal decPrincipalGuaranteeAnnuitySavNonTaxableAmt
                                                                    //END - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
                                                                    )
        {
            return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.SaveDeathBenefitCalculatorFormDetails(strdecsdPersID, strdecsdFundeventID,
                                                                                                                                        StrBeneficiaryName,
                                                                                                                                        decJSAnnuityAmount,
                                                                                                                                        decRetPlan,
                                                                                                                                        decPrincipalGuaranteeAnnuity_RP,
                                                                                                                                        decSavPlan,
                                                                                                                                        decPrincipalGuaranteeAnnuity_SP,
                                                                                                                                        decRetiredDeathBenefit,
                                                                                                                                        decAnnuityMFromRP,
                                                                                                                                        decFirstMAnnuityFromRP,
                                                                                                                                        decAnnuityCFromRP,
                                                                                                                                        decFirstCAnnuityFromRP,
                                                                                                                                        decLumpSumFromNonHumanBen,
                                                                                                                                        decAnnuityMFromSP,
                                                                                                                                        decFirstMAnnuityFromSP,
                                                                                                                                        decAnnuityCFromSP,
                                                                                                                                        decFirstCAnnuityFromSP,
                                                                                                                                        decAnnuityMFromRDB,
                                                                                                                                        decFirstMAnnuityFromRDB,
                                                                                                                                        decAnnuityFromJSAndRDB,
                                                                                                                                        decFirstAnnuityFromJSAndRDB,
                                                                                                                                        decAnnuityMFromResRemainingOfRP,
                                                                                                                                        decFirstMAnnuityFromResRemainingOfRP,
                                                                                                                                        decAnnuityCFromResRemainingOfRP,
                                                                                                                                        decFirstCAnnuityFromResRemainingOfRP,
                                                                                                                                        decAnnuityMFromResRemainingOfSP,
                                                                                                                                        decFirstMAnnuityFromResRemainingOfSP,
                                                                                                                                        decAnnuityCFromResRemainingOfSP,
                                                                                                                                        decFirstCAnnuityFromResRemainingOfSP,
                                                                                                                                        intMonths,
                                                                                                                                        blnActiveDeathBenfit,
                                                                                                                                        blnSection1Visible,
                                                                                                                                        blnSection2Visible,
                                                                                                                                        blnSection3Visible,
                                                                                                                                        blnSection4Visible,
                                                                                                                                        blnSection5Visible,
                                                                                                                                        blnSection6Visible,
                                                                                                                                        blnSection7Visible,
                                                                                                                                        blnSection8Visible,
                                                                                                                                        blnSection9Visible,
                                                                                                                                        blnSection10Visible,
                                                                                                                                        blnSection11Visible,
                                                                                                                                        blnSection12Visible,
                                                                                                                                        blnCopyIDM,
                                                                                                                                        blnFollowUp ,
                                                                                                                                        strAddressID, 
                                                                                                                                        StrBeneficiaryFirstName, 
                                                                                                                                        StrBeneficiaryLastName,
                                                                                                                                        StrBeneficiarySSNo,
                                                                                                                                        strRepFirstName,
                                                                                                                                        strRepLastName ,
                                                                                                                                        strRepSalutation,
                                                                                                                                        strRepTelephone,                                                                                                                                        
                                                                                                                                        decRetTaxable,
                                                                                                                                        decRetNonTaxable,
                                                                                                                                        decSavTaxable,
                                                                                                                                        decSavNonTaxable,
                                                                                                                                        //START - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
                                                                                                                                        decPrincipalGuaranteeAnnuityRetTaxableAmt,
                                                                                                                                        decPrincipalGuaranteeAnnuityRetNonTaxableAmt,
                                                                                                                                        decPrincipalGuaranteeAnnuitySavTaxableAmt,
                                                                                                                                        decPrincipalGuaranteeAnnuitySavNonTaxableAmt
                                                                                                                                        //END - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
                                                                                                                                        );    
        }

        //AA :2015.10.12 YRS-AT-2478 Added new columns to store taxable and non taxable money
        //Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up
        public static DataSet GetMetaAdditionalForms()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetMetaAdditionalForms();
            }
            catch
            {
                throw;
            }
        }
        public static void SaveDeathFormDetails(DataTable dtDeathBenefitFormReqdDocs)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.SaveDeathFormDetails(dtDeathBenefitFormReqdDocs);
            }
            catch
            {
                throw;
            }
        }
        //Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up

        //Anudeep A :2012.12.04-YRS 5.0-1707:New Death Benefit Application form 
        //To get the beneficiary details before caluculations
        public static DataSet PopulateBeneficiaries(string parameterPersId, string parameterFundEventID, DateTime parameterDeathDate, string parameterFundStatus)
        {          
            
            try
            {
                return (YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.PopulateBeneficiaries(parameterPersId, parameterFundEventID, parameterDeathDate, parameterFundStatus));
                
            }
            catch
            {
                throw;
            }
        }

        

         //Sanjay R.:2012.12.14-YRS 5.0-1707:New Death Benefit Application form 
        //To get the JS annuity details
        public static DataSet PopulateJSAnnuities(string parameterFundEventID)
        {        
           
            try
            {
                return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.PopulateJSAnnuities(parameterFundEventID);
                
            }
            catch
            {
                throw;
            }
        }


        //SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
        public static DataSet GetBeneficiaryAdress(string parameterSSN, string parameterPersId, string paramFirstName, string paramLastName)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetBeneficiaryAdress(parameterSSN, parameterPersId,paramFirstName,paramLastName);

            }
            catch
            {
                throw;
            }
        }

        //Start;Anudeep:12.08.2013:Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
        public static DataSet GetFollowupDays()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetFollowupDays();
            }
            catch
            {
                throw;
            }
        }

        public static void SaveResponse(List<Dictionary<string,string>> LTerminationIds)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.SaveResponse(LTerminationIds);
            }
            catch 
            {
                
                throw;
            }
        }

        public static void UpdateFollowupStatus(List<Dictionary<string, string>> LTerminationIds)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.UpdateFollowupStatus(LTerminationIds);
            }
            catch
            {

                throw;
            }
        }
        //End:Anudeep:12.08.2013:Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.

        // Start: Anudeep BT:2460:YRS 5.0-2331 YRS 5.0-2331 - Added to get the uniqueid of joint survivor address 
        public static DataSet GetJointSurviourAdress(string parameterSSN, string parameterPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetJointSurviourAdress(parameterSSN, parameterPersId);

            }
            catch
            {
                throw;
            }
        }
        // End: Anudeep BT:2460:YRS 5.0-2331 YRS 5.0-2331 - Added to get the uniqueid of joint survivor address 

        // Start: 2014.12.02 Shashank BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
        public static DataSet GetFollowupBeneficiariesDetails(string paramPersId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetFollowupBeneficiariesDetails(paramPersId);
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetFollowUpRepresentativeDetails(string paramPersId, string paramGuiBeneficiaryId, string paramBeneSSN)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DeathBenefitsCalculatorDAClass.GetFollowUpRepresentativeDetails(paramPersId, paramGuiBeneficiaryId, paramBeneSSN);
            }
            catch
            {
                throw;
            }
        }
        // End: 2014.12.02  Shashank BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN

        //START: MMR |2020.02.10 | YRS-AT-4770 | Added method for secure act rules 
        /// <summary>
        /// Get beneficiary Details(Birth Date, Relationship code)
        /// </summary>
        /// <param name="beneficiaryId">Beneficiary ID</param>
        /// <returns>Beneficiary details in Table</returns>
        public static DataTable GetBeneficiaryDetails(string beneficiarySSNo)
        {
            try
            {
                return YmcaBusinessObject.SecureActBOClass.GetBeneficiaryDetails(beneficiarySSNo);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get secured act rule applicable or not(True/False)
        /// </summary>
        /// <param name="perssBirthDate">Participant Birth Date</param>
        /// <param name="beneficiaryBirthDate">Beneficiary Birth Date</param>
        /// <param name="beneficiaryRelationShipCode">Beneficiary Relationship Code</param>
        /// <param name="cutOffDate">Participant Death Date</param>
        /// <param name="chronicallyIll">Participant Chronically Ill</param>
        /// <returns>True/False</returns>
        public static Boolean IsSecureActApplicable(DateTime perssBirthDate, DateTime beneficiaryBirthDate, string beneficiaryRelationShipCode, DateTime cutOffDate, bool chronicallyIll)
        {
            try
            {
                return YmcaBusinessObject.SecureActBOClass.IsSecureActApplicable(perssBirthDate, beneficiaryBirthDate, beneficiaryRelationShipCode, cutOffDate, chronicallyIll);
            }
            catch
            {
                throw;
            }
        }
        //END: MMR |2020.02.10 | YRS-AT-4770 | Added method for secure act rules
	}
   
}
