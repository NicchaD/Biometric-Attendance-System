/*
* Copyright YMCA Retirement Fund All Rights Reserved. 
 *
 * Project Name		:	YMCA-YRS
 * FileName			:	MetaMessages.cs 
 * Author Name		:	Shashank Patel
 * Description      :   This class is used to handle message object
 *                      for displaying message from database.
 * Employee ID		:	55381
 * Creation Date	:	08-July-2014
 *************************************************************
 * Modified By          Date         Descritption
 * ***********************************************************
 *Anudeep A             2015.08.12     YRS 5.0-2441-Modifications for 403b Loans
 * 
 * 
 * ***********************************************************
 * */

using System;

namespace YMCAObjects
{
    public class MetaMessage : ICloneable
     {
        public int MessageNo
        {
            get;
            set;
        }

        public string DisplayText
        {
            get;
            set;
        }

        public string MessageDescription
        {
            get;
            set;
        }

        public EnumMessageTypes MessageType
        {
            get;
            set;
        }

        public string ModuleName
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public DateTime CreatedOn
        {
            get;
            set;
        }
        public string DisplayMessageType
        {
            get {
                return this.MessageType.ToString();
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        //AA:2015.08.12 YRS 5.0-2441 Added Display code to show code in loan utility screen
        public string DisplayCode
        {
            get;
            set;
        }
     }
}
