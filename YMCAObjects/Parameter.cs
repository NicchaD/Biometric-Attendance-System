/**************************************************************************************************************/
// Author: Vinayan C.
// Created on: 10/08/2018
// Summary of Functionality: Helps to build key value pair, which can easily be converted to XML. 
// Declared in Version: 20.6.0 | YRS-AT-4018 - YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab 
//
/**************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name               | Date          | Version No        | Ticket
// ------------------------------------------------------------------------------------------------------
// 			                    | 	            |		            | 
// ------------------------------------------------------------------------------------------------------
/**************************************************************************************************************/

using System.Xml;
using System.Xml.Serialization;

namespace YMCAObjects
{
    [XmlType("PAIR")]
    public class Parameter
    {
        [XmlElement("KEY")]
        public string Key { get; set; }

        [XmlElement("VALUE")]
        public string Value { get; set; }

    }
}
