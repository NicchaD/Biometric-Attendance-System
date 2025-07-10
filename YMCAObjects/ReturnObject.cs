// ******************************************************************
// Project ID: YMCA
// Author: Sanjay GS Rawat
// Created on: 11/15/2016
// Summary of Functionality: Data Object
// ***************************************
//
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name | Date | Version No | Project/IssueNo | Change
// ------------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------------
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace YMCAObjects
{
    //[DataContract, Serializable]
    public class ReturnObject<T>
    {
        /// <summary>
        /// Representing long value
        /// </summary>
        private T value;
        //[DataMember]
        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// List of messages
        /// </summary>
        private List<string> msgList = new List<string>();
        //[DataMember]
        public List<string> MessageList
        {
            get
            {
                return msgList;
            }
            set
            {
                msgList = new List<string>();
                this.msgList = value;
            }
        }
    }
}
