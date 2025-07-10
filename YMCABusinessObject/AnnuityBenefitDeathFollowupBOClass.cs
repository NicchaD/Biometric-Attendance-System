/*************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	AnnuityBenefitDeathFollowupBOClass
' Author Name		:	Jagadeesh Bollabathini
' Employee ID		:	58657 .... 
' Email				:	jagadeesh.bollabathini@3i-infotech.com
' Contact No		:	
' Creation Time		:	22/04/2015 
' Unit Test Plan Name			:	
' Description					:	This form is used to Annuity Beneficiary Death Follow-up
'****************************************************************************************/
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaBusinessObject;
using YMCARET.YmcaDataAccessObject;
using System.Collections.Generic;

namespace YMCARET.YmcaBusinessObject
{
    public class AnnuityBenefitDeathFollowupBOClass
    {
        
        public static DataSet GetPendingFollowupDays()
        {
            try
            {
                return AnnuityBenefitDeathFollowupDAClass.GetABDFLPendingList();
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateJointSurvivorDeathDocReceived(DataSet paramDSFollowup)
        {
            try
            {
                AnnuityBenefitDeathFollowupDAClass.UpdateJointSurvivorDeathDocReceived(paramDSFollowup);
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetAnnBenFromConfiguration(string ParameterConfigCategaryCode)
        {
            try
            {
                return AnnuityBenefitDeathFollowupDAClass.GetAnnBenFromConfiguration(ParameterConfigCategaryCode);
            }
            catch
            {
                throw;
            }
        }

    }
}
