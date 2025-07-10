//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Anudeep A             06.28.2016  YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
//Manthan R             09.18.2017  YRS-AT-3665 - YRS enh: Data Corrections Tool - Admin screen option to create a manual credit 
//*****************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
namespace YMCAObjects
{
    public class YMCAActionEntry
    {
        public string Action { get; set; }
        public string ActionBy { get; set; }
        public bool SuccessStatus { get; set; }
        //Start:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
        //public Guid EntityId { get; set; }
        //End:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
        public string EntityId { get; set; }
        public EntityTypes EntityType { get; set; } 
        public string Module { get; set; }
        public string Data { get; set; }
    }

    public enum EntityTypes{
        YMCA,
        PERSON,
        BENEFICIARY,
        LOAN //AA:04.28.2016 YRS-AT-2830 Added a new entity type which helps to store the entity type in the activity log
    }
    public enum PINVerificationActionTypes
    {
        YES,
        NO,
        CANCEL
    }
    public enum PINVerificationModule
    {
        MAINTENANCEPERSON,
        WITHDRAWAL,
        RETIREMENTESTIMATE
        
    }

    //START: MMR | 09/18/2017 | YRS-AT-3665 | Added new entity type for data correction tools which helps to store the entity type in the activity log
    public struct ActionYRSActivityLog
    {
        public static readonly string ADD_YMCA_CREDITS = "ADD_YMCA_CREDITS";
        public static readonly string EDIT_REMAINING_DEATH_BENEFIT = "EDIT_REMAINING_DEATH_BENEFIT";
        public static readonly string REVERSE_ANNUITY = "REVERSE_ANNUITY";
        public static readonly string CHANGE_FUND_STATUS = "CHANGE_FUND_STATUS";
    }
    //END: MMR | 09/18/2017 | YRS-AT-3665 | Added new entity type for data correction tools which helps to store the entity type in the activity log
}