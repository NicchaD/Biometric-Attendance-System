//**************************************************************************************************************/
// Author: Manthan Rajguru
// Created on: 09/13/2017
// Summary of Functionality: Class created to Get/Set YMCA credit details in property
// Declared in Version: 20.4.0 | YRS-AT-3665 -  YRS enh: Data Corrections Tool - Admin screen option to create a manual credit

//**************************************************************************************************************
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
//                             | 	          |		            | 
// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************

using System;

namespace YMCAObjects.DCTools
{
    [System.Serializable]
    public class YMCACredit
    {
        public string YMCAID { get; set; }

        public string YMCANo { get; set; }

        public string YMCAName { get; set; }

        public string CurrentCredit { get; set; }

        public string AddedCredit { get; set; }

        public string CreditSourceCode { get; set; }

        public DateTime ReceivedDate { get; set; }

        public DateTime ReceivedAccountingDate { get; set; }

        public string SystemNotes { get; set; }

        public string UserNotes { get; set; }
    }
}
