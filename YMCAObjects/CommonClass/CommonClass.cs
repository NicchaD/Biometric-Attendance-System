//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS Plan Split Project	
// FileName			:   CommonClass.cs
// Author Name		:
// Employee ID		:
// Email			:
// Contact No		:	
// Creation Time		:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	
// 
//*******************************************************************************
//	Date		Changed By			Description
//*******************************************************************************
// 2015.12.04   Chandrasekar.c      YRS-AT-2610: Calculating  Person age based on  As of Date
// 2015.12.15   Pramod P. Pokale    YRS-AT-2610: Resolved leap year age calculation 
// 2015.12.17   Sanjay Singh        YRS-AT-2252 - Annuity Estimate (Website/YRS) -needs to check the current Annual Limits table 
// 2016.10.24   Chandra sekar       YRS-AT-3088 - YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)
// 2016.11.21   Santosh Bura        YRS-AT-3203 -  YRS enh: RMD Utility distinguish cashout candidates PHASE 2 OF 2 (TrackIT 26224)
// 2017.04.27   Manthan Rajguru     YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977)    
// 2017.04.10   Santosh Bura        YRS-AT-3400 -  YRS enh: due MAY 2017 - RMD Print Letters- Letter to Non-respondents (new screen) 
//                                  YRS-AT-3401 -  RMD Print Letters- Satisfied but not elected (new screen)
// 2018.04.25   Sanjay GS Rawat	    YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
//*******************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;  // SR | 2015.12.17 | YRS-AT-2252
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling; 

namespace YMCAObjects
{
    public static class CommonClass
    {
        //START: SB | 2017.04.10 | YRS-AT-3400 & 3401 | Not used anywhere in the application as it is replaced by "RPTLetter" enum
        //public const string InitialLetter = "RMDINTLT";
        //public const string FollowUpLetter = "RMDFLWLT";
        //public const string CoverLetter = "RMDCOVER";
        //END: SB | 2017.04.10 | YRS-AT-3400 & 3401 | Not used anywhere in the application as it is replaced by "RPTLetter" enum

        //Start Added by Chandra sekar.c YRS-Ticket-2610 This Method is used to return Person Age with two argument one is date of Birthday and another date as of Date  
        public static int GetPersonAge(DateTime parameterDOB, DateTime parameterAsOnDate)
        {

            try
            {

                return Convert.ToInt32(Math.Floor(DateDiff("y", Convert.ToDateTime(parameterDOB), Convert.ToDateTime(parameterAsOnDate))));

            }
            catch
            {
                throw;
            }
        }
        //End Added by Chandra sekar.c YRS-Ticket-2610 This Method is used to return Person Age with two argument one is date of Birthday and another date as of Date  
        //Start Added by Chandra sekar.c YRS-Ticket-2610 This Method is used to calculate date difference base on interval of (day,time,year,week,monthly,bi-monthly,quarterly) and return age 
        public static double DateDiff(string Interval, DateTime EndDate, DateTime FromDate)
        {
            try
            {
                double dblDateDiff = 0;
                TimeSpan time = FromDate - EndDate;
                double timeHours = Math.Abs(time.Hours);
                double timeDays = Math.Abs(time.Days);

                switch (Interval.ToLower())
                {
                    case "h": // hours
                        dblDateDiff = timeHours;
                        break;
                    case "d": // days
                        dblDateDiff = timeDays;
                        break;
                    case "w": // weeks
                        dblDateDiff = timeDays / 7;
                        break;
                    case "bw": // bi-weekly
                        dblDateDiff = (timeDays / 7) / 2;
                        break;
                    case "m": // monthly
                        timeDays = timeDays - ((timeDays / 365) * 5);
                        dblDateDiff = timeDays / 30;
                        break;
                    case "bm": // bi-monthly
                        timeDays = timeDays - ((timeDays / 365) * 5);
                        dblDateDiff = (timeDays / 30) / 2;
                        break;
                    case "q": // quarterly
                        timeDays = timeDays - ((timeDays / 365) * 5);
                        dblDateDiff = (timeDays / 90);
                        break;
                    case "y": // yearly
                        // START: PPP | 2015.12.15 | YRS-AT-2610 | Resolved leap year age calculation 
                        //dblDateDiff = (timeDays / 365.2425);
                        dblDateDiff = new DateTime(time.Ticks).Year - 1;
                        // END: PPP | 2015.12.15 | YRS-AT-2610 | Resolved leap year age calculation 
                        break;
                }

                return dblDateDiff;
            }
            catch
            {
                throw;
            }
        }
        //End Added by Chandra sekar.c YRS-Ticket-2610  This Method is used to calculate date difference base on interval of (day,time,year,week,monthly,bi-monthly,quarterly) and return age 

