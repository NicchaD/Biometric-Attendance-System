/*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		    :	YMCA YRS
// FileName			    :	AddressBOClass.cs
// Author Name		    :	
// Employee ID		    :	
// Creation Time		:	
// Program Specification Name	:	

//Modification History :
********************************************************************************************************************************
    Author           Date          Description
********************************************************************************************************************************
    Anudeep         24.09.2013      BT-1501:YRS 5.0-1745:Capture Beneficiary addresses
    Anudeep         4.07.2014       BT-1051:YRS 5.0-1618 :Enhancements to Roll In process 
    Manthan Rajguru 2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
********************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
   public sealed  class AddressBOClass
    {
        public static DataSet GetAddressByEntity(string strEntityID,string strEntityCode ) 
        {
            return YMCARET.YmcaDataAccessObject.AddressDAClass.GetAddressByEntity(strEntityID,strEntityCode);
        }

        public static string SaveAddress(DataSet dsAddress)
        {
            return YMCARET.YmcaDataAccessObject.AddressDAClass.SaveAddress(dsAddress);

        }

        public static DataSet GetAddressByID(string strAddressID)
        {
            return YMCARET.YmcaDataAccessObject.AddressDAClass.GetAddressByID(strAddressID);
        }
        public static DataSet GetBeneficiariesAddress(string strPersID, string strSSNO, string strFirstname, string strLastname)
        {
            return YMCARET.YmcaDataAccessObject.AddressDAClass.GetBeneficiariesAddress(strPersID, strSSNO,strFirstname,  strLastname);
        }
        public static DataSet GetAddessForBeneficiariesOfPerson(string strPersID, string strEntityCode)
        {
           return YMCARET.YmcaDataAccessObject.AddressDAClass.GetAddessForBeneficiariesOfPerson(strPersID, strEntityCode);
        }

        public static void SaveAddressOfBeneficiariesByPerson(DataSet dsAddress)
        {
            YMCARET.YmcaDataAccessObject.AddressDAClass.SaveAddressOfBeneficiariesByPerson(dsAddress);
        }

        public static DataSet GetAddressForYMCA(string guiYmcaID) 
        {
            return YMCARET.YmcaDataAccessObject.AddressDAClass.GetAddressForYMCA(guiYmcaID); 
        }

        public static DataSet GetCountryList() 
        {
            return YMCARET.YmcaDataAccessObject.AddressDAClass.GetCountryList(); 
        }

        public static DataSet GetState()
        {
            return YMCARET.YmcaDataAccessObject.AddressDAClass.GetState(); 
        }
       //AA:2013.09.24:BT-1501:Handling the getting reason from address class and getting reasons for beneficiary address change
        public static DataSet GetAddressNotesReasonSource(string parameterNotesSourceReason,string parameterNotesSourceFor)
        {
            return YMCARET.YmcaDataAccessObject.AddressDAClass.GetAddressNotesReasonSource(parameterNotesSourceReason, parameterNotesSourceFor);
        }
        public static void SaveAddressForYMCA(DataSet ds)
        {
            YMCARET.YmcaDataAccessObject.AddressDAClass.SaveAddressForYMCA(ds);
        }
        
       //Start: Anudeep:04.07.2014:BT-1051:YRS 5.0-1618 : Added code to save the rolloverinstitution address
        public static string SaveInstitutionAddress(DataSet ds)
        {
           return YMCARET.YmcaDataAccessObject.AddressDAClass.SaveInstitutionAddress(ds);
        }
        //End: Anudeep:04.07.2014:BT-1051:YRS 5.0-1618 : Added code to save the rolloverinstitution address
       
    }
}
