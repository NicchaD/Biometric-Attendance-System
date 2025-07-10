//********************************************************************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	MRDBO.cs
// Author Name		:	
// Employee ID		:	
// Email			:	
// Contact No		:	
// Description		:	Business class for MRD form
//********************************************************************************************************************************
//Modification History
//********************************************************************************************************************************
//Modified By			Date				Description
//********************************************************************************************************************************
//Sanjay R.             2014.01.29          MRD Enhancement
//Anudeep A.            2014.05.07          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages
//Dinesh k              2014.06.10          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages
//Sanjay R              2014.07.18          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Anudeep Adusumilli    2015.10.21          YRS-AT-2614 - YRS: files for IDM - .idx filename needs to match .pdf filename
//Sanjay GS Rawat       2016.04.05          YRS-AT-2843 - YRS enh-RMD Utility should use J&S table if primary bene is spouse more than 10 yrs younger (TrackIT 25233)
//Chandra sekar         2016.10.17          YRS-AT-2476 - Change to RMD utility - generate one release blank instead of two if same participant (TrackIT 22029)
//Santosh Bura			2016.10.24 			YRS-AT-2685 -  YRS enh-save RMD Print Letters batches and add wording near "close" button (TrackIT 24380) 
//Manthan Rajguru       2016.10.27          YRS-AT-2922 -  YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)
//Chandra sekar         2016.11.01          YRS-AT-2922 - YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)
//Santosh Bura			2016.12.05 			YRS-AT-3203 -  YRS enh: RMD Utility distinguish cashout candidates PHASE 2 OF 2 (TrackIT 26224) 
//Manthan Rajguru       2017.04.27          YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977)    
//Santosh Bura			2017.04.11 			YRS-AT-3400 -  YRS enh: due MAY 2017 - RMD Print Letters- Letter to Non-respondents (new screen). (2 of 3 tickets) (TrackIT 29186)
//Santosh Bura			2017.04.25 			YRS-AT-3401 -  YRS REPORTS enh: new Report to track 403(b) TD Savings (Smart Accounts) (TrackIT 29031)YRS enh: due MAY 2017 - RMD Print Letters- Satisfied but not elected (new screen). (3 of 3 tickets) (TrackIT 29186&#
//Santosh Bura          2017.07.28          YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
//Santosh Bura          2018.01.11          YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
//Pramod P. Pokale      2018.10.03          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
//********************************************************************************************************************************

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
using System.Collections;

