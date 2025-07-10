//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	StateWithholdingBO.cs
// Author Name		:	Megha Lad
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	01/10/2019
// Description	    :	Business Class For State Withholding 
// Declared in Version : 20.7.0 | YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing
//**************************************************************************************************************
// MODIFICATION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
// Megha Lad                   | 11/19/2019   | 20.7.0          | YRS-AT-4719 - State Withholding - Additional text & warning messages for AL, CA and MA.
// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YMCAObjects;
using System.Web.SessionState;
namespace YMCARET.YmcaBusinessObject
{
    public class StateWithholdingBO 
    {

/// <summary>
///Provides Applicable Matrix StateName wise for State Withholding Input Controls
/// </summary>        
/// <returns>List of Applicable Matrix for State Withholding Input Controls</returns>
        public static List<StateWithholdingInput> GetStateWiseInputList()
        {
            List<StateWithholdingInput> lstDataList;
         
            try
            {
                lstDataList = YMCARET.YmcaDataAccessObject.StateWithholdingDAClass.GetStateWiseInputList();
                return lstDataList;
            }
            catch (Exception)
            {               
                throw;
            }
            finally
            {
                lstDataList = null;                
            }
        }

/// <summary>
/// This method performs Validation For State Tax Input Details.
/// </summary>
/// <param name="data"></param>
/// <param name="numAnnuityAmount"></param>
/// <param name="para_errorMessage"></param>
/// <returns></returns>

        public static Boolean ValidateStateTaxInputDetail(StateWithholdingDetails objSTWPersonDetail, double? numGrossAmount, double? numFederalAmount, string federalType, ref string para_errorMessage, ref string para_warningMessage)
       {
           
           string strNYMinimumThresholdAmt;
           string strNJMinimumThresholdAmt;
           string strALMinimumThresholdAmt;
           double? yonkersAmount;
           double? newyorkcityAmount;
           //Comman Validation
           if ((objSTWPersonDetail.chvDisbursementType == "REF") || (objSTWPersonDetail.chvDisbursementType == string.Empty))
           {
               para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_DISBURSEMENT).DisplayText;
               return false;
           }

           if (objSTWPersonDetail.bitStateTaxNotElected == false)
           {
               Dictionary<string, string> dic = new Dictionary<string, string>();
              
               
               if (objSTWPersonDetail.chvStateCode == "NY") // Validation For NY
               {
                   strNYMinimumThresholdAmt = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.GetConfigurationKeyValue("STW_NY_MINIMUM_THRESHOLD_AMOUNT").ToString().ToLower().Trim();
                   if ((objSTWPersonDetail.numFlatAmount < Convert.ToInt32(strNYMinimumThresholdAmt)) )
                   {
                       dic.Add("STW_MINIMUM_THRESHOLD_AMOUNT", strNYMinimumThresholdAmt);
                       dic.Add("STATE_NAME", "New York");
                       para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_FLATAMOUNTRANGE, dic).DisplayText;
                       return false;
                   }
                   if (numGrossAmount != null)
                   {
                       if (objSTWPersonDetail.numNewYorkCityAmount == null) newyorkcityAmount = 0; else newyorkcityAmount = objSTWPersonDetail.numNewYorkCityAmount;
                       if (objSTWPersonDetail.numYonkersAmount == null) yonkersAmount = 0; else yonkersAmount = objSTWPersonDetail.numYonkersAmount;

                       if ((objSTWPersonDetail.numFlatAmount + newyorkcityAmount + yonkersAmount) > numGrossAmount)
                       {
                           para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_STATEWITHHOLDINGAMOUNT).DisplayText;
                           return false;
                       }
                   }
               }
               
