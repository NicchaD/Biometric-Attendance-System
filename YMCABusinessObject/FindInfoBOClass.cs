//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	FindInfoBOClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	8/3/2005 10:18:20 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	Business class for FindInfo form
//********************************************************************************************************************
//Modified              Date                Description
//********************************************************************************************************************
// Deven                02-Sep-2010         Added funcion GetPersonDetail to get the Person detail by FundEventID or its Gui ID
//Sanket Vaidya         15-Sep-2011         BT-798 : System should not allow disability retirement for QD and BF fundevents
//Dinesh Kanojia        2013-03-19          Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
//Anudeep Adusumilli    2014-02-03          BT:2292:YRS 5.0-2248 - YRS Pin number 
//Manthan Rajguru       2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Sanjay GS Rawat       2016.06.26          YRS-AT-2484 - Change needed to RMD Utility-employees who terminate when older than 70 1/2 (TrackIT 22190) 
//******************************************************************************************************************************
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// Summary description for FindInfo.
    /// </summary>
    public sealed class FindInfo
    {
        private FindInfo()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// Method to call the mathod of the data access class to fetch the list of persons
        /// </summary>
        /// <param name="parameterSSNo"></param>
        /// <param name="parameterFundNo"></param>
        /// <param name="parameterLastName"></param>
        /// <param name="parameterFirstName"></param>
        /// <returns>dataset</returns>
        // Dinesh Kanojia : 2013-03-19 : Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
        public static DataSet LookUpPersons(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName, string parameterFormName, string parameterCity, string parameterState, string paramEmail, string paramPhone)
       // public static DataSet LookUpPersons(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName, string parameterFormName, string parameterCity, string parameterState)
        {
            try
            {
                //Dinesh Kanojia : 2013-03-19 : Bug ID: 1535:YRS 5.0-1758:Add telepone and email as lookup fields for person maintenance
                return (FindInfoDAClass.LookUpPerson(parameterSSNo, parameterFundNo, parameterLastName, parameterFirstName, parameterFormName, parameterCity, parameterState, paramEmail, paramPhone));
                //return (FindInfoDAClass.LookUpPerson(parameterSSNo, parameterFundNo, parameterLastName, parameterFirstName, parameterFormName, parameterCity, parameterState));
            }
            catch
            {
                throw;
            }
        }

        //Sanket Vaidya :   BT-798 System should not allow disability retirement for QD and BF fundevents
        public static DataSet LookUpPersons(string parameterSSNo, string parameterFundNo, string parameterLastName, string parameterFirstName, string parameterFormName, string parameterCity, string parameterState, string parameterRetirementType)
        {
            try
            {
                return (FindInfoDAClass.LookUpPerson(parameterSSNo, parameterFundNo, parameterLastName, parameterFirstName, parameterFormName, parameterCity, parameterState, parameterRetirementType));
            }
            catch
            {
                throw;
            }
        }

        public static DataTable GetQDRODetails_BYSSNO(string p_string_SSNO)
        {
            try
            {
                return (FindInfoDAClass.GetQDRODetails_BYSSNO(p_string_SSNO));
            }
            catch
            {
                throw;
            }
        }
        // Deven 02-Sep-2010
        public static DataTable GetPersonDetail(string parameterguiFundEventID, string parameterguiPersID)
        {
            try
            {
                return (FindInfoDAClass.GetPersonDetail(parameterguiFundEventID, parameterguiPersID));
            }
            catch
            {
                throw;
            }
        }
        //Start:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To get the PIN value
        public static string GetUserPIN(string strGuiPersId)
        {
            try
            {
                return FindInfoDAClass.GetUserPIN(strGuiPersId);
            }
            catch
            {
                throw;
            }
        }
        //End:AA:2014-02-03 - BT:2292:YRS 5.0-2248 - To get the PIN value

        //START | SR | 2016.06.26 | YRS-AT-2484 - Change needed to RMD Utility-employees who terminate when older than 70 1/2 (TrackIT 22190) 
        public static DataSet GetFundStatusChangedParticipant()
        {
            try
            {
                return FindInfoDAClass.GetFundStatusChangedParticipant();
            }
            catch
            {
                throw;
            }
        }
        //END | SR | 2016.06.26 | YRS-AT-2484 - Change needed to RMD Utility-employees who terminate when older than 70 1/2 (TrackIT 22190) 
    }
}
