//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA YRS ODC
// FileName			:	RetirementBatchEstimateBOClass.cs
// Author Name		:	Shashank Patel
// Employee ID		:	55381
// Email			:	shashank.patel@3i-infotech.com
// Contact No		:	8618
// Creation Time	:	22-Nov-2010
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					: YRS 5.0-1365 : Need new batch processing option	
//************************************************************************************
//Modficiation History
//************************************************************************************
//Date			Modified By			Description
//************************************************************************************
//2012.02.29    Shashank Patel      Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant
//2012.07.23	Shashank Patel		Bt Id:1007 Batch Estimate observation.
//2014.10.10    Anudeep  Adusumilli BT:2357 YRS 5.0-2285 - Need ability to exclude pre-eligible participants 
//2015.09.16    Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//****************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YMCARET.YmcaBusinessObject
{
    public class RetirementBatchEstimateBOClass
    {
        #region Constructor
        public RetirementBatchEstimateBOClass()
        { }  
        #endregion  

        public static DataSet SearchPersonInfo(string tcrFundNo, string tcFirstName, string tcLastName, string tcSSNo, string tcYmcaName)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementBatchEstimateDAClass.GetEstimatePersonInfo(tcrFundNo, tcFirstName, tcLastName, tcSSNo, tcYmcaName));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet SearchPersonInfo(string tcrFundNo, string tcFirstName, string tcLastName, string tcSSNo, string tcYmcaName, string tcYmcaNo, string tcExcludeFundEvents)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementBatchEstimateDAClass.GetEstimatePersonInfo(tcrFundNo, tcFirstName, tcLastName, tcSSNo, tcYmcaName, tcYmcaNo, tcExcludeFundEvents)); //AA:10.10.2014 BT:2357-YRS 5.0-2285 - Added to exclude the ra , pe fund events
            }
            catch
            {
                throw;
            }
        }
        //2012.02.29   Shashank Patel  :     Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant (remove parameter xmlfundeeventsids)
        //public static int SaveBatchEstimateParamater(string tdretireAge, string tdRetireDeathBenificary, string tcRetireSal, string tcRetirePlanType, string tcRetireInterest, string tdFutureSalEffectiveDate, int userID)
        //{
        //    try
        //    {
        //        return (YMCARET.YmcaDataAccessObject.RetirementBatchEstimateDAClass.InsertRetirementBatchEstimateParamater(tdretireAge, tdRetireDeathBenificary, tcRetireSal, tcRetirePlanType, tcRetireInterest, tdFutureSalEffectiveDate, userID));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //2012.02.29   Shashank Patel  :     Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant (remove parameter xmlfundeeventsids)
        public static int SaveBatchEstimate(string tdBatchEstimateDetailXml,int tdUSerID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementBatchEstimateDAClass.InsertRetirementBatchEstimate(tdBatchEstimateDetailXml, tdUSerID));
            }
            catch
            {
                throw;
            }
        }
        //2012.02.29   Shashank Patel  :     Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant (remove parameter xmlfundeeventsids)
        //public static DataSet SearchBatchEstimateParameter(int tdUserID)
        //{ 
        //    try
        //    {
        //        return (YMCARET.YmcaDataAccessObject.RetirementBatchEstimateDAClass.GetEstimateParameterByUserID(tdUserID));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //2012.02.29   Shashank Patel  :     Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant (remove parameter xmlfundeeventsids)
        public static DataSet SearchBatchEstimatePerson(int tdUserID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementBatchEstimateDAClass.GetBatchEstimateByUserID(tdUserID));
            }
            catch
            {
                throw;
            }
        }
        //2012.02.29   Shashank Patel  :     Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant (remove parameter xmlfundeeventsids)
        public static int DeleteBatchEstimateByUserID(int tdUserID)
        {
            try
            {//2012.02.29   Shashank Patel  :     Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant (remove parameter xmlfundeeventsids)
                return (YMCARET.YmcaDataAccessObject.RetirementBatchEstimateDAClass.DeleteRetirementBatchEstimateByUserID(tdUserID));
            }
            catch
            {
                throw;
            }
        }

        //SP -2012.07.23 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
        public static int GetRetirementBatchListCount(int iUserId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RetirementBatchEstimateDAClass.GetRetirementBatchListCount(iUserId);
            }
            catch
            {
                throw;
            }
        }
		//SP -2012.07.23 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END

    }
}
