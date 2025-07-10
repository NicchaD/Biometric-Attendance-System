//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	VoidandTransferAnnuityBOClass.cs
// Author Name		:	Sanjay Rawat
// Employee ID		:	51193
// Email	    	:	sanjay.singh@3i-infotech.com
// Contact No		:	8637
// Creation Time	:	16/09/2013
// Description	    :	Business Class for Void and Transfer Annuity
//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
    public class VoidAndTransferAnnuityBOClass
    {

        public VoidAndTransferAnnuityBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}


        //		'***************************************************************************************************//
        //		'Event Description         :This event will return deceased retiree(s) based on selected criteria   //
        //		'***************************************************************************************************//
       public static DataSet LookUpPerson(string paramStrSSNo, string paramStrFundNo, string paramStrLastName, string paramStrFirstName, string paramStrFormName)       
        {
            try
            {
                return (VoidAndTransferAnnuityDAClass.LookUpPerson(paramStrSSNo, paramStrFundNo, paramStrLastName, paramStrFirstName, paramStrFormName));
            }
            catch
            {
                throw;
            }
        }


        //		'***************************************************************************************************//
        //		'Event Description         :This event will check for existing SSN in the atsperss table   //
        //		'***************************************************************************************************//
        public static int CheckForExistingSSN(string paramStrSSNo)
        {
            try
            {
               return  VoidAndTransferAnnuityDAClass.CheckForExistingSSN(paramStrSSNo);
            }
            catch
            {
                throw;
            }
        
        }

       
        //		'***************************************************************************************************//
        //		'Event Description         : This event will return unvoided and unpaid Disbursements               //
        //		'***************************************************************************************************//
        public static DataSet GetDisbursementsByPersId(string paramStrPersId)
        {
            try
            {
                return VoidAndTransferAnnuityDAClass.GetDisbursementsByPersId(paramStrPersId);
            }
            catch
            {
                throw;
            }        
        
        }



        //		'*******************************************************************************************************************//
        //		'Event Description         :This event will save and Transfer annuity to new/existing Payee in the atsperss table   //
        //		'*******************************************************************************************************************//
        public static string SaveandTransferAnnuity(string paramStrPersId, string paramStrSSNo, string paramStrLastName, string paramStrFirstName, string paramStrMiddleName, string paramStrSalutation, string paramStrSuffix, string paramStrBirthdate, string paramStrMaritalCode, List<Dictionary<string, string>> paramlstDisbursementIDs, string ParamStrAddrsId, Boolean paramBlnPayeeExist, string paramStrEmailId, string paramStrPhoneNo)
        {
            return VoidAndTransferAnnuityDAClass.SaveandTransferAnnuity(paramStrPersId, paramStrSSNo, paramStrLastName, paramStrFirstName, paramStrMiddleName, paramStrSalutation, paramStrSuffix, paramStrBirthdate, paramStrMaritalCode, paramlstDisbursementIDs, ParamStrAddrsId, paramBlnPayeeExist,paramStrEmailId, paramStrPhoneNo);
        }



    }
}