        public enum RPTLetter
        {
            RMDINIT,
            RMDFOLLOW,
            RMDCOVER,
            RMDCSHLT, // CS | 2016.10.24 |  YRS-AT-3088 | To create  RMD Cashout Letter
            RMDCFLLT, // SB | 2016.11.21 |  YRS-AT-3203 | To create  RMD Follow-up Cashout Letter
            //START: MMR | 2017.04.24 | YRS-AT-3205 | Added letter code to create initial and follow-up letter for special QD participants
            RMDIAPLT, // For RMD special QD initial letter
            RMDFAPLT  // For RMD special QD Follow-up letter  
            //END: MMR | 2017.04.24 | YRS-AT-3205 | Added letter code to create initial and follow-up letter for RMD special QD participants
            ,RMDNRLET // SB | 2017.04.10 | YRS-AT-3400 | To create  RMD NonRespondent Annual Letter
            ,RMDERLET // SB | 2017.04.19 | YRS-AT-3401 | To create Letter for Satisfied RMD but Annual not Elected 
        }

        public enum BatchProcess
        {
            RMDBatchProcess,
            RMDInitLetters,
            RMDFollwLetters,
            CashOutBatch,
            RollInsLetters,
            //START: MMR | 2017.04.24 | YRS-AT-3205 | Added module type of intital and follow-up letter for RMD special QD participants
            RMDSpecialQDInitLetters,
            RMDSpecialQDFollowLetters
            //END: MMR | 2017.04.24 | YRS-AT-3205 | Added module type of intital and follow-up letter for RMD special QD participants
            ,RMDNonRespondentLetters // SB | 2017.04.10 | YRS-AT-3400 | To create  RMD NonRespondent Annual Letter
            ,RMDSatisfiedButNotElectedLetters // SB | 2017.04.17 | YRS-AT-3401 | To create  for Satisfied RMD but Annual not Elected
        }

        // START | SR | 2015.12.17 | YRS-AT-2252 - Below function can be used check empty dataset and datatable.
        public static bool isNonEmpty( DataSet ds)
        {
            if (ds == null)
                return false;
            if (ds.Tables.Count == 0)
                return false;
            if (ds.Tables[0].Rows.Count == 0)
                return false;
            return true;
        }

        public static bool isNonEmpty(DataTable dt)
        {
            if (dt == null)
                return false;
            if (dt.Rows.Count == 0)
                return false;
            return true;
        }
        // END | SR | 2015.12.17 | YRS-AT-2252 - Below function can be used check empty dataset and datatable.     

		// START : SR | 2018.04.25 | YRS-AT-3101 | Added new method to Log exceptions from BO class.
        public static void LogException(string message, Exception ex)
        {
            Exception exc;
            try
            {
                exc = new Exception(message,ex);
                ExceptionPolicy.HandleException(exc, "Exception Policy");
            }
            catch (Exception)
            {                
                throw;
            }
        }
		// END : SR | 2018.04.25 | YRS-AT-3101 | Added new method to Log exceptions from BO class.

        // START : SR | 2018.05.28 | YRS-AT-3101 | Creates copy of given object
        public static T DeepCopy<T>(T item)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }
        // END : SR | 2018.05.28 | YRS-AT-3101 | Creates copy of given object

    }
}