               if (objSTWPersonDetail.chvStateCode == "NJ")  // Validation For NJ
               {
                   strNJMinimumThresholdAmt = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.GetConfigurationKeyValue("STW_NJ_MINIMUM_THRESHOLD_AMOUNT").ToString().ToLower().Trim();
                   if ((objSTWPersonDetail.numFlatAmount < Convert.ToInt32(strNJMinimumThresholdAmt)))
                   {
                       dic.Add("STW_MINIMUM_THRESHOLD_AMOUNT", strNJMinimumThresholdAmt);
                       dic.Add("STATE_NAME", "New Jersey");
                       para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_FLATAMOUNTRANGE, dic).DisplayText;
                       return false;
                   }
               }
               
               if (objSTWPersonDetail.chvStateCode == "AL") // Validation For AL
               {
                   strALMinimumThresholdAmt = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.GetConfigurationKeyValue("STW_AL_MINIMUM_THRESHOLD_EXEMPTION").ToString().ToLower().Trim();
                   if ((objSTWPersonDetail.intNoOfExemption == Convert.ToInt32(strALMinimumThresholdAmt)) || (objSTWPersonDetail.intNoOfExemption > Convert.ToInt32(strALMinimumThresholdAmt)))
                   {
                       dic.Add("STW_MINIMUM_THRESHOLD_EXEMPTION", strALMinimumThresholdAmt);
                       para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_EXEMPTIONRANGE , dic).DisplayText;
                       return false;
                   }

                    if (String.IsNullOrEmpty(objSTWPersonDetail.chvMaritalStatusCode))
                   {
                       para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_MARITALSTATUS).DisplayText;
                       return false;
                   }
               }
               
               if (objSTWPersonDetail.chvStateCode == "MA")// Validation For MA               
               {                   
                   if (String.IsNullOrEmpty(objSTWPersonDetail.chvMaritalStatusCode))
                   {
                       para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_MARITALSTATUS).DisplayText;
                       return false;
                   }
               }
               
               if (objSTWPersonDetail.chvStateCode == "CA")// Validation For CA                             
               {
                   if ((objSTWPersonDetail.intNoOfExemption !=null ) && (objSTWPersonDetail.numAdditionalAmount !=null) && (String.IsNullOrEmpty(objSTWPersonDetail.chvMaritalStatusCode)))
                   {
                       para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_MARITALSTATUS).DisplayText;
                       return false;
                   }
                   //START: PK| 01/03/2020 | YRS-AT-4719 | Added condition to check flat amount should not be less than 0 when bitStateTaxNotElected is false.
                   if (objSTWPersonDetail.numFlatAmount<=0)
                   {
                       para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_FLATAMOUNT).DisplayText;
                       return false;
                   }
                   //END: PK| 01/03/2020 | YRS-AT-4719 | Added condition to check flat amount should not be less than 0 when bitStateTaxNotElected is false.
               }

               //Comman Validation
               if (objSTWPersonDetail.numFlatAmount != null)
               {
                   if ((objSTWPersonDetail.numFlatAmount < 0) || (objSTWPersonDetail.numFlatAmount > 10000))
                   {
                       dic.Add("AMOUNT_TYPE", "Flat Amount");
                       para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_WITHHOLDINGAMOUNTRANGE, dic).DisplayText;
                       return false;
                   }
                   if (!string.IsNullOrEmpty(numGrossAmount.ToString()) && ((objSTWPersonDetail.chvStateCode == "NJ") || (objSTWPersonDetail.chvStateCode == "CA")))
                   {
                       if (objSTWPersonDetail.numFlatAmount > numGrossAmount)
                       {
                           para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_STATEWITHHOLDINGAMOUNT).DisplayText;
                           return false;
                       }
                   }
               }

               if (objSTWPersonDetail.numAdditionalAmount != null)
               {

                   if ((objSTWPersonDetail.numAdditionalAmount < 0) || (objSTWPersonDetail.numAdditionalAmount > 10000))
                   {
                       dic.Add("AMOUNT_TYPE", "Additional Amount");
                       para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_WITHHOLDINGAMOUNTRANGE, dic).DisplayText;
                       return false;
                   }

                   if (numGrossAmount != null)
                   {
                       if (objSTWPersonDetail.numAdditionalAmount > numGrossAmount)
                       {
                           para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_STATEWITHHOLDINGAMOUNT).DisplayText;
                           return false;
                       }
                   }
               }
               //START: ML|11/19/2019 |YRS-AT-4719 | called validation function here
               if (!ValidateFedTaxVSStateTaxInputDetailForCA(objSTWPersonDetail, numFederalAmount, federalType, ref para_warningMessage))
                   return false;
               //END: ML|11/19/2019 |YRS-AT-4719 | called validation function here
           }
           return true;
       }

//START: ML|11/19/2019 |YRS-AT-4719 | Validation performed to check statetax detail against fedtax details.
 /// <summary>
