//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	RollInReminderBOClass.cs
// Author Name		:	Anudeep Adusumilli
// Employee ID		:	56556
// Email			:	
// Contact No		:	
// Creation Time	:	6/05/2014 
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<BO class for rollin reminder screen>
//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace YMCARET.YmcaBusinessObject
{
    public class RollInReminderBOClass
    {
        public RollInReminderBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //To get the open rollin(s)
        public static DataSet GetOpenRollIns()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RollInReminderDAClass.GetOpenRollIns();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        //To insert the follow-up data
        public static void InsertPrintLetters(string strRolloverId)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.RollInReminderDAClass.InsertPrintLetters(strRolloverId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
