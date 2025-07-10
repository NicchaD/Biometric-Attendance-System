//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YRS-YMCA
// FileName			:	RefundBeneficiaryBOClass.cs
// Author Name		:	Vipul Patel
// Employee ID		:	32900
// Email			:	vipul.patel@3i-infotech.com
// Contact No		:	55928738
// Creation Time	:	10/19/2005 
// Program Specification Name	:	
// Unit Test Plan Name			:	
//*******************************************************************************
//Change History
//*******************************************************************************
//Modified by           Date            Description
//****************************************************
// Sanjay R.            2014.05.07      YRS 5.0-2188 : RMDs for Beneficiaries 
// Manthan Rajguru      2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Sanjay GS Rawat       2016.12.07 	    YRS-AT-3222 - YRS enh-allow regenerate RMD for deceased participants Phase 2 of 2 (TrackIT 27024) 
//Sanjay GS Rawat       2017.12.04   	YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
//*******************************************************************************

using System;
using System.Data;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for RefundBeneficiaryBOClass.
	/// </summary>
	public class RefundBeneficiaryBOClass
	{
		public RefundBeneficiaryBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet Get_BeneficiaryBenefitDetails4Refund (string paramBeneficiaryBenefitOptionID)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.RefundBeneficiaryDAClass.Get_BeneficiaryBenefitDetails4Refund(paramBeneficiaryBenefitOptionID);
				
			}
			catch
			{
				throw;
			}
		}

		public static void Get_RefundRolloverInstitutionID(string paramRolloverInstitutionName, out string outpara_RolloverInstitutionUniqueID )
		{
			/* Call  Procedure dbo.yrs_usp_BS_Get_RefundRolloverInstitutionID with Institution Name
				/* If it exists use it, else  INSERT new record & use that Unique ID  */
				
			try 
			{ 

				YMCARET.YmcaDataAccessObject.RefundBeneficiaryDAClass.Get_RefundRolloverInstitutionID(paramRolloverInstitutionName, out outpara_RolloverInstitutionUniqueID) ;

			}
			catch
			{
				throw;
			}
		}

        //Start - SR:2014.05.07:YRS 5.0-2188: RMDs for Beneficiaries 
        public static DataSet GetBeneficiaryRMDs(string paramDeathBenefitOptionID, decimal paramTaxableAmount, decimal paramNonTaxableAmount, string paramRMDEligibledt)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RefundBeneficiaryDAClass.GetBeneficiaryRMDs(paramDeathBenefitOptionID, paramTaxableAmount, paramNonTaxableAmount, paramRMDEligibledt));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetPersonRMDEligibledate(string paramPersID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RefundBeneficiaryDAClass.GetPersonRMDEligibledate(paramPersID));
            }
            catch
            {
                throw;
            }
        }
     
        //End - SR:2014.05.07:YRS 5.0-2188: RMDs for Beneficiaries 

        #region InsertBeneficiaryRMDs
        //		'***************************************************************************************************//
        //		'Class Name                : RefundBeneficiaryDAClass                    Used In     : YMCAUI       //
        //		'Created By                : Sanjay Singh                                Modified On :				//
        //		'Modified By               :                                                                        //
        //		'Modify Reason             :                                                                        //
        //		'Constructor Description   :                                                                        //
        //		'Event Description         :This event will insert the Beneficiary RMD details in the atsBeneficiaryRMDs table   //
        //		'***************************************************************************************************//
        public static void SaveBeneficiaryRMDs(string paramDecsdPersId,string DeathBenefitOptionID, DataSet dsRMDs)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.RefundBeneficiaryDAClass.SaveBeneficiaryRMDs(paramDecsdPersId, DeathBenefitOptionID, dsRMDs);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        //START | SR | 2016.12.07 | YRS-AT-3222 | Calculate RMD for beneficiary of QD participant
        public static DataSet GetBeneficiaryRMDsForAltPayee(string deathBenefitOptionID, decimal taxableAmount, decimal nonTaxableAmount)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RefundBeneficiaryDAClass.GetBeneficiaryRMDsForAltPayee(deathBenefitOptionID, taxableAmount, nonTaxableAmount));
            }
            catch
            {
                throw;
            }
        }
        //END | SR | 2016.12.07 | YRS-AT-3222 | Calculate RMD for beneficiary of QD participant


        //START | SR | 2017.12.04 | YRS-AT-3756 | Validate participant is RMD eligible or not
        public static YMCAObjects.ReturnObject<bool> ValidateParticipantRMDEligibility(string deathBenefitOptionID, bool isHumanBeneficiary, decimal deathBenefitAmount, decimal lumpSumAmount, string deceasedFundStatus, DateTime persDeathdate)
        {
            DataSet ParticipantRMDEligible = null;
            bool isUnsatisfiedRMDExist = false;
            bool isNonHumanBeneficiaryExist = false;
            bool isRMDEligible = true;
            YMCAObjects.ReturnObject<bool> requestResult;
            requestResult = new YMCAObjects.ReturnObject<bool>();
            requestResult.Value = true;
            try
            {
                //Validations
                //1. If Beneficiary is non-human beneficiary then beneficiary is not rmd eligible.
                //2. If participant was retiree and having only death benefit amount remaining then do not allow rmd for beneficiary.
                //3. If participant having one of the beneficairy as non-human then do not calculate RMD for beneficiary.
                //4. If settlement year is same as death year & participant not having any unsatisfied amount exist then beneficiary is not rmd eligible.

                //1. If Beneficiary is non-human beneficiary then beneficiary is not rmd eligible.
                if (isHumanBeneficiary == false)
                {
                    requestResult.Value = false;                   
                }
                //2. If participant was retiree and having only death benefit amount remaining then do not allow rmd for beneficiary.
                if ((deceasedFundStatus == "DR" || deceasedFundStatus == "DD") && (lumpSumAmount == deathBenefitAmount))
                {
                    requestResult.Value = false;  
                }
                
                ParticipantRMDEligible = YMCARET.YmcaDataAccessObject.RefundBeneficiaryDAClass.ValidateParticipantRMDEligibility (deathBenefitOptionID);
                if (ParticipantRMDEligible !=null)
				{
                 if (ParticipantRMDEligible.Tables["ParticipantRMDEligibility"].Rows.Count > 0)
				    {
                            isUnsatisfiedRMDExist = Convert.ToBoolean(ParticipantRMDEligible.Tables["ParticipantRMDEligibility"].Rows[0]["IsUnsatisfiedRMDExists"]);
                            isNonHumanBeneficiaryExist = Convert.ToBoolean(ParticipantRMDEligible.Tables["ParticipantRMDEligibility"].Rows[0]["IsNonHumanBeneficiaryExist"]);
					}
                    //3. If participant having one of the beneficairy as non-human then do not calculate RMD for beneficiary.
                    //if (isNonHumanBeneficiaryExist)
                    //{                        
                    //    requestResult.MessageList.Add("Non-HumanBeneficiary");  
                    //}
                    //4. If settlement year is same as death year & participant not having any unsatisfied amount exist then beneficiary is not rmd eligible.
                    if (persDeathdate.Year == DateTime.Today.Year)
                    {
                        if  (!(isUnsatisfiedRMDExist))
                        {
                            requestResult.Value = false;  
                        }
                    }
                    else if (DateTime.Today.Year < persDeathdate.Year)
                    {
                        requestResult.Value = false;  
                    }
				}

                return requestResult;
            }
            catch
            {
                throw;
            }
        }
        //END | SR | 2017.12.04 | YRS-AT-3756 | Validate participant is RMD eligible or not
    }
}