/// This method performs Validation For StateTax Detail VS FedTax Detail
/// </summary>
/// <param name="data"></param>
/// <param name="numAnnuityAmount"></param>
/// <param name="para_errorMessage"></param>
/// <returns></returns>
        public static Boolean ValidateFedTaxVSStateTaxInputDetailForCA(StateWithholdingDetails objSTWPersonDetail, double? numFederalAmount, string federalType, ref string para_warningMessage)
        {
            if (objSTWPersonDetail.bitStateTaxNotElected == false)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if ((objSTWPersonDetail.chvStateCode == "CA") && (numFederalAmount != null) && (!string.IsNullOrEmpty(federalType)))//  If Federal is definded CA validation will work                                                       
                   {
                     //Participant cannot apply for State Tax ,If Participant had not apply for Federal Tax or Federal Tax is 0(State CA)
                       string withholdPercent = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.GetConfigurationKeyValue("STW_CA_PERCENTOFFEDERAL_AMOUNT").ToString().ToLower().Trim();
                       if ((objSTWPersonDetail.numPercentageofFederalWithholding !=null) && (numFederalAmount <=0) && (federalType.ToUpper() == "FLAT"))
                       {
                           dic.Add("withholdPercent", withholdPercent);
                           para_warningMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_INVALID_FEDERALAMOUNT_CA, dic).DisplayText;
                           return false;
                       }
                    }
            }
            return true;        
        }

        public static Boolean ValidateFedTaxVSStateTaxInputDetailForMA(StateWithholdingDetails objSTWPersonDetail, double? numFederalAmount,string federalType, ref string para_errorMessage)
        {
           
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if ((objSTWPersonDetail.chvStateCode == "MA") && (!string.IsNullOrEmpty(federalType)) &&  (numFederalAmount != null))// If Federal is definded Ma validation will work                           
                {
                    //START : Participant should not allowed to save, if federal tax is any amount apart from flat "0" and bitStateTaxNotElected is true.
                    if ((objSTWPersonDetail.bitStateTaxNotElected == true)  && (numFederalAmount > 0) && (federalType.ToUpper() == "FLAT"))
                    {
                        para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_REQUIRED_WITHHOLDING_MA).DisplayText;
                        return false;
                    }

                    if ((objSTWPersonDetail.bitStateTaxNotElected == true) &&  (numFederalAmount >= 0) && ((federalType.ToUpper() == "FORMUL") || (federalType.ToUpper() == "DEFALT")))
                    {
                        para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_REQUIRED_WITHHOLDING_MA).DisplayText;
                        return false;
                    }
                    //END : Participant should not allowed to save, if federal tax is any amount apart from flat "0" and bitStateTaxNotElected is true.

                    //Participant should not allowed to save, if federal tax is flat "0" and bitStateTaxNotElected is false.
                    if ((objSTWPersonDetail.bitStateTaxNotElected == false)  && (numFederalAmount <= 0) && (federalType.ToUpper() == "FLAT"))
                    {
                        para_errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_STW_REQUIRED_FEDERALAMOUNT_MA).DisplayText;
                        return false;
                    }
                }
            
            return true;
        }
//END: ML|11/19/2019 |YRS-AT-4719 | Validation performed to check statetax detail against fedtax details.


/// <summary>
/// This method save tax details in database using data objects.
/// </summary>
/// <param name="data"></param>
/// <returns></returns>
       public static Boolean SavePersStateTaxdetails(YMCAObjects.StateWithholdingDetails objSTWPersonDetail)
        {          
            try
            {                
                return YMCARET.YmcaDataAccessObject.StateWithholdingDAClass.SavePersStateTaxdetails(objSTWPersonDetail);
            }
            catch (Exception)
            {
                throw;
            }
        }

/// <summary>
/// To get details of state tax detail of person to display in gridview.
/// </summary>
/// <param name="perssid"></param>
/// <returns></returns>
        public static List<StateWithholdingDetails> GetStateTaxDetails(string perssid)
        {
            List<StateWithholdingDetails> lsDataList;

            try
            {
                lsDataList = YMCARET.YmcaDataAccessObject.StateWithholdingDAClass.GetStateTaxDetails(perssid);
                return lsDataList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lsDataList = null;

            }

        }
        /// <summary>
        /// To get Person details to show in UI screen
        /// </summary>
        /// <param name="perssid"></param>
        /// <returns></returns>
        public static StateWithholdingPersonDetails GetPersonDetails(string perssid)
        {
            StateWithholdingPersonDetails objSTWPersonDetail;

            try
            {
                objSTWPersonDetail = YMCARET.YmcaDataAccessObject.StateWithholdingDAClass.GetPersonDetails(perssid);
                return objSTWPersonDetail;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objSTWPersonDetail = null;
            }
        }
    }
}
