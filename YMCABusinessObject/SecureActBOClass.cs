//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	SecureActDAClass.cs
// Author Name		:	Megha Lad
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	02/10/2020
// Description	    :	Data Access Class For Secure Act 
// Declared in Version : 20.8.0| YRS-AT-4769 - YRS enhancement-new Secure Act rule for Annuity Estimate YRS changes (TrackIt-  41078)
//**************************************************************************************************************
// MODIFICATION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace YMCARET.YmcaBusinessObject
{
    public class SecureActBOClass
    {
        public SecureActBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

         /// <summary>
        /// Check Secure act New rule is applicable to not.
        /// </summary>
        /// <param name="data"></param>        
        /// <returns></returns>        
        public static Boolean IsSecureActApplicable(DateTime perssBirthDate, DateTime beneficiaryBirthDate, string beneficiaryRelationShipCode, DateTime cutOffDate, bool chronicallyIll)
        {
            try
            {
                return YmcaDataAccessObject.SecureActDAClass.IsSecureActApplicable(perssBirthDate, beneficiaryBirthDate, beneficiaryRelationShipCode, cutOffDate, chronicallyIll);
            }
            catch
            {
                throw; 
            }
        }

        /// <summary>
        /// Get beneficiary Details(Birth Date, Relationship code)
        /// </summary>
        /// <param name="beneficiaryId">Beneficiary ID</param>
        /// <returns>Beneficiary details in Table</returns>
        public static DataTable GetBeneficiaryDetails(string beneficiarySSNo)
        {
            try
            {
                return YmcaDataAccessObject.SecureActDAClass.GetBeneficiaryDetails(beneficiarySSNo);
            }
            catch
            {
                throw;
            }
        }
    }
}
