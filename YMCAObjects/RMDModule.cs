/**************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
// 	Chandra sekar		       | 2016.10.24   | 18.3.0		    | YRS-AT-3088 - YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)using System;
// ------------------------------------------------------------------------------------------------------
/**************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects
{
    class RMDModule
    {
    }


    public class RMDPrintLetters
    {
        public string strRefId { get; set; }
        public string strPersID { get; set; }
        public string strLetterCode { get; set; }
        public string strFundNo { get; set; }
        public string strSSNo { get; set; }
        public string strFirstName { get; set; }
        public string strLastName { get; set; }
        public string strMiddleName { get; set; }
        //START : CS | 2016.10.24 | YRS-AT-3088 |  Declared the variable to add columns in the (Intial/Cashout/Followup) Letter(s)
        public string Plantype { get; set; }
        public string Name { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal RmdAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Fundstatus { get; set; }
        public DateTime Duedate { get; set; }
        public int IsCashOutEligible { get; set; }
        //END : CS | 2016.10.24 | YRS-AT-3088 |  Declared the variable to add columns in the (Intial/Cashout/Followup) Letter(s)
    }
}
