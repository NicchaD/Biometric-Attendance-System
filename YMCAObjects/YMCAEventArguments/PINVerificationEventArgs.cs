//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Anudeep A             06.28.2016  YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
//*****************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects
{
    public class PINVerificationEventArgs : YMCAEventArgs
    {
        public string PinNumber { get;set; }
        //Start:AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
        //public Guid guiPersId { get;set; }
        public string guiPersId { get; set; }
        //End :AA:04.28.2016 YRS-AT-2830 Changed As the entityid can be integer or uniqueid
        public PINVerificationActionTypes PINActionType { get; set; }
        public PINVerificationModule PINModule { get; set; }
        public override string ToString()
        {
            return string.Format("PinNumber = '{0}' and Action = '{1}'", PinNumber, PINActionType);
        
        }

    }
}