namespace YMCARET.YmcaBusinessObject
{
    public class MRDBO
    {
        //AA:05.07.2014 BT:2434:YRS 5.0-2315 - Added parameter to get the records as per the year         
        public static DataSet GetMRDRecords(int intRMDYear, int month, string fundNo) //MMR | 2016.10.27 | YRS-AT-2922 | Passed parameters to get records by RMD due date and fund no alongwith RMD Year
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetMRDRecords(intRMDYear, month, fundNo)); //MMR | 2016.10.27 | YRS-AT-2922 | Passed parameters to get records by RMD due date and fund no alongwith RMD Year
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetBatchMRDRecords()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetBatchMRDRecords());
            }
            catch
            {
                throw;
            }
        }


        public static DataSet GetRMDRecordsForBatchProcess()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRMDRecordsForBatchProcess());
            }
            catch
            {
                throw;
            }
        }

        public static int SaveCurrentMRD(DateTime ProcessDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.SaveCurrentMRD(ProcessDate));
            }
            catch
            {
                throw;
            }
        }

        public static string GenerateMRDRecords(DateTime dtGenerateMRD)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GenerateMRDRecords(dtGenerateMRD));
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetGeneratedRMDYears()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetGeneratedRMDYears());
            }
            catch
            {
                throw;
            }
        }
        public static Boolean IsAllowedToGenerateRMDForCurrentYear()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.IsAllowedToGenerateRMDForCurrentYear());
            }
            catch
            {
                throw;
            }
        }
        public static string GetLastRMDProcessedDate()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetLastRMDProcessedDate());
            }
            catch
            {
                throw;
            }
        }

        #region Added By Sanjeev Gupta 14th Oct 2011 BT-925 Regenerate RMD
        public static DataSet GetGeneratedMRDRecords(string parameterFundID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetGeneratedMRDRecords(parameterFundID));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetRegeneratedRMDRecords(int iProcessYear, string stFundID, out string stMessage)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRegeneratedRMDRecords(iProcessYear: iProcessYear, stFundID: stFundID, stMessage: out stMessage));
            }
            catch
            {
                throw;
            }
        }

        public static int SaveRegeneratedMRD(string stXMLRegenMRD, string stFundID, out string stMessage)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.SaveRegeneratedMRD(stXMLRegenMRD: stXMLRegenMRD, stFundID: stFundID, stMessage: out stMessage));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetDisbursementDetails(string stFundEventID, int iYear, string stPlanType)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetDisbursementDetails(stFundEventID, iYear, stPlanType));
            }
            catch
            {
                throw;
            }
        }
        #endregion




        #region Added By Sanjeev Gupta on 17th Feb 2012 for BT-1000
        public static void GetRegenYearByTermDate(string stFundID, out Int32 iStartYear)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.MRDDAClass.GetRegenYearByTermDate(stFundID, out iStartYear);
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region "Added by Dinesh Kanojia on 17th Oct 2013 BT:- 2139 :- YRS 5.0-2165:RMD enhancements "

        public static DataSet GetPersonMetaConfigurationDetails(string strGuiPerssID, string strConfigCode)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetPersonMetaConfigurationDetails(strGuiPerssID, strConfigCode));
            }
            catch
            {
                throw;
            }
        }

        //GetMRDBatchProcessRecords

        public static DataSet GetMRDBatchProcessRecords()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetMRDBatchProcessRecords());
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetBatchRMDForInitialLetter()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetBatchRMDForInitialLetter());
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetBatchRMDForPrintReport(string strProcessDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetBatchRMDForPrintReport(strProcessDate));
            }
            catch
            {
                throw;
            }
        }

        //Start-SR:2014.01.29:BT2139: YRS 5.0-2165:RMD enhancements 
        public static string SaveRMDInitialLetterData(string paramStrPersId)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.SaveRMDInitialLetterData(paramStrPersId));
            }
            catch
            {
                throw;
            }
        }
        //End-SR:2014.01.29:BT2139: YRS 5.0-2165:RMD enhancements 

        #endregion

        //public static DataSet GetGeneratedRMDYears()
        //{
        //    try
        //    {
        //        return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetGeneratedRMDYears());
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public static Boolean IsAllowToGenerateRMDForCurrentYear()
        //{
        //    try
        //    {
        //        return (YMCARET.YmcaDataAccessObject.MRDDAClass.IsAllowToGenerateRMDForCurrentYear());
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        #region "Added by Dinesh Kanojia on 17th May 2014  BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages"

        public static DataSet GetBatchNonEligibleMRDRecords(string[] strParam = null)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetBatchNonEligibleMRDRecords(strParam));
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetBatchRMDForFollowupLetter()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetBatchRMDForFollowupLetter());
            }
            catch
            {
                throw;
            }
        }


        public static DataSet GetStagingDetails(string strbatchId, string strProcessName)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetStagingDetails(strbatchId, strProcessName));
            }
            catch
            {
                throw;
            }
        }


        public static string InsertStagingLogs(string strBatchId, string strFundEventId, string strRefId, string strLinkingId, string strFundno, string strProcessName)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.InsertStagingLogs(strBatchId, strFundEventId, strRefId, strLinkingId, strFundno, strProcessName));
            }
            catch
            {
                throw;
            }
        }


        public static DataSet GetNextBatchId(DateTime dtRMDProcessDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetNextBatchId(dtRMDProcessDate));
            }
            catch
            {
                throw;
            }
        }

        //START: MMR | 2017.05.05 | YRS-AT-3205 | Changed return type of method from string to int, now returning new rows AtsPrintLetters.intUniqueId
        //public static string InsertPrintLetters(string strRefId, string strPersId, string strLettersCode, string strChvNotes)     // SB | 2016.12.05 | YRS-AT-3203 | Added new parameter to describe plan type in print letter
        public static int InsertPrintLetters(string strRefId, string strPersId, string strLettersCode, string strChvNotes)     // SB | 2016.12.05 | YRS-AT-3203 | Added new parameter to describe plan type in print letter
        //END: MMR | 2017.05.05 | YRS-AT-3205 | Changed return type of method from string to intt, now returning new rows AtsPrintLetters.intUniqueId
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.InsertPrintLetters(strRefId, strPersId, strLettersCode, strChvNotes));     // SB | 2016.12.05 | YRS-AT-3203 | Added new parameter to describe plan type in print letter
            }
            catch
            {
                throw;
            }

        }


        public static DataSet GetCloseRMDDetails()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetCloseRMDDetails());
            }
            catch
            {
                throw;
            }

        }

        //AA: 2015.10.21 YRS-AT-2614 - Added optional parameter access the YRS_IDM dataconfig which will use sql authonication for FT folder files as these files will take machine id for the windows authentication
        public static DataSet GetAtsTemp(string strBatchId, string strModule , bool blnDBForFT = false)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetAtsTemp(strBatchId, strModule, blnDBForFT));//AA: 2015.10.21 YRS-AT-2614 - Added optional parameter to access the YRS_IDM dataconfig file for FT folder
            }
            catch
            {
                throw;
            }

        }
        //START: CS | 2016.10.17 |  YRS-AT-2476 | Change to RMD utility - generate one release blank instead of two if same participant (TrackIT 22029)
        public static DataSet GetMrdRecordPlanWise(string batchID, string moduleName, bool isDBForFT = false)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetMrdRecordPlanWise(batchID, moduleName, isDBForFT));
            }
            catch
            {
                throw;
            }
        }
        //END: CS | 2016.10.17 |  YRS-AT-2476 | Change to RMD utility - generate one release blank instead of two if same participant (TrackIT 22029)

        public static DataSet GetAllTempBatchID()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetAllTempBatchID());
            }
            catch
            {
                throw;
            }

        }

        //AA: 2015.10.21 YRS-AT-2614 - Added optional parameter to access the YRS_IDM dataconfig file for FT folder
        public static string InsertAtsTemp(string strBatchId, string strModule, DataSet strData, bool blnDBForFT = false)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.InsertAtsTemp(strBatchId, strModule, strData, blnDBForFT));//AA: 2015.10.21 YRS-AT-2614 - Added optional parameter to access the YRS_IDM dataconfig file for FT folder
            }
            catch
            {
                throw;
            }

        }

        //START: PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | Creating overload method of InsertAtsTemp by changing type of strData from DataSet to string
        //                                                 It will give flexibility of saving XML prepared by individual modules directly into system
        public static string InsertAtsTemp(string strBatchId, string strModule, string strData, bool blnDBForFT = false)
        {
            return (YMCARET.YmcaDataAccessObject.MRDDAClass.InsertAtsTemp(strBatchId, strModule, strData, blnDBForFT));
        }
        //END: PPP | 10/03/2018 | 20.6.0 | YRS-AT-4017 | Creating overload method of InsertAtsTemp by changing type of strData from DataSet to string
        #endregion

        //Start-SR:2014.07.18 - BT:2434:YRS 5.0-2315-Added procedure to retieve RMD Process log
        public static DataSet GetRMDProcessLog()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRMDProcessLog());
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetRMDStatistics()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRMDStatistics());
            }
            catch
            {
                throw;
            }
        }
        //End-SR:2014.07.18 - BT:2434:YRS 5.0-2315-Added procedure to retieve RMD Process log
        // START | SR | 2016.04.05 | YRS-AT-2843 - YRS enh-RMD Utility should use J&S table if primary bene is spouse more than 10 yrs younger (TrackIT 25233)
        public static string UpdateParticipantRMDforSolePrimaryBeneficiary(int paramMRDUniqueId, decimal paramRMDTaxableAmount, decimal paramRMDNonTaxableAmount)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.MRDDAClass.UpdateParticipantRMDforSolePrimaryBeneficiary(paramMRDUniqueId, paramRMDTaxableAmount, paramRMDNonTaxableAmount);
            }
            catch
            {
                throw;
            }
        }
        // END | SR | 2016.04.05 | YRS-AT-2843 - YRS enh-RMD Utility should use J&S table if primary bene is spouse more than 10 yrs younger (TrackIT 25233)

        // START | SB | 2016.10.24 | YRS-AT- 2685 - Get the whole batchid's for RMD reprint letters and details for selected batchid
        public static DataSet GetRMDLetterBatchIDList(string ModuleName )
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRMDLetterBatchIDList(ModuleName));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetRMDLetterDetailsByBatchId(string BatchId, string ModuleName)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRMDLetterDetailsByBatchId(BatchId, ModuleName));
            }
            catch
            {
                throw;
            }

        }
        // END | SB | 2016.10.24 | YRS-AT- 2685 - Get the whole batchid's for RMD reprint letters and details for selected batchid

        //START: MMR | 2016.10.27 |  YRS-AT-2922 | Added to fetch RMD process status
        public static DataSet GetMrdRecordProcessStatus(string batchID, int year, string dueDate, Boolean isMarchClosed, Boolean isDecemberClosed)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetMrdRecordProcessStatus(batchID, year, dueDate, isMarchClosed, isDecemberClosed));
            }
            catch
            {
                throw;
            }
        }
        //END: MMR | 2016.10.27 |  YRS-AT-2922 | Added to fetch RMD process status

        //START: CS | 2016.11.01 |  YRS-AT-2922 | YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)
        public static void InsertNotProcessedAndNonEligibleParticipants(DateTime processDate)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.MRDDAClass.InsertNotProcessedAndNonEligibleParticipants(processDate);
            }
            catch
            {
                throw;
            }
        }
        //END: CS | 2016.11.01 |  YRS-AT-2922 | YRS enh- RMD Utility- separate March and Dec, text changes, filter Fund ID, save details (TrackIT 25626)

        //START: MMR | 2017.04.24 | YRS-AT-3205 | Get special QD participants with first RMD due in December for initial and follow-up letters
        # region "RMD Special QDRO"
        public static DataSet GetRMDSpecialQDInitialLetterParticipants()
        {
            return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRMDSpecialQDInitialLetterParticipants());
        }

        public static DataSet GetRMDSpecialQDFollowupLetterParticipants()
        {
            return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRMDSpecialQDFollowupLetterParticipants());
        }

        public static DataSet GetIDMDetailsForReprinting(string batchID)
        {
            return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetIDMDetailsForReprinting(batchID));
        }
        # endregion
        //END: MMR | 2017.04.24 | YRS-AT-3205 | Get special QD participants with first RMD due in December for initial and follow-up letters

        //START: SB | 2017.04.11 | YRS-AT-3400 & 3401 
        #region "ReminderLetter"
        //To get the RMD Non-Respondends Annnual Letter Participants 
        public static DataSet GetRMDReminderLettersForNonRespondent()
        {
            return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRMDReminderLettersForNonRespondent());
        }

        //To get the RMD Satisfied But Annual Not Elected Letter Participants 
        public static DataSet GetRMDReminderLettersForAnnualNotElected()
        {
            return (YMCARET.YmcaDataAccessObject.MRDDAClass.GetRMDReminderLettersForAnnualNotElected());
        }
        #endregion
        //END: SB | 2017.04.11 | YRS-AT-3400 & 3401

        //START: SB | 2017.07.26 | YRS-AT-3324 - Function to check if RMD need's to be generate/regenerate when processing withdrawal, death notification and death settlement
        #region "WithDrawal / Death Notification / Death Settlement Restrictions"
        //START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | Added additional parameter to function for checking deceased participants mrd records uptill deceased year
        //public static DataSet IsRMDGenerationRequired(string modulename, string fundeventid)
        public static DataSet IsRMDGenerationRequired(string moduleName, string fundEventID, string deceasedDate)
        //END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | added additional parameter to function for checking deceased participants mrd records uptill deceased year
        {
            try
            {
                //START: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | Added additional parameter to function for checking deceased participants mrd records uptill deceased year
                //return (YMCARET.YmcaDataAccessObject.MRDDAClass.IsRMDGenerationRequired(moduleName, fundEventID));
                return (YMCARET.YmcaDataAccessObject.MRDDAClass.IsRMDGenerationRequired(moduleName, fundEventID, deceasedDate));
                //END: SB | 2018.01.03 | YRS-AT-3324 | 20.4.3 | Added additional parameter to function for checking deceased participants mrd records uptill deceased year
            }
            catch
            {
                throw;
            }
        }
        #endregion
        //END: SB | 2017.07.26 | YRS-AT-3324 - Function to check if RMD need's to be generate/regenerate when processing withdrawal, death notification and death settlement
    }
}
