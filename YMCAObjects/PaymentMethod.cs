//************************************************************************************************************************/
// Author:                   : Sanjay GS Rawat
// Created on:               : 04/11/2018
// Summary of Functionality  : Provides available payment method
// Declared in Version       : 20.6.0 | YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//
//************************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
// 			                  | 	         |		           | 
// ------------------------------------------------------------------------------------------------------
//************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace YMCAObjects
{
    /// <summary>
    /// Added structure type for payment method which can be used in required modules for checking payment method instead of hardcoded.
    /// </summary>
    public class PaymentMethod
    {
        public static string CHECK = "CHECK";
        public static string EFT = "EFT";
        public static string APPROVED_EFT = "APPROVED_EFT";
    }
}